# `@key` usage — evidence report  

**Microsoft guidance:** [Use `@key` to preserve elements and components](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/rendering#use-key-to-control-the-preservation-of-elements-and-components)

---

## Summary

Every toolkit-owned Razor collection that emits repeated elements uses `@key`. Single-instance controls correctly omit it. Chart points are not Razor-looped (SVG builder model).

| Component | Collection | `@key` | Expression | Notes |
|-----------|------------|--------|------------|-------|
| Calendar grid | Rows + day cells | Yes | `dayCells` / `localCalDate.Ticks` | Strong (date identity) |
| DatePicker / DateTimePicker | Via calendar renderer | Yes | Inherited | Same as calendar |
| TimePicker | Time list `<li>` | Yes | `@item` | Stable only if list instances reused |
| Dialog | Footer buttons | Yes | `@i` | OK if order fixed |
| Uploader | File list `<li>` | Yes | `Name + ":" + listIndex` | Watch duplicate names |
| Chart | N/A (no Razor loop) | N/A | — | Builder model |
| Button, ButtonGroup, inputs, Tooltip, Spinner | No internal list | N/A | — | Correct |

---

## Component evidence

### Calendar (`CalendarBaseRender.razor`)
```razor
<tr @key="dayCells" ...>
  <CalendarDayCell @key="localCalDate.Ticks" ... />
```
Stable date-based keys; supports cell reuse on month navigation.

### TimePicker (`SfTimePicker.razor`)
```razor
<li @key="@item" ...>
```
Present. Prefer value key (`ItemData` / time) if `ListData` is rebuilt each open.

### Dialog (`SfDialog.razor`)
```razor
<SfButton @key="@i" ...>
```
Index key — acceptable for fixed footer config.

### Uploader (`SfUploader.razor`)
```razor
@* MS-5.2: stable @key so Blazor reuses <li> elements *@
<li @key="@(filedata.Name + ":" + listIndex)" ...>
```
Present with readiness comment. Risk if two files share the same name.

### Chart / simple controls
No Razor collection → `@key` not applicable.

---

## Conformance

| Claim | Status |
|-------|--------|
| All product list loops use `@key` | **Met** |
| Calendar uses identity keys | **Met** |
| Key quality ideal in every edge case | **Partial** (index / object / name+index caveats) |

---

## Manual verification — `@key` usage

| ID | Component | What to do | What “pass” looks like | Result | Owner / date |
|----|-----------|------------|------------------------|--------|--------------|
| KEY-CAL-01 | Calendar | Open calendar → go to next month → go back to the same month. Watch the same date cell (e.g. the 15th). | That cell is **reused**, not torn down and recreated, when the date is still in view. UI still shows the correct month and selection. | ☐ Pass / ☐ Fail / ☐ N/A | |
| KEY-CAL-02 | Calendar | Switch to **year** and **decade** views (and Islamic mode if enabled). | No duplicate-key errors in the console. Each cell still maps to the correct period. | ☐ Pass / ☐ Fail / ☐ N/A | |
| KEY-TP-01 | TimePicker | Open the time popup, close it, open it again (same interval settings). | List items keep **stable identity** (no full list flicker/recreate if data is unchanged). Selected time still highlights correctly. | ☐ Pass / ☐ Fail / ☐ N/A | |
| KEY-TP-02 | TimePicker | Open popup → select a different time (list stays open if possible). | Only selection styling changes; the whole `<ul>` is **not** rebuilt from scratch. | ☐ Pass / ☐ Fail / ☐ N/A | |
| KEY-DLG-01 | Dialog | Show a dialog with 2+ footer buttons → change only a button’s text/disabled state (order unchanged). | Buttons keep order and focus behavior; no unnecessary remount of all footer buttons. | ☐ Pass / ☐ Fail / ☐ N/A | |
| KEY-UP-01 | Uploader | Add two files with the **same file name** (if the control allows). | Both rows show and update independently; no mixed status/icons between the two rows. | ☐ Pass / ☐ Fail / ☐ N/A | |
| KEY-UP-02 | Uploader | Upload a file and let status change (e.g. Ready → Uploading → Success) without removing the file. | The **same** list row is updated in place; row does not disappear/reappear. | ☐ Pass / ☐ Fail / ☐ N/A | |

---

## Source paths

| File |
|------|
| `src/Components/Calendars/Base/Renderer/CalendarBaseRender.razor` |
| `src/Components/Calendars/TimePicker/SfTimePicker.razor` |
| `src/Components/Popups/Dialog/SfDialog.razor` |
| `src/Components/Inputs/Uploader/SfUploader.razor` |