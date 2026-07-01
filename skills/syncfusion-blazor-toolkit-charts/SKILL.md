---
name: syncfusion-blazor-toolkit-charts
description: Implement Syncfusion Blazor Chart (SfChart) component for core data visualizations. Use this when creating line, area, column/bar, scatter, bubble, spline and stacking chart variants in Blazor applications.
metadata:
  author: "Syncfusion Inc"
  version: "1.0.0"
  category: "Data Visualization"
---

# Implementing Syncfusion Blazor Charts

**NuGet:** `Syncfusion.Blazor.Toolkit` + `Syncfusion.Blazor.Themes`  
**Namespace:** `Syncfusion.Blazor.Toolkit`

A comprehensive guide for implementing the Syncfusion Blazor Chart component to create interactive, feature-rich data visualizations in Blazor applications. The Chart component supports core series types (line, area, column/bar, scatter, bubble, spline) and stacking/step/spline variants, multiple axes, interactivity, and customization.

## When to Use This Skill

Use this skill when you need to:
- **Create data visualizations** in Blazor applications (line, column, bar, area, etc.)
- **Display comparative data** with stacked or grouped charts
- **Add interactive features** like tooltips, zooming, crosshair, selection
- **Customize chart appearance** with themes, gradients, annotations
- **Handle dynamic data** with live updates and data editing
- **Implement accessible charts** with ARIA support and internationalization

## Component Overview

The Syncfusion Blazor Chart component is a powerful data visualization tool that provides:

- **33+ Chart Types:** Line, Column, Bar, Area, Spline, Scatter, Bubble, and more
- **Multiple Axes:** Support for category, numeric, datetime, and logarithmic axes
- **Rich Interactivity:** Zooming, panning, crosshair, trackball, tooltips, selection
- **Data Binding:** Work with List, DataManager, dynamic updates, and live data
- **Visual Elements:** Markers, data labels, annotations, legends, gradients
- **Advanced Features:** Technical indicators, trend lines, strip lines, multiple panes
- **Accessibility:** Full ARIA support, keyboard navigation, localization, RTL
- **Customization:** Themes, responsive design, print support, adaptive layout

## Documentation and Navigation Guide

### Complete API Reference

📄 **Read:** [references/api-reference.md](references/api-reference.md)

**CRITICAL: Use this reference FIRST for all API-related questions**

Use this authoritative API reference when:
- Looking up correct enum values (ChartSeriesType, ValueType, SelectionMode, etc.)
- Finding public method signatures (RefreshAsync, ExportAsync, ShowTooltip, etc.)
- Verifying property names and types
- Generating code samples
- Troubleshooting API-related issues

Topics covered:
- Complete list of SfChart public methods with signatures
- All chart enumerations with correct values
- Key component classes and their properties
- Method usage examples
- Common patterns and best practices

**Key Points:**
- All enum values are **exact** - do not use variations
- Method names follow C# conventions (e.g., `RefreshAsync` not `Refresh`)
- Always use `@ref` to access chart instance for method calls
- Namespace: `Syncfusion.Blazor.Toolkit.Charts`

---

### Getting Started

📄 **Read:** [references/getting-started.md](references/getting-started.md)

Use this when:
- Setting up a new Blazor Chart project
- Installing NuGet packages and configuring services
- Creating your first chart component
- Understanding basic chart structure and data binding
- Working with Visual Studio, VS Code, or .NET CLI

Topics covered:
- Installation and prerequisites
- Package setup (Syncfusion.Blazor.Toolkit.Charts)
- Namespace imports and service registration
- Script references
- Basic chart implementation
- Simple data binding example

---

### Chart Types

#### Common Chart Types

📄 **Read:** [references/chart-types-common.md](references/chart-types-common.md)

Use this for frequently-used chart types:
- Line charts for trend visualization
- Area charts for showing magnitude over time
- Column charts for categorical comparisons
- Bar charts for horizontal comparisons
- Spline charts for smooth curves
- Step charts for stepped data progression

Topics covered:
- Line, Area, Column, Bar chart implementations
- Spline and Spline Area variations
- Step Line and Step Area patterns
- When to use each chart type
- Configuration and customization
- Multi-series examples

