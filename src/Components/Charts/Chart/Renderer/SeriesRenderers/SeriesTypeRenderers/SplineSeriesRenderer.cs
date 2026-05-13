using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders a spline series within the chart renderer pipeline.
    /// </summary>
    /// <remarks>
    /// Keeps path direction, chart data and handles visibility, symbol locations and regions for spline points.
    /// </remarks>
    internal class SplineSeriesRenderer : SplineBaseSeriesRenderer
    {
        #region Private Methods

        /// <summary>
        /// Calculates the path direction and chart data for the spline series.
        /// </summary>
        private void CalculateDirection()
        {
            bool isInverted = Owner is not null && Owner._requireInvertedAxis;
            List<Point> points = Series?.Renderer.Points ?? null!;
            string startPoint = "M";
            Direction = new System.Text.StringBuilder();
            ChartData = new System.Text.StringBuilder();

            int count = points.Count;
            Point firstPoint = null!;

            for (int i = 0; i < points.Count; i++)
            {
                Point point = points[i];
                point.SymbolLocations = [];
                point.Regions = [];
                Point previousPoint = (i - 1 > -1) && (i - 1 < count) ? points[i - 1] : null!;
                Point nextPoint = i + 1 < count ? points[i + 1] : null!;

                if (point.Visible && ChartHelper.WithInRange(previousPoint, point, nextPoint, XAxisRenderer))
                {
                    ProcessVisiblePoint(ref firstPoint, ref startPoint, points, point, isInverted);
                }
                else
                {
                    ProcessNonVisiblePoint(ref startPoint, point);
                    firstPoint = null!;
                }
                IChartPoint chartPoint = ChartPoints?[point.Index] ?? null!;
                if (IsTooltipEnabled() && chartPoint.SymbolLocations.Count > 0)
                {
                    AppendChartData(chartPoint);
                }
            }
        }

        /// <summary>
        /// Handles a point that is visible and within range.
        /// </summary>
        /// <param name="firstPoint">Reference to the previously processed visible point.</param>
        /// <param name="startPoint">Reference to the current path start command ("M" or "L").</param>
        /// <param name="points">All points in the series.</param>
        /// <param name="point">The current point being processed.</param>
        /// <param name="isInverted">Whether the chart uses an inverted axis.</param>
        private void ProcessVisiblePoint(ref Point firstPoint, ref string startPoint, List<Point> points, Point point, bool isInverted)
        {
            int previous = GetPreviousIndex(points, point.Index - 1, Series ?? null!);
            if (firstPoint is not null)
            {
                _ = Direction.Append(GetSplineDirection(DrawPoints[previous], firstPoint, point, isInverted, Series ?? null!, startPoint));
                startPoint = "L";
            }

            firstPoint = point;
            StorePointLocation(point, Series ?? null!, isInverted);
        }

        /// <summary>
        /// Handles a point that is not visible or out of range.
        /// </summary>
        /// <param name="startPoint">Reference to the current path start command.</param>
        /// <param name="point">The point being processed.</param>
        private void ProcessNonVisiblePoint(ref string startPoint, Point point)
        {
            startPoint = "M";
            point.SymbolLocations = [];
            if (ChartPoints is { })
            {
                ChartPoints[point.Index].SymbolLocations = [];
            }
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Prepares series rendering state and path options for the spline series.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            string name = SeriesID();
            _options = new PathOptions()
            {
                Id = name,
                Fill = Constants.Transparent,
                StrokeWidth = Series?.Width ?? 0,
                Stroke = Interior ?? string.Empty,
                Opacity = Series?.Opacity ?? 1,
                StrokeDashArray = Series?.DashArray ?? string.Empty,
                Direction = Direction.ToString(),
                DataPoint = GetDataPoints()
            };

            if (ShouldAnimate())
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        /// <summary>
        /// Builds render tree for the spline series element.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to construct the render tree.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect is null || builder is null || (Series is not null && !Series.Visible))
            {
                return;
            }

            CreateSeriesElements(builder);
            RenderSeriesElement(builder, _options ?? null!);
            builder.CloseElement();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the direction string when an incremental update is required.
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
        #endregion
    }
}
