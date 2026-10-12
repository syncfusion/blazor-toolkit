---
license: MIT
name: syncfusion-blazor-toolkit-notifications
description: >
  Implement Syncfusion Blazor Toolkit loading and activity indicators —
  SfSpinner.
  USE FOR: form submission and async-operation feedback, content loading
  decorations, overlay compositions with z-index stacking, status-region
  labeling via Label and AriaLabel, AriaLive settings, visibility control
  via @bind-Visible or VisibleChanged, cancelable open/close events, and
  custom visuals via nested SpinnerTemplates.Template.
  Initial visible markup can render in static SSR. Client-driven visibility,
  browser event handling, and JS interop require an interactive host (Server,
  WebAssembly, or Auto on .NET 8+). Body scroll locking is application-owned.
  DO NOT USE FOR: skeleton loaders, toast notifications, or determinate
  progress bars. SfSkeleton, SfToast, and SfProgressBar are not available
  in this Toolkit; they would require a separate library or application UI.
compatibility: ".NET 8+, render-modes: Server, WebAssembly, Auto"
metadata:
  author: "Syncfusion Inc"
  version: "1.0.0"
---

# Syncfusion Blazor Toolkit: Notifications & Loading Indicators

> ⚙️ **Render mode:** client-driven visibility, browser event handlers, and JS body scroll lock require an interactive host. `SfSpinner Visible="true"` can emit initial visible markup in static SSR; this does not enable client-side event handling. For server-streamed loading UI consider `[StreamRendering]`. Follow `AGENTS.md` if present and the repository's [Development Guide](../../DEVELOPMENT.md) for source-development guidance.

The Notifications components provide visual feedback during asynchronous operations and content loading scenarios. This skill guides you through implementing the **Spinner** component for indicating background processing.

Source authority: `src/Components/Spinner/SfSpinner.razor`, its `Members`,
`Events`, `LifeCycle`, and implementation partials, `ChildComponents/SpinnerTemplates.cs`,
and `src/wwwroot/styles/spinner.scss`.

## Core Rules

1. **Use `@bind-Visible` OR explicit `Visible` + `VisibleChanged`**, not both.
   Binding generates those parameters; duplicating them is a Razor error.
   Use `Visible` alone for one-way display controlled by the parent.
2. **`OnOpen` / `OnClose` are awaited in the show/hide path.** Setting
   `args.Cancel = true` skips that transition's CSS/state update. Initial
   visible markup does not depend on `OnOpen`; cancellation does not undo a
   parent state change or cancel the application's work.
3. **`Label` is optional visible text and can coexist with a custom visual.**
   Nest `<Template>` inside `<SpinnerTemplates>`; mark decorative custom
   visual nodes `aria-hidden="true"`. The root emits `role="status"`,
   `aria-busy`, `aria-live` (default `polite`), and `aria-label` from
   `AriaLabel` (fallback `Loading`). Test actual announcements; these
   attributes alone do not establish accessibility compliance.
4. **The default root is absolutely positioned**, with 100% width/height
   and top/left zero, despite its `inline-flex` display. Use a positioned,
   sized container for a region spinner; override root positioning via
   `CssClass` for inline or page-level compositions.
5. **Body scroll locking is application-owned**, not provided by `SfSpinner`.
   If using JS interop, acquire/release the lock on visibility transitions,
   restore prior styles, and plan for navigation and disconnected-host cleanup.
6. **`VisibleChanged` is `EventCallback<bool>`; open/close use
   `EventCallback<SpinnerEventArgs>`.** Use `Task`/`async Task` for async
   handlers, or `void` for purely synchronous handlers. Avoid `async void`
   and unawaited work; do not bind a `ValueTask` handler directly.
7. **Requesting a render is not guaranteeing a browser paint.** Normal
   Blazor event callbacks schedule renders; `InvokeAsync(StateHasChanged)`
   can marshal a render request from external notifications, but cannot
   force an immediate paint or make synchronous work non-blocking.
