using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Represents a visual divider between toolbar items. It renders with <c>role="separator"</c> and is skipped by keyboard navigation.
    /// </summary>
    /// <remarks>
    /// <strong>Security:</strong> <see cref="CssClass"/> is a trust-boundary input. Do not fill it from untrusted data.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfToolbar AriaLabel="Edit">
    ///     <ToolbarItem Text="Cut" />
    ///     <ToolbarSeparator />
    ///     <ToolbarItem Text="Paste" />
    /// </SfToolbar>
    /// ]]></code>
    /// </example>
    public partial class ToolbarSeparator : ComponentBase
    {
        private const string RootClass = "e-toolbar-separator";

        private string _cssClass = RootClass;
        private string _ariaOrientation = "vertical";

        /// <exclude />
        [CascadingParameter]
        internal IToolbarHost? Host { get; set; }

        /// <summary>
        /// Gets or sets whether the separator is rendered.
        /// </summary>
        /// <value><see langword="true"/> to render the separator; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets additional CSS classes applied to the separator.
        /// </summary>
        /// <value>A space-separated list of CSS class names. The default is <see langword="null"/>.</value>
        [Parameter]
        public string? CssClass { get; set; }

        /// <exclude />
        protected override void OnParametersSet()
        {
            _cssClass = string.IsNullOrWhiteSpace(CssClass) ? RootClass : RootClass + " " + CssClass;
            // A separator between items of a horizontal toolbar is a vertical line, and the other way round.
            _ariaOrientation = Host?.Orientation == ToolbarOrientation.Vertical ? "horizontal" : "vertical";
        }
    }
}
