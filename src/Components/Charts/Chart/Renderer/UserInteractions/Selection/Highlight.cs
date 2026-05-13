namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Handles highlight interactions for a chart instance.
    /// </summary>
    /// <remarks>
    /// Responsible for wiring mouse events, preparing highlight-related identifiers and invoking
    /// highlight style application. Ensures event unsubscription during disposal to prevent memory leaks.
    /// </remarks>
    internal class Highlight : Selection
    {
        #region Fields
        private SfChart _chartInstance;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Highlight"/> class for the provided chart.
        /// </summary>
        /// <param name="chart">The chart instance to attach highlight behavior to.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="chart"/> is <c>null</c>.</exception>
        internal Highlight(SfChart chart) : base(chart, false)
        {
            _chartInstance = chart;
            AddEventListener();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Attaches necessary event listeners to the chart instance.
        /// </summary>
        private void AddEventListener()
        {
            _chartInstance.MouseMove += MouseMoveHandler;
        }

        /// <summary>
        /// Prepares identifiers, clears selection buffers and computes whether highlighting is series mode.
        /// </summary>
        private void DeclarePrivateVariables()
        {
            StyleId = _chartInstance.ID + "_ej2_chart_highlight";
            Unselected = _chartInstance.ID + "_ej2_deselected";
            SelectedDataIndexes.Clear();
            HighlightDataIndexes.Clear();
            IsSeriesMode = _chartInstance.HighlightMode == HighlightMode.Series || (_chartInstance._legendRenderer?.Legend is not null && _chartInstance._legendRenderer.Legend.EnableHighlight);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Informs the chart that highlight-related properties changed and disables series animation.
        /// </summary>
        internal void PropertyChanged()
        {
            _chartInstance._shouldAnimateSeries = false;
        }

        /// <summary>
        /// Prepares and applies highlight styles for the current chart state.
        /// </summary>
        internal void InvokeHighlight()
        {
            DeclarePrivateVariables();
            Series = _chartInstance._seriesContainer?.Renderers ?? null!;
            SeriesStyles();
            CurrentMode = (ChartSelectionMode)_chartInstance.HighlightMode;
        }

        /// <summary>
        /// Releases resources used by this instance, detaches events and calls base disposal.
        /// </summary>
        internal new void Dispose()
        {
            base.Dispose();
            _chartInstance = null!;
        }
        #endregion
    }
}