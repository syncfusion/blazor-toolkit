using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Popups
{

    /// <summary>
    /// The SfTooltip component displays a pop-up containing information or a message when you hover, click, focus, or touch an element.
    /// </summary>
    /// <remarks>
    /// The SfTooltip component provides a highly customizable tooltip experience with support for various triggers, animations, positioning, and styling options.
    /// It can be used to display contextual information, form validation messages, or help text for UI elements.
    /// The component supports sticky tooltips, mouse trailing, collision detection, and rich content templates.
    /// <para><strong>Disposal and Resource Management:</strong></para>
    /// This component inherits from <see cref="SfBaseComponent"/> which implements <see cref="IDisposable"/>.
    /// When the component is removed from the render tree, the Blazor framework automatically calls Dispose() which:
    /// <list type="bullet">
    /// <item><description>Destroys the JavaScript tooltip instance on the client</description></item>
    /// <item><description>Releases the DotNetObjectReference used for JS interop callbacks</description></item>
    /// <item><description>Clears all internal collections and references</description></item>
    /// <item><description>Disposes window.sfBlazorToolkit client-side instances</description></item>
    /// <item><description>Suppresses finalization for optimized garbage collection</description></item>
    /// </list>
    /// Manual disposal is not required - the framework handles component lifecycle automatically.
    /// EventCallback subscriptions are also automatically managed by the Blazor runtime and do not require explicit cleanup.
    /// 
    /// <para><strong>Async/Await Pattern:</strong></para>
    /// All async methods in this component use <c>.ConfigureAwait(true)</c> to ensure continuation on the UI synchronization context.
    /// This is required for Blazor components because:
    /// <list type="bullet">
    /// <item><description>Component state updates must occur on the UI thread to maintain proper rendering context</description></item>
    /// <item><description>Event callbacks (EventCallback&lt;T&gt;) require the synchronization context for proper invocation</description></item>
    /// <item><description>StateHasChanged() and UI updates require access to the component's renderer</description></item>
    /// <item><description>JavaScript interop results may trigger UI updates that need the component context</description></item>
    /// </list>
    /// Using <c>.ConfigureAwait(false)</c> would cause runtime errors as the continuation would execute on a thread pool thread
    /// without access to the Blazor component's synchronization context.
    /// </remarks>
    /// <example>
    /// Basic usage of the SfTooltip component:
    /// <code><![CDATA[
    /// <SfTooltip Target="#target" Content="This is a tooltip">
    ///     <div id="target">Hover over me</div>
    /// </SfTooltip>
    /// ]]></code>
    /// </example>
    public partial class SfTooltip : SfBaseComponent
    {
        #region Constants

        private const string BEFORERENDERCALLBACK = "beforeRenderCallBack";
        private const string CONTENTUPDATED = "contentUpdated";
        private const string BEFOREOPENCALLBACK = "beforeOpenCallBack";
        private const string BEFORECLOSECALLBACK = "beforeCloseCallBack";
        private const string DESTROY = "destroy";
        private const string UPDATEPROPERTIES = "updateProperties";
        private const string WIREEVENTS = "wireEvents";
        private const string SHOWTOOLTIP = "showTooltip";
        private const string HIDETOOLTIP = "hideTooltip";
        private const string REFRESH = "refresh";
        private const string REFRESHPOSITION = "refreshPosition";

        // CSS Class Names
        private const string CONTROLCSSCLASS = "e-control e-tooltip e-lib";
        private const string TOOLTIPCONTENTCLASS = "e-hidden e-tooltip-wrap e-popup e-lib";
        private const string ARRORTIP = "e-arrow-tip";
        private const string ARROWTIPOUTER = "e-arrow-tip-outer";
        private const string ARROWIPINNER = "e-arrow-tip-inner";
        private const string CLOSEICON = "e-toolkit-icons e-close";
        private const string TOOLTIPCONTENTPLACEHOLDER = "e-tooltip-content-placeholder";
        private const string TOOLTIPCONTENT = "e-tip-content";

        #endregion

        #region Fields

        private string _classList = CONTROLCSSCLASS;
        private string? _tooltipTargetContainer;
        private IDictionary<string, object> _attributes = new Dictionary<string, object>();
        private ElementReference _tooltipElement;
        private bool _renderWrapper;
        private bool _beforeCollisionTriggered;
        private bool _isDestroyed;
        private bool _isScriptRendered;
        private bool _shouldRender = true;
        private string? _tooltipContent;
        private string? _tooltipHeight;
        private bool _tooltipIsSticky;
        private bool _tooltipShowTip;
        private double _tooltipOffsetX;
        private string? _tooltipCssClass;
        private bool _tooltipEnableRtl;
        private bool _tooltipWindowCollision;
        private double _tooltipOffsetY;
        private string? _tooltipOpensOn;
        private string? _tooltipTarget;
        private string? _tooltipContainer;
        private Position _tooltipPosition;
        private TipPointerPosition _tooltipTipPointerPosition;
        private string? _tooltipWidth;
        private IJSObjectReference? _tooltipJsModule;
        private IJSInProcessObjectReference? _tooltipInProcessModule;
        private IJSInProcessObjectReference? _animationInProcessModule;
        private static readonly Action<ILogger, Exception?> _jsRuntimeDisconnectedDuringDisposal =
            LoggerMessage.Define(LogLevel.Warning, new EventId(0, nameof(_jsRuntimeDisconnectedDuringDisposal)),
                "JS runtime was disconnected during tooltip disposal.");
        #endregion

        #region Internal Fields

        internal string _dataId = "sfTooltip-" + Guid.NewGuid().ToString();

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets or sets the JavaScript runtime instance used for JS interop operations in the tooltip component.
        /// </summary>
        /// <exclude/>
        [Inject]
        protected IJSRuntime? JsRuntime { get; set; }

        /// <summary>
        /// Gets or sets the logger instance for recording diagnostic information and errors.
        /// </summary>
        /// <exclude/>
        [Inject]
        protected ILogger<SfTooltip>? Logger { get; set; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets the dictionary of event configurations for the tooltip component.
        /// </summary>
        /// <exclude/>
        /// <returns>A dictionary containing event handler configurations with their respective delegate states.</returns>
        /// <remarks>
        /// This method builds a dictionary mapping event names to their delegate availability status.
        /// It's used internally to configure which JavaScript events should be wired up on the client side.
        /// Events include beforeRender, beforeCollision, beforeOpen, opened, beforeClose, and closed.
        /// </remarks>
        protected IDictionary<string, object> GetEventsList()
        {
            Dictionary<string, object> eventList = [];
            _ = SfBaseUtils.UpdateDictionary("beforeRender", OnRender.HasDelegate, eventList);
            _ = SfBaseUtils.UpdateDictionary("beforeCollision", Colliding.HasDelegate, eventList);
            _ = SfBaseUtils.UpdateDictionary("beforeOpen", OnOpen.HasDelegate, eventList);
            _ = SfBaseUtils.UpdateDictionary("opened", Opened.HasDelegate, eventList);
            _ = SfBaseUtils.UpdateDictionary("beforeClose", OnClose.HasDelegate, eventList);
            _ = SfBaseUtils.UpdateDictionary("closed", Closed.HasDelegate, eventList);
            return eventList;
        }

        /// <summary>
        /// Gets the dictionary of tooltip properties for JavaScript interop initialization.
        /// </summary>
        /// <exclude/>
        /// <returns>A dictionary containing all tooltip configuration properties and their current values.</returns>
        /// <remarks>
        /// This method serializes all tooltip properties into a dictionary format that can be passed to the JavaScript layer.
        /// It includes properties like target selectors, positioning, animation settings, content configuration, and behavioral flags.
        /// The method handles type conversions and conditional property inclusion based on current component state.
        /// </remarks>
        protected IDictionary<string, object> GetProperties()
        {
            Dictionary<string, object> properties = [];
            AddTargetingProperties(properties);
            AddPositioningProperties(properties);
            AddBehaviorProperties(properties);
            AddTimingProperties(properties);
            AddAppearanceProperties(properties);
            return properties;
        }

        /// <summary>
        /// Gets the dictionary of changed properties for dynamic tooltip updates.
        /// </summary>
        /// <exclude/>
        /// <returns>A dictionary containing only the properties that have changed since the last update.</returns>
        /// <remarks>
        /// This method processes the PropertyChanges collection to create a dictionary of modified properties.
        /// It maps internal property names to their JavaScript equivalents and includes their current values.
        /// This selective update approach optimizes performance by only sending changed properties to the client-side component.
        /// The content property is always included to ensure proper content synchronization.
        /// </remarks>
        protected IDictionary<string, object> GetPropertyChanges()
        {
            Dictionary<string, object> properties = [];

            if (PropertyChanges == null || PropertyChanges.Count == 0)
            {
                _ = SfBaseUtils.UpdateDictionary("content", !string.IsNullOrEmpty(Content) || ContentTemplate != null, properties);
                return properties;
            }

            foreach (KeyValuePair<string, object> propertyChange in PropertyChanges)
            {
                if (propertyChange.Key == null)
                {
                    continue;
                }

                // Route the change to the appropriate functional handler group.
                if (HandleTargetingChange(propertyChange.Key, properties))
                {
                    continue;
                }
                if (HandlePositioningChange(propertyChange.Key, properties))
                {
                    continue;
                }
                if (HandleBehaviorChange(propertyChange.Key, properties))
                {
                    continue;

                }
                if (HandleTimingChange(propertyChange.Key, properties))
                {
                    continue;
                }
                if (HandleAppearanceChange(propertyChange.Key, properties))
                {
                    continue;
                }
            }

            _ = SfBaseUtils.UpdateDictionary("content", !string.IsNullOrEmpty(Content) || ContentTemplate != null, properties);
            return properties;
        }

        /// <summary>
        /// Programmatically closes a sticky tooltip using click interaction.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous close operation.</returns>
        /// <remarks>
        /// This method is specifically designed for closing sticky tooltips that remain visible until explicitly closed.
        /// It simulates a click interaction to trigger the close animation and hide the tooltip.
        /// The method only executes if the JavaScript interop has been properly initialized (isScriptRendered is true).
        /// The close animation uses the configured Animation.Close settings for smooth transitions.
        /// <para><strong>Note:</strong></para>
        /// Uses <c>.ConfigureAwait(true)</c> to maintain UI thread synchronization context, which is required for
        /// Blazor component state management and proper event callback invocation.
        /// </remarks>
        /// <exclude/>
        protected async Task StickyCloseAsync()
        {
            bool isClick = true;
            if (_isScriptRendered && await IsTooltipJsAvailableAsync().ConfigureAwait(true))
            {
                await InvokeVoidAsync(_tooltipJsModule, _tooltipInProcessModule, HIDETOOLTIP, _dataId, Animation.Close!, isClick).ConfigureAwait(true);
            }
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// Gets the animation configuration model for tooltip open and close operations.
        /// </summary>
        /// <returns>An <see cref="AnimationModel"/> containing the animation settings for opening and closing the tooltip.</returns>
        /// <remarks>
        /// This method creates an animation model with separate configurations for open and close animations.
        /// If animation properties are not explicitly set, default values are used: 0ms delay, 150ms duration, FadeIn for open, and FadeOut for close.
        /// The animation model is used by the JavaScript layer to control tooltip transition effects.
        /// </remarks>
        internal AnimationModel GetAnimationValue()
        {
            return new AnimationModel()
            {
                Open = new TooltipAnimationSettings
                {
                    Delay = Animation.Open != null ? Animation.Open.Delay : 0,
                    Duration = Animation.Open != null ? Animation.Open.Duration : 150,
                    Effect = Animation.Open != null ? Animation.Open.Effect : Effect.FadeIn
                },
                Close = new TooltipAnimationSettings
                {
                    Delay = Animation.Close != null ? Animation.Close.Delay : 0,
                    Duration = Animation.Close != null ? Animation.Close.Duration : 150,
                    Effect = Animation.Close != null ? Animation.Close.Effect : Effect.FadeOut
                }
            };
        }

        /// <summary>
        /// Updates the tooltip content in the JavaScript layer after rendering or content changes.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous content update operation.</returns>
        /// <remarks>
        /// This method synchronizes the tooltip content between the Blazor component and the JavaScript layer.
        /// It handles different execution contexts (WASM, Server) and ensures content updates are applied correctly.
        /// The method only triggers content updates when the tooltip wrapper is rendered to avoid unnecessary DOM operations.
        /// For WASM applications, it includes additional checks for SyncfusionService integration.
        /// </remarks>
        internal async Task UpdatedTooltipContentAsync()
        {
            // Require both the JS module to be loaded and wireEvents to have completed.
            // contentUpdated() accesses the JS instance's internal DOM element, which is
            // only created after wireEvents runs — calling it before that causes a querySelector crash.
            if (!_isScriptRendered || !await IsTooltipJsAvailableAsync().ConfigureAwait(true))
            {
                return;
            }

            if (_renderWrapper)
            {
                await InvokeVoidAsync(_tooltipJsModule, _tooltipInProcessModule, CONTENTUPDATED, _dataId).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Performs cleanup operations when the tooltip component is being disposed.
        /// </summary>
        /// <remarks>
        /// This method handles the complete cleanup of the tooltip component including:
        /// - Destroying the JavaScript tooltip instance
        /// - Clearing internal references and collections
        /// - Triggering the Destroyed event
        /// - Disposing window instances and managed resources
        /// - Handling disposal exceptions gracefully
        /// 
        /// The method ensures proper resource cleanup to prevent memory leaks and handles
        /// exceptions that may occur during the disposal process by notifying through the Destroyed event.
        /// It only executes cleanup operations if the component was previously rendered and not already destroyed.
        /// </remarks>
        protected override async ValueTask DisposeAsyncCore()
        {
            if (IsRendered && !_isDestroyed)
            {
                try
                {
                    _classList = null!;
                    _attributes = null!;
                    _isDestroyed = true;

                    // Only call JS destroy when the module is available.
                    if (await IsTooltipJsAvailableAsync().ConfigureAwait(true))
                    {
                        await InvokeVoidAsync(_tooltipJsModule!, _tooltipInProcessModule!, DESTROY, _dataId).ConfigureAwait(true);
                        // Use ConfigureAwait(true) consistently to stay on the UI synchronization context.
                        await WindowInstanceDisposeAsync(_dataId).ConfigureAwait(true);
                    }

                    if (Destroyed.HasDelegate)
                    {
                        await Destroyed.InvokeAsync(null).ConfigureAwait(true);
                    }

                    // Dispose only tooltip-specific JS modules here. The shared base-class modules
                    // (_popupJsModule, _popupInProcessModule, _animationJsModule) are owned and
                    // disposed by SfBaseComponent.DisposeAsync() — disposing them here too would
                    // cause double-disposal due to the async void fire-and-forget execution order.
                    if (_tooltipJsModule != null)
                    {
                        await _tooltipJsModule.DisposeAsync().ConfigureAwait(true);
                        _tooltipJsModule = null;
                    }

                    _tooltipInProcessModule?.Dispose();
                    _tooltipInProcessModule = null;

                    _animationInProcessModule?.Dispose();
                    _animationInProcessModule = null;
                }
                catch (JSDisconnectedException ex)
                {
                    // JS runtime may be disconnected during disposal (e.g. page reload). Null out
                    // references so they are not reused after the circuit is gone.
                    if (Logger != null)
                    {
                        _jsRuntimeDisconnectedDuringDisposal(Logger, ex);
                    }
                    _tooltipJsModule = null;
                    _tooltipInProcessModule?.Dispose();
                    _tooltipInProcessModule = null;
                    _animationInProcessModule?.Dispose();
                    _animationInProcessModule = null;
                }
                catch (InvalidOperationException invEx)
                {
                    // Dispose tooltip-specific modules to prevent leaks before notifying consumers.
                    _tooltipJsModule = null;
                    _tooltipInProcessModule?.Dispose();
                    _tooltipInProcessModule = null;
                    _animationInProcessModule?.Dispose();
                    _animationInProcessModule = null;

                    // Notify consumers that the component was destroyed due to invalid operation.
                    if (Destroyed.HasDelegate)
                    {
                        await Destroyed.InvokeAsync(invEx).ConfigureAwait(true);
                    }
                }
                catch (Exception ex)
                {
                    // Notify consumers about unexpected errors. Do NOT re-throw from async void —
                    // an unhandled exception here propagates directly to the SynchronizationContext
                    // and will terminate the Blazor Server circuit or crash the WASM app.
                    try
                    {
                        if (Destroyed.HasDelegate)
                        {
                            await Destroyed.InvokeAsync(ex).ConfigureAwait(true);
                        }
                    }
                    catch
                    {
                        throw;
                        // Swallow exceptions from the Destroyed callback to avoid masking the original exception.
                    }

                    throw;
                }
            }
            await base.DisposeAsyncCore().ConfigureAwait(true);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates or removes the tooltip wrapper element based on the JavaScript callback.
        /// </summary>
        /// <param name="args">A boolean value indicating whether the tooltip wrapper should be rendered (true) or removed (false).</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is invoked from JavaScript to dynamically control the rendering of the tooltip wrapper element.
        /// When args is true, it triggers a re-render to include the tooltip wrapper in the DOM.
        /// When args is false, it triggers a re-render to remove the tooltip wrapper from the DOM.
        /// This approach ensures efficient DOM management by only rendering tooltip elements when needed.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CreateTooltipAsync(bool args)
        {
            _renderWrapper = args;
            await InvokeAsync(StateHasChanged).ConfigureAwait(true);
        }

        /// <summary>
        /// Triggers the beforeRender event and handles the tooltip rendering workflow.
        /// </summary>
        /// <param name="beforeRenderArgs">The event arguments containing tooltip rendering information and cancellation support.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous event triggering and callback operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript before the tooltip is rendered to the DOM.
        /// Tooltip properties can be modified or the rendering process cancelled through the event args.
        /// The method handles the event delegation, processes cancellation status, and triggers the JavaScript callback.
        /// For WASM applications, tooltip content is immediately updated after the event processing.
        /// The cancellation status is passed back to JavaScript to control the rendering workflow.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerBeforeRenderEventAsync(TooltipEventArgs beforeRenderArgs)
        {
            bool isCancelled = false;
            if (OnRender.HasDelegate && beforeRenderArgs != null)
            {
                beforeRenderArgs.JsRuntime = JSRuntime!;
                await OnRender.InvokeAsync(beforeRenderArgs).ConfigureAwait(true);
                isCancelled = beforeRenderArgs.Cancel;
            }
            if (_tooltipJsModule != null || _tooltipInProcessModule != null)
            {
                try
                {
                    await InvokeVoidAsync(_tooltipJsModule!, _tooltipInProcessModule!, BEFORERENDERCALLBACK, _dataId, isCancelled).ConfigureAwait(true);
                }
                catch (JSDisconnectedException)
                {
                    // Circuit disconnected while awaiting BeforeRender event handler — safe to swallow.
                }
            }
            // Check whether the application is WASM or not.
            if (JsRuntime is IJSInProcessRuntime)
            {
                await UpdatedTooltipContentAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Triggers the beforeCollision event when the tooltip position needs adjustment due to viewport boundaries.
        /// </summary>
        /// <param name="beforeCollisionArgs">The event arguments containing collision detection information and positioning details.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous event triggering operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript when the tooltip's position would cause it to extend beyond the viewport boundaries.
        /// The collision handling behavior can be customized or notifications about position adjustments can be received.
        /// The method includes a flag (beforeCollisionTriggered) to ensure the event is only triggered once per tooltip display cycle.
        /// The event provides information about the collision axis, available space, and suggested position adjustments.
        /// This event is particularly useful for implementing custom collision detection algorithms or logging positioning issues.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerBeforeCollisionEventAsync(TooltipEventArgs beforeCollisionArgs)
        {
            if (!_beforeCollisionTriggered)
            {
                _beforeCollisionTriggered = true;
                if (Colliding.HasDelegate && beforeCollisionArgs != null)
                {
                    beforeCollisionArgs.JsRuntime = JSRuntime!;
                    await Colliding.InvokeAsync(beforeCollisionArgs).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Triggers the beforeOpen event before the tooltip is displayed and handles cancellation logic.
        /// </summary>
        /// <param name="beforeOpenEventArgs">The event arguments containing tooltip opening information and cancellation support.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous event triggering and callback operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript just before the tooltip becomes visible to the user.
        /// Tooltip properties can be modified, conditions validated, or the opening process cancelled.
        /// The event args allow setting the Cancel property to true, which prevents the tooltip from being displayed.
        /// After processing the event, the cancellation status is communicated back to the JavaScript layer.
        /// This event is essential for implementing conditional tooltip display logic and dynamic content validation.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerBeforeOpenEventAsync(TooltipEventArgs beforeOpenEventArgs)
        {
            bool isCancelled = false;
            if (OnOpen.HasDelegate && beforeOpenEventArgs != null)
            {
                beforeOpenEventArgs.JsRuntime = JSRuntime!;
                await OnOpen.InvokeAsync(beforeOpenEventArgs).ConfigureAwait(true);
                isCancelled = beforeOpenEventArgs.Cancel;
            }
            if (_tooltipJsModule != null || _tooltipInProcessModule != null)
            {
                try
                {
                    await InvokeVoidAsync(_tooltipJsModule!, _tooltipInProcessModule!, BEFOREOPENCALLBACK, _dataId, isCancelled).ConfigureAwait(true);
                }
                catch (JSDisconnectedException)
                {
                    // Circuit disconnected while awaiting BeforeOpen event handler — safe to swallow.
                }
            }
        }

        /// <summary>
        /// Triggers the opened event after the tooltip has been successfully displayed and animations completed.
        /// </summary>
        /// <param name="openedEventArgs">The event arguments containing information about the opened tooltip state.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous event triggering operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript after the tooltip has been fully rendered and all opening animations have completed.
        /// Confirmation is provided that the tooltip is now visible and interactive for the user.
        /// The event is useful for performing post-display operations, analytics tracking, or UI state updates.
        /// Unlike the beforeOpen event, this event cannot be cancelled as the tooltip is already visible.
        /// The event args contain information about the final tooltip position, dimensions, and display state.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerOpenedEventAsync(TooltipEventArgs openedEventArgs)
        {
            if (Opened.HasDelegate && openedEventArgs != null)
            {
                openedEventArgs.JsRuntime = JSRuntime!;
                await Opened.InvokeAsync(openedEventArgs).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Triggers the beforeClose event before the tooltip is hidden and handles cancellation logic.
        /// </summary>
        /// <param name="beforeCloseEventArgs">The event arguments containing tooltip closing information and cancellation support.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous event triggering and callback operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript just before the tooltip starts its closing process.
        /// Closing conditions can be validated, state saved, or the closing operation cancelled.
        /// The event args allow setting the Cancel property to true, which prevents the tooltip from being hidden.
        /// After processing the event, the cancellation status is communicated back to the JavaScript layer.
        /// This event is particularly useful for implementing sticky tooltips or preventing accidental closures.
        /// The cancellation feature allows for sophisticated tooltip management scenarios.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerBeforeCloseEventAsync(TooltipEventArgs beforeCloseEventArgs)
        {
            bool isCancelled = false;
            if (OnClose.HasDelegate && beforeCloseEventArgs != null)
            {
                beforeCloseEventArgs.JsRuntime = JSRuntime!;
                await OnClose.InvokeAsync(beforeCloseEventArgs).ConfigureAwait(true);
                isCancelled = beforeCloseEventArgs.Cancel;
            }
            if (_tooltipJsModule != null || _tooltipInProcessModule != null)
            {
                try
                {
                    await InvokeVoidAsync(_tooltipJsModule!, _tooltipInProcessModule!, BEFORECLOSECALLBACK, _dataId, isCancelled).ConfigureAwait(true);
                }
                catch (JSDisconnectedException)
                {
                    // Circuit disconnected while awaiting BeforeClose event handler — safe to swallow.
                }
            }
        }

        /// <summary>
        /// Triggers the closed event after the tooltip has been successfully hidden and animations completed.
        /// </summary>
        /// <param name="closedEventArgs">The event arguments containing information about the closed tooltip state.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous event triggering operation.</returns>
        /// <remarks>
        /// This method is called from JavaScript after the tooltip has been fully hidden and all closing animations have completed.
        /// Confirmation is provided that the tooltip is no longer visible and has been removed from the display.
        /// The method also resets the beforeCollisionTriggered flag to prepare for the next tooltip display cycle.
        /// The event is useful for performing cleanup operations, state resets, or analytics tracking.
        /// Unlike the beforeClose event, this event cannot be cancelled as the tooltip is already hidden.
        /// The event args contain information about the final tooltip state and closing context.
        /// </remarks>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerClosedEventAsync(TooltipEventArgs closedEventArgs)
        {
            if (Closed.HasDelegate && closedEventArgs != null)
            {
                closedEventArgs.JsRuntime = JSRuntime!;
                await Closed.InvokeAsync(closedEventArgs).ConfigureAwait(true);
            }
            _beforeCollisionTriggered = false;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Adds target and container related properties to the dictionary.
        /// </summary>
        /// <param name="properties">The dictionary to update with targeting properties.</param>
        private void AddTargetingProperties(Dictionary<string, object> properties)
        {
            _ = SfBaseUtils.UpdateDictionary("target", Target, properties);
            _ = SfBaseUtils.UpdateDictionary("container", Container, properties);
            _ = SfBaseUtils.UpdateDictionary("targetContainer", TargetContainer, properties);
            _ = SfBaseUtils.UpdateDictionary("opensOn", OpensOn, properties);
        }

        /// <summary>
        /// Adds positioning related properties to the dictionary.
        /// </summary>
        /// <param name="properties">The dictionary to update with positioning properties.</param>
        private void AddPositioningProperties(Dictionary<string, object> properties)
        {
            _ = SfBaseUtils.UpdateDictionary("position", SfBaseUtils.ChangeType(Position, typeof(string))!, properties);
            _ = SfBaseUtils.UpdateDictionary("offsetX", OffsetX, properties);
            _ = SfBaseUtils.UpdateDictionary("offsetY", OffsetY, properties);
            _ = SfBaseUtils.UpdateDictionary("tipPointerPosition", SfBaseUtils.ChangeType(TipPointerPosition, typeof(string))!, properties);
            _ = SfBaseUtils.UpdateDictionary("windowCollision", WindowCollision, properties);
        }

        /// <summary>
        /// Adds behavior related properties to the dictionary.
        /// </summary>
        /// <param name="properties">The dictionary to update with behavior properties.</param>
        private void AddBehaviorProperties(Dictionary<string, object> properties)
        {
            _ = SfBaseUtils.UpdateDictionary("isSticky", IsSticky, properties);
            _ = SfBaseUtils.UpdateDictionary("mouseTrail", MouseTrail, properties);
            _ = SfBaseUtils.UpdateDictionary("showTipPointer", ShowTipPointer, properties);
        }

        /// <summary>
        /// Adds timing and animation related properties to the dictionary.
        /// </summary>
        /// <param name="properties">The dictionary to update with timing properties.</param>
        private void AddTimingProperties(Dictionary<string, object> properties)
        {
            _ = SfBaseUtils.UpdateDictionary("animation", GetAnimationValue(), properties);
            _ = SfBaseUtils.UpdateDictionary("closeDelay", CloseDelay, properties);
            _ = SfBaseUtils.UpdateDictionary("openDelay", OpenDelay, properties);
        }

        /// <summary>
        /// Adds appearance related properties to the dictionary.
        /// </summary>
        /// <param name="properties">The dictionary to update with appearance properties.</param>
        private void AddAppearanceProperties(Dictionary<string, object> properties)
        {
            _ = SfBaseUtils.UpdateDictionary("width", Width, properties);
            _ = SfBaseUtils.UpdateDictionary("height", Height, properties);
            _ = SfBaseUtils.UpdateDictionary("enableRtl", SyncfusionService != null && SyncfusionService._options.EnableRtl, properties);
            _ = SfBaseUtils.UpdateDictionary("content", !string.IsNullOrEmpty(Content) || ContentTemplate != null, properties);
        }

        /// <summary>
        /// Handle targeting-related property changes (target/container/opensOn).
        /// </summary>
        private bool HandleTargetingChange(string key, Dictionary<string, object> properties)
        {
            switch (key)
            {
                case "Target":
                    _ = SfBaseUtils.UpdateDictionary("target", Target, properties);
                    return true;
                case "Container":
                    _ = SfBaseUtils.UpdateDictionary("container", Container, properties);
                    return true;
                case "OpensOn":
                    _ = SfBaseUtils.UpdateDictionary("opensOn", OpensOn, properties);
                    return true;
                case "TargetContainer":
                    _ = SfBaseUtils.UpdateDictionary("targetContainer", TargetContainer, properties);
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Handle positioning-related property changes (position, offsets, tip pointer, window collision).
        /// </summary>
        private bool HandlePositioningChange(string key, Dictionary<string, object> properties)
        {
            switch (key)
            {
                case "Position":
                    _ = SfBaseUtils.UpdateDictionary("position", Position.ToString(), properties);
                    return true;
                case "OffsetX":
                    _ = SfBaseUtils.UpdateDictionary("offsetX", OffsetX, properties);
                    return true;
                case "OffsetY":
                    _ = SfBaseUtils.UpdateDictionary("offsetY", OffsetY, properties);
                    return true;
                case "TipPointerPosition":
                    _ = SfBaseUtils.UpdateDictionary("tipPointerPosition", TipPointerPosition.ToString(), properties);
                    return true;
                case "windowCollision":
                    _ = SfBaseUtils.UpdateDictionary("windowCollision", WindowCollision, properties);
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Handle behavioral property changes (sticky, mouse trail, show tip).
        /// </summary>
        private bool HandleBehaviorChange(string key, Dictionary<string, object> properties)
        {
            switch (key)
            {
                case "IsSticky":
                    _ = SfBaseUtils.UpdateDictionary("isSticky", IsSticky, properties);
                    return true;
                case "MouseTrail":
                    _ = SfBaseUtils.UpdateDictionary("mouseTrail", MouseTrail, properties);
                    return true;
                case "ShowTipPointer":
                    _ = SfBaseUtils.UpdateDictionary("showTipPointer", ShowTipPointer, properties);
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Handle timing and animation related property changes.
        /// </summary>
        private bool HandleTimingChange(string key, Dictionary<string, object> properties)
        {
            switch (key)
            {
                case "Animation":
                    _ = SfBaseUtils.UpdateDictionary("animation", GetAnimationValue(), properties);
                    return true;
                case "CloseDelay":
                    _ = SfBaseUtils.UpdateDictionary("closeDelay", CloseDelay, properties);
                    return true;
                case "OpenDelay":
                    _ = SfBaseUtils.UpdateDictionary("openDelay", OpenDelay, properties);
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Handle appearance related property changes (width, height, RTL flag, content presence).
        /// </summary>
        private bool HandleAppearanceChange(string key, Dictionary<string, object> properties)
        {
            switch (key)
            {
                case "Width":
                    _ = SfBaseUtils.UpdateDictionary("width", Width, properties);
                    return true;
                case "Height":
                    _ = SfBaseUtils.UpdateDictionary("height", Height, properties);
                    return true;
                case "EnableRtl":
                    _ = SfBaseUtils.UpdateDictionary("enableRtl", SyncfusionService != null && SyncfusionService._options.EnableRtl, properties);
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns whether animations are enabled by global service settings.
        /// </summary>
        private bool IsAnimationEnabled()
        {
            return SyncfusionService is not null &&
                   (SyncfusionService._options.Animation == GlobalAnimationMode.Default ||
                    SyncfusionService._options.Animation == GlobalAnimationMode.Enable);
        }

        #endregion
    }
}