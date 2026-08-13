# Third-Party Notices

The Syncfusion Blazor Toolkit is licensed under the MIT License (see `LICENSE.txt`).
This file identifies the third-party components included in or used to produce the package and states the license under which each is distributed.

---

## 1. Runtime NuGet dependencies

These packages are resolved as transitive dependencies and are available to consumers of the NuGet package.
All are published by Microsoft under the MIT License: https://licenses.nuget.org/MIT

| Package | Resolved versions (net8 / net9 / net10) | SPDX | Upstream |
|---|---|---|---|
| Microsoft.AspNetCore.Authorization | 8.0.23 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/aspnetcore |
| Microsoft.AspNetCore.Components | 8.0.23 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/aspnetcore |
| Microsoft.AspNetCore.Components.Analyzers | 8.0.23 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/aspnetcore |
| Microsoft.AspNetCore.Components.Forms | 8.0.23 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/aspnetcore |
| Microsoft.AspNetCore.Components.Web | 8.0.23 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/aspnetcore |
| Microsoft.AspNetCore.Metadata | 8.0.23 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/aspnetcore |
| Microsoft.Extensions.Configuration | — / — / 10.0.2 | MIT | https://github.com/dotnet/runtime |
| Microsoft.Extensions.Configuration.Abstractions | — / — / 10.0.2 | MIT | https://github.com/dotnet/runtime |
| Microsoft.Extensions.Configuration.Binder | — / — / 10.0.2 | MIT | https://github.com/dotnet/runtime |
| Microsoft.Extensions.DependencyInjection | 8.0.1 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/runtime |
| Microsoft.Extensions.DependencyInjection.Abstractions | 8.0.2 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/runtime |
| Microsoft.Extensions.Diagnostics | — / — / 10.0.2 | MIT | https://github.com/dotnet/runtime |
| Microsoft.Extensions.Diagnostics.Abstractions | — / — / 10.0.2 | MIT | https://github.com/dotnet/runtime |
| Microsoft.Extensions.Localization | 8.0.23 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/aspnetcore |
| Microsoft.Extensions.Localization.Abstractions | 8.0.23 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/aspnetcore |
| Microsoft.Extensions.Logging.Abstractions | 8.0.3 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/runtime |
| Microsoft.Extensions.Options | 8.0.2 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/runtime |
| Microsoft.Extensions.Options.ConfigurationExtensions | — / — / 10.0.2 | MIT | https://github.com/dotnet/runtime |
| Microsoft.Extensions.Primitives | 8.0.0 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/runtime |
| Microsoft.Extensions.Validation | — / — / 10.0.2 | MIT | https://github.com/dotnet/runtime |
| Microsoft.JSInterop | 8.0.23 / 9.0.12 / 10.0.2 | MIT | https://github.com/dotnet/aspnetcore |
| System.IO.Pipelines | 8.0.0 / — / — | MIT | https://github.com/dotnet/runtime |

---

## 2. Build-time NuGet tools (PrivateAssets — not transitive to consumers)

These packages are consumed during the build only. They are not listed as dependencies in the produced `.nupkg` and are not restored by consumers.

| Package | Version | SPDX | Purpose |
|---|---|---|---|
| Microsoft.NET.ILLink.Tasks | 8.0.29 / 9.0.18 / 10.0.10 | MIT | IL trimming analyser |
| Microsoft.SourceLink.GitHub | 8.0.0 | MIT | Embeds Git commit SHA in PDB |
| Microsoft.SourceLink.Common | 8.0.0 | MIT | SourceLink shared infrastructure |
| Microsoft.Build.Tasks.Git | 8.0.0 | MIT | Git repository metadata for SourceLink |

---

## 3. Build-time npm tools (devDependencies — not shipped)

These packages are used at build time to compile SCSS and bundle static assets. They are listed in `package.json` as `devDependencies` and are not included in the NuGet package output.

| Package | Version | SPDX | Purpose |
|---|---|---|---|
| gulp | ^4.0.2 | MIT | Build task runner |
| gulp-sass | 5.1.0 | MIT | SCSS compilation |
| sass | 1.51.0 | MIT | Dart Sass compiler |
| gulp-clean-css | ^4.3.0 | MIT | CSS minification |
| gulp-rename | ^2.1.0 | MIT | File renaming in gulp pipeline |
| shelljs | ^0.8.5 | BSD-3-Clause | Shell utilities |
| @playwright/test | ^1.58.2 | Apache-2.0 | End-to-end test runner |
