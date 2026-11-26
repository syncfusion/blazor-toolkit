using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfToolbar
    {
        private ElementReference _toolbarElement;
        private ToolbarNestedMenuCallback? _nestedMenuCallback;
        private DotNetObjectReference<ToolbarNestedMenuCallback>? _nestedMenuCallbackReference;
        private bool _nestedMenuTracking;

        Task IToolbarHost.OpenNestedMenuAsync(ToolbarItem item)
        {
            return OpenNestedMenuAsync(item);
        }

        private async Task OpenNestedMenuAsync(ToolbarItem item)
        {
            if (IsDisposed)
            {
                return;
            }

            foreach (ToolbarItem openMenu in _collection.Flatten()
                         .Select(static item => item.Component)
                         .OfType<ToolbarItem>()
                         .Where(candidate => candidate.IsNestedMenuOpen && !ReferenceEquals(candidate, item)))
            {
                await openMenu.CloseMenuFromHostAsync().ConfigureAwait(true);
            }

            await StartNestedMenuTrackingAsync().ConfigureAwait(true);
        }

        Task IToolbarHost.NotifyNestedMenuClosedAsync()
        {
            return StopNestedMenuTrackingWhenClosedAsync();
        }

        private async Task StopNestedMenuTrackingWhenClosedAsync()
        {
            if (IsDisposed || _collection.Flatten()
                    .Select(static item => item.Component)
                    .OfType<ToolbarItem>()
                    .Any(static item => item.IsNestedMenuOpen))
            {
                return;
            }

            await StopNestedMenuTrackingAsync().ConfigureAwait(true);
        }

        private async Task StartNestedMenuTrackingAsync()
        {
            if (_nestedMenuTracking || IsDisposed)
            {
                return;
            }

            await EnsureModuleAsync().ConfigureAwait(true);
            if (IsDisposed || (_toolbarModule is null && _toolbarInProcessModule is null))
            {
                return;
            }

            _nestedMenuCallback ??= new ToolbarNestedMenuCallback(this);
            _nestedMenuCallbackReference ??= DotNetObjectReference.Create(_nestedMenuCallback);
            _nestedMenuTracking = true;
            await GuardedAsync(() => InvokeVoidAsync(
                _toolbarModule,
                _toolbarInProcessModule,
                "trackNestedMenus",
                _toolbarElement,
                _nestedMenuCallbackReference)).ConfigureAwait(true);
        }

        private async Task StopNestedMenuTrackingAsync()
        {
            if (!_nestedMenuTracking)
            {
                return;
            }

            _nestedMenuTracking = false;
            await GuardedAsync(() => InvokeVoidAsync(
                _toolbarModule,
                _toolbarInProcessModule,
                "untrackNestedMenus",
                _toolbarElement)).ConfigureAwait(true);
        }

        internal async Task CloseNestedMenusFromOutsideAsync()
        {
            if (IsDisposed)
            {
                return;
            }

            foreach (ToolbarItem openMenu in _collection.Flatten()
                         .Select(static item => item.Component)
                         .OfType<ToolbarItem>()
                         .Where(static item => item.IsNestedMenuOpen))
            {
                await openMenu.CloseMenuFromHostAsync().ConfigureAwait(true);
            }

            await StopNestedMenuTrackingAsync().ConfigureAwait(true);
        }

        internal Task HandleNestedMenuOutsideClickAsync()
        {
            return IsDisposed ? Task.CompletedTask : InvokeAsync(CloseNestedMenusFromOutsideAsync);
        }
    }

    internal sealed class ToolbarNestedMenuCallback
    {
        private readonly SfToolbar _toolbar;

        internal ToolbarNestedMenuCallback(SfToolbar toolbar)
        {
            _toolbar = toolbar;
        }

        [JSInvokable]
        public Task OnNestedMenuOutsideClickAsync()
        {
            return _toolbar.HandleNestedMenuOutsideClickAsync();
        }
    }
}
