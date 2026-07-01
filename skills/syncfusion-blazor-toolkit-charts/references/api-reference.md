# Syncfusion Blazor Charts - Complete API Reference

## Table of Contents

- [Overview](#overview)
- [SfChart Component](#sfchart-component)
   - [Public Methods](#public-methods)
- [Enumerations](#enumerations)
   - [ChartSeriesType](#chartseriestype)
   - [ValueType](#valuetype)
   - [SelectionMode](#selectionmode)
   - [HighlightMode](#highlightmode)
   - [SelectionPattern](#selectionpattern)
   - [ExportType](#exporttype)
   - [LegendPosition](#legendposition)
   - [EmptyPointMode](#emptypointmode)
   - [LabelPlacement](#labelplacement)
   - [EdgeLabelPlacement](#edgelabelplacement)
   - [LabelIntersectAction](#labelintersectaction)
   - [ChartShape](#chartshape)
   - [TrendlineTypes](#trendlinetypes)
   - [TechnicalIndicators](#technicalindicators)
   - [ZoomMode](#zoommode)
   - [ToolbarItems](#toolbaritems)
   - [Theme](#theme)
- [Key Classes and Components](#key-classes-and-components)
   - [ChartSeries](#chartseries)
   - [ChartPrimaryXAxis / ChartPrimaryYAxis](#chartprimaryxaxis-chartprimaryyaxis)
   - [ChartTooltipSettings](#charttooltipsettings)
   - [ChartLegendSettings](#chartlegendsettings)
   - [ChartZoomSettings](#chartzoomsettings)
- [Important Notes](#important-notes)
- [Common Patterns](#common-patterns)
   - [Basic Chart with Data](#basic-chart-with-data)
   - [Chart with Multiple Series](#chart-with-multiple-series)
   - [Chart with Zooming](#chart-with-zooming)


## Overview

This document provides the complete and accurate API reference for Syncfusion Blazor Charts based on the official Syncfusion API documentation. Use this as the authoritative source for all enum values, method signatures, and property names.

---

## SfChart Component

### Public Methods

The `SfChart` component provides the following public methods for programmatic control:

#### RefreshAsync(bool shouldAnimate = true)

Re-renders the chart with optional animation.

```csharp
public Task RefreshAsync(bool shouldAnimate = true)
```

**Parameters:**
- `shouldAnimate` - Specifies whether the chart should animate during refresh (default: true)

**Returns:** `Task`

**Example:**
```razor
<SfChart @ref="ChartRef">
    <!-- Chart configuration -->
</SfChart>

@code {
    SfChart ChartRef;
    
    async Task UpdateChart()
    {
        await ChartRef.RefreshAsync(true);
    }
}
```

---

#### ExportAsync(ExportType type, string fileName, PdfPageOrientation? orientation = null, bool allowDownload = true, bool isBase64 = false)

Exports the chart to various formats (PDF, PNG, JPEG, SVG).

```csharp
public Task ExportAsync(ExportType type, string fileName, PdfPageOrientation? orientation = null, bool allowDownload = true, bool isBase64 = false)
```

**Parameters:**
- `type` - The export format (`ExportType` enum)
- `fileName` - The name of the exported file
- `orientation` - PDF page orientation (Portrait/Landscape)
- `allowDownload` - Whether to download the file
- `isBase64` - Whether to return as base64 string

**Returns:** `Task`

**Example:**
```razor
<SfChart @ref="ChartRef">
    <!-- Chart configuration -->
</SfChart>

@code {
    SfChart ChartRef;
    
    async Task ExportChart()
    {
        await ChartRef.ExportAsync(ExportType.PNG, "SalesChart");
    }
}
```

---

#### PrintAsync(ElementReference elementRef = default)

Prints the chart.

```csharp
public Task PrintAsync(ElementReference elementRef = default)
```

**Parameters:**
- `elementRef` - Optional reference to the chart element

**Returns:** `Task`

**Example:**
```razor
<SfChart @ref="ChartRef">
    <!-- Chart configuration -->
</SfChart>

@code {
    SfChart ChartRef;
    
    async Task PrintChart()
    {
        await ChartRef.PrintAsync();
    }
}
```

---

#### ShowTooltip(object x, double y, bool isPoint = true)

Displays tooltip at specified coordinates or data points.

```csharp
public void ShowTooltip(object x, double y, bool isPoint = true)
```

**Parameters:**
- `x` - X-value of the point or x-coordinate
- `y` - Y-value of the point or y-coordinate  
- `isPoint` - Whether x and y are data points (true) or coordinates (false)

**Example:**
```razor
<SfChart @ref="ChartRef">
    <ChartTooltipSettings Enable="true" />
    <!-- Chart configuration -->
</SfChart>

@code {
    SfChart ChartRef;
    
    void DisplayTooltip()
    {
        ChartRef.ShowTooltip("January", 35);
    }
}
```

---

#### HideTooltip()

Hides the currently displayed tooltip.

```csharp
public void HideTooltip()
```

**Example:**
```razor
@code {
    SfChart ChartRef;
    
    void HideChartTooltip()
    {
        ChartRef.HideTooltip();
    }
}
```

---

#### ShowCrosshair(double x, double y)

Displays crosshair at specified coordinates.

```csharp
public void ShowCrosshair(double x, double y)
```

**Parameters:**
- `x` - X-coordinate on the chart
- `y` - Y-coordinate on the chart

**Example:**
```razor
<SfChart @ref="ChartRef">
    <ChartCrosshairSettings Enable="true" />
    <!-- Chart configuration -->
</SfChart>

@code {
    SfChart ChartRef;
    
    void DisplayCrosshair()
    {
        ChartRef.ShowCrosshair(100, 50);
    }
}
```

---

#### HideCrosshair()

Hides the currently displayed crosshair.

```csharp
public void HideCrosshair()
```

**Example:**
```razor
@code {
    SfChart ChartRef;
    
    void HideChartCrosshair()
    {
        ChartRef.HideCrosshair();
    }
}
```

---

#### ClearSelection()

Clears all selections in the chart.

```csharp
public void ClearSelection()
```

**Example:**
```razor
<SfChart @ref="ChartRef" SelectionMode="SelectionMode.Point">
    <!-- Chart configuration -->
</SfChart>

@code {
    SfChart ChartRef;
    
    void ClearAllSelections()
    {
        ChartRef.ClearSelection();
    }
}
```

---

#### Sort(string propertyName, ListSortDirection direction)

Sorts chart data by property name and direction.

```csharp
public void Sort(string propertyName, ListSortDirection direction)
```

**Parameters:**
- `propertyName` - Property name to sort by
- `direction` - Sort direction (`Ascending` or `Descending`)

**Example:**
```razor
<SfChart @ref="ChartRef" DataSource="@SalesData">
    <ChartSorting PropertyName="X" Direction="ListSortDirection.Ascending" />
    <!-- Chart configuration -->
</SfChart>

@code {
    SfChart ChartRef;
    
    void SortByValue()
    {
        ChartRef.Sort("Y", ListSortDirection.Descending);
    }
}
```

---

#### ClearSort()

Clears the sorting applied to the chart.

```csharp
public void ClearSort()
```

**Example:**
```razor
@code {
    SfChart ChartRef;
    
    void RemoveSorting()
    {
        ChartRef.ClearSort();
    }
}
```

---

#### PreventRender(bool preventRender = true)

Prevents or allows chart rendering.

```csharp
public void PreventRender(bool preventRender = true)
```

**Parameters:**
- `preventRender` - If true, prevents rendering; if false, allows rendering

**Example:**
```razor
@code {
    SfChart ChartRef;
    
    void StopRendering()
    {
        ChartRef.PreventRender(true);
        // Perform batch updates
        ChartRef.PreventRender(false); // Resume rendering
    }
}
```

---

## Enumerations

### ChartSeriesType

Specifies the type of chart series.

```csharp
public enum ChartSeriesType
{
    Line,
    Column,
    Area,
    Bar,
    Bubble,
    Scatter,
    Spline,
    SplineArea,
    StepLine,
    StepArea,
    StackingColumn,
    StackingColumn100,
    StackingBar,
    StackingBar100,
    StackingArea,
    StackingArea100,
    StackingLine,
    StackingLine100,
    MultiColoredLine,
    MultiColoredArea
}
```

**Common Values:**
- `Line` - Line chart
- `Column` - Vertical column chart
- `Area` - Area chart
- `Bar` - Horizontal bar chart
- `Spline` - Smooth line chart
- `Scatter` - Scatter plot
- `Bubble` - Bubble chart
- `StackingColumn` - Stacked column chart
- `StackingColumn100` - 100% stacked column chart

---

### ValueType

Specifies the type of axis.

```csharp
public enum ValueType
{
    Double,
    DateTime,
    Category,
    Logarithmic,
    DateTimeCategory
}
```

**Values:**
- `Double` - Numeric axis
- `DateTime` - DateTime axis
- `Category` - Category axis
- `Logarithmic` - Logarithmic axis
- `DateTimeCategory` - DateTime category axis

---

### SelectionMode

Specifies the selection mode.

```csharp
public enum SelectionMode
{
    None,
    Series,
    Point,
    Cluster,
    DragXY,
    DragX,
    DragY,
    Lasso
}
```

**Values:**
- `None` - No selection
- `Series` - Select entire series
- `Point` - Select individual point
- `Cluster` - Select cluster of points
- `DragXY` - Drag to select in both directions
- `DragX` - Drag to select horizontally
- `DragY` - Drag to select vertically
- `Lasso` - Lasso selection

---

### HighlightMode

Specifies the highlight mode.

```csharp
public enum HighlightMode
{
    None,
    Series,
    Point,
    Cluster
}
```

**Values:**
- `None` - No highlighting
- `Series` - Highlight entire series
- `Point` - Highlight individual point
- `Cluster` - Highlight cluster of points

---

### SelectionPattern

Specifies selection/highlight patterns.

```csharp
public enum SelectionPattern
{
    None,
    DiagonalForward,
    DiagonalBackward,
    Crosshatch,
    Dots,
    Chessboard,
    Grid,
    Turquoise,
    Star,
    Triangle,
    Circle,
    Tile,
    HorizontalDash,
    VerticalDash,
    Rectangle,
    Box,
    VerticalStripe,
    HorizontalStripe,
    Bubble
}
```

---

### ExportType

Specifies export types.

```csharp
public enum ExportType
{
    PNG,
    JPEG,
    SVG,
    PDF
}
```

**Values:**
- `PNG` - Export as PNG image
- `JPEG` - Export as JPEG image
- `SVG` - Export as SVG vector
- `PDF` - Export as PDF document

---

### LegendPosition

Specifies legend position.

```csharp
public enum LegendPosition
{
    Auto,
    Top,
    Left,
    Bottom,
    Right,
    Custom
}
```

**Values:**
- `Auto` - Automatically positions legend
- `Top` - Position at top
- `Left` - Position at left
- `Bottom` - Position at bottom
- `Right` - Position at right
- `Custom` - Custom position using X and Y coordinates

---

### EmptyPointMode

Specifies how to handle empty points.

```csharp
public enum EmptyPointMode
{
    Gap,
    Zero,
    Drop,
    Average
}
```

**Values:**
- `Gap` - Leave gap at empty point
- `Zero` - Treat as zero
- `Drop` - Drop the empty point
- `Average` - Use average of surrounding points

---

### LabelPlacement

Specifies label placement for category axis.

```csharp
public enum LabelPlacement
{
    BetweenTicks,
    OnTicks
}
```

**Values:**
- `BetweenTicks` - Place labels between ticks
- `OnTicks` - Place labels on ticks

---

### EdgeLabelPlacement

Specifies edge label placement.

```csharp
public enum EdgeLabelPlacement
{
    None,
    Hide,
    Shift
}
```

**Values:**
- `None` - No special treatment
- `Hide` - Hide edge labels
- `Shift` - Shift edge labels inside

---

### LabelIntersectAction

Specifies action for intersecting labels.

```csharp
public enum LabelIntersectAction
{
    None,
    Hide,
    Trim,
    Wrap,
    MultipleRows,
    Rotate45,
    Rotate90
}
```

**Values:**
- `None` - No action
- `Hide` - Hide intersecting labels
- `Trim` - Trim labels with ellipsis
- `Wrap` - Wrap label text
- `MultipleRows` - Display in multiple rows
- `Rotate45` - Rotate labels 45 degrees
- `Rotate90` - Rotate labels 90 degrees

---

### ChartShape

Specifies marker shapes.

```csharp
public enum ChartShape
{
    Circle,
    Rectangle,
    Triangle,
    Diamond,
    Pentagon,
    Cross,
    HorizontalLine,
    VerticalLine,
    InvertedTriangle,
    Image
}
```

---

### TrendlineTypes

Specifies trendline types.

```csharp
public enum TrendlineTypes
{
    Linear,
    Exponential,
    Logarithmic,
    Polynomial,
    Power,
    MovingAverage
}
```

**Values:**
- `Linear` - Linear trendline
- `Exponential` - Exponential trendline
- `Logarithmic` - Logarithmic trendline
- `Polynomial` - Polynomial trendline
- `Power` - Power trendline
- `MovingAverage` - Moving average trendline

---

### TechnicalIndicators

Specifies technical indicator types.

```csharp
public enum TechnicalIndicators
{
    Sma,
    Ema,
    Tma,
    Momentum,
    Atr,
    AccumulationDistribution,
    Bollinger,
    Macd,
    Stochastic,
    Rsi
}
```

**Common Values:**
- `Sma` - Simple Moving Average
- `Ema` - Exponential Moving Average
- `Tma` - Triangular Moving Average
- `Momentum` - Momentum indicator
- `Rsi` - Relative Strength Index
- `Macd` - Moving Average Convergence Divergence
- `Bollinger` - Bollinger Bands
- `Atr` - Average True Range
- `Stochastic` - Stochastic oscillator

---

### ZoomMode

Specifies zooming mode.

```csharp
public enum ZoomMode
{
    X,
    Y,
    XY
}
```

**Values:**
- `X` - Zoom horizontally only
- `Y` - Zoom vertically only
- `XY` - Zoom in both directions

---

### ToolbarItems

Specifies zooming toolbar items.

```csharp
public enum ToolbarItems
{
    Zoom,
    ZoomIn,
    ZoomOut,
    Pan,
    Reset
}
```

---

### Theme  

Specifies chart themes. Use `Syncfusion.Blazor.Theme` enum for setting the `Theme` property of `SfChart`.

**IMPORTANT:** The enum name is `Theme`, not `ChartTheme`. Use it as `Theme="Syncfusion.Blazor.Theme.Material"` or `Theme="Theme.Material"` (with proper using directive).

```csharp
public enum Theme
{
    Material,
    Fabric,
    Bootstrap,
    Bootstrap4,
    HighContrastLight,
    MaterialDark,
    FabricDark,
    BootstrapDark,
    Bootstrap4Dark,
    HighContrast,
    Tailwind,
    TailwindDark,
    Bootstrap5,
    Bootstrap5Dark,
    Fluent,
    FluentDark,
    Material3,
    Material3Dark
}
```

**Usage Example:**
```razor
<SfChart Title="Sales Chart" Theme="Syncfusion.Blazor.Theme.Bootstrap5">
    <!-- chart content -->
</SfChart>
```

---

## Key Classes and Components

### ChartSeries

Represents a chart series with its data and configuration.

**Key Properties:**
```csharp
public string DataSource { get; set; }
public string XName { get; set; }
public string YName { get; set; }
public ChartSeriesType Type { get; set; }
public string Name { get; set; }
public string Fill { get; set; }
public double Width { get; set; }
public string DashArray { get; set; }
public double Opacity { get; set; }
```

---

### ChartPrimaryXAxis / ChartPrimaryYAxis

Configures the primary axes.

**Key Properties:**
```csharp
public ValueType ValueType { get; set; }
public string Title { get; set; }
public object Minimum { get; set; }
public object Maximum { get; set; }
public double Interval { get; set; }
public string LabelFormat { get; set; }
public EdgeLabelPlacement EdgeLabelPlacement { get; set; }
public LabelIntersectAction LabelIntersectAction { get; set; }
```

---

### ChartTooltipSettings

Configures tooltip behavior.

**Key Properties:**
```csharp
public bool Enable { get; set; }
public string Format { get; set; }
public bool Shared { get; set; }
public string Fill { get; set; }
public string TextStyle { get; set; }
public RenderFragment<object> Template { get; set; }
```

---

### ChartLegendSettings

Configures legend appearance and behavior.

**Key Properties:**
```csharp
public bool Visible { get; set; }
public LegendPosition Position { get; set; }
public Alignment Alignment { get; set; }
public LegendShape Shape { get; set; }
public RenderFragment<object> Template { get; set; }
```

---

### ChartZoomSettings

Configures zooming behavior.

**Key Properties:**
```csharp
public bool EnableSelectionZooming { get; set; }
public bool EnablePinchZooming { get; set; }
public bool EnableMouseWheelZooming { get; set; }
public bool EnableScrollbar { get; set; }
public bool EnablePan { get; set; }
public ZoomMode Mode { get; set; }
public ToolbarItems[] ToolbarItems { get; set; }
```

---

## Important Notes

1. **Correct Enum Usage**: Always use the fully qualified enum name:
   ```razor
    Type="ChartSeriesType.Column"  <!-- Correct -->
    Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column"  <!-- Also correct -->
   ```

2. **Method Naming**: All public methods use proper C# naming conventions:
   - `RefreshAsync()` - NOT `Refresh()`
   - `ExportAsync()` - NOT `Export()`
   - `ShowTooltip()` - NOT `Show_Tooltip()`

3. **Namespace**: Always include the namespace import:
   ```razor
    @using Syncfusion.Blazor.Toolkit.Charts
   ```

4. **Component Reference**: To call methods, use `@ref`:
   ```razor
   <SfChart @ref="ChartRef">
   </SfChart>
   
   @code {
       SfChart ChartRef;
   }
   ```

---

## Common Patterns

### Basic Chart with Data

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart>
    <ChartPrimaryXAxis ValueType="ValueType.Category" />
    <ChartPrimaryYAxis Title="Sales" />
    
        <ChartSeries DataSource="@SalesData" 
                     XName="Month" 
                     YName="Sales" 
                     Type="ChartSeriesType.Column">
        </ChartSeries>
</SfChart>

@code {
    public class SalesInfo
    {
        public string Month { get; set; }
        public double Sales { get; set; }
    }
    
    public List<SalesInfo> SalesData = new List<SalesInfo>
    {
        new SalesInfo { Month = "Jan", Sales = 35 },
        new SalesInfo { Month = "Feb", Sales = 28 },
        new SalesInfo { Month = "Mar", Sales = 34 }
    };
}
```

### Chart with Multiple Series

```razor
<SfChart>
    <ChartPrimaryXAxis ValueType="ValueType.Category" />
    
    <ChartLegendSettings Visible="true" />
    
        <ChartSeries DataSource="@Data1" 
                     Name="Product A"
                     XName="X" 
                     YName="Y" 
                     Type="ChartSeriesType.Column">
        </ChartSeries>
        <ChartSeries DataSource="@Data2" 
                     Name="Product B"
                     XName="X" 
                     YName="Y" 
                     Type="ChartSeriesType.Column">
        </ChartSeries>
</SfChart>
```

### Chart with Zooming

```razor
<SfChart>
    <ChartZoomSettings EnableSelectionZooming="true" 
                       EnableMouseWheelZooming="true"
                       EnablePan="true"
                       Mode="ZoomMode.XY">
    </ChartZoomSettings>
    
        <!-- Series configuration -->
</SfChart>
```

---

This API reference document provides accurate information based on the official Syncfusion Blazor Charts API. Always refer to this document when generating code samples or providing API guidance.
