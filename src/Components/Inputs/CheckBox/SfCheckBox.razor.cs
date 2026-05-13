using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Globalization;
using Syncfusion.Blazor.Toolkit;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents a configurable CheckBox component which allows the user to select, deselect, or display an indeterminate state. 
    /// This component supports checked, unchecked, and indeterminate states, and can be used in forms or as a standalone UI input 
    /// to collect binary or triple-state responses.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="SfCheckBox{TChecked}"/> component supports <c>checked</c>, <c>unchecked</c>, and <c>indeterminate</c> states. 
    /// It provides support for two-state and tristate selection patterns with flexible property configuration, including custom label 
    /// positioning, enable/disable tri-state, RTL support, and accessibility options.
    /// </para>
    /// <para>
    /// The checkbox state can be two or three states based on the <see cref="EnableTriState"/> property and is intended for both 
    /// standalone use and within forms.
    /// </para>
    /// <para>
    /// Supported generic types for <typeparamref name="TChecked"/>: <c>bool</c>, <c>bool?</c>, <c>byte</c>, <c>byte?</c>.
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>Basic checkbox that is initially checked:</para>
    /// <code><![CDATA[
    /// <SfCheckBox @bind-Checked="isAccepted" Label="I accept the terms" />
    /// 
    /// @code {
    ///     private bool isAccepted = true;
    /// }
    /// ]]></code>
    /// <para>Tristate checkbox with indeterminate state:</para>
    /// <code><![CDATA[
    /// <SfCheckBox @bind-Checked="selectAll" 
    ///             EnableTriState="true" 
    ///             Label="Select All"
    ///             ValueChange="OnSelectAllChanged" />
    /// 
    /// @code {
    ///     private bool? selectAll = null;
    ///     
    ///     private void OnSelectAllChanged(CheckedChangeEventArgs <bool?> args)
    ///     {
    ///         // Handle the state change
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    /// <typeparam name="TChecked">The type of the checked value. Supported types: bool, bool?, byte, byte?</typeparam>
    public partial class SfCheckBox<TChecked> : SfSelectionBase<TChecked>
    {
        #region Constants

        private const string Space = " ";
        private const string RtlClass = "e-rtl";
        private const string LabelClass = "e-label";
        private const string CheckboxType = "checkbox";
        private const string CheckClass = "e-check";
        private const string IndeterminateClass = "e-stop";
        private const string FrameClass = "e-toolkit-icons e-frame";
        private const string DisabledClass = "e-checkbox-disabled";
        private const string RootClass = "e-control e-checkbox e-lib";
        private const string CheckboxWrapperClass = "e-checkbox-wrapper e-wrapper";
        private const string TitleAttribute = "title";
        private const string OnChangeAttribute = "onchange";
        private const string AriaLabelAttribute = "aria-label";
        private const string ReadOnlyAttribute = "readonly";

        #endregion

        #region Fields

        private string _frameClass = string.Empty;
        private string _checkboxClass = string.Empty;
        private readonly Dictionary<string, object> _htmlAttributes = [];
        private Dictionary<string, object> _labelAttributes = [];

        // JS module references for JS isolation
        private IJSObjectReference? _checkBoxJsModule;
        private IJSInProcessObjectReference? _checkBoxInProcessModule;

        #endregion

        #region Injected Services


        /// <summary>
        /// Gets or sets the logger used for capturing diagnostic and runtime information
        /// related to the <see cref="SfCheckBox{TChecked}"/> component.
        /// </summary>
        /// <remarks>
        /// This logger is primarily intended for internal use to record trace, debug,
        /// warning, and error information during component execution.
        /// </remarks>
        [Inject]
        public required ILogger<SfCheckBox<TChecked>> Logger { get; set; }

        #endregion


        #region Private Methods
        /// <summary>
        /// Determines whether the input element should be rendered as checked.
        /// </summary>
        /// <returns><c>true</c> if the visual should render as checked; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// The indeterminate state takes precedence over the checked state to avoid conflicting UI states.
        /// </remarks>
        private bool GetIsChecked()
        {
            if (!TryConvertToBool(Checked, out bool? checkedValue))
            {
                return false;
            }

            // Indeterminate state takes visual precedence
            return !Indeterminate && checkedValue == true;
        }

        /// <summary>
        /// Returns the ARIA readonly attribute value.
        /// </summary>
        /// <returns>"true" if the readonly attribute is present and set to true; otherwise, "false".</returns>
        private string GetAriaReadOnly()
        {
            return _inputAttributes is not null && _inputAttributes.TryGetValue(ReadOnlyAttribute, out object? readOnly)
                ? readOnly?.ToString()?.Equals("true", StringComparison.OrdinalIgnoreCase) == true ? "true" : "false"
                : "false";
        }

        /// <summary>
        /// Converts the generic checked value to a nullable boolean.
        /// </summary>
        /// <param name="value">The generic value to convert.</param>
        /// <param name="result">When this method returns, contains the converted boolean value if the conversion succeeded, 
        /// or null if the conversion failed or the value was null.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// <para>
        /// This method supports conversion from the following types:
        /// <list type="bullet">
        /// <item><description><c>bool</c> and <c>bool?</c> - Direct conversion</description></item>
        /// <item><description><c>byte</c> and <c>byte?</c> - 0 converts to false, non-zero converts to true</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// Null values are valid for tri-state scenarios and will return true with result set to null.
        /// Note: Nullable&lt;T&gt; values are boxed to their underlying type when they have a value.
        /// </para>
        /// </remarks>
        private static bool TryConvertToBool(TChecked? value, out bool? result)
        {
            result = null;

            if (value is null)
            {
                // Null is valid for tri-state support
                return true;
            }

            try
            {
                // Nullable<T> with a value boxes to its underlying type, so match on bool/byte and handle null separately
                if (value is bool boolValue)
                {
                    result = boolValue;
                    return true;
                }

                if (value is byte byteValue)
                {
                    result = byteValue != 0;
                    return true;
                }

                // Fallback for edge cases - attempt standard conversion
                result = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                return true;
            }
            catch (InvalidCastException)
            {
                result = null;
                return false;
            }
            catch (FormatException)
            {
                result = null;
                return false;
            }
            catch (OverflowException)
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Creates a <typeparamref name="TChecked"/> value from a boolean state.
        /// </summary>
        /// <param name="isChecked">The boolean state to convert.</param>
        /// <returns>A <typeparamref name="TChecked"/> value representing the specified state.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <typeparamref name="TChecked"/> is not a supported type (bool, bool?, byte, or byte?).
        /// </exception>
        /// <remarks>
        /// This method is used internally to create properly typed values for state transitions.
        /// </remarks>
        private static TChecked CreateCheckedValueFromBool(bool isChecked)
        {
            Type type = typeof(TChecked);

            if (type == typeof(bool))
            {
                return (TChecked)(object)isChecked;
            }

            if (type == typeof(bool?))
            {
                bool? val = isChecked;
                return (TChecked)(object)val;
            }

            if (type == typeof(byte))
            {
                byte val = isChecked ? (byte)1 : (byte)0;
                return (TChecked)(object)val;
            }

            if (type == typeof(byte?))
            {
                byte? val = isChecked ? (byte)1 : (byte)0;
                return (TChecked)(object)val;
            }

            // Catch invalid generic type usage immediately at runtime
            throw new InvalidOperationException(
                $"Unsupported TChecked type '{type.FullName}' for SfCheckBox. " +
                $"Supported types: bool, bool?, byte, byte?");
        }

        /// <summary>
        /// Determines whether the specified type is a nullable value type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the type is <see cref="Nullable{T}"/>; otherwise, <c>false</c>.</returns>
        private static bool IsNullableType(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// Updates the underlying checked and indeterminate state and invokes change notifications.
        /// </summary>
        /// <param name="state">The target checkbox state. If null, the component enters indeterminate state.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method coordinates state transitions, ensuring proper event notifications and data binding updates.
        /// For indeterminate states, it only updates nullable types to avoid runtime errors.
        /// </remarks>
        private async Task UpdateCheckedStateAsync(CheckboxState? state = null)
        {
            try
            {
                if (state is CheckboxState.Checked or CheckboxState.Unchecked)
                {
                    bool isChecked = state == CheckboxState.Checked;
                    TChecked checkedState = CreateCheckedValueFromBool(isChecked);
                    await UpdateCheckStateAsync(checkedState).ConfigureAwait(false);

                    // Clear indeterminate state when transitioning to checked/unchecked
                    if (Indeterminate)
                    {
                        await IndeterminateChanged.InvokeAsync(false).ConfigureAwait(false);
                        Indeterminate = false;
                    }
                }
                else
                {
                    // Indeterminate state - only set to null if type supports it
                    if (IsNullableType(typeof(TChecked)))
                    {
                        await CheckedChanged.InvokeAsync(default).ConfigureAwait(false);
                        Checked = default;
                    }

                    await IndeterminateChanged.InvokeAsync(true).ConfigureAwait(false);
                    Indeterminate = true;
                }
            }
            catch (Exception ex)
            {
                _logErrorUpdatingCheckedState(Logger, ex);
                // Rethrow to avoid swallowing unexpected exceptions (satisfies CA1031)
                throw;
            }
        }

        /// <summary>
        /// Updates the visual CSS classes based on the provided checkbox state.
        /// </summary>
        /// <param name="state">The target visual state. If null, applies indeterminate visuals.</param>
        /// <remarks>
        /// This method resets the frame class and applies the appropriate visual indicators for the specified state.
        /// </remarks>
        private void UpdateVisualState(CheckboxState? state = null)
        {
            _frameClass = FrameClass;

            if (state == CheckboxState.Checked)
            {
                ApplyCheckVisuals();
            }
            else if (state == CheckboxState.Unchecked)
            {
                ApplyUncheckVisuals();
            }
            else
            {
                ApplyIndeterminateVisuals();
            }
        }


        /// <summary>
        /// Applies visual CSS classes for the checked state.
        /// </summary>
        /// <remarks>
        /// Removes indeterminate class and adds the check class if not already present.
        /// </remarks>
        private void ApplyCheckVisuals()
        {
            _frameClass = _frameClass.Replace(Space + IndeterminateClass, string.Empty, StringComparison.Ordinal);

            if (_frameClass.IndexOf(Space + CheckClass, StringComparison.Ordinal) < 0)
            {
                _frameClass += Space + CheckClass;
            }
        }

        /// <summary>
        /// Applies visual CSS classes for the unchecked state.
        /// </summary>
        /// <remarks>
        /// Removes both check and indeterminate classes, leaving only the base frame class.
        /// </remarks>
        private void ApplyUncheckVisuals()
        {
            _frameClass = _frameClass.Replace(Space + IndeterminateClass, string.Empty, StringComparison.Ordinal);
            _frameClass = _frameClass.Replace(Space + CheckClass, string.Empty, StringComparison.Ordinal);
        }

        /// <summary>
        /// Applies visual CSS classes for the indeterminate (mixed) state.
        /// </summary>
        /// <remarks>
        /// Removes check class and adds the indeterminate class if not already present.
        /// </remarks>
        private void ApplyIndeterminateVisuals()
        {
            _frameClass = _frameClass.Replace(Space + CheckClass, string.Empty, StringComparison.Ordinal);

            if (_frameClass.IndexOf(Space + IndeterminateClass, StringComparison.Ordinal) < 0)
            {
                _frameClass += Space + IndeterminateClass;
            }
        }

        /// <summary>
        /// Safely merges the provided title attribute into the container HTML attributes.
        /// </summary>
        /// <param name="titleValue">The title attribute value to set.</param>
        /// <returns>The merged attribute collection.</returns>
        /// <remarks>
        /// Updates the existing title attribute if present, otherwise adds a new one.
        /// </remarks>
        private Dictionary<string, object> GetAttributes(object titleValue)
        {
            if (_htmlAttributes.TryGetValue(TitleAttribute, out object? _))
            {
                _htmlAttributes[TitleAttribute] = titleValue;
            }
            else
            {
                _htmlAttributes.Add(TitleAttribute, titleValue);
            }

            return _htmlAttributes;
        }

        /// <summary>
        /// Determines the next checkbox state based on current values and tri-state configuration.
        /// </summary>
        /// <param name="isChecked">The current checked state (false if null).</param>
        /// <param name="isIndeterminate">The current indeterminate state flag.</param>
        /// <param name="allowTriState">Indicates whether tri-state mode is enabled.</param>
        /// <returns>The next <see cref="CheckboxState"/> to transition to.</returns>
        /// <remarks>
        /// <para>
        /// When tri-state is enabled, the cycle is: Checked → Indeterminate → Unchecked → Checked.
        /// </para>
        /// <para>
        /// When tri-state is disabled, the cycle is: Checked ⇄ Unchecked (standard two-state toggle).
        /// </para>
        /// </remarks>
        private static CheckboxState DetermineNextState(bool isChecked, bool isIndeterminate, bool allowTriState)
        {
            if (allowTriState)
            {
                // Tri-state cycle: Checked → Indeterminate → Unchecked → Checked
                return isChecked && !isIndeterminate
                    ? CheckboxState.Indeterminate
                    : isIndeterminate ? CheckboxState.Unchecked : CheckboxState.Checked;
            }

            // Two-state toggle: handle indeterminate as a transition state
            if (isIndeterminate)
            {
                return isChecked ? CheckboxState.Checked : CheckboxState.Unchecked;
            }

            // Standard toggle
            return isChecked ? CheckboxState.Unchecked : CheckboxState.Checked;
        }

        /// <summary>
        /// Persists the checkbox state to local storage (if enabled) and raises the <see cref="ValueChange"/> event.
        /// </summary>
        /// <param name="args">The mouse event arguments that triggered the change.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called after state transitions to notify subscribers and persist data.
        /// </remarks>
        private async Task PersistAndNotifyAsync(MouseEventArgs args)
        {
            try
            {
                if (EnablePersistence)
                {
                    await SetLocalStorageAsync(_idValue, Checked!).ConfigureAwait(false);
                }

                if (ValueChange.HasDelegate)
                {
                    await ValueChange.InvokeAsync(new CheckedChangeEventArgs<TChecked>
                    {
                        Checked = Checked,
                        Event = args
                    }).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logErrorDuringPersistenceOrNotify(Logger, ex);
                throw;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles click interactions to toggle the checkbox state.
        /// </summary>
        /// <param name="args">The mouse event arguments from the click event.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method coordinates the state transition logic, visual updates, and event notifications.
        /// Disabled checkboxes will not respond to clicks.
        /// </remarks>
        private async Task HandleClickAsync(MouseEventArgs args)
        {
            if (Disabled)
            {
                return;
            }

            try
            {
                // Determine current state
                if (!TryConvertToBool(Checked, out bool? current))
                {
                    // If we cannot determine current state, default to unchecked → checked toggle
                    current = false;
                }

                bool isChecked = current ?? false;
                CheckboxState next = DetermineNextState(isChecked, Indeterminate, EnableTriState);

                // Apply changes
                UpdateVisualState(next);
                await UpdateCheckedStateAsync(next).ConfigureAwait(false);
                await PersistAndNotifyAsync(args).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logErrorProcessingClick(Logger, ex);
                throw;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Initializes the visual state and CSS classes for the component.
        /// </summary>
        /// <param name="isDynamic">Indicates whether the checked value changed dynamically after initial render.</param>
        /// <remarks>
        /// This method is called during component initialization and parameter updates to synchronize
        /// the visual state with the current property values.
        /// </remarks>
        protected override void InitRender(bool isDynamic = false)
        {
            // Initialize base checkbox wrapper class
            _checkboxClass = CheckboxWrapperClass;

            // Handle ARIA label extraction for unlabeled checkboxes
            if (string.IsNullOrEmpty(Label) && ChildContent == null)
            {
                if (_inputAttributes is not null && _inputAttributes.TryGetValue(AriaLabelAttribute, out object? ariaLabel))
                {
                    _labelAttributes = new() { { AriaLabelAttribute, ariaLabel } };
                    _ = _inputAttributes.Remove(AriaLabelAttribute);
                }
                else if (_labelAttributes.Count == 0)
                {
                    // Provide default ARIA label for accessibility when no label is provided
                    _labelAttributes = new() { { AriaLabelAttribute, "checkbox" } };
                }
            }

            // Determine initial visual state
            _ = TryConvertToBool(Checked, out bool? state);
            bool isChecked = state.GetValueOrDefault(false);
            UpdateVisualState(isChecked ? CheckboxState.Checked : CheckboxState.Unchecked);

            // Handle tri-state logic
            if (EnableTriState && (!isChecked || isDynamic))
            {
                Indeterminate = false;
            }

            if (Indeterminate || (EnableTriState && state is null))
            {
                Indeterminate = true;
                UpdateVisualState(CheckboxState.Indeterminate);
            }

            // Apply custom CSS class
            if (!string.IsNullOrEmpty(CssClass))
            {
                _checkboxClass += Space + CssClass;
            }

            // Apply disabled state
            if (Disabled)
            {
                if (_checkboxClass.IndexOf(DisabledClass, StringComparison.Ordinal) < 0)
                {
                    _checkboxClass += Space + DisabledClass;
                }
            }
            else
            {
                _checkboxClass = _checkboxClass.Replace(DisabledClass, string.Empty, StringComparison.Ordinal);

                // Remove onchange attribute if not explicitly set by user
                if (_inputAttributes is not null && _inputAttributes.ContainsKey(OnChangeAttribute) && !HasOnChangeEvent)
                {
                    _ = _inputAttributes.Remove(OnChangeAttribute);
                }
            }

            // Apply RTL support
            if (SyncfusionService?._options?.EnableRtl ?? false)
            {
                _checkboxClass += Space + RtlClass;
            }
            else
            {
                _checkboxClass = _checkboxClass.Replace(RtlClass, string.Empty, StringComparison.Ordinal);
            }
        }

        #endregion

        // LoggerMessage-backed delegates to avoid allocations (CA1848)
        private static readonly Action<ILogger, Exception?> _logErrorUpdatingCheckedState =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(1001, nameof(_logErrorUpdatingCheckedState)),
                "Error updating checkbox checked state.");

        private static readonly Action<ILogger, Exception?> _logErrorUpdatingUIVisuals =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(1002, nameof(_logErrorUpdatingUIVisuals)),
                "Error updating UI visuals in UpdateVisualState.");

        private static readonly Action<ILogger, Exception?> _logErrorProcessingClick =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(1003, nameof(_logErrorProcessingClick)),
                "Error processing click in HandleClickAsync.");

        private static readonly Action<ILogger, Exception?> _logErrorDuringPersistenceOrNotify =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(1004, nameof(_logErrorDuringPersistenceOrNotify)),
                "Error during persistence or ValueChange notification.");
    }
}

