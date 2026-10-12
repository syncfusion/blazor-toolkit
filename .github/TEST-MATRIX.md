# Test matrix and evidence status

Configuration inventory from **2026-10-03**, reconciled with retained R00 evidence
on **2026-10-06**. A configured job is not a passing run, and a framework targeted
by the library is not necessarily a tested runtime. No remote CI pass is asserted
here. For local prerequisites and commands, see [DEVELOPMENT.md](DEVELOPMENT.md).

## Configured checks

Sources: [CI workflow](workflows/ci.yml), [CodeQL workflow](workflows/codeql.yml),
[Playwright config](../playwright.config.ts), and the
[bUnit project](../tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj).

| Check | Configured scope | Limits / interpretation |
|---|---|---|
| Library build | `net8.0;net9.0;net10.0` in the source project | Building all targets needs SDK 10. A build is not runtime testing. |
| bUnit | Jobs labelled SDK `8.0.x`, `9.0.x`, `10.0.x`; test project targets only `net8.0` | This is an **SDK-labelled job matrix**, not tests running on three runtimes. The selected SDK and installed .NET 8 runtime must be checked in logs. |
| Playwright | One job; only `chromium` / Desktop Chrome; one `net10.0` Interactive Server sample host at port 5000 | No browser/runtime/render-mode matrix. Trace collection is off; one retry and four workers are configured. Installing three SDKs in the job does not create three runs. |
| NuGet vulnerability scan | One job scanning the library, including transitive dependencies | Restore audit settings plus report-text detection; not a per-runtime test, whole-repository audit, or zero-vulnerability guarantee. |
| ESLint security | `npm run lint:security`, Node 22 in CI | Excludes `src/wwwroot/scripts/**`, minified assets, themes, samples, tests and gulp tooling. It does not audit the shipped script surface. |
| XSS heuristic | `npx gulp security-xss-scan` over `src/Components`, `src/Base`, `src/Data` | Excludes `wwwroot`, samples and tests. Pattern matches are not complete XSS detection or bundled-script coverage. |
| Unsigned pack | SDK-labelled `8.0.x`, `9.0.x`, `10.0.x` jobs, dependent on all five CI checks above | Commands do not select one TFM; do not describe artifacts as one package per TFM. These are review artifacts, not publication candidates. |
| CodeQL | C#, JavaScript/TypeScript and Actions; push/PR to `main`, weekly Monday schedule and manual trigger | Separate workflow, not a dependency of the CI pack job. JavaScript analysis is present; actual analyzed files/findings require run evidence. |

The CI workflow runs on push/PR to `main` and manual dispatch. Its summary job
checks declared job outcomes and attempts a PR comment. Repository branch
protection and actual successful execution are not established by that YAML.

## Measured source baseline — 2026-10-06

[R00 report and raw evidence](../docs/evidence/2026-10-06-r00-1922761/index.md)
identify commit `19227619082433fa3b86e15d4e7aee625850aed9`, a clean-start isolated
worktree, macOS arm64, SDK 10.0.401, Node 26.10.0 and npm 12.1.0. These are local
source checks, not current remote CI, package or full support-matrix results.
R01 links this existing evidence; it does not rerun those checks.

| Executed check | Recorded result | Scope / unresolved limitation |
|---|---|---|
| Library Debug net8/net9/net10 build | 0 warnings/errors with `--disable-build-servers -p:UseSharedCompilation=false` | Default builds failed; clean/serial alone did not resolve cascading Razor diagnostics. Exact root cause remains open under R02. |
| bUnit Debug net8.0 | 2,735 discovered; 2,735 passed twice, 0 failed/skipped | 3,537 build-warning occurrences; no net9/net10 test-runtime evidence. |
| Gallery Debug build | 232 warnings, 0 errors with the same compiler workaround | Not a published WASM, trim or AOT run. |
| Playwright discovery | 2,421 cases in 325 files | Nonzero discovery, not execution. |
| Chart basic-rendering smoke | Four passed, 0 failed/skipped/flaky; retries disabled | net10 Server Release, Chromium 153.0.8010.12 / Playwright 1.63.0; 2,417 remaining cases unrun. No interaction, accessibility or performance gate closed. |
| npm audit | 11 high affected-package entries; exit 1 | Tooling dependency paths require R12 triage; not 11 independently proven product exploits. |
| Library NuGet advisory query | No vulnerable packages reported by current feed | Library/transitives only, not all projects or shipped JS. |
| Existing ESLint / XSS heuristic | Both passed | Their shipped-script exclusions still apply. |
| Top-level checked-in SBOM checksum | Mismatch reproduced | Snapshot unusable as release evidence; R17 remains open. |

Both failures and successful diagnostic runs are retained. No release-readiness,
coverage-percentage, warning-waiver or independent security approval follows.

### R02 harness follow-up — 2026-10-06

[R02 evidence](../docs/evidence/2026-10-06-r02-1922761/index.md) applies to the same
base commit plus the recorded test-only patch, in a fresh isolated worktree:

