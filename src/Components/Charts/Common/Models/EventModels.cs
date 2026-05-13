namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the border styling configuration for chart elements.
    /// </summary>
    /// <remarks>
    /// Defines the visual appearance of borders, including color and thickness.
    /// Applicable to chart series, legend, tooltip, and axis elements.
    /// </remarks>
    public class BorderModel
    {
        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
        /// <value>
        /// A CSS color string (hex, rgba, or named color). Default: <c>string.Empty</c>.
        /// </value>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the border width in pixels.
        /// </summary>
        /// <value>
        /// The thickness of the border in pixels. Default: <c>1</c>.
        /// </value>
        public double Width { get; set; } = 1;
    }

    /// <summary>
    /// Represents the visible range configuration for a chart axis.
    /// </summary>
    /// <remarks>
    /// Encapsulates the axis range metrics including minimum, maximum, delta, and interval values.
    /// Used to determine the visible range and scaling of axis labels and gridlines.
    /// </remarks>
    public class VisibleRangeModel
    {
        /// <summary>
        /// Gets or sets the axis delta value (the difference between maximum and minimum).
        /// </summary>
        /// <value>
        /// The span of the axis range. Default: <c>0</c>.
        /// </value>
        public double Delta { get; set; }

        /// <summary>
        /// Gets or sets the axis interval value (spacing between axis labels).
        /// </summary>
        /// <value>
        /// The interval or step size for axis labels and gridlines. Default: <c>0</c>.
        /// </value>
        public double Interval { get; set; }

        /// <summary>
        /// Gets or sets the maximum value visible on the axis.
        /// </summary>
        /// <value>
        /// The upper bound of the axis range. Default: <c>0</c>.
        /// </value>
        public double Max { get; set; }

        /// <summary>
        /// Gets or sets the minimum value visible on the axis.
        /// </summary>
        /// <value>
        /// The lower bound of the axis range. Default: <c>0</c>.
        /// </value>
        public double Min { get; set; }
    }
}