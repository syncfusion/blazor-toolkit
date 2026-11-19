# Navigation Troubleshooting

## Toolbar

### Popup overflow does not appear during server prerender

Popup item placement depends on browser-side measurement. Confirm the page
becomes interactive and the Toolkit toolbar script is available. Static SSR
renders the toolbar but cannot measure its available width.

### The overflow button is missing

In Popup mode it appears only when items overflow and
`OverflowSettings.ShowOverflowButton` is true. In Extended mode it is the
expand/collapse control. Scrollable mode instead shows navigation chevrons only
when the content can scroll.

### Keyboard focus skips a custom item

`ToolbarItem.Template` content manages its own focus and is not included as a
generated button in the toolbar roving tab stop. Ensure the template contains
an accessible focus target and manage its keyboard behavior. Disabled items,
separators, spacers, and invisible items are intentionally skipped.

### The toolbar has no accessible name

Set `AriaLabel`, or provide `aria-labelledby` through `HtmlAttributes`. For
icon-only items, set `Text`; do not use a tooltip as the only label.

### Nested menu does not open from the primary click

DropDown/Menu items open from the main button, but Split opens from its
chevron. Supply nested `Items` or `ChildContent`; nested menu support is one
level deep. Check whether `OnItemClick` sets `Cancel`.

## Sidebar

### Sidebar did not close on navigation

Confirm `CloseOnNavigate` is true and `@bind-IsOpen` is wired. The component
subscribes to location changes only when `IsOpenChanged` has a delegate. A
hash-only change may not emit the navigation event used by the component.

### Sidebar does not change mode at the expected width

The component checks viewport width, not the sidebar's parent container.
`Responsive` must be true, and the threshold uses a strict below comparison.
The initial viewport read does not raise `OnBreakpointChanged`.

### Sidebar remains closed after widening the viewport

This is intentional. Crossing into mobile behavior closes an open panel;
crossing back does not reopen it. Reopen it explicitly or update the bound
state in your application.

### Escape or backdrop does not close the panel

Check `IsEnabled`, `CloseOnEscape`, `ShowBackdrop`, and
`CloseOnOverlayClick`. Backdrop dismissal only applies to an Overlay sidebar
with a rendered backdrop.

### Focus does not move into a modal panel

Modal focus behavior requires an interactive render and an Overlay sidebar
with `ShowBackdrop="true"`. Ensure the page did not remove the panel before the
focus request and use `FocusAsync()` only after render.

### Sidebar dimensions fall back to defaults

`Width` and `MiniWidth` accept safe CSS length syntax. Quotes, declaration
separators, comments, `url()` and `expression()` are rejected. Use values such
as `280px`, `24rem`, or a safe `calc()` expression.

## General Integration Checks

- Confirm the Toolkit namespace, package, stylesheet/theme, and render mode
  are set up in the host application.
- Use an interactive render mode for events, keyboard handling, focus
  management, media/viewport observation, or browser-measured toolbar layout.
- Name each toolbar and Sidebar. For icon-only toolbar items, provide `Text`;
  for a Sidebar trigger, keep `aria-controls` and `aria-expanded` synchronized.
- Distinguish parent-controlled Sidebar updates from component-driven state
  changes: only the latter raise the open/close callbacks.
- Treat the Sidebar breakpoint as a viewport width threshold, not a responsive
  container query. The initial measurement is not a crossing notification.
- For toolbar Popup mode, verify that items are measurable and that the
  overflow control is enabled. For Scrollable mode, verify that content
  actually exceeds the available main-axis size.
