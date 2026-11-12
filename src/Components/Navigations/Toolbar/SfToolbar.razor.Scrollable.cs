using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfToolbar
    {
        #region Scrollable navigation

        /// <summary>
        /// Gets whether the toolbar scrolls along its main axis instead of moving items out of the bar.
        /// </summary>
        private bool IsScrollableMode => OverflowMode == ToolbarOverflowMode.Scrollable;

        /// <summary>
        /// Gets whether the navigation chevrons are rendered.
        /// </summary>
        /// <remarks>
        /// The chevrons appear only when the items do not fit, so a toolbar that shows everything keeps
        /// a clean edge (TB-OVF-050).
        /// </remarks>
        private bool ShowScrollButtons => IsScrollableMode && _canScroll;

        /// <summary>
        /// Gets whether the chevron that scrolls towards the start is disabled.
        /// </summary>
        private bool ScrollStartDisabled => !_canScroll || _atScrollStart;

        /// <summary>
        /// Gets whether the chevron that scrolls towards the end is disabled.
        /// </summary>
        private bool ScrollEndDisabled => !_canScroll || _atScrollEnd;

        /// <summary>
        /// Gets the icon of the chevron that scrolls towards the start.
        /// </summary>
        private static string ScrollStartIcon => "e-icons e-chevron-left";

        /// <summary>
        /// Gets the icon of the chevron that scrolls towards the end.
        /// </summary>
        private static string ScrollEndIcon => "e-icons e-chevron-right";

        private string ScrollStartLabel => ResolveBuiltInString(null, "Toolbar_ScrollStart", "Scroll to start");

        private string ScrollEndLabel => ResolveBuiltInString(null, "Toolbar_ScrollEnd", "Scroll to end");

        /// <summary>
        /// Gets the attributes of the chevron that scrolls towards the start.
        /// </summary>
        private Dictionary<string, object> ScrollStartAttributes => BuildScrollButtonAttributes("toolbar-scroll-start", ScrollStartLabel, ScrollStartDisabled, -1);

        /// <summary>
        /// Gets the attributes of the chevron that scrolls towards the end.
        /// </summary>
        private Dictionary<string, object> ScrollEndAttributes => BuildScrollButtonAttributes("toolbar-scroll-end", ScrollEndLabel, ScrollEndDisabled, 1);

        private Dictionary<string, object> BuildScrollButtonAttributes(string testId, string label, bool disabled, int direction)
        {
            return new Dictionary<string, object>
            {
                ["aria-label"] = label,
                ["title"] = label,
                ["disabled"] = disabled,
                ["aria-disabled"] = disabled ? "true" : "false",
                ["data-testid"] = testId,
                // Pointer handlers drive hold-to-scroll: the button starts repeating as long as
                // the pointer or the touch is held down, and stops as soon as it is released.
                ["onpointerdown"] = EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.PointerEventArgs>(this, _ => StartScrollRepeat(direction)),
                ["onpointerup"] = EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.PointerEventArgs>(this, _ => StopScrollRepeat()),
                ["onpointerleave"] = EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.PointerEventArgs>(this, _ => StopScrollRepeat()),
                ["onpointercancel"] = EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.PointerEventArgs>(this, _ => StopScrollRepeat())
            };
        }

        /// <summary>
        /// Scrolls the toolbar one page towards the start or the end.
        /// </summary>
        /// <param name="direction">-1 scrolls towards the start, 1 scrolls towards the end.</param>
        /// <returns>A task that completes when the scroll has been applied.</returns>
        private async Task ScrollPageAsync(int direction)
        {
            if (IsDisposed || !IsScrollableMode || (_toolbarModule is null && _toolbarInProcessModule is null))
            {
                return;
            }
            await GuardedAsync(async () => await InvokeVoidAsync(_toolbarModule, _toolbarInProcessModule, "scrollByPage", _contentElement, direction, Orientation == ToolbarOrientation.Vertical).ConfigureAwait(true)).ConfigureAwait(true);
            await UpdateScrollStateAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Scrolls the toolbar a small step. The chevron repeats this while it is held down, so the
        /// content keeps moving under the pointer.
        /// </summary>
        /// <param name="direction">-1 scrolls towards the start, 1 scrolls towards the end.</param>
        /// <returns>A task that completes when the scroll has been applied.</returns>
        private async Task ScrollStepAsync(int direction)
        {
            if (IsDisposed || !IsScrollableMode || (_toolbarModule is null && _toolbarInProcessModule is null))
            {
                return;
            }
            await GuardedAsync(async () => await InvokeVoidAsync(_toolbarModule, _toolbarInProcessModule, "scrollBy", _contentElement, ScrollStepDistance * direction, Orientation == ToolbarOrientation.Vertical).ConfigureAwait(true)).ConfigureAwait(true);
            await UpdateScrollStateAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Starts the repeat that scrolls while the chevron is held down.
        /// </summary>
        /// <param name="direction">-1 scrolls towards the start, 1 scrolls towards the end.</param>
        private void StartScrollRepeat(int direction)
        {
            StopScrollRepeat();
            if (IsDisposed || !IsScrollableMode)
            {
                return;
            }
            _scrollRepeatDirection = direction;
            _scrollRepeatTimer = new Timer(_ => _ = InvokeAsync(async () =>
            {
                if (_scrollRepeatDirection == 0)
                {
                    return;
                }
                await ScrollStepAsync(_scrollRepeatDirection).ConfigureAwait(true);
            }), null, ScrollRepeatDelay, ScrollRepeatInterval);
        }

        /// <summary>
        /// Stops the repeat that scrolls while the chevron is held down.
        /// </summary>
        private void StopScrollRepeat()
        {
            _scrollRepeatTimer?.Dispose();
            _scrollRepeatTimer = null;
            _scrollRepeatDirection = 0;
        }

        /// <summary>
        /// Reads the current scroll position and updates the state of the two chevrons.
        /// </summary>
        /// <returns>A task that completes when the state has been refreshed.</returns>
        private async Task UpdateScrollStateAsync()
        {
            if (IsDisposed || !IsScrollableMode || (_toolbarModule is null && _toolbarInProcessModule is null))
            {
                return;
            }
            ToolbarScrollState? state = null;
            await GuardedAsync(async () =>
                state = await InvokeAsync<ToolbarScrollState>(_toolbarModule!, _toolbarInProcessModule!, "scrollState", _contentElement, Orientation == ToolbarOrientation.Vertical).ConfigureAwait(true)).ConfigureAwait(true);

            if (state is null || IsDisposed)
            {
                return;
            }
            if (state.CanScroll != _canScroll || state.AtStart != _atScrollStart || state.AtEnd != _atScrollEnd)
            {
                _canScroll = state.CanScroll;
                _atScrollStart = state.AtStart;
                _atScrollEnd = state.AtEnd;
                StateHasChanged();
            }
        }

        #endregion
    }
}
