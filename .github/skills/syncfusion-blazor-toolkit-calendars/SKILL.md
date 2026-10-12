---
license: MIT
name: syncfusion-blazor-toolkit-calendars
description: >
  Implement Syncfusion Blazor Toolkit calendar and date/time input
  components — SfCalendar, SfDatePicker, SfDateTimePicker, SfTimePicker.
  USE FOR: date-only, date-time, or time-only selection; date range
  restriction (Min/Max); custom date formats (culture-invariant vs locale);
  multi-date selection; disabled weekends/holidays via DayCellRendering;
  and culture/timezone-sensitive formatting.
  DO NOT USE FOR: text or email input (use
  syncfusion-blazor-toolkit-inputs), scheduling or calendar recurrence
  (use Scheduler — not in this skill), or date validation that requires an
  API round-trip (handle in EditContext validator instead).
compatibility: ".NET 8+, render-modes: Server, WebAssembly, Auto"
metadata:
  author: "Syncfusion Inc"
  version: "1.0.0"
---

# Syncfusion Blazor Toolkit - Calendar Components

> ⚙️ **Render mode:** use Server, WebAssembly, or Auto for calendar interaction and event delivery. The shared base has static-render detection on .NET 9+ (not .NET 8); do not equate static markup with interactive calendar behavior. Read `AGENTS.md` for host conventions.

## Core Rules

Read [references/critical-gotchas.md](references/critical-gotchas.md) for
component-specific types, culture, range checks, and event binding. Source
under `src/Components/Calendars/` is authoritative.

1. **Match `TValue` to the model:** `DateOnly?` for date-only selection,
   `DateTime?` for date/time, and `TimeOnly?` is supported by `SfTimePicker`.
   Nullable values are useful for an empty/required field, not mandatory.
2. **Timezone conversion belongs to the application.** There is no
   `EnableUtc` parameter. Choose date-only, wall-clock, or instant semantics
   and explicitly convert at persistence boundaries as needed.
3. **`Format` uses the component culture.** Standard patterns (`d`, `D`)
   follow culture; custom patterns can still contain culture-sensitive
   separators and names. `SfCalendar` itself has no input `Format` parameter.
4. **`Min` / `Max` are inclusive.** Supply bounds consistent with the value's
   intended calendar date/time; do not assume automatic timezone conversion.

Rules 5–8 — `DayCellRendering` mutation discipline, accessible labelling,
`@bind-Value` / `ValueChanged` duplicate parameters, and midnight re-render —
live in [references/critical-gotchas.md](references/critical-gotchas.md).
Examples are fragments unless they include all referenced state and helpers.

## Don'ts

> **Cross-reference with critical-gotchas.md:** rules below mirror the 8-rule
> reference at [references/critical-gotchas.md](references/critical-gotchas.md);
> the rules duplicated here (rows 1, 3, 5, 7, 8) are surfaced inline because
> they're the most common agent mistakes. When in doubt on timezones,
> values, or rendering — read the full gotchas reference.

| Anti-pattern | Symptom | Fix |
|---|---|---|
| Mismatching `TValue` and bound model type | Binding/conversion errors | Match the model; use nullable types when an empty value is needed |
| Rejecting `TimeOnly?` on `SfTimePicker` | Unnecessary date-bearing model | `TimeOnly` and `TimeOnly?` are supported |
| Treating a calendar date as a UTC instant | Off-by-one date after application conversion | Use `DateOnly` or explicitly define timezone conversion at the boundary |
| `args.IsHighlight` or `args.CellClass` in `DayCellRendering` | Properties do not exist | Append CSS classes to `args.CellData.ClassList` when `CellData` is non-null |
| Subscribe `ValueChanged` *and* `@bind-Value` | Duplicate generated component parameter | Pick one; use `@bind-Value:after` for side effects |
| One `SfCalendar` instance reused for two date inputs | A component instance has one rendered location | Use a picker per input and share application state |
| Assuming server `DateTime.Today` is the user's date | Server and user timezones can differ | Determine the intended date in application code |
| Assuming a custom format is culture-invariant | Separators/month names may vary | Choose and verify the pattern with the configured culture |
| Reading `args.Date` as `DateOnly` | It is `DateTime` | Compare the date portion consistently |
| Omitting a business-required upper bound | Future dates remain selectable | Set `Max` when required by the domain; one-sided ranges are valid |

## Anti-Pattern Workflows

### Workflow 1 — Date-only selection is not an instant

Do not use `DateTime.UtcNow.Date` as an implicit user's local date or add a
nonexistent UTC switch. For date-only business values use `DateOnly?` and
explicit date bounds (`Min` / `Max` remain `DateTime` parameters):

```razor
@using Syncfusion.Blazor.Toolkit.Calendars

<SfCalendar TValue="DateOnly?" @bind-Value="selectedDay"
            Min="@(new DateTime(2026, 1, 1))"
            Max="@(new DateTime(2026, 12, 31))" />

@code {
    private DateOnly? selectedDay;
}
```

