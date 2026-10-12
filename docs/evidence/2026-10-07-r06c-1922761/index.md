# R06c — Retry, reset, parent updates and overlapping controls

**Owner/session date: 2026-10-07.** Base `address-warnings` / `19227619082433fa3b86e15d4e7aee625850aed9`, preserved approved R02–R05 changes, uncommitted test-only continuation. [Controlling plan](../../ENTERPRISE-READINESS-PLAN.md#9-execution-log-and-restart-record) · [Decisions](../../R06-DECISIONS.md) · [R06b history](../2026-10-07-r06b-1922761/index.md).

**Partial characterization, not an R06 fix or completed matrix.** Option **B** was selected at the owner's blocker checkpoint: retain endpoint retargeting as blocked/out of this phase by the missing child-module export. Option A's production repair was not selected. The two R06b endpoint failures remain evidence, not passes. Continue unblocked characterization only; no formal API checkpoint or release approval follows from this record.

## Scope

- Added ordinary Razor-parent mutation/replacement controls, without the frozen wrapper/direct parameter calls used in R06b.
- Extended the matrix with retry-from-stage byte digests, automatic retry exhaustion, clear/reselection and upload/removal overlap with explicit/null/empty removal headers and form markers.
- Extended the approved test-only endpoint to hold the first occurrence of selected chunk 0, 1 or 2. Its admission/body/deadline/opt-in constraints are unchanged. Tested later/final pause, repeated controls and resume before the held response completes.
- No library, public API, API baseline, default CI workflow or package dependency change. No production export repair, callback shim or forced internal upload status.

## Final results

| Check | Result and interpretation |
|---|---|
| Established observations, final reviewed files | **54 passed**, 27 unique cases repeated twice, no failures/skips/flaky results, retries disabled. Composition: 11 R06a + 11 established R06b + 5 new R06c (two retries, clear, two ordinary-parent cases). Explicitly excludes two endpoint probes and three overlap failures. |
| Completed-file removal overlapping active upload | **3 failed**: explicit, null and empty removal headers. Transport/header/form assertions reached; all subsequently report `RemoveFileDataAsync` through browser error collection. Not counted as passes. |
| Later chunk, response settled while paused | **1 observed pass**: receipts `[0,1,1,2]` after resume; one completion. |
| Final chunk, response settled while paused | **1 observed pass**: response completes the file despite pause; receipts `[0,1,2]`; later resume is a no-op. |
| Later chunk, resume while original response pending | **1 observed pass**: receipts `[0,1,1,2]`; original response release causes a second file-success callback. Passing means the duplicate was reproduced, not accepted. |
| Final chunk, resume while original response pending | **1 failed**: exact server receipts `[0,1,2]`, completed 3, active 0, no endpoint timeout; expected file success never appears. No browser `pageerror` was captured. This does not exclude interop/server errors. |
| Original first-chunk pause control | **1 observed pass** on the extended endpoint; receipts `[0,0,1,2]`. |
| Endpoint active/disabled controls | **2 separate passes**, fresh host each. Bad origin/size/budget/control-header and opt-in-disabled behavior retained. |
| Separate managed characterization | **8 passed**, 0 failed/skipped, net8 Release. Not the full main bUnit suite. |
| Strict types | **Pass**, all nine harness TypeScript files, using retained installed tooling and temporary pinned Node declarations. |
| Source preservation | **511 checkout source files + 8 API-baseline files** byte-identical to tested snapshot and retained R05. Both isolated snapshots additionally contain the same 21 generated theme files absent from the checkout. |

The final standalone browser groups comprise 60 passing executions and four failures; this total deliberately excludes the two historically retained endpoint failures and two historical proposed-policy failures. It is not an all-green suite or complete-matrix count. Passing observations must be replaced by approved safe-behavior regressions before production implementation; never preserve a defect to keep these tests green.

Full main bUnit (historical 2,738 passes), fresh all-TFM library/API gates, R04/R05 browser matrix, released-package consumers, other browsers/render modes, security/dependency scans and SBOM validation were **not rerun**. Playwright built the required net10 test host; no new zero-warning/library-matrix claim. Editor diagnostics in the checkout still report missing Playwright/Node declarations; isolated strict checking does not repair repository-native tooling.

## Observations, not desired contracts

### Parent updates and reset

Ordinary parent render reproduces delayed default-header propagation for both mutation and replacement. A subsequent CSS parameter change exports the new defaults; selected file ID/name/raw file remain intact, and the next actual request contains the expected dummy default. This supplements rather than rewrites R06b's controlled direct-parameter observations.

After completed upload, `ClearAllAsync` empties files/input but leaves shared headers, form data and the true credential flag in the mounted instance. Reselecting and supplying empty headers/false credentials produces a request with no probe headers but still reports `WithCredentials=true`. This is the XHR flag only, not proof of cookie transmission or an approved cookie policy.

### Retry byte alignment and budget

For three distinct 1,024-byte A/B/C slices, initial chunk 0 succeeds and chunk 1 fails. `RetryAsync(true)` then sends the **C bytes under chunk index 1**, followed by an **empty slice under index 2**. Exact sizes and SHA-256 digests are asserted; `BeforeUpload` runs only once. This is an observed index/byte mismatch, not a restart guarantee.

With retry budget 2 and repeated chunk-1 failures, attempts are `[0,1,1,1]`, the three index-1 attempts contain identical B bytes, three chunk-failure callbacks occur and one terminal failure is reported. No success is observed.

### Overlap and removal failures

A's first chunk is held; B completes; removal of B is held; A is released. With explicit removal headers, A's later chunks inherit B's removal header **and form marker**. With null or empty removal headers, the later chunks have no explicit probe header but still inherit removal form data. These assertions and exact later indices `[1,2]` pass before the removal response is released.

All three probes then retain a generic `RemoveFileDataAsync` exception; completion is not a pass. The exact managed root cause was not established. No broad catch, expected-failure annotation, retry, assertion waiver or product repair was introduced.

### Pause/resume failure boundary

Each variant uses a fresh host, genuine upload progress and public pause/resume methods. Transparent observation wrappers call original methods; no product metadata/status is assigned. Settled later resume retransmits the paused index; final response overrides pause; pending later resume can report two successes when the original response later completes.

Pending final resume fails waiting for success even though all three server handlers complete. Source inspection suggests a null-event path: `resumeFiles` supplies null, final-chunk `resumeUpload` forwards it to `chunkUploadComplete`, which dereferences `e.target`. **This is a source-supported explanation, not a runtime-confirmed exception stack/root cause.** Empty browser page-error collection does not prove an error-free circuit. Endpoint handler completion does not establish client completion, storage commit or late-write prevention.

## Harness review and development history

- Initial ordinary-parent assertions assumed an `.e-upload-success` class; the actual successful status text and status code were visible. Corrected the fixture assertion to measured semantics; initial two failures remain raw harness-development history, not product defects.
- Final read-only review identified absence assertions could hide non-dummy values filtered from evidence. The collector now records a value-free error for non-dummy probe headers/form markers before redacting them. Exact endpoint receipts/completion/timeout are asserted in the pending-final branch. Reviewed files were rerun: 54 pass, three overlap failures, three pause passes and one pause failure unchanged.
- Always-attached sanitized matrix/pause state preserves failures; teardown releases gates and waits for local activity to drain. This is test hygiene, not proof of production cleanup/lifetime behavior.
- An initial preservation comparison included generated theme output absent from the checkout and failed. Investigation identified exactly 21 theme artifacts, identical across isolated R05/R06c snapshots; the 511 source files and eight API files match. No source mismatch was waived.
- One final review attempt failed due to service error; a subsequent read-only review succeeded. Agent review is not independent maintainer/security sign-off.
- Raw server diagnostics contain a local connection identifier. They remain restricted locally and are **excluded** from the curated archive. No raw headers, cookies or full upload bodies are intentionally retained in curated request records.

## Reproduction environment and selection

Canonical isolated worktree `/private/tmp/blazor-toolkit-r06c-eJ6cTz/source`, preserved dirty-tree overlay, not a new clean baseline. macOS arm64; SDK 10.0.401; net10 Interactive Server Release; Playwright 1.63.0 / Chromium 153.0.8010.12; TypeScript 6.0.3; temporary `@types/node@22.10.2`. Installed tooling reused from retained R04; declarations from R05. Source builds opt out of CycloneDX only to preserve source-test isolation, not to verify SBOM.

- Established: `npx --no-install playwright test --config tools/transport-characterization/playwright.config.ts uploader.characterization.ts matrix.characterization.ts parent-updates.characterization.ts --project observations --grep-invert 'endpoint-|completed-file removal' --repeat-each=2 --reporter=line,json`.
- Retained overlap failures: same config, `matrix.characterization.ts --project observations --grep 'completed-file removal'`, separate report/output, exit 1.
- Pause variants: `--config tools/transport-characterization/pause-variants.config.ts`; separate fresh process for each combination of `R06_HOLD_CHUNK=1|2` and `R06_RESUME_PENDING=0|1`. No same-host repeats.
- Original first pause: base config with `pause.characterization.ts --project observations`, once per host.
- Guard modes: `--config tools/transport-characterization/guard.config.ts`, separately normal and `R06_VERIFY_DISABLED=1`.
- Managed: `dotnet test tools/transport-characterization/Managed/TransportCharacterization.csproj -c Release -p:CycloneDxEnabled=false` with explicit TRX/results paths; isolated-project discovery fallback as previously documented.
- Types: `tsc --noEmit --strict --types node --typeRoots <retained-temporary-node-types> --target ES2022 --moduleResolution node16 --module node16 --skipLibCheck tools/transport-characterization/*.ts`.

Every browser run uses distinct private JSON/log/output paths, one worker, no retries and a fresh host. Raw timestamps remain UTC **2026-10-06**, including reviewed final runs 21:21–21:22; the owner's session date is not substituted into emitted timestamps.

## Retained evidence and identities

Private local reference **R06C-LOCAL-20261007**, `characterization-retry-overlap-pause.tar.gz`, SHA-256:

`735bca960d5e2681d0a9c5615695d7d564a9657c8bc366f0b4f352c9b5bbc018`

Current-user Code Studio application-support `readiness-evidence/2026-10-07-r06c-1922761/`, directory 0700/archive 0600. Contains 44 curated files: exact final reviewed results (including four failures), guards/original pause/managed/type records, preservation hashes and tested harness/host sources. The archived README precedes this session's documentation update; runtime fixture hashes below are final. Raw development history remains under the restricted R06c evidence directory. Curated text and decoded JSON attachments lack tested antiforgery/connection-identifier markers; this is **not** comprehensive secret certification or an approved organizational retention store.

| Tested file | SHA-256 |
|---|---|
| `matrix.characterization.ts` | `2d998aae71513109b22907809f2a01f6ef30f503d67bf00c9d8e3a1669973bff` |
| `parent-updates.characterization.ts` | `d0994a3bdccb1cd7fa3c8a04048be60067bac04591295b9dc78972078f219729` |
| `pause-variants.characterization.ts` | `be78f5fcae0c15678a23b54f357d96280c8b41eb82a0a1d592df6394139b34ba` |
| `pause-variants.config.ts` | `870e5a23503f7f74214f7b58efe0e38d501df1288e81c741cbf842323debfcf8` |
| `TransportProbe.razor` | `84943643af218b8d3af64d30b0aba2c319c7509d33843b8292e314ad4bed5498` |
| `TransportMatrix.razor` | `82cc8be4478a0651870ee6fafc0a9ab440bf933be97fc7ad2c12260b5f257a6b` |
| `TransportParentUpdates.razor` | `d338081c3cb82ca1f26ee5975eadc0a6ff397375638d5178426fa2f504615bb9` |
| `R06LoopbackEndpoint.cs` | `ff378f9c91744557c004206499dae1d4705e812507242f1556b5cf2d563189a1` |

Earlier archives, original SBOM bytes/checksum mismatch and incoming user changes are preserved. No commit, push, tag, signing, publishing, external report or remote-settings change.

## Remaining work and held decisions

Option B dispositions endpoint retargeting and dependent endpoint-change retry/resume/removal/cleanup cases as blocked/out of this phase; it does not close R06 or prove those paths safe. Preserve both R06b failures. The three overlap and pending-final failures require retained follow-up/root-cause characterization, not conversion into green observations by ignoring exceptions.

Remaining unblocked priorities 1–4 include broader callback rejection/veto schedules, additional interleavings/form-state reset and exact event/outcome accounting. Broader cancellation/cleanup, lifecycle/disconnect/remount, automatic/sequential/template/hidden-list modes and priority 5–8 coverage remain open. Endpoint negative peer/Host/timeout/parser-shape coverage is not exhaustive. This bounded continuation does **not** fulfill an exhaustive-matrix claim or close the owner's continuation request.

Exact public/wire/error/timeout/migration contract, host/storage guarantees, XHR redirect/cookie policy, named private security/disclosure/retention and release approvals remain held. No formal API checkpoint until the agreed matrix and dispositions are finished. R04/R05 holds, R06 checkbox and all nine final release gates remain open; no automatic R07 advancement.
