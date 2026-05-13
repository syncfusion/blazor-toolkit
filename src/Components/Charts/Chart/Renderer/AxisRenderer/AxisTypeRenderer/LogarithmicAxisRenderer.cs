namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renderer for logarithmic axis. Converts values to log-scale, computes intervals and visible labels accordingly.
    /// </summary>
    /// <remarks>
    /// This renderer transforms data values using logarithmic scale, supports custom log base,
    /// and automatically calculates appropriate intervals for the logarithmic scale.
    /// </remarks>
    public class LogarithmicAxisRenderer : ChartAxisRenderer
    {
        #region Internal Methods

        /// <summary>
        /// Initializes the double range for the logarithmic axis based on configured min/max and base.
        /// </summary>
        /// <returns>The initialized double range with logarithmic scaling applied.</returns>
        internal override DoubleRange InitializeDoubleRange()
        {
            Min = Axis?.Minimum is not null ? Convert.ToDouble(Axis.Minimum, null) : (double.IsNaN(Min) || double.IsPositiveInfinity(Min)) ? 0 : Min;
            Max = Axis?.Maximum is not null ? Convert.ToDouble(Axis.Maximum, null) : (double.IsNaN(Max) || double.IsNegativeInfinity(Max)) ? 5 : Max;

            if (Min == Max)
            {
                Max = Min + 1;
            }

            Min = Min < 0 ? 0 : Min;
            double logStart = ChartHelper.LogBase(Min, Axis?.LogBase ?? 10);
            logStart = double.IsFinite(logStart) ? logStart : Min;

            double logEnd = double.IsFinite(logStart) ? Max == 1 ? 1 : ChartHelper.LogBase(Max, Axis?.LogBase ?? 10) : Max;
            Min = Math.Floor(logStart / 1);
            Max = Math.Ceiling(logEnd / 1);

            return new DoubleRange(Min, Max);
        }

        /// <summary>
        /// Calculates the actual interval for logarithmic axis.
        /// </summary>
        /// <param name="range">The range to calculate interval for.</param>
        /// <returns>The calculated interval value.</returns>
        internal override double CalculateActualInterval(DoubleRange range)
        {
            return !ChartHelper.IsNaNOrZero(Axis?.Interval ?? 0) ? (Axis?.Interval ?? 0) : CalculateLogNiceInterval(Max - Min);
        }

        /// <summary>
        /// Calculates visible range on zoom and triggers range render event if present.
        /// </summary>
        /// <param name="actualRange">The actual range before zoom calculation.</param>
        /// <returns>The calculated visible range.</returns>
        internal override DoubleRange CalculateVisibleRange(DoubleRange actualRange)
        {
            if (Axis is not null && (Axis.ZoomFactor < 1 || Axis.ZoomPosition > 0))
            {
                actualRange = CalculateVisibleRangeOnZooming();
                if (Axis.EnableAutoIntervalOnZooming)
                {
                    double interval = CalculateLogNiceInterval(actualRange.Delta);
                    VisibleInterval = Math.Floor(interval) == 0 ? 1 : Math.Floor(interval);
                }
            }

            if (Chart?.OnAxisActualRangeCalculated is not null)
            {
                actualRange = TriggerRangeRender(actualRange);
            }

            return actualRange;
        }

        /// <summary>
        /// Generates visible labels using log base power conversion.
        /// </summary>
        internal override void GenerateVisibleLabels()
        {
            double tempInterval = VisibleRange.Start;
            VisibleLabels = [];
            if (Axis?.ZoomFactor < 1 || Axis?.ZoomPosition > 0)
            {
                tempInterval = VisibleRange.Start - (VisibleRange.Start % VisibleInterval);
            }

            while (tempInterval <= VisibleRange.End)
            {
                if (ChartHelper.WithIn(tempInterval, VisibleRange))
                {
                    TriggerLabelRender(tempInterval, FormatValue(Math.Pow(Axis?.LogBase ?? 10, tempInterval)));
                }

                tempInterval += VisibleInterval;
            }

            GetMaxLabelWidth();
            base.GenerateVisibleLabels();
        }

        /// <summary>
        /// Converts a data point to its log-scaled point value.
        /// </summary>
        /// <param name="x">The data value to convert.</param>
        /// <returns>The logarithmic value of the input.</returns>
        internal override double GetPointValue(double x)
        {
            return ChartHelper.LogBase((x > 0) ? x : 1, Axis?.LogBase ?? 10);
        }

        /// <summary>
        /// Converts axis double-value back to original axis scale.
        /// </summary>
        /// <param name="pointValue">The logarithmic value to convert.</param>
        /// <returns>The original scale value.</returns>
        internal override object GetAxisData(double pointValue)
        {
            return Math.Pow(Axis?.LogBase ?? 10, pointValue);
        }

        /// <summary>
        /// Formats axis value for display.
        /// </summary>
        /// <param name="pointValue">The point value to format.</param>
        /// <returns>The formatted text representation.</returns>
        internal override object GetFormatText(object pointValue)
        {
            return FormatValue(Convert.ToDouble(pointValue, Culture));
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates a nice interval value for logarithmic scale.
        /// </summary>
        /// <param name="delta">The range delta to calculate interval for.</param>
        /// <returns>The calculated nice interval.</returns>
        private double CalculateLogNiceInterval(double delta)
        {
            double niceInterval = delta;
            double minInterval = Math.Pow(10, Math.Floor(ChartHelper.LogBase(niceInterval, 10)));

            for (int j = 0, len = IntervalDivs.Length; j < len; j++)
            {
                double currentInterval = minInterval * IntervalDivs[j];
                if (ChartHelper.GetActualDesiredIntervalsCount(AxisAvailabelSize ?? null!, Axis?.DesiredIntervals ?? 0, Orientation, Axis?.MaximumLabels ?? 3) < (delta / currentInterval))
                {
                    break;
                }

                niceInterval = currentInterval;
            }
            return niceInterval;
        }
        #endregion
    }
}
