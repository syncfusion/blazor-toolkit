# Roadmap — Syncfusion® Toolkit for Blazor

**Chart-first, quality before breadth.** Chart is the flagship; the MIT toolkit remains independently useful; **no Data Grid is planned**. Growth toward 40+ supporting components is exploratory, without a promised list or dates. This is not approval of a release schedule, a compatibility guarantee, or a funded support commitment. Component selection and timing remain subject to maintainer review and community feedback.

[README](README.md) · [Changelog](CHANGELOG.md) · [Documentation index](.github/Index.md) · [Test evidence matrix](.github/TEST-MATRIX.md)

## How this document was improvised

This revision keeps the chart-first charter and absorbs only competitor patterns that fit an MIT, quality-gated toolkit. It does **not** adopt competitor shipping cadence, component counts, or product categories that conflict with current source or stated non-goals.

| Pattern considered | Fit | What this roadmap does |
|---|---|---|
| Chart series expansion (pie/doughnut, radar, financial, waterfall, heatmap, range) | Suitable as evaluation only | Listed under chart evolution. Not a delivery item. The public `ChartSeriesType` enum does not include these values. |
| Accessibility evidence over conformance claims | Suitable | Remains priority 2. WCAG 2.2 AA stays a goal; measured application-level results stay the gate. |
| Trimming / WebAssembly AOT publish evidence | Suitable | The library already declares `IsTrimmable` and `IsAotCompatible` and enables trim/AOT analyzers. Missing piece is supported-host publish logs and runtime checks, not a new feature. |
| CSS-variable theming and opt-in alternate themes | Suitable, scoped | The shared theme already emits `:root` tokens plus dark, high-contrast, and forced-colors rules through `SfThemeRoot`. Material 3, Fluent 2, and Bootstrap-aligned themes are evaluation only and must stay opt-in. |
| Localization and RTL | Suitable after quality gates | Culture-aware date/number formatting already exists. A first-class RTL surface is not in the current public API, so it stays an evaluation track. |
| Layout primitives that complete chart dashboards and forms | Suitable if gated | Tab, list, menu, and dropdown/autocomplete stay planned for evaluation. Sidebar and Card are not in source. |
| Supply-chain and dependency triage as a standing gate | Suitable | Already required by [SECURITY.md](.github/SECURITY.md) monthly servicing and the committed SBOM. Kept as a gate, not a new product track. |
| Community influence via issues and discussions | Suitable | Unchanged. Proposals must include scenario, render mode, expected behavior, and measurements. |
| AI chat, tool calling, smart paste, MCP documentation helpers | Not suitable now | No AI surface exists in this repository. Adding one before chart and accessibility evidence is solid would widen maintenance without serving the flagship. |
| Data Grid, TreeGrid, Pivot, Spreadsheet, Gantt, Scheduler | Not suitable | Explicit non-goals. These belong to the commercial suite or to a different maintenance profile. |
| Multi-CSS provider model (Bootstrap, Tailwind, Material, Ant Design as peers) | Not suitable | The toolkit ships one tokenized theme with contrast variants. A provider matrix would multiply theme, a11y, and test cost. |
| Aggressive series parity or dated component lists | Not suitable | Inclusion here is exploration only. No dates, no compatibility promise, no feature parity with other libraries. |

## Present in the current source

Checked against package **2.0.1** source. Public component types live under `Syncfusion.Blazor.Toolkit`.

