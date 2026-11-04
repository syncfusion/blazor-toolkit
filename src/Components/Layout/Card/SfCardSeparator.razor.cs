using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Renders a divider between regions of an <see cref="SfCard"/>.
    /// </summary>
    /// <remarks>
    /// A vertical card exposes <c>aria-orientation="horizontal"</c>.
    /// In a <see cref="CardOrientation.Horizontal"/> card the divider is drawn vertically and exposes <c>aria-orientation="vertical"</c>.
    /// <c>role="separator"</c> requires one of those two values. An omitted orientation fails automated accessibility checks.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfCardContent>Body</SfCardContent>
    /// <SfCardSeparator />
    /// <SfCardFooter>Footer</SfCardFooter>
    /// ]]></code>
    /// </example>
    public partial class SfCardSeparator : SfBaseComponent
    {
        private const string RegionClass = "e-card-separator";

        /// <exclude />
        private string _class = RegionClass;

        /// <exclude />
        private string? _style;

        /// <exclude />
        private string? _orientation;

        /// <exclude />
        private Dictionary<string, object> _attributes = [];

        /// <exclude />
        [CascadingParameter]
        internal SfCard? Card { get; set; }

        /// <summary>
        /// Gets or sets optional content of the divider.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/>. The default value is <see langword="null"/>.</value>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets additional CSS class names for the divider element.
        /// </summary>
        /// <value>One or more space-separated class names. The default value is <see langword="null"/>.</value>
        [Parameter]
        public string? CssClass { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes for the divider element.
        /// </summary>
        /// <value>A dictionary of attribute names and values. User attributes win over computed ones, except <c>class</c> and <c>style</c>, which are merged.</value>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? HtmlAttributes { get; set; }

        /// <exclude />
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            _class = CardAttributes.MergeClass(RegionClass, CssClass, HtmlAttributes);
            _style = CardAttributes.MergeStyle(null, HtmlAttributes);
            _attributes = CardAttributes.Filter(HtmlAttributes);
            // role="separator" requires horizontal or vertical. An omitted value fails axe aria-valid-attr-value.
            _orientation = Card?.Orientation == CardOrientation.Horizontal ? "vertical" : "horizontal";
        }
    }
}
