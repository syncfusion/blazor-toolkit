using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Spinner
{
    /// <summary>
    /// Configures the event handlers for the <see cref="SfSpinner"/> component.
    /// </summary>
    /// <remarks>
    /// The <see cref="SfSpinner"/> component allows you to handle various events triggered during the component's lifecycle,
    /// such as creation, opening, and closing. This partial class defines all event-related properties and callbacks.
    /// </remarks>
    /// <example>
    /// A simple example of using <see cref="SfSpinner"/>.
    /// <code><![CDATA[
    /// <SfSpinner Created="@HandleCreatedAsync" 
    ///                    OnOpen="@HandleOpenAsync" 
    ///                    OnClose="@HandleCloseAsync" 
    ///                    Destroyed="@HandleDestroyedAsync" >
    /// </SfSpinner>
    ///
    /// @code {
    ///     private void HandleCreatedAsync(object args)
    ///     {
    ///         // Handle the created event
    ///     }
    ///
    ///     private void HandleOpenAsync(SpinnerEventArgs args)
    ///     {
    ///         // Handle the before open event
    ///     }
    ///
    ///     private void HandleCloseAsync(SpinnerEventArgs args)
    ///     {
    ///         // Handle the before close event
    ///     }
    ///
    ///     private void HandleDestroyedAsync(object args)
    ///     {
    ///         // Handle the destroyed event
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public partial class SfSpinner : SfBaseComponent
    {
        #region Properties
        /// <summary>
        /// Gets or sets the event callback that is invoked after the <see cref="SfSpinner"/> is created and rendered.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> of type <see cref="object"/>. The default value is an empty callback.
        /// </value>
        /// <remarks>
        /// This event can be used to perform initialization actions once the spinner is fully rendered.
        /// The event is invoked only on the first render cycle of the component.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfSpinner Created="@OnSpinnerCreated" />
        ///
        /// @code {
        ///     private async Task OnSpinnerCreated(object args)
        ///     {
        ///         // Perform initialization actions
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked before the <see cref="SfSpinner"/> is shown.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked before the spinner is displayed. The callback receives a <see cref="SfSpinner"/>.
        /// </value>
        /// <remarks>
        /// This event allows you to perform custom logic before the spinner becomes visible.
        /// You can cancel the show operation by setting <see cref="SpinnerEventArgs.Cancel"/> to <see langword="true"/>.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfSpinner OnOpen="@OnSpinnerOpen" />
        ///
        /// @code {
        ///     private async Task OnSpinnerOpen(SpinnerEventArgs args)
        ///     {
        ///         // Perform validation or setup before showing
        ///         if (someCondition)
        ///         {
        ///             args.Cancel = true; // Prevent showing
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<SpinnerEventArgs> OnOpen { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked before the <see cref="SfSpinner"/> is hidden.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked before the spinner is removed from view. The callback receives a <see cref="SpinnerEventArgs"/>.
        /// </value>
        /// <remarks>
        /// This event allows you to execute custom logic before the spinner is hidden.
        /// You can cancel the hide operation by setting <see cref="SpinnerEventArgs.Cancel"/> to <see langword="true"/>.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfSpinner OnClose="@OnSpinnerClose" />
        ///
        /// @code {
        ///     private async Task OnSpinnerClose(SpinnerEventArgs args)
        ///     {
        ///         // Perform cleanup or validation before hiding
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<SpinnerEventArgs> OnClose { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked after the <see cref="SfSpinner"/> is destroyed and removed from the DOM.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> of type <see cref="object"/>. The default value is an empty callback.
        /// </value>
        /// <remarks>
        /// This event can be used to perform cleanup actions after the spinner has been completely removed from the DOM.
        /// This is useful for releasing resources or updating application state.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfSpinner Destroyed="@OnSpinnerDestroyed" />
        ///
        /// @code {
        ///     private async Task OnSpinnerDestroyed(object args)
        ///     {
        ///         // Perform cleanup actions
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents the event arguments for the <see cref="SfSpinner.OnOpen"/> and <see cref="SfSpinner.OnClose"/> events of the <see cref="SfSpinner"/> component.
    /// </summary>
    /// <remarks>
    /// These event arguments enable you to conditionally prevent the spinner from being shown or hidden by setting the <see cref="Cancel"/> property to <see langword="true"/>.
    /// This allows for custom logic to control the spinner's visibility based on application state or other conditions.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to use <see cref="SpinnerEventArgs"/> in an event handler to cancel the spinner's visibility change.
    /// <code><![CDATA[
    /// <SfSpinner @bind-Visible="@IsVisible" OnOpen="HandleBeforeOpenAsync">
    /// </SfSpinner>
    /// @code {
    ///     private bool IsVisible { get; set; } = true;
    ///
    ///     private void HandleBeforeOpenAsync(SpinnerEventArgs args)
    ///     {
    ///         // Cancel the spinner from being shown based on custom condition
    ///         args.Cancel = true;
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class SpinnerEventArgs
    {
        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether the spinner's visibility change should be canceled.
        /// </summary>
        /// <value>
        /// <see langword="true"/> to prevent the spinner's visibility change; otherwise, <see langword="false"/>. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// This property is evaluated by the component after the <see cref="SfSpinner.OnOpen"/> or <see cref="SfSpinner.OnClose"/> event is raised.
        /// Setting it to <see langword="true"/> stops the spinner from being shown or hidden.
        /// </remarks>
        public bool Cancel { get; set; }
        #endregion
    }
}