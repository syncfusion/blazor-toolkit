
namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Base renderer for date/time axes. Provides helpers to compute time intervals and skeleton formats.
    /// </summary>
    public class DateTimeBase : ChartAxisRenderer
    {
        #region Constants
        private const double MILLIS_PER_DAY = 86400000;
        #endregion

        #region Fields
        private DateTime _startDate = new(1970, 1, 1);
        #endregion

        #region Private Methods

        /// <summary>
        /// Computes absolute number of days between two internal millisecond values.
        /// </summary>
        /// <param name="start">Start in internal milliseconds.</param>
        /// <param name="end">End in internal milliseconds.</param>
        /// <returns>Absolute difference in days.</returns>
        private double ComputeTotalDays(double start, double end)
        {
            double startTime = GetTime(new DateTime(1970, 1, 1).AddMilliseconds(start));
            double endTime = GetTime(new DateTime(1970, 1, 1).AddMilliseconds(end));
            return Math.Abs((startTime - endTime) / MILLIS_PER_DAY);
        }

        /// <summary>
        /// Resolves the string used to determine RangeIntervalType taking Axis settings and flags into account.
        /// </summary>
        /// <param name="isDateOnly">Date-only flag.</param>
        /// <param name="isTimeOnly">Time-only flag.</param>
        /// <returns>Interval type string suitable to parse into RangeIntervalType.</returns>
        private string ResolveIntervalTypeString(bool isDateOnly, bool isTimeOnly)
        {
            string intervalType = ((IsDateOnly || isDateOnly) && (Axis?.IntervalType == IntervalType.Hours || Axis?.IntervalType == IntervalType.Minutes || Axis?.IntervalType == IntervalType.Seconds)) ||
                            ((IsTimeOnly || isTimeOnly) && (Axis?.IntervalType == IntervalType.Years || Axis?.IntervalType == IntervalType.Months || Axis?.IntervalType == IntervalType.Days)) ? "Auto"
                            : Axis?.AxisIntervalType ?? null!;

            intervalType = string.IsNullOrEmpty(intervalType) ? Axis?.IntervalType.ToString() ?? string.Empty : intervalType;
            return intervalType;
        }

        /// <summary>
        /// Maps RangeIntervalType to a numeric nice interval; sets ActualIntervalType when required.
        /// </summary>
        /// <param name="rangeType">Range interval type to resolve.</param>
        /// <param name="totalDays">Total span in days.</param>
        /// <param name="isChart">If false, certain rounding choices differ (used by some callers).</param>
        /// <returns>Computed interval value for the resolved interval type.</returns>
        private double ResolveIntervalByRangeType(RangeIntervalType rangeType, double totalDays, bool isChart)
        {
            double interval = 0;
            switch (rangeType)
            {
                case RangeIntervalType.Years:
                    interval = CalculateNumericNiceInterval(totalDays / 365);
                    break;
                case RangeIntervalType.Quarter:
                    interval = CalculateNumericNiceInterval(totalDays / 365 * 4);
                    break;
                case RangeIntervalType.Months:
                    interval = CalculateNumericNiceInterval(totalDays / 30);
                    break;
                case RangeIntervalType.Weeks:
                    interval = CalculateNumericNiceInterval(totalDays / 7);
                    break;
                case RangeIntervalType.Days:
                    interval = CalculateNumericNiceInterval(totalDays);
                    break;
                case RangeIntervalType.Hours:
                    interval = CalculateNumericNiceInterval(totalDays * 24);
                    break;
                case RangeIntervalType.Minutes:
                    interval = CalculateNumericNiceInterval(totalDays * 24 * 60);
                    break;
                case RangeIntervalType.Seconds:
                    interval = CalculateNumericNiceInterval(totalDays * 24 * 60 * 60);
                    break;
                case RangeIntervalType.Auto:
                    return ResolveAutoInterval(totalDays, isChart);
                default:
                    break;
            }

            return interval;
        }

        /// <summary>
        /// Tries successive interval granularities and selects the first reasonable one; updates ActualIntervalType.
        /// </summary>
        /// <param name="totalDays">Total span in days.</param>
        /// <param name="isChart">Chart context flag.</param>
        /// <returns>Computed interval for the chosen granularity.</returns>
        private double ResolveAutoInterval(double totalDays, bool isChart)
        {
            double interval;

            interval = CalculateNumericNiceInterval(totalDays / 365);
            if (interval >= 1)
            {
                ActualIntervalType = IntervalType.Years;
                if (Axis is { })
                {
                    Axis.AxisActualIntervalType = ActualIntervalType.ToString();
                }
                return interval;
            }

            interval = CalculateNumericNiceInterval(totalDays / 30);
            if (interval >= 1)
            {
                ActualIntervalType = IntervalType.Months;
                if (Axis is { })
                {
                    Axis.AxisActualIntervalType = ActualIntervalType.ToString();
                }
                return interval;
            }

            interval = CalculateNumericNiceInterval(totalDays / 7);
            if (interval >= 1 && !isChart)
            {
                ActualIntervalType = IntervalType.Days;
                if (Axis is { })
                {
                    Axis.AxisActualIntervalType = ActualIntervalType.ToString();
                }
                return interval;
            }

            interval = CalculateNumericNiceInterval(totalDays);
            if (interval >= 1)
            {
                ActualIntervalType = IntervalType.Days;
                if (Axis is { })
                {
                    Axis.AxisActualIntervalType = ActualIntervalType.ToString();
                }
                return interval;
            }

            interval = CalculateNumericNiceInterval(totalDays * 24);
            if (interval >= 1)
            {
                ActualIntervalType = IntervalType.Hours;
                if (Axis is { })
                {
                    Axis.AxisActualIntervalType = ActualIntervalType.ToString();
                }
                return interval;
            }

            interval = CalculateNumericNiceInterval(totalDays * 24 * 60);
            if (interval >= 1)
            {
                ActualIntervalType = IntervalType.Minutes;
                if (Axis is { })
                {
                    Axis.AxisActualIntervalType = ActualIntervalType.ToString();
                }
                return interval;
            }

            interval = CalculateNumericNiceInterval(totalDays * 24 * 60 * 60);
            ActualIntervalType = IntervalType.Seconds;

            if (Axis is { })
            {
                Axis.AxisActualIntervalType = ActualIntervalType.ToString();
            }
            return interval;
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Calculates a "nice" interval for date/time ranges. Sets ActualIntervalType where applicable.
        /// </summary>
        /// <param name="start">Start value in internal milliseconds.</param>
        /// <param name="end">End value in internal milliseconds.</param>
        /// <param name="isDateOnly">If set, treats data as date-only.</param>
        /// <param name="isTimeOnly">If set, treats data as time-only.</param>
        /// <param name="isChart">Indicates whether calculation is for a chart context.</param>
        /// <returns>Computed interval as numeric units corresponding to ActualIntervalType.</returns>
        protected double CalculateDateTimeNiceInterval(double start, double end, bool isDateOnly = false, bool isTimeOnly = false, bool isChart = true)
        {
            double totalDays = ComputeTotalDays(start, end);
            string intervalTypeString = ResolveIntervalTypeString(isDateOnly, isTimeOnly);

            if (Axis is not null)
            {
                ActualIntervalType = Axis.IntervalType;
            }

            RangeIntervalType resolved = Enum.Parse<RangeIntervalType>(intervalTypeString);
            double interval = ResolveIntervalByRangeType(resolved, totalDays, isChart);

            if (Axis is { })
            {
                Axis.AxisActualIntervalType = ActualIntervalType.ToString();
            }
            return interval;
        }

        /// <summary>
        /// Returns a skeleton format string based on the axis actual interval type and configured format.
        /// </summary>
        /// <returns>Skeleton format string.</returns>
        protected string GetSkeleton()
        {
            RangeIntervalType intervalType = Enum.Parse<RangeIntervalType>(Axis?.AxisActualIntervalType ?? null!);

            return !string.IsNullOrEmpty(Axis?.Format)
                ? Axis.Format
                : intervalType switch
                {
                    RangeIntervalType.Years or RangeIntervalType.Quarter => "y",
                    RangeIntervalType.Months or RangeIntervalType.Weeks => "m",
                    RangeIntervalType.Days => "d",
                    RangeIntervalType.Hours => "t",
                    RangeIntervalType.Auto => "T",
                    RangeIntervalType.Minutes => "T",
                    RangeIntervalType.Seconds => "T",
                    _ => "T"
                };
        }

        /// <summary>
        /// Finds the appropriate custom format for datetime axis labels based on interval and configuration.
        /// </summary>
        /// <returns>Custom label format string or skeleton when empty.</returns>
        protected string FindCustomFormats()
        {
            bool isAdaptiveLabels = Chart is not null && Chart.EnableAdaptiveRendering && Chart._widthCategory != ChartWidthCategory.Normal;
            string labelFormat = !string.IsNullOrEmpty(Axis?.LabelFormat) && !isAdaptiveLabels ? Axis.LabelFormat : string.Empty;

            if (string.IsNullOrEmpty(Axis?.Format) && ActualIntervalType == IntervalType.Months && string.IsNullOrEmpty(labelFormat))
            {
                labelFormat = Axis?.ValueType == ValueType.DateTime ? "yyyy MMM" : "yMMM";
            }

            if (string.IsNullOrEmpty(labelFormat))
            {
                labelFormat = ActualIntervalType == IntervalType.Years ? (IsIntervalInDecimal ? "yyyy" : "MMM y") :
                     (ActualIntervalType == IntervalType.Days && !IsIntervalInDecimal) ? "ddd HH tt" : string.Empty;
            }

            return string.IsNullOrEmpty(labelFormat) ? GetSkeleton() : labelFormat;
        }
        #endregion

        /// <summary>
        /// Converts a DateTime to milliseconds since epoch used internally by renderers.
        /// </summary>
        /// <param name="current">The date/time to convert.</param>
        /// <returns>Milliseconds since the internal epoch (1970-01-01).</returns>
        internal double GetTime(DateTime current)
        {
            return (current - _startDate).TotalMilliseconds;
        }
    }
}