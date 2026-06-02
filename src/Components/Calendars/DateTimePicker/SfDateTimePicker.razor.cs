using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Calendars.Internal;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// The DateTimePicker is a graphical user interface component that allows users to select both date and time values through an interactive popup interface.
    /// </summary>
    public partial class SfDateTimePicker<TValue> : SfDatePicker<TValue>
    {
        #region Constants

        /// <summary>
        /// CSS class constant for the time icon element.
        /// </summary>
        /// <remarks>
        /// This constant defines the CSS classes applied to the time icon that users click to open the time selection popup.
        /// The classes provide both the icon styling and ensure proper visual representation.
        /// </remarks>
        internal const string TIME_ICON = "e-clock e-toolkit-icons";

        /// <summary>
        /// CSS class constant for individual time list items in the time popup.
        /// </summary>
        /// <remarks>
        /// This constant is used to apply consistent styling to each time option displayed in the time selection list.
        /// </remarks>
        internal const string LIST_ITEM = "e-list-item";

        /// <summary>
        /// CSS class constant indicating the selected state of a time list item.
        /// </summary>
        /// <remarks>
        /// This class is applied to the currently selected time item to provide visual feedback about the active selection.
        /// </remarks>
        internal const string SELECTED = "e-active";

        /// <summary>
        /// CSS class constant for keyboard navigation state of time list items.
        /// </summary>
        /// <remarks>
        /// This class is applied to time list items when they are navigated using keyboard controls,
        /// providing visual indication of the currently focused item during keyboard interaction.
        /// </remarks>
        internal const string NAVIGATION = "e-navigation";

        /// <summary>
        /// CSS class constant for hover state of time list items.
        /// </summary>
        /// <remarks>
        /// This class is applied when users hover over time list items with the mouse,
        /// providing visual feedback before selection.
        /// </remarks>
        internal const string HOVER = "e-hover";

        /// <summary>
        /// Default tab index value for the component's interactive elements.
        /// </summary>
        /// <remarks>
        /// This constant defines the default tab order position for the component in the document's tab sequence.
        /// </remarks>
        internal const int TAB_INDEX = 0;

        #endregion

        #region Private Variables

        /// <summary>
        /// Gets or sets the list of time options available for selection in the time popup.
        /// </summary>
        /// <exclude />
        private List<ListOptions<TValue>>? ListData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the time popup list should be displayed.
        /// </summary>
        /// <exclude />
        private bool ShowPopupList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the time list has been rendered on the client side.
        /// </summary>
       /// <exclude />
        private bool IsListRendered { get; set; }

        /// <summary>
        /// Gets or sets the ARIA active descendant ID for accessibility support in the time list.
        /// </summary>
        /// <exclude />
        private string? AriaActiveDescendantID { get; set; }

        #endregion

        #region Protected Variables

        /// <summary>
        /// Gets or sets the root class of the component.
        /// </summary>
        /// <exclude/>
        protected override string ROOT { get; set; } = "e-control e-datetimepicker e-lib";

        /// <summary>
        /// Gets or sets the CSS class string for the time icon displayed in the component.
        /// </summary>
        /// <exclude/>
        protected string TimeIcon { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the container class of the component.
        /// </summary>
        /// <exclude/>
        protected override string CONTAINERCLASS { get; set; } = "e-control-wrapper e-datetime-wrapper e-control-container e-datetime-container";

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles keyboard actions when time list popup is visible and processes navigation and selection keys.
        /// If time popup is not rendered, it delegates to the DatePicker's keyboard handling logic.
        /// </summary>
        /// <param name="args">The keyboard event arguments containing information about the pressed key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method provides a unified keyboard handling strategy for the DateTimePicker:
        /// 1. When no popup is open: Delegates to DatePicker's keyboard handling (Alt+Down opens calendar).
        /// 2. When calendar popup is open: Uses DatePicker's keyboard handling. Alt+Down arrow opens time popup.
        /// 3. When time popup is open: Handles time list navigation, selection, and closure using Alt+Up arrow.
        /// </remarks>
        private async Task KeyActionHandlerAsync(KeyboardEventArgs args)
        {
            if (args is null)
            {
                return;
            }
            await InvokeAsync(async () =>
            {
                // If time popup is rendered, handle time list keyboard actions
                if (IsListRender)
                {
                    await HandleTimePopupKeyboardAsync(args).ConfigureAwait(false);
                }
                // If time popup is not rendered, delegate to DatePicker's keyboard handling
                else
                {
                    await HandleDatePickerKeyboardAsync(args).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);

            if (OnKeyDown.HasDelegate)
            {
                await OnKeyDown.InvokeAsync(args).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handles keyboard actions specific to the time popup list when it is visible.
        /// Processes arrow keys for navigation, Enter for selection, and Alt+Up to close the popup.
        /// </summary>
        /// <param name="args">The keyboard event arguments containing information about the pressed key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method manages time list navigation using arrow keys (Up/Down), Home/End keys,
        /// and handles selection with Enter key. Alt+Up arrow closes the time popup.
        /// </remarks>
        private async Task HandleTimePopupKeyboardAsync(KeyboardEventArgs args)
        {
            // Handle Alt+Up arrow to close time popup
            if (args.Code == "ArrowUp" && args.AltKey)
            {
                await HidePopupAsync().ConfigureAwait(false);
                return;
            }

            switch (args.Code)
            {
                case "ArrowDown":
                case "Home":
                case "End":
                case "ArrowUp":
                    // Handle arrow key navigation (only if Alt key is not pressed for ArrowUp/ArrowDown)
                    if ((args.Code != "ArrowUp" && args.Code != "ArrowDown") || !args.AltKey)
                    {
                        await KeyHandlerAsync(args).ConfigureAwait(false);
                        await Task.Yield();
                        await InvokeVoidAsync(_datePickerJsModule, _datePickerJsInProcessModule, "updateScrollPosition", [DataId]).ConfigureAwait(true);
                    }
                    break;

                case "Enter":
                    // Handle selection of time list item
                    await HandleTimeListSelectionAsync(args).ConfigureAwait(false);
                    break;

                case "Escape":
                    // Close time popup with Escape key
                    await HidePopupAsync().ConfigureAwait(false);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Handles keyboard actions when the time popup is not rendered.
        /// Delegates to DatePicker's keyboard handling and manages transitions between date and time popups.
        /// </summary>
        /// <param name="args">The keyboard event arguments containing information about the pressed key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// When the time popup is not visible, this method uses DatePicker's keyboard handling logic.
        /// It also handles special cases:
        /// - When no popup is open and Alt+Down is pressed, open calendar popup
        /// - When calendar popup is open and Alt+Down is pressed, open time popup
        /// - When calendar popup is open and Alt+Up is pressed, close calendar popup and return focus
        /// </remarks>
        private async Task HandleDatePickerKeyboardAsync(KeyboardEventArgs args)
        {
            // Handle Alt+Down arrow
            if (args.Code == "ArrowDown" && args.AltKey)
            {
                if (!IsCalendarRender && !IsListRender)
                {
                    await InvokeAsync(async () =>
                    {
                        IsDatePickerPopup = true;
                        await StrictModeUpdateAsync().ConfigureAwait(false);
                        await UpdateInputAsync().ConfigureAwait(false);
                        await ChangeTriggerAsync(args).ConfigureAwait(false);
                        await OpenPopupAsync(args).ConfigureAwait(false);
                        StateHasChanged();
                    }).ConfigureAwait(false);
                }
                else if (IsCalendarRender && !IsListRender)
                {
                    await InvokeAsync(KeyboardTimePopupActionAsync).ConfigureAwait(false);
                }
            }
            // Handle Alt+Up arrow: close calendar popup and return focus
            else if (args.Code == "ArrowUp" && args.AltKey)
            {
                if (IsListRender)
                {
                    await HidePopupAsync().ConfigureAwait(false);
                }
                else if (IsCalendarRender)
                {
                    await InvokeAsync(async () =>
                    {
                        await HidePopupAsync().ConfigureAwait(false);
                        await FocusAsync().ConfigureAwait(false);
                    }).ConfigureAwait(false);
                }
            }
            // For all other keys, delegate to DatePicker's keyboard handling
            else
            {
                await DelegateToDatePickerKeyboardAsync(args).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Delegates keyboard handling to the DatePicker's InputKeyActionHandlerAsync method.
        /// This ensures that DatePicker's keyboard logic is properly applied to the DateTimePicker.
        /// </summary>
        /// <param name="args">The keyboard event arguments to be processed by DatePicker's handler.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method converts the KeyboardEventArgs to the KeyActions format expected by DatePicker's
        /// InputKeyActionHandlerAsync method, preserving all relevant key information.
        /// </remarks>
        private async Task DelegateToDatePickerKeyboardAsync(KeyboardEventArgs args)
        {
            string keyAction = MapInputKeyToAction(args);
            if (!string.IsNullOrEmpty(keyAction))
            {
                KeyActions keyArgs = new()
                {
                    Action = keyAction,
                    Events = new MouseEventArgs() { },
                    Key = args.Code
                };
                await InputKeyActionHandleAsync(keyArgs, CurrentValueAsString, true).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handles the selection of a time list item when the Enter key is pressed.
        /// </summary>
        /// <param name="args">The keyboard event arguments from the Enter key press.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method finds the currently navigated or selected time list item and processes its selection.
        /// It updates the component value, triggers the Selected event, and closes the time popup.
        /// </remarks>
        private async Task HandleTimeListSelectionAsync(KeyboardEventArgs args)
        {
            // Find the currently navigated or selected time list item
            ListOptions<TValue>? findItems = ListData?.Where(item =>
                item.ListClass.Contains(NAVIGATION, StringComparison.CurrentCultureIgnoreCase) ||
                item.ListClass.Contains(SELECTED, StringComparison.Ordinal)).FirstOrDefault();

            if (findItems is not null)
            {
                UpdateListSelection(findItems.ItemData, SELECTED);
            }

            await StrictModeUpdateAsync().ConfigureAwait(false);
            await SelectTimeListAsync().ConfigureAwait(false);

            if (findItems is not null)
            {
                await InvokeSelectEventAsync(new SelectedEventArgs<TValue>() { Value = findItems.DateTimeValue }).ConfigureAwait(false);
            }

            await ChangeTriggerAsync(args).ConfigureAwait(false);
            UpdateErrorClass();
            await HidePopupAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Processes keyboard navigation within the time list and updates the selected item based on key input.
        /// </summary>
        /// <param name="args">The keyboard event arguments containing the navigation key information.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method handles the logic for navigating through time list items using arrow keys, Home, and End keys.
        /// It generates the time list if needed, calculates the appropriate active index, and updates both the
        /// selection state and the component value based on the navigation action.
        /// </remarks>
        private async Task KeyHandlerAsync(KeyboardEventArgs args)
        {
            if (Step > 0 && ListData is null)
            {
                GenerateList();
            }

            int? listCount = ListData?.Count;
            int? activeIndex = null;
            if (string.IsNullOrEmpty(CurrentValueAsString) && Value is null)
            {
                activeIndex = (args.Code == "End") ? (int)listCount! - 1 : 0;
            }
            else
            {
                ListOptions<TValue>? selectedData = ListData?.Where(listItem => listItem.ListClass.Contains(SELECTED, StringComparison.CurrentCulture)).FirstOrDefault();
                ListOptions<TValue>? navigationData = ListData?.Where(listItem => listItem.ListClass.Contains(NAVIGATION, StringComparison.CurrentCulture)).FirstOrDefault();
                ListOptions<TValue>? findItems = navigationData ?? selectedData;
                activeIndex = findItems is not null ? ListData?.IndexOf(findItems) : 0;
                activeIndex = (args.Code == "ArrowDown") ? ++activeIndex : (args.Code == "ArrowUp") ? --activeIndex : (args.Code == "Home") ? 0 : (int)listCount! - 1;
            }

            activeIndex = (activeIndex >= 0) ? activeIndex : ListData?.Count + activeIndex;
            ListOptions<TValue>? selectItem = ListData?.ElementAtOrDefault((int)activeIndex!);
            if (selectItem is not null)
            {
                UpdateListSelection(selectItem.ItemData, NAVIGATION);
                await UpdateValueAsync(selectItem.DateTimeValue).ConfigureAwait(false);
                await SelectTimeListAsync().ConfigureAwait(false);
                if (EnableMask && ShowPopupList && CurrentValueAsString is not null)
                {
                    CurrentMaskValue = CurrentValueAsString;
                }
                await InvokeVoidAsync(_datePickerJsModule!, _datePickerJsInProcessModule!, "updateScrollPosition", [DataId, true, activeIndex!]).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Updates the selection state of time list items by applying appropriate CSS classes.
        /// </summary>
        /// <param name="item">The item data string that identifies which list item should be updated.</param>
        /// <param name="className">The CSS class name to be applied to the selected item (SELECTED, NAVIGATION, or HOVER).</param>
        /// <remarks>
        /// This method manages the visual state of time list items by removing previous state classes
        /// and applying the new state class to the appropriate item. It also updates ARIA attributes
        /// for accessibility compliance when items are navigated using keyboard.
        /// </remarks>
        private void UpdateListSelection(string item, string className)
        {
            if (ListData is null)
            {
                return;
            }
            ListOptions<TValue>? currentNavItem = null;
            int currentNavIndex = -1;
            for (int i = 0; i < ListData.Count; i++)
            {
                ListOptions<TValue> listItem = ListData[i];
                // Remove existing HOVER class
                if (listItem.ListClass.Contains(HOVER, StringComparison.CurrentCulture))
                {
                    listItem.ListClass = SfBaseUtils.RemoveClass(listItem.ListClass, HOVER);
                }
                // Remove HOVER from NAVIGATION items
                if (listItem.ListClass.Contains(NAVIGATION, StringComparison.CurrentCulture))
                {
                    listItem.ListClass = SfBaseUtils.RemoveClass(listItem.ListClass, HOVER);
                    // Track last NAVIGATION item for aria
                    currentNavItem = listItem;
                    currentNavIndex = i;
                }
                // Remove existing selected class
                if (listItem.ListClass.Contains(className, StringComparison.CurrentCulture))
                {
                    listItem.ListClass = SfBaseUtils.RemoveClass(listItem.ListClass, className);
                }
                // Apply new class to matching item
                if (SfBaseUtils.Equals(listItem.ItemData, item))
                {
                    listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, className);
                }
            }
            // Update aria active descendant
            AriaActiveDescendantID = currentNavItem is not null ? ID + "_" + currentNavIndex.ToString(CultureInfo.CurrentCulture) : null;
            if (IsListRender && ShowPopupList && !string.IsNullOrEmpty(AriaActiveDescendantID))
            {
                _ = SfBaseUtils.UpdateDictionary(ARIAACTIVEDESCENDANT, AriaActiveDescendantID, InputHtmlAttributes);
            }
        }

        /// <summary>
        /// Triggers change events and handles value persistence when the component value changes.
        /// </summary>
        /// <param name="args">The event arguments associated with the change, or <c>null</c> if the change was programmatic.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method compares the current value with the previous value and triggers the ValueChange event
        /// if they differ. It also handles value persistence if enabled and updates validation classes.
        /// The method ensures proper state synchronization and event propagation for value changes.
        /// </remarks>
        private async Task ChangeTriggerAsync(EventArgs? args = null)
        {
            if (CurrentValueAsString != PreviousElementValue && PreviousDate is not null && SfTimePickerUtils.CompareValues(PreviousDate, Value) != 0)
            {
                if (ValueChange.HasDelegate)
                {
                    ChangedEventArgs<TValue> changedEventArgs = new()
                    {
                        Value = Value!,
                        Event = args is null ? new() : args,
                        IsInteracted = args is not null,
                        Element = InputElement
                    };
                    _ = InvokeAsync(() => ValueChange.InvokeAsync(changedEventArgs));
                }
                if (EnablePersistence)
                {
                    await SetLocalStorageAsync(ID, Value!).ConfigureAwait(false);
                }

                PreviousElementValue = CurrentValueAsString;
                PreviousDate = Value;
                if (CalendarMode == CalendarType.Islamic)
                {
                    PreviousElementValue = IslamicValueAsString;
                }
            }

            UpdateValidateClass();
        }

        /// <summary>
        /// Updates the input display value when a time is selected from the time list.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method formats the current component value according to the specified format or default
        /// culture format and updates the input field display. It ensures that the displayed value
        /// reflects the selected time in the appropriate format.
        /// </remarks>
        private async Task SelectTimeListAsync()
        {
            string date = string.Empty;
            if (Value is not null)
            {
                string formatString = !string.IsNullOrEmpty(Format) ? Format : CurrentCulture?.DateTimeFormat.ShortDatePattern + " " + CurrentCulture?.DateTimeFormat.ShortTimePattern;
                date = SfDateTimePicker<TValue>.FormatDateValue(Value, formatString);
            }

            await UpdateInputValueAsync(date).ConfigureAwait(false);
        }

        /// <summary>
        /// Formats a time value according to the specified format string and current culture settings.
        /// </summary>
        /// <param name="timeValue">The time value to be formatted.</param>
        /// <param name="formatString">The format string that defines how the value should be displayed.</param>
        /// <returns>A <c>string</c> representation of the formatted time value.</returns>
        /// <remarks>
        /// This method uses the Intl formatting utilities to ensure proper localization and formatting
        /// of date and time values according to the current calendar locale and format specifications.
        /// </remarks>
        private static string FormatDateValue(TValue timeValue, string formatString)
        {
            return Intl.GetDateFormat(timeValue, formatString);
        }

        /// <summary>
        /// Updates the input element's displayed value and manages associated UI elements.
        /// </summary>
        /// <param name="timeValue">The formatted string value to be displayed in the input element.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method calls the SetValue method to update the input element's display value
        /// and coordinate with floating label behavior and clear button visibility.
        /// </remarks>
        private async Task UpdateInputValueAsync(string timeValue)
        {
            await SetValueAsync(timeValue, FloatLabelType, ShowClearButton).ConfigureAwait(false);
        }

        /// <summary>
        /// Calculates the start time for the time list generation based on the provided date and component constraints.
        /// </summary>
        /// <param name="startDate">The date for which to calculate the start time.</param>
        /// <returns>A <see cref="DateTime"/> representing the calculated start time for the time list.</returns>
        /// <remarks>
        /// This method determines the appropriate start time for generating the time list by considering
        /// the minimum date constraints, current date boundaries, and component settings. It ensures
        /// that the time list begins at a valid time according to all constraints.
        /// </remarks>
        private DateTime StartTime(DateTime startDate)
        {
            DateTime tempMin = Min;
            bool start = false;
            DateTime tempStartValue = default;
            if ((startDate.Date == tempMin.Date && startDate.Month == tempMin.Month && startDate.Year == tempMin.Year) || (new DateTime(startDate.Year, startDate.Month, startDate.Day) <= new DateTime(tempMin.Year, tempMin.Month, tempMin.Day)))
            {
                start = false;
                tempStartValue = Min;
            }
            else if (startDate.Ticks < Max.Ticks && startDate.Ticks > Min.Ticks)
            {
                start = true;
                tempStartValue = startDate;
            }
            else if (startDate.Ticks >= Max.Ticks)
            {
                start = true;
                tempStartValue = Max;
            }

            return CalculateStartEnd(tempStartValue, start, "starttime");
        }

        /// <summary>
        /// Calculates the end time for the time list generation based on the provided date and component constraints.
        /// </summary>
        /// <param name="endDate">The date for which to calculate the end time.</param>
        /// <returns>A <see cref="DateTime"/> representing the calculated end time for the time list.</returns>
        /// <remarks>
        /// This method determines the appropriate end time for generating the time list by considering
        /// the maximum date constraints, current date boundaries, and component settings. It ensures
        /// that the time list ends at a valid time according to all constraints.
        /// </remarks>
        private DateTime EndTime(DateTime endDate)
        {
            DateTime tempMax = Max;
            bool end = false;
            DateTime tempEndValue = default;
            if ((endDate.Date == tempMax.Date && endDate.Month == tempMax.Month && endDate.Year == tempMax.Year) || (new DateTime(endDate.Year, endDate.Month, endDate.Day) >= new DateTime(tempMax.Year, tempMax.Month, tempMax.Day)))
            {
                end = false;
                tempEndValue = Max;
            }
            else if (endDate.Ticks < Max.Ticks && endDate.Ticks > Min.Ticks)
            {
                end = true;
                tempEndValue = endDate;
            }
            else if (endDate.Ticks <= Min.Ticks)
            {
                end = true;
                tempEndValue = Min;
            }

            return CalculateStartEnd(tempEndValue, end, "endtime");
        }

        /// <summary>
        /// Calculates the start or end time based on the provided date value, range flag, and method type.
        /// </summary>
        /// <param name="dateValue">The base date value to use for calculation.</param>
        /// <param name="range">A <c>bool</c> value indicating whether to use range-based calculation (start/end of day) or preserve the exact time.</param>
        /// <param name="method">A <c>string</c> specifying the calculation method - "starttime" for day start or "endtime" for day end.</param>
        /// <returns>A <see cref="DateTime"/> representing the calculated start or end time.</returns>
        /// <remarks>
        /// This method provides flexible time calculation logic that can return either the start of day (00:00:00),
        /// end of day (23:59:59), or the exact time from the input value based on the range parameter.
        /// It's used internally by StartTime and EndTime methods for consistent time boundary calculations.
        /// </remarks>
        private static DateTime CalculateStartEnd(DateTime dateValue, bool range, string method)
        {
            int day = dateValue.Day;
            int month = dateValue.Month;
            int year = dateValue.Year;
            int hours = dateValue.Hour;
            int minutes = dateValue.Minute;
            int seconds = dateValue.Second;
            int milliseconds = dateValue.Millisecond;
            return range
                ? (method == "starttime") ? new DateTime(year, month, day, 0, 0, 0) : new DateTime(year, month, day, 23, 59, 59)
                : new DateTime(year, month, day, hours, minutes, seconds, milliseconds);
        }

        /// <summary>
        /// Generates the list of time options for the time selection popup based on component settings.
        /// </summary>
        /// <remarks>
        /// This method creates a list of selectable time options based on the Step interval, MinTime, MaxTime,
        /// and current date constraints. It generates ListOptions with formatted display text and handles
        /// the selection state for the current value. The generated list is used to populate the time popup.
        /// </remarks>
        private void GenerateList()
        {
            if (Step > 0)
            {
                DateTime datetimeValue = Value is not null ? ConvertDate(Value) : DateTime.Now;
                TimeSpan start = StartTime(datetimeValue).TimeOfDay;
                TimeSpan end = EndTime(datetimeValue).TimeOfDay;
                TimeSpan interval = new(0, Step, 0);
                start = MinTime.TimeOfDay > start ? MinTime.TimeOfDay : start;
                end = MaxTime.TimeOfDay < end ? MaxTime.TimeOfDay : end;
                ListData = [];
                string? formatString = string.IsNullOrEmpty(TimeFormat) ? CurrentCulture?.DateTimeFormat.ShortTimePattern : TimeFormat;
                while (end >= start)
                {
                    datetimeValue = StartTime(datetimeValue);
                    DateTime listDate = new(datetimeValue.Year, datetimeValue.Month, datetimeValue.Day, start.Hours, start.Minutes, start.Seconds, start.Milliseconds, DateTimeKind.Local);
                    ListOptions<TValue> listItem = new()
                    {
                        DateTimeValue = ConvertGeneric(listDate),
                        ItemData = Intl.GetDateFormat(listDate, formatString),
                        ListClass = LIST_ITEM
                    };
                    if (Value is not null && SfBaseUtils.Equals(ConvertDate(Value), listDate))
                    {
                        listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, SELECTED);
                    }

                    ListData.Add(listItem);
                    start = start.Add(interval);
                }
            }
        }

        /// <summary>
        /// Validates the current time value against MinTime and MaxTime constraints and applies strict mode corrections.
        /// </summary>
        /// <remarks>
        /// This method checks if the current component value's time portion falls within the acceptable time range.
        /// In strict mode, it automatically corrects invalid times to the nearest valid boundary.
        /// Otherwise, it applies error styling to indicate the validation failure to users.
        /// </remarks>
        private async Task IsValidTimeAsync()
        {
            if (Value is not null)
            {
                DateTime datetimeValue = ConvertDate(Value);
                TimeSpan dateTime = datetimeValue.TimeOfDay;
                TimeSpan minTime = MinTime.TimeOfDay;
                TimeSpan maxTime = MaxTime.TimeOfDay;
                if (dateTime < minTime || dateTime > maxTime)
                {
                    if (StrictMode && !IsFocused)
                    {
                        DateTime targetTime = dateTime < minTime ? MinTime : MaxTime;
                        await UpdateValueAsync(ConvertGeneric(new DateTime(datetimeValue.Year, datetimeValue.Month, datetimeValue.Day, targetTime.Hour, targetTime.Minute, targetTime.Second))).ConfigureAwait(false);
                        if (ChangedArgs is not null)
                        {
                            ChangedArgs.Value = Value;
                        }
                    }
                    else
                    {
                        ContainerClass = SfBaseUtils.AddClass(ContainerClass, ERRORCLASS);
                        InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIAINVALID, TRUE, InputHtmlAttributes);
                    }
                }
                else if (datetimeValue > Min && datetimeValue < Max)
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERRORCLASS);
                    InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIAINVALID, FALSE, InputHtmlAttributes);
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Handles keyboard action to open the time popup when specific key combinations are pressed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is typically called when Alt+Down arrow keys are pressed to open the time selection popup.
        /// It first hides any existing popup and then shows the time popup if it's not already rendered.
        /// </remarks>
        /// <exclude/>
        protected override async Task KeyboardTimePopupActionAsync()
        {
            await HidePopupAsync().ConfigureAwait(false);
            IsDatePickerPopup = false;
            if (TimeIcon is not null)
            {
                TimeIcon = SfBaseUtils.AddClass(TimeIcon, ACTIVE);
            }

            await Task.Yield();
            if (!IsListRender)
            {
                await OpenPopupAsync(null).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handles the client-side rendering of popups for both date and time selection.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method determines whether to render a date picker popup or time selection list popup based on the current state.
        /// It manages the popup container classes and invokes appropriate client-side JavaScript methods for popup rendering.
        /// The method handles different rendering scenarios for date selection calendar and time selection list.
        /// </remarks>
        /// <exclude/>
        protected override async Task ClientPopupRenderAsync()
        {
            if (IsDatePickerPopup)
            {
                PopupContainer = SfBaseUtils.AddClass(PopupContainer, DATEPICKER);
                await base.ClientPopupRenderAsync().ConfigureAwait(false);
            }
            else if (ShowPopupList && IsListRendered)
            {
                IsListRendered = false;
                DatePickerClientProps<TValue> options = GetClientProperties();
                await InvokeVoidAsync(_datePickerJsModule, _datePickerJsInProcessModule, "renderPopup", [DataId, PopupElement, PopupHolderEle, PopupEventArgs, options, Step, ScrollTo ?? default]).ConfigureAwait(true);
                IsListRender = true;
            }
        }

        /// <summary>
        /// Triggers events when the popup is opened or closed and manages the time list generation.
        /// </summary>
        /// <param name="isOpen">A <c>bool</c> value indicating whether the popup is in the opened state.</param>
        /// <param name="args">The event arguments associated with the popup action.</param>
        /// <returns>A <see cref="Task{PopupObjectArgs}"/> containing the popup event arguments with cancellation and prevention options.</returns>
        /// <remarks>
        /// This method handles both popup opening and closing events by invoking the appropriate DateTimePicker events.
        /// When opening a time popup (not date picker popup), it automatically generates the time selection list.
        /// The method creates and configures popup event arguments that can be used to cancel or modify popup behavior.
        /// </remarks>
        /// <exclude/>
        protected override async Task<PopupObjectArgs> InvokeOpenEventAsync(bool isOpen, EventArgs? args = null)
        {
            PopupObjectArgs openEventArgs = new()
            {
                Cancel = false,
                Event = args is null ? new() : args,
                PreventDefault = false
            };
            if (!IsDatePickerPopup && isOpen)
            {
                GenerateList();
            }

            PopupEventArgs = new DatePickerPopupArgs { AppendTo = IsDevice ? MODEL : BODY, Cancel = false, Event = args is null ? new() : args, PreventDefault = false };
            await SfBaseUtils.InvokeEventAsync(isOpen ? OnOpen : OnClose, openEventArgs).ConfigureAwait(false);
            return openEventArgs;
        }

        /// <summary>
        /// Handles the click event on the time icon and manages the time popup display logic.
        /// </summary>
        /// <param name="eventArgs">The event arguments from the time icon click event.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method manages the time icon click behavior including validation of component state, handling device-specific logic,
        /// and toggling between time popup visibility. It also manages the visual state of the time icon and handles
        /// focus management for different interaction scenarios.
        /// </remarks>
        /// <exclude/>
        protected override async Task TimeIconHandlerAsync(EventArgs eventArgs)
        {
            if (!Disabled && eventArgs is not null)
            {
                bool isDisabled = (InputHtmlAttributes is not null && InputHtmlAttributes.ContainsKey("disabled")) || (HtmlAttributes is not null && HtmlAttributes.ContainsKey("disabled"));
                if (isDisabled)
                {
                    return;
                }

                await Task.Delay(10).ConfigureAwait(false); // set the delay for prevent the icon click action.
                if (IsDevice)
                {
                    _ = SfBaseUtils.UpdateDictionary(READONLYATTR, true, InputHtmlAttributes);
                    await FocusOutAsync().ConfigureAwait(false);
                }
                else
                {
                    PreventIconHandler = true;
                }

                if (IsListRender)
                {
                    await HidePopupAsync(eventArgs).ConfigureAwait(false);
                }
                else
                {
                    if (IsCalendarRender)
                    {
                        await HidePopupAsync(eventArgs).ConfigureAwait(false);
                    }

                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUTFOCUS);
                    if (!((!Disabled && Readonly) || Disabled))
                    {
                        TimeIcon = SfBaseUtils.AddClass(TimeIcon, ACTIVE);
                    }

                    await SetReadOnlyFocusAsync().ConfigureAwait(false);
                    await OpenPopupAsync(eventArgs).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Hides the time popup and resets the popup list display state.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called to close the time selection popup and update the internal state to reflect
        /// that the popup is no longer visible. It ensures proper cleanup of popup-related states.
        /// </remarks>
        /// <exclude/>
        protected override async Task HideTimePopupAsync()
        {
            await HidePopupAsync().ConfigureAwait(false);
            ShowPopupList = false;
        }

        /// <summary>
        /// Triggered when the value of the component changes and handles the value change event propagation.
        /// </summary>
        /// <param name="args">The event arguments associated with the change event.</param>
        /// <param name="isSelection">A <c>bool</c> value that determines whether the change was made using mouse or keyboard selection.</param>
        /// <remarks>
        /// This method compares the current value with the previous value and triggers the ValueChange event if they differ.
        /// It also handles persistence of the value if EnablePersistence is enabled and manages calendar selection state.
        /// The method ensures proper event propagation and state management for value changes.
        /// </remarks>
        /// <exclude/>
        protected override void ChangeEvent(EventArgs? args, bool isSelection = false)
        {
            if (!SfBaseUtils.Equals(Value, PreviousDate))
            {
                _ = SelectCalendarAsync();
                if (ValueChange.HasDelegate && !(Disabled || Readonly))
                {
                    ChangedEventArgs = new ChangedEventArgs<TValue>()
                    {
                        Value = ChangedArgs.Value,
                        Event = args is null ? new() : args,
                        IsInteracted = args is not null
                    };
                    _ = InvokeAsync(() => ValueChange.InvokeAsync(ChangedEventArgs));
                }
                if (EnablePersistence)
                {
                    _ = SetLocalStorageAsync(ID, Value!);
                }

                PreviousDate = Value;
                PreviousElementValue = CurrentValueAsString;
                if (CalendarMode == CalendarType.Islamic)
                {
                    PreviousElementValue = IslamicValueAsString;
                }
            }
        }

        /// <summary>
        /// Updates the internal state variables for date and time popup visibility and manages accessibility attributes.
        /// </summary>
        /// <param name="isOpen">A <c>bool</c> value indicating whether the popup should be in the open state.</param>
        /// <remarks>
        /// This method synchronizes multiple state variables related to popup rendering and visibility.
        /// It manages ARIA attributes for accessibility compliance and updates the visual state of the time icon.
        /// The method also triggers a state change notification to update the component's UI.
        /// </remarks>
        /// <exclude/>
        protected override void UpdateDateTimePopupState(bool isOpen)
        {
            IsListRender = ShowPopupList = IsListRendered = isOpen;
            if (isOpen)
            {
                _ = SfBaseUtils.UpdateDictionary(ARIAEXPANDED, TRUE, InputHtmlAttributes);
                if (!string.IsNullOrEmpty(AriaActiveDescendantID))
                {
                    _ = SfBaseUtils.UpdateDictionary(ARIAACTIVEDESCENDANT, AriaActiveDescendantID, InputHtmlAttributes);
                }
            }
            else
            {
                TimeIcon = SfBaseUtils.RemoveClass(TimeIcon, ACTIVE);
            }

        }

        /// <summary>
        /// Triggers when the component receives focus and handles the Focus event and OpenOnFocus behavior.
        /// </summary>
        /// <param name="args">The focus event arguments from the input element.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method invokes the OnFocus event callback if one has been assigned and automatically opens the popup
        /// if the OpenOnFocus property is enabled. This provides convenient behavior for users who want the popup
        /// to appear automatically when the component receives focus.
        /// </remarks>
        /// <exclude/>
        protected override async Task InvokeFocusEventAsync(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (OnFocus.HasDelegate)
            {
                await OnFocus.InvokeAsync(new FocusEventArgs()).ConfigureAwait(false);
            }

            if (OpenOnFocus)
            {
                await OpenPopupAsync().ConfigureAwait(false);

            }
        }

        /// <summary>
        /// Triggers when the component loses focus and handles cleanup and validation tasks.
        /// </summary>
        /// <param name="args">The focus event arguments from the input element losing focus.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method performs several cleanup tasks when the component loses focus including removing active states
        /// from icons, validating the current time value, and invoking the OnBlur event callback if assigned.
        /// The time validation ensures that the selected time falls within the acceptable range.
        /// </remarks>
        /// <exclude/>
        protected override async Task InvokeBlurEventAsync(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            DateIcon = SfBaseUtils.RemoveClass(DateIcon, ACTIVE);
            TimeIcon = SfBaseUtils.RemoveClass(TimeIcon, ACTIVE);
            await IsValidTimeAsync().ConfigureAwait(false);
            if (OnBlur.HasDelegate)
            {
                await OnBlur.InvokeAsync(new BlurEventArgs()).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handles mouse click events on time list items and processes the selection.
        /// </summary>
        /// <param name="listItem">The time list item that was clicked, containing the time value and display information.</param>
        /// <param name="eventArgs">The mouse event arguments from the click event.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method processes clicks on time list items by updating the selection state, hiding the popup,
        /// invoking selection events, and updating the component value. It also handles mask creation if masking
        /// is enabled and triggers change events to notify of the value update.
        /// </remarks>
        /// <exclude/>
        protected async Task OnMouseClickAsync(ListOptions<TValue> listItem, MouseEventArgs eventArgs)
        {
            if (listItem is not null && listItem.ItemData is not null)
            {
                if (IsListRender)
                {
                    await HidePopupAsync(eventArgs).ConfigureAwait(false);
                }

                UpdateListSelection(listItem.ItemData, SELECTED);
                await InvokeSelectEventAsync(new SelectedEventArgs<TValue>() { Value = listItem.DateTimeValue }).ConfigureAwait(false);
                await UpdateValueAsync(listItem.DateTimeValue).ConfigureAwait(false);
            }
            IsChangeValue = true;
            if (EnableMask && IsRendered)
            {
                await CreateMaskAsync().ConfigureAwait(false);
            }

            await SelectTimeListAsync().ConfigureAwait(false);
            await ChangeTriggerAsync(eventArgs).ConfigureAwait(false);
        }

        /// <summary>
        /// Triggers the ValueChange event when the component value is modified.
        /// </summary>
        /// <param name="args">The event arguments associated with the change, or <c>null</c> if the change was programmatic.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method creates and invokes the ValueChange event callback with the current component value and interaction state.
        /// It respects the component's enabled and readonly states to prevent unwanted event firing.
        /// The IsInteracted property in the event arguments indicates whether the change was user-initiated.
        /// </remarks>
        /// <exclude/>
        protected override async Task InvokeChangeEventAsync(EventArgs? args = null)
        {
            if (ValueChange.HasDelegate && !(Disabled || Readonly))
            {
                ChangedEventArgs = new ChangedEventArgs<TValue>()
                {
                    Value = Value!,
                    Event = args is null ? new() : args,
                    IsInteracted = args is not null
                };
                await ValueChange.InvokeAsync(ChangedEventArgs).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Converts a DateTime value to the generic type TValue used by the component.
        /// </summary>
        /// <param name="dateValue">The DateTime value to be converted to the target generic type.</param>
        /// <returns>The converted value of type TValue, which can be DateTime, DateTimeOffset, or their nullable variants.</returns>
        /// <remarks>
        /// This method handles the conversion between DateTime and DateTimeOffset types, preserving the appropriate
        /// time zone information when dealing with DateTimeOffset values. It ensures that the component can work
        /// with different date-time types seamlessly.
        /// </remarks>
        /// <exclude/>
        protected override TValue ConvertGeneric(DateTime dateValue)
        {
            if (IsDateTimeOffsetType())
            {
                DateTimeOffset dynamicDateValue = dateValue != default ? dateValue : default(DateTimeOffset);
                int year = dynamicDateValue.Year;
                int month = dynamicDateValue.Month;
                int day = dynamicDateValue.Day;
                TimeSpan offset = dynamicDateValue.Offset;
                DateTimeOffset offsetValue = new(year, month, day, dateValue.Hour, dateValue.Minute, dateValue.Second, dateValue.Millisecond, offset);
                return (TValue)SfBaseUtils.ChangeType(offsetValue, typeof(TValue));
            }
            else
            {
                return (TValue)SfBaseUtils.ChangeType(dateValue, typeof(TValue));
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the calendar properties when values are selected from the date picker popup.
        /// </summary>
        /// <param name="key">The property key that identifies which property needs to be updated.</param>
        /// <param name="dateTimeValue">The new date and time value to be applied to the component.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method handles the conversion between different date-time types (DateTime and DateTimeOffset) 
        /// and updates the component value accordingly. It also manages popup state and focus behavior
        /// when values are selected from the calendar popup.
        /// </remarks>
        /// <exclude/>
        internal override async Task UpdateCalendarPropertyAsync(string key, object? dateTimeValue)
        {
            if (key == nameof(Value))
            {
                object? dateValue = null;
                if (!IsDateTimeType() && dateTimeValue is not null)
                {
                    int year = ((DateTimeOffset)dateTimeValue).Year;
                    int month = ((DateTimeOffset)dateTimeValue).Month;
                    int day = ((DateTimeOffset)dateTimeValue).Day;
                    TimeSpan offset = ((DateTimeOffset)dateTimeValue).Offset;
                    int hour = ((DateTimeOffset)dateTimeValue).Hour;
                    int minute = ((DateTimeOffset)dateTimeValue).Minute;
                    int second = ((DateTimeOffset)dateTimeValue).Second;
                    int milliSecond = ((DateTimeOffset)dateTimeValue).Millisecond;
                    dateValue = new DateTimeOffset(year, month, day, hour, minute, second, milliSecond, offset);
                }
                else
                {
                    if (dateTimeValue is not null)
                    {
                        dateValue = new DateTime(((DateTime)dateTimeValue).Ticks, DateTimeKind.Local);
                    }
                }
                await UpdateValueAsync(dateValue).ConfigureAwait(false);
                IsChangeValue = true;
                if (IsCalendarRender)
                {
                    await HidePopupAsync().ConfigureAwait(false);
                    await FocusAsync().ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Triggers the Selected event when a value is chosen from either the date or time popups.
        /// </summary>
        /// <param name="args">The selection event arguments containing the selected value and related information.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called internally when users make a selection from either the calendar popup or time list popup.
        /// It ensures that the Selected event callback is invoked if one has been assigned, allowing consumers
        /// to respond to selection events.
        /// </remarks>
        /// <exclude/>
        internal override async Task InvokeSelectEventAsync(SelectedEventArgs<TValue> args)
        {
            if (Selected.HasDelegate)
            {
                await Selected.InvokeAsync(args).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Binds and invokes the Navigated event when the calendar view is navigated.
        /// </summary>
        /// <param name="eventArgs">The navigation event arguments containing information about the navigation action.</param>
        /// <remarks>
        /// This method is called when users navigate between months, years, or decades in the calendar popup.
        /// It invokes the Navigated event callback if one has been assigned, allowing consumers to respond
        /// to calendar navigation actions.
        /// </remarks>
        /// <exclude/>
        internal override void BindNavigateEvent(NavigatedEventArgs eventArgs)
        {
            if (Navigated.HasDelegate)
            {
                _ = Navigated.InvokeAsync(eventArgs);
            }
        }

        /// <summary>
        /// Binds and invokes the DayCellRendering event for customizing individual day cells in the calendar.
        /// </summary>
        /// <param name="eventArgs">The render day cell event arguments containing day cell information and customization options.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called for each day cell when the calendar is rendered, allowing consumers to
        /// customize the appearance, disable specific dates, or add custom content to day cells.
        /// </remarks>
        /// <exclude/>
        internal override async Task BindRenderDayEventAsync(RenderDayCellEventArgs eventArgs)
        {
            eventArgs.CurrentView = CurrentView();
            if (DayCellRendering.HasDelegate)
            {
                await DayCellRendering.InvokeAsync(eventArgs).ConfigureAwait(false);
            }
        }

        #endregion
    }
}
