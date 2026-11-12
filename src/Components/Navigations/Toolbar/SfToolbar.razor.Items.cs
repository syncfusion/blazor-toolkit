namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfToolbar
    {
        #region Registration

        void IToolbarHost.Register(IToolbarItem item)
        {
            if (IsDisposed)
            {
                return;
            }
            _ = _collection.Add(item);
            ScheduleRecalculation();
            if (item.IsRadio || item.IsCheck)
            {
                StateHasChanged();
            }
        }

        void IToolbarHost.RegisterGroup(IToolbarHost group)
        {
            if (!IsDisposed)
            {
                _ = _collection.AddGroup(group);
            }
        }

        void IToolbarHost.Unregister(IToolbarItem item)
        {
            _ = _collection.Remove(item);
            ReleaseReferences(item);
            RequestTabStopRefresh();
            ScheduleRecalculation();
            StateHasChanged();
        }

        void IToolbarHost.UnregisterGroup(IToolbarHost group)
        {
            _ = _collection.RemoveGroup(group);
            RequestTabStopRefresh();
            ScheduleRecalculation();
        }

        async Task IToolbarHost.SelectGroupAsync(IToolbarItem selected)
        {
            if (IsDisposed || !selected.IsRadio || string.IsNullOrEmpty(selected.SelectionGroup))
            {
                return;
            }
            foreach (IToolbarItem item in _collection.Flatten())
            {
                if (!ReferenceEquals(item, selected) && item.IsRadio && item.IsSelected
                    && string.Equals(item.SelectionGroup, selected.SelectionGroup, StringComparison.Ordinal))
                {
                    await item.ClearSelectionAsync().ConfigureAwait(true);
                }
            }
        }

        void IToolbarHost.NotifyItemChanged(IToolbarItem item)
        {
            if (IsDisposed)
            {
                return;
            }
            if (ReferenceEquals(_tabStop, item) && !item.IsFocusable)
            {
                _tabStop = null;
            }
            RequestTabStopRefresh();
            ScheduleRecalculation();
        }

        private void ReleaseReferences(IToolbarItem item)
        {
            if (ReferenceEquals(_tabStop, item))
            {
                _tabStop = null;
            }
            if (ReferenceEquals(_lastFocused, item))
            {
                _lastFocused = null;
            }
        }

        #endregion
    }
}
