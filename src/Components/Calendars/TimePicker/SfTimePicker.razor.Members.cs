using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Toolkit.Inputs;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// The SfTimePicker is an intuitive component which provides options to select a time value from a popup list or to set a desired time value through direct input.
    /// </summary>
    /// <typeparam name="TValue">
    /// Specifies the type of the time value that the SfTimePicker component will handle. This can be <see cref="DateTime"/>, <see cref="DateTime"/>?, <see cref="DateTimeOffset"/>, <see cref="DateTimeOffset"/>?, <see cref="TimeOnly"/>, or <see cref="TimeOnly"/>?.
    /// </typeparam>
    /// <remarks>
    /// The SfTimePicker component inherits from <see cref="SfInputBase{TValue}"/> and provides a comprehensive time selection interface. 
    /// It supports various time formats, input validation, keyboard navigation, and localization features. 
    /// The component can be configured to restrict time selection within specific ranges and supports both popup-based selection and direct text input.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to create a basic SfTimePicker component:
    /// <code><![CDATA[
    /// <SfTimePicker TValue="DateTime?" @bind-Value="@selectedTime" Placeholder="Select time"></SfTimePicker>
    /// 
    /// @code {
    ///     private DateTime? selectedTime = DateTime.Now;
    /// }
    /// ]]></code>
    /// </example>
    public partial class SfTimePicker<TValue> : SfInputBase<TValue>
    {
        #region Private Variables

        private string InternalCssClass { get; set; } = default!;
        private bool InternalEnableRtl { get; set; }
        private bool InternalReadonly { get; set; }
        private bool InternalEnableMask { get; set; }
        private string? InternalFormat { get; set; }
        private string[]? InternalInputFormats { get; set; }
        private Dictionary<string, object>? InternalKeyConfigs { get; set; }
        private DateTime? InternalScrollTo { get; set; }
        private int InternalStep { get; set; }
        private string? InternalWidth { get; set; }
        private int InternalZIndex { get; set; }
        private int _step = 30;

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfTimePicker{TValue}"/> allows users to change the value via direct typing.
        /// </summary>
        /// <value>
        /// <c>true</c> if the TimePicker allows users to change the value via typing; otherwise, <c>false</c> to restrict users to picker-only selection. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>false</c>, users can only select time values from the popup list and cannot type directly into the input field.
        /// This property is useful when you want to ensure that only predefined time values can be selected.
        /// </remarks>
        /// <example>
        /// The following example shows how to disable direct typing in the TimePicker:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" AllowEdit="false" Placeholder="Select from popup only"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool AllowEdit { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfTimePicker{TValue}"/> popup opens automatically when the input field receives focus.
        /// </summary>
        /// <value>
        /// <c>true</c> if the TimePicker popup should open on input focus; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Use this property to improve user experience by automatically showing the TimePicker popup when the user clicks or tabs into the input field.
        /// This is particularly useful for interfaces where quick access to time selection is desirable and reduces the number of clicks required to select a time.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to enable automatic popup opening on focus:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" OpenOnFocus="true" Placeholder="Focus to open popup"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool OpenOnFocus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable full screen layout for the <see cref="SfTimePicker{TValue}"/> component popup on mobile devices.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable full screen layout for popup on mobile devices; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// The FullScreen property is applicable for mobile and tablet devices only. When enabled, the popup will occupy the entire screen on mobile devices, 
        /// providing better usability and touch interaction on smaller screens.
        /// </remarks>
        /// <example>
        /// The following example shows how to enable full screen mode for mobile devices:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" FullScreen="true" Placeholder="Select time"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool FullScreen { get; set; }

        /// <summary>
        /// Gets or sets the floating label behavior of the <see cref="SfTimePicker{TValue}"/> that determines how the <see cref="Placeholder"/> text floats above the input field.
        /// </summary>
        /// <value>
        /// A <see cref="FloatLabelType"/> enumeration value that specifies the floating label behavior. The default value is <see cref="FloatLabelType.Never"/>.
        /// </value>
        /// <remarks>
        /// The floating label behavior can be configured with the following options:
        /// <list type="bullet">
        /// <item><term>Never</term><description>The label never floats above the input field when the placeholder is available.</description></item>
        /// <item><term>Always</term><description>The floating label always remains above the input field.</description></item>
        /// <item><term>Auto</term><description>The floating label appears above the input field when it is focused or has a value.</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to set the floating label behavior:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" FloatLabelType="FloatLabelType.Auto" Placeholder="Select time"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public FloatLabelType FloatLabelType
        {
            get => BaseFloatLabelType; set => BaseFloatLabelType = value;
        }

        /// <summary>
        /// Backing field for the public <see cref="FloatLabelType"/> parameter.
        /// </summary>
        /// <exclude/>
        protected override FloatLabelType BaseFloatLabelType { get; set; }

        /// <summary>
        /// Gets or sets the text that is shown as a hint or placeholder until the user focuses on or enters a value in the <see cref="SfTimePicker{TValue}"/>.
        /// </summary>
        /// <value>
        /// A string value representing the placeholder text. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// The placeholder text behavior depends on the <see cref="FloatLabelType"/> property. 
        /// When <see cref="FloatLabelType"/> is set to <see cref="FloatLabelType.Auto"/>, the placeholder text will float above the input when focused or when the field has a value.
        /// </remarks>
        /// <example>
        /// The following example shows how to set a placeholder text:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" Placeholder="Enter time (HH:mm)"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Placeholder
        {
            get => BasePlaceholder ?? default!; set => BasePlaceholder = value;
        }

        /// <summary>
        /// Backing field for the public <see cref="Placeholder"/> parameter.
        /// </summary>
        /// <exclude/>
        protected override string BasePlaceholder { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfTimePicker{TValue}"/> is in read-only mode, preventing user interaction.
        /// </summary>
        /// <value>
        /// <c>true</c> if the TimePicker value cannot be edited; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When the read-only mode is enabled, users cannot edit the time value through typing or popup interaction. 
        /// The popup cannot be opened when this property is set to <c>true</c>.
        /// </remarks>
        /// <example>
        /// The following example shows how to create a read-only TimePicker:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" Readonly="true" Value="DateTime.Now"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool Readonly
        {
            get => BaseReadOnly; set => BaseReadOnly = value;
        }

        /// <summary>
        /// Backing field for the public <see cref="Readonly"/> parameter.
        /// </summary>
        /// <exclude/>
        protected override bool BaseReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the input element itself should be marked as read-only.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> indicating whether the input element is read-only. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// This property specifically controls the readonly attribute on the underlying HTML input element,
        /// which may behave differently from the component-level <see cref="BaseReadOnly"/> property.
        /// </remarks>
        /// <exclude/>
        protected override bool BaseIsReadOnlyInput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mask rendering is enabled in the <see cref="SfTimePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// <c>true</c> if the component renders with input mask formatting; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When the EnableMask property is enabled, it restricts users from typing unwanted characters in the input field. 
        /// It allows only eligible time format characters to be typed, providing better input validation and user experience.
        /// The mask format is based on the specified <see cref="Format"/> property.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to enable input masking:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" EnableMask="true" Format="HH:mm" Placeholder="Enter time"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool EnableMask { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the clear button is displayed in the <see cref="SfTimePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// <c>true</c> if the clear button is displayed; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When enabled, a clear button (×) appears on the right side of the input field, allowing users to quickly clear the selected time value.
        /// The clear button is only visible when the TimePicker has a value.
        /// </remarks>
        /// <example>
        /// The following example shows how to enable the clear button:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" ShowClearButton="true" Placeholder="Select time"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool ShowClearButton
        {
            get => BaseShowClearButton; set => BaseShowClearButton = value;
        }

        /// <summary>
        /// Backing field for the public <see cref="ShowClearButton"/> parameter.
        /// </summary>
        /// <exclude/>
        protected override bool BaseShowClearButton { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes such as styles, classes, and more to be applied to the root element of the <see cref="SfTimePicker{TValue}"/>.
        /// </summary>
        /// <value>
        /// A dictionary of key-value pairs representing HTML attributes. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// Additional attributes can be added by specifying an inline attribute or by using the @attributes directive.
        /// These attributes will be applied directly to the root HTML element of the TimePicker component.
        /// </remarks>
        /// <example>
        /// The following example shows how to add HTML attributes:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" HtmlAttributes="@(new Dictionary<string, object> { {"class", "custom-timepicker"}, {"title", "Select time"} })"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public Dictionary<string, object>? HtmlAttributes
        {
            get => BaseHtmlAttributes; set => BaseHtmlAttributes = value;
        }

        /// <summary>
        /// Gets or sets additional input attributes such as disabled, value, and more to be applied to the input element of the <see cref="SfTimePicker{TValue}"/>.
        /// </summary>
        /// <value>
        /// A dictionary of key-value pairs representing input attributes. The default value is an empty dictionary.
        /// </value>
        /// <remarks>
        /// If you configure both a component property and an equivalent input attribute, the component considers the property value over the attribute value.
        /// These attributes are applied directly to the underlying input HTML element.
        /// </remarks>
        /// <example>
        /// The following example shows how to add input attributes:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" InputAttributes="@(new Dictionary<string, object> { {"maxlength", "10"}, {"autocomplete", "off"} })"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? InputAttributes
        {
            get => BaseInputAttributes; set => BaseInputAttributes = value;
        }

        /// <summary>
        /// Backing field for the public <see cref="HtmlAttributes"/> parameter.
        /// </summary>
        /// <exclude/>
        protected override Dictionary<string, object>? BaseHtmlAttributes { get; set; }

        /// <summary>
        /// Backing field for the public <see cref="InputAttributes"/> parameter.
        /// </summary>
        /// <exclude/>
        protected override Dictionary<string, object>? BaseInputAttributes { get; set; }

        /// <summary>
        /// Gets or sets the width of the <see cref="SfTimePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// A string value representing the width in CSS units (px, %, em, etc.). The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// The width can be specified in various CSS units such as pixels (px), percentage (%), em, rem, etc.
        /// If not specified, the component will use its default width based on the applied theme.
        /// </remarks>
        /// <example>
        /// The following example shows how to set the width of the TimePicker:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" Width="300px" Placeholder="Select time"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Width
        {
            get => BaseWidth ?? default!; set => BaseWidth = value;
        }

        /// <summary>
        /// Backing field for the public <see cref="Width"/> parameter.
        /// </summary>
        /// <exclude/>
        protected override string? BaseWidth { get; set; } = default;

        /// <summary>
        /// Gets or sets the tab index order of the <see cref="SfTimePicker{TValue}"/> component for keyboard navigation.
        /// </summary>
        /// <value>
        /// An integer value representing the tab index. The default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// The TabIndex property specifies the order in which elements receive focus when the user navigates through the page using the Tab key.
        /// A higher value indicates that the element will receive focus later in the tab order.
        /// </remarks>
        /// <example>
        /// The following example shows how to set the tab index:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" TabIndex="1" Placeholder="Select time"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public int TabIndex
        {
            get => BaseTabIndex; set => BaseTabIndex = value;
        }

        /// <summary>
        /// Backing field for the public <see cref="TabIndex"/> parameter.
        /// </summary>
        /// <exclude/>
        protected override int BaseTabIndex { get; set; }

        /// <summary>
        /// Gets or sets the time format pattern for displaying values in the <see cref="SfTimePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// A string value representing the time format pattern. The default value is based on the current culture (typically <c>h:mm tt</c> for en-US culture).
        /// </value>
        /// <remarks>
        /// The format string follows standard .NET time format patterns. Common patterns include:
        /// <list type="bullet">
        /// <item><c>h:mm tt</c> - 12-hour format with AM/PM (e.g., 2:30 PM)</item>
        /// <item><c>HH:mm</c> - 24-hour format (e.g., 14:30)</item>
        /// <item><c>h:mm:ss tt</c> - 12-hour format with seconds</item>
        /// <item><c>HH:mm:ss</c> - 24-hour format with seconds</item>
        /// </list>
        /// If not specified, the component uses the default format based on the current culture.
        /// </remarks>
        /// <example>
        /// The following example shows how to set a 24-hour time format:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" Format="HH:mm" Placeholder="Select time (24h)"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Format { get; set; } = default!;

        /// <summary>
        /// Gets or sets the array of input formats to be used for parsing typed time values in the <see cref="SfTimePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// An array of strings representing the acceptable input formats for time values. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property allows the <see cref="SfTimePicker{TValue}"/> to interpret typed time values using a specified array of formats.
        /// The formats can include both standard and custom time formats supported in C#. When both InputFormats and <see cref="Format"/> properties are specified, the InputFormats property takes priority for parsing.
        /// If only InputFormats is specified, parsing will be attempted using the formats provided in the array.
        /// The parsing logic prioritizes the formats in the order they are specified in the InputFormats array.
        /// If a successful parsing occurs, the <see cref="SfTimePicker{TValue}"/> updates its value accordingly. Error handling is controlled by the <see cref="StrictMode"/> property.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to set multiple input formats for flexible time entry:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" InputFormats='new string[] { "hh:mm", "hh mm", "hhmm", "HH:mm" }' Placeholder="Enter time"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string[] InputFormats { get; set; } = default!;

        /// <summary>
        /// Gets or sets the keyboard shortcut configurations for the <see cref="SfTimePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// A dictionary of key-value pairs representing keyboard shortcuts and their corresponding actions. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property allows customization of keyboard shortcuts for various TimePicker actions. 
        /// It is particularly useful when using non-standard keyboards (such as German keyboards) or when you need to override default keyboard behaviors.
        /// The dictionary keys represent the keyboard shortcuts, and the values represent the corresponding actions.
        /// </remarks>
        /// <example>
        /// The following example shows how to customize keyboard shortcuts:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" KeyConfigs="@keyConfig" Placeholder="Select time"></SfTimePicker>
        /// 
        /// @code {
        ///     Dictionary<string, object> keyConfig = new Dictionary<string, object>
        ///     {
        ///         { "escape", "close" },
        ///         { "enter", "select" }
        ///     };
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public Dictionary<string, object> KeyConfigs { get; set; } = default!;

        /// <summary>
        /// Gets or sets the locale for time formatting and parsing in the <see cref="SfTimePicker{TValue}"/>.
        /// </summary>
        /// <value>
        /// A string representing the locale code (e.g., "en-US", "de-DE"). The default value is <c>null</c> (uses system locale).
        /// </value>
        /// <remarks>
        /// This property determines the culture-specific formatting of time values, including AM/PM designators
        /// and the order of hours, minutes, and seconds in the time string.
        /// </remarks>
        /// <exclude />
        internal string TimePickerLocale { get; set; } = default!;

        /// <summary>
        /// Gets or sets the maximum time value that can be selected in the <see cref="SfTimePicker{TValue}"/>.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value representing the maximum selectable time. The default value is December 31, 2099, 23:59:59.
        /// </value>
        /// <remarks>
        /// Time values in the popup list and user input that exceed this maximum value will be disabled or rejected.
        /// Only the time portion of the DateTime value is considered; the date portion is used for internal calculations.
        /// This property works in conjunction with the <see cref="Min"/> property to define a valid time range.
        /// </remarks>
        /// <example>
        /// The following example shows how to restrict time selection to business hours:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" Max="new DateTime(2023, 1, 1, 18, 0, 0)" Min="new DateTime(2023, 1, 1, 9, 0, 0)" Placeholder="Business hours only"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public DateTime Max { get; set; } = new DateTime(2099, 12, 31, 23, 59, 59);

        /// <summary>
        /// Gets or sets the minimum time value that can be selected in the <see cref="SfTimePicker{TValue}"/>.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value representing the minimum selectable time. The value must be less than or equal to the <see cref="Max"/> value. The default value is January 1, 1900, 00:00:00.
        /// </value>
        /// <remarks>
        /// Time values in the popup list and user input that are below this minimum value will be disabled or rejected.
        /// Only the time portion of the DateTime value is considered; the date portion is used for internal calculations.
        /// This property works in conjunction with the <see cref="Max"/> property to define a valid time range.
        /// </remarks>
        /// <example>
        /// The following example shows how to set a minimum time of 9:00 AM:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" Min="new DateTime(2023, 1, 1, 9, 0, 0)" Placeholder="Select time after 9 AM"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public DateTime Min { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00);

        /// <summary>
        /// Gets or sets the initial scroll position of the time popup list in the <see cref="SfTimePicker{TValue}"/>.
        /// </summary>
        /// <value>
        /// A nullable <see cref="DateTime"/> value representing the time to scroll to when the popup opens. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// When the popup opens, it will automatically scroll to the specified time position in the list.
        /// If the specified time is not present in the popup list (due to the <see cref="Step"/> interval), the popup will scroll to the nearest available time.
        /// If this property is <c>null</c> and no value is selected, the popup will show from the beginning of the time list.
        /// </remarks>
        /// <example>
        /// The following example shows how to set the initial scroll position to 2:00 PM:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" ScrollTo="new DateTime(2023, 1, 1, 14, 0, 0)" Placeholder="Select time"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public DateTime? ScrollTo { get; set; }

        /// <summary>
        /// Gets or sets the time interval in minutes between adjacent time values in the <see cref="SfTimePicker{TValue}"/> popup list.
        /// </summary>
        /// <value>
        /// An integer value representing the time interval in minutes. The default value is <c>30</c>.
        /// </value>
        /// <remarks>
        /// This property determines the granularity of time options available in the popup list.
        /// For example, a step of 30 minutes will show times like 12:00, 12:30, 1:00, 1:30, etc.
        /// <para>
        /// The step value must evenly divide 1440 (the total number of minutes in a day)
        /// to ensure uniform and predictable time entries.
        /// </para>
        /// <para>
        /// Values that do not divide 1440 (for example, 25 or 90) cause time entries to drift
        /// and may omit commonly expected times. When such values are provided, they are
        /// coerced to the nearest lower valid divisor of 1440.
        /// </para>
        /// </remarks>
        /// <example>
        /// The following example shows how to set a 15-minute interval:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" Step="15" Placeholder="Select time (15 min intervals)"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public int Step
        {
            get => _step;
            set
            {
                // Guard: non-positive values are invalid
                if (value <= 0)
                {
                    _step = 30;
                    return;
                }

                // Ensure step evenly divides a full day (1440 minutes)
                if (1440 % value != 0)
                {
                    _step = GetNearestValidStep(value);
                    return;
                }

                _step = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfTimePicker{TValue}"/> operates in strict mode for input validation.
        /// </summary>
        /// <value>
        /// <c>true</c> if the component should only allow valid time values within the specified range and reset invalid entries to the previous value; otherwise, <c>false</c> to allow invalid or out-of-range time values with error highlighting. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When StrictMode is enabled, the component will automatically validate user input and reset to the previous valid value if an invalid time is entered.
        /// When disabled, invalid time values are allowed but will be highlighted with an error class, giving users visual feedback about the invalid input.
        /// This property works in conjunction with the <see cref="Min"/> and <see cref="Max"/> properties to determine valid time ranges.
        /// </remarks>
        /// <example>
        /// The following example shows how to enable strict mode:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" StrictMode="true" Min="new DateTime(2023, 1, 1, 9, 0, 0)" Max="new DateTime(2023, 1, 1, 17, 0, 0)" Placeholder="Business hours only"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool StrictMode { get; set; }

        /// <summary>
        /// Gets or sets the z-index CSS property value of the <see cref="SfTimePicker{TValue}"/> popup element.
        /// </summary>
        /// <value>
        /// An integer value representing the z-index of the popup. The default value is <c>1000</c>.
        /// </value>
        /// <remarks>
        /// The z-index property specifies the stack order of the popup element. Higher values appear in front of elements with lower values.
        /// This property is useful when you need to ensure the TimePicker popup appears above other elements on the page, 
        /// especially in complex layouts with overlapping elements.
        /// </remarks>
        /// <example>
        /// The following example shows how to set a higher z-index for the popup:
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" ZIndex="2000" Placeholder="Select time"></SfTimePicker>
        /// ]]></code>
        /// </example>
        [Parameter]
        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the parent component of the TimePicker when used within an InPlaceEditor.
        /// </summary>
        /// <value>
        /// A dynamic object representing the parent InPlaceEditor component. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property is automatically set when the TimePicker is used as a child component within an InPlaceEditor component.
        /// It enables communication between the TimePicker and its parent InPlaceEditor for coordinated behavior and styling.
        /// </remarks>
        /// <exclude />
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic? TimePickerParent { get; set; }
    }

    /// <summary>
    /// Represents the client-side properties for the <see cref="SfTimePicker{TValue}"/> component.
    /// </summary>
    /// <typeparam name="TValue">
    /// Specifies the type of the time value that the TimePicker component will handle.
    /// </typeparam>
    /// <remarks>
    /// This internal class contains properties used for client-side communication and state management 
    /// of the TimePicker component. It includes configuration, formatting, and state properties 
    /// that are synchronized between server and client sides.
    /// </remarks>
    /// <exclude/>
    internal class TimePickerClientProps<TValue>
    {
        /// <summary>
        /// Gets or sets a bool value <see cref="EnableRtl"/> to enable or disable rendering <see cref="SfTimePicker{TValue}"/> in right to left direction.
        /// </summary>
        /// <exclude/>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies a boolean value that determines whether the component is disabled and prevents user interaction.
        /// </summary>
        /// <exclude/>
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ZIndex"/> value of the <see cref="SfTimePicker{TValue}"/> popup element.
        /// </summary>
        /// <exclude/>
        public int ZIndex { get; set; }

        /// <summary>
        /// Customizes the <see cref="KeyConfigs"/> in <see cref="SfTimePicker{TValue}"/>. 
        /// </summary>
        /// <exclude/>
        public Dictionary<string, object> KeyConfigs { get; set; } = default!;

        /// <summary>
        /// Gets or sets the <see cref="Value"/> of the <see cref="SfTimePicker{TValue}"/> component. 
        /// </summary>
        /// <exclude/>      
        public TValue Value { get; set; } = default!;

        /// <summary>
        /// Gets or sets the <see cref="Width"/> of the <see cref="SfTimePicker{TValue}"/> component.
        /// </summary>
        /// <exclude/>
        public string Width { get; set; } = default!;

        /// <summary>
        /// Gets or sets the scroll bar position.
        /// </summary>
        /// <exclude/>
        public DateTime? ScrollTo { get; set; }

        /// <summary>
        /// Gets or sets the time interval.
        /// </summary>
        /// <exclude/>     
        public int Step { get; set; }

        /// <summary>
        /// Gets or sets mask rendering in the <see cref="SfTimePicker{TValue}"/> component.
        /// </summary>
        /// <exclude/>
        public bool EnableMask { get; set; }

        /// <summary>
        /// Gets or sets the required time <see cref="Format"/> of value that is to be displayed in component.
        /// <para>By default, the format is based on the culture.</para>
        /// </summary>
        /// <exclude/>
        public string Format { get; set; } = default!;

        /// <summary>  
        /// Gets or sets the array of input formats to be used for parsing time values in the <see cref="SfTimePicker{TValue}"/> component.  
        /// </summary>
        /// <value>
        /// An array of strings representing the acceptable input formats for time values. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// By default, the input formats is <c>null</c>.
        /// You can set the input formats to "InputFormats='new string[] { "hh:mm", "hh mm", "hhmm" }'".
        /// </remarks>
        /// <exclude/>
        [Parameter]
        public string[] InputFormats { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfTimePicker{TValue}"/> component is currently focused.
        /// </summary>
        /// <value>
        /// <c>true</c> if the component is focused; otherwise, <c>false</c>.
        /// </value>
        /// <exclude/>
        public bool IsFocused { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfTimePicker{TValue}"/> component has been rendered.
        /// </summary>
        /// <value>
        /// <c>true</c> if the component has been rendered; otherwise, <c>false</c>.
        /// </value>
        /// <exclude/>
        public bool IsRendered { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DayName"/> string array that contains the culture-specific full names of the days of the week.
        /// </summary>
        /// <exclude/>
        public string[] DayName { get; set; } = default!;

        /// <summary>
        /// Gets or sets a <see cref="MonthName"/> array of type System.String containing the culture-specific full names of the months.
        /// </summary>
        /// <exclude/>
        public string[] MonthName { get; set; } = default!;

        /// <summary>
        /// Gets or sets a <see cref="DayAbbreviatedName"/> array of type System.String containing the culture-specific abbreviated names of the days of the week.
        /// </summary>
        /// <exclude/>
        public string[] DayAbbreviatedName { get; set; } = default!;

        /// <summary>
        /// Gets or sets a <see cref="MonthAbbreviatedName"/> string array that contains the culture-specific abbreviated names of the months.
        /// </summary>
        /// <exclude/>
        public string[] MonthAbbreviatedName { get; set; } = default!;

        /// <summary>
        ///  Gets or sets the <see cref="DayPeriod"/> string designator for hours that are <c>post meridiem</c> (after noon) or <c>ante meridiem</c> (before noon).
        /// </summary>
        /// <exclude/>
        public string[] DayPeriod { get; set; } = default!;

        /// <summary>
        /// Gets or sets the default <see cref="MaskPlaceholder"/> values based on culture's value.
        /// </summary>
        /// <exclude/>
        public Dictionary<string, string> MaskPlaceholderDictionary { get; set; } = [];

        /// <summary>
        /// Gets or sets the <see cref="FloatLabelType"/> behavior of the TextBox that the <see cref="Placeholder"/> text floats above the TextBox based on the following values.
        /// </summary>
        /// <value>
        /// Accepts the string value. A string value representing the behavior of the floating label in the TextBox.
        /// </value>
        /// <exclude/>
        public string FloatLabelType { get; set; } = default!;

        /// <summary>
        /// Gets or sets the text that is shown as a hint or <see cref="Placeholder"/> until the user focuses or enter a value in TimePicker.
        /// </summary>
        /// <exclude/>
        public string Placeholder { get; set; } = default!;

        /// <summary>
        /// Gets or sets the <see cref="ValueString"/> of the <see cref="SfTimePicker{TValue}"/> in string type. The value is parsed based on the culture specific time format.
        /// </summary>
        /// <exclude/>
        public string ValueString { get; set; } = default!;

        /// <summary>
        /// Gets or sets the culture's <see cref="Offset"/> value.
        /// </summary>
        /// <exclude/>
        public string Offset { get; set; } = default!;

        /// <summary>
        /// Gets or sets the Navigated value.
        /// </summary>
        /// <exclude/>
        public bool Navigated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfTimePicker{TValue}"/> component has lost focus.
        /// </summary>
        /// <value>
        /// <c>true</c> if the component has been blurred (lost focus); otherwise, <c>false</c>.
        /// </value>
        /// <exclude/>
        public bool IsBlurred { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfTimePicker{TValue}"/> is in read-only mode, preventing user interaction.
        /// </summary>
        /// <value>
        /// <c>true</c> if the TimePicker value cannot be edited; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When the read-only mode is enabled, users cannot edit the time value through typing or popup interaction.
        /// The popup cannot be opened when this property is set to <c>true</c>.
        /// </remarks>
        /// <exclude/>
        public bool Readonly { get; set; }
    }
}
