using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// The NumericTextBox is used to get the number inputs from the user. The input values can be incremented or decremented by a predefined step value.
    /// </summary>
    public partial class SfNumericTextBox<TValue> : SfInputBase<TValue>
    {
        /// <summary>
        /// Decrements the value asynchronously by the specified step.
        /// </summary>
        /// <param name="step">The step value by which to decrement the current value. If not provided, the numeric value will be decremented based on the <see cref="Step"/> property value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method decreases the current value of the <see cref="SfNumericTextBox{TValue}"/> by the specified step amount.
        /// The operation is performed asynchronously and will trigger the change event after the value is updated.
        /// If the resulting value goes below the minimum allowed value, it will be constrained to the minimum value.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Decrement by a custom step value
        /// await numericTextBox.DecrementAsync(5);
        /// 
        /// // Decrement by the default step value
        /// await numericTextBox.DecrementAsync(default(TValue));
        /// ]]></code>
        /// </example>
        public async Task DecrementAsync(TValue step)
        {
            await ChangeValueAsync(PerformAction(Value, step, "decrement")).ConfigureAwait(true);
            await RaiseChangeEventAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Sets focus to the <see cref="SfNumericTextBox{TValue}"/> component for user interaction.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method programmatically focuses the <see cref="SfNumericTextBox{TValue}"/> component, making it ready for user input.
        /// When focused, the component will be highlighted and ready to receive keyboard input or other user interactions.
        /// This is useful for accessibility scenarios or when you need to direct user attention to the component.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Focus the numeric textbox
        /// await numericTextBox.FocusAsync();
        /// ]]></code>
        /// </example>
        public async Task FocusAsync()
        {
            try
            {
                await InputElement.FocusAsync().ConfigureAwait(true);
            }
            catch (InvalidOperationException ex)
            {
                System.Diagnostics.Debug.WriteLine($"FocusAsync invalid state: {ex.Message}");
            }
            catch (JSException ex)
            {
                System.Diagnostics.Debug.WriteLine($"FocusAsync JS failure: {ex.Message}");
            }
        }

        /// <summary>
        /// Removes the focus from the NumericTextBox component, if the component is in focus state.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method programmatically removes focus from the <see cref="SfNumericTextBox{TValue}"/> component.
        /// If the component is currently focused, it will lose focus and any pending input will be processed.
        /// This method is useful for form validation scenarios or when you need to blur the component programmatically.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Remove focus from the numeric textbox
        /// await numericTextBox.FocusOutAsync();
        /// ]]></code>
        /// </example>
        public async Task FocusOutAsync()
        {
            await InvokeVoidAsync(_numericTextBoxJsModule, _numericTextBoxJsInProcessModule, "focusOut", [DataId]).ConfigureAwait(true);

        }

        /// <summary>
        /// Gets the properties to be maintained in the persisted state.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains a <see cref="string"/> with the persisted data.</returns>
        /// <remarks>
        /// This method retrieves the persisted state data for the <see cref="SfNumericTextBox{TValue}"/> component from the browser's local storage.
        /// The persisted data includes component properties that should be maintained across page refreshes or application restarts.
        /// This is useful for maintaining user preferences and component state in web applications.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Get persisted data
        /// string persistedData = await numericTextBox.GetPersistDataAsync();
        /// ]]></code>
        /// </example>
        public async Task<string?> GetPersistDataAsync()
        {
            return await InvokeAsync<string>(_baseJsModule!, _baseJsInProcessModule!, "getLocalStorageItem", [ID]).ConfigureAwait(true);
        }

        /// <summary>
        /// Gets the formatted text representation of the current value.
        /// </summary>
        /// <returns>A <see cref="string"/> containing the formatted text representation of the current value.</returns>
        /// <remarks>
        /// This method returns the current value of the <see cref="SfNumericTextBox{TValue}"/> formatted according to the component's formatting settings.
        /// The formatting includes decimal places, currency symbols, percentage signs, and other culture-specific formatting as configured.
        /// This is useful when you need to display the formatted value in other parts of your application.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Get the formatted text of the current value
        /// string formattedText = numericTextBox.GetFormattedText();
        /// ]]></code>
        /// </example>
        public string? GetFormattedText()
        {
            return FormatValueAsString(InputTextValue);
        }

        /// <summary>
        /// Sets the Disabled value based on the disabled state of the closest <fieldset></fieldset>.
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
            return Task.CompletedTask;
        }

        /// <summary>
        /// Increments the NumericTextBox value with the specified step value.
        /// </summary>
        /// <param name="step">Specifies the value used to increment the NumericTextBox value. If not provided, the numeric value will be incremented based on the <see cref="Step"/> property value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method increases the current value of the <see cref="SfNumericTextBox{TValue}"/> by the specified step amount.
        /// The operation is performed asynchronously and will trigger the change event after the value is updated.
        /// If the resulting value exceeds the maximum allowed value, it will be constrained to the maximum value.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// // Increment by a custom step value
        /// await numericTextBox.IncrementAsync(10);
        /// 
        /// // Increment by the default step value
        /// await numericTextBox.IncrementAsync(default(TValue));
        /// ]]></code>
        /// </example>
        public async Task IncrementAsync(TValue step)
        {
            await ChangeValueAsync(PerformAction(Value, step, "increment")).ConfigureAwait(true);
            await RaiseChangeEventAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Invokes the event when pasting the value to the input element.
        /// </summary>
        /// <param name="beforeValue">Specifies the previous element value.</param>
        /// <param name="eventType">Specifies the type of the event.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task InvokePasteHandlerAsync(string beforeValue, string eventType)
        {
            if (JSRuntime is not IJSInProcessRuntime)
            {
                IsDropValue = eventType != "paste";
                IsPasteValue = eventType == "paste";
                await UpdatePasteInputAsync(beforeValue).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Invokes the event when focusing the input element.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task<Dictionary<string, object>> UpdateFocusInputAsync()
        {
            return await FocusInputAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Invokes the increment/decrement actions.
        /// </summary>
        /// <param name="action">Specifies the action.</param>
        /// <param name="args"><see cref="EventArgs"/> arguments.</param>
        /// <param name="currentInputValue">Specifies the input value</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ServerActionAsync(string action, EventArgs? args, string currentInputValue)
        {
            await ActionAsync(action, args, currentInputValue).ConfigureAwait(true);
        }
    }
}
