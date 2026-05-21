using Microsoft.JSInterop;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Partial lifecycle implementation for the <c>SfSwitch&lt;TChecked&gt;</c> component.
    /// Initializes CSS classes, manages script initialization, and disposes interop resources.
    /// </summary>
    public partial class SfSwitch<TChecked>
    {

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Called after each render; on first render, reapplies persisted state if enabled.
        /// </summary>
        /// <param name="firstRender">True if this is the initial render.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
            if (firstRender && EnablePersistence)
            {
                bool isChecked = Convert.ToBoolean(Checked, CultureInfo.InvariantCulture);
                ChangeState(isChecked ? Check : UnCheck);
            }
        }

        /// <summary>
        /// Imports the Switch component ES module for JS isolation.
        /// </summary>
        internal override async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true);

            JsModuleReference switchJsModuleReference = await ImportModuleAsync("./_content/Syncfusion.Blazor.Toolkit/scripts/switch.js", _componentJsModule, _componentsInProcessModule).ConfigureAwait(true);
            _componentJsModule = switchJsModuleReference.AsyncRef;
            _componentsInProcessModule = switchJsModuleReference.InProcessRef;
        }

        /// <summary>
        /// Invoked after required scripts are available; sets up switch interop.
        /// </summary>
        internal override async Task OnAfterScriptRenderedAsync()
        {
            await base.OnAfterScriptRenderedAsync().ConfigureAwait(false);
            try
            {
                await InvokeVoidAsync(_componentJsModule, _componentsInProcessModule, "initialize", _input, _container).ConfigureAwait(true);
            }
            catch (OperationCanceledException ex)
            {
                _logErrorInitializeSwitch(Logger, ex.Message, ex);
            }
            catch (InvalidOperationException ex)
            {
                _logErrorInitializeSwitch(Logger, ex.Message, ex);
            }
        }

        /// <exclude />
        /// <summary>
        /// Disposes JS resources and clears references when the component is removed from the UI.
        /// This method is called by the framework to allow the component to release unmanaged
        /// or JS interop resources and to break strong references that could prevent garbage collection.
        /// </summary>
        protected override async ValueTask DisposeAsyncCore()
        {
            try
            {
                if (IsRendered)
                {
                    await InvokeVoidAsync(_componentJsModule, _componentsInProcessModule, "destroy", _input).ConfigureAwait(false);
                }
            }
            catch (InvalidOperationException ex)
            {
                _logErrorDestroySwitch(Logger, ex.Message, ex);
            }
            catch (OperationCanceledException ex)
            {
                _logErrorDestroySwitch(Logger, ex.Message, ex);
            }

            try
            {
                if (_componentJsModule != null)
                {
                    await _componentJsModule.DisposeAsync().ConfigureAwait(true);
                }

                _componentsInProcessModule?.Dispose();
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