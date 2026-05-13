# Radio Button Component Test Plan

## Application Overview

The SfRadioButton is a Blazor component that enables users to select a single option from a set of mutually exclusive choices. It supports various features including label placement, size customization, visual states, color themes, RTL layout, value binding to multiple data types (bool, string, int, nullable bool), and event handling.

**Features Tested:**
- ✅ Basic rendering and HTML structure
- ✅ Label and label positioning (Before/After)
- ✅ Size variations (e-small, default, e-bigger)
- ✅ Selection states and mutual exclusivity
- ✅ Disabled state interactions
- ✅ Value binding and checked state
- ✅ Event handling (ValueChange, CheckedChanged, Created)
- ✅ Focus management and keyboard navigation
- ✅ Dynamic changes and re-rendering
- ✅ Edge cases and empty values
- ✅ Accessibility features
- ✅ Form integration
- ✅ RTL support
- ✅ CSS customization and color variants
- ⚠️ State persistence (basic state maintenance - localStorage persistence pending helper implementation)

## Test Execution

**Total Test Files:** 18
**Total Test Cases:** ~60+
**Status:** Ready for execution


## Test Scenarios

### 1. Basic Rendering and UI

**Seed:** `seed.spec.ts`

#### 1.1. Verify radio button basic structure rendering

**File:** `tests/radio-button/basic-rendering.spec.ts`

**Steps:**
  1. Navigate to the radio button samples page at /buttons/radio-button
    - expect: The page loads successfully
    - expect: Radio button components are visible on the page
  2. Inspect the first radio button element structure
    - expect: Radio button has a wrapper div with class 'e-radio-wrapper e-wrapper'
    - expect: Contains an input element with type='radio' and class 'e-control e-radio e-lib'
    - expect: Contains a label element with class 'e-label'
  3. Verify label text is displayed correctly
    - expect: Label text is rendered and visible
    - expect: Label is associated with input via 'for' attribute
  4. Check that radio buttons are unchecked by default
    - expect: Radio buttons without Checked attribute are not selected
    - expect: Checked visual state is not applied

#### 1.2. Verify disabled radio button rendering

**File:** `tests/radio-button/basic-rendering.spec.ts`

**Steps:**
  1. Locate a disabled radio button on the page
    - expect: Disabled radio button is visible
    - expect: Disabled attribute is set on the input element
  2. Check disabled button styling
    - expect: Disabled button has visual indication of disabled state
    - expect: Button cannot be interacted with

#### 1.3. Verify radio button with label is accessible

**File:** `tests/radio-button/basic-rendering.spec.ts`

**Steps:**
  1. Click on a radio button's label text
    - expect: Clicking label selects the associated radio button
    - expect: Label-to-input association works correctly

### 2. Label Placement and Positioning

**Seed:** `seed.spec.ts`

#### 2.1. Test label position 'After' (default/right positioning)

**File:** `tests/radio-button/label-placement.spec.ts`

**Steps:**
  1. Find radio button with LabelPosition='After'
    - expect: Label appears to the right of the radio button input
    - expect: Visual layout shows label-after-input arrangement
  2. Verify label text is properly spaced from radio button
    - expect: Label is not overlapping with radio button
    - expect: Proper spacing between input and label

#### 2.2. Test label position 'Before' (left positioning)

**File:** `tests/radio-button/label-placement.spec.ts`

**Steps:**
  1. Find radio button with LabelPosition='Before'
    - expect: Label appears to the left of the radio button input
    - expect: Visual layout shows label-before-input arrangement
  2. Verify proper spacing and styling
    - expect: No visual overlap
    - expect: Label is clearly associated with input

#### 2.3. Test label text with special characters

**File:** `tests/radio-button/label-placement.spec.ts`

**Steps:**
  1. Set label text with special characters like &, <, >, quotes
    - expect: Special characters are properly HTML-encoded
    - expect: Label displays safely without XSS issues

