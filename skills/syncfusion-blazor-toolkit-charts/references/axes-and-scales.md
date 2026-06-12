# Blazor Chart Axes and Scales - Comprehensive Reference Guide

## Table of Contents

- [1. Category Axis](#1-category-axis)
   - [Basic Setup](#basic-setup)
   - [Label Placement](#label-placement)
   - [Range and Interval](#range-and-interval)
   - [Indexed Category Axis](#indexed-category-axis)
- [2. Numeric Axis](#2-numeric-axis)
   - [Basic Configuration](#basic-configuration)
   - [Numeric Range and Interval](#numeric-range-and-interval)
   - [Range Padding Types](#range-padding-types)
   - [Numeric Label Format](#numeric-label-format)
   - [Grouping Separator](#grouping-separator)
- [3. DateTime Axis](#3-datetime-axis)
   - [Basic DateTime Axis](#basic-datetime-axis)
   - [DateTime Category Axis](#datetime-category-axis)
   - [Interval Customization](#interval-customization)
   - [DateTime Range Padding](#datetime-range-padding)
   - [DateTime Label Format](#datetime-label-format)
- [4. Logarithmic Axis](#4-logarithmic-axis)
   - [Logarithmic Basic Configuration](#logarithmic-basic-configuration)
   - [Logarithmic Base](#logarithmic-base)
   - [Logarithmic Interval](#logarithmic-interval)
   - [Logarithmic Label Format](#logarithmic-label-format)
- [5. Axis Customization](#5-axis-customization)
   - [Axis Titles](#axis-titles)
   - [Axis Crossing](#axis-crossing)
   - [Opposed Position](#opposed-position)
   - [Inversed Axis](#inversed-axis)
   - [Tick Lines](#tick-lines)
   - [Grid Lines](#grid-lines)
- [6. Axis Labels](#6-axis-labels)
   - [Smart Axis Labels](#smart-axis-labels)
   - [Label Positioning](#label-positioning)
   - [Label Rotation](#label-rotation)
   - [Label Trimming](#label-trimming)
   - [Label Wrapping](#label-wrapping)
   - [Edge Label Placement](#edge-label-placement)
   - [Multilevel Labels](#multilevel-labels)
- [7. Multiple Axes](#7-multiple-axes)
   - [Secondary Axes](#secondary-axes)
   - [Axis Naming](#axis-naming)
   - [Multiple Rows and Columns](#multiple-rows-and-columns)
- [8. Common Properties](#8-common-properties)
- [9. Best Practices](#9-best-practices)
- [10. Troubleshooting](#10-troubleshooting)
- [Complete Working Example](#complete-working-example)
- [Summary](#summary)


## 1. Category Axis

### Basic Setup
Category axis is used to represent string values instead of numeric values.

```cshtml
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
    </ChartPrimaryXAxis>

    <ChartSeriesCollection>
        <ChartSeries DataSource="@MedalDetails" XName="Country" YName="Medals" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code{
    public class ChartData
    {
        public string Country { get; set; }
        public double Medals { get; set; }
    }

    public List<ChartData> MedalDetails = new List<ChartData>
    {
        new ChartData { Country = "USA", Medals = 46 },
        new ChartData { Country = "GBR", Medals = 27 },
        new ChartData { Country = "CHN", Medals = 26 },
        new ChartData { Country = "UK", Medals = 23 },
        new ChartData { Country = "AUS", Medals = 16 }
    };
}
```

### Label Placement
Category labels can be positioned between ticks or on ticks.

```cshtml
<ChartPrimaryXAxis LabelPlacement="LabelPlacement.OnTicks" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
</ChartPrimaryXAxis>
```

**Options:**
- `BetweenTicks` - Labels appear between tick marks (default)
- `OnTicks` - Labels appear on tick marks

### Range and Interval
Control the visible range and spacing of category labels.

```cshtml
<ChartPrimaryXAxis Maximum="5" Minimum="1" Interval="2" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
</ChartPrimaryXAxis>
```

### Indexed Category Axis
Use data source index values for rendering category axis.

```cshtml
<ChartPrimaryXAxis IsIndexed="true" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
</ChartPrimaryXAxis>

<ChartSeriesCollection>
    <ChartSeries DataSource="@WeatherReports1" XName="X" YName="Y" 
                 Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
    </ChartSeries>
    <ChartSeries DataSource="@WeatherReports2" XName="X" YName="Y" 
                 Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
    </ChartSeries>
</ChartSeriesCollection>
```

---

## 2. Numeric Axis

### Basic Configuration
Numeric axis uses double values and is the default axis type.

```cshtml
<SfChart>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Double">
    </ChartPrimaryXAxis>

    <ChartSeriesCollection>
        <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code{
    public List<ChartData> Data = new List<ChartData>
    {
        new ChartData { XValue = 10, YValue = 21 },
        new ChartData { XValue = 20, YValue = 24 },
        new ChartData { XValue = 30, YValue = 36 },
        new ChartData { XValue = 40, YValue = 38 }
    };
}
```

### Numeric Range and Interval

```cshtml
<ChartPrimaryXAxis Minimum="5" Maximum="50" Interval="5">
</ChartPrimaryXAxis>
```

### Range Padding Types

**None** - Minimum and maximum based on data
```cshtml
<ChartPrimaryYAxis RangePadding="ChartRangePadding.None">
</ChartPrimaryYAxis>
```

**Round** - Rounds to nearest interval value
```cshtml
<ChartPrimaryYAxis RangePadding="ChartRangePadding.Round">
</ChartPrimaryYAxis>
```

**Additional** - Adds interval padding
```cshtml
<ChartPrimaryYAxis RangePadding="ChartRangePadding.Additional">
</ChartPrimaryYAxis>
```

**Normal** - Applies default padding
```cshtml
<ChartPrimaryYAxis RangePadding="ChartRangePadding.Normal">
</ChartPrimaryYAxis>
```

**Auto** - Horizontal uses None, Vertical uses Normal
```cshtml
<ChartPrimaryYAxis RangePadding="ChartRangePadding.Auto">
</ChartPrimaryYAxis>
```

### Numeric Label Format

```cshtml
<ChartPrimaryYAxis LabelFormat="c">
</ChartPrimaryYAxis>
```

**Common Formats:**
| Format | Example | Description |
|--------|---------|-------------|
| n1 | 1000.0 | 1 decimal place |
| n2 | 1000.00 | 2 decimal places |
| p1 | 1.0% | Percentage with 1 decimal |
| c1 | $1000.0 | Currency with 1 decimal |
| c2 | $1000.00 | Currency with 2 decimals |

**Custom Format:**
```cshtml
<ChartPrimaryYAxis LabelFormat="${value}K">
</ChartPrimaryYAxis>
```

### Grouping Separator

```cshtml
<SfChart UseGroupingSeparator="true">
    <ChartPrimaryYAxis>
    </ChartPrimaryYAxis>
</SfChart>
```

---

## 3. DateTime Axis

### Basic DateTime Axis

```cshtml
<SfChart>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
    </ChartPrimaryXAxis>

    <ChartSeriesCollection>
        <ChartSeries DataSource="@WeatherReports" XName="Date" YName="Temperature" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code{
    public class ChartData
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
    }

    public List<ChartData> WeatherReports = new List<ChartData>
    {
        new ChartData { Date = new DateTime(2005, 01, 01), Temperature = 21 },
        new ChartData { Date = new DateTime(2006, 01, 01), Temperature = 24 },
        new ChartData { Date = new DateTime(2007, 01, 01), Temperature = 36 }
    };
}
```

### DateTime Category Axis
Displays date-time values with non-linear intervals.

```cshtml
<ChartPrimaryXAxis Format="d MMM yyyy" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTimeCategory">
</ChartPrimaryXAxis>
```

### Interval Customization

```cshtml
<ChartPrimaryXAxis Interval="2" IntervalType="IntervalType.Months" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
</ChartPrimaryXAxis>
```

**Interval Types:**
- `Auto`
- `Years`
- `Months`
- `Days`
- `Hours`
- `Minutes`
- `Seconds`

### DateTime Range Padding

**None:**
```cshtml
<ChartPrimaryXAxis RangePadding="ChartRangePadding.None" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
</ChartPrimaryXAxis>
```

**Round:**
```cshtml
<ChartPrimaryXAxis RangePadding="ChartRangePadding.Round" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
</ChartPrimaryXAxis>
```

**Additional:**
```cshtml
<ChartPrimaryXAxis RangePadding="ChartRangePadding.Additional" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
</ChartPrimaryXAxis>
```

### DateTime Label Format

```cshtml
<ChartPrimaryXAxis LabelFormat="yMd" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
</ChartPrimaryXAxis>
```

**Common DateTime Formats:**
| Format | Example | Description |
|--------|---------|-------------|
| EEEE | Monday | Day of week |
| yMd | 04/10/2000 | Month/Date/Year |
| MMM | Apr | Short month name |
| hm | 12:00 AM | Hours:Minutes |
| hms | 12:00:00 AM | Hours:Minutes:Seconds |

---

## 4. Logarithmic Axis

### Logarithmic Basic Configuration

```cshtml
<SfChart>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
    </ChartPrimaryXAxis>

    <ChartPrimaryYAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Logarithmic">
    </ChartPrimaryYAxis>

    <ChartSeriesCollection>
        <ChartSeries DataSource="@Data" XName="XValue" YName="YValue">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code{
    public List<ChartData> Data = new List<ChartData>
    {
        new ChartData { XValue = new DateTime(2005, 01, 01), YValue = 100 },
        new ChartData { XValue = new DateTime(2006, 01, 01), YValue = 200 },
        new ChartData { XValue = new DateTime(2007, 01, 01), YValue = 500 },
        new ChartData { XValue = new DateTime(2008, 01, 01), YValue = 1000 },
        new ChartData { XValue = new DateTime(2009, 01, 01), YValue = 8000 }
    };
}
```

### Logarithmic Base

```cshtml
<ChartPrimaryYAxis LogBase="2" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Logarithmic">
</ChartPrimaryYAxis>
```

Common bases: 2, 5, 10 (default)

### Logarithmic Interval

```cshtml
<ChartPrimaryYAxis Interval="2" LogBase="10" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Logarithmic">
</ChartPrimaryYAxis>
```

When base is 10 and interval is 2, labels are placed at 10Â², 10â´, 10â¶, etc.

### Logarithmic Label Format

```cshtml
<ChartPrimaryYAxis LabelFormat="c1" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Logarithmic">
</ChartPrimaryYAxis>
```

**Custom Format:**
```cshtml
<ChartPrimaryYAxis LabelFormat="${value}K" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Logarithmic">
</ChartPrimaryYAxis>
```

---

## 5. Axis Customization

### Axis Titles

```cshtml
<ChartPrimaryXAxis Title="Countries" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
    <ChartAxisTitleStyle Size="16px" Color="red" FontFamily="Segoe UI" 
                         FontWeight="bold" TextAlignment="Alignment.Near">
    </ChartAxisTitleStyle>
</ChartPrimaryXAxis>

<ChartPrimaryYAxis Title="Medal Counts">
    <ChartAxisTitleStyle Size="16px" FontFamily="Segoe UI" 
                         FontWeight="bold" TextAlignment="Alignment.Far">
    </ChartAxisTitleStyle>
</ChartPrimaryYAxis>
```

**Title Alignment Options:**
- `Alignment.Near`
- `Alignment.Center`
- `Alignment.Far`

### Axis Crossing

```cshtml
<ChartPrimaryXAxis CrossesAt="15" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
</ChartPrimaryXAxis>

<ChartPrimaryYAxis CrossesAt="5">
</ChartPrimaryYAxis>
```

**For Multiple Axes:**
```cshtml
<ChartPrimaryXAxis CrossesAt="0" CrossesInAxis="YAxis1">
</ChartPrimaryXAxis>

<ChartAxes>
    <ChartAxis Name="YAxis1" OpposedPosition="true">
    </ChartAxis>
</ChartAxes>
```

### Opposed Position

```cshtml
<ChartPrimaryYAxis OpposedPosition="true">
</ChartPrimaryYAxis>
```

Places axis on the opposite side (right for Y-axis, top for X-axis).

### Inversed Axis

```cshtml
<ChartPrimaryYAxis IsInversed="true">
</ChartPrimaryYAxis>
```

Inverts the axis direction (greatest value near origin).

### Tick Lines

```cshtml
<ChartPrimaryXAxis MinorTicksPerInterval="2">
    <ChartAxisMajorTickLines Width="5" Color="blue">
    </ChartAxisMajorTickLines>
    <ChartAxisMinorTickLines Width="1" Color="red">
    </ChartAxisMinorTickLines>
</ChartPrimaryXAxis>

<ChartPrimaryYAxis MinorTicksPerInterval="1">
    <ChartAxisMajorTickLines Width="5" Color="blue">
    </ChartAxisMajorTickLines>
    <ChartAxisMinorTickLines Width="1" Color="red">
    </ChartAxisMinorTickLines>
</ChartPrimaryYAxis>
```

### Grid Lines

```cshtml
<ChartPrimaryXAxis MinorTicksPerInterval="2">
    <ChartAxisMajorGridLines Width="2" Color="blue">
    </ChartAxisMajorGridLines>
    <ChartAxisMinorGridLines Width="0.5" Color="red">
    </ChartAxisMinorGridLines>
</ChartPrimaryXAxis>
```

---

## 6. Axis Labels

### Smart Axis Labels

**Hide Overlapping Labels:**
```cshtml
<ChartPrimaryXAxis LabelIntersectAction="LabelIntersectAction.Hide" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
</ChartPrimaryXAxis>
```

**Rotate 45 Degrees:**
```cshtml
<ChartPrimaryXAxis LabelIntersectAction="LabelIntersectAction.Rotate45" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
</ChartPrimaryXAxis>
```

**Rotate 90 Degrees:**
```cshtml
<ChartPrimaryXAxis LabelIntersectAction="LabelIntersectAction.Rotate90" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
</ChartPrimaryXAxis>
```

### Label Positioning

```cshtml
<ChartPrimaryXAxis LabelPosition="AxisPosition.Inside" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
</ChartPrimaryXAxis>
```

**Options:**
- `AxisPosition.Outside` (default)
- `AxisPosition.Inside`

### Label Rotation

Handled automatically by `LabelIntersectAction` or can be customized in events.

### Label Trimming

```cshtml
<ChartPrimaryXAxis EnableTrim="true" MaximumLabelWidth="40" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
</ChartPrimaryXAxis>
```

### Label Wrapping

```cshtml
<ChartPrimaryXAxis EnableTrim="true" MaximumLabelWidth="50">
    <ChartMultiLevelLabels>
        <ChartMultiLevelLabel Overflow="TextOverflow.Wrap">
            <ChartCategories>
                <ChartCategory Start="-0.5" End="3.5" Text="First Quarter" 
                               MaximumTextWidth="50">
                </ChartCategory>
            </ChartCategories>
        </ChartMultiLevelLabel>
    </ChartMultiLevelLabels>
</ChartPrimaryXAxis>
```

### Edge Label Placement

```cshtml
<ChartPrimaryXAxis EdgeLabelPlacement="EdgeLabelPlacement.Shift" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
</ChartPrimaryXAxis>
```

**Options:**
- `None` - Leave as is
- `Shift` - Move label inside chart area
- `Hide` - Hide edge labels

### Multilevel Labels

```cshtml
<ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
    <ChartMultiLevelLabels>
        <ChartMultiLevelLabel Alignment="Alignment.Center" 
                              Overflow="TextOverflow.Trim">
            <ChartCategories>
                <ChartCategory Start="-0.5" End="3.5" Text="Q1 2024" 
                               MaximumTextWidth="100">
                </ChartCategory>
                <ChartCategory Start="3.5" End="7.5" Text="Q2 2024" 
                               MaximumTextWidth="100">
                </ChartCategory>
            </ChartCategories>
            <ChartAxisMultiLevelLabelTextStyle Size="14px" Color="blue">
            </ChartAxisMultiLevelLabelTextStyle>
            <ChartAxisMultiLevelLabelBorder Type="BorderType.Brace" 
                                            Color="blue" Width="2">
            </ChartAxisMultiLevelLabelBorder>
        </ChartMultiLevelLabel>
    </ChartMultiLevelLabels>
</ChartPrimaryXAxis>
```

**Border Types:**
- `Rectangle`
- `Brace`
- `WithoutBorder`
- `WithoutTopBorder`
- `WithoutTopandBottomBorder`
- `CurlyBrace`

---

## 7. Multiple Axes

### Secondary Axes

```cshtml
<SfChart Title="Weather Reports">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
    </ChartPrimaryXAxis>

    <ChartPrimaryYAxis Title="Temperature (Â°F)">
    </ChartPrimaryYAxis>

    <ChartAxes>
        <ChartAxis Name="YAxis1" Title="Temperature (Â°C)" OpposedPosition="true">
        </ChartAxis>
    </ChartAxes>

    <ChartSeriesCollection>
        <ChartSeries DataSource="@Data" XName="Month" YName="TempF" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
        <ChartSeries DataSource="@Data" XName="Month" YName="TempC" 
                     YAxisName="YAxis1" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>
```

### Axis Naming

Each secondary axis requires a unique name specified in the `Name` property:

```cshtml
<ChartAxes>
    <ChartAxis Name="YAxis1" OpposedPosition="true">
    </ChartAxis>
    <ChartAxis Name="YAxis2" OpposedPosition="false">
    </ChartAxis>
</ChartAxes>
```

Bind series to axis using `YAxisName` or `XAxisName`:

```cshtml
<ChartSeries YAxisName="YAxis1" DataSource="@Data" XName="X" YName="Y">
</ChartSeries>
```

### Multiple Rows and Columns

**Rows:**
```cshtml
<ChartRows>
    <ChartRow Height="50%">
    </ChartRow>
    <ChartRow Height="50%">
    </ChartRow>
</ChartRows>

<ChartAxes>
    <ChartAxis Name="YAxis1" RowIndex="1" OpposedPosition="true">
    </ChartAxis>
</ChartAxes>
```

**Columns:**
```cshtml
<ChartColumns>
    <ChartColumn Width="50%">
    </ChartColumn>
    <ChartColumn Width="50%">
    </ChartColumn>
</ChartColumns>

<ChartAxes>
    <ChartAxis Name="XAxis1" ColumnIndex="1" 
               ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
    </ChartAxis>
</ChartAxes>
```

**Span Across Rows/Columns:**
```cshtml
<ChartPrimaryYAxis Span="2" Title="Temperature">
</ChartPrimaryYAxis>
```

---

## 8. Common Properties

**Essential Axis Properties:**

| Property | Type | Description |
|----------|------|-------------|
| `Minimum` | object | Minimum value for axis range |
| `Maximum` | object | Maximum value for axis range |
| `Interval` | double | Spacing between axis labels |
| `ValueType` | ValueType | Category, Double, DateTime, DateTimeCategory, Logarithmic |
| `Name` | string | Unique identifier for axis |
| `Title` | string | Axis title text |
| `OpposedPosition` | bool | Places axis on opposite side |
| `IsInversed` | bool | Inverts axis direction |
| `LabelFormat` | string | Format string for labels |
| `LabelPlacement` | LabelPlacement | BetweenTicks or OnTicks |
| `LabelPosition` | AxisPosition | Inside or Outside |
| `EdgeLabelPlacement` | EdgeLabelPlacement | None, Shift, or Hide |
| `RangePadding` | ChartRangePadding | None, Round, Additional, Normal, Auto |
| `EnableTrim` | bool | Enables label trimming |
| `MaximumLabelWidth` | double | Maximum width for labels |
| `LabelIntersectAction` | LabelIntersectAction | None, Hide, Rotate45, Rotate90 |
| `RowIndex` | int | Row index for multi-pane charts |
| `ColumnIndex` | int | Column index for multi-pane charts |
| `Span` | int | Number of rows/columns to span |

---

## 9. Best Practices

1. **Choose the Right Axis Type:**
   - Use Category axis for string labels
   - Use Numeric axis for continuous numeric data
   - Use DateTime axis for time-series data
   - Use Logarithmic axis for data spanning multiple orders of magnitude

2. **Optimize Label Display:**
   - Use `LabelIntersectAction` to prevent overlapping labels
   - Set appropriate `MaximumLabelWidth` for long labels
   - Use `EnableTrim` when labels are too long
   - Consider `EdgeLabelPlacement="Shift"` for better edge visibility

3. **Range Configuration:**
   - Set explicit `Minimum` and `Maximum` for consistent scaling
   - Use appropriate `RangePadding` for better data visibility
   - Set `Interval` to control label density

4. **Multiple Axes:**
   - Always provide unique `Name` for secondary axes
   - Use `OpposedPosition` for better visual separation
   - Consider using rows/columns for complex multi-axis scenarios

5. **Performance:**
   - Limit the number of visible labels using `Interval`
   - Use `RangePadding="None"` when exact range is needed
   - Avoid excessive multilevel labels

6. **Accessibility:**
   - Provide meaningful axis titles
   - Use high-contrast colors for grid lines and labels
   - Ensure label text is readable (minimum 10px size)

---

## 10. Troubleshooting

**Problem: Labels are overlapping**
```cshtml
<ChartPrimaryXAxis LabelIntersectAction="LabelIntersectAction.Rotate45">
</ChartPrimaryXAxis>
```

**Problem: Axis range is too tight**
```cshtml
<ChartPrimaryYAxis RangePadding="ChartRangePadding.Additional">
</ChartPrimaryYAxis>
```

**Problem: Edge labels are cut off**
```cshtml
<ChartPrimaryXAxis EdgeLabelPlacement="EdgeLabelPlacement.Shift">
</ChartPrimaryXAxis>
```

**Problem: Too many labels showing**
```cshtml
<ChartPrimaryXAxis Interval="2" Minimum="0" Maximum="10">
</ChartPrimaryXAxis>
```

**Problem: Labels are too long**
```cshtml
<ChartPrimaryXAxis EnableTrim="true" MaximumLabelWidth="50">
</ChartPrimaryXAxis>
```

**Problem: Secondary axis not showing**
```cshtml
<!-- Ensure axis has unique name -->
<ChartAxes>
    <ChartAxis Name="YAxis1" OpposedPosition="true">
    </ChartAxis>
</ChartAxes>

<!-- Bind series to axis -->
<ChartSeries YAxisName="YAxis1" DataSource="@Data" XName="X" YName="Y">
</ChartSeries>
```

**Problem: Logarithmic axis showing negative values**
```cshtml
<!-- Ensure all data values are positive -->
<ChartPrimaryYAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Logarithmic" 
                   Minimum="1" LogBase="10">
</ChartPrimaryYAxis>
```

**Problem: DateTime labels not formatted correctly**
```cshtml
<ChartPrimaryXAxis LabelFormat="MMM yyyy" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
</ChartPrimaryXAxis>
```

**Problem: Category axis showing numeric indices**
```cshtml
<!-- Ensure ValueType is set to Category -->
<ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
</ChartPrimaryXAxis>
```

---

## Complete Working Example

```cshtml
@using Syncfusion.Blazor.Charts

<SfChart Title="Multi-Axis Sales and Temperature Analysis" Width="100%" Height="450px">
    <!-- Primary X-Axis: Category -->
    <ChartPrimaryXAxis Title="Months" 
                       ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"
                       LabelIntersectAction="LabelIntersectAction.Rotate45"
                       EdgeLabelPlacement="EdgeLabelPlacement.Shift">
        <ChartAxisTitleStyle Size="14px" FontWeight="bold">
        </ChartAxisTitleStyle>
    </ChartPrimaryXAxis>

    <!-- Primary Y-Axis: Numeric with currency format -->
    <ChartPrimaryYAxis Title="Sales (USD)" 
                       LabelFormat="c0" 
                       RangePadding="ChartRangePadding.Additional">
        <ChartAxisTitleStyle Size="14px" FontWeight="bold">
        </ChartAxisTitleStyle>
    </ChartPrimaryYAxis>

    <!-- Secondary Y-Axis: Numeric for temperature -->
    <ChartAxes>
        <ChartAxis Name="TempAxis" 
                   Title="Temperature (Â°C)" 
                   OpposedPosition="true"
                   LabelFormat="n0"
                   Minimum="0"
                   Maximum="40"
                   Interval="10">
            <ChartAxisTitleStyle Size="14px" FontWeight="bold">
            </ChartAxisTitleStyle>
            <ChartAxisMajorGridLines Width="0">
            </ChartAxisMajorGridLines>
        </ChartAxis>
    </ChartAxes>

    <!-- Series -->
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" 
                     XName="Month" 
                     YName="Sales" 
                     Name="Sales"
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
        <ChartSeries DataSource="@SalesData" 
                     XName="Month" 
                     YName="Temperature" 
                     Name="Temperature"
                     YAxisName="TempAxis"
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
            <ChartMarker Visible="true" Height="10" Width="10">
            </ChartMarker>
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public class DataPoint
    {
        public string Month { get; set; }
        public double Sales { get; set; }
        public double Temperature { get; set; }
    }

    public List<DataPoint> SalesData = new List<DataPoint>
    {
        new DataPoint { Month = "Jan", Sales = 35000, Temperature = 15 },
        new DataPoint { Month = "Feb", Sales = 28000, Temperature = 17 },
        new DataPoint { Month = "Mar", Sales = 34000, Temperature = 21 },
        new DataPoint { Month = "Apr", Sales = 32000, Temperature = 25 },
        new DataPoint { Month = "May", Sales = 40000, Temperature = 30 },
        new DataPoint { Month = "Jun", Sales = 32000, Temperature = 35 },
        new DataPoint { Month = "Jul", Sales = 35000, Temperature = 38 },
        new DataPoint { Month = "Aug", Sales = 45000, Temperature = 36 },
        new DataPoint { Month = "Sep", Sales = 38000, Temperature = 32 },
        new DataPoint { Month = "Oct", Sales = 30000, Temperature = 26 },
        new DataPoint { Month = "Nov", Sales = 25000, Temperature = 20 },
        new DataPoint { Month = "Dec", Sales = 42000, Temperature = 16 }
    };
}
```

---

## Summary

This reference guide provides comprehensive coverage of Blazor Chart axes and scales including:

- **4 Axis Types**: Category, Numeric, DateTime, and Logarithmic
- **Extensive Customization**: Titles, positioning, styling, and behavior
- **Label Management**: Smart labels, rotation, trimming, and multilevel labels
- **Multiple Axes Support**: Secondary axes, rows, columns, and spanning
- **Best Practices**: Optimization tips and common patterns
- **Troubleshooting**: Solutions to common issues

Use this guide as a quick reference for implementing and customizing chart axes in your Blazor applications.
