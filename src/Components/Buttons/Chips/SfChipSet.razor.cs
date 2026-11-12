using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// A set of chips bound to <typeparamref name="TItem"/> or composed from <see cref="SfChip"/> children.
    /// </summary>
    /// <typeparam name="TItem">Data item type. Use <see cref="string"/> for declarative-only sets.</typeparam>
    /// <remarks>
    /// The parent owns <see cref="SfChipSet{TItem}.DataSource"/>. Removal hides a chip; it does not mutate the source.
    /// </remarks>
    public partial class SfChipSet<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TItem> : SfBaseComponent, IChipSetRegistration
    {
        private readonly List<ProjectedChip> _projected = [];
        private readonly List<object> _removedKeys = [];
        private readonly List<DeclarativeChip> _declarative = [];
        private IJSObjectReference? _chipModule;
        private ElementReference _rootReference;
        private int _focusedIndex;
        private int _hiddenCount;
        private bool _overflowExpanded;
        private bool _choiceWarned;
        private bool _jsReady;
        private string? _focusTargetId;

        private string RootId => string.IsNullOrEmpty(Id) ? $"chipset-{GetHashCode():x}" : Id;

        private bool HasItems => DataSource?.Any() == true || _declarative.Count > 0 || ChildContent is not null;

        private ChipSelectionMode EffectiveMode
        {
            get
            {
                if (ChipType == ChipType.Choice && SelectionMode == ChipSelectionMode.Multiple)
                {
                    if (!_choiceWarned)
                    {
                        _choiceWarned = true;
                        Debug.WriteLine("SfChipSet Choice with ChipSelectionMode.Multiple behaves as Single.");
                    }

                    return ChipSelectionMode.Single;
                }

                return SelectionMode;
            }
        }

        private IEnumerable<ProjectedChip> VisibleChips => _projected.Where(chip => !chip.Removed);

        /// <summary>Id the focus coordinator would move to after the last removal. bUnit asserts this; Playwright asserts document.activeElement.</summary>
        public string? FocusTargetId => _focusTargetId;

        /// <summary>Selects a value through the cancelable selection pipeline.</summary>
        public ValueTask SelectAsync(object? value)
        {
            return CommitSelectionAsync(default!, value, false);
        }

        /// <summary>Selects an item through the cancelable selection pipeline.</summary>
        public ValueTask SelectItemAsync(TItem item)
        {
            object? value = ValueSelector is not null ? ValueSelector(item) : item;
            return CommitSelectionAsync(item, value, false);
        }

        /// <summary>Clears selection when <see cref="SfChipSet{TItem}.AllowEmpty"/> allows it.</summary>
        public ValueTask ClearSelectionAsync()
        {
            return CommitSelectionAsync(default!, null, true);
        }

        /// <summary>Moves focus to the set root.</summary>
        public ValueTask FocusAsync()
        {
            return _rootReference.FocusAsync();
        }

        /// <summary>Moves focus to the chip with the given key when it is rendered.</summary>
        public ValueTask FocusChipAsync(object key)
        {
            ProjectedChip? chip = _projected.FirstOrDefault(item => ChipKeyComparer.Instance.Equals(item.Model.Key, key) && !item.Removed);
            _focusedIndex = chip is null ? _focusedIndex : _projected.IndexOf(chip);
            _focusTargetId = chip?.ElementId;
            return chip?.Chip is null ? ValueTask.CompletedTask : chip.Chip.FocusAsync();
        }

        /// <summary>Removes a chip by key. Does not mutate <see cref="SfChipSet{TItem}.DataSource"/>.</summary>
        public ValueTask RemoveAsync(object key)
        {
            return RequestRemoveAsync(key, default!, ChipRemoveReason.Programmatic);
        }

        /// <summary>Removes a chip by item.</summary>
        public ValueTask RemoveItemAsync(TItem item)
        {
            object key = ItemKey is not null ? ItemKey(item) : item!;
            return RequestRemoveAsync(key, item, ChipRemoveReason.Programmatic);
        }

        /// <summary>Expands collapsed overflow and raises <see cref="SfChipSet{TItem}.OverflowExpandedChanged"/> only on change.</summary>
        public ValueTask ExpandOverflowAsync()
        {
            return SetOverflowAsync(true);
        }

        /// <summary>Collapses overflow and raises <see cref="SfChipSet{TItem}.OverflowExpandedChanged"/> only on change.</summary>
        public ValueTask CollapseOverflowAsync()
        {
            return SetOverflowAsync(false);
        }

        void IChipSetRegistration.Register(DeclarativeChip chip)
        {
            Register(chip);
        }

        void IChipSetRegistration.Unregister(string id)
        {
            Unregister(id);
        }

        async Task IChipSetRegistration.ActivateAsync(object? key, MouseEventArgs args)
        {
            DeclarativeChip? child = FindChild(key);
            await OnChipClick.InvokeAsync(new ChipClickEventArgs<TItem>
            {
                Key = child?.Key ?? key,
            }).ConfigureAwait(true);
            if (EffectiveMode != ChipSelectionMode.None && !Disabled && !ReadOnly && child?.Disabled != true)
            {
                await CommitSelectionAsync(default!, child?.Value ?? key, false).ConfigureAwait(true);
            }
        }

        Task IChipSetRegistration.RemoveChildAsync(object? key, ChipRemoveReason reason)
        {
            DeclarativeChip? child = FindChild(key);
            return RequestRemoveAsync(child?.Key ?? key ?? string.Empty, default!, reason).AsTask();
        }

        private DeclarativeChip? FindChild(object? key)
        {
            return _declarative.FirstOrDefault(item =>
                ChipKeyComparer.Instance.Equals(item.Key, key)
                || ChipKeyComparer.Instance.Equals(item.Value, key)
                || ChipKeyComparer.Instance.Equals(item.Id, key));
        }

        internal void Register(DeclarativeChip chip)
        {
            DeclarativeChip? existing = _declarative.FirstOrDefault(item => item.Id == chip.Id);
            if (existing is null)
            {
                _declarative.Add(chip);
                return;
            }

            existing.Key = chip.Key;
            existing.Value = chip.Value;
            existing.Text = chip.Text;
            existing.Disabled = chip.Disabled;
            existing.Removable = chip.Removable;
            existing.Chip = chip.Chip;
        }

        internal void Unregister(string id)
        {
            _ = _declarative.RemoveAll(item => item.Id == id);
        }

        private RenderFragment RenderProjected(ProjectedChip chip)
        {
            return builder =>
            {
                int seq = 0;
                builder.OpenComponent<SfChip>(seq++);
                builder.AddAttribute(seq++, "Id", chip.ElementId);
                builder.AddAttribute(seq++, "Text", chip.Model.Text);
                builder.AddAttribute(seq++, "Value", chip.Model.Value);
                builder.AddAttribute(seq++, "ChipType", ChipType);
                builder.AddAttribute(seq++, "TabIndex", chip.TabIndex);
                builder.AddAttribute(seq++, "Variant", Variant);
                builder.AddAttribute(seq++, "Size", Size);
                builder.AddAttribute(seq++, "Shape", Shape);
                builder.AddAttribute(seq++, "IconCss", chip.Model.IconCss);
                builder.AddAttribute(seq++, "ImageUrl", chip.Model.ImageUrl);
                builder.AddAttribute(seq++, "ImageAlt", chip.Model.ImageAlt);
                builder.AddAttribute(seq++, "Href", chip.Model.Href);
                builder.AddAttribute(seq++, "CssClass", chip.Model.CssClass);
                builder.AddAttribute(seq++, "Selected", IsSelected(chip.Model.Value));
                builder.AddAttribute(seq++, "Disabled", Disabled || chip.Model.Disabled);
                builder.AddAttribute(seq++, "ReadOnly", ReadOnly);
                builder.AddAttribute(seq++, "Removable", chip.Model.Removable);
                builder.AddAttribute(seq++, "EnableRtl", EnableRtl);
                if (ChipTemplate is not null)
                {
                    builder.AddAttribute(seq++, "ChildContent", ChipTemplate(chip.Item));
                }
                if (LeadingTemplate is not null)
                {
                    builder.AddAttribute(seq++, "LeadingTemplate", LeadingTemplate(chip.Item));
                }
                if (TrailingTemplate is not null)
                {
                    builder.AddAttribute(seq++, "TrailingTemplate", TrailingTemplate(chip.Item));
                }
                if (RemoveTemplate is not null)
                {
                    builder.AddAttribute(seq++, "RemoveTemplate", RemoveTemplate(chip.Item));
                }
                builder.AddAttribute(seq++, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, _ => OnProjectedClickAsync(chip)));
                builder.AddAttribute(seq++, "Removing", EventCallback.Factory.Create<ChipRemovingEventArgs>(this, args => OnProjectedRemovingAsync(chip, args)));
                builder.AddComponentReferenceCapture(seq++, instance => chip.Chip = (SfChip)instance);
                builder.CloseComponent();
            };
        }

        private bool IsSelected(object? value)
        {
            return ChipSelectionStore.Current(EffectiveMode, SelectedValue, SelectedValues)
                .Any(item => ChipKeyComparer.Instance.Equals(item, value));
        }

        private bool CanFocus(int index)
        {
            return index >= 0 && index < _projected.Count && !_projected[index].Removed && !_projected[index].Model.Disabled && !Disabled;
        }

        private void NotifyEditContext()
        {
            if (EditContext is null || ValueExpression is null || ReadOnly || Disabled)
            {
                return;
            }

            EditContext.NotifyFieldChanged(FieldIdentifier.Create(ValueExpression));
        }

        private void Rebuild()
        {
            Dictionary<object, SfChip> chips = _projected
                .Where(chip => chip.Chip is not null)
                .GroupBy(chip => chip.Model.Key, ChipKeyComparer.Instance)
                .ToDictionary(group => group.Key, group => group.First().Chip!, ChipKeyComparer.Instance);
            _projected.Clear();
            if (DataSource is null)
            {
                int childIndex = 0;
                foreach (DeclarativeChip child in _declarative)
                {
                    _projected.Add(new ProjectedChip
                    {
                        Model = new ChipModel
                        {
                            Key = child.Key ?? child.Id,
                            Value = child.Value ?? child.Text,
                            Text = child.Text ?? string.Empty,
                            Disabled = child.Disabled,
                            Removable = child.Removable,
                        },
                        ElementId = child.Id,
                        Chip = child.Chip,
                        Removed = _removedKeys.Any(existing => ChipKeyComparer.Instance.Equals(existing, child.Key ?? child.Id) || ChipKeyComparer.Instance.Equals(existing, child.Value)),
                        TabIndex = childIndex == _focusedIndex ? 0 : -1,
                    });
                    childIndex++;
                }

                ApplyCollapse();
                return;
            }

            IReadOnlyList<ChipModel> models = ChipFieldResolver.Resolve(
                DataSource,
                ItemKey,
                TextSelector,
                ValueSelector,
                DisabledSelector,
                RemovableSelector,
                IconCssSelector,
                ImageUrlSelector,
                ImageAltSelector,
                HrefSelector,
                CssClassSelector,
                Fields,
                Removable);
            int index = 0;
            foreach (ChipModel model in models)
            {
                _projected.Add(new ProjectedChip
                {
                    Item = DataSource.ElementAt(index),
                    Model = model,
                    ElementId = $"{RootId}-chip-{index}",
                    Chip = chips.TryGetValue(model.Key, out SfChip? existing) ? existing : null,
                    Removed = _removedKeys.Any(existingKey => ChipKeyComparer.Instance.Equals(existingKey, model.Key) || ChipKeyComparer.Instance.Equals(existingKey, model.Value)),
                    TabIndex = index == _focusedIndex ? 0 : -1,
                });
                index++;
            }

            ApplyCollapse();
        }

        private void ApplyCollapse()
        {
            IEnumerable<ProjectedChip> visible = [.. _projected.Where(chip => !chip.Removed)];
            foreach (ProjectedChip chip in _projected)
            {
                chip.Collapsed = false;
            }

            if (OverflowMode != ChipOverflowMode.Collapse || _overflowExpanded)
            {
                _hiddenCount = 0;
                return;
            }

            int keep = CollapsedVisibleCount ?? visible.Count();
            int shown = 0;
            foreach (ProjectedChip chip in visible)
            {
                bool pin = IsSelected(chip.Model.Value);
                if (!pin && shown >= keep)
                {
                    chip.Collapsed = true;
                }
                else
                {
                    shown++;
                }
            }

            _hiddenCount = visible.Count(chip => chip.Collapsed);
        }

        private Dictionary<string, object> RootAttributes()
        {
            Dictionary<string, object> attributes = new(StringComparer.OrdinalIgnoreCase);
            if (HtmlAttributes is not null)
            {
                foreach (KeyValuePair<string, object> pair in HtmlAttributes)
                {
                    attributes[pair.Key] = pair.Value;
                }
            }

            string overflowClass = OverflowMode switch
            {
                ChipOverflowMode.Wrap => WrapClass,
                ChipOverflowMode.Scroll => ScrollClass,
                ChipOverflowMode.Collapse => CollapseClass,
                _ => WrapClass,
            };
            attributes[IdAttribute] = RootId;
            attributes[ClassAttribute] = string.Join(' ', new[] { RootClass, overflowClass, EnableRtl ? RtlClass : string.Empty, CssClass }.Where(token => !string.IsNullOrWhiteSpace(token)));
            attributes[TestIdAttribute] = ChipSetRootTestId;
            attributes[SelectionAttribute] = EffectiveMode.ToString();
            attributes[TabIndexAttribute] = NegativeTabIndex;
            if (!string.IsNullOrEmpty(Style))
            {
                attributes[StyleAttribute] = Style;
            }

            if (!string.IsNullOrEmpty(AriaLabel))
            {
                attributes[AriaLabelAttribute] = AriaLabel;
            }

            if (!string.IsNullOrEmpty(AriaLabelledBy))
            {
                attributes[AriaLabelledByAttribute] = AriaLabelledBy;
            }

            if (EnableRtl || SyncfusionService?._options.EnableRtl == true)
            {
                attributes[DirAttribute] = RtlDirection;
            }

            if (Disabled)
            {
                attributes[AriaDisabledAttribute] = TrueValue;
            }

            if (ReadOnly)
            {
                attributes[AriaReadonlyAttribute] = TrueValue;
            }

            return attributes;
        }

        private sealed class ProjectedChip
        {
            internal TItem Item { get; init; } = default!;

            internal ChipModel Model { get; init; } = new();

            internal string ElementId { get; init; } = string.Empty;

            internal bool Removed { get; set; }

            internal bool Collapsed { get; set; }

            internal int TabIndex { get; set; } = -1;

            internal SfChip? Chip { get; set; }
        }
    }

    /// <summary>Non-generic registration surface cascaded to _declarative <see cref="SfChip"/> children.</summary>
    internal interface IChipSetRegistration
    {
        void Register(DeclarativeChip chip);

        void Unregister(string id);

        Task ActivateAsync(object? key, MouseEventArgs args);

        Task RemoveChildAsync(object? key, ChipRemoveReason reason);
    }

    /// <summary>Declarative child registration handle.</summary>
    internal sealed class DeclarativeChip
    {
        internal string Id { get; set; } = string.Empty;

        internal object? Key { get; set; }

        internal object? Value { get; set; }

        internal string? Text { get; set; }

        internal bool Disabled { get; set; }

        internal bool Removable { get; set; }

        internal SfChip? Chip { get; set; }
    }
}
