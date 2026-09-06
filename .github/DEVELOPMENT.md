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

---

## Release readiness defects (D1–D9, manual NuGet signing)

The numbers below correspond to the readiness-defect list reviewed on
2026-09-06. Per current policy, NuGet package signing and publishing
are performed **manually** by the release maintainer; no signing or
publishing automation exists in this public repository and none must
be added.

### D1 / LP-10 — Pinned release commit (automatic on public commit)

`RepositoryCommit` is **not** declared as a literal in
`src/Syncfusion.Blazor.Toolkit.csproj`. It is populated automatically
during `dotnet pack` by:

- `Microsoft.SourceLink.GitHub` reading the local `.git/HEAD` and
  storing the SHA in `SourceRevisionId`; and
- `Directory.Build.props` defaulting `RepositoryCommit` to
  `$(SourceRevisionId)` (which the .NET SDK emits into the `.nuspec`
  `<repository>` element).

The maintainer must run `dotnet pack` from a clone whose `HEAD`
matches the on-`main` tag-candidate commit. Verification before sign:

```sh
git cat-file -e <SHA>^{commit} \
  || { echo "ERROR: <SHA> is not on main"; exit 1; }
```

- `Directory.Build.props` — `RepositoryCommit` default + branch resolution
- `src/Syncfusion.Blazor.Toolkit.csproj` — `<RepositoryUrl>`, `<EmbedUntrackedSources>true</EmbedUntrackedSources>`, `<PublishRepositoryUrl>true</PublishRepositoryUrl>`
- `.github/workflows/ci.yml` — `pack` job smoke-packs an unsigned `.nupkg` per TFM to confirm the wire-up

### D2 / PI-01 — Strong-name signing (manual)

