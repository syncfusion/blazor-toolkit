using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfChipSet<TItem>
    {
        #region Events

        /// <summary>Raised when <see cref="SelectedValue"/> changes.</summary>
        [Parameter]
        public EventCallback<object?> SelectedValueChanged { get; set; }

        /// <summary>Raised when <see cref="SelectedValues"/> changes.</summary>
        [Parameter]
        public EventCallback<IEnumerable<object?>> SelectedValuesChanged { get; set; }

        /// <summary>Raised before a selection change. Cancel keeps the current selection.</summary>
        [Parameter]
        public EventCallback<ChipSelectionChangingEventArgs<TItem>> SelectionChanging { get; set; }

        /// <summary>Raised after a committed selection change.</summary>
        [Parameter]
        public EventCallback<ChipSelectionChangedEventArgs<TItem>> SelectionChanged { get; set; }

        /// <summary>Raised before removal. Cancel keeps the chip.</summary>
        [Parameter]
        public EventCallback<ChipRemovingEventArgs<TItem>> Removing { get; set; }

        /// <summary>Raised after a chip is removed from the rendered set. <see cref="DataSource"/> is not mutated.</summary>
        [Parameter]
        public EventCallback<ChipRemovedEventArgs<TItem>> Removed { get; set; }

        /// <summary>Raised for activation that is not a remove action.</summary>
        [Parameter]
        public EventCallback<ChipClickEventArgs<TItem>> OnChipClick { get; set; }

        /// <summary>Raised when collapse overflow expands or collapses.</summary>
        [Parameter]
        public EventCallback<bool> OverflowExpandedChanged { get; set; }

        private async Task OnProjectedClickAsync(ProjectedChip chip)
        {
            await OnChipClick.InvokeAsync(new ChipClickEventArgs<TItem>
            {
                Item = chip.Item,
                Key = chip.Model.Key,
            }).ConfigureAwait(true);
            if (EffectiveMode != ChipSelectionMode.None && !Disabled && !ReadOnly && !chip.Model.Disabled)
            {
                await CommitSelectionAsync(chip.Item, chip.Model.Value, false).ConfigureAwait(true);
            }
        }

        private async Task OnProjectedRemovingAsync(ProjectedChip chip, ChipRemovingEventArgs args)
        {
            await RequestRemoveAsync(chip.Model.Key, chip.Item, args.Reason).ConfigureAwait(true);
            args.Cancel = true;
        }

        private async ValueTask CommitSelectionAsync(TItem item, object? value, bool clear)
        {
            if (Disabled || ReadOnly || EffectiveMode == ChipSelectionMode.None)
            {
                return;
            }

            IReadOnlyList<object?> current = ChipSelectionStore.Current(EffectiveMode, SelectedValue, SelectedValues);
            IReadOnlyList<object?> proposed = ChipSelectionStore.Propose(EffectiveMode, AllowEmpty, current, value, clear);
            if (ChipSelectionStore.Same(current, proposed))
            {
                return;
            }

            ChipSelectionChangingEventArgs<TItem> changing = new()
            {
                Item = item,
                Value = value,
                CurrentSelectedValues = current,
                ProposedSelectedValues = proposed,
            };
            await SelectionChanging.InvokeAsync(changing).ConfigureAwait(true);
            if (changing.Cancel)
            {
                return;
            }

            if (EffectiveMode == ChipSelectionMode.Single)
            {
                SelectedValue = proposed.Count == 0 ? null : proposed[0];
                await SelectedValueChanged.InvokeAsync(SelectedValue).ConfigureAwait(true);
            }
            else
            {
                SelectedValues = [.. proposed];
                await SelectedValuesChanged.InvokeAsync(SelectedValues).ConfigureAwait(true);
            }

            await SelectionChanged.InvokeAsync(new ChipSelectionChangedEventArgs<TItem>
            {
                Item = item,
                Value = value,
                SelectedValues = proposed,
            }).ConfigureAwait(true);
            NotifyEditContext();
        }

        private async ValueTask RequestRemoveAsync(object key, TItem item, ChipRemoveReason reason)
        {
            if (Disabled || ReadOnly)
            {
                return;
            }

            ProjectedChip? chip = _projected.FirstOrDefault(itemChip => ChipKeyComparer.Instance.Equals(itemChip.Model.Key, key) && !itemChip.Removed);
            if (chip is not null && !chip.Model.Removable)
            {
                return;
            }

            ChipRemovingEventArgs<TItem> removing = new()
            {
                Item = chip is null ? item : chip.Item,
                Text = chip?.Model.Text,
                Key = key,
                Reason = reason,
            };
            await Removing.InvokeAsync(removing).ConfigureAwait(true);
            if (removing.Cancel)
            {
                return;
            }

            if (!_removedKeys.Any(existing => ChipKeyComparer.Instance.Equals(existing, key)))
            {
                _removedKeys.Add(key);
            }

            if (chip?.Model.Value is not null && !_removedKeys.Any(existing => ChipKeyComparer.Instance.Equals(existing, chip.Model.Value)))
            {
                _removedKeys.Add(chip.Model.Value);
            }

            int index = chip is null ? _focusedIndex : _projected.IndexOf(chip);
            Rebuild();
            StateHasChanged();
            int next = ChipFocusCoordinator.Restore(index, _projected.Count, CanFocus);
            _focusedIndex = next;
            _focusTargetId = next < 0 ? RootId : _projected[next].ElementId;
            await Removed.InvokeAsync(new ChipRemovedEventArgs<TItem>
            {
                Item = removing.Item,
                Text = removing.Text,
                Key = key,
                Reason = reason,
            }).ConfigureAwait(true);
        }

        private async Task OnKeyDownAsync(KeyboardEventArgs args)
        {
            int count = _projected.Count;
            int next = args.Key switch
            {
                ArrowRightKey or ArrowDownKey => ChipFocusCoordinator.Move(_focusedIndex, count, EnableRtl && args.Key == ArrowRightKey ? -1 : 1, CanFocus),
                ArrowLeftKey or ArrowUpKey => ChipFocusCoordinator.Move(_focusedIndex, count, EnableRtl && args.Key == ArrowLeftKey ? 1 : -1, CanFocus),
                HomeKey => ChipFocusCoordinator.Edge(count, false, CanFocus),
                EndKey => ChipFocusCoordinator.Edge(count, true, CanFocus),
                _ => _focusedIndex,
            };
            if (next != _focusedIndex && next >= 0)
            {
                _focusedIndex = next;
                ProjectedChip chip = _projected[next];
                _focusTargetId = chip.ElementId;
                if (chip.Chip is not null)
                {
                    await chip.Chip.FocusAsync().ConfigureAwait(true);
                }
            }
        }

        private async Task ToggleOverflowAsync()
        {
            await SetOverflowAsync(!_overflowExpanded).ConfigureAwait(true);
        }

        private async ValueTask SetOverflowAsync(bool expanded)
        {
            if (_overflowExpanded == expanded)
            {
                return;
            }

            _overflowExpanded = expanded;
            ApplyCollapse();
            StateHasChanged();
            await OverflowExpandedChanged.InvokeAsync(expanded).ConfigureAwait(true);
        }

        #endregion
    }
}
