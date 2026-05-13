# Checkbox Component Test Plan

## Application Overview

The SfCheckBox component is a checkbox input that supports multiple states (checked, unchecked, indeterminate), label positioning, tri-state functionality, custom HTML attributes, and accessibility features. This test plan covers the features that are currently implemented and tested with Playwright.

## Test Scenarios

### 1. Basic Functionality

**Seed:** ``

#### 1.1. Default checkbox rendering

**File:** `tests/checkbox/basic-functionality/default-checkbox.spec.ts`

**Steps:**
  1. Render a checkbox component with no properties set
    - expect: Checkbox renders with default unchecked state
    - expect: No label is displayed
    - expect: Checkbox is interactive and clickable
  2. Inspect the rendered HTML
    - expect: Checkbox element is visible on the page
    - expect: Checkbox has proper HTML structure with input type='checkbox'
    - expect: Aria-disabled attribute is set to 'false'

#### 1.2. Checkbox checked state

**File:** `tests/checkbox/basic-functionality/checked-state.spec.ts`

**Steps:**
  1. Render a checkbox with Checked='true'
    - expect: Checkbox renders in checked state
    - expect: Visual indicator (checkmark) is displayed
  2. Verify ARIA attributes
    - expect: Aria-checked attribute is set to 'true'
  3. Click the checkbox
    - expect: Clicking a checked checkbox unchecks it
  4. Verify data binding
    - expect: Two-way binding updates the bound variable when state changes

#### 1.3. Toggle checked and unchecked states

**File:** `tests/checkbox/basic-functionality/toggle-states.spec.ts`

**Steps:**
  1. Render an unchecked checkbox
    - expect: Initial checkbox state is unchecked
  2. Click the checkbox to toggle it
    - expect: Checkbox toggles to checked state
    - expect: Visual appearance changes with checkmark
  3. Click the checkbox again
    - expect: Checkbox toggles back to unchecked state
  4. Click multiple times rapidly
    - expect: Multiple rapid clicks toggle the state correctly
    - expect: State persists after toggle without external changes

#### 1.4. Disabled checkbox interaction

**File:** `tests/checkbox/basic-functionality/disabled-state.spec.ts`

**Steps:**
  1. Render a disabled checkbox (Disabled='true')
    - expect: Checkbox is visually disabled with reduced opacity
  2. Attempt to click the disabled checkbox
    - expect: Disabled checkbox cannot be clicked or toggled by user
    - expect: Aria-disabled attribute is set to 'true'
  3. Verify visibility and state
    - expect: Disabled checkbox is still visible on the page
  4. Test tab navigation
    - expect: Tab navigation bypasses disabled checkbox
  5. Change disabled checkbox state programmatically
    - expect: Checkbox state can be changed programmatically even when disabled

#### 1.5. Indeterminate state (tri-state basic)

**File:** `tests/checkbox/basic-functionality/indeterminate-state.spec.ts`

**Steps:**
  1. Render a checkbox with Indeterminate='true'
    - expect: Indeterminate checkbox displays with special visual indicator (dash)
  2. Check ARIA attributes
    - expect: Aria-checked attribute is set to 'mixed'
  3. Enable tri-state mode and click
    - expect: With EnableTriState=true, clicking cycles through states correctly
  4. Verify state persistence
    - expect: Indeterminate state persists until user interaction

### 2. Label & Label Positioning

**Seed:** ``

#### 2.1. Label text display

**File:** `tests/checkbox/label/label-text.spec.ts`

**Steps:**
  1. Render a checkbox with Label='Accept Terms'
    - expect: Checkbox displays label text next to the checkbox
  2. Click on the label text
    - expect: Label text is clickable and toggles the checkbox
  3. Verify toggle on label click
    - expect: Clicking on label has same effect as clicking checkbox

#### 2.2. Label position - Before (default)

**File:** `tests/checkbox/label/label-before.spec.ts`

**Steps:**
  1. Render a checkbox with LabelPosition='Before'
    - expect: Label appears to the left of checkbox
  2. Render a checkbox without specifying LabelPosition
    - expect: Default label position is 'Before'
  3. Verify alignment
    - expect: Label and checkbox are properly aligned vertically

#### 2.3. Label position - After

**File:** `tests/checkbox/label/label-after.spec.ts`

**Steps:**
  1. Render a checkbox with LabelPosition='After'
    - expect: Label appears to the right of checkbox
  2. Verify alignment
    - expect: Label and checkbox alignment matches design
  3. Test clicking on label
    - expect: Label is clickable and toggling works correctly

#### 2.4. No label with accessibility

