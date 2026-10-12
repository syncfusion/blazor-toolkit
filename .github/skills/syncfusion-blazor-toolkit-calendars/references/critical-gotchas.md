---
license: MIT
name: critical-gotchas
description: >
  The 8 critical Syncfusion Blazor calendar gotchas every coding agent must
  internalize. Loaded automatically by syncfusion-blazor-toolkit-calendars
  before any Calendar / DatePicker / DateTimePicker / TimePicker code is
  generated. Covers UTC vs local time, TValue type selection, Format string
  precedence, Min/Max inclusivity, DayCellRendering mutation rules, and
  accessibility labelling.
---

# Calendar Critical Gotchas

**Always read this before generating calendar code.** Syncfusion Blazor Toolkit
components have component-specific value and event contracts. The guidance
below is checked against `src/Components/Calendars/`; fragments still need
the Toolkit Calendar imports and the application's host configuration.

## 1. Match `TValue` to the model

| Need | `TValue` | Why |
|---|---|---|
| Date without time | `DateOnly?` | Models a date, not an instant |
| Date + time | `DateTime?` | Application defines the timezone semantics |
| Time-only | `TimeOnly?` on `SfTimePicker` | Supported by its parsing/conversion source |
| Date-bearing time value | `DateTime?` on `SfTimePicker` | Supported alternative |
| Required initialized value | Non-nullable supported type | Valid; `EditForm` does not change a property's type |

Configuration fragments; declare matching bound fields:

```razor
<SfCalendar TValue="DateOnly?" @bind-Value="selectedDay" />
<SfDateTimePicker TValue="DateTime?" @bind-Value="selectedAt" />
<SfTimePicker TValue="TimeOnly?" @bind-Value="selectedTime" Format="HH:mm" />

@code {
    private DateOnly? selectedDay;
    private DateTime? selectedAt;
    private TimeOnly? selectedTime;
}
```

Nullable values allow empty fields and meaningful `[Required]` validation.

## 2. UTC vs local time

There is **no `EnableUtc` parameter** in these Toolkit components. A date
selection is not automatically a UTC instant. Use `DateOnly` for date-only
business data. For appointments, explicitly combine the chosen date/time
with an application-selected timezone and handle ambiguous/invalid daylight
saving times before persisting an instant. Do not assume the server's local
timezone is the user's timezone.

## 3. `Format` and culture

| Value | Meaning |
|---|---|
| `Format="dd/MM/yyyy"` | Custom pattern; `/` is a culture-sensitive date separator |
| `Format="d"` | Culture's short date pattern |
| `Format="D"` | Culture's long date pattern |
| `Format="MMMM yyyy"` | Custom pattern with a localized month name |

```razor
<SfDatePicker TValue="DateTime?" Format="yyyy-MM-dd" />
<SfDatePicker TValue="DateTime?" Format="d" />
```

Custom and standard patterns may coexist; choose intentionally and validate
with the configured culture. `Format` belongs to the picker inputs, not
`SfCalendar`.

## 4. `Min` and `Max` are inclusive

Provide bounds consistent with the intended date/time value. Do not assume
the component converts them to a browser timezone. For date-only selection:

```razor
<SfDatePicker TValue="DateTime?"
              Min="@(new DateTime(2026, 1, 1))"
              Max="@(new DateTime(2026, 12, 31))" />
```

One-sided ranges are valid; supply both limits when the domain requires them.

## 5. `DayCellRendering` mutation rules

`DayCellRendering` runs per rendered day cell. Use `IsDisabled` to disable
a date and `CellData.ClassList` to add styling. `CellClass`, `CellText`,
`IsHighlight`, and `AddClass` are not properties/methods on the event args.

```razor
<SfCalendar TValue="DateTime?" DayCellRendering="OnCell" />

@code {
    private void OnCell(RenderDayCellEventArgs args)
    {
        if (args.Date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            args.IsDisabled = true;
            if (args.CellData is not null)
                args.CellData.ClassList += " weekend-cell";
        }
    }
}
```

Supply CSS for `weekend-cell`; avoid replacing the component's existing classes.

## 6. Accessibility — `aria-label` is NOT auto-applied

Wrapping a calendar in a `<label>` is not enough — the calendar renders
multiple `<button>` cells. Use the `AriaLabel` property:

```razor
<SfDatePicker TValue="DateTime?"
              AriaLabel="Appointment date"
              @bind-Value="@date" />
<ValidationMessage For="() => date" />
```

If the component is part of a label-required form, **also** add `<label>` to
the parent so screen readers announce the field.

## 7. Don't combine `@bind-Value` and `ValueChanged`

`@bind-Value` generates `Value` and `ValueChanged`. Supplying `ValueChanged`
again is a duplicate parameter, not a supported double-fire pattern. For
post-binding side effects, use `@bind-Value:after` (fragment; declare `d` and
`OnDChanged` in the owning component):

```razor
<SfDatePicker TValue="DateTime?" @bind-Value="d" @bind-Value:after="OnDChanged" />
```

## 8. `DateTime.Now` snapshot — calendars don't re-render on `tick`

If you build a "today" calendar that should highlight the current day
continuously, the component **does not** re-render to advance the highlight at
midnight. You must trigger a render:

```razor
@implements IDisposable
@code {
    private Timer? _t;

    protected override void OnInitialized()
    {
        _t = new Timer(_ => InvokeAsync(StateHasChanged),
                      null,
                      TimeSpan.Zero,
                      TimeSpan.FromHours(1));
    }

    public void Dispose() => _t?.Dispose();
}
```

For most cases this is overkill — a `@bind-Value` on user click already
re-renders. Add the timer only if the calendar is read-only and the page is
expected to stay open across midnight.

---

## When in doubt

- Date-only ⇒ `DateOnly?`; set `Format` only on picker inputs
- Date + time ⇒ `DateTime?` plus an explicit application timezone policy
- Time-only ⇒ `TimeOnly?` on `SfTimePicker`, e.g. `Format="HH:mm"`
- Range restricted ⇒ consistent `Min`/`Max` values and `DayCellRendering`
  for per-cell disabling (weekends, holidays)
- Source ⇒ `src/Components/Calendars/`, not commercial component assumptions