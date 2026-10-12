# Render tree efficiency — source observations and manual plan

**Reviewed:** 2026-10-03, current local source. The mechanisms below are implementation observations, not measured benchmark results, runtime pass/fail evidence, a waiver, or certification. No builds, tests, or profiling were run for this documentation synchronization.

## Summary

| Mechanism | Source observation | Unmeasured limitation |
|-----------|--------------------|-----------------------|
| `ShouldRender` gates | Chart renderer stack; Tooltip also overrides it | Does not prove every event avoids work or that updates are correct |
| Render queue | Chart children request `StateHasChanged` | Scheduling exists; deduplication/minimal paints need measurement |
| Adaptive layer suppression | `SfChart` size-dependent marker/label flags | Not general point-count windowing; behavior varies by feature/series |
| Conditional subtrees | Dialog, Tooltip, Spinner, time popups, uploader file list | Hidden/mounted/retained state and browser DOM need separate checks |
| Change tracking | Base helper and component lifecycle code | Tracks deltas, not a global rendering veto |
| Eager fixed calendar | Month matrix uses 42 slots; year/decade 12 | Hidden rows and date bounds affect visible/generated counts |
| SVG builders **and components** | Marker helpers open `SvgEllipse` / `SvgPath` / `SvgImage` | Per-point component and DOM growth can occur |
| Keyed repeated UI | Chart selection/patterns, TextBox icons and other loops | Keys do not guarantee universal reuse; see [key observations](./key-usage.md) |

No “Strong/Good/Met” performance rating is assigned from source inspection alone.

---

## Component evidence

Paths are relative to the repository root; these observations describe the local tree, not the historical `readiness-corrections` branch or a released package.

### ChartRenderer and marker pipeline

- `src/Components/Charts/Chart/Renderer/ChartRenderer.cs`: `ShouldRender` returns `RendererShouldRender`; `BuildRenderTree` clears it after building the cascading child content. `ProcessRenderQueue` invokes `StateHasChanged`; it does not itself prove a request was necessary.
- `src/Components/Charts/Chart/Renderer/SeriesRenderers/MarkerRenders/ChartMarkerRenderer.cs`: computes `_symbolOptions` for visible/in-range point locations and iterates those options in `RenderMarkers`. Color/fill/opacity paths update cached options. Its own `ShouldRender` also permits a pending Static SSR pass.
- `src/Components/Charts/Common/Renderer/SvgRendering.cs`: `RenderEllipse`, `RenderPath`, and `RenderImage` call `OpenComponent<SvgEllipse>`, `OpenComponent<SvgPath>`, and `OpenComponent<SvgImage>`. Thus marker rendering **can create per-point SVG component instances and DOM nodes**. The old “not per-point components” claim was incorrect; geometry aggregation varies by series and feature.

### SfChart adaptive flags and event handling

- `src/Components/Charts/Chart/SfChart.razor.cs`: `SetAdaptiveWidthCategory` and `SetAdaptiveHeightCategory` adjust `_shouldRenderMarker`, `_shouldRenderDataLabel`, and (for width) `_shouldRenderStackLabel` when adaptive rendering is enabled. This is size-based suppression, not proof of bounded point cost.
- `SfChart.razor` resets SVG sequence bookkeeping and includes feature children/fragments. It declares `@implements IHandleEvent`, but no custom `HandleEventAsync` implementation was found in the inspected chart/base source. The declaration alone does not bypass the inherited component event/render behavior; no “no-op events never redraw” claim is supported here.

### Base and supporting components

- `src/Base/SfBaseComponent.cs`: `NotifyPropertyChanges` records differences; it does not supply a global `ShouldRender` override. Delta tracking is distinct from preventing a render pass.
- `src/Components/Calendars/Base/Renderer/CalendarBaseRender.razor` and its constants/code-behind: eager 42-slot month matrix (12 cells for year/decade), with hidden-row/date-bound behavior. Row/cell keys have sibling-scope limitations, not guaranteed cross-month reuse.
- `src/Components/Calendars/TimePicker/SfTimePicker.razor` and `DateTimePicker/SfDateTimePicker.razor`: popup branches loop over all `ListData`. Generation is eager; see [virtualization observations](./virtualization-startegy.md).
- `src/Components/Popups/Dialog/SfDialog.razor`: shared `DialogInner` fragment, conditional header/footer/content and prerender mount conditions. `AllowPrerender` can retain mounted content; visibility is not equivalent to absence from the DOM.
- `src/Components/Popups/Tooltip/SfTooltip.razor`: `_renderWrapper` gates the inner content; the outer target wrapper persists. `SfTooltip.razor.LifeCycle.cs` also implements `ShouldRender`: returns `_shouldRender` and resets it to `true`; `PreventRender` changes that flag.
- `src/Components/Spinner/SfSpinner.razor`: `_enableRender` gates graphics; this is not a component-wide render veto.
- `src/Components/Inputs/Uploader/SfUploader.razor`: private computed `ShouldRenderFileList` conditionally emits the file list; the public display parameter is `ShowFileList`. Neither establishes file-count/byte bounds.
- `src/Components/Inputs/TextBox/SfTextBox.razor`: keyed icon loop over `ListOfButtons`; not all inputs are single-node trees. ButtonGroup change tracking controls its selection logic, not every child render.

---

## Evidence limits

The former MS-rule “Met” ratings are not supported by this source review. Render gates, small fixed matrices, and conditional subtrees are observable mechanisms, but minimal work, correct invalidation, memory bounds, and responsiveness require runtime evidence. No benchmark or accepted exception is supplied here.

## Manual verification plan — not executed

For each scenario retain commit/runtime/render mode/browser/hardware, input size and enabled features, render/component/DOM counts, traces, latency and memory observations, and reviewer. Measure from before the operation, not after navigation/network idle. Agree budgets separately; absence of flicker is not proof that no render occurred.

| ID | Component | Planned check | Evidence to collect | Status |
|----|-----------|---------------|---------------------|--------|
| **RTE-CH-01** | Chart | Parent-only update with stable data/size; vary point count and marker/label settings. | Render/geometry invocation counts, SVG/component counts, correctness, latency and heap. | Not executed |
| **RTE-CH-02** | Chart | No-op interactions versus data/visibility changes. | Event path, actual scheduled renders and redraw work; do not assume `IHandleEvent` suppresses them. | Not executed |
| **RTE-CAL-01** | Calendar | Navigate views and date bounds. | Generated vs visible cells and component identity; hidden rows recorded separately. | Not executed |
| **RTE-DLG-01** | Dialog | Header/footer absent/present; prerender on/off; show/hide. | Which subtrees mount, hide or unmount and whether retained state is correct. | Not executed |
| **RTE-TIP-01** | Tooltip | Show/hide and parameter updates; exercise `PreventRender`. | Persistent wrapper vs inner content, JS-retained DOM, gate behavior and subsequent update correctness. | Not executed |
| **RTE-UP-01** | Uploader | Exercise supported file-list display configuration and both template branches. | Internal `ShouldRenderFileList` state, rendered rows, retained data and cost; do not expose internal state as a public API. | Not executed |

See [enterprise plan R23](../../../docs/ENTERPRISE-READINESS-PLAN.md) for the outstanding reproducible Chart benchmark gate.
