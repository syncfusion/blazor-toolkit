# TimePicker Component Test Plan

## IMPORTANT: Supported Features & Data Types

### ✅ SUPPORTED Properties
- **Value** - Selected time value (DateTime, DateTime?, TimeOnly, TimeOnly?, TimeSpan, TimeSpan?, DateTimeOffset, DateTimeOffset?)
- **Placeholder** - Placeholder text when input is empty
- **Format** - Time format string (e.g., 'HH:mm', 'h:mm tt')
- **InputFormats** - Array of acceptable input formats for parsing typed time values
- **Min** - Minimum selectable time (default: 00:00:00)
- **Max** - Maximum selectable time (default: 23:59:59)
- **CssClass** - Custom CSS classes for styling
- **Enabled** - Enable/disable the component
- **EnableRtl** - Right-to-left layout support
- **ShowClearButton** - Show clear icon to reset value
- **StrictMode** - Enforce strict parsing of typed input
- **AllowEdit** - Allow direct typing vs. popup-only selection
- **OpenOnFocus** - Automatically open popup on input focus
- **FullScreen** - Full screen layout for mobile devices
- **FloatLabelType** - Floating label behavior (Never, Always, Auto)
- **Width** - Visual width of the input
- **Step** - Time interval in minutes (default: 30)
- **EnableMask** - Input masking for time entry
- **Readonly** - Read-only mode preventing user interaction

### ✅ SUPPORTED Events
- **ValueChange** - Fired when the selected time changes
- **Open** - Fired when the popup opens (OnOpen)
- **Close** - Fired when the popup closes (OnClose)
- **Created** - Fired after component first renders
- **Focus** - Fired when input receives focus
- **Blur** - Fired when input loses focus
- **Selected** - Fired when a time is selected from popup
- **Cleared** - Fired when clear button is clicked
- **ItemRender** - Fired for each time item in popup (for customization)
- **Destroyed** - Fired when component is being destroyed

### ✅ SUPPORTED Data Types
- DateTime
- DateTime?
- DateTimeOffset
- DateTimeOffset?
- TimeOnly (.NET 6+)
- TimeOnly? (.NET 6+)
- TimeSpan
- TimeSpan?

### ✅ PUBLIC METHODS
- **FocusAsync()** - Programmatically focus the component
- **FocusOutAsync()** - Remove focus from the component
- **ShowPopupAsync(EventArgs? args = null)** - Open the time popup
- **HidePopupAsync(EventArgs? args = null)** - Close the time popup

## Application Overview

The SfTimePicker component is a Syncfusion Blazor Toolkit time input with a popup time list. It accepts typed input and time selection from a popup, supports min/max constraints, multiple time formats, formatting, RTL, masking, and accessibility features. This test plan covers rendering, input parsing, popup interactions, keyboard navigation, accessibility, RTL, edge cases, data type support, and integration scenarios.

## Test Scenarios

### 1. Content & Display

**Seed:** `seed.spec.ts`

#### 1.1. Render with placeholder text

**File:** `tests/calendar/timepicker/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Input renders with provided placeholder
    - expect: Placeholder disappears when value set
  2. -
    - expect: Input is visible and sized correctly
    - expect: Placeholder styling matches design

#### 1.2. Render with initial Value

**File:** `tests/calendar/timepicker/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Input displays formatted initial time
    - expect: Popup opens showing time list around selected time
  2. -
    - expect: Value persists across re-renders
    - expect: Created event fires on mount

#### 1.3. Empty or null Value

**File:** `tests/calendar/timepicker/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Component renders without a value
    - expect: No errors thrown for null/undefined value
  2. -
    - expect: Clear button is hidden when no value and `ShowClearButton=false`
    - expect: Input remains focusable and accessible

#### 1.4. Dynamic value updates

**File:** `tests/calendar/timepicker/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Value updates when parent state changes
    - expect: Input re-renders to new formatted time
  2. -
    - expect: Popup reflects updated time when opened

#### 1.5. Support multiple TValue types

**File:** `tests/calendar/timepicker/content-display.spec.ts`

**Steps:**
  1. -
    - expect: DateTime type stores and displays correctly
    - expect: TimeOnly type displays time without date portion
  2. -
    - expect: TimeSpan type formats as duration (HH:mm:ss)
    - expect: DateTimeOffset type preserves timezone information

### 2. Input Parsing & Formats

**Seed:** `seed.spec.ts`

#### 2.1. Accept typed input matching `Format`

**File:** `tests/calendar/timepicker/parsing.spec.ts`

**Steps:**
  1. -
    - expect: Typing a time that matches `Format` updates `Value`
    - expect: `ValueChange` fired with correct time value
  2. -
    - expect: Input loses focus retains parsed value

#### 2.2. StrictMode parsing behavior

**File:** `tests/calendar/timepicker/parsing.spec.ts`

**Steps:**
  1. -
    - expect: With `StrictMode=true`, invalid time formats are rejected
    - expect: Input displays validation indication or reverts to previous value
  2. -
    - expect: With `StrictMode=false`, loose parsing attempts to interpret input

#### 2.3. Accept multiple input formats via InputFormats

