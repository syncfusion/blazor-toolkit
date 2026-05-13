# DatePicker Component Test Plan

## IMPORTANT: Supported vs Unsupported Features

### ✅ SUPPORTED Properties
- **Value** - Selected date value (DateTime)
- **Placeholder** - Placeholder text when input is empty
- **Format** - Date format string (e.g., 'MM/dd/yyyy')
- **Min** - Minimum selectable date
- **Max** - Maximum selectable date
- **CssClass** - Custom CSS classes for styling
- **Enabled** - Enable/disable the component
- **EnableRtl** - Right-to-left layout support
- **ShowClearButton** - Show clear icon to reset value
- **StrictMode** - Enforce strict parsing of typed input
- **FirstDayOfWeek** - Calendar first day (Sunday/Monday)
- **Width** - Visual width of the input

### ✅ SUPPORTED Events
- **ValueChange** - Fired when the selected date changes
- **Open** - Fired when the calendar popup opens
- **Close** - Fired when the calendar popup closes
- **Created** - Fired after component first renders
- **Focus** - Fired when input receives focus
- **Blur** - Fired when input loses focus

### ❌ NOT SUPPORTED
- **Range selection** - This DatePicker supports single-date selection only (use DateRangePicker for ranges)
- **Time selection** - No time-of-day selection (use DateTimePicker for date+time)
- **Custom calendar views** - Cannot replace month/day views with arbitrary templates

### 🔄 Updated Test Cases
Due to single-date support, tests reference DateRangePicker only for range scenarios. Tests that assumed time selection have been removed or redirected to DateTimePicker tests.

## Application Overview

The SfDatePicker component is a Syncfusion Blazor Toolkit date input with a popup calendar. It accepts typed input and calendar selection, supports min/max constraints, formatting, RTL, and accessibility attributes. This test plan covers rendering, input parsing, calendar interactions, keyboard navigation, accessibility, RTL, edge cases, and integration scenarios.

## Test Scenarios

### 1. Content & Display

**Seed:** `seed.spec.ts`

#### 1.1. Render with placeholder text

**File:** `tests/calendar/datepicker/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Input renders with provided placeholder
    - expect: Placeholder disappears when value set
  2. -
    - expect: Input is visible and sized correctly
    - expect: Placeholder styling matches design

#### 1.2. Render with initial Value

**File:** `tests/calendar/datepicker/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Input displays formatted initial date
    - expect: Calendar opens showing the selected month
  2. -
    - expect: Value persists across re-renders
    - expect: Created event fires on mount

#### 1.3. Empty or null Value

**File:** `tests/calendar/datepicker/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Component renders without a value
    - expect: No errors thrown for null/undefined value
  2. -
    - expect: Clear button is hidden when no value and `ShowClearButton=false`
    - expect: Input remains focusable and accessible

#### 1.4. Dynamic value updates

**File:** `tests/calendar/datepicker/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Value updates when parent state changes
    - expect: Input re-renders to new formatted value
  2. -
    - expect: Calendar reflects updated date when opened

### 2. Input Parsing & Formats

**Seed:** `seed.spec.ts`

#### 2.1. Accept typed input matching `Format`

**File:** `tests/calendar/datepicker/parsing.spec.ts`

**Steps:**
  1. -
    - expect: Typing a date that matches `Format` updates `Value`
    - expect: `ValueChange` fired with correct DateTime
  2. -
    - expect: Input loses focus retains parsed value

#### 2.2. StrictMode parsing behavior

**File:** `tests/calendar/datepicker/parsing.spec.ts`

**Steps:**
  1. -
    - expect: With `StrictMode=true`, invalid formats are rejected
    - expect: Input displays validation indication or reverts
  2. -
    - expect: With `StrictMode=false`, loose parsing attempts to interpret input

#### 2.3. Accept multiple locale formats

**File:** `tests/calendar/datepicker/parsing.spec.ts`

**Steps:**
  1. -
    - expect: Component respects culture-specific separators and order
    - expect: Formatted display follows `Format` and locale

#### 2.4. Clear button resets value

**File:** `tests/calendar/datepicker/parsing.spec.ts`

**Steps:**
  1. -
    - expect: Clicking clear removes `Value` and fires `ValueChange` with null
    - expect: Input placeholder reappears

### 3. Calendar UI & Selection

**Seed:** `seed.spec.ts`

#### 3.1. Open calendar popup via icon click

**File:** `tests/calendar/datepicker/calendar-ui.spec.ts`

**Steps:**
  1. -
    - expect: Clicking calendar icon fires `Open` and shows popup
    - expect: Popup positions correctly below input
  2. -
    - expect: `Created` fired only once on initial render

#### 3.2. Select date from calendar

**File:** `tests/calendar/datepicker/calendar-ui.spec.ts`

**Steps:**
  1. -
    - expect: Clicking a date sets `Value` and closes popup
    - expect: `ValueChange` contains selected date
  2. -
    - expect: Selected day has active/highlight class

#### 3.3. Navigate months and years

**File:** `tests/calendar/datepicker/calendar-ui.spec.ts`

