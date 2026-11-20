# Toolbar Component

`SfToolbar` renders a responsive command bar from declarative toolbar items,
groups, separators, spacers, templates, or a data-bound model collection.

## Table of Contents

- [Basic Usage](#basic-usage)
- [Layout and Defaults](#layout-and-defaults)
- [Data-Bound Items](#data-bound-items)
- [Public Methods](#public-methods)
- [Events](#events)
- [Accessibility](#accessibility)
- [Styling](#styling)

## Basic Usage

```razor
<SfToolbar AriaLabel="Document commands">
    <ToolbarGroup Label="Document">
        <ToolbarItem Text="New" IconCss="e-icons e-file-new" />
        <ToolbarItem Text="Save" IconCss="e-icons e-save" />
    </ToolbarGroup>
    <ToolbarSeparator />
    <ToolbarItem Text="Bold"
                 Type="ToolbarItemType.Toggle"
                 @bind-Selected="_bold" />
    <ToolbarSpacer />
    <ToolbarItem Text="Export" />
</SfToolbar>

@code {
    private bool _bold;
}
```

## Layout and Defaults

| Parameter | Default | Description |
|---|---|---|
| `Orientation` | `Horizontal` | Horizontal or vertical command layout |
| `OverflowMode` | `Popup` | Popup, Scrollable, MultiRow, or Extended |
| `Align` | `Start` | Start, Center, End, or Justify alignment |
| `EnableRtl` | `null` | Inherit direction when null; otherwise force RTL/LTR |
| `AriaLabel` | `null` | Accessible name; required in practice unless `aria-labelledby` is supplied |
| `CssClass` | `null` | Extra root class |
| `HtmlAttributes` | `null` | Unmatched root attributes, including `aria-labelledby`, `id`, and `style` |

Items remain in declaration order. `ToolbarGroup` organizes related items
semantically and visually; it is not a nested toolbar.

The four overflow modes are:

- `Popup` (default): items that do not fit move to a More menu. The browser
  measures the available space after interactive render.
- `Scrollable`: items remain on one line and the bar scrolls along its main
  axis. Start/end scroll controls appear only when needed.
- `MultiRow`: items wrap onto additional rows.
- `Extended`: extra rows start collapsed and are revealed by an expand control.
  That control is not a menu button.

Popup overflow preference is controlled by each item's `Overflow` and
`Priority`: `Show` items are preferred to stay, then `None`, then `Hide`;
lower priority values stay longer within each preference. Original declaration
order breaks ties.

## Data-Bound Items

`Items` accepts `IEnumerable<ToolbarItemModel>`. Declarative `ChildContent`
renders before data-bound items. An optional `ItemTemplate` receives a
`ToolbarTemplateContext` containing the model and `IsOverflow`.

```razor
<SfToolbar AriaLabel="Quick actions" Items="_actions" />

@code {
    private readonly ToolbarItemModel[] _actions =
    [
        new() { Id = "refresh", Text = "Refresh", IconCss = "e-icons e-refresh" },
        new() { Id = "export", Text = "Export", Overflow = ToolbarOverflowOption.Hide }
    ];
}
```

Do not confuse `SfToolbar.Items` with `ToolbarItem.Items`: the former is the
data-bound root collection; the latter is nested menu content for Split,
DropDown, or Menu items.

`ToolbarItem` supports `Button`, `Toggle`, `Check`, `Split`, `DropDown`,
`Menu`, and `Template` types. Use its `Overflow` and `Priority` parameters to
choose which commands stay visible first in Popup mode. `ToolbarGroup` accepts
`Label`, `ChildContent`, and `CssClass`; `ToolbarSeparator` and
`ToolbarSpacer` provide non-focusable structure.

Nested menu items use `Items` or `ChildContent`, with `Items` taking precedence
when both are supplied. Nested menus are one level deep. A Split item performs
its primary action on the main button and opens the menu from its chevron;
DropDown and Menu open from their main button.

## Public Methods

| Method | Description |
|---|---|
| `RefreshOverflowAsync()` | Refreshes registered item rendering, the roving tab stop, and scheduled layout/overflow calculation |
| `FocusAsync()` | Focuses the last focused item, or the first enabled focusable item |
| `GetVisibleItems()` | Returns visible, non-overflowed component items in logical order; excludes separators, spacers, and groups |
| `GetOverflowItems()` | Returns visible component items placed in the overflow menu |

Call `RefreshOverflowAsync()` after external changes the component cannot
observe. Toolbar items and groups register and unregister over their component
lifecycle; dispose dynamic items normally rather than retaining stale component
references.

## Events

- Item-level `ToolbarItem.OnClick` runs before `SfToolbar.OnItemClick`.
- `OnItemClick` receives `ToolbarItemClickEventArgs` and fires even if its
  handler sets `Cancel`; `Cancel` prevents the item's default action.
- `OnOverflowOpen` and `OnOverflowClose` are no-argument callbacks and fire on
  actual popup open/close state transitions. Extended mode uses an expand
  control rather than an overflow menu.
- `SelectedChanged` supports `@bind-Selected` for selectable items.

For an activated item, item-level `OnClick` runs first, followed by
`OnItemClick`, which receives `ItemId`, the data model when the item is
data-bound, the original mouse event, and `Cancel`.

## Accessibility

The root renders `role="toolbar"` and `aria-orientation`. Its children use a
roving tab stop. Name each toolbar with `AriaLabel` or `aria-labelledby`, and
name each group with `ToolbarGroup.Label`. Horizontal arrow keys follow the
text direction; vertical toolbars use Up/Down. Home and End move to the first
and last enabled visible item. Disabled items, invisible items, separators,
and spacers are skipped. `FocusAsync()` focuses the last-focused item or the
first focusable item.

Set `Text` even when `DisplayMode="IconOnly"` so the icon-only button has an
accessible name. Nested menus and the Popup overflow menu use menu semantics;
Escape closes them and returns focus appropriately. A custom item `Template`
owns the focusability and keyboard behavior of its contents.

## Styling

The component emits `SfThemeRoot` and includes an isolated stylesheet. Root
attributes and `CssClass` are merged onto the toolbar. Use CSS custom properties
and logical properties for theme and RTL behavior. Avoid overriding focus
indicators; forced-colors styles use system colors.