- Main bUnit discovery: **2,738**; Debug and Release each **2,738 passed, 0 failed,
  0 skipped**. The increase is exactly three new harness cases, not new component
  feature coverage. Existing 3,537 test-build warning occurrences remain.
- Standalone ButtonGroup: **36 discovered/passed**, no failures/skips (33 existing
  cases plus the same three harness checks). These overlap the main suite.
- Both restored graphs resolve `Syncfusion.Blazor.Toolkit/1.0.0` as a **project**;
  the standalone wildcard commercial reference and test signing were removed.
  Parent compile items retain ButtonGroup/Base tests and exclude nested bin/obj C#.
- Default clean/nonincremental library builds: **0 warnings/errors**; gallery:
  **232 warnings, 0 errors**. R00's compiler failure did not reproduce; no compiler
  switch or suppression was committed and no root-cause fix is claimed.
- Existing `chart-basics.spec.ts` test `basic chart renders title, series and legend`:
  **one passed**, net10 Server Release/Chromium, no retries. An initially over-anchored
  CLI filter discovered zero cases and exited 1; that failed attempt was retained,
  not counted as a pass. Browser source/configuration was unchanged.

This closes runnable-harness verification, not R11 test-quality remediation,
the full browser matrix, warning clearance, package validation or release readiness.

## Historical local narratives — 2026-10-03

The [dated development notes](DEVELOPMENT.md#known-analyzer--trim--aot-findings)
retain the warning-cleanup and compatibility decisions. Original raw artifacts
for these older narratives were not located by R00; do not substitute the newer
runs for missing historical proof:

- **2026-10-03, final approved constraint follow-up:** non-incremental library
  build across .NET 8/9/10: **0 warnings, 0 errors**.
- **bUnit on .NET 8:** **2,735 passed, 0 failed, 0 skipped**, including the
  preceding follow-up's 142 new regression cases. This does not prove .NET 9/10
  runtime coverage or universal component behavior coverage.
- Warning-level library style verification: **0 findings**; gallery and
  Playwright sample-host solution builds passed.
- Browser end-to-end and trimmed/AOT publication were not run in that follow-up.
  Test/sample warnings outside the library cleanup remain outside its claim.

The 2026-09-06 note citing 132 `Fact`/`Theory` cases is historical, not the
current executed-case count. Counts of source methods, theory rows, rendered
components and executed tests are different measures. No all-components or
coverage-percentage claim is made without the corresponding report.

## Evidence locations and retention

When produced, `ci.yml` uploads:

- `bunit-results-<SDK-label>`: TRX/HTML results and collected Cobertura output;
- `playwright-report`: HTML report and `test-results/`;
- `nuget-vuln-report`, plus `xss-scan-report` when findings produce that file;
- `unsigned-pkg-<SDK-label>`: unsigned `.nupkg` smoke artifacts.

The configured retention is **14 days**. Some uploads tolerate missing files;
no artifact can be assumed present or current from configuration alone. Record
the run URL, tested commit, selected SDK/runtime, command and report before citing
results. CodeQL findings belong to its own run/security-analysis output. No
`actions/attest-build-provenance` step is configured in these workflows.

## Gaps and maintainer follow-up

- SDK 8/9-labelled jobs build a multi-target project including .NET 10 without
  an explicit TFM restriction. There is no SDK pin; inspect SDK selection and
  .NET 8 runtime availability rather than assuming all labels work as intended.
- Pack jobs do not explicitly provision Node/npm/themes. The library has a
  conditional theme-build fallback; clean-runner behavior needs evidence.
- CodeQL's C# job uses npm caching and `npm ci`; keep the tracked lockfile aligned
  with `package.json`. Configuration alone does not establish a successful run.
- No tracked `tests/playwright-baselines/` or approved screenshot baseline was
  found. Snapshot-style assertions alone do not establish a passing visual
  regression baseline. No nightly Accessibility Insights workflow is configured;
  accessibility notes are not a certification or a current automated test run.
- No configured gallery WebAssembly smoke, Auto/static-SSR matrix, Firefox/WebKit
  run, trimmed publish, WebAssembly AOT publish or Native AOT publish gate.
- R03 now supplies a pinned 1.0.2/APICompat gate, exact approved major exceptions
  and current-contract manifests instead of `PublicAPI.Shipped.txt`; see
  [API policy](../docs/API-COMPATIBILITY.md) and [local R03 results](../docs/evidence/2026-10-06-r03-1922761/index.md).
  The new `api-compatibility` job blocks pack/summary; remote execution, candidate
  package checks and runtime/CSS upgrade behavior remain unverified.
- ESLint/XSS exclusions leave shipped scripts outside those two checks. CodeQL
  JavaScript analysis exists, but its presence alone does not close that gap.
- Non-CI conditional SBOM generation and manual signing/publication are not
  release guarantees; see [manual release preparation](DEVELOPMENT.md#manual-release-preparation).

These are evidence/configuration gaps, not changes to workflows or target settings.