# Button Component Test Plan

## IMPORTANT: Supported vs Unsupported Features

### ✅ SUPPORTED Properties
- **Content** - String content to display
- **ChildContent** - Custom HTML child content
- **CssClass** - CSS classes for styling
- **Disabled** - Boolean flag to disable button
- **EnableRtl** - Enable right-to-left support
- **IconCss** - CSS classes for icons
- **IconPosition** - Icon positioning (Left, Right, Top, Bottom)
- **IsPrimary** - Primary button styling flag
- **IsToggle** - Toggle button functionality flag
- **HtmlAttributes** - Capture unmatched HTML attributes (title, data-*, etc.)

### ✅ SUPPORTED Events
- **OnClick** - Fired when button is clicked by user
- **Created** - Fired after component first renders

### ❌ NOT SUPPORTED
- **Type property** - Button HTML type attribute is not configurable
- **@bind-Toggled** - Two-way binding for toggle state is not supported
- **Label property** - Not available for SfButton
- **Name property** - Not available for SfButton
- **Toggled parameter** - Cannot bind to or read toggle state programmatically
- **Form submission typing** - Cannot specify button types (Submit/Reset/Button)

### 🔄 Updated Test Cases
Due to missing `Toggled` parameter, tests have been updated to:
- Verify toggle buttons render correctly with `IsToggle='true'`
- Test user interactions (clicks) on toggle buttons
- Removed expectations about reading/tracking toggle state from .razor code
- Toggle state is tracked internally via `e-active` CSS class from component

## Application Overview

The SfButton component is a Syncfusion Blazor Toolkit button that triggers events when clicked, supporting text, icons, SVG, or a combination as its content. This comprehensive test plan covers all button features including content display, styling variants, semantic colors, disabled states, icon positioning, toggle functionality, RTL support, and event handling.

## Test Scenarios

### 1. Content & Display

**Seed:** `seed.spec.ts`

#### 1.1. Render button with string content

**File:** `tests/button/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Button renders successfully with text content
    - expect: Button displays text 'Click Me'
  2. -
    - expect: Button is visible on the page
    - expect: Text is properly centered within the button
  3. -
    - expect: Button has proper padding and height
    - expect: Content is not truncated

#### 1.2. Render button with empty content

**File:** `tests/button/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with no visible text
    - expect: Button still has height and width
  2. -
    - expect: Button element exists in DOM
    - expect: Button is clickable even without content

#### 1.3. Render button with child HTML content

**File:** `tests/button/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with custom HTML child content
    - expect: Custom HTML elements display correctly inside button
  2. -
    - expect: Button layout adapts to child content size
    - expect: Child content styling is preserved

#### 1.4. Render button with very long text content

**File:** `tests/button/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Button displays all text content
    - expect: Text wraps properly within button bounds
  2. -
    - expect: Button maintains proper shape
    - expect: Text is readable and not distorted

#### 1.5. Update button content dynamically

**File:** `tests/button/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Button initially displays first content
    - expect: Content updates after state change
  2. -
    - expect: Button re-renders with new content
    - expect: Old content is completely replaced

#### 1.6. Render button with special characters in content

**File:** `tests/button/content-display.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with special characters
    - expect: Content is properly escaped and displayed
  2. -
    - expect: XSS attack attempt is prevented
    - expect: Malicious scripts are not executed

### 2. Button Styling & Appearance

**Seed:** `seed.spec.ts`

#### 2.1. Render default button without CSS classes

**File:** `tests/button/styling.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with default solid appearance
    - expect: Button has proper default styling
  2. -
    - expect: Button element has e-control, e-btn, e-lib classes applied
    - expect: Button is styled consistently

#### 2.2. Render outlined button variant

**File:** `tests/button/styling.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with outlined style (CssClass='e-outline')
    - expect: Button has visible border and transparent background
  2. -
    - expect: Button outline is clearly visible
    - expect: Button is visually distinct from filled buttons

#### 2.3. Render flat button variant

