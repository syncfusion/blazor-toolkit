using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// <see cref="Trendline_SplineSeriesRenderer"/> renders spline trendline series for charting.
    /// </summary>
    /// <remarks>
    /// This renderer is used by the chart's trendline container. It computes path directions,
    /// prepares SVG path options and coordinates with animation and clipping infrastructure.
    /// </remarks>
    internal class Trendline_SplineSeriesRenderer : SplineBaseSeriesRenderer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the trendline series instance used by this renderer.
        /// </summary>
        /// <value>The trendline <see cref="ChartSeries"/> associated with this renderer.</value>
        [Parameter]
        public ChartSeries Trendlineseries { get; set; } = null!;
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes renderer fields and registers with owner's trendline container.
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
        /// Computes the stroke color/fill for the trendline by resolving from parent series trendline config.
        /// </summary>
        /// <returns>Resolved stroke/fill string.</returns>
        private string GetStrokeFill()
        {
            ChartSeries? parentSeries = (Owner?._seriesContainer?.Renderers[SourceIndex] as ChartSeriesRenderer)?.Series;
            ChartTrendline? trendline = parentSeries?.Trendlines?.FirstOrDefault(t => t.TargetSeries == Series);
            return GetTrendlineFill(trendline!, Series?.Fill ?? string.Empty);
        }

        /// <summary>
        /// Calculates the path direction for the spline trendline by iterating visible points and building spline segments.
        /// </summary>
        private void CalculateDirection()
        {
            Direction = new System.Text.StringBuilder();
            ChartData = new System.Text.StringBuilder();
            List<Point> points = Series?.Renderer.Points ?? null!;
            bool isInverted = Owner is not null && Owner._requireInvertedAxis;
            string startPoint = "M";
            Point firstPoint = null!;

            foreach (Point point in points)
            {
                int previous = GetPreviousIndex(points, point.Index - 1, Series ?? null!), next = GetNextIndex(points, point.Index - 1, Series ?? null!);
                InitializePointCollections(point);

                if (point.Visible && ChartHelper.WithInRange(previous > -1 ? points[previous] : null!, point, (previous > -1) && (next < points.Count) ? points[next] : null!, XAxisRenderer))
                {
                    if (firstPoint is not null)
                    {
                        _ = Direction.Append(GetSplineDirection(DrawPoints[previous], firstPoint, point, isInverted, Series ?? null!, startPoint));
                        startPoint = "L";
                    }

                    firstPoint = point;
                    StorePointLocation(point, Series ?? null!, isInverted);
                }
                else
                {
                    startPoint = "M";
                    firstPoint = null!;
                    point.SymbolLocations = [];
                    if (ChartPoints is { })
                    {
                        ChartPoints[point.Index].SymbolLocations = [];
                    }
                }
                if (Owner is not null && Owner.Focusable && !double.IsNaN(FirstFocusTrendlineSeriesIndex) && Owner._tooltip is not null && point.SymbolLocations.Count > 0)
                {
                    AppendChartData(ChartPoints?[point.Index]);
                }
            }
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
        /// Creates the SVG elements required to render the trendline (group, clip, path).
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
        private void CreateTrendlineElement(RenderTreeBuilder builder)
        {
            Rect rect = new() { X = 0, Y = 0, Width = ClipRect?.Width ?? 0, Height = ClipRect?.Height ?? 0 };
            SvgRenderer?.OpenGroupElement(builder, SeriesElementId(), "translate(" + ClipRect?.X.ToString(Culture) + "," + ClipRect?.Y.ToString(Culture) + ")", "url(#" + ClipPathId() + ")", "e-trendspline-outline", Owner is not null && Owner.Focusable && Series is not null && Series.Focusable && !double.IsNaN(FirstFocusTrendlineSeriesIndex) && Index == FirstFocusTrendlineSeriesIndex ? "0" : "-1", GetSeriesDescriptionFormatText(Points ?? null!), "false", GetDataPoints(), !string.IsNullOrEmpty(Series?.AccessibilityRole) ? Series.AccessibilityRole : "group");
            SvgRenderer?.RenderClipPath(builder, ClipPathId(), rect, ((Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)) && Owner is not null && Owner._shouldAnimateSeries ? "hidden" : "visible");
            _ = SvgRenderer?.RenderPath(builder, _options ?? null!);
            builder.CloseElement();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds a stable identifier for the trendline series element.
        /// </summary>
        /// <returns>A unique series identifier string.</returns>
        protected override string SeriesID()
        {
            return Owner?.ID + "_Series_" + SourceIndex + "_TrendLine_" + Index;
        }

        /// <summary>
        /// Prepares path options and optional animation options for the trendline.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            _options = new PathOptions()
            {
                Id = SeriesID(),
                Fill = Constants.Transparent,
                StrokeWidth = Series?.Width ?? 0,
                Stroke = GetStrokeFill(),
                Opacity = Series?.Opacity ?? 1,
                StrokeDashArray = Series?.DashArray ?? string.Empty,
                Direction = Direction.ToString()
            };

            if (Owner is not null && Owner._shouldAnimateSeries && ((Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        /// <summary>
        /// Builds the render tree for the trendline. If clip rect is not ready or legend visibility disallows,
        /// rendering is skipped.
        /// </summary>
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
        /// Returns the clip-path id used for trendline clipping.
        /// </summary>
        /// <returns>Clip path identifier string.</returns>
        internal override string ClipPathId()
        {
            return Owner?.ID + "_ChartTrendlineClipRect_" + SourceIndex;
        }

        /// <summary>
        /// Returns the SVG group id used for the trendline series group.
        /// </summary>
        /// <returns>Series group identifier.</returns>
        internal override string SeriesElementId()
        {
            return Owner?.ID + "TrendLineSeriesGroup" + SourceIndex;
        }

        /// <summary>
        /// Updates the direction string when chart reflows or points move.
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
        /// Handles chart size changes by marking the renderer for re-render and updating dependent markers.
        /// </summary>
        /// <param name="rect">The current chart clipping rectangle.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = true;
            SeriesRenderer();
            Series?.Marker?.Renderer?.HandleChartSizeChange(rect);
        }

        /// <summary>
        /// Requests a render update asynchronously and forwards the queue processing to marker renderer.
        /// </summary>
        public override void ProcessRenderQueue()
        {
            _ = InvokeAsync(StateHasChanged);
            Series?.Marker?.Renderer?.ProcessRenderQueue();
        }
        #endregion
    }
}
