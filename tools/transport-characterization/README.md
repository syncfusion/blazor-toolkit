# R06 local transport characterization (opt-in)

This is an investigation harness, **not a production safety test suite or an approved transport contract**. It intentionally records current defects. [Decision log](../../docs/R06-DECISIONS.md) and [evidence](../../docs/evidence/2026-10-06-r06a-1922761/index.md) state exact limits.

- `observations` asserts measured current behavior; these assertions must be replaced with approved safe-behavior regressions when implementation starts. Never retain a vulnerable behavior just to keep observations passing.
- `proposed-policy` is expected to fail against the current source. It contains real assertions, not skips or expected-failure annotations. A nonzero exit is red evidence, not CI success.
- The scripts are `.characterization.ts` under `tools`, outside normal `tests/**/*.spec.ts` discovery. The managed project is also separate from the normal bUnit project. No CI workflow was changed.
- The sample fixture renders only when the test host explicitly sets `R06_CHARACTERIZATION=1`; this configuration supplies it. Normal host execution renders a disabled message. The opt-in is an accident-prevention guard, not authentication.
- Port 5000 must be free; `reuseExistingServer=false` prevents silently testing another checkout.
- All values/files are synthetic. Ordinary matrix requests are intercepted; live pause and endpoint-guard cases reach only the approved bounded loopback handler, never a storage service. Browser routes abort external HTTP origins; service workers are blocked and traces disabled. This is not a complete network sandbox.
- Records retain only known dummy-prefixed `X-R06-*` and Authorization values, paths and chunk indices; not cookies, full bodies or unrestricted headers. Do not reuse this harness with real credentials or customer data.

## Explicit commands from repository root

- Observe: `npx --no-install playwright test --config tools/transport-characterization/playwright.config.ts --project observations --reporter=line,json --output=<private-evidence>/observations-results`.
- Red policy: same command with `--project proposed-policy` and separate output. Set `PLAYWRIGHT_JSON_OUTPUT_FILE` to a separate private file for each run.
- Repeat original observations: explicitly select `uploader.characterization.ts` and add `--repeat-each=2` (11 unique cases, 22 executions). Do **not** repeat the live pause case in the same host.
- R06c established observations: select `uploader.characterization.ts matrix.characterization.ts parent-updates.characterization.ts --grep-invert 'endpoint-|completed-file removal' --repeat-each=2` (54 executions). This explicitly excludes **two blocked endpoint probes and three failing removal-overlap cases**, not a complete passing matrix. R06b's 44-case count belongs to its retained older source.
- Retained red overlap: `matrix.characterization.ts --project observations --grep 'completed-file removal'` (three failures). Endpoint probes remain source-controlled and historically red, selected separately with `--grep 'endpoint-'`; option B keeps their retargeting scope blocked, not silently passed.
- Live pause: explicitly select `pause.characterization.ts`, once per fresh host. The approved loopback endpoint is Development/opt-in only, bounded to 8 KiB/16 admitted requests/15-second waits, memory-only exact dummy chunks. It has process-lifetime state and must not share a host with budget tests.
- Later/final pause: use `--config tools/transport-characterization/pause-variants.config.ts`, a fresh process for each `R06_HOLD_CHUNK=1|2` and `R06_RESUME_PENDING=0|1` combination. Three observed passes include duplicate completion; pending-final resume genuinely fails. These cases have no `@observed` tag and are not selected by the base observation project. No same-host repetition.
- Endpoint guards: use `--config tools/transport-characterization/guard.config.ts`; repeat in a new process with `R06_VERIFY_DISABLED=1` to check the disabled host.
- Managed: `dotnet test tools/transport-characterization/Managed/TransportCharacterization.csproj -c Release -p:CycloneDxEnabled=false --logger 'trx;LogFileName=r06-managed.trx' --results-directory <private-evidence>/managed`.

Use a canonical isolated source worktree, required SDK/runtimes, installed Node dependencies and generated themes. SBOM opt-out is explicit source-test isolation, not SBOM verification. R05 found missing repository Node declarations; focused strict checking currently requires separately pinned temporary declarations as described in the evidence record.

## What is measured

Initialization/parent-update default-header export; supplied-client handler/ownership; awaited managed bridges; early upload/remove callbacks; ordinary late callbacks; three chunks; one automatic retry; a pending later-chunk callback; direct removal.

Managed bUnit callbacks use explicit `Func<T, Task>` to avoid an async-void overload in the old harness. Browser observation wrappers call the original synchronous continuation unchanged, recording return only to distinguish decision consumption from managed-handler release. XHR `send`/`loadend` accounting is test-only; no library function is replaced with a mock transport. Teardown releases callback and response gates and waits for pending XHRs; this is not proof of production lifecycle correctness.

R06a did not measure live pause/resume, concurrent credentials, child-only updates or client replacement. [R06b](../../docs/evidence/2026-10-07-r06b-1922761/index.md) records bounded observations for those paths and manual retry. Two endpoint probes remain failed/blocked by the missing child export and circuit termination; never add a product shim or suppress them to make characterization pass.

[R06c](../../docs/evidence/2026-10-07-r06c-1922761/index.md) adds ordinary parent renders, retry byte digests/exhaustion, clear/reselection, removal header/form overlap and later/final pause variants. Option B keeps endpoint retargeting/dependent destination changes blocked/out of phase. Three overlap-completion failures and pending-final resume remain genuine red outcomes; the passing pending-later case intentionally records duplicate success. Dummy-value redaction fails value-free on unexpected probe values rather than silently reporting them absent.

Still not established: exhaustive callback veto/rejection/interleaving/outcome schedules, cancellation cleanup server effects, same-name reconciliation, broader endpoint mutation, disconnection/remount, remaining automatic/sequential/template/hidden-list paths, redirects, package/other browsers/render modes or all event multiplicities. The owner approved the bounded dummy endpoint only; host/storage guarantees and XHR cookie/redirect policy remain explicitly unapproved. No production source changes are authorized; matrix completion and formal API checkpoint remain held.
