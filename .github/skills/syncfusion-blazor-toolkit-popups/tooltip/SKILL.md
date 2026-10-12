---
license: MIT
name: syncfusion-blazor-toolkit-tooltip
description: >
  Implement Syncfusion Blazor Toolkit SfTooltip — hover/focus/click triggered
  informational text with positioning, arrow indicators, animation, and ARIA
  (role="tooltip", aria-describedby).
  USE FOR: hover help on icon buttons, keyboard-shortcut hints ("Ctrl+S"),
  brief explanatory text on hover/focus, automatic positioning, and rich
  content via `ContentTemplate`.
  DO NOT USE FOR: required user input or confirmations (use
  syncfusion-blazor-toolkit-dialog with `IsModal="true"`), forms or
  fields (use syncfusion-blazor-toolkit-inputs), or non-blocking progress
  overlays (use syncfusion-blazor-toolkit-notifications / SfSpinner).
metadata:
  author: "Syncfusion Inc"
  version: "1.0.0"
---

# Syncfusion Blazor Toolkit: Tooltip (SfTooltip)

`SfTooltip` displays contextual text or markup on hover, focus, or click.
It's portal-rendered, polite to interaction, and never blocks the underlying
page.

## Core Rules

1. **`OpensOn` is a string**, default `"Auto"` (hover or focus).
   Use `OpensOn="Hover"`, `"Focus"`, `"Click"`, or `"Custom"`, not an enum
   expression. Ensure click triggers also have keyboard-operable semantics.
2. **Use `Content` for text, `ContentTemplate` for markup.** `Content` is
   a string, not a `RenderFragment`; templates hold non-interactive markup.
3. **Tooltip never blocks the page.** It traps no focus and dismisses on
   `Escape`. Don't rely on it to confirm a destructive action.
4. **There is no `Visible` / `VisibleChanged` or `@bind-Visible` API.** For
   manual control use `OpensOn="Custom"` and await `OpenAsync()` / `CloseAsync()`
   after the component reference is assigned.
5. **Wrap `disabled` triggers with a focusable wrapper** — `<SfButton
   Disabled="true">` doesn't fire mouse/focus events. Wrap with a
   `<span tabindex="0">` to attach the tooltip, or render a fallback
   explanation adjacent to the disabled control.
6. **Don't put custom focus traps in tooltips** — they defeat the
   WCAG `role="tooltip"` semantics. The trigger keeps focus.
7. **`Position` is one of 12 named slots** (TopLeft, TopCenter, …,
   BottomRight). Stick to center slots; corner slots collide more often.
8. **`Container` constrains the portal.** Pair with `WindowCollision="true"`
   to keep the tooltip on-screen near page edges.

## Minimal Example

Use an interactive host for tooltip behavior. Other snippets are configuration
fragments unless they declare all referenced state and handlers.

```razor
@using Syncfusion.Blazor.Toolkit
@using Syncfusion.Blazor.Toolkit.Popups
@using Syncfusion.Blazor.Toolkit.Buttons

<SfTooltip Target="#saveBtn" Content="Save (Ctrl+S)"
           Position="Position.TopCenter"
           OpensOn="Auto"
           ShowTipPointer="true">
    <SfButton id="saveBtn">Save</SfButton>
</SfTooltip>
```

For trigger composition (disabled control, Custom `OpensOn`, `ContentTemplate`)
see the **Common Patterns** section below.

## When to Use Tooltip vs Dialog

If you're unsure, read the cross-skill decision matrix first:
[router/dialog-vs-tooltip-decision-matrix.md](../router/dialog-vs-tooltip-decision-matrix.md).

## Documentation and Navigation Guide

| Need | Read |
|---|---|
| Setup, properties, all triggers, RTL, full examples | [references/tooltip-implementation.md](references/tooltip-implementation.md) |

## Common Patterns

### Pattern 1 — Help icon / keyboard shortcut

```razor
@using Syncfusion.Blazor.Toolkit
@using Syncfusion.Blazor.Toolkit.Popups

<SfTooltip Target="#saveBtn" Content="Save (Ctrl+S)">
    <SfButton id="saveBtn">Save</SfButton>
</SfTooltip>
```

### Pattern 2 — HTML content template

```razor
@using Syncfusion.Blazor.Toolkit
@using Syncfusion.Blazor.Toolkit.Popups

<div id="custom">
    <SfTooltip Position="Position.BottomCenter" Target="#btn"
               WindowCollision="true">
        <ContentTemplate>
            <p>HTML inside a <strong>tooltip</strong>.</p>
        </ContentTemplate>
        <ChildContent>
            <button id="btn" type="button">HTML Tooltip</button>
        </ChildContent>
    </SfTooltip>
</div>
```

### Pattern 3 — Disabled control with focusable wrapper

