using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders an SVG path element with customizable attributes and state management.
    /// </summary>
    /// <remarks>
    /// The <see cref="SvgPath"/> component provides a reusable SVG path with support for dynamic attribute updates, accessibility, and styling. It is commonly used in data visualization charts.
    /// </remarks>
    public partial class SvgPath
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the SVG path element.
        /// </summary>
        /// <value>A string representing the element ID. Required.</value>
        [Parameter]
        public override string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the SVG path data (d attribute).
        /// </summary>
        /// <value>The path data string. Default: <c>null</c>.</value>
        [Parameter]
        public string Direction { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke dash array pattern.
        /// </summary>
        /// <value>A string representing the dash pattern (e.g., "5,5"). Default: <c>null</c>.</value>
        [Parameter]
        public string StrokeDashArray { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke color of the path.
        /// </summary>
        /// <value>A color string or hex value. Default: <c>null</c>.</value>
        [Parameter]
        public string Stroke { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke width of the path.
        /// </summary>
        /// <value>A numeric value in pixels. Default: <c>1</c>.</value>
        [Parameter]
        public double StrokeWidth { get; set; } = 1;

        /// <summary>
        /// Gets or sets the opacity of the path element.
        /// </summary>
        /// <value>A value between 0 and 1. Default: <c>1</c>.</value>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the fill color of the path.
        /// </summary>
        /// <value>A color string, hex value, or "none". Default: <c>"none"</c>.</value>
        [Parameter]
        public string Fill { get; set; } = "none";

        /// <summary>
        /// Gets or sets the SVG transform attribute.
        /// </summary>
        /// <value>A transform string (e.g., "translate(0,0)"). Default: <c>null</c>.</value>
        [Parameter]
        public string Transform { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke miter limit.
        /// </summary>
        /// <value>A numeric value. Default: <c>null</c>.</value>
        [Parameter]
        public string StrokeMiterLimit { get; set; } = null!;

        /// <summary>
        /// Gets or sets the clip path reference.
        /// </summary>
        /// <value>A URL reference (e.g., "url(#clipPath)"). Default: <see cref="string.Empty"/>.</value>
        [Parameter]
        public string ClipPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets inline CSS styles for the path element.
        /// </summary>
        /// <value>A CSS style string. Default: <see cref="string.Empty"/>.</value>
        [Parameter]
        public string Style { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the data point identifier for chart integration.
        /// </summary>
        /// <value>A data point key or identifier. Default: <see cref="string.Empty"/>.</value>
        [Parameter]
        public string DataPoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accessibility text for screen readers.
        /// </summary>
        /// <value>A descriptive text string. Default: <see cref="string.Empty"/>.</value>
        [Parameter]
        public string AccessibilityText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the visibility state of the path.
        /// </summary>
        /// <value>"visible", "hidden", or an empty string. Default: <see cref="string.Empty"/>.</value>
        [Parameter]
        public string Visibility { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tab index for keyboard navigation.
        /// </summary>
        /// <value>A numeric string or empty. Default: <see cref="string.Empty"/>.</value>
        [Parameter]
        public string TabIndex { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title attribute for tooltip support.
        /// </summary>
        /// <value>A descriptive title. Default: <c>"Path Element"</c>.</value>
        [Parameter]
        public string Title { get; set; } = "Path Element";

        #endregion

        #region Fields
        CultureInfo _culture { get; set; } = CultureInfo.InvariantCulture;
        #endregion
    }
}