#### 2.4. Test empty or null label

**File:** `tests/radio-button/label-placement.spec.ts`

**Steps:**
  1. Create a radio button without Label property
    - expect: Radio button renders without error
    - expect: No label element is shown if label is null

### 3. Size Variations

**Seed:** `seed.spec.ts`

#### 3.1. Test small size radio button

**File:** `tests/radio-button/size-variations.spec.ts`

**Steps:**
  1. Find radio button with CssClass='e-small'
    - expect: Radio button is smaller than default
    - expect: e-small class is applied to wrapper
    - expect: All elements scale appropriately
  2. Verify interactive functionality of small radio button
    - expect: Small radio button can be clicked
    - expect: Selection works correctly
    - expect: No functionality loss due to size

#### 3.2. Test medium/default size radio button

**File:** `tests/radio-button/size-variations.spec.ts`

**Steps:**
  1. Find radio button without size class
    - expect: Radio button displays at default/medium size
    - expect: Standard proportions are maintained
  2. Compare with small and large versions
    - expect: Medium size is visually between small and large
    - expect: Proper scaling progression

#### 3.3. Test large size radio button

**File:** `tests/radio-button/size-variations.spec.ts`

**Steps:**
  1. Find radio button with CssClass='e-bigger'
    - expect: Radio button is larger than default
    - expect: e-bigger class is applied to wrapper
    - expect: All elements scale proportionally
  2. Verify large radio button functionality
    - expect: Large radio button can be clicked and selected
    - expect: Full functionality preserved

### 4. Selection States

**Seed:** `seed.spec.ts`

#### 4.1. Test checking an unchecked radio button

**File:** `tests/radio-button/selection-states.spec.ts`

**Steps:**
  1. Click on an unchecked radio button
    - expect: Radio button becomes checked
    - expect: Visual check mark/filled state is displayed
    - expect: Checked attribute reflects true state
  2. Verify the onclick event is triggered
    - expect: Click event handler is called
    - expect: State change is registered

#### 4.2. Test unchecking a checked radio button in different groups

**File:** `tests/radio-button/selection-states.spec.ts`

**Steps:**
  1. Start with one radio button in a group already checked
    - expect: Initial radio button shows checked state
  2. Click on another radio button in the same group (same Name attribute)
    - expect: Previously checked button becomes unchecked
    - expect: Newly clicked button becomes checked
    - expect: Only one button in group is checked

#### 4.3. Test disabled radio button cannot be selected

**File:** `tests/radio-button/selection-states.spec.ts`

**Steps:**
  1. Click on a disabled radio button
    - expect: Disabled button does not get selected
    - expect: No state change occurs
    - expect: No events are fired
  2. Verify cursor/pointer state over disabled button
    - expect: Cursor shows 'not-allowed' or similar disabled indicator

#### 4.4. Test radio button remains checked across interactions

**File:** `tests/radio-button/selection-states.spec.ts`

**Steps:**
  1. Check a radio button
    - expect: Radio button is checked
  2. Perform other page interactions like scrolling or clicking other elements
    - expect: Radio button remains checked
    - expect: State is persistent

### 5. Mutual Exclusivity (Grouping)

**Seed:** `seed.spec.ts`

#### 5.1. Test radio buttons with same Name are mutually exclusive

**File:** `tests/radio-button/mutual-exclusivity.spec.ts`

**Steps:**
  1. Create a group of radio buttons with Name='payment'
    - expect: All buttons have the same Name attribute
    - expect: Buttons are visually grouped
  2. Check the first radio button
    - expect: First button becomes checked
  3. Check the second radio button in the same group
    - expect: Second button becomes checked
    - expect: First button automatically becomes unchecked
  4. Check the third button
    - expect: Third button becomes checked
    - expect: First and second buttons are unchecked

#### 5.2. Test independent groups work correctly

**File:** `tests/radio-button/mutual-exclusivity.spec.ts`