```razor
@using Syncfusion.Blazor.Toolkit
@using Syncfusion.Blazor.Toolkit.Popups

<SfTooltip Target="#submitDisabledWrap" Content="Form is not valid">
    <span id="submitDisabledWrap" tabindex="0">
        <SfButton Disabled="true">Submit</SfButton>
    </span>
</SfTooltip>
```

### Pattern 4 — Programmatic Open / Close (Custom mode)

```razor
@using Syncfusion.Blazor.Toolkit.Popups

<SfTooltip @ref="tt" Content="Manually controlled" OpensOn="Custom">
    <button type="button" @onclick="@(() => tt.OpenAsync())">Show tooltip</button>
</SfTooltip>
<button type="button" @onclick="@(() => tt.CloseAsync())">Hide tooltip</button>

@code {
    private SfTooltip tt = default!;
}
```

## Key Properties & Events

| Group | Members |
|---|---|
| Content | `Content` (string), `ContentTemplate` (RenderFragment), `ChildContent` (trigger markup) |
| Trigger | `Target` (selector), `OpensOn` (string: Auto/Hover/Focus/Click/Custom), `OpenDelay`/`CloseDelay` (ms) |
| Position | `Position` (enum, default `TopCenter`), `TipPointerPosition`, `ShowTipPointer`, `WindowCollision`, `TargetContainer`, `Container`, `OffsetX`/`OffsetY`, `IsSticky`, `MouseTrail` |
| Animation | `AnimationModel` with `Open` / `Close` (`TooltipAnimationSettings`) and `Effect` enum values |
| Events | `OnOpen` / `Opened`; `OnClose` / `Closed` (`TooltipEventArgs`) |
| Methods (`@ref`) | `OpenAsync(string? handleId = null, TooltipAnimationSettings? animation = null)`, `CloseAsync(TooltipAnimationSettings? animation = null)`, `RefreshAsync()`, `RefreshPositionAsync(ElementReference? target = null)`; each returns `Task` |

There is no `TargetSelector`, `TooltipPosition`, or visibility binding. Source:
`src/Components/Popups/Tooltip/SfTooltip.razor.{Members,Events,Methods}.cs`.

## Accessibility

| Need | Implementation |
|---|---|
| `aria-describedby` on the trigger | Set automatically when `Target` resolves |
| Keyboard focus | Trigger element receives focus; tooltip reads when focused |
| Esc dismiss | Default |
| Disabled buttons | Wrap in focusable `<span tabindex="0">` — see Pattern 3 |

> ⚠️ Tooltip content is informational (`role="tooltip"`, not `role="alert"`); this does not mean its behavior works in Static SSR. For async progress use [SfSpinner](../../syncfusion-blazor-toolkit-notifications/SKILL.md) with appropriate live-region semantics.

## Decision Tree

Use this routing table alongside the Don'ts below — the tree answers
"which trigger / mode should I pick"; the Don'ts answer "what *not* to do".

```
Need hover help?
├─ On a focusable control?
│  └─ Plain <SfTooltip> wraps the trigger directly
├─ On a disabled control?
│  └─ Wrap in <span tabindex="0"> or render fallback copy
├─ On a non-focusable element (<p>)?
│  └─ OpensOn="Click" + role="button" + tabindex="0"
└─ Shows HTML or rich formatting?
   └─ <ContentTemplate> (Content is string-only)
```

## Don'ts

| Anti-pattern | Symptom | Fix |
|---|---|---|
| Use `SfTooltip` for confirmation ("Are you sure?") | Tooltip can't block; user can click past it; both gestures succeed | Use `SfDialog` with `IsModal="true"` (load `syncfusion-blazor-toolkit-dialog`) |
| `<SfButton Disabled="true">` wrapped directly | Tooltip never appears (disabled control fires no events) | Wrap in `<span tabindex="0">`; or render fallback copy adjacent to the disabled control |
| Long content (paragraphs, tables) inside `Content` | Overflow + overflowed tooltip with broken scroll affordance | Use a popover or layout-bounded panel; tooltips are 1–4 lines |
| `OpensOn="Click"` on a `<button>` trigger | Two interactive gestures compete — click action fires before tooltip opens | Use `OpensOn="Auto"` for buttons; reserve `Click` for non-button triggers (`<span>`, `<p>`) |
| Two `Effect` values: `Fade` + `Slide` on same tooltip | Animation queue races; either _bad_ both render | Pick exactly one effect |
| `Role="dialog"` / `role="alertdialog"` in `ContentTemplate` | Tooltip loses `role="tooltip"`; ARIA mis-announced as modal | Allow default `role="tooltip"`; for visual emphasis, use CSS not roles |
| `Id` collision on `Target="#x"` when modal also `#x` | Tooltip finds the wrong node (modal backdrop); wrong arrow direction | Use unique IDs per page section |
| `Container="body"` + `WindowCollision="true"` with custom scrollable parent | Tooltip overflows scroll container; disappears when parent scrolls | Either set `Container` to the scrollable parent, or accept that page-level scroll will hide the tooltip |
| `OnOpen="async void OnOpen(...) "` | Exceptions silently escape the Blazor dispatcher | `OnOpen="async Task OnOpen(...)"` |