**File:** `tests/button/styling.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with flat style (CssClass='e-flat')
    - expect: Button has flat appearance with solid background
  2. -
    - expect: Button has reduced visual depth
    - expect: Button appearance is simpler than default

#### 2.4. Render round button variant

**File:** `tests/button/styling.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with round style (CssClass='e-round')
    - expect: Button corners are fully rounded
  2. -
    - expect: Round button is circular or nearly circular
    - expect: Button icon-only buttons appear as circles

#### 2.5. Apply semantic color - info

**File:** `tests/button/styling.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with info color (CssClass='e-info')
    - expect: Button displays in blue/info color scheme
  2. -
    - expect: Info color is visually distinct
    - expect: Color matches design system specifications

#### 2.6. Apply semantic color - success

**File:** `tests/button/styling.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with success color (CssClass='e-success')
    - expect: Button displays in green/success color scheme
  2. -
    - expect: Success color is visually distinct
    - expect: Color matches design system specifications

#### 2.7. Apply semantic color - warning

**File:** `tests/button/styling.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with warning color (CssClass='e-warning')
    - expect: Button displays in orange/yellow color scheme
  2. -
    - expect: Warning color is visually distinct
    - expect: Color conveys caution appropriately

#### 2.8. Apply semantic color - danger

**File:** `tests/button/styling.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with danger color (CssClass='e-danger')
    - expect: Button displays in red/danger color scheme
  2. -
    - expect: Danger color is visually distinct
    - expect: Color conveys destructive action

#### 2.9. Combine button type with semantic color

**File:** `tests/button/styling.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with combined styles (e.g., 'e-outline e-success')
    - expect: Button has both outline style and success color
  2. -
    - expect: Styles combine correctly without conflicts
    - expect: Button appearance is visually coherent

#### 2.10. Apply primary button styling with IsPrimary property

**File:** `tests/button/styling.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with IsPrimary='true'
    - expect: Button displays with primary styling (e.g., e-primary class)
  2. -
    - expect: Primary button is visually prominent
    - expect: Primary styling takes precedence over other styles

#### 2.11. Apply custom CSS classes

**File:** `tests/button/styling.spec.ts`

**Steps:**
  1. -
    - expect: Custom CSS class is applied to button element
    - expect: Custom styles are reflected in button appearance
  2. -
    - expect: Multiple custom classes can be applied
    - expect: Button applies both Syncfusion and custom CSS classes

### 3. Button States & Interactions

**Seed:** `seed.spec.ts`

#### 3.1. Click enabled button

**File:** `tests/button/states.spec.ts`

**Steps:**
  1. -
    - expect: Button is clickable when Disabled='false'
    - expect: Click event fires when button is clicked
  2. -
    - expect: Button visual feedback shows on click
    - expect: OnClick handler is invoked

#### 3.2. Disable button with Disabled property

**File:** `tests/button/states.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with disabled state (Disabled='true')
    - expect: Button appears visually disabled
  2. -
    - expect: Button is not clickable
    - expect: Mouse cursor changes to 'not-allowed'
    - expect: aria-disabled='true' attribute is set
  3. -
    - expect: Click events are not triggered on disabled button
    - expect: No OnClick handler is invoked

#### 3.3. Toggle button between enabled and disabled states

**File:** `tests/button/states.spec.ts`

**Steps:**
  1. -
    - expect: Button starts as enabled
    - expect: Button is clickable and shows enabled styling
  2. -
    - expect: Button is disabled programmatically
    - expect: Button visual state updates to disabled
  3. -
    - expect: Button is re-enabled
    - expect: Button returns to enabled state and is clickable again

#### 3.4. Toggle button functionality with IsToggle property

**File:** `tests/button/states.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with IsToggle='true'
    - expect: Button can toggle between active and inactive states
  2. -
    - expect: First click activates button (e-active class added)
    - expect: Button visual appearance changes
  3. -
    - expect: Second click deactivates button (e-active class removed)
    - expect: Button returns to inactive appearance

#### 3.5. Toggle button with initial active state