Persist the selected date without applying instant/timezone conversion.

### Workflow 2 — Agent sets `DayCellRendering` to highlight holidays but `args.IsHighlight` doesn't exist

**Bad:**
```razor
private void OnCell(RenderDayCellEventArgs args)
{
    if (IsHoliday(args.Date)) args.IsHighlight = true;  // CS1061
}
```

**Fix:** mutate `CellData.ClassList`, preserving existing classes. Handler
fragment for a component with an application-defined `IsHoliday` helper:

```csharp
private void OnCell(RenderDayCellEventArgs args)
{
    if (IsHoliday(args.Date))
    {
        if (args.CellData is not null)
            args.CellData.ClassList += " holiday-cell";
        args.IsDisabled = true;
    }
}
```

```css
.holiday-cell { background: var(--brand-warning-tint); color: var(--brand-warning-fg); }
```

### Workflow 3 — Agent binds `@bind-Value` to `ValueChanged`

**Bad:**
```razor
@using Syncfusion.Blazor.Toolkit.Calendars

<SfDatePicker TValue="DateTime?"
              @bind-Value="@_date"
              ValueChanged="OnDate" />
<!-- invalid: @bind-Value already generates ValueChanged -->
```

**Correct:** use `@bind-Value:after` (.NET 8+) for post-commit logic:

```razor
@using Syncfusion.Blazor.Toolkit.Calendars

<SfDatePicker TValue="DateTime?"
              @bind-Value="@_date"
              @bind-Value:after="OnDateCommitted" />

@code {
    private DateTime? _date;
    private async Task OnDateCommitted() { /* fires once after binding commits */ }
}
```

### Workflow 4 — Agent expects calendar to "remain today" past midnight

**Bad:** leave `SfCalendar` open across midnight expecting the highlight to
move forward — the renderer doesn't re-tick.

**Correct:** for "ticking" UX, wire a `Timer` for the page-level refresh:

```razor
@implements IDisposable
@code {
    private System.Threading.Timer? _t;
    protected override void OnAfterRender(bool first)
    {
        if (first)
        {
            _t = new System.Threading.Timer(_ => InvokeAsync(StateHasChanged),
                                           null,
                                           TimeSpan.Zero,
                                           TimeSpan.FromMinutes(15));
        }
    }
    public void Dispose() => _t?.Dispose();
}
```

For non-essential UX, prefer a plain "Today" pill button that re-binds the
date on click instead of keeping a timer running.

### Workflow 5 — Persisting an instant from a selected date/time

Bind a picker to a wall-clock value, then convert using an explicitly chosen
`TimeZoneInfo` in application code. Validate daylight-saving gaps/ambiguities
before converting to UTC. `DateTime.ToLocalTime()` on a server uses the server's
timezone, not necessarily the user's. There is no component UTC switch.

Picker fragment; persistence is deliberately application-owned:

```razor
@using Syncfusion.Blazor.Toolkit.Calendars

<SfDateTimePicker TValue="DateTime?" @bind-Value="appointmentLocal"
                  Format="yyyy-MM-dd HH:mm" />
@code {
    private DateTime? appointmentLocal;
}
```

> For the full 8-rule reference, see
> [references/critical-gotchas.md](references/critical-gotchas.md).

## Minimal Example

```razor
@using Syncfusion.Blazor.Toolkit.Calendars

<SfDatePicker TValue="DateTime?"
              Placeholder="Select a date"
              @bind-Value="selectedDate" />

@code {
    private DateTime? selectedDate;
}
```

---

## Documentation and Navigation Guide

| Need | Read |
|---|---|
| First component / installation | [references/getting-started.md](references/getting-started.md) |
| Core features (selection, formatting, validation) | [references/features.md](references/features.md) |
| Per-component reference (`SfCalendar`, `DatePicker`, `DateTimePicker`, `TimePicker`) | [references/calendar.md](references/calendar.md), [references/datepicker.md](references/datepicker.md), [references/datetimepicker.md](references/datetimepicker.md), [references/timepicker.md](references/timepicker.md) |
| Events and two-way binding | [references/events-binding.md](references/events-binding.md) |
| API reference (properties, events, methods, enums) | [references/api-reference.md](references/api-reference.md) |
| Troubleshooting | [references/troubleshooting.md](references/troubleshooting.md) |

> ⚠️ Read [references/critical-gotchas.md](references/critical-gotchas.md) **first** — UTC vs local time, `TValue` selection, `Format` mixing, `Min`/`Max` inclusivity, `DayCellRendering` mutation rules, accessibility labelling, `@bind-Value` + `ValueChanged` interaction, `DateTime.Now` snapshot.

---

## Next Steps

1. **Read first:** [references/critical-gotchas.md](references/critical-gotchas.md)
2. **Setup:** [references/getting-started.md](references/getting-started.md)
3. **Per-component reference:** the relevant `references/*.md`

**Demo:** https://blazor.syncfusion.com/demos/toolkit/calendar