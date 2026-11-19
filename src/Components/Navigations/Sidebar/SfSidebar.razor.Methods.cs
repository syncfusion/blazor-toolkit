using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfSidebar
    {
        /// <summary>
        /// Opens the sidebar.
        /// </summary>
        /// <returns>A task that completes after the events of the change were raised.</returns>
        /// <remarks>
        /// Does nothing if the sidebar is already open. Otherwise raises <see cref="IsOpenChanged"/> and then <see cref="OnOpened"/> with <see cref="SidebarTrigger.Api"/>.
        /// </remarks>
        public Task OpenAsync()
        {
            return InvokeAsync(() => SetOpenStateAsync(true, SidebarTrigger.Api));
        }

        /// <summary>
        /// Closes the sidebar.
        /// </summary>
        /// <returns>A task that completes after the events of the change were raised.</returns>
        /// <remarks>
        /// Does nothing if the sidebar is already closed. Otherwise raises <see cref="IsOpenChanged"/> and then <see cref="OnClosed"/> with <see cref="SidebarTrigger.Api"/>.
        /// </remarks>
        public Task CloseAsync()
        {
            return InvokeAsync(() => SetOpenStateAsync(false, SidebarTrigger.Api));
        }

        /// <summary>
        /// Opens the sidebar when it is closed and closes it when it is open.
        /// </summary>
        /// <returns>A task that completes after the events of the change were raised.</returns>
        public Task ToggleAsync()
        {
            return InvokeAsync(() => SetOpenStateAsync(!_isOpen, SidebarTrigger.Api));
        }

        /// <summary>
        /// Moves keyboard focus into the sidebar panel.
        /// </summary>
        /// <returns>A task that completes after the focus request was sent.</returns>
        /// <remarks>
        /// Focuses the first focusable descendant, or the panel itself when there is none. Completes without effect before the first render or while the sidebar is
        /// closed without a mini rail. Never changes the open state.
        /// </remarks>
        public async Task FocusAsync()
        {
            if (!IsSidebarFocusable)
            {
                return;
            }

            try
            {
                IJSObjectReference? module = await GetModuleAsync().ConfigureAwait(true);
                if (module is null)
                {
                    await _sidebarElement.FocusAsync().ConfigureAwait(true);
                    return;
                }

                await module.InvokeVoidAsync("focusPanel", _sidebarElement).ConfigureAwait(true);
            }
            catch (InvalidOperationException)
            {
                // The element reference is cleared when the circuit disconnects between render and focus.
            }
            catch (JSException)
            {
                // The panel was removed before the focus request arrived.
            }
        }
    }
}
