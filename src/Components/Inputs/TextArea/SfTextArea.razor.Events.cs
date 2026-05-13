using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// The TextArea is an textarea element that allows to get input from the user. It allows the user to edit or display the text value.
    /// </summary>
    public partial class SfTextArea : SfInputBase<string>
    {
        /// <summary>
        /// Gets or sets the event callback that will be invoked when the content of <see cref="SfTextArea"/> has changed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the value of the <see cref="SfTextArea"/> changes. The callback receives a <see cref="TextAreaValueChangeEventArgs"/> containing the changed value information.
        /// </value>
        /// <remarks>
        /// The <see cref="ValueChange"/> event is triggered when the user modifies the text content and the <see cref="SfTextArea"/> loses focus, or when the value is programmatically changed.
        /// This event provides the old and new values, allowing you to track changes and perform validation or other operations based on the modified content.
        /// Unlike the <see cref="OnInput"/> event, <see cref="ValueChange"/> is typically fired less frequently and is suitable for handling final value changes rather than real-time input tracking.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextArea Placeholder="Enter a value" ValueChange="@OnChange">
        /// </SfTextArea>
        /// @code{
        ///     private void OnChange(TextAreaValueChangeEventArgs args)
        ///     {
        ///         var TextValue = args.Value;
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<TextAreaValueChangeEventArgs> ValueChange { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the user types or pastes text into the <see cref="SfTextArea"/>.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked on each input action within the <see cref="SfTextArea"/>. The callback receives a <see cref="TextAreaInputEventArgs"/> containing the current input value and related event details.
        /// </value>
        /// <remarks>
        /// The <see cref="OnInput"/> event is triggered whenever the user interacts with the <see cref="SfTextArea"/> by typing, pasting, or
        /// using any input method to modify the content. It provides real-time updates as the user enters text,
        /// allowing you to perform actions or validation based on the changing input.
        ///
        /// It is important to note that the <see cref="OnInput"/> event may fire frequently during user input, potentially with
        /// each keystroke, so it is generally suitable for handling real-time updates or feedback rather than more
        /// intensive processing.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextArea Placeholder="Enter a value" OnInput="@OnInput">
        /// </SfTextArea>
        /// @code{
        ///     private void OnInput(TextAreaInputEventArgs args)
        ///     {
        ///         var TextValue = args.Value;
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<TextAreaInputEventArgs> OnInput { get; set; }

        /// <summary> 
        /// Gets or sets the event callback that will be invoked when the <see cref="SfTextArea"/> component is created. 
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the <see cref="SfTextArea"/> component is successfully created and initialized. The callback receives an <c>object</c> parameter.
        /// </value>
        /// <remarks>
        /// The <see cref="Created"/> event is triggered once during the component lifecycle, immediately after the <see cref="SfTextArea"/> component has been rendered and initialized in the DOM.
        /// This event is useful for performing initialization tasks, setting up event handlers, or executing code that depends on the component being fully created.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextArea @ref="textAreaRef" Placeholder="Enter your text" 
        ///             Created="@OnTextAreaCreated"></SfTextArea>
        ///
        /// @code {
        ///     SfTextArea? textAreaRef;
        ///     private string creationStatus = string.Empty;
        ///     private async Task OnTextAreaCreated(object args)
        ///     {
        ///         // Component is now fully initialized
        ///         creationStatus = "TextArea component created successfully";
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary> 
        /// Gets or sets the event callback that will be invoked when the <see cref="SfTextArea"/> component is destroyed. 
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the <see cref="SfTextArea"/> component is being destroyed or removed from the DOM. The callback receives an <c>object</c> parameter.
        /// </value>
        /// <remarks>
        /// The <see cref="Destroyed"/> event is triggered during the component cleanup phase, before the <see cref="SfTextArea"/> component is removed from the DOM.
        /// This event is useful for performing cleanup operations, disposing resources, removing event handlers, or executing code that should run when the component is being destroyed.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextArea @ref="textAreaRef" Placeholder="Enter your text" 
        ///             Destroyed="@OnTextAreaDestroyed"></SfTextArea>
        /// <p>@destructionMessage</p>
        /// @code {
        ///     SfTextArea? textAreaRef;
        ///     private string destructionMessage = string.Empty;
        ///     private async Task OnTextAreaDestroyed(object args)
        ///     {
        ///         // Perform cleanup operations
        ///         destructionMessage = "TextArea component is being destroyed";
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the <see cref="SfTextArea"/> gets focus.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the <see cref="SfTextArea"/> receives focus. The callback receives a <see cref="TextAreaFocusInEventArgs"/> containing focus-related information.
        /// </value>
        /// <remarks>
        /// The <see cref="OnFocus"/> event is triggered when the user clicks on the <see cref="SfTextArea"/>, tabs to it, or programmatically sets focus to the component.
        /// This event is useful for performing actions when the textarea becomes active, such as highlighting the input area, showing validation messages, or updating the UI state.
        /// The event provides information about the focus operation through the <see cref="TextAreaFocusInEventArgs"/> parameter.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextArea @ref="textAreaRef" Placeholder="Enter your text"
        ///             OnFocus="@OnTextAreaFocus"></SfTextArea>
        ///
        /// @code {
        ///     SfTextArea? textAreaRef;
        ///     private string focusMessage = string.Empty;
        ///
        ///     private void OnTextAreaFocus(TextAreaFocusInEventArgs args)
        ///     {
        ///         focusMessage = $"TextArea focused with value: {args.Value ?? "empty"}";
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<TextAreaFocusInEventArgs> OnFocus { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked when the <see cref="SfTextArea"/> loses focus.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the <see cref="SfTextArea"/> loses focus. The callback receives a <see cref="TextAreaFocusOutEventArgs"/> containing blur-related information.
        /// </value>
        /// <remarks>
        /// The <see cref="OnBlur"/> event is triggered when the user clicks outside the <see cref="SfTextArea"/>, tabs away from it, or programmatically removes focus from the component.
        /// This event is commonly used for validation purposes, saving draft content, hiding input-specific UI elements, or updating the application state when the user finishes interacting with the textarea.
        /// The event provides information about the blur operation through the <see cref="TextAreaFocusOutEventArgs"/> parameter.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextArea @ref="textAreaRef" Placeholder="Enter your message"
        ///             OnBlur="@OnTextAreaBlur" MaxLength="200"></SfTextArea>
        /// <span>@validationMessage</span>
        /// @code {
        ///     SfTextArea? textAreaRef;
        ///     private string validationMessage = string.Empty;
        ///     private void OnTextAreaBlur(TextAreaFocusOutEventArgs args)
        ///     {
        ///         // Perform validation on blur
        ///         if (string.IsNullOrEmpty(args.Value))
        ///         {
        ///             validationMessage = "This field is required";
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<TextAreaFocusOutEventArgs> OnBlur { get; set; }

        #region EventHandler Methods

        /// <summary>
        /// Handles the focus event when the TextArea receives input focus.
        /// </summary>
        /// <param name="args">The focus event arguments containing event details.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous focus handling operation.</returns>
        /// <remarks>
        /// This method triggers both the OnFocus event (with TextArea-specific event args) and the OnFocus event
        /// when the component receives focus, allowing for custom focus handling logic.
        /// </remarks>
        protected override async Task FocusHandlerAsync(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (OnFocus.HasDelegate)
            {
                TextAreaFocusInEventArgs eventArgs = new()
                {
                    Event = args,
                    Value = Value
                };
                await OnFocus.InvokeAsync(eventArgs).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles the blur event when the TextArea loses input focus.
        /// </summary>
        /// <param name="args">The focus event arguments containing event details.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous blur handling operation.</returns>
        /// <remarks>
        /// This method checks for value changes and triggers change events if the value has been modified.
        /// It also invokes the OnBlur event with TextArea-specific event arguments for custom blur handling.
        /// </remarks>
        protected override async Task FocusOutHandlerAsync(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (!(string.IsNullOrEmpty(_previousValue) && string.IsNullOrEmpty(Value) && string.IsNullOrEmpty(InputTextValue)) && _previousValue != InputTextValue)
            {
                await RaiseChangeEventAsync(true).ConfigureAwait(true);
            }

            if (OnBlur.HasDelegate)
            {
                TextAreaFocusOutEventArgs eventArgs = new()
                {
                    Event = args,
                    Value = Value
                };
                await OnBlur.InvokeAsync(eventArgs).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles input events as the user types in the TextArea.
        /// </summary>
        /// <param name="args">The change event arguments containing the current input value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous input handling operation.</returns>
        /// <remarks>
        /// This method captures real-time input changes and triggers the OnInput event with both current and
        /// previous values, enabling scenarios like character counting, real-time validation, or live formatting.
        /// </remarks>
        protected override async Task InputHandlerAsync(ChangeEventArgs? args)
        {
            if (OnInput.HasDelegate)
            {
                TextAreaInputEventArgs eventArgs = new()
                {
                    Value = args?.Value is not null ? args.Value.ToString() : null,
                    PreviousValue = _inputPreviousValue
                };
                await OnInput.InvokeAsync(eventArgs).ConfigureAwait(true);
            }
            _inputPreviousValue = args?.Value is not null ? args.Value.ToString() : null;
        }

        /// <summary>
        /// Handles change events when the TextArea value is committed.
        /// </summary>
        /// <param name="args">The change event arguments containing the committed value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous change handling operation.</returns>
        /// <remarks>
        /// This method updates the component's current value and triggers change events when the user 
        /// commits their input, typically on blur or when pressing Enter in certain scenarios.
        /// </remarks>
        protected override async Task ChangeHandlerAsync(ChangeEventArgs? args)
        {
            string? changeVal = args?.Value is not null ? args.Value.ToString() : null;
            CurrentValueAsString = changeVal;
            await RaiseChangeEventAsync(true).ConfigureAwait(true);
        }

        #endregion
    }
}
