using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders an SVG line element with customizable coordinates and styling.
    /// </summary>
    /// <remarks>
    /// The <see cref="SvgLine"/> component provides a reusable SVG line for chart axes, gridlines, and other linear graphic elements.
    /// </remarks>
    public partial class SvgLine
    {
        #region Fields
        CultureInfo _culture { get; set; } = CultureInfo.InvariantCulture;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the accessibility text for screen readers.
        /// </summary>
        /// <value>A descriptive text string. Default: <see cref="string.Empty"/>.</value>
        [Parameter]
        public string AccessibilityText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier for the SVG line element.
        /// </summary>
        /// <value>A string representing the element ID. Required.</value>
        [Parameter]
        public override string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke color of the line.
        /// </summary>
        /// <value>A color string, hex value, or "transparent". Default: <c>"transparent"</c>.</value>
        [Parameter]
        public string Stroke { get; set; } = "transparent";

        /// <summary>
        /// Gets or sets the stroke width of the line.
        /// </summary>
        /// <value>A numeric value in pixels. Default: <c>1</c>.</value>
        [Parameter]
        public double StrokeWidth { get; set; } = 1;

        /// <summary>
        /// Gets or sets the x-coordinate of the line start point.
        /// </summary>
        /// <value>A numeric value. Default: <c>0</c>.</value>
        [Parameter]
        public double X1 { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the line start point.
        /// </summary>
        /// <value>A numeric value. Default: <c>0</c>.</value>
        [Parameter]
        public double Y1 { get; set; }

        /// <summary>
        /// Gets or sets the x-coordinate of the line end point.
        /// </summary>
        /// <value>A numeric value. Default: <c>0</c>.</value>
        [Parameter]
        public double X2 { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the line end point.
        /// </summary>
        /// <value>A numeric value. Default: <c>0</c>.</value>
        [Parameter]
        public double Y2 { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the line element.
        /// </summary>
        /// <value>A value between 0 and 1. Default: <c>1</c>.</value>
        [Parameter]
        public double Opacity { get; set; } = 1;

        #endregion
    }
}