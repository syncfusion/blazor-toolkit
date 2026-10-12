# R06a — Local transport characterization and decision direction

**2026-10-06; exploratory dirty-tree source evidence, not remediation.** Base `address-warnings` / `19227619082433fa3b86e15d4e7aee625850aed9` plus preserved R02–R05 changes. [Decision/reason register](../../R06-DECISIONS.md) · [Design](../../R06-UPLOADER-TRANSPORT-DESIGN.md) · [Controlling plan](../../ENTERPRISE-READINESS-PLAN.md#9-execution-log-and-restart-record).

User authorized the next step and delegated recommended decisions. This pass performs **bounded local characterization only**. Exact new API/behavior contracts, host cleanup guarantees, private security and release approvals remain pending. R06 remains unchecked; its credential behavior is **not fixed**.

## Harness and safety boundaries

New opt-in `tools/transport-characterization/` contains a separate Playwright configuration and managed test project. It is outside existing browser/main bUnit CI discovery. `observations` deliberately describes current defects; `proposed-policy` contains genuine failing safety assertions. Do not preserve unsafe behavior to keep characterization green.

The new test-host Razor route is disabled unless `R06_CHARACTERIZATION=1`. The dedicated configuration starts a fresh host with that flag and refuses server reuse. Normal host use does not render the uploader. This is an accident-prevention guard, not authorization or a network sandbox.

Only dummy defaults, dummy explicit headers and 4/3,072-byte synthetic files are used. HTTP save/remove requests are intercepted locally with 200/500 replies; no upload/storage endpoint or external service was used. Other HTTP origins are blocked; service workers disabled. SignalR uses the local host; no claim of universal WebSocket/network isolation. The supplied fixture client has a counting/throwing handler.

Held callbacks use independent release buttons; held responses are released separately. Test-only wrappers observe original JS continuation return and XHR send/loadend without replacing transport decisions. Cancellation absence is checked after the original continuation consumes the returned decision. Teardown releases both gates and waits for tracked XHR loadend; this is **not** production lifecycle verification.

## Executed observations

| Scope | Result | What it establishes |
|---|---|---|
| Managed .NET 8, opt-in project | **8 passed, 0 failed/skipped** | Four auto/persistence combinations expose dummy defaults in actual initialize/parent-update arguments without client sends; supplied handler not disposed by uploader. Four explicit Task-returning callback bridges remain pending until release, then return dummy headers/cancel. No configured SaveUrl in these managed cases. |
| Browser, reviewed final run | **11 cases × 2 = 22 passed**, no skipped/flaky/retries | Current behavior observations below reproduced twice; not safe behavior. |
| Proposed safety policy | **2 failed, 0 passed/skipped** | Default export absent and no send while upload-start decision pending both fail on unchanged library. These are red controls, not expected-failure annotations or a CI pass. |
| Focused strict TypeScript | **Pass** | ES2022/node16 using the existing temporary pinned `@types/node@22.10.2`; no dependency/lockfile change. |
| Source preservation | **511 non-generated `src` files byte-identical to retained R05 source** | No R06 production implementation; API guard/tooling manifests unchanged. |

Browser findings:

- Before any selection, browser options contain dummy backend defaults. Changing client defaults plus tracked parent CSS changes resends updated values. The subsequent positive upload request contains those defaults and the explicit early-event dummy header. Fixture `HttpClient` handler remains unused.
- `BeforeUpload` and ordinary `BeforeRemove` hold dispatch while pending; after release each noncancel case produces one matching request with its explicit header. Cancel cases produce zero matching requests after original browser continuation consumption.
- Ordinary `OnUploadStart` and `OnRemove` can remain pending while a request is already sent. Releasing with cancellation cannot erase observed dispatch. Their current requests contain the earlier event's header, not the later return value.
- Three chunks produce indices `[0,1,2]`; one controlled failure at index 1 produces `[0,1,1,2]`. Three chunk-success entries and one final upload success occur. Chunk 0 uses upload-start, not chunk-start; this differs from the proposed uniform per-attempt event order.
- Chunk 2 can dispatch while its managed chunk-start callback is still pending. This disproves an assumption that all chunk callbacks are already safely awaited.
- Direct removal sends one remove request and runs `BeforeRemove`, while bypassing `OnRemove`.

Environment: SDK 10.0.401, net8 managed tests, net10 Interactive Server Release test host; retained macOS arm64 environment, Node 26.10.0, Playwright 1.63.0 / Chromium 153.0.8010.12. Selected Desktop Chrome device emulation does not constitute a Windows run. Tooling reused from the prior installed R04 worktree; themes generated in the isolated source snapshot.

## Failed harness attempts and limits

- Initial managed compile failed due to an ambiguous expression-lambda/dispatcher shape. A subsequent run had four false bridge failures because the old bUnit builder selected async-void delegates. Explicit `EventCallback.Factory.Create` with `Func<T, Task>` fixed the harness; all eight then passed. These are not library await defects.
- Initial request capture was too broad and included newly generated local antiforgery cookies. Corrected records retain only dummy-prefixed known header fields. **Initial raw logs/attachments are excluded from the curated archive**, remain locally restricted, and require owner retention/disposal handling. No claim that no sensitive data appeared anywhere. The curated archive was checked for the local antiforgery cookie-name marker; this is not a general secret-scan certification.
- Review prompted an environment opt-in fixture guard, centralized teardown and browser decision-consumption checkpoints. Final reruns use those changes. Before-guard runs are not evidence of guarded fixture behavior.
- Editor test discovery did not find the new separate managed project; documented terminal fallback was used.
- No full bUnit/library-all-TFM/API rerun was needed for a claimed production change because none occurred. Builds required by the actual host/managed tests ran; no general zero-warning/build-matrix claim is made.

**Not characterized:** child-only configuration update identifiers, client replacement or mutation without tracked change, concurrent upload/removal headers, all callback counts, pause/resume, active cancel/cleanup, same-name/server reconciliation, endpoint changes, disconnect/remount, templates/auto/sequential browser paths, redirects/cookies/CORS behavior, WebAssembly/Auto, Firefox/WebKit or actual package consumption. This is a first bounded pass, not completion of plan slice 1 or proof of the proposed redesign.

## Reproduction

From canonical isolated source, with all output retained privately:

- `npx --no-install playwright test --config tools/transport-characterization/playwright.config.ts --project observations --repeat-each=2 --reporter=line,json --output=<evidence>/observations-reviewed-results`, with `PLAYWRIGHT_JSON_OUTPUT_FILE=<evidence>/observations-reviewed.json`.
- Same command with `--project proposed-policy`, no repeat and separate `policy-reviewed` paths; **exit 1 is the recorded negative result**.
- `dotnet test tools/transport-characterization/Managed/TransportCharacterization.csproj -c Release -p:CycloneDxEnabled=false --logger 'trx;LogFileName=r06-managed-verified.trx' --results-directory <evidence>/managed-verified`.
- Focused `tsc --noEmit --strict --types node --typeRoots <temporary>/node_modules/@types --target ES2022 --moduleResolution node16 --module node16 --skipLibCheck` on the two harness TypeScript files.

## Private evidence and tested identities

Local reference **R06A-LOCAL-20261006**, curated `characterization-reviewed.tar.gz`, SHA-256:

`ac9f36d9d47becfc835466ee05c5a98b685432753d72d6103f7a35d26dac32a5`

Stored under current-user Code Studio application-support `readiness-evidence/2026-10-06-r06a-1922761/` (0700 directory; archive 0600). Includes only reviewed sanitized browser runs, policy failures, verified managed log/TRX, typecheck and tested harness sources. Working investigation is `/private/tmp/blazor-toolkit-r06-BEE9yC` with owner-only access and restricted evidence. Private owner/store/retention/disclosure approval remains unassigned; local permissions are not an organizational security disposition.

| File | SHA-256 |
|---|---|
| `tools/transport-characterization/playwright.config.ts` | `1faa6eb4ffa8640e50c9a6b1750f4339a2afc5407a4e4267b862a87fbb7e7a88` |
| `tools/transport-characterization/uploader.characterization.ts` | `c6c070c19c10abb4864dacb1eb5337a6df2b7af1268b42895d436b2143ec5be9` |
| `Managed/TransportCharacterization.csproj` | `d731ab0c188a0a073bae1c04e1b94e1b75c3660650aec9be052c0f90393b61e1` |
| `Managed/UploaderTransportCharacterization.cs` | `8d99b105c8733ffe1825e2dee245625349e6866b2173e44eb9edfbc5e9d7f400` |
| Test-host `TransportCharacterization.razor` | `3ed53e6cc641b3734598e645473c13ded91f1dd3d1fee2d0a148713f3d7880f8` |

All five tested sources match the user checkout. Existing user/R04/R05 changes preserved; no production or API baseline mutation, commit, push, signing, publication, remote-setting change or external security submission.

**Next:** finish bounded characterization and exact contract specification, then API/host owner review before production changes. [Decisions](../../R06-DECISIONS.md) explain selected recommendations and remaining approvals. R04/R05/R06 and all final release gates remain open.
