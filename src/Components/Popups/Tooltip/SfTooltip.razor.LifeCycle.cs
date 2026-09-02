using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// The SfTooltip component displays a pop-up containing information or a message when you hover, click, focus, or touch an element.
    /// </summary>
    /// <remarks>
    /// The SfTooltip component provides flexible tooltip functionality with various positioning options, animation effects, 
    /// and customizable appearance. It supports different trigger modes including hover, click, focus, and custom events.
    /// The tooltip can be positioned at different locations relative to the target element and supports automatic collision detection.
    /// <para><strong>Async/Await Pattern:</strong></para>
    /// All lifecycle methods use <c>.ConfigureAwait(true)</c> to preserve the Blazor synchronization context, ensuring that
    /// state updates, event callbacks, and UI rendering operations execute on the correct thread
    /// </remarks>
    /// <example>
    /// Basic tooltip implementation:
    /// <code><![CDATA[
    /// <SfTooltip Content="Click to save the document">
    ///     <button>Save</button>
    /// </SfTooltip>
    /// ]]></code>
    /// </example>
    public partial class SfTooltip : SfBaseComponent
    {
        #region Constants
        private const string IDPREFIX = "tooltip-";
        private static readonly Action<ILogger, Exception?> _tooltipJsModuleFetchFailed =
            LoggerMessage.Define(LogLevel.Error, new EventId(0, nameof(_tooltipJsModuleFetchFailed)),
                "tooltip.js module could not be fetched. Rebuild the project and restart the server to regenerate the static asset fingerprints.");
        #endregion

        #region Public Methods

        /// <summary>
        /// Controls the re-rendering behavior of the Tooltip component.
        /// </summary>
        /// <param name="preventRender">Optional. Determines whether the component should be prevented from re-rendering. Default value is true.</param>
        /// <remarks>
        /// This method internally sets the value to be returned by the ShouldRender method.
        /// By default, this method prevents the component from rendering. To enable rendering again, set preventRender to false.
        /// </remarks>
        public void PreventRender(bool preventRender = true)
        {
            _shouldRender = !preventRender;
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// Asynchronously initializes the SfTooltip component.
        /// </summary>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        /// <remarks>
        /// This method performs the following initialization tasks:
        /// <list type="bullet">
        /// <item><description>Generates a unique ID if none is provided</description></item>
        /// <item><description>Initializes all internal property values from component parameters</description></item>
        /// <item><description>Sets up HTML attributes and CSS classes</description></item>
        /// <item><description>Configures the required script modules for tooltip functionality</description></item>
        /// </list>
        /// The method ensures that all tooltip properties are properly initialized before the component is rendered.
        /// </remarks>
        /// <exclude />
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = IDPREFIX + Guid.NewGuid().ToString();
            }

            await base.OnInitializedAsync().ConfigureAwait(true);
            _tooltipContent = Content;
            _tooltipCssClass = CssClass;
            _tooltipEnableRtl = SyncfusionService != null && SyncfusionService._options.EnableRtl;
            _tooltipHeight = Height;
            _tooltipOffsetX = OffsetX;
            _tooltipOffsetY = OffsetY;
            _tooltipOpensOn = OpensOn;
            _tooltipPosition = Position;
            _tooltipTipPointerPosition = TipPointerPosition;
            _tooltipWindowCollision = WindowCollision;
            _tooltipWidth = Width;
            _tooltipTarget = Target;
            _tooltipContainer = Container;
            _tooltipIsSticky = IsSticky;
            _tooltipShowTip = ShowTipPointer;
            _tooltipTargetContainer = TargetContainer;
            _attributes = GetAttributes(_classList, HtmlAttributes ?? []);

            // Accessibility: if no explicit Target is provided, the wrapper div is the
            // tooltip trigger. Ensure it references the tooltip content via
            // `aria-describedby="{ID}_content"` unless the consumer already set
            // an `aria-describedby` attribute (case-insensitive).
            if (string.IsNullOrEmpty(Target))
            {
                bool hasAriaDescribedBy = _attributes.Keys.Any(k => string.Equals(k, "aria-describedby", StringComparison.OrdinalIgnoreCase));
                if (!hasAriaDescribedBy)
                {
                    _attributes["aria-describedby"] = ID + "_content";
                }
            }
        }

        /// <summary>
        /// Diffs the tooltip's public properties against cached backing fields and queues the changed keys for client-side dispatch.
        /// </summary>
        /// <returns>A <see cref="Task"/> that completes when the parameter diff and queue update are finished.</returns>
        /// <remarks>
        /// Invoked by the framework after the base parameter cycle assigns the new values. The method uses <c>NotifyPropertyChanges</c> to compare each public property (such as <see cref="Content"/>, <see cref="CssClass"/>, <see cref="Position"/>, <see cref="OpensOn"/>, <see cref="Height"/>, <see cref="Width"/>, <see cref="Target"/>, <see cref="Container"/>, <see cref="OffsetX"/>, <see cref="OffsetY"/>, <see cref="IsSticky"/>, <see cref="WindowCollision"/>, <see cref="TipPointerPosition"/>, <see cref="ShowTipPointer"/>, <see cref="TargetContainer"/>, and the RTL option) against the cached <c>_tooltip*</c> fields, recording only the changes. The changed property keys are then captured into <c>_pendingPropertyChanges</c> so that <see cref="OnAfterRenderAsync"/> can dispatch a single <c>UPDATEPROPERTIES</c> call to the tooltip JavaScript module on the next render.
        /// </remarks>
        /// <exclude />
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            _tooltipContent = NotifyPropertyChanges(nameof(Content), Content, _tooltipContent);
            _tooltipCssClass = NotifyPropertyChanges(nameof(CssClass), CssClass, _tooltipCssClass);
            if (SyncfusionService != null)
            {
                _tooltipEnableRtl = NotifyPropertyChanges(nameof(SyncfusionService._options.EnableRtl), SyncfusionService._options.EnableRtl, _tooltipEnableRtl);
            }
            _tooltipWindowCollision = NotifyPropertyChanges(nameof(WindowCollision), WindowCollision, _tooltipWindowCollision);
            _tooltipHeight = NotifyPropertyChanges(nameof(Height), Height, _tooltipHeight);
            _tooltipTarget = NotifyPropertyChanges(nameof(Target), Target, _tooltipTarget);
            _tooltipContainer = NotifyPropertyChanges(nameof(Container), Container, _tooltipContainer);
            _tooltipOffsetX = NotifyPropertyChanges(nameof(OffsetX), OffsetX, _tooltipOffsetX);
            _tooltipOffsetY = NotifyPropertyChanges(nameof(OffsetY), OffsetY, _tooltipOffsetY);
            _tooltipOpensOn = NotifyPropertyChanges(nameof(OpensOn), OpensOn, _tooltipOpensOn);
            _tooltipPosition = NotifyPropertyChanges(nameof(Position), Position, _tooltipPosition);
            _tooltipIsSticky = NotifyPropertyChanges(nameof(IsSticky), IsSticky, _tooltipIsSticky);
            _tooltipTipPointerPosition = NotifyPropertyChanges(nameof(TipPointerPosition), TipPointerPosition, _tooltipTipPointerPosition);
            _tooltipWidth = NotifyPropertyChanges(nameof(Width), Width, _tooltipWidth);
            _tooltipShowTip = NotifyPropertyChanges(nameof(ShowTipPointer), ShowTipPointer, _tooltipShowTip);
            _tooltipTargetContainer = NotifyPropertyChanges(nameof(TargetContainer), TargetContainer, _tooltipTargetContainer);
            if (PropertyChanges is not null && PropertyChanges.Count > 0)
            {
                _pendingPropertyChanges = [.. PropertyChanges.Keys];
            }
        }

        /// <summary>
        /// Fires the <c>Created</c> event, dispatches pending property changes to the client, and refreshes the tooltip content after each render.
        /// </summary>
        /// <param name="firstRender"><c>true</c> on the first render after the component is created; <c>false</c> on subsequent renders.</param>
        /// <returns>A <see cref="Task"/> that completes when all post-render operations finish.</returns>
        /// <remarks>
        /// Invoked by the framework after each render cycle. On the first render, the <c>Created</c> event is raised when a delegate is registered. After the script is loaded, pending property changes recorded during <see cref="OnParametersSetAsync"/> are dispatched to the client through the tooltip JavaScript module, and the tooltip content is refreshed through <c>UpdatedTooltipContentAsync</c>. The pending-changes buffer is always cleared at the end of the call so that the next render pass starts from a clean state.
        /// </remarks>
        /// <exclude />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            if (firstRender && Created.HasDelegate)
            {
                await Created.InvokeAsync(null).ConfigureAwait(true);
            }

            if (!_isDestroyed
                && _isScriptRendered
                && _pendingPropertyChanges is { Count: > 0 }
                && await IsTooltipJsAvailableAsync().ConfigureAwait(true))
            {
                await InvokeVoidAsync(_tooltipJsModule, _tooltipInProcessModule, UPDATEPROPERTIES, _dataId, GetPropertyChanges()).ConfigureAwait(true);
            }

            _pendingPropertyChanges = null;
            await UpdatedTooltipContentAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Determines whether the component should re-render.
        /// </summary>
        /// <returns>True if the component should re-render; otherwise, false.</returns>
        /// <remarks>
        /// This method controls the rendering behavior of the SfTooltip component by:
        /// <list type="bullet">
        /// <item><description>Reading the current shouldRender flag value</description></item>
        /// <item><description>Resetting the shouldRender flag to true for the next render cycle</description></item>
        /// <item><description>Returning the previous flag value to determine if rendering should occur</description></item>
        /// </list>
        /// This mechanism allows for fine-grained control over when the component updates, optimizing performance
        /// by preventing unnecessary re-renders when component state hasn't meaningfully changed.
        /// </remarks>
        /// <exclude />
        protected override bool ShouldRender()
        {
            bool tmp = _shouldRender;
            _shouldRender = true;
            return tmp;
        }

        #endregion

        #region Internal Protected Methods

        /// <summary>
        /// Asynchronously executes logic after the JavaScript scripts for the component have been rendered.
        /// </summary>
        /// <returns>A task that represents the asynchronous script initialization operation.</returns>
        /// <remarks>
        /// This method is responsible for:
        /// <list type="bullet">
        /// <item><description>Setting the script rendered flag to enable property updates</description></item>
        /// <item><description>Wiring up JavaScript events and initializing the tooltip instance</description></item>
        /// <item><description>Passing component properties and event configurations to the JavaScript layer</description></item>
        /// </list>
        /// This method is called after all required scripts have been loaded and the component is ready for JavaScript interaction.
        /// </remarks>
        internal override async Task OnAfterScriptRenderedAsync()
        {
            // Guard: only invoke JS when the Tooltip module is actually loaded.
            // In minimal-JS mode the tooltip renders via C# alone and the JS
            // wireEvents call would throw an InvalidOperationException.
            if (!await IsTooltipJsAvailableAsync().ConfigureAwait(true))
            {
                return;
            }

            _isScriptRendered = true;
            await InvokeVoidAsync(_tooltipJsModule, _tooltipInProcessModule, WIREEVENTS, _dataId, _tooltipElement, DotnetObjectReference!, GetProperties(), GetEventsList()).ConfigureAwait(true);
        }

        internal override async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true);
            try
            {
                JsModuleReference tooltipJsModuleReference = await ImportModuleAsync(
                    "./_content/Syncfusion.Blazor.Toolkit/scripts/tooltip.js",
                    _tooltipJsModule,
                    _tooltipInProcessModule
                ).ConfigureAwait(true);
                _tooltipJsModule = tooltipJsModuleReference.AsyncRef;
                _tooltipInProcessModule = tooltipJsModuleReference.InProcessRef;

                JsModuleReference popupJsModuleReference = await ImportModuleAsync(
                    "./_content/Syncfusion.Blazor.Toolkit/scripts/popup.js",
                    _popupJsModule,
                    _popupInProcessModule
                ).ConfigureAwait(true);
                _popupJsModule = popupJsModuleReference.AsyncRef;
                _popupInProcessModule = popupJsModuleReference.InProcessRef;

                if (IsAnimationEnabled())
                {
                    JsModuleReference animationJsModuleReference = await ImportModuleAsync(
                        "./_content/Syncfusion.Blazor.Toolkit/scripts/animation.js",
                        _animationJsModule,
                        _animationInProcessModule
                    ).ConfigureAwait(true);
                    _animationJsModule = animationJsModuleReference.AsyncRef;
                    _animationInProcessModule = animationJsModuleReference.InProcessRef;
                }
            }
            catch (JSDisconnectedException)
            {
                // Circuit disconnected during module import (e.g. page refresh or navigation). Safe to ignore.
            }
            catch (JSException ex) when (ex.Message?.Contains("Failed to fetch", StringComparison.OrdinalIgnoreCase) ?? false)
            {
                // The tooltip.js module could not be fetched — typically caused by a stale static asset
                // manifest after the file was modified. A full rebuild and server restart is required.
                if (Logger != null)
                {
                    _tooltipJsModuleFetchFailed(Logger, ex);
                }
            }
        }

        /// <summary>
        /// Returns true when the tooltip JavaScript module is loaded
        /// and available for interop calls; false otherwise.
        /// </summary>
        private Task<bool> IsTooltipJsAvailableAsync()
        {
            return Task.FromResult(_tooltipJsModule != null || _tooltipInProcessModule != null);
        }

        /// <summary>
        /// Update the dictionary based on the @attributes key value check.
        /// <param name="classList">class list to be added in the string format.</param>
        /// <param name="dictionary">@attribute property value for updating class list.</param>
        /// <returns>Returns Dictionary.</returns>
        /// </summary>
        private static Dictionary<string, object> GetAttributes(string classList, Dictionary<string, object> dictionary)
        {
            if (!dictionary.TryAdd("class", classList))
            {
                dictionary["class"] = SfBaseUtils.AddClass(classList, dictionary["class"].ToString());
            }

            return dictionary;
        }

        #endregion
    }
}