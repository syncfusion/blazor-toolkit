---
license: MIT
name: dialog-vs-tooltip-decision-matrix
description: >
  Decide between SfDialog and SfTooltip in the Syncfusion Blazor Toolkit
  with side-by-side comparison, lifecycle differences, and trigger-mode
  semantics. Required reading before either popup component is generated.
---

# Dialog vs Tooltip Decision Matrix

Both `SfDialog` and `SfTooltip` render a "popup" overlay, but they have
diametrically opposite UX semantics. Misusing them is the most common
popup-component bug.

## Quick decision

```
Need to block the user with required input or confirmation?
├─ Yes → SfDialog
└─ No
   ├─ Need interaction (a button in the popup)?
   │  └─ ❌ Neither — use SfPopover (not in this skill)
   └─ Just informational text?
      ├─ Triggered by hover / focus → SfTooltip
      └─ Triggered by click explicitly → SfTooltip with OpensOn="Click"
```

## Side-by-side

| Property | `SfDialog` | `SfTooltip` |
|---|---|---|
| Default trigger | Manual (`Visible`) | Hover / focus |
| Block UI? | Yes (with `IsModal`) | No |
| Focus trap | When `IsModal=true` | None |
| Close-on-Escape ✓ | `CloseOnEscape="true"` | Always (Escape is global) |
| Render strategy | Inline | Portal-style (renders to body root) |
| Light dismiss (click outside) | Handle `OnOverlayModalClick` and request close | Depends on trigger and `IsSticky` |
| `aria-modal` | Auto when `IsModal=true` | Not modal; uses `aria-describedby` |
| Default ARIA role | `dialog` | `tooltip` |
| Static rendering vs interaction | Static path detected on .NET 9+; JS lifecycle needs interactivity | Trigger markup can render; tooltip behavior requires interactivity |
| Position | `DialogPositionData` child with `X` / `Y` | 12 slots via `Position` enum |
| Animation | Yes (fade, slide, zoom) | Yes (fade, slide, none) |
| Buttons? | Yes (`DialogButtons`) | No |
| Form inputs | Yes | No — produces awkward patterns |
| Async / await inside | Yes (lifecycle methods) | Yes (`OnOpen`/`OnClose`) |
| Typical size | Modal: 320–640px; full-screen: 100% | 1–4 lines of text |
| Default duration | Indefinite | 100–300ms based on pointer movement |

## Lifecycle differences

`SfDialog` defaults to `Visible=true`; bind an initially false field or set
`Visible="false"` to start closed. Opening invokes cancelable `OnOpen`, then
`Opened`. Closing invokes cancelable `OnClose`, then `Closed`. Await
`ShowAsync()` / `HideAsync()` for programmatic control. A `DialogButton` does
not automatically close the dialog: its handler must request dismissal.
`AllowPrerender` controls retention of closed content, not host interactivity.

`SfTooltip` exposes cancelable `OnOpen` / `OnClose` and after-transition
`Opened` / `Closed` callbacks for its interaction lifecycle. These are not
limited to custom/programmatic mode. It has no visibility binding. Await
`OpenAsync()` / `CloseAsync()` for manual control and `RefreshAsync()` when
refreshing targets; use `RefreshPositionAsync()` for positioning.

## Trigger modes (`OpensOn`)

`OpensOn` is a string parameter, not an enum:

| Value | Trigger |
|---|---|
| `"Auto"` | Hover or focus (default) |
| `"Hover"` | Pointer interaction |
| `"Focus"` | Focus interaction |
| `"Click"` | Click interaction |
| `"Custom"` | Manual `OpenAsync()` / `CloseAsync()` |

Default to `Auto` for focusable controls. Ensure click triggers are
keyboard-operable. Use `Custom` for application-owned visibility, not merely
to subscribe to lifecycle events.

## Position enum (12 slots)

```text
TopLeft       TopCenter       TopRight
LeftCenter                                    RightCenter
BottomLeft    BottomCenter    BottomRight
```

`SfTooltip`: `Position="Syncfusion.Blazor.Toolkit.Position.TopCenter"`.
Import `Syncfusion.Blazor.Toolkit.Popups` for the component and
`Syncfusion.Blazor.Toolkit` for enums. Other slots include `LeftTop`,
`LeftBottom`, `RightTop`, and `RightBottom`.

`SfDialog`: use the `DialogPositionData` child component's `X` / `Y` values.

## Animation parity

