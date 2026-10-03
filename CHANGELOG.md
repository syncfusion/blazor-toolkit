# Changelog

All notable changes to the **Syncfusion® Toolkit for Blazor** are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Fixed
- Render internal chart components through typed render fragments instead of unresolved Razor tags, without exposing the renderer classes publicly.
- Correct nullable data, reflection, calendar, input, and dialog flows; retain null grouping keys, no-op reflection accessors, and null prompt-cancellation results.
- Preserve label/index alignment when sorting date-category chart points with null X values, and use the owning chart's theme for stripline tooltips.
- Isolate axis-overlap state per chart layout, preventing concurrent charts from clearing each other's previous-axis references and coordinates.
- Restore bUnit compilation, configure chart-only browser interop fixtures, and synchronize asynchronous rendering assertions.

### Changed
- Correct public nullable annotations to describe existing null inputs/results. CLR signatures and collection types are unchanged, but consumers may see more accurate nullable-analysis diagnostics.
- `DataAdaptor<T>` now declares `where T : notnull`, matching `OwningComponentBase<T>`. Generic subclasses must propagate this constraint; nullable type arguments or unconstrained type parameters may produce nullable-analysis warnings when consumers rebuild (errors under warnings-as-errors policies). This compiler-level contract does not add a CLR-enforced runtime constraint or change service scope/disposal behavior.
- `QueryableOperation` filtering/searching over `DynamicObject` records now throws a descriptive `InvalidOperationException` when a requested field is null or missing in the first record, rather than dereferencing the missing value. It does not infer types from later records; the existing `ExpandoObject` path is unchanged.

### Planned
See [ROADMAP.md](ROADMAP.md) for upcoming components (Data Grid, Select/DropDownList, Autocomplete, Navigation, and more).

## [1.0.0] - 2026-10-01

Initial public release of the open-source, MIT-licensed toolkit.

### Added
- **Data Viz** — `SfChart` with line, area, column/bar, scatter, bubble, spline, and stacking series.
- **Buttons** — `SfButton`, `SfButtonGroup`, `SfCheckBox`, `SfRadioButton`, and toggle `SfSwitch`.
- **Calendars** — `SfCalendar`, `SfDatePicker`, `SfDateTimePicker`, and `SfTimePicker` with culture and range support.
- **Inputs** — `SfTextBox`, `SfTextArea`, `SfNumericTextBox`, and `SfUploader` (file upload).
- **Popups / Layout** — `SfDialog` (modal) and `SfTooltip`.
- **Notification** — `SfSpinner` loading indicator.
- Support for **.NET 8, .NET 9, and .NET 10** across Blazor Server, WebAssembly, and Auto render modes.
- Accessibility conformance with WCAG, keyboard navigation, and ARIA support; published [VPAT](docs/VPAT-2.5-INT.md).
- Security and supply-chain assurance: CodeQL scanning, NuGet vulnerability auditing, threat model, and a published SBOM.
- Trimming and AOT compatibility (`IsTrimmable`, `IsAotCompatible`) with trim/AOT analyzers enabled.
- `Syncfusion.Blazor.Toolkit.Templates` project templates (`blazortoolkitweb`, `blazortoolkitwasm`).

[Unreleased]: https://github.com/syncfusion/blazor-toolkit/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.0
