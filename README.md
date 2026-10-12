![Syncfusion Toolkit for Blazor banner](https://cdn.syncfusion.com/content/images/blazor-toolkit/blazor-toolkit-banner.png)
# Syncfusion® Toolkit for Blazor

The [**Syncfusion® Toolkit for Blazor**](https://www.syncfusion.com/blazor-toolkit) is an open-source collection of UI components for Blazor applications, with a Chart-first focus and supporting buttons, calendars, inputs and popups. Explore the [component demos](https://blazor.syncfusion.com/demos/toolkit/) and contribute improvements to components, documentation, tests, accessibility and performance.

[![NuGet](https://img.shields.io/nuget/v/Syncfusion.Blazor.Toolkit.svg?style=flat-square)](https://www.nuget.org/packages/Syncfusion.Blazor.Toolkit)
[![GitHub license](https://img.shields.io/badge/license-syncfusion/blazor-toolkit?style=flat-square&color=green)](https://github.com/syncfusion/blazor-toolkit/blob/main/LICENSE.txt)
[![GitHub stars](https://badgen.net/github/stars/syncfusion/blazor-toolkit?color=blue)](https://github.com/syncfusion/blazor-toolkit/stargazers)
[![Total Downloads](https://img.shields.io/nuget/dt/Syncfusion.Blazor.Toolkit?style=flat-square&color=green)](https://www.nuget.org/packages/Syncfusion.Blazor.Toolkit)
![Platforms](https://img.shields.io/badge/platform-Blazor-blue)
[![Source targets .NET 8, 9 and 10](https://img.shields.io/badge/.NET%208%20%7C%20.NET%209%20%7C%20.NET%2010-5C2D91?logo=.net&logoColor=white)](https://dotnet.microsoft.com/en-us/download/dotnet)
[![Contributors](https://img.shields.io/github/contributors/syncfusion/blazor-toolkit?style=flat-square&color=blue&label=Contributors)](https://github.com/syncfusion/blazor-toolkit/graphs/contributors)
[![GitHub issues](https://img.shields.io/github/issues/syncfusion/blazor-toolkit?style=flat-square)](https://github.com/syncfusion/blazor-toolkit/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/syncfusion/blazor-toolkit?style=flat-square)](https://github.com/syncfusion/blazor-toolkit/pulls)

**Chart-first · MIT licensed · No eligibility check · Source targets .NET 8 / 9 / 10**

[Getting started](#getting-started) · [Components](#components) · [Contributor setup](#quick-contributor-start) · [Roadmap](ROADMAP.md) · [Changelog](CHANGELOG.md) · [Documentation index](.github/Index.md)

![Syncfusion Toolkit for Blazor component gallery](https://cdn.syncfusion.com/content/images/blazor-toolkit/blazor-toolkit-demo.png)

## License and product scope

The toolkit is released under the [MIT License](LICENSE.txt): no eligibility check, revenue cap or registration is required. Individuals, startups and enterprises may use it in commercial and closed-source products, subject to the license's notice requirements and warranty disclaimer. The commercial suite's Community License eligibility rules do **not** apply to this package.

The [commercial Syncfusion Blazor suite](https://www.syncfusion.com/blazor-components) is a separate licensed offering with additional controls and support. Familiar concepts do not guarantee drop-in compatibility: adoption can require API, markup, styling and licensing changes. This repository has no Data Grid component; the [roadmap](ROADMAP.md) makes no Grid delivery commitment.

## Release and quality status

- **Public release history:** the GitHub API response retained in the [2026-10-06 baseline](docs/evidence/2026-10-06-r00-1922761/index.md#release-identity) lists [v1.0.2](https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.2) (2026-09-01, newest in that response) and [v1.0.1](https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.1) (2026-06-26). The [source project](src/Syncfusion.Blazor.Toolkit.csproj) now declares **2.0.0** under an authorized, uncommitted major-version change—not a published release. See the [API migration policy](docs/API-COMPATIBILITY.md) and [changelog](CHANGELOG.md#release-evidence-and-version-discrepancy); final package/tag reconciliation remains required.
- **Measured source baseline, 2026-10-06, commit `1922761`:** [retained logs and results](docs/evidence/2026-10-06-r00-1922761/index.md) record a .NET 8/9/10 library build with **0 warnings/errors only after disabling compiler-server reuse**; default builds failed. The .NET 8 bUnit suite passed **2,735 cases twice**, with no failures/skips but **3,537 build-warning occurrences**. Samples built with **232 warnings** under the same workaround. This is not a clean default-build, remote CI or release-package assurance.
- **Browser scope:** **2,421 cases discovered; four basic Chart rendering tests passed** on .NET 10 Interactive Server/Chromium; the other 2,417 were not run. [TEST-MATRIX.md](.github/TEST-MATRIX.md) separates configured, measured and untested scope. Trim/AOT declarations are not deployment proof; accessibility documents remain [draft evidence registers](#accessibility), not certification.
- **Security / supply chain:** R00 recorded **11 high-severity npm affected-package entries** requiring triage, and a mismatching checked-in SBOM checksum. Library-only NuGet scanning reported no advisories; this is not a whole-repository clearance. Conditional SBOM generation and empty GitHub release asset lists do not establish final package contents or signatures. See [SBOM limitations](artifacts/sbom/README.md) and [release guidance](.github/DEVELOPMENT.md#repository-metadata-and-sbom).

**Enterprise readiness remains unestablished.** The [readiness plan](docs/ENTERPRISE-READINESS-PLAN.md) tracks the remaining gates; this documentation update does not close implementation or release checks.

## Quick Contributor Start

Repository development needs **.NET SDK 10**, the **.NET 8 runtime** for bUnit, and **Node.js 22.13+ within 22.x** with npm. Default non-CI library builds use the **global CycloneDX .NET tool**; install it with `dotnet tool install --global CycloneDX`, or append `-p:CycloneDxEnabled=false` to the local build/test commands below to skip SBOM generation. That local opt-out is not release evidence. See [DEVELOPMENT.md](.github/DEVELOPMENT.md) for tool and workload details.

Run from the repository root:

```sh
git clone https://github.com/syncfusion/blazor-toolkit.git
cd blazor-toolkit
npm ci
npx gulp blazor-toolkit-themes
dotnet restore src/Syncfusion.Blazor.Toolkit.slnx
dotnet build src/Syncfusion.Blazor.Toolkit.slnx -c Release --no-restore
dotnet test tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj -c Release -f net8.0
```

The source solution contains only the library. For the gallery, run its server host; install the SDK 10 `wasm-tools` workload if using WebAssembly AOT/native-toolchain scenarios. R00's ordinary Debug gallery build did not require a listed `wasm-tools` workload and is not AOT evidence.

```sh
dotnet run --project samples/Blazor.Toolkit.Samples/Blazor.Toolkit.Samples.csproj
```

Browser checks, security tooling, the R00 compiler-state workaround and release preparation are documented in the [development guide](.github/DEVELOPMENT.md). These quick-start commands were not rerun as part of R01's documentation review; use the dated baseline for the exact configuration actually executed.

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

## Repository Structure

```text
blazor-toolkit/
├── src/
│   ├── Syncfusion.Blazor.Toolkit.slnx
│   ├── Syncfusion.Blazor.Toolkit.csproj
│   ├── Base/                         # Shared component infrastructure
│   ├── Data/                         # Data binding, queries and adaptors (not a Grid)
│   ├── Components/                   # Buttons, Calendars, Charts, Inputs, Popups, Spinner
│   └── wwwroot/                      # Bundled scripts, styles and theme sources
├── samples/
│   ├── Blazor.Toolkit.Samples.slnx
│   ├── Blazor.Toolkit.Samples/        # .NET 10 server host
│   └── Blazor.Toolkit.Samples.Client/ # .NET 10 WebAssembly client
├── tests/
│   ├── Syncfusion.Blazor.Toolkit.BUnitTest/
│   └── Syncfusion.Blazor.Playwright.Test/
├── docs/                             # Analyses and draft standards register
├── artifacts/sbom/                   # Dependency inventory artifacts and guidance
└── .github/                          # Documentation, skills, policies and workflows
```

## Development Workflow

Branch from `main`, keep changes scoped, add regression coverage, and use the explicit build/test paths above. Open a PR against `main` with reproduction details and the checks actually run. See [CONTRIBUTING.md](.github/CONTRIBUTING.md) for review expectations; a configured workflow is not proof of a passing run.

## Framework and Render Modes

The current library targets `net8.0;net9.0;net10.0`. These are source build targets, not a runtime certification matrix or a promise of ongoing Microsoft support; consult the [.NET support policy](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core).

Interactive Server, WebAssembly and Auto require the corresponding host configuration. Component requirements differ: interactive chart features need interactivity, while Chart's static-SSR detection path is compiled on .NET 9+. Do not assume .NET 8 static-SSR parity or universal behavior from one sample. See [render-mode guidance](.github/RENDER-MODE-SECURITY.md) and the [test matrix](.github/TEST-MATRIX.md).

## Components

Current source capabilities; use the linked guidance for exact APIs and mode requirements.

| Category | Control | Description |
|---|---|---|
| Data visualization | Chart | 21 core/variant series types: line, area, column/bar, scatter, bubble, spline, step, multicolored and stacking variants; axes, legends, tooltips, selection, zoom/pan, annotations, data labels and trendlines. |
| Buttons | Button | Icons, visual styles, toggle and disabled states. |
| Buttons | ButtonGroup | Grouped buttons with single/multiple selection. |
| Selection | CheckBox | Checked, unchecked and indeterminate states with labels and form binding. |
| Selection | RadioButton | Single choice within a named group. |
| Selection | Switch | Boolean toggle with configurable on/off labels. |
| Calendars | Calendar | Date selection, multiple views, multi-date selection and Min/Max restrictions. |
| Calendars | DatePicker | Date input and calendar popup with formatting and culture support. |
| Calendars | DateTimePicker | Combined date/time input with calendar and time selection. |
| Calendars | TimePicker | Time input/list with formats, intervals and Min/Max restrictions. |
| Inputs | Uploader | Browse/drag-and-drop, multiple files, progress and client-side validation; applications must validate uploads on the server. |
| Inputs | NumericTextBox | Numeric formatting, spin buttons, decimals and Min/Max values. |
| Inputs | TextArea | Multiline input with rows/columns, user-controlled resize direction, maximum length and floating labels. |
| Inputs | TextBox | Single-line input types, floating labels and clear button; use TextArea for multiline input. |
| Popups | Dialog | Modal/non-modal dialogs with dragging, resizing and templates. |
| Popups | Tooltip | Contextual help with hover, focus, click or custom triggers. |
| Notification | Spinner | Loading indicator with size/type, visibility and overlay styling. |

Toolkit Chart does not include pie/doughnut, radar, financial series, technical indicators or built-in export/print APIs. TextArea does not expose built-in auto-resize or a character-counter API. Commercial component documentation is not the Toolkit API contract.

## Skills and Component Guidance

The repository includes specialized skill files that document component usage patterns and implementation guidance for the major toolkit areas:

- [Buttons skill](https://github.com/syncfusion/blazor-toolkit/tree/main/.github/skills/syncfusion-blazor-toolkit-buttons/SKILL.md)
- [Calendars skill](https://github.com/syncfusion/blazor-toolkit/tree/main/.github/skills/syncfusion-blazor-toolkit-calendars/SKILL.md)
- [Charts skill](https://github.com/syncfusion/blazor-toolkit/tree/main/.github/skills/syncfusion-blazor-toolkit-charts/SKILL.md)
- [Inputs skill](https://github.com/syncfusion/blazor-toolkit/tree/main/.github/skills/syncfusion-blazor-toolkit-inputs/SKILL.md)
- [Notifications skill](https://github.com/syncfusion/blazor-toolkit/tree/main/.github/skills/syncfusion-blazor-toolkit-notifications/SKILL.md)
- [Popups skill](https://github.com/syncfusion/blazor-toolkit/tree/main/.github/skills/syncfusion-blazor-toolkit-popups/SKILL.md)

These skill files are intended to help contributors and maintainers understand the supported patterns, accessibility expectations, and implementation conventions for each toolkit area.

## Getting Started

[Manual integration](#manual-integration) uses the standard .NET Blazor template and the single [Toolkit NuGet package](https://www.nuget.org/packages/Syncfusion.Blazor.Toolkit). No commercial Syncfusion package, license key, separate Themes package or global Syncfusion script is required. See also the [Toolkit overview](https://blazor.syncfusion.com/documentation/toolkit/overview) and [component guidance](#skills-and-component-guidance).

### Manual Integration

This example targets **.NET 10 Interactive Server**. It includes a complete `Program.cs` and chart page within a standard generated host; the CSS block is explicitly a fragment. The [historical 2026-10-03 note](docs/ENTERPRISE-READINESS-PLAN.md#11-documentation-sync-progress--2026-10-03) reports compilation of the C#/Razor blocks against published `Syncfusion.Blazor.Toolkit` 1.0.2 with 0 warnings/errors, but R00 did not locate the original consumer or raw build log. Treat that as a recorded narrative, not independently retained package/runtime proof. R01 did not compile/run this example; R20 must retain executable onboarding evidence. For an existing app, merge the service/endpoint configuration into its host rather than replacing authentication or other middleware. .NET 8/9 hosts should retain their own template's static-asset and error-handling setup.

#### 1. Create the host and install the package

Use [SDK 10](https://dotnet.microsoft.com/download) and create the app outside this repository:

```sh
dotnet new blazor -o MyApp --framework net10.0 --interactivity Server
cd MyApp
dotnet add package Syncfusion.Blazor.Toolkit
```

This installs the package version resolved by NuGet; record that version when validating. A package installation is not a build of this checkout's Unreleased source. Node, gulp and CycloneDX are source-development tools, not prerequisites for this package-based example.

#### 2. Replace `MyApp/Program.cs`

Complete host entry point for the generated `MyApp` project (keep its `Components`, routing, layouts, error pages and static assets):

```csharp
using MyApp.Components;
using Syncfusion.Blazor.Toolkit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSyncfusionBlazorToolkit();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
```

If you choose a different project name, adjust the `MyApp.Components` namespace.

#### 3. Add bundled CSS in `MyApp/Components/App.razor`

Insert this **fragment inside the existing `<head>`**; retain the template's `HeadOutlet`, `Routes`, import map, other assets and Blazor framework script:

```html
<link href="_content/Syncfusion.Blazor.Toolkit/styles/fluent.min.css" rel="stylesheet" />
```

Toolkit components automatically import their bundled ES modules. Do not add a commercial `sf-blazor.min.js` script. The template's `_framework/blazor.web.js` is still required for interactivity.

#### 4. Add `MyApp/Components/Pages/ChartExample.razor`

This complete page includes all chart/render-mode imports and its data model. The imports can instead be shared through `Components/_Imports.razor`, keeping its existing template imports.

```razor
@page "/chart-example"
@using Microsoft.AspNetCore.Components.Web
@using static Microsoft.AspNetCore.Components.Web.RenderMode
@using Syncfusion.Blazor.Toolkit
@using Syncfusion.Blazor.Toolkit.Charts
@rendermode InteractiveServer

<PageTitle>Monthly sales</PageTitle>
<h1>Monthly sales</h1>
<p>Sales in units for January through March. Exact values follow the chart.</p>

<SfChart Height="330px" Title="Monthly sales">
    <ChartPrimaryXAxis Title="Month"
                       ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    <ChartPrimaryYAxis Title="Units" />
    <ChartSeries DataSource="@SalesData" XName="Month" YName="Units"
                 Name="Sales" Type="ChartSeriesType.Column" />
    <ChartTooltipSettings Enable="true" />
</SfChart>

<table>
    <caption>Monthly sales data</caption>
    <thead>
        <tr><th scope="col">Month</th><th scope="col">Units</th></tr>
    </thead>
    <tbody>
        @foreach (var point in SalesData)
        {
            <tr><th scope="row">@point.Month</th><td>@point.Units</td></tr>
        }
    </tbody>
</table>

@code {
    private readonly SalesPoint[] SalesData =
    {
        new() { Month = "Jan", Units = 35 },
        new() { Month = "Feb", Units = 28 },
        new() { Month = "Mar", Units = 34 }
    };

    public sealed class SalesPoint
    {
        public string Month { get; set; } = string.Empty;
        public double Units { get; set; }
    }
}
```

#### 5. Run from `MyApp`

```sh
dotnet watch
```

Open `/chart-example` at the host URL printed by the command. The adjacent table supplies a text alternative; application-level accessibility still needs evaluation.

### Optional External Templates

The separately distributed [Syncfusion.Blazor.Toolkit.Templates package](https://www.nuget.org/packages/Syncfusion.Blazor.Toolkit.Templates#readme-body-tab) is optional. Consult its own README for current template names, options and compatibility. This repository does not contain that scaffolder, and its behavior is not verified here. If it creates a `MyApp` directory, enter `cd MyApp` before running `dotnet watch` (or use the generated host project's directory for a multi-project template).

## Support

For full support details (community channels, bug reports, security, code of conduct), see
[`.github/SUPPORT.md`](https://github.com/syncfusion/blazor-toolkit/tree/main/.github/SUPPORT.md).

- **[GitHub Issues](https://github.com/syncfusion/blazor-toolkit/issues)** — Non-security bugs and feature requests; include a minimal reproduction.
- **[GitHub Discussions](https://github.com/syncfusion/blazor-toolkit/discussions)** — Usage questions and design proposals; use available issue channels if Discussions is unavailable.

Community support is **best-effort**, without response, fix or release deadlines; see [SUPPORT.md](.github/SUPPORT.md). Report vulnerabilities privately to **security@syncfusion.com** under [SECURITY.md](.github/SECURITY.md), not in public issues.

### Commercial Support

- **[Syncfusion Blazor Components](https://www.syncfusion.com/blazor-components)** — Licensed commercial components with additional enterprise features and dedicated support.
- **Toolkit-specific inquiries** — For paid support, licensing, and toolkit-specific questions: [submit a ticket](https://blazortoolkit.syncfusion.com/support/tickets/).

## Contributing

Contributions are welcome! If you'd like to contribute, check out our [contributing guide](https://github.com/syncfusion/blazor-toolkit/tree/main/.github/CONTRIBUTING.md) for details on how to get started. Whether you find a bug, have a feature request, or want to submit code, we appreciate your help in improving the toolkit.

See the [Development Guide](https://github.com/syncfusion/blazor-toolkit/tree/main/.github/DEVELOPMENT.md) for more details about this repository and project structure.

Review our [Code of Conduct](https://github.com/syncfusion/blazor-toolkit/tree/main/.github/CODE_OF_CONDUCT.md) — all community interactions are expected to follow it.

## Contributors

This project exists thanks to all the people who contribute.

<img src="https://contrib.rocks/image?repo=syncfusion/blazor-toolkit" alt="Syncfusion Blazor Toolkit contributors" loading="lazy" />

## Accessibility

**WCAG 2.2 Level AA is a goal, not verified conformance.** The [draft accessibility evidence register](.github/ACCESSIBILITY.md), [draft standards/component register](docs/VPAT-2.5-INT.md) and [screen-reader checklist](.github/accessibility/screen-reader-smoke.md) document mechanisms and evaluation gaps; they are not an approved VPAT/ACR or certification. Runtime screen-reader checks remain Not Evaluated in the register.

Evaluate labels, keyboard/focus behavior, chart alternatives, validation, themes and supported render modes in your application. Report reproducible problems through [accessibility issues](https://github.com/syncfusion/blazor-toolkit/issues?q=is%3Aopen+is%3Aissue+label%3Aaccessibility). The [documentation index](.github/Index.md) links related policies and evidence; the [test matrix](.github/TEST-MATRIX.md) records test scope and limits.

## About Syncfusion®

Founded in 2001 and headquartered in Research Triangle Park, N.C., Syncfusion® has more than 35,000 customers and more than 1 million users, including large financial institutions, Fortune 500 companies, and global IT consultancies.
 
Today, we provide 1800+ components and frameworks for web ([Blazor](https://www.syncfusion.com/blazor-components?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [ASP.NET Core](https://www.syncfusion.com/aspnet-core-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [ASP.NET MVC](https://www.syncfusion.com/aspnet-mvc-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Angular](https://www.syncfusion.com/angular-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [React](https://www.syncfusion.com/react-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Vue](https://www.syncfusion.com/vue-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), and [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget)), mobile ([.NET MAUI](https://www.syncfusion.com/maui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Xamarin](https://www.syncfusion.com/xamarin-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), and [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget)), and desktop development ([WinForms](https://www.syncfusion.com/winforms-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [WPF](https://www.syncfusion.com/wpf-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [WinUI](https://www.syncfusion.com/winui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget) and [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget)).
___

[sales@syncfusion.com](mailto:sales@syncfusion.com?subject=Syncfusion%20Blazor%20Toolkit%20-%20NuGet) | [www.syncfusion.com](https://www.syncfusion.com?utm_source=nuget&utm_medium=listing&utm_campaign=blazor-toolkit-nuget) | Toll Free: 1-888-9 DOTNET