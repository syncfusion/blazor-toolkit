using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders numeric axis logic for the chart. Provides numeric range padding, visible label generation, formatting and other numeric axis related computations.
    /// </summary>
    /// <remarks>
    /// This renderer handles standard numeric axes with support for various range padding modes
    /// and automatic interval calculation based on the data range.
    /// </remarks>
    public class NumericAxisRenderer : ChartAxisRenderer
    {
        #region Private Methods

        /// <summary>
        /// Computes additional padded range when ChartRangePadding is Additional or Round.
        /// </summary>
        /// <param name="start">The start value of the range.</param>
        /// <param name="end">The end value of the range.</param>
        /// <param name="interval">The calculated interval for the axis.</param>
        /// <returns>A DoubleRange with the computed minimum and maximum values.</returns>
        private DoubleRange FindAdditional(double start, double end, double interval)
        {
            double minimum = start;
            double maximum = end;
            if (Axis?.Minimum is null)
            {
                minimum = Math.Floor(start / interval) * interval;
                if (Axis?.RangePadding == ChartRangePadding.Additional)
                {
                    minimum -= interval;
                }
            }

            if (Axis?.Maximum is null)
            {
                maximum = Math.Ceiling(end / interval) * interval;
                if (Axis?.RangePadding == ChartRangePadding.Additional)
                {
                    maximum += interval;
                }
            }

            return new DoubleRange(minimum, maximum);
        }

        /// <summary>
        /// Computes normal padded range (Auto/Normal) for numeric axes.
        /// </summary>
        /// <param name="start">The start value of the range.</param>
        /// <param name="end">The end value of the range.</param>
        /// <param name="interval">The calculated interval for the axis.</param>
        /// <returns>A DoubleRange with the computed minimum and maximum values.</returns>
        private DoubleRange FindNormal(double start, double end, double interval)
        {
            double minimum = start;
            double maximum = end;

            if (start < 0 && end < 0)
            {
                minimum = end;
                maximum = start;
            }

            double startValue = start;

            if (Axis?.Minimum is null)
            {
                minimum = CalculateMinimumBound(start, end, interval, startValue);
            }

            double max = CalculateMaximumBound(startValue, end, interval);

            if (Axis?.Maximum is null)
            {
                maximum = max;
            }

            if (Axis?.Minimum is null && minimum == 0)
            {
                if (ChartHelper.IsNaNOrZero(Axis?.Interval ?? 0))
                {
                    interval = CalculateNumericNiceInterval(max - minimum);
                }

                if (Axis?.Maximum is null)
                {
                    maximum = Math.Ceiling(max / interval) * interval;
                }
            }

            VisibleInterval = ActualInterval = interval;
            return new DoubleRange(minimum, maximum);
        }

        /// <summary>
        /// Calculates the minimum bound for normal range padding.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="end">The end value.</param>
        /// <param name="interval">The interval value.</param>
        /// <param name="startValue">The original start value.</param>
        /// <returns>The calculated minimum bound.</returns>
        private static double CalculateMinimumBound(double start, double end, double interval, double startValue)
        {
            double minimum;

            if (start < 0)
            {
                _ = (start < 0 && end < 0) ? startValue : 0;
                minimum = start + ((start - end) * 0.05);

                if ((0.365 * interval) >= (interval + (minimum % interval)))
                {
                    minimum -= interval;
                }

                if (minimum % interval < 0)
                {
                    minimum = Convert.ToDouble(minimum - interval - (minimum % interval));
                }
            }
            else
            {
                minimum = start < (5.0 / 6.0 * end) ? 0 : (start - ((end - start) * 0.5));

                if (minimum % interval > 0)
                {
                    minimum -= minimum % interval;
                }
            }

            return minimum;
        }

        /// <summary>
        /// Calculates the maximum bound for normal range padding.
        /// </summary>
        /// <param name="startValue">The start value.</param>
        /// <param name="end">The end value.</param>
        /// <param name="interval">The interval value.</param>
        /// <returns>The calculated maximum bound.</returns>
        private static double CalculateMaximumBound(double startValue, double end, double interval)
        {
            double max = end > 0 ? end + ((end - startValue) * 0.05) : end - ((startValue - end) * 0.05);

            if ((0.365 * interval) >= (interval - (max % interval)))
            {
                max += interval;
            }

            if (max % interval > 0)
            {
                max = max + interval - (max % interval);
            }

            if (max % interval < 0)
            {
                max -= max % interval;
            }

            return max;
        }

        /// <summary>
        /// Processes visible labels for exponential values with proper parsing.
        /// </summary>
        private void ProcessVisibleLabelsExponential()
        {
            double start = double.Parse(VisibleRange.Start.ToString(Culture),
                NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, Culture);
            double end = double.Parse(VisibleRange.End.ToString(Culture),
                NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, Culture);

            ProcessVisibleLabels(start, end);
        }

        /// <summary>
        /// Gets the number of format digits from the label format.
        /// </summary>
        /// <returns>The number of format digits.</returns>
        private int GetFormatDigits()
        {
            int formatDigits = 0;
            if (!string.IsNullOrEmpty(Axis?.LabelFormat) && Axis.LabelFormat.ToLower(Culture).Contains('n', StringComparison.InvariantCulture))
            {
                _ = int.TryParse(Axis.LabelFormat.AsSpan(1, Axis.LabelFormat.Length - 1), out formatDigits);
            }
            return formatDigits;
        }

        /// <summary>
        /// Gets the number of decimal digits in the interval.
        /// </summary>
        /// <returns>The number of decimal digits.</returns>
        private int GetIntervalDigits()
        {
            int intervalDigits = 0;
            if (VisibleInterval > 0 && VisibleInterval.ToString(Culture).Contains('.', StringComparison.InvariantCulture))
            {
                intervalDigits = VisibleInterval.ToString(Culture).Split('.')[1].Length;
            }
            return intervalDigits;
        }

        /// <summary>
        /// Generates standard numeric labels.
        /// </summary>
        private void GenerateStandardLabels(double startInterval, int formatDigits, int intervalDigits, double start, double end, double visibleInterval)
        {
            double tempInterval = startInterval;

            while (tempInterval <= end)
            {
                if (ChartHelper.WithIn(tempInterval, VisibleRange))
                {
                    TriggerLabelRender(tempInterval, FormatValue(tempInterval));
                }

                tempInterval += visibleInterval;
                if (start == tempInterval)
                {
                    tempInterval = end;
                    break;
                }
            }

            string tempString = tempInterval.ToString(Culture);
            bool isTooLarge = false;
            try
            {
                decimal decimalValue = Convert.ToDecimal(tempInterval);
            }
            catch (OverflowException)
            {
                isTooLarge = true;
            }
            if (!double.IsNaN(tempInterval) && tempInterval > 0 && tempString.Contains('.', StringComparison.InvariantCulture) && tempString.Split('.')[1].Length > 10 && !isTooLarge)
            {
                decimal value = tempString.Split('.')[1].Length > Math.Max(formatDigits, intervalDigits) ? Math.Round(Convert.ToDecimal(tempInterval), Math.Max(formatDigits, intervalDigits)) : Convert.ToDecimal(tempInterval);
                tempInterval = Convert.ToDouble(value);
                if (tempInterval <= end)
                {
                    TriggerLabelRender(tempInterval, FormatValue(tempInterval));
                }
            }
            else if (isTooLarge)
            {
                if (Axis is { })
                {
                    Axis.IsAxisLabelTrim = true;
                }
                if (tempInterval <= end)
                {
                    TriggerLabelRender(tempInterval, FormatValue(tempInterval));
                }
            }
        }

        #endregion

        /// <summary>
        /// Applies range padding depending on axis type and configuration.
        /// </summary>
        /// <param name="range">The initial range to apply padding to.</param>
        /// <param name="interval">The calculated interval for the axis.</param>
        /// <returns>The range with appropriate padding applied.</returns>
        protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
        {
            double min = range.Start;
            double max = range.End;
            if (Axis is not null && !Axis.StartFromZero && IsColumn > 0)
            {
                max += interval;
                min = ((min - interval) < 0 && min > 0) ? 0 : min - interval;
            }

            if (Axis is not null && !ChartHelper.SetRange(Axis))
            {
                ChartRangePadding padding = GetRangePadding;
                if (padding is ChartRangePadding.Additional or ChartRangePadding.Round)
                {
                    return FindAdditional(min, max, interval);
                }
                else if (padding is ChartRangePadding.Normal)
                {
                    return FindNormal(min, max, interval);
                }
            }

            return range;
        }

        #region Internal Methods

        /// <summary>
        /// Calculates actual interval for numeric axis.
        /// </summary>
        /// <param name="range">The range to calculate interval for.</param>
        /// <returns>The calculated interval value.</returns>
        internal override double CalculateActualInterval(DoubleRange range)
        {
            return Axis is not null && !ChartHelper.IsNaNOrZero(Axis.Interval) ? Axis.Interval : CalculateNumericNiceInterval(range.Delta);
        }

        /// <summary>
        /// Generates visible labels for numeric axis; handles exponential notation safely.
        /// </summary>
        internal override void GenerateVisibleLabels()
        {
            VisibleLabels = [];
            if (!VisibleInterval.ToString(Culture).ToLower(Culture).Contains('e', StringComparison.InvariantCulture))
            {
                ProcessVisibleLabels();
            }
            else
            {
                ProcessVisibleLabelsExponential();
            }
            GetMaxLabelWidth();
            base.GenerateVisibleLabels();
        }

        /// <summary>
        /// Processes visible labels for exponential-range parsing.
        /// </summary>
        internal void ProcessVisibleLabels(double start, double end)
        {
            double tempInterval = start;
            double visibleInterval = VisibleInterval;

            if (Axis?.ZoomFactor < 1 || Axis?.ZoomPosition > 0 || PaddingInterval > 0)
            {
                tempInterval = start - (start % visibleInterval);
            }

            int formatDigits = GetFormatDigits();
            int intervalDigits = GetIntervalDigits();

            GenerateStandardLabels(tempInterval, formatDigits, intervalDigits, start, end, visibleInterval);
        }

        /// <summary>
        /// Processes visible labels for standard numeric ranges.
        /// </summary>
        internal void ProcessVisibleLabels()
        {
            double tempInterval = VisibleRange.Start;

            if (Axis?.ZoomFactor < 1 || Axis?.ZoomPosition > 0 || PaddingInterval > 0)
            {
                tempInterval = VisibleRange.Start - (VisibleRange.Start % VisibleInterval);
            }

            int intervalDigits = 0, formatDigits = 0;
            if (!string.IsNullOrEmpty(Axis?.LabelFormat) && Axis.LabelFormat.ToLower(Culture).Contains('n', StringComparison.InvariantCulture))
            {
                _ = int.TryParse(Axis.LabelFormat.AsSpan(1, Axis.LabelFormat.Length - 1), out formatDigits);
            }

            if (VisibleInterval > 0 && VisibleInterval.ToString(Culture).Contains('.', StringComparison.InvariantCulture))
            {
                intervalDigits = VisibleInterval.ToString(Culture).Split('.')[1].Length;
            }

            while (tempInterval <= VisibleRange.End && VisibleInterval != 0)
            {
                if (ChartHelper.WithIn(tempInterval, VisibleRange))
                {
                    TriggerLabelRender(tempInterval, FormatValue(tempInterval));
                }

                tempInterval += VisibleInterval;
            }

            string tempString = tempInterval.ToString(Culture);
            if (!double.IsNaN(tempInterval) && tempInterval > 0 && tempString.Contains('.', StringComparison.InvariantCulture) && tempString.Split('.')[1].Length > 10)
            {
                tempInterval = tempString.Split('.')[1].Length > Math.Max(formatDigits, intervalDigits) ? Math.Round(tempInterval, Math.Max(formatDigits, intervalDigits)) : tempInterval;
                if (tempInterval <= VisibleRange.End)
                {
                    TriggerLabelRender(tempInterval, FormatValue(tempInterval));
                }
            }
        }

        /// <summary>
        /// Returns the formatted text for an axis value.
        /// </summary>
        /// <param name="pointValue">The point value to format.</param>
        /// <returns>The formatted text representation of the value.</returns>
        internal override object GetFormatText(object pointValue)
        {
            return FormatValue(Convert.ToDouble(pointValue, Culture));
        }
        #endregion
    }

    /// <summary>
    /// Default primary X axis renderer for numeric axes.
    /// </summary>
    public class PrimaryXAxisRenderer : NumericAxisRenderer
    {
        /// <summary>
        /// Determines whether this is the default renderer.
        /// </summary>
        /// <returns><see langword="true"/> if this is the default renderer; otherwise, <see langword="false"/>.</returns>
        internal override bool IsDefaultRenderer()
        {
            return true;
        }
    }

    /// <summary>
    /// Default primary Y axis renderer for numeric axes.
    /// </summary>
    public class PrimaryYAxisRenderer : NumericAxisRenderer
    {
        /// <summary>
        /// Determines whether this is the default renderer.
        /// </summary>
        /// <returns><see langword="true"/> if this is the default renderer; otherwise, <see langword="false"/>.</returns>
        internal override bool IsDefaultRenderer()
        {
            return true;
        }
    }

    /// <summary>
    /// Renderer for Pareto chart axes with numeric scaling.
    /// </summary>
    /// <remarks>
    /// Applies numeric axis rendering logic to Pareto charts, inheriting interval calculation and label generation from the numeric renderer.
    /// </remarks>
    public class ParetoAxisRenderer : NumericAxisRenderer
    {
        /// <summary>
        /// Determines whether this renderer is the default for Pareto chart axes.
        /// </summary>
        /// <returns><see langword="true"/> as this is the default numeric axis renderer for Pareto charts; otherwise <see langword="false"/>.</returns>
        internal override bool IsDefaultRenderer()
        {
            return true;
        }
    }
}