**File:** `tests/button/states.spec.ts`

**Steps:**
  1. -
    - expect: Toggle button renders with initial e-active class applied
    - expect: Button displays as active/pressed
  2. -
    - expect: First click deactivates the button
    - expect: Active class is removed

#### 3.6. aria-pressed attribute for toggle button

**File:** `tests/button/states.spec.ts`

**Steps:**
  1. -
    - expect: Toggle button has aria-pressed attribute
    - expect: aria-pressed='false' when button is inactive
  2. -
    - expect: aria-pressed='true' when button is active
    - expect: Accessibility requirements are met

#### 3.7. Button hover state styling

**File:** `tests/button/states.spec.ts`

**Steps:**
  1. -
    - expect: Button is initially not hovered
    - expect: Button styling is in normal state
  2. -
    - expect: Mouse hovers over button
    - expect: Button hover styling is applied
  3. -
    - expect: Mouse leaves button
    - expect: Button returns to normal styling

#### 3.8. Button focus state styling

**File:** `tests/button/states.spec.ts`

**Steps:**
  1. -
    - expect: Button receives focus (via tab or programmatic)
    - expect: Button focus styling is applied (e.g., outline or highlight)
  2. -
    - expect: Button focus is visible to indicate keyboard navigation
    - expect: Accessibility improvements are evident

#### 3.9. Button active/pressed state styling

**File:** `tests/button/states.spec.ts`

**Steps:**
  1. -
    - expect: Button is clicked and held
    - expect: Button displays active/pressed styling
  2. -
    - expect: Mouse is released
    - expect: Button returns to normal styling

### 4. Icon Support & Positioning

**Seed:** `seed.spec.ts`

#### 4.1. Render button with icon using IconCss

**File:** `tests/button/icons.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with IconCss='e-icons e-save'
    - expect: Icon is displayed in the button
  2. -
    - expect: Icon element has correct CSS classes applied
    - expect: Icon is visible and properly styled

#### 4.2. Position icon on the left (IconPosition.Left)

**File:** `tests/button/icons.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with IconPosition='Left' (default)
    - expect: Icon appears to the left of content
  2. -
    - expect: Icon and text are properly aligned
    - expect: Layout is visually correct

#### 4.3. Position icon on the right (IconPosition.Right)

**File:** `tests/button/icons.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with IconPosition='Right'
    - expect: Icon appears to the right of content
  2. -
    - expect: Icon and text are properly aligned on the right
    - expect: Layout is visually correct

#### 4.4. Position icon on top (IconPosition.Top)

**File:** `tests/button/icons.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with IconPosition='Top'
    - expect: Icon appears above the content
  2. -
    - expect: Button layout is vertical (icon above text)
    - expect: Icon and text are vertically aligned

#### 4.5. Position icon on bottom (IconPosition.Bottom)

**File:** `tests/button/icons.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with IconPosition='Bottom'
    - expect: Icon appears below the content
  2. -
    - expect: Button layout is vertical (text above icon)
    - expect: Icon and text are vertically aligned

#### 4.6. Icon-only button without content

**File:** `tests/button/icons.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with icon only and no text content
    - expect: e-icon-btn class is applied
  2. -
    - expect: Button appears compact
    - expect: Icon is centered in the button

#### 4.7. Round icon-only button

**File:** `tests/button/icons.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with e-round and e-icon-btn classes
    - expect: Button appears as a circle
  2. -
    - expect: Icon is centered in circular button
    - expect: Button appearance is symmetric

#### 4.8. Icon with semantic color

**File:** `tests/button/icons.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with icon and semantic color (e.g., e-success)
    - expect: Icon color matches button color scheme
  2. -
    - expect: Icon and button color are visually coherent
    - expect: Color semantic is maintained

#### 4.9. Accessibility - icon aria-hidden attribute

**File:** `tests/button/icons.spec.ts`

**Steps:**
  1. -
    - expect: Icon element has aria-hidden='true' attribute
    - expect: Icon is hidden from screen readers
  2. -
    - expect: Screen readers focus on button text content, not icon
    - expect: Accessibility is improved

