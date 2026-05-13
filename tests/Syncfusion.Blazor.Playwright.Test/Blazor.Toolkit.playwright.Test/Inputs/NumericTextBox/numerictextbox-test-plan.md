# NumericTextBox Component Test Plan

This document maps the NumericTextBox test plan scenarios to the existing focused Playwright specs and the remaining unique tests in `numerictextbox.comprehensive.spec.ts`.

**Test Execution**

```bash
# Run all NumericTextBox tests
npm test NumericTextBox

# Run a specific spec file
npm test -- tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/render-and-format.spec.ts

# Run headed
npm test -- --headed
```

**Base URL**: Tests assume the sample app is running (e.g. `http://localhost:5000`) and NumericTextBox samples are at `/inputs/numerictextbox`.

**Key DOM Selectors**

- Container: `.e-numerictextbox` (first instance for sample)
- Input element: `.e-input` inside container
- Spin up button: `.e-spin-up` or button with `.e-spin-up` icon
- Spin down button: `.e-spin-down` or button with `.e-spin-down` icon
- Clear button (when configured): `.e-clear-icon` or `.e-clear` element

**Test Scenario Mapping (17 scenarios)**

1. Rendering, basic DOM & formatting
   - Covered by: [render-and-format.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/render-and-format.spec.ts)

2. Min / Max / Step behavior
   - Covered by: [min-max-step.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/min-max-step.spec.ts)

3. Keyboard interactions (Arrow keys, Home/End)
   - Covered by: [events-and-binding.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/events-and-binding.spec.ts)

4. Spin buttons, click and long-press auto-increment
   - Covered by: [edgecases-and-performance.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/edgecases-and-performance.spec.ts)
   - Long-press specifics: retained in `numerictextbox.comprehensive.spec.ts` (unique long-press automation)

5. Mouse wheel behavior and step changes
   - Covered by: [edgecases-and-performance.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/edgecases-and-performance.spec.ts)
   - Mouse-wheel granular checks: retained in `numerictextbox.comprehensive.spec.ts` (unique scenarios)

6. Clipboard paste & input validation
   - Covered by: [clipboard-and-validation.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/clipboard-and-validation.spec.ts)

7. Number formatting, culture & currency display
   - Covered by: [render-and-format.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/render-and-format.spec.ts)
   - Additional i18n smoke test: retained in `numerictextbox.comprehensive.spec.ts`

8. Decimal precision and rounding rules
   - Unique tests kept in: `numerictextbox.comprehensive.spec.ts`

9. Event callbacks and two-way binding
   - Covered by: [events-and-binding.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/events-and-binding.spec.ts)

10. Disabled / Read-only / Focus states
    - Covered by: [accessibility-and-states.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/accessibility-and-states.spec.ts)

11. Accessibility (ARIA, keyboard focus, screen reader hooks)
    - Covered by: [accessibility-and-states.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/accessibility-and-states.spec.ts)

12. Clear button behavior (when present)
    - Unique/edge checks kept in: `numerictextbox.comprehensive.spec.ts`

13. Large numbers, magnitudes and performance under load
    - Covered by: [edgecases-and-performance.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/edgecases-and-performance.spec.ts)

14. NaN / invalid input handling and fallback behavior
    - Covered by: [clipboard-and-validation.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/clipboard-and-validation.spec.ts)

15. Programmatic API calls (set/get, focus methods)
    - Covered by: [events-and-binding.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/events-and-binding.spec.ts)

16. RTL and localization layout checks
    - Covered by: [render-and-format.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/render-and-format.spec.ts)
    - Additional RTL smoke in `numerictextbox.comprehensive.spec.ts`

17. Edge cases: step fractional, negative step, boundary clamps
    - Covered by: [min-max-step.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/min-max-step.spec.ts)
    - A few fractional-step precision tests are kept in `numerictextbox.comprehensive.spec.ts` (unique)

**Coverage Summary**

| Scenario Category | Tests / File | Notes |
|---|---|---|
| Rendering & Format | [render-and-format.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/render-and-format.spec.ts) | Formats, placeholders, CSS classes |
| Min/Max/Step | [min-max-step.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/min-max-step.spec.ts) | Boundary clamping, step behavior |
| Events & Binding | [events-and-binding.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/events-and-binding.spec.ts) | Focus, change events, API methods |
| Clipboard & Validation | [clipboard-and-validation.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/clipboard-and-validation.spec.ts) | Paste, NaN, filtering |
| Accessibility & States | [accessibility-and-states.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/accessibility-and-states.spec.ts) | ARIA, disabled, keyboard |
| Edgecases & Performance | [edgecases-and-performance.spec.ts](tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Inputs/NumericTextBox/edgecases-and-performance.spec.ts) | Mouse wheel, long-press, perf |
| Comprehensive (unique) | numerictextbox.comprehensive.spec.ts | Decimal precision, mouse-wheel granularity, long-press spin, clear button, i18n smoke |

**Author Notes**

- Focused specs should remain the canonical place for the scenario category they cover. The comprehensive file should only contain residual, unique tests.
- When checking element properties in TypeScript, cast DOM nodes in `evaluate` callbacks (e.g. `(el: any) => el.disabled`) to avoid union-type errors.
- Replace fragile NaN checks with `Number.isFinite(Number(value))` style assertions.

Next steps available on request:
- Create a PR adding this test-plan and linking it from the NumericTextBox folder README
- Run a duplication scan across spec titles and report remaining overlaps

---
Generated as a concise test-plan mapping the 17 plan scenarios to repository test files.
