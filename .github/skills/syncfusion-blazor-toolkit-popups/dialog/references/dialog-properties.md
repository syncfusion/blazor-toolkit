---
license: MIT
name: dialog-properties
description: >
  Complete reference for Syncfusion Blazor Toolkit SfDialog properties,
  events, methods, and animation settings. Read when you need an
  authoritative signature or default value.
---

# SfDialog Properties & API Reference

The full surface of the `SfDialog` component. Use this when you need
canonical signatures, default values, or are debugging unexpected behavior.

## Properties

| Property | Type | Default | Purpose |
|---|---|---|---|
| `Visible` | `bool` | `true` | Show/hide the dialog (set `false` or bind a `false` field to start closed) |
| `@bind-Visible` | `bool` | — | Two-way binding for visibility (preferred) |
| `Header` | `string` | — | Top-bar text |
| `Content` | `string` | — | Body text |
| `DialogTemplates` (`Header`, `Content`, `FooterTemplate`) | child component with `RenderFragment`s | — | Header / body / footer markup |
| `DialogButtons` | child component containing `DialogButton` items | — | Declarative footer buttons |
| `Width` | `string` | `"100%"` | Dialog width (CSS units; e.g. `"420px"`, `"60%"`) |
| `Height` | `string` | `"auto"` | Dialog height |
| `IsModal` | `bool` | `false` | Block underlying UI (installs `aria-modal` + focus trap) |
| `AllowDragging` | `bool` | `false` | Drag header to reposition |
| `EnableResize` | `bool` | `false` | Resize handle on edges |
| `AllowPrerender` | `bool` | `true` | Set `false` when dialog body depends on interactive content |
| `CloseOnEscape` | `bool` | `true` | Escape closes the dialog |
| `ShowCloseIcon` | `bool` | `false` | `×` icon in the header (required for mouse-only users) |
| `ZIndex` | `double` | `1000` | CSS stacking layer |
| `Target` | `string` | — | CSS selector of the container the dialog is rendered in |
| `DialogAnimationSettings` | child component | — | `Effect` (`DialogEffect`), `Duration`, `Delay` |
| `DialogPositionData` | child component | — | `X` / `Y` position strings |
| `CssClass` | `string` | — | Custom CSS classes applied to root element |

## Events

| Event | Signature | Cancellable? | Fires |
|---|---|---|---|
| `OnOpen` | `EventCallback<BeforeOpenEventArgs>` | yes | Before the dialog opens |
| `Opened` | `EventCallback<OpenEventArgs>` | no | After the dialog opened |
| `OnClose` | `EventCallback<BeforeCloseEventArgs>` | **yes** | Before dismissal (set `args.Cancel = true` to block) |
| `Closed` | `EventCallback<CloseEventArgs>` | no | After dismissal |
| `OnOverlayModalClick` | `EventCallback<OverlayModalClickEventArgs>` | no | User clicked the modal backdrop (`IsModal="true"` only) |
| `OnDragStart` / `OnDrag` / `OnDragStop` | `EventCallback<DragStartEventArgs>` / `DragEventArgs` / `DragStopEventArgs` | no | Dragging lifecycle |
| `OnResizeStart` / `Resizing` / `OnResizeStop` | `EventCallback<MouseEventArgs>` | no | Resize lifecycle |
| `Created` / `Destroyed` | `EventCallback<object>` | no | Component lifecycle |
| `VisibleChanged` | `EventCallback<bool>` | n/a | Two-way binding mirror (wired by `@bind-Visible`) |

## Methods (via `@ref`)

| Method | Returns | Purpose |
|---|---|---|
| `ShowAsync(bool? isFullScreen = null)` | `Task` | Open programmatically (fires `OnOpen`) |
| `HideAsync()` / `HideAsync(string)` / `HideAsync(MouseEventArgs)` / `HideAsync(KeyboardEventArgs)` | `Task` | Close (fires `OnClose`, then `Closed`) |
| `RefreshPositionAsync()` | `Task` | Re-centre / re-position after a size or content change |
| `GetDimensionAsync()` | `Task<DialogDimension>` | Read current width / height |

> **Programmatic control caveat:** `ShowAsync()` / `HideAsync()` and `@bind-Visible`
> compete — pick one mechanism. Mixing them causes "two-state controllers"
> fights (see Don'ts).

## Animation

`DialogAnimationSettings` exposes:

| Field | Type | Notes |
|---|---|---|
| `Effect` | `DialogEffect` | `Fade`, `FadeZoom`, `Zoom`, `None`, `SlideLeft`, `SlideRight`, `SlideTop`, `SlideBottom`, `Flip*` |
| `Delay` | `double` (ms) | Default 0 |
| `Duration` | `double` (ms) | Default 400 |

Pick exactly one `Effect` per dialog.

## Common Use Cases

| Scenario | Key Properties |
|---|---|
| Confirmation modal | `IsModal=true`, `ShowCloseIcon=true`, `CloseOnEscape=true`, `AllowPrerender=false` (for interactive content) |
| Form-in-modal | `<EditForm>` inside `DialogTemplates.Content`; `AllowPrerender=false` |
| Alert/info dialog | `IsModal=false`, `ShowCloseIcon=true` |
| Wizard | Sequential `@bind-Visible` states per step (don't nest `<SfDialog>`) |
| Alert / confirm / prompt from code | Inject `SfDialogService` (`AlertAsync` / `ConfirmAsync` / `PromptAsync`) and render `<SfDialogProvider />` once at the app root (see [../SKILL.md](../SKILL.md), Pattern 3) |

## See Also

- [references/dialog-basics.md](references/dialog-basics.md) — installation, first modal walk-through
- [references/dialog-advanced.md](references/dialog-advanced.md) — service pattern, dynamic creator, wizard, cascading
- [../router/dialog-vs-tooltip-decision-matrix.md](../router/dialog-vs-tooltip-decision-matrix.md) — dialog vs tooltip decision matrix