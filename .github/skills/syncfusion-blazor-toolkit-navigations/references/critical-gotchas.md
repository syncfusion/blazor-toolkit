# Critical Navigation Gotchas

Read this before wiring navigation interactions.

1. **A toolbar needs an accessible name.** Set `AriaLabel` or pass
   `aria-labelledby` in `HtmlAttributes`.
2. **Icon-only toolbar items still need `Text`.** The item uses it as its
   accessible name; `Tooltip` is only supplemental.
3. **Do not bind a toolbar toggle and separately mutate it from stale parent
   state.** Use `@bind-Selected` or explicit `Selected`/`SelectedChanged`.
   Toggle items with the same non-empty `GroupName` are mutually exclusive;
   Check items allow multiple selections.
4. **Toolbar click cancellation does not cancel event delivery.** Item
   `OnClick` runs first, then toolbar `OnItemClick`; setting `Cancel` prevents
   the default action but `OnItemClick` still completes.
5. **Only Popup mode moves toolbar items out of the bar.** Scrollable preserves
   items and scrolls; MultiRow wraps; Extended expands/collapses wrapped rows.
   Extended's control is not a menu trigger.
6. **Static SSR cannot perform browser measurement.** Popup overflow and
   Scrollable controls update with the browser module after interactive render.
7. **Sidebar responsive behavior is based on viewport width, not host width.**
   Below the threshold the effective mode is Overlay, and an open sidebar
   closes. Crossing back does not reopen it.
8. **Initial viewport state is not a breakpoint crossing.** Do not expect
   `OnBreakpointChanged` to report the initial measurement.
9. **Navigation auto-close requires the binding callback.** With
   `CloseOnNavigate="true"`, the location watcher is subscribed only when
   `IsOpenChanged` has a delegate; use `@bind-IsOpen`.
10. **Parent-controlled Sidebar changes do not raise component events.**
    `OnOpened`/`OnClosed` correspond to component-applied state changes, not a
    parameter update from the parent.
11. **`IsEnabled` gates user interactions, not programmatic control.** Public
    methods and parent parameter changes still apply while it is false.
12. **A Sidebar backdrop is modal only in Overlay mode.** `ShowBackdrop` does
    not make Push/Shrink modal, and `CloseOnOverlayClick` applies only to the
    backdrop.
13. **A mini rail is not a substitute for an accessible toggle.**
    `OpenMiniOnHover` does not change `IsOpen`, emits no open/close event, and
    responds only to mouse/pen on applicable desktop states.
14. **CSS dimensions are validated.** Unsafe `Width`/`MiniWidth` values fall
    back to defaults; use ordinary CSS lengths or safe `calc()` expressions.
15. **Avoid feeding trust-boundary values from untrusted input.** This includes
    `CssClass` and HTML attribute dictionaries on either component.

## Decision Summary

- Name the toolbar with `AriaLabel` or `aria-labelledby`; label groups and set
  descriptive `Text` on every item.
- Keep keyboard focus roving within the toolbar. Horizontal arrow keys follow
  direction; vertical arrows use Up/Down; Home and End go to the boundaries.
- Popup is the only mode that moves items into a menu. Scrollable, MultiRow,
  and Extended have distinct behaviors; Extended only expands rows.
- Item `OnClick` precedes toolbar `OnItemClick`. Cancellation prevents the
  default action but does not suppress either callback.
- Sidebar `@bind-IsOpen` synchronizes application state and enables the
  navigation-close subscription. Parent parameter changes raise no state
  events.
- Sidebar responsive behavior uses viewport thresholds. It closes when
  entering mobile behavior and does not reopen when leaving it.
  `OnBreakpointChanged` reports crossings only, not the initial viewport.
- Modal focus behavior applies to an open Overlay sidebar with a backdrop.
  Push/Shrink are not modal; provide a keyboard-accessible external control.
- Static SSR renders initial markup but cannot deliver interactions, watch
  viewport/media changes, measure toolbar overflow, or manage browser focus.
