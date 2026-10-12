# `@key` usage — source observations and manual plan

**Reviewed:** 2026-10-03, current local source. Key presence is not a benchmark, identity/reuse test result, MS-5.2 conformance pass, or certification. No runtime verification was performed for this documentation synchronization.

## Summary

Keys exist in calendar, time-popup, chart, TextBox, dialog, and default uploader loops. Coverage is not universal: uploader template rows have no explicit key, and SVG builder paths are not all keyed. Keys are scoped to siblings; they neither virtualize collections nor guarantee reuse after an item/subtree is removed.

| Component | Collection | Observed key | Limitation |
|-----------|------------|--------------|------------|
| Calendar grid/header | Rows, cells, weekday headings | `dayCells`, `localCalDate.Ticks`, `days` | Row/header indices; date identity only within its containing row |
| DatePicker / DateTimePicker date view | Shared calendar renderer | As above | Same sibling-scope caveat |
| TimePicker / DateTimePicker time view | Time `<li>` items | `item` | Object identity; generation creates new list-item instances |
| Dialog | Footer buttons | `i` | Positional, not stable button identity after insert/remove/reorder |
| Uploader default rows | File `<li>` items | `Name + ":" + listIndex` | Duplicate names get different indices, but reindexing changes identity |
| Uploader template rows | File `<li>` items | **None** | No explicit key on this branch |
| TextBox | `ListOfButtons` icon spans | `icon` | Object identity; replacement/reordering needs verification |
| Chart selection | Rectangle/path selections | `item.Id` | Requires unique stable IDs among siblings |
| Chart patterns | Pattern components and shapes | `item.Id`; geometry/hash-derived shape keys | Equal geometry/hash collisions and mutation need review |
| Chart renderer builders | Series/axis/trendline children | `SetKey` with renderer identity/suffixes | Not a claim that every SVG primitive is keyed |

---

## Source evidence and identity caveats

Paths below are relative to the repository root.

- `src/Components/Calendars/Base/Renderer/CalendarBaseRender.razor` keys rows by index and `CalendarDayCell` by date ticks; `CalendarTableHeader.razor` keys weekday headings by index. A date moving between rows is not guaranteed to retain its component instance.
- `src/Components/Calendars/TimePicker/SfTimePicker.razor` and `DateTimePicker/SfDateTimePicker.razor` key time options by `item`. Their generation methods create new `ListOptions<TValue>` objects. Do not promise retained identity across popup unmount or list regeneration; display text alone is not necessarily a unique replacement key.
- `src/Components/Popups/Dialog/SfDialog.razor` uses index keys. Fixed-order updates and dynamic footer reordering must be evaluated separately.
- `src/Components/Inputs/Uploader/SfUploader.razor` keys only the default row branch. The `Name + ":" + listIndex` expression distinguishes same-name rows at different indices but changes when earlier rows are removed. The template `<li>` has no `@key`; the source readiness comment does not prove universal stable identity.
- `src/Components/Inputs/TextBox/SfTextBox.razor` renders keyed `ListOfButtons` icon spans; inputs cannot all be categorized as having no internal collection.
- `src/Components/Charts/Chart/Renderer/UserInteractions/Selection/SvgSelectionRectCollection.razor` keys rectangle/path selection components by `item.Id`.
- `src/Components/Charts/Common/BaseSelection/SelectionStyleComponent.razor` keys `SvgPattern` by `item.Id`. `Common/SvgComponents/SvgPattern.razor` uses coordinate-based rectangle/ellipse keys and `GetHashCode()`-based path keys. These are not universally unique semantic IDs.
- `src/Components/Charts/Chart/Renderer/SeriesRenderers/BaseRenderers/SeriesContainer.cs`, `AxisRenderer/ChartAxisContainer.cs`, and `TrendlineRenderer/TrendlineContainer.cs` contain `builder.SetKey` calls. Conversely, helpers in `src/Components/Charts/Common/Renderer/SvgRendering.cs` open SVG components without a `SetKey` in the inspected primitive methods. Builder rendering is not evidence that keys are inapplicable or unnecessary everywhere.

No source or key strategy is changed by this report. Any corrective implementation needs a separate scope and regression evidence.

---

## Manual verification — `@key` usage (not executed)

Record commit/runtime/render mode/browser, component configuration, mount/disposal or DOM-identity traces, observed behavior, and reviewer. Visual stability alone is not proof of instance reuse. Do not assume reuse across removed subtrees; define expected identity for each supported mutation before judging results.

| ID | Component | Planned check / evidence | Status |
|----|-----------|--------------------------|--------|
| KEY-CAL-01 | Calendar | Navigate months; distinguish dates retained in the same row from dates moved/removed. Record cell identity and selection correctness. | Not executed |
| KEY-CAL-02 | Calendar | Year/decade and supported Islamic views; inspect duplicate-key errors and correct period mapping. | Not executed |
| KEY-TP-01 | TimePicker / DateTimePicker | Close/reopen and regenerate time lists; record remounts separately from selection correctness. | Not executed |
| KEY-TP-02 | TimePicker / DateTimePicker | Change selection while mounted; trace item identity and affected styling. | Not executed |
| KEY-DLG-01 | Dialog | Text/disabled updates, then insert/remove/reorder footer buttons; verify focus and handler association. | Not executed |
| KEY-UP-01 | Uploader | Same-name files in default and template branches; then remove/reorder rows. Record status/focus association and identity changes. | Not executed |
| KEY-UP-02 | Uploader | Progress/status-only update with unchanged order; trace row identity rather than inferring it from appearance. | Not executed |
| KEY-TXT-01 | TextBox | Add/remove/reorder/replace icon objects; verify handlers, focus, and duplicate-key behavior. | Not executed |
| KEY-CH-01 | Chart | Add/remove selection rectangles and pattern shapes, including repeated geometry; inspect IDs/hash collisions and component reuse. | Not executed |

Performance effects and any key changes remain unmeasured; see [render-tree observations](./render-tree-efficiency.md).