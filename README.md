# Syncfusion® Toolkit for Blazor

The **Syncfusion® Toolkit for Blazor** is a high-performance, open-source collection of lightweight UI components designed to accelerate Blazor application development (Server and WebAssembly). These controls help developers build modern, responsive, and feature-rich web applications faster, with clean code and excellent performance.

Built with community collaboration in mind, the toolkit incorporates user feedback and contributions to deliver practical, high-quality components that integrate seamlessly with the broader Syncfusion Blazor ecosystem.

[![NuGet](https://img.shields.io/nuget/v/Syncfusion.Blazor.Toolkit.svg?style=flat-square)](https://www.nuget.org/packages/Syncfusion.Blazor.Toolkit)
[![GitHub license](https://img.shields.io/github/license/syncfusion/blazor-toolkit?style=flat-square)](https://github.com/syncfusion/blazor-toolkit/blob/main/LICENSE)
[![GitHub stars](https://img.shields.io/github/stars/syncfusion/blazor-toolkit?style=flat-square)](https://github.com/syncfusion/blazor-toolkit/stargazers)

## Getting Started

- [Install .NET](https://dotnet.microsoft.com/download) (8.0 or later recommended)
- [Syncfusion Blazor Toolkit Documentation](https://blazor.syncfusion.com/documentation/toolkit/overview) *(update when live)*
- [Development Guide](/.github/DEVELOPMENT.md)

## Controls List

| **Category**   | **Control**                | **Description**                                                                 |
|----------------|----------------------------|---------------------------------------------------------------------------------|
| Data Viz      | Charts                    | Versatile charting component for rendering interactive data visualizations like line, bar, pie, and more with zooming, tooltips, and legends. |
| Buttons       | Button                    | Customizable button with support for icons, predefined styles (primary, success, etc.), sizes, and states like disabled or loading. |
| Buttons       | ButtonGroup               | Groups multiple buttons together for unified styling and actions, ideal for toolbars or segmented controls. |
| Buttons       | Checkbox                  | Toggleable input for single or grouped selections with customizable labels, states, and indeterminate mode. |
| Buttons       | Radio Button              | Grouped selection control allowing single choice from options, with customizable appearance and accessibility support. |
| Buttons       | Toggle Switch Button      | On/off toggle control (like a switch) for boolean inputs, with customizable labels and states. |
| Calendars     | Calendar                  | Interactive monthly calendar view for date selection with navigation, multi-select, and range highlighting. |
| Calendars     | DatePicker                | Popup calendar input for selecting single dates with formatting, validation, and culture support. |
| Calendars     | DateTime Picker           | Combined date and time selector with calendar popup and time spinner for precise datetime input. |
| Calendars     | TimePicker                | Time selection input with spinner or list view, supporting 12/24-hour formats and intervals. |
| Inputs        | File Upload               | Drag-and-drop or browse file input with progress tracking, multiple file support, and validation. |
| Inputs        | Numeric TextBox           | Input for numeric values with formatting, spin buttons, decimals, min/max ranges, and culture support. |
| Inputs        | TextArea                  | Multi-line text input with auto-resize, character counter, and floating label support. |
| Inputs        | Textbox                   | Single-line text input with floating labels, icons, validation states, and clear button. |
| Layout        | Dialog                    | Modal/popup window for alerts, forms, or confirmations with drag, resize, and animation support. |
| Layout        | Tooltip                   | Lightweight popup that displays contextual information when users hover over, focus on, or interact with a target element, with customizable positioning and animation support. |
| Notification  | Spinner                   | Loading indicator with customizable size, type, and overlay for async operations. |

## Installation

Install the Syncfusion Blazor Toolkit via NuGet:

```bash
dotnet add package Syncfusion.Blazor.Toolkit
```

Alternatively, add it directly in your `.csproj` file:

```xml
<PackageReference Include="Syncfusion.Blazor.Toolkit" Version="x.x.x" />
```

## Setup in Your Blazor App

To use the Syncfusion® Blazor Toolkit, register the Syncfusion Blazor services in your `Program.cs` file as follows:

**Program.cs**

```csharp
using Syncfusion.Blazor.Toolkit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // or AddInteractiveWebAssemblyComponents() for WASM

// Initialize the Syncfusion Blazor Toolkit by adding the below line of code
builder.Services.AddSyncfusionBlazorToolkit();

// Continue initializing your Blazor App here

var app = builder.Build();

// ... rest of your app configuration
```

### Add imports to _Imports.razor

Add the following imports to your `_Imports.razor` file:

```razor
@using Syncfusion.Blazor
@using Syncfusion.Blazor.Toolkit
```

### Add CSS to your App.razor

Add the Syncfusion Blazor Toolkit styles to the `<head>` section of your `App.razor` file:

```html
@* Add syncfusion blazor toolkit style reference *@    
<link id="syncfusion-theme" href="_content/Syncfusion.Blazor.Toolkit/styles/fluent.min.css" rel="stylesheet" />
```

## Usage Example

Here's a quick example to get you started with one of the controls, such as the Chart:

The following Razor code demonstrates how to set up a basic `SfChart` using the Syncfusion® Blazor Toolkit. This code snippet should be included in a `.razor` page of your Blazor project. It sets up the necessary namespaces, binds data to the component, and configures the `SfChart` with two spline area series for comparing Online vs Retail sales data.

**Pages/ChartExample.razor**

```razor
@page "/chart-example"
@using Syncfusion.Blazor.Toolkit.Charts

<h3>Online vs Retails</h3>

<SfChart Height="330px" Title="Online vs Retails">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    <ChartPrimaryYAxis Title="Sales in Units">
    </ChartPrimaryYAxis>   
        <ChartSeries DataSource="@ChartData"
                     XName="X"
                     YName="Y"
                     Type="ChartSeriesType.SplineArea"
                     Name="Online"
                     Fill="#00BFFF"
                     Opacity="0.5" />
        <ChartSeries DataSource="@ChartData"
                     XName="X"
                     YName="Y1"
                     Type="ChartSeriesType.SplineArea"
                     Name="Retails"
                     Fill="#FF00CC80"
                     Opacity="0.5" /> 
    <ChartLegendSettings Visible="true"></ChartLegendSettings>
    <ChartTooltipSettings Enable="true"
                          Format="${point.x} : <b>${point.y} Units</b>" />
</SfChart>

@code {
    private List<ChartDataPoint> ChartData = new();

    protected override void OnInitialized()
    {
        ChartData = new List<ChartDataPoint>()
        {
            new ChartDataPoint { X = "Jan", Y = 35, Y1 = 28 },
            new ChartDataPoint { X = "Feb", Y = 28, Y1 = 35 },
            new ChartDataPoint { X = "Mar", Y = 34, Y1 = 32 },
            new ChartDataPoint { X = "Apr", Y = 32, Y1 = 44 },
            new ChartDataPoint { X = "May", Y = 40, Y1 = 32 },
            new ChartDataPoint { X = "Jun", Y = 32, Y1 = 35 },
            new ChartDataPoint { X = "Jul", Y = 35, Y1 = 41 },
            new ChartDataPoint { X = "Aug", Y = 55, Y1 = 48 },
            new ChartDataPoint { X = "Sep", Y = 38, Y1 = 52 },
            new ChartDataPoint { X = "Oct", Y = 30, Y1 = 34 },
            new ChartDataPoint { X = "Nov", Y = 25, Y1 = 36 },
            new ChartDataPoint { X = "Dec", Y = 32, Y1 = 40 }
        };
    }
}
```

Define a simple data model C# class named `ChartDataPoint` to represent a data point in your application.

**ChartDataPoint.cs**

```csharp
/// <summary>
/// Represents a data point for the Chart with monthly sales data.
/// </summary>
public class ChartDataPoint
{
    /// <summary>
    /// Gets or sets the month (X-axis value).
    /// </summary>
    public string X { get; set; }

    /// <summary>
    /// Gets or sets the online sales in units (Y-axis value).
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Gets or sets the retail sales in units (Y1-axis value).
    /// </summary>
    public double Y1 { get; set; }
}
```

## Support

For any other queries, reach our [Syncfusion support team](https://blazortoolkit.syncfusion.com/support/tickets/) through ticket.

## Contributing

Contributions are welcome! If you'd like to contribute, please check out our [contributing guide](./.github/CONTRIBUTING.md) for details on how to get started. Whether you find a bug, have a feature request, or want to submit code, we appreciate your help in improving the toolkit.

See the [Development Guide](./.github/DEVELOPMENT.md) for more details about this repository and project structure.

## About Syncfusion®

Founded in 2001 and headquartered in Research Triangle Park, N.C., Syncfusion® has more than 35,000 customers and more than 1 million users, including large financial institutions, Fortune 500 companies, and global IT consultancies.
 
Today, we provide 1800+ components and frameworks for web ([Blazor](https://www.syncfusion.com/blazor-components?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [ASP.NET Core](https://www.syncfusion.com/aspnet-core-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [ASP.NET MVC](https://www.syncfusion.com/aspnet-mvc-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Angular](https://www.syncfusion.com/angular-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [React](https://www.syncfusion.com/react-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Vue](https://www.syncfusion.com/vue-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), and [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget)), mobile ([.NET MAUI](https://www.syncfusion.com/maui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Xamarin](https://www.syncfusion.com/xamarin-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), and [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget)), and desktop development ([WinForms](https://www.syncfusion.com/winforms-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [WPF](https://www.syncfusion.com/wpf-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [WinUI](https://www.syncfusion.com/winui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget) and [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget)).
___

[sales@syncfusion.com](mailto:sales@syncfusion.com?Subject=Syncfusion%20Maui%toolkit%20-%20NuGet) | [www.syncfusion.com](https://www.syncfusion.com?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget) | Toll Free: 1-888-9 DOTNET