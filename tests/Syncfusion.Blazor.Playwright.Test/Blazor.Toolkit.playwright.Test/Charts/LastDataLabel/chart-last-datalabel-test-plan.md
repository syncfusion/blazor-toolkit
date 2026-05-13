# Chart Last Data Label Component Test Plan

## Application Overview

The Chart Last Data Label component demonstrates the Syncfusion Blazor Toolkit SfChart component with Last Data Label feature. The chart displays the efficiency of oil-fired power production over years using a column series with a customized last data label showing the final value with styling (background, border, rounded corners, and custom font).

## Test Scenarios

### 1. Rendering & DOM Structure
**File:** `basic-rendering.spec.ts`

- Chart container renders successfully
- Chart title displays correctly ("Efficiency of oil-fired power production")
- Primary X-Axis renders with title ("Year")
- Primary Y-Axis renders with title ("Efficiency") and label format ("%")
- Chart series renders as column type
- Data points render for all 7 years of data
- Tooltip settings are enabled
- Data labels on regular data points render
- Last data label renders with visibility toggle

### 2. Last Data Label Display & Styling
**File:** `last-datalabel-styling.spec.ts`

- Last data label displays on the final column
- Last data label shows the correct value (40 initially)
- Background color is applied (yellow)
- Border styling applied (red, width 2px, dash array)
- Border radius applied (Rx=5, Ry=5)
- Font styling applied (color #F0E68C, Arial, Italic, Bold, 12px)
- Last data label is visible by default (ShowLabel=true)
- Last data label can be toggled off
- Last data label can be toggled back on
- Last data label text is not truncated

### 3. Data & Series
**File:** `data-series.spec.ts`

- Chart loads with correct initial data (7 data points)
- X-axis displays all years (2005-2011)
- Y-axis displays efficiency percentages
- All columns render in the chart
- Data point values are correct
- Column height corresponds to data values
- Series uses correct chart type (Column)
- Data source binds correctly

### 4. Interactive Features
**File:** `interactions.spec.ts`

- Update Value button is visible and clickable
- Update Value button click changes last data point value (40 → 45)
- Updated value reflects in last column height
- Last data label updates with new value after update
- Toggle Label button is visible and clickable
- Toggle Label button hides last data label on click
- Toggle Label button shows last data label on second click
- Multiple toggles work correctly
- Update and toggle operations can be combined

### 5. Tooltip & Hover States
**File:** `tooltip-hover.spec.ts`

- Tooltip appears on column hover
- Tooltip displays correct data values
- Tooltip disappears when moving away from column
- Tooltip works for all data points
- Last column tooltip appears correctly
- Tooltip positioning is appropriate

### 6. Axis & Labels
**File:** `axis-labels.spec.ts`

- X-axis displays all year labels (2005, 2006, 2007, 2008, 2009, 2010, 2011)
- Y-axis displays percentage labels (0%, 20%, 40%, etc.)
- X-axis title "Year" displays
- Y-axis title "Efficiency" displays
- Axis labels are readable and properly positioned
- Grid lines render (if enabled)
- Axis scaling is appropriate

### 7. Data Labels on Regular Points
**File:** `data-labels.spec.ts`

- Data labels appear on regular data points (not just last)
- Data labels display correct values
- Data labels are visible and readable
- Data label styling is consistent

### 8. Responsive & Layout
**File:** `responsive-layout.spec.ts`

- Chart container renders with correct dimensions (800x500)
- Chart container width is 70% of parent
- Chart maintains aspect ratio
- Chart elements are properly positioned
- Chart title is centered or appropriately positioned
- Legend renders (if applicable)
- No horizontal scrolling needed
- Chart uses full available space

### 9. State Management & Updates
**File:** `state-updates.spec.ts`

- Initial ShowLabel state is true
- ShowLabel state changes on toggle
- Last data point value updates correctly
- State changes persist during chart interaction
- Multiple rapid updates are handled
- Previous state doesn't affect new state

### 10. Accessibility & Integration
**File:** `accessibility-integration.spec.ts`

- Chart elements have appropriate WAI-ARIA attributes
- Buttons have proper role attributes
- Buttons are keyboard accessible
- Tab order is correct
- Chart container is identified properly
- Title is semantically correct
- Test with different screen sizes/orientations

## Test File Organization

```
Charts/
├── LastDataLabel/
│   ├── chart-last-datalabel-test-plan.md
│   ├── basic-rendering.spec.ts
│   ├── last-datalabel-styling.spec.ts
│   ├── data-series.spec.ts
│   ├── interactions.spec.ts
│   ├── tooltip-hover.spec.ts
│   ├── axis-labels.spec.ts
│   ├── data-labels.spec.ts
│   ├── responsive-layout.spec.ts
│   ├── state-updates.spec.ts
│   └── accessibility-integration.spec.ts
```

## Key Test Data

- **Chart Type:** Column
- **Data Points:** 7 years (2005-2011)
- **Last Data Label Value:** 40 (initially), 45 (after update)
- **Y-Axis Format:** Percentage (%)
- **Colors:** Yellow background, red border, #F0E68C font color
- **Font:** Arial, Italic, Bold, 12px

## Page URL

- `http://localhost:5000/chart/last-datalabel`

## Test Execution Notes

- Ensure test data loader is running before test execution
- Tests should run sequentially for chart rendering tests
- Tooltip tests may need hover duration adjustments
- Visual regression tests may be added for styling verification
- Performance tests may be added for large datasets