| Animation | `DialogAnimationSettings.Effect` | Tooltip `AnimationModel.Open` / `.Close` effects |
|---|---|---|
| Fade | `DialogEffect.Fade` | `Effect.FadeIn` / `Effect.FadeOut` |
| Slide | `DialogEffect.SlideLeft` etc. | `Effect.SlideLeftIn` / `Effect.SlideLeftOut` etc. |
| Zoom | `DialogEffect.Zoom` | `Effect.ZoomIn` / `Effect.ZoomOut` |
| None | `DialogEffect.None` | `Effect.None` |

Tooltip open/close settings are `TooltipAnimationSettings`; dialog animation
is configured using its child component. See the component references for
configuration fragments.

## Accessibility matrix

| Need | `SfDialog` | `SfTooltip` |
|---|---|---|
| `<label>` association | Inside the dialog | Use `aria-describedby` on the trigger |
| Focus on open | First focusable child | Trigger element |
| Focus on close | Returns to last focused | Trigger element |
| `aria-modal` | Auto | Never |
| `role` | `dialog`, `alertdialog` if `IsModal=true` and only one button | `tooltip` (semantic) |
| Screen reader announcement | Dialog title | Tooltip content |
| Tab between trigger and content | Tab moves into dialog | Tab leaves trigger entirely |

`role="tooltip"` is the only role a tooltip may have. Don't make a dialog
with `role="tooltip"` — it loses `aria-modal` semantics.

## Common anti-patterns

### ◾ Dialog as a tooltip

```razor
<!-- ❌ WRONG: User just wanted to show help text. -->
<SfButton @onclick="@(() => isHelpVisible = true)">?</SfButton>
<SfDialog @bind-Visible="isHelpVisible"
          IsModal="true"
          Header="Help">
    Clear your browser cache and reload.
</SfDialog>
```

This blocks the page with a modal that only contains informational text. Use
`SfTooltip` instead:

```razor
<!-- ✅ RIGHT -->
<SfTooltip Content="Clear your browser cache and reload.">
    <SfButton>?</SfButton>
</SfTooltip>
```

### ◾ Tooltip for a confirmation

```razor
<!-- ❌ WRONG: Tooltip can't block confirmation. -->
<SfTooltip Content="Are you sure? (Click again to confirm)">
    <SfButton @onclick="@Submit">Submit</SfButton>
</SfTooltip>
```

Use `SfDialog`:

```razor
<!-- ✅ RIGHT -->
<SfDialog @bind-Visible="@showConfirm" IsModal="true"
          Header="Confirm">
    <DialogTemplates>
        <Content>
            <p>Are you sure?</p>
        </Content>
    </DialogTemplates>
    <DialogButtons>
        <DialogButton Content="Submit" OnClick="@Submit" />
        <DialogButton Content="Cancel" />
    </DialogButtons>
</SfDialog>
```

### ◾ Tooltip on a `disabled` element

`disabled="true"` elements don't fire pointer events. Wrap in a span:

```razor
<!-- ❌ Tooltip won't appear because the button doesn't fire mouseenter -->
<SfTooltip Content="Can't submit yet">
    <SfButton Disabled="true">Submit</SfButton>
</SfTooltip>

<!-- ✅ Target the focusable wrapper with automatic hover/focus behavior. -->
<SfTooltip Content="Can't submit yet" OpensOn="Auto" Target="#submitHelp">
    <span id="submitHelp" tabindex="0">
        <SfButton Disabled="true">Submit</SfButton>
    </span>
</SfTooltip>
```

(For accessibility, render a fallback explanation *inside* the disabled
button instead — the tabindex=0 wrapper is the workaround only when tooltip
absolutely must appear on a disabled control.)

### ◾ Dialog rendered inside a form button

`SfDialog` rendered inside `<DialogButtons>` of another dialog is fragile
because the nested-portal positioning needs an explicit z-index. Stack them
in parallel and toggle via two flags.

## Decision cheat-sheet

| Need | Component |
|---|---|
| Confirm destructive action | `SfDialog` (modal) |
| Show a form for input | `SfDialog` |
| Show brief informational text on hover | `SfTooltip` |
| Help text for an icon button | `SfTooltip` |
| Long-form content with multiple sections | `SfDialog` |
| Keyboard-shortcut hint | `SfTooltip` (`content="Ctrl+S"`) |
| Async progress notification | Use `SfSpinner` (overlay), not `SfTooltip` |
| Action menu attached to point-of-click | Use `SfMenuButton` / `SfContextMenu` — not in this skill |