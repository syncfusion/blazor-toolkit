using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Calendars.Internal;
using Syncfusion.Blazor.Toolkit.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// The <see cref="SfDatePicker{TValue}"/> component provides a date selection interface for forms and applications in Blazor. It enables users to select dates, open or close the popup calendar, navigate views, set focus, and interact with masking features.
    /// </summary>
    /// <remarks>
    /// The <see cref="SfDatePicker{TValue}"/> supports both keyboard and mouse interaction, offers control over popup state, and provides several helper utilities for managing persisted state and navigation within the date picker UI.
    /// </remarks>
    /// <typeparam name="TValue">Specifies the type used by the date picker for value binding and manipulation.</typeparam>
    /// <example>
    /// The following example demonstrates how to use the <see cref="SfDatePicker{TValue}"/> in a Blazor application.
    /// <code><![CDATA[
    /// <SfDatePicker TValue="DateTime" Placeholder="Choose a date"></SfDatePicker>
    /// ]]></code>
    /// </example>
    public partial class SfDatePicker<TValue> : CalendarBase<TValue>
    {
        /// <summary>
        /// Sets focus to the <see cref="SfDatePicker{TValue}"/> component for user interaction.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation that sets focus to the component.
        /// </returns>
        /// <remarks>
        /// Call this method to programmatically set input focus on the DatePicker, for example after form submission or to assist keyboard navigation.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// await datePickerRef.FocusAsync();
        /// ]]></code>
        /// </example>
        public async Task FocusAsync()
        {
            await InvokeVoidAsync(_datePickerJsModule, _datePickerJsInProcessModule, "focusIn", [DataId, false]).ConfigureAwait(true);
        }

        /// <summary>
        /// Removes input focus from the <see cref="SfDatePicker{TValue}"/> component if it is currently focused.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation that removes focus from the component.
        /// </returns>
        /// <remarks>
        /// Use this method to programmatically blur or remove focus from the DatePicker control, such as when navigating away or validating input.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// await datePickerRef.FocusOutAsync();
        /// ]]></code>
        /// </example>
        public async Task FocusOutAsync()
        {
            await InvokeVoidAsync(_datePickerJsModule, _datePickerJsInProcessModule, "focusOut", [DataId]).ConfigureAwait(true);
        }

        private async Task MoveFocusToPopupAsync()
        {
            await InvokeVoidAsync(_datePickerJsModule, _datePickerJsInProcessModule, "moveFocusToPopup", [DataId]).ConfigureAwait(true);
        }

        /// <summary>
        /// Gets the persisted state properties of the <see cref="SfDatePicker{TValue}"/> for maintaining component state.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{String}"/> representing the asynchronous operation that returns persisted state information as a JSON string.
        /// </returns>
        /// <remarks>
        /// Use this method to fetch the component's persisted state, typically for state handling across reloads or navigation.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// string persistedData = await datePickerRef.GetPersistDataAsync();
        /// ]]></code>
        /// </example>
        public async Task<string> GetPersistDataAsync()
        {
            return await InvokeAsync<string>(_baseJsModule!, _baseJsInProcessModule!, "getLocalStorageItem", [ID]).ConfigureAwait(true);
        }

        /// <summary>
        /// Opens the popup calendar associated with the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <param name="args">The event arguments that trigger the popup. Optional.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation to show the calendar popup.
        /// </returns>
        /// <remarks>
        /// This method opens the calendar popup, allowing the user to select a date from a visual calendar interface. If the DatePicker is not enabled, the popup will not display.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// await datePickerRef.ShowPopupAsync();
        /// ]]></code>
        /// </example>
        public async Task ShowPopupAsync(EventArgs? args = null)
        {
            if (!((!Disabled && Readonly) || Disabled))
            {
                if (IsDevice)
                {
                    CalendarClass = SfBaseUtils.AddClass(CalendarClass, DEVICE);
                    TValue? modelValue = (TValue)SfBaseUtils.ChangeType(Value!, typeof(TValue));
                    if (modelValue is not null)
                    {
                        ModelYear = Intl.GetDateFormat(modelValue, FORMAT_YEAR);
                        ModelDay = Intl.GetDateFormat(modelValue, FORMAT_DAY);
                        ModelMonth = Intl.GetDateFormat(modelValue, FORMAT_MONTH);
                    }
                }
                else
                {
                    CalendarClass = SfBaseUtils.RemoveClass(CalendarClass, DEVICE);
                }
                PopupObjectArgs openEventArgs = await InvokeOpenEventAsync(true, args).ConfigureAwait(false);
                if (!openEventArgs.Cancel)
                {
                    if (!IsDatePickerPopup)
                    {
                        UpdateDateTimePopupState(true);
                    }
                    else
                    {
                        IsCalendarRendered = true;
                        SetPopupVisibility(true);
                    }
                    _ = SfBaseUtils.UpdateDictionary(ARIAEXPANDED, TRUE, InputHtmlAttributes);
                    _ = SfBaseUtils.UpdateDictionary(ARIA_OWN, ID + POPUPS, InputHtmlAttributes);
                }
                await InvokeAsync(StateHasChanged).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Sets the Disabled value based on whether the closest <fieldset></fieldset> is disabled.
        /// </summary>
        /// <param name="isDisabled">
        /// A boolean value indicating whether the component should be disabled.
        /// </param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public Task UpdateFieldSetStatus(bool isDisabled)
        {
            Disabled = isDisabled;
            SetEnabled();
            StateHasChanged();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Hides the calendar popup of the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <param name="args">The event arguments that trigger hiding the popup. Optional.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation to hide the popup calendar.
        /// </returns>
        /// <remarks>
        /// This method closes the popup calendar if it is currently open, restoring the component to its normal state.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// await datePickerRef.HidePopupAsync();
        /// ]]></code>
        /// </example>
        public async Task HidePopupAsync(EventArgs? args = null)
        {
            PopupObjectArgs closeEventArgs = await InvokeOpenEventAsync(false, args).ConfigureAwait(false);
            if (!closeEventArgs.Cancel)
            {
                DatePickerClientProps<TValue> options = new()
                {
                    Readonly = Readonly,
                    Disabled = Disabled,
                    ZIndex = ZIndex,
                    EnableRtl = SyncfusionService!._options.EnableRtl,
                    KeyConfigs = KeyConfigs,
                    ShowClearButton = ShowClearButton,
                    Value = Value!,
                    Width = Width,
                    IsDatePopup = IsDatePickerPopup,
                    AllowEdit = AllowEdit,
                    Depth = Depth.ToString(),
                    EnableMask = EnableMask,
                    Format = Format,
                    IsRendered = IsRendered,
                    FloatLabelType = FloatLabelType.ToString(),
                    IsFocused = IsFocused,
                    DayAbbreviatedName = CurrentCulture.DateTimeFormat.AbbreviatedDayNames,
                    DayName = CurrentCulture.DateTimeFormat.DayNames,
                    MonthName = CurrentCulture.DateTimeFormat.MonthNames,
                    MonthAbbreviatedName = CurrentCulture.DateTimeFormat.AbbreviatedMonthNames,
                    DayPeriod = [CurrentCulture.DateTimeFormat.AMDesignator, CurrentCulture.DateTimeFormat.PMDesignator],
                    MaskPlaceholderDictionary = MaskPlaceholderDictionary
                };
                if (DateIcon is not null)
                {
                    DateIcon = SfBaseUtils.RemoveClass(DateIcon, ACTIVE);
                }
                _ = InputHtmlAttributes.Remove(ARIAACTIVEDESCENDANT);
                await InvokeVoidAsync(_datePickerJsModule, _datePickerJsInProcessModule, "closePopup", [DataId, PopupEventArgs, options]).ConfigureAwait(true);
                IsCalendarRender = false;
            }
        }

        /// <summary>
        /// Gets the current view of the calendar for the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <returns>
        /// A <c>string</c> describing the current view (such as Month, Year, or Decade) of the calendar UI.
        /// </returns>
        /// <remarks>
        /// This method reports the current view in the popup calendar, which is useful for UI automation or debugging.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// string currentView = datePickerRef.CurrentView();
        /// ]]></code>
        /// </example>
        public string CurrentView()
        {
            return CalendarBaseInstance is not null ? CalendarBaseInstance.CurrentView() : Start.ToString();
        }

        /// <summary>
        /// Navigates to the specified view (month, year, or decade) and date in the calendar for the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <param name="view">The target <see cref="CalendarView"/> (such as Month, Year, or Decade).</param>
        /// <param name="date">The focused date to display in the given view.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous navigation operation.
        /// </returns>
        /// <remarks>
        /// This method programmatically changes the calendar UI to display the specified view and highlights the chosen date. Use it for advanced navigation scenarios.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// await datePickerRef.NavigateAsync(CalendarView.Year, DateTime.Now);
        /// ]]></code>
        /// </example>
        public async Task NavigateAsync(CalendarView view, TValue date)
        {
            if (CalendarBaseInstance is not null)
            {
                await CalendarBaseInstance.NavigateToAsync(view, date).ConfigureAwait(false);
            }
            else
            {
                CurrentValueAsString = Intl.GetDateFormat(date, GetDefaultFormat());
                Start = view;
            }

        }

        /// <summary>
        /// Hides the calenar popup.
        /// </summary>
        /// <param name="args">The args<see cref="EventArgs"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task HidePopupElementAsync(EventArgs? args = null)
        {
            IsDateIconClicked = false;
            await HidePopupAsync(args).ConfigureAwait(false);
        }

        /// <summary>
        /// Method which is used to navigate to next section when full screen mobile calendar popup get scrolled.
        /// </summary>
        /// <param name="IsUpward">True when the scroll made from upward direction , otherwise false.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void ScrollToNextSection(bool IsUpward = false)
        {
            if (IsUpward)
            {
                CalendarBaseInstance?.NavigateNextHandler(null, true);
            }
            else
            {
                CalendarBaseInstance?.NavigatePreviousHandler(null, true);
            }
        }

        /// <summary>
        /// Invoke the before the popup close.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ClosePopupAsync()
        {
            if (IsDevice)
            {
                IsDateIconClicked = false;
            }
            await ClosePopupElementAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke the keyboard action handler.
        /// </summary>
        /// <param name="args">The args<see cref="KeyActions"/>.</param>
        /// <param name="inputvalue">Updated Value</param>
        /// <param name="IsInput"></param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task InputKeyActionHandleAsync(KeyActions args, string? inputvalue = null, bool IsInput = false)
        {
            if (inputvalue is not null)
            {
                CurrentMaskValue = inputvalue;
            }
            if (args is not null)
            {
                await InputKeyActionHandlerAsync(args, IsInput, inputvalue is not null ? inputvalue : string.Empty).ConfigureAwait(false);
                if (args.Action is not TAB and not SHIFT_TAB and not HOME and not END)
                {
                    IsChangeValue = false;
                }
            }
        }

        private async Task OnInputKeyDownAsync(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
        {
            string action = MapInputKeyToAction(e);
            KeyActions args = new()
            {
                Action = action ?? string.Empty,
                Key = e.Key ?? string.Empty,
                KeyCode = 0,
                Events = new Microsoft.AspNetCore.Components.Web.MouseEventArgs()
            };
            // If popup/calendar is open, include focused/selected cell context from server state
            if (ShowPopupCalendar && IsCalendarRender && CellListData is not null)
            {
                CellDetails? focused = CellListData.FirstOrDefault(c => !string.IsNullOrEmpty(c.ClassList) && c.ClassList.Contains("e-focused-date", StringComparison.Ordinal));
                CellDetails? selected = CellListData.FirstOrDefault(c => !string.IsNullOrEmpty(c.ClassList) && c.ClassList.Contains("e-selected", StringComparison.Ordinal));
                args.SelectDate = selected?.CellID ?? string.Empty;
                args.FocusedDate = focused?.CellID ?? string.Empty;
                args.ClassList = !string.IsNullOrEmpty(selected?.ClassList) ? selected.ClassList : (focused?.ClassList ?? string.Empty);
                args.ID = focused?.CellID ?? selected?.CellID ?? string.Empty;
                args.TargetClassList = CalendarClass ?? string.Empty;
            }
            // pass current input value and indicate this originated from input
            await InputKeyActionHandleAsync(args, CurrentValueAsString, true).ConfigureAwait(false);
        }

        internal static string MapInputKeyToAction(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
        {
            return e is null || string.IsNullOrEmpty(e.Key)
                ? string.Empty
                : e.AltKey && e.Key == "ArrowUp"
                ? ALT_UP_ARROW
                : e.AltKey && e.Key == "ArrowDown"
                ? ALT_DOWN_ARROW
                : e.CtrlKey && e.Key == "ArrowUp"
                ? "controlUp"
                : e.CtrlKey && e.Key == "ArrowDown" ? "controlDown" : e.Key
                switch
                {
                    "ArrowLeft" => "moveLeft",
                    "ArrowRight" => "moveRight",
                    "ArrowUp" => MOVEUP,
                    "ArrowDown" => MOVEDOWN,
                    "Enter" => ENTER,
                    "Escape" => ESCAPE,
                    "Home" => HOME,
                    "End" => END,
                    "Tab" => e.ShiftKey ? SHIFT_TAB : TAB,
                    _ => e.Key ?? string.Empty,
                };
        }
    }
    /// <summary>
    /// Handles the Mask supported formats and values.
    /// </summary>
    internal class ClientMaskValues
    {
        /// <summary>
        /// Gets or sets the current mask format value.This property stores maskformat with respect to format property.
        /// </summary>       
        /// <exclude/>
        public string CurrentMaskFormat { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the mask date value.This property stores maskformat with respect to value property.
        /// </summary>       
        /// <exclude/>
        public string InputElementValue { get; set; } = string.Empty;
    }
}

