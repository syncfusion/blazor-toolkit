# Getting Started with Navigation Components

Use the Toolkit namespace and place the component in the page where the
navigation surface belongs. Both components can render in Static SSR. Choose an
interactive render mode when the page needs their event-driven behavior.

## Setup

```razor
@using Syncfusion.Blazor.Toolkit
```

Ensure the application has the Toolkit package and its normal stylesheet/theme
setup. These components emit `SfThemeRoot` themselves; do not add one per
component or copy the component's CSS into application styles.

## Render Modes

| Component/behavior | Static SSR | Interactive render mode |
|---|---|---|
| Toolbar structure, labels, item content | Renders | Renders |
| Toolbar item click and toggle state | No event delivery | Available |
| Toolbar roving keyboard focus | No event delivery | Available |
| Toolbar Popup overflow measurement and popup placement | Not measured | Available |
| Toolbar Scrollable controls and scroll state | Not measured | Available |
| Sidebar initial markup from parameters | Renders | Renders |
| Sidebar responsive viewport/media-query observation | Not observed | Available |
| Sidebar overlay focus trap and focus restoration | Not active | Available |
| Sidebar click, Escape, gesture, and binding events | No event delivery | Available |

Interactive behavior requires an interactive page or an interactive component
render mode supported by the host. A server-pre-rendered component initially
renders from its parameters; browser-derived state is reconciled after the
interactive render.

## Minimal Toolbar

```razor
<SfToolbar AriaLabel="Main commands">
    <ToolbarItem Text="Save" IconCss="e-icons e-save" />
    <ToolbarSeparator />
    <ToolbarItem Text="Undo" IconCss="e-icons e-undo" />
</SfToolbar>
```

`Popup` is the default overflow mode. Popup overflow requires browser-side
measurement; use `OverflowMode="ToolbarOverflowMode.Scrollable"` for a
single-line scrolling bar, or `MultiRow`/`Extended` for wrapping layouts.

## Minimal Sidebar

```razor
<SfSidebar AriaLabel="Primary navigation"
           @bind-IsOpen="_open"
           Mode="SidebarMode.Push"
           Responsive="false">
    <ChildContent>
        <nav aria-label="Primary">
            <a href="/">Home</a>
        </nav>
    </ChildContent>
    <TargetContent>
        <main>Page content</main>
    </TargetContent>
</SfSidebar>

@code {
    private bool _open;
}
```

Use `TargetContent` for content that should participate in Push or Shrink
layout. Overlay can render without a target region.

## Essential Interaction Notes

Toolbar item clicks and keyboard behavior require an interactive render mode.
The toolbar is one tab stop: use orientation-aware arrow keys, Home, and End to
navigate enabled items. Popup overflow and Scrollable controls need interactive
browser measurement; Static SSR emits only the initial markup. Give the toolbar
an accessible name with `AriaLabel` or `aria-labelledby`, and give every
icon-only `ToolbarItem` a `Text` value.

Sidebar state can be managed with `@bind-IsOpen` or the `OpenAsync`,
`CloseAsync`, and `ToggleAsync` methods. Component-driven state changes call
`IsOpenChanged` before `OnOpened`/`OnClosed`; a parent parameter change does
not invoke those events. By default, a viewport below 768px changes the
effective mode to Overlay and closes an open panel. Widening does not
automatically reopen it. Automatic close on navigation requires the bound
`IsOpenChanged` callback, normally supplied by `@bind-IsOpen`.

Name Sidebar with `AriaLabel` or `AriaLabelledBy`. An open Overlay sidebar
with an active backdrop behaves as a modal dialog and manages focus; ensure
the external trigger has matching `aria-controls` and `aria-expanded`.
