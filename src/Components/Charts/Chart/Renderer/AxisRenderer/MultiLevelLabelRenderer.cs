using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Responsible for measuring, calculating and rendering multi level axis labels and their borders.
    /// </summary>
    internal class MultiLevelLabelRenderer
    {
        #region Constants
        private const string SPACE = " ";
        #endregion

        #region Fields
        private double[]? _y_AxisMultiLabelHeight;
        private double[]? _y_AxisPrevHeight;
        private double[]? _x_AxisMultiLabelHeight;
        private double[]? _x_AxisPrevHeight;
        private CultureInfo _culture = CultureInfo.InvariantCulture;
        private ChartAxis? _axis;
        private ChartAxisRenderer? _axisRenderer;
        private SfChart? _chart;
        private string? _clipPathID;
        private string? _groupID;
        private Rect? _clipRect;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the border path options produced while calculating multi level label borders.
        /// </summary>
        internal List<PathOptions> BorderOptions { get; set; } = [];

        /// <summary>
        /// Gets the text rendering options produced while calculating multi level labels.
        /// </summary>
        internal List<TextOptions> TextOptions { get; set; } = [];
        #endregion

        #region Private Methods

        /// <summary>
        /// Computes heights for each multi level label group and stores cached per-level heights.
        /// </summary>
        private void GetMultilevelLabelsHeight()
        {
            bool isVertical = _axisRenderer?.Orientation == Orientation.Vertical;
            double axisValue = isVertical ? _axisRenderer?.Rect.Height ?? 0 : _axisRenderer?.Rect.Width ?? 0;
            axisValue = double.IsNaN(axisValue) ? 0 : axisValue;
            List<ChartMultiLevelLabel> multiLevelLabelCollection = _axisRenderer?.Axis?.MultiLevelLabels ?? null!;
            int labelCount = multiLevelLabelCollection.Count;
            double data = 0;
            double padding = 10;
            double[] multiLevelLabelsHeight = new double[labelCount];
            double[] prevHeight = new double[labelCount];

            for (int index = 0; index < labelCount; index++)
            {
                multiLevelLabelsHeight[index] = MeasureGroupMaxHeight(multiLevelLabelCollection[index], isVertical, axisValue, padding);
                prevHeight[index] = data;
                data += !double.IsNaN(multiLevelLabelsHeight[index]) ? (multiLevelLabelsHeight[index] + padding) : 0;
            }

            if (_axisRenderer is { })
            {
                _axisRenderer.MultiLevelLabelHeight = data + (!string.IsNullOrEmpty(_axisRenderer.Axis?.Title) || (_chart?._legendRenderer is not null && _chart._legendRenderer.LegendSettings is not null && _chart._legendRenderer.LegendSettings.Visible) ? padding / 2 : 0);
            }

            if (isVertical)
            {
                _y_AxisMultiLabelHeight = multiLevelLabelsHeight;
                _y_AxisPrevHeight = prevHeight;
            }
            else
            {
                _x_AxisMultiLabelHeight = multiLevelLabelsHeight;
                _x_AxisPrevHeight = prevHeight;
            }
        }

        /// <summary>
        /// Measures the maximum height for a multi-level label group.
        /// </summary>
        /// <param name="multiLevel">The multi-level label group.</param>
        /// <param name="isVertical">Whether axis is vertical.</param>
        /// <param name="axisValue">Length of the axis (px).</param>
        /// <param name="padding">Padding to apply for wrapped labels.</param>
        /// <returns>Maximum height required for the group.</returns>
        private double MeasureGroupMaxHeight(ChartMultiLevelLabel multiLevel, bool isVertical, double axisValue, double padding)
        {
            double maxHeight = 0;

            foreach (ChartCategory categoryLabel in multiLevel.Categories)
            {
                if (!string.IsNullOrEmpty(categoryLabel.Text) && categoryLabel.Start is not null && categoryLabel.End is not null)
                {
                    Size labelSize = ChartHelper.MeasureText(categoryLabel.Text, multiLevel.TextStyle.GetChartFontOptions());
                    double height = isVertical ? labelSize.Width : labelSize.Height + ((2 * multiLevel.Border.Width) + (multiLevel.Border.Type == BorderType.CurlyBrace ? padding : 0));
                    double gap = categoryLabel.MaximumTextWidth != 0 ? categoryLabel.MaximumTextWidth : (ChartHelper.ValueToCoefficient(_axisRenderer?.GetDoubleValue(categoryLabel.End) ?? 0, _axisRenderer ?? null!) * axisValue) - (ChartHelper.ValueToCoefficient(_axisRenderer?.GetDoubleValue(categoryLabel.Start) ?? 0, _axisRenderer ?? null!) * axisValue);
                    if (labelSize.Width > (gap - padding) && gap > 0 && multiLevel.Overflow == TextOverflow.Wrap && !isVertical)
                    {
                        height *= ChartHelper.TextWrap(categoryLabel.Text, gap - padding, multiLevel.TextStyle.GetChartFontOptions()).Count;
                    }
                    maxHeight = Math.Max(height, maxHeight);
                }
            }
            return maxHeight;
        }

        /// <summary>
        /// Adds a border path option entry from the computed path.
        /// </summary>
        /// <param name="borderIndex">Multi-level border index.</param>
        /// <param name="axisIndex">Axis index.</param>
        /// <param name="path">SVG path data.</param>
        /// <param name="pointIndex">Category point index.</param>
        private void CalculateBorderElement(int borderIndex, int axisIndex, string path, int pointIndex)
        {
            PathOptions pathOption = new()
            {
                Id = _chart?.ID + axisIndex + "_Axis_MultiLevelLabel_Rect_" + borderIndex + '_' + pointIndex,
                Fill = Constants.Transparent,
                StrokeWidth = _axis?.MultiLevelLabels[borderIndex].Border.Width ?? 1,
                Stroke = !string.IsNullOrEmpty(_axis?.MultiLevelLabels[borderIndex].Border.Color) ? _axis.MultiLevelLabels[borderIndex].Border.Color : _chart?._chartThemeStyle?.AxisLine ?? string.Empty,
                Opacity = 1,
                StrokeDashArray = string.Empty,
                Direction = path
            };
            pathOption.Direction = ChartHelper.AppendPathElements(_chart ?? null!, pathOption.Direction, pathOption.Id);
            BorderOptions.Add(pathOption);
        }


        /// <summary>
        /// Processes axis label text and inserts manual line breaks when the markup br is present.
        /// </summary>
        /// <returns>
        /// The same <see cref="TextOptions"/> instance provided in <paramref name="options"/>, updated with line‑broken text segments when applicable.
        /// </returns>
        private static TextOptions AxisLabelBreak(string text, TextOptions options)
        {
            bool isAxisLabelBreak = text.Contains("<br>", StringComparison.InvariantCulture);
            string[] textArr;
            if (isAxisLabelBreak)
            {
                textArr = text.Split("<br>");
                foreach (string textValue in textArr)
                {
                    options.TextCollection.Add(textValue);
                }
            }
            return options;
        }

        /// <summary>
        /// Calculates and returns path string for a X axis category border.
        /// </summary>
        /// <param name="labelIndex">Label level index.</param>
        /// <param name="gap">Available gap width.</param>
        /// <param name="startX">Start X coordinate.</param>
        /// <param name="startY">Start Y baseline.</param>
        /// <param name="labelSize">Measured label size.</param>
        /// <param name="textOptions">Text rendering options.</param>
        /// <param name="axisRect">Axis rectangle.</param>
        /// <param name="alignment">Text alignment.</param>
        /// <param name="path">Initial path (can be empty).</param>
        /// <param name="isOutside">Whether labels are outside axis.</param>
        /// <param name="opposedPosition">Whether axis is opposed.</param>
        /// <param name="categoryIndex">Category index in group.</param>
        /// <returns>SVG path string for the border.</returns>
        private string CalculateXAxisLabelBorder(int labelIndex, double gap, double startX, double startY, Size labelSize, TextOptions textOptions, Rect axisRect, Alignment alignment, string path, bool isOutside, bool opposedPosition, int categoryIndex)
        {
            double padding = 10;
            ChartMultiLevelLabel groupLabel = _axis?.MultiLevelLabels[labelIndex] ?? null!;
            BorderType categoryType = (groupLabel.Categories[categoryIndex].Type == BorderType.Auto) ? groupLabel.Border.Type : groupLabel.Categories[categoryIndex].Type;
            double width = gap + padding;
            double height = (_x_AxisMultiLabelHeight?[labelIndex] ?? 0) + padding;
            double scrollBarHeight = _axis?.Renderer?.LabelPosition == AxisPosition.Outside ? _axis.ScrollBarHeight : 0;
            double x = startX + axisRect.X;
            double y = (!opposedPosition && isOutside) || (opposedPosition && !isOutside) ? (startY + axisRect.Y + (_x_AxisPrevHeight?[labelIndex] ?? 0) + scrollBarHeight) : (axisRect.Y - startY - (_x_AxisPrevHeight?[labelIndex] ?? 0) - scrollBarHeight);

            switch (categoryType)
            {
                case BorderType.WithoutTopandBottomBorder:
                case BorderType.Rectangle:
                case BorderType.WithoutTopBorder:
                    height = ((!opposedPosition && isOutside) || (opposedPosition && !isOutside)) ? height : -height;
                    path += BuildRectangleBorderPathForXAxis(x, y, width, height, categoryType);
                    break;
                case BorderType.Brace:
                    height = (!opposedPosition && isOutside) || (opposedPosition && !isOutside) ? height : -height;
                    path += BuildBraceBorderPathForXAxis(x, y, labelSize, textOptions, alignment, width, height);
                    break;
                case BorderType.CurlyBrace:
                    bool positivePadding = (!opposedPosition && isOutside) || (opposedPosition && !isOutside);
                    path += BuildCurlyBraceBorderPathForXAxis(x, y, width, alignment, positivePadding);
                    break;
                case BorderType.WithoutBorder:
                case BorderType.Auto:
                    break;
                default:
                    break;
            }

            return path;
        }

        /// <summary>
        /// Builds rectangle/line style path for X axis label border.
        /// </summary>
        private string BuildRectangleBorderPathForXAxis(double x, double y, double width, double height, BorderType categoryType)
        {
            string path = string.Empty;

            path += "M " + x.ToString(_culture) + SPACE + y.ToString(_culture) + " L " + x.ToString(_culture) + SPACE + (y + height).ToString(_culture)
                + " M " + (x + width).ToString(_culture) + SPACE + y.ToString(_culture) + " L " + (x + width).ToString(_culture) + SPACE + (y + height).ToString(_culture)
                + (categoryType != BorderType.WithoutTopandBottomBorder ? (" L" + SPACE + x.ToString(_culture) + SPACE + (y + height).ToString(_culture) + SPACE) : SPACE)
                + (categoryType == BorderType.Rectangle ? ("M " + x.ToString(_culture) + SPACE + y.ToString(_culture) + " L " + (x + width).ToString(_culture) + SPACE + y.ToString(_culture)) : SPACE);

            return path;
        }

        /// <summary>
        /// Builds brace style path for X axis label border.
        /// </summary>
        private string BuildBraceBorderPathForXAxis(double x, double y, Size labelSize, TextOptions textOptions, Alignment alignment, double width, double height)
        {
            double data, value1;
            if (alignment == Alignment.Near)
            {
                data = Convert.ToDouble(textOptions.X, _culture);
                value1 = Convert.ToDouble(textOptions.X, _culture) + labelSize.Width + 2;
            }
            else if (alignment == Alignment.Center)
            {
                data = Convert.ToDouble(textOptions.X, _culture) - (labelSize.Width / 2) - 2;
                value1 = Convert.ToDouble(textOptions.X, _culture) + (labelSize.Width / 2) + 2;
            }
            else
            {
                data = Convert.ToDouble(textOptions.X, _culture) - labelSize.Width - 2;
                value1 = Convert.ToDouble(textOptions.X, _culture);
            }

            string path = string.Empty;
            path += "M " + x.ToString(_culture) + SPACE + y.ToString(_culture) + " L " + x.ToString(_culture) + SPACE + (y + (height / 2)).ToString(_culture)
                + " M " + x.ToString(_culture) + SPACE + (y + (height / 2)).ToString(_culture) + " L " + (data - 2).ToString(_culture) + SPACE + (y + (height / 2)).ToString(_culture)
                + " M " + value1.ToString(_culture) + SPACE + (y + (height / 2)).ToString(_culture) + " L " + (x + width).ToString(_culture) + SPACE + (y + (height / 2)).ToString(_culture)
                + " M " + (x + width).ToString(_culture) + SPACE + (y + (height / 2)).ToString(_culture) + " L " + (x + width).ToString(_culture) + SPACE + y.ToString(_culture);
            return path;
        }

        /// <summary>
        /// Builds curly brace style path for X axis label border.
        /// </summary>
        private string BuildCurlyBraceBorderPathForXAxis(double x, double y, double width, Alignment alignment, bool positivePadding)
        {
            double padding = positivePadding ? 10 : -10;
            double padding1 = positivePadding ? 15 : -15;
            double padding2 = positivePadding ? 5 : -5;
            string path = string.Empty;

            if (alignment == Alignment.Center)
            {
                path += "M " + x.ToString(_culture) + SPACE + y.ToString(_culture) + " C " + x.ToString(_culture) + SPACE + y.ToString(_culture) + SPACE + (x + 5).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + SPACE + (x + 10).ToString(_culture) + SPACE +
                    (y + padding).ToString(_culture) + " L " + (x + (width / 2) - 5).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + " L " + (x + (width / 2)).ToString(_culture) + SPACE + (y + padding1).ToString(_culture) +
                    " L " + (x + (width / 2) + 5).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + " L " + (x + width - 10).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + " C " +
                    (x + width - 10).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + SPACE + (x + width).ToString(_culture) + SPACE + (y + padding2).ToString(_culture) + SPACE + (x + width).ToString(_culture) + SPACE + y.ToString(_culture);
            }
            else if (alignment == Alignment.Near)
            {
                path += "M " + x.ToString(_culture) + SPACE + y.ToString(_culture) + " C " + x.ToString(_culture) + SPACE + y.ToString(_culture) + SPACE + (x + 5).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + SPACE + (x + 10).ToString(_culture) + SPACE +
                    (y + padding).ToString(_culture) + " L " + (x + 15).ToString(_culture) + SPACE + (y + padding1).ToString(_culture) + " L " + (x + 20).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + " L " +
                    (x + width - 10).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + " C " + (x + width - 10).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + SPACE + (x + width).ToString(_culture) + SPACE + (y + padding2).ToString(_culture) + SPACE + (x + width).ToString(_culture) + SPACE + y.ToString(_culture);
            }
            else
            {
                path += "M " + x.ToString(_culture) + SPACE + y.ToString(_culture) + " C " + x.ToString(_culture) + SPACE + y.ToString(_culture) + SPACE + (x + 5).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + SPACE + (x + 10).ToString(_culture) + SPACE +
                    (y + padding).ToString(_culture) + " L " + (x + width - 20).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + " L " + (x + width - 15).ToString(_culture) + SPACE + (y + padding1).ToString(_culture) +
                    " L " + (x + width - 10).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + " L " + (x + width - 10).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + " C "
                    + (x + width - 10).ToString(_culture) + SPACE + (y + padding).ToString(_culture) + SPACE + (x + width).ToString(_culture) + SPACE + (y + padding2).ToString(_culture) + SPACE + (x + width).ToString(_culture) + SPACE + y.ToString(_culture);
            }
            return path;
        }

        /// <summary>
        /// Calculates and returns path string for a Y axis category border.
        /// </summary>
        /// <param name="labelIndex">Label level index.</param>
        /// <param name="endY">Category end coefficient*height value.</param>
        /// <param name="startX">Start offset.</param>
        /// <param name="startY">Category start coefficient*height value.</param>
        /// <param name="labelSize">Measured label size.</param>
        /// <param name="textOptions">Text rendering options.</param>
        /// <param name="rect">Axis rectangle.</param>
        /// <param name="alignment">Text alignment.</param>
        /// <param name="path">Initial path (can be empty).</param>
        /// <param name="isOutside">Whether labels are outside axis.</param>
        /// <param name="opposedPosition">Whether axis is opposed.</param>
        /// <param name="categoryIndex">Category index in group.</param>
        /// <returns>SVG path string for the border.</returns>
        private string CalculateYAxisLabelBorder(int labelIndex, double endY, double startX, double startY, Size labelSize, TextOptions textOptions, Rect rect, Alignment alignment, string path, bool isOutside, bool opposedPosition, int categoryIndex)
        {
            double height = endY - startY;
            double padding = 10;
            ChartMultiLevelLabel groupLabel = _axis?.MultiLevelLabels[labelIndex] ?? null!;
            BorderType categoryType = (groupLabel.Categories[categoryIndex].Type == BorderType.Auto) ? groupLabel.Border.Type : groupLabel.Categories[categoryIndex].Type;
            double y = rect.Y + rect.Height - endY;
            double scrollBarHeight = (isOutside && _axis?.CrossesAt is null ? _axis?.ScrollBarHeight ?? 0 : 0) * (opposedPosition ? 1 : -1);
            double width = (_y_AxisMultiLabelHeight?[labelIndex] ?? 0) + padding;
            double x = ((!opposedPosition && isOutside) || (opposedPosition && !isOutside) ? rect.X - startX - (_y_AxisPrevHeight?[labelIndex] ?? 0) : rect.X + startX + (_y_AxisPrevHeight?[labelIndex] ?? 0)) + scrollBarHeight;

            switch (categoryType)
            {
                case BorderType.WithoutTopandBottomBorder:
                case BorderType.Rectangle:
                case BorderType.WithoutTopBorder:
                    width = (!opposedPosition && isOutside) || (opposedPosition && !isOutside) ? -width : width;
                    path += BuildRectangleBorderPathForYAxis(x, y, width, height, categoryType);
                    break;
                case BorderType.Brace:
                    width = (!opposedPosition && isOutside) || (opposedPosition && !isOutside) ? width : -width;
                    path += BuildBraceBorderPathForYAxis(x, y, width, height, textOptions, labelSize);
                    break;
                case BorderType.CurlyBrace:
                    bool positivePadding = (!opposedPosition && isOutside) || (opposedPosition && !isOutside);
                    path += BuildCurlyBraceBorderPathForYAxis(x, y, height, alignment, positivePadding);
                    break;
                case BorderType.WithoutBorder:
                    break;
                case BorderType.Auto:
                    break;
                default:
                    break;
            }

            return path;
        }

        /// <summary>
        /// Builds rectangle/line style path for Y axis label border.
        /// </summary>
        private string BuildRectangleBorderPathForYAxis(double x, double y, double width, double height, BorderType categoryType)
        {
            string path = string.Empty;
            path += "M " + x.ToString(_culture) + SPACE + y.ToString(_culture) + " L " + (x + width).ToString(_culture) + SPACE + y.ToString(_culture)
                + " M " + x.ToString(_culture) + SPACE + (y + height).ToString(_culture) + " L " + (x + width).ToString(_culture) + SPACE + (y + height).ToString(_culture)
                + (categoryType != BorderType.WithoutTopandBottomBorder ? " L" + SPACE + (x + width).ToString(_culture) + SPACE + y.ToString(_culture) + SPACE : SPACE)
                + (categoryType == BorderType.Rectangle ? "M " + x.ToString(_culture) + SPACE + (y + height).ToString(_culture) + "L" + SPACE + x.ToString(_culture) + SPACE + y.ToString(_culture) + SPACE : SPACE);
            return path;
        }

        /// <summary>
        /// Builds brace style path for Y axis label border.
        /// </summary>
        private string BuildBraceBorderPathForYAxis(double x, double y, double width, double height, TextOptions textOptions, Size labelSize)
        {
            string path = string.Empty;
            path += "M " + x.ToString(_culture) + SPACE + y.ToString(_culture) + " L " + (x - (width / 2)).ToString(_culture) + SPACE + y.ToString(_culture)
                + " L " + (x - (width / 2)).ToString(_culture) + SPACE + (Convert.ToDouble(textOptions.Y, _culture) - (labelSize.Height / 2) - 4).ToString(_culture)
                + " M " + (x - (width / 2)).ToString(_culture) + SPACE + (Convert.ToDouble(textOptions.Y, _culture) + (labelSize.Height / 4) + 2).ToString(_culture)
                + " L " + (x - (width / 2)).ToString(_culture) + SPACE + (y + height).ToString(_culture) + " L " + x.ToString(_culture) + SPACE + (y + height).ToString(_culture);
            return path;
        }

        /// <summary>
        /// Builds curly brace style path for Y axis label border.
        /// </summary>
        private string BuildCurlyBraceBorderPathForYAxis(double x, double y, double height, Alignment alignment, bool positivePadding)
        {
            double padding = positivePadding ? -10 : 10;
            double padding1 = positivePadding ? -15 : 15;
            double padding2 = positivePadding ? -5 : 5;
            string path = string.Empty;

            if (alignment == Alignment.Center)
            {
                path += "M " + x.ToString(_culture) + SPACE + y.ToString(_culture) + " C " + x.ToString(_culture) + SPACE + y.ToString(_culture) + SPACE + (x + padding).ToString(_culture) + SPACE + y.ToString(_culture) + SPACE + (x + padding).ToString(_culture) + SPACE + (y + 10).ToString(_culture)
                    + " L " + (x + padding).ToString(_culture) + SPACE + (y + ((height - 10) / 2)).ToString(_culture) + " L " + (x + padding1).ToString(_culture) + SPACE + (y + ((height - 10) / 2) + 5).ToString(_culture)
                    + " L " + (x + padding).ToString(_culture) + SPACE + (y + ((height - 10) / 2) + 10).ToString(_culture) + " L " + (x + padding).ToString(_culture) + SPACE + (y + (height - 10)).ToString(_culture) +
                    " C " + (x + padding).ToString(_culture) + SPACE + (y + (height - 10)).ToString(_culture) + SPACE + (x + padding2).ToString(_culture) + SPACE + (y + height).ToString(_culture) + SPACE + x.ToString(_culture) + SPACE + (y + height).ToString(_culture);
            }
            else if (alignment == Alignment.Far)
            {
                path += "M " + x.ToString(_culture) + SPACE + y.ToString(_culture) + " C " + x.ToString(_culture) + SPACE + y.ToString(_culture) + SPACE + (x + padding).ToString(_culture) + SPACE + y.ToString(_culture) + SPACE + (x + padding).ToString(_culture) + SPACE + (y + 10).ToString(_culture)
                    + " L " + (x + padding).ToString(_culture) + SPACE + (y + height - 20).ToString(_culture) + SPACE + " L " + (x + padding1).ToString(_culture) + SPACE + (y + (height - 15)).ToString(_culture) +
                    " L " + (x + padding).ToString(_culture) + SPACE + (y + (height - 10)).ToString(_culture) + " L " + (x + padding).ToString(_culture) + SPACE + (y + (height - 10)).ToString(_culture) +
                    " C " + (x + padding).ToString(_culture) + SPACE + (y + (height - 10)).ToString(_culture) + SPACE + (x + padding).ToString(_culture) + SPACE + (y + height).ToString(_culture) + SPACE + x.ToString(_culture) + SPACE + (y + height).ToString(_culture);
            }
            else
            {
                path += "M " + x.ToString(_culture) + SPACE + y.ToString(_culture) + " C " + x.ToString(_culture) + SPACE + y.ToString(_culture) + SPACE + (x + padding).ToString(_culture) + SPACE + y.ToString(_culture) + SPACE + (x + padding).ToString(_culture) + SPACE + (y + 10).ToString(_culture)
                    + " L " + (x + padding1).ToString(_culture) + SPACE + (y + 15).ToString(_culture) + " L " + (x + padding).ToString(_culture) + SPACE + (y + 20).ToString(_culture) + " L " + (x + padding).ToString(_culture) + SPACE + (y + (height - 10)).ToString(_culture) +
                    " C " + (x + padding).ToString(_culture) + SPACE + (y + (height - 10)).ToString(_culture) + SPACE + (x + padding2).ToString(_culture) + SPACE + (y + height).ToString(_culture) + SPACE + x.ToString(_culture) + SPACE + (y + height).ToString(_culture);
            }

            return path;
        }

        /// <summary>
        /// Processes one X axis multi level and its categories.
        /// </summary>
        private void ProcessXAxisLevel(int level, int axisIndex, Rect axisRect, double startY, double padding, bool isOutside, bool opposedPosition, double scrollBarHeight)
        {
            ChartMultiLevelLabel multiLevel = _axis?.MultiLevelLabels[level] ?? null!;
            int pointIndex = 0;
            for (int i = 0; i < multiLevel.Categories.Count; i++)
            {
                ChartCategory categoryLabel = multiLevel.Categories[i];
                string pathRect = string.Empty;
                object? start = categoryLabel?.Start?.GetType() == typeof(string) ? Convert.ToDouble(DateTime.Parse((string)categoryLabel.Start, _culture)) : categoryLabel?.Start;
                object? end = categoryLabel?.End?.GetType() == typeof(string) ? Convert.ToDouble(DateTime.Parse((string)categoryLabel.End, _culture)) : categoryLabel?.End;
                if (categoryLabel is null)
                {
                    return;
                }
                AxisMultiLabelRenderEventArgs argsData = new(Constants.AxisMultiLevelLabelRender, false, _axis ?? null!, categoryLabel.CustomAttributes, categoryLabel.Text, multiLevel.TextStyle, multiLevel.Alignment);

                if (_chart?.OnAxisMultiLevelLabelRender is not null)
                {
                    _chart.OnAxisMultiLevelLabelRender.Invoke(argsData);
                }

                if (!argsData.Cancel)
                {
                    double startX = ChartHelper.ValueToCoefficient(_axisRenderer?.GetDoubleValue(start!) ?? 0, _axisRenderer ?? null!) * axisRect.Width;
                    double endX = ChartHelper.ValueToCoefficient(_axisRenderer?.GetDoubleValue(end!) ?? 0, _axisRenderer ?? null!) * axisRect.Width;
                    endX = _axis is not null && _axis.IsAxisInverse ? new double[] { startX, startX = endX }[0] : endX;
                    Size labelSize = ChartHelper.MeasureText(argsData.Text, argsData.TextStyle.GetChartFontOptions());
                    double gap = (ChartHelper.IsNaNOrZero(categoryLabel.MaximumTextWidth) ? endX - startX : categoryLabel.MaximumTextWidth) - padding;
                    double x = startX + axisRect.X + padding;
                    double y = (((opposedPosition && !isOutside) || (!opposedPosition && isOutside)) ? (startY + axisRect.Y + (labelSize.Height / 2) + padding + (_x_AxisPrevHeight?[level] ?? 0)) : (axisRect.Y - startY + (labelSize.Height / 2) -
                            (_x_AxisMultiLabelHeight?[level] ?? 0) - (_x_AxisPrevHeight?[level] ?? 0))) + scrollBarHeight;
                    string anchor;
                    if (argsData.Alignment == Alignment.Center)
                    {
                        x += (endX - startX - padding) / 2;
                        anchor = "middle";
                    }
                    else if (argsData.Alignment == Alignment.Far)
                    {
                        x = x + (endX - startX - padding) - (multiLevel.Border.Width / 2);
                        anchor = "end";
                    }
                    else
                    {
                        anchor = "start";
                        x += multiLevel.Border.Width / 2;
                    }

                    y = multiLevel.Border.Type == BorderType.CurlyBrace ? (((!opposedPosition && isOutside) || (opposedPosition && !isOutside)) ? y + padding : y - (padding / 2)) : y;
                    TextOptions options = new
                    (
                        x.ToString(_culture),
                        y.ToString(_culture),
                        !string.IsNullOrEmpty(argsData.TextStyle.Color) ? argsData.TextStyle.Color : _chart?._chartThemeStyle?.AxisLabel ?? string.Empty,
                        argsData.TextStyle.GetChartFontOptions(),
                        argsData.Text,
                        anchor,
                        _chart?.ID + axisIndex + "_Axis_MultiLevelLabel_Level_" + level + "_Text_" + i
                    );

                    options = AxisLabelBreak(categoryLabel.Text, options);

                    if (multiLevel.Overflow != TextOverflow.None && !categoryLabel.Text.Contains("<br>", StringComparison.InvariantCulture))
                    {
                        options.TextCollection = multiLevel.Overflow == TextOverflow.Wrap ? ChartHelper.TextWrap(argsData.Text, gap, argsData.TextStyle.GetChartFontOptions()) : [ChartHelper.TextTrim(gap, argsData.Text, argsData.TextStyle.GetChartFontOptions())];
                        options.X = (x - (padding / 2)).ToString(_culture);
                    }

                    string[] locations = ChartHelper.AppendTextElements(_chart ?? null!, options.Id, Convert.ToDouble(options.X, _culture), Convert.ToDouble(options.Y, _culture));
                    options.X = locations[0];
                    options.Y = locations[1];
                    TextOptions.Add(options);

                    if (multiLevel.Border.Width > 0 && multiLevel.Border.Type != BorderType.WithoutBorder)
                    {
                        pathRect = CalculateXAxisLabelBorder(level, endX - startX - padding, startX, startY, labelSize, options, axisRect, argsData.Alignment, pathRect, isOutside, opposedPosition, pointIndex);
                        if (!string.IsNullOrEmpty(pathRect))
                        {
                            CalculateBorderElement(level, axisIndex, pathRect, pointIndex);
                            pointIndex++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Processes one Y axis multi level and its categories.
        /// </summary>
        private void ProcessYAxisLevel(int level, int axisIndex, Rect rect, double startX, bool isOutside, bool isOpposed, double scrollBarHeight)
        {
            if (_axis is null)
            {
                return;
            }
            ChartMultiLevelLabel multiLevel = _axis.MultiLevelLabels[level];
            int pointIndex = 0;
            for (int i = 0; i < multiLevel?.Categories.Count; i++)
            {
                ChartCategory categoryLabel = multiLevel.Categories[i];
                string pathRect = string.Empty;
                object? end = categoryLabel.End?.GetType() == typeof(string) ? Convert.ToDouble(DateTime.Parse((string)categoryLabel.End, _culture), _culture) : categoryLabel.End;
                object? start = categoryLabel.Start?.GetType() == typeof(string) ? Convert.ToDouble(DateTime.Parse((string)categoryLabel.Start, _culture), _culture) : categoryLabel.Start;
                double startY = ChartHelper.ValueToCoefficient(_axisRenderer?.GetDoubleValue(start!) ?? 0, _axisRenderer ?? null!) * rect.Height;
                double endY = _axis.IsAxisInverse ? startY : ChartHelper.ValueToCoefficient(_axisRenderer?.GetDoubleValue(end!) ?? 0, _axisRenderer ?? null!) * rect.Height;

                AxisMultiLabelRenderEventArgs argsData = new(Constants.AxisMultiLevelLabelRender, false, _axis, categoryLabel.CustomAttributes, categoryLabel.Text, multiLevel.TextStyle, multiLevel.Alignment);
                if (_chart?.OnAxisMultiLevelLabelRender is not null)
                {
                    _chart.OnAxisMultiLevelLabelRender.Invoke(argsData);
                }

                if (!argsData.Cancel)
                {
                    Size labelSize = ChartHelper.MeasureText(argsData.Text, argsData.TextStyle.GetChartFontOptions());
                    double gap = endY - startY;
                    double x = rect.X - startX - (_y_AxisPrevHeight?[level] ?? 0) - ((_y_AxisMultiLabelHeight?[level] ?? 0) / 2) - (10 / 2);
                    double y = rect.Height + rect.Y - startY - (gap / 2);
                    double x1 = isOutside ? rect.X + startX + (10 / 2) + ((_y_AxisMultiLabelHeight?[level] ?? 0) / 2) + (_y_AxisPrevHeight?[level] ?? 0) + scrollBarHeight : rect.X - startX - ((_y_AxisMultiLabelHeight?[level] ?? 0) / 2) - (_y_AxisPrevHeight?[level] ?? 0) - (10 / 2);
                    double x2 = isOutside ? x + scrollBarHeight : rect.X + startX + (10 / 2) + ((_y_AxisMultiLabelHeight?[level] ?? 0) / 2) + (_y_AxisPrevHeight?[level] ?? 0);

                    x = isOpposed ? x1 : x2;

                    if (argsData.Alignment == Alignment.Center)
                    {
                        y += labelSize.Height / 4;
                    }
                    else if (argsData.Alignment == Alignment.Far)
                    {
                        y += (gap / 2) - (labelSize.Height / 2);
                    }
                    else
                    {
                        y = y - (gap / 2) + labelSize.Height;
                    }

                    x = multiLevel.Border.Type == BorderType.CurlyBrace ? ((!isOpposed && isOutside) || (isOpposed && !isOutside) ? x - 10 : x + 10) : x;
                    TextOptions options = new(x.ToString(_culture), y.ToString(_culture), !string.IsNullOrEmpty(argsData.TextStyle.Color) ? argsData.TextStyle.Color : _chart?._chartThemeStyle?.AxisLabel ?? string.Empty, argsData.TextStyle.GetChartFontOptions(), argsData.Text, "middle", _chart?.ID + axisIndex + "_Axis_MultiLevelLabel_Level_" + level + "_Text_" + i);
                    options = AxisLabelBreak(categoryLabel.Text, options);
                    options.Text = (multiLevel.Overflow == TextOverflow.Trim) ? ChartHelper.TextTrim(ChartHelper.IsNaNOrZero(categoryLabel.MaximumTextWidth) ? (_y_AxisMultiLabelHeight?[level] ?? 0) : categoryLabel.MaximumTextWidth, argsData.Text, argsData.TextStyle.GetChartFontOptions()) : options.Text;
                    string[] locations = ChartHelper.AppendTextElements(_chart ?? null!, options.Id, x, y);
                    options.X = locations[0];
                    options.Y = locations[1];
                    TextOptions.Add(options);

                    if (multiLevel.Border.Width > 0 && multiLevel.Border.Type != BorderType.WithoutBorder)
                    {
                        pathRect = CalculateYAxisLabelBorder(level, endY, startX, startY, labelSize, options, rect, argsData.Alignment, pathRect, isOutside, isOpposed, pointIndex);
                        if (!string.IsNullOrEmpty(pathRect))
                        {
                            CalculateBorderElement(level, axisIndex, pathRect, pointIndex);
                            pointIndex++;
                        }
                    }
                }
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Initializes multi level label renderer with the provided axis renderer.
        /// </summary>
        /// <param name="renderer">Axis renderer to initialize from.</param>
        internal void IniMultilevelLabel(ChartAxisRenderer renderer)
        {
            _chart = renderer.Owner;
            _axis = renderer.Axis;
            _axisRenderer = renderer;
            GetMultilevelLabelsHeight();
        }

        /// <summary>
        /// Clears stored path and text options.
        /// </summary>
        internal void ClearPathOptions()
        {
            BorderOptions.Clear();
            TextOptions.Clear();
        }

        /// <summary>
        /// Calculates X axis multi level label text and border options for a given axis index and axis rectangle.
        /// </summary>
        /// <param name="index">Axis index.</param>
        /// <param name="axisRect">Axis rectangle.</param>
        internal void CalculateXAxisMultiLevelLabels(int index, Rect axisRect)
        {
            ClearPathOptions();
            double padding = 10;
            double startY = (_axis?.Renderer?.LabelPosition == _axis?.TickPosition ? _axis?.Renderer?.MajorTickLinesHeight ?? 0 : 0) + (_axisRenderer?.MaxLabelSize.Height ?? 0) + padding;
            bool isOutside = _axis?.Renderer?.LabelPosition == AxisPosition.Outside;
            bool opposedPosition = _axis is not null && _axis.IsAxisOpposedPosition;
            double scrollBarHeight = (_axis is not null && _axis.ScrollbarSettings.Enable) || (isOutside && (_axis?.CrossesAt is null)) ? _axis?.ScrollBarHeight ?? 0 : 0;
            double clipY = (opposedPosition && !isOutside) || (!opposedPosition && isOutside) ? ((axisRect.Y + startY - _axis?.Renderer?.MajorTickLinesWidth) ?? 0) : (axisRect.Y - startY - (_axisRenderer?.MultiLevelLabelHeight ?? 0));

            _clipPathID = _chart?.ID + "_XAxis_Clippath_" + index;
            _groupID = _chart?.ID + "XAxisMultiLevelLabel" + index;
            _clipRect = new Rect() { X = (axisRect.X - _axis?.Renderer?.MajorTickLinesWidth) ?? 0, Y = clipY + scrollBarHeight, Height = (_axisRenderer?.MultiLevelLabelHeight ?? 0) + padding, Width = axisRect.Width + ((2 * _axis?.Renderer?.MajorTickLinesWidth) ?? 0) };
            int labelCount = _axis?.MultiLevelLabels.Count ?? 0;

            for (int level = 0; level < labelCount; level++)
            {
                ProcessXAxisLevel(level, index, axisRect, startY, padding, isOutside, opposedPosition, scrollBarHeight);
            }
        }

        /// <summary>
        /// Calculates Y axis multi level label text and border options for a given axis index and rectangle.
        /// </summary>
        /// <param name="index">Axis index.</param>
        /// <param name="rect">Axis rectangle.</param>
        internal void CalculateYAxisMultiLevelLabels(int index, Rect rect)
        {
            ClearPathOptions();
            bool isOutside = _axis?.Renderer?.LabelPosition == AxisPosition.Outside;
            double startX = (_axis?.TickPosition == _axis?.Renderer?.LabelPosition ? _axis?.Renderer?.MajorTickLinesHeight ?? 0 : 0) + (_axisRenderer?.MaxLabelSize.Width ?? 0) + 10;
            bool isOpposed = _axis is not null && _axis.IsAxisOpposedPosition;
            double scrollBarHeight = (isOutside && _axis?.CrossesAt is null ? (_axis?.ScrollBarHeight ?? 0) : 0) * (isOpposed ? 1 : -1);
            double clipX = (isOpposed && !isOutside) || (!isOpposed && isOutside) ? (rect.X - (_axisRenderer?.MultiLevelLabelHeight ?? 0) - startX - 10) : (rect.X + startX);

            _clipPathID = _chart?.ID + "_YAxis_Clippath_" + index;
            _groupID = _chart?.ID + "YAxisMultiLevelLabel" + index;
            _clipRect = new Rect { X = clipX + scrollBarHeight, Y = rect.Y - (_axis?.Renderer?.MajorTickLinesWidth ?? 0), Height = rect.Height + (2 * (_axis?.Renderer?.MajorTickLinesWidth ?? 0)), Width = (_axisRenderer?.MultiLevelLabelHeight ?? 0) + 10 };

            for (int level = 0; level < _axis?.MultiLevelLabels.Count; level++)
            {
                ProcessYAxisLevel(level, index, rect, startX, isOutside, isOpposed, scrollBarHeight);
            }
        }

        /// <summary>
        /// Renders previously computed multi level label text and borders to the provided RenderTreeBuilder.
        /// </summary>
        /// <param name="builder">Render tree builder to write SVG elements into.</param>
        internal void RenderMultilevelLabel(RenderTreeBuilder builder)
        {
            if (_clipRect is null || builder is null)
            {
                return;
            }
            _chart?._svgRenderer?.OpenGroupElement(builder, _groupID ?? string.Empty, string.Empty, "url(#" + _clipPathID + ")");
            _chart?._svgRenderer?.RenderClipPath(builder, _clipPathID ?? string.Empty, _clipRect);

            for (int i = 0; i < TextOptions.Count; i++)
            {
                _chart?._svgRenderer?.OpenGroupElement(builder, _chart.ID + _axisRenderer?.Index + "_MultiLevelLabel" + i);
                ChartHelper.TextElement(builder, _chart?._svgRenderer ?? null!, TextOptions[i]);
                if (BorderOptions.Count > 0 && i < BorderOptions.Count)
                {
                    _chart?._svgRenderer?.RenderPath(builder, BorderOptions[i], "e-pointer-series");
                }
                builder.CloseElement();
            }

            builder.CloseElement();
        }

        /// <summary>
        /// Cleans up resources when the component is disposed.
        /// </summary>
        internal void Dispose()
        {
            _y_AxisMultiLabelHeight = null;
            _x_AxisMultiLabelHeight = null;
            _x_AxisPrevHeight = null;
            _y_AxisPrevHeight = null;
            _chart = null;
            _axis = null;
            _axisRenderer = null;
            BorderOptions.Clear();
            TextOptions.Clear();
            _clipRect = null;
        }
        #endregion
    }
}