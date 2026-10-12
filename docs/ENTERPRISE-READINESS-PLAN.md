# Blazor Toolkit — Enterprise Readiness and Chart-First Plan

> **Purpose:** A standalone requirements summary, ordered implementation checklist, and session handoff. Start from this file in a fresh checkout; prior chat history is not required.
>
> **Created:** 2026-10-02. **Status:** R00–R03 are complete within their recorded source/documentation scope. R04–R25 and all final release gates remain open. No task is complete merely because this document exists.
>
> **Security handling:** Some findings describe potentially security-sensitive defects. Revalidate and triage through `.github/SECURITY.md` before publishing detailed reproductions or this assessment externally. This file contains no exploit payloads.
>
> **Current baseline (2026-10-06):** Base `address-warnings` / `19227619082433fa3b86e15d4e7aee625850aed9` plus **uncommitted** approved changes. [R03](evidence/2026-10-06-r03-1922761/index.md) establishes the released/API guard and authorized source Version=2.0.0. [R04](evidence/2026-10-06-r04-1922761/index.md) Chart measurement and [R05](evidence/2026-10-06-r05-1922761/index.md) uploader metadata source fixes are verified; both remain open pending named private security review. User approved the recommended original-filename behavior correction and review hold. See [migration policy](API-COMPATIBILITY.md) and [§9](#9-execution-log-and-restart-record). No 2.0 release occurred. R06 production implementation awaits exact contract review; the user authorized characterization and delegated recommendations. [R06a](evidence/2026-10-06-r06a-1922761/index.md) reproduces export/timing defects; [decisions/reasons](R06-DECISIONS.md) record owner approvals still needed. No credential behavior changed. **2026-10-07:** [R06b](evidence/2026-10-07-r06b-1922761/index.md) adds owner-approved local characterization and a bounded test endpoint; two endpoint probes remain failed/blocked. API/migration checkpoint, host guarantees and XHR policy remain held. Planned CSS isolation is separate; enterprise readiness is not established.
>
> **Latest continuation (2026-10-07):** [R06c](evidence/2026-10-07-r06c-1922761/index.md) records option B: endpoint retargeting remains blocked/out of phase, no production export repair. Established observations repeat 54 passes; three removal-overlap probes and pending-final resume fail separately. Matrix remains incomplete; continue unblocked cases before the formal contract checkpoint. All owner/release holds above remain.
>
> **Documentation synchronization (2026-10-03):** See [§11](#11-documentation-sync-progress--2026-10-03) for historical local source dispositions and approved documentation scope. The original audit basis and finding register remain preserved; that synchronization did not complete a readiness gate.
>
> **Closure navigation:** [§12 — Remaining-issue closure plan](#12-remaining-issue-closure-plan) · [§9 — Authoritative execution log](#9-execution-log-and-restart-record) · [Manual/external evidence tracker](MANUAL-EXTERNAL-EVIDENCE-TRACKER.md) · [Release checklist](RELEASE-CHECKLIST.md). Use the companion procedures and blank records to collect evidence; their existence does not establish completed gates.

## 1. Agreed product direction

- **Chart is the flagship.** The longer-term roadmap includes 40+ supporting/tool components; their exact scope and delivery dates require product approval.
- **No Grid is planned.** Do not implement, promise, or prioritize a Grid under this plan.
- The toolkit is a useful, independently adoptable, MIT-licensed introduction to commercial Syncfusion components—not a trial that becomes unusable without an upgrade.
- The objective is to become **a default choice for Blazor charts and a safe long-term dependency**, not necessarily the default complete application UI framework.
- Make adding Toolkit Chart to an existing application easy, including applications using MudBlazor. Commercial adoption should follow positive experience and demand for additional capabilities/support.
- Maintain a clear distinction between the MIT toolkit and the commercial suite's licensing. Do not promise identical APIs, a package-only swap, or zero-rewrite migration without tested evidence.

**Positioning to earn:** “A production-grade, MIT-licensed Blazor Chart and supporting toolkit—useful on its own, backed by Syncfusion, with an optional path to the commercial suite.”

**Out of scope:** new components, Grid development, a commercial-suite rewrite, licensing changes, unapproved public delivery commitments, speculative claims about competitors, and broad cosmetic/analyzer cleanup unrelated to an identified risk.

## 2. Audit baseline and limitations

The source audit covered a **dirty local working tree**, not a reproducible release:

| Item | Recorded baseline |
|---|---|
| Repository | `https://github.com/syncfusion/blazor-toolkit` |
| Local branch | `main` |
| Base commit | `5c11f9c619dce4ec807ceb66d04de46d5da60eff` |
| Audit date | 2026-10-02 |
| Local changes at handoff | 212 tracked-change entries and 41 untracked entries before adding this plan |
| Locally visible tags | `v1.0.1`, `v1.0.2`; not proof of complete remote release history |
| Historical URL supplied earlier | `address-warnings` branch; do not assume it is the audited checkout or the correct implementation base |

The audit read source, tests, documentation, build configuration, and workflows. **It did not execute builds, tests, vulnerability scans, benchmarks, or exploit reproductions.** GitHub rulesets, private signing procedures, remote CI results, published-package contents, and customer deployments were not verified.

A fresh checkout may lack local fixes, source-generated contexts, generated assets, documentation, or the exact defects seen here. Every finding must be classified again against the selected commit. Existing documents that claim a pass are not substitute test evidence.

### Known corrections to earlier generated material

The previous session created or changed local `README.md`, `ROADMAP.md`, `CHANGELOG.md`, and issue templates. Some may be absent from a fresh checkout. Where present, review them rather than assuming they are approved:

- Remove the unapproved Grid-first roadmap and invented feature/delivery commitments.
- Reconstruct release history from real tags/releases. The inferred `1.0.0` release date of `2026-10-01` was not verified.
- Replace blanket WCAG, AOT, SBOM-shipping, and zero-rewrite claims with bounded, evidenced statements.
- Do not repeat unsupported comparative claims about MudBlazor's security, accessibility, or performance.
- Feature requests should not be blocked merely because an item is already on the roadmap; support useful additional scenarios and references to existing requests.

## 3. Requirements and evidence rules

| ID | Requirement | Evidence required before claiming completion |
|---|---|---|
| REQ-01 | Honest chart-first scope and licensing | Approved scope, verified license/package metadata, accurate roadmap/release history |
| REQ-02 | Safe handling of untrusted data | Threat-boundary review, negative tests, high-risk surface review |
| REQ-03 | Reliable component lifetime and tenant isolation | Disposal, cancellation, concurrency, reconnect, and resource-budget tests |
| REQ-04 | Trustworthy regression protection | Runnable tests with meaningful assertions, discovered counts, failure artifacts |
| REQ-05 | Explicit platform compatibility | Executed framework/render-mode/browser matrix for supported combinations |
| REQ-06 | Safe, predictable upgrades | Public API baseline, package compatibility checks, documented exceptions/deprecations |
| REQ-07 | Verifiable shipped artifacts | Clean package-consumer tests, SBOM validation, traceable provenance and signature checks |
| REQ-08 | Accessible supported scenarios | WCAG 2.2 AA assessment with automated/manual evidence and accurate limitations |
| REQ-09 | Measured Chart performance and correctness | Reproducible benchmark results and edge-case/interaction tests |
| REQ-10 | Frictionless adoption and optional expansion | Compile-tested examples, coexistence tests, accurate commercial integration guide |
| REQ-11 | Sustainable maintenance | Named ownership, support/EOL policy, funded security response, retained release evidence |

For every claim distinguish **implemented**, **tested on this commit**, **tested on the release artifact**, **documented only**, and **unknown**. No badge, configuration flag, test count, or document title establishes conformance by itself.

**Responsibility boundary:** the library owns safe rendering, bounded component resource use, interop hygiene, and accurate contracts. Host applications own authentication, authorization, tenant data access, upload-endpoint validation/storage, server credentials, and deployment policy. Examples must demonstrate the boundary without claiming the library secures the entire application.

## 4. Fresh-checkout and session protocol

### Before implementation

1. Preserve this Markdown file outside the current worktree or carry it in an approved documentation commit. A fresh clone will not contain an uncommitted file. Do not copy the entire dirty worktree as the baseline.
2. Select the intended remote branch and exact commit with the maintainer. Record the commit, working-tree status, OS, SDKs/workloads, Node/npm, and relevant tool versions.
3. Read repository instructions and contribution/security policies available in that checkout. Resolve relative paths below from the repository root; no machine-specific paths or prior agent tools are required.
4. Revalidate each finding when its task begins. Classify it as `reproduced`, `source-confirmed`, `not present`, `already fixed`, `blocked`, or `needs investigation`. Do not reintroduce old code to make the audit fit.
5. Establish clean build/test baselines. Missing dependencies or test-discovery failures are blockers, not passing results. Keep secrets out of commands, logs, issues, and this document.

### One task per session

- Take the lowest-numbered incomplete, unblocked task or substep whose dependencies are satisfied. For externally blocked work, record the owner and unblock condition, then continue with the next eligible independent task; never mark the blocked work complete. Record a different order if urgent security remediation must proceed sooner.
- Mark it `in progress` in the execution log. Read current implementation and tests before editing.
- For behavior fixes, write a regression test that fails before the fix, then implement the smallest change that makes it pass. If the main harness is blocked, use a focused approved harness and record the limitation.
- Preserve public API/serialization/JS contracts unless a change is explicitly approved. Split independent fixes and formatting into separate reviewable changes.
- Run targeted verification, relevant wider regressions, and review the diff. Do not disable tests, weaken assertions, or broaden suppressions to obtain a green result.
- Update the checkbox only after the acceptance gate passes, or after a documented not-applicable/already-fixed disposition is reviewed. A deferred item remains incomplete.
- Record commit/PR or reviewed diff, exact commands/tool actions, test counts, evidence locations, limitations, and next task. Stop at the task boundary unless asked to continue.
- Do not commit, push, tag, publish, change remote settings, or discard user changes without authorization. Never print signing/publishing credentials.

**Sizing:** each numbered task is a bounded work package. If it exceeds one focused session, create ordered substeps such as `R07a` and `R07b` inside its section and the execution log. In particular, platform matrices, accessibility remediation, and coexistence testing must be sliced by scenario rather than implemented as one large PR.

## 5. Finding register: leads to revalidate, not release assertions

Paths are relative; use symbols because line numbers will drift. `PW` below means `tests/Syncfusion.Blazor.Playwright.Test/Blazor.Toolkit.playwright.Test/`; `BU` means `tests/Syncfusion.Blazor.Toolkit.BUnitTest/`.

| Finding | Source-level observation at audit time | Primary locations | Tasks |
|---|---|---|---|
| F01 | Chart measurement can interpret data as HTML; line-break handling can bypass replacement. An additional non-Latin measurement path uses HTML. | `src/wwwroot/scripts/svgbase.js` (`measureText`); `src/wwwroot/scripts/chart.js`; `src/Components/Charts/Common/ChartUtils/ChartHelper.cs` | R04 |
| F02 | AJAX-rendered preloaded filenames reach HTML assignments without passing through the new-file selection sanitizer. | `src/wwwroot/scripts/uploader.js` (`renderPreLoadFiles`/preload path, `createFileList`); Razor uploader list is a separate path | R05 |
| F03 | Uploader copies all supplied HTTP-client default headers into JavaScript initialization options. Credential exposure depends on what the application supplies. | `src/Components/Inputs/Uploader/SfUploader.razor.cs` (`GetCustomHeaders`, `GetProperty`); `SfUploader.razor.Members.cs` | R06 |
| F04 | Eager uploader buffering lacks a demonstrated aggregate file-count/byte budget; a fallback opens a stream with `long.MaxValue`. | `SfUploader.razor.cs` (`CacheFilesAsync`, selection/removal/progress paths) | R07 |
| F05 | Base disposal sets `IsDisposed` before Chart cleanup, but `UnWireEventsAsync` requires it to be false. Separately, Tooltip removes listeners using newly bound functions. | `src/Base/SfBaseComponent.cs`; `src/Components/Charts/Chart/SfChart.razor.cs`; `SfChart.razor.LifeCycle.cs`; `src/wwwroot/scripts/tooltip.js`, `base.js` | R08 |
| F06 | Browser measurements/global layout state can be shared across Server circuits; scoped service holds and mutates a shared options object. | `ChartHelper.cs`; `src/Components/Charts/Chart/Renderer/AxisRenderer/Layout/CartesianAxisLayout*.cs`; `src/Base/SyncfusionService.cs` (`EnableRtl`) | R09 |
| F07 | Observable-data ownership and stale/discarded async work need lifecycle review; some interop callbacks lack adequate input checks. | `SfChart.razor.LifeCycle.cs`, `SfChart.razor.cs`, `SfChart.razor.JsInterop.cs`; `src/Data/` for reachable paths | R10 |
| F08 | Local development notes report nested generated-source duplication and five test-source collection-type compilation errors. | `BU/Syncfusion.Blazor.Toolkit.BUnitTest.csproj`; `Buttons/ButtonGroup/`; `Popups/Dialog/SfDialogTest.cs`; Chart `SharedTooltip.razor`, `Crosshair.razor`, `Events.razor`; `.github/DEVELOPMENT.md` | R02 |
| F09 | Several tests assert visibility/count >= 0, conditionally skip assertions, swallow failures, or assert hardcoded coverage counts. | `PW/Charts/Events/legend.spec.ts`; tooltip/zoom/keyboard specs; `PW/Charts/Series/series-coverage-index.spec.ts`; TextBox specs | R11 |
| F10 | Documented test matrix exceeds configured execution: bUnit net8, browser host net10 Server, Chromium only. No published WASM-AOT test found. | `.github/workflows/ci.yml`; `.github/TEST-MATRIX.md`; test `.csproj` files; `playwright.config.ts`; `samples/Blazor.Toolkit.Samples.Client/LinkerConfig.xml` | R13–R15 |
| F11 | SBOM generation is disabled under `CI=true`; multitarget pack hooks do not establish package inclusion; contract test checks documentation words. | `Directory.Build.targets`; `artifacts/sbom/`; `BU/Base/SbomContractTests.cs` | R16–R17 |
| F12 | API-baseline/provenance promises lack corresponding automated enforcement in inspected configuration. | `.github/DEVELOPMENT.md`; `Directory.Build.props`; library project; workflows | R03, R16, R18 |
| F13 | Product JavaScript is excluded from ESLint/custom XSS checks. CodeQL JavaScript is configured, so analysis is not wholly absent. | `eslint.config.mjs`; `gulpfile.js`; `.github/workflows/codeql.yml` | R12 |
| F14 | VPAT misclassifies Focus Appearance as AA and uses keyboard-only alternatives as evidence for Dragging Movements. Referenced accessibility reports/automation were not located. | `docs/VPAT-2.5-INT.md`; `.github/ACCESSIBILITY.md`; `.github/accessibility/insights-summary.md` | R21–R22 |
| F15 | Chart onboarding mixes toolkit package/services/assets with commercial APIs; some documented methods/parameters differ from implementation. | `.github/skills/syncfusion-blazor-toolkit-charts/references/`; README and samples | R20 |
| F16 | Chart optimizations exist, but no dedicated reproducible performance/reliability baseline was located. A timing test starts after navigation/network idle. | `PW/Charts/Series/LineCharts/multicolored-line-basic-rendering.spec.ts`; `.github/evidences/performance/` | R23 |
| F17 | Security docs describe an HTTP/render-mode enforcement mechanism not found in implementation. Render mode is not an authorization boundary. | `.github/RENDER-MODE-SECURITY.md`; `BU/Base/RenderModeSecurityTests.cs`; `.github/THREAT-MODEL.md` | R01, R19 |

**Do not overgeneralize:** unsafe-looking APIs are not automatically exploitable; fixed markup and encoded Razor paths can be safe. Reachability, trusted/untrusted input, host configuration, and browser behavior must be established. The audit did not establish reachable DataManager SSRF, credential leakage in a deployment, or cross-user data disclosure.

## 6. Ordered implementation checklist

Priority denotes release risk, not a claim of confirmed vulnerability severity. All work starts with R00. Listed dependencies are minimum prerequisites; the default is numerical order.

### Phase A — Establish a truthful, runnable baseline

- [x] **R00 — Revalidate the checkout and record a baseline**
  - **Depends on:** none. **Priority:** P0. **Scope:** small, read-only investigation first.
  - Record selected commit and environment; inventory workflows, all test projects, package consumers, and actual release tags. Run build and test discovery/execution using repository guidance; distinguish existing failures from environmental setup failures. Inventory findings F01–F17 without reproducing attacks against external systems.
  - **Gate:** append a baseline record below with exact results, failure classifications, source/release scope, and next task. Do not claim a clean baseline unless it is measured.
  - **2026-10-06 result:** investigation completed on the user-selected commit in an initially clean isolated worktree; [evidence and classified failures](evidence/2026-10-06-r00-1922761/index.md). This is not a clean default-build, package or readiness pass. R01 is next.

- [x] **R01 — Correct scope and unsupported assurances**
  - **Depends on:** R00. **Priority:** P0. **Scope:** documentation-only changes.
  - Review `README.md`, any `ROADMAP.md`/`CHANGELOG.md`, issue forms, security/render-mode claims, test matrix, and accessibility banners. Keep Chart flagship and supporting components as approved direction; no Grid promise. Replace unverified claims with precise limitations. Preserve legitimate historical release facts.
  - **Gate:** every public assurance links to evidence or is explicitly a target/limitation; release dates come from verified history; roadmap is product-approved. Remove nonexistent enforcement claims without inventing replacement guarantees.
  - **2026-10-06 result:** [reviewed documentation and evidence](evidence/2026-10-06-r01-1922761/index.md); user explicitly approved Chart-first/MIT/no-Grid scope with exploratory 40+ direction and no dates/list. Pre-existing edits preserved; remaining SBOM/quality assurances corrected. 201 local links/anchors, two issue forms and two documentation-contract tests passed. No implementation, package or conformance gate closed.

- [x] **R02 — Restore regression-test execution**
  - **Depends on:** R00. **Priority:** P0. **Scope:** test project configuration, then test-source fixes as separate substeps.
  - Recheck F08 on a clean checkout. Correct nested source/output inclusion and collection-type mismatches only where present; do not revert intentional production API changes just to satisfy stale tests. Check that tests intended for the toolkit do not accidentally run against a wildcard commercial dependency.
  - **Gate:** intended bUnit tests compile, are discovered with nonzero counts, and execute; targeted and full results are recorded. New failures are diagnosed, not hidden by skips. Establish a minimal executable Chart browser regression target if browser setup is blocked.
  - **2026-10-06 result:** [R02 evidence](evidence/2026-10-06-r02-1922761/index.md): standalone ButtonGroup now references Toolkit source, not wildcard commercial package; regression failed before/passed after. Main 2,738 discovered/passed in Debug and Release, standalone 36 discovered/passed, no skips; one existing Chart SVG/title/series/point/legend browser case passed. F08 fixes retained. Default builds passed without compiler switches; R00 failure not reproduced, exact root cause unresolved. Warnings and assertion-quality work remain R11, not waived.

- [x] **R03 — Establish a released API baseline**
  - **Depends on:** R00, R02. **Priority:** P1. **Scope:** API/package-validation configuration.
  - Verify the last appropriate released package, capture the public API, and introduce compatibility checks. Document already-existing differences separately from newly introduced changes; include Razor parameters, generic signatures, and serialization/JS contracts where automatic tools are insufficient.
  - **Gate:** a deliberate temporary API break fails validation; baseline matches a verified release rather than silently blessing the dirty tree. Any approved break has a migration/versioning decision. Do not delete test coverage to resolve compatibility failures.
  - **2026-10-06 result:** [R03 evidence](evidence/2026-10-06-r03-1922761/index.md), [migration policy](API-COMPATIBILITY.md), [194 exact approved exceptions](API-BREAK-INVENTORY.md). User approved named major changes, getter-only ownership and uncommitted 2.0.0 version diff in place of R03a's commit prerequisite. Pinned 1.0.2/three-TFM gate and reviewed contract guard pass Debug/Release; two compiled deliberate breaks fail and were reverted. 2,738 tests, nine verifier tests and two browser smoke cases pass. Runtime/package/CSS/AT/security release gates remain separate.

**Checkpoint A:** scope is truthful and tests are runnable. Review blockers before continuing; urgent security work can use focused regression tests without waiting for unrelated cleanup.

### Phase B — Address existing security and lifetime risks

- [ ] **R04 — Make Chart text measurement safe**
  - **Depends on:** R02; consult R03 for contract changes. **Priority:** P0. **Scope:** Chart JS measurement plus browser tests.
  - Trace F01 from category/series/tooltip data through normal and non-Latin measurement paths. Prefer text APIs/canvas; if formatting must remain, separate trusted formatting from substituted text and construct narrowly allowed nodes.
  - **Gate:** regressions prove untrusted input cannot create executable markup or unexpected requests; supported line breaks, Unicode, RTL, and sizing still work. Prove the test fails on the vulnerable path before the fix where reproducible. Record private security disposition.
  - **2026-10-06 partial result:** [R04 record](evidence/2026-10-06-r04-1922761/index.md): safe text/explicit formatting implemented; final old-sink control 6 failed/3 passed, fixed nine new cases plus two smoke cases repeated = 22 passed; 2,738 bUnit and three-TFM API gate pass. **Remains open:** user selected recommended hold pending named private security-owner review/disposition. No released-package remediation, disclosure or risk approval claimed.

- [ ] **R05 — Render uploader metadata as text**
  - **Depends on:** R02. **Priority:** P0. **Scope:** uploader JS plus browser tests.
  - Address F02 in preload and selection paths, both AJAX and Razor rendering. Filename, extension, and ordinary status rendering must not depend on an optional sanitizer setting to be safe.
  - **Gate:** benign and hostile metadata cases cover both routes, templates as a separate trust boundary, and sanitizer on/off behavior. Tests assert DOM behavior, not merely absence of a literal script string.
  - **2026-10-06 partial result:** [R05 evidence](evidence/2026-10-06-r05-1922761/index.md): text sinks and inert validation implemented; user approved original-filename correction. Initial 9/20 and supplemental 4/4 cases fail before fixes; custom-element validation defect separately reproduced. Final 40-case matrix passes; combined repeated browser run 102 pass, bUnit 2,738 pass, three-TFM build/API gate pass. **Open:** named private security-owner disposition, as selected by the user. No release remediation claimed.

- [ ] **R06 — Clarify and constrain uploader credential forwarding**
  - **Depends on:** R02, R03. **Priority:** P0. **Scope:** API decision, implementation, documentation, negative tests.
  - Revalidate F03. Document where upload requests execute. Agree on an explicit browser-header policy; avoid silently breaking legitimate per-user authorization while preventing accidental export of privileged backend credentials. Server-side handler/timeouts must not be falsely promised for browser AJAX.
  - **Gate:** dummy privileged headers are absent from browser initialization/network traffic by default; explicitly permitted user-bound headers behave as documented. Any public API/security-default change receives maintainer review and migration guidance.
  - **2026-10-06 partial characterization:** user authorized the next step with recommended decisions delegated; [decision register](R06-DECISIONS.md) logs reasons and outstanding API/host/security/release approvals. [R06a evidence](evidence/2026-10-06-r06a-1922761/index.md): eight managed tests and 22 repeated current-behavior observations pass; two proposed safety assertions fail, reproducing export and late dispatch. **No production/API-baseline change.** Full characterization and exact production contract remain open; [design](R06-UPLOADER-TRANSPORT-DESIGN.md) is not implemented.

- [ ] **R07 — Bound upload resource use**
  - **Depends on:** R02, R03. **Priority:** P0. **Scope:** split budget enforcement, streaming/cancellation, and eviction if needed.
  - Review F04 across every stream path. Apply trusted maximum file count, per-file bytes, aggregate bytes, and concurrency before reading; never use untrusted reported size as the sole limit. Remove unlimited fallback reads, stream where feasible, and define same-name file behavior and eviction.
  - **Gate:** over-limit, repeated selection, cancellation, removal, clear-all, and disposal tests enforce the budgets and release retained data. Server resource tests show bounded growth under the declared scenario; host upload authorization/validation remains explicit.

- [ ] **R08 — Repair Chart teardown and listener ownership**
  - **Depends on:** R02. **Priority:** P1. **Scope:** Chart/base lifecycle first; Tooltip listener identity as a separate substep.
  - Fix F05 with idempotent cleanup and explicit ownership. Test derived cleanup ordering, JS registry removal, callbacks, timers/listeners, bridge/module disposal, and disconnected circuits. Consider browser-side DOM-removal cleanup where Blazor disposal cannot safely access the DOM.
  - **Gate:** repeated mount/unmount leaves no retained registrations/listeners attributable to the component; disconnected or repeated disposal is safe. Shared base changes pass regressions for other components. Do not merely suppress cleanup exceptions.

- [ ] **R09 — Isolate per-chart and per-circuit state**
  - **Depends on:** R02, R08. **Priority:** P1. **Scope:** split layout state, measurement cache, and options ownership.
  - Address F06. Keep browser-derived measurements local to their trust/lifetime scope; reserve global caches for deterministic trusted values. Validate measurement sizes/counts. Remove mutable static layout ownership and copy mutable runtime options into the proper scope.
  - **Gate:** concurrent circuits with different charts, fonts, cultures, and RTL settings cannot alter one another. Cache size/lifetime is bounded, and a modified client response cannot affect a second circuit's layout.

- [ ] **R10 — Harden async updates and callback inputs**
  - **Depends on:** R08, R09. **Priority:** P1. **Scope:** one reachable update/callback family per substep.
  - Recheck F07. Detach replaced observable sources; cancel owned async work on replacement/disposal; prevent stale results winning races. Validate client-supplied indices, counts, numeric values, and callback payloads. Trace remote-data paths before asserting network vulnerabilities; do not redesign unused adaptors speculatively.
  - **Gate:** delayed/out-of-order updates, malformed callbacks, collection replacement, navigation, and disconnect tests produce no stale mutation or unobserved task failure. Host handlers do not treat browser success callbacks as authorization or proof of persisted uploads.

**Checkpoint B:** high-risk findings have reproductions/dispositions and regression coverage; Chart lifetime/isolation tests pass. Unresolved security risk requires an explicit release decision, not a marketing waiver.

### Phase C — Make CI and platform evidence trustworthy

- [ ] **R11 — Replace tests that cannot catch regressions**
  - **Depends on:** R02. **Priority:** P1. **Scope:** Chart interaction tests first; adjacent weak tests in follow-up slices.
  - Replace F09's constant-only, conditional, and swallowed assertions with visible behavior: axis ranges, event payloads, selected points, focus location, accessible names, tooltip values, and series visibility. Repair incorrect routes and reviewed visual baselines. Enable focused-test protection; define skip/flaky-test policy and preserve traces on failure.
  - **Gate:** deliberately breaking each representative behavior makes its test fail. Report discovery/pass/fail/skip counts; no zero-test success or unexplained coverage inflation. Coverage and changed-code thresholds follow a measured baseline, not an arbitrary percentage alone.

- [ ] **R12 — Cover shipped scripts and dependencies with security checks**
  - **Depends on:** R02, R04, R05. **Priority:** P1. **Scope:** analysis coverage, then dependency controls.
  - Address F13 by analyzing shipped JS or its authoritative source with verified source-to-artifact correspondence. Audit NuGet across relevant projects and npm dependencies; review bundled scripts/licenses, not just package-manager graphs. Pin action commits and important tools, use locked restore policies where appropriate, remove wildcard dependencies, and key browser caches to the lockfile.
  - **Gate:** intentional failing fixtures prove gates run over the intended source/dependency set. Record actual advisory results and time-bounded exceptions. Keep existing CodeQL protections; do not claim zero vulnerabilities merely because a scanner is configured.

- [ ] **R13 — Define the supported platform and servicing contract**
  - **Depends on:** R00, R01. **Priority:** P1. **Scope:** policy and test-matrix design with maintainer approval.
  - Specify supported SDK/TFM/runtime, browsers, render modes per component, OS/device assumptions, culture/RTL, and patch/EOL policy. Separate build compatibility from tested production support. Record PR/nightly/release tiers and cost; do not require unsupported combinations to appear supported.
  - **Gate:** `.github/TEST-MATRIX.md` and support docs distinguish planned/configured/executed status. Verify current Microsoft support dates; .NET 8/9 were listed to end on 2026-11-10 and .NET 10 on 2028-11-14 at this audit. Prioritize .NET 10 and approve the older-target policy before EOL.

- [ ] **R14 — Execute the supported runtime/browser matrix**
  - **Depends on:** R11, R13. **Priority:** P1. **Scope:** one host/mode/browser slice at a time.
  - Make SDK selection and test target frameworks explicit. Add supported Server/WASM/Auto scenarios, prerender/hydration, enhanced navigation, reconnection, and Static SSR behavior where applicable. Add Chromium/Firefox/WebKit tiers plus real Safari/device manual coverage where promised.
  - **Gate:** recorded results identify actual runtime, mode, browser, and artifact—not merely installed SDKs. Representative Chart and helper flows pass with no unexpected console/network errors. Unsupported interactive behavior has a documented contract instead of a fake passing test.

- [ ] **R15 — Verify published trimming and WebAssembly AOT**
  - **Depends on:** R13, R14. **Priority:** P1. **Scope:** minimal consumers, then supported data-binding shapes.
  - Publish and execute realistic consumers; use Blazor WASM `RunAOTCompilation` rather than conflating it with server Native AOT. Test typed and documented dynamic/reflection-based Chart binding separately. Evaluate linker roots and suppressions; do not preserve everything merely to claim trimming safety.
  - **Gate:** supported published scenarios run; actionable trim/AOT diagnostics are resolved or narrowly documented with reproductions. Unsupported shapes are stated plainly. Later repeat these checks against the actual package in R16.

**Checkpoint C:** supported scenarios have trustworthy results; warnings and limitations are explicit. A clean analyzer report is not a replacement for browser/publish verification.

### Phase D — Verify what customers install

- [ ] **R16 — Validate the generated NuGet package in clean consumers**
  - **Depends on:** R03, R14, R15. **Priority:** P1. **Scope:** package inspection, then consumer hosts.
  - Build/pack in a clean controlled environment and install from a local feed without toolkit `ProjectReference`. Verify target assemblies, licenses/notices, XML docs, scripts/styles, SourceLink/commit identity, and generated asset freshness. Re-run representative render-mode and publish tests against the package.
  - **Gate:** clean consumers render and interact using only required documented assets; API compatibility passes. Package identity matches the built commit, including PR merge/head semantics. No local untracked assets mask missing package content.

- [ ] **R17 — Generate and verify release-specific SBOMs**
  - **Depends on:** R12, R16. **Priority:** P1. **Scope:** SBOM generation/packaging plus artifact tests.
  - Fix F11 using a pinned tool and correct multitarget pack integration. Define SBOM scope including shipped runtime dependencies and bundled assets; distinguish build-only dependencies. Generate from the same controlled build and validate schema, versions, checksums, freshness, and intended package/release placement.
  - **Gate:** tests unpack the package and validate the actual SBOM/attachments—not text in a Markdown policy. Missing or stale required SBOM evidence fails the release gate. The exact distribution contract is documented.

- [ ] **R18 — Verify release identity, signing, and provenance**
  - **Depends on:** R16, R17. **Priority:** P1. **Scope:** public verification first; credentialed operations require approval.
  - Review manual/private release controls with owners. Bind source commit, build inputs, artifact hashes, signing, provenance, and release tag. Distinguish assembly strong names from NuGet package signatures and build provenance. Retain protected signing if required; do not introduce credentials into public PR CI.
  - **Gate:** an approved release rehearsal verifies final package signatures and artifact identity, then repeats appropriate consumer checks after final packaging/signing. Record branch protection/required-check approval separately; do not assert a SLSA level without meeting the chosen specification's requirements.

- [ ] **R19 — Close the security verification and host-contract review**
  - **Depends on:** R04–R10, R12 for R19a; R18 for R19b only. **Priority:** P1. **Scope:** scoped review with a named security owner.
  - **R19a — implementation review:** map applicable OWASP ASVS requirements to library/host responsibilities and evidence. Review CSP behavior for Server/WASM separately, including necessary framework allowances, offline/self-hosted operation, unsolicited network traffic/telemetry, interop limits, upload examples, and reachable network adaptors. Correct F17. Do not delay this review while signing access is unavailable.
  - **R19b — release reconciliation:** once R18 is complete, reconcile the threat model and supply-chain claims with actual signing/provenance evidence. Track the two substeps independently in the execution log.
  - **Gate:** a reviewed risk register identifies resolved findings, exclusions, accepted risks with owners/expiry, and private disclosure status. Independent review covers Chart text/interop and uploader trust boundaries. R19 completes only when both substeps pass. No blanket “ASVS certified” or “fully secure” claim.

**Checkpoint D:** a release candidate is traceable, inspectable, installable, and verified. Private/admin steps remain blocked until an authorized owner supplies evidence; independent later tasks may proceed, but this checkpoint cannot be declared complete.

### Phase E — Make adoption and quality independently demonstrable

- [ ] **R20 — Compile and run the onboarding contract**
  - **Depends on:** R01, R16. **Priority:** P1. **Scope:** first-chart example, then guides/skills/templates.
  - Correct F15: package ID, namespace, service registration, assets, render mode, methods, localization, and data binding. Test template-based and manual setup against the package. Remove unintended commercial dependencies; do not mutate process-global culture from a user-scoped sample.
  - **Gate:** clean first-chart consumers compile and work by following published instructions. Selected Markdown/skill examples are executable test fixtures or generated from verified examples; documentation drift fails CI. Record time to first working chart from a fresh project.

- [ ] **R21 — Correct the accessibility assessment and evidence structure**
  - **Depends on:** R01, R13. **Priority:** P1. **Scope:** standards-correct assessment; no unsupported pass claims.
  - Reassess F14 against the current WCAG 2.2 text: 2.4.11 is AA, 2.4.13 is AAA, and 2.5.7 requires a non-dragging single-pointer alternative. Identify tested versions, themes, scenarios, browsers, assistive technology, report retention, and genuine limitations. Validate references to automation/reports.
  - **Gate:** criteria and applicability are correct; every support assertion has evidence or is marked not evaluated/partially supported as appropriate. Use “Accessibility Conformance Report” accurately; automated checks and a VPAT template are not certification.

- [ ] **R22 — Remediate and continuously test accessibility**
  - **Depends on:** R11, R14, R21. **Priority:** P1. **Scope:** one Chart interaction/criterion, then one helper component per slice.
  - Automate appropriate accessibility checks and meaningful keyboard/focus tests. Add manual NVDA/VoiceOver and additional supported combinations. Cover Chart descriptions/data alternatives, non-color encoding, zoom/drag alternatives, tooltips, reduced motion, forced colors, contrast, and responsive/zoomed layouts. Measure relevant interactive states, not only initial render.
  - **Gate:** applicable agreed AA scenarios meet the criterion or have an explicit unresolved limitation; evidence is tied to the candidate build. Manual records include scenario, steps, environment, outcome, and reviewer. Fixes do not regress pointer functionality or localization.

- [ ] **R23 — Publish a reproducible Chart correctness/performance baseline**
  - **Depends on:** R08–R11, R14, R16. **Priority:** P2. **Scope:** correctness fixtures, benchmark harness, then budgets/results.
  - Test empty/null/NaN/infinite data, duplicate categories, negative/logarithmic values, date/time and culture boundaries, resize/hidden containers, and rapid updates. Benchmark initial usable render, incremental/full updates, tooltip/selection/pan/zoom, multiple charts, and long-duration teardown. Use representative 1k/10k/100k-point scenarios only where supported; disclose sampling/point reduction and features enabled.
  - **Gate:** report median/p95, incremental payload over an equivalent empty app, browser heap/DOM and server-per-circuit growth, and interop traffic with reproducible inputs/hardware/version details. Timing starts before the operation measured. Approve budgets from measured baselines and enforce regression checks; do not invent universal performance targets or unfair competitor comparisons.

- [ ] **R24 — Prove coexistence and the commercial transition**
  - **Depends on:** R16, R20. **Priority:** P2. **Scope:** track R24a and R24b separately.
  - **R24a — general coexistence:** demonstrate Toolkit Chart in another UI library's application shell. Test CSS/theme isolation, shared helper classes, script globals, service registration, namespaces, and localization.
  - **R24b — optional commercial validation:** with approved commercial access, test adding a commercial component without replacing Toolkit Chart, then document actual migration differences. Do not publish credentials or proprietary artifacts.
  - **Gate:** version-pinned examples run without CSS/JS/service collisions. Document what is compatible, what changes, and licensing boundaries; no unsupported “NuGet swap” promise. R24b stays explicitly blocked if authorized dependencies are unavailable; that blocks commercial claims, not independent toolkit readiness. Mark R24 complete only after both substeps pass or an approved scope decision removes R24b and its claims.

- [ ] **R25 — Operationalize maintenance and publish the evidence index**
  - **Depends on:** R13 for R25a; R18–R23 and R24a for R25b. R24b is needed only for commercial-integration claims. **Priority:** P1 for support readiness, P2 for adoption reporting. **Scope:** track ownership and publication separately.
  - **R25a — maintenance readiness:** assign component/release/security owners and backups, approve support windows and realistic response commitments, document deprecation/backport/incident/release rollback procedures, and verify GitHub rulesets/labels/discussions. This may proceed while private signing or commercial access is blocked.
  - **R25b — evidence index:** publish a versioned index linking real test, benchmark, SBOM, provenance, accessibility, and limitation evidence. Keep release evidence beyond short CI artifact retention. Explicitly omit unverified optional commercial claims rather than delaying core support readiness.
  - **Gate:** owners approve and can execute the procedures; support metrics use observed data rather than aspirational SLA claims. Measure adoption through successful first-chart setup, opt-in production feedback, retention and voluntary commercial evaluation; no hidden telemetry. Both substeps and the applicable final checklist must pass before asserting readiness.

## 7. Final release-readiness gates

These are proposed acceptance gates, not claims about current quality. Product/security/release owners approve numerical budgets and any explicitly scoped exceptions.

- [ ] All high-priority findings are fixed and regression-tested, disproved with evidence, or explicitly dispositioned by the security/release owner; unresolved release blockers cannot be waived by wording changes.
- [ ] Full required tests discover nonzero cases and pass; meaningful assertions, skips, flaky cases, warnings, and coverage are reviewed.
- [ ] Supported framework/render/browser/culture combinations have current evidence; unsupported cases and platform EOL policy are visible.
- [ ] Actual packaged consumers pass functional, trimmed, and supported WASM-AOT checks; API compatibility has no unexplained regression.
- [ ] Final artifact identity, signatures, SBOM, dependency/license inventory, and provenance are verified and retained.
- [ ] Accessibility assessment is criteria-correct and tied to real manual/automated results; remaining limitations are disclosed.
- [ ] Chart correctness, interaction, performance, resource use, and lifecycle budgets pass on the declared environment.
- [ ] Quick starts and representative examples work without commercial prerequisites; coexistence/integration claims are proven.
- [ ] Maintainers have approved release/support/security ownership and recovery procedures; public claims match the exact released version.

**Completion is not “all standards everywhere.”** It is a bounded support contract, safe defaults, measured behavior, and honest evidence that can be repeated by another engineer.

## 8. Decisions requiring maintainer input

Resolve only when the relevant task needs the answer; do not block R00 on the whole list.

| Decision | Needed by |
|---|---|
| Exact implementation branch/commit and authorized release target | R00 |
| Approved wording/scope for the 40+ future tools; no Grid | R01 |
| Released package baseline and policy for security-motivated breaking changes | R03/R06 |
| Upload byte/count/concurrency defaults and permitted browser-header contract | R06/R07 |
| Supported frameworks, render modes, browsers, cultures, EOL and backport windows | R13 |
| Required release evidence, SBOM distribution, signing/provenance owners and access | R17/R18 |
| Independent security review and assistive-technology testing owners | R19/R22 |
| Benchmark hardware, workloads, budgets, and comparison methodology | R23 |
| Commercial test dependencies and licensing permissions | R24 |
| Support staffing, evidence retention, issue-response commitments | R25 |

## 9. Execution log and restart record

Keep this plan as the single control document; link to versioned tests/reports or immutable CI artifacts as evidence rather than copying long logs. Those future implementation artifacts need not be embedded in this file.

| Task | State | Commit/PR or reviewed diff | Verification and evidence | Blocker / next action |
|---|---|---|---|---|
| Plan creation | Documentation only | Not committed by this session | Original source/config audit summarized; implementation not run | Preserve this file; begin R00 in selected checkout |
| R00 | Completed — baseline investigation only, 2026-10-06 | User selected `address-warnings` at `19227619082433fa3b86e15d4e7aee625850aed9`; isolated clean-start worktree; uncommitted record/evidence only | [E01 baseline report and retained archive](evidence/2026-10-06-r00-1922761/index.md): nonshared library build 0 warnings/errors; samples 232 warnings/0 errors; bUnit 2,735 passed twice; browser discovery 2,421, four smoke cases passed. Default build failures, 11 high npm findings and SBOM mismatch retained and classified | R01 next; investigate build/compiler-state sensitivity under R02. F14/F17 documentation corrections absent here. R01–R25 and all nine final gates remain open |
| Closure planning — 2026-10-03 | Documentation only; no implementation task started or completed | Uncommitted planning appendix and companion procedures | Historical planning record; no candidate verification performed at that time | Superseded for current task state by dated R00/R01/R02 records |
| R01 | Completed — documentation only, 2026-10-06 | Same base SHA; pre-existing edits preserved; user approved bounded scope | [R01 report / checksummed checks](evidence/2026-10-06-r01-1922761/index.md): 201 links/anchors, two YAML forms and two documentation-contract tests passed | Documentation only; implementation and release gates remain separate |
| R02 | Completed — runnable harness, 2026-10-06 | Same base SHA plus two-file test-only patch; uncommitted; agent quality review | [R02 evidence](evidence/2026-10-06-r02-1922761/index.md): red 1 fail/2 pass → green 3 pass; main 2,738 pass Debug and Release; standalone 36 pass; actual source graph and nested-output exclusion verified; one Chart browser case pass | R03a next. Default builds pass now; historical compiler failure unrooted. Existing warnings/assertion gaps remain R11; no release gate closed |
| R03a exploratory | Historical partial record — superseded for current state by R03 below | Same base SHA; initial planning-only decision | [Unsuppressed comparison](evidence/2026-10-06-r03a-1922761/index.md): 195 diagnostics/TFM and consumer failures retained unchanged | Individual approvals/enforcement followed; historical evidence not rewritten |
| R03 | Completed — bounded source compatibility gate, 2026-10-06 | User-approved 2.0.0 uncommitted version diff and exact 194 major exceptions; source namespace repair; no commit/publish | [R03 evidence](evidence/2026-10-06-r03-1922761/index.md): three-TFM Debug/Release gate passes; two compiled mutations fail; 2,738 regressions, nine verifier and two browser cases pass | R04 next. Candidate package/provenance, CSS isolation and final release gates remain open |
| R04 | Source verified; blocked on private review — 2026-10-06 | Same base plus preserved R02/R03 and five-file R04 patch; uncommitted | [R04 evidence summary](evidence/2026-10-06-r04-1922761/index.md): original sinks 6 fail/3 pass, fixed 22 browser executions pass; 2,738 bUnit pass; zero-warning Release build and API gate pass all TFMs | User chose recommended hold: named security owner/private disposition pending. Raw evidence retained locally outside repo. R05 independently eligible on continuation; no final gate closed |
| R05 | Source verified; blocked on private review — 2026-10-06 | Same base plus preserved prior changes; user-approved original-filename correction; five source/test files, uncommitted | [R05 evidence](evidence/2026-10-06-r05-1922761/index.md): fail-before controls, 40-case matrix, 102 combined repeated browser passes; 2,738 bUnit and three-TFM build/API passes | Named security owner/private disposition pending; R04 hold unchanged. R06 credential policy next on user continuation; all final gates remain open |
| R06 | Partial characterization; option B endpoint hold — 2026-10-07 | Owner approved opt-in dummy/local priorities 1–4; B selected, no production export repair; production/API unchanged | [R06a](evidence/2026-10-06-r06a-1922761/index.md), [R06b](evidence/2026-10-07-r06b-1922761/index.md), [R06c](evidence/2026-10-07-r06c-1922761/index.md): 54 repeated established passes, eight managed passes and separate pause/guard controls; three removal-overlap and pending-final resume failures. Two historical endpoint failures retained. [Decisions](R06-DECISIONS.md) | Matrix incomplete; endpoint/dependent changes blocked/out of phase. Exact contract/migration, host/storage and XHR policy held. Continue unblocked cases, no formal checkpoint, production fix, release closure or automatic R07 |

### R00a/R00b session record — 2026-10-06

- **Authorization / scope:** user selected current branch/commit above. Read-only source investigation and baseline execution only; source `Version=1.0.0` unchanged. No release target, risk exception or independent review approved.
- **Environment:** macOS 27.0.1 arm64; SDK 10.0.401; installed runtimes 8.0.31/9.0.20/10.0.12; Node 26.10.0, npm 12.1.0; Playwright 1.63.0, Chromium 153.0.8010.12. Full inventory/commands are in the linked evidence report.
- **Verification:** main bUnit net8 Debug discovered/executed 2,735 cases, all passed, no skips; repeat agreed. Browser net10 Server Release/Chromium: four existing basic Chart cases passed with retries disabled; 2,417 other discovered browser cases not run. No package/trim/AOT/manual AT/performance verification.
- **Failure classification:** default library/sample builds produced cascading Razor/compiler errors; isolated clean/serial builds still failed. Both passed with compiler-server reuse disabled, unchanged source. Environment/state sensitivity is measured; exact compiler root cause remains unproven. Samples reported 232 warnings; bUnit compilation logged 3,537 warning occurrences. No suppressions added.
- **Security/supply-chain findings:** existing ESLint/XSS checks passed but exclude shipped JS; library-only NuGet query reported no advisories; npm audit reported 11 high affected-package entries. Top-level SBOM checksum mismatch reproduced. No automatic dependency/checksum repair or security clearance.
- **Finding inventory:** all F01–F17 revalidated at source level. F08 recorded blockers already fixed; F06 axis-instance fix present, other isolation issues remain. F14/F17 wording corrections described historically are absent in this commit; current dispositions supersede the historical narrative for execution.
- **Evidence:** [report](evidence/2026-10-06-r00-1922761/index.md), checksummed raw log/TRX/browser/archive bundle alongside it; public GitHub release metadata rechecked for v1.0.1/v1.0.2, not package identity/signature proof. Evidence owner/reviewer/retention remain unassigned.
- **Changes / preservation:** only this plan and new local evidence files changed in the user checkout. Existing user files, production/test code, versions, dependency lockfile and SBOM snapshots preserved. CSS/SBOM generation occurred only in the isolated worktree; it and raw logs are retained. No commit, push, tag, signing, publication or remote-setting change.
- **State / handoff:** R00 complete as a classified baseline, not a clean default-build assertion. Stop at the task boundary. Next eligible task **R01**, then R02 build/harness follow-up; independent owner approvals and final release gates remain open.

### R01 session record — 2026-10-06

- **Scope / approval:** documentation-only on the same `address-warnings` SHA. User approved Chart flagship, independently useful MIT toolkit, no Grid, exploratory 40+ supporting direction without a promised list/dates. No release, SLA, security exception or API change approved.
- **Incoming state:** substantial documentation corrections were already uncommitted at session start. Preserved and reviewed them; did not attribute them to R00 or overwrite historical evidence. F14/F17 assurance corrections now exist in the working tree, not the base commit.
- **Changes:** linked retained R00 results and limitations from README/roadmap/development/test matrix/index; corrected the remaining SBOM README's shipping/provenance/checksum claims without changing artifacts; removed links to absent historical records; qualified lifecycle/example/readiness wording. Full reviewed scope and file list are in the [R01 report](evidence/2026-10-06-r01-1922761/index.md).
- **Verification:** 201 local links/anchors across 24 core documents; both issue-form YAML files parsed with unique field IDs; unchanged prebuilt net8 R00 test assembly against current document snapshots: **2 passed, 0 failed, 0 skipped**. These are text/selector-document contracts, not package tests. Edited guidance diagnostics and scoped whitespace checks passed. Whole-tree whitespace still flags a pre-existing Chart skill Markdown hard break, left unchanged.
- **Evidence / limitations:** [checksummed validation/TRX archive](evidence/2026-10-06-r01-1922761/index.md); release dates checked against retained R00 response; all SBOM JSON/checksum bytes and R00 archive preserved. No full build/bUnit/browser/scan rerun, AT session, publish or onboarding execution. Separate review agent unavailable; executor review only, independent reviewer/retention owner unassigned.
- **State / next:** R01 completed for the reviewed documentation scope; uncommitted. Stop at task boundary. **R02** next: investigate default-build/compiler-state sensitivity, warning relevance and intended test-project boundary, preserving already-fixed F08 changes. R02–R25 and all nine final gates remain open. No commit, push, signing, publication or remote-setting changes.

### R02 session record — 2026-10-06

- **Base / scope:** same `address-warnings` SHA; fresh isolated worktree with matching two-file test patch. SDK 10.0.401/macOS arm64; net8 bUnit, net10 Server/Chromium browser. Original production code/configuration and SBOM bytes preserved.
- **F08 / targeting disposition:** previous compile exclusions and `IList<T>` corrections already present. Standalone ButtonGroup commercial wildcard/signing/shared-suite boundary was source-confirmed and corrected. Added three shared declaration/assembly regression cases; one failed before fix, all three passed after. Parent suite still includes all original tests and excludes nested generated C# after standalone builds.
- **Verification:** main 2,738 discovered, full Debug and Release each 2,738 passed/0 failed/0 skipped; standalone 36 discovered/passed, overlapping main coverage. Actual restored graphs reference Toolkit source only among Syncfusion entries. Default clean/nonincremental library builds 0 warnings/errors; samples 232 warnings/0 errors. R00 compiler-state failure did not reproduce; no workaround added, no root cause claimed.
- **Browser:** one existing Chart title/series/point/legend test passed, net10 Server Release/Chromium with retries off. First over-anchored CLI filter failed with zero discovery; corrected selection and both records retained. No assertion/source weakening, no full browser-matrix claim.
- **Warnings / limits:** unchanged 3,537 main test warning occurrences reviewed by category; 82 unknown `ChartAxes`, two required-parameter and seven unawaited-call occurrences feed R11 semantics/quality review. No suppressions/skips or readiness waiver. Editor runner could not discover isolated projects; CLI fallback retained.
- **Evidence / review:** [R02 report and checksummed raw/patch bundle](evidence/2026-10-06-r02-1922761/index.md). Read-only agent quality review found no required issues; optional case-insensitive package-ID check applied and retested. Not maintainer/security approval; retention owner remains unassigned. Corrected malformed incoming closure/R01 log rows against existing records; preserved newly present historical docs.
- **State / handoff:** R02 completed within executable-harness scope, uncommitted. Stop at task boundary. Next **R03a**, obtain verified released-baseline and compatibility/version decision before enforcement. No inferred release number, public API change, commit, push, signing or publication. All nine final gates remain open.

### R03a session record — 2026-10-06

- **Scope / identity:** same base SHA; comparison only, no production API or version changes. Historical NuGet Toolkit 1.0.2 downloaded from nuget.org, SHA-256 `06934c512f9c42497fb37464f36d1b0cd7b987624bf2dab01c77b8a80865705d`; `dotnet nuget verify --all` exit 0 with author/repository signatures. This does not close candidate signing/provenance gates.
- **Measured API delta:** pinned Microsoft.DotNet.ApiCompat.Tool 10.0.401 compared all three released TFMs against unchanged R02-built source assemblies: each exited 1 with 195 diagnostics (84 type, 107 member, one version, one constraint, two constructor/subclassing). No suppressions. Parameter-name and optional attribute checks included; self-comparisons exited 0. Diagnostic rows overlap and are not 585 unique breaks.
- **Consumer evidence:** isolated net8 direct-assembly probes compiled against 1.0.2 and failed against current source for unconstrained `DataAdaptor<T>` (CS8714 under warnings-as-errors), List-valued `GetButtonItems` (CS0266), and public `ChartRenderer` access (CS0122). Explicit notnull/IList migrations compiled. No binary/runtime/package-consumer guarantee.
- **Non-CLR / provenance limits:** 20/21 packaged scripts match source after CRLF/LF normalization; NumericTextBox spinner focus/prevent-default change retained as a diff, not runtime tested. Managed serialization, complete Razor contracts and each public break still need review. Package embedded commit `2e0b790...` differs local tag `27cf7a7...`; public lookup returned 422. Not proof of tampering, but source/artifact reconciliation remains open.
- **User decisions:** approved the exact downloaded NuGet 1.0.2 as comparison baseline, explicit major-release direction and **proposed 2.0.0 planning value only**. No blanket break acceptance, source version update/commit, suppression or publishing approval. Source stays 1.0.0.
- **Evidence / review:** [R03a report, reproduction probes and checksummed archive](evidence/2026-10-06-r03a-1922761/index.md); read-only agent review bounded conclusions and identified overbroad collection-compatibility wording, corrected in changelog. Existing changes and R00–R02 evidence preserved. No full test/browser/scan/publish rerun.
- **State / next:** **R03a partial; R03 unchecked**. Finish symbol-level migrations/dispositions and non-CLR review; obtain remaining owner/version-commit authorization before candidate identity or enforcement. Existing failed comparisons do not substitute for a deliberate-new-break gate test. No commit, push, tag, signing, publication or remote setting changed.

### R03 completion record — 2026-10-06

- **Scope / explicit decisions:** user approved the reviewed exact existing major changes, both getter-only collection removals and source Version=2.0.0. User explicitly accepted review of the uncommitted version diff instead of the R03a version-commit prerequisite; no commit/publication authorized. Bounded same-version asset/representative DTO migration approach documented; arbitrary old persisted/internal state unsupported, not silently deleted.
- **Implementation:** hash/signature-pinned NuGet 1.0.2; APICompat 10.0.401; 194 exact symbol/rule exceptions (CP0003 never suppressed); independent frozen current public/protected/generic/Razor/callback/JSON guard; strict context/schema/identity/TFM checks. CI pack and summary require the new job. Fresh build's unapproved SfDataManager namespace break fixed explicitly; Dialog XML example corrected. No CSS migration implemented.
- **Proof:** removed RefreshLiveData in disposable build → CP0002 failure; changed already-excepted GetButtonItems return again → APICompat passed but current-surface guard failed. Both reverted. Final Debug/Release three-TFM gates pass; generated-context negative fails. Nine verifier tests, Razor compile 0 warnings/errors, full net8 Release 2,738 pass/0 fail/0 skip, two Chromium smoke cases pass. Probe runs assembly inventories on net10, not three runtime hosts.
- **Review / limitations:** two agent reviews addressed JSON bool/number equality, protected nested types, generic methods/interfaces, generated fallback and Debug/Release normalization; no suppressions added for these failures. Existing test warnings remain. CSS isolation is planned separately and requires asset/selector/package/consumer validation. Historical package embedded-commit/tag discrepancy remains R18; remote CI/admin settings and actual candidate package remain unverified.
- **Evidence / state:** [checksummed R03 bundle](evidence/2026-10-06-r03-1922761/index.md), [policy](API-COMPATIBILITY.md), [individual target inventory](API-BREAK-INVENTORY.md). R03/R03a source-contract requirements complete under explicit scope/version decision; all changes uncommitted. Original SBOM bytes and prior evidence preserved. **R04 next**; all nine final release gates open. No signing, push, tag, publication or remote-setting change.

### R04 source implementation / private-review hold — 2026-10-06

- **Scope / implementation:** same base plus preserved R02/R03 diff; new shared text/explicit-formatting helper replaces both measurement HTML sinks. Only exact attribute-free break/bold tokens create nodes; other markup/entities remain literal. Existing fonts, geometry APIs, lone-space handling and JS/managed contracts retained. No cache, CSS migration or unrelated tooltip-renderer fix.
- **Routes / proof:** actual category/series/mapped-tooltip/crosshair, initial layout and explicit refresh tested; whole-string path specifically Arabic U+0600–U+06FF, not all non-Latin. Final original-sink control in disposable tree: **6 failed/3 passed**; fixed nine cases plus two existing smoke cases repeated: **22 passed/0 failed/skipped/flaky**. Transient observer sees old unexpected nodes; direct tests check local requests/handler sentinel; Unicode/RTL/break/bold/space/entity/geometry and reuse/null/malformed cases pass.
- **Wider checks:** fresh Release net8/net9/net10 build 0 warnings/errors; existing R03 API/current-contract gate passes all three with no changes to exceptions; net8 bUnit 2,738 passed/0 failed/skipped; strict focused TypeScript check passes in installed worktree. Test runner cannot discover isolated project; CLI fallback used. Original checkout lacks Playwright declarations; existing test warnings remain, not suppressed.
- **Investigation / remaining defects:** early integration failures are not security-red proof. Corrected fixture dimensions, explicit refresh, transient text capture and axis-order assumption; no assertions skipped. Existing attributed-tag tooltip validator exception and CSS viewport/hit-test mismatch require separate R10/R11 follow-up. Unsupported markup may still measure differently from tooltip rendering; universal tooltip safety/layout not claimed. Package inclusion remains R16.
- **Review / disposition:** two read-only agent reviews; bounded coverage findings addressed. User selected recommended approach to **keep R04 open pending named private security-owner review**. Owner, private ticket, affected-release/impact assessment, retention policy and disclosure decision remain unassigned. No external report sent; agent review is not security approval or risk acceptance.
- **Evidence / preservation:** [non-sensitive R04 summary](evidence/2026-10-06-r04-1922761/index.md) records local restricted archive reference/hash and tested file identities. Detailed reproduction evidence retained outside repository; transfer to approved private storage pending. Tested five-file patch matches user checkout; mutations reverted; prior evidence and original SBOM bytes preserved. All changes uncommitted; no push/tag/signing/publication/admin change.
- **Handoff:** R04 source work verified but checklist **unchecked**. Stop at this task boundary. **R05** is the next independently eligible implementation on user continuation; retain R04 private-review blocker. All nine final release gates remain open.

### R05 source implementation / private-review hold — 2026-10-06

- **Scope / changes:** preserved current source and R04 patch on same base SHA. Eight uploader metadata HTML writes replaced by text; default Razor expressions unchanged. Optional selection validation preserves names rather than numeric entities; user selected the recommended compatibility correction. Sanitizer comparison uses an inert template-owned document and repairs undefined banned-tag removal. XML/migration guidance updated; no header, budget, shared DOM-helper or template-boundary redesign.
- **Fail-before evidence:** initial 20 cases 9 failed/11 passed; supplemental four all failed. After initial fix, 24 passed; review's harmless custom-element counter exposed live-document parsing, then inert-document fix passed. One additional failed review assertion was a guessed localization value, corrected to the measured existing missing-key/Razor-remapping behavior; not counted as security proof.
- **Final verification:** 40 uploader cases pass, covering real AJAX/Razor routing, preload/selection/API/event modification, sanitizer on/off, full template lifecycle, validation flags, ordinary/hostile text, cancellation/progress/truncation and direct legacy helper. Combined 40 R05 + 9 R04 + 2 smoke cases repeated: **102 passed/0 failed/skipped/flaky**. Full bUnit net8 Release **2,738 passed**, fresh three-TFM library build **0 warnings/errors**, API/current-contract gate passes with no new exceptions. Only source/project-reference Chromium/net10 Server; no actual upload endpoints used.
- **Tooling / review:** two read-only agent reviews, final no source blocker. Isolated test target discovery required CLI fallback. Focused TypeScript passed with temporary pinned Node declarations; repository install lacks them, initial failed checks retained. Existing test warnings, sanitizer-name localization, Razor status remapping, sample-relative-URL and no-extension presentation differences remain R11 follow-ups. No full browser/package/security-matrix assurance.
- **Disposition / preservation:** user selected recommended private-review hold alongside filename approval. R05 **unchecked**, named owner/private record/impact/disclosure/retention decision pending; R04 hold unchanged. [R05 summary](evidence/2026-10-06-r05-1922761/index.md) links behavior and records local private archive hash/file identities. Raw reproduction evidence retained outside repo with owner-only permissions, not an approved organizational evidence store. Tested source matches checkout; prior archives, original SBOM and user changes preserved.
- **Handoff:** stop at R05 task boundary; **R06** next on continuation, with explicit credential-policy decision required before changing behavior. No commit, push, tag, signing, publication, external report or remote-setting change. All nine final release gates remain open.

### R06 broader transport design review — 2026-10-06

- **User decision:** after source tracing and a proposed bounded no-default-export/early-event migration, user explicitly selected **“Review a broader transport redesign first.”** Production implementation is paused; no approval of the narrow fix or new API is inferred.
- **Source findings:** managed initialization/updates export all client default headers; browser XHR bypasses .NET transport/handlers/timeout. Header/form/credential state is component-wide, later callback timing varies, and retry/resume/remove/cleanup paths differ. These are source-confirmed, not R06 runtime reproductions or deployed-compromise evidence.
- **Design output:** [unapproved R06 proposal](R06-UPLOADER-TRANSPORT-DESIGN.md) and proposed `tasks/plan.md` / `tasks/todo.md`. Recommend one typed per-attempt decision, empty defaults, no implicit/legacy credential merge, deterministic awaits, generation/target binding, deadlines and explicit cleanup identity handling. New API, legacy failure behavior, endpoint policy, cancellation/control outcomes and XHR limitations all require approval.
- **Review refinements:** read-only design review surfaced unresolved late server writes after cleanup success, stable host identity/reconciliation needs, clear-versus-destroy registration ownership, cancellation/clear veto timeouts, batch/attempt deadlines and approval sequencing. Draft now distinguishes each; exact API/outcome choices remain open, not production-ready approval. An initial authorization may cover characterization only; a later explicit exact-contract checkpoint precedes production edits or R03 manifest updates.
- **Verification / preservation:** documentation-only after source reads; no R06 builds/tests/browser requests, endpoint creation, credential collection or new production worktree. Existing R04/R05 implementation and private-review holds preserved. No commit, push, publication, report, remote setting or API baseline changed.
- **Handoff:** R06 unchecked; current forwarding behavior still exists. Request review/revisions of D1–D8 or authorization for local dummy-data characterization. Do not automatically implement the proposal or advance to R07. All nine final release gates remain open.

### R06a bounded characterization / decision delegation — 2026-10-06

- **Authorization:** user said proceed to the next step, use recommended decisions, log why and identify owner approvals. Treated as characterization authorization plus selected technical direction, not approval of unspecified public signatures, host storage guarantees or private/release sign-off.
- **Test-only work:** opt-in `tools/transport-characterization` config/managed project, guarded test-host fixture, dummy-only local intercepted requests. Outside default CI discovery so current defects do not become permanent contracts. Production and API baselines unchanged; 511 source files match retained R05 snapshot.
- **Observations:** eight managed cases pass; 11 browser cases repeated = 22 pass, no skips/flaky/retries; two safety checks genuinely fail. Dummy defaults appear in actual initialization/update/request; early events await; ordinary late upload/remove and chunk2 can send before return. Three chunks and one controlled retry counts measured; supplied HttpClient unused/not disposed. Managed async-void test-builder pitfall corrected, not labeled production bug.
- **Review / evidence:** fixture environment opt-in, response/callback teardown and decision-consumption checkpoints added after review. [R06a report](evidence/2026-10-06-r06a-1922761/index.md) records curated restricted archive/hash and limitations. Initial overly broad header capture included local antiforgery cookies, excluded from curated archive and left restricted for owner retention handling; corrected reviewed runs retain only dummy header fields. No external uploads/storage endpoint.
- **Decisions / gaps:** [R06 decision register](R06-DECISIONS.md) selects D1–D8 recommendations with reasons. API maintainer exact contract, host identity/reconciliation and redirect/cookie requirements, named private security disposition and release authorization remain required. Child-only updates, concurrency, pause/resume/cleanup, endpoint/lifecycle matrix remain uncharacterized; no complete slice-1 claim.
- **Handoff:** continue remaining bounded characterization and exact contract drafting, then approval checkpoint before production rewrite/rebaseline. R06 unchecked; R04/R05 holds unchanged. No commit/push/sign/publish/report/admin changes. No automatic R07 advancement; all final release gates open.

### R06b owner-authorized priorities 1–4 — 2026-10-07

- **Owner decision:** characterization continues opt-in/dummy/local/no production changes/no commit or push. Bounded in-process/loopback dummy endpoint approved only for characterization, never default CI/production. No host/storage or late-write guarantee claimed; XHR redirect/cookie policy observation is not approval. Exact API/migration sign-off held until matrix completion and formal checkpoint.
- **Execution / scope:** priorities 1–4 attempted in order using isolated snapshot and test-host probes. Mutation/replacement and child-only updates, same-component interleaving in both orders/two-instance control, null/empty headers and true→false flag, endpoint updates, manual retries and genuine progress/pause/resume. No internal status assignment or product export shim. Only test-host/tool/docs changed; 511 source and eight API-baseline files match retained R05 bytes.
- **Evidence / counts:** [R06b report](evidence/2026-10-07-r06b-1922761/index.md): nine config/isolation and two retry observations pass; combined R06a/R06b established cases repeated = **44 pass**, no skips/flaky/retries. Live pause **one pass** with `[0,0,1,2]` receipts; active bounds and disabled-host guards **two separate passes**; strict types pass. **Two endpoint probes fail** because child interop export failure recurs and ends circuit; retained as blocked/unproved retargeting, not hidden in green count. Initial fixture/guard/type failures classified separately.
- **Local endpoint / review:** Development plus explicit environment opt-in, loopback peer/host and local origin checks, exact dummy chunks, 8 KiB read bound, 16 admitted requests, 15-second waits, memory-only/no storage. Guard tests prove malformed budget/size/origin rejection and disabled behavior. Same-host pause repetition prohibited; host restarted for each run. Review improved counts/parser bounds/teardown and decision observations; this is not production security approval.
- **Findings / limits:** delayed default-header propagation, missing child module export, within-component header contamination, sticky XHR credential flag and different ordinary/chunk retry callback behavior measured. No real cookie policy or server idempotency/cleanup guarantee. Retry-from-stage, broader endpoint/concurrency/cleanup/lifecycle/modes/outcome cases still open. No full bUnit/API/platform rerun or source remediation claim.
- **Preservation / handoff:** curated private archive hash and tested identities in report; raw diagnostics restricted locally. Earlier session records retained as history. R06 unchecked, R04/R05 private holds unchanged, all nine final gates open. Continue remaining characterization or discuss blocked scope; formal API/migration checkpoint only after complete/explicitly dispositioned matrix. No production patch, commit/push/sign/publish/external report/admin action or R07 advancement.

### R06c blocker disposition and bounded continuation — 2026-10-07

- **Owner checkpoint / choice:** selected **B**, retaining missing child-module export and endpoint retargeting/dependent destination-change cases as blocked/out of this phase. Option A's production repair was not selected. Continue unblocked priorities 1–4 under existing opt-in dummy/local/no-production/no-commit constraints. No formal API checkpoint yet.
- **Scope / source:** same base/branch, isolated current dirty-tree snapshot, net10 Server/Chromium and net8 managed characterization. Test-only ordinary parent render, retry-from-stage/exhaustion, clear/reselection, removal header/form overlap and later/final pause/resume variants. Endpoint still bounded, memory-only, Development plus explicit opt-in. No library or API-baseline change.
- **Final evidence:** [R06c report](evidence/2026-10-07-r06c-1922761/index.md): **54 established executions pass** (27 unique ×2, explicit endpoint/overlap exclusions), **eight managed pass**, three additional pause observations, original first-pause control and two guard modes pass. **Three removal-overlap probes and one pending-final resume probe fail**, unsuppressed. Two R06b endpoint failures and R06a policy failures remain historical red evidence, not rerun or folded into green counts.
- **Findings / boundaries:** retry-from-stage advances bytes without index and then sends empty final slice; clear preserves credential flag; real parent updates confirm delayed defaults; removal changes active-upload headers/form. Pending later resume duplicates success; final settled response overrides pause. All overlap completions raise generic `RemoveFileDataAsync`; pending-final has completed HTTP handlers but no file success. Exact runtime causes unconfirmed; null-event resume path is source-supported only. Passing observations are not approved contracts.
- **Review / preservation:** strengthened value-free non-dummy detection and final pending receipts/timeouts, then reran reviewed files. Strict types pass with temporary declarations; checkout missing-declaration diagnostics persist. **511 source + eight API files** match retained R05/tested bytes; 21 generated theme artifacts classified separately. Original SBOM and prior evidence preserved. Curated restricted archive SHA-256 `735bca960d5e2681d0a9c5615695d7d564a9657c8bc366f0b4f352c9b5bbc018`; raw connection diagnostics excluded, retained privately. No organizational retention/security approval or fresh full bUnit/API/R04/R05 matrix claim.
- **State / next:** bounded continuation recorded, **matrix still incomplete**. Continue unblocked failure characterization, callback veto/rejection/interleaving/form reset and outcome subcases; broader cleanup/lifecycle/modes remain open. Exact API/migration, host/storage guarantees, XHR redirect/cookie policy, named private security/release approvals remain held. R04/R05 holds, R06 checkbox and all nine final gates unchanged. No production repair, commit/push/tag/sign/publish/external report/admin change or R07 advancement; this record does not close the owner's full continuation request.

For each work session append a record with:

- **Task/substep and date:**
- **Base commit, branch, environment:**
- **Finding disposition and scope:** source tree / release package / host configuration.
- **Changes and public-contract impact:**
- **Verification:** exact commands or actions, framework/mode/browser, discovered/passed/failed/skipped counts, and before/after regression result.
- **Evidence:** test/report paths or immutable links; artifact hash where relevant. Do not use local absolute paths as the only evidence reference.
- **Security/maintainer review and approved exceptions:**
- **Commit/PR or uncommitted diff summary:**
- **State:** not started / in progress / blocked / completed / not applicable with reviewed rationale.
- **Next task and remaining blockers:**

### Prompt for a new session

> Read `docs/ENTERPRISE-READINESS-PLAN.md` and the repository's current instructions. Chart is the flagship; no Grid or new components are in scope. This plan came from a dirty working-tree audit, so do not assume its findings or prior changes exist in this checkout. Review the execution log, verify the selected branch/commit and dependencies, and work on only the next eligible incomplete task, starting with R00 if no baseline exists. Reproduce behavior findings before fixing them, preserve public contracts unless approved, and record exact verification and limitations. Do not weaken tests or fabricate compliance/release evidence. Update this same document with task status and handoff details. Do not commit, push, publish, change remote settings, or discard user changes without authorization. Stop at the task boundary and summarize the result.

## 10. Standards and policy references

Recheck versions and dates when execution resumes; these links are references, not claims of certification.

- [.NET support policy](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core) — distinguish framework targeting from a supported, patched deployment.
- [WCAG 2.2](https://www.w3.org/TR/WCAG22/) — target applicable A/AA criteria and document component/host responsibilities.
- [OWASP ASVS](https://owasp.org/www-project-application-security-verification-standard/) — stable version reported as 5.0.0 during this audit; map applicable requirements explicitly rather than claiming the full application standard applies to a UI library.
- [SLSA specification](https://slsa.dev/spec/) — select the appropriate published version/track and verify requirements before claiming a level.
- [CycloneDX specifications](https://cyclonedx.org/specification/overview/) — choose supported schema/tool versions, validate generated SBOMs, and bind them to the release.
- [Semantic Versioning](https://semver.org/) and [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) — communicate actual compatibility/release history, not planned features as shipped functionality.

## 11. Documentation-sync progress — 2026-10-03

**State:** documentation progress and bounded local source reconciliation, not completion of R00, R01, R02, R09, any phase checkpoint, or any final release gate. Sections 2 and 5 remain the **original 2026-10-02 audit basis/findings**; the open checkboxes and full acceptance criteria in §§6–7 are unchanged. Do not reintroduce old defects to match that baseline.

**Approved scope:** the user approved Chart-first/supporting-tool direction with **no Grid**, evidence-accurate documentation, and manual publishing. This synchronization covers root documentation, `.github` guides/skills/issue-form wording, historical/performance/index records, accessibility/security evidence registers, and SBOM guidance. Only the contradictory signing-key comment in `.gitignore` changed; ignore patterns are unchanged. No repository source, project/package version, analyzer/security configuration, build workflow, or release credential is changed here. Keys stay with authorized manual publishing owners, never in the repository or repository CI.

| Item | Later disposition / evidence | Remaining gate |
|------|-----------------------------|----------------|
| **R01** | Documentation updated under user-approved Chart-first/no-Grid scope. The [index](../.github/Index.md) separates current guidance, evidence status, plans, historical records and accessibility drafts. Performance notes describe source mechanisms and unexecuted checks, not benchmarks or conformance. | Full R01 assurance/evidence review and release-specific reconciliation are still open; no task checkbox is completed by wording changes. |
| **F08 / R02** | **Resolved in current local source** for the recorded compiler blockers: the bUnit project excludes nested `**/obj/**/*.cs;**/bin/**/*.cs`; the two dialog assignments and Chart `SharedTooltip.razor`, `Crosshair.razor`, `Events.razor` assignments now use `IList<T>`. The [development record](../.github/DEVELOPMENT.md#known-analyzer--trim--aot-findings) records later successful local compilation/execution. | Revalidate on the selected clean checkout, retain run/discovery artifacts and satisfy the complete R02 gate (including the Chart browser target). No release-package result is inferred. |
| **F06 / R09** | **Axis-overlap statics fixed only:** `CartesianAxisLayout._previousAxis`, `_previousAxisEnd`, `_previousStartX` are instance fields. `tests/Syncfusion.Blazor.Toolkit.BUnitTest/Charts/Chart/Rendering/AxisLayoutIsolationTests.cs` contains the two-layout reset regression; the development record reports it failed before the fix and passed afterward. | Browser measurement/cache scope, shared service-options ownership, bounds and concurrent-circuit isolation remain open. This does not resolve all of F06 or R09. |
| **Recorded local validation** | The 2026-10-03 development record, after the approved `DataAdaptor<T> where T : notnull` change, reports non-incremental library builds on .NET 8/9/10: **0 warnings, 0 errors**; bUnit **net8.0: 2,735 passed, 0 failed, 0 skipped**. | Existing reported local evidence, not rerun here, not net9/net10 test execution, remote CI, package verification, benchmark or certification. Preserve exact run artifacts for reproducibility. |
| **Release history** | Supplied GitHub metadata snapshot: [v1.0.2](https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.2), published **2026-09-01**; [v1.0.1](https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.1), published **2026-06-26**. | Metadata is not NuGet artifact/signature/SBOM evidence. No source version update or publication is performed. Manual owners must provide final artifact verification under R16–R18. |

**Verification performed for this synchronization:** source/configuration/document inspection, declared-parameter inventory (not analyzer diagnostic counts), public GitHub release API retrieval, W3C criterion-text checks, and Markdown/link/anchor/YAML/diff checks. The README's verbatim `Program.cs` and chart page compiled in an isolated minimal .NET 10 host against **published Toolkit 1.0.2**, with **0 warnings, 0 errors**. The two targeted `SbomContractTests` / `StyleContractTests` passed; these assert documentation text, not package compliance. The full 2,735-test result above was not rerun. No browser/AT sessions, security scans, benchmarks, trimmed/AOT publication, signing, or publishing were performed. The original audit's dirty-tree counts/date/commit remain historical, not a fresh clean environment baseline.

**Outstanding evidence / next session:** select and record the clean implementation commit under R00; retain original local run artifacts; complete the unresolved security/lifetime/circuit review, meaningful browser/render-mode matrix, package/API/trim/WASM-AOT validation, manual-owner release/signature/SBOM/provenance checks, measured Chart workloads, and accessibility/assistive-technology evaluations. Reconcile source `Version=1.0.0` with the public release history and the checked-in SBOM checksum mismatch before using those artifacts as release evidence. Obtain real owner approvals or risk dispositions where required; no waiver, approval, or certification is fabricated by this documentation update.

## 12. Remaining-issue closure plan

**Planning only — 2026-10-03.** This appendix translates the remaining issues into closure actions; it starts no implementation and satisfies no gate. §§1–11 retain their audit/history, R00–R25 remain open, and all nine §7 checkboxes remain open. Later dispositions below qualify the original findings rather than rewriting them.

**Control:** [§9](#9-execution-log-and-restart-record) remains the authoritative execution/state table. Reuse R00–R25 and their substeps; E01–E12 identify evidence categories, not a competing task list. All owner roles below are **UNASSIGNED** until a named person accepts responsibility.

The [manual/external evidence tracker](MANUAL-EXTERNAL-EVIDENCE-TRACKER.md) supplies request/status ledgers, detailed procedures, report formats and review requirements. The [release checklist](RELEASE-CHECKLIST.md) applies the unchanged §7 gates to a specific candidate, beginning with source-version reconciliation. Both are blank/reusable planning documents, not collected evidence. Neither may independently mark an R-task complete.

Evidence cases are staged EXPLORATORY, PREPUBLICATION, AUTHORIZATION or POSTPUBLICATION. Version selection can use exploratory API outputs; R16 verifies the approved-version package later. Initial R15 publication may use a source/project-reference consumer, with package-only reruns under R16. Required prepublication cases precede go/no-go; downloaded-public-package cases remain pending until publication and gate closeout, not publication itself.

### 12.1. Disposition boundaries

| Recorded state | What remains to close |
|---|---|
| F14 documentation corrected | `docs/VPAT-2.5-INT.md` now distinguishes 2.4.11 AA, 2.4.13 AAA and the non-dragging single-pointer requirement. This is a draft evidence register, not measured conformance; R21 review and R22 implementation/manual AT evidence remain. |
| F17 documentation corrected | `.github/RENDER-MODE-SECURITY.md` withdraws nonexistent enforcement claims. R01 assurance review and R19 host/security review remain; corrected wording does not add a security boundary. |
| F08 fixed in current local source | Preserve the reported fixes and run records in §11; revalidate compilation, discovery and full execution on the selected clean checkout under R00/R02. Do not reapply obsolete fixes or infer package/runtime-matrix success. |
| F06 / R09 partially fixed | Axis-overlap state is instance-owned with a reported regression. Measurement-cache scope/bounds, shared mutable options and concurrent-circuit isolation remain; the axis test alone cannot close R09. |
| Release and SBOM evidence unresolved | Source `Version=1.0.0`, supplied v1.0.2/v1.0.1 metadata and the documented snapshot checksum mismatch are different evidence scopes. Reconcile them; none establishes final package identity. |

### 12.2. Initial priority worklist — nothing completed

Read this as future order, not permission to execute. All §6 dependencies still apply; preparation may not bypass an implementation gate.

| Order | Eligible work and dependency gate | Required exit / next handoff |
|---|---|---|
| First | R00a, then R00b; no prerequisite | Selected clean commit/environment and classified baseline in E01/§9; no R00 start is recorded by this planning edit. |
| Next | R01, then R02 after R00 | Reconcile assurances and revalidate the harness; retain nonzero discovery/results before compatibility enforcement. |
| Next | R03a, then remaining R03 after R01/R02 | Verify released API baseline, approve version/compatibility treatment, enforce it with a deliberate failing fixture; E02. |
| Then | R04–R10 in eligible dependency order | Security/lifetime/isolation fixes with failing-before/passing-after regressions and private review; E03/E04. No new components or unrelated features. |
| Then | R11–R12 after their §6 prerequisites | Meaningful regression assertions and proven source/dependency gates; E01/E03/E04. |
| Then | R13–R15 after their §6 prerequisites | Approved support scope followed by actual runtime/browser and published trim/WASM-AOT evidence; E04/E05. |
| Then | R16a and remaining R16 → R17a and remaining R17 → R18 | Inspectable package, reconciled SBOM/inventory, then authorized manual final-artifact verification; E06–E08. |
| Eligible later work | R19–R25 with their existing split dependencies | R19a need not wait for R18; R25a need not wait for signing. R20–R24 run only after their respective prerequisites, not merely because owners are available. |
| Preparation in parallel | Request owner nominations, access, retained logs, AT/hardware time, commercial permission and release-control evidence early | Requests only; no dependent remediation, test-result claim, signing or release action is authorized early. Record unfulfilled requests as blocked, not passed. |

### 12.3. Ordered substeps for the immediate ambiguities

These substeps refine existing tasks; they neither replace the parent gates nor authorize commits/publication in this session.

**R00a — Select and inventory the baseline.** Depends on maintainer selection of branch/commit; maintainer and QA roles **UNASSIGNED**.
- Files/expected record: §9/E01; inventory `src/Syncfusion.Blazor.Toolkit.csproj`, test projects, workflows, release metadata and existing evidence without changing source or preserving a dirty tree as the baseline.
- Record commit, cleanliness, environment/tools and source-versus-package scope; retain the old local reports as historical evidence with their original limitations.
- Proof/stop: approved baseline identity and inventory; stop baseline-dependent work if commit authority, required inputs or environment cannot be established.

**R00b — Measure and classify the baseline.** Depends on R00a; QA/build roles **UNASSIGNED**.
- Files/expected record: §9/E01 and finding dispositions; execute the agreed build/discovery/test baseline later, retaining exact commands, counts and failures rather than changing implementation during inventory.
- Record source `1.0.0`, supplied releases v1.0.2 (**2026-09-01**) and v1.0.1 (**2026-06-26**), package-verification gaps, and the documented SBOM mismatch as unresolved inputs to R03/R17.
- Proof/stop: reproducible results or explicit environmental/existing-failure classification. Zero discovery is not success; a classified harness defect feeds R02 without pretending baseline tests passed.

**R03a — Approve released baseline and next-version treatment.** Depends on R00, R01 and R02; API maintainer/product/release roles **UNASSIGNED**.
- **2026-10-06 execution amendment:** the requesting user explicitly authorized source 2.0.0 and review of the uncommitted version diff in place of the historical version-commit prerequisite below. This permits R03 source-contract closure, not publication or waiver of a clean release candidate. Exact major exceptions and limitations are in [API-COMPATIBILITY.md](API-COMPATIBILITY.md); historical planning wording below remains for traceability.
- Files/expected change: library project, API/package-validation configuration in `Directory.Build.props`/workflows as appropriate, `CHANGELOG.md` and version guidance; first record a reviewed decision, not an automatic version bump.
- Verify actual released NuGet package identity and compare public/Razor/generic/serialization/JS contracts. Source `1.0.0` does not override release history, and tag metadata alone does not choose the API baseline.
- Assess the public `DataAdaptor<T> where T : notnull` change against consumers explicitly; existing approval of the source change is not approval of a patch-version classification or release number.
- **Do not infer `1.0.3`.** Obtain the next-version and compatibility/migration decision. Only an authorized version commit may change project/package identity; update public release-valued documentation/examples only after that approved commit, without relabeling historical releases.
- Proof/stop: E02 links reviewed compatibility delta, release baseline, decision and authorized commit. Stop dependent public-contract changes and candidate release identity selection while approval is missing.
- Remaining R03: implement checks and prove an intentional incompatibility fails; preserve evidence of pre-existing differences separately. Approval alone does not satisfy the validation gate.

**R16a — Prove package identity/content before consumer claims.** Depends on R03, R14 and R15; packaging/QA roles **UNASSIGNED**.
- Files/expected change: library pack configuration, asset generation, package-inspection/consumer tests and unsigned CI artifacts; build from the approved version commit with controlled inputs, not untracked local assets.
- Inspect every TFM, scripts/styles, XML docs, license/notices, SourceLink and repository identity; distinguish PR head from merge commit. Record the package file list and hashes in E06.
- Proof/stop: reject missing/stale assets, unexpected commercial dependencies, mismatched version/commit or unexplained API failures. Remaining R16 runs clean local-feed consumers without toolkit `ProjectReference`, including E04/E05 package reruns.
- R17 may change package bytes: repeat affected R16 checks after SBOM integration; R18 must verify final signed bytes separately. Earlier unsigned-package success cannot certify the final artifact.

**R17a — Reconcile snapshot provenance, then regenerate.** Depends on R12 and R16; build/supply-chain roles **UNASSIGNED**.
- Files/expected change: `artifacts/sbom/sbom.cdx.json`, its `.sha256`, `artifacts/sbom/README.md` and retained generation records. Preserve the mismatch record before any replacement.
- Establish authoritative source/project/dependency inputs, TFM, actual pinned executable/version, generation command and timestamp. Reconcile unsupported tool-history labels; do not assume snapshot metadata proves its origin.
- Regenerate from those inputs; validate the chosen CycloneDX schema, product/component versions, inventory completeness, license mapping and freshness. Reconcile bundled browser assets and build-only versus shipped dependencies against E06.
- Only then produce the checksum for the validated bytes. **Replacing the hash alone is not closure**; a matching hash proves byte consistency, not authenticity. E07 must explain how the regenerated snapshot relates to the candidate, not relabel it as a past release attestation.
- Proof/stop: retain provenance plus schema/inventory/checksum results; unknown generation inputs, inventory gaps or irreconcilable provenance block use as release evidence.

**Remaining R17 — Fix source-project SBOM generation/embedding and test artifacts.** Follows R17a; build/supply-chain roles **UNASSIGNED**.
- Files/expected change: `Directory.Build.targets`, `Directory.Build.props`, pinned tool configuration, unsigned CI pack flow and tests including `BU/Base/SbomContractTests.cs`; use the pinned executable actually recorded, not an uncontrolled global fallback.
- Current generation requires enabled, `CI != true`, packable, non-test and nonempty `TargetFramework`; it consumes the source project per TFM, not signed package bytes. Approve the intended CI emission/skip contract without adding signing credentials.
- Implement generation/aggregation at the correct inner/outer multitarget pack stages before package contents are finalized. An existing mirror must not silently satisfy freshness or inclusion checks after a skip.
- Artifact tests must exercise CI true/false, opt-out, test/non-packable projects, empty outer TFM versus each inner TFM, clean and stale mirrors, tool failure and no-build pack policy; fail when a required release SBOM is absent/stale.
- Unpack the actual `.nupkg` to assert per-TFM placement/schema/inventory; documentation-word tests are insufficient. Re-run affected E06 consumers; E07 closure needs the approved distribution contract and retained artifact results.

### 12.4. Closure matrix

Current status is carried forward from the documented/source observations, not newly tested here. Each row retains the complete §6 gate and dependencies; paths use §5's `PW`/`BU` abbreviations. Role labels are nominations only, all **UNASSIGNED**.

| Task / current status | Corrective action; files / expected change | Required evidence and stop condition | Owner role — UNASSIGNED |
|---|---|---|---|
| R00 — not started | R00a/b; §9 baseline record | E01; stop dependent work without an approved, classified baseline | Maintainer / QA |
| R01 — documentation progress only | Review root docs, `.github` policies/skills and claims against current scope/evidence | E01/E02/E09/E12; unsupported assurances remain open | Product / documentation |
| R02 — F08 fixed locally; clean run pending | Revalidate `BU` project/source fixes and minimal Chart browser target; change only reproduced failures | E01/E04; nonzero full discovery/execution, no hidden skips or wrong package target | Test maintainer |
| R03 — release baseline/version unresolved | R03a plus project/props/API compatibility enforcement | E02; no unexplained contract break or unapproved release value | API / release maintainer |
| R04 — F01 unclosed | `svgbase.js`, `chart.js`, `ChartHelper.cs`, `PW`; replace unsafe measurement sinks with safe text/approved formatting | E03/E04; negative DOM/network tests plus Unicode/RTL/sizing regressions | Chart / security |
| R05 — F02 unclosed | `uploader.js`, uploader Razor and `PW`; text-safe preload/selection metadata in both routes | E03/E04; test sanitizer on/off and template boundary; unsafe route blocks | Inputs / security |
| R06 — F03 unclosed | Uploader header forwarding and docs/tests; reviewed browser-header policy after R03 | E02/E03/E04; no dummy privileged headers exported by default; migration approval required | Inputs / security |
| R07 — F04 unclosed | Uploader streams/cache/tests; enforce count/byte/aggregate/concurrency budgets, cancellation and eviction | E03/E04/E10; unlimited reads or unbounded retained resources block | Inputs / resource testing |
| R08 — F05 unclosed | Base/Chart lifecycle, `tooltip.js`, `base.js`, tests; idempotent teardown and stable listener ownership | E03/E04/E10; mount/unmount/disconnect must release registrations and resources | Component lifecycle |
| R09 — axis-instance fix only | Axis regression plus `ChartHelper.cs`, `SyncfusionService.cs`, tests; isolate/cache-bound measurements and mutable options | E03/E04/E10; concurrent font/culture/RTL circuits must not interfere | Chart / isolation review |
| R10 — F07 unclosed | Chart lifecycle/interop and reachable `src/Data/` paths; cancel stale work, detach sources, validate callbacks | E03/E04; out-of-order/malformed/disposal cases cannot mutate stale state | Chart / data / security |
| R11 — weak assertions remain | `PW`, Playwright configuration and CI; behavior assertions, focused-test protection, retained failure traces | E01/E04; deliberate regressions fail; zero-test/conditional-pass results block | Test / CI maintainer |
| R12 — shipped-JS coverage gap | ESLint/gulp/workflows/dependency policy; cover authoritative shipped sources, pin inputs, scan inventories | E03/E07; failing fixtures prove enforcement; unresolved security/license blockers stop release | Security / CI |
| R13 — contract not approved/executed | `.github/TEST-MATRIX.md` and servicing policy; approve supported runtimes/modes/browsers and EOL scope | E04/E12; installed SDKs or target declarations are not runtime support proof | Platform / support |
| R14 — actual matrix incomplete | Test hosts/projects, `PW`, workflows; execute supported runtime/mode/browser slices | E04; identify actual runtime and artifact, hydration/navigation/reconnect outcomes; missing required slice blocks | Platform QA |
| R15 — published trim/AOT unproved | Publish consumers/linker configuration; exercise typed and documented dynamic binding under trimming/WASM AOT | E05; analyzer flags/build success alone cannot pass; broad preservation cannot hide failures | Runtime / AOT QA |
| R16 — customer package unverified | R16a then clean package consumers and asset/API inspection | E02/E04/E05/E06; missing content, identity mismatch or consumer failure stops release | Packaging / QA |
| R17 — mismatch and CI/pack gaps | R17a then source-project generation/multitarget embedding tests; snapshot is not a signed-package inventory | E07 plus refreshed E06; provenance/schema/inventory/freshness failures stop release | Build / supply-chain |
| R18 — final artifacts/signing unknown | Owner-only manual release procedure, final artifacts and verification records; no repository/CI secrets | E08/E06/E07; unavailable authorized owner, access or final bytes keeps release blocked | Authorized manual release owner |
| R19 — F17 wording corrected, review open | `.github/THREAT-MODEL.md`, security/render-mode docs and host examples; R19a independent review, R19b post-R18 reconciliation | E03/E08/E12; both substeps required; risk acceptance is not test success | Independent security reviewer |
| R20 — isolated example compile only | README, Chart skills/guides and clean package fixtures; execute full onboarding contract | E06/E04; real first-chart interaction without commercial prerequisites; compilation alone insufficient | Developer experience / QA |
| R21 — F14 criteria corrected, draft only | VPAT/accessibility registers; approved scope/applicability, evaluator and release identity | E09; no unsupported Supports/ACR approval or invented external report | Accessibility evaluator |
| R22 — runtime/manual AT absent | Component/PW fixes, accessibility exports and AT session records | E09/E04; keyboard, non-drag pointer, contrast, zoom and screen-reader evidence; disclose unresolved limits | Accessibility / component QA |
| R23 — no measured baseline | Chart fixtures, performance harness and `.github/evidences/performance/`; approve measured budgets | E10; raw/reproducible correctness, median/p95 and long-run resource results; no invented thresholds | Performance / Chart |
| R24 — coexistence/commercial proof absent | Version-pinned consumers and integration guide; R24a general coexistence, R24b licensed optional validation | E11/E06; absent commercial permission blocks R24b/claims, not independent toolkit work; retain parent scope rule | Integration QA / licensing |
| R25 — operational approval unknown | Support/security/recovery docs, admin records and versioned evidence index; R25a then eligible R25b | E12 and E01–E11 as applicable; owner/backup/access/retention approval required, not assumed | Support / repository admin / release |

### 12.5. Evidence handoff contract

The companion tracker should use these same E-identifiers. Each record needs R-task/substep, source commit and/or exact artifact hash, environment/tool versions, method/commands, dated result/counts, limitations, durable location and named reviewer. Unknown/missing proof stays pending; sensitive material uses access-controlled references, not secrets or public exploit details.

| Evidence ID | Minimum retained proof | Producing tasks / owner role — UNASSIGNED |
|---|---|---|
| E01 — baseline | Selected clean commit/environment, inventory, build/discovery/test results and classified failures; distinguish old local reports | R00–R02/R11; maintainer / QA |
| E02 — compatibility/version | Verified released package/API baseline, contract diff including `notnull`, version/migration approval, authorized commit and failing compatibility fixture | R03/R06/R16; API / release maintainer |
| E03 — security review | Threat-boundary/host review, negative regressions, dependency/shipped-JS analysis and private dispositions | R04–R12/R19; security reviewer |
| E04 — runtime/browser | Actual TFM/runtime/render-mode/browser/culture/artifact matrix, meaningful results, traces and console/network outcomes | R02/R04–R14/R16/R20/R22; platform QA |
| E05 — trim/AOT | Published trimmed and WASM-AOT consumers, executed supported binding shapes and diagnostic disposition; repeat with package | R15/R16; runtime QA |
| E06 — package | Controlled pack inputs, identity/hash/file list/API/assets and clean non-project-reference consumer results; final-byte reconciliation | R16–R18/R20; packaging QA |
| E07 — SBOM/licenses | Generation provenance, pinned tool, schema/inventory/license/freshness/checksum results and actual per-TFM package inclusion | R12/R17; supply-chain / licensing |
| E08 — signatures/provenance | Authorized owner's final package hashes, signature verification, source/build/tag binding and retained release attachments/attestations | R18/R19b; manual release owner |
| E09 — accessibility | Criterion-correct scope, automation exports, manual keyboard/pointer/AT/contrast records, limitations and evaluator approval | R21/R22; accessibility evaluator |
| E10 — performance | Reproducible inputs/hardware, correctness and median/p95 results, payload/heap/DOM/circuit/interop budgets and teardown runs | R07–R10/R23; performance QA |
| E11 — coexistence | Pinned shell/toolkit combinations, CSS/JS/DI/localization results, optional licensed commercial evidence and migration limits | R24; integration QA / licensing |
| E12 — ops/admin/support | Named owners/backups, approved support/EOL/incident/rollback/retention policy, verified admin controls and executable procedures | R13/R19/R25; support / repository admin |

### 12.6. Stop, approval and restart rules

- Package/security blockers **stop release**. No unexplained waiver, missing artifact or absent owner may become a green checkbox; do not disguise a blocker as documentation cleanup.
- Risk acceptance requires a named authorized reviewer, scoped rationale, expiry, mitigation and residual limitation. It is a disposition, **not “test passed”**, and cannot silently remove a required gate.
- Final signing, publication and artifact access remain manual owner-only operations. GitHub release metadata (including empty asset lists) does not prove NuGet signatures, SBOM attachments or provenance; current final-artifact status is **unknown**.
- Preparatory external requests may run early, but implementation and substantive gate execution follow dependencies. Pending access may permit independent eligible work, never dependent features or premature release claims.
- For each future session, update §9 with the eligible R-substep, reviewed diff, exact result/evidence links, owner/blocker and next action. Update companions by reference, not a second authoritative status system.
- Reconcile E01–E12 and the release checklist against the exact candidate; invalidate affected evidence after changes/repacking/signing and rerun the relevant gates. Historical passes are not transferable by label.
- Before handoff, validate companion links, anchors, E-ID mapping and procedure order. Documentation validation is not execution of the release commands or evidence procedures.
- **Next action remains R00a after authorization, not R00 in progress.** This update creates planning documents only. No candidate build/test/scan, SBOM repair, version change, commit, signing or publication was performed; no task or release gate is claimed satisfied.
