# Changelog

Notable changes to the **Syncfusion® Toolkit for Blazor**, with historical summaries attributed to upstream release notes.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/). Historical version labels are preserved as published; see the [development guide](.github/DEVELOPMENT.md#versioning-and-evidence-status) for versioning and compatibility guidance.

## [2.0.0] - 2026-10-09

### Breaking Changes

- **Namespace consolidation** — Domain-specific namespaces were removed. All public types now live under the single root namespace `Syncfusion.Blazor.Toolkit`.
  - Migration: Replace previous domain imports (for example component- or feature-scoped namespaces) with:

    ```csharp
    @using Syncfusion.Blazor.Toolkit
    ```

    or the equivalent in `_Imports.razor` / C# files. Update any fully qualified type names accordingly.

- **API surface (approved 2.0 contract)** — Compared to NuGet 1.0.2, 194 named compatibility diagnostics are accepted for this major version (type visibility, collection signatures, getter-only ownership, parent metadata types, static helper construction, and `DataAdaptor<T> where T : notnull`). Full list and migration guidance: [API-COMPATIBILITY.md](API-COMPATIBILITY.md) and [API-BREAK-INVENTORY.md](API-BREAK-INVENTORY.md).
  - Chart: many formerly public renderer/internal types are now internal — use public components, settings, events, and templates; do not subclass or construct internal renderers.
  - Collections: several `List<T>` public members are now `IList<T>` / interface contracts — do not cast results to `List<T>`.
  - `UploadedFiles.Files` and protected `ChartData.CurrentPoints` are getter-only for ownership; mutate via Clear/Add, do not replace the list.
  - Parent properties (`*Parent` on date/time/textbox) are `object` instead of `dynamic`.
  - `ChartHelper` / `DataVizCommonHelper` instance construction/subclassing removed (`StringToNumber` remains where applicable).
  - `DataAdaptor<T>` requires `where T : notnull` on subclasses.

- **Theme delivery model** — Introduced `SfThemeRoot`, a render-time emitter that writes the shared theme layer (`:root` design tokens, base/utility styles, icon font, keyframes, dark / high-contrast / forced-colors rules) as a single `<style id="sf-theme-root">` element. Every consumer-facing root component now renders `<SfThemeRoot />` as the **previous sibling** of its root element (never inside it). Only one owner per Blazor renderer emits the style; ownership recovers automatically on dispose.
  - Migration: No consumer configuration is required for the common case. Custom root components that previously assumed global theme CSS alone must ensure toolkit components (or an explicit `<SfThemeRoot />`) are present in the render tree so the theme is emitted in static SSR, interactive, and transition scenarios.
  - Placement rules are enforced by tests: emitter must be a sibling before the root element, not inside the root, not inside `<svg>`, and not inside popup/portal content moved by script.

- **CSS isolation** — Component styles migrated to Blazor CSS isolation (`.razor.css` → scoped bundle). Selectors and cascade behavior may differ from the previous global stylesheet approach. App-level overrides that targeted unscoped toolkit class names may need adjustment to account for isolation attributes / scope.

### Added

- `SfThemeRoot` and `SfThemeScope` (renderer-scoped owner election and recovery).

- Unit tests for theme-root emission, ownership, and placement enforcement (`SfThemeRootTests`, `SfThemeRootEnforcementTests`).

- JavaScript fallback path for incorrect omission of `<SfThemeRoot />` (injects into `document.head` only when no owner exists for that renderer).

### Changed

- Shared theme layer is now part of component render output, so styles are present in static SSR / prerendered HTML and on the first interactive render without JavaScript or consumer setup.

- Uploader default filename, extension and status display treats metadata as text. The user-approved correction preserves original selected filenames in callbacks instead of numeric-entity rewriting while retaining optional invalid-name validation. Templates remain application-owned. Private security review and released-package validation remain pending.

- Contrast theme support expanded, with readiness reports and sample browser UI updates.

- Sample browser layouts, namespaces, and demo pages updated for CSS isolation, the single root namespace, and theme-root placement (including popups, Getting Started, and layout action buttons).

- Component style sheets updated for Buttons, ButtonGroup, Calendars (Calendar, DatePicker, DateTimePicker, TimePicker), Inputs (CheckBox, NumericTextBox, RadioButton, Switch, TextArea, TextBox, Uploader), Popups (Dialog, Tooltip), Spinner, and Chart.

- Development dependency updates (eslint group, sass, glob, braces/gulp, playwright, setup-dotnet, test-reporter, gulp-sass, and related tooling).

- Correct public nullable annotations to describe existing null inputs/results. Annotation-only edits do not themselves change CLR types, but **the complete `2.0.0` surface is not compatible with released 1.0.2**: collection signatures and public type visibility also changed. No blanket compatibility assurance is intended.

- `DataAdaptor<T>` now declares `where T : notnull`, matching `OwningComponentBase<T>`. Generic subclasses must propagate this constraint; nullable type arguments or unconstrained type parameters may produce nullable-analysis warnings when consumers rebuild (errors under warnings-as-errors policies). This compiler-level contract does not add a CLR-enforced runtime constraint or change service scope/disposal behavior.

- Released `SfDialog.GetButtonItems()` returns `List<DialogButton>?`; current source returns `IList<DialogButton>?`. A net8 consumer assigning the result to `List<DialogButton>?` compiled against 1.0.2 and failed against current source with CS0266. Other collection/accessibility differences require individual migration review.

- `QueryableOperation` filtering/searching over `DynamicObject` records now throws a descriptive `InvalidOperationException` when a requested field is null or missing in the first record, rather than dereferencing the missing value. It does not infer types from later records; the existing `ExpandoObject` path is unchanged.

### Fixed

- Render internal chart components through typed render fragments instead of unresolved Razor tags, without exposing the renderer classes publicly.

- Correct nullable data, reflection, calendar, input, and dialog flows; retain null grouping keys, no-op reflection accessors, and null prompt-cancellation results.

- Preserve label/index alignment when sorting date-category chart points with null X values, and use the owning chart's theme for stripline tooltips.

- Isolate axis-overlap state per chart layout, preventing concurrent charts from clearing each other's previous-axis references and coordinates.

- Restore bUnit compilation, configure chart-only browser interop fixtures, and synchronize asynchronous rendering assertions.

- Styles not applied correctly in **static SSR** mode; theme and component styles now emit during the server render pass.

- Chart rendering under static SSR (default SVG size and layout path when post-render measurement / JS is unavailable).

- Spinner and Checkbox styling regressions after CSS isolation migration; missing styles restored for Calendars and TextArea.

- NumericTextBox and Calendar component styles after CSS isolation migration.

- Invalid selectors in generated CSS.

- Loss of dark-theme color overrides after isolation/theme-root changes.

- Server app stylesheet issues in the sample host.

- Console errors in the sample browser.

- Conflict resolution and stability fixes in `SfThemeRoot` / theme-scope ownership.

### Migration Notes

- Version `2.0.0` is not source-compatible with `1.0.2`.
- Replace all domain-specific `@using` directives with:

  ```csharp
  @using Syncfusion.Blazor.Toolkit
  ```

- Review any custom CSS overrides after the CSS isolation migration.
- Ensure toolkit components, or an explicit `SfThemeRoot`, are present in applications that require theme generation.
- Update code consuming `SfDialog.GetButtonItems()` to use `IList<DialogButton>?` or another suitable collection interface.
- Propagate the `where T : notnull` constraint when extending `DataAdaptor<T>`.

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
- `Syncfusion.Blazor.Toolkit.Templates` project templates (`blazortoolkitweb`, `blazortoolkitwasm`).

[2.0.0]: https://github.com/syncfusion/blazor-toolkit/compare/v1.0.2...v2.0.0
[1.0.2]: https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.2
[1.0.1]: https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.1
[1.0.0]: https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.0
