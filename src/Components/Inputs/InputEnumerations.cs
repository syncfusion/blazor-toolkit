using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Specifies the type of drop effect to be applied during drag-and-drop operations in input components.
    /// </summary>
    /// <remarks>
    /// The <see cref="DropEffect"/> enumeration defines the visual feedback and behavior that occurs when an item is dropped during a drag-and-drop operation.
    /// This enum is commonly used in file upload components and other input controls that support drag-and-drop functionality.
    /// The drop effect determines how the dragged content will be handled when dropped onto the target area.
    /// </remarks>
    /// <example>
    /// Setting a drop effect for a file upload component:
    /// <code><![CDATA[
    /// <SfUploader DropEffect="DropEffect.Copy" ValueChange="OnChange">
    /// </SfUploader>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DropEffect
    {
        /// <summary>
        /// Creates a copy of the dragged item at the drop location.
        /// </summary>
        /// <remarks>
        /// When <c>Copy</c> is specified, the original item remains in its source location while a duplicate is created at the drop target.
        /// This is the most common drop effect for file uploads where the original file remains in its source directory.
        /// </remarks>
        [EnumMember(Value = "Copy")]
        Copy,

        /// <summary>
        /// Moves the dragged item from its source to the drop location.
        /// </summary>
        /// <remarks>
        /// When <c>Move</c> is specified, the item is relocated from its original position to the drop target location.
        /// The original item is removed from the source location once the drop operation is completed successfully.
        /// </remarks>
        [EnumMember(Value = "Move")]
        Move,

        /// <summary>
        /// Creates a link or reference to the dragged item at the drop location.
        /// </summary>
        /// <remarks>
        /// When <c>Link</c> is specified, a reference or shortcut to the original item is created at the drop target.
        /// The original item remains unchanged in its source location, and the link provides access to the original content.
        /// This is useful for creating shortcuts or references without duplicating the actual data.
        /// </remarks>
        [EnumMember(Value = "Link")]
        Link,

        /// <summary>
        /// Indicates that no drop effect should be applied, effectively disabling the drop operation.
        /// </summary>
        /// <remarks>
        /// When <c>None</c> is specified, the drop target will not accept the dragged item, and no operation will be performed.
        /// This effectively disables drag-and-drop functionality for the target area and provides visual feedback that dropping is not allowed.
        /// </remarks>
        [EnumMember(Value = "None")]
        None,

        /// <summary>
        /// Uses the default drop effect behavior as determined by the browser or component.
        /// </summary>
        /// <remarks>
        /// When <c>Default</c> is specified, the component will use the standard drop effect behavior provided by the browser.
        /// This typically defaults to the <c>Copy</c> operation for most file upload scenarios, but may vary based on the browser implementation and context.
        /// Using the default option allows the component to automatically determine the most appropriate drop effect.
        /// </remarks>
        [EnumMember(Value = "Default")]
        Default,
    }

    /// <summary>
    /// Defines the floating label behavior for input components, controlling how and when the label transitions from placeholder to floating position.
    /// </summary>
    /// <value>
    /// An enumeration that specifies the floating label behavior mode for input components.
    /// </value>
    /// <remarks>
    /// <para>The floating label provides enhanced user experience by transforming the placeholder text into a label that appears above the input field.
    /// This behavior helps maintain context while the user interacts with the input, ensuring they always know what information is expected.
    /// The different modes offer flexibility for various design patterns and user interaction preferences.</para>
    /// <list type="bullet">
    /// <item><description><see cref="Never"/> - The label remains as placeholder text and never transitions to a floating position.</description></item>
    /// <item><description><see cref="Always"/> - The label is permanently positioned above the input field.</description></item>
    /// <item><description><see cref="Auto"/> - The label automatically floats when the field receives focus or contains a value.</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// Setting floating label behavior:
    /// <code><![CDATA[
    /// <SfTextBox FloatLabelType="FloatLabelType.Auto" Placeholder="Enter your name"></SfTextBox>
    /// ]]></code>
    /// </example>
    public enum FloatLabelType
    {
        /// <summary>
        /// The label remains static as a placeholder and never transitions to a floating position above the input.
        /// </summary>
        /// <value>
        /// Represents the "Never" floating label mode where the label stays as placeholder text.
        /// </value>
        /// <remarks>
        /// When this mode is selected, the label text remains in the input field as placeholder text and does not move to a floating position above the input, even when the field is focused or contains a value.
        /// </remarks>
        [EnumMember(Value = "Never")]
        Never,

        /// <summary>
        /// The label is permanently positioned above the input field, regardless of focus state or content.
        /// </summary>
        /// <value>
        /// Represents the "Always" floating label mode where the label is permanently positioned above the input.
        /// </value>
        /// <remarks>
        /// In this mode, the label is always displayed above the input field, providing constant visual context for the expected input regardless of whether the field is focused or contains a value.
        /// </remarks>
        [EnumMember(Value = "Always")]
        Always,

        /// <summary>
        /// The label automatically transitions to a floating position above the input when the field receives focus or contains a value.
        /// </summary>
        /// <value>
        /// Represents the "Auto" floating label mode where the label automatically transitions based on input state.
        /// </value>
        /// <remarks>
        /// This is the most dynamic mode where the label starts as placeholder text and smoothly transitions to a floating position above the input when the user focuses on the field or when the field contains a value. This provides the best balance of space efficiency and user experience.
        /// </remarks>
        [EnumMember(Value = "Auto")]
        Auto,
    }

    /// <summary>
    /// Defines whether the browser is allowed to automatically enter or select values for input fields using stored user data.
    /// </summary>
    /// <value>
    /// An enumeration that controls browser autocomplete behavior for input fields.
    /// </value>
    /// <remarks>
    /// <para>The autocomplete feature leverages browser-stored user data such as previously entered values, saved passwords,
    /// or address information to provide suggestions and automatic filling capabilities. This enhances user experience
    /// by reducing repetitive data entry, but can be disabled for sensitive fields or when custom autocomplete
    /// functionality is preferred.</para>
    /// <list type="bullet">
    /// <item><description><see cref="On"/> - Enables browser autocomplete suggestions and automatic filling.</description></item>
    /// <item><description><see cref="Off"/> - Disables browser autocomplete for the input field.</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// Controlling autocomplete behavior:
    /// <code><![CDATA[
    /// <SfTextBox AutoComplete="AutoComplete.Off" Placeholder="Sensitive information"></SfTextBox>
    /// ]]></code>
    /// </example>
    public enum AutoComplete
    {
        /// <summary>
        /// Enables browser autocomplete functionality, allowing automatic suggestions and filling based on user's previous inputs and saved data.
        /// </summary>
        /// <value>
        /// Represents the "on" state for browser autocomplete functionality.
        /// </value>
        /// <remarks>
        /// When enabled, the browser will provide autocomplete suggestions based on previously entered values, saved form data, and other stored user information. This improves user experience by reducing the need for repetitive typing.
        /// </remarks>
        [EnumMember(Value = "on")]
        On,

        /// <summary>
        /// Disables browser autocomplete functionality, preventing automatic suggestions and requiring manual input for all values.
        /// </summary>
        /// <value>
        /// Represents the "off" state for browser autocomplete functionality.
        /// </value>
        /// <remarks>
        /// When disabled, the browser will not provide autocomplete suggestions or automatically fill the input field. This is typically used for sensitive information such as passwords, credit card numbers, or when implementing custom autocomplete functionality.
        /// </remarks>
        [EnumMember(Value = "off")]
        Off
    }

    /// <summary>
    /// Specifies the input type for TextBox components, determining the data format, validation behavior, and browser-specific features.
    /// </summary>
    /// <value>
    /// An enumeration that defines the input type and associated behavior for TextBox components.
    /// </value>
    /// <remarks>
    /// Different input types provide specialized functionality including:
    /// <list type="bullet">
    /// <item><description>Format validation (email, URL patterns)</description></item>
    /// <item><description>Virtual keyboard optimization on mobile devices</description></item>
    /// <item><description>Browser-specific UI enhancements (password masking, number steppers)</description></item>
    /// <item><description>Accessibility improvements for screen readers</description></item>
    /// <item><description>Built-in validation messages and constraints</description></item>
    /// </list>
    /// Choose the appropriate input type to ensure optimal user experience and data integrity.
    /// </remarks>
    /// <example>
    /// Setting different input types:
    /// <code><![CDATA[
    /// <SfTextBox Type="InputType.Email" Placeholder="Enter email address"></SfTextBox>
    /// <SfTextBox Type="InputType.Password" Placeholder="Enter password"></SfTextBox>
    /// ]]></code>
    /// </example>
    public enum InputType
    {
        /// <summary>
        /// Standard single-line text input accepting any alphanumeric characters and symbols without format restrictions.
        /// </summary>
        /// <value>
        /// Represents the "text" input type for general text entry.
        /// </value>
        /// <remarks>
        /// This is the default input type that allows users to enter any combination of letters, numbers, and special characters. No specific format validation or input restrictions are applied.
        /// </remarks>
        [EnumMember(Value = "text")]
        Text,

        /// <summary>
        /// Email address input with built-in validation for proper email format and optimized virtual keyboard on mobile devices.
        /// </summary>
        /// <value>
        /// Represents the "email" input type for email address entry.
        /// </value>
        /// <remarks>
        /// This input type provides built-in email format validation and displays an optimized virtual keyboard on mobile devices with easy access to the @ symbol and common email domains.
        /// </remarks>
        [EnumMember(Value = "email")]
        Email,

        /// <summary>
        /// Password input where characters are visually masked for security, preventing shoulder surfing and maintaining privacy.
        /// </summary>
        /// <value>
        /// Represents the "password" input type for secure text entry.
        /// </value>
        /// <remarks>
        /// Characters entered in password fields are masked with dots or asterisks to prevent visual eavesdropping. This input type also typically disables browser autocomplete and copy functionality for enhanced security.
        /// </remarks>
        [EnumMember(Value = "password")]
        Password,

        /// <summary>
        /// Numeric input with built-in number validation and spinner controls for incrementing/decrementing values.
        /// </summary>
        /// <value>
        /// Represents the "number" input type for numeric entry.
        /// </value>
        /// <remarks>
        /// This input type restricts input to numeric values and may provide spinner controls (up/down arrows) for incrementing and decrementing values. Mobile devices will display a numeric keypad for easier number entry.
        /// </remarks>
        [EnumMember(Value = "number")]
        Number,

        /// <summary>
        /// Search input optimized for search queries with enhanced styling and potential search-specific browser features.
        /// </summary>
        /// <value>
        /// Represents the "search" input type for search functionality.
        /// </value>
        /// <remarks>
        /// Search inputs may have special styling such as rounded corners and a search icon. Some browsers provide additional features like a clear button (X) to quickly empty the search field.
        /// </remarks>
        [EnumMember(Value = "search")]
        Search,

        /// <summary>
        /// Telephone number input with specialized virtual keyboard layout and potential format validation for phone numbers.
        /// </summary>
        /// <value>
        /// Represents the "tel" input type for telephone number entry.
        /// </value>
        /// <remarks>
        /// This input type optimizes the virtual keyboard on mobile devices for phone number entry, typically displaying a numeric keypad with additional characters like + and * that are commonly used in phone numbers.
        /// </remarks>
        [EnumMember(Value = "tel")]
        Tel,

        /// <summary>
        /// URL input with validation for proper web address format and optimized virtual keyboard for URL entry.
        /// </summary>
        /// <value>
        /// Represents the "url" input type for web address entry.
        /// </value>
        /// <remarks>
        /// URL inputs provide format validation for web addresses and display an optimized virtual keyboard on mobile devices with easy access to common URL characters like forward slashes, dots, and the .com key.
        /// </remarks>
        [EnumMember(Value = "url")]
        URL
    }

    /// <summary>
    /// Defines the resize behavior and directional constraints for TextArea components, controlling how users can dynamically adjust the input area dimensions.
    /// </summary>
    /// <value>
    /// An enumeration that specifies the resize capabilities and directional constraints for TextArea components.
    /// </value>
    /// <remarks>
    /// The resize functionality allows users to adjust the TextArea dimensions to accommodate varying content lengths and personal preferences.
    /// Different resize modes provide flexibility while maintaining layout integrity:
    /// <list type="bullet">
    /// <item><description><strong>None:</strong> Maintains fixed dimensions for consistent layouts</description></item>
    /// <item><description><strong>Vertical:</strong> Allows height adjustment for accommodating more text lines</description></item>
    /// <item><description><strong>Horizontal:</strong> Allows width adjustment for longer text lines</description></item>
    /// <item><description><strong>Both:</strong> Provides maximum flexibility for user customization</description></item>
    /// </list>
    /// Consider the layout requirements and user needs when selecting the appropriate resize mode.
    /// </remarks>
    /// <example>
    /// Configuring TextArea resize behavior:
    /// <code><![CDATA[
    /// <SfTextArea ResizeMode="Resize.Vertical" Placeholder="Enter your message"></SfTextArea>
    /// ]]></code>
    /// </example>
    public enum Resize
    {
        /// <summary>
        /// The TextArea component maintains fixed dimensions and cannot be resized by the user in any direction.
        /// </summary>
        /// <value>
        /// Represents the "None" resize mode where the TextArea has fixed dimensions.
        /// </value>
        /// <remarks>
        /// When this mode is selected, the TextArea maintains its initial width and height settings and users cannot resize it. This ensures consistent layout appearance and prevents users from disrupting the page layout.
        /// </remarks>
        [EnumMember(Value = "None")]
        None,

        /// <summary>
        /// The TextArea component can be resized vertically to adjust height while maintaining a fixed width.
        /// </summary>
        /// <value>
        /// Represents the "Vertical" resize mode allowing height adjustment only.
        /// </value>
        /// <remarks>
        /// This mode allows users to adjust only the height of the TextArea by dragging the resize handle vertically. The width remains fixed, making it ideal for accommodating varying amounts of text content while maintaining consistent column layouts.
        /// </remarks>
        [EnumMember(Value = "Vertical")]
        Vertical,

        /// <summary>
        /// The TextArea component can be resized horizontally to adjust width while maintaining a fixed height.
        /// </summary>
        /// <value>
        /// Represents the "Horizontal" resize mode allowing width adjustment only.
        /// </value>
        /// <remarks>
        /// This mode allows users to adjust only the width of the TextArea by dragging the resize handle horizontally. The height remains fixed, which is useful for accommodating longer text lines while maintaining consistent row heights in the layout.
        /// </remarks>
        [EnumMember(Value = "Horizontal")]
        Horizontal,

        /// <summary>
        /// The TextArea component can be resized in both vertical and horizontal directions, providing complete dimensional flexibility.
        /// </summary>
        /// <value>
        /// Represents the "Both" resize mode allowing full dimensional adjustment.
        /// </value>
        /// <remarks>
        /// This mode provides maximum flexibility by allowing users to resize the TextArea in both width and height dimensions. Users can drag the resize handle in any direction to adjust the component size according to their content and preference needs.
        /// </remarks>
        [EnumMember(Value = "Both")]
        Both,

    }

    /// <summary>
    /// Internal representation of checkbox state transitions for managing visual and logical states.
    /// </summary>
    public enum CheckboxState
    {
        /// <summary>The checkbox is in the checked state.</summary>
        Checked,

        /// <summary>The checkbox is in the unchecked state.</summary>
        Unchecked,

        /// <summary>The checkbox is in the indeterminate (mixed) state.</summary>
        Indeterminate
    }
}
