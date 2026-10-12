# Development Guide

Build and work on the Syncfusion Toolkit for Blazor from source. For application
setup, start with the [README](../README.md). Work from `main` and target pull
requests there. The toolkit is Chart-first; this repository does not contain a
Grid component. See [CONTRIBUTING.md](CONTRIBUTING.md) before starting a large change.

## Prerequisites

- **.NET 10 SDK** for the `net8.0;net9.0;net10.0` library and the .NET 10 sample
  hosts. There is no checked-in `global.json` SDK pin. Check `dotnet --info` and
  `dotnet --list-sdks` in the terminal used for the build.
- **.NET 8 runtime** to execute the `net8.0` bUnit project. Installing SDK 10
  alone does not install that runtime; check `dotnet --list-runtimes`.
- **Node.js 22.13 or newer within the 22.x line** is the documented CI-aligned
  choice, with npm, for ESLint/tooling and themes. R00 ran on Node 26.10.0/npm
  12.1.0, not Node 22; that run does not validate all supported tooling versions.
- **CycloneDX global .NET tool** for default non-CI library builds and packs:
  `dotnet tool install --global CycloneDX`. The target invokes the executable
  under `$HOME/.dotnet/tools` (Windows: `%USERPROFILE%\.dotnet\tools`) directly.
  Record the installed version when collecting evidence; it is not pinned here.
- **`wasm-tools` for SDK 10** is needed for WebAssembly AOT/native-toolchain
  scenarios: `dotnet workload install wasm-tools`. It was not listed among R00's
  installed workloads, and the ordinary Debug gallery build passed; do not
  treat it as a measured prerequisite for that build or claim AOT was tested.
- Git and an editor such as Code Studio. Use a terminal whose .NET installation
  contains both the selected SDK and the runtime needed by the tests.

## Build from the repository root

Generate themes explicitly from the root, where `package.json` and `gulpfile.js`
reside. The current checkout has a tracked, matching `package-lock.json`; use
`npm ci` to install its resolutions. If working from a revision without a matching
lockfile, use `npm install` and record the newly resolved dependencies.

```sh
npm ci
npx gulp blazor-toolkit-themes
dotnet restore src/Syncfusion.Blazor.Toolkit.slnx
dotnet build src/Syncfusion.Blazor.Toolkit.slnx -c Release --no-restore
```

The source solution contains the library only. Do not use a bare root-level
`dotnet restore` or an assumed root-level solution path. The project's
`RunGulpOnceBeforeBuild` fallback checks for `fluent.min.css` on the first target
framework, but explicit root-level generation avoids its working-directory
ambiguity and refreshes CSS after SCSS changes. `dotnet pack --no-build` must not
be relied on to generate themes or rebuild assemblies.

