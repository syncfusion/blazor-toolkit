# Style contract for `Syncfusion.Blazor.Toolkit`

## Scope

All selectors prefixed with `e-` (for example `.e-btn`, `.e-calendar`,
`.e-dialog`, `.e-spinner`) that ship from `src/wwwroot/styles/*.scss`
are part of the **public** styling API surface of the toolkit.

This document is the contract that component authors, themers, and
consumers can rely on. It is binding under D5 / BEQ-20.

## Namespace

- **Public selectors** are the selectors used by consumers for theming
  and host-window customization.
- **Internal selectors** are nested under `:where(.e-{component})` and
  carry names beginning with `e-`, but they are **not** listed in the
  public-selectors table below. Consumers must not depend on internal
  selectors; they may change in any release.

## Required public selectors

The following selectors are stable across patch releases and
backwards-compatible minor releases:

| Selector | Component |
|---|---|
| `.e-lib`, `.e-control` | Common base classes applied by every component |
| `.e-btn` | `SfButton` |
| `.e-btn-group` | `SfButtonGroup` |
| `.e-checkbox` | `SfCheckBox` |
| `.e-radio` | `SfRadioButton` |
| `.e-switch` | `SfSwitch` |
| `.e-textbox` | `SfTextBox` |
| `.e-textarea` | `SfTextArea` |
| `.e-numerictextbox` | `SfNumericTextBox` |
| `.e-uploader` | `SfUploader` |
| `.e-calendar` | `SfCalendar` |
| `.e-datepicker` | `SfDatePicker` |
| `.e-datetimepicker` | `SfDateTimePicker` |
| `.e-timepicker` | `SfTimePicker` |
| `.e-dialog` | `SfDialog` |
| `.e-tooltip` | `SfTooltip` |
| `.e-spinner` | `SfSpinner` |
| `.e-chart` | `SfChart` |

Additive slot selectors within any of the above follow SemVer
minor-version rules. A rename or removal of any **required** selector
requires a SemVer major-version bump and must be recorded in
`THREAT-MODEL.md`.

## Component CSS isolation

A component may opt into Blazor CSS isolation for local styles that
do not need to be themable across hosts. When that is done, the file
`Component.razor.css` is colocated with `Component.razor` and the
`.scss` from which it is compiled lives alongside it. Currently in use:

- `src/Components/Spinner/Renderer/Border.razor.css`
- `src/Components/Inputs/NumericTextBox/SfNumericTextBox.razor.css`

CSS isolation is preferred for component-private tokens that would
otherwise leak into every host page; the global `e-*` sheet is
preferred for themable appearance that consumers customise.

## Delivery

The compiled stylesheet is shipped under the static asset path
`_content/Syncfusion.Blazor.Toolkit/styles/` and consumed from a host
via:

```html
<link href="_content/Syncfusion.Blazor.Toolkit/styles/fluent.css"
      rel="stylesheet" />
```

The exact theme CSS file name (`fluent`, `highcontrast`, etc.) is
configured by `gulp blazor-toolkit-themes`.