# Switch Component Test Plan (Real Component Tests)

## Overview

Comprehensive test plan for the Syncfusion Blazor Toolkit real Switch component. These tests navigate to actual Blazor sample pages and test the real SfSwitch component from the Syncfusion.Blazor.Toolkit.Buttons library.

## Test Scenarios

### 1. Basic Rendering (`basic-rendering.spec.ts`)

Tests the fundamental structure and rendering of the Switch component.

- **Verify switch wrapper and input structure**: Validates that the switch wrapper has correct CSS classes (`e-switch-wrapper`, `e-wrapper`), and the input element has required classes (`e-control`, `e-switch`, `e-lib`)
- **Verify switch handle and inner elements**: Validates presence of `.e-switch-inner` and `.e-switch-handle` elements
- **Verify initial unchecked state**: Confirms switch starts in OFF/unchecked state with `aria-checked="false"`
- **Switch has correct tabindex**: Verifies switch is keyboard navigable with proper tabindex attribute
- **Verify aria-label or aria-labelledby exists**: Confirms switch has accessible naming
- **Verify aria-disabled attribute**: Confirms switch has correct disabled state announcement

### 2. Toggle Functionality (`toggle.spec.ts`)

Tests user interactions with the switch.

- **Switch toggles from OFF to ON when clicked**: Verifies clicking the switch wrapper toggles state and updates `aria-checked="true"`
- **Switch toggles from ON to OFF when clicked again**: Verifies toggle behavior works in both directions
- **Multiple consecutive toggles work correctly**: Validates stable toggle behavior through multiple interactions
- **Clicking input directly also toggles**: Confirms clicking the input element directly works
- **Switch handle shows active state when ON**: Validates `.e-switch-active` class appears on handle when checked
- **Switch inner element shows active state when ON**: Validates `.e-switch-active` class appears on inner when checked

### 3. Disabled State (`disabled-state.spec.ts`)

Tests disabled switch behavior.

- **Disabled switch input has disabled attribute**: Confirms proper HTML disabled attribute
- **Disabled switch cannot be toggled**: Validates clicks don't change state when disabled
- **Disabled switch shows disabled styling**: Checks for `.e-switch-disabled` CSS class
- **aria-disabled attribute is set to true**: Validates accessibility attribute
- **Disabled switch is not focusable via keyboard**: Confirms proper keyboard navigation handling

### 4. Value Binding (`value-binding.spec.ts`)

Tests @bind-Checked two-way binding with the Switch component.

- **Switch binds value correctly**: Validates that toggling updates the bound value and display
- **Programmatic value change updates switch**: Confirms UI updates when value changes via code (Set True/Set False buttons)
- **Two-way binding updates on user interaction**: Validates binding works when user toggles
- **Rapid clicks are processed correctly**: Tests that quick successive clicks update value correctly

### 5. Size Variations (`size-variations.spec.ts`)

Tests CSS size customization.

- **Small switch renders with e-small class**: Validates size modifier class application
- **Large switch renders with e-bigger class**: Confirms larger size styling
- **Default switch without size modifier**: Tests standard size
- **Different size switches function independently**: Validates multiple sizes on same page maintain independent state
- **Size modification does not affect toggle functionality**: Confirms CSS classes don't break core behavior

### 6. Label Placement (`label-placement.spec.ts`)

Tests Label, OnLabel, and OffLabel properties.

- **Switch with external label and on/off labels**: Validates label elements render
- **OnLabel is visible when switch is ON**: Confirms state-dependent label visibility (when applicable)
- **OffLabel is visible when switch is OFF**: Confirms opposite state label (when applicable)
- **Clicking on external label toggles switch**: Tests label association and clickability
- **External label is associated with switch input**: Validates `for` attribute matches input `id`
- **Accessibility: Label has correct structure**: Confirms aria-labelledby or aria-label presence

### 7. Event Handling (`event-handling.spec.ts`)

Tests ValueChange event callback behavior.

- **ValueChange event fires when switch is toggled**: Confirms event callback is invoked
- **Event log updates on value changes**: Validates event data is captured and logged
- **Multiple value changes are logged**: Tests that all changes are recorded
- **Clear log button removes all log entries**: Tests event log clearing
- **Event log displays changes in chronological order**: Confirms most recent first
- **Switch value display updates on toggle**: Validates binding updates from events

