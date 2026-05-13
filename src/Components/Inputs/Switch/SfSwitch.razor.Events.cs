using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public partial class SfSwitch<TChecked>
    {
        /// <summary>
        /// Gets or sets the event callback that fires when the checked state changes through UI interaction.
        /// </summary>
        /// <value>An <see cref="EventCallback{TValue}"/> invoked when the value changes.</value>
        [Parameter] public EventCallback<CheckedChangeEventArgs<TChecked>> ValueChange { get; set; }
    }
}
