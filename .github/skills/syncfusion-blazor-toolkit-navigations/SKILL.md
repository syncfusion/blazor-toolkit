---
license: MIT
name: syncfusion-blazor-toolkit-navigations
description: >
  Implement Syncfusion Blazor Toolkit navigation components — SfToolbar and
  SfSidebar. USE FOR: responsive command bars, toolbar items and groups,
  priority-based overflow, keyboard navigation, collapsible side panels,
  overlay/push/shrink layouts, mini rails, responsive breakpoints, and
  navigation state/events. DO NOT USE FOR: application route menus or menu
  trees (use a dedicated menu component when available), tab navigation,
  dialogs unrelated to a navigation panel, or chart zoom toolbars.
compatibility: ".NET 8+, render-modes: Static SSR, Server, WebAssembly, Auto"
metadata:
  author: "Syncfusion Inc"
  version: "1.0.0"
---

# Syncfusion Blazor Toolkit - Navigation Components

> **Render modes:** `SfToolbar` renders static markup in Static SSR; interactive
> render modes enable item events, keyboard handling, scrolling, popup placement,
> and Popup-mode overflow measurement. `SfSidebar` also renders from parameters
> in Static SSR; viewport detection, media-query watching, modal focus behavior,
> and interactive state changes require an interactive render mode.

## Core Rules

1. **Give every toolbar an accessible name.** Set `AriaLabel` or provide a
   meaningful `aria-labelledby` through `HtmlAttributes`.
2. **Keep the toolbar's single-tab-stop pattern.** Use arrow keys, Home, and End
   to move among enabled, visible items; do not add custom tab stops to
   separators or spacers. Use `FocusAsync()` when focus must enter the toolbar.
3. **Choose toolbar overflow intentionally.** `Popup` is the default and moves
   non-fitting items into a menu; `Scrollable` keeps one scrollable line;
   `MultiRow` wraps; `Extended` collapses additional rows behind an expand
   control.
4. **Use `@bind-IsOpen` when Sidebar state must stay synchronized.** It enables
   `IsOpenChanged` and the navigation-close subscription. Parent-driven changes
   do not raise `OnOpened` or `OnClosed`.
5. **Treat Sidebar responsiveness as viewport behavior.** With `Responsive`
   enabled, a viewport below `Breakpoint` uses Overlay and closes an open
   sidebar; growing past the breakpoint does not reopen it.
6. **Keep accessibility labels separate from tooltips.** Toolbar `Text`
   remains the accessible name in icon-only mode; `Tooltip` is supplemental.
   Sidebar uses `AriaLabelledBy` in preference to `AriaLabel`.
7. **Use trusted values for styling/attribute inputs.** Do not populate
   `CssClass`, toolbar `HtmlAttributes`, or Sidebar `HtmlAttributes` from
   untrusted data. Sidebar `Width` and `MiniWidth` are sanitized CSS lengths.

## Don'ts

| Anti-pattern | Symptom | Fix |
|---|---|---|
| Rendering a toolbar without `AriaLabel` or `aria-labelledby` | Toolbar landmark has no accessible name | Set `AriaLabel` or an ID reference in `HtmlAttributes` |
| Adding every toolbar button to the page tab order | Keyboard users must tab through every command | Keep the component's roving focus behavior; use arrow keys |
| Using `Tooltip` as the only name for an icon-only toolbar item | Accessible name may be missing or inconsistent | Set `Text`; the component uses it as `aria-label` |
| Assuming `Responsive` measures a parent container | Sidebar switches at unexpected widths | It observes viewport width; use `Responsive="false"` if mode must not adapt |
| Expecting an unbound Sidebar to close automatically on route navigation | Open state can remain unchanged | Use `@bind-IsOpen` with `CloseOnNavigate="true"` |
| Expecting breakpoint growth to reopen Sidebar | It remains closed after returning to desktop width | Reopen explicitly or bind the state and handle the breakpoint event |
| Treating `IsEnabled="false"` as disabling API and parent control | Programmatic state changes still work | It disables user interactions only; guard API calls at the application level |
| Assuming `OnItemClick` runs before item `OnClick` | Application side effects occur in the wrong order | Item `OnClick` runs first; toolbar `OnItemClick` follows |

## Minimal Examples

### Toolbar

```razor
@using Syncfusion.Blazor.Toolkit

<SfToolbar AriaLabel="Document commands">
    <ToolbarGroup Label="Document">
        <ToolbarItem Text="Save"
                     IconCss="e-icons e-save"
                     OnClick="SaveAsync" />
        <ToolbarItem Text="Bold"
                     Type="ToolbarItemType.Toggle"
                     @bind-Selected="_isBold" />
    </ToolbarGroup>
    <ToolbarSeparator />
    <ToolbarSpacer />
    <ToolbarItem Text="More"
                 Type="ToolbarItemType.Menu">
        <Items>
            <ToolbarItem Text="Export" OnClick="ExportAsync" />
            <ToolbarItem Text="Print" OnClick="PrintAsync" />
        </Items>
    </ToolbarItem>
</SfToolbar>

@code {
    private bool _isBold;
    private Task SaveAsync() => Task.CompletedTask;
    private Task ExportAsync() => Task.CompletedTask;
    private Task PrintAsync() => Task.CompletedTask;
}
```

### Sidebar

```razor
@using Syncfusion.Blazor.Toolkit

<SfButton aria-controls="primary-navigation"
          aria-expanded="@_isOpen"
          OnClick="@(() => _isOpen = !_isOpen)">
    Navigation
</SfButton>

<SfSidebar Id="primary-navigation"
           AriaLabel="Primary navigation"
           @bind-IsOpen="_isOpen"
           Mode="SidebarMode.Push"
           Responsive="false"
           Fixed="false">
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
}
```

## Documentation and Navigation Guide

| Need | Read |
|---|---|
| Setup and render-mode behavior | [references/getting-started.md](references/getting-started.md) |
| Toolbar basics, items, templates, groups | [references/toolbar.md](references/toolbar.md), [references/toolbar-items-and-groups.md](references/toolbar-items-and-groups.md) |
| Toolbar overflow and orientation | [references/toolbar-overflow.md](references/toolbar-overflow.md) |
| Sidebar basics, modes, and responsive behavior | [references/sidebar.md](references/sidebar.md), [references/sidebar-layout.md](references/sidebar-layout.md), [references/sidebar-responsive.md](references/sidebar-responsive.md) |
| Events and state binding | [references/events-binding.md](references/events-binding.md) |
| Keyboard, ARIA, focus, and styling | [references/accessibility.md](references/accessibility.md) |
| Component parameters, methods, enums | [references/api-reference.md](references/api-reference.md) |
| Common integration errors | [references/critical-gotchas.md](references/critical-gotchas.md), [references/troubleshooting.md](references/troubleshooting.md) |

## Next Steps

1. **Read first:** [references/critical-gotchas.md](references/critical-gotchas.md).
2. **Choose a component:** begin with [references/toolbar.md](references/toolbar.md)
   or [references/sidebar.md](references/sidebar.md).
3. **Review details:** read the relevant API, interaction, accessibility, and
   troubleshooting references before generating component markup.

**Toolbar demos:** `samples/Blazor.Toolkit.Samples.Client/Pages/Demos/Navigations/Toolbar/`

**Sidebar demos:** `samples/Blazor.Toolkit.Samples.Client/Pages/Demos/Navigations/Sidebar/`
