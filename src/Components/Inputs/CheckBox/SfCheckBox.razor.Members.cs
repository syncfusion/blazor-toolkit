using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public partial class SfCheckBox<TChecked>
    {
        #region Members

        /// <summary>
        /// Gets or sets the label text displayed next to the checkbox.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the label content for the checkbox. The default value is <see cref="string.Empty"/>.
        /// </value>
        /// <remarks>
        /// <para>
        /// This property specifies the descriptive text shown beside the checkbox element. 
        /// Use this to provide clear context about what the checkbox represents.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCheckBox Label="Accept Terms and Conditions" @bind-Checked="acceptedTerms" />
        /// 
        /// @code {
        ///     private bool acceptedTerms;
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the position of the label relative to the checkbox.
        /// </summary>
        /// <value>
        /// One of the <see cref="LabelPosition"/> enumeration values: <see cref="LabelPosition.Before"/> or 
        /// <see cref="LabelPosition.After"/>. The default value is <see cref="LabelPosition.Before"/>.
        /// </value>
        /// <remarks>
        /// <para>
        /// <see cref="LabelPosition.Before"/>: The label appears to the left of the checkbox (in LTR layouts).
        /// </para>
        /// <para>
        /// <see cref="LabelPosition.After"/>: The label appears to the right of the checkbox (in LTR layouts).
        /// </para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCheckBox Label="I agree to the terms" 
        ///             LabelPosition="LabelPosition.After"
        ///             @bind-Checked="agreed" />
        /// 
        /// @code {
        ///     private bool agreed;
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public LabelPosition LabelPosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the checkbox is in the indeterminate (mixed) state.
        /// </summary>
        /// <value>
        /// <c>true</c> if the checkbox is in the indeterminate state; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// <para>
        /// The indeterminate state represents a "mixed" or "partially checked" state, typically used in hierarchical 
        /// scenarios where some but not all child items are selected.
        /// </para>
        /// <para>
        /// Setting <see cref="Indeterminate"/> to <c>true</c> visually displays the checkbox in the indeterminate state 
        /// and sets the native HTML <c>indeterminate</c> property, which is properly announced by assistive technologies 
        /// as "mixed" or "partially checked" across supported platforms and environments.
        /// </para>
        /// <para>
        /// When the user clicks an indeterminate checkbox, it transitions based on the <see cref="EnableTriState"/> setting.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>Hierarchical checkbox scenario:</para>
        /// <code><![CDATA[
        /// <SfCheckBox @bind-Checked="selectAll" 
        ///             @bind-Indeterminate="isPartiallySelected"
        ///             Label="Select All"
        ///             ValueChange="OnParentChanged" />
        /// 
        /// @code {
        ///     private bool? selectAll;
        ///     private bool isPartiallySelected;
        ///     
        ///     private void OnParentChanged(ChangeEventArgs<bool?> args)
        ///     {
        ///         // Handle parent checkbox state change
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool Indeterminate { get; set; }

        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EventCallback<bool> IndeterminateChanged { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the checkbox supports three-state mode (checked, unchecked, and indeterminate).
        /// </summary>
        /// <value>
        /// <c>true</c> if tri-state mode is enabled; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// <para>
        /// When enabled, the checkbox cycles through three states when clicked:
        /// </para>
        /// <list type="number">
        /// <item><description>Checked</description></item>
        /// <item><description>Indeterminate</description></item>
        /// <item><description>Unchecked</description></item>
        /// </list>
        /// <para>
        /// Tri-state mode is particularly useful for "Select All" scenarios in hierarchical lists or 
        /// when representing partial selections.
        /// </para>
        /// <para>
        /// To support tri-state functionality, use a nullable boolean type for <typeparamref name="TChecked"/> 
        /// (e.g., <c>bool?</c>).
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>Tri-state checkbox with nullable boolean:</para>
        /// <code><![CDATA[
        /// <SfCheckBox TChecked="bool?"
        ///             @bind-Checked="triStateValue"
        ///             EnableTriState="true"
        ///             Label="Select All Items" />
        /// 
        /// @code {
        ///     private bool? triStateValue = null; // null = indeterminate
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool EnableTriState { get; set; }

        #endregion
    }
}
