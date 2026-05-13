# TextArea Component Test Plan

## Application Overview

The SfTextArea is a Blazor component that provides an enhanced textarea element for capturing and editing multiline text values. It supports advanced features including floating labels, clear button functionality, resize modes (None, Vertical, Horizontal, Both), maxlength validation, CSS customization (appearance variants: Outline, Filled), value binding, form integration with validation, accessibility features, keyboard navigation, RTL support, and event handling.

**Features Tested:**
- ✅ Basic rendering and HTML structure
- ✅ Float label behavior (Never, Auto, Always)
- ✅ Row and column count configuration
- ✅ Resize modes (None, Vertical, Horizontal, Both)
- ✅ MaxLength validation
- ✅ Clear button functionality
- ✅ Disabled and ReadOnly states
- ✅ Appearance variants (Outline, Filled) - dynamic switching
- ✅ Value binding and data management
- ✅ Input attributes and HTML attributes (maxlength, minlength, etc.)
- ✅ Event handling (Input, ValueChange, Focus, Blur)
- ✅ Form integration and EditContext validation
- ✅ Keyboard navigation and accessibility
- ✅ Right-to-Left (RTL) support
- ✅ State persistence (localStorage)
- ✅ API attributes (aria-label, data-testid, custom CSS classes)
- ✅ Edge cases and error handling
- ✅ Dynamic changes and re-rendering

## Test Execution

**Total Test Files:** 16
**Total Test Cases:** ~90+
**Status:** Ready for execution

## Test Scenarios

### 1. Basic Rendering and Structure

**File:** `basic-rendering.spec.ts`

#### 1.1. Verify TextArea basic structure rendering
- Navigate to `/inputs/textarea` page
- Verify TextArea components are visible
- Check textarea element has proper wrapper structure
- Verify placeholder text display
- Confirm default empty state

#### 1.2. Verify disabled TextArea rendering
- Locate TextArea with `Disabled="true"`
- Verify disabled attribute on element
- Check visual disabled styling (grayed out, dimmed)
- Verify inability to interact

#### 1.3. Verify read-only TextArea rendering
- Locate TextArea with `ReadOnly="true"`
- Verify readonly attribute
- Check normal appearance (not grayed out)
- Verify text selection still works

#### 1.4. Verify TextArea with placeholder text
- Find TextArea with `Placeholder="Enter text"`
- Verify placeholder displays when empty
- Verify placeholder disappears on focus/input

#### 1.5. Verify ARIA attributes
- Check aria-label attributes
- Verify label association
- Confirm proper role attributes
- Validate accessibility compliance

### 2. Float Label Behavior

**File:** `float-label.spec.ts`

#### 2.1. Test FloatLabelType Auto - floats on focus
- Create TextArea with `FloatLabelType="FloatLabelType.Auto"`
- Click to focus TextArea
- Verify placeholder animates upward
- Verify floating label styling

#### 2.2. Test FloatLabelType Auto - floats when value exists
- Create empty TextArea with Auto float label
- Type text into TextArea
- Verify placeholder becomes floating label
- Verify floating label persists with text

#### 2.3. Test FloatLabelType Always - always floats
- Create TextArea with `FloatLabelType="FloatLabelType.Always"`
- Verify placeholder always displays as floating label
- Verify visible regardless of focus or value

#### 2.4. Test FloatLabelType Never - no floating label
- Create TextArea with `FloatLabelType="FloatLabelType.Never"`
- Verify placeholder stays inside textarea
- Verify no floating animation
- Confirm static placeholder behavior

#### 2.5. Test placeholder with special characters
- Use placeholders with &, <, >, quotes, accents
- Verify proper HTML encoding
- Confirm no XSS vulnerabilities

#### 2.6. Test empty placeholder with float labels
- Create TextArea without Placeholder
- Verify functionality without placeholder text
- Confirm TextArea remains usable

### 3. Row and Column Count

**File:** `row-column-count.spec.ts`

