using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Toolkit.Internal;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using Microsoft.JSInterop;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents the Toolkit TextBox component for Blazor applications, which provides an enhanced input element
    /// that allows users to enter and edit text values with advanced features like floating labels, input validation,
    /// clear button functionality, and various styling options.
    /// </summary>
    /// <remarks>
    /// The SfTextBox component extends the standard HTML input element with additional functionality including:
    /// - Support for floating labels (Auto, Always, Never)
    /// - Built-in clear button with customizable behavior
    /// - Input validation with visual feedback
    /// - Accessibility features with ARIA support
    /// - Integration with Blazor's EditForm validation
    /// - Persistence support for maintaining state across browser sessions
    /// - Customizable styling and theming options
    /// </remarks>
    public partial class SfTextBox : SfInputBase<string>
    {
        #region Constant variables

        private const string INVALID = "invalid";
        private const string MODIFIED_INVALID = "modified invalid";
        private const string MODIFIED_VALID = "modified valid";
        private const string ERROR_CLASS = "e-error";
        private const string SUCCESS_CLASS = "e-success";
        private const string ROOT_CLASS = "e-control e-textbox e-lib";
        private const string TYPE = "type";
        private const string AUTOCOMPLETE = "autocomplete";
        private const string OUTLINE = "e-outline";
        private const string ARIA_LABEL = "aria-label";

        #endregion

        #region Protected variables

        /// <summary>
        /// Gets or sets the root CSS class for the <see cref="SfTextBox"/> component, which defines the base styling for the TextBox element.
        /// </summary>
        protected override string RootClass { get; set; } = "e-control e-textbox e-lib";

        /// <summary>
        /// Gets or sets the container CSS class for the <see cref="SfTextBox"/> component, which is used to apply additional styling to the TextBox container element.
        /// </summary>
        protected override string ContainerClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the base autocomplete value for the <see cref="SfTextBox"/> component, which is used to specify the autocomplete behavior of the input element. The default value is determined by the Autocomplete property.
        /// </summary>
        protected override string BaseAutocomplete { get; set; } = default!;

        #endregion

        #region Private variables

        private string? _inputPreviousValue;

        private string? _previousValue;

        private string? _validClass;

        private bool _isClearIconClick;

        [GeneratedRegex(@"\s+")]
        private static partial Regex WhitespaceRegex();

        #endregion

        #region Private Methods

        /// <summary>
        /// Asynchronously processes individual property changes by applying specific update logic based on the property type and updating the <see cref="SfTextBox"/> component's state accordingly.
        /// </summary>
        /// <param name="newProps">
        /// A <see cref="Dictionary{TKey, TValue}"/> of <see cref="string"/> keys and <see cref="object"/> values containing the changed properties as key-value pairs, where the key represents the property name and the value represents the new property value.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous property change processing operation that completes when all property updates have been applied.
        /// </returns>
        /// <remarks>
        /// <para>This method handles the following property changes:</para>
        /// <list type="bullet">
        /// <item><description><see cref="SfInputBase{TValue}.Value"/> - Updates the component value, triggers change events, and manages previous value tracking</description></item>
        /// <item><description><see cref="SfInputBase{TValue}.CssClass"/> - Applies or removes CSS classes from the container element</description></item>
        /// <item><description><see cref="FloatLabelType"/> - Re-initializes client-side scripts for float label behavior</description></item>
        /// <item><description><see cref="Autocomplete"/> - Updates the autocomplete attribute in the input element</description></item>
        /// <item><description><see cref="Type"/> - Updates the input type attribute for proper input behavior</description></item>
        /// </list>
        /// <para>Each property change is processed individually to ensure proper component state management.</para>
        /// </remarks>
        private async Task OnPropertyChangeAsync(Dictionary<string, object> newProps)
        {
            List<KeyValuePair<string, object>> newProperties = [.. newProps];
            foreach (KeyValuePair<string, object> prop in newProperties)
            {
                switch (prop.Key)
                {
                    case nameof(Value):
                        if (PropertyChanges is not null && PropertyChanges.ContainsKey(nameof(Value)) && _previousValue != Value)
                        {
                            await SetValueAsync(Value, FloatLabelType, ShowClearButton).ConfigureAwait(true);
                            await RaiseChangeEventAsync(null, false).ConfigureAwait(true);
                        }
                        break;
                    case nameof(CssClass):

                        if (!string.IsNullOrEmpty(ContainerClass) && _cssClass is not null)
                        {
                            ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, _cssClass.Trim());
                        }
                        _cssClass = CssClass;
                        SetCssClass();
                        break;
                    case nameof(FloatLabelType):
                        await OnAfterScriptRenderedAsync().ConfigureAwait(true);
                        break;
                    case nameof(Autocomplete):
                        string? autoCompleteEnumValue = SfBaseUtils.GetEnumValue(Autocomplete);
                        if (autoCompleteEnumValue is not null)
                        {
                            InputHtmlAttributes = SfBaseUtils.UpdateDictionary(AUTOCOMPLETE, autoCompleteEnumValue, InputHtmlAttributes);
                        }
                        break;
                    case nameof(Type):
                        string? typeEnumValue = SfBaseUtils.GetEnumValue(Type);
                        if (typeEnumValue is not null)
                        {
                            InputHtmlAttributes = SfBaseUtils.UpdateDictionary(TYPE, typeEnumValue, InputHtmlAttributes);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Applies the specified CSS class to the <see cref="SfTextBox"/> component's container element if it is not already present.
        /// </summary>
        /// <remarks>
        /// <para>This method checks if the <see cref="SfInputBase{TValue}.CssClass"/> property contains a non-empty value and verifies whether the class is already applied to the ContainerClass.</para>
        /// <para>If the class is not present, it adds the class to maintain proper component styling and avoid CSS class duplication.</para>
        /// </remarks>
        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = ContainerClass.Contains(CssClass, StringComparison.Ordinal) ? ContainerClass : SfBaseUtils.AddClass(ContainerClass, CssClass);
            }
        }

        /// <summary>
        /// Asynchronously notifies the <see cref="SfTextBox"/> component of property changes by comparing current values with previous values and updating internal tracking variables for change detection.
        /// </summary>
        /// <returns>
        /// A completed <see cref="Task"/> representing the property update operation.
        /// </returns>
        /// <remarks>
        /// <para>This method performs change detection for the following properties:</para>
        /// <list type="bullet">
        /// <item><description><see cref="SfInputBase{TValue}.CssClass"/> - Tracks changes in component styling classes</description></item>
        /// <item><description><see cref="Autocomplete"/> - Monitors autocomplete behavior modifications</description></item>
        /// <item><description><see cref="Type"/> - Detects input type changes (text, password, etc.)</description></item>
        /// <item><description><see cref="SfInputBase{TValue}.Value"/> - Tracks text value modifications for change events</description></item>
        /// </list>
        /// <para>The method uses the NotifyPropertyChanges mechanism to maintain property state and ensure proper component re-rendering when properties change.</para>
        /// </remarks>
        private async Task PropertyUpdateAsync()
        {
            _ = NotifyPropertyChanges(nameof(CssClass), CssClass, _cssClass);
            _autocomplete = NotifyPropertyChanges(nameof(Autocomplete), Autocomplete, _autocomplete);
            _type = NotifyPropertyChanges(nameof(Type), Type, _type);
            InternalValue = NotifyPropertyChanges(nameof(Value), Value, InternalValue);
            await Task.CompletedTask.ConfigureAwait(true);
        }

        /// <summary>
        /// Configures and binds the input event handler to the input element when specific features require real-time input monitoring.
        /// </summary>
        /// <remarks>
        /// <para>This method conditionally attaches an input event handler based on the following conditions:</para>
        /// <list type="bullet">
        /// <item><description><see cref="FloatLabelType"/> is set to <see cref="FloatLabelType.Auto"/> or <see cref="FloatLabelType.Always"/> (requires real-time input monitoring)</description></item>
        /// <item><description><see cref="ShowClearButton"/> is enabled (requires input value tracking)</description></item>
        /// <item><description>OnInput or <see cref="OnInput"/> event callbacks are registered (custom input handling)</description></item>
        /// <item><description>ValidateOnInput is enabled (requires real-time validation)</description></item>
        /// </list>
        /// <para>The input event handler enables real-time updates and provides immediate feedback for interactive features without waiting for the change event or focus loss.</para>
        /// </remarks>
        private void InvokeInputEvent()
        {
            if (FloatLabelType == FloatLabelType.Auto || FloatLabelType == FloatLabelType.Always || ShowClearButton || OnInput.HasDelegate || ValidateOnInput)
            {
                if (!(OnInput.HasDelegate || ValidateOnInput))
                {
                    IsBindInputEvent = true;
                }
                else
                {
                    IsBindInputEvent = false;
                    EventCallback<ChangeEventArgs> createInputEvent = EventCallback.Factory.Create<ChangeEventArgs>(this, OnInputHandlerAsync);
                    InputHtmlAttributes = SfBaseUtils.UpdateDictionary("oninput", createInputEvent, InputHtmlAttributes);
                }
            }
            if (OnPaste.HasDelegate)
            {
                EventCallback<ClipboardEventArgs> createPasteEvent = EventCallback.Factory.Create<ClipboardEventArgs>(this, OnPasteHandlerAsync);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary("onpaste", createPasteEvent, InputHtmlAttributes);
            }
        }

        /// <summary>
        /// Initializes essential <see cref="SfTextBox"/> component properties and sets up default values during component creation.
        /// </summary>
        /// <remarks>
        /// <para>This method performs the following initialization tasks:</para>
        /// <list type="bullet">
        /// <item><description>Generates a unique component ID if not explicitly provided</description></item>
        /// <item><description>Sets the DataId property for internal component tracking</description></item>
        /// <item><description>Initializes RootClass and ContainerClass with default values</description></item>
        /// <item><description>Establishes initial value tracking for InputPreviousValue and PreviousValue</description></item>
        /// </list>
        /// <para>The unique ID generation ensures proper component identification in scenarios with multiple TextBox instances and prevents DOM element conflicts.</para>
        /// </remarks>
        private void InitializeProps()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = "textbox-" + Guid.NewGuid().ToString();
            }
            DataId = ID;
            RootClass = ROOT_CLASS;
            ContainerClass = string.Empty;
            _inputPreviousValue = Value;
            _previousValue = Value;
        }

        /// <summary>
        /// Asynchronously handles touch events for the clear button on touch-enabled devices.
        /// </summary>
        /// <param name="args">
        /// An <see cref="EventArgs"/> containing event information from the touch interaction.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous touch event handling operation that completes when the clear operation finishes.
        /// </returns>
        /// <remarks>
        /// <para>This method is specifically designed for touch devices and provides the same clearing functionality as mouse click events.</para>
        /// <para>It delegates to the common clear button event handler to ensure consistent behavior across different input methods.</para>
        /// </remarks>
        private async Task BindClearBtnTouchEventsAsync(EventArgs args)
        {
            await InvokeClearBtnEventAsync(args).ConfigureAwait(true);
        }

        /// <summary>
        /// Asynchronously handles mouse click events for the clear button, clearing the input value and restoring focus to the input element.
        /// </summary>
        /// <param name="args">
        /// An <see cref="EventArgs"/> containing event information from the mouse click.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous clear button event handling operation that completes when the clear and focus operations finish.
        /// </returns>
        /// <remarks>
        /// <para>This method performs the standard clear button click operation:</para>
        /// <list type="bullet">
        /// <item><description>Invokes the common clear button event handler to clear the input value</description></item>
        /// <item><description>Restores focus to the input element for seamless user experience</description></item>
        /// <item><description>Maintains proper event flow and user interaction patterns</description></item>
        /// </list>
        /// </remarks>
        private async Task BindClearBtnEventsAsync(EventArgs args)
        {
            await InvokeClearBtnEventAsync(args).ConfigureAwait(true);
            await FocusAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Asynchronously executes the core clear button functionality, including value clearing, event invocation, change detection, and state management.
        /// </summary>
        /// <param name="args">
        /// An <see cref="EventArgs"/> containing event information from the clear button interaction.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous clear button operation that completes when all clearing operations and event notifications finish.
        /// </returns>
        /// <remarks>
        /// <para>This method performs the following clear button operations:</para>
        /// <list type="bullet">
        /// <item><description>Clears the current input value by setting CurrentValueAsString to <see langword="null"/></description></item>
        /// <item><description>Applies a time delay to ensure proper value clearing timing</description></item>
        /// <para><strong>Delay Rationale:</strong> The delay is necessary to ensure proper sequencing in Blazor's rendering pipeline:</para>
        /// <list type="number">
        /// <item><description>Setting CurrentValueAsString to <see langword="null"/> triggers a component re-render, updating the DOM with an empty input field</description></item>
        /// <item><description>The delay allows the browser to complete the DOM update and visual refresh cycle</description></item>
        /// <item><description>SetValueAsync() is then called with a clean slate, synchronizing internal state with the cleared DOM</description></item>
        /// <item><description>Without this delay, state inconsistencies can occur where SetValueAsync() executes before the DOM update completes</description></item>
        /// <item><description>This ensures EditContext validation state, floating labels, and CSS classes all synchronize correctly</description></item>
        /// </list>
        /// <item><description>Updates the component value using SetValue with current configuration</description></item>
        /// <item><description>Invokes <see cref="OnInput"/> event callbacks with cleared value information</description></item>
        /// <item><description>Raises change events to notify external subscribers</description></item>
        /// <item><description>Updates previous value tracking for change detection</description></item>
        /// <item><description>Restores focus to the input element for continued interaction</description></item>
        /// </list>
        /// </remarks>
        private async Task InvokeClearBtnEventAsync(EventArgs args)
        {
            _isClearIconClick = true;
            CurrentValueAsString = null;
            await Task.Delay(100).ConfigureAwait(true);
            await SetValueAsync(null, FloatLabelType, ShowClearButton).ConfigureAwait(true);
            if (OnInput.HasDelegate)
            {
                InputEventArgs eventArgs = new()
                {
                    Value = InputTextValue,
                    Event = args,
                    PreviousValue = _inputPreviousValue,
                };
                await OnInput.InvokeAsync(eventArgs).ConfigureAwait(true);
            }
            await RaiseChangeEventAsync(args, true).ConfigureAwait(true);
            _previousValue = InputTextValue;
            await FocusAsync().ConfigureAwait(true);
            _isClearIconClick = false;
        }

        /// <summary>
        /// Asynchronously raises the value change event with proper state management, persistence handling, and validation updates.
        /// </summary>
        /// <param name="args">
        /// An optional <see cref="EventArgs"/> containing event information. Can be <see langword="null"/> for programmatic changes.
        /// </param>
        /// <param name="isInteraction">
        /// A <see cref="bool"/> value indicating whether the change originated from user interaction. When <see langword="true"/>, the change resulted from user interaction; when <see langword="false"/>, the change was programmatic. The default value is <see langword="false"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous change event operation that completes when all event notifications and persistence operations finish.
        /// </returns>
        /// <remarks>
        /// <para>This method performs the following change event operations:</para>
        /// <list type="bullet">
        /// <item><description>Handles persistence by storing the current value in browser local storage if EnablePersistence is enabled</description></item>
        /// <item><description>Validates component state (<see cref="SfInputBase{TValue}.Disabled"/> and <see cref="ReadOnly"/> properties) before invoking change events</description></item>
        /// <item><description>Creates and populates <see cref="ChangedEventArgs"/> with comprehensive event information</description></item>
        /// <item><description>Invokes <see cref="ValueChange"/> event callbacks for external change handling</description></item>
        /// <item><description>Updates PreviousValue tracking for subsequent change detection</description></item>
        /// <item><description>Triggers validation class updates to reflect current validation state</description></item>
        /// </list>
        /// <para>The method respects component state and only raises events when the component is enabled and not readonly.</para>
        /// </remarks>
        private async Task RaiseChangeEventAsync(EventArgs? args = null, bool isInteraction = false)
        {
            if (EnablePersistence)
            {
                await SetLocalStorageAsync(ID, Value!).ConfigureAwait(true);
            }

            if (ValueChange.HasDelegate && !(Disabled || ReadOnly))
            {
                ChangedEventArgs eventArgs = new()
                {
                    Event = args,
                    Value = Value,
                    PreviousValue = _previousValue,
                    IsInteracted = isInteraction,
                };
                await ValueChange.InvokeAsync(eventArgs).ConfigureAwait(true);
            }
            _previousValue = Value;
            UpdateValidateClass();
        }

        /// <summary>
        /// Asynchronously stores the specified data value in the browser's local storage using the provided persistence ID.
        /// </summary>
        /// <param name="persistId">
        /// A <see cref="string"/> representing the unique identifier used as the key for storing the value in local storage. This should be a unique string that identifies the component instance.
        /// </param>
        /// <param name="dataValue">
        /// A <see cref="string"/> value to be stored in local storage. This represents the current component value that should be persisted across sessions.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous local storage operation that completes when the value has been stored.
        /// </returns>
        /// <remarks>
        /// <para>This method uses JavaScript interop to invoke the browser's localStorage.setItem method, allowing the component to maintain its value even after page refreshes or browser restarts.</para>
        /// <para>The stored value is automatically restored during component initialization if persistence is enabled.</para>
        /// </remarks>
        private async Task SetLocalStorageAsync(string persistId, string dataValue)
        {
            await InvokeVoidAsync(_baseJsModule, _baseJsInProcessModule, "setLocalStorageItem", persistId, dataValue).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates the validation-related CSS classes on the <see cref="SfTextBox"/> container element based on the current validation state from Blazor's EditContext validation system.
        /// </summary>
        /// <remarks>
        /// <para>This method performs comprehensive validation class management by:</para>
        /// <list type="bullet">
        /// <item><description>Creating a field identifier from the ValueExpression for EditContext integration</description></item>
        /// <item><description>Removing previously applied validation classes to prevent class accumulation</description></item>
        /// <item><description>Obtaining the current validation state from InputEditContext.FieldCssClass</description></item>
        /// <item><description>Applying appropriate validation classes (invalid, modified-invalid, modified-valid, etc.)</description></item>
        /// <item><description>Managing error and success visual indicators through CSS classes</description></item>
        /// <item><description>Normalizing whitespace in the ContainerClass to prevent formatting issues</description></item>
        /// </list>
        /// <para>The method handles the following validation states:</para>
        /// <list type="bullet">
        /// <item><description>"invalid" or "modified invalid": Applies error styling and removes success classes</description></item>
        /// <item><description>"modified valid": Applies success styling and removes error classes</description></item>
        /// <item><description>"valid": Removes both error and success classes (unless explicitly set in <see cref="SfInputBase{TValue}.CssClass"/>)</description></item>
        /// </list>
        /// <para>This integration with Blazor's validation system ensures that the TextBox provides appropriate visual feedback for form validation scenarios.</para>
        /// </remarks>
        private void UpdateValidateClass()
        {
            if (ValueExpression is not null && InputEditContext is not null)
            {
                FieldIdentifier fieldIdentifier = FieldIdentifier.Create(ValueExpression);
                ContainerClass = !string.IsNullOrEmpty(_validClass) ? SfBaseUtils.RemoveClass(ContainerClass, " " + _validClass) : ContainerClass;
                ContainerClass = !string.IsNullOrEmpty(_validClass) ? SfBaseUtils.RemoveClass(ContainerClass, _validClass + " ") : ContainerClass;
                _validClass = InputEditContext.FieldCssClass(fieldIdentifier);
                ContainerClass = !string.IsNullOrEmpty(_validClass) ? SfBaseUtils.AddClass(ContainerClass, _validClass) : ContainerClass;
                ContainerClass = WhitespaceRegex().Replace(ContainerClass, " ");
                if (_validClass is INVALID or MODIFIED_INVALID)
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, SUCCESS_CLASS);
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, ERROR_CLASS);
                }
                else if (_validClass == MODIFIED_VALID)
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERROR_CLASS);
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, SUCCESS_CLASS);
                }
                else if (_validClass == "valid" && !(!string.IsNullOrEmpty(CssClass) && (CssClass.Contains(ERROR_CLASS, StringComparison.Ordinal) || CssClass.Contains(SUCCESS_CLASS, StringComparison.Ordinal))))
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERROR_CLASS);
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, SUCCESS_CLASS);
                }
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Asynchronously handles the focus event when the input element receives focus, invoking registered focus event callbacks.
        /// </summary>
        /// <param name="args">
        /// A <see cref="FocusEventArgs"/> containing focus event details from the browser's focus event.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous focus event handling operation that completes when all focus event callbacks finish.
        /// </returns>
        /// <remarks>
        /// <para>This method processes focus events by:</para>
        /// <list type="bullet">
        /// <item><description>Creating <see cref="FocusInEventArgs"/> with current component state and event information</description></item>
        /// <item><description>Invoking the <see cref="OnFocus"/> event callback if registered, providing detailed focus context</description></item>
        /// <item><description>Invoking the OnFocus event callback if registered for additional focus handling</description></item>
        /// </list>
        /// <para>The method provides both detailed focus information through FocusInEventArgs and standard focus event args for different use cases and backward compatibility.</para>
        /// </remarks>
        protected override async Task FocusHandlerAsync(FocusEventArgs args)
        {
            if (OnFocus.HasDelegate && !_isClearIconClick)
            {
                FocusInEventArgs eventArgs = new()
                {
                    Event = args,
                    Value = Value
                };
                await OnFocus.InvokeAsync(eventArgs).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Asynchronously handles the focus out (blur) event when the input element loses focus, performing value change detection and invoking registered blur event callbacks.
        /// </summary>
        /// <param name="args">
        /// A <see cref="FocusEventArgs"/> containing blur event details from the browser's blur event.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous focus out event handling operation that completes when all blur event callbacks and change notifications finish.
        /// </returns>
        /// <remarks>
        /// <para>This method processes blur events by:</para>
        /// <list type="bullet">
        /// <item><description>Performing change detection by comparing PreviousValue with current InputTextValue</description></item>
        /// <item><description>Raising change events if the value has been modified since the last change notification</description></item>
        /// <item><description>Creating <see cref="FocusOutEventArgs"/> with current component state and event information</description></item>
        /// <item><description>Invoking the <see cref="OnBlur"/> event callback if registered, providing detailed focus out context</description></item>
        /// </list>
        /// <para>The change detection logic ensures that value changes are properly captured and notified even if they occurred during input without explicit change events being fired.</para>
        /// </remarks>
        protected override async Task FocusOutHandlerAsync(FocusEventArgs args)
        {
            if (!(string.IsNullOrEmpty(_previousValue) && string.IsNullOrEmpty(Value) && string.IsNullOrEmpty(InputTextValue)) && _previousValue != InputTextValue)
            {
                await RaiseChangeEventAsync(args, true).ConfigureAwait(true);
            }

            if (OnBlur.HasDelegate && !_isClearIconClick)
            {
                FocusOutEventArgs eventArgs = new()
                {
                    Event = args,
                    Value = Value
                };
                await OnBlur.InvokeAsync(eventArgs).ConfigureAwait(true);
            }
        }
        /// <summary>
        /// Formats the specified generic value into a string representation suitable for display in the input element.
        /// </summary>
        /// <param name="genericValue">
        /// A <see cref="string"/> value to be formatted. Can be <see langword="null"/>.
        /// </param>
        /// <returns>
        /// A formatted <see cref="string"/> representation of the input value, or <see langword="null"/> if the input is <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// <para>This method provides a safe conversion mechanism by:</para>
        /// <list type="bullet">
        /// <item><description>Returning the default value (<see langword="null"/>) when the input value is <see langword="null"/></description></item>
        /// <item><description>Using SfBaseUtils.ChangeType for type-safe conversion to string</description></item>
        /// <item><description>Ensuring consistent string formatting across the component</description></item>
        /// </list>
        /// <para>The method is part of the value formatting pipeline and ensures that all values displayed in the TextBox are properly converted to string representation.</para>
        /// </remarks>
        protected override string? FormatValue(string? genericValue)
        {
            return genericValue == null ? default : (string)SfBaseUtils.ChangeType(genericValue, typeof(string))!;
        }

        /// <summary>
        /// Asynchronously handles input events that occur during real-time text entry, providing immediate feedback and event notifications.
        /// </summary>
        /// <param name="args">
        /// A <see cref="ChangeEventArgs"/> containing the current input value and event details from the browser's input event. Can be <see langword="null"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous input event handling operation that completes when all input event callbacks finish.
        /// </returns>
        /// <remarks>
        /// <para>This method processes real-time input events by:</para>
        /// <list type="bullet">
        /// <item><description>Creating <see cref="InputEventArgs"/> with comprehensive input context information</description></item>
        /// <item><description>Providing the current input value and previous value for comparison</description></item>
        /// <item><description>Invoking <see cref="OnInput"/> event callbacks if registered for real-time input monitoring</description></item>
        /// <item><description>Updating InputPreviousValue tracking for subsequent change detection</description></item>
        /// </list>
        /// <para>The input handler is essential for features that require real-time feedback, such as character counting, input validation, and dynamic content updates.</para>
        /// </remarks>
        protected override async Task InputHandlerAsync(ChangeEventArgs? args)
        {
            if (OnInput.HasDelegate)
            {
                InputEventArgs eventArgs = new()
                {
                    Value = args is not null ? args.Value as string : null,
                    Event = args,
                    PreviousValue = _inputPreviousValue
                };
                await OnInput.InvokeAsync(eventArgs).ConfigureAwait(true);
            }
            _inputPreviousValue = args is not null ? args.Value as string : null;
        }

        /// <summary>
        /// Asynchronously handles change events that occur when the input value is modified and committed, typically triggered by focus loss or explicit user actions.
        /// </summary>
        /// <param name="args">
        /// A <see cref="ChangeEventArgs"/> containing the committed input value and event details from the browser's change event. Can be <see langword="null"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous change event handling operation that completes when all change event processing finishes.
        /// </returns>
        /// <remarks>
        /// <para>This method processes committed value changes by:</para>
        /// <list type="bullet">
        /// <item><description>Extracting the changed value from the event arguments</description></item>
        /// <item><description>Updating the component's CurrentValueAsString property to reflect the new value</description></item>
        /// <item><description>Raising the change event through RaiseChangeEvent with interaction flag set to <see langword="true"/></description></item>
        /// </list>
        /// <para>Unlike the InputHandler which fires during real-time typing, the ChangeHandler fires when the value is considered "committed" by the browser, making it suitable for final value processing and validation.</para>
        /// </remarks>
        protected override async Task ChangeHandlerAsync(ChangeEventArgs? args)
        {
            string? changeVal = args is not null ? args.Value as string : null;
            CurrentValueAsString = changeVal;
            await RaiseChangeEventAsync(args, true).ConfigureAwait(true);
        }

        #endregion

        #region JS Invokable Methods

        /// <summary>
        /// Updates the disabled state of the TextBox based on the parent fieldset's disabled attribute.
        /// </summary>
        /// <param name="isDisabled">
        /// A boolean value indicating whether the component should be disabled based on the parent fieldset state.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the synchronous completion of the operation.
        /// </returns>
        /// <remarks>
        /// This method is automatically invoked when the TextBox is contained within a 
        /// <c>&lt;fieldset&gt;</c> element that has its <c>disabled</c> attribute changed.
        /// When the parent fieldset is disabled, this method sets the TextBox's <c>Disabled</c>
        /// property to <c>true</c>, preventing user interaction.
        /// This ensures proper accessibility behavior and compliance with HTML form standards.
        /// </remarks>
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
