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
  version: "1.0.0"   # skill document revision only — not the NuGet package version (package is 2.x)
---

# Syncfusion Blazor Toolkit - Calendar Components

> ⚙️ **Render mode:** `SfCalendar`, `SfDatePicker`, `SfDateTimePicker`, `SfTimePicker` require an **interactive** render mode (Server, WebAssembly, or Auto @ .NET 8+). Interactive event delivery (`ValueChanged`, `DayCellRendering`, …) requires it.

## Core Rules

The four rules below are the highest-impact; the rest (5–8) live in
[references/critical-gotchas.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/critical-gotchas.md) — read it
first if you have any time/format/validation question. These top 4 together
prevent ~95% of calendar bug reports.

1. **Pick the right `TValue`**: `DateOnly?` for date-only (avoids accidental
   timezone drift); `DateTime?` for date-time; never use `TimeOnly?` — it is
   not supported by `SfTimePicker`.
2. **The calendars do no timezone conversion.** The bound `DateTime` is the wall-clock value the user picked,
   and its `DateTimeKind` is whatever you supplied (typically `Unspecified`).
   Keep the UI in local wall-clock time and convert explicitly at the
   persistence boundary — see rule 2 in
   [references/critical-gotchas.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/critical-gotchas.md).
3. **`Format` is culture-invariant when you supply a literal pattern**
   (`"dd/MM/yyyy"`); culture-aware when you supply a token (`"d"`, `"D"`).
   Never mix the two in one form.
