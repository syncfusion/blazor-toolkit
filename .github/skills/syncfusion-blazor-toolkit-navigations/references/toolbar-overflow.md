# Toolbar Overflow and Layout

`SfToolbar` offers four overflow modes. Popup is the default.

## Modes

| Mode | Behavior | Browser measurement |
|---|---|---|
| `Popup` | Moves items that do not fit into a More menu; the button appears when needed | Yes; uses ResizeObserver/measurement interop |
| `Scrollable` | Keeps items in one line and provides scroll controls when content exceeds available size | Yes; reads scroll position and observes scrolling |
| `MultiRow` | Wraps content onto additional rows | CSS layout |
| `Extended` | Starts with one visible row and expands/collapses additional rows | CSS layout and an expand control |

Static SSR emits markup, but browser-dependent measurement and popup placement
are not available until interactive rendering. In Extended mode the control is
an expand/collapse button, not a menu button.

## Popup Priority

`ToolbarItem.Overflow` controls preference:

1. `Show` items are preferred to stay in the bar.
2. `None` items follow the middle preference.
3. `Hide` items are moved out first.
4. Within each preference, smaller `Priority` values stay visible longer.
5. Original declaration order breaks ties.

The calculator fits candidates in this order and returns visible items in
declaration order. It reserves space for the overflow button only when some
items must overflow. Do not treat priority as a numeric pixel value.

```razor
<SfToolbar AriaLabel="Commands" OverflowMode="ToolbarOverflowMode.Popup">
    <ToolbarItem Text="Save" Overflow="ToolbarOverflowOption.Show" Priority="-10" />
    <ToolbarItem Text="Undo" />
    <ToolbarItem Text="Export" Overflow="ToolbarOverflowOption.Hide" Priority="20" />
</SfToolbar>
```

`OverflowSettings` controls whether the overflow button is shown, its icon,
tooltip, popup class, and whether selecting a popup item closes the menu. If
`ShowOverflowButton` is false, there is no control through which to open the
overflow popup.

| Setting | Default | Effect |
|---|---|---|
| `ShowOverflowButton` | `true` | Shows the More/expand control when the active mode needs it |
| `OverflowButtonIconCss` | `null` | Overrides the built-in control icon |
| `OverflowButtonTooltip` | `"More"` | Supplies the More menu's label/tooltip |
| `PopupCssClass` | `null` | Adds a class to the Popup-mode menu |
| `CloseOnSelect` | `true` | Closes the Popup menu after an item is activated |

When items overflow, Popup reserves room for the More control. The overflow
calculator ranks items by `Show`, `None`, and `Hide`, then ascending priority,
then declaration order. Visible items retain their original declaration order.
The control appears only when there are overflowed items. If no item has been
measured yet, it is not prematurely moved out of the bar.

The Popup is rendered as a menu and positioned against the overflow control.
It can open above or below the anchor based on available viewport space. A
backdrop click closes it; Escape closes it and restores focus to the control.
`CloseOnSelect` governs whether item activation closes it.

## Scrollable Orientation

In Scrollable mode the toolbar keeps the commands in the bar. Start/end
chevrons appear only when content actually overflows; they are disabled at
their respective scroll boundaries. Arrow-key item navigation remains
orientation-aware. The scrolling axis follows the toolbar orientation.

## Static Rendering and Interaction

Static SSR emits the toolbar markup but cannot measure Popup overflow, position
the popup, or read the scroll position for Scrollable controls. Those behaviors
are reconciled after the component becomes interactive. MultiRow and Extended
wrapping are CSS-driven; Extended's control expands rows rather than opening a
menu.

The root `Orientation` may be horizontal or vertical. Horizontal scrolling
uses the inline axis, and vertical scrolling uses the block axis. RTL mirrors
logical placement and the horizontal scroll direction.

## Related Toolbar Behavior

The toolbar root also supports `Align` (`Start`, `Center`, `End`, `Justify`)
and `EnableRtl` (`null` inherits, `true` forces RTL, `false` forces LTR).
`Orientation` defaults to Horizontal; `OverflowMode` defaults to Popup.
Focusable commands use the toolbar's roving tab stop: Left/Right for
horizontal layout (reversed for RTL), Up/Down for vertical layout, and Home/End
for the first/last item. Disabled and invisible items are excluded.

Item `OnClick` runs before root `OnItemClick`; setting the latter's
`ToolbarItemClickEventArgs.Cancel` prevents the default item action. Popup
open/close callbacks are `OnOverflowOpen` and `OnOverflowClose`. They do not
represent Extended mode expand/collapse state.
