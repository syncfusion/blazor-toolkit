using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Represents a flexible space that expands to push the items after it toward the far edge.
    /// It is not interactive and is skipped by keyboard navigation.
    /// </summary>
    /// <remarks>
    /// <strong>Security:</strong> <see cref="CssClass"/> is a trust-boundary input. Do not fill it from untrusted data.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfToolbar AriaLabel="Main">
    ///     <ToolbarItem Text="New" />
    ///     <ToolbarSpacer />
    ///     <ToolbarItem Text="Help" />
    /// </SfToolbar>
    /// ]]></code>
    /// </example>
    public partial class ToolbarSpacer : ComponentBase
    {
        private const string RootClass = "e-toolbar-spacer";

        private string _cssClass = RootClass;

        /// <summary>
        /// Gets or sets additional CSS classes applied to the spacer.
        /// </summary>
        /// <value>A space-separated list of CSS class names. The default is <see langword="null"/>.</value>
        [Parameter]
        public string? CssClass { get; set; }

        /// <exclude />
        protected override void OnParametersSet()
        {
            _cssClass = string.IsNullOrWhiteSpace(CssClass) ? RootClass : RootClass + " " + CssClass;
        }
    }
}
