using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    public partial class SfCalendar<TValue>
    {
        /// <summary>
        /// Gets or sets an event callback that is invoked when the <see cref="SfCalendar{TValue}.Values"/> property changes.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is triggered when the calendar values are changed. The callback receives an instance of <see cref="ChangedEventArgs{TValue}"/> containing the new value and other details.
        /// </value>
        /// <remarks>
        /// Use this event to respond to changes in the selected dates when interacting with the calendar, such as via UI input or programmatic assignment.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCalendar TValue="DateTime?" ValueChange="@ValueChange">
        /// </SfCalendar>
        /// @code {
        ///     private void ValueChange(ChangedEventArgs<DateTime?> args) {
        ///         Console.WriteLine($"Changed value: {args.Value}");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<ChangedEventArgs<TValue>> ValueChange { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is invoked after one or more date values are selected in the calendar.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{SelectedEventArgs}"/> that is invoked when dates are selected. The callback receives a <see cref="SelectedEventArgs{TValue}"/> instance containing the information about the selected value(s).
        /// </value>
        /// <remarks>
        /// This event is raised after a date or multiple dates are selected by the user via the calendar UI or through code.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCalendar TValue="DateTime?" Selected="@ValueSelected">
        /// </SfCalendar>
        /// @code {
        ///     private void ValueSelected(SelectedEventArgs<DateTime?> args) {
        ///         Console.WriteLine($"Selected value: {args.Value}");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<SelectedEventArgs<TValue>> Selected { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is invoked after deselecting a value in the calendar.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{DeSelectedEventArgs}"/> that is invoked when a value is deselected. The callback receives a <see cref="DeSelectedEventArgs{TValue}"/> instance providing details of the deselected date.
        /// </value>
        /// <remarks>
        /// This event is only triggered when the <see cref="SfCalendar{TValue}.IsMultiSelection"/> property is enabled, allowing multiple date selection and deselection in the calendar.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCalendar TValue="DateTime?" IsMultiSelection="true" DeSelected="@ValueDeselected">
        /// </SfCalendar>
        /// @code {
        ///     private void ValueDeselected(DeSelectedEventArgs<DateTime?> args) {
        ///         Console.WriteLine($"Deselected value: {args.Value}");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<DeSelectedEventArgs<TValue>> DeSelected { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is invoked when the calendar component is created and initialized.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> of type <c>object</c> that is triggered after the component is created and fully initialized. The callback receives an <c>object</c> containing creation details.
        /// </value>
        /// <remarks>
        /// This event is useful for executing code after the calendar's resources have been fully allocated and rendered.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCalendar TValue="DateTime?" Created="@OnCreated">
        /// </SfCalendar>
        /// @code {
        ///     private void OnCreated(object args) {
        ///         Console.WriteLine("Calendar created.");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is invoked when the calendar component is disposed and destroyed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> of type <c>object</c> that is triggered during component disposal. The callback receives an <c>object</c> providing destruction context.
        /// </value>
        /// <remarks>
        /// Use this event to perform cleanup or release resources associated with the calendar when it is destroyed.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCalendar TValue="DateTime?" Destroyed="@OnDestroyed">
        /// </SfCalendar>
        /// @code {
        ///     private void OnDestroyed(object args) {
        ///         Console.WriteLine("Calendar destroyed.");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is invoked after the calendar is navigated to another level or within the same level of the calendar view.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{NavigatedEventArgs}"/> that is triggered after navigation in the calendar. The callback receives a <see cref="NavigatedEventArgs"/> instance describing the navigation context.
        /// </value>
        /// <remarks>
        /// Use this event to track and respond to changes in the calendar's current view, such as when switching between months, years, or decades.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCalendar TValue="DateTime?" Navigated="@ViewNavigated">
        /// </SfCalendar>
        /// @code {
        ///     private void ViewNavigated(NavigatedEventArgs args) {
        ///         Console.WriteLine($"Current view: {args.View}");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<NavigatedEventArgs> Navigated { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is invoked as each calendar day cell is rendered.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{RenderDayCellEventArgs}"/> that is triggered when a day cell is rendered in the calendar. The callback receives a <see cref="RenderDayCellEventArgs"/> providing details for customizing the cell appearance.
        /// </value>
        /// <remarks>
        /// This event allows customizing the visual appearance or properties of individual day cells as they are rendered in the calendar UI.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCalendar TValue="DateTime?" DayCellRendering="@CellRendered">
        /// </SfCalendar>
        /// @code {
        ///     private void CellRendered(RenderDayCellEventArgs args) {
        ///         args.CellData.ClassList = "e-custom-style";
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<RenderDayCellEventArgs> DayCellRendering { get; set; }
    }
}