**Steps:**
  1. Create two separate groups with different Name attributes (e.g., 'payment' and 'delivery')
    - expect: Both groups are independent
  2. Check one button from each group
    - expect: Both buttons show as checked
    - expect: Groups do not interfere with each other
  3. Change selection in first group
    - expect: Second group selection remains unchanged
    - expect: Groups maintain independence

#### 5.3. Test payment method selection example

**File:** `tests/radio-button/mutual-exclusivity.spec.ts`

**Steps:**
  1. Find the payment method group with options: Credit/Debit Card, Net Banking, Cash on Delivery, Other Wallets
    - expect: All four options are visible
    - expect: All have Name='pay'
  2. Select each option one by one and verify mutual exclusivity
    - expect: Only one option is checked at any time
    - expect: Selected value is displayed correctly
    - expect: Previous selection is cleared

### 6. Value Binding

**Seed:** `seed.spec.ts`

#### 6.1. Test string value binding

**File:** `tests/radio-button/value-binding.spec.ts`

**Steps:**
  1. Find radio buttons with string values like 'Card', 'Net', 'COD'
    - expect: Each button has a distinct string Value
    - expect: Buttons use @bind-Checked for two-way binding
  2. Select each option and verify the displayed value matches
    - expect: Selected value ('Card', 'Net', 'COD', or 'Wallets') is displayed in the UI
    - expect: Binding updates in real-time

#### 6.2. Test boolean value binding

**File:** `tests/radio-button/value-binding.spec.ts`

**Steps:**
  1. Find radio buttons with boolean values (true/false)
    - expect: Radio buttons can bind to bool type
    - expect: Values are properly parsed as boolean
  2. Select true and false options
    - expect: Selecting 'true' button shows true in bound property
    - expect: Selecting 'false' button shows false in bound property

#### 6.3. Test nullable boolean binding

**File:** `tests/radio-button/value-binding.spec.ts`

**Steps:**
  1. Find radio buttons bound to nullable bool (bool?)
    - expect: Three options represent null, true, false
  2. Select each option
    - expect: Null option sets bound value to null
    - expect: True option sets bound value to true
    - expect: False option sets bound value to false

#### 6.4. Test integer value binding

**File:** `tests/radio-button/value-binding.spec.ts`

**Steps:**
  1. Find radio buttons with integer values
    - expect: Radio buttons bind to int type
    - expect: Values like 0, 1, 2 are properly parsed
  2. Select different integer options
    - expect: Selected integer value is correctly displayed
    - expect: Type conversion works without errors

#### 6.5. Test checked state synchronization

**File:** `tests/radio-button/value-binding.spec.ts`

**Steps:**
  1. Click a radio button to change the bound value
    - expect: The Checked property updates
    - expect: The displayed value changes immediately
  2. Click another button in the same group
    - expect: The bound value updates to the new selection
    - expect: Previous button unchecks automatically

### 7. Color Customization

**Seed:** `seed.spec.ts`

#### 7.1. Test success color variant

**File:** `tests/radio-button/color-customization.spec.ts`

**Steps:**
  1. Find radio button with CssClass='e-success'
    - expect: Radio button has green/success color styling
    - expect: Color is applied when button is checked
  2. Check the radio button
    - expect: Checked state shows success color
    - expect: Color is distinct from default and other variants

#### 7.2. Test info color variant

**File:** `tests/radio-button/color-customization.spec.ts`

**Steps:**
  1. Find radio button with CssClass='e-info'
    - expect: Radio button has info/blue color styling
  2. Check the radio button
    - expect: Checked state displays info color
    - expect: Color is clearly visible and distinct

#### 7.3. Test warning color variant

**File:** `tests/radio-button/color-customization.spec.ts`

**Steps:**
  1. Find radio button with CssClass='e-warning'
    - expect: Radio button has warning/yellow color styling
  2. Check the radio button
    - expect: Checked state shows warning color
    - expect: Color provides good contrast

