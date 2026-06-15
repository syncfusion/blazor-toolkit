using Microsoft.AspNetCore.Components;
using System.Globalization;
using Syncfusion.Blazor.Toolkit.Internal;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit.Calendars.Internal
{
    /// <summary>
    /// The Calendar base is a graphical user interface component that displays a Gregorian Calendar, and allows a user to select a date.
    /// </summary>
    /// <typeparam name="TCalendarCell">Specifies the type of CalendarDayCell.</typeparam>
    public partial class CalendarDayCell<TCalendarCell> : CalendarBase<TCalendarCell>
    {
        internal const string OTHERMONTH = "e-other-month";
        internal const string OTHERDECADE = "e-other-year";
        internal const string DISABLED = "e-disabled";
        internal const string OVERLAY = "e-overlay";
        internal const string WEEKEND = "e-weekend";
        internal const string WEEKNUMBER = "e-week-number";
        internal const string LINK = "e-day";
        internal const string CELL = "e-cell";
        internal const string TODAY = "e-today";
        internal const string SELECTED = "e-selected";
        internal const string FOCUSEDDATE = "e-focused-date";
        internal const string VALUE = "Value";
        internal const string RENDERDAYCELL = "OnRenderDayCell";
        internal const string FORMATDATE = " d ";
        internal const string FORMATYEAR = "yyyy";
        internal const string FORMATSHORTDATE = "M/d/yy";
        internal const int CELLCOUNT = 42;
        internal const int WEEKCOUNT = 7;
        internal const string FORMATMONTH = "MMM";
        internal const string PERSIAN = "fa";
        internal string? RowEleClass { get; set; }
        internal string[] _reservedClass = [
            CELL,
            TODAY,
            SELECTED,
            FOCUSEDDATE,
            WEEKEND,
            WEEKNUMBER,
            LINK,
            OTHERMONTH,
            OTHERDECADE,
            DISABLED,
            OVERLAY
        ];

        private ElementReference _dayCell;

        private string TdEleClass { get; set; } = string.Empty;
        private string? DayTitle { get; set; }
        private string? DayLink { get; set; }
        private bool RangeDisabled { get; set; }
        private bool IsDisabled { get; set; }
        private bool IsMinMaxUpdate { get; set; }

        [CascadingParameter]
        internal CalendarBase<TCalendarCell>? Parent { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is triggered when a calendar day cell is clicked by the user.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{CellDetails}"/> representing the callback event that occurs when a cell is clicked. The callback receives the details for the clicked cell.
        /// </value>
        /// <remarks>
        /// Use this event to handle custom logic or actions when a user clicks on a day cell in the calendar. The associated event argument provides detailed information about the clicked cell, such as its date and status. This event enables integration and user interaction within the parent calendar component.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to handle day cell clicks in a calendar:
        /// <code><![CDATA[
        /// <SfCalendar @bind-Value="selectedDate">
        ///     <CalendarDayCell OnCellClick="HandleCellClick" />
        /// </SfCalendar>
        ///
        /// @code {
        ///     private void HandleCellClick(CellDetails cellDetails)
        ///     {
        ///         // Custom logic when a day cell is clicked.
        ///         Console.WriteLine($"Cell clicked: {cellDetails.CurrentDate}");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        /// <exclude/>
        [Parameter]
        public EventCallback<CellDetails> OnCellClick { get; set; }

        /// <summary>
        /// Gets or sets the current date value for this calendar cell.
        /// </summary>
        /// <value>
        /// A <c>DateTime</c> value representing the date associated with the current calendar cell.
        /// </value>
        /// <remarks>
        /// This property indicates the date value used to represent the currently rendered cell in the calendar grid. It is typically bound to the main calendar value, and updated as navigation or selection changes.
        /// </remarks>
        /// <example>
        /// Assigning or reading the current cell's date allows precise control over cell-specific behavior:
        /// <code><![CDATA[
        /// var todayCellDate = CalendarDayCell.CurrentCellDate;
        /// ]]></code>
        /// </example>
        /// <exclude/>
        [Parameter]
        public DateTime CurrentCellDate { get; set; }

        /// <summary>
        /// Gets or sets the local date value for the calendar cell.
        /// </summary>
        /// <value>
        /// A <c>DateTime</c> value indicating the date that this cell represents, localized according to the culture or calendar mode (e.g., Gregorian, Islamic, or Persian).
        /// </value>
        /// <remarks>
        /// This property is used for localization and correct display of calendar cells. Depending on the calendar mode, the local date may reflect a Gregorian, Hijri, or Persian date.
        /// </remarks>
        /// <example>
        /// Use <c>LocalDates</c> to obtain the culture-specific date of a calendar cell:
        /// <code><![CDATA[
        /// var persianDay = CalendarDayCell.LocalDates;
        /// ]]></code>
        /// </example>
        /// <exclude/>
        [Parameter]
        public DateTime LocalDates { get; set; }

        /// <summary>
        /// Gets or sets the CSS class applied to the calendar cell.
        /// </summary>
        /// <value>
        /// A <c>string</c> representing one or more CSS class names assigned to the cell.
        /// </value>
        /// <remarks>
        /// This property allows theming and visual customization of the individual calendar cells by applying custom CSS styles. The classes specified will be added to the cell element's class list. For built-in cell states (e.g., selected, today, disabled), use the appropriate constants defined in <see cref="CalendarDayCell{TCalendarCell}"/>.
        /// </remarks>
        /// <example>
        /// Add a custom class in your markup or logic:
        /// <code><![CDATA[
        /// <CalendarDayCell CellClass="my-special-style" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string? CellClass { get; set; } = default;

        /// <summary>
        /// Gets or sets a value indicating whether the current date cell should be marked as focused in the calendar view.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current cell's date is visually focused; otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// When enabled, this property causes the cell corresponding to the current date to receive focus highlighting in the calendar display. Typically, only one cell is focused at a time.
        /// </remarks>
        /// <example>
        /// Set <c>IsFocusTodayDate</c> to <c>false</c> to disable cell focus highlighting:
        /// <code><![CDATA[
        /// <CalendarDayCell IsFocusTodayDate="false" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool IsFocusTodayDate { get; set; } = true;

        /// <summary>
        /// Get or Set Cell value.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public int Cells { get; set; }

        /// <summary>
        /// Get or Set the today date value.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public DateTime TodayCellDate { get; set; }

        /// <summary>
        /// Get or Set calendar navigation.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool IsNavigation { get; set; }

        /// <summary>
        /// Get or Set current calendar view.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public CalendarView CalendarRenderView { get; set; }

        /// <summary>
        /// Get or Set the calendar cell selection.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool IsSelect { get; set; }

        /// <summary>
        /// Specifies the option to enable the multiple dates selection of the calendar.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool IsMultiSelect { get; set; }

        /// <summary>
        /// Get or Set calendar values.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public DateTime[] MultiselectValues { get; set; } = default!;
        /// <exclude/>

        /// <summary>
        /// Invoked by the Blazor framework when the calendar cell component initializes.
        /// Triggers the initial rendering and value binding for the calendar cell.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.
        /// The task completes once the initial cell rendering and state setup is done.
        /// </returns>
        /// <remarks>
        /// This override performs setup logic unique to the cell's rendering and calendar integration. It calls <see cref="RenderCellAsync"/> to ensure the cell's state is fully initialized before display.
        /// </remarks>
        /// <exclude />
        protected override async Task OnInitializedAsync()
        {
            if (Parent is null)
            {
                return;
            }
            await RenderCellAsync(CurrentCellDate, Parent.Value!, IsMultiSelect, MultiselectValues, CalendarRenderView).ConfigureAwait(false);
        }

        /// <summary>
        /// Invoked automatically by the Blazor framework whenever a parameter or bound value changes.
        /// Updates the cell's rendering and internal state on dynamic property updates, navigations, or selection changes.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing completion of asynchronous update logic.
        /// The task completes after all dynamic changes and possible re-renders are handled.
        /// </returns>
        /// <remarks>
        /// This override checks for property or navigation changes. If a navigation or value difference is detected, <see cref="RenderCellAsync"/> is triggered to re-render the calendar cell while maintaining accurate navigation and selection state.
        /// </remarks>
        /// <exclude />
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);
            if (Parent is not null && (IsNavigation || (Parent.PropertyChanges is not null && Parent.PropertyChanges.Count > 0 && !SfBaseUtils.Equals(GenericValue(CurrentCellDate), Parent.Value))))
            {
                await RenderCellAsync(CurrentCellDate, Parent.Value!, IsMultiSelect, MultiselectValues, CalendarRenderView).ConfigureAwait(false);
                if (IsNavigation)
                {
                    UpdateCellDetails();
                    IsMinMaxUpdate = true;
                }
            }
        }

        /// <summary>
        /// Performs additional logic after the calendar cell's render tree has been built.
        /// Used to update cell details and refresh min/max logic after the first render or navigation changes.
        /// </summary>
        /// <param name="firstRender">
        /// <c>true</c> if this is the first time the component is rendered; otherwise, <c>false</c>.
        /// </param>
        /// <remarks>
        /// This method is useful for focus management and feature updates that must occur after the Blazor rendering pass, such as updating accessibility ARIA details and handling navigation boundary conditions.
        /// </remarks>
        /// <exclude />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                UpdateCellDetails();
            }
            if (IsNavigation && IsMinMaxUpdate)
            {
                if (Parent is not null)
                {
                    await Parent.BindMinMaxDaysAsync().ConfigureAwait(false);
                }
                IsMinMaxUpdate = false;
            }
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates internal cell details in the parent calendar component after rendering or navigation.
        /// Adds this cell’s details to the parent’s tracking structure if not already present.
        /// </summary>
        /// <remarks>
        /// This logic is needed for cell-level event handling, ARIA compliance, and custom rendering extensibility. 
        /// Called on first render and during navigation boundary updates.
        /// </remarks>
        private void UpdateCellDetails()
        {
            CellDetails? isCellPresent = Parent?.CellDetailsData?.Where(i => i.CellID == LocalDates.Ticks + "_" + Cells).FirstOrDefault();
            if (isCellPresent is null)
            {
                Parent?.CellDetailsData?.Add(new CellDetails { CellID = LocalDates.Ticks + "_" + Cells, ClassList = !string.IsNullOrEmpty(CellClass) ? CellClass : TdEleClass, Element = _dayCell, EventArgs = new MouseEventArgs(), CurrentDate = CurrentCellDate });
            }
        }
        /// <summary>
        /// Updates the display title and accessibility description for the current cell based on its view context.
        /// Handles localization, calendar modes, and disables cells out of valid range.
        /// </summary>
        /// <param name="calendarView">The current calendar view mode (month, year, or decade).</param>
        /// <remarks>
        /// This internal method accounts for Gregorian, Islamic, or other calendar modes, updating the displayed title and the cell’s CSS/ARIA state. Sets the cell as disabled if its date falls outside the parent min/max range.
        /// </remarks>
        private void UpdateTitle(CalendarView calendarView)
        {
            RangeDisabled = calendarView == CalendarView.Decade ? ((Parent?.Min.Date.Year > LocalDates.Year) || (Parent?.Max.Date.Year < LocalDates.Date.Year)) : ((Parent?.Min.Date > LocalDates) || (Parent?.Max.Date < LocalDates.Date));
            if (RangeDisabled)
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED + " " + OVERLAY);
            }
            string title = string.Empty;
            switch (calendarView)
            {
                case CalendarView.Month:
                    if (CalendarMode == CalendarType.Islamic)
                    {
                        HijriDate hijriCellDate = HijriParser.ToHijriDate(LocalDates);
                        title = $"{LocalDates.DayOfWeek}, {Parent?._hijri_Month_Wide[hijriCellDate.Month - 1]} {hijriCellDate.Date}, {hijriCellDate.Year} AH";
                    }
                    else
                    {
                        title = Intl.GetDateFormat(LocalDates, CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern);
                    }
                    break;
                case CalendarView.Year:
                    if (CalendarMode == CalendarType.Islamic)
                    {
                        HijriDate hijriDate = HijriParser.ToHijriDate(LocalDates);
                        title = Parent?._hijri_Month_Wide[hijriDate.Month - 1] + hijriDate.Year;
                    }
                    else
                    {
                        title = Intl.GetDateFormat(LocalDates, CultureInfo.CurrentCulture.DateTimeFormat.YearMonthPattern);
                    }
                    break;
                case CalendarView.Decade:
                    if (CalendarMode == CalendarType.Islamic)
                    {
                        HijriDate hijriDecadeDate = HijriParser.ToHijriDate(LocalDates);
                        // Clamp day to valid range for the hijri month to avoid invalid date construction
                        int safeDay = Math.Min(hijriDecadeDate.Date, new HijriCalendar().GetDaysInMonth(hijriDecadeDate.Year, hijriDecadeDate.Month));
                        HijriDate safeHijri = new() { Year = hijriDecadeDate.Year, Month = hijriDecadeDate.Month, Date = safeDay };
                        DateTime dateTime = HijriParser.ToGregorian(safeHijri);
                        title = Intl.GetDateFormat(dateTime, FORMATYEAR);
                    }
                    else
                    {
                        title = Intl.GetDateFormat(LocalDates, FORMATYEAR);
                    }
                    break;
                default:
                    break;
            }
            DayTitle = title;
        }
        /// <summary>
        /// Updates cell CSS classes to reflect weekend and "other month" visual states depending on calendar type/grouping.
        /// </summary>
        /// <param name="currentDate">The date representing the current visible reference/date for the calendar group or row.</param>
        /// <remarks>
        /// Weekend calculation accounts for Persian, Islamic, and Gregorian calendars. Marks cells as belonging to other months or decades accordingly.
        /// </remarks>
        private void UpdateWeekEnds(DateTime currentDate)
        {
            bool isPersianCulture = CultureInfo.CurrentCulture.Name.StartsWith(PERSIAN, StringComparison.Ordinal);
            if (isPersianCulture)
            {
                PersianCalendar persianCalendar = new();
                if (persianCalendar.GetYear(currentDate) != persianCalendar.GetYear(LocalDates) || persianCalendar.GetMonth(currentDate) != persianCalendar.GetMonth(LocalDates))
                {
                    TdEleClass = SfBaseUtils.AddClass(TdEleClass, OTHERMONTH);
                }
            }
            else if (!IsSameCalendarMonth(LocalDates, currentDate))
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, OTHERMONTH);
            }
            if (LocalDates.DayOfWeek == 0 || (int)LocalDates.DayOfWeek == 6)
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, WEEKEND);
            }
        }

        /// <summary>
        /// Determines whether two dates belong to the same visible month in the active calendar mode.
        /// </summary>
        private bool IsSameCalendarMonth(DateTime firstDate, DateTime secondDate)
        {
            if (CalendarMode == CalendarType.Islamic)
            {
                HijriDate firstHijriDate = HijriParser.ToHijriDate(firstDate);
                HijriDate secondHijriDate = HijriParser.ToHijriDate(secondDate);
                return firstHijriDate.Year == secondHijriDate.Year && firstHijriDate.Month == secondHijriDate.Month;
            }

            return firstDate.Year == secondDate.Year && firstDate.Month == secondDate.Month;
        }

        /// <summary>
        /// Converts a Gregorian <see cref="DateTime"/> to its corresponding HijriDate when in Islamic calendar mode.
        /// </summary>
        /// <param name="greGorianDate">The Gregorian date to convert.</param>
        /// <returns>The equivalent <see cref="HijriDate"/> value for Islamic calendar scenarios. For other modes, returns a default/unaltered HijriDate.</returns>
        /// <remarks>
        /// This helper is mainly for rendering Islamic calendar cells, navigation, and date comparison logic. Not needed in strict Gregorian scenarios.
        /// </remarks>
        internal static HijriDate ConvertHijriDate(DateTime greGorianDate)
        {
            HijriDate hijridate = HijriParser.ToHijriDate(greGorianDate);
            return hijridate;
        }

        /// <summary>
        /// Updates and tracks cell disable state, reflecting date disable logic and managing visual cell disabling for multi-select scenarios.
        /// </summary>
        /// <param name="eventArgs">Cell rendering event arguments containing contextual cell state and current date.</param>
        /// <param name="dateValue">Current cell value (selected date if any).</param>
        /// <param name="multiSelection">Indicates if multi-select mode is enabled.</param>
        /// <param name="values">Set of currently selected dates (for multi-select context).</param>
        /// <returns>A <see cref="Task"/> completing after updates and any required parent notification logic.</returns>
        /// <remarks>
        /// This method ensures both disable overlays and multi-date un-selection when a disabled state is detected. It updates both the UI and parent component's disabled state ledger.
        /// </remarks>
        private async Task UpdateDisableCellsAsync(RenderDayCellEventArgs eventArgs, TCalendarCell dateValue, bool multiSelection, DateTime[] values)
        {
            bool isDisabledCell = TdEleClass is not null && TdEleClass.Contains(DISABLED, StringComparison.Ordinal);
            if (eventArgs.IsDisabled || (IsSelect && (isDisabledCell || RangeDisabled)))
            {
                if (multiSelection && values is not null && values.Length > 0)
                {
                    for (int index = 0; index < values.Length; index++)
                    {
                        List<DateTime> val = [.. values];
                        if (eventArgs.Date.Ticks == values[index].Ticks)
                        {
                            val.RemoveAt(index);
                            values = [.. val];
                            index = -1;
                        }
                    }
                }
                else if (dateValue is not null && ConvertDate(dateValue) == eventArgs.Date)
                {
                    if (Parent is not null)
                    {
                        await Parent.UpdateCalendarPropertyAsync(VALUE, default(TCalendarCell)).ConfigureAwait(false);
                    }
                }
            }
            if (TdEleClass is not null && ((eventArgs.IsDisabled && (LocalDates != DateTime.Now)) || (IsSelect && (isDisabledCell || RangeDisabled))))
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED + " " + OVERLAY);
            }
            if (eventArgs.IsDisabled)
            {
                CellDetails? isPresentData = Parent?.DisabledDayCellData is not null ? Parent.DisabledDayCellData.Where(item => item.CellID == eventArgs.CellData?.CellID)?.FirstOrDefault() : null;
                if ((Parent is not null) && Parent.DisabledDayCellData is null)
                {
                    if (eventArgs.CellData is not null)
                    {
                        Parent.DisabledDayCellData = [eventArgs.CellData];
                    }
                }
                else if (Parent is not null && isPresentData is null)
                {
                    Parent.DisabledDayCellData.Add(eventArgs.CellData);
                }
            }
        }
        /// <summary>
        /// Updates calendar cell visual state when multi-selection is enabled and several dates are selected.
        /// Sets the appropriate cell classes and unselects if all values are cleared.
        /// </summary>
        /// <param name="values">The array of all currently-selected cell dates.</param>
        /// <param name="dateValue">The currently active/selected cell value (if any).</param>
        /// <param name="otherMnthBool">Indicates if the cell is in the "other month" visual state.</param>
        /// <param name="disabledCls">Indicates if the cell is visually or logically disabled.</param>
        /// <remarks>
        /// This logic ensures correct multi-select cell coloring and automatic de-selection if needed. Used only for multi-select scenarios where visual state is dynamic per value selection.
        /// </remarks>
        private void UpdateMultiValues(DateTime[] values, TCalendarCell? dateValue, bool otherMnthBool, bool disabledCls)
        {
            dateValue = (dateValue is not null) ? dateValue : GenericValue(values[0]);
            DateTime getValue = ConvertDate(dateValue!);
            for (int tempValue = 0; tempValue < values.Length; tempValue++)
            {
                string localDateString = Intl.GetDateFormat(LocalDates, FORMATSHORTDATE);
                string tempDateString = Intl.GetDateFormat(values[tempValue], FORMATSHORTDATE);
                if ((localDateString == tempDateString && GetDateVal(LocalDates, values[tempValue])) || GetDateVal(LocalDates, getValue))
                {
                    if (TdEleClass is not null)
                    {
                        TdEleClass = TdEleClass.Contains(FOCUSEDDATE, StringComparison.Ordinal) ? SfBaseUtils.RemoveClass(TdEleClass, FOCUSEDDATE) : TdEleClass;
                        TdEleClass = SfBaseUtils.AddClass(TdEleClass, SELECTED);
                    }
                }
                else
                {
                    UpdateFocus(otherMnthBool, disabledCls, LocalDates, CurrentCellDate);
                }
            }

            if (values.Length <= 0)
            {
                UpdateFocus(otherMnthBool, disabledCls, LocalDates, CurrentCellDate);
                if (Parent is not null && dateValue is not null)
                {
                    Parent.Value = default!;
                }
            }
        }
        private async Task RenderDayCellAsync(DateTime currentDate, TCalendarCell dateValue, bool multiSelection, DateTime[] values, CalendarView calendarView)
        {
            DateTime date = LocalDates;
            string dateString = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            if ((IsSelect || (Parent is not null && Parent.IsTodayClick)) && !string.IsNullOrEmpty(TdEleClass))
            {
                TdEleClass = TdEleClass.Replace(TODAY, string.Empty, StringComparison.Ordinal);
                TdEleClass = TdEleClass.Replace(SELECTED, string.Empty, StringComparison.Ordinal);
                TdEleClass = TdEleClass.Replace(FOCUSEDDATE, string.Empty, StringComparison.Ordinal);
                TdEleClass = TdEleClass.Replace(OTHERMONTH, string.Empty, StringComparison.Ordinal);
                if ((Parent is not null) && Parent.IsTodayClick)
                {
                    TdEleClass = TdEleClass.Replace(DISABLED, string.Empty, StringComparison.Ordinal);
                    TdEleClass = TdEleClass.Replace(OVERLAY, string.Empty, StringComparison.Ordinal);
                    if (!string.IsNullOrEmpty(TdEleClass))
                    {
                        string[] classList = TdEleClass.Split(SPACE);
                        List<string> customClass = [.. classList.Where(x => !_reservedClass.Contains(x))];
                        TdEleClass = customClass is not null && customClass.Count != 0 ? SfBaseUtils.RemoveClass(TdEleClass, string.Join(SPACE, customClass)) : TdEleClass;
                    }
                }
            }
            else
            {
                TdEleClass = CELL;
            }
            DayLink = Intl.GetDateFormat(LocalDates, FORMATDATE).Trim();
            if (CalendarMode == CalendarType.Islamic)
            {
                HijriDate hijriDate = HijriParser.ToHijriDate(LocalDates);
                DayLink = Intl.GetNativeDigits(hijriDate.Date.ToString(CultureInfo.InvariantCulture), CultureInfo.CurrentCulture.NumberFormat.NativeDigits);
            }
            UpdateTitle(calendarView);
            UpdateWeekEnds(currentDate);
            RenderDayCellEventArgs eventArgs = await TriggerDayCellEventAsync().ConfigureAwait(false);
            await UpdateDisableCellsAsync(eventArgs is null ? new() : eventArgs, dateValue, multiSelection, values).ConfigureAwait(false);
            bool otherMnthBool = TdEleClass.Contains(OTHERMONTH, StringComparison.Ordinal);
            bool disabledCls = TdEleClass.Contains(DISABLED, StringComparison.Ordinal);
            if (multiSelection && values is not null && values.Length > 0 && !disabledCls)
            {
                UpdateMultiValues(values, dateValue, otherMnthBool, disabledCls);
            }
            else if (multiSelection && values is not null && values.Length <= 0)
            {
                UpdateFocus(otherMnthBool, disabledCls, LocalDates, CurrentCellDate);
                if (Parent is not null && dateValue is not null)
                {
                    Parent.Value = default;
                }
            }
            else if (dateValue is not null)
            {
                DateTime dateVal = ConvertDate(dateValue);
                if (!disabledCls && GetDateVal(LocalDates, dateVal))
                {
                    TdEleClass = SfBaseUtils.AddClass(TdEleClass, SELECTED);
                }
                UpdateFocus(otherMnthBool, disabledCls, LocalDates, CurrentCellDate);
            }
            else
            {
                UpdateFocus(otherMnthBool, disabledCls, LocalDates, CurrentCellDate);
            }

            if (date.Month == DateTime.Now.Month && date.Day == DateTime.Now.Day && date.Year == DateTime.Now.Year)
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, TODAY);
            }
            if (Cells == CELLCOUNT)
            {
                IsNavigation = false;
            }
            if (TdEleClass.Contains(FOCUSEDDATE, StringComparison.Ordinal))
            {
                if (Parent is not null)
                {
                    await Parent.UpdateAriaActiveDescendantAsync(eventArgs?.CellData?.CellID).ConfigureAwait(false);
                }
            }
            UpdateCustomClass(dateString);
        }

        /// <summary>
        /// Method which adds custom classes to the table data element
        /// </summary>
        /// <exclude/>
        internal void UpdateCustomClass(string dateString)
        {
            if ((Parent is not null) && Parent.CustomizedDates.TryGetValue(dateString, out _))
            {
                string className = Parent.CustomizedDates[dateString];
                if (TdEleClass is not null && !TdEleClass.Contains(className, StringComparison.Ordinal))
                {
                    TdEleClass = SfBaseUtils.AddClass(TdEleClass, className);
                }
            }
        }

        private async Task<RenderDayCellEventArgs> TriggerDayCellEventAsync()
        {
            CellDetails cellData = new() { CellID = LocalDates.Ticks + "_" + Cells, ClassList = CellClass ?? TdEleClass, Element = _dayCell, EventArgs = new MouseEventArgs(), CurrentDate = CurrentCellDate };
            RenderDayCellEventArgs eventArgs = new()
            {
                Date = LocalDates,
                IsDisabled = IsDisabled,
                IsOutOfRange = RangeDisabled,
                Name = RENDERDAYCELL,
                CellData = cellData
            };
            bool isEqualVal = Parent is not null && Parent.PropertyChanges is not null && Parent.PropertyChanges.Count > 0 && Parent.PropertyChanges.ContainsKey(VALUE);
            if (!IsSelect && !isEqualVal)
            {
                if (Parent is not null)
                {
                    await Parent.BindRenderDayEventAsync(eventArgs).ConfigureAwait(false);
                }
                if (!string.IsNullOrEmpty(eventArgs.CellData?.ClassList))
                {
                    string[]? classList = eventArgs.CellData?.ClassList.Split(SPACE);
                    List<string>? customClasses = classList is null ? [] : [.. classList.Where(x => !_reservedClass.Contains(x))];
                    if (eventArgs.IsDisabled)
                    {
                        customClasses.Add(DISABLED);
                        customClasses.Add(OVERLAY);
                    }
                    string dateString = eventArgs.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (Parent is not null && Parent.CustomizedDates.TryGetValue(dateString, out string? customizedDate) && customizedDate is not null && !eventArgs.IsDisabled && customizedDate.Contains(DISABLED, StringComparison.OrdinalIgnoreCase) && customizedDate.Contains(OVERLAY, StringComparison.OrdinalIgnoreCase))
                    {
                        Parent.CustomizedDates[dateString] = customizedDate.Replace(DISABLED, string.Empty, StringComparison.OrdinalIgnoreCase).Replace(OVERLAY, string.Empty, StringComparison.OrdinalIgnoreCase);
                    }
                    if (customClasses is not null && customClasses.Count != 0 && Parent is not null && !Parent.CustomizedDates.ContainsKey(dateString))
                    {
                        Parent.CustomizedDates.Add(dateString, string.Join(SPACE, customClasses));
                    }
                    else if (customClasses is not null && customClasses.Count == 0 && Parent is not null && Parent.CustomizedDates.ContainsKey(dateString))
                    {
                        Parent.CustomizedDates.Remove(dateString);
                    }
                }
            }

            return eventArgs;
        }

        /// <summary>
        /// Asynchronously renders and updates the visual and logical state of a calendar cell depending on the current calendar view (month, year, decade).
        /// </summary>
        /// <param name="currentDate">The reference date for the cell's context—typically the currently visible date or navigation origin.</param>
        /// <param name="dateValue">The cell's current value, representing the selected or highlighted value, if any.</param>
        /// <param name="multiSelection">Indicates if multiple date selection is enabled for the calendar.</param>
        /// <param name="values">The set of currently selected dates, if in multi-select mode.</param>
        /// <param name="calendarView">The current visual view mode of the calendar (month, year, or decade).</param>
        /// <returns>A <see cref="Task"/> that completes once the necessary cell rendering and state logic is finished.</returns>
        /// <remarks>
        /// This method dispatches to the correct cell-rendering logic for day, month, or year views based on the <paramref name="calendarView"/> value. It supports advanced scenarios such as multi-date selection and localization.
        /// </remarks>
        internal async Task RenderCellAsync(DateTime currentDate, TCalendarCell dateValue, bool multiSelection, DateTime[] values, CalendarView calendarView)
        {
            switch (calendarView)
            {
                case CalendarView.Year:
                    await RenderMonthCellAsync(currentDate, dateValue, calendarView).ConfigureAwait(false);
                    break;
                case CalendarView.Decade:
                    await RenderYearCellAsync(currentDate, dateValue, calendarView).ConfigureAwait(false);
                    break;
                case CalendarView.Month:
                    await RenderDayCellAsync(currentDate, dateValue, multiSelection, values, calendarView).ConfigureAwait(false);
                    break;
                default:
                    break;
            }
        }

        private async Task RenderMonthCellAsync(DateTime currentDate, TCalendarCell dateValue, CalendarView calendarView)
        {
            if (Parent is null)
            {
                return;
            }
            DateTime curDate = currentDate;
            int curYrs = LocalDates.Year;
            int minYr = Parent.Min.Year;
            int maxYr = Parent.Max.Year;
            if (CalendarMode == CalendarType.Islamic)
            {
                minYr = HijriParser.ToHijriDate(Parent.Min).Year;
                maxYr = HijriParser.ToHijriDate(Parent.Max).Year;
                curYrs = HijriParser.ToHijriDate(LocalDates).Year;
            }
            int month = Cells + 1;
            DateTime dateVal = dateValue is not null ? ConvertDate(dateValue) : DateTime.Now;
            bool localMonth = dateValue is not null && dateVal.Month == LocalDates.Month;
            bool select = dateValue is not null && dateVal.Year == curDate.Year && localMonth;
            TdEleClass = CELL;
            if (CalendarMode == CalendarType.Islamic)
            {

                HijriDate currentHijriDate = HijriParser.ToHijriDate(currentDate);
                HijriDate islamicDate = new() { Year = currentHijriDate.Year, Month = LocalDates.Month, Date = 1 };
                LocalDates = HijriParser.ToGregorian(islamicDate);
                HijriDate hijriDateValue = HijriParser.ToHijriDate(dateVal);
                select = dateValue is not null && hijriDateValue.Month == islamicDate.Month && hijriDateValue.Year == currentHijriDate.Year;
                DayLink = Parent._hijri_Month_Abbreviated[islamicDate.Month - 1];
                if (CultureInfo.CurrentCulture.Name.StartsWith("ar", StringComparison.Ordinal))
                {
                    CultureInfo culture = new("ar-SA");
                    string[] arabic = culture.DateTimeFormat.AbbreviatedMonthNames[islamicDate.Month - 1].Split(" ");
                    DayLink = arabic[0];
                }
                else if (CultureInfo.CurrentCulture.Name.StartsWith("fr", StringComparison.Ordinal))
                {
                    DayLink = Parent._hijri_Month_Abbreviated_French[islamicDate.Month - 1];
                }
            }
            else
            {
                DayLink = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Intl.GetDateFormat(LocalDates, FORMATMONTH));
            }
            UpdateTitle(calendarView);
            if (TdEleClass.Contains(OVERLAY, StringComparison.Ordinal) && TdEleClass.Contains(DISABLED, StringComparison.Ordinal))
            {
                TdEleClass = SfBaseUtils.RemoveClass(TdEleClass, DISABLED + " " + OVERLAY);
            }
            if (curYrs < minYr || (month < Parent.Min.Month && curYrs == minYr) || curYrs > maxYr || (month > Parent.Max.Month && curYrs >= maxYr))
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED);
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, OVERLAY);
            }
            else if (dateValue is not null && select)
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, SELECTED);
            }
            else if (CalendarMode == CalendarType.Islamic)
            {
                HijriDate hijriLocalDate = HijriParser.ToHijriDate(LocalDates);
                HijriDate hijriCurDate = HijriParser.ToHijriDate(curDate);
                HijriDate hijriCurrentDate = HijriParser.ToHijriDate(currentDate);
                if (hijriLocalDate.Month == hijriCurDate.Month && hijriCurrentDate.Month == hijriCurDate.Month)
                {
                    if ((IsFocusTodayDate && Parent.Start == CalendarView.Year) || IsFocusTodayDate || (Parent.Start != CalendarView.Year))
                    {
                        RemoveFocusedDate();
                        TdEleClass = SfBaseUtils.AddClass(TdEleClass, FOCUSEDDATE);
                    }
                }
            }
            else if (LocalDates.Month == curDate.Month && currentDate.Month == curDate.Month)
            {
                if ((IsFocusTodayDate && Parent.Start == CalendarView.Year) || IsFocusTodayDate || (Parent.Start != CalendarView.Year))
                {
                    RemoveFocusedDate();
                    TdEleClass = SfBaseUtils.AddClass(TdEleClass, FOCUSEDDATE);
                }
            }

            RenderDayCellEventArgs eventArgs = await TriggerDayCellEventAsync().ConfigureAwait(false);
            if (eventArgs.IsDisabled && (LocalDates != DateTime.Now))
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED);
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, OVERLAY);
            }
            if (TdEleClass.Contains(FOCUSEDDATE, StringComparison.Ordinal))
            {
                await Parent.UpdateAriaActiveDescendantAsync(eventArgs.CellData.CellID).ConfigureAwait(false);
            }
        }

        private async Task RenderYearCellAsync(DateTime currentDate, TCalendarCell dateValue, CalendarView calendarView)
        {
            int localYr = currentDate.Year;
            localYr = localYr < 10 ? 10 : localYr;
            int startYear = localYr - (localYr % 10);
            int endYear = localYr - (localYr % 10) + (10 - 1);
            DateTime startYr = new(startYear, 1, 1);
            DateTime endYr = new(endYear, 1, 1);
            DateTime start = new(localYr - (localYr % 10) - 1, 1, 1);
            int year = start.Year + Cells;
            DateTime localDate = new(year, LocalDates.Month, LocalDates.Day);
            if (CalendarMode == CalendarType.Islamic)
            {
                localDate = LocalDates;
                HijriDate localHijriDate = HijriParser.ToHijriDate(localDate);
                if (localHijriDate.Year % 10 != 9 && Cells == 0)
                {
                    localDate = LocalDates;
                    localHijriDate = HijriParser.ToHijriDate(localDate);
                }
                localDate = new DateTime(localHijriDate.Year, localHijriDate.Month, localHijriDate.Date);
            }
            DayLink = Intl.GetDateFormat(localDate, FORMATYEAR);
            TdEleClass = CELL;
            UpdateTitle(calendarView);
            if (TdEleClass.Contains(OVERLAY, StringComparison.Ordinal) && TdEleClass.Contains(DISABLED, StringComparison.Ordinal))
            {
                TdEleClass = SfBaseUtils.RemoveClass(TdEleClass, DISABLED + " " + OVERLAY);
            }
            if ((year < startYr.Year) || (year > endYr.Year))
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, OTHERDECADE);
                if (year < Parent?.Min.Year || year > Parent?.Max.Year)
                {
                    TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED);
                    TdEleClass = SfBaseUtils.AddClass(TdEleClass, OVERLAY);
                }
            }
            else if (year < Parent?.Min.Year || year > Parent?.Max.Year)
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED);
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, OVERLAY);
            }
            else if (dateValue is not null)
            {
                DateTime dateTimeVal = ConvertDate(dateValue);
                if (CalendarMode == CalendarType.Islamic)
                {
                    HijriDate hijriCurrentDate = HijriParser.ToHijriDate(currentDate);
                    HijriDate hijriDate = HijriParser.ToHijriDate(ConvertDate(dateValue));
                    dateTimeVal = new DateTime(hijriDate.Year, hijriDate.Month, hijriDate.Date);
                    if (localDate.Year == hijriCurrentDate.Year)
                    {
                        if ((IsFocusTodayDate && Parent?.Start == CalendarView.Year) || IsFocusTodayDate || (Parent?.Start != CalendarView.Year))
                        {
                            RemoveFocusedDate();
                            TdEleClass = SfBaseUtils.AddClass(TdEleClass, FOCUSEDDATE);
                        }
                    }
                }
                bool isLocalyear = localDate.Year == currentDate.Year && !TdEleClass.Contains(DISABLED, StringComparison.Ordinal);
                TdEleClass = (localDate.Year == dateTimeVal.Year) ? SfBaseUtils.AddClass(TdEleClass, SELECTED) : isLocalyear ? SfBaseUtils.AddClass(TdEleClass, FOCUSEDDATE) : TdEleClass;
            }
            else if (localDate.Year == currentDate.Year && !TdEleClass.Contains(DISABLED, StringComparison.Ordinal))
            {
                if ((IsFocusTodayDate && Parent?.Start == CalendarView.Decade) || IsFocusTodayDate || (Parent?.Start != CalendarView.Decade))
                {
                    RemoveFocusedDate();
                    TdEleClass = SfBaseUtils.AddClass(TdEleClass, FOCUSEDDATE);
                }
            }
            RenderDayCellEventArgs eventArgs = await TriggerDayCellEventAsync().ConfigureAwait(false);
            if (eventArgs.IsDisabled && (LocalDates != DateTime.Now))
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED);
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, OVERLAY);
            }
            if (TdEleClass.Contains(FOCUSEDDATE, StringComparison.Ordinal))
            {
                if (Parent is not null)
                {
                    await Parent.UpdateAriaActiveDescendantAsync(eventArgs?.CellData?.CellID).ConfigureAwait(false);
                }
            }
        }

        private static bool GetDateVal(DateTime date, DateTime dateValue)
        {
            return date.Day == dateValue.Day && date.Month == dateValue.Month && date.Year == dateValue.Year;
        }

        /// <summary>
        /// Updates the focused visual state of the cell depending on navigation, selection, and cell status.
        /// Adds or removes the "focused date" class as needed, ensuring visual focus for accessibility and navigation logic.
        /// </summary>
        /// <param name="otherMonth">Indicates if the cell is styled as belonging to another month.</param>
        /// <param name="disabled">Indicates if the cell is visually or logically disabled.</param>
        /// <param name="localDate">The cell's localized date value.</param>
        /// <param name="currentDate">The current visible, navigated, or selected date reference.</param>
        /// <remarks>
        /// The method manages both visual CSS focus and associated global cell state across navigation and focus transitions. Used during selection logic and navigation edge-cases.
        /// </remarks>
        private void UpdateFocus(bool otherMonth, bool disabled, DateTime localDate, DateTime currentDate)
        {
            if (currentDate.Day == localDate.Day && !otherMonth && !disabled && IsFocusTodayDate)
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, FOCUSEDDATE);
                CellDetails? previousListData = Parent?.PreviousCellListData is not null ? Parent.PreviousCellListData.Where(item => item.CellID == localDate.Ticks + "_" + Cells)?.FirstOrDefault() : null;
                if (previousListData is not null && previousListData.ClassList is not null && !previousListData.ClassList.Contains(FOCUSEDDATE, StringComparison.Ordinal))
                {
                    RemoveFocusedDate();
                    previousListData.ClassList += SPACE + FOCUSEDDATE;
                    CellClass = previousListData.ClassList;
                }
                else
                {
                    RemoveFocusedDate();
                }
                CellDetails? listData = Parent?.CellListData is not null ? Parent.CellListData.Where(item => item.CellID == localDate.Ticks + "_" + Cells)?.FirstOrDefault() : null;
                if ((listData is not null && listData.ClassList is not null && !listData.ClassList.Contains(FOCUSEDDATE, StringComparison.Ordinal)) || (previousListData is not null && previousListData == listData))
                {
                    RemoveFocusedDate();
                    listData.ClassList += SPACE + FOCUSEDDATE;
                    CellClass = listData.ClassList;
                }
                else
                {
                    RemoveFocusedDate();
                }
            }
            else
            {
                CellClass = CellClass is not null ? CellClass.Contains(FOCUSEDDATE, StringComparison.Ordinal) ? SfBaseUtils.RemoveClass(CellClass, FOCUSEDDATE) : CellClass : CellClass;
                bool checkMinFocus = currentDate >= Parent?.Max && localDate == Parent.Max && !otherMonth && !disabled;
                bool checkMaxFocus = currentDate <= Parent?.Min && localDate == Parent.Min && !otherMonth && !disabled;
                TdEleClass = (checkMinFocus || (checkMaxFocus && IsFocusTodayDate)) ? SfBaseUtils.AddClass(TdEleClass, FOCUSEDDATE) : TdEleClass;
            }
        }
        /// <summary>
        /// Removes the "focused date" class from all previously-selected or tracked calendar cell data objects.
        /// Helps manage single-focus consistency on navigation and selection transitions.
        /// </summary>
        /// <remarks>
        /// This cleanup is called automatically during navigation or selection logic to ensure that only one cell can have a focused visual state at any one time for UX and accessibility.
        /// </remarks>
        /// <exclude/>
        private void RemoveFocusedDate()
        {
            List<CellDetails> previousCellList = Parent?.PreviousCellListData is not null ? Parent.PreviousCellListData : [];
            foreach (CellDetails item in previousCellList)
            {
                item.ClassList = item.ClassList.Contains(FOCUSEDDATE, StringComparison.Ordinal) ? SfBaseUtils.RemoveClass(item.ClassList, FOCUSEDDATE) : item.ClassList;
            }
            List<CellDetails> cellList = Parent?.CellListData is not null ? Parent.CellListData : [];
            foreach (CellDetails item in cellList)
            {
                item.ClassList = item.ClassList.Contains(FOCUSEDDATE, StringComparison.Ordinal) ? SfBaseUtils.RemoveClass(item.ClassList, FOCUSEDDATE) : item.ClassList;
            }
        }

        /// <summary>
        /// Releases resources and performs cleanup when the calendar base is disposed.
        /// </summary>
        /// <exclude />
        protected override ValueTask DisposeAsyncCore()
        {
            // Clear event callbacks and element refs
            OnCellClick = default;
            _dayCell = default;
            // Remove this cell's details from parent tracking lists to avoid retained references
            if (Parent is not null)
            {
                string id = LocalDates.Ticks + "_" + Cells;
                Parent.CellDetailsData?.RemoveAll(c => c.CellID == id);
                Parent.PreviousCellListData?.RemoveAll(c => c.CellID == id);
            }
            Parent = null;
            return base.DisposeAsyncCore();
        }
    }
}