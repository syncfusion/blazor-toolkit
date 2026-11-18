using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Represents a single command inside an <see cref="SfToolbar"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A button, toggle, split, drop-down or menu item renders a Toolkit <see cref="SfButton"/>. The item registers with its toolbar
    /// when it initializes and unregisters when it is disposed, so items can be added, removed and reordered at runtime.
    /// </para>
    /// <para>
    /// <strong>Security:</strong> <see cref="Text"/>, <see cref="IconCss"/>, <see cref="Tooltip"/>, <see cref="Badge"/> and <see cref="Id"/>
    /// are encoded by Blazor and never reach a raw HTML sink. <see cref="CssClass"/> is a trust-boundary input. Do not fill it from untrusted data.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfToolbar AriaLabel="Edit">
    ///     <ToolbarItem Id="cut" Text="Cut" IconCss="e-icons e-cut" OnClick="@OnCut" />
    ///     <ToolbarItem Text="Bold" Type="ToolbarItemType.Toggle" @bind-Selected="isBold" />
    /// </SfToolbar>
    /// ]]></code>
    /// </example>
    public sealed partial class ToolbarItem : ComponentBase, IToolbarItem, IDisposable
    {
        #region Constants

        private const string ItemRootClass = "e-toolbar-item";
        private const string SeparatorRootClass = "e-toolbar-separator";
        private const string SpacerRootClass = "e-toolbar-spacer";
        private const string ButtonRootClass = "e-tbar-btn";
        private const string ActiveClass = "e-active";
        private const string IdPrefix = "sf-toolbar-item-";
        private const string OverflowedClass = " e-toolbar-item-overflowed";
        private const string TrueValue = "true";
        private const string FalseValue = "false";

        #endregion

        #region Fields

        private IToolbarHost? _host;
        private SfButton? _button;
        private SfButton? _chevronButton;
        private bool _splitChevronFocused;
        private readonly string _menuId = "sf-toolbar-menu-" + Guid.NewGuid().ToString("N")[..12];
        private bool MenuOpen { get; set; }
        private bool _menuAbove;
        private bool _menuPlacementPending;
        private ElementReference _menuElement;
        private ElementReference _itemElement;
        private bool _focusNestedOnRender;
        private int _menuFocusIndex;
        private int? _nestedTabIndex;
        private readonly List<IToolbarItem> _nestedItems = [];
        private EventCallback<MouseEventArgs> _clickCallback;
        private EventCallback<MouseEventArgs> _chevronCallback;
        private ToolbarTemplateContext _templateContext = new();
        private (bool Visible, bool Enabled, ToolbarOverflowOption Overflow, int Priority, ToolbarItemType Type) _layoutState;
        private bool _initialized;
        private bool _disposed;
        private int _renderedTabIndex = int.MinValue;

        private string _separatorClass = SeparatorRootClass;
        private string _spacerClass = SpacerRootClass;
        private string _separatorOrientation = "vertical";
        private string? _explicitId;
        private string? _buttonClass;
        private string? _iconCss;
        private bool _iconBeforeText = true;
        private bool _showText = true;
        private bool _hasBadge;
        private bool _hasRichBody;

        #endregion

        #region Internal members

        /// <exclude />
        [CascadingParameter]
        internal IToolbarHost? Host { get; set; }

        [CascadingParameter(Name = "NestedOwner")]
        internal ToolbarItem? NestedOwner { get; set; }

        ComponentBase IToolbarItem.Component => this;

        string IToolbarItem.ItemId => ResolvedId;

        bool IToolbarItem.IsVisible => Visible;

        bool IToolbarItem.IsEnabled => Enabled;

        bool IToolbarItem.IsFocusable =>
            Visible &&
            Enabled &&
            !((IToolbarItem)this).IsOverflowed &&
            NestedOwner is null &&
            Type != ToolbarItemType.Separator &&
            Type != ToolbarItemType.Spacer &&
            !((IToolbarItem)this).IsCollapsedExtended;

        ToolbarOverflowOption IToolbarItem.OverflowOption => Overflow;

        int IToolbarItem.OverflowPriority => Priority;

        int IToolbarItem.RenderedTabIndex => _renderedTabIndex;

        bool IToolbarItem.IsMeasurable =>
            NestedOwner is null &&
            Visible &&
            Type != ToolbarItemType.Separator &&
            Type != ToolbarItemType.Spacer;

        bool IToolbarItem.IsOverflowed { get; set; }

        bool IToolbarItem.IsCollapsedExtended { get; set; }

        string? IToolbarItem.DisplayText => Text;

        string? IToolbarItem.DisplayIconCss => IconCss;

        Task IToolbarItem.ActivateAsync(MouseEventArgs args)
        {
            return HandleClickAsync(args);
        }

        private bool IsTemplated => Template is not null || Type == ToolbarItemType.Template;

        private string GeneratedId { get; } = IdPrefix + Guid.NewGuid().ToString("N")[..12];

        private string MeasureId => ResolvedId;

        private string BaseItemClass { get; set; } = ItemRootClass;

        private string ItemClass => ((IToolbarItem)this).IsOverflowed ? BaseItemClass + OverflowedClass : BaseItemClass;

        private int TemplateTabIndex
        {
            get
            {
                int tabIndex = _nestedTabIndex ?? _host?.GetTabIndex(this) ?? (((IToolbarItem)this).IsFocusable ? 0 : -1);
                _renderedTabIndex = tabIndex;
                return tabIndex;
            }
        }

        private string ResolvedId => string.IsNullOrWhiteSpace(Id) ? GeneratedId : Id;

        private bool IsToggle => Type == ToolbarItemType.Toggle;

        private bool IsCheck => Type == ToolbarItemType.Check;

        private bool IsSelectable => IsToggle || IsCheck;

        private bool IsRadio => IsToggle && !string.IsNullOrEmpty(GroupName);

        private bool HasNestedMenu => Type is ToolbarItemType.Split or ToolbarItemType.DropDown or ToolbarItemType.Menu;

        internal bool IsNestedMenuOpen => MenuOpen;

        private bool OpensFromPrimary => Type is ToolbarItemType.DropDown or ToolbarItemType.Menu;

        /// <summary>
        /// Gets the class of the nested menu. The class tells the stylesheet which side of the item the
        /// menu opens on, so it is never cut off by the bottom of the viewport.
        /// </summary>
        private string MenuClass => _menuAbove ? "e-toolbar-nested-menu e-toolbar-nested-menu-top" : "e-toolbar-nested-menu";

        string? IToolbarItem.SelectionGroup => string.IsNullOrEmpty(GroupName) ? null : GroupName;

        bool IToolbarItem.IsRadio => IsRadio;

        bool IToolbarItem.IsCheck => IsCheck && !string.IsNullOrEmpty(GroupName);

        bool IToolbarItem.IsSelected => Selected;

        #endregion

        #region Lifecycle

        /// <exclude />
        protected override void OnInitialized()
        {
            _host = Host;
            _clickCallback = EventCallback.Factory.Create<MouseEventArgs>(this, HandleClickAsync);
            _chevronCallback = EventCallback.Factory.Create<MouseEventArgs>(this, HandleChevronAsync);
            _host?.Register(this);
            NestedOwner?.RegisterNested(this);
        }

        /// <exclude />
        protected override void OnParametersSet()
        {
            ComputeRenderState();
            (bool, bool, ToolbarOverflowOption, int, ToolbarItemType) state = (Visible, Enabled, Overflow, Priority, Type);
            bool changed = _initialized && !_layoutState.Equals(state);
            _layoutState = state;
            _initialized = true;
            if (changed)
            {
                _host?.NotifyItemChanged(this);
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
            NestedOwner?.UnregisterNested(this);
            _host?.Unregister(this);
            _host = null;
            _button = null;
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Render state

        internal void MarkOverflowContext(bool isOverflow)
        {
            _templateContext.IsOverflow = isOverflow;
        }

        private void ComputeRenderState()
        {
            _explicitId = string.IsNullOrWhiteSpace(Id) ? null : Id;
            _hasBadge = !string.IsNullOrEmpty(Badge);
            _showText = DisplayMode != ToolbarItemDisplayMode.IconOnly || string.IsNullOrEmpty(IconCss);
            bool showIcon = DisplayMode != ToolbarItemDisplayMode.TextOnly;
            _iconCss = showIcon && !string.IsNullOrEmpty(IconCss) ? IconCss : null;
            _iconBeforeText = IconLeadsText(IconPosition, _host?.IsRtl ?? false);
            _hasRichBody = !IsTemplated && !HasNestedMenu && !IsRadio && !(IsCheck && !string.IsNullOrEmpty(GroupName)) && (ChildContent is not null || _hasBadge);

            BaseItemClass = Join(ItemRootClass, CssClass, IsSelectable && Selected ? "e-selected" : null);
            _separatorClass = Join(SeparatorRootClass, CssClass, null);
            _spacerClass = Join(SpacerRootClass, CssClass, null);
            _buttonClass = Join(ButtonRootClass, null, IsSelectable && Selected ? ActiveClass : null);
            _separatorOrientation = _host?.Orientation == ToolbarOrientation.Vertical ? "horizontal" : "vertical";

            _templateContext = new ToolbarTemplateContext
            {
                Item = new ToolbarItemModel
                {
                    Id = Id,
                    Text = Text,
                    IconCss = IconCss,
                    Type = Type,
                    Enabled = Enabled,
                    Visible = Visible,
                    Selected = Selected,
                    Overflow = Overflow,
                    Priority = Priority,
                    Tooltip = Tooltip,
                    Badge = Badge,
                    GroupName = GroupName,
                    DisplayMode = DisplayMode
                },
                IsOverflow = ((IToolbarItem)this).IsOverflowed
            };
        }

        /// <summary>
        /// Decides whether the icon is rendered before the text. Left and Top lead in LTR.
        /// Right and Bottom lead in RTL, matching the logical start of the item. The toolbar
        /// direction is used, not the global button option, so a click cannot swap the order.
        /// </summary>
        private static bool IconLeadsText(IconPosition position, bool rtl)
        {
            bool logicalStart = position is IconPosition.Left or IconPosition.Top;
            return rtl ? !logicalStart : logicalStart;
        }

        private static string Join(string root, string? custom, string? extra)
        {
            string result = root;
            if (!string.IsNullOrWhiteSpace(extra))
            {
                result += " " + extra;
            }
            if (!string.IsNullOrWhiteSpace(custom))
            {
                result += " " + custom;
            }
            return result;
        }

        /// <summary>
        /// Builds the parameters and attributes passed to the <see cref="SfButton"/>. The roving tab index is read here so that the
        /// value the host expects is the value that is rendered.
        /// </summary>
        private Dictionary<string, object> BuildButtonAttributes()
        {
            int itemTabIndex = _nestedTabIndex ?? _host?.GetTabIndex(this) ?? 0;
            _renderedTabIndex = itemTabIndex;
            int tabIndex = Type == ToolbarItemType.Split && _nestedTabIndex is null && _splitChevronFocused
                ? -1
                : itemTabIndex;

            Dictionary<string, object> attributes = new()
            {
                ["Disabled"] = !Enabled,
                ["CssClass"] = _buttonClass ?? ButtonRootClass,
                ["OnClick"] = _clickCallback,
                ["HtmlAttributes"] = BuildHtmlAttributes(tabIndex)
            };

            return attributes;
        }

        private Dictionary<string, object> BuildHtmlAttributes(int tabIndex)
        {
            Dictionary<string, object> html = new()
            {
                ["tabindex"] = tabIndex.ToString(CultureInfo.InvariantCulture),
                ["data-testid"] = Type switch
                {
                    ToolbarItemType.Toggle => "toolbar-toggle",
                    ToolbarItemType.Check => "toolbar-check",
                    ToolbarItemType.Split => "toolbar-split-primary",
                    ToolbarItemType.DropDown => "toolbar-dropdown",
                    ToolbarItemType.Menu => "toolbar-menubutton",
                    ToolbarItemType.Button => "toolbar-button",
                    // Separator, Spacer and Template never render a button, so they cannot reach this
                    // method. They keep the default id in case an invalid combination does.
                    ToolbarItemType.Separator => "toolbar-button",
                    ToolbarItemType.Spacer => "toolbar-button",
                    ToolbarItemType.Template => "toolbar-button",
                    _ => "toolbar-button"
                }
            };
            if (IsSelectable)
            {
                html["aria-pressed"] = Selected ? TrueValue : FalseValue;
            }
            html["onpointerdown"] = EventCallback.Factory.Create<PointerEventArgs>(this, StartPointerRippleAsync);
            if (OpensFromPrimary)
            {
                html["aria-haspopup"] = "menu";
                html["aria-expanded"] = MenuOpen ? TrueValue : FalseValue;
            }
            if (!_showText && !string.IsNullOrEmpty(Text))
            {
                html["aria-label"] = Text;
            }
            if (!string.IsNullOrEmpty(Tooltip))
            {
                html["title"] = Tooltip;
            }
            if (IsRadio)
            {
                html["data-group"] = GroupName!;
            }
            if (HasNestedMenu && NestedOwner is null)
            {
                html["onkeydown"] = EventCallback.Factory.Create<KeyboardEventArgs>(this, HandlePrimaryKeyDownAsync);
                html["onfocus"] = EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.FocusEventArgs>(this, HandlePrimaryFocusIn);
            }
            if (_nestedTabIndex is not null)
            {
                html["data-testid"] = "toolbar-nested-item";
                // An item inside a nested menu is a child of role="menu", and that role only accepts
                // menuitem, menuitemradio and menuitemcheckbox children. The button is plain HTML, so
                // it needs the role written out.
                html["role"] = IsRadio ? "menuitemradio" : IsCheck ? "menuitemcheckbox" : "menuitem";
            }
            return html;
        }

        private Dictionary<string, object> BuildChevronHtmlAttributes()
        {
            Dictionary<string, object> html = new()
            {
                ["aria-haspopup"] = "menu",
                ["aria-expanded"] = MenuOpen ? TrueValue : FalseValue,
                ["aria-label"] = string.IsNullOrEmpty(Text) ? "Open" : Text + " menu",
                ["tabindex"] = _host?.GetTabIndex(this) == 0 && _splitChevronFocused ? "0" : "-1",
                ["data-testid"] = "toolbar-split-toggle"
            };
            html["onkeydown"] = EventCallback.Factory.Create<KeyboardEventArgs>(this, HandleChevronKeyDownAsync);
            html["onfocus"] = EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.FocusEventArgs>(this, HandleChevronFocusIn);
            return html;
        }

        private Dictionary<string, object> BuildChevronAttributes()
        {
            Dictionary<string, object> attributes = new()
            {
                ["Disabled"] = !Enabled,
                ["CssClass"] = ButtonRootClass + " e-toolbar-chevron",
                ["IconCss"] = "e-icons e-chevron-down",
                ["OnClick"] = _chevronCallback,
                ["HtmlAttributes"] = BuildChevronHtmlAttributes()
            };
            return attributes;
        }

        #endregion

        #region Events

        private async Task HandleClickAsync(MouseEventArgs args)
        {
            if (_disposed || !Enabled)
            {
                return;
            }

            // The item handler runs first, then the toolbar handler, which may set Cancel (ADR-004).
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(args).ConfigureAwait(true);
            }

            bool cancel = false;
            if (_host is not null)
            {
                ToolbarItemClickEventArgs clickArgs = new() { ItemId = ResolvedId, Item = BoundModel, OriginalArgs = args };
                cancel = await _host.RaiseItemClickAsync(this, clickArgs).ConfigureAwait(true);
            }

            if (cancel || _disposed)
            {
                return;
            }
            if (OpensFromPrimary && HasNestedMenu)
            {
                if (MenuOpen)
                {
                    await CloseMenuAsync().ConfigureAwait(true);
                }
                else
                {
                    await OpenMenuAsync().ConfigureAwait(true);
                }
            }
            if (IsSelectable)
            {
                if (!(IsRadio && Selected))
                {
                    Selected = !Selected;
                }
                ComputeRenderState();
                await SelectedChanged.InvokeAsync(Selected).ConfigureAwait(true);
                if (IsRadio && Selected && _host is not null)
                {
                    await _host.SelectGroupAsync(this).ConfigureAwait(true);
                }
            }

            if (NestedOwner is not null && !HasNestedMenu)
            {
                await NestedOwner.CloseMenuAfterSelectionAsync().ConfigureAwait(true);
            }
        }

        private Task HandleChevronAsync(MouseEventArgs args)
        {
            return _disposed || !Enabled ? Task.CompletedTask : ToggleMenuAsync();
        }

        /// <summary>
        /// Starts the color wash at the pointer. A keyboard activation has no pointer, so it does not call this.
        /// </summary>
        private async Task StartPointerRippleAsync(PointerEventArgs args)
        {
            if (_disposed || !Enabled || _host is null || args.PointerId == 0 || (args.PointerType == "mouse" && args.Buttons == 0))
            {
                return;
            }
            await _host.PlayRippleAsync(_itemElement, args.ClientX, args.ClientY).ConfigureAwait(true);
        }

        private Task ToggleMenuAsync()
        {
            return MenuOpen ? CloseMenuAsync() : OpenMenuAsync();
        }

        private async Task OpenMenuAsync()
        {
            if (MenuOpen || (Items is null && ChildContent is null))
            {
                return;
            }
            if (_host is not null)
            {
                // Close the other menus only when this one is about to open. Doing it on every
                // primary click closed this menu before its own toggle could run.
                await _host.OpenNestedMenuAsync(this).ConfigureAwait(true);
            }
            if (_disposed)
            {
                return;
            }
            MenuOpen = true;
            _menuFocusIndex = 0;
            _focusNestedOnRender = true;
            _menuPlacementPending = true;
            ApplyNestedTabStops();
            StateHasChanged();
        }

        /// <exclude />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_menuPlacementPending && MenuOpen)
            {
                _menuPlacementPending = false;
                await ApplyMenuPlacementAsync().ConfigureAwait(true);
            }
            if (_focusNestedOnRender && MenuOpen && _nestedItems.Count > 0)
            {
                _focusNestedOnRender = false;
                await FocusNestedAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Asks the toolbar which side the nested menu opens on, so the menu is never cut off by the
        /// bottom of the viewport.
        /// </summary>
        private async Task ApplyMenuPlacementAsync()
        {
            if (_disposed || _host is null)
            {
                return;
            }
            bool above = await _host.PlaceNestedMenuAsync(_menuElement, _itemElement).ConfigureAwait(true);
            if (_disposed || above == _menuAbove)
            {
                return;
            }
            _menuAbove = above;
            StateHasChanged();
        }
        private Task CloseMenuAfterSelectionAsync()
        {
            return CloseMenuAsync();
        }

        internal Task CloseMenuFromHostAsync()
        {
            return CloseMenuAsync(restoreFocus: false, notifyHost: false);
        }

        private async Task CloseMenuAsync(bool restoreFocus = true, bool notifyHost = true)
        {
            if (!MenuOpen)
            {
                return;
            }
            MenuOpen = false;
            StateHasChanged();
            if (notifyHost && _host is not null)
            {
                await _host.NotifyNestedMenuClosedAsync().ConfigureAwait(true);
            }
            if (restoreFocus && _button is not null)
            {
                await _button.FocusElementAsync().ConfigureAwait(true);
            }
        }

        private async Task HandleMenuKeyDownAsync(KeyboardEventArgs args)
        {
            if (!MenuOpen || _nestedItems.Count == 0)
            {
                return;
            }
            switch (args.Key)
            {
                case "Escape":
                    await CloseMenuAsync().ConfigureAwait(true);
                    return;
                case "ArrowDown":
                    // Navigation does not wrap. At the last entry the focus stays.
                    _menuFocusIndex = Math.Min(_nestedItems.Count - 1, _menuFocusIndex + 1);
                    break;
                case "ArrowUp":
                    _menuFocusIndex = Math.Max(0, _menuFocusIndex - 1);
                    break;
                case "Home":
                    _menuFocusIndex = 0;
                    break;
                case "End":
                    _menuFocusIndex = _nestedItems.Count - 1;
                    break;
                default:
                    return;
            }

            ApplyNestedTabStops();
            StateHasChanged();
            await FocusNestedAsync().ConfigureAwait(true);
        }

        private void ApplyNestedTabStops()
        {
            if (_nestedItems.Count == 0)
            {
                return;
            }
            _menuFocusIndex = Math.Clamp(_menuFocusIndex, 0, _nestedItems.Count - 1);
            for (int i = 0; i < _nestedItems.Count; i++)
            {
                _nestedItems[i].SetNestedTabIndex(i == _menuFocusIndex ? 0 : -1);
            }
        }

        private async Task FocusNestedAsync()
        {
            if (_menuFocusIndex < 0 || _menuFocusIndex >= _nestedItems.Count)
            {
                return;
            }
            await _nestedItems[_menuFocusIndex].FocusItemAsync().ConfigureAwait(true);
        }

        internal void RegisterNested(IToolbarItem item)
        {
            if (_nestedItems.Contains(item))
            {
                return;
            }
            _nestedItems.Add(item);
            item.SetNestedTabIndex(_nestedItems.Count - 1 == _menuFocusIndex ? 0 : -1);
        }

        internal void UnregisterNested(IToolbarItem item)
        {
            _ = _nestedItems.Remove(item);
            item.SetNestedTabIndex(null);
        }

        void IToolbarItem.SetNestedTabIndex(int? index)
        {
            _nestedTabIndex = index;
            if (!_disposed)
            {
                _ = InvokeAsync(StateHasChanged);
            }
        }

        async Task IToolbarItem.ClearSelectionAsync()
        {
            if (_disposed || !Selected)
            {
                return;
            }
            Selected = false;
            ComputeRenderState();
            await SelectedChanged.InvokeAsync(false).ConfigureAwait(true);
            StateHasChanged();
        }

        private Task HandleKeyDownAsync(KeyboardEventArgs args)
        {
            return _disposed || _host is null || !IsNavigationKey(args.Key)
                ? Task.CompletedTask
                : _host.NavigateAsync(this, args.Key);
        }

        private async Task HandlePrimaryKeyDownAsync(KeyboardEventArgs args)
        {
            if (_disposed || _host is null || !IsNavigationKey(args.Key))
            {
                return;
            }
            if (Type == ToolbarItemType.Split && IsForwardInlineKey(args.Key))
            {
                await FocusSplitPartAsync(chevron: true).ConfigureAwait(true);
                return;
            }
            await _host.NavigateAsync(this, args.Key).ConfigureAwait(true);
        }

        private async Task HandleChevronKeyDownAsync(KeyboardEventArgs args)
        {
            if (_disposed || _host is null || !IsNavigationKey(args.Key))
            {
                return;
            }
            if (IsBackwardInlineKey(args.Key))
            {
                await FocusSplitPartAsync(chevron: false).ConfigureAwait(true);
                return;
            }
            await _host.NavigateAsync(this, args.Key).ConfigureAwait(true);
        }

        private void HandleFocusIn(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (!_disposed)
            {
                _host?.NotifyItemFocused(this);
            }
        }

        private void HandlePrimaryFocusIn(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (_disposed)
            {
                return;
            }
            if (_splitChevronFocused)
            {
                _splitChevronFocused = false;
                StateHasChanged();
            }
            _host?.NotifyItemFocused(this);
        }

        private void HandleChevronFocusIn(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (_disposed)
            {
                return;
            }
            if (!_splitChevronFocused)
            {
                _splitChevronFocused = true;
                StateHasChanged();
            }
            _host?.NotifyItemFocused(this);
        }

        private async Task FocusSplitPartAsync(bool chevron)
        {
            if (Type != ToolbarItemType.Split || _host is null)
            {
                return;
            }
            _splitChevronFocused = chevron;
            _host.NotifyItemFocused(this);
            StateHasChanged();
            await Task.Yield();
            SfButton? button = chevron ? _chevronButton : _button;
            if (button is not null)
            {
                await button.FocusElementAsync().ConfigureAwait(true);
            }
        }

        private bool IsForwardInlineKey(string key)
        {
            return _host?.IsRtl == true ? key == "ArrowLeft" : key == "ArrowRight";
        }

        private bool IsBackwardInlineKey(string key)
        {
            return _host?.IsRtl == true ? key == "ArrowRight" : key == "ArrowLeft";
        }

        private static bool IsNavigationKey(string key)
        {
            return key is "ArrowLeft" or "ArrowRight" or "ArrowUp" or "ArrowDown" or "Home" or "End";
        }

        #endregion

        #region IToolbarItem

        ValueTask IToolbarItem.FocusItemAsync()
        {
            if (_splitChevronFocused)
            {
                _splitChevronFocused = false;
                StateHasChanged();
            }
            return _button is not null
                ? _button.FocusElementAsync()
                : _itemElement.Context is null
                    ? ValueTask.CompletedTask
                    : _itemElement.FocusAsync();
        }

        void IToolbarItem.Refresh()
        {
            if (!_disposed)
            {
                _ = InvokeAsync(StateHasChanged);
            }
        }

        #endregion
    }
}
