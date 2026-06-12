using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// The NumericTextBox is used to get the number inputs from the user. The input values can be incremented or decremented by a predefined step value.
    /// </summary>
    /// <typeparam name="TValue">Specifies the generic type of the <see cref="SfNumericTextBox{TValue}"/> component. This type determines the data type of the numeric value that can be entered and processed by the component.</typeparam>
    /// <remarks>
    /// The <see cref="SfNumericTextBox{TValue}"/> component provides a user-friendly interface for numeric input with features like spin buttons, validation, formatting, and culture-specific number display. It supports various numeric types including <c>int</c>, <c>double</c>, <c>decimal</c>, <c>float</c>, and their nullable counterparts.
    /// </remarks>
    /// <example>
    /// A simple NumericTextBox component.
    /// <code><![CDATA[
    /// <SfNumericTextBox TValue="int" Value="10" Min="0" Max="100" Step="1"></SfNumericTextBox>
    /// ]]></code>
    /// </example>
    public partial class SfNumericTextBox<TValue> : SfInputBase<TValue>
    {
        private const string INVALID = "invalid";
        private const string MODIFIED_INVALID = "modified invalid";
        private const string MODIFIED_VALID = "modified valid";
        private const string ERROR_CLASS = "e-error";
        private const string SUCCESS_CLASS = "e-success";
        private const string BLUR = "Blur";
        private const string VALUECHANGE = "ValueChange";
        private const string STEP = "Step";
        private const string MIN = "Min";
        private const string MAX = "Max";
        private const string CONTAINER_CLASS = "e-numeric";
        private const string ROOT_CLASS = "e-control e-numerictextbox e-lib";
        private const string ROLE = "role";
        private const string SPIN_BUTTON = "spinbutton";
        private const string ARIA_LIVE = "aria-live";
        private const string ASSERTIVE = "assertive";
        private const string INCREMENT_CONTENT = "increment";
        private const string ADD = "add";
        private const string SUB = "sub";
        private const string ARIA_INVALID = "aria-invalid";
        private const string ARIA_VALUE_NOW = "aria-valuenow";
        private const string ARIA_VALUE_MIN = "aria-valuemin";
        private const string ARIA_VALUE_MAX = "aria-valuemax";
        private const string FALSE = "false";
        private const string TRUE = "true";
        private const string MIN_VALUE = "MinValue";
        private const string MAX_VALUE = "MaxValue";
        private const string DISABLED = "Disabled";
        private const string VALIDATE_DECIMAL_TYPE = "ValidateDecimalOnType";
        private const string READ_ONLY = "Readonly";
        private const string SHOW_SPIN_BUTTON = "ShowSpinButton";
        private const string DECIMALS = "Decimals";
        private const string ARABIC = "ar";
        private const string THAILAND = "th";
        private const string PERSIAN = "fa";

        private bool IsTriggerFocusHandler { get; set; }

        private bool IsScriptRendered { get; set; }

        private bool IsClearIconClick { get; set; }

        private bool ClearBtnStopPropagation { get; set; }

        /// <summary>
        /// Specifies the class value that is appended to container of TextBox.
        /// </summary>
        /// <exclude/>
        protected override string ContainerClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Set the component class to element.
        /// </summary>
        /// <exclude/>
        protected override string RootClass { get; set; } = "e-control e-numerictextbox e-lib";

        private TValue? PrevValue { get; set; }

        private bool IsDevice { get; set; }

        /// <summary>
        /// Specifies the input is focused state.
        /// </summary>
        internal bool IsFocus { get; set; }

        private bool IsPrevFocused { get; set; }

        private bool IsValidState { get; set; }

        private bool IsSpinButtonChanged { get; set; }

        private bool IsNumberCulture { get; set; }

        private string? ValidClass { get; set; }

        private string? FocusInputValue { get; set; }

        private bool IsDropValue { get; set; }

        private bool IsPasteValue { get; set; }

        /// <summary>
        /// Set the min and max validation value to the property.
        /// </summary>
        private string? MinMaxValue { get; set; }

        private bool IsInValidNumber { get; set; }

        private bool IsDoubleValue { get; set; }

        private bool _allowMouseWheel = true;

        private IJSObjectReference? _numericTextBoxJsModule;

        /// <summary>
        /// Gets or sets the logger used for capturing diagnostic and runtime information
        /// related to the <see cref="SfNumericTextBox{TValue}"/> component.
        /// </summary>
        /// <exclude/>
        [Inject]
        protected ILogger<SfNumericTextBox<TValue>> Logger { get; set; }

        private IJSInProcessObjectReference? _numericTextBoxJsInProcessModule;
        private DotNetObjectReference<SfNumericTextBox<TValue>>? _selectRangeDotNetRef;
        private CancellationTokenSource? _delayCancellationTokenSource;

        private const int ClearButtonDelayMs = 200;
        private const int StrictModeDelayMs = 30;

        [GeneratedRegex(@"[,.](.*)")]
        private static partial Regex DecimalPartRegex();

        [GeneratedRegex(@"[eE](.*)")]
        private static partial Regex ExponentialRegex();

        /// <summary>
        /// Set the css class to component container element.
        /// </summary>
        /// <remarks>
        /// This method adds the <see cref="SfInputBase{TValue}.CssClass"/> property values to the container element,
        /// enabling custom styling without overwriting existing classes.
        /// </remarks>
        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = ContainerClass.Contains(CssClass, StringComparison.Ordinal) ? ContainerClass : SfBaseUtils.AddClass(ContainerClass, CssClass);
            }
        }

        /// <summary>
        /// Handles property changes and updates the component state accordingly.
        /// </summary>
        /// <param name="newProps">A dictionary containing the properties that have changed and their new values.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method processes various property changes including Value, Format, Min, Max, Step, Enabled, Decimals, Readonly, ValidateDecimalOnType, ShowSpinButton, CssClass, and FloatLabelType. It prevents recursive calls and handles value formatting and validation.
        /// </remarks>
        private async Task OnPropertyChangedAsync(Dictionary<string, object> newProps)
        {
            foreach (KeyValuePair<string, object> prop in newProps)
            {
                switch (prop.Key)
                {
                    case nameof(Value):
                    case nameof(Format):
                        if (CompareValue(PrevValue, InputTextValue) == 0)
                        {
                            return;
                        }
                        PropertyChanges?.Remove(nameof(Value));
                        await ChangeValueAsync(value: (Value is null) ? default : StrictMode ? TrimValue(Value) : Value).ConfigureAwait(true);
                        if (prop.Key == "Value")
                        {
                            await RaiseChangeEventAsync(null).ConfigureAwait(true);
                        }
                        FocusInputValue = FormatValueAsString(Value);
                        break;
                    case nameof(Min):
                    case nameof(Max):
                        ValidateMinMax();
                        if (!StrictMode)
                        {
                            ValidateState();
                        }
                        break;
                    case nameof(Step):
                        ValidateStep();
                        break;
                    case nameof(Disabled):
                    case nameof(Decimals):
                    case nameof(Readonly):
                    case nameof(ValidateDecimalOnType):
                        await InvokeVoidAsync(_numericTextBoxJsModule, _numericTextBoxJsInProcessModule, "propertyChanges", [DataId, new NumericClientProps { Readonly = Readonly, Disabled = Disabled, Locale = CultureInfo.CurrentCulture.Name, ValidateDecimalOnType = ValidateDecimalOnType, Decimals = Decimals, DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator }]).ConfigureAwait(true);
                        break;
                    case nameof(ShowSpinButton):
                        IsSpinButtonChanged = true;
                        break;
                    case nameof(CssClass):
                        ContainerClass = string.IsNullOrEmpty(ContainerClass) ? ContainerClass : SfBaseUtils.RemoveClass(ContainerClass, ContainerCssClass);
                        ContainerCssClass = CssClass;
                        SetCssClass();
                        break;
                    case nameof(FloatLabelType):
                        await OnAfterScriptRenderedAsync().ConfigureAwait(true);
                        break;
                    case nameof(AllowMouseWheel):
                        _allowMouseWheel = AllowMouseWheel;
                        if (IsRendered)
                        {
                            await InvokeVoidAsync(_numericTextBoxJsModule, _numericTextBoxJsInProcessModule, "propertyChanges", [DataId, new NumericClientProps { Readonly = Readonly, Disabled = Disabled, AllowMouseWheel = AllowMouseWheel, Decimals = Decimals, ValidateDecimalOnType = ValidateDecimalOnType, DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, Locale = CultureInfo.CurrentCulture.Name }]).ConfigureAwait(true);

                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Formats the specified numeric value as a string representation based on the current formatting settings.
        /// </summary>
        /// <param name="formatValue">The numeric value to be formatted.</param>
        /// <returns>A string representation of the formatted value, or null if an error occurs.</returns>
        /// <remarks>
        /// This method applies culture-specific formatting, handles exponential notation, manages decimal separators, and updates ARIA attributes for accessibility. The formatting behavior differs based on whether the input is focused or not.
        /// </remarks>
        /// <exception cref="Exception">Logs any unexpected errors that occur during formatting to the console.</exception>
        /// <exclude/>
        protected override string? FormatValueAsString(TValue? formatValue)
        {
            try
            {
                if (formatValue is not null)
                {
                    formatValue = StrictMode ? TrimValue(formatValue) : formatValue;
                    string? value = FormatNumber();
                    bool isExponential = (Convert.ToString(formatValue, CultureInfo.CurrentCulture) ?? string.Empty).Contains('E', StringComparison.Ordinal);
                    bool isNumberFormat = Format is not null && Format.ToLower(CultureInfo.CurrentCulture).Contains('n', StringComparison.Ordinal);
                    string? formatString = (isExponential && isNumberFormat) ? null : Format;
                    string elemValue = IsFocus ? SfBaseUtils.RemoveClass(value, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator)
                            : Intl.GetNumericFormat(formatValue, formatString, Currency);
                    if (IsIgnoreDecimal() && IsFocus)
                    {
                        elemValue = SfBaseUtils.RemoveClass(elemValue, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    }
                    if (formatValue is not null)
                    {
                        InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIA_VALUE_NOW, formatValue, InputHtmlAttributes);
                    }
                    return elemValue;
                }
                else
                {
                    return default;
                }
            }
            catch (FormatException ex)
            {
                _logFormattingErrorOccurred(Logger, ex);
                return null;
            }
            catch (ArgumentException ex)
            {
                _logArgumentErrorOccurred(Logger, ex);
                return null;
            }
            catch (OverflowException ex)
            {
                _logOverflowErrorOccurred(Logger, ex);
                return null;
            }
            catch (InvalidOperationException ex)
            {
                _logInvalidOperationOccurred(Logger, ex);
                return null;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Converts a string input value to the appropriate numeric type based on the current component configuration.
        /// </summary>
        /// <param name="genericValue">The string value to be converted to the numeric type.</param>
        /// <returns>The converted numeric value of type <typeparamref name="TValue"/>, or default value if conversion fails.</returns>
        /// <remarks>
        /// This method handles culture-specific number parsing, decimal separator validation, and supports both double and decimal numeric types. It also manages formatting for focused input states and validates number formats.
        /// </remarks>
        /// <exception cref="Exception">Logs any unexpected errors that occur during value conversion to the console.</exception>
        /// <exclude/>
        protected override TValue? FormatValue(string? genericValue)
        {
            if (IsFocus)
            {
                try
                {
                    string? inputTextValue = genericValue;
                    FocusInputValue = inputTextValue;
                    inputTextValue = IsNumberCulture ? RemoveCultureDigits(inputTextValue) : inputTextValue;
                    string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    bool IsDecimalSeparator = !string.IsNullOrEmpty(inputTextValue) && inputTextValue.Length == 1 && inputTextValue == decimalSeparator;
                    if (IsDoubleValue)
                    {
                        if (double.TryParse(inputTextValue, NumberStyles.Any, CultureInfo.CurrentCulture, out double doubleValue) || string.IsNullOrEmpty(inputTextValue))
                        {
                            TValue? inputValue = string.IsNullOrEmpty(inputTextValue) || IsDecimalSeparator ? default : ChangeType(double.Parse(inputTextValue, CultureInfo.CurrentCulture));
                            int? numberOfDecimals = GetNumberOfDecimals(Value, inputTextValue);
                            string maximumFraction = (numberOfDecimals is not null) ? numberOfDecimals.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                            inputTextValue = (inputValue is null) ? inputTextValue : Intl.GetNumericFormat<TValue>(inputValue, GetFormatString(inputValue, maximumFraction), Currency);
                            inputTextValue = IsNumberCulture ? RemoveCultureDigits(inputTextValue) : inputTextValue;
                            if (IsIgnoreDecimal())
                            {
                                inputTextValue = SfBaseUtils.RemoveClass(inputTextValue, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                            }
                            IsInValidNumber = false;
                            return (inputValue is null) ? default : ChangeType(double.Parse(inputTextValue ?? string.Empty, CultureInfo.CurrentCulture));
                        }
                        else
                        {
                            IsInValidNumber = true;
                            return default;
                        }
                    }
                    else
                    {
                        if ((inputTextValue is not null && decimal.TryParse(inputTextValue, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal decimalValue)) || string.IsNullOrEmpty(inputTextValue))
                        {
                            TValue? inputValue = string.IsNullOrEmpty(inputTextValue) || IsDecimalSeparator ? default : ChangeType(decimal.Parse(inputTextValue, CultureInfo.CurrentCulture));
                            int? numberOfDecimals = GetNumberOfDecimals(Value, inputTextValue);
                            string maximumFraction = (numberOfDecimals is not null) ? numberOfDecimals.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                            inputTextValue = (inputValue is null) ? inputTextValue : Intl.GetNumericFormat<TValue>(inputValue, GetFormatString(inputValue, maximumFraction), Currency);
                            inputTextValue = IsNumberCulture ? RemoveCultureDigits(inputTextValue) : inputTextValue;
                            if (IsIgnoreDecimal())
                            {
                                inputTextValue = SfBaseUtils.RemoveClass(inputTextValue, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                            }
                            IsInValidNumber = false;
                            return (inputValue is null) ? default : ChangeType(decimal.Parse(inputTextValue ?? string.Empty, CultureInfo.CurrentCulture));
                        }
                        else
                        {
                            IsInValidNumber = true;
                            return default;
                        }
                    }
                }
                catch (FormatException ex)
                {
                    _logFormattingErrorInFormatValue(Logger, ex);
                    return default;
                }
                catch (OverflowException ex)
                {
                    _logOverflowErrorInFormatValue(Logger, ex);
                    return default;
                }
                catch (InvalidCastException ex)
                {
                    _logTypeConversionErrorInFormatValue(Logger, ex);
                    return default;
                }
                catch (ArgumentException ex)
                {
                    _logArgumentErrorInFormatValue(Logger, ex);
                    return default;
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return genericValue is not null ? InputTextValue : default;
            }
        }



        /// <summary>
        /// Initializes private backing fields with current property values during component initialization.
        /// </summary>
        /// <remarks>
        /// This method stores the initial values of key properties in private backing fields to enable change detection and property comparison during the component lifecycle. This is essential for the property change notification system to function correctly.
        /// </remarks>
        private void PropertyInitialized()
        {
            //_value = Value;
            PreviousStep = Step;
            PreviousMax = Max;
            ContainerCssClass = CssClass;
            PreviousMin = Min;
            PreviousDisabled = Disabled;
            PreviousReadonly = Readonly;
            PreviousValidateDecimalOnType = ValidateDecimalOnType;
            PreviousShowSpinButton = ShowSpinButton;
            InternalValue = Value;
        }

        /// <summary>
        /// Updates backing fields and tracks property changes for all component properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous property update operation.</returns>
        /// <remarks>
        /// This method is called during parameter updates to:
        /// <list type="bullet">
        /// <item><description>Compare current property values with their previous values</description></item>
        /// <item><description>Update backing fields with new values</description></item>
        /// <item><description>Track changed properties in the PropertyChanges collection</description></item>
        /// <item><description>Enable targeted property-specific updates and optimizations</description></item>
        /// </list>
        /// The method ensures that only properties that have actually changed trigger update logic.
        /// </remarks>
        private async Task PropertyUpdateAsync()
        {
            PreviousStep = NotifyPropertyChanges(STEP, Step, PreviousStep);
            PreviousDecimals = NotifyPropertyChanges(DECIMALS, Decimals, PreviousDecimals);
            NotifyPropertyChanges(nameof(CssClass), CssClass, ContainerCssClass);
            PreviousMax = NotifyPropertyChanges(MAX, Max, PreviousMax);
            PreviousMin = NotifyPropertyChanges(MIN, Min, PreviousMin);
            PreviousFormat = NotifyPropertyChanges(nameof(Format), Format, PreviousFormat);
            PreviousDisabled = NotifyPropertyChanges(DISABLED, Disabled, PreviousDisabled);
            PreviousReadonly = NotifyPropertyChanges(READ_ONLY, Readonly, PreviousReadonly);
            PreviousShowSpinButton = NotifyPropertyChanges(SHOW_SPIN_BUTTON, ShowSpinButton, PreviousShowSpinButton);
            PreviousValidateDecimalOnType = NotifyPropertyChanges(VALIDATE_DECIMAL_TYPE, ValidateDecimalOnType, PreviousValidateDecimalOnType);
            InternalValue = NotifyPropertyChanges(nameof(Value), Value, InternalValue);
            await Task.CompletedTask.ConfigureAwait(true);
            _allowMouseWheel = NotifyPropertyChanges(nameof(AllowMouseWheel), AllowMouseWheel, _allowMouseWheel);
        }

        /// <summary>
        /// Binds the input event to the input element for enabled clear button and float label to the component.
        /// </summary>
        /// <remarks>
        /// This method configures event callbacks for:
        /// <list type="bullet">
        /// <item><description>Input event when FloatLabelType is Auto, Always, or when ShowClearButton is enabled</description></item>
        /// <item><description>Paste event when OnPaste delegate is present</description></item>
        /// </list>
        /// </remarks>
        private void InvokeInputEvent()
        {
            if (FloatLabelType == FloatLabelType.Auto || FloatLabelType == FloatLabelType.Always || ShowClearButton || OnInput.HasDelegate || ValidateOnInput)
            {
                EventCallback<ChangeEventArgs> createInputEvent = EventCallback.Factory.Create<ChangeEventArgs>(this, OnInputHandlerAsync);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary("oninput", createInputEvent, InputHtmlAttributes);
            }
            if (OnPaste.HasDelegate)
            {
                EventCallback<ClipboardEventArgs> createPasteEvent = EventCallback.Factory.Create<ClipboardEventArgs>(this, OnPasteHandlerAsync);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary("onpaste", createPasteEvent, InputHtmlAttributes);
            }
        }

        /// <summary>
        /// Handles input events for the NumericTextBox component.
        /// </summary>
        /// <param name="args">The change event arguments containing the input value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This override processes input events and invokes the OnInput callback if registered.
        /// </remarks>
        /// <exclude/>
        protected override async Task InputHandlerAsync(ChangeEventArgs? args)
        {
            if (OnInput.HasDelegate)
            {
                await OnInput.InvokeAsync(args).ConfigureAwait(true);
            }
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Trims the specified value to ensure it falls within the defined minimum and maximum bounds.
        /// </summary>
        /// <param name="value">The value to be trimmed.</param>
        /// <returns>The trimmed value that falls within the Min and Max bounds, or the original value if no bounds are exceeded.</returns>
        /// <remarks>
        /// This method is used when <c>StrictMode</c> is enabled to enforce min/max constraints on the input value.
        /// If the value exceeds the maximum bound, it returns the maximum value.
        /// If the value is below the minimum bound, it returns the minimum value.
        /// </remarks>
        private TValue TrimValue(TValue value)
        {
            return Max is not null && CompareValue(value, Max) >= 1 ? Max : Min is not null && CompareValue(value, Min) <= -1 ? Min : value;
        }

        /// <summary>
        /// Compares two values of type <typeparamref name="TValue"/> and returns an indication of their relative values.
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="value1"/> and <paramref name="value2"/>:
        /// <list type="bullet">
        /// <item><description>Less than zero: <paramref name="value1"/> is less than <paramref name="value2"/>.</description></item>
        /// <item><description>Zero: <paramref name="value1"/> equals <paramref name="value2"/>.</description></item>
        /// <item><description>Greater than zero: <paramref name="value1"/> is greater than <paramref name="value2"/>.</description></item>
        /// </list>
        /// </returns>
        private static int CompareValue(TValue? value1, TValue? value2)
        {
            return Comparer<TValue>.Default.Compare(value1, value2);
        }

        /// <summary>
        /// Updates the component's value and triggers associated formatting, validation, and display updates.
        /// </summary>
        /// <param name="value">The new numeric value to be set for the component.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method performs the following operations:
        /// <list type="bullet">
        /// <item><description>Rounds the value based on decimal precision settings</description></item>
        /// <item><description>Updates the internal text representation</description></item>
        /// <item><description>Validates the state if not in strict mode</description></item>
        /// <item><description>Clears invalid input indicators when in strict mode</description></item>
        /// </list>
        /// </remarks>
        private async Task ChangeValueAsync(TValue? value)
        {
            if (value is not null)
            {
                int? numberOfDecimals = GetNumberOfDecimals(value);
                TValue? roundValue = RoundNumber(value, numberOfDecimals);
                InputTextValue = roundValue;
            }

            await ModifyTextAsync().ConfigureAwait(true);
            if (!StrictMode)
            {
                ValidateState();
            }
            if (IsInValidNumber && StrictMode)
            {
                await InvokeVoidAsync(_numericTextBoxJsModule, _numericTextBoxJsInProcessModule, "clearInvalid", [DataId, default!]).ConfigureAwait(true);

            }
        }

        /// <summary>
        /// Validates the current state of the numeric input against min/max constraints and number validity.
        /// </summary>
        /// <remarks>
        /// This method checks if the current value is within the specified Min and Max bounds and whether the input represents a valid number. It updates the validation state and applies appropriate error styling through <see cref="CheckErrorClass"/>.
        /// The validation is performed when not in strict mode, as strict mode automatically trims values to valid ranges.
        /// </remarks>
        private void ValidateState()
        {
            IsValidState = true;
            if (Value is not null)
            {
                IsValidState = !(CompareValue(Value, Max) >= 1 || CompareValue(Value, Min) <= -1);
            }
            if (IsInValidNumber && !StrictMode)
            {
                IsValidState = false;
            }
            CheckErrorClass();
        }

        /// <summary>
        /// Updates the component's CSS classes and ARIA attributes based on the current validation state.
        /// </summary>
        /// <remarks>
        /// This method adds or removes the error CSS class from the container element and updates the aria-invalid attribute to provide proper accessibility support. When the component is in a valid state, error styling is removed; when invalid, error styling is applied.
        /// </remarks>
        private void CheckErrorClass()
        {
            ContainerClass = IsValidState
                ? ContainerClass.Replace(ERROR_CLASS, string.Empty, StringComparison.Ordinal)
                : ContainerClass.Contains(ERROR_CLASS, StringComparison.Ordinal) ? ContainerClass : ContainerClass + " " + ERROR_CLASS;
            SfBaseUtils.UpdateDictionary(ARIA_INVALID, IsValidState ? FALSE : TRUE, InputHtmlAttributes);
        }

        /// <summary>
        /// Rounds a numeric value to the specified number of decimal places based on the data type.
        /// </summary>
        /// <param name="value">The numeric value to be rounded.</param>
        /// <param name="precision">The number of decimal places to round to. If null, defaults to 0.</param>
        /// <returns>The rounded numeric value of type <typeparamref name="TValue"/>.</returns>
        /// <remarks>
        /// This method handles rounding for different numeric types:
        /// <list type="bullet">
        /// <item><description>For double types, the maximum decimal places is limited to 15</description></item>
        /// <item><description>Exponential notation values are not rounded</description></item>
        /// <item><description>Uses appropriate rounding methods based on the underlying data type (Math.Round for doubles, decimal.Round for decimals)</description></item>
        /// </list>
        /// </remarks>
        private static TValue? RoundNumber(TValue value, int? precision)
        {
            TValue? result = value;
            Type propertyType = typeof(TValue);
            int decimals = precision is null ? 0 : (int)precision;
            decimals = ((propertyType == typeof(double) || Nullable.GetUnderlyingType(propertyType) == typeof(double)) && decimals >= 15) ? 15 : decimals;     //Maximum number of decimal places allowed for a Double data type is 15.
            string? valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
            if (valueString is not null)
            {
                decimal value2 = decimal.Round(Convert.ToDecimal(result, CultureInfo.CurrentCulture), decimals);
                double value1 = Math.Round(Convert.ToDouble(result, CultureInfo.CurrentCulture), decimals);
                result = valueString.Contains('E', StringComparison.Ordinal) ? result :
                    (propertyType == typeof(double) || Nullable.GetUnderlyingType(propertyType) == typeof(double)) ? (TValue)SfBaseUtils.ChangeType(value1, propertyType, true) :
                        (TValue)SfBaseUtils.ChangeType(value2, propertyType, true);
            }
            return result;
        }

        /// <summary>
        /// Updates the visual text representation of the numeric value in the input element.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method formats the current value and updates the input element's display. If the value is null, it clears the input and removes the aria-valuenow attribute for accessibility compliance.
        /// </remarks>
        private async Task ModifyTextAsync()
        {
            if (Value is not null)
            {
                string? elemValue = FormatValueAsString(Value);
                await SetValueAsync(elemValue, FloatLabelType, ShowClearButton).ConfigureAwait(true);
            }
            else
            {

                await SetValueAsync(null, FloatLabelType, ShowClearButton).ConfigureAwait(true);
                InputHtmlAttributes.Remove(ARIA_VALUE_NOW);
            }
        }

        /// <summary>
        /// Formats the current numeric value according to the specified locale and formatting rules.
        /// </summary>
        /// <returns>A formatted string representation of the current value, or null if the value is null.</returns>
        /// <remarks>
        /// This method determines the number of decimal places to display and applies culture-specific number formatting using the Intl API. The formatting considers the component's decimal settings and locale preferences.
        /// </remarks>
        private string? FormatNumber()
        {
            int? numberOfDecimals = GetNumberOfDecimals(Value);
            string maximumFraction = (numberOfDecimals is not null) ? numberOfDecimals.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            string? formatString = GetFormatString(Value, maximumFraction);
            return Value is null ? null : Intl.GetNumericFormat(Value, formatString, Currency);
        }

        /// <summary>
        /// Validates and adjusts the minimum and maximum values based on the component's decimal settings and data type constraints.
        /// </summary>
        /// <remarks>
        /// This method performs the following operations:
        /// <list type="bullet">
        /// <item><description>Sets default min/max values if not specified</description></item>
        /// <item><description>Applies decimal precision formatting to min/max values</description></item>
        /// <item><description>Ensures minimum value doesn't exceed maximum value</description></item>
        /// <item><description>Updates ARIA attributes for accessibility</description></item>
        /// <item><description>Handles culture-specific number formatting</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="Exception">Logs any unexpected errors that occur during validation to the console.</exception>
        private void ValidateMinMax()
        {
            try
            {
                TValue? minValue = GetNumericValue<TValue>(MIN_VALUE);
                TValue? maxValue = GetNumericValue<TValue>(MAX_VALUE);
                Min = (Min is null) ? minValue : Min;
                Max = (Max is null) ? maxValue : Max;
                bool isMin = CompareValue(Min, minValue) != 0;
                bool isMax = CompareValue(Max, maxValue) != 0;
                if (Decimals is not null)
                {
                    if (isMin)
                    {
                        string? inputValue = FormattedValue(Decimals, Min);
                        if (IsNumberCulture)
                        {
                            inputValue = RemoveCultureDigits(inputValue);
                        }
                        if (inputValue is not null)
                        {
                            Min = IsDoubleValue ? ChangeType(double.Parse(inputValue, CultureInfo.CurrentCulture)) : ChangeType(decimal.Parse(inputValue, CultureInfo.CurrentCulture));
                        }
                        isMin = true;
                    }
                    if (isMax)
                    {
                        string? inputValue = FormattedValue(Decimals, Max);
                        if (IsNumberCulture)
                        {
                            inputValue = RemoveCultureDigits(inputValue);
                        }
                        if (inputValue is not null)
                        {
                            Max = IsDoubleValue ? ChangeType(double.Parse(inputValue, CultureInfo.CurrentCulture)) : ChangeType(decimal.Parse(inputValue, CultureInfo.CurrentCulture));
                        }
                        isMax = true;
                    }

                }
                Min = (CompareValue(Min, Max) >= 1) ? Max : Min;
                UpdateAriaMinMaxAttributes(isMin, isMax);
            }
            catch (FormatException ex)
            {
                _logFormattingErrorOccurred(Logger, ex);
            }
            catch (ArgumentException ex)
            {
                _logArgumentErrorOccurred(Logger, ex);
            }
            catch (OverflowException ex)
            {
                _logOverflowErrorOccurred(Logger, ex);
            }
            catch (InvalidCastException ex)
            {
                _logTypeConversionErrorInFormatValue(Logger, ex);
            }
        }

        /// <summary>
        /// Updates or removes the aria-valuemin and aria-valuemax attributes based on custom min/max configuration.
        /// </summary>
        /// <param name="isMin">Indicates whether a custom minimum value is configured.</param>
        /// <param name="isMax">Indicates whether a custom maximum value is configured.</param>
        /// <remarks>
        /// When custom min/max values are configured, corresponding ARIA attributes are added to the input element.
        /// Otherwise, existing ARIA attributes are removed to avoid incorrect accessibility metadata.
        /// </remarks>
        /// <exclude/>
        private void UpdateAriaMinMaxAttributes(bool isMin, bool isMax)
        {
            if (isMax)
            {
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIA_VALUE_MAX, Max?.ToString() ?? string.Empty, InputHtmlAttributes);
            }
            else
            {
                InputHtmlAttributes.Remove(ARIA_VALUE_MAX);
            }
            if (isMin)
            {
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIA_VALUE_MIN, Min?.ToString() ?? string.Empty, InputHtmlAttributes);
            }
            else
            {
                InputHtmlAttributes.Remove(ARIA_VALUE_MIN);
            }
        }

        /// <summary>
        /// Validates and formats the step value according to the component's decimal settings.
        /// </summary>
        /// <remarks>
        /// This method ensures that the step value conforms to the specified decimal precision. If decimal places are defined, the step value is formatted accordingly and converted back to the appropriate numeric type. Culture-specific digit formatting is also handled.
        /// </remarks>
        private void ValidateStep()
        {
            if (Decimals is not null)
            {
                string? inputValue = FormattedValue(Decimals, Step);
                if (IsNumberCulture)
                {
                    inputValue = RemoveCultureDigits(inputValue);
                }
                if (inputValue is not null)
                {
                    Step = IsDoubleValue ? ChangeType(double.Parse(inputValue, CultureInfo.CurrentCulture)) : ChangeType(decimal.Parse(inputValue, CultureInfo.CurrentCulture));
                }
            }
        }

        /// <summary>
        /// Formats a numeric value with the specified number of decimal places using culture-specific formatting.
        /// </summary>
        /// <param name="decimals">The number of decimal places to display. If null, no specific decimal formatting is applied.</param>
        /// <param name="value">The numeric value to be formatted.</param>
        /// <returns>A formatted string representation of the value, or null if an error occurs.</returns>
        /// <remarks>
        /// This method uses the Intl API to format numbers according to the current locale and currency settings. The formatting follows the "n" (number) format specifier with the specified maximum fraction digits.
        /// </remarks>
        /// <exception cref="Exception">Logs any unexpected errors that occur during formatting to the console and returns null.</exception>
        private string? FormattedValue(int? decimals, TValue value)
        {
            try
            {
                string maximumFraction = decimals is not null ? decimals.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                return Intl.GetNumericFormat<TValue>(value, "n" + maximumFraction, Currency);
            }
            catch (FormatException ex)
            {
                _logFormattingErrorOccurred(Logger, ex);
                return null;
            }
            catch (ArgumentException ex)
            {
                _logArgumentErrorOccurred(Logger, ex);
                return null;
            }
            catch (OverflowException ex)
            {
                _logOverflowErrorOccurred(Logger, ex);
                return null;
            }
        }

        /// <summary>
        /// Determines the number of decimal places in a numeric value or input text.
        /// </summary>
        /// <param name="value">The numeric value to analyze for decimal places.</param>
        /// <param name="inputTextValue">Optional string representation of the value to analyze instead of the numeric value.</param>
        /// <returns>The number of decimal places in the value, constrained by the component's Decimals property if specified.</returns>
        /// <remarks>
        /// This method analyzes the decimal portion of a number by:
        /// <list type="bullet">
        /// <item><description>Using culture-appropriate decimal separators</description></item>
        /// <item><description>Excluding exponential notation from decimal counting</description></item>
        /// <item><description>Respecting the component's maximum decimal constraint</description></item>
        /// <item><description>Handling both direct numeric values and string representations</description></item>
        /// </list>
        /// </remarks>
        private int? GetNumberOfDecimals(TValue? value, string? inputTextValue = null)
        {
            string? valueString = inputTextValue ?? Convert.ToString(value, CultureInfo.InvariantCulture);
            char decimalSeparator = inputTextValue is null ? '.' : CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            string decimalPart = valueString?.Split([decimalSeparator]).Length > 1 && !valueString.Contains('E', StringComparison.Ordinal) ? valueString.Split([decimalSeparator])[1] : string.Empty;
            int? numberOfDecimals = string.IsNullOrEmpty(decimalPart) || decimalPart.Length < 0 ? 0 : decimalPart.Length;
            if (Decimals is not null)
            {
                numberOfDecimals = numberOfDecimals < Decimals ? numberOfDecimals : Decimals;
            }

            return numberOfDecimals;
        }

        /// <summary>
        /// Initializes the client-side JavaScript functionality after the component scripts have been rendered.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous initialization operation.</returns>
        /// <remarks>
        /// This method performs the following initialization tasks:
        /// <list type="bullet">
        /// <item><description>Initializes the base TextBox JavaScript functionality</description></item>
        /// <item><description>Initializes NumericTextBox-specific JavaScript features including validation, decimal handling, and spin button events</description></item>
        /// <item><description>Sets up client-side properties for culture-specific formatting</description></item>
        /// <item><description>Detects device mode for responsive behavior</description></item>
        /// </list>
        /// </remarks>
        internal override async Task OnAfterScriptRenderedAsync()
        {
            await UpdateIsDeviceModeAsync().ConfigureAwait(true);
            await InvokeVoidAsync(_numericTextBoxJsModule, _numericTextBoxJsInProcessModule, "initialize", [DataId, ContainerElement, InputElement, DotnetObjectReference!, new NumericClientProps { Readonly = Readonly, Disabled = Disabled, Locale = CultureInfo.CurrentCulture.Name, ValidateDecimalOnType = ValidateDecimalOnType, Decimals = Decimals, DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, AllowMouseWheel = AllowMouseWheel }]).ConfigureAwait(true);
            if (SyncfusionService is not null)
            {
                IsDevice = SyncfusionService.IsDeviceMode;
            }
            IsScriptRendered = true;
            await CheckFieldsetDisabledAsync().ConfigureAwait(true);
        }

        private async Task CheckFieldsetDisabledAsync()
        {
            if (!InputElement.Equals(default))
            {
                bool disabled = await InvokeAsync<bool>(_numericTextBoxJsModule!, _numericTextBoxJsInProcessModule!, "isFieldsetDisabled", [InputElement]).ConfigureAwait(true);
                if (disabled)
                {
                    await UpdateFieldSetStatus(disabled).ConfigureAwait(true);
                }
            }
        }
        /// <summary>
        /// Handles the focus event when the numeric textbox receives focus.
        /// </summary>
        /// <param name="args">The focus event arguments containing event details.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous focus handling operation.</returns>
        /// <remarks>
        /// This method is called when the input element receives focus. It triggers client-side focus handling only if the component is enabled, not readonly, and the focus handler hasn't been triggered yet. The focus handler manages input formatting and user interaction states.
        /// </remarks>
        /// <exclude/>
        protected override async Task FocusHandlerAsync(FocusEventArgs args)
        {
            if (IsTriggerFocusHandler && !Disabled && !Readonly)
            {
                if (!IsScriptRendered)
                {
                    if (OnFocus.HasDelegate)
                    {
                        NumericFocusEventArgs<TValue> eventArgs = new()
                        {
                            Event = args,
                            Value = Value,
                            Name = "Focus",
                        };
                        await OnFocus.InvokeAsync(eventArgs).ConfigureAwait(true);
                    }
                    if (!Disabled && !Readonly)
                    {
                        IsFocus = true;
                        if (ValueExpression is not null)
                        {
                            UpdateValidateClass();
                        }
                        else
                        {
                            ContainerClass = ContainerClass.Replace(ERROR_CLASS, string.Empty, StringComparison.Ordinal);
                        }

                        PrevValue = Value;
                        if (Value is not null)
                        {
                            string? formatNumbers = FormatNumber();
                            if (IsIgnoreDecimal())
                            {
                                formatNumbers = SfBaseUtils.RemoveClass(formatNumbers, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                            }

                            string formatValue = SfBaseUtils.RemoveClass(formatNumbers, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);
                            await SetValueAsync(formatValue, FloatLabelType, ShowClearButton).ConfigureAwait(true);

                            if (!IsPrevFocused && IsRendered)
                            {
                                _selectRangeDotNetRef ??= DotNetObjectReference.Create(this);
                                await InvokeVoidAsync(_numericTextBoxJsModule, _numericTextBoxJsInProcessModule, "selectRange", [DataId, FormatValueAsString(InputTextValue) ?? string.Empty, _selectRangeDotNetRef]).ConfigureAwait(true);
                            }
                        }
                    }
                    if (FloatLabelType == FloatLabelType.Auto)
                    {
                        await InvokeVoidAsync(_numericTextBoxJsModule, _numericTextBoxJsInProcessModule, "removeFloatLabelSize", [DataId]).ConfigureAwait(true);

                    }
                }
                else
                {
                    await InvokeVoidAsync(_numericTextBoxJsModule, _numericTextBoxJsInProcessModule, "focusHandler", [DataId]).ConfigureAwait(true);
                }
                IsTriggerFocusHandler = false;
            }
        }

        /// <summary>
        /// Handles the blur event when the numeric textbox loses focus.
        /// </summary>
        /// <param name="args">The focus event arguments containing event details.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous blur handling operation.</returns>
        /// <remarks>
        /// This method is called when the input element loses focus and performs the following operations:
        /// <list type="bullet">
        /// <item><description>Triggers the Blur event if subscribed</description></item>
        /// <item><description>Validates and formats the current input value</description></item>
        /// <item><description>Handles device-specific behavior for mobile platforms</description></item>
        /// <item><description>Updates float label sizing</description></item>
        /// <item><description>Parses and converts input text to appropriate numeric types</description></item>
        /// </list>
        /// The method prevents recursive calls and ensures proper value conversion based on the data type (double or decimal).
        /// </remarks>
        /// <exclude/>
        protected override async Task FocusOutHandlerAsync(FocusEventArgs args)
        {
            if (OnBlur.HasDelegate && !IsClearIconClick)
            {
                NumericBlurEventArgs<TValue> eventArgs = new()
                {
                    Event = args,
                    Value = Value,
                    Name = BLUR
                };
                await OnBlur.InvokeAsync(eventArgs).ConfigureAwait(true);
            }
            if (!Disabled && !Readonly)
            {
                if (IsPrevFocused)
                {
                    if (IsDevice)
                    {
                        string? value = FocusInputValue;
                        await FocusAsync().ConfigureAwait(true);
                        IsPrevFocused = false;
                        await Task.Delay(ClearButtonDelayMs, _delayCancellationTokenSource?.Token ?? CancellationToken.None).ConfigureAwait(true);    // Remove the clear button on time delay in component.
                        if (value is not null)
                        {
                            await SetValueAsync(value, FloatLabelType, ShowClearButton).ConfigureAwait(true);
                        }
                    }
                }
                else
                {
                    IsFocus = false;
                    string? inputValue = FocusInputValue;
                    if (string.IsNullOrEmpty(inputValue))
                    {
                        InputTextValue = default;
                    }
                    if (IsNumberCulture)
                    {
                        inputValue = RemoveCultureDigits(inputValue);
                    }
                    if (IsDoubleValue)
                    {
                        TValue? changedValue = !string.IsNullOrEmpty(inputValue) && double.TryParse(inputValue, NumberStyles.Any, CultureInfo.CurrentCulture, out double doubleValue)
                            ? ChangeType(doubleValue)
                            : default;
                        await UpdateValueAsync(changedValue, args).ConfigureAwait(true);
                    }
                    else
                    {
                        TValue? changedValue = !string.IsNullOrEmpty(inputValue) && decimal.TryParse(inputValue, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal decimalValue)
                            ? ChangeType(decimalValue)
                            : default;
                        await UpdateValueAsync(changedValue, args).ConfigureAwait(true);
                    }
                }
                if (string.IsNullOrEmpty(FocusInputValue))
                {
                    await InvokeVoidAsync(_numericTextBoxJsModule, _numericTextBoxJsInProcessModule, "clearInvalid", [DataId, FormatValueAsString(InputTextValue) ?? string.Empty]).ConfigureAwait(true);

                }
                IsInValidNumber = false;
            }
            if (FloatLabelType is FloatLabelType.Auto or FloatLabelType.Never)
            {
                await InvokeVoidAsync(_numericTextBoxJsModule, _numericTextBoxJsInProcessModule, "updateFloatLabelSize", [DataId]).ConfigureAwait(true);

            }
        }

        /// <summary>
        /// Handles the internal focus logic and prepares the input for user interaction.
        /// </summary>
        /// <returns>A dictionary containing focus state information including render status and formatted value.</returns>
        /// <remarks>
        /// This method is called internally when the input receives focus and performs the following operations:
        /// <list type="bullet">
        /// <item><description>Triggers the Focus event if subscribed</description></item>
        /// <item><description>Updates validation classes if using form validation</description></item>
        /// <item><description>Formats the value for editing mode (removes group separators, handles decimals)</description></item>
        /// <item><description>Updates float label behavior</description></item>
        /// <item><description>Returns focus state information for client-side processing</description></item>
        /// </list>
        /// </remarks>
        private async Task<Dictionary<string, object>> FocusInputAsync()
        {
            if (OnFocus.HasDelegate && !IsClearIconClick)
            {
                NumericFocusEventArgs<TValue> eventArgs = new()
                {
                    Event = new FocusEventArgs { Type = "focus" },
                    Value = Value,
                    Name = "Focus",
                };
                await OnFocus.InvokeAsync(eventArgs).ConfigureAwait(true);
            }
            IsClearIconClick = false;
            Dictionary<string, object> returnValue = new() { { "isRendered", false }, { "formatValue", string.Empty } };
            if (!Disabled && !Readonly)
            {
                IsFocus = true;
                if (ValueExpression is not null)
                {
                    UpdateValidateClass();
                }
                else
                {
                    ContainerClass = ContainerClass.Replace(ERROR_CLASS, string.Empty, StringComparison.Ordinal);
                }
                PrevValue = Value;
                if (Value is not null)
                {
                    string? formatNumbers = FormatNumber();
                    if (IsIgnoreDecimal())
                    {
                        formatNumbers = SfBaseUtils.RemoveClass(formatNumbers, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    }

                    string formatValue = SfBaseUtils.RemoveClass(formatNumbers, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);
                    await SetValueAsync(formatValue, FloatLabelType, ShowClearButton).ConfigureAwait(false);
                    if (!IsPrevFocused && IsRendered)
                    {
                        returnValue["isRendered"] = IsRendered;
                        returnValue["formatValue"] = FormatValueAsString(InputTextValue) ?? string.Empty;
                    }
                }
            }
            if (FloatLabelType == FloatLabelType.Auto)
            {
                await InvokeVoidAsync(_numericTextBoxJsModule, _numericTextBoxJsInProcessModule, "removeFloatLabelSize", [DataId]).ConfigureAwait(true);
            }

            return returnValue;
        }

        /// <summary>
        /// Converts culture-specific digit characters to standard ASCII digits (0-9).
        /// </summary>
        /// <param name="inputValue">The input string that may contain culture-specific digits.</param>
        /// <returns>A string with all culture-specific digits converted to standard ASCII digits.</returns>
        /// <remarks>
        /// This method is essential for supporting right-to-left (RTL) cultures and cultures that use alternative digit representations such as Arabic, Persian, or Thai numerals. It ensures that numeric input can be properly parsed regardless of the input culture by converting all digit characters to their ASCII equivalents while preserving non-digit characters like decimal separators and negative signs.
        /// </remarks>
        private static string RemoveCultureDigits(string? inputValue)
        {
            string outValue = string.Empty;
            if (!string.IsNullOrEmpty(inputValue))
            {
                for (int index = 0; index < inputValue.Length; index++)
                {
                    outValue += char.IsDigit(inputValue[index]) ? Convert.ToString(char.GetNumericValue(inputValue, index), CultureInfo.InvariantCulture) : inputValue[index].ToString();
                }
            }

            return outValue;
        }

        /// <summary>
        /// Updates decimal-related properties for integer-based numeric types that should not display decimal places.
        /// </summary>
        /// <remarks>
        /// This method is called for integer-based types (int, byte, long, short) and their nullable counterparts to:
        /// <list type="bullet">
        /// <item><description>Set Decimals to 0 if not already specified</description></item>
        /// <item><description>Enable decimal validation on typing to prevent decimal input</description></item>
        /// <item><description>Update the format string from "n2" to "n0" to hide decimal places</description></item>
        /// </list>
        /// This ensures that integer types behave appropriately by not allowing or displaying decimal values.
        /// </remarks>
        private void UpdateDecimalType()
        {
            if (IsIgnoreDecimal())
            {
                Decimals = PreviousDecimals = Decimals is null ? 0 : Decimals;
                ValidateDecimalOnType = PreviousValidateDecimalOnType = true;
                Format = Format == "n2" ? "n0" : Format;
            }
        }

        /// <summary>
        /// Determines whether decimal input should be ignored based on the component's generic type parameter.
        /// </summary>
        /// <returns><c>true</c> if the type is an integer-based type that should not support decimal input; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method checks if the generic type <typeparamref name="TValue"/> is one of the following integer types or their nullable counterparts:
        /// <list type="bullet">
        /// <item><description><c>int</c> and <c>int?</c></description></item>
        /// <item><description><c>byte</c> and <c>byte?</c></description></item>
        /// <item><description><c>long</c> and <c>long?</c></description></item>
        /// <item><description><c>short</c> and <c>short?</c></description></item>
        /// </list>
        /// When this method returns <c>true</c>, the component will automatically disable decimal input and formatting to maintain integer-only behavior.
        /// </remarks>
        private static bool IsIgnoreDecimal()
        {
            Type type = typeof(TValue);
            return type == typeof(int) || Nullable.GetUnderlyingType(type) == typeof(int) || Nullable.GetUnderlyingType(type) == typeof(byte) || type == typeof(byte) || type == typeof(long) || Nullable.GetUnderlyingType(type) == typeof(long) || type == typeof(short) || Nullable.GetUnderlyingType(type) == typeof(short);
        }

        /// <summary>
        /// Updates the component's value with proper rounding, validation, and change event handling.
        /// </summary>
        /// <param name="value">The new value to be set. Can be null for clearing the input.</param>
        /// <param name="args">Optional event arguments to be passed to the change event.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous update operation.</returns>
        /// <remarks>
        /// This method performs a complete value update cycle:
        /// <list type="bullet">
        /// <item><description>Rounds the value to the specified decimal places if applicable</description></item>
        /// <item><description>Applies strict mode constraints (min/max trimming) if enabled</description></item>
        /// <item><description>Updates the internal value and visual representation</description></item>
        /// <item><description>Triggers change events for subscribers</description></item>
        /// </list>
        /// </remarks>
        private async Task UpdateValueAsync(TValue? value, EventArgs? args = null)
        {
            if (value is not null)
            {
                if (Decimals is not null)
                {
                    value = RoundNumber(value, Decimals);
                }
            }

            await ChangeValueAsync((value is null) ? default : StrictMode ? TrimValue(value) : value).ConfigureAwait(true);
            await RaiseChangeEventAsync(args).ConfigureAwait(true);
        }

        /// <summary>
        /// Handles the change event when the input value is modified by the user.
        /// </summary>
        /// <param name="args">The change event arguments containing the new input value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous change handling operation.</returns>
        /// <remarks>
        /// This method is responsible for processing user input and performs the following operations:
        /// <list type="bullet">
        /// <item><description>Handles drag-and-drop input sanitization</description></item>
        /// <item><description>Processes full-width character conversion</description></item>
        /// <item><description>Validates numeric input against allowed patterns</description></item>
        /// <item><description>Handles both double and decimal parsing based on data type</description></item>
        /// <item><description>Applies strict mode constraints and formatting</description></item>
        /// <item><description>Manages paste operations and invalid input states</description></item>
        /// </list>
        /// The method supports culture-specific number formatting and ensures proper validation of numeric input.
        /// </remarks>
        /// <exclude/>
        protected override async Task ChangeHandlerAsync(ChangeEventArgs? args)
        {
            string? formattedVal = args is not null ? args.Value as string : null;
            if (IsDropValue)
            {
                string dropDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                string negativeSign = CultureInfo.CurrentCulture.NumberFormat.NegativeSign;
                bool hasNegative = false;
                string tempVal = formattedVal ?? string.Empty;
                if (!string.IsNullOrEmpty(tempVal))
                {
                    if (tempVal.StartsWith(negativeSign, StringComparison.Ordinal))
                    {
                        hasNegative = true;
                        tempVal = tempVal.Substring(tempVal.StartsWith(negativeSign, StringComparison.Ordinal) ? 1 : negativeSign.Length);
                    }
                }
                string pattern = $"[^0-9{Regex.Escape(dropDecimalSeparator)}]";
                tempVal = Regex.Replace(tempVal, pattern, "");
                formattedVal = (hasNegative ? negativeSign : "") + tempVal;
                IsDropValue = !IsDropValue;
            }
            if (string.IsNullOrEmpty(formattedVal))
            {
                formattedVal = default;
                InputTextValue = default;
            }
            FocusInputValue = !IsClearButtonClicked ? formattedVal : string.Empty;
            NumberFormatInfo numberFormat = CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormat.NumberDecimalSeparator;
            string thousandSeparator = numberFormat.NumberGroupSeparator;
            string negativeSignStr = numberFormat.NegativeSign;
            if (!string.IsNullOrEmpty(formattedVal) && !string.IsNullOrEmpty(FocusInputValue))
            {
                StringBuilder sb = new();
                bool isFirstChar = true;
                bool hasNegativeProcessed = false;
                foreach (char c in formattedVal)
                {
                    if (c is >= '０' and <= '９')
                    {
                        sb.Append((char)(c - '０' + '0'));
                    }
                    else if ((c == '-' || (negativeSignStr.Length == 1 && c.ToString() == negativeSignStr)) && isFirstChar && !hasNegativeProcessed)
                    {
                        sb.Append(negativeSignStr);
                        hasNegativeProcessed = true;
                    }
                    else if (char.IsDigit(c))
                    {
                        sb.Append(c);
                    }
                    else if (c.ToString() == decimalSeparator)
                    {
                        sb.Append(decimalSeparator);
                    }
                    else if (c.ToString() == thousandSeparator)
                    {
                        StringBuilder stringBuilder = sb.Append(thousandSeparator);
                    }
                    isFirstChar = false;
                }
                FocusInputValue = sb.ToString();
                formattedVal = FocusInputValue;
            }
            if (IsDoubleValue)
            {
                if (!double.TryParse(formattedVal, NumberStyles.Any, CultureInfo.CurrentCulture, out _))
                {
                    await RaiseChangeEventAsync(args).ConfigureAwait(true);
                    IsInValidNumber = !string.IsNullOrEmpty(formattedVal);
                    return;
                }
                TValue? inputValue = string.IsNullOrEmpty(formattedVal) ? default : ChangeType(double.Parse(formattedVal, CultureInfo.CurrentCulture));
                int? numberOfDecimals = GetNumberOfDecimals(inputValue);
                string maximumFraction = (numberOfDecimals is not null) ? numberOfDecimals.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                TValue? changedValue = string.IsNullOrEmpty(formattedVal) ? default : ChangeType(double.Parse(formattedVal, CultureInfo.CurrentCulture));
                string? formatString = GetFormatString(changedValue, maximumFraction);
                string formattedValue = Intl.GetNumericFormat(changedValue, formatString, Currency);
                string textValue = IsNumberCulture ? RemoveCultureDigits(formattedValue) : formattedValue;
                TValue? parseInput = changedValue is null ? default : ChangeType(double.Parse(textValue, CultureInfo.CurrentCulture));
                TValue? validateValue = (parseInput is null) ? default : StrictMode ? TrimValue(parseInput) : parseInput;
                if (StrictMode && SfBaseUtils.Equals(InputTextValue, validateValue) && !IsPasteValue)
                {
                    MinMaxValue = formattedVal;
                    CurrentValueAsString = formattedVal;
                    InputTextValue = parseInput;
                    await Task.Delay(StrictModeDelayMs, _delayCancellationTokenSource?.Token ?? CancellationToken.None).ConfigureAwait(true);
                }
                MinMaxValue = null;
                IsPasteValue = false;
                IsInValidNumber = false;
                await UpdateValueAsync(validateValue, args).ConfigureAwait(true);
            }
            else
            {
                if (!decimal.TryParse(formattedVal, NumberStyles.Any, CultureInfo.CurrentCulture, out _))
                {
                    await RaiseChangeEventAsync(args).ConfigureAwait(true);
                    IsInValidNumber = !string.IsNullOrEmpty(formattedVal);
                    return;
                }
                TValue? inputValue = string.IsNullOrEmpty(formattedVal) ? default : ChangeType(decimal.Parse(formattedVal, CultureInfo.CurrentCulture));
                int? numberOfDecimals = GetNumberOfDecimals(inputValue);
                string maximumFraction = (numberOfDecimals is not null) ? numberOfDecimals.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                TValue? changedValue = string.IsNullOrEmpty(formattedVal) ? default : ChangeType(decimal.Parse(formattedVal, CultureInfo.CurrentCulture));
                string? formatString = GetFormatString(changedValue, maximumFraction);
                string formattedValue = Intl.GetNumericFormat(changedValue, formatString, Currency);
                string textValue = IsNumberCulture ? RemoveCultureDigits(formattedValue) : formattedValue;
                TValue? parseInput = changedValue is null ? default : ChangeType(decimal.Parse(textValue, CultureInfo.CurrentCulture));
                TValue? validateValue = (parseInput is null) ? default : StrictMode ? TrimValue(parseInput) : parseInput;
                if (StrictMode && SfBaseUtils.Equals(InputTextValue, validateValue) && !IsPasteValue)
                {
                    MinMaxValue = formattedVal;
                    CurrentValueAsString = formattedVal;
                    InputTextValue = parseInput;
                    await Task.Delay(StrictModeDelayMs, _delayCancellationTokenSource?.Token ?? CancellationToken.None).ConfigureAwait(true);
                }
                MinMaxValue = null;
                IsPasteValue = false;
                IsInValidNumber = false;
                await UpdateValueAsync(validateValue, args).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles mouse down events on spinner buttons to manage focus state during spinner interactions.
        /// </summary>
        /// <remarks>
        /// This method is called when the user presses down on a spinner button (up or down arrow). It preserves the focus state and prevents certain mouse events from interfering with the spinner operation. This ensures smooth user experience during value increment/decrement operations.
        /// </remarks>
        private void MouseDownOnSpinner()
        {
            if (IsFocus)
            {
                IsPrevFocused = true;
                MouseDownSpinnerPrevent = true;
            }
        }

        /// <summary>
        /// Handles mouse up events on spinner buttons to restore focus state and trigger change events.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous mouse up handling operation.</returns>
        /// <remarks>
        /// This method is called when the user releases the mouse button after clicking a spinner button. It performs the following operations:
        /// <list type="bullet">
        /// <item><description>Restores the previous focus state (except on mobile devices)</description></item>
        /// <item><description>Manages mouse event prevention flags</description></item>
        /// <item><description>Triggers the OnChange event if there are subscribers</description></item>
        /// </list>
        /// This ensures proper completion of spinner button interactions and event notification.
        /// </remarks>
        private async Task MouseUpOnSpinnerAsync()
        {
            if (IsPrevFocused)
            {
                if (!IsDevice)
                {
                    IsPrevFocused = false;
                }
            }

            MouseDownSpinnerPrevent = true;
            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(new ChangeEventArgs() { Value = Value }).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Performs increment or decrement actions on the numeric value using the specified step value.
        /// </summary>
        /// <param name="action">The action to perform - typically "increment" or "decrement".</param>
        /// <param name="args">Optional event arguments to be passed to change events.</param>
        /// <param name="currentInputValue">Optional current input value to use instead of the component's current value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous action operation.</returns>
        /// <remarks>
        /// This method is used by spin buttons and keyboard shortcuts to modify the numeric value:
        /// <list type="bullet">
        /// <item><description>Parses the current input value or uses the component's current value</description></item>
        /// <item><description>Handles culture-specific digit conversion</description></item>
        /// <item><description>Performs the mathematical operation (addition or subtraction) with the step value</description></item>
        /// <item><description>Applies proper formatting and validation</description></item>
        /// <item><description>Triggers change events and updates the UI state</description></item>
        /// </list>
        /// </remarks>
        private async Task ActionAsync(string action, EventArgs? args = null, string? currentInputValue = null)
        {
            TValue? value;
            if (IsFocus)
            {
                string? inputTextValue = !string.IsNullOrEmpty(currentInputValue) ? currentInputValue : FormatValueAsString(InputTextValue);
                inputTextValue = IsNumberCulture ? RemoveCultureDigits(inputTextValue) : inputTextValue;
                TValue? inputValue;
                if (string.IsNullOrEmpty(inputTextValue) || Regex.IsMatch(inputTextValue, @"^-$"))
                {
                    inputValue = default;
                }
                else
                {
                    if (IsDoubleValue)
                    {
                        if (double.TryParse(inputTextValue, NumberStyles.Any, CultureInfo.CurrentCulture, out double doubleResult))
                        {
                            inputValue = ChangeType(doubleResult);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (decimal.TryParse(inputTextValue, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal decimalResult))
                        {
                            inputValue = ChangeType(decimalResult);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                int? numberOfDecimals = GetNumberOfDecimals(Value);
                string maximumFraction = (numberOfDecimals is not null) ? numberOfDecimals.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
                inputTextValue = (inputValue is null) ? inputTextValue : Intl.GetNumericFormat(inputValue, GetFormatString(inputValue, maximumFraction), Currency);
                inputTextValue = IsNumberCulture ? RemoveCultureDigits(inputTextValue) : inputTextValue;
                if (IsIgnoreDecimal())
                {
                    inputTextValue = SfBaseUtils.RemoveClass(inputTextValue, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                }
                inputTextValue ??= string.Empty;
                value = (inputValue is null) ? default : IsDoubleValue ? ChangeType(double.Parse(inputTextValue, CultureInfo.CurrentCulture)) : ChangeType(decimal.Parse(inputTextValue, CultureInfo.CurrentCulture));
            }
            else
            {
                value = Value;
            }

            await ChangeValueAsync(PerformAction(value, Step, action)).ConfigureAwait(true);
            await RaiseChangeEventAsync(args).ConfigureAwait(true);
            UpdateValidateClass();
        }

        /// <summary>
        /// Generates the appropriate format string for number formatting based on the component's current state and value characteristics.
        /// </summary>
        /// <param name="numericValue">The numeric value to be formatted.</param>
        /// <param name="maximumFraction">The maximum number of fraction digits to display.</param>
        /// <returns>A format string suitable for use with Intl number formatting, or null if no specific format is needed.</returns>
        /// <remarks>
        /// This method determines the appropriate format string by considering:
        /// <list type="bullet">
        /// <item><description>Whether the value is in exponential notation</description></item>
        /// <item><description>The component's focus state</description></item>
        /// <item><description>The current Format property setting</description></item>
        /// <item><description>The calculated maximum fraction digits from the value</description></item>
        /// </list>
        /// For exponential values, the original format is preserved. Otherwise, a number format with specific fraction digits is used.
        /// </remarks>
        private string? GetFormatString(TValue? numericValue, string maximumFraction)
        {
            bool isExponential = (Convert.ToString(numericValue, CultureInfo.CurrentCulture) ?? string.Empty).Contains('E', StringComparison.Ordinal);
            bool isNumberFormat = Format is not null && Format.ToLower(CultureInfo.CurrentCulture).Contains('n', StringComparison.Ordinal);
            string? formatValue = (IsFocus && !isNumberFormat) ? (Decimals is not null ? "n" + Decimals : "n") : Format;
            return isExponential ? formatValue : "n" + maximumFraction;
        }

        /// <summary>
        /// Raises the ValueChange event when the component's value has changed.
        /// </summary>
        /// <param name="args">Optional event arguments to indicate the source of the change.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous event handling operation.</returns>
        /// <remarks>
        /// This method is called whenever the component's value changes and performs the following operations:
        /// <list type="bullet">
        /// <item><description>Compares the current value with the previous value to detect actual changes</description></item>
        /// <item><description>Saves the value to local storage if persistence is enabled</description></item>
        /// <item><description>Creates and invokes the ValueChange event with detailed event arguments</description></item>
        /// <item><description>Updates validation CSS classes based on the new value</description></item>
        /// <item><description>Only triggers if the component is enabled and not readonly</description></item>
        /// </list>
        /// </remarks>
        private async Task RaiseChangeEventAsync(EventArgs? args = null)
        {
            if (CompareValue(PrevValue, InputTextValue) != 0)
            {
                if (EnablePersistence)
                {
                    await SetLocalStorageAsync(ID, Value).ConfigureAwait(true);
                }
                TValue? previousValue = PrevValue;
                PrevValue = InputTextValue;
                if (ValueChange.HasDelegate && !(Disabled || Readonly))
                {
                    ChangeEventArgs<TValue> eventArgs = new()
                    {
                        Value = InputTextValue,
                        PreviousValue = previousValue,
                        Event = args,
                        IsInteracted = args is not null,
                        Name = VALUECHANGE
                    };
                    await ValueChange.InvokeAsync(eventArgs).ConfigureAwait(true);
                }
                UpdateValidateClass();
            }
        }

        /// <summary>
        /// Stores the component's current value in the browser's local storage for persistence across sessions.
        /// </summary>
        /// <param name="persistId">The unique identifier used as the key in local storage.</param>
        /// <param name="dataValue">The value to be stored in local storage.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous storage operation.</returns>
        /// <remarks>
        /// This method is used when <c>EnablePersistence</c> is true to maintain the component's value across browser sessions. The value is stored using the component's ID as the key and can be retrieved on subsequent page loads.
        /// </remarks>
        private async Task SetLocalStorageAsync(string persistId, TValue? dataValue)
        {
            if (dataValue is null)
            {
                await InvokeVoidAsync(_baseJsModule, _baseJsInProcessModule, "setLocalStorageItem", [persistId, ""]).ConfigureAwait(true);
            }
            else
            {
                await InvokeVoidAsync(_baseJsModule, _baseJsInProcessModule, "setLocalStorageItem", [persistId, dataValue]).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Converts an object value to the component's generic type <typeparamref name="TValue"/>.
        /// </summary>
        /// <param name="value">The object value to be converted.</param>
        /// <returns>The converted value of type <typeparamref name="TValue"/>, or the maximum value if conversion fails.</returns>
        /// <remarks>
        /// This method provides type-safe conversion of numeric values to the component's generic type. If the conversion fails for any reason, it returns the maximum allowed value as a fallback to prevent runtime errors.
        /// </remarks>
        private TValue ChangeType(object value)
        {
            try
            {
                object? converted = SfBaseUtils.ChangeType(value, typeof(TValue), true);
                return converted is null ? (Max ?? default!) : (TValue)converted;
            }
            catch (InvalidCastException)
            {
                return Max ?? default!;
            }
            catch (FormatException)
            {
                return Max ?? default!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Performs arithmetic operations (increment/decrement) on the numeric value with proper rounding and constraint handling.
        /// </summary>
        /// <param name="value">The base value to perform the operation on. If null, defaults to zero.</param>
        /// <param name="step">The step value to add or subtract.</param>
        /// <param name="operation">The operation to perform - "increment" for addition, otherwise subtraction.</param>
        /// <returns>The result of the arithmetic operation, optionally constrained by min/max bounds in strict mode.</returns>
        /// <remarks>
        /// This method handles the core arithmetic logic for spin button operations:
        /// <list type="bullet">
        /// <item><description>Defaults null values to zero before calculation</description></item>
        /// <item><description>Performs addition or subtraction based on the operation parameter</description></item>
        /// <item><description>Applies rounding correction to handle floating-point precision issues</description></item>
        /// <item><description>Enforces min/max constraints when in strict mode</description></item>
        /// </list>
        /// </remarks>
        private TValue PerformAction(TValue? value, TValue step, string operation)
        {
            value ??= ChangeType("0");
            TValue updatedValue = (operation == INCREMENT_CONTENT) ? NumberOperate(value, step, ADD) : NumberOperate(value, step, SUB);
            updatedValue = CorrectRounding(value, step, updatedValue);
            return StrictMode ? TrimValue(updatedValue) : updatedValue;
        }

        /// <summary>
        /// Processes and sanitizes pasted input to ensure only valid numeric characters are retained.
        /// </summary>
        /// <param name="inputValue">The pasted input value to be processed.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous paste processing operation.</returns>
        /// <remarks>
        /// This method is called when content is pasted into the numeric textbox and performs the following sanitization:
        /// <list type="bullet">
        /// <item><description>Removes all non-numeric characters except digits, decimal separator, and negative sign</description></item>
        /// <item><description>Uses culture-specific decimal separator patterns</description></item>
        /// <item><description>Updates the input element with the sanitized value</description></item>
        /// </list>
        /// This ensures that pasted content conforms to valid numeric input formats.
        /// </remarks>
        private async Task UpdatePasteInputAsync(string inputValue)
        {
            string inputTextValue = inputValue;
            string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string pattern = $"[^-0-9{Regex.Escape(decimalSeparator)}]";
            inputTextValue = Regex.Replace(inputTextValue, pattern, "");
            await SetValueAsync(inputTextValue, FloatLabelType, ShowClearButton).ConfigureAwait(false);
        }

        /// <summary>
        /// Corrects floating-point precision errors that can occur during arithmetic operations by applying appropriate rounding.
        /// </summary>
        /// <param name="value">The original value before the operation.</param>
        /// <param name="step">The step value used in the operation.</param>
        /// <param name="result">The result of the arithmetic operation that may need rounding correction.</param>
        /// <returns>The corrected result with proper rounding applied, or the original result if no correction is needed.</returns>
        /// <remarks>
        /// This method addresses floating-point precision issues that can occur when performing arithmetic operations on decimal numbers:
        /// <list type="bullet">
        /// <item><description>Analyzes both the original value and step for decimal digit count</description></item>
        /// <item><description>Determines the maximum precision needed based on both operands</description></item>
        /// <item><description>Applies rounding correction only for non-exponential decimal numbers</description></item>
        /// <item><description>Preserves the original result for exponential notation or integer-only operations</description></item>
        /// </list>
        /// </remarks>
        private TValue CorrectRounding(TValue value, TValue step, TValue result)
        {
            string? valueText = Convert.ToString(value, CultureInfo.InvariantCulture);
            string? stepText = Convert.ToString(step, CultureInfo.InvariantCulture);
            string valueTextNonNull = valueText ?? string.Empty;
            string stepTextNonNull = stepText ?? string.Empty;
            bool floatValue = DecimalPartRegex().IsMatch(valueTextNonNull);
            bool floatStep = DecimalPartRegex().IsMatch(stepTextNonNull);
            bool isExponential = ExponentialRegex().IsMatch(valueTextNonNull);
            if ((floatValue || floatStep) && !isExponential)
            {
                int valueCount = floatValue ? DecimalPartRegex().Matches(valueTextNonNull)[0].Groups[1].Length : 0;
                int stepCount = floatStep ? DecimalPartRegex().Matches(stepTextNonNull)[0].Groups[1].Length : 0;
                int max = Math.Max(valueCount, stepCount);
                value = RoundValue(result, max);
                return value;
            }

            return result;
        }

        /// <summary>
        /// Rounds a numeric value to the specified precision using multiplication and division to avoid floating-point precision errors.
        /// </summary>
        /// <param name="result">The numeric value to be rounded.</param>
        /// <param name="precision">The number of decimal places to round to. If null, defaults to 0.</param>
        /// <returns>The rounded numeric value of type <typeparamref name="TValue"/>.</returns>
        /// <remarks>
        /// This method uses a multiply-round-divide approach to ensure accurate rounding:
        /// <list type="bullet">
        /// <item><description>Multiplies the value by 10^precision to shift decimal places</description></item>
        /// <item><description>Rounds the shifted value to eliminate precision errors</description></item>
        /// <item><description>Divides back by 10^precision to restore the original scale</description></item>
        /// </list>
        /// This technique provides more reliable rounding than direct decimal rounding for certain floating-point scenarios.
        /// </remarks>
        private TValue RoundValue(TValue result, double? precision)
        {
            TValue roundVal = result;
            precision = precision is null ? 0 : precision;
            double divide = Math.Pow(10, (double)precision);
            roundVal = MultipleValue(roundVal, divide);
            roundVal = DivideValue(roundVal, divide);
            return roundVal;
        }

        /// <summary>
        /// Multiplies a numeric value by a double precision divisor value.
        /// </summary>
        /// <param name="value">The base numeric value to multiply.</param>
        /// <param name="divide">The double precision multiplier value.</param>
        /// <returns>The result of the multiplication operation as type <typeparamref name="TValue"/>.</returns>
        /// <remarks>
        /// This method is part of the precision rounding process and uses dynamic typing to handle different numeric types generically.
        /// </remarks>
        private TValue MultipleValue(TValue value, double divide)
        {
            dynamic? result = value;
            dynamic? devideVal = ChangeType(divide);
            return (TValue)(result * devideVal);
        }

        /// <summary>
        /// Divides a numeric value by a double precision divisor after rounding the numerator.
        /// </summary>
        /// <param name="value">The numeric value to be divided (will be rounded first).</param>
        /// <param name="divide">The double precision divisor value.</param>
        /// <returns>The result of the division operation as type <typeparamref name="TValue"/>.</returns>
        /// <remarks>
        /// This method completes the precision rounding process by rounding the multiplied value and then dividing it back to the original scale. The rounding step eliminates floating-point precision artifacts.
        /// </remarks>
        private TValue DivideValue(TValue value, double divide)
        {
            dynamic? result = value;
            dynamic? devideVal = ChangeType(divide);
            return (TValue)(Math.Round(result) / devideVal);
        }

        /// <summary>
        /// Performs basic arithmetic operations (addition or subtraction) on two numeric values with overflow protection for byte types.
        /// </summary>
        /// <param name="value1">The first operand in the arithmetic operation.</param>
        /// <param name="value2">The second operand in the arithmetic operation.</param>
        /// <param name="action">The operation to perform - "add" for addition, otherwise subtraction.</param>
        /// <returns>The result of the arithmetic operation as type <typeparamref name="TValue"/>, with byte overflow protection applied if applicable.</returns>
        /// <remarks>
        /// This method provides generic arithmetic operations with special handling for byte types:
        /// <list type="bullet">
        /// <item><description>Uses dynamic typing to handle different numeric types generically</description></item>
        /// <item><description>Performs addition when action equals "add" (case-insensitive), otherwise subtraction</description></item>
        /// <item><description>For byte and nullable byte types, constrains results within byte.MinValue and byte.MaxValue to prevent overflow</description></item>
        /// <item><description>Returns the raw arithmetic result for all other numeric types</description></item>
        /// </list>
        /// </remarks>
        private static TValue NumberOperate(TValue value1, TValue value2, string action)
        {
            dynamic? changeVal1 = value1;
            dynamic? changeVal2 = value2;
            dynamic numberValue = action.Equals(ADD, StringComparison.OrdinalIgnoreCase) ? (changeVal1 + changeVal2) : (changeVal1 - changeVal2);
            Type type = typeof(TValue);
            if (Nullable.GetUnderlyingType(type) == typeof(byte) || type == typeof(byte))
            {
                dynamic byteValue = byte.MinValue > numberValue ? byte.MinValue : byte.MaxValue < numberValue ? byte.MaxValue : numberValue;
                return (TValue)byteValue;
            }
            return (TValue)numberValue;
        }

        /// <summary>
        /// Handles touch events for the clear button functionality.
        /// </summary>
        /// <param name="args">The event arguments associated with the touch event.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous touch event handling operation.</returns>
        /// <remarks>
        /// This method provides touch-specific handling for the clear button, ensuring proper behavior on touch-enabled devices by delegating to the common clear button event logic.
        /// </remarks>
        internal async Task BindClearBtnTouchEventsAsync(EventArgs args)
        {
            await InvokeClearBtnEventAsync(args).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates the component's validation CSS classes based on the current form validation state.
        /// </summary>
        /// <remarks>
        /// This method integrates with Blazor's form validation system to provide visual feedback about the component's validation state:
        /// <list type="bullet">
        /// <item><description>Creates a field identifier from the ValueExpression to track validation state</description></item>
        /// <item><description>Removes previously applied validation classes to avoid accumulation</description></item>
        /// <item><description>Applies new validation classes based on the current EditContext state</description></item>
        /// <item><description>Maps validation states to appropriate visual indicators:</description>
        ///   <list type="bullet">
        ///   <item><description>"invalid" or "modified invalid" → Error styling</description></item>
        ///   <item><description>"modified valid" → Success styling</description></item>
        ///   <item><description>"valid" → Neutral styling (removes error/success)</description></item>
        ///   </list>
        /// </item>
        /// <item><description>Normalizes whitespace in the final class string</description></item>
        /// </list>
        /// This method only operates when the component is used within a form with validation expressions.
        /// </remarks>
        private void UpdateValidateClass()
        {
            if (ValueExpression is not null && InputEditContext is not null)
            {
                FieldIdentifier fieldIdentifier = FieldIdentifier.Create(ValueExpression);
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, " " + ValidClass) : ContainerClass;
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, ValidClass + " ") : ContainerClass;
                ValidClass = InputEditContext.FieldCssClass(fieldIdentifier);
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.AddClass(ContainerClass, ValidClass) : ContainerClass;
                ContainerClass = Regex.Replace(ContainerClass, @"\s+", " ");
                string validClass = ValidClass;
                if (validClass is not null && (ValidClass == INVALID || ValidClass == MODIFIED_INVALID))
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, SUCCESS_CLASS);
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, ERROR_CLASS);
                }
                else if (ValidClass == MODIFIED_VALID)
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERROR_CLASS);
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, SUCCESS_CLASS);
                }
                else if (ValidClass == "valid" &&
                         !(!string.IsNullOrEmpty(CssClass) &&
                           (CssClass.Contains(ERROR_CLASS, StringComparison.Ordinal) ||
                            CssClass.Contains(SUCCESS_CLASS, StringComparison.Ordinal))))
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERROR_CLASS);
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, SUCCESS_CLASS);
                }
            }
        }
        private async Task OnMouseWheelAsync(WheelEventArgs e)
        {
            if (Disabled || Readonly || IsDevice || !AllowMouseWheel)
            {
                return;
            }
            string? action = null;
            if (e.DeltaY < 0)
            {
                action = "increment";
            }
            else if (e.DeltaY > 0)
            {
                action = "decrement";
            }
            if (action is not null)
            {
                string currentValue = Value is not null ? Value.ToString()! : string.Empty;
                await ServerActionAsync(action, e, currentValue).ConfigureAwait(true);
            }
        }
        /// <summary>
        /// Handles click events for the clear button functionality.
        /// </summary>
        /// <param name="args">The event arguments associated with the click event.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous click event handling operation.</returns>
        /// <remarks>
        /// This method provides click-specific handling for the clear button, ensuring proper behavior on desktop devices by delegating to the common clear button event logic.
        /// </remarks>
        internal async Task BindClearBtnEventsAsync(EventArgs args)
        {
            await InvokeClearBtnEventAsync(args).ConfigureAwait(true);
        }

        /// <summary>
        /// Handles the core logic for clear button events, clearing the input value and updating the component state.
        /// </summary>
        /// <param name="args">The event arguments associated with the clear button interaction.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous clear operation.</returns>
        /// <remarks>
        /// This method performs the following operations when the clear button is activated:
        /// <list type="bullet">
        /// <item><description>Sets the clear button clicked flag to modify input processing behavior</description></item>
        /// <item><description>Clears the input value by setting it to null</description></item>
        /// <item><description>Triggers change events to notify subscribers of the value change</description></item>
        /// <item><description>Prevents event propagation to avoid conflicts with other input handling</description></item>
        /// <item><description>Returns focus to the input element for continued user interaction</description></item>
        /// </list>
        /// </remarks>
        private async Task InvokeClearBtnEventAsync(EventArgs args)
        {
            IsClearIconClick = true;
            IsClearButtonClicked = true;
            await SetValueAsync(null, FloatLabelType, ShowClearButton).ConfigureAwait(true);
            await RaiseChangeEventAsync(args).ConfigureAwait(true);
            ClearBtnStopPropagation = true;
            await FocusAsync().ConfigureAwait(true);
            IsClearIconClick = false;
        }

        // LoggerMessage-backed delegates to avoid allocations (CA1848)
        private static readonly Action<ILogger, Exception?> _logFormattingErrorOccurred =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(2001, nameof(_logFormattingErrorOccurred)),
                "Formatting error occurred.");

        private static readonly Action<ILogger, Exception?> _logArgumentErrorOccurred =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(2002, nameof(_logArgumentErrorOccurred)),
                "Argument error occurred.");

        private static readonly Action<ILogger, Exception?> _logOverflowErrorOccurred =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(2003, nameof(_logOverflowErrorOccurred)),
                "Overflow error occurred.");

        private static readonly Action<ILogger, Exception?> _logInvalidOperationOccurred =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(2004, nameof(_logInvalidOperationOccurred)),
                "Invalid operation occurred.");

        private static readonly Action<ILogger, Exception?> _logFormattingErrorInFormatValue =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(2005, nameof(_logFormattingErrorInFormatValue)),
                "Formatting error in FormatValue.");

        private static readonly Action<ILogger, Exception?> _logOverflowErrorInFormatValue =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(2006, nameof(_logOverflowErrorInFormatValue)),
                "Overflow error in FormatValue.");

        private static readonly Action<ILogger, Exception?> _logTypeConversionErrorInFormatValue =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(2007, nameof(_logTypeConversionErrorInFormatValue)),
                "Type conversion error in FormatValue.");

        private static readonly Action<ILogger, Exception?> _logArgumentErrorInFormatValue =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(2008, nameof(_logArgumentErrorInFormatValue)),
                "Argument error in FormatValue.");

        private static readonly Action<ILogger, Exception?> _logFormattingIssue =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(2011, nameof(_logFormattingIssue)),
                "Formatting issue in OnInitializedAsync.");

        private static readonly Action<ILogger, Exception?> _logJsError =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(2012, nameof(_logJsError)),
                "JS error in OnInitializedAsync.");

        private static readonly Action<ILogger, Exception?> _logUnexpectedErrorOnParametersSet =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(2013, nameof(_logUnexpectedErrorOnParametersSet)),
                "Unexpected error in OnParametersSetAsync.");

        private static readonly Action<ILogger, Exception?> _logUnexpectedErrorOnAfterRender =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(2014, nameof(_logUnexpectedErrorOnAfterRender)),
                "Unexpected error in OnAfterRenderAsync.");
    }
}
