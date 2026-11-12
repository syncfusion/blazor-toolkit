using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Groups related toolbar items visually and semantically. It renders with <c>role="group"</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A group exposes <see cref="ChildContent"/> only. Use an item-level template for custom item content.
    /// </para>
    /// <para>
    /// <strong>Security:</strong> <see cref="Label"/> is encoded by Blazor. <see cref="CssClass"/> is a trust-boundary input.
    /// Do not fill it from untrusted data.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfToolbar AriaLabel="Main">
    ///     <ToolbarGroup Label="Clipboard">
    ///         <ToolbarItem Text="Cut" />
    ///         <ToolbarItem Text="Copy" />
    ///     </ToolbarGroup>
    /// </SfToolbar>
    /// ]]></code>
    /// </example>
    public sealed partial class ToolbarGroup : ComponentBase, IToolbarHost, IDisposable
    {
        private const string RootClass = "e-toolbar-group";

        private readonly ToolbarItemCollection _collection = new();
        private string _cssClass = RootClass;
        private IToolbarHost? _parent;
        private bool _disposed;

        /// <exclude />
        [CascadingParameter]
        internal IToolbarHost? Parent { get; set; }

        /// <summary>
        /// Gets or sets the accessible name of the group, rendered as <c>aria-label</c>.
        /// </summary>
        /// <value>A text string. The default is <see langword="null"/>.</value>
        [Parameter]
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets the items that belong to the group.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/> holding toolbar items. The default is <see langword="null"/>.</value>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets additional CSS classes applied to the group element.
        /// </summary>
        /// <value>A space-separated list of CSS class names. The default is <see langword="null"/>.</value>
        [Parameter]
        public string? CssClass { get; set; }

        private IToolbarHost Self => this;

        ToolbarItemCollection IToolbarHost.Collection => _collection;

        ToolbarOrientation IToolbarHost.Orientation => Parent?.Orientation ?? ToolbarOrientation.Horizontal;

        bool IToolbarHost.IsRtl => Parent?.IsRtl ?? false;

        /// <exclude />
        protected override void OnInitialized()
        {
            _parent = Parent;
            _parent?.RegisterGroup(this);
        }

        /// <exclude />
        protected override void OnParametersSet()
        {
            _cssClass = string.IsNullOrWhiteSpace(CssClass) ? RootClass : RootClass + " " + CssClass;
        }

        void IToolbarHost.Register(IToolbarItem item)
        {
            if (_collection.Add(item))
            {
                _parent?.NotifyItemChanged(item);
            }
        }

        void IToolbarHost.RegisterGroup(IToolbarHost group)
        {
            _ = _collection.AddGroup(group);
        }

        void IToolbarHost.Unregister(IToolbarItem item)
        {
            if (_collection.Remove(item))
            {
                _parent?.NotifyItemChanged(item);
            }
        }

        void IToolbarHost.UnregisterGroup(IToolbarHost group)
        {
            _ = _collection.RemoveGroup(group);
        }

        void IToolbarHost.NotifyItemChanged(IToolbarItem item)
        {
            _parent?.NotifyItemChanged(item);
        }

        Task<bool> IToolbarHost.RaiseItemClickAsync(IToolbarItem item, ToolbarItemClickEventArgs args)
        {
            return _parent is null ? Task.FromResult(false) : _parent.RaiseItemClickAsync(item, args);
        }

        int IToolbarHost.GetOverflowButtonTabIndex()
        {
            return -1;
        }

        int IToolbarHost.GetTabIndex(IToolbarItem item)
        {
            return _parent?.GetTabIndex(item) ?? -1;
        }

        void IToolbarHost.NotifyItemFocused(IToolbarItem item)
        {
            _parent?.NotifyItemFocused(item);
        }

        Task IToolbarHost.NavigateAsync(IToolbarItem item, string key)
        {
            return _parent is null ? Task.CompletedTask : _parent.NavigateAsync(item, key);
        }

        Task IToolbarHost.SelectGroupAsync(IToolbarItem selected)
        {
            return _parent is null ? Task.CompletedTask : _parent.SelectGroupAsync(selected);
        }

        Task<bool> IToolbarHost.PlaceNestedMenuAsync(ElementReference menu, ElementReference anchor)
        {
            return _parent is null ? Task.FromResult(false) : _parent.PlaceNestedMenuAsync(menu, anchor);
        }

        /// <exclude />
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            _parent?.UnregisterGroup(this);
            _collection.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