#### 3.1. Test default Rows and Cols attributes
- Create TextArea with `Rows="10"` and `Cols="50"`
- Verify textarea has specified rows
- Verify textarea has specified cols
- Confirm proper dimensions

#### 3.2. Test custom Rows configuration
- Set `Rows="5"`, `Rows="8"`, `Rows="15"`
- Verify height adjusts based on rows
- Confirm proper number of visible lines

#### 3.3. Test custom Cols configuration
- Set `Cols="30"`, `Cols="60"`, `Cols="100"`
- Verify width adjusts based on cols
- Confirm text wraps at specified column width

#### 3.4. Test dynamic Rows update
- Create TextArea with dynamic rows input
- Change rows value at runtime
- Verify height updates immediately
- Confirm re-rendering works

#### 3.5. Test dynamic Cols update
- Create TextArea with dynamic cols input
- Change cols value at runtime
- Verify width updates immediately
- Confirm content reflows

### 4. Resize Modes

**File:** `resize-modes.spec.ts`

#### 4.1. Test no resize mode (Resize.None)
- Create TextArea with `ResizeMode="Resize.None"`
- Verify resize handle is not visible
- Confirm user cannot resize textarea

#### 4.2. Test vertical resize only (Resize.Vertical)
- Create TextArea with `ResizeMode="Resize.Vertical"`
- Verify resize handle appears at bottom
- Confirm vertical resizing is allowed
- Verify horizontal resizing is prevented

#### 4.3. Test horizontal resize only (Resize.Horizontal)
- Create TextArea with `ResizeMode="Resize.Horizontal"`
- Verify resize handle appears at right
- Confirm horizontal resizing is allowed
- Verify vertical resizing is prevented

#### 4.4. Test both directions resize (Resize.Both)
- Create TextArea with `ResizeMode="Resize.Both"`
- Verify resize handle appears at corner
- Confirm resizing in both directions allowed
- Verify proper resize cursor

### 5. MaxLength Validation

**File:** `maxlength-validation.spec.ts`

#### 5.1. Test maxlength attribute is applied
- Create TextArea with `MaxLength="100"`
- Verify maxlength attribute on element
- Confirm value is "100"

#### 5.2. Test cannot exceed maxlength of 50
- Create TextArea with `MaxLength="50"`
- Type 100 characters
- Verify input stops at 50 characters
- Confirm character count doesn't exceed limit

#### 5.3. Test cannot exceed maxlength of 100
- Create TextArea with `MaxLength="100"`
- Type 150 characters
- Verify input stops at 100 characters
- Confirm browser enforces limit

#### 5.4. Test cannot exceed maxlength of 500
- Create TextArea with `MaxLength="500"`
- Type beyond 500 characters
- Verify input stops at 500
- Confirm limit is enforced

#### 5.5. Test exact character limit can be filled
- Create TextArea with `MaxLength="50"`
- Fill exactly 50 characters
- Verify all characters are accepted
- Confirm no truncation

#### 5.6. Test paste operation respects maxlength
- Create TextArea with `MaxLength="50"`
- Attempt to paste 100 characters
- Verify paste stops at maxlength limit
- Confirm browser validation works

#### 5.7. Test maxlength display updates character count
- Display current character count
- Type text and verify count updates
- Confirm count reflects maxlength progress

### 6. Clear Button Functionality

**File:** `clear-button.spec.ts`

#### 6.1. Test clear button visibility with content
- Create TextArea with `ShowClearButton="true"`
- Enter text
- Verify clear button (X icon) appears
- Verify clear button is positioned correctly

#### 6.2. Test clear button is hidden when empty
- Create TextArea with `ShowClearButton="true"`
- Leave empty or clear
- Verify clear button is not visible
- Confirm button disappears

#### 6.3. Test clicking clear empties textarea
- Create TextArea with `ShowClearButton="true"`
- Enter text and click clear button
- Verify textarea value is cleared
- Confirm placeholder reappears