Shipped assemblies are strong-name signed by the release maintainer
as part of the manual sign-and-publish procedure. The public
repository contains no strong-name key material, no `SignAssembly=true`
directive, and no `AssemblyOriginatorKeyFile` value — by policy.
Public CI does not import, reference, or attempt to use any signing
artefact. Consumers can therefore trust that the absence of automated
signing in CI is deliberate and that a signed `.nupkg` is the
result of the manual procedure in [§Manual NuGet sign and publish](#manual-nuget-sign-and-publish-pi-01-pi-02)
below.

- Accepted Risk AR-1 in THREAT-MODEL.md.

### D3 / PI-02 — Authenticode and `.nupkg` signing (manual)

The inner DLLs are not Authenticode signed (`NotSigned`). The outer
`.nupkg` is signed manually (primary + counter) by the release
maintainer as part of the same manual sign-and-publish procedure. Public
CI does not generate, sign, or modify a `.nupkg` for publication.

- Accepted Risk AR-2 in THREAT-MODEL.md.

### D4 / BEQ-10 — Required parameters

Every `Sf*` component parameter whose XML documentation describes the
parameter as required must also carry `[EditorRequired]`. The
`tests/Syncfusion.Blazor.Toolkit.BUnitTest/Base/EditorRequiredAttributeTests.cs`
bUnit test enforces this by reflection over every public `[Parameter]`
in `src/Components/` and asserts `EditorRequiredAttribute` presence
when the docs mark the parameter as required.

### D5 / BEQ-20 — `e-*` style contract

Global styles for the toolkit are namespaced under `e-*`. They are
shipped from `src/wwwroot/styles/` and consumed via the
`_content/Syncfusion.Blazor.Toolkit/styles` static asset path. The
contract, including stable selectors and renaming policy, is defined
in [`src/wwwroot/styles/STYLE-CONTRACT.md`](../src/wwwroot/styles/STYLE-CONTRACT.md).
A subset of components uses Blazor CSS isolation for self-contained
styling (`Border.razor.css` under `Spinner`, `SfNumericTextBox.razor.css`);
those files are colocated with the component.

### D6 / PI-06 — SBOM (manual)

For every release, the release maintainer generates an SPDX 2.3 SBOM
and a CycloneDX 1.5 SBOM **locally**, **from the signed `.nupkg`**,
and attaches both to the GitHub release as release assets. This step
is part of the manual process described under D8 / D9. Public CI does
not upload any SBOM.

- Accepted Risk AR-5 in THREAT-MODEL.md.

### D7 / CI-01 — PR CI gates pack

Every PR runs restore → build → bUnit → Playwright → eslint → xss →
vulnerability scan → **pack** for .NET 8/9/10. The `pack` job
produces an **unsigned** `.nupkg` artifact labelled
`unsigned-pkg-<tfm>` for human review only. Pack must succeed with
`RepositoryCommit=<PR head SHA>` before the summary job reports
success. The artifact is not the publication candidate and is not
intended to be pushed to nuget.org.

### D8 / CI-07 — Public repository security boundary

There is no `.github/workflows/nuget-publish.yml`, no signing tool
invocation, no SHA-256 hand-off, no `STRONG_NAME_KEY_BASE64` secret
reference, and no `no-secrets.yml` sentinel. Adding any of these is
prohibited by the manual-signing policy. The repository boundary is
the maintainer's local machine running the manual procedure below.

- Accepted Risk AR-4 in THREAT-MODEL.md.

### D9 / CI-08 — Manual sign-and-publish hand-off

There is no automated immutable-artifact job in CI. The
tamper-evident control point is the maintainer's local pre-publish
verification: the maintainer runs `sha256sum` against the final
`.nupkg` and records the digest in the private release ticket before
executing `dotnet nuget push`. No public workflow participates in or
records this hand-off.

- Accepted Risk AR-4 in THREAT-MODEL.md.

### Manual NuGet sign and publish (PI-01, PI-02)

This procedure is the canonical sign-and-publish process. It is
performed **locally** by the Syncfusion release maintainer; no part
of it runs on public CI.

1. Pre-flight:
   - Confirm the tag candidate SHA exists on `main`:
     `git cat-file -e <SHA>^{commit}`.
   - `RepositoryCommit` is populated automatically from the local
     `.git/HEAD` via `Microsoft.SourceLink.GitHub` +
     `Directory.Build.props` (AR-3) — no manual substitution is
     required.
2. Restore + Build + Pack (locally):
   ```dotnetcli
   dotnet restore src/Syncfusion.Blazor.Toolkit.csproj
   dotnet build   src/Syncfusion.Blazor.Toolkit.csproj -c Release --no-restore
   dotnet pack    src/Syncfusion.Blazor.Toolkit.csproj -c Release --no-build -o nupkg -p:ContinuousIntegrationBuild=true
   ```
3. Strong-name sign the inner assemblies (PI-01, manual):
   ```sh
   sn -R <assembly.dll> <strong-name.snk>   # private key, never committed
   ```
4. Re-pack with the signed DLLs:
   ```dotnetcli
   dotnet pack src/Syncfusion.Blazor.Toolkit.csproj -c Release --no-build -o nupkg-final
   ```
5. Sign the `.nupkg` (PI-02, manual):
   ```sh
   nuget sign nupkg-final/Syncfusion.Blazor.Toolkit.<version>.nupkg \
     -CertificateSubjectName "<maintainer's code-signing subject>" \
     -CertificateStore Location=CurrentUser;StoreName=My \
     -TimestampserverUrl http://timestamp.digicert.com \
     -HashAlgorithm SHA256
   ```
6. Cross-sign with a counter-signing certificate as configured by
   the maintainer:
   ```sh
   nuget sign nupkg-final/Syncfusion.Blazor.Toolkit.<version>.nupkg \
     -CertificateSubjectName "<counter-signing subject>" \
     -CertificateStore Location=CurrentUser;StoreName=My
   ```
7. Generate SBOMs locally from the **signed** `.nupkg`:
   ```sh
   cd nupkg-final
   cyclonedx-dotnet -i Syncfusion.Blazor.Toolkit.<version>.nupkg -o bom.xml
   spdx-tools generate -i Syncfusion.Blazor.Toolkit.<version>.nupkg -o spdx.json
   ```
8. Compute the audit-trail hashes:
   ```sh
   sha256sum *.nupkg *.spdx.json bom.xml > SHA256SUMS
   ```
9. Publish:
   ```sh
   dotnet nuget push nupkg-final/Syncfusion.Blazor.Toolkit.<version>.nupkg \
     -ApiKey $NUGET_API_KEY -Source https://api.nuget.org/v3/index.json
   ```
10. Attach `*.spdx.json`, `bom.xml`, and `SHA256SUMS` to the GitHub
    release page corresponding to the tag. The signed `.nupkg` itself
    is published **only** via `dotnet nuget push` in step 9.
11. No csproj revert is required — `RepositoryCommit` is automatic.

### Repository metadata + SBOM during `dotnet pack`

`Directory.Build.props` (repo root) sets defaults so a normal
`dotnet pack` run from a public commit automatically wires:

- `RepositoryUrl` — set per packable project in its `.csproj`.
- `RepositoryType` — `git`, set per packable project.
- `RepositoryCommit` — auto-populated from
  `Microsoft.SourceLink.GitHub`'s `SourceRevisionId` (the local
  `.git/HEAD` SHA). Override with `-p:SourceRevision=<SHA>` if the
  checkout context demands it.