**File:** `tests/checkbox/label/no-label-aria.spec.ts`

**Steps:**
  1. Render a checkbox without Label or ChildContent
    - expect: No visible label is displayed
  2. Check ARIA attributes
    - expect: A default or provided aria-label is set for accessibility

#### 2.5. Long label text handling

**File:** `tests/checkbox/label/long-label.spec.ts`

**Steps:**
  1. Render a checkbox with a long label (100+ characters)
    - expect: Long label text wraps to multiple lines correctly
  2. Verify checkbox alignment
    - expect: Checkbox alignment remains correct with multi-line label

### 3. Tri-State Mode & Advanced States

**Seed:** ``

#### 3.1. Enable tri-state mode

**File:** `tests/checkbox/tristate/enable-tristate.spec.ts`

**Steps:**
  1. Render a checkbox with EnableTriState='true' and TChecked='bool?'
    - expect: Checkbox supports three states: checked, unchecked, indeterminate
  2. Click to cycle through states
    - expect: Checkbox cycles through checked -> indeterminate -> unchecked when clicked
  3. Verify visual representation
    - expect: Indeterminate state is visually distinct from checked/unchecked

#### 3.2. Tri-state nullable boolean binding

**File:** `tests/checkbox/tristate/nullable-binding.spec.ts`

**Steps:**
  1. Render a checkbox with TChecked='bool?' and tri-state enabled
    - expect: null value represents indeterminate state
    - expect: true represents checked state
    - expect: false represents unchecked state
  2. Verify two-way binding updates all values
    - expect: All three state values update correctly via binding

#### 3.3. Parent-child checkbox group

**File:** `tests/checkbox/tristate/parent-child-group.spec.ts`

**Steps:**
  1. Create parent checkbox and 3 child checkboxes
    - expect: Parent checkbox displays properly with EnableTriState='true'
  2. Check all child checkboxes one by one
    - expect: Parent shows checked when all children are checked
  3. Uncheck one child
    - expect: Parent transitions to indeterminate state
  4. Click parent to select all
    - expect: All child checkboxes are checked when parent is clicked

#### 3.4. Tri-state cycle behavior

**File:** `tests/checkbox/tristate/cycle-behavior.spec.ts`

**Steps:**
  1. Render tri-state enabled checkbox
    - expect: Checkbox cycles in fixed order: checked -> indeterminate -> unchecked
  2. Click multiple times from each starting state
    - expect: Cycle order is consistent regardless of starting position
  3. Verify state changes match cycle order
    - expect: Each state flows to the correct next state

### 4. Sizes & Styling

**Seed:** ``

#### 4.1. Default size checkbox

**File:** `tests/checkbox/styling/default-size.spec.ts`

**Steps:**
  1. Render checkbox without size class
    - expect: Checkbox renders at default size
    - expect: Size is appropriate for normal interaction

#### 4.2. Small size with e-small class

**File:** `tests/checkbox/styling/small-size.spec.ts`

**Steps:**
  1. Render checkbox with CssClass='e-small'
    - expect: Checkbox renders smaller than default
  2. Interact with small checkbox
    - expect: Small checkbox is fully clickable and functional

#### 4.3. Large size with e-bigger class

**File:** `tests/checkbox/styling/large-size.spec.ts`

**Steps:**
  1. Render checkbox with CssClass='e-bigger'
    - expect: Checkbox renders larger than default
  2. Test interaction
    - expect: Large checkbox maintains full functionality

#### 4.4. Color theme - Primary (e-primary)

**File:** `tests/checkbox/styling/color-primary.spec.ts`

**Steps:**
  1. Render checkbox with CssClass='e-primary'
    - expect: Checked state uses primary color
  2. Check and hover over checkbox
    - expect: Primary color is applied to checked state and hover

#### 4.5. Color theme - Success (e-success)

**File:** `tests/checkbox/styling/color-success.spec.ts`

**Steps:**
  1. Render checkbox with CssClass='e-success'
    - expect: Checked state uses success color (green)
  2. Verify color on checked state
    - expect: Success color is consistently applied

#### 4.6. Color theme - Info (e-info)

**File:** `tests/checkbox/styling/color-info.spec.ts`

**Steps:**
  1. Render checkbox with CssClass='e-info'
    - expect: Checked state uses info color (blue)

#### 4.7. Color theme - Warning (e-warning)

**File:** `tests/checkbox/styling/color-warning.spec.ts`

**Steps:**
  1. Render checkbox with CssClass='e-warning'
    - expect: Checked state uses warning color (orange)

#### 4.8. Color theme - Danger (e-danger)

