# R02 — Runnable regression harness, 2026-10-06

[Authoritative execution log](../../ENTERPRISE-READINESS-PLAN.md#9-execution-log-and-restart-record) · [R00 failures and baseline](../2026-10-06-r00-1922761/index.md) · [R01 documentation](../2026-10-06-r01-1922761/index.md)

**State: completed for the R02 harness gate.** Toolkit tests compile, discover nonzero cases and execute; the standalone commercial-reference defect is corrected with failing-before/passing-after coverage. This is not warning clearance, proof of all tests' quality, a supported-platform matrix or enterprise/release readiness. R03–R25 and all nine final release gates remain open.

## Source identity and scope

- User requested the next task on `address-warnings`, base `19227619082433fa3b86e15d4e7aee625850aed9`.
- Testing used a fresh detached worktree at `/tmp/blazor-toolkit-r02-pfWWYI/source` (also `/private/tmp/...`) with the two test-only changes described below. The worktree started clean; npm/themes and ordinary builds generated assets/SBOM mirrors there only. No original source, version, compiler settings, warnings, workflows or SBOM bytes were changed.
- Test configuration and new regression file were byte-compared between the tested worktree and user checkout. Incoming documentation, unrelated metadata and newly present historical documents were preserved. R00/R01 archives are not rewritten.
- Environment: macOS 27.0.1 arm64, SDK 10.0.401, net8 runtime 8.0.31, net10 runtime 10.0.12; Node 26.10.0/npm 12.1.0; Playwright 1.63.0/Chromium 153.0.8010.12. .NET tests used `TZ=UTC LANG=en_US.UTF-8`. Library builds cover net8/net9/net10; unit execution remains net8 only.

## Changes and reproduction

### Standalone ButtonGroup ownership

Before the fix, evaluated `Syncfusion.Blazor.Tests.ButtonGroup.csproj` had:

- No `ProjectReference`; commercial `Syncfusion.Blazor` with wildcard `Version="*"`.
- `SignAssembly=true`, with no test signing-key setup.
- No explicit test-project or implicit-usings setting.
- A shared `Base/*.cs` compile glob pulling unrelated test suites into a nominally standalone ButtonGroup project.

The new `Base/TestProjectContractTests.cs` ran through the existing main harness **before** the correction: **one failed, two passed**. The failing theory row identified the standalone project's missing Toolkit source reference; this was an assertion failure, not zero discovery or compiler failure. No commercial wildcard package was restored to manufacture a failure.

The configuration fix:

- References `../../../../src/Syncfusion.Blazor.Toolkit.csproj` and removes the commercial package reference.
- Disables test signing, enables implicit usings, explicitly sets `IsTestProject=true`, retains `IsPackable=false`.
- Links only `BunitTestContext.cs` and the new shared harness tests from Base. The full parent project still compiles **all** its existing Base/ButtonGroup tests; none were deleted or excluded.
- Keeps all remaining test/tool dependency versions and the existing assembly name unchanged. No public library API or production signing policy changed.

Three new cases check both project declarations and the emitted direct assembly references. The NuGet-ID check is case-insensitive. They require output beneath a source checkout and are not a complete MSBuild-import or transitive-graph validator. Actual evaluated items and restored graphs were checked separately: both resolve only `Syncfusion.Blazor.Toolkit/1.0.0` among `Syncfusion.Blazor*` entries, with `type=project`.

### F08 and compiler-state disposition

The existing parent exclusion `**/obj/**/*.cs;**/bin/**/*.cs` and five `IList<T>` fixture corrections were already present. They were not reapplied or reverted. Evaluated parent compile items after standalone output generation contain ButtonGroup and Base sources but no nested bin/obj C#.

R00's cascading Razor/compiler errors **did not reproduce** in this fresh worktree. Initial library build, default sample build, clean nonincremental library rebuild, nonincremental sample rebuild and post-browser library rebuild all passed with default compiler-server settings. No `UseSharedCompilation` or build-server override was used in R02, and none was committed. This is a measured non-reproduction, **not identification or repair of the historical root cause**. Retain R00 logs and capture compiler/environment state if it recurs; no speculative source edit or global compiler workaround is justified by this run.

## Executed results

| Check | Result | Retained evidence inside archive |
|---|---|---|
| New regression before correction | **3 total: 2 passed, 1 failed, 0 skipped** | `contracts-red.log`, `red/contracts-red.trx`, `nested-before.json` |
| Same regression after correction | **3 passed, 0 failed/skipped** | `contracts-green.log`, `green/contracts-green.trx` |
| Main discovery after standalone outputs | **2,738**, exit 0 | `main-discovery.log`, `main-compile-items.json` |
| Full main Debug, initial verification | **2,738 passed, 0 failed/skipped** | `main-full.log`, `full/main-full.trx` |
| Final main Debug | **2,738 passed, 0 failed/skipped** | `main-final-debug.log`, `final/main-final-debug.trx` |
| Final main Release | **2,738 passed, 0 failed/skipped** | `main-final-release.log`, `final/main-final-release.trx` |
| Standalone discovery | **36**, exit 0 | `nested-discovery.log` |
| Standalone execution, initial and final | **36 passed each, 0 failed/skipped** | `nested-first.log`, `nested-final.log`, corresponding TRX |
| Library default nonincremental builds | **0 warnings, 0 errors**, including after clean and after browser execution | `default-clean.log`, `default-library-rebuild.log`, `default-post-browser-library.log` |
| Gallery default Debug builds | **232 warnings, 0 errors**, including nonincremental repeat | `default-sample-before.log`, `default-sample-after.log` |
| Chart smoke first selection attempt | **Failed with zero tests**, exit 1 | `chart-smoke.log` / JSON; over-anchored CLI grep omitted filename/project prefix |
| Corrected existing Chart smoke | **1 passed, 0 failed/skipped/flaky**, retries 0 | `chart-smoke-corrected.log` / JSON |

Counts are not additive: the standalone 33 ButtonGroup cases and three shared checks also run in the 2,738-case main suite. The main increase is **2,735 + 3 harness cases**, not an invented component-coverage expansion.

The browser case is `Charts/Others/chart-basics.spec.ts` → `basic chart renders title, series and legend`. It asserts visible Chart SVG, exact title, series group, first data point and “Gold” legend. The configured host is net10 Interactive Server, Release, Chromium, localhost:5000; no listener was present before the initial browser attempt. Correcting the CLI filter did not edit or weaken the test. Other cases, duplicate IDs in adjacent chart fixtures and broader interaction/assertion quality remain R11 work. No full browser, accessibility or performance result is claimed.

## Warning review — retained, not waived

Both final main builds logged **3,537 warning occurrences**, unchanged from R00. Principal categories: CS8618 1,616; CS8602 1,533; BL0005 83; RZ10012 82; CS8604 59; other categories are retained in `verification-summary.txt`.

- All 82 RZ10012 occurrences concern `ChartAxes` in existing Chart fixtures. They signal possible fixture semantics/coverage gaps requiring R11 review, not permission to suppress warnings or claim complete Chart coverage.
- Two RZ2012 warnings in `Charts/Chart/Series/Column.razor` concern missing `XName`/`YName`; determine fixture intent under R11 before adding values merely to quiet them.
- Seven CS4014 warnings are in existing Tooltip/Chart/calendar/NumericTextBox fixtures. They require awaited-behavior review under R11; they were not introduced or hidden here.
- Nullability and externally set component-parameter diagnostics remain existing test/sample debt. No blanket analyzer/nullable changes, test skips or exclusions were used to obtain success.

The R02 gate is executable intended regression coverage; it does not close the separate R11 gate for meaningful assertions or the final warning/coverage review.

## Exact actions

From the isolated worktree root:

1. `npm ci`; `npx --no-install gulp blazor-toolkit-themes`.
2. `dotnet msbuild tests/Syncfusion.Blazor.Toolkit.BUnitTest/Buttons/ButtonGroup/Syncfusion.Blazor.Tests.ButtonGroup.csproj -getItem:ProjectReference,PackageReference,Compile -getProperty:SignAssembly,IsTestProject,ImplicitUsings` before/after correction.
3. C# Dev Tools Build action targeting the isolated library: succeeded. C# Dev Tools test actions could not discover the out-of-workspace class/project; CLI fallbacks below were used and logs retained. That tooling limitation is not a test failure or skip.
4. `TZ=UTC LANG=en_US.UTF-8 dotnet test tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj -c Debug -f net8.0 --filter FullyQualifiedName~TestProjectContractTests --logger 'trx;LogFileName=contracts-red.trx' --results-directory <evidence>/red`; repeated after correction with green names.
5. `TZ=UTC LANG=en_US.UTF-8 dotnet test tests/Syncfusion.Blazor.Toolkit.BUnitTest/Buttons/ButtonGroup/Syncfusion.Blazor.Tests.ButtonGroup.csproj -c Debug --logger 'trx;LogFileName=nested-final.trx' --results-directory <evidence>/final`; discovery used `--list-tests`.
6. Main discovery: same main project, `-c Debug -f net8.0 --list-tests`. Initial full execution used `--no-build --no-restore`; final Debug and Release executions rebuilt normally, using `--logger 'trx;LogFileName=main-final-<configuration>.trx' --results-directory <evidence>/final` with no filter.
7. `dotnet clean src/Syncfusion.Blazor.Toolkit.csproj -c Debug --verbosity quiet`; `dotnet build src/Syncfusion.Blazor.Toolkit.csproj -c Debug --no-incremental --verbosity minimal`; gallery targeted `samples/Blazor.Toolkit.Samples.slnx` with Debug, once incrementally and once `--no-incremental`. Library nonincremental build repeated after browser/test runs.
8. With `PLAYWRIGHT_JSON_OUTPUT_FILE=<evidence>/chart-smoke-corrected.json`: `npx --no-install playwright test tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Charts/Others/chart-basics.spec.ts --grep 'basic chart renders title, series and legend' --project=chromium --workers=1 --retries=0 --trace=retain-on-failure --reporter=line,json --output=<evidence>/chart-smoke-corrected-results`. Initial failing grep was `^Chart - Basic Rendering Suite basic chart renders title, series and legend$`.
9. `dotnet msbuild tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj -p:TargetFramework=net8.0 -getItem:Compile`; parse both `obj/project.assets.json` files and assert Toolkit's `type=project` and absence of commercial Syncfusion package entries.

## Review, preservation and evidence

A read-only Explore agent reviewed project boundaries, coverage preservation and cross-platform path handling; no required correction was found. Its optional case-insensitive NuGet-ID recommendation was applied, then both configurations were retested. Actual graph validation was also retained. This is agent code-quality review, **not independent maintainer/security/release approval or Windows/Linux execution evidence**.

Tested/user file SHA-256 values:

- Nested project: `579cc91434cd056d79db3be90b2b534a8b8a7a185fb735e51f7ec4c0f81c6028`.
- New regression: `e4548f7214474a21df251d59d48891f945ea992888eda3977ff88811bf642d59`.

[Harness evidence archive](harness-evidence.tar.gz), SHA-256:

`ec08a9e600c8fd536452b3540454309b43a028179b619e42d3179c9bd6486e1e`

Includes red/green/full/standalone TRX, failed/successful browser selection logs, build logs, evaluated-item JSON, graph/warning/hash summary, exact two-file patch and environment. Common-secret-pattern scan found no matches; this is not a comprehensive secrets audit. Logs contain local paths and synthetic test names. Evidence is uncommitted/local, and retention owner/reviewer assignment remains open.

Documentation now explains the standalone boundary, current counts and the absent `run-tests.sh` reference. The plan's malformed incoming closure/R01 table row was reconciled with its existing R01 completion record. Historical documents newly present at R02 start were preserved, not assigned new provenance or retroactively added to R01's evidence.

No public API/production code, sample implementation, browser spec/config, dependency version, workflow, package identity or original SBOM artifact was changed. No commit, push, tag, signing, publication or remote-setting operation occurred.

**Next: R03a — agree the released package/API baseline and version/compatibility treatment before implementing compatibility enforcement.** Source `1.0.0` is not the baseline decision; do not infer `1.0.3`. R12 dependency findings, R11 warning/assertion review, the non-reproduced compiler observation, and all platform/package/security/accessibility/performance/ownership gates remain separate open work.
