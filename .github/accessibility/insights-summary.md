# Accessibility Insights — summary

This file summarises the Accessibility Insights FastPass and
Assessment results for major components of the Syncfusion Blazor
Toolkit. Each test run produces a per-component JSON report under
`tests/accessibility/insights/`; a human-readable rolled-up summary
is updated here at every major release.

## Latest run

| Field | Value |
|---|---|
| Run date | 2026-09-06 |
| Tool | Accessibility Insights for Web (v3.0.0) |
| Browser | Microsoft Edge Stable 130 |
| Render mode | Interactive Server |
| Components swept | 17 (SfButton, SfButtonGroup, SfCheckBox, SfRadioButton, SfSwitch, SfTextBox, SfTextArea, SfNumericTextBox, SfUploader, SfCalendar, SfDatePicker, SfDateTimePicker, SfTimePicker, SfDialog, SfTooltip, SfSpinner, SfChart) |
| Total FastPass findings | 0 critical; 0 serious; 3 moderate (filed as issues labelled `accessibility`); 0 minor |
| Total Assessment findings | 0 serious; 5 moderate (filed as issues labelled `accessibility`); 12 minor (filed) |

### Findings by component

| Component | FastPass | Assessment | Severity | Issue |
|---|---:|---:|---|---|
| SfUploader | 1 | 0 | Moderate | `input[type='file']` lacks visible button-text rename on Firefox — [`#271`](https://github.com/syncfusion/blazor-toolkit/issues/271) |
| SfTooltip | 0 | 2 | Moderate | Long tooltip on hover does not surface as live region — [`#272`](https://github.com/syncfusion/blazor-toolkit/issues/272) |
| SfCalendar | 2 | 3 | Moderate/Minor | Arrow-key navigation is not announced by all screen readers — [`#273`](https://github.com/syncfusion/blazor-toolkit/issues/273) |
| SfChart | 0 | 0 | — | No outstanding issues |
| others | 0 | 0 | — | No outstanding issues |

## Continuous integration

A nightly run of Accessibility Insights against the unpacked WebAssembly
sample initiates a job that is intentionally not in PR CI (it is too
slow and too environment-flavour-sensitive to block PRs). Results are
uploaded to issue-tracking via the `accessibility-bot` GitHub Action,
which files new issues and updates the per-component table above.

## Approach summary

- Automated FastPass scans every PR that touches a component file
  (`src/Components/**/*Members.cs`, `*.cs`, `*.razor`).
- Manual Assessment runs every quarter against the latest minor
  release cycle on Windows + Edge + NVDA, with parallel sweeps on
  macOS + Safari + VoiceOver for parity.

## Contact

Report a new finding as an issue with the `accessibility` label. Private
disclosures can be sent to security@syncfusion.com per SECURITY.md.