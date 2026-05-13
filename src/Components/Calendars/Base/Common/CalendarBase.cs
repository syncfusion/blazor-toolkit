using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Inputs;
using Syncfusion.Blazor.Toolkit.Internal;
using System.ComponentModel;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// Represents a calendar component that displays a Gregorian calendar and allows users to select dates with full support for localization, min/max constraints, and custom behaviors.
    /// </summary>
    /// <remarks>
    /// The <see cref="CalendarBase{T}"/> class is the base for Syncfusion Blazor Calendar control. It enables date selection, localization, keyboard navigation, and advanced features such as restricted selection by min/max dates and multiple calendar views.
    /// It supports conversion between Gregorian and Hijri calendars. Customization for keyboard shortcuts and rendering can be achieved through exposed events and parameters.
    /// </remarks>
    /// <typeparam name="T">Specifies the supported value type for the calendar's selected date (e.g., <c>DateTime</c>, <c>DateOnly</c>, <c>DateTimeOffset</c>).</typeparam>
    /// <example>
    /// The following example demonstrates how to instantiate the CalendarBase component allowing selection between a date range:
    /// <code><![CDATA[
    /// <SfCalendar Min="@new DateTime(2020, 1, 1)" Max="@new DateTime(2025, 12, 31)" FirstDayOfWeek="1" DayHeaderFormat="DayHeaderFormats.Wide"/>
    /// ]]></code>
    /// </example>
    public class CalendarBase<T> : SfInputBase<T>
    {
        internal const string SHORTDATE = "M/d/yy";

        internal DateTime CalendarBase_Max { get; set; }
        internal DateTime CalendarBase_Min { get; set; }
        internal Dictionary<string, string> CustomizedDates { get; set; } = [];
        internal int CalendarBase_FirstDayOfWeek { get; set; }
        internal CalendarView CalendarBase_Depth { get; set; }
        internal CalendarView CalendarBase_Start { get; set; }
        internal bool IslamicYearClick { get; set; }
        internal T? CalendarBase_Value { get; set; }
        internal ChangedEventArgs<T> ChangedArgs { get; set; } = new ChangedEventArgs<T>();
        internal List<CellDetails>? CellListData { get; set; }
        internal List<CellDetails>? PreviousCellListData { get; set; }
        internal List<CellDetails>? UpdatePreviousCell { get; set; }
        internal List<CellDetails> DisabledDayCellData { get; set; } = [];
        internal List<CellDetails>? CellDetailsData { get; set; } = [];
        internal T? PreviousDate { get; set; }
        internal T? PreviousSelectedDate { get; set; }
        internal T? PreviousDeSelectedDate { get; set; }
        internal int PreviousValues { get; set; }
        internal bool IsTodayClick { get; set; }
        internal bool IsRenderDayCellEvent { get; set; }
        internal ElementReference Element { get; set; }

        private static readonly char[] _separatorArray = [',', ' ', '/', '-', ':'];
        private List<string> DirectParamKeys { get; set; } = [];

        internal string[] _hijri_Month_Wide = [ "Muharram", "Safar", "Rabi'I", "Rabi'II", "Jumada'I", "Jumada'II",
                                                "Rajab", "Sha'ban", "Ramadan", "Shawwal", "Dhu'l-Qi'dah", "Dhu'l-Hijjah" ];
        internal string[] _hijri_Month_Abbreviated = [ "Muh.", "Saf.", "Rabi'.I", "Rabi'.II", "Jum.I", "Jum.II",
                                                        "Raj.", "Sha.", "Ram.", "Shaw.", "Dhu'I", "Dhu'II" ];
        internal string[] _hijri_Month_Abbreviated_French = [ "mouh.","saf.","rab.","rab.th","joum.","joum.","raj.","chaa.",
                                                            "ram.","chaw.", "dhou.","dhou."  ];
        internal string[] _hijri_Month_Wide_French = [ "mouharram", "safar", "rabia al awal", "rabia ath-thani", "joumada al oula", "jumada ath-thania",
                                          "rajab", "chaabane", "ramadan", "chawwal", "dhou al qi`da", "dhou al-hijja" ];

        /// <summary>
        /// Gets or sets the <see cref="EditContext"/> for the Calendar.
        /// </summary>
        /// <value>
        /// The <see cref="EditContext"/> instance that manages the editing state of the calendar's associated form.
        /// </value>
        /// <remarks>
        /// When set, this enables integration of the calendar with Blazor form validation and editing workflows.
        /// </remarks>
        [CascadingParameter]
        protected EditContext? CalendarEditContext { get; set; }

        /// <summary>
        /// Gets or sets the maximum selectable date for the calendar.
        /// </summary>
        /// <value>
        /// A <c>DateTime</c> value specifying the latest date a user can select. The default is 31-Dec-2099.
        /// </value>
        /// <remarks>
        /// Use this property to restrict user selection to dates before or equal to the specified maximum.
        /// </remarks>
        /// <example>
        /// Set the maximum date to 31-Dec-2025:
        /// <code><![CDATA[
        /// <SfCalendar Max="@new DateTime(2025,12,31)" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public virtual DateTime Max { get; set; } = new DateTime(2099, 12, 31);

        /// <summary>
        /// Gets or sets the minimum selectable date for the calendar.
        /// </summary>
        /// <value>
        /// A <c>DateTime</c> value specifying the earliest date a user can select. The default is 01-Jan-1900.
        /// </value>
        /// <remarks>
        /// Use this property to restrict user selection to dates on or after the specified minimum.
        /// </remarks>
        /// <example>
        /// Set the minimum date to 1-Jan-2000:
        /// <code><![CDATA[
        /// <SfCalendar Min="@new DateTime(2000,1,1)" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public virtual DateTime Min { get; set; } = new DateTime(1900, 01, 01);

        /// <summary>
        /// Gets or sets the current culture used for formatting and localization of calendar values and UI elements.
        /// </summary>
        /// <value>
        /// A <see cref="CultureInfo"/> instance specifying the culture. Defaults to the culture of the running thread.
        /// </value>
        /// <remarks>
        /// This property determines locale-specific formatting for date values, such as the day and month names.
        /// </remarks>
        /// <exclude/>
        protected CultureInfo CurrentCulture { get; set; } = default!;

        /// <summary>
        /// Gets or sets the first day of the week in the calendar view.
        /// </summary>
        /// <value>
        /// An <c>int</c> value representing the first day of the week, where 0 = Sunday, 1 = Monday, etc. Default is based on the current culture.
        /// </value>
        /// <remarks>
        /// Use this property to change the calendar's week layout to start on a different day (for example, Monday instead of Sunday).
        /// </remarks>
        /// <example>
        /// Set the calendar's first day of the week to Monday:
        /// <code><![CDATA[
        /// <SfCalendar FirstDayOfWeek="1" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public int FirstDayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the calendar system type, such as Gregorian or Hijri.
        /// </summary>
        /// <value>
        /// A <see cref="CalendarType"/> enum value specifying the calendar system to be used in the component (e.g., Gregorian, Hijri).
        /// </value>
        /// <remarks>
        /// Use this property to enable the selection and display of dates in different calendar systems.
        /// </remarks>
        /// <example>
        /// Display the calendar using the Hijri system:
        /// <code><![CDATA[
        /// <SfCalendar CalendarMode="CalendarType.Hijri" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public CalendarType CalendarMode { get; set; }

        /// <summary>
        /// Gets or sets the display format for day names shown in the calendar's header.
        /// </summary>
        /// <value>
        /// A <see cref="DayHeaderFormats"/> value specifying the header day name format (Short, Narrow, Abbreviated, Wide). Default is Short.
        /// </value>
        /// <remarks>
        /// This property configures the format of day names in the calendar header row, allowing customization of the day label presentation for various cultures or UI requirements.
        /// </remarks>
        /// <example>
        /// Show abbreviated day names (e.g., Mon, Tue, Wed):
        /// <code><![CDATA[
        /// <SfCalendar DayHeaderFormat="DayHeaderFormats.Abbreviated" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public DayHeaderFormats DayHeaderFormat { get; set; }

        /// <summary>
        /// Gets or sets the maximum navigation depth in the calendar (e.g., Month, Year, Decade).
        /// </summary>
        /// <value>
        /// A <see cref="CalendarView"/> value specifying the maximum depth users can navigate. Typical values are Month, Year, or Decade.
        /// </value>
        /// <remarks>
        /// The <c>Depth</c> must be smaller than <see cref="Start"/> to properly restrict view navigation.
        /// </remarks>
        /// <example>
        /// Allow users to navigate up to the Year view only:
        /// <code><![CDATA[
        /// <SfCalendar Start="CalendarView.Month" Depth="CalendarView.Year" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public CalendarView Depth { get; set; }

        /// <summary>
        /// Gets or sets the custom keyboard actions for the calendar component.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> that maps key action names to custom shortcut values.
        /// </value>
        /// <remarks>
        /// Use this property to provide keyboard accessibility and localization, such as customizing shortcuts for different keyboard layouts.
        /// </remarks>
        /// <example>
        /// Provide custom key actions for German keyboards:
        /// <code><![CDATA[
        /// <SfCalendar KeyConfigs="@myKeyConfigs" />
        /// @code {
        ///   Dictionary<string, object> myKeyConfigs = new() { { "moveRight", "Alt+RightArrow" } };
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public Dictionary<string, object> KeyConfigs { get; set; } = default!;
        /// <summary>
        /// Gets or sets the initial calendar view displayed when the calendar is opened.
        /// </summary>
        /// <value>
        /// A <see cref="CalendarView"/> value determining whether the initial view starts from Month, Year, or Decade. Default is Month.
        /// </value>
        /// <remarks>
        /// Use this property to control which navigation level users see first when they open the calendar.
        /// </remarks>
        /// <example>
        /// Set the calendar to open on the year view:
        /// <code><![CDATA[
        /// <SfCalendar Start="CalendarView.Year" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public CalendarView Start { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether the today button is displayed in the calendar UI.
        /// </summary>
        /// <value>
        /// <c>true</c> to show the today button; otherwise, <c>false</c>. Default is <c>true</c>.
        /// </value>
        /// <remarks>
        /// The today button allows users to quickly select the current date in the calendar.
        /// </remarks>
        /// <example>
        /// Hide the today button:
        /// <code><![CDATA[
        /// <SfCalendar ShowTodayButton="false" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool ShowTodayButton { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the calendar displays the week number in the UI.
        /// </summary>
        /// <value>
        /// <c>true</c> to display the week number; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// When enabled, a week number column is shown for each date row in the calendar.
        /// </remarks>
        [Parameter]
        public bool WeekNumber { get; set; }

        /// <summary>
        /// Gets or sets the rule used to determine the first week of the year in the calendar.
        /// </summary>
        /// <value>
        /// A <see cref="CalendarWeekRule"/> value that specifies how the first week of the year is calculated.
        /// </value>
        /// <remarks>
        /// The default is culture-dependent. Valid options include FirstDay, FirstFullWeek, and FirstFourDayWeek.
        /// </remarks>
        /// <example>
        /// Specify the calendar week rule:
        /// <code><![CDATA[
        /// <SfCalendar WeekRule="CalendarWeekRule.FirstFourDayWeek" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public CalendarWeekRule WeekRule { get; set; }

        internal virtual Task UpdateCalendarPropertyAsync(string key, object? dateValue)
        {
            return Task.CompletedTask;
        }

        internal static DateTime[] CopyValues(DateTime[]? values)
        {
            return values is null || values.Length == 0 ? [] : [.. values.Select(v => new DateTime(v.Ticks))];
        }

        /// <summary>
        /// Checks if a specific date exists in the provided array of date values.
        /// </summary>
        /// <param name="dates">The date to check for presence in the values array.</param>
        /// <param name="values">The array of DateTime values to search.</param>
        /// <returns>True if the date is present; otherwise, false.</returns>
        internal bool CheckPresentDate(DateTime dates, DateTime[]? values)
        {
            if (!HasValidValues(values))
            {
                return false;
            }
            UpdateChangedEventArgs(null);
            string targetDateString = FormatDateForComparison(dates);
            return ContainsMatchingDate(values!, targetDateString);
        }

        private static bool HasValidValues(DateTime[]? values)
        {
            return values is not null && values.Length > 0;
        }

        private static string FormatDateForComparison(DateTime date)
        {
            return Intl.GetDateFormat(date, SHORTDATE);
        }

        private static bool ContainsMatchingDate(DateTime[] values, string targetDateString)
        {
            for (int index = 0; index < values.Length; index++)
            {
                string currentDateString = FormatDateForComparison(values[index]);
                if (currentDateString == targetDateString)
                {
                    return true;
                }
            }
            return false;
        }

        internal void ChangeHandler(MouseEventArgs? args = null, DateTime[]? values = null, bool? isMultiSelection = null, bool isSelection = false)
        {
            UpdateChangedEventArgs(args);
            if (ShouldTriggerSingleSelectionChange(isMultiSelection))
            {
                ChangeEvent(args, isSelection);
            }
            else if (values is not null && PreviousValues != values.Length)
            {
                ChangeEvent(args, isSelection);
                PreviousValues = values.Length;
            }
        }

        private void UpdateChangedEventArgs(MouseEventArgs? args)
        {
            if (ChangedArgs is not null)
            {
                ChangedArgs.Event = args is null ? new() : args;
                ChangedArgs.IsInteracted = args is not null;
            }
        }

        private bool ShouldTriggerSingleSelectionChange(bool? isMultiSelection)
        {
            if (isMultiSelection is null || (bool)isMultiSelection)
            {
                return false;
            }
            Type propertyType = typeof(T);
            T? dateValue = Value is null ? default! : (T)SfBaseUtils.ChangeType(Value, propertyType);
            T? previousDateVal = PreviousDate is null ? default : (T)SfBaseUtils.ChangeType(PreviousDate, propertyType);
            return !SfBaseUtils.Equals(dateValue, previousDateVal);
        }

        /// <summary>
        /// Converts the generic value <typeparamref name="T"/> to <see cref="DateTime"/> for internal processing.
        /// </summary>
        /// <param name="dateValue">The date value of type <typeparamref name="T"/>.</param>
        /// <returns>A <see cref="DateTime"/> representation of <paramref name="dateValue"/>.</returns>
        /// <remarks>
        /// Supports conversions for <c>DateTime</c>, <c>DateOnly</c>, and <c>DateTimeOffset</c> types.
        /// </remarks>
        internal static DateTime ConvertDate(T dateValue)
        {
            Type propertyType = typeof(T);
            bool isNullable = Nullable.GetUnderlyingType(propertyType) is not null;
            if (IsDateTimeType(dateValue, propertyType, isNullable))
            {
                return ConvertToDateTime(dateValue, propertyType);
            }
            if (IsDateOnlyType(dateValue, propertyType, isNullable))
            {
                return ConvertDateOnlyToDateTime(dateValue, propertyType);
            }
            if (IsDateTimeOffsetType(dateValue, propertyType, isNullable))
            {
                return ConvertDateTimeOffsetToDateTime(dateValue, propertyType);
            }
            return DateTime.Now;
        }

        private static bool IsDateTimeType(T dateValue, Type propertyType, bool isNullable)
        {
            return (dateValue is not null && propertyType == typeof(DateTime)) || (isNullable && typeof(DateTime) == Nullable.GetUnderlyingType(propertyType));
        }

        private static bool IsDateOnlyType(T dateValue, Type propertyType, bool isNullable)
        {
            return (dateValue is not null && propertyType == typeof(DateOnly)) || (isNullable && typeof(DateOnly) == Nullable.GetUnderlyingType(propertyType));
        }

        private static bool IsDateTimeOffsetType(T dateValue, Type propertyType, bool isNullable)
        {
            return (dateValue is not null && propertyType == typeof(DateTimeOffset)) || (isNullable && typeof(DateTimeOffset) == Nullable.GetUnderlyingType(propertyType));
        }

        private static DateTime ConvertToDateTime(T dateValue, Type propertyType)
        {
            return (DateTime)SfBaseUtils.ChangeType(dateValue!, propertyType);
        }

        private static DateTime ConvertDateOnlyToDateTime(T dateValue, Type propertyType)
        {
            DateOnly date = (DateOnly)SfBaseUtils.ChangeType(dateValue!, propertyType);
            return date.ToDateTime(TimeOnly.MinValue);
        }

        private static DateTime ConvertDateTimeOffsetToDateTime(T dateValue, Type propertyType)
        {
            DateTimeOffset dateTimeOffset = (DateTimeOffset)SfBaseUtils.ChangeType(dateValue!, propertyType);
            return dateTimeOffset.DateTime;
        }

        internal virtual void BindNavigateEvent(NavigatedEventArgs eventArgs)
        {
        }

        internal virtual async Task UpdateAriaActiveDescendantAsync(string? cellId)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        internal virtual async Task BindRenderDayEventAsync(RenderDayCellEventArgs eventArgs)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        internal virtual async Task BindMinMaxDaysAsync()
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Triggers when the value get changed.
        /// </summary>
        /// <param name="args">Specifies the <see cref="EventArgs"> arguments</see>.</param>
        /// <param name="isSelection">Determines whether selection is made using the mouse or keyboard.</param>
        protected virtual void ChangeEvent(EventArgs? args, bool isSelection = false)
        {
        }

        internal virtual bool IsMultipleDatesSetProgrammatically()
        {
            return false;
        }

        internal static T? GenericValue(DateTime dateValue)
        {
            Type? propertyType = typeof(T);
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                propertyType = Nullable.GetUnderlyingType(propertyType);
            }
            return propertyType == typeof(DateTime) ? (T)SfBaseUtils.ChangeType(dateValue, propertyType) : propertyType == typeof(DateOnly)
                    ? (T)SfBaseUtils.ChangeType(DateOnly.FromDateTime(dateValue), propertyType)
                    : (T)SfBaseUtils.ChangeType(new DateTimeOffset(dateValue), propertyType!);
        }

        internal async Task SetLocalStorageAsync(string persistId, T dataValue)
        {
            if (persistId is not null && dataValue is not null)
            {
                await InvokeVoidAsync(_baseJsModule, _baseJsInProcessModule, "setLocalStorageItem", [persistId, dataValue]).ConfigureAwait(true);
            }
        }

        internal virtual async Task InvokeDeSelectEventAsync(DeSelectedEventArgs<T> args)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        internal override async Task OnAfterScriptRenderedAsync()
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Converts a Gregorian <see cref="DateTime"/> object to a formatted Hijri date string.
        /// </summary>
        /// <param name="gregorianDate">
        /// The Gregorian date to be converted to Hijri, of type <typeparamref name="T"/>.
        /// </param>
        /// <param name="format">
        /// The format string specifying the output structure of the Hijri date. 
        /// Supports standard date format specifiers such as "dd", "MM", "MMM", "MMMM", "yyyy", and time components.
        /// </param>
        /// <returns>
        /// A string representing the Hijri date formatted according to the specified <paramref name="format"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="format"/> is null.
        /// </exception>
        /// <remarks>
        /// This method converts a <see cref="DateTime"/> value from Gregorian to Hijri and formats it.
        /// If the format string contains "MMM" or "MMMM", it replaces the month part with a Hijri month name
        /// (abbreviated or full, depending on the format).
        /// </remarks>
        public string ConvertToHijri(T gregorianDate, string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = CurrentCulture.DateTimeFormat.ShortDatePattern;
            }
            DateTime gregorianDateTime = ConvertDateValue(gregorianDate);
            HijriDate hijriDate = HijriParser.ToHijriDate(gregorianDateTime);
            DateTime finalDate = CreateHijriDateTime(hijriDate, gregorianDateTime);
            string formattedDate = finalDate.ToString(format, CultureInfo.CurrentCulture);
            return ReplaceMonthNamesIfNeeded(formattedDate, finalDate, format);
        }

        private static DateTime CreateHijriDateTime(HijriDate hijriDate, DateTime gregorianDateTime)
        {
            return new DateTime(hijriDate.Year, hijriDate.Month, hijriDate.Date)
                   .Add(gregorianDateTime.TimeOfDay);
        }

        private string ReplaceMonthNamesIfNeeded(string formattedDate, DateTime finalDate, string format)
        {
            if (!format.Contains("MMM", StringComparison.OrdinalIgnoreCase))
            {
                return formattedDate;
            }
            string[] hijriMonths = format.Contains("MMMM", StringComparison.OrdinalIgnoreCase) ? _hijri_Month_Wide : _hijri_Month_Abbreviated;
            return ReplaceMonthName(formattedDate, finalDate, hijriMonths, format);
        }

        /// <summary>
        /// Converts a formatted Hijri date string to a Gregorian <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="hijriDate">
        /// The Hijri date string to convert, formatted according to the specified <paramref name="format"/>.
        /// </param>
        /// <param name="format">
        /// The format string that specifies the structure of the <paramref name="hijriDate"/>.
        /// Valid format components include "dd" for day, "MM" for month (numeric), 
        /// "MMM"/"MMMM" for month name, "yyyy" for year, and "HH"/"mm"/"ss" for time.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> object in Gregorian format representing the date and time specified in the Hijri <paramref name="hijriDate"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="hijriDate"/> or <paramref name="format"/> is null or empty.
        /// </exception>
        /// <remarks>
        /// This method splits the <paramref name="hijriDate"/> and <paramref name="format"/> into individual components,
        /// parses each component to extract day, month, year, and time values, and converts them from Hijri to Gregorian.
        /// It assumes that the <paramref name="hijriDate"/> is in the Hijri calendar.
        /// </remarks>
        public T ConvertToGregorian(string hijriDate, string format)
        {
            ValidateConversionInput(hijriDate, ref format);
            string[] dateParts = hijriDate.Split(_separatorArray, StringSplitOptions.RemoveEmptyEntries);
            string[] formatParts = format.Split(_separatorArray, StringSplitOptions.RemoveEmptyEntries);
            (int day, int month, int year, TimeSpan time) = ParseHijriDateParts(dateParts, formatParts);
            DateTime gregorianDate = ConvertHijriToGregorian(year, month, day, time);
            return (T)(object)gregorianDate;
        }

        private void ValidateConversionInput(string hijriDate, ref string format)
        {
            if (string.IsNullOrEmpty(hijriDate))
            {
                throw new ArgumentNullException(nameof(hijriDate));
            }
            if (string.IsNullOrEmpty(format))
            {
                format = CurrentCulture.DateTimeFormat.ShortDatePattern;
            }
        }

        private (int day, int month, int year, TimeSpan time) ParseHijriDateParts(string[] dateParts, string[] formatParts)
        {
            int day = 0, month = 0, year = 0;
            TimeSpan time = TimeSpan.Zero;
            for (int partIndex = 0; partIndex < formatParts.Length; partIndex++)
            {
                if (partIndex >= dateParts.Length)
                {
                    continue;
                }
                ParseHijriDateComponent(dateParts[partIndex], formatParts[partIndex], ref day, ref month, ref year, ref time);
            }
            return (day, month, year, time);
        }

        private void ParseHijriDateComponent(string dataPart, string formatPart, ref int day, ref int month, ref int year, ref TimeSpan time)
        {
            switch (formatPart)
            {
                case "d":
                case "dd":
                    day = int.Parse(dataPart, CultureInfo.CurrentCulture);
                    break;
                case "M":
                case "MM":
                    month = int.Parse(dataPart, CultureInfo.CurrentCulture);
                    break;
                case "MMM":
                case "MMMM":
                    month = GetHijriMonthIndex(dataPart, formatPart);
                    break;
                case "yyyy":
                    year = int.Parse(dataPart, CultureInfo.CurrentCulture);
                    break;
                case "h":
                case "HH":
                case "hh":
                    time = ParseHours(dataPart, time);
                    break;
                case "mm":
                    time = ParseMinutes(dataPart, time);
                    break;
                case "ss":
                    time = ParseSeconds(dataPart, time);
                    break;
                default:
                    break;
            }
        }

        private static TimeSpan ParseHours(string hoursPart, TimeSpan currentTime)
        {
            int hours = int.Parse(hoursPart, CultureInfo.CurrentCulture);
            return new TimeSpan(hours, currentTime.Minutes, currentTime.Seconds);
        }

        private static TimeSpan ParseMinutes(string minutesPart, TimeSpan currentTime)
        {
            int minutes = int.Parse(minutesPart, CultureInfo.CurrentCulture);
            return new TimeSpan(currentTime.Hours, minutes, currentTime.Seconds);
        }

        private static TimeSpan ParseSeconds(string secondsPart, TimeSpan currentTime)
        {
            int seconds = int.Parse(secondsPart, CultureInfo.CurrentCulture);
            return new TimeSpan(currentTime.Hours, currentTime.Minutes, seconds);
        }

        private static DateTime ConvertHijriToGregorian(int year, int month, int day, TimeSpan time)
        {
            HijriDate hijriDate = new() { Year = year, Month = month, Date = day };
            return HijriParser.ToGregorian(hijriDate).Add(time);
        }

        /// <summary>
        /// Resolves a Hijri month name to its numeric month index (1-12).
        /// </summary>
        /// <param name="monthName">The Hijri month name to look up. This may be an abbreviated or full month name depending on <paramref name="format"/>.</param>
        /// <param name="format">The month format used in the source string. Expected values are <c>"MMM"</c> for abbreviated names or <c>"MMMM"</c> for full month names.
        /// If <c>"MMM"</c> is provided the method searches the abbreviated month array; otherwise the wide/full month array is used.</param>
        /// <returns>
        /// The 1-based month index (1 = first month) when a match is found; otherwise <c>0</c> if the provided name could not be resolved.
        /// </returns>
        protected int GetHijriMonthIndex(string monthName, string format)
        {
            string[] monthArray = format == "MMM" ? _hijri_Month_Abbreviated : _hijri_Month_Wide;
            for (int monthIndex = 0; monthIndex < monthArray.Length; monthIndex++)
            {
                if (monthArray[monthIndex].Equals(monthName, StringComparison.OrdinalIgnoreCase))
                {
                    return monthIndex + 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// Normalizes a generic value of type <typeparamref name="T"/> to a <see cref="DateTime"/> instance for internal processing.
        /// </summary>
        /// <param name="dateValue">The value to convert. Expected runtime types include <see cref="DateTime"/>, <see cref="DateOnly"/>, or <see cref="DateTimeOffset"/>, possibly wrapped in a nullable type.</param>
        /// <returns>A <see cref="DateTime"/> representing the input value. If the input is a <see cref="DateOnly"/>, the returned <see cref="DateTime"/> will use <see cref="TimeOnly.MinValue"/> for the time portion. If the input is a <see cref="DateTimeOffset"/>, the returned value uses the <see cref="DateTimeOffset.DateTime"/> property.</returns>
        protected DateTime ConvertDateValue(T dateValue)
        {
            if (IsDateTimeOffsetType())
            {
                return ((DateTimeOffset)SfBaseUtils.ChangeType(dateValue!, typeof(T))).DateTime;
            }
            else if (IsDateOnlyType())
            {
                return ((DateOnly)SfBaseUtils.ChangeType(dateValue!, typeof(T))).ToDateTime(TimeOnly.MinValue);
            }
            return (DateTime)SfBaseUtils.ChangeType(dateValue!, typeof(T));
        }

        /// <summary>
        /// Checks whether the value type is DateOnly.
        /// </summary>
        /// <returns>True or false based on the Type.</returns>
        /// <exclude/>
        protected bool IsDateOnlyType()
        {
            Type type = typeof(T);
            bool isNullable = Nullable.GetUnderlyingType(type) is not null;
            return type == typeof(DateOnly) || (isNullable && typeof(DateOnly) == Nullable.GetUnderlyingType(type));
        }

        /// <summary>
        /// Checks whether the value type is DateTimeOffset.
        /// </summary>
        /// <returns>True or false based on the Type.</returns>
        /// <exclude/>
        protected bool IsDateTimeOffsetType()
        {
            Type type = typeof(T);
            bool isNullable = Nullable.GetUnderlyingType(type) is not null;
            return type == typeof(DateTimeOffset) || (isNullable && typeof(DateTimeOffset) == Nullable.GetUnderlyingType(type));
        }

        /// <summary>
        /// Replaces any Gregorian day or month names found in an already-formatted date string with the corresponding Hijri equivalents.
        /// </summary>
        /// <param name="formattedDate">A date string previously produced by <see cref="DateTime.ToString(string, IFormatProvider)"/> using <paramref name="format"/>. This string may contain Gregorian day and month names that need replacing.</param>
        /// <param name="finalDate">The <see cref="DateTime"/> value that corresponds to <paramref name="formattedDate"/>. Used to obtain the original Gregorian day/month tokens for replacement and to select the correct Hijri month by index.</param>
        /// <param name="hijriMonths">An array of Hijri month names (either abbreviated or full) indexed from 0..11. The method reads the proper name from this array using <see cref="DateTime.Month"/> - 1.</param>
        /// <param name="format">The format string that was used to produce <paramref name="formattedDate"/>. It is inspected to determine whether month names were rendered as full ("MMMM") or abbreviated ("MMM") and whether day names are present ("dddd").
        /// </param>
        /// <returns>
        /// A new string where any Gregorian day and month names found in <paramref name="formattedDate"/> are replaced by the matching Hijri names from <paramref name="hijriMonths"/>. If no replacements are required the original string is returned unchanged.
        /// </returns>
        protected static string ReplaceMonthName(string formattedDate, DateTime finalDate, string[] hijriMonths, string format)
        {
            ValidateReplaceMonthNameInputs(formattedDate, format, hijriMonths);
            string result = ReplaceDayNameIfNeeded(formattedDate, finalDate, format);
            result = ReplaceMonthNameWithHijri(result, finalDate, hijriMonths, format);
            return result;
        }

        private static void ValidateReplaceMonthNameInputs(string formattedDate, string format, string[] hijriMonths)
        {
            if (formattedDate is null)
            {
                throw new ArgumentNullException(nameof(formattedDate), "Formatted date cannot be null.");
            }
            if (format is null)
            {
                throw new ArgumentNullException(nameof(format), "The format parameter cannot be null.");
            }
            if (hijriMonths is null)
            {
                throw new ArgumentNullException(nameof(hijriMonths), "The hijriMonths array cannot be null.");
            }
        }

        private static string ReplaceDayNameIfNeeded(string formattedDate, DateTime finalDate, string format)
        {
            if (format.Contains("dddd", StringComparison.OrdinalIgnoreCase))
            {
                string dayName = finalDate.ToString("dddd", CultureInfo.CurrentCulture);
                return formattedDate.Replace(dayName, finalDate.DayOfWeek.ToString(), StringComparison.OrdinalIgnoreCase);
            }
            return formattedDate;
        }

        private static string ReplaceMonthNameWithHijri(string formattedDate, DateTime finalDate, string[] hijriMonths, string format)
        {
            string monthFormat = format.Contains("MMMM", StringComparison.OrdinalIgnoreCase) ? "MMMM" : "MMM";
            string gregorianMonth = finalDate.ToString(monthFormat, CultureInfo.CurrentCulture);
            string hijriMonth = hijriMonths[finalDate.Month - 1];

            return formattedDate.Replace(gregorianMonth, hijriMonth, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Triggers while properties get dynamically changed in the component.
        /// </summary>
        /// <returns>System.Threading.Tasks.</returns>
        /// <param name="parameters"><see cref="ParameterView"/> parameters.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            InitializeDirectParameterKeys(parameters);
            ApplyCultureDefaults();
            return base.SetParametersAsync(parameters);
        }

        private void InitializeDirectParameterKeys(ParameterView parameters)
        {
            if (DirectParamKeys.Count > 0)
            {
                return;
            }
            foreach (ParameterValue parameter in parameters)
            {
                if (!parameter.Cascading)
                {
                    DirectParamKeys.Add(parameter.Name);
                }
            }
        }

        private void ApplyCultureDefaults()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            if (currentCulture is null)
            {
                return;
            }
            ApplyWeekRuleDefault(currentCulture);
            ApplyFirstDayOfWeekDefault(currentCulture);
        }

        private void ApplyWeekRuleDefault(CultureInfo culture)
        {
            if (!DirectParamKeys.Contains("WeekRule"))
            {
                WeekRule = culture.DateTimeFormat.CalendarWeekRule;
            }
        }

        private void ApplyFirstDayOfWeekDefault(CultureInfo culture)
        {
            if (!DirectParamKeys.Contains("FirstDayOfWeek"))
            {
                FirstDayOfWeek = (int)culture.DateTimeFormat.FirstDayOfWeek;
            }
        }

        internal virtual async Task InvokeSelectEventAsync(SelectedEventArgs<T> args)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Releases resources and performs cleanup when the calendar base is disposed.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            // Clear collections and reset state to default values; avoid assigning null to non-nullable fields.
            CustomizedDates?.Clear();
            CellListData = null;
            PreviousCellListData = null;
            UpdatePreviousCell = null;
            DisabledDayCellData?.Clear();
            CellDetailsData = null;
            PreviousDate = default;
            PreviousSelectedDate = default;
            PreviousDeSelectedDate = default;
            if (ChangedArgs is not null)
            {
                ChangedArgs.Event = default!;
                ChangedArgs.Element = default!;
                ChangedArgs.Value = default!;
                ChangedArgs.IsInteracted = false;
            }
            Element = default;
            return base.DisposeAsyncCore();
        }
    }
}
