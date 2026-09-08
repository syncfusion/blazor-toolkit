# Render tree efficiency — evidence report  

**MS rule:** Minimize render work. Override `ShouldRender` where appropriate; avoid unnecessary `StateHasChanged` calls; keep render trees small. Don’t trigger re-renders from every parameter change if the result is unchanged.

**Microsoft guidance:** [ASP.NET Core Blazor rendering](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/rendering)

---

## Summary

| Technique | Where applied |
|-----------|----------------|
| `ShouldRender` one-shot gate | Chart renderer stack |
| Controlled invalidation (render queue) | Chart children |
| Adaptive layer suppression | `SfChart` (markers / labels on small sizes) |
| Conditional markup (omit subtrees) | Dialog, Tooltip, Spinner, TimePicker popup, Uploader list |
| Change tracking before work | `SfBaseComponent.NotifyPropertyChanges`, ButtonGroup selection |
| Bounded / small trees | Calendar (~35–42 cells), single-field inputs, buttons |
| Imperative SVG builders (not per-point components) | Chart markers / series geometry |

| Component | Tree shape | Efficiency mechanism | Assessment |
|-----------|------------|----------------------|------------|
| **ChartRenderer** + children | Cascading + builder | `ShouldRender` + clear flag after paint | **Strong** |
| **SfChart** | Root + SVG + modules | Adaptive flags; render queue | **Strong** |
| **Calendar / DatePicker** | Fixed cell grid | Bounded size; `@key` reuse | **Good** |
| **TimePicker** | Flat `<ul>`/`<li>` when open | Popup gated; list size = interval | **OK / measure** |
| **Dialog** | Shared `DialogInner` fragment | Conditional sections + prerender gate | **Good** |
| **Tooltip / Spinner** | On-demand inner UI | `_renderWrapper` / `_enableRender` | **Good** |
| **Uploader** | Optional file list | `ShouldRenderFileList` | **Good** |
| **Button / simple inputs** | Minimal DOM | Default render; small tree | **Acceptable** |

---

## Component evidence

### ChartRenderer — gate + small controlled paint

**File:** [ChartRenderer.cs](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Charts/Chart/Renderer/ChartRenderer.cs)

- `ShouldRender() => RendererShouldRender`
- After `BuildRenderTree`, sets `RendererShouldRender = false`
- Children cascade only when a paint is allowed; avoids rebuilding on every parent pass

### Chart markers — incremental options, not full rebuild

**File:** [ChartMarkerRenderer.cs](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Charts/Chart/Renderer/SeriesRenderers/MarkerRenders/ChartMarkerRenderer.cs)

- Precomputes `_symbolOptions`; emits ellipse/path/image via SVG helpers  
- Color/fill/opacity can update cached options without full geometry rebuild  
- No per-point Razor component instances (keeps the logical tree smaller)

### SfChart — adaptive layers + controlled updates

**Files:**  
[SfChart.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Charts/Chart/SfChart.razor) · [SfChart.razor.cs](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Charts/Chart/SfChart.razor.cs)

- `_shouldRenderMarker` / data-label / stack-label flags drop expensive layers on small charts  
- `_svgRenderer?.ResetSequence()` once per render  
- Feature modules as dedicated children (selection, styles, tooltip data) rather than one monolithic tree  
- `@implements IHandleEvent` — intended to avoid auto full refresh on every DOM event (**confirm implementation**)

### Base — skip work when parameters unchanged

**File:** [SfBaseComponent.cs](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Base/SfBaseComponent.cs)

- `NotifyPropertyChanges` records deltas only when values differ  
- `PropertyChanges` cleared after each `OnAfterRenderAsync`  
- Does **not** globally override `ShouldRender` (opt-in per component)

### Calendar — bounded tree

**File:** [CalendarBaseRender.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Calendars/Base/Renderer/CalendarBaseRender.razor)

- Fixed matrix (~35–42 cells); not an unbounded list  
- `@key` on rows/cells limits recreate cost when navigating months  

### TimePicker — popup-scoped list

**File:** [SfTimePicker.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Calendars/TimePicker/SfTimePicker.razor)

- List DOM only when popup is shown  
- Simple `<li>` loop (efficient structure); cost scales with `ListData.Count`  