**Steps:**
  1. -
    - expect: Prev/Next month buttons change displayed month
    - expect: Month/Year drop-down (if present) changes view
  2. -
    - expect: Rapid navigation does not break selection state

#### 3.4. Respect `Min` and `Max` constraints

**File:** `tests/calendar/datepicker/calendar-ui.spec.ts`

**Steps:**
  1. -
    - expect: Dates outside `[Min, Max]` are disabled/unclickable
    - expect: Disabled dates have `aria-disabled` and visual styling
  2. -
    - expect: Typing out-of-range date is rejected or corrected

### 4. Styling & Appearance

**Seed:** `seed.spec.ts`

#### 4.1. Custom `CssClass` applied

**File:** `tests/calendar/datepicker/styling.spec.ts`

**Steps:**
  1. -
    - expect: `CssClass` appears on root element
    - expect: Custom styles are applied

#### 4.2. Width and responsive layout

**File:** `tests/calendar/datepicker/styling.spec.ts`

**Steps:**
  1. -
    - expect: `Width` property sets input width
    - expect: Component adapts to container resizing

#### 4.3. ShowClearButton and icon visibility

**File:** `tests/calendar/datepicker/styling.spec.ts`

**Steps:**
  1. -
    - expect: Clear icon appears when `ShowClearButton=true` and `Value` set
    - expect: Icons have `aria-hidden=true` and are accessible via labels

### 5. States & Interactions

**Seed:** `seed.spec.ts`

#### 5.1. Focus and blur behavior

**File:** `tests/calendar/datepicker/states.spec.ts`

**Steps:**
  1. -
    - expect: Input receives focus via Tab and `Focus` event fires
    - expect: Blur fires on tab away or click outside

#### 5.2. Disabled state

**File:** `tests/calendar/datepicker/states.spec.ts`

**Steps:**
  1. -
    - expect: `Enabled=false` renders read-only or disabled input
    - expect: Calendar icon does not open popup
  2. -
    - expect: `aria-disabled='true'` and `disabled` attribute present

#### 5.3. Keyboard navigation in calendar

**File:** `tests/calendar/datepicker/states.spec.ts`

**Steps:**
  1. -
    - expect: Arrow keys move focus between days
    - expect: Enter selects focused day
  2. -
    - expect: Home/End/ PageUp/PageDown navigate calendar as expected

### 6. Accessibility & RTL Support

**Seed:** `seed.spec.ts`

#### 6.1. Enable RTL mode

**File:** `tests/calendar/datepicker/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: `EnableRtl=true` applies `e-rtl` class and mirrors layout
    - expect: Icons and text align right-to-left

#### 6.2. ARIA attributes and roles

**File:** `tests/calendar/datepicker/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Input has role `combobox` or implicit input semantics
    - expect: Popup has proper `role='dialog'` or `role='listbox'` as appropriate
  2. -
    - expect: Disabled dates expose `aria-disabled` and selected date has `aria-selected`

#### 6.3. Screen reader labeling

**File:** `tests/calendar/datepicker/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Input `aria-label` or visible label is announced by SR
    - expect: Clear and calendar buttons have accessible names

### 7. Edge Cases & Special Scenarios

**Seed:** `seed.spec.ts`

#### 7.1. Rapid open/close cycles

**File:** `tests/calendar/datepicker/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Repeatedly opening and closing popup does not leak DOM nodes
    - expect: `Open`/`Close` events fire predictably

#### 7.2. Invalid typed input

**File:** `tests/calendar/datepicker/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Typing invalid date does not crash component
    - expect: Component either rejects, clears, or provides validation feedback

#### 7.3. Min/Max equal or inverted

**File:** `tests/calendar/datepicker/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: If `Min==Max`, only that date is selectable
    - expect: If `Min>Max`, component handles gracefully (rejects config or disables selection)

#### 7.4. Very large date ranges

**File:** `tests/calendar/datepicker/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Calendar navigates efficiently across years without UI freeze

### 8. Performance & Rendering

**Seed:** `seed.spec.ts`

#### 8.1. Render cost for many pickers

**File:** `tests/calendar/datepicker/performance.spec.ts`

**Steps:**
  1. -
    - expect: 10+ `SfDatePicker` instances render without significant lag

#### 8.2. Popup creation vs reuse

**File:** `tests/calendar/datepicker/performance.spec.ts`

**Steps:**
  1. -
    - expect: Popup DOM is reused where possible to minimize allocations

### 9. Integration & Combinations

**Seed:** `seed.spec.ts`

#### 9.1. Form submission with DatePicker

**File:** `tests/calendar/datepicker/integration.spec.ts`

**Steps:**
  1. -
    - expect: DatePicker inside `<form>` participates in submission when value present

#### 9.2. Combine with validation components

**File:** `tests/calendar/datepicker/integration.spec.ts`

**Steps:**
  1. -
    - expect: Validation messages appear when date invalid/out-of-range

#### 9.3. Interaction with DateRangePicker and DateTimePicker

**File:** `tests/calendar/datepicker/integration.spec.ts`

**Steps:**
  1. -
    - expect: Use cases requiring range or time selection are redirected to appropriate components
