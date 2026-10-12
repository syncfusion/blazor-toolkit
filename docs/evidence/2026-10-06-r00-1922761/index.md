# R00 — Selected-commit baseline, 2026-10-06

[Authoritative execution log](../../ENTERPRISE-READINESS-PLAN.md#9-execution-log-and-restart-record) · [Evidence procedures](../../MANUAL-EXTERNAL-EVIDENCE-TRACKER.md)

**State: baseline investigation completed with limitations and classified failures; NOT enterprise ready.** Only R00 was executed. R01–R25 and all final release gates remain open. This report is exploratory source evidence, not a release-candidate approval or package verification.

## Identity, authorization and preservation

- The user explicitly selected the current `address-warnings` checkout at **`19227619082433fa3b86e15d4e7aee625850aed9`** for R00. No version, commit, signing or publication was authorized.
- The original checkout had a modified `.DS_Store`, untracked `.vscode/`, `docs/.DS_Store`, the readiness plan and its two companion documents. No tracked source, project, test or lockfile changes were present.
- Execution used an initially clean detached Git worktree at the same SHA: `/tmp/blazor-toolkit-r00-DFpdGW/source` (macOS also resolves this through `/private/tmp`). This avoided relying on the original checkout's `bin/obj` or untracked assets.
- `npm ci` restored the committed lockfile; gulp generated required CSS. Ordinary builds regenerated three tracked per-TFM SBOM mirrors **in the isolated worktree only**. The working tree is therefore no longer clean after build; those generated files are not approved release artifacts.
- Original source, package versions, dependencies, workflows and SBOM bytes were not changed. No tests were edited, disabled or weakened. No Git commit, push, tag, signing, publication or remote-settings change occurred.
- Executor: Code Studio session. Independent maintainer/security/release reviewer, evidence-retention owner and retention deadline: **UNASSIGNED**. User selection of the baseline is not approval of findings, risk acceptance or release readiness.

## Environment

| Item | Observed |
|---|---|
| OS / architecture | macOS 27.0.1, build 26A434; arm64 |
| Selected SDK / MSBuild | .NET SDK 10.0.401 / MSBuild 18.9.11 |
| Installed SDKs | 8.0.425, 9.0.318, 10.0.401; no `global.json` |
| Installed .NET / ASP.NET runtimes | 8.0.31, 9.0.20, 10.0.12 |
| Workloads | `maui`; no WASM AOT workload recorded |
| Node / npm / Git | 26.10.0 / 12.1.0 / 2.54.0 (Apple Git-157) |
| Playwright / Chromium | 1.63.0 / 153.0.8010.12 |
| Global CycloneDX | 6.2.0; existing global installation, not a repository-pinned tool |
| bUnit environment | `TZ=UTC`, `LANG=en_US.UTF-8`; Debug, net8.0 |
| Browser host | Existing net10.0 Interactive Server project, Release, localhost:5000 |

Installed SDKs are not evidence of tests running on each SDK/runtime. No browser process was listening on port 5000 before execution; Playwright started its configured host. Port 5000 was no longer listening after the run.

## Measured results

Paths in the Evidence column are inside the archived `evidence/` directory.

| Check | Result | Evidence / limitation |
|---|---|---|
| npm locked restore | Passed; 295 packages added; npm reported 11 high findings | Initial terminal action; npm blocked optional install scripts for `@parcel/watcher` and `fsevents`. No scripts approved and no audit fix applied |
| Theme generation | Passed, with Sass deprecation warnings | `assets.log`; generated CSS only, not package-content verification |
| Initial library build | C# Dev Tools reported success; Debug assemblies observed for net8/net9/net10 | Tool action recorded below; concise tool result did not expose warning counts |
| bUnit discovery | **2,735 cases**, exit 0 | `bunit-discovery.log`; 3,537 compiler/Razor warning occurrences, not 3,537 failing tests |
| Full bUnit execution | **2,735 passed, 0 failed, 0 skipped**, exit 0 | `bunit-run.log`, `bunit/bunit-net8.trx` |
| Full bUnit repeat after compiler isolation | **2,735 passed, 0 failed, 0 skipped**, exit 0 | `bunit-repeat.log`, `bunit/bunit-net8-repeat.trx`; same 3,537 build-warning occurrences |
| Browser discovery | **2,421 tests in 325 files**, exit 0 | `playwright-discovery.log`; discovery is not execution |
| Minimal Chart browser target | **4 passed, 0 failed, 0 skipped, 0 flaky**, retries disabled | `playwright-smoke.log`, `playwright-smoke.json`; SVG/title/subtitle/dimensions only. Remaining **2,417 cases not run** |
| Default sample solution build | Failed; CLI reproduction: **122 errors, 0 warnings** | `samples-build.log`; editor action first failed without structured errors |
| Clean serial default library / sample builds | Failed; **183 errors, 0 warnings each** | `library-clean.log`, `library-serial-build.log`, `samples-serial-build.log` |
| Nonshared, nonincremental library build | Passed across net8/net9/net10: **0 warnings, 0 errors** | `library-no-shared-compiler.log`; no source change |
| Nonshared, nonincremental sample solution build | Passed: **232 warnings, 0 errors** | `samples-no-shared-compiler.log`; 464 printed warning occurrences include repeated build-summary entries |
| Existing security ESLint | Passed, exit 0 | `eslint.log`; shipped `src/wwwroot/scripts/**` excluded |
| Existing XSS heuristic | Passed, exit 0 | `xss-scan.log`; shipped scripts excluded; not a security clearance |
| npm advisory audit | Failed, exit 1: **11 high, 0 critical/moderate/low** | `npm-audit.json`; affected package entries, not 11 independent exploitable product vulnerabilities |
| Library NuGet advisory query | Exit 0; no vulnerable packages reported by current feed | `nuget-audit.log`; library/transitives only, not all six projects or bundled assets |
| Checked-in SBOM checksum | **Mismatch reproduced** | Actual `c5062333134c5ac3836f3ab3abbc18fe9244e17960b6a2f08a6dbf6b8f7b2579`; expected `ce5ac7c9194195560ea5e88e60ef17b342bb221fb40f7dba11a8cbd9afaf880f` |

### Failure classification and follow-up

1. **Build environment/compiler-state sensitivity — reproduced, precise cause unproven.** Razor failures began with RZ1021 and cascaded into malformed-tag/C# errors. Cleaning isolated build outputs and using `-m:1` did not resolve them. The same unchanged source built successfully with both `--disable-build-servers` and `-p:UseSharedCompilation=false`. Both switches were changed together, so this does not identify a specific compiler-server bug or prove which switch is necessary. Retain the failed runs; investigate under R02 before claiming default-build reproducibility. No source repair was inferred from cascading diagnostics.
2. **Editor test discovery limitation — tooling, not a failed bUnit case.** The C# Dev Tools test action could not discover the temporary out-of-workspace project. The CLI fallback discovered and executed the exact intended project with retained TRX evidence. The nested commercial ButtonGroup test project was not run.
3. **Existing dependency findings — R12 triage.** Audit lists `braces`, `chokidar`, `fast-glob`, `findup-sync`, `glob-watcher`, `gulp`, `gulp-cli`, `liftoff`, `micromatch`, `shelljs`, `source-map-js`. These are npm tooling/dev dependency paths; shipped/runtime exposure needs assessment. Suggested audit changes include incompatible version changes, so no automatic fix was applied.
4. **Warnings are not waived.** bUnit warnings include nullable diagnostics, 82 RZ10012 unknown-component occurrences, 2 RZ2012 required-parameter occurrences, 7 CS4014 unawaited-call occurrences and 83 BL0005 occurrences. Sample warnings also include component and nullability diagnostics. R02/R11 must classify relevance; broad analyzer cleanup remains out of scope.
5. **SBOM integrity — existing artifact defect, R17.** The top-level checked-in snapshot and checksum disagree. Newly generated per-TFM mirrors do not repair or validate that snapshot. No checksum-only repair, schema assertion or provenance claim was made.
6. **Documentation divergence — source-confirmed, R01.** The plan's historical §§11–12 describe documentation corrections that are not consistently present in this selected commit. In particular, F14 and F17 remain present. Those historical narratives must not be treated as current dispositions.

## Inventory

Six project files were located:

- `src/Syncfusion.Blazor.Toolkit.csproj`: library, `Version=1.0.0`, net8/net9/net10, MIT metadata, SourceLink and trim/AOT analyzers.
- `tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj`: main net8 bUnit/xUnit project; Toolkit `ProjectReference`.
- `tests/Syncfusion.Blazor.Toolkit.BUnitTest/Buttons/ButtonGroup/Syncfusion.Blazor.Tests.ButtonGroup.csproj`: separate net8 project with wildcard **commercial** `Syncfusion.Blazor` dependency and signing enabled. Not executed as Toolkit validation.
- `tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.Playwright.Samples/Blazor.Toolkit.Playwright.Samples.csproj`: net10 Server host; Toolkit `ProjectReference`.
- `samples/Blazor.Toolkit.Samples/Blazor.Toolkit.Samples.csproj`: net10 sample host.
- `samples/Blazor.Toolkit.Samples.Client/Blazor.Toolkit.Samples.Client.csproj`: net10 WASM client; Toolkit `ProjectReference`, wildcard Markdig, broad Chart linker roots.

Three solution files, 325 browser spec files, and two workflow files were located. `.github/workflows/ci.yml` configures bUnit, Playwright, NuGet audit, ESLint, XSS heuristic, unsigned pack and summary jobs. `codeql.yml` includes C#, JavaScript/TypeScript and Actions analysis. Remote workflow runs and required-check settings were not verified. Playwright config is Chromium-only, has no `forbidOnly`, disables traces by default and allows server reuse; the smoke invocation overrode trace/retry/worker/reporting settings only.

No retained clean Toolkit `PackageReference` consumer was located. No repository `AGENTS.md` or general instruction file was located; contribution/development/security policies were read. Component skills exist but are not build evidence. Historical 2,735-test narratives exist; their original TRX/build reports were not located.

### Release identity

Local tags: `v1.0.1` → `dd1f3af9fabd23f890b5421bb3bddca54cbdab45`; `v1.0.2` → `27cf7a7cd24964bcc7468c955166cdda50d95f3e`.

Public GitHub releases API was retrieved during this session (`github-releases.json`):

- `v1.0.2`: published `2026-09-01T14:34:46Z`, non-draft, non-prerelease, zero attached assets.
- `v1.0.1`: published `2026-06-26T13:48:03Z`, non-draft, non-prerelease, zero attached assets.

This metadata does **not** prove NuGet identity, signatures, missing/present private evidence or publication history outside that response. Source `1.0.0` remains unreconciled. No next version was selected.

## F01–F17 disposition at this commit

“Source-confirmed” means the pattern described in the original finding exists; it does not establish exploitability, severity or exposure in a released package. No attack payloads or external exploit reproductions were used. Sensitive validation belongs in the private process in `.github/SECURITY.md`.

| Finding | Current disposition | Follow-up |
|---|---|---|
| F01 | Source-confirmed: Chart text-measurement HTML paths remain | R04 negative DOM/network regressions |
| F02 | Source-confirmed: uploader preload metadata HTML path remains; Razor path is separate | R05 route-specific regressions |
| F03 | Source-confirmed: supplied default HTTP headers forwarded to browser options | R03/R06 approved policy; host-dependent impact |
| F04 | Source-confirmed: aggregate buffering budget absent, unlimited fallback remains | R07 resource/cancellation tests |
| F05 | Source-confirmed: Chart cleanup ordering and Tooltip listener identity concerns | R08 lifetime regressions |
| F06 | Partially already fixed: axis fields are instance-owned; shared measurement/options state remains | R09 circuit isolation and bounds |
| F07 | Source-confirmed leads: observable ownership, discarded updates, callback bounds | R10 reachable-family investigation; no SSRF conclusion |
| F08 | Recorded compiler blockers already fixed; main suite compiled/discovered/executed | R02 still needs build-state, warning and project-target review |
| F09 | Source-confirmed: vacuous, conditional, swallowed and constant-count assertions remain | R11 deliberate-regression validation |
| F10 | Source-confirmed: documented matrix exceeds executable/configured scope | R13–R15; this run proves only the declared slices |
| F11 | Source-confirmed: CI skip and documentation-only SBOM tests; package inclusion unknown | R16/R17 artifact tests |
| F12 | Source-confirmed: promised API/provenance gates not located | R03/R16/R18 |
| F13 | Source-confirmed: shipped scripts excluded from ESLint/XSS heuristic; CodeQL retained | R12 source-to-artifact coverage |
| F14 | Source-confirmed, **not corrected in this checkout**: WCAG level/pointer-alternative issues | R01/R21/R22; no ACR approval |
| F15 | Source-confirmed: Chart onboarding APIs/assets/package guidance diverge | R20 compile/run fixtures; `RefreshAsync(bool)` does exist |
| F16 | Source-confirmed timing flaw; no retained measured benchmark baseline located | R23; smoke duration is not Chart performance evidence |
| F17 | Source-confirmed, **not corrected in this checkout**: documented enforcement type not located | R01/R19 host-contract review |

## Exact actions and reproducibility

All shell actions below ran from the clean worktree root unless stated otherwise. No test result is inferred from the commands' existence.

1. In the original checkout, recorded `git branch --show-current`, `git rev-parse HEAD`, `git status --short`, `git tag --list`, OS/tool versions and workloads. After user approval: `git worktree add --detach <temporary-root>/source 19227619082433fa3b86e15d4e7aee625850aed9`.
2. `npm ci`; `npx --no-install gulp blazor-toolkit-themes`; `npx --no-install playwright test --list --reporter=list`.
3. C# Dev Tools Build action targeting `src/Syncfusion.Blazor.Toolkit.csproj`: success (Debug outputs). Test action targeting the main bUnit `.csproj`, project scope: discovery error for temporary project. Build action targeting `samples/Blazor.Toolkit.Samples.slnx`: failed; later library Build action also failed. These concise tool responses are described here; they are not raw archived MSBuild logs.
4. `TZ=UTC LANG=en_US.UTF-8 dotnet test tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj -c Debug -f net8.0 --list-tests`.
5. `TZ=UTC LANG=en_US.UTF-8 dotnet test tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj -c Debug -f net8.0 --no-build --no-restore --logger 'trx;LogFileName=bunit-net8.trx' --results-directory <evidence>/bunit`.
6. With `PLAYWRIGHT_JSON_OUTPUT_FILE=<evidence>/playwright-smoke.json`: `npx --no-install playwright test tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/Charts/Series/LineCharts/multicolored-line-basic-rendering.spec.ts --project=chromium --grep 'Basic Rendering' --workers=1 --retries=0 --trace=retain-on-failure --reporter=line,json --output=<evidence>/playwright-smoke-results`.
7. `dotnet build samples/Blazor.Toolkit.Samples.slnx -c Debug --verbosity minimal`; `dotnet clean src/Syncfusion.Blazor.Toolkit.csproj -c Debug --verbosity quiet`; `dotnet build src/Syncfusion.Blazor.Toolkit.csproj -c Debug --no-incremental -m:1 --verbosity minimal`; `dotnet build samples/Blazor.Toolkit.Samples.slnx -c Debug -m:1 --verbosity minimal`.
8. Successful diagnostic builds: `dotnet build src/Syncfusion.Blazor.Toolkit.csproj -c Debug --no-incremental --disable-build-servers -p:UseSharedCompilation=false -m:1 --verbosity minimal`, then the same options targeting `samples/Blazor.Toolkit.Samples.slnx`.
9. Repeated full bUnit command from step 5 with `--no-restore --disable-build-servers -p:UseSharedCompilation=false` instead of `--no-build --no-restore`, and `LogFileName=bunit-net8-repeat.trx`.
10. `npm run lint:security`; `npx --no-install gulp security-xss-scan`; `npm audit --json`; `dotnet list src/Syncfusion.Blazor.Toolkit.csproj package --vulnerable --include-transitive`; `shasum -a 256 artifacts/sbom/sbom.cdx.json` compared to its committed `.sha256`.
11. Public metadata: `curl -fsSL --max-time 30 'https://api.github.com/repos/syncfusion/blazor-toolkit/releases?per_page=100'`. Browser version obtained by launching Playwright Chromium and reading `browser.version()`.

## Retained evidence and next task

[Download the local evidence archive](baseline-logs.tar.gz), SHA-256:

`96111d4bceec13dbedde821e2e25391f8fbc19280ddfcfaa14f770760b1e807c`

The archive contains build successes **and failures**, discovery logs, both TRX reports, browser JSON/logs, advisory JSON, lint/heuristic logs, environment record and public release response. Raw logs contain local filesystem paths and synthetic test names; no HAR or authentication data was intentionally collected. A limited common-secret-pattern scan found no matches; this is not a comprehensive secrets audit. Review access/retention before external distribution. The archive and report are uncommitted local evidence, not immutable remote CI evidence.

Handoff validation checked local document links, archive SHA-256 and safe member paths, both archived TRX counters, browser JSON counters, exactly one checked R-task (R00), all nine final gates still open, and an empty production/configuration diff. Edited Markdown diagnostics reported no errors. A separate automated review attempt was unavailable because of a provider-routing error; this record has executor self-review only, not independent approval.

**Next eligible task: R01**, reconcile current public assurances with measured evidence and approved Chart-first/no-Grid scope. Then R02 addresses runnable-test/build reproducibility and warnings. No automatic promotion of the historical documentation dispositions is allowed. Baseline completion does not mark R02 or any security/platform/package gate complete.

Remaining evidence includes full meaningful browser execution, supported runtime/mode/browser matrix, clean package consumers, API compatibility, trimming/WASM AOT, independent security review, verified SBOM/signatures/provenance, manual accessibility/AT, measured performance, coexistence and named maintenance owners. The temporary worktree and raw logs are retained for investigation; it is not a release candidate. Re-run affected checks after source/dependency/environment changes.
