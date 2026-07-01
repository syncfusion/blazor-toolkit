# Advanced Features Reference - Syncfusion Blazor Charts

This comprehensive reference covers advanced features for implementing sophisticated chart visualizations in Blazor applications using Syncfusion Charts.

## Table of Contents

- [Multiple Panes](#multiple-panes)
   - [When to Use](#when-to-use)
   - [Creating Rows (Horizontal Division)](#creating-rows-horizontal-division)
   - [Creating Columns (Vertical Division)](#creating-columns-vertical-division)
   - [Configuration Options](#configuration-options)
- [Technical Indicators](#technical-indicators)
   - [When to Use](#when-to-use)
   - [Moving Average (SMA, EMA, TMA)](#moving-average-sma-ema-tma)
   - [RSI (Relative Strength Index)](#rsi-relative-strength-index)
   - [Bollinger Bands](#bollinger-bands)
   - [MACD (Moving Average Convergence Divergence)](#macd-moving-average-convergence-divergence)
   - [Available Indicators](#available-indicators)
- [Trend Lines](#trend-lines)
   - [When to Use](#when-to-use)
   - [Linear Trend Line](#linear-trend-line)
   - [Polynomial Trend Line](#polynomial-trend-line)
   - [Exponential Trend Line](#exponential-trend-line)
   - [Logarithmic Trend Line](#logarithmic-trend-line)
   - [Moving Average Trend Line](#moving-average-trend-line)
   - [Forecasting with Trend Lines](#forecasting-with-trend-lines)
   - [Configuration Options](#configuration-options)
- [Strip Lines](#strip-lines)
   - [When to Use](#when-to-use)
   - [Horizontal Strip Lines](#horizontal-strip-lines)
   - [Vertical Strip Lines](#vertical-strip-lines)
   - [Segmented Strip Lines](#segmented-strip-lines)
   - [Strip Line with Custom Text](#strip-line-with-custom-text)
   - [Strip Line with Tooltip](#strip-line-with-tooltip)
   - [Configuration Options](#configuration-options)
- [Multiple Axes](#multiple-axes)
   - [When to Use](#when-to-use)
   - [Dual Y-Axes](#dual-y-axes)
   - [Multiple X and Y Axes](#multiple-x-and-y-axes)
   - [Configuration Options](#configuration-options)
- [Data Editing](#data-editing)
   - [When to Use](#when-to-use)
   - [Basic Data Editing](#basic-data-editing)
   - [Configuration Options](#configuration-options)
- [Empty Points](#empty-points)
   - [When to Use](#when-to-use)
   - [Empty Point Modes](#empty-point-modes)
   - [Empty Point Modes Explained](#empty-point-modes-explained)
- [Chart Export](#chart-export)
   - [When to Use](#when-to-use)
   - [Export to Image Formats](#export-to-image-formats)
   - [Export to PDF](#export-to-pdf)
   - [Export to Excel (XLSX/CSV)](#export-to-excel-xlsxcsv)
   - [Export as Base64 String](#export-as-base64-string)
   - [Customize Export with Events](#customize-export-with-events)
   - [Export Multiple Charts](#export-multiple-charts)
   - [Supported Export Formats](#supported-export-formats)
- [Print](#print)
   - [When to Use](#when-to-use)
   - [Basic Printing](#basic-printing)
   - [Print Multiple Charts](#print-multiple-charts)
   - [Print-Specific Styling](#print-specific-styling)
- [RTL Support](#rtl-support)
   - [When to Use](#when-to-use)
   - [Enabling RTL Mode](#enabling-rtl-mode)
   - [RTL with Localization](#rtl-with-localization)
   - [RTL Configuration](#rtl-configuration)
- [Implementation Patterns](#implementation-patterns)
   - [Pattern 1: Financial Dashboard](#pattern-1-financial-dashboard)
   - [Pattern 2: Performance Monitoring](#pattern-2-performance-monitoring)
   - [Pattern 3: Comparative Analysis](#pattern-3-comparative-analysis)
- [Best Practices](#best-practices)
- [Common Use Cases Summary](#common-use-cases-summary)
- [Reference Links](#reference-links)


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

Technical indicators are mathematical calculations based on historical price, volume, or open interest data, used primarily in financial chart analysis.

### When to Use
- Stock market analysis and trading platforms
- Financial forecasting applications
- Technical analysis dashboards
- Real-time market monitoring



### Bollinger Bands

```razor
<ChartIndicators>
    <ChartIndicator Type="TechnicalIndicators.BollingerBands" 
                    Field="FinancialDataFields.Close" 
                    SeriesName="Stock" Period="14" StandardDeviation="2">
        <ChartIndicatorUpperLine Color="#FF6B6B" Width="2"/>
        <ChartIndicatorLowerLine Color="#4ECDC4" Width="2"/>
    </ChartIndicator>
</ChartIndicators>
```

### MACD (Moving Average Convergence Divergence)

```razor
<ChartIndicators>
    <ChartIndicator Type="TechnicalIndicators.Macd" SeriesName="Stock" 
                    MacdType="MacdType.Both" FastPeriod="12" SlowPeriod="26" 
                    Period="9" MacdPositiveColor="#26A69A" MacdNegativeColor="#EF5350">
        <ChartIndicatorMacdLine Color="#FF6D00" Width="2"/>
    </ChartIndicator>
</ChartIndicators>
```

### Available Indicators
- **SMA**: Simple Moving Average
- **EMA**: Exponential Moving Average
- **TMA**: Triangular Moving Average
- **RSI**: Relative Strength Index
- **MACD**: Moving Average Convergence Divergence
- **Bollinger Bands**: Volatility indicator
- **ATR**: Average True Range
- **Stochastic**: Momentum indicator
- **Momentum**: Rate of price change
- **Accumulation Distribution**: Volume flow indicator

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

Export rendered charts to various formats for reporting and sharing.

### When to Use
- Generating reports and presentations
- Saving charts for offline viewing
- Email attachments and documentation
- Archiving visualizations

### Export to Image Formats

```razor
@using Syncfusion.Blazor.Toolkit.Charts
@using Syncfusion.Blazor.Buttons

<SfChart @ref="ChartRef" Title="Annual Report">
        <ChartSeries DataSource="@ReportData" XName="Category" YName="Value" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column"/>
</SfChart>

<div class="button-group">
    <SfButton Content="Export PNG" @onclick="ExportPNG" IsPrimary="true"/>
    <SfButton Content="Export JPEG" @onclick="ExportJPEG"/>
    <SfButton Content="Export SVG" @onclick="ExportSVG"/>
</div>

@code {
    SfChart ChartRef;
    
    private async Task ExportPNG() => 
        await ChartRef.ExportAsync(ExportType.PNG, "chart");
    
    private async Task ExportJPEG() => 
        await ChartRef.ExportAsync(ExportType.JPEG, "chart");
    
    private async Task ExportSVG() => 
        await ChartRef.ExportAsync(ExportType.SVG, "chart");
}
```

### Export to PDF

```razor
<SfButton Content="Export PDF" @onclick="ExportPDF"/>

@code {
    private async Task ExportPDF()
    {
        await ChartRef.ExportAsync(ExportType.PDF, "report", 
            PdfPageOrientation.Landscape);
    }
}
```

### Export to Excel (XLSX/CSV)

```razor
<SfButton Content="Export Excel" @onclick="ExportExcel"/>
<SfButton Content="Export CSV" @onclick="ExportCSV"/>

@code {
    private async Task ExportExcel() => 
        await ChartRef.ExportAsync(ExportType.XLSX, "data");
    
    private async Task ExportCSV() => 
        await ChartRef.ExportAsync(ExportType.CSV, "data");
}
```

### Export as Base64 String

```razor
<ChartEvents OnExportComplete="ExportComplete"/>

@code {
    private async Task ExportBase64()
    {
        await ChartRef.ExportAsync(ExportType.PDF, "chart", 
            PdfPageOrientation.Portrait, allowDownload: false, isBase64: true);
    }
    
    private void ExportComplete(ExportEventArgs args)
    {
        string base64String = args.Base64;
        // Use base64 string for custom operations
    }
}
```

### Customize Export with Events

```razor
<ChartEvents Exporting="BeforeExport"/>

@code {
    private void BeforeExport(ChartExportEventArgs args)
    {
        // Customize dimensions
        args.Width = 1200;
        args.Height = 800;
        
        // For Excel exports, customize workbook
        if (args.Workbook != null)
        {
            var sheet = args.Workbook.Worksheets.First();
            sheet.Rows[0].Cells[0].CellStyle.BackColor = "#4472C4";
            sheet.Rows[0].Cells[0].CellStyle.Bold = true;
        }
    }
}
```

### Export Multiple Charts

```razor
<div @ref="ChartContainer">
    <SfChart Title="Chart 1">...</SfChart>
    <SfChart Title="Chart 2">...</SfChart>
</div>

<SfButton Content="Export All" @onclick="ExportAll"/>

@code {
    ElementReference ChartContainer;
    SfChart Chart1;
    
    private async Task ExportAll() => 
        await Chart1.PrintAsync(ChartContainer);
}
```

### Supported Export Formats
- **PNG**: Raster image format
- **JPEG**: Compressed image format
- **SVG**: Vector graphics format
- **PDF**: Portable document format
- **XLSX**: Excel workbook
- **CSV**: Comma-separated values

---

## Print

Print charts directly from the browser with customization options.

### When to Use
- Creating hard copies for meetings
- Generating physical reports
- Archiving visualizations
- Presentation materials

### Basic Printing

```razor
<SfChart @ref="ChartRef" Title="Sales Report">
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales"/>
</SfChart>

<SfButton Content="Print Chart" @onclick="PrintChart" IsPrimary="true"/>

@code {
    SfChart ChartRef;
    
    private async Task PrintChart() => await ChartRef.PrintAsync();
}
```

### Print Multiple Charts

```razor
<div @ref="PrintContainer" class="print-area">
    <SfChart @ref="Chart1" Title="Revenue">...</SfChart>
    <SfChart Title="Expenses">...</SfChart>
    <SfChart Title="Profit">...</SfChart>
</div>

<SfButton Content="Print Dashboard" @onclick="PrintAll"/>

@code {
    ElementReference PrintContainer;
    SfChart Chart1;
    
    private async Task PrintAll() => 
        await Chart1.PrintAsync(PrintContainer);
}
```

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
   - Limit number of technical indicators (use max 3-4)
   - Use data editing judiciously for large datasets
   - Consider data aggregation for trend lines on large datasets

2. **User Experience**
   - Provide clear labels for multiple axes
   - Use contrasting colors for strip lines
   - Include legends when using multiple indicators
   - Add tooltips for interactive features

3. **Accessibility**
   - Use sufficient color contrast for strip lines
   - Provide text alternatives for visual indicators
   - Enable keyboard navigation for data editing
   - Support screen readers with proper ARIA labels

4. **Export Optimization**
   - Set appropriate dimensions before export
   - Use vector formats (SVG, PDF) for scalable output
   - Optimize image quality vs. file size for raster formats
   - Include chart title and legends in exports

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
| Technical Indicators | Stock trading, Market analysis | Line |
| Trend Lines | Forecasting, Pattern recognition | Scatter, Line, Area |
| Strip Lines | Threshold monitoring, Range highlighting | All types |
| Multiple Axes | Dual-scale comparison | Line, Column, Area |
| Data Editing | Interactive forecasting, Manual adjustment | Column, Line, Scatter |
| Empty Points | Incomplete data handling | Line, Area, Spline |
| Export | Reporting, Documentation | All types |
| Print | Hard copy generation | All types |
| RTL | International applications | All types |

---