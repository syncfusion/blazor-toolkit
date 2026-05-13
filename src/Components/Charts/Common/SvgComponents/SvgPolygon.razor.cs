using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents an SVG polygon element component for rendering polygon shapes within data visualizations.
    /// </summary>
    /// <remarks>
    /// The <see cref="SvgPolygon"/> component inherits from <see cref="SvgClass"/> and provides a convenient
    /// way to render SVG polygon elements with customizable properties such as points, fill color, and identifier.
    /// </remarks>
    public partial class SvgPolygon
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the polygon element.
        /// </summary>
        /// <value>
        /// A string representing the SVG element's ID attribute. Required. Default: <see langword="null"/>.
        /// </value>
        [Parameter]
        public override string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the points that define the polygon shape.
        /// </summary>
        /// <value>
        /// A comma-separated string of x,y coordinate pairs (e.g., "0,0 100,0 100,100 0,100").
        /// Required. Default: <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// The points must be formatted as SVG polygon points attribute. Each pair represents a vertex.
        /// </remarks>
        [Parameter]
        public string Points { get; set; } = null!;

        /// <summary>
        /// Gets or sets the fill color of the polygon.
        /// </summary>
        /// <value>
        /// A valid SVG color value (hex, rgb, or color name). Default: <see langword="null"/>.
        /// </value>
        [Parameter]
        public string Fill { get; set; } = null!;

        #endregion
    }
}