#### 6.4. Test clear button focuses textarea after clearing
- Create TextArea with `ShowClearButton="true"`
- Enter text and click clear button
- Verify focus returns to textarea
- Confirm user can type immediately

#### 6.5. Test textarea without clear button
- Create TextArea with `ShowClearButton="false"`
- Enter text
- Verify clear button does not appear
- Confirm manual clearing via delete only

#### 6.6. Test clear button works multiple times
- Create TextArea with `ShowClearButton="true"`
- Repeat entering and clearing 3+ times
- Verify clear button works consistently
- Confirm no errors occur

### 7. Disabled and ReadOnly States

**File:** `disabled-readonly.spec.ts`

#### 7.1. Test disabled TextArea cannot be edited
- Create TextArea with `Disabled="true"`
- Attempt to type
- Verify typing is prevented
- Confirm disabled attribute prevents interaction

#### 7.2. Test disabled TextArea styling
- Compare enabled vs disabled TextArea
- Verify disabled appears grayed out/dimmed
- Confirm visual distinction is clear

#### 7.3. Test readonly TextArea prevents editing
- Create TextArea with `ReadOnly="true"` and initial value
- Attempt to type or delete
- Verify editing is prevented
- Confirm readonly attribute works

#### 7.4. Test readonly TextArea allows selection and copy
- Create TextArea with `ReadOnly="true"`
- Select all text (Ctrl+A)
- Copy text (Ctrl+C)
- Verify selection and copy work

#### 7.5. Test readonly vs disabled visual difference
- Compare readonly and disabled side by side
- Verify readonly appears normal (not dimmed)
- Confirm disabled appears visually disabled

#### 7.6. Test toggling disabled state at runtime
- Change `Disabled` from true to false
- Verify TextArea becomes editable
- Confirm visual styling updates

#### 7.7. Test toggling readonly state at runtime
- Change `ReadOnly` from true to false
- Verify TextArea becomes editable
- Confirm text can be modified

### 8. Appearance Variants

**File:** `appearance.spec.ts`

#### 8.1. Test outline appearance rendering
- Create TextArea with `Appearance="AppearanceType.Outline"`
- Verify outline style is applied
- Confirm border is visible

#### 8.2. Test filled appearance rendering
- Create TextArea with `Appearance="AppearanceType.Filled"`
- Verify filled style is applied
- Confirm background color is present

#### 8.3. Test appearance can be switched dynamically to filled
- Create TextArea with dynamic appearance switching
- Click "Filled" button
- Verify appearance changes to filled
- Confirm visual styling updates

#### 8.4. Test appearance can be switched dynamically to outline
- Create TextArea with dynamic switching enabled
- Switch to filled then back to outline
- Verify outline style is restored
- Confirm transitions work smoothly

#### 8.5. Test appearance does not affect functionality
- Test with outline appearance, enter text
- Switch to filled appearance
- Verify text input works in both appearances
- Confirm functionality is unaffected

### 9. Value Binding

**File:** `value-binding.spec.ts`

#### 9.1. Test two-way value binding with @bind-Value
- Create TextArea with `@bind-Value="@textValue"`
- Enter text in textarea
- Verify bound property updates
- Confirm changes in bound property reflect in textarea

#### 9.2. Test programmatic value updates
- Update bound property from code
- Click "Set Value" button
- Verify textarea displays new value
- Confirm binding synchronizes

#### 9.3. Test clear value programmatically
- Set value programmatically
- Click "Clear Value" button
- Verify textarea becomes empty
- Confirm clear button disappears if enabled

#### 9.4. Test initial value is displayed
- Create TextArea with initial value='Syncfusion TextArea'
- Verify value displays on load
- Confirm initial value is not empty

#### 9.5. Test null and empty string values
- Set Value to null
- Verify textarea displays as empty
- Confirm no errors occur with null values

#### 9.6. Test multiline value binding works correctly
- Bind value with multiple lines (Line1\nLine2\nLine3)
- Verify all lines display
- Confirm line breaks are preserved

#### 9.7. Test special characters in value binding
- Bind value with special chars: !@#$%^&*()
- Verify special characters display correctly
- Confirm no rendering issues

### 10. Events

**File:** `events.spec.ts`

#### 10.1. Test Input event fires on real-time input
- Create TextArea with Input event handler
- Type text
- Verify Input event fires for each keystroke
- Confirm event provides current value

#### 10.2. Test ValueChange event fires on blur
- Create TextArea with ValueChange event handler
- Type text and blur
- Verify ValueChange fires once after blur
- Confirm event provides old and new values

#### 10.3. Test Focus event fires on focus
- Create TextArea with Focus event handler
- Click to focus
- Verify Focus event fires
- Confirm event provides value at focus time

#### 10.4. Test Blur event fires on blur
- Create TextArea with Blur event handler
- Click and then click outside
- Verify Blur event fires on focus loss
- Confirm event provides value at blur time

#### 10.5. Test event message updates correctly
- Type text and observe event messages
- Verify messages display event type and value
- Confirm all events are logged properly

### 11. Form Integration and Validation

**File:** `validation-edit-context.spec.ts`

#### 11.1. Test required field validation triggers on empty submit
- Create TextArea with [Required] attribute
- Submit form without entering value
- Verify validation error displays
- Confirm error styling is applied

#### 11.2. Test required field validates successfully with value
- Create TextArea with [Required] attribute
- Enter value and submit
- Verify no validation error displays
- Confirm form submits successfully

#### 11.3. Test comments field maxlength validation
- Create TextArea with MaxLength="200"
- Verify maxlength attribute set to 200
- Confirm input stops at 200 characters

#### 11.4. Test comments field validates minimum length
- Create TextArea with StringLength(5, 200) validation
- Enter less than 5 characters
- Verify validation error displays
- Confirm length requirement is enforced

#### 11.5. Test comments field validates maximum length
- Create TextArea with StringLength(5, 200) validation
- Enter more than 200 characters
- Verify validation error displays
- Confirm maximum is enforced

#### 11.6. Test feedback field has optional validation
- Create optional TextArea without [Required]
- Submit form without filling
- Verify no validation error displays
- Confirm optional field works

#### 11.7. Test form reset clears all textareas
- Fill all textareas with values
- Click Reset button
- Verify all textareas clear
- Confirm form state resets

#### 11.8. Test validation message disappears when valid
- Submit with validation error
- Fix the validation error
- Verify error message disappears
- Confirm valid styling is shown

### 12. Keyboard Navigation & Accessibility

**File:** `keyboard-accessibility.spec.ts`

#### 12.1. Test Tab navigation to TextArea
- Press Tab repeatedly
- Verify TextArea receives focus
- Confirm focus indicator is visible

#### 12.2. Test Tab order respects TabIndex
- Create multiple textareas with different TabIndex values
- Press Tab and verify navigation order
- Confirm proper sequencing

#### 12.3. Test typing in focused TextArea
- Focus TextArea via Tab or click
- Type text
- Verify characters appear in textarea

#### 12.4. Test arrow keys navigate within TextArea
- Focus TextArea with text
- Press arrow keys
- Verify cursor moves correctly
- Confirm text selection works

#### 12.5. Test Home key moves to start of line
- Focus TextArea with content
- Press Home key
- Verify cursor moves to line start
- Confirm positioning is correct

#### 12.6. Test End key moves to end of line
- Focus TextArea with content
- Press End key
- Verify cursor moves to line end
- Confirm positioning is correct

#### 12.7. Test Ctrl+A selects all text
- Focus TextArea and type text
- Press Ctrl+A
- Verify all text is selected
- Confirm selection works

#### 12.8. Test Ctrl+C copies text
- Select text with Ctrl+A
- Press Ctrl+C
- Verify text is copied to clipboard
- Confirm copy functionality works

