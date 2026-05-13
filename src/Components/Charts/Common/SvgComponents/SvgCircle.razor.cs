using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents an SVG circle component for data visualization.
    /// </summary>
    /// <remarks>
    /// This component renders an SVG circle element with customizable properties for position, size, stroke, fill, and visibility. It is typically used within chart and diagram visualizations.
    /// </remarks>
    public partial class SvgCircle
    {
        #region Fields
        CultureInfo _culture { get; set; } = CultureInfo.InvariantCulture;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the SVG circle element.
        /// </summary>
        /// <value>A unique string identifier. Default: <c>null</c>.</value>
        [Parameter]
        public override string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the x-coordinate of the circle's center.
        /// </summary>
        /// <value>The x-coordinate as a string representation. Default: <c>null</c>.</value>
        [Parameter]
        public string Cx { get; set; } = null!;

        /// <summary>
        /// Gets or sets the y-coordinate of the circle's center.
        /// </summary>
        /// <value>The y-coordinate as a string representation. Default: <c>null</c>.</value>
        [Parameter]
        public string Cy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the radius of the circle.
        /// </summary>
        /// <value>The radius as a string representation. Default: <c>null</c>.</value>
        [Parameter]
        public string R { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke color of the circle.
        /// </summary>
        /// <value>A CSS color value or <c>null</c> for no stroke. Default: <c>null</c>.</value>
        [Parameter]
        public string Stroke { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke width of the circle outline.
        /// </summary>
        /// <value>The width in units. Default: <c>1</c>.</value>
        [Parameter]
        public double StrokeWidth { get; set; } = 1;

        /// <summary>
        /// Gets or sets the stroke dash array pattern for the circle outline.
        /// </summary>
        /// <value>A comma-separated list of dash lengths, or <c>null</c> for solid stroke. Default: <c>null</c>.</value>
        [Parameter]
        public string StrokeDashArray { get; set; } = null!;

        /// <summary>
        /// Gets or sets the fill color of the circle.
        /// </summary>
        /// <value>A CSS color value. Default: <c>"none"</c>.</value>
        [Parameter]
        public string Fill { get; set; } = "none";

        /// <summary>
        /// Gets or sets the opacity of the circle.
        /// </summary>
        /// <value>A value between 0 (transparent) and 1 (opaque). Default: <c>1</c>.</value>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the visibility of the circle.
        /// </summary>
        /// <value>A CSS visibility value (e.g., "visible", "hidden"), or an empty string for default. Default: <c>string.Empty</c>.</value>
        [Parameter]
        public string Visibility { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility text describing the circle.
        /// </summary>
        /// <remarks>
        /// Used for screen readers and accessibility compliance. Should describe the purpose or meaning of the circle element.
        /// </remarks>
        /// <value>The descriptive text for accessibility. Default: <c>string.Empty</c>.</value>
        [Parameter]
        public string AccessibilityText { get; set; } = string.Empty;
        #endregion
    }
}