## Anti-Pattern Workflows

### Workflow 1 — Agent tries to put a tooltip on a disabled save button

**Anti-pattern:**
```razor
@using Syncfusion.Blazor.Toolkit
@using Syncfusion.Blazor.Toolkit.Popups

<SfTooltip Content="Please fill the form">
    <SfButton Disabled="true">Save</SfButton>
</SfTooltip>
```

**Fix:**
```razor
@using Syncfusion.Blazor.Toolkit
@using Syncfusion.Blazor.Toolkit.Popups

<!-- Target the focusable wrapper rather than the disabled button. -->
<SfTooltip Content="Please fill the form" OpensOn="Auto" Target="#disabledSaveHelp">
    <span id="disabledSaveHelp" tabindex="0" aria-disabled="true" class="disabled-wrapper">
        <SfButton Disabled="true">Save</SfButton>
    </span>
</SfTooltip>
```

Or — **preferred** — render the explanation *visibly* in the form validation
summary instead.

### Workflow 2 — Agent wants long copy in the tooltip

**Anti-pattern:** paragraphs and tables inside `Content`. Tooltips aren't
scrollable; long content overflows visually.

**Fix:** either break it down into multiple tooltips (one per concept), or
delegate to a Popover/Popup. For help text needing richer layout, escalate
to `SfDialog` with `IsModal="false"`:

```razor
@using Syncfusion.Blazor.Toolkit
@using Syncfusion.Blazor.Toolkit.Popups

<!-- ❌ Tooltip overflow -->
<SfTooltip Content="Click to start the workflow, then select the customer, then…">
    <SfButton>?</SfButton>
</SfTooltip>

<!-- ✅ Link out to modal help -->
<SfButton OnClick="@OpenHelp" CssClass="e-flat">?</SfButton>
@code { private void OpenHelp() { /* render SfDialog IsModal="false" with full help */ } }
```

### Workflow 3 — Use the source animation model

`Animation` takes `AnimationModel`, whose `Open` and `Close` values are
`TooltipAnimationSettings`. `TooltipAnimation` / `AnimationEffect` are not
this component's model/enum.

```razor
@using Syncfusion.Blazor.Toolkit
@using Syncfusion.Blazor.Toolkit.Popups

<SfTooltip Content="Help" Animation="animation">
    <button type="button">Target</button>
</SfTooltip>

@code {
    private AnimationModel animation = new()
    {
        Open = new TooltipAnimationSettings { Effect = Effect.FadeIn, Duration = 200 },
        Close = new TooltipAnimationSettings { Effect = Effect.FadeOut, Duration = 200 }
    };
}
```

### Workflow 4 — Agent wants to *prevent* focus when the trigger is a button

**Bad:**
```razor
@using Syncfusion.Blazor.Toolkit
@using Syncfusion.Blazor.Toolkit.Popups

<SfTooltip Content="Delete this" OpensOn="Hover">
    <SfButton OnClick="Delete">Delete</SfButton>
</SfTooltip>
```

User keyboard-tabs to button → tooltip won't show (`Hover` only fires on
pointer enter).

**Fix:** keep `OpensOn="Auto"` (default), or — if `Hover` was intentional —
add a static `:focus` CSS style and roll your own focus announcement.

### Workflow 5 — Refresh targets after DOM updates

`Target` is a CSS selector (`"#other"`, not an element ID without `#`).
The component tracks parameter changes. When application rendering adds or
replaces target DOM, call `RefreshAsync()` after that render; use
`RefreshPositionAsync()` when only the position needs recalculation.

Interactive fragment showing a post-render refresh:

```razor
@using Syncfusion.Blazor.Toolkit.Popups

<button type="button" @onclick="AddTarget">Add target</button>
<SfTooltip @ref="tt" Target=".help-target" Content="Help">
    <ChildContent>
        <button class="help-target" type="button">First</button>
        @if (showSecond)
        {
            <button class="help-target" type="button">Second</button>
        }
    </ChildContent>
</SfTooltip>

@code {
    private SfTooltip? tt;
    private bool showSecond;
    private bool refreshPending;

    private void AddTarget() { showSecond = true; refreshPending = true; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (refreshPending && tt is not null)
        {
            refreshPending = false;
            await tt.RefreshAsync();
        }
    }
}
```

## Next Steps

1. **First tooltip:** read
   [tooltip-implementation.md](references/tooltip-implementation.md).
2. **Modal confirm / block UI:** exit this skill and load
   [syncfusion-blazor-toolkit-dialog](../dialog/SKILL.md).
3. **In doubt:** read
   [router/dialog-vs-tooltip-decision-matrix.md](../router/dialog-vs-tooltip-decision-matrix.md).

**Demo:** Syncfusion Blazor Toolkit official demo at
https://blazor.syncfusion.com/demos/toolkit/tooltip
