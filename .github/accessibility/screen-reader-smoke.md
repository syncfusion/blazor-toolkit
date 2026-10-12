# Screen-reader smoke — DRAFT validation checklist

**Not Evaluated:** no retained session records establish the former NVDA, JAWS, or Narrator results. VoiceOver results are also unverified. This is a checklist for a future evaluation, not a record that testing occurred. FastPass/Assessment is not a substitute for observing an actual screen reader.

## Component scenarios

For **every row**, NVDA, JAWS, Narrator, and VoiceOver status is **Not Evaluated**. Select and record actual OS/browser/AT versions and supported pairings when testing; no versions or evaluators are implied by the column scope.

| Component | Scenario to exercise and record |
|---|---|
| `SfButton` | Text and icon-only names; Enter/Space; toggle and disabled states. |
| `SfButtonGroup` | None/single/multiple modes; group/item names; native radio navigation; selected state and visible focus. |
| `SfCheckBox` | Label, checked/mixed/disabled states; keyboard activation; validation association. |
| `SfRadioButton` | Shared-name group navigation; selected state and label; validation association. |
| `SfSwitch` | Accessible name versus visible on/off labels; switch state and keyboard activation. |
| `SfTextBox` | Visible/programmatic labels, clear button, validation errors and descriptions. |
| `SfTextArea` | Label, multiline editing, clear button and error association. |
| `SfNumericTextBox` | Value/range announcements, spin buttons, keyboard changes, invalid input and status-region repetition. |
| `SfUploader` | Browse/file input, selection/removal, localized instructions, progress/error status, focus after list changes. |
| `SfCalendar` | Selected/current/disabled dates; month/year changes and keyboard navigation announcements. |
| `SfDatePicker` | Input name/value, open/close, grid navigation, focus return and selected date. |
| `SfDateTimePicker` | Switching date/time views; active versus selected options, validation, focus and value announcements. |
| `SfTimePicker` | Computed name with supplied labels; active/selected list options; open/close and value changes. |
| `SfDialog` | Name/description, initial focus, modal cycling, dismissal and restoration, nested/dynamic content. |
| `SfTooltip` | Focus and pointer triggers, description association, dismissal and persistence; long/sticky content. |
| `SfSpinner` | Busy transitions and status announcement; visible label versus accessible name; repeated updates. |
| `SfChart` | Summary/data alternatives, series/point names, keyboard traversal, selection/zoom, dynamic updates. |

## Required session record

- Exact package/source revision (and local modifications), tested URL/fixture and component parameters.
- Date and actual evaluator, OS/browser/AT versions, verbosity/settings, locale, theme, viewport, and render mode/prerender phase.
- Steps and expected behavior, then verbatim observed announcements and focus behavior, with evidence references where available.
- Result per scenario: observed success, failure, blocked, or not evaluated. A smoke success is not full criterion conformance.
- Reproduction and confirmed issue links for failures; no assumed remediation date or approval.

Run scenarios on real sample routes from the checked-out application. No `/accessibility/smoke` route or bundled Insights installation is established by this document. Use [DEVELOPMENT.md](../DEVELOPMENT.md) for host setup, then record the exact route used. Cover Server, WebAssembly, and both Auto hosts where supported; distinguish static/prerender markup from interactive behavior.

## Limitations and superseded claims

Source mechanisms and open questions are recorded in [the DRAFT standards register](../../docs/VPAT-2.5-INT.md). Native state semantics can provide announcements without a live region; a missing `aria-live` is not automatically a failure. Conversely, live-region markup does not prove reliable or nonduplicated speech.

Earlier PASS/PARTIAL entries, the claimed September 2026 sessions, release-by-release reverification, Calendar timing differences between screen readers, and Welsh/Lithuanian file-input behavior are **unverified and superseded**. They are not established historical observations. Issue #272/#273 associations likewise need verification before reuse.

Report findings under [ACCESSIBILITY.md](../ACCESSIBILITY.md). Private disclosure remains **security@syncfusion.com** per [SECURITY.md](../SECURITY.md).