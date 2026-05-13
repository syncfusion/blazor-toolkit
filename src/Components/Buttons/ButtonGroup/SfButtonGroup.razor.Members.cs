using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Buttons
{
    /// <content>
    /// Members (constants, fields, and properties) for <see cref="SfButtonGroup"/>.
    /// </content>
    public partial class SfButtonGroup
    {
        #region Properties

        /// <exclude/>
        /// <summary>
        /// Gets or sets the child content of the <see cref="SfButtonGroup"/>, which typically consists of <see cref="ButtonGroupButton"/> components.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> that represents the content to be rendered inside the ButtonGroup. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property is used to define the buttons or other elements that appear within the <see cref="SfButtonGroup"/>.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to provide child content to the <see cref="SfButtonGroup"/>.
        /// <code><![CDATA[
        /// <SfButtonGroup>
        ///   <ButtonGroupButton>Left</ButtonGroupButton>
        ///   <ButtonGroupButton>Center</ButtonGroupButton>
        ///   <ButtonGroupButton>Right</ButtonGroupButton>
        /// </SfButtonGroup>
        /// ]]></code>
        /// </example>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets a CSS class string to customize the appearance of the <see cref="SfButtonGroup"/>.
        /// </summary>
        /// <value>
        /// A string representing one or more CSS classes, separated by a space. The default value is <c>string.Empty</c>.
        /// </value>
        /// <remarks>
        /// Use this property to apply custom CSS classes to the root element of the ButtonGroup, allowing for tailored styling.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to apply a custom CSS class to the <see cref="SfButtonGroup"/>.
        /// <code><![CDATA[
        /// <SfButtonGroup CssClass="e-custom-style">
        ///   <ButtonGroupButton>Left</ButtonGroupButton>
        ///   <ButtonGroupButton>Center</ButtonGroupButton>
        ///   <ButtonGroupButton>Right</ButtonGroupButton>
        /// </SfButtonGroup>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <exclude/>
        /// <summary>
        /// Gets or sets a collection of additional HTML attributes that will be applied to the <see cref="SfButtonGroup"/> container element.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> where the key is a string representing the attribute name and the value is the attribute value. The default is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property allows you to add custom attributes to the ButtonGroup's root element. For example, you can add a <c>style</c> attribute to apply inline styles.
        /// </remarks>
        /// <example>
        /// In the following example, the <c>style</c> attribute is used to set a custom width for the <see cref="SfButtonGroup"/>.
        /// <code><![CDATA[
        /// <SfButtonGroup style="width:200px">
        ///   <ButtonGroupButton>Left</ButtonGroupButton>
        ///   <ButtonGroupButton>Center</ButtonGroupButton>
        ///   <ButtonGroupButton>Right</ButtonGroupButton>
        /// </SfButtonGroup>
        /// ]]></code>
        /// </example>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes { get; set; } = [];

        /// <summary>
        /// Gets or sets the selection mode of the <see cref="SfButtonGroup"/>.
        /// </summary>
        /// <value>
        /// One of the <see cref="SelectionMode"/> enumeration values. The default value is <see cref="SelectionMode.Default"/>.
        /// </value>
        /// <remarks>
        /// The selection mode determines how buttons are selected within the group:
        /// <list type="bullet">
        /// <item><description><see cref="SelectionMode.None"/>: No selection is enforced.</description></item>
        /// <item><description><see cref="SelectionMode.Single"/>: Allows only one button to be selected at a time, similar to radio buttons.</description></item>
        /// <item><description><see cref="SelectionMode.Multiple"/>: Allows multiple buttons to be selected, similar to checkboxes.</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// The following example configures the <see cref="SfButtonGroup"/> for single selection mode.
        /// <code><![CDATA[
        /// <SfButtonGroup Mode="SelectionMode.Single">
        ///   <ButtonGroupButton>Option 1</ButtonGroupButton>
        ///   <ButtonGroupButton>Option 2</ButtonGroupButton>
        ///   <ButtonGroupButton>Option 3</ButtonGroupButton>
        /// </SfButtonGroup>
        /// ]]></code>
        /// </example>
        [Parameter]
        public SelectionMode Mode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the buttons in the <see cref="SfButtonGroup"/> are arranged vertically.
        /// </summary>
        /// <value>
        /// <see langword="true"/> to arrange buttons vertically; <see langword="false"/> to arrange them horizontally. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// When set to <see langword="true"/>, the CSS class <c>e-vertical</c> is applied to the ButtonGroup container, causing the buttons to be displayed in a vertical layout.
        /// When set to <see langword="false"/>, the <c>e-vertical</c> class is removed, displaying buttons in their default horizontal layout.
        /// This property supports dynamic changes and will update the layout immediately when modified.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to create a vertically oriented <see cref="SfButtonGroup"/>.
        /// <code><![CDATA[
        /// <SfButtonGroup IsVertical="true">
        ///   <ButtonGroupButton>Top</ButtonGroupButton>
        ///   <ButtonGroupButton>Middle</ButtonGroupButton>
        ///   <ButtonGroupButton>Bottom</ButtonGroupButton>
        /// </SfButtonGroup>
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool IsVertical { get; set; }

        #endregion
    }
}
