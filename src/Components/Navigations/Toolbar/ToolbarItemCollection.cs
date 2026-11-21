namespace Syncfusion.Blazor.Toolkit
{
    /// <exclude />
    /// <summary>
    /// Holds the items and groups registered with one host, in the order the host registered them.
    /// </summary>
    /// <remarks>
    /// Direct children of the toolbar initialize in declaration order during the first render. Children of a group
    /// initialize after the group renders. Each group therefore owns its own list, and <see cref="Flatten"/> walks the
    /// hierarchy depth first. This gives declaration order for the initial render (ADR-019).
    /// </remarks>
    internal sealed class ToolbarItemCollection
    {
        private readonly List<object> _nodes = [];

        /// <exclude />
        /// <summary>
        /// Gets the number of direct nodes (items and groups).
        /// </summary>
        internal int Count => _nodes.Count;

        /// <exclude />
        /// <summary>
        /// Adds an item. Adding the same instance twice has no effect.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns><see langword="true"/> when added; <see langword="false"/> when it was already present.</returns>
        internal bool Add(IToolbarItem item)
        {
            ArgumentNullException.ThrowIfNull(item);
            if (_nodes.Contains(item))
            {
                return false;
            }
            _nodes.Add(item);
            return true;
        }

        /// <exclude />
        /// <summary>
        /// Adds a nested group. Adding the same instance twice has no effect.
        /// </summary>
        /// <param name="group">The group's host.</param>
        /// <returns><see langword="true"/> when added; <see langword="false"/> when it was already present.</returns>
        internal bool AddGroup(IToolbarHost group)
        {
            ArgumentNullException.ThrowIfNull(group);
            if (_nodes.Contains(group))
            {
                return false;
            }
            _nodes.Add(group);
            return true;
        }

        /// <exclude />
        /// <summary>
        /// Removes an item.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns><see langword="true"/> when removed; otherwise, <see langword="false"/>.</returns>
        internal bool Remove(IToolbarItem item)
        {
            return _nodes.Remove(item);
        }

        /// <exclude />
        /// <summary>
        /// Removes a nested group.
        /// </summary>
        /// <param name="group">The group's host.</param>
        /// <returns><see langword="true"/> when removed; otherwise, <see langword="false"/>.</returns>
        internal bool RemoveGroup(IToolbarHost group)
        {
            return _nodes.Remove(group);
        }

        /// <exclude />
        /// <summary>
        /// Removes every node.
        /// </summary>
        internal void Clear()
        {
            _nodes.Clear();
        }

        /// <exclude />
        /// <summary>
        /// Returns every item, depth first.
        /// </summary>
        /// <returns>The items in logical order.</returns>
        internal List<IToolbarItem> Flatten()
        {
            List<IToolbarItem> result = [];
            foreach (object node in _nodes)
            {
                if (node is IToolbarItem item)
                {
                    result.Add(item);
                }
                else if (node is IToolbarHost group)
                {
                    result.AddRange(group.Collection.Flatten());
                }
            }
            return result;
        }

        /// <exclude />
        /// <summary>
        /// Returns the items that can hold keyboard focus, depth first.
        /// </summary>
        /// <returns>The focusable items in logical order.</returns>
        internal List<IToolbarItem> GetFocusable()
        {
            return Flatten().FindAll(static i => i.IsFocusable);
        }
    }
}
