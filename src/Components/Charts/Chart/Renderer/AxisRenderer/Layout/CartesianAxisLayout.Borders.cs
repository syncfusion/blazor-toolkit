
namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    internal partial class CartesianAxisLayout
    {
        #region Private Methods

        /// <summary>
        /// Returns whether the supplied point is a border tick that must be drawn.
        /// </summary>
        /// <param name="axis">Axis context.</param>
        /// <param name="index">Label index.</param>
        /// <param name="point">Coordinate point value.</param>
        /// <returns><see langword="true"/> when border should be rendered.</returns>
        private bool IsBorder(ChartAxis axis, int index, double point)
        {
            bool isHorizontal = axis.Renderer?.Orientation == Orientation.Horizontal;
            double start = isHorizontal ? SeriesClipRect?.X ?? 0 : SeriesClipRect?.Y ?? 0;
            double size = isHorizontal ? SeriesClipRect?.Width ?? 0 : SeriesClipRect?.Height ?? 0;
            int firstLabelIndex = isHorizontal ? 0 : ((axis.Renderer?.VisibleLabels.Count - 1) ?? -1);
            int lastLabelIndex = isHorizontal ? ((axis.Renderer?.VisibleLabels.Count - 1) ?? -1) : 0;
            ChartAreaBorder border = Chart?._chartAreaRenderer?.Area?.Border ?? null!;

            if (axis.PlotOffset > 0)
            {
                return true;
            }
            else if ((Math.Round(point) == Math.Round(start) || point == (start + size)) && (border.Width <= 0 || border.Color == Constants.Transparent))
            {
                return true;
            }
            else if ((point != start && index == firstLabelIndex) || (point != (start + size) && index == lastLabelIndex))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculates the X-axis label border and border path for the axis.
        /// </summary>
        /// <param name="axis">Axis.</param>
        /// <param name="index">Axis index.</param>
        /// <param name="axisRect">Bounding rectangle for the axis labels.</param>
        private void CalculateXAxisBorder(ChartAxis axis, int index, Rect axisRect)
        {
            if (axis.Border.Width > 0)
            {
                ChartAxisRenderer? axisRenderer = axis.Renderer;
                double scrollBarHeight = axis.Renderer?.LabelPosition == AxisPosition.Outside ? axis.ScrollBarHeight : 0;
                bool isOpposed = axis.IsAxisOpposedPosition;
                double startY = axisRect.Y + ((isOpposed ? -1 : 1) * scrollBarHeight);
                double padding = 10;
                double visibleRangeDelta = axisRenderer?.VisibleRange.Delta ?? 0;
                double interval = axis.ValueType == ValueType.DateTime ? (axisRenderer?.DateTimeInterval ?? 0) : (axisRenderer?.VisibleInterval ?? 0);
                double gap = visibleRangeDelta == 0 ? 0 : axisRect.Width / visibleRangeDelta * interval;
                double length = (axisRenderer?.MaxLabelSize.Height ?? 0) + ((axis.TickPosition == axis.Renderer?.LabelPosition) ? axis.Renderer.MajorTickLinesHeight : 0);
                double ticksBetweenLabel = (axis.ValueType == ValueType.Category && axis.LabelPlacement == LabelPlacement.BetweenTicks) ? -0.5 : 0;
                double endY = ((isOpposed && axis.Renderer?.LabelPosition == AxisPosition.Inside) || (!isOpposed && axis.Renderer?.LabelPosition == AxisPosition.Outside)) ?
                    (axisRect.Y + length + padding + scrollBarHeight) : (axisRect.Y - length - padding - scrollBarHeight);

                string labelBorder = BuildXAxisLabelBorder(axis, axisRenderer!, axisRect, gap, ticksBetweenLabel, startY, endY);

                labelBorder += axis.Border.Type == BorderType.Rectangle
                    ? ("M " + SPACE + axisRect.X.ToString(Culture) + SPACE + startY.ToString(Culture) + "L" + SPACE + (axisRect.X + axisRect.Width).ToString(Culture) + SPACE + startY.ToString(Culture))
                    : string.Empty;

                if (!string.IsNullOrEmpty(labelBorder) && axisRenderer is not null)
                {
                    axisRenderer.AxisRenderInfo.AxisBorder = new PathOptions(Chart?.ID + "_BorderLine_" + index, labelBorder, string.Empty,
                        axis.Border.Width, !string.IsNullOrEmpty(axis.Border.Color) ? axis.Border.Color : Chart?._chartThemeStyle?.AxisLine ?? string.Empty, 1, Constants.Transparent);
                }
            }

            SetXAxisMultiLevelLabelOptions(axis, axisRect);
        }

        /// <summary>
        /// Helper to set multi-level label options for X axis or clear when not present.
        /// </summary>
        /// <param name="axis">Axis instance.</param>
        /// <param name="axisRect">Axis rectangle.</param>
        private static void SetXAxisMultiLevelLabelOptions(ChartAxis axis, Rect axisRect)
        {
            if (axis.Renderer?.MultiLevelLabelRenderer is not null)
            {
                if (axis.MultiLevelLabels.Count > 0)
                {
                    axis.Renderer.MultiLevelLabelRenderer.CalculateXAxisMultiLevelLabels(axis.Renderer.Index, axisRect);
                }
                else
                {
                    axis.Renderer.MultiLevelLabelRenderer.ClearPathOptions();
                }
            }
        }

        /// <summary>
        /// Helper to set multi-level label options for Y axis or clear when not present.
        /// </summary>
        /// <param name="axis">Axis instance.</param>
        /// <param name="rect">Axis rectangle.</param>
        private static void SetYAxisMultiLevelLabelOptions(ChartAxis axis, Rect rect)
        {
            if (axis.Renderer?.MultiLevelLabelRenderer is not null)
            {
                if (axis.MultiLevelLabels.Count > 0)
                {
                    axis.Renderer.MultiLevelLabelRenderer.CalculateYAxisMultiLevelLabels(axis.Renderer.Index, rect);
                }
                else
                {
                    axis.Renderer.MultiLevelLabelRenderer.ClearPathOptions();
                }
            }
        }

        /// <summary>
        /// Builds the X axis label border path string by iterating visible labels.
        /// </summary>
        /// <param name="axis">Axis instance.</param>
        /// <param name="axisRenderer">Axis renderer.</param>
        /// <param name="axisRect">Axis rectangle.</param>
        /// <param name="gap">Gap between labels.</param>
        /// <param name="ticksBetweenLabel">Tick offset for categories.</param>
        /// <param name="startY">Start Y coordinate.</param>
        /// <param name="endY">End Y coordinate.</param>
        /// <returns>SVG path string for axis border.</returns>
        private string BuildXAxisLabelBorder(ChartAxis axis, ChartAxisRenderer axisRenderer, Rect axisRect, double gap, double ticksBetweenLabel, double startY, double endY)
        {
            string labelBorder = string.Empty;
            int len = axisRenderer?.VisibleLabels.Count ?? 0;

            for (int i = 0; i < len; i++)
            {
                double pointX = ChartHelper.ValueToCoefficient((axisRenderer?.VisibleLabels[i].Value ?? 0) + ticksBetweenLabel, axisRenderer ?? null!);
                pointX = (axis.IsAxisInverse ? (1 - pointX) : pointX) * axisRect.Width;

                double startX, endX;
                if (axis.ValueType == ValueType.Category && axis.LabelPlacement == LabelPlacement.BetweenTicks)
                {
                    startX = pointX + axisRect.X;
                    endX = pointX + gap + axisRect.X;
                }
                else
                {
                    startX = pointX - (gap * 0.5) + axisRect.X;
                    endX = pointX + (gap * 0.5) + axisRect.X;
                }

                labelBorder = AppendXAxisLabelBorder(axis, axisRenderer!, axisRect, labelBorder, i, startX, endX, startY, endY);
            }

            return labelBorder;
        }

        /// <summary>
        /// Appends X-axis label border pieces based on border type.
        /// </summary>
        private string AppendXAxisLabelBorder(ChartAxis axis, ChartAxisRenderer axisRenderer, Rect axisRect, string labelBorder, int index, double startX, double endX, double startY, double endY)
        {
            return axis.Border.Type switch
            {
                BorderType.Rectangle or BorderType.WithoutTopBorder => AppendRectangleOrWithoutTopBorderXAxis(axisRenderer, axisRect, labelBorder, index, startX, endX, startY, endY),
                BorderType.WithoutTopandBottomBorder => AppendWithoutTopAndBottomBorderXAxis(axisRect, labelBorder, startX, endX, startY, endY),
                BorderType.Brace => "",
                BorderType.WithoutBorder => "",
                BorderType.CurlyBrace => "",
                BorderType.Auto => "",
                _ => labelBorder,
            };
        }

        /// <summary>
        /// Appends rectangular X-axis border segments (handles Rectangle and WithoutTopBorder types).
        /// </summary>
        private string AppendRectangleOrWithoutTopBorderXAxis(ChartAxisRenderer axisRenderer, Rect axisRect, string labelBorder, int i, double startX, double endX, double startY, double endY)
        {
            if (startX < axisRect.X)
            {
                labelBorder += "M" + SPACE + axisRect.X.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + "L" + SPACE + endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE;
            }
            else if (Math.Floor(endX) > axisRect.Width + axisRect.X && !(axisRenderer?.VisibleLabels.Count == 1))
            {
                labelBorder += "M" + SPACE + startX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + "L" + SPACE +
                               startX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + "L" + SPACE +
                               (axisRect.Width + axisRect.X).ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE;
            }
            else
            {
                labelBorder += "M" + SPACE + startX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + "L" + SPACE +
                               startX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + "L" + SPACE +
                               endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE;

                if (i == 0)
                {
                    labelBorder += "M" + SPACE + startX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + "L" + SPACE +
                                   startX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + "M " +
                                   startX.ToString(Culture) + SPACE + endY.ToString(Culture) + " L " +
                                   axisRect.X.ToString(Culture) + SPACE + endY.ToString(Culture);
                }

                if (i == axisRenderer?.VisibleLabels.Count - 1)
                {
                    labelBorder += "M" + SPACE + endX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + "L" + SPACE +
                                   endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + "M " +
                                   endX.ToString(Culture) + SPACE + endY.ToString(Culture) + " L " +
                                   (axisRect.Width + axisRect.X).ToString(Culture) + SPACE + endY.ToString(Culture);
                }
            }

            return labelBorder;
        }

        /// <summary>
        /// Appends X-axis vertical lines only (WithoutTopandBottomBorder type).
        /// </summary>
        private string AppendWithoutTopAndBottomBorderXAxis(Rect axisRect, string labelBorder, double startX, double endX, double startY, double endY)
        {
            if (!(startX < axisRect.X) && !(Math.Floor(endX) > axisRect.Width + axisRect.X))
            {
                labelBorder += "M" + SPACE + startX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + "L" + SPACE +
                               startX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + "M " +
                               endX.ToString(Culture) + SPACE + startY.ToString(Culture) + " L " +
                               endX.ToString(Culture) + SPACE + endY.ToString(Culture);
            }

            return labelBorder;
        }

        /// <summary>
        /// Calculates the Y-axis label border and border path for the axis.
        /// </summary>
        /// <param name="axis">Axis.</param>
        /// <param name="index">Axis index.</param>
        /// <param name="rect">Axis rectangle.</param>
        private void CalculateYAxisBorder(ChartAxis axis, int index, Rect rect)
        {
            if (axis.Border.Width > 0)
            {
                ChartAxisRenderer? axisRenderer = axis.Renderer;
                bool isOpposed = axis.IsAxisOpposedPosition;
                double scrollBarHeight = (isOpposed ? 1 : -1) * (axis.Renderer?.LabelPosition == AxisPosition.Outside ? axis.ScrollBarHeight : 0);
                double visibleRangeDelta = axisRenderer?.VisibleRange.Delta ?? 0;
                double interval = axis.ValueType == ValueType.DateTime ? (axisRenderer?.DateTimeInterval ?? 0) : (axisRenderer?.VisibleInterval ?? 0);
                double gap = visibleRangeDelta == 0 ? 0 : rect.Height / visibleRangeDelta * interval;
                double length = (axisRenderer?.MaxLabelSize.Width ?? 0) + 10 + ((axis.TickPosition == axis.Renderer?.LabelPosition) ? axis.Renderer.MajorTickLinesHeight : 0) + (!string.IsNullOrEmpty(axis.Title) ? 5 : 0);
                double ticksBetweenLabel = (axis.ValueType == ValueType.Category && axis.LabelPlacement == LabelPlacement.BetweenTicks) ? -0.5 : 0;
                double endX = ((isOpposed && axis.Renderer?.LabelPosition == AxisPosition.Inside) || (!isOpposed
                    && axis.Renderer?.LabelPosition == AxisPosition.Outside)) ? rect.X - length + scrollBarHeight : rect.X + length + scrollBarHeight;

                string labelBorder = BuildYAxisLabelBorder(axis, axisRenderer!, rect, gap, ticksBetweenLabel, endX, scrollBarHeight);

                labelBorder += (axis.Border.Type == BorderType.Rectangle)
                    ? ('M' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + rect.Y.ToString(Culture) + SPACE + 'L' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + (rect.Y + rect.Height).ToString(Culture) + SPACE)
                    : string.Empty;

                if (!string.IsNullOrEmpty(labelBorder) && axisRenderer is not null)
                {
                    axisRenderer.AxisRenderInfo.AxisBorder = new PathOptions(Chart?.ID + "_BorderLine_" + index, labelBorder, string.Empty,
                        axis.Border.Width, !string.IsNullOrEmpty(axis.Border.Color) ? axis.Border.Color : Chart?._chartThemeStyle?.AxisLine ?? string.Empty, 1, Constants.Transparent);
                }
            }

            SetYAxisMultiLevelLabelOptions(axis, rect);
        }

        /// <summary>
        /// Builds the Y axis label border path string by iterating visible labels.
        /// </summary>
        private string BuildYAxisLabelBorder(ChartAxis axis, ChartAxisRenderer axisRenderer, Rect rect, double gap, double ticksBetweenLabel, double endX, double scrollBarHeight)
        {
            string labelBorder = string.Empty;
            int len = axisRenderer?.VisibleLabels.Count ?? 0;

            for (int i = 0; i < len; i++)
            {
                double pointY = ChartHelper.ValueToCoefficient((axisRenderer?.VisibleLabels[i].Value ?? 0) + ticksBetweenLabel, axisRenderer ?? null!);
                pointY = (axis.IsAxisInverse ? (1 - pointY) : pointY) * rect.Height;

                double startY, endY;
                if (axis.ValueType == ValueType.Category && axis.LabelPlacement == LabelPlacement.BetweenTicks)
                {
                    startY = (pointY * -1) + (rect.Y + rect.Height);
                    endY = (pointY * -1) - gap + (rect.Y + rect.Height);
                }
                else
                {
                    startY = (pointY * -1) + (gap / 2) + (rect.Y + rect.Height);
                    endY = (pointY * -1) - (gap / 2) + (rect.Y + rect.Height);
                }

                labelBorder = AppendYAxisLabelBorder(axis, axisRenderer!, rect, labelBorder, i, startY, endY, endX, scrollBarHeight);
            }

            return labelBorder;
        }

        /// <summary>
        /// Helper to append Y-axis label border pieces based on border type.
        /// </summary>
        /// <summary>
        /// Appends Y-axis label border pieces based on border type.
        /// </summary>
        private string AppendYAxisLabelBorder(ChartAxis axis, ChartAxisRenderer axisRenderer, Rect rect, string labelBorder, int i, double startY, double endY, double endX, double scrollBarHeight)
        {
            switch (axis.Border.Type)
            {
                case BorderType.Rectangle:
                case BorderType.WithoutTopBorder:
                    return AppendRectangleOrWithoutTopBorderYAxis(axisRenderer, rect, labelBorder, i, startY, endY, endX, scrollBarHeight);
                case BorderType.WithoutTopandBottomBorder:
                    return AppendWithoutTopAndBottomBorderYAxis(rect, labelBorder, startY, endY, endX, scrollBarHeight);
                default:
                    return labelBorder;
            }
        }

        /// <summary>
        /// Appends rectangular Y-axis border segments (handles Rectangle and WithoutTopBorder types).
        /// </summary>
        private string AppendRectangleOrWithoutTopBorderYAxis(ChartAxisRenderer axisRenderer, Rect rect, string labelBorder, int i, double startY, double endY, double endX, double scrollBarHeight)
        {
            if (startY > (rect.Y + rect.Height))
            {
                labelBorder += 'M' + SPACE + endX.ToString(Culture) + SPACE + (rect.Y + rect.Height).ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE;
            }
            else if (Math.Floor(rect.Y) > endY)
            {
                labelBorder += 'M' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + rect.Y.ToString(Culture) + SPACE;
            }
            else
            {
                labelBorder += 'M' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE;
                if (i == axisRenderer?.VisibleLabels.Count - 1)
                {
                    labelBorder += 'M' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE;
                }
            }

            return labelBorder;
        }

        /// <summary>
        /// Appends Y-axis horizontal lines only (WithoutTopandBottomBorder type).
        /// </summary>
        private string AppendWithoutTopAndBottomBorderYAxis(Rect rect, string labelBorder, double startY, double endY, double endX, double scrollBarHeight)
        {
            if (!(startY > rect.Y + rect.Height) && !(endY < Math.Floor(rect.Y)))
            {
                labelBorder += 'M' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + 'M' + SPACE + endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + 'L' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + endY.ToString(Culture);
            }

            return labelBorder;
        }

        /// <summary>
        /// Calculates a row border for an axis if the row exists.
        /// </summary>
        /// <param name="axis">Axis instance.</param>
        /// <param name="rect">Axis bounds.</param>
        private void CalculateRowBorder(ChartAxis axis, Rect rect)
        {
            if (axis.Container?._rowContainer?.Elements.Count > (int)axis.RowIndex)
            {
                ChartRow? row = axis.Container._rowContainer.Elements[(int)axis.RowIndex] as ChartRow;
                string borderDirection = "M " + rect.X.ToString(Culture) + ", " + (rect.Y + rect.Height).ToString(Culture) + " L " + (rect.X + ((SeriesClipRect?.Width ?? 0) * (axis.IsAxisOpposedPosition ? -1 : 1))).ToString(Culture) + "," + (rect.Y + rect.Height).ToString(Culture);
                if (axis.Renderer is { })
                {
                    axis.Renderer.AxisRenderInfo.RowBorder = new PathOptions(axis.Renderer.Owner?.ID + "_RowBorder_" + (int)axis.RowIndex, borderDirection, "", row?.BorderWidth ?? 1, row?.BorderColor ?? null!);
                }
            }
        }

        /// <summary>
        /// Calculates a column border for an axis if the column exists.
        /// </summary>
        /// <param name="axis">Axis instance.</param>
        /// <param name="rect">Axis bounds.</param>
        private void CalculateColumnBorder(ChartAxis axis, Rect rect)
        {
            if (axis.Container?._columnContainer?.Elements.Count > (int)axis.ColumnIndex)
            {
                ChartColumn? column = axis.Container._columnContainer.Elements[(int)axis.ColumnIndex] as ChartColumn;
                string borderDirection = "M " + rect.X.ToString(Culture) + ", " + rect.Y.ToString(Culture) + " L " + rect.X.ToString(Culture) + "," + (rect.Y + ((SeriesClipRect?.Height ?? 0) * (axis.IsAxisOpposedPosition ? 1 : -1))).ToString(Culture);
                if (axis.Renderer is { })
                {
                    axis.Renderer.AxisRenderInfo.ColumnBorder = new PathOptions(axis.Renderer.Owner?.ID + "_ColumnBorder_" + (int)axis.ColumnIndex, borderDirection, "", column?.BorderWidth ?? 1, column?.BorderColor ?? null!);
                }
            }
        }
        #endregion
    }
}
