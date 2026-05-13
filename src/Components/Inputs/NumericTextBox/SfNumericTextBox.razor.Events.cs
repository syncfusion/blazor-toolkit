using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public partial class SfNumericTextBox<TValue>
    {

        /// <summary>
        /// Gets or sets an event callback that is invoked when the value of the <see cref="SfNumericTextBox{TValue}"/> component changes.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback"/> that receives a <see cref="ChangeEventArgs{TValue}"/> containing the previous and current values when the component's value changes.
        /// </value>
        /// <remarks>
        /// The ValueChange event is triggered whenever the user modifies the numeric value in the TextBox, either by typing, using the spin buttons, or through programmatic changes.
        /// This event provides both the previous and current values, allowing you to track changes and implement custom logic based on value modifications.
        /// </remarks>
        /// <example>
        /// Example of handling the ValueChange event:
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int?" Placeholder="Enter the value" ValueChange="@OnValueChange">
        /// </SfNumericTextBox>
        ///
        /// @code {
        ///     private void OnValueChange(ChangeEventArgs<int?> args)
        ///     {
        ///         var previousValue = args.PreviousValue;
        ///         var currentValue = args.Value;
        ///         Console.WriteLine($"Value changed from {previousValue} to {currentValue}");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<ChangeEventArgs<TValue>> ValueChange { get; set; }


        /// <summary>
        /// Gets or sets an event callback that is invoked when the <see cref="SfNumericTextBox{TValue}"/> component receives focus.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback"/> that receives a <see cref="NumericFocusEventArgs{TValue}"/> containing information about the focus event.
        /// </value>
        /// <remarks>
        /// The Focus event is triggered when the NumericTextBox receives focus, typically when the user clicks on the component or navigates to it using the Tab key.
        /// You can use this event to highlight the component, show additional UI elements, or perform initialization tasks that should occur when the component becomes active.
        /// </remarks>
        /// <example>
        /// Example of handling the Focus event:
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="float?" OnFocus="@OnFocus">
        /// </SfNumericTextBox>
        ///
        /// @code {
        ///     private void OnFocus(NumericFocusEventArgs<float?> args)
        ///     {
        ///         // Handle focus event
        ///         Console.WriteLine($"NumericTextBox received focus. Current value: {args.Value}");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<NumericFocusEventArgs<TValue>> OnFocus { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is invoked when the <see cref="SfNumericTextBox{TValue}"/> component loses focus.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback"/> that receives a <see cref="NumericBlurEventArgs{TValue}"/> containing information about the blur event.
        /// </value>
        /// <remarks>
        /// The Blur event is triggered when the NumericTextBox loses focus, typically when the user clicks outside the component or navigates to another control using the Tab key.
        /// You can use this event to perform validation, save data, or update the UI based on the component losing focus.
        /// </remarks>
        /// <example>
        /// Example of handling the Blur event:
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int?" OnBlur="@OnBlur">
        /// </SfNumericTextBox>
        ///
        /// @code {
        ///     private void OnBlur(NumericBlurEventArgs<int?> args)
        ///     {
        ///         // Perform validation or other actions when component loses focus
        ///         Console.WriteLine($"NumericTextBox lost focus. Current value: {args.Value}");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<NumericBlurEventArgs<TValue>> OnBlur { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is invoked when the user types or modifies the value in the <see cref="SfNumericTextBox{TValue}"/> component.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback"/> that receives a <see cref="ChangeEventArgs"/> containing the previous and current values during real-time input.
        /// </value>
        /// <remarks>
        /// The OnInput event is triggered whenever the user modifies the numeric value in the TextBox, either by typing, using the spin buttons, or through programmatic changes.
        /// This event provides both the previous and current values, allowing you to track changes and implement custom logic based on value modifications.
        /// </remarks>
        /// <example>
        /// Example of handling the OnInput event:
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="int?" Placeholder="Enter the value" OnInput="@OnInput">
        /// </SfNumericTextBox>
        ///
        /// @code {
        ///     private void OnInput(ChangeEventArgs args)
        ///     {
        ///         Console.WriteLine("Input value changed");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is invoked when the <see cref="SfNumericTextBox{TValue}"/> component is created and fully initialized.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback"/> that receives an <see cref="object"/> parameter when the component creation is complete.
        /// </value>
        /// <remarks>
        /// The Created event is triggered after the NumericTextBox component has been fully rendered and initialized on the client-side.
        /// This event is useful for performing initialization tasks, setting up additional configurations, or integrating with third-party libraries that require the component to be fully rendered.
        /// </remarks>
        /// <example>
        /// Example of handling the Created event:
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="decimal?" Created="@OnCreated">
        /// </SfNumericTextBox>
        /// 
        /// @code {
        ///     private void OnCreated(object args)
        ///     {
        ///         // Component is fully created and rendered
        ///         Console.WriteLine("NumericTextBox component has been created successfully");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is invoked when the <see cref="SfNumericTextBox{TValue}"/> component is destroyed or disposed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback"/> that receives an <see cref="object"/> parameter when the component destruction occurs.
        /// </value>
        /// <remarks>
        /// The Destroyed event is triggered when the NumericTextBox component is being removed from the DOM or disposed of.
        /// This event is useful for performing cleanup tasks, removing event listeners, or disposing of resources that were allocated during the component's lifecycle.
        /// </remarks>
        /// <example>
        /// Example of handling the Destroyed event:
        /// <code><![CDATA[
        /// <SfNumericTextBox TValue="double?" Destroyed="@OnDestroyed" >
        /// </SfNumericTextBox>
        /// 
        /// @code {
        ///     private void OnDestroyed(object args)
        ///     {
        ///         // Perform cleanup when component is destroyed
        ///         Console.WriteLine("NumericTextBox component has been destroyed");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

    }
}
