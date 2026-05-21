using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public partial class SfUploader
    {
        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await base.OnInitializedAsync().ConfigureAwait(true);
                PropertyInitialized();
                PreRender();
                Render();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unhandled exception occurred.", ex);
            }
        }

        /// <summary>
        /// Triggers while dynamically updating the component properties.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                await base.OnParametersSetAsync().ConfigureAwait(true);
                PropertyParametersSet();
                UpdateBrowsBtn();
                if (IsPropertyChanged() && IsRendered)
                {
                    await HandlePropertyChangesAsync().ConfigureAwait(true);
                }

                UpdateAttributesAndInputs();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unhandled exception occurred.", ex);
            }
        }

        /// <summary>
        /// Triggers after the component was rendered.
        /// </summary>
        /// <param name="firstRender">true if the component rendered for the firts time.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
                if (firstRender)
                {
                    if (Created.HasDelegate)
                    {
                        await Created.InvokeAsync(null).ConfigureAwait(true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unhandled exception occurred.", ex);
            }
        }

        internal override async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true); // import base JS

            JsModuleReference uploaderJsModuleReference = await ImportModuleAsync(
                "./_content/Syncfusion.Blazor.Toolkit/scripts/uploader.js",
                _uploaderJsModule,
                _uploaderJsInProcessModule
            ).ConfigureAwait(true);
            _uploaderJsModule = uploaderJsModuleReference.AsyncRef;
            _uploaderJsInProcessModule = uploaderJsModuleReference.InProcessRef;

            JsModuleReference ajaxJsModuleReference = await ImportModuleAsync(
                "./_content/Syncfusion.Blazor.Toolkit/scripts/ajax.js",
                _ajaxJsModule,
                _ajaxJsInProcessModule
            ).ConfigureAwait(true);
            _ajaxJsModule = ajaxJsModuleReference.AsyncRef;
            _ajaxJsInProcessModule = ajaxJsModuleReference.InProcessRef;

            if (EnableHtmlSanitizer)
            {
                JsModuleReference sanitizeJsModuleReference = await ImportModuleAsync(
                    "./_content/Syncfusion.Blazor.Toolkit/scripts/sanitize-html-helper.js",
                    _sanitizeJsModule,
                    _sanitizeJsInProcessModule
                ).ConfigureAwait(true);
                _sanitizeJsModule = sanitizeJsModuleReference.AsyncRef;
                _sanitizeJsInProcessModule = sanitizeJsModuleReference.InProcessRef;
            }

            if (UploadAsyncSettings != null && !string.IsNullOrEmpty(UploadAsyncSettings.SaveUrl) && !string.IsNullOrEmpty(UploadAsyncSettings.RemoveUrl))
            {
                await LoadAnimationScriptAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Disposes the uploader component.
        /// </summary>
        /// <exclude/>
        protected override async ValueTask DisposeAsyncCore()
        {
            try
            {
                if (IsRendered)
                {
                    await InvokeVoidAsync(_uploaderJsModule, _uploaderJsInProcessModule, "destroy", [DataId]).ConfigureAwait(true);
                }
                FileSemaphore?.Dispose();
                try
                {
                    if (_uploaderJsModule != null)
                    {
                        await _uploaderJsModule.DisposeAsync().ConfigureAwait(true);
                    }
                    _uploaderJsInProcessModule?.Dispose();
                    if (_sanitizeJsModule != null)
                    {
                        await _sanitizeJsModule.DisposeAsync().ConfigureAwait(true);
                    }
                    _sanitizeJsInProcessModule?.Dispose();

                    if (_ajaxJsModule != null)
                    {
                        await _ajaxJsModule.DisposeAsync().ConfigureAwait(true);
                    }
                    _ajaxJsInProcessModule?.Dispose();
                }
                catch (JSDisconnectedException)
                {
                    // Ignore: The circuit disconnected (e.g., page reload) before JS disposal could complete.
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unhandled exception occurred.", ex);
            }
            await base.DisposeAsyncCore().ConfigureAwait(true);
        }
    }
}
