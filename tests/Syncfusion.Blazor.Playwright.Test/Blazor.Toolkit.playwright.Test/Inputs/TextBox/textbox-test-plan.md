# TextBox Component Test Plan

## Application Overview

The SfTextBox is a Blazor component that provides an enhanced input element for capturing and editing text values. It supports single-line and multiline modes with advanced features including floating labels, clear button functionality, input type variations (text, password, email, tel, url, search), CSS customization, validation integration, accessibility features, and event handling. The component supports various input types, placeholder text, disabled and read-only states, RTL layout, and state persistence across browser sessions.

**Features Tested:**
- ✅ Basic rendering and HTML structure
- ✅ Float label behavior (Never, Auto, Always)
- ✅ Input type variations (text, password, email, tel, url, search)
- ✅ Clear button functionality
- ✅ Multiline TextArea support
- ✅ CSS customization and variants (filled, outlined, small, success, error, warning)
- ✅ Value binding and data management
- ✅ Input attributes and HTML attributes (maxlength, minlength, pattern, autocomplete, etc.)
- ✅ Event handling (Input, ValueChange, Focus, Blur, Created, Destroyed)
- ✅ Disabled and ReadOnly states
- ✅ Width and layout control
- ✅ Form integration and validation
- ✅ Icon integration (AddIconAsync)
- ✅ Keyboard navigation and accessibility
- ✅ Right-to-Left (RTL) support
- ✅ State persistence (localStorage)
- ✅ Autocomplete behavior
- ✅ Edge cases and error handling
- ✅ Dynamic changes and re-rendering

## Test Execution

**Total Test Files:** 19
**Total Test Cases:** ~100+
**Status:** Ready for execution

## Test Scenarios

### 1. Basic Rendering and Structure

**Seed:** `seed.spec.ts`

#### 1.1. Verify TextBox basic structure rendering

**File:** `tests/textbox/basic-rendering.spec.ts`

**Steps:**
  1. Navigate to the TextBox samples page at /inputs/textbox
    - expect: The page loads successfully
    - expect: TextBox components are visible on the page
  2. Inspect the first default TextBox element structure
    - expect: TextBox has a wrapper span with class 'e-input-group' or 'e-float-input'
    - expect: Contains an input element with type='text'
    - expect: Contains a span element with class 'e-float-line'
    - expect: Contains a label element for floating label functionality
  3. Verify input element properties
    - expect: Input element has proper id attribute
    - expect: Input element has role='textbox'
    - expect: Input element has aria-label attribute
  4. Check default rendering state
    - expect: TextBox renders as empty input by default
    - expect: Placeholder text is displayed if provided
    - expect: No value is pre-filled unless specified

#### 1.2. Verify disabled TextBox rendering

**File:** `tests/textbox/basic-rendering.spec.ts`

**Steps:**
  1. Locate a disabled TextBox on the page (Disabled='true')
    - expect: Disabled TextBox is visible
    - expect: Disabled attribute is set on the input element
  2. Check disabled button styling
    - expect: Disabled TextBox has visual indication of disabled state
    - expect: TextBox appears grayed out or dimmed
    - expect: Disabled attribute prevents user interaction

#### 1.3. Verify read-only TextBox rendering

**File:** `tests/textbox/basic-rendering.spec.ts`

**Steps:**
  1. Locate a read-only TextBox on the page (ReadOnly='true')
    - expect: Read-only TextBox is visible
    - expect: ReadOnly attribute is set on the input element
  2. Check read-only styling and behavior
    - expect: Read-only TextBox maintains normal appearance (not grayed out)
    - expect: User cannot edit the content
    - expect: User can select and copy text

#### 1.4. Verify TextBox with placeholder text

**File:** `tests/textbox/basic-rendering.spec.ts`

**Steps:**
  1. Find TextBox with Placeholder='Enter text'
    - expect: Placeholder text is displayed in the input field
    - expect: Placeholder text is visible when TextBox is empty and not focused
  2. Verify placeholder behavior on focus
    - expect: Placeholder text disappears when TextBox gains focus
    - expect: Placeholder reappears if TextBox loses focus without input

#### 1.5. Verify ARIA attributes and accessibility attributes

**File:** `tests/textbox/basic-rendering.spec.ts`

**Steps:**
  1. Inspect TextBox for accessibility attributes
    - expect: Input element has aria-label attribute set to 'textbox'
    - expect: Label element is associated with input via 'for' attribute
    - expect: Input element has proper role attribute
  2. Verify label-input association
    - expect: Label 'for' attribute matches input 'id' attribute
    - expect: Clicking label focuses the input element

### 2. Float Label Behavior

**Seed:** `seed.spec.ts`

#### 2.1. Test FloatLabelType Auto - floats on focus

**File:** `tests/textbox/float-label.spec.ts`

**Steps:**
  1. Create TextBox with FloatLabelType='FloatLabelType.Auto'
    - expect: TextBox loads successfully
    - expect: Placeholder text displays inside the input field initially
  2. Click on the TextBox to focus it
    - expect: Placeholder text animates upward and becomes a floating label
    - expect: Floating label is positioned above the input field
    - expect: Floating label is styled with different appearance from placeholder
  3. Blur the TextBox without entering text
    - expect: Floating label animates back down
    - expect: Placeholder text reappears inside input

