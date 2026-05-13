# DateTimePicker Component Test Plan

## IMPORTANT: Supported vs Unsupported Features

### ✅ SUPPORTED Properties
- **Value** - Selected date and time value (DateTime)
- **Placeholder** - Placeholder text when input is empty
- **Format** - Date and time format string (e.g., 'MM/dd/yyyy HH:mm')
- **TimeFormat** - Time format for popup list (e.g., 'HH:mm', 'hh:mm tt')
- **Min** - Minimum selectable date and time
- **Max** - Maximum selectable date and time
- **MinTime** - Minimum selectable time (across all dates)
- **MaxTime** - Maximum selectable time (across all dates)
- **Step** - Time interval in minutes (default: 30)
- **ScrollTo** - Initial scroll position in time popup
- **CssClass** - Custom CSS classes for styling
- **Enabled** - Enable/disable the component
- **EnableRtl** - Right-to-left layout support
- **ShowClearButton** - Show clear icon to reset value
- **StrictMode** - Enforce strict parsing of typed input
- **FirstDayOfWeek** - Calendar first day (Sunday/Monday)
- **Width** - Visual width of the input

### ✅ SUPPORTED Events
- **ValueChange** - Fired when the selected date and time changes
- **Open** - Fired when the calendar or time popup opens
- **Close** - Fired when the calendar or time popup closes
- **Created** - Fired after component first renders
- **Focus** - Fired when input receives focus
- **Blur** - Fired when input loses focus
- **OnItemRender** - Fired for each time item in popup (for customization)

### ✅ SUPPORTED Data Types
- DateTime
- DateTime?

### ✅ PUBLIC METHODS
- **FocusAsync()** - Programmatically focus the component
- **FocusOutAsync()** - Remove focus from the component
- **ShowPopupAsync(EventArgs? args = null)** - Open the date/time popup
- **HidePopupAsync(EventArgs? args = null)** - Close the date/time popup

## Application Overview

The SfDateTimePicker component is a Syncfusion Blazor Toolkit date and time input with a combined calendar and time popup. It accepts typed input and date/time selection from popups, supports min/max constraints for both date and time, multiple time formats, formatting, RTL, and accessibility features. This test plan covers rendering, input parsing, calendar/time interactions, keyboard navigation, accessibility, RTL, edge cases, and integration scenarios.

## Test Scenarios

### 1. Content & Display

**Seed:** `seed.spec.ts`

#### 1.1. Render with placeholder text

**File:** `tests/calendar/datetimepicker/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Input renders with provided placeholder
    - expect: Placeholder disappears when value set
  2. -
    - expect: Input is visible and sized correctly
    - expect: Placeholder styling matches design

#### 1.2. Render with initial Value

**File:** `tests/calendar/datetimepicker/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Input displays formatted initial date and time
    - expect: Calendar opens showing selected date
  2. -
    - expect: Value persists across re-renders
    - expect: Created event fires on mount

#### 1.3. Empty or null Value

**File:** `tests/calendar/datetimepicker/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Component renders without a value
    - expect: No errors thrown for null/undefined value
  2. -
    - expect: Clear button is hidden when no value and `ShowClearButton=false`
    - expect: Input remains focusable and accessible

#### 1.4. Dynamic value updates

**File:** `tests/calendar/datetimepicker/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Value updates when parent state changes
    - expect: Input re-renders to new formatted date and time
  2. -
    - expect: Calendar reflects updated date when opened

### 2. Input Parsing & Formats

**Seed:** `seed.spec.ts`

#### 2.1. Accept typed input matching `Format`

**File:** `tests/calendar/datetimepicker/parsing.spec.ts`

**Steps:**
  1. -
    - expect: Typing a date and time that matches `Format` updates `Value`
    - expect: `ValueChange` fired with correct DateTime
  2. -
    - expect: Input loses focus retains parsed value

#### 2.2. StrictMode parsing behavior

**File:** `tests/calendar/datetimepicker/parsing.spec.ts`

**Steps:**
  1. -
    - expect: With `StrictMode=true`, invalid formats are rejected
    - expect: Input displays validation indication or reverts
  2. -
    - expect: With `StrictMode=false`, loose parsing attempts to interpret input

