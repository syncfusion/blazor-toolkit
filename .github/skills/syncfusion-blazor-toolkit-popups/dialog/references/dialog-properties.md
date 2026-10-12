---
license: MIT
name: dialog-properties
description: >
  Complete reference for Syncfusion Blazor Toolkit SfDialog properties,
  events, methods, and animation settings. Read when you need an
  authoritative signature or default value.
---

# SfDialog Properties & API Reference

Selected APIs checked against `src/Components/Popups/Dialog/`. Source is
authoritative for the full surface; commercial component docs may differ.

## Properties

| Property | Type | Default | Purpose |
|---|---|---|---|
| `Visible` | `bool` | `true` | Set false or bind a false field to start closed |
| `@bind-Visible` | `bool` | — | Generates `Visible` and `VisibleChanged`; do not duplicate them |
| `Header` / `Content` | `string` | unset | Header/body text |
| `FooterTemplate` | `string` | unset | Footer string; for fragments use `DialogTemplates.FooterTemplate` |
| `DialogTemplates` | child component | — | `Header`, `Content`, `FooterTemplate` are fragment slots |
| `Width` | `string` | `"100%"` | CSS width |
| `Height` | `string` | `"auto"` | CSS height |
| `IsModal` | `bool` | `false` | Modal backdrop and focus behavior |
| `AllowDragging` | `bool` | `false` | Drag header; may be used with modal dialogs |
| `EnableResize` | `bool` | `false` | Enable resize handles |
| `AllowPrerender` | `bool` | `false` | Retain dialog content while closed; not the host render mode |
| `CloseOnEscape` | `bool` | `true` | Escape dismissal |
| `ShowCloseIcon` | `bool` | `false` | Header close affordance; a Close/Cancel button is another option |
| `ZIndex` | `double` | `1000` | CSS stacking layer |
| `DialogAnimationSettings` | child component | — | `Effect`, `Duration`, `Delay` |
| `CssClass` | `string` | `""` | Custom root classes |

`IsResponsive`, direct `HeaderTemplate` / `ContentTemplate`, and `Footer`
are not `SfDialog` parameters. Use the template slots above.

## Events

| Event | Signature | Cancellable? | Fires |
|---|---|---|---|
| `OnOpen` | `EventCallback<BeforeOpenEventArgs>` | yes | Before opening |
| `Opened` | `EventCallback<OpenEventArgs>` | no | After opening |
| `OnClose` | `EventCallback<BeforeCloseEventArgs>` | yes | Before closing |
| `Closed` | `EventCallback<CloseEventArgs>` | no | After closing |
| `OnOverlayModalClick` | `EventCallback<OverlayModalClickEventArgs>` | no | Backdrop click; close explicitly if desired |
| `VisibleChanged` | `EventCallback<bool>` | n/a | Visibility state synchronization |

## Methods (via `@ref`)

| Method | Returns | Purpose |
|---|---|---|
| `ShowAsync(bool? isFullScreen = null)` | `Task` | Open programmatically |
| `HideAsync()` | `Task` | Request close through cancelable lifecycle |
| `RefreshPositionAsync()` | `Task` | Recalculate position |
| `GetDimensionAsync()` | `Task<DialogDimension>` | Read dimensions |
| `GetButton(int index)` | `DialogButton?` | Access a configured button |
| `GetButtonItems()` | `IList<DialogButton>?` | Access configured buttons |

Await JS-backed methods after the component is rendered interactively.
Binding can mirror programmatic changes; avoid redundant state mutations
for the same action. Component disposal is normally owned by Blazor.

## Animation

`DialogAnimationSettings` exposes:

| Field | Type | Notes |
|---|---|---|
| `Effect` | `DialogEffect` | `Fade`, `Zoom`, `None`, `SlideLeft`, `SlideRight`, `SlideDown`, `SlideUp` |
| `Duration` | `int` (ms) | Default 400 |

Configure one `DialogAnimationSettings` child per dialog. `Effect` and
`Duration` belong to that child, not to an `Animation` parameter on `SfDialog`.

## Common Use Cases

| Scenario | Key Properties |
|---|---|
| Confirmation modal | `IsModal=true`, explicit initial visibility, visible dismissal control |
| Form-in-modal | `<EditForm>` inside `DialogTemplates.Content`; configure an interactive host |
| Alert/info dialog | Choose `IsModal` according to whether the user must acknowledge it |
| Wizard | Application-owned step state and `@bind-Visible` |
| Service-driven dialog | `SfDialogService` with `SfDialogProvider`; see [dialog basics](dialog-basics.md) |

## See Also

- [dialog-basics.md](dialog-basics.md) — installation, first modal walk-through
- [dialog-advanced.md](dialog-advanced.md) — templates, forms, drag/resize, and composition
- [Dialog vs tooltip](../../router/dialog-vs-tooltip-decision-matrix.md) — decision matrix