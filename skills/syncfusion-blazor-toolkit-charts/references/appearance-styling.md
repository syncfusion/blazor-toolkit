# Syncfusion Blazor Charts - Appearance and Styling Reference

This comprehensive reference guide covers all aspects of appearance customization and styling for Syncfusion Blazor Charts. Use this guide to create visually appealing and responsive chart visualizations.

## Table of Contents

- [Chart Dimensions](#chart-dimensions)
   - [Default Dimensions](#default-dimensions)
   - [Pixel-Based Sizing](#pixel-based-sizing)
   - [Percentage-Based Sizing](#percentage-based-sizing)
   - [Container-Based Sizing](#container-based-sizing)
- [Color Customization](#color-customization)
   - [Custom Color Palette](#custom-color-palette)
   - [Series-Specific Colors](#series-specific-colors)
   - [Point-Level Color Customization](#point-level-color-customization)
- [Chart Background and Border](#chart-background-and-border)
   - [Background Color](#background-color)
   - [Border Styling](#border-styling)
   - [Combined Background and Border](#combined-background-and-border)
- [Chart Area Customization](#chart-area-customization)
   - [Chart Area Background](#chart-area-background)
   - [Chart Area Border](#chart-area-border)
   - [Chart Area Width](#chart-area-width)
- [Chart Margins](#chart-margins)
   - [Basic Margin Configuration](#basic-margin-configuration)
   - [Asymmetric Margins](#asymmetric-margins)
- [Title and Subtitle Styling](#title-and-subtitle-styling)
   - [Title Styling](#title-styling)
   - [Title and Subtitle Combined](#title-and-subtitle-combined)
   - [Title Position Customization](#title-position-customization)
- [Theme Support](#theme-support)
   - [Built-in Themes](#built-in-themes)
   - [Custom Theme Colors](#custom-theme-colors)
- [Series Styling](#series-styling)
   - [Fill Color and Opacity](#fill-color-and-opacity)
   - [Series Border Styling](#series-border-styling)
   - [Series Animation](#series-animation)
- [Chart Responsiveness](#chart-responsiveness)
   - [Responsive Percentage Sizing](#responsive-percentage-sizing)
   - [Container-Based Responsive Charts](#container-based-responsive-charts)
- [Styling Best Practices](#styling-best-practices)
   - [Design Guidelines](#design-guidelines)
   - [Performance Tips](#performance-tips)
   - [Accessibility Considerations](#accessibility-considerations)
   - [Troubleshooting Common Issues](#troubleshooting-common-issues)
- [Complete Styling Example](#complete-styling-example)
- [Summary](#summary)


## Chart Dimensions

Control the size of your chart using width and height properties. Charts can be sized using pixels, percentages, or container-based sizing.

### Default Dimensions

When no size is specified, charts render with default dimensions:
- **Default Width**: 600px
- **Default Height**: 450px

### Pixel-Based Sizing

Define exact chart dimensions in pixels for precise control:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Sales Data" Width="800px" Height="400px">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Revenue" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
</SfChart>

@code {
    public class SalesInfo
    {
        public string Month { get; set; }
        public double Revenue { get; set; }
    }
    
    public List<SalesInfo> SalesData = new List<SalesInfo>
    {
        new SalesInfo { Month = "Jan", Revenue = 35 },
        new SalesInfo { Month = "Feb", Revenue = 28 },
        new SalesInfo { Month = "Mar", Revenue = 34 },
        new SalesInfo { Month = "Apr", Revenue = 32 },
        new SalesInfo { Month = "May", Revenue = 40 }
    };
}
```

### Percentage-Based Sizing

Use percentages to make charts responsive to their container:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Revenue Analysis" Width="80%" Height="90%">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@RevenueData" XName="Quarter" YName="Amount" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line" />
</SfChart>

@code {
    public class RevenueInfo
    {
        public string Quarter { get; set; }
        public double Amount { get; set; }
    }
    
    public List<RevenueInfo> RevenueData = new List<RevenueInfo>
    {
        new RevenueInfo { Quarter = "Q1", Amount = 120000 },
        new RevenueInfo { Quarter = "Q2", Amount = 135000 },
        new RevenueInfo { Quarter = "Q3", Amount = 145000 },
        new RevenueInfo { Quarter = "Q4", Amount = 160000 }
    };
}
```

### Container-Based Sizing

Scale charts to fit within a container using CSS styles:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<div style="width: 100%; height: 500px; background-color: #f5f5f5; padding: 20px;">
    <SfChart Title="Performance Metrics">
        <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
        
            <ChartSeries DataSource="@MetricsData" XName="Metric" YName="Value" 
                         Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Bar" />
    </SfChart>
</div>

@code {
    public class MetricInfo
    {
        public string Metric { get; set; }
        public double Value { get; set; }
    }
    
    public List<MetricInfo> MetricsData = new List<MetricInfo>
    {
        new MetricInfo { Metric = "Speed", Value = 85 },
        new MetricInfo { Metric = "Quality", Value = 92 },
        new MetricInfo { Metric = "Efficiency", Value = 78 }
    };
}
```

---

## Color Customization

Control chart colors through color palettes, series-specific colors, and point-level customization.

### Custom Color Palette

Define a custom palette to apply colors consistently across all series:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Olympic Medals" Palettes="@CustomPalette">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@MedalData" XName="Country" YName="Gold" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
        <ChartSeries DataSource="@MedalData" XName="Country" YName="Silver" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
        <ChartSeries DataSource="@MedalData" XName="Country" YName="Bronze" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
</SfChart>

@code {
    public class MedalInfo
    {
        public string Country { get; set; }
        public double Gold { get; set; }
        public double Silver { get; set; }
        public double Bronze { get; set; }
    }
    
    public List<MedalInfo> MedalData = new List<MedalInfo>
    {
        new MedalInfo { Country = "USA", Gold = 46, Silver = 37, Bronze = 38 },
        new MedalInfo { Country = "China", Gold = 38, Silver = 32, Bronze = 18 },
        new MedalInfo { Country = "UK", Gold = 27, Silver = 23, Bronze = 17 },
        new MedalInfo { Country = "Japan", Gold = 27, Silver = 14, Bronze = 17 }
    };
    
    public string[] CustomPalette = new string[] { "#FFD700", "#C0C0C0", "#CD7F32" };
}
```

### Series-Specific Colors

Apply individual colors to each series using the Fill property:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Product Sales Comparison">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@ProductData" XName="Product" YName="Sales" 
                     Fill="#00bdae" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
        <ChartSeries DataSource="@ProductData" XName="Product" YName="Target" 
                     Fill="#e56590" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
</SfChart>

@code {
    public class ProductInfo
    {
        public string Product { get; set; }
        public double Sales { get; set; }
        public double Target { get; set; }
    }
    
    public List<ProductInfo> ProductData = new List<ProductInfo>
    {
        new ProductInfo { Product = "Laptop", Sales = 120, Target = 150 },
        new ProductInfo { Product = "Phone", Sales = 210, Target = 200 },
        new ProductInfo { Product = "Tablet", Sales = 80, Target = 100 }
    };
}
```

### Point-Level Color Customization

Customize individual data point colors:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Project Status">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@ProjectData" XName="Phase" YName="Completion" 
                     PointColorMapping="Color" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
</SfChart>

@code {
    public class ProjectPhase
    {
        public string Phase { get; set; }
        public double Completion { get; set; }
        public string Color { get; set; }
    }
    
    public List<ProjectPhase> ProjectData = new List<ProjectPhase>
    {
        new ProjectPhase { Phase = "Planning", Completion = 100, Color = "#4CAF50" },
        new ProjectPhase { Phase = "Development", Completion = 75, Color = "#FF9800" },
        new ProjectPhase { Phase = "Testing", Completion = 40, Color = "#F44336" },
        new ProjectPhase { Phase = "Deployment", Completion = 0, Color = "#9E9E9E" }
    };
}
```

---

## Chart Background and Border

Customize the overall appearance of your chart with background colors and borders.

### Background Color

Set the chart background using the Background property:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Temperature Analysis" Background="#E8F5E9">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@TemperatureData" XName="Month" YName="Temp" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line" />
</SfChart>

@code {
    public class TempInfo
    {
        public string Month { get; set; }
        public double Temp { get; set; }
    }
    
    public List<TempInfo> TemperatureData = new List<TempInfo>
    {
        new TempInfo { Month = "Jan", Temp = 15 },
        new TempInfo { Month = "Feb", Temp = 18 },
        new TempInfo { Month = "Mar", Temp = 22 },
        new TempInfo { Month = "Apr", Temp = 26 }
    };
}
```

### Border Styling

Add borders to your chart with customizable color and width:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Revenue Growth" Background="#FFFFFF">
    <ChartBorder Color="#4A90E2" Width="3" />
    
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@RevenueData" XName="Year" YName="Revenue" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Area" />
</SfChart>

@code {
    public class YearRevenue
    {
        public string Year { get; set; }
        public double Revenue { get; set; }
    }
    
    public List<YearRevenue> RevenueData = new List<YearRevenue>
    {
        new YearRevenue { Year = "2020", Revenue = 1200000 },
        new YearRevenue { Year = "2021", Revenue = 1450000 },
        new YearRevenue { Year = "2022", Revenue = 1680000 },
        new YearRevenue { Year = "2023", Revenue = 1920000 }
    };
}
```


---

## Chart Area Customization

The chart area is the region where data is plotted. Customize its background, border, and width.

### Chart Area Background

Set a distinct background for the plotting area:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Sales Performance">
    <ChartArea Background="#F0F8FF">
    </ChartArea>
    
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@SalesData" XName="Quarter" YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
</SfChart>

@code {
    public class QuarterlySales
    {
        public string Quarter { get; set; }
        public double Sales { get; set; }
    }
    
    public List<QuarterlySales> SalesData = new List<QuarterlySales>
    {
        new QuarterlySales { Quarter = "Q1", Sales = 150000 },
        new QuarterlySales { Quarter = "Q2", Sales = 175000 },
        new QuarterlySales { Quarter = "Q3", Sales = 195000 },
        new QuarterlySales { Quarter = "Q4", Sales = 220000 }
    };
}
```

### Chart Area Border

Add a border around the chart plotting area:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Customer Analytics">
    <ChartArea Background="#FFFACD">
        <ChartAreaBorder Color="#4169E1" Width="2" />
    </ChartArea>
    
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@CustomerData" XName="Month" YName="Customers" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.SplineArea" />
</SfChart>

@code {
    public class CustomerInfo
    {
        public string Month { get; set; }
        public double Customers { get; set; }
    }
    
    public List<CustomerInfo> CustomerData = new List<CustomerInfo>
    {
        new CustomerInfo { Month = "Jan", Customers = 1200 },
        new CustomerInfo { Month = "Feb", Customers = 1450 },
        new CustomerInfo { Month = "Mar", Customers = 1680 },
        new CustomerInfo { Month = "Apr", Customers = 1920 }
    };
}
```

### Chart Area Width

Control the width of the chart area as a percentage:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Expense Distribution">
    <ChartArea Background="#FFF5EE" Width="70%">
        <ChartAreaBorder Color="#DC143C" Width="1" />
    </ChartArea>
    
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@ExpenseData" XName="Category" YName="Amount" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
</SfChart>

@code {
    public class ExpenseCategory
    {
        public string Category { get; set; }
        public double Amount { get; set; }
    }
    
    public List<ExpenseCategory> ExpenseData = new List<ExpenseCategory>
    {
        new ExpenseCategory { Category = "Housing", Amount = 2500 },
        new ExpenseCategory { Category = "Food", Amount = 800 },
        new ExpenseCategory { Category = "Transport", Amount = 500 },
        new ExpenseCategory { Category = "Entertainment", Amount = 400 }
    };
}
```

---

## Chart Margins

Configure spacing between the chart and its container using margin properties.

### Basic Margin Configuration

Set uniform margins around the chart:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Traffic Analysis" Background="#F5F5F5">
    <ChartMargin Left="40" Right="40" Top="40" Bottom="40" />
    <ChartBorder Color="#333333" Width="1" />
    
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@TrafficData" XName="Hour" YName="Visitors" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line" />
</SfChart>

@code {
    public class TrafficInfo
    {
        public string Hour { get; set; }
        public double Visitors { get; set; }
    }
    
    public List<TrafficInfo> TrafficData = new List<TrafficInfo>
    {
        new TrafficInfo { Hour = "8 AM", Visitors = 320 },
        new TrafficInfo { Hour = "12 PM", Visitors = 580 },
        new TrafficInfo { Hour = "4 PM", Visitors = 720 },
        new TrafficInfo { Hour = "8 PM", Visitors = 450 }
    };
}
```

### Asymmetric Margins

Apply different margins to each side:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Investment Portfolio" Background="#E0F7FA">
    <ChartMargin Left="80" Right="20" Top="60" Bottom="40" />
    <ChartBorder Color="#0097A7" Width="2" />
    
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@InvestmentData" XName="Asset" YName="Value" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Bar" />
</SfChart>

@code {
    public class AssetInfo
    {
        public string Asset { get; set; }
        public double Value { get; set; }
    }
    
    public List<AssetInfo> InvestmentData = new List<AssetInfo>
    {
        new AssetInfo { Asset = "Stocks", Value = 45000 },
        new AssetInfo { Asset = "Bonds", Value = 30000 },
        new AssetInfo { Asset = "Real Estate", Value = 80000 },
        new AssetInfo { Asset = "Commodities", Value = 15000 }
    };
}
```

---

## Title and Subtitle Styling

Customize chart titles and subtitles with font properties, colors, and positioning.

### Title Styling

Apply custom styling to the chart title:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Annual Revenue Report">
    <ChartTitleStyle Size="24px" Color="#1976D2" FontFamily="Segoe UI" 
                     FontWeight="600" FontStyle="normal" />
    
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@AnnualData" XName="Year" YName="Revenue" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
</SfChart>

@code {
    public class AnnualRevenue
    {
        public string Year { get; set; }
        public double Revenue { get; set; }
    }
    
    public List<AnnualRevenue> AnnualData = new List<AnnualRevenue>
    {
        new AnnualRevenue { Year = "2020", Revenue = 850000 },
        new AnnualRevenue { Year = "2021", Revenue = 920000 },
        new AnnualRevenue { Year = "2022", Revenue = 1050000 },
        new AnnualRevenue { Year = "2023", Revenue = 1180000 }
    };
}
```

### Title and Subtitle Combined

Add both title and subtitle with distinct styling:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Employee Satisfaction Survey" SubTitle="2023 Annual Results">
    <ChartTitleStyle Size="22px" Color="#E91E63" FontFamily="Arial" 
                     FontWeight="bold" FontStyle="normal" />
    <ChartSubTitleStyle Size="16px" Color="#757575" FontFamily="Arial" 
                        FontWeight="normal" FontStyle="italic" />
    
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@SurveyData" XName="Department" YName="Score" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
</SfChart>

@code {
    public class SurveyResult
    {
        public string Department { get; set; }
        public double Score { get; set; }
    }
    
    public List<SurveyResult> SurveyData = new List<SurveyResult>
    {
        new SurveyResult { Department = "Engineering", Score = 4.5 },
        new SurveyResult { Department = "Sales", Score = 4.2 },
        new SurveyResult { Department = "HR", Score = 4.8 },
        new SurveyResult { Department = "Finance", Score = 4.3 }
    };
}
```

### Title Position Customization

Position the title at different locations:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Monthly Website Traffic" SubTitle="Visitor Analytics">
    <ChartTitleStyle Position="Syncfusion.Blazor.Toolkit.ChartTitlePosition.Bottom" Size="20px"
                     Color="#2E7D32" FontFamily="Verdana" />
    
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@TrafficData" XName="Month" YName="Visits" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.SplineArea" />
</SfChart>

@code {
    public class WebTraffic
    {
        public string Month { get; set; }
        public double Visits { get; set; }
    }
    
    public List<WebTraffic> TrafficData = new List<WebTraffic>
    {
        new WebTraffic { Month = "Jan", Visits = 12500 },
        new WebTraffic { Month = "Feb", Visits = 14200 },
        new WebTraffic { Month = "Mar", Visits = 16800 },
        new WebTraffic { Month = "Apr", Visits = 18300 }
    };
}
```

---

## Theme Support

Syncfusion Blazor Charts support built-in themes and custom theme creation.

### Built-in Themes

Apply predefined themes by including the theme API:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Product Performance" Theme="Syncfusion.Blazor.Theme.Bootstrap5">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@PerformanceData" XName="Product" YName="Rating" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
</SfChart>

@code {
    public class ProductPerformance
    {
        public string Product { get; set; }
        public double Rating { get; set; }
    }
    
    public List<ProductPerformance> PerformanceData = new List<ProductPerformance>
    {
        new ProductPerformance { Product = "Product A", Rating = 4.5 },
        new ProductPerformance { Product = "Product B", Rating = 3.8 },
        new ProductPerformance { Product = "Product C", Rating = 4.2 },
        new ProductPerformance { Product = "Product D", Rating = 4.7 }
    };
}
```

**Available Built-in Themes:**
- Material
- Bootstrap
- Bootstrap 4
- Bootstrap 5
- Fabric
- Tailwind
- Material 3
- Fluent
- High Contrast

### Custom Theme Colors

Create a custom appearance by overriding default palette:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Regional Sales" Palettes="@CustomThemePalette">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries Deries DataSource ource="@RegionalData" XName="Region" YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
</SfChart>

@code {
    public class RegionalSales
    {
        public string Region { get; set; }
        public double Sales { get; set; }
    }
    
    public List<RegionalSales> RegionalData = new List<RegionalSales>
    {
        new RegionalSales { Region = "North", Sales = 125000 },
        new RegionalSales { Region = "South", Sales = 98000 },
        new RegionalSales { Region = "East", Sales = 112000 },
        new RegionalSales { Region = "West", Sales = 135000 }
    };
    
    // Custom corporate color palette
    public string[] CustomThemePalette = new string[] 
    { 
        "#0066CC", "#FF6600", "#339933", "#CC0066", "#9933FF" 
    };
}
```

---

## Series Styling

Customize individual series appearance including fill colors, opacity, and borders.

### Fill Color and Opacity

Control series fill color and transparency:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Quarterly Comparison">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@ComparisonData" XName="Quarter" YName="Current" 
                     Fill="#FF5733" Opacity="0.8" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
        <ChartSeries DataSource="@ComparisonData" XName="Quarter" YName="Previous" 
                     Fill="#33FF57" Opacity="0.6" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
</SfChart>

@code {
    public class QuarterComparison
    {
        public string Quarter { get; set; }
        public double Current { get; set; }
        public double Previous { get; set; }
    }
    
    public List<QuarterComparison> ComparisonData = new List<QuarterComparison>
    {
        new QuarterComparison { Quarter = "Q1", Current = 85000, Previous = 78000 },
        new QuarterComparison { Quarter = "Q2", Current = 92000, Previous = 85000 },
        new QuarterComparison { Quarter = "Q3", Current = 98000, Previous = 90000 },
        new QuarterComparison { Quarter = "Q4", Current = 105000, Previous = 95000 }
    };
}
```

### Series Border Styling

Add borders to series elements:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Market Trends">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@TrendData" XName="Month" YName="Value" 
                     Fill="#4A90E2" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
            <ChartSeriesBorder Color="#2E5C8A" Width="2" />
        </ChartSeries>
</SfChart>

@code {
    public class TrendInfo
    {
        public string Month { get; set; }
        public double Value { get; set; }
    }
    
    public List<TrendInfo> TrendData = new List<TrendInfo>
    {
        new TrendInfo { Month = "Jan", Value = 450 },
        new TrendInfo { Month = "Feb", Value = 520 },
        new TrendInfo { Month = "Mar", Value = 480 },
        new TrendInfo { Month = "Apr", Value = 590 }
    };
}
```

### Series Animation

Configure animation properties for series:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Growth Analysis">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@GrowthData" XName="Year" YName="Growth" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line" Width="3">
            <ChartSeriesAnimation Enable="true" Duration="2000" Delay="300" />
        </ChartSeries>
</SfChart>

@code {
    public class GrowthInfo
    {
        public string Year { get; set; }
        public double Growth { get; set; }
    }
    
    public List<GrowthInfo> GrowthData = new List<GrowthInfo>
    {
        new GrowthInfo { Year = "2019", Growth = 5.2 },
        new GrowthInfo { Year = "2020", Growth = 6.8 },
        new GrowthInfo { Year = "2021", Growth = 7.5 },
        new GrowthInfo { Year = "2022", Growth = 8.2 },
        new GrowthInfo { Year = "2023", Growth = 9.1 }
    };
}
```

---

## Chart Responsiveness

Ensure charts adapt to different screen sizes and orientations.

### Responsive Percentage Sizing

Use percentage-based dimensions for automatic responsiveness:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<div class="chart-container">
    <SfChart Title="Responsive Dashboard" Width="100%" Height="100%">
        <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
        
            <ChartSeries DataSource="@DashboardData" XName="Metric" YName="Value" 
                         Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
    </SfChart>
</div>

<style>
    .chart-container {
        width: 100%;
        height: 400px;
        padding: 10px;
    }
    
    @@media (max-width: 768px) {
        .chart-container {
            height: 300px;
        }
    }
</style>

@code {
    public class DashboardMetric
    {
        public string Metric { get; set; }
        public double Value { get; set; }
    }
    
    public List<DashboardMetric> DashboardData = new List<DashboardMetric>
    {
        new DashboardMetric { Metric = "Users", Value = 15000 },
        new DashboardMetric { Metric = "Sessions", Value = 45000 },
        new DashboardMetric { Metric = "Pageviews", Value = 120000 }
    };
}
```

### Container-Based Responsive Charts

Create fully responsive charts that adapt to parent container:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<div style="width: 100%; max-width: 1200px; margin: 0 auto;">
    <SfChart Title="Flexible Chart Layout">
        <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
        
            <ChartSeries DataSource="@FlexData" XName="Category" YName="Amount" 
                         Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Area" />
    </SfChart>
</div>

@code {
    public class FlexInfo
    {
        public string Category { get; set; }
        public double Amount { get; set; }
    }
    
    public List<FlexInfo> FlexData = new List<FlexInfo>
    {
        new FlexInfo { Category = "A", Amount = 120 },
        new FlexInfo { Category = "B", Amount = 150 },
        new FlexInfo { Category = "C", Amount = 180 },
        new FlexInfo { Category = "D", Amount = 140 }
    };
}
```

---

## Styling Best Practices

Follow these guidelines for optimal chart appearance and performance.

### Design Guidelines

**Color Selection:**
- Use color palettes with sufficient contrast for accessibility
- Limit palette to 5-7 distinct colors for clarity
- Ensure colors are distinguishable for colorblind users
- Use consistent colors across related charts

**Typography:**
- Choose readable fonts (12px minimum for labels)
- Use font weights to establish hierarchy (bold titles, regular labels)
- Ensure sufficient contrast between text and background

**Spacing:**
- Provide adequate margins (30-50px minimum)
- Use chart area width of 80-90% for balanced appearance
- Leave space for legends and labels

### Performance Tips

**Optimize Rendering:**
```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Optimized Chart" EnableCanvas="true">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@OptimizedData" XName="Item" YName="Value" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
</SfChart>

@code {
    public class OptimizedDataPoint
    {
        public string Item { get; set; }
        public double Value { get; set; }
    }
    
    public List<OptimizedDataPoint> OptimizedData = new List<OptimizedDataPoint>
    {
        new OptimizedDataPoint { Item = "Item 1", Value = 45 },
        new OptimizedDataPoint { Item = "Item 2", Value = 62 },
        new OptimizedDataPoint { Item = "Item 3", Value = 38 }
    };
}
```

**Animation Best Practices:**
- Use animations sparingly for data updates
- Keep duration between 500-2000ms
- Disable animations for large datasets (>1000 points)

### Accessibility Considerations

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<SfChart Title="Accessible Chart Design" 
         Palettes="@AccessiblePalette">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
        <ChartAxisMajorGridLines Width="1" Color="#E0E0E0" />
    </ChartPrimaryXAxis>
    
    <ChartPrimaryYAxis>
        <ChartAxisMajorGridLines Width="1" Color="#E0E0E0" />
    </ChartPrimaryYAxis>
    
        <ChartSeries DataSource="@AccessibleData" XName="Label" YName="Value" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" />
</SfChart>

@code {
    public class AccessibleDataPoint
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }
    
    public List<AccessibleDataPoint> AccessibleData = new List<AccessibleDataPoint>
    {
        new AccessibleDataPoint { Label = "Category A", Value = 75 },
        new AccessibleDataPoint { Label = "Category B", Value = 60 },
        new AccessibleDataPoint { Label = "Category C", Value = 85 }
    };
    
    // Color-blind friendly palette
    public string[] AccessiblePalette = new string[] 
    { 
        "#0173B2", "#DE8F05", "#029E73", "#CC78BC", "#CA9161" 
    };
}
```

### Troubleshooting Common Issues

**Issue: Chart not rendering at correct size**
- Solution: Add script reference to avoid delayed rendering
```html
<script src="https://cdn.syncfusion.com/blazor/syncfusion-blazor-base.min.js"></script>
```

**Issue: Colors not applying correctly**
- Solution: Ensure Palettes array matches or exceeds number of series
- Alternatively, use Fill property on individual series

**Issue: Responsive behavior not working**
- Solution: Use percentage values (e.g., "100%") instead of fixed pixels
- Ensure parent container has defined dimensions

**Issue: Title overlapping chart content**
- Solution: Adjust ChartMargin Top property to provide space
- Consider using title Position property

---

## Complete Styling Example

Here's a comprehensive example combining multiple styling techniques:

```razor
@using Syncfusion.Blazor.Toolkit.Charts

<div style="width: 100%; max-width: 900px; margin: 20px auto; padding: 20px; background: #F9F9F9;">
    <SfChart Title="Complete Styling Example" 
             SubTitle="Combining Multiple Appearance Features"
             Width="100%" 
             Height="450px"
             Background="#FFFFFF"
             Palettes="@CompletePalette">
        
        <ChartBorder Color="#2196F3" Width="2" />
        <ChartMargin Left="50" Right="50" Top="70" Bottom="50" />
        
        <ChartTitleStyle Size="22px" Color="#1565C0" FontFamily="Segoe UI" 
                         FontWeight="600" />
        <ChartSubTitleStyle Size="14px" Color="#757575" FontFamily="Segoe UI" 
                           FontStyle="italic" />
        
        <ChartArea Background="#F5F5F5" Width="85%">
            <ChartAreaBorder Color="#BDBDBD" Width="1" />
        </ChartArea>
        
        <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
            <ChartAxisMajorGridLines Width="1" Color="#E0E0E0" />
        </ChartPrimaryXAxis>
        
        <ChartPrimaryYAxis>
            <ChartAxisMajorGridLines Width="1" Color="#E0E0E0" />
        </ChartPrimaryYAxis>
        
            <ChartSeries DataSource="@CompleteData" XName="Month" YName="Sales" 
                         Name="Sales" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" Opacity="0.9">
                <ChartSeriesBorder Color="#1976D2" Width="1" />
                <ChartSeriesAnimation Enable="true" Duration="1500" />
            </ChartSeries>
            <ChartSeries DataSource="@CompleteData" XName="Month" YName="Target" 
                         Name="Target" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line" Width="3">
                <ChartSeriesAnimation Enable="true" Duration="1500" Delay="200" />
            </ChartSeries>
        
        <ChartLegendSettings Visible="true" Position="LegendPosition.Bottom" />
    </SfChart>
</div>

@code {
    public class CompleteDataPoint
    {
        public string Month { get; set; }
        public double Sales { get; set; }
        public double Target { get; set; }
    }
    
    public List<CompleteDataPoint> CompleteData = new List<CompleteDataPoint>
    {
        new CompleteDataPoint { Month = "Jan", Sales = 35000, Target = 40000 },
        new CompleteDataPoint { Month = "Feb", Sales = 42000, Target = 40000 },
        new CompleteDataPoint { Month = "Mar", Sales = 38000, Target = 40000 },
        new CompleteDataPoint { Month = "Apr", Sales = 45000, Target = 45000 },
        new CompleteDataPoint { Month = "May", Sales = 48000, Target = 45000 },
        new CompleteDataPoint { Month = "Jun", Sales = 52000, Target = 50000 }
    };
    
    public string[] CompletePalette = new string[] 
    { 
        "#2196F3", "#FF9800", "#4CAF50", "#E91E63" 
    };
}
```

---

## Summary

This reference guide covers all essential aspects of Syncfusion Blazor Charts appearance and styling:

- **Dimensions**: Control chart size using pixels, percentages, or container sizing
- **Colors**: Customize using palettes, series colors, or point-level colors
- **Backgrounds**: Apply backgrounds to chart and chart area with borders
- **Margins**: Configure spacing around the chart
- **Titles**: Style titles and subtitles with fonts, colors, and positioning
- **Themes**: Use built-in themes or create custom color schemes
- **Series**: Customize individual series with fills, opacity, and borders
- **Responsiveness**: Create adaptive charts for all screen sizes
- **Best Practices**: Follow design guidelines for optimal appearance and performance

Use these techniques to create professional, accessible, and visually appealing chart visualizations in your Blazor applications.