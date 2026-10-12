# Changelog

Notable changes to the **Syncfusion® Toolkit for Blazor**, with historical summaries attributed to upstream release notes.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/). Historical version labels are preserved as published; see the [development guide](.github/DEVELOPMENT.md#versioning-and-evidence-status) for versioning and compatibility guidance.

## [Unreleased]

### Compatibility preparation
- Source version is now **2.0.0**, explicitly authorized for the uncommitted major-version change; no 2.0 release or publication is implied. [API migration policy](docs/API-COMPATIBILITY.md) documents the exact 194 approved existing diagnostic/target pairs against hash-pinned NuGet 1.0.2, including separately approved getter-only collection ownership.
- Added a pinned released-API gate and independent current public/protected/Razor/callback/JSON guard. Two deliberate compiled regressions were rejected; CI pack/summary depend on the check. Actual final package validation remains open.
- Preserve released `Syncfusion.Blazor.Toolkit.Data.SfDataManager` identity with an explicit Razor namespace; correct the Dialog IList-return example. Planned CSS isolation is not implemented or certified by this API change.

### Fixed
- Render internal chart components through typed render fragments instead of unresolved Razor tags, without exposing the renderer classes publicly.
- Correct nullable data, reflection, calendar, input, and dialog flows; retain null grouping keys, no-op reflection accessors, and null prompt-cancellation results.
- Preserve label/index alignment when sorting date-category chart points with null X values, and use the owning chart's theme for stripline tooltips.
- Isolate axis-overlap state per chart layout, preventing concurrent charts from clearing each other's previous-axis references and coordinates.
- Restore bUnit compilation, configure chart-only browser interop fixtures, and synchronize asynchronous rendering assertions.

### Changed
- Uploader default filename, extension and status display treats metadata as text in the uncommitted source fix. The user-approved correction preserves original selected filenames in callbacks instead of numeric-entity rewriting while retaining optional invalid-name validation. Templates remain application-owned. See [R05 behavior and evidence](docs/API-COMPATIBILITY.md#r05-selected-filename-behavior-correction--2026-10-06); private security review and released-package validation remain pending.
- Correct public nullable annotations to describe existing null inputs/results. Annotation-only edits do not themselves change CLR types, but **the complete Unreleased surface is not compatible with released 1.0.2**: collection signatures and public type visibility also changed. See [R03a's measured comparison and consumer failures](docs/evidence/2026-10-06-r03a-1922761/index.md); no blanket compatibility assurance is intended.
- `DataAdaptor<T>` now declares `where T : notnull`, matching `OwningComponentBase<T>`. Generic subclasses must propagate this constraint; nullable type arguments or unconstrained type parameters may produce nullable-analysis warnings when consumers rebuild (errors under warnings-as-errors policies). This compiler-level contract does not add a CLR-enforced runtime constraint or change service scope/disposal behavior.
- Released `SfDialog.GetButtonItems()` returns `List<DialogButton>?`; current source returns `IList<DialogButton>?`. A net8 consumer assigning the result to `List<DialogButton>?` compiled against 1.0.2 and failed against current source with CS0266. Other collection/accessibility differences require individual migration review; the R03a major-release direction is not blanket approval of them.
- `QueryableOperation` filtering/searching over `DynamicObject` records now throws a descriptive `InvalidOperationException` when a requested field is null or missing in the first record, rather than dereferencing the missing value. It does not infer types from later records; the existing `ExpandoObject` path is unchanged.

### Documentation
- Synchronize release summaries with GitHub, correct build/onboarding and component API examples, and align the roadmap with the approved Chart-first direction without a Grid commitment.
- Separate configured checks, recorded local results, release evidence and planned validation. Replace unsupported security, accessibility, signing/SBOM and support guarantees with explicit evidence boundaries.
- Preserve the historical README/Toolkit 1.0.2 compile-check narrative, noting that its original consumer/build log was not located. Executable package-onboarding evidence remains an R20 requirement.
- Reconcile public guidance with the retained [2026-10-06 R00 baseline](docs/evidence/2026-10-06-r00-1922761/index.md), including failed default builds, bounded test scope, dependency findings and the SBOM mismatch. No artifact/version change or release assurance follows from R01 documentation corrections.

### Direction
See [ROADMAP.md](ROADMAP.md) for Chart-first priorities and longer-term exploration, not a committed component list or delivery schedule.

## [1.0.2] - 2026-09-01

Scoped summary of the [upstream v1.0.2 release notes](https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.2); these are historical release descriptions, not new verification results.

### Breaking changes
- **TextBox:** removed `Multiline` and its multiline rendering support. Replace multiline `SfTextBox` usage with `SfTextArea` and review the corresponding bindings and parameters.
- **CheckBox:** changed the tri-state selection order to **Checked → Unchecked → Indeterminate → Checked**. Review interaction logic and tests that depend on the previous order.

### Added / Changed
- Reported ARIA, accessible-name, keyboard/focus and NVDA/Narrator support improvements; these do not establish WCAG conformance.
- Added HighContrast and HighContrast-Light theme support and improved Windows forced-colors styling.
- Standardized component lifecycle patterns for initialization, parameter updates, rendering and disposal.
- Chart sorting no longer modifies bound `PropertyName` / `Direction` values; added required-property validation, improved text-measurement caching and simplified SVG styling.
- Improved CheckBox indeterminate-state synchronization, form validation and grouping behavior.

### Fixed
- TextArea resize, floating-label rendering and validation-state reporting.
- CheckBox state persistence and interaction; ButtonGroup selection state.
- Uploader file-list rendering/updates and Spinner overlay rendering.

## [1.0.1] - 2026-06-26

The [upstream v1.0.1 release notes](https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.1) describe the component offering across buttons and selection controls, calendars, text/numeric inputs, uploads, popups, Spinner and Chart. Chart highlights include core and stacking series, annotations/data labels, axes, legends, tooltips, selection, zooming and panning. This is a scoped historical summary, not an endorsement of every capability or readiness claim in those notes; use the [current component guidance](README.md#components) for source-supported APIs.

## Release evidence and version discrepancy

The GitHub API release response retained during [R00 on **2026-10-06**](docs/evidence/2026-10-06-r00-1922761/index.md#release-identity) contains two public release records: **v1.0.2** (newest in that response) and **v1.0.1**, confirming the publication dates previously reviewed on 2026-10-03. Both have empty `assets` lists; neither record establishes an attached SBOM, certification, signed artifact or template delivery. This does not determine the contents of a NuGet package.

No v1.0.0 release record was present in those responses; this is not proof that no tag or NuGet package exists. The source declared **1.0.0** during R00/R03a; R03's explicit authorization now changes it to **2.0.0 in the uncommitted working tree**. This is not a release entry or evidence of a published package. Final source/package/tag identity still requires release-owner reconciliation. Current source targets and local Unreleased results must not be backdated into either historical release.

See the [test evidence matrix](.github/TEST-MATRIX.md), [draft accessibility register](.github/ACCESSIBILITY.md) and [documentation index](.github/Index.md) for evidence boundaries.

[Unreleased]: https://github.com/syncfusion/blazor-toolkit/compare/v1.0.2...HEAD
[1.0.2]: https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.2
[1.0.1]: https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.1