- `RepositoryBranch` — resolved at build time from `.git/HEAD` by
  the `_ResolveRepositoryBranch` target. Override with
  `-p:RepositoryBranch=<name>` for a hotfix-branch release.
- **SBOM** — `Directory.Build.targets` runs `dotnet CycloneDX` after
  the kernel is compiled and packages `Syncfusion.Blazor.Toolkit.cdx.json`
  inside the `.nupkg` at `_sbom/cyclonedx/<tfm>/`, plus a sibling copy
  at `artifacts/sbom/<id>/<version>/<tfm>/`.

Sources:

- `Directory.Build.props` (defaults + `_ResolveRepositoryBranch`)
- `Directory.Build.targets` (`_GenerateCycloneDxSbom`,
  `_PackCycloneDxSbomIntoNupkg`)
- `src/Syncfusion.Blazor.Toolkit.csproj` (`<RepositoryUrl>`,
  `<PublishRepositoryUrl>true`, `<EmbedUntrackedSources>true`)

The maintainer must `dotnet tool install --global CycloneDX` once on
the release workstation before the first release.

### Why there is no public publish workflow

There is intentionally no `.github/workflows/nuget-publish.yml` in
this repository. NuGet Trusted Publishing (OIDC) is **not** used for
this package. The reasons are:

1. **Manual signing** — the `.nupkg` is signed (primary + counter)
   on the release maintainer's local machine with an HSM-backed code
   signing identity. OIDC would have CI sign and push directly, which
   contradicts the published manual-signing policy (THREAT-MODEL.md
   AR-1, AR-2, AR-4).
2. **Trusted identity in CI is broader** — an OIDC trust relationship
   between GitHub Actions and nuget.org means that any workflow job
   with the right permissions can push a package impersonating this
   repo. The release workstation, in contrast, is the only place
   the code-signing certificate exists.
3. **Audit trail at the human boundary** — the manual sign-and-publish
   procedure in §Manual NuGet sign and publish leaves the maintainer
   in control of the publish command. This is the audit boundary the
   threat model relies on.
4. **No secrets in CI** — there is no `STRONG_NAME_KEY_BASE64`, no
   `NUGET_API_KEY`, and no certificate in public CI. Adding them
   would directly conflict with AR-1, AR-2 and AR-4.

If the publish process is ever changed, it must be re-ratified through
a new Accepted Risk entry and reflected in THREAT-MODEL.md before
deployment.

### Render-mode security

Blazor offers three render modes: static SSR, Interactive Server, and
Interactive WebAssembly. Each has a different security profile:

- **Static SSR** produces no JS interop and no SignalR circuit; it is
  equivalent to a server-rendered page. Components run on the server
  using only server-allowed APIs (`IHttpContextAccessor`,
  `NavigationManager`, DI services marked `Scoped`). There is no
  browser-exposed attack surface beyond the HTML payload.
- **Interactive Server** operates over a SignalR circuit. Components
  retain access to all server-side APIs; the browser sees only
  diff-rendered DOM. State is server-resident and never sent to the
  browser other than through Blazor's diff protocol.
