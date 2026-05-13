```markdown
# Dialog Component Test Plan

## IMPORTANT: Supported vs Unsupported Features

### ✅ SUPPORTED Properties
- **IsModal** - Render modal (with overlay) or modeless dialog
- **Target** - CSS selector or element to attach dialog to (defaults to document.body)
- **Visible** - Show or hide dialog (two-way binding via VisibleChanged)
- **AllowDragging** - Enable dragging via header
- **EnableResize** - Enable resize handles and resizing behavior
- **ResizeHandles** - Configure resize directions
- **AllowPrerender** - Keep DOM present when not visible
- **Header** - Header text or template
- **Content** - String content alternative to `ChildContent`
- **FooterTemplate** - Footer RenderFragment or string for dialog actions
- **CloseOnEscape** - Close dialog on Escape key
- **ZIndex** - Base z-index for dialog and overlay
- **HtmlAttributes** - Preserve unmatched HTML attributes (title, data-*, etc.)

### ✅ SUPPORTED Events
- **OnOpen / Opened** - Fired before/after open (BeforeOpenEventArgs supports Cancel)
- **OnClose / Closed** - Fired before/after close (BeforeCloseEventArgs supports Cancel)
- **OnDragStart / OnDrag / OnDragStop** - When dragging enabled
- **OnResizeStart / Resizing / OnResizeStop** - When resizing enabled
- **OnOverlayModalClick** - Click on modal overlay/backdrop
- **Created / Destroyed** - Component lifecycle

### ❌ NOT SUPPORTED
- **Server-only DOM operations** - DOM-heavy operations rely on JS interop and are not SSR-native
- **Implicit focus management when PreventFocus=true** - Focus restore/prevent focus has edge behaviors that must be validated in app context

### 🔄 Updated Test Notes
- `Visible` supports two-way binding via `VisibleChanged`; tests should assert both DOM visibility and raised events.
- Modal dialogs include overlay; overlay z-index should be `ZIndex - 1` and receive overlay click events.

## Application Overview

The `SfDialog` component displays modal/modeless dialogs with header, content, and footer sections. It supports dragging, resizing, RTL, focus trapping, overlay handling, and lifecycle events. Tests cover rendering, interaction, accessibility, keyboard behavior, events, edge cases, and performance.

## Test Scenarios

### 1. DOM Structure & Rendering

**Seed:** `seed.spec.ts`

#### 1.1. Render non-modal dialog (default)

**File:** `tests/dialog/dom.spec.ts`

**Steps:**
  1. -
    - expect: Dialog renders with root container `e-dialog`
    - expect: No overlay element exists for non-modal dialog
  2. -
    - expect: Header, content, and footer sections present when provided
    - expect: `role="dialog"` attribute is set on root element

#### 1.2. Render modal dialog with overlay

**File:** `tests/dialog/dom.spec.ts`

**Steps:**
  1. -
    - expect: Dialog renders inside `e-dlg-container` when `IsModal='true'`
    - expect: An overlay element with class `e-dlg-overlay` exists
  2. -
    - expect: Overlay has z-index of `ZIndex - 1`
    - expect: Dialog root has `aria-modal='true'`

#### 1.3. Target property rendering

**File:** `tests/dialog/dom.spec.ts`

**Steps:**
  1. -
    - expect: Dialog renders within specified `Target` selector
    - expect: When `Target` is invalid, dialog falls back to `document.body`

### 2. Header, Content & Footer

**Seed:** `seed.spec.ts`

#### 2.1. Header content and template

**File:** `tests/dialog/content.spec.ts`

**Steps:**
  1. -
    - expect: `Header` string renders inside header area
    - expect: Header template (RenderFragment) renders custom content
  2. -
    - expect: Close icon exists when configured and has appropriate aria-label

#### 2.2. Body content rendering (Content vs ChildContent)

**File:** `tests/dialog/content.spec.ts`

**Steps:**
  1. -
    - expect: `Content` string displays inside `e-dlg-content`
    - expect: `ChildContent` RenderFragment displays complex markup correctly
  2. -
    - expect: Long content scrolls inside dialog content area (no overflow outside root)

#### 2.3. Footer / Buttons rendering

**File:** `tests/dialog/content.spec.ts`

**Steps:**
  1. -
    - expect: `FooterTemplate` renders action buttons or custom footer
    - expect: Buttons receive click events and are keyboard accessible

### 3. Visibility, Show/Hide & Events

**Seed:** `seed.spec.ts`

#### 3.1. Visible toggle and events

**File:** `tests/dialog/events.spec.ts`

**Steps:**
  1. -
    - expect: Setting `Visible='true'` shows dialog in DOM
    - expect: `OnOpen` / `Opened` events fire in order (BeforeOpen then Opened)
  2. -
    - expect: Setting `Visible='false'` hides dialog
    - expect: `OnClose` / `Closed` events fire in order (BeforeClose then Closed)

#### 3.2. Cancelable open/close

**File:** `tests/dialog/events.spec.ts`

**Steps:**
  1. -
    - expect: `BeforeOpenEventArgs.Cancel=true` prevents dialog from opening
    - expect: `BeforeCloseEventArgs.Cancel=true` prevents dialog from closing

#### 3.3. Created / Destroyed lifecycle

**File:** `tests/dialog/events.spec.ts`

**Steps:**
  1. -
    - expect: `Created` fires after initial render
    - expect: `Destroyed` fires when component is removed

### 4. Dragging & Resizing

**Seed:** `seed.spec.ts`

#### 4.1. AllowDragging behavior

**File:** `tests/dialog/interaction.spec.ts`

**Steps:**
  1. -
    - expect: With `AllowDragging='true'`, header can be used to drag dialog
    - expect: `OnDragStart`, `OnDrag`, `OnDragStop` events fire with position data

#### 4.2. EnableResize behavior

**File:** `tests/dialog/interaction.spec.ts`

**Steps:**
  1. -
    - expect: With `EnableResize='true'` resize handles appear when hovered
    - expect: `OnResizeStart`, `Resizing`, `OnResizeStop` events fire during resize

#### 4.3. ResizeHandles directions and RTL swap

**File:** `tests/dialog/interaction.spec.ts`

**Steps:**
  1. -
    - expect: Resizing respects configured `ResizeHandles`
    - expect: In RTL mode, directional handles are mirrored appropriately

### 5. Keyboard & Accessibility

**Seed:** `seed.spec.ts`

#### 5.1. Escape key closes when `CloseOnEscape` true

**File:** `tests/dialog/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Pressing Escape closes modal when `CloseOnEscape='true'`
    - expect: `OnClose` events fire after Escape

