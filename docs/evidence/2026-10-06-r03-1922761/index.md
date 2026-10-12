# R03 — Enforced released API baseline and reviewed major migration

**Date:** 2026-10-06. **Base:** `address-warnings` / `19227619082433fa3b86e15d4e7aee625850aed9` plus the documented uncommitted changes. [Plan](../../ENTERPRISE-READINESS-PLAN.md#9-execution-log-and-restart-record) · [Migration policy](../../API-COMPATIBILITY.md) · [Exact approved symbols](../../API-BREAK-INVENTORY.md) · [Original unsuppressed comparison](../2026-10-06-r03a-1922761/index.md).

**R03 completed for the user-approved bounded source/API contract.** All final release-readiness gates remain open. This is not a release, signed candidate, remote CI result, universal binary compatibility claim or complete runtime/persistence assessment.

## Explicit approvals and boundaries

The requesting user approved the exact existing changes after grouped symbol review: 84 named Chart type internalizations, 99 collection signature rows, four dynamic-to-object parent metadata changes, two helper static conversions and the `DataAdaptor<T>` constraint. Both getter-only ownership removals (`UploadedFiles.Files`, protected `ChartData.CurrentPoints`) were separately approved. Together these produce **194 exact diagnostic/target pairs**, excluding CP0003 version regression.

The user authorized source `Version=2.0.0` and review of the **uncommitted version diff in place of R03a's version-commit prerequisite**. No commit/tag/publish permission was inferred. Release requirements still need an approved clean candidate commit. The user delegated bounded upgrade-contract approach; matching-version assemblies/assets, representative supported fixtures and explicit unsupported arbitrary old state/internal extension paths are documented rather than promising seamless upgrades.

The user's planned CSS-isolation migration is recorded but **not implemented, included in these exceptions or claimed verified**. Asset paths/selectors/scoped CSS, package consumers and coexistence need separate review before changing setup instructions.

## Implementation

- `tools/api-compat/baseline.json`: exact NuGet 1.0.2 URL/SHA-256, APICompat 10.0.401, three required TFMs, candidate version.
- `approved-breaks.xml`: 194 user-approved exact symbol/rule pairs; no wildcard rule/namespace or version suppression. `docs/API-BREAK-INVENTORY.md` lists every target.
- `gate.py`: hash/signature/nuspec validation, pinned tool check, all-TFM failure-on-missing inputs, released comparison, candidate assembly identity/TFM validation, complete required manifest schema and JSON-type-sensitive snapshot checking. No automatic baseline-update option.
- `reviewed-*.json.gz`: three compact, deterministic reviewed manifests. Each records **10,256 surface rows, 1,113 parameter declarations, 86 JS-invokable signatures, 23 wire observations and eight generated-metadata fixtures**. Five wire observations explicitly record unsupported deserialization, not successful round trips.
- `ApiContractProbe`: separate process for each assembly; public/protected—including protected nested—surface, interfaces, generic type/method constraints and arity, fields/defaults/contract/nullable metadata, Razor and callback metadata. Invariant culture and narrowly omitted compiler/debug attributes make Debug/Release snapshots comparable. Candidate mode requires generated contexts/type info; only explicit historical mode permits reflection fallback.
- Razor fixture compiles seven one-way collection parameters and actual NumericTextBox value/Dialog visibility two-way binding with warnings-as-errors. It is not a runtime UI test.
- CI `api-compatibility` job builds fresh inputs with SDK 10.0.401 and is a dependency of pack and the failing summary. No signing/publishing added. Remote execution/admin required-check settings remain unverified; 14-day CI retention is not permanent release evidence.
- Source edits: authorized `Version=2.0.0`; explicit released namespace `Syncfusion.Blazor.Toolkit.Data` on `SfDataManager`; accurate IList Dialog XML example. No other API changes were made to quiet diagnostics.

## Unexpected break and negative controls

The fresh build unexpectedly emitted `SfDataManager` under `Syncfusion.Blazor.Toolkit`, unlike the released `Syncfusion.Blazor.Toolkit.Data` identity. The released gate failed CP0001. A no-shared-compiler rebuild retained the wrong namespace. Adding an explicit Razor namespace fixed it; **no exception was added**. This demonstrates why old bin outputs and inferred namespace assumptions are insufficient.

Two deliberate mutations were made **only in the disposable worktree** and reverted:

1. `SfChart.RefreshLiveData` public → internal: build succeeded, gate exited **1**, APICompat reported new CP0002.
2. Already-excepted `SfDialog.GetButtonItems` IList → ICollection: APICompat passed its historical exception, but the current-surface guard exited **1** for unreviewed surface change. This proves historical exceptions cannot silently authorize later changes to the same member.

Additional controls: wrong baseline bytes, empty/subset/duplicate framework lists, missing/empty manifest categories, nested boolean→number JSON changes and missing public/protected-nested surface entries are tested. Passing an old assembly without generated contexts in **candidate mode** failed with a required-metadata error; explicit historical mode is separate.

## Executed verification

| Check | Result / limitation |
|---|---|
| Final API/contract gate, Release | **Pass net8/net9/net10** against pinned 1.0.2 plus exact approved exceptions and reviewed candidate manifests |
| Final same gate, Debug | **Pass all three** using the same configuration-normalized manifests |
| Library nonincremental builds | **0 warnings, 0 errors** in recorded Debug/Release runs; SDK 10.0.401, macOS arm64 |
| Python verifier tests | **9 passed**, including subcases; initial missing implementation and incomplete nested-type fixture failures retained |
| Full bUnit Release net8 | **2,738 passed, 0 failed, 0 skipped**; existing test warnings remain, not suppressed |
| Razor migration fixture | **0 warnings/errors**, TreatWarningsAsErrors enabled; first /tmp-versus-/private/tmp static-asset identity failure retained, canonical-path retry passed |
| Browser smoke | **2 passed, 0 failed/skipped/flaky**, Chromium, net10 Interactive Server Release, retries disabled: Chart title/series/point/legend and NumericTextBox keyboard step |
| Wire/metadata comparison | **86 callbacks unchanged**; seven collection parameter changes, ten nullable changes, six removed renderer parameter declarations; same three EditorRequired parameters |
| JSON fixtures | Supported observations match released baseline; actual HTTP POST retains zero/omits null and stringifies enum, PATCH omits defaults; no network requests sent |
| Mutation tests | Both actual compiled new-break cases failed; restored candidate passed |
| CI structure | YAML parsed; pack/summary require API job. No remote CI execution claimed |
| Review | Two read-only agent reviews; required fail-closed/context/schema/generic/protected-type/JSON-type findings addressed and rerun. Agent review is not security/release certification |

The probe exercises each TFM assembly on **net10**, not three runtime test hosts. The two browser cases do not cover all NumericTextBox unfocused/mouse/touch/hold ordering. Full dispatch, arbitrary persistence migrations, malformed-client handling, trimming/AOT, package consumers, CSS behavior and cross-platform execution remain their own gates. Matching script files do not imply universal application compatibility.

## Commands / retained reproduction

- Library builds via C# Dev Tools, and isolated terminal diagnostic/configuration builds: `dotnet build <worktree>/src/Syncfusion.Blazor.Toolkit.csproj -c Release --no-incremental -p:CycloneDxEnabled=false -v:minimal`; corresponding Debug build. Opting out of SBOM for API checks is explicit, not SBOM evidence.
- Probe: `dotnet build tools/ApiContractProbe/ApiContractProbe.csproj`; `dotnet <probe.dll> <assembly> [--historical]` (historical flag only for released comparison).
- Verifier: `python3 -m unittest discover -s tools/api-compat -p test_gate.py`.
- Gate: `python3 tools/api-compat/gate.py --assemblies <worktree>/src/bin/<Debug-or-Release> --probe <probe.dll> --package <exact-1.0.2.nupkg> --output <isolated-output>`; gate logs record pinned tool and package verification. Output names `gate-mutation` and `gate-shadowed-mutation` identify deliberate failures.
- Razor: `dotnet build <canonical-worktree>/tools/ApiContractProbe/Fixtures/RazorConsumer.csproj -c Release -p:CycloneDxEnabled=false -v:minimal`.
- bUnit: `TZ=UTC LANG=en_US.UTF-8 dotnet test <canonical-worktree>/tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj -c Release -f net8.0 -p:CycloneDxEnabled=false --logger 'trx;LogFileName=r03-full.trx' --results-directory <evidence>/full`.
- Browser: `npx --no-install playwright test 'Charts/Others/chart-basics.spec.ts' 'Inputs/NumericTextBox/min-max-step.spec.ts' --grep 'basic chart renders title, series and legend|arrow keys increment/decrement by step' --project=chromium --workers=1 --retries=0 --trace=retain-on-failure --reporter=line,json` with explicit report paths.

## Evidence and preservation

[Compatibility gate evidence archive](compatibility-gate-evidence.tar.gz), SHA-256:

`164cd8a19ece2a64733ad555246ed7386178f1915f5e74c4a6d5b80f795ae392`

Contains failed and successful probe/build/gate logs, contract observations, full-suite TRX, browser JSON, exact production/workflow patch and final hashes. New gate/probe sources and manifests are in the repository. The temporary `UNAPPROVED-api-delta.xml` inside the archive is historical input to the review, **not** the active approved file; only the checked-in `tools/api-compat/approved-breaks.xml` is used by the gate. Local investigation worktree: `/tmp/blazor-toolkit-r03b-UxGqWJ/source`; canonical macOS path starts `/private/tmp`.

Tested production files match the user checkout byte-for-byte. Deliberate mutations are absent. R00/R01/R02/R03a archives and original SBOM JSON/checksum bytes were preserved; generated SBOM mirrors occurred only in isolated worktrees. Incoming user documents and root `run-tests.sh` were preserved. No commit, tag, push, signing, publication or remote-settings change occurred. Evidence is local/uncommitted, with retention owner still unassigned.

**Next eligible task: R04 — safe Chart text measurement.** API exceptions are limited to this approved major migration; later public changes must receive their own decision. R18 retains the historical embedded-commit/tag discrepancy, and R16 must repeat compatibility against actual candidate package bytes. All nine final release gates remain open.
