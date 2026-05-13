# ButtonGroup Component Test Plan (Real Component Tests)

## Overview

Comprehensive test plan for the Syncfusion Blazor Toolkit real ButtonGroup component. These tests navigate to actual Blazor sample pages and test the real SfButtonGroup and ButtonGroupButton components from the Syncfusion.Blazor.Toolkit.Buttons library.

## Test Scenarios

### 1. Basic Rendering (`basic-rendering.spec.ts`)

Tests the fundamental structure and rendering of the ButtonGroup component.

- **ButtonGroup wrapper renders with correct structure**: Validates that the ButtonGroup wrapper has `role="group"` attribute and CSS classes like `e-btn-group`
- **ButtonGroup contains multiple buttons**: Confirms multiple button elements exist within the group
- **Individual buttons render correctly**: Verifies individual button text content and IDs
- **ButtonGroup has role attribute**: Validates semantic HTML `role="group"` for accessibility
- **Buttons are clickable**: Confirms buttons are enabled and interactive

### 2. Selection Modes (`selection-modes.spec.ts`)

Tests different selection behavior modes of ButtonGroup.

- **Single selection mode: only one button can be selected**: Validates mutual exclusivity in single-selection mode
  - Uses HTML radio buttons (`input[type="radio"]`) internally
  - Clicking one button deselects previously selected button
- **Multiple selection mode: multiple buttons can be selected**: Tests independent selection state
  - Uses HTML checkboxes (`input[type="checkbox"]`) internally  
  - Multiple buttons can be selected simultaneously
- **Single selection shows selected value**: Validates value display updates when selection changes
- **Default mode buttons are clickable**: Tests default mode where buttons don't maintain selection state
- **Selection mode determines selection behavior**: Confirms Mode property controls input type and behavior

### 3. Content Variations (`content-variations.spec.ts`)

Tests different types of button content within ButtonGroup.

- **Text-only buttons render and are clickable**: Simple text content buttons
- **Icon and text buttons render both**: Buttons with IconCss property showing icons alongside text
- **Icon-only buttons are accessible**: Buttons with only icons have aria-label or title for accessibility
- **Custom content buttons display correctly**: ChildContent with custom markup renders
- **Different content types coexist**: Multiple content variation types on same page
- **Buttons are independently interactive**: Each button type responds to clicks
- **Button text is preserved and visible**: Text content displays correctly
- **Icon elements have correct classes**: Icons have `e-icons` class

### 4. Disabled State (`disabled-state.spec.ts`)

Tests disabled button behavior within ButtonGroup.

- **Disabled buttons have disabled attribute**: Validates proper HTML disabled attribute
- **Disabled button cannot be clicked**: Confirms clicks don't trigger interactions
- **Mixed enabled/disabled buttons render correctly**: Multiple buttons with different enable states
- **All disabled buttons group renders non-interactive**: Full group can be disabled
- **Disabled buttons show disabled styling**: Checks for disabled visual indicators
- **Enabled buttons remain clickable among disabled**: Disabled buttons don't affect others
- **Disabled state doesn't affect other buttons**: Independent enable/disable states

### 5. Event Handling (`event-handling.spec.ts`)

Tests button selection and event behavior.

- **Single selection: clicking button updates selection display**: Validates selection state updates
- **Single selection: selecting one button deselects the previous**: Tests mutual exclusivity in single mode
- **Multiple selection: multiple buttons can be selected independently**: Tests independent state in multiple mode
- **Multiple selection: total selected count updates**: Validates count tracking
- **Clear events button empties the event log**: Tests event log management
- **Single selection: selection count stays at 1**: Validates mode behavior
- **Multiple selection: selected permissions update correctly**: Tests multiple selection tracking

## Excluded Tests

The following test scenarios from the fake DOM test plan are **NOT applicable** to the real component and have been excluded:

- **Orientation tests**: Vertical/horizontal layout requires CSS-heavy implementation not fully implemented
- **Nested ButtonGroup tests**: Nested groups are not a core feature in current implementation
- **Form Integration tests**: Requires form submission and server-side validation
- **Touch/Gesture tests**: Not part of current scope - would require touch simulation
- **Event Propagation tests**: Real component uses Blazor events, not JavaScript events
- **Focus Management (advanced)**: Keyboard navigation covered in basic tests
- **Localization tests**: Requires i18n framework setup
- **Theme/CSS Customization (advanced)**: Basic CSS customization covered by size/content tests
- **Performance tests**: Not applicable for component unit tests
- **State Persistence tests**: Browser localStorage testing requires special setup
- **API/Method tests**: ButtonGroup doesn't expose complex public APIs in current implementation

## Test Coverage Summary

- ✅ **Basic Structure & Rendering**: Full coverage with wrapper, buttons, and roles
- ✅ **Selection Modes**: Single and multiple selection validated
- ✅ **Content Types**: Text, icons, mixed, and custom content tested
- ✅ **Disabled State**: Mixed and full disable states covered
- ✅ **Accessibility**: role="group" and interactive button validation
- ✅ **Button Interaction**: Click events and state changes tested
- ❌ **Orientation**: Vertical layout not fully tested
- ❌ **Form Integration**: POST/form submission out of scope
- ❌ **Touch Events**: Gesture input not covered
- ❌ **Event Propagation**: Only checked basic interactions
- ❌ **Localization**: i18n not tested
- ❌ **Themes**: Only basic styling validated

## Running the Tests

```bash
npm run test tests/Syncfusion.Blazor.Playwright.Test/button-group/
```

Or run specific test file:
```bash
npm run test tests/Syncfusion.Blazor.Playwright.Test/button-group/basic-rendering.spec.ts
```

## Test Files Summary

- `basic-rendering.spec.ts` - 5 tests for component structure
- `selection-modes.spec.ts` - 6 tests for selection behavior
- `content-variations.spec.ts` - 8 tests for content types
- `disabled-state.spec.ts` - 7 tests for disabled buttons
- `event-handling.spec.ts` - 7 tests for selection events

**Total Test Files: 5 | Total Test Cases: 33+**

## Test Environment

- **Base URL**: http://localhost:5000
- **Sample App**: Blazor.Toolkit.Playwright.Samples
- **Framework**: Playwright Test
- **Browser**: Chromium (default)

## Test Data

Test data is generated dynamically by the Blazor sample pages:
- Sample pages located at: `Components/Pages/Buttons/ButtonGroup/`
- Routes: `/button-group/basic-rendering`, `/button-group/selection-modes`, `/button-group/content-variations`, `/button-group/disabled-state`

## Notes

- All tests require the Blazor sample server to be running
- Tests assume sample pages exist at the specified routes
- Tests validate the rendered HTML/DOM and user interactions
- Real component tests focus on user-visible behavior and accessibility
- ButtonGroup component uses native HTML input elements (radio/checkbox) for selection modes
- Selection mode (Mode property) determines the underlying input type:
  - `SelectionMode.Default` → buttons without hidden inputs
  - `SelectionMode.Single` → radio button inputs  
  - `SelectionMode.Multiple` → checkbox inputs
