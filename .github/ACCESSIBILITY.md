# Accessibility status — DRAFT evidence register

The Syncfusion Blazor Toolkit **aims to support WCAG 2.2 Level AA**. That is a design goal, **not a verified conformance claim, approved Accessibility Conformance Report (ACR), or certification**. The current evidence establishes selected source mechanisms and test definitions; it does not establish end-to-end accessibility of the library, a published package, or a consuming application.

## Source-supported mechanisms

- Native button/input/textarea markup and label hooks exist, for example in `src/Components/Buttons/Button/SfButton.razor` and `src/Components/Inputs/TextBox/SfTextBox.razor`.
- Calendar cells expose grid states in `src/Components/Calendars/Base/Renderer/CalendarDayCell.razor`; modal Dialog markup supplies `role="dialog"` and `aria-modal`, with focus/key handling in `src/wwwroot/scripts/dialog.js`.
- `src/Components/Spinner/SfSpinner.razor` uses `role="status"`, `aria-busy`, and a configurable live region defaulting to polite. `SfNumericTextBox.razor` also has a polite status region. This does **not** establish a common live-region default for Dialog or every component.
- `SfButton.razor.css` contains focus-visible and forced-colors rules; `SfSpinner.razor.css` contains reduced-motion rules. Rule presence does not prove selector coverage, rendered contrast, or assistive-technology behavior.

See the [DRAFT standards and component register](../docs/VPAT-2.5-INT.md) for source paths, evaluation tasks, and limitations. It retains the historical filename for discoverability but is not an approved VPAT/ACR.

## Limitations and evidence gaps

- Icon-only buttons need a meaningful accessible name supplied by the application; native semantics alone do not provide it.
- Labels, validation associations, popup focus/exit behavior, tooltip triggers, chart alternatives, and status announcements require testing in the actual configuration.
- Generated themes and consumer overrides must be evaluated. No measured contrast ratios, target sizes, zoom/reflow results, or complete keyboard/AT matrix are established here. `e-lib` is not evidence of a measured theme palette.
- SSR/prerender, Interactive Server, WebAssembly, and both Auto execution paths need separate coverage where applicable; static markup does not imply functional parity.

The [Insights register](accessibility/insights-summary.md) and [screen-reader checklist](accessibility/screen-reader-smoke.md) record **Not Evaluated** runtime status and missing artifacts. Playwright accessibility specs exist, but test source is not a retained successful run or a manual AT evaluation. No automatic release-by-release conformance regeneration is established.

**Legacy claims superseded/unverified:** earlier PASS/PARTIAL matrices, claimed September 2026 Insights and NVDA/JAWS/Narrator sessions, measured contrast, issue-number associations, evaluator credentials, nightly accessibility-bot runs, and sign-offs are not supported by retained evidence reviewed here. They are not treated as historical sessions that merely need refreshing.

## Reporting issues

Open an issue describing the accessibility problem; use the [`accessibility` label](https://github.com/syncfusion/blazor-toolkit/issues?q=is%3Aopen+is%3Aissue+label%3Aaccessibility) where available. Include the package/source version, component/sample, render mode, theme, operating system, browser/AT versions, steps, and expected/actual behavior. Include a WCAG criterion if known; identifying one is not a prerequisite to reporting.

For private disclosure, retain **security@syncfusion.com** following [SECURITY.md](SECURITY.md). No response SLA, evaluator approval, or certification is created by this status document.