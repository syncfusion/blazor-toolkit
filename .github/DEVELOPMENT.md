# Development Guide

This page contains the steps to build and run the Syncfusion Toolkit for Blazor repository from source. If you are looking to build apps with the Syncfusion Toolkit for Blazor, please head over to the links in the [README](https://github.com/syncfusion/blazor-toolkit/blob/main/README.md) to get started.

## Initial setup
   ### Windows
   - Install Visual Studio 2022 (v17.10 or newer) with ASP.NET and web development workload.
   - Install [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
   - Clone the repository:
     ```shell
     git clone https://github.com/syncfusion/blazor-toolkit.git
     cd blazor-toolkit
     ```

## Building the Solution

1. Open a command prompt/terminal window.
2. Navigate to the location of your cloned `syncfusion/blazor-toolkit` repo.
3. Run these commands:
    ```dotnetcli
    dotnet restore
    dotnet build ./Syncfusion.Blazor.Toolkit.slnx
    ```

### Release sanity check (local)

If you want to mimic what `.github/workflows/nuget-publish.yml` does on a release runner, pass `-p:ContinuousIntegrationBuild=true` so SourceLink and the package hash match what CI produces:

```dotnetcli
dotnet restore src/Syncfusion.Blazor.Toolkit.csproj -p:ContinuousIntegrationBuild=true
dotnet build   src/Syncfusion.Blazor.Toolkit.csproj -c Release --no-restore -p:ContinuousIntegrationBuild=true
dotnet pack    src/Syncfusion.Blazor.Toolkit.csproj -c Release --no-build -o nupkg -p:ContinuousIntegrationBuild=true
```

> **Note**: `dotnet pack` triggers a `BeforeBuild` target that runs `npm install` and `gulp blazor-toolkit-themes` if `src/wwwroot/styles/fluent.min.css` is absent. Make sure Node.js (LTS) is on `PATH`. The release workflow installs Node 22 explicitly to handle this.

## Running Samples

- Open the `samples/Blazor.Toolkit.Samples.slnx` file in Visual Studio.
- Set the desired sample project as the startup project and run.

## What branch should I use?

As a general rule:
- [main](https://github.com/syncfusion/blazor-toolkit/tree/main)

## Sample projects

### Samples
```
├── samples
│   ├── Blazor.Toolkit.Samples
│   ├── Blazor.Toolkit.Samples.Client
```

- *Blazor.Toolkit.Samples*: Full gallery sample with all of the controls and features of the Syncfusion Blazor Toolkit.
- *Blazor.Toolkit.Samples.Client*: Client-side Blazor WebAssembly sample project.

## Security Review Cycle

The threat model and security posture are reviewed on a structured schedule:

| Trigger | Activity |
|---|---|
| Every **major release** (`x.0.0`) | Full review of [THREAT-MODEL.md](THREAT-MODEL.md): reassess all threats, verify mitigations, update or re-ratify the accepted-risks table. |
| Every **minor release** (`x.y.0`) | Review the accepted-risks table; escalate any entry whose circumstances have changed. |
| **Monthly servicing** | Dependency review and CVE triage per [SECURITY.md §3.3](SECURITY.md). |

The outcome of each structured review is recorded by updating the "Security Self-Attestation" date in [SECURITY.md](SECURITY.md) and the accepted-risks table in [THREAT-MODEL.md](THREAT-MODEL.md).

## Versioning and API stability

The project follows [Semantic Versioning 2.0.0](https://semver.org/spec/v2.0.0.html).
Release history lives on the GitHub release page:

> **Release history:** <https://github.com/syncfusion/blazor-toolkit/releases>

Every published tag has a release page on that URL that enumerates
the added, changed, deprecated, and removed APIs for that version.
Consumers should subscribe to the GitHub Releases feed (or watch
the repo's releases-only notifications) to be notified of new
versions.

The short version of the policy is:

- **Major (`x.0.0`)** — breaking public-API changes; the public
  surface is re-baselined in `PublicAPI.Shipped.txt`.
- **Minor (`x.y.0`)** — backwards-compatible additions only.
- **Patch (`x.y.z`)** — backwards-compatible bug fixes only.

Until the project ships a `1.0.0` release, the minor digit may
include breaking changes per SemVer §4. The currently shipped
version is `v1.0.1`.

APIs marked `[Obsolete]` are retained for at least **two minor
releases** before removal. APIs marked `[Experimental]` are not
covered by the SemVer compatibility promise and may change in any
release.
