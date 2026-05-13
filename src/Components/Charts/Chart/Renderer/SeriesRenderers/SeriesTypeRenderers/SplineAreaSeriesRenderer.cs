﻿using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders spline area series. Calculates path directions and prepares render options.
    /// </summary>
    /// <remarks>
    /// This internal renderer is used by the charting engine to construct SVG path data
    /// for spline area series and its border. It preserves original behavior while
    /// improving clarity, method sizing and XML documentation.
    /// </remarks>
    internal class SplineAreaSeriesRenderer : SplineBaseSeriesRenderer
    {
        #region Fields
        private string? _dataPoints;
        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates the SVG path direction(s) and collects chart data for spline area rendering.
        /// </summary>
        private void CalculateDirection()
        {
            bool isInverted = Owner is not null && Owner._requireInvertedAxis;
            List<Point> points = Series?.Renderer.Points ?? null!;
            Direction = new System.Text.StringBuilder();
            ChartData = new System.Text.StringBuilder();
            double origin = Math.Max(YAxisRenderer?.VisibleRange.Start ?? 0, 0);
            int count = points.Count;

            Point firstPoint = null!;
            ChartEventLocation data = null!;
            ChartEventLocation startPoint = null!;

            for (int i = 0; i < count; i++)
            {
                Point point = points[i];
                point.SymbolLocations = [];
                point.Regions = [];

                Point previousPoint = (i - 1 > -1) && (i - 1 < count) ? points[i - 1] : null!;
                Point nextPoint = i + 1 < count ? points[i + 1] : null!;

                if (point.Visible && ChartHelper.WithInRange(previousPoint, point, nextPoint, XAxisRenderer))
                {
                    ProcessVisiblePoint(points, point, isInverted, origin, ref firstPoint, ref data, ref startPoint);

                }
                else
                {
                    HandleHiddenPoint(point);
                    firstPoint = null!;
                }

                AppendTooltipData(points, point, i, data, ref startPoint, origin, isInverted);
            }
        }

        /// <summary>
        /// Processes a single visible point within the spline area series calculation.
        /// </summary>
        /// <param name="points">The points collection for the series.</param>
        /// <param name="point">The current point being processed.</param>
        /// <param name="isInverted">Indicates whether the chart axes are inverted.</param>
        /// <param name="origin">The baseline origin value for the area fill.</param>
        /// <param name="firstPoint">Reference to the first visible point in a contiguous segment.</param>
        /// <param name="data">Reference to the calculated point location (updated).</param>
        /// <param name="startPoint">Reference to the segment start location (updated).</param>
        private void ProcessVisiblePoint(List<Point> points, Point point, bool isInverted, double origin, ref Point firstPoint, ref ChartEventLocation data, ref ChartEventLocation startPoint)
        {
            int previous = GetPreviousIndex(points, point.Index - 1, Series ?? null!);

            if (firstPoint is not null)
            {
                data = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                _ = Direction.Append(GetSplineAreaDirection(DrawPoints[previous], data, isInverted, Series ?? null!));
            }
            else
            {
                startPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(origin), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                _ = Direction.Append("M " + startPoint.X.ToString(Culture) + SPACE + startPoint.Y.ToString(Culture) + SPACE);
                ChartEventLocation startPoint1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                _ = Direction.Append("L " + startPoint1.X.ToString(Culture) + SPACE + startPoint1.Y.ToString(Culture) + SPACE);
            }

            firstPoint = point;
            StorePointLocation(point, Series ?? null!, isInverted);
            _dataPoints = GetDataPoints(point.XValue, point.YValue, point.Regions);
        }

        /// <summary>
        /// Handles a point that is not visible by clearing symbol locations.
        /// </summary>
        /// <param name="point">The hidden point to clear locations for.</param>
        private void HandleHiddenPoint(Point point)
        {
            point.SymbolLocations = [];
            if (ChartPoints is { })
            {
                ChartPoints[point.Index].SymbolLocations = [];
            }
        }

        /// <summary>
        /// Appends tooltip-related data and finalizes segment closing when needed.
        /// </summary>
        /// <param name="points">The series points collection.</param>
        /// <param name="point">The current point.</param>
        /// <param name="index">The index of the current point.</param>
        /// <param name="data">The last calculated chart event location.</param>
        /// <param name="startPoint">Reference to the segment start point; may be updated.</param>
        /// <param name="origin">The baseline origin value.</param>
        /// <param name="isInverted">Whether axes are inverted.</param>
        private void AppendTooltipData(List<Point> points, Point point, int index, ChartEventLocation data, ref ChartEventLocation startPoint, double origin, bool isInverted)
        {
            if (((index + 1 < points.Count && !points[index + 1].Visible) || index == points.Count - 1) && data is not null && startPoint is not null)
            {
                startPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(origin), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                _ = Direction.Append("L " + startPoint.X.ToString(Culture) + SPACE + startPoint.Y.ToString(Culture) + SPACE);
            }
            IChartPoint chartPoint = ChartPoints?[point.Index] ?? null!;
            if (IsTooltipEnabled() && chartPoint.SymbolLocations.Count > 0)
            {
                AppendChartData(chartPoint);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Prepares render options for the spline area series and animation options when enabled.
        /// </summary>
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            string name = SeriesID();
            _options = new PathOptions()
            {
                Id = name,
                Fill = Interior ?? string.Empty,
                StrokeWidth = Series?.Border.Width ?? 0,
                Stroke = Series?.Border.Color ?? string.Empty,
                Opacity = Series?.Opacity ?? 1,
                StrokeDashArray = Series?.DashArray ?? string.Empty,
                Direction = Series?.Renderer.Points?.Count > 1 && Direction.Length != 0 ? Direction.ToString() : string.Empty,
                DataPoint = _dataPoints ?? string.Empty
            };

            if (Owner is not null && Owner._shouldAnimateSeries && ((Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        /// <summary>
        /// Returns the border path direction built from the main area direction path.
        /// </summary>
        /// <param name="seriesDirection">The main area series direction string.</param>
        /// <returns>The border path direction string.</returns>
        protected override string GetBorderDirection(string seriesDirection)
        {
            List<string> coordinates = [.. seriesDirection.Split(" ")];
            RemoveEmptyPointsBorder(coordinates);
            if (coordinates.Count > 4)
            {
                coordinates.RemoveRange(coordinates.Count - 4, 4);
            }
            return string.Join(" ", coordinates);
        }

        /// <summary>
        /// Removes empty point directions from the series direction of area types.
        /// </summary>
        /// <param name="coordinates">A list of direction tokens to sanitize.</param>
        protected override void RemoveEmptyPointsBorder(List<string> coordinates)
        {
            int startIndex = 0;
            int currentIndex;
            do
            {
                currentIndex = coordinates.FindIndex(startIndex, x => x.Contains('M', StringComparison));
                if (currentIndex > -1)
                {
                    coordinates.RemoveRange(currentIndex + 1, 3);
                    startIndex = currentIndex + 1;
                    if (currentIndex - 6 > 0)
                    {
                        coordinates[currentIndex] = "M";
                        coordinates.RemoveRange(currentIndex - 3, 3);
                        startIndex -= 3;
                    }
                }
            } while (currentIndex != -1);
        }

        /// <summary>
        /// Builds render tree for the spline area series elements.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> instance.</param>
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
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Recomputes direction and updates options with the current direction.
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