#### 5.2. Focus trapping and restore

**File:** `tests/dialog/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: When modal, Tab cycles focus inside dialog (focus trap)
    - expect: On close, focus is restored to the previously focused element unless PreventFocus is set

#### 5.3. ARIA attributes and roles

**File:** `tests/dialog/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Dialog root has `role='dialog'` and `aria-labelledby` or `aria-describedby` referencing header/content
    - expect: `aria-modal='true'` present for modal dialogs

#### 5.4. Overlay semantics

**File:** `tests/dialog/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: Overlay has appropriate presentation role and is keyboard inert
    - expect: Clicking overlay triggers `OnOverlayModalClick` when enabled

### 6. RTL Support & Styling

**Seed:** `seed.spec.ts`

#### 6.1. Enable RTL layout

**File:** `tests/dialog/accessibility.spec.ts`

**Steps:**
  1. -
    - expect: `e-rtl` class is applied when RTL enabled
    - expect: Header/content alignment is mirrored

#### 6.2. ZIndex and stacking

**File:** `tests/dialog/styling.spec.ts`

**Steps:**
  1. -
    - expect: Dialog z-index matches `ZIndex` property
    - expect: For modal, overlay z-index is `ZIndex - 1` and dialog overlays content

### 7. Edge Cases & Special Scenarios

**Seed:** `seed.spec.ts`

#### 7.1. AllowPrerender keeps DOM when hidden

**File:** `tests/dialog/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: With `AllowPrerender='true'`, DOM remains when `Visible='false'`
    - expect: No flicker or re-creation on subsequent show

#### 7.2. Rapid show/hide cycles

**File:** `tests/dialog/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Repeated toggles of `Visible` do not leak DOM nodes or events
    - expect: Event order remains consistent for open/close sequences

#### 7.3. Invalid `Target` selector

**File:** `tests/dialog/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Invalid `Target` gracefully falls back to `document.body`
    - expect: No console errors are thrown

#### 7.4. Dialog destroyed and recreated

**File:** `tests/dialog/edge-cases.spec.ts`

**Steps:**
  1. -
    - expect: Component destroyed without JS errors
    - expect: Recreated component fires `Created` and behaves normally

### 8. Performance & Rendering

**Seed:** `seed.spec.ts`

#### 8.1. Render performance for many dialogs

**File:** `tests/dialog/performance.spec.ts`

**Steps:**
  1. -
    - expect: Rendering multiple non-modal dialogs (10+) does not cause significant jank
    - expect: Modal overlay stacking remains correct

#### 8.2. Drag/resize performance

**File:** `tests/dialog/performance.spec.ts`

**Steps:**
  1. -
    - expect: Dragging and resizing are smooth under reasonable load
    - expect: No memory leaks after repeated drag/resize cycles

### 9. Integration & Combinations

**Seed:** `seed.spec.ts`

#### 9.1. Modal with custom footer and focusable controls

**File:** `tests/dialog/integration.spec.ts`

**Steps:**
  1. -
    - expect: Modal with form controls inside traps focus and returns values on submit
    - expect: Footer buttons trigger expected actions and close dialog when appropriate

#### 9.2. Dragging + Resizing + RTL

**File:** `tests/dialog/integration.spec.ts`

**Steps:**
  1. -
    - expect: Dragging and resizing work while RTL layout is enabled
    - expect: Events and positioning remain correct when combined

#### 9.3. Disabled interactions when overlay or modal blocks input

**File:** `tests/dialog/integration.spec.ts`

**Steps:**
  1. -
    - expect: Modal overlay prevents interacting with elements behind it
    - expect: Only dialog elements are tabbable/focusable while modal

---

``` 
