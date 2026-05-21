using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents a reusable SVG rectangle component for data visualization.
    /// </summary>
    /// <remarks>
    /// The <see cref="SvgRect"/> component renders an SVG <c>&lt;rect&gt;</c> element with full support for positioning, styling, transformations, and accessibility attributes. It is designed for use in chart and data visualization contexts where SVG-based rendering is required.
    /// </remarks>
    public partial class SvgRect
    {
        #region Fields
        private CultureInfo _culture { get; set; } = CultureInfo.InvariantCulture;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the rectangle element.
        /// </summary>
        /// <value>The element ID. Default: <see langword="null"/>.</value>
        [Parameter]
        public override string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the height of the rectangle in pixels.
        /// </summary>
        /// <value>The height value. Default: 450.</value>
        [Parameter]
        public double Height { get; set; } = 450;

        /// <summary>
        /// Gets or sets the width of the rectangle in pixels.
        /// </summary>
        /// <value>The width value. Default: 100.</value>
        [Parameter]
        public double Width { get; set; } = 100;

        /// <summary>
        /// Gets or sets the stroke (border) color of the rectangle.
        /// </summary>
        /// <value>A valid SVG color value or color name. Default: "transparent".</value>
        [Parameter]
        public string Stroke { get; set; } = "transparent";

        /// <summary>
        /// Gets or sets the fill color of the rectangle.
        /// </summary>
        /// <value>A valid SVG color value or color name. Default: "transparent".</value>
        [Parameter]
        public string Fill { get; set; } = "transparent";

        /// <summary>
        /// Gets or sets the SVG filter effect to apply to the rectangle.
        /// </summary>
        /// <value>An SVG filter ID or definition. Default: <see langword="null"/>.</value>
        [Parameter]
        public string Filter { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke dash array pattern for the rectangle border.
        /// </summary>
        /// <value>A dash pattern string (e.g., "5,5"). Default: <see langword="null"/>.</value>
        [Parameter]
        public string DashArray { get; set; } = null!;

        /// <summary>
        /// Gets or sets the SVG transform attribute for rotating, scaling, or translating the rectangle.
        /// </summary>
        /// <value>An SVG transform string (e.g., "rotate(45)"). Default: <see langword="null"/>.</value>
        [Parameter]
        public string Transform { get; set; } = null!;

        /// <summary>
        /// Gets or sets the width of the stroke (border) in pixels.
        /// </summary>
        /// <value>The stroke width value. Default: 1.</value>
        [Parameter]
        public double StrokeWidth { get; set; } = 1;

        /// <summary>
        /// Gets or sets the x-coordinate of the rectangle's top-left corner.
        /// </summary>
        /// <value>The x-coordinate in pixels. Default: 0.</value>
        [Parameter]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the rectangle's top-left corner.
        /// </summary>
        /// <value>The y-coordinate in pixels. Default: 0.</value>
        [Parameter]
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the x-axis radius for rounded corners.
        /// </summary>
        /// <value>The corner radius in pixels. Default: 0 (sharp corners).</value>
        [Parameter]
        public double Rx { get; set; }

        /// <summary>
        /// Gets or sets the y-axis radius for rounded corners.
        /// </summary>
        /// <value>The corner radius in pixels. Default: 0 (sharp corners).</value>
        [Parameter]
        public double Ry { get; set; }

        /// <summary>
        /// Gets or sets the opacity level of the rectangle.
        /// </summary>
        /// <value>A value between 0 (fully transparent) and 1 (fully opaque). Default: 1.</value>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Gets or sets custom inline CSS styles for the rectangle.
        /// </summary>
        /// <value>A CSS style string. Default: <see langword="null"/>.</value>
        [Parameter]
        public string Style { get; set; } = null!;

        /// <summary>
        /// Gets or sets the visibility state of the rectangle.
        /// </summary>
        /// <value>Either "visible" or "hidden". Default: "visible".</value>
        [Parameter]
        public string Visibility { get; set; } = "visible";

        /// <summary>
        /// Gets or sets the tab index for keyboard navigation.
        /// </summary>
        /// <value>The tab index value. Default: empty string.</value>
        [Parameter]
        public string TabIndex { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the rectangle is hidden from screen readers.
        /// </summary>
        /// <value><see langword="true"/> to hide from assistive technology; otherwise <see langword="false"/>. Default: "true".</value>
        [Parameter]
        public string AriaHidden { get; set; } = "true";

        /// <summary>
        /// Gets or sets the title attribute for accessibility tooltips.
        /// </summary>
        /// <value>A descriptive title string. Default: "Rect Element".</value>
        [Parameter]
        public string Title { get; set; } = "Rect Element";

        /// <summary>
        /// Gets or sets the value for accessibility text.
        /// </summary>
        [Parameter]
        public string AccessibilityText { get; set; } = string.Empty;
        #endregion
    }
}