#### 2.3. Accept multiple locale formats

**File:** `tests/calendar/datetimepicker/parsing.spec.ts`

**Steps:**
  1. -
    - expect: Component respects culture-specific separators and order
    - expect: Formatted display follows `Format` and locale

#### 2.4. Clear button resets value

**File:** `tests/calendar/datetimepicker/parsing.spec.ts`

**Steps:**
  1. -
    - expect: Clicking clear removes `Value` and fires `ValueChange` with null
    - expect: Input placeholder reappears

### 3. Calendar & Time UI

**Seed:** `seed.spec.ts`

#### 3.1. Open calendar popup via date icon click

**File:** `tests/calendar/datetimepicker/calendar-time-ui.spec.ts`

**Steps:**
  1. -
    - expect: Clicking calendar icon fires `Open` and shows calendar popup
    - expect: Popup positions correctly below input
  2. -
    - expect: `Created` fired only once on initial render

#### 3.2. Open time popup via time icon click

**File:** `tests/calendar/datetimepicker/calendar-time-ui.spec.ts`

**Steps:**
  1. -
    - expect: Clicking time icon fires `Open` and shows time list popup
    - expect: Time list shows intervals based on `Step`

#### 3.3. Select date from calendar

**File:** `tests/calendar/datetimepicker/calendar-time-ui.spec.ts`

**Steps:**
  1. -
    - expect: Clicking a date in calendar updates the date portion of value
    - expect: Calendar closes and time popup may open
  2. -
    - expect: Selected day has active/highlight class

#### 3.4. Select time from popup

**File:** `tests/calendar/datetimepicker/calendar-time-ui.spec.ts`

**Steps:**
  1. -
    - expect: Clicking a time sets the time portion and fires `ValueChange`
    - expect: Time popup closes
  2. -
    - expect: Selected time has active/highlight class

#### 3.5. Navigate months and years

**File:** `tests/calendar/datetimepicker/calendar-time-ui.spec.ts`

**Steps:**
  1. -
    - expect: Prev/Next month buttons change displayed month
    - expect: Month/Year drop-down changes view
  2. -
    - expect: Rapid navigation does not break selection state

#### 3.6. Respect `Min`, `Max`, `MinTime`, and `MaxTime` constraints

**File:** `tests/calendar/datetimepicker/calendar-time-ui.spec.ts`

**Steps:**
  1. -
    - expect: Dates outside `[Min, Max]` are disabled/unclickable
    - expect: Times outside `[MinTime, MaxTime]` are disabled/unclickable
  2. -
    - expect: Disabled dates and times have visual styling and aria attributes

#### 3.7. Time popup list shows Step interval times

**File:** `tests/calendar/datetimepicker/calendar-time-ui.spec.ts`

**Steps:**
  1. -
    - expect: With `Step=15`, time popup shows 15-minute intervals
    - expect: With `Step=60`, time popup shows hourly times
  2. -
    - expect: Scrolling reveals more times

#### 3.8. ScrollTo positions time popup initial scroll

**File:** `tests/calendar/datetimepicker/calendar-time-ui.spec.ts`

**Steps:**
  1. -
    - expect: When time popup opens, it scrolls to `ScrollTo` time if provided
    - expect: If `ScrollTo` not provided, scrolls to current value or beginning

### 4. Styling & Appearance

**Seed:** `seed.spec.ts`

#### 4.1. Custom `CssClass` applied

**File:** `tests/calendar/datetimepicker/styling.spec.ts`

**Steps:**
  1. -
    - expect: `CssClass` appears on root element
    - expect: Custom styles are applied

#### 4.2. Width and responsive layout

**File:** `tests/calendar/datetimepicker/styling.spec.ts`

**Steps:**
  1. -
    - expect: `Width` property sets input width
    - expect: Component adapts to container resizing

#### 4.3. ShowClearButton and icon visibility

**File:** `tests/calendar/datetimepicker/styling.spec.ts`

**Steps:**
  1. -
    - expect: Clear icon appears when `ShowClearButton=true` and `Value` set
    - expect: Icons have `aria-hidden=true` and are accessible via labels

#### 4.4. TimeFormat affects time popup display

