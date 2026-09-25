# ShouldRender optimization — evidence report

**Microsoft guidance:** [Suppress UI refreshing with `ShouldRender`](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/rendering)

---

## Summary

Formal `ShouldRender` overrides are used where render cost is highest (chart renderer stack). Other components use default `ShouldRender` or equivalent gates (conditional markup / adaptive flags / change tracking). That matches Microsoft guidance: gate expensive updates; keep simple controls simple.

| Component | `ShouldRender` override? | Equivalent gate | Assessment |
|-----------|--------------------------|-----------------|------------|
| **ChartRenderer** (+ chart children) | **Yes** | One-shot `RendererShouldRender` | Primary evidence |
| **ChartMarkerRenderer** | Inherited | Adaptive + incremental updates | Strong |
| **SfChart** | No | Adaptive layer flags (`_shouldRenderMarker`, etc.) | Intentional |
| **SfUploader** | No | `ShouldRenderFileList` (omit file list) | Equivalent gate |
| **SfDialog** | No | Prerender / visibility mount | Equivalent gate |
| **SfTooltip** | No | `_renderWrapper` | Equivalent gate |
| **SfSpinner** | No | `_enableRender` | Equivalent gate |
| **TimePicker / DatePicker** | No | Popup visibility | Equivalent gate |
| **ButtonGroup child** | No | `NotifyPropertyChanges` | Logic only |
| **SfButton, simple inputs, Calendar** | No | — / bounded grid | Default OK |

---

## Component evidence

### ChartRenderer (formal override)

**File:** [ChartRenderer.cs](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Charts/Chart/Renderer/ChartRenderer.cs)

- `protected override bool ShouldRender() => RendererShouldRender;`
- After `BuildRenderTree`, sets `RendererShouldRender = false` (one-shot allow)
- Invalidation via render queue → `StateHasChanged` only when needed

### Chart markers (flag + incremental work)

**File:** [ChartMarkerRenderer.cs](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Charts/Chart/Renderer/SeriesRenderers/MarkerRenders/ChartMarkerRenderer.cs)

- Sets `RendererShouldRender` from visibility / adaptive owner flags  
- Color/fill paths can update cached options without full geometry rebuild  

### SfChart (adaptive layers, not root `ShouldRender`)

**Files:**  
[SfChart.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Charts/Chart/SfChart.razor) · [SfChart.razor.cs](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Charts/Chart/SfChart.razor.cs)

- `_shouldRenderMarker` / data-label / stack-label flags suppress expensive layers on small sizes  

### Base (change-tracking helper, no override)

**File:** [SfBaseComponent.cs](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Base/SfBaseComponent.cs)

- No `ShouldRender` override  
- `NotifyPropertyChanges` + `PropertyChanges` for selective logic in derived types  

### Equivalent gates (not `ComponentBase.ShouldRender`)

| Component | Gate | File |
|-----------|------|------|
| Uploader | `ShouldRenderFileList` | [SfUploader.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Inputs/Uploader/SfUploader.razor) |
| Dialog | `AllowPrerender` / `IsPreRender` | [SfDialog.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Popups/Dialog/SfDialog.razor) |
| Tooltip | `_renderWrapper` | [SfTooltip.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Popups/Tooltip/SfTooltip.razor) |
| Spinner | `_enableRender` | [SfSpinner.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Spinner/SfSpinner.razor) |
| TimePicker | `ShowPopupList` | [SfTimePicker.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Calendars/TimePicker/SfTimePicker.razor) |
| ButtonGroup child | `NotifyPropertyChanges` | [Button.razor.LifeCycle.cs](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Buttons/ButtonGroup/Button.razor.LifeCycle.cs) |

### Default `ShouldRender` (acceptable)

| Component | File |
|-----------|------|
| SfButton | [SfButton.razor.LifeCycle.cs](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Buttons/Button/SfButton.razor.LifeCycle.cs) |
| Calendar grid | [CalendarBaseRender.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Calendars/Base/Renderer/CalendarBaseRender.razor) |

---

## Conformance

| Claim | Status |
|-------|--------|
| Expensive chart children override `ShouldRender` with a one-shot flag | **Met** |
| Lightweight controls use default or conditional markup | **Met** |
| Base supplies change-tracking for selective updates | **Met** |

---

## Manual verification — ShouldRender

| ID | Component | What to do | Expected result (Pass) | Result | Owner / date |
|----|-----------|------------|------------------------|--------|--------------|
| **SR-CH-01** | Chart markers | Load chart with markers. Trigger a **parent-only** re-render (e.g. page counter) without changing chart data or size. | Markers stay stable (no flicker). Marker rebuild path does **not** run; unrelated UI still updates. | ☐ Pass / ☐ Fail / ☐ N/A | |
| **SR-CH-02** | Chart markers | With markers visible, change **only** marker color/fill/opacity (no data or size change). | Color updates correctly. No full geometry rebuild or heavy redraw when only style changed. | ☐ Pass / ☐ Fail / ☐ N/A | |
| **SR-CH-03** | SfChart | Interact with no-op actions (empty plot click, sibling focus) vs a real data/visibility change. | No-op: no full-chart flicker. Real change: chart updates as designed. No idle redraw loop. | ☐ Pass / ☐ Fail / ☐ N/A | |
| **SR-UP-01** | Uploader | Turn off file-list rendering (`ShouldRenderFileList = false`) with files present in the model if applicable. | File `<li>` rows are **not** in the DOM. Drop zone/input can remain; list region is omitted. | ☐ Pass / ☐ Fail / ☐ N/A | |