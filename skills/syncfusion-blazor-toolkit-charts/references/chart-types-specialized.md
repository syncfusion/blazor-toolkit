# Specialized Blazor Chart Types Reference

A comprehensive guide to implementing specialized Syncfusion Blazor chart types including stacked, scatter, and bubble charts. This document is self-contained with complete examples and best practices.

## Table of Contents

- [Stacked Charts](#stacked-charts)
    - [Stacked Area](#stacked-area)
    - [Stacked Column/Bar](#stacked-columnbar)
    - [Stacked Line](#stacked-line)
- [Scatter and Bubble Charts](#scatter-and-bubble-charts)
    - [Scatter Chart](#scatter-chart)
    - [Bubble Chart](#bubble-chart)
- [Vertical Chart Orientation](#vertical-chart-orientation)
- [Best Practices](#best-practices)
    - [Empty Point Handling](#empty-point-handling)
    - [Series Customization Event](#series-customization-event)
    - [Point Customization Event](#point-customization-event)
    - [Gradient Fill](#gradient-fill)
- [Common Properties](#common-properties)
    - [All Chart Types Support:](#all-chart-types-support)
    - [Data Binding:](#data-binding)
- [Quick Reference](#quick-reference)


## Stacked Charts

### Stacked Area

**Overview**: Shows contribution of multiple series to total over time.

**Multi-Series Implementation**:
```cshtml
<SfChart>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
        <ChartSeries DataSource="@RevenueData" 
                     XName="Year" 
                     YName="ProductA" 
                     Name="Product A"
                     Fill="red" 
                     Opacity="0.7"
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.StackingArea">
            <ChartSeriesBorder Width="2" Color="black" />
        </ChartSeries>
        
        <ChartSeries DataSource="@RevenueData" 
                     XName="Year" 
                     YName="ProductB" 
                     Name="Product B"
                     Fill="blue" 
                     Opacity="0.7"
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.StackingArea">
            <ChartSeriesBorder Width="2" Color="black" />
        </ChartSeries>
        
        <ChartSeries DataSource="@RevenueData" 
                     XName="Year" 
                     YName="ProductC" 
                     Name="Product C"
                     Fill="green" 
                     Opacity="0.7"
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.StackingArea">
            <ChartSeriesBorder Width="2" Color="black" />
        </ChartSeries>
    
    <ChartStackLabelSettings Visible="true" Format="{value}" Fill="#ADD8E6">
        <ChartStackLabelFont FontWeight="600" Color="blue" />
    </ChartStackLabelSettings>
</SfChart>

@code {
    public class StackedData
    {
        public double Year { get; set; }
        public double ProductA { get; set; }
        public double ProductB { get; set; }
        public double ProductC { get; set; }
    }

    public List<StackedData> RevenueData = new List<StackedData>
    {
        new StackedData { Year = 2020, ProductA = 0.61, ProductB = 0.03, ProductC = 0.48 },
        new StackedData { Year = 2021, ProductA = 0.81, ProductB = 0.05, ProductC = 0.53 },
        new StackedData { Year = 2022, ProductA = 0.91, ProductB = 0.06, ProductC = 0.57 }
    };
}
```

**Stack Labels**: Display cumulative totals with `ChartStackLabelSettings`

---

### Stacked Column/Bar

**Similar to Stacked Area but with columns/bars**:
```cshtml
<ChartSeries Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.StackingColumn" />
<ChartSeries Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.StackingBar" />
```

---

### Stacked Line

```cshtml
<ChartSeries Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.StackingLine" Width="2" />
```

---

## Scatter and Bubble Charts

### Scatter Chart

**Overview**: Plots individual data points to show correlation between two variables.

```cshtml
<SfChart>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    
fix th         <ChartSeries DataSource="@ScatterData" 
                     XName="Country" 
                     YName="GoldMedals" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Scatter"
                     Fill="green"
                     Opacity="0.5">
            <ChartMarker Height="10" Width="10" Shape="ChartShape.Triangle" />
        </ChartSeries>
        
        <ChartSeries DataSource="@ScatterData" 
                     XName="Country" 
                     YName="SilverMedals" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Scatter"
                     Fill="blue"
                     Opacity="0.5">
            <ChartMarker Height="10" Width="10" Shape="ChartShape.Rectangle" />
        </ChartSeries>
</SfChart>
```

---

### Bubble Chart

**Overview**: Three-dimensional scatter chart where bubble size represents third parameter.

**Data Requirements**: XName, YName, Size

```cshtml
<SfChart>
        <ChartSeries DataSource="@PopulationData" 
                     XName="LiteracyRate" 
                     YName="GrowthRate" 
                     Size="Population"
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Bubble"
                     Fill="blue"
                     Opacity="0.5">
        </ChartSeries>
</SfChart>

@code {
    public class BubbleData
    {
        public double LiteracyRate { get; set; }
        public double GrowthRate { get; set; }
        public double Population { get; set; }
        public string Country { get; set; }
    }

    public List<BubbleData> PopulationData = new List<BubbleData>
    {
        new BubbleData { LiteracyRate = 92.2, GrowthRate = 7.8, Population = 1.347, Country = "China" },
        new BubbleData { LiteracyRate = 74, GrowthRate = 6.5, Population = 1.241, Country = "India" }
    };
}
```

---

## Vertical Chart Orientation

**Apply to any chart type**:
```cshtml
<SfChart IsTransposed="true">
        <ChartSeries DataSource="@Data" 
                     XName="X" 
                     YName="Y" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Spline" />
</SfChart>
```

---

## Best Practices

### Empty Point Handling
```cshtml
<ChartEmptyPointSettings Mode="EmptyPointMode.Average" Fill="#FFDE59">
    <ChartEmptyPointBorder Color="red" Width="2" />
</ChartEmptyPointSettings>
```

### Series Customization Event
```cshtml
<ChartEvents OnSeriesRender="SeriesRender" />

@code {
    void SeriesRender(SeriesRenderEventArgs args)
    {
        args.Fill = "#FF4081";
    }
}
```

### Point Customization Event
```cshtml
<ChartEvents OnPointRender="PointRender" />

@code {
    void PointRender(PointRenderEventArgs args)
    {
        args.Fill = (args.Point.Index % 2 != 0) ? "#ff6347" : "#009cb8";
    }
}
```

### Gradient Fill
```cshtml
<ChartSeries Fill="url(#grad1)" />

<svg style="height: 0">
    <defs>
        <linearGradient id="grad1" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="20%" style="stop-color:orange;stop-opacity:1" />
            <stop offset="100%" style="stop-color:black;stop-opacity:1" />
        </linearGradient>
    </defs>
</svg>
```

---

## Common Properties

### All Chart Types Support:
- **Fill**: Series color
- **Opacity**: Transparency (0-1)
- **DashArray**: Border pattern
- **ChartSeriesBorder**: Border width and color
- **ChartMarker**: Data point markers
- **ChartDataLabel**: Label customization
- **ChartEmptyPointSettings**: Handling null values

### Data Binding:
- Use `DataSource` property
- Map fields with `XName`, `YName`, `High`, `Low`, etc.
- Supports `SfDataManager` for remote data

---

## Quick Reference

| Chart Type | Data Fields | Use Case |
|------------|-------------|----------|
| Stacked Area | X, Y (multiple) | Contribution analysis |
| Scatter | X, Y | Correlation |
| Bubble | X, Y, Size | 3D relationships |

---

**Document Version**: 1.0  
**Last Updated**: March 2026  
**Total Lines**: ~390
