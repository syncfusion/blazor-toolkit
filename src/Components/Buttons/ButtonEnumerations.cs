using System.Runtime.Serialization;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Specifies the possible label positions for components supporting label alignment.
    /// </summary>
    /// <remarks>
    /// The <see cref="LabelPosition"/> enumeration allows you to choose whether the label appears before or after
    /// the associated component, such as a button or an input. This enables customization of the UI layout and accessibility support.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to set the label position for a button:
    /// <code><![CDATA[
    /// <SfButton Label="Save" LabelPosition="LabelPosition.After" />
    /// ]]></code>
    /// </example>
    public enum LabelPosition
    {
        /// <summary>
        /// Positions the label after the component (for example: text will be rendered to the right of a button).
        /// </summary>
        /// <value>
        /// Represents the label placed after the associated component.
        /// </value>
        /// <remarks>
        /// Use <see cref="After"/> to display the label following the component.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton Label="Click Me" LabelPosition="LabelPosition.After" />
        /// ]]></code>
        /// </example>
        After,

        /// <summary>
        /// Positions the label before the component (for example: text will be rendered to the left of a button).
        /// </summary>
        /// <value>
        /// Represents the label placed before the associated component.
        /// </value>
        /// <remarks>
        /// Use <see cref="Before"/> to display the label preceding the component.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton Label="Click Me" LabelPosition="LabelPosition.Before" />
        /// ]]></code>
        /// </example>
        Before,
    }

    /// <summary>
    /// Specifies the selection behavior of the <see cref="SfButtonGroup"/>.
    /// </summary>
    public enum SelectionMode
    {
        /// <summary>
        /// No items can be selected. Selection is disabled.
        /// </summary>
        None,

        /// <summary>
        /// Only one item can be selected at a time. Selecting a new item automatically deselects the previously selected item.
        /// </summary>
        Single,

        /// <summary>
        /// Multiple items can be selected simultaneously. Users can select and deselect items independently.
        /// </summary>
        Multiple,
    }

    /// <summary>
    /// Specifies the layout position of an icon inside a <see cref="SfButton"/>.
    /// </summary>
    /// <remarks>
    /// This enumeration determines where the icon is placed relative to the button content: left, right, above, or below.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfButton IconCss="e-icons e-search" IconPosition="IconPosition.Top" Content="Search" />
    /// ]]></code>
    /// </example>
    public enum IconPosition
    {
        /// <summary>
        /// Positions the icon to the left of the button content.
        /// </summary>
        Left,

        /// <summary>
        /// Positions the icon to the right of the button content.
        /// </summary>
        Right,

        /// <summary>
        /// Positions the icon above the button content.
        /// </summary>
        Top,

        /// <summary>
        /// Positions the icon below the button content.
        /// </summary>
        Bottom,
    }

    /// <summary>
    /// Defines the button types for HTML button element behavior.
    /// </summary>
    /// <remarks>
    /// This enumeration allows you to specify how the button interacts with forms.
    /// Use these types to control form submission, reset, or default button behavior.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to use button types:
    /// <code><![CDATA[
    /// <SfButton Type="ButtonType.Submit" Content="Submit Form" />
    /// ]]></code>
    /// </example>
    public enum ButtonType
    {
        /// <summary>
        /// Specifies that the button is a standard button that does not interact with forms.
        /// </summary>
        /// <remarks>
        /// This is the default button type and won't trigger form submission or reset.
        /// </remarks>
        [EnumMember(Value = "Button")]
        Button,

        /// <summary>
        /// Specifies that the button submits the form when clicked.
        /// </summary>
        /// <remarks>
        /// Use this type to trigger form validation and submission.
        /// </remarks>
        [EnumMember(Value = "Submit")]
        Submit,

        /// <summary>
        /// Specifies that the button resets all form fields to their initial values when clicked.
        /// </summary>
        /// <remarks>
        /// Use this type to clear form fields back to their default state.
        /// </remarks>
        [EnumMember(Value = "Reset")]
        Reset
    }
}