#### 2.2. Test FloatLabelType Auto - floats when value is entered

**File:** `tests/textbox/float-label.spec.ts`

**Steps:**
  1. Create empty TextBox with FloatLabelType='FloatLabelType.Auto'
    - expect: TextBox displays placeholder text inside input
  2. Type text into the TextBox
    - expect: Placeholder text animates upward and becomes a floating label
    - expect: Floating label remains visible while text is being entered
    - expect: Floating label stays visible even without focus if text exists

#### 2.3. Test FloatLabelType Always - always floats

**File:** `tests/textbox/float-label.spec.ts`

**Steps:**
  1. Create TextBox with FloatLabelType='FloatLabelType.Always'
    - expect: TextBox loads successfully
  2. Verify floating label behavior
    - expect: Placeholder text is always displayed as floating label above input
    - expect: Floating label is visible regardless of focus state
    - expect: Floating label is visible regardless of whether input has value

#### 2.4. Test FloatLabelType Never - no floating label

**File:** `tests/textbox/float-label.spec.ts`

**Steps:**
  1. Create TextBox with FloatLabelType='FloatLabelType.Never'
    - expect: TextBox renders successfully
  2. Verify placeholder behavior
    - expect: Placeholder text stays inside input as static placeholder
    - expect: Placeholder text disappears when input gains focus
    - expect: No floating label animation occurs
    - expect: Placeholder text reappears on blur if no value

#### 2.5. Test placeholder text with special characters

**File:** `tests/textbox/float-label.spec.ts`

**Steps:**
  1. Set placeholder with special characters: &, <, >, quotes, accents
    - expect: Special characters are properly HTML-encoded
    - expect: Placeholder displays safely without XSS issues
  2. Verify floating label with special characters
    - expect: Floating label displays special characters correctly
    - expect: No HTML injection occurs

#### 2.6. Test empty placeholder with float labels

**File:** `tests/textbox/float-label.spec.ts`

**Steps:**
  1. Create TextBox without Placeholder property
    - expect: TextBox renders without placeholder text
    - expect: Floating label is not shown or is empty
    - expect: TextBox remains functional and accepts input

### 3. Input Type Variations

**Seed:** `seed.spec.ts`

#### 3.1. Test Text input type (default)

**File:** `tests/textbox/input-types.spec.ts`

**Steps:**
  1. Create TextBox without specifying Type property
    - expect: Input element has type='text'
  2. Verify text input behavior
    - expect: TextBox accepts alphanumeric characters
    - expect: TextBox accepts special characters
    - expect: No character restrictions apply

#### 3.2. Test Email input type

**File:** `tests/textbox/input-types.spec.ts`

**Steps:**
  1. Create TextBox with Type='InputType.Email'
    - expect: Input element has type='email'
  2. Verify email-specific behavior
    - expect: TextBox displays email placeholder suggestions
    - expect: On mobile, keyboard shows @ symbol and .com suggestions
    - expect: Browser provides email validation on form submission

#### 3.3. Test Password input type

**File:** `tests/textbox/input-types.spec.ts`

**Steps:**
  1. Create TextBox with Type='InputType.Password'
    - expect: Input element has type='password'
  2. Verify password masking
    - expect: Text entered is masked with dots or asterisks
    - expect: Characters are not visible for privacy
    - expect: Clear button works correctly with password field

#### 3.4. Test Tel input type

**File:** `tests/textbox/input-types.spec.ts`

**Steps:**
  1. Create TextBox with Type='InputType.Tel'
    - expect: Input element has type='tel'
  2. Verify tel-specific behavior
    - expect: Mobile keyboard shows numeric keypad
    - expect: Format hints like '+1 234 567 8900' are displayed
    - expect: TextBox accepts phone number formats

#### 3.5. Test URL input type

**File:** `tests/textbox/input-types.spec.ts`

**Steps:**
  1. Create TextBox with Type='InputType.URL'
    - expect: Input element has type='url'
  2. Verify URL-specific behavior
    - expect: Mobile keyboard shows URL-specific keys (/, :, .)
    - expect: Browser may validate URL format
    - expect: Accepts valid URL formats

#### 3.6. Test Search input type

**File:** `tests/textbox/input-types.spec.ts`

**Steps:**
  1. Create TextBox with Type='InputType.Search'
    - expect: Input element has type='search'
  2. Verify search-specific behavior
    - expect: Browser shows search-specific styling
    - expect: Some browsers show clear button with search type
    - expect: Search functionality works normally

### 4. Clear Button Functionality

**Seed:** `seed.spec.ts`

#### 4.1. Test clear button visibility

**File:** `tests/textbox/clear-button.spec.ts`

**Steps:**
  1. Create TextBox with ShowClearButton='true' and empty initially
    - expect: Clear button is not visible when TextBox is empty
  2. Enter text into the TextBox
    - expect: Clear button (X icon) appears in the TextBox
    - expect: Clear button is positioned on the right side of input
    - expect: Clear button has proper styling and icon

#### 4.2. Test clear button click clears value

**File:** `tests/textbox/clear-button.spec.ts`

**Steps:**
  1. Create TextBox with ShowClearButton='true' and enter text
    - expect: TextBox contains the entered text
    - expect: Clear button is visible
  2. Click the clear button
    - expect: TextBox value is cleared to empty string
    - expect: Placeholder text reappears
    - expect: Clear button disappears

