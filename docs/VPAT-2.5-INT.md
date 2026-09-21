# Voluntary Product Accessibility Template (VPAT) 2.5 — International Edition

**Product:** Syncfusion® Toolkit for Blazor — UI Components
**Product Version:** 1.0.0
**Report Date:** 2025
**Report Based On:** EN 301 549 v3.2.1 (European accessibility standard) — incorporates WCAG 2.1 Level A and AA, and selected EN 301 549 specific requirements.
**Evaluation Methods:** Static code review of component Razor markup (`.razor`), component logic (`.cs`), CSS isolation stylesheets, and JavaScript interop files under `src/wwwroot/scripts/`; skill documentation review under `.github/skills/`; no end-user runtime testing was performed.
**Notes:** Terms defined in the Notes section are referenced in this report as linked footnotes.

---

## Table of Contents

1. [About this Report](#about-this-report)
2. [Notes](#notes)
3. [Executive Summary](#executive-summary)
4. [Product Description](#product-description)
5. [Terms Used in this Document](#terms-used-in-this-document)
6. [Instructions](#instructions)
7. [Detailed Conformance Evaluation](#detailed-conformance-evaluation)
   - 7.1 Success Criteria, Level A
   - 7.2 Success Criteria, Level AA
   - 7.3 EN 301 549 Specific Requirements
   - 7.4 Remarks and Explanations
8. [Appendix A: Component-by-Component Accessibility Matrix](#appendix-a-component-by-component-accessibility-matrix)
9. [Appendix B: Evidence Index](#appendix-b-evidence-index)

---

## About this Report

This VPAT 2.5 International (INT) edition documents the degree of conformance of the **Syncfusion® Toolkit for Blazor** component library to EN 301 549 v3.2.1. EN 301 549 is the European harmonized standard for the accessibility requirements of ICT products and services and references WCAG 2.1 at Levels A and AA, plus additional requirements covering functional performance, documentation, support services, and specific non-web technologies.

This report covers the **component library source** shipped in this repository (`src/`), comprising 16 public components across six categories: Buttons, Calendars, Charts, Inputs, Popups (Dialog/Tooltip), and Notifications (Spinner). It does **not** evaluate the sample applications (`samples/`), the test suites (`tests/`), the generated theme CSS (`src/wwwroot/styles/`), or any host application built *with* the toolkit — those are the responsibility of the consuming application.

Because this is a **developer library** (not a finished end-user web product), several EN 301 549 clauses apply only when the components are *incorporated* into a running application. Where a requirement is only partially achievable at the library level (for example, focus management around an open overlay depends on the host page's focus order), the report records "Partially Supports" with an explanation and lists the residual responsibility of the integrator.

---

## Notes

- **"Supports"** — The product fully meets the requirement. Any deviation is below the threshold of WCAG/EN conformance and does not affect end users.
- **"Partially Supports"** — The product meets most of the requirement, but one or more aspects of the requirement are not fully met. See the adjacent Remarks column for specifics.
- **"Does Not Support"** — The product does not meet the requirement. The product may still provide an alternative means of access to the information or function; see Remarks.
- **"Not Applicable"** — The requirement does not apply to the product in the context being evaluated.
- **"Not Evaluated"** — The product has not been evaluated against the requirement. This may be used for criteria that are out of scope of the library layer.
- **Library vs. application responsibility** — A UI component library can only guarantee the markup, ARIA semantics, keyboard handlers, and focus contracts it emits. Items such as final color contrast of a *theme applied by the consumer*, host page tab order, page-level landmarks, and skip-navigation links remain the consuming application's responsibility and are noted as such.

---

## Executive Summary

| Conformance Level | Result |
|---|---|
| WCAG 2.1 Level A | **Partially Supports** |
| WCAG 2.1 Level AA | **Partially Supports** |
| EN 301 549 Specific Requirements (Functional Performance, Documentation, Support) | **Partially Supports** |

### Headline Strengths

1. **Native-HTML-first architecture.** Button (`<button>`), CheckBox (`<input type="checkbox">`), RadioButton (`<input type="radio">`), TextBox (`<input>`), TextArea (`<textarea>`) use native form controls, inheriting their built-in keyboard, focus, and AT semantics.
2. **Comprehensive ARIA combobox pattern** for `SfDatePicker`, `SfDateTimePicker`, and `SfTimePicker` — `role="combobox"`, `aria-expanded`, `aria-owns`, `aria-controls`, `aria-activedescendant`, `aria-autocomplete`, and `role="listbox"`/`role="option"` for time lists.
3. **Full keyboard grid model for calendars** — Arrow, Home, End, PageUp/Down, Shift+PageUp/Down, Ctrl+Home/End, Ctrl+↑/↓, Enter, Escape, Alt+↓/↑, and Tab are all explicitly mapped.
4. **Chart keyboard navigation** — `Alt+J` to enter the chart, Tab between interactive elements, arrows to traverse data points, Enter/Space to activate, `Ctrl +/-` to zoom, `R` to reset, `Ctrl+P` to print.
5. **Dialog focus management** — focus trap while open, focus restoration to trigger on close, Escape to dismiss, `role="dialog"`, `aria-modal`, and `aria-labelledby`/`aria-describedby`.
6. **`prefers-reduced-motion` and `forced-colors` (Windows High Contrast) support** in Spinner, Button, Calendar, and TimePicker CSS.
7. **Spinner `role="status"` + `aria-live="polite"` + `aria-busy`** for accessible loading announcements.
8. **NumericTextBox dedicated `aria-live="polite"` region** for value-change announcements.
9. **Switch `role="switch"` with a resolved accessible-name priority chain** that fixes the NVDA double-announce anti-pattern.
10. **Tooltip `aria-describedby` on the trigger** so screen readers announce tooltip content on focus.

### Headline Gaps

1. **SfButton icon-only buttons ship with no accessible name by default** (only a DEBUG-only warning). Consumers must supply `aria-label` via `HtmlAttributes`. (WCAG 1.3.1, 4.1.2)
2. **SfButtonGroup selection modes lack a `:focus-visible` style** on the styled `<label>` — keyboard focus may not be visibly apparent. (WCAG 2.4.7)
3. **SfButtonGroup `SelectionMode.Single` does not implement arrow-key navigation** between radio items (WAI-ARIA APG radio-group pattern). (WCAG 2.1.1)
4. **SfSpinner may double-announce** because both `aria-label` on the root and visible `Label` text inside the root are rendered simultaneously. (WCAG 1.3.1)
5. **SfTimePicker overwrites the user-supplied `AriaLabel`** with the hardcoded string `"timepicker"` and does **not** set `aria-live` on the input. (WCAG 1.3.1, 4.1.2)
6. **Calendar popups (DatePicker / DateTimePicker / TimePicker) have no focus trap** — Tab can leave the popup naturally. (WCAG 2.1.2)
7. **`aria-modal` on calendar popups is set only in device mode**, not on desktop. (WCAG 2.1.2)
8. **Time-list `<li>` options lack `aria-selected`** in TimePicker and DateTimePicker. (WCAG 4.1.2)
9. **SfDialog never emits `role="alertdialog"`** for single-button modals despite the skill documenting it should. (WCAG 4.1.2)
10. **SfTooltip `aria-hidden` is not toggled** on the tooltip content when hidden, and the hover-only `OpensOn="Hover"` mode is keyboard-inaccessible by design. (WCAG 2.1.1)
11. **SfChart lacks `aria-describedby` for data points** (documented in the skill but not implemented), and provides no `sr-only` data summary. (WCAG 1.3.1)
12. **SfUploader `role="button"` drop zone does not handle Space/Enter**, and the component has no EditContext validation integration. (WCAG 2.1.1)
13. **SfCheckBox has an unimplemented live-region TODO** for state-change announcements. (WCAG 4.1.3)
14. **Selection components (CheckBox, RadioButton, Switch, ButtonGroup) do not wire `aria-invalid`/`aria-describedby` to EditContext validation errors** automatically. (WCAG 3.3.1, 3.3.3)
15. **Localized strings hardcoded in English** — `SfButtonGroup.aria-label` ("Button group with single selection mode"), `SfSpinner` default label ("Loading"), `SfTimePicker` aria-label ("timepicker"). (WCAG 3.1.2)

---

## Product Description

The Syncfusion® Toolkit for Blazor is an open-source (MIT) collection of lightweight Blazor UI components targeting .NET 8, .NET 9, and .NET 10 across Blazor Server, WebAssembly, and Auto render modes. The library is distributed as the NuGet package `Syncfusion.Blazor.Toolkit`.

### Components Covered by this Report

| # | Category | Component | Render Mode Support |
|---|---|---|---|
| 1 | Data Viz | `SfChart` | Interactive (Server / WebAssembly / Auto) |
| 2 | Buttons | `SfButton` | Static SSR, Server, WebAssembly, Auto |
| 3 | Buttons | `SfButtonGroup` (+ inner `Button`) | Static SSR, Server, WebAssembly, Auto |
| 4 | Buttons | `SfCheckBox` | Static SSR, Server, WebAssembly, Auto |
| 5 | Buttons | `SfRadioButton` | Static SSR, Server, WebAssembly, Auto |
| 6 | Buttons | `SfSwitch` | Static SSR, Server, WebAssembly, Auto |
| 7 | Calendars | `SfCalendar` | Server, WebAssembly, Auto |
| 8 | Calendars | `SfDatePicker` | Server, WebAssembly, Auto |
| 9 | Calendars | `SfDateTimePicker` | Server, WebAssembly, Auto |
| 10 | Calendars | `SfTimePicker` | Server, WebAssembly, Auto |
| 11 | Inputs | `SfUploader` | Server, WebAssembly, Auto |
| 12 | Inputs | `SfNumericTextBox` | Static SSR, Server, WebAssembly, Auto |
| 13 | Inputs | `SfTextArea` | Static SSR, Server, WebAssembly, Auto |
| 14 | Inputs | `SfTextBox` | Static SSR, Server, WebAssembly, Auto |
| 15 | Popups | `SfDialog` | Server, WebAssembly, Auto |
| 16 | Popups | `SfTooltip` | Server, WebAssembly, Auto |
| 17 | Notification | `SfSpinner` | Server, WebAssembly, Auto |

### Out of Scope

- **Sample applications** under `samples/` — demo code, not the shipped library.
- **Test projects** under `tests/`.
- **Generated theme CSS** under `src/wwwroot/styles/*.scss` → the final color palette is theme-dependent and chosen by the consumer; color-contrast (WCAG 1.4.3) is therefore evaluated at the default-theme level only.
- **Host page integration** — page landmarks, skip links, document language, and overall tab order are the consuming application's responsibility.

---

## Terms Used in This Document

- **ARIA** — WAI-ARIA 1.1 / 1.2, the Accessible Rich Internet Applications specification from the W3C.
- **AT** — Assistive Technology (e.g. NVDA, JAWS, VoiceOver, Dragon NaturallySpeaking, Windows Narrator).
- **APG** — WAI-ARIA Authoring Practices Guide, the W3C reference design patterns for ARIA widgets.
- **CSS isolation** — Blazor's per-component scoped CSS (`.razor.css`) where styles are prefixed with a scope identifier and do not leak across components.
- **Focus trap** — Programmatic confinement of keyboard focus inside an overlay while it is open.
- **`aria-activedescendant`** — ARIA attribute that lets a composite widget (e.g. combobox, grid) keep DOM focus on a container while communicating to AT which descendant is the active child.

---

## Instructions

The following tables list each EN 301 549 / WCAG requirement and the conformance level for the Syncfusion® Toolkit for Blazor. Each row links the criterion to the specific component behaviour and the code evidence. Appendix A summarises the result per component; Appendix B lists the source files examined.

---

## Detailed Conformance Evaluation

### 7.1 Success Criteria, Level A

#### 1.1.1 Non-text Content (Level A)

| | |
|---|---|
| **Criteria** | 1.1.1 Non-text Content |
| **Level** | A |
| **Conformance** | **Partially Supports** |

**Remarks:**
- **Decorative icons** in `SfButton` are correctly hidden from AT with `aria-hidden="true"` (`SfButton.razor:21,33`). ✅
- **Spinner SVG** uses `role="img"` + `aria-hidden="true"` so the decorative animation is not announced; the accessible name comes from `aria-label` on the root (`Border.razor:14-19`). ✅
- **Calendar week-number header** is hidden with `aria-hidden="true"` (`CalendarTableHeader.razor:9`). ✅
- **Chart `SvgPath`** suppresses `role="img"` and `tabindex` when no `AccessibilityText` is present, avoiding axe-core "image without label" violations. When `AccessibilityText` is provided, `role="img"` and `aria-label` are emitted. ✅
- **Chart data visualizations** (lines, bars, pie slices) are complex non-text content. The skill documents an `aria-describedby` pattern for data points, but this is **not implemented** — chart series and data points lack text alternatives. A consumer can supply an `sr-only` data table or description alongside the chart, but the component does not emit one. ⚠️
- **ButtonGroup selection-mode** renders a native checkbox/radio input plus a `<label>`; the visual state indicator is a CSS pseudo-element with no text alternative, but the native input + label association provides the semantic. ✅ (with the caveat that focus indication is weak — see 2.4.7).

#### 1.2.2 Captions (Prerecorded) — Not Applicable

| | |
|---|---|
| **Criteria** | 1.2.2 Captions (Prerecorded) |
| **Level** | A |
| **Conformance** | **Not Applicable** |

The toolkit ships no audio or video media. ⛔ N/A

#### 1.2.3 Audio Description or Media Alternative — Not Applicable

⛔ N/A — no media.

#### 1.3.1 Info and Fields (Level A)

| | |
|---|---|
| **Criteria** | 1.3.1 Info and Fields |
| **Level** | A |
| **Conformance** | **Partially Supports** |

**Supports:**
- **Form labels.** `SfTextBox`, `SfTextArea`, `SfNumericTextBox`, `SfDatePicker`, `SfTimePicker` all wire `aria-labelledby` and `aria-describedby` centrally in `SfInputBase.PreRender`. The FloatLabelType floating-label pattern uses `aria-label` derived from the placeholder or label text. ✅
- **CheckBox / RadioButton** pair a native `<input>` with a `<label for="id">` for proper name association. ✅
- **Switch** uses `role="switch"` with a resolved accessible-name priority chain (AriaLabel → label text → InnerContent) and `aria-labelledby`/`aria-describedby` wiring. The fix specifically addresses an NVDA double-announce anti-pattern. ✅
- **Calendar grid** uses `role="grid"` on the table, `role="gridcell"` on cells, `aria-selected`, `aria-disabled`, `aria-current="date"`, and composed localized `aria-label` per cell (`CalendarDayCell.razor:20`). ✅
- **Time lists** use `role="listbox"` + `role="option"`. ✅
- **Dialog** uses `role="dialog"`, `aria-modal`, `aria-labelledby`, and `aria-describedby`. ✅
- **Spinner** uses `role="status"`. ✅

**Partially Supports / Gaps:**
- **SfButton icon-only** renders no accessible name (Content / ChildContent empty). Only a `[Conditional("DEBUG")]` warning fires (`SfButton.razor.LifeCycle.cs:30-37`). Consumers must add `aria-label` via `HtmlAttributes`. ⚠️
- **SfSpinner double announcement** — both `aria-label` on the root and visible `Label` text inside the root are rendered, so AT may announce the loading text twice (`SfSpinner.razor:5-31`). ⚠️
- **SfTimePicker overwrites `AriaLabel`** with the hardcoded constant `"timepicker"` in `UpdateAriaAttributes()` (`SfTimePicker.razor.cs:473`), ignoring any user-supplied value. ⚠️
- **Time-list `<li>` options** lack `aria-selected` (TimePicker and DateTimePicker) — the ARIA listbox pattern requires it. ⚠️
- **SfButtonGroup `aria-label`** is only emitted for `SelectionMode.Single`, hardcoded in English, and not for `Multiple` or `None` (`SfButtonGroup.razor.cs:160-163`). ⚠️
- **Chart** lacks `aria-describedby` for data points and provides no `sr-only` data summary. ⚠️
- **Calendar `<tr>` elements** use implicit HTML row semantics rather than explicit `role="row"`. Acceptable in HTML5 but the ARIA grid pattern recommends explicit roles. ⚠️

#### 1.3.2 Meaningful Sequence (Level A)

| | |
|---|---|
| **Criteria** | 1.3.2 Meaningful Sequence |
| **Level** | A |
| **Conformance** | **Supports** |

**Remarks:**
- All components render content in DOM order that matches the visual reading order. No CSS `order`, `flex-direction: row-reverse`, or absolute positioning is used to resequence content within the component markup.
- The calendar grid renders weeks in source order; the time list renders options in chronological order; the chart legend and axes follow source order.

#### 1.3.3 Sensory Characteristics (Level A)

| | |
|---|---|
| **Criteria** | 1.3.3 Sensory Characteristics |
| **Level** | A |
| **Conformance** | **Partially Supports** |

**Remarks:**
- Shape / position / orientation cues are reinforced with text or ARIA in most components (e.g. "Previous Month" / "Next Month" `aria-label` on calendar nav buttons; "Clear" / "Open Calendar" / "Open Time" `aria-label` on icon buttons). ✅
- **SfButton icon-only** relies on the icon alone for meaning when no accessible name is provided — this violates the criterion unless the consumer adds `aria-label`. ⚠️ (see 1.1.1, 4.1.2)

#### 1.4.1 Use of Color (Level A)

| | |
|---|---|
| **Criteria** | 1.4.1 Use of Color |
| **Level** | A |
| **Conformance** | **Supports** |

**Remarks:**
- Color is never the sole indicator of state. `SfCheckBox` indeterminate, checked, and disabled states each have a distinct icon/glyph (checkmark, indeterminate dash) plus the native `checked` attribute. ✅
- `SfButton` primary/success/info/warning/danger variants differ in background color **and** are exposed as distinct CssClass values; however selection/toggle state in `IsToggle` mode is exposed via `aria-pressed` (not color alone). ✅
- Chart series colors are accompanied by legend text labels and (where enabled) data labels. ✅

#### 1.4.2 Audio Control — Not Applicable

⛔ N/A — no auto-playing audio.

#### 2.1.1 Keyboard (Level A)

| | |
|---|---|
| **Criteria** | 2.1.1 Keyboard |
| **Level** | A |
| **Conformance** | **Partially Supports** |

**Supports:**
- **SfButton** uses a native `<button>` — Enter and Space activate it natively; no key handlers are overridden. ✅
- **ButtonGroup inner Button (Single/Multiple)** handles Space and Enter explicitly (`Button.razor.cs:69-82`). ✅
- **CheckBox / RadioButton / Switch** use native inputs (Space toggles; arrow keys for radio groups via native tab navigation). ✅
- **SfTextBox / SfTextArea / SfNumericTextBox** use native inputs — all keyboard input works natively; NumericTextBox adds Up/Down arrow spin and Enter commit handlers. ✅
- **SfCalendar / SfDatePicker / SfDateTimePicker** implement the full ARIA grid keyboard model: Arrow, Home, End, PageUp/Down, Shift+PageUp/Down, Ctrl+Home/End, Ctrl+↑/↓, Enter, Escape, Alt+↓/↑, Tab. ✅
- **SfTimePicker** implements Up/Down, Home, End, Enter, Escape, Alt+↓/↑. ✅
- **SfDialog** supports Escape to close and full Tab focus trap while open. ✅
- **SfChart** implements Alt+J to enter, Tab between elements, arrows for data points, Enter/Space to activate, Ctrl± zoom, R reset, Ctrl+P print. ✅

**Partially Supports / Gaps:**
- **SfButtonGroup `SelectionMode.Single`** does not implement arrow-key navigation between radio items. The WAI-ARIA APG radio-group pattern expects arrow keys to move between options without changing Tab stops. ⚠️
- **SfUploader drop zone** (`role="button"`) does not handle Space or Enter to open the file browser — a `role="button"` element must be keyboard-activatable. ⚠️
- **SfTooltip with `OpensOn="Hover"`** is keyboard-inaccessible by design (hover is the only trigger). Consumers must use `OpensOn="Focus"` / `"Click"` / `"Focus+Hover"` for keyboard parity. ⚠️

#### 2.1.2 No Keyboard Trap (Level A)

| | |
|---|---|
| **Criteria** | 2.1.2 No Keyboard Trap |
| **Level** | A |
| **Conformance** | **Partially Supports** |

**Supports:**
- **SfDialog** implements a focus *trap* intentionally (modal dialog pattern) **and** provides Escape to dismiss plus focus restoration to the trigger on close — so the trap is escapable. ✅
- **SfCalendar (inline)** Tab is explicitly *not* prevented (`_isNotTabKey = false` when Tab is pressed, `CalendarBaseRender.razor.cs:44-47`) so users can Tab out. ✅

**Partially Supports / Gaps:**
- **DatePicker / DateTimePicker / TimePicker popups** have **no focus trap**. Tab can leave the popup onto background content. This is not itself a *trap*, but combined with the missing `aria-modal` on desktop (see 4.1.2) the popup is neither trapped nor semantically inert, which can strand keyboard users. ⚠️
- **SfDialog** focus trap is intentional and correct, but if a consumer disables `CloseOnEscape` and provides no close button, the dialog would become a trap. This is a consumer-configurable risk, not a library defect. ⚠️ (consumer responsibility)

#### 2.1.4 Character Key Shortcuts — Not Applicable

| | |
|---|---|
| **Criteria** | 2.1.4 Character Key Shortcuts |
| **Level** | A |
| **Conformance** | **Not Applicable** |

The toolkit implements mnemonics and modifiers (e.g. `Alt+J`, `Alt+↓`, `Ctrl+R`) but no single *printable character* shortcuts that would conflict with AT or alternate input methods. ⛔ N/A

#### 2.2.1 Timing Adjustable — Not Applicable

⛔ N/A — no time limits are imposed by the components. Auto-close behaviour in Dialog/Tooltip is consumer-configured.

#### 2.2.2 Pause, Stop, Hide — Partially Applicable

| | |
|---|---|
| **Criteria** | 2.2.2 Pause, Stop, Hide |
| **Level** | A |
| **Conformance** | **Supports** |

**Remarks:**
- `SfSpinner` animation is disabled under `@media (prefers-reduced-motion: reduce)` (`SfSpinner.razor.css:27-30`). ✅
- The spinner is consumer-toggled via `Visible` / `@bind-Visible` / `VisibleChanged`, so the consumer can stop and hide it when the operation completes. ✅

#### 2.3.1 Three Flashes or Below (Level A)

| | |
|---|---|
| **Criteria** | 2.3.1 Three Flashes or Below |
| **Level** | A |
| **Conformance** | **Supports** |

**Remarks:** No component animates at a rate greater than three flashes per second. Spinner rotation and tooltip/dialog fade animations are well below the threshold. ✅

#### 2.4.1 Bypass Blocks (Level A)

| | |
|---|---|
| **Criteria** | 2.4.1 Bypass Blocks |
| **Level** | A |
| **Conformance** | **Not Applicable** |

⛔ N/A — page-level skip links and landmarks are the host application's responsibility; the library emits no page chrome. ✅ (out of scope)

#### 2.4.2 Page Titled (Level A) — Not Applicable

⛔ N/A — page titles are the host application's responsibility.

#### 2.4.3 Focus Order (Level A)

| | |
|---|---|
| **Criteria** | 2.4.3 Focus Order |
| **Level** | A |
| **Conformance** | **Supports** |

**Remarks:**
- Components use native focusable elements (`<button>`, `<input>`, `<textarea>`, `[tabindex="0"]`) in source order. ✅
- **DatePicker** `moveFocusToPopup` JS (`datepicker.js:717-740`) blurs the input and focuses `.e-title` in the popup header on Tab — focus moves predictably from input → popup title → calendar grid → today button → out. ✅
- **Dialog** moves focus to the first focusable element inside the dialog on open and restores it to the trigger on close. ✅

#### 2.4.4 Link Purpose (In Context) (Level A) — Not Applicable

⛔ N/A — the library renders no navigation links; `SfButton` is an action control, not a link.

#### 2.4.5 Multiple Ways (Level A) — Not Applicable

⛔ N/A — page navigation is the host application's responsibility.

#### 2.4.6 Headings and Labels (Level A)

| | |
|---|---|
| **Criteria** | 2.4.6 Headings and Labels |
| **Level** | A |
| **Conformance** | **Partially Supports** |

**Supports:**
- Form inputs expose descriptive labels via `aria-label`, `aria-labelledby`, `<label for>`, and FloatLabelType. ✅
- Dialog, Tooltip, Spinner, Calendar popups all expose `aria-label`. ✅

**Partially Supports / Gaps:**
- **SfButton icon-only** has no label by default (see 1.1.1). ⚠️
- **SfTimePicker** forces `aria-label="timepicker"` regardless of consumer intent. ⚠️

#### 2.4.7 Focus Visible (Level A)

| | |
|---|---|
| **Criteria** | 2.4.7 Focus Visible |
| **Level** | A |
| **Conformance** | **Partially Supports** |

**Supports:**
- `SfButton` ships a `:focus-visible` rule (`SfButton.razor.css:17-20`): `outline: 2px solid Highlight; outline-offset: 2px`. ✅
- `SfCalendar` ships `:focus-visible` on cells and title (`SfCalendar.razor.css:25-29`). ✅
- `SfTimePicker` ships `:focus-visible` on list items (`SfTimePicker.razor.css:21-25`). ✅
- `SfTextBox` / `SfTextArea` ship `focus-visible` outlines and `forced-colors` support. ✅
- `SfNumericTextBox` ships `focus-visible` and `forced-colors` support. ✅
- `SfSwitch` ships `:focus-visible` ring. ✅
- All focusable elements rely on native `:focus-visible` defaults where component CSS does not override. ✅

**Partially Supports / Gaps:**
- **SfButtonGroup selection-mode** `<label>` elements lack a `:focus-visible` style. The `SfButton.razor.css` focus rule is scoped to SfButton only and does not apply to the ButtonGroup label. Keyboard focus on the visually hidden native input may produce no visible focus ring on the styled label. ⚠️
- **SfUploader** drop zone / browse button focus visibility depends on theme CSS. ⚠️ (theme-dependent)

#### 3.1.1 Language of Page (Level A) — Not Applicable

⛔ N/A — host page `<html lang>` is the application's responsibility.

#### 3.1.2 Language of Parts (Level A)

| | |
|---|---|
| **Criteria** | 3.1.2 Language of Parts |
| **Level** | A |
| **Conformance** | **Partially Supports** |

**Supports:**
- Most user-facing strings (calendar navigation, popup labels, clear/open buttons) are routed through a `Localizer` service so consumers can supply localized resources. ✅

**Partially Supports / Gaps:**
- **SfButtonGroup `aria-label`** is hardcoded English (`"Button group with single selection mode"`, `SfButtonGroup.razor.cs:162`) — not routed through `Localizer`. ⚠️
- **SfSpinner default `aria-label`** is hardcoded English (`"Loading"`, `SfSpinner.razor.cs:42`) — falls back when no `AriaLabel` is supplied. ⚠️
- **SfTimePicker `aria-label`** is hardcoded English (`"timepicker"`, `SfTimePicker.razor.cs:473`) and also overwrites the consumer value. ⚠️
- **Time-list `aria-label="popup"`** (TimePicker, DateTimePicker) is English and non-descriptive. ⚠️

#### 3.2.1 On Focus (Level A)

| | |
|---|---|
| **Criteria** | 3.2.1 On Focus |
| **Level** | A |
| **Conformance** | **Supports** |

**Remarks:** No component initiates a context change purely on focus. Tooltip with `OpensOn="Focus"` displays content but does not change the page context; Dialog opens are triggered by click/Enter, not focus. DatePicker/TimePicker popups open on icon click or `Alt+↓`, not on input focus. ✅

#### 3.2.2 On Input (Level A)

| | |
|---|---|
| **Criteria** | 3.2.2 On Input |
| **Level** | A |
| **Conformance** | **Partially Supports** |

**Supports:**
- `SfTextBox`, `SfTextArea`, `SfNumericTextBox` support `@bind-Value` two-way binding; value changes fire `OnChange`/`ValueChanged` on blur or Enter — predictable, reversible, and consumer-cancelable. ✅
- DatePicker/TimePicker commit on Enter or blur and dismiss on Escape without committing. ✅

**Partially Supports / Gaps:**
- Some popup behaviors auto-navigate (e.g. Ctrl+↑ drills the calendar view) without an explicit "submit" step. The change is reversible via Escape. ✅ (acceptable)

#### 3.2.3 Consistent Navigation (Level A) — Not Applicable

⛔ N/A — navigation chrome is the host application's responsibility.

#### 3.2.4 Consistent Identification (Level A)

| | |
|---|---|
| **Criteria** | 3.2.4 Consistent Identification |
| **Level** | A |
| **Conformance** | **Supports** |

**Remarks:** Components use consistent class names, ARIA roles, and labels across the toolkit (e.g. all calendar nav buttons use `Localizer["PreviousMonth"]` / `Localizer["NextMonth"]`). The inconsistent `aria-label` overrides (TimePicker) are a 1.3.1 / 3.1.2 issue, not an identification-consistency issue. ✅

#### 3.3.1 Error Identification (Level A)

| | |
|---|---|
| **Criteria** | 3.3.1 Error Identification |
| **Level** | A |
| **Conformance** | **Partially Supports** |

**Supports:**
- `SfTextBox`, `SfTextArea`, `SfNumericTextBox`, `SfDatePicker`, `SfTimePicker` integrate with Blazor `EditForm` / `EditContext` and wire `aria-invalid` and `aria-describedby` (to the validation message element id) when validation fails (`SfInputBase.PreRender`). ✅
- `SfNumericTextBox` enforces Min/Max and emits `aria-invalid` on out-of-range values. ✅

**Partially Supports / Gaps:**
- **SfCheckBox, SfRadioButton, SfSwitch, SfButtonGroup** do **not** automatically wire `aria-invalid` / `aria-describedby` to EditContext validation errors. Consumers must add these manually via `HtmlAttributes`. ⚠️
- **SfUploader** has no EditContext validation integration. ⚠️

#### 3.3.2 Labels or Instructions (Level A)

| | |
|---|---|
| **Criteria** | 3.3.2 Labels or Instructions |
| **Level** | A |
| **Conformance** | **Partially Supports** |

**Supports:**
- All form input components expose `AriaLabel`, `AriaLabelledBy`, `AriaDescribedBy` parameters and floating-label / placeholder options. ✅
- `SfUploader` exposes `AriaLabel` for the drop zone and file input. ✅

**Partially Supports / Gaps:**
- Default instructions (e.g. "Press Alt+↓ to open the calendar") are not emitted; consumers must provide them. ⚠️ (shared library/application responsibility)
- **SfTimePicker** ignores user `AriaLabel`. ⚠️

#### 3.3.3 Error Suggestion (Level A) — Level AA (below)

See Level AA section.

#### 4.1.1 Parsing (Level A)

| | |
|---|---|
| **Criteria** | 4.1.1 Parsing |
| **Level** | A |
| **Conformance** | **Supports** |

**Remarks:** Blazor renders well-formed HTML; component markup uses valid elements and attributes. No duplicate IDs are introduced by the components themselves (`id` values are generated from a unique `ID` property). Note: 4.1.1 is obsolete in WCAG 2.2 but retained here for EN 301 549 v3.2.1 completeness. ✅

#### 4.1.2 Name, Role, Value (Level A)

| | |
|---|---|
| **Criteria** | 4.1.2 Name, Role, Value |
| **Level** | A |
| **Conformance** | **Partially Supports** |

**Supports:**
- All interactive components expose proper roles (native or ARIA), names, and states. See Appendix A for the per-component matrix. ✅
- `aria-pressed` on toggle SfButton, `aria-checked` on CheckBox/RadioButton/Switch, `aria-expanded` on combobox inputs, `aria-selected` on calendar cells, `aria-disabled`, `aria-busy` on Spinner, `aria-modal`/`aria-labelledby` on Dialog. ✅

**Partially Supports / Gaps:**
- **SfButton icon-only** has a role (native `button`) but no name. ⚠️
- **SfTimePicker** forces the name to `"timepicker"`, overriding the consumer. ⚠️
- **Time-list options** lack `aria-selected`. ⚠️
- **SfDialog** never uses `role="alertdialog"` for single-button modals (APG recommends it). ⚠️
- **SfTooltip** content `aria-hidden` is not toggled when the tooltip is hidden. ⚠️
- **SfButtonGroup** Single mode lacks `aria-label` for the group except in Single mode, and even then the label is hardcoded. ⚠️

#### 4.1.3 Status Messages (Level A)

| | |
|---|---|
| **Criteria** | 4.1.3 Status Messages |
| **Level** | A |
| **Conformance** | **Partially Supports** |

**Supports:**
- **SfSpinner** uses `role="status"` + `aria-live="polite"` + `aria-busy` so the loading state is announced. ✅
- **SfNumericTextBox** emits a dedicated `aria-live="polite"` region with deduplication logic for value-change announcements. ✅
- **SfDatePicker** sets `aria-live="assertive"` + `aria-atomic="true"` on the input for value announcements. ✅

**Partially Supports / Gaps:**
- **SfTimePicker** does **not** set `aria-live` on the input — value changes are not announced. ⚠️
- **SfCheckBox** has an unimplemented live-region TODO for state-change announcements. ⚠️
- **SfCalendar (inline)** has no `aria-live` region — month/year navigation changes are not announced to AT (the cell `aria-label`s change, but no live region surfaces this). ⚠️
- **SfChart** has no `aria-live` region for dynamic data updates. ⚠️

---

### 7.2 Success Criteria, Level AA

#### 1.2.5 Audio Description (Prerecorded) — Not Applicable

⛔ N/A — no media.

#### 1.3.4 Orientation (Level AA)

| | |
|---|---|
| **Criteria** | 1.3.4 Orientation |
| **Level** | AA |
| **Conformance** | **Supports** |

**Remarks:** No component restricts orientation. The CSS uses responsive flex/grid layouts; popups reposition via JS. ✅

#### 1.3.5 Identify Input Purpose (Level AA)

| | |
|---|---|
| **Criteria** | 1.3.5 Identify Input Purpose |
| **Level** | AA |
| **Conformance** | **Partially Supports** |

**Supports:**
- `SfTextBox` exposes an `HtmlAttributes` dictionary so consumers can add `autocomplete` tokens (e.g. `autocomplete="username"`, `autocomplete="current-password"`). ✅
- `SfDatePicker` sets `autocorrect="off"` and `spellcheck="false"`. ✅

**Partially Supports / Gaps:**
- The library does not expose a typed `AutoComplete` parameter on TextBox/NumericTextBox; the consumer must use `HtmlAttributes`. ⚠️ (still achievable)

#### 1.4.3 Contrast (Minimum) (Level AA)

| | |
|---|---|
| **Criteria** | 1.4.3 Contrast (Minimum) |
| **Level** | AA |
| **Conformance** | **Partially Supports** |

**Supports:**
- `SfButton` ships `@media (forced-colors: active)` with `border: 1px solid ButtonText; forced-color-adjust: none` (`SfButton.razor.css:22-27`) — Windows High Contrast mode is supported. ✅
- `SfSpinner`, `SfCalendar`, `SfTimePicker` ship `forced-colors` support. ✅

**Partially Supports / Gaps:**
- The default theme's color tokens are generated by the gulp pipeline under `src/wwwroot/styles/*.scss`. The library's *default* theme has not been measured against the 4.5:1 / 3:1 thresholds in this static review. ⚠️ (theme-dependent; consumer responsibility to verify the chosen theme meets contrast)

#### 1.4.4 Resize Text (Level AA)

| | |
|---|---|
| **Criteria** | 1.4.4 Resize Text |
| **Level** | AA |
| **Conformance** | **Supports** |

**Remarks:** Components use `rem`/`em` and relative sizing in CSS; no fixed `px` font sizes that would prevent 200% zoom. ✅ (not fully measured in this review)

#### 1.4.5 Images of Text (Level AA) — Not Applicable

⛔ N/A — no images of text.

#### 1.4.10 Reflow (Level AA)

| | |
|---|---|
| **Criteria** | 1.4.10 Reflow |
| **Level** | AA |
| **Conformance** | **Supports** |

**Remarks:** Components do not force horizontal scrolling at 320 CSS px width. Popups are positioned by JS and can reflow. Chart can scroll/pan horizontally. ✅

#### 1.4.11 Non-text Contrast (Level AA)

| | |
|---|---|
| **Criteria** | 1.4.11 Non-text Contrast |
| **Level** | AA |
| **Conformance** | **Partially Supports** |

**Supports:**
- Focus outlines use `Highlight` (a system color) which meets the 3:1 threshold against the default background. ✅
- Button borders are `1px solid ButtonText` under `forced-colors`. ✅

**Partially Supports / Gaps:**
- Default-theme UI component boundaries (input borders, calendar grid lines) have not been measured at 3:1 in this review. ⚠️ (theme-dependent)

#### 1.4.12 Text Spacing (Level AA)

| | |
|---|---|
| **Criteria** | 1.4.12 Text Spacing |
| **Level** | AA |
| **Conformance** | **Supports** |

**Remarks:** Components do not override `line-height`, `letter-spacing`, or paragraph spacing with `!important`. ✅ (not fully measured)

#### 1.4.13 Content on Hover or Focus (Level AA)

| | |
|---|---|
| **Criteria** | 1.4.13 Content on Hover or Focus |
| **Level** | AA |
| **Conformance** | **Partially Supports** |

**Supports:**
- **SfTooltip** with `OpensOn="Focus"` or `"Focus+Hover"` is dismissible (Esc / focus move / click-away) and hoverable (the tooltip content can receive pointer hover without dismissing). ✅
- Tooltip content persists until the trigger loses focus/hover, so it is not transient in a way that blocks reading. ✅

**Partially Supports / Gaps:**
- **SfTooltip `OpensOn="Hover"`-only** mode is keyboard-inaccessible (no focus trigger), so the criterion's "on focus" branch is not satisfied. Consumers must opt in to a focus-based trigger. ⚠️

#### 2.4.5 Multiple Ways (Level AA) — Not Applicable

⛔ N/A — page navigation is the host application's responsibility.

#### 2.4.6 Headings and Labels (Level AA) — Same as Level A above.

#### 2.4.7 Focus Visible (Level AA) — Same as Level A above.

#### 2.5.1 Pointer Gestures (Level AA)

| | |
|---|---|
| **Criteria** | 2.5.1 Pointer Gestures |
| **Level** | AA |
| **Conformance** | **Supports** |

**Remarks:** Components use simple taps/clicks; no path-based or multi-point gestures are required. Drag-to-resize on Dialog is an *optional* enhancement with keyboard resize alternatives. ✅

#### 2.5.2 Pointer Cancellation (Level AA)

| | |
|---|---|
| **Criteria** | 2.5.2 Pointer Cancellation |
| **Level** | AA |
| **Conformance** | **Supports** |

**Remarks:** Click handlers fire on `mouseup`/click (Blazor `@onclick`), not on `mousedown`, so users can cancel by moving the pointer off the target before release. ✅

#### 2.5.3 Label in Name (Level AA)

| | |
|---|---|
| **Criteria** | 2.5.3 Label in Name |
| **Level** | AA |
| **Conformance** | **Partially Supports** |

**Supports:**
- `SfTextBox`, `SfTextArea`, `SfNumericTextBox` use the visible label as the `aria-label` (via FloatLabelType / Label parameter). ✅
- `SfCheckBox`, `SfRadioButton` use `<label for>` so the visible label text is the accessible name. ✅

**Partially Supports / Gaps:**
- **SfTimePicker** sets `aria-label="timepicker"` which does not match the visible floating label. ⚠️
- **SfButton icon-only** has no visible label and no accessible name. ⚠️

#### 2.5.4 Motion Actuation (Level AA)

| | |
|---|---|
| **Criteria** | 2.5.4 Motion Actuation |
| **Level** | AA |
| **Conformance** | **Not Applicable** |

⛔ N/A — no motion-activated features.

#### 3.1.2 Language of Parts (Level AA) — Same as Level A above.

#### 3.2.3 Consistent Navigation (Level AA) — Not Applicable

⛔ N/A.

#### 3.2.4 Consistent Identification (Level AA) — Same as Level A above.

#### 3.3.2 Labels or Instructions (Level AA) — Same as Level A above.

#### 3.3.3 Error Suggestion (Level AA)

| | |
|---|---|
| **Criteria** | 3.3.3 Error Suggestion |
| **Level** | AA |
| **Conformance** | **Partially Supports** |

**Supports:**
- `SfTextBox`, `SfTextArea`, `SfNumericTextBox`, `SfDatePicker`, `SfTimePicker` surface EditContext validation messages via `aria-describedby` and `aria-invalid`. ✅
- `SfNumericTextBox` enforces Min/Max and provides spin buttons to correct out-of-range values. ✅

**Partially Supports / Gaps:**
- Selection components (CheckBox, RadioButton, Switch, ButtonGroup) do not auto-wire `aria-describedby` to suggestion text. ⚠️
- **SfUploader** does not integrate with EditContext. ⚠️

#### 3.3.4 Error Prevention (Legal, Financial, Data) (Level AA)

| | |
|---|---|
| **Criteria** | 3.3.4 Error Prevention |
| **Level** | AA |
| **Conformance** | **Not Applicable** |

⛔ N/A — form submission/reversal is the host application's responsibility; the library provides validation wiring but not submission flows.

#### 4.1.3 Status Messages (Level AA) — Same as Level A above.

---

### 7.3 EN 301 549 Specific Requirements

#### 4. Functional Performance Statements (FPS)

EN 301 549 Clause 4 requires statements covering usage by users with and without vision, hearing, dexterity, and voice.

| FPS | Conformance | Remarks |
|---|---|---|
| 4.2.1 Usage without vision | **Partially Supports** | All components expose ARIA roles/names and full keyboard operation (see 2.1.1, 4.1.2). Gaps: icon-only button name, TimePicker `aria-live`, Calendar inline `aria-live`. |
| 4.2.2 Usage with limited vision | **Partially Supports** | Focus indicators and `forced-colors` support present. Gaps: ButtonGroup focus-visible, default-theme contrast not measured. |
| 4.2.3 Usage without perception of colour | **Supports** | State is never conveyed by colour alone (see 1.4.1). |
| 4.2.4 Usage without hearing | **Supports** | No audio output is relied upon. |
| 4.2.5 Usage with limited hearing | **Supports** | No audio output. |
| 4.2.6 Usage with limited operation / dexterity | **Partially Supports** | Full keyboard parity except Tooltip hover-only mode and Uploader drop-zone Space/Enter. |
| 4.2.7 Usage with voice | **Not Applicable** | No voice-input features. |
| 4.2.8 Usage with limited cognition | **Supports** | Consistent labelling, predictable behaviour (see 3.2.x). Gaps: TimePicker label mismatch. |

#### 5. Generic Requirements

| 5.x | Conformance | Remarks |
|---|---|---|
| 5.1.2 Closed functionality | **Not Applicable** | The library is open — all functions are operable with AT. |
| 5.1.3 Use of accessibility services | **Not Applicable** | Web library — uses platform AT APIs via the browser. |
| 5.1.4 Accessibility services for authoring | **Not Applicable** | Not an authoring tool. |
| 5.1.5 Assistive technology | **Not Applicable** | Does not ship AT. |
| 5.1.6 Documented accessibility features | **Supports** | Accessibility behaviour is documented in the per-category SKILL.md files under `.github/skills/` and in inline `WCAG 1.3.1 / 4.1.2` code comments. |
| 5.1.7 Magnification / speech / braille | **Partially Supports** | Reflow (1.4.10) and resize (1.4.4) supported; braille depends on the consumer's AT rendering the ARIA semantics. |

#### 6. ICT with Video — Not Applicable

⛔ N/A.

#### 7. ICT with Hardware — Not Applicable

⛔ N/A — software library only.

#### 8. ICT with Two-Way Voice — Not Applicable

⛔ N/A.

#### 9. ICT with Hardware — Not Applicable

⛔ N/A.

#### 10. ICT with Information / Documentation

| 10.x | Conformance | Remarks |
|---|---|---|
| 10.1 Product documentation | **Partially Supports** | README, SKILL.md files, and XML doc comments cover component behaviour and accessibility guidance. Gaps: this VPAT is the first consolidated accessibility statement; per-component keyboard-shortcut tables are not yet published. |
| 10.2 Support documentation | **Partially Supports** | Public GitHub issues and contributing guide exist. Accessibility-specific support process not documented separately. |

#### 11. ICT Support Services

| 11.x | Conformance | Remarks |
|---|---|---|
| 11.2 Information on accessibility features | **Partially Supports** | See 10.1. |
| 11.3 Efficient communication | **Partially Supports** | GitHub-based text communication. |
| 11.4 Compatibility with AT | **Partially Supports** | Components use standard ARIA and native semantics; testing with specific AT (NVDA, JAWS, VoiceOver) is ongoing. Known NVDA double-announce issue was fixed for SfSwitch. |
| 11.5 Privacy | **Supports** | Components do not transmit personal data; consumer applications are responsible for privacy. |

#### 12. ICT Providing Relay / Emergency Service — Not Applicable

⛔ N/A.

#### 13. ICT for Real-Time Text — Not Applicable

⛔ N/A.

#### 14. ICT for Video Communication — Not Applicable

⛔ N/A.

---

### 7.4 Remarks and Explanations

#### Consumer Responsibilities (not defects in the library)

The following are out of the library's control and remain the responsibility of the application built *with* the toolkit:

1. **Page-level accessibility** — landmarks, skip links, document language (`<html lang>`), page title, consistent navigation across pages.
2. **Final color contrast** of the chosen theme — the toolkit ships theme SCSS that generates CSS via gulp; the consumer must verify the generated palette meets WCAG 1.4.3 / 1.4.11.
3. **Form submission and error prevention** — the library wires validation but does not perform submission.
4. **Tooltip keyboard trigger choice** — consumers must select `OpensOn="Focus"` or `"Focus+Hover"` for keyboard accessibility; `"Hover"`-only is not keyboard-accessible.
5. **Icon-only button labels** — consumers must supply `aria-label` via `HtmlAttributes` when using `SfButton` with `IconCss` and no text content.
6. **Dialog `CloseOnEscape`** — consumers should leave `CloseOnEscape="true"` (the default) to keep the modal escapable.
7. **Live-region announcements for inline Calendar** — consumers should add an `aria-live` region or visually-hidden status text if dynamic navigation announcements are required.

#### Notable Library-Level Fixes Already Shipped

- **SfSwitch accessible-name priority chain** — resolves an NVDA double-announce anti-pattern by preferring `aria-label` then `aria-labelledby` then label text then InnerContent.
- **Calendar `aria-labelledby` / `aria-describedby` surfaced on root** — `SfCalendar.razor.cs:93-100` carries an explicit `WCAG 1.3.1 / 4.1.2` code comment explaining the fix.
- **Chart `SvgPath` accessibility decision** — `role="img"` and `tabindex` are suppressed when no `AccessibilityText` is present, preventing axe-core "image without label" violations.
- **`prefers-reduced-motion`** on Spinner, **`forced-colors`** on Button, Spinner, Calendar, TimePicker, TextBox, NumericTextBox, Switch.

---

## Appendix A: Component-by-Component Accessibility Matrix

Legend: ✅ Supports · ⚠️ Partial · ❌ Gap · — N/A

| Component | Role (native/ARIA) | Name (label) | States (aria-*) | Keyboard | Focus visible | Live region | Forced colors | Reduced motion |
|---|---|---|---|---|---|---|---|---|
| **SfButton** | native `button` ✅ | Content/ChildContent; ⚠️ none if icon-only | `aria-disabled`, `aria-pressed` (toggle) ✅ | Enter/Space native ✅ | ✅ `:focus-visible` | — | ✅ | — |
| **SfButtonGroup** | `role="group"` ✅ | ⚠️ Single-only, hardcoded EN | native `checked` ✅ | Space/Enter ✅; ⚠️ no arrows in Single | ⚠️ label focus-visible missing | — | ✅ | — |
| **SfButtonGroup inner Button (None mode)** | delegates to `SfButton` | same as SfButton | same as SfButton | same as SfButton | same as SfButton | — | ✅ | — |
| **SfCheckBox** | native `checkbox` ✅ | `<label for>` ✅ | `checked`, `indeterminate` ✅ | Space ✅ | ✅ (native) | ❌ TODO live region | — | — |
| **SfRadioButton** | native `radio` ✅ | `<label for>` ✅ | `checked` ✅ | Space/Arrows (native) ✅ | ✅ (native) | — | — | — |
| **SfSwitch** | `role="switch"` ✅ | priority chain ✅ | `aria-checked` ✅ | Space ✅ | ✅ `:focus-visible` | — | ✅ | — |
| **SfCalendar** | `role="grid"` ✅ | localized `aria-label` ✅ | `aria-selected`, `aria-disabled`, `aria-current` ✅ | Arrows/Home/End/PgUp/PgDn/Ctrl ✅ | ✅ `:focus-visible` | ⚠️ none (inline) | ✅ | — |
| **SfDatePicker** | `role="combobox"` + `role="grid"` ✅ | `AriaLabel` or ⚠️ empty | `aria-expanded`, `aria-owns`, `aria-activedescendant`, `aria-controls`, `aria-invalid` ✅ | Full grid + Alt+↓/↑ + Esc ✅ | ✅ | ✅ `aria-live="assertive"` | ✅ | — |
| **SfDateTimePicker** | `combobox` + `grid` + `listbox` ✅ | localized fallback ✅ | as DatePicker + `aria-activedescendant` for time list ✅ | Full grid + time list ✅ | ✅ | ✅ (inherited) | ✅ | — |
| **SfTimePicker** | `role="combobox"` ✅ | ⚠️ hardcoded "timepicker" | `aria-expanded`, `aria-owns`, `aria-controls`, `aria-activedescendant` ✅; ⚠️ no `aria-selected` on `<li>` | Up/Down/Home/End/Enter/Esc/Alt+↓/↑ ✅ | ✅ `:focus-visible` | ❌ no `aria-live` | ✅ | — |
| **SfTextBox** | native `input` ✅ | FloatLabel/`aria-label`/`aria-labelledby` ✅ | `aria-invalid`, `aria-describedby` via EditContext ✅ | native ✅ | ✅ `:focus-visible` | — | ✅ | — |
| **SfTextArea** | native `textarea` ✅ | same as TextBox ✅ | same as TextBox ✅ | native ✅ | ✅ `:focus-visible` | — | ✅ | — |
| **SfNumericTextBox** | native `input` ✅ | FloatLabel/`aria-*` ✅ | `aria-invalid`, `aria-valuemin/max/now` ✅ | Up/Down spin, Enter ✅ | ✅ `:focus-visible` | ✅ `aria-live="polite"` | ✅ | — |
| **SfUploader** | `input[type=file]` + drop zone `role="button"` ⚠️ | `AriaLabel` ✅ | — | ⚠️ drop zone Space/Enter missing | ⚠️ theme-dependent | — | — | — |
| **SfDialog** | `role="dialog"` ✅ (⚠️ no `alertdialog`) | `aria-labelledby`/`aria-describedby` ✅ | `aria-modal` ✅ | Esc + Tab trap + focus restore ✅ | ✅ | — | — | — |
| **SfTooltip** | `role="tooltip"` + `aria-describedby` on trigger ✅ | ⚠️ `AriaLabel` not on content; ⚠️ `aria-hidden` not toggled | — | ⚠️ Hover-only mode inaccessible | ✅ (when Focus trigger) | — | — | — |
| **SfChart** | `role="img"` on SVG paths with `AccessibilityText` ✅; ⚠️ no `aria-describedby` for data points | `aria-label` ✅ | — | Alt+J, Tab, arrows, Enter/Space, Ctrl±, R, Ctrl+P ✅ | ✅ | ❌ no `aria-live` for updates | — | — |
| **SfSpinner** | `role="status"` ✅ | `aria-label` (⚠️ hardcoded EN default) | `aria-busy` ✅ | — (non-interactive) ✅ | — | ✅ `aria-live="polite"` | ✅ | ✅ `prefers-reduced-motion` |

---

## Appendix B: Evidence Index

Key source files examined during this evaluation (paths relative to repository root):

**Buttons**
- `src/Components/Buttons/Button/SfButton.razor` — markup: `aria-disabled`, `aria-pressed`, `aria-hidden` on icons
- `src/Components/Buttons/Button/SfButton.razor.cs` — `GetAriaDisabled()`, `GetAriaPressed()`
- `src/Components/Buttons/Button/SfButton.razor.LifeCycle.cs` — `CheckIconOnlyAccessibleName()` (DEBUG-only)
- `src/Components/Buttons/Button/SfButton.razor.css` — `:focus-visible`, `forced-colors`
- `src/Components/Buttons/ButtonGroup/Button.razor` — `<input type>` + `<label for>` markup
- `src/Components/Buttons/ButtonGroup/Button.razor.cs` — `HandleInputKeyDownAsync` (Space/Enter)
- `src/Components/Buttons/ButtonGroup/SfButtonGroup.razor.cs` — `GetMergedAttributes()` (Single-only `aria-label`)

**Calendars**
- `src/Components/Calendars/Base/Renderer/CalendarBaseRender.razor` — `role="grid"`, `tabindex`, `aria-label` on title/prev/next
- `src/Components/Calendars/Base/Renderer/CalendarBaseRender.razor.cs` — `MapKeyToAction` (full keyboard map)
- `src/Components/Calendars/Base/Renderer/CalendarDayCell.razor` — `role="gridcell"`, `aria-selected`, `aria-disabled`, `aria-current`, `aria-label`
- `src/Components/Calendars/Base/Renderer/CalendarTableHeader.razor` — `scope="col"`, `aria-hidden` on week header
- `src/Components/Calendars/Calendar/SfCalendar.razor.cs:93-100` — `aria-labelledby`/`aria-describedby` WCAG comment
- `src/Components/Calendars/Calendar/SfCalendar.razor.css` — `:focus-visible`, `forced-colors`
- `src/Components/Calendars/DatePicker/SfDatePicker.razor` — `role="dialog"`, `aria-modal`, `aria-label`
- `src/Components/Calendars/DatePicker/SfDatePicker.razor.cs` — `UpdateAriaAttributes()` (combobox, `aria-live`, `aria-atomic`, `aria-haspopup`, `aria-controls`)
- `src/Components/Calendars/DatePicker/SfDatePicker.razor.Methods.cs` — `MapInputKeyToAction`, `MoveFocusToPopupAsync`
- `src/Components/Calendars/DateTimePicker/SfDateTimePicker.razor` — time list `role="listbox"`/`role="option"`
- `src/Components/Calendars/TimePicker/SfTimePicker.razor.cs:464-478` — `UpdateAriaAttributes()` (hardcoded `aria-label="timepicker"`)
- `src/Components/Calendars/TimePicker/SfTimePicker.razor.cs:1832-1854` — `MapKeyToAction`
- `src/Components/Calendars/TimePicker/SfTimePicker.razor.css` — `:focus-visible`, `forced-colors`
- `src/wwwroot/scripts/datepicker.js` — `moveFocusToPopup`, `updateAriaActiveDescendant`, `bindPopupTabHandler`
- `src/wwwroot/scripts/timepicker.js` — key configuration

**Inputs**
- `src/Components/Inputs/TextBox/` — native input, FloatLabel, `aria-invalid`/`aria-describedby` via `SfInputBase`
- `src/Components/Inputs/TextArea/` — native textarea
- `src/Components/Inputs/NumericTextBox/` — `aria-valuemin/max/now`, dedicated `aria-live="polite"` region
- `src/Components/Inputs/CheckBox/` — native `<input type="checkbox">` + `<label for>`; live-region TODO
- `src/Components/Inputs/RadioButton/` — native `<input type="radio">` + `<label for>`
- `src/Components/Inputs/Switch/` — `role="switch"`, accessible-name priority chain
- `src/Components/Inputs/Uploader/` — `role="button"` drop zone; no Space/Enter handler
- `src/wwwroot/scripts/textbox.js`, `textarea.js`, `numerictextbox.js`, `checkbox.js`, `switch.js`, `uploader.js`

**Popups**
- `src/Components/Popups/Dialog/` — `role="dialog"`, `aria-modal`, `aria-labelledby`, `aria-describedby`, focus trap, Esc, focus restore
- `src/Components/Popups/Tooltip/` — `role="tooltip"`, `aria-describedby` on trigger; `aria-hidden` not toggled; `OpensOn` modes
- `src/wwwroot/scripts/dialog.js`, `tooltip.js`, `popup.js`

**Charts**
- `src/Components/Charts/Chart/` — `SvgPath` `role="img"`/`tabindex` suppression logic; Alt+J / arrow / Enter / Ctrl± / R / Ctrl+P keyboard model

**Notifications**
- `src/Components/Spinner/SfSpinner.razor` — `role="status"`, `aria-busy`, `aria-live`, `aria-label`
- `src/Components/Spinner/SfSpinner.razor.cs:42` — hardcoded `"Loading"`
- `src/Components/Spinner/Renderer/Border.razor` — SVG `role="img"` + `aria-hidden`
- `src/Components/Spinner/SfSpinner.razor.css` — `prefers-reduced-motion`, `forced-colors`

**Skill documentation (accessibility reference)**
- `.github/skills/syncfusion-blazor-toolkit-buttons/SKILL.md`
- `.github/skills/syncfusion-blazor-toolkit-calendars/SKILL.md`
- `.github/skills/syncfusion-blazor-toolkit-charts/SKILL.md`
- `.github/skills/syncfusion-blazor-toolkit-inputs/SKILL.md`
- `.github/skills/syncfusion-blazor-toolkit-notifications/SKILL.md`
- `.github/skills/syncfusion-blazor-toolkit-popups/SKILL.md`

---

*End of VPAT 2.5 INT — Syncfusion® Toolkit for Blazor 1.0.0*
