using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents the Toolkit TextBox component, which is an input element that allows users to enter, edit, and display text values.
    /// The TextBox provides various customization options including floating labels, clear button, multiline support, and input validation.
    /// </summary>
    /// <remarks>
    /// The <see cref="SfTextBox"/> component supports both single-line and multiline input modes. It can be configured with
    /// floating labels, placeholder text, clear button functionality, and various input types. The component inherits from
    /// <see cref="SfInputBase{TValue}"/> providing base functionality for text input operations.
    /// </remarks>
    /// <example>
    /// The following code example demonstrates how to create a basic TextBox with floating label and clear button:
    /// <code><![CDATA[
    /// <SfTextBox @bind-Value="@textValue" 
    ///           Placeholder="Enter your name" 
    ///           FloatLabelType="FloatLabelType.Auto" 
    ///           ShowClearButton="true">
    /// </SfTextBox>
    /// 
    /// @code {
    ///     private string textValue = "";
    /// }
    /// ]]></code>
    /// </example>
    public partial class SfTextBox : SfInputBase<string>
    {
        #region Private variables

        private string _cssClass = string.Empty;

        private AutoComplete _autocomplete;

        private InputType _type;

        #endregion

        #region Public variables

        /// <summary>
        /// Gets or sets a value indicating whether the browser should automatically complete or suggest values for the <see cref="SfTextBox"/> component.
        /// </summary>
        /// <value>
        /// An <see cref="AutoComplete"/> enumeration value specifying the autocomplete behavior. The default value is <see cref="AutoComplete.On"/>.
        /// </value>
        /// <remarks>
        /// <para>This property controls the HTML autocomplete attribute behavior. When enabled, browsers may display a dropdown list of previously entered values for the same field.</para>
        /// <para>Possible values:</para>
        /// <list type="bullet">
        /// <item><description><see cref="AutoComplete.On"/> - Autocomplete is enabled, allowing the browser to suggest previously entered values.</description></item>
        /// <item><description><see cref="AutoComplete.Off"/> - Autocomplete is disabled, preventing the browser from suggesting values.</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox Autocomplete="AutoComplete.Off" Placeholder="Enter password"></SfTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public AutoComplete Autocomplete { get; set; }

        /// <summary>
        /// Gets or sets the floating label behavior of the <see cref="SfTextBox"/> component. The <see cref="Placeholder"/> text acts as a floating label.
        /// </summary>
        /// <value>
        /// A <see cref="FloatLabelType"/> enumeration value specifying how the <see cref="Placeholder"/> text behaves as a floating label. The default value is <see cref="FloatLabelType.Never"/>.
        /// </value>
        /// <remarks>
        /// <para>The floating label behavior determines how the placeholder text is displayed:</para>
        /// <list type="bullet">
        /// <item><description><see cref="FloatLabelType.Never"/> - The placeholder text remains static and does not float as a label.</description></item>
        /// <item><description><see cref="FloatLabelType.Auto"/> - The placeholder text floats above the TextBox as a label when the component gains focus or has a value.</description></item>
        /// <item><description><see cref="FloatLabelType.Always"/> - The placeholder text is always displayed as a label above the TextBox component.</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox FloatLabelType="FloatLabelType.Auto" Placeholder="Enter a value"></SfTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public FloatLabelType FloatLabelType
        {
            get => BaseFloatLabelType; set => BaseFloatLabelType = value;
        }

        /// <summary>
        /// Gets or sets the placeholder text that is displayed when the <see cref="SfTextBox"/> has no value.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the placeholder text displayed when the TextBox has no value. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// <para>The placeholder text provides a hint to users about what should be entered in the TextBox.</para>
        /// <para>The behavior of the placeholder depends on the <see cref="FloatLabelType"/> property:</para>
        /// <list type="bullet">
        /// <item><description>When <see cref="FloatLabelType"/> is <see cref="FloatLabelType.Never"/>, the placeholder text disappears when the TextBox gains focus.</description></item>
        /// <item><description>When <see cref="FloatLabelType"/> is <see cref="FloatLabelType.Auto"/> or <see cref="FloatLabelType.Always"/>, the placeholder text acts as a floating label.</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox Placeholder="Enter your email address" FloatLabelType="FloatLabelType.Auto"></SfTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Placeholder
        {
            get => BasePlaceholder; set => BasePlaceholder = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfTextBox"/> is read-only and prevents user input.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> value indicating whether the TextBox is read-only. When <see langword="true"/>, the component cannot be edited by the user; when <see langword="false"/>, the component is editable. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// <para>When the TextBox is set to read-only mode, users can view and select the text content but cannot modify it.</para>
        /// <para>The component will still participate in form submission and data binding operations.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox @bind-Value="@readOnlyValue" ReadOnly="true" Placeholder="Read-only text"></SfTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool ReadOnly
        {
            get => BaseReadOnly; set => BaseReadOnly = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the clear button is displayed in the <see cref="SfTextBox"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> value indicating whether the clear button should be shown. When <see langword="true"/>, the clear button is displayed; when <see langword="false"/>, it is hidden. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// <para>The clear button appears when the TextBox has a value and allows users to quickly clear the entire content with a single click.</para>
        /// <para>The button is typically displayed as an 'X' icon on the right side of the TextBox.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox @bind-Value="@textValue" ShowClearButton="true" Placeholder="Enter text"></SfTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool ShowClearButton
        {
            get => BaseShowClearButton; set => BaseShowClearButton = value;
        }

        /// <summary>
        /// Gets or sets the width of the <see cref="SfTextBox"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the width in pixels, percentage, or other CSS units. The default value is <see langword="null"/>, which renders as 100%.
        /// </value>
        /// <remarks>
        /// <para>The width can be specified using various CSS units such as pixels (px), percentages (%), em, rem, etc.</para>
        /// <para>If not specified, the component will take the full width of its container.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox Width="300px"></SfTextBox>
        /// <SfTextBox Width="50%"></SfTextBox>
        /// <SfTextBox Width="20rem"></SfTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string? Width
        {
            get => BaseWidth; set => BaseWidth = value;
        }

        /// <summary>
        /// Gets or sets the tab order of the <see cref="SfTextBox"/> component in the tab navigation sequence.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> value representing the tab index of the TextBox component. The default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// <para>The <see cref="TabIndex"/> property determines the order in which components receive focus when the user navigates using the Tab key.</para>
        /// <list type="bullet">
        /// <item><description>A value of <c>0</c> means the component participates in the default tab order.</description></item>
        /// <item><description>A positive value specifies the explicit tab order position.</description></item>
        /// <item><description>A negative value removes the component from the tab navigation sequence.</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox TabIndex="1" Placeholder="First in tab order"></SfTextBox>
        /// <SfTextBox TabIndex="2" Placeholder="Second in tab order"></SfTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public int TabIndex
        {
            get => BaseTabIndex; set => BaseTabIndex = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfTextBox"/> operates in multiline mode (textarea).
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> value indicating whether multiline mode is enabled. When <see langword="true"/>, the component renders as a textarea element; when <see langword="false"/>, it renders as a single-line input element. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// <para>When multiline mode is enabled, the TextBox transforms into a textarea element that supports:</para>
        /// <list type="bullet">
        /// <item><description>Multiple lines of text input</description></item>
        /// <item><description>Line breaks and paragraph formatting</description></item>
        /// <item><description>Scrolling when content exceeds the visible area</description></item>
        /// <item><description>Resizable functionality (browser-dependent)</description></item>
        /// </list>
        /// <para>This mode is particularly useful for capturing longer text content such as comments, descriptions, addresses, or any scenario requiring multi-line input.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox @bind-Value="@description" 
        ///           Multiline="true" 
        ///           Placeholder="Enter your description" 
        ///           FloatLabelType="FloatLabelType.Auto">
        /// </SfTextBox>
        /// 
        /// @code {
        ///     private string description = "";
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool Multiline
        {
            get => MultilineInput; set => MultilineInput = value;
        }

        /// <summary>
        /// Gets or sets a collection of additional HTML attributes that will be applied to the outer wrapper element of the <see cref="SfTextBox"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> of <see cref="string"/> keys and <see cref="object"/> values representing HTML attributes to be applied to the wrapper element. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// <para>This property allows adding custom HTML attributes to the TextBox wrapper element, such as:</para>
        /// <list type="bullet">
        /// <item><description><c>class</c> - Additional CSS classes</description></item>
        /// <item><description><c>style</c> - Inline CSS styles</description></item>
        /// <item><description><c>title</c> - Tooltip text</description></item>
        /// <item><description><c>data-*</c> - Custom data attributes</description></item>
        /// <item><description>Other standard HTML attributes</description></item>
        /// </list>
        /// <para><b>Note:</b> If both a component property and its equivalent HTML attribute are configured, the component property takes precedence.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox HtmlAttributes="@customAttributes">
        /// </SfTextBox>
        /// 
        /// @code {
        ///     Dictionary<string, object> customAttributes = new Dictionary<string, object>()
        ///     {
        ///         { "title", "Enter your full name" },
        ///         { "class", "custom-textbox" },
        ///         { "data-testid", "name-input" }
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
        /// Gets or sets a collection of additional HTML attributes that will be applied directly to the input element of the <see cref="SfTextBox"/> component.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> of <see cref="string"/> keys and <see cref="object"/> values containing attribute names and values to be applied to the input element. The default value is an empty dictionary.
        /// </value>
        /// <remarks>
        /// <para>This property captures unmatched attributes and applies them directly to the HTML input element. Common attributes include:</para>
        /// <list type="bullet">
        /// <item><description><c>maxlength</c> - Maximum number of characters allowed</description></item>
        /// <item><description><c>minlength</c> - Minimum number of characters required</description></item>
        /// <item><description><c>pattern</c> - Regular expression pattern for validation</description></item>
        /// <item><description><c>autocomplete</c> - Browser autocomplete behavior</description></item>
        /// <item><description><c>spellcheck</c> - Spellcheck functionality</description></item>
        /// <item><description><c>data-*</c> - Custom data attributes</description></item>
        /// </list>
        /// <para>Attributes can be specified using inline attributes or the <c>@attributes</c> directive.</para>
        /// <para><b>Note:</b> If both a component property and its equivalent HTML attribute are configured, the component property takes precedence.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <!-- Using @attributes directive -->
        /// <SfTextBox Placeholder="Enter PIN" @attributes="@_inputAttributes">
        /// </SfTextBox>
        /// 
        /// <!-- Using inline attributes -->
        /// <SfTextBox Placeholder="Enter email" maxlength="50" pattern="[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$">
        /// </SfTextBox>
        /// 
        /// @code {
        ///     Dictionary<string, object> _inputAttributes = new Dictionary<string, object>()
        ///     {
        ///         { "maxlength", "4" },
        ///         { "pattern", "[0-9]{4}" },
        ///         { "autocomplete", "one-time-code" }
        ///     };
        /// }
        /// ]]></code>
        /// </example>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? InputAttributes
        {
            get => BaseInputAttributes; set => BaseInputAttributes = value;
        }

        /// <summary>
        /// Gets or sets the input type behavior of the <see cref="SfTextBox"/> component, such as text, password, email, and more.
        /// </summary>
        /// <value>
        /// An <see cref="InputType"/> enumeration value specifying the type of input element to render. The default value is <see cref="InputType.Text"/>.
        /// </value>
        /// <remarks>
        /// <para>The input type determines the behavior, validation, and appearance of the TextBox:</para>
        /// <list type="bullet">
        /// <item><description><see cref="InputType.Text"/> - Standard text input</description></item>
        /// <item><description><see cref="InputType.Password"/> - Password input with masked characters</description></item>
        /// <item><description><see cref="InputType.Email"/> - Email input with built-in validation</description></item>
        /// <item><description><see cref="InputType.Number"/> - Numeric input with spinner controls</description></item>
        /// <item><description><see cref="InputType.Tel"/> - Telephone number input</description></item>
        /// <item><description><see cref="InputType.URL"/> - URL input with validation</description></item>
        /// <item><description><see cref="InputType.Search"/> - Search input with search-specific styling</description></item>
        /// </list>
        /// <para>Different input types may trigger different virtual keyboards on mobile devices and provide built-in browser validation.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox Type="InputType.Email" Placeholder="Enter your email"></SfTextBox>
        /// <SfTextBox Type="InputType.Password" Placeholder="Enter your password"></SfTextBox>
        /// <SfTextBox Type="InputType.Tel" Placeholder="Enter your phone number"></SfTextBox>
        /// ]]></code>
        /// </example>
        [Parameter]
        public InputType Type { get; set; }

        #endregion

        #region Protected variables

        /// <summary>
        /// Gets and sets the placeholder text for the <see cref="SfTextBox"/> component, which serves as a hint to users about what to enter in the input field when it is empty.
        /// </summary>
        /// <exclude/>
        protected override string BasePlaceholder { get; set; } = default!;

        /// <summary>
        /// Gets and sets a value indicating whether the <see cref="SfTextBox"/> component is read-only, which prevents users from editing the content of the input field while still allowing them to view and select the text.
        /// </summary>
        /// <exclude/>
        protected override bool BaseReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the input element itself should be marked as read-only.
        /// </summary>
        /// <exclude/>
        protected override bool BaseIsReadOnlyInput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the clear button should be displayed in the <see cref="SfTextBox"/> component, allowing users to quickly clear the input content with a single click when the TextBox has a value.
        /// </summary>
        /// <exclude/>
        protected override bool BaseShowClearButton { get; set; }

        /// <summary>
        /// Gets or sets the width of the <see cref="SfTextBox"/> component.
        /// </summary>
        /// <exclude/>
        protected override string? BaseWidth { get; set; }

        /// <summary>
        /// Gets or sets the tab index of the <see cref="SfTextBox"/> component for keyboard navigation.
        /// </summary>
        /// <exclude/>
        protected override int BaseTabIndex { get; set; }

        /// <summary>
        /// Gets or sets the floating label behavior of the <see cref="SfTextBox"/> component that determines how the placeholder text is displayed.
        /// </summary>
        /// <exclude/>
        protected override FloatLabelType BaseFloatLabelType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SfTextBox"/> component supports multiple lines of text.
        /// </summary>
        /// <exclude/>
        protected override bool MultilineInput { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes to be applied to the <see cref="SfTextBox"/> component's container element.
        /// </summary>
        /// <exclude/>
        protected override Dictionary<string, object>? BaseHtmlAttributes { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes to be applied specifically to the input element itself.
        /// </summary>
        /// <exclude/>
        protected override Dictionary<string, object>? BaseInputAttributes { get; set; } = [];

        /// <summary>
        /// Gets or sets the parent component reference when the <see cref="SfTextBox"/> is used within other Toolkit components.
        /// </summary>
        /// <value>
        /// A dynamic reference to the parent component, typically used when the TextBox is embedded within components like InPlaceEditor. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// <para>This property is automatically set by cascading parameters when the TextBox is used as a child component within other Toolkit components.</para>
        /// <para>It enables proper communication between the TextBox and its parent component.</para>
        /// </remarks>
        /// <exclude/>
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic? TextBoxParent { get; set; }

        #endregion
    }
}