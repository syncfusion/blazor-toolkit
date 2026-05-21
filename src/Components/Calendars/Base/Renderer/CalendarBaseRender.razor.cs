using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;
using System.Text;

namespace Syncfusion.Blazor.Toolkit.Calendars.Internal
{
    /// <summary>
    /// Provides base rendering functionality for the calendar, allowing users to interact with and select Gregorian or supported alternative calendar dates.
    /// </summary>
    /// <remarks>
    /// This class serves as the rendering backbone for calendar-based UI controls, such as <see cref="SfCalendar{TValue}"/>, and supports modern calendar experiences including keyboard navigation, range/selection options, and localization.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime"></SfCalendar>
    /// ]]></code>
    /// </example>
    public partial class CalendarBaseRender<TValue> : CalendarBase<TValue>
    {
        internal ElementReference TableBodyEle { get; set; }
        private bool _isShiftKey;
        private bool _isNotTabKey = true;
        private static readonly char[] _separatorChar = ['_'];
        private static readonly string[] _calendarKeys =
        [
            nameof(Start),
            nameof(Depth),
            nameof(FirstDayOfWeek),
            nameof(Min),
            nameof(Max),
            nameof(SfCalendar<TValue>.Values)
        ];

        private async Task OnCalendarKeyDownAsync(KeyboardEventArgs e)
        {
            _isNotTabKey = true;
            if (e is null || Parent is null)
            {
                return;
            }
            if (e.Key == "Tab")
            {
                _isNotTabKey = false;
            }
            string action = MapKeyToAction(e);
            CellDetails? focused = Parent.CellListData?.FirstOrDefault(c => !string.IsNullOrEmpty(c.ClassList) && c.ClassList.Contains("e-focused-date", StringComparison.CurrentCulture));
            CellDetails? selected = Parent.CellListData?.FirstOrDefault(c => !string.IsNullOrEmpty(c.ClassList) && c.ClassList.Contains("e-selected", StringComparison.CurrentCulture));
            KeyActions args = new()
            {
                Action = action ?? string.Empty,
                Key = e.Key ?? string.Empty,
                SelectDate = selected?.CellID ?? string.Empty,
                FocusedDate = focused?.CellID ?? string.Empty,
                ClassList = !string.IsNullOrEmpty(selected?.ClassList) ? selected.ClassList : (focused?.ClassList ?? string.Empty),
                ID = focused?.CellID ?? selected?.CellID ?? string.Empty,
                TargetClassList = CONTENT_TABLE,
                KeyCode = 0,
                IsLeftCalendar = false,
                FocusedDateClassList = focused?.ClassList ?? string.Empty,
                DateValue = focused is not null ? Intl.GetDateFormat(focused.CurrentDate, FORMAT_SHORT_DATE) : string.Empty
            };
            await TableBodyEle.FocusAsync().ConfigureAwait(false);
            await KeyActionHandlerAsync(args).ConfigureAwait(false);
        }

        private static string MapKeyToAction(KeyboardEventArgs e)
        {
            if (e is null || string.IsNullOrEmpty(e.Key))
            {
                return string.Empty;
            }
            bool ctrl = e.CtrlKey;
            bool shift = e.ShiftKey;
            return e.Key switch
            {
                "ArrowLeft" => MOVE_LEFT,
                "ArrowRight" => MOVE_RIGHT,
                "ArrowUp" when ctrl => CONTROL_UP,
                "ArrowDown" when ctrl => CONTROL_DOWN,
                "ArrowUp" => MOVE_UP,
                "ArrowDown" => MOVE_DOWN,
                "Enter" => SELECT,
                "Home" when ctrl => CONTROL_HOME,
                "End" when ctrl => CONTROL_END,
                "Home" => HOME,
                "End" => END,
                "PageUp" when shift => SHIFT_PAGE_UP,
                "PageDown" when shift => SHIFT_PAGE_DOWN,
                "PageUp" => PAGE_UP,
                "PageDown" => PAGE_DOWN,
                _ => e.Key ?? string.Empty,
            };
        }

