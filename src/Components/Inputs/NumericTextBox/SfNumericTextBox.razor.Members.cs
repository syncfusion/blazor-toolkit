using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// The NumericTextBox is used to get the number inputs from the user. The input values can be incremented or decremented by a predefined step value.
    /// </summary>
    public partial class SfNumericTextBox<TValue> : SfInputBase<TValue>
    {
        /// <summary>
        /// Stores the custom CSS class value applied to the component's container.
        /// </summary>
        private string ContainerCssClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the currency code to use in currency formatting. Possible values are the ISO 4217 currency codes, such as <c>USD</c> for the US dollar and <c>EUR</c> for the euro.
        /// </summary>
        /// <value>
        /// A string value representing the ISO 4217 currency code. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property enables currency formatting for the numeric values displayed in the <see cref="SfNumericTextBox{TValue}"/>. 
        /// When set, the component will format numbers according to the specified currency's conventions.
        /// Common examples include "USD" for US Dollar, "EUR" for Euro, "GBP" for British Pound, etc.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="decimal?" Currency="USD" Placeholder="Enter amount">
        /// </SfNumericTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string? Currency { get; set; }

        /// <summary>
        /// Gets or sets the floating label behavior of the <see cref="SfNumericTextBox{TValue}"/>. The <see cref="Placeholder"/> text acts as a label.
        /// </summary>
        /// <value>
        /// One of the <see cref="FloatLabelType"/> enumeration. The default value is <see cref="FloatLabelType.Never"/>
        /// </value>
        /// <remarks>
        /// If the <c>FloatLabelType</c> is <c>Never</c>, the placeholder text does not float as a label.
        /// If the <c>FloatLabelType</c> is <c>Auto</c>, the placeholder text will float above the Numeric TextBox component as a label after focusing it.
        /// If the <c>FloatLabelType</c> is <c>Always</c>, the placeholder text is displayed as a label above the Numeric TextBox component.
        /// </remarks>
        /// <example>
        /// In the following code example, set the floating label to <c>Auto</c>.
        /// <code><![CDATA[
        ///   <SfNumericTextBox TValue="int?" Placeholder="Enter the value" FloatLabelType="FloatLabelType.Auto">
        ///   </SfNumericTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public FloatLabelType FloatLabelType
        {
            get => BaseFloatLabelType; set => BaseFloatLabelType = value;
        }

        /// <summary>
        /// Overrides the base floating label type for the <see cref="SfNumericTextBox{TValue}"/>.
        /// </summary>
        /// <exclude/>
        protected override FloatLabelType BaseFloatLabelType { get; set; }

        /// <summary>
        /// Gets or sets the placeholder text that is displayed when the <see cref="SfNumericTextBox{TValue}"/> is empty and is removed when focused.
        /// </summary>
        /// <value>
        /// The text that is displayed when the TextBox has no value. The default value is <c>string.Empty</c>.
        /// </value>
        /// <remarks>
        /// The property depends on the <see cref="FloatLabelType"/> property. The placeholder text acts as a label.
        /// </remarks>
        [Parameter]
        public string? Placeholder
        {
            get => BasePlaceholder; set => BasePlaceholder = value;
        }

        /// <summary>
        /// Overrides the base placeholder text for the <see cref="SfNumericTextBox{TValue}"/>.
        /// </summary>
        /// <exclude/>
        protected override string? BasePlaceholder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfNumericTextBox{TValue}"/> allows users to change the text.
        /// </summary>
        /// <value>
        /// <c>true</c>, if the TextBox component cannot be edited. Otherwise, <c>false</c>.
        /// </value>
        [Parameter]
        public bool Readonly
        {
            get => BaseReadOnly; set => BaseReadOnly = value;
        }

        /// <summary>
        /// Overrides the base read-only state for the <see cref="SfNumericTextBox{TValue}"/>.
        /// </summary>
        /// <exclude/>
        protected override bool BaseReadOnly { get; set; }

        /// <summary>
        /// Overrides the base input read-only state for the <see cref="SfNumericTextBox{TValue}"/>.
        /// </summary>
        /// <exclude/>
        protected override bool BaseIsReadOnlyInput { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the clear button is displayed in the <see cref="SfNumericTextBox{TValue}"/> component.
        /// </summary>
        /// <value>
        /// <c>true</c> if the clear button should be shown; otherwise, <c>false</c>. The default is <c>false</c>.
        /// </value>
        [Parameter]
        public bool ShowClearButton
        {
            get => BaseShowClearButton; set => BaseShowClearButton = value;
        }

        /// <summary>
        /// Overrides the base clear button setting for the <see cref="SfNumericTextBox{TValue}"/>.
        /// </summary>
        /// <exclude/>
        protected override bool BaseShowClearButton { get; set; }

        /// <summary>
        /// Gets or sets the width of the <see cref="SfNumericTextBox{TValue}"/> component.
        /// </summary>
        /// <value>
        /// The preferred width in pixels or percentage value. The default value is <c>100%</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        ///   <SfNumericTextBox TValue="int?" Width="200px">
        ///   </SfNumericTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string? Width
        {
            get => BaseWidth; set => BaseWidth = value;
        }

        /// <summary>
        /// Overrides the base width setting for the <see cref="SfNumericTextBox{TValue}"/>.
        /// </summary>
        /// <exclude/>
        protected override string? BaseWidth { get; set; }

        /// <summary>
        /// Gets or sets the tab order of the <see cref="SfNumericTextBox{TValue}"/> component.
        /// </summary>
        /// <value>
        /// An integer value representing the tab index of the component.
        /// </value>
        [Parameter]
        public int TabIndex
        {
            get => BaseTabIndex; set => BaseTabIndex = value;
        }

        /// <summary>
        /// Overrides the base tab index for the <see cref="SfNumericTextBox{TValue}"/>.
        /// </summary>
        /// <exclude/>
        protected override int BaseTabIndex { get; set; }

        /// <summary>
        /// Gets or sets a collection of additional attributes such as styles, class, and more that will be applied to the <see cref="SfNumericTextBox{TValue}"/> component.
        /// </summary>
        /// <value>
        /// The value as dictionary collection. The default value is <c>null</c>
        /// </value>
        /// <remarks>
        /// If you configured both property and equivalent html attributes, then the component considers the property value.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int?" HtmlAttributes="@CustomAttribute">
        /// </SfNumericTextBox>
        /// @code{
        ///     Dictionary<string, object> CustomAttribute = new Dictionary<string, object>()
        ///     {
        ///         { "title", "Please enter the unit" }
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
        /// Overrides the base HTML attributes for the <see cref="SfNumericTextBox{TValue}"/>.
        /// </summary>
        /// <exclude/>
        protected override Dictionary<string, object>? BaseHtmlAttributes { get; set; }

        /// <summary>
        /// Gets or sets a collection of additional inputs attributes such as disabled, value, and more that will be applied to the TextBox component.        
        /// </summary>
        /// <value>
        /// The value as dictionary collection. The default value is <c>null</c>
        /// </value>
        /// <remarks>
        /// Additional attributes can be added by specifying as inline attributes or by specifying <c>@attributes</c> directive.
        /// If you configured both property and equivalent html attributes, then the component considers the property value.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int?" Placeholder="Enter the PIN" @attributes="@CustomAttribute">
        /// </SfNumericTextBox>
        /// @code{
        ///     Dictionary<string, object> CustomAttribute = new Dictionary<string, object>()
        ///     {
        ///         { "maxlength", "4" }
        ///     };
        /// }
        /// ]]></code>
        /// </example>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes
        {
            get => BaseInputAttributes; set => BaseInputAttributes = value;
        }

        /// <summary>
        /// Overrides the base input attributes for the <see cref="SfNumericTextBox{TValue}"/>.
        /// </summary>
        /// <exclude/>
        protected override Dictionary<string, object> BaseInputAttributes { get; set; } = [];

        /// <summary>
        /// Gets or sets the number of decimal places to display for numeric values.
        /// </summary>
        /// <value>
        /// An integer value representing the number of decimal places. Use <c>null</c> to display the default number of decimal places. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property controls the precision of decimal values displayed in the <see cref="SfNumericTextBox{TValue}"/>.
        /// When set to a specific value, the component will format numbers to show exactly that many decimal places.
        /// If set to <c>null</c>, the component will use the default decimal precision based on the data type and format string.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="decimal?" Decimals="3" Placeholder="Enter value">
        /// </SfNumericTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public int? Decimals { get; set; }

        /// <summary>
        /// Stores the previous value of the <see cref="Decimals"/> property for change detection.
        /// </summary>
        private int? PreviousDecimals { get; set; }


        /// <summary>
        /// Stores the previous value of the `Disabled` property for change detection.
        /// </summary>
        private bool PreviousDisabled { get; set; }

        /// <summary>
        /// Gets or sets the format string used to display numeric values.
        /// </summary>
        /// <value>
        /// A string value representing the format pattern. The default value is <c>n2</c> which represents a number with two decimal places.
        /// </value>
        /// <remarks>
        /// You can customize the format string according to your requirements. The format string follows the standard format specifiers used in .NET formatting. For example, <c>n3</c> would display a number with three decimal places.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        ///   <SfNumericTextBox TValue="decimal?" Placeholder="Enter the value" Format="n3" >
        ///   </SfNumericTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Format { get; set; } = "n2";

        /// <summary>
        /// Stores the previous value of the <see cref="Format"/> property for change detection.
        /// </summary>
        private string? PreviousFormat { get; set; }

        /// <summary>
        /// Stores the previous value of the <see cref="Readonly"/> property for change detection.
        /// </summary>
        private bool PreviousReadonly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display spin buttons for incrementing and decrementing the numeric value.
        /// </summary>
        /// <value>
        /// <c>true</c> if spin buttons should be shown; otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// When enabled, the <see cref="SfNumericTextBox{TValue}"/> displays up and down arrow buttons that allow users to increment or decrement the numeric value.
        /// The spin buttons provide an intuitive way for users to adjust values without typing, especially useful for precise value adjustments.
        /// The increment/decrement amount is controlled by the <see cref="Step"/> property.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int?" ShowSpinButton="true" Step="5">
        /// </SfNumericTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool ShowSpinButton
        {
            get => SpinButton; set => SpinButton = value;
        }

        /// <summary>
        /// Stores the previous value of the <see cref="ShowSpinButton"/> property for change detection.
        /// </summary>
        private bool PreviousShowSpinButton { get; set; }

        /// <summary>
        /// Gets or sets the increment or decrement value for changing the numeric value.
        /// </summary>
        /// <value>
        /// The step value that determines how much the numeric value changes when interacting with the component. The default value varies based on the data type.
        /// </value>
        /// <remarks>
        /// This property defines the amount by which the value increases or decreases when users interact with the spin buttons, use keyboard arrow keys, or scroll the mouse wheel.
        /// The step value should be appropriate for the expected range and precision of values in your application.
        /// For example, use smaller step values for decimal numbers requiring precision, and larger step values for whole numbers.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int?" Placeholder="Enter the value" Step="5">
        /// </SfNumericTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public TValue Step { get; set; } = GetNumericValue<TValue>(STEP);

        /// <summary>
        /// Stores the previous value of the <see cref="Step"/> property for change detection.
        /// </summary>
        private TValue? PreviousStep { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the component operates in strict mode for value validation.
        /// </summary>
        /// <value>
        /// <c>true</c> if input values are restricted between the minimum and maximum range; otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// When <see cref="StrictMode"/> is enabled (<c>true</c>), the input value will be automatically constrained between the <see cref="Min"/> and <see cref="Max"/> values.
        /// If a user enters a value outside this range, it will be corrected to the nearest boundary value when the component loses focus.
        /// When disabled (<c>false</c>), the component allows any value, even if it falls outside the specified range. In this case, an error class is applied to highlight invalid values visually.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int?" Min="1" Max="100" StrictMode="true">
        /// </SfNumericTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool StrictMode { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to perform decimal validation during typing.
        /// </summary>
        /// <value>
        /// <c>true</c> if decimal validation should be performed during typing; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When enabled, this property validates decimal input in real-time as the user types, preventing invalid decimal characters from being entered.
        /// This provides immediate feedback and ensures that only valid decimal values can be typed into the <see cref="SfNumericTextBox{TValue}"/>.
        /// When disabled, validation occurs only when the component loses focus or when the value is programmatically set.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="decimal?" ValidateDecimalOnType="true">
        /// </SfNumericTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool ValidateDecimalOnType { get; set; }

        /// <summary>
        /// Stores the previous value of the <see cref="ValidateDecimalOnType"/> property for change detection.
        /// </summary>
        private bool PreviousValidateDecimalOnType { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowable value for the input.
        /// </summary>
        /// <value>
        /// The maximum allowable value for the input. The default value is determined by the maximum possible value for the data type.
        /// </value>
        /// <remarks>
        /// This property defines the upper boundary for values that can be entered in the <see cref="SfNumericTextBox{TValue}"/>.
        /// When <see cref="StrictMode"/> is enabled, values exceeding this maximum will be automatically corrected to this maximum value.
        /// When <see cref="StrictMode"/> is disabled, values can exceed this maximum but will be flagged with an error state.
        /// The spin buttons and keyboard interactions respect this maximum limit.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int?" Placeholder="Enter the value" Max="10">
        /// </SfNumericTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public TValue Max { get; set; } = GetNumericValue<TValue>(MAX_VALUE);

        /// <summary>
        /// Stores the previous value of the <see cref="Max"/> property for change detection.
        /// </summary>
        private TValue? PreviousMax { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowable value for the input.
        /// </summary>
        /// <value>
        /// The minimum allowable value for the input. The default value is determined by the minimum possible value for the data type.
        /// </value>
        /// <remarks>
        /// This property defines the lower boundary for values that can be entered in the <see cref="SfNumericTextBox{TValue}"/>.
        /// When <see cref="StrictMode"/> is enabled, values below this minimum will be automatically corrected to this minimum value.
        /// When <see cref="StrictMode"/> is disabled, values can go below this minimum but will be flagged with an error state.
        /// The spin buttons and keyboard interactions respect this minimum limit.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int?" Placeholder="Enter the value" Min="5">
        /// </SfNumericTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public TValue Min { get; set; } = GetNumericValue<TValue>(MIN_VALUE);

        /// <summary>
        /// Stores the previous value of the <see cref="Min"/> property for change detection.
        /// </summary>
        private TValue? PreviousMin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse wheel interaction is enabled for incrementing or decrementing the value in the <see cref="SfNumericTextBox{TValue}"/> component.
        /// </summary>
        /// <value>
        /// <c>true</c> if mouse wheel interaction is enabled, allowing the value to be incremented or decremented when scrolling the mouse wheel; otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// - When <c>AllowMouseWheel</c> is set to <c>true</c>, scrolling the mouse wheel will increment or decrement the value in the <see cref="SfNumericTextBox{TValue}"/> based on the step value.
        /// - When <c>AllowMouseWheel</c> is set to <c>false</c>, mouse wheel scrolling will be ignored.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int?" Value="10" AllowMouseWheel="false" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool AllowMouseWheel { get; set; } = true;
    }
}