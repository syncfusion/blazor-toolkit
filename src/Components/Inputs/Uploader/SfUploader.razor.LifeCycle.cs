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
                if (firstRender && Created.HasDelegate)
                {
                    await Created.InvokeAsync(null).ConfigureAwait(true);
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
        /// Asynchronously releases the uploader-specific resources when the component is removed
        /// from the render tree.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> that completes after disposal work is finished.</returns>
        /// <exception cref="InvalidOperationException">Thrown when an unhandled exception occurs while disposing uploader resources (file semaphore, JS modules, or destroy interop).</exception>
        /// <remarks>
        /// <para>
        /// This override owns disposal of the file semaphore, the uploader-specific JavaScript
        /// modules (<c>uploader.js</c>, <c>ajax.js</c>, and the optional
        /// <c>sanitize-html-helper.js</c>), and the client-side <c>destroy</c> interop call when
        /// the component has been rendered. The base <see cref="SfBaseComponent.DisposeAsyncCore"/>
        /// handles the shared <c>base.js</c>, <c>animation.js</c>, <c>popup.js</c>, and
        /// <c>touch.js</c> modules plus the <see cref="DotNetObjectReference{T}"/> bridge.
        /// </para>
        /// <para>
        /// <see cref="JSDisconnectedException"/> and <see cref="ObjectDisposedException"/> are
        /// caught and ignored during teardown because the Blazor circuit may disconnect (for
        /// example, after a page reload) or a module reference may have already been released
        /// before the asynchronous disposal finishes.
        /// </para>
        /// </remarks>
        /// <exclude/>
        protected override async ValueTask DisposeAsyncCore()
        {
            try
            {
                if (IsRendered)
                {
                    try
                    {
                        await InvokeVoidAsync(_uploaderJsModule, _uploaderJsInProcessModule, "destroy", [DataId]).ConfigureAwait(true);
                    }
                    catch (JSDisconnectedException)
                    {
                        // Ignore: The circuit disconnected (e.g., page reload) before JS destroy could complete.
                    }
                    catch (ObjectDisposedException)
                    {
                        // Ignore: JS module reference was disposed before destroy could complete.
                    }
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
                catch (ObjectDisposedException)
                {
                    // Ignore: Module reference was already disposed.
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
