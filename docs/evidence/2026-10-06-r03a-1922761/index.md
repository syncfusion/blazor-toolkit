# R03a — Released API comparison and major-release direction

**Date:** 2026-10-06. [Authoritative plan](../../ENTERPRISE-READINESS-PLAN.md#9-execution-log-and-restart-record) · [R02 source build/test evidence](../2026-10-06-r02-1922761/index.md).

**State: exploratory comparison and user decisions recorded; R03a and parent R03 remain incomplete.** Individual breaking-change dispositions, complete Razor/serialization review, authorized version commit and a demonstrated enforcing gate are still required. No incompatibility was suppressed or approved en masse.

## Decisions actually received

The requesting user explicitly selected:

1. **Baseline:** the downloaded NuGet `Syncfusion.Blazor.Toolkit` **1.0.2** package, rather than treating the differing local tag as an equivalent artifact.
2. **Compatibility direction:** plan an explicit major release instead of restoring all 1.x contracts by default.
3. **Proposed version:** **2.0.0 for planning only**. Source `<Version>` stays **1.0.0**. This is not authorization to commit a version change, publish, or accept all 195 diagnostics.

No named release/security owner, individual exception approval, version-bearing commit or release authorization was supplied. The next work is a reviewed symbol-level migration/disposition inventory, not automatic suppression generation. Prior approval of the `notnull` source change did not establish release compatibility; the probes below now demonstrate its consumer effect.

## Baseline identity and verification

- Download URL: `https://api.nuget.org/v3-flatcontainer/syncfusion.blazor.toolkit/1.0.2/syncfusion.blazor.toolkit.1.0.2.nupkg`.
- NuGet version index returned 1.0.0, 1.0.1, 1.0.2 as stable versions during this session. This is a dated response, not a future “latest” guarantee.
- Package SHA-256: **`06934c512f9c42497fb37464f36d1b0cd7b987624bf2dab01c77b8a80865705d`**. Fresh download matches the pre-existing local NuGet cache byte-for-byte by hash.
- Nuspec ID/version: `Syncfusion.Blazor.Toolkit` / `1.0.2`; assemblies exist for net8.0, net9.0 and net10.0.
- `dotnet nuget verify <package> --all`: **exit 0**, reporting a Syncfusion Inc author signature and NuGet.org repository signature. This verifies this downloaded historical artifact under the workstation's trust/configuration, not any future candidate or per-DLL Authenticode status.
- Nuspec repository URL: `https://github.com/syncfusion/blazor-toolkit.git`; branch `refs/heads/1047900-NugetRelease`; embedded commit **`2e0b7905fcb6e66678478776a22ec6ea7449761f`**.
- Local `v1.0.2` points to **`27cf7a7cd24964bcc7468c955166cdda50d95f3e`**. The embedded commit was not a local object, and a public GitHub commit API lookup returned **HTTP 422 / “No commit found for SHA”**. This is unresolved source/artifact correspondence, not proof of tampering or nonexistence in every repository. Release-owner reconciliation remains required.

The historical signature observation does not close R18. No final candidate package was built, signed or published. Package bytes are retained inside the evidence archive for repeat comparison; retain their included license/notices if redistributing.

## Compared source and tools

- User checkout: `address-warnings`, **`19227619082433fa3b86e15d4e7aee625850aed9`**. Existing R01 documentation and R02 test-only edits preserved; no production source/configuration changes in this task.
- Right-hand assemblies: R02's freshly built Debug library outputs, net8/net9/net10, from the same SHA. The worktree has no library-source changes; assembly hashes are retained in `comparison-summary.txt`. These are **source assemblies**, not a prospective 2.0.0 package.
- Environment: macOS arm64, SDK **10.0.401**. Tool: **Microsoft.DotNet.ApiCompat.Tool 10.0.401**, installed into an isolated temporary tool path, not globally or into repository configuration.
- Comparison direction: released DLL on the left, source DLL on the right; public/protected checks with parameter-name rule enabled, no `--respect-internals`, no strict-mode addition requirement, no suppressions or `NoWarn` overrides.

## API results

All three framework comparisons **failed, exit 1**. Normalized diagnostic sets match across TFMs:

| Diagnostic | Per TFM | Interpretation |
|---|---:|---|
| CP0001 | 84 | Released public types missing from the current visible surface, including types now internal. |
| CP0002 | 107 | Released member signatures/accessors not matched, including collection changes and removed constructors. These are not necessarily 107 separate feature removals. |
| CP0003 | 1 | Source assembly version `1.0.0.0` is lower than released `1.0.2.0`. Record discrepancy; do not silently override the version. |
| CP0021 | 1 | New `notnull` constraint on `DataAdaptor<T>`. |
| CP0009 | 2 | Construction/subclassing differences for helper types with no current visible constructor. |
| **Total** | **195** | Diagnostic rows, not 195 independent runtime failures or 585 unique breaks across frameworks. |

Repeating with `--enable-rule-attributes-must-match` reported the same counts. That does not prove complete Razor/nullability/serialization compatibility. Comparing each released assembly with itself passed, **exit 0**, as a control. Existing failures and self-comparisons do not fulfill the later requirement to inject a **new deliberate break into the configured repository gate** and prove rejection.

The complete normalized list is in `evidence/api-diagnostics-normalized.txt` within the archive. No list of historical internalization decisions was substituted for DLL measurements; public symbols remain compatibility-relevant even when their namespace includes `Internal`.

## Representative consumer reproductions

An isolated net8 project references a selected DLL directly plus `Microsoft.AspNetCore.App`, with nullable enabled and `TreatWarningsAsErrors=true`. It is not a package consumer, Razor host or runtime test. Every case was compiled non-incrementally with SDK 10.0.401.

| Probe | Released 1.0.2 | Current source | Demonstrated migration / limitation |
|---|---|---|---|
| `ConsumerAdaptor<T> : DataAdaptor<T>` without constraint | Pass: 0 warnings/errors | Fail **CS8714** | Adding `where T : notnull` passes against current. This is a compiler nullability constraint, not a new CLR-enforced runtime constraint. |
| Return `dialog.GetButtonItems()` as `List<DialogButton>?` | Pass: 0 warnings/errors | Fail **CS0266** | Return `IList<DialogButton>?` passes. A forced cast or `ToList()` is not automatically equivalent: copying can change identity/mutation behavior. |
| `typeof(ChartRenderer)` from an external consumer | Pass: 0 warnings/errors | Fail **CS0122** | No direct replacement demonstrated. Supported extension/migration policy requires individual review. |

The five probe sources and project are retained in `consumer/` in the archive. These establish representative **source** failures and two possible source migrations; binary loading, full app behavior, all affected generics and every removed type remain unverified.

## Initial migration/disposition inventory

**All rows require individual review; major-release direction is not exception approval.**

| Family / representative symbol | Old → current | Required next decision / evidence |
|---|---|---|
| Dialog `GetButtonItems()` | `List<DialogButton>?` → `IList<DialogButton>?` | Document rebuild and binary implications; fix stale List-return XML example when API docs are updated. Consumer migration above is measured. |
| Chart `AddSeriesAsync` and collection properties/events | `List<T>` signatures → `IList<T>` or other current interfaces | Inventory exact overload/accessor deltas, callback/Razor binding impact and mutation expectations; do not equate assignable arguments with binary compatibility. |
| Data query/operation/request models and uploader event models | Numerous concrete collection signatures changed | Review all normalized member rows and serialization/binding consumers. Existing tests compiled against current API are not old-consumer proof. |
| Chart renderers / formerly public DTOs | 84 visible-type diagnostics, including `ChartRenderer`, `DomRect`, renderer/animation types | Review each released symbol, supported alternative, migration or explicit no-replacement policy. Avoid publishing “39 removed” from an unverified historical inventory. |
| `DataAdaptor<T>` | No `notnull` → `notnull` | Propagate constraints in generic subclasses; assess warnings-as-errors and nullable consumers. Probe confirms CS8714; CLR signature/runtime constraint behavior is distinct. |
| Protected parent accessors | Released `dynamic?` signatures not matched by current declarations | Review derived consumers, dynamic metadata and accessibility; do not assume every diagnostic represents a different CLR return type. |
| `ChartHelper` / `DataVizCommonHelper` | Visible construction/subclassing surface reduced | Decide supported usage and migration; no blanket namespace exemption. |
| Assembly/package identity | `1.0.2.0` released vs `1.0.0.0` current; proposed 2.0.0 | Separate authorized version-bearing change and candidate identity reconciliation required. |

Uncovered or incomplete contracts include `[Parameter]`/`[EditorRequired]` semantics, generated Razor binding behavior, all nullable metadata changes, persisted JSON, enum representations, callback argument order and old-binary runtime behavior. Do not close R03a/E02 before their scoped review and any required approvals.

## Shipped JavaScript correspondence

All **21** packaged `staticwebassets/scripts/*.js` files had different raw hashes from same-name source files. After explicitly normalizing CRLF to LF, **20 were identical**. Only `numerictextbox.js` retained a content difference: spinner mousedown now prevents default before the focus check and focuses the input when previously unfocused. Its normalized diff is retained.

Thus raw byte differences alone are not evidence of 21 behavior changes. Conversely, matching JS content does not prove that managed JSON payloads, JS-invokable methods or application state contracts remain compatible. The NumericTextBox behavior change was not browser-tested here. No future mixed-version asset compatibility is promised.

## Commands and exit status scope

Temporary root was `/tmp/blazor-toolkit-r03-0UVoSG`; placeholders below describe retained inputs, not repository configuration changes.

- Fetch NuGet flat-container version index, explicit 1.0.2 package and registration record with HTTPS `curl -fsSL`; package SHA-256 computed with `shasum -a 256`. Public embedded-commit lookup retained response and HTTP 422.
- `dotnet nuget verify <downloaded-1.0.2.nupkg> --all` → **0**. The separate tool-install log's “Skipping NuGet package signature verification” line concerns tool installation, not this retained package-verification command.
- `dotnet tool install Microsoft.DotNet.ApiCompat.Tool --version 10.0.401 --tool-path <temporary>/tools` → **0**. Tool version/help and install output retained.
- For each TFM: `apicompat -l <baseline>/lib/<tfm>/Syncfusion.Blazor.Toolkit.dll -r <R02>/src/bin/Debug/<tfm>/Syncfusion.Blazor.Toolkit.dll --enable-rule-cannot-change-parameter-name` → **1**; repeated with `--enable-rule-attributes-must-match` → **1**; baseline-versus-itself → **0**.
- For each `Generic`, `Collections`, `Visibility` case and baseline/current assembly: `dotnet build <consumer>/CompatibilityProbe.csproj -p:ToolkitAssembly=<assembly> -p:ProbeCase=<case> --no-incremental -v:minimal`. Baseline all **0**, current all **1**. `GenericMigrated` and `CollectionsMigrated` against current both **0**.
- Python ZIP/XML/JSON inspection and SHA-256 checks retained exact package/DLL identities; script comparison normalized only CRLF/LF. Production/configuration diffs remained empty.

No main suite, browser suite, dependency scan, trim/AOT publish or package-content conformance gate was rerun. Consumer probe builds are compilation experiments, not the complete R16/R20 consumer contract.

## Evidence and review

[API comparison evidence archive](api-comparison-evidence.tar.gz), SHA-256:

`a20557fdc5af9bbaf408d1a80f481a5a1c1143d1773f7b7d90a241464957be22`

Contains downloaded historical package bytes, public metadata/signature log, all comparison/control logs, normalized diagnostics, assembly hashes, script comparisons, consumer sources/logs and environment. Source-side DLLs are not duplicated in the bundle; they are identified by SHA-256 and reproducible source/build inputs, with the retained R02 worktree still available. No claim of independent deterministic reproduction is made.

A read-only agent review supported the bounded interpretation and identified overbroad changelog wording about unchanged collection/CLR signatures; that wording is corrected without modifying production API. It did not approve individual breaks or releases. A common-secret-pattern scan found no matches, not a comprehensive secrets audit. Raw logs contain workstation paths and public certificate details. Evidence is local/uncommitted; retention owner and independent release/API reviewer remain unassigned.

## Handoff and stop condition

**R03a remains open:** baseline and proposed version direction are approved, but full symbol/contract disposition and authorized version commit are not. **R03 remains open:** no repository/API enforcement, suppression baseline or intentional-break gate test was added. Next session should review the retained diagnostics into explicit migration decisions, complete non-CLR/consumer checks and obtain the required owner approvals before a narrowly scoped enforcement change. Do not infer that proposing 2.0.0 authorizes every breaking change or source version update.

Existing user changes preserved. No production/public API, version, build workflow, SBOM bytes or package metadata changed. No commit, push, tag, signing, publication or remote-setting operation occurred.