#### 4.3. Test clear button click triggers events

**File:** `tests/textbox/clear-button.spec.ts`

**Steps:**
  1. Create TextBox with clear button and attach event handlers
    - expect: Input and ValueChange events are attached
  2. Enter text and click clear button
    - expect: Input event is triggered with cleared value
    - expect: ValueChange event is triggered with null/empty value
    - expect: Events provide previous value in event args

#### 4.4. Test clear button focus restoration

**File:** `tests/textbox/clear-button.spec.ts`

**Steps:**
  1. Create TextBox with clear button and enter text
    - expect: TextBox has focus
  2. Click clear button
    - expect: Focus returns to TextBox after clearing
    - expect: User can immediately type new text without additional click

#### 4.5. Test clear button with ShowClearButton='false'

**File:** `tests/textbox/clear-button.spec.ts`

**Steps:**
  1. Create TextBox with ShowClearButton='false' (default)
    - expect: Clear button is not displayed
  2. Enter text into TextBox
    - expect: No clear icon appears even when TextBox has value
    - expect: Manual clearing only through delete/backspace

#### 4.6. Test clear button with different input types

**File:** `tests/textbox/clear-button.spec.ts`

**Steps:**
  1. Test clear button with email, password, and text inputs
    - expect: Clear button works consistently across all input types
    - expect: Cleared value is empty string
    - expect: No errors occur with different types

### 5. Multiline TextArea Support

**Seed:** `seed.spec.ts`

#### 5.1. Test multiline mode renders textarea

**File:** `tests/textbox/multiline.spec.ts`

**Steps:**
  1. Create TextBox with Multiline='true'
    - expect: Component renders as textarea element instead of input
  2. Verify textarea properties
    - expect: Textarea has proper dimensions for multiline display
    - expect: Text wraps to multiple lines
    - expect: Scrollbar appears when content exceeds visible area

#### 5.2. Test multiline text input and display

**File:** `tests/textbox/multiline.spec.ts`

**Steps:**
  1. Type multiple lines of text with line breaks
    - expect: Line breaks are preserved
    - expect: Text displays on separate lines
    - expect: Textarea maintains proper formatting

#### 5.3. Test multiline with placeholder

**File:** `tests/textbox/multiline.spec.ts`

**Steps:**
  1. Create multiline TextBox with Placeholder='Enter description'
    - expect: Placeholder text is displayed in textarea
    - expect: Placeholder disappears on focus
    - expect: Floating label works with multiline

#### 5.4. Test multiline with maxlength attribute

**File:** `tests/textbox/multiline.spec.ts`

**Steps:**
  1. Create multiline TextBox with InputAttributes maxlength='200'
    - expect: User cannot type beyond maxlength characters
    - expect: Text stops accepting input at maxlength
    - expect: Character count reflects maxlength constraint

#### 5.5. Test multiline with rows attribute

**File:** `tests/textbox/multiline.spec.ts`

**Steps:**
  1. Set InputAttributes with rows='4' and rows='8'
    - expect: Textarea height adjusts based on rows attribute
    - expect: Default rows display expected number of lines
    - expect: Content scrolls when exceeding row height

#### 5.6. Test single-line to multiline mode switching

**File:** `tests/textbox/multiline.spec.ts`

**Steps:**
  1. Create TextBox with Multiline='false'
    - expect: Component renders as input element
  2. Change Multiline to 'true'
    - expect: Component re-renders as textarea element
    - expect: Text content is preserved during switching

### 6. CSS Customization and Variants

**Seed:** `seed.spec.ts`

#### 6.1. Test filled variant (e-filled class)

**File:** `tests/textbox/css-customization.spec.ts`

**Steps:**
  1. Create TextBox with CssClass='e-filled'
    - expect: Filled variant styling is applied
    - expect: TextBox has filled background appearance
    - expect: Underline is visible below input

#### 6.2. Test outlined variant (e-outlined class)

**File:** `tests/textbox/css-customization.spec.ts`

**Steps:**
  1. Create TextBox with CssClass='e-outlined'
    - expect: Outlined variant styling is applied
    - expect: TextBox has border around entire element
    - expect: Focus state shows border color change

#### 6.3. Test small size variant (e-small class)

**File:** `tests/textbox/css-customization.spec.ts`

**Steps:**
  1. Create TextBox with CssClass='e-small'
    - expect: TextBox is smaller than default size
    - expect: Padding and font size are reduced
    - expect: All elements scale proportionally

#### 6.4. Test success color variant (e-success class)

**File:** `tests/textbox/css-customization.spec.ts`

**Steps:**
  1. Create TextBox with CssClass='e-success'
    - expect: TextBox has green/success color styling
    - expect: Success indicator is visible
    - expect: Color is distinct from default

#### 6.5. Test error color variant (e-error class)

**File:** `tests/textbox/css-customization.spec.ts`

**Steps:**
  1. Create TextBox with CssClass='e-error'
    - expect: TextBox has red/error color styling
    - expect: Error indicator is visible
    - expect: User recognizes as error state

#### 6.6. Test warning color variant (e-warning class)

**File:** `tests/textbox/css-customization.spec.ts`

