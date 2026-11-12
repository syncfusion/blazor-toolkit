# Toolbar Items and Groups

Toolbar content is declared with `ToolbarItem`, `ToolbarGroup`,
`ToolbarSeparator`, `ToolbarSpacer`, and template content.

## ToolbarItem

`ToolbarItem` represents a command and composes Toolkit `SfButton` controls for
button-like item types. Common parameters:

| Parameter | Purpose |
|---|---|
| `Id` | Consumer identifier; a stable generated ID is used when omitted |
| `Text` | Visible text and accessible name, including in icon-only mode |
| `IconCss`, `IconPosition` | Icon class and placement |
| `Type` | Button, Toggle, Check, Split, DropDown, Menu, or Template |
| `Enabled`, `Visible` | Disable interaction or omit the item |
| `Selected`, `SelectedChanged` | Toggle/check state and two-way binding |
| `GroupName` | Toggle group name for mutual exclusion; Check groups allow multiple selected items |
| `DisplayMode` | IconOnly, TextOnly, or IconAndText |
| `Overflow`, `Priority` | Overflow preference and secondary ordering |
| `Tooltip`, `Badge`, `CssClass` | Supplemental presentation |
| `ChildContent`, `Template` | Custom content or a `ToolbarTemplateContext`-based template |
| `Items` | Nested commands for Split, DropDown, or Menu |
| `OnClick` | Item click callback |

```razor
<ToolbarItem Text="Bold"
             IconCss="e-icons e-bold"
             DisplayMode="ToolbarItemDisplayMode.IconOnly"
             Type="ToolbarItemType.Toggle"
             @bind-Selected="_bold" />
```

`Text` remains the accessible name when the text is hidden. `Tooltip` never
replaces it. A custom `Template` manages its own focusable elements; it is not
automatically treated as the generated button for keyboard focus.

## Groups, Separators, and Spacers

```razor
<ToolbarGroup Label="Clipboard">
    <ToolbarItem Text="Cut" />
    <ToolbarItem Text="Copy" />
</ToolbarGroup>
<ToolbarSeparator />
<ToolbarSpacer />
```

- `ToolbarGroup` renders `role="group"` with `aria-label` from `Label`; it
  exposes `ChildContent` and `CssClass`.
- `ToolbarSeparator` exposes `Visible` and `CssClass`. Separators are not
  focusable.
- `ToolbarSpacer` is a flexible, hidden-from-assistive-technology layout element;
  it is not focusable.
- Separator orientation follows the toolbar orientation.

The group API has no group template. Put custom rendering in an item's
`Template` or use a `ToolbarTemplateItem` for arbitrary child content.

## Nested Commands

Use `Split` for a primary action plus a separate chevron, and `DropDown` or
`Menu` when the main button opens nested commands. Nested menus support one
level. For nested content, use `Items` or `ChildContent`; `Items` takes
precedence when both are supplied.

```razor
<ToolbarItem Text="Save"
             Type="ToolbarItemType.Split"
             OnClick="SaveAsync">
    <Items>
        <ToolbarItem Text="Save as..." OnClick="SaveAsAsync" />
        <ToolbarItem Text="Save all" OnClick="SaveAllAsync" />
    </Items>
</ToolbarItem>
```

For Split, the primary button invokes the action; its chevron opens the menu.
For DropDown and Menu, the primary button opens the nested menu. Setting
`ToolbarItemClickEventArgs.Cancel` prevents the default open/toggle action.

Only one nested menu stays open in a toolbar. Activating an open DropDown or
Menu trigger closes it; activating a different nested-menu trigger switches to
that menu. Selecting an enabled nested command closes its owner after the
existing item and toolbar click callbacks and default selection behavior run.
Focus returns to the trigger after selection. If a callback sets `Cancel`, the
nested menu remains open. Escape also closes the menu and returns focus to its
trigger. A click outside the open menu and its trigger dismisses it without
preventing the clicked control's action or stealing focus.

## Data Templates

`SfToolbar.ItemTemplate` is applied to items created from the root `Items`
collection. `ToolbarTemplateContext.Item` is the model, and `IsOverflow` tells
whether the template is currently rendered in the overflow popup. For
declarative content, `ToolbarItem.Template` can use the same context.

The root `Items` collection is rendered after declarative `ChildContent`.
When no `ItemTemplate` is supplied, each model renders as a `ToolbarItem`.
`ToolbarItem.Items` is instead the nested command content for one-level
Split/DropDown/Menu items; it is distinct from `SfToolbar.Items`.

## Keyboard and Accessible Names

`SfToolbar` uses a single roving tab stop. Enabled and visible button-like
items participate; separators, spacers, invisible items, and disabled items do
not. Horizontal navigation uses Left/Right and reverses in RTL; vertical
navigation uses Up/Down. Home and End move to the first and last item.
`ToolbarItem.Text` provides the accessible name, including for icon-only
display. `Tooltip` is supplemental and does not replace `Text`.

The toolbar itself needs `AriaLabel` or `aria-labelledby`. Groups render as
`role="group"` with `aria-label` from `Label`. A custom `Template` is
responsible for keyboard accessibility and focus behavior of its own content.

## Overflow Preferences

For Popup mode, an item can set `Overflow` to `Show`, `None`, or `Hide`.
Candidates are ranked in that order, then by ascending `Priority`, then by
their declaration order. Lower priority numbers remain visible longer.
Scrollable, MultiRow, and Extended modes do not move individual items into a
popup.

## Binding and Activation

Use `@bind-Selected` or explicit `Selected`/`SelectedChanged`, not both.
Item-level `OnClick` runs before toolbar-level `OnItemClick`; setting
`ToolbarItemClickEventArgs.Cancel` in the latter prevents the default toggle
or menu-open action. A grouped Toggle with an existing selection does not
deselect itself when clicked again. Check items are independently selectable.

## Overflow

Popup mode moves non-fitting items into the overflow menu. Items are ordered
for placement by `OverflowOption` (`Show`, then `None`, then `Hide`), ascending
`Priority`, and their declaration order. Lower priority numbers remain visible
longer. The resulting visible commands keep their original declaration order.
The other modes behave differently: Scrollable keeps all items in a scrolling
line, MultiRow wraps them, and Extended collapses extra rows behind an expand
control.