**File:** `tests/checkbox/styling/color-danger.spec.ts`

**Steps:**
  1. Render checkbox with CssClass='e-danger'
    - expect: Checked state uses danger color (red)

#### 4.9. Combined size and color styling

**File:** `tests/checkbox/styling/combined-styling.spec.ts`

**Steps:**
  1. Render checkbox with CssClass='e-small e-primary'
    - expect: Both size and color are applied correctly
  2. Render checkbox with CssClass='e-bigger e-danger'
    - expect: Large danger checkbox displays correctly

### 5. Data Binding & Events

**Seed:** ``

#### 5.1. Two-way binding (@bind-Checked)

**File:** `tests/checkbox/binding/two-way-binding.spec.ts`

**Steps:**
  1. Render checkbox with @bind-Checked='myBool'
    - expect: Clicking checkbox updates the bound variable
  2. Change bound variable programmatically
    - expect: Visual state updates to match bound value

#### 5.2. Explicit binding pattern

**File:** `tests/checkbox/binding/explicit-binding.spec.ts`

**Steps:**
  1. Implement explicit Checked + CheckedChanged pattern
    - expect: CheckedChanged event fires when user interacts
  2. Update bound variable in callback
    - expect: Custom logic in CheckedChanged is executed correctly

#### 5.3. ValueChange event on user interaction

**File:** `tests/checkbox/binding/value-change-event.spec.ts`

**Steps:**
  1. Attach ValueChange event handler
    - expect: ValueChange fires when user clicks checkbox
  2. Verify event arguments
    - expect: Event includes the new Checked value
    - expect: Event includes mouse event details
  3. Change state programmatically
    - expect: ValueChange does NOT fire for programmatic changes

#### 5.4. ValueChange with tri-state

**File:** `tests/checkbox/binding/value-change-tristate.spec.ts`

**Steps:**
  1. Attach ValueChange to tri-state checkbox
    - expect: Event fires for all three state transitions
  2. Verify event args for each state
    - expect: true for checked, false for unchecked, null for indeterminate

#### 5.5. IndeterminateChanged event

**File:** `tests/checkbox/binding/indeterminate-change.spec.ts`

**Steps:**
  1. Use @bind-Indeterminate with checkbox
    - expect: IndeterminateChanged fires when state changes
  2. Toggle indeterminate state
    - expect: Two-way binding tracks indeterminate state correctly

#### 5.6. Multiple event handlers

**File:** `tests/checkbox/binding/multiple-handlers.spec.ts`

**Steps:**
  1. Add multiple event handlers to checkbox
    - expect: All event handlers are executed
  2. Verify handler execution order
    - expect: Handlers execute in documented order

### 6. Form Integration & Validation

**Seed:** ``

#### 6.1. Checkbox inside EditForm

**File:** `tests/checkbox/form/edit-form.spec.ts`

**Steps:**
  1. Place checkbox inside Blazor EditForm component
    - expect: Checkbox binds to form model
  2. Submit the form
    - expect: Form includes checkbox value in submission

#### 6.2. Required validation attribute

**File:** `tests/checkbox/form/required-validation.spec.ts`

**Steps:**
  1. Create form with required checkbox validation
    - expect: Validation error appears when checkbox is unchecked
  2. Check the checkbox
    - expect: Validation error clears

#### 6.3. Form submission with checked checkbox

**File:** `tests/checkbox/form/submit-checked.spec.ts`

**Steps:**
  1. Check the required checkbox
    - expect: Form validation passes
  2. Click submit button
    - expect: OnValidSubmit is called
    - expect: Form submits successfully

#### 6.4. Form submission blocked when unchecked

**File:** `tests/checkbox/form/submit-unchecked.spec.ts`

**Steps:**
  1. Leave required checkbox unchecked
    - expect: Form validation fails
  2. Attempt form submission
    - expect: OnValidSubmit is NOT called
    - expect: Validation error message displays

#### 6.5. Enable/disable submit button based on checkbox

**File:** `tests/checkbox/form/button-binding.spec.ts`

**Steps:**
  1. Create checkbox that controls submit button disabled state
    - expect: Submit button is disabled when checkbox is unchecked
  2. Check the checkbox
    - expect: Submit button becomes enabled

#### 6.6. Multiple checkboxes in form

**File:** `tests/checkbox/form/multiple-checkboxes.spec.ts`

**Steps:**
  1. Add multiple checkboxes to form model
    - expect: Each checkbox tracks independently
  2. Toggle each checkbox
    - expect: Each checkbox state updates correctly
  3. Submit form
    - expect: All checkbox states are included in submission