### Dialog — shared fragment + conditional sections

**File:** [SfDialog.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Popups/Dialog/SfDialog.razor)

- One `DialogInner` `RenderFragment` for modal and non-modal (no duplicated large trees)  
- Header / content / footer omitted when unused  
- Outer mount gated by prerender flags  

### Tooltip / Spinner — omit idle chrome

**Files:**  
[SfTooltip.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Popups/Tooltip/SfTooltip.razor) · [SfSpinner.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Spinner/SfSpinner.razor)

- Tooltip content only when `_renderWrapper`  
- Spinner graphics only when `_enableRender`  

### Uploader — optional list subtree

**File:** [SfUploader.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Inputs/Uploader/SfUploader.razor)

- File `<ul>` emitted only when `ShouldRenderFileList` is true  

### Buttons / simple inputs — inherently small trees

**Examples:** [SfButton](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Buttons/Button) · ButtonGroup [Button.razor.LifeCycle.cs](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Buttons/ButtonGroup/Button.razor.LifeCycle.cs)

- Single control DOM; ButtonGroup child uses `NotifyPropertyChanges` so selection logic runs only on real deltas  

---

## MS rule mapping

| Rule item | Toolkit response | Status |
|-----------|------------------|--------|
| Override `ShouldRender` where appropriate | Chart renderer stack | **Met** |
| Avoid unnecessary `StateHasChanged` | Chart render queue; `IHandleEvent` on chart (verify) | **Met / confirm** |
| Keep render trees small | Bounded calendar; conditional popups; SVG builders vs per-point components | **Met** |
| Don’t re-render when result unchanged | `NotifyPropertyChanges`; one-shot chart flag; conditional markup | **Met** (strongest on charts) |

---

## Conformance

| Claim | Status |
|-------|--------|
| Expensive chart children minimize paints via `ShouldRender` | **Met** |
| Subtrees omitted when idle (Dialog/Tooltip/Spinner/Uploader/TimePicker popup) | **Met** |
| Calendar and form controls stay small by design | **Met** |
| No unnecessary full-chart refresh on no-op events | **Confirm** (`IHandleEvent`) |
| TimePicker list stays small under dense intervals | **Measure** (see virtualization report) |

---

## Manual verification — Render tree efficiency

| ID | Component | What to do | Expected result (Pass) | Result | Owner / date |
|----|-----------|------------|------------------------|--------|--------------|
| **RTE-CH-01** | Chart | Load a chart with markers. Trigger a **parent-only** update (e.g. page counter) without changing series data or chart size. | Chart stays visually stable (no marker/series flicker). Child chart layers do **not** fully rebuild when nothing chart-related changed. | ☐ Pass / ☐ Fail / ☐ N/A | |
| **RTE-CH-02** | Chart | (1) Click empty plot area or move focus to a sibling control. (2) Then change real data or toggle series visibility. | Step 1: no full-chart redraw. Step 2: chart updates once as expected. No continuous redraw while idle. | ☐ Pass / ☐ Fail / ☐ N/A | |
| **RTE-CAL-01** | Calendar | Open month view. In dev tools, count day cells. Go next month, then back. | About **35–42** day cells each month—not hundreds. Same dates can reuse cells; UI stays correct. | ☐ Pass / ☐ Fail / ☐ N/A | |
| **RTE-DLG-01** | Dialog | Open a dialog configured **without** header and **without** footer (content only). Inspect DOM. | No header/footer blocks in the DOM. Only the content region is present (not empty hidden shells). | ☐ Pass / ☐ Fail / ☐ N/A | |
| **RTE-TIP-01** | Tooltip | Load page with tooltip target; do **not** open tooltip. Then show tooltip (hover/focus per sample). | Idle: no tooltip content chrome in DOM. After show: content appears. Hide again: content goes away. | ☐ Pass / ☐ Fail / ☐ N/A | |
| **RTE-UP-01** | Uploader | With files selected, turn **off** file-list rendering (`ShouldRenderFileList = false` or equivalent sample setting). Inspect DOM. | File list rows (`<li>`) are **gone** from the DOM—not only CSS-hidden. Drop zone can still show. | ☐ Pass / ☐ Fail / ☐ N/A | |
