using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Inputs;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// Represents the Syncfusion DatePicker component, a graphical user interface control that enables users to select or enter a date value interactively or via direct input.
    /// </summary>
    /// <remarks>
    /// The <see cref="SfDatePicker{TValue}"/> supports various display, formatting, and accessibility options, including custom attributes, input masks, floating labels, date parsing formats, placeholder text, and more. This component is intended for use in forms and user interfaces that require easy and robust date selection.
    /// </remarks>
    /// <example>
    /// Basic usage:
    /// <code><![CDATA[
    /// <SfDatePicker TValue="DateTime?" Placeholder="Select a date" />
    /// ]]></code>
    /// </example>
    public partial class SfDatePicker<TValue> : CalendarBase<TValue>
    {
        private string InternalCssClass { get; set; } = string.Empty;
        private string? InternalFormat { get; set; }
        private string[]? InternalInputFormats { get; set; }
        private bool InternalReadOnly { get; set; }

        internal bool DateStrictMode { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that determines whether the <see cref="SfDatePicker{TValue}"/> allows the user to edit the value by typing in the input field, in addition to using the date picker popup.
        /// </summary>
        /// <value>
        /// <c>true</c> if the input field can be edited by the user; otherwise, <c>false</c>. Default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>false</c>, the user can only select a date using the picker popup. Editing the value via direct text entry is disabled.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" AllowEdit="false" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool AllowEdit { get; set; } = true;

        /// <summary>
        /// Gets or sets a boolean value indicating whether the date picker popup should be displayed in full screen mode on mobile or tablet devices.
        /// </summary>
        /// <value>
        /// <c>true</c> enables full screen layout for the popup; otherwise, <c>false</c>. Default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Full screen layout is applicable only for mobile and tablet devices, providing a better user experience on small screens.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" FullScreen="true" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool FullScreen { get; set; }

        /// <summary>
        /// Gets or sets a boolean value to determine whether the popup calendar opens automatically when the input element receives focus.
        /// </summary>
        /// <value>
        /// <c>true</c> if the popup should open on input focus; otherwise, <c>false</c>. Default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Enabling this property can enhance user experience by triggering the date picker popup as soon as the user clicks or tabs into the input element. This is especially useful in scenarios that require quick, single-step date selection.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" OpenOnFocus="true" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool OpenOnFocus { get; set; }

        /// <summary>
        /// Gets or sets a boolean value to enable or disable the use of an input mask in the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// <c>true</c> enables input mask rendering; otherwise, <c>false</c>. Default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// The input mask property restricts input to characters allowed by the date format, preventing unwanted characters from being typed. This helps ensure only valid date values are entered.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" EnableMask="true" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool EnableMask { get; set; }

        /// <summary>
        /// Gets or sets the floating label behavior for the <see cref="SfDatePicker{TValue}"/> component, determining how the placeholder text is displayed.
        /// </summary>
        /// <value>
        /// A value of the <see cref="FloatLabelType"/> enum that specifies the float label mode. Default is <see cref="FloatLabelType.Never"/>.
        /// </value>
        /// <remarks>
        /// This property allows you to control whether the placeholder text always floats, never floats, or floats only when the input is focused or contains a value.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" FloatLabelType="FloatLabelType.Auto" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public FloatLabelType FloatLabelType
        {
            get => BaseFloatLabelType; set => BaseFloatLabelType = value;
        }

        /// <exclude />
        protected override FloatLabelType BaseFloatLabelType { get; set; }

        /// <summary>
        /// Gets or sets the display format string used to present the selected date value in the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// A <c>string</c> representing the date format. The default format is culture-specific (<c>M/d/yyyy</c> for en-US).
        /// </value>
        /// <remarks>
        /// By default, the component's format string depends on the current culture. To customize the display, set your desired format string.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" Format="dd/MM/yyyy" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an array of input format strings used to parse input values in the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// An array of <c>string</c> values representing acceptable input formats. Default is <c>null</c> (culture-based).
        /// </value>
        /// <remarks>
        /// When both <c>InputFormats</c> and <c>Format</c> properties are specified, <c>InputFormats</c> takes precedence when parsing entered values. Supported C# standard and custom date formats are accepted. Parsing will try each pattern in order, successful parsing updates the value. For invalid input, the <c>StrictMode</c> property determines error handling: in strict mode, the value resets to previous if invalid; otherwise, it is highlighted with an error CSS class.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" InputFormats='new string[] { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd" }' />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string[] InputFormats { get; set; } = default!;

        /// <summary>
        /// Gets or sets additional HTML attributes, such as style or class, that are added to the root element of the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> of <c>string</c> and <c>object</c> containing custom HTML attributes.
        /// </value>
        /// <remarks>
        /// Additional attributes can be specified inline or via the <c>@attributes</c> directive.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" HtmlAttributes="@CustomAttributes" />
        /// @code{
        ///     Dictionary<string, object> CustomAttributes = new Dictionary<string, object>
        ///     {
        ///         { "title", "Select a Date" }
        ///     };
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public Dictionary<string, object>? HtmlAttributes
        {
            get => BaseHtmlAttributes; set => BaseHtmlAttributes = value;
        }

        /// <summary>
        /// Gets or sets additional input attributes such as <c>disabled</c> or <c>value</c> for the root input element of the <see cref="SfDatePicker{TValue}"/>.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> of <c>string</c> and <c>object</c> containing additional input attributes.
        /// </value>
        /// <remarks>
        /// If both property-specific and HTML attribute values are set for the same property, the property value takes precedence over the input attribute.
        /// </remarks>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? InputAttributes
        {
            get => BaseInputAttributes; set => BaseInputAttributes = value;
        }
        /// <summary>
        /// Backing storage for additional HTML attributes applied to the component's root element.
        /// </summary>
        /// <value>
        /// A dictionary of attribute name/value pairs that will be rendered on the DatePicker root element. Use the
        /// public <see cref="HtmlAttributes"/> parameter to read or set these values.
        /// </value>
        /// <exclude />
        protected override Dictionary<string, object>? BaseHtmlAttributes { get; set; }

        /// <summary>
        /// Backing storage for additional attributes applied specifically to the input element of the DatePicker.
        /// </summary>
        /// <value>
        /// A dictionary of input attribute name/value pairs (for example, <c>disabled</c>, <c>value</c>, <c>aria-*</c> attributes)
        /// that will be applied to the rendered input element. Use the public <see cref="InputAttributes"/> parameter to
        /// interact with these values.
        /// </value>
        /// <exclude />
        protected override Dictionary<string, object>? BaseInputAttributes { get; set; }

        /// <summary>
        /// Gets or sets the placeholder text displayed in the input box until the user enters a value or focuses the <see cref="SfDatePicker{TValue}"/>.
        /// </summary>
        /// <value>
        /// A <c>string</c> representing the placeholder text. Default value is <c>string.Empty</c>.
        /// </value>
        /// <remarks>
        /// Placeholder display behavior may depend on the <see cref="FloatLabelType"/> property value.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" Placeholder="Select your birthday..." />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Placeholder
        {
            get => BasePlaceholder ?? default!; set => BasePlaceholder = value;
        }
        /// <summary>
        /// Backing storage for the public <see cref="Placeholder"/> parameter.
        /// </summary>
        /// <value>
        /// The placeholder text displayed in the input when it has no value. This value may be affected by
        /// the <see cref="FloatLabelType"/> setting which can change how and when the placeholder is shown.
        /// </value>
        /// <exclude />
        protected override string BasePlaceholder { get; set; } = default!;

        /// <summary>
        /// Gets or sets a boolean value indicating whether text entry is read-only in the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// <c>true</c> if the value cannot be edited; otherwise, <c>false</c>. Default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// When <c>Readonly</c> is enabled, the input field cannot be changed and the date picker popup will not open.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" Readonly="true" />
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
        /// <value>
        /// When <c>true</c> the input element is rendered as read-only and user text entry is disallowed; when <c>false</c>
        /// the input may accept typed values (subject to <see cref="AllowEdit"/> and other settings).
        /// </value>
        /// <exclude />
        protected override bool BaseReadOnly { get; set; }

        /// <summary>
        /// Indicates whether the input should be treated as a read-only input element for rendering purposes.
        /// </summary>
        /// <value>
        /// <c>true</c> when the rendered input element should include read-only attributes and styling; otherwise <c>false</c>.
        /// </value>
        /// <exclude />
        protected override bool BaseIsReadOnlyInput { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether a clear button is shown in the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// <c>true</c> displays the clear button; otherwise, <c>false</c>. Default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When the clear button is displayed, users can quickly reset the input value to its default state.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" ShowClearButton="true" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool ShowClearButton
        {
            get => BaseShowClearButton; set => BaseShowClearButton = value;
        }

        /// <summary>
        /// Backing property for the public <see cref="ShowClearButton"/> parameter.
        /// </summary>
        /// <value>
        /// When <c>true</c>, the DatePicker will render a clear button that allows users to reset the input value.
        /// When <c>false</c>, the clear button is not rendered.
        /// </value>
        /// <exclude />
        protected override bool BaseShowClearButton { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that determines whether only valid date values within a specified range can be entered. If set to <c>true</c>, invalid or out-of-range values are disallowed or reset.
        /// </summary>
        /// <value>
        /// <c>true</c> prevents entry of invalid or out-of-range values; otherwise, <c>false</c> allows invalid values. Default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// In strict mode, invalid dates revert to the previous valid value. Otherwise, input errors are visually highlighted.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" StrictMode="true" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool StrictMode { get; set; }

        /// <summary>
        /// Gets or sets the width of the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// A <c>string</c> value specifying the width. Default value is <c>null</c> (auto width).
        /// </value>
        /// <remarks>
        /// Set this property to specify a custom width for the control in CSS units.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" Width="280px" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Width
        {
            get => BaseWidth ?? default!; set => BaseWidth = value;
        }

        /// <summary>
        /// Backing property for the public <see cref="Width"/> parameter.
        /// </summary>
        /// <value>
        /// A CSS size string (for example, "280px" or "50%") that controls the rendered width of the DatePicker input and popup.
        /// When <c>null</c> the control will size automatically based on layout.
        /// </value>
        /// <exclude />
        protected override string? BaseWidth { get; set; }

        /// <summary>
        /// Gets or sets the CSS Z-Index value for the popup associated with the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// An <c>int</c> representing the ZIndex of the popup. Default value is 1000.
        /// </value>
        /// <remarks>
        /// The ZIndex controls the stacking order of the popup on the page. Increasing the value brings the popup above more elements.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" ZIndex="2000" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the tab index order of the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <value>
        /// An <c>int</c> indicating the tab order. Default is 0.
        /// </value>
        /// <remarks>
        /// Set this property to control the keyboard tab navigation order for accessibility.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" TabIndex="1" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public int TabIndex
        {
            get => BaseTabIndex; set => BaseTabIndex = value;
        }

        /// <summary>
        /// Backing property for the public <see cref="TabIndex"/> parameter.
        /// </summary>
        /// <value>
        /// An integer that determines the component's position in the page tab order. A value of <c>0</c> places the
        /// component in the natural tab sequence; positive values move it later in the order.
        /// </value>
        /// <exclude />
        protected override int BaseTabIndex { get; set; }

        /// <summary>
        /// Parent component of the DatePicker instance, provided via cascading parameter.
        /// </summary>
        /// <value>
        /// A reference to the parent component that hosts this DatePicker, if any.
        /// </value>
        /// <remarks>
        /// Primarily used internally for integration with the InPlaceEditor. Not intended for direct use.
        /// </remarks>
        /// <exclude/>
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic? DatePickerParent { get; set; }
    }

    /// <summary>
    /// Specifies the client properties of datepicker.
    /// </summary>
    /// <typeparam name="TValue">Gets or sets the type of DatePickerClientProps.</typeparam>
    internal sealed class DatePickerClientProps<TValue>
    {
        /// <summary>
        /// Gets or sets the boolean value to <see cref="Readonly"/> whether the <see cref="SfDatePicker{TValue}"/> allows the user to change the text.
        /// </summary>
        /// <exclude/>
        public bool Readonly { get; set; }

        /// <summary>
        /// Gets or sets mask rendering in the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <exclude/>
        public bool EnableMask { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that determines whether the <see cref="SfDatePicker{TValue}"/>
        /// component is disabled and prevents user interaction.
        /// </summary>
        /// <exclude/>
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets the global culture and localization of the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <exclude/>     
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a bool value <see cref="EnableRtl"/> to enable or disable rendering DatePicker in right to left direction.
        /// </summary>
        /// <exclude/>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ZIndex"/> value of the DatePicker popup element.
        /// </summary>
        /// <exclude/>
        public int ZIndex { get; set; }

        /// <summary>
        /// Customizes the key actions in <see cref="Calendars"/>. 
        /// </summary>
        /// <exclude/>
        public Dictionary<string, object> KeyConfigs { get; set; } = default!;

        /// <summary>
        /// Gets or sets a boolean value to <see cref="ShowClearButton"/> this indicates whether the clear button is displayed in <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <exclude/>
        public bool ShowClearButton { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Value"/> of the <see cref="SfDatePicker{TValue}"/> component. The value is parsed based on the culture specific Date format.
        /// </summary>
        /// <exclude/>    
        public TValue Value { get; set; } = default!;

        /// <summary>
        /// Gets or sets a boolean value to <see cref="AllowEdit"/> whether the <see cref="SfDatePicker{TValue}"/> allows user to change the value via typing. 
        /// </summary>
        /// <exclude/>
        public bool AllowEdit { get; set; }

        /// <summary>
        /// Sets the maximum level of view such as month, year, and decade in the <see cref="Calendars"/>.
        /// <para><see cref="Depth"/> view should be smaller than the start view to restrict its view navigation.</para>
        /// </summary>
        /// <exclude/>
        public string Depth { get; set; } = default!;

        /// <summary>
        /// Gets or sets the <see cref="Width"/> of the <see cref="SfDatePicker{TValue}"/> component.
        /// </summary>
        /// <exclude/>
        public string Width { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the <see cref="IsDatePopup"/> value to open and close the date popup.
        /// </summary>
        /// <exclude/>
        public bool IsDatePopup { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Format"/> of the value that to be displayed in component.
        /// <para>By default, the format is based on the culture.</para>
        /// <para>You can set the format to "format:'dd/MM/yyyy hh:mm'".</para>
        /// </summary>
        /// <exclude/>
        public string Format { get; set; } = string.Empty;

        /// <summary>  
        /// Gets or sets the array of input formats to be used for parsing date values in the <see cref="SfDatePicker{TValue}"/> component.
        /// <para>By default, the input formats is <c>null</c>.</para>
        /// <para>You can set the input formats to "InputFormats='new string[] { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd" }'".</para>
        /// </summary>
	    /// <exclude/>
        public string[] InputFormats { get; set; } = default!;

        /// <summary>
        /// Gets or sets the <see cref="IsFocused"/> value of <see cref="SfDatePicker{TValue}"/> Component format focused or not.
        /// </summary>        
        /// <exclude/>
        public bool IsFocused { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IsRendered"/> value of <see cref="SfDatePicker{TValue}"/> Component rendered or not.
        /// </summary>
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
        /// Gets or sets the <see cref="MaskPlaceholderDictionary"/> values based on culture's value.
        /// </summary>        
        /// <exclude/>
        public Dictionary<string, string> MaskPlaceholderDictionary { get; set; } = [];

        /// <summary>
        /// Gets or sets the <see cref="FloatLabelType"/> behavior of the <see cref="SfDatePicker{TValue}"/> that the placeholder text floats above the DatePicker based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the DatePicker when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the DatePicker.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the DatePicker after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <exclude/>
        public string FloatLabelType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text that is shown as a hint or <see cref="Placeholder"/> until the user focuses or enter a value in DatePicker.
        /// </summary>
        /// <exclude/>
        public string Placeholder { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the <see cref="ValueString"/> of the DatePicker in string type. The value is parsed based on the culture specific time format.
        /// </summary>        
        /// <exclude/>
        public string ValueString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the culture's <see cref="Offset"/> value.
        /// </summary>
        /// <exclude/>
        public string Offset { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the <see cref="IsBlurred"/> value of <see cref="FocusOutEventArgs"/> of the Component rendered or not.
        /// </summary>
        /// <exclude/>
        public bool IsBlurred { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MinMax"/> value indicating whether the value selection is out of the <see cref="SfDatePicker{TValue}.MIN"/> and <see cref="SfDatePicker{TValue}.MAX"/> property's range.
        /// </summary>
        /// <exclude/>
        public bool MinMax { get; set; }
    }
}