**Steps:**
  1. Create TextBox with CssClass='e-warning'
    - expect: TextBox has yellow/warning color styling
    - expect: Warning indicator is visible

#### 6.7. Test multiple CSS classes

**File:** `tests/textbox/css-customization.spec.ts`

**Steps:**
  1. Create TextBox with CssClass='e-filled e-small'
    - expect: Multiple classes are applied together
    - expect: Styles from both classes are combined
    - expect: No conflicting styles occur

#### 6.8. Test custom CSS class application

**File:** `tests/textbox/css-customization.spec.ts`

**Steps:**
  1. Create TextBox with CssClass='e-custom my-textbox'
    - expect: Custom classes are added to container
    - expect: Custom styling can be applied via CSS
    - expect: Class names are properly formatted

### 7. Value Binding and Data Management

**Seed:** `seed.spec.ts`

#### 7.1. Test string value binding with @bind-Value

**File:** `tests/textbox/value-binding.spec.ts`

**Steps:**
  1. Create TextBox with @bind-Value='@textValue' where textValue='Initial Value'
    - expect: TextBox displays the initial value
    - expect: Value property is properly bound
  2. Verify two-way binding works
    - expect: Changes in bound property reflect in TextBox
    - expect: Changes in TextBox reflect in bound property

#### 7.2. Test value changes reflect in bound property

**File:** `tests/textbox/value-binding.spec.ts`

**Steps:**
  1. Create TextBox with two-way binding
  2. Type new text into TextBox
    - expect: Bound C# property updates with typed text
    - expect: Changes are reflected in real-time with Input event

#### 7.3. Test programmatic value updates

**File:** `tests/textbox/value-binding.spec.ts`

**Steps:**
  1. Update the bound C# property from code
    - expect: TextBox displays the updated value
    - expect: UI re-renders with new value
    - expect: Binding is synchronized

#### 7.4. Test null and empty string values

**File:** `tests/textbox/value-binding.spec.ts`

**Steps:**
  1. Set Value to null or empty string
    - expect: TextBox displays as empty
    - expect: Placeholder reappears when appropriate
    - expect: No errors occur with null values

#### 7.5. Test Value property one-way binding

**File:** `tests/textbox/value-binding.spec.ts`

**Steps:**
  1. Create TextBox with Value='Static Value' (without @bind-)
    - expect: TextBox displays the value
    - expect: User input does not update bound property without two-way binding

#### 7.6. Test clearing value programmatically

**File:** `tests/textbox/value-binding.spec.ts`

**Steps:**
  1. Set Value property to null or empty string
    - expect: TextBox becomes empty
    - expect: Clear button disappears if ShowClearButton is enabled

### 8. Input Attributes and HTML Attributes

**Seed:** `seed.spec.ts`

#### 8.1. Test maxlength attribute constraint

**File:** `tests/textbox/input-attributes.spec.ts`

**Steps:**
  1. Add InputAttributes with maxlength='10'
    - expect: User cannot type more than 10 characters
    - expect: Input stops at maxlength limit

#### 8.2. Test minlength attribute validation

**File:** `tests/textbox/input-attributes.spec.ts`

**Steps:**
  1. Add InputAttributes with minlength='5'
    - expect: TextBox enforces minimum length requirement
    - expect: Validation shows when less than minimum characters

#### 8.3. Test pattern attribute for regex validation

**File:** `tests/textbox/input-attributes.spec.ts`

**Steps:**
  1. Add InputAttributes with pattern='[0-9]{3}-[0-9]{3}-[0-9]{4}'
    - expect: Pattern is applied for format validation
    - expect: Browser validates against regex pattern

#### 8.4. Test autocomplete attribute

**File:** `tests/textbox/input-attributes.spec.ts`

**Steps:**
  1. Set Autocomplete='AutoComplete.Off'
    - expect: Browser autocomplete dropdown is disabled
    - expect: No previous values are suggested

#### 8.5. Test spellcheck attribute

**File:** `tests/textbox/input-attributes.spec.ts`

**Steps:**
  1. Add InputAttributes with spellcheck='true'
    - expect: Spellcheck is enabled in TextBox
    - expect: Misspelled words are underlined

#### 8.6. Test data attributes on input

**File:** `tests/textbox/input-attributes.spec.ts`

**Steps:**
  1. Add InputAttributes with data-testid='email-input'
    - expect: Data attributes are applied to input element
    - expect: Can be used for testing and automation

#### 8.7. Test HtmlAttributes on wrapper element

**File:** `tests/textbox/input-attributes.spec.ts`

**Steps:**
  1. Add HtmlAttributes with custom class and data attributes
    - expect: HtmlAttributes are applied to wrapper element
    - expect: Wrapper has specified attributes

### 9. Events and Callbacks

**Seed:** `seed.spec.ts`

#### 9.1. Test Input event fires on real-time input

**File:** `tests/textbox/events.spec.ts`

**Steps:**
  1. Create TextBox with Input event handler
  2. Type text into TextBox
    - expect: Input event fires for each keystroke
    - expect: Event provides current value and previous value
    - expect: Event is triggered before TextBox loses focus

#### 9.2. Test ValueChange event fires on blur

**File:** `tests/textbox/events.spec.ts`

**Steps:**
  1. Create TextBox with ValueChange event handler
  2. Type text and press Tab to blur
    - expect: ValueChange event fires only once after blur
    - expect: Event provides old and new values
    - expect: Event includes IsInteracted flag set to true

