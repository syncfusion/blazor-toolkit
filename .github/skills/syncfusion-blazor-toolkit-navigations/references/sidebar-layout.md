# Sidebar Modes and Layout

`Mode` determines how an open sidebar relates to its target content.

## Modes

| Mode | Layout |
|---|---|
| `Overlay` | Panel sits over the target; a backdrop is shown when `ShowBackdrop` is true |
| `Push` | Panel sits beside the target and shifts it aside |
| `Shrink` | Panel sits beside the target and reduces the target's available width |

For Push and Shrink, provide `TargetContent` to receive the layout change.
`Fixed="true"` (default) positions the panel against the viewport; with
`Fixed="false"`, it is positioned inside the layout host. The backdrop follows
the same positioning reference.

`Responsive` defaults to true. At viewport widths below `Breakpoint`, the
effective mode becomes Overlay and the sidebar closes if open. Set
`Responsive="false"` to keep the configured mode at all widths.

## Position

`Start` and `End` are logical positions and follow RTL. `Left` and `Right` are
physical edges and do not change with text direction.

```razor
<SfSidebar Position="SidebarPosition.End"
           Mode="SidebarMode.Shrink"
           Responsive="false">
    <ChildContent>...</ChildContent>
    <TargetContent>...</TargetContent>
</SfSidebar>
```

## Backdrop and Dismissal

- `ShowBackdrop` applies only to an open Overlay sidebar. With a backdrop, the
  panel has modal dialog semantics and the target is inert.
- `CloseOnOverlayClick` controls backdrop activation only.
- `CloseOnEscape` independently controls Escape dismissal.
- `IsEnabled="false"` ignores backdrop, Escape, hover expansion, and swipe
  gestures; it does not prevent parent or public-method state changes.

## Mini Rail and Animation

`EnableMini` keeps a narrow rail visible while the panel is closed.
`MiniWidth` sets that rail's width. `OpenMiniOnHover` expands the closed rail
under mouse/pen hover when enabled and not in mobile behavior; it does not
change `IsOpen` and raises no state events. Provide a separate keyboard
accessible toggle or use the public methods.

`Duration` is a non-negative transition time in milliseconds. The parameter
defaults to 300ms. `EnableAnimation="false"`, global animation disablement, or
the user's reduced-motion preference removes the wait before opened/closed
callbacks.

## Responsive Thresholds

| `Breakpoint` | Pixel threshold |
|---|---:|
| `None`, `Xs`, `Always` | No mobile threshold |
| `Sm` | 576 |
| `Md` | 768 |
| `Lg` | 992 |
| `Xl` | 1200 |
| `Xxl` | 1400 |

The test is strictly below the threshold and is based on viewport width, not
the containing element. The initial viewport is used to establish behavior,
not reported as an `OnBreakpointChanged` crossing. Entering mobile behavior
raises `OnBreakpointChanged(true)` before any close state callbacks; widening
does not reopen the sidebar.

## Events and Accessibility

For component-driven changes, state is applied, `IsOpenChanged` runs, then
`OnOpened` or `OnClosed`. `SidebarEventArgs` carries `IsOpen`, effective
`Mode`, and a `Trigger` (`Api`, `Backdrop`, `Keyboard`, `Gesture`, or
`Navigation`). Parent-driven changes do not raise these callbacks. Animated
callbacks wait `Duration` unless animation is disabled or reduced motion is
requested; a newer state change cancels a pending callback.

An Overlay sidebar with an active backdrop is modal and uses dialog semantics.
It moves focus into the panel, contains Tab/Shift+Tab, makes target content
inert, and restores focus on close. Push/Shrink modes are non-modal. Name the
panel with `AriaLabel` or `AriaLabelledBy`; provide the external trigger's
`aria-controls` and synchronized `aria-expanded`.
