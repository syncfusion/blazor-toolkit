using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public partial class SfCheckBox<TChecked>
    {
        #region LifeCycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component state when the component is first created.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            if (string.IsNullOrEmpty(_idValue) || (_inputAttributes is not null && _inputAttributes.ContainsKey("id")))
            {
                _idValue = "sfcheckbox" + "-" + Guid.NewGuid().ToString();
            }
        }

        /// <exclude />
        /// <summary>
        /// Invokes the base implementation after each render; the checkbox requires no component-specific post-render work.
        /// </summary>
        /// <param name="firstRender"><see langword="true"/> when this is the first time the component has rendered; otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// The <see cref="SfSelectionBase{TChecked}"/> base owns first-render JavaScript interop, persistence
        /// restoration, and visual state recomputation.
        /// </remarks>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
            await InvokeVoidAsync(_checkBoxJsModule, _checkBoxInProcessModule, "syncIndeterminate", _input, Indeterminate).ConfigureAwait(true);
        }

        /// <summary>
        /// Invoked after required scripts are available to initialize JavaScript interop for the checkbox.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method sets up event handlers and UI interactions through JavaScript interop.
        /// </remarks>
        internal override async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true);

            JsModuleReference checkBoxJsModuleReference = await ImportModuleAsync("./_content/Syncfusion.Blazor.Toolkit/scripts/checkbox.js", _checkBoxJsModule, _checkBoxInProcessModule).ConfigureAwait(true);
            _checkBoxJsModule = checkBoxJsModuleReference.AsyncRef;
            _checkBoxInProcessModule = checkBoxJsModuleReference.InProcessRef;
        }

        /// <summary>
        /// Invoked after required scripts are available to initialize JavaScript interop for the checkbox.
        /// </summary>
        internal override async Task OnAfterScriptRenderedAsync()
        {
            await base.OnAfterScriptRenderedAsync().ConfigureAwait(true);
            try
            {
                await InvokeVoidAsync(_checkBoxJsModule, _checkBoxInProcessModule, "initialize", _input, _container).ConfigureAwait(true);
            }
            catch (Exception ex) when (Logger is not null)
            {
                // Logger is wired up: capture the exception for diagnostics and prevent the failure
                // from surfacing to Blazor's error boundary (which would tear down the circuit).
                _logErrorInitializingCheckBoxInterop(Logger, ex);
            }
            catch (Exception)
            {
                // No logger is configured: rethrow so the developer is alerted to the failure.
                throw;
            }
        }

        /// <exclude />
        /// <summary>
        /// Disposes component resources and cleans up JavaScript interop handlers.
        /// </summary>
        /// <remarks>
        /// This method is called when the component is removed from the render tree. It ensures
        /// proper cleanup to prevent memory leaks by destroying JavaScript event handlers.
        /// JavaScript interop operations are initiated as fire-and-forget tasks to avoid blocking disposal.
        /// </remarks>
        protected override async ValueTask DisposeAsyncCore()
        {
            if (IsRendered)
            {
                try
                {
                    await InvokeVoidAsync(_checkBoxJsModule, _checkBoxInProcessModule, "destroy", _input).ConfigureAwait(false);
                }
                catch (Exception ex) when (Logger is not null)
                {
                    // Logger is wired up: capture the exception for diagnostics and prevent the failure
                    // from surfacing to Blazor's error boundary (which would tear down the circuit).
                    _logErrorDestroyingCheckBoxInterop(Logger, ex);
                }
                catch (Exception)
                {
                    // No logger is configured: rethrow so the developer is alerted to the failure.
                    throw;
                }
            }
            try
            {
                if (_checkBoxJsModule is not null)
                {
                    await _checkBoxJsModule.DisposeAsync().ConfigureAwait(true);
                }
                _checkBoxInProcessModule?.Dispose();
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
