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
        /// Asynchronously handles parameter changes in the SfTooltip component.
        /// </summary>
        /// <returns>A task that represents the asynchronous parameter update operation.</returns>
        /// <remarks>
        /// This method is called whenever component parameters are updated. It performs the following operations:
        /// <list type="bullet">
        /// <item><description>Compares new parameter values with existing values</description></item>
        /// <item><description>Tracks property changes for efficient updates</description></item>
        /// <item><description>Updates the JavaScript component if the component is already rendered</description></item>
        /// <item><description>Ensures proper synchronization between Blazor properties and JavaScript tooltip instance</description></item>
        /// </list>
        /// The method uses the NotifyPropertyChanges mechanism to optimize updates by only processing changed properties.
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
            if (PropertyChanges!.Count > 0 && _isScriptRendered && !_isDestroyed)
            {
                // Skip JS update when the module is not loaded.
                if (await IsTooltipJsAvailableAsync().ConfigureAwait(true))
                {
                    await InvokeVoidAsync(_tooltipJsModule, _tooltipInProcessModule, UPDATEPROPERTIES, _dataId, GetPropertyChanges()).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Asynchronously executes logic after the component has rendered.
        /// </summary>
        /// <param name="firstRender">True if this is the first time the component has rendered; otherwise, false.</param>
        /// <returns>A task that represents the asynchronous post-render operation.</returns>
        /// <remarks>
        /// This method handles post-render operations including:
        /// <list type="bullet">
        /// <item><description>Invoking the Created event callback on first render</description></item>
        /// <item><description>Updating tooltip content after rendering</description></item>
        /// <item><description>Ensuring proper synchronization between the rendered DOM and component state</description></item>
        /// </list>
        /// The firstRender parameter is used to determine if initialization-specific logic should be executed.
        /// </remarks>
        /// <exclude />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            if (firstRender && Created.HasDelegate)
            {
                await Created.InvokeAsync(null).ConfigureAwait(true);
            }

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