#### 12.9. Test Backspace deletes character
- Focus TextArea with text
- Press Backspace
- Verify character before cursor is deleted
- Confirm deletion works correctly

#### 12.10. Test Delete key removes character
- Position cursor at start
- Press Delete
- Verify character after cursor is removed
- Confirm deletion works

#### 12.11. Test Enter key creates new line
- Focus TextArea
- Type "Line 1", press Enter, type "Line 2"
- Verify text appears on separate lines
- Confirm multiline entry works

#### 12.12. Test Shift+Tab navigates backward
- Focus TextArea
- Press Shift+Tab
- Verify focus moves to previous element
- Confirm backward navigation works

#### 12.13. Test ARIA attributes are accessible
- Verify aria-label attributes
- Confirm role attributes
- Validate accessibility compliance

#### 12.14. Test focus indicator is visible
- Focus TextArea
- Verify focus indicator is visible
- Confirm visual feedback is clear

#### 12.15. Test Ctrl+Z undo works
- Type text
- Press Ctrl+Z
- Verify text is undone
- Confirm undo functionality works

#### 12.16. Test Ctrl+Y redo works
- Type and undo
- Press Ctrl+Y
- Verify text is redone
- Confirm redo functionality works

### 13. State Persistence

**File:** `state-persistence.spec.ts`

#### 13.1. Test EnablePersistence stores value in localStorage
- Create TextArea with EnablePersistence="true"
- Enter text
- Verify localStorage contains value
- Confirm storage key is correct

#### 13.2. Test persistence restores value on page reload
- Enter text with persistence enabled
- Reload page
- Verify textarea displays persisted value
- Confirm restoration works

#### 13.3. Test persistence disabled by default
- Create TextArea without EnablePersistence
- Enter text and reload
- Verify value is NOT restored
- Confirm default is no persistence

#### 13.4. Test multiple TextArea persistence independence
- Create two persistent textareas
- Fill with different values
- Reload page
- Verify each has correct persisted value

#### 13.5. Test persistence with null/empty values
- Fill and then clear textarea
- Reload page
- Verify empty state is preserved
- Confirm empty values persist

#### 13.6. Test persistence with special characters
- Persist value with special characters
- Reload page
- Verify special characters are preserved
- Confirm encoding/decoding works

#### 13.7. Test clearing localStorage clears persistence
- Store value with persistence
- Clear localStorage
- Reload page
- Verify value is cleared

#### 13.8. Test persistence survives multiple reloads
- Persist value and reload 3+ times
- Verify value remains across all reloads
- Confirm stability

### 14. Dynamic Changes and Re-rendering

**File:** `dynamic-changes.spec.ts`

#### 14.1. Test changing Placeholder at runtime
- Change Placeholder property
- Verify new placeholder displays
- Confirm update is immediate

#### 14.2. Test changing FloatLabelType dynamically
- Change FloatLabelType from Never to Auto
- Verify float label behavior changes
- Confirm immediate update

#### 14.3. Test toggling ShowClearButton at runtime
- Toggle ShowClearButton between true/false
- Verify clear button appears/disappears
- Confirm dynamic toggle works

#### 14.4. Test changing Rows at runtime
- Change Rows property
- Verify height updates immediately
- Confirm re-rendering works

#### 14.5. Test changing Appearance at runtime
- Switch appearance between Outline/Filled
- Verify styling updates
- Confirm smooth transition

#### 14.6. Test toggling Disabled at runtime
- Toggle Disabled property
- Verify enabled/disabled state changes
- Confirm visual styling updates

### 15. API Attributes and Accessibility

**File:** `api-attributes.spec.ts`

#### 15.1. Test aria-label attribute is applied
- Create TextArea with aria-label
- Verify aria-label attribute present
- Confirm accessibility attribute works

#### 15.2. Test aria-multiline is set to true
- Create TextArea
- Verify aria-multiline="true"
- Confirm multiline attribute

