﻿using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders a stacking step area series for the chart. Calculates the SVG path (direction)
    /// for the series based on stacked start/end values, step position and empty point handling.
    /// </summary>
    /// <remarks>
    /// This renderer prepares PathOptions (fill, stroke, opacity, direction) used by the chart rendering pipeline.
    /// The renderer computes the top and bottom boundaries of the stacked area and generates
    /// move/line commands that represent stepped edges according to the configured StepPosition.
    /// </remarks>
    internal class StackingStepAreaSeriesRenderer : LineBaseSeriesRenderer
    {
        #region Fields
        private string? _datapoints;
        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates the SVG path direction for the stacking step area series.
        /// </summary>
        private void CalculateDirection()
        {
            bool isInverted = Owner is not null && Owner._requireInvertedAxis;
            InitializeBuilders();

            StackValues? stackedValues = Series?.Renderer?.StackedValues;
            if (!CanRenderStackingStepArea(stackedValues))
            {
                return;
            }

            StackValues stackedvalue = stackedValues!;
            List<Point> visiblePoint = EnableComplexProperty();
            double origin = Math.Max(YAxisRenderer?.VisibleRange.Start ?? 0, stackedvalue.StartValues[0]);
            double lineLength = GetCategoryLineLength();

            int startPoint = ProcessVisiblePoints(visiblePoint, stackedvalue, isInverted, lineLength, origin);
            CompleteAreaClosure(visiblePoint, stackedvalue, isInverted, lineLength, startPoint);
        }

        /// <summary>
        /// Initializes string builders used to assemble SVG direction and chart data.
        /// </summary>
        private void InitializeBuilders()
        {
            Direction = new System.Text.StringBuilder();
            ChartData = new System.Text.StringBuilder();
        }

        /// <summary>
        /// Determines whether the renderer has sufficient data to build a stacking step area.
        /// </summary>
        /// <param name="stackedValues">Stacked values container.</param>
        /// <returns><see langword="true"/> if rendering can proceed; otherwise <see langword="false"/>.</returns>
        private bool CanRenderStackingStepArea(StackValues? stackedValues)
        {
            return Series is not null && Series.Visible && stackedValues is not null && stackedValues.StartValues.Count != 0;
        }

        /// <summary>
        /// Gets category offset used when axis has labeled placement between ticks.
        /// </summary>
        /// <returns>Category offset length.</returns>
        private double GetCategoryLineLength()
        {
            return XAxisRenderer.Axis?.ValueType == ValueType.Category && XAxisRenderer.Axis.LabelPlacement == LabelPlacement.BetweenTicks
                ? 0.5
                : 0;
        }

        /// <summary>
        /// Processes the visible points collection and builds top-side path segments.
        /// </summary>
        /// <param name="visiblePoint">Visible points list.</param>
        /// <param name="stackedvalue">Stacked values container.</param>
        /// <param name="isInverted">Whether chart is inverted.</param>
        /// <param name="lineLength">Category line length offset.</param>
        /// <param name="origin">Baseline origin value.</param>
        /// <returns>Index of the start point for closing the area.</returns>
        private int ProcessVisiblePoints(List<Point> visiblePoint, StackValues stackedvalue, bool isInverted, double lineLength, double origin)
        {
            ChartEventLocation? start = null;
            Point? prevPoint = null;
            int startPoint = 0;
            int pointsLength = visiblePoint.Count;

            for (int i = 0; i < pointsLength; i++)
            {
                Point point = visiblePoint[i];
                PreparePointCollections(point);

                Point? nextPoint = i + 1 < pointsLength ? visiblePoint[i + 1] : null;
                int pointIndex = point.Index;
                double xValue = point.XValue;

                if (point.Visible && ChartHelper.WithInRange(i - 1 > -1 ? visiblePoint[i - 1] : null!, point, nextPoint!, XAxisRenderer))
                {
                    EnsureSeriesStart(ref start, xValue, origin, stackedvalue.EndValues[pointIndex], isInverted, lineLength);
                    AppendSegmentFromPrevious(point, prevPoint, stackedvalue, isInverted);
                    StoreMarkerInfo(point, stackedvalue, isInverted);
                    prevPoint = point;
                }

                if (ShouldHandleGap(nextPoint))
                {
                    startPoint = HandleGapSegment(visiblePoint, stackedvalue, isInverted, startPoint, i, ref start, ref prevPoint);
                }

                _datapoints = GetDataPoints(point.XValue, point.YValue);
                AppendTooltipData(point);
            }

            return startPoint;
        }

        /// <summary>
        /// Resets collection fields on the point to prepare for rendering.
        /// </summary>
        /// <param name="point">Target point.</param>
        private void PreparePointCollections(Point point)
        {
            point.SymbolLocations = [];
            point.Regions = [];
            if (ChartPoints is not null)
            {
                ChartPoints[point.Index].SymbolLocations = [];
                ChartPoints[point.Index].Regions = [];
            }
        }

        /// <summary>
        /// Appends move and initial line commands to begin a new area segment.
        /// </summary>
        /// <param name="start">Current series start chart event location reference.</param>
        /// <param name="xValue">X value for the current point.</param>
        /// <param name="origin">Baseline origin value.</param>
        /// <param name="stackedEndValue">Stacked end value at the point index.</param>
        /// <param name="isInverted">Whether chart is inverted.</param>
        /// <param name="lineLength">Category line length offset.</param>
        private void EnsureSeriesStart(ref ChartEventLocation? start, double xValue, double origin, double stackedEndValue, bool isInverted, double lineLength)
        {
            if (start is not null)
            {
                return;
            }

            start = new ChartEventLocation(xValue, 0);
            ChartEventLocation currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(xValue - lineLength), YAxisRenderer.GetPointValue(origin), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            AppendMoveCommand(currentPoint);
            currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(xValue - lineLength), YAxisRenderer.GetPointValue(stackedEndValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            AppendLineCommand(currentPoint);
        }

        /// <summary>
        /// Appends move command to the Direction builder for a given location.
        /// </summary>
        /// <param name="location">Chart event location to move to.</param>
        private void AppendMoveCommand(ChartEventLocation location)
        {
            _ = Direction.Append("M" + SPACE + location.X.ToString(Culture) + SPACE + location.Y.ToString(Culture) + SPACE);
        }

        /// <summary>
        /// Appends line command to the Direction builder for a given location.
        /// </summary>
        /// <param name="location">Chart event location to draw a line to.</param>
        private void AppendLineCommand(ChartEventLocation location)
        {
            _ = Direction.Append("L" + SPACE + location.X.ToString(Culture) + SPACE + location.Y.ToString(Culture) + SPACE);
        }

        /// <summary>
        /// Appends the top edge segment from the previous plotted point according to StepPosition.
        /// </summary>
        /// <param name="point">Current point.</param>
        /// <param name="prevPoint">Previous visible point if any.</param>
        /// <param name="stackedvalue">Stacked values container.</param>
        /// <param name="isInverted">Whether chart is inverted.</param>
        private void AppendSegmentFromPrevious(Point point, Point? prevPoint, StackValues stackedvalue, bool isInverted)
        {
            int pointIndex = point.Index;
            if (prevPoint is not null)
            {
                ChartEventLocation currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(stackedvalue.EndValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                ChartEventLocation secondPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(prevPoint.XValue), YAxisRenderer.GetPointValue(stackedvalue.EndValues[prevPoint.Index]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                _ = Direction.Append(GetStepLineDirection(secondPoint, currentPoint, Series!.StepPosition));
            }
            else if (Series!.EmptyPointSettings.Mode == EmptyPointMode.Gap)
            {
                AppendLineToCurrent(point, stackedvalue.EndValues[pointIndex], isInverted);
            }
        }

        /// <summary>
        /// Appends a simple line to current point's stacked value.
        /// </summary>
        /// <param name="point">The target point.</param>
        /// <param name="value">Stacked Y value.</param>
        /// <param name="isInverted">Whether chart axes are inverted.</param>
        private void AppendLineToCurrent(Point point, double value, bool isInverted)
        {
            ChartEventLocation currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(value), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            AppendLineCommand(currentPoint);
        }

        /// <summary>
        /// Stores computed symbol locations and region rectangles for a point.
        /// </summary>
        /// <param name="point">Target point.</param>
        /// <param name="stackedvalue">Stacked values container.</param>
        /// <param name="isInverted">Whether chart is inverted.</param>
        private void StoreMarkerInfo(Point point, StackValues stackedvalue, bool isInverted)
        {
            int pointIndex = point.Index;
            ChartEventLocation location = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(stackedvalue.EndValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            point.SymbolLocations.Add(location);
            point.SymbolLocations.ForEach(loc =>
            {
                ChartPoints?[point.Index]?.SymbolLocations.Add(new IChartInternalLocation(Math.Round(loc.X, 2), Math.Round(loc.Y, 2)));
            });

            point.Regions.Add(new Rect(location.X - Series!.Marker.Width, location.Y - Series.Marker.Height, 2 * Series.Marker.Width, 2 * Series.Marker.Height));
            point.Regions.ForEach(rect =>
            {
                ChartPoints?[point.Index]?.Regions.Add(new IRect(Math.Round(rect.X, 2), Math.Round(rect.Y, 2), rect.Width, rect.Height));
            });
        }

        /// <summary>
        /// Appends tooltip data for a point when tooltip is enabled.
        /// </summary>
        /// <param name="point">Target point.</param>
        private void AppendTooltipData(Point point)
        {
            if (IsTooltipEnabled() && point.SymbolLocations.Count > 0)
            {
                AppendChartData(ChartPoints?[point.Index]);
            }
        }

        /// <summary>
        /// Determines whether a gap-handling step should run for the next point.
        /// </summary>
        /// <param name="nextPoint">Next point in sequence.</param>
        /// <returns>True when a gap needs handling; otherwise false.</returns>
        private bool ShouldHandleGap(Point? nextPoint)
        {
            return Series!.EmptyPointSettings.Mode != EmptyPointMode.Drop && nextPoint is not null && !nextPoint.Visible;
        }

        /// <summary>
        /// Handles a segment that contains null/empty points and builds the appropriate bottom-side path back to start.
        /// </summary>
        /// <param name="visiblePoint">Visible points list.</param>
        /// <param name="stackedvalue">Stacked values container.</param>
        /// <param name="isInverted">Whether chart is inverted.</param>
        /// <param name="startPoint">Current start point index.</param>
        /// <param name="currentIndex">Index where a gap is encountered.</param>
        /// <param name="start">Reference to current series start location.</param>
        /// <param name="prevPoint">Reference to previous visible point.</param>
        /// <returns>New start point index after closing the gap.</returns>
        private int HandleGapSegment(List<Point> visiblePoint, StackValues stackedvalue, bool isInverted, int startPoint, int currentIndex, ref ChartEventLocation? start, ref Point? prevPoint)
        {
            ChartEventLocation? gapPreviousPoint = null;
            ChartEventLocation currentPoint;

            for (int j = currentIndex; j > startPoint; j--)
            {
                int pointIndex = visiblePoint[j].Index;
                int previousPointIndex = j == 0 ? 0 : visiblePoint[j - 1].Index;

                if (j != 0 && (stackedvalue.StartValues[pointIndex] < stackedvalue.StartValues[previousPointIndex] || stackedvalue.StartValues[pointIndex] > stackedvalue.StartValues[previousPointIndex]))
                {
                    currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[pointIndex].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                    AppendLineCommand(currentPoint);
                    if (visiblePoint[previousPointIndex].Visible)
                    {
                        // Need to calculate step position when null points is given in _datapoints
                        if (Series!.StepPosition == StepPosition.Right)
                        {
                            currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[previousPointIndex].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        }
                        else if (Series.StepPosition == StepPosition.Center)
                        {
                            currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[pointIndex].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                            gapPreviousPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[previousPointIndex].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[previousPointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        }
                        else
                        {
                            currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[pointIndex].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[previousPointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        }
                    }
                }
                else
                {
                    currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[pointIndex].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                }

                AppendGapStepLine(currentPoint, gapPreviousPoint);
            }

            if (startPoint == 0)
            {
                currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[startPoint].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[startPoint]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                AppendLineCommand(currentPoint);
            }

            startPoint = currentIndex + 1;
            start = null;
            prevPoint = null;
            return startPoint;
        }

        /// <summary>
        /// Appends the step line for gap handling, supporting center step behaviour.
        /// </summary>
        /// <param name="currentPoint">Current endpoint location.</param>
        /// <param name="previousPoint">Optional previous endpoint for center step.</param>
        private void AppendGapStepLine(ChartEventLocation currentPoint, ChartEventLocation? previousPoint)
        {
            if (Series!.StepPosition == StepPosition.Center && previousPoint is not null)
            {
                double midPoint = previousPoint.X + ((currentPoint.X - previousPoint.X) / 2);
                _ = Direction.Append("L" + SPACE + midPoint.ToString(Culture) + SPACE + currentPoint.Y.ToString(Culture) + SPACE);
                _ = Direction.Append("L" + SPACE + midPoint.ToString(Culture) + SPACE + previousPoint.Y.ToString(Culture) + SPACE);
            }
            else
            {
                AppendLineCommand(currentPoint);
            }
        }

        /// <summary>
        /// Completes the area closure by tracing the bottom boundary and adding final line commands.
        /// </summary>
        /// <param name="visiblePoint">Visible points list.</param>
        /// <param name="stackedvalue">Stacked values container.</param>
        /// <param name="isInverted">Whether chart is inverted.</param>
        /// <param name="lineLength">Category line length offset.</param>
        /// <param name="startPoint">Start index for closing the area.</param>
        private void CompleteAreaClosure(List<Point> visiblePoint, StackValues stackedvalue, bool isInverted, double lineLength, int startPoint)
        {
            if (Direction.Length == 0 || visiblePoint.Count == 0)
            {
                return;
            }

            int pointsLength = visiblePoint.Count;
            int pointIndex;
            ChartEventLocation secondPoint;
            ChartEventLocation point2 = null!;

            if (pointsLength > 1)
            {
                pointIndex = visiblePoint[pointsLength - 1].Index;
                ChartEventLocation start = new(visiblePoint[pointsLength - 1].XValue + lineLength, stackedvalue.EndValues[pointIndex]);
                secondPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(start.X), YAxisRenderer.GetPointValue(start.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                AppendLineCommand(secondPoint);
                start = new ChartEventLocation(visiblePoint[pointsLength - 1].XValue + lineLength, stackedvalue.StartValues[pointIndex]);
                secondPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(start.X), YAxisRenderer.GetPointValue(start.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                AppendLineCommand(secondPoint);
            }

            for (int j = pointsLength - 1; j > startPoint; j--)
            {
                if (visiblePoint[j].Visible)
                {
                    pointIndex = visiblePoint[j].Index;
                    point2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[j].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                    AppendLineCommand(point2);
                }

                if (j is 0 or 0 || visiblePoint[j - 1].Visible)
                {
                    _ = Direction.Append(GetStackedStepLineDirection(ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[j - 1].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[visiblePoint[j - 1].Index]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted), point2, Series!.StepPosition));
                }
                if (j - 1 == 0)
                {
                    point2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[j - 1].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[visiblePoint[j - 1].Index]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                    AppendLineCommand(point2);
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds options for rendering the series path and sets up animation when enabled.
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
                Direction = Direction.ToString(),
                DataPoint = _datapoints ?? string.Empty
            };

            if (Owner is not null && Owner._shouldAnimateSeries && ((Series is not null && Series.Animation.Enable && SyncfusionService?._options.Animation == GlobalAnimationMode.Default) || (SyncfusionService?._options.Animation == GlobalAnimationMode.Enable)))
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        /// <summary>
        /// Renders the series element into the provided RenderTreeBuilder.
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
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
        /// Returns the stacked step line direction string between two points according to step position.
        /// </summary>
        /// <param name="point1">First point location.</param>
        /// <param name="point2">Second point location.</param>
        /// <param name="stepPosition">Step position mode.</param>
        /// <returns>SVG path segment commands.</returns>
        internal string GetStackedStepLineDirection(ChartEventLocation point1, ChartEventLocation point2, StepPosition stepPosition)
        {
            return stepPosition == StepPosition.Right
                ? "L" + SPACE + point1.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE
                : stepPosition == StepPosition.Center
                    ? "L" + SPACE + (point1.X + ((point2.X - point1.X) / 2)).ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE + "L" + SPACE + (point1.X + ((point2.X - point1.X) / 2)).ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE
                    : "L" + SPACE + point2.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE;
        }

        /// <summary>
        /// Recomputes the direction and updates cached options.
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
