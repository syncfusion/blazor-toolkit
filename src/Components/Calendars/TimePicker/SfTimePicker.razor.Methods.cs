using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Calendars.Internal;
using Syncfusion.Blazor.Toolkit.Inputs;
using Syncfusion.Blazor.Toolkit.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// The TimePicker is an intuitive component that provides options to select a time value from a popup list or to set a desired time value directly in the input.
    /// </summary>
    public partial class SfTimePicker<TValue> : SfInputBase<TValue>
    {
        #region Constants

        private const string MODEL = "model";
        private const string BODY = "body";
        private const string OPEN = "Open";
        private const string CLOSE = "Close";

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets focus to the TimePicker component, allowing for immediate user interaction.
        /// </summary>
        /// <remarks>
        /// This method programmatically brings the TimePicker into focus, making it the active element on the page.
        /// </remarks>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton OnClick="@FocusTimePicker">Focus TimePicker</SfButton>
        /// <SfTimePicker @ref="TimePickerRef" TValue="DateTime?"></SfTimePicker>
        /// @code {
        ///     private SfTimePicker<DateTime?> TimePickerRef;
        ///     private async Task FocusTimePicker()
        ///     {
        ///         await TimePickerRef.FocusAsync();
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task FocusAsync()
        {
            await InvokeVoidAsync(_timePickerJsModule, _timePickerJsInProcessModule, "focusIn", [DataId, false]).ConfigureAwait(true);
        }

        /// <summary>
        /// Removes focus from the TimePicker component if it is currently focused.
        /// </summary>
        /// <remarks>
        /// This method programmatically removes focus from the TimePicker, de-targeting it as the active element.
        /// </remarks>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton OnClick="@FocusOutTimePicker">Focus Out TimePicker</SfButton>
        /// <SfTimePicker @ref="TimePickerRef" TValue="DateTime?"></SfTimePicker>
        /// @code {
        ///     private SfTimePicker<DateTime?> TimePickerRef;
        ///     private async Task FocusOutTimePicker()
        ///     {
        ///         await TimePickerRef.FocusOutAsync();
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task FocusOutAsync()
        {
            await InvokeVoidAsync(_timePickerJsModule, _timePickerJsInProcessModule, "focusOut", [DataId]).ConfigureAwait(true);
        }

        /// <summary>
        /// Opens the TimePicker's popup, which displays the list of time values.
        /// </summary>
        /// <param name="args">Specifies the optional event arguments.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <remarks>
        /// This method programmatically opens the time selection popup, which is useful for triggering the popup from external events or custom logic.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton OnClick="@ShowPopup">Show Popup</SfButton>
        /// <SfTimePicker @ref="TimePickerRef" TValue="DateTime?"></SfTimePicker>
        /// @code {
        ///     private SfTimePicker<DateTime?> TimePickerRef;
        ///     private async Task ShowPopup()
        ///     {
        ///         await TimePickerRef.ShowPopupAsync();
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task ShowPopupAsync(EventArgs? args = null)
        {
            if (!((!Disabled && Readonly) || Disabled))
            {
                await GenerateListAsync().ConfigureAwait(false);
                bool isCancelled = false;
                if (!string.IsNullOrEmpty(CurrentValueAsString))
                {
                    UpdateListSelection(CurrentValueAsString, SELECTED);
                }
                EventArgs eventArgs = args is null ? new() : args;
                PopupEventArgs = new DatePickerPopupArgs { AppendTo = IsDevice ? MODEL : BODY, Cancel = false, Event = eventArgs, PreventDefault = false };
                if (OnOpen.HasDelegate)
                {
                    PopupEventArgs openEventArgs = new()
                    {
                        Cancel = false,
                        Event = eventArgs,
                        Name = OPEN
                    };
                    await OnOpen.InvokeAsync(openEventArgs).ConfigureAwait(false);
                    isCancelled = openEventArgs.Cancel;
                }
                if (!isCancelled)
                {
                    IsListRendered = true;
                    await Task.Delay(POPUP_SHOW_DELAY_MS).ConfigureAwait(false);
                    ShowPopupList = true;
                    if (!string.IsNullOrEmpty(AriaActiveDescendantID))
                    {
                        _ = SfBaseUtils.UpdateDictionary(ARIA_ACTIVE_DESCENDANT, AriaActiveDescendantID, InputHtmlAttributes);
                    }
                    _ = SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, TRUE, InputHtmlAttributes);
                    _ = SfBaseUtils.UpdateDictionary(ARIA_OWN, ID + POPUPS, InputHtmlAttributes);
                    IsTimeIconClicked = false;
                    await InvokeAsync(StateHasChanged).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Hides the TimePicker's popup if it is currently open.
        /// </summary>
        /// <param name="args">Specifies the optional event arguments.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <remarks>
        /// This method programmatically closes the time selection popup.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton OnClick="@HidePopup">Hide Popup</SfButton>
        /// <SfTimePicker @ref="TimePickerRef" TValue="DateTime?"></SfTimePicker>
        /// @code {
        ///     private SfTimePicker<DateTime?> TimePickerRef;
        ///     private async Task HidePopup()
        ///     {
        ///         // You can open the popup using ShowPopupAsync() before hiding it.
        ///         await TimePickerRef.HidePopupAsync();
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task HidePopupAsync(EventArgs? args = null)
        {
            bool isCancelled = false;
            EventArgs eventArgs = args is null ? new() : args;
            PopupEventArgs = new DatePickerPopupArgs { AppendTo = IsDevice ? MODEL : BODY, Cancel = false, Event = eventArgs, PreventDefault = false };
            if (OnClose.HasDelegate)
            {
                PopupEventArgs closeEventArgs = new()
                {
                    Cancel = false,
                    Event = eventArgs,
                    Name = CLOSE
                };
                await OnClose.InvokeAsync(closeEventArgs).ConfigureAwait(false);
                isCancelled = closeEventArgs.Cancel;
            }
            if (!isCancelled)
            {
                TimePickerClientProps<TValue> options = new()
                {
                    EnableRtl = SyncfusionService!._options.EnableRtl,
                    ZIndex = ZIndex,
                    KeyConfigs = KeyConfigs,
                    Value = Value!,
                    Width = Width,
                    Step = Step,
                    ScrollTo = ScrollTo
                };
                if (IsDevice && BaseInputAttributes is not null)
                {
                    BaseInputAttributes = RemoveAttributes(READ_ONLY, BaseInputAttributes);
                }
                _ = InputHtmlAttributes.Remove(ARIA_ACTIVE_DESCENDANT);
                TimeIcon = SfBaseUtils.RemoveClass(TimeIcon, ACTIVE);
                await InvokeVoidAsync(_timePickerJsModule!, _timePickerJsInProcessModule!, "closePopup", [DataId, PopupEventArgs, options]).ConfigureAwait(true);
                IsListRender = false;
            }
        }

        #endregion

        #region JSInterop Methods

        /// <summary>
        /// A JS interop method that shows the TimePicker popup.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> arguments.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ShowPopup(EventArgs? args = null)
        {
            await ShowPopupAsync(args).ConfigureAwait(false);
        }

        /// <summary>
        /// A JS interop method that hides the TimePicker popup.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> representing the event arguments.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task HidePopup(EventArgs? args = null)
        {
            await HidePopupAsync(args).ConfigureAwait(false);
        }

        /// <summary>
        /// A JS interop method that is invoked before the popup closes.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ClosePopupAsync()
        {
            await ClosePopupActionAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// A JS interop method that invokes the keyboard action handler.
        /// </summary>
        /// <param name="args">The <see cref="KeyActions"/> arguments.</param>
        /// <param name="inputvalue">The updated input value.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task KeyboardHandlerAsync(KeyActions args, string? inputvalue = null)
        {
            if (inputvalue is not null)
            {
                CurrentMaskValue = inputvalue;
            }
            if (args is not null)
            {
                await KeyboardActionsAsync(args).ConfigureAwait(false);
                IsChangeValue = false;
            }
        }

        /// <summary>
        /// A JS interop method that updates the disabled status of the component based on the fieldset.
        /// </summary>
        /// <param name="isDisabled">
        /// A boolean value that indicates whether the component should be disabled.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
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

        #endregion
    }
}