#### 7.4. Test danger color variant

**File:** `tests/radio-button/color-customization.spec.ts`

**Steps:**
  1. Find radio button with CssClass='e-danger'
    - expect: Radio button has danger/red color styling
  2. Check the radio button
    - expect: Checked state displays danger color
    - expect: Color is prominently visible

#### 7.5. Test all color variants in same group

**File:** `tests/radio-button/color-customization.spec.ts`

**Steps:**
  1. Find the color customization group with all four color variants
    - expect: All four colored buttons are visible
    - expect: Each has distinct color
  2. Select each color button in sequence
    - expect: Each button shows its respective color when checked
    - expect: Mutual exclusivity is maintained
    - expect: All colors are visually distinct

### 8. Event Handling

**Seed:** `seed.spec.ts`

#### 8.1. Test ValueChange event is fired on user selection

**File:** `tests/radio-button/event-handling.spec.ts`

**Steps:**
  1. Monitor network or component state for ValueChange event
    - expect: ValueChange event is triggered when radio button is clicked
  2. Click a radio button and observe the event data
    - expect: Event contains the new selected value
    - expect: Event data includes the original mouse event
    - expect: Event is fired only for user interactions, not programmatic changes

#### 8.2. Test Created event fires after component initialization

**File:** `tests/radio-button/event-handling.spec.ts`

**Steps:**
  1. Navigate to radio button page
    - expect: Created event fires during component initialization
    - expect: component is fully rendered and ready

#### 8.3. Test CheckedChanged event for two-way binding

**File:** `tests/radio-button/event-handling.spec.ts`

**Steps:**
  1. Use @bind-Checked directive on radio button
    - expect: CheckedChanged event is automatically invoked
    - expect: Two-way binding updates property correctly
  2. Update bound property programmatically
    - expect: Radio button visual state updates
    - expect: Component reflects the new checked state

### 9. Disabled State

**Seed:** `seed.spec.ts`

#### 9.1. Test disabled radio button cannot be interacted with

**File:** `tests/radio-button/disabled-state.spec.ts`

**Steps:**
  1. Locate a radio button with Disabled='true'
    - expect: Disabled attribute is set on input element
    - expect: Button appears grayed out or visually disabled
  2. Attempt to click the disabled button
    - expect: Button does not get selected
    - expect: No click events are fired
    - expect: Component state remains unchanged
  3. Attempt to focus on disabled button
    - expect: Button cannot receive focus
    - expect: Tab order may skip disabled button

#### 9.2. Test disabled and enabled buttons in same group

**File:** `tests/radio-button/disabled-state.spec.ts`

**Steps:**
  1. Find the state variant group with options: Checked, Unchecked, Disabled
    - expect: All three buttons are visible
    - expect: Disabled button is visually distinct
  2. Select the enabled buttons
    - expect: Enabled buttons can be selected normally
    - expect: Disabled button cannot be selected
  3. Verify disabled button label is clickable for accessibility
    - expect: Clicking the disabled button's label still does not check it
    - expect: Label association is correct but functionality is disabled

#### 9.3. Test enabled button displays correctly

**File:** `tests/radio-button/disabled-state.spec.ts`

**Steps:**
  1. Compare enabled and disabled button styling
    - expect: Enabled buttons have normal appearance
    - expect: Disabled buttons have visual indication of disabled state

### 10. Right-to-Left (RTL) Support

**Seed:** `seed.spec.ts`

#### 10.1. Test RTL layout with EnableRtl property

**File:** `tests/radio-button/rtl-support.spec.ts`

**Steps:**
  1. Create radio button with EnableRtl='true'
    - expect: RTL class is applied to component
    - expect: Layout reverses for right-to-left languages
  2. Check label position with RTL enabled
    - expect: Label position adapts to RTL layout
    - expect: Visual arrangement reflects RTL direction

