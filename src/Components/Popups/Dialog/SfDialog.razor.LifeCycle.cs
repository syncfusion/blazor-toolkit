using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    public partial class SfDialog : SfBaseComponent
    {
        /// <summary>
        /// Initializes the dialog component, configuring CSS classes, generating a unique ID if needed, and setting up initial state from parameters.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous initialization process.</returns>
        /// <remarks>
        /// During initialization, this method sets up drag-and-drop, resizing, and modal behavior based on the component's parameters.
        /// It also handles prerendering state by setting <c>IsPreRender</c> when applicable.
        /// </remarks>
        /// <exclude />
        protected override async Task OnInitializedAsync()
        {
            try
            {
                _dialogClass = IsStaticRendering ? "e-dialog e-lib" : _dialogClass;
                _dataId = ID = string.IsNullOrEmpty(ID) ? SfBaseUtils.GenerateID(DIALOG) : ID;
                _previousCssClass = CssClass;
                _previousVisible = Visible;
                _cssClass = CssClass;
                UpdateLocalProperties();
                UpdateLocale();
                await base.OnInitializedAsync().ConfigureAwait(false);
                _allowDragging = AllowDragging;
                _closeOnEscape = CloseOnEscape;
                _enableResize = EnableResize;
                _height = Height;
                _isModal = IsModal;
                _minHeight = MinHeight;
                _target = Target;
                _visible = Visible;
                _width = Width;
                _zIndex = ZIndex;
                IsPreRender = !AllowPrerender && Visible;
            }
            catch (NullReferenceException ex)
            {
                LogError("OnInitializedAsync", ex);
            }
        }

        /// <summary>
        /// Tracks parameter changes and propagates updates to the client when properties change.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation of processing parameter updates.</returns>
        /// <remarks>
        /// When properties change, this method uses <c>ClientPropertyChangeHandlerAsync</c> to apply updates client-side, ensuring the dialog reflects the latest parameter values.
        /// </remarks>
        /// <exclude />
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                await base.OnParametersSetAsync().ConfigureAwait(false);
                _allowDragging = NotifyPropertyChanges(ALLOW_DRAGGING, AllowDragging, _allowDragging);
                _closeOnEscape = NotifyPropertyChanges(CLOSE_ON_ESCAPE, CloseOnEscape, _closeOnEscape);
                _cssClass = NotifyPropertyChanges(CSSCLASS, CssClass, _cssClass);
                _enableResize = NotifyPropertyChanges(ENABLE_RESIZE, EnableResize, _enableResize);
                _height = NotifyPropertyChanges(HEIGHT, Height, _height);
                _isModal = NotifyPropertyChanges(ISMODAL, IsModal, _isModal);
                _minHeight = NotifyPropertyChanges(MIN_HEIGHT, MinHeight, _minHeight);
                _target = NotifyPropertyChanges(TARGET, Target, _target);
                _visible = await SfBaseUtils.UpdatePropertyAsync(Visible, _visible, VisibleChanged).ConfigureAwait(false);
                _width = NotifyPropertyChanges(WIDTH, Width, _width);
                _zIndex = NotifyPropertyChanges(ZINDEX, ZIndex, _zIndex);
                if (!AllowPrerender && Visible && !_preventVisibility)
                {
                    IsPreRender = true;
                }
                if (PropertyChanges is not null && PropertyChanges.Count > 0)
                {
                    List<string> changedKeys = [.. PropertyChanges.Keys];
                    await ClientPropertyChangeHandlerAsync(changedKeys).ConfigureAwait(false);
                }
            }
            catch (NullReferenceException ex)
            {
                LogError("OnParametersSetAsync", ex);
            }
        }

        /// <summary>
        /// Handles post-render tasks including visibility state changes, client-side property synchronization, and deferred dialog initialization.
        /// </summary>
        /// <param name="firstRender">A <c>bool</c> that is <c>true</c> if this is the first time the component has been rendered; otherwise, <c>false</c>.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous post-rendering operation.</returns>
        /// <remarks>
        /// On first render, this method may initialize and display the dialog when <c>IsPreRender</c> is set and <c>AllowPrerender</c> is disabled.
        /// It also syncs the dialog's visibility state with the server when the <see cref="Visible"/> property changes.
        /// </remarks>
        /// <exclude />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (IsRendered && _previousVisible != Visible)
                {
                    await ServerPropertyChangeHandlerAsync().ConfigureAwait(false);
                }
                if (!AllowPrerender && IsPreRender && _isShowCall && !DialogShown)
                {
                    _isShowCall = false;
                    await InvokeVoidAsync(_dialogJsModule, _dialogJsInProcessModule, JS_INITIALIZE, GetInstance(false)).ConfigureAwait(true);
                    await ShowDialogAsync().ConfigureAwait(false);
                }
                await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LogError("OnAfterRenderAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Loads persisted dialog state from local storage if <see cref="EnablePersistence"/> is enabled, then initializes the JavaScript interop modules and displays the dialog if visible.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation of finalizing client-side initialization.</returns>
        /// <remarks>
        /// This method handles the final setup after JavaScript modules have loaded. It invokes the <see cref="Created"/> event and shows the dialog if <see cref="Visible"/> is <c>true</c>.
        /// </remarks>
        internal override async Task OnAfterScriptRenderedAsync()
        {
            try
            {
                if (EnablePersistence)
                {
                    string localStorageValue = await InvokeAsync<string>(_baseJsModule!, _baseJsInProcessModule!, JS_WINDOW_LOCAL_STORAGE_GET_ITEM, [ID]).ConfigureAwait(true);
                    if (localStorageValue is not null)
                    {
                        Dictionary<string, object>? localValue = JsonSerializer.Deserialize<Dictionary<string, object>>(localStorageValue);
                        Dictionary<string, object>? updatedInstance = GetInstance(true);
                        string? X = localValue is not null && localValue.TryGetValue("X", out object? xVal) ? xVal?.ToString() ?? string.Empty : string.Empty;
                        string? Y = localValue is not null && localValue.TryGetValue("Y", out object? yVal) ? yVal?.ToString() ?? string.Empty : string.Empty;
                        string? width = localValue is not null && localValue.TryGetValue("width", out object? widthVal) ? widthVal?.ToString() ?? string.Empty : string.Empty;
                        string? height = localValue is not null && localValue.TryGetValue("height", out object? heightVal) ? heightVal?.ToString() ?? string.Empty : string.Empty;
                        updatedInstance["position"] = new Dictionary<string, string> { { "X", X }, { "Y", Y } };
                        if (localValue is not null && localValue.TryGetValue("width", out object? widthResult))
                        {
                            updatedInstance["width"] = width;
                        }
                        if (localValue is not null && localValue.TryGetValue("height", out object? heightResult))
                        {
                            updatedInstance["height"] = height;
                        }
                        await InvokeVoidAsync(_dialogJsModule!, _dialogJsInProcessModule!, JS_INITIALIZE, updatedInstance).ConfigureAwait(true);
                    }
                    else
                    {
                        await InvokeVoidAsync(_dialogJsModule!, _dialogJsInProcessModule!, JS_INITIALIZE, GetInstance(true)).ConfigureAwait(true);
                    }
                }
                else
                {
                    await InvokeVoidAsync(_dialogJsModule!, _dialogJsInProcessModule!, JS_INITIALIZE, GetInstance(true)).ConfigureAwait(true);
                }
                if (Created.HasDelegate)
                {
                    await Created.InvokeAsync(new { Name = CREATED }).ConfigureAwait(false);
                }
                if (Visible)
                {
                    await ShowDialogAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                LogError("OnAfterScriptRenderedAsync", ex);
                throw;
            }
        }

        internal override async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true);

            if (IsAnimationEnabled())
            {
                // Load animation-related script before datepicker scripts
                await LoadAnimationScriptAsync().ConfigureAwait(true);
            }

            // Load popup-related script before datepicker scripts
            await LoadPopupScriptAsync().ConfigureAwait(true);

            JsModuleReference dialogJsModuleReference = await ImportModuleAsync(
                "./_content/Syncfusion.Blazor.Toolkit/scripts/dialog.js",
                _dialogJsModule,
                _dialogJsInProcessModule
            ).ConfigureAwait(true);
            _dialogJsModule = dialogJsModuleReference.AsyncRef;
            _dialogJsInProcessModule = dialogJsModuleReference.InProcessRef;

            if (AllowDragging)
            {
                JsModuleReference draggableJsModuleReference = await ImportModuleAsync(
                    "./_content/Syncfusion.Blazor.Toolkit/scripts/draggable.js",
                    _draggableJsModule,
                    _draggableJsInModule
                ).ConfigureAwait(true);
                _draggableJsModule = draggableJsModuleReference.AsyncRef;
                _draggableJsInModule = draggableJsModuleReference.InProcessRef;
            }

            if (EnableResize)
            {
                JsModuleReference resizeJsModuleReference = await ImportModuleAsync(
                    "./_content/Syncfusion.Blazor.Toolkit/scripts/resize.js",
                    _resizeJsModule,
                    _resizeJsInModule
                ).ConfigureAwait(true);
                _resizeJsModule = resizeJsModuleReference.AsyncRef;
                _resizeJsInModule = resizeJsModuleReference.InProcessRef;
            }
        }

        private static readonly Action<ILogger, string, Exception?> _logError =
            LoggerMessage.Define<string>(LogLevel.Error, new EventId(0, "Dialog"), "Error in {MethodName}");

        internal void LogError(string methodName, Exception ex)
        {
            if (Logger is not null)
            {
                _logError(Logger, methodName, ex);
            }
        }

        /// <summary>
        /// Cleans up dialog resources by destroying the client-side instance, invoking the <see cref="Destroyed"/> event, and disposing JavaScript modules and render fragments.
        /// </summary>
        /// <remarks>
        /// This method safely handles <see cref="JSDisconnectedException"/> when the circuit disconnects before disposal completes. It clears all internal state including templates and event handlers.
        /// </remarks>
        /// <exclude />
        protected override async ValueTask DisposeAsyncCore()
        {
            try
            {
                if (IsRendered)
                {
                    await InvokeVoidAsync(_dialogJsModule!, _dialogJsInProcessModule!, JS_DESTROY, new Dictionary<string, object> {
                            { "dataId", _dataId },
                            { DICTIONARY_CSSCLASS, CssClass },
                            { "isClient", JSRuntime is IJSInProcessRuntime },
                        }).ConfigureAwait(true);
                    if (Destroyed.HasDelegate)
                    {
                        await Destroyed.InvokeAsync(new { Name = DESTROYED }).ConfigureAwait(false);
                    }
                    try
                    {
                        if (_dialogJsModule is not null)
                        {
                            await _dialogJsModule.DisposeAsync().ConfigureAwait(false);
                        }
                        if (_draggableJsModule is not null)
                        {
                            await _draggableJsModule.DisposeAsync().ConfigureAwait(false);
                        }
                        if (_resizeJsModule is not null)
                        {
                            await _resizeJsModule.DisposeAsync().ConfigureAwait(false);
                        }
                        _dialogJsInProcessModule?.Dispose();
                        _draggableJsInModule?.Dispose();
                        _resizeJsInModule?.Dispose();
                    }
                    catch (JSDisconnectedException)
                    {
                        // Ignore: The circuit disconnected (e.g., page reload) before JS disposal could complete.
                    }
                }
                ChildContent = default!;
                AnimationSettingsValue = default!;
                ResizeHandles = [ResizeDirection.SouthEast];
                PositionValue = null;
                _onClosedArgs = new BeforeCloseEventArgs();
                ButtonsValue?.Clear();
                ButtonsValue = null;
                HeaderTemplate = null;
                ContentTemplate = null;
                FooterTemplates = null;
                DialogElement = default;
                ModalDialogElement = default;
                _dialogAttribute?.Clear();
                _dialogAttribute = [];
                CloseIconAttributes?.Clear();
                CloseIconAttributes = null;
            }
            catch (ObjectDisposedException ex)
            {
                LogError("DisposeAsyncCore", ex);
            }
            catch (TaskCanceledException ex)
            {
                LogError("DisposeAsyncCore", ex);
            }
            catch (InvalidOperationException ex)
            {
                LogError("DisposeAsyncCore", ex);
            }
            catch (JSDisconnectedException ex)
            {
                LogError("DisposeAsyncCore", ex);
            }
            catch (OperationCanceledException ex)
            {
                LogError("DisposeAsyncCore", ex);
            }
            await base.DisposeAsyncCore().ConfigureAwait(true);
        }
    }
}
