using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Calendars.Interfaces;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// Provides configuration for placeholder text to be displayed in a masked <see cref="SfDatePicker{TValue}"/> control, based on the <see cref="SfDatePicker{TValue}.Format"/>, until the user enters a value.
    /// </summary>
    /// <remarks>
    /// The <c>DatePickerMaskPlaceholder</c> class enables customization of day, month, and year placeholder text in the <see cref="SfDatePicker{TValue}"/> input mask.
    /// Its properties are effective only when <see cref="SfDatePicker{TValue}.EnableMask"/> is set to <c>true</c>.
    /// This class inherits from <see cref="MaskPlaceholder"/>, which provides the base structure for mask placeholder customization.
    /// </remarks>
    /// <example>
    /// This example demonstrates how to configure custom placeholders for day, month, and year segments in a date picker mask.
    /// <code><![CDATA[
    /// <SfDatePicker TValue="DateTime" EnableMask="true">
    ///     <DatePickerMaskPlaceholder Day="day" Month="month" Year="year"></DatePickerMaskPlaceholder>
    /// </SfDatePicker>
    /// ]]></code>
    /// </example>
    public class DatePickerMaskPlaceholder : MaskPlaceholder
    {
        /// <summary>
        /// Gets or sets a cascading parameter that provides the parent mask placeholder component.
        /// </summary>
        /// <exclude/>
        [CascadingParameter]
        private IMaskPlaceholder? Parent { get; set; }

        /// <summary>
        /// Executes component initialization logic. Notifies the parent <see cref="IMaskPlaceholder"/> of updated child placeholder properties.
        /// </summary>
        /// <remarks>
        /// This method is called by the framework during the asynchronous initialization of the component.
        /// It ensures that any parent component implementing <see cref="IMaskPlaceholder"/> is notified of property changes, supporting proper propagation of mask placeholder configuration for child components.
        /// </remarks>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        /// <exclude />
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
            Parent?.UpdateChildProperties(this);
        }
    }
}
