using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Defines the collection of <see cref="ChartTrendline"/> items used to predict trends on a series.
    /// </summary>
    /// <remarks>
    /// Place one or more <see cref="ChartTrendline"/> elements inside this collection to overlay trend analysis
    /// on a <see cref="ChartSeries"/>.
    /// </remarks>
    public class ChartTrendlines : ChartSubComponent, ISubcomponentTracker
    {
        #region Fields

        private int _pendingParametersSetCount;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parent series provided via cascading parameters.
        /// </summary>
        [CascadingParameter]
        internal ChartSeries? Series { get; set; }

        /// <summary>
        /// Gets the collection of trendlines associated with the series.
        /// </summary>
        /// <value>A list of <see cref="ChartTrendline"/> items.</value>
        internal List<ChartTrendline> Trendlines { get; set; } = [];

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the trendline collection and binds it to the parent <see cref="ChartSeries"/>.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartSeries chartSeries)
            {
                Series = chartSeries;
            }

            Series?.UpdateSeriesProperties(nameof(Trendlines), Trendlines);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Indicates a nested subcomponent started parameter updates.
        /// </summary>
        void ISubcomponentTracker.PushSubcomponent()
        {
            _pendingParametersSetCount++;
        }

        /// <summary>
        /// Indicates a nested subcomponent finished parameter updates and triggers prerender
        /// when all pending updates are complete.
        /// </summary>
        void ISubcomponentTracker.PopSubcomponent()
        {
            _pendingParametersSetCount--;

            if (_pendingParametersSetCount == 0)
            {
                Series?.Container?._trendlineContainer?.Prerender();
            }
        }

        #endregion
    }
}
