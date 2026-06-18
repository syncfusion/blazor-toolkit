---
name: syncfusion-blazor-toolkit-calendars
description: Build and customize calendar-based components in Syncfusion Blazor Toolkit. Covers Calendar, DatePicker, DateTimePicker, and TimePicker. Use when implementing date/time selection features, handling date range selection, formatting dates, managing calendar events, validating dates, or customizing calendar appearance. Trigger for date picker UI implementation, time selection logic, date range functionality, calendar styling, or date validation workflows.
metadata:
  author: "Syncfusion Inc"
  version: "1.0.0"
---

# Syncfusion Blazor Toolkit - Calendar Components

The Syncfusion Blazor Toolkit provides a comprehensive suite of calendar and date/time selection components for building intuitive date input experiences.

## Table of Contents
- [Getting Started](#getting-started)
- [Core Features](#core-features)
- [Component-Specific Guides](#component-specific-guides)
- [Event Handling & Data Binding](#event-handling--data-binding)
- [API Reference](#api-reference)
- [Troubleshooting](#troubleshooting)
- [Quick Start](#quick-start)
- [Common Patterns](#common-patterns)

## Navigation Guide

### Getting Started
📄 **Read:** [references/getting-started.md](references/getting-started.md)
- Component overview, installation, basic setup, component types

### Core Features
📄 **Read:** [references/features.md](references/features.md)
- Date selection, time selection, range selection, date formatting, validation

### Component-Specific Guides
📄 **Read:** [references/calendar.md](references/calendar.md) - Calendar component with selection modes
📄 **Read:** [references/datepicker.md](references/datepicker.md) - Single date selection with popup
📄 **Read:** [references/datetimepicker.md](references/datetimepicker.md) - Combined date and time selection
📄 **Read:** [references/timepicker.md](references/timepicker.md) - Time-only selection

### Event Handling & Data Binding
📄 **Read:** [references/events-binding.md](references/events-binding.md)
- Change events, selection callbacks, value binding

### API Reference
📄 **Read:** [references/api-reference.md](references/api-reference.md)
- Properties, events, methods, enums, parameters

### Troubleshooting
📄 **Read:** [references/troubleshooting.md](references/troubleshooting.md)
- Common issues, date format problems, validation edge cases, browser compatibility

## Quick Start

### Calendar Component
```razor
<SfCalendar TValue="DateTime" Value="@selectedDate"></SfCalendar>

@code {
    private DateTime selectedDate = new DateTime(2026, 5, 13);
}
```

### DatePicker Component
```razor
<SfDatePicker TValue="DateTime?" Placeholder="Select a date"></SfDatePicker>
```

### DateTimePicker Component
```razor
<SfDateTimePicker TValue="DateTime?" Placeholder="Select date and time"></SfDateTimePicker>
```

### TimePicker Component
```razor
<SfTimePicker TValue="DateTime?" Placeholder="Select time"></SfTimePicker>
```

## Common Patterns

### Pattern 1: Two-Way Date Binding
```razor
<SfDatePicker TValue="DateTime?" @bind-Value="selectedDate"></SfDatePicker>

@code {
    private DateTime? selectedDate;
}
```

### Pattern 2: Date Selection with Change Event
```razor
<SfCalendar TValue="DateTime" ValueChange="@OnDateChanged"></SfCalendar>

@code {
    private void OnDateChanged(ChangedEventArgs<DateTime> args)
    {
        Console.WriteLine($"Selected date: {args.Value}");
    }
}
```

### Pattern 4: Disable Specific Dates
Use the `DayCellRendering` event to customize cell appearance and disable dates:

```razor
<SfCalendar TValue="DateTime" DayCellRendering="@OnDayCellRendering"></SfCalendar>

@code {
    private void OnDayCellRendering(RenderDayCellEventArgs args)
    {
        // Disable weekends
        if (args.Date.DayOfWeek == DayOfWeek.Saturday || args.Date.DayOfWeek == DayOfWeek.Sunday)
        {
            args.IsDisabled = true;
        }
        // Disable specific days (e.g., 13th of every month)
        if (args.Date.Day == 13)
        {
            args.IsDisabled = true;
        }
    }
}
```

### Pattern 5: Custom Date Format
```razor
<SfDatePicker TValue="DateTime?" Format="dd/MM/yyyy"></SfDatePicker>
```
