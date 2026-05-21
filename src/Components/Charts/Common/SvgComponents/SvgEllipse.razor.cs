using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents an SVG ellipse element wrapper used by DataViz components.
    /// </summary>
    public partial class SvgEllipse
    {
        #region Fields
        private CultureInfo _culture { get; set; } = CultureInfo.InvariantCulture;
        #endregion

        #region Properties
        /// <summary>
        /// Accessibility text exposed for assistive technologies.
        /// </summary>
        /// <value>The accessible description; defaults to empty string.</value>
        [Parameter]
        public string AccessibilityText { get; set; } = string.Empty;

        /// <summary>
        /// Element id used in the rendered SVG element.
        /// </summary>
        /// <value>An identifier string. This overrides the base Id.</value>
        [Parameter]
        public override string Id { get; set; } = null!;

        /// <summary>
        /// X-axis radius of the ellipse.
        /// </summary>
        /// <value>CSS-style or numeric string used for the rx attribute.</value>
        [Parameter]
        public string Rx { get; set; } = null!;

        /// <summary>
        /// Y-axis radius of the ellipse.
        /// </summary>
        /// <value>CSS-style or numeric string used for the ry attribute.</value>
        [Parameter]
        public string Ry { get; set; } = null!;

        /// <summary>
        /// X-coordinate of the ellipse center.
        /// </summary>
        /// <value>CSS-style or numeric string used for the cx attribute.</value>
        [Parameter]
        public string Cx { get; set; } = null!;

        /// <summary>
        /// Y-coordinate of the ellipse center.
        /// </summary>
        /// <value>CSS-style or numeric string used for the cy attribute.</value>
        [Parameter]
        public string Cy { get; set; } = null!;

        /// <summary>
        /// Stroke dash array for dashed strokes.
        /// </summary>
        /// <value>String for stroke-dasharray attribute; empty means solid.</value>
        [Parameter]
        public string StrokeDashArray { get; set; } = null!;

        /// <summary>
        /// Stroke color of the ellipse.
        /// </summary>
        /// <value>CSS color string.</value>
        [Parameter]
        public string Stroke { get; set; } = null!;

        /// <summary>
        /// Stroke width of the ellipse.
        /// </summary>
        /// <value>Numeric width in CSS pixels. Defaults to 1.</value>
        [Parameter]
        public double StrokeWidth { get; set; } = 1;

        /// <summary>
        /// Opacity of the ellipse.
        /// </summary>
        /// <value>Value between 0 and 1. Defaults to 1.</value>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Fill color of the ellipse.
        /// </summary>
        /// <value>CSS color string or 'none'. Defaults to 'none'.</value>
        [Parameter]
        public string Fill { get; set; } = "none";

        /// <summary>
        /// Visibility attribute for the SVG element.
        /// </summary>
        /// <value>Visibility CSS or SVG value; empty indicates default visibility.</value>
        [Parameter]
        public string Visibility { get; set; } = string.Empty;

        /// <summary>
        /// Arbitrary data point metadata serialized as a string.
        /// </summary>
        /// <value>Used by parent components to attach metadata.</value>
        [Parameter]
        public string DataPoint { get; set; } = string.Empty;

        /// <summary>
        /// ARIA/tooltip title for the element.
        /// </summary>
        /// <value>Text used as title; defaults to "Ellipse Element".</value>
        [Parameter]
        public string Title { get; set; } = "Ellipse Element";
        #endregion
    }
}