# R06 proposed implementation checklist

**CHARACTERIZATION AUTHORIZED; exact production contract pending — 2026-10-06.** [Design](../docs/R06-UPLOADER-TRANSPORT-DESIGN.md) · [Decision log](../docs/R06-DECISIONS.md) · [Plan](plan.md) · [Authoritative status](../docs/ENTERPRISE-READINESS-PLAN.md#9-execution-log-and-restart-record).

This is an implementation checklist, not new backlog tasks. No item is complete merely because design text exists.

- [x] Review proposed D1–D8 direction and obtain authorization for local characterization only.
  - User delegated recommended decisions and reasons. Exact API/host/private security approvals remain separate.
- [ ] Complete local dummy-header/interleaved-callback characterization before fixes.
  - First bounded pass completed: opt-in eight managed tests and 22 browser observations pass; two safety controls fail. Production unchanged. See [evidence/limits](../docs/evidence/2026-10-06-r06a-1922761/index.md).
  - R06b priorities 1–4: nine config/isolation + two retry observations pass, combined 44 repeated cases; live pause and two endpoint guard cases pass. Two endpoint-change probes fail due to child interop/circuit errors. [Evidence](../docs/evidence/2026-10-07-r06b-1922761/index.md).
  - Blocker checkpoint: **B selected**, endpoint retargeting/dependent destination changes blocked/out of phase; no production export repair. Do not relabel retained failures or call the matrix complete.
  - R06c: ordinary parent updates, retry-from-stage/exhaustion and clear add five established cases (combined **54 repeated passes**); eight managed and separate pause/guard controls pass. **Three overlap-removal failures and one pending-final resume failure** retained. [Evidence](../docs/evidence/2026-10-07-r06c-1922761/index.md).
  - Remaining: failure root-cause characterization, callback rejection/veto schedules, broader interleaving/form-state reset and exact outcomes; cleanup/lifecycle/modes and remaining scope stay open. No host/storage or XHR policy approval. Exact contract/migration sign-off waits for agreed matrix/disposition completion and formal checkpoint; this bounded session does not close the continuation request.
- [ ] Finalize exact API/timeout/error/cleanup-identity definitions and callback order using observations; obtain explicit production-contract approval.
  - Verify: unresolved decisions block implementation; characterization approval does not substitute.
- [ ] Stop managed default-header export and implement the approved typed decision bridge.
  - Verify: every configuration path contains no dummy defaults; client ownership preserved; exact new API/serialization reviewed.
- [ ] Route ordinary upload attempts through request-local awaited preparation.
  - Verify: pending/failed/stale decisions send zero, released valid decisions send once; credential reset and multipart/progress tests pass.
- [ ] Route chunks, retry and resume through the same pipeline.
  - Verify: exact attempts/chunk ordering and fresh headers; no shared polling/duplicate sends or automatic retries on policy failure.
- [ ] Route remove and cancellation cleanup through the same policy.
  - Verify: direct remove still authorizes; early cancel sends nothing; cleanup failure and same-name conflicts follow approved contract.
- [ ] Fence configuration changes, cancellation, clear, teardown and observed disconnect.
  - Verify: no late send/state mutation; bounded timers/registry ownership; no teardown cleanup traffic; remount starts empty.
- [ ] Run wider regressions and review exact API/migration changes.
  - Verify: all-TFM build, full bUnit, R03 guard, R04/R05 browser coverage and requested host/browser cases; retain all limitations and failed attempts.
- [ ] Record private disposition/evidence and update R06 acceptance state.
  - Verify: named owner/reference required for approval claims; all release gates remain separately assessed. No commit/publication without authorization.