8. **A spinner is feedback, not an input lock.** Keep the submit action
   disabled while busy and preserve its accessible label. Prefer a label
   swap or an adjacent indicator to avoid positioning/announcement conflicts.

## Don'ts

| Anti-pattern | Symptom | Fix |
|---|---|---|
| `@bind-Visible` plus explicit `Visible` / `VisibleChanged` | Duplicate generated parameters; Razor error | Pick binding or explicit parameters |
| Using `SfSpinner` for confirmation ("are you sure?") | No confirmation input is collected | Use `SfDialog`; show the spinner during the confirmed operation |
| Expecting a fixed overlay to lock body scroll in static SSR | CSS positioning does not provide a scroll-lock lifecycle | Static overlay markup is possible; Blazor-managed JS locking needs an interactive host and cleanup |
| Unstyled `SfSpinner` inside `SfButton` | Absolute positioning can obscure the label or cover a larger region | Keep the button disabled while busy; use a label swap or a positioned adjacent spinner |
| Adding `role="alert"` to every wrapper | May create redundant or overly urgent announcements | Use the spinner's status region; add `aria-busy` to the affected content region only as appropriate |
| Treating an omitted `Label` as an automatic WCAG failure | Confuses visible text with the accessible name | `Label` has no minimum length; choose a meaningful `AriaLabel` and test the composition |
| Relying on a spinner or `pointer-events: none` to disable input | Underlying controls can remain operable | Disable the action separately and provide busy feedback |
| Expecting `args.Cancel` to roll back the parent's `Visible` value | Parent state and the canceled transition can disagree | Keep the parent state consistent with the cancellation decision; avoid recursive show/hide calls |
| Assuming two spinners cannot share a `bool` | Unnecessary restriction; each instance has its own layout and callbacks | Shared state is valid; choose intentional regions and avoid redundant announcements |
| `OnOpen` / `OnClose` implemented as `async void` | Async completion and exceptions cannot be tracked through the callback task | Return `Task`; await all asynchronous work |

## Anti-Pattern Workflows

Examples below are fragments unless all state and dependencies are declared.
Interactive event examples assume host/service setup, relevant namespace imports,
and application-provided save handlers; they have not been compile-tested here.

### Workflow 1 — Agent wires three visibility bindings

**Bad (intentionally invalid Razor):**
```razor
<SfSpinner Visible="@_busy"
           @bind-Visible="@_busy"
           VisibleChanged="@(v => _busy = v)"
           Label="Saving…" />
<!-- Duplicate Visible and VisibleChanged parameters generated by @bind-Visible. -->
```

**Fix:** pick exactly one. `@bind-Visible` is enough:

```razor
<SfSpinner @bind-Visible="@_busy" Label="Saving…" AriaLabel="Saving…" />
```

### Workflow 2 — Agent expects an immediate browser paint

**Concern:** `_busy = true` followed by `await SaveAsync()` is a normal
interactive event-handler pattern, not inherently an error. Blazor schedules
renders around async event callbacks. If the work completes synchronously or
blocks before yielding, the intermediate busy state may not be displayed.
`StateHasChanged` (even via awaited `InvokeAsync`) requests a render; it does
not wait for a browser paint.

**Pattern:** use genuinely asynchronous work and clear busy state in `finally`.
Fragment for a normal interactive Blazor event; `_busy` and `SaveAsync` are
application-provided:

```csharp
private async Task SubmitAsync()
{
    if (_busy) return;
    _busy = true;
    try
    {
        await SaveAsync();
    }
    finally
    {
        _busy = false;
    }
}
```

Keep synchronous blocking work off the UI execution path using a strategy
appropriate to the host. Do not promise visible intermediate frames for fast
operations; explicit render requests are not paint barriers.

### Workflow 3 — Agent wants confirmation "do you really want to save?" with a spinner

**Anti-pattern:** raise the spinner for 3s before commit, hoping the user
will click away.

**Fix:** use a modal dialog (load
[syncfusion-blazor-toolkit-dialog](../syncfusion-blazor-toolkit-popups/dialog/SKILL.md)).
Wire both confirm and cancel actions. Only after confirmation, run the async
operation with the `try`/`finally` busy-state pattern in Workflow 2.

