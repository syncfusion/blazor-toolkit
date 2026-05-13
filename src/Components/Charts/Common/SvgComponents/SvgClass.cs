using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Base component providing common SVG element attributes.
    /// </summary>
    public class SvgClass : ComponentBase
    {
        #region Properties
        /// <summary>
        /// Gets or sets the CSS class(es) applied to the SVG element.
        /// </summary>
        /// <value>
        /// A space-separated list of CSS classes. Default: <c>string.Empty</c>.
        /// </value>
        [Parameter]
        public virtual string Class { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the id attribute for the SVG element.
        /// </summary>
        /// <value>
        /// The element id. If not provided, one will be generated during parameter binding.
        /// </value>
        [Parameter]
        public virtual string Id { get; set; } = null!;
        #endregion
    }
}