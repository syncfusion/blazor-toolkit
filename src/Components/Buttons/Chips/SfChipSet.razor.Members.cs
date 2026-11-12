using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>Public parameters for <see cref="SfChipSet{TItem}"/>.</summary>
    public partial class SfChipSet<TItem>
    {
        /// <summary>Declarative chips. Ignored when <see cref="DataSource"/> is set.</summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>Bound items. The set never mutates this collection.</summary>
        [Parameter]
        public IEnumerable<TItem>? DataSource { get; set; }

        /// <summary>Stable key selector. Duplicate keys keep the first occurrence for selection.</summary>
        [Parameter]
        public Func<TItem, object>? ItemKey { get; set; }

        /// <summary>Text selector. Preferred over reflection.</summary>
        [Parameter]
        public Func<TItem, string>? TextSelector { get; set; }

        /// <summary>Value selector.</summary>
        [Parameter]
        public Func<TItem, object?>? ValueSelector { get; set; }

        /// <summary>Disabled selector.</summary>
        [Parameter]
        public Func<TItem, bool>? DisabledSelector { get; set; }

        /// <summary>Removable selector.</summary>
        [Parameter]
        public Func<TItem, bool>? RemovableSelector { get; set; }

        /// <summary>Icon CSS selector.</summary>
        [Parameter]
        public Func<TItem, string?>? IconCssSelector { get; set; }

        /// <summary>Image URL selector.</summary>
        [Parameter]
        public Func<TItem, string?>? ImageUrlSelector { get; set; }

        /// <summary>Image alternate-text selector.</summary>
        [Parameter]
        public Func<TItem, string?>? ImageAltSelector { get; set; }

        /// <summary>Href selector.</summary>
        [Parameter]
        public Func<TItem, string?>? HrefSelector { get; set; }

        /// <summary>CSS class selector.</summary>
        [Parameter]
        public Func<TItem, string?>? CssClassSelector { get; set; }

        /// <summary>Property-name map used when a selector is not set.</summary>
        [Parameter]
        public ChipFields? Fields { get; set; }

        /// <summary>Selection behavior. Default is <see cref="ChipSelectionMode.None"/>.</summary>
        [Parameter]
        public ChipSelectionMode SelectionMode { get; set; } = ChipSelectionMode.None;

        /// <summary>Selected value in single mode.</summary>
        [Parameter]
        public object? SelectedValue { get; set; }

        /// <summary>Selected values in multiple mode.</summary>
        [Parameter]
        public IEnumerable<object?>? SelectedValues { get; set; }

        /// <summary>Allows clearing the last selected value. Default is <c>true</c>.</summary>
        [Parameter]
        public bool AllowEmpty { get; set; } = true;

        /// <summary>Default chip type for projected items.</summary>
        [Parameter]
        public ChipType ChipType { get; set; } = ChipType.Informational;

        /// <summary>Default variant.</summary>
        [Parameter]
        public ChipVariant Variant { get; set; } = ChipVariant.Filled;

        /// <summary>Default size.</summary>
        [Parameter]
        public ChipSize Size { get; set; } = ChipSize.Medium;

        /// <summary>Default shape.</summary>
        [Parameter]
        public ChipShape Shape { get; set; } = ChipShape.Rounded;

        /// <summary>Overflow behavior. Default is <see cref="ChipOverflowMode.Wrap"/>.</summary>
        [Parameter]
        public ChipOverflowMode OverflowMode { get; set; } = ChipOverflowMode.Wrap;

        /// <summary>
        /// Chips kept visible in collapse mode before script measurement.
        /// After the first measurement, chip.js may overwrite this value so resize can change the fold.
        /// </summary>
        [Parameter]
        public int? CollapsedVisibleCount { get; set; }

        /// <summary>Disables the set.</summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>Blocks selection and removal without hiding values.</summary>
        [Parameter]
        public bool ReadOnly { get; set; }

        /// <summary>Default removable flag for items that do not specify their own.</summary>
        [Parameter]
        public bool Removable { get; set; }

        /// <summary>Loading state. Renders <see cref="LoadingTemplate"/> only.</summary>
        [Parameter]
        public bool IsLoading { get; set; }

        /// <summary>Error state. Renders <see cref="ErrorTemplate"/> only.</summary>
        [Parameter]
        public bool HasError { get; set; }

        /// <summary>Item body template.</summary>
        [Parameter]
        public RenderFragment<TItem>? ChipTemplate { get; set; }

        /// <summary>Leading slot template.</summary>
        [Parameter]
        public RenderFragment<TItem>? LeadingTemplate { get; set; }

        /// <summary>Trailing slot template.</summary>
        [Parameter]
        public RenderFragment<TItem>? TrailingTemplate { get; set; }

        /// <summary>Remove-button content template. The button element remains.</summary>
        [Parameter]
        public RenderFragment<TItem>? RemoveTemplate { get; set; }

        /// <summary>Rendered when there are no items and the set is not loading or in error.</summary>
        [Parameter]
        public RenderFragment? EmptyTemplate { get; set; }

        /// <summary>Rendered instead of chips while <see cref="IsLoading"/> is true.</summary>
        [Parameter]
        public RenderFragment? LoadingTemplate { get; set; }

        /// <summary>Rendered instead of chips while <see cref="HasError"/> is true.</summary>
        [Parameter]
        public RenderFragment? ErrorTemplate { get; set; }

        /// <summary>Expand control content. Receives the hidden count.</summary>
        [Parameter]
        public RenderFragment<int>? OverflowExpandTemplate { get; set; }

        /// <summary>Additional CSS classes.</summary>
        [Parameter]
        public string? CssClass { get; set; }

        /// <summary>Inline style.</summary>
        [Parameter]
        public string? Style { get; set; }

        /// <summary>Element id.</summary>
        [Parameter]
        public string? Id { get; set; }

        /// <summary>Accessible name.</summary>
        [Parameter]
        public string? AriaLabel { get; set; }

        /// <summary>Id of the labelling element.</summary>
        [Parameter]
        public string? AriaLabelledBy { get; set; }

        /// <summary>Right-to-left layout.</summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>HTML attributes applied to the set root.</summary>
        [Parameter]
        public Dictionary<string, object>? HtmlAttributes { get; set; }

        /// <summary>Optional field expression used to notify <see cref="EditContext"/>.</summary>
        [Parameter]
        public Expression<Func<TItem>>? ValueExpression { get; set; }

        [CascadingParameter]
        private EditContext? EditContext { get; set; }

        [Inject]
        private IStringLocalizer? Localizer { get; set; }
    }
}
