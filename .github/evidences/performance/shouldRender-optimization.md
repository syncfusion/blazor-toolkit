# ShouldRender optimization — source observations and manual plan

**Reviewed:** 2026-10-03, current local source. No runtime verification or benchmark was run for this documentation synchronization. A render gate's presence is not a performance pass, approved waiver, or certification.

## Summary

Formal `ShouldRender` overrides exist in chart renderers **and `SfTooltip`**. Conditional markup and change tracking are separate mechanisms, not equivalent component-wide vetoes. `ShouldRender` does not suppress a component's initial render or prove that parameter processing, geometry work, or JS calls were avoided.

| Component | Observed `ShouldRender` behavior | Other mechanism / limitation |
|-----------|----------------------------------|------------------------------|
| **ChartRenderer** | Returns `RendererShouldRender`; base build clears it | Render queue schedules `StateHasChanged`; need/cost not measured |
| **ChartMarkerRenderer** | Own override: renderer flag OR pending Static SSR pass | `_staticSSRRenderPending` cleared after the static pass; cached option updates |
| **Other chart renderers** | Several independent overrides | Review per renderer; not all gates share one implementation |
| **SfChart** | No root override found | Size-based adaptive layer flags; not a global event/render bypass |
| **SfTooltip** | Returns `_shouldRender`, then resets it to `true` | `PreventRender` sets the flag; `_renderWrapper` separately gates inner content |
| **SfUploader** | No override found | Internal `ShouldRenderFileList` omits only the list subtree |
| **SfDialog / SfSpinner** | No override found | Prerender/mount conditions / `_enableRender` graphics condition |
| **TimePicker / DatePicker / DateTimePicker** | No override found | Popup branch conditions; generated time lists remain eager |
| **ButtonGroup child / buttons / inputs / Calendar** | No override found in inspected paths | Change tracking or default lifecycle; no blanket “default OK” performance rating |

---

## Component evidence

Paths are relative to the repository root; observations refer to the current local tree.

### Chart renderer gates

- `src/Components/Charts/Chart/Renderer/ChartRenderer.cs`: `ShouldRender` returns `RendererShouldRender`; `BuildRenderTree` clears it. `ProcessRenderQueue` schedules `StateHasChanged` without a separate “only when needed” test in that method.
- `src/Components/Charts/Chart/Renderer/SeriesRenderers/MarkerRenders/ChartMarkerRenderer.cs`: its override returns `RendererShouldRender || (IsStaticSSR() && _staticSSRRenderPending)`. `BuildRenderTree` clears the static pending flag. Color/fill/opacity methods can update cached symbol options; this does not establish measured savings or correct invalidation for every configuration.
- Other overrides occur in `ChartElementRenderer.cs`, `CustomLegendRenderer.cs`, `AxisRenderer/ChartAxisContainer.cs`, `AxisRenderer/ChartAxisOutsideContainer.cs`, `SeriesRenderers/GradientRenderer/ChartGradientRenderer.cs`, and `UserInteractions/ZoomToolkit.cs` under the same renderer directory. Presence alone is not evidence that all paths share a one-shot gate.
- `src/Components/Charts/Chart/SfChart.razor.cs` uses size-dependent adaptive flags. The `IHandleEvent` declaration in `SfChart.razor` is not a custom event implementation or proof that no-op events avoid rendering; see [render-tree observations](./render-tree-efficiency.md).

### Tooltip's actual override

`src/Components/Popups/Tooltip/SfTooltip.razor.LifeCycle.cs` implements both `PreventRender(bool preventRender = true)` and `ShouldRender()`:

- `PreventRender` sets `_shouldRender = !preventRender`.
- `ShouldRender` saves the current flag, resets `_shouldRender = true`, and returns the saved value. This is a **one-check suppression**, not a permanent disabled-render state.
- `src/Components/Popups/Tooltip/SfTooltip.razor` separately uses `_renderWrapper` for inner content; its outer wrapper/child content is distinct. Skipping a render and omitting a subtree must not be conflated.

### Base and conditional subtrees

- `src/Base/SfBaseComponent.cs`: `NotifyPropertyChanges` and `PropertyChanges` track differences for derived logic; no global `ShouldRender` override.
- `src/Components/Inputs/Uploader/SfUploader.razor`: internal `ShouldRenderFileList` gates the file list, not the whole component or retained data.
- `src/Components/Popups/Dialog/SfDialog.razor`: prerender/mount conditions can retain content while hidden.
- `src/Components/Spinner/SfSpinner.razor`: `_enableRender` conditions graphics, not all lifecycle work.
- Calendar input Razor files condition popup branches, but TimePicker/DateTimePicker generate and render complete time lists. See [virtualization observations](./virtualization-startegy.md).
- `src/Components/Buttons/ButtonGroup/Button.razor.LifeCycle.cs`: change tracking controls selection work, not a component-level render veto.

---

## Evidence limits

The earlier “Met” entries conflated observed code with runtime verification. Minimal rendering, correct invalidation, cost savings, and applicability to a particular review requirement remain unverified by this report. No benchmark, accepted N/A waiver, or certification is claimed.

## Manual verification plan — not executed

Record commit/runtime/render mode/browser, feature settings, render/geometry counters or traces, elapsed time/memory and reviewer. Measure correctness as well as work avoided; no flicker is insufficient evidence of no render. Initial render and Static SSR behavior need separate observations.

| ID | Component | Planned check / evidence | Status |
|----|-----------|--------------------------|--------|
| **SR-CH-01** | Chart markers | Parent-only update with stable data/size; trace gate calls, geometry calculations and renders, including initial/pending Static SSR handling. | Not executed |
| **SR-CH-02** | Chart markers | Style-only versus geometry changes; record cached-option and rebuild paths and resulting SVG correctness. | Not executed |
| **SR-CH-03** | SfChart | No-op interactions versus data/visibility changes; record actual event/render counts without assuming a root override. | Not executed |
| **SR-UP-01** | Uploader | Exercise supported file-list configuration; observe internal gate, actual DOM omission and retained model independently. | Not executed |
| **SR-TIP-01** | Tooltip | After initial render, call `PreventRender`, request an update, then request another; verify one-check flag reset and eventual correct content. Test `PreventRender(false)` separately. | Not executed |

See [enterprise plan R23](../../../docs/ENTERPRISE-READINESS-PLAN.md) for the outstanding measured-performance gate.