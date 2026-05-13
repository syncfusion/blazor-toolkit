using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    internal abstract class AxisLayout
    {
        #region Constants
        protected const string SPACE = " ";
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent chart instance.
        /// </summary>
        internal SfChart? Chart { get; set; }

        /// <summary>
        /// Gets or sets the clipping rectangle for series rendering.
        /// </summary>
        internal Rect? SeriesClipRect { get; set; }

        /// <summary>
        /// Gets or sets the radius for circular axis calculations.
        /// </summary>
        internal double Radius { get; set; }

        /// <summary>
        /// Gets or sets the culture information for axis rendering.
        /// </summary>
        protected CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
        #endregion

        #region Methods

        /// <summary>
        /// Adds an axis to the layout tracking.
        /// </summary>
        /// <param name="axis">The axis to add.</param>
        internal abstract void AddAxis(ChartAxis axis);

        /// <summary>
        /// Removes an axis from the layout tracking.
        /// </summary>
        /// <param name="axis">The axis to remove.</param>
        internal abstract void RemoveAxis(ChartAxis axis);

        /// <summary>
        /// Clears all axes tracked by the layout.
        /// </summary>
        internal abstract void ClearAxes();

        /// <summary>
        /// Computes the plot area bounds and performs layout.
        /// </summary>
        /// <param name="rect">The plot area rectangle to compute against.</param>
        internal abstract void ComputePlotAreaBounds(Rect rect);

        /// <summary>
        /// Performs calculations required for rendering an axis.
        /// </summary>
        /// <param name="renderer">The axis renderer instance.</param>
        internal abstract void AxisRenderingCalculation(ChartAxisRenderer renderer);

        #endregion
    }
}