#### Specialized Chart Types

📄 **Read:** [references/chart-types-specialized.md](references/chart-types-specialized.md)

Use this for specialized visualization needs:
- Stacked charts (Stack Area/Bar/Column/Line, 100% variants)
- Scatter and Bubble charts
- Multicolored Line / Multicolored Area
- Vertical chart orientation

Topics covered:
- Financial chart patterns and configurations
- Statistical analysis chart types
- Stacking modes (normal vs 100%)
- Range visualization techniques
- Circular chart layouts
- XY scatter relationships
- Chart orientation options

---

### Axes and Data Configuration

📄 **Read:** [references/axes-and-scales.md](references/axes-and-scales.md)

Use this when:
- Configuring axis types (category, numeric, datetime, logarithmic)
- Customizing axis appearance and behavior
- Formatting axis labels and values
- Working with multiple axes
- Setting axis ranges and intervals

Topics covered:
- Category axis for discrete data
- Numeric axis for continuous values
- DateTime axis for time-series data
- Logarithmic axis for exponential data
- Axis customization (title, range, interval)
- Label formatting, rotation, and positioning
- Multiple axis configuration
- Inverse and opposed axes

📄 **Read:** [references/data-handling.md](references/data-handling.md)

Use this when:
- Binding data sources to charts
- Adding, removing, or updating data dynamically
- Enabling data editing
- Sorting chart data
- Working with live or streaming data

Topics covered:
- Data source binding (List, DataManager)
- Dynamic data operations
- Data editing functionality
- Chart sorting options
- Real-time data updates
- Data serialization patterns

---

### Visual Elements

📄 **Read:** [references/visual-elements.md](references/visual-elements.md)

Use this when:
- Adding data markers to chart points
- Displaying data labels on series
- Creating custom label templates
- Adding annotations (text, shapes, images)
- Applying gradient fills

Topics covered:
- Data marker types and customization
- Data label visibility and formatting
- Label positioning strategies
- Data label templates
- Last data label highlighting
- Chart annotations (text, shapes, images)
- Gradient color fills

📄 **Read:** [references/legend.md](references/legend.md)

Use this when:
- Enabling and configuring chart legend
- Positioning legend (top, bottom, left, right)
- Customizing legend appearance
- Implementing legend click behavior
- Creating custom legend templates

Topics covered:
- Legend visibility and positioning
- Legend shape and text customization
- Interactive legend (toggle series)
- Legend pagination
- Custom legend templates
- Legend alignment options

---

### Interactive Features

📄 **Read:** [references/interactive-features.md](references/interactive-features.md)

Use this when:
- Enabling tooltips with custom formatting
- Adding crosshair or trackball for data tracking
- Implementing point, series, or cluster selection
- Enabling zooming and panning
- Creating interactive user experiences

Topics covered:
- Tooltip configuration and templates
- Crosshair and trackball features
- Selection modes and patterns
- Zoom types (selection, pinch, mousewheel)
- Zoom toolbar configuration
- Pan functionality
- User interaction events

---

### Customization and Appearance

📄 **Read:** [references/appearance-styling.md](references/appearance-styling.md)

Use this when:
- Customizing chart appearance and themes
- Setting chart dimensions and sizing
- Configuring responsive/adaptive layout
- Enabling print functionality
- Styling background, borders, and margins

Topics covered:
- Chart appearance customization
- Theme configuration
- Chart dimensions and sizing
- Responsive design patterns
- Adaptive layout for mobile
- Print support
- Background and border styling
- Gradient customization

📄 **Read:** [references/advanced-features.md](references/advanced-features.md)

Use this when:
- Adding technical indicators (EMA, SMA, RSI, etc.)
- Implementing trend lines (linear, exponential, polynomial)
- Creating strip lines (horizontal/vertical bands)
- Using multiple panes (sub-charts)
- Handling empty points

Topics covered:
- Technical indicators for financial charts
- Trend line types and configuration
- Strip line patterns
- Multiple pane layouts
- Empty point handling strategies
- Row and column definitions

---

### Accessibility and Internationalization

📄 **Read:** [references/accessibility-internationalization.md](references/accessibility-internationalization.md)

