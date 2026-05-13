# SfCalendar Component Test Plan

## IMPORTANT: Supported vs Unsupported Features

### ✅ SUPPORTED Properties
- **Value / Values** — Single and multi-date binding
- **IsMultiSelection** — Multi-select enablement
- **Min / Max** — Date bounds and overlay behavior
- **FirstDayOfWeek** — Culture-aware week start
- **CalendarMode** — Gregorian / Islamic (Hijri)
- **EnableRtl** — RTL support
- **EnablePersistence** — localStorage persistence
- **CssClass / HtmlAttributes / TabIndex** — styling and container attributes

### ✅ SUPPORTED Events
- **ValueChanged / ValuesChanged** — selection events
- **Selected / DeSelected** — per-date selection events
- **Navigated** — view navigation events
- **OnRenderDayCell** — cell render callback for custom content

### ❌ NOT SUPPORTED (in tests)
- Direct private/internal API calls not exposed by sample pages
- Internal component state inspection without a test harness

## Application Overview

The `SfCalendar<TValue>` renders an inline calendar UI that supports single and multi-date selection, view navigation (month/year/decade), localization (including Hijri), persistence and rich accessibility attributes. The Playwright test suite exercises the sample harness pages that host `SfCalendar` with DOM hooks such as `#event-log` and helper buttons (e.g., `#add-dates`, `#remove-dates`).

Testing approach:
- Prefer interacting with the visible DOM and validating accessible attributes (`aria-*`) and CSS state classes (`e-selected`, `e-active`, `e-overlay`, `e-rtl`).
- Use the sample-page harness buttons for programmatic API coverage (AddDatesAsync/RemoveDatesAsync, toggle options).
- Keep tests deterministic by using current-month dates and clearing persistence between tests.

## Test Scenarios (detailed, file-mapped)

The following sections mirror the structure and level of detail used in `button-test-plan.md`. Each scenario includes the target test file and explicit test steps / expected outcomes.

### 1. Basic Rendering (`basic-rendering.spec.ts`)

Steps:
1. Load `/calendar-test` sample and wait for `#calendar-test` to be visible.
  - expect: root element exists and is visible.
2. Verify CSS classes on root: `e-control`, `e-calendar`, `e-lib` are present.
  - expect: `#calendar-test` has those classes.
3. Verify table/grid and day cells render: `td[aria-label*="calendar cell"]` exist and non-zero.
  - expect: at least 20 visible day cells for the month view.
4. Verify ARIA and unique ids: day cells include `aria-selected` and unique `id` values.
  - expect: `aria-selected` attribute present (value `true`/`false`), ids are unique.

### 2. Single-Date Selection (Click) (`selection-click.spec.ts`)

Steps:
1. Click a visible day cell in current month.
  - expect: the clicked cell has `aria-selected="true"` or `e-selected` class.
2. Validate selection event via `#event-log` (if sample exposes ValueChanged) or via UI state.
  - expect: `#event-log` contains `ValueChanged` or UI reflects selection.
3. Validate `Value` binding on sample page (if exposed) updates accordingly.
  - expect: page bound-value element (if present) equals the clicked date.

### 3. Keyboard Navigation & Selection (`keyboard-navigation.spec.ts`)

Steps:
1. Tab to calendar container and assert focus.
  - expect: calendar root is focused and keyboard-ready.
2. Use Arrow keys to move focus and observe `e-focused-date` or focus change.
  - expect: focused cell moves as arrow keys pressed.
3. Use `Enter`/`Space` to select focused cell.
  - expect: cell becomes selected and `ValueChanged` is emitted (or UI state changes).
4. Verify `Home`/`End` navigate to first/last day of month.
  - expect: focus jumps correctly.

### 4. Multi-Selection UI (`multiselection-ui.spec.ts`)

Steps:
1. Enable multi-selection via harness: click `#toggle-multiselect` or use `#add-dates` sample helper.
  - expect: component accepts multiple selected dates.
2. Click two distinct day cells; assert each has `aria-selected="true"`.
  - expect: both cells show selected state; `ValuesChanged` appears with at least 2 values if logged.
3. Verify duplicate prevention: clicking the same day twice does not produce duplicate values array.
  - expect: `ValuesChanged` contains unique items.

### 5. Programmatic Multi-Selection API (`multiselection-api.spec.ts`)

Steps:
1. Use sample `#add-dates` to programmatically add multiple dates.
  - expect: UI shows added dates and `ValuesChanged` is emitted.
2. Use `#remove-dates` to remove dates and validate removal in UI and events.
  - expect: removed dates are no longer selected; `ValuesChanged` reflects removal.
3. Call `AddDatesAsync(null)` and `RemoveDatesAsync([])` via harness where available; assert no crash and no-op behavior.
  - expect: page remains stable and no Exceptions are logged.

### 6. Min/Max Constraint & Overlay (`minmax-overlay.spec.ts`)

Steps:
1. Click `#set-minmax` to set Min > Max.
  - expect: root receives `e-overlay` class indicating invalid configuration.
2. Set valid `Min`/`Max` and verify dates outside range are disabled (`e-disable` or lack `aria-selected`).
  - expect: disabled dates cannot be selected and have disabled styling/attributes.

### 7. Persistence (localStorage) (`persistence.spec.ts`)

Steps:
1. Enable persistence via `#toggle-persistence` then select a date.
  - expect: selection triggers storage into `localStorage` under a stable key.
