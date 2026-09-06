# Accessibility conformance statement

The Syncfusion Blazor Toolkit **aims to conform** to **WCAG 2.2 Level AA**.

## What this means

- Every interactive component in the toolkit exposes accessible
  names and roles (`aria-label`, `role`) consistent with WAI-ARIA 1.2.
- Keyboard navigation follows the WAI-ARIA Authoring Practices for the
  relevant widget pattern (`tab`, `shift+tab`, `enter`, `escape`).
- Color contrast in the default `e-lib` theme meets or exceeds 4.5:1
  for normal text and 3:1 for large text against the standard
  backgrounds.
- Components that expose a customisable live region (Spinner, Dialog)
  use the polite live region by default and only flip to assertive on
  an explicit configuration.

## Known limitations

Known limitations are tracked as GitHub issues
labelled [`accessibility`](https://github.com/syncfusion/blazor-toolkit/issues?q=is%3Aopen+is%3Aissue+label%3Aaccessibility).
Each issue lists the affected component, the WCAG Success Criterion
that is not yet satisfied, and the planned remediation.

## Evidence

- Accessibility Insights FastPass / Assessment reports for major
  components are checked in under
  [`.github/accessibility/insights-summary.md`](accessibility/insights-summary.md).
- Manual screen reader smoke notes for NVDA / JAWS / Narrator against
  the major components live next to it at
  [`.github/accessibility/screen-reader-smoke.md`](accessibility/screen-reader-smoke.md).
- Conformance claims are regenerated before each major release and
  filed in the release ticket.

## Reporting issues

If you find an accessibility bug, file a new issue with the
`accessibility` label. Include the operating system, browser / screen
reader pairing, the component affected, and the WCAG success criterion
that is failing.

For private disclosure, contact security@syncfusion.com following
[SECURITY.md](SECURITY.md).