- **Interactive WebAssembly** runs code on the client. Components
  in this mode **MUST NOT** call server-only APIs
  (`HttpContextAccessor`, `IDbContextFactory` without preloading,
  `SignInManager`, etc.) directly; doing so throws at runtime.

The codebase contains components that work in Interactive Server and
WebAssembly (e.g. `SfButton`, `SfDialog`, `SfTooltip`) and components
that are documented as static or server-only. The render mode is
declared per sample, not per component. The render-mode contract for
each component is documented in its XML doc-comment `Remarks`
section. Security implications are summarised in
[RENDER-MODE-SECURITY.md](RENDER-MODE-SECURITY.md).

### Performance notes

The codebase uses a number of standard Blazor performance patterns:

- Virtualization is used in `SfChart` for large data sets.
- `@key` is supplied on collection items in `SfDropDownList`-style
  inputs and in the dialog list rendering to keep DIff operations
  stable across re-renders.
- `ShouldRender` overrides are used in components where re-rendering
  is expensive (`SfNumericTextBox`, `SfDatePicker`).
- JS interop is limited to one well-typed module surface
  (`Base/SfJsInterop`) to keep marshalling overhead low.

Performance regressions should be tracked with a `perf`-labelled bug.

### Trim and AOT compatibility

`src/Syncfusion.Blazor.Toolkit.csproj` declares
`<IsTrimmable>true</IsTrimmable>` and `<IsAotCompatible>true</IsAotCompatible>`.
This means:

- A **publish** of a sample with `-p:PublishTrimmed=true` is run
  prior to every minor release. Any remaining ILLink warnings are
  either fixed or annotated in the [Known analyzer / trim / AOT
  findings](#known-analyzer-trim-aot-findings) section.
- A **publish** with `-p:PublishAot=true` is run prior to every
  major release against `samples/Blazor.Toolkit.Samples.Client`
  (the WebAssembly sample). Any remaining ILCompiler warnings are
  either fixed or annotated.

### Test matrix

`.github/workflows/ci.yml` runs the bUnit component tests on the full
.NET matrix (8.0.x, 9.0.x, 10.0.x). Playwright validation runs on the
same triple. The unpacked WebAssembly sample smoke runs on .NET 10
only. Documented coverage and gaps are kept up to date in
[TEST-MATRIX.md](TEST-MATRIX.md).

### Known analyzer / trim / AOT findings

The compiler-analyzer configuration promotes only security-relevant
CA rules to errors (`src/Syncfusion.Blazor.Toolkit.csproj`,
`<WarningsAsErrors>`). The following pre-existing findings are
documented and not treated as defects:

| Finding | Source | Rationale |
|---|---|---|
| Finding | Source | Rationale |
|---|---|---|
| `CA1014` / `CA1017` (assembly attributes) | applies to all TFM builds | The toolkit is a client-only Blazor component library; COM exposure and CLS-compliance enforcement are not consumer surfaces. Suppressed assembly-wide in `src/Properties/GlobalSuppressions.cs`. |
| `CA1305` (string IFormat) | applies to error/log message formatting | Error/log message formatting in this codebase never substitutes user-controlled values. Suppressed assembly-wide in `src/Properties/GlobalSuppressions.cs`. |
| `CA1716` (identifier naming) | applies consistently across contributors | Identifiers in the public Data namespace mirror .NET design-time naming (`Dynamic`, `Value`, etc.) required by the style guide. Suppressed assembly-wide in `src/Properties/GlobalSuppressions.cs`. |

Suppressions are added via `src/Properties/GlobalSuppressions.cs` so
they are visible to maintainers during code review and re-evaluated at
each major release. The file's per-rule rationale is duplicated in
`Justification` comments so each audit can be completed without
referring back to this table.

### Trim and AOT residual warnings

Residual `ILLink` warnings from `-p:PublishTrimmed=true` against
`samples/Blazor.Toolkit.Samples` and residual `ILCompiler` warnings
from `-p:PublishAot=true` against
`samples/Blazor.Toolkit.Samples.Client` are captured under the
`trim-and-aot` label in the issues queue. Each issue lists the rule
id, the symbol, and the planned remediation.