For local development only, `-p:CycloneDxEnabled=false` can be passed to build,
test, or pack commands to skip the SBOM targets when the global tool is unavailable.
This is an opt-out, not release evidence or certification. No target configuration
change is needed. See [Repository metadata and SBOM](#repository-metadata-and-sbom).

## Run tests and samples

Run the component tests using the explicit project path:

```sh
dotnet test tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj -c Release -f net8.0
```

The earlier `run-tests.sh` reference is not a repository entry point: no such
script is present. Use the explicit project command above rather than a hardcoded
SDK path.

The standalone ButtonGroup project now references Toolkit source, not a wildcard
commercial package, and runs only its ButtonGroup tests plus shared harness checks:

```sh
dotnet test tests/Syncfusion.Blazor.Toolkit.BUnitTest/Buttons/ButtonGroup/Syncfusion.Blazor.Tests.ButtonGroup.csproj -c Debug
```

Those cases also run in the main suite; do not sum the two results as unique
coverage. Build the nested project before the parent when checking nested-output
exclusion. `TestProjectContractTests` checks project declarations and emitted
assembly identity; it assumes test output is beneath the checkout, not an external
artifact directory. It is not full transitive/MSBuild-import enforcement.

[R02 evidence](../docs/evidence/2026-10-06-r02-1922761/index.md) records 2,738 main
cases passing in both Debug and Release, 36 standalone cases passing, and default
clean/nonincremental library builds succeeding without R00's compiler switches.
The earlier compiler-state failure was **not reproduced**, not proven fixed;
retain diagnostics if it recurs. Existing test/sample warnings are not waived.

Build the gallery solution and run its server host (the client is a referenced
WebAssembly project, not a separate server):

```sh
dotnet build samples/Blazor.Toolkit.Samples.slnx -c Release
dotnet run --project samples/Blazor.Toolkit.Samples/Blazor.Toolkit.Samples.csproj -c Release
```

For browser checks, after theme generation and npm installation:

```sh
npx playwright install chromium
npx playwright test
```

On Linux, browser system dependencies may also be needed. Playwright starts
`tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.Playwright.Samples/Blazor.Toolkit.Playwright.Samples.csproj`
on port 5000. It can reuse an existing server, so ensure that port belongs to the
intended host. Current coverage is Chromium against one .NET 10 Interactive
Server host, not all browsers or render modes. See [TEST-MATRIX.md](TEST-MATRIX.md)
for configured checks, local results, and gaps.

## Versioning and evidence status

Use [Semantic Versioning](https://semver.org/spec/v2.0.0.html): major for breaking
APIs, minor for compatible additions, patch for compatible fixes. [R03's API policy](../docs/API-COMPATIBILITY.md)
now pins NuGet 1.0.2 and APICompat 10.0.401, records 194 individually named approved
major exceptions, and separately guards the reviewed current public/protected,
Razor/callback and representative JSON surface. The user authorized source
`Version=2.0.0` as an **uncommitted source change**, not a publication. No
`PublicAPI.Shipped.txt` is used; compressed contract manifests and exact exceptions
live under `tools/api-compat/`.

Follow the policy's build/gate commands before changing an API. CI's new
`api-compatibility` job blocks pack/summary on failure; remote results and admin
required-check settings remain unverified. [R03 evidence](../docs/evidence/2026-10-06-r03-1922761/index.md)
retains passing source checks and deliberate rejected mutations. Never generate
blanket suppressions or automatically replace reviewed snapshots. CSS isolation
is planned separately; current asset instructions remain until consumer-tested.
Compatibility and deprecation decisions still require review; no fixed removal
window or universal old-state migration guarantee is established.

The public-release metadata reviewed on 2026-10-03 and rechecked in the
[retained R00 response on 2026-10-06](../docs/evidence/2026-10-06-r00-1922761/index.md#release-identity) lists
[v1.0.2](https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.2)
(2026-09-01) and
[v1.0.1](https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.1)
(2026-06-26), both with empty release-asset lists. No `v1.0.0` release record was
present in those responses. This does not establish the absence of a tag or NuGet
package. Source was `1.0.0` in that baseline; R03 now authorizes `2.0.0` in the
uncommitted tree. Maintainers must still reconcile candidate source/package/tag
identity before publishing. Local unreleased changes and the dated results below
are not evidence of what shipped in those releases.

## Manual release preparation

**Signing and publication remain manual, owner-controlled operations.** Public
CI smoke-packs unsigned review artifacts; there is no public publish workflow.
Absence of signing configuration does not prove that a released package or its
assemblies are signed. Strong-name, Authenticode, and NuGet package signatures
are different checks and require evidence from the actual final artifact.

For every release, copy the [release checklist](../docs/RELEASE-CHECKLIST.md)
into the release record. Start with approval and a source `<Version>` update
before freezing the candidate SHA; the checklist includes evaluated-version
checks to catch missed updates. Use the [manual/external evidence tracker](../docs/MANUAL-EXTERNAL-EVIDENCE-TRACKER.md)
for named owners, execution steps, report formats and approval records. The
[remaining-issue closure plan](../docs/ENTERPRISE-READINESS-PLAN.md#12-remaining-issue-closure-plan)
orders implementation work; these documents do not authorize signing/publication
or mark an unexecuted gate complete.

### Manual NuGet sign and publish (PI-01, PI-02)

This is a preparation checklist, not a replacement for the release owner's
approved signing tools and procedure. Do not add publishing credentials or
signing keys to this repository or public CI.

1. Agree the version and candidate commit with the release owner. Start from a
   clean checkout of that exact public commit and record SDK, runtime, npm,
   dependency, workload, and CycloneDX versions plus relevant test results.
2. Fetch the approved upstream `main` reference, verify the candidate's ancestry
   with `git merge-base --is-ancestor <candidate-sha> <upstream-remote>/main`, and
   compare `git rev-parse HEAD` with the approved candidate SHA. Merely finding
   an object with `git cat-file -e` does not prove it belongs to `main`; the
   current tip of `main` need not equal an approved earlier candidate.
3. Generate themes, restore, build, and pack the same candidate using a consistent
   Release configuration. Confirm the agreed package version rather than assuming
   the source `1.0.0` value is appropriate. For an **unsigned local smoke check**
   after the prerequisite/theme steps:

   ```sh
   dotnet restore src/Syncfusion.Blazor.Toolkit.csproj
   dotnet build src/Syncfusion.Blazor.Toolkit.csproj -c Release --no-restore
   dotnet pack src/Syncfusion.Blazor.Toolkit.csproj -c Release --no-build -o nupkg
   ```

4. Inspect the `.nuspec` repository URL, commit and version, target-framework
   contents, static assets, PDB/SourceLink metadata, and any included SBOM.
   Metadata strings are assertions to check, not proof of provenance.
5. Apply any required assembly signing **before** final packaging using the
   owner's approved build/signing process. Do not use `sn -R` as a recipe to
   convert an arbitrary unsigned DLL into a correctly strong-named assembly.
   The public source build is not evidence of the released assemblies' signing.
6. Apply and verify the final NuGet signature with the owner's approved tooling,
   certificate policy and timestamp procedure. Repeated `nuget sign` calls are
   not a generic countersigning procedure. Verify any repository countersignature
   separately on the downloaded published package. No certificate identity, HSM
   use, or signature status is established by this guide.
7. Collect package-specific dependency/license and SBOM evidence, recording the
   input artifact and tool versions. The source-project CycloneDX output described
   below is not an inventory derived from a signed package. Do not label it so.
8. Record a SHA-256 digest of the **final** candidate and keep the verification
   record with the release approval. A digest identifies bytes; by itself it
   neither authenticates the publisher nor proves source/build provenance.
   Do not rebuild or mutate the artifact after final verification.
9. The authorized owner publishes manually through the approved private process.
   Verify the downloaded package, release/tag/version mapping and any evidence
   attachments afterward. Record any repository-added signature separately from
   the pre-upload artifact. This guide contains no publication credential examples.

`ContinuousIntegrationBuild` is already set in `Directory.Build.props`. Setting
that property does not run a workflow or guarantee identical local/CI hashes.

### Historical readiness references (2026-09-06)

The D1–D9 / LP / PI / BEQ / CI labels below are retained for audit traceability,
not as certifications or closure claims. Consult [THREAT-MODEL.md](THREAT-MODEL.md)
for the historical accepted-risk discussion; current evidence must be attached
before relying on a control.

| Reference | Current interpretation / evidence needed |
|---|---|
| D1 / LP-10 / MS-1.4(b) | Repository metadata is configured; inspect the candidate `.nuspec`, SourceLink data and approved public commit. |
| D2 / PI-01 (AR-1) | Strong-name signing is an owner-controlled release concern; final-assembly verification is required. |
| D3 / PI-02 (AR-2) | Authenticode and package-signature status must be checked separately on released artifacts. |
| D4 / BEQ-10 | `Base/EditorRequiredAttributeTests.cs` checks selected types using `DescriptionAttribute`, not XML documentation across every public parameter. Review XML-doc/`[EditorRequired]` consistency separately. |
| D5 / BEQ-20 | Review styles under `src/wwwroot/styles/` and the [style contract](../src/wwwroot/styles/STYLE-CONTRACT.md); do not infer universal selector/isolation coverage. |
| D6 / PI-06 (AR-5) | Conditional source-project CycloneDX generation exists; release SBOM completeness and attachments remain unverified. |
| D7 / CI-01 | CI declares dependent unsigned pack jobs, not verified successful publication; see [TEST-MATRIX.md](TEST-MATRIX.md). |
| D8 / CI-07 and D9 / CI-08 (AR-4) | Signing/publishing stay manual. No public automated signed-artifact hand-off or provenance attestation is configured. |

### Repository metadata and SBOM

Read [Directory.Build.props](../Directory.Build.props),
[Directory.Build.targets](../Directory.Build.targets) and the
[library project](../src/Syncfusion.Blazor.Toolkit.csproj) as the source of truth.
SourceLink and the repository-property defaults are intended to populate package
metadata. Their presence alone does not verify a commit, branch, or artifact.
Inspect the actual package, especially with detached checkouts or worktrees.

The SBOM targets have these important boundaries:

- Generation hooks run after `Build` / before `GenerateNuspec` only when
  `CycloneDxEnabled=true`, `CI` is not `true`, the project is packable and not a
  test project, and `TargetFramework` is nonempty. `ContinuousIntegrationBuild`
  is a separate property; it does not determine this `CI` condition.
- The command uses the **global** CycloneDX executable, passing the source
  `.csproj`, `--framework` and `-dpr`. There is no checked-in local tool manifest;
  the conditional `dotnet tool restore` step does not replace that global-tool
  prerequisite. SDK-inferred packability can matter outside the library too.
- Output is intended under `artifacts/sbom/<package-id>/<version>/<tfm>/` and
  next to build output. Packing attempts to include an existing mirror under
  `_sbom/cyclonedx/<tfm>/`. Verify actual paths and package contents on the release
  workstation; generation, copying and packing have different conditions.
- CI generation is skipped. The pack inclusion target checks for an existing
  file, so stale local mirrors must not be accepted as fresh evidence. Neither
  `--no-build` nor a successful pack proves an SBOM was freshly generated/included.
- This is a **source-project dependency inventory**, not an inventory extracted
  from the final signed `.nupkg`, an SPDX generator, or a license-completeness
  check. Validate the generated document's format/version; the sidecar's hard-coded
  format label is not tool-version or schema verification.

The reviewed release snapshot contains no attached SBOM assets for v1.0.1 or
v1.0.2. It does not establish whether a NuGet package embeds one. See
[THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md) for dependency evidence boundaries.

## Security, render modes, performance and publishing checks

Use [SECURITY.md](SECURITY.md) for private vulnerability reporting. The review
cadence and accepted risks in that policy and [THREAT-MODEL.md](THREAT-MODEL.md)
are policy/history, not proof that scheduled reviews took place. Record the
reviewed commit, date, findings, owner and supporting artifacts; do not simply
advance an attestation date.

Render modes have different trust boundaries: static SSR has no interactive
component circuit but still needs safe HTML and endpoint handling; Interactive
Server uses SignalR and browser JS; WebAssembly executes in an untrusted client;
Auto can use both interactive hosting models. Keep secrets/server-only services
out of client code and enforce authorization on server endpoints. Consult
[RENDER-MODE-SECURITY.md](RENDER-MODE-SECURITY.md) and verify each component's
actual mode requirements rather than inferring them from the gallery or one test.

Report performance regressions with component, render mode, dataset size,
reproduction and measurements. This guide makes no virtualization or benchmark
claim for `SfChart` and does not imply a Grid or DropDownList component exists.

The library declares `IsTrimmable`, `IsAotCompatible`, and trim/AOT analyzers.
Those settings are **not proof of a successful trimmed or AOT deployment**.
No corresponding publish gate is configured in the current workflows. Browser
WebAssembly AOT uses `RunAOTCompilation`; it is not interchangeable with Native
AOT's `PublishAot`. Any deployment claim needs an appropriate supported host,
SDK/workload details, publish output and runtime checks. See
[TEST-MATRIX.md](TEST-MATRIX.md) for current evidence and gaps.

### Known analyzer / trim / AOT findings

The library project's `WarningsAsErrors` lists security-focused CA rules. Review
actual configuration and the scoped justifications in
`src/Properties/GlobalSuppressions.cs`; a suppression is not a finding of safety
for every caller or input. The historical `docs/BL0007-analysis.md` referenced by
older notes is absent from this checkout. Recover the original record from its
owner/history before relying on it; do not invent a replacement approval.

**Latest retained baseline (2026-10-06):** [R00 evidence](../docs/evidence/2026-10-06-r00-1922761/index.md)
records SDK 10.0.401 builds on macOS arm64 at commit `1922761`. Default builds
failed with cascading Razor/compiler diagnostics. Cleaning and serializing builds
alone did not resolve them; non-incremental Debug library builds across .NET
8/9/10 passed with **0 warnings/errors** using both `--disable-build-servers`
and `-p:UseSharedCompilation=false`. The same workaround built the gallery with
**232 warnings**. This is diagnostic evidence, not a source fix or proof of the
precise compiler root cause; R02 must investigate default-build reproducibility.

The full net8.0 bUnit suite passed **2,735 cases twice**, with no failures/skips,
but compilation logged **3,537 warning occurrences**. Four basic Chart browser
cases passed; the full browser matrix, trim/AOT and package consumers remain
unverified. [TEST-MATRIX.md](TEST-MATRIX.md) records scope and scan findings.
No compiler switch, suppression, dependency or build configuration is changed by
this documentation correction.

**Historical local narrative (2026-10-03):** the following notes report the older
0-warning library / 2,735-test result. R00 did not locate those original raw run
artifacts. Preserve the decisions as history, not remote CI or package proof;
the newer baseline does not retroactively substantiate missing historical logs.

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

### Trim and AOT follow-up evidence

The older guide referred to residual warnings under a `trim-and-aot` issue label,
but no publish logs or issue-to-warning inventory are established here. For a
future publish investigation, record the supported host, SDK/workload versions,
exact publish options, rule IDs, symbols, runtime checks and issue links. The
local analyzer/build results above do not substitute for that evidence.