        /// <summary>
        /// Invoked during the initial rendering phase of the <see cref="CalendarBaseRender{TValue}"/> component.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method initializes scripts and prepares the calendar rendering for user interaction. Overridden from <see cref="CalendarBase{TValue}"/>.
        /// </remarks>
        /// <example>
        /// This method is called automatically by the Blazor framework when the component is being initialized.
        /// </example>
        /// <exclude />
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
            CalendarBase_MultiValues = MultiValues;
            await RenderAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Handles dynamic property changes for the component.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// Invoked automatically by the Blazor runtime when parameters are set or receive updates, it coordinates state synchronization for dynamic calendar property changes such as <c>Start</c>, <c>Depth</c>, and <c>Values</c>, maintaining correct rendering and value updates.
        /// </remarks>
        /// <example>
        /// This method is called by the runtime when one or more component parameters change.
        /// </example>
        /// <summary>
        /// Handles dynamic property changes for the component.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// Invoked automatically by the Blazor runtime when parameters are set or receive updates.
        /// </remarks>
        /// <exclude />
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);
            await UpdateMultiValuesPropertyAsync().ConfigureAwait(false);
            await ProcessPropertyChangesAsync().ConfigureAwait(false);
            UpdateNavigationFlag();
        }

        /// <summary>
        /// Updates the MultiValues property if it has changed.
        /// </summary>
        private async Task UpdateMultiValuesPropertyAsync()
        {
            MultiValues = CalendarBase_MultiValues = await SfBaseUtils.UpdatePropertyAsync(MultiValues, CalendarBase_MultiValues, MultiValuesChanged).ConfigureAwait(false);
        }

        /// <summary>
        /// Processes changes to parent component properties.
        /// </summary>
        private async Task ProcessPropertyChangesAsync()
        {
            if (Parent is not null && Parent.PropertyChanges is not null && Parent.PropertyChanges.Count == 0)
            {
                return;
            }
            await HandleCalendarPropertyChangesAsync().ConfigureAwait(false);
            await HandleValuePropertyChangeAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Handles changes to calendar-specific properties like Start, Depth, FirstDayOfWeek, Min, Max, and Values.
        /// </summary>
        private async Task HandleCalendarPropertyChangesAsync()
        {
            if (Parent is null || Parent.PropertyChanges is null)
            {
                return;
            }
            List<string> changedKeys = [.. Parent.PropertyChanges.Keys];
            foreach (string key in changedKeys)
            {
                if (_calendarKeys.Contains(key))
                {
                    RemoveCalendarPropertyChange(key);
                    IsNavigation = true;
                    await UpdateAsync().ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Removes a property change from the parent component if it's a calendar component.
        /// </summary>
        private void RemoveCalendarPropertyChange(string key)
        {
            if (Parent as SfCalendar<TValue> is not null && Parent.PropertyChanges is not null)
            {
                _ = Parent.PropertyChanges.Remove(key);
            }
        }

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private async Task HandleValuePropertyChangeAsync()
        {
            if (Parent is not null && SfBaseUtils.Equals(Parent.Value, GenericValue(CurrentDate)))
            {
                return;
            }
            if (Parent as SfCalendar<TValue> is not null)
            {
                await UpdateCalendarValueAsync().ConfigureAwait(false);
            }
            else
            {
                await UpdateAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Updates the calendar when the value has changed.
        /// </summary>
        private async Task UpdateCalendarValueAsync()
        {
            ValidateDate();
            if (Parent is not null)
            {
                await MinMaxUpdateAsync(Parent.Value!).ConfigureAwait(false);
            }
            int currentView = GetViewNumber(CurrentView());
            SwitchView(currentView);
        }

        /// <summary>
        /// Updates the navigation flag if multiple dates are set programmatically.
        /// </summary>
        private void UpdateNavigationFlag()
        {
            if (Parent is not null && Parent.IsMultipleDatesSetProgrammatically())
            {
                IsNavigation = true;
            }
        }

        internal override async Task OnAfterScriptRenderedAsync()
        {
            if (Parent is SfCalendar<TValue>)
            {
                await UpdateIsDeviceModeAsync().ConfigureAwait(false);
                IsDeviceMode = SyncfusionService is not null && SyncfusionService.IsDeviceMode;
            }
        }

        private async Task RenderAsync()
        {
            InitializeRenderState();
            InitializeCurrentDate();
            ApplyIslamicCalendarDefaults();
            await ValidateAndUpdateCalendarAsync().ConfigureAwait(false);
            CreateHeader();
            CreateContent();
        }

        /// <summary>
        /// Initializes the basic rendering state.
        /// </summary>
        private void InitializeRenderState()
        {
            IsNavigation = false;
            PropertyType = typeof(TValue);
            TodayDate = DateTime.Now.Date;
        }

        /// <summary>
        /// Initializes the current date from parent value or current date value.
        /// </summary>
        private void InitializeCurrentDate()
        {
            DateTime defaultDate = DateTime.Now.Date;
            CurrentDate = Parent is not null && HasValidParentValue()
                ? ConvertDate(Parent.Value!)
                : CurrentDateValue is not null ? ConvertDate(CurrentDateValue) : defaultDate;
        }

        /// <summary>
        /// Checks if the parent has a valid value.
        /// </summary>
        private bool HasValidParentValue()
        {
            return Parent is not null && Parent.Value is not null && !SfBaseUtils.Equals(Parent.Value, default);
        }

        /// <summary>
        /// Applies default min/max date ranges for Islamic calendar mode.
        /// </summary>
        private void ApplyIslamicCalendarDefaults()
        {
            if (CalendarMode != CalendarType.Islamic || Parent is null)
            {
                return;
            }
            UpdateIslamicMinDate();
            UpdateIslamicMaxDate();
        }

        /// <summary>
        /// Updates the minimum date for Islamic calendar if it's at the Gregorian default.
        /// </summary>
        private void UpdateIslamicMinDate()
        {
            DateTime gregorianDefaultMin = new(GREGORIAN_DEFAULT_MIN_YEAR, FIRST_MONTH_OF_YEAR, FIRST_DAY_OF_MONTH);
            if (Parent is not null && Parent.Min.Date == gregorianDefaultMin.Date)
            {
                Parent.Min = new DateTime(ISLAMIC_MIN_YEAR, ISLAMIC_MIN_MONTH, ISLAMIC_MIN_DAY);
            }
        }

        /// <summary>
        /// Updates the maximum date for Islamic calendar if it's at the Gregorian default.
        /// </summary>
        private void UpdateIslamicMaxDate()
        {
            DateTime gregorianDefaultMax = new(GREGORIAN_DEFAULT_MAX_YEAR, GREGORIAN_DEFAULT_MAX_MONTH, GREGORIAN_DEFAULT_MAX_DAY);
            if (Parent is not null && Parent.Max.Date == gregorianDefaultMax.Date)
            {
                Parent.Max = new DateTime(ISLAMIC_MAX_YEAR, ISLAMIC_MAX_MONTH, ISLAMIC_MAX_DAY);
            }
        }

        /// <summary>
        /// Validates the current date and updates calendar state if needed.
        /// </summary>
        private async Task ValidateAndUpdateCalendarAsync()
        {
            ConvertHijriYearIfNeeded();
            ValidateDate();
            if (Parent is not null)
            {
                await MinMaxUpdateAsync(Parent.Value!).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Converts Hijri year to Gregorian if the current date is in the Hijri year range.
        /// </summary>
        private void ConvertHijriYearIfNeeded()
        {
            if (CalendarMode != CalendarType.Islamic)
            {
                return;
            }
            if (IsHijriYearInRange(CurrentDate.Year))
            {
                HijriDate hijriDate = new() { Year = CurrentDate.Year, Month = CurrentDate.Month, Date = CurrentDate.Day };
                CurrentDate = HijriParser.ToGregorian(hijriDate);
            }
        }

        /// <summary>
        /// Checks if a year is in the Hijri year range (1362-1492).
        /// </summary>
        private static bool IsHijriYearInRange(int year)
        {
            return year is > HIJRI_YEAR_RANGE_START and <= HIJRI_YEAR_RANGE_END;
        }

        private void CreateHeader()
        {
            TitleClass = LINK + SPACE + TITLE;
            ContentHeader = HEADER;
            PrevIconClass = PREV_ICON;
            NextIconClass = NEXT_ICON;
            PrevIconAttr = SfBaseUtils.UpdateDictionary(ARIADISABLED, FALSE, PrevIconAttr);
            NextIconAttr = SfBaseUtils.UpdateDictionary(ARIADISABLED, FALSE, NextIconAttr);
        }

        /// <summary>
        /// Validates and updates the current date based on value and min/max constraints.
        /// </summary>
        private void ValidateDate()
        {
            CurrentDate = (CurrentDate != default) ? CurrentDate : DateTime.Now.Date + new TimeSpan(0, 0, 0);
            if (Parent is null)
            {
                return;
            }
            if (Parent.Value is null || SfBaseUtils.Equals(Parent.Value, default))
            {
                return;
            }
            DateTime currentVal = MultiSelection ? CurrentDate : ConvertDate(Parent.Value);
            if (Parent.Min <= Parent.Max && currentVal >= Parent.Min && currentVal <= Parent.Max)
            {
                CurrentDate = currentVal;
            }
        }

        /// <summary>
        /// Updates the current calendar date based on the minimum and maximum value constraints.
        /// </summary>
        /// <param name="minMaxValue">The new date value to evaluate against the current range for validity (<c>TValue</c>).</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is responsible for ensuring the <c>CurrentDate</c> remains within the configured <c>Min</c> and <c>Max</c> property boundaries after a parameter or property change, and updates state accordingly for supported date types.
        /// </remarks>
        /// <example>
        /// Called when <c>Min</c> or <c>Max</c> is changed on the calendar to ensure the current value remains valid.
        /// </example>
        /// <exclude />
        protected async Task MinMaxUpdateAsync(TValue minMaxValue)
        {
            AdjustCurrentDateToMinMaxBounds(minMaxValue);
            if (HasValidValue(minMaxValue))
            {
                ClampValueToMinMaxBounds(minMaxValue);
            }
            else
            {
                UpdateMinMax(minMaxValue);
            }
            await Task.CompletedTask.ConfigureAwait(false);
        }

        private void AdjustCurrentDateToMinMaxBounds(TValue minMaxValue)
        {
            DateTime getValue = GetDateValue(minMaxValue);
            if (Parent is not null && ShouldClampToMin(getValue))
            {
                CurrentDate = Parent.Min;
            }
            else if (Parent is not null && ShouldClampToMax(getValue))
            {
                CurrentDate = Parent.Max;
            }
        }

        private DateTime GetDateValue(TValue value)
        {
            return (value is null || SfBaseUtils.Equals(value, default)) ? CurrentDate : ConvertDate(value);
        }

        private bool ShouldClampToMin(DateTime value)
        {
            return Parent is not null && value.Date != Parent.Min.Date && value <= Parent.Min && Parent.Min <= Parent.Max;
        }

        private bool ShouldClampToMax(DateTime value)
        {
            return Parent is not null && value.Date != Parent.Max.Date && value >= Parent.Max && Parent.Min <= Parent.Max;
        }

        private bool HasValidValue(TValue value)
        {
            return value is not null && !SfBaseUtils.Equals(value, default);
        }

        private void ClampValueToMinMaxBounds(TValue minMaxValue)
        {
            DateTime dateValue = GetClampedDateValue(minMaxValue);
            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) is not null;

            if (Parent is not null && IsValueBelowMin(dateValue))
            {
                SetMinMaxValue(Parent.Min, type, isNullable);
            }
            else if (Parent is not null && IsValueAboveMax(dateValue))
            {
                SetMinMaxValue(Parent.Max, type, isNullable);
            }
        }

        private DateTime GetClampedDateValue(TValue minMaxValue)
        {
            DateTime dateValue = ConvertDate(minMaxValue);
            return CalendarMode == CalendarType.Islamic ? GetDateValue(minMaxValue) : dateValue;
        }

        private bool IsValueBelowMin(DateTime dateValue)
        {
            return Parent is not null && dateValue < Parent.Min && Parent.Min <= Parent.Max;
        }

        private bool IsValueAboveMax(DateTime dateValue)
        {
            return Parent is not null && dateValue > Parent.Max && Parent.Min <= Parent.Max;
        }

        private void UpdateMinMax(TValue? dateValue)
        {
            DateTime currentVal = (dateValue is null || SfBaseUtils.Equals(dateValue, default)) ? CurrentDate : ConvertDate(dateValue);
            if ((Parent is not null) && Parent.Min <= Parent.Max && dateValue is not null && !SfBaseUtils.Equals(dateValue, default) && currentVal <= Parent.Max && currentVal >= Parent.Min)
            {
                CurrentDate = currentVal;
            }
            else
            {
                bool isMaxVal = Parent?.Min <= Parent?.Max && (dateValue is null || SfBaseUtils.Equals(dateValue, default)) && CurrentDate > Parent.Max;
                bool isMinVal = CurrentDate < Parent?.Min;
                CurrentDate = isMaxVal && Parent is not null ? Parent.Max : isMinVal && Parent is not null ? Parent.Min : CurrentDate;
            }
        }

        private void SetMinMaxValue(DateTime dateValue, Type type, bool isNullable)
        {
            bool isDateTimeOffset = type == typeof(DateTimeOffset) || (isNullable && Nullable.GetUnderlyingType(type) == typeof(DateTimeOffset));
            bool isDateOnly = type == typeof(DateOnly) || (isNullable && Nullable.GetUnderlyingType(type) == typeof(DateOnly));
            TValue? val = isDateTimeOffset ? (TValue)SfBaseUtils.ChangeType(new DateTimeOffset(dateValue), type)
                : isDateOnly ? (TValue)SfBaseUtils.ChangeType(new DateOnly(dateValue.Year, dateValue.Month, dateValue.Day), type)
                : (TValue)SfBaseUtils.ChangeType(dateValue, type);
            UpdateMinMax(val);
        }

        private void CreateContentBody()
        {
            switch (Parent?.Start)
            {
                case CalendarView.Year:
                    RenderYears();
                    break;
                case CalendarView.Decade:
                    RenderDecades();
                    break;
                case CalendarView.Month:
                    RenderMonths();
                    break;
                default:
                    break;
            }
        }

        internal void RenderMonths(MouseEventArgs? args = null)
        {
            LocalMainDate = [];
            CalendarView = CalendarView.Month;
            NumCells = WEEK_NUMBER;
            RenderDays(CurrentDate);
            RenderTemplate(NumCells, MONTH, args);
        }

        private void RenderYears(MouseEventArgs? args = null)
        {
            LocalMainDate = [];
            CalendarView = CalendarView.Year;
            CellsCount = YEAR_NUMBER;
            NumCells = CELL_ROW;
            LocalDate = new DateTime(CurrentDate.Year, FIRST_MONTH_OF_YEAR, CurrentDate.Day, CurrentDate.Hour, CurrentDate.Minute, CurrentDate.Second, CurrentDate.Millisecond);
            TitleUpdate(CurrentDate, MONTHS);
            RenderTemplate(NumCells, YEAR, args);
        }

        private void RenderDecades(MouseEventArgs? args = null)
        {
            LocalMainDate = [];
            CalendarView = CalendarView.Decade;
            CellsCount = YEAR_NUMBER;
            NumCells = CELL_ROW;
            LocalDate = new DateTime(CurrentDate.Year, FIRST_MONTH_OF_YEAR, FIRST_DAY_OF_MONTH, CurrentDate.Hour, CurrentDate.Minute, CurrentDate.Second, CurrentDate.Millisecond);
            UpdateDecadeTitle();
            RenderTemplate(DECADE_CELL_COUNT, DECADE, args);
        }

        /// <summary>
        /// Updates the decade view header title based on the current calendar mode.
        /// </summary>
        private void UpdateDecadeTitle()
        {
            int localYear = GetDecadeBaseYear();
            int startYear = CalculateDecadeStartYear(localYear);
            int endYear = startYear + (DECADE_MOD_VALUE - SINGLE_YEAR_OFFSET);
            string startHeaderYear = FormatDecadeYear(startYear);
            string endHeaderYear = FormatDecadeYear(endYear);
            HeaderTitle = startHeaderYear + TITLE_SEPARATOR + endHeaderYear;
            LocalDate = CreateDecadeStartDate(localYear);
        }

        /// <summary>
        /// Gets the base year for decade calculations, handling Islamic calendar if needed.
        /// </summary>
        private int GetDecadeBaseYear()
        {
            int year = CalendarMode == CalendarType.Islamic ? HijriParser.ToHijriDate(LocalDate).Year : LocalDate.Year;
            return year < MIN_YEAR_VALUE ? MIN_YEAR_VALUE : year;
        }

        /// <summary>
        /// Calculates the starting year of the decade.
        /// </summary>
        private static int CalculateDecadeStartYear(int localYear)
        {
            return localYear - (localYear % DECADE_MOD_VALUE);
        }

        /// <summary>
        /// Creates the starting date for the decade view.
        /// </summary>
        private static DateTime CreateDecadeStartDate(int localYear)
        {
            int decadeStart = localYear - (localYear % DECADE_MOD_VALUE) - DECADE_START_OFFSET;
            return new DateTime(decadeStart, FIRST_MONTH_OF_YEAR, FIRST_DAY_OF_MONTH);
        }

        /// <summary>
        /// Formats the decade year based on calendar mode.
        /// </summary>
        /// <param name="year">The year to format.</param>
        /// <returns>Formatted year string.</returns>
        private string FormatDecadeYear(int year)
        {
            int yearOffset = CalendarMode == CalendarType.Islamic ? YEAR_OFFSET_FOR_ISLAMIC : 0;
            return Intl.GetDateFormat(new DateTime(year + yearOffset, FIRST_MONTH_OF_YEAR, FIRST_DAY_OF_MONTH), FORMAT_YEAR);
        }

        private static string StartHeadYr(DateTime date)
        {
            DateTime datevalue = new(date.Year, 1, 1);
            int localYr = datevalue.Year;
            int startYr = localYr - (localYr % 10);
            string startHdrYr = Intl.GetDateFormat(new DateTime(startYr, 1, 1), FORMAT_YEAR);
            return startHdrYr;
        }

        private static string EndHeadYr(DateTime date)
        {
            DateTime datevalue = new(date.Year, 1, 1);
            int localYr = datevalue.Year;
            int endYr = localYr - (localYr % 10) + (10 - 1);
            string endHdrYr = Intl.GetDateFormat(new DateTime(endYr, 1, 1), FORMAT_YEAR);
            return endHdrYr;
        }

        /// <summary>
        /// Renders the days view of the calendar, calculating the first day to display based on calendar mode and first day of week.
        /// </summary>
        private void RenderDays(DateTime CurrentDate)
        {
            InitializeDayViewSettings(CurrentDate);
            CalculateStartingLocalDate(CurrentDate);
        }

        /// <summary>
        /// Initializes basic settings for the day view rendering.
        /// </summary>
        private void InitializeDayViewSettings(DateTime CurrentDate)
        {
            CellsCount = CELLCOUNT;
            LocalDate = CurrentDate;
            NumCells = WEEK_NUMBER;
            TitleUpdate(CurrentDate, DAYS);
        }

        /// <summary>
        /// Calculates the starting local date for the calendar grid based on first day of week and calendar mode.
        /// </summary>
        private void CalculateStartingLocalDate(DateTime CurrentDate)
        {
            if (Parent is null)
            {
                return;
            }
            int firstDayValue = Parent.FirstDayOfWeek;
            LocalDate = ShouldUseCustomFirstDay(firstDayValue)
                ? CalculateFirstDayWithCustomStart(firstDayValue)
                : CalculateFirstDayWithDefaultStart(CurrentDate);
        }

        /// <summary>
        /// Determines if a custom first day of week should be used.
        /// </summary>
        private bool ShouldUseCustomFirstDay(int firstDayValue)
        {
            return firstDayValue != 0 && CalendarMode != CalendarType.Islamic;
        }

        /// <summary>
        /// Calculates the first day to display when using a custom first day of week.
        /// </summary>
        private DateTime CalculateFirstDayWithCustomStart(int firstDayValue)
        {
            DateTime firstDayOfMonth = GetFirstDayOfCurrentMonth();
            while ((int)firstDayOfMonth.DayOfWeek != firstDayValue)
            {
                firstDayOfMonth = firstDayOfMonth.AddDays(-SINGLE_MONTH_OFFSET);
            }
            return firstDayOfMonth;
        }

        /// <summary>
        /// Gets the first day of the current month, handling Persian calendar if needed.
        /// </summary>
        private DateTime GetFirstDayOfCurrentMonth()
        {
            bool isPersianCulture = CultureInfo.CurrentCulture.Name.StartsWith(PERSIAN, StringComparison.Ordinal);
            return isPersianCulture
                ? GetPersianFirstDayOfMonth()
                : new DateTime(LocalDate.Year, LocalDate.Month, FIRST_DAY_OF_MONTH,
                LocalDate.Hour, LocalDate.Minute, LocalDate.Second);
        }

        /// <summary>
        /// Gets the first day of the month using the Persian calendar.
        /// </summary>
        private DateTime GetPersianFirstDayOfMonth()
        {
            PersianCalendar persianCalendar = new();
            int year = persianCalendar.GetYear(LocalDate);
            int month = persianCalendar.GetMonth(LocalDate);
            return new DateTime(year, month, FIRST_DAY_OF_MONTH, LocalDate.Hour, LocalDate.Minute, LocalDate.Second, LocalDate.Millisecond, persianCalendar);
        }

        /// <summary>
        /// Calculates the first day to display when using default first day of week (Sunday).
        /// </summary>
        private DateTime CalculateFirstDayWithDefaultStart(DateTime CurrentDate)
        {
            return CalendarMode == CalendarType.Islamic ? CalculateIslamicFirstDay(CurrentDate) : CalculateGregorianFirstDay();
        }

        /// <summary>
        /// Calculates the first day to display for Islamic calendar.
        /// </summary>
        private static DateTime CalculateIslamicFirstDay(DateTime CurrentDate)
        {
            HijriDate currentHijriDate = HijriParser.ToHijriDate(CurrentDate);
            HijriDate firstHijriDay = new() { Year = currentHijriDate.Year, Month = currentHijriDate.Month, Date = FIRST_DAY_OF_MONTH };
            DateTime firstDayOfMonth = HijriParser.ToGregorian(firstHijriDay);
            int dayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            return IsDefaultDate(firstDayOfMonth) ? firstDayOfMonth : firstDayOfMonth.AddDays(-dayOfWeek);
        }

        /// <summary>
        /// Calculates the first day to display for Gregorian calendar.
        /// </summary>
        private DateTime CalculateGregorianFirstDay()
        {
            DateTime firstDayOfMonth = new(LocalDate.Year, LocalDate.Month, FIRST_DAY_OF_MONTH, LocalDate.Hour, LocalDate.Minute, LocalDate.Second);
            int dayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            return IsDefaultDate(firstDayOfMonth) ? firstDayOfMonth : firstDayOfMonth.AddDays(-1 * dayOfWeek);
        }

        /// <summary>
        /// Checks if a date is the default DateTime value.
        /// </summary>
        private static bool IsDefaultDate(DateTime date)
        {
            return date.Date == default(DateTime).Date;
        }

        /// <summary>
        /// Determines whether two dates belong to the same visible calendar month for the active calendar mode.
        /// </summary>
        /// <param name="firstDate">The first date to compare.</param>
        /// <param name="secondDate">The second date to compare.</param>
        /// <returns><c>true</c> if both dates are in the same visible month; otherwise, <c>false</c>.</returns>
        internal bool IsSameCalendarMonth(DateTime firstDate, DateTime secondDate)
        {
            if (CalendarMode == CalendarType.Islamic)
            {
                HijriDate firstHijriDate = HijriParser.ToHijriDate(firstDate);
                HijriDate secondHijriDate = HijriParser.ToHijriDate(secondDate);
                return firstHijriDate.Year == secondHijriDate.Year && firstHijriDate.Month == secondHijriDate.Month;
            }

            if (CultureInfo.CurrentCulture.Name.StartsWith(PERSIAN, StringComparison.Ordinal))
            {
                PersianCalendar persianCalendar = new();
                return persianCalendar.GetYear(firstDate) == persianCalendar.GetYear(secondDate) && persianCalendar.GetMonth(firstDate) == persianCalendar.GetMonth(secondDate);
            }

            return firstDate.Year == secondDate.Year && firstDate.Month == secondDate.Month;
        }

        private void RenderTemplate(int count, string classNm, MouseEventArgs? args = null)
        {
            ContentElementClass = CONTENT + SPACE + classNm;
            ContentHeader = HEADER + SPACE + classNm;
            Row = count;
            Count = count;
            IconHandler();
            if (Parent is null)
            {
                return;
            }
            TValue? tempValue = Parent.Value is null ? default : (TValue)SfBaseUtils.ChangeType(Parent!.Value!, PropertyType!);
            Parent.ChangedArgs = new ChangedEventArgs<TValue> { Value = tempValue!, Values = MultiValues };
            Parent.ChangeHandler(args, MultiValues, MultiSelection);
        }

        private void IconHandler()
        {
            if (Parent is null)
            {
                return;
            }
            switch (CurrentView())
            {
                case MONTH_VIEW:
                    PreviousIconHandler(CompareMonth(CurrentDate, Parent.Min) < 1);
                    NextIconHandler(CompareMonth(CurrentDate, Parent.Max) > -1);
                    break;
                case YEAR_VIEW:
                    PreviousIconHandler(CompareDateVal(CurrentDate, Parent.Min, 0) < 1);
                    NextIconHandler(CompareDateVal(CurrentDate, Parent.Max, 0) > -1);
                    break;
                case DECADE_VIEW:
                    PreviousIconHandler(CompareDateVal(CurrentDate, Parent.Min, 10) < 1);
                    NextIconHandler(CompareDateVal(CurrentDate, Parent.Max, 10) > -1);
                    break;
                default:
                    break;
            }
        }

        private void PreviousIconHandler(bool disabled)
        {
            PrevIconClass = disabled ? SfBaseUtils.AddClass(PrevIconClass, DISABLED) : SfBaseUtils.RemoveClass(PrevIconClass, DISABLED);
            PrevIconClass = disabled ? SfBaseUtils.AddClass(PrevIconClass, OVERLAY) : SfBaseUtils.RemoveClass(PrevIconClass, OVERLAY);
            PrevIconAttr[ARIADISABLED] = disabled ? TRUE : FALSE;
        }

        /// <summary>
        /// Gets the currently active view (Month, Year, or Decade) for the calendar.
        /// </summary>
        /// <value>A <c>string</c> representing the current view. Possible values include <c>"Month"</c>, <c>"Year"</c>, and <c>"Decade"</c>.</value>
        /// <remarks>
        /// This property is used internally to track which calendar view (month, year, decade) is currently being displayed, affecting keyboard and navigation logic.
        /// </remarks>
        internal string CurrentView()
        {
            return (ContentElementClass is not null) && ContentElementClass.Contains(YEAR, StringComparison.Ordinal)
                ? YEAR_VIEW
                : (ContentElementClass is not null) && ContentElementClass.Contains(DECADE, StringComparison.Ordinal) ? DECADE_VIEW : MONTH_VIEW;
        }

        private void NextIconHandler(bool disabled)
        {
            NextIconClass = disabled ? SfBaseUtils.AddClass(NextIconClass, DISABLED) : SfBaseUtils.RemoveClass(NextIconClass, DISABLED);
            NextIconClass = disabled ? SfBaseUtils.AddClass(NextIconClass, OVERLAY) : SfBaseUtils.RemoveClass(NextIconClass, OVERLAY);
            NextIconAttr[ARIADISABLED] = disabled ? TRUE : FALSE;
        }

        internal static int GetViewNumber(string? stringVal)
        {
            return (stringVal == MONTH_VIEW) ? MONTH_VIEW_VAL : (stringVal == YEAR_VIEW) ? YEAR_VIEW_VAL : DECADE_VIEW_VAL;
        }

        internal void CreateContent()
        {
            ContentElement = true;
            ContentElementClass = CONTENT;
            CreateContentBody();
            if ((Parent is not null) && Parent.ShowTodayButton)
            {
                CreateContentFooter();
            }
        }

        internal async Task UpdateAsync()
        {
            IsSelect = false;
            ValidateDate();
            if (Parent is not null)
            {
                await MinMaxUpdateAsync(Parent.Value!).ConfigureAwait(false);
            }
            CreateContentBody();
        }

        /// <summary>
        /// Updates the calendar title based on the current date and view.
        /// </summary>
        /// <param name="CurrentDate">The current date to display.</param>
        /// <param name="view">The current calendar view (DAYS, MONTHS, or YEAR).</param>
        private void TitleUpdate(DateTime? CurrentDate, string view)
        {
            if (CurrentDate is null || string.IsNullOrEmpty(view))
            {
                HeaderTitle = string.Empty;
                return;
            }
            if (CalendarMode == CalendarType.Islamic)
            {
                UpdateIslamicTitle(CurrentDate.Value, view);
            }
            else
            {
                UpdateGregorianTitle(CurrentDate.Value, view);
            }
        }

        /// <summary>
        /// Updates the title for Islamic calendar mode.
        /// </summary>
        /// <param name="CurrentDate">The current date.</param>
        /// <param name="view">The current view.</param>
        private void UpdateIslamicTitle(DateTime CurrentDate, string view)
        {
            if (view == DAYS)
            {
                HeaderTitle = GetHijriHeaderTitle(CurrentDate);
            }
            else
            {
                HijriDate hijriDate = HijriParser.ToHijriDate(CurrentDate);
                DateTime currentHijriDate = new(hijriDate.Year, hijriDate.Month, 1);
                string formatTitle = view == DAYS ? FORMAT_MONTHS : FORMAT_YEAR;
                HeaderTitle = Intl.GetDateFormat(currentHijriDate, formatTitle);
            }
        }

        /// <summary>
        /// Updates the title for Gregorian calendar mode.
        /// </summary>
        /// <param name="CurrentDate">The current date.</param>
        /// <param name="view">The current view.</param>
        private void UpdateGregorianTitle(DateTime CurrentDate, string view)
        {
            string formatTitle = view == DAYS ? FORMAT_MONTHS : FORMAT_YEAR;
            HeaderTitle = Intl.GetDateFormat(CurrentDate, formatTitle);
        }

        // Helper function to get the Hijri header title based on the culture
        private string GetHijriHeaderTitle(DateTime date)
        {
            if (Parent is null)
            {
                return string.Empty;
            }
            HijriDate hijriDate = HijriParser.ToHijriDate(date);
            string monthName;
            if (CultureInfo.CurrentCulture.Name.StartsWith("ar", StringComparison.Ordinal))
            {
                monthName = new CultureInfo("ar-SA").DateTimeFormat.AbbreviatedMonthNames[hijriDate.Month - 1];
                return monthName + Intl.GetDateFormat(new DateTime(hijriDate.Year, 1, 1), FORMAT_YEAR);
            }
            else if (CultureInfo.CurrentCulture.Name.StartsWith("fr", StringComparison.Ordinal))
            {
                monthName = Parent._hijri_Month_Wide_French is not null && Parent._hijri_Month_Wide_French.Length >= hijriDate.Month
                            ? Parent._hijri_Month_Wide_French[hijriDate.Month - 1]
                            : string.Empty;
                return monthName + hijriDate.Year;
            }
            else
            {
                monthName = Parent._hijri_Month_Wide is not null && Parent._hijri_Month_Wide.Length >= hijriDate.Month
                            ? Parent._hijri_Month_Wide[hijriDate.Month - 1]
                            : string.Empty;
                return monthName + hijriDate.Year;
            }
        }

        internal void NavigatePreviousHandler(MouseEventArgs? args = null, bool isScroll = false)
        {
            if (!Disabled && (args is not null || isScroll) && Parent is not null)
            {
                Parent.IsTodayClick = false;
                NextPrevIconHandler(false);
            }
        }

        internal void NavigateNextHandler(MouseEventArgs? args = null, bool IsScroll = false)
        {
            if (!Disabled && (args is not null || IsScroll) && Parent is not null)
            {
                Parent.IsTodayClick = false;
                NextPrevIconHandler(true);
            }
        }

        private void TriggerNavigate(MouseEventArgs? args)
        {
            NavigatedEventArgs eventArgs = new()
            {
                Date = CurrentDate,
                View = CurrentView(),
                Event = args is null ? new() : args,
                Name = NAVIGATED,
            };
            IsSelect = false;
            Parent?.BindNavigateEvent(eventArgs);
        }

        internal void AddMonths(DateTime date, int index)
        {
            if (Parent is null)
            {
                return;
            }
            DateTime currentVal = date.AddMonths(index);
            currentVal = index == -1 ? currentVal >= Parent.Min ? currentVal : Parent.Min : currentVal <= Parent.Max ? currentVal : Parent.Max;
            if (Parent.Min <= Parent.Max && currentVal >= Parent.Min && currentVal <= Parent.Max)
            {
                CurrentDate = currentVal;
            }
        }

        /// <summary>
        /// Adds or subtracts years from the specified date within min/max boundaries.
        /// </summary>
        /// <param name="date">The base date to modify.</param>
        /// <param name="index">The number of years to add (positive) or subtract (negative).</param>
        /// <param name="dateValue">Optional date value for Islamic calendar day calculation.</param>
        internal void AddYears(DateTime date, int index, string? dateValue = null)
        {
            if (CalendarMode == CalendarType.Islamic)
            {
                HandleIslamicYearAddition(date, index, dateValue);
            }
            else
            {
                HandleGregorianYearAddition(date, index);
            }
        }

        /// <summary>
        /// Handles year addition for Gregorian calendar.
        /// </summary>
        /// <param name="date">The base date.</param>
        /// <param name="index">The year offset.</param>
        private void HandleGregorianYearAddition(DateTime date, int index)
        {
            DateTime currentVal = date.AddYears(index);
            currentVal = ApplyMinMaxConstraints(currentVal, index);
            if (Parent is not null && Parent.Min <= Parent.Max && currentVal >= Parent.Min && currentVal <= Parent.Max)
            {
                CurrentDate = currentVal;
            }
        }

        /// <summary>
        /// Handles year addition for Islamic (Hijri) calendar.
        /// </summary>
        /// <param name="date">The base date.</param>
        /// <param name="index">The year offset.</param>
        /// <param name="dateValue">Optional date value string.</param>
        private void HandleIslamicYearAddition(DateTime date, int index, string? dateValue)
        {
            int currentHijriDay = ParseHijriDay(dateValue);
            HijriDate islamicDate = HijriParser.ToHijriDate(date);
            HijriDate hijriDate = new() { Year = islamicDate.Year + index, Month = islamicDate.Month, Date = currentHijriDay };
            const int MAX_HIJRI_YEAR = 1490;
            if (hijriDate.Year > MAX_HIJRI_YEAR)
            {
                hijriDate = new HijriDate { Year = islamicDate.Year, Month = islamicDate.Month, Date = 1 };
            }
            CurrentDate = HijriParser.ToGregorian(hijriDate);
        }

        /// <summary>
        /// Parses the Hijri day value from a string.
        /// </summary>
        /// <param name="dateValue">The date value string to parse.</param>
        /// <returns>The parsed day value, or 1 if parsing fails.</returns>
        private int ParseHijriDay(string? dateValue)
        {
            return int.TryParse(dateValue, out int hijriDayFromValue) && hijriDayFromValue > 0 ? hijriDayFromValue : 1;
        }

        /// <summary>
        /// Applies min/max constraints to the calculated date value.
        /// </summary>
        /// <param name="currentVal">The current calculated value.</param>
        /// <param name="index">The index indicating direction (negative for past).</param>
        /// <returns>The constrained date value.</returns>
        private DateTime ApplyMinMaxConstraints(DateTime currentVal, int index)
        {
            if (Parent is null)
            {
                return new DateTime();
            }
            bool isBackwardNavigation = index is -1 or -10;
            return isBackwardNavigation ? (currentVal >= Parent.Min ? currentVal : Parent.Min)
                : (currentVal <= Parent.Max ? currentVal : Parent.Max);
        }

        internal void SwitchView(int view, MouseEventArgs? args = null)
        {
            switch (view)
            {
                case MONTH_VIEW_VAL:
                    IsNavigation = true;
                    RenderMonths(args);
                    break;
                case YEAR_VIEW_VAL:
                    IsNavigation = true;
                    RenderYears(args);
                    SetAnimation();
                    break;
                case DECADE_VIEW_VAL:
                    IsNavigation = true;
                    RenderDecades(args);
                    SetAnimation();
                    break;
                default:
                    break;
            }
            _ = InvokeAsync(StateHasChanged);
            TriggerNavigate(args);
        }

        private void SetAnimation()
        {
            if ((Parent is not null) && !Parent.IsTodayClick && !IsKeyboardSelect)
            {
                _ = InvokeVoidAsync(Parent._animationJsModule, Parent._animationJsInProcessModule, "animate", [TableBodyEle, Animate]).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles the next/previous navigation icon click logic.
        /// </summary>
        /// <param name="isNext">True to navigate forward; false to navigate backward.</param>
        private void NextPrevIconHandler(bool isNext)
        {
            int currentView = GetViewNumber(CurrentView());
            string view = CurrentView();
            switch (view)
            {
                case MONTH_VIEW:
                    HandleMonthViewNavigation(isNext);
                    SwitchView(currentView);
                    break;
                case YEAR_VIEW:
                    AddYears(CurrentDate, isNext ? 1 : -1);
                    SwitchView(currentView);
                    break;
                case DECADE_VIEW:
                    AddYears(CurrentDate, isNext ? 10 : -10);
                    SwitchView(currentView);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles month view navigation for Gregorian and Islamic calendars.
        /// </summary>
        /// <param name="isNext">True to navigate forward; false to navigate backward.</param>
        private void HandleMonthViewNavigation(bool isNext)
        {
            if (CalendarMode == CalendarType.Islamic)
            {
                NavigateIslamicMonth(isNext);
            }
            else
            {
                AddMonths(CurrentDate, isNext ? 1 : -1);
            }
        }

        /// <summary>
        /// Navigates Islamic calendar month forward or backward.
        /// </summary>
        /// <param name="isNext">True to navigate forward; false to navigate backward.</param>
        private void NavigateIslamicMonth(bool isNext)
        {
            HijriDate hpCurrentDate = HijriParser.ToHijriDate(CurrentDate);
            int monthOffset = isNext ? 1 : -1;
            HijriDate hijriDate = new() { Year = hpCurrentDate.Year, Month = hpCurrentDate.Month + monthOffset, Date = 1 };
            CurrentDate = HijriParser.ToGregorian(hijriDate);
        }

        internal void NavigateTitle()
        {
            if (!Disabled)
            {
                IsSelect = false;
                int currentView = GetViewNumber(CurrentView());
                IsKeyboardSelect = false;
                SwitchView(++currentView);
            }
        }

        private async Task ClickHandlerAsync(CellDetails args)
        {
            if (!Disabled)
            {
                if (IsDeviceMode)
                {
                    IsCellClicked = true;
                    ContentElement = false;
                }
                ValidateDate();
                if (Parent is not null)
                {
                    await MinMaxUpdateAsync(Parent.Value!).ConfigureAwait(false);
                }
                await CellClickAsync(args).ConfigureAwait(false);
            }
        }

        private async Task CellClickAsync(CellDetails args)
        {
            UpdatePreviousCellData();
            int view = GetViewNumber(CurrentView());
            string? classList = args.ClassList;
            if (IsOtherMonthCell(classList))
            {
                await ContentClickAsync(args, MONTH_VIEW_VAL).ConfigureAwait(false);
            }
            else if (IsDepthReached(view))
            {
                await ContentClickAsync(args, YEAR_VIEW_VAL).ConfigureAwait(false);
            }
            else if (view == DECADE_VIEW_VAL)
            {
                await ContentClickAsync(args, YEAR_VIEW_VAL).ConfigureAwait(false);
            }
            else if (IsCurrentMonthCell(classList, view))
            {
                await SelectDateAsync(args.EventArgs, args.CellID, MultiSelection, MultiValues, args, true).ConfigureAwait(false);
            }
            else
            {
                await ContentClickAsync(args, MONTH_VIEW_VAL).ConfigureAwait(false);
            }
        }

        private void UpdatePreviousCellData()
        {
            PreviousCellListData = UpdatePreviousCell ?? PreviousCellListData;
        }

        private bool IsOtherMonthCell(string? classList)
        {
            return classList is not null && classList.Contains(OTHER_MONTH, StringComparison.Ordinal);
        }

        private bool IsDepthReached(int view)
        {
            if (Parent is null)
            {
                return false;
            }
            int depthView = GetViewNumber(Parent.Depth.ToString());
            int startView = GetViewNumber(Parent.Start.ToString());
            return view == depthView && startView >= depthView;
        }

        private bool IsCurrentMonthCell(string? classList, int view)
        {
            return classList is not null && !classList.Contains(OTHER_MONTH, StringComparison.Ordinal) && view == MONTH_VIEW_VAL;
        }

        internal async Task ContentClickAsync(CellDetails args, int view, bool dateRangeKeyBoardAction = false)
        {
            int currentView = GetViewNumber(CurrentView());
            DateTime curDate = GetClickedDate(dateRangeKeyBoardAction, args);
            bool isDepthView = IsDepthViewReached(currentView);
            if (view == MONTH_VIEW_VAL)
            {
                await HandleMonthViewClickAsync(args, curDate, isDepthView).ConfigureAwait(false);
            }
            else if (view == YEAR_VIEW_VAL)
            {
                await HandleYearViewClickAsync(args, curDate, isDepthView).ConfigureAwait(false);
            }
        }

        private DateTime GetClickedDate(bool dateRangeKeyBoardAction, CellDetails args)
        {
            return IsKeyboardSelect || dateRangeKeyBoardAction ? CurrentDate : GetIdValue(args);
        }

        private bool IsDepthViewReached(int currentView)
        {
            string? depth = Parent?.Depth.ToString();
            string? start = Parent?.Start.ToString();
            return currentView == GetViewNumber(depth) && GetViewNumber(start) >= GetViewNumber(depth);
        }

        private async Task HandleMonthViewClickAsync(CellDetails args, DateTime curDate, bool isDepthView)
        {
            if (isDepthView)
            {
                await ProcessMonthViewDepthSelectionAsync(args, curDate).ConfigureAwait(false);
            }
            else
            {
                UpdateContentDate(curDate);
            }
            await TriggerMonthViewNavigationAsync(args.EventArgs).ConfigureAwait(false);
        }

        private async Task ProcessMonthViewDepthSelectionAsync(CellDetails args, DateTime curDate)
        {
            CurrentDate = curDate;
            List<DateTime> copyValues = [.. CopyValues(MultiValues)];
            if (MultiSelection && Parent is not null && !Parent.CheckPresentDate(curDate, MultiValues))
            {
                HandleMultiSelectionAdd(curDate, copyValues);
                Parent.ChangeHandler(args.EventArgs, MultiValues, MultiSelection);
            }
            else if (Parent is not null && ShouldRemoveFromSelection(curDate))
            {
                RemoveFromSelection(curDate, copyValues);
                Parent.ChangeHandler(args.EventArgs, MultiValues, MultiSelection);
            }
            await UpdateCalendarValuesAsync(copyValues).ConfigureAwait(false);
        }

        private void HandleMultiSelectionAdd(DateTime curDate, List<DateTime> copyValues)
        {
            bool isDefaultValue = Parent is not null && Parent.Value is not null && !SfBaseUtils.Equals(Parent.Value, default);
            if (isDefaultValue && Parent is not null)
            {
                DateTime changeValue = (DateTime)SfBaseUtils.ChangeType(Parent.Value!, typeof(TValue));
                if (!copyValues.Contains(changeValue))
                {
                    copyValues.Add(changeValue);
                }
            }
            copyValues.Add(curDate);
        }

        private bool ShouldRemoveFromSelection(DateTime curDate)
        {
            return MultiSelection && MultiValues is not null && MultiValues.Length > 0 && Parent is not null && CheckPresentDate(curDate, MultiValues);
        }

        private void RemoveFromSelection(DateTime curDate, List<DateTime> copyValues)
        {
            for (int tempIndex = 0; tempIndex < copyValues.Count; tempIndex++)
            {
                if (copyValues[tempIndex].Date == curDate)
                {
                    copyValues.RemoveAt(tempIndex);
                    break;
                }
            }
        }

        private async Task UpdateCalendarValuesAsync(List<DateTime> copyValues)
        {
            if (Parent is null)
            {
                return;
            }
            MultiValues = [.. copyValues];
            await Parent.UpdateCalendarPropertyAsync(CALENDAR_BASE_VALUES, copyValues.ToArray()).ConfigureAwait(false);
            if (MultiSelection)
            {
                TValue? selectValues = MultiValues.Length > 0 ? GenericValue(MultiValues[^1]) : default;
                await Parent.UpdateCalendarPropertyAsync(VALUE, selectValues).ConfigureAwait(false);
            }
            else
            {
                await Parent.UpdateCalendarPropertyAsync(VALUE, GenericValue(CurrentDate)).ConfigureAwait(false);
            }
        }

        private void UpdateContentDate(DateTime curDate)
        {
            DateTime contentDate = new(curDate.Year, curDate.Month, curDate.Day, curDate.Hour, curDate.Minute, curDate.Second, curDate.Millisecond);
            if (curDate.Month > 0 && CurrentDate.Month != curDate.Month)
            {
                contentDate = new DateTime(contentDate.Year, contentDate.Month, curDate.Day, curDate.Hour, curDate.Minute, curDate.Second, curDate.Millisecond);
            }
            contentDate = new DateTime(curDate.Year, contentDate.Month, contentDate.Day, curDate.Hour, curDate.Minute, curDate.Second, curDate.Millisecond);
            CurrentDate = contentDate;
        }

        private async Task TriggerMonthViewNavigationAsync(MouseEventArgs? eventArgs)
        {
            await InvokeVoidAsync(Parent!._animationJsModule!, Parent._animationJsInProcessModule!, "animate", [TableBodyEle, Animate]).ConfigureAwait(true);
            IsNavigation = true;
            RenderMonths(eventArgs);
            TriggerNavigate(eventArgs);
        }

        private async Task HandleYearViewClickAsync(CellDetails args, DateTime curDate, bool isDepthView)
        {
            if (isDepthView)
            {
                await SelectDateAsync(args.EventArgs, args.CellID, MultiSelection, MultiValues, args, true).ConfigureAwait(false);
            }
            else
            {
                UpdateYearViewDate(curDate);
                await TriggerYearViewNavigationAsync(args.EventArgs).ConfigureAwait(false);
            }
        }

        private void UpdateYearViewDate(DateTime curDate)
        {
            if (CalendarMode == CalendarType.Islamic)
            {
                CurrentDate = curDate;
                IslamicYearClick = true;
            }
            else
            {
                CurrentDate = AdjustForLeapYear(curDate);
            }
        }

        private DateTime AdjustForLeapYear(DateTime curDate)
        {
            int day = (curDate.Month == 2 && curDate.Day == 29 && !DateTime.IsLeapYear(curDate.Year)) ? 28 : curDate.Day;
            return new DateTime(curDate.Year, curDate.Month, day, CurrentDate.Hour, CurrentDate.Minute, CurrentDate.Second, CurrentDate.Millisecond, curDate.Kind);
        }

        private async Task TriggerYearViewNavigationAsync(MouseEventArgs? eventArgs)
        {
            await InvokeVoidAsync(Parent!._animationJsModule!, Parent._animationJsInProcessModule!, "animate", [TableBodyEle, Animate]).ConfigureAwait(true);
            IsNavigation = true;
            RenderYears();
            TriggerNavigate(eventArgs);
        }

        private async Task SetDateDecadeAsync(DateTime date, int year)
        {
            int day = (date.Month == 2 && date.Day == 29 && !DateTime.IsLeapYear(year)) ? 28 : date.Day;
            date = new DateTime(year, date.Month, day);
            if (Parent is not null)
            {
                await Parent.UpdateCalendarPropertyAsync(VALUE, GenericValue(date)).ConfigureAwait(false);
            }
        }

        private async Task SetDateYearAsync(DateTime dateValue)
        {
            int dayValue = Parent is not null && Parent.Value is not null ? dateValue.Day : DateTime.DaysInMonth(dateValue.Year, dateValue.Month);
            DateTime date = new(dateValue.Year, dateValue.Month, dayValue);
            date = (dateValue.Month != date.Month) ? new DateTime(dateValue.Year, dateValue.Month, 0) : date;
            if (Parent is not null)
            {
                await Parent.UpdateCalendarPropertyAsync(VALUE, GenericValue(date)).ConfigureAwait(false);
            }
        }

        private async Task SelectedDateValueAsync(TValue selectedValue)
        {
            if ((Parent is not null && !SfBaseUtils.Equals(selectedValue, Parent.PreviousSelectedDate)) || (Parent is not null && SfBaseUtils.Equals(selectedValue, Parent.PreviousDeSelectedDate)))
            {
                SelectedEventArgs<TValue> selectEventArgs = new() { Value = selectedValue };
                await Parent.InvokeSelectEventAsync(selectEventArgs).ConfigureAwait(false);
                Parent.PreviousSelectedDate = selectedValue;
            }
        }

        private async Task SelectDateAsync(MouseEventArgs? events, string cellId, bool multiSelection, DateTime[]? values, CellDetails? args = null, bool isSelection = false)
        {
            if (CellClickHandler.HasDelegate)
            {
                await HandleCellClickCallbackAsync(cellId, args).ConfigureAwait(false);
                return;
            }
            DateTime date = ParseCellIdToDate(cellId);
            await ProcessDateSelectionAsync(events, date, multiSelection, values, isSelection).ConfigureAwait(false);
        }

        private async Task HandleCellClickCallbackAsync(string cellId, CellDetails? args)
        {
            args ??= new CellDetails() { CellID = cellId };
            CurrentDate = args.CurrentDate;
            await CellClickHandler.InvokeAsync(args).ConfigureAwait(false);
        }

        private DateTime ParseCellIdToDate(string cellId)
        {
            if (string.IsNullOrEmpty(cellId))
            {
                return CurrentDate;
            }
            long id = long.Parse(cellId.Split(_separatorChar)[0], CultureInfo.CurrentCulture);
            return new DateTime(id);
        }

        private async Task ProcessDateSelectionAsync(MouseEventArgs? events, DateTime date, bool multiSelection, DateTime[]? values, bool isSelection)
        {
            if (Parent is null)
            {
                return;
            }
            Parent.IsTodayClick = false;
            if (CurrentView() == DECADE_VIEW)
            {
                await SetDateDecadeAsync(CurrentDate, date.Year).ConfigureAwait(false);
            }
            else if (CurrentView() == YEAR_VIEW)
            {
                await SetDateYearAsync(date).ConfigureAwait(false);
            }
            else
            {
                await HandleMonthViewSelectionAsync(date, multiSelection, values).ConfigureAwait(false);
            }
            await HandleMultiSelectionRemovalAsync(date, multiSelection, values).ConfigureAwait(false);
            FinalizeSelection(events, date, multiSelection, isSelection);
        }

        private async Task HandleMonthViewSelectionAsync(DateTime date, bool multiSelection, DateTime[]? values)
        {

            if (multiSelection && Parent is not null && !CheckPresentDate(date, values))
            {
                await AddToMultiSelectionAsync(date, values).ConfigureAwait(false);
            }
            else
            {
                await SetSingleDateValueAsync(date, multiSelection).ConfigureAwait(false);
            }
        }

        private async Task AddToMultiSelectionAsync(DateTime date, DateTime[]? values)
        {
            if (Parent is null)
            {
                return;
            }
            List<DateTime> copyValues = [.. CopyValues(values)];
            bool isDefaultValue = Parent.Value is not null && !SfBaseUtils.Equals(Parent.Value, default);
            if (isDefaultValue)
            {
                DateTime changeValue = (DateTime)SfBaseUtils.ChangeType(Parent.Value!, typeof(TValue));
                if (!copyValues.Contains(changeValue))
                {
                    copyValues.Add(changeValue);
                }
            }
            copyValues.Add(date);
            MultiValues = [.. copyValues];
            await Parent.UpdateCalendarPropertyAsync(CALENDAR_BASE_VALUES, copyValues.ToArray()).ConfigureAwait(false);
            TValue? selectedValue = GenericValue(copyValues[^1]);
            await SelectedDateValueAsync(selectedValue!).ConfigureAwait(false);
            await Parent.UpdateCalendarPropertyAsync(VALUE, selectedValue).ConfigureAwait(false);
        }

        private async Task SetSingleDateValueAsync(DateTime date, bool multiSelection)
        {
            TValue? selectedValue = GenericValue(date);
            if (!multiSelection)
            {
                await SelectedDateValueAsync(selectedValue!).ConfigureAwait(false);
            }
            if (CalendarMode == CalendarType.Islamic && Parent is not null && Parent.Value is not null)
            {
                await SetIslamicDateValueAsync(date).ConfigureAwait(false);
            }
            else if (Parent is not null)
            {
                await Parent.UpdateCalendarPropertyAsync(VALUE, selectedValue).ConfigureAwait(false);
            }
        }

        private async Task SetIslamicDateValueAsync(DateTime date)
        {
            if (Parent is null)
            {
                return;
            }
            DateTime parentDate = ConvertDate(Parent.Value!);
            DateTime adjustedDate = date.AddHours(parentDate.TimeOfDay.TotalHours);
            await Parent.UpdateCalendarPropertyAsync(VALUE, GenericValue(adjustedDate)).ConfigureAwait(false);
        }

        private async Task HandleMultiSelectionRemovalAsync(DateTime date, bool multiSelection, DateTime[]? values)
        {
            if (!ShouldRemoveFromMultiSelection(multiSelection, values, date) || Parent is null)
            {
                return;
            }
            List<DateTime> copyValues = [.. CopyValues(values)];
            await RemoveMatchingDateAsync(date, copyValues).ConfigureAwait(false);
            MultiValues = [.. copyValues];
            await Parent.UpdateCalendarPropertyAsync(CALENDAR_BASE_VALUES, copyValues.ToArray()).ConfigureAwait(false);
            TValue? selectValues = MultiValues.Length > 0 ? GenericValue(MultiValues[^1]) : default;
            await Parent.UpdateCalendarPropertyAsync(VALUE, selectValues).ConfigureAwait(false);
        }

        private bool ShouldRemoveFromMultiSelection(bool multiSelection, DateTime[]? values, DateTime date)
        {
            return multiSelection && values is not null && values.Length > 0 && values.Any(d => d.Date == date.Date);
        }

        private async Task RemoveMatchingDateAsync(DateTime date, List<DateTime> copyValues)
        {
            string localDateString = Intl.GetDateFormat(date, FORMAT_SHORT_DATE);
            for (int item = 0; item < copyValues.Count; item++)
            {
                string tempDateString = Intl.GetDateFormat(copyValues[item], FORMAT_SHORT_DATE);
                if (localDateString == tempDateString)
                {
                    await InvokeDeselectionEventAsync(copyValues[item]).ConfigureAwait(false);
                    copyValues.RemoveAt(item);
                    break;
                }
            }
        }

        private async Task InvokeDeselectionEventAsync(DateTime dateValue)
        {
            TValue? deselectedValue = GenericValue(dateValue);
            if (Parent is null)
            {
                return;
            }
            if (SfBaseUtils.Equals(deselectedValue, Parent.PreviousDeSelectedDate) && !SfBaseUtils.Equals(deselectedValue, Parent.PreviousSelectedDate))
            {
                return;
            }
            DeSelectedEventArgs<TValue> deselectEventArgs = new() { Value = deselectedValue! };
            await Parent.InvokeDeSelectEventAsync(deselectEventArgs).ConfigureAwait(false);
            Parent.PreviousDeSelectedDate = deselectedValue;
        }

        private void FinalizeSelection(MouseEventArgs? events, DateTime date, bool multiSelection, bool isSelection)
        {
            IsSelect = true;
            IsNavigation = true;
            CurrentDate = date;
            if (CurrentView() == DECADE_VIEW)
            {
                UpdateDecadeSelection();
            }
            InvokeChangeHandler(events, multiSelection, isSelection);
        }

        private void UpdateDecadeSelection()
        {
            LocalDate = new DateTime(CurrentDate.Year, 1, 1, CurrentDate.Hour, CurrentDate.Minute, CurrentDate.Second, CurrentDate.Millisecond);
            UpdateDecadeTitle();
        }

        private void InvokeChangeHandler(MouseEventArgs? events, bool multiSelection, bool isSelection)
        {
            if (Parent is null)
            {
                return;
            }
            TValue? tempValue = IsNullValue(Parent.Value!) ? default! : (TValue)SfBaseUtils.ChangeType(Parent.Value!, PropertyType);
            Parent.ChangedArgs = new ChangedEventArgs<TValue> { Value = tempValue!, Values = MultiValues };
            Parent.ChangeHandler(events, MultiValues, multiSelection, isSelection);
        }

        private static bool IsNullValue(TValue DateValue)
        {
            return DateValue is null;
        }

        private DateTime GetIdValue(CellDetails args)
        {
            long id = long.Parse(args.CellID.Split(_separatorChar)[0], CultureInfo.CurrentCulture);
            string dateString = Intl.GetDateFormat(new DateTime(id), FORMAT_FULL_DATE);
            CultureInfo cultureInfo = Intl.GetCulture();
            bool checkDateValue = DateTime.TryParseExact(dateString, FORMAT_FULL_DATE, cultureInfo, DateTimeStyles.AllowWhiteSpaces, out _);
            DateTime date = checkDateValue ? DateTime.ParseExact(dateString, FORMAT_FULL_DATE, cultureInfo) : new DateTime(id);
            return date;
        }

        private void CreateContentFooter()
        {
            if (Localizer is null)
            {
                return;
            }
            TodayEleContent = Localizer[TODAY_LOCALE_KEY];
            TodayEleClass = BTN + SPACE + SPACE + TODAY + SPACE + FLAT + SPACE + PRIMARY + SPACE + CSS;
            if ((Parent is not null) && !(Parent.Min.Date <= TodayDate && TodayDate <= Parent.Max.Date))
            {
                TodayEleClass = SfBaseUtils.AddClass(TodayEleClass, DISABLED);
            }
        }

        private async Task TodayButtonClickAsync(MouseEventArgs? args = null)
        {
            if (!Disabled && Parent is not null)
            {
                if (CurrentView() != Parent?.Depth.ToString())
                {
                    await InvokeVoidAsync(Parent!._animationJsModule!, Parent._animationJsInProcessModule!, "animate", [TableBodyEle, Animate]).ConfigureAwait(true);
                }
                Parent.IsTodayClick = true;
                DateTime tempValue = GenerateTodayVal(Parent.Value!);
                if (Parent is not null)
                {
                    await Parent.UpdateCalendarPropertyAsync(VALUE, GenericValue(tempValue)).ConfigureAwait(false);
                }
                if (MultiSelection)
                {
                    List<DateTime> copyValues = [.. CopyValues(MultiValues)];

                    if (Parent is not null && !CheckPresentDate(tempValue, MultiValues))
                    {
                        copyValues.Add(tempValue);
                        MultiValues = [.. copyValues];
                        await Parent.UpdateCalendarPropertyAsync(CALENDAR_BASE_VALUES, copyValues.ToArray()).ConfigureAwait(false);
                    }
                }
                await BaseTodayButtonClickAsync(tempValue, args).ConfigureAwait(false);
            }
        }

        private async Task TodayButtonKeyDownAsync(KeyboardEventArgs? args = null)
        {
            if (args is null || !string.Equals(args.Key, "Tab", StringComparison.Ordinal))
            {
                if (args is not null && string.Equals(args.Key, "Shift", StringComparison.Ordinal))
                {
                    _isShiftKey = true;
                }
                return;
            }

            if (_isShiftKey && string.Equals(args.Key, "Tab", StringComparison.Ordinal))
            {
                _isShiftKey = false;
                return;
            }

            // If there is no parent or the parent is the standalone SfCalendar, nothing to hide.
            if (Parent is null or SfCalendar<TValue>)
            {
                return;
            }

            if (Parent is SfDatePicker<TValue> datePicker)
            {
                await datePicker.FocusAsync().ConfigureAwait(true);
                await datePicker.HidePopupAsync().ConfigureAwait(true);
            }
            else if (Parent is SfDateTimePicker<TValue> dateTimePicker)
            {
                await dateTimePicker.HidePopupAsync().ConfigureAwait(false);
            }
        }

        private async Task BaseTodayButtonClickAsync(DateTime dateValue, MouseEventArgs? args = null)
        {
            IsSelect = false;
            TValue? dateVal = GenericValue(dateValue);
            if (Parent is not null && GetViewNumber(Parent.Start.ToString()) >= GetViewNumber(Parent.Depth.ToString()))
            {
                await NavigateToAsync(Parent.Depth, dateVal!, args).ConfigureAwait(false);
            }
            else
            {
                if (Parent is not null)
                {
                    await NavigateToAsync(Parent.Depth, dateVal!, args).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// This method is used to navigate to the month/year/decade view of the Calendar.
        /// </summary>
        internal async Task NavigateToAsync(CalendarView view, TValue dateValue, MouseEventArgs? args = null)
        {
            if (Parent is null)
            {
                return;
            }
            DateTime date = ConvertDate(dateValue);
            await MinMaxUpdateAsync(Parent.Value!).ConfigureAwait(false);
            UpdateCurrentDateWithinBounds(date);
            CalendarView effectiveView = DetermineEffectiveView(view);
            SwitchView(GetViewNumber(effectiveView.ToString()), args);
        }

        /// <summary>
        /// Updates the current date ensuring it stays within min/max bounds.
        /// </summary>
        private void UpdateCurrentDateWithinBounds(DateTime date)
        {
            if (Parent is null)
            {
                return;
            }
            CurrentDate = IsDateWithinBounds(date) ? date : ClampDateToBounds(date);
        }

        /// <summary>
        /// Checks if a date is within the min/max bounds.
        /// </summary>
        private bool IsDateWithinBounds(DateTime date)
        {
            return Parent is not null && date >= Parent.Min && date <= Parent.Max;
        }

        /// <summary>
        /// Clamps a date to the min/max bounds.
        /// </summary>
        private DateTime ClampDateToBounds(DateTime date)
        {
            return Parent is null ? CurrentDate : date <= Parent.Min ? Parent.Min : date > Parent.Max ? Parent.Max : CurrentDate;
        }

        /// <summary>
        /// Determines the effective view based on depth and start constraints.
        /// </summary>
        private CalendarView DetermineEffectiveView(CalendarView requestedView)
        {
            if (Parent is null)
            {
                return CalendarView.Month;
            }
            string depth = Parent.Depth.ToString();
            string calView = requestedView.ToString();
            if (string.IsNullOrEmpty(depth))
            {
                return requestedView;
            }
            int depthNumber = GetViewNumber(depth);
            int viewNumber = GetViewNumber(calView);
            return !ShouldAdjustView(depthNumber, viewNumber) ? requestedView
                : ShouldUseParentDepth(depthNumber, viewNumber) ? Parent.Depth : requestedView;
        }

        /// <summary>
        /// Determines if the view should be adjusted based on depth.
        /// </summary>
        private static bool ShouldAdjustView(int depthNumber, int viewNumber)
        {
            return depthNumber >= viewNumber;
        }

        /// <summary>
        /// Determines if the parent depth should be used instead of requested view.
        /// </summary>
        private bool ShouldUseParentDepth(int depthNumber, int viewNumber)
        {
            if (Parent is null)
            {
                return false;
            }
            int startNumber = GetViewNumber(Parent.Start.ToString());
            return (depthNumber <= startNumber) || (depthNumber == viewNumber);
        }

        private DateTime GenerateTodayVal(TValue dateValue)
        {
            DateTime changeValue = (dateValue is not null) ? ConvertDate(dateValue) : DateTime.Now;
            return Parent is not null && Parent.Value is not null && !SfBaseUtils.Equals(Parent.Value, default)
                ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, changeValue.Hour, changeValue.Minute, changeValue.Second, changeValue.Millisecond)
                : new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
        }

        private static int CompareMonth(DateTime start, DateTime end)
        {
            int getStartVal = (start.Month == end.Month) ? 0 : (start.Month > end.Month) ? 1 : -1;
            return (start.Year > end.Year) ? 1 : (start.Year < end.Year) ? -1 : getStartVal;
        }

        private static int CompareDateVal(DateTime startDate, DateTime endDate, int modifier)
        {
            int start = endDate.Year;
            int end = start;
            if (modifier > 0)
            {
                start -= start % modifier;
                end = start - (start % modifier) + modifier - 1;
            }
            return (startDate.Year > end) ? 1 : (startDate.Year < start) ? -1 : 0;
        }

        internal async Task KeyActionHandlerAsync(KeyActions args)
        {
            if (Disabled || Parent is null)
            {
                return;
            }
            PrepareKeyboardNavigation();
            int view = GetViewNumber(CurrentView());
            int depthValue = GetViewNumber(Parent.Depth.ToString());
            bool levelRestrict = view == depthValue && GetViewNumber(Parent.Start.ToString()) >= depthValue;
            CellDetails eventArgs = CreateCellDetails(args);
            await ProcessKeyActionAsync(args, view, levelRestrict, eventArgs).ConfigureAwait(false);
        }

        private void PrepareKeyboardNavigation()
        {
            IsSelect = false;
            IsKeyboardSelect = true;
        }

        private CellDetails CreateCellDetails(KeyActions args)
        {
            return new CellDetails() { CellID = args.ID, ClassList = args.ClassList, CurrentDate = CurrentDate, Element = null, EventArgs = args.Events };
        }

        private async Task ProcessKeyActionAsync(KeyActions args, int view, bool levelRestrict, CellDetails eventArgs)
        {
            switch (args.Action)
            {
                case MOVE_LEFT:
                case MOVE_RIGHT:
                    HandleLeftRightKeys(args, view);
                    break;
                case MOVE_UP:
                case MOVE_DOWN:
                    HandleUpDownKeys(args, view);
                    break;
                case SELECT:
                    await HandleSelectKeyAsync(args, eventArgs, levelRestrict, view).ConfigureAwait(false);
                    break;
                case CONTROL_UP:
                    NavigateTitle();
                    break;
                case CONTROL_DOWN:
                    await ControlDownKeyActionAsync(args, levelRestrict, eventArgs, view).ConfigureAwait(false);
                    break;
                case HOME:
                    HandleHomeKey(view, args.DateValue);
                    break;
                case END:
                    HandleEndKey(view);
                    break;
                case PAGE_UP:
                case PAGE_DOWN:
                    await HandlePageKeysAsync(args).ConfigureAwait(false);
                    break;
                case SHIFT_PAGE_UP:
                case SHIFT_PAGE_DOWN:
                    await HandleShiftPageKeysAsync(args).ConfigureAwait(false);
                    break;
                case CONTROL_HOME:
                case CONTROL_END:
                    await HandleControlHomeEndKeysAsync(args).ConfigureAwait(false);
                    break;
                default:
                    break;
            }
        }

        private void HandleLeftRightKeys(KeyActions args, int view)
        {
            if (args.TargetClassList is not null && args.TargetClassList.Contains(CONTENT_TABLE, StringComparison.Ordinal))
            {
                int mouseNavValue = args.Action == MOVE_LEFT ? -1 : 1;
                KeyboardNavigate(mouseNavValue, view);
            }
        }

        private void HandleUpDownKeys(KeyActions args, int view)
        {
            if (args.TargetClassList is not null && args.TargetClassList.Contains(CONTENT_TABLE, StringComparison.Ordinal))
            {
                int cellRow = args.Action == MOVE_UP ? -CELL_ROW : CELL_ROW;
                int weekNumber = args.Action == MOVE_UP ? -WEEK_NUMBER : WEEK_NUMBER;
                KeyboardNavigate((view == 0) ? weekNumber : cellRow, view);
            }
        }

        private async Task HandleSelectKeyAsync(KeyActions args, CellDetails eventArgs, bool levelRestrict, int view)
        {
            if (eventArgs.CellID is not null)
            {
                await SelectKeyActionAsync(args, eventArgs, levelRestrict, view).ConfigureAwait(false);
            }
        }

        private void HandleHomeKey(int view, string dateValue)
        {
            if (CalendarMode == CalendarType.Islamic)
            {
                IslamicHomeKeyAction(view, dateValue);
            }
            else
            {
                HomeKeyAction(view);
            }
        }

        private void HandleEndKey(int view)
        {
            if (CalendarMode == CalendarType.Islamic)
            {
                IslamicEndKeyAction(view);
            }
            else
            {
                EndKeyAction(view);
            }
        }

        private async Task HandlePageKeysAsync(KeyActions args)
        {
            if (CalendarMode == CalendarType.Islamic)
            {
                await IslamicPageKeyActionAsync(args.Action, args.DateValue).ConfigureAwait(false);
            }
            else
            {
                await PageKeyActionAsync(args.Action).ConfigureAwait(false);
            }
        }

        private async Task HandleShiftPageKeysAsync(KeyActions args)
        {
            if (Parent is null)
            {
                return;
            }
            int shiftPageValue = args.Action == SHIFT_PAGE_UP ? -1 : 1;
            AddYears(CurrentDate, shiftPageValue, args.DateValue);
            await NavigateToAsync(Parent.Depth, (TValue)SfBaseUtils.ChangeType(CurrentDate, PropertyType)).ConfigureAwait(false);
        }

        private async Task HandleControlHomeEndKeysAsync(KeyActions args)
        {
            if (Parent is null)
            {
                return;
            }
            DateTime homeEndDate = CalculateControlHomeEndDate(args.Action);
            await NavigateToAsync(Parent.Depth, (TValue)SfBaseUtils.ChangeType(homeEndDate, PropertyType)).ConfigureAwait(false);
        }

        private DateTime CalculateControlHomeEndDate(string action)
        {
            return CalendarMode == CalendarType.Islamic
                ? CalculateIslamicControlHomeEndDate(action)
                : action == CONTROL_HOME
                ? new DateTime(CurrentDate.Year, 1, 1)
                : new DateTime(CurrentDate.Year, YEAR_NUMBER, 31);
        }

        private DateTime CalculateIslamicControlHomeEndDate(string action)
        {
            HijriCalendar hijriCalendar = new();
            int currentHijriYear = hijriCalendar.GetYear(CurrentDate);
            DateTime homeEndDate;
            if (action == CONTROL_HOME)
            {
                homeEndDate = hijriCalendar.ToDateTime(currentHijriYear, 1, 1, 0, 0, 0, 0);
            }
            else
            {
                int lastMonth = 12;
                int lastDay = hijriCalendar.GetDaysInMonth(currentHijriYear, lastMonth);
                homeEndDate = hijriCalendar.ToDateTime(currentHijriYear, lastMonth, lastDay, 0, 0, 0, 0);
            }
            return homeEndDate.Date.Add(CurrentDate.TimeOfDay);
        }

        private async Task ControlDownKeyActionAsync(KeyActions args, bool levelRestrict, CellDetails eventArgs, int view)
        {
            if ((args.FocusedDate is not null || args.SelectDate is not null) && !levelRestrict)
            {
                await ContentClickAsync(eventArgs, --view).ConfigureAwait(false);
                StateHasChanged();
            }
        }

        private void HomeKeyAction(int view)
        {
            int localYr = CurrentDate.Year;
            localYr = localYr < 10 ? 10 : localYr;
            int startYr = localYr - (localYr % 10);
            DateTime homeDatetime = view == 1 ? new DateTime(CurrentDate.Year, 1, 1) : view == 2 ? new DateTime(startYr, 1, 1) : new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            CurrentDate = homeDatetime;
            SwitchView(view);
        }

        private void IslamicHomeKeyAction(int view, string? dateValue)
        {
            DateTime homeDate = GetHijriStartDate(view);
            CurrentDate = homeDate;
            SwitchView(view);
        }

        private void EndKeyAction(int view)
        {
            DateTime firstDayOfNextMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1).AddMonths(1);
            DateTime lastDayThisMonth = firstDayOfNextMonth.AddDays(-1);
            int localEndYear = CurrentDate.Year;
            localEndYear = localEndYear < 10 ? 10 : localEndYear;
            int startYear = localEndYear - (localEndYear % 10);
            int endYr = startYear + (10 - 1);
            DateTime yearView = view == 1 ? new DateTime(CurrentDate.Year, 12, 1) : view == 2 ? new DateTime(endYr, 1, 1) : lastDayThisMonth;
            CurrentDate = yearView;
            SwitchView(view);
        }

        private void IslamicEndKeyAction(int view)
        {
            DateTime endDate = GetHijriEndDate(view);
            CurrentDate = endDate;
            SwitchView(view);
        }

        private DateTime GetHijriStartDate(int view)
        {
            HijriDate hijriDate = HijriParser.ToHijriDate(CurrentDate);
            int hijriYear = hijriDate.Year;
            int hijriMonth = hijriDate.Month;
            return view switch
            {
                1 => HijriParser.ToGregorian(new HijriDate() { Year = hijriYear, Month = 1, Date = 1 }),
                2 => HijriParser.ToGregorian(new HijriDate() { Year = hijriYear - (hijriYear % 10), Month = 1, Date = 1 }),
                _ => HijriParser.ToGregorian(new HijriDate() { Year = hijriYear, Month = hijriMonth, Date = 1 }),
            };
        }

        private DateTime GetHijriEndDate(int view)
        {
            HijriDate hijriDate = HijriParser.ToHijriDate(CurrentDate);
            int hijriYear = hijriDate.Year;
            int hijriMonth = hijriDate.Month;
            return view switch
            {
                1 => HijriParser.ToGregorian(new HijriDate() { Year = hijriYear, Month = 12, Date = 1 }),
                2 => HijriParser.ToGregorian(new HijriDate() { Year = hijriYear - (hijriYear % 10) + 9, Month = 1, Date = 1 }),
                _ => HijriParser.ToGregorian(new HijriDate() { Year = hijriYear, Month = hijriMonth, Date = new HijriCalendar().GetDaysInMonth(hijriYear, hijriMonth) }),
            };
        }

        private async Task SelectKeyActionAsync(KeyActions args, CellDetails eventArgs, bool levelRestrict, int view)
        {
            if (args.TargetClassList is not null && args.TargetClassList.Contains(TODAY, StringComparison.Ordinal) && args.TargetClassList.Contains(BTN, StringComparison.Ordinal))
            {
                await TodayButtonClickAsync(new MouseEventArgs()).ConfigureAwait(false);
            }
            else if (args.TargetClassList is not null && args.TargetClassList.Contains(TitleClass, StringComparison.Ordinal))
            {
                NavigateTitle();
            }
            else if (args.TargetClassList is not null && args.TargetClassList.Contains(PrevIconClass, StringComparison.Ordinal))
            {
                NavigatePreviousHandler(null, true);
            }
            else if (args.TargetClassList is not null && args.TargetClassList.Contains(NextIconClass, StringComparison.Ordinal))
            {
                NavigateNextHandler(null, true);
            }
            else
            {
                if (args.ClassList is not null && !args.ClassList.Contains(DISABLED, StringComparison.Ordinal))
                {
                    if (levelRestrict)
                    {
                        await SelectDateAsync(args.Events, args.ID, MultiSelection, MultiValues).ConfigureAwait(false);
                    }
                    else
                    {
                        await ContentClickAsync(eventArgs, --view).ConfigureAwait(false);
                    }
                    //StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Handles page up/down keyboard navigation actions.
        /// </summary>
        /// <param name="action">The keyboard action (PAGE_UP or PAGE_DOWN).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task PageKeyActionAsync(string action)
        {
            if (Parent is null)
            {
                return;
            }
            if (IsYearView())
            {
                await NavigateYearViewAsync(action).ConfigureAwait(false);
            }
            else if (IsDecadeView())
            {
                await NavigateDecadeViewAsync(action).ConfigureAwait(false);
            }
            else
            {
                await NavigateMonthViewAsync(action).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Checks if the calendar is in year view.
        /// </summary>
        private bool IsYearView()
        {
            return Parent is not null && Parent.Start == CalendarView.Year && Parent.Depth == CalendarView.Year;
        }

        /// <summary>
        /// Checks if the calendar is in decade view.
        /// </summary>
        private bool IsDecadeView()
        {
            return Parent is not null && Parent.Start == CalendarView.Decade && Parent.Depth == CalendarView.Decade;
        }

        /// <summary>
        /// Navigates the year view by one month forward or backward.
        /// </summary>
        private async Task NavigateYearViewAsync(string action)
        {
            int monthOffset = action == PAGE_DOWN ? SINGLE_MONTH_OFFSET : -SINGLE_MONTH_OFFSET;
            AddMonths(CurrentDate, monthOffset);
            await NavigateToAsync(CalendarView.Year, (TValue)SfBaseUtils.ChangeType(CurrentDate, PropertyType)).ConfigureAwait(false);
        }

        /// <summary>
        /// Navigates the decade view by ten years forward or backward.
        /// </summary>
        private async Task NavigateDecadeViewAsync(string action)
        {
            int yearOffset = action == PAGE_DOWN ? DECADE_YEAR_OFFSET : -DECADE_YEAR_OFFSET;
            AddYears(CurrentDate, yearOffset);
            await NavigateToAsync(CalendarView.Decade, (TValue)SfBaseUtils.ChangeType(CurrentDate, PropertyType)).ConfigureAwait(false);
        }

        /// <summary>
        /// Navigates the month view by one month forward or backward.
        /// </summary>
        private async Task NavigateMonthViewAsync(string action)
        {
            int monthOffset = action == PAGE_DOWN ? SINGLE_MONTH_OFFSET : -SINGLE_MONTH_OFFSET;
            AddMonths(CurrentDate, monthOffset);
            await NavigateToAsync(CalendarView.Month, (TValue)SfBaseUtils.ChangeType(CurrentDate, PropertyType)).ConfigureAwait(false);
        }

        private async Task IslamicPageKeyActionAsync(string action, string? dateValue)
        {
            HijriDate hijriDate = HijriParser.ToHijriDate(CurrentDate);
            int currentHijriDay = ParseHijriDay(dateValue, hijriDate);
            (int newYear, int newMonth) = CalculateIslamicPageNavigation(action, hijriDate.Year, hijriDate.Month);
            DateTime newDate = CreateIslamicNavigationDate(newYear, newMonth, currentHijriDay);
            CurrentDate = newDate;
            CalendarView viewToNavigate = DetermineNavigationView();
            await NavigateToAsync(viewToNavigate, (TValue)SfBaseUtils.ChangeType(CurrentDate, PropertyType)).ConfigureAwait(false);
        }

        private int ParseHijriDay(string? dateValue, HijriDate hijriDate)
        {
            return int.TryParse(dateValue, out int hijriDayFromValue) && hijriDayFromValue > 0 ? hijriDayFromValue : hijriDate.Date;
        }

        private (int year, int month) CalculateIslamicPageNavigation(string action, int currentYear, int currentMonth)
        {
            if (IsYearViewNavigation())
            {
                return (currentYear + (action == PAGE_DOWN ? 1 : -1), currentMonth);
            }
            return IsDecadeViewNavigation()
                ? ((int year, int month))(currentYear + (action == PAGE_DOWN ? 10 : -10), currentMonth)
                : CalculateIslamicMonthNavigation(action, currentYear, currentMonth);
        }

        private bool IsYearViewNavigation()
        {
            return Parent is not null && Parent.Start == CalendarView.Year && Parent.Depth == CalendarView.Year;
        }

        private bool IsDecadeViewNavigation()
        {
            return Parent is not null && Parent.Start == CalendarView.Decade && Parent.Depth == CalendarView.Decade;
        }

        private (int year, int month) CalculateIslamicMonthNavigation(string action, int currentYear, int currentMonth)
        {
            if (action == PAGE_DOWN)
            {
                return currentMonth == 12 ? (currentYear + 1, 1) : (currentYear, currentMonth + 1);
            }
            return currentMonth == 1 ? (currentYear - 1, 12) : (currentYear, currentMonth - 1);
        }

        private DateTime CreateIslamicNavigationDate(int year, int month, int preferredDay)
        {
            HijriCalendar hijriCalendar = new();
            int daysInMonth = hijriCalendar.GetDaysInMonth(year, month);
            int validDay = Math.Min(preferredDay, daysInMonth);
            HijriDate hijriDate = new() { Year = year, Month = month, Date = validDay };
            return HijriParser.ToGregorian(hijriDate);
        }

        private CalendarView DetermineNavigationView()
        {
            return Parent?.Start switch
            {
                CalendarView.Year => CalendarView.Year,
                CalendarView.Decade => CalendarView.Decade,
                CalendarView.Month => CalendarView.Month,
                _ => throw new NotImplementedException(),
            };
        }

        private void UpdateKeyNavigation(DateTime date)
        {
            if (IsMonthYearRange(date))
            {
                IsNavigation = true;
            }
            else
            {
                CurrentDate = date;
            }
        }

        internal void KeyboardNavigate(int number, int currentView)
        {
            DateTime date = CurrentDate;
            switch (currentView)
            {
                case DECADE_VIEW_VAL:
                    AddYears(CurrentDate, number);
                    HeaderTitle = CalendarMode == CalendarType.Islamic
                               ? GetHijriYearHeader(int.Parse(StartHeadYr(CurrentDate), CultureInfo.CurrentCulture), int.Parse(EndHeadYr(CurrentDate), CultureInfo.CurrentCulture))
                               : StartHeadYr(CurrentDate) + TITLE_SEPARATOR + EndHeadYr(CurrentDate);
                    LocalDate = new DateTime(CurrentDate.Year, 1, 1, CurrentDate.Hour, CurrentDate.Minute, CurrentDate.Second, CurrentDate.Millisecond);
                    UpdateDecadeTitle();
                    UpdateKeyNavigation(CurrentDate);
                    break;
                case YEAR_VIEW_VAL:
                    AddMonths(CurrentDate, number);
                    TitleUpdate(CurrentDate, CurrentView());
                    UpdateKeyNavigation(CurrentDate);
                    break;
                case MONTH_VIEW_VAL:
                    DateTime addDate = date.AddDays(number);
                    CurrentDate = (addDate.Date >= Parent?.Min.Date && addDate.Date <= Parent.Max.Date) ? addDate : date;
                    if (date.Date >= Parent?.Min.Date && date.Date <= Parent.Max.Date)
                    {
                        SwitchView(0);
                    }

                    break;
                default:
                    break;
            }
        }

        private bool IsMonthYearRange(DateTime date)
        {
            return (date.Month >= Parent?.Min.Month || date.Year >= Parent?.Min.Year) && (date.Month <= Parent.Max.Month || date.Year <= Parent.Max.Year);
        }

        /// <summary>
        /// Checks if the maximum allowable date has been reached during calendar rendering.
        /// </summary>
        /// <param name="rowIterator">The current row iteration count.</param>
        /// <param name="localDate">The local date being evaluated.</param>
        /// <returns>True if max date is reached; otherwise, false.</returns>
        private bool IsMaxDateReached(int rowIterator, DateTime localDate)
        {
            if (localDate.Year != DateTime.MaxValue.Year)
            {
                return false;
            }
            if (CalendarView == CalendarView.Month)
            {
                DateTime futureDate = localDate.AddDays(rowIterator - 1);
                return futureDate.Day == DateTime.MaxValue.Day && futureDate.Month == DateTime.MaxValue.Month;
            }
            return CalendarView == CalendarView.Decade && localDate.AddYears(rowIterator - 1).Year == DateTime.MaxValue.Year;
        }

        private string GetHijriYearHeader(int startYear, int endYear)
        {
            if (CalendarMode != CalendarType.Islamic || string.IsNullOrEmpty(HeaderTitle))
            {
                return string.Empty;
            }
            string[] splitYear = HeaderTitle.Split('-');
            if (splitYear.Length == 2)
            {
                int currentStartHijriYear = HijriParser.ToHijriDate(new DateTime(startYear, 1, 1)).Year;
                int currentEndHijriYear = HijriParser.ToHijriDate(new DateTime(endYear, 1, 1)).Year;
                int prevStartYear = ParseYear(splitYear[0]);
                int prevEndYear = ParseYear(splitYear[1]);
                AlignToDecade(ref currentStartHijriYear, ref currentEndHijriYear);
                AdjustHijriYears(ref currentStartHijriYear, ref currentEndHijriYear, prevStartYear, prevEndYear, splitYear);
                return FormatHijriHeader(currentStartHijriYear, currentEndHijriYear);
            }
            else
            {
                int startHijriYear = ParseSingleYear(HeaderTitle);
                int endHijriYear = startHijriYear + 9;
                AlignToDecade(ref startHijriYear, ref endHijriYear);
                return FormatHijriHeader(startHijriYear, endHijriYear);
            }
        }

        // Helper function to parse year with cultural support
        private static int ParseYear(string yearStr)
        {
            return CultureInfo.CurrentCulture.Name.StartsWith("ar", StringComparison.Ordinal)
                ? int.Parse(ConvertArabicToNumeric(yearStr), CultureInfo.CurrentCulture)
                : int.Parse(yearStr, CultureInfo.CurrentCulture);
        }

        // Align start and end years to a 10-year block
        private static void AlignToDecade(ref int startYear, ref int endYear)
        {
            startYear = startYear - (startYear % 10) + 1;
            endYear = startYear + 9;
        }

        // Adjust the start and end Hijri years based on previous years and splitYear data
        private static void AdjustHijriYears(ref int startYear, ref int endYear, int prevStartYear, int prevEndYear, string[] splitYear)
        {
            if (startYear < prevStartYear && endYear < prevEndYear)
            {
                foreach (string yearStr in splitYear)
                {
                    int year = ParseYear(yearStr);
                    endYear = AdjustEndYear(endYear, year);
                }
                if (Math.Abs(endYear - ParseYear(splitYear[^1])) == 8)
                {
                    endYear -= 9;
                    startYear -= 9;
                }
            }
            else
            {
                foreach (string yearStr in splitYear)
                {
                    int year = ParseYear(yearStr);
                    startYear = AdjustStartYear(startYear, year);
                }
            }
            AlignToDecade(ref startYear, ref endYear); // Ensure alignment to a 10-year block after adjustments
        }

        // Adjust the end year based on specific conditions
        private static int AdjustEndYear(int endYear, int year)
        {
            int difference = Math.Abs(endYear - year);
            if (difference is >= 2 and <= 5)
            {
                endYear += difference - 1;
            }
            return endYear;
        }

        // Adjust the start year based on specific conditions
        private static int AdjustStartYear(int startYear, int year)
        {
            int difference = Math.Abs(startYear - year);
            if (difference == 0)
            {
                startYear++;
            }
            else if (difference == 2 && startYear > year)
            {
                startYear--;
            }
            else if (difference == 1 && startYear < year)
            {
                startYear += 2;
            }
            return startYear;
        }

        // Parse single year for cultures that support Arabic numerals
        private static int ParseSingleYear(string year)
        {
            return CultureInfo.CurrentCulture.Name.StartsWith("ar", StringComparison.Ordinal)
                ? int.Parse(ConvertArabicToNumeric(year), CultureInfo.CurrentCulture)
                : int.Parse(year, CultureInfo.CurrentCulture);
        }

        // Format the start and end Hijri years into a string header
        private string FormatHijriHeader(int startYear, int endYear)
        {
            string startYearFormatted = Intl.GetDateFormat(new DateTime(startYear, 1, 1), FORMAT_YEAR);
            string endYearFormatted = Intl.GetDateFormat(new DateTime(endYear, 1, 1), FORMAT_YEAR);
            return startYearFormatted + TITLE_SEPARATOR + endYearFormatted;
        }

        private static string ConvertArabicToNumeric(string input)
        {
            if (CultureInfo.CurrentCulture.Name.StartsWith("ar", StringComparison.Ordinal))
            {
                StringBuilder result = new();
                foreach (char character in input)
                {
                    if (character is >= '\u0660' and <= '\u0669')
                    {
                        char westernDigit = (char)(character - '\u0660' + '0');
                        _ = result.Append(westernDigit);
                    }
                    else
                    {
                        _ = result.Append(character);
                    }
                }
                return result.ToString();
            }
            else
            {
                return input;
            }
        }
    }
}
