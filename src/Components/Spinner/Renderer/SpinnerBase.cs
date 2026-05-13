using System.ComponentModel;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Spinner.Internal
{
    /// <summary>
    /// Serves as a base class for Syncfusion Blazor Spinner themes, providing common methods and properties for spinner rendering.
    /// </summary>
    /// <remarks>
    /// This class contains foundational logic for calculating spinner dimensions, defining animation points, and drawing SVG arcs 
    /// that are essential for rendering the spinner visuals across different themes. It is intended for internal use within the Syncfusion Blazor Spinner component.
    /// This class implements utility methods that are shared across all spinner implementations.
    /// </remarks>
    /// <exclude/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SpinnerBase : SfBaseComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the SVG class for styling the spinner.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the CSS class for the SVG element. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// This class is applied to the main SVG element and determines the spinner's visual theme.
        /// </remarks>
        internal string? SpinnerSvgClass { get; set; }

        /// <summary>
        /// Gets or sets the viewBox attribute for the SVG element.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the viewBox value (e.g., "0 0 100 100"). The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// The viewBox defines the coordinate system for the SVG and is calculated based on the spinner's dimensions.
        /// </remarks>
        internal string? ViewBox { get; set; }

        /// <summary>
        /// Gets or sets the inline style attribute for the SVG element.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the CSS inline styles. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// This includes dimensions, transforms, and other visual properties.
        /// </remarks>
        internal string? SvgStyle { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the SVG element.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the unique SVG element ID. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// This ID is auto-generated and used for internal reference and debugging purposes.
        /// </remarks>
        internal string? SvgId { get; set; }

        /// <summary>
        /// Gets or sets the CSS path class for the spinner's circle path.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the path class. The default value is "e-path-circle".
        /// </value>
        /// <remarks>
        /// This class is applied to the static circle path element used as the background for the rotating arc.
        /// </remarks>
        internal string PathClass { get; set; } = "e-path-circle";

        #endregion

        #region Helper Methods
        /// <summary>
        /// Calculates the radius of the spinner based on its size parameter.
        /// </summary>
        /// <param name="size">The custom size of the spinner (in pixels, e.g., "60px").</param>
        /// <returns>The calculated radius as an integer (half of the provided size).</returns>
        /// <remarks>
        /// If no size is provided or parsing fails, a default radius of 15 pixels (diameter 30px) is used.
        /// The radius is calculated as half the diameter to ensure proper SVG path generation.
        /// CSS units (px, %, em, etc.) are automatically stripped before parsing.
        /// </remarks>
        internal static int CalculateRadius(string? size)
        {
            if (string.IsNullOrEmpty(size))
            {
                return 15;
            }

            ReadOnlySpan<char> span = size.AsSpan().TrimEnd();
            int end = 0;
            for (int i = 0; i < span.Length; i++)
            {
                if (char.IsDigit(span[i]) || span[i] is '.' or '-')
                {
                    end = i + 1;
                }
                else
                {
                    break;
                }
            }

            return float.TryParse(span[..end], NumberStyles.Float, CultureInfo.InvariantCulture, out float w)
                ? Convert.ToInt32(w / 2)
                : 15;
        }

        /// <summary>
        /// Defines arc points for SVG circle path calculations.
        /// </summary>
        /// <param name="centerX">The X coordinate of the circle center.</param>
        /// <param name="centerY">The Y coordinate of the circle center.</param>
        /// <param name="radius">The radius of the circle in pixels.</param>
        /// <param name="angle">The angle in degrees (0-360).</param>
        /// <returns>An <see cref="ArcPoints"/> object containing the calculated X and Y coordinates.</returns>
        /// <remarks>
        /// This method uses trigonometric calculations to determine the point on a circle at a given angle.
        /// The angle is adjusted by -90 degrees to match SVG's coordinate system where 0 degrees is at the right.
        /// </remarks>
        internal static ArcPoints DefineArcPoints(int centerX, int centerY, int radius, int angle)
        {
            // Convert angle to radians (offset by -90 to align with SVG coordinates)
            double radians = (angle - 90) * Math.PI / 180.0;

            // Calculate point coordinates using trigonometry
            return new ArcPoints
            {
                PointX = centerX + (radius * Math.Cos(radians)),
                PointY = centerY + (radius * Math.Sin(radians))
            };
        }

        #endregion
    }

    /// <summary>
    /// Represents a point with X and Y coordinates for arc calculations in SVG paths.
    /// </summary>
    /// <remarks>
    /// This immutable record struct holds coordinate values used when calculating SVG arc paths
    /// for spinner rendering. It's used internally to store the result of trigonometric calculations.
    /// </remarks>
    internal readonly record struct ArcPoints(double PointX, double PointY);

}
