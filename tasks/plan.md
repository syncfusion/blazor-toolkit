# Proposed implementation sequence — R06 uploader transport

**2026-10-06 — CHARACTERIZATION AUTHORIZED; EXACT PRODUCTION CONTRACT PENDING.** The user requested broader review, then delegated recommended decisions and authorized the next step. This is an execution aid, not a second backlog. [Controlling plan](../docs/ENTERPRISE-READINESS-PLAN.md#9-execution-log-and-restart-record) · [Design](../docs/R06-UPLOADER-TRANSPORT-DESIGN.md) · [Decision reasons/owner approvals](../docs/R06-DECISIONS.md) · [Checklist](todo.md).

The [first bounded R06a pass](../docs/evidence/2026-10-06-r06a-1922761/index.md) executed: eight managed tests, 22 current-behavior browser observations, two failing proposed safety controls. Tools live in opt-in `tools/transport-characterization/`, not normal CI; test-host fixture is environment-gated. Full characterization and exact contract review remain. Production still has the reproduced forwarding/timing behavior. R04/R05 holds remain; do not automatically advance to R07.

## Owner continuation — 2026-10-07

Owner approved priorities 1–4 characterization in order and a bounded opt-in in-process/loopback dummy endpoint. No production changes/commit/push; no host/storage guarantees; XHR policy unapproved; exact contract/migration held until matrix completion and formal checkpoint. [R06b](../docs/evidence/2026-10-07-r06b-1922761/index.md) records 44 repeated established observations, one live pause and two guard-mode passes, plus **two retained endpoint failures**. This is partial progress, not a complete characterization gate. Do not repair the child export or rebaseline under this authorization.

**Blocker disposition: B selected.** Endpoint retargeting and dependent destination-change cases remain blocked/out of this phase; production repair option A was not selected. [R06c](../docs/evidence/2026-10-07-r06c-1922761/index.md) adds ordinary parent rendering, retry byte alignment/exhaustion, clear, removal overlap and later/final pause variants: 54 repeated established passes, eight managed passes, separate pause/guard controls, **three removal failures and one pending-final resume failure**. These are observations, not approved behavior; matrix completion remains unchecked.

Next characterization: retain/diagnose those failures without production repair, continue unblocked callback rejection/veto/interleaving/form-state/outcome subcases, then remaining agreed lifecycle/mode scope. Keep dependent endpoint cases explicitly blocked and other unexecuted cases open. No formal API checkpoint until the agreed matrix and dispositions are finished; this bounded session does not close the full continuation request.

## Dependency sequence

**Authorize characterization only → characterize → review exact contract → explicitly approve production changes → managed export boundary/API → ordinary request pipeline → chunk/retry/resume → removal/cleanup → lifecycle/concurrency → complete verification and migration review.**

All slices change the same uploader transport and should be sequential. Read-only reviews/test-design work can be parallel; do not let multiple agents edit the same transport or invent different contracts. Unintegrated slices must not be described as an R06 fix or released.

### 0. Review direction and authorize characterization only

- Review proposed D1–D8 and authorize local dummy-data characterization, or request revisions. No production-contract approval is inferred at this stage.
- List unresolved API/error/timeout/cleanup identity questions and the observations needed to answer them.
- Verify source/dirty-tree state and preserve previous evidence. Assign security review separately; user architecture approval does not constitute security sign-off.
- Files: design, readiness plan, this plan/checklist. Documentation only.

### 1. Characterize all network entry paths with dummy inputs

- Add a focused local fixture and browser spec with gated callbacks and intercepted requests; block external requests.
- Reproduce default-header initialization/update exposure and at least one send-before-callback defect before fixes. Characterize chunk/event/retry/cleanup counts without declaring buggy behavior the desired contract.
- Add managed interop tests for configuration serialization, supplied-client ownership and bridge completion; fail on wrong child-module identifiers independently.
- Likely files (3): new `Uploader/TransportRequests.razor`, `Inputs/Uploader/transport-requests.spec.ts`, `Inputs/Uploader/SfUploaderTransportTests.cs`.
- Verify: targeted bUnit and browser runs; retain failures privately. **No production edits.**

**Required approval checkpoint:** use the observations to finalize D1–D8, callback counts/order, typed API/error/timeout definitions and host cleanup identity/reconciliation mechanism. Obtain explicit user/maintainer approval of that exact production contract and plan. If unresolved, stop here. Do not update R03 manifests yet; exact API diffs receive their own review during approved implementation.

### 2. Remove implicit export and add the approved decision bridge

Split into two small patches if needed:

- Managed configuration stops serializing client default values, including updates; preserve supplied-client ownership and implement approved value-free legacy detection.
- Add the reviewed per-attempt callback/context/decision DTO and bridge. No shared-client fallback or header transport at initialization. Test malicious client context; it is not trusted authorization state.
- Likely source: `SfUploader.razor.cs`, `.Members.cs`, `.Events.cs`, `.Methods.cs`, a focused new event-model file; tests from slice 1. Limit each patch to roughly 3–5 files.
- Verify: fail-before tests pass, Razor consumer compiles, deliberate API mismatch still fails pending explicit manifest review. Snapshot approval occurs only for the approved exact new contract.

### 3. Ordinary upload through one local preparation pipeline

- Introduce uploader-owned operation/attempt state and pure header validation/snapshot helpers if these reduce complexity; do not enlarge shared `Ajax.send` semantics.
- Await ordinary preparation callbacks, authoritative decision, validation/deadline/generation checks; send once with request-local body/headers/credentials.
- Implement approved legacy failure and preparation error statuses. No retry after policy/callback/preparation failure.
- Likely files: `uploader.js`, small uploader-local transport helper if justified, browser spec and managed tests (3–4).
- Verify: gated pending/rejected/late callbacks, empty/default reset, normal/retry success, explicit cancel, multipart/progress preservation and two interleaved files.

**Checkpoint A:** ordinary path works under the approved contract; incomplete chunk/remove paths prevent R06 closure or release.

### 4. Chunk, retry and resume use the same pipeline

- Remove shared five-second polling and duplicate-send workarounds only once replacing behavior is tested.
- Pin upload/cleanup targets and chunk geometry per operation; every network attempt reacquires its own decision.
- Preserve bounded network retry policy and approved callback multiplicities. Callback errors do not become automatic network retries.
- Likely files: uploader JS/helper and browser spec (2–3).
- Verify: chunk 0/1/later, failed attempt/retry, pause/resume, multi-file overlap, delayed callbacks beyond previous polling threshold, exact per-attempt headers/counts.

### 5. Removal and cancellation cleanup

- Await ordinary removal callbacks; `removeDirectly` cannot skip authorization. Local list removal has no send hook.
- Preserve cancel veto semantics, fence pending sends, abort accepted active cancellation and authorize cleanup separately only after prior dispatch.
- Enforce pinned cleanup target, no-RemoveUrl behavior, unresolved cleanup state and approved same-name restart conflict policy. No invented server-session guarantees.
- Likely files: uploader JS/helper and browser spec; event docs if needed (3–4).
- Verify: positive remove/cleanup counts, denied cleanup, zero-send early cancel, concurrent upload/remove and delayed cleanup versus restart.

### 6. Lifetime and configuration invalidation

- Wire observed clear/configuration changes to operation fencing, timer disposal and XHR abort **without deregistering a still-mounted component**. Preserve clear veto and mounted unresolved-identity records; implement bounded control-decision waits and transport-versus-cleanup outcome arbitration.
- Destroy/DOM removal additionally deregisters the component. Ignore late callbacks/responses; remount starts with empty credentials, not a guarantee of resolved remote state. Do not initiate cleanup on teardown or promise immediate server-disconnect detection.
- Correct child-update module identifiers if reproduced; keep this patch distinct from unrelated component cleanup.
- Likely files per small patch: uploader JS/helper, uploader lifecycle or child settings, browser spec (3–5).
- Verify: pending/active transitions, accepted/vetoed cancellation, endpoint mutation, callback rejection, observed circuit failure and reconnect/remount; no header/state leakage across instances.

**Checkpoint B:** all sending paths use the same approved decision contract; no residual defaults/shared-state bypass or unsafe cleanup fallback.

### 7. Full validation and truthful migration record

- Run targeted and full bUnit, fresh all-TFM builds, API/Razor/JSON/bridge guard, focused type checks, R04/R05 and relevant uploader browser regressions.
- Review actual emitted requests/event outcomes in accepted platform configurations. Package/runtime/security matrices remain separate when unexecuted.
- Update HttpClient/CurrentRequest/event documentation, user migration examples, test plan and changelog; clearly label behavioral breaks and remaining browser limitations.
- Freeze private fail-before/after evidence, checksums and exact tested diff. R06 is complete only if its implementation gate and required maintainer/security disposition are satisfied; never substitute the design document.
- No commit, push, signing, publication, external report or remote-settings change without authorization.

## Verification commands after approval

Run in a canonical isolated worktree with explicit logs and source identity; these commands are proposed, **not executed for R06**.

- Fresh library: `dotnet build src/Syncfusion.Blazor.Toolkit.csproj -c Release --no-incremental -p:CycloneDxEnabled=false -v:minimal`. Prefer the editor's .NET build integration; SBOM opt-out is source verification only.
- Targeted managed: `dotnet test tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj -c Release -p:CycloneDxEnabled=false --filter FullyQualifiedName~SfUploaderTransportTests --logger 'trx;LogFileName=r06-targeted.trx' --results-directory artifacts/r06/targeted`. Prefer editor test execution; isolated-discovery failure requires documented fallback.
- Full managed: same command without `--filter`, a distinct TRX filename/results directory; record discovered/executed counts.
- Browser: `npx --no-install playwright test Inputs/Uploader/transport-requests.spec.ts --project=chromium --workers=1 --retries=0 --reporter=line,json --output=artifacts/r06/browser`. Set explicit JSON report path; positive request assertions and checkpoint barriers, not sleeps/conditional skips.
- Probe: `dotnet build tools/ApiContractProbe/ApiContractProbe.csproj -c Release`; then `python3 tools/api-compat/gate.py --assemblies src/bin/Release --probe tools/ApiContractProbe/bin/Release/net10.0/ApiContractProbe.dll --output artifacts/r06/api`. An approved new API requires reviewed manifest diffs before this can pass; existing exact exceptions are not blanket permission.
- Types: use the focused ES2022/node16 strict check with explicit Node declarations. R05 found repository declarations missing; record temporary pinned tooling or separately approve repository dependency changes rather than suppress errors.

## Style and scope

Follow actual component patterns: `EventCallback<T>`, explicit serialized property names, awaited bridge calls using existing synchronization conventions. Keep DOM metadata text-safe as in R05. New internal JS should have small named operations and explicit state transitions, not more shared flags. Do not paste untrusted header objects onto arbitrary component objects.

No layout redesign. Pending preparation/cancel/error states must be reflected using existing uploader status and accessible controls, without blocking unrelated files or exposing credentials. Preserve keyboard cancellation/retry and progress semantics; add a new visual/API state only after review.

## Primary risks

- Significant migration of old header hooks despite retained signatures: explicit approval/docs required.
- More per-attempt callbacks and Server latency: measure, do not silently cache credentials to optimize.
- Same-name cleanup races and redirects cannot be solved by local IDs or CORS assertions: host protocol/endpoint prerequisites must remain visible.
- Disconnected callbacks, delayed timers and completed server work: fence locally and report uncertainty; no impossible abort/zeroization guarantees.
- Existing R07 buffering limits are not addressed by this transport redesign and must not be marked resolved.
