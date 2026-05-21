using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Defines the collection of regions (segments) that helps to differentiate a line-type series.
    /// </summary>
    public class ChartSegments : ChartSubComponent
    {
        #region Properties

        /// <summary>
        /// Gets the collection of <see cref="ChartSegment"/> items.
        /// </summary>
        /// <value>The list of segments for the owning series.</value>
        internal List<ChartSegment> _segments = [];

        /// <summary>
        /// Gets or sets the associated series via cascading parameters.
        /// </summary>
        /// <value>The owning <see cref="ChartSeries"/> instance.</value>
        [CascadingParameter]
        private ChartSeries? Series { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Called by the framework to initialize the component. Binds to the parent series and
        /// registers the current segment list with the series so that it can use them during rendering.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartSeries chartSeries)
            {
                Series = chartSeries;
            }

            Series?.UpdateSeriesProperties("Segments", _segments);

        }

        /// <summary>
        /// Disposes the component and clears segment references.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            Series = null;
            _segments.Clear();
            return base.DisposeAsyncCore();
        }

        #endregion
    }
}
