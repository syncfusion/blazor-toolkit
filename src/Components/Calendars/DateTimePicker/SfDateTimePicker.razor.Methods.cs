using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// The DateTimePicker is a graphical user interface component that allows users to select both date and time values through an interactive popup interface.
    /// </summary>
    public partial class SfDateTimePicker<TValue>
    {
        /// <summary>
        /// Suffix appended to the component <see cref="CalendarBase{TValue}.ID"/> to form the DOM
        /// id of the time-list popup rendered by the <see cref="SfDateTimePicker{TValue}"/>.
        /// Kept in sync with the <c>ID="@(ID + &quot;_timepopup&quot;)"</c> declaration on the
        /// <c>&lt;SfPopup @ref=&quot;_timePopupRef&quot;&gt;</c> element in
        /// <c>SfDateTimePicker.razor</c>.
        /// </summary>
        private const string TIMEPOPUP_SUFFIX = "_timepopup";
        private const string POPUPS = "_popups";

        /// <summary>
        /// Returns the DOM element id that the input's <c>aria-owns</c> attribute should point to
        /// while a popup is open. Overrides the base implementation so the value resolves to the
        /// popup that is currently in the accessibility tree: the time-list popup
        /// (<c>ID + &quot;_timepopup&quot;</c>) when <see cref="ShowPopupList"/> is true,
        /// otherwise the calendar popup (<c>ID + POPUPS</c>).
        /// </summary>
        /// <returns>The element id that <c>aria-owns</c> should reference.</returns>
        /// <remarks>
        /// Without this override, the inherited <see cref="SfDatePicker{TValue}.GetAriaOwnsTarget"/>
        /// always returns the calendar popup id; when only the time popup is open
        /// (e.g. <see cref="ShowTimePopupAsync"/>) that id is not in the accessibility tree and
        /// axe-core flags the input with <c>aria-valid-attr-value</c> (critical).
        /// </remarks>
        protected override string GetAriaOwnsTarget()
        {
            return ShowPopupList ? ID + TIMEPOPUP_SUFFIX : ID + POPUPS;
        }
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