4. **`Min` / `Max` are inclusive and compared as wall-clock values.** Provide
   them in the same frame as `Value` (the user's local wall-clock time); never
   mix a UTC `Min` with a local `Value`.

Rules 5–8 — `DayCellRendering` mutation discipline, `aria-label` opt-in,
`@bind-Value` + `ValueChanged` double-fire, and midnight re-render — live in
[references/critical-gotchas.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/critical-gotchas.md).

## Don'ts

> **Cross-reference with critical-gotchas.md:** rules below mirror the 8-rule
> reference at [references/critical-gotchas.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/critical-gotchas.md);
> the rules duplicated here (rows 1, 3, 5, 7, 8) are surfaced inline because
> they're the most common agent mistakes. When in doubt on timezones,
> values, or rendering — read the full gotchas reference.

| Anti-pattern | Symptom | Fix |
|---|---|---|
| `TValue="DateTime"` (non-nullable) inside `EditForm` | "Object reference not set" on first render; type-mismatch exception | Use `TValue="DateTime?"` (or `DateOnly?`) |
| `TValue="TimeOnly?"` on `SfTimePicker` | Empty input; `[Required]` rejects valid user input | `TValue="DateTime? Format="HH:mm"` |
| `Min`/`Max` built from `DateTime.UtcNow` while `Value` is local wall-clock | Off-by-one day selection near midnight | Build `Min`/`Max` and `Value` in the same frame (local wall-clock), and convert to UTC only when persisting |
| `args.IsHighlight = true` in `DayCellRendering` | Throws — property doesn't exist | Use `args.CellClass = "highlight"` and style the class |
| Subscribe `ValueChanged` *and* `@bind-Value` | Both fire; the second wins; sometimes a render-loop | Pick one. Use `@bind-Value:after` for side effects |
| One `SfCalendar` driving two date inputs | Renderer mounts a single DOM, second input has no calendar | Per-input `<SfDatePicker>`, share state via `CascadingValue<T>` |
| `<SfCalendar>` inside a Server-mode page that pre-renders today | Highlight misses today's date because of a half-day clock skew between server UTC and client local | Compute "today" from the user's timezone (e.g. a client-supplied offset/`TimeZoneInfo`), not `DateTime.Now` on the server |
| `Format="d"` mixed with `Format="yyyy-MM-dd"` in one form | Validation messages disagree; tooltip provides two formats | Pick one (invariant vs culture) per form |
| Reading `args.Date` and assuming it's `DateOnly`-typed | It's `DateTime` (midnight, server-side); comparison may be wrong | Compare against `args.Date.Date` against your `Min` same way |
| Setting `Min` only — never `Max` | Calendar allows any future date; user picks 2099 | Pair with `Max` whenever `Min` is set |

## Anti-Pattern Workflows

### Workflow 1 — Agent mixes UTC and local values

**Bad:**
```razor
<SfCalendar TValue="DateTime?" @bind-Value="@today" Min="@new DateTime(2026,1,1)" />
@code { private DateTime? today = DateTime.UtcNow.Date; }
```

**Why it fails:** `today` and `Min` are UTC-based, but the user picks dates
as local wall-clock values and the calendar does no conversion. A user in Tokyo at 09:00 (Jan 1 local =
00:00 UTC) sees "Jan 1 enabled" — correct by accident. A user in New York
at 19:00 (Jan 1 local = 00:00 UTC) sees "Dec 31 disabled" — wrong.

**Correct:** keep `Value`, `Min` and `Max` in one frame (local wall-clock) and
convert only when persisting:

```razor
@using Syncfusion.Blazor.Toolkit

<SfCalendar TValue="DateTime?"
            Min="@(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Unspecified))"
            @bind-Value="@today" />
@code {
    private DateTime? today;        // user's local wall-clock date

    // Convert at the boundary, using the user's timezone (not the server's):
    private DateTime? ToUtc(DateTime? local, TimeZoneInfo userZone) =>
        local is null ? null : TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(local.Value, DateTimeKind.Unspecified), userZone);
}
```

### Workflow 2 — Agent sets `DayCellRendering` to highlight holidays but `args.IsHighlight` doesn't exist

**Bad:**
```razor
private void OnCell(RenderDayCellEventArgs args)
{
    if (IsHoliday(args.Date)) args.IsHighlight = true;  // CS1061
}
```

**Fix:** use `CellClass` (you style with CSS):

```razor
private void OnCell(RenderDayCellEventArgs args)
{
    if (IsHoliday(args.Date))
    {
        args.CellClass = "holiday-cell";
        args.IsDisabled = true;            // pick one — these aren't mutually exclusive
    }
}
```

```css
.holiday-cell { background: var(--brand-warning-tint); color: var(--brand-warning-fg); }
```

### Workflow 3 — Agent binds `@bind-Value` to `ValueChanged`

**Bad:**
```razor
@using Syncfusion.Blazor.Toolkit

<SfDatePicker TValue="DateTime?"
              @bind-Value="@_date"
              ValueChanged="OnDate" />
<!-- fires twice, @bind-Value then ValueChanged -->
```

**Correct:** use `@bind-Value:after` (.NET 8+) for post-commit logic:

```razor
@using Syncfusion.Blazor.Toolkit

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

### Workflow 5 — Agent wants to persist UTC but show local values

**Correct:**
```razor
@using Syncfusion.Blazor.Toolkit

<SfCalendar TValue="DateTime?"
            @bind-Value="@_localDate"
            Format="yyyy-MM-dd" />
@code {
    private DateTime? _localDate;           // what the calendar shows and edits
    private readonly TimeZoneInfo _userZone = TimeZoneInfo.Local; // replace with the user's zone in Server mode

    // Persist: wall-clock -> UTC
    public DateTime? UtcForStorage => _localDate is null ? null
        : TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(_localDate.Value, DateTimeKind.Unspecified), _userZone);

    // Load: UTC -> wall-clock
    public void Load(DateTime utc) =>
        _localDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), _userZone);
}
```

> For the full 8-rule reference, see
> [references/critical-gotchas.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/critical-gotchas.md).

## Minimal Example

```razor
@using Syncfusion.Blazor.Toolkit

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
| First component / installation | [references/getting-started.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/getting-started.md) |
| Core features (selection, formatting, validation) | [references/features.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/features.md) |
| Per-component reference (`SfCalendar`, `DatePicker`, `DateTimePicker`, `TimePicker`) | [references/calendar.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/calendar.md), [references/datepicker.md](references/datepicker.md), [references/datetimepicker.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/datetimepicker.md), [references/timepicker.md](references/timepicker.md) |
| Events and two-way binding | [references/events-binding.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/events-binding.md) |
| API reference (properties, events, methods, enums) | [references/api-reference.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/api-reference.md) |
| Troubleshooting | [references/troubleshooting.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/troubleshooting.md) |

> ⚠️ Read [references/critical-gotchas.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/critical-gotchas.md) **first** — UTC vs local time, `TValue` selection, `Format` mixing, `Min`/`Max` inclusivity, `DayCellRendering` mutation rules, accessibility labelling, `@bind-Value` + `ValueChanged` interaction, `DateTime.Now` snapshot.

---

## Next Steps

1. **Read first:** [references/critical-gotchas.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/critical-gotchas.md)
2. **Setup:** [references/getting-started.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/skills/syncfusion-blazor-toolkit-calendars/references/getting-started.md)
3. **Per-component reference:** the relevant `references/*.md`

**Demo:** https://blazor.syncfusion.com/demos/toolkit/calendar