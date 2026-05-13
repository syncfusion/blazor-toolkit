using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the configuration for marker explosion on chart data point interaction.
    /// </summary>
    /// <remarks>
    /// This helper binds into interaction logic that visually emphasizes a point (e.g., on hover or selection).
    /// </remarks>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public class MarkerExplode : ChartData
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkerExplode"/> class.
        /// </summary>
        /// <param name="chart">The owning chart instance.</param>
        internal MarkerExplode(SfChart chart) : base(chart)
        {
            Chart = chart;
        }

        #endregion
    }
}