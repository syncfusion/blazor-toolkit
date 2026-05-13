using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    public partial class SfDatePicker<TValue>
    {
        /// <summary>
        /// Gets or sets the event callback that is invoked when the component loses focus.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{BlurEventArgs}"/> triggered when the input loses focus.
        /// </value>
        /// <remarks>
        /// Use this event to perform custom logic when the input field loses focus, such as finalizing data entry or validation.
        /// </remarks>
        [Parameter]
        public EventCallback<BlurEventArgs> OnBlur { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when the component value is changed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{T}"/> of type <c>ChangedEventArgs&lt;TValue&gt;</c> triggered whenever the DatePicker value changes.
        /// </value>
        /// <remarks>
        /// Use this event to respond to changes to the input value, either from user interaction or programmatic changes. The event provides the new value in the event arguments.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" ValueChange="@ValueChange">
        /// </SfDatePicker>
        /// @code{
        ///    private void ValueChange(ChangedEventArgs<DateTime?> args) {
        ///         Console.WriteLine(args.Value);
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<ChangedEventArgs<TValue>> ValueChange { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is invoked when the user inputs or modifies the value in the DatePicker.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback"/> that receives a <see cref="ChangeEventArgs"/> containing the input value information.
        /// </value>
        /// <remarks>
        /// This event is triggered on each user input as they type or modify the date value directly in the input field.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" OnInput="@OnInput">
        /// </SfDatePicker>
        /// @code{
        ///    private void OnInput(ChangeEventArgs args) {
        ///         Console.WriteLine("Input value changed");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked after selecting the value from the component.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{SelectedEventArgs}"/> of type <c>SelectedEventArgs&lt;TValue&gt;</c> that fires after a value is selected from the Calendar popup.
        /// </value>
        /// <remarks>
        /// This event occurs after the user selects a value from the Calendar, but before the value is set. Use this event for custom actions such as formatting or validation prior to assignment.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" Selected="@ValueSelected">
        /// </SfDatePicker>
        /// @code{
        ///    private void ValueSelected(SelectedEventArgs<DateTime?> args) {
        ///         Console.WriteLine(args.Value);
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<SelectedEventArgs<TValue>> Selected { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when the component value is cleared using the clear button.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{ClearedEventArgs}"/> triggered after the value is cleared by the user.
        /// </value>
        /// <remarks>
        /// Use this event to handle actions, such as resetting dependent fields or updating state, after the DatePicker value is cleared.
        /// </remarks>
        [Parameter]
        public EventCallback<ClearedEventArgs> Cleared { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when the popup is closed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{PopupObjectArgs}"/> triggered when the popup is closed, either by user action or programmatically.
        /// </value>
        /// <remarks>
        /// Use this event to execute custom logic after the popup closes. You can cancel closing through the event argument.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" OnClose="@PopupClose">
        /// </SfDatePicker>
        /// @code{
        ///    private void PopupClose(PopupObjectArgs args) {
        ///         args.Cancel = true;
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<PopupObjectArgs> OnClose { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when the component is created.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback"/> triggered once the DatePicker component is created and ready for use.
        /// </value>
        /// <remarks>
        /// Use this event to perform initialization or startup logic for the DatePicker.
        /// </remarks>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when the component is destroyed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback"/> triggered just before the DatePicker component is removed from the UI.
        /// </value>
        /// <remarks>
        /// Clean up resources or perform any necessary teardown logic inside this event.
        /// </remarks>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when the component receives focus.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{FocusEventArgs}"/> triggered when the component input receives keyboard or mouse focus.
        /// </value>
        /// <remarks>
        /// Use this event to respond to focus changes or update UI highlights as required.
        /// </remarks>
        [Parameter]
        public EventCallback<FocusEventArgs> OnFocus { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when the Calendar is navigated to another level or within the same view.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{NavigatedEventArgs}"/> triggered each time the view changes between year, month, or decade.
        /// </value>
        /// <remarks>
        /// Use this event to capture or act on navigation between the Calendar views, such as customizing data for a specific view.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" Navigated="@ViewNavigated">
        /// </SfDatePicker>
        /// @code{
        ///    private void ViewNavigated(NavigatedEventArgs args) {
        ///         Console.WriteLine(args.View);
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<NavigatedEventArgs> Navigated { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when the popup is opened.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{PopupObjectArgs}"/> triggered when the picker popup is opened.
        /// </value>
        /// <remarks>
        /// Use this event to customize the popup prior to it being displayed, such as modifying content or cancelling the open action.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" OnOpen="@PopupOpen">
        /// </SfDatePicker>
        /// @code{
        ///    private void PopupOpen(PopupObjectArgs args) {
        ///         args.Cancel = true;
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<PopupObjectArgs> OnOpen { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when each day cell of the Calendar is rendered.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{RenderDayCellEventArgs}"/> triggered while rendering each day cell in the Calendar Popup.
        /// </value>
        /// <remarks>
        /// Use this event to customize the appearance and content of individual day cells in the Calendar.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDatePicker TValue="DateTime?" DayCellRendering="CellRendered">
        /// </SfDatePicker>
        /// @code{
        ///    private void CellRendered(RenderDayCellEventArgs args) {
        ///         args.CellData.ClassList = "e-custom-style";
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<RenderDayCellEventArgs> DayCellRendering { get; set; }
    }
}

