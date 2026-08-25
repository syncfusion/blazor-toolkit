using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Partial class containing parameter and member property definitions for <see cref="SfSelectionBase{TChecked}"/>.
    /// </summary>
    public partial class SfSelectionBase<TChecked>
    {
        #region Members

        /// <summary>
        /// Gets or sets the CSS class name(s) to customize the visual appearance of the component.
        /// </summary>
        /// <value>
        /// A space-separated string of CSS class names. The default value is <see cref="string.Empty"/>.
        /// </value>
        /// <remarks>
        /// Use <see cref="CssClass"/> property to apply custom styling, themes, or branding to the component.
        /// Multiple classes can be specified by separating them with spaces.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCheckBox CssClass="custom-checkbox primary-theme" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name attribute of the _input element for form submission.
        /// </summary>
        /// <value>
        /// A string representing the _input's name. The default value is <see cref="string.Empty"/>.
        /// </value>
        /// <remarks>
        /// The <see cref="Name"/> property identifies the component's value when a form is submitted.
        /// It is essential for proper form integration and server-side model binding.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfRadioButton Name="gender" Value="male" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value to be submitted with the form data.
        /// </summary>
        /// <value>
        /// A string representing the _input value. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// The <see cref="Value"/> property specifies the value that will be sent to the server when the form is submitted.
        /// For radio buttons, this typically represents the unique value for each option in a group.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCheckBox Value="subscribe" Name="newsletter" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the checked state of the component.
        /// </summary>
        /// <value>
        /// A value of type <typeparamref name="TChecked"/> (typically <see cref="bool"/> for checkboxes or <see cref="string"/> for radio buttons).
        /// </value>
        /// <remarks>
        /// This <see cref="Checked"/> property controls the selection state of the component and determines whether it is rendered as checked or selected.
        /// Changing this value updates the visual state and can trigger two-way binding updates.
        /// Use <c>@bind-Checked</c> to enable two-way binding.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCheckBox @bind-Checked="isAccepted" />
        /// <SfRadioButton TValue="string" @bind-Checked="selectedOption" Value="option1" />
        /// @code {
        ///     private bool isAccepted;
        ///     private string selectedOption;
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public TChecked? Checked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the component is disabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the component is disabled; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When <see cref="Disabled"/> is <c>true</c>, the component is non-interactive and cannot receive user _input.
        /// The component will also display visual styling indicating its disabled state.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfRadioButton Disabled="true" Label="Unavailable Option" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to persist the component's state across browser sessions.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable state persistence; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When <see cref="EnablePersistence"/> is <c>true</c>, the component's checked state is saved to the browser's localStorage
        /// and automatically restored when the page is reloaded. This provides a better user experience
        /// by maintaining the component's state across sessions.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCheckBox EnablePersistence="true" Label="Remember my choice" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <exclude />
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes
        {
            get => _inputAttributes;
            set => _inputAttributes = value;
        }

        /// <summary>
        /// Gets or sets the accessible name for the component when no visible <see cref="Label"/> is supplied.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> that is announced by screen readers in place of (or in addition to) the visible label.
        /// The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// Use this property to provide a meaningful accessible name for assistive technologies when the
        /// component is not accompanied by visible label text. When both <see cref="Label"/> and
        /// <see cref="AriaLabel"/> are supplied, the visible label takes precedence for sighted users and the
        /// component continues to use the visible label as its accessible name.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCheckBox AriaLabel="Subscribe to newsletter" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string? AriaLabel { get; set; }

        #endregion
    }
}
