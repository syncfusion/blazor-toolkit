using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Calendars.Interfaces;
using Syncfusion.Blazor.Toolkit.Calendars.Internal;
using Syncfusion.Blazor.Toolkit.Inputs;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// The TimePicker is an intuitive component that provides options to select a time value from a popup list or to set a desired time value.
    /// </summary>
    /// <remarks>
    /// The TimePicker component supports various time formats, keyboard navigation, validation, localization, and accessibility features.
    /// It can work with different data types including DateTime, TimeOnly, TimeSpan, and DateTimeOffset.
    /// The component provides options for time selection through dropdown list, direct input, and keyboard interactions.
    /// </remarks>
    /// <example>
    /// Basic usage of TimePicker component:
    /// <code><![CDATA[
    /// <SfTimePicker TValue="DateTime" @bind-Value="selectedTime" Format="HH:mm" />
    /// ]]></code>
    /// </example>
    public partial class SfTimePicker<TValue> : SfInputBase<TValue>, IMaskPlaceholder
    {
        #region Constants

        private const string INVALID = "invalid";
        private const string MODIFIED_INVALID = "modified invalid";
        private const string MODIFIED_VALID = "modified valid";
        private const string ERROR_CLASS = "e-error";
        private const string SUCCESS_CLASS = "e-success";
        private const string CONTAINER_CLASS = "e-time-wrapper";
        private const string ROOT = "e-control e-timepicker e-lib";
        private const string POPUP = "e-popup";
        private const string NON_EDIT = "e-non-edit";
        private const string TIME_ICON = "e-clock e-toolkit-icons";
        private const string FALSE = "false";
        private const string TRUE = "true";
        private const string ARIA_HAS_POPUP = "aria-haspopup";
        private const string ARIA_ACTIVE_DESCENDANT = "aria-activedescendant";
        private const string ARIA_OWN = "aria-owns";
        private const string POPUPS = "_popup";
        private const string ROLE = "role";
        private const string COMBOBOX = "combobox";
        private const string AUTO_CORRECT = "autocorrect";
        private const string OFF = "off";
        private const string SPELL_CHECK = "spellcheck";
        private const string ARIA_INVALID = "aria-invalid";
        private const string ARIA_CONTROLS = "aria-controls";
        private const string ARIA_LABEL = "aria-label";
        private const string TIMEPICKER = "timepicker";
        private const string ARIA_AUTOCOMPLETE = "aria-autocomplete";
        private const string LIST = "list";
        private const string AUTO_CAPITAL = "autocapitalize";
        private const string ARIA_EXPANDED = "aria-expanded";
        private const string POPUP_CONTENT = "e-content";
        private const string DISABLED = "e-disabled";
        private const string RTL = "e-rtl";
        private const string READ_ONLY = "readonly";
        private const string INPUT_FOCUS = "e-input-focus";
        private const string POPUP_CONTAINER = "e-popup-wrapper";
        private const string ACTIVE = "e-active";
        private const string TIME_PICKER = "timepicker";
        private const string LIST_ITEM = "e-list-item";
        private const string SELECTED = "e-active";
        private const string HOVER = "e-hover";
        private const string NAVIGATION = "e-navigation";
        private const string ARABIC = "ar";
        private const string THAILAND = "th";
        private const char ARABIC_START_DIGIT = (char)1632;
        private const char ARABIC_END_DIGIT = (char)1641;
        private const char THAILAND_START_DIGIT = (char)3664;
        private const char THAILAND_END_DIGIT = (char)3675;
        private const string DAY_LOCALE_KEY = "Day";
        private const string MONTH_LOCALE_KEY = "Month";
        private const string YEAR_LOCALE_KEY = "Year";
        private const string HOUR_LOCALE_KEY = "Hour";
        private const string MINUTE_LOCALE_KEY = "Minute";
        private const string SECOND_LOCALE_KEY = "Second";
        private const string DAYOFWEEK_LOCALE_KEY = "DayOfWeek";
        private const int POPUP_SHOW_DELAY_MS = 10;
        private const int ICON_CLICK_DELAY_MS = 10;
        private const int CLIENT_POPUP_RENDER_DELAY_MS = 1;

        /// <summary>
        /// Specifies the CSS class for expandable popup in mobile devices.
        /// </summary>
        /// <value>A string constant containing the CSS class "e-popup-expand".</value>
        /// <remarks>This constant is used to apply fullscreen popup behavior on mobile devices.</remarks>
        /// <exclude/>
        protected const string POPUPEXPAND = "e-popup-expand";

        /// <summary>
        /// Specifies the CSS class for the modal header in popup.
        /// </summary>
        /// <value>A string constant containing the CSS class "e-model-header".</value>
        /// <remarks>This constant is used to style the header section of the time picker popup.</remarks>
        /// <exclude/>
        protected const string MODELHEADER = "e-model-header";

        #endregion

        #region Private Variables

        /// <summary>
        /// Gets or sets the CSS class for the time icon displayed in the input field.
        /// </summary>
        /// <value>A string representing the CSS class for the time icon.</value>
        /// <remarks>This property controls the styling and visibility of the time icon that triggers the popup.</remarks>
        private string TimeIcon { get; set; } = default!;

        /// <summary>
        /// Gets or sets the CSS class for the popup container element.
        /// </summary>
        /// <value>A string representing the CSS class for the popup container.</value>
        /// <remarks>This property is used to style the container that holds the time selection popup.</remarks>
        private string PopupContainer { get; set; } = default!;

        /// <summary>
        /// Gets or sets the previous value of the input element for change detection.
        /// </summary>
        /// <value>A string representing the previous input value.</value>
        /// <remarks>This property is used internally to track value changes and trigger events accordingly.</remarks>
        private string? PreviousElementValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the component is running on a mobile device.
        /// </summary>
        /// <value>true if the component is on a mobile device; otherwise, false.</value>
        /// <remarks>This property affects the popup behavior and styling for mobile-specific interactions.</remarks>
        private bool IsDevice { get; set; }

        /// <summary>
        /// Gets or sets the mask placeholder configuration for input formatting.
        /// </summary>
        /// <value>A TimePickerMaskPlaceholder object containing placeholder settings.</value>
        /// <remarks>This property is used when EnableMask is true to configure input field placeholders.</remarks>
        private TimePickerMaskPlaceholder? MaskPlaceholder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether keyboard navigation is active in the popup.
        /// </summary>
        /// <value>true if navigation is active; otherwise, false.</value>
        /// <remarks>This property is used internally to handle keyboard navigation states in the time list popup.</remarks>
        private bool Navigated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to stop propagation of clear button events.
        /// </summary>
        /// <value>true to stop event propagation; otherwise, false.</value>
        /// <remarks>This property is used internally to control event bubbling for the clear button functionality.</remarks>
        private bool ClearBtnStopPropagation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the input value was cleared.
        /// </summary>
        /// <value>true if the value was cleared; otherwise, false.</value>
        /// <remarks>This property tracks the clear operation state for proper event handling and masking.</remarks>
        private bool IsCleared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the popup list.
        /// </summary>
        /// <value>true to show the popup; otherwise, false.</value>
        /// <remarks>This property controls the visibility state of the time selection popup.</remarks>
        private bool ShowPopupList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the list has been rendered.
        /// </summary>
        /// <value>true if the list is rendered; otherwise, false.</value>
        /// <remarks>This property tracks the rendering state of the time list for optimization purposes.</remarks>
        private bool IsListRendered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the list is currently being rendered.
        /// </summary>
        /// <value>true if the list is being rendered; otherwise, false.</value>
        /// <remarks>This property manages the rendering lifecycle of the popup time list.</remarks>
        private bool IsListRender { get; set; }

        /// <summary>
        /// Gets or sets the popup event arguments for open/close operations.
        /// </summary>
        /// <value>A DatePickerPopupArgs object containing popup event data.</value>
        /// <remarks>This property holds event arguments passed to popup-related event handlers.</remarks>
        private DatePickerPopupArgs PopupEventArgs { get; set; } = new();

        /// <summary>
        /// Gets or sets a value indicating whether the time icon was clicked.
        /// </summary>
        /// <value>true if the time icon was clicked; otherwise, false.</value>
        /// <remarks>This property tracks time icon click state for proper focus and popup management.</remarks>
        private bool IsTimeIconClicked { get; set; }

        /// <summary>
        /// Gets or sets the current culture information for formatting and localization.
        /// </summary>
        /// <value>A CultureInfo object representing the current culture.</value>
        /// <remarks>This property determines how dates, times, and numbers are formatted and displayed.</remarks>
        private CultureInfo CurrentCulture { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the time list has been updated.
        /// </summary>
        /// <value>true if the list was updated; otherwise, false.</value>
        /// <remarks>This property tracks when the time list data has been refreshed or regenerated.</remarks>
        private bool ListUpdated { get; set; }

        /// <summary>
        /// Gets or sets the date part used for time calculations.
        /// </summary>
        /// <value>A DateTime representing the date portion for time operations.</value>
        /// <remarks>This property provides the date context when working with time-only values.</remarks>
        private DateTime DatePart { get; set; }

        /// <summary>
        /// Gets or sets the list of time options displayed in the popup.
        /// </summary>
        /// <value>A list of ListOptions containing time values and display information.</value>
        /// <remarks>This property contains all time options generated based on the Step property and min/max constraints.</remarks>
        private List<ListOptions<TValue>>? ListData { get; set; }

        /// <summary>
        /// Gets or sets the client-side mask values and formatting information.
        /// </summary>
        /// <value>A ClientMaskValues object containing mask-related data.</value>
        /// <remarks>This property holds mask formatting information received from JavaScript interop when EnableMask is true.</remarks>
        private ClientMaskValues? ClientMaskValue { get; set; } = new ClientMaskValues();

        /// <summary>
        /// Gets or sets the index of the currently active item in the popup list.
        /// </summary>
        /// <value>An integer representing the active item index, or null if no item is active.</value>
        /// <remarks>This property tracks which item is currently highlighted or selected during keyboard navigation.</remarks>
        private int? ActiveIndex { get; set; }

        /// <summary>
        /// Gets or sets the previous datetime value for change detection.
        /// </summary>
        /// <value>A TValue representing the previous datetime value.</value>
        /// <remarks>This property is used to detect value changes and trigger appropriate events.</remarks>
        private TValue? PreviousDateTime { get; set; }

        /// <summary>
        /// Gets or sets the validation CSS class applied to the component.
        /// </summary>
        /// <value>A string representing the validation CSS class.</value>
        /// <remarks>This property contains validation-related CSS classes based on the current validation state.</remarks>
        private string? ValidClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether keyboard navigation is active.
        /// </summary>
        /// <value>true if navigation is active; otherwise, false.</value>
        /// <remarks>This property tracks whether the user is navigating through the time list using keyboard keys.</remarks>
        private bool IsNavigate { get; set; }

        /// <summary>
        /// Gets or sets the current input value as a string.
        /// </summary>
        /// <value>A string representing the current input value.</value>
        /// <remarks>This property holds the raw string value currently displayed in the input field.</remarks>
        private string? CurrentInputValue { get; set; }

        /// <summary>
        /// Gets or sets the strict mode value when validation fails.
        /// </summary>
        /// <value>A string representing the value under strict mode validation.</value>
        /// <remarks>This property is used in strict mode to handle invalid input values appropriately.</remarks>
        private string? StrictValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current value is valid.
        /// </summary>
        /// <value>true if the value is valid; otherwise, false.</value>
        /// <remarks>This property tracks the validation state of the current input value.</remarks>
        private bool IsValideValue { get; set; }

        /// <summary>
        /// Gets or sets the ID of the currently active descendant for ARIA accessibility.
        /// </summary>
        /// <value>A string representing the active descendant ID.</value>
        /// <remarks>This property is used for accessibility to indicate which popup item is currently active.</remarks>
        private string? AriaActiveDescendantID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the component is in a blurred state.
        /// </summary>
        /// <value>true if the component is blurred; otherwise, false.</value>
        /// <remarks>This property tracks whether the input field has lost focus for proper event handling.</remarks>
        private bool IsBlurred { get; set; }

        [GeneratedRegex("^[F/U/u/O/o/d/D/f/g/G/m/M/R/r/s/t/T/y/Y]*$", RegexOptions.CultureInvariant)]
        private static partial Regex StandardDateFormatRegex();

        [GeneratedRegex(@"[^a-zA-Z\s]", RegexOptions.CultureInvariant)]
        private static partial Regex NonAlphaTimeSpanRegex();

        [GeneratedRegex(@"\\+", RegexOptions.CultureInvariant)]
        private static partial Regex BackSlashSequenceRegex();

        [GeneratedRegex(@"\s+", RegexOptions.CultureInvariant)]
        private static partial Regex MultipleWhiteSpaceRegex();

        private IJSObjectReference? _timePickerJsModule;
        private IJSInProcessObjectReference? _timePickerJsInProcessModule;
        private IJSObjectReference? _maskJsModule;
        private IJSInProcessObjectReference? _maskJsInProcessModule;

        #endregion

        #region Internal Variables

        /// <summary>
        /// Gets or sets the current mask format string.
        /// </summary>
        /// <value>A string representing the current mask format pattern.</value>
        /// <remarks>This property contains the mask format used for input validation and display when EnableMask is enabled.</remarks>
        /// <exclude/>
        internal string CurrentMaskFormat { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the element reference for the popup container.
        /// </summary>
        /// <value>An ElementReference pointing to the popup DOM element.</value>
        /// <remarks>This property provides direct access to the popup DOM element for JavaScript interop.</remarks>
        internal ElementReference PopupElement { get; set; }

        /// <summary>
        /// Gets or sets the element reference for the popup holder container.
        /// </summary>
        /// <value>An ElementReference pointing to the popup holder DOM element.</value>
        /// <remarks>This property provides access to the popup holder element used for positioning and styling.</remarks>
        internal ElementReference PopupHolderEle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value was changed by keyboard input.
        /// </summary>
        /// <value>true if the value was changed via keyboard; otherwise, false.</value>
        /// <remarks>This property is used internally to track the source of value changes for proper event handling.</remarks>
        /// <exclude/>
        internal bool IsChangeValue { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the value was selected from the popup list.
        /// </summary>
        /// <value>true if the value was selected from popup; otherwise, false.</value>
        /// <remarks>This property helps distinguish between popup selection and direct input for proper validation.</remarks>
        /// <exclude/>
        internal bool IsKeySelect { get; set; }

        /// <summary>
        /// Gets or sets the selected time value with mask format.
        /// </summary>
        /// <exclude/>
        internal string CurrentMaskValue { get; set; } = string.Empty;

        #endregion

        #region Protected Variables

        /// <summary>
        /// Gets or sets the dictionary containing default mask placeholder values based on current culture.
        /// </summary>
        /// <value>A dictionary mapping placeholder keys to localized values.</value>
        /// <remarks>This dictionary is populated based on the current culture and mask placeholder settings when EnableMask is true.</remarks>
        /// <exclude/>
        protected Dictionary<string, string> MaskPlaceholderDictionary { get; set; } = [];

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes component properties with current values for change tracking.
        /// </summary>
        /// <remarks>
        /// This method stores the initial values of key properties to enable change detection
        /// in the component lifecycle. It also sets the default format when EnableMask is true
        /// and no format is specified.
        /// </remarks>
        private void PropertyInitialized()
        {
            InternalEnableRtl = SyncfusionService!._options.EnableRtl;
            InternalKeyConfigs = KeyConfigs;
            InternalScrollTo = ScrollTo;
            InternalStep = Step;
            InternalFormat = Format;
            InternalWidth = Width;
            InternalZIndex = ZIndex;
            InternalCssClass = CssClass;
            InternalValue = Value;
            if (Format is null && EnableMask)
            {
                Format = GetDefaultFormat();
            }
            InternalFormat = Format;
            InternalInputFormats = InputFormats;
        }

        /// <summary>
        /// Handles property changes and updates the component state accordingly.
        /// </summary>
        /// <param name="newProps">Dictionary containing the changed properties and their new values.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous property change operation.</returns>
        /// <remarks>
        /// This method processes specific property changes like Value, Format, CssClass, FloatLabelType, and ReadOnly.
        /// It updates masks, formats values, and triggers necessary UI updates based on the changed properties.
        /// </remarks>
        private async Task OnPropertyChangeAsync(Dictionary<string, object> newProps)
        {
            List<KeyValuePair<string, object>> newProperties = [.. newProps];
            foreach (KeyValuePair<string, object> prop in newProperties)
            {
                switch (prop.Key)
                {
                    case nameof(Value):
                        if (EnableMask)
                        {
                            await CreateMaskAsync().ConfigureAwait(false);
                        }
                        UpdateErrorClass();
                        PreviousElementValue = CurrentValueAsString;
                        PreviousDateTime = Value;
                        break;
                    case nameof(Format):
                        if (EnableMask)
                        {
                            await CreateMaskAsync().ConfigureAwait(false);
                        }
                        if ((!string.IsNullOrEmpty(Value!.ToString()) || FloatLabelType == FloatLabelType.Always || string.IsNullOrEmpty(Placeholder)) && ClientMaskValue is not null && EnableMask)
                        {
                            CurrentValueAsString = ClientMaskValue.InputElementValue;
                        }
                        if (Value is not null)
                        {
                            CurrentValueAsString = FormatDateValue(Value, GetDefaultFormat());
                        }
                        break;
                    case nameof(CssClass):
                        ContainerClass = string.IsNullOrEmpty(ContainerClass) ? ContainerClass : SfBaseUtils.RemoveClass(ContainerClass, InternalCssClass);
                        PopupContainer = string.IsNullOrEmpty(PopupContainer) ? PopupContainer : SfBaseUtils.RemoveClass(PopupContainer, InternalCssClass);
                        InternalCssClass = CssClass;
                        break;
                    case nameof(FloatLabelType):
                        if (!string.IsNullOrEmpty(Value!.ToString()) || ((FloatLabelType == FloatLabelType.Always || string.IsNullOrEmpty(Placeholder)) && ClientMaskValue is not null && EnableMask))
                        {
                            CurrentValueAsString = ClientMaskValue?.InputElementValue;
                        }
                        await OnAfterScriptRenderedAsync().ConfigureAwait(false);
                        break;
                    case nameof(READ_ONLY):
                        if (EnableMask)
                        {
                            await CreateMaskAsync().ConfigureAwait(false);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Sets the right-to-left (RTL) CSS class on the popup container based on current RTL settings.
        /// </summary>
        /// <remarks>
        /// This method applies RTL styling to the popup container when either the component's EnableRtl property
        /// or the global RTL setting in SyncfusionService is enabled.
        /// </remarks>
        private void SetRTL()
        {
            PopupContainer = SyncfusionService!._options.EnableRtl ? SfBaseUtils.AddClass(PopupContainer, RTL) : SfBaseUtils.RemoveClass(PopupContainer, RTL);
        }

        /// <summary>
        /// Updates ARIA (Accessible Rich Internet Applications) attributes on the input element for accessibility.
        /// </summary>
        /// <remarks>
        /// This method sets various ARIA attributes including role, popup indicators, autocomplete behavior,
        /// and validation states to ensure proper accessibility support for screen readers and assistive technologies.
        /// </remarks>
        private void UpdateAriaAttributes()
        {
            _ = SfBaseUtils.UpdateDictionary(ARIA_HAS_POPUP, TRUE, InputHtmlAttributes);
            _ = SfBaseUtils.UpdateDictionary(ARIA_AUTOCOMPLETE, LIST, InputHtmlAttributes);
            _ = SfBaseUtils.UpdateDictionary(ROLE, COMBOBOX, InputHtmlAttributes);
            _ = SfBaseUtils.UpdateDictionary(AUTO_CORRECT, OFF, InputHtmlAttributes);
            _ = SfBaseUtils.UpdateDictionary(AUTO_CAPITAL, FALSE, InputHtmlAttributes);
            _ = SfBaseUtils.UpdateDictionary(SPELL_CHECK, FALSE, InputHtmlAttributes);
            _ = SfBaseUtils.UpdateDictionary(ARIA_INVALID, FALSE, InputHtmlAttributes);
            _ = SfBaseUtils.UpdateDictionary(ARIA_LABEL, TIMEPICKER, InputHtmlAttributes);
            _ = SfBaseUtils.UpdateDictionary(ARIA_CONTROLS, $"{ID}_popup", InputHtmlAttributes);
        }

        /// <summary>
        /// Processes and applies HTML attributes from the BaseHtmlAttributes collection to component properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous attribute processing operation.</returns>
        /// <remarks>
        /// This method extracts common HTML attributes like step, min, max, disabled, readonly, tabindex, and value
        /// from the BaseHtmlAttributes and applies them to the corresponding component properties.
        /// </remarks>
        private async Task SetHtmlAttributesAsync()
        {
            if (BaseHtmlAttributes is not null)
            {
                foreach (KeyValuePair<string, object> item in BaseHtmlAttributes)
                {
                    switch (item.Key)
                    {
                        case "step":
                            Step = Convert.ToInt32(item.Value, CultureInfo.CurrentCulture);
                            break;
                        case "min":
                            if (DateTime.TryParse(item.Value.ToString(), GetDefaultCulture(), DateTimeStyles.None, out DateTime minValue))
                            {
                                Min = minValue;
                            }

                            break;
                        case "max":
                            if (DateTime.TryParse(item.Value.ToString(), GetDefaultCulture(), DateTimeStyles.None, out DateTime maxValue))
                            {
                                Max = maxValue;
                            }

                            break;
                        case "disabled":
                            if (item.Value.ToString() is "disabled" or "true")
                            {
                                Disabled = true;
                            }

                            break;
                        case "readonly":
                            if (item.Value.ToString() is "readonly" or "true")
                            {
                                Readonly = true;
                                AllowEdit = false;
                            }

                            break;
                        case "tabindex":
                            TabIndex = Convert.ToInt32(item.Value, CultureInfo.CurrentCulture);
                            break;
                        case "value":
                            await CheckValueAsync(item.Value.ToString()).ConfigureAwait(false);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Applies the custom CSS class to container and popup elements if specified.
        /// </summary>
        /// <remarks>
        /// This method adds the CssClass property value to both the main container and popup container
        /// elements, ensuring consistent styling across the component's visual elements.
        /// </remarks>
        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = (!string.IsNullOrEmpty(ContainerClass) && !ContainerClass.Contains(CssClass, StringComparison.Ordinal)) ? SfBaseUtils.AddClass(ContainerClass, CssClass) : ContainerClass;
                PopupContainer = (!string.IsNullOrEmpty(PopupContainer) && !PopupContainer.Contains(CssClass, StringComparison.Ordinal)) ? SfBaseUtils.AddClass(PopupContainer, CssClass) : PopupContainer;
            }
        }

        /// <summary>
        /// Sets the readonly attribute on the input element based on AllowEdit and Readonly properties.
        /// </summary>
        /// <remarks>
        /// This method manages the readonly state of the input field by adding or removing the readonly attribute
        /// based on the combination of AllowEdit and Readonly property values.
        /// </remarks>
        private void SetTimeAllowEdit()
        {
            InputHtmlAttributes = AllowEdit ? !Readonly ? RemoveAttributes(READ_ONLY, InputHtmlAttributes) : InputHtmlAttributes
                    : SfBaseUtils.UpdateDictionary(READ_ONLY, string.Empty, InputHtmlAttributes);
        }

        /// <summary>
        /// Removes a specified attribute from the attributes dictionary.
        /// </summary>
        /// <param name="removeClass">The attribute key to remove from the dictionary.</param>
        /// <param name="attr">The attributes dictionary to modify.</param>
        /// <returns>The modified attributes dictionary with the specified attribute removed.</returns>
        /// <remarks>
        /// This utility method safely removes attributes from the input element's attribute collection
        /// and is used to manage dynamic attribute changes.
        /// </remarks>
        private static Dictionary<string, object> RemoveAttributes(string removeClass, Dictionary<string, object> attr)
        {
            _ = attr.Remove(removeClass);
            return attr;
        }

        /// <summary>
        /// Saves the component's current value to browser's local storage for persistence.
        /// </summary>
        /// <param name="persistId">The unique identifier for the stored value.</param>
        /// <param name="dataValue">The current time value to persist.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous storage operation.</returns>
        /// <remarks>
        /// This method is only active when EnablePersistence is true and allows the component
        /// to restore its value across browser sessions.
        /// </remarks>
        private async Task SetLocalStorageAsync(string persistId, TValue dataValue)
        {
            if (EnablePersistence && dataValue is not null)
            {
                await InvokeVoidAsync(_baseJsModule, _baseJsInProcessModule, "setLocalStorageItem", [persistId, dataValue]).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Updates the icon state CSS class based on the current edit and readonly states.
        /// </summary>
        /// <remarks>
        /// This method manages the non-edit CSS class on the container element based on the
        /// AllowEdit, Readonly properties, and whether the input has a value.
        /// </remarks>
        private void UpdateIconState()
        {
            ContainerClass = (!AllowEdit && !Readonly) ? string.IsNullOrEmpty(CurrentValueAsString) ?
                    SfBaseUtils.RemoveClass(ContainerClass, NON_EDIT) : SfBaseUtils.AddClass(ContainerClass, NON_EDIT) : SfBaseUtils.RemoveClass(ContainerClass, NON_EDIT);
        }

        /// <summary>
        /// Notifies the component of parameter changes and updates internal tracking variables.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous parameter notification operation.</returns>
        /// <remarks>
        /// This method uses the NotifyPropertyChanges method to detect which properties have changed
        /// and updates the corresponding internal tracking fields for change detection.
        /// </remarks>
        private async Task PropertyParametersSetAsync()
        {
            InternalFormat = NotifyPropertyChanges(nameof(Format), Format, InternalFormat);
            InternalInputFormats = NotifyPropertyChanges(nameof(InputFormats), InputFormats, InternalInputFormats);
            InternalEnableRtl = NotifyPropertyChanges(nameof(SyncfusionService._options.EnableRtl), SyncfusionService!._options.EnableRtl, InternalEnableRtl);
            InternalKeyConfigs = NotifyPropertyChanges(nameof(KeyConfigs), KeyConfigs, InternalKeyConfigs);
            InternalScrollTo = NotifyPropertyChanges(nameof(ScrollTo), ScrollTo, InternalScrollTo);
            InternalStep = NotifyPropertyChanges(nameof(Step), Step, InternalStep);
            InternalWidth = NotifyPropertyChanges(nameof(Width), Width, InternalWidth);
            InternalZIndex = NotifyPropertyChanges(nameof(ZIndex), ZIndex, InternalZIndex);
            _ = NotifyPropertyChanges(nameof(CssClass), CssClass, InternalCssClass);
            InternalValue = NotifyPropertyChanges(nameof(Value), Value, InternalValue);
            InternalReadonly = NotifyPropertyChanges(nameof(READ_ONLY), Readonly, InternalReadonly);
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Converts the current Value to a standardized string representation for JavaScript interop.
        /// </summary>
        /// <returns>A formatted string representation of the current value, or null if no value is set.</returns>
        /// <remarks>
        /// This method handles different TValue types (DateTime, TimeOnly, TimeSpan, DateTimeOffset) and
        /// converts them to consistent string formats for communication with the JavaScript side.
        /// The format used is culture-invariant for reliable parsing on the client side.
        /// </remarks>
        private string ValueString()
        {
            if (Value is not null)
            {
                if (IsDateTimeType())
                {
                    DateTime DateValue = DateTime.Parse(Value.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                    return DateValue.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
                else if (IsTimeOnlyType())
                {
                    return Value is TimeOnly timeValue ? timeValue.ToString("HH:mm:ss", CultureInfo.InvariantCulture) : string.Empty;
                }
                else if (IsTimeSpanType())
                {
                    TimeSpan TimeSpanValue = TimeSpan.Parse(Value.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                    return TimeSpanValue.ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture);
                }
                else
                {
                    DateTimeOffset DateValue = DateTimeOffset.Parse(Value.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                    return DateValue.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
            }
            return default!;
        }

        /// <summary>
        /// Converts standard .NET format characters to culture-specific format patterns.
        /// </summary>
        /// <param name="format">The format string to convert, which may be a standard format character.</param>
        /// <returns>The expanded format pattern if the input is a standard format character; otherwise, the original format.</returns>
        /// <remarks>
        /// This method handles standard .NET DateTime format characters (like 't', 'T', 'd', etc.) and
        /// expands them to their full culture-specific patterns using the current culture settings.
        /// This ensures consistent formatting behavior across different locales.
        /// </remarks>
        private string GetStandardFormatString(string format)
        {
            if (!string.IsNullOrEmpty(format) && format.Length == 1 && StandardDateFormatRegex().IsMatch(format))
            {
                string[]? cultureFormat = CurrentCulture?.DateTimeFormat.GetAllDateTimePatterns(format[0]);
                return cultureFormat is { Length: > 0 } ? cultureFormat[0] : format;
            }
            return Format;
        }

        /// <summary>
        /// Renders the popup on the client side when the popup list is ready to be displayed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous popup rendering operation.</returns>
        /// <remarks>
        /// This method coordinates with JavaScript to render the time selection popup, including
        /// positioning, styling, and event handling setup. It's called when the popup needs to be shown.
        /// </remarks>
        private async Task ClientPopupRenderAsync()
        {
            if (ShowPopupList && IsListRendered)
            {
                IsListRendered = false;
                TimePickerClientProps<TValue> options = new()
                {
                    EnableRtl = SyncfusionService!._options.EnableRtl,
                    ZIndex = ZIndex,
                    KeyConfigs = KeyConfigs,
                    Value = Value!,
                    Width = Width is not null ? SfBaseUtils.FormatUnit(Width) : default!,
                    Step = Step,
                    ScrollTo = ScrollTo
                };
                await Task.Delay(CLIENT_POPUP_RENDER_DELAY_MS).ConfigureAwait(false);
                await InvokeVoidAsync(_timePickerJsModule!, _timePickerJsInProcessModule!, "renderPopup", [DataId, PopupElement, PopupHolderEle, PopupEventArgs, options]).ConfigureAwait(true);
                IsListRender = true;
            }
        }

        /// <summary>
        /// Updates the component value and input display according to strict mode validation rules.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous strict mode update operation.</returns>
        /// <remarks>
        /// This method handles value validation and formatting in strict mode, ensuring that invalid
        /// inputs are either corrected or reverted to valid values. It also manages mask updates
        /// when masking is enabled.
        /// </remarks>
        private async Task StrictModeUpdateAsync()
        {
            string format = !string.IsNullOrEmpty(Format) ? Format : CurrentCulture.DateTimeFormat.ShortTimePattern;
            string inputValue = GetInputCultureDate(CurrentInputValue?.Trim());
            TValue? date = default;
            if (IsTryParse(inputValue, format))
            {
                date = CreateDateObj(inputValue, Value);
            }

            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) is not null;
            if (Value is not null && string.IsNullOrEmpty(inputValue) && !isNullable)
            {
                date = Value;
                await UpdateInputValueAsync(FormatDateValue(date, format)).ConfigureAwait(false);
            }

            if (StrictMode && !(IsFocused && ValidateOnInput) && date is not null)
            {
                await UpdateInputValueAsync(FormatDateValue(date, format)).ConfigureAwait(false);
                if (PreviousElementValue != inputValue)
                {
                    UpdateValue(date);
                }
            }
            else if ((!StrictMode || (IsFocused && ValidateOnInput)) && (PreviousElementValue != inputValue))
            {
                UpdateValue(date);
            }

            if (StrictMode && !(IsFocused && ValidateOnInput) && date is null && string.IsNullOrEmpty(inputValue))
            {
                UpdateValue(null);
            }
            if (StrictMode && EnableMask && IsRendered)
            {
                await CreateMaskAsync().ConfigureAwait(false);
                await InvokeVoidAsync(_timePickerJsModule!, _timePickerJsInProcessModule!, "updateCurrentValue", [DataId, CurrentValueAsString!]).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Handles the clear button click event and clears the TimePicker value.
        /// </summary>
        /// <param name="args">The event arguments from the clear button click.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous clear operation.</returns>
        /// <remarks>
        /// This method clears the current value, updates masks if enabled, triggers the Cleared event,
        /// closes any open popup, and restores focus to the input. It also handles event propagation control.
        /// </remarks>
        private async Task InvokeClearBtnEventAsync(EventArgs args)
        {
            if (!IsDevice)
            {
                ClearBtnStopPropagation = true;
            }

            IsCleared = true;
            CurrentInputValue = null;
            UpdateValue(null);
            await UpdateInputValueAsync(null).ConfigureAwait(false);
            if (EnableMask)
            {
                await CreateMaskAsync().ConfigureAwait(false);
            }
            if (Cleared.HasDelegate)
            {
                await Cleared.InvokeAsync(new ClearedEventArgs() { Event = args }).ConfigureAwait(false);
            }

            await FocusAsync().ConfigureAwait(false);
            await ChangeTriggerAsync(args).ConfigureAwait(false);
            await HidePopupAsync(args).ConfigureAwait(false);
            AriaActiveDescendantID = null;
            _ = InputHtmlAttributes.Remove(ARIA_ACTIVE_DESCENDANT);
            if (EnableMask)
            {
                IsCleared = false;
            }
        }

        /// <summary>
        /// Handles mouse click events on the time picker icon.
        /// </summary>
        /// <param name="eventArgs">The mouse event arguments.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous icon click handling operation.</returns>
        /// <remarks>
        /// This method delegates to TimeIconHandler to process the icon click event.
        /// </remarks>
        private async Task MouseIconHandlerAsync(EventArgs eventArgs)
        {
            await TimeIconHandlerAsync(eventArgs).ConfigureAwait(false);
        }

        /// <summary>
        /// Handles time icon click events to show or hide the time selection popup.
        /// </summary>
        /// <param name="args">The event arguments from the icon click.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous icon handling operation.</returns>
        /// <remarks>
        /// This method manages popup visibility, handles device-specific behavior, applies focus styling,
        /// and coordinates between showing and hiding the popup based on current state.
        /// </remarks>
        private async Task TimeIconHandlerAsync(EventArgs? args = null)
        {
            if (!Disabled)
            {
                await Task.Delay(ICON_CLICK_DELAY_MS).ConfigureAwait(false); // set the delay for prevent the icon click action.
                if (IsDevice)
                {
                    _ = SfBaseUtils.UpdateDictionary(READ_ONLY, string.Empty, InputHtmlAttributes);
                    await FocusOutAsync().ConfigureAwait(false);
                }

                IsTimeIconClicked = true;
                if (IsListRender)
                {
                    await HidePopupAsync(args).ConfigureAwait(false);
                }
                else
                {
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUT_FOCUS);
                    if (TimeIcon is not null)
                    {

                        TimeIcon = SfBaseUtils.AddClass(TimeIcon, ACTIVE);
                    }
                    await SetReadOnlyFocusAsync().ConfigureAwait(false);
                    await ShowPopupAsync(args).ConfigureAwait(false);
                }
            }
        }


        /// <summary>
        /// Returns the nearest lower step value that evenly divides a full day.
        /// </summary>
        /// <remarks>
        /// Used to coerce invalid <c>Step</c> values into a safe divisor of 1440 minutes,
        /// ensuring predictable and uniform time entries in the TimePicker popup.
        /// </remarks>
        private static int GetNearestValidStep(int value)
        {
            int[] validSteps =
            [
                720, 480, 360, 240, 180, 120, 90,
                60,  45,  40,  30,  20,  15,
                12,  10,   6,   5,   4,   3,   2,   1
            ];
            foreach (int step in validSteps)
            {
                if (step <= value)
                {
                    return step;
                }
            }
            return 30;
        }

        /// <summary>
        /// Generates the list of time options for the popup based on Step, Min, and Max properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous list generation operation.</returns>
        /// <remarks>
        /// This method creates time intervals based on the Step property, formats them according to
        /// the current format and culture, handles item render events for customization, and manages
        /// disabled states for individual time options.
        /// </remarks>
        private async Task GenerateListAsync()
        {
            if (Step > 0)
            {
                TimeSpan start = Min.TimeOfDay;
                TimeSpan end = Max.TimeOfDay;
                TimeSpan interval = new(0, Step, 0);
                ListData = [];
                string formatString = GetDefaultFormat();
                if (IsTimeSpanType())
                {
                    formatString = string.IsNullOrEmpty(Format) ? "HH:mm:ss" : formatString.Replace("hh", "HH", StringComparison.Ordinal);
                }
                while (end >= start)
                {
                    DateTime listDateTime = new(DatePart.Year, DatePart.Month, DatePart.Day, start.Hours, start.Minutes, start.Seconds, start.Milliseconds, DatePart.Kind);
                    string timeFormatValue = Intl.GetDateFormat(listDateTime, formatString);
                    ListOptions<TValue> listItem = new()
                    {
                        DateTimeValue = ConvertGeneric(listDateTime),
                        ItemData = timeFormatValue,
                        ListClass = LIST_ITEM
                    };
                    bool disabled = false;
                    if (OnItemRender.HasDelegate)
                    {
                        ItemEventArgs<TValue> itemEventArgs = new()
                        {
                            Name = "OnItemRender",
                            Value = listItem.DateTimeValue,
                            Text = listItem.ItemData,
                            IsDisabled = false
                        };
                        await OnItemRender.InvokeAsync(itemEventArgs).ConfigureAwait(false);
                        disabled = itemEventArgs.IsDisabled;
                    }
                    if (disabled)
                    {
                        listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, DISABLED);
                    }

                    ListData.Add(listItem);
                    start = start.Add(interval);
                }

                ListUpdated = true;
            }
        }

        /// <summary>
        /// Gets the default time format string based on current culture or the specified Format property.
        /// </summary>
        /// <returns>A string representing the time format to use for display and parsing.</returns>
        /// <remarks>
        /// This method returns the Format property if specified, otherwise falls back to the
        /// current culture's short time pattern for consistent time formatting.
        /// </remarks>
        private string GetDefaultFormat()
        {
            CultureInfo currentCulture = GetDefaultCulture();
            return string.IsNullOrEmpty(Format) ? currentCulture.DateTimeFormat.ShortTimePattern : Format;
        }

        /// <summary>
        /// Gets the default culture information based on the TimePickerLocale or system default.
        /// </summary>
        /// <returns>A <see cref="CultureInfo"/> object representing the culture to use for formatting.</returns>
        /// <remarks>
        /// This method returns a culture based on the TimePickerLocale property if specified,
        /// otherwise uses the system's default culture for localization and formatting.
        /// </remarks>
        private CultureInfo GetDefaultCulture()
        {
            _ = string.IsNullOrEmpty(TimePickerLocale) ? string.Empty : TimePickerLocale;
            return Intl.GetCulture();
        }

        /// <summary>
        /// Closes the time selection popup and updates the component state accordingly.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous popup closing operation.</returns>
        /// <remarks>
        /// This method handles popup closure, updates ARIA attributes, restores focus if needed,
        /// and triggers component state changes to reflect the closed popup state.
        /// </remarks>
        private async Task ClosePopupActionAsync()
        {
            if (IsTimeIconClicked)
            {
                await FocusAsync().ConfigureAwait(false);
            }

            IsListRender = false;
            ShowPopupList = false;
            if (IsDevice && !Readonly)
            {
                InputHtmlAttributes = RemoveAttributes(READ_ONLY, InputHtmlAttributes);
            }
            _ = SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, FALSE, InputHtmlAttributes);
            InputHtmlAttributes = RemoveAttributes(ARIA_OWN, InputHtmlAttributes);
        }

        /// <summary>
        /// Handles click events on individual time items in the popup list.
        /// </summary>
        /// <param name="item">The list item that was clicked.</param>
        /// <param name="args">Optional event arguments from the click event.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous item click handling operation.</returns>
        /// <remarks>
        /// This method processes time selection from the popup list, updates the selected value,
        /// closes the popup if appropriate, and handles mask updates when masking is enabled.
        /// </remarks>
        private async Task ListItemClickAsync(ListOptions<TValue> item, EventArgs? args = null)
        {
            IsValideValue = true;
            if (item.ListClass is not null && !item.ListClass.Contains(DISABLED, StringComparison.Ordinal) && !item.ListClass.Contains(SELECTED, StringComparison.Ordinal))
            {
                UpdateListSelection(item.ItemData, SELECTED);
                await CheckValueAsync(item.ItemData, args).ConfigureAwait(false);
            }

            if (IsListRender || (item.ListClass is not null && item.ListClass.Contains(DISABLED, StringComparison.Ordinal)) || (item.ListClass is not null && item.ListClass.Contains(SELECTED, StringComparison.Ordinal)))
            {
                await HidePopupAsync(args).ConfigureAwait(false);
            }
            if (EnableMask && IsRendered)
            {
                Navigated = true;
                await CreateMaskAsync().ConfigureAwait(false);
                Navigated = false;
            }
        }

        /// <summary>
        /// Updates the selection state of list items by applying or removing the specified CSS class.
        /// </summary>
        /// <param name="item">The item data to match for selection.</param>
        /// <param name="className">The CSS class name to apply (e.g., SELECTED, NAVIGATION).</param>
        /// <remarks>
        /// This method manages visual states of popup list items, removing old states and applying
        /// new ones. It also updates ARIA attributes for accessibility after selection changes.
        /// </remarks>
        private void UpdateListSelection(string? item, string className)
        {
            if (ListData is null)
            {
                return;
            }
            foreach (ListOptions<TValue> listItem in ListData)
            {
                // Remove HOVER class
                if (listItem.ListClass.Contains(HOVER, StringComparison.CurrentCulture))
                {
                    listItem.ListClass = SfBaseUtils.RemoveClass(listItem.ListClass, HOVER);
                }
                // Remove NAVIGATION class
                if (listItem.ListClass.Contains(NAVIGATION, StringComparison.CurrentCulture))
                {
                    listItem.ListClass = SfBaseUtils.RemoveClass(listItem.ListClass, NAVIGATION);
                }
                // Remove existing selected class
                if (listItem.ListClass.Contains(className, StringComparison.CurrentCulture))
                {
                    listItem.ListClass = SfBaseUtils.RemoveClass(listItem.ListClass, className);
                }
                if (SfBaseUtils.Equals(listItem.ItemData, item))
                {
                    listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, className);
                }
            }
            UpdateAriaActiveDescendant();
        }

        /// <summary>
        /// Validates and processes an input value, updating the component's time value accordingly.
        /// </summary>
        /// <param name="inputValue">The input string to validate and convert.</param>
        /// <param name="args">Optional event arguments for triggering selection events.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous value checking operation.</returns>
        /// <remarks>
        /// This method converts string input to the appropriate TValue type, updates the component value,
        /// refreshes the time list display, and triggers Selected and Change events as appropriate.
        /// </remarks>
        private async Task CheckValueAsync(string? inputValue, EventArgs? args = null)
        {
            TValue? timeValue = default;
            if (!string.IsNullOrEmpty(inputValue))
            {
                timeValue = GetDateObject(GetInputCultureDate(inputValue));
            }

            UpdateValue(timeValue);
            await SelectTimeListAsync().ConfigureAwait(false);
            if (args is not null)
            {
                if (Selected.HasDelegate)
                {
                    await Selected.InvokeAsync(new SelectedEventArgs<TValue>() { Value = timeValue! }).ConfigureAwait(false);
                }
            }

            await ChangeTriggerAsync(args).ConfigureAwait(false);
        }

        /// <summary>
        /// Converts culture-specific digits (Arabic or Thai) to standard Western digits for parsing.
        /// </summary>
        /// <param name="isArabic">true if converting Arabic digits; false for Thai digits.</param>
        /// <param name="dateValue">The string containing culture-specific digits to convert.</param>
        /// <returns>A string with culture-specific digits replaced by Western digits.</returns>
        /// <remarks>
        /// This method handles input from Arabic and Thai locales where digits may be displayed
        /// in local number systems, converting them to standard digits for reliable parsing.
        /// </remarks>
        private static string RemoveCultureDigits(bool isArabic, string dateValue)
        {
            string outDate = string.Empty;
            foreach (char item in dateValue)
            {
                char startVal = isArabic ? ARABIC_START_DIGIT : THAILAND_START_DIGIT;
                char endVal = isArabic ? ARABIC_END_DIGIT : THAILAND_END_DIGIT;
                outDate += (item >= startVal && item <= endVal) ? char.GetNumericValue(item).ToString(CultureInfo.CurrentCulture) : item.ToString();
            }

            return outDate;
        }

        /// <summary>
        /// Processes input values to handle culture-specific digit formats for reliable parsing.
        /// </summary>
        /// <param name="inputValue">The input string that may contain culture-specific digits.</param>
        /// <returns>A string with standardized digits suitable for parsing.</returns>
        /// <remarks>
        /// This method detects Arabic and Thai cultures and converts their native digits
        /// to Western digits to ensure consistent parsing across different locales.
        /// </remarks>
        private string GetInputCultureDate(string? inputValue)
        {
            bool isArabicCulture = CurrentCulture.Name.StartsWith(ARABIC, StringComparison.Ordinal);
            bool isThailandCulture = CurrentCulture.Name.StartsWith(THAILAND, StringComparison.Ordinal);
            return isArabicCulture || isThailandCulture
                ? !string.IsNullOrEmpty(inputValue!) ? RemoveCultureDigits(isArabicCulture, inputValue!) : inputValue!
                : inputValue!;
        }

        /// <summary>
        /// Triggers the ValueChange event when the component value changes.
        /// </summary>
        /// <param name="args">Optional event arguments from the triggering action.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous change event processing.</returns>
        /// <remarks>
        /// This method detects value changes, constructs change event arguments, triggers the ValueChange event,
        /// updates persistence storage, and refreshes validation state.
        /// </remarks>
        private async Task ChangeTriggerAsync(EventArgs? args = null)
        {
            string? inputValue = CurrentValueAsString;
            if (inputValue != PreviousElementValue && SfTimePickerUtils.CompareValues(PreviousDateTime, Value) != 0)
            {
                if (ValueChange.HasDelegate && !(Disabled || Readonly))
                {
                    ChangeEventArgs<TValue> changedEventArgs = new()
                    {
                        Value = Value!,
                        Event = args is null ? new() : args,
                        IsInteracted = args is not null,
                        Text = inputValue ?? string.Empty,
                    };
                    _ = InvokeAsync(() => ValueChange.InvokeAsync(changedEventArgs));
                }
                PreviousElementValue = inputValue;
                PreviousDateTime = Value;
                IsChangeValue = true;
                await SetLocalStorageAsync(ID, Value!).ConfigureAwait(false);
            }

            UpdateErrorClass();
            UpdateValidateClass();
        }

        /// <summary>
        /// Updates the input field display with the current selected time value.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous input update operation.</returns>
        /// <remarks>
        /// This method formats the current Value according to the specified format or culture default
        /// and updates the input field display to reflect the selected time.
        /// </remarks>
        private async Task SelectTimeListAsync()
        {
            string? date = string.Empty;
            if (Value is not null)
            {
                string formatString = !string.IsNullOrEmpty(Format) ? Format : CurrentCulture.DateTimeFormat.ShortTimePattern;
                date = FormatDateValue(Value, formatString);
            }

            await UpdateInputValueAsync(date).ConfigureAwait(false);
        }

        /// <summary>
        /// Formats a time value using the specified format string and handles type-specific formatting.
        /// </summary>
        /// <param name="timeValue">The time value to format.</param>
        /// <param name="formatString">The format string to use for formatting.</param>
        /// <returns>A formatted string representation of the time value, or null if the value is null.</returns>
        /// <remarks>
        /// This method handles different TValue types and applies appropriate formatting, including
        /// special handling for TimeSpan types and escape character processing for complex formats.
        /// </remarks>
        private string? FormatDateValue(TValue? timeValue, string formatString)
        {
            if (IsTimeSpanType())
            {
                formatString = Format is not null ? formatString : "hh:mm:ss";
                formatString = EscapeBackSlashTimeSpan(formatString);
            }
            if ((IsDateTimeType() || IsDateTimeOffsetType()) && Format is not null)
            {
                if (formatString.Contains('\\', StringComparison.Ordinal))
                {
                    formatString = EscapeBackSlashDateTime(formatString);
                }
            }
            return timeValue is not null ? Intl.GetDateFormat(timeValue, formatString) : null;
        }

        /// <summary>
        /// Updates the input field display and handles validation based on the current or previous value.
        /// </summary>
        /// <param name="isInit">true if this is an initialization update; otherwise, false.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous input update operation.</returns>
        /// <remarks>
        /// This method formats and displays the current time value, applies min/max constraints in strict mode,
        /// handles empty values appropriately, and updates error states and icon visibility.
        /// </remarks>
        private async Task UpdateInputAsync(bool isInit = false)
        {
            TValue? dateValue = isInit ? Value : PreviousDateTime;
            if (dateValue is not null)
            {
                if (StrictMode && !(IsFocused && ValidateOnInput))
                {
                    MinMaxUpdates(dateValue);
                }
                string? formatString = string.IsNullOrEmpty(Format) ? CurrentCulture?.DateTimeFormat.ShortTimePattern : Format;
                string dateString = FormatDateValue(dateValue, formatString!);
                await UpdateInputValueAsync(dateString).ConfigureAwait(false);
                bool checkValue = (ConvertDate(dateValue) >= Max) || (ConvertDate(dateValue) <= Min);
                if ((SfTimePickerUtils.CompareValues(ConvertGeneric(Max), dateValue) == 1 && SfTimePickerUtils.CompareValues(dateValue, ConvertGeneric(Min)) == 1) || ((!StrictMode || (IsFocused && ValidateOnInput)) && checkValue))
                {
                    await UpdateInputValueAsync(dateString).ConfigureAwait(false);
                }
            }
            else
            {
                UpdateValue(null);
                if (StrictMode && !(IsFocused && ValidateOnInput) && !EnableMask)
                {
                    await UpdateInputValueAsync(null).ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(CurrentValueAsString))
                {
                    await UpdateInputValueAsync(CurrentValueAsString).ConfigureAwait(false);
                }
            }

            UpdateErrorClass();
            UpdateIconState();
        }

        /// <summary>
        /// Enforces min/max constraints on the time value in strict mode.
        /// </summary>
        /// <param name="timeValue">The time value to validate against min/max constraints.</param>
        /// <remarks>
        /// This method ensures that the time value stays within the defined Min and Max range
        /// when StrictMode is enabled, automatically adjusting values that fall outside the valid range.
        /// </remarks>
        private void MinMaxUpdates(TValue timeValue)
        {
            if (timeValue != null && SfTimePickerUtils.CompareValues(ConvertGeneric(Min), timeValue) == 1 && Min <= Max && StrictMode)
            {
                UpdateValue(ConvertGeneric(Min));
            }
            else if (timeValue != null && SfTimePickerUtils.CompareValues(timeValue, ConvertGeneric(Max)) == 1 && Min <= Max && StrictMode)
            {
                UpdateValue(ConvertGeneric(Max));
            }
        }

        /// <summary>
        /// Updates the component's internal value by converting the provided object to the appropriate TValue type.
        /// </summary>
        /// <param name="timeValue">The time value object to convert and set.</param>
        /// <remarks>
        /// This method safely converts the provided value to the generic TValue type and
        /// updates the InputTextValue property which triggers the component's value binding.
        /// </remarks>
        private void UpdateValue(object? timeValue)
        {
            TValue tempValue = (TValue)SfBaseUtils.ChangeType(timeValue!, typeof(TValue));
            InputTextValue = tempValue;
        }

        /// <summary>
        /// Updates the input field display value and processes culture-specific formatting.
        /// </summary>
        /// <param name="timeValue">The string value to display in the input field.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous input value update operation.</returns>
        /// <remarks>
        /// This method updates the current input value tracking, processes culture-specific digits,
        /// and coordinates with the base component's value setting mechanism.
        /// </remarks>
        private async Task UpdateInputValueAsync(string? timeValue)
        {
            CurrentInputValue = timeValue;
            await SetValueAsync(GetInputCultureDate(timeValue), FloatLabelType, ShowClearButton).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the error CSS class and ARIA invalid attribute based on the current validation state.
        /// </summary>
        /// <remarks>
        /// This method applies error styling when the current value is outside the min/max range
        /// or when an invalid value is present in non-strict mode, and removes error styling for valid values.
        /// </remarks>
        private void UpdateErrorClass()
        {
            if ((Value is not null && !(ConvertDate(Value) >= Min && ConvertDate(Value) <= Max)) ||
                ((!StrictMode || (IsFocused && ValidateOnInput)) && !string.IsNullOrEmpty(CurrentValueAsString) && Value is null && (CurrentMaskFormat != CurrentValueAsString)))
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, ERROR_CLASS);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIA_INVALID, TRUE, InputHtmlAttributes);
            }
            else
            {
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERROR_CLASS);
                InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIA_INVALID, FALSE, InputHtmlAttributes);
            }
        }

        /// <summary>
        /// Converts a text string to a TValue time object by parsing and combining with date context.
        /// </summary>
        /// <param name="text">The text string containing time information to parse.</param>
        /// <returns>A TValue object representing the parsed time, or default if parsing fails.</returns>
        /// <remarks>
        /// This method parses time strings and combines them with appropriate date context,
        /// handling the current value's date portion or falling back to the current date.
        /// </remarks>
        private TValue? GetDateObject(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                TValue? dateValue = CreateDateObj(text);
                bool isValue = Value is not null;
                if (dateValue is not null)
                {
                    int day = isValue ? ConvertDate(Value!).Day : DateTime.Now.Day;
                    int month = isValue ? ConvertDate(Value!).Month : DateTime.Now.Month;
                    int year = isValue ? ConvertDate(Value!).Year : DateTime.Now.Year;
                    DateTime dateVal = ConvertDate(dateValue);
                    DateTime dateTimeVal = new(year, month, day, dateVal.Hour, dateVal.Minute, dateVal.Second, dateVal.Millisecond, DateTimeKind.Local);
                    return ConvertGeneric(dateTimeVal);
                }
            }

            return default;
        }

        /// <summary>
        /// Converts a TValue time object to a DateTime for internal processing.
        /// </summary>
        /// <param name="timeValue">The TValue time object to convert.</param>
        /// <returns>A DateTime representation of the time value.</returns>
        /// <remarks>
        /// This method handles conversion from different TValue types (DateTime, DateTimeOffset, TimeOnly, TimeSpan)
        /// to a standard DateTime format for internal calculations and comparisons.
        /// </remarks>
        private DateTime ConvertDate(TValue timeValue)
        {
            if (IsDateTimeOffsetType())
            {
                DateTimeOffset offsetValue = (DateTimeOffset)SfBaseUtils.ChangeType(timeValue!, typeof(TValue));
                return offsetValue.DateTime;
            }
            else if (IsDateTimeType())
            {
                return (DateTime)SfBaseUtils.ChangeType(timeValue!, typeof(TValue));
            }
            else if (IsTimeOnlyType())
            {
                TimeOnly timeOnlyValue = (TimeOnly)SfBaseUtils.ChangeType(timeValue!, typeof(TValue));
                return Min.Date + timeOnlyValue.ToTimeSpan();
            }
            else
            {
                return DateTime.Now.Date + (TimeSpan)SfBaseUtils.ChangeType(timeValue!, typeof(TValue));
            }
        }

        /// <summary>
        /// Converts a DateTime back to the appropriate TValue type.
        /// </summary>
        /// <param name="timeValue">The DateTime to convert to TValue type.</param>
        /// <returns>A TValue representation of the DateTime.</returns>
        /// <remarks>
        /// This method handles conversion from DateTime to the specific TValue type (DateTime, DateTimeOffset, TimeOnly, TimeSpan)
        /// maintaining type safety and preserving time information appropriately for each type.
        /// </remarks>
        private static TValue ConvertGeneric(DateTime timeValue)
        {
            if (IsDateTimeOffsetType())
            {
                DateTimeOffset dynamicDateValue = timeValue.Date != default(DateTime).Date ? timeValue : default(DateTimeOffset);
                DateTimeOffset offsetValue = new(dynamicDateValue.Year, dynamicDateValue.Month, dynamicDateValue.Day, timeValue.Hour, timeValue.Minute, timeValue.Second, timeValue.Millisecond, dynamicDateValue.Offset);
                return (TValue)SfBaseUtils.ChangeType(offsetValue!, typeof(TValue));
            }
            else if (IsDateTimeType())
            {
                return (TValue)SfBaseUtils.ChangeType(timeValue!, typeof(TValue));
            }
            else if (IsTimeSpanType())
            {
                return (TValue)SfBaseUtils.ChangeType(timeValue.TimeOfDay!, typeof(TValue));
            }
            else if (IsTimeOnlyType())
            {
                return (TValue)SfBaseUtils.ChangeType(timeValue.TimeOfDay!, typeof(TValue));
            }

            return default!;
        }

        /// <summary>
        /// Creates a TValue time object from a string value using appropriate parsing logic.
        /// </summary>
        /// <param name="val">The string value to parse into a time object.</param>
        /// <param name="defaultTimeValue">Optional default time value to use for date context.</param>
        /// <returns>A TValue time object parsed from the string, or default if parsing fails.</returns>
        /// <remarks>
        /// This method handles AM/PM time parsing and delegates to TimeParse for detailed parsing logic,
        /// using the current date context or the provided default time value.
        /// </remarks>
        private TValue? CreateDateObj(string val, TValue? defaultTimeValue = default)
        {
            string formatString = CurrentCulture.DateTimeFormat.ShortDatePattern;
            TValue? timeValue = default;
            DateTime timeItemValue = defaultTimeValue is not null ? ConvertDate(defaultTimeValue) : DateTime.Now;
            string today = GetInputCultureDate(Intl.GetDateFormat(timeItemValue, formatString));
            if (val.Contains("AM", StringComparison.OrdinalIgnoreCase) || val.Contains("PM", StringComparison.OrdinalIgnoreCase))
            {
                if (DateTime.TryParse(today + SPACE + val, CurrentCulture, DateTimeStyles.AssumeLocal, out DateTime dateTime))
                {
                    timeValue = ConvertGeneric(dateTime);
                }

                timeValue ??= TimeParse(today, val);
            }
            else
            {
                timeValue = TimeParse(today, val);
            }

            return timeValue;
        }

        /// <summary>
        /// Converts standard format characters to culture-specific time format patterns.
        /// </summary>
        /// <param name="format">The format string which may contain standard format characters.</param>
        /// <returns>The expanded format pattern or the original format if not a standard character.</returns>
        /// <remarks>
        /// This method expands standard .NET time format characters ('T' for long time, 't' for short time)
        /// to their culture-specific patterns based on the current culture settings.
        /// </remarks>
        private string GetFormat(string format)
        {
            if (string.Equals(format, "T", StringComparison.Ordinal))
            {
                if (CurrentCulture is not null)
                {
                    format = CurrentCulture.DateTimeFormat.LongTimePattern;
                }
            }
            else if (string.Equals(format, "t", StringComparison.Ordinal))
            {
                if (CurrentCulture is not null)
                {
                    format = CurrentCulture.DateTimeFormat.ShortTimePattern;
                }
            }

            return format;
        }

        /// <summary>
        /// Parses a time string into a TValue object using the specified format and date context.
        /// </summary>
        /// <param name="today">The date string to use as context for the time parsing.</param>
        /// <param name="val">The time string to parse.</param>
        /// <param name="format">Optional specific format to use for parsing.</param>
        /// <returns>A parsed TValue time object, or default if parsing fails.</returns>
        /// <remarks>
        /// This method handles parsing for different TValue types (DateTime, DateTimeOffset, TimeSpan, TimeOnly)
        /// and supports escape character processing for complex format patterns.
        /// </remarks>
        private TValue? TimeParse(string today, string? val, string? format = null)
        {
            Type propertyType = typeof(TValue);
            string dateTimeFormat = CurrentCulture.DateTimeFormat.ShortDatePattern + SPACE + CurrentCulture.DateTimeFormat.ShortTimePattern;
            string formatTime = string.IsNullOrEmpty(format) ? CurrentCulture.DateTimeFormat.ShortDatePattern + SPACE + GetFormat(Format) : CurrentCulture.DateTimeFormat.ShortDatePattern + SPACE + GetFormat(format);
            string formatString = string.IsNullOrEmpty(Format) && string.IsNullOrEmpty(format) ? dateTimeFormat : formatTime;
            if (IsDateTimeType())
            {
                DateTime dateTime;
                if (formatString.Contains('\\', StringComparison.Ordinal) && val is not null && val.Contains('\\', StringComparison.Ordinal))
                {
                    string formatAsString = EscapeBackSlashDateTime(formatString);
                    if (DateTime.TryParseExact(today + SPACE + val, formatAsString, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime))
                    {
                        return (TValue)SfBaseUtils.ChangeType(dateTime, typeof(TValue));
                    }
                }
                else if (DateTime.TryParseExact(today + SPACE + val, formatString, CurrentCulture, DateTimeStyles.None, out dateTime))
                {
                    return (TValue)SfBaseUtils.ChangeType(dateTime, propertyType);
                }
            }
            else if (IsDateTimeOffsetType())
            {
                DateTimeOffset offsetValue;
                if (formatString.Contains('\\', StringComparison.Ordinal) && val is not null && val.Contains('\\', StringComparison.Ordinal))
                {
                    string formatAsString = EscapeBackSlashDateTime(formatString);
                    if (DateTimeOffset.TryParseExact(today + SPACE + val, formatAsString, CultureInfo.CurrentCulture, DateTimeStyles.None, out offsetValue))
                    {
                        return (TValue)SfBaseUtils.ChangeType(offsetValue, typeof(TValue));
                    }
                }
                if (DateTimeOffset.TryParseExact(today + SPACE + val, formatString, CurrentCulture, DateTimeStyles.None, out offsetValue))
                {
                    return (TValue)SfBaseUtils.ChangeType(offsetValue, propertyType);
                }
            }
            else if (IsTimeSpanType())
            {
                string timeformat = string.IsNullOrEmpty(Format) ? "hh:mm:ss" : Format;
                string formatasString = EscapeBackSlashTimeSpan(timeformat);
                if (TimeSpan.TryParseExact(val, formatasString, CultureInfo.CurrentCulture, out TimeSpan timeSpanValue))
                {
                    return (TValue)SfBaseUtils.ChangeType(timeSpanValue, typeof(TValue));
                }
            }
            else if (IsTimeOnlyType())
            {
                string timeformat = string.IsNullOrEmpty(format) ? string.IsNullOrEmpty(Format) ? CurrentCulture.DateTimeFormat.ShortTimePattern : Format : format;
                if (TimeOnly.TryParseExact(val, timeformat, CurrentCulture, DateTimeStyles.None, out TimeOnly timeOnlyValue))
                {
                    return (TValue)SfBaseUtils.ChangeType(timeOnlyValue, typeof(TValue));
                }
            }
            return default;
        }

        /// <summary>
        /// Attempts to parse a time string using the specified format to validate input.
        /// </summary>
        /// <param name="timeValue">The time string to validate.</param>
        /// <param name="format">The format pattern to use for parsing validation.</param>
        /// <returns>true if the time string can be parsed with the given format; otherwise, false.</returns>
        /// <remarks>
        /// This method validates input strings against specific formats for different TValue types,
        /// handling escape characters and providing format validation without actually converting the value.
        /// </remarks>
        private static bool IsTryParse(string timeValue, string format)
        {
            if (IsDateTimeType())
            {
                if (format.Contains('\\', StringComparison.Ordinal) && timeValue is not null && timeValue.Contains('\\', StringComparison.Ordinal))
                {
                    string formatString = EscapeBackSlashDateTime(format);
                    return DateTime.TryParseExact(timeValue, formatString, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out _);
                }
                else
                {
                    return DateTime.TryParseExact(timeValue, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out _);
                }

            }
            else if (IsDateTimeOffsetType())
            {
                if (format.Contains('\\', StringComparison.Ordinal) && timeValue is not null && timeValue.Contains('\\', StringComparison.Ordinal))
                {
                    string formatString = EscapeBackSlashDateTime(format);
                    return DateTimeOffset.TryParseExact(timeValue, formatString, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out _);
                }
                else
                {
                    return DateTimeOffset.TryParseExact(timeValue, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out _);
                }
            }
            else if (IsTimeOnlyType())
            {
                return TimeOnly.TryParseExact(timeValue, format, out _);
            }
            else
            {
                string formatString = EscapeBackSlashTimeSpan(format);
                return TimeSpan.TryParseExact(timeValue, formatString, CultureInfo.CurrentCulture, out _);
            }
        }

        /// <summary>
        /// Escapes special characters in TimeSpan format strings for proper parsing.
        /// </summary>
        /// <param name="inputString">The format string to process.</param>
        /// <returns>A format string with escaped special characters.</returns>
        /// <remarks>
        /// This method adds escape characters before non-alphabetic characters in TimeSpan format strings
        /// to ensure they are treated as literal characters during parsing operations.
        /// </remarks>
        private static string EscapeBackSlashTimeSpan(string inputString)
        {
            return NonAlphaTimeSpanRegex().Replace(inputString, match => "\\" + match.Value);
        }

        /// <summary>
        /// Processes escape sequences in DateTime format strings for proper parsing.
        /// </summary>
        /// <param name="input">The format string containing escape sequences.</param>
        /// <returns>A format string with properly processed escape sequences.</returns>
        /// <remarks>
        /// This method handles backslash escape sequences in DateTime format strings,
        /// ensuring that escaped characters are properly interpreted during parsing operations.
        /// </remarks>
        private static string EscapeBackSlashDateTime(string input)
        {
            int maxSlashCount = 0;
            string modifiedInput = BackSlashSequenceRegex().Replace(input, match =>
            {
                int slashCount = match.Value.Length;
                if (slashCount > maxSlashCount)
                {
                    maxSlashCount = slashCount;
                }
                return new string('\\', 2 * slashCount);
            });
            return modifiedInput;
        }

        /// <summary>
        /// Parses a time value string using exact format matching for validation purposes.
        /// </summary>
        /// <param name="timeValue">The time string to parse.</param>
        /// <param name="format">The exact format to use for parsing.</param>
        /// <returns>A parsed TValue object if successful, or default if parsing fails.</returns>
        /// <remarks>
        /// This method provides strict parsing using exact format matching for different TValue types,
        /// used primarily for validation scenarios where exact format compliance is required.
        /// </remarks>
        private static TValue? ParseDateTimeVal(string timeValue, string format)
        {
            Type propertyType = typeof(TValue);
            if (IsDateTimeType())
            {
                if (DateTime.TryParseExact(timeValue, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTimeVal))
                {
                    return (TValue)SfBaseUtils.ChangeType(dateTimeVal, propertyType);
                }
            }
            else if (IsTimeOnlyType())
            {
                if (TimeOnly.TryParseExact(timeValue, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out TimeOnly timeOnlyVal))
                {
                    return (TValue)SfBaseUtils.ChangeType(timeOnlyVal, propertyType);
                }
            }
            else
            {
                if (DateTimeOffset.TryParseExact(timeValue, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out DateTimeOffset dateTimeOffsetVal))
                {
                    return (TValue)SfBaseUtils.ChangeType(dateTimeOffsetVal, propertyType);
                }
            }

            return default;
        }

        /// <summary>
        /// Handles keyboard actions and navigation within the TimePicker component.
        /// </summary>
        /// <param name="args">The keyboard action arguments containing action type and event details.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous keyboard action processing.</returns>
        /// <remarks>
        /// This method processes various keyboard actions including navigation (home, end, up, down),
        /// selection (enter), popup control (open, escape, close), and updates masks when appropriate.
        /// It only operates when the component is enabled and not readonly.
        /// </remarks>
        private async Task KeyboardActionsAsync(KeyActions args)
        {
            if (!Readonly && !Disabled)
            {
                switch (args.Action)
                {
                    case "home":
                    case "end":
                    case "up":
                    case "down":
                        bool PopupOpen = false;
                        if (ShowPopupList)
                        {
                            await KeyHandlerAsync(args).ConfigureAwait(false);
                            PopupOpen = true;
                        }
                        if (EnableMask && PopupOpen)
                        {
                            await CreateMaskAsync().ConfigureAwait(false);
                        }
                        break;
                    case "enter":
                        if (InputFormats is not null && !string.IsNullOrEmpty(CurrentValueAsString))
                        {
                            IsBlurred = true;
                            await UpdateInputValueAsync(CurrentValueAsString).ConfigureAwait(false);
                            IsBlurred = false;
                        }
                        if (EnableMask && !IsKeySelect)
                        {
                            await KeyHandlerAsync(CurrentMaskValue).ConfigureAwait(false);
                            await InvokeVoidAsync(_timePickerJsModule!, _timePickerJsInProcessModule!, "updateCurrentValue", [DataId, CurrentValueAsString!]).ConfigureAwait(true);
                        }
                        IsChangeValue = false;
                        IsKeySelect = false;
                        string? selectItem = IsNavigate ? ListData?.ElementAtOrDefault((int)ActiveIndex!)?.ItemData : CurrentValueAsString;
                        if (IsListRender)
                        {
                            UpdateListSelection(selectItem, SELECTED);
                            await CheckValueAsync(selectItem, args.Events).ConfigureAwait(false);
                        }
                        if (Value is not null)
                        {
                            await CheckValueAsync(selectItem, args.Events).ConfigureAwait(false);
                        }
                        await HidePopupAsync(args.Events).ConfigureAwait(false);
                        await FocusAsync().ConfigureAwait(false);
                        await InvokeVoidAsync(_timePickerJsModule!, _timePickerJsInProcessModule!, "selectInputText", [DataId]).ConfigureAwait(true);
                        if (EnableMask && IsRendered)
                        {
                            await CreateMaskAsync().ConfigureAwait(false);
                        }
                        break;
                    case "open":
                        if (!IsListRender)
                        {
                            await ShowPopupAsync(args.Events).ConfigureAwait(false);
                        }

                        break;
                    case "escape":
                        await UpdateInputAsync().ConfigureAwait(false);
                        await HidePopupAsync(args.Events).ConfigureAwait(false);
                        break;
                    case "close":
                        await HidePopupAsync(args.Events).ConfigureAwait(false);
                        break;
                    default:
                        IsNavigate = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the key down event for the TimePicker input and delegates to KeyboardActions.
        /// </summary>
        /// <param name="e">The keyboard event arguments.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task OnHandleKeyDownAsync(KeyboardEventArgs e)
        {
            if (e is null)
            {
                return;
            }

            string action = SfTimePicker<TValue>.MapKeyToAction(e);

            if (!string.IsNullOrEmpty(action))
            {
                KeyActions keyActions = new()
                {
                    Action = action,
                    Key = e.Key ?? string.Empty,
                    KeyCode = 0,
                    Events = new MouseEventArgs()
                };
                await KeyboardActionsAsync(keyActions).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Maps a <see cref="KeyboardEventArgs"/> to a time picker action string.
        /// </summary>
        /// <param name="e">The keyboard event arguments.</param>
        /// <returns>The mapped action string.</returns>
        private static string MapKeyToAction(KeyboardEventArgs e)
        {
            if (e is null || string.IsNullOrEmpty(e.Key))
            {
                return string.Empty;
            }

            // Alt+Arrow maps
            return e.AltKey && e.Key == "ArrowUp"
                ? "close"
                : e.AltKey && e.Key == "ArrowDown"
                ? "open"
                : e.Key switch
                {
                    "ArrowUp" => "up",
                    "ArrowDown" => "down",
                    "Enter" => "enter",
                    "Escape" => "escape",
                    "Home" => "home",
                    "End" => "end",
                    _ => e.Key ?? string.Empty,
                };
        }

        /// <summary>
        /// Updates the ARIA active descendant attribute on the input element for accessibility.
        /// </summary>
        /// <remarks>
        /// This method sets the aria-activedescendant attribute to indicate which item in the popup list
        /// is currently active or focused. This is important for screen readers and other assistive technologies
        /// to understand the current navigation state within the time picker popup.
        /// </remarks>
        /// <exclude/>
        private void UpdateAriaActiveDescendant()
        {
            if (ListData is null || ListData.Count == 0)
            {
                AriaActiveDescendantID = null;
                return;
            }

            int navigationIndex = -1;
            int selectedIndex = -1;
            for (int i = 0; i < ListData.Count; i++)
            {
                string listClass = ListData[i].ListClass;
                if (listClass.Contains(NAVIGATION, StringComparison.Ordinal))
                {
                    navigationIndex = i; // NAVIGATION takes priority
                }
                else if (listClass.Contains(SELECTED, StringComparison.Ordinal))
                {
                    selectedIndex = i;
                }
            }
            int activeIndex = navigationIndex != -1 ? navigationIndex : selectedIndex;
            AriaActiveDescendantID = activeIndex != -1 ? ID + "_" + activeIndex.ToString(CultureInfo.CurrentCulture) : null;
            if (IsListRender && ShowPopupList && !string.IsNullOrEmpty(AriaActiveDescendantID))
            {
                _ = SfBaseUtils.UpdateDictionary(ARIA_ACTIVE_DESCENDANT, AriaActiveDescendantID, InputHtmlAttributes);
            }
        }

        /// <summary>
        /// Processes keyboard navigation within the time selection popup list.
        /// </summary>
        /// <param name="args">The keyboard action arguments specifying the navigation direction.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous key handling operation.</returns>
        /// <remarks>
        /// This method handles navigation through the popup time list using arrow keys, home/end keys,
        /// manages the active index, skips disabled items, updates visual states, and synchronizes
        /// the input field with the navigated selection.
        /// </remarks>
        private async Task KeyHandlerAsync(KeyActions args)
        {
            if (!await EnsureAndResolveActiveIndexAsync(args).ConfigureAwait(false))
            {
                return;
            }

            await ApplySelectionAsync().ConfigureAwait(false);
            IsNavigate = true;
        }

        private async Task<bool> EnsureAndResolveActiveIndexAsync(KeyActions args)
        {
            if (Step > 0 && ListData is null)
            {
                await GenerateListAsync().ConfigureAwait(false);
            }
            if (ListData is null || ListData.Count == 0)
            {
                return false;
            }
            int firstEnabled = -1;
            int lastEnabled = -1;
            for (int i = 0; i < ListData.Count; i++)
            {
                if (ListData[i].ListClass.Contains(DISABLED, StringComparison.Ordinal))
                {
                    continue;
                }
                firstEnabled = firstEnabled == -1 ? i : firstEnabled;
                lastEnabled = i;
            }

            if (firstEnabled == -1)
            {
                return false;
            }

            if (ActiveIndex is null && string.IsNullOrEmpty(CurrentValueAsString) && Value is null)
            {
                ActiveIndex = args.Action is "up" or "end" ? lastEnabled : firstEnabled;
                return true;
            }

            int currentIndex = firstEnabled;
            for (int i = 0; i < ListData.Count; i++)
            {
                if (ListData[i].ItemData == CurrentValueAsString)
                {
                    currentIndex = i;
                    break;
                }
                if (Value is not null && ConvertDate(ListData[i].DateTimeValue).Ticks >= ConvertDate(Value).Ticks)
                {
                    currentIndex = i;
                    break;
                }
            }
            ActiveIndex = MoveToNextEnabledIndex(args, currentIndex, firstEnabled, lastEnabled);
            return true;
        }

        private int MoveToNextEnabledIndex(KeyActions args, int current, int firstEnabled, int lastEnabled)
        {
            int next = args.Action switch
            {
                "down" => Math.Min(current + 1, ListData!.Count - 1),
                "up" => Math.Max(current - 1, 0),
                "home" => firstEnabled,
                "end" => lastEnabled,
                _ => current
            };
            while (ListData![next].ListClass.Contains(DISABLED, StringComparison.Ordinal))
            {
                next = args.Action == "down"
                    ? Math.Min(next + 1, lastEnabled)
                    : Math.Max(next - 1, firstEnabled);
            }
            return next;
        }

        private async Task ApplySelectionAsync()
        {
            ListOptions<TValue>? selected = ListData!.ElementAtOrDefault(ActiveIndex!.Value);
            if (selected is null)
            {
                return;
            }
            if (IsListRender)
            {
                for (int i = 0; i < ListData?.Count; i++)
                {
                    if (ListData[i].ListClass.Contains(NAVIGATION, StringComparison.Ordinal))
                    {
                        ListData[i].ListClass =
                            SfBaseUtils.RemoveClass(ListData[i].ListClass, NAVIGATION);
                        break;
                    }
                }
                selected.ListClass = SfBaseUtils.AddClass(selected.ListClass, NAVIGATION);
                UpdateAriaActiveDescendant();
            }
            await UpdateInputValueAsync(selected.ItemData).ConfigureAwait(false);
            UpdateValidateClass();
            if (EnableMask && ShowPopupList)
            {
                CurrentMaskValue = selected.ItemData;
            }
            await InvokeVoidAsync(_timePickerJsModule!, _timePickerJsInProcessModule!, "selectInputText", [DataId, true, ActiveIndex.Value]).ConfigureAwait(true);
        }

        /// <summary>
        /// Determines whether the TValue type is DateTime or nullable DateTime.
        /// </summary>
        /// <returns>true if TValue is DateTime or DateTime?; otherwise, false.</returns>
        /// <remarks>
        /// This utility method is used throughout the component to branch logic based on the
        /// specific time type being used, enabling appropriate parsing and formatting behavior.
        /// </remarks>
        private static bool IsDateTimeType()
        {
            Type propertyType = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(propertyType) is not null;
            return propertyType == typeof(DateTime) || (isNullable && typeof(DateTime) == Nullable.GetUnderlyingType(propertyType));
        }

        /// <summary>
        /// Determines whether the TValue type is TimeOnly or nullable TimeOnly.
        /// </summary>
        /// <returns>true if TValue is TimeOnly or TimeOnly?; otherwise, false.</returns>
        /// <remarks>
        /// This utility method enables TimeOnly-specific logic for .NET 6+ time-only scenarios,
        /// providing appropriate handling for time values without date components.
        /// </remarks>
        private static bool IsTimeOnlyType()
        {
            Type proertyType = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(proertyType) is not null;
            return proertyType == typeof(TimeOnly) || (isNullable && typeof(TimeOnly) == Nullable.GetUnderlyingType(proertyType));
        }

        /// <summary>
        /// Determines whether the TValue type is TimeSpan or nullable TimeSpan.
        /// </summary>
        /// <returns>true if TValue is TimeSpan or TimeSpan?; otherwise, false.</returns>
        /// <remarks>
        /// This utility method enables TimeSpan-specific logic for duration-based time scenarios,
        /// requiring special formatting and parsing considerations.
        /// </remarks>
        private static bool IsTimeSpanType()
        {
            Type propertyType = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(propertyType) is not null;
            return propertyType == typeof(TimeSpan) || (isNullable && typeof(TimeSpan) == Nullable.GetUnderlyingType(propertyType));
        }

        /// <summary>
        /// Determines whether the TValue type is DateTimeOffset or nullable DateTimeOffset.
        /// </summary>
        /// <returns>true if TValue is DateTimeOffset or DateTimeOffset?; otherwise, false.</returns>
        /// <remarks>
        /// This utility method enables DateTimeOffset-specific logic for timezone-aware scenarios,
        /// providing appropriate handling for time values with timezone information.
        /// </remarks>
        private static bool IsDateTimeOffsetType()
        {
            Type propertyType = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(propertyType) is not null;
            return propertyType == typeof(DateTimeOffset) || (isNullable && typeof(DateTimeOffset) == Nullable.GetUnderlyingType(propertyType));
        }

        /// <summary>
        /// Updates validation-related CSS classes based on the current validation state.
        /// </summary>
        /// <remarks>
        /// This method integrates with Blazor's validation system to apply appropriate CSS classes
        /// for different validation states (invalid, modified invalid, modified valid, etc.)
        /// and updates the container styling accordingly.
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
                ContainerClass = MultipleWhiteSpaceRegex().Replace(ContainerClass, " ");
                if (ValidClass is INVALID or MODIFIED_INVALID)
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, SUCCESS_CLASS);
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, ERROR_CLASS);
                }
                else if (ValidClass == MODIFIED_VALID)
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERROR_CLASS);
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, SUCCESS_CLASS);
                }
                else if (ValidClass == "valid" && !(!string.IsNullOrEmpty(CssClass) && (CssClass.Contains(ERROR_CLASS, StringComparison.Ordinal) || CssClass.Contains(SUCCESS_CLASS, StringComparison.Ordinal))))
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERROR_CLASS);
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, SUCCESS_CLASS);
                }
            }
        }

        private string GetMaskValue(string? value, string localeKey)
        {
            return string.IsNullOrEmpty(value) ? Localizer[localeKey] : value;
        }

        /// <summary>
        /// Configures mask placeholder content based on current culture and custom placeholder settings.
        /// </summary>
        /// <remarks>
        /// This method populates the MaskPlaceholderDictionary with localized placeholder values
        /// for different date/time components (Day, Month, Year, Hour, Minute, Second, DayOfWeek).
        /// It combines custom placeholder settings with culture-specific defaults and localization.
        /// </remarks>
        private void MaskPlaceholderContent()
        {
            if (EnableMask)
            {
                MaskPlaceholderDictionary?.Add("Day", GetMaskValue(MaskPlaceholder?.Day, DAY_LOCALE_KEY));
                MaskPlaceholderDictionary?.Add("Month", GetMaskValue(MaskPlaceholder?.Month, MONTH_LOCALE_KEY));
                MaskPlaceholderDictionary?.Add("Year", GetMaskValue(MaskPlaceholder?.Year, YEAR_LOCALE_KEY));
                MaskPlaceholderDictionary?.Add("Hour", GetMaskValue(MaskPlaceholder?.Hour, HOUR_LOCALE_KEY));
                MaskPlaceholderDictionary?.Add("Minute", GetMaskValue(MaskPlaceholder?.Minute, MINUTE_LOCALE_KEY));
                MaskPlaceholderDictionary?.Add("Second", GetMaskValue(MaskPlaceholder?.Second, SECOND_LOCALE_KEY));
                MaskPlaceholderDictionary?.Add("DayOfWeek", GetMaskValue(MaskPlaceholder?.DayOfWeek, DAYOFWEEK_LOCALE_KEY));
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Determines whether the component is configured for extended device popup display.
        /// </summary>
        /// <returns>true if the component is on a device and has expandable popup configuration; otherwise, false.</returns>
        /// <remarks>
        /// This method checks if the component is running on a mobile device and whether the popup container
        /// includes the expandable popup CSS class for fullscreen display.
        /// </remarks>
        /// <exclude/>
        protected bool IsExtendedDevicePopup()
        {
            return IsDevice && !string.IsNullOrEmpty(PopupContainer) && PopupContainer.Contains(POPUPEXPAND, StringComparison.Ordinal);
        }

        /// <summary>
        /// Handles the popup close operation when the close icon in fullscreen mobile popup is clicked.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous popup close operation.</returns>
        /// <remarks>
        /// This method is specifically designed for mobile devices where the popup is displayed in fullscreen mode.
        /// It provides a way to close the popup using the close icon in the header.
        /// </remarks>
        /// <exclude/>
        protected async Task PopupCloseHandlerAsync()
        {
            await HidePopupAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Creates and configures the input mask for the TimePicker when EnableMask is true.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous mask creation operation.</returns>
        /// <remarks>
        /// This method initializes the client-side masking functionality, retrieves mask values
        /// from JavaScript, and updates the current value string and mask format accordingly.
        /// It's called when mask configuration changes or needs to be refreshed.
        /// </remarks>
        /// <exclude/>
        protected async Task CreateMaskAsync()
        {
            TimePickerClientProps<TValue> options = GetClientProperties();
            ClientMaskValue = await InvokeAsync<ClientMaskValues>(_timePickerJsModule!, _timePickerJsInProcessModule!, "createMask", [DataId, options]).ConfigureAwait(false);
            if (ClientMaskValue is not null)
            {
                CurrentValueAsString = ClientMaskValue.InputElementValue;
                CurrentMaskFormat = ClientMaskValue.CurrentMaskFormat;
            }
        }

        /// <summary>
        /// Formats the given value as a string for display in the input field.
        /// </summary>
        /// <param name="formatValue">The value to format as a string.</param>
        /// <returns>A formatted string representation of the value for display purposes.</returns>
        /// <remarks>
        /// This override handles time-specific formatting, strict mode validation, and
        /// maintains the current input value state. It applies min/max updates in strict mode
        /// and manages the strict value for invalid inputs during focused validation.
        /// </remarks>
        /// <exclude/>
        protected override string FormatValueAsString(TValue? formatValue)
        {
            if (formatValue is not null)
            {
                if (StrictMode)
                {
                    MinMaxUpdates(formatValue);
                }
                string formatString = string.IsNullOrEmpty(Format) ? CurrentCulture.DateTimeFormat.ShortTimePattern : Format;
                string? dateFormatValue = FormatDateValue(formatValue, formatString);
                dateFormatValue = StrictMode && !(IsFocused && ValidateOnInput) && StrictValue is not null ? StrictValue : dateFormatValue;
                StrictValue = null;
                CurrentInputValue = dateFormatValue;
                return dateFormatValue!;
            }
            else
            {
                string resultValue = StrictValue!;
                CurrentInputValue = resultValue;
                return resultValue;
            }

        }

        /// <summary>
        /// Converts a string input value to the strongly-typed TValue format.
        /// </summary>
        /// <param name="genericValue">The string value to parse and convert.</param>
        /// <returns>The parsed value as TValue type, or default if parsing fails.</returns>
        /// <remarks>
        /// This override handles parsing of time strings into various TValue types (DateTime, TimeOnly, TimeSpan, DateTimeOffset).
        /// It supports multiple input formats, strict mode validation, and maintains proper state for invalid inputs.
        /// The method also handles TimeSpan-specific formatting and culture-specific input processing.
        /// </remarks>
        /// <exclude/>
        protected override TValue FormatValue(string? genericValue = null)
        {
            if (string.IsNullOrEmpty(genericValue))
            {
                StrictValue = null;
                return default!;
            }
            else
            {
                string? format = !string.IsNullOrEmpty(Format) ? Format : CurrentCulture?.DateTimeFormat.ShortTimePattern;
                string? inputValue = genericValue.Trim();
                if (IsTimeSpanType() && string.IsNullOrEmpty(Format) && !string.IsNullOrEmpty(inputValue))
                {
                    inputValue = GetDateObject(GetInputCultureDate(inputValue!)).ToString();
                    format = "hh:mm:ss";
                }
                TValue? date = default;
                bool isTryParse = false;
                if (IsTryParse(inputValue!, format!))
                {
                    if (StrictMode)
                    {
                        MinMaxUpdates(Value!);
                    }
                    date = CreateDateObj(inputValue!, Value);
                    isTryParse = true;
                }
                else if (InputFormats is not null && IsBlurred)
                {
                    foreach (string inputFormat in InputFormats)
                    {
                        if (IsTryParse(inputValue!, inputFormat))
                        {
                            if (StrictMode)
                            {
                                MinMaxUpdates(Value!);
                            }
                            DateTime timeItemValue = Value is not null ? ConvertDate(Value) : DateTime.Now;
                            string today = GetInputCultureDate(Intl.GetDateFormat(timeItemValue, CurrentCulture!.DateTimeFormat.ShortDatePattern));
                            date = TimeParse(today, inputValue, inputFormat);
                            isTryParse = true;
                            break;
                        }
                    }
                }
                if (!isTryParse)
                {
                    if (IsFocused && ValidateOnInput)
                    {
                        StrictValue = inputValue;
                    }
                    else
                    {
                        Type type = typeof(TValue);
                        bool isNullable = Nullable.GetUnderlyingType(type) is not null;
                        if (Value is not null && (!isNullable || IsValideValue))
                        {
                            date = Value;
                        }

                        if (StrictMode && !SfBaseUtils.Equals(date, default))
                        {
                            StrictValue = inputValue;
                        }
                        else if ((!StrictMode || (IsFocused && ValidateOnInput)) && (PreviousElementValue != inputValue) && (!IsValideValue || EnableMask))
                        {
                            StrictValue = inputValue;
                            UpdateValue(date);
                        }

                        if (StrictMode && date is null && string.IsNullOrEmpty(inputValue))
                        {
                            UpdateValue(null);
                        }
                        if (StrictMode && SfBaseUtils.Equals(date, default))
                        {
                            StrictValue = inputValue;
                            TValue? dateValue = (Value is null || string.IsNullOrEmpty(inputValue)) ? default : Value;
                            date = dateValue;
                        }
                    }
                }
                return date!;
            }
        }

        /// <summary>
        /// Handles input changes in the TimePicker input field.
        /// </summary>
        /// <param name="args">The change event arguments containing the new input value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous input handling operation.</returns>
        /// <remarks>
        /// This override processes input field changes, updates the current value string when masking is disabled,
        /// and marks the input as potentially invalid for validation purposes.
        /// </remarks>
        /// <exclude/>
        protected override async Task InputHandlerAsync(ChangeEventArgs? args)
        {
            IsValideValue = false;
            if (OnInput.HasDelegate)
            {
                await OnInput.InvokeAsync(args).ConfigureAwait(true);
            }
            if (!EnableMask)
            {
                CurrentValueAsString = args is not null && args.Value is not null ? (string)args.Value : null;
                await Task.CompletedTask.ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handles the focus event when the TimePicker input receives focus.
        /// </summary>
        /// <param name="args">The focus event arguments.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous focus handling operation.</returns>
        /// <remarks>
        /// This override manages focus behavior including mask value display, text selection,
        /// float label adjustments, and optional popup opening based on OpenOnFocus setting.
        /// It also triggers the Focus event for subscribers.
        /// </remarks>
        /// <exclude/>
        protected override async Task FocusHandlerAsync(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (ClientMaskValue is not null && (CurrentMaskFormat == ClientMaskValue.InputElementValue) && EnableMask)
            {
                if (Value is not null && string.IsNullOrEmpty(Value.ToString()) && (FloatLabelType == FloatLabelType.Auto || FloatLabelType == FloatLabelType.Never) && !string.IsNullOrEmpty(Placeholder))
                {
                    CurrentValueAsString = ClientMaskValue.InputElementValue;
                }
            }
            await InvokeVoidAsync(_timePickerJsModule, _timePickerJsInProcessModule, "selectInputText", [DataId]).ConfigureAwait(true);
            if (OnFocus.HasDelegate)
            {
                await OnFocus.InvokeAsync(new FocusEventArgs()).ConfigureAwait(false);
            }

            if (FloatLabelType == FloatLabelType.Auto)
            {
                await InvokeVoidAsync(_timePickerJsModule, _timePickerJsInProcessModule, "removeFloatLabelSize", [DataId]).ConfigureAwait(true);
            }
            if (OpenOnFocus)
            {
                await ShowPopupAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handles change events from the input field when the user modifies the value.
        /// </summary>
        /// <param name="args">The change event arguments containing the new value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous change handling operation.</returns>
        /// <remarks>
        /// This override processes value changes from the input field and delegates to the KeyHandler
        /// method for further processing and validation.
        /// </remarks>
        /// <exclude/>
        protected override async Task ChangeHandlerAsync(ChangeEventArgs? args)
        {
            IsChangeValue = true;
            await KeyHandlerAsync((string?)args?.Value).ConfigureAwait(false);
            await Task.CompletedTask.ConfigureAwait(false);

        }

        /// <summary>
        /// Handles the focus out (blur) event when the TimePicker loses focus.
        /// </summary>
        /// <param name="args">The focus event arguments.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous focus out handling operation.</returns>
        /// <remarks>
        /// This override manages blur behavior including mask value processing, strict mode updates,
        /// popup closing, validation updates, and float label adjustments. It also triggers the Blur event.
        /// </remarks>
        /// <exclude/>
        protected override async Task FocusOutHandlerAsync(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            IsBlurred = true;
            if (Value is not null && string.IsNullOrEmpty(Value.ToString()) && !string.IsNullOrEmpty(Placeholder) && (FloatLabelType == FloatLabelType.Auto || FloatLabelType == FloatLabelType.Never) && EnableMask)
            {
                if (!(!StrictMode && CurrentValueAsString != CurrentMaskFormat))
                {
                    CurrentValueAsString = null;
                }
                if (!StrictMode && CurrentValueAsString is null)
                {
                    await CreateMaskAsync().ConfigureAwait(false);
                    await UpdateInputValueAsync(null).ConfigureAwait(false);
                }
            }
            if (EnableMask && !IsChangeValue && !string.IsNullOrEmpty(CurrentMaskValue))
            {
                await KeyHandlerAsync(CurrentMaskValue).ConfigureAwait(false);
            }
            AriaActiveDescendantID = Value is null ? null : AriaActiveDescendantID;
            TimeIcon = SfBaseUtils.RemoveClass(TimeIcon, ACTIVE);
            IsTimeIconClicked = false;
            IsKeySelect = false;
            await StrictModeUpdateAsync().ConfigureAwait(false);
            if ((StrictMode && Value is null && (FloatLabelType != FloatLabelType.Always) && !string.IsNullOrEmpty(Placeholder) && EnableMask) || (!EnableMask && Value is null && StrictMode))
            {
                await UpdateInputValueAsync(null).ConfigureAwait(false);
            }

            await UpdateInputAsync(true).ConfigureAwait(false);
            await ChangeTriggerAsync(args).ConfigureAwait(false);
            if (IsListRender)
            {
                await HidePopupAsync(args).ConfigureAwait(false);
            }
            if (OnBlur.HasDelegate)
            {
                _ = InvokeAsync(() => OnBlur.InvokeAsync(new BlurEventArgs()));
            }

            IsBlurred = false;
            if (FloatLabelType is FloatLabelType.Auto or FloatLabelType.Never)
            {
                await InvokeVoidAsync(_timePickerJsModule, _timePickerJsInProcessModule, "updateFloatLabelSize", [DataId]).ConfigureAwait(true);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Builds and returns the client-side properties object for JavaScript interop.
        /// </summary>
        /// <returns>A <see cref="TimePickerClientProps{TValue}"/> object containing all necessary client properties.</returns>
        /// <remarks>
        /// This method assembles all component properties, culture information, and state data
        /// needed for the JavaScript side of the TimePicker component to function properly.
        /// It includes formatting, localization, mask settings, and current component state.
        /// </remarks>
        internal TimePickerClientProps<TValue> GetClientProperties()
        {
            return new TimePickerClientProps<TValue>
            {
                EnableRtl = SyncfusionService!._options.EnableRtl,
                Disabled = Disabled,
                ZIndex = ZIndex,
                KeyConfigs = KeyConfigs,
                Value = Value!,
                ValueString = ValueString(),
                Width = Width,
                Step = Step,
                ScrollTo = ScrollTo,
                EnableMask = EnableMask,
                Format = GetStandardFormatString(Format),
                IsRendered = IsRendered,
                FloatLabelType = FloatLabelType.ToString(),
                IsFocused = IsFocused,
                DayAbbreviatedName = CurrentCulture.DateTimeFormat.AbbreviatedDayNames,
                DayName = CurrentCulture.DateTimeFormat.DayNames,
                MonthName = CurrentCulture.DateTimeFormat.MonthNames,
                MonthAbbreviatedName = CurrentCulture.DateTimeFormat.AbbreviatedMonthNames,
                DayPeriod = [CurrentCulture.DateTimeFormat.AMDesignator, CurrentCulture.DateTimeFormat.PMDesignator],
                MaskPlaceholderDictionary = MaskPlaceholderDictionary,
                Offset = Intl.GetDateFormat(default(DateTime), "zzzz").ToString(),
                Placeholder = Placeholder,
                Navigated = Navigated,
                IsBlurred = IsBlurred,
                Readonly = Readonly
            };
        }

        /// <summary>
        /// Processes input value changes and updates the component's value accordingly.
        /// </summary>
        /// <param name="changeValue">The new input value to process.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous value processing operation.</returns>
        /// <remarks>
        /// This method handles value processing, mask format validation in strict mode,
        /// and coordinates with the base component's value setting mechanism.
        /// </remarks>
        internal async Task KeyHandlerAsync(string? changeValue)
        {
            StrictValue = string.IsNullOrEmpty(changeValue) ? null : StrictValue;
            CurrentInputValue = changeValue;
            if (EnableMask && (CurrentMaskFormat == changeValue) && StrictMode)
            {
                changeValue = null;
            }
            if (!IsCleared)
            {
                await SetValueAsync(changeValue, FloatLabelType, ShowClearButton).ConfigureAwait(false);
            }

            IsCleared = false;
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Sets focus to the TimePicker input through JavaScript interop, handling readonly scenarios.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous focus setting operation.</returns>
        /// <remarks>
        /// This method uses JavaScript to properly focus the input element, especially important
        /// when dealing with readonly states or device-specific focus behavior.
        /// </remarks>
        internal async Task SetReadOnlyFocusAsync()
        {
            await InvokeVoidAsync(_timePickerJsModule, _timePickerJsInProcessModule, "focusIn", [DataId, IsDevice]).ConfigureAwait(true);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the child content properties for mask placeholder configuration.
        /// </summary>
        /// <param name="maskPlaceholderValue">An object containing mask placeholder field values, or null to use default values.</param>
        /// <remarks>
        /// This method is used to configure mask placeholder settings for the TimePicker when EnableMask is true.
        /// If null is provided, default TimePickerMaskPlaceholder values are used.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var placeholders = new TimePickerMaskPlaceholder 
        /// { 
        ///     Hour = "HH", 
        ///     Minute = "MM", 
        ///     Second = "SS" 
        /// };
        /// timePicker.UpdateChildProperties(placeholders);
        /// ]]></code>
        /// </example>
        public void UpdateChildProperties(object maskPlaceholderValue)
        {
            MaskPlaceholder = maskPlaceholderValue is null ? new TimePickerMaskPlaceholder() : (TimePickerMaskPlaceholder)maskPlaceholderValue;
        }

        #endregion
    }
}
