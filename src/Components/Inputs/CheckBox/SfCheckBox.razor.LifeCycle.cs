using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Globalization;

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
        /// Executes after each render to apply persisted state and initialize JavaScript interop.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the component's first render.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// On first render, if persistence is enabled, the component restores its state from local storage
        /// and updates the visual display accordingly.
        /// </remarks>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
            if (firstRender && EnablePersistence)
            {
                try
                {
                    bool isChecked = Convert.ToBoolean(Checked, CultureInfo.InvariantCulture);
                    UpdateVisualState(isChecked ? CheckboxState.Checked : CheckboxState.Unchecked);
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, "Error applying persisted checked state in OnAfterRenderAsync.");
                }
            }
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
                await InvokeVoidAsync(_checkBoxJsModule!, _checkBoxInProcessModule!, "initialize", _input, _container).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error initializing CheckBox interop in OnAfterScriptRendered.");
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
                catch (Exception ex)
                {
                    Logger?.LogError(ex, "Error destroying CheckBox interop in DisposeAsyncCore.");
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
