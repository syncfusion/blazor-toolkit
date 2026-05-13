using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Buttons
{
    public partial class SfButton
    {
        #region Members
        /// <exclude />
        /// <summary>
        /// Gets or sets the child content of the button, which can include custom HTML elements.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> that represents the content rendered inside the button. The default is <c>null</c>.
        /// </value>
        /// <remarks>
        /// When <see cref="ChildContent"/> is not specified, the button's content is rendered from the <see cref="Content"/> property.
        /// </remarks>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment? ChildContent { get; set; }
        /// <summary> 
        /// Gets or sets the string content to be displayed in the <see cref="SfButton"/> component.
        /// </summary> 
        /// <value> 
        /// A <c>string</c> value that represents the button's text. The default value is <c>String.Empty</c>.
        /// </value> 
        /// <remarks> 
        /// To use HTML structure within the button, specify it as child content using the <see cref="ChildContent"/> property.
        /// </remarks> 
        /// <example>
        /// The following code example shows how to specify button content via this property:
        /// <code><![CDATA[
        /// <SfButton Content="Submit" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Content { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets one or more CSS class names to customize the visual appearance of the button.
        /// </summary> 
        /// <value>
        /// A <c>string</c> containing space-separated CSS class names. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// Use this property to apply one or more custom styles to the button in addition to Syncfusion built-in classes.
        /// </remarks>
        /// <example>
        /// This example demonstrates applying a style via the <see cref="CssClass"/> property:
        /// <code><![CDATA[
        /// <SfButton CssClass="e-primary e-custom-style" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value that determines whether the button control is enabled or disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the component is disabled; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When this property is set to <c>true</c>, all user interaction with the button is prevented.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton Content="Save" Disabled="true" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets the CSS class name(s) for including an icon or image in the button.
        /// </summary>
        /// <value>
        /// A <c>string</c> containing one or more CSS classes for icons/images. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// When set, the button displays the specified icon or image alongside its content.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton IconCss="e-icons e-add" Content="Add Item" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string IconCss { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets how the icon is positioned relative to button content.
        /// </summary>
        /// <value>
        /// A <see cref="IconPosition"/> enumeration value specifying the icon position. The default is <see cref="IconPosition.Right"/>, <see cref="IconPosition.Top"/>, and <see cref="IconPosition.Bottom"/>.
        /// </value>
        /// <remarks>
        /// Position options include <see cref="IconPosition.Left"/>, <see cref="IconPosition.Right"/>, <see cref="IconPosition.Top"/>, and <see cref="IconPosition.Bottom"/>.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton IconCss="e-icons e-edit" IconPosition="IconPosition.Right" Content="Edit" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public IconPosition IconPosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the button is styled as a primary action button.
        /// </summary>
        /// <value>
        /// <c>true</c> to use the primary style. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Primary buttons highlight important actions in the UI using distinctive styling.
        /// <para>This property is <c>virtual</c> to allow derived components to override the default behavior.
        /// <see cref="SfFab"/> overrides this property to return <c>true</c> by default, reflecting the
        /// FAB design convention of always rendering as a primary action button.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton IsPrimary="true" Content="Submit" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public virtual bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the button acts as a toggle button.
        /// </summary>
        /// <value>
        /// <c>true</c> for a toggleable button; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When enabled, each click toggles the button's active state.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton IsToggle="true" Content="Bold" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool IsToggle { get; set; }

        /// <summary>
        /// Gets or sets the type of the button to control its form behavior.
        /// </summary>
        /// <value>
        /// A <see cref="ButtonType"/> enumeration value. The default is <see cref="ButtonType.Button"/>.
        /// </value>
        /// <remarks>
        /// Set <see cref="ButtonType.Submit"/> to trigger form validation and submission, or <see cref="ButtonType.Reset"/> to clear all form fields back to their initial values. The default <see cref="ButtonType.Button"/> does not interact with forms.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfButton Type="ButtonType.Submit" Content="Submit" />
        /// <SfButton Type="ButtonType.Reset" Content="Clear" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public ButtonType Type { get; set; } = ButtonType.Button;

        #endregion
    }
}