Use this when:
- Implementing accessible charts (ARIA, keyboard nav)
- Configuring advanced accessibility features
- Enabling internationalization (i18n)
- Setting up localization (l10n)
- Supporting RTL layouts

Topics covered:
- Accessibility features and ARIA support
- Keyboard navigation patterns
- Advanced accessibility configuration
- Internationalization setup
- Localization resources
- RTL support
- Screen reader compatibility

---

### Events

📄 **Read:** [references/events.md](references/events.md)

Use this when:
- Handling chart lifecycle events
- Responding to user interactions (clicks, mouse events)
- Listening to axis, series, or point events
- Implementing custom event handlers
- Triggering actions on zoom, pan, or selection

Topics covered:
- Chart load and render events
- Point and series events
- Mouse events (click, move, leave)
- Axis label render events
- Legend events
- Zoom and pan events
- Selection events
- Event handler patterns

---

### Practical Examples and How-To Guides

📄 **Read:** [references/practical-examples.md](references/practical-examples.md)

Use this for common implementation scenarios:
- Adding/removing series dynamically
- Hiding axes programmatically
- Converting milliseconds to datetime
- Updating points dynamically
- Implementing lazy loading
- Creating live/real-time charts
- Getting selected data
- Synchronizing multiple charts
- Adding threshold lines
- Custom tooltip tables

Topics covered:
- Dynamic series management
- Axis visibility control
- Date conversion techniques
- Dynamic point updates
- Lazy loading patterns
- Live chart implementation
- Selection data retrieval
- Chart synchronization
- Threshold line implementation
- Custom tooltip tables

---

## Public Methods Reference

The `SfChart` component provides several public methods for programmatic control. Always use `@ref` to access these methods:

### Essential Methods

```razor
<SfChart @ref="ChartRef">
    <!-- Configuration -->
</SfChart>

@code {
    SfChart ChartRef;
    
    // Refresh the chart
    async Task RefreshChart() => await ChartRef.RefreshAsync();
    
    // Export the chart
    async Task ExportChart() => await ChartRef.ExportAsync(ExportType.PNG, "chart.png");
    
    // Print the chart
    async Task PrintChart() => await ChartRef.PrintAsync();
    
    // Show/Hide tooltip
    void ShowChartTooltip() => ChartRef.ShowTooltip("January", 35);
    void HideChartTooltip() => ChartRef.HideTooltip();
    
    // Show/Hide crosshair
    void ShowChartCrosshair() => ChartRef.ShowCrosshair(100, 50);
    void HideChartCrosshair() => ChartRef.HideCrosshair();
    
    // Selection control
    void ClearSelections() => ChartRef.ClearSelection();
    
    // Sorting
    void SortData() => ChartRef.Sort("YValue", ListSortDirection.Descending);
    void ClearChartSort() => ChartRef.ClearSort();
}
```

**See [references/api-reference.md](references/api-reference.md) for complete method documentation.**

---

## ⚠️ CRITICAL: API Usage Guidelines

### 1. Enum Names - MUST Use Full Namespace

**Syncfusion Blazor Toolkit Charts requires FULLY QUALIFIED enum names:**

```razor
<!-- ✅ CORRECT - Use full namespace -->
<ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
<ChartSeries Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />

<!-- ❌ WRONG - Short form will cause errors -->
<ChartPrimaryXAxis ValueType="ValueType.Category" />
<ChartSeries Type="ChartSeriesType.Column" />
```

**Always use `Syncfusion.Blazor.Toolkit.` prefix for:**
- `ValueType` (Category, Double, DateTime, etc.)
- `ChartSeriesType` (Column, Line, Area, etc.)
- `LegendPosition`, `SelectionMode`, `ZoomMode`, `ChartShape`, etc.

### 2. Component Property Restrictions

**⚠️ Common Property Errors to Avoid:**

#### ChartCrosshairSettings - Supported Properties
```razor
<ChartCrosshairSettings Enable="true" DashArray="2,3" LineType="LineType.Both">
    <ChartCrosshairLine Width="1.5"></ChartCrosshairLine>
</ChartCrosshairSettings>
```

