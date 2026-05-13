﻿using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders stacking area series by computing path directions, border path and animation options.
    /// </summary>
    internal class StackingAreaSeriesRenderer : LineBaseSeriesRenderer
    {
        #region Fields
        private string? _dataPoints;
        #endregion

        #region Private Methods

        /// <summary>
        /// Appends the initial 'move to' command for the path based on the first visible point and origin.
        /// </summary>
        /// <param name="visiblePoints">The visible points list.</param>
        /// <param name="pointsLength">Total number of points.</param>
        /// <param name="origin">Y axis origin for start of area.</param>
        private void AppendInitialMove(List<Point> visiblePoints, int pointsLength, double origin)
        {
            if (pointsLength > 0)
            {
                ChartEventLocation point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoints[0].XValue), YAxisRenderer.GetPointValue(origin), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner is not null && Owner._requireInvertedAxis);
                _ = Direction.Append("M" + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
            }
        }

        /// <summary>
        /// Initializes point collections used for hit-testing and rendering.
        /// </summary>
        /// <param name="point">Point to initialize.</param>
        private void InitializePointContainers(Point point)
        {
            point.Regions = [];
            point.SymbolLocations = [];
            if (ChartPoints is not null)
            {
                ChartPoints[point.Index].SymbolLocations = [];
                ChartPoints[point.Index].Regions = [];
            }
        }

        /// <summary>
        /// Appends path segments for a visible point (normal stacking behavior).
        /// </summary>
        /// <param name="stackedvalue">Stacked values container.</param>
        /// <param name="point">Current point.</param>
        /// <param name="i">Index of current point.</param>
        private void AppendVisiblePointDirection(StackValues stackedvalue, Point point, int i)
        {
            if (Series is null)
            {
                return;
            }
            double startValue = GetStackingStartValue(point.Index, GetVisibleSeriesIndex());
            ChartEventLocation point1 = ChartHelper.GetPoint
            (
                XAxisRenderer.GetPointValue(point.XValue),
                YAxisRenderer.GetPointValue((!Series.Visible && Series._isLegendClicked) ? startValue : stackedvalue.EndValues[i]),
                XAxisRenderer,
                YAxisRenderer,
                XLength,
                YLength,
                Owner is not null && Owner._requireInvertedAxis
            );
            _ = Direction.Append("L" + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);

            point.SymbolLocations.Add(point1);
            point.SymbolLocations.ForEach(loc =>
            {
                ChartPoints?[point.Index]?.SymbolLocations.Add(new IChartInternalLocation(Math.Round(loc.X, 2), Math.Round(loc.Y, 2)));
            });

            point.Regions.Add(new Rect(point.SymbolLocations[0].X - Series.Marker.Width, point.SymbolLocations[0].Y - Series.Marker.Height, 2 * Series.Marker.Width, 2 * Series.Marker.Height));
            point.Regions.ForEach(rect =>
            {
                ChartPoints?[point.Index]?.Regions.Add(new IRect(Math.Round(rect.X, 2), Math.Round(rect.Y, 2), rect.Width, rect.Height));
            });
        }

        /// <summary>
        /// Appends path segments when encountering empty points and updates the start point for continued drawing.
        /// </summary>
        /// <param name="visiblePoints">List of visible points.</param>
        /// <param name="stackedvalue">Stacked values container.</param>
        /// <param name="startPoint">Reference start index used to draw back to baseline.</param>
        /// <param name="i">Current point index.</param>
        private void AppendEmptyPointDirection(List<Point> visiblePoints, StackValues stackedvalue, ref double startPoint, int i)
        {
            for (int j = i - 1; j >= startPoint; j--)
            {
                ChartEventLocation point2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoints[j].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[j]), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner is not null && Owner._requireInvertedAxis);
                _ = Direction.Append("L" + SPACE + point2.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE);
            }

            if (i + 1 < visiblePoints.Count && visiblePoints[i + 1] is not null && visiblePoints[i + 1].Visible)
            {
                ChartEventLocation point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoints[i + 1].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[i + 1]), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner is not null && Owner._requireInvertedAxis);
                _ = Direction.Append("M" + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
            }

            startPoint = i + 1;
        }

        /// <summary>
        /// Updates chart data used for tooltips and other data-driven features.
        /// </summary>
        /// <param name="point">Point to extract data from.</param>
        private void AppendPointData(Point point)
        {
            _dataPoints = GetDataPoints(point.XValue, point.YValue);
            if (IsTooltipEnabled() && point.SymbolLocations.Count > 0)
            {
                AppendChartData(ChartPoints?[point.Index]);
            }
        }

        /// <summary>
        /// Appends closing path segments to complete the stacking area border.
        /// </summary>
        /// <param name="visiblePoints">Visible points list.</param>
        /// <param name="pointsLength">Total points count.</param>
        /// <param name="startPoint">Start index for closing.</param>
        /// <param name="stackedvalue">Stacked values container.</param>
        private void AppendClosingDirection(List<Point> visiblePoints, int pointsLength, double startPoint, StackValues stackedvalue)
        {
            for (int j = pointsLength - 1; j >= startPoint; j--)
            {
                ChartSeries previousSeries = GetPreviousSeries(Series!);
                if (previousSeries.EmptyPointSettings.Mode != EmptyPointMode.Drop || (previousSeries.Renderer.Points != null && !previousSeries.Renderer.Points[j].IsEmpty))
                {
                    double startValue = GetStackingStartValue(visiblePoints[j].Index, GetVisibleSeriesIndex());
                    ChartEventLocation point2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoints[j].XValue), YAxisRenderer.GetPointValue((!Series.Visible && Series._isLegendClicked) ? startValue : stackedvalue.StartValues[j]), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner is not null && Owner._requireInvertedAxis);
                    double borderWidth = Series.Renderer.Index == 0 ? 0 : Series.Border.Width / 2;
                    _ = Direction.Append("L" + SPACE + point2.X.ToString(Culture) + SPACE + (point2.Y - borderWidth).ToString(Culture) + SPACE);
                }
            }
        }

        /// <summary>
        /// Finds the previous series in the owner's series container by renderer index.
        /// </summary>
        /// <param name="series">Current series.</param>
        /// <returns>Previous chart series when available; otherwise first series in container.</returns>
        private ChartSeries GetPreviousSeries(ChartSeries series)
        {
            List<ChartSeries> seriesCollection = Owner?._seriesContainer?.Elements.Cast<ChartSeries>().ToList() ?? null!;
            for (int i = 0, length = seriesCollection.Count; i < length; i++)
            {
                if (seriesCollection[i].Renderer is not null && series.Renderer is not null && series.Renderer.Index == seriesCollection[i].Renderer.Index && i != 0)
                {
                    return seriesCollection[i - 1];
                }
            }

            return seriesCollection[0];
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Renders series options and prepares animation when required.
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
                Opacity = Series?.Opacity ?? 0,
                StrokeDashArray = Series?.DashArray ?? string.Empty,
                Direction = Direction.ToString(),
                DataPoint = _dataPoints ?? string.Empty
            };

            if (Owner is not null && Owner._shouldAnimateSeries && ((Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        /// <summary>
        /// Builds border direction string from the main direction path.
        /// </summary>
        /// <param name="seriesDirection">Main series direction path.</param>
        /// <returns>Border path direction string.</returns>
        protected override string GetBorderDirection(string seriesDirection)
        {
            if (string.IsNullOrEmpty(seriesDirection))
            {
                return string.Empty;
            }

            List<string> coordinates = [.. seriesDirection.Split(" ")];
            if (coordinates.Count > 0)
            {
                int halfLength = (coordinates.Count / 2) + 1;
                coordinates.RemoveRange(halfLength, Math.Min(halfLength, coordinates.Count - halfLength));
                RemoveLastCoordainate(coordinates);
                RemoveEmptyPointsBorder(coordinates);
                RemoveLastCoordainate(coordinates);
            }
            return string.Join(" ", coordinates);
        }

        /// <summary>
        /// Removes a trailing command token if it is 'L' or 'M'.
        /// </summary>
        /// <param name="coordinates">Coordinate token list.</param>
        protected static void RemoveLastCoordainate(List<string> coordinates)
        {
            string last = coordinates[^1];
            if (last is "L" or "M")
            {
                coordinates.RemoveAt(coordinates.Count - 1);
            }
        }

        /// <summary>
        /// Renders SVG elements for the series (border + fill).
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect is null || builder is null || (Series != null && !Series.Visible && !Series._isLegendClicked))
            {
                return;
            }

            CreateSeriesElements(builder);
            if (Series is not null && Series.Visible)
            {
                SetBorderOptions();
                RenderSeriesElement(builder, _borderOptions ?? null!);
            }
            RenderSeriesElement(builder, _options ?? null!);
            builder.CloseElement();
            if (Series is not null && Series._isLegendClicked)
            {
                Series._isLegendClicked = false;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates series direction and notifies base renderer.
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
        /// Calculates the SVG path direction for the stacking area series.
        /// </summary>
        internal void CalculateDirection()
        {
            List<Point> visiblePoints = EnableComplexProperty();
            Direction = new System.Text.StringBuilder();
            ChartData = new System.Text.StringBuilder();
            if ((Series is not null && !Series.Visible && !Series._isLegendClicked) || Series?.Renderer.StackedValues is null || Series.Renderer.StackedValues.StartValues.Count == 0)
            {
                return;
            }

            int pointsLength = visiblePoints.Count;
            StackValues stackedvalue = Series.Renderer.StackedValues;
            double origin = Math.Max(YAxisRenderer?.VisibleRange.Start ?? 0, stackedvalue.StartValues[0]);
            double startPoint = 0;

            AppendInitialMove(visiblePoints, pointsLength, origin);

            for (int i = 0; i < visiblePoints.Count; i++)
            {
                Point point = visiblePoints[i];
                InitializePointContainers(point);
                Point previousPoint = i - 1 > -1 ? visiblePoints[i - 1] : null!;
                Point nextPoint = i + 1 < visiblePoints.Count ? visiblePoints[i + 1] : null!;

                if (point.Visible && ChartHelper.WithInRange(previousPoint, visiblePoints[i], nextPoint, XAxisRenderer))
                {
                    AppendVisiblePointDirection(stackedvalue, point, i);
                }
                else if (Series.EmptyPointSettings.Mode != EmptyPointMode.Drop)
                {
                    AppendEmptyPointDirection(visiblePoints, stackedvalue, ref startPoint, i);
                }
                AppendPointData(point);
            }
            AppendClosingDirection(visiblePoints, pointsLength, startPoint, stackedvalue);
        }
        #endregion
    }

    /// <summary>
    /// Marker class for 100% stacking area series. Inherits behavior from <see cref="StackingAreaSeriesRenderer"/>.
    /// </summary>
    internal class StackingArea100SeriesRenderer : StackingAreaSeriesRenderer
    {
    }
}