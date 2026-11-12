# Sidebar Responsive Behavior

By default, `Responsive` is enabled and `Breakpoint` is `SidebarBreakpoint.Md`
(768px). The component observes the browser viewport after interactive render.

## Breakpoint Values

| Value | Threshold | Behavior below threshold |
|---|---:|---|
| `None` | none | Configured mode remains active at every width |
| `Xs` | 0px | Configured mode remains active at every width |
| `Sm` | 576px | Mobile behavior below 576px |
| `Md` | 768px | Mobile behavior below 768px |
| `Lg` | 992px | Mobile behavior below 992px |
| `Xl` | 1200px | Mobile behavior below 1200px |
| `Xxl` | 1400px | Mobile behavior below 1400px |
| `Always` | none | Configured mode remains active at every width |

Mobile behavior means effective `Overlay` mode and an initially closed panel.
When a crossing makes the viewport mobile, an open sidebar closes. A later
crossing back to desktop does not automatically reopen it. The threshold
compares viewport width to the exact pixel value, not a range or parent
container size.

```razor
<SfSidebar @bind-IsOpen="_open"
           Mode="SidebarMode.Push"
           Responsive="true"
           Breakpoint="SidebarBreakpoint.Lg"
           OnBreakpointChanged="HandleBreakpointChanged">
    ...
</SfSidebar>
```

`OnBreakpointChanged` reports crossings after the first interactive render;
the initial viewport state is not itself a crossing event. A breakpoint or
Responsive parameter change at a fixed width should not be treated as a
crossing notification.

When a crossing moves below the threshold, `OnBreakpointChanged(true)` runs
before the component closes an open sidebar through its ordinary state-change
pipeline. At or above the threshold, a real crossing reports `false`, but
does not reopen the panel. `OnBreakpointChanged` is independent of the
`IsOpenChanged` and `OnClosed` callbacks.

## MediaQuery

The separate `MediaQuery` parameter watches a CSS media query in the browser.
A match requests open; a non-match requests closed. It is not the same as
`Responsive`/`Breakpoint`. The watcher starts after render, runs only where
`matchMedia` is available, and is disposed when the query changes or the
component is disposed. The initial query result is applied through the normal
state pipeline with the API trigger.

## Navigation

`CloseOnNavigate` defaults to true, but the component subscribes to
`NavigationManager.LocationChanged` only when `IsOpenChanged` has a delegate.
Use `@bind-IsOpen` to enable that binding callback and navigation-close
subscription. Hash-only navigation does not necessarily raise
`LocationChanged`; test the actual navigation type used by the app. A
navigation-driven close is identified with `SidebarTrigger.Navigation`.
Setting `CloseOnNavigate="false"` prevents this subscription.

## RTL and Reduced Motion

Logical `Start`/`End` follow the effective direction. The browser environment is
read after interactive render. The component also observes reduced-motion
preference; transition callbacks complete without the configured delay when
reduced motion is requested.

## Render-Mode and Interaction Notes

Static SSR uses component parameters without evaluating viewport width or
media queries. Responsive and `MediaQuery` behavior starts after interactive
render; a test host without browser media-query support keeps parameter-driven
rendering.

`IsEnabled="false"` suppresses user-driven backdrop, Escape, hover, and swipe
actions, but not parameter/API changes or responsive/navigation closes. The
component does not reopen automatically after the viewport grows past the
breakpoint.

The sidebar supports `Overlay`, `Push`, and `Shrink` modes, and logical
`Start`/`End` positions follow RTL while physical `Left`/`Right` positions do
not. Reduced-motion preference removes the transition delay before
`OnOpened`/`OnClosed`.
