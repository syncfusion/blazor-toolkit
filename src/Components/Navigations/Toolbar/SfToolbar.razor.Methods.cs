using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfToolbar
    {
        private static void OnBoundSelectedChanged(ToolbarItemModel model, bool value)
        {
            model.Selected = value;
        }

        #region Methods

        /// <summary>
        /// Re-renders the toolbar items and refreshes the roving tab stop.
        /// </summary>
        /// <returns>A task that completes when the refresh has been scheduled.</returns>
        /// <remarks>
        /// Call it after a change the toolbar cannot observe by itself. Overflow measurement is added with overflow support.
        /// </remarks>
        public ValueTask RefreshOverflowAsync()
        {
            if (IsDisposed)
            {
                return ValueTask.CompletedTask;
            }
            foreach (IToolbarItem item in _collection.Flatten())
            {
                item.Refresh();
            }
            RequestTabStopRefresh();
            ScheduleRecalculation();
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// Moves keyboard focus into the toolbar: to the last focused item, or to the first enabled item.
        /// </summary>
        /// <returns>A task that completes when focus has moved. It completes at once when no item can take focus.</returns>
        public async ValueTask FocusAsync()
        {
            if (IsDisposed)
            {
                return;
            }
            IToolbarItem? target = ResolveTabStop();
            if (target is null)
            {
                return;
            }
            _tabStop = target;
            _lastFocused = target;
            RefreshTabStops();
            await target.FocusItemAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Returns the items currently shown in the main toolbar area.
        /// </summary>
        /// <returns>The visible item components in logical order, excluding items moved to Popup overflow or hidden in collapsed Extended rows or columns. Separators, spacers and groups are not included.</returns>
        public IReadOnlyList<ComponentBase> GetVisibleItems()
        {
            List<IToolbarItem> items = _collection.Flatten();
            return items.FindAll(static i => i.IsVisible && !i.IsOverflowed && !i.IsCollapsedExtended).ConvertAll(static i => i.Component);
        }

        /// <summary>
        /// Returns the items currently moved to Popup overflow or hidden in collapsed Extended rows or columns.
        /// </summary>
        /// <returns>The hidden item components in logical order. The list is empty when all visible items remain in the main toolbar area, as in MultiRow mode.</returns>
        public IReadOnlyList<ComponentBase> GetOverflowItems()
        {
            if (IsDisposed)
            {
                return [];
            }
            List<IToolbarItem> items = _collection.Flatten();
            return items.FindAll(static i => i.IsVisible && (i.IsOverflowed || i.IsCollapsedExtended)).ConvertAll(static i => i.Component);
        }

        #endregion
    }
}
