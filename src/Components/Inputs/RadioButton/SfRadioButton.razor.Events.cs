using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public partial class SfRadioButton<TChecked>
    {
        /// <summary>
        /// Gets or sets a callback invoked when the radio button's value changes due to user interaction.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> of type <see cref="ChangeArgs{TChecked}"/> that is invoked when the checked state changes because of user action.
        /// The callback receives a <see cref="ChangeArgs{TChecked}"/> instance containing the new value and the original event data.
        /// </value>
        /// <remarks>
        /// <para>
        /// The <see cref="ValueChange"/> event is raised only for user-initiated changes (for example, clicks). It is suitable for reacting to user selection,
        /// updating other UI state, or performing validation. Programmatically setting the <see cref="SfSelectionBase{TChecked}.Checked"/> property from code does not automatically trigger this event.
        /// </para>
        /// <para>
        /// When handling the callback, prefer using the provided <see cref="ChangeArgs{TChecked}"/> to access both the value and the originating event payload.
        /// </para>
        /// </remarks>
        /// <example>
        /// The following example demonstrates registering a handler for the <c>ValueChange</c> event:
        /// <code><![CDATA[
        /// <SfRadioButton ValueChange="OnRadioChanged" />
        /// 
        /// @code {
        ///     private void OnRadioChanged(ChangeArgs<string> args)
        ///     {
        ///         // React to the new value: args.Value
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<ChangeArgs<TChecked>> ValueChange { get; set; }
    }
}
