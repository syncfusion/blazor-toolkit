namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfToolbar
    {
        #region Roving tab stop

        /// <summary>
        /// Returns the item that holds the single tab stop: the last focused item while it can still take focus,
        /// otherwise the first focusable item.
        /// </summary>
        private IToolbarItem? ResolveTabStop()
        {
            if (_tabStop is not null && _tabStop.IsFocusable)
            {
                return _tabStop;
            }
            List<IToolbarItem> focusable = _collection.GetFocusable();
            return focusable.Count > 0 ? focusable[0] : null;
        }

        /// <summary>
        /// Gets whether the overflow control currently holds the single tab stop.
        /// </summary>
        /// <remarks>
        /// The overflow control is not an <see cref="IToolbarItem"/>, so it cannot live in the item collection.
        /// It still takes part in the roving tab stop, because it is the only way a keyboard user reaches the
        /// items that moved into the overflow popup. The control is the last stop in the sequence.
        /// </remarks>
        private bool IsOverflowButtonFocused => OverflowButtonFocused && ShowOverflowButton;

        int IToolbarHost.GetOverflowButtonTabIndex()
        {
            return IsOverflowButtonFocused ? 0 : ResolveTabStop() is null ? 0 : -1;
        }

        int IToolbarHost.GetTabIndex(IToolbarItem item)
        {
            return IsOverflowButtonFocused
                ? -1
                : item.IsFocusable && ReferenceEquals(ResolveTabStop(), item) ? 0 : -1;
        }

        /// <summary>
        /// Re-renders only the items whose rendered tab index differs from the wanted one.
        /// </summary>
        private void RefreshTabStops()
        {
            if (IsDisposed)
            {
                return;
            }
            foreach (IToolbarItem item in _collection.Flatten())
            {
                if (item.RenderedTabIndex == int.MinValue)
                {
                    continue;
                }
                // The resolver knows that the overflow control holds the tab stop, so every item gives it up.
                int wanted = ((IToolbarHost)this).GetTabIndex(item);
                if (item.RenderedTabIndex != wanted)
                {
                    item.Refresh();
                }
            }
            // The overflow control is not an item, so it is not part of the loop above. Its tab index
            // comes from the attributes of the button, which only change when the toolbar renders.
            RefreshOverflowButtonTabStop();
        }

        /// <summary>
        /// Re-renders the toolbar when the overflow control has to give up or take the tab stop.
        /// </summary>
        private void RefreshOverflowButtonTabStop()
        {
            if (_overflowButton is null)
            {
                return;
            }
            int wanted = ((IToolbarHost)this).GetOverflowButtonTabIndex();
            if (OverflowButtonTabIndex != wanted)
            {
                OverflowButtonTabIndex = wanted;
                StateHasChanged();
            }
        }

        private void RequestTabStopRefresh()
        {
            if (IsDisposed)
            {
                return;
            }
            _ = InvokeAsync(RefreshTabStops);
        }

        void IToolbarHost.NotifyItemFocused(IToolbarItem item)
        {
            if (IsDisposed || !item.IsFocusable)
            {
                return;
            }
            _lastFocused = item;
            _tabStop = item;
            RefreshTabStops();
        }

        #endregion

        #region Keyboard navigation

        async Task IToolbarHost.NavigateAsync(IToolbarItem item, string key)
        {
            if (IsDisposed)
            {
                return;
            }
            List<IToolbarItem> focusable = _collection.GetFocusable();
            int index = focusable.IndexOf(item);
            if (index < 0)
            {
                return;
            }

            int target = ResolveTargetIndex(key, index, focusable.Count);
            if (target < 0 || target == index)
            {
                return;
            }

            // The overflow control sits after the last item of the bar, so it is the next stop when the
            // focus moves towards the end and the previous stop when it moves back towards the start.
            if (target == focusable.Count && ShowOverflowButton)
            {
                await FocusOverflowButtonAsync().ConfigureAwait(true);
                return;
            }

            IToolbarItem next = focusable[target];
            _lastFocused = next;
            _tabStop = next;
            OverflowButtonFocused = false;
            RefreshTabStops();
            await next.FocusItemAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Maps a key to the index of the item that should take focus. Returns -1 when the key does not move focus.
        /// Navigation does not wrap: at the first or last item the focus stays.
        /// </summary>
        /// <remarks>
        /// The returned index may be the count of the focusable items. That is the position of the overflow
        /// control, which follows the last item of the bar. It is only a valid target when the control is shown.
        /// </remarks>
        private int ResolveTargetIndex(string key, int current, int count)
        {
            if (key == "Home")
            {
                return 0;
            }
            if (key == "End")
            {
                return ShowOverflowButton ? count : count - 1;
            }

            int step;
            if (Orientation == ToolbarOrientation.Vertical)
            {
                step = key == "ArrowDown" ? 1 : key == "ArrowUp" ? -1 : 0;
            }
            else
            {
                int forward = _rtl ? -1 : 1;
                step = key == "ArrowRight" ? forward : key == "ArrowLeft" ? -forward : 0;
            }

            if (step == 0)
            {
                return -1;
            }

            int moved = current + step;
            // The overflow control is one stop past the last item. Without it the focus stays on the
            // last item, so the clamp keeps the index inside the item range.
            int last = ShowOverflowButton ? count : count - 1;
            return Math.Clamp(moved, 0, last);
        }

        #endregion

        #region Click handling

        async Task<bool> IToolbarHost.RaiseItemClickAsync(IToolbarItem item, ToolbarItemClickEventArgs args)
        {
            if (IsDisposed)
            {
                return false;
            }
            if (OnItemClick.HasDelegate)
            {
                await OnItemClick.InvokeAsync(args).ConfigureAwait(true);
            }
            return args.Cancel;
        }

        #endregion
    }
}
