using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// The DateTimePicker is a graphical user interface component that allows users to select both date and time values through an interactive popup interface.
    /// </summary>
    public partial class SfDateTimePicker<TValue>
    {
        /// <summary>
        /// Returns the DOM element id that the input's <c>aria-owns</c> attribute should point to
        /// while a popup is open. Both the calendar popup and the time-list popup share the same
        /// DOM id suffix (<c>_popup</c>, matching the <c>id="@(ID)_popup"</c> declarations in
        /// <c>SfDateTimePicker.razor</c>), so we delegate to the base implementation which returns
        /// the same <c>ID + "_popup"</c> id.
        /// </summary>
        /// <returns>The element id that <c>aria-owns</c> should reference.</returns>
        /// <remarks>
        /// Earlier revisions of this override returned <c>_popups</c> / <c>_timepopup</c>, but the
        /// rendered DOM uses <c>_popup</c> for both popups, which left <c>aria-owns</c> pointing at
        /// a non-existent id and triggered axe-core's <c>aria-valid-attr-value</c> (critical) rule.
        /// Overriding is still useful to lock the behaviour at this level so future drift in the
        /// base class does not silently change the DateTimePicker's accessibility contract.
        /// </remarks>
        protected override string GetAriaOwnsTarget()
        {
            return base.GetAriaOwnsTarget();
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
