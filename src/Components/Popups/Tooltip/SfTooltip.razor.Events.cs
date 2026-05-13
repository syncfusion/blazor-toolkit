using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// The SfTooltip component displays a pop-up containing information or a message when you hover, click, focus, or touch an element.
    /// </summary>
    public partial class SfTooltip
    {
        #region Event Callbacks

        /// <summary>
        /// Gets or sets an event callback that is raised when the Tooltip component is closed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> of type <see cref="TooltipEventArgs"/> that is invoked when the Tooltip is closed.
        /// </value>
        /// <remarks>
        /// This event is triggered after the Tooltip has been completely closed and removed from view.
        /// It is useful for performing additional actions when the Tooltip is closed, such as updating the UI, performing cleanup tasks, or logging user interactions.
        /// The event callback function receives a <see cref="TooltipEventArgs"/> parameter, which provides detailed information about the Tooltip that was closed, including the target element and event details.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Closed="OnTooltipClosed">
        ///     <div>Hover over me</div>
        /// </SfTooltip>
        /// 
        /// @code {
        ///     private void OnTooltipClosed(TooltipEventArgs args)
        ///     {
        ///         // Perform cleanup or update UI
        ///         Console.WriteLine("Tooltip closed");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<TooltipEventArgs> Closed { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is raised when the Tooltip component is opened.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> of type <see cref="TooltipEventArgs"/> that is invoked when the Tooltip is opened.
        /// </value>
        /// <remarks>
        /// This event is triggered after the Tooltip has been successfully opened and is visible to the user.
        /// It is useful for performing additional actions when the Tooltip is opened, such as updating the UI, tracking user interactions, or initializing dynamic content.
        /// The event callback function receives a <see cref="TooltipEventArgs"/> parameter, which provides detailed information about the Tooltip that was opened, including the target element and event context.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Opened="OnTooltipOpened">
        ///     <div>Hover over me</div>
        /// </SfTooltip>
        /// 
        /// @code {
        ///     private void OnTooltipOpened(TooltipEventArgs args)
        ///     {
        ///         // Track user interaction or update UI
        ///         Console.WriteLine("Tooltip opened");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<TooltipEventArgs> Opened { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is raised before the Tooltip hides from the screen.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> of type <see cref="TooltipEventArgs"/> that is invoked before the Tooltip is hidden.
        /// </value>
        /// <remarks>
        /// This event is triggered before the Tooltip starts the hiding process, providing an opportunity to perform validation or cleanup operations.
        /// It is useful for performing additional actions before the Tooltip is closed, such as updating the UI, validating user input, or saving data.
        /// The event callback function receives a <see cref="TooltipEventArgs"/> parameter, which provides information about the Tooltip that is about to be closed.
        /// To prevent the Tooltip from closing, set the <c>Cancel</c> property of the <see cref="TooltipEventArgs"/> parameter to <c>true</c>.
        /// This cancellation capability makes this event ideal for implementing custom validation logic or user confirmation dialogs.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip OnClose="OnTooltipClose">
        ///     <div>Hover over me</div>
        /// </SfTooltip>
        /// 
        /// @code {
        ///     private void OnTooltipClose(TooltipEventArgs args)
        ///     {
        ///         // Validate or prevent closing
        ///         if (someCondition)
        ///         {
        ///             args.Cancel = true; // Prevent tooltip from closing
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<TooltipEventArgs> OnClose { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is raised for every collision fit calculation.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> of type <see cref="TooltipEventArgs"/> that is invoked during collision detection calculations.
        /// </value>
        /// <remarks>
        /// This event is triggered whenever the Tooltip component performs collision detection to determine the optimal positioning on the screen.
        /// It is useful for fine-tuning the placement of the Tooltip on the screen and avoiding overlaps with other UI elements or viewport boundaries.
        /// The event callback function receives a <see cref="TooltipEventArgs"/> parameter, which provides comprehensive information about the Tooltip and its placement calculations.
        /// The <see cref="TooltipEventArgs"/> parameter includes the target element and collision information object, which describe the Tooltip's current position, collision detection results, and suggested positioning adjustments.
        /// This event allows for custom collision handling logic and dynamic positioning strategies based on the current layout and available screen space.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Colliding="OnTooltipCollision">
        ///     <div>Hover over me</div>
        /// </SfTooltip>
        ///
        /// @code {
        ///     private void OnTooltipCollision(TooltipEventArgs args)
        ///     {
        ///         // Handle collision and adjust positioning
        ///         Console.WriteLine($"Collision detected at position: {args.CollisionInfo}");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<TooltipEventArgs> Colliding { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is raised before the Tooltip is displayed over the target element.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> of type <see cref="TooltipEventArgs"/> that is invoked before the Tooltip is displayed.
        /// </value>
        /// <remarks>
        /// This event is triggered before the Tooltip becomes visible, providing an opportunity to customize its appearance, content, or behavior.
        /// It is useful for dynamically modifying the Tooltip's appearance or behavior before it is displayed, such as setting custom content, applying conditional styling, or performing pre-display validation.
        /// The event callback function receives a <see cref="TooltipEventArgs"/> parameter, which provides comprehensive information about the Tooltip and its target element.
        /// The <see cref="TooltipEventArgs"/> parameter includes the target element and an optional <c>Cancel</c> property, which can be set to <c>true</c> to prevent the Tooltip from being displayed.
        /// This cancellation capability allows for conditional Tooltip display based on dynamic conditions or user permissions.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip OnOpen="OnTooltipOpen">
        ///     <div>Hover over me</div>
        /// </SfTooltip>
        /// 
        /// @code {
        ///     private void OnTooltipOpen(TooltipEventArgs args)
        ///     {
        ///         // Customize tooltip before display
        ///         if (!userHasPermission)
        ///         {
        ///             args.Cancel = true; // Prevent tooltip from showing
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<TooltipEventArgs> OnOpen { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is raised before the Tooltip and its contents will be added to the DOM.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> of type <see cref="TooltipEventArgs"/> that is invoked before the Tooltip is rendered to the DOM.
        /// </value>
        /// <remarks>
        /// This event is triggered before the Tooltip element and its contents are created and added to the DOM, providing the earliest opportunity for customization.
        /// When the <c>Cancel</c> property of the <see cref="TooltipEventArgs"/> parameter is set to <c>true</c>, the Tooltip can be prevented from rendering on the page.
        /// This event is primarily used to customize the Tooltip before it appears on the screen, such as loading dynamic content, applying custom styling, or setting animation effects.
        /// It is ideal for scenarios where you need to load AJAX content, apply conditional formatting, or perform any DOM manipulation before the Tooltip becomes visible.
        /// The event provides full control over the Tooltip's initial state and allows for dynamic content generation based on the target element or application state.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip OnRender="OnTooltipRender">
        ///     <div>Hover over me</div>
        /// </SfTooltip>
        /// 
        /// @code {
        ///     private void OnTooltipRender(TooltipEventArgs args)
        ///     {
        ///         // Load dynamic content or customize before rendering
        ///         if (needsAjaxContent)
        ///         {
        ///             // Load AJAX content
        ///             args.Content = GetDynamicContent();
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<TooltipEventArgs> OnRender { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is raised after the Tooltip component is created.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> of type <see cref="object"/> that is invoked after the Tooltip component is successfully created.
        /// </value>
        /// <remarks>
        /// This event is triggered after the Tooltip component has been fully initialized and is ready for use.
        /// It provides an opportunity to perform post-initialization tasks, such as setting up additional event handlers, applying custom configurations, or integrating with other components.
        /// The event callback receives a generic <see cref="object"/> parameter that contains component creation details.
        /// This is typically used for component lifecycle management and integration with external libraries or frameworks.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Created="OnTooltipCreated">
        ///     <div>Hover over me</div>
        /// </SfTooltip>
        /// 
        /// @code {
        ///     private void OnTooltipCreated(object args)
        ///     {
        ///         // Perform post-creation tasks
        ///         Console.WriteLine("Tooltip component created successfully");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is raised when the Tooltip component is destroyed.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> of type <see cref="object"/> that is invoked when the Tooltip component is being destroyed.
        /// </value>
        /// <remarks>
        /// This event is triggered when the Tooltip component is being disposed and removed from the component tree.
        /// It provides an opportunity to perform cleanup tasks, such as removing event handlers, clearing cached data, or disposing of resources.
        /// The event callback receives a generic <see cref="object"/> parameter that contains component destruction details.
        /// This is essential for proper memory management and preventing memory leaks in complex applications.
        /// Use this event to ensure that all resources associated with the Tooltip are properly cleaned up before the component is removed.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Destroyed="OnTooltipDestroyed">
        ///     <div>Hover over me</div>
        /// </SfTooltip>
        /// 
        /// @code {
        ///     private void OnTooltipDestroyed(object args)
        ///     {
        ///         // Perform cleanup tasks
        ///         Console.WriteLine("Tooltip component destroyed");
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        #endregion
    }
}
