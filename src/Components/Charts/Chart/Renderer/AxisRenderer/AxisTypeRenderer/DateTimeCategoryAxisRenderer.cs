using System.Globalization;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renderer for axes that represent DateTime values but behave as category axis.
    /// Handles weekly boundaries, label selection and formatting for category-style datetime axes.
    /// </summary>
    /// <remarks>
    /// This renderer is designed for scenarios where DateTime values should be treated as discrete categories
    /// rather than continuous values. It includes special logic for weekly grouping and intelligent label placement.
    /// </remarks>
    internal class DateTimeCategoryAxisRenderer : CategoryAxisRenderer
    {
        #region Constants
        private const int AXIS_LABEL_MAXIMUM_LENGTH = 100;
        private const int LABELS_COUNT_THRESHOLD = 30;
        #endregion

        #region Fields
        private List<string> _mondayLabels = [];
        private bool _isMonday;
        private int _previousIndex;

        /// <summary>
        /// Gets the collection of labels for the start of each week.
        /// </summary>
        /// <value>A list of datetime values representing Monday labels.</value>
        private List<string> MondayLabels => _mondayLabels;
        #endregion

        #region Private Methods

        /// <summary>
        /// Determines if two dates fall within the same interval based on interval type.
        /// </summary>
        /// <param name="currentDate">The current date value.</param>
        /// <param name="previousDate">The previous date value.</param>
        /// <param name="type">The interval type for comparison.</param>
        /// <param name="index">The current index position.</param>
        /// <returns><see langword="true"/> if dates are in same interval; otherwise, <see langword="false"/>.</returns>
        private static bool SameInterval(double currentDate, double previousDate, IntervalType type, double index)
        {
            if (index != 0)
            {
                switch (type)
                {
                    case IntervalType.Years:
                        return ChartHelper.GetDate(currentDate).Year == ChartHelper.GetDate(previousDate).Year;
                    case IntervalType.Months:
                        return ChartHelper.GetDate(currentDate).Year == ChartHelper.GetDate(previousDate).Year && ChartHelper.GetDate(currentDate).Month == ChartHelper.GetDate(previousDate).Month;
                    case IntervalType.Days:
                        return Math.Abs(currentDate - previousDate) < 24 * 60 * 60 * 1000 && ChartHelper.GetDate(currentDate).Day == ChartHelper.GetDate(previousDate).Day;
                    case IntervalType.Hours:
                        return Math.Abs(currentDate - previousDate) < 60 * 60 * 1000 && ChartHelper.GetDate(currentDate).Day == ChartHelper.GetDate(previousDate).Day;
                    case IntervalType.Minutes:
                        return Math.Abs(currentDate - previousDate) < 60 * 1000 && ChartHelper.GetDate(currentDate).Minute == ChartHelper.GetDate(previousDate).Minute;
                    case IntervalType.Seconds:
                        return Math.Abs(currentDate - previousDate) < 1000 && ChartHelper.GetDate(currentDate).Day == ChartHelper.GetDate(previousDate).Day;
                    case IntervalType.Auto:
                        break;
                    default:
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the distance between labels meets maximum spacing requirements.
        /// </summary>
        /// <param name="index">The current index.</param>
        /// <param name="ticksBetweenLabel">The ticks between labels.</param>
        /// <returns><see langword="true"/> if spacing is adequate; otherwise, <see langword="false"/>.</returns>
        private bool IsMaximumSpacing(int index, double ticksBetweenLabel)
        {
            double pointX = CalculatePointX(index, ticksBetweenLabel);
            double previousPointX = CalculatePointX(_previousIndex, ticksBetweenLabel);

            return VisibleLabels.Count == 0 || (pointX - previousPointX) >= GetMinimumSpacing();
        }

        /// <summary>
        /// Calculates the X coordinate for a given index.
        /// </summary>
        /// <param name="index">The index to calculate position for.</param>
        /// <param name="ticksBetweenLabel">The ticks between labels.</param>
        /// <returns>The calculated X coordinate.</returns>
        private double CalculatePointX(int index, double ticksBetweenLabel)
        {
            return ChartHelper.ValueToCoefficient(index - ticksBetweenLabel, Axis?.Renderer ?? null!) * (Axis?.Renderer?.Rect.Width ?? 0);
        }

        /// <summary>
        /// Gets the minimum spacing between labels based on label count.
        /// </summary>
        /// <returns>The minimum spacing value.</returns>
        private int GetMinimumSpacing()
        {
            return Labels.Count >= 15 ? AXIS_LABEL_MAXIMUM_LENGTH : (AXIS_LABEL_MAXIMUM_LENGTH - 50);
        }

        /// <summary>
        /// Determines if the current date starts a new week.
        /// </summary>
        /// <param name="currentDate">The current date value.</param>
        /// <param name="previousDate">The previous date value.</param>
        /// <param name="index">The current index.</param>
        /// <param name="ticksbwtLabel">The ticks between labels.</param>
        /// <returns><see langword="true"/> if date starts a new week; otherwise, <see langword="false"/>.</returns>
        private bool StartOfWeek(double currentDate, double previousDate, int index, double ticksbwtLabel)
        {
            if (Chart is not null && index >= 0)
            {
                // Place all the x-axis labels if labels count less than 30 
                DayOfWeek previousDay = ChartHelper.GetDate(previousDate).DayOfWeek;
                DayOfWeek currentDay = ChartHelper.GetDate(currentDate).DayOfWeek;

                DateTime previousWeekStart = ChartHelper.GetDate(previousDate).AddDays(-(int)previousDay);
                DateTime currentWeekStart = ChartHelper.GetDate(currentDate).AddDays(-(int)currentDay);
                bool isSameWeek = currentWeekStart == previousWeekStart;
                _isMonday = Labels.Count >= LABELS_COUNT_THRESHOLD ? !isSameWeek && GetAxisLabels(index, ticksbwtLabel, currentDate) : GetAxisLabels(index, ticksbwtLabel, currentDate);

            }
            return _isMonday;
        }

        /// <summary>
        /// Gets axis labels with proper spacing checks.
        /// </summary>
        /// <param name="index">The current index.</param>
        /// <param name="ticksbwtLabel">The ticks between labels.</param>
        /// <param name="currentDate">The current date value.</param>
        /// <returns><see langword="true"/> if label was added; otherwise, <see langword="false"/>.</returns>
        private bool GetAxisLabels(int index, double ticksbwtLabel, double currentDate)
        {
            if (IsMaximumSpacing(index, ticksbwtLabel))
            {
                _mondayLabels.Add(currentDate.ToString(CultureInfo.InvariantCulture));
                _isMonday = true;
            }
            else
            {
                _isMonday = false;
            }
            return _isMonday;
        }

        /// <summary>
        /// Calculates ticks between labels based on axis configuration.
        /// </summary>
        /// <returns>The calculated ticks between labels.</returns>
        private double CalculateTicksBetweenLabel()
        {
            return (Axis?.AxisValueType is not null &&
                    Axis.AxisValueType.Contains("Category", StringComparison.InvariantCulture) &&
                    Axis.LabelPlacement == LabelPlacement.BetweenTicks) ? 0.5 : 0;
        }

        /// <summary>
        /// Determines the interval type for the axis.
        /// </summary>
        private void DetermineIntervalType()
        {
            if (Axis?.IntervalType == IntervalType.Auto)
            {
                double startValue = Convert.ToDouble(Labels[0], CultureInfo.InvariantCulture);
                double endValue = Convert.ToDouble(Labels[^1], CultureInfo.InvariantCulture);
                _ = CalculateDateTimeNiceInterval(startValue, endValue);
            }
            else
            {
                ActualIntervalType = Axis?.IntervalType ?? IntervalType.Auto;
                if (Axis is { })
                {
                    Axis.AxisActualIntervalType = Axis.AxisIntervalType ?? null!;
                }
            }
        }

        /// <summary>
        /// Processes all labels and generates visible labels.
        /// </summary>
        /// <param name="ticksBetweenLabel">The ticks between labels.</param>
        private void ProcessLabels(double ticksBetweenLabel)
        {
            int interval = !double.IsNaN(Axis?.Interval ?? 1) ? (int)(Axis?.Interval ?? 1) : 1;

            for (int tempInterval = 0; tempInterval <= Labels.Count - 1; tempInterval += interval)
            {
                ProcessSingleLabel(tempInterval, ticksBetweenLabel);
            }
        }

        /// <summary>
        /// Processes a single label at the given interval.
        /// </summary>
        /// <param name="tempInterval">The current interval index.</param>
        /// <param name="ticksBetweenLabel">The ticks between labels.</param>
        private void ProcessSingleLabel(int tempInterval, double ticksBetweenLabel)
        {
            double labelValue = Convert.ToDouble(Labels[tempInterval], CultureInfo.InvariantCulture);
            DateTime dateTime = ChartHelper.GetDate(labelValue);
            double previousValue = GetPreviousValue(tempInterval);

            if (Chart is not null)
            {
                ProcessChartLabel(tempInterval, labelValue, previousValue, dateTime);
            }
            else
            {
                ProcessNonChartLabel(tempInterval, labelValue, previousValue, dateTime, ticksBetweenLabel);
            }
        }

        /// <summary>
        /// Gets the previous value for comparison.
        /// </summary>
        /// <param name="currentInterval">The current interval index.</param>
        /// <returns>The previous value.</returns>
        private double GetPreviousValue(int currentInterval)
        {
            int previousIndex = currentInterval == 0 ? 0 : currentInterval - 1;
            return Convert.ToDouble(Labels[previousIndex], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Processes label when Chart is available.
        /// </summary>
        /// <param name="tempInterval">The interval index.</param>
        /// <param name="labelValue">The label value.</param>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="dateTime">The date time value.</param>
        private void ProcessChartLabel(int tempInterval, double labelValue, double previousValue, DateTime dateTime)
        {
            bool shouldShowLabel = !SameInterval(labelValue, previousValue, ActualIntervalType, tempInterval) ||
                                 (Axis is not null && Axis.IsIndexed);
            double position = tempInterval - (Axis?.LabelPlacement == LabelPlacement.BetweenTicks ? 0.5 : 0);

            if (shouldShowLabel && ChartHelper.WithIn(position, VisibleRange))
            {
                string labelText = Axis is not null && Axis.IsIndexed ? GetIndexedAxisLabel(Labels[tempInterval]) : Intl.GetDateFormat(dateTime, CustomFormat());
                TriggerLabelRender(tempInterval, labelText, Axis?.LabelTemplate is not null ? dateTime : null);
            }
        }

        /// <summary>
        /// Processes label when Chart is not available.
        /// </summary>
        /// <param name="tempInterval">The interval index.</param>
        /// <param name="labelValue">The label value.</param>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="dateTime">The date time value.</param>
        /// <param name="ticksBetweenLabel">The ticks between labels.</param>
        private void ProcessNonChartLabel(int tempInterval, double labelValue, double previousValue, DateTime dateTime, double ticksBetweenLabel)
        {
            if (Axis?.IntervalType == IntervalType.Auto)
            {
                ProcessAutoIntervalLabel(tempInterval, labelValue, previousValue, dateTime, ticksBetweenLabel);
            }
            else
            {
                ProcessFixedIntervalLabel(tempInterval, labelValue, previousValue, dateTime, ticksBetweenLabel);
            }
        }

        /// <summary>
        /// Processes label with auto interval type.
        /// </summary>
        /// <param name="tempInterval">The interval index.</param>
        /// <param name="labelValue">The label value.</param>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="dateTime">The date time value.</param>
        /// <param name="ticksBetweenLabel">The ticks between labels.</param>
        private void ProcessAutoIntervalLabel(int tempInterval, double labelValue, double previousValue, DateTime dateTime, double ticksBetweenLabel)
        {
            bool isStartOfWeek = StartOfWeek(labelValue, previousValue, tempInterval, ticksBetweenLabel) || (Axis is not null && Axis.IsIndexed);
            double position = tempInterval - (Axis?.LabelPlacement == LabelPlacement.BetweenTicks ? 0.5 : 0);

            if (isStartOfWeek && ChartHelper.WithIn(position, VisibleRange))
            {
                string labelText = Axis is not null && Axis.IsIndexed
                    ? GetIndexedAxisLabel(MondayLabels[VisibleLabels.Count]) : Intl.GetDateFormat(ChartHelper.GetDate(Convert.ToDouble(MondayLabels[VisibleLabels.Count], CultureInfo.InvariantCulture)), CustomFormat());

                TriggerLabelRender(tempInterval, labelText, Axis?.LabelTemplate is not null ? dateTime : null);
                _previousIndex = tempInterval;
            }
        }

        /// <summary>
        /// Processes label with fixed interval type.
        /// </summary>
        /// <param name="tempInterval">The interval index.</param>
        /// <param name="labelValue">The label value.</param>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="dateTime">The date time value.</param>
        /// <param name="ticksBetweenLabel">The ticks between labels.</param>
        private void ProcessFixedIntervalLabel(int tempInterval, double labelValue, double previousValue, DateTime dateTime, double ticksBetweenLabel)
        {
            bool shouldShowLabel = !SameInterval(labelValue, previousValue, ActualIntervalType, tempInterval) || (Axis is not null && Axis.IsIndexed);
            double position = tempInterval - (Axis?.LabelPlacement == LabelPlacement.BetweenTicks ? 0.5 : 0);

            if (shouldShowLabel && ChartHelper.WithIn(position, VisibleRange))
            {
                if (IsMaximumSpacing(tempInterval, ticksBetweenLabel))
                {
                    string labelText = Axis is not null && Axis.IsIndexed ? GetIndexedAxisLabel(Labels[tempInterval]) : Intl.GetDateFormat(dateTime, CustomFormat());
                    TriggerLabelRender(tempInterval, labelText, Axis?.LabelTemplate is not null ? dateTime : null);
                    _previousIndex = tempInterval;
                }
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Generates visible labels for the datetime category axis.
        /// </summary>
        internal override void GenerateVisibleLabels()
        {
            VisibleLabels = [];
            _mondayLabels = [];
            _isMonday = false;

            double ticksBetweenLabel = CalculateTicksBetweenLabel();

            if (Labels.Count == 0)
            {
                MaxLabelSize = new Size(0, 0);
                return;
            }

            DetermineIntervalType();
            ProcessLabels(ticksBetweenLabel);

            DateFormat = CustomFormat();
            _previousIndex = 0;
            GetMaxLabelWidth();
        }

        /// <summary>
        /// Calculates the actual interval for the axis.
        /// </summary>
        /// <param name="range">The range to calculate interval for.</param>
        /// <returns>The calculated interval.</returns>
        internal override double CalculateActualInterval(DoubleRange range)
        {
            return (Axis is not null && double.IsNaN(Axis.Interval))
                ? Math.Max(1, Math.Floor(range.Delta / GetActualDesiredIntervalsCount()))
                : Math.Ceiling(Axis?.Interval ?? 1);
        }

        /// <summary>
        /// Calculates the visible range for the axis.
        /// </summary>
        /// <param name="actualRange">The actual range before calculation.</param>
        /// <returns>The calculated visible range.</returns>
        internal override DoubleRange CalculateVisibleRange(DoubleRange actualRange)
        {
            if (Axis?.ZoomFactor < 1 || Axis?.ZoomPosition > 0)
            {
                actualRange = CalculateVisibleRangeOnZooming();
                if (Axis.EnableAutoIntervalOnZooming && Axis.ValueType != ValueType.Category)
                {
                    CalculateAutoIntervalOnBothAxisRange(actualRange);
                    VisibleInterval = CalculateNumericNiceInterval(actualRange.Delta);
                }
            }
            return actualRange;
        }

        /// <summary>
        /// Gets the custom date format for labels.
        /// </summary>
        /// <returns>The custom format string.</returns>
        internal string CustomFormat()
        {
            return string.IsNullOrEmpty(Axis?.LabelFormat) ? (ActualIntervalType == IntervalType.Years && Chart is not null ? "yyyy" : GetSkeleton()) : Axis.LabelFormat;
        }

        /// <summary>
        /// Gets the indexed axis label for multiple values.
        /// </summary>
        /// <param name="axisValue">The comma-separated axis values.</param>
        /// <returns>The formatted indexed label.</returns>
        internal string GetIndexedAxisLabel(string axisValue)
        {
            string[] texts = axisValue.Split(',');
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i] = Intl.GetDateFormat(ChartHelper.GetDate(Convert.ToDouble(texts[i], null)), CustomFormat());
            }

            return string.Join(", ", texts);
        }

        /// <summary>
        /// Gets the axis data for a given point value.
        /// </summary>
        /// <param name="pointValue">The point value to get data for.</param>
        /// <returns>The formatted axis data.</returns>
        internal override object GetAxisData(double pointValue)
        {
            if (Labels.Count == 0)
            {
                return null!;
            }

            int index = (int)Math.Round(Math.Abs(pointValue));
            index = Math.Min(index, Labels.Count - 1);

            double dateValue = Convert.ToDouble(Labels[index], CultureInfo.InvariantCulture);
            return Intl.GetDateFormat(ChartHelper.GetDate(dateValue), string.Empty);
        }

        /// <summary>
        /// Determines if this axis is a category axis.
        /// </summary>
        /// <returns><see langword="true"/> if labels are placed between ticks; otherwise, <see langword="false"/>.</returns>
        internal override bool IsCategory()
        {
            return Axis?.LabelPlacement == LabelPlacement.BetweenTicks;
        }
        #endregion
    }
}