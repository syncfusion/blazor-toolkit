using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Renders the title of an <see cref="SfCard"/> header.
    /// </summary>
    /// <remarks>
    /// The title renders a neutral element so the page controls its heading level. Place a real heading
    /// (for example <c>&lt;h3&gt;</c>) inside it when the card title should appear in the document outline.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfCardHeader>
    ///     <SfCardTitle><h3>Trail Runner</h3></SfCardTitle>
    /// </SfCardHeader>
    /// ]]></code>
    /// </example>
    public partial class SfCardTitle : SfBaseComponent
    {
        private const string RegionClass = "e-card-title";

        /// <exclude />
        private string _class = RegionClass;

        /// <exclude />
        private string? _style;

        /// <exclude />
        private Dictionary<string, object> _attributes = [];

        /// <summary>
        /// Gets or sets the content of the title.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/>. The default value is <see langword="null"/>.</value>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets additional CSS class names for the title element.
        /// </summary>
        /// <value>One or more space-separated class names. The default value is <see langword="null"/>.</value>
        [Parameter]
        public string? CssClass { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes for the title element.
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
