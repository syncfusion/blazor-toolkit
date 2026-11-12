using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit
{
    /// <exclude />
    /// <summary>
    /// Implemented by the components that own toolbar items: the toolbar itself and each group.
    /// Items find their nearest host through a cascading parameter.
    /// </summary>
    internal interface IToolbarHost
    {
        /// <exclude />
        /// <summary>
        /// Registers an item with this host.
        /// </summary>
        /// <param name="item">The item to register.</param>
        void Register(IToolbarItem item);

        /// <exclude />
        /// <summary>
        /// Registers a nested group with this host.
        /// </summary>
        /// <param name="group">The group's own host.</param>
        void RegisterGroup(IToolbarHost group);

        /// <exclude />
        /// <summary>
        /// Removes an item from this host.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        void Unregister(IToolbarItem item);

        /// <exclude />
        /// <summary>
        /// Removes a nested group from this host.
        /// </summary>
        /// <param name="group">The group's own host.</param>
        void UnregisterGroup(IToolbarHost group);

        /// <exclude />
        /// <summary>
        /// Gets the item collection owned by this host.
        /// </summary>
        ToolbarItemCollection Collection { get; }

        /// <exclude />
        /// <summary>
        /// Called by an item when a value that affects layout, focus or overflow has changed.
        /// </summary>
        /// <param name="item">The item that changed.</param>
        void NotifyItemChanged(IToolbarItem item);

        /// <exclude />
        /// <summary>
        /// Gets the orientation of the owning toolbar.
        /// </summary>
        ToolbarOrientation Orientation { get; }

        /// <exclude />
        /// <summary>
        /// Gets whether the owning toolbar renders right to left.
        /// </summary>
        bool IsRtl { get; }

        /// <exclude />
        /// <summary>
        /// Raises the item click events and applies the cancel rule (ADR-004).
        /// </summary>
        /// <param name="item">The clicked item.</param>
        /// <param name="args">The event arguments for the toolbar-level event.</param>
        /// <returns>A task that completes after the toolbar event has run.</returns>
        Task<bool> RaiseItemClickAsync(IToolbarItem item, ToolbarItemClickEventArgs args);

        /// <exclude />
        /// <summary>
        /// Returns the roving tab index for an item: 0 for the single tab stop, -1 for every other item.
        /// </summary>
        /// <param name="item">The item that is rendering.</param>
        /// <returns>0 or -1.</returns>
        int GetTabIndex(IToolbarItem item);

        /// <exclude />
        /// <summary>
        /// Returns the roving tab index of the overflow button: 0 only when no item holds the tab stop, otherwise -1.
        /// </summary>
        /// <returns>0 or -1.</returns>
        int GetOverflowButtonTabIndex();

        /// <exclude />
        /// <summary>
        /// Called by an item when it receives keyboard focus, so the roving tab stop follows it.
        /// </summary>
        /// <param name="item">The focused item.</param>
        void NotifyItemFocused(IToolbarItem item);

        /// <exclude />
        /// <summary>
        /// Called by an item for a navigation key.
        /// </summary>
        /// <param name="item">The item that handled the key.</param>
        /// <param name="key">The key name from the browser event.</param>
        /// <returns>A task that completes when focus has moved.</returns>
        Task NavigateAsync(IToolbarItem item, string key);

        /// <exclude />
        /// <summary>
        /// Clears every other radio item that shares the selected item's group name.
        /// </summary>
        /// <param name="selected">The item that became selected.</param>
        /// <returns>A task that completes when the other items have cleared.</returns>
        Task SelectGroupAsync(IToolbarItem selected);

        /// <exclude />
        /// <summary>
        /// Decides whether the nested menu of an item opens above or below the item, so it is never cut
        /// off by the bottom of the viewport.
        /// </summary>
        /// <param name="menu">The nested menu element.</param>
        /// <param name="anchor">The element the menu is anchored to.</param>
        /// <returns><see langword="true"/> when the menu opens above the item; otherwise <see langword="false"/>.</returns>
        Task<bool> PlaceNestedMenuAsync(ElementReference menu, ElementReference anchor);
    }
}
