using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents a double precision numeric range with helper operations.
    /// </summary>
    public struct DoubleRange
    {

        #region Properties

        /// <summary>
        /// Gets the start value of the range.
        /// </summary>
        public double Start { get; }

        /// <summary>
        /// Gets the end value of the range.
        /// </summary>
        public double End { get; }

        /// <summary>
        /// Gets the delta (End - Start).
        /// </summary>
        public readonly double Delta => End - Start;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleRange"/> struct.
        /// </summary>
        /// <param name="start">Range start.</param>
        /// <param name="end">Range end.</param>
        public DoubleRange(double start, double end)
        {
            if (start > end)
            {
                Start = end;
                End = start;
            }
            else
            {
                Start = start;
                End = end;
            }
        }
        #endregion
    }

    /// <summary>
    /// Renderer responsible for axis layout and drawing.
    /// </summary>
    public class ChartAxisRenderer : ChartRenderer, IChartElementRenderer
    {
        #region Constants
        private const double DEFAULT_LABEL_HEIGHT = 15.96;
        #endregion

        #region Fields
        private Rect? _availableRect;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the axis this renderer represents.
        /// </summary>
        [Parameter]
        public string? AxisName { get; set; }

        /// <summary>
        /// Gets the culture information used for formatting and localization.
        /// </summary>
        protected CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        /// <summary>
        /// Gets or sets the axis instance this renderer is associated with.
        /// </summary>
        internal ChartAxis? Axis { get; set; }

        /// <summary>
        /// Gets or sets the cross-axis that intersects with this axis.
        /// </summary>
        internal ChartAxis? CrossInAxis { get; set; }

        /// <summary>
        /// Gets or sets the index of this axis in the axis collection.
        /// </summary>
        internal int Index { get; set; }

        /// <summary>
        /// Gets or sets the rendering rectangle for this axis.
        /// </summary>
        internal Rect Rect { get; set; } = new Rect();

        /// <summary>
        /// Gets or sets the updated rectangle after cross-axis adjustments.
        /// </summary>
        internal Rect UpdatedRect { get; set; } = new Rect();

        /// <summary>
        /// Gets or sets the minimum value in the axis range.
        /// </summary>
        internal double Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum value in the axis range.
        /// </summary>
        internal double Max { get; set; }

        /// <summary>
        /// Gets or sets the minimum value from all associated series.
        /// </summary>
        internal double SeriesMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum value from all associated series.
        /// </summary>
        internal double SeriesMax { get; set; }

        /// <summary>
        /// Gets or sets the parent chart instance.
        /// </summary>
        internal SfChart? Chart { get; set; }

        /// <summary>
        /// Gets or sets the calculated interval between axis values.
        /// </summary>
        internal double ActualInterval { get; set; }

        /// <summary>
        /// Gets or sets the actual range of the axis including padding.
        /// </summary>
        internal DoubleRange ActualRange { get; set; }

        /// <summary>
        /// Gets or sets the interval between visible labels.
        /// </summary>
        internal double VisibleInterval { get; set; }

        /// <summary>
        /// Gets or sets the currently visible range after zooming/panning.
        /// </summary>
        internal DoubleRange VisibleRange { get; set; }

        /// <summary>
        /// Gets or sets the orientation of this axis.
        /// </summary>
        internal Orientation Orientation { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of data points across all series.
        /// </summary>
        internal int MaxPointLength { get; set; }

        /// <summary>
        /// Gets or sets the interval divisors used for nice number calculation.
        /// </summary>
        internal double[] IntervalDivs { get; set; } = [10, 5, 2, 1];

        /// <summary>
        /// Gets or sets a value indicating whether this axis is for a 100% stacked series.
        /// </summary>
        internal bool IsStack100 { get; set; }

        /// <summary>
        /// Gets the collection of series renderers associated with this axis.
        /// </summary>
        internal List<ChartSeriesRenderer> SeriesRenderer { get; set; } = [];

        /// <summary>
        /// Gets the collection of visible axis labels.
        /// </summary>
        internal List<VisibleLabels> VisibleLabels { get; set; } = [];

        /// <summary>
        /// Gets the collection of title text lines when title wrapping occurs.
        /// </summary>
        internal List<string> TitleCollection { get; set; } = [];

        /// <summary>
        /// Gets or sets the rotation angle for axis labels.
        /// </summary>
        internal double Angle { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of any label in the axis.
        /// </summary>
        internal Size MaxLabelSize { get; set; } = new Size(0, 0);

        /// <summary>
        /// Gets or sets the longest label text used for rotation calculations.
        /// </summary>
        internal string? RotatedLabel { get; set; }

        /// <summary>
        /// Gets the collection of category labels for category axes.
        /// </summary>
        internal List<string> Labels { get; set; } = [];

        /// <summary>
        /// Gets or sets the interval value for DateTime axes.
        /// </summary>
        internal double DateTimeInterval { get; set; }

        /// <summary>
        /// Gets or sets the date format string for DateTime axes.
        /// </summary>
        internal string? DateFormat { get; set; }

        /// <summary>
        /// Gets the rendering information container for this axis.
        /// </summary>
        internal AxisRenderInfo AxisRenderInfo { get; set; } = new AxisRenderInfo();

        /// <summary>
        /// Gets or sets the value type of this axis.
        /// </summary>
        internal ValueType Type { get; set; }

        /// <summary>
        /// Gets or sets the actual interval type for DateTime axes.
        /// </summary>
        internal IntervalType ActualIntervalType { get; set; }

        /// <summary>
        /// Gets or sets the padding interval for bar/column charts.
        /// </summary>
        protected double PaddingInterval { get; set; }

        /// <summary>
        /// Gets or sets the count of column-type series.
        /// </summary>
        protected double IsColumn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether intervals should be in decimal format.
        /// </summary>
        protected bool IsIntervalInDecimal { get; set; } = true;

        /// <summary>
        /// Gets or sets the available size for the axis area.
        /// </summary>
        internal Size? AxisAvailabelSize { get; set; }

        /// <summary>
        /// Gets or sets the total height of multi-level labels.
        /// </summary>
        internal double MultiLevelLabelHeight { get; set; }

        /// <summary>
        /// Gets or sets the renderer for multi-level labels.
        /// </summary>
        internal MultiLevelLabelRenderer? MultiLevelLabelRenderer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the axis line is positioned inside the plot area.
        /// </summary>
        internal bool IsAxisInside { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether tick marks are positioned inside the plot area.
        /// </summary>
        internal bool IsTickInside { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether axis labels are positioned inside the plot area.
        /// </summary>
        internal bool IsAxisLabelInside { get; set; }

        /// <summary>
        /// Gets or sets the renderer for outside axis elements.
        /// </summary>
        internal ChartAxisOutsideRenderer? OutSideRenderer { get; set; }

        /// <summary>
        /// Gets or sets the value where this axis crosses another axis.
        /// </summary>
        internal double CrossAt { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets a value indicating whether this axis uses DateOnly types.
        /// </summary>
        internal bool IsDateOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this axis uses TimeOnly types.
        /// </summary>
        internal bool IsTimeOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this axis is for column-type series.
        /// </summary>
        protected bool IsColumnType { get; set; }

        /// <summary>
        /// Gets or sets the height of major tick lines.
        /// </summary>
        internal double MajorTickLinesHeight { get; set; }

        /// <summary>
        /// Gets or sets the width of major tick lines.
        /// </summary>
        internal double MajorTickLinesWidth { get; set; }

        /// <summary>
        /// Gets or sets the width of major grid lines.
        /// </summary>
        internal double MajorGridLinesWidth { get; set; }

        /// <summary>
        /// Gets or sets the action to take when axis labels intersect.
        /// </summary>
        internal LabelIntersectAction LabelIntersectAction { get; set; }

        /// <summary>
        /// Gets or sets the position of axis labels.
        /// </summary>
        internal AxisPosition LabelPosition { get; set; }

        /// <summary>
        /// Gets or sets the placement strategy for edge labels.
        /// </summary>
        internal EdgeLabelPlacement EdgeLabelPlacement { get; set; }

        /// <summary>
        /// Gets or sets the rotation angle for axis labels.
        /// </summary>
        internal double LabelRotation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scrollbar should be rendered.
        /// </summary>
        internal bool ShouldRenderScrollbar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether strip line tooltips are enabled.
        /// </summary>
        internal bool IsStripLineTooltip { get; set; }

        /// <summary>
        /// Gets or sets the cascading renderer container supplied by the parent chart component.
        /// </summary>
        /// <value>The <see cref="ChartRendererContainer"/> ancestor, or <see langword="null"/> if absent.</value>
        [CascadingParameter]
        internal ChartRendererContainer? Container { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs initialization and registers this renderer with the axis container.
        /// </summary>
        protected override void OnInitialized()
        {
            Owner?._axisContainer?.AddRenderer(this);
            SvgRenderer = Owner?._svgRenderer ?? null;
            if (Axis is not null)
            {
                Axis.Renderer = this;
                Chart = Axis.Container;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Calculate title height component for label area.
        /// </summary>
        /// <param name="innerPadding">Inner padding to add.</param>
        /// <param name="axisWidth">Axis width used for title wrapping.</param>
        /// <returns>Title size height.</returns>
        private double CalculateTitleSize(double innerPadding, double axisWidth)
        {
            if (Axis == null || string.IsNullOrEmpty(Axis.Title))
            {
                return 0;
            }

            bool isVertical = Axis.Renderer?.Orientation == Orientation.Vertical;

            if (Owner is not null && Owner.EnableAdaptiveRendering && ((isVertical && (Owner._widthCategory == ChartWidthCategory.Small || Owner._widthCategory == ChartWidthCategory.Medium || Owner._heightCategory == ChartHeightCategory.Small)) || (!isVertical && (Owner._heightCategory == ChartHeightCategory.Small || Owner._heightCategory == ChartHeightCategory.Medium || Owner._widthCategory == ChartWidthCategory.Small))))
            {
                return 0;
            }

            ChartFontOptions axisTitleStyle = Axis.TitleStyle.GetChartFontOptions(Owner?._chartThemeStyle ?? null!);
            double titleSize = ChartHelper.MeasureText(Axis.Title, axisTitleStyle).Height + innerPadding + (Axis.Renderer?.Orientation == Orientation.Vertical ? 10 : 0);

            if (!double.IsNaN(axisWidth) && Axis.Renderer is not null)
            {
                Axis.Renderer.TitleCollection = ChartHelper.GetTitle(Axis.Title, axisTitleStyle, axisWidth);
                titleSize *= Axis.Renderer.TitleCollection.Count;
            }

            return titleSize;
        }

        /// <summary>
        /// Determines whether the axis area is too small to render labels or ticks based on the given orientation.
        /// </summary>
        /// <param name="orientation">The orientation of the axis being evaluated.</param>
        /// <returns><c>true</c> if the available chart size is 100 pixels or fewer in the relevant dimension; otherwise, <c>false</c>.</returns>
        private bool IsExtremelySmall(Orientation orientation)
        {
            if (Owner is null)
            {
                return false;
            }

            bool isVertical = orientation == Orientation.Vertical;
            return isVertical
                ? (Owner.AvailableSize.Width <= 100)
                : (Owner.AvailableSize.Height <= 100 || Owner.AvailableSize.Width <= 100);
        }

        /// <summary>
        /// Applies cross axis adjustments to label size.
        /// </summary>
        /// <param name="labelSize">Original label size.</param>
        /// <returns>Adjusted label size.</returns>
        private double ApplyCrossAxisLabelAdjustment(double labelSize)
        {
            if (CrossInAxis is null || CrossInAxis.Renderer is null)
            {
                return 0;
            }
            DoubleRange range = CrossInAxis.Renderer.VisibleRange;
            double size = (CrossInAxis.Renderer.Orientation == Orientation.Horizontal) ? CrossInAxis.Renderer.Rect.Width : CrossInAxis.Renderer.Rect.Height;

            if (double.IsNaN(size) && size == 0)
            {
                return 0;
            }
            else if (IsInside(range))
            {
                double diffValue = FindDifference(CrossInAxis);
                double diff = diffValue * (size / range.Delta);
                diff = diffValue * ((size - (diff < labelSize ? (labelSize - diff) : 0)) / range.Delta);
                return (diff < labelSize) ? (labelSize - diff) : 0;
            }
            return labelSize;
        }

        /// <summary>
        /// Calculates the absolute difference between the current axis's <c>CrossAt</c> value and the corresponding visible range boundary of the specified <paramref name="crossAxis"/>.
        /// </summary>
        /// <param name="crossAxis">
        /// The axis whose visible range is used to compute the difference against <c>CrossAt</c>.
        /// </param>
        /// <returns>
        /// The absolute difference between <c>CrossAt</c> and the resolved boundary value of <paramref name="crossAxis"/>.
        /// </returns>
        private double FindDifference(ChartAxis crossAxis)
        {
            bool isCrossAxisOpposed = Axis is not null && Axis.IsAxisOpposedPosition;
            double range = isCrossAxisOpposed ? (crossAxis.IsAxisInverse ? crossAxis.Renderer?.VisibleRange.Start ?? 0 : crossAxis.Renderer?.VisibleRange.End ?? 0) : crossAxis.IsAxisInverse ? crossAxis.Renderer?.VisibleRange.End ?? 0 : crossAxis.Renderer?.VisibleRange.Start ?? 0;
            return Math.Abs(CrossAt - range);
        }

        /// <summary>
        /// Sets Y axis min/max for visible X range.
        /// </summary>
        /// <param name="axisCollection">Collection of axes.</param>
        /// <param name="visibleRange">Visible X range.</param>
        private void SetYAxisMinMax(List<ChartAxis> axisCollection, DoubleRange visibleRange)
        {
            foreach (ChartSeriesRenderer series in SeriesRenderer.ToArray())
            {
                List<double> pointYValue = [];
                if (series.Points is not null)
                {
                    foreach (Point points in series.Points.ToArray())
                    {
                        if ((points.XValue > visibleRange.Start) && (points.XValue < visibleRange.End))
                        {
                            pointYValue.Add(points.YValue);
                        }
                    }
                }

                foreach (ChartAxis _ in axisCollection.ToArray())
                {
                    if (Orientation == Orientation.Vertical && series is not null && pointYValue.Count > 0)
                    {
                        series.YMin = pointYValue.Min();
                        series.YMax = pointYValue.Max();
                    }
                }
            }
        }

        /// <summary>
        /// Sets X axis min/max for visible Y range.
        /// </summary>
        /// <param name="axisCollection">Collection of axes.</param>
        /// <param name="visibleRange">Visible Y range.</param>
        private void SetXAxisMinMax(List<ChartAxis> axisCollection, DoubleRange visibleRange)
        {
            foreach (ChartSeriesRenderer series in SeriesRenderer.ToArray())
            {
                List<double> pointXValue = [];
                if (series.Points is not null)
                {
                    foreach (Point points in series.Points.ToArray())
                    {
                        if ((points.YValue > visibleRange.Start) && (points.YValue < visibleRange.End))
                        {
                            pointXValue.Add(points.XValue);
                        }
                    }
                }

                foreach (ChartAxis _ in axisCollection.ToArray())
                {
                    if (Orientation == Orientation.Horizontal && series is not null && pointXValue.Count > 0)
                    {
                        series.XMin = pointXValue.Min();
                        series.XMax = pointXValue.Max();
                    }
                }
            }
        }

        /// <summary>
        /// Returns an adaptive formatted value depending on axis type.
        /// </summary>
        /// <param name="value">Value to adapt.</param>
        /// <param name="formatValue">Previously formatted value.</param>
        /// <returns>Adaptive format string.</returns>
        private string GetAdaptiveFormatValue(double value, string formatValue)
        {
            string result = string.Empty;

            if (Axis?.ValueType == ValueType.Double)
            {
                result = GetNumericFormatValue(value);
            }
            else if (Axis?.ValueType == ValueType.Logarithmic)
            {
                result = GetLogarithmicFormatValue(value);
            }
            return string.IsNullOrEmpty(result) ? formatValue : result;
        }

        /// <summary>
        /// Returns human-friendly numeric abbreviations for large values.
        /// </summary>
        /// <param name="value">Numeric value.</param>
        /// <returns>Abbreviated string or empty if not applicable.</returns>
        private string GetNumericFormatValue(double value)
        {
            return value is >= 1000000 or <= -1000000
                ? (value / 1000000).ToString("0.##M", Culture)
                : value is >= 1000 or <= -1000 ? (value / 1000).ToString("0.##K", Culture) : string.Empty;
        }

        /// <summary>
        /// Returns logarithmic format representation for a positive value.
        /// </summary>
        /// <param name="value">Value to format.</param>
        /// <returns>Formatted logarithmic label or empty.</returns>
        private string GetLogarithmicFormatValue(double value)
        {
            if (value <= 0 || Axis?.LogBase <= 1)
            {
                return string.Empty;
            }

            int exponent = (int)Math.Log(value, Axis?.LogBase ?? 10);
            string superscriptExponent = ConvertToSuperscript(exponent);
            return $"{Axis?.LogBase}{superscriptExponent}";
        }

        /// <summary>
        /// Converts integer exponent to Unicode superscript digits.
        /// </summary>
        /// <param name="exponent">Exponent integer.</param>
        /// <returns>Superscript string.</returns>
        private string ConvertToSuperscript(int exponent)
        {
            string[] superscriptDigits = ["⁰", "¹", "²", "³", "⁴", "⁵", "⁶", "⁷", "⁸", "⁹"];
            char[] exponentChars = exponent.ToString(Culture).ToCharArray();
            string result = "";

            foreach (char c in exponentChars)
            {
                int digit = c - '0';
                result += superscriptDigits[digit];
            }

            return result;
        }

        /// <summary>
        /// Measures the size of template-based axis labels and updates <see cref="MaxLabelSize"/> accordingly.
        /// </summary>
        /// <param name="rendererAxisInfo">The axis render info containing template size data.</param>
        private void GetMaxTemplateWidth(AxisRenderInfo rendererAxisInfo)
        {
            Angle = Axis?.Renderer?.LabelRotation ?? 0;
            double previousEnd = 0;
            Size maxTemplateSize = new(0, 0);

            if (rendererAxisInfo.AxisLabelTemplateSizeList is not null && rendererAxisInfo.AxisLabelTemplateSizeList.Count > 0)
            {
                maxTemplateSize = new Size(rendererAxisInfo.AxisLabelTemplateSizeList.Max(x => x.Value.Width), rendererAxisInfo.AxisLabelTemplateSizeList.Max(x => x.Value.Height));
            }
            double padding = 5;
            LabelIntersectAction? action = Axis?.Renderer?.LabelIntersectAction ?? null;

            for (int i = 0; i < VisibleLabels.Count; i++)
            {
                VisibleLabels label = VisibleLabels[i];
                Size labelTemplateSize = (i < rendererAxisInfo.AxisLabelTemplateSizeList?.Count) ? (rendererAxisInfo.AxisLabelTemplateSizeList[i].Value.Width != 0 && rendererAxisInfo.AxisLabelTemplateSizeList[i].Value.Height != 0 ? rendererAxisInfo.AxisLabelTemplateSizeList[i].Value : maxTemplateSize) : maxTemplateSize;
                label.TemplateSize = new Size(labelTemplateSize.Width, labelTemplateSize.Height);
                label.Size = new Size(labelTemplateSize.Width, labelTemplateSize.Height);

                if (labelTemplateSize.Width > MaxLabelSize.Width)
                {
                    MaxLabelSize.Width = labelTemplateSize.Width;
                }
                if (labelTemplateSize.Height > MaxLabelSize.Height)
                {
                    MaxLabelSize.Height = labelTemplateSize.Height;
                }

                if (action is LabelIntersectAction.None or LabelIntersectAction.Hide)
                {
                    continue;
                }

                if ((action != LabelIntersectAction.None || Angle % 360 == 0) && Orientation == Orientation.Horizontal && Rect.Width > 0)
                {
                    double width1 = label.Size.Width;
                    double pointX = (ChartHelper.ValueToCoefficient(label.Value, this) * Rect.Width) + Rect.X;
                    pointX = pointX - (width1 / 2) + padding;
                    width1 += padding;

                    if (Axis?.Renderer?.EdgeLabelPlacement == EdgeLabelPlacement.Shift)
                    {
                        if (i == 0 && pointX < Rect.X)
                        {
                            pointX = Rect.X;
                        }

                        if (i == VisibleLabels.Count - 1 && ((pointX + width1) > (Rect.X + Rect.Width)))
                        {
                            pointX = Rect.X + Rect.Width - width1;
                        }
                    }
                    if (action is not null and (LabelIntersectAction.Rotate45 or LabelIntersectAction.Rotate90))
                    {
                        if (i > 0 && (Axis is not null && !Axis.IsAxisInverse ? pointX <= previousEnd : pointX + width1 >= previousEnd))
                        {
                            Angle = (action == LabelIntersectAction.Rotate45) ? 45 : 90;
                            break;
                        }
                    }

                    previousEnd = Axis is not null && Axis.IsAxisInverse ? pointX : pointX + width1;
                }
            }

            MaxLabelSize.Height = VisibleLabels.Count == 0 && Axis is not null && Axis.CrosshairTooltip.Enable ? DEFAULT_LABEL_HEIGHT : MaxLabelSize.Height;

            if (Axis?.LabelTemplate is not null)
            {
                if (Rect.Height != 0)
                {
                    MaxLabelSize.Height = Math.Min(MaxLabelSize.Height, Rect.Height / 2);
                }
                if (Rect.Width != 0)
                {
                    MaxLabelSize.Width = Math.Min(MaxLabelSize.Width, Rect.Width / 2);
                }
            }

            if (Angle != 0 && Orientation == Orientation.Horizontal && Axis?.LabelTemplate is not null)
            {
                MaxLabelSize = RotateTemplateSize(MaxLabelSize, Angle);
            }

            if (Axis?.MultiLevelLabels.Count > 0)
            {
                MultiLevelLabelRenderer = new MultiLevelLabelRenderer();
                MultiLevelLabelRenderer.IniMultilevelLabel(this);
            }
        }

        /// <summary>
        /// Measures the size of a single label and updates <see cref="MaxLabelSize"/> accordingly.
        /// </summary>
        /// <param name="label">The label whose size is computed.</param>
        /// <param name="isAxisLabelBreak"><see langword="true"/> when the label contains a line break tag.</param>
        /// <param name="axisLabelStyle">The resolved font options.</param>
        /// <param name="isCustomLabelFormat"><see langword="true"/> when the axis uses a custom label format.</param>
        /// <param name="formatTextSize">Pre-measured size of the static format text portion.</param>
        /// <param name="formatText">The static portion of the label format string.</param>
        private void MeasureLabelSizes(VisibleLabels label, bool isAxisLabelBreak, ChartFontOptions axisLabelStyle, bool isCustomLabelFormat, Size formatTextSize, string formatText)
        {
            if (isAxisLabelBreak)
            {
                label.Size = ChartHelper.MeasureText(label.OriginalText.Replace("<br>", " ", StringComparison.InvariantCulture), axisLabelStyle);
                label.BreakLabelSize = ChartHelper.MeasureText(Axis is not null && Axis.EnableTrim ? string.Join("<br>", label.TextArr) : label.OriginalText, axisLabelStyle);
            }
            else
            {
                label.Size = isCustomLabelFormat ? GetCustomFormatLabelSize(label.Text, axisLabelStyle, formatTextSize, formatText) : ChartHelper.MeasureText(label.Text, axisLabelStyle);
            }
        }

        /// <summary>
        /// Updates <see cref="MaxLabelSize"/> and <see cref="RotatedLabel"/> when the current label exceeds previous maximums.
        /// </summary>
        /// <param name="label">The label being evaluated.</param>
        /// <param name="isAxisLabelBreak"><see langword="true"/> when the label uses break formatting.</param>
        private void UpdateMaxLabelWidth(VisibleLabels label, bool isAxisLabelBreak)
        {
            double width = isAxisLabelBreak ? label.BreakLabelSize.Width : label.Size.Width;
            if (width > MaxLabelSize.Width)
            {
                MaxLabelSize.Width = width;
                RotatedLabel = label.Text;
            }

            double height = isAxisLabelBreak ? label.BreakLabelSize.Height : label.Size.Height;
            if (height > MaxLabelSize.Height)
            {
                MaxLabelSize.Height = height;
            }
        }

        /// <summary>
        /// Applies the configured label-intersect action (MultipleRows, Rotate45/90, or Wrap) for a single label.
        /// </summary>
        /// <param name="index">The zero-based label index.</param>
        /// <param name="label">The label being processed.</param>
        /// <param name="isAxisLabelBreak"><see langword="true"/> when the label uses break formatting.</param>
        /// <param name="axisLabelStyle">The resolved font options.</param>
        /// <param name="previousEnd">A reference to the X position where the previous label ended.</param>
        private void ProcessIntersectAction(int index, VisibleLabels label, bool isAxisLabelBreak, ChartFontOptions axisLabelStyle, ref double previousEnd)
        {
            double padding = 5;
            double width1 = isAxisLabelBreak ? label.BreakLabelSize.Width : label.Size.Width;
            double height1 = isAxisLabelBreak ? label.BreakLabelSize.Height : label.Size.Height;
            double pointX = (ChartHelper.ValueToCoefficient(label.Value, this) * Rect.Width) + Rect.X;
            pointX = pointX - (width1 / 2) + padding;
            width1 += padding;

            if (Axis?.Renderer?.EdgeLabelPlacement == EdgeLabelPlacement.Shift)
            {
                if (index == 0 && pointX < Rect.X)
                {
                    pointX = Rect.X;
                }

                if (index == VisibleLabels.Count - 1 && ((pointX + width1) > (Rect.X + Rect.Width)))
                {
                    pointX = Rect.X + Rect.Width - width1;
                }
            }
            if (Axis is null)
            {
                return;
            }

            switch (Axis.Renderer?.LabelIntersectAction)
            {
                case LabelIntersectAction.MultipleRows:
                    if (index > 0)
                    {
                        FindMultiRows(index, pointX, label, isAxisLabelBreak);
                    }
                    break;
                case LabelIntersectAction.Rotate45:
                case LabelIntersectAction.Rotate90:
                    if (index > 0 && (!Axis.IsAxisInverse ? pointX <= previousEnd : pointX + width1 >= previousEnd))
                    {
                        Angle = (Axis.Renderer.LabelIntersectAction == LabelIntersectAction.Rotate45) ? 45 : 90;
                    }
                    break;
                default:
                    if (isAxisLabelBreak)
                    {
                        WrapBrokenLabelText(label, axisLabelStyle);
                    }
                    else
                    {
                        label.TextArr = [.. ChartHelper.TextWrap(label.Text, Rect.Width / VisibleLabels.Count, axisLabelStyle)];
                    }

                    double lheight = height1 * label.TextArr.Length;
                    if (lheight > MaxLabelSize.Height)
                    {
                        MaxLabelSize.Height = lheight;
                    }

                    break;
            }

            previousEnd = Axis is not null && Axis.IsAxisInverse ? pointX : pointX + width1;
        }

        /// <summary>
        /// Wraps each segment of a broken label independently and rebuilds the text array.
        /// </summary>
        /// <param name="label">The label whose segments are wrapped.</param>
        /// <param name="axisLabelStyle">The font options used for measuring wrap width.</param>
        private void WrapBrokenLabelText(VisibleLabels label, ChartFontOptions axisLabelStyle)
        {
            string[] result;
            List<string> result1 = [];
            string str;
            for (int index = 0; index < label.TextArr.Length; index++)
            {
                result = [.. ChartHelper.TextWrap(label.TextArr[index], Rect.Width / VisibleLabels.Count, axisLabelStyle)];
                if (result.Length > 1)
                {
                    for (int j = 0; j < result.Length; j++)
                    {
                        str = result[j];
                        result1.Add(str);
                    }
                }
                else
                {
                    result1.Add(result[0]);
                }
            }

            label.TextArr = [.. result1];
        }

        /// <summary>
        /// Calculates the scrollbar height that should be reserved for the axis based on scrollbar settings and zoom state.
        /// </summary>
        /// <returns>
        /// The scrollbar height in pixels when the scrollbar should be rendered next to the axis line;
        /// otherwise, <c>0</c>.
        /// </returns>
        private double GetScrollbarHeight()
        {
            bool isVerticalAxis = Orientation == Orientation.Vertical;
            ScrollbarPosition originalPosition = Axis?.ScrollbarSettings.Position ?? ScrollbarPosition.PlaceNextToAxisLine;
            ScrollbarPosition actualPosition = (isVerticalAxis && (originalPosition == ScrollbarPosition.Top || originalPosition == ScrollbarPosition.Bottom))
                || (!isVerticalAxis && (originalPosition == ScrollbarPosition.Left || originalPosition == ScrollbarPosition.Right)) ? ScrollbarPosition.PlaceNextToAxisLine : originalPosition;
            bool shouldRenderScrollbar = Axis?.Renderer is not null && Axis.Renderer.ShouldRenderScrollbar && (Axis.ScrollbarSettings.Enable || (Owner?._zoomingModule is not null && Owner._zoomSettings.EnableScrollbar
                && Axis.EnableScrollbarOnZooming && (Axis.ZoomFactor < 1 || Axis.ZoomPosition > 0)));

            return (shouldRenderScrollbar && actualPosition == ScrollbarPosition.PlaceNextToAxisLine) ? Axis?.ScrollbarSettings.Height ?? 16 : 0;
        }

        /// <summary>
        /// Resolves the theme color for the specified axis grid or tick element key.
        /// </summary>
        /// <param name="key">
        /// The element key identifying the target line type.
        /// Use <c>Constants.MajorGridLine</c>, <c>Constants.MajorTickLine</c>,
        /// <c>Constants.MinorGridLine</c>, or <c>Constants.MinorTickLine</c>.
        /// </param>
        /// <returns>The resolved theme color string, or <see cref="string.Empty"/> when the key is not recognized.</returns>
        private string AxisThemeColor(string key)
        {
            return key switch
            {

                Constants.MajorGridLine => Chart?._chartThemeStyle?.MajorGridLine ?? string.Empty,
                Constants.MajorTickLine => Chart?._chartThemeStyle?.MajorTickLine ?? string.Empty,
                Constants.MinorGridLine => Chart?._chartThemeStyle?.MinorGridLine ?? string.Empty,
                Constants.MinorTickLine => Chart?._chartThemeStyle?.MinorTickLine ?? string.Empty,
                _ => string.Empty,
            };
        }

        /// <summary>
        /// Renders all inside-collection elements for a Cartesian axis, including grid lines, tick marks,
        /// labels, borders, multi-level labels, and the axis title, into the provided render tree.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to build the SVG render tree.</param>
        private void RenderCartesianAxisInsideCollection(RenderTreeBuilder builder)
        {
            SvgRenderer?.OpenGroupElement(builder, Owner?.ID + "AxisGroup" + Index + "Inside", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "true");
            if (AxisRenderInfo.AxisLine is not null && !IsAxisInside)
            {
                DrawLine(builder, AxisRenderInfo.AxisLine);
            }

            foreach (KeyValuePair<string, List<PathOptions>> keyValue in AxisRenderInfo.AxisGridOptions.ToArray())
            {
                if (keyValue.Key.Equals(Constants.MajorGridLine, StringComparison.Ordinal) || keyValue.Key.Equals(Constants.MinorGridLine, StringComparison.Ordinal))
                {
                    DrawLine(builder, keyValue.Value);
                }

                if (!IsTickInside)
                {
                    if (keyValue.Key.Equals(Constants.MajorTickLine, StringComparison.Ordinal))
                    {
                        DrawLine(builder, keyValue.Value);
                    }

                    if (keyValue.Key.Equals(Constants.MinorTickLine, StringComparison.Ordinal))
                    {
                        DrawLine(builder, keyValue.Value);
                    }
                }
            }

            if (!IsAxisLabelInside)
            {
                if (Axis?.LabelTemplate is not null)
                {
                    Owner?._axisLabelTemplateContainer?.InvalidateRender();
                }
                else
                {
                    SvgRenderer?.OpenGroupElement(builder, Owner?.ID + "AxisLabels" + Index);
                    foreach (TextOptions option in AxisRenderInfo.AxisLabelOptions.ToArray())
                    {
                        if (option is not null)
                        {
                            ChangeAxisLabelText(option);
                            ChartHelper.TextElement(builder, SvgRenderer ?? null!, option);
                        }
                    }
                    builder.CloseElement();
                }

                if (AxisRenderInfo.AxisBorder is not null)
                {
                    SvgRenderer?.RenderPath(builder, AxisRenderInfo.AxisBorder, "e-pointer-series");
                }

                if (Orientation != Orientation.Null)
                {
                    MultiLevelLabelRenderer?.RenderMultilevelLabel(builder);
                }
            }

            if (AxisRenderInfo.RowBorder is not null)
            {
                DrawLine(builder, AxisRenderInfo.RowBorder);
            }

            if (AxisRenderInfo.ColumnBorder is not null)
            {
                DrawLine(builder, AxisRenderInfo.ColumnBorder);
            }

            if (AxisRenderInfo.AxisTitleOption is not null && !IsAxisInside)
            {
                ChartHelper.TextElement(builder, SvgRenderer ?? null!, AxisRenderInfo.AxisTitleOption);
            }

            builder.CloseElement();
        }

        /// <summary>
        /// Computes the rendered size of a label that uses a custom format string by combining
        /// the pre-measured static format portion with the measured dynamic value portion.
        /// </summary>
        /// <param name="label">The full label text including the formatted value.</param>
        /// <param name="labelStyle">The font options used for text measurement.</param>
        /// <param name="formatTextSize">The pre-measured size of the static portion of the format string.</param>
        /// <param name="formatText">The static portion of the format string used to isolate the value text.</param>
        /// <returns>
        /// A <see cref="Size"/> whose width is the sum of the static and value widths,
        /// and whose height is the maximum of the two heights.
        /// </returns>
        private static Size GetCustomFormatLabelSize(string label, ChartFontOptions labelStyle, Size formatTextSize, string formatText)
        {
            Size axisValueSize = ChartHelper.MeasureText(ChartHelper.SplitLabelFormat(label, formatText), labelStyle);
            return new Size { Width = formatTextSize.Width + axisValueSize.Width, Height = Math.Max(formatTextSize.Height, axisValueSize.Height) };
        }

        /// <summary>
        /// Converts the <paramref name="crossAt"/> object value to a numeric double based on the value type of the specified <paramref name="axis"/>.
        /// </summary>
        /// <param name="axis">The axis whose <see cref="ChartAxis.ValueType"/> determines the conversion logic.</param>
        /// <param name="crossAt">
        /// The cross-at value to convert. Expected types are <see cref="DateTime"/> for <c>DateTime</c> axes,
        /// a numeric or category string for <c>Category</c> axes, or a numeric value for all other axes.
        /// </param>
        /// <returns>A <see cref="double"/> representation of <paramref name="crossAt"/> appropriate for the axis value type.</returns>
        private double UpdateCrossAt(ChartAxis axis, object crossAt)
        {
            switch (axis.ValueType)
            {
                case ValueType.DateTime:
                    return ((DateTime)crossAt - new DateTime(1970, 1, 1)).TotalMilliseconds;
                case ValueType.Category:
                    string crossValue = crossAt.ToString() ?? string.Empty;
                    return !float.Parse(crossValue, null).Equals(float.NaN) ? float.Parse(crossValue, null) : Labels.IndexOf(crossValue);
                case ValueType.Logarithmic:
                    return ChartHelper.LogBase(Convert.ToDouble(crossAt, null), axis.LogBase);
                default:
                    return Convert.ToDouble(crossAt, Culture);
            }
        }

        /// <summary>
        /// Determines multi-row stacking for a label that would overlap a previously placed label.
        /// </summary>
        /// <param name="length">The number of labels preceding this one.</param>
        /// <param name="currentX">The left X edge of the current label.</param>
        /// <param name="currentLabel">The label being positioned.</param>
        /// <param name="isBreakLabels"><see langword="true"/> when break-label sizing is used.</param>
        private void FindMultiRows(int length, double currentX, VisibleLabels currentLabel, bool isBreakLabels)
        {
            VisibleLabels label;
            double pointX, breakLabelwidth;
            List<double> store = [];
            bool isMultiRows;
            for (int i = length - 1; i >= 0; i--)
            {
                label = VisibleLabels[i];
                breakLabelwidth = isBreakLabels ? label.BreakLabelSize.Width : label.Size.Width;
                pointX = (ChartHelper.ValueToCoefficient(label.Value, this) * Rect.Width) + Rect.X;
                isMultiRows = Axis is not null && !Axis.IsAxisInverse ? currentX < (pointX + (breakLabelwidth * 0.5)) : currentX + currentLabel.Size.Width > (pointX - (breakLabelwidth * 0.5));
                if (isMultiRows)
                {
                    store.Add(label.Index);
                    currentLabel.Index = (currentLabel.Index > label.Index) ? currentLabel.Index : label.Index + 1;
                }
                else
                {
                    currentLabel.Index = store.IndexOf(label.Index) > -1 ? currentLabel.Index : label.Index;
                }
            }

            double height = ((isBreakLabels ? currentLabel.BreakLabelSize.Height : currentLabel.Size.Height) * currentLabel.Index) + (5 * (currentLabel.Index - 1));
            if (height > MaxLabelSize.Height)
            {
                MaxLabelSize.Height = height;
            }
        }

        /// <summary>
        /// Iterates all bound series to determine the raw min/max values for this axis.
        /// </summary>
        /// <returns>A <see cref="DoubleRange"/> representing the raw data extent.</returns>
        private DoubleRange CalculateActualRange()
        {
            Min = double.NaN;
            Max = double.NaN;
            SeriesMin = double.NaN;
            SeriesMax = double.NaN;
            if (Axis is not null && !ChartHelper.SetRange(Axis))
            {
                ChartAxis axis = Axis;
                foreach (ChartSeriesRenderer seriesRenderer in SeriesRenderer.ToArray())
                {
                    ChartSeries series = seriesRenderer.Series ?? null!;
                    if (!series.Visible)
                    {
                        continue;
                    }

                    PaddingInterval = 0;
                    MaxPointLength = seriesRenderer.Points?.Count ?? 0;
                    string type = series.SeriesType ?? null!;
                    if (IsRectType(type) && seriesRenderer.XAxisRenderer.Axis?.ValueType == ValueType.Category && seriesRenderer.XAxisRenderer.Axis.LabelPlacement == LabelPlacement.OnTicks)
                    {
                        PaddingInterval = 0.5;
                    }
                    else if (IsRectType(type) && (seriesRenderer.XAxisRenderer.Axis?.ValueType == ValueType.Double || seriesRenderer.XAxisRenderer.Axis?.ValueType == ValueType.DateTime || (seriesRenderer.XAxisRenderer.Axis?.ValueType == ValueType.DateTimeCategory && seriesRenderer.XAxisRenderer.Axis.LabelPlacement == LabelPlacement.OnTicks)) && seriesRenderer.XAxisRenderer.Axis.RangePadding == ChartRangePadding.Auto)
                    {
                        IsColumnType = true;
                        PaddingInterval = ChartHelper.GetMinPointsDelta(seriesRenderer.XAxisRenderer.Axis, axis.Renderer?.SeriesRenderer ?? null!) * 0.5;
                    }

                    if (Orientation == Orientation.Horizontal)
                    {
                        if (Chart is not null && Chart._requireInvertedAxis)
                        {
                            YAxisRange(series);
                        }
                        else
                        {
                            FindSeriesMinMax(seriesRenderer.XMin, seriesRenderer.XMax);
                            FindMinMax(seriesRenderer.XMin - PaddingInterval, seriesRenderer.XMax + PaddingInterval);
                        }
                    }

                    if (Orientation == Orientation.Vertical)
                    {
                        IsColumn += series.Type is ChartSeriesType.Column or ChartSeriesType.Bar ? 1 : 0;
                        if (Chart is not null && Chart._requireInvertedAxis)
                        {
                            FindSeriesMinMax(seriesRenderer.XMin, seriesRenderer.XMax);
                            FindMinMax(seriesRenderer.XMin - PaddingInterval, seriesRenderer.XMax + PaddingInterval);
                        }
                        else
                        {
                            YAxisRange(series);
                        }
                    }
                }
            }

            return InitializeDoubleRange();
        }

        /// <summary>
        /// Determines whether the specified series type string represents a rectangular (column, bar, or histogram) series.
        /// </summary>
        /// <param name="type">The series type string to evaluate.</param>
        /// <returns><c>true</c> if the type contains "Column", "Histogram", or "Bar"; otherwise, <c>false</c>.</returns>
        private static bool IsRectType(string type)
        {
            return type.Contains("Column", StringComparison.InvariantCulture) || type.Contains("Histogram", StringComparison.InvariantCulture) || type.Contains("Bar", StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Updates running min/max from the Y extents of the specified series.
        /// </summary>
        /// <param name="series">The series whose Y range is applied.</param>
        private void YAxisRange(ChartSeries series)
        {
            FindMinMax(series.Renderer.YMin, series.Renderer.YMax);
        }

        /// <summary>
        /// Updates the axis running minimum and maximum from a new candidate pair.
        /// </summary>
        /// <param name="min">Candidate minimum value.</param>
        /// <param name="max">Candidate maximum value.</param>
        private void FindMinMax(double min, double max)
        {
            Min = (double.IsNaN(Min) || Min > min) ? min : Min;
            Max = (double.IsNaN(Max) || Max < max) ? max : Max;
            Max = double.IsNaN(Axis?.Interval ?? 0) && Max == Min && Max < 0 && Min < 0 ? 0 : Max;
        }

        /// <summary>
        /// Updates the series-level running minimum and maximum from a new candidate pair.
        /// </summary>
        /// <param name="min">Candidate series minimum value.</param>
        /// <param name="max">Candidate series maximum value.</param>
        private void FindSeriesMinMax(double min, double max)
        {
            SeriesMin = (double.IsNaN(SeriesMin) || SeriesMin > min) ? min : SeriesMin;
            SeriesMax = (double.IsNaN(SeriesMax) || SeriesMax < max) ? max : SeriesMax;
            SeriesMax = double.IsNaN(Axis?.Interval ?? 0) && SeriesMax == SeriesMin && SeriesMax < 0 && SeriesMin < 0 ? 0 : SeriesMax;
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// Cleans up resources when the component is disposed.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            VisibleLabels?.Clear();
            ClearAxisInfo();
            return base.DisposeAsyncCore();
        }

        /// <summary>
        /// Triggers the axis actual range calculated event and allows modification.
        /// </summary>
        /// <param name="range">Current actual range.</param>
        /// <returns>Possibly modified range.</returns>
        protected DoubleRange TriggerRangeRender(DoubleRange range)
        {
            AxisRangeCalculatedEventArgs argsData = new("OnAxisActualRangeCalculated", false, range.Start, range.End, VisibleInterval, Rect, Axis?.Name ?? string.Empty);
            Chart?.OnAxisActualRangeCalculated.Invoke(argsData);

            if (!argsData.Cancel)
            {
                VisibleInterval = argsData.Interval;
                return new DoubleRange(argsData.Minimum, argsData.Maximum);
            }
            return range;
        }

        /// <summary>
        /// Triggers label render event for a label and stores visible label when not canceled.
        /// </summary>
        /// <param name="tempInterval">Interval associated with label.</param>
        /// <param name="text">Label text.</param>
        /// <param name="dateTime">Optional associated date/time.</param>
        protected void TriggerLabelRender(double tempInterval, string text, DateTime? dateTime = null)
        {
            AxisLabelRenderEventArgs argsData = new("OnAxisLabelRender", false, Axis ?? null!, text, tempInterval, Axis?.LabelStyle ?? null!);

            if (Chart?.OnAxisLabelRender is not null)
            {
                Chart.OnAxisLabelRender.Invoke(argsData);
            }

            if (!argsData.Cancel)
            {
                VisibleLabels.Add(new VisibleLabels((Axis is not null && Axis.EnableTrim) || (Axis is not null && Axis.IsAxisLabelTrim) ? ChartHelper.TextTrim(Axis.MaximumLabelWidth, argsData.Text, Axis.LabelStyle.GetChartFontOptions(Owner?._chartThemeStyle ?? null!)) : argsData.Text, argsData.Value, argsData.LabelStyle as ChartAxisLabelStyle ?? null!, argsData.Text, null!, null!, 1, dateTime is not null ? dateTime : null));
            }
        }

        /// <summary>
        /// Calculates alternate axis ranges when zooming and auto-interval is enabled.
        /// </summary>
        /// <param name="visibleRange">Current visible range.</param>
        protected void CalculateAutoIntervalOnBothAxisRange(DoubleRange visibleRange)
        {
            if (Orientation == Orientation.Horizontal /*&& Chart.ZoomSettings.Mode == ZoomMode.X*/)
            {
                SetYAxisMinMax(Chart?._axisContainer?.Axes.Values.ToList() ?? null!, visibleRange);
            }

            if (Orientation == Orientation.Vertical /*&& Chart.ZoomSettings.Mode == ZoomMode.Y*/)
            {
                SetXAxisMinMax(Chart?._axisContainer?.Axes.Values.ToList() ?? null!, visibleRange);
            }
        }

        /// <summary>
        /// Computes <see cref="MaxLabelSize"/> by iterating visible labels, measuring each, and handling rotation and intersection actions.
        /// </summary>
        protected void GetMaxLabelWidth()
        {
            MaxLabelSize = new Size(0, 0);

            AxisRenderInfo rendererAxisInfo = Axis?.Renderer?.AxisRenderInfo ?? null!;
            if (rendererAxisInfo.AxisLabelTemplateSizeList is not null && rendererAxisInfo.AxisLabelTemplateSizeList.Count > 0)
            {
                GetMaxTemplateWidth(rendererAxisInfo);
                return;
            }

            Angle = Axis?.Renderer?.LabelRotation ?? 0;
            LabelIntersectAction? action = Axis?.Renderer?.LabelIntersectAction;
            double previousEnd = 0;

            ChartFontOptions axisLabelStyle = Chart is not null && Axis is not null ? Axis.LabelStyle.GetChartFontOptions(Chart._chartThemeStyle ?? null!) : Axis?.LabelStyle.GetChartFontOptions() ?? null!;
            bool isCustomLabelFormat = Axis is not null && Axis.LabelFormat.Contains("{value}", StringComparison.InvariantCulture);
            string formatText = isCustomLabelFormat ? ChartHelper.SplitLabelFormat(Axis?.LabelFormat ?? string.Empty, "{value}") : string.Empty;
            Size formatTextSize = isCustomLabelFormat ? ChartHelper.MeasureText(formatText, axisLabelStyle) : null!;

            for (int i = 0, len = VisibleLabels.Count; i < len; i++)
            {
                VisibleLabels label = VisibleLabels[i];
                bool isAxisLabelBreak = label.OriginalText.Contains("<br>", StringComparison.InvariantCulture);

                MeasureLabelSizes(label, isAxisLabelBreak, axisLabelStyle, isCustomLabelFormat, formatTextSize, formatText);
                UpdateMaxLabelWidth(label, isAxisLabelBreak);

                if (isAxisLabelBreak)
                {
                    label.TextArr = Axis is not null && Axis.EnableTrim ? [label.Text] : label.OriginalText.Split("<br>");
                }

                if (action is LabelIntersectAction.None or LabelIntersectAction.Hide or LabelIntersectAction.Trim)
                {
                    continue;
                }

                if ((action != LabelIntersectAction.None || Angle % 360 == 0) && Orientation == Orientation.Horizontal && Rect.Width > 0)
                {
                    ProcessIntersectAction(i, label, isAxisLabelBreak, axisLabelStyle, ref previousEnd);
                }
            }

            MaxLabelSize.Height = VisibleLabels.Count == 0 && Axis is not null && Axis.CrosshairTooltip.Enable ? DEFAULT_LABEL_HEIGHT : MaxLabelSize.Height;

            if (Angle != 0 && Orientation == Orientation.Horizontal)
            {
                RotatedLabel = string.IsNullOrEmpty(RotatedLabel) ? string.Empty : RotatedLabel;
                MaxLabelSize = RotatedLabel.Contains("<br>", StringComparison.InvariantCulture) ? ChartHelper.MeasureText(RotatedLabel, axisLabelStyle) : ChartHelper.RotateTextSize(axisLabelStyle, RotatedLabel, Angle);
            }

            if (Axis?.MultiLevelLabels.Count > 0)
            {
                MultiLevelLabelRenderer = new MultiLevelLabelRenderer();
                MultiLevelLabelRenderer.IniMultilevelLabel(this);
            }
        }

        /// <summary>
        /// Applies range padding to the calculated axis range based on the padding mode and interval.
        /// Derived renderers override this method to provide type-specific padding logic.
        /// </summary>
        /// <param name="range">The raw <see cref="DoubleRange"/> computed from series data before padding is applied.</param>
        /// <param name="interval">The calculated axis interval used to determine padding increments.</param>
        /// <returns>
        /// A padded <see cref="DoubleRange"/>; the base implementation returns the original
        /// <paramref name="range"/> unchanged.
        /// </returns>
        protected virtual DoubleRange ApplyRangePadding(DoubleRange range, double interval)
        {
            return range;
        }

        /// <summary>
        /// Calculates a visually pleasant interval for a numeric axis delta.
        /// </summary>
        /// <param name="delta">The range delta (max minus min) to subdivide.</param>
        /// <returns>A nice-rounded interval value.</returns>
        protected double CalculateNumericNiceInterval(double delta)
        {
            double actualDesiredIntervalsCount = GetActualDesiredIntervalsCount();
            double niceInterval = delta / actualDesiredIntervalsCount;
            double minInterval = Math.Pow(10, Math.Floor(ChartHelper.LogBase(niceInterval, 10)));

            if (Axis is not null && !double.IsNaN(Axis.DesiredIntervals))
            {
                return niceInterval;
            }

            foreach (double interval in IntervalDivs)
            {
                double currentInterval = minInterval * interval;
                if (actualDesiredIntervalsCount < (delta / currentInterval))
                {
                    break;
                }
                niceInterval = currentInterval;
            }

            return niceInterval;
        }

        /// <summary>
        /// Returns the desired number of axis label intervals based on available size or a fixed setting.
        /// </summary>
        /// <returns>A positive double representing the target interval count.</returns>
        protected double GetActualDesiredIntervalsCount()
        {
            return Axis is not null && double.IsNaN(Axis.DesiredIntervals)
                ? Axis.Renderer?.Orientation == Orientation.Horizontal ? Math.Max((AxisAvailabelSize?.Width ?? 0) * 0.533 * Axis.MaximumLabels / 100, 1) : Math.Max((AxisAvailabelSize?.Height ?? 0) * Axis.MaximumLabels / 100, 1)
                : Axis?.DesiredIntervals ?? 0;
        }

        /// <summary>
        /// Resolves the effective <see cref="ChartRangePadding"/> for this axis, applying defaults based on orientation and series type.
        /// </summary>
        /// <returns>The <see cref="ChartRangePadding"/> to be applied.</returns>
        protected ChartRangePadding GetRangePadding
        {
            get
            {
                ChartRangePadding padding = Axis?.RangePadding ?? ChartRangePadding.Auto;
                if (padding != ChartRangePadding.Auto)
                {
                    return padding;
                }

                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        bool isHorizontal = Chart is not null && Chart._requireInvertedAxis;
                        padding = isHorizontal ? (IsStack100 ? ChartRangePadding.Round : ChartRangePadding.Normal) : ChartRangePadding.None;
                        break;
                    case Orientation.Vertical:
                        bool isVertical = Chart is not null && !Chart._requireInvertedAxis;
                        padding = isVertical ? (IsStack100 ? ChartRangePadding.Round : ChartRangePadding.Normal) : ChartRangePadding.None;
                        break;
                    case Orientation.Null:
                        break;
                    default:
                        break;
                }
                return padding;
            }
        }

        /// <summary>
        /// Builds the render tree for this axis renderer.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (_availableRect is not null && builder is not null)
            {
                if (IsAxisRendererRect())
                {
                    return;
                }
                if (Owner?._axisContainer is not null && Owner._axisContainer.AxisLayout is CartesianAxisLayout)
                {
                    RenderCartesianAxisInsideCollection(builder);
                }
                else
                {
                    RenderPolarRadarAxisInsideCollection(builder);
                }

                RendererShouldRender = false;
            }
        }

        /// <summary>
        /// Computes the visible range when zoom factor or zoom position is active.
        /// </summary>
        /// <returns>A <see cref="DoubleRange"/> clamped within the actual axis range.</returns>
        protected DoubleRange CalculateVisibleRangeOnZooming()
        {
            double start, end;
            if (Axis is not null && !Axis.IsAxisInverse)
            {
                start = ActualRange.Start + (Axis.ZoomPosition * ActualRange.Delta);
                end = start + (Axis.ZoomFactor * ActualRange.Delta);
            }
            else
            {
                start = ActualRange.End - ((Axis?.ZoomPosition ?? 0) * ActualRange.Delta);
                end = start - ((Axis?.ZoomFactor ?? 0) * ActualRange.Delta);
            }

            if (start < ActualRange.Start)
            {
                end += ActualRange.Start - start;
                start = ActualRange.Start;
            }

            if (end > ActualRange.End)
            {
                start -= end - ActualRange.End;
                end = ActualRange.End;
            }

            return new DoubleRange(start, end);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Finds height/length of the major ticks depending on settings.
        /// </summary>
        /// <returns>Tick length.</returns>
        internal double FindTickSize()
        {
            return Axis?.TickPosition == AxisPosition.Inside
                ? 0
                : CrossInAxis?.Renderer is not null && IsInside(CrossInAxis.Renderer.VisibleRange, CrossAt)
                ? 0
                : Axis?.Renderer?.MajorTickLinesHeight ?? 0;
        }

        /// <summary>
        /// Calculates label area size including title and cross axis adjustments.
        /// </summary>
        /// <param name="innerPadding">Inner padding between elements.</param>
        /// <param name="axisWidth">Available axis width for title wrapping.</param>
        /// <returns>Calculated size for labels/title.</returns>
        internal double FindLabelSize(double innerPadding, double axisWidth)
        {
            double titleSize = CalculateTitleSize(innerPadding, axisWidth);

            if (Owner is not null && Owner.EnableAdaptiveRendering && IsExtremelySmall(Axis?.Renderer?.Orientation ?? Orientation.Horizontal))
            {
                return titleSize;
            }

            if (Axis?.Renderer?.LabelPosition == AxisPosition.Inside)
            {
                return titleSize + innerPadding;
            }

            double baseLabelSize = (titleSize + innerPadding + Constants.AxisPadding + (Axis?.LabelPadding ?? 5) + (Axis?.Renderer?.Orientation == Orientation.Vertical ? Axis.Renderer.MaxLabelSize.Width : Axis?.Renderer?.MaxLabelSize.Height ?? 0) + Axis?.Renderer?.MultiLevelLabelHeight) ?? 0;

            return CrossInAxis?.Renderer is not null && Axis is not null && Axis.PlaceNextToAxisLine
                ? ApplyCrossAxisLabelAdjustment(baseLabelSize)
                : baseLabelSize;
        }

        /// <summary>
        /// Determines whether the axis's <c>CrossAt</c> value lies within the specified <paramref name="range"/> or falls on the acceptable boundary based on whether the axis is in an opposed position.
        /// </summary>
        /// <param name="range"> The numeric range within which the <c>CrossAt</c> value is evaluated. </param>
        /// <returns>
        /// <c>true</c> if <c>CrossAt</c> is inside the range or meets the boundary conditions derived from the axis's opposed state; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsInside(DoubleRange range)
        {
            bool isOppposed = Axis is not null && Axis.IsAxisOpposedPosition;
            return ChartHelper.Inside(CrossAt, range) || (!isOppposed && CrossAt >= range.End) || (isOppposed && CrossAt <= range.Start);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="crossValue"/> lies within the given <paramref name="range"/> or satisfies the boundary conditions depending on whether the axis is in an opposed position.
        /// </summary>
        /// <param name="range"> The numeric range to evaluate against.</param>
        /// <param name="crossValue"> The value to check for containment or boundary validity.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="crossValue"/> is inside the range or falls on the boundary allowed for the axis's opposed state; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsInside(DoubleRange range, double crossValue)
        {
            bool isOppposed = Axis is not null && Axis.IsAxisOpposedPosition;
            return ChartHelper.Inside(crossValue, range) || (!isOppposed && crossValue >= range.End) || (isOppposed && crossValue <= range.Start);
        }

        /// <summary>
        /// Formats a label value using axis format and adaptive rules.
        /// </summary>
        /// <param name="tempInterval">Numeric value to format.</param>
        /// <param name="isCustomlabel">Indicates whether custom label format is used.</param>
        /// <param name="fractionCount">Maximum fraction digits.</param>
        /// <returns>Formatted label string.</returns>
        internal string FormatValue(double tempInterval, bool isCustomlabel = true, int fractionCount = 10)
        {
            string labelFormat = isCustomlabel ? GetFormat() : string.Empty;
            Regex regex = new("(?<=[\\.])[0-9]+");
            bool isFraction = regex.Match(Convert.ToString(tempInterval, CultureInfo.InvariantCulture)).Value.Length > 2,
            isCustom = labelFormat.Contains("{value}", StringComparison.InvariantCulture),
            isNumericFormat = labelFormat.Contains('n', StringComparison.InvariantCulture) || labelFormat.Contains('p', StringComparison.InvariantCulture) || labelFormat.Contains('c', StringComparison.InvariantCulture);

            if (Convert.ToString(VisibleInterval, CultureInfo.InvariantCulture).Contains('.', StringComparison.InvariantCulture))
            {
                fractionCount = Convert.ToString(VisibleInterval, CultureInfo.InvariantCulture).Split('.')[1].Length;
            }
            if (isFraction && !isNumericFormat)
            {
                tempInterval = Convert.ToDouble(tempInterval.ToString("N" + fractionCount, null), null);
            }

            string labelFormatValue = Axis?.Renderer?.Type == ValueType.Double && Chart is not null && Chart.UseGroupingSeparator && (labelFormat.Contains("{value}", StringComparison.InvariantCulture) || string.IsNullOrEmpty(labelFormat)) ? "#,##0.###" : labelFormat;
            string formatValue = ChartHelper.FormatValue(tempInterval, isCustom && !labelFormatValue.Contains("#,##0.###", StringComparison.InvariantCulture), labelFormatValue);
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            string currencyFormat = "C";

            if (Axis?.Renderer?.Type == ValueType.Double && !labelFormatValue.Contains("{value}", StringComparison.InvariantCulture) && labelFormatValue.ToUpper(CultureInfo.InvariantCulture).Contains(currencyFormat, StringComparison.InvariantCulture))
            {
                int currencySymbolIndex = formatValue.IndexOf(currentCulture.NumberFormat.CurrencySymbol, 0, StringComparison.InvariantCulture);
                if (Chart is not null && Chart._seriesContainer is not null)
                {
                    Chart._seriesContainer._isBackSymbol = currencySymbolIndex > 0;
                }
            }

            bool isAdaptiveLabels = Chart is not null && Chart.EnableAdaptiveRendering && (Chart._widthCategory != ChartWidthCategory.Normal);
            formatValue = isAdaptiveLabels && !isCustom ? GetAdaptiveFormatValue(tempInterval, formatValue) : formatValue;
            return isCustom ? labelFormat.Replace("{value}", formatValue, StringComparison.InvariantCulture) : formatValue;
        }

        /// <summary>
        /// Rotates a label size by the given angle and returns the bounding dimensions of the rotated rectangle.
        /// </summary>
        /// <param name="label">The original <see cref="Size"/> of the label before rotation.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <returns>A <see cref="Size"/> representing the bounding box of the rotated label.</returns>
        internal static Size RotateTemplateSize(Size label, double angle)
        {
            double theta = angle * Math.PI / 180.0;
            double sin = Math.Abs(Math.Sin(theta));
            double cos = Math.Abs(Math.Cos(theta));
            double rotatedWidth = (label.Width * cos) + (label.Height * sin);
            double rotatedHeight = (label.Width * sin) + (label.Height * cos);
            return new Size(rotatedWidth, rotatedHeight);
        }

        /// <summary>Gets renderer type for a ValueType.</summary>
        /// <param name="type">Value type.</param>
        /// <returns>Type of axis renderer.</returns>
        internal static Type GetRendererType(ValueType type)
        {
            return type switch
            {
                ValueType.Double => typeof(NumericAxisRenderer),
                ValueType.DateTime => typeof(DateTimeAxisRenderer),
                ValueType.Category => typeof(CategoryAxisRenderer),
                ValueType.DateTimeCategory => typeof(DateTimeCategoryAxisRenderer),
                ValueType.Logarithmic => typeof(LogarithmicAxisRenderer),
                _ => null!,
            };
        }

        /// <summary>
        /// Computes sizes and triggers label generation.
        /// </summary>
        /// <param name="availableSize">Available size for axis area.</param>
        internal void ComputeSize(Size availableSize)
        {
            AxisAvailabelSize = availableSize;
            Rect = new Rect(0, 0, availableSize.Width, availableSize.Height);
            if (Axis is { })
            {
                Axis.ScrollBarHeight = GetScrollbarHeight();
            }
            CalculateRangeAndInterval();
            GenerateVisibleLabels();
        }

        /// <summary>
        /// Changes axis range and optionally recalculates.
        /// </summary>
        /// <param name="updateRange">If true recalculates range.</param>
        internal void ChangeAxisRange(bool updateRange = true)
        {
            RendererShouldRender = true;
            ClearAxisInfo();
            if (updateRange)
            {
                CalculateRangeAndInterval();
            }

            GenerateVisibleLabels();
            UpdateAxisRendering();
        }

        /// <summary>
        /// Triggers axis rendering calculations and requests a re-render of this axis and any associated annotation containers.
        /// </summary>
        internal void UpdateAxisRendering()
        {
            RendererShouldRender = true;
            SetAdaptiveAxisValues(Axis ?? null!, Orientation);
            Owner?._axisContainer?.AxisLayout?.AxisRenderingCalculation(this);
            ProcessRenderQueue();
            Owner?._annotationContainer?.UpdateRenderers();
        }

        /// <summary>
        /// Determines whether both the <see cref="ChartAxis.Minimum"/> and <see cref="ChartAxis.Maximum"/>
        /// properties are explicitly set on the axis, indicating a fixed range.
        /// </summary>
        /// <returns><c>true</c> if both minimum and maximum are set; otherwise, <c>false</c>.</returns>
        internal bool IsFixedRange()
        {
            return Axis?.Minimum is not null && Axis.Maximum is not null;
        }

        /// <summary>Determines whether number of digits changed enough to trigger relayout.</summary>
        /// <param name="min">Minimum to compare.</param>
        /// <param name="max">Maximum to compare.</param>
        /// <returns>True when layout change needed.</returns>
        internal bool NeedAxisLayoutChange(double min, double max)
        {
            int maxDigits = Math.Max(ActualRange.Start.ToString(CultureInfo.InvariantCulture).Length, ActualRange.End.ToString(CultureInfo.InvariantCulture).Length);
            int minDigits = Math.Max(min.ToString(CultureInfo.InvariantCulture).Length, max.ToString(CultureInfo.InvariantCulture).Length);
            return minDigits != maxDigits;
        }

        /// <summary>
        /// Computes <see cref="ActualRange"/>, <see cref="ActualInterval"/>, and <see cref="VisibleRange"/>
        /// for this axis based on the bound series data.
        /// </summary>
        internal void CalculateRangeAndInterval()
        {
            DoubleRange range = CalculateActualRange();
            VisibleInterval = ActualInterval = CalculateActualInterval(range);
            ActualRange = ApplyRangePadding(range, ActualInterval);
            VisibleRange = CalculateVisibleRange(ActualRange);
        }

        /// <summary>
        /// Initializes the <see cref="DoubleRange"/> for this axis by resolving the final minimum and maximum
        /// values from series data, explicit <see cref="ChartAxis.Minimum"/> and <see cref="ChartAxis.Maximum"/>
        /// overrides, and category-specific fallbacks when all series are hidden.
        /// </summary>
        /// <returns>
        /// A <see cref="DoubleRange"/> constructed from the resolved <see cref="Min"/> and <see cref="Max"/> values.
        /// </returns>
        internal virtual DoubleRange InitializeDoubleRange()
        {
            bool isCategory = Axis?.ValueType is ValueType.Category or ValueType.DateTimeCategory;

            // calculate axis Maximum of category & datetimcategory axis when all series make unvisible using legend
            if (isCategory && double.IsNaN(Max) && !SeriesRenderer.Any(series => series?.Series is not null && series.Series.Visible) && Axis?.Maximum is null)
            {
                List<int> points = [];

                foreach (ChartSeriesRenderer series in SeriesRenderer.ToArray())
                {
                    points.Add((series.Points?.Count ?? 0) - 1);
                }

                Max = points.Max();
            }

            Min = Axis?.Minimum is not null ? Convert.ToDouble(Axis.Minimum, null) : (double.IsNaN(Min) || double.IsPositiveInfinity(Min)) ? 0 : Min;
            Max = Axis?.Maximum is not null ? Convert.ToDouble(Axis.Maximum, null) : (double.IsNaN(Max) || double.IsNegativeInfinity(Max)) ? 5 : Max;

            if (Min == Max && !isCategory)
            {
                if (Min < 0)
                {
                    Min = Max - 1;
                }
                else
                {
                    Max = Min + 1;
                }
            }

            return new DoubleRange(Min, Max);
        }

        /// <summary>
        /// Calculates the actual interval for the given range. Derived renderers override this method
        /// to provide type-specific interval calculation logic.
        /// </summary>
        /// <param name="range">The <see cref="DoubleRange"/> for which the interval is calculated.</param>
        /// <returns>A <see cref="double"/> representing the calculated interval; defaults to <c>1.0</c>.</returns>
        internal virtual double CalculateActualInterval(DoubleRange range)
        {
            return 1.0;
        }

        /// <summary>
        /// Calculates the visible range for the axis, applying zoom adjustments, auto-interval recalculation,
        /// range-padding corrections, and raising the <c>OnAxisActualRangeCalculated</c> event when subscribed.
        /// Derived renderers may override this method to apply type-specific visible range logic.
        /// </summary>
        /// <param name="actualRange">The padded actual range to base the visible range on.</param>
        /// <returns>A <see cref="DoubleRange"/> representing the final visible range.</returns>
        internal virtual DoubleRange CalculateVisibleRange(DoubleRange actualRange)
        {
            if (Chart?._chartAreaType == ChartAreaType.CartesianAxes && (Axis?.ZoomFactor < 1 || Axis?.ZoomPosition > 0))
            {
                actualRange = CalculateVisibleRangeOnZooming();
                if (Axis.EnableAutoIntervalOnZooming && Axis.ValueType != ValueType.Category)
                {
                    CalculateAutoIntervalOnBothAxisRange(actualRange);
                    VisibleInterval = CalculateNumericNiceInterval(actualRange.Delta);
                }
            }

            if (Axis?.ValueType == ValueType.Double && Axis.Renderer?.Orientation == Orientation.Vertical && Axis.RangePadding == ChartRangePadding.Auto)
            {
                double rangeDifference = (actualRange.End - actualRange.Start) % VisibleInterval;

                if (!ChartHelper.IsNaNOrZero(rangeDifference))
                {
                    double intervals = Math.Floor((actualRange.End - actualRange.Start) / VisibleInterval);
                    double duplicateTempInterval = actualRange.Start + (intervals * VisibleInterval);

                    if (duplicateTempInterval < actualRange.End)
                    {
                        actualRange = new DoubleRange(actualRange.Start, duplicateTempInterval + VisibleInterval);
                    }
                }
            }

            if (Chart is not null && Chart.OnAxisActualRangeCalculated is not null)
            {
                actualRange = TriggerRangeRender(actualRange);
            }

            return actualRange;
        }

        /// <summary>
        /// Populates the <see cref="VisibleLabels"/> collection for this axis.
        /// Derived renderers override this method to generate type-specific labels.
        /// </summary>
        internal virtual void GenerateVisibleLabels()
        {
            RendererShouldRender = true;
        }

        /// <summary>
        /// Sets default renderer values during initialization.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {
            HandleChartSizeChange(Owner?.InitialRect ?? new Rect(0, 0, 0, 0));
            Container?.SetDefaultRendererContainerValues();
        }

        /// <summary>
        /// Determines whether the axis renderer rectangle contains any <see cref="double.NaN"/> values
        /// that would prevent safe rendering.
        /// </summary>
        /// <returns>
        /// <c>true</c> if any of the rectangle's <c>X</c>, <c>Y</c>, <c>Width</c>, or <c>Height</c> values
        /// is <see cref="double.NaN"/>; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsAxisRendererRect()
        {
            return double.IsNaN(Rect.Width) || double.IsNaN(Rect.Height) || double.IsNaN(Rect.X) || double.IsNaN(Rect.Y);
        }

        /// <summary>
        /// Resolves the effective label format string for this axis, applying the 100-stacked percentage
        /// format when applicable.
        /// </summary>
        /// <returns>
        /// The axis <see cref="ChartAxis.LabelFormat"/> if set; <c>"{value}%"</c> when the axis is
        /// part of a 100% stacked series; or <see cref="string.Empty"/> when no format is defined.
        /// </returns>
        internal string GetFormat()
        {
            return !string.IsNullOrEmpty(Axis?.LabelFormat)
                ? Axis.LabelFormat.StartsWith('p') && !Axis.LabelFormat.Contains("{value}", StringComparison.InvariantCulture) && IsStack100
                    ? "{value}%"
                    : Axis.LabelFormat
                : IsStack100 ? "{value}%" : string.Empty;
        }

        /// <summary>
        /// Updates rendering options for a specific axis element key (such as grid lines, label style,
        /// or title style) and marks the renderer as dirty so the next render pass picks up the changes.
        /// </summary>
        /// <param name="key">
        /// The element key to update. Supported values are <c>"MajorGridLines"</c>, <c>"LabelStyle"</c>,
        /// and <c>"TitleStyle"</c>.
        /// </param>
        internal void CustomizeGridRenderingOptions(string key)
        {
            RendererShouldRender = true;
            switch (key)
            {
                case "MajorGridLines":
                    if (AxisRenderInfo.AxisGridOptions.TryGetValue(key, out List<PathOptions>? axisGridOptions))
                    {
                        foreach (PathOptions option in axisGridOptions)
                        {
                            option.Stroke = Axis?.MajorGridLines.Color ?? string.Empty;
                            option.StrokeWidth = Axis?.Renderer?.MajorGridLinesWidth ?? 0;
                            option.StrokeDashArray = Axis?.MajorGridLines.DashArray ?? string.Empty;
                        }
                    }
                    else
                    {
                        foreach (CircleOptions option in AxisRenderInfo.MajorGridCircleOptions)
                        {
                            option.Stroke = Axis?.MajorGridLines.Color ?? string.Empty;
                            option.StrokeWidth = Axis?.Renderer?.MajorGridLinesWidth ?? 0;
                            option.StrokeDashArray = Axis?.MajorGridLines.DashArray ?? string.Empty;
                        }
                    }

                    break;
                case "LabelStyle":
                    foreach (TextOptions option in AxisRenderInfo.AxisLabelOptions)
                    {
                        option.Fill = Axis?.LabelStyle.Color ?? string.Empty;
                        option.FontSize = Axis?.LabelStyle.Size ?? string.Empty;
                        option.FontFamily = Axis?.LabelStyle.FontFamily ?? string.Empty;
                    }

                    break;
                case "TitleStyle":
                    if (AxisRenderInfo.AxisTitleOption is not null)
                    {
                        AxisRenderInfo.AxisTitleOption.Fill = Axis?.TitleStyle.Color ?? string.Empty;
                        AxisRenderInfo.AxisTitleOption.FontSize = Axis?.TitleStyle.Size ?? string.Empty;
                        AxisRenderInfo.AxisTitleOption.FontFamily = Axis?.TitleStyle.FontFamily ?? string.Empty;
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Applies the current chart theme colors to all axis grid lines, circle grid options, axis labels,
        /// and the axis title, then marks the renderer as dirty for the next render pass.
        /// </summary>
        internal void OnThemeChange()
        {
            RendererShouldRender = true;
            foreach (KeyValuePair<string, List<PathOptions>> keyValue in AxisRenderInfo.AxisGridOptions)
            {
                if (AxisRenderInfo.AxisGridOptions.TryGetValue(keyValue.Key, out List<PathOptions>? axisGridOptions))
                {
                    string themeColor = AxisThemeColor(keyValue.Key);
                    axisGridOptions.ForEach(option => option.Stroke = themeColor);
                }
            }

            foreach (CircleOptions circle in AxisRenderInfo.MajorGridCircleOptions)
            {
                circle.Stroke = Chart?._chartThemeStyle?.MajorGridLine ?? string.Empty;
            }

            foreach (TextOptions text in AxisRenderInfo.AxisLabelOptions)
            {
                text.Fill = Chart?._chartThemeStyle?.AxisLabel ?? string.Empty;
            }

            if (AxisRenderInfo.AxisTitleOption is { })
            {
                AxisRenderInfo.AxisTitleOption.Fill = Chart?._chartThemeStyle?.AxisTitle ?? string.Empty;
            }
        }

        /// <summary>
        /// Clears all cached axis rendering information, including grid options, label options,
        /// title, axis line, border, circle options, and label template options.
        /// </summary>
        internal void ClearAxisInfo()
        {
            AxisRenderInfo.AxisGridOptions.Clear();
            AxisRenderInfo.AxisLabelOptions.Clear();
            AxisRenderInfo.AxisTitleOption = null;
            AxisRenderInfo.AxisLine = null;
            AxisRenderInfo.AxisBorder = null;
            AxisRenderInfo.MajorGridCircleOptions.Clear();
            AxisRenderInfo.LabelTemplateOptions.Clear();
        }

        /// <summary>
        /// Renders the polar or radar axis grid lines and circular grid options into the provided render tree.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to build the SVG render tree.</param>
        internal void RenderPolarRadarAxisInsideCollection(RenderTreeBuilder builder)
        {
            SvgRenderer?.OpenGroupElement(builder, Chart?.ID + "AxisGroup" + Index);
            foreach (CircleOptions option in AxisRenderInfo.MajorGridCircleOptions)
            {
                SvgRenderer?.RenderCircle(builder, option);
            }

            foreach (KeyValuePair<string, List<PathOptions>> keyValue in AxisRenderInfo.AxisGridOptions)
            {
                if (keyValue.Key.Equals(Constants.MajorGridLine, StringComparison.Ordinal) || keyValue.Key.Equals(Constants.MinorGridLine, StringComparison.Ordinal))
                {
                    DrawLine(builder, keyValue.Value);
                }
            }

            builder.CloseElement();
        }

        /// <summary>
        /// Renders a collection of path elements as SVG lines into the provided render tree.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to build the SVG render tree.</param>
        /// <param name="pathOptionCollection">The collection of <see cref="PathOptions"/> to render.</param>
        internal void DrawLine(RenderTreeBuilder builder, List<PathOptions> pathOptionCollection)
        {
            foreach (PathOptions axisLineOption in pathOptionCollection.ToArray())
            {
                SvgRenderer?.RenderPath(builder, axisLineOption.Id, axisLineOption.Direction, axisLineOption.StrokeDashArray, axisLineOption.StrokeWidth, axisLineOption.Stroke);
            }
        }

        /// <summary>
        /// Renders a single path element as an SVG line into the provided render tree.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to build the SVG render tree.</param>
        /// <param name="axisLineOption">The <see cref="PathOptions"/> describing the line to render.</param>
        internal void DrawLine(RenderTreeBuilder builder, PathOptions axisLineOption)
        {
            SvgRenderer?.RenderPath(builder, axisLineOption.Id, axisLineOption.Direction, axisLineOption.StrokeDashArray, axisLineOption.StrokeWidth, axisLineOption.Stroke);
        }

        /// <summary>
        /// Returns the point value for the given input without transformation.
        /// Derived renderers override this method to provide type-specific value conversion.
        /// </summary>
        /// <param name="x">The raw point value.</param>
        /// <returns>The point value unchanged.</returns>
        internal virtual double GetPointValue(double x)
        {
            return x;
        }

        /// <summary>
        /// Converts an <see cref="object"/> to a <see cref="double"/> using culture-invariant conversion.
        /// Derived renderers override this method to provide type-specific conversion logic.
        /// </summary>
        /// <param name="x">The object value to convert.</param>
        /// <returns>A <see cref="double"/> representation of the input value.</returns>
        internal virtual double GetDoubleValue(object x)
        {
            return Convert.ToDouble(x, null);
        }

        /// <summary>
        /// Indicates whether this renderer is the default (base) axis renderer type.
        /// Derived renderers override this to return <c>true</c> when appropriate.
        /// </summary>
        /// <returns><c>false</c> in the base implementation; derived types may return <c>true</c>.</returns>
        internal virtual bool IsDefaultRenderer()
        {
            return false;
        }

        /// <summary>
        /// Returns the axis data representation for the specified point value.
        /// Derived renderers override this to return type-specific representations such as
        /// category labels or <see cref="DateTime"/> values.
        /// </summary>
        /// <param name="pointValue">The numeric point value to convert.</param>
        /// <returns>The point value as an <see cref="object"/>; defaults to the raw double value.</returns>
        internal virtual object GetAxisData(double pointValue)
        {
            return pointValue;
        }

        /// <summary>
        /// Indicates whether this axis renderer represents a category-type axis.
        /// Derived category axis renderers override this to return <c>true</c>.
        /// </summary>
        /// <returns><c>false</c> in the base implementation.</returns>
        internal virtual bool IsCategory()
        {
            return false;
        }

        /// <summary>
        /// Returns a formatted text representation of the specified point value, applying the
        /// <see cref="DateFormat"/> when set and the value can be converted to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="pointValue">The point value to format.</param>
        /// <returns>
        /// A date-formatted string when <see cref="DateFormat"/> is set; otherwise, the original
        /// <paramref name="pointValue"/> object.
        /// </returns>
        internal virtual object GetFormatText(object pointValue)
        {
            return !string.IsNullOrEmpty(DateFormat) ? Intl.GetDateFormat(Convert.ToDateTime(Convert.ToString(pointValue, Culture), Culture), DateFormat) : pointValue;
        }

        /// <summary>
        /// Synchronizes the <see cref="ChartAxis.IsAxisInverse"/> and <see cref="ChartAxis.IsAxisOpposedPosition"/>
        /// flags on the associated axis, taking RTL mode and orientation into account.
        /// </summary>
        internal void SetInverseAndOpposedPosition()
        {
            if (Axis is not null)
            {
                Axis.IsAxisInverse = Axis.IsInversed || (Chart is not null && Chart.EnableRtl && Orientation == Orientation.Horizontal);
                Axis.IsAxisOpposedPosition = Chart is not null && Chart.EnableRtl && Axis.OpposedPosition ? !Axis.OpposedPosition : Axis.OpposedPosition ? Axis.OpposedPosition : (Chart is not null && Chart.EnableRtl && Orientation == Orientation.Vertical);
            }
        }

        /// <summary>
        /// Applies adaptive rendering overrides to axis properties such as label intersect action,
        /// edge label placement, tick sizes, label rotation, and scrollbar visibility based on
        /// the current chart size category.
        /// </summary>
        /// <param name="axis">The <see cref="ChartAxis"/> whose settings are used as the base values.</param>
        /// <param name="orientation">The orientation of the axis being configured.</param>
        internal void SetAdaptiveAxisValues(ChartAxis axis, Orientation orientation)
        {
            LabelIntersectAction = axis.LabelIntersectAction;
            EdgeLabelPlacement = axis.EdgeLabelPlacement;
            LabelPosition = axis.LabelPosition;
            MajorTickLinesHeight = axis.MajorTickLines.Height;
            MajorTickLinesWidth = axis.MajorTickLines.Width;
            LabelRotation = axis.LabelRotation;
            ShouldRenderScrollbar = true;
            MajorGridLinesWidth = axis.MajorGridLines.Width;

            if (Chart is not null && Chart.EnableAdaptiveRendering)
            {
                ChartWidthCategory widthCategory = Chart._widthCategory;
                ChartHeightCategory heightCategory = Chart._heightCategory;
                EdgeLabelPlacement = EdgeLabelPlacement.Shift;
                MajorGridLinesWidth = (Chart._widthCategory == ChartWidthCategory.Small || Chart._heightCategory == ChartHeightCategory.Small) ? 0 : MajorGridLinesWidth;

                if (orientation == Orientation.Horizontal)
                {
                    LabelIntersectAction = LabelIntersectAction.Rotate45;
                    if (Chart.AvailableSize.Height <= 100 || Chart.AvailableSize.Width <= 100)
                    {
                        MajorTickLinesHeight = MajorTickLinesWidth = 0;
                    }
                    else if (heightCategory == ChartHeightCategory.Small)
                    {
                        LabelPosition = AxisPosition.Inside;
                        MajorTickLinesHeight = MajorTickLinesWidth = 0;
                        LabelIntersectAction = LabelIntersectAction.Rotate90;
                    }
                    if (axis.ValueType == ValueType.Category && (widthCategory == ChartWidthCategory.Small || widthCategory == ChartWidthCategory.Medium))
                    {
                        LabelRotation = -45;
                        LabelIntersectAction = LabelIntersectAction.Trim;
                    }
                    ShouldRenderScrollbar = !(heightCategory == ChartHeightCategory.Small || heightCategory == ChartHeightCategory.Medium || widthCategory == ChartWidthCategory.Small);
                }
                else
                {
                    LabelIntersectAction = LabelIntersectAction.Hide;
                    if (Chart.AvailableSize.Width <= 100)
                    {
                        MajorTickLinesHeight = MajorTickLinesWidth = 0;
                    }
                    else if (widthCategory == ChartWidthCategory.Small)
                    {
                        LabelPosition = AxisPosition.Inside;
                        MajorTickLinesHeight = MajorTickLinesWidth = 0;

                    }
                    ShouldRenderScrollbar = !(widthCategory == ChartWidthCategory.Small || widthCategory == ChartWidthCategory.Medium || heightCategory == ChartHeightCategory.Small);
                }
            }
        }

        /// <summary>
        /// Updates the SVG text element identified by the label option's <c>Id</c> when its
        /// rendered text differs from the option's current text value.
        /// </summary>
        /// <param name="option">The <see cref="TextOptions"/> containing the target element id and new text.</param>
        internal void ChangeAxisLabelText(TextOptions option)
        {
            SvgText? axisLabelElement = SvgRenderer?.TextElementList?.FirstOrDefault(item => item.Id == option.Id && item.Text != option.Text);
            axisLabelElement?.ChangeText(option.Text);
        }

        /// <summary>
        /// Computes the updated cross-axis intersection rectangle (<see cref="UpdatedRect"/>) for this axis
        /// by projecting the <see cref="CrossAt"/> value onto the cross-axis coordinate space.
        /// When no cross-at value is configured or the value lies outside the visible range,
        /// <see cref="UpdatedRect"/> is set equal to <see cref="Rect"/>.
        /// </summary>
        internal void UpdateCrossValue()
        {
            if (Axis?.CrossesAt is null)
            {
                UpdatedRect = Rect;
                return;
            }

            CrossAt = UpdateCrossAt(CrossInAxis ?? null!, Axis.CrossesAt);
            if (double.IsNaN(CrossAt) || (CrossInAxis?.Renderer is not null && !IsInside(CrossInAxis.Renderer.VisibleRange, CrossAt)))
            {
                UpdatedRect = Rect;
                return;
            }

            double crossValue = CrossAt;

            DoubleRange range = new();
            if (CrossInAxis?.Renderer is not null)
            {
                range = CrossInAxis.Renderer.VisibleRange;
            }

            if (!Axis.IsAxisOpposedPosition && CrossAt > range.End)
            {
                crossValue = range.End;
            }
            else if (CrossAt < range.Start)
            {
                crossValue = range.Start;
            }

            UpdatedRect = new Rect() { X = Rect.X, Y = Rect.Y, Height = Rect.Height, Width = Rect.Width };
            if (Orientation == Orientation.Horizontal)
            {
                crossValue = (CrossInAxis?.Renderer?.Rect.Height ?? 0) - (ChartHelper.ValueToCoefficient(crossValue, CrossInAxis?.Renderer ?? null!) * (CrossInAxis?.Renderer?.Rect.Height ?? 0));
                UpdatedRect.Y = (CrossInAxis?.Renderer?.Rect.Y ?? 0) + crossValue;
            }
            else
            {
                crossValue = ChartHelper.ValueToCoefficient(crossValue, CrossInAxis?.Renderer ?? null!) * (CrossInAxis?.Renderer?.Rect.Width ?? 0);
                UpdatedRect.X = (CrossInAxis?.Renderer?.Rect.X ?? 0) + crossValue;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles layout changes for the axis.
        /// </summary>
        public void HandleLayoutChange()
        {
        }

        /// <summary>
        /// Handles changes to the chart size and updates the axis rectangle.
        /// </summary>
        /// <param name="rect">The new available rectangle for the chart.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = true;
            if (_availableRect != rect)
            {
                _availableRect = rect;
            }
        }

        /// <summary>
        /// Requests a render refresh for this axis.
        /// </summary>
        public void InvalidateRender()
        {
            StateHasChanged();
        }

        /// <summary>
        /// Schedules an asynchronous re-render for this axis renderer and propagates the render
        /// request to the associated <see cref="OutSideRenderer"/> when present.
        /// </summary>
        public override void ProcessRenderQueue()
        {
            _ = InvokeAsync(StateHasChanged);
            OutSideRenderer?.ProcessRenderQueue();
        }

        #endregion
    }
}
