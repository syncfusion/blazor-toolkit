using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// Represents the event callbacks for the <see cref="SfDateTimePicker{TValue}"/> component, providing handlers for various user interactions and component lifecycle events.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of the DateTimePicker value, typically <see cref="DateTime"/> or nullable DateTime.</typeparam>
    /// <remarks>
    /// This class contains event callback properties that allow you to handle various events raised by the DateTimePicker component.
    /// These events include value changes, focus/blur events, popup open/close events, and calendar navigation events.
    /// Use these events to implement custom logic in response to user interactions with the DateTimePicker component.
    /// </remarks>
    /// <example>
    /// Basic usage of DateTimePicker events:
    /// <code><![CDATA[
    /// <SfDateTimePicker TValue="DateTime?"
    ///                   ValueChange="@OnValueChange"
    ///                   OnOpen="@OnPopupOpen"
    ///                   OnFocus="@OnFocus" />
    ///
    /// @code {
    ///     private void OnValueChange(ChangedEventArgs<DateTime?> args)
    ///     {
    ///         Console.WriteLine($"Selected date: {args.Value}");
    ///     }
    ///
    ///     private void OnPopupOpen(PopupObjectArgs args)
    ///     {
    ///         Console.WriteLine("Popup opened");
    ///     }
    ///
    ///     private void OnFocus(FocusEventArgs args)
    ///     {
    ///         Console.WriteLine("DateTimePicker focused");
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public partial class SfDateTimePicker<TValue>
    {
        /// <summary>
        /// Gets or sets an event callback that is triggered when a key is pressed down while the input element has focus.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{KeyboardEventArgs}"/> that is invoked when a key down event occurs on the input element.
        /// </value>
        /// <remarks>
        /// This event allows you to handle keyboard interactions such as navigation keys, Enter key for selection, 
        /// and Escape key for closing popups. The event is particularly useful for implementing custom keyboard shortcuts
        /// and navigation behavior in the DateTimePicker component.
        /// </remarks>
        /// <example>
        /// Handling the OnKeyDown event:
        /// <code><![CDATA[
        /// <SfDateTimePicker TValue="DateTime" OnKeyDown="HandleKeyDown">
        /// </SfDateTimePicker>
        /// 
        /// private async Task HandleKeyDown(KeyboardEventArgs args)
        /// {
        ///     if (args.Key == "F2")
        ///     {
        ///         // Custom functionality
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

        /// <summary>
        /// Gets or sets the event callback that will be invoked while rendering each item in the DateTimePicker popup list.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that receives an <see cref="ItemEventArgs{TValue}"/> for each item being rendered in the popup.
        /// </value>
        /// <remarks>
        /// This event is triggered for each item (such as time slots or quick access options) that is rendered in the DateTimePicker popup.
        /// Use this event to customize the appearance, content, or behavior of individual items in the popup list.
        /// You can modify item properties, add custom CSS classes, or conditionally show/hide items based on your requirements.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfDateTimePicker TValue="DateTime?" OnItemRender="@OnItemRender" />
        /// 
        /// @code {
        ///     private void OnItemRender(ItemEventArgs<DateTime?> args)
        ///     {
        ///         // Customize item rendering
        ///         if (args.Item is not null)
        ///         {
        ///             args.Item.CssClass = "custom-item-style";
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public EventCallback<ItemEventArgs<TValue>> OnItemRender { get; set; }
    }
}
