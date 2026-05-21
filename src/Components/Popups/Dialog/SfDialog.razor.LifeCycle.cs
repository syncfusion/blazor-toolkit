using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    public partial class SfDialog : SfBaseComponent
    {
        /// <summary>
        /// This method is invoked when the component is ready to start, and it initializes the dialog's properties and state.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous initialization process.</returns>
        /// <remarks>
        /// This lifecycle method is responsible for setting up the initial state of the <see cref="SfDialog"/>. It assigns a unique ID if one is not provided, 
        /// configures CSS classes, and initializes various properties like <see cref="AllowDragging"/>, <see cref="EnableResize"/>, and <see cref="IsModal"/>. 
        /// It also prepares the component for rendering, especially in server-side or prerendering scenarios.
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
        /// This method is invoked when the component has received parameters from its parent, and it handles property updates.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation of setting parameters.</returns>
        /// <remarks>
        /// This method tracks changes in component parameters and updates the dialog's state accordingly. If any properties have changed, it calls the <c>ClientPropertyChangeHandler</c> 
        /// to apply these updates on the client side, ensuring that the dialog's appearance and behavior reflect the latest parameter values.
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
        /// This method is invoked after the component has been rendered, and it handles post-rendering logic, such as showing the dialog.
        /// </summary>
        /// <param name="firstRender">A <c>bool</c> that is <c>true</c> if this is the first time the component has been rendered; otherwise, <c>false</c>.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous post-rendering operation.</returns>
        /// <remarks>
        /// This method is responsible for handling tasks that must occur after the component's UI has been updated. It manages visibility changes and ensures that the dialog is correctly 
        /// initialized and displayed on the client side, particularly in scenarios involving prerendering.
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
        /// This method is invoked after the JavaScript modules for the component have been rendered, and it finalizes the dialog's initialization on the client side.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation of finalizing the component's client-side initialization.</returns>
        /// <remarks>
        /// This method handles the final setup of the dialog after all necessary JavaScript has been loaded. It manages persistence by loading the dialog's state from local storage 
        /// if <see cref="EnablePersistence"/> is enabled. It also invokes the <c>Created</c> event and shows the dialog if it is set to be visible initially.
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
        /// This method is called when the component is being disposed, and it handles the cleanup of the dialog's resources.
        /// </summary>
        /// <remarks>
        /// This lifecycle method is responsible for cleaning up all resources used by the <see cref="SfDialog"/>. It destroys the client-side dialog instance, invokes the <c>Destroyed</c> event, 
        /// and disposes of all related objects to prevent memory leaks. It also includes error handling to manage exceptions that may occur during the disposal process.
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