#### 10.2. Test RTL with label position 'Before'

**File:** `tests/radio-button/rtl-support.spec.ts`

**Steps:**
  1. Create radio button with LabelPosition='Before' and EnableRtl='true'
    - expect: Label appears on the left in RTL mode
    - expect: Proper e-right and e-rtl classes are applied

#### 10.3. Test RTL label position 'After'

**File:** `tests/radio-button/rtl-support.spec.ts`

**Steps:**
  1. Create radio button with LabelPosition='After' and EnableRtl='true'
    - expect: Label appears on the right in RTL mode
    - expect: RTL class is applied appropriately

#### 10.4. Test RTL functionality preserved

**File:** `tests/radio-button/rtl-support.spec.ts`

**Steps:**
  1. Use radio button with RTL enabled
    - expect: Selection functionality works in RTL
    - expect: Events fire correctly
    - expect: Value binding works as expected

### 11. Form Integration and Submission

**Seed:** `seed.spec.ts`

#### 11.1. Test radio button works within form context

**File:** `tests/radio-button/form-integration.spec.ts`

**Steps:**
  1. Create a form with multiple radio button groups
    - expect: Form renders correctly with radio buttons
    - expect: All radio buttons are accessible
  2. Select options from different groups
    - expect: Multiple selections can be made in different groups
    - expect: Each group maintains mutual exclusivity

#### 11.2. Test Name attribute for form submission

**File:** `tests/radio-button/form-integration.spec.ts`

**Steps:**
  1. Verify each radio button has correct Name attribute
    - expect: Name attribute matches the group identifier
    - expect: Name is set correctly for form submission
  2. Submit form with selected radio buttons
    - expect: Form data includes selected radio button values
    - expect: Name-value pairs are correctly submitted

#### 11.3. Test Value attribute for form data

**File:** `tests/radio-button/form-integration.spec.ts`

**Steps:**
  1. Check each radio button has distinct Value
    - expect: Value attribute uniquely identifies each option
    - expect: Value is used in form submission

### 12. State Persistence

**Seed:** `seed.spec.ts`

**Note:** Tests for localStorage persistence (EnablePersistence feature) have been removed as they required undefined helper functions. These tests can be re-enabled once the localStorage persistence implementation and test helper functions are complete.

#### 12.1. Test radio button state is maintained across interactions (VALID)

**File:** `tests/radio-button/state-persistence.spec.ts`

**Steps:**
  1. Select a radio button
    - expect: Radio button becomes checked
  2. Perform other page interactions
    - expect: Radio button state is maintained
    - expect: Selection remains unchanged across interactions

#### 12.2. Test multiple radio group states independently (VALID)

**File:** `tests/radio-button/state-persistence.spec.ts`

**Steps:**
  1. Create multiple independent radio button groups
    - expect: Each group can maintain its own state
  2. Select different options in each group
    - expect: Each group's selected state is independent
    - expect: Selections in one group don't affect others

#### 12.3. Test persistence is disabled by default (VALID)

**File:** `tests/radio-button/state-persistence.spec.ts`

**Steps:**
  1. Create radio button without EnablePersistence property (defaults to false)
    - expect: State is not saved to localStorage
    - expect: No persistence data is created
  2. Select radio button and reload page
    - expect: Selected state is not restored
    - expect: Radio button returns to initial state

**Removed Tests** (require future implementation):
- ~~Test EnablePersistence saves state to localStorage~~ ❌
- ~~Test persistence with multiple radio buttons~~ ❌

**Future Enhancement**: When localStorage persistence helper functions are implemented, add tests for:
1. EnablePersistence='true' state saving and restoration
2. Multiple independent persistent groups
3. Persistence across browser sessions

### 13. CSS Customization

**Seed:** `seed.spec.ts`

#### 13.1. Test CssClass property applies custom classes

**File:** `tests/radio-button/css-customization.spec.ts`