### 7. Accessibility & ARIA

**Seed:** ``

#### 7.1. HTML structure verification

**File:** `tests/checkbox/a11y/html-structure.spec.ts`

**Steps:**
  1. Inspect rendered HTML of checkbox
    - expect: Input element has type='checkbox'
    - expect: Input has unique id attribute
    - expect: Label has correct 'for' attribute matching input id

#### 7.2. ARIA checked attribute

**File:** `tests/checkbox/a11y/aria-checked.spec.ts`

**Steps:**
  1. Render unchecked checkbox
    - expect: aria-checked='false'
  2. Check the checkbox
    - expect: aria-checked='true'
  3. Set indeterminate state
    - expect: aria-checked='mixed'

#### 7.3. ARIA disabled attribute

**File:** `tests/checkbox/a11y/aria-disabled.spec.ts`

**Steps:**
  1. Render enabled checkbox
    - expect: aria-disabled='false'
  2. Render disabled checkbox
    - expect: aria-disabled='true'

#### 7.4. ARIA label for unlabeled checkbox

**File:** `tests/checkbox/a11y/aria-label.spec.ts`

**Steps:**
  1. Render checkbox without Label or ChildContent
    - expect: aria-label is set for accessibility
  2. Can set custom aria-label via InputAttributes
    - expect: Custom aria-label is applied

#### 7.5. Keyboard navigation - Tab key

**File:** `tests/checkbox/a11y/keyboard-tab.spec.ts`

**Steps:**
  1. Tab through page with multiple checkboxes
    - expect: Checkbox receives focus in expected tab order
  2. Tab while on focused checkbox
    - expect: Focus moves to next focusable element
  3. Add disabled checkbox to page
    - expect: Disabled checkbox is skipped in tab order

#### 7.6. Keyboard interaction - Space key

**File:** `tests/checkbox/a11y/keyboard-space.spec.ts`

**Steps:**
  1. Tab to checkbox to focus it
    - expect: Checkbox is focused
  2. Press Space key
    - expect: Checkbox toggles state
  3. Press Space again
    - expect: Checkbox toggles back

#### 7.7. Keyboard tri-state cycling

**File:** `tests/checkbox/a11y/keyboard-tristate.spec.ts`

**Steps:**
  1. Focus tri-state enabled checkbox
    - expect: Keyboard focus is visible
  2. Press Space repeatedly
    - expect: Cycles through checked -> indeterminate -> unchecked

#### 7.8. Focus visible styling

**File:** `tests/checkbox/a11y/focus-visible.spec.ts`

**Steps:**
  1. Tab to focus checkbox
    - expect: Focus indicator is visible (outline or border)
  2. Click with mouse
    - expect: Focus indicator may be hidden (focus-visible)

### 8. RTL (Right-to-Left) Support

**Seed:** ``

#### 8.1. RTL label position - Before

**File:** `tests/checkbox/rtl/rtl-label-before.spec.ts`

**Steps:**
  1. Render checkbox with EnableRtl='true' and LabelPosition='Before'
    - expect: Label appears on the right side in RTL mode
  2. Compare with LTR layout
    - expect: Positions are mirrored correctly

#### 8.2. RTL label position - After

**File:** `tests/checkbox/rtl/rtl-label-after.spec.ts`

**Steps:**
  1. Render checkbox with EnableRtl='true' and LabelPosition='After'
    - expect: Label appears on the left side in RTL mode

#### 8.3. RTL container direction

**File:** `tests/checkbox/rtl/rtl-direction.spec.ts`

**Steps:**
  1. Inspect container element with RTL enabled
    - expect: Container has dir='rtl' attribute or RTL class

#### 8.4. RTL interaction behavior

**File:** `tests/checkbox/rtl/rtl-interaction.spec.ts`

**Steps:**
  1. Render RTL checkbox
    - expect: Clicking checkbox toggles state normally
  2. Use keyboard navigation in RTL
    - expect: Space key toggles state correctly
  3. Verify visual indicator position
    - expect: Checkmark position matches RTL layout

### 9. Grouping & Hierarchical Selection

**Seed:** ``

#### 9.1. Select all functionality

**File:** `tests/checkbox/grouping/select-all.spec.ts`

**Steps:**
  1. Create parent and 3-5 child checkboxes
    - expect: Parent checkbox is unchecked initially
  2. Click parent checkbox once
    - expect: All child checkboxes become checked
  3. Click parent again
    - expect: All child checkboxes become unchecked

#### 9.2. Indeterminate state with partial selection

