# Chart Playwright Coverage Summary

## Overview
This document summarizes the current Playwright coverage for the Syncfusion `SfChart` component in this repository.

The analysis is based on the existing Playwright test suite in `tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Charts` and the hosted sample pages under `tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.Playwright.Samples/Components/Pages/Charts`.

## Confirmation: Real Chart Component Coverage
- Playwright tests navigate to real Blazor sample routes such as `http://localhost:5000/chart/custom-position`, `http://localhost:5000/chart/bar/regression`, `http://localhost:5000/chart/no-data-template`, and others.
- Sample pages use actual `<SfChart>` elements from `Syncfusion.Blazor.Toolkit.Charts`.
- Test assertions verify rendered SVG output (`svg`, `rect`, `path`, `circle`, `text`) and page-level chart render state.
- `tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Charts/Series/series-coverage-index.spec.ts` explicitly asserts that the tests are using real `svg` chart output and not fake DOM.
- Therefore, the Playwright chart tests cover real rendered Chart component behavior.

## Major Feature Areas Covered
The following Chart feature areas are covered by Playwright tests:

### Chart Rendering and Core Layout
- `Others/chart-basics.spec.ts`
- `Others/category-axis.spec.ts`
- `Others/axes.spec.ts`
- `Others/annotation.spec.ts`
- `Others/step-custom-position.spec.ts`
- Title styling and layout (`ChartTitleStyle/*`)
- General chart rendering, multiple charts, responsive layout

### Axes and Axis Features
- `ChartAxes/category-axis.spec.ts`
- `ChartAxes/date-time-axis.spec.ts`
- `ChartAxes/date-time-category-axis.spec.ts`
- `ChartAxes/indexed-category-axis.spec.ts`
- `ChartAxes/inversed-axis.spec.ts`
- `ChartAxes/logarithmic-axis.spec.ts`
- `ChartAxes/multi-level-label.spec.ts`
- `ChartAxes/multiple-axis.spec.ts`
- `ChartAxes/multiple-panes.spec.ts`
- `ChartAxes/numeric-axis.spec.ts`
- `ChartAxes/recurence-strip-line.spec.ts`
- `ChartAxes/smart-axis-label.spec.ts`
- `ChartAxes/strip-line.spec.ts`
- `ChartAxes/axis-label-template.spec.ts`
- `ChartAxes/axis-crossing.spec.ts`

### Series Types and Styling
- `Series/bar-series.spec.ts`
- `Series/stacking-line-series.spec.ts`
- `Series/auto-marker-shape.spec.ts`
- `Series/sorting-playwright.spec.ts`
- `Series/LineCharts/*`
- `Series/BarCharts/*`
- `Series/AreaCharts/*`
- `Series/ScatterAndBubbleChart/*`
- `Series/column-cross-marker.spec.ts`
- `Series/series-coverage-index.spec.ts`

### Marker API, Data Labels, and Last Data Label
- `MarkerAPI/*`
- `LastDataLabel/*`
- Data label rendering and styling tests
- Marker visibility and marker shape coverage

### Legend and Templates
- `Legend/LegendTemplate/*`
- `Legend/LegendCustomization/*`
- `Legend/DashArrayLine/*`
- Legend element rendering and layout

### Gradients
- `Gradient/*`
- Gradient fill rendering in chart series

### No Data Template
- `NoDataTemplate/*`
- No-data state rendering, layout, and accessibility

### User Interactions and Events
- `UserInteractions/Tooltip/shared-tooltip-events.spec.ts`
- `UserInteractions/Selection/selection-drag.spec.ts`
- `UserInteractions/DataEditing/data-editing-drag.spec.ts`
- `UserInteractions/Crosshair/*`
- `UserInteractions/Zooming/*`
- Event firing and interaction stability

## Feature Coverage Matrix
The Playwright suite covers these high-level capabilities:

- Series rendering across chart types: bar, column, line, spline, area, stepline, steparea, stacking steparea, stacking line, stacking line100, multicolored line, dashed line, auto marker shapes, scatter/bubble
- Axis types: category, date-time, numeric, logarithmic, inverted, multi-level labels, multiple axes, multiple panes, strip lines, axis label templates, axis crossing
- Chart core features: title styling, legend rendering, annotations, gradients, chart borders, chart area rendering
- Interactions: tooltip display, shared tooltip events, crosshair, selection drag, data editing drag, zooming, pan mode, mouse wheel zoom, pinch zoom basics
- Accessibility-focused tests: title accessibility, chart region roles, legend accessibility, last data label accessibility, no-data template accessibility

## Source Coverage Confirmation
- The actual chart implementation lives in `src/Components/Charts/Chart` and related `src/Components/Charts/Common` directories.
- Playwright tests cover rendered output produced by the chart renderer and actual browser DOM.
- The test harness is not using fake or mocked DOM nodes; it exercises the component through the hosted Blazor sample app.

## Notes and Observations
- The Playwright chart coverage is broad and covers the major feature groups present in the chart component.
- If you need an exact gap analysis, the next step is to compare the test matrix against the Chart API surface in `src/Components/Charts/Chart/ChartSeries.cs`, `ChartAxis.cs`, and related model classes.
- A few advanced chart feature areas such as deeper export validation of binary output and advanced annotation positioning were not obviously present in the current coverage tree, but the existing export and annotation tests do cover UI-level behavior.

## Recommended Location for This File
`tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Charts/chart-playwright-coverage.md`

---

> Summary: Playwright Chart tests are real-browser tests using actual `SfChart` component samples. Coverage spans axes, series, legend, markers, gradients, title styling, trendlines, export, no-data templates, events, and user interactions.