**Steps:**
  1. Create radio button with CssClass='custom-class'
    - expect: Custom class is applied to wrapper element
    - expect: Class is added alongside default classes
  2. Verify custom styling is applied
    - expect: Custom CSS rules are applied correctly
    - expect: No style conflicts with default classes

#### 13.2. Test multiple CSS classes in CssClass property

**File:** `tests/radio-button/css-customization.spec.ts`

**Steps:**
  1. Create radio button with CssClass='class1 class2 class3'
    - expect: All classes are applied to wrapper
    - expect: Classes are space-separated
    - expect: All classes coexist without conflicts

#### 13.3. Test size classes (e-small, e-bigger) work correctly

**File:** `tests/radio-button/css-customization.spec.ts`

**Steps:**
  1. Create radio buttons with e-small, default, and e-bigger classes
    - expect: Each size class produces appropriate visual size
    - expect: Classes are properly applied

#### 13.4. Test semantic color classes (e-success, e-info, e-warning, e-danger)

**File:** `tests/radio-button/css-customization.spec.ts`

**Steps:**
  1. Apply each semantic color class to radio buttons
    - expect: Each class produces its semantic color
    - expect: Checked state shows correct color variant

### 14. Accessibility Features

**Seed:** `seed.spec.ts`

#### 14.1. Test label-input association for accessibility

**File:** `tests/radio-button/accessibility.spec.ts`

**Steps:**
  1. Check label 'for' attribute matches input 'id'
    - expect: Label is associated with input via for/id attributes
    - expect: Screen readers can announce label with input
  2. Click on label text
    - expect: Clicking label selects the radio button
    - expect: Accessibility requirement is met

#### 14.2. Test tab navigation through radio button group

**File:** `tests/radio-button/accessibility.spec.ts`

**Steps:**
  1. Press Tab key to navigate to first radio button in group
    - expect: First button receives focus
    - expect: Focus is visible
  2. Press Right/Down arrow keys to navigate within group
    - expect: Focus moves between radio buttons in group
    - expect: Arrow keys allow selection of grouped options
  3. Press Tab again to move to next element
    - expect: Focus leaves the radio button group
    - expect: Tab order is logical

#### 14.3. Test ARIA attributes are present

**File:** `tests/radio-button/accessibility.spec.ts`

**Steps:**
  1. Inspect radio button for ARIA attributes
    - expect: Input has type='radio' for semantic meaning
    - expect: Name attribute is set for grouping

#### 14.4. Test disabled button accessibility

**File:** `tests/radio-button/accessibility.spec.ts`

**Steps:**
  1. Check disabled radio button for accessibility attributes
    - expect: Disabled attribute is properly set
    - expect: Screen readers announce button as disabled

### 15. Focus Management

**Seed:** `seed.spec.ts`

#### 15.1. Test FocusAsync method sets focus programmatically

**File:** `tests/radio-button/focus-management.spec.ts`

**Steps:**
  1. Call FocusAsync() on a radio button reference
    - expect: Radio button receives focus
    - expect: Focus is visible (outline or highlight)
  2. Verify focus state is active
    - expect: Button is ready for keyboard input
    - expect: Keyboard navigation works from focused state

#### 15.2. Test focus outline on keyboard navigation

**File:** `tests/radio-button/focus-management.spec.ts`

**Steps:**
  1. Navigate to radio button using Tab key
    - expect: Radio button shows focus outline
    - expect: Focus indicator is visible

#### 15.3. Test disabled button cannot receive focus

**File:** `tests/radio-button/focus-management.spec.ts`

**Steps:**
  1. Attempt to focus on disabled radio button
    - expect: Focus does not move to disabled button
    - expect: Tab order skips disabled button

### 16. Edge Cases and Error Handling

**Seed:** `seed.spec.ts`

#### 16.1. Test radio button with null Value

**File:** `tests/radio-button/edge-cases.spec.ts`

