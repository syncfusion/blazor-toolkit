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
  version: "1.0.0"   # skill document revision only — not the NuGet package version (package is 2.x)
---

# Syncfusion Blazor Toolkit: Tooltip (SfTooltip)

`SfTooltip` displays contextual text or markup on hover, focus, or click.
It's portal-rendered, polite to interaction, and never blocks the underlying
page.

## Core Rules

1. **Default trigger is `Auto` (hover or focus)** — `OpensOn="Auto"`.
   `OpensOn` is a **string** (`"Auto"`, `"Hover"`, `"Click"`, `"Focus"`, `"Custom"`),
   not an enum — there is no `OpensOn.Auto` type. Use `Hover` for mouse-only aesthetics, `Focus` for keyboard-only flows,
   `Click` for non-interactive triggers (`<p>`, `<span>`).
2. **Use `Content` for text, `ContentTemplate` for markup** — never wrap HTML
   strings in `Content`. Templates also accept `RenderFragment` so you can
   keep rich content inside the tooltip.
3. **Tooltip never blocks the page.** It traps no focus and dismisses on
   `Escape` automatically. Don't rely on it to *confirm* a destructive
   action.
4. **Programmatic control uses `OpenAsync` / `CloseAsync`** — the hover/focus
   lifecycle handles visibility automatically. Call `OpenAsync()` /
   `CloseAsync()` through a `@ref` only with `OpensOn="Custom"`. There is no
   `Show()` / `Hide()` method and no `Visible` / `@bind-Visible` parameter.
5. **Wrap `disabled` triggers with a focusable wrapper** — `<SfButton
   Disabled="true">` doesn't fire mouse/focus events. Wrap with a
   `<span tabindex="0">` to attach the tooltip, or render a fallback
   explanation adjacent to the disabled control.
6. **Don't put custom focus traps in tooltips** — they defeat the
   WCAG `role="tooltip"` semantics. The trigger keeps focus.
7. **`Position` is a value of the `Position` enum** (e.g.
   `Position="Position.TopCenter"`; TopLeft, TopCenter, …, BottomRight). There
   is no `TooltipPosition` type. Stick to center slots; corner slots collide more often.
8. **`Container` constrains the portal.** Pair with `WindowCollision="true"`
   to keep the tooltip on-screen near page edges.

## Minimal Example

```razor
@using Syncfusion.Blazor.Toolkit

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
[router/dialog-vs-tooltip-decision-matrix.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-popups/router/dialog-vs-tooltip-decision-matrix.md).

## Documentation and Navigation Guide

| Need | Read |
|---|---|
| Setup, properties, all triggers, RTL, full examples | [references/tooltip-implementation.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-popups/tooltip/references/tooltip-implementation.md) |

## Common Patterns

### Pattern 1 — Help icon / keyboard shortcut

```razor
@using Syncfusion.Blazor.Toolkit

<SfTooltip Target="#saveBtn" Content="Save (Ctrl+S)">
    <SfButton id="saveBtn">Save</SfButton>
</SfTooltip>
```

### Pattern 2 — HTML content template

```razor
@using Syncfusion.Blazor.Toolkit

<SfTooltip Position="Position.BottomCenter"
            Target="#btn"
            WindowCollision="true"
            TargetContainer="#custom">
    <ContentTemplate>
        <div class="tooltip-html">
            <p>HTML inside a <strong>tooltip</strong>.</p>
            <SfButton Content="Inner button" />
        </div>
    </ContentTemplate>
    <Target>
        <SfButton id="btn" Content="HTML Tooltip" />
    </Target>
</SfTooltip>
```

### Pattern 3 — Disabled control with focusable wrapper

```razor
@using Syncfusion.Blazor.Toolkit

<SfTooltip Target="#submitDisabledWrap" Content="Form is not valid">
    <span id="submitDisabledWrap" tabindex="0">
        <SfButton Disabled="true">Submit</SfButton>
    </span>
</SfTooltip>
```

### Pattern 4 — Programmatic open / close (Custom mode)

```razor
@using Syncfusion.Blazor.Toolkit

<SfTooltip @ref="tt" Content="Manually controlled" OpensOn="Custom">
    <SfButton id="manualBtn" @onclick="OpenTooltipAsync">Show tooltip</SfButton>
    <SfButton @onclick="CloseTooltipAsync">Hide tooltip</SfButton>
</SfTooltip>