#### 4.10. Multiple icon classes

**File:** `tests/button/icons.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with multiple icon classes (e.g., 'e-icons e-save')
    - expect: All icon classes are applied
  2. -
    - expect: Icon is rendered correctly with combined classes
    - expect: Icon appearance is as expected

### 5. Event Handling

**Seed:** `seed.spec.ts`

#### 5.1. OnClick event fires on user click

**File:** `tests/button/events.spec.ts`

**Steps:**
  1. -
    - expect: Button has OnClick event handler
    - expect: Button is clickable
  2. -
    - expect: User clicks button
    - expect: OnClick event fires and handler is invoked

#### 5.2. OnClick event receives MouseEventArgs

**File:** `tests/button/events.spec.ts`

**Steps:**
  1. -
    - expect: OnClick handler receives MouseEventArgs parameter
    - expect: Event contains click details (coordinates, buttons, etc.)
  2. -
    - expect: UI updates based on event data
    - expect: Handler can access click event information

#### 5.3. OnClick event fires only for user interaction

**File:** `tests/button/events.spec.ts`

**Steps:**
  1. -
    - expect: User clicks button with mouse
    - expect: OnClick handler is invoked
  2. -
    - expect: User presses Enter key on focused button
    - expect: OnClick handler is invoked for keyboard interaction
  3. -
    - expect: OnClick handler is NOT invoked for programmatic clicks
    - expect: Only user interactions trigger OnClick

#### 5.4. Created event fires after component render

**File:** `tests/button/events.spec.ts`

**Steps:**
  1. -
    - expect: Button component is rendered in DOM
    - expect: Created event fires
  2. -
    - expect: Created handler is invoked after first render
    - expect: Handler receives object parameter

#### 5.5. Created event for multiple buttons

**File:** `tests/button/events.spec.ts`

**Steps:**
  1. -
    - expect: Multiple buttons are rendered on page
    - expect: Created event fires for each button
  2. -
    - expect: Each button's Created handler is invoked independently
    - expect: Multiple creation events are captured

#### 5.6. Multiple OnClick handlers can be attached

**File:** `tests/button/events.spec.ts`

**Steps:**
  1. -
    - expect: Button has multiple OnClick handlers
    - expect: All handlers fire when button is clicked
  2. -
    - expect: Handlers execute in correct order
    - expect: All handler logic executes

#### 5.7. OnClick handler can prevent default behavior

**File:** `tests/button/events.spec.ts`

**Steps:**
  1. -
    - expect: OnClick handler calls preventDefault()
    - expect: Button's default behavior is prevented
  2. -
    - expect: Form submission is prevented (if button in form)
    - expect: Custom behavior takes precedence

#### 5.8. Event handler receives correct event context

**File:** `tests/button/events.spec.ts`

**Steps:**
  1. -
    - expect: Button is clicked
    - expect: OnClick handler receives MouseEventArgs with correct context
  2. -
    - expect: e.clientX and e.clientY are populated
    - expect: Other event properties are accessible

### 6. Accessibility & RTL Support

**Seed:** `seed.spec.ts`

#### 6.1. Enable RTL mode with EnableRtl property

**File:** `tests/button/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with EnableRtl='true'
    - expect: e-rtl class is applied to button element
  2. -
    - expect: Button layout is mirrored for RTL
    - expect: Button icon and text are positioned right-to-left

#### 6.2. RTL with icon positioning

**File:** `tests/button/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with RTL enabled and icon positioned on left
    - expect: Icon appears on right in RTL layout
  2. -
    - expect: Right arrow icon visually appears on the left side
    - expect: RTL layout is correctly reversed

#### 6.3. Button has proper role and ARIA attributes

**File:** `tests/button/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Button element renders as <button> HTML tag
    - expect: Button has implicit role='button'
  2. -
    - expect: aria-disabled attribute matches Disabled property
    - expect: aria-disabled='false' for enabled button

