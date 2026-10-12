---
license: MIT
name: syncfusion-blazor-toolkit-dialog
description: >
  Implement Syncfusion Blazor Toolkit SfDialog — modal and modeless windows with
  buttons, drag, resize, focus trap, ARIA semantics, and animation.
  USE FOR: confirmation modals, form-in-modal, full-screen dialogs, alert
  dialogs, settings panels, modals that block the underlying UI, and
  DialogService-driven dynamic creation.
  DO NOT USE FOR: brief informational text on hover (use the
  syncfusion-blazor-toolkit-tooltip skill), non-blocking progress overlays
  (use syncfusion-blazor-toolkit-notifications / SfSpinner), or as a navigation
  primitive.
metadata:
  author: "Syncfusion Inc"
  version: "1.0.0"
---

# Syncfusion Blazor Toolkit: Dialog (SfDialog)

`SfDialog` renders an overlay window that *blocks* the underlying page when
modal, or floats alongside it when modeless. It supports a header, body,
buttons, drag, resize, and animation. The Toolkit's predefined-dialog service
is `SfDialogService` with `SfDialogProvider` (`AlertAsync`, `ConfirmAsync`,
`PromptAsync`); arbitrary dynamic component creation is application-owned.

## Core Rules

1. **Use `IsModal="true"` for any dialog that requires user input** — the
   framework installs the focus trap and `aria-modal`.
2. **Use `@bind-Visible` to mirror visibility**, or `Visible` with an
   explicit `VisibleChanged` callback, never both. Default `Visible` is
   `true`; bind a false field or set `Visible="false"` to start closed.
3. **Buttons do not dismiss automatically.** A `DialogButton.OnClick`
   handler must explicitly update state or await `dialog.HideAsync()`.
4. **`AllowPrerender` defaults to `false`.** It controls retaining dialog
   content while closed, not the host's Blazor prerender/interactivity mode.
   It is not a general solution for JS interop during prerender.
5. **`CloseOnEscape` defaults to `true`.** Provide a visible dismissal
   control (`ShowCloseIcon` or a Cancel/Close button) when appropriate.
6. **`OnOpen` / `OnClose` are cancelable.** After-transition events are
   `Opened` (`OpenEventArgs`) and `Closed` (`CloseEventArgs`).
7. **Programmatic lifecycle:** await `ShowAsync(bool? isFullScreen = null)`
   / `HideAsync()` after the `SfDialog` reference is assigned. The source
   also has a static-render path detected on .NET 9+; JS-backed operations
   and callbacks require interactivity.

Examples below are component fragments: supply referenced fields, handlers,
imports, and host setup unless the snippet declares them.

```razor
@using Syncfusion.Blazor.Toolkit.Popups

<SfDialog @ref="confirm"
          @bind-Visible="@_showConfirm"
          IsModal="true"
          ShowCloseIcon="true"
          AllowPrerender="false"
          CloseOnEscape="true"
          Header="Confirm"
          Width="420px"
          OnClose="OnApproachClose">
    <DialogTemplates>
        <Content>
            <p>@_message</p>
        </Content>
    </DialogTemplates>
    <DialogButtons>
        <DialogButton Content="Yes" IsPrimary="true"
                      OnClick="@(() => OnDecideAsync(true))" />
        <DialogButton Content="No"
                      OnClick="@(() => OnDecideAsync(false))" />
    </DialogButtons>
</SfDialog>
```

## Decision Tree

```
Need a dialog?
├─ Single-shot static content?
│  └─ Inline <SfDialog @bind-Visible="…">
├─ Predefined alert/confirm/prompt from many places?
│  └─ SfDialogService + SfDialogProvider
└─ Multi-step wizard or arbitrary dynamic content?
   └─ Inline <SfDialog> + application-owned state/composition
```

## Documentation and Navigation Guide

