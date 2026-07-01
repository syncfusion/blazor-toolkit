## Table of Contents

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Installation Steps](#installation-steps)
   - [Option 1: Visual Studio](#option-1-visual-studio)
   - [Option 2: Visual Studio Code](#option-2-visual-studio-code)
   - [Option 3: .NET CLI](#option-3-net-cli)
- [Configuration](#configuration)
   - [Step 1: Add Namespaces](#step-1-add-namespaces)
   - [Step 2: Register Syncfusion Services](#step-2-register-syncfusion-services)
   - [Step 3: Add Script Reference](#step-3-add-script-reference)
- [Creating Your First Chart](#creating-your-first-chart)
   - [Basic Empty Chart](#basic-empty-chart)
   - [Chart with Data](#chart-with-data)
   - [Adding Titles](#adding-titles)
   - [Adding Data Labels](#adding-data-labels)
   - [Enabling Tooltips](#enabling-tooltips)
   - [Adding Legend](#adding-legend)
- [Complete Example](#complete-example)
- [Running the Application](#running-the-application)
   - [Visual Studio](#visual-studio)
   - [Visual Studio Code / .NET CLI](#visual-studio-code-net-cli)
- [Multiple Series Example](#multiple-series-example)
- [Common Setup Issues](#common-setup-issues)
   - [Issue: Chart Not Rendering](#issue-chart-not-rendering)
   - [Issue: No Data Displayed](#issue-no-data-displayed)
   - [Issue: Interactive Mode Error](#issue-interactive-mode-error)
- [Next Steps](#next-steps)
- [Additional Resources](#additional-resources)

# Getting Started with Blazor Chart Component

This guide covers everything you need to set up and create your first Blazor Chart component, including installation, configuration, and basic chart implementation.

## Overview

The Syncfusion Blazor Chart component can be integrated into Blazor Server or WebAssembly applications. This guide walks through the setup process using Visual Studio, Visual Studio Code, or .NET CLI.

## Prerequisites

- **.NET SDK:** Version 6.0 or later
- **Development Environment:** Visual Studio 2022, Visual Studio Code, or .NET CLI
- **Blazor App:** Server, WebAssembly, or Hybrid app template
- **System Requirements:** Check [Syncfusion Blazor system requirements](https://blazor.syncfusion.com/documentation/system-requirements)

## Installation Steps

### Option 1: Visual Studio

#### Step 1: Create Blazor App
1. Open Visual Studio 2022
2. Create new project → **Blazor Web App** template
3. Configure project name and location
4. Select **Interactive render mode** (Server, WebAssembly, or Auto)
5. Choose **Interactivity location** (Global or Per page/component)

#### Step 2: Install NuGet Package
1. Right-click project → **Manage NuGet Packages**
2. Search for `Syncfusion.Blazor.Toolkit`
3. Install the package

**Or use Package Manager Console:**
```powershell
Install-Package Syncfusion.Blazor.Toolkit
```

### Option 2: Visual Studio Code

#### Step 1: Create Blazor App
```bash
dotnet new blazor -o BlazorChartApp -int Server
cd BlazorChartApp
```

#### Step 2: Install NuGet Package
```bash
dotnet add package Syncfusion.Blazor.Toolkit.Charts
dotnet restore
```

### Option 3: .NET CLI

#### Step 1: Verify .NET SDK
```bash
dotnet --version
```

#### Step 2: Create Blazor App
```bash
dotnet new blazor -o BlazorChartApp -int Server
cd BlazorChartApp
```

#### Step 3: Install Package
```bash
dotnet add package Syncfusion.Blazor.Toolkit.Charts
dotnet restore
```

## Configuration

### Step 1: Add Namespaces

Open `_Imports.razor` and add:

```razor
@using Syncfusion.Blazor
@using Syncfusion.Blazor.Toolkit.Charts
```

### Step 2: Register Syncfusion Services

In `Program.cs`, add the Syncfusion Blazor service:

```csharp
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register Syncfusion Blazor Service
builder.Services.AddSyncfusionBlazor();

var app = builder.Build();
```

### Step 3: Add Script Reference

In `App.razor` (or `_Layout.cshtml` for older templates), add the script reference before the closing `</body>` tag:

```html
<body>
    <!-- Other content -->
    
    <script src="_content/Syncfusion.Blazor.Core/scripts/syncfusion-blazor.min.js" type="text/javascript"></script>
</body>
```

**Note:** The Chart component script is included in `syncfusion-blazor.min.js`. For individual component scripts:
```html
<script src="_content/Syncfusion.Blazor.Toolkit.Charts/scripts/sf-chart.min.js" type="text/javascript"></script>
```

## Creating Your First Chart

### Basic Empty Chart

Create a new Razor page (e.g., `ChartDemo.razor`) and add:

```razor
@page "/chart-demo"
@using Syncfusion.Blazor.Toolkit.Charts

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

Here's a complete working example combining all elements:

```razor
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

### Visual Studio
- Press `Ctrl + F5` (Windows) or `⌘ + F5` (macOS)

### Visual Studio Code / .NET CLI
```bash
dotnet run
```

Navigate to the chart page URL (e.g., `https://localhost:5001/chart-demo`)

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

**Cause:** Missing service registration or script reference

**Solution:** 
1. Verify `AddSyncfusionBlazor()` in `Program.cs`
2. Check script reference in `App.razor`
3. Ensure namespaces in `_Imports.razor`

### Issue: No Data Displayed

**Cause:** Incorrect property binding

**Solution:**
- Verify `XName` and `YName` match data model properties exactly (case-sensitive)
- Ensure `DataSource` is populated before rendering

### Issue: Interactive Mode Error

**Cause:** Component requires interactivity

**Solution:** Add render mode at page top:
```razor
@rendermode InteractiveServer
```

Or configure globally in `App.razor`.

## Next Steps

After setting up your first chart:
- Explore different chart types (Line, Area, Column/Bar, Scatter, Bubble, Spline)
- Customize axes (numeric, datetime, logarithmic)
- Add interactive features (zooming, selection, crosshair)
- Implement data binding with DataManager
- Style with themes and custom colors
- Add annotations and markers

