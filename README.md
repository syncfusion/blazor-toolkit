# Syncfusion® Toolkit for Blazor

The [**Syncfusion® Toolkit for Blazor**](https://www.syncfusion.com/blazor-toolkit) is a high-performance, open-source collection of lightweight UI components designed to accelerate Blazor application development (Server and WebAssembly). These controls help developers build modern, responsive, and feature-rich web applications faster, with clean code and excellent performance.

Built with community collaboration in mind, the toolkit incorporates user feedback and contributions to deliver practical, high-quality components that integrate seamlessly with the broader Syncfusion Blazor ecosystem. We welcome contributions across components, documentation, tests, accessibility, and performance.

[![NuGet](https://img.shields.io/nuget/v/Syncfusion.Blazor.Toolkit.svg?style=flat-square)](https://www.nuget.org/packages/Syncfusion.Blazor.Toolkit)
[![GitHub license](https://img.shields.io/badge/license-syncfusion/blazor-toolkit?style=flat-square&color=green)](https://github.com/syncfusion/blazor-toolkit/blob/main/LICENSE.txt)
[![GitHub stars](https://badgen.net/github/stars/syncfusion/blazor-toolkit?color=blue)](https://github.com/syncfusion/blazor-toolkit/stargazers)
[![Total Downloads](https://img.shields.io/nuget/dt/Syncfusion.Blazor.Toolkit?style=flat-square&color=green)](https://www.nuget.org/packages/Syncfusion.Blazor.Toolkit)
![Platforms](https://img.shields.io/badge/platform-Blazor-blue)
[![.NET 8](https://img.shields.io/badge/.NET%208%20%7C%20.NET%209%20%7C%20.NET%2010-5C2D91?logo=.net&logoColor=white)](https://dotnet.microsoft.com/en-us/download/dotnet)
[![Contributors](https://img.shields.io/github/contributors/syncfusion/blazor-toolkit?style=flat-square&color=blue&label=Contributors)](https://github.com/syncfusion/blazor-toolkit/graphs/contributors)
[![GitHub issues](https://img.shields.io/github/issues/syncfusion/blazor-toolkit?style=flat-square)](https://github.com/syncfusion/blazor-toolkit/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/syncfusion/blazor-toolkit?style=flat-square)](https://github.com/syncfusion/blazor-toolkit/pulls)

<!-- Build, test, and issues badges require GitHub Actions workflows to be configured.
     When CI workflows exist, uncomment these lines:
     [![Build](https://img.shields.io/github/actions/workflow/status/syncfusion/blazor-toolkit/build.yml?style=flat-square)](https://github.com/syncfusion/blazor-toolkit/actions)
     [![Tests](https://img.shields.io/github/actions/workflow/status/syncfusion/blazor-toolkit/test.yml?style=flat-square)](https://github.com/syncfusion/blazor-toolkit/actions)
     [![GitHub issues](https://img.shields.io/github/issues/syncfusion/blazor-toolkit?style=flat-square)](https://github.com/syncfusion/blazor-toolkit/issues)
-->

## Quick Contributor Start

```bash
git clone https://github.com/syncfusion/blazor-toolkit.git
cd blazor-toolkit
```
Run these commands to build the solution:
```bash
dotnet restore
dotnet build ./Syncfusion.Blazor.Toolkit.slnx
```
To run the samples,
- Open the `samples/Blazor.Toolkit.Samples.slnx` file in Visual Studio.
- Set the desired sample project as the startup project and run.

## Ways to Contribute

We welcome contributions of all sizes:

- **Bug fixes** — Help us ship cleaner, more correct components
- **Documentation** — Improve XML docs, examples, and usage guides
- **Test coverage** — Add bUnit tests for components with low coverage
- **Accessibility** — Improve ARIA attributes, keyboard navigation, and screen reader support
- **Performance** — Profile rendering, reduce allocations, tighten JS interop
- **API review** — Give feedback on component APIs and patterns
- **Feature proposals** — Open a discussion before opening a PR for large changes

Not sure where to start? Look for issues labeled [`good first issue`](https://github.com/syncfusion/blazor-toolkit/issues?q=is%3Aopen+is%3Aissue+label%3A%22good+first+issue%22) or [`help wanted`](https://github.com/syncfusion/blazor-toolkit/issues?q=is%3Aopen+is%3Aissue+label%3Ahelp+wanted).

## Good First Issues

Looking for a place to start? Here are some examples of contributions we regularly accept:

- Fix incomplete or incorrect XML documentation on public API members
- Add bUnit tests for components with low test coverage
- Improve accessibility labels and ARIA attributes on existing components
- Tighten JavaScript interop scoping (reduce unnecessary `IJSRuntime` surface exposure)
- Refactor large enums or constants into domain-specific files
- Add missing `<example>` blocks in XML documentation comments

For a full list, see [open issues labeled good first issue](https://github.com/syncfusion/blazor-toolkit/issues?q=is%3Aopen+is%3Aissue+label%3A%22good+first+issue%22).

## Repository Structure

```
blazor-toolkit/
├── src/
│   ├── Base/                  # Shared infrastructure (ComponentBase, event callbacks, utilities)
│   ├── Components/            # UI components by domain (Buttons/, Calendars/, Charts/, Inputs/, Popups/, Spinner/)
│   │   └── <Component>/       # Each component has its own folder (SfButton/, SfChart/, etc.)
│   └── wwwroot/               # Static assets: JavaScript, CSS, and generated theme files
│       └── styles/            # Theme CSS generated by gulp — do not edit by hand
├── samples/                   # Sample browser application
├── tests/                     # Test suites
│   ├── BUnitTest/             # Component unit tests (bUnit + xUnit)
│   └── Playwright.Test/       # Browser automation and visual regression tests                    
└── .github/                   # Architecture, coding standards, and development documentation
```

## Development Workflow

1. **Branch** — Create a branch from `main` blazor toolkit repository
2. **Change** — Make your change. All PRs target the `main` branch
3. **Test** — Run `dotnet test` and confirm no linter warnings before opening a PR
4. **PR** — Open a pull request against `main`. Fill out the PR template completely
5. **Review** — Two maintainers review. We aim to respond within 5 business days
6. **Merge** — PRs are merged after approval and passing CI checks

For full details on commit style, PR requirements, and review criteria, see the [Contributing Guide](./.github/CONTRIBUTING.md).

## Getting Started

- [Install .NET](https://dotnet.microsoft.com/download) (8.0 or later recommended)
- [Syncfusion Blazor Toolkit Documentation](https://blazor.syncfusion.com/documentation/toolkit/overview) (in active development — contributor-focused docs in [DEVELOPMENT.md](./.github/DEVELOPMENT.md))
- [Development Guide](./.github/DEVELOPMENT.md)

## Components

| Category | Control | Description |
|----------|---------|-------------|
| Data Viz | Chart | Versatile charting component for rendering interactive data visualizations like line, bar, pie, and more with zooming, tooltips, and legends. |
| Buttons | Button | Customizable button with support for icons, predefined styles (primary, success, etc.), sizes, and states like disabled or loading. |
| Buttons | ButtonGroup | Groups multiple buttons together for unified styling and actions, ideal for toolbars or segmented controls. |
| Buttons | Checkbox | Toggleable input for single or grouped selections with customizable labels, states, and indeterminate mode. |
| Buttons | Radio Button | Grouped selection control allowing single choice from options, with customizable appearance and accessibility support. |
| Buttons | Toggle Switch | On/off toggle control (like a switch) for boolean inputs, with customizable labels and states. |
| Calendars | Calendar | Interactive monthly calendar view for date selection with navigation, multi-select, and range highlighting. |
| Calendars | DatePicker | Popup calendar input for selecting single dates with formatting, validation, and culture support. |
| Calendars | DateTime Picker | Combined date and time selector with calendar popup and time spinner for precise datetime input. |
| Calendars | TimePicker | Time selection input with spinner or list view, supporting 12/24-hour formats and intervals. |
| Inputs | File Upload | Drag-and-drop or browse file input with progress tracking, multiple file support, and validation. |
| Inputs | Numeric TextBox | Input for numeric values with formatting, spin buttons, decimals, min/max ranges, and culture support. |
| Inputs | TextArea | Multi-line text input with auto-resize, character counter, and floating label support. |
| Inputs | Textbox | Single-line text input with floating labels, icons, validation states, and clear button. |
| Layout | Dialog | Modal/popup window for alerts, forms, or confirmations with drag, resize, and animation support. |
| Layout | Tooltip | Lightweight popup that displays contextual information when users hover over, focus on, or interact with a target element, with customizable positioning and animation support. |
| Notification | Spinner | Loading indicator with customizable size, type, and overlay for async operations. |

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

To use the [Syncfusion® Blazor Toolkit](https://www.syncfusion.com/blazor-toolkit), register the Syncfusion Blazor Toolkit services in your `Program.cs` file as follows:

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

The following Razor code demonstrates how to set up a basic `SfChart` using the [Syncfusion® Blazor Toolkit](https://www.syncfusion.com/blazor-toolkit). This code snippet should be included in a `.razor` page of your Blazor project. It sets up the necessary namespaces, binds data to the component, and configures the `SfChart` with two spline area series for comparing Online vs Retail sales data.

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

### Community Support (Open Source)

- **[GitHub Issues](https://github.com/syncfusion/blazor-toolkit/issues)** — Bug reports and bug-fix PRs
- **[GitHub Discussions](https://github.com/syncfusion/blazor-toolkit/discussions)** — Ideas, questions, and feature proposals before opening a PR

Response time for community channels is best-effort; we aim to acknowledge within 5 business days.

### Commercial Support

- **[Syncfusion Blazor Components](https://www.syncfusion.com/blazor-components)** — Licensed commercial components with additional enterprise features and dedicated support.
- **Toolkit-specific inquiries** — For paid support, licensing, and toolkit-specific questions: [submit a ticket](https://blazortoolkit.syncfusion.com/support/tickets/).

## Contributing

Contributions are welcome! If you'd like to contribute, check out our [contributing guide](./.github/CONTRIBUTING.md) for details on how to get started. Whether you find a bug, have a feature request, or want to submit code, we appreciate your help in improving the toolkit.

See the [Development Guide](./.github/DEVELOPMENT.md) for more details about this repository and project structure.

Review our [Code of Conduct](./.github/CODE_OF_CONDUCT.md) — all community interactions are expected to follow it.

## Contributors

This project exists thanks to all the people who contribute.

<img src="https://contrib.rocks/image?repo=syncfusion/blazor-toolkit" alt="Contributors">

## About Syncfusion®

Founded in 2001 and headquartered in Research Triangle Park, N.C., Syncfusion® has more than 35,000 customers and more than 1 million users, including large financial institutions, Fortune 500 companies, and global IT consultancies.
 
Today, we provide 1800+ components and frameworks for web ([Blazor](https://www.syncfusion.com/blazor-components?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [ASP.NET Core](https://www.syncfusion.com/aspnet-core-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [ASP.NET MVC](https://www.syncfusion.com/aspnet-mvc-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Angular](https://www.syncfusion.com/angular-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [React](https://www.syncfusion.com/react-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Vue](https://www.syncfusion.com/vue-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), and [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget)), mobile ([.NET MAUI](https://www.syncfusion.com/maui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Xamarin](https://www.syncfusion.com/xamarin-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), and [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget)), and desktop development ([WinForms](https://www.syncfusion.com/winforms-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [WPF](https://www.syncfusion.com/wpf-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [WinUI](https://www.syncfusion.com/winui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget) and [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget)).
___

[sales@syncfusion.com](mailto:sales@syncfusion.com?Subject=Syncfusion%20Maui%toolkit%20-%20NuGet) | [www.syncfusion.com](https://www.syncfusion.com?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget) | Toll Free: 1-888-9 DOTNET