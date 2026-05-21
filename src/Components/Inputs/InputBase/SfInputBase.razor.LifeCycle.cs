using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public abstract partial class SfInputBase<TValue>
    {
        #region Lifecycle methods

        /// <summary>
        /// Asynchronously initializes the component during the initial rendering phase.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous initialization operation that completes when the component is fully initialized.</returns>
        /// <remarks>
        /// <para>This method is called once when the component is first initialized and performs the following operations:</para>
        /// <list type="bullet">
        /// <item><description>Sets up the default script modules if no specific component reference is provided</description></item>
        /// <item><description>Initializes the icon position for component elements</description></item>
        /// <item><description>Configures the initial clear button state</description></item>
        /// <item><description>Calls the base class initialization logic</description></item>
        /// </list>
        /// </remarks>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(ComponentReference))
            {
                await base.OnInitializedAsync().ConfigureAwait(true);
                ClearIconClass = CLEARICON;
            }
            else
            {
                await base.OnInitializedAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Asynchronously processes parameter changes and updates the component state accordingly.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous parameter processing operation that completes when the component state is updated.</returns>
        /// <remarks>
        /// <para>This method is called whenever the component's parameters change and performs the following operations:</para>
        /// <list type="bullet">
        /// <item><description>Calls the base class parameter processing logic</description></item>
        /// <item><description>Triggers pre-render processing if no specific component reference is set</description></item>
        /// <item><description>Updates internal state based on the new parameter values</description></item>
        /// </list>
        /// </remarks>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            if (string.IsNullOrEmpty(ComponentReference))
            {
                PreRender();
            }
        }

        /// <summary>
        /// Asynchronously performs operations after the component has been rendered to the DOM.
        /// </summary>
        /// <param name="firstRender">A <see cref="bool"/> value indicating whether this is the first time the component is being rendered.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous post-render operation that completes when all post-render initialization is finished.</returns>
        /// <remarks>
        /// <para>This method is called after each render cycle and performs the following operations:</para>
        /// <list type="bullet">
        /// <item><description>Calls the base class post-render logic</description></item>
        /// <item><description>On first render, initializes JavaScript functionality for button groups if present</description></item>
        /// <item><description>Ensures proper DOM integration and event binding</description></item>
        /// </list>
        /// </remarks>
        /// <exclude/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            if (ListOfButtons is not null && ListOfButtons.Count > 0 && firstRender)
            {
                await OnAfterScriptRenderedAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Imports the JavaScript module required for the Input components asynchronously.
        /// </summary>
        /// <remarks>Depending on the execution context, either an in-process or standard JavaScript
        /// module is imported.</remarks>
        /// <returns>A task that represents the asynchronous import operation.</returns>
        internal override async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true);

            JsModuleReference textBoxJsModuleReference = await ImportModuleAsync(
                "./_content/Syncfusion.Blazor.Toolkit/scripts/textbox.js",
                _textBoxJsModule,
                _textBoxJsInProcessModule
            ).ConfigureAwait(true);
            _textBoxJsModule = textBoxJsModuleReference.AsyncRef;
            _textBoxJsInProcessModule = textBoxJsModuleReference.InProcessRef;
        }

        /// <summary>
        /// Initializes JavaScript functionality after the component's scripts have been rendered and loaded.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous script initialization operation that completes when JavaScript interop is configured.</returns>
        /// <remarks>
        /// <para>This method is called after the component's JavaScript modules are loaded and performs:</para>
        /// <list type="bullet">
        /// <item><description>Calls the base class script initialization</description></item>
        /// <item><description>Initializes client-side TextBox functionality through JavaScript interop</description></item>
        /// <item><description>Sets up DOM event handling and component behavior coordination</description></item>
        /// </list>
        /// </remarks>
        internal override async Task OnAfterScriptRenderedAsync()
        {
            await base.OnAfterScriptRenderedAsync().ConfigureAwait(true);
            if (_textBoxJsModule is not null || _textBoxJsInProcessModule is not null)
            {
                await InvokeVoidAsync(_textBoxJsModule!, _textBoxJsInProcessModule!, "initialize", InputElement, DotnetObjectReference!, ContainerElement).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Releases resources and performs cleanup operations when the component is being disposed.
        /// </summary>
        /// <remarks>
        /// <para>This method is called during component disposal to clean up JavaScript interop resources and release managed resources.</para>
        /// <para>It checks if the component has been rendered before invoking the JavaScript destroy method and clearing references to ensure proper cleanup of event listeners, DOM references, and other resources, preventing memory leaks.</para>
        /// </remarks>
        /// <exclude/>
        protected override async ValueTask DisposeAsyncCore()
        {
            if (IsRendered)
            {
                InputHtmlAttributes = default!;
                BaseInputAttributes = null;
                ContainerAttributes = null;
                InputEditContext = null;
            }
            try
            {
                if (_textBoxJsModule is not null)
                {
                    await _textBoxJsModule.DisposeAsync().ConfigureAwait(true);
                }
                _textBoxJsInProcessModule?.Dispose();
            }
            catch (JSDisconnectedException)
            {
                // Ignore: The circuit disconnected (e.g., page reload) before JS disposal could complete.
            }
            await base.DisposeAsyncCore().ConfigureAwait(true);
        }
        #endregion
    }
}
