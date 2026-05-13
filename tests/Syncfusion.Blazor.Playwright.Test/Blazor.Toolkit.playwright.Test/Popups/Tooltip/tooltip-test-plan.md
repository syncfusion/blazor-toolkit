```markdown
# Tooltip Component Test Plan

This document defines Playwright test coverage for the `SfTooltip` component. It follows the structure and conventions used by the Dialog test plan, and maps tests to Playwright spec files used in this repository.

## IMPORTANT: Supported vs Unsupported Features

### ✅ SUPPORTED Properties
- **Content / ContentTemplate** — string or RenderFragment content and templates
- **OpensOn** — open triggers (Auto, Hover, Click, Focus, Custom)
- **Position** — precise placements (TopCenter, TopLeft, TopRight, BottomLeft, BottomCenter, BottomRight, LeftTop, LeftCenter, LeftBottom, RightTop, RightCenter, RightBottom)
- **OffsetX / OffsetY** — positional offsets
- **WindowCollision** — boolean collision handling (flip/fit behavior)
- **IsSticky** — remains open until closed programmatically/user action
- **MouseTrail** — tooltip follows mouse pointer when enabled
- **ShowTipPointer / TipPointerPosition** — arrow pointer visibility and placement
- **Container / Target** — attach to container or specific target(s)
- **Width / Height** — explicit sizing
- **HtmlAttributes** — preserve arbitrary attributes on tooltip DOM
- **Animation (Open/Close effects)** — open/close animation control

### ✅ SUPPORTED Events / Lifecycle
- **BeforeRender / BeforeOpen / BeforeCollision / BeforeClose** — cancellable pre-events
- **Opened / Closed** — post events
- **ContentUpdated / CreateTooltipAsync** — dynamic content lifecycle

### ❌ NOT SUPPORTED / OUT OF SCOPE
- Server-only DOM operations (tooltips are JS-heavy and require a running browser)
- Native mobile-specific gestures beyond hover/click/tap-hold (limited coverage possible with Playwright mobile emulation)

## Test Strategy & Conventions

- Group tests by feature: content, triggers, positioning, collision/offset, pointer, container/target, accessibility, animation, and edge cases.
- Prefer `locator` and `hover()` as the primary reveal method; fall back to `focus()` / `click()` for `OpensOn` variations and flaky environments.
- Avoid brittle pixel assertions; verify presence, positive size, and expected text/content instead of exact coordinates.
- Use dual-selector detection for tooltip DOM: prefer `body .e-tooltip-wrap` (toolkit DOM) and fall back to `[role="tooltip"]` for ARIA checks.
- Use reasonable timeouts (10–15s) for Blazor render/interop; add screenshots and HTML snippets on failure to aid debugging.

## File mapping (Playwright specs)

- `content.spec.ts` — basic content types: `Content`, `ContentTemplate`, RenderFragment, MarkupString, title-attribute fallback
- `behavior.spec.ts` — size/dimensions, target wiring, open modes (Hover/Click/Focus/Custom) and masking of brittle checks
- `modal.spec.ts` — markup rendering, interactive template tests (click handlers inside templates)
- `usecases.spec.ts` — positions, offset/collision, sticky, mouseTrail, container target, hide pointer, HtmlAttributes

## Test Scenarios

### 1. Content Rendering

1.1 Simple text content
- Expect tooltip displays `Content` string when target hovered.

1.2 Title attribute fallback
- If target has `title` and `Content` not set, tooltip uses `title` and removes `title` to avoid duplicate native tooltip.

1.3 ContentTemplate (HTML template)
- Template markup renders correctly inside tooltip; images/links present and accessible.

1.4 RenderFragment / Dynamic content
- RenderFragment content renders on demand and `ContentUpdated` flows produce updated tooltip text.

1.5 MarkupString (HTML inside tooltip)
- HTML content is rendered as HTML, not escaped; links and inner elements exist and are clickable if intended.

### 2. Triggers & Open Modes

2.1 Hover (default / Auto)
- On hover over target, tooltip appears and sets `aria-describedby` on the target.

2.2 Click
- For `OpensOn="Click"`, tooltip opens on click and respects `IsSticky` when set.

2.3 Focus
- `OpensOn="Focus"` opens on focus; keyboard navigation should reveal tooltip.

2.4 Custom / Programmatic
- When `OpensOn="Custom"`, tooltip does not open automatically; `showTooltip`/`hideTooltip` JS interop works.

### 3. Positioning & Tip Pointer

3.1 Position enumerations
- Verify tooltips display for `TopCenter`, `TopLeft`, `TopRight`, `BottomLeft`, `BottomCenter`, `BottomRight`, `LeftTop`, `LeftCenter`, `LeftBottom`, `RightTop`, `RightCenter`, `RightBottom`.
- Tests should assert tooltip is visible and that the arrow pointer class (e.g., `.e-tip-top`, `.e-tip-left`) corresponds to the requested placement when `ShowTipPointer` is true.

3.2 Tip Pointer hide/show and `TipPointerPosition`
- When `ShowTipPointer=false`, arrow elements are absent.
- `TipPointerPosition` variants should affect arrow alignment classes (Start/Middle/End).

### 4. Offset & Window Collision

4.1 OffsetX/OffsetY
- Verify tooltip position offset by configured values (use relative checks: position shifted, not absolute pixel matching).

4.2 WindowCollision
- When `WindowCollision=true`, tooltip flips/fits to remain in viewport; simulate near-edge target and assert tooltip appears and still visible.

### 5. MouseTrail & Sticky

5.1 MouseTrail
- When `MouseTrail=true`, moving the mouse updates tooltip position; assert tooltip remains visible while moving and updates coordinates indirectly (positive size).

5.2 Sticky
- When `IsSticky=true` and `OpensOn=Click`, tooltip stays open after mouse leave; closing via `hideTooltip` or clicking the close icon or target again hides it.

### 6. Container & Target Variations

6.1 Multi-target selectors
- Targets using CSS selectors with multiple matches should open tooltip for each matched element; ensure `data-tooltip-id` is applied per target.

6.2 Container attachment
- When `Container` is set to a non-body element, tooltip is appended into that container and its position respects container scroll.

### 7. Sizing, HtmlAttributes, and Styling

7.1 Width/Height
- Tooltip respects explicit `Width`/`Height` and `max-width` behavior; validate positive bounding box and CSS attributes.

7.2 HtmlAttributes
- Arbitrary attributes (data-*, title, role) are copied to the tooltip wrapper.

### 8. Animation

8.1 Open/Close effects
- Verify open/close animation properties do not prevent content from being visible to tests (use shorter timeouts or wait for final visible state).

### 9. Accessibility

9.1 ARIA attributes
- Tooltip sets `aria-describedby` on the target and `role="tooltip"` on tooltip wrapper.

9.2 Keyboard
- Focus-triggered tooltips open on keyboard focus; Escape closes where applicable.

### 10. Events & Lifecycle

10.1 BeforeRender/BeforeOpen/BeforeClose
- Events can cancel open/close; tests should assert that cancelation prevents the expected visual state change.

10.2 Opened/Closed
- Events fire after visual changes; assert order when possible (BeforeOpen → Opened, BeforeClose → Closed).

10.3 ContentUpdated
- Dynamic content updates propagate and tooltip updates without a full recreate.

### 11. Edge Cases & Flakiness Mitigation

- Tooltips are animation/interop dependent — prefer hover+focus+click fallbacks and increase timeouts for Blazor rendering.
- Capture screenshots and page HTML snippets on failures to speed triage.
- Avoid relying on exact coordinates or pixel-perfect placements in CI; prefer presence, size>0, and CSS class checks.

## Test Files & Quick Run

- Primary spec files: `content.spec.ts`, `behavior.spec.ts`, `modal.spec.ts`, `usecases.spec.ts` (see the `Popups/Tooltip` folder in tests)
- Run locally against the samples app URL: `http://localhost:5000/tooltip-all-samples`

Playwright run example:
```bash
# from repository root
npm run build # if needed to produce samples app
dotnet watch run --project samples/Blazor.Toolkit.Samples/Blazor.Toolkit.Samples.csproj
npx playwright test tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Popups/Tooltip --headed
```

## Reporting & Triage Guidance

- On failure, collect `playwright-failure-*.png` screenshots and top HTML snippet saved by tests.
- Check for missing CSS/JS assets (fluent.min.css and tooltip.js) in sample app; missing assets are a common cause of invisibility.
- If tests fail intermittently, increase timeouts or add explicit `locator.scrollIntoViewIfNeeded()` / `locator.focus()` fallback.

---

```