#### 6.4. Disabled button has correct ARIA attributes

**File:** `tests/button/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Disabled button has aria-disabled='true'
    - expect: aria-disabled attribute is set correctly
  2. -
    - expect: disabled='disabled' HTML attribute is set
    - expect: Button is properly marked as disabled

#### 6.5. Toggle button has aria-pressed attribute

**File:** `tests/button/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Toggle button has aria-pressed attribute
    - expect: aria-pressed='false' initially
  2. -
    - expect: aria-pressed='true' when button is active
    - expect: aria-pressed toggles with button state

#### 6.6. Button supports keyboard navigation

**File:** `tests/button/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Button can receive focus via Tab key
    - expect: Button is in tab order
  2. -
    - expect: Focused button can be activated with Enter or Space key
    - expect: Keyboard interaction works

#### 6.7. Button has visible focus indicator

**File:** `tests/button/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Button receives focus via keyboard
    - expect: Visual focus indicator is visible
  2. -
    - expect: Focus indicator meets contrast requirements
    - expect: Focus state is clearly distinguishable

#### 6.8. Button label is properly associated for screen readers

**File:** `tests/button/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Screen reader reads button content as label
    - expect: Button text is accessible to screen readers
  2. -
    - expect: Button purpose is clear from label alone
    - expect: Accessibility is improved

#### 6.9. HTML attributes like title and data-* are supported

**File:** `tests/button/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with HtmlAttributes (e.g., title='Click to save')
    - expect: HTML attributes are applied to button element
  2. -
    - expect: title attribute appears in browser tooltip
    - expect: data-* attributes are accessible

#### 6.10. Button color contrast for accessibility

**File:** `tests/button/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Button text color has sufficient contrast against background
    - expect: WCAG AA contrast standards are met
  2. -
    - expect: Disabled button color contrast is also acceptable
    - expect: Accessibility is maintained in all states

### 7. Edge Cases & Special Scenarios

**Seed:** `seed.spec.ts`

#### 7.1. Rapid clicking on button

**File:** `tests/button/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Button is clicked multiple times rapidly
    - expect: All clicks are registered
  2. -
    - expect: OnClick event fires for each click
    - expect: No clicks are lost or double-fired

#### 7.2. Button with null or undefined content

**File:** `tests/button/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Button Content property is null or undefined
    - expect: Button renders without errors
  2. -
    - expect: Button element exists in DOM
    - expect: No exception is thrown

#### 7.3. Button component is destroyed and recreated

**File:** `tests/button/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Button component is mounted
    - expect: Component renders successfully
  2. -
    - expect: Component is destroyed from DOM
    - expect: Cleanup occurs without errors
  3. -
    - expect: New button component is created
    - expect: Component is re-rendered successfully

#### 7.4. Button with very large icon

**File:** `tests/button/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with oversized icon
    - expect: Button scales appropriately
  2. -
    - expect: Button maintains functional clickable area
    - expect: Icon does not overflow unexpectedly

#### 7.5. Button CssClass property with invalid class names

**File:** `tests/button/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: CssClass property contains invalid/non-existent class
    - expect: Button renders without errors
  2. -
    - expect: Button element receives all CSS classes specified
    - expect: Browser handles non-existent CSS gracefully

#### 7.6. Button with both IsPrimary and e-primary class

**File:** `tests/button/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Button has IsPrimary='true' AND CssClass='e-primary'
    - expect: e-primary class appears only once in HTML
  2. -
    - expect: Primary styling is applied correctly without duplication
    - expect: No CSS conflicts occur

#### 7.7. Button in a form submission

**File:** `tests/button/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Button is placed inside <form> element
    - expect: Button can trigger form submission
  2. -
    - expect: Clicking button submits form
    - expect: Form data is properly sent

#### 7.8. Button nested inside another button