@code {
    private SfTooltip? tt;

    private Task OpenTooltipAsync() => tt is null ? Task.CompletedTask : tt.OpenAsync("manualBtn");
    private Task CloseTooltipAsync() => tt is null ? Task.CompletedTask : tt.CloseAsync();
}
```

## Key Properties & Events

| Group | Members |
|---|---|
| Content | `Content` (string/RenderFragment), `ContentTemplate` (RenderFragment); use `Content` for text, `ContentTemplate` for HTML |
| Trigger | `Target` (CSS selector), `OpensOn` (string: `"Auto"`/`"Hover"`/`"Focus"`/`"Click"`/`"Custom"`), `OpenDelay`/`CloseDelay` (ms) |
| Position | `Position` (12-slot enum, default `TopCenter`), `TipPointerPosition`, `ShowTipPointer`, `WindowCollision`, `TargetContainer`, `Container`, `OffsetX`/`OffsetY`, `IsSticky`, `MouseTrail` |
| Animation | `Animation` (`AnimationModel` with `Open` / `Close` `TooltipAnimationSettings { Effect, Duration, Delay }`; default `Effect.FadeIn` / `Effect.FadeOut`) |
| Events | `OnOpen` / `Opened` (before / after the tooltip opens); `OnClose` / `Closed` (before / after it closes); `OnRender`, `Colliding`, `Created`, `Destroyed` |
| Methods (`@ref`) | `OpenAsync(string? handleId = null, TooltipAnimationSettings? animation = null)` / `CloseAsync(TooltipAnimationSettings? animation = null)` / `RefreshAsync()` / `RefreshPositionAsync(ElementReference? target = null)` |

## Accessibility

| Need | Implementation |
|---|---|
| `aria-describedby` on the trigger | Set automatically when `Target` resolves |
| Keyboard focus | Trigger element receives focus; tooltip reads when focused |
| Esc dismiss | Default |
| Disabled buttons | Wrap in focusable `<span tabindex="0">` — see Pattern 3 |

> ⚠️ Tooltips are **static** (`role="tooltip"`, not `role="alert"`). Don't rely on them for async announcements — use [SfSpinner](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-notifications/SKILL.md) for that.

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
   └─ <ContentTemplate> or Content="RenderFragment"
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

<SfTooltip Content="Please fill the form">
    <SfButton Disabled="true">Save</SfButton>
</SfTooltip>
```

**Fix:**
```razor
@using Syncfusion.Blazor.Toolkit

<!-- Wrap disabled button in tab-able span; tooltip finds the wrapper -->
<SfTooltip Content="Please fill the form" OpensOn="Auto">
    <span tabindex="0" aria-disabled="true" class="disabled-wrapper">
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

<!-- ❌ Tooltip overflow -->
<SfTooltip Content="Click to start the workflow, then select the customer, then…">
    <SfButton>?</SfButton>
</SfTooltip>

<!-- ✅ Link out to modal help -->
<SfButton OnClick="@OpenHelp" CssClass="e-flat">?</SfButton>
@code { private void OpenHelp() { /* render SfDialog IsModal="false" with full help */ } }
```

### Workflow 3 — Agent invents `TooltipAnimation` / `AnimationEffect` types

**Bad (these types do not exist in the shipped API):**
```razor
<SfTooltip Content="…" Animation="@(new TooltipAnimation { Effect = AnimationEffect.Fade })" />
```

**Correct:** `Animation` is an `AnimationModel` with separate `Open` and `Close`
settings, each using the `Effect` enum:

```razor
@using Syncfusion.Blazor.Toolkit

<SfTooltip Content="…" Animation="@_animation">
    <span>Target</span>
</SfTooltip>

@code {
    private readonly AnimationModel _animation = new()
    {
        Open = new TooltipAnimationSettings { Effect = Effect.FadeIn, Duration = 200, Delay = 0 },
        Close = new TooltipAnimationSettings { Effect = Effect.FadeOut, Duration = 200, Delay = 0 }
    };
}
```

### Workflow 4 — Agent wants to *prevent* focus when the trigger is a button

**Bad:**
```razor
@using Syncfusion.Blazor.Toolkit

<SfTooltip Content="Delete this" OpensOn="Hover">
    <SfButton OnClick="Delete">Delete</SfButton>
</SfTooltip>
```

User keyboard-tabs to button → tooltip won't show (`Hover` only fires on
pointer enter).

**Fix:** keep `OpensOn="Auto"` (default), or — if `Hover` was intentional —
add a static `:focus` CSS style and roll your own focus announcement.

### Workflow 5 — Agent changes `Target` at runtime, expects tooltip to reposition

**Bad:**
```razor
@using Syncfusion.Blazor.Toolkit

<SfTooltip Target="@TargetId" Content="…">
    <SfButton>@currentLabel</SfButton>
</SfTooltip>
@code { private string TargetId = ""; private void Move() { TargetId = "other"; } }
```

`Target` resolves once at mount. Changing it without `RefreshAsync()` causes
the arrow to point at the old location.

**Fix:**
```razor
@using Syncfusion.Blazor.Toolkit

<SfTooltip @ref="tt" Target="@TargetId" Content="…">
    <SfButton>@currentLabel</SfButton>
</SfTooltip>
@code {
    private SfTooltip? tt;
    private async Task Move()
    {
        TargetId = "other";
        if (tt is not null) await tt.RefreshAsync();
    }
}
```

(Or — and this is the simpler route — use `<SfTooltip>` as a *wrapper*
of the trigger rather than a `Target=` reference, so positioning follows
the trigger DOM automatically.)
  silently

## Next Steps

1. **First tooltip:** read
   [tooltip-implementation.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-popups/tooltip/references/tooltip-implementation.md).
2. **Modal confirm / block UI:** exit this skill and load
   [syncfusion-blazor-toolkit-dialog](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-popups/dialog/SKILL.md).
3. **In doubt:** read
   [router/dialog-vs-tooltip-decision-matrix.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-popups/router/dialog-vs-tooltip-decision-matrix.md).

**Demo:** Syncfusion Blazor Toolkit official demo at
https://blazor.syncfusion.com/demos/toolkit/tooltip