2. Reload page and assert previously selected date remains selected.
  - expect: selection persisted across reload.
3. Click `#clear-storage` (if provided) and reload; expect no persisted selection.
  - expect: persistence cleared and calendar starts fresh.

### 8. RTL Mode (`rtl.spec.ts`)

Steps:
1. Click `#toggle-rtl` to enable RTL.
  - expect: root contains `e-rtl` class.
2. Verify layout and navigation still functional in RTL (arrow behavior mirrored where applicable).
  - expect: no regressions; selection and navigation work.

### 9. Calendar View Navigation (`view-navigation.spec.ts`)

Steps:
1. Use header controls to drill up/down between Month, Year and Decade views.
  - expect: DOM updates to show year cells (for Year view) and decade cells (for Decade view).
2. Assert `Navigated` event is logged when view changes.
  - expect: `#event-log` contains `Navigated` with the new view.

### 10. OnRenderDayCell Template Callback (`render-daycell.spec.ts`)

Steps:
1. Ensure sample registers `OnRenderDayCell` which mutates cells (adds class or data-attr).
  - expect: rendered cells include the mutation (e.g., `.e-custom-day-cell` or `data-rendered` attribute).
2. Validate Playwright can assert presence of the custom DOM additions.
  - expect: at least one cell contains injected content.

### 11. Accessibility & ARIA (`accessibility.spec.ts`)

Steps:
1. Validate each day cell includes `aria-label` and `aria-selected`.
  - expect: `aria-label` contains a readable description, `aria-selected` present.
2. Validate unique `id` per day cell for programmatic focus.
  - expect: no duplicate ids among visible day cells.
3. Validate keyboard tab order and focus indicators.
  - expect: calendar is keyboard reachable and focus is visible.

### 12. Hijri Calendar / CalendarMode (`hijri.spec.ts`)

Steps:
1. Load the `#calendar-hijri` instance and assert it renders.
  - expect: element visible and header or day cells contain text (month/year names).
2. Validate Hijri parsing round-trip (if sample exposes parser helpers).
  - expect: conversion yields consistent day mapping.

### 13. Edge Cases & Error Handling (`edge-cases.spec.ts`)

Steps:
1. `NavigateAsync(null)` — when harness exposes API, assert it throws `ArgumentNullException` (or fails gracefully).
  - expect: safe failure or logged exception as specified by API.
2. Invalid `NavigateAsync` view — assert `ArgumentOutOfRangeException` or graceful error handling.
3. `AddDatesAsync(null)` and `RemoveDatesAsync([])` — assert no-op and no crash.
4. When `IsMultiSelection=false`, calling multi-date APIs should not add extra values.
  - expect: UI remains single-selection and API calls are no-op.

## Test Design & Conventions

- Tests are independent: clear `localStorage` and reload the sample page when persistence is involved.
- Prefer public DOM interactions; use `page.evaluate` only for harness helpers (set testLocale, inspect `localStorage`).
- Use robust selectors scoped under `#calendar-test` to avoid collisions.
- Use helper functions (in `helpers/calendar-utils.ts`) for common operations: `selectDate`, `getDayCellLocator`, `toggleMultiSelect`, `navigateYearView`, `assertDisabledDate`.
- If a harness control is missing for a scenario, the test should skip gracefully with a note in the test output.

## Test Files Mapping (recommended)
- `basic-rendering.spec.ts`
- `selection-click.spec.ts`
- `keyboard-navigation.spec.ts`
- `multiselection-ui.spec.ts`
- `multiselection-api.spec.ts`
- `minmax-overlay.spec.ts`
- `persistence.spec.ts`
- `rtl.spec.ts`
- `view-navigation.spec.ts`
- `render-daycell.spec.ts`
- `accessibility.spec.ts`
- `hijri.spec.ts`
- `edge-cases.spec.ts`

## Running the Tests

Start the sample app (example):
```bash
cd samples/Blazor.Toolkit.Samples
dotnet watch run
```

Run Playwright tests (example):
```bash
npx playwright test tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Calendars/Calendar/
```

Run a specific spec:
```bash
npx playwright test tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Calendars/Calendar/selection-click.spec.ts
```

## Test Environment

- Base URL: http://localhost:5000 (or configure `baseURL` in `playwright.config.ts`)
- Sample App: Blazor.Toolkit.Playwright.Samples hosting the `SfCalendar` at `/calendar-test` and `/calendar-hijri`
- Framework: Playwright Test (TypeScript)
- Browser: Chromium (CI headless), run cross-browser if needed

## Notes & Recommendations

- Provide a deterministic test harness page that exposes the calendar instance with stable IDs and helper buttons: `#toggle-weeknumber`, `#toggle-multiselect`, `#toggle-persistence`, `#toggle-rtl`, `#set-minmax`, `#clear-storage`, `#add-dates`, `#remove-dates`, `#event-log`.
- Ensure `#event-log` records events like `Created`, `ValueChanged`, `ValuesChanged`, and `Navigated` to simplify assertions.
- Use current-month dates for selection tests to avoid CI date drift; calculate dates programmatically in tests.
- When asserting persistence, read `localStorage` keys via `page.evaluate` and document the exact key names in the harness for stability.
- If Hijri tests need deterministic validation, include sample data that maps specific Gregorian dates to known Hijri equivalents.

