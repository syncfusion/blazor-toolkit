using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Specifies the configuration of markers that appear on a trendline.
    /// </summary>
    /// <remarks>
    /// This child component updates its owning <see cref="ChartTrendline"/> with marker settings
    /// and triggers a refresh on initialization and parameter updates.
    /// </remarks>
    public class ChartTrendlineMarker : ChartCommonMarker
    {
        #region Fields

        /// <summary>
        /// The parent trendline provided via cascading parameters.
        /// </summary>
        [CascadingParameter]
        private ChartTrendline? Trendline { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the marker component and propagates settings to the parent <see cref="ChartTrendline"/>.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartTrendline chartTrendline)
            {
                Trendline = chartTrendline;
            }

            Trendline?.UpdateTrendlineProperty(nameof(ChartTrendline.Marker), this);
            Trendline?.TrendlineInitiator?.UpdateTrendlineMarker();
        }

        /// <exclude />
        /// <summary>
        /// Propagates parameter changes to the parent <see cref="ChartTrendline"/>.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Trendline?.UpdateTrendlineProperty(nameof(ChartTrendline.Marker), this);
        }

        /// <summary>
        /// Disposes the marker component and clears references to prevent memory leaks.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            Trendline = null;
            return base.DisposeAsyncCore();
        }

        #endregion
    }
}
