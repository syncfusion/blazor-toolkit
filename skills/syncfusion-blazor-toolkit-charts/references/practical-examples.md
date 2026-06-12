# Practical Examples for Syncfusion Blazor Charts

Complete, copy-paste-ready real-world examples demonstrating common chart implementation scenarios.

## Table of Contents

- [1. Sales Dashboard](#1-sales-dashboard)
- [2. Financial Stock Chart](#2-financial-stock-chart)
- [3. Performance Comparison](#3-performance-comparison)
- [4. Trend Analysis](#4-trend-analysis)
- [5. Regional Distribution](#5-regional-distribution)
- [6. Real-Time Monitoring](#6-real-time-monitoring)
- [7. Interactive Report](#7-interactive-report)
- [8. Responsive Analytics](#8-responsive-analytics)
- [Summary](#summary)


## 1. Sales Dashboard

**Scenario:** Display monthly sales data for multiple products with interactive tooltips and legends.

**When to use:** Business dashboards, sales reports, performance tracking.

**Key features:** Multi-series line chart, custom colors, data labels, markers, legend.

```razor
@page "/sales-dashboard"
@using Syncfusion.Blazor.Charts

<div class="dashboard-container">
    <h2>Sales Performance Dashboard</h2>
    <SfChart Title="Monthly Sales Comparison" Width="100%" Height="450px">
        <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category" 
                           Title="Months">
            <ChartAxisMajorGridLines Width="0"></ChartAxisMajorGridLines>
        </ChartPrimaryXAxis>
        <ChartPrimaryYAxis Title="Sales (in thousands)" 
                           Minimum="0" Maximum="100" Interval="20" 
                           LabelFormat="${value}K">
        </ChartPrimaryYAxis>
        <ChartTooltipSettings Enable="true" 
                              Format="<b>${point.x}</b><br/>${series.name}: <b>${point.y}K</b>">
        </ChartTooltipSettings>
        <ChartLegendSettings Visible="true" Position="LegendPosition.Top">
        </ChartLegendSettings>
        <ChartSeriesCollection>
            <ChartSeries DataSource="@SalesData" Name="Product A" 
                         XName="Month" YName="ProductA" 
                         Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line" Width="3" 
                         Fill="#0066CC">
                <ChartMarker Visible="true" Width="10" Height="10" 
                             Shape="ChartShape.Circle">
                </ChartMarker>
                <ChartSeriesDataLabel Visible="true" Position="LabelPosition.Top">
                </ChartSeriesDataLabel>
            </ChartSeries>
            <ChartSeries DataSource="@SalesData" Name="Product B" 
                         XName="Month" YName="ProductB" 
                         Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line" Width="3" 
                         Fill="#FF6B35">
                <ChartMarker Visible="true" Width="10" Height="10" 
                             Shape="ChartShape.Diamond">
                </ChartMarker>
                <ChartSeriesDataLabel Visible="true" Position="LabelPosition.Top">
                </ChartSeriesDataLabel>
            </ChartSeries>
            <ChartSeries DataSource="@SalesData" Name="Product C" 
                         XName="Month" YName="ProductC" 
                         Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line" Width="3" 
                         Fill="#28A745">
                <ChartMarker Visible="true" Width="10" Height="10" 
                             Shape="ChartShape.Triangle">
                </ChartMarker>
                <ChartSeriesDataLabel Visible="true" Position="LabelPosition.Top">
                </ChartSeriesDataLabel>
            </ChartSeries>
        </ChartSeriesCollection>
    </SfChart>
</div>

@code {
    public class SalesInfo
    {
        public string Month { get; set; }
        public double ProductA { get; set; }
        public double ProductB { get; set; }
        public double ProductC { get; set; }
    }

    public List<SalesInfo> SalesData = new List<SalesInfo>
    {
        new SalesInfo { Month = "Jan", ProductA = 35, ProductB = 28, ProductC = 42 },
        new SalesInfo { Month = "Feb", ProductA = 42, ProductB = 35, ProductC = 48 },
        new SalesInfo { Month = "Mar", ProductA = 48, ProductB = 42, ProductC = 55 },
        new SalesInfo { Month = "Apr", ProductA = 55, ProductB = 48, ProductC = 62 },
        new SalesInfo { Month = "May", ProductA = 62, ProductB = 55, ProductC = 68 },
        new SalesInfo { Month = "Jun", ProductA = 68, ProductB = 62, ProductC = 75 },
        new SalesInfo { Month = "Jul", ProductA = 75, ProductB = 68, ProductC = 82 },
        new SalesInfo { Month = "Aug", ProductA = 72, ProductB = 65, ProductC = 78 },
        new SalesInfo { Month = "Sep", ProductA = 78, ProductB = 72, ProductC = 85 },
        new SalesInfo { Month = "Oct", ProductA = 85, ProductB = 78, ProductC = 92 },
        new SalesInfo { Month = "Nov", ProductA = 88, ProductB = 82, ProductC = 95 },
        new SalesInfo { Month = "Dec", ProductA = 92, ProductB = 88, ProductC = 98 }
    };
}
```

---

## 2. Financial Stock Chart (removed)

Note: Financial chart examples (candlestick, HILO, OHLC) have been removed — these chart types are not available in this build.

    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
    }

    public List<StockPrice> StockData = new List<StockPrice>
    {
        new StockPrice { Date = new DateTime(2026, 1, 1), Open = 100, High = 105, Low = 98, Close = 103, Volume = 1200000 },
        new StockPrice { Date = new DateTime(2026, 1, 2), Open = 103, High = 108, Low = 102, Close = 107, Volume = 1350000 },
        new StockPrice { Date = new DateTime(2026, 1, 3), Open = 107, High = 110, Low = 105, Close = 108, Volume = 1400000 },
        new StockPrice { Date = new DateTime(2026, 1, 6), Open = 108, High = 112, Low = 106, Close = 110, Volume = 1500000 },
        new StockPrice { Date = new DateTime(2026, 1, 7), Open = 110, High = 113, Low = 108, Close = 109, Volume = 1300000 },
        new StockPrice { Date = new DateTime(2026, 1, 8), Open = 109, High = 111, Low = 105, Close = 106, Volume = 1450000 },
        new StockPrice { Date = new DateTime(2026, 1, 9), Open = 106, High = 109, Low = 104, Close = 108, Volume = 1250000 },
        new StockPrice { Date = new DateTime(2026, 1, 10), Open = 108, High = 115, Low = 107, Close = 113, Volume = 1600000 },
        new StockPrice { Date = new DateTime(2026, 1, 13), Open = 113, High = 118, Low = 112, Close = 116, Volume = 1700000 },
        new StockPrice { Date = new DateTime(2026, 1, 14), Open = 116, High = 120, Low = 114, Close = 118, Volume = 1800000 },
        new StockPrice { Date = new DateTime(2026, 1, 15), Open = 118, High = 122, Low = 116, Close = 120, Volume = 1650000 },
        new StockPrice { Date = new DateTime(2026, 1, 16), Open = 120, High = 123, Low = 117, Close = 119, Volume = 1550000 },
        new StockPrice { Date = new DateTime(2026, 1, 17), Open = 119, High = 121, Low = 115, Close = 117, Volume = 1400000 },
        new StockPrice { Date = new DateTime(2026, 1, 20), Open = 117, High = 119, Low = 113, Close = 115, Volume = 1350000 }
    };
}
```

---

## 3. Performance Comparison

**Scenario:** Compare performance metrics across multiple teams and quarters.

**When to use:** Performance reviews, team comparisons, quarterly reports.

**Key features:** Grouped column chart, multiple categories, selection, highlighting.

```razor
@page "/performance-comparison"
@using Syncfusion.Blazor.Charts

<div class="performance-container">
    <h2>Quarterly Performance Comparison</h2>
    <SfChart Title="Team Performance Metrics" Width="100%" Height="450px" 
             SelectionMode="Syncfusion.Blazor.Charts.SelectionMode.Point">
        <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category" 
                           Title="Quarters">
        </ChartPrimaryXAxis>
        <ChartPrimaryYAxis Title="Performance Score" Minimum="0" Maximum="100" 
                           Interval="20">
        </ChartPrimaryYAxis>
        <ChartTooltipSettings Enable="true" 
                              Format="<b>${point.x}</b><br/>${series.name}: <b>${point.y}%</b>">
        </ChartTooltipSettings>
        <ChartLegendSettings Visible="true" Position="LegendPosition.Top">
        </ChartLegendSettings>
        <ChartEvents OnPointClick="OnPointClick"></ChartEvents>
        <ChartSeriesCollection>
            <ChartSeries DataSource="@PerformanceData" Name="Team Alpha" 
                         XName="Quarter" YName="TeamAlpha" 
                         Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" 
                         ColumnWidth="0.6" ColumnSpacing="0.2" 
                         Fill="#6366F1">
                <ChartSeriesDataLabel Visible="true" Position="LabelPosition.Top">
                </ChartSeriesDataLabel>
            </ChartSeries>
            <ChartSeries DataSource="@PerformanceData" Name="Team Beta" 
                         XName="Quarter" YName="TeamBeta" 
                         Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" 
                         ColumnWidth="0.6" ColumnSpacing="0.2" 
                         Fill="#EC4899">
                <ChartSeriesDataLabel Visible="true" Position="LabelPosition.Top">
                </ChartSeriesDataLabel>
            </ChartSeries>
            <ChartSeries DataSource="@PerformanceData" Name="Team Gamma" 
                         XName="Quarter" YName="TeamGamma" 
                         Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" 
                         ColumnWidth="0.6" ColumnSpacing="0.2" 
                         Fill="#10B981">
                <ChartSeriesDataLabel Visible="true" Position="LabelPosition.Top">
                </ChartSeriesDataLabel>
            </ChartSeries>
        </ChartSeriesCollection>
    </SfChart>
    @if (!string.IsNullOrEmpty(SelectedInfo))
    {
        <div class="selection-info">
            <p>@SelectedInfo</p>
        </div>
    }
</div>

@code {
    public string SelectedInfo = "";

    public class PerformanceMetric
    {
        public string Quarter { get; set; }
        public double TeamAlpha { get; set; }
        public double TeamBeta { get; set; }
        public double TeamGamma { get; set; }
    }

    public List<PerformanceMetric> PerformanceData = new List<PerformanceMetric>
    {
        new PerformanceMetric { Quarter = "Q1 2025", TeamAlpha = 75, TeamBeta = 70, TeamGamma = 80 },
        new PerformanceMetric { Quarter = "Q2 2025", TeamAlpha = 80, TeamBeta = 75, TeamGamma = 85 },
        new PerformanceMetric { Quarter = "Q3 2025", TeamAlpha = 85, TeamBeta = 82, TeamGamma = 88 },
        new PerformanceMetric { Quarter = "Q4 2025", TeamAlpha = 90, TeamBeta = 88, TeamGamma = 92 }
    };

    public void OnPointClick(PointEventArgs args)
    {
        SelectedInfo = $"Selected: {args.SeriesName} - {args.PointIndex} with value {args.Y}%";
        StateHasChanged();
    }
}
```

---

## 4. Trend Analysis

**Scenario:** Analyze sales trends with forecasting and key milestone annotations.

**When to use:** Trend forecasting, data analysis, strategic planning.

**Key features:** Area chart, trend lines, forecasting, annotations.

```razor
@page "/trend-analysis"
@using Syncfusion.Blazor.Charts

<div class="trend-container">
    <h2>Sales Trend Analysis with Forecast</h2>
    <SfChart Title="Revenue Trend & Forecast" Width="100%" Height="450px">
        <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category" 
                           Title="Months">
        </ChartPrimaryXAxis>
        <ChartPrimaryYAxis Title="Revenue (in millions)" Minimum="0" Maximum="150" 
                           LabelFormat="${value}M">
        </ChartPrimaryYAxis>
        <ChartTooltipSettings Enable="true">
        </ChartTooltipSettings>
        <ChartSeriesCollection>
            <ChartSeries DataSource="@TrendData" Name="Actual Revenue" 
                         XName="Month" YName="Revenue" 
                         Type="Syncfusion.Blazor.Charts.ChartSeriesType.Area" 
                         Fill="rgba(99, 102, 241, 0.5)" 
                         Border-Width="2" Border-Color="#6366F1">
                <ChartMarker Visible="true" Width="8" Height="8">
                </ChartMarker>
                <ChartTrendline Type="TrendlineTypes.Linear" Width="3" 
                               Name="Growth Trend" Fill="#FF6B35" 
                               ForwardForecast="3" BackwardForecast="0">
                </ChartTrendline>
            </ChartSeries>
        </ChartSeriesCollection>
        <ChartAnnotations>
            <ChartAnnotation X="Jun" Y="95" CoordinateUnits="Units.Point">
                <ContentTemplate>
                    <div style="background: #FFA500; color: white; padding: 5px; border-radius: 5px;">
                        <b>Product Launch</b>
                    </div>
                </ContentTemplate>
            </ChartAnnotation>
            <ChartAnnotation X="Dec" Y="120" CoordinateUnits="Units.Point">
                <ContentTemplate>
                    <div style="background: #28A745; color: white; padding: 5px; border-radius: 5px;">
                        <b>Holiday Peak</b>
                    </div>
                </ContentTemplate>
            </ChartAnnotation>
        </ChartAnnotations>
    </SfChart>
</div>

@code {
    public class TrendInfo
    {
        public string Month { get; set; }
        public double Revenue { get; set; }
    }

    public List<TrendInfo> TrendData = new List<TrendInfo>
    {
        new TrendInfo { Month = "Jan", Revenue = 45 },
        new TrendInfo { Month = "Feb", Revenue = 52 },
        new TrendInfo { Month = "Mar", Revenue = 58 },
        new TrendInfo { Month = "Apr", Revenue = 65 },
        new TrendInfo { Month = "May", Revenue = 72 },
        new TrendInfo { Month = "Jun", Revenue = 95 },
        new TrendInfo { Month = "Jul", Revenue = 88 },
        new TrendInfo { Month = "Aug", Revenue = 92 },
        new TrendInfo { Month = "Sep", Revenue = 98 },
        new TrendInfo { Month = "Oct", Revenue = 105 },
        new TrendInfo { Month = "Nov", Revenue = 112 },
        new TrendInfo { Month = "Dec", Revenue = 120 }
    };
}
```

---

## 5. Regional Distribution

**Regional Distribution (removed)**

Note: Accumulation/Pie/Doughnut charts are not available in this build; regional-distribution examples have been removed.


---

## 6. Real-Time Monitoring

**Scenario:** Monitor live metrics with auto-updating data stream.

**When to use:** System monitoring, IoT dashboards, live sensor data.

**Key features:** Live updates, dynamic data binding, auto-scroll, ObservableCollection.

```razor
@page "/realtime-monitoring"
@using Syncfusion.Blazor.Charts
@using System.Collections.ObjectModel
@using System.Timers

<div class="monitoring-container">
    <h2>Server CPU Monitoring (Live)</h2>
    <button class="btn btn-primary" @onclick="ToggleMonitoring">
        @(isMonitoring ? "Stop Monitoring" : "Start Monitoring")
    </button>
    <SfChart @ref="liveChart" Title="CPU Usage (%)" Width="100%" Height="450px">
        <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.DateTime" 
                           LabelFormat="HH:mm:ss" Title="Time">
            <ChartAxisMajorGridLines Width="0"></ChartAxisMajorGridLines>
        </ChartPrimaryXAxis>
        <ChartPrimaryYAxis Title="CPU (%)" Minimum="0" Maximum="100" Interval="20">
        </ChartPrimaryYAxis>
        <ChartTooltipSettings Enable="true" Format="<b>${point.x}</b><br/>CPU: <b>${point.y}%</b>">
        </ChartTooltipSettings>
        <ChartSeriesCollection>
            <ChartSeries DataSource="@LiveData" Name="CPU Usage" 
                         XName="Timestamp" YName="Value" 
                         Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line" Width="3" 
                         Fill="#FF4560">
                <ChartSeriesAnimation Enable="false"></ChartSeriesAnimation>
                <ChartMarker Visible="true" Width="6" Height="6">
                </ChartMarker>
            </ChartSeries>
        </ChartSeriesCollection>
    </SfChart>
    <div class="stats">
        <p>Current: @CurrentValue% | Average: @AverageValue% | Peak: @PeakValue%</p>
    </div>
</div>

@code {
    private SfChart liveChart;
    private Timer updateTimer;
    private Random random = new Random();
    private bool isMonitoring = false;
    private int maxDataPoints = 20;

    public ObservableCollection<MetricData> LiveData = new ObservableCollection<MetricData>();

    protected override void OnInitialized()
    {
        // Initialize with sample data
        for (int i = 0; i < maxDataPoints; i++)
        {
            LiveData.Add(new MetricData
            {
                Timestamp = DateTime.Now.AddSeconds(i - maxDataPoints),
                Value = random.Next(20, 60)
            });
        }
    }

    private void ToggleMonitoring()
    {
        if (isMonitoring)
        {
            StopMonitoring();
        }
        else
        {
            StartMonitoring();
        }
    }

    private void StartMonitoring()
    {
        isMonitoring = true;
        updateTimer = new Timer(1000); // Update every second
        updateTimer.Elapsed += UpdateData;
        updateTimer.AutoReset = true;
        updateTimer.Enabled = true;
    }

    private void StopMonitoring()
    {
        isMonitoring = false;
        updateTimer?.Stop();
        updateTimer?.Dispose();
    }

    private void UpdateData(object source, ElapsedEventArgs e)
    {
        if (liveChart == null) return;

        LiveData.RemoveAt(0);
        LiveData.Add(new MetricData
        {
            Timestamp = DateTime.Now,
            Value = random.Next(15, 95)
        });
        InvokeAsync(StateHasChanged);
    }

    public double CurrentValue => LiveData.LastOrDefault()?.Value ?? 0;
    public double AverageValue => Math.Round(LiveData.Average(x => x.Value), 1);
    public double PeakValue => LiveData.Max(x => x.Value);

    public class MetricData
    {
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }

    public void Dispose()
    {
        updateTimer?.Dispose();
    }
}
```

---

## 7. Interactive Report

**Scenario:** Create interactive chart with drill-down and export capabilities.

**When to use:** Business reports, executive dashboards, data exploration.

**Key features:** Drill-down, export to PDF/Excel, print support, point click events.

```razor
@page "/interactive-report"
@using Syncfusion.Blazor.Charts

<div class="report-container">
    <h2>Interactive Sales Report</h2>
    <div class="toolbar">
        <button class="btn btn-secondary" @onclick="ExportToPDF">Export PDF</button>
        <button class="btn btn-secondary" @onclick="ExportToExcel">Export Excel</button>
        <button class="btn btn-secondary" @onclick="PrintChart">Print</button>
        @if (isDrilledDown)
        {
            <button class="btn btn-primary" @onclick="DrillUp">← Back to Categories</button>
        }
    </div>
    <SfChart @ref="reportChart" Title="@ChartTitle" Width="100%" Height="450px">
        <ChartEvents OnPointClick="OnDrillDown"></ChartEvents>
        <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category" 
                           Title="@XAxisTitle">
        </ChartPrimaryXAxis>
        <ChartPrimaryYAxis Title="Sales (in thousands)" LabelFormat="${value}K">
        </ChartPrimaryYAxis>
        <ChartTooltipSettings Enable="true">
        </ChartTooltipSettings>
        <ChartSeriesCollection>
            <ChartSeries DataSource="@CurrentData" XName="Category" YName="Sales" 
                         Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" 
                         ColumnWidth="0.7" 
                         Fill="#6366F1">
                <ChartSeriesDataLabel Visible="true" Position="LabelPosition.Top">
                </ChartSeriesDataLabel>
            </ChartSeries>
        </ChartSeriesCollection>
    </SfChart>
</div>

@code {
    private SfChart reportChart;
    private bool isDrilledDown = false;
    private string selectedCategory = "";

    public string ChartTitle => isDrilledDown ? $"{selectedCategory} - Product Details" : "Sales by Category";
    public string XAxisTitle => isDrilledDown ? "Products" : "Categories";

    public class SalesData
    {
        public string Category { get; set; }
        public double Sales { get; set; }
    }

    public List<SalesData> CategoryData = new List<SalesData>
    {
        new SalesData { Category = "Electronics", Sales = 245 },
        new SalesData { Category = "Clothing", Sales = 180 },
        new SalesData { Category = "Home & Garden", Sales = 165 },
        new SalesData { Category = "Sports", Sales = 140 }
    };

    public Dictionary<string, List<SalesData>> ProductData = new Dictionary<string, List<SalesData>>
    {
        ["Electronics"] = new List<SalesData>
        {
            new SalesData { Category = "Laptops", Sales = 95 },
            new SalesData { Category = "Phones", Sales = 85 },
            new SalesData { Category = "Tablets", Sales = 65 }
        },
        ["Clothing"] = new List<SalesData>
        {
            new SalesData { Category = "Shirts", Sales = 75 },
            new SalesData { Category = "Pants", Sales = 55 },
            new SalesData { Category = "Shoes", Sales = 50 }
        },
        ["Home & Garden"] = new List<SalesData>
        {
            new SalesData { Category = "Furniture", Sales = 90 },
            new SalesData { Category = "Decor", Sales = 45 },
            new SalesData { Category = "Garden Tools", Sales = 30 }
        },
        ["Sports"] = new List<SalesData>
        {
            new SalesData { Category = "Fitness", Sales = 60 },
            new SalesData { Category = "Outdoor", Sales = 50 },
            new SalesData { Category = "Team Sports", Sales = 30 }
        }
    };

    public List<SalesData> CurrentData => isDrilledDown ? ProductData[selectedCategory] : CategoryData;

    private void OnDrillDown(PointEventArgs args)
    {
        if (!isDrilledDown)
        {
            selectedCategory = args.X.ToString();
            isDrilledDown = true;
            StateHasChanged();
        }
    }

    private void DrillUp()
    {
        isDrilledDown = false;
        selectedCategory = "";
        StateHasChanged();
    }

    private async Task ExportToPDF()
    {
        await reportChart.ExportAsync(ExportType.PDF, "SalesReport");
    }

    private async Task ExportToExcel()
    {
        await reportChart.ExportAsync(ExportType.CSV, "SalesReport");
    }

    private async Task PrintChart()
    {
        await reportChart.PrintAsync();
    }
}
```

---

## 8. Responsive Analytics

**Scenario:** Mobile-friendly analytics dashboard with adaptive layout.

**When to use:** Mobile apps, responsive web apps, cross-device dashboards.

**Key features:** Adaptive layout, touch interactions, mobile optimization, responsive sizing.

```razor
@page "/responsive-analytics"
@using Syncfusion.Blazor.Charts

<div class="analytics-container">
    <h2>Mobile-Responsive Analytics</h2>
    <div class="responsive-grid">
        <SfChart Title="Monthly Visitors" Width="100%" Height="300px">
            <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category">
            </ChartPrimaryXAxis>
            <ChartPrimaryYAxis LabelFormat="{value}K">
            </ChartPrimaryYAxis>
            <ChartTooltipSettings Enable="true">
            </ChartTooltipSettings>
            <ChartSeriesCollection>
                <ChartSeries DataSource="@VisitorData" XName="Month" YName="Visitors" 
                             Type="Syncfusion.Blazor.Charts.ChartSeriesType.SplineArea" 
                             Fill="rgba(99, 102, 241, 0.6)">
                </ChartSeries>
            </ChartSeriesCollection>
        </SfChart>

        <!-- Accumulation/Pie examples removed: not available in this build -->

        <SfChart Title="Device Usage" Width="100%" Height="300px">
            <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category">
            </ChartPrimaryXAxis>
            <ChartTooltipSettings Enable="true">
            </ChartTooltipSettings>
            <ChartSeriesCollection>
                <ChartSeries DataSource="@DeviceData" XName="Device" YName="Users" 
                             Type="Syncfusion.Blazor.Charts.ChartSeriesType.Bar" 
                             Fill="#10B981">
                    <ChartSeriesDataLabel Visible="true" Position="LabelPosition.Top">
                    </ChartSeriesDataLabel>
                </ChartSeries>
            </ChartSeriesCollection>
        </SfChart>
    </div>
</div>

<style>
    .analytics-container {
        padding: 20px;
    }
    
    .responsive-grid {
        display: grid;
        gap: 20px;
        grid-template-columns: 1fr;
    }
    
    @@media (min-width: 768px) {
        .responsive-grid {
            grid-template-columns: repeat(2, 1fr);
        }
    }
    
    @@media (min-width: 1200px) {
        .responsive-grid {
            grid-template-columns: repeat(3, 1fr);
        }
    }
</style>

@code {
    public class AnalyticsData
    {
        public string Month { get; set; }
        public double Visitors { get; set; }
    }

    public class TrafficSource
    {
        public string Source { get; set; }
        public double Percentage { get; set; }
    }

    public class DeviceStats
    {
        public string Device { get; set; }
        public double Users { get; set; }
    }

    public List<AnalyticsData> VisitorData = new List<AnalyticsData>
    {
        new AnalyticsData { Month = "Jan", Visitors = 45 },
        new AnalyticsData { Month = "Feb", Visitors = 52 },
        new AnalyticsData { Month = "Mar", Visitors = 68 },
        new AnalyticsData { Month = "Apr", Visitors = 75 },
        new AnalyticsData { Month = "May", Visitors = 82 },
        new AnalyticsData { Month = "Jun", Visitors = 95 }
    };

    public List<TrafficSource> TrafficData = new List<TrafficSource>
    {
        new TrafficSource { Source = "Organic", Percentage = 45 },
        new TrafficSource { Source = "Direct", Percentage = 25 },
        new TrafficSource { Source = "Social", Percentage = 20 },
        new TrafficSource { Source = "Referral", Percentage = 10 }
    };

    public List<DeviceStats> DeviceData = new List<DeviceStats>
    {
        new DeviceStats { Device = "Desktop", Users = 125 },
        new DeviceStats { Device = "Mobile", Users = 185 },
        new DeviceStats { Device = "Tablet", Users = 65 }
    };
}
```

---

## Summary

These practical examples demonstrate complete, production-ready implementations of Syncfusion Blazor Charts for common business scenarios. Each example includes:

- **Complete razor page code** with all necessary imports
- **Data model classes** properly structured
- **Sample data** for immediate testing
- **Key features** fully configured
- **Real-world scenarios** for practical application

Copy any example directly into your Blazor application and customize as needed. All examples are self-contained and require only the Syncfusion.Blazor.Charts package.
