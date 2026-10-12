# Syncfusion Blazor Toolkit Charts - API Reference

## Table of Contents

- [Overview](#overview)
- [SfChart Component](#sfchart-component)
   - [Public Methods](#public-methods)
- [Enumerations](#enumerations)
   - [ChartSeriesType](#chartseriestype)
   - [ValueType](#valuetype)
   - [ChartSelectionMode](#chartselectionmode)
   - [HighlightMode](#highlightmode)
   - [SelectionPattern](#selectionpattern)
   - [ExportType — unavailable](#exporttype)
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
   - [ChartPrimaryXAxis / ChartPrimaryYAxis](#chartprimaryxaxis--chartprimaryyaxis)
   - [ChartTooltipSettings](#charttooltipsettings)
   - [ChartLegendSettings](#chartlegendsettings)
   - [ChartZoomSettings](#chartzoomsettings)
- [Important Notes](#important-notes)
- [Common Patterns](#common-patterns)
   - [Basic Chart with Data](#basic-chart-with-data)
   - [Chart with Multiple Series](#chart-with-multiple-series)
   - [Chart with Zooming](#chart-with-zooming)


## Overview

This reference describes the repository's Toolkit Chart, not the commercial Blazor Charts product. Source authority: `src/Components/Charts/ChartEnumerations.cs`, `Chart/SfChart.razor.Methods.cs`, and the relevant component declarations. Signatures below are reference extracts; examples are fragments needing host setup, imports, data, and handlers. They have not been compile-tested in this documentation sync.

---

## SfChart Component

### Public Methods

The `SfChart` component provides the following public methods for programmatic control. Source authority is `Chart/SfChart.razor.Methods.cs`; call instance methods after rendering assigns `@ref`.

| Additional method | Returns | Purpose |
|---|---|---|
| `RefreshLiveData()` | `void` | Refresh live series data |
| `AddSeriesAsync(IList<ChartSeries> seriesCollection)` | `Task` | Add series dynamically |
| `RemoveSeries(int index)` | `void` | Remove a series |
| `ClearSeries()` | `void` | Remove all series |

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

#### Export and print availability

**Unavailable in this Toolkit:** `ExportAsync`, `PrintAsync`, and `ExportType` are not source APIs. Browser printing and application-owned data export are separate concerns, not replacements on `SfChart`.

---

#### ShowTooltipAsync(object x, double y, bool isPoint = true)

Displays a tooltip at data values (`isPoint = true`) or chart coordinates (`false`). Enable `ChartTooltipSettings.Enable` first. Returns `Task`.

#### HideTooltipAsync()

Hides the displayed tooltip. Returns `Task`.

#### ShowCrosshairAsync(double x, double y)

Displays the crosshair at chart coordinates. Enable `ChartCrosshairSettings.Enable` first. Returns `Task`.

#### HideCrosshairAsync()

Hides the crosshair. Returns `Task`.

Interactive component fragment; supply series data and call these handlers after the component reference is assigned:

```razor
<SfChart @ref="ChartRef">
    <ChartTooltipSettings Enable="true" />
    <ChartCrosshairSettings Enable="true" />
</SfChart>

@code {
    private SfChart ChartRef = default!;
    private Task DisplayTooltip() => ChartRef.ShowTooltipAsync("January", 35);
    private Task HideChartTooltip() => ChartRef.HideTooltipAsync();
    private Task DisplayCrosshair() => ChartRef.ShowCrosshairAsync(100, 50);
    private Task HideChartCrosshair() => ChartRef.HideCrosshairAsync();
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
<SfChart @ref="ChartRef" SelectionMode="Syncfusion.Blazor.Toolkit.ChartSelectionMode.Point">
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

Sorts chart data by property name and direction. The enum is
`Syncfusion.Blazor.Toolkit.Data.ListSortDirection`, not the similarly named
`System.ComponentModel` enum.

Interactive fragment (supply `SalesData` and the series mapping):

```razor
<SfChart @ref="ChartRef" DataSource="@SalesData">
    <!-- Supply chart series. -->
</SfChart>

@code {
    private SfChart ChartRef = default!;

    private void SortByValue()
    {
        ChartRef.Sort("Y", Syncfusion.Blazor.Toolkit.Data.ListSortDirection.Descending);
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

The 21 series values, in source declaration order. This is a reference extract, not an enum to redefine in an application.

```csharp
public enum ChartSeriesType
{
    Line,
    Column,
    Area,
    Bar,
    StackingColumn,
    StackingArea,
    StackingLine,
    StackingBar,
    StackingStepArea,
    StepLine,
    StepArea,
    SplineArea,
    Scatter,
    Spline,
    StackingColumn100,
    StackingBar100,
    StackingLine100,
    StackingArea100,
    Bubble,
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

### ChartSelectionMode

Specifies the chart selection mode. Set it on the `SfChart.SelectionMode` parameter; the parameter name does not change.

```csharp
public enum ChartSelectionMode
{
    None,
    Series,
    Point,
    Cluster,
    DragXY,
    DragY,
    DragX,
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
    Chessboard,
    Dots,
    DiagonalForward,
    Crosshatch,
    Pacman,
    DiagonalBackward,
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

**Unavailable in this Toolkit.** There is no chart export enum or built-in export method. This heading is retained to make the unsupported commercial API boundary explicit.

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
    Circle = 0,
    Triangle = 1,
    Diamond = 2,
    Rectangle = 3,
    Pentagon = 4,
    InvertedTriangle = 5,
    VerticalLine = 6,
    Cross = 7,
    Plus = 8,
    HorizontalLine = 9,
    Image = 10,
    Auto = 11
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
    Polynomial,
    Power,
    Logarithmic,
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

**Unavailable in this Toolkit.** No technical-indicator enum or components are exposed. `TrendlineTypes` and `ChartTrendline` above are supported and must not be confused with indicators.

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

Use `Syncfusion.Blazor.Toolkit.Theme`, not the commercial theme enum. The source values are `Fluent`, `FluentDark`, `HighContrast`, and `HighContrastLight`.

Configuration fragment:

```razor
<SfChart Title="Sales Chart" Theme="Syncfusion.Blazor.Toolkit.Theme.Fluent">
    <!-- Supply chart data and series. -->
</SfChart>
```

---

## Key Classes and Components

### ChartSeries

Represents a chart series with its data and configuration.

**Key Properties (signature fragments):**
```csharp
public IEnumerable<object> DataSource { get; set; }
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
public ChartTooltipTextStyle TextStyle { get; set; }
public RenderFragment<object> Template { get; set; }
```

---

### ChartLegendSettings

Configures legend appearance and behavior.

**Key Properties (signature fragments):**
```csharp
public bool Visible { get; set; }
public LegendPosition Position { get; set; }
public Alignment Alignment { get; set; }
```

Set the legend symbol on `ChartSeries.LegendShape` and custom legend markup
on `ChartSeries.LegendItemTemplate`. Do not invent a `Shape` parameter or a
public template parameter on `ChartLegendSettings`.

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

2. **Method Naming**: Use the source signatures, awaiting async operations:
   - `RefreshAsync()`
   - `ShowTooltipAsync()` / `HideTooltipAsync()`
   - `ShowCrosshairAsync()` / `HideCrosshairAsync()`
   - Export/print methods are unavailable

3. **Namespaces**: Import components and enums:
   ```razor
    @using Syncfusion.Blazor.Toolkit
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

Cross-check any API not listed here against the Toolkit source before generating code. These fragments are not a claim of compatibility with the commercial Charts API or of compile-tested sample coverage.