| Need | Read |
|---|---|
| First modal / project setup | [references/dialog-basics.md](references/dialog-basics.md) |
| Properties, events, methods, animation surface (this skill's table-of-truth) | [references/dialog-properties.md](references/dialog-properties.md) |
| Service-pattern, dynamic creator, wizard, cascading integration | [references/dialog-advanced.md](references/dialog-advanced.md) |

For cross-component decision matrix (dialog vs tooltip vs popover), see
[../router/dialog-vs-tooltip-decision-matrix.md](../router/dialog-vs-tooltip-decision-matrix.md).

## Top 5 Properties You'll Use

| Property | Why it matters |
|---|---|
| `@bind-Visible` | Synchronizes state; do not also supply `VisibleChanged` |
| `IsModal` | Modal backdrop and focus behavior |
| `AllowPrerender` | Retains content while closed when true; defaults false |
| `ShowCloseIcon` | Optional header dismissal; a Close/Cancel button is another option |
| `DialogAnimationSettings` | Child component with `Effect` (`DialogEffect`), `Duration`, and `Delay` |

Full property/event/method/animation reference: [references/dialog-properties.md](references/dialog-properties.md).

## Common Patterns

### Pattern 1 — Inline confirmation modal (delete / unsaved-changes guard)

See the **Core Rules** snippet above for a full modal example.

### Pattern 2 — Programmatic lifecycle

```razor
@using Syncfusion.Blazor.Toolkit.Popups

<SfDialog @ref="dlg" Visible="false" AllowPrerender="false" />
<button @onclick="@(() => dlg.ShowAsync())">Open programmatically</button>

@code {
    private SfDialog dlg = default!;
}
```

## Don'ts

| Anti-pattern | Symptom | Fix |
|---|---|---|
| Nested `<SfDialog>` inside another `<SfDialog>` | Render artifacts on overlay click; z-index collisions; nested focus trap locks browser | Use portals/z-index (inner > outer by ≥1000), or replace outer with stacked steps |
| Redundant state mutations plus `HideAsync()` for one dismissal | Unnecessary lifecycle requests | Await `HideAsync()` and let binding mirror the result, or update the bound state once |
| `CloseOnEscape` used to cancel close | That flag toggles default behaviour; it doesn't intercept | Use the cancelable `OnClose` event with `args.Cancel = true` |
| Assuming modal dialogs cannot drag | Unnecessary restriction | `AllowDragging` and `IsModal` can be combined; validate focus and keyboard behavior |
| `<SfDialog>` containing `<form>` directly without binding the dialog lifecycle | Submit button doesn't post to the page EditContext model | Use `<EditForm Model="@X" OnValidSubmit="@Save">` inside `DialogTemplates`, and trigger save via button inside the form |
| `IsModal="false"` *and* no backdrop for destructive actions | User can click delete triggers elsewhere | `IsModal="true"` plus `ShowCloseIcon="true"` |
| `Visible="true"` initial in markup | Dialog opens on first paint — usually unintended auto-pop | Use `@bind-Visible` and toggle from a button |

> Don'ts covered by Anti-Pattern Workflows (double-binding, JS in `OnInitialized`, JS in `DialogTemplates`/`OnAfterRenderAsync`, no `DisposeAsync`) are not duplicated here — see the workflows below.

## Anti-Pattern Workflows

> **Note:** **Bad** snippets illustrate anti-patterns. **Correct** snippets are focused fragments, not a guarantee of standalone executability: provide application state, handlers, required imports, and interactive host setup.

### Workflow 1 — Agent copies the dead `onConfirmCallback` snippet

**Anti-pattern** (matches an older SKILL.md `Modal Confirmation Dialog` snippet):
```razor
<SfDialog @bind-Visible="showConfirm" IsModal="true" Header="Confirmation">
    <DialogTemplates><Content><p>@confirmMessage</p></Content></DialogTemplates>
    <DialogButtons>
        <DialogButton Content="Yes" IsPrimary="true" OnClick="@OnConfirmYes"/>
        <DialogButton Content="No"  OnClick="@OnConfirmNo"/>
    </DialogButtons>
</SfDialog>

@code {
    private string confirmMessage = "";
    private Func<Task> onConfirmCallback;   // ❌ assigned, never invoked
    private async Task OnConfirmYes() { showConfirm = false; /* callback never called */ }
}
```

**Why it fails:** `onConfirmCallback` is set by `ShowConfirmation(...)` but
the button onclick never invokes it. Pressing "Yes" effectively does nothing.

**Correct — `EventCallback<T>`-based:**
```razor
@using Syncfusion.Blazor.Toolkit.Popups

<SfDialog @bind-Visible="@_showConfirm" IsModal="true" Header="Confirm"
          AllowPrerender="false" ShowCloseIcon="true">
    <DialogTemplates><Content><p>@_message</p></Content></DialogTemplates>
    <DialogButtons>
        <DialogButton Content="Yes" IsPrimary="true" OnClick="@(() => Decide(true))"/>
        <DialogButton Content="No" OnClick="@(() => Decide(false))"/>
    </DialogButtons>
</SfDialog>

@code {
    private bool _showConfirm;
    private string _message = "";
    [Parameter] public EventCallback<bool> OnDecision { get; set; }

    public void Ask(string message) { _message = message; _showConfirm = true; }
    private async Task Decide(bool accepted)
    {
        _showConfirm = false;
        await OnDecision.InvokeAsync(accepted);
    }
}
```

The parent assigns an `OnDecision` callback and receives the bool; it is not
an `@bind-OnDecision` pair.

### Workflow 2 — Calling a dialog before its reference exists

`@ref` is assigned after rendering, not in `OnInitialized`. For an
interactive component that intentionally opens after its first render:

```razor
@using Syncfusion.Blazor.Toolkit.Popups

<SfDialog @ref="dialog" Visible="false" />

@code {
    private SfDialog? dialog;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && dialog is not null)
            await dialog.ShowAsync();
    }
}
```

Blazor owns child-component disposal; dispose only resources the parent owns.

### Workflow 3 — Agent tries JS init inside `DialogTemplates` during prerender

**Bad:**
```razor
<SfDialog AllowPrerender="true">
    <DialogTemplates>
        <Content>
            @inject IJSRuntime JS
            <p>URL: @currentUrl</p>   <!-- JS throws on first paint -->
        </Content>
    </DialogTemplates>
</SfDialog>
@code {
    private string currentUrl;
    protected override void OnInitialized()
    {
        currentUrl = JS.InvokeAsync<string>("getHref").Result;
    }
}
```

**Fix:**
```razor
@using Syncfusion.Blazor.Toolkit.Popups

<SfDialog AllowPrerender="false">
    <DialogTemplates>
        <Content>
            @if (_url is null) { <p>Loading…</p> }
            else { <p>URL: @_url</p> }
        </Content>
    </DialogTemplates>
</SfDialog>

@code {
    private string? _url;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    protected override async Task OnAfterRenderAsync(bool first)
    {
        if (first) _url = await JS.InvokeAsync<string>("getHref");
    }
}
```

### Workflow 4 — Agent wants a confirmation modal that traps `OnClose`

**Correct:**
```razor
@using Syncfusion.Blazor.Toolkit.Popups

<SfDialog OnClose="@BlockIfDirty" @bind-Visible="@_open">
    <DialogTemplates>
        <Content><p>Save the document?</p></Content>
    </DialogTemplates>
    <DialogButtons>
        <DialogButton Content="Save"   OnClick="SaveFirst"/>
        <DialogButton Content="Discard" OnClick="Discard"/>
    </DialogButtons>
</SfDialog>

@code {
    private bool _dirty;
    private void BlockIfDirty(BeforeCloseEventArgs e)
    {
        e.Cancel = _dirty;     // user must explicitly Save or Discard
    }
}
```

**Button vs `OnClose` order:** `DialogButton.OnClick` alone does not close
the dialog. Await the save in the handler, clear the dirty flag only after
success, then explicitly request dismissal (for example `HideAsync()`). The
cancelable `OnClose` handler can block that request.

## Next Steps

1. **First modal:** [references/dialog-basics.md](references/dialog-basics.md)
2. **Service pattern / dynamic creator / wizard:** [references/dialog-advanced.md](references/dialog-advanced.md)
3. **Cross-skill decision matrix:** [../router/dialog-vs-tooltip-decision-matrix.md](../router/dialog-vs-tooltip-decision-matrix.md)

**Demo:** https://blazor.syncfusion.com/demos/toolkit/dialogs