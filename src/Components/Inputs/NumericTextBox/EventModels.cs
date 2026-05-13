namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents the event arguments for the value change event in the NumericTextBox component.
    /// </summary>
    /// <typeparam name="T">The type of the value that changed in the NumericTextBox.</typeparam>
    /// <remarks>
    /// This class provides detailed information about the change event, including the previous and current values,
    /// whether the change was triggered by user interaction, and the original event arguments.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfNumericTextBox @bind-Value="@numericValue" ValueChange="OnValueChange">
    /// </SfNumericTextBox>
    /// 
    /// @code {
    ///     private double numericValue = 10;
    /// 
    ///     private void OnValueChange(ChangeEventArgs<double> args)
    ///     {
    ///         Console.WriteLine($"Previous Value: {args.PreviousValue}");
    ///         Console.WriteLine($"Current Value: {args.Value}");
    ///         Console.WriteLine($"Is User Interaction: {args.IsInteracted}");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class ChangeEventArgs<T>
    {
        /// <summary>
        /// Gets or sets the original event arguments that triggered the change event.
        /// </summary>
        /// <value>
        /// An <see cref="EventArgs"/> object containing the original event information, or <c>null</c> if not available.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying event arguments from the browser's change event.
        /// It can be used to access additional event details if needed.
        /// </remarks>
        public EventArgs? Event { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the change was triggered by user interaction.
        /// </summary>
        /// <value>
        /// <c>true</c> if the value change was caused by user interaction (such as typing or clicking spin buttons); 
        /// otherwise, <c>false</c> if changed programmatically.
        /// </value>
        /// <remarks>
        /// This property helps distinguish between programmatic value changes and user-initiated changes,
        /// which can be useful for implementing different behaviors based on the source of the change.
        /// </remarks>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> value representing the event name, or <c>null</c> if not specified.
        /// </value>
        /// <remarks>
        /// This property typically contains the name of the specific event that was triggered,
        /// such as "ValueChange" for value change events.
        /// </remarks>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the previous value of the NumericTextBox before the change occurred.
        /// </summary>
        /// <value>
        /// The previous value of type <typeparamref name="T"/>, or <c>null</c> if there was no previous value.
        /// </value>
        /// <remarks>
        /// This property allows you to compare the old and new values to determine the magnitude of the change
        /// or to implement undo functionality.
        /// </remarks>
        public T? PreviousValue { get; set; }

        /// <summary>
        /// Gets or sets the current value of the NumericTextBox after the change.
        /// </summary>
        /// <value>
        /// The current value of type <typeparamref name="T"/>, or <c>null</c> if the value is empty or invalid.
        /// </value>
        /// <remarks>
        /// This property contains the new value after the change event has occurred.
        /// The value type depends on the generic type parameter specified for the NumericTextBox.
        /// </remarks>
        public T? Value { get; set; }
    }

    /// <summary>
    /// Represents the event arguments for the blur event in the NumericTextBox component.
    /// </summary>
    /// <typeparam name="T">The type of the value in the NumericTextBox.</typeparam>
    /// <remarks>
    /// This class is used when the NumericTextBox loses focus, providing information about the current state
    /// of the component including its value and container element reference.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfNumericTextBox @bind-Value="@numericValue" OnBlur="OnBlur">
    /// </SfNumericTextBox>
    ///
    /// @code {
    ///     private double numericValue = 10;
    ///
    ///     private void OnBlur(NumericBlurEventArgs<double> args)
    ///     {
    ///         Console.WriteLine($"NumericTextBox lost focus with value: {args.Value}");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class NumericBlurEventArgs<T>
    {
        /// <summary>
        /// Gets or sets the original browser event arguments that triggered the blur event.
        /// </summary>
        /// <value>
        /// An <see cref="object"/> containing the original event arguments, or <c>null</c> if not available.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying browser event object that caused the blur event,
        /// allowing access to additional event properties if needed.
        /// </remarks>
        public object? Event { get; set; }

        /// <summary>
        /// Gets or sets the name of the blur event.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> value representing the event name, or <c>null</c> if not specified.
        /// </value>
        /// <remarks>
        /// This property typically contains "Blur" for blur events.
        /// </remarks>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the current value of the NumericTextBox when the blur event occurred.
        /// </summary>
        /// <value>
        /// The current value of type <typeparamref name="T"/>, or <c>null</c> if the value is empty or invalid.
        /// </value>
        /// <remarks>
        /// This property contains the value of the NumericTextBox at the time it lost focus.
        /// </remarks>
        public T? Value { get; set; }
    }

    /// <summary>
    /// Represents the event arguments for the focus event in the NumericTextBox component.
    /// </summary>
    /// <typeparam name="T">The type of the value in the NumericTextBox.</typeparam>
    /// <remarks>
    /// This class is used when the NumericTextBox receives focus, providing information about the current state
    /// of the component including its value and container element reference.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfNumericTextBox @bind-Value="@numericValue" OnFocus="OnFocus">
    /// </SfNumericTextBox>
    ///
    /// @code {
    ///     private double numericValue = 10;
    ///
    ///     private void OnFocus(NumericFocusEventArgs<double> args)
    ///     {
    ///         Console.WriteLine($"NumericTextBox received focus with value: {args.Value}");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class NumericFocusEventArgs<T>
    {
        /// <summary>
        /// Gets or sets the original browser event arguments that triggered the focus event.
        /// </summary>
        /// <value>
        /// An <see cref="object"/> containing the original event arguments, or <c>null</c> if not available.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying browser event object that caused the focus event,
        /// allowing access to additional event properties if needed.
        /// </remarks>
        public object? Event { get; set; }

        /// <summary>
        /// Gets or sets the name of the focus event.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> value representing the event name, or <c>null</c> if not specified.
        /// </value>
        /// <remarks>
        /// This property typically contains "Focus" for focus events.
        /// </remarks>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the current value of the NumericTextBox when the focus event occurred.
        /// </summary>
        /// <value>
        /// The current value of type <typeparamref name="T"/>, or <c>null</c> if the value is empty or invalid.
        /// </value>
        /// <remarks>
        /// This property contains the value of the NumericTextBox at the time it received focus.
        /// </remarks>
        public T? Value { get; set; }
    }

    internal class NumericClientProps
    {
        /// <summary>
        /// Specifies the component is in read-only mode or not.
        /// </summary>
        /// <exclude/>
        public bool Readonly { get; set; }

        /// <summary>
        /// Specifies the component is in disabled state or not.
        /// </summary>
        /// <exclude/>
        public bool Disabled { get; set; }

        /// <summary>
        /// Specifies the locale property.
        /// </summary>
        /// <exclude/>
        public string? Locale { get; set; }

        /// <summary>
        /// Specifies the ValidateDecimalOnType property.
        /// </summary>
        /// <exclude/>
        public bool ValidateDecimalOnType { get; set; }

        /// <summary>
        /// Specifies the Decimals property.
        /// </summary>
        /// <exclude/>
        public int? Decimals { get; set; }

        /// <summary>
        /// Specifies the DecimalSeparator property.
        /// </summary>
        /// <exclude/>
        public string? DecimalSeparator { get; set; }

        /// <summary>
        /// Specifies whether mouse wheel interaction is allowed.
        /// </summary>
        /// <exclude/>
        public bool AllowMouseWheel { get; set; }

    }

    /// <summary>
    /// Represents the configuration model for the NumericTextBox component.
    /// </summary>
    /// <typeparam name="T">The type of the numeric value that the NumericTextBox will handle.</typeparam>
    /// <remarks>
    /// This class contains all the configurable properties for the NumericTextBox component,
    /// including formatting options, validation settings, and behavioral configurations.
    /// It supports various numeric types such as int, double, decimal, and float.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfNumericTextBox TValue="double" 
    ///                   Value="@model.Value"
    ///                   Min="@model.Min"
    ///                   Max="@model.Max"
    ///                   Step="@model.Step"
    ///                   Format="@model.Format">
    /// </SfNumericTextBox>
    /// 
    /// @code {
    ///     private NumericTextBoxModel<double> model = new NumericTextBoxModel<double>
    ///     {
    ///         Value = 10.5,
    ///         Min = 0,
    ///         Max = 100,
    ///         Step = 0.1,
    ///         Format = "n2"
    ///     };
    /// }
    /// ]]></code>
    /// </example>
    public class NumericTextBoxModel<T>
    {
        /// <summary>
        /// Gets or sets the CSS class names to be applied to the NumericTextBox root element.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing one or more CSS class names separated by spaces, or <c>null</c> if no custom classes are applied.
        /// </value>
        /// <remarks>
        /// <para>Multiple CSS classes can be specified by separating them with spaces.</para>
        /// <para>These classes are applied in addition to the default Syncfusion CSS classes.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox CssClass="custom-numeric my-style" TValue="int"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public string? CssClass { get; set; }

        /// <summary>
        /// Gets or sets the currency code for currency formatting.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing an ISO 4217 currency code, or <c>null</c> if no currency formatting is applied.
        /// </value>
        /// <remarks>
        /// <para>Use standard ISO 4217 currency codes such as:</para>
        /// <list type="bullet">
        /// <item><description>USD - US Dollar</description></item>
        /// <item><description>EUR - Euro</description></item>
        /// <item><description>GBP - British Pound</description></item>
        /// <item><description>JPY - Japanese Yen</description></item>
        /// </list>
        /// <para>This property works in conjunction with the <see cref="Format"/> property to display currency values.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="decimal" Currency="USD" Format="c2"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public string? Currency { get; set; }

        /// <summary>
        /// Gets or sets the number of decimal places to display when the NumericTextBox is focused.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> value representing the number of decimal places, or <c>null</c> to use the default precision.
        /// </value>
        /// <remarks>
        /// <para>This property controls the precision shown during editing (when focused).</para>
        /// <para>When the control loses focus, the number of decimals may be adjusted based on the <see cref="Format"/> property.</para>
        /// <para>If not specified, the default precision based on the numeric type will be used.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="double" Decimals="3" Value="123.456789"></SfNumericTextBox>
        /// <!-- Shows 123.457 when focused -->
        /// ]]></code>
        /// </example>
        public int? Decimals { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the NumericTextBox state should be persisted between page reloads.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable state persistence; otherwise, <c>false</c>. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// <para>When enabled, the following properties are persisted:</para>
        /// <list type="bullet">
        /// <item><description><see cref="Value"/> - The current numeric value</description></item>
        /// </list>
        /// <para>The state is stored in the browser's local storage using the component's ID as the key.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int" EnablePersistence="true" ID="myNumeric"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the NumericTextBox is enabled for user interaction.
        /// </summary>
        /// <value>
        /// <c>true</c> if the NumericTextBox is enabled; otherwise, <c>false</c>. The default is <c>true</c>.
        /// </value>
        /// <remarks>
        /// <para>When disabled (<c>false</c>), the NumericTextBox:</para>
        /// <list type="bullet">
        /// <item><description>Cannot receive focus</description></item>
        /// <item><description>Does not respond to user input</description></item>
        /// <item><description>Appears visually disabled (grayed out)</description></item>
        /// <item><description>Is excluded from form submissions</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int" Enabled="false" Value="100"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the floating label behavior for the NumericTextBox.
        /// </summary>
        /// <value>
        /// A <see cref="FloatLabelType"/> value that determines how the placeholder text behaves as a floating label.
        /// The default is <see cref="FloatLabelType.Never"/>.
        /// </value>
        /// <remarks>
        /// <para>The floating label behavior options are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term><see cref="FloatLabelType.Never"/></term>
        /// <description>The placeholder text remains static and does not float above the input.</description>
        /// </item>
        /// <item>
        /// <term><see cref="FloatLabelType.Always"/></term>
        /// <description>The label always appears above the input field, regardless of focus or content.</description>
        /// </item>
        /// <item>
        /// <term><see cref="FloatLabelType.Auto"/></term>
        /// <description>The label floats above the input when focused or when the input contains a value.</description>
        /// </item>
        /// </list>
        /// <para>This property works in conjunction with the <see cref="Placeholder"/> property.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="double" 
        ///                   Placeholder="Enter amount" 
        ///                   FloatLabelType="FloatLabelType.Auto">
        /// </SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public FloatLabelType FloatLabelType { get; set; }

        /// <summary>
        /// Gets or sets the format string that defines how the numeric value is displayed.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing a .NET numeric format string, or <c>null</c> to use the default format.
        /// </value>
        /// <remarks>
        /// <para>Supports standard and custom .NET numeric format strings:</para>
        /// <list type="bullet">
        /// <item><description>Standard formats: "n" (number), "c" (currency), "p" (percentage), "f" (fixed-point)</description></item>
        /// <item><description>Custom formats: "#,##0.00", "0.###", etc.</description></item>
        /// </list>
        /// <para>The format is applied when the control loses focus or for display purposes.</para>
        /// <para>During editing (when focused), the precision may be controlled by the <see cref="Decimals"/> property.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="decimal" Format="c2" Currency="USD"></SfNumericTextBox>
        /// <!-- Displays as $123.45 -->
        /// 
        /// <SfNumericTextBox TValue="double" Format="n3"></SfNumericTextBox>
        /// <!-- Displays as 1,234.567 -->
        /// ]]></code>
        /// </example>
        public string? Format { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes to be applied to the NumericTextBox root element.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> where keys are attribute names and values are attribute values,
        /// or <c>null</c> if no additional attributes are specified.
        /// </value>
        /// <remarks>
        /// <para>This allows you to add custom HTML attributes such as:</para>
        /// <list type="bullet">
        /// <item><description>Custom CSS styles</description></item>
        /// <item><description>Data attributes</description></item>
        /// <item><description>ARIA attributes for accessibility</description></item>
        /// <item><description>Custom event handlers</description></item>
        /// </list>
        /// <para><strong>Note:</strong> If both a component property and an equivalent HTML attribute are specified,
        /// the component property takes precedence.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @{
        ///     var htmlAttributes = new Dictionary<string, object>
        ///     {
        ///         { "style", "border: 2px solid red;" },
        ///         { "data-testid", "amount-input" },
        ///         { "aria-label", "Enter amount" }
        ///     };
        /// }
        /// <SfNumericTextBox TValue="decimal" HtmlAttributes="@htmlAttributes"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public Dictionary<string, object>? HtmlAttributes { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes to be applied to the input element of the NumericTextBox.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> where keys are attribute names and values are attribute values,
        /// or <c>null</c> if no additional input attributes are specified.
        /// </value>
        /// <remarks>
        /// <para>This allows you to add custom HTML attributes specifically to the input element, such as:</para>
        /// <list type="bullet">
        /// <item><description>Form-related attributes (name, form, etc.)</description></item>
        /// <item><description>Validation attributes (required, pattern, etc.)</description></item>
        /// <item><description>Accessibility attributes (aria-describedby, etc.)</description></item>
        /// <item><description>Custom data attributes</description></item>
        /// </list>
        /// <para><strong>Note:</strong> If both a component property and an equivalent input attribute are specified,
        /// the component property takes precedence.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @{
        ///     var inputAttributes = new Dictionary<string, object>
        ///     {
        ///         { "name", "amount" },
        ///         { "data-validation", "required" },
        ///         { "aria-describedby", "amount-help" }
        ///     };
        /// }
        /// <SfNumericTextBox TValue="decimal" InputAttributes="@inputAttributes"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public Dictionary<string, object>? InputAttributes { get; set; }

        /// <summary>
        /// Gets or sets the placeholder text displayed when the NumericTextBox is empty.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the placeholder text, or <c>null</c> if no placeholder is specified.
        /// </value>
        /// <remarks>
        /// <para>The placeholder text serves as a hint to users about the expected input.</para>
        /// <para>The behavior of the placeholder is influenced by the <see cref="FloatLabelType"/> property:</para>
        /// <list type="bullet">
        /// <item><description>When <see cref="FloatLabelType.Never"/>, it appears as static placeholder text</description></item>
        /// <item><description>When <see cref="FloatLabelType.Auto"/> or <see cref="FloatLabelType.Always"/>, it acts as a floating label</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="double" Placeholder="Enter amount"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public string? Placeholder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the NumericTextBox is read-only.
        /// </summary>
        /// <value>
        /// <c>true</c> if the NumericTextBox is read-only; otherwise, <c>false</c>. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// <para>When read-only (<c>true</c>), the NumericTextBox:</para>
        /// <list type="bullet">
        /// <item><description>Displays the current value but prevents editing</description></item>
        /// <item><description>Can still receive focus and be selected</description></item>
        /// <item><description>Does not show spin buttons or clear button</description></item>
        /// <item><description>Can be included in form submissions</description></item>
        /// </list>
        /// <para>This differs from <see cref="Enabled"/> = <c>false</c>, which completely disables interaction.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="decimal" Readonly="true" Value="1000"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public bool Readonly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the clear button is displayed in the NumericTextBox.
        /// </summary>
        /// <value>
        /// <c>true</c> to display the clear button; otherwise, <c>false</c>. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// <para>When enabled, a clear button (×) appears on the right side of the input when it contains a value.</para>
        /// <para>Clicking the clear button removes the current value and triggers the <c>ValueChange</c> event.</para>
        /// <para>The clear button is automatically hidden when the input is empty, disabled, or read-only.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int" ShowClearButton="true" Value="100"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public bool ShowClearButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the spin buttons (up/down arrows) are displayed in the NumericTextBox.
        /// </summary>
        /// <value>
        /// <c>true</c> to display the spin buttons; otherwise, <c>false</c>. The default is <c>true</c>.
        /// </value>
        /// <remarks>
        /// <para>When enabled, up and down arrow buttons appear that allow users to increment or decrement the value.</para>
        /// <para>The increment/decrement amount is controlled by the <see cref="Step"/> property.</para>
        /// <para>Spin buttons respect the <see cref="Min"/> and <see cref="Max"/> value constraints.</para>
        /// <para>The buttons are automatically disabled when the input is disabled or read-only.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="double" ShowSpinButton="true" Step="0.5"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public bool ShowSpinButton { get; set; }

        /// <summary>
        /// Gets or sets the increment/decrement step size for the NumericTextBox spin buttons and keyboard navigation.
        /// </summary>
        /// <value>
        /// A value of type <typeparamref name="T"/> representing the step size, or <c>null</c> to use the default step (1 for integers, 0.01 for decimals).
        /// </value>
        /// <remarks>
        /// <para>This property controls:</para>
        /// <list type="bullet">
        /// <item><description>The amount by which the value changes when spin buttons are clicked</description></item>
        /// <item><description>The increment/decrement when using Up/Down arrow keys</description></item>
        /// <item><description>The step size for mouse wheel scrolling (if enabled)</description></item>
        /// </list>
        /// <para>The step value should be positive and appropriate for the value type and range.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="double" Step="0.5" Min="0" Max="10"></SfNumericTextBox>
        /// <!-- Values: 0, 0.5, 1.0, 1.5, 2.0, ... -->
        /// 
        /// <SfNumericTextBox TValue="int" Step="5" Min="0" Max="100"></SfNumericTextBox>
        /// <!-- Values: 0, 5, 10, 15, 20, ... -->
        /// ]]></code>
        /// </example>
        public T? Step { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the NumericTextBox should enforce strict value range validation.
        /// </summary>
        /// <value>
        /// <c>true</c> to enforce strict mode where values are automatically corrected to fit within the Min/Max range; 
        /// otherwise, <c>false</c> to allow out-of-range values with visual error indication. The default is <c>true</c>.
        /// </value>
        /// <remarks>
        /// <para>When <c>true</c> (strict mode):</para>
        /// <list type="bullet">
        /// <item><description>Values are automatically clamped to the <see cref="Min"/>/<see cref="Max"/> range on blur</description></item>
        /// <item><description>Invalid values are corrected to the nearest valid value</description></item>
        /// <item><description>No error styling is applied</description></item>
        /// </list>
        /// <para>When <c>false</c> (non-strict mode):</para>
        /// <list type="bullet">
        /// <item><description>Out-of-range values are preserved</description></item>
        /// <item><description>Error CSS classes are applied for visual feedback</description></item>
        /// <item><description>Validation logic must be handled separately</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <!-- Strict mode: values auto-corrected -->
        /// <SfNumericTextBox TValue="int" StrictMode="true" Min="0" Max="100"></SfNumericTextBox>
        /// 
        /// <!-- Non-strict mode: shows error styling for invalid values -->
        /// <SfNumericTextBox TValue="int" StrictMode="false" Min="0" Max="100"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public bool StrictMode { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether decimal places should be validated during typing.
        /// </summary>
        /// <value>
        /// <c>true</c> to restrict decimal input during typing based on the <see cref="Decimals"/> property; 
        /// otherwise, <c>false</c> to allow unrestricted decimal input during typing. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// <para>When <c>true</c>:</para>
        /// <list type="bullet">
        /// <item><description>Users cannot type more decimal places than specified by <see cref="Decimals"/></description></item>
        /// <item><description>Prevents invalid decimal input in real-time</description></item>
        /// <item><description>Provides immediate feedback during typing</description></item>
        /// </list>
        /// <para>When <c>false</c>:</para>
        /// <list type="bullet">
        /// <item><description>Users can type any number of decimal places during editing</description></item>
        /// <item><description>Validation occurs on blur or form submission</description></item>
        /// <item><description>Value is formatted according to <see cref="Decimals"/> when focus is lost</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <!-- Restricts to 2 decimal places during typing -->
        /// <SfNumericTextBox TValue="decimal" 
        ///                   ValidateDecimalOnType="true" 
        ///                   Decimals="2"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public bool ValidateDecimalOnType { get; set; }

        /// <summary>
        /// Gets or sets the current value of the NumericTextBox.
        /// </summary>
        /// <value>
        /// The current numeric value of type <typeparamref name="T"/>, or <c>null</c> if no value is set or the input is empty.
        /// </value>
        /// <remarks>
        /// <para>This property supports two-way data binding and triggers change events when modified.</para>
        /// <para>The value is subject to validation based on:</para>
        /// <list type="bullet">
        /// <item><description><see cref="Min"/> and <see cref="Max"/> constraints</description></item>
        /// <item><description><see cref="StrictMode"/> behavior</description></item>
        /// <item><description>Type-specific precision and formatting rules</description></item>
        /// </list>
        /// <para>When setting the value programmatically, it will be formatted according to the <see cref="Format"/> property.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox @bind-Value="@currentValue" TValue="double"></SfNumericTextBox>
        /// 
        /// @code {
        ///     private double currentValue = 25.75;
        /// }
        /// ]]></code>
        /// </example>
        public T? Value { get; set; }

        /// <summary>
        /// Gets or sets the width of the NumericTextBox component.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the width in CSS units (px, %, em, etc.), or <c>null</c> to use the default width.
        /// </value>
        /// <remarks>
        /// <para>Accepts any valid CSS width value such as:</para>
        /// <list type="bullet">
        /// <item><description>"200px" - Fixed pixel width</description></item>
        /// <item><description>"50%" - Percentage of parent container</description></item>
        /// <item><description>"15em" - Relative to font size</description></item>
        /// <item><description>"auto" - Automatic sizing</description></item>
        /// </list>
        /// <para>If not specified, the component uses its default width based on the theme and content.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int" Width="250px"></SfNumericTextBox>
        /// <SfNumericTextBox TValue="double" Width="100%"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public string? Width { get; set; }

        /// <summary>
        /// Gets or sets the tab order of the NumericTextBox in the document's tab sequence.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> value representing the tab index. The default is 0.
        /// </value>
        /// <remarks>
        /// <para>Controls the order in which the NumericTextBox receives focus when users navigate using the Tab key:</para>
        /// <list type="bullet">
        /// <item><description>Positive values: Elements are focused in increasing numerical order</description></item>
        /// <item><description>0: Element is focused in its natural document order</description></item>
        /// <item><description>-1: Element can be focused programmatically but is excluded from tab navigation</description></item>
        /// </list>
        /// <para>This property sets the HTML tabindex attribute on the input element.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int" TabIndex="1"></SfNumericTextBox>
        /// <SfNumericTextBox TValue="double" TabIndex="2"></SfNumericTextBox>
        /// ]]></code>
        /// </example>
        public int TabIndex { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed value for the NumericTextBox.
        /// </summary>
        /// <value>
        /// The maximum value of type <typeparamref name="T"/>, or <c>null</c> if no maximum constraint is applied.
        /// </value>
        /// <remarks>
        /// <para>When specified, this property:</para>
        /// <list type="bullet">
        /// <item><description>Prevents users from entering values greater than the maximum</description></item>
        /// <item><description>Disables the up spin button when the maximum is reached</description></item>
        /// <item><description>Works with <see cref="StrictMode"/> to control validation behavior</description></item>
        /// <item><description>Affects keyboard navigation and mouse wheel interactions</description></item>
        /// </list>
        /// <para>The maximum value should be greater than or equal to the <see cref="Min"/> value.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int" Min="0" Max="100" Value="50"></SfNumericTextBox>
        /// <!-- Allows values from 0 to 100 -->
        /// 
        /// <SfNumericTextBox TValue="decimal" Max="999.99" Format="c2"></SfNumericTextBox>
        /// <!-- Currency input with maximum limit -->
        /// ]]></code>
        /// </example>
        public T? Max { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed value for the NumericTextBox.
        /// </summary>
        /// <value>
        /// The minimum value of type <typeparamref name="T"/>, or <c>null</c> if no minimum constraint is applied.
        /// </value>
        /// <remarks>
        /// <para>When specified, this property:</para>
        /// <list type="bullet">
        /// <item><description>Prevents users from entering values less than the minimum</description></item>
        /// <item><description>Disables the down spin button when the minimum is reached</description></item>
        /// <item><description>Works with <see cref="StrictMode"/> to control validation behavior</description></item>
        /// <item><description>Affects keyboard navigation and mouse wheel interactions</description></item>
        /// </list>
        /// <para>The minimum value should be less than or equal to the <see cref="Max"/> value.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="double" Min="0.0" Max="100.0" Step="0.1"></SfNumericTextBox>
        /// <!-- Allows values from 0.0 to 100.0 with 0.1 increments -->
        /// 
        /// <SfNumericTextBox TValue="int" Min="-50" Max="50"></SfNumericTextBox>
        /// <!-- Allows negative and positive values -->
        /// ]]></code>
        /// </example>
        public T? Min { get; set; }
    }
}
