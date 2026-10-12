---
license: MIT
name: syncfusion-blazor-toolkit-buttons
description: >
  Implement Syncfusion Blazor Toolkit button components — SfButton,
  SfButtonGroup, and inner Button.
  USE FOR: primary/secondary clickable buttons, icon-only buttons, toggle
  buttons, button groups (single/multi selection), event handlers, and
  accessibility (keyboard, ARIA, focus).
  DO NOT USE FOR: form input fields (use syncfusion-blazor-toolkit-inputs),
  navigation menu UI (use SfMenu / SfToolbar — not in this skill),
  icon-only controls that should suppress focus (use a styled
  syncfusion-blazor-toolkit-notifications spinner instead).
compatibility: ".NET 8+, render-modes: Static SSR, Server, WebAssembly, Auto"
metadata:
  author: "Syncfusion Inc"
  version: "1.0.0"
---

# Syncfusion Blazor Toolkit Buttons

> ✅ **Render mode:** `SfButton` can render in Static SSR; client event handlers (`OnClick`) and button-group selection callbacks require interactivity. Read `AGENTS.md` for host conventions. Examples are fragments unless they include all imports, state, services, and handlers.

## Core Rules

1. **`SfButton` does not render a `<form>` wrapper.** Add `<EditForm>` or
   `<form>` yourself if you need form-scoped submission.
2. **`IsPrimary="true"` is presentational only.** It applies the active-theme
   primary CSS class — pair with theme CSS to control actual visual weight.
3. **`OnClick` is `EventCallback<MouseEventArgs>`**, fired after click; not
   cancellable. Use the `Created` lifecycle event for initialization, not
   `OnClick`.
4. **`Disabled` is one-way.** The component does not auto-clear it after a
   click; toggle the bound flag manually in your `@code` block.
5. **`IconCss` accepts a Syncfusion icon class pair** (e.g. `e-icons e-add`).
   Bare-prefix classes won't render.
6. **`SfButtonGroup Mode="SelectionMode.Multiple"` allows empty selection.**
   If you need mandatory selection, validate in code.
7. **Inner `Button` components inside `SfButtonGroup` do not inherit
   `CssClass`.** Re-apply at the inner level.
8. **Keyboard activation is `Enter` and `Space`.** Don't override or you'll
   break accessibility.
9. **`HtmlAttributes` captures arbitrary HTML attributes** (id, data-*, role)
   without exposing every one as a property.

## Don'ts

| Anti-pattern | Symptom | Fix |
|---|---|---|
| `async void OnClick = …` | Exceptions silently escape the dispatcher; spinner "sticks" with no error | `async Task OnClick(MouseEventArgs e)` — let Blazor handle re-rendering |
| `<SfButton>` inside `<a>` | Invalid nested interactive controls | Use a single real `<a href="…">` for navigation; `e-link` CSS does not change a button's semantics |
| `Disabled="@(bool?)flag"` | Nullable binding throws on toggle; component treats as always-false | Bind to a `bool`, never `bool?` |
| Override `:focus` without `:focus-visible` replacement | WCAG 2.4.7 violation; keyboard users lose focus visibility | Define `:focus-visible { outline: 2px solid var(--brand) }` |
| `OnClick` for form submit with no surrounding `<EditForm>` | Validation skipped; data posted without `DataAnnotationsValidator` | Wrap in `<EditForm Model="…"><DataAnnotationsValidator/>` then call `OnValidSubmit` |

## Anti-Pattern Workflows

These are the four most common mistakes agents make; the workflow below each
says exactly what to do instead.

### Workflow 1 — Agent wants to wire `OnClick="@Save"`

**Bad:**
```razor
<SfButton OnClick="@Save">Save</SfButton>
@code { private async void Save(MouseEventArgs _) { /* ... */ } }
```

**Why it fails:** `async void` says "I will never await me" — Blazor's
dispatcher can't catch exceptions, so the unhappy path dumps to console.
Re-rendering also fights the awaited continuations.

