using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfSidebar
    {
        /// <summary>
        /// Takes the initial open state from <see cref="IsOpen"/> and computes the render state.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exclude />
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            _isOpen = IsOpen;
            _isOpenParameter = IsOpen;
            UpdateDerivedState();
        }

        /// <summary>
        /// Applies a changed <see cref="IsOpen"/> and recomputes the render state from the current parameters.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// <see cref="IsOpen"/> is compared with the last value received from the parent, not with the internal state, so a state that the
        /// user changed without two-way binding is not reset by an unrelated parent re-render. A parent-driven change raises no events.
        /// </remarks>
        /// <exclude />
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            if (_isOpenParameter != IsOpen)
            {
                _isOpenParameter = IsOpen;
                if (_isOpen != IsOpen)
                {
                    _isOpen = IsOpen;
                    Announce();
                }
            }

            UpdateDerivedState();
        }

        /// <summary>
        /// Reconciles the viewport, starts or ends the modal focus behavior, and keeps the legacy media-query and navigation watchers current.
        /// </summary>
        /// <param name="firstRender"><see langword="true"/> on the first render.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exclude />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            await SyncEnvironmentAsync().ConfigureAwait(true);
            await SyncModalAsync().ConfigureAwait(true);
            await SyncMediaQueryAsync().ConfigureAwait(true);
            SyncNavigationWatcher();
        }

        private async Task<IJSObjectReference?> GetModuleAsync()
        {
            if (IsDisposed || !IsRendered || JSRuntime is null)
            {
                return null;
            }

            try
            {
                _sidebarJsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", MEDIA_QUERY_SCRIPT).ConfigureAwait(true);
            }
            catch (JSException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }

            return _sidebarJsModule;
        }

        private async Task SyncEnvironmentAsync()
        {
            int threshold = Threshold;
            if (_observedThreshold == threshold)
            {
                return;
            }

            IJSObjectReference? module = await GetModuleAsync().ConfigureAwait(true);
            if (module is null || IsDisposed)
            {
                return;
            }

            bool first = _observedThreshold < 0;
            _observedThreshold = threshold;
            bool isMobile;
            bool environmentChanged = false;
            try
            {
                if (first)
                {
                    _callbacksReference = DotNetObjectReference.Create(new SidebarCallbacks(this));
                    SidebarEnvironment? environment = await module.InvokeAsync<SidebarEnvironment?>(
                        "observe",
                        _generatedId,
                        _callbacksReference,
                        _sidebarElement,
                        threshold).ConfigureAwait(true);
                    if (environment is null)
                    {
                        // A host that does not run the script, including the unit-test runtime, leaves the sidebar on its parameters.
                        return;
                    }

                    bool motionChanged = _reducedMotion != environment.ReducedMotion;
                    bool rtlChanged = _hostRtl != environment.IsRtl;
                    environmentChanged = motionChanged || rtlChanged;
                    _reducedMotion = environment.ReducedMotion;
                    _hostRtl = environment.IsRtl;
                    isMobile = environment.IsMobile;
                    // The initial environment can change RTL or reduced motion even when the viewport state is unchanged.
                    if (isMobile == _isMobile && !motionChanged && !rtlChanged)
                    {
                        return;
                    }
                }
                else
                {
                    isMobile = await module.InvokeAsync<bool>("update", _generatedId, threshold).ConfigureAwait(true);
                }
            }
            catch (JSException)
            {
                // A host without matchMedia, including the unit-test runtime, leaves the sidebar on its parameters.
                return;
            }
            catch (InvalidOperationException)
            {
                return;
            }

            if (isMobile == _isMobile)
            {
                if (environmentChanged)
                {
                    UpdateDerivedState();
                    StateHasChanged();
                }

                return;
            }

            if (threshold == 0)
            {
                _isMobile = false;
                UpdateDerivedState();
                StateHasChanged();
                return;
            }

            await ApplyViewportAsync(isMobile, raiseBreakpointChanged: false).ConfigureAwait(true);
        }

        private async Task SyncModalAsync()
        {
            if (IsDisposed || IsModal == _modalActive)
            {
                return;
            }

            IJSObjectReference? module = await GetModuleAsync().ConfigureAwait(true);
            if (module is null || IsDisposed)
            {
                return;
            }

            bool activate = IsModal;
            _modalActive = activate;
            try
            {
                if (activate)
                {
                    await module.InvokeVoidAsync("activateModal", _generatedId, _sidebarElement).ConfigureAwait(true);
                }
                else
                {
                    await module.InvokeVoidAsync("deactivateModal", _generatedId, true).ConfigureAwait(true);
                }
            }
            catch (JSException)
            {
                // The panel was removed before the focus request arrived.
            }
            catch (InvalidOperationException)
            {
                // The element reference is cleared when the circuit disconnects between render and focus.
            }
        }

        /// <summary>
        /// Receives a media-query match change from <c>sidebar.js</c> and applies it through the normal open-state pipeline.
        /// </summary>
        /// <param name="matches"><see langword="true"/> when the watched query matches.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exclude />
        [JSInvokable(MEDIA_QUERY_CHANGED)]
        public Task OnMediaQueryChanged(bool matches)
        {
            return SetOpenStateAsync(matches, SidebarTrigger.Api);
        }

        private async Task SyncMediaQueryAsync()
        {
            if (IsDisposed || !IsRendered || string.Equals(_watchedMediaQuery, MediaQuery, StringComparison.Ordinal))
            {
                return;
            }

            await ReleaseMediaQueryAsync().ConfigureAwait(true);
            _watchedMediaQuery = MediaQuery;
            if (string.IsNullOrWhiteSpace(MediaQuery) || JSRuntime is null || DotnetObjectReference is null)
            {
                return;
            }

            try
            {
                IJSObjectReference? module = await GetModuleAsync().ConfigureAwait(true);
                if (module is null)
                {
                    _watchedMediaQuery = null;
                    return;
                }

                _mediaQuerySubscription = await module.InvokeAsync<IJSObjectReference>(
                    "watchMedia",
                    MediaQuery,
                    DotnetObjectReference).ConfigureAwait(true);
            }
            catch (JSException)
            {
                // A host without matchMedia, including the unit-test runtime, leaves the parameter inert.
                _watchedMediaQuery = null;
            }
            catch (InvalidOperationException)
            {
                _watchedMediaQuery = null;
            }
        }

        private bool ShouldWatchNavigation => CloseOnNavigate && IsOpenChanged.HasDelegate;

        private void SyncNavigationWatcher()
        {
            if (IsDisposed || ShouldWatchNavigation == _locationSubscribed)
            {
                return;
            }

            if (ShouldWatchNavigation)
            {
                NavigationManager.LocationChanged += HandleLocationChanged;
            }
            else
            {
                NavigationManager.LocationChanged -= HandleLocationChanged;
            }

            _locationSubscribed = ShouldWatchNavigation;
        }

        private void UnsubscribeNavigation()
        {
            if (_locationSubscribed)
            {
                NavigationManager.LocationChanged -= HandleLocationChanged;
                _locationSubscribed = false;
            }
        }

        private void HandleLocationChanged(object? sender, LocationChangedEventArgs args)
        {
            if (IsDisposed || !_isOpen)
            {
                return;
            }

            // Location changes are reported off the renderer dispatcher. State changes must run on it.
            _ = InvokeAsync(() => SetOpenStateAsync(false, SidebarTrigger.Navigation));
        }

        private async ValueTask ReleaseMediaQueryAsync()
        {
            if (_mediaQuerySubscription is null)
            {
                return;
            }

            try
            {
                await _mediaQuerySubscription.InvokeVoidAsync("dispose").ConfigureAwait(true);
                await _mediaQuerySubscription.DisposeAsync().ConfigureAwait(true);
            }
            catch (JSDisconnectedException)
            {
                // The circuit is already gone; the browser drops the listener with the page.
            }
            catch (JSException)
            {
                // The module was never installed. Nothing remains to release.
            }

            _mediaQuerySubscription = null;
        }

        /// <summary>
        /// Releases the navigation subscription and the pending transition. The asynchronous disposal releases the JavaScript resources.
        /// </summary>
        /// <exclude />
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the navigation subscription and the pending transition.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> when called from <see cref="Dispose()"/>.</param>
        /// <exclude />
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            UnsubscribeNavigation();
            CancelTransition();
        }

        /// <summary>
        /// Releases the viewport, media-query and navigation subscriptions and cancels the pending transition.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exclude />
        protected override async ValueTask DisposeAsyncCore()
        {
            UnsubscribeNavigation();
            CancelTransition();
            await ReleaseMediaQueryAsync().ConfigureAwait(true);
            if (_sidebarJsModule is not null)
            {
                try
                {
                    await _sidebarJsModule.InvokeVoidAsync("deactivateModal", _generatedId, false).ConfigureAwait(true);
                }
                catch (JSDisconnectedException)
                {
                    // The circuit disconnected before the modal listener could be released.
                }
                catch (JSException)
                {
                    // The module is no longer available.
                }

                try
                {
                    await _sidebarJsModule.InvokeVoidAsync("unobserve", _generatedId).ConfigureAwait(true);
                }
                catch (JSDisconnectedException)
                {
                    // The circuit disconnected before the observer could be released.
                }
                catch (JSException)
                {
                    // The module is no longer available.
                }

                try
                {
                    await _sidebarJsModule.DisposeAsync().ConfigureAwait(true);
                }
                catch (JSDisconnectedException)
                {
                    // The circuit disconnected before the module could be disposed.
                }
                catch (JSException)
                {
                    // The module is no longer available.
                }
            }

            _callbacksReference?.Dispose();
            _callbacksReference = null;

            _sidebarJsModule = null;
            await base.DisposeAsyncCore().ConfigureAwait(true);
        }
    }
}
