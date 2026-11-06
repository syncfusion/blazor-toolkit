# Development Guide

This page contains the steps to build and run the Syncfusion Toolkit for Blazor repository from source. If you are looking to build apps with the Syncfusion Toolkit for Blazor, please head over to the links in the [README](https://github.com/syncfusion/blazor-toolkit/blob/main/README.md) to get started.

## Initial setup

### Prerequisites (all platforms)

- **.NET SDKs** for all supported target frameworks:
  [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0),
  [.NET 9](https://dotnet.microsoft.com/download/dotnet/9.0), and
  [.NET 10](https://dotnet.microsoft.com/download/dotnet/10.0)
  (or an install that provides SDKs for 8, 9, and 10).
- **Node.js (LTS)** on `PATH` if project targets require it during pack/build.
- Clone the repository:
  ```shell
  git clone https://github.com/syncfusion/blazor-toolkit.git
  cd blazor-toolkit

### Windows (primary)

- Install Visual Studio 2022 (v17.10 or newer) with the ASP.NET and web development workload, or use the `dotnet` CLI alone.
- Build: `dotnet build ./src/Syncfusion.Blazor.Toolkit.slnx`
- Samples: open `samples/Blazor.Toolkit.Samples.slnx` in Visual Studio and set the desired startup project.

### macOS / Linux
- Use the `dotnet` CLI with the SDKs above.
- Optional: VS Code or JetBrains Rider.
- Build: `dotnet restore` then `dotnet build ./src/Syncfusion.Blazor.Toolkit.slnx`.
- Samples: open `samples/Blazor.Toolkit.Samples.slnx` in a supported IDE, or run the host project with `dotnet run` from the samples folder.


## Building the Solution

1. Open a command prompt/terminal window.
2. Navigate to the location of your cloned `syncfusion/blazor-toolkit` repo.
3. Run these commands:
    ```dotnetcli
    dotnet restore
    dotnet build ./src/Syncfusion.Blazor.Toolkit.slnx
    ```

### Release sanity check (local)

To produce a local pack that matches CI **unsigned** pack properties (SourceLink / package metadata), pass `-p:ContinuousIntegrationBuild=true`:

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
| Every **major release** (`x.0.0`) | Full review of [THREAT-MODEL.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/THREAT-MODEL.md): reassess all threats, verify mitigations, update or re-ratify the accepted-risks table. |
| Every **minor release** (`x.y.0`) | Review the accepted-risks table; escalate any entry whose circumstances have changed. |
| **Monthly servicing** | Dependency review and CVE triage per [SECURITY.md §3.3](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/SECURITY.md). |

The outcome of each structured review is recorded by updating the "Security Self-Attestation" date in [SECURITY.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/SECURITY.md) and the accepted-risks table in [THREAT-MODEL.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/THREAT-MODEL.md).

## Versioning and API stability

The project follows [Semantic Versioning 2.0.0](https://semver.org/spec/v2.0.0.html).

Release history:

- **GitHub Releases:** <https://github.com/syncfusion/blazor-toolkit/releases>
- **In-repo changelog:** [CHANGELOG.md](https://github.com/syncfusion/blazor-toolkit/blob/main/CHANGELOG.md)

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

The package version is declared in `src/Syncfusion.Blazor.Toolkit.csproj`
(currently `2.0.1`). The latest published GitHub release is `v2.0.1`.

The pre-1.0 SemVer exception no longer applies. On **2.x**, breaking changes
require a new major version. **2.0.0** was a breaking re-baseline; see
[CHANGELOG.md](https://github.com/syncfusion/blazor-toolkit/blob/main/CHANGELOG.md),
[docs/API-COMPATIBILITY.md](https://github.com/syncfusion/blazor-toolkit/blob/main/docs/API-COMPATIBILITY.md), and
[docs/API-BREAK-INVENTORY.md](https://github.com/syncfusion/blazor-toolkit/blob/main/docs/API-BREAK-INVENTORY.md).

APIs marked `[Obsolete]` are retained for at least **two minor
releases** before removal. APIs marked `[Experimental]` are not
covered by the SemVer compatibility promise and may change in any
release.

---

## Release readiness defects (D1–D9)

The numbers below correspond to the readiness-defect list reviewed on
2026-09-06 and were re-baselined against the shipped 2.0.1 behaviour.
NuGet package signing, SBOM generation and publication are performed by
the **`Publish to GitHub Packages (Signed)`** workflow
(`.github/workflows/release.yml`) — see
[Release, signing and publication](#release-signing-and-publication). They
are no longer manual and no longer happen outside this repository.

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
matches the on-`main` tag-candidate commit. Verification before release:

```sh
git cat-file -e <SHA>^{commit} \
  || { echo "ERROR: <SHA> is not on main"; exit 1; }
```

### Release Checklist — RepositoryCommit resolution (MS-1.4(b) / LP-10)

The `<RepositoryCommit>` field on the produced `.nuspec` is **never** a
literal in the repository; it is populated automatically at `dotnet pack`
time via `Microsoft.SourceLink.GitHub` + `Directory.Build.props`. The
release maintainer MUST run the following checklist on every candidate
build, **before** publishing the GitHub release that triggers the
[release workflow](#release-signing-and-publication) (PI-01, PI-02).

1. **Confirm the public commit SHA on disk matches the on-`main`
   tag-candidate commit.**

   ```sh
   git rev-parse HEAD                 # local SHA
   git ls-remote https://github.com/syncfusion/blazor-toolkit.git HEAD \
     | awk '{print $1}'               # upstream SHA
   test "$LOCAL_SHA" = "$UPSTREAM_SHA" \
     || { echo "ERROR: HEAD is not on main"; exit 1; }
   ```

2. **Confirm `SourceRevisionId` (set by SourceLink during `dotnet
   restore` for `EmbedUntrackedSources=true`) equals the local SHA.**

   ```sh
   dotnet restore src/Syncfusion.Blazor.Toolkit.csproj
   git rev-parse HEAD                 # local SHA — must match
   # SourceLink injects the SHA into the assembly metadata; the
   # .nupkg's <repository> element will reflect it after pack.
   ```

3. **Smoke-pack the unsigned `.nupkg` per TFM and inspect the `<repository>`
   element.**

   ```sh
   dotnet pack src/Syncfusion.Blazor.Toolkit.csproj \
     -c Release \
     -p:ContinuousIntegrationBuild=true \
     -o ./verify-pkg
   unzip -p verify-pkg/Syncfusion.Blazor.Toolkit.<Version>.nupkg \
     Syncfusion.Blazor.Toolkit.nuspec \
     | grep -E '<repository '
   # Expected: an XML element whose 'commit' attribute equals the local SHA.
   ```

4. **Record the SHA + branch on the release ticket.** This is
   the same SHA the CI `pack` job echoes (`PACK_REPO_COMMIT=${github.event.pull_request.head.sha || github.sha}`);
   the on-disk SHA MUST match it. If it does not, abort the release —
   do not ad-hoc re-pack, do not bypass.

   - `Directory.Build.props` — `RepositoryCommit` default + branch resolution (`<SourceRevision>` → `<RepositoryCommit>`)
   - `src/Syncfusion.Blazor.Toolkit.csproj` — `<RepositoryUrl>`, `<EmbedUntrackedSources>true</EmbedUntrackedSources>`, `<PublishRepositoryUrl>true</PublishRepositoryUrl>`
   - `.github/workflows/ci.yml` — `pack` job smoke-packs an unsigned `.nupkg` per TFM to confirm the wire-up
   - `.github/workflows/release.yml` — packs, signs and publishes the release package from the tagged commit

### D2 / PI-01 — Strong-name signing (not applied; accepted risk)

**Policy A is in force: shipped assemblies are _not_ strong-name signed.**
`Syncfusion.Blazor.Toolkit.dll` is built with `PublicKeyToken=null`. The
repository contains no strong-name key material, no `SignAssembly=true`
directive and no `AssemblyOriginatorKeyFile` value, and neither
`release.yml` nor any manual step applies strong-name signing
(there is no `sn -R` step). Documentation must not claim otherwise.

The authentication control for a release is the **NuGet package
signature** on the `.nupkg` (see D3) plus the SLSA build-provenance
attestation, not an assembly identity. Consumers who require strong-named
assemblies are not served by this package; this is Accepted Risk AR-1 in
THREAT-MODEL.md. Changing this policy (Policy B: strong-name signing the
DLLs before pack) requires a new key-management decision, a build change
and a new accepted-risk review, and must update this section, AR-1 and
`release.yml` together.

### D3 / PI-02 — Authenticode and `.nupkg` signing

The inner DLLs are not Authenticode signed (`NotSigned`). The outer
`.nupkg` is signed by `release.yml` with `dotnet nuget sign` using the
`NUGET_SIGNING_CERT_BASE64` / `NUGET_SIGNING_CERT_PASSWORD` repository
secrets and a DigiCert RFC 3161 timestamp. The workflow fails if the
secrets are missing, and then runs `dotnet nuget verify --all` on every
package before anything is attested, pushed or uploaded. The package
signature is a single (primary) signature; no counter-signature is applied.

- Accepted Risk AR-2 in THREAT-MODEL.md.

### D4 / BEQ-10 — Required parameters

Every `Sf*` component parameter whose XML documentation describes the
parameter as required must also carry `[EditorRequired]`. The
`tests/Syncfusion.Blazor.Toolkit.BUnitTest/Base/EditorRequiredAttributeTests.cs`
bUnit test enforces this by reflection over every public `[Parameter]`
in `src/Components/` and asserts `EditorRequiredAttribute` presence
when the docs mark the parameter as required.

### D5 / BEQ-20 — Style and theme delivery (2.0)

As of **2.0.0**, toolkit styling is delivered in two layers:

1. **Component styles** — Colocated Blazor CSS isolation (`.razor.css` next to
   each component). Edit component appearance in those source files. The build
   produces the scoped CSS bundle.

2. **Shared theme layer** — `:root` tokens, base/utility styles, icon font,
   keyframes, and dark / high-contrast / forced-colors rules are emitted at
   render time by **`SfThemeRoot`** (`src/Base/SfThemeRoot.cs`,
   `SfThemeRoot.Payload.cs`), with ownership coordinated by **`SfThemeScope`**.
   Root toolkit components render `<SfThemeRoot />` as the previous sibling of
   their root element. For the common case, consumers do **not** need separate
   toolkit theme `<link>` tags.

Class names may still use the `e-*` prefix where applicable. Prefer matching
existing isolated selectors and the theme payload rather than a permanent
checked-in `src/wwwroot/styles/` tree. `src/wwwroot/` primarily holds
JavaScript interop scripts.

See [CHANGELOG.md](https://github.com/syncfusion/blazor-toolkit/blob/main/CHANGELOG.md) section `[2.0.0]` (theme delivery model and
CSS isolation) for migration details.

### D6 / PI-06, PI-07 — SBOMs and their subject

Each release carries two SBOMs plus a checksum file, all attached to the
GitHub release:

| Asset | Format | Subject |
|---|---|---|
| `Syncfusion.Blazor.Toolkit.cdx.json` | CycloneDX (JSON) | The package's dependency graph. Generated by `dotnet-CycloneDX` in `release.yml`; signing does not change dependencies, so it applies equally to the unsigned and signed package. |
| `Syncfusion.Blazor.Toolkit-<tag>.spdx.json` | SPDX | The **final signed `.nupkg` files** in the release. |
| `SHA256SUMS` | text | SHA-256 of every `.nupkg`, the CycloneDX file and the SPDX file. |

The SPDX file is generated by `sbom-tool` **after** `dotnet nuget sign` and
`dotnet nuget verify`, so the SHA-256 recorded in it is the digest of the
signed package. The workflow then re-reads the SPDX file and fails the
release if any recorded SHA-256 differs from the digest of the package
about to be published. (Earlier releases generated SPDX before signing, so
its digest matched neither distributed package; that no longer happens.)

The signed `.nupkg` attached to the GitHub release is the same file that is
pushed to GitHub Packages. If a copy of it is ever published to another
registry (for example nuget.org), it must be that exact file — do not
re-pack or re-sign — and then its digest equals the one in `SHA256SUMS` and
in the SPDX file. A re-packed or re-signed package has a different digest
and is **not** covered by the published SBOM.

- Accepted Risk AR-5 in THREAT-MODEL.md.

### D7 / CI-01 — PR CI gates pack

Every PR runs restore → build → bUnit → Playwright → eslint → xss →
vulnerability scan → **pack** for .NET 8/9/10. The `pack` job
produces an **unsigned** `.nupkg` artifact labelled
`unsigned-pkg-<tfm>` for human review only, and checks the popups and
calendars AI skill documentation against the packed public API
(`.github/scripts/check-skill-api.py`). Pack must succeed with
`RepositoryCommit=<PR head SHA>` before the summary job reports
success. The artifact is not the publication candidate and must not be
pushed to any registry.

### D8 / CI-07 — Repository security boundary

Signing and publication run in `.github/workflows/release.yml`, which is
triggered only by a **published GitHub release**. It is the only workflow
that touches signing material or a publish credential:

- the signing certificate is read from the `NUGET_SIGNING_CERT_BASE64` /
  `NUGET_SIGNING_CERT_PASSWORD` secrets, written to a temporary file and
  destroyed immediately after signing;
- registry authentication uses the job's `GITHUB_TOKEN` (`packages: write`);
  there is no `NUGET_API_KEY` secret;
- there is no `STRONG_NAME_KEY_BASE64` secret (see D2);
- `ci.yml` (pull requests) never has access to these secrets and never signs
  or publishes.

Protect the release path accordingly: restrict who can publish a release,
require review on changes to `.github/workflows/release.yml`, and scope the
signing secrets to the protected release environment where available.

- Accepted Risk AR-4 in THREAT-MODEL.md.

### D9 / CI-08 — Tamper-evident hand-off

The tamper-evident chain is automated in `release.yml`: pack →
`dotnet nuget sign` → `dotnet nuget verify` → SPDX generated from the signed
files and cross-checked against `sha256sum` → `SHA256SUMS` → SLSA provenance
attestation (`actions/attest-build-provenance`) over the signed `.nupkg` →
push to GitHub Packages → upload to the GitHub release. Consumers can verify a
downloaded package with `dotnet nuget verify --all`,
`gh attestation verify <file> --repo syncfusion/blazor-toolkit` and
`sha256sum -c SHA256SUMS`.

### Release, signing and publication

This is the single authoritative release path.

**Distribution channel.** Release packages are published to **GitHub
Packages** (`https://nuget.pkg.github.com/syncfusion/index.json`) and
attached to the corresponding **GitHub release**. Both carry the identical
signed `.nupkg`. `release.yml` does not publish to nuget.org and nothing in
this repository holds a nuget.org API key; treat nuget.org as outside the
automated path. Any other channel must redistribute the signed `.nupkg` from
the GitHub release unchanged (see D6).

**Procedure (maintainer):**

1. Confirm the tag-candidate commit is on `main` and CI is green
   (see the [RepositoryCommit checklist](#release-checklist--repositorycommit-resolution-ms-14b--lp-10)).
2. Update the version in `src/Syncfusion.Blazor.Toolkit.csproj` and
   `CHANGELOG.md`, merge to `main`, and tag the release commit.
3. Publish a GitHub release for the tag. This triggers
   **Publish to GitHub Packages (Signed)**.

**What the workflow does, in order** (a failure in any step stops the
release and nothing later runs):

| # | Step | Gate |
|---|---|---|
| 1 | Restore | — |
| 2 | Vulnerability scan (`.github/scripts/check-vulnerabilities.sh`) | Fails on command, restore or advisory-source errors, then on any High/Critical finding in the structured JSON result. Failures are never masked. |
| 3 | `dotnet pack -c Release` (deterministic, SourceLink) | — |
| 4 | Skill-documentation API check (`.github/scripts/check-skill-api.py`) | Popups and calendars skills must only reference members present in the packed public API. |
| 5 | CycloneDX SBOM | Must be non-empty. |
| 6 | Signing-secret check, `dotnet nuget sign`, `dotnet nuget verify --all` | Missing secrets or an invalid signature fail the release. |
| 7 | SPDX SBOM from the signed packages + digest cross-check + `SHA256SUMS` | SPDX SHA-256 must equal the signed package digest. |
| 8 | SLSA provenance attestation over the signed `.nupkg` | — |
| 9 | `dotnet nuget push` to GitHub Packages (`--skip-duplicate`) | — |
| 10 | Upload `.nupkg`, `.cdx.json`, `.spdx.json`, `SHA256SUMS` to the release | Fails if an expected file is missing. |

`--skip-duplicate` means re-running the workflow for a version that already
exists in GitHub Packages will not replace it. If a release must be redone,
publish a new patch version; do not delete and re-push a version, as that
would give two different digests for one version.

To exercise the same checks locally:

```sh
bash .github/scripts/check-vulnerabilities.sh --project src/Syncfusion.Blazor.Toolkit.csproj
python3 .github/scripts/check-skill-api.py --xml <path-to>/Syncfusion.Blazor.Toolkit.<version>.nupkg \
  --skills .github/skills/syncfusion-blazor-toolkit-popups .github/skills/syncfusion-blazor-toolkit-calendars
```

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

`Directory.Build.targets` skips this embedded SBOM when `CI=true`, so
`release.yml` generates the CycloneDX file explicitly and attaches it to the
release (see D6). To produce the embedded copy locally, run
`dotnet tool install --global CycloneDX` once and `dotnet pack` outside CI.

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
[RENDER-MODE-SECURITY.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/RENDER-MODE-SECURITY.md).

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
[TEST-MATRIX.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/TEST-MATRIX.md).

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

#### Standing IDE compatibility rationale (2026-10-02)

The audit found 291 unique warning-level IDE diagnostic locations:
267 in the clean library build and 24 additional simplifications in
`dotnet format style --verify-no-changes --no-restore --severity warn`.
Of these, 148 were fixed in source. The remaining 143 compatibility
findings are covered by 135 central `Scope = "member"` entries in
`src/Properties/GlobalSuppressions.cs`.
Entries are grouped by rule, use exact `~F` / `~M` / `~P` targets and
carry a `2026-10-02:` justification. Counts differ where several
locations belong to one method. Documented IDs were checked against
the generated XML documentation; undocumented private-member IDs
were derived from source declarations.

| Rule | Diagnostics | Entries | Standing rationale |
|---|---:|---:|---|
| `IDE0032` | 96 | 96 | Target the backing **field** at each diagnostic, not its property. Retain custom/reactive chart accessors, lazy query-method caches and guarded uploader event reads. Ordinary auto-properties lose side effects, lazy initialization or guard semantics; C# 14 `field` syntax cannot compile on .NET 8/C# 12 or .NET 9/C# 13. Six trivial properties were already converted and are excluded. |
| `IDE0031` | 27 | 22 | All suggestions are null-conditional **assignments**, including event `-=`. Retain explicit guards in shared older-target source because this assignment syntax requires C# 14. |
| `IDE0340` | 4 | 1 | Keep bound `Nullable<DateTime>` / `Nullable<DateTimeOffset>` `nameof` expressions in the exact private `QueryableExtensions.Predicate` overload. Unbound generic `nameof` requires C# 14, unavailable in C# 12/13. |
| `IDE1006` | 11 | 11 | Preserve ABI/source and JS/JSON names: protected `BaseComponent._uniqueId`; public `Refresh`, `Trigger`, `UpdateModel`; both `DataManager.ExecuteQuery<T>` overloads; `WhereFilter.value`; `SfTimePicker<T>.ShowPopup`/`HidePopup`; and internal `IChartInternalLocation.x`/`y` wire names. |
| `IDE0060` | 3 | 3 | Preserve public `SfChart.TriggerZoomingEvents(string, bool)`, protected `SfDatePicker<T>.SelectCalendarAsync(bool)` and public `ValueConvert.ParseValueWithTypeInformation(string, object, bool)` signatures, including parameter names for named arguments and JS dispatch compatibility. |
| `IDE0390` | 1 | 1 | Keep `SfChart.UpdateNeededRenderersAsync` as an awaited async helper so errors remain faulted-Task results rather than synchronous throws. |
| `IDE0391` | 1 | 1 | Keep `TrimTooltipBase.OnInitializedAsync` in the async lifecycle hook to preserve initialization ordering, subclass/base-call contracts and faulted-Task error delivery. |
| **Total** | **143** | **135** | No new type-, namespace- or assembly-wide suppression. |

Only the stale `CA2213` entries for `ChartStackLabelSettings._font`
and `ChartStackLabelSettings._margin` were removed: their explicit
fields were removed by the safe auto-property conversion. The
`_border` entry and every other pre-existing suppression remain unchanged.
Analyzer settings and security policy/suppressions are unchanged.
**BL0007 suppressions and `docs/BL0007-analysis.md` are unchanged.**
Re-attest these entries at each major release, especially if supported
language versions or member contracts change.

Verification on 2026-10-02:

- Clean library build across .NET 8/9/10: **0 IDE diagnostics, 0 errors**;
  total warnings decreased from 940 to 405. The remaining 135 unique
  non-IDE locations (reported once per target) were unchanged.
- Warning-level style verification: **0 findings** in the library;
  all five other C# projects also reported no warning-level style fixes.
- Sample solution and Playwright sample-host solution builds passed.
- bUnit could not execute: the parent test project's default compile
  glob includes `Buttons/ButtonGroup/obj` assembly attributes, causing
  duplicate attributes. A temporary external build target excluding
  only those nested generated inputs exposed five existing `CS0266`
  `IList<T>`-to-`List<T>` assignment errors in `SfDialogTest.cs`,
  `SharedTooltip.razor`, `Crosshair.razor`, and `Events.razor`.
  Test source and project configuration were not changed; no passing
  regression-test result is claimed.

#### Nullable and Razor follow-up (2026-10-03)

This follow-up supersedes the remaining-warning and blocked-test results above:

- Clean library build across .NET 8/9/10: **3 warnings, 0 errors**, down
  from 405 warnings. All three are the same `CS8714` location on
  `DataAdaptor<T>` in `src/Data/DataManager.cs`, reported once per target.
- All 19 unique `RZ10012` findings were fixed by rendering the internal
  chart components through typed fragments while retaining their accessibility,
  ordering, SVG nesting, and cascading context.
- Accurate nullable annotations were explicitly approved for existing null
  inputs/results. CLR signatures and collection types were retained. Null
  reflection values, grouping sentinels, and prompt cancellation remain null;
  no replacement empty values or required public properties were introduced.
- Null/missing first-record `DynamicObject` values cannot establish a query
  field's type. Filtering/searching now report `InvalidOperationException`
  with the field name rather than an incidental null dereference; later
  records are not substituted for inference. This exception clarification is
  documented on the public methods and in `CHANGELOG.md`.
- Chart axis-overlap tracking was made instance-scoped after parallel tests
  exposed a shared-static-state race. A deterministic two-layout regression
  failed before the fix and passed afterward; reset timing/values are retained.
- bUnit now compiles: nested `obj`/`bin` C# is excluded and the five test
  assignments consume the existing `IList<T>` contracts. Chart fixtures mock
  real module boundaries and wait for initialization rather than treating
  initial markup as completed initialization. **2,735 tests passed, 0 failed,
  0 skipped**, including 142 new regression cases. The test project targets
  .NET 8; this is not a claim of running tests on .NET 9/10.
- Warning-level library style verification reports **0 findings**. No new
  warnings were reported in the added regression-test files or shared chart
  fixture. Existing warnings elsewhere in the test project are outside this
  library-warning cleanup; the entire repository is not warning-free.
- Library analyzer/security settings, `GlobalSuppressions.cs`, and BL0007
  policy remain unchanged. This pass added no suppressions and disabled no
  analyzer rules. Browser end-to-end, trimming publication, and Native AOT
  publication were not run as part of this follow-up.

#### Approved generic adaptor constraint (2026-10-03)

The initially deferred `CS8714` is now addressed with explicit approval:
`DataAdaptor<T>` declares `where T : notnull`, matching the service-type
requirement of `OwningComponentBase<T>`. This supersedes the three-warning
status above. The existing `DataAdaptor<object>` test subclass already satisfies
this requirement; no repository generic subclasses required changes.

Consumer migration: propagate `where T : notnull` on generic subclasses and
use non-nullable service type arguments. Nullable arguments and unconstrained
type parameters may produce new compiler diagnostics when consumers rebuild,
including errors where warnings are treated as errors. `notnull` is a
compiler-level nullability contract, not a new CLR-enforced runtime constraint.
Existing binaries, service resolution, owned scopes, and disposal behavior are
unchanged. No broader `class` or `new()` restriction was introduced, and no
analyzer setting or suppression was changed.

Verification after the approved constraint change:

- Non-incremental library build across .NET 8/9/10: **0 warnings, 0 errors**.
- bUnit on .NET 8: **2,735 passed, 0 failed, 0 skipped**.
- Warning-level library style verification: **0 findings**.
- Sample solution and Playwright sample-host solution builds: **passed**.
- These results apply to the library build and stated checks; existing warnings
  in test/sample projects and editor-only suggestions are not claimed resolved.

### Trim and AOT residual warnings

Residual `ILLink` warnings from `-p:PublishTrimmed=true` against
`samples/Blazor.Toolkit.Samples` and residual `ILCompiler` warnings
from `-p:PublishAot=true` against
`samples/Blazor.Toolkit.Samples.Client` are captured under the
`trim-and-aot` label in the issues queue. Each issue lists the rule
id, the symbol, and the planned remediation.
