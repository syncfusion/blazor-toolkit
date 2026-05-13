﻿using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders a multi-colored area series.
    /// </summary>
    /// <remarks>
    /// This renderer builds SVG path options per segment and coordinates animation and rendering lifecycle.
    /// </remarks>
    internal class MultiColoredAreaSeriesRenderer : MultiColoredBaseSeriesRenderer
    {
        #region Fields
        private new readonly List<PathOptions> _options = [];
        private List<ChartSegment> _segments = [];
        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates drawing directions and generates path options per colored segment.
        /// </summary>
        /// <returns>List of chart segments computed for this series.</returns>
        private List<ChartSegment> CalculateDirection()
        {
            _options.Clear();
            bool isInverted = Owner is not null && Owner._requireInvertedAxis, rendered = false;
            List<Point> visiblePoints = EnableComplexProperty();
            ChartEventLocation startPoint = null!;
            Direction = new System.Text.StringBuilder();
            ChartData = new System.Text.StringBuilder();
            double origin = Math.Max(YAxisRenderer?.VisibleRange.Start ?? 0, 0);
            Point previous = null!;
            _segments = SortSegments(Series ?? null!, Series?.Segments ?? null!);

            ProcessPoints(visiblePoints, ref startPoint, ref previous, ref rendered, origin, isInverted);

            return _segments;
        }

        /// <summary>
        /// Processes visible points to compute path directions, segment options, and tooltip data. Handles segment breaks and empty points.
        /// </summary>
        private void ProcessPoints(List<Point> visiblePoints, ref ChartEventLocation startPoint, ref Point previous, ref bool rendered, double origin, bool isInverted)
        {
            int count = visiblePoints.Count;
            for (int i = 0; i < count; i++)
            {
                Point point = visiblePoints[i];
                point.SymbolLocations = [];
                point.Regions = [];
                Point previousPoint = GetPreviousPoint(visiblePoints, i);
                Point nextPoint = GetNextPoint(visiblePoints, i);
                rendered = false;

                if (point.Visible && ChartHelper.WithInRange(previousPoint, point, nextPoint, XAxisRenderer))
                {
                    GetAreaPathDirection(point.XValue, origin, Series ?? null!, isInverted, startPoint ?? null!, "M");
                    startPoint ??= new ChartEventLocation(point.XValue, origin);
                    ChartEventLocation firstPoint = ChartHelper.GetPoint(
                        XAxisRenderer.GetPointValue(point.XValue),
                        YAxisRenderer.GetPointValue(point.YValue),
                        XAxisRenderer,
                        YAxisRenderer,
                        XLength,
                        YLength,
                        isInverted
                    );

                    if (previous is not null && SetPointColor(point, previous, Series ?? null!, Series?.SegmentAxis == Segment.X, _segments))
                    {
                        RenderNewSegment(firstPoint, previous, startPoint, ref rendered, origin, isInverted);
                    }
                    else
                    {
                        _ = Direction.Append("L" + SPACE + firstPoint.X.ToString(Culture) + SPACE + firstPoint.Y.ToString(Culture) + SPACE);
                        _ = SetPointColor(point, null!, Series ?? null!, Series?.SegmentAxis == Segment.X, _segments);
                    }

                    if (i + 1 < visiblePoints.Count && nextPoint is not null && !nextPoint.Visible && Series is not null && Series.EmptyPointSettings.Mode != EmptyPointMode.Drop)
                    {
                        GetAreaEmptyDirection(new ChartEventLocation(point.XValue, origin), startPoint, Series, isInverted);
                        startPoint = null!;
                    }

                    previous = point;
                    StorePointLocation(point, Series ?? null!, isInverted);
                }

                // Series points are needed on the script side for keyboard navigation if markers and tooltips are not enabled.
                IChartPoint chartPoint = ChartPoints?[point.Index] ?? null!;
                if (IsTooltipEnabled() && chartPoint.SymbolLocations.Count > 0)
                {
                    AppendChartData(chartPoint);
                }
            }
            if (!rendered)
            {
                if (count > 1)
                {
                    GetAreaPathDirection(previous?.XValue ?? 0, origin, Series ?? null!, isInverted, null!, "L");
                }

                GeneratePathOption(_options, Series ?? null!, previous ?? null!, Direction.ToString(), string.Empty);
            }
        }

        /// <summary>
        /// Gets the previous point from the list of visible points based on the current index, returning null if out of bounds.
        /// </summary>
        private static Point GetPreviousPoint(List<Point> visiblePoints, int i)
        {
            return i - 1 > -1 ? visiblePoints[i - 1] : null!;
        }

        /// <summary>
        /// Gets the next point from the list of visible points based on the current index, returning null if out of bounds.
        /// </summary>
        private static Point GetNextPoint(List<Point> visiblePoints, int i)
        {
            return i + 1 < visiblePoints.Count ? visiblePoints[i + 1] : null!;
        }

        /// <summary>
        /// Renders a new segment when a color change is detected, creating path options for the previous segment and starting a new path for the current segment. Updates the rendered flag to indicate a new segment has been processed.
        /// </summary>
        private void RenderNewSegment(ChartEventLocation firstPoint, Point previous, ChartEventLocation startPoint, ref bool rendered, double origin, bool isInverted)
        {
            rendered = true;
            ChartEventLocation startRegion = ChartHelper.GetPoint(
                XAxisRenderer.GetPointValue(startPoint.X),
                YAxisRenderer.GetPointValue(origin),
                XAxisRenderer,
                YAxisRenderer,
                XLength,
                YLength,
                isInverted
            );
            _ = Direction.Append("L" + SPACE + firstPoint.X.ToString(Culture) + SPACE + firstPoint.Y.ToString(Culture) + SPACE);
            _ = Direction.Append("L" + SPACE + firstPoint.X.ToString(Culture) + SPACE + startRegion.Y.ToString(Culture) + SPACE);
            GeneratePathOption(_options, Series ?? null!, previous, Direction.ToString(), "_Point_" + previous.Index);
            Direction = new System.Text.StringBuilder();
            _ = Direction.Append("M" + SPACE + firstPoint.X.ToString(Culture) + SPACE + startRegion.Y.ToString(Culture) + SPACE + "L" + SPACE + firstPoint.X.ToString(Culture) + SPACE + firstPoint.Y.ToString(Culture) + SPACE);
        }

        /// <summary>
        /// Generates path options for a given segment based on the series properties and point data. Configures the path options with appropriate styling and data attributes for rendering. This method is called when a new segment is detected or when properties change that affect the segment's appearance.
        /// </summary>
        private void GeneratePathOption(List<PathOptions> _options, ChartSeries series, Point point, string direction, string id)
        {
            _options.Add(new PathOptions(Owner?.ID + "_Series_" + series.Renderer.Index + id, direction, series.DashArray, series.Border.Width, series.Border.Color, series.Opacity, SetPointColor(point, Interior ?? null!), "", "", "", "", "", GetDataPoints(point.XValue, point.YValue)));
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Performs series rendering work and configures animation when enabled.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            _ = CalculateDirection();

            bool isDefaultAnimation = Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default;
            if (Owner is not null && Owner._shouldAnimateSeries && (isDefaultAnimation || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        /// <summary>
        /// Builds the render tree for the multi-colored area series, creating SVG path elements based on the calculated path options. Checks for visibility and clipping before rendering, and applies segment axis configurations as needed. Calls the base implementation to ensure proper rendering structure.
        /// </summary>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect is null || builder is null || (Series is not null && !Series.Visible))
            {
                return;
            }

            CreateSeriesElements(builder);
            if (_options.Count != 0)
            {
                ApplySegmentAxis(builder, Series ?? null!, _options, _segments);
            }

            builder.CloseElement();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the direction of the series when properties change, recalculating path directions and segment options as needed. Calls the base implementation to ensure proper rendering updates.
        /// </summary>
        internal override void UpdateDirection()
        {
            _ = CalculateDirection();
            base.UpdateDirection();
        }

        /// <summary>
        /// Updates the series customization when properties like fill, dash array, width, or opacity change. Recalculates path options based on the updated properties and triggers marker color changes if necessary. Calls the base implementation to ensure proper rendering updates.
        /// </summary>
        internal override void UpdateCustomization(string property)
        {
            RendererShouldRender = Series is not null && Series.Visible;
            if (property == "Fill")
            {
                Owner?._seriesContainer?.GetSeriesRendererInterior(this);
            }
            foreach (PathOptions option in _options)
            {
                switch (property)
                {
                    case "Fill":
                        option.Stroke = Interior ?? string.Empty;
                        Series?.Marker.Renderer?.MarkerColorChanged();
                        break;
                    case "DashArray":
                        option.StrokeDashArray = Series?.DashArray ?? string.Empty;
                        break;
                    case "Width":
                        option.StrokeWidth = Series?.Width ?? 0;
                        break;
                    case "Opacity":
                        option.Opacity = Series?.Opacity ?? 0;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
    }
}