**Steps:**
  1. Create radio button with Value='null'
    - expect: Component handles null value correctly
    - expect: Selection works without error
  2. Select the button and verify state
    - expect: Bound property is set to null
    - expect: Component state is valid

#### 16.2. Test radio button with very long label text

**File:** `tests/radio-button/edge-cases.spec.ts`

**Steps:**
  1. Set very long label text on radio button
    - expect: Label text is displayed
    - expect: Text wraps or truncates appropriately
    - expect: No layout breaking

#### 16.3. Test radio button with empty Name attribute

**File:** `tests/radio-button/edge-cases.spec.ts`

**Steps:**
  1. Create radio button without Name attribute
    - expect: Component renders without error
    - expect: Name defaults to empty string

#### 16.4. Test rapid clicking on radio button

**File:** `tests/radio-button/edge-cases.spec.ts`

**Steps:**
  1. Rapidly click a radio button multiple times
    - expect: Component handles rapid clicks gracefully
    - expect: State remains consistent
    - expect: No duplicate events or errors

#### 16.5. Test radio button value type mismatch

**File:** `tests/radio-button/edge-cases.spec.ts`

**Steps:**
  1. Bind radio button to bool but set Value to non-boolean string
    - expect: Component attempts to parse value
    - expect: Parsing fails gracefully or converts appropriately

#### 16.6. Test radio button without Value attribute

**File:** `tests/radio-button/edge-cases.spec.ts`

**Steps:**
  1. Create radio button without Value property
    - expect: Component renders without error
    - expect: Value defaults to null
    - expect: Selection and binding still work

### 17. Dynamic Changes and Re-rendering

**Seed:** `seed.spec.ts`

#### 17.1. Test changing Label after initial render

**File:** `tests/radio-button/dynamic-changes.spec.ts`

**Steps:**
  1. Update Label property of existing radio button
    - expect: Label text updates on screen
    - expect: Component re-renders correctly

#### 17.2. Test toggling Disabled state at runtime

**File:** `tests/radio-button/dynamic-changes.spec.ts`

**Steps:**
  1. Toggle Disabled property from false to true
    - expect: Button becomes disabled visually
    - expect: Click events are no longer processed
  2. Toggle Disabled back to false
    - expect: Button becomes enabled again
    - expect: Functionality is restored

#### 17.3. Test changing CssClass dynamically

**File:** `tests/radio-button/dynamic-changes.spec.ts`

**Steps:**
  1. Change CssClass from 'e-small' to 'e-bigger'
    - expect: Component size changes
    - expect: Styling updates immediately

#### 17.4. Test changing Checked state from parent component

**File:** `tests/radio-button/dynamic-changes.spec.ts`

**Steps:**
  1. Programmatically change Checked property from parent
    - expect: Radio button visual state updates
    - expect: DOM reflects new checked state

### 18. Data Type Binding Edge Cases

**Seed:** `seed.spec.ts`

#### 18.1. Test integer zero value binding

**File:** `tests/radio-button/data-type-binding.spec.ts`

**Steps:**
  1. Create radio button group with integer values including 0
    - expect: Zero value is properly recognized
    - expect: Selecting zero value works correctly

#### 18.2. Test negative integer values

**File:** `tests/radio-button/data-type-binding.spec.ts`

**Steps:**
  1. Create radio buttons with negative integer values
    - expect: Negative values are parsed correctly
    - expect: Selection and binding work as expected

#### 18.3. Test string value with numeric content

**File:** `tests/radio-button/data-type-binding.spec.ts`

**Steps:**
  1. Create radio buttons with string values like '123', '456'
    - expect: String values are treated as strings, not numbers
    - expect: Type parsing is correct

#### 18.4. Test empty string value

**File:** `tests/radio-button/data-type-binding.spec.ts`

**Steps:**
  1. Create radio button with Value=''
    - expect: Empty string is treated as valid value
    - expect: Selection and binding handle empty string correctly