**File:** `tests/checkbox/grouping/partial-selection.spec.ts`

**Steps:**
  1. Create parent and child checkboxes (parent tri-state enabled)
    - expect: Parent initially shows unchecked
  2. Check some (but not all) children
    - expect: Parent shows indeterminate state
  3. Check remaining child
    - expect: Parent transitions to checked state

#### 9.3. Child update affects parent

**File:** `tests/checkbox/grouping/child-to-parent.spec.ts`

**Steps:**
  1. Create parent and children where parent is tri-state
    - expect: Parent state reflects child states
  2. Check individual children
    - expect: Parent updates to unchecked/indeterminate/checked appropriately

#### 9.4. Parent click syncs to children

**File:** `tests/checkbox/grouping/parent-to-children.spec.ts`

**Steps:**
  1. From indeterminate parent state, click parent
    - expect: All children become checked
  2. Click parent again
    - expect: All children become unchecked
  3. Verify ValueChange fires for children
    - expect: Children events are triggered appropriately

### 10. Edge Cases & Special Scenarios

**Seed:** ``

#### 10.1. Rapid successive clicks

**File:** `tests/checkbox/edge-cases/rapid-clicks.spec.ts`

**Steps:**
  1. Click checkbox 10+ times rapidly
    - expect: Each click toggles the state correctly
  2. Verify final state
    - expect: Final state matches number of clicks

#### 10.2. Change state while disabled

**File:** `tests/checkbox/edge-cases/programmatic-change-disabled.spec.ts`

**Steps:**
  1. Create disabled checkbox
    - expect: User cannot toggle via click
  2. Change state programmatically
    - expect: State changes even when disabled
    - expect: Events fire for programmatic changes

#### 10.3. Toggle disabled property dynamically

**File:** `tests/checkbox/edge-cases/dynamic-disabled.spec.ts`

**Steps:**
  1. Render enabled checkbox and interact with it
    - expect: Checkbox works normally
  2. Set Disabled='true'
    - expect: Checkbox becomes disabled
    - expect: Aria-disabled changes to true
  3. Set Disabled='false'
    - expect: Checkbox becomes enabled again

#### 10.4. Change label dynamically

**File:** `tests/checkbox/edge-cases/dynamic-label.spec.ts`

**Steps:**
  1. Render checkbox with label
    - expect: Initial label displays
  2. Change Label property
    - expect: New label displays immediately
  3. Verify state unchanged
    - expect: Checkbox state is preserved

#### 10.5. Multiple checkboxes independence

**File:** `tests/checkbox/edge-cases/multiple-instances.spec.ts`

**Steps:**
  1. Render 5-10 checkboxes on same page
    - expect: Each checkbox has unique id
  2. Toggle different checkboxes
    - expect: Each maintains independent state
  3. Verify bindings don't cross-contaminate
    - expect: Each binding updates correctly without affecting others

#### 10.6. Component disposal and cleanup

**File:** `tests/checkbox/edge-cases/disposal.spec.ts`

**Steps:**
  1. Render checkbox component
    - expect: Component initializes correctly
  2. Remove checkbox from page
    - expect: JavaScript interop is cleaned up
    - expect: No memory leaks detected

#### 10.7. Value and Name attributes

**File:** `tests/checkbox/edge-cases/value-name-attrs.spec.ts`

**Steps:**
  1. Set Value and Name properties on checkbox
    - expect: Value attribute is on input element
  2. Include checkbox in form submission
    - expect: Name and Value are submitted together

#### 10.8. InputAttributes custom properties

**File:** `tests/checkbox/edge-cases/input-attributes.spec.ts`

**Steps:**
  1. Set InputAttributes with title, readonly, etc.
    - expect: Attributes are applied to input element
  2. Verify functionality with custom attributes
    - expect: Component works correctly with custom attributes

### 11. Performance & State Persistence

**Seed:** ``

#### 11.1. Many checkboxes on single page

**File:** `tests/checkbox/performance/many-checkboxes.spec.ts`

**Steps:**
  1. Render 100+ checkboxes on page
    - expect: Page loads without lag
  2. Interact with individual checkboxes
    - expect: Response time is immediate
  3. Monitor page performance
    - expect: Memory usage remains acceptable

#### 11.2. State persistence to local storage

**File:** `tests/checkbox/performance/persistence.spec.ts`

**Steps:**
  1. Render checkbox with EnablePersistence='true' and check it
    - expect: State is saved to local storage
  2. Refresh the page
    - expect: Checkbox state is restored after page reload
  3. Disable persistence and check box
    - expect: State is NOT saved to local storage on next refresh