**Supported ChartCrosshairSettings Properties:**
- `Enable` - Enable/disable crosshair (boolean)
- `DashArray` - Dash pattern for dashed lines (e.g., "2,3")
- `LineType` - Crosshair line type (Vertical, Horizontal, Both)

**Supported ChartCrosshairLine Properties:**
- `Width` - Line width (number)
- `Color` - Line color (string)

#### Striplines - Use Plural Form
```razor
<!-- ✅ CORRECT - ChartStriplines (plural) -->
<ChartPrimaryYAxis>
    <ChartStriplines>
        <ChartStripline Start="50" End="60" Color="red" />
    </ChartStriplines>
</ChartPrimaryYAxis>

<!-- ❌ WRONG - ChartAxisStripLineSettings does not exist -->
<ChartAxisStripLineSettings>
    <ChartAxisStripLine Start="50" End="60" />
</ChartAxisStripLineSettings>
```

### 3. Validation Checklist

Before deploying, verify:
- ✅ All enums use `Syncfusion.Blazor.Toolkit.` prefix
- ✅ ChartCrosshairLine uses ONLY Width and Color
- ✅ Striplines use `<ChartStriplines>` and `<ChartStripline>`
- ✅ Component references use `@ref` for method access
- ✅ Property names match official API exactly (case-sensitive)

---

## Quick Start Example

Here's a minimal example to create a column chart with data:

```razor
@page "/chart-demo"
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Sales Analysis">
    <ChartPrimaryXAxis Title="Month" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
    </ChartPrimaryXAxis>
    
    <ChartPrimaryYAxis Title="Sales in Dollar">
    </ChartPrimaryYAxis>
    
    <ChartTooltipSettings Enable="true"></ChartTooltipSettings>
    
        <ChartSeries DataSource="@SalesData" 
                     Name="Sales" 
                     XName="Month" 
                     YName="SalesValue" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
            <ChartMarker>
                <ChartDataLabel Visible="true"></ChartDataLabel>
            </ChartMarker>
        </ChartSeries>
</SfChart>

@code {
    public class SalesInfo
    {
        public string Month { get; set; }
        public double SalesValue { get; set; }
    }

    public List<SalesInfo> SalesData = new List<SalesInfo>
    {
        new SalesInfo { Month = "Jan", SalesValue = 35 },
        new SalesInfo { Month = "Feb", SalesValue = 28 },
        new SalesInfo { Month = "Mar", SalesValue = 34 },
        new SalesInfo { Month = "Apr", SalesValue = 32 },
        new SalesInfo { Month = "May", SalesValue = 40 },
        new SalesInfo { Month = "Jun", SalesValue = 32 }
    };
}
```

**Prerequisites:**
1. Install `Syncfusion.Blazor.Toolkit.Charts` NuGet package
2. Add `@using Syncfusion.Blazor.Toolkit.Charts` to `_Imports.razor`
3. Register service: `builder.Services.AddSyncfusionBlazor();` in `Program.cs`
4. Add script reference in `App.razor`

---

## Common Patterns

### Multi-Series Comparison Chart

```razor
<SfChart Title="Product Sales Comparison">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
    
        <ChartSeries DataSource="@Product1Sales" XName="Month" YName="Sales" 
                     Name="Product 1" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
        </ChartSeries>
        <ChartSeries DataSource="@Product2Sales" XName="Month" YName="Sales" 
                     Name="Product 2" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
        </ChartSeries>
    
    <ChartLegendSettings Visible="true"></ChartLegendSettings>
</SfChart>
```

### Time-Series Line Chart with Zooming

```razor
<SfChart>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime" 
                        Title="Date">
    </ChartPrimaryXAxis>
    
    <ChartZoomSettings EnableSelectionZooming="true" 
                       EnablePan="true" 
                       EnableMouseWheelZooming="true">
    </ChartZoomSettings>
    
        <ChartSeries DataSource="@TimeSeriesData" 
                     XName="Date" 
                     YName="Value" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line">
        </ChartSeries>
</SfChart>
```

### Stacked Area Chart with Legend

```razor
<SfChart Title="Resource Allocation">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
    
        <ChartSeries DataSource="@ResourceData1" XName="Period" YName="Hours" 
                     Name="Development" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.StackingArea">
        </ChartSeries>
        <ChartSeries DataSource="@ResourceData2" XName="Period" YName="Hours" 
                     Name="Testing" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.StackingArea">
        </ChartSeries>
        <ChartSeries DataSource="@ResourceData3" XName="Period" YName="Hours" 
                     Name="Documentation" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.StackingArea">
        </ChartSeries>
    
    <ChartLegendSettings Visible="true" Position="Syncfusion.Blazor.Toolkit.LegendPosition.Bottom">
    </ChartLegendSettings>
</SfChart>
```

---

## Key Configuration Properties

### Chart Component (`SfChart`)
- `Title` - Chart title text
- `Width` / `Height` - Chart dimensions (accepts px or %, default: "100%")
- `Theme` - Visual theme (see Theme enum in API reference)
- `Background` - Chart background color
- `EnableAnimation` - Enable/disable animation (default: true)
- `SelectionMode` - Selection mode (None, Series, Point, Cluster, DragXY, DragX, DragY, Lasso)
- `HighlightMode` - Highlight mode (None, Series, Point, Cluster)

### Primary Axes (`ChartPrimaryXAxis`, `ChartPrimaryYAxis`)
- `ValueType` - Axis type: `Syncfusion.Blazor.Toolkit.ValueType.Category`, `ValueType.Double`, `ValueType.DateTime`, `ValueType.Logarithmic`, `ValueType.DateTimeCategory`
- `Title` - Axis title
- `Minimum` / `Maximum` - Axis range
- `Interval` - Spacing between labels
- `LabelFormat` - Format string for labels (e.g., "C0", "N2", "dd MMM")
- `EdgeLabelPlacement` - Edge label handling (None, Hide, Shift)
- `LabelIntersectAction` - Label intersection handling (None, Hide, Trim, Wrap, MultipleRows, Rotate45, Rotate90)
- **Striplines** - Use `<ChartStriplines>` (plural) for horizontal/vertical bands

### Series (`ChartSeries`)
- `DataSource` - Data collection (IEnumerable<object>)
- `XName` / `YName` - Property names for X and Y values (case-sensitive)
- `Type` - Chart type: `Syncfusion.Blazor.Toolkit.ChartSeriesType.Column`, `ChartSeriesType.Line`, etc. (see API reference)
- `Name` - Series name for legend
- `Fill` - Series color (CSS color string)
- `Width` - Line/border width (pixels)
- `DashArray` - Dash pattern (e.g., "5,5")
- `Opacity` - Transparency (0 to 1)

### Interactivity
- `ChartTooltipSettings` - Tooltip configuration
  - `Enable` - Show/hide tooltip
  - `Shared` - Enable shared tooltip for multiple series
  - `Format` - Custom tooltip format
  - `Template` - Custom tooltip template (RenderFragment)
- `ChartZoomSettings` - Zoom and pan options
  - `EnableSelectionZooming` - Enable selection-based zoom
  - `EnableMouseWheelZooming` - Enable mouse wheel zoom
  - `EnablePinchZooming` - Enable pinch zoom (touch devices)
  - `EnablePan` - Enable panning
  - `Mode` - Zoom mode (X, Y, XY)
- `SelectionMode` - Selection behavior (see SelectionMode enum)
- `ChartCrosshairSettings` - Crosshair configuration
  - `Enable` - Show/hide crosshair
  - `LineType` - Crosshair line type (Vertical, Horizontal, Both)
  - `ChartCrosshairLine` - Line style (Width, Color ONLY - NO DashArray)

### Visual Elements
- `ChartMarker` - Data point markers
  - `Visible` - Show/hide markers
  - `Shape` - Marker shape (see ChartShape enum)
  - `Width` / `Height` - Marker dimensions
- `ChartDataLabel` - Data labels on points
  - `Visible` - Show/hide labels
  - `Position` - Label position
  - `Template` - Custom label template
- `ChartLegendSettings` - Legend configuration
  - `Visible` - Show/hide legend
  - `Position` - Legend position (see LegendPosition enum)
- `ChartAnnotations` - Custom annotations
  - Support for text, images, and shapes

---