#### 9.3. Test Focus event on focus

**File:** `tests/textbox/events.spec.ts`

**Steps:**
  1. Create TextBox with Focus event handler
  2. Click on TextBox to focus
    - expect: Focus event fires when TextBox gains focus
    - expect: Event provides current value at time of focus

#### 9.4. Test Blur event on blur

**File:** `tests/textbox/events.spec.ts`

**Steps:**
  1. Create TextBox with Blur event handler
  2. Click TextBox, then click outside
    - expect: Blur event fires when focus is lost
    - expect: Event provides current value at time of blur

#### 9.5. Test Created event after component initialization

**File:** `tests/textbox/events.spec.ts`

**Steps:**
  1. Create TextBox with Created event handler
    - expect: Created event fires once after component renders
    - expect: Event can be used for post-initialization setup

#### 9.6. Test Destroyed event on component removal

**File:** `tests/textbox/events.spec.ts`

**Steps:**
  1. Create TextBox with Destroyed event handler
  2. Remove TextBox component from DOM
    - expect: Destroyed event fires before component removal
    - expect: Event provides cleanup opportunity

#### 9.7. Test IsInteracted flag in ValueChange event

**File:** `tests/textbox/events.spec.ts`

**Steps:**
  1. Create TextBox and change value through user interaction
    - expect: IsInteracted is true for user-initiated changes
  2. Change value programmatically
    - expect: IsInteracted is false for programmatic changes

#### 9.8. Test event order and sequencing

**File:** `tests/textbox/events.spec.ts`

**Steps:**
  1. Monitor all events while typing and blurring
    - expect: Input events fire during typing
    - expect: Focus event fires on initial focus
    - expect: Blur event fires on focus loss
    - expect: ValueChange fires after blur if value changed

### 10. Disabled and ReadOnly States

**Seed:** `seed.spec.ts`

#### 10.1. Test disabled TextBox cannot be edited

**File:** `tests/textbox/disabled-readonly.spec.ts`

**Steps:**
  1. Create TextBox with Disabled='true'
    - expect: TextBox appears grayed out
    - expect: User cannot type into disabled TextBox
    - expect: Clicking does not focus disabled TextBox

#### 10.2. Test disabled TextBox styling

**File:** `tests/textbox/disabled-readonly.spec.ts`

**Steps:**
  1. Compare enabled and disabled TextBox
    - expect: Disabled TextBox has reduced opacity
    - expect: Disabled TextBox shows cursor:'not-allowed'
    - expect: Disabled TextBox background appears dimmed

#### 10.3. Test readonly TextBox prevents editing

**File:** `tests/textbox/disabled-readonly.spec.ts`

**Steps:**
  1. Create TextBox with ReadOnly='true' and initial value
    - expect: User cannot type or delete text
    - expect: ReadOnly TextBox maintains normal appearance

#### 10.4. Test readonly TextBox allows selection and copy

**File:** `tests/textbox/disabled-readonly.spec.ts`

**Steps:**
  1. Triple-click readonly TextBox to select all text
    - expect: Text is selected and can be copied
    - expect: User can interact with read-only text for viewing

#### 10.5. Test readonly vs disabled visual difference

**File:** `tests/textbox/disabled-readonly.spec.ts`

**Steps:**
  1. Compare readonly and disabled TextBox side by side
    - expect: Readonly appears normal, disabled appears grayed out
    - expect: Visual distinction is clear to users

#### 10.6. Test toggling disabled state at runtime

**File:** `tests/textbox/disabled-readonly.spec.ts`

**Steps:**
  1. Change Disabled property from true to false
    - expect: TextBox becomes enabled and editable
    - expect: Visual styling updates immediately

#### 10.7. Test toggling readonly state at runtime

**File:** `tests/textbox/disabled-readonly.spec.ts`

**Steps:**
  1. Change ReadOnly property from true to false
    - expect: TextBox becomes editable
    - expect: Text can be modified after toggling off

### 11. Width and Layout Control

**Seed:** `seed.spec.ts`

#### 11.1. Test Width property with pixel values

**File:** `tests/textbox/width-layout.spec.ts`

**Steps:**
  1. Create TextBox with Width='300px'
    - expect: TextBox renders with specified width
    - expect: Element measures 300px wide

#### 11.2. Test Width property with percentage values

**File:** `tests/textbox/width-layout.spec.ts`

**Steps:**
  1. Create TextBox with Width='50%'
    - expect: TextBox takes up 50% of container width
    - expect: Width scales with container

#### 11.3. Test default width (auto/100%)

**File:** `tests/textbox/width-layout.spec.ts`

**Steps:**
  1. Create TextBox without Width property
    - expect: TextBox takes full container width
    - expect: Default width is 100% of parent

#### 11.4. Test Width with different units (em, rem)

**File:** `tests/textbox/width-layout.spec.ts`

**Steps:**
  1. Create TextBox with Width='20rem' and Width='15em'
    - expect: TextBox renders with specified em/rem values
    - expect: Sizing scales relative to font-size

### 12. Form Integration and Validation

**Seed:** `seed.spec.ts`

#### 12.1. Test TextBox in EditForm with validation

**File:** `tests/textbox/form-integration.spec.ts`