**File:** `tests/calendar/timepicker/parsing.spec.ts`

**Steps:**
  1. -
    - expect: Component accepts times in multiple specified formats
    - expect: Parsing tries formats in order until one succeeds
  2. -
    - expect: Formatted display follows primary `Format` and locale

#### 2.4. Handle AM/PM time parsing

**File:** `tests/calendar/timepicker/parsing.spec.ts`

**Steps:**
  1. -
    - expect: Typing "2:30 PM" converts to 14:30 in 24-hour cultures
    - expect: Typing "14:30" accepts both 12h and 24h input formats
  2. -
    - expect: Clear button resets value

#### 2.5. Clear button resets value

**File:** `tests/calendar/timepicker/parsing.spec.ts`

**Steps:**
  1. -
    - expect: Clicking clear removes `Value` and fires `ValueChange` with null
    - expect: Input placeholder reappears

### 3. Popup UI & Selection

**Seed:** `seed.spec.ts`

#### 3.1. Open popup via icon click

**File:** `tests/calendar/timepicker/popup-ui.spec.ts`

**Steps:**
  1. -
    - expect: Clicking time icon fires `OnOpen` and shows popup
    - expect: Popup positions correctly below input
  2. -
    - expect: Popup list shows time intervals based on `Step`

#### 3.2. Popup opens on focus with OpenOnFocus

**File:** `tests/calendar/timepicker/popup-ui.spec.ts`

**Steps:**
  1. -
    - expect: With `OpenOnFocus=true`, clicking input opens popup
    - expect: With `OpenOnFocus=false`, popup only opens on icon click
  2. -
    - expect: `OnOpen` event fires regardless

#### 3.3. Select time from popup

**File:** `tests/calendar/timepicker/popup-ui.spec.ts`

**Steps:**
  1. -
    - expect: Clicking a time sets `Value` and closes popup
    - expect: `ValueChange` and `Selected` events fire with selected time
  2. -
    - expect: Selected time has highlight/active class

#### 3.4. Popup respects Min and Max constraints

**File:** `tests/calendar/timepicker/popup-ui.spec.ts`

**Steps:**
  1. -
    - expect: Times outside `[Min, Max]` are disabled/unclickable
    - expect: Disabled times have `aria-disabled` and visual styling
  2. -
    - expect: Typing out-of-range time is rejected in `StrictMode=true`

#### 3.5. Popup list shows Step interval times

**File:** `tests/calendar/timepicker/popup-ui.spec.ts`

**Steps:**
  1. -
    - expect: With `Step=15`, popup shows 00:00, 00:15, 00:30, 00:45, etc.
    - expect: With `Step=60`, popup shows hourly times
  2. -
    - expect: Scrolling reveals more times

#### 3.6. ScrollTo positions popup initial scroll

**File:** `tests/calendar/timepicker/popup-ui.spec.ts`

**Steps:**
  1. -
    - expect: When popup opens, it scrolls to `ScrollTo` time if provided
    - expect: If `ScrollTo` not provided, scrolls to current `Value` or beginning

### 4. Styling & Appearance

**Seed:** `seed.spec.ts`

#### 4.1. Custom `CssClass` applied

**File:** `tests/calendar/timepicker/styling.spec.ts`

**Steps:**
  1. -
    - expect: `CssClass` appears on root element
    - expect: Custom styles are applied

#### 4.2. Width and responsive layout

**File:** `tests/calendar/timepicker/styling.spec.ts`

**Steps:**
  1. -
    - expect: `Width` property sets input width
    - expect: Component adapts to container resizing

#### 4.3. ShowClearButton and icon visibility

**File:** `tests/calendar/timepicker/styling.spec.ts`

**Steps:**
  1. -
    - expect: Clear icon appears when `ShowClearButton=true` and `Value` set
    - expect: Icons have `aria-hidden=true` and are accessible via labels

#### 4.4. FloatLabelType behavior

**File:** `tests/calendar/timepicker/styling.spec.ts`

**Steps:**
  1. -
    - expect: With `FloatLabelType=Never`, label stays inline
    - expect: With `FloatLabelType=Always`, label floats above
  2. -
    - expect: With `FloatLabelType=Auto`, label floats on focus or when value set

#### 4.5. Full screen mode on mobile

**File:** `tests/calendar/timepicker/styling.spec.ts`

**Steps:**
  1. -
    - expect: With `FullScreen=true` on mobile, popup expands to full screen
    - expect: Modal header and close button appear on mobile

### 5. States & Interactions

**Seed:** `seed.spec.ts`

#### 5.1. Focus and blur behavior

**File:** `tests/calendar/timepicker/states.spec.ts`

**Steps:**
  1. -
    - expect: Input receives focus via Tab and `Focus` event fires
    - expect: `Blur` fires on tab away or click outside

#### 5.2. Disabled state

**File:** `tests/calendar/timepicker/states.spec.ts`

**Steps:**
  1. -
    - expect: `Enabled=false` or `Disabled=true` renders disabled input
    - expect: Time icon does not open popup
  2. -
    - expect: `aria-disabled='true'` and `disabled` attribute present

#### 5.3. Read-only state

