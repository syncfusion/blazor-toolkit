using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// The SfTooltip component displays a pop-up containing information or a message when you hover, click, focus, or touch an element.
    /// </summary>
    /// <remarks>
    /// The <see cref="SfTooltip"/> component provides contextual information in a lightweight overlay that appears near target elements.
    /// It supports various interaction modes including hover, click, focus, and touch, with customizable positioning, animation, and styling options.
    /// The component automatically handles collision detection and repositioning to ensure optimal visibility within the viewport.
    /// </remarks>
    /// <example>
    /// A simple Tooltip component.
    /// <code><![CDATA[
    /// <SfTooltip Content="This is a tooltip">
    ///     <button>Hover me</button>
    /// </SfTooltip>
    /// ]]></code>
    /// </example>
    public partial class SfTooltip
    {
        #region Public Methods
        /// <summary>
        /// Asynchronously opens the Tooltip with customizable animation settings and target element.
        /// </summary>
        /// <param name="handleId"></param>
        /// <param name="animation">The animation settings to apply during the tooltip opening transition. When <see langword="null"/>, uses the default "Open" animation configuration from the <see cref="Animation"/> property.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation of opening the tooltip with the specified settings.</returns>
        /// <remarks>
        /// <para>
        /// This method programmatically displays the tooltip with precise control over the target element and animation behavior.
        /// The method ensures proper JavaScript interop communication to render the tooltip with optimal positioning and visual effects.
        /// </para>
        /// <para>
        /// When specifying a custom target element, the tooltip will be positioned relative to that element using the configured positioning logic.
        /// The animation parameter allows for fine-grained control over the opening transition, including duration, easing, and effects.
        /// </para>
        /// <para>
        /// The method performs validation to ensure the component's JavaScript runtime is properly initialized before executing the tooltip display logic.
        /// If the script is not rendered, the operation will be deferred until the component is fully loaded.
        /// </para>
        /// </remarks>
        /// <example>
        /// Opening a tooltip with custom animation on a specific element.
        /// <code><![CDATA[
        /// // Open tooltip on a specific button element with fade-in animation
        /// var customAnimation = new TooltipAnimationSettings { Effect = "FadeIn", Duration = 300 };
        /// await tooltipComponent.OpenAsync(buttonElementId, customAnimation);
        /// 
        /// // Open tooltip with default settings
        /// await tooltipComponent.OpenAsync();
        /// ]]></code>
        /// </example>
        public async Task OpenAsync(string? handleId = null, TooltipAnimationSettings? animation = null)
        {
            animation ??= Animation.Open;
            if (_isScriptRendered && await IsTooltipJsAvailableAsync().ConfigureAwait(true))
            {
                // If a handle id is provided, build a selector to target that handle element.
                string targetProp = !string.IsNullOrEmpty(handleId) ? "#" + handleId : Target;
                await InvokeVoidAsync(_tooltipJsModule, _tooltipInProcessModule, SHOWTOOLTIP, _dataId, animation!, targetProp).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Asynchronously closes the Tooltip with customizable animation settings for the closing transition.
        /// </summary>
        /// <param name="animation">The animation configuration to apply during the tooltip closing transition. When <see langword="null"/>, uses the default "Close" animation configuration from the <see cref="Animation"/> property.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation of closing the tooltip with the specified animation settings.</returns>
        /// <remarks>
        /// <para>
        /// This method programmatically hides the currently visible tooltip with precise control over the closing animation behavior.
        /// The method ensures smooth visual transitions and proper cleanup of tooltip resources through JavaScript interop communication.
        /// </para>
        /// <para>
        /// The animation parameter provides comprehensive control over the closing transition, including effect type, duration, easing function, and delay settings.
        /// When no custom animation is specified, the component uses the default closing animation configured in the <see cref="Animation"/> property.
        /// </para>
        /// <para>
        /// The method includes validation to ensure the component's JavaScript runtime is properly initialized before executing the tooltip hiding logic.
        /// This prevents potential runtime errors when the component is not fully loaded or has been disposed.
        /// </para>
        /// </remarks>
        /// <example>
        /// Closing a tooltip with custom animation settings.
        /// <code><![CDATA[
        /// // Close tooltip with custom fade-out animation
        /// var customCloseAnimation = new TooltipAnimationSettings { Effect = "FadeOut", Duration = 500 };
        /// await tooltipComponent.CloseAsync(customCloseAnimation);
        /// 
        /// // Close tooltip with default animation
        /// await tooltipComponent.CloseAsync();
        /// ]]></code>
        /// </example>
        public async Task CloseAsync(TooltipAnimationSettings? animation = null)
        {
            animation ??= Animation.Close;
            if (_isScriptRendered && await IsTooltipJsAvailableAsync().ConfigureAwait(true))
            {
                await InvokeVoidAsync(_tooltipJsModule, _tooltipInProcessModule, HIDETOOLTIP, _dataId, animation!).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Asynchronously refreshes the entire Tooltip component to synchronize with dynamic DOM changes and target element updates.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation of refreshing the tooltip component's configuration and bindings.</returns>
        /// <remarks>
        /// <para>
        /// This method performs a comprehensive refresh of the tooltip component, rebuilding its internal state to accommodate dynamic changes in the target elements or DOM structure.
        /// It is particularly useful when target elements are added, removed, or modified after the initial component initialization.
        /// </para>
        /// <para>
        /// The refresh operation re-evaluates all tooltip configurations, including target selectors, positioning logic, content bindings, and event handlers.
        /// This ensures that the tooltip maintains proper functionality even when the underlying DOM structure changes dynamically through JavaScript manipulation or framework updates.
        /// </para>
        /// <para>
        /// The method communicates with the JavaScript runtime to rebuild the tooltip's internal data structures and re-establish proper event bindings with updated target elements.
        /// This operation is essential for maintaining tooltip functionality in single-page applications where DOM content changes frequently.
        /// </para>
        /// </remarks>
        /// <example>
        /// Refreshing the tooltip after dynamic content changes.
        /// <code><![CDATA[
        /// // After adding new elements dynamically to the page
        /// await AddNewElementsToPage();
        /// await tooltipComponent.RefreshAsync();
        /// 
        /// // After modifying existing target elements
        /// await UpdateTargetElementAttributes();
        /// await tooltipComponent.RefreshAsync();
        /// ]]></code>
        /// </example>
        public async Task RefreshAsync()
        {
            if (await IsTooltipJsAvailableAsync().ConfigureAwait(true))
            {
                await InvokeVoidAsync(_tooltipJsModule, _tooltipInProcessModule, REFRESH, _dataId).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Asynchronously recalculates and updates the Tooltip's position based on the current location and dimensions of the target element.
        /// </summary>
        /// <param name="target">The target element reference for position calculation. When <see langword="null"/>, uses the default target element configured through the <see cref="Target"/> property.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation of recalculating and updating the tooltip's position.</returns>
        /// <remarks>
        /// <para>
        /// This method performs precise position recalculation for scenarios where the target element's position, size, or visibility has changed after the tooltip was initially positioned.
        /// It is essential for maintaining proper tooltip alignment in dynamic layouts, responsive designs, or when target elements are moved programmatically.
        /// </para>
        /// <para>
        /// The position refresh operation considers all positioning constraints including collision detection, viewport boundaries, offset values, and preferred positioning strategies.
        /// The method ensures optimal tooltip placement while respecting the configured positioning preferences and avoiding content overflow or clipping.
        /// </para>
        /// <para>
        /// When a specific target element is provided, the method calculates the position relative to that element's current bounding rectangle.
        /// If no target is specified, the method uses the default target configuration, making it suitable for both targeted and general position updates.
        /// </para>
        /// <para>
        /// This operation is particularly valuable in scenarios such as window resizing, scrolling, element animations, or dynamic content changes that affect element positioning.
        /// </para>
        /// </remarks>
        /// <example>
        /// Refreshing tooltip position after layout changes.
        /// <code><![CDATA[
        /// // Refresh position after window resize
        /// window.addEventListener("resize", async () => {
        ///     await tooltipComponent.RefreshPositionAsync();
        /// });
        /// 
        /// // Refresh position for a specific target element after animation
        /// await AnimateElement(targetElement);
        /// await tooltipComponent.RefreshPositionAsync(targetElement);
        /// 
        /// // Refresh position after scrolling or layout changes
        /// await tooltipComponent.RefreshPositionAsync();
        /// ]]></code>
        /// </example>
        public async Task RefreshPositionAsync(ElementReference? target = null)
        {
            if (await IsTooltipJsAvailableAsync().ConfigureAwait(true))
            {
                // Use the provided ElementReference when given; fall back to the string Target selector otherwise.
                object? targetArg = target.HasValue ? target.Value : Target;
                await InvokeVoidAsync(_tooltipJsModule, _tooltipInProcessModule, REFRESHPOSITION, _dataId, targetArg).ConfigureAwait(true);
            }
        }

        #endregion

        #region Internal Methods
        /// <summary>
        /// Set tooltip content from external components in a safe way.
        /// This updates the component's Content property internally and refreshes slider the tooltip.
        /// </summary>
        /// <param name="content">New content for the tooltip.</param>
        internal async Task SetContentAsync(string content)
        {
            Content = content;
            await RefreshAsync().ConfigureAwait(true);
        }
        #endregion
    }
}