| Area | Current scope |
|---|---|
| Chart | 21 public `ChartSeriesType` values: line, area, column, bar, scatter, bubble, spline, spline area, step line, step area, multicolored line/area, and stacking variants (including 100% stacking and stacking step area). Axes, legends, tooltips, selection, zoom/pan, annotations, data labels, trendlines, and striplines are already in source. |
| Buttons and selection | Button, ButtonGroup, CheckBox, RadioButton, and Switch. |
| Calendars | Calendar, DatePicker, DateTimePicker, and TimePicker. |
| Inputs | TextBox, TextArea, NumericTextBox, and Uploader. |
| Popups and notification | Dialog, Tooltip, and Spinner. `Popups/Popup` currently holds a model only; it is not a shipped `SfPopup` component. |
| Theme | `SfThemeRoot` emits one shared CSS-variable layer (default, dark, high-contrast, forced-colors). Component CSS is isolated. Alternate design-system themes are not shipped. |
| Targets | Library source targets **.NET 8 / 9 / 10**. Render-mode requirements vary by component; target frameworks and sample hosts do not prove equivalent behavior in every mode. See [component guidance](README.md#skills-and-component-guidance) and [render-mode guidance](.github/RENDER-MODE-SECURITY.md). |

Chart annotations, trendlines, and striplines already exist; they are not future additions. Pie/doughnut, radar/spider, polar, financial series (candlestick/OHLC), waterfall, heatmap, range series, and technical indicators are **not** part of the current Toolkit Chart API. There is no Data Grid component and no Data Grid delivery commitment.

## Priorities to pursue

Order is unchanged. Competitor breadth does not reorder these gates. Priority depends on user impact, accessibility and security risk, maintenance cost, and available validation — not component count.

| Order | Focus | Scenarios and evidence sought |
|---|---|---|
| 1 | Chart reliability and usability | Null/empty data, sorting, concurrent chart layouts, live updates, and disposal; reproducible regressions and measurements for larger datasets. No virtualization or performance threshold is promised. |
| 2 | Accessibility in composed applications | Chart text alternatives, keyboard/focus behavior, labels and validation, forced-colors themes, and screen-reader checks; retain browser and assistive-technology versions and results. |
| 3 | Existing component quality | Calendar culture and date boundaries, input validation, upload failure handling, popup focus and exit behavior, and loading announcements. |
| 4 | Broader validation and release evidence | Resolve SDK/runtime job gaps, extend browser and render-mode checks where supported, retain trimmed/WebAssembly AOT publish logs and runtime checks, and verify package metadata and dependency evidence. |
| 5 | Supporting component breadth | Explore additions that complement charts and common forms or application layouts, only after reviewing scope, maintenance cost, and evidence needs. |

## Chart evolution

Chart stays the flagship. New series are evaluation candidates, not commitments. A candidate moves forward only when a proposal includes a concrete user scenario, a render-mode matrix, an accessibility plan, and reproducible performance notes. Live-update and disposal evidence for the series already shipped stays ahead of new types.

| Candidate | Why it is on the list | Gate |
|---|---|---|
| Pie / doughnut | Common composition next to the existing cartesian series; not in `ChartSeriesType`. | Scenario + a11y text alternative + empty-data behavior. |
| Radar / spider | Asked for in comparable chart libraries; different axis model from the current cartesian chart. | Do not start until cartesian reliability evidence in priority 1 is in hand. |
| Basic financial (candlestick / OHLC) | Distinct from a full stock-chart suite. | Require a bounded scenario. Technical indicators and a stock-chart shell stay out of scope. |
| Waterfall, heatmap, range variants | Useful dashboard complements, each with its own layout and a11y cost. | Evaluate one at a time. Do not batch them into a series-parity release. |

Out of scope unless community demand and a validation plan are both strong: technical indicators, a full stock-chart suite, 3D charts, and any promise of virtualization or a performance threshold.

## Supporting components

These names are exploration and planning only. Sidebar, Card, Tab, dropdowns, List, AutoComplete, and Menu are **not** present as public components in the current source. Popup is a model under `Popups/Popup`, not a finished component. Order, scope, and timing may change.

| Status | Components | Notes |
|---|---|---|
| In progress | Sidebar, Card, Popup | Not shipped. Each needs a public API sketch, accessibility plan, and render-mode evidence before it can be treated as implemented. |
| Planned for evaluation | Tab, Dropdowns (select / autocomplete), List, AutoComplete, Menu | Prefer controls that complete common form and chart-dashboard layouts. |
| Future opportunities | Additional navigation and layout (app bar, drawer variants), data-entry helpers, and a basic timeline only if evidence justifies it. | Scheduler-like, Gantt, spreadsheet, and grid-family controls are not in this set. |

Every candidate must answer this checklist before implementation starts:

- What user scenario does it serve that existing Dialog, Tooltip, inputs, or Chart cannot?
- Which render modes are in scope, and which are explicitly unsupported?
- What is the accessibility plan (name, role, keyboard, focus exit, forced colors)?
- What is the bundle-size and theme-surface impact on the single tokenized theme?
- What localization or RTL surface does it add, if any?
- Does it reduce pressure to add a Data Grid, or does it quietly become one?

Inclusion in this table is not a delivery commitment, release guarantee, compatibility promise, or implementation timeline.

## Cross-cutting tracks

These are evaluation tracks. They do not outrank priorities 1–4.

- **Theming.** Keep the current tokenized theme (default, dark, high-contrast, forced-colors) as the default. Material 3, Fluent 2, or Bootstrap-aligned themes may be evaluated only as opt-in packs that reuse the existing CSS variables. Do not adopt a multi-provider CSS architecture.
- **Localization and RTL.** Keep improving culture and date-boundary evidence for calendars and numeric inputs. A documented RTL/direction story is in scope only after the quality gates above are met. Do not claim RTL support that the public API does not expose.
- **Trimming, AOT, and bundle size.** Declarations and analyzers are already enabled. The follow-up is supported-host publish logs and runtime checks. Analyzer-clean builds are not deployment certification.
- **Supply chain.** Continue monthly dependency triage, SBOM checksum review, and shipped-script coverage described in [SECURITY.md](.github/SECURITY.md) and [artifacts/sbom/README.md](artifacts/sbom/README.md). This is a standing release gate.
- **AI-assisted experiences.** Not in scope for this toolkit at this time. Optional prompt surfaces, tool calling, or documentation helpers will not be evaluated until chart reliability and accessibility evidence are solid, and any future surface would have to stay optional and provider-agnostic. This is a deferral, not a roadmap item.

## Longer-term direction

Growing toward **40+ components** is a direction for exploration, not a committed inventory, release count, or delivery date. Proposals for supporting controls, theming, localization, and RTL must show a concrete application need and a validation plan. This direction does not commit to Data Grid, TreeGrid, Pivot, or feature parity with the commercial Syncfusion suite.

Explicit non-goals:

- Data Grid or any grid family.
- Feature parity with the commercial Syncfusion Blazor suite, or with any competitor's component list.
- Promised virtualization or numeric performance thresholds.
- AI chat or tool-calling components.
- A multi-CSS provider matrix.

## Current evidence versus planned validation

Figures below are the retained evidence already recorded in this repository. They are not a new test run, not remote CI results, and not release guarantees. Re-count after meaningful test changes using the command in [TEST-MATRIX.md](.github/TEST-MATRIX.md). Historical releases are recorded in [CHANGELOG.md](CHANGELOG.md).

| Area | Evidence available now | Follow-up needed |
|---|---|---|
| Build and unit tests | [TEST-MATRIX.md](.github/TEST-MATRIX.md): bUnit **2,735** tests passed (0 failed, 0 skipped) on .NET 8 as of the 2026-10-03 development verification note. CI is declared for bUnit on .NET 8/9/10. Library version is **2.0.1**. | Re-count after test changes. Broader runtime evidence and released-package checks remain open. The 2026-10-06 R00 links, warning totals, and browser case counts from an earlier draft are **not** in this repository and are not repeated here. |
| Browser coverage | Playwright projects exist for visual regression and accessibility smoke; CI lists them for .NET 8/9/10. | Interaction coverage, additional hosts/browsers, and the full required matrix remain open. See [TEST-MATRIX.md](.github/TEST-MATRIX.md). |
| Accessibility | Source mechanisms, [ACCESSIBILITY.md](.github/ACCESSIBILITY.md), screen-reader smoke notes, and [docs/VPAT-2.5-INT.md](docs/VPAT-2.5-INT.md) exist. WCAG 2.2 AA is a goal, last reviewed for package 2.0.1. | Measured application-level evaluations. A draft or in-repo VPAT is not by itself an approved ACR for every composed application. |
| Trimming / AOT | `IsTrimmable`, `IsAotCompatible`, and trim/AOT analyzers are set in the library project. | Supported-host publish logs and runtime checks. Analyzer-clean builds are not deployment certification. |
| Security / supply chain | [SECURITY.md](.github/SECURITY.md) (self-attestation 2026-10-19), [THREAT-MODEL.md](.github/THREAT-MODEL.md), monthly `dotnet list package --vulnerable` servicing, NuGet audit on the library, and a committed CycloneDX SBOM under [artifacts/sbom/](artifacts/sbom/README.md). | Ongoing dependency triage, shipped-script coverage, and final-package checks. A committed SBOM does not by itself certify every release asset. |
| API compatibility | [CHANGELOG.md](CHANGELOG.md), [API-COMPATIBILITY.md](docs/API-COMPATIBILITY.md), and [API-BREAK-INVENTORY.md](docs/API-BREAK-INVENTORY.md) document the 2.0 contract. | Maintainer review for any further breaks. Adopting commercial controls may require API, markup, styling, and licensing changes. |
| Theming | `SfThemeRoot` tokens plus dark, high-contrast, and forced-colors rules ship with 2.0.x. | No alternate design-system theme is evidenced. Opt-in themes need their own contrast and forced-colors checks before they can be called supported. |

## How to influence priorities

Open or upvote a feature request (https://github.com/syncfusion/blazor-toolkit/issues/new/choose), or start a discussion (https://github.com/syncfusion/blazor-toolkit/discussions). Include the user scenario, current limitation, render mode, expected behavior, and any reproduction or measurements. Priority depends on user impact, accessibility and security risk, maintenance cost, and available validation — not component count alone.

Community feedback can reorder or de-scope any exploratory item in this document. It cannot turn a non-goal into a commitment.

_Last evidence review: 2026-10-19, aligned to package 2.0.1 and in-repo evidence. This date does not approve a delivery schedule._
