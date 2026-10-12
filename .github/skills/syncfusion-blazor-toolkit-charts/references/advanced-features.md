# Advanced Features Reference - Syncfusion Blazor Charts

This reference covers supported Toolkit Chart features and explicit availability boundaries. Samples are configuration fragments needing imports, data, handlers, and an appropriate host; see [getting started](getting-started.md). Repository source, not the commercial Chart API, is authoritative.

## Table of Contents

- [Multiple Panes](#multiple-panes)
   - [When to Use](#when-to-use)
   - [Creating Rows (Horizontal Division)](#creating-rows-horizontal-division)
   - [Creating Columns (Vertical Division)](#creating-columns-vertical-division)
   - [Configuration Options](#configuration-options)
- [Technical Indicators — unavailable](#technical-indicators)
- [Trend Lines](#trend-lines)
   - [When to Use](#when-to-use-1)
   - [Linear Trend Line](#linear-trend-line)
   - [Polynomial Trend Line](#polynomial-trend-line)
   - [Exponential Trend Line](#exponential-trend-line)
   - [Logarithmic Trend Line](#logarithmic-trend-line)
   - [Moving Average Trend Line](#moving-average-trend-line)
   - [Forecasting with Trend Lines](#forecasting-with-trend-lines)
   - [Configuration Options](#configuration-options-1)
- [Strip Lines](#strip-lines)
   - [When to Use](#when-to-use-2)
   - [Horizontal Strip Lines](#horizontal-strip-lines)
   - [Vertical Strip Lines](#vertical-strip-lines)
   - [Segmented Strip Lines](#segmented-strip-lines)
   - [Strip Line with Custom Text](#strip-line-with-custom-text)
   - [Strip Line with Tooltip](#strip-line-with-tooltip)
   - [Configuration Options](#configuration-options-2)
- [Multiple Axes](#multiple-axes)
   - [When to Use](#when-to-use-3)
   - [Dual Y-Axes](#dual-y-axes)
   - [Multiple X and Y Axes](#multiple-x-and-y-axes)
   - [Configuration Options](#configuration-options-3)
- [Data Editing](#data-editing)
   - [When to Use](#when-to-use-4)
   - [Basic Data Editing](#basic-data-editing)
   - [Configuration Options](#configuration-options-4)
- [Empty Points](#empty-points)
   - [When to Use](#when-to-use-5)
   - [Empty Point Modes](#empty-point-modes)
   - [Empty Point Modes Explained](#empty-point-modes-explained)
- [Chart Export — unavailable](#chart-export)
- [Print — browser only](#print)
   - [Print-Specific Styling](#print-specific-styling)
- [RTL Support](#rtl-support)
   - [When to Use](#when-to-use-6)
   - [Enabling RTL Mode](#enabling-rtl-mode)
   - [RTL with Localization](#rtl-with-localization)
   - [RTL Configuration](#rtl-configuration)
- [Implementation Patterns](#implementation-patterns)
   - [Pattern 2: Performance Monitoring](#pattern-2-performance-monitoring)
   - [Pattern 3: Comparative Analysis](#pattern-3-comparative-analysis)
- [Best Practices](#best-practices)
- [Common Use Cases Summary](#common-use-cases-summary)


## Multiple Panes

Multiple panes allow you to divide the chart area into separate plotting regions, ideal for comparing different data series or displaying related metrics with different scales.

### When to Use
- Comparing series with vastly different value ranges
- Creating dashboard-style layouts with multiple related charts
- Financial charts showing price and volume data
- Weather charts displaying temperature and humidity

### Creating Rows (Horizontal Division)

```razor
<SfChart Title="Weather Comparison">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/>
    <ChartPrimaryYAxis Title="Temperature (°F)" LabelFormat="{value}°F" 
                       Minimum="0" Maximum="90" Interval="20"/>
    
    <ChartRows>
        <ChartRow Height="50%"/>
        <ChartRow Height="50%"/>
    </ChartRows>
    
    <ChartAxes>
        <ChartAxis Minimum="24" Maximum="36" Interval="2" OpposedPosition="true" 
                   RowIndex="1" Name="YAxis" LabelFormat="{value}°C"/>
    </ChartAxes>
    
        <ChartSeries DataSource="@WeatherData" XName="Month" YName="TempF" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column"/>
        <ChartSeries DataSource="@WeatherData" XName="Month" YName="TempC" 
                     YAxisName="YAxis" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line"/>
</SfChart>
```

### Creating Columns (Vertical Division)

```razor
<SfChart Title="Sales Analysis">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/>
    <ChartPrimaryYAxis Title="Sales ($)"/>
    
    <ChartColumns>
        <ChartColumn Width="50%"/>
        <ChartColumn Width="50%"/>
    </ChartColumns>
    
    <ChartAxes>
        <ChartAxis OpposedPosition="true" ColumnIndex="1" Name="XAxis2" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/>
    </ChartAxes>
    
        <ChartSeries DataSource="@RegionData" XName="Quarter" YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column"/>
        <ChartSeries DataSource="@ProductData" XName="Quarter" YName="Revenue" 
                     XAxisName="XAxis2" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Area"/>
</SfChart>
```

### Configuration Options
- `Height`/`Width`: Set pane dimensions (percentage or pixels)
- `RowIndex`/`ColumnIndex`: Bind axis to specific pane
- `Span`: Span axis across multiple panes
- `Border`: Customize pane boundaries

---

## Technical Indicators

**Unavailable in this Toolkit.** The source does not expose `ChartIndicators`, `ChartIndicator`, or `TechnicalIndicators`. Do not copy commercial financial-chart examples into a Toolkit app. An application may calculate its own derived data and bind it to a supported series, but that is not a built-in indicator API.

The supported `ChartTrendlines` / `ChartTrendline` components below remain available, including the moving-average trendline.

---

## Trend Lines

Trend lines visualize the direction and pace of data trends using mathematical regression models.

### When to Use
- Forecasting future data points
- Identifying data patterns and correlations
- Scientific data analysis
- Business intelligence dashboards

### Linear Trend Line

```razor
<ChartSeries DataSource="@SalesData" XName="Year" YName="Revenue" 
             Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Scatter">
    <ChartMarker Visible="true" Width="10" Height="10"/>
    <ChartTrendlines>
        <ChartTrendline Type="TrendlineTypes.Linear" Width="3" 
                        Name="Linear Trend" Fill="#FF6B6B"/>
    </ChartTrendlines>
</ChartSeries>
```

### Polynomial Trend Line

```razor
<ChartTrendlines>
    <ChartTrendline Type="TrendlineTypes.Polynomial" Width="3" 
                    Name="Polynomial" Fill="#4ECDC4" PolynomialOrder="3"/>
</ChartTrendlines>
```

### Exponential Trend Line

```razor
<ChartTrendlines>
    <ChartTrendline Type="TrendlineTypes.Exponential" Width="3" 
                    Name="Growth Trend" Fill="#95E1D3"/>
</ChartTrendlines>
```

### Logarithmic Trend Line

```razor
<ChartTrendlines>
    <ChartTrendline Type="TrendlineTypes.Logarithmic" Width="3" 
                    Name="Log Trend" Fill="#F38181"/>
</ChartTrendlines>
```

### Moving Average Trend Line

```razor
<ChartTrendlines>
    <ChartTrendline Type="TrendlineTypes.MovingAverage" Width="3" 
                    Period="5" Name="5-Day MA" Fill="#AA96DA"/>
</ChartTrendlines>
```

### Forecasting with Trend Lines

```razor
<ChartTrendlines>
    <ChartTrendline Type="TrendlineTypes.Linear" Width="3" 
                    ForwardForecast="5" BackwardForecast="3" 
                    Name="Forecast" Fill="#FFB6B9"/>
</ChartTrendlines>
```

### Configuration Options
- `Type`: Linear, Exponential, Logarithmic, Polynomial, Power, MovingAverage
- `Width`: Line thickness
- `Fill`: Trend line color
- `ForwardForecast`: Predict future points
- `BackwardForecast`: Show historical trend
- `Period`: Moving average calculation period
- `PolynomialOrder`: Degree for polynomial trends

---

## Strip Lines

Strip lines are horizontal or vertical bands that highlight specific ranges or thresholds in the chart.

### When to Use
- Highlighting target ranges or thresholds
- Marking acceptable/warning/critical zones
- Showing business hours or time periods
- Indicating baseline or benchmark values

### Horizontal Strip Lines

```razor
<ChartPrimaryYAxis Title="Performance %">
    <ChartStriplines>
        <!-- Excellent Zone -->
        <ChartStripline Start="80" End="100" Color="#C6F6D5" 
                        Text="Excellent" ZIndex="ZIndex.Behind"/>
        
        <!-- Good Zone -->
        <ChartStripline Start="60" End="80" Color="#FED7D7" 
                        Text="Good" ZIndex="ZIndex.Behind"/>
        
        <!-- Warning Zone -->
        <ChartStripline Start="40" End="60" Color="#FEF3C7" 
                        Text="Warning" ZIndex="ZIndex.Behind"/>
    </ChartStriplines>
</ChartPrimaryYAxis>
```

### Vertical Strip Lines

```razor
<ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
    <ChartStriplines>
        <!-- Business Hours -->
        <ChartStripline Start="new DateTime(2024, 01, 01, 09, 0, 0)" 
                        End="new DateTime(2024, 01, 01, 17, 0, 0)" 
                        Color="#E8F5E9" Text="Business Hours" Opacity="0.5"/>
    </ChartStriplines>
</ChartPrimaryXAxis>
```

### Segmented Strip Lines

```razor
<ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime">
    <ChartStriplines>
        <ChartStripline Start="new DateTime(2024, 01, 01, 08, 0, 0)" 
                        End="new DateTime(2024, 01, 01, 10, 0, 0)" 
                        IsSegmented="true" SegmentStart="50" SegmentEnd="75" 
                        SegmentAxisName="YAxis" Color="#FFE0B2"/>
    </ChartStriplines>
</ChartPrimaryXAxis>
```

### Strip Line with Custom Text

```razor
<ChartStripline Start="75" End="100" Color="#C8E6C9" 
                Text="Target Achieved" HorizontalAlignment="Anchor.Middle" 
                VerticalAlignment="Anchor.Middle" Rotation="90">
    <ChartStriplineTextStyle Size="14px" Color="#2E7D32" FontWeight="600"/>
    <ChartStriplineBorder Width="2" Color="#388E3C"/>
</ChartStripline>
```

### Strip Line with Tooltip

```razor
<ChartStripline Start="new DateTime(2024, 01, 01, 07, 00, 00)"
                End="new DateTime(2024, 01, 01, 09, 00, 00)"
                Text="Rush Hour" Color="#FFED4A" Visible="true">
    <ChartStriplineTooltip Enable="true" Header="Peak Traffic"
                          Content="Range: ${stripline.start} - ${stripline.end}"
                          Fill="#F43F5E" Opacity="0.95">
        <ChartStriplineTooltipTextStyle Size="14px" Color="#FFFFFF"/>
    </ChartStriplineTooltip>
</ChartStripline>
```

### Configuration Options
- `Start`/`End`: Define strip line boundaries
- `Color`: Background color
- `Opacity`: Transparency level
- `ZIndex`: Drawing order (Behind/Over)
- `IsSegmented`: Create partial strip lines
- `Size`: Alternative to End for fixed width
- `StartFromAxis`: Begin from axis origin

---

## Multiple Axes

Multiple axes enable displaying series with different scales or units on the same chart.

### When to Use
- Comparing metrics with different units (e.g., temperature and rainfall)
- Financial charts with price and volume
- Dual-scale dashboards
- Correlation analysis

### Dual Y-Axes

```razor
<SfChart Title="Sales vs. Profit Analysis">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/>
    
    <ChartPrimaryYAxis Title="Sales (Units)" LabelFormat="{value}K" 
                       Minimum="0" Maximum="100"/>
    
    <ChartAxes>
        <ChartAxis Name="SecondaryY" OpposedPosition="true" 
                   Title="Profit (%)" LabelFormat="{value}%" 
                   Minimum="0" Maximum="50"/>
    </ChartAxes>
    
        <ChartSeries DataSource="@BusinessData" XName="Product" YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" Name="Sales"/>
        
        <ChartSeries DataSource="@BusinessData" XName="Product" YName="Profit" 
                     YAxisName="SecondaryY" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line" 
                     Name="Profit Margin">
            <ChartMarker Visible="true" Height="10" Width="10"/>
        </ChartSeries>
</SfChart>
```

### Multiple X and Y Axes

```razor
<SfChart>
    <ChartPrimaryXAxis Name="XAxis1" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/>
    <ChartPrimaryYAxis Name="YAxis1" Title="Temperature (°C)"/>
    
    <ChartAxes>
        <ChartAxis Name="XAxis2" OpposedPosition="true" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/>
        <ChartAxis Name="YAxis2" OpposedPosition="true" 
                   Title="Humidity (%)"/>
    </ChartAxes>
    
        <ChartSeries DataSource="@WeatherData" XName="Month" YName="Temp" 
                     XAxisName="XAxis1" YAxisName="YAxis1"/>
        <ChartSeries DataSource="@WeatherData" XName="Month" YName="Humidity" 
                     XAxisName="XAxis2" YAxisName="YAxis2"/>
</SfChart>
```

### Configuration Options
- `Name`: Unique identifier for axis
- `OpposedPosition`: Place on opposite side
- `RowIndex`/`ColumnIndex`: Position in multi-pane layout
- `Span`: Span across multiple panes
- All standard axis properties (min, max, interval, etc.)

---

## Data Editing

Data editing allows users to interactively drag and modify data points at runtime.

### When to Use
- Interactive data entry applications
- Manual data adjustment tools
- What-if analysis scenarios
- Training and simulation applications

### Basic Data Editing

```razor
<SfChart Title="Interactive Sales Forecast">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime"/>
    <ChartPrimaryYAxis LabelFormat="${value}K" Minimum="0" Maximum="100"/>
    
    <ChartTooltipSettings Enable="true"/>
    
        <ChartSeries DataSource="@SalesData" XName="Date" YName="Value" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
            <ChartMarker Visible="true" Width="10" Height="10"/>
            <ChartDataEditSettings Enable="true" Fill="#FF6B6B" 
                                   MinY="0" MaxY="100"/>
        </ChartSeries>
        
        <ChartSeries DataSource="@ForecastData" XName="Date" YName="Value" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line">
            <ChartMarker Visible="true" Width="10" Height="10"/>
            <ChartDataEditSettings Enable="true" Fill="#4ECDC4" 
                                   MinY="0" MaxY="100"/>
        </ChartSeries>
</SfChart>
```

### Configuration Options
- `Enable`: Enable/disable data editing
- `Fill`: Color for edited points
- `MinY`/`MaxY`: Restrict editing range

---

## Empty Points

Handle missing or null data points with various rendering modes.

### When to Use
- Datasets with incomplete information
- Time series with gaps
- Sensor data with occasional failures
- Survey results with non-responses

### Empty Point Modes

```razor
<SfChart Title="Sales Data with Gaps">
        <!-- Zero Mode: Shows null as zero -->
        <ChartSeries DataSource="@DataWithNulls" XName="Month" YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" Name="Zero Mode">
            <ChartEmptyPointSettings Mode="EmptyPointMode.Zero" Fill="#E0E0E0"/>
        </ChartSeries>
        
        <!-- Average Mode: Interpolates with average -->
        <ChartSeries DataSource="@DataWithNulls" XName="Month" YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line" Name="Average Mode">
            <ChartEmptyPointSettings Mode="EmptyPointMode.Average" Fill="#FFB6B9"/>
            <ChartMarker Visible="true"/>
        </ChartSeries>
        
        <!-- Gap Mode: Shows discontinuity -->
        <ChartSeries DataSource="@DataWithNulls" XName="Month" YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.SplineArea" Name="Gap Mode">
            <ChartEmptyPointSettings Mode="EmptyPointMode.Gap"/>
        </ChartSeries>
        
        <!-- Drop Mode: Excludes empty points -->
        <ChartSeries DataSource="@DataWithNulls" XName="Month" YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Scatter" Name="Drop Mode">
            <ChartEmptyPointSettings Mode="EmptyPointMode.Drop"/>
        </ChartSeries>
</SfChart>
```

### Empty Point Modes Explained
- **Zero**: Treats null/undefined as zero value
- **Average**: Calculates average of adjacent points
- **Gap**: Shows break in continuity (line/area charts)
- **Drop**: Completely ignores the point

---

## Chart Export

**Unavailable in this Toolkit.** `SfChart` does not expose `ExportAsync`, an `ExportType` enum, or export completion events. PNG/JPEG/SVG/PDF/XLSX/CSV export and base64 output are not built-in features. Implement reporting or data downloads in application code if required; do not call commercial chart APIs on Toolkit `SfChart`.

---

## Print

**Unavailable as a chart API.** `SfChart` has no `PrintAsync` method or print-completion callback. The browser's print command is separate from the component; an application can provide print CSS without claiming a Toolkit print API.

### Print-Specific Styling

```razor
<style>
    @@media print {
        .no-print { display: none; }
        .print-area { page-break-inside: avoid; }
        .chart-container { width: 100%; }
    }
</style>
```

---

## RTL Support

Right-to-left (RTL) rendering for internationalization in languages like Arabic and Hebrew.

### When to Use
- Applications supporting RTL languages
- Multi-language dashboard applications
- Internationalized reporting tools
- Global market applications

### Enabling RTL Mode

```razor
<SfChart EnableRtl="true" Title="مخطط المبيعات">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/>
    <ChartPrimaryYAxis Title="القيمة"/>
    
        <ChartSeries DataSource="@SalesData" XName="Category" YName="Value" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column"/>
</SfChart>
```

### RTL with Localization

```razor
<SfChart EnableRtl="true" Locale="ar-AE">
    <ChartPrimaryXAxis Title="الفئات"/>
    <ChartPrimaryYAxis Title="القيمة" LabelFormat="{value} ر.س"/>
    
    <ChartTooltipSettings Enable="true" Header="التفاصيل"/>
    
    <ChartLegendSettings Position="LegendPosition.Right"/>
    
        <ChartSeries DataSource="@Data" XName="Category" YName="Value"/>
</SfChart>
```

### RTL Configuration
- `EnableRtl`: Enable right-to-left rendering
- `Locale`: Set culture-specific formatting
- Automatically mirrors: legends, tooltips, data labels
- Reverses: axis direction, series rendering order

---

## Implementation Patterns



### Pattern 2: Performance Monitoring

```razor
<SfChart Title="System Performance Monitor">
    <ChartPrimaryYAxis Title="Usage (%)" Minimum="0" Maximum="100">
        <ChartStriplines>
            <ChartStripline Start="0" End="60" Color="#C6F6D5" Text="Normal"/>
            <ChartStripline Start="60" End="80" Color="#FEF3C7" Text="Warning"/>
            <ChartStripline Start="80" End="100" Color="#FED7D7" Text="Critical"/>
        </ChartStriplines>
    </ChartPrimaryYAxis>
    
        <ChartSeries DataSource="@PerformanceData" XName="Time" YName="CPU" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.SplineArea" Name="CPU">
            <ChartDataEditSettings Enable="true"/>
        </ChartSeries>
</SfChart>
```

### Pattern 3: Comparative Analysis

```razor
<SfChart Title="Regional Comparison">
    <ChartColumns>
        <ChartColumn Width="50%"/>
        <ChartColumn Width="50%"/>
    </ChartColumns>
    
    <ChartAxes>
        <ChartAxis Name="SecondaryX" ColumnIndex="1" 
                   ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/>
    </ChartAxes>
    
        <ChartSeries DataSource="@RegionA" XName="Category" YName="Value">
            <ChartTrendlines>
                <ChartTrendline Type="TrendlineTypes.Linear" ForwardForecast="3"/>
            </ChartTrendlines>
        </ChartSeries>
        
        <ChartSeries DataSource="@RegionB" XName="Category" YName="Value" 
                     XAxisName="SecondaryX">
            <ChartTrendlines>
                <ChartTrendline Type="TrendlineTypes.Linear" ForwardForecast="3"/>
            </ChartTrendlines>
        </ChartSeries>
</SfChart>
```

---

## Best Practices

1. **Performance Optimization**
   - Bound the number of derived series calculated by the application
   - Use data editing judiciously for large datasets
   - Consider data aggregation for trend lines on large datasets

2. **User Experience**
   - Provide clear labels for multiple axes
   - Use contrasting colors for strip lines
   - Include legends when using multiple series
   - Add tooltips for interactive features

3. **Accessibility**
   - Use sufficient color contrast for strip lines
   - Provide text alternatives for visual indicators
   - Validate keyboard alternatives for data editing
   - Support screen readers with proper ARIA labels

4. **Reporting Boundary**
   - Built-in chart export is unavailable
   - Implement any report generation separately in application code
   - Preserve titles, legends, and accessible data alternatives in reports

5. **RTL Considerations**
   - Test thoroughly with actual RTL content
   - Ensure custom styling respects RTL mode
   - Verify numeric formatting in RTL cultures
   - Check tooltip and legend positioning

---

## Common Use Cases Summary

| Feature | Primary Use Case | Best Chart Types |
|---------|-----------------|------------------|
| Multiple Panes | Financial analysis, Multi-metric dashboards | Line, Column |
| Technical Indicators | Unavailable in Toolkit | None |
| Trend Lines | Forecasting, Pattern recognition | Scatter, Line, Area |
| Strip Lines | Threshold monitoring, Range highlighting | All types |
| Multiple Axes | Dual-scale comparison | Line, Column, Area |
| Data Editing | Interactive forecasting, Manual adjustment | Column, Line, Scatter |
| Empty Points | Incomplete data handling | Line, Area, Spline |
| Export | Application-owned reporting only | No built-in API |
| Print | Browser printing only | No built-in API |
| RTL | International applications | All types |

---