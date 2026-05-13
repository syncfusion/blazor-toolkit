using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders a line trendline series for chart components.
    /// </summary>
    /// <remarks>
    /// This renderer adapts a ChartSeries instance to draw trendlines inside a chart.
    /// It relies on members from the base <see cref="LineBaseSeriesRenderer"/>.
    /// </remarks>
    internal class Trendline_LineSeriesRenderer : LineBaseSeriesRenderer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the trendline target <see cref="ChartSeries"/> instance.
        /// </summary>
        /// <value>The series object whose trendline will be rendered.</value>
        [Parameter]
        public ChartSeries Trendlineseries { get; set; } = null!;
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes renderer-specific fields and links the renderer to the owner.
        /// </summary>
        protected override void OnInitialized()
        {
            InitSeriesRendererFields();
            Series = Trendlineseries;
            Series.SeriesType = string.IsNullOrEmpty(Series.SeriesType) ? Series.Type.ToString() : Series.SeriesType;
            Series.Renderer = this;
            SvgRenderer = Owner?._svgRenderer ?? null;
            Owner?._trendlineContainer?.AddRenderer(this);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Determines stroke/fill value for the trendline by consulting the parent series' trendline definition.
        /// </summary>
        /// <returns>Stroke or fill color string.</returns>
        private string GetStrokeFill()
        {
            ChartSeries? parentSeries = (Owner?._seriesContainer?.Renderers[SourceIndex] as ChartSeriesRenderer)?.Series;
            ChartTrendline? trendline = parentSeries?.Trendlines?.FirstOrDefault(t => t.TargetSeries == Series);
            return GetTrendlineFill(trendline!, Series?.Fill ?? string.Empty);
        }

        /// <summary>
        /// Initializes location and region collections on the specified point.
        /// </summary>
        /// <param name="point">The point to initialize.</param>
        private static void InitializePointCollections(Point point)
        {
            point.SymbolLocations = [];
            point.Regions = [];
        }

        /// <summary>
        /// Processes a visible point and updates path direction and stored locations.
        /// </summary>
        /// <param name="point">The current chart point.</param>
        /// <param name="startPoint">Reference to the current path move command.</param>
        /// <param name="prevPointX">Reference to the previous X coordinate.</param>
        /// <param name="prevPointY">Reference to the previous Y coordinate.</param>
        private void ProcessVisiblePoint(Point point, ref string startPoint, ref double prevPointX, ref double prevPointY)
        {
            double pointX = point.XValue;
            double pointY = point.YValue;
            GetLineDirection(prevPointX, prevPointY, pointX, pointY, Owner is not null && Owner._requireInvertedAxis, startPoint);
            startPoint = !double.IsNaN(prevPointX) ? "L" : startPoint;
            prevPointX = pointX;
            prevPointY = pointY;
            StorePointLocation(point, Series ?? null!, Owner is not null && Owner._requireInvertedAxis);
        }

        /// <summary>
        /// Processes a non-visible point: resets continuity and clears symbol locations.
        /// </summary>
        /// <param name="point">The point that is not visible.</param>
        /// <param name="startPoint">Reference to the current path move command.</param>
        /// <param name="prevPointX">Reference to the previous X coordinate.</param>
        /// <param name="prevPointY">Reference to the previous Y coordinate.</param>
        private void ProcessNonVisiblePoint(Point point, ref string startPoint, ref double prevPointX, ref double prevPointY)
        {
            prevPointX = prevPointY = double.NaN;
            startPoint = "M";
            point.SymbolLocations = [];
            if (ChartPoints is { })
            {
                ChartPoints[point.Index].SymbolLocations = [];
            }
        }

        /// <summary>
        /// Creates the SVG elements for the trendline path and its clipping.
        /// </summary>
        /// <param name="builder">Render tree builder used to emit markup.</param>
        private void CreateTrendlineElement(RenderTreeBuilder builder)
        {
            Rect rect = new() { X = 0, Y = 0, Width = ClipRect?.Width ?? 0, Height = ClipRect?.Height ?? 0 };
            SvgRenderer?.OpenGroupElement(builder, SeriesElementId(), "translate(" + ClipRect?.X.ToString(Culture) + "," + ClipRect?.Y.ToString(Culture) + ")", "url(#" + ClipPathId() + ")", "e-trendline-outline", Owner is not null && Owner.Focusable && Series is not null && Series.Focusable && !double.IsNaN(FirstFocusTrendlineSeriesIndex) && Index == FirstFocusTrendlineSeriesIndex ? "0" : "-1", GetSeriesDescriptionFormatText(Points ?? null!), "false", GetDataPoints(), !string.IsNullOrEmpty(Series?.AccessibilityRole) ? Series.AccessibilityRole : "group");
            SvgRenderer?.RenderClipPath(builder, ClipPathId(), rect, ((Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)) && Owner is not null && Owner._shouldAnimateSeries ? "hidden" : "visible");
            _ = SvgRenderer?.RenderPath(builder, _options ?? null!);
            builder.CloseElement();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds a consistent series identifier.
        /// </summary>
        /// <returns>A string id for the series path element.</returns>
        protected override string SeriesID()
        {
            return Owner?.ID + "_Series_" + SourceIndex + "_TrendLine_" + Index;
        }

        /// <summary>
        /// Prepares and configures rendering options for the series path.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            _options = new PathOptions(SeriesID(), Direction.ToString(), Series?.DashArray ?? string.Empty, Series?.Width ?? 0, GetStrokeFill(), Series?.Opacity ?? 1);
            if (Owner is not null && Owner._shouldAnimateSeries && ((Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        /// <summary>
        /// Builds the render tree for the trendline series.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect is null || builder is null || !TrendLineLegendVisibility)
            {
                return;
            }

            CreateTrendlineElement(builder);
            RendererShouldRender = false;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns the clip path identifier for trendlines.
        /// </summary>
        /// <returns>Clip path id string.</returns>
        internal override string ClipPathId()
        {
            return Owner?.ID + "_ChartTrendlineClipRect_" + SourceIndex;
        }

        /// <summary>
        /// Returns the group element id used to render the trendline series.
        /// </summary>
        /// <returns>Series element id string.</returns>
        internal override string SeriesElementId()
        {
            return Owner?.ID + "TrendLineSeriesGroup" + SourceIndex;
        }

        /// <summary>
        /// Calculates the path direction for the trendline based on visible points.
        /// </summary>
        internal void CalculateDirection()
        {
            string startPoint = "M";
            List<Point> visiblePoints = EnableComplexProperty();
            Direction = new System.Text.StringBuilder();
            ChartData = new System.Text.StringBuilder();
            int count = visiblePoints.Count;

            double prevPointX = double.NaN;
            double prevPointY = double.NaN;

            for (int i = 0; i < count; i++)
            {
                Point point = visiblePoints[i];
                InitializePointCollections(point);
                Point previousPoint = (i - 1 > -1) && (i - 1 < count) ? visiblePoints[i - 1] : null!;
                Point nextPoint = i + 1 < count ? visiblePoints[i + 1] : null!;

                if (point.Visible && ChartHelper.WithInRange(previousPoint, point, nextPoint, XAxisRenderer))
                {
                    ProcessVisiblePoint(point, ref startPoint, ref prevPointX, ref prevPointY);
                }
                else
                {
                    ProcessNonVisiblePoint(point, ref startPoint, ref prevPointX, ref prevPointY);
                }
                if (Owner is not null && Owner.Focusable && !double.IsNaN(FirstFocusTrendlineSeriesIndex) && Owner._tooltip is not null && point.SymbolLocations.Count > 0)
                {
                    AppendChartData(ChartPoints?[point.Index]);
                }
            }
        }

        /// <summary>
        /// Updates stored direction into the options and triggers base update.
        /// </summary>
        internal override void UpdateDirection()
        {
            CalculateDirection();
            if (_options is { })
            {
                _options.Direction = Direction.ToString();
            }
            base.UpdateDirection();
        }

        /// <summary>
        /// Returns the category for this renderer.
        /// </summary>
        /// <returns>SeriesCategories.TrendLine</returns>
        internal override SeriesCategories Category()
        {
            return SeriesCategories.TrendLine;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Called when the chart size changes and forces the series to re-render.
        /// </summary>
        /// <param name="rect">The new chart rectangle.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = true;
            SeriesRenderer();
            Series?.Marker?.Renderer?.HandleChartSizeChange(rect);
        }

        /// <summary>
        /// Queues a UI refresh for this renderer and associated markers.
        /// </summary>
        public override void ProcessRenderQueue()
        {
            _ = InvokeAsync(StateHasChanged);
            Series?.Marker?.Renderer?.ProcessRenderQueue();
        }
        #endregion
    }
}
