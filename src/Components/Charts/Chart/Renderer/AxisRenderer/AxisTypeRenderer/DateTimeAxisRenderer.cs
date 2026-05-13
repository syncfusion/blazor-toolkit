using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders a datetime axis with proper start/end alignment, interval calculations, and label generation.
    /// </summary>
    public class DateTimeAxisRenderer : DateTimeBase
    {
        #region Private Methods

        /// <summary>
        /// Adds decimal portion of interval to the supplied result DateTime depending on interval type.
        /// </summary>
        /// <param name="result">Starting DateTime to advance.</param>
        /// <param name="interval">Interval value; may contain fractional part.</param>
        /// <param name="intervalType">Type of the interval granularity.</param>
        /// <returns>Adjusted DateTime after applying decimal interval.</returns>
        private static DateTime GetDecimalInterval(DateTime result, double interval, RangeIntervalType intervalType)
        {
            double roundValue = Math.Floor(interval);
            double decimalValue = interval - roundValue;

            switch (intervalType)
            {
                case RangeIntervalType.Years:
                    result = result.AddYears((int)roundValue).AddMonths((int)Math.Round(12 * decimalValue));
                    return result;
                case RangeIntervalType.Quarter:
                    return result.AddMonths((int)(3 * interval));
                case RangeIntervalType.Months:
                    result = result.AddMonths((int)roundValue).AddDays(Math.Round(30 * decimalValue));
                    return result;
                case RangeIntervalType.Weeks:
                    return result.AddDays(interval * 7);
                case RangeIntervalType.Days:
                    result = result.AddDays(roundValue).AddHours(Math.Round(24 * decimalValue));
                    return result;
                case RangeIntervalType.Hours:
                    result = result.AddHours(roundValue).AddMinutes(Math.Round(60 * decimalValue));
                    return result;
                case RangeIntervalType.Minutes:
                    result = result.AddMinutes(roundValue).AddSeconds(Math.Round(60 * decimalValue));
                    return result;
                case RangeIntervalType.Seconds:
                    result = result.AddSeconds(roundValue).AddMilliseconds(Math.Round(1000 * decimalValue));
                    return result;
                case RangeIntervalType.Auto:
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// Converts axis-supplied value to internal millisecond representation.
        /// </summary>
        /// <param name="dateValue">Value supplied as object (string, long, DateTime).</param>
        /// <returns>Internal milliseconds value.</returns>
        private double ConvertAxisValueToTime(object dateValue)
        {
            return dateValue is string text
                ? long.TryParse(text, out long milliseconds)
                    ? GetTime(
                        DateTimeOffset
                            .FromUnixTimeMilliseconds(milliseconds)
                            .UtcDateTime
                    )
                    : GetTime(DateTime.Parse(text, Culture))
                : GetTime((DateTime)dateValue);
        }

        /// <summary>
        /// Computes Min/Max for year-based intervals with padding applied.
        /// </summary>
        private void GetYear(DateTime minimum, DateTime maximum, ChartRangePadding rangePadding, double interval)
        {
            if (rangePadding == ChartRangePadding.Additional)
            {
                Min = GetTime(new DateTime(minimum.Year - (int)interval, 1, 1, 0, 0, 0));
                Max = GetTime(new DateTime(maximum.Year + (int)interval, 1, 1, 0, 0, 0));
            }
            else
            {
                Min = GetTime(new DateTime(minimum.Year, 1, 1, 0, 0, 0));
                Max = GetTime(new DateTime(maximum.Year, 11, 30, 23, 59, 59));
            }
        }

        /// <summary>
        /// Computes Min/Max for month-based intervals with padding applied.
        /// </summary>
        private void GetMonth(DateTime minimum, DateTime maximum, ChartRangePadding rangePadding, double interval)
        {
            if (rangePadding == ChartRangePadding.Round)
            {
                Min = GetTime(new DateTime(minimum.Year, minimum.Month, 1, 0, 0, 0));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, new DateTime(maximum.Year, maximum.Month, 1).Day, 23, 59, 59));
            }
            else
            {
                int month = minimum.Month + (int)-interval, year = month > 0 ? minimum.Year : minimum.Year - 1;
                month = month <= 0 ? 12 + month : month;
                Min = GetTime(new DateTime(year, month, 1, 0, 0, 0));
                int maxmonth = maximum.Month + (int)interval, maxyear = maxmonth < 12 ? maximum.Year : maximum.Year + 1;
                maxmonth = maxmonth > 12 ? maxmonth - 12 : maxmonth;
                Max = GetTime(new DateTime(maxyear, maxmonth, maxmonth == 2 ? 28 : 30, 0, 0, 0));
            }
        }

        /// <summary>
        /// Computes Min/Max for day-based intervals with padding applied.
        /// </summary>
        private void GetDay(DateTime minimum, DateTime maximum, ChartRangePadding rangePadding, double interval)
        {
            if (rangePadding == ChartRangePadding.Round)
            {
                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, 0, 0, 0));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, minimum.Day, 23, 59, 59));
            }
            else
            {
                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, 0, 0, 0).AddDays((int)-interval));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, maximum.Day, 0, 0, 0).AddDays((int)interval));
            }
        }

        /// <summary>
        /// Computes Min/Max for hour-based intervals with padding applied.
        /// </summary>
        private void GetHour(DateTime minimum, DateTime maximum, ChartRangePadding rangePadding, double interval)
        {
            int hour = minimum.Hour / (int)interval * (int)interval;
            int endHour = maximum.Hour + (minimum.Hour - hour);

            if (rangePadding == ChartRangePadding.Round)
            {
                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, hour, 0, 0));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, maximum.Day, endHour, 59, 59));
            }
            else
            {
                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, hour + (int)-interval, 0, 0));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, maximum.Day, endHour + (int)interval, 0, 0));
            }
        }

        /// <summary>
        /// Dispatch helper that sets Min/Max based on the interval type.
        /// </summary>
        /// <param name="intervalType">Interval type of the axis.</param>
        /// <param name="minimum">Minimum DateTime.</param>
        /// <param name="maximum">Maximum DateTime.</param>
        /// <param name="rangePadding">Padding mode.</param>
        /// <param name="interval">Interval magnitude.</param>
        private void ApplyPaddingForInterval(IntervalType intervalType, DateTime minimum, DateTime maximum, ChartRangePadding rangePadding, double interval)
        {
            switch (intervalType)
            {
                case IntervalType.Years:
                    GetYear(minimum, maximum, rangePadding, interval);
                    break;
                case IntervalType.Months:
                    GetMonth(minimum, maximum, rangePadding, interval);
                    break;
                case IntervalType.Days:
                    GetDay(minimum, maximum, rangePadding, interval);
                    break;
                case IntervalType.Hours:
                    GetHour(minimum, maximum, rangePadding, interval);
                    break;
                case IntervalType.Minutes:
                    ApplyMinutePadding(minimum, maximum, rangePadding, interval);
                    break;
                case IntervalType.Seconds:
                    ApplySecondPadding(minimum, maximum, rangePadding, interval);
                    break;
                case IntervalType.Auto:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Computes Min/Max for minute-based intervals with padding applied.
        /// </summary>
        private void ApplyMinutePadding(DateTime minimum, DateTime maximum, ChartRangePadding rangePadding, double interval)
        {
            int minute = Convert.ToInt32(minimum.Minute / interval * interval);
            int endMinute = Convert.ToInt32(maximum.Minute + (minimum.Minute - minute));

            if (rangePadding == ChartRangePadding.Round)
            {
                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, minimum.Hour, minute, 0));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, minimum.Day, maximum.Hour, endMinute, 59));
            }
            else
            {
                Min = GetTime(new DateTime(minimum.Year, maximum.Month, minimum.Day, minimum.Hour, minute + (int)-interval, 0));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, maximum.Day, maximum.Hour, endMinute + (int)interval, 0));
            }
        }

        /// <summary>
        /// Computes Min/Max for second-based intervals with padding applied.
        /// </summary>
        private void ApplySecondPadding(DateTime minimum, DateTime maximum, ChartRangePadding rangePadding, double interval)
        {
            int second = Convert.ToInt32(minimum.Second / interval * interval);
            int endSecond = Convert.ToInt32(maximum.Second + (minimum.Second - second));

            if (rangePadding == ChartRangePadding.Round)
            {
                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, minimum.Hour, minimum.Minute, second, 0));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, maximum.Day, maximum.Hour, maximum.Minute, endSecond, 0));
            }
            else
            {
                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, minimum.Hour, minimum.Minute, second + (int)-interval, 0));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, maximum.Day, maximum.Hour, maximum.Minute, endSecond + (int)interval, 0));
            }
        }

        /// <summary>
        /// Aligns the start of a date range to the nearest interval boundary.
        /// </summary>
        /// <param name="startDate">Starting date in milliseconds since epoch.</param>
        /// <param name="intervalSize">Size of the interval for alignment.</param>
        /// <returns>DateTime aligned to the interval boundary.</returns>
        private DateTime AlignRangeStart(double startDate, double intervalSize)
        {
            DateTime dateTime = new DateTime(1970, 1, 1).AddMilliseconds(startDate);
            return ActualIntervalType switch
            {
                IntervalType.Years => new DateTime((int)Math.Floor(Math.Floor(dateTime.Year / intervalSize) * intervalSize), dateTime.Month, dateTime.Day, 0, 0, 0),
                IntervalType.Months => AlignMonthBoundary(dateTime, intervalSize),
                IntervalType.Days => AlignDayBoundary(dateTime, intervalSize),
                IntervalType.Hours => AlignHourBoundary(dateTime, intervalSize),
                IntervalType.Minutes => AlignMinuteBoundary(dateTime, intervalSize),
                IntervalType.Seconds => AlignSecondBoundary(dateTime, intervalSize),
                IntervalType.Auto => dateTime,
                _ => dateTime,
            };
        }

        /// <summary>
        /// Aligns a DateTime to the nearest month boundary.
        /// </summary>
        private static DateTime AlignMonthBoundary(DateTime dateTime, double intervalSize)
        {
            int month = (int)Math.Floor(Math.Floor(dateTime.Month / intervalSize) * intervalSize);
            month = month <= 1 ? 1 : (month == 2 && dateTime.Day > 28) ? (DateTime.IsLeapYear(dateTime.Year) && dateTime.Day == 29) ? month : 3 : month;
            return new DateTime(dateTime.Year, month, 1, 0, 0, 0).AddDays(dateTime.Day - 1);
        }

        /// <summary>
        /// Aligns a DateTime to the nearest day boundary.
        /// </summary>
        private static DateTime AlignDayBoundary(DateTime dateTime, double intervalSize)
        {
            int day = (int)Math.Floor(Math.Floor(dateTime.Day / intervalSize) * intervalSize);
            return (day <= 0)
                ? new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0).AddDays(-1)
                : new DateTime(dateTime.Year, dateTime.Month, day, 0, 0, 0);
        }

        /// <summary>
        /// Aligns a DateTime to the nearest hour boundary.
        /// </summary>
        private static DateTime AlignHourBoundary(DateTime dateTime, double intervalSize)
        {
            double hours = Math.Floor(Math.Floor(dateTime.Hour / intervalSize) * intervalSize);
            hours = (hours <= 0 || double.IsNaN(hours)) ? 0 : hours;
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, (int)hours, 0, 0);
        }

        /// <summary>
        /// Aligns a DateTime to the nearest minute boundary.
        /// </summary>
        private static DateTime AlignMinuteBoundary(DateTime dateTime, double intervalSize)
        {
            double minutes = Math.Floor(Math.Floor(dateTime.Minute / intervalSize) * intervalSize);
            minutes = (minutes <= 0 || double.IsNaN(minutes)) ? 0 : minutes;
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, (int)minutes, 0, 0);
        }

        /// <summary>
        /// Aligns a DateTime to the nearest second boundary.
        /// </summary>
        private static DateTime AlignSecondBoundary(DateTime dateTime, double intervalSize)
        {
            double seconds = Math.Floor(Math.Floor(dateTime.Second / intervalSize) * intervalSize);
            seconds = (seconds <= 0 || double.IsNaN(seconds)) ? 0 : seconds;
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, (int)seconds, 0);
        }

        /// <summary>
        /// Applies default date formats based on the interval type.
        /// </summary>
        private void ApplyDefaultDateFormats()
        {
            if (ActualIntervalType is IntervalType.Months or IntervalType.Days)
            {
                DateFormat = !string.IsNullOrEmpty(Axis?.LabelFormat)
                    ? Axis.LabelFormat : (ActualIntervalType == IntervalType.Months)
                    ? "yyyy MMM" : "d";
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Applies range padding depending on interval type and range padding mode.
        /// </summary>
        /// <param name="range">Current double range (unused when Axis range is set).</param>
        /// <param name="interval">Interval value used for padding calculations.</param>
        /// <returns>Adjusted DoubleRange with Min and Max set.</returns>
        protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
        {
            if (Axis is not null && !ChartHelper.SetRange(Axis))
            {
                ChartRangePadding rangePadding = GetRangePadding;
                DateTime minimum = new DateTime(1970, 1, 1).AddMilliseconds(Min);
                DateTime maximum = new DateTime(1970, 1, 1).AddMilliseconds(Max);

                if (rangePadding == ChartRangePadding.None)
                {
                    Min = GetTime(minimum);
                    Max = GetTime(maximum);
                }
                else if (rangePadding is ChartRangePadding.Additional or ChartRangePadding.Round)
                {
                    ApplyPaddingForInterval(ActualIntervalType, minimum, maximum, rangePadding, interval);
                }
            }

            return new DoubleRange(Min, Max);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Initializes the minimum and maximum values for the axis in internal millisecond representation.
        /// </summary>
        /// <returns>DoubleRange with initialized Min and Max.</returns>
        internal override DoubleRange InitializeDoubleRange()
        {
            if (Axis?.Minimum is not null)
            {
                Min = ConvertAxisValueToTime(Axis.Minimum);
            }
            else if (double.IsNaN(Min) || Min == double.PositiveInfinity)
            {
                Min = GetTime(new DateTime(1970, 1, 1));
            }

            if (Axis?.Maximum is not null)
            {
                Max = ConvertAxisValueToTime(Axis.Maximum);
            }
            else if (double.IsNaN(Max) || Max == double.NegativeInfinity)
            {
                Max = GetTime(new DateTime(1970, 5, 1));
            }

            if (Min == Max)
            {
                Max += 2592000000;
                Min -= 2592000000;
            }

            return new DoubleRange(Min, Max);
        }

        /// <summary>
        /// Calculates the actual interval for the given double range.
        /// </summary>
        /// <param name="range">Range used to compute interval.</param>
        /// <returns>Numeric interval value.</returns>
        internal override double CalculateActualInterval(DoubleRange range)
        {
            double dateTimeInterval = CalculateDateTimeNiceInterval(range.Start, range.End);
            return ChartHelper.IsNaNOrZero(Axis?.Interval ?? 0) ? dateTimeInterval : Axis?.Interval ?? 0;
        }

        /// <summary>
        /// Calculates visible range, adapts on zooming and triggers range render event if present.
        /// </summary>
        /// <param name="actualRange">Actual axis range.</param>
        /// <returns>Possibly adjusted visible range.</returns>
        internal override DoubleRange CalculateVisibleRange(DoubleRange actualRange)
        {
            if (Axis?.ZoomFactor < 1 || Axis?.ZoomPosition > 0)
            {
                actualRange = CalculateVisibleRangeOnZooming();
                if (Axis.EnableAutoIntervalOnZooming)
                {
                    CalculateAutoIntervalOnBothAxisRange(actualRange);
                    VisibleInterval = CalculateDateTimeNiceInterval(actualRange.Start, actualRange.End);
                }
            }

            RangeIntervalType intervalType = Enum.Parse<RangeIntervalType>(Axis?.AxisActualIntervalType ?? string.Empty);
            DateTimeInterval = GetTime(IncreaseDateTimeInterval(VisibleRange.Start, VisibleInterval, intervalType)) - VisibleRange.Start;

            if (Chart?.OnAxisActualRangeCalculated is not null)
            {
                actualRange = TriggerRangeRender(actualRange);
            }

            return actualRange;
        }

        /// <summary>
        /// Generates visible labels for the axis according to computed intervals.
        /// </summary>
        internal override void GenerateVisibleLabels()
        {
            VisibleLabels = [];
            if (!SeriesRenderer.Any(series => series?.Series is not null && series.Series.Visible) && (Axis?.Minimum is null || Axis.Maximum is null))
            {
                return;
            }

            double tempInterval = VisibleRange.Start;
            RangeIntervalType intervalType = Enum.Parse<RangeIntervalType>(Axis?.AxisActualIntervalType ?? string.Empty);
            bool isSameMinMax = SeriesMin == SeriesMax;

            if (isSameMinMax && Axis?.IntervalType == IntervalType.Months)
            {
                tempInterval = GetTime(new DateTime(1970, 1, 1).AddMilliseconds(SeriesMin).AddMonths(-(int)VisibleInterval));
            }
            else if (isSameMinMax && Axis?.IntervalType == IntervalType.Years)
            {
                tempInterval = GetTime(new DateTime(1970, 1, 1).AddMilliseconds(SeriesMin).AddYears(-(int)VisibleInterval));
            }
            else if (!ChartHelper.SetRange(Axis ?? null!))
            {
                tempInterval = (IsColumnType && !double.IsPositiveInfinity(SeriesMin) && !double.IsNegativeInfinity(SeriesMin) && Axis?.IntervalType != IntervalType.Auto)
                    ? GetTime(new DateTime(1970, 1, 1).AddMilliseconds(SeriesMin))
                    : GetTime(AlignRangeStart(tempInterval, VisibleInterval));
            }

            while (tempInterval <= VisibleRange.End && VisibleInterval > 0)
            {
                VisibleLabels[] axisLabels = [.. VisibleLabels];
                double previousValue = !double.IsNaN(axisLabels.Length) && (axisLabels.Length >= 1) ? VisibleLabels[axisLabels.Length - 1].Value : tempInterval;

                DateFormat = FindCustomFormats();
                if (ChartHelper.WithIn(tempInterval, VisibleRange))
                {
                    DateTime dateTime = new DateTime(1970, 1, 1).AddMilliseconds(tempInterval);
                    TriggerLabelRender(tempInterval, Intl.GetDateFormat(dateTime, FindCustomFormats()), Axis?.LabelTemplate is not null ? dateTime : null);
                }

                tempInterval = GetTime(IncreaseDateTimeInterval(tempInterval, VisibleInterval, intervalType));
            }

            ApplyDefaultDateFormats();
            GetMaxLabelWidth();
            base.GenerateVisibleLabels();
        }

        /// <summary>
        /// Increases a DateTime value by the specified interval respecting fractional intervals.
        /// </summary>
        /// <param name="minValue">Start value in internal milliseconds.</param>
        /// <param name="interval">Interval value (may be fractional).</param>
        /// <param name="intervalType">Type of interval to apply.</param>
        /// <returns>New DateTime after interval increase.</returns>
        internal DateTime IncreaseDateTimeInterval(double minValue, double interval, RangeIntervalType intervalType)
        {
            DateTime result = new DateTime(1970, 1, 1).AddMilliseconds(minValue);
            if (!ChartHelper.IsNaNOrZero(Axis?.Interval ?? 0))
            {
                IsIntervalInDecimal = (interval % 1) == 0;
                VisibleInterval = interval;
            }
            else
            {
                interval = Math.Ceiling(interval);
                VisibleInterval = interval;
            }

            if (IsIntervalInDecimal)
            {
                switch (intervalType)
                {
                    case RangeIntervalType.Years:
                        return result.AddYears((int)interval);
                    case RangeIntervalType.Quarter:
                        return result.AddMonths((int)(3 * interval));
                    case RangeIntervalType.Months:
                        return result.AddMonths((int)interval);
                    case RangeIntervalType.Weeks:
                        return result.AddDays(interval * 7);
                    case RangeIntervalType.Days:
                        return result.AddDays(interval);
                    case RangeIntervalType.Hours:
                        return result.AddHours(interval);
                    case RangeIntervalType.Minutes:
                        return result.AddMinutes(interval);
                    case RangeIntervalType.Seconds:
                        return result.AddSeconds(interval);
                    case RangeIntervalType.Auto:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                result = GetDecimalInterval(result, interval, intervalType);
            }

            return result;
        }

        /// <summary>
        /// Converts an object value to the renderer's double representation.
        /// </summary>
        /// <param name="x">Value to convert (DateTime or convertible).</param>
        /// <returns>Internal double representation (milliseconds).</returns>
        internal override double GetDoubleValue(object x)
        {
            return GetTime(Convert.ToDateTime(x, Culture));
        }

        /// <summary>
        /// Returns axis-friendly formatted date for a given point value.
        /// </summary>
        /// <param name="pointValue">Internal value in milliseconds.</param>
        /// <returns>Formatted date string suitable for rendering.</returns>
        internal override object GetAxisData(double pointValue)
        {
            return Intl.GetDateFormat(ChartHelper.GetDate(Convert.ToDouble(pointValue, Culture)), string.Empty);
        }

        #endregion
    }
}