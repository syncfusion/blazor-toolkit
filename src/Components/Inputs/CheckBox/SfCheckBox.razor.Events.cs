using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public partial class SfCheckBox<TChecked>
    {
        #region Events

        /// <summary>
        /// Gets or sets an event callback that is invoked when the checkbox state changes due to user interaction.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> of <see cref="ChangeEventArgs{TChecked}"/> that is triggered when the 
        /// checkbox state is changed by the user. The callback argument contains the updated checked state and the 
        /// mouse event details.
        /// </value>
        /// <remarks>
        /// <para>
        /// This event is raised only from UI-based interactions (such as mouse or touch clicks), not from programmatic 
        /// changes to the <see cref="SfSelectionBase{TChecked}.Checked"/> property.
        /// </para>
        /// <para>
        /// Use this event to react to state changes for:
        /// </para>
        /// <list type="bullet">
        /// <item><description>Form validation</description></item>
        /// <item><description>Conditional logic based on checkbox state</description></item>
        /// <item><description>Synchronizing with other UI elements</description></item>
        /// <item><description>Analytics or logging</description></item>
        /// </list>
        /// <para>
        /// For two-way data binding without custom logic, use <c>@bind-Checked</c> instead.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>Basic value change handling:</para>
        /// <code><![CDATA[
        /// <SfCheckBox Label="Accept Terms"
        ///             @bind-Checked="acceptedTerms"
        ///             ValueChange="OnTermsChanged" />
        /// 
        /// @code {
        ///     private bool acceptedTerms;
        ///     
        ///     private void OnTermsChanged(CheckedChangeEventArgs<bool> args)
        ///     {
        ///         Console.WriteLine($"Checkbox changed to: {args.Checked}");
        ///         
        ///         // Perform validation or other logic
        ///         if (args.Checked)
        ///         {
        ///             // User accepted terms
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// <para>Tri-state checkbox with value change:</para>
        /// <code><![CDATA[
        /// <SfCheckBox TChecked="bool?"
        ///             @bind-Checked="selectAll"
        ///             EnableTriState="true"
        ///             Label="Select All"
        ///             ValueChange="OnSelectAllChanged" />
        /// 
        /// @code {
        ///     private bool? selectAll;
        ///     
        ///     private void OnSelectAllChanged(CheckedChangeEventArgs<bool?> args)
        ///     {
        ///         if (args.Checked == true)
        ///         {
        ///             // Select all items
        ///         }
        ///         else if (args.Checked == false)
        ///         {
        ///             // Deselect all items
        ///         }
        ///         else
        ///         {
        ///             // Indeterminate state (null)
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<CheckedChangeEventArgs<TChecked>> ValueChange { get; set; }

        #endregion
    }
}
