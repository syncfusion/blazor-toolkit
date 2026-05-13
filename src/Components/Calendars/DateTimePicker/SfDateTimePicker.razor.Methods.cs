using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// The DateTimePicker is a graphical user interface component that allows users to select both date and time values through an interactive popup interface.
    /// </summary>
    public partial class SfDateTimePicker<TValue>
    {
        /// <summary>
        /// Opens the date picker popup to show the calendar for date selection.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method programmatically opens the calendar popup, allowing users to select a date.
        /// It's useful for implementing custom UI behaviors or keyboard shortcuts that should open the date picker.
        /// </remarks>
        /// <example>
        /// Opening the date popup programmatically:
        /// <code><![CDATA[
        /// @ref SfDateTimePicker<DateTime> dateTimePicker;
        /// 
        /// private async Task OpenCalendar()
        /// {
        ///     await dateTimePicker.ShowDatePopupAsync();
        /// }
        /// ]]></code>
        /// </example>
        public async Task ShowDatePopupAsync()
        {
            await OpenPopupAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Opens the time picker popup to show the time selection list.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method programmatically opens the time selection popup, displaying a list of available time options
        /// based on the configured step interval. The time icon is also activated to reflect the popup state.
        /// It's useful for implementing custom behaviors that need to show the time picker.
        /// </remarks>
        /// <example>
        /// Opening the time popup programmatically:
        /// <code><![CDATA[
        /// @ref SfDateTimePicker<DateTime> dateTimePicker;
        /// 
        /// private async Task OpenTimeList()
        /// {
        ///     await dateTimePicker.ShowTimePopupAsync();
        /// }
        /// ]]></code>
        /// </example>
        public async Task ShowTimePopupAsync()
        {
            IsDatePickerPopup = false;
            if (TimeIcon is not null)
            {
                TimeIcon = SfBaseUtils.AddClass(TimeIcon, ACTIVE);
            }
            await OpenPopupAsync(null).ConfigureAwait(false);
        }
    }
}
