using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    internal partial class CartesianAxisLayout
    {
        #region Private Methods

        /// <summary>
        /// Adds a path option to the axis grid dictionary, creating the list if needed.
        /// </summary>
        /// <param name="renderer">The <see cref="ChartAxisRenderer"/> containing the grid options.</param>
        /// <param name="key">The dictionary key (e.g., Constants.MajorGridLine).</param>
        /// <param name="value">The <see cref="PathOptions"/> to add.</param>
        private static void LoadDictionaryValue(ChartAxisRenderer renderer, string key, PathOptions value)
        {
            if (!renderer.AxisRenderInfo.AxisGridOptions.TryGetValue(key, out List<PathOptions>? _))
            {
                renderer.AxisRenderInfo.AxisGridOptions.Add(key, []);
            }

            renderer.AxisRenderInfo.AxisGridOptions[key].Add(value);
        }

        /// <summary>
        /// Calculates and renders the axis line (backbone) based on visibility and styling.
        /// </summary>
        /// <param name="axis">The <see cref="ChartAxis"/> to render.</param>
        /// <param name="renderer">The <see cref="ChartAxisRenderer"/> providing rendering context.</param>
        private void CalculateAxisLine(ChartAxis axis, ChartAxisRenderer renderer)
        {
            if (!axis.Visible || axis.LineStyle.Width <= 0)
            {
                return;
            }

            string direction = renderer.Orientation == Orientation.Vertical ? BuildVerticalAxisLine(renderer, axis) : BuildHorizontalAxisLine(renderer, axis);

            renderer.AxisRenderInfo.AxisLine = new PathOptions
            {
                Id = renderer.Owner?.ID + "AxisLine_" + renderer.Index,
                Direction = direction,
                StrokeDashArray = axis.LineStyle.DashArray,
                StrokeWidth = axis.LineStyle.Width,
                Stroke = !string.IsNullOrEmpty(axis.LineStyle.Color) ? axis.LineStyle.Color : Chart?._chartThemeStyle?.AxisLine ?? string.Empty
            };
        }

        /// <summary>
        /// Builds the SVG path string for a vertical (Y) axis line.
        /// </summary>
        /// <param name="renderer">The axis renderer containing position information.</param>
        /// <param name="axis">The axis configuration.</param>
        /// <returns>An SVG path string representing the vertical axis line.</returns>
        private string BuildVerticalAxisLine(ChartAxisRenderer renderer, ChartAxis axis)
        {
            return "M " + renderer.UpdatedRect.X.ToString(Culture) + SPACE + (renderer.UpdatedRect.Y - axis.PlotOffset).ToString(Culture) +
                   " L " + (renderer.UpdatedRect.X + renderer.UpdatedRect.Width).ToString(Culture) + SPACE +
                   (renderer.UpdatedRect.Y + renderer.UpdatedRect.Height + axis.PlotOffset).ToString(Culture);
        }

        /// <summary>
        /// Builds the SVG path string for a horizontal (X) axis line.
        /// </summary>
        /// <param name="renderer">The axis renderer containing position information.</param>
        /// <param name="axis">The axis configuration.</param>
        /// <returns>An SVG path string representing the horizontal axis line.</returns>
        private string BuildHorizontalAxisLine(ChartAxisRenderer renderer, ChartAxis axis)
        {
            return "M " + (renderer.UpdatedRect.X - axis.PlotOffset).ToString(Culture) + SPACE + renderer.UpdatedRect.Y.ToString(Culture) +
                   " L " + (renderer.UpdatedRect.X + renderer.UpdatedRect.Width + axis.PlotOffset).ToString(Culture) + SPACE +
                   (renderer.UpdatedRect.Y + renderer.UpdatedRect.Height).ToString(Culture);
        }

        /// <summary>
        /// Calculates X-axis grid lines, major/minor, and ticks.
        /// </summary>
        /// <param name="axis">The axis to calculate.</param>
        /// <param name="index">The axis index.</param>
        /// <param name="rect">The axis rectangle.</param>
        /// <remarks>
        /// This method generates major and minor grid lines and tick marks for the X-axis.
        /// It handles category and date-time category axes with special label placement options.
        /// </remarks>
        private void CalculateXAxisGridLine(ChartAxis axis, int index, Rect rect)
        {
            if (axis.Renderer is null)
            {
                return;
            }

            XAxisGridContext context = CreateXAxisGridContext(axis, rect);
            List<VisibleLabels> visibleLabels = axis.Renderer.VisibleLabels;
            int length = CalculateGridLineLength(axis, visibleLabels.Count);

            for (int i = 0; i < length; i++)
            {
                double tempInterval = CalculateXAxisInterval(axis, visibleLabels, i, context.TicksBetweenLabel);
                double pointX = CalculateXAxisPoint(axis, rect, tempInterval);

                if (IsPointInRange(pointX, rect.X, rect.X + rect.Width))
                {
                    ProcessXAxisGridPoint(axis, index, rect, i, tempInterval, pointX, context);
                }
            }
        }

        /// <summary>
        /// Creates the context object for X-axis grid line calculation.
        /// </summary>
        /// <param name="axis">The axis configuration.</param>
        /// <param name="rect">The axis rectangle.</param>
        /// <returns>An <see cref="XAxisGridContext"/> containing calculation parameters.</returns>
        private static XAxisGridContext CreateXAxisGridContext(ChartAxis axis, Rect rect)
        {
            bool isOpposed = axis.IsAxisOpposedPosition;
            bool isTickInside = axis.TickPosition == AxisPosition.Inside;
            bool isCategoryType = axis.ValueType is ValueType.Category or ValueType.DateTimeCategory;

            double tickSize = isOpposed ? -axis.Renderer?.MajorTickLinesHeight ?? 0 : axis.Renderer?.MajorTickLinesHeight ?? 0;
            double axisLineSize = isOpposed ? -axis.LineStyle.Width * 0.5 : axis.LineStyle.Width * 0.5;
            double scrollBarHeight = axis.CrossesAt is null ? (isOpposed ? -axis.ScrollBarHeight : axis.ScrollBarHeight) : 0;
            double ticksBetweenLabel = (isCategoryType && axis.LabelPlacement == LabelPlacement.BetweenTicks) ? 0.5 : 0;
            double ticks = isTickInside ? (rect.Y - tickSize - axisLineSize) : (rect.Y + tickSize + axisLineSize + scrollBarHeight);

            return new XAxisGridContext
            {
                Ticks = ticks,
                AxisLineSize = axisLineSize,
                IsTickInside = isTickInside,
                ScrollBarHeight = scrollBarHeight,
                TicksBetweenLabel = ticksBetweenLabel
            };
        }

        /// <summary>
        /// Calculates the grid line length based on axis type and label placement.
        /// </summary>
        /// <param name="axis">The axis configuration.</param>
        /// <param name="visibleLabelCount">The number of visible labels.</param>
        /// <returns>The calculated grid line length.</returns>
        private static int CalculateGridLineLength(ChartAxis axis, int visibleLabelCount)
        {
            bool isCategoryType = axis.ValueType is ValueType.Category or ValueType.DateTimeCategory;

            return isCategoryType && visibleLabelCount > 0 && axis.LabelPlacement == LabelPlacement.BetweenTicks
                ? visibleLabelCount + 1
                : visibleLabelCount;
        }

        /// <summary>
        /// Calculates the interval value for a specific X-axis point.
        /// </summary>
        private double CalculateXAxisInterval(ChartAxis axis, List<VisibleLabels> visibleLabels, int index, double ticksBetweenLabel)
        {
            return axis.ValueType != ValueType.DateTimeCategory
                ? index < visibleLabels.Count
                    ? (visibleLabels[index]?.Value ?? (visibleLabels[index - 1].Value + (axis.Renderer?.VisibleInterval ?? 0))) - ticksBetweenLabel
                    : visibleLabels[index - 1].Value + (axis.Renderer?.VisibleInterval ?? 0) - ticksBetweenLabel
                : index < visibleLabels.Count
                ? (visibleLabels[index]?.Value ?? axis.Renderer?.VisibleRange.End ?? 0) - ticksBetweenLabel
                : axis.Renderer?.VisibleRange.End ?? 0;
        }

        /// <summary>
        /// Calculates the X coordinate for an axis point.
        /// </summary>
        private static double CalculateXAxisPoint(ChartAxis axis, Rect rect, double interval)
        {
            return (ChartHelper.ValueToCoefficient(interval, axis.Renderer ?? null!) * rect.Width) + rect.X;
        }

        /// <summary>
        /// Checks if a point is within the specified range.
        /// </summary>
        private static bool IsPointInRange(double point, double min, double max)
        {
            return point >= min && point <= max;
        }

        /// <summary>
        /// Processes a single X-axis grid point (major grid/tick and minor lines).
        /// </summary>
        private void ProcessXAxisGridPoint(ChartAxis axis, int index, Rect rect, int labelIndex, double tempInterval, double pointX, XAxisGridContext context)
        {
            if (ShouldRenderMajorGrid(axis, labelIndex, tempInterval, pointX))
            {
                RenderXAxisMajorGrid(axis, index, labelIndex, pointX);
            }

            RenderXAxisMajorTick(axis, index, rect, labelIndex, pointX, context);

            if (ShouldRenderMinorLines(axis))
            {
                RenderXAxisMinorLines(axis, index, rect, labelIndex, tempInterval);
            }
        }

        /// <summary>
        /// Determines if major grid should be rendered for the current point.
        /// </summary>
        private bool ShouldRenderMajorGrid(ChartAxis axis, int index, double interval, double pointX)
        {
            return axis.Renderer is not null && (ChartHelper.Inside(interval, axis.Renderer.VisibleRange) || IsBorder(axis, index, pointX));
        }

        /// <summary>
        /// Renders the major grid line for an X-axis point.
        /// </summary>
        private void RenderXAxisMajorGrid(ChartAxis axis, int axisIndex, int labelIndex, double pointX)
        {
            string majorGridId = axis.Renderer?.Owner?.ID + "_MajorGridLine_" + axisIndex + "_" + labelIndex;
            string majorGridDirection = BuildXAxisMajorGridPath(pointX);

            _ = ChartHelper.AppendPathElements(axis.Renderer?.Owner ?? null!, majorGridDirection, majorGridId);

            if (axis.Renderer?.MajorGridLinesWidth > 0 && axis.Visible)
            {
                LoadDictionaryValue(axis.Renderer, Constants.MajorGridLine, CreateMajorGridPathOptions(majorGridId, majorGridDirection, axis));
            }
        }

        /// <summary>
        /// Builds the SVG path for an X-axis major grid line.
        /// </summary>
        private string BuildXAxisMajorGridPath(double pointX)
        {
            return "M " + pointX.ToString(Culture) + SPACE + ((SeriesClipRect?.Y ?? 0) + (SeriesClipRect?.Height ?? 0)).ToString(Culture) + " L " + pointX.ToString(Culture) + SPACE + SeriesClipRect?.Y.ToString(Culture);
        }

        /// <summary>
        /// Creates path options for a major grid line.
        /// </summary>
        private PathOptions CreateMajorGridPathOptions(string id, string direction, ChartAxis axis)
        {
            return new PathOptions
            {
                Id = id,
                Direction = direction,
                StrokeDashArray = axis.MajorGridLines.DashArray,
                StrokeWidth = axis.Renderer?.MajorGridLinesWidth ?? 0,
                Stroke = !string.IsNullOrEmpty(axis.MajorGridLines.Color) ? axis.MajorGridLines.Color : Chart?._chartThemeStyle?.MajorGridLine ?? string.Empty
            };
        }

        /// <summary>
        /// Renders the major tick line for an X-axis point.
        /// </summary>
        private void RenderXAxisMajorTick(ChartAxis axis, int axisIndex, Rect rect, int labelIndex, double pointX, XAxisGridContext context)
        {
            string majorTickId = axis.Renderer?.Owner?.ID + "_MajorTickLine_" + axisIndex + "_" + labelIndex;
            string majorTickDirection = BuildXAxisMajorTickPath(pointX, rect, context);

            _ = ChartHelper.AppendPathElements(axis.Renderer?.Owner ?? null!, majorTickDirection, majorTickId);

            if (axis.Renderer?.MajorTickLinesWidth > 0 && axis.Visible)
            {
                LoadDictionaryValue(axis.Renderer, Constants.MajorTickLine, CreateMajorTickPathOptions(majorTickId, majorTickDirection, axis));
            }
        }

        /// <summary>
        /// Builds the SVG path for an X-axis major tick line.
        /// </summary>
        private string BuildXAxisMajorTickPath(double pointX, Rect rect, XAxisGridContext context)
        {
            return "M " + pointX.ToString(Culture) + SPACE +
                   (rect.Y + context.AxisLineSize + (context.IsTickInside ? context.ScrollBarHeight : 0)).ToString(Culture) +
                   " L " + pointX.ToString(Culture) + SPACE + context.Ticks.ToString(Culture);
        }

        /// <summary>
        /// Creates path options for a major tick line.
        /// </summary>
        private PathOptions CreateMajorTickPathOptions(string id, string direction, ChartAxis axis)
        {
            return new PathOptions
            {
                Id = id,
                Direction = direction,
                StrokeWidth = axis.Renderer?.MajorTickLinesWidth ?? 0,
                Stroke = !string.IsNullOrEmpty(axis.MajorTickLines.Color) ? axis.MajorTickLines.Color : Chart?._chartThemeStyle?.MajorTickLine ?? string.Empty
            };
        }

        /// <summary>
        /// Determines if minor lines should be rendered.
        /// </summary>
        private static bool ShouldRenderMinorLines(ChartAxis axis)
        {
            return axis.MinorTicksPerInterval > 0 && (axis.MinorGridLines.Width > 0 || axis.MinorTickLines.Width > 0);
        }

        /// <summary>
        /// Renders minor grid and tick lines for an X-axis point.
        /// </summary>
        private void RenderXAxisMinorLines(ChartAxis axis, int axisIndex, Rect rect, int labelIndex, double tempInterval)
        {
            string[] minorDirection = CalculateAxisMinorLine(axis, tempInterval, rect, labelIndex);

            RenderMinorGridLine(axis, axisIndex, labelIndex, minorDirection[0]);
            RenderMinorTickLine(axis, axisIndex, labelIndex, minorDirection[1]);
        }

        /// <summary>
        /// Renders a minor grid line.
        /// </summary>
        private void RenderMinorGridLine(ChartAxis axis, int axisIndex, int labelIndex, string direction)
        {
            if (string.IsNullOrEmpty(direction))
            {
                return;
            }

            string minorId = axis.Renderer?.Owner?.ID + "_MinorGridLine_" + axisIndex + "_" + labelIndex;
            _ = ChartHelper.AppendPathElements(axis.Renderer?.Owner ?? null!, direction, minorId);

            if (axis.MinorGridLines.Width > 0 && axis.Visible)
            {
                LoadDictionaryValue(axis.Renderer ?? null!, Constants.MinorGridLine, new PathOptions
                {
                    Id = minorId,
                    Direction = direction,
                    StrokeWidth = axis.MinorGridLines.Width,
                    Stroke = !string.IsNullOrEmpty(axis.MinorGridLines.Color) ? axis.MinorGridLines.Color : Chart?._chartThemeStyle?.MinorGridLine ?? string.Empty,
                    StrokeDashArray = axis.MinorGridLines.DashArray
                });
            }
        }

        /// <summary>
        /// Renders a minor tick line.
        /// </summary>
        private void RenderMinorTickLine(ChartAxis axis, int axisIndex, int labelIndex, string direction)
        {
            if (string.IsNullOrEmpty(direction))
            {
                return;
            }

            string minorId = axis.Renderer?.Owner?.ID + "_MinorTickLine_" + axisIndex + "_" + labelIndex;
            _ = ChartHelper.AppendPathElements(axis.Renderer?.Owner ?? null!, direction, minorId);

            if (axis.MinorTickLines.Width > 0 && axis.Visible)
            {
                LoadDictionaryValue(axis.Renderer ?? null!, Constants.MinorTickLine, new PathOptions
                {
                    Id = minorId,
                    Direction = direction,
                    StrokeWidth = axis.MinorTickLines.Width,
                    Stroke = !string.IsNullOrEmpty(axis.MinorTickLines.Color) ? axis.MinorTickLines.Color : Chart?._chartThemeStyle?.MinorTickLine ?? string.Empty
                });
            }
        }

        /// <summary>
        /// Calculates Y-axis grid lines, major/minor, and ticks.
        /// </summary>
        /// <param name="axis">The axis to calculate.</param>
        /// <param name="index">The axis index.</param>
        /// <param name="rect">The axis rectangle.</param>
        /// <remarks>
        /// This method generates major and minor grid lines and tick marks for the Y-axis.
        /// It handles category axes with special label placement options.
        /// </remarks>
        private void CalculateYAxisGridLine(ChartAxis axis, int index, Rect rect)
        {
            YAxisGridContext context = CreateYAxisGridContext(axis, rect);
            double length = axis.Renderer?.VisibleLabels.Count ?? 0;

            if (ShouldAddExtraLabelForCategory(axis, length))
            {
                length += 1;
            }

            for (int i = 0; i < length; i++)
            {
                double tempInterval = CalculateYAxisInterval(axis, i, context.TicksBetweenLabel);
                double pointY = CalculateYAxisPoint(axis, rect, tempInterval);

                if (IsPointInRange(pointY, rect.Y, rect.Y + rect.Height))
                {
                    ProcessYAxisGridPoint(axis, index, rect, i, tempInterval, pointY, context);
                }
            }
        }

        /// <summary>
        /// Creates the context object for Y-axis grid line calculation.
        /// </summary>
        private static YAxisGridContext CreateYAxisGridContext(ChartAxis axis, Rect rect)
        {
            bool isOpposed = axis.IsAxisOpposedPosition;
            bool isTickInside = axis.TickPosition == AxisPosition.Inside;
            bool isCategoryType = axis.ValueType is ValueType.Category or ValueType.DateTimeCategory;

            double tickSize = isOpposed ? axis.Renderer?.MajorTickLinesHeight ?? 0 : -axis.Renderer?.MajorTickLinesHeight ?? 0;
            double axisLineSize = isOpposed ? axis.LineStyle.Width * 0.5 : -axis.LineStyle.Width * 0.5;
            double scrollBarHeight = (axis.CrossesAt is null) ? (isOpposed ? axis.ScrollBarHeight : -axis.ScrollBarHeight) : 0;
            double ticksBetweenLabel = (isCategoryType && axis.LabelPlacement == LabelPlacement.BetweenTicks) ? 0.5 : 0;
            double ticks = isTickInside
                ? (rect.X - tickSize - axisLineSize)
                : (rect.X + tickSize + axisLineSize + scrollBarHeight);

            return new YAxisGridContext
            {
                Ticks = ticks,
                AxisLineSize = axisLineSize,
                TickSize = tickSize,
                ScrollBarHeight = scrollBarHeight,
                IsTickInside = isTickInside,
                IsCategoryType = isCategoryType,
                TicksBetweenLabel = ticksBetweenLabel
            };
        }

        /// <summary>
        /// Determines if an extra label should be added for category axes.
        /// </summary>
        private static bool ShouldAddExtraLabelForCategory(ChartAxis axis, double labelCount)
        {
            bool isCategoryType = axis.ValueType is ValueType.Category or ValueType.DateTimeCategory;
            return isCategoryType && axis.LabelPlacement == LabelPlacement.BetweenTicks && labelCount > 0;
        }

        /// <summary>
        /// Calculates the interval value for a specific Y-axis point.
        /// </summary>
        private static double CalculateYAxisInterval(ChartAxis axis, int index, double ticksBetweenLabel)
        {
            return index < axis.Renderer?.VisibleLabels.Count
                ? axis.Renderer.VisibleLabels[index] is not null
                    ? axis.Renderer.VisibleLabels[index].Value - ticksBetweenLabel
                    : axis.Renderer.VisibleLabels[index - 1].Value + axis.Renderer.VisibleInterval - ticksBetweenLabel
                : (axis.Renderer?.VisibleLabels[index - 1].Value ?? 0) + (axis.Renderer?.VisibleInterval ?? 0) - ticksBetweenLabel;
        }

        /// <summary>
        /// Calculates the Y coordinate for an axis point.
        /// </summary>
        private static double CalculateYAxisPoint(ChartAxis axis, Rect rect, double interval)
        {
            double pointY = ChartHelper.ValueToCoefficient(interval, axis.Renderer ?? null!) * rect.Height;
            return (pointY * -1) + (rect.Y + rect.Height);
        }

        /// <summary>
        /// Processes a single Y-axis grid point (major grid/tick and minor lines).
        /// </summary>
        private void ProcessYAxisGridPoint(ChartAxis axis, int index, Rect rect, int labelIndex,
            double tempInterval, double pointY, YAxisGridContext context)
        {
            if (ShouldRenderYAxisMajorGrid(axis, labelIndex, tempInterval, pointY))
            {
                RenderYAxisMajorGrid(axis, index, labelIndex, pointY);
            }

            RenderYAxisMajorTick(axis, index, rect, labelIndex, pointY, context);

            if (ShouldRenderMinorLines(axis))
            {
                RenderYAxisMinorLines(axis, index, rect, labelIndex, tempInterval);
            }
        }

        /// <summary>
        /// Determines if major grid should be rendered for the Y-axis point.
        /// </summary>
        private bool ShouldRenderYAxisMajorGrid(ChartAxis axis, int index, double interval, double pointY)
        {
            return (axis.Renderer is not null && ChartHelper.Inside(interval, axis.Renderer.VisibleRange)) || IsBorder(axis, index, pointY);
        }

        /// <summary>
        /// Renders the major grid line for a Y-axis point.
        /// </summary>
        private void RenderYAxisMajorGrid(ChartAxis axis, int axisIndex, int labelIndex, double pointY)
        {
            string majorGridId = axis.Renderer?.Owner?.ID + "_MajorGridLine_" + axisIndex + "_" + labelIndex;
            string majorGridDirection = BuildYAxisMajorGridPath(pointY);

            _ = ChartHelper.AppendPathElements(axis.Renderer?.Owner ?? null!, majorGridDirection, majorGridId);

            if (axis.Renderer?.MajorGridLinesWidth > 0 && axis.Visible)
            {
                LoadDictionaryValue(axis.Renderer, Constants.MajorGridLine, CreateMajorGridPathOptions(majorGridId, majorGridDirection, axis));
            }
        }

        /// <summary>
        /// Builds the SVG path for a Y-axis major grid line.
        /// </summary>
        private string BuildYAxisMajorGridPath(double pointY)
        {
            return "M " + SeriesClipRect?.X.ToString(Culture) + SPACE + pointY.ToString(Culture) +
                   " L " + ((SeriesClipRect?.X ?? 0) + (SeriesClipRect?.Width ?? 0)).ToString(Culture) + SPACE +
                   pointY.ToString(Culture);
        }

        /// <summary>
        /// Renders the major tick line for a Y-axis point.
        /// </summary>
        private void RenderYAxisMajorTick(ChartAxis axis, int axisIndex, Rect rect, int labelIndex, double pointY, YAxisGridContext context)
        {
            string majorTickId = axis.Renderer?.Owner?.ID + "_MajorTickLine_" + axisIndex + "_" + labelIndex;
            string majorTickDirection = BuildYAxisMajorTickPath(pointY, rect, context);

            _ = ChartHelper.AppendPathElements(axis.Renderer?.Owner ?? null!, majorTickDirection, majorTickId);

            if (axis.Renderer?.MajorTickLinesWidth > 0 && axis.Visible)
            {
                LoadDictionaryValue(axis.Renderer, Constants.MajorTickLine, CreateMajorTickPathOptions(majorTickId, majorTickDirection, axis));
            }
        }

        /// <summary>
        /// Builds the SVG path for a Y-axis major tick line.
        /// </summary>
        private string BuildYAxisMajorTickPath(double pointY, Rect rect, YAxisGridContext context)
        {
            return "M " + (rect.X + context.AxisLineSize + (context.IsTickInside ? context.ScrollBarHeight : 0)).ToString(Culture) +
                   SPACE + pointY.ToString(Culture) + " L " + context.Ticks.ToString(Culture) + SPACE + pointY.ToString(Culture);
        }

        /// <summary>
        /// Renders minor grid and tick lines for a Y-axis point.
        /// </summary>
        private void RenderYAxisMinorLines(ChartAxis axis, int axisIndex, Rect rect, int labelIndex, double tempInterval)
        {
            string[] minorDirection = CalculateAxisMinorLine(axis, tempInterval, rect, labelIndex);

            RenderMinorGridLine(axis, axisIndex, labelIndex, minorDirection[0]);
            RenderMinorTickLine(axis, axisIndex, labelIndex, minorDirection[1]);
        }

        /// <summary>
        /// Calculates minor grid/tick lines for a given major interval.
        /// </summary>
        /// <param name="axis">The axis configuration.</param>
        /// <param name="tempInterval">The major interval value.</param>
        /// <param name="rect">The axis rectangle.</param>
        /// <param name="labelIndex">The label index.</param>
        /// <returns>An array with minor grid direction and minor tick direction.</returns>
        private string[] CalculateAxisMinorLine(ChartAxis axis, double tempInterval, Rect rect, int labelIndex)
        {
            MinorLineContext context = CreateMinorLineContext(axis, tempInterval);
            List<string> directions = new(capacity: 2);

            if (axis.Renderer?.Orientation == Orientation.Horizontal)
            {
                CalculateHorizontalMinorLines(axis, rect, labelIndex, context, directions);
            }
            else
            {
                CalculateVerticalMinorLines(axis, rect, labelIndex, context, directions);
            }

            return [.. directions];
        }

        /// <summary>
        /// Creates the context for minor line calculation (tick direction/size and logarithmic parameters).
        /// </summary>
        private static MinorLineContext CreateMinorLineContext(ChartAxis axis, double tempInterval)
        {
            bool isTickInside = axis.TickPosition == AxisPosition.Inside;
            double tickSize = axis.IsAxisOpposedPosition ? -axis.MinorTickLines.Height : axis.MinorTickLines.Height;

            double logInterval = 1d;
            double logPosition = 1d;

            if (axis.ValueType == ValueType.Logarithmic)
            {
                double visibleInterval = axis.Renderer?.VisibleInterval ?? 0d;
                double logStart = Math.Pow(axis.LogBase, tempInterval - visibleInterval);
                logInterval = (Math.Pow(axis.LogBase, tempInterval) - logStart) / (axis.MinorTicksPerInterval + 1);
                logPosition = logStart + logInterval;
            }

            return new MinorLineContext
            {
                TickSize = tickSize,
                IsTickInside = isTickInside,
                LogInterval = logInterval,
                LogPosition = logPosition,
                TempInterval = tempInterval
            };
        }

        /// <summary>
        /// Calculates minor lines for horizontal orientation. Preserves the original order of operations.
        /// </summary>
        private void CalculateHorizontalMinorLines(ChartAxis axis, Rect rect, int labelIndex, MinorLineContext context, List<string> directions)
        {
            string minorGrid = string.Empty;
            string minorTick = string.Empty;

            for (int j = 0; j < axis.MinorTicksPerInterval; j++)
            {
                context.TempInterval = FindLogNumeric(axis, context.LogPosition, context.TempInterval, labelIndex);
                context.LogPosition += context.LogInterval;

                if (axis.Renderer is not null && ChartHelper.Inside(context.TempInterval, axis.Renderer.VisibleRange))
                {
                    (string grid, string tick) = BuildHorizontalMinorLinePaths(context.TempInterval, rect, axis, context);
                    minorGrid = string.Concat(minorGrid, grid);
                    minorTick = string.Concat(minorTick, tick);
                }
            }

            directions.Add(minorGrid);
            directions.Add(minorTick);
        }

        /// <summary>
        /// Builds SVG paths for horizontal minor grid and tick.
        /// </summary>
        private (string grid, string tick) BuildHorizontalMinorLinePaths(double interval, Rect rect, ChartAxis axis, MinorLineContext context)
        {
            double position = (interval - axis.Renderer!.VisibleRange.Start) / (axis.Renderer.VisibleRange.End - axis.Renderer.VisibleRange.Start);
            position = Math.Ceiling((axis.IsAxisInverse ? (1 - position) : position) * rect.Width);
            double coordinate = Math.Floor(position + rect.X);
            string grid = "M " + coordinate.ToString(Culture) + SPACE + SeriesClipRect?.Y.ToString(Culture) +
                "L " + coordinate.ToString(Culture) + SPACE + ((SeriesClipRect?.Y ?? 0) + (SeriesClipRect?.Height ?? 0)).ToString(Culture);

            string tick = "M " + coordinate.ToString(Culture) + SPACE + rect.Y.ToString(Culture) + "L " + coordinate.ToString(Culture) + SPACE +
                (context.IsTickInside ? (rect.Y - context.TickSize) : rect.Y + context.TickSize + axis.ScrollBarHeight).ToString(Culture);

            return (grid, tick);
        }

        /// <summary>
        /// Calculates minor lines for vertical orientation. Preserves the original order of operations.
        /// </summary>
        private void CalculateVerticalMinorLines(ChartAxis axis, Rect rect, int labelIndex, MinorLineContext context, List<string> directions)
        {
            string minorGrid = string.Empty;
            string minorTick = string.Empty;

            for (int j = 0; j < axis.MinorTicksPerInterval; j++)
            {
                context.TempInterval = FindLogNumeric(axis, context.LogPosition, context.TempInterval, labelIndex);

                if (axis.Renderer is not null && ChartHelper.Inside(context.TempInterval, axis.Renderer.VisibleRange))
                {
                    (string grid, string tick) = BuildVerticalMinorLinePaths(context.TempInterval, rect, axis, context);
                    minorGrid = string.Concat(minorGrid, grid);
                    minorTick = string.Concat(minorTick, tick);
                }

                context.LogPosition += context.LogInterval;
            }

            directions.Add(minorGrid);
            directions.Add(minorTick);
        }

        /// <summary>
        /// Builds SVG paths for vertical minor grid and tick.
        /// </summary>
        private (string grid, string tick) BuildVerticalMinorLinePaths(double interval, Rect rect, ChartAxis axis, MinorLineContext context)
        {
            double position = Math.Ceiling((interval - axis.Renderer!.VisibleRange.Start) / (axis.Renderer.VisibleRange.End - axis.Renderer.VisibleRange.Start) * rect.Height) * -1;
            double coordinate = Math.Floor(position + rect.Y + rect.Height);
            string grid = "M " + SeriesClipRect?.X.ToString(Culture) + SPACE + coordinate.ToString(Culture)
                + "L " + ((SeriesClipRect?.X ?? 0) + (SeriesClipRect?.Width ?? 0)).ToString(Culture) + SPACE + coordinate.ToString(Culture) + SPACE;

            string tick = "M " + rect.X.ToString(Culture) + SPACE + coordinate.ToString(Culture)
                + "L " + (context.IsTickInside ? (rect.X + context.TickSize) : rect.X - context.TickSize - axis.ScrollBarHeight).ToString(Culture) + SPACE + coordinate.ToString(Culture) + SPACE;

            return (grid, tick);
        }

        /// <summary>
        /// Determines whether axis labels should be skipped based on adaptive rendering settings.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if labels should be skipped due to small chart size; otherwise <see langword="false"/>.
        /// </returns>
        private bool ShouldSkipAxisLabels()
        {
            return Chart is not null && Chart.EnableAdaptiveRendering && (Chart.AvailableSize.Height <= 100 || Chart.AvailableSize.Width <= 100);
        }

        /// <summary>
        /// Calculates the padding delta for rotated labels.
        /// </summary>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="isLabelInside">Indicates if the label is positioned inside the axis.</param>
        /// <param name="isOpposed">Indicates if the axis is in opposed position.</param>
        /// <returns>The padding adjustment value.</returns>
        private static double CalculateRotatedPaddingDelta(double angle, bool isLabelInside, bool isOpposed)
        {
            return (angle is 90 or 270 or -90 or -270) ? ((isLabelInside && !isOpposed && !isLabelInside && isOpposed) ? 5 : -5) : 0;
        }

        /// <summary>
        /// Determines whether the label should use end anchor alignment.
        /// </summary>
        /// <param name="isOpposed">Indicates if the axis is in opposed position.</param>
        /// <param name="isLabelInside">Indicates if the label is positioned inside the axis.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <returns> <see langword="true"/> if end anchor should be used; otherwise <see langword="false"/>.</returns>
        private static bool CalculateIsEndAnchor(bool isOpposed, bool isLabelInside, double angle)
        {
            return ((!isOpposed && !isLabelInside) || (isOpposed && isLabelInside)) ? (angle is (>= 180 and <= 360) or (>= -180 and <= -1)) : (angle is (>= 1 and <= 180) or (>= -360 and <= -181));
        }

        /// <summary>
        /// Calculates the effective label width based on intersection action and rotation angle.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="labelIntersectAction">The action to take when labels intersect.</param>
        /// <param name="angle">The label rotation angle in degrees.</param>
        /// <param name="intervalLength">The spacing between labels.</param>
        /// <param name="labelWidth">The measured label width.</param>
        /// <param name="label">The visible label information.</param>
        /// <returns>The calculated label width.</returns>
        private static double CalculateLabelWidth(ChartAxis axis, LabelIntersectAction? labelIntersectAction, double angle, double intervalLength, double labelWidth, VisibleLabels label)
        {
            return (labelIntersectAction == LabelIntersectAction.Trim || labelIntersectAction == LabelIntersectAction.Wrap) && angle == 0 && labelWidth > intervalLength
                ? intervalLength
                : axis.LabelTemplate is null ? labelWidth : intervalLength < labelWidth ? intervalLength : labelWidth;
        }

        /// <summary>
        /// Adjusts the X-coordinate of a label based on rotation angle and text wrapping.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="labelHeight">The height of the label text.</param>
        /// <param name="width">The width of the label.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="isWrappedText">Indicates if the label text is wrapped.</param>
        /// <param name="pointX">The initial X-coordinate.</param>
        /// <returns>The adjusted X-coordinate.</returns>
        private static double AdjustPointXForAngle(ChartAxis axis, double labelHeight, double width, double angle, bool isWrappedText, double pointX)
        {
            return axis.LabelTemplate is not null
                ? pointX - (angle == 0 ? width / 2 : 0)
                : pointX - (isWrappedText ? 0 : (angle == 0) ? (width / 2) : (angle is -90 or 270 ? -labelHeight : (angle is 90 or -270) ? labelHeight : 0));
        }

        /// <summary>
        /// Calculates the Y-coordinate for horizontal axis labels.
        /// </summary>
        /// <param name="rect">The axis rectangle bounds.</param>
        /// <param name="isLabelInside">Indicates if labels are inside the axis.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="isOpposed">Indicates if the axis is in opposed position.</param>
        /// <param name="padding">The label padding value.</param>
        /// <param name="labelHeight">The height of the label text.</param>
        /// <param name="label">The visible label information.</param>
        /// <param name="scrollBarHeight">The scrollbar height offset.</param>
        /// <returns>The calculated Y-coordinate.</returns>
        private static double CalculatePointY(Rect rect, bool isLabelInside, double angle, bool isOpposed, double padding, double labelHeight, VisibleLabels label, double scrollBarHeight)
        {
            if (isLabelInside && angle != 0)
            {
                return isOpposed ? (rect.Y + padding + labelHeight) : (rect.Y - padding - labelHeight);
            }

            double labelPadding = ((isOpposed && !isLabelInside) || (!isOpposed && isLabelInside))
                ? (-(padding + (angle != 0 ? labelHeight : (label.Index > 1 ? (2 * labelHeight) : 0))) * label.Index) - scrollBarHeight
                : ((padding + ((angle != 0 ? 1 : 3) * labelHeight)) * label.Index) + scrollBarHeight;

            if (label.TemplateSize is not null && label.TemplateSize.Height > 0)
            {
                labelPadding = ((isOpposed && !isLabelInside) || (!isOpposed && isLabelInside))
                    ? (-(padding + (angle != 0 ? 0 : label.Size.Height)) - scrollBarHeight) : padding + scrollBarHeight;
            }

            return rect.Y + labelPadding;
        }

        /// <summary>
        /// Creates text rendering options for an axis label.
        /// </summary>
        /// <param name="pointX">The X-coordinate for the label.</param>
        /// <param name="pointY">The Y-coordinate for the label.</param>
        /// <param name="label">The visible label information.</param>
        /// <param name="labelStyle">The font styling options.</param>
        /// <param name="chart">The parent chart instance.</param>
        /// <param name="index">The axis index.</param>
        /// <param name="labelIndex">The label index within the axis.</param>
        /// <param name="isEndAnchor">Indicates if end anchor alignment should be used.</param>
        /// <returns>The configured text options.</returns>
        private TextOptions CreateTextOptions(double pointX, double pointY, VisibleLabels label, ChartFontOptions labelStyle, SfChart chart, int index, int labelIndex, bool isEndAnchor)
        {
            return new TextOptions(pointX.ToString(Culture), pointY.ToString(Culture), !string.IsNullOrEmpty(label.LabelStyle.Color) ? label.LabelStyle.Color : Chart?._chartThemeStyle?.AxisLabel ?? string.Empty, labelStyle, string.Empty, isEndAnchor ? "end" : string.Empty, chart.ID + index + "_AxisLabel_" + labelIndex);
        }

        /// <summary>
        /// Calculates the longest text width among wrapped text lines.
        /// </summary>
        /// <param name="label">The visible label information.</param>
        /// <param name="labelStyle">The font styling options.</param>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="longestTextWidth">Output parameter for the longest text width.</param>
        /// <param name="longestTextIndex">Output parameter for the index of the longest text.</param>
        /// <param name="intervalLength">The spacing between labels.</param>
        private static void CalculateLongestText(VisibleLabels label, ChartFontOptions labelStyle, ChartAxis axis, ref double longestTextWidth, ref int longestTextIndex, double intervalLength)
        {
            if (axis.LabelTemplate is null)
            {
                for (int j = 0; j < label.TextArr.Length; j++)
                {
                    string labelText = label.TextArr[j];
                    double w = ChartHelper.MeasureText(labelText, labelStyle).Width;
                    if (w > longestTextWidth)
                    {
                        longestTextWidth = w;
                        longestTextIndex = j;
                    }
                }
            }
            else if (label.TemplateSize?.Width > 0)
            {
                longestTextWidth = label.TemplateSize.Width;
                longestTextWidth = longestTextWidth > intervalLength ? intervalLength : longestTextWidth;
            }
        }

        /// <summary>
        /// Processes edge label placement for labels with zero rotation angle.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="axisRenderer">The axis renderer instance.</param>
        /// <param name="rect">The axis rectangle bounds.</param>
        /// <param name="intervalLength">The spacing between labels (modified by reference).</param>
        /// <param name="labelWidth">The label width (modified by reference).</param>
        /// <param name="pointX">The X-coordinate (modified by reference).</param>
        /// <param name="previousEnd">The end position of the previous label (modified by reference).</param>
        /// <param name="label">The visible label information.</param>
        /// <param name="labelStyle">The font styling options.</param>
        /// <param name="isWrappedText">Indicates if the label text is wrapped.</param>
        /// <param name="labelIntersectAction">The action to take when labels intersect.</param>
        /// <param name="width">The total width of the label.</param>
        /// <param name="isOpposed">Indicates if the axis is in opposed position.</param>
        /// <param name="chartBorderStartX">The left edge of the chart border.</param>
        /// <param name="chartBorderEndX">The right edge of the chart border.</param>
        /// <param name="labelIndex">The current label index.</param>
        /// <param name="totalLabels">The total number of labels.</param>
        /// <param name="options">The text rendering options (modified by reference).</param>
        /// <param name="longestTextWidth">The width of the longest text line.</param>
        /// <returns>
        /// <see langword="true"/> if the label should be skipped; otherwise <see langword="false"/>.
        /// </returns>
        private bool ProcessEdgeLabelPlacementZeroAngle(ChartAxis axis, ChartAxisRenderer axisRenderer, Rect rect, ref double intervalLength, ref double labelWidth, ref double pointX, ref double previousEnd, VisibleLabels label, ChartFontOptions labelStyle,
            bool isWrappedText, LabelIntersectAction? labelIntersectAction, double width, bool isOpposed, double chartBorderStartX, double chartBorderEndX, int labelIndex, int totalLabels, TextOptions options, double longestTextWidth)
        {
            switch (axisRenderer.EdgeLabelPlacement)
            {
                case EdgeLabelPlacement.None:
                    return false;

                case EdgeLabelPlacement.Hide:
                    if (((labelIndex == 0 || (axis.IsAxisInverse && labelIndex == totalLabels - 1)) && Convert.ToDouble(options.X, Culture) <= rect.X) || ((labelIndex == totalLabels - 1 || (axis.IsAxisInverse && labelIndex == 0)) && (Convert.ToDouble(options.X, Culture) + width > rect.X + rect.Width)))
                    {
                        return true;
                    }
                    return false;

                case EdgeLabelPlacement.Shift:
                    double xValue = Convert.ToDouble(options.X, Culture);
                    double edgeLabelPadding = 2;

                    if (axis.LabelTemplate is null)
                    {
                        double shiftableWidth;

                        if (((labelIndex == 0 && !axis.IsAxisInverse) || (axis.IsAxisInverse && labelIndex == totalLabels - 1)) && (xValue < chartBorderStartX || (isWrappedText && xValue - (longestTextWidth / 2) < chartBorderStartX)))
                        {
                            if (axis.LabelPlacement == LabelPlacement.OnTicks && labelIntersectAction == LabelIntersectAction.Wrap)
                            {
                                shiftableWidth = xValue - (longestTextWidth / 2) + chartBorderStartX;
                                intervalLength -= shiftableWidth;
                                xValue = isWrappedText ? xValue - shiftableWidth + edgeLabelPadding : chartBorderStartX + edgeLabelPadding;
                            }
                            else
                            {
                                intervalLength -= -xValue;
                                xValue = chartBorderStartX + edgeLabelPadding;
                            }

                            options.X = xValue.ToString(Culture);
                            pointX = xValue;
                        }
                        else if (((labelIndex == totalLabels - 1 && !axis.IsAxisInverse) || (axis.IsAxisInverse && labelIndex == 0)) && ((xValue + longestTextWidth) > chartBorderEndX))
                        {
                            double effectiveWidth = isWrappedText ? Math.Min(longestTextWidth, intervalLength) : width;
                            shiftableWidth = isWrappedText ? xValue + (effectiveWidth / 2) - chartBorderEndX : xValue + effectiveWidth - chartBorderEndX;

                            if (label.Size.Width > intervalLength && axis.LabelPlacement == LabelPlacement.OnTicks && labelIntersectAction == LabelIntersectAction.Trim)
                            {
                                intervalLength -= xValue + width - chartBorderEndX;
                                xValue = chartBorderEndX - intervalLength - edgeLabelPadding;

                                if (!axis.IsAxisInverse && xValue <= previousEnd)
                                {
                                    intervalLength = chartBorderEndX - (previousEnd + AXIS_LABEL_SPACE);
                                    xValue += previousEnd - xValue + AXIS_LABEL_SPACE;
                                }
                            }
                            else if (axis.LabelPlacement == LabelPlacement.OnTicks && labelIntersectAction == LabelIntersectAction.Wrap)
                            {
                                intervalLength -= shiftableWidth;
                                xValue -= shiftableWidth + edgeLabelPadding;

                                if (!axis.IsAxisInverse && (xValue <= previousEnd || xValue - (longestTextWidth / 2) <= previousEnd))
                                {
                                    double overlappedWidth = xValue < previousEnd ? previousEnd - xValue : previousEnd - (xValue - (longestTextWidth / 2));
                                    intervalLength = chartBorderEndX - (previousEnd + AXIS_LABEL_SPACE);
                                    label.TextArr = [.. ChartHelper.TextWrap(label.Text, intervalLength, labelStyle)];
                                    xValue += overlappedWidth + AXIS_LABEL_SPACE;
                                }
                            }
                            else
                            {
                                intervalLength = width;
                                xValue = isWrappedText || (axis.LabelPlacement == LabelPlacement.BetweenTicks && axis.ValueType == ValueType.Category) ? xValue : xValue - shiftableWidth - edgeLabelPadding;
                            }

                            options.X = xValue.ToString(Culture);
                            pointX = xValue;
                        }
                    }
                    else
                    {
                        double maxAllowedWidth = intervalLength;

                        if (labelWidth > maxAllowedWidth)
                        {
                            labelWidth = maxAllowedWidth;
                        }

                        if (((labelIndex == 0 && !axis.IsAxisInverse) || (axis.IsAxisInverse && labelIndex == totalLabels - 1)) && (xValue < chartBorderStartX || (isWrappedText && xValue - (labelWidth / 2) < chartBorderStartX)))
                        {
                            double shiftAmount = xValue - chartBorderStartX;
                            xValue = chartBorderStartX + edgeLabelPadding;
                            intervalLength -= shiftAmount;
                        }
                        else if (((labelIndex == totalLabels - 1 && !axis.IsAxisInverse) || (axis.IsAxisInverse && labelIndex == 0)) && (xValue + labelWidth > chartBorderEndX))
                        {
                            double shiftAmount = xValue + labelWidth - chartBorderEndX;
                            xValue -= shiftAmount + edgeLabelPadding;
                            intervalLength -= shiftAmount;

                            if (!axis.IsAxisInverse && xValue <= previousEnd)
                            {
                                double overlap = previousEnd - xValue + AXIS_LABEL_SPACE;
                                xValue += overlap;
                                labelWidth -= overlap;
                            }
                        }

                        if (xValue + labelWidth > chartBorderEndX)
                        {
                            double overflow = xValue + labelWidth - chartBorderEndX;
                            xValue -= overflow;
                            labelWidth -= overflow;
                        }

                        if (xValue < chartBorderStartX)
                        {
                            double overflow = chartBorderStartX - xValue;
                            xValue += overflow;
                            labelWidth -= overflow;
                        }

                        options.X = xValue.ToString(Culture);
                        label.Size.Width = labelWidth;
                        pointX = xValue;
                    }

                    return false;
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// Handles edge label shifting for rotated labels.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="rect">The axis rectangle bounds.</param>
        /// <param name="label">The visible label information.</param>
        /// <param name="labelStyle">The font styling options.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="isWrappedText">Indicates if the label text is wrapped.</param>
        /// <param name="longestTextWidth">The width of the longest text line.</param>
        /// <param name="longestTextIndex">The index of the longest text line.</param>
        /// <param name="chartBorderStartX">The left edge of the chart border.</param>
        /// <param name="chartBorderEndX">The right edge of the chart border.</param>
        /// <param name="labelIndex">The current label index.</param>
        /// <param name="totalLabels">The total number of labels.</param>
        /// <param name="pointX">The X-coordinate (modified by reference).</param>
        /// <param name="previousEnd">The end position of the previous label (modified by reference).</param>
        /// <param name="options">The text rendering options.</param>
        /// <param name="pointY">The Y-coordinate.</param>
        private void HandleRotatedEdgeLabelShift(ChartAxis axis, Rect rect, VisibleLabels label, ChartFontOptions labelStyle, double angle, bool isWrappedText, double longestTextWidth,
            int longestTextIndex, double chartBorderStartX, double chartBorderEndX, int labelIndex, int totalLabels, ref double pointX, ref double previousEnd, TextOptions options, double pointY)
        {
            double rotateLabelwidth = ChartHelper.RotateTextSize(labelStyle, isWrappedText ? label.TextArr[longestTextIndex] : label.Text, angle).Width;
            double height = pointY - Convert.ToDouble(options.Y, Culture) - ((label.Size.Height / 2) + 10);
            ChartEventLocation[] rotatedLabelPoints = GetRotatedRectangleCoordinates(
                GetRectanglePoints(new Rect(Convert.ToDouble(options.X, Culture), Convert.ToDouble(options.Y, Culture) - ((label.Size.Height / 2) - 5), label.Size.Width, height)),
                pointX,
                pointY - (height / 2),
                angle);

            if (labelIndex == 0 && Convert.ToDouble(rotatedLabelPoints[0].X, Culture) < chartBorderStartX)
            {
                options.X = (Convert.ToDouble(options.X, Culture) + (rotateLabelwidth / 4)).ToString(CultureInfo.InvariantCulture);
                options.Y = (Convert.ToDouble(options.Y, Culture) + (rotateLabelwidth / 4)).ToString(CultureInfo.InvariantCulture);
            }
            else if (labelIndex == totalLabels - 1 && ((Convert.ToDouble(options.X, Culture) + rotateLabelwidth) > chartBorderEndX) && angle > 0)
            {
                double xValue = Convert.ToDouble(options.X, Culture);
                xValue -= xValue + rotateLabelwidth - chartBorderEndX;

                if (!axis.IsAxisInverse && previousEnd < chartBorderEndX && xValue < previousEnd)
                {
                    label.Text = ChartHelper.TextTrim(chartBorderEndX - Convert.ToDouble(options.X, Culture), label.Text, labelStyle);
                }
                else
                {
                    options.X = xValue.ToString(Culture);
                    pointX = xValue;
                }
            }
        }

        /// <summary>
        /// Applies rotation transformation to label rendering options.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="label">The visible label information.</param>
        /// <param name="labelHeight">The height of the label text.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="isOpposed">Indicates if the axis is in opposed position.</param>
        /// <param name="pointX">The X-coordinate.</param>
        /// <param name="pointY">The Y-coordinate.</param>
        /// <param name="isRotatedLabelIntersect">
        /// Output flag indicating if rotated labels intersect (modified by reference).
        /// </param>
        /// <param name="newPoints">Collection of rotated rectangle coordinates.</param>
        /// <param name="labelIntersectAction">The action to take when labels intersect.</param>
        /// <param name="labelIndex">The current label index.</param>
        /// <param name="options">The text rendering options (modified by reference).</param>
        private void ApplyRotationOptions(ChartAxis axis, VisibleLabels label, double labelHeight, double angle, bool isOpposed, double pointX, double pointY,
            ref bool isRotatedLabelIntersect, List<ChartEventLocation[]> newPoints, LabelIntersectAction? labelIntersectAction, int labelIndex, TextOptions options)
        {
            options.Transform = "rotate(" + angle.ToString(Culture) + "," + pointX.ToString(Culture) + "," + pointY.ToString(Culture) + ')';
            options.Y = (ChartHelper.IsBreakLabel(label.OriginalText)
                ? Convert.ToDouble(options.Y, Culture) + (isOpposed ? (4 * label.TextArr.Length) : -(4 * label.TextArr.Length))
                : Convert.ToDouble(options.Y, Culture)).ToString(Culture);

            double height = pointY - Convert.ToDouble(options.Y, Culture) - (label.Size.Height / 2);
            newPoints.Add(GetRotatedRectangleCoordinates(
                GetRectanglePoints(new Rect(Convert.ToDouble(options.X, Culture), Convert.ToDouble(options.Y, Culture) - ((label.Size.Height / 2) - 5), label.Size.Width, height)),
                pointX,
                pointY - (height / 2),
                angle));

            isRotatedLabelIntersect = false;

            if (labelIntersectAction != LabelIntersectAction.None)
            {
                for (int index1 = labelIndex; index1 > 0; index1--)
                {
                    if (newPoints[labelIndex] is not null && newPoints[index1 - 1] is not null && IsRotatedRectIntersect(newPoints[labelIndex], newPoints[index1 - 1]))
                    {
                        isRotatedLabelIntersect = true;
                        newPoints[labelIndex] = null!;
                        break;
                    }
                }
            }

            if (axis.LabelTemplate is not null)
            {
                (double x, double y) = GetRotateTemplateStyles(angle, label.Size.Width, label.Size.Height);
                options.X = (Convert.ToDouble(options.X, Culture) + (x / 2)).ToString(Culture);
                options.Y = (Convert.ToDouble(options.Y, Culture) + y).ToString(Culture);
                options.Transform = $"transform: rotate({angle.ToString(Culture)}deg); " + $"transform-origin: 0 0";
            }
        }

        /// <summary>
        /// Determines if an edge label should be hidden or skipped.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="rect">The axis rectangle bounds.</param>
        /// <param name="width">The width of the label.</param>
        /// <param name="chartBorderEndX">The right edge of the chart border.</param>
        /// <param name="labelIndex">The current label index.</param>
        /// <param name="totalLabels">The total number of labels.</param>
        /// <param name="options">The text rendering options.</param>
        /// <returns>
        /// <see langword="true"/> if the label should be skipped; otherwise <see langword="false"/>.
        /// </returns>
        private bool ShouldSkipEdgeLabelForHide(ChartAxis axis, Rect rect, double width, double chartBorderEndX, int labelIndex, int totalLabels, TextOptions options)
        {
            return ((labelIndex == 0 || (axis.IsAxisInverse && labelIndex == totalLabels - 1)) && Convert.ToDouble(options.X, Culture) <= rect.X) || ((labelIndex == totalLabels - 1 || (axis.IsAxisInverse && labelIndex == 0)) && (Convert.ToDouble(options.X, Culture) + width > chartBorderEndX));
        }

        /// <summary>
        /// Processes overlap detection with the previous axis and adjusts label rendering accordingly.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="width">The width of the current label.</param>
        /// <param name="labelIndex">The current label index.</param>
        /// <param name="totalLabels">The total number of labels.</param>
        /// <param name="options">The text rendering options.</param>
        /// <returns>
        /// <see langword="true"/> if the label should be skipped due to overlap; otherwise <see langword="false"/>.
        /// </returns>
        private bool ProcessPreviousAxisOverlap(ChartAxis axis, double width, int labelIndex, int totalLabels, TextOptions options)
        {
            if ((labelIndex == 0 && !axis.IsAxisInverse) || (axis.IsAxisInverse && labelIndex == totalLabels - 1))
            {
                if (Convert.ToDouble(options.X, Culture) >= _previousStartX && Convert.ToDouble(options.X, Culture) <= _previousAxisEnd)
                {
                    List<TextOptions>? prevAxisLabels = _previousAxis?.Renderer?.AxisRenderInfo.AxisLabelOptions;
                    int count = prevAxisLabels?.Count ?? 0;
                    List<TextOptions> tempAxisLabels = [];

                    for (int j = 0; j < count; j++)
                    {
                        if (j != (_previousAxis!.IsAxisInverse ? 0 : count - 1))
                        {
                            tempAxisLabels.Add(prevAxisLabels![j]);
                        }
                        else
                        {
                            if ((Convert.ToDouble(prevAxisLabels![j].X, Culture) + (_previousAxis.Renderer?.VisibleLabels[j].Size.Width / _previousAxis.Renderer?.VisibleLabels[j].TextArr.Length)) < Convert.ToDouble(options.X, Culture))
                            {
                                tempAxisLabels.Add(prevAxisLabels[j]);
                            }
                        }
                    }

                    if (tempAxisLabels.Count != 0 && _previousAxis?.Renderer is not null)
                    {
                        _previousAxis.Renderer.AxisRenderInfo.AxisLabelOptions = tempAxisLabels;
                    }
                }
            }
            else if ((labelIndex == totalLabels - 1 && !axis.IsAxisInverse) || (axis.IsAxisInverse && labelIndex == 0))
            {
                if (Convert.ToDouble(options.X, Culture) <= _previousStartX && (Convert.ToDouble(options.X, Culture) + width) >= _previousStartX)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds an axis label template to the rendering pipeline.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="index">The axis index.</param>
        /// <param name="labelIndex">The label index within the axis.</param>
        /// <param name="rect">The axis rectangle bounds.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="isLabelInside">Indicates if the label is positioned inside the axis.</param>
        /// <param name="isOpposed">Indicates if the axis is in opposed position.</param>
        /// <param name="intervalLength">The spacing between labels.</param>
        /// <param name="label">The visible label information.</param>
        /// <param name="options">The text rendering options.</param>
        private void AddAxisLabelTemplate(ChartAxis axis, int index, int labelIndex, Rect rect, double angle, bool isLabelInside, bool isOpposed, double intervalLength, VisibleLabels label, TextOptions options)
        {
            string templateY = options.Y;
            if (angle != 0 && ((isLabelInside && !isOpposed) || (isOpposed && !isLabelInside)))
            {
                templateY = (Convert.ToDouble(options.Y, Culture) - (axis.Renderer?.MaxLabelSize.Height ?? 0)).ToString(Culture);
            }

            double templateWidth = axis.EdgeLabelPlacement == EdgeLabelPlacement.Shift ? label.Size.Width : label.Size.Width > intervalLength ? intervalLength : label.Size.Width;
            LabelTemplateOptions labelOptions = new(options.X, templateY, Chart?.ID + index + "_AxisLabelTemplate_" + labelIndex, new ChartAxisLabelInfo(label.OriginalText, label.DateTimeValue), templateWidth, label.Size.Height)
            {
                TemplateSize = label.TemplateSize ?? null!,
                Transform = options.Transform
            };
            axis.Renderer?.AxisRenderInfo.LabelTemplateOptions.Add(labelOptions);
        }

        /// <summary>
        /// Determines whether to skip rendering for adaptive mode based on width threshold.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if chart width is below threshold; otherwise <see langword="false"/>.
        /// </returns>
        private bool ShouldSkipAdaptiveRendering()
        {
            return Chart is not null && Chart.EnableAdaptiveRendering && Chart.AvailableSize.Width <= 100;
        }

        /// <summary>
        /// Initializes the context data for Y-axis label calculations.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="rect">The axis rectangle bounds.</param>
        /// <returns>A context object containing calculated Y-axis label parameters.</returns>
        private YAxisLabelContext InitializeYAxisContext(ChartAxis axis, Rect rect)
        {
            double angle = (axis.Renderer?.Angle ?? 0) % 360;
            double intervalHeight = (rect.Height / (axis.Renderer?.VisibleLabels.Count ?? 0)) - 5;
            AxisPosition? labelPosition = axis.Renderer?.LabelPosition ?? null!;
            bool isLabelInside = labelPosition == AxisPosition.Inside;

            double labelSpace = axis.LabelPadding;
            double tickSpace = (labelPosition == axis.TickPosition && axis.Renderer is not null)
                ? axis.Renderer.MajorTickLinesHeight : 0;
            double padding = tickSpace + labelSpace + (axis.LineStyle.Width * 0.5);
            bool isOpposed = axis.IsAxisOpposedPosition;
            padding = isOpposed ? padding : -padding;

            ChartFontOptions labelStyle = axis.LabelStyle.GetChartFontOptions(Chart?._chartThemeStyle ?? null!);

            List<double> labelWidths = [];
            foreach (VisibleLabels vl in axis.Renderer?.VisibleLabels ?? [])
            {
                Size size = ChartHelper.IsBreakLabel(vl.OriginalText) ? vl.BreakLabelSize : vl.Size;
                labelWidths.Add(size.Width);
            }
            double labelMaxWidth = labelWidths.Count > 0 ? labelWidths.Max() : 0;

            return new YAxisLabelContext
            {
                Angle = angle,
                IntervalHeight = intervalHeight,
                LabelPosition = labelPosition,
                IsLabelInside = isLabelInside,
                Padding = padding,
                IsOpposed = isOpposed,
                LabelStyle = labelStyle,
                LabelMaxWidth = labelMaxWidth,
                VisibleLabelsCount = axis.Renderer?.VisibleLabels.Count ?? 0
            };
        }

        /// <summary>
        /// Assigns a template identifier to a visible label.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="labelIndex">The index of the label.</param>
        private static void AssignTemplateID(ChartAxis axis, int labelIndex)
        {
            if (axis.Renderer is { })
            {
                axis.Renderer.VisibleLabels[labelIndex].TemplateID = axis.LabelTemplate is not null ? axis.Renderer.Chart?.ID + axis.Renderer.Index + "_AxisLabelTemplate_" + labelIndex : string.Empty;
            }
        }

        /// <summary>
        /// Adjusts the label size for template-based rendering.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="labelIndex">The index of the label.</param>
        private static void AdjustLabelSizeForTemplate(ChartAxis axis, int labelIndex)
        {
            if (axis.Renderer is not null)
            {
                VisibleLabels visibleLabel = axis.Renderer.VisibleLabels[labelIndex];
                visibleLabel.Size.Width = (axis.LabelTemplate is not null && axis.Renderer.MaxLabelSize.Width < visibleLabel.Size.Width) ? axis.Renderer.MaxLabelSize.Width : visibleLabel.Size.Width;
            }
        }

        /// <summary>
        /// Calculates the X-coordinate for Y-axis labels.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="rect">The axis rectangle bounds.</param>
        /// <param name="context">The Y-axis label context.</param>
        /// <param name="labelIndex">The index of the label.</param>
        /// <returns>The calculated X-coordinate.</returns>
        private static double CalculateYAxisLabelX(ChartAxis axis, Rect rect, YAxisLabelContext context, int labelIndex)
        {
            double pointX = context.IsLabelInside ? (rect.X - context.Padding) : (rect.X + context.Padding + (axis.CrossesAt is null ? axis.ScrollBarHeight * (context.IsOpposed ? 1 : -1) : 0));
            if (axis.LabelTemplate is not null)
            {
                pointX = context.IsLabelInside
                    ? (rect.X - context.Padding - (context.IsOpposed ? (axis.Renderer?.VisibleLabels[labelIndex].Size.Width ?? 0) : 0))
                    : rect.X + context.Padding + (axis.CrossesAt is null ? axis.ScrollBarHeight * (context.IsOpposed ? 1 : -1) : 0) + (context.IsOpposed ? 0 : -(axis.Renderer?.VisibleLabels[labelIndex].Size.Width ?? 0));
            }

            return pointX;
        }

        /// <summary>
        /// Calculates the Y-coordinate for Y-axis labels.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="rect">The axis rectangle bounds.</param>
        /// <param name="label">The visible label information.</param>
        /// <param name="intervalHeight">The spacing between labels.</param>
        /// <param name="labelIndex">The index of the label.</param>
        /// <param name="isAxisBreakLabel">Indicates if the label spans multiple lines.</param>
        /// <param name="elementSize">The measured size of the label element.</param>
        /// <param name="isLabelInside">Indicates if the label is positioned inside the axis.</param>
        /// <returns>The calculated Y-coordinate.</returns>
        private static double CalculateYAxisLabelY(ChartAxis axis, Rect rect, VisibleLabels label, double intervalHeight, int labelIndex, bool isAxisBreakLabel, Size elementSize, bool isLabelInside)
        {
            double pointY = (ChartHelper.ValueToCoefficient(label.Value, axis.Renderer ?? null!) * rect.Height) + 0;
            pointY = Math.Floor((pointY * -1) + (rect.Y + rect.Height));

            double textHeight = elementSize.Height / 8 * (label.TextArr.Length / 2);
            double textPadding = axis.LabelTemplate is null ? (elementSize.Height / 4 * 3) + 3
                    : (!isLabelInside ? (intervalHeight < (axis.Renderer?.VisibleLabels[labelIndex].Size.Height ?? elementSize.Height)
                    ? -(intervalHeight / 2) : -(elementSize.Height / 2)) : 0);

            pointY = isAxisBreakLabel ? (isLabelInside ? (pointY - (elementSize.Height / 2) - textHeight + textPadding) : (pointY - textHeight))
                : (isLabelInside ? (pointY + textPadding) : pointY + (axis.LabelTemplate is null ? (elementSize.Height / 4) : textPadding));

            return pointY;
        }

        /// <summary>
        /// Determines the text anchor alignment for Y-axis labels.
        /// </summary>
        /// <param name="isOpposed">Indicates if the axis is in opposed position.</param>
        /// <param name="isLabelInside">Indicates if the label is positioned inside the axis.</param>
        /// <returns>
        /// <see langword="true"/> to use end anchor alignment; otherwise <see langword="false"/>.
        /// </returns>
        private bool DetermineYAxisAnchor(bool isOpposed, bool isLabelInside)
        {
            bool isEndAnchor = (isOpposed && isLabelInside) || (!isOpposed && !isLabelInside);
            isEndAnchor = Chart is not null && Chart.EnableRtl ? !isEndAnchor : isEndAnchor;
            return isEndAnchor;
        }

        /// <summary>
        /// Calculates the effective height for Y-axis labels.
        /// </summary>
        /// <param name="isAxisBreakLabel">Indicates if the label spans multiple lines.</param>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="label">The visible label information.</param>
        /// <returns>The calculated label height.</returns>
        private static double CalculateYAxisLabelHeight(bool isAxisBreakLabel, ChartAxis axis, VisibleLabels label)
        {
            return (isAxisBreakLabel && axis.LabelTemplate is null) ? label.BreakLabelSize.Height : label.Size.Height;
        }

        /// <summary>
        /// Creates text rendering options for a Y-axis label.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="index">The axis index.</param>
        /// <param name="labelIndex">The label index within the axis.</param>
        /// <param name="pointX">The X-coordinate for the label.</param>
        /// <param name="pointY">The Y-coordinate for the label.</param>
        /// <param name="label">The visible label information.</param>
        /// <param name="labelStyle">The font styling options.</param>
        /// <param name="isEndAnchor">Indicates if end anchor alignment should be used.</param>
        /// <param name="isAxisBreakLabel">Indicates if the label spans multiple lines.</param>
        /// <returns>The configured text options.</returns>
        private TextOptions CreateYAxisTextOptions(ChartAxis axis, int index, int labelIndex, double pointX, double pointY, VisibleLabels label, ChartFontOptions labelStyle, bool isEndAnchor, bool isAxisBreakLabel)
        {
            TextOptions options = new(
                pointX.ToString(Culture),
                pointY.ToString(Culture),
                !string.IsNullOrEmpty(axis.LabelStyle.Color) ? axis.LabelStyle.Color : (Chart?._chartThemeStyle?.AxisLabel ?? string.Empty),
                GetFontOptions(labelStyle),
                label.Text,
                isEndAnchor ? "end" : "start",
                axis.Renderer?.Owner?.ID + index + "_AxisLabel_" + labelIndex);

            if (isAxisBreakLabel && axis.LabelTemplate is null)
            {
                foreach (string text in label.TextArr)
                {
                    options.TextCollection.Add(text);
                }
            }

            return options;
        }

        /// <summary>
        /// Determines if a Y-axis label should be hidden due to intersection with the previous label.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="context">The Y-axis label context.</param>
        /// <param name="labelIndex">The current label index.</param>
        /// <param name="options">The text rendering options.</param>
        /// <param name="labelHeight">The height of the label.</param>
        /// <param name="previousEnd">The end position of the previous label.</param>
        /// <returns>
        /// <see langword="true"/> if the label should be hidden; otherwise <see langword="false"/>.
        /// </returns>
        private bool ShouldHideLabelForIntersection(ChartAxis axis, YAxisLabelContext context, int labelIndex, TextOptions options, double labelHeight, double previousEnd)
        {
            ChartAxisRenderer axisRenderer = axis.Renderer ?? null!;
            return context.Angle == 0 && axisRenderer.LabelIntersectAction == LabelIntersectAction.Hide && labelIndex != 0
                && (axis.IsAxisInverse ? Convert.ToDouble(options.Y, Culture) <= previousEnd : Convert.ToDouble(options.Y, Culture) + labelHeight >= previousEnd);
        }

        /// <summary>
        /// Applies edge label placement rules and determines if the label should be skipped.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="rect">The axis rectangle bounds.</param>
        /// <param name="context">The Y-axis label context.</param>
        /// <param name="labelIndex">The current label index.</param>
        /// <param name="totalLabels">The total number of labels.</param>
        /// <param name="options">The text rendering options (modified by reference).</param>
        /// <param name="elementSize">The measured size of the label element.</param>
        /// <param name="label">The visible label information.</param>
        /// <param name="intervalHeight">The spacing between labels.</param>
        /// <returns>
        /// <see langword="true"/> if the label should be skipped; otherwise <see langword="false"/>.
        /// </returns>
        private bool ApplyEdgeLabelPlacementAndMaybeSkip(ChartAxis axis, Rect rect, YAxisLabelContext context, int labelIndex, int totalLabels, TextOptions options, Size elementSize, VisibleLabels label, double intervalHeight)
        {
            ChartAxisRenderer axisRenderer = axis.Renderer ?? null!;
            double tempHeight = label.Size.Height > intervalHeight ? intervalHeight : label.Size.Height;

            switch (axisRenderer.EdgeLabelPlacement)
            {
                case EdgeLabelPlacement.None:
                    return false;

                case EdgeLabelPlacement.Hide:
                    if (((labelIndex == 0 || (axis.IsAxisInverse && labelIndex == totalLabels - 1)) && Convert.ToDouble(options.Y, Culture) + (axis.LabelTemplate is not null ? tempHeight : 0) > rect.Y + rect.Height)
                        || (((labelIndex == totalLabels - 1) || (axis.IsAxisInverse && labelIndex == 0)) && (Convert.ToDouble(options.Y, Culture) - (axis.LabelTemplate is null ? (elementSize.Height * 0.5) : 0)) < rect.Y))
                    {
                        options.Text = string.Empty;
                        if (axis.LabelTemplate is not null)
                        {
                            return true;
                        }
                    }
                    return false;

                case EdgeLabelPlacement.Shift:
                    return ApplyEdgeShift(axis, rect, context.LabelPosition, labelIndex, totalLabels, options, elementSize, tempHeight, intervalHeight);

                default:
                    return false;
            }
        }

        /// <summary>
        /// Applies edge label shifting logic for Y-axis labels to prevent clipping at chart boundaries.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="rect">The axis rectangle bounds.</param>
        /// <param name="labelPosition">The position of labels relative to the axis (inside or outside).</param>
        /// <param name="labelIndex">The current label index.</param>
        /// <param name="totalLabels">The total number of labels.</param>
        /// <param name="options">The text rendering options (modified by reference).</param>
        /// <param name="elementSize">The measured size of the label element.</param>
        /// <param name="tempHeight">The temporary height constraint for the label.</param>
        /// <param name="intervalHeight">The spacing between labels.</param>
        /// <returns>
        /// <see langword="false"/> as shifted labels are not skipped; always returns <see langword="false"/>.
        /// </returns>
        private bool ApplyEdgeShift(ChartAxis axis, Rect rect, AxisPosition? labelPosition, int labelIndex, int totalLabels, TextOptions options, Size elementSize, double tempHeight, double intervalHeight)
        {
            double chartBorderStartY = (Chart?._chartBorderRenderer?.ChartBorder?.Width ?? 0) * 0.5;
            double startLimit = labelPosition == AxisPosition.Inside
                ? rect.Y + rect.Height
                : chartBorderStartY + ((Chart?.AvailableSize.Height ?? 0) - (Chart?._chartBorderRenderer?.ChartBorder?.Width ?? 0));
            double endLimit = labelPosition == AxisPosition.Inside ? rect.Y : chartBorderStartY;

            double pointY = Convert.ToDouble(options.Y, Culture);

            if (axis.LabelTemplate is null)
            {
                if ((labelIndex == 0 || (axis.IsAxisInverse && labelIndex == totalLabels - 1)) && pointY > startLimit)
                {
                    pointY = startLimit;
                    options.Y = pointY.ToString(Culture);
                }
                else if (((labelIndex == totalLabels - 1) || (axis.IsAxisInverse && labelIndex == 0)) && ((pointY - (elementSize.Height * 0.5)) < endLimit))
                {
                    pointY = endLimit + (elementSize.Height * 0.5);
                    options.Y = pointY.ToString(Culture);
                }
            }
            else
            {
                if ((labelIndex == 0 || (axis.IsAxisInverse && labelIndex == totalLabels - 1)) && pointY + tempHeight > startLimit)
                {
                    double overflow = pointY + tempHeight - startLimit;
                    if (overflow > 0)
                    {
                        pointY -= overflow;
                    }
                    options.Y = pointY.ToString(Culture);
                }
                else if (((labelIndex == totalLabels - 1) || (axis.IsAxisInverse && labelIndex == 0)) && pointY < endLimit)
                {
                    double overflow = Math.Abs(pointY) - endLimit;
                    if (overflow > 0)
                    {
                        pointY += overflow > intervalHeight / 2 ? intervalHeight / 2 : overflow;
                    }
                    options.Y = pointY.ToString(Culture);
                }
            }

            return false;
        }

        /// <summary>
        /// Applies rotation transformation to Y-axis label rendering options.
        /// </summary>
        /// <param name="options">The text rendering options to be modified with rotation transform.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <param name="labelMaxWidth">The maximum width among all labels in the axis.</param>
        /// <param name="elementSize">The measured size of the label element.</param>
        private void ApplyYAxisRotation(TextOptions options, double angle, double labelMaxWidth, Size elementSize)
        {
            double currentX = Convert.ToDouble(options.X, Culture);
            double currentY = Convert.ToDouble(options.Y, Culture);
            double centerX = (options.TextAnchor == "end") ? currentX - (labelMaxWidth / 2) : currentX + (labelMaxWidth / 2);
            double centerY = currentY - (elementSize.Height / 2);

            options.X = centerX.ToString(Culture);
            options.Y = centerY.ToString(Culture);
            options.TextAnchor = "middle";
            options.Transform = $"rotate({angle.ToString(Culture)},{options.X},{options.Y})";
        }

        /// <summary>
        /// Finalizes Y-axis label rendering options and adds them to the axis renderer.
        /// </summary>
        /// <param name="axis">The chart axis being rendered.</param>
        /// <param name="options">The text rendering options for the label.</param>
        /// <param name="labelStyle">The font styling options.</param>
        /// <param name="index">The axis index.</param>
        /// <param name="labelIndex">The label index within the axis.</param>
        /// <param name="label">The visible label information.</param>
        /// <param name="intervalHeight">The spacing between labels.</param>
        private void FinalizeYAxisLabelOptions(ChartAxis axis, TextOptions options, ChartFontOptions labelStyle, int index, int labelIndex, VisibleLabels label, double intervalHeight)
        {
            string[] locations = ChartHelper.AppendTextElements(axis.Renderer?.Owner ?? null!, options.Id, Convert.ToDouble(options.X, Culture), Convert.ToDouble(options.Y, Culture));

            options.X = locations[0];
            options.Y = locations[1];
            options.Font = labelStyle;

            axis.Renderer?.AxisRenderInfo.AxisLabelOptions.Add(options);

            if (axis.LabelTemplate is not null)
            {
                LabelTemplateOptions labelOptions = new(
                    options.X,
                    options.Y,
                    Chart?.ID + index + "_AxisLabelTemplate_" + labelIndex,
                    new ChartAxisLabelInfo(label.OriginalText, label.DateTimeValue),
                    label.Size.Width,
                    label.Size.Height > intervalHeight ? intervalHeight : label.Size.Height)
                {
                    TemplateSize = label.TemplateSize
                };
                axis.Renderer?.AxisRenderInfo.LabelTemplateOptions.Add(labelOptions);
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Calculates and renders X-axis labels with support for rotation, wrapping, and edge placement.
        /// </summary>
        /// <param name="axis">The chart axis to calculate labels for.</param>
        /// <param name="index">The axis index in the chart's axis collection.</param>
        /// <param name="rect">The axis rectangle defining the rendering area.</param>
        internal void CalculateXAxisLabels(ChartAxis axis, int index, Rect rect)
        {
            if (ShouldSkipAxisLabels())
            {
                return;
            }

            double pointY;
            double labelWidth;

            SfChart chart = axis.Renderer?.Owner ?? null!;
            AxisPosition? labelPosition = axis.Renderer?.LabelPosition ?? null!;
            bool islabelInside = labelPosition == AxisPosition.Inside;
            double tickSpace = labelPosition == axis.TickPosition && axis.Renderer is not null ? axis.Renderer.MajorTickLinesHeight : 0;
            double padding = tickSpace + axis.LabelPadding + (axis.LineStyle.Width * 0.5);
            double angle = (axis.Renderer?.Angle ?? 0) % 360;
            ChartFontOptions labelStyle = axis.LabelStyle.GetChartFontOptions(Chart?._chartThemeStyle ?? null!);
            double intervalLength = rect.Width / (axis.Renderer?.VisibleLabels.Count ?? 0);
            double previousEnd = axis.IsAxisInverse ? (rect.X + rect.Width) : rect.X;
            bool isOpposed = axis.IsAxisOpposedPosition;
            double scrollBarHeight = axis.ScrollbarSettings.Enable || (!islabelInside && axis.CrossesAt is null && (axis.ZoomFactor < 1 || axis.ZoomPosition > 0)) ? axis.ScrollBarHeight : 0;
            List<ChartEventLocation[]> newPoints = [];
            bool isRotatedLabelIntersect = false;

            padding += CalculateRotatedPaddingDelta(angle, islabelInside, isOpposed);

            bool isEndAnchor = CalculateIsEndAnchor(isOpposed, islabelInside, angle);
            isEndAnchor = Chart is not null && Chart.EnableRtl ? !isEndAnchor : isEndAnchor;

            LabelIntersectAction? labelIntersectAction = axis.Renderer?.LabelIntersectAction;
            ChartAxisRenderer axisRenderer = axis.Renderer ?? null!;

            for (int i = 0, len = axisRenderer.VisibleLabels.Count; i < len; i++)
            {
                VisibleLabels label = axis.Renderer?.VisibleLabels[i] ?? null!;
                label.TemplateID = axis.LabelTemplate is not null ? axis.Renderer?.Chart?.ID + axis.Renderer?.Index + "_AxisLabelTemplate_" + i : string.Empty;

                bool isAxisBreakLabel = ChartHelper.IsBreakLabel(label.OriginalText);
                double pointX = (ChartHelper.ValueToCoefficient(label.Value, axisRenderer) * rect.Width) + rect.X;

                labelWidth = isAxisBreakLabel ? label.BreakLabelSize.Width : label.Size.Width;
                double width = CalculateLabelWidth(axis, labelIntersectAction, angle, intervalLength, labelWidth, label);

                label.Size.Height = axis.LabelTemplate is not null ? (axis.Renderer?.MaxLabelSize.Height < label.Size.Height ? axis.Renderer.MaxLabelSize.Height : label.Size.Height) : label.Size.Height;
                double labelHeight = label.Size.Height / 4;
                bool isWrappedText = label.TextArr.Length > 1 && labelIntersectAction == LabelIntersectAction.Wrap;

                pointX = AdjustPointXForAngle(axis, labelHeight, width, angle, isWrappedText, pointX);

                pointY = CalculatePointY(rect, islabelInside, angle, isOpposed, padding, labelHeight, label, scrollBarHeight);

                TextOptions options = CreateTextOptions(pointX, pointY, label, labelStyle, chart, index, i, isEndAnchor);

                double longestTextWidth = 0;
                int longestTextIndex = -1;

                double chartBorderStartX = (chart._chartBorderRenderer?.ChartBorder?.Width ?? 0) * 0.5;
                double chartBorderEndX = chartBorderStartX + (chart.AvailableSize.Width - (chart._chartBorderRenderer?.ChartBorder?.Width ?? 0));

                CalculateLongestText(label, labelStyle, axis, ref longestTextWidth, ref longestTextIndex, intervalLength);

                if (angle == 0)
                {
                    bool skip = ProcessEdgeLabelPlacementZeroAngle(axis, axisRenderer, rect, ref intervalLength, ref labelWidth, ref pointX, ref previousEnd, label,
                        labelStyle, isWrappedText, labelIntersectAction, width, isOpposed, chartBorderStartX, chartBorderEndX, i, len, options, longestTextWidth);

                    if (skip)
                    {
                        continue;
                    }
                }
                else if (axisRenderer.EdgeLabelPlacement == EdgeLabelPlacement.Shift)
                {
                    HandleRotatedEdgeLabelShift(axis, rect, label, labelStyle, angle, isWrappedText, longestTextWidth, longestTextIndex, chartBorderStartX,
                        chartBorderEndX, i, len, ref pointX, ref previousEnd, options, pointY);
                }

                if (axis.LabelTemplate is null)
                {
                    List<string> text = GetLabelText(label, axis, intervalLength);
                    options.Text = text.FirstOrDefault() ?? string.Empty;
                    if (ChartHelper.IsBreakLabel(label.OriginalText) || (labelIntersectAction == LabelIntersectAction.Wrap))
                    {
                        options.TextCollection = text;
                    }
                }

                if (angle == 0 && labelIntersectAction == LabelIntersectAction.Hide && i != 0 && (!axis.IsAxisInverse ? Convert.ToDouble(options.X, Culture) <= previousEnd : Convert.ToDouble(options.X, Culture) + width >= previousEnd))
                {
                    continue;
                }

                previousEnd = axis.IsAxisInverse ? Convert.ToDouble(options.X, Culture) : isWrappedText ? Convert.ToDouble(options.X, Culture) + (longestTextWidth / 2) : Convert.ToDouble(options.X, Culture) + width;

                if (angle != 0)
                {
                    ApplyRotationOptions(axis, label, labelHeight, angle, isOpposed, pointX, pointY, ref isRotatedLabelIntersect, newPoints, labelIntersectAction, i, options);
                }

                if (angle != 90 && axisRenderer.EdgeLabelPlacement == EdgeLabelPlacement.Hide)
                {
                    if (ShouldSkipEdgeLabelForHide(axis, rect, width, chartBorderEndX, i, len, options))
                    {
                        continue;
                    }
                }

                if (_previousAxisEnd != 0 && _previousAxis is not null && _previousAxis.IsAxisOpposedPosition == isOpposed && axis.ColumnIndex != _previousAxis.ColumnIndex)
                {
                    if (ProcessPreviousAxisOverlap(axis, width, i, len, options))
                    {
                        continue;
                    }
                }

                if ((i == len - 1 && !axis.IsAxisInverse) || (axis.IsAxisInverse && i == 0))
                {
                    _previousAxisEnd = axis.IsAxisInverse ? previousEnd + width : previousEnd;
                }

                string[] locations = ChartHelper.AppendTextElements(chart, options.Id, Convert.ToDouble(options.X, Culture), Convert.ToDouble(options.Y, Culture));
                options.X = locations[0];
                options.Y = locations[1];
                options.Font = labelStyle;
                options.IsMinus = isOpposed != (labelPosition == AxisPosition.Inside);
                options.IsRotatedLabelIntersect = isRotatedLabelIntersect;
                options.TextAnchor = isWrappedText && angle == 0 ? "middle" : isEndAnchor ? "end" : "";
                axis.Renderer?.AxisRenderInfo.AxisLabelOptions.Add(options);

                if (axis.LabelTemplate is not null)
                {
                    AddAxisLabelTemplate(axis, index, i, rect, angle, islabelInside, isOpposed, intervalLength, label, options);
                }
            }

            if (axis.Renderer?.AxisRenderInfo.AxisLabelOptions.Count > 0)
            {
                _previousStartX = Convert.ToDouble(axis.Renderer.AxisRenderInfo.AxisLabelOptions[axis.IsInversed ? axis.Renderer.AxisRenderInfo.AxisLabelOptions.Count - 1 : 0].X, Culture);
            }

            _previousAxis = axis;
        }

        /// <summary>
        /// Calculates and renders Y-axis labels with support for rotation, edge placement, and templates.
        /// </summary>
        /// <param name="axis">The chart axis to calculate labels for.</param>
        /// <param name="index">The axis index in the chart's axis collection.</param>
        /// <param name="rect">The axis rectangle defining the rendering area.</param>
        internal void CalculateYAxisLabels(ChartAxis axis, int index, Rect rect)
        {
            if (ShouldSkipAdaptiveRendering())
            {
                return;
            }

            YAxisLabelContext context = InitializeYAxisContext(axis, rect);
            double previousEnd = !axis.IsAxisInverse ? (rect.Y + rect.Height) : rect.Y;

            ChartAxisRenderer axisRenderer = axis.Renderer ?? null!;

            for (int i = 0, len = context.VisibleLabelsCount; i < len; i++)
            {
                AssignTemplateID(axis, i);

                VisibleLabels label = axisRenderer.VisibleLabels[i];
                bool isAxisBreakLabel = ChartHelper.IsBreakLabel(label.OriginalText ?? string.Empty);

                AdjustLabelSizeForTemplate(axis, i);

                double pointX = CalculateYAxisLabelX(axis, rect, context, i);
                Size elementSize = isAxisBreakLabel ? label.BreakLabelSize : label.Size;

                double pointY = CalculateYAxisLabelY(axis, rect, label, context.IntervalHeight, i, isAxisBreakLabel, elementSize, context.IsLabelInside);
                bool isEndAnchor = DetermineYAxisAnchor(context.IsOpposed, context.IsLabelInside);
                double labelHeight = CalculateYAxisLabelHeight(isAxisBreakLabel, axis, label);

                TextOptions options = CreateYAxisTextOptions(axis, index, i, pointX, pointY, label, context.LabelStyle, isEndAnchor, isAxisBreakLabel);

                if (ShouldHideLabelForIntersection(axis, context, i, options, labelHeight, previousEnd))
                {
                    continue;
                }

                previousEnd = !axis.IsAxisInverse ? Convert.ToDouble(options.Y, Culture) : Convert.ToDouble(options.Y, Culture) + labelHeight;
                bool skipRest = ApplyEdgeLabelPlacementAndMaybeSkip(axis, rect, context, i, len, options, elementSize, label, context.IntervalHeight);

                if (skipRest)
                {
                    continue;
                }

                if (context.Angle != 0 && !string.IsNullOrEmpty(options.Text))
                {
                    ApplyYAxisRotation(options, context.Angle, context.LabelMaxWidth, elementSize);
                }

                FinalizeYAxisLabelOptions(axis, options, context.LabelStyle, index, i, label, context.IntervalHeight);
            }
        }

        #endregion

        #region Helper Classes

        /// <summary>
        /// Context data for X-axis grid line calculation.
        /// </summary>
        private struct XAxisGridContext
        {
            internal double Ticks { get; set; }
            internal double AxisLineSize { get; set; }
            internal bool IsTickInside { get; set; }
            internal double ScrollBarHeight { get; set; }
            internal double TicksBetweenLabel { get; set; }
        }

        /// <summary>
        /// Context data for Y-axis grid line calculation.
        /// </summary>
        private struct YAxisGridContext
        {
            internal double Ticks { get; set; }
            internal double AxisLineSize { get; set; }
            internal double TickSize { get; set; }
            internal double ScrollBarHeight { get; set; }
            internal bool IsTickInside { get; set; }
            internal bool IsCategoryType { get; set; }
            internal double TicksBetweenLabel { get; set; }
        }

        /// <summary>
        /// Context data for minor line calculation.
        /// </summary>
        private struct MinorLineContext
        {
            internal bool IsTickInside { get; set; }
            internal double TickSize { get; set; }
            internal double LogInterval { get; set; }
            internal double LogPosition { get; set; }
            internal double TempInterval { get; set; }
        }

        /// <summary>
        /// Context data for Y-axis label calculation.
        /// </summary>
        private sealed class YAxisLabelContext
        {
            internal double Angle { get; set; }
            internal double IntervalHeight { get; set; }
            internal AxisPosition? LabelPosition { get; set; }
            internal bool IsLabelInside { get; set; }
            internal double Padding { get; set; }
            internal bool IsOpposed { get; set; }
            internal ChartFontOptions LabelStyle { get; set; } = null!;
            internal double LabelMaxWidth { get; set; }
            internal int VisibleLabelsCount { get; set; }
        }
        #endregion
    }
}
