# R06 — Uploader transport redesign for review

**Status: recommended direction selected; bounded characterization executed; exact production contract pending — 2026-10-06.**

The user first chose **“Review a broader transport redesign first”**, then authorized the next step and delegated recommended decisions with reasons and owner-approval needs recorded. [Decision register](R06-DECISIONS.md) selects the technical direction; [R06a evidence](evidence/2026-10-06-r06a-1922761/index.md) records the first bounded runtime pass. This is not exact API/production implementation approval. [Readiness plan](ENTERPRISE-READINESS-PLAN.md#9-execution-log-and-restart-record) remains authoritative; R04/R05 private-review holds remain. [Sequence](../tasks/plan.md) and [checklist](../tasks/todo.md) retain the pre-production approval checkpoint.

Base: `address-warnings` / `19227619082433fa3b86e15d4e7aee625850aed9` plus preserved uncommitted changes through R05. The initial design pass was read-only; subsequent **test-only local characterization** produced eight managed passes, 22 current-behavior observations and two failing proposed safety controls. No R06 production/API-baseline changes, external report or release occurred.

## 1. Objective and proposed assumptions

Prevent accidental server-client header disclosure and make deliberate browser request preparation deterministic across uploads, chunks, retries, resume, removal and cancellation cleanup.

Assumptions **requiring approval**, not existing guarantees:

- This is an explicit 2.0 behavioral migration, not a compatibility patch hidden behind unchanged signatures.
- One new typed, awaited per-attempt callback is the only source of application-supplied browser headers and cross-origin credential opt-in. Names below are provisional.
- Keep XHR for current upload-progress behavior. Do not rewrite shared `ajax.js` for every component.
- Preserve host ownership of authentication, endpoints, storage and upload-session identity. No server endpoint or protocol is silently added.
- No automatic fallback to stored credentials when preparation fails. Browser-bound values are visible to the browser by design.
- This changes the existing uploader, not application navigation, visual design, CSS isolation, R07 file budgets or DataManager transport.

## 2. Current implementation — source-confirmed findings

| Finding | Inspected source |
|---|---|
| Every `HttpClientInstance.DefaultRequestHeaders` value is serialized by `GetCustomHeaders()` into `GetProperty()`; initialization and tracked parent/child updates use that configuration | `src/Components/Inputs/Uploader/SfUploader.razor.cs`: `OnAfterScriptRenderedAsync`, `GetCustomHeaders`, `AddLocalizationAndHeaders`, property-update path |
| Upload/remove requests are browser XHR, not requests sent through the supplied .NET client | `src/wwwroot/scripts/uploader.js`: `uploadFilesRequest`, `sendRequest`, `removeUploadedFile`; `src/wwwroot/scripts/ajax.js`: `send` |
| `BeforeUpload` and ordinary `BeforeRemove` are awaited, but then write component-wide header/form/credential state | `uploader.js`: `upload`, `beforeUploadCallback`, `remove`, `beforeRemoveCallback`, `UpdateHttpHeaders` |
| Some later callbacks resolve after dispatch; chunk preparation contains shared polling/re-send state instead of one await contract | `ajax.js`: synchronous `beforeSend`; `uploader.js`: `uploadFilesRequest`, `sendRequest`, `removeUploadedFile` |
| Defaults can accumulate; null headers and false credential settings can retain old state; retry/resume/cleanup paths differ | `uploader.js`: `beforeUploadCallback`, `retryRequest`, `resumeUpload`, cancellation cleanup paths |
| Clear/destroy do not establish complete credential-state and registry cleanup; disconnected disposal can skip browser destroy | `uploader.js`: `clearData`, `destroy`; `SfUploader.razor.LifeCycle.cs`: `DisposeAsyncCore` |
| Child settings/button update invocation names appear inconsistent with the module's flat export | `ChildComponents/UploaderAsyncSettings.cs`, `ChildComponents/UploaderButtons.razor.cs`, `uploader.js`: `propertyChanges` |

These establish risk and test targets, not actual deployed compromise. `HttpClientInstance` handlers, timeout, base address, certificates and retry policies do not govern the inspected browser XHR path. Default-header export is distinct from handler-held credentials. DataManager genuinely uses its own HTTP client and is outside this change.

## 3. Recommended architecture

### A. One uploader-owned request preparation pipeline

Proposed order per request attempt:

1. Resolve the logical operation and pin its destination/configuration snapshot.
2. Await applicable existing batch/file/chunk/removal callbacks and apply cancellation and custom-form decisions to that operation only.
3. Finalize the request-local body and immutable request context.
4. Create **fresh empty headers, `WithCredentials=false`, `Cancel=false`**; await proposed `BeforeRequest`.
5. Validate the returned decision, deadline, operation generation, component lifetime and pinned target. Discard all stale results.
6. Configure XHR synchronously from that attempt's finalized snapshot; send exactly once.
7. Observe the outcome, release attempt data and schedule any eligible retry through this same pipeline.

No asynchronous credential work remains inside shared `Ajax.beforeSend`. The uploader may prepare before constructing an `Ajax`, or use a small uploader-local transport adapter if existing hooks make one-send accounting impractical. Choose after characterization; neither permits changing all shared Ajax callers as a shortcut.

**Request-local means more than headers:** form data, file/chunk metadata, credential mode, callback results, cancellation state, timers and send/settlement flags belong to the logical operation or its attempt—not a reusable uploader-wide mutable slot. No global queue should block unrelated operations.

### B. Proposed typed callback contract

Provisional public API: `EventCallback<UploaderBeforeRequestEventArgs> BeforeRequest`. Exact C#/wire definitions require API review before implementation.

| Context — inspect only | Decision — application may set |
|---|---|
| Absolute resolved target and method | Case-insensitive `IDictionary<string, string>` headers, initially empty |
| Purpose: upload, remove, cancel-cleanup | `WithCredentials`, initially false |
| Stable logical operation ID; unique attempt ID | `Cancel`, initially false |
| Attempt number; trigger: initial, retry, resume | No URL rewrite or live-XHR mutation |
| Read-only file metadata snapshot; nullable chunk index/count/offset/length | No raw bytes/base64 or existing mutable `FileInfo` reference |

The .NET bridge returns only decisions, not an authoritative replacement context. IDs and browser metadata are correlation information, **not authentication**. Server callbacks must authorize against trusted user/session/application state and check destination allowlists; a modified client can call exposed interop endpoints with fabricated context. Merely installing this callback must not expose a backend credential oracle.

No callback means empty application headers and cross-origin credential opt-out, allowing ordinary unauthenticated or same-origin cookie-backed requests. A configured callback's error/cancellation/timeout must never silently fall back to that no-callback behavior.

### C. Header validation

- Remove all automatic .NET default-header export, at the managed boundary, for initialization and all update paths. JS filtering is too late.
- Validate in managed code for useful feedback and again in JS before `send`. Values must be strings; names must satisfy HTTP token syntax. Reject CR/LF/NUL and malformed structures. Canonicalize names case-insensitively; reject ambiguous duplicate names rather than comma-concatenating credentials.
- Reject browser-forbidden/request-controlled headers and reserve multipart `Content-Type` for the browser. The exact maintained forbidden-name/prefix rules and tests are part of API/security review; `setRequestHeader` can silently ignore names and cannot alone establish validation.
- Do not infer permission from header name, origin or a blacklist of sensitive names. Deliberate `Authorization` and application token headers are allowed only through the explicit callback and host policy.
- Copy and freeze each decision after callback completion. Null/empty headers mean empty **for this attempt**, never previous headers. False always resets credential opt-in.
- Do not log/persist header values or full authorization-bearing URLs. Error messages identify validation category/operation, not credential contents. Dropping references limits retention; JavaScript/.NET memory cannot promise secure erasure.

### D. Legacy migration — approval required

Recommended **no legacy merging or fallback**. Preserve old property signatures initially, but publish the following proposed 2.0 behavior changes:

| Existing path | Proposed migration |
|---|---|
| `HttpClientInstance.DefaultRequestHeaders` | Never export values; client remains application-owned and is not disposed or used as XHR transport. Detect configured defaults using a value-free marker and block affected sends with a migration error when observed, rather than silently losing intended authentication. Detection timing for in-place mutation must be tested/documented; no export is allowed even when a marker is stale. |
| `CurrentRequest` on selection/upload/chunk/remove event models | Structurally retained; no live XHR claim. Populated legacy header data blocks dispatch with a value-free migration error. Move header logic into `BeforeRequest`. Empty/null is not an error. |
| `BeforeUpload.WithCredentials` | Move to per-attempt decision. True in the legacy path produces a migration error; false remains its neutral default. |
| Existing cancellation/custom-form/event handlers | Remain supported under the explicit await/order table below, without being alternate credential channels. |

This is intentionally stricter than silently ignoring old values. **Alternative for approval:** documented ignore-with-warning semantics, or a separately versioned opt-in transport. Neither is selected here. A fallback that copies legacy headers defeats the new boundary and is not recommended.

The new parameter/type/bridge and any failure model additions change frozen R03 manifests. Review exact diffs and migrations; do not regenerate snapshots or add suppressions merely to pass the gate. Existing R03 approval does not authorize R06 API additions.

### E. Proposed event order and retry contract

All orders are per logical operation, not global serialization. Existing completion/progress callback counts need characterization before the table is approved.

| Path | Proposed before-send ordering |
|---|---|
| Ordinary batch upload | `BeforeUpload` once per explicit batch; `OnUploadStart` per file attempt; `BeforeRequest` last for every actual attempt |
| Chunked upload | `BeforeUpload` once per batch; `OnUploadStart` once at initial logical file start; `OnChunkUploadStart` for each attempted chunk including chunk 0 and retries; `BeforeRequest` every attempt |
| Retry/resume | Existing retry/resume notifications where applicable; applicable per-attempt start callback; fresh `BeforeRequest`. Never reuse authorization from an earlier attempt. |
| Explicit remove | `BeforeRemove` once per remove batch; `OnRemove` per file unless `removeDirectly` skips it; `BeforeRequest` always before actual remove sends |
| Cancellation cleanup | Cancellation decision accepted first; separate `BeforeRequest` with cancel-cleanup purpose before any cleanup attempt. No inherited upload/remove credential collection. |
| Local list-only removal/no endpoint | Existing local callbacks; no request and no `BeforeRequest` |

`removeDirectly`, templates and hidden lists cannot bypass request preparation. Callback rejection, invalid headers, migration failure and preparation deadline are **not** automatic network-retry conditions. Actual network failures retain a reviewed bounded retry policy and reacquire authorization on each attempt. No new unlimited retry behavior.

Awaiting preparation callbacks does not pause an already-sent request. Native progress notifications require an ordered/coalesced delivery strategy with bounded retention; completion must not produce an unbounded backlog or be represented as network cancellation. Verify actual status/action-complete behavior rather than inventing new event multiplicities silently.

### F. Lifetimes, endpoints and cleanup

- Logical upload snapshots include upload destination, associated cleanup destination and chunk geometry. Resolve the exact absolute target before the authorization callback and send that same URL; no live option reread after await.
- **Recommended configuration-change policy:** invalidate pending/queued attempts and abort active affected operations when a relevant endpoint/chunk configuration changes; require an explicit restart under new configuration. Do not silently retarget later chunks, retries or cleanup. Alternative: let existing pinned operations finish while changes affect only new operations. Approval must choose one.
- Signed-URL renewal is not a hidden `BeforeRequest` URL rewrite. Under the recommended policy, changing a URL requires a restart. Applications needing renewal during a multipart session need a separately specified resolver/server protocol.
- Cancellation intent immediately fences unsent attempts. Existing `OnCancel.Cancel=true` veto semantics mean active XHR abort occurs **after cancellation is accepted**, not irreversibly while waiting for a possible veto. On veto, release the fence only if the operation/generation remains valid. Forced teardown/configuration invalidation is not a user-veto event. Control callback failure/timeout is a separate forced-stop outcome, described below.
- Accepted cancellation before any upload bytes/request were sent causes **zero upload and cleanup sends**. Previously dispatched requests may have reached the server despite abort.
- Cleanup uses the operation's pinned cleanup endpoint, new authorization and its own bounded attempt state. An absent RemoveUrl means no remote cleanup, never POST to the page URL. Denial/failure must report unresolved remote state, not successful deletion.
- A local operation ID does not fix server deletion keyed only by filename. **Recommended conservative policy:** prevent conflicting reuse while the server outcome is unresolved. Even a successful cleanup response cannot prove an aborted upload will not commit afterward. Release the conflict only after host reconciliation proves no late writer remains, or use a new server-issued upload-session identity with host-enforced ordering/idempotency. If unavailable, keep the conflict unresolved rather than guessing.
- Literal URL equality is not a reliable conflict key: different signed URLs can address the same object. A host-supplied stable destination/storage identity plus file/upload-session identity and an explicit reconciliation acknowledgment mechanism must be designed and approved before D6 can be implemented. Without that API/protocol, the safe fallback is blocking uncertain file reuse within the component irrespective of changed URL—not an assertion of cross-tab/server safety. Clear must retain unresolved conflict records while mounted; remount loses local protection, so hosts must retain reconciliation state independently.
- User clear is a **vetoable operation reset**, not component destruction: fence affected sends while awaiting `OnClear`; accepted clear invalidates operations, clears queued authorization/timers and aborts active requests, but retains component registration and unresolved remote-state records. A veto resumes only valid work. Endpoint/configuration changes likewise retain registration. No new cleanup traffic on clear unless separately designed and approved.
- Destroy/observed DOM removal additionally unregisters the component and releases owned references, invalidates generations, cancels timers and aborts XHRs. Do not initiate network cleanup on teardown. Dropping local records does not resolve unknown server outcomes.
- Disconnect is observable only when a callback/interop fails or browser-side lifecycle signal is received. Do not promise instant server-disposal awareness or reversal of requests already dispatched. A pending late response must fail generation/deadline checks; remount starts without cached credentials, not without possible unresolved server state.

### G. Preparation deadline and error reporting

Proposed **30-second pre-send deadline per attempt**, including its per-attempt preparation callbacks. It starts only when that attempt leaves the queue, not while earlier sequential files upload. Once-per-batch `BeforeUpload`/`BeforeRemove` has its own separate 30-second deadline; failure prevents all unsent batch attempts, and resolved noncredential batch decisions are copied into each operation. Do not restart or charge a batch wait against every queued file. Callback/migration/preparation failure never enters network retries.

Control callbacks (`OnCancel`, `OnClear`) have a separate proposed 30-second decision deadline even for active XHRs. While awaiting them, fence new sends but do not assume the network has stopped. **Proposed timeout/rejection policy:** invalidate pending work, abort still-active requests, release the wait, report control-decision failure and unresolved remote state; no automatic cleanup or resume. This forced-stop behavior needs approval and is not a successful user cancellation. A successful veto restores only still-valid unsent work.

Record an upload's actual transport terminal outcome once if it completes during a control wait. A later accepted cancel may authorize a separate cleanup of already-dispatched work; it must not rewrite transport success as never sent or duplicate completion callbacks. A late veto/timeout cannot resurrect a completed attempt. Exactly which public completion/action callbacks expose transport versus cleanup outcomes must be fixed by characterization and explicit contract approval.

Separately choose timeout configurability (potentially `RequestPreparationTimeout` plus a control-decision setting); exact API/type/range is **not approved**. Network XHR timeout is distinct and is not `HttpClient.Timeout`. Recheck elapsed deadline immediately before send because browser timers can be delayed. Timeout invalidates results; it cannot forcibly terminate arbitrary application callback work. Where feasible, provide cancellation notification without claiming guaranteed task termination.

Errors must have stable value-free categories: legacy-configuration, invalid-header, callback-failed, preparation-timeout, control-decision-failed, stale-operation, transport-failed and cleanup-unresolved. Map to existing failure/cancel UI only where semantically accurate; new public error/completion models require review. Exactly one transport terminal result per attempt and a separately tracked cleanup result; no success for blocked dispatch or failed cleanup.

## 4. Browser and host limitations

XHR is retained **only if approved**:

- `WithCredentials=false` does not suppress same-origin cookies or explicitly set Authorization headers.
- XHR follows redirects; inspecting `responseURL` afterward is too late to prevent disclosure. The inspected destination is the initial target, not guaranteed final origin.
- Hosts must use trusted, nonredirecting upload, remove and cleanup endpoints and enforce CORS/CSRF/authentication appropriately. This is a host requirement, not an uploader-enforced redirect guarantee.
- If strict browser-enforced redirect rejection is required, review a Fetch `redirect:"error"` transport or a backend proxy instead. Fetch changes portable upload-progress behavior; proxy design introduces a server security boundary. Neither is silently included.
- Browser values are never appropriate for server-only service credentials. A backend proxy must perform privileged operations without returning its credentials to browser event callbacks.
- .NET Server, WebAssembly and Auto differ in where application code runs; all browser XHR paths still bypass .NET client handlers. Static SSR does not perform these requests until an interactive component exists.

## 5. Acceptance evidence — proposed, not executed

Use only explicit dummy values and local intercepted endpoints. Block all unrelated/external traffic. No production token collection, public demo uploads or remote reproduction.

1. **Managed boundary:** exact initialization and parent/child-update interop contain no dummy default headers under manual/auto/no-URL/template/persistence configurations. Client replacement and in-place mutation cannot export values. Fake handler proves no .NET send/disposal of supplied client. Correct module identifiers independently tested.
2. **Request preparation:** controlled callback gates prove zero dispatch while pending; release produces exactly one expected request with fresh headers/credential mode. Rejection, timeout, invalid metadata, cancellation and stale generation produce zero sends. Retained late callbacks cannot affect another operation.
3. **Every send path:** ordinary, auto, sequential, multi-file, templates, hidden list, chunk 0/later chunks, automatic/manual retry, resume, remove/direct remove, cancellation cleanup. Assert actual positive request counts on success paths and every attempt's headers; an empty list is not a successful forwarding test.
4. **Concurrency:** interleave two instances and two files in one instance; overlap upload/remove; null/empty reset and true→false; endpoints change while callbacks are pending and requests active. No cross-operation header/form/credential state. Cleanup/restart conflicts produce the chosen explicit outcome.
5. **Lifecycle:** pending/active cancel with veto/accept, clear, dispose, DOM removal, callback failure, observed disconnection/reconnect/remount. Track timers/XHRs/registry and ignore late work. No cleanup after teardown and no automatic resend of an uncertain completed request.
6. **Migration/API:** deliberate legacy configuration yields chosen migration behavior; default unauthenticated use still works; approved typed callback Razor fixture compiles; exact API/JSON/bridge additions reviewed. Negative new-break controls still fail the R03 guard.
7. **Wider regression:** relevant uploader bUnit/browser suites, retained R04/R05 tests, full bUnit and all-TFM builds. Preserve multipart fields, progress/status/final event outcomes and supplied-client ownership. Repeat stress-sensitive interleavings without sleeps or conditional passes.
8. **Scope:** start with net10 Server/Chromium; run relevant accepted platform modes and browser matrix before broader support claims. Package-reference, lifecycle, security-owner and released-artifact approvals remain separate. Retain fail-before/after and limitations privately as required.

## 6. Review decisions — selected direction, exact contract pending

The user's later delegation selects the recommendations as technical direction; the [decision register](R06-DECISIONS.md) explains why and separates remaining API/host/security/release approvals. None is an implemented guarantee.

| ID | Recommended choice | Principal cost / alternative |
|---|---|---|
| D1 | One new authoritative typed `BeforeRequest` for every attempt; no implicit defaults or legacy merge | Explicit API/behavior migration; alternative carefully specified legacy adapters, not automatic reuse |
| D2 | Fail closed on observed populated legacy credential settings, with value-free migration errors | Existing apps must migrate; alternative ignore-with-warning risks unintended unauthenticated requests |
| D3 | Await/order table above, fresh authorization every retry/resume/cleanup attempt | More callbacks/latency; application handlers must tolerate retries and use trusted context |
| D4 | Invalidate/abort on relevant destination/configuration changes, explicit restart | Signed-URL renewal and live queues may need a separately approved design |
| D5 | Preserve cancellation veto; fence pending sends immediately, abort after acceptance | Active request can complete during veto wait; immediate abort would be a behavior break |
| D6 | Block uncertain reuse even after cleanup success until host reconciliation/session ordering resolves late-writer risk | Stable host identity/acknowledgment API or server protocol is still to be designed; clear must not discard mounted conflict records |
| D7 | Separate bounded 30-second batch, attempt and control-decision waits; force-stop on control timeout/rejection | Timeout/error/completion API and configuration require review; cannot stop arbitrary .NET work |
| D8 | Retain XHR, explicitly accept redirect/cookie limitations and nonredirecting-host requirement | Strict redirect rejection needs Fetch/proxy and progress/hosting redesign |

**Next step:** continue the remaining local characterization under the current authorization and draft exact API/error/timeout/cleanup identity definitions. The first bounded pass is complete, not the full matrix. Then obtain explicit exact-production-contract approval and host/security decisions identified in the decision register. Characterization/direction delegation is not approval of unspecified public signatures or server guarantees. Do not start R06 production changes, alter current baselines, or proceed to R07 automatically while those gates remain open.
