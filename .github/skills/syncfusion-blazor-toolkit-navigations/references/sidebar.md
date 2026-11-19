# Sidebar Component

`SfSidebar` is a collapsible side panel for navigation or contextual content.
It can overlay, push, or shrink adjacent content and supports an optional mini
rail.

## Table of Contents

- [Basic Usage](#basic-usage)
- [Content Regions](#content-regions)
- [Open State](#open-state)
- [Defaults](#defaults)
- [Accessible Operation](#accessible-operation)

## Basic Usage

```razor
<SfButton aria-controls="app-sidebar"
          aria-expanded="@_isOpen"
          OnClick="ToggleSidebar">
    Menu
</SfButton>

<SfSidebar Id="app-sidebar"
           AriaLabel="Primary navigation"
           @bind-IsOpen="_isOpen"
           Mode="SidebarMode.Push"
           Responsive="false"
           Fixed="false"
           Width="280px">
    <ChildContent>
        <nav aria-label="Primary">
            <a href="/">Home</a>
            <a href="/reports">Reports</a>
        </nav>
    </ChildContent>
    <TargetContent>
        <main>Page content</main>
    </TargetContent>
</SfSidebar>

@code {
    private bool _isOpen;
    private void ToggleSidebar() => _isOpen = !_isOpen;
}
```

## Content Regions

| Region | Purpose |
|---|---|
| `HeaderTemplate` | Optional panel header |
| `ChildContent` | Main sidebar body |
| `FooterTemplate` | Optional panel footer |
| `TargetContent` | Adjacent content for Push/Shrink layout |

Header and footer wrappers render only when their templates are supplied.
`TargetContent` is optional; without it, the component renders no target region.
When a modal overlay is active, target content is marked inert.

## Open State

Use `@bind-IsOpen` for two-way state. Or call `OpenAsync()`, `CloseAsync()`,
`ToggleAsync()`, and `FocusAsync()` through a component reference.

- A user/API/navigation/breakpoint state change calls `IsOpenChanged` (if
  configured) and then `OnOpened` or `OnClosed`.
- A parent-supplied `IsOpen` change updates rendering but raises neither
  `IsOpenChanged` nor the open/close events.
- Repeating an operation that does not change state does not raise duplicate
  state-change events.
- If animation is enabled and motion is not reduced, `OnOpened`/`OnClosed`
  follows the configured `Duration`; a newer transition cancels the pending
  callback.

`SidebarEventArgs` contains the resulting `IsOpen` state, effective `Mode`,
and `Trigger` (`Api`, `Backdrop`, `Keyboard`, `Gesture`, or `Navigation`).
`OnBreakpointChanged(bool)` is independent of open/close events and reports
viewport crossings only; it does not report the initial viewport measurement.

With `CloseOnNavigate="true"` (the default), a location-change subscription is
active only when `IsOpenChanged` has a delegate, usually through
`@bind-IsOpen`. Hash-only navigation might not raise the location event.

`IsEnabled="false"` disables user-initiated backdrop, Escape, hover, and swipe
interactions. It does not block public methods, parent-driven parameter
changes, responsive closing, or navigation closing.

## Defaults

| Parameter | Default |
|---|---|
| `IsOpen` | `false` |
| `Mode` | `Overlay` |
| `Position` | `Start` |
| `Width` / `MiniWidth` | `280px` / `64px` |
| `ShowBackdrop`, `CloseOnOverlayClick`, `CloseOnEscape` | `true` |
| `EnableGestures`, `Responsive`, `IsEnabled` | `true` |
| `Breakpoint` | `Md` (768px) |
| `CloseOnNavigate` | `true` |
| `EnableAnimation` / `Duration` | `true` / 300ms |
| `Fixed` | `true` |

`Responsive` is on by default. Turn it off when the configured `Mode` must
remain in effect at every viewport width.

## Accessible Operation

The default panel role is `navigation`, with a localized accessible name.
Supply `AriaLabel` or `AriaLabelledBy`; the latter takes precedence. An open
Overlay sidebar with a backdrop becomes a modal dialog, traps focus, makes
target content inert, and returns focus to the opener when closed. The opening
control is application-owned: give it `aria-controls` pointing to the panel
`Id` and keep `aria-expanded` synchronized.

`FocusAsync()` focuses the first focusable descendant, or the panel when none
exists. It does not open the sidebar and has no effect before rendering or
while closed without a mini rail.