> This is a general Blazor rule across all click handlers (`SfButton`,
> `<button @onclick>`, etc.) — see the
> [`author-component`](https://github.com/dotnet/skills/blob/main/plugins/dotnet-blazor/skills/author-component/SKILL.md)
> upstream skill for the broader rule set.

**Correct:**
```razor
@using Syncfusion.Blazor.Toolkit.Buttons

<SfButton OnClick="@SaveAsync">Save</SfButton>
@code {
    private bool _busy;
    private async Task SaveAsync(MouseEventArgs _)
    {
        if (_busy) return;
        _busy = true;
        try   { await SaveService.SaveAsync(model); }
        finally { _busy = false; }
    }
}
```

### Workflow 2 — Agent nests `SfButtonGroup` inside `<EditForm>` expecting `Disabled` to clear

**Bad:**
```razor
<SfButtonGroup Mode="@SelectionMode.Multiple">
    <Button>Bold</Button>
    <Button>Italic</Button>
</SfButtonGroup>
```

**Why it fails:** empty selection is valid `Multiple` mode. Form-validation
won't catch "user selected nothing" unless you write a custom validator.

**Correct:** selection belongs to each child `Button`. `SelectedChanged` is
`EventCallback<bool>`; `SfButtonGroup` has no group selection DTO/event.
This interactive form example validates that at least one option is selected:

```razor
@using Syncfusion.Blazor.Toolkit
@using Syncfusion.Blazor.Toolkit.Buttons
@using System.ComponentModel.DataAnnotations

<EditForm Model="editor">
    <DataAnnotationsValidator />
    <ValidationSummary />
    <SfButtonGroup Mode="SelectionMode.Multiple">
        <Button @bind-Selected="editor.Bold">Bold</Button>
        <Button @bind-Selected="editor.Italic">Italic</Button>
    </SfButtonGroup>
    <button type="submit">Validate</button>
</EditForm>

@code {
    private EditorModel editor = new();

    public class EditorModel : IValidatableObject
    {
        public bool Bold { get; set; }
        public bool Italic { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (!Bold && !Italic)
                yield return new ValidationResult("Choose at least one option.");
        }
    }
}
```

### Workflow 3 — Agent places `<SfButton>` inside a `<a>` to make a card link

**Bad:**
```razor
<a href="/detail/42">
    <SfButton Content="Open" OnClick="OpenDetail" />  <!-- both click handlers fire -->
</a>
```

**Why it fails:** clicking the button bubbles to the parent anchor and
fires two navigations. ARIA reports two controls on one element.

**Correct:** use one native link for navigation:

```razor
<a href="/detail/42" class="e-btn e-link" aria-label="Open detail 42">Open</a>
```

`CssClass="e-link"` changes appearance only. `SfButton` still renders a
button, so do not nest it inside an anchor or describe it as a semantic link.

### Workflow 4 — Agent assumes `Disabled="@isProcessing"` clears via the button click

**Bad:**
```razor
<SfButton Disabled="@_isProcessing" OnClick="SubmitAsync">Submit</SfButton>
@code {
    private async Task SubmitAsync(MouseEventArgs _) { _isProcessing = true; await DoWork(); }
    // submit fires but `_isProcessing` is never set back to false ⇒ permanent lock
}
```

**Why it fails:** `Disabled` is one-way; the click handler must clear the
flag itself.

**Correct:**
```razor
@using Syncfusion.Blazor.Toolkit.Buttons

<SfButton Disabled="@_isProcessing" OnClick="SubmitAsync">Submit</SfButton>
@code {
    private bool _isProcessing;
    private async Task SubmitAsync(MouseEventArgs _)
    {
        if (_isProcessing) return;
        _isProcessing = true;
        try   { await DoWork(); }
        finally { _isProcessing = false; }   // mandatory clear
    }
}
```

**Or:** keep the same application-owned busy flag and show an adjacent
`SfSpinner` (see
[syncfusion-blazor-toolkit-notifications](../syncfusion-blazor-toolkit-notifications/SKILL.md)). `IsBusy` / `ButtonState` are not `SfButton` APIs.

## Minimal Example

```razor
@using Syncfusion.Blazor.Toolkit.Buttons

<SfButton Content="Save"
          IsPrimary="true"
          OnClick="@SaveAsync" />

@code {
    private async Task SaveAsync(MouseEventArgs _)
    {
        // your async work; see Workflow 1 for the busy-flag pattern
    }
}
```

---

## Documentation and Navigation Guide

| Need | Read |
|---|---|
| First button / project setup | [references/getting-started.md](references/getting-started.md) |
| Styling, `Disabled`, `CssClass` | [references/button-fundamentals.md](references/button-fundamentals.md) |
| Icons, `Content` vs `ChildContent` | [references/icons-and-content.md](references/icons-and-content.md) |
| Events, `EventCallback`, async patterns | [references/events-and-callbacks.md](references/events-and-callbacks.md) |
| `SfButtonGroup`, selection modes | [references/button-group.md](references/button-group.md) |

> Full property reference for `SfButton`, `SfButtonGroup`, and inner `Button` lives in `references/button-fundamentals.md` and `references/button-group.md`.

---

## Next Steps

1. **Start Simple:** Read [references/getting-started.md](references/getting-started.md)
2. **Style and Configure:** Explore [references/button-fundamentals.md](references/button-fundamentals.md)
3. **Add Interactivity:** Learn [references/events-and-callbacks.md](references/events-and-callbacks.md)
4. **Expand:** Grouping in [references/button-group.md](references/button-group.md)

**Demo:** https://blazor.syncfusion.com/demos/toolkit/buttons/button