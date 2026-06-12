using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Buttons
{
    public partial class Button
    {
        #region Properties

        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets a CSS class string to customize the appearance of the button.
        /// </summary>
        /// <value>
        /// A string that represents the CSS class. The default value is <c>string.Empty</c>.
        /// </value>
        /// <remarks>
        /// You can add multiple classes separated by spaces to apply custom styles to the button.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to apply a custom CSS class to a <see cref="Button"/>.
        /// <code><![CDATA[
        /// <SfButtonGroup>
        ///   <Button CssClass="custom-style">Left</Button>
        ///   <Button>Center</Button>
        ///   <Button>Right</Button>
        /// </SfButtonGroup>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a CSS class string to include an icon on the button.
        /// </summary>
        /// <value>
        /// A string representing the icon's CSS class, with support for multiple classes separated by spaces. The default value is <c>string.Empty</c>.
        /// </value>
        /// <remarks>
        /// This property is used to apply a CSS class that defines the button's icon, typically by setting a background image.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to add icons to <see cref="Button"/> components.
        /// <code><![CDATA[
        /// <SfButtonGroup>
        ///   <Button IconCss="e-icons e-cut">Cut</Button>
        ///   <Button IconCss="e-icons e-copy">Copy</Button>
        ///   <Button IconCss="e-icons e-paste">Paste</Button>
        /// </SfButtonGroup>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string IconCss { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the button is disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the button is disabled; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// A disabled button cannot be clicked or focused, and its appearance will be altered to indicate its inactive state.
        /// </remarks>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the button is in the selected state.
        /// </summary>
        /// <value>
        /// <c>true</c> if the button is selected; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property is particularly useful when the parent <see cref="SfButtonGroup"/> is configured for single or multiple selection modes using the <see cref="SfButtonGroup.Mode"/> property.
        /// Use <c>@bind-Selected</c> to enable two-way binding.
        /// </remarks>
        /// <example>
        /// The following example demonstrates two-way binding of the <see cref="Selected"/> state.
        /// <code><![CDATA[
        /// <SfButtonGroup Mode="SelectionMode.Multiple">
        ///   <Button @bind-Selected="isSelected">Option</Button>
        /// </SfButtonGroup>
        /// @code {
        ///   bool isSelected = true;
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool Selected { get; set; }

        /// <summary>
        /// Gets or sets the name attribute for the button's input element.
        /// </summary>
        /// <value>
        /// A string that represents the name of the button. The default value is <c>string.Empty</c>.
        /// </value>
        /// <remarks>
        /// The <see cref="Name"/> property is used to set the <c>name</c> attribute of the underlying HTML input element, which is useful for form submissions.
        /// </remarks>
        /// <example>
        /// Use the <see cref="Name"/> property to group radio inputs when using single-selection mode.
        /// <code><![CDATA[
        /// <SfButtonGroup Mode="SelectionMode.Single">
        ///   <Button Name="group1">A</Button>
        ///   <Button Name="group1">B</Button>
        /// </SfButtonGroup>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value attribute for the button's input element.
        /// </summary>
        /// <value>
        /// A string that represents the value of the button. The default value is <c>string.Empty</c>.
        /// </value>
        /// <remarks>
        /// The <see cref="Value"/> property is used to set the <c>value</c> attribute of the underlying HTML input element, which is useful for form submissions.
        /// </remarks>
        /// <example>
        /// Set a custom <see cref="Value"/> when using the group within a form to identify the selected option.
        /// <code><![CDATA[
        /// <SfButtonGroup Mode="SelectionMode.Single">
        ///   <Button Value="left">Left</Button>
        ///   <Button Value="right">Right</Button>
        /// </SfButtonGroup>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text content to be displayed on the button.
        /// </summary>
        /// <value>
        /// A string representing the text to be displayed. The default value is <c>string.Empty</c>.
        /// </value>
        /// <remarks>
        /// For rendering complex HTML content, it is recommended to use the <see cref="ChildContent"/> property instead.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to set the text content for <see cref="Button"/> components.
        /// <code><![CDATA[
        /// <SfButtonGroup>
        ///   <Button Content="Left"></Button>
        ///   <Button Content="Center"></Button>
        ///   <Button Content="Right"></Button>
        /// </SfButtonGroup>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the button functions as a toggle button.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable toggle functionality; otherwise, <c>false</c>. The default is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When <see cref="IsToggle"/> is set to <c>true</c>, the button persists in a selected state after being clicked and can be deselected by clicking it again.
        /// </remarks>
        /// <example>
        /// Enable toggle behavior so the button stays selected after a click.
        /// <code><![CDATA[
        /// <SfButtonGroup Mode="SelectionMode.Multiple">
        ///   <Button IsToggle="true">Toggle Me</Button>
        /// </SfButtonGroup>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool IsToggle { get; set; }

        /// <summary>
        /// Gets or sets the position of the icon relative to the text content.
        /// </summary>
        /// <value>
        /// One of the <see cref="IconPosition"/> enumeration values that specifies the icon's placement. The default value is <see cref="IconPosition.Left"/>.
        /// </value>
        /// <remarks>
        /// <para>The available positions are:</para>
        /// <para><see cref="IconPosition.Left"/>: Places the icon to the left of the text.</para>
        /// <para><see cref="IconPosition.Right"/>: Places the icon to the right of the text.</para>
        /// <para><see cref="IconPosition.Top"/>: Places the icon above the text.</para>
        /// <para><see cref="IconPosition.Bottom"/>: Places the icon below the text.</para>
        /// </remarks>
        /// <example>
        /// Place an icon to the right of the text.
        /// <code><![CDATA[
        /// <SfButtonGroup>
        ///   <Button IconCss="e-icons e-save" IconPosition="IconPosition.Right">Save</Button>
        /// </SfButtonGroup>
        /// ]]></code>
        /// </example>
        [Parameter]
        public IconPosition IconPosition { get; set; }

        /// <exclude />
        [Parameter(CaptureUnmatchedValues = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, object> HtmlAttributes
        {
            get => _inputAttributes; set => _inputAttributes = value;
        }

        /// <exclude />
        /// <summary>
        /// Internal cached ID for the input element to maintain stable for attribute linkage.
        /// </summary>
        internal string InputId { get; private set; } = string.Empty;

        #endregion
    }
}