#### 15.3. Test data-testid attribute is present
- Create TextArea with data-testid
- Verify data-testid attribute present
- Confirm testid attribute works

#### 15.4. Test custom CSS class is applied
- Create TextArea with CssClass="custom-class"
- Verify class is applied
- Confirm custom styling works

#### 15.5. Test textarea with custom attributes is accessible
- Create TextArea with custom attributes
- Verify all attributes work correctly
- Confirm accessibility

#### 15.6. Test multiple HTML attributes can be combined
- Apply multiple attributes
- Verify all are present and functional
- Confirm combination works

### 16. Edge Cases and Error Handling

**File:** `edge-cases.spec.ts`

#### 16.1. Test TextArea with null initial value
- Create with Value="null"
- Verify renders as empty
- Confirm no errors

#### 16.2. Test TextArea with very long text (1000+ chars)
- Enter long text
- Verify all text accepted
- Confirm no truncation unless maxlength set

#### 16.3. Test TextArea with special characters
- Type special chars: &, <, >, quotes, accents, emoji
- Verify all accepted
- Confirm no XSS issues

#### 16.4. Test rapid typing in TextArea
- Type or paste rapidly
- Verify all input captured
- Confirm no character loss

#### 16.5. Test copying and pasting in TextArea
- Copy from one textarea, paste to another
- Verify paste works correctly
- Confirm formatting preserved

#### 16.6. Test TextArea value with whitespace only
- Enter only spaces
- Verify accepted as valid input
- Confirm whitespace handling

#### 16.7. Test multiline with line breaks
- Enter text with \n line breaks
- Verify line breaks preserved
- Confirm multiline formatting

## Test Design & Conventions

- **Selectors:** Use stable IDs provided in sample page (e.g., `#defaultTextArea`, `#clearButtonTextArea`)
- **Assertions:** Verify both DOM attributes and visual state
- **Isolation:** Reset localStorage between persistence tests
- **Event Validation:** Check visible event messages in DOM
- **Keyboard Events:** Use Playwright `press()` for accurate simulation
- **Accessibility:** Validate ARIA attributes and focus order

## Test Files Mapping

- `basic-rendering.spec.ts` — DOM structure, placeholder, aria checks
- `float-label.spec.ts` — float label types (Auto/Always/Never)
- `row-column-count.spec.ts` — rows and cols attributes
- `resize-modes.spec.ts` — resize behavior
- `maxlength-validation.spec.ts` — maxlength constraints
- `clear-button.spec.ts` — clear button visibility and behavior
- `disabled-readonly.spec.ts` — disabled and readonly states
- `appearance.spec.ts` — appearance variants (Outline/Filled)
- `value-binding.spec.ts` — two-way binding and updates
- `events.spec.ts` — Input, ValueChange, Focus, Blur events
- `validation-edit-context.spec.ts` — EditForm and validation
- `keyboard-accessibility.spec.ts` — keyboard navigation and accessibility
- `state-persistence.spec.ts` — localStorage persistence
- `dynamic-changes.spec.ts` — runtime property changes
- `api-attributes.spec.ts` — aria and custom attributes
- `edge-cases.spec.ts` — special characters, long text, etc.

## Running the Tests

Start the sample app:
```bash
cd samples/Blazor.Toolkit.Samples
dotnet watch run
```

Run all TextArea tests:
```bash
npx playwright test tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/TextArea/
```

Run specific test file:
```bash
npx playwright test textarea-test-plan.spec.ts
```

## Test Environment

- **Base URL:** `http://localhost:5000` (configured in `playwright.config.ts`)
- **Browsers:** Chromium for CI, cross-browser locally
- **Sample Page:** `/inputs/textarea`

## Notes & Recommendations

- Ensure sample page has stable IDs for all test textareas
- Clear localStorage before persistence tests
- Add helper button for toggling properties if needed
- Keep tests independent and modular
- Use `locator.fill()` for realistic user input
- Reference TextBox test plan for similar test patterns