﻿using Microsoft.AspNetCore.Components.Rendering;
using System.Text;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renderer for step area series in chart components.
    /// Handles calculation of step area paths, point locations, and animation for step area visualization.
    /// </summary>
    /// <remarks>
    /// This renderer extends <see cref="LineBaseSeriesRenderer"/> to provide specialized rendering
    /// for step area charts, including direction calculation, border rendering, and data point tracking.
    /// </remarks>
    internal class StepAreaSeriesRenderer : LineBaseSeriesRenderer
    {
        #region Fields
        private string? _dataPoints;
        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes path rendering options with series styling and properties.
        /// </summary>
        private void InitializePathOptions()
        {
            string seriesId = SeriesID();
            _options = new PathOptions()
            {
                Id = seriesId,
                Fill = Interior ?? string.Empty,
                StrokeWidth = Series?.Border.Width ?? 0,
                Stroke = Series?.Border.Color ?? string.Empty,
                Opacity = Series?.Opacity ?? 1,
                StrokeDashArray = Series?.DashArray ?? string.Empty,
                Direction = Direction.ToString(),
                DataPoint = _dataPoints ?? string.Empty
            };
        }

        /// <summary>
        /// Calculates the SVG direction path for the step area series, including visible points,
        /// empty point handling, and inverted axis support.
        /// </summary>
        private void CalculateDirection()
        {
            bool isInverted = Owner is not null && Owner._requireInvertedAxis;
            List<Point> visiblePoints = EnableComplexProperty();
            Direction = new StringBuilder();
            ChartData = new StringBuilder();

            double lineLength = GetLineLength();
            double origin = Math.Max(YAxisRenderer?.VisibleRange.Start ?? 0, 0);

            ProcessSeriesPoints(visiblePoints, lineLength, origin, isInverted);
            FinalizeAreaPath(visiblePoints, lineLength, origin, isInverted);
        }

        /// <summary>
        /// Determines the line length offset based on the X-axis configuration.
        /// </summary>
        /// <returns>The calculated line length offset.</returns>
        private double GetLineLength()
        {
            return XAxisRenderer.Axis?.ValueType == ValueType.Category && XAxisRenderer.Axis.LabelPlacement == LabelPlacement.BetweenTicks
                ? 0.5
                : 0;
        }

        /// <summary>
        /// Processes each visible series point to calculate step area direction segments.
        /// </summary>
        /// <param name="visiblePoints">The list of visible data points.</param>
        /// <param name="lineLength">The offset line length for axis label placement.</param>
        /// <param name="origin">The Y-axis origin value.</param>
        /// <param name="isInverted">Indicates whether the chart axes are inverted.</param>
        private void ProcessSeriesPoints(List<Point> visiblePoints, double lineLength, double origin, bool isInverted)
        {
            ChartEventLocation start = null!;
            Point prevPoint = null!;

            for (int i = 0; i < visiblePoints.Count; i++)
            {
                Point point = visiblePoints[i];
                point.SymbolLocations = [];
                point.Regions = [];
                Point nextPoint = i + 1 < visiblePoints.Count ? visiblePoints[i + 1] : null!;

                if (point.Visible && ChartHelper.WithInRange(i - 1 > -1 ? visiblePoints[i - 1] : null!, point, nextPoint, XAxisRenderer))
                {
                    if (start is null)
                    {
                        start = new ChartEventLocation(point.XValue, 0);
                        InitializeAreaStart(point, lineLength, origin, isInverted);
                    }

                    if (prevPoint is not null)
                    {
                        AppendStepSegment(prevPoint, point, isInverted);
                    }
                    else if (Series?.EmptyPointSettings.Mode == EmptyPointMode.Gap)
                    {
                        AppendLineSegment(point, isInverted);
                    }

                    StorePointLocation(point, Series ?? null!, isInverted);
                    prevPoint = point;
                    _dataPoints = GetDataPoints(point.XValue, point.YValue);
                }
                //Series points are needed on the script side for keyboard navigation if markers and tooltips are not enabled.
                IChartPoint chartPoint = ChartPoints?[point.Index] ?? null!;
                if (IsTooltipEnabled() && chartPoint.SymbolLocations.Count > 0)
                {
                    AppendChartData(chartPoint);
                }

                HandleDisconnection(nextPoint, point, lineLength, origin, isInverted, ref start, ref prevPoint);
            }
        }

        /// <summary>
        /// Initializes the starting point of the area path.
        /// </summary>
        private void InitializeAreaStart(Point point, double lineLength, double origin, bool isInverted)
        {
            ChartEventLocation startLocation = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue - lineLength), YAxisRenderer.GetPointValue(origin), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            _ = Direction.Append("M" + SPACE + Convert.ToInt32(startLocation.X).ToString(Culture) + SPACE + Convert.ToInt32(startLocation.Y).ToString(Culture) + SPACE);
            ChartEventLocation lineStart = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue - lineLength), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            _ = Direction.Append("L" + SPACE + Convert.ToInt32(lineStart.X).ToString(Culture) + SPACE + Convert.ToInt32(lineStart.Y).ToString(Culture) + SPACE);
        }

        /// <summary>
        /// Appends a step line segment to the direction path.
        /// </summary>
        private void AppendStepSegment(Point prevPoint, Point point, bool isInverted)
        {
            ChartEventLocation currentLocation = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            ChartEventLocation prevLocation = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(prevPoint.XValue), YAxisRenderer.GetPointValue(prevPoint.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            _ = Direction.Append(GetStepLineDirection(prevLocation, currentLocation, Series?.StepPosition ?? StepPosition.Left));
        }

        /// <summary>
        /// Appends a simple line segment to the direction path.
        /// </summary>
        private void AppendLineSegment(Point point, bool isInverted)
        {
            ChartEventLocation location = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            _ = Direction.Append("L" + SPACE + Convert.ToInt32(location.X).ToString(Culture) + SPACE + Convert.ToInt32(location.Y).ToString(Culture) + SPACE);
        }

        /// <summary>
        /// Handles disconnections in the series (e.g., empty points with Drop mode).
        /// </summary>
        private void HandleDisconnection(Point nextPoint, Point point, double lineLength, double origin, bool isInverted, ref ChartEventLocation start, ref Point prevPoint)
        {
            if (nextPoint is not null && !nextPoint.Visible && (Series?.EmptyPointSettings.Mode != EmptyPointMode.Drop))
            {
                ChartEventLocation endLocation = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue + lineLength), YAxisRenderer.GetPointValue(origin), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                _ = Direction.Append(((start is null) ? "M" : "L") + SPACE + Convert.ToInt32(endLocation.X).ToString(Culture) + SPACE + Convert.ToInt32(endLocation.Y).ToString(Culture));
                start = null!;
                prevPoint = null!;
            }
        }

        /// <summary>
        /// Finalizes the area path by closing the bottom edge of the area.
        /// </summary>
        private void FinalizeAreaPath(List<Point> visiblePoints, double lineLength, double origin, bool isInverted)
        {
            if ((visiblePoints.Count > 1) && Direction.Length > 0)
            {
                Point lastPoint = visiblePoints[^1];
                ChartEventLocation endLocation = new(lastPoint.XValue + lineLength, lastPoint.YValue);
                ChartEventLocation originLocation = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(endLocation.X), YAxisRenderer.GetPointValue(endLocation.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                _ = Direction.Append(SPACE + "L" + SPACE + Convert.ToInt32(originLocation.X).ToString(Culture) + SPACE + Convert.ToInt32(originLocation.Y).ToString(Culture) + SPACE);
                endLocation = new ChartEventLocation(lastPoint.XValue + lineLength, origin);
                originLocation = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(endLocation.X), YAxisRenderer.GetPointValue(endLocation.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                if (lastPoint.Visible)
                {
                    _ = Direction.Append("L" + SPACE + Convert.ToInt32(originLocation.X).ToString(Culture) + SPACE + Convert.ToInt32(originLocation.Y).ToString(Culture) + SPACE);
                }
            }
            else
            {
                Direction = new StringBuilder();
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Performs the initial rendering of the step area series, including animation setup.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            InitializePathOptions();

            if (Owner is not null && Owner._shouldAnimateSeries && ((Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        /// <summary>
        /// Extracts the border direction path from the area series path by removing the first move and last close commands.
        /// </summary>
        /// <param name="seriesDirection">The complete area series direction path.</param>
        /// <returns>The border direction path without area fill coordinates.</returns>
        protected override string GetBorderDirection(string seriesDirection)
        {
            List<string> coordinates = [.. seriesDirection.Split(" ")];
            if (!string.IsNullOrEmpty(seriesDirection))
            {
                coordinates.RemoveRange(1, 3);
                coordinates.RemoveRange(coordinates.Count - 4, 3);
            }
            return string.Join(" ", coordinates);
        }

        /// <summary>
        /// Builds the render tree for the step area series, including the area fill and border.
        /// </summary>
        /// <param name="builder">The render tree builder used to construct the component.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect is null || builder is null || (Series is not null && !Series.Visible))
            {
                return;
            }

            CreateSeriesElements(builder);
            SetBorderOptions();
            RenderSeriesElement(builder, _options ?? null!);
            RenderSeriesElement(builder, _borderOptions ?? null!);
            builder.CloseElement();
            _ = Direction.Clear();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the direction path and applies the changes to rendering options.
        /// </summary>
        internal override void UpdateDirection()
        {
            CalculateDirection();
            if (_options is { })
            {
                _options.Direction = Direction.ToString();
            }
            if (_borderOptions is { })
            {
                _borderOptions.Direction = GetBorderDirection(_options?.Direction ?? null!);
            }
            base.UpdateDirection();
        }
        #endregion
    }
}
