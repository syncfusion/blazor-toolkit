using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents a TextBox component that provides an input element for accepting text input from users.
    /// </summary>
    /// <remarks>
    /// <para>The <see cref="SfTextBox"/> component allows users to edit or display text values with support for various input types, validation, floating labels, and customization options.</para>
    /// <para>The component provides a rich set of features including real-time input events, focus management, and accessibility support.</para>
    /// </remarks>
    public partial class SfTextBox : SfInputBase<string>
    {
        #region Events

        /// <summary>
        /// Gets or sets the event callback that is invoked when the <see cref="SfTextBox"/> loses focus.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the TextBox loses focus. The callback receives a <see cref="FocusOutEventArgs"/> containing event data.
        /// </value>
        /// <remarks>
        /// <para>The <see cref="OnBlur"/> event is triggered when the user clicks outside the TextBox or navigates away from it using keyboard navigation (such as pressing the Tab key).</para>
        /// <para>This event is useful for performing validation, saving changes, or updating the UI when the user finishes interacting with the TextBox.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox Placeholder="Enter your name" OnBlur="@OnBlur">
        /// </SfTextBox>
        /// @code{
        ///     private void OnBlur(FocusOutEventArgs args)
        ///     {
        ///         // Perform validation or other actions when TextBox loses focus
        ///         Console.WriteLine("TextBox lost focus");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<FocusOutEventArgs> OnBlur { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when the <see cref="SfTextBox"/> content has changed and the component loses focus.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the TextBox content changes and loses focus. The callback receives a <see cref="ChangedEventArgs"/> containing the previous and current values.
        /// </value>
        /// <remarks>
        /// <para>The <see cref="ValueChange"/> event is triggered only when the TextBox loses focus and the value has actually changed from its previous state.</para>
        /// <para>This differs from the <see cref="OnInput"/> event, which fires on every keystroke during real-time input. Use <see cref="ValueChange"/> for final value processing, validation, or when comparing the old and new values is required.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox Placeholder="Enter a value" ValueChange="@OnChange">
        /// </SfTextBox>
        /// @code{
        ///     private void OnChange(ChangedEventArgs args)
        ///     {
        ///         var TextValue = args.Value;
        ///         var PreviousValue = args.PreviousValue;
        ///         // Process the changed value
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<ChangedEventArgs> ValueChange { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when the <see cref="SfTextBox"/> component is created.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the TextBox component is successfully created and rendered. The callback receives an <see cref="object"/> parameter.
        /// </value>
        /// <remarks>
        /// <para>The <see cref="Created"/> event is triggered after the TextBox component has been fully initialized and rendered in the DOM.</para>
        /// <para>This event is useful for performing initialization tasks, setting up additional configurations, or integrating with JavaScript interop that requires the component to be fully rendered.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox Placeholder="Enter text" Created="@OnCreated">
        /// </SfTextBox>
        /// @code{
        ///     private void OnCreated(object args)
        ///     {
        ///         // Component initialization logic
        ///         Console.WriteLine("TextBox component created successfully");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when the <see cref="SfTextBox"/> component is destroyed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the TextBox component is being destroyed or removed from the DOM. The callback receives an <see cref="object"/> parameter.
        /// </value>
        /// <remarks>
        /// <para>The <see cref="Destroyed"/> event is triggered when the TextBox component is being disposed or removed from the component tree.</para>
        /// <para>This event provides an opportunity to perform cleanup operations, such as removing event listeners, clearing timers, canceling pending operations, or releasing resources that were allocated during the component's lifecycle.</para>
        /// <para>This event occurs only once, at the end of the component's lifecycle, just before it is removed from the DOM.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox Placeholder="Enter text" Destroyed="@OnDestroyed">
        /// </SfTextBox>
        /// @code{
        ///     private void OnDestroyed(object args)
        ///     {
        ///         // Cleanup logic
        ///         Console.WriteLine("TextBox component destroyed");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when the <see cref="SfTextBox"/> receives focus.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the TextBox receives focus. The callback receives a <see cref="FocusInEventArgs"/> containing event data.
        /// </value>
        /// <remarks>
        /// <para>The <see cref="OnFocus"/> event is triggered when the user clicks on the TextBox, navigates to it using keyboard navigation (such as the Tab key), or when focus is programmatically set to the component.</para>
        /// <para>This event is useful for highlighting the TextBox, showing additional UI elements such as tooltips or help text, selecting all text, or preparing the component for user input.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox Placeholder="Enter your email" OnFocus="@OnFocus">
        /// </SfTextBox>
        /// @code{
        ///     private void OnFocus(FocusInEventArgs args)
        ///     {
        ///         // Perform actions when TextBox gains focus
        ///         Console.WriteLine("TextBox gained focus");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<FocusInEventArgs> OnFocus { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when the user types, pastes, or modifies text in the <see cref="SfTextBox"/>.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> of <see cref="InputEventArgs"/> that is invoked during real-time text input.
        /// </value>
        /// <remarks>
        /// <para>The <see cref="OnInput"/> event is triggered whenever the user interacts with the TextBox by typing, pasting, cutting, or using any input method to modify the content. It provides real-time updates as the user enters text, allowing actions or validation based on the changing input.</para>
        /// <para>This event fires frequently during user input, potentially with each keystroke, making it suitable for real-time updates, character counting, or dynamic filtering. For performance-sensitive operations, consider using the <see cref="ValueChange"/> event instead, which fires only when the TextBox loses focus.</para>
        /// <para>The <see cref="InputEventArgs"/> parameter provides access to both the <see cref="InputEventArgs.Value"/> and <see cref="InputEventArgs.PreviousValue"/> properties, enabling incremental change tracking.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextBox Placeholder="Enter a value" OnInput="@OnInput">
        /// </SfTextBox>
        /// @code{
        ///     private void OnInput(InputEventArgs args)
        ///     {
        ///         var TextValue = args.Value;
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<InputEventArgs> OnInput { get; set; }

        #endregion
    }
}