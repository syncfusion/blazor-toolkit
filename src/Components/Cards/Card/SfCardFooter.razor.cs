using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Renders the footer region of an <see cref="SfCard"/>.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// <SfCard>
    ///     <SfCardFooter>footer</SfCardFooter>
    /// </SfCard>
    /// ]]></code>
    /// </example>
    public partial class SfCardFooter : SfBaseComponent
    {
        private const string RegionClass = "e-card-footer";

        /// <exclude />
        private string _class = RegionClass;

        /// <exclude />
        private string? _style;

        /// <exclude />
        private Dictionary<string, object> _attributes = [];

        /// <exclude />
        [CascadingParameter]
        internal SfCard? Card { get; set; }

        /// <summary>
        /// Gets or sets the content of the footer region.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/>. The default value is <see langword="null"/>.</value>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets additional CSS class names for the region element.
        /// </summary>
        /// <value>One or more space-separated class names. The default value is <see langword="null"/>.</value>
        [Parameter]
        public string? CssClass { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes for the region element.
        /// </summary>
        /// <value>A dictionary of attribute names and values. User attributes win over computed ones, except <c>class</c> and <c>style</c>, which are merged.</value>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? HtmlAttributes { get; set; }

        /// <exclude />
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            _class = CardAttributes.MergeClass(RegionClass, CssClass, HtmlAttributes);
            string? padding = Card is null || Card.ContentPadding ? "padding:var(--e-card-padding, 16px)" : null;
            _style = CardAttributes.MergeStyle(padding, HtmlAttributes);
            _attributes = CardAttributes.Filter(HtmlAttributes);
        }
    }
}
