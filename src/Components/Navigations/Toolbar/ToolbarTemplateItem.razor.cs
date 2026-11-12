using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Renders arbitrary custom content inside a toolbar and still takes part in overflow.
    /// </summary>
    /// <remarks>
    /// The content manages its own focus, so a template item is not part of the toolbar's arrow-key order.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfToolbar AriaLabel="Search">
    ///     <ToolbarTemplateItem>
    ///         <input type="search" aria-label="Search" />
    ///     </ToolbarTemplateItem>
    /// </SfToolbar>
    /// ]]></code>
    /// </example>
    public sealed partial class ToolbarTemplateItem : ComponentBase, IToolbarItem, IDisposable
    {
        private readonly string _generatedId = "sf-toolbar-template-" + Guid.NewGuid().ToString("N")[..12];
        private IToolbarHost? _host;
        private (ToolbarOverflowOption Overflow, int Priority) _layoutState;
        private bool _initialized;
        private string ItemClass => ((IToolbarItem)this).IsOverflowed
            ? "e-toolbar-item e-toolbar-template-item e-toolbar-item-overflowed"
            : "e-toolbar-item e-toolbar-template-item";
        private bool _disposed;

        /// <exclude />
        [CascadingParameter]
        internal IToolbarHost? Host { get; set; }

        /// <summary>
        /// Gets or sets the custom content.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/>. The default is <see langword="null"/>.</value>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets how strongly the item prefers to stay in the main bar when space is short.
        /// </summary>
        /// <value>A <see cref="ToolbarOverflowOption"/> value. The default is <see cref="ToolbarOverflowOption.None"/>.</value>
        [Parameter]
        public ToolbarOverflowOption Overflow { get; set; } = ToolbarOverflowOption.None;

        /// <summary>
        /// Gets or sets the secondary overflow sort key. Lower values stay visible longer.
        /// </summary>
        /// <value>An integer. The default is 0.</value>
        [Parameter]
        public int Priority { get; set; }

        ComponentBase IToolbarItem.Component => this;

        string IToolbarItem.ItemId => _generatedId;

        bool IToolbarItem.IsVisible => true;

        bool IToolbarItem.IsEnabled => true;

        bool IToolbarItem.IsFocusable => false;

        ToolbarOverflowOption IToolbarItem.OverflowOption => Overflow;

        int IToolbarItem.OverflowPriority => Priority;

        int IToolbarItem.RenderedTabIndex => -1;

        bool IToolbarItem.IsMeasurable => true;

        bool IToolbarItem.IsOverflowed { get; set; }

        string? IToolbarItem.DisplayText => null;

        string? IToolbarItem.DisplayIconCss => null;

        string? IToolbarItem.SelectionGroup => null;

        bool IToolbarItem.IsRadio => false;

        bool IToolbarItem.IsCheck => false;

        bool IToolbarItem.IsSelected => false;

        Task IToolbarItem.ClearSelectionAsync()
        {
            return Task.CompletedTask;
        }

        void IToolbarItem.SetNestedTabIndex(int? index)
        {
        }

        Task IToolbarItem.ActivateAsync(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            return Task.CompletedTask;
        }

        /// <exclude />
        protected override void OnInitialized()
        {
            _host = Host;
            _host?.Register(this);
        }

        /// <exclude />
        protected override void OnParametersSet()
        {
            (ToolbarOverflowOption, int) state = (Overflow, Priority);
            bool changed = _initialized && !_layoutState.Equals(state);
            _layoutState = state;
            _initialized = true;
            if (changed)
            {
                _host?.NotifyItemChanged(this);
            }
        }

        ValueTask IToolbarItem.FocusItemAsync()
        {
            return ValueTask.CompletedTask;
        }

        void IToolbarItem.Refresh()
        {
            if (!_disposed)
            {
                _ = InvokeAsync(StateHasChanged);
            }
        }

        /// <exclude />
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            _host?.Unregister(this);
            _host = null;
            GC.SuppressFinalize(this);
        }
    }
}