**File:** `tests/calendar/datetimepicker/styling.spec.ts`

**Steps:**
  1. -
    - expect: With `TimeFormat='HH:mm'`, times show 24-hour format
    - expect: With `TimeFormat='hh:mm tt'`, times show 12-hour with AM/PM
  2. -
    - expect: Format applied consistently to all time list items

### 5. States & Interactions

**Seed:** `seed.spec.ts`

#### 5.1. Focus and blur behavior

**File:** `tests/calendar/datetimepicker/states.spec.ts`

**Steps:**
  1. -
    - expect: Input receives focus via Tab and `Focus` event fires
    - expect: Blur fires on tab away or click outside

#### 5.2. Disabled state

**File:** `tests/calendar/datetimepicker/states.spec.ts`

**Steps:**
  1. -
    - expect: `Enabled=false` renders disabled input
    - expect: Calendar and time icons do not open popups
  2. -
    - expect: `aria-disabled='true'` and `disabled` attribute present

#### 5.3. Keyboard navigation in calendar

**File:** `tests/calendar/datetimepicker/states.spec.ts`

**Steps:**
  1. -
    - expect: Arrow keys move focus between days in calendar
    - expect: Enter selects focused day
  2. -
    - expect: Home/End/PageUp/PageDown navigate calendar

#### 5.4. Keyboard navigation in time popup

**File:** `tests/calendar/datetimepicker/states.spec.ts`

**Steps:**
  1. -
    - expect: Arrow Up/Down navigate through time items
    - expect: Enter selects focused time item

### 6. Accessibility & RTL Support

**Seed:** `seed.spec.ts`

#### 6.1. Enable RTL mode

**File:** `tests/calendar/datetimepicker/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: `EnableRtl=true` applies `e-rtl` class and mirrors layout
    - expect: Icons and text align right-to-left

#### 6.2. ARIA attributes and roles

**File:** `tests/calendar/datetimepicker/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Input has proper ARIA attributes
    - expect: Calendar popup has `role='dialog'`
  2. -
    - expect: Disabled dates and times expose `aria-disabled`
    - expect: Selected date have `aria-selected`

#### 6.3. Screen reader labeling

**File:** `tests/calendar/datetimepicker/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Input `aria-label` or visible label is announced by SR
    - expect: Clear, calendar, and time buttons have accessible names

### 7. Edge Cases & Special Scenarios

**Seed:** `seed.spec.ts`

#### 7.1. Rapid open/close cycles

**File:** `tests/calendar/datetimepicker/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Repeatedly opening and closing popups does not leak DOM nodes
    - expect: `Open`/`Close` events fire predictably

#### 7.2. Invalid typed input

**File:** `tests/calendar/datetimepicker/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Typing invalid date/time does not crash component
    - expect: Component either rejects, clears, or provides validation feedback

#### 7.3. Min/Max and MinTime/MaxTime edge cases

**File:** `tests/calendar/datetimepicker/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: If `Min==Max`, only that date is selectable
    - expect: If `MinTime==MaxTime`, only that time is selectable
  2. -
    - expect: If `MinTime>MaxTime`, component handles gracefully

#### 7.4. Date at MinTime or MaxTime boundary

**File:** `tests/calendar/datetimepicker/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Midnight (00:00) and end-of-day (23:59) handled correctly

### 8. Performance & Rendering

**Seed:** `seed.spec.ts`

#### 8.1. Render cost for many pickers

**File:** `tests/calendar/datetimepicker/performance.spec.ts`

**Steps:**
  1. -
    - expect: 10+ `SfDateTimePicker` instances render without significant lag

#### 8.2. Popup creation vs reuse

**File:** `tests/calendar/datetimepicker/performance.spec.ts`

**Steps:**
  1. -
    - expect: Popup DOM is reused where possible to minimize allocations

### 9. Integration & Combinations

**Seed:** `seed.spec.ts`

#### 9.1. Form submission with DateTimePicker

**File:** `tests/calendar/datetimepicker/integration.spec.ts`

**Steps:**
  1. -
    - expect: DateTimePicker inside `<form>` captures date and time value and includes in form submission
    - expect: Form submission result contains the submitted date and time value
