using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Renders the header region of an <see cref="SfCard"/>, typically holding a <see cref="SfCardTitle"/> and <see cref="SfCardSubTitle"/>.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// <SfCard>
    ///     <SfCardHeader>header</SfCardHeader>
    /// </SfCard>
    /// ]]></code>
    /// </example>
    public partial class SfCardHeader : SfBaseComponent
    {
        private const string RegionClass = "e-card-header";

        /// <exclude />
        private string _class = RegionClass;

        /// <exclude />
        private string? _style;

        /// <exclude />
        private Dictionary<string, object> _attributes = [];

        /// <summary>
        /// Gets or sets the content of the header region.
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
            _style = CardAttributes.MergeStyle(null, HtmlAttributes);
            _attributes = CardAttributes.Filter(HtmlAttributes);
        }
    }
}
