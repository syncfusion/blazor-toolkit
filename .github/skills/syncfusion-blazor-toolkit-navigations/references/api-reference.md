# Navigation API Reference

This reference summarizes the public component surface for `SfToolbar` and
`SfSidebar`, including common defaults and interactions.

## SfToolbar

### Parameters

| Parameter | Type | Default |
|---|---|---|
| `Orientation` | `ToolbarOrientation` | `Horizontal` |
| `OverflowMode` | `ToolbarOverflowMode` | `Popup` |
| `Align` | `ToolbarAlignment` | `Start` |
| `CssClass` | `string?` | `null` |
| `HtmlAttributes` | `Dictionary<string, object>?` | `null` |
| `AriaLabel` | `string?` | `null` |
| `EnableRtl` | `bool?` | `null` |
| `ChildContent` | `RenderFragment?` | `null` |
| `OverflowSettings` | `ToolbarOverflowSettings` | New defaults |
| `Items` | `IEnumerable<ToolbarItemModel>?` | `null` |
| `ItemTemplate` | `RenderFragment<ToolbarTemplateContext>?` | `null` |
| `OnItemClick` | `EventCallback<ToolbarItemClickEventArgs>` | — |
| `OnOverflowOpen`, `OnOverflowClose` | `EventCallback` | — |

### Methods

`RefreshOverflowAsync()`, `FocusAsync()`, `GetVisibleItems()`, and
`GetOverflowItems()`.

### Child Components

- `ToolbarItem`: command, toggle/check, split/dropdown/menu, or template item.
- `ToolbarGroup`: `Label`, `ChildContent`, `CssClass`.
- `ToolbarSeparator`: `Visible`, `CssClass`.
- `ToolbarSpacer`: flexible layout space.
- `ToolbarTemplateItem`: custom child content.

### ToolbarItem Parameters

| Parameter | Type | Default |
|---|---|---|
| `Id`, `Text`, `IconCss` | `string?` | Generated ID, `null`, `null` |
| `IconPosition` | `IconPosition` | `Left` |
| `Type` | `ToolbarItemType` | `Button` |
| `Enabled`, `Visible` | `bool` | `true`, `true` |
| `Selected` | `bool` | `false` |
| `SelectedChanged` | `EventCallback<bool>` | — |
| `Overflow` | `ToolbarOverflowOption` | `None` |
| `Priority` | `int` | `0` |
| `Tooltip`, `Badge`, `CssClass` | `string?` | `null` |
| `DisplayMode` | `ToolbarItemDisplayMode` | `IconAndText` |
| `GroupName` | `string?` | `null` |
| `Items`, `ChildContent` | `RenderFragment?` | `null` |
| `Template` | `RenderFragment<ToolbarTemplateContext>?` | `null` |
| `OnClick` | `EventCallback<MouseEventArgs>` | — |

`Items` is for nested Split, DropDown, and Menu commands. `ChildContent` is
custom button content or nested menu content depending on item type.

### Overflow Settings

| Property | Type | Default |
|---|---|---|
| `ShowOverflowButton` | `bool` | `true` |
| `OverflowButtonIconCss` | `string?` | `null` |
| `OverflowButtonTooltip` | `string` | `"More"` |
| `PopupCssClass` | `string?` | `null` |
| `CloseOnSelect` | `bool` | `true` |

### Enums

`ToolbarOrientation` (`Horizontal`, `Vertical`),
`ToolbarAlignment` (`Start`, `Center`, `End`, `Justify`),
`ToolbarOverflowMode` (`Scrollable`, `Popup`, `MultiRow`, `Extended`),
`ToolbarOverflowOption` (`Show`, `Hide`, `None`),
`ToolbarItemType` (`Button`, `Toggle`, `Check`, `Split`, `DropDown`, `Menu`,
`Separator`, `Spacer`, `Template`), and
`ToolbarItemDisplayMode` (`IconOnly`, `TextOnly`, `IconAndText`).

