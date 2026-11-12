using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Diagnostics;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// A single chip rendered as a native button, link, or span. Does not wrap <c>SfButton</c>.
    /// </summary>
    /// <remarks>
    /// Navigation without <see cref="SfChip.Href"/> renders as <c>button type="button"</c> and writes a debug diagnostic.
    /// Click, selection, and removal stay in C#.
    /// </remarks>
    public partial class SfChip : SfBaseComponent
    {
        private bool _removed;
        private bool _navigationWarned;
        private bool _registered;
        private ElementReference _rootReference;
        private ElementReference _removeReference;

        private string RootId => string.IsNullOrEmpty(Id) ? $"chip-{GetHashCode():x}" : Id;

        private bool Interactive => ChipType is ChipType.Action or ChipType.Assist or ChipType.Filter or ChipType.Choice or ChipType.Suggestion or ChipType.Navigation;

        private bool IsLink => ChipType == ChipType.Navigation && !string.IsNullOrEmpty(Href) && !_removed;

        private string RootTag => _removed ? SpanTag : IsLink ? AnchorTag : Interactive ? ButtonTag : SpanTag;

        private bool ShowRemove => Removable && !_removed;

        private bool HasAuthorLeading => LeadingTemplate is not null || !string.IsNullOrEmpty(ImageUrl) || !string.IsNullOrEmpty(IconCss);

        private bool ShowSelectionCheck => Selected && ChipType is ChipType.Filter or ChipType.Choice && !HasAuthorLeading;

        private bool ShowLeading => HasAuthorLeading || ShowSelectionCheck;

        private bool ActivationBlocked => Disabled || ReadOnly || IsLoading || _removed;

        private bool RemoveDisabled => Disabled || ReadOnly;

        private string VisibleName => !string.IsNullOrEmpty(AriaLabel) ? AriaLabel : !string.IsNullOrEmpty(Text) ? Text : string.Empty;

        private string RemoveLabel => ChipResources.Format(Localizer, ChipResources.RemoveKey, "Remove {0}", VisibleName);

        private string Css => string.Join(
            ' ',
            new[]
            {
                RootClass,
                TypeClass(ChipType),
                Variant == ChipVariant.Outlined ? OutlinedClass : FilledClass,
                Size switch
                {
                    ChipSize.Small => SmallClass,
                    ChipSize.Medium => MediumClass,
                    ChipSize.Large => LargeClass,
                    _ => MediumClass,
                },
                Shape == ChipShape.Pill ? PillClass : RoundedClass,
                Selected ? SelectedClass : null,
                Disabled ? DisabledClass : null,
                ReadOnly ? ReadOnlyClass : null,
                Removable ? RemovableClass : null,
                HasError ? ErrorClass : null,
                IsLoading ? LoadingClass : null,
                CssClass,
                EnableRtl || (Owner is null && SyncfusionService?._options.EnableRtl == true) ? RtlClass : null,
            }.Where(token => !string.IsNullOrWhiteSpace(token)));

        /// <summary>
        /// Maps a <see cref="ChipType"/> to its lowercase CSS class token.
        /// </summary>
        /// <remarks>
        /// An explicit map keeps the emitted class name culture-invariant and avoids the
        /// per-render allocation of <c>ToString()</c> plus <c>ToLowerInvariant()</c>.
        /// </remarks>
        private static string TypeClass(ChipType chipType)
        {
            return chipType switch
            {
                ChipType.Informational => InformationalClass,
                ChipType.Status => StatusClass,
                ChipType.Action => ActionClass,
                ChipType.Assist => AssistClass,
                ChipType.Filter => FilterClass,
                ChipType.Choice => ChoiceClass,
                ChipType.Input => InputClass,
                ChipType.Suggestion => SuggestionClass,
                ChipType.Navigation => NavigationClass,
                ChipType.Custom => CustomClass,
                _ => TypeClassPrefix + chipType.ToString(),
            };
        }

        /// <summary>Moves focus to the chip root.</summary>
        public ValueTask FocusAsync()
        {
            return _rootReference.FocusAsync();
        }

        /// <summary>Moves focus to the remove control when it is rendered.</summary>
        public ValueTask FocusRemoveAsync()
        {
            return ShowRemove ? _removeReference.FocusAsync() : ValueTask.CompletedTask;
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

            attributes[IdAttribute] = RootId;
            attributes[ClassAttribute] = string.Join(' ', new[] { RootClass, Css }.Where(token => !string.IsNullOrWhiteSpace(token)).Distinct(StringComparer.Ordinal));
            attributes[TestIdAttribute] = ChipRootTestId;
            if (TabIndex is not null || !attributes.ContainsKey(TabIndexAttribute))
            {
                attributes[TabIndexAttribute] = TabIndex ?? 0;
            }
            attributes[ChipTypeAttribute] = ChipType.ToString();
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
                attributes[AriaLabelledByAttribute] = AriaLabelledBy!;
            }

            if (!string.IsNullOrEmpty(AriaDescribedBy))
            {
                attributes[AriaDescribedByAttribute] = AriaDescribedBy!;
            }

            if (ChipType is ChipType.Filter or ChipType.Choice)
            {
                attributes[AriaPressedAttribute] = Selected ? TrueValue : FalseValue;
            }

            if (RootTag == ButtonTag)
            {
                attributes[TypeAttribute] = ButtonType;
                if (Disabled)
                {
                    attributes[DisabledAttribute] = true;
                }
            }
            else if (IsLink)
            {
                attributes[HrefAttribute] = Href!;
                if (!string.IsNullOrEmpty(Target))
                {
                    attributes[TargetAttribute] = Target!;
                }

                if (!string.IsNullOrEmpty(Rel))
                {
                    attributes[RelAttribute] = Rel!;
                }

                if (Disabled)
                {
                    attributes[AriaDisabledAttribute] = TrueValue;
                    attributes[TabIndexAttribute] = NegativeTabIndex;
                }
            }
            else if (Disabled)
            {
                attributes[AriaDisabledAttribute] = TrueValue;
            }

            if (ReadOnly)
            {
                attributes[AriaReadonlyAttribute] = TrueValue;
            }

            if (EnableRtl || (Owner is null && SyncfusionService?._options.EnableRtl == true))
            {
                attributes[DirAttribute] = RtlDirection;
            }

            return attributes;
        }

        private void WarnNavigation()
        {
            if (ChipType == ChipType.Navigation && string.IsNullOrEmpty(Href) && !_navigationWarned)
            {
                _navigationWarned = true;
                Debug.WriteLine("SfChip navigation without Href renders as button type=button.");
            }
        }
    }
}
