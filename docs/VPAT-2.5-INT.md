# DRAFT accessibility evidence register — WCAG / EN 301 549 / Section 508

**Product:** Syncfusion Toolkit for Blazor (`Syncfusion.Blazor.Toolkit`).
**Status:** DRAFT; **not an approved Accessibility Conformance Report (ACR), completed VPAT, certification, or procurement assurance**. The historical filename `VPAT-2.5-INT.md` is retained for discoverability only.
**Basis:** selected source and configuration observations in the current working tree. Runtime conformance, evaluator identity/qualifications, evaluation dates, approval, and release-package identity are not established.

The local [project](../src/Syncfusion.Blazor.Toolkit.csproj) declares version `1.0.0` and targets .NET 8/9/10. That is not a verified evaluated release. Supplied GitHub metadata lists v1.0.2 (September 1, 2026) and v1.0.1 (June 26, 2026), both without uploaded assets; it supplies neither a release-specific ACR nor proof of NuGet contents. No historical commit is designated as a tested baseline by this draft.

## Standards coverage and authority

| Reference | Use in this draft |
|---|---|
| [WCAG 2.2](https://www.w3.org/TR/WCAG22/) | A/AA checklist, not a conformance claim. WCAG 2.2 adds six A/AA and three AAA criteria and removes 4.1.1; it is not simply WCAG 2.1 plus the former six-row list. |
| [WCAG 2.1](https://www.w3.org/TR/WCAG21/) | Earlier A/AA coverage, including a separate legacy parsing check where needed. |
| [EN 301 549 v3.2.1](https://www.etsi.org/deliver/etsi_en/301500_301599/301549/03.02.01_60/en_301549v030201p.pdf) | Clause-level applicability register. Its web requirements reference WCAG 2.1, not WCAG 2.2. No legal or harmonization conclusion is made for a deployment. |
| [Revised Section 508](https://www.access-board.gov/ict/) | Separate scoping and chapter checklist; incorporates WCAG 2.0 A/AA in applicable provisions. No automatic EN-to-508 equivalence or conformance mapping is claimed. |
| [WAI-ARIA 1.2](https://www.w3.org/TR/wai-aria-1.2/) and [APG](https://www.w3.org/WAI/ARIA/apg/) | Semantics and informative widget-pattern guidance; neither source comments nor APG resemblance certify WCAG conformance. |

## Evidence vocabulary and scope

- **Source mechanism:** markup, code, CSS rule, or test definition exists. It is not an observed result and does not establish full behavior or conformance.
- **Not Evaluated:** sufficient runtime/measurement evidence is missing. This does not mean Supports, Partially Supports, or Does Not Support.
- **Applicability pending:** determine whether a requirement applies to the chosen product/integration scope before assigning Not Applicable with a rationale.

Coverage includes the 17 component scenarios in the component register, their generated styles/browser scripts, and integration conditions that must be exercised. Host page structure, content, forms, navigation and authentication remain application responsibilities, but cannot be ignored in a full-page/complete-process WCAG evaluation. Default theme contrast and interaction behavior are still part of library evaluation; customization does not excuse an unmeasured default theme.

## Evidence inventory

**No runtime accessibility testing is asserted by this synchronization.** No retained Insights exports or AT session records substantiate the prior results. Playwright specs exist under `tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/`, and [CI](../.github/workflows/ci.yml) configures execution/report upload. The root [Playwright configuration](../playwright.config.ts) selects Chromium and the Playwright sample host. Definitions and configuration do not establish successful runs or manual keyboard/screen-reader testing.

See the [Insights register](../.github/accessibility/insights-summary.md) and [AT checklist](../.github/accessibility/screen-reader-smoke.md) for required evidence. No nightly Insights/bot workflow, measured contrast report, independent audit, or evaluator sign-off is established here.

## WCAG evaluation checklist

**Every criterion below is Not Evaluated.** Entries describe needed evidence, not conformance ratings. Combined rows retain each listed criterion as a separate evaluation obligation; applicability is pending for media, motion, page/process and other scope-dependent requirements.

### Perceivable — Levels A and AA

| Criterion | Level | Evidence to collect (Not Evaluated) |
|---|---|---|
| 1.1.1 Non-text Content | A | Names for icon-only actions; text/data alternatives for charts; decorative-icon handling. Individual SVG labels are not a complete chart alternative. |
| 1.2.1 Audio-only and Video-only (Prerecorded); 1.2.2 Captions (Prerecorded); 1.2.3 Audio Description or Media Alternative (Prerecorded) | A | Inventory media in the evaluated product/integration, then assess each applicable alternative/caption obligation or justify non-applicability. |
| 1.2.4 Captions (Live); 1.2.5 Audio Description (Prerecorded) | AA | Determine media scope and retain caption/description evidence for applicable content. |
| 1.3.1 Info and Relationships | A | Inspect computed labels, grid/list semantics, grouping and validation associations in rendered states. |
| 1.3.2 Meaningful Sequence | A | Compare DOM/AT reading order with visual order, including RTL, popups and responsive layouts. |
| 1.3.3 Sensory Characteristics | A | Check instructions do not rely solely on position, shape, color, size, orientation or sound. |
| 1.3.4 Orientation | AA | Exercise portrait/landscape layouts and overlays without unnecessary orientation restrictions. |
| 1.3.5 Identify Input Purpose | AA | Verify appropriate programmatic purpose for in-scope personal-data inputs; host attribute choices matter. |
| 1.4.1 Use of Color | A | Check states, validation and chart series with color removed; CSS class names are not a user-perceivable alternative. |
| 1.4.2 Audio Control | A | Determine autoplay audio applicability and controls. |
| 1.4.3 Contrast (Minimum) | AA | Measure text/background pairs in actual themes/states: 4.5:1 normal text, 3:1 large text, with criterion exceptions documented. These are thresholds, not measured results. |
| 1.4.4 Resize Text | AA | Test text at 200% without loss of content/function; relative CSS units alone do not prove this. |
| 1.4.5 Images of Text | AA | Inventory images of text and assess customization/essential exceptions. |
| 1.4.10 Reflow | AA | Test applicable content at 320 CSS px width/256 CSS px height, including zoom and documented two-dimensional-layout exceptions. |
| 1.4.11 Non-text Contrast | AA | Measure required control/state indicators and graphical objects at 3:1 against adjacent colors; system-color names alone are not measurements. |
| 1.4.12 Text Spacing | AA | Apply specified line, paragraph, letter and word spacing overrides and check for clipping or lost functionality. |
| 1.4.13 Content on Hover or Focus | AA | Verify dismissible, hoverable and persistent tooltip/popup content, subject to the criterion's exceptions. |

### Operable — Levels A and AA

| Criterion | Level | Evidence to collect (Not Evaluated) |
|---|---|---|
| 2.1.1 Keyboard | A | Exercise every action without a pointer, including selection groups, file browse/actions, calendar views, dialogs, tooltip triggers and charts. |
| 2.1.2 No Keyboard Trap | A | Verify an available keyboard exit and focus restoration. A nonmodal popup need not trap focus; an intentional modal focus cycle must remain escapable. |
| 2.1.4 Character Key Shortcuts | A | Inventory chart and other single-character shortcuts and their focus scope, disable/remap options; do not assume every shortcut has a modifier. |
| 2.2.1 Timing Adjustable | A | Inventory host/component time limits, dismissal and timeout controls with applicable exceptions. |
| 2.2.2 Pause, Stop, Hide | A | Check duration, context and controls for moving/auto-updating content; reduced-motion CSS alone is not proof of satisfaction. |
| 2.3.1 Three Flashes or Below Threshold | A | Inspect actual animations/content for frequency and flash thresholds; no measured result is available. |
| 2.4.1 Bypass Blocks; 2.4.2 Page Titled | A | Assess host landmarks/skip mechanisms and page titles, not just isolated components. |
| 2.4.3 Focus Order | A | Verify order across open/close, dynamic updates, validation, responsive layouts and navigation. |
| 2.4.4 Link Purpose (In Context) | A | Inspect links in the integration and templates in context. |
| 2.4.5 Multiple Ways | AA | Assess navigation within the relevant set of pages, including exceptions. |
| 2.4.6 Headings and Labels | AA | Check descriptive visible/programmatic labels and headings, including localization. |
| 2.4.7 Focus Visible | AA | Observe all keyboard targets in actual themes, including styled selection labels and SVG controls. |
| 2.4.11 Focus Not Obscured (Minimum) | AA | Verify the focused component is not entirely hidden by author-created content; test overlays, sticky content and scrolling. This is the WCAG 2.2 AA addition, not 2.4.13. |
| 2.5.1 Pointer Gestures | A | Test single-pointer alternatives to multipoint/path-based gestures, subject to essential exceptions. |
| 2.5.2 Pointer Cancellation | A | Test down/up events, abort/undo and exceptions, including NumericTextBox press/release controls. Not every handler is a click handler. |
| 2.5.3 Label in Name | A | Compare computed accessible names with visible labels, particularly TimePicker, Switch and custom content. |
| 2.5.4 Motion Actuation | A | Inventory motion-triggered functionality; assess UI alternatives and disabling motion response where applicable. |
| 2.5.7 Dragging Movements | AA | For dialog dragging/resizing, upload drop, chart selection/zoom/pan and other actual drag functions, verify an equivalent **single-pointer operation without dragging**, unless essential or unmodified user-agent functionality. Keyboard-only alternatives do not satisfy this criterion. Closing a dialog is not equivalent to repositioning it. |
| 2.5.8 Target Size (Minimum) | AA | Measure actual pointer targets at least 24 × 24 CSS px, or justify a criterion exception. For spacing, 24-CSS-pixel-diameter circles centered on undersized target bounding boxes must not intersect another target or another such circle. Assess equivalent control, inline, unmodified user-agent and essential exceptions explicitly. Native controls and visual icon dimensions alone prove nothing about hit geometry. |

### Understandable and robust — Levels A and AA

| Criterion | Level | Evidence to collect (Not Evaluated) |
|---|---|---|
| 3.1.1 Language of Page | A | Verify host document language and applicable language changes across navigation. |
| 3.1.2 Language of Parts | AA | Verify language identification for mixed-language content and labels; localization alone is not the criterion. |
| 3.2.1 On Focus; 3.2.2 On Input | A | Exercise focus and input changes for unexpected context changes; include consumer event handlers. |
| 3.2.3 Consistent Navigation; 3.2.4 Consistent Identification | AA | Compare repeated navigation and same-function controls across the evaluated pages, not just CSS names. |
| 3.2.6 Consistent Help | A | If help mechanisms repeat across pages, assess their relative order. Host scope/applicability must be recorded. |
| 3.3.1 Error Identification; 3.3.2 Labels or Instructions | A | Verify identification/text descriptions of errors, required input instructions and labels in every input configuration. |
| 3.3.3 Error Suggestion | AA | Check useful correction suggestions and programmatic association with fields; attribute hooks do not author the suggestions. |
| 3.3.4 Error Prevention (Legal, Financial, Data) | AA | Assess the host process's reversal, checking or confirmation for applicable submissions. |
| 3.3.7 Redundant Entry | A | Evaluate complete processes for repeated entry and reuse/selection of previously entered information, with exceptions. |
| 3.3.8 Accessible Authentication (Minimum) | AA | Assess host authentication, cognitive-function tests, available alternatives/mechanisms and exceptions; no authentication conformance follows from providing a TextBox. |
| 4.1.2 Name, Role, Value | A | Inspect accessibility trees and AT observations for roles, names, values and state updates. Include custom templates and unlabeled/icon-only configurations. |
| 4.1.3 Status Messages | AA | Observe loading, upload, validation and dynamic-data announcements without moving focus. Native state changes need not use a live region; live-region markup does not prove usable speech. |

### Legacy and enhanced checks

| Criterion | Scope / level | Status and task |
|---|---|---|
| 4.1.1 Parsing | A in WCAG 2.0/2.1; removed from WCAG 2.2 | Not Evaluated for legacy standards scope. Consult the applicable version and current interpretation; inspect duplicate IDs, relationships and invalid markup without claiming Blazor guarantees correctness. |
| 2.4.12 Focus Not Obscured (Enhanced) | AAA, WCAG 2.2 | Not Evaluated; no part of a focused component hidden by author-created content, subject to the criterion notes. Not an AA obligation. |
| 2.4.13 Focus Appearance | AAA, WCAG 2.2 | Not Evaluated; measure focus indicator area and focused/unfocused contrast with exceptions. A two-pixel CSS declaration alone does not prove the rendered requirement. |
| 3.3.9 Accessible Authentication (Enhanced) | AAA, WCAG 2.2 | Not Evaluated; assess separately if AAA is requested. |

This is not a complete AAA checklist and makes no AAA claim. Other AAA requirements would need explicit scoping if that goal is adopted.

## WCAG 2.2 interpretation notes

The six new A/AA criteria are included in the checklist above: 2.4.11, 2.5.7, 2.5.8, 3.2.6, 3.3.7 and 3.3.8. Focus Appearance (2.4.13) is AAA. Geometry, contrast, single-pointer alternatives and focus obscuration require rendered evidence; native HTML and keyboard handlers alone cannot establish them.

The former suggestion of a 44 × 44 target requirement under EN 301 549 clause 5.7 was incorrect. WCAG 2.5.5 Target Size (Enhanced), a separate AAA criterion, uses 44 × 44 CSS px with exceptions; it must not be substituted for 2.5.8 or assigned an invented EN mapping.

## EN 301 549 v3.2.1 applicability register

All entries are **Not Evaluated; applicability pending**. This is a clause-level planning index, not a detailed conformance table. Use the official standard to enumerate applicable subclauses after defining the evaluated product, documentation and support scope. Earlier incorrect clause titles/mappings are superseded.

| Clause | Coverage to assess |
|---|---|
| 4 — Functional performance | Assess use without/with limited vision, color perception, hearing, vocal capability, manipulation/strength, reach, photosensitive triggers, cognition and privacy. Do not infer performance from ARIA attributes alone. |
| 5 — Generic requirements | Determine applicable generic requirements, including closed functionality, activation of accessibility features, biometrics and operable parts. Open source is not the definition of absence of closed functionality. |
| 6 — ICT with two-way voice communication | Inventory any in-scope communications integration; assess applicable voice and real-time text requirements or justify exclusion. |
| 7 — ICT with video capabilities | Inventory applicable video/caption/audio-description features and controls. |
| 8 — Hardware | Determine whether any hardware is in the evaluated deliverable; do not transfer a software-only assumption to a larger deployment. |
| 9 — Web | Evaluate rendered web content against the standard's WCAG 2.1-based requirements and conformance requirements; use the checklist above with the appropriate version. |
| 10 — Non-web documents | Assess in-scope documentation and exports, including accessible formats; Markdown authorship alone is not sufficient. |
| 11 — Software | Determine applicability to delivered software, AT interoperability, preferences and any authoring features. A UI library is not automatically exempt from software requirements. |
| 12 — Documentation and support services | Verify accessibility-feature documentation, accessible support documentation, and communication/support provisions. No support SLA or conformance is implied by a contact link. |
| 13 — ICT providing relay or emergency service access | Determine whether such functionality is within the product scope before assigning non-applicability. |

## Revised Section 508 applicability register

All entries are **Not Evaluated; applicability pending**. Determine the applicable Section 508 scoping provisions first (including electronic content/software/support obligations and exceptions), rather than asserting equivalence with EN 301 549.

| Coverage | Evaluation task |
|---|---|
| Chapter 3 — Functional Performance Criteria (302) | Evaluate without vision, limited vision, without color perception, without/limited hearing, without speech, limited manipulation/reach, and limited language/cognitive/learning abilities as applicable. |
| Chapter 4 — Hardware | Record a justified scope decision for hardware; no conformance inference from the web library. |
| Chapter 5 — Software | Review 501 general scope, 502 interoperability with assistive technology, 503 applications and 504 authoring tools where applicable. Use official subclauses, not the superseded mappings. |
| Chapter 6 — Support Documentation and Services | Review 602 support documentation and 603 support services, including accessible formats and effective communication. |
| Incorporated WCAG 2.0 A/AA | Assess applicable content/software using the incorporated version and scoping rules. Later WCAG criteria do not automatically amend Section 508. |

No assertion is made about evaluator credentials, legal compliance, approved waivers, or procurement eligibility.

## Component evidence register

Paths below are relative to the repository root. Related implementation files are validation starting points, not an assertion that every path/state was exhaustively audited. **Runtime status for all rows: Not Evaluated.**

| Component | Source mechanism / reference | Remaining checks and limitations |
|---|---|---|
| `SfButton` | `src/Components/Buttons/Button/SfButton.razor`: native button, hidden decorative icons, toggle/disabled ARIA. `SfButton.razor.css`: focus-visible and forced-colors rules. | Icon-only content does not create a meaningful name; supply one. Verify computed name, activation, state and visible focus in generated themes. |
| `SfButtonGroup` (+ inner `Button`) | `src/Components/Buttons/ButtonGroup/Button.razor`: native radio/checkbox inputs and associated labels in selection modes. `SfButtonGroup.razor.cs`: single-mode English group label. | Test native radio arrow behavior before alleging a missing-handler defect; measure focus on styled labels, naming/overrides and localization. |
| `SfCheckBox` | `src/Components/Inputs/CheckBox/SfCheckBox.razor`: input/label markup and checked/mixed-state attributes. | Verify DOM properties, mixed-state speech, click/keyboard behavior and error association; no extra live region is inherently required for native state changes. |
| `SfRadioButton` | `src/Components/Inputs/RadioButton/SfRadioButton.razor`: native input, shared-name hook and label. | Test grouping, arrow navigation, name, validation and disabled states. |
| `SfSwitch` | `src/Components/Inputs/Switch/SfSwitch.razor`: checkbox with switch role, name helper and state attributes. | Verify computed name, visible-label consistency, touch/keyboard behavior; no NVDA fix or observed speech result is asserted. |
| `SfTextBox` | `src/Components/Inputs/TextBox/SfTextBox.razor`: input, attribute dictionary, floating label and labeled clear button. | Check labels for every configuration, input-purpose tokens, clear/icon controls and actual error-description relationships. |
| `SfTextArea` | `src/Components/Inputs/TextArea/SfTextArea.razor`: textarea, optional floating label and clear button. | Check multiline editing, resize/zoom, naming and validation. |
| `SfNumericTextBox` | `src/Components/Inputs/NumericTextBox/SfNumericTextBox.razor`: input, labeled spin controls and polite status span. | Test value/range semantics, announcements, press/release cancellation, keyboard spin, limits and validation. |
| `SfUploader` | `src/Components/Inputs/Uploader/SfUploader.razor`: native browse button and `InputFile`, drop-area `role="region"`, polite file list, remove-action key handlers. | Earlier non-keyboard `role="button"` drop-zone allegation is not current markup. Verify browser dialog activation, cancellation, statuses, localization, custom templates and focus after removal. |
| `SfCalendar` | `src/Components/Calendars/Base/Renderer/CalendarDayCell.razor`: gridcell, selected/disabled/current state and label. | Test date names, navigation, views, localized headers and month/year announcements. |
| `SfDatePicker` | `src/Components/Calendars/DatePicker/SfDatePicker.razor` and associated ARIA/key handling; shared calendar renderer. | Verify computed input name, popup role/modality, open/close, selection, validation and focus return; absence of a modal trap is not automatically a 2.1.2 failure. |
| `SfDateTimePicker` | `src/Components/Calendars/DateTimePicker/SfDateTimePicker.razor`: date/time popup composition. | Verify both views, active versus selected time state, keyboard transitions and focus/announcements. |
| `SfTimePicker` | `src/Components/Calendars/TimePicker/SfTimePicker.razor`: input label binding, dialog/listbox/options. ARIA dictionary setup in `.razor.cs`. | Option markup lacks explicit `aria-selected`; verify the live DOM and selection model. Check label precedence/localization rather than asserting the literal dictionary label always wins. |
| `SfDialog` | `src/Components/Popups/Dialog/SfDialog.razor`: dialog role, description reference and modal ARIA; `src/wwwroot/scripts/dialog.js`: focus/key handling. | Test focus cycling/restoration, close controls, dynamic content and dragging. `alertdialog` is not mandated merely because there is one button; use appropriate semantics for the actual task. |
| `SfTooltip` | `src/Components/Popups/Tooltip/SfTooltip.razor`: tooltip role; `src/wwwroot/scripts/tooltip.js`: description association and Escape handling. | Test keyboard-reachable triggers, dismissal/hoverability/persistence and hidden state. Missing explicit `aria-hidden` or a live region alone is not proof of failure. |
| `SfSpinner` | `src/Components/Spinner/SfSpinner.razor`: status/busy, polite default live region, name and visible label. `.razor.css`: reduced-motion and forced-colors rules. | Test busy transitions and speech repetition; verify CSS selectors affect actual rendered animation. No measured motion or AT result is claimed. |
| `SfChart` | `src/Components/Charts/Common/SvgComponents/SvgPath.razor`: configurable path role/tabindex and accessibility text; chart interaction code and `src/wwwroot/scripts/chart.js`. | Validate complete chart alternatives, labels, keyboard shortcuts, zoom/selection, target geometry and dynamic updates. Individual SVG labels do not establish equivalent access to the dataset. |

## Integration and evidence requirements

Provide meaningful names, page language/structure, validation/error text, chart alternatives and keyboard-reachable help. Keep modal dialogs escapable through an available keyboard action even when Escape behavior is configured differently. Verify the exact tooltip trigger syntax/behavior in the current API and test it; do not copy the superseded `Focus+Hover` claim as a verified configuration.

Evaluate generated theme CSS and consumer overrides together, including focus on all targets, reduced motion, forced colors and speech behavior. Test static/prerender output separately from Interactive Server, WebAssembly and both Auto execution paths where applicable. See [render-mode responsibilities](../.github/RENDER-MODE-SECURITY.md). A source mechanism does not prove release inclusion or render-mode parity.

## Remaining validation work

All tasks are open; no assigned evaluator, delivery date, issue linkage or approval is implied.

- [ ] Pin exact package bytes, source commit/changes, generated assets and integration scope; record browser/OS, culture, viewport, theme, render mode and tool versions.
- [ ] Execute and retain automated scans and relevant Playwright runs, including failures, excluded rules, incomplete checks and states not exercised.
- [ ] Perform manual keyboard and single-pointer scenarios across the 17 components; validate focus order, exit/restoration, hover content and drag alternatives.
- [ ] Record real NVDA, JAWS, Narrator and VoiceOver sessions on chosen supported pairings, with expected and observed speech/focus behavior. No previous session is assumed.
- [ ] Measure contrast, target geometry/spacing, focus obscuration and any requested AAA focus appearance; record calculation inputs, screenshots/state and justified exceptions.
- [ ] Test zoom/reflow, text spacing, forced colors, reduced motion, localization, validation and dynamic changes against rendered output.
- [ ] Determine standards applicability, complete the necessary EN/508 subclause assessment and host process checks, and record supported conclusions with actual evidence.
- [ ] Only after evaluation, prepare any requested ACR using the appropriate current template and actual reviewers. Check the accessibility of Markdown rendering and any Word/PDF export. Transferring this draft into a template is not sufficient to create an approved report.

## Superseded legacy claims

The former September 2026 report dates/commit pin, engineering-team qualifications, Insights findings, NVDA/JAWS/Narrator PASS/PARTIAL matrix, VoiceOver sweeps, measured theme claims, issue #271–#273 associations, per-release review cadence, and claims of already-shipped accessibility fixes are **unverified and superseded**. They are not established historical sessions that merely became stale. Previous Supports/Partially Supports ratings and incorrect EN/508 mappings must not be reused as evidence.

This rebuild does not publish an external report, approve a waiver, certify a product, or record a maintainer sign-off. Additional evidence may support a future assessment, but none is presumed.

## Contact

Use the reporting route in [ACCESSIBILITY.md](../.github/ACCESSIBILITY.md), with reproducible environment and scenario details. Private disclosures remain **security@syncfusion.com** under [SECURITY.md](../.github/SECURITY.md).