**Steps:**
  1. Create TextBox inside EditForm with DataAnnotationsValidator
    - expect: TextBox participates in form validation
    - expect: Validation messages display correctly

#### 12.2. Test Required attribute validation

**File:** `tests/textbox/form-integration.spec.ts`

**Steps:**
  1. Create TextBox with [Required] attribute in model
  2. Submit form without entering value
    - expect: Validation error displays
    - expect: TextBox shows error styling

#### 12.3. Test Email validation

**File:** `tests/textbox/form-integration.spec.ts`

**Steps:**
  1. Create TextBox with [EmailAddress] attribute
  2. Enter invalid email and submit
    - expect: Email validation error displays

#### 12.4. Test StringLength validation

**File:** `tests/textbox/form-integration.spec.ts`

**Steps:**
  1. Create TextBox with [StringLength(50)] attribute
  2. Enter more than 50 characters
    - expect: Length validation error displays on submit

#### 12.5. Test Phone validation

**File:** `tests/textbox/form-integration.spec.ts`

**Steps:**
  1. Create TextBox with [Phone] attribute
  2. Enter invalid phone format
    - expect: Phone validation error displays

#### 12.6. Test validation error styling (e-error class)

**File:** `tests/textbox/form-integration.spec.ts`

**Steps:**
  1. Trigger validation error on TextBox
    - expect: TextBox shows e-error class
    - expect: Error color/styling is applied
    - expect: Error visual feedback is clear

#### 12.7. Test validation success styling (e-success class)

**File:** `tests/textbox/form-integration.spec.ts`

**Steps:**
  1. Enter valid data that passes validation
    - expect: TextBox shows success styling
    - expect: e-success class is applied

#### 12.8. Test form submission with TextBox values

**File:** `tests/textbox/form-integration.spec.ts`

**Steps:**
  1. Fill TextBox with data and submit form
    - expect: TextBox value is included in form data
    - expect: Value is sent to server correctly

### 13. Icon Integration

**Seed:** `seed.spec.ts`

#### 13.1. Test AddIconAsync with prepend position

**File:** `tests/textbox/icons.spec.ts`

**Steps:**
  1. Call AddIconAsync('prepend', 'e-icons e-search') in Created event
    - expect: Icon appears before the input field
    - expect: Icon class is applied correctly
    - expect: Icon is visually positioned left of input

#### 13.2. Test AddIconAsync with append position

**File:** `tests/textbox/icons.spec.ts`

**Steps:**
  1. Call AddIconAsync('append', 'e-icons e-search') in Created event
    - expect: Icon appears after the input field
    - expect: Icon class is applied correctly
    - expect: Icon is visually positioned right of input

#### 13.3. Test icon with click events

**File:** `tests/textbox/icons.spec.ts`

**Steps:**
  1. Add icon with click event handler
    - expect: Icon is clickable
    - expect: Click event fires when icon is clicked

#### 13.4. Test multiple icons

**File:** `tests/textbox/icons.spec.ts`

**Steps:**
  1. Call AddIconAsync for both prepend and append positions
    - expect: Both icons are displayed
    - expect: Icons don't overlap or conflict

#### 13.5. Test icon visibility with content

**File:** `tests/textbox/icons.spec.ts`

**Steps:**
  1. Add icon and enter text into TextBox
    - expect: Icon remains visible with text
    - expect: Icon positioning is not affected by content

### 14. Keyboard Navigation and Accessibility

**Seed:** `seed.spec.ts`

#### 14.1. Test Tab navigation to TextBox

**File:** `tests/textbox/keyboard-navigation.spec.ts`

**Steps:**
  1. Press Tab to navigate to TextBox
    - expect: TextBox receives focus
    - expect: Outline or focus indicator is visible

#### 14.2. Test Tab order with TabIndex property

**File:** `tests/textbox/keyboard-navigation.spec.ts`

**Steps:**
  1. Create TextBox with TabIndex='1' and another with TabIndex='2'
    - expect: Tab navigation follows specified order
    - expect: TabIndex=1 is focused first

#### 14.3. Test typing in focused TextBox

**File:** `tests/textbox/keyboard-navigation.spec.ts`

**Steps:**
  1. Focus TextBox and type text
    - expect: Characters appear in TextBox
    - expect: Input is captured correctly

#### 14.4. Test arrow keys in TextBox

**File:** `tests/textbox/keyboard-navigation.spec.ts`

**Steps:**
  1. Focus TextBox with text and press arrow keys
    - expect: Cursor moves as expected
    - expect: Arrow key behavior is standard

#### 14.5. Test Home/End keys in TextBox

**File:** `tests/textbox/keyboard-navigation.spec.ts`

**Steps:**
  1. Press Home and End keys in focused TextBox
    - expect: Cursor jumps to start/end of line

#### 14.6. Test focus with FocusAsync method

**File:** `tests/textbox/keyboard-navigation.spec.ts`

**Steps:**
  1. Call FocusAsync() on TextBox reference
    - expect: TextBox receives focus
    - expect: Focus indicator is displayed

#### 14.7. Test FocusOutAsync method

**File:** `tests/textbox/keyboard-navigation.spec.ts`

**Steps:**
  1. Call FocusOutAsync() on focused TextBox
    - expect: TextBox loses focus
    - expect: Focus is removed

