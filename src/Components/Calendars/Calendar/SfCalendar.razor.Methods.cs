using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// The <see cref="SfCalendar{TValue}"/> component displays a visual Gregorian calendar allowing users to select one or multiple dates interactively, supporting navigation and multiple selection modes for varied scenarios such as event scheduling, booking, or date range input.
    /// </summary>
    /// <remarks>
    /// This component provides advanced features for date selection and navigation including single and multi-select, programmatic navigation, and state persistence. Use this component to enable rich and interactive calendar functionalities within your Blazor application.
    /// </remarks>
    /// <example>
    /// Here is an example of rendering an SfCalendar in a Blazor component:
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime" />
    /// ]]></code>
    /// </example>
    public partial class SfCalendar<TValue> : CalendarBase<TValue>
    {
        /// <summary>
        /// Retrieves the data to be persisted for this calendar instance upon browser refresh or reload.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a <c>string</c> with the persisted state for the calendar instance.
        /// </returns>
        /// <remarks>
        /// Use this method to fetch stateful data such as selected values from browser storage for the current <see cref="SfCalendar{TValue}"/> instance using its unique identifier.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var persistData = await calendar.GetPersistDataAsync();
        /// ]]></code>
        /// </example>
        public async Task<string> GetPersistDataAsync()
        {
            return await InvokeAsync<string>(_baseJsModule!, _baseJsInProcessModule!, "getLocalStorageItem", [ID]).ConfigureAwait(true);
        }

        /// <summary>
        /// Returns the current view in which the calendar is displayed (such as Month, Year, or Decade).
        /// </summary>
        /// <returns>
        /// A <c>string</c> that represents the current calendar view. Can be <c>"Month"</c>, <c>"Year"</c>, or <c>"Decade"</c>.
        /// </returns>
        /// <remarks>
        /// Use this method to check which logical view is currently shown to the user. Typical use is in UI customization or conditionally showing controls based on calendar state.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var view = calendar.CurrentView();
        /// ]]></code>
        /// </example>
        public string CurrentView()
        {
            return CalendarBase?.CurrentView() ?? string.Empty;
        }

        /// <summary>
        /// Navigates the calendar programmatically to a specified logical view and date.
        /// </summary>
        /// <param name="view">
        /// Specifies the target logical view of the calendar. Use <see cref="CalendarView"/> enum values: <c>Month</c>, <c>Year</c>, or <c>Decade</c>.
        /// </param>
        /// <param name="date">
        /// The date to focus in the new view, of type <c>TValue</c> (typically <c>DateTime</c>).
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous navigation operation.
        /// </returns>
        /// <remarks>
        /// Use this method for scenarios that require calendar navigation at runtime, automating view changes for guided workflows or conditional behaviors based on user actions or events.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// await calendar.NavigateAsync(CalendarView.Year, DateTime.Now);
        /// ]]></code>
        /// </example>
        public async Task NavigateAsync(CalendarView view, TValue date)
        {
            if (date is null)
            {
                throw new ArgumentNullException(nameof(date), "Date parameter cannot be null");
            }
            if (!Enum.IsDefined(view))
            {
                throw new ArgumentOutOfRangeException(nameof(view), "Invalid calendar view specified");
            }
            if (CalendarBase is null)
            {
                return;
            }
            await CalendarBase.NavigateToAsync(view, date).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds one or more dates to the <see cref="Values"/> selection when the calendar is in multi-selection mode.
        /// </summary>
        /// <param name="dates">
        /// An array of <see cref="DateTime"/> objects to be added to the calendar's selection. If <c>null</c> or empty, the method does nothing.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous add operation.
        /// </returns>
        /// <remarks>
        /// This method updates the selected dates only when <see cref="IsMultiSelection"/> is set to <c>true</c>.
        /// It does not allow duplicates in the selection. If a date is already selected, it will not be added again.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// await calendar.AddDatesAsync(new DateTime[] { new DateTime(2022,2,12), new DateTime(2022,2,14) });
        /// ]]></code>
        /// </example>
        public async Task AddDatesAsync(DateTime[]? dates = null)
        {
            if (!IsMultiSelection || dates is null || dates.Length == 0)
            {
                return;
            }
            List<DateTime> updatedValues = BuildUpdatedDateList(dates);
            await ApplyMultiSelectionChangesAsync(updatedValues).ConfigureAwait(false);
        }

        /// <exclude />
        private List<DateTime> BuildUpdatedDateList(DateTime[] dates)
        {
            List<DateTime> copyValues = [.. CopyValues(Values)];
            foreach (DateTime date in dates)
            {
                if (!CheckPresentDate(date, [.. copyValues]))
                {
                    copyValues.Add(date);
                }
            }
            return copyValues;
        }

        /// <exclude />
        private async Task ApplyMultiSelectionChangesAsync(List<DateTime> updatedValues)
        {
            NotifyPropertyChanges(nameof(Value), GenericValue(updatedValues.LastOrDefault()), CalendarBase_Value);
            await UpdateDateValuesAsync([.. updatedValues]).ConfigureAwait(false);
        }

        /// <exclude />
        private async Task UpdateDateValuesAsync(DateTime[] copyDateValue)
        {
            IsMultipleDatesProgrammaticallySet = true;
            await UpdateCalendarPropertyAsync(nameof(Values), copyDateValue).ConfigureAwait(false);
            if (copyDateValue.Length > 0)
            {
                await UpdateCalendarPropertyAsync(nameof(Value), GenericValue(copyDateValue[^1])).ConfigureAwait(false);
            }
            TValue? tempValue = Value is null ? default! : (TValue)SfBaseUtils.ChangeType(Value, typeof(TValue));
            ChangedArgs = new ChangedEventArgs<TValue> { Value = tempValue!, Values = Values };
            await ChangeHandlerAsync(null, Values, IsMultiSelection).ConfigureAwait(false);
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes one or more dates from the <see cref="Values"/> selection when the calendar is in multi-selection mode.
        /// </summary>
        /// <param name="dates">
        /// An array of <see cref="DateTime"/> objects specifying which dates to remove from the current selection.
        /// If <c>null</c> or empty, no dates are removed.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous remove operation.
        /// </returns>
        /// <remarks>
        /// This method updates the selected dates only if <see cref="IsMultiSelection"/> is <c>true</c> and selected <see cref="Values"/> are not empty.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// await calendar.RemoveDatesAsync(new DateTime[] { new DateTime(2022,2,12) });
        /// ]]></code>
        /// </example>
        public async Task RemoveDatesAsync(DateTime[]? dates = null)
        {
            if (!CanRemoveDates(dates))
            {
                return;
            }
            List<DateTime> updatedValues = RemoveDatesFromList(dates!);
            await UpdateDateValuesAsync([.. updatedValues]).ConfigureAwait(false);
        }

        /// <exclude />
        private bool CanRemoveDates(DateTime[]? dates)
        {
            return IsMultiSelection && Values is not null && Values.Length > 0 && dates is not null && dates.Length > 0;
        }

        /// <exclude />
        private List<DateTime> RemoveDatesFromList(DateTime[] dates)
        {
            List<DateTime> copyValues = [.. CopyValues(Values)];
            HashSet<DateTime> datesToRemove = [.. dates.Select(d => d.Date)];
            return [.. copyValues.Where(cv => !datesToRemove.Contains(cv.Date))];
        }
    }
}
