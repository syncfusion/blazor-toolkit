using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents the Toolkit TextArea component for Blazor applications, which provides a multiline text input element
    /// that allows users to enter and edit multi-line text content with advanced features like resizing, floating labels,
    /// clear button functionality, validation, and various styling options.
    /// </summary>
    /// <remarks>
    /// The SfTextArea component extends the standard HTML textarea element with additional functionality including:
    /// <list type="bullet">
    /// <item><description>Support for floating labels (Auto, Always, Never)</description></item>
    /// <item><description>Built-in clear button with customizable behavior</description></item>
    /// <item><description>Input validation with visual feedback through Blazor's EditForm integration</description></item>
    /// <item><description>Resizable text area with configurable resize modes (None, Horizontal, Vertical, Both)</description></item>
    /// <item><description>Accessibility features with ARIA support</description></item>
    /// <item><description>Persistence support for maintaining state across browser sessions</description></item>
    /// <item><description>Customizable styling and theming options</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// A basic TextArea component with floating label.
    /// <code><![CDATA[
    /// <SfTextArea @bind-Value="@textValue"
    ///             Placeholder="Enter your message..."
    ///             RowCount="4"
    ///             ColumnCount="50"
    ///             MaxLength="500"
    ///             ResizeMode="Resize.Both">
    /// </SfTextArea>
    ///
    /// @code {
    ///     private string textValue = string.Empty;
    /// }
    /// ]]></code>
    /// </example>
    public partial class SfTextArea : SfInputBase<string>
    {
        #region Constants

        private const string INVALID = "invalid";
        private const string MODIFIED_INVALID = "modified invalid";
        private const string MODIFIED_VALID = "modified valid";
        private const string ERROR_CLASS = "e-error";
        private const string SUCCESS_CLASS = "e-success";
        private const string ROOT_CLASS = "e-control e-textarea e-lib";
        private const string RESIZE_VERTICAL = "e-resize-y";
        private const string RESIZE_HORIZONTAL = "e-resize-x";
        private const string RESIZE_BOTH = "e-resize-xy";
        private const string RESIZE_NONE = "e-resize-none";
        private const string CONTAINER_CLASS = "e-multi-line-input";
        private const string AUTO_WIDTH = "e-auto-width";
        private const string MAX_LENGTH = "maxlength";
        private const string ROWS = "rows";
        private const string COLS = "cols";
        private const string ARIA_MULTILINE = "aria-multiline";
        private const string OUTLINE = "e-outline";
        private const string ARIA_LABEL = "aria-label";
        private const string NULL_STRING = "null";

        #endregion

        #region Private Variables

        [GeneratedRegex(@"\s+")]
        private static partial Regex WhitespaceRegex();

        private string _previousResize = string.Empty;
        private string? _inputPreviousValue;
        private string? _validClass;
        private string? _previousValue;
        private Dictionary<string, object> ContainerAttribute { get; set; } = [];
        private IJSObjectReference? _textAreaJsModule;
        private IJSInProcessObjectReference? _textAreaJsInProcessModule;

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the input element's HTML attributes based on its current properties.
        /// </summary>
        /// <remarks>
        /// This method populates the InputHtmlAttributes dictionary with attributes specific to the textarea element,
        /// such as aria-multiline, maxlength, rows, and cols, consuming the component's public properties.
        /// </remarks>
        private void UpdateInputAttributes()
        {
            InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIA_MULTILINE, "true", InputHtmlAttributes);
            if (MaxLength > 0)
            {
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(MAX_LENGTH, MaxLength.ToString(CultureInfo.InvariantCulture), InputHtmlAttributes);
            }
            if (RowCount > 0)
            {
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ROWS, RowCount.ToString(CultureInfo.InvariantCulture), InputHtmlAttributes);
            }
            if (ColumnCount > 0)
            {
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(COLS, ColumnCount.ToString(CultureInfo.InvariantCulture), InputHtmlAttributes);
            }
        }

        /// <summary>
        /// Sets up the initial state and internal properties of the component.
        /// </summary>
        /// <remarks>
        /// This is called during the OnInitializedAsync lifecycle method to ensure all internal fields are
        /// populated with their initial values from the corresponding component parameters before the first render.
        /// </remarks>
        private void InitializeProps()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = $"textarea-{Guid.NewGuid()}";
            }
            DataId = ID;
            RootClass = ROOT_CLASS;
            ContainerClass = CONTAINER_CLASS;
            _inputPreviousValue = Value;
            _previousValue = Value;
            _cssClass = CssClass;
            InputTextValue = Value;
            InternalValue = Value;
            _resizeMode = ResizeMode;
            _maxLength = MaxLength;
            _rowCount = RowCount;
            _columnCount = ColumnCount;
            _disabled = Disabled;
            _width = Width;
        }

        /// <summary>
        /// Handles the logic for reacting to changes in component parameters.
        /// </summary>
        /// <param name="newProps">A dictionary containing the names and new values of the properties that have changed.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <remarks>
        /// This method iterates through the changed properties and triggers specific update logic,
        /// such as updating the value, re-evaluating CSS classes, or changing resize behavior.
        /// </remarks>
        private async Task OnPropertyChangeAsync(Dictionary<string, object> newProps)
        {
            foreach (string prop in newProps.Keys)
            {
                switch (prop)
                {
                    case nameof(Value):
                        await HandleValuePropertyChangeAsync().ConfigureAwait(false);
                        break;
                    case nameof(Disabled):
                        HandleEnabledPropertyChange();
                        break;
                    case nameof(CssClass):
                        HandleCssClassPropertyChange();
                        break;
                    case nameof(FloatLabelType):
                        await OnAfterScriptRenderedAsync().ConfigureAwait(true);
                        break;
                    case nameof(ResizeMode):
                        SetResizeMode();
                        break;
                    case nameof(MaxLength):
                        InputHtmlAttributes = SfBaseUtils.UpdateDictionary(MAX_LENGTH, MaxLength.ToString(CultureInfo.InvariantCulture), InputHtmlAttributes);
                        break;
                    case nameof(RowCount):
                        InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ROWS, RowCount.ToString(CultureInfo.InvariantCulture), InputHtmlAttributes);
                        break;
                    case nameof(ColumnCount):
                        InputHtmlAttributes = SfBaseUtils.UpdateDictionary(COLS, ColumnCount.ToString(CultureInfo.InvariantCulture), InputHtmlAttributes);
                        break;
                    case nameof(Width):
                        SetWidth();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Applies value-related updates when the <see cref="SfInputBase{TValue}.Value"/> parameter changes.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        /// <remarks>
        /// <para>Synchronizes internal previous-value tracking using <c>PrevChanges</c> when available.</para>
        /// <para>Invokes <see cref="SfInputBase{TValue}.SetValueAsync"/> and raises <see cref="ValueChange"/> via <see cref="RaiseChangeEventAsync(bool)"/> when a meaningful change is detected.</para>
        /// </remarks>
        private async Task HandleValuePropertyChangeAsync()
        {
            if (!(PropertyChanges is not null && PropertyChanges.ContainsKey(nameof(Value))) || string.Equals(_previousValue, Value, StringComparison.Ordinal))
            {
                return;
            }
            await SetValueAsync(Value, FloatLabelType, ShowClearButton).ConfigureAwait(true);
            await RaiseChangeEventAsync(false).ConfigureAwait(true);
        }

        /// <summary>
        /// Applies enabled/disabled state changes to internal state and resize-related CSS classes.
        /// </summary>
        /// <remarks>
        /// When disabled, forces the resize mode to <c>e-resize-none</c>. When enabled, restores the previously applied resize class.
        /// </remarks>
        private void HandleEnabledPropertyChange()
        {
            _disabled = Disabled;
            if (Disabled)
            {
                RootClass = SfBaseUtils.RemoveClass(RootClass, _previousResize);
                RootClass = SfBaseUtils.AddClass(RootClass, RESIZE_NONE);
                return;
            }

            RootClass = SfBaseUtils.RemoveClass(RootClass, RESIZE_NONE);
            RootClass = SfBaseUtils.AddClass(RootClass, _previousResize);
        }

        /// <summary>
        /// Reconciles the container CSS class when the <see cref="SfInputBase{TValue}.CssClass"/> parameter changes.
        /// </summary>
        /// <remarks>
        /// Removes the previously applied consumer class and re-applies the current <see cref="SfInputBase{TValue}.CssClass"/> value.
        /// </remarks>
        private void HandleCssClassPropertyChange()
        {
            ContainerClass = string.IsNullOrEmpty(ContainerClass) ? ContainerClass : SfBaseUtils.RemoveClass(ContainerClass, !string.IsNullOrEmpty(_cssClass) ? _cssClass.Trim() : _cssClass);
            _cssClass = CssClass;
            SetCssClass();
        }

        /// <summary>
        /// Compares current parameter values with their previous values to detect changes.
        /// </summary>
        /// <returns>A <see cref="Task"/> that completes when the property change detection is finished.</returns>
        /// <remarks>
        /// This method uses the NotifyPropertyChanges helper to populate the PropertyChanges dictionary,
        /// which is then consumed by OnPropertyChangeAsync to react to updates.
        /// </remarks>
        private void PropertyUpdate()
        {
            _ = NotifyPropertyChanges(nameof(CssClass), CssClass, _cssClass);
            InternalValue = NotifyPropertyChanges(nameof(Value), Value, InternalValue);
            _resizeMode = NotifyPropertyChanges(nameof(ResizeMode), ResizeMode, _resizeMode);
            _maxLength = NotifyPropertyChanges(nameof(MaxLength), MaxLength, _maxLength);
            _rowCount = NotifyPropertyChanges(nameof(RowCount), RowCount, _rowCount);
            _columnCount = NotifyPropertyChanges(nameof(ColumnCount), ColumnCount, _columnCount);
            _disabled = NotifyPropertyChanges(nameof(Disabled), Disabled, _disabled);
            _width = NotifyPropertyChanges(nameof(Width), Width, _width);
        }

        /// <summary>
        /// Applies the appropriate CSS classes to the root element to control the resize behavior.
        /// </summary>
        /// <remarks>
        /// It determines the correct resize class based on the ResizeMode, Enabled, and Width properties.
        /// </remarks>
        private void SetResizeMode()
        {
            if (!Disabled)
            {
                RootClass = SfBaseUtils.RemoveClass(RootClass, _previousResize);
                string resizeClass = ResizeMode switch
                {
                    Resize.Both => Width is null ? RESIZE_BOTH : RESIZE_VERTICAL,
                    Resize.Horizontal when Width is null => RESIZE_HORIZONTAL,
                    Resize.Vertical => RESIZE_VERTICAL,
                    Resize.None => RESIZE_NONE,
                    _ => RESIZE_NONE
                };

                RootClass = SfBaseUtils.AddClass(RootClass, resizeClass);
                _previousResize = resizeClass;
            }
            else
            {
                RootClass = SfBaseUtils.RemoveClass(RootClass, _previousResize);
                RootClass = SfBaseUtils.AddClass(RootClass, RESIZE_NONE);
            }
        }

        /// <summary>
        /// Adjusts CSS classes based on whether a specific width has been set.
        /// </summary>
        /// <remarks>
        /// If Width is not specified, it adds an e-auto-width class. It also modifies the resize behavior
        /// if ResizeMode is set to Both while a fixed width is applied, defaulting to vertical resizing.
        /// </remarks>
        private void SetWidth()
        {
            if (Width is null)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, AUTO_WIDTH);
                if (RootClass.Contains(RESIZE_VERTICAL, StringComparison.Ordinal))
                {
                    RootClass = SfBaseUtils.RemoveClass(RootClass, RESIZE_VERTICAL);
                }
                RootClass = SfBaseUtils.AddClass(RootClass, _previousResize);
            }
            else if (ResizeMode == Resize.Both)
            {
                RootClass = SfBaseUtils.RemoveClass(RootClass, _previousResize);
                RootClass = SfBaseUtils.AddClass(RootClass, RESIZE_VERTICAL);
            }
            if (Width is not null)
            {
                string styleWidth = $"width: {Width}";
                ContainerAttribute = new Dictionary<string, object>
                {
                    {"style", styleWidth }
                };
            }
        }

        /// <summary>
        /// Merges the user-defined CssClass into the component's container element classes.
        /// </summary>
        /// <remarks>
        /// This ensures that any custom classes provided by the consumer are applied to the primary container element.
        /// </remarks>
        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = ContainerClass.Contains(CssClass, StringComparison.Ordinal) ? ContainerClass : SfBaseUtils.AddClass(ContainerClass, CssClass);
            }
        }

        /// <summary>
        /// Clears the current input value, raises related input/change events, and sets focus back to the control.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <remarks>
        /// This method nullifies the component's value, invokes the Input and ValueChange events,
        /// and brings focus back to the input element.
        /// </remarks>
        private async Task InvokeClearBtnEventAsync()
        {
            CurrentValueAsString = null;
            await Task.Delay(100).ConfigureAwait(true); // Remove the value on time delay in the component
            await SetValueAsync(string.Empty, FloatLabelType, ShowClearButton).ConfigureAwait(true);
            if (OnInput.HasDelegate)
            {
                TextAreaInputEventArgs eventArgs = new()
                {
                    Value = InputTextValue,
                    PreviousValue = _inputPreviousValue,
                };
                await OnInput.InvokeAsync(eventArgs).ConfigureAwait(false);
            }
            await RaiseChangeEventAsync(true).ConfigureAwait(true);
            _previousValue = InputTextValue;
            await FocusAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Persists the current value if persistence is enabled, raises the value-change event when applicable,
        /// updates internal previous-value state, and refreshes validation classes.
        /// </summary>
        /// <param name="isInteraction">A flag indicating if the change was triggered by direct user interaction.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        private async Task RaiseChangeEventAsync(bool isInteraction = false)
        {
            if (EnablePersistence)
            {
                await SetLocalStorageAsync(ID, Value!).ConfigureAwait(true);
            }

            if (ValueChange.HasDelegate && !(Disabled || ReadOnly))
            {
                TextAreaValueChangeEventArgs eventArgs = new()
                {
                    Value = Value,
                    PreviousValue = _previousValue,
                    IsInteracted = isInteraction,
                };
                await ValueChange.InvokeAsync(eventArgs).ConfigureAwait(true);
            }
            _previousValue = Value;
            UpdateValidationClass();
        }

        /// <summary>
        /// Persists a key–value pair to the browser's localStorage.
        /// </summary>
        /// <param name="persistId">The key used to store the data in local storage.</param>
        /// <param name="dataValue">The string value to be stored.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        private async Task SetLocalStorageAsync(string persistId, string dataValue)
        {
            await InvokeVoidAsync(_baseJsModule, _baseJsInProcessModule, "setLocalStorageItem", [persistId, dataValue]).ConfigureAwait(true);
        }

        /// <summary>
        /// Applies validation-related CSS classes based on the EditContext state.
        /// </summary>
        /// <remarks>
        /// This method integrates with Blazor's standard forms and validation system. It queries the field's
        /// validation status from the EditContext and updates the component's container class to reflect
        /// whether the input is valid, invalid, or modified.
        /// </remarks>
        private void UpdateValidationClass()
        {
            if (ValueExpression is null || InputEditContext is null)
            {
                return;
            }
            FieldIdentifier fieldIdentifier = FieldIdentifier.Create(ValueExpression);
            ContainerClass = !string.IsNullOrEmpty(_validClass) ? SfBaseUtils.RemoveClass(ContainerClass, " " + _validClass) : ContainerClass;
            ContainerClass = !string.IsNullOrEmpty(_validClass) ? SfBaseUtils.RemoveClass(ContainerClass, _validClass + " ") : ContainerClass;
            _validClass = InputEditContext.FieldCssClass(fieldIdentifier);
            ContainerClass = !string.IsNullOrEmpty(_validClass) ? SfBaseUtils.AddClass(ContainerClass, _validClass) : ContainerClass;
            ContainerClass = WhitespaceRegex().Replace(ContainerClass, " ");
            ApplyValidationStateClasses();
        }

        /// <summary>
        /// Applies component-level error/success classes based on the resolved field validation state.
        /// </summary>
        /// <remarks>
        /// Adds <c>e-error</c> for invalid states and <c>e-success</c> for modified-valid states.
        /// For the <c>valid</c> state, clears error/success classes unless overridden by <see cref="SfInputBase{TValue}.CssClass"/>.
        /// </remarks>
        private void ApplyValidationStateClasses()
        {
            if (_validClass is INVALID or MODIFIED_INVALID)
            {
                ApplyErrorState();
                return;
            }

            if (_validClass == MODIFIED_VALID)
            {
                ApplySuccessState();
                return;
            }

            if (_validClass == "valid" && !(!string.IsNullOrEmpty(CssClass) && (CssClass.Contains(ERROR_CLASS, StringComparison.Ordinal) || CssClass.Contains(SUCCESS_CLASS, StringComparison.Ordinal))))
            {
                ClearValidationState();
            }
        }

        /// <summary>
        /// Applies the error visual state to the container.
        /// </summary>
        private void ApplyErrorState()
        {
            ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, SUCCESS_CLASS);
            ContainerClass = SfBaseUtils.AddClass(ContainerClass, ERROR_CLASS);
        }

        /// <summary>
        /// Applies the success visual state to the container.
        /// </summary>
        private void ApplySuccessState()
        {
            ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERROR_CLASS);
            ContainerClass = SfBaseUtils.AddClass(ContainerClass, SUCCESS_CLASS);
        }

        /// <summary>
        /// Clears error and success classes from the container.
        /// </summary>
        private void ClearValidationState()
        {
            ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERROR_CLASS);
            ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, SUCCESS_CLASS);
        }

        /// <summary>
        /// Restores the persisted value from <c>window.localStorage</c> when persistence is enabled.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        /// <remarks>
        /// Normalizes empty and <c>"null"</c> values to null.
        /// When the persisted value is null and the current <see cref="SfInputBase{TValue}.Value"/> is non-null, the current value is preserved.
        /// </remarks>
        private async Task RestorePersistedValueAsync()
        {
            if (!EnablePersistence)
            {
                return;
            }
            string? localStorageValue = await InvokeAsync<string>(_baseJsModule!, _baseJsInProcessModule!, "getLocalStorageItem", [ID]).ConfigureAwait(true);
            localStorageValue = (string.IsNullOrEmpty(localStorageValue) || localStorageValue == NULL_STRING) ? null : localStorageValue;
            if (!(localStorageValue is null && Value is not null))
            {
                await SetValueAsync(localStorageValue ?? string.Empty, FloatLabelType, ShowClearButton).ConfigureAwait(true);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Handles touch events for the clear button in touch-enabled devices.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous touch event handling operation.</returns>
        /// <remarks>
        /// This method provides touch-specific handling for the clear button functionality, 
        /// ensuring proper behavior on mobile and tablet devices.
        /// </remarks>
        internal async Task BindClearBtnTouchEventsAsync()
        {
            await InvokeClearBtnEventAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Handles click events for the clear button to reset the TextArea value.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous clear button event handling operation.</returns>
        /// <remarks>
        /// This method clears the TextArea value and returns focus to the input element after clearing, 
        /// providing a complete clear button interaction experience.
        /// </remarks>
        internal async Task BindClearBtnEventsAsync()
        {
            await InvokeClearBtnEventAsync().ConfigureAwait(true);
            await FocusAsync().ConfigureAwait(true);
        }

        #endregion
    }
}