### 15. Right-to-Left (RTL) Support

**Seed:** `seed.spec.ts`

#### 15.1. Test TextBox with RTL enabled

**File:** `tests/textbox/rtl-support.spec.ts`

**Steps:**
  1. Create TextBox with EnableRtl='true'
    - expect: TextBox renders in right-to-left direction
    - expect: Text input direction is RTL

#### 15.2. Test RTL with floating labels

**File:** `tests/textbox/rtl-support.spec.ts`

**Steps:**
  1. Create RTL TextBox with FloatLabelType='Auto'
    - expect: Floating label appears on correct side in RTL
    - expect: Animation direction is appropriate for RTL

#### 15.3. Test RTL with icons

**File:** `tests/textbox/rtl-support.spec.ts`

**Steps:**
  1. Add icons to RTL-enabled TextBox
    - expect: Prepend icon appears on right in RTL
    - expect: Append icon appears on left in RTL

#### 15.4. Test RTL with clear button

**File:** `tests/textbox/rtl-support.spec.ts`

**Steps:**
  1. Create RTL TextBox with ShowClearButton='true'
    - expect: Clear button appears on left side in RTL
    - expect: Clear button functionality works normally

### 16. State Persistence

**Seed:** `seed.spec.ts`

#### 16.1. Test EnablePersistence stores value in localStorage

**File:** `tests/textbox/state-persistence.spec.ts`

**Steps:**
  1. Create TextBox with EnablePersistence='true' and enter value
    - expect: Value is stored in localStorage
    - expect: Storage uses component ID as key

#### 16.2. Test persistence restores value on page reload

**File:** `tests/textbox/state-persistence.spec.ts`

**Steps:**
  1. Enter text with persistence enabled, reload page
    - expect: TextBox displays persisted value after reload
    - expect: Value is correctly restored

#### 16.3. Test persistence disabled by default

**File:** `tests/textbox/state-persistence.spec.ts`

**Steps:**
  1. Create TextBox without EnablePersistence property
    - expect: Value is not persisted on page reload

#### 16.4. Test multiple TextBox persistence independence

**File:** `tests/textbox/state-persistence.spec.ts`

**Steps:**
  1. Create multiple TextBox components with EnablePersistence='true'
    - expect: Each TextBox stores its own value
    - expect: Values don't interfere with each other

#### 16.5. Test persistence with null/empty values

**File:** `tests/textbox/state-persistence.spec.ts`

**Steps:**
  1. Clear persistent TextBox and reload page
    - expect: Empty state is preserved after reload

### 17. Autocomplete Behavior

**Seed:** `seed.spec.ts`

#### 17.1. Test Autocomplete On (default)

**File:** `tests/textbox/autocomplete.spec.ts`

**Steps:**
  1. Create TextBox with Autocomplete='AutoComplete.On'
    - expect: Browser shows autocomplete suggestions
    - expect: Previously entered values are suggested

#### 17.2. Test Autocomplete Off

**File:** `tests/textbox/autocomplete.spec.ts`

**Steps:**
  1. Create TextBox with Autocomplete='AutoComplete.Off'
    - expect: Browser autocomplete dropdown is disabled
    - expect: No suggestions appear

### 18. Edge Cases and Error Handling

**Seed:** `seed.spec.ts`

#### 18.1. Test TextBox with null initial value

**File:** `tests/textbox/edge-cases.spec.ts`

**Steps:**
  1. Create TextBox with Value='null' or no initial value
    - expect: TextBox renders as empty
    - expect: No errors occur

#### 18.2. Test TextBox with very long text

**File:** `tests/textbox/edge-cases.spec.ts`

**Steps:**
  1. Enter 1000+ character text
    - expect: All text is accepted and displayed
    - expect: No truncation occurs unless maxlength is set

#### 18.3. Test TextBox with special characters

**File:** `tests/textbox/edge-cases.spec.ts`

**Steps:**
  1. Type special characters: &, <, >, quotes, accents, emoji
    - expect: All characters are accepted
    - expect: No XSS vulnerabilities

#### 18.4. Test rapid typing in TextBox

**File:** `tests/textbox/edge-cases.spec.ts`

**Steps:**
  1. Type rapidly or paste large text
    - expect: All characters are captured
    - expect: No characters are lost

#### 18.5. Test TextBox with mixed input validation

**File:** `tests/textbox/edge-cases.spec.ts`

**Steps:**
  1. Try entering invalid format in email type TextBox
    - expect: Browser validates format on submission

#### 18.6. Test TextBox value with whitespace only

**File:** `tests/textbox/edge-cases.spec.ts`

**Steps:**
  1. Enter only spaces into TextBox
    - expect: Whitespace is accepted as valid input

#### 18.7. Test copying and pasting in TextBox

**File:** `tests/textbox/edge-cases.spec.ts`

**Steps:**
  1. Copy text from one TextBox and paste to another
    - expect: Pasted text appears correctly
    - expect: Formatting is preserved

#### 18.8. Test undo/redo in TextBox

**File:** `tests/textbox/edge-cases.spec.ts`

**Steps:**
  1. Type text, press Ctrl+Z to undo
    - expect: Undo removes typed text
    - expect: Redo restores text

### 19. Dynamic Changes and Re-rendering

**Seed:** `seed.spec.ts`

