
namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    internal partial class CartesianAxisLayout
    {
        #region Constants
        private const string START = "start";
        private const string END = "end";
        private const string MIDDLE = "middle";
        private const string UNDEFINED = "undefined";
        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates X-axis title position and options.
        /// </summary>
        /// <param name="axis">Axis.</param>
        /// <param name="index">Axis index.</param>
        /// <param name="rect">Axis rectangle.</param>
        private void CalculateXAxisTitle(ChartAxis axis, int index, Rect rect)
        {
            if (Chart is not null && Chart.EnableAdaptiveRendering && (Chart._heightCategory == ChartHeightCategory.Small || Chart._heightCategory == ChartHeightCategory.Medium || Chart._widthCategory == ChartWidthCategory.Small))
            {
                return;
            }

            ChartFontOptions titleStyle = GetTitleStyle(axis);
            Size elementSize = ChartHelper.MeasureText(axis.Title, titleStyle);
            double titleSize = MeasureMultiTitleHeight(axis, titleStyle);
            double padding = ComputeXAxisPadding(axis, elementSize, titleSize);
            Alignment textAlignment = axis.TitleStyle.TextAlignment;

            string xValue = ComputeXAxisValue(textAlignment, rect);
            string anchor = ComputeHorizontalAnchor(textAlignment);

            TextOptions titleOption = new(
                xValue,
                (rect.Y + padding).ToString(Culture),
                !string.IsNullOrEmpty(axis.TitleStyle.Color) ? axis.TitleStyle.Color : Chart?._chartThemeStyle?.AxisTitle ?? string.Empty,
                GetFontOptions(titleStyle),
                axis.Title,
                anchor,
                axis.Renderer?.Owner?.ID + "_AxisTitle_" + index,
                string.Empty,
                "0",
                UNDEFINED,
                !string.IsNullOrEmpty(axis.Description) ? axis.Description : axis.Title
            )
            {
                TextCollection = axis.Renderer?.TitleCollection ?? null!,
                Font = titleStyle
            };

            if (axis.Renderer is { })
            {
                axis.Renderer.AxisRenderInfo.AxisTitleOption = titleOption;
            }
        }

        /// <summary>
        /// Calculates Y-axis title position and options.
        /// </summary>
        /// <param name="axis">Axis.</param>
        /// <param name="index">Axis index.</param>
        /// <param name="rect">Axis rectangle.</param>
        private void CalculateYAxisTitle(ChartAxis axis, int index, Rect rect)
        {
            if (Chart is not null && Chart.EnableAdaptiveRendering && (Chart._widthCategory == ChartWidthCategory.Small || Chart._widthCategory == ChartWidthCategory.Medium || Chart._heightCategory == ChartHeightCategory.Small))
            {
                return;
            }

            ChartFontOptions titleStyle = GetTitleStyle(axis);
            bool isOpposed = axis.IsAxisOpposedPosition;
            double labelRotation = isOpposed ? 90 : -90;
            double scrollBarHeight = axis.ScrollBarHeight;
            double padding = ComputeYAxisPadding(axis);
            padding = isOpposed ? padding + scrollBarHeight : -padding - scrollBarHeight;
            double titleSize = MeasureMultiTitleHeight(axis, titleStyle);

            Alignment textAlignment = axis.TitleStyle.TextAlignment;
            double x = rect.X + padding;
            double y = ComputeYAxisY(textAlignment, rect);

            string anchor = ComputeVerticalAnchor(textAlignment, isOpposed);

            ChartAxisRenderer axisRenderer = axis.Renderer ?? null!;
            axisRenderer.AxisRenderInfo.AxisTitleOption = new TextOptions(
                x.ToString(Culture),
                (y - axis.LabelPadding - titleSize).ToString(Culture),
                !string.IsNullOrEmpty(axis.TitleStyle.Color) ? axis.TitleStyle.Color : Chart?._chartThemeStyle?.AxisTitle ?? string.Empty,
                GetFontOptions(titleStyle),
                axis.Title,
                anchor,
                axisRenderer.Owner?.ID + "_AxisTitle_" + index,
                "rotate(" + labelRotation.ToString(Culture) + "," + x.ToString(Culture) + "," + y.ToString(Culture) + ")",
                labelRotation.ToString(Culture),
                UNDEFINED,
                !string.IsNullOrEmpty(axis.Description) ? axis.Description : axis.Title)
            {
                TextCollection = axisRenderer.TitleCollection,
                Font = titleStyle
            };
        }

        /// <summary>
        /// Retrieves the title style from the axis configuration.
        /// </summary>
        /// <param name="axis">The axis from which to retrieve the title style.</param>
        /// <returns>A <see cref="ChartFontOptions"/> object containing the title style.</returns>
        private static ChartFontOptions GetTitleStyle(ChartAxis axis)
        {
            return axis.TitleStyle.GetChartFontOptions(axis.Renderer?.Owner?._chartThemeStyle ?? null!);
        }

        /// <summary>
        /// Measures the total height of multi-level axis titles.
        /// </summary>
        /// <remarks>
        /// Used to calculate padding when multiple title levels are present.
        /// </remarks>
        /// <param name="axis">The axis containing the title collection.</param>
        /// <param name="style">The font style to measure text with.</param>
        /// <returns>The total height of multi-level titles, or 0 if only one level exists.</returns>
        private static double MeasureMultiTitleHeight(ChartAxis axis, ChartFontOptions style)
        {
            int count = (axis.Renderer?.TitleCollection.Count ?? 0) - 1;
            return count <= 0 ? 0 : ChartHelper.MeasureText(axis.Title, style).Height * count;
        }

        /// <summary>
        /// Computes the vertical padding for X-axis titles.
        /// </summary>
        /// <remarks>
        /// Padding is calculated based on tick position, label position, element height,
        /// and scroll bar height.
        /// </remarks>
        /// <param name="axis">The axis for which to compute padding.</param>
        /// <param name="elementSize">The measured size of the title element.</param>
        /// <param name="titleSize">The height of multi-level titles.</param>
        /// <returns>The computed padding value.</returns>
        private static double ComputeXAxisPadding(ChartAxis axis, Size elementSize, double titleSize)
        {
            double tickPadding = axis.TickPosition == AxisPosition.Inside ? 5 : (axis.Renderer?.MajorTickLinesHeight ?? 0) + 5;
            double labelPadding = axis.Renderer?.LabelPosition == AxisPosition.Inside ? 5 : (axis.Renderer?.MaxLabelSize.Height ?? 0) + (axis.Renderer?.MultiLevelLabelHeight ?? 0) + 5;
            double padding = tickPadding + labelPadding;
            double scrollBarHeight = axis.CrossesAt is null ? axis.ScrollBarHeight : 0;
            return axis.IsAxisOpposedPosition ? -(padding + (elementSize.Height * 0.25) + scrollBarHeight + titleSize) : (padding + ((elementSize.Height * 0.75) + scrollBarHeight));
        }

        /// <summary>
        /// Computes the horizontal padding for Y-axis titles.
        /// </summary>
        /// <remarks>
        /// Padding varies based on device mode to optimize rendering for touch and desktop scenarios.
        /// </remarks>
        /// <param name="axis">The axis for which to compute padding.</param>
        /// <returns>The computed padding value.</returns>
        private double ComputeYAxisPadding(ChartAxis axis)
        {
            double tickPadding = axis.TickPosition == AxisPosition.Inside ? 0 : (axis.Renderer?.MajorTickLinesHeight ?? 0) + 5;
            double labelPadding = axis.Renderer?.LabelPosition == AxisPosition.Inside ? 0 : (axis.Renderer?.MaxLabelSize.Width ?? 0) + (axis.Renderer?.MultiLevelLabelHeight ?? 0) + 5;
            double devicePadding = (Chart is not null && Chart.SyncfusionService is not null && Chart.SyncfusionService.IsDeviceMode) ? 5 : 10;
            return tickPadding + labelPadding + devicePadding;
        }

        /// <summary>
        /// Determines the horizontal text anchor position based on alignment.
        /// </summary>
        /// <remarks>
        /// RTL support is considered when computing the anchor position.
        /// </remarks>
        /// <param name="alignment">The desired text alignment.</param>
        /// <returns>
        /// A string representing SVG text anchor: "start", "middle", or "end".
        /// </returns>
        private string ComputeHorizontalAnchor(Alignment alignment)
        {
            return alignment == Alignment.Center
                ? MIDDLE
                : alignment == Alignment.Near
                ? (Chart is not null && !Chart.EnableRtl) ? START : END
                : (Chart is not null && !Chart.EnableRtl) ? END : START;
        }

        /// <summary>
        /// Determines the vertical text anchor position based on alignment and axis opposition.
        /// </summary>
        /// <remarks>
        /// Accounts for RTL mode and opposed axis positioning.
        /// </remarks>
        /// <param name="alignment">The desired text alignment.</param>
        /// <param name="isOpposed">Whether the axis is in opposed (secondary) position.</param>
        /// <returns>
        /// A string representing SVG text anchor: "start", "middle", or "end".
        /// </returns>
        private string ComputeVerticalAnchor(Alignment alignment, bool isOpposed)
        {
            return alignment == Alignment.Center
                ? MIDDLE
                : alignment == Alignment.Near
                ? isOpposed && Chart is not null && !Chart.EnableRtl ? END : !isOpposed && Chart is not null && Chart.EnableRtl ? END : START
                : isOpposed && Chart is not null && !Chart.EnableRtl ? START : !isOpposed && Chart is not null && Chart.EnableRtl ? START : END;
        }

        /// <summary>
        /// Computes the X coordinate for the axis title based on alignment and bounds.
        /// </summary>
        /// <param name="alignment">The desired text alignment.</param>
        /// <param name="rect">The rectangular bounds of the axis.</param>
        /// <returns>The X coordinate as a string formatted for SVG rendering.</returns>
        private string ComputeXAxisValue(Alignment alignment, Rect rect)
        {
            double xCoord = alignment == Alignment.Center ? rect.X + (rect.Width * 0.5) : alignment == Alignment.Near ? rect.X : rect.X + rect.Width;
            return xCoord.ToString(Culture);
        }

        /// <summary>
        /// Computes the Y coordinate for the axis title based on alignment and bounds.
        /// </summary>
        /// <param name="alignment">The desired text alignment.</param>
        /// <param name="rect">The rectangular bounds of the axis.</param>
        /// <returns>The Y coordinate as a double value.</returns>
        private static double ComputeYAxisY(Alignment alignment, Rect rect)
        {
            return alignment == Alignment.Center ? rect.Y + (rect.Height * 0.5) : alignment == Alignment.Near ? rect.Y + rect.Height : rect.Y;
        }
        #endregion
    }
}
