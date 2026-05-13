using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// Configures the event handlers for the <see cref="SfTimePicker{TValue}"/> component, allowing you to respond to various user interactions and component lifecycle events.
    /// </summary>
    /// <remarks>
    /// This partial class defines the event callback methods for events such as value changes, focus, blur, and popup interactions.
    /// </remarks>
    public partial class SfTimePicker<TValue>
    {
        /// <summary>
        /// Gets or sets an event callback that is triggered when the component loses focus.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the component loses focus. The callback receives a <see cref="BlurEventArgs"/>.
        /// </value>
        /// <remarks>
        /// This event can be used to perform actions when the user navigates away from the TimePicker component.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" OnBlur="@OnBlur"></SfTimePicker>
        /// @code{
        /// private void OnBlur(BlurEventArgs args)
        /// {
        /// // Your logic here
        /// }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<BlurEventArgs> OnBlur { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is triggered when the value of the component is changed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the component's value changes. The callback receives a <see cref="ChangeEventArgs{TValue}"/>.
        /// </value>
        /// <remarks>
        /// This event is fired when the user selects a time from the popup, or when the value is changed programmatically.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" ValueChange="@ValueChange"></SfTimePicker>
        /// @code{
        ///    private void ValueChange(ChangeEventArgs<DateTime?> args) {
        ///         Console.WriteLine(args.Value);
        ///     }
        ///   }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<ChangeEventArgs<TValue>> ValueChange { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is invoked when the user inputs or modifies the value in the TimePicker.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback"/> that receives a <see cref="ChangeEventArgs"/> containing the input value information.
        /// </value>
        /// <remarks>
        /// This event is triggered on each user input as they type or modify the time value directly in the input field.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" OnInput="@OnInput">
        /// </SfTimePicker>
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
        /// Gets or sets an event callback that is triggered after a time value is selected from the popup.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when a value is selected. The callback receives a <see cref="SelectedEventArgs{TValue}"/>.
        /// </value>
        /// <remarks>
        /// This event provides the selected time value and can be used to perform actions immediately after a selection is made.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" Selected="@ValueSelected"></SfTimePicker>
        /// @code{
        ///    private void ValueSelected(SelectedEventArgs<DateTime?> args) {
        ///         Console.WriteLine(args.Value);
        ///     }
        ///   }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<SelectedEventArgs<TValue>> Selected { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is triggered when the component's value is cleared using the clear button.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the clear button is clicked. The callback receives a <see cref="ClearedEventArgs"/>.
        /// </value>
        /// <remarks>
        /// This event is only applicable if the <see cref="SfTimePicker{TValue}.ShowClearButton"/> property is set to <c>true</c>.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" ShowClearButton="true" Cleared="@OnCleared"></SfTimePicker>
        /// @code{
        /// private void OnCleared(ClearedEventArgs args)
        /// {
        /// // Your logic here
        /// }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<ClearedEventArgs> Cleared { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is triggered when the time suggestion popup is closed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the popup closes. The callback receives a <see cref="PopupEventArgs"/>.
        /// </value>
        /// <remarks>
        /// You can prevent the popup from closing by setting the <see cref="PopupEventArgs.Cancel"/> property to <c>true</c> within the event handler.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" OnClose="@PopupClose"></SfTimePicker>
        /// @code{
        ///    private void PopupClose(PopupEventArgs args) {
        ///        args.Cancel = true;
        ///     }
        ///   }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<PopupEventArgs> OnClose { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is triggered when the component is created.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the component is first rendered.
        /// </value>
        /// <remarks>
        /// This event is useful for performing one-time initialization actions after the component has been rendered.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfTimePicker TValue="DateTime?" Created="@OnCreated"></SfTimePicker>
        /// @code {
        ///     private void OnCreated(object args)
        ///     {
        ///         // Your logic here
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is triggered when the component is destroyed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the component is being removed from the DOM.
        /// </value>
        /// <remarks>
        /// This event is useful for performing cleanup operations before the component is completely removed.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" Destroyed="@OnDestroyed"></SfTimePicker>
        /// @code{
        /// private void OnDestroyed(object args)
        /// {
        /// // Your logic here
        /// }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is triggered when the component gains focus.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the component receives focus. The callback receives a <see cref="FocusEventArgs"/>.
        /// </value>
        /// <remarks>
        /// This event is useful for performing actions when the user starts interacting with the TimePicker.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" OnFocus="@OnFocus"></SfTimePicker>
        /// @code{
        /// private void OnFocus(Syncfusion.Blazor.Toolkit.Calendars.FocusEventArgs args)
        /// {
        /// // Your logic here
        /// }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<FocusEventArgs> OnFocus { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is triggered while rendering each item in the time suggestion popup.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked for each list item. The callback receives an <see cref="ItemEventArgs{TValue}"/>.
        /// </value>
        /// <remarks>
        /// This event allows for the customization of each time item, such as adding custom attributes or disabling specific times.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" OnItemRender="@ItemRender"></SfTimePicker>
        /// @code{
        ///    private void ItemRender(ItemEventArgs<DateTime?> args) {
        ///        Console.WriteLine(args.Text);
        ///     }
        ///   }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<ItemEventArgs<TValue>> OnItemRender { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is triggered when the time suggestion popup is opened.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the popup opens. The callback receives a <see cref="PopupEventArgs"/>.
        /// </value>
        /// <remarks>
        /// You can prevent the popup from opening by setting the <see cref="PopupEventArgs.Cancel"/> property to <c>true</c> within the event handler.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTimePicker TValue="DateTime?" OnOpen="@PopupOpen"></SfTimePicker>
        /// @code{
        ///    private void PopupOpen(PopupEventArgs args) {
        ///        args.Cancel = true;
        ///     }
        ///   }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<PopupEventArgs> OnOpen { get; set; }
    }
}