#### 19.1. Test changing Placeholder at runtime

**File:** `tests/textbox/dynamic-changes.spec.ts`

**Steps:**
  1. Change Placeholder property
    - expect: New placeholder text appears immediately

#### 19.2. Test changing FloatLabelType dynamically

**File:** `tests/textbox/dynamic-changes.spec.ts`

**Steps:**
  1. Change FloatLabelType from Never to Auto
    - expect: Float label behavior changes immediately

#### 19.3. Test toggling ShowClearButton at runtime

**File:** `tests/textbox/dynamic-changes.spec.ts`

**Steps:**
  1. Toggle ShowClearButton between true and false
    - expect: Clear button appears/disappears accordingly

#### 19.4. Test changing Width at runtime

**File:** `tests/textbox/dynamic-changes.spec.ts`

**Steps:**
  1. Change Width property
    - expect: TextBox width updates immediately

#### 19.5. Test changing Type at runtime

**File:** `tests/textbox/dynamic-changes.spec.ts`

**Steps:**
  1. Change Type from Text to Password
    - expect: Input type attribute updates

#### 19.6. Test changing CssClass at runtime

**File:** `tests/textbox/dynamic-changes.spec.ts`

**Steps:**
  1. Change CssClass from 'e-filled' to 'e-outlined'
    - expect: Visual styling changes immediately

## Test Design & Conventions

- **Selectors:** Prefer stable IDs provided in the sample page (e.g., `#textTypeInput`, `#clearButtonInput`). When IDs are unavailable use scoped selectors under the component container and avoid brittle class-only selectors.
- **Assertions:** Verify both DOM attributes (`type`, `disabled`, `readonly`, `aria-*`) and visual state classes (`e-focused`, `e-filled`, `e-outlined`, `e-error`, `e-success`, `e-warning`, `e-clear-icon`).
- **Isolation:** Each test must reset persistence (clear `localStorage`) and reload the page when tests mutate storage or component state.
- **Determinism:** Use programmatic date calculations or sample-provided constants where time-sensitive behavior exists. For TextBox, prefer current-month-insensitive checks.
- **Event validation:** Use the harness's visible logs or exposed DOM text where available instead of relying on framework internals.
- **Keyboard events:** Use Playwright `press` for accurate key simulations; validate resulting DOM changes and event log entries.
- **Accessibility:** Validate `aria-label`, `role`, `aria-required` and focus order for keyboard navigation.

## Test Files Mapping (recommended)

- `basic-rendering.spec.ts` — basic DOM, placeholder, aria checks
- `float-label.spec.ts` — float label behaviors (Auto/Always/Never)
- `input-types.spec.ts` — type variations (email, password, tel, url, search)
- `clear-button.spec.ts` — clear button visibility, behavior and events
- `multiline.spec.ts` — textarea/multiline behavior
- `css-customization.spec.ts` — CssClass/variants
- `value-binding.spec.ts` — two-way binding and programmatic updates
- `input-attributes.spec.ts` — maxlength, minlength, pattern, autocomplete, spellcheck
- `events.spec.ts` — Input, ValueChange, Focus, Blur, Created, Destroyed sequencing
- `disabled-readonly.spec.ts` — disabled and readonly behaviors
- `width-layout.spec.ts` — width variants and units
- `form-integration.spec.ts` — EditForm and DataAnnotation validations
- `icons.spec.ts` — AddIconAsync tests and icon interactions
- `keyboard-navigation.spec.ts` — Tab order, Home/End, arrow keys, FocusAsync
- `rtl-support.spec.ts` — RTL layout and visual checks
- `state-persistence.spec.ts` — EnablePersistence checks with `localStorage`
- `autocomplete.spec.ts` — Autocomplete on/off behavior
- `edge-cases.spec.ts` — long text, rapid typing, special chars, undo/redo
- `dynamic-changes.spec.ts` — runtime changes and re-render tests

## Running the Tests

Start the sample app (example):
```bash
cd samples/Blazor.Toolkit.Samples
dotnet watch run
```

Run Playwright tests (example):
```bash
npx playwright test tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/TextBox/
```

Run a specific spec:
```bash
npx playwright test tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/TextBox/clear-button.spec.ts
```

## Test Environment

- **Base URL:** configured in `playwright.config.ts` (usually `http://localhost:5000`) — confirm before running.
- **Browsers:** Chromium for CI; run cross-browser locally if needed.
- **Node / Playwright:** Use the repository's `package.json` and `npx playwright` to run tests.
- **Sample app:** The tests use the sample page at `/inputs/textbox` (see `tests/.../Samples` project).

## Notes & Recommendations

- Ensure the sample page exposes stable IDs for each TextBox used in tests — this file already uses explicit `Id` attributes (e.g., `textTypeInput`).
- Add small harness helper buttons where needed (e.g., toggle persistence, trigger Created/Destroyed hooks) to avoid driving internals via `evaluate` where possible.
- Keep each test independent: clear `localStorage`, reset UI state and reload the page between tests that mutate global state.
- Prefer `locator.fill()` and `locator.press()` over `evaluate` for user-like interactions.
- Use `textbox-utils.ts` for shared helpers and to keep tests concise and DRY.
- When adding more tests, group them by feature area and limit per-file size to maintain readability and execution speed.