Spinner for *post-confirmation* (during actual save); dialog gates *user input*.

### Workflow 4 — Agent's overlay uses `body { overflow: hidden }` but never clears

**Anti-pattern:** fire-and-forget `JS.InvokeVoidAsync("lockBody")` on every
render, then rely only on disposal to unlock the page.

**Fix:** use application-owned lock/unlock helpers that save and restore the
prior overflow style. Await interop, acquire/release on busy-state transitions,
and coordinate ownership if multiple overlays can be open. No scroll-lock
helper is supplied by `SfSpinner`.

**Hosting caveat:** call browser interop only when the host is interactive,
not during prerendering/static SSR. A server circuit can disconnect before
`DisposeAsync` runs, so JS cleanup there is best-effort, not guaranteed.
Provide client-side cleanup for navigation/removal when needed. Handle only
expected teardown exceptions such as `JSDisconnectedException` or
`ObjectDisposedException` in that cleanup path; do not suppress all errors
with a catch-all. `IAsyncDisposable.DisposeAsync` returns `ValueTask`; that is
separate from the `Task`-returning component event handlers discussed above.

### Workflow 5 — Agent nests `SfSpinner` inside `SfButton` for a submit button

**Composition concern:** nesting is not forbidden, but the default absolute
positioning can obscure the button label or cover more than the button.
Decorative `aria-hidden` content does not disable input. Prefer a label swap
or an adjacent indicator, and keep the button disabled while processing.

**Fix #1 — label swap (simplest):** fragment using the application's `_busy`
state and `Task`-returning `SubmitAsync` handler:

```razor
<SfButton Disabled="@_busy" OnClick="SubmitAsync">
    @(_busy ? "Saving…" : "Submit")
</SfButton>
```

**Fix #2 — adjacent indicator:** use the positioned composition in
[spinner-template.md](references/spinner-template.md#inline-button-adjacent-spinner).
An indicator or overlay remains feedback, not a substitute for disabling the
busy action or guarding the handler against duplicate submission.

> See also: [references/spinner-template.md](references/spinner-template.md)
> (custom visuals + composition) and
> [references/spinner-overlay.md](references/spinner-overlay.md)
> (full-page, region, modal overlays).

## Minimal Example

Initial visible markup with a positioned region (requires the usual Toolkit
services and styles). Static SSR can emit this markup; client-driven state
changes require an interactive host.

```razor
@using Syncfusion.Blazor.Toolkit.Spinner

<div style="position: relative; min-height: 80px;">
    <SfSpinner Visible="true" Label="Loading data..." AriaLabel="Loading data..." />
</div>
```

For installation, services, visibility control, labels, CSS classes, and
size/positioning see [references/spinner-implementation.md](references/spinner-implementation.md).

---

## Documentation and Navigation Guide

| Need | Read |
|---|---|
| Setup, services, styles, visibility, labels, CSS, sizing | [references/spinner-implementation.md](references/spinner-implementation.md) |
| `OnOpen` / `OnClose` / `Created` / `Destroyed`, cancellation, templates | [references/spinner-events-customization.md](references/spinner-events-customization.md) |
| Accessibility considerations / keyboard / screen reader | [references/accessibility-best-practices.md](references/accessibility-best-practices.md) |
| Branded visuals, nested `SpinnerTemplates.Template`, scoped CSS | [references/spinner-template.md](references/spinner-template.md) |
| Overlay patterns (page / region / modal / scroll-lock) | [references/spinner-overlay.md](references/spinner-overlay.md) |

These links are navigation aids, not a claim that every linked reference has
been source-validated in this focused cleanup. Verify examples against the
current Toolkit source before adopting them.

---

## Next Steps

1. **Setup:** [references/spinner-implementation.md](references/spinner-implementation.md)
2. **Overlay patterns:** [references/spinner-overlay.md](references/spinner-overlay.md)

**Demo:** https://blazor.syncfusion.com/demos/toolkit/spinner