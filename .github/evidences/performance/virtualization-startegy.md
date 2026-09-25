# Virtualization strategy — evidence report

**Microsoft guidance:** [ASP.NET Core Blazor virtualization](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/virtualization)

**Partner attestation:** [ms-bar-attestations.md — MS-5.4](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/.github/ms-bar-attestations.md)

---

## Summary

| Finding | Detail |
|---------|--------|
| `<Virtualize>` in product components | **Not used** under `src/Components` |
| Compliance path used | **Bounded collections** (fixed or config-capped size) — MS-5.4 “documented N/A path is intentional” |
| Consumer virtualization API | **Not exposed** as a first-class “wrap content in `<Virtualize>`” pattern for internal lists (lists are owned by the component) |

MS allows either **use `<Virtualize>`** or **document why it is not needed / how consumers virtualize**. This toolkit relies on **small, bounded lists**, not scroll-window virtualization.

| Component | List / surface | Strategy | `<Virtualize>`? |
|-----------|----------------|----------|-----------------|
| Calendar / DatePicker grid | Day cells | Fixed ~35–42 cells | No — not required |
| TimePicker | Popup `ListData` | Full `@for` over list; size driven by interval | No — **measure / confirm bounds** |
| DateTimePicker | Date grid + time list | Same as calendar + time list | No |
| Dialog | Footer buttons | Config-sized | No — not required |
| Uploader | File rows | Bounded by upload limits; `@key` on rows | No — not required |
| Chart | Series points | SVG/builder + adaptive layers (not a DOM list) | N/A |
| Button, inputs, Tooltip, Spinner | No large list | N/A | N/A |

---

## Component evidence

### Calendar grid (fixed bound)

**File:** [CalendarBaseRender.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Calendars/Base/Renderer/CalendarBaseRender.razor)

- Month view builds a **fixed table** of day cells (typical **35–42** cells), not an unbounded scroll list.
- `@key` on rows/cells supports reuse; virtualization is unnecessary for this size.

### TimePicker (eager list — main risk)

**File:** [SfTimePicker.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Calendars/TimePicker/SfTimePicker.razor)

```razor
@for (int i = 0; i < ListData.Count; i++)
{
    var item = ListData[i];
    <li @key="@item" ...>@item.ItemData</li>
}
```

- **No** `<Virtualize>`.
- Renders **all** `ListData` items when the popup is open.
- MS-5.4 claims windowing via `PageSize`; that logic is **not** visible in this Razor loop and must be confirmed in list-generation code-behind (**manual**).

### Dialog (config-bounded)

**File:** [SfDialog.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Popups/Dialog/SfDialog.razor)

- Footer buttons loop over `ButtonsValue` (author-configured, small N).
- Not a large data list → `<Virtualize>` not required.

### Uploader (limit-bounded)

**File:** [SfUploader.razor](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Inputs/Uploader/SfUploader.razor)

- `@foreach` over `FileData`; list can be omitted via `ShouldRenderFileList`.
- Practical bound: max files / size rules (see component API); not an infinite scroll surface.

### Chart (not a virtualized DOM list)

**Files:**  
[ChartRenderer.cs](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Charts/Chart/Renderer/ChartRenderer.cs) · [SfChart.razor.cs](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Charts/Chart/SfChart.razor.cs)

- Points/markers use **SVG / render-tree builders** and adaptive visibility, not a Razor item list.
- `<Virtualize>` does not apply to this model.

### Simple controls

**Examples:** [SfButton](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Buttons/Button), inputs, [SfTooltip](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Popups/Tooltip/SfTooltip.razor), [SfSpinner](https://github.com/syncfusion/blazor-toolkit/blob/readiness-corrections/src/Components/Spinner/SfSpinner.razor) — no large repeated collections.

---

## MS rule mapping

| MS expectation | Toolkit response | Status |
|----------------|------------------|--------|
| Use `<Virtualize>` for large lists | Not used in product components | **N/A by design** for fixed/config-bounded UIs |
| Or document why / how consumers virtualize | MS-5.4: bounded sets; N/A path intentional | **Documented** in attestation |
| Large or unbounded internal lists | TimePicker `ListData` is the main case to prove stays bounded | **Confirm** `PageSize` / interval caps |

**Gap to close (if MS challenges):**  
1) Point to **code** that caps TimePicker/DateTimePicker `ListData` (or add `<Virtualize>` / windowing).  
2) Optionally add a short **consumer note** in docs: for app-owned large lists, use Blazor `<Virtualize>`; toolkit popups are not general-purpose virtualized list hosts.

---

## Conformance

| Claim | Status |
|-------|--------|
| No unbounded calendar grid | **Met** (fixed cell matrix) |
| Dialog / Uploader lists config- or limit-bounded | **Met** |
| Chart not a DOM list virtualization scenario | **Met** |
| `<Virtualize>` used for large product lists | **Not used** — intentional N/A for current surface |
| TimePicker list always small / windowed | **Attested (MS-5.4); verify in code + runtime** |

---

## Manual verification

| ID | Component | What to do | Expected result (Pass) | Result | Owner / date |
|----|-----------|------------|------------------------|--------|--------------|
| **VIRT-CAL-01** | Calendar | Open month view; inspect DOM cell count. | About **35–42** day cells (not hundreds). Grid stays bounded when changing months. | ☐ Pass / ☐ Fail / ☐ N/A | |
| **VIRT-TP-01** | TimePicker | Set 1-minute interval over 24h; open popup; count `<li>` (or profile DOM). | Document actual count. Pass if product defines a **cap/window** and DOM matches it; fail if ~1440 nodes with no documented bound. | ☐ Pass / ☐ Fail / ☐ N/A | |
| **VIRT-TP-02** | TimePicker | Locate list-build / `PageSize` (or interval) logic in code-behind. | Found and documented path that limits `ListData` **before** render, **or** accepted risk recorded. | ☐ Pass / ☐ Fail / ☐ N/A | |
| **VIRT-UP-01** | Uploader | Add files up to max-count rule; inspect list. | List length respects max-file rules; no runaway growth. | ☐ Pass / ☐ Fail / ☐ N/A | |
| **VIRT-CH-01** | Chart | Large series (e.g. 10k points), markers on vs adaptive/small size. | Not a `<Virtualize>` list; markers/layers scale via chart pipeline without a huge DOM node list per point. | ☐ Pass / ☐ Fail / ☐ N/A | |

---