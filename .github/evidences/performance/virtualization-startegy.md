# Virtualization strategy — source observations and manual plan

**Reviewed:** 2026-10-03, current local source. No benchmark or runtime verification was performed for this documentation synchronization. This is not a conformance pass, approved waiver, or certification. The historical filename `virtualization-startegy.md` is retained for stable links.

## Summary

| Finding | Source observation / limitation |
|---------|---------------------------------|
| `<Virtualize>` / `PageSize` | Neither an implementation nor a paging API was found under `src/Components`; no dropdown component implementation was found there either. Descriptive references to dropdowns are not such an API. |
| Collection bounds | Calendar has a fixed matrix. Time lists are finite but eager; dialog/file/icon collections scale with supplied data/configuration. Finite does not mean small or measured fast. |
| Consumer virtualization | App-owned large lists can use Blazor `<Virtualize>`. That does not virtualize toolkit-owned popup lists or chart SVG. |

| Component | List / surface | Observed strategy |
|-----------|----------------|-------------------|
| Calendar / DatePicker / DateTimePicker date view | Calendar cells | Eager fixed matrix; month uses 42 slots, year/decade 12 |
| TimePicker | Popup `ListData` | Eager generation and full `@for`; time range and positive minute `Step` determine count |
| DateTimePicker time view | Popup `ListData` | Eager generation and full `@for` in both popup branches; time/date constraints and `Step` determine count |
| Dialog | Footer buttons | Full configured `ButtonsValue` loop; no hard small-count cap established |
| Uploader | File rows | Full `FileData` loop when `ShouldRenderFileList`; no demonstrated aggregate count/byte budget |
| Chart | Series/markers/labels | SVG components and builders, conditional/adaptive layers; not scroll-window virtualization |
| TextBox | Added icons | Full `ListOfButtons` loop; supplied collection size matters |

---

## Component evidence

Source paths below are relative to the repository root and describe this working tree, not a released artifact.

### Calendar grid (fixed matrix, eager rendering)

- `src/Components/Calendars/Base/Renderer/CalendarBaseRender.razor` loops through rows and `CalendarDayCell` components.
- `CalendarBaseRender.razor.Constant.cs` defines `CELLCOUNT = 42`, `WEEK_NUMBER = 7`, `YEAR_NUMBER = 12`, and `CELL_ROW = 4`; the code-behind selects these for the view.
- Month rows can be CSS-hidden and maximum-date checks can stop cell emission. Visible cell count is not the same as generated cell count. This is a bounded eager table, not virtualization; `@key` does not change that.

### TimePicker / DateTimePicker (finite, eager time lists)

- `src/Components/Calendars/TimePicker/SfTimePicker.razor.cs`, `GenerateListAsync`: initializes `ListData`, then appends every time from `Min.TimeOfDay` to `Max.TimeOfDay` inclusive in `Step`-minute increments. `NormalizeStep` preserves positive supported intervals (invalid values fall back/coerce); it does not window the list.
- `src/Components/Calendars/DateTimePicker/SfDateTimePicker.razor.cs`, `GenerateList`: uses date-dependent start/end times plus `MinTime`/`MaxTime` and a positive `Step`, then appends the full range. Do not assume TimePicker's normalization also applies here.
- Both Razor files render all generated items when their time-popup branch is mounted. For a valid full-day range, a one-minute step can yield 1,440 entries; this is a source-derived count, **not a measured DOM count or timing result**.
- No `PageSize`, virtualized dropdown, or viewport-windowing implementation was found in these generation/render paths. The previous MS-5.4 paging assertion is unsupported and is not an accepted N/A disposition.

### Dialog / Uploader / TextBox (configuration is not a resource budget)

- `src/Components/Popups/Dialog/SfDialog.razor` renders the configured footer buttons. A typically small author-selected list is not an enforced limit.
- `src/Components/Inputs/Uploader/SfUploader.razor` renders `FileData` eagerly. `MaxFileSize` and `AllowMultiple` in `SfUploader.razor.Members.cs` do not establish an aggregate file-count/retained-byte budget. Both default and template rows need consideration.
- `src/Components/Inputs/TextBox/SfTextBox.razor` iterates `ListOfButtons` for icons. This is not a general-purpose virtualized list host.

### Chart (SVG cost still scales with enabled features)

- `src/Components/Charts/Chart/Renderer/SeriesRenderers/MarkerRenders/ChartMarkerRenderer.cs` iterates visible/in-range point symbols.
- `src/Components/Charts/Common/Renderer/SvgRendering.cs` opens `SvgEllipse`, `SvgPath`, and `SvgImage` components for those symbols. Per-point component/DOM growth is possible; builder use does not eliminate it.
- `SfChart.razor.cs` has size-dependent adaptive flags, not a general point-count budget or viewport virtualization. See [render-tree observations](./render-tree-efficiency.md).

---

## Evidence limits and outstanding decisions

The prior MS-5.4 mapping is a review topic, not proof of compliance. Source establishes eager/fixed/configured rendering only. Runtime cost, acceptable resource budgets, and any formally reviewed applicability decision remain outstanding. Do not create a waiver or mark a pass from this document.

## Manual verification plan — not executed

Record commit, SDK/runtime, render mode, browser/hardware, exact configuration, generated and visible counts, timings/heap, traces, and reviewer. Agree budgets from measurements before assigning pass/fail. These rows are investigation plans, not results.

| ID | Component | Planned check | Evidence to collect | Status |
|----|-----------|---------------|---------------------|--------|
| **VIRT-CAL-01** | Calendar | Navigate month/year/decade, date bounds and week-number modes. | Generated vs visible cells/rows; confirm fixed-matrix behavior and correctness. | Not executed |
| **VIRT-TP-01** | TimePicker | Open full-day lists at default and one-minute `Step`. | Actual item/DOM counts, opening latency, memory and keyboard behavior; no assumed window. | Not executed |
| **VIRT-TP-02** | TimePicker / DateTimePicker | Exercise date/time boundaries and valid/invalid step settings separately. | Generated range/count, eager popup cost, and product decision on supported budgets; no assumed shared normalization. | Not executed |
| **VIRT-UP-01** | Uploader | Repeated selections, duplicate names, removal/clear, default and template rows. | Retained file/row/byte growth and any host-enforced limits; establish aggregate budget separately. | Not executed |
| **VIRT-CH-01** | Chart | Supported large-series workload (e.g. 10k points), markers/labels on/off and adaptive sizes. | SVG/component/DOM counts, latency and heap for each configuration; identify any sampling explicitly. | Not executed |

See [enterprise plan R07/R23](../../../docs/ENTERPRISE-READINESS-PLAN.md) for unresolved resource and performance gates.