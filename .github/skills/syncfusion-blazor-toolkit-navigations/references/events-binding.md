# Navigation Events and Binding

## Toolbar

### Item Click Order

For a toolbar item activation:

1. `ToolbarItem.OnClick` runs.
2. `SfToolbar.OnItemClick` runs with `ToolbarItemClickEventArgs`.
3. Unless `Cancel` is true, the default item behavior occurs: nested menu open
   or selectable state toggle.

`OnItemClick` still runs when `Cancel` is set. Use cancellation to prevent the
default toggle/open action, not to suppress the callback.

```razor
<SfToolbar AriaLabel="Editing"
           OnItemClick="HandleToolbarClick">
    <ToolbarItem Text="Bold"
                 Type="ToolbarItemType.Toggle"
                 @bind-Selected="_bold"
                 OnClick="LogClick" />
</SfToolbar>

@code {
    private bool _bold;
    private void LogClick(MouseEventArgs args) { }

    private void HandleToolbarClick(ToolbarItemClickEventArgs args)
    {
        if (args.ItemId == "locked-command")
        {
            args.Cancel = true;
        }
    }
}
```

The callback carries `ItemId`, the bound `ToolbarItemModel` when data-bound,
`OriginalArgs`, and `Cancel`. `OnOverflowOpen` and `OnOverflowClose` are
parameterless and run once for each actual popup state transition. Extended
mode's expand/collapse control does not open the overflow menu.

### Selected Binding

Use either `@bind-Selected` or explicit `Selected`/`SelectedChanged`.
`Toggle` items with the same non-empty `GroupName` are mutually exclusive;
`Check` items can be selected independently. A click on a selected radio-style
Toggle does not clear it.

## Sidebar

`@bind-IsOpen` is shorthand for `IsOpen` and `IsOpenChanged`. For
user/API/navigation/breakpoint changes, state is applied, `IsOpenChanged` is
invoked if configured, and then `OnOpened` or `OnClosed` is raised. A
parent-driven parameter change updates component state but does not invoke
those callbacks.

```razor
<SfSidebar @bind-IsOpen="_open"
           OnOpened="HandleOpened"
           OnClosed="HandleClosed"
           OnBreakpointChanged="HandleBreakpoint">
    ...
</SfSidebar>
```

`SidebarEventArgs` includes the applied `IsOpen`, effective `Mode`, and
`Trigger`. Trigger values include API, backdrop, keyboard, gesture, and
navigation. `OnBreakpointChanged(bool)` reports whether the viewport is below
the breakpoint and is independent of opened/closed events.

`OpenAsync`, `CloseAsync`, and `ToggleAsync` are no-ops when the requested state
is already active. With animation enabled, global animation on, and no reduced
motion, opened/closed callbacks occur after `Duration`; a newer transition
cancels a pending callback.

`SidebarEventArgs` contains the applied `IsOpen` value, the effective `Mode`,
and `Trigger` (`Api`, `Backdrop`, `Keyboard`, `Gesture`, or `Navigation`).
`OnBreakpointChanged(bool)` reports `true` below the breakpoint and `false`
when a crossing returns to desktop width. The initial viewport measurement is
not a crossing and does not raise this event. On entering mobile behavior,
`OnBreakpointChanged(true)` runs before the `IsOpenChanged(false)` and
`OnClosed` sequence caused by closing the panel.

## Parameters That Affect Event Delivery

- `CloseOnEscape` defaults to true. Escape closes an open sidebar when user
  interactions are enabled.
- `CloseOnOverlayClick` defaults to true and applies only to an active Overlay
  backdrop.
- `IsEnabled="false"` suppresses user backdrop, Escape, hover, and gesture
  actions, but not parent/API changes or responsive/navigation closing.
- `CloseOnNavigate` defaults to true. A location subscription is made only
  when `IsOpenChanged` has a delegate, and is removed when no longer needed or
  on disposal. Hash-only navigation may not raise the location event.
- `MediaQuery` changes use the normal state pipeline and the API trigger.

## Lifecycle Timing

Component-driven state is applied before its callbacks. Parent-driven
parameter updates synchronize rendering without invoking the component's
open/close callbacks. When animation is active, `OnOpened` and `OnClosed` wait
for `Duration`; disabled animation or reduced-motion preference makes them
immediate.