### 8. RTL Support (`rtl-support.spec.ts`)

Tests right-to-left (RTL) layout and interactions with EnableRtl parameter.

- **Basic RTL switch renders with e-rtl class**: Validates RTL CSS class application
- **RTL switch with labels displays in RTL order**: Confirms labels render correctly in RTL
- **RTL switch responds to clicks normally**: Validates toggle works in RTL mode
- **RTL big switch has correct CSS classes**: Confirms both e-rtl and e-bigger classes present
- **RTL disabled switch cannot be toggled**: Validates disabled state in RTL
- **RTL applied to multiple switches independently**: Tests multiple RTL switches on same page

### 9. State Persistence (`state-persistence.spec.ts`)

Tests EnablePersistence feature and localStorage integration.

- **Persistent switch saves state to localStorage**: Validates storage on toggle
- **Persistent switch restores state on reload**: Confirms localStorage persistence across page reloads
- **Non-persistent switch does not save to localStorage**: Validates non-persistent behavior
- **Non-persistent switch loses state on reload**: Confirms state reset on page reload
- **Multiple persistent switches maintain independent state**: Tests individual persistence
- **Persistent switch shows correct initial display value**: Validates initial rendering
- **Toggling persistent switch multiple times maintains final state**: Tests resilience

### 10. Form Integration (`form-integration.spec.ts`)

Tests form submission, Name/Value attributes, and EditForm binding.

- **Switch inputs have correct name and value attributes**: Validates form field attributes
- **Form submits with switch values**: Tests form submission with switch data
- **Disabled form switch cannot be toggled**: Validates disabled in form context
- **Switch reflects form state changes**: Tests binding with form state
- **All form switches have correct role attribute**: Confirms accessibility in forms
- **Multiple form switches can be toggled independently**: Tests multiple switches in single form
- **Form submission includes only enabled switches**: Validates form data includes all switches
- **Switch maintains two-way binding with form model**: Tests EditForm integration

## Test Coverage Summary

- ✅ **Basic Structure & Rendering**: Full coverage (6 tests)
- ✅ **User Interaction**: Click and toggle behavior (6 tests)
- ✅ **State Management**: Checked/unchecked states (5 tests)
- ✅ **Disabled State**: Non-interactive behavior (5 tests)
- ✅ **Value Binding**: Two-way binding with @bind-Checked (5 tests)
- ✅ **CSS Styling**: Size variations and active states (5 tests)
- ✅ **Labels**: External label and on/off label support (6 tests)
- ✅ **Accessibility**: ARIA attributes and keyboard navigation (7 tests)
- ✅ **RTL Support**: EnableRtl and layout reversal (6 tests)
- ✅ **Persistence**: EnablePersistence and localStorage (7 tests)
- ✅ **Form Integration**: EditForm, Name, Value attributes (8 tests)

## Running the Tests

```bash
npm run test tests/Syncfusion.Blazor.Playwright.Test/switch/
```

Or run specific test file:
```bash
npm run test tests/Syncfusion.Blazor.Playwright.Test/switch/basic-rendering.spec.ts
```

## Test Files Summary

- `basic-rendering.spec.ts` - 6 tests for component structure
- `toggle.spec.ts` - 6 tests for toggle functionality
- `disabled-state.spec.ts` - 5 tests for disabled behavior
- `value-binding.spec.ts` - 5 tests for two-way binding
- `size-variations.spec.ts` - 5 tests for CSS size classes
- `label-placement.spec.ts` - 6 tests for labels
- `event-handling.spec.ts` - 7 tests for ValueChange events
- `rtl-support.spec.ts` - 6 tests for RTL support (NEW)
- `state-persistence.spec.ts` - 7 tests for state persistence (NEW)
- `form-integration.spec.ts` - 8 tests for form integration (NEW)

**Total Test Files: 10 | Total Test Cases: 61+**

## Test Environment

- **Base URL**: http://localhost:5000
- **Sample App**: Blazor.Toolkit.Playwright.Samples
- **Framework**: Playwright Test
- **Browser**: Chromium (default)

## Notes

- All tests require the Blazor sample server to be running
- Tests assume sample pages exist at routes like `/switch/basic-rendering`, `/switch/value-binding`, etc.
- Tests validate the rendered HTML/DOM, not internal component logic
- Real component tests focus on user-visible behavior and accessibility