The component implementation also supports item `Items` for one level of
nested commands. Split items open nested commands from their chevron;
DropDown and Menu items open them from the main button. `ToolbarTemplateContext`
provides `Item` and `IsOverflow`. `ToolbarItemClickEventArgs` provides
`ItemId`, optional data-bound `Item`, `OriginalArgs`, and mutable `Cancel`.
The item callback runs before the toolbar callback; `Cancel` suppresses the
default toggle/menu-open action.

## SfSidebar

### Parameters

| Parameter | Type | Default |
|---|---|---|
| `IsOpen` | `bool` | `false` |
| `Mode` | `SidebarMode` | `Overlay` |
| `Position` | `SidebarPosition` | `Start` |
| `Width`, `MiniWidth` | `string?` | `280px`, `64px` |
| `EnableMini` | `bool` | `false` |
| `ShowBackdrop`, `CloseOnOverlayClick`, `CloseOnEscape` | `bool` | `true` |
| `EnableGestures` | `bool` | `true` |
| `CssClass`, `Id`, `Role`, `AriaLabel` | `string?` | `""`, generated, automatic, localized fallback |
| `MediaQuery` | `string?` | `null` |
| `CloseOnNavigate`, `Responsive` | `bool` | `true` |
| `Breakpoint` | `SidebarBreakpoint` | `Md` |
| `OpenMiniOnHover` | `bool` | `false` |
| `IsEnabled` | `bool` | `true` |
| `AriaLabelledBy` | `string?` | `null` |
| `Duration` | `int` | `300` |
| `EnableAnimation`, `Fixed` | `bool` | `true` |
| `BackdropClass` | `string?` | `null` |
| `HtmlAttributes` | `Dictionary<string, object>?` | `null` |
| `HeaderTemplate`, `ChildContent`, `FooterTemplate`, `TargetContent` | `RenderFragment?` | `null` |

`IsOpenChanged` supports `@bind-IsOpen`. `OnOpened` and `OnClosed` receive
`SidebarEventArgs`; `OnBreakpointChanged` receives a `bool`.

### Methods

`OpenAsync()`, `CloseAsync()`, `ToggleAsync()`, and `FocusAsync()`.

### Enums

- `SidebarMode`: `Overlay`, `Push`, `Shrink`.
- `SidebarPosition`: `Start`, `End`, `Left`, `Right`.
- `SidebarBreakpoint`: `None`, `Xs`, `Sm`, `Md`, `Lg`, `Xl`, `Xxl`, `Always`.
- `SidebarTrigger`: `Api`, `Backdrop`, `Keyboard`, `Gesture`, `Navigation`.

`SidebarEventArgs` exposes the applied open state, effective mode, and trigger.

### Sidebar Event Triggers

`SidebarTrigger` distinguishes API calls or component-driven changes,
backdrop activation, Escape, swipe gestures, and navigation. The
`OnBreakpointChanged` callback is a separate `bool`, true below the configured
breakpoint and false at or above it when a crossing occurs.

`OnOpened` and `OnClosed` follow `IsOpenChanged` for component-driven changes.
They do not fire for a parent-driven `IsOpen` parameter update. With active
animation, they wait for `Duration`; reduced motion or disabled animation makes
them immediate.

### Sidebar Layout and Responsive Behavior

`SidebarMode` selects Overlay (covers content), Push (shifts target), or Shrink
(reduces target width). `SidebarPosition.Start`/`End` follow RTL; `Left`/`Right`
are physical. If `Responsive` is true, the default `Md` breakpoint is 768px;
below it the effective mode is Overlay and the panel closes. `None`, `Xs`, and
`Always` do not select a mobile threshold. `MediaQuery` is a separate
browser-side watcher. `CloseOnNavigate` requires `IsOpenChanged` to be bound
for the location subscription to be active.

### Sidebar Accessibility Defaults

The panel defaults to navigation semantics and a localized accessible name.
`AriaLabelledBy` takes precedence over `AriaLabel`. An Overlay panel with an
active backdrop is exposed as a modal dialog, manages focus containment and
restoration, and marks target content inert. The external toggle is
application-owned and should reference the panel through `aria-controls` and
expose its state through `aria-expanded`.
