using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders step line series for charts with support for inverted axes and animation.
    /// </summary>
    /// <remarks>
    /// This renderer handles the calculation of direction paths for step line series,
    /// supporting different step positions (Left, Center, Right) and empty point handling modes.
    /// </remarks>
    internal class StepLineSeriesRenderer : LineBaseSeriesRenderer
    {
        #region Private Methods

        /// <summary>
        /// Calculates the direction path for the step line series based on visible points.
        /// </summary>
        private void CalculateDirection()
        {
            bool isInverted = Owner is not null && Owner._requireInvertedAxis;
            string startPoint = "M";
            Point prevPoint = null!;
            double lineLength = GetLineLength();
            List<Point> visiblePoints = Series?.Renderer.Points ?? null!;

            Direction = new System.Text.StringBuilder();
            ChartData = new System.Text.StringBuilder();

            foreach (Point point in visiblePoints.ToArray())
            {
                InitializePointCollections(point);
                Point previousPoint = point.Index - 1 > -1 ? visiblePoints[point.Index - 1] : null!;
                Point nextPoint = point.Index + 1 < visiblePoints.Count ? visiblePoints[point.Index + 1] : null!;

                if (point.Visible && ChartHelper.WithInRange(previousPoint, point, nextPoint, XAxisRenderer))
                {
                    if (prevPoint is not null)
                    {
                        AppendStepLineSegment(startPoint, point, prevPoint, isInverted);
                        startPoint = "L";
                    }
                    else
                    {
                        AppendInitialPoint(startPoint, point, lineLength, isInverted);
                        startPoint = "L";
                    }
                    StorePointLocation(point, Series ?? null!, isInverted);
                    prevPoint = point;
                }
                else
                {
                    prevPoint = Series?.EmptyPointSettings.Mode == EmptyPointMode.Drop ? prevPoint : null!;
                    startPoint = Series?.EmptyPointSettings.Mode == EmptyPointMode.Drop ? startPoint : "M";
                }
                //Series points are needed on the script side for keyboard navigation if markers and tooltips are not enabled.
                IChartPoint chartPoint = ChartPoints?[point.Index] ?? null!;
                if (IsTooltipEnabled() && chartPoint.SymbolLocations.Count > 0)
                {
                    AppendChartData(chartPoint);
                }
            }

            AppendFinalPoint(visiblePoints, lineLength, isInverted, startPoint);
        }

        /// <summary>
        /// Gets the line length offset based on axis configuration.
        /// </summary>
        /// <returns>The computed line length; 0.5 for category axis with BetweenTicks placement, otherwise 0.</returns>
        private double GetLineLength()
        {
            return XAxisRenderer.Axis?.ValueType == ValueType.Category && XAxisRenderer.Axis.LabelPlacement == LabelPlacement.BetweenTicks
                ? 0.5
                : 0;
        }

        /// <summary>
        /// Initializes symbol locations and region collections for a point.
        /// </summary>
        /// <param name="point">The point to initialize.</param>
        private static void InitializePointCollections(Point point)
        {
            point.SymbolLocations = [];
            point.Regions = [];
        }

        /// <summary>
        /// Appends a step line segment direction from the previous point to the current point.
        /// </summary>
        /// <param name="startPoint">The drawing command (M or L).</param>
        /// <param name="point">The current data point.</param>
        /// <param name="previousPoint">The previous data point.</param>
        /// <param name="isInverted">Whether the axis is inverted.</param>
        private void AppendStepLineSegment(string startPoint, Point point, Point previousPoint, bool isInverted)
        {
            ChartEventLocation point2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            ChartEventLocation point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(previousPoint.XValue), YAxisRenderer.GetPointValue(previousPoint.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            _ = Direction.Append(startPoint + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
            _ = Direction.Append(GetStepLineDirection(point1, point2, Series?.StepPosition ?? StepPosition.Left));
        }

        /// <summary>
        /// Appends the initial point for the series.
        /// </summary>
        /// <param name="point">The first data point.</param>
        /// <param name="lineLength">The line length offset.</param>
        /// <param name="isInverted">Whether the axis is inverted.</param>
        /// <param name="startPoint">The drawing command (M or L).</param>
        private void AppendInitialPoint(string startPoint, Point point, double lineLength, bool isInverted)
        {
            ChartEventLocation point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue - lineLength), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            _ = Direction.Append(startPoint + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
        }

        /// <summary>
        /// Appends the final point to close the series path.
        /// </summary>
        /// <param name="visiblePoints">The list of all visible points.</param>
        /// <param name="lineLength">The line length offset.</param>
        /// <param name="isInverted">Whether the axis is inverted.</param>
        /// <param name="startPoint">The drawing command (M or L).</param>
        private void AppendFinalPoint(List<Point> visiblePoints, double lineLength, bool isInverted, string startPoint)
        {
            if (visiblePoints.Count > 0)
            {
                Point lastPoint = visiblePoints[^1];
                ChartEventLocation point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(lastPoint.XValue + lineLength), YAxisRenderer.GetPointValue(lastPoint.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                _ = Direction.Append(startPoint + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Renders the series by calculating direction paths and configuring animation options.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            _options = new PathOptions(SeriesID(), Direction.ToString(), Series?.DashArray ?? string.Empty, Series?.Width ?? 0, Interior ?? string.Empty, Series?.Opacity ?? 1, "None", " ", " ", " ", " ", " ", GetDataPoints());

            if (Owner is not null && Owner._shouldAnimateSeries && ((Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        /// <summary>
        /// Builds the render tree for the series.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
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
        /// Updates the series direction path for re-rendering.
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
