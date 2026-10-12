# Roadmap — Syncfusion® Toolkit for Blazor

**Chart-first, quality before breadth.** The requesting user approved the scope during R01 on 2026-10-06: Chart is the flagship; the MIT toolkit remains independently useful; **no Grid is planned**. Growth toward 40+ supporting components is exploratory, without a promised list or dates. This is not approval of a release schedule, compatibility guarantee or funded support commitment. Component selection and timing remain subject to maintainer review and community feedback.

[README](README.md) · [Changelog](CHANGELOG.md) · [Documentation index](.github/Index.md) · [Test evidence matrix](.github/TEST-MATRIX.md)

## Present in the current source

| Area | Current scope |
|---|---|
| Chart | 21 core/variant series types: line, area, column/bar, scatter, bubble, spline, step, multicolored and stacking variants; axes, legends, tooltips, selection, zoom/pan, annotations, data labels, trendlines and striplines. |
| Buttons and selection | Button, ButtonGroup, CheckBox, RadioButton and Switch. |
| Calendars | Calendar, DatePicker, DateTimePicker and TimePicker. |
| Inputs | TextBox, TextArea, NumericTextBox and Uploader. |
| Popups and notification | Dialog, Tooltip and Spinner. |

Chart annotations already exist; they are not a future addition. Pie/doughnut, radar, financial series and technical indicators are not part of the current Toolkit Chart API. There is no Data Grid component or Data Grid delivery commitment in this roadmap.

The library source targets **.NET 8 / 9 / 10**. Render-mode requirements vary by component; target frameworks and sample hosts do not prove equivalent behavior in every mode. See the [component guidance](README.md#skills-and-component-guidance) and [render-mode guidance](.github/RENDER-MODE-SECURITY.md).

## Priorities to pursue

| Order | Focus | Scenarios and evidence sought |
|---|---|---|
| 1 | Chart reliability and usability | Null/empty data, sorting, concurrent chart layouts, live updates and disposal; reproducible regressions and measurements for larger datasets. No virtualization or performance threshold is promised. |
| 2 | Accessibility in composed applications | Chart text alternatives, keyboard/focus behavior, labels and validation, forced-colors themes and screen-reader checks; retain browser/AT versions and results. |
| 3 | Existing component quality | Calendar culture/date boundaries, input validation, upload failure handling, popup focus/exit behavior and loading announcements. |
| 4 | Broader validation and release evidence | Resolve SDK/runtime job gaps, extend browser/render-mode checks where supported, evaluate trimmed/WebAssembly AOT publishing, and verify package-specific metadata and dependency evidence. |
| 5 | Supporting component breadth | Explore additions that complement charts and common forms/application layouts, only after reviewing scope, maintenance cost and evidence needs. |

## Longer-term direction: 40+ components

Growing toward **40+ components** is a direction for exploration, not a committed inventory, exact component list, release count or delivery date. Proposals for supporting controls, theming, localization and RTL should demonstrate a concrete application need and include validation plans. This direction does not commit to Data Grid or to feature parity with the commercial Syncfusion suite.

## Current evidence versus planned validation

| Area | Evidence available now | Follow-up needed |
|---|---|---|
| Build and unit tests | [R00, 2026-10-06, commit `1922761`](docs/evidence/2026-10-06-r00-1922761/index.md): library .NET 8/9/10 build **0 warnings/errors with compiler-server reuse disabled**; .NET 8 bUnit **2,735 passed twice**, no failures/skips. | Default-build failures and 3,537 bUnit build-warning occurrences remain; samples built with 232 warnings. Broader runtime evidence and released-package checks remain open. |
| Browser coverage | R00 discovered 2,421 cases and passed four basic Chart rendering cases on .NET 10 Interactive Server/Chromium. | Remaining 2,417 cases unrun; meaningful interaction, additional applicable hosts/browsers and full required matrix remain open. |
| Accessibility | Source mechanisms and [draft evidence registers](.github/ACCESSIBILITY.md) exist; WCAG 2.2 AA is a goal. | Measured application-level evaluations; the [draft standards register](docs/VPAT-2.5-INT.md) is not an approved VPAT/ACR. |
| Trimming / AOT | Project declarations and analyzers are present. | Supported-host publish logs and runtime checks; analyzer-clean builds are not deployment certification. |
| Security / supply chain | R00 retained scan results: npm reported 11 high affected-package entries; top-level SBOM checksum mismatch reproduced. | Dependency triage, shipped-script coverage and final-package checks; conditional generation and empty release asset lists do not establish per-release SBOM or signing. |
| API compatibility | [Migration notes](CHANGELOG.md) document known changes. | Maintainer review and compatibility evidence; adoption of commercial controls may require API, markup, styling and licensing changes. |

These are retained R00 results, not new execution during the R01 documentation review, remote CI results or release guarantees. See [DEVELOPMENT.md](.github/DEVELOPMENT.md) and [TEST-MATRIX.md](.github/TEST-MATRIX.md) for scope and gaps. Historical releases are recorded separately in the [changelog](CHANGELOG.md).

## How to influence priorities

Open or upvote a [feature request](https://github.com/syncfusion/blazor-toolkit/issues/new/choose), or start a [discussion](https://github.com/syncfusion/blazor-toolkit/discussions). Include the user scenario, current limitation, render mode, expected behavior and any reproduction or measurements. Priority depends on user impact, accessibility/security risk, maintenance cost and available validation—not component count alone.

_Last evidence review: 2026-10-06, R01; this date does not approve a delivery schedule._
