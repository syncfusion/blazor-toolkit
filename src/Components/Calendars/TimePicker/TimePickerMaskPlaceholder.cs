using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Calendars.Interfaces;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// Configures the placeholder text for the <see cref="SfTimePicker{TValue}"/> mask, which is displayed based on the specified format until the user enters a value.
    /// </summary>
    /// <remarks>
    /// The properties of this class are only applicable when the <see cref="SfTimePicker{TValue}.EnableMask"/> property is set to <c>true</c>.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfTimePicker TValue="DateTime?" EnableMask="true">
    /// <TimePickerMaskPlaceholder Hour="hh" Minute="mm" Second="ss" />
    /// </SfTimePicker>
    /// ]]></code>
    /// </example>
    public class TimePickerMaskPlaceholder : MaskPlaceholder
    {
        /// <summary>
        /// Gets or sets the parent component, which is an instance of <see cref="IMaskPlaceholder"/>.
        /// </summary>
        [CascadingParameter]
        private IMaskPlaceholder? Parent { get; set; }

        /// <summary>
        /// Initializes the component and updates the parent with its properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous initialization operation.</returns>
        /// <remarks>
        /// This method is called by the Blazor framework when the component is first initialized.
        /// </remarks>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            Parent?.UpdateChildProperties(this);
        }
    }
}