**File:** `tests/button/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Nested button scenario triggers error
    - expect: HTML validation fails (nested buttons invalid)
  2. -
    - expect: Behavior is undefined or handles gracefully
    - expect: Component does not crash

#### 7.9. Button with extremely long icon CSS class string

**File:** `tests/button/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with very long IconCss string
    - expect: No performance degradation
  2. -
    - expect: Icon displays correctly
    - expect: Button remains responsive

#### 7.10. Button parameter changes after render

**File:** `tests/button/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Button component parameters are updated after initial render
    - expect: Component re-renders with new parameters
  2. -
    - expect: CSS classes are recalculated correctly
    - expect: Button appearance updates to reflect changes

### 8. Performance & Rendering

**Seed:** `seed.spec.ts`

#### 8.1. Button renders quickly with minimal DOM operations

**File:** `tests/button/performance.spec.ts`

**Steps:**
  1. -
    - expect: Button component renders on page
    - expect: Render time is within acceptable limits
  2. -
    - expect: Button does not cause layout shifts
    - expect: Rendering is smooth and efficient

#### 8.2. Multiple buttons render without performance impact

**File:** `tests/button/performance.spec.ts`

**Steps:**
  1. -
    - expect: 10+ buttons are added to page
    - expect: All buttons render correctly
  2. -
    - expect: Page performance remains acceptable
    - expect: No noticeable lag or freezing

#### 8.3. CSS class composition uses StringBuilder optimization

**File:** `tests/button/performance.spec.ts`

**Steps:**
  1. -
    - expect: Button CSS classes are composed during render
    - expect: StringBuilder is used for efficiency
  2. -
    - expect: Memory allocation is minimized
    - expect: CSS class string is built efficiently

#### 8.4. Button parameters update efficiently

**File:** `tests/button/performance.spec.ts`

**Steps:**
  1. -
    - expect: Button parameters (Content, CssClass, etc.) are changed
    - expect: Component re-renders efficiently
  2. -
    - expect: Only affected DOM elements are updated
    - expect: No unnecessary re-renders occur

#### 8.5. Button does not cause memory leaks

**File:** `tests/button/performance.spec.ts`

**Steps:**
  1. -
    - expect: Button component is created and destroyed multiple times
    - expect: Memory is released properly
  2. -
    - expect: No memory accumulation after each cycle
    - expect: Garbage collection works as expected

### 9. Integration & Combinations

**Seed:** `seed.spec.ts`

#### 9.1. Primary button with icon and semantic color

**File:** `tests/button/integration.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with IsPrimary='true', IconCss set, and e-success class
    - expect: All properties combine correctly
  2. -
    - expect: Button displays primary styling with icon and success color
    - expect: Complex combination works as expected

#### 9.2. Toggle button with outline style

**File:** `tests/button/integration.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with IsToggle='true' and e-outline class
    - expect: Button is both toggleable and outlined
  2. -
    - expect: Button toggles between active and inactive outlined states
    - expect: Styles persist through toggle

#### 9.3. Disabled toggle button

**File:** `tests/button/integration.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with Disabled='true' and IsToggle='true'
    - expect: Button is both disabled and toggleable
  2. -
    - expect: Button cannot be toggled because it is disabled
    - expect: Disabled state takes precedence

#### 9.4. Round icon button with danger color

**File:** `tests/button/integration.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with e-round, e-icon-btn, and e-danger classes
    - expect: Button is round and red
  2. -
    - expect: Button appears as a circle with icon
    - expect: Danger intent is clear

#### 9.5. RTL button with right-positioned icon

**File:** `tests/button/integration.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with EnableRtl='true' and IconPosition='Right'
    - expect: RTL and icon positioning combine
  2. -
    - expect: Button layout is mirrored and icon appears correctly in RTL context
    - expect: Both features work together

#### 9.6. Flat button with custom HTML attributes

**File:** `tests/button/integration.spec.ts`

**Steps:**
  1. -
    - expect: Button renders with e-flat class and HtmlAttributes (title, data-*, etc.)
    - expect: Custom attributes are applied
  2. -
    - expect: Button has both flat styling and custom HTML attributes
    - expect: Attributes are accessible
