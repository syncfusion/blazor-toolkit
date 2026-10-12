# Third-Party Notices and Dependency Evidence

The Syncfusion Blazor Toolkit source is licensed under the
[MIT License](../LICENSE.txt). This document is a dependency reference, **not a
complete legal inventory, license clearance, SBOM certification, or proof of the
contents of any released package**. Preserve applicable upstream license and
notice texts; a package's license expression alone may not satisfy redistribution
obligations for all included material.

Evidence reviewed on **2026-10-03** is separated below:

- **Declared:** library `PackageReference` entries and root npm `devDependencies`.
- **Locally resolved:** `src/obj/project.assets.json`, installed NuGet metadata,
  and local npm package metadata. These can differ by SDK/restore and checkout.
- **Package proof:** requires inspection of the exact released `.nupkg`, its
  `.nuspec`, assemblies, static assets and notices. No such verification is
  asserted here for v1.0.1 or v1.0.2.

## 1. Declared library NuGet dependencies

Source: [Syncfusion.Blazor.Toolkit.csproj](../src/Syncfusion.Blazor.Toolkit.csproj).

| Direct reference | Declared version (net8 / net9 / net10) | Asset role | License metadata |
|---|---|---|---|
| Microsoft.AspNetCore.Components.Web | 8.0.23 / 9.0.12 / 10.0.2 | Consumer dependency | MIT |
| Microsoft.Extensions.Localization | 8.0.23 / 9.0.12 / 10.0.2 | Consumer dependency | MIT |
| Microsoft.SourceLink.GitHub | 10.0.303 for all targets | Build tooling, `PrivateAssets="All"` | MIT |

These are the three explicit references, not the whole restore graph. The SDK
also supplies tooling such as ILLink. Consumer resolution is determined by the
actual package dependency groups and the consuming application's restore.

## 2. Local NuGet resolution snapshot

The following direct/transitive framework-package versions were observed in the
local library assets file. Microsoft metadata identifies these packages as
[MIT](https://licenses.nuget.org/MIT). Inclusion in this table does not mean every
package or analyzer becomes a consumer runtime dependency. A dash means absent
from that target's local graph, not unsupported on that runtime.

| Package | Locally resolved versions (net8 / net9 / net10) | SPDX | Upstream |
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

### Local build-tool dependency graph

| Package | Locally resolved version (net8 / net9 / net10 where different) | SPDX metadata | Role |
|---|---|---|---|
| Microsoft.NET.ILLink.Tasks | 8.0.31 / 9.0.20 / 10.0.12 | MIT | SDK-provided trim/AOT tooling |
| Microsoft.SourceLink.GitHub | 10.0.303 | MIT | Direct private build reference for SourceLink |
| Microsoft.SourceLink.Common | 10.0.303 | MIT | SourceLink dependency |
| Microsoft.Build.Tasks.Git | 10.0.303 | MIT | Git metadata build tasks |
| System.IO.Hashing | 10.0.11 | MIT | Dependency in the local build-tool graph |

`PrivateAssets="All"` on SourceLink prevents its dependency assets from flowing
as ordinary consumer dependencies. It does not, by itself, prove the absence of
all tool material from an artifact. Confirm generated dependency groups and files
in the exact package. ILLink versions depend on the installed SDK/restore, not a
fixed explicit library reference.

## 3. Declared npm development tools

Source: [package.json](../package.json). Versions below are **declarations**, not
necessarily installed versions. License identifiers were read from local installed
package metadata during this review; they do not cover each package's entire
transitive tree. Use the matching tracked lockfile for reproducible installation
and inspect the upstream notices for the exact versions being redistributed.

| Direct devDependency | Declared version | Local license metadata | Purpose |
|---|---|---|---|
| @eslint/js | ^10.0.1 | MIT | ESLint recommended configuration |
| @playwright/test | ^1.63.0 | Apache-2.0 | Browser test runner |
| eslint | ^10.11.0 | MIT | JS/TS lint runner |
| eslint-plugin-security | ^4.0.1 | Apache-2.0 | Security heuristic lint rules |
| glob | ^13.0.6 | BlueOak-1.0.0 | Build/scan file matching |
| globals | ^17.12.0 | MIT | Linter environment globals |
| gulp | ^5.0.1 | MIT | Build task runner |
| gulp-clean-css | ^4.3.0 | MIT | CSS minification |
| gulp-rename | ^2.1.0 | MIT | File naming in theme generation |
| gulp-sass | 6.0.1 | MIT | SCSS pipeline integration |
| sass | 1.105.0 | MIT | Dart Sass compiler |
| shelljs | ^0.10.0 | BSD-3-Clause | Build filesystem utilities |
| typescript-eslint | ^8.70.1 | MIT | TypeScript lint integration |

These tools are declared as development dependencies, not NuGet runtime
references. Generated CSS and other static assets can still be distributed in
the library. A `devDependencies` label is not proof that generated output or
bundled material has no third-party notice obligations.

## 4. Scope not established by these tables

- **Shipped static assets:** review scripts, styles, icons, fonts and embedded
  material under `src/wwwroot` and the final package. npm declarations alone do
  not inventory their origins or licenses.
- **Samples and tests:** their separate project references are outside the library
  tables. The gallery client still declares `Markdig` with `Version="*"`. R02
  replaced the nested ButtonGroup project's wildcard commercial `Syncfusion.Blazor`
  reference with Toolkit source; dotCover remains a test-tool dependency. See
  [R02 graph evidence](../docs/evidence/2026-10-06-r02-1922761/index.md). Do not extend
  the Toolkit MIT license to independent tools or infer they ship in its package.
- **Tools and transitive dependencies:** browser binaries, workloads, global
  CycloneDX, the full npm transitive tree, and other build/test tools require
  their own version/license records where applicable.
- **SBOM and package evidence:** the conditional CycloneDX target inventories a
  source project, not the final signed package. The reviewed v1.0.1/v1.0.2 release
  metadata has empty asset lists; embedded package SBOMs and signing were not
  verified by this document. See [DEVELOPMENT.md](DEVELOPMENT.md#repository-metadata-and-sbom).

Before a release, the owner should reconcile the actual package inventory with
its dependency/license records, retain required copyright/license/NOTICE texts,
and record unresolved origins or obligations for legal review. This file is not
explicitly included by the current library project packaging items; do not assume
it or all required notices reach consumers without inspecting the final package.
