## Table of Contents

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Installation Steps](#installation-steps)
   - [Code Studio / .NET CLI](#code-studio--net-cli)
- [Configuration](#configuration)
   - [Step 1: Add Namespaces](#step-1-add-namespaces)
   - [Step 2: Register Syncfusion Services](#step-2-register-syncfusion-services)
   - [Step 3: Add Bundled Styles](#step-3-add-bundled-styles)
- [Creating Your First Chart](#creating-your-first-chart)
   - [Basic Empty Chart](#basic-empty-chart)
   - [Chart with Data](#chart-with-data)
   - [Adding Titles](#adding-titles)
   - [Adding Data Labels](#adding-data-labels)
   - [Enabling Tooltips](#enabling-tooltips)
   - [Adding Legend](#adding-legend)
- [Complete Example](#complete-example)
- [Running the Application](#running-the-application)
- [Multiple Series Example](#multiple-series-example)
- [Common Setup Issues](#common-setup-issues)
   - [Issue: Chart Not Rendering](#issue-chart-not-rendering)
   - [Issue: No Data Displayed](#issue-no-data-displayed)
   - [Issue: Interactive Mode Error](#issue-interactive-mode-error)
- [Next Steps](#next-steps)

# Getting Started with Blazor Chart Component

This guide covers everything you need to set up and create your first Blazor Chart component, including installation, configuration, and basic chart implementation.

## Overview

Use Toolkit Chart in a Blazor app developed with Code Studio or the .NET CLI. The source in this repository is authoritative; commercial Chart setup instructions are not interchangeable.

## Prerequisites

- **Consumer framework:** .NET 8 or later; the library targets `net8.0;net9.0;net10.0`.
- **Repository development:** .NET SDK 10 for the multi-target library and samples.
- **Interactivity:** Server, WebAssembly, or Auto for interactive chart features. Chart also has a static SSR path, with renderer detection compiled on .NET 9+.

## Installation Steps

### Code Studio / .NET CLI

Create a Blazor Web App, then add the single Toolkit package:

```bash
dotnet new blazor -o BlazorChartApp -int Server
cd BlazorChartApp
dotnet add package Syncfusion.Blazor.Toolkit
```

`Syncfusion.Blazor.Toolkit.Charts` is a component namespace, not a NuGet package. The remaining snippets are fragments to integrate into the generated app, not standalone host programs. Enable interactivity on the page (as shown below) or an ancestor; configuring Server services alone does not make every page interactive.

## Configuration

### Step 1: Add Namespaces

Open `_Imports.razor` and add:

```razor
@using Syncfusion.Blazor.Toolkit
@using Syncfusion.Blazor.Toolkit.Charts
```

### Step 2: Register Syncfusion Services

`Program.cs` fragment (keep the template's endpoint, middleware, and `app.Run()` setup):

```csharp
using Syncfusion.Blazor.Toolkit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSyncfusionBlazorToolkit();

var app = builder.Build();
```

### Step 3: Add Bundled Styles

In the host document's `<head>` (`App.razor` for a Blazor Web App):

```html
<link href="_content/Syncfusion.Blazor.Toolkit/styles/fluent.min.css" rel="stylesheet" />
```

No separate commercial Themes package or global Syncfusion script is required. Components import their bundled ES modules automatically. Keep the template's Blazor framework script.

## Creating Your First Chart

### Basic Empty Chart

Create a new Razor page (e.g., `ChartDemo.razor`) and add:

```razor
@page "/chart-demo"
@using Syncfusion.Blazor.Toolkit.Charts
@using static Microsoft.AspNetCore.Components.Web.RenderMode
@rendermode InteractiveServer

<SfChart>
</SfChart>
```

This renders an empty chart container.

### Chart with Data

#### Step 1: Define Data Model

```razor
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
        new SalesInfo { Month = "Jun", SalesValue = 32 },
        new SalesInfo { Month = "Jul", SalesValue = 35 }
    };
}
```

#### Step 2: Configure Chart with Series

```razor
<SfChart>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
    </ChartPrimaryXAxis>
    
        <ChartSeries DataSource="@SalesData" 
                     XName="Month" 
                     YName="SalesValue" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
        </ChartSeries>
</SfChart>
```

**Key Properties:**
- `ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"` - Treats X-axis as discrete categories
- `DataSource` - Binds the data collection
- `XName` - Property name for X-axis values (`Month`)
- `YName` - Property name for Y-axis values (`SalesValue`)
- `Type` - Chart type (`Column`, `Line`, `Bar`, etc.)

### Adding Titles

Add titles to the chart and axes for context:

```razor
<SfChart Title="Sales Analysis">
    <ChartPrimaryXAxis Title="Month" 
                        ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
    </ChartPrimaryXAxis>
    
    <ChartPrimaryYAxis Title="Sales in Dollar">
    </ChartPrimaryYAxis>
    
        <ChartSeries DataSource="@SalesData" 
                     XName="Month" 
                     YName="SalesValue" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
        </ChartSeries>
</SfChart>
```

### Adding Data Labels

Display values on data points:

```razor
<SfChart Title="Sales Analysis">
    <ChartPrimaryXAxis Title="Month" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
    <ChartPrimaryYAxis Title="Sales in Dollar"></ChartPrimaryYAxis>
    
        <ChartSeries DataSource="@SalesData" 
                     XName="Month" 
                     YName="SalesValue" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
            <ChartMarker>
                <ChartDataLabel Visible="true"></ChartDataLabel>
            </ChartMarker>
        </ChartSeries>
</SfChart>
```

### Enabling Tooltips

Show data on hover:

```razor
<SfChart Title="Sales Analysis">
    <ChartPrimaryXAxis Title="Month" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
    <ChartPrimaryYAxis Title="Sales in Dollar"></ChartPrimaryYAxis>
    
    <ChartTooltipSettings Enable="true"></ChartTooltipSettings>
    
        <ChartSeries DataSource="@SalesData" 
                     XName="Month" 
                     YName="SalesValue" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
        </ChartSeries>
</SfChart>
```

### Adding Legend

Enable legend for multi-series charts:

```razor
<SfChart Title="Sales Analysis">
    <ChartPrimaryXAxis Title="Month" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
    <ChartPrimaryYAxis Title="Sales in Dollar"></ChartPrimaryYAxis>
    
    <ChartLegendSettings Visible="true"></ChartLegendSettings>
    
        <ChartSeries DataSource="@SalesData" 
                     Name="Sales"
                     XName="Month" 
                     YName="SalesValue" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
        </ChartSeries>
</SfChart>
```

**Note:** The `Name` property sets the legend text for each series.

## Complete Example

Page example combining the elements above; it depends on the configured host and imports. It has not been compile-tested in this documentation sync:

```razor
@using static Microsoft.AspNetCore.Components.Web.RenderMode
@rendermode InteractiveServer
@page "/chart-demo"
@using Syncfusion.Blazor.Toolkit.Charts

<h3>Monthly Sales Analysis</h3>

<SfChart Title="Sales Analysis" Width="90%">
    <ChartPrimaryXAxis Title="Month" 
                        ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category">
    </ChartPrimaryXAxis>
    
    <ChartPrimaryYAxis Title="Sales in Dollar" 
                        Minimum="0" 
                        Maximum="50" 
                        Interval="10">
    </ChartPrimaryYAxis>
    
    <ChartTooltipSettings Enable="true"></ChartTooltipSettings>
    <ChartLegendSettings Visible="true"></ChartLegendSettings>
    
        <ChartSeries DataSource="@SalesData" 
                     Name="Sales"
                     XName="Month" 
                     YName="SalesValue" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column"
                     Fill="#0066CC">
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
        new SalesInfo { Month = "Jun", SalesValue = 32 },
        new SalesInfo { Month = "Jul", SalesValue = 35 }
    };
}
```

## Running the Application

From the application's directory in Code Studio's terminal:

```bash
dotnet run
```

Navigate to `/chart-demo` using the listening URL printed by the application.

## Multiple Series Example

To compare multiple datasets:

```razor
<SfChart Title="Product Comparison">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"></ChartPrimaryXAxis>
    
    <ChartLegendSettings Visible="true"></ChartLegendSettings>
    <ChartTooltipSettings Enable="true"></ChartTooltipSettings>
    
        <ChartSeries DataSource="@Product1Data" 
                     Name="Product A"
                     XName="Month" 
                     YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
        </ChartSeries>
        
        <ChartSeries DataSource="@Product2Data" 
                     Name="Product B"
                     XName="Month" 
                     YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
        </ChartSeries>
</SfChart>

@code {
    public class ProductSales
    {
        public string Month { get; set; }
        public double Sales { get; set; }
    }

    public List<ProductSales> Product1Data = new List<ProductSales>
    {
        new ProductSales { Month = "Jan", Sales = 35 },
        new ProductSales { Month = "Feb", Sales = 28 },
        new ProductSales { Month = "Mar", Sales = 34 }
    };

    public List<ProductSales> Product2Data = new List<ProductSales>
    {
        new ProductSales { Month = "Jan", Sales = 20 },
        new ProductSales { Month = "Feb", Sales = 35 },
        new ProductSales { Month = "Mar", Sales = 30 }
    };
}
```

## Common Setup Issues

### Issue: Chart Not Rendering

**Check setup:**
1. Verify `AddSyncfusionBlazorToolkit()` in `Program.cs`
2. Check bundled CSS and successful automatic ES-module requests (do not add a commercial global script)
3. Ensure both Toolkit and Charts namespaces are imported
4. Confirm the host render mode matches the required interactions

### Issue: No Data Displayed

**Cause:** Incorrect property binding

**Solution:**
- Verify `XName` and `YName` match data model properties exactly (case-sensitive)
- Ensure `DataSource` is populated before rendering

### Issue: Interactive Mode Error

Tooltips, zooming, selection, and event/method-driven interactions require an interactive host. For example, on a page in a server-interactive app:

```razor
@rendermode InteractiveServer
```

Configure the matching services and endpoints in the host. Static Chart rendering is also implemented: `IsStaticServerRendering()` detects the static renderer only on .NET 9+. Static output does not provide interactive events or JS methods; do not claim Chart is never SSR-capable.

## Next Steps

After setting up your first chart:
- Explore different chart types (Line, Area, Column/Bar, Scatter, Bubble, Spline)
- Customize axes (numeric, datetime, logarithmic)
- Add interactive features (zooming, selection, crosshair)
- Implement data binding with DataManager
- Style with themes and custom colors
- Add annotations and markers

