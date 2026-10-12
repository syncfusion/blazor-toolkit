# R06b — Priorities 1–4 characterization, with endpoint blockers retained

**Owner/session date: 2026-10-07.** Base `address-warnings` / `19227619082433fa3b86e15d4e7aee625850aed9` plus preserved R02–R05 source changes. Raw Playwright timestamps use UTC on 2026-10-06 (20:32–20:35); they are retained as emitted, not relabeled. [Plan](../../ENTERPRISE-READINESS-PLAN.md#9-execution-log-and-restart-record) · [Decisions](../../R06-DECISIONS.md) · [Prior R06a](../2026-10-06-r06a-1922761/index.md).

**Partial characterization only; R06 is unchecked.** No production/library or API-baseline changes. The owner authorized opt-in dummy/local characterization, including a bounded loopback endpoint, but explicitly held API/migration approval until the matrix is complete and a formal checkpoint occurs. No host/storage guarantees or XHR redirect/cookie policy approval is inferred.

## Scope and execution order

1. Configuration mutation/replacement and child-only updates.
2. Same-component/per-instance credential isolation, null/empty headers and true→false credential flag.
3. Endpoint changes during pending callbacks/chunks.
4. Ordinary/chunk manual retry and genuine pause/resume.

New test-host `TransportProbe`/`TransportMatrix` fixtures isolate public parameter/method calls. The wrapper's rendering is frozen to avoid incidental ledger rerenders; this does **not** freeze uploader descendants. Public `SetParametersAsync` probes and explicit CSS resynchronization are controlled investigation paths, not a claim that ordinary application parent rendering works identically. No JS production state is assigned to force results.

## Results

| Check | Result / meaning |
|---|---|
| Configuration/isolation matrix | **9 observed cases pass**: mutation, replacement, two child-update probes, interleaving in both orders, two-instance control, null and empty header/credential reset cases |
| Manual retries | **2 observed cases pass**: ordinary and chunk restart |
| Repeated established observations | **44 passed, 0 failed/skipped/flaky**, retries disabled: 11 existing R06a + 11 successful R06b cases, each twice. Endpoint probes explicitly excluded from this run and separately retained below. |
| Endpoint pending/chunk probes | **2 failed**, retained final run. Child missing-export failure recurs during descendant rendering and ends the circuit. Expected upload completion/retargeting assertions do not pass. No skip or product shim. |
| Genuine live pause/resume | **1 passed**, on a fresh opt-in host using real upload progress; repeated in separate host runs, not twice against consumed endpoint state |
| Endpoint rejection bounds | **1 passed**: bad origin, 8,193-byte rejection, 16 admitted malformed attempts then 429, missing release-control header |
| Disabled host | **1 passed**: state route absent, POST not handled by the endpoint, matrix fixture shows disabled and no uploader |
| Focused strict TypeScript | **Pass** with retained temporary pinned Node declarations; no repository dependency change |
| Preservation | **511 production source files + 8 API-baseline files unchanged** against retained R05 snapshot |

These passes assert **current behavior**, including defects. They do not validate a safe redesign. R06a proposed-policy failures remain historical red controls; not rerun here. Full managed tests, API gates, multi-TFM builds and default CI suites were not rerun. Required host builds occurred through Playwright; there is no new zero-warning/library-matrix claim.

## Observed behavior

- Changing default headers or replacing `HttpClientInstance` alone leaves old browser defaults and the next request's dummy default unchanged. A tracked parent change exports the newly read default. Supplied clients remain fixture-owned.
- Both async-settings and button-only updates attempt nonexistent module member `sfBlazorToolkit.Uploader.propertyChanges`. The fixture catches only the matching exception and records it; browser endpoint remains old at that checkpoint.
- Interleaved chunk uploads for two files in one component: file A's later chunk uses the header last installed by file B; reversed ordering reproduces the converse. A two-component control retains each component's headers. This is within-component contamination, not evidence of cross-user disclosure.
- Null headers retain the previous header; an empty collection clears it. Managed decision logging proves the second callback supplied the chosen category and false credentials. The observed second ordinary XHR still reports `WithCredentials=true`. This is the XHR flag only, not observed cookie transmission or approved policy.
- Endpoint attempts fail: a child update followed by explicit parent resync causes another descendant update to throw, ending the circuit. Some browser options can already have changed, but complete end-to-end retargeting is **blocked/unproven**. Server diagnostics locate the missing export; no library repair or bypass was introduced.
- Ordinary manual retry invokes `BeforeUpload` again and sends two whole-file attempts. Chunk manual restart sends indices `[0,1,0,1,2]` after failure at index 1, without another `BeforeUpload`; the restarted first slice matches the original dummy digest.
- Genuine pause after chunk 0 reached status `3` via upload progress, then status `4` via public `PauseAsync`. After releasing/settling the first response, it remained paused with only one send. Public resume retransmitted the same slice/index: `[0,0,1,2]`, four bounded server receipts/completions. No internal status or metadata assignment; no claim of server idempotency.

## Approved loopback endpoint boundary

Only the **test host** changed: one conditional mapping call plus `R06LoopbackEndpoint.cs`. Registration requires Development **and** `R06_CHARACTERIZATION=1`. Normal CI does not use the opt-in config; disabled behavior is tested. Dedicated config refuses server reuse. Owner approval does not authorize production use.

Endpoint constraints:

- Loopback peer and local Host checks; exact local browser Origin for upload; explicit dummy control header for release.
- Declared body length 1–8,192 bytes, fixed 8,193-byte read buffer, no `ReadForm`, file buffering, disk storage or body logging.
- Monotonic maximum **16 admitted upload attempts** per host, including malformed bodies. State/release control requests are outside that upload budget.
- Exact dummy `A.txt` and 1,024-byte A/B/C chunk contents required; narrow fixture parser, not a general multipart implementation.
- 15-second per-request/body/gate deadline linked to request abort; active requests tracked, timeout evidence sticky. No general host-lifetime timeout claim.
- One irreversible release/history state per host. Pause and budget-guard tests must run on separate fresh hosts; no reset endpoint or same-host pause repetitions.
- The endpoint disables only status-page re-execution for its own requests so 403/413/429 remain observable. Antiforgery is not disabled. Without registration, this host's missing POST is re-executed into its Razor not-found handler and yields 400; GET state returns 404.

No actual credentials, external endpoints or customer bytes were used. HTTP routes block nonlocal origins; no claim of universal network isolation. Redirect/cookie handling is **not approved**. Host late-write prevention and storage/session guarantees remain deferred.

## Harness failures and review

Preserved initial fixture compilation failure (nested Razor quotes), then prerender failure (method groups on not-yet-assigned component references); fixed test markup/deferred lambdas. These are not product failures.

Initial endpoint probes expected successful resynchronization. They failed; subsequent server diagnostics exposed the repeated child lifecycle error/circuit failure. Final probes retain those failed assertions. No production change, broad exception catch or assertion waiver was used.

Read-only review led to monotonic endpoint admission limits, malformed-index bounds, sticky timeout evidence, pending-XHR/route settlement checks, always-attached sanitized matrix evidence, explicit decision logging and pristine-host checks for pause. Guard assertions initially expected direct rejection statuses but host status pages transformed them; test-only endpoint status-page handling was corrected and guards rerun. A config type error was fixed by narrowing the single-server type, not suppressing TypeScript checks.

Teardown releases managed/response gates, waits for pending XHR loadend and retains route settlements/errors. Endpoint probes also retain teardown circuit errors; release does not prove production cleanup. Child-error diagnostics in raw local logs may contain local circuit identifiers; only curated dummy-result outputs are archived for review.

## Commands and environment

Same retained macOS arm64/SDK 10.0.401, net10 Interactive Server Release, Playwright 1.63.0 / Chromium 153.0.8010.12. Fresh canonical worktree `/private/tmp/blazor-toolkit-r06b-xIa0DO/source`, current source overlay labeled dirty-tree verification, reused prior installed tooling, themes generated only there.

- Established cases: `npx --no-install playwright test --config tools/transport-characterization/playwright.config.ts uploader.characterization.ts matrix.characterization.ts --project observations --grep-invert 'endpoint-' --repeat-each=2 --reporter=line,json`.
- Blockers: same config with `matrix.characterization.ts --project observations --grep 'endpoint-'`; exit 1, two failures.
- Live progress: same config with `pause.characterization.ts --project observations`; one per fresh host.
- Bounds: `npx --no-install playwright test --config tools/transport-characterization/guard.config.ts --reporter=line,json`.
- Disabled control: same guard command with `R06_VERIFY_DISABLED=1`.
- Every run uses explicit separate JSON/output paths. Strict `tsc --noEmit --strict --types node --typeRoots <retained-temporary-node-types> --target ES2022 --moduleResolution node16 --module node16 --skipLibCheck` covers new harness files. No default test configuration/workflow/dependency change.

## Retained evidence

Private local reference **R06B-LOCAL-20261007**, `characterization-priorities1-4.tar.gz`, SHA-256:

`8a8ee1b713f3f170812672c0d28bff2555c6c9e2c574ae4c13b53c5a80ed9c3a`

Current-user Code Studio application-support `readiness-evidence/2026-10-07-r06b-1922761/`, mode 0700 and archive 0600. Contains final successful observations **and both final endpoint failures**, guard/pause/type records and tested source. Raw investigation remains restricted under `/private/tmp/blazor-toolkit-r06b-xIa0DO/evidence`. Curated archive lacks the local antiforgery cookie-name marker; not a comprehensive secret certification or approved retention store. Named evidence owner/retention/private disposition remains pending.

| Tested file | SHA-256 |
|---|---|
| `matrix.characterization.ts` | `673f35288a20122ffbed5640c7e59ad8bffd3f09e1392fc15203c35c7744417e` |
| `pause.characterization.ts` | `3dc912800f125dd8f9a339b26eb1c25bb30c34bf320e8ce63a73ce23df7233e4` |
| `endpoint-guards.characterization.ts` | `6b4975ea9baf97c72c35060fa96bf8fff94c624cab28b6a22a765196c3f1b474` |
| `guard.config.ts` | `36f31318c2b4ff756241e363f869b1b7145e3e03e9de38e869b704bf0dd545c0` |
| Test-host `Program.cs` | `5af23934ae4176dce591871945a6bf268497bb8e08e5e72d2f44f8129fbc4693` |
| `R06LoopbackEndpoint.cs` | `aadaa8883005b2832d9b3caf3db45743aed8398b18a197272164a6e73118bcb3` |
| `TransportProbe.razor` | `1faf8c69851d927b2fb0ba0862bea3cb921979ad060155fc23e6fb3ea9878d42` |
| `TransportMatrix.razor` | `a29dfdc244154d49b06ab9a2008b6fcbd203a6fedbfee9dd159a33704bb2b34a` |

## Remaining matrix and checkpoint

Priorities 1–4 were executed in order, **not exhaustively completed**. Endpoint-change end-to-end cases remain blocked; mutation-only and replacement probes use controlled direct parameter calls. Retry-from-canceled-stage byte alignment, endpoint retry/resume changes, simultaneous upload/remove, form-data isolation and all event multiplicities remain open. Priorities 5–8 (broader cancellation/cleanup, lifecycle/disconnect, remaining modes and outcome accounting) are not closed. Negative peer/Host variants, timeout expiration and more malformed multipart shapes also remain untested.

Continue characterization or hold a scope/blocker discussion; do **not** fix the child export under this no-production-change authorization. Formal API/migration checkpoint waits for complete/explicitly dispositioned characterization. No host/storage or XHR-policy approval. R04/R05 holds, R06 checkbox and all nine final release gates remain open. No commit, push, publish, signing, external report or remote setting change.