**File:** `tests/calendar/timepicker/states.spec.ts`

**Steps:**
  1. -
    - expect: `Readonly=true` prevents value editing
    - expect: Popup cannot be opened
  2. -
    - expect: Input retains its value visually

#### 5.4. AllowEdit property behavior

**File:** `tests/calendar/timepicker/states.spec.ts`

**Steps:**
  1. -
    - expect: With `AllowEdit=false`, direct typing is prevented
    - expect: Value can only be selected from popup
  2. -
    - expect: With `AllowEdit=true`, direct typing allowed

#### 5.5. Keyboard navigation in popup

**File:** `tests/calendar/timepicker/states.spec.ts`

**Steps:**
  1. -
    - expect: Arrow Up/Down keys move through time list items
    - expect: Enter selects focused item and closes popup
  2. -
    - expect: Home/End navigate to first/last time
    - expect: Escape closes popup without selecting

### 6. Accessibility & RTL Support

**Seed:** `seed.spec.ts`

#### 6.1. Enable RTL mode

**File:** `tests/calendar/timepicker/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: `EnableRtl=true` applies `e-rtl` class and mirrors layout
    - expect: Icons and text align right-to-left

#### 6.2. ARIA attributes and roles

**File:** `tests/calendar/timepicker/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Input has `role='combobox'` and `aria-haspopup='true'`
    - expect: Popup has `role='dialog'` or `role='listbox'`
  2. -
    - expect: Disabled times expose `aria-disabled='true'`
    - expect: Selected time has `aria-selected='true'`

#### 6.3. Screen reader labeling

**File:** `tests/calendar/timepicker/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Input `aria-label` or visible label announced by SR
    - expect: Clear and time buttons have accessible names
  2. -
    - expect: `aria-owns` links input to popup ID

#### 6.4. Keyboard support

**File:** `tests/calendar/timepicker/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Tab key navigates to component
    - expect: Alt+Up closes popup, Alt+Down opens popup

### 7. Input Masking

**Seed:** `seed.spec.ts`

#### 7.1. Enable input mask

**File:** `tests/calendar/timepicker/masking.spec.ts`

**Steps:**
  1. -
    - expect: With `EnableMask=true`, input shows format placeholder
    - expect: Typing restricts to valid time characters
  2. -
    - expect: Mask placeholder matches `Format` pattern

#### 7.2. Mask prevents invalid characters

**File:** `tests/calendar/timepicker/masking.spec.ts`

**Steps:**
  1. -
    - expect: Typing non-digit characters rejected (except separators)
    - expect: Mask auto-advances through fields (HH:mm:ss)
  2. -
    - expect: Clearing mask input reverts to placeholder

#### 7.3. Mask with dynamic Format

**File:** `tests/calendar/timepicker/masking.spec.ts`

**Steps:**
  1. -
    - expect: Changing `Format` updates mask placeholder
    - expect: Previous value reformatted if possible

### 8. Edge Cases & Special Scenarios

**Seed:** `seed.spec.ts`

#### 8.1. Rapid open/close cycles

**File:** `tests/calendar/timepicker/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Repeatedly opening and closing popup does not leak DOM nodes
    - expect: `OnOpen`/`OnClose` events fire predictably

#### 8.2. Invalid typed input

**File:** `tests/calendar/timepicker/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Typing invalid time does not crash component
    - expect: Component either rejects, clears, or provides validation feedback

#### 8.3. Min/Max equal or inverted

**File:** `tests/calendar/timepicker/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: If `Min==Max`, only that time is selectable
    - expect: If `Min>Max`, component handles gracefully (rejects config)

#### 8.4. Mid-day boundaries

**File:** `tests/calendar/timepicker/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Midnight (00:00) and end-of-day (23:59) handled correctly
    - expect: Times spanning midnight handled appropriately

#### 8.5. TimeSpan edge cases

**File:** `tests/calendar/timepicker/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: TimeSpan displays as duration (HH:mm:ss format)
    - expect: Negative TimeSpans handled (if supported by component logic)
  2. -
    - expect: TimeSpan input format works correctly

### 9. Performance & Rendering

**Seed:** `seed.spec.ts`

#### 9.1. Render cost for many pickers

**File:** `tests/calendar/timepicker/performance.spec.ts`

**Steps:**
  1. -
    - expect: 10+ `SfTimePicker` instances render without significant lag

#### 9.2. Popup creation vs reuse

**File:** `tests/calendar/timepicker/performance.spec.ts`

**Steps:**
  1. -
    - expect: Popup DOM is reused where possible to minimize allocations

#### 9.3. Large Step intervals

**File:** `tests/calendar/timepicker/performance.spec.ts`

**Steps:**
  1. -
    - expect: Popup list renders quickly even with large time ranges
    - expect: Scrolling is smooth

### 10. Integration & Combinations

**Seed:** `seed.spec.ts`

#### 10.1. Form submission with TimePicker

**File:** `tests/calendar/timepicker/integration.spec.ts`

**Steps:**
  1. -
    - expect: TimePicker inside `<form>` captures time value and includes in form submission
    - expect: Form submission result contains the submitted time value
