# R06 decisions and ownership register

**2026-10-06 — characterization executed; production contract not yet approved.** [Design](R06-UPLOADER-TRANSPORT-DESIGN.md) · [Evidence](evidence/2026-10-06-r06a-1922761/index.md) · [Readiness plan](ENTERPRISE-READINESS-PLAN.md#9-execution-log-and-restart-record).

## Authorization and scope

The user directed: **“proceed to next step - use the best and recommended approach for decisions. log the decisions why it is best and if any decision still required owner approval, let me know.”** This authorizes the next agreed step, local characterization, and delegates recommended technical choices. It does not identify a security/release owner, establish host-side storage guarantees, or approve an exact public API that has not yet been specified. The earlier explicit request to review a broader redesign first remains respected.

Recommended D1–D8 below are now the **selected technical direction under that delegation**, not a claim that the redesign is implemented or all contract details approved. No production/library or API-baseline file changed in this characterization step.

## Owner decision — 2026-10-07

- Characterization continues: opt-in, dummy/local only, no production changes and no commit/push.
- A bounded in-process/loopback dummy endpoint is approved for characterization only; never enabled in default CI or production.
- No host/storage guarantees are claimed. Browser tests cannot prove late-write prevention; host contract discussion is deferred.
- XHR redirect/cookie behavior may be observed but is **not approved policy**; separate security review is required before production reliance.
- Exact public contract, migration and API-maintainer sign-off are held until the characterization matrix is complete and a formal checkpoint is held.
- Proceed through recommended priorities 1–4 in order; append records, keep R06 unchecked and all final release gates open. These constraints supersede any inference of approval from the technical direction below.

## Blocker checkpoint — 2026-10-07: option B selected

The owner offered A (narrow production export fix with regression evidence) or B (retain endpoint retargeting as blocked for this phase). **B is selected under that choice:** the known missing child-module export remains unfixed; endpoint retargeting and dependent destination-change cases are blocked/out of this characterization phase. This avoids mixing a production repair into the continuing characterization evidence. A was not selected and no production-fix permission is carried forward.

Retain the two R06b failed probes and their evidence; do not skip them silently, relabel them passing, or call the matrix complete. Continue unblocked priorities 1–4 with opt-in dummy/local harnesses only, no broader production changes or commit/push. Exact public contract/migration, host/storage guarantees, XHR redirect/cookie policy, private security/release approvals, R06 completion and all release gates remain held. No formal API checkpoint until the agreed matrix and blocker disposition are finished.

## Selected technical direction and reasons

| Decision | Recommended selection | Why this is preferable here | Cost / remaining approval |
|---|---|---|---|
| D1 — header authority | A single typed, awaited, per-attempt `BeforeRequest`-style decision; empty headers/false credential opt-in initially; never export backend defaults or merge cached legacy headers | Initial/update/browser-request dummy export is reproduced. One source of headers prevents ambiguous event precedence and makes every network send independently testable. | New callback/context/decision signatures, JSON bridge and exact migration diff still need API-maintainer review; names remain provisional. |
| D2 — legacy behavior | Fail closed with value-free migration diagnostics when populated legacy header/credential configuration is observed, rather than silently ignoring or forwarding it | Silent ignoring can cause unintended unauthenticated actions; forwarding repeats the disclosure boundary. Explicit failure makes required migration visible without logging values. | Exact detection/error surface and whether harmless default headers block an operation require the production-contract checkpoint. No new opt-in forwarding escape hatch recommended. |
| D3 — awaits and retries | Uploader-owned preparation before synchronous XHR send; request-local headers/form/credential state; fresh authorization on every attempt | Managed bridges await correctly, but browser normal upload/remove and a later chunk send before callbacks settle. Three-chunk retry emits an extra actual attempt. Patching shared Ajax alone cannot reconcile all paths. | Approve callback order/count changes after remaining characterization. Current observed chunk 0 uses upload-start, not chunk-start; the proposed uniform chunk callback is an intentional behavioral change. |
| D4 — configuration changes | Pin logical-operation upload/cleanup targets and chunk geometry; invalidate pending work/abort affected work on relevant changes; require explicit restart | Prevents later chunks or late callback results from silently changing destinations. Avoids using a decision for a different target. | Signed URL renewal and endpoint-rotation requirements need host/product confirmation. This behavior has not been runtime tested yet. |
| D5 — cancellation and clear | Preserve veto semantics; fence unsent work immediately, abort active XHR after acceptance; use a bounded forced-stop outcome on control callback failure | Aborting before a veto resolves is irreversible. Separating transport completion from cleanup prevents claiming an already-sent request never occurred. Clear must retain mounted registration and unresolved identities. | Exact timeout/failure/status/action-complete contract needs approval; active cancellation/cleanup has not been characterized in this pass. |
| D6 — unresolved server state | Never infer server rollback from browser abort or cleanup success. Keep uncertain reuse blocked; require trusted host reconciliation/session identity to release it | A late upload can commit after a cleanup response. URLs and filenames cannot prove stable storage identity or cross-client ordering. Local IDs alone would offer false safety. | **Host/storage owner required:** stable destination/session identity, idempotency/ordering and reconciliation acknowledgment. No universal library-only solution or invented protocol is approved. |
| D7 — bounded waits | Separate 30-second default batch, per-attempt and control-decision budgets; no automatic network retry after authorization/preparation failure | Prevents hung callbacks or stale results from authorizing later work, without consuming a sequential file's budget while queued. Better than current ad hoc polling. | API/configurable range and timeout/outcome mapping require API/product approval. Thirty seconds is an engineering default proposal, not measured SLA or owner-approved requirement. |
| D8 — transport | Retain XHR for upload progress; state cookie/redirect limitations explicitly; require trusted nonredirecting endpoints | Minimizes unrelated transport/UI regressions and retains existing progress behavior. Fetch redirect rejection would be stronger but changes portable upload-progress behavior and host design. | **Host/security owner required:** acceptance of XHR redirects, same-origin cookies, cross-origin credential policy and endpoint guarantees. If strict redirect rejection is mandatory, revisit Fetch/proxy before implementation. |

## Decisions made for this investigation

- **Keep characterization opt-in and outside CI.** Passing observations describe known defects, not desired long-term behavior. Separate `proposed-policy` checks fail without skips. This protects future fixes from being blocked by tests demanding unsafe legacy behavior.
- **Use real components/interop plus intercepted local requests.** Positive request counts distinguish actual dispatch from empty-list false passes. No server upload/storage handler was added.
- **Guard the fixture by a test-host environment opt-in.** Normal runs do not render a live characterization uploader; warnings alone were insufficient. The guard is not an access-control mechanism.
- **Use explicit asynchronous callback types in managed tests.** An initial test-builder overload selected async-void behavior and invalidated the bridge-await assertion; corrected typed delegates show the real bridge awaits. Preserve initial failures as harness-development history, not product defects.
- **Limit retained request data to known dummy values.** Initial exploratory capture included fresh local antiforgery cookies; those raw attempts are excluded from the curated archive and kept locally restricted pending retention-owner handling. Corrected reruns omit cookies. Do not claim no sensitive value ever appeared in investigation logs.
- **Defer unsupported scenarios rather than simulate them misleadingly.** No live endpoint was added for pause/cancel-cleanup. Concurrency, reconnect, child-only updates, destination changes and server reconciliation remain uncharacterized, not passed.

## R06b observed follow-up — 2026-10-07

[Priorities 1–4 evidence](evidence/2026-10-07-r06b-1922761/index.md): nine configuration/isolation cases and two retry cases pass as observations; combined with R06a, 44 repeated executions pass. One genuine loopback pause/resume and two endpoint guard modes pass separately. Two endpoint-change probes **fail** after child-update errors end the circuit; retargeting remains blocked, not approved or proved.

Same-component header contamination, sticky true credentials, delayed default-header propagation, missing child export and repeated chunk on resume support further investigation of request-local state and explicit attempt contracts. No production correction follows from these observations. The bounded live endpoint is now present in the test host under the owner's narrow authorization; earlier “no endpoint” statements describe R06a history.

The owner explicitly confirms no host/storage guarantee and no XHR redirect/cookie policy approval. API/migration sign-off waits for matrix completion and a formal checkpoint. Priorities 1–4 were attempted in order, not exhaustively closed; remaining subcases and priorities 5–8 stay visible.

## R06c continuation — 2026-10-07

[Retry/overlap/pause evidence](evidence/2026-10-07-r06c-1922761/index.md) records **54 repeated established observations**, eight managed passes, three additional pause observations, original first-pause and two endpoint guard controls. Separately, **three removal-overlap probes and pending-final resume fail**; no suppression or production repair. Two R06b endpoint failures remain historical blockers under option B, not part of the green count.

- Ordinary parent render confirms delayed client-default propagation, supplementing the controlled R06b parameter calls. Clear/reselection preserves the true credential flag.
- Retry-from-stage sends the next byte slice under the failed index, then an empty final slice. Budget exhaustion uses the recorded same-byte retry count. Request-local attempt state remains recommended; these defects are not approved behavior.
- Completed-file removal changes another active upload's headers/form data. Null/empty removal decisions clear the probe header but still replace form data. All three probes fail at removal completion; exact managed cause remains unconfirmed.
- A settled final response overrides pause; pending later resume produces two successes. Pending final resume has server handler completion but no file success. The source null-event path is a hypothesis, not a captured runtime stack.
- Retain real errors and strengthen assertions rather than changing product behavior: value-free rejection of non-dummy probe values avoids false absence assertions, and pending-final server counts/timeouts are exact. Final reviewed tests rerun with the same four failures.
- Restrict raw diagnostics containing local connection identifiers; curate only reviewed dummy outputs, including failures. Private local archive/checksum is not an organizational retention or security approval.

**Scope remains incomplete.** Option B dispositions only endpoint retargeting/dependent destination changes for this phase. Other callback rejection/veto/interleaving/outcome cases and broader cleanup/lifecycle/modes remain open. Continue unblocked characterization; no formal API checkpoint yet. This bounded session is not closure of the owner's full continuation request.

## Owner approvals still required

1. **API/maintainer:** exact new public/wire/error/timeout contract and explicit 2.0 legacy/event-order migration. The user has delegated direction, but no exact symbol-level contract exists yet. R03 manifests must not change before that review.
2. **Host/storage:** upload-session identity, server late-write/idempotency guarantees and a reconciliation mechanism. Without these, preserve conservative unresolved state; do not enable conflicting reuse by assumption.
3. **Host/security:** endpoint trust/redirect/cookie/CORS/CSRF policy and whether XHR's limitations are acceptable. Never return backend service credentials to a browser callback; client-supplied context is not authorization evidence.
4. **Named private security reviewer:** affected-version/impact assessment, private disposition, approved evidence store/retention/disclosure. R04/R05 holds remain; agent review or user direction is not a fabricated sign-off.
5. **Release owner:** clean candidate, package/runtime/security verification, commit/signing/publishing or external reporting. None was authorized or performed.

**Recommended next action:** continue the remaining bounded characterization and draft exact API/error/timeout definitions, then present the concrete production contract for maintainer approval. Do not start a broad production rewrite or automatically advance to R07. A separately authorized urgent no-export mitigation is still possible, but the user previously chose broader review first and no such permission is inferred here.
