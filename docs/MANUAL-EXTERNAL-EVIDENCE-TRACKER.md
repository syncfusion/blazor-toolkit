# Manual testing and external evidence tracker

**Created:** 2026-10-03. **Status:** reusable procedures and blank records; no assessment, approval, or test result is created by this file.

[Readiness plan](ENTERPRISE-READINESS-PLAN.md#12-remaining-issue-closure-plan) · [Release checklist](RELEASE-CHECKLIST.md) · [Test matrix](../.github/TEST-MATRIX.md) · [Private security reporting](../.github/SECURITY.md)

The readiness plan's **R00–R25 are implementation tasks**. **E01–E12 below identify evidence families**, not another implementation backlog. A passing source build does not satisfy a package, browser, security, or accessibility assessment. Request access and assign owners early; execute each procedure only after its prerequisites exist.

## 1. How to use this tracker

1. Choose the proposed release/candidate and assign a named owner, backup and reviewer to each applicable E-ID. Suggested roles below are **not assignments**.
2. Copy the [candidate record](#3-candidate-record) and [result template](#4-result-and-finding-templates) into a candidate-specific evidence index. Keep this master procedure reusable.
3. Split each family into concrete cases, e.g. `E04-server-net10-chromium-001` or `E09-chart-keyboard-001`. Use the matrix row identity in filenames and reports.
4. Record `NOT STARTED`, `IN PROGRESS`, `BLOCKED`, `PASS`, `FAIL`, or `NOT APPLICABLE`. `NOT APPLICABLE` requires scope rationale and reviewer approval. A waiver is a **separate release decision**, never a passing test.
5. Assign each case a stage: `EXPLORATORY` (before version approval), `PREPUBLICATION` (candidate verification), `AUTHORIZATION` (owner's go/no-go decision), or `POSTPUBLICATION` (downloaded-package checks). Evidence families can contain cases at several stages. Only required PREPUBLICATION cases precede authorization; POSTPUBLICATION cases remain openly pending until publication and are mandatory for closeout where applicable.
6. Record expected behavior **before execution**, then observed results, counts, raw evidence and reviewer decision. Failed, skipped, retried and incomplete cases remain visible.
7. Close an R-task only when its full acceptance gate is met on the relevant source/package. Do not copy a historical pass onto a new SHA or package hash. Do not require an authorization decision as its own prerequisite.
8. Reopen affected records after changes to code, dependencies, generated assets, package bytes, browser/AT versions or supported scope. For unchanged evidence, document applicability and reviewer approval rather than silently reusing it.

### Tracking ledger — fill for each candidate

All rows below start without current-candidate evidence. Historical local results can be linked as background, not entered as new passes.

| ID | Evidence family / R-tasks | Suggested accountable role | Owner / backup / reviewer | State | Due date / blocker | Record / immutable evidence link |
|---|---|---|---|---|---|---|
| E01 | Checkout, environment, build/test baseline — R00–R02 | Engineering lead / QA | UNASSIGNED | NOT STARTED | TBD | — |
| E02 | Version decision and released API comparison — R03 | API maintainer / release owner | UNASSIGNED | NOT STARTED | TBD | — |
| E03 | Security review, negative tests and scan coverage — R04–R12, R19a | Security owner | UNASSIGNED | NOT STARTED | TBD | — |
| E04 | Runtime, render-mode and browser matrix — R11, R13–R14 | QA owner | UNASSIGNED | NOT STARTED | TBD | — |
| E05 | Published trimming and WebAssembly AOT — R15 | Runtime/build owner | UNASSIGNED | NOT STARTED | TBD | — |
| E06 | Exact-package consumers and onboarding — R16, R20 | Package maintainer | UNASSIGNED | NOT STARTED | TBD | — |
| E07 | SBOM, dependency and license inventory — R17 | Supply-chain owner / legal reviewer | UNASSIGNED | NOT STARTED | TBD | — |
| E08 | Signatures, source identity and provenance — R18, R19b | Authorized release owner | UNASSIGNED | NOT STARTED | TBD | — |
| E09 | Accessibility, keyboard, pointer and AT — R21–R22 | Accessibility evaluator | UNASSIGNED | NOT STARTED | TBD | — |
| E10 | Chart correctness, performance and lifetime budgets — R23 | Performance / QA owner | UNASSIGNED | NOT STARTED | TBD | — |
| E11 | Coexistence and optional commercial integration — R24 | Integration owner | UNASSIGNED | NOT STARTED | TBD | — |
| E12 | Repository controls, support and release recovery — R13, R25 | Maintainer / repository administrator | UNASSIGNED | NOT STARTED | TBD | — |

## 2. Evidence storage and handling

Use a durable, access-controlled release evidence store, or reviewed non-sensitive files under a proposed `docs/evidence/<release>/<candidate>/` directory. That directory is **not created or populated by this template**. An immutable CI artifact URL is acceptable only if it remains available for the approved retention period; current CI uploads expire after 14 days.

Suggested bundle layout (names are a convention, not existing reports):

```text
<release>/<candidate>/
  index.md                 # Candidate identity, ledger, review and release decision
  environment.txt          # Sanitized tool/runtime/OS details
  E01/                     # Build/discovery/TRX/coverage logs
  E02/                     # API diff, consumer diagnostics, version approval
  E04/                     # Per-host/browser logs, screenshots and traces
  E05/                     # Publish logs and published-output run results
  E06/                     # Package file lists and consumer lock/assets records
  E07/                     # Validated SBOMs, inventory review, checksums
  E08/                     # Public verification output; restricted records linked
  E09/                     # Criteria matrix, manual sessions, measured geometry
  E10/                     # Raw benchmark samples and calculations
  E11/                     # Coexistence matrix and migration notes
  E12/                     # Redacted control/approval/rollback records
  SHA256SUMS               # Checksums of retained evidence files
```

- Keep E03 vulnerability details, credentials, signing procedures, protected admin exports and unredacted network/AT recordings in the approved private system. The public index should contain only a safe reference and disposition.
- Do not retain API keys, private keys, tokens, cookies, personal data, live customer data or passwords in reports. Sanitize HAR files, traces, screenshots and logs before sharing; record the redaction method.
- Record separate hashes for raw restricted evidence and redacted copies when policy permits. A checksum proves byte consistency, not authenticity or evaluation quality.
- Assign a retention owner, retention end date and access policy. Retain release evidence beyond short CI retention; do not invent an organization-wide legal retention period.
- Freeze completed records. Corrections create a dated revision referencing the prior record rather than overwriting adverse results.

## 3. Candidate record

Copy this into the candidate's `index.md`. Resolve all required PREPUBLICATION placeholders before go/no-go; keep authorization and postpublication records visibly pending until those stages occur. An evidence family's overall state must not hide unfinished cases at a later stage.

```markdown
# Candidate evidence index

- Release version / proposed tag: UNAPPROVED
- Candidate identifier: TBD
- Source repository and immutable commit SHA: TBD
- Working-tree state and patch/diff hash (if exploratory): TBD
- Package ID/version and artifact stage (unsigned / final signed / downloaded): TBD
- Exact package SHA-256 and durable location for each artifact stage: TBD
- Evidence stage: EXPLORATORY | PREPUBLICATION | AUTHORIZATION | POSTPUBLICATION
- Generated asset manifest/hash: TBD
- SDK, runtimes, workloads, Node/npm, tools and lockfile identity: TBD
- Evaluated frameworks, render modes, browsers, OS/devices, themes/cultures: TBD
- Explicitly excluded scenarios and reasons: TBD
- Required E-IDs/cases, stage and release-blocking/closeout criteria: TBD
- Owner / backup / reviewer and approval date: UNASSIGNED
- Evidence-store location, access policy and retention end: TBD
- Security/private record references (no sensitive details): TBD
- Overall state: NOT STARTED
- Release decision: NOT REQUESTED
- Postpublication verification / closeout: NOT STARTED
- Revalidation triggers: source, package, dependency, asset or scope changes

| Case ID | R-task | Evidence stage | Artifact identity | Required for | Owner / reviewer | Status | Evidence / blocker |
|---|---|---|---|---|---|---|---|
| <E-ID>-scenario-001 | TBD | TBD | TBD | Version decision / go-no-go / authorization / closeout | UNASSIGNED | NOT STARTED | — |
```

For a local source exploration without a clean commit, label it **exploratory dirty-tree evidence**. It may help reproduce a defect but cannot stand in for a clean released-artifact result. Pre-sign, final signed and downloaded packages need distinct identities even if their assembly payloads agree.

## 4. Result and finding templates

### Per-case report

Choose the applicable family E01–E12 for `<E-ID>`; the placeholder is not a thirteenth evidence family.

```markdown
# <E-ID>-scenario-001 — descriptive test name

- R-task(s) / requirement / criterion: TBD
- Candidate SHA / package version / package SHA-256: TBD
- Scope: source | unsigned package | final signed package | downloaded package
- Stage: EXPLORATORY | PREPUBLICATION | AUTHORIZATION | POSTPUBLICATION
- Owner / executor / independent reviewer: UNASSIGNED
- Start/end (UTC), OS/device, framework/runtime, host mode: TBD
- Browser/AT/tool versions, theme, culture, viewport/DPR/zoom: TBD
- Preconditions and fixtures (including data counts/seed): TBD
- Expected behavior / acceptance threshold agreed before run: TBD
- Exact steps and commands (secrets removed):
  1. TBD
  2. TBD
- Observed behavior / measurements: NOT RUN
- Status: NOT STARTED
- Discovered / executed / passed / failed / skipped / retried: NOT RUN
- Raw log/report/trace/screenshot references and SHA-256: NONE
- Limitations and uncovered scenarios: TBD
- Finding IDs and severity rationale: NONE ASSESSED
- Reviewer decision/date: NOT REVIEWED
- Retest trigger, follow-up owner, and due date: TBD
```

### Finding and remediation record

```markdown
# Finding <private-or-public-id>

- Evidence case / affected version / SHA / hash: TBD
- Expected versus actual behavior: TBD
- Reproduction and trust boundary: restricted link if sensitive
- Severity / impact / affected configurations / confidence: TBD
- Implementation R-task / owner / target milestone: TBD
- Fix commit / regression test: NOT IMPLEMENTED
- Before-fix failure evidence: NOT RUN
- After-fix and wider regression evidence: NOT RUN
- Residual risk / unsupported cases: TBD
- State: needs investigation | reproduced | fixed pending retest | verified | blocked
- Reviewer / decision date: UNASSIGNED
```

### Exception or not-applicable decision

```markdown
- Case/criterion and candidate scope: TBD
- Decision: NOT APPLICABLE | TIME-BOUNDED RELEASE EXCEPTION | HOLD
- Evidence and rationale (not "test noisy"): TBD
- Impact, compensating controls and public limitation: TBD
- Authorized risk/release owner and independent reviewer: UNASSIGNED
- Approval date / expiry / review trigger: NOT APPROVED
- Remediation task, owner and due date: TBD
- Gates that remain failed/blocked: TBD
```

Unknown signatures, failed identity checks, unapproved versions, or missing required evidence are **stop conditions**, not items to mark N/A for convenience. An accepted risk does not authorize a false public conformance claim.

## 5. E01 — Baseline and retained automation results

**Access:** source checkout, required SDK/runtime/workloads, CI read access. **Output:** environment manifest, clean-tree SHA, build/discovery/test logs, TRX, coverage, and CI links.

1. Select the approved branch/SHA and verify the working tree. Record `git rev-parse HEAD`, status, tool versions, target frameworks and lockfile hashes. Follow [development prerequisites](../.github/DEVELOPMENT.md#prerequisites).
2. Generate styles, restore and build explicitly named projects. Retain the exit code and full diagnostic log, not just a screenshot of a green badge.
3. Discover and run the main bUnit project; retain counts and failures. Do not count SDK-labelled CI jobs as different test runtimes: the current test project targets `net8.0`.
4. Download the matching CI and CodeQL run evidence while available. Verify their actual tested SHA, selected SDK/runtime and analyzed-file scope. A successful job with zero discovered tests is not acceptable.
5. Review retries, ignored failures, skips, generated-file changes and warnings. Compare to the agreed baseline and link unresolved findings.

**Accept only when:** results refer to the chosen candidate, required checks really ran, nonzero discovery is verified, and failures have an explicit disposition. The historical 2,735-pass local result is background, not this candidate's result.

## 6. E02 — Version approval and API compatibility

**Access:** verified previous release package and API-review owner; exploratory source/build outputs initially, approved-version candidate package later. **Output:** reviewed API/metadata diff, source/binary consumer results and signed-off version/migration decision.

**Two stages:** first use explicitly EXPLORATORY outputs from the selected source to assess the delta and choose a version; do not publish them or treat their temporary package identity as a release. After the approved version-bearing commit, repeat compatibility checks against the actual candidate package as PREPUBLICATION evidence under R16. Version selection does not require an already approved-version package; final package verification does.

1. Confirm the previous published version/tag/package independently. Reconcile source `1.0.0` versus the recorded public `v1.0.2`; do not rewrite old tags or choose the next version solely by incrementing a number.
2. Capture a released API baseline from the real package using an approved API/package validation tool; record its name/version and exact invocation. This gate is not implemented by current configuration.
3. Compare CLR members, nullable metadata, generic constraints, Blazor parameters, serialized names and JS callback contracts. Explicitly assess `DataAdaptor<T> where T : notnull`, changed nullability and exception behavior.
4. Compile representative existing consumers with normal diagnostics and nullable warnings-as-errors. Test binary compatibility separately where claimed. Include generic custom adaptors, parameter binding and serialization round trips.
5. Prove the compatibility gate catches a deliberate temporary breaking change in an isolated test checkout, then discard that probe and verify the real candidate. Never commit the probe or silently re-baseline failures.
6. Record the approved SemVer and migration notes before the version-bearing candidate commit. Existing patch-numbered releases are historical facts, not permission to hide future breaks.

**Accept only when:** every difference is explained, the version is explicitly approved, and the compared artifact identities are retained.

## 7. E03 — Security and independent review

**Access:** designated security owner; an independent internal reviewer or authorized external specialist with agreed scope; isolated test environment. **Output:** private report, safe findings index, negative-test logs and retest report. An external certification is not presumed or automatically required.

1. Agree rules of engagement: exact package/commit, component/host boundaries, allowed local targets, test data, prohibited actions and disclosure route. Never test customer or third-party systems without authorization.
2. Map R04–R10 and R12 to input sources, sanitization/encoding, browser sinks, upload endpoints, mutable state, disposal/callbacks and dependencies. Trace reachability before assigning severity.
3. Use synthetic text/files and dummy headers. Test chart text/Unicode/line breaks, upload selection/preload metadata, template trust boundaries, allowed browser headers and resource budgets. Assert observable DOM/network behavior and no unintended requests, not simply absence of a string.
4. Exercise cancellation, duplicate disposal, disconnected circuits, stale async responses, malformed numeric/index payloads and cross-instance state. Prove each regression fails before its fix where reproducible.
5. Review actual shipped JS plus the .NET/data surface. Record ESLint/XSS exclusions and CodeQL file coverage; configured tools alone do not close uncovered paths. Review dependency and bundled-asset inventory separately.
6. Ask the reviewer for methodology, tool versions, scope/exclusions, evidence, reproducible private findings, severity rationale, remediation and retest disposition. Redact secrets/personal data and publish only approved summaries.

**Accept only when:** required findings are remediated and retested or explicitly dispositioned by the authorized owner; unresolved blockers hold release. No blanket “ASVS certified” or vulnerability-free assertion follows.

## 8. E04 — Runtime and browser manual matrix

**Access:** representative hosts and real browsers/devices for the approved support scope. **Output:** one record per actual combination, logs/trace and case table.

```markdown
| Case ID | TFM + actual runtime | Host mode | Browser/OS/device | Culture/theme/viewport | Action | Expected | Observed | Status | Evidence |
|---|---|---|---|---|---|---|---|---|---|
| E04-001 | TBD | Server/WASM/Auto/static | TBD | TBD | TBD | TBD | NOT RUN | NOT STARTED | — |
```

1. Obtain R13's approved scope. Mark unavailable combinations BLOCKED; do not silently reduce scope. Current Chromium/net10 Server configuration is only a starting point.
2. Run clean-session initial navigation, prerender-to-interactive hydration and enhanced navigation. Confirm which runtime executed; for Auto, test both initial Server and subsequent WASM execution rather than inferring the mode.
3. Exercise Chart data binding, axes/legend, selection, tooltip/crosshair, zoom/pan, resizing, empty/null data and dynamic updates; test supporting inputs/popups/upload feedback used by the scenario.
4. Navigate away/back, reconnect where supported, replace data, dispose/mount repeatedly, and run simultaneous charts with different culture/theme/RTL settings. Watch console/network errors, stale updates and focus.
5. Record screenshots and meaningful assertions; retain Playwright traces for failures and retries. Reproduce browser-specific issues manually. Test native Safari/device combinations explicitly if promised; emulation is not equivalent.
6. For static SSR, verify only the documented static output contract and absence of accidental JS during rendering. Do not expect interactive behavior or claim parity for unsupported TFM/component combinations.

**Accept only when:** approved combinations/scenarios are executed with expected behavior and reviewed logs. An absent browser/device/host is a scope decision or blocker, never a pass.

## 9. E05 — Trimmed and WebAssembly AOT publication

**Access:** supported SDK/workloads and source consumers for initial R15, followed by package-only consumers under R16. **Output:** publish logs, warnings, output manifests and published-app browser results.

**Dependency order:** R15 may first use a project reference to the selected source, explicitly labelled source-consumer evidence. R16 then repeats the supported scenarios with the actual package and no toolkit project reference. Final signing/payload changes trigger affected reruns. Source publication does not satisfy the final package gate; requiring the package at initial R15 would create a dependency cycle.

1. Define supported consumer shapes: ordinary typed data binding and separately reviewed dynamic/reflection paths. Record the source SHA/consumer for initial R15 and pin package version/hash for the R16 rerun.
2. Publish the relevant host with its supported trimming settings; for Blazor WebAssembly use `RunAOTCompilation=true` on the client project. Server Native AOT is a different deployment capability and must not be inferred from WASM AOT.
3. Capture every linker/AOT warning and suppression rationale. Do not preserve every assembly or disable analyzers merely to obtain a successful publish.
4. Serve the published output, not the development server, and repeat binding/rendering/interaction scenarios. Retain the exact output manifest, actual runtime and browser results.
5. Repeat against the final package if assembly signing/repackaging changes the payload. Record excluded dynamic APIs and user-required annotations honestly.

**Accept only when:** the declared published scenarios run and relevant diagnostics are resolved/dispositioned. `IsTrimmable`/`IsAotCompatible` flags are not evidence of this result.

## 10. E06 — Package consumers and onboarding

**Access:** unsigned candidate initially, final signed package before authorization, and independently downloaded package after publication; isolated package cache/feed and clean consumer hosts. **Output:** package listing, `.nuspec`, restore graph, compiled/running samples and migration results.

1. Inspect all expected framework DLLs/XML docs, PDB/SourceLink information, scripts/styles, license/readme/notices, dependency groups and required SBOM entries. Compare package ID/version and repository commit to approved inputs; retain the file list and exact package hash.
2. Create clean consumers with Toolkit `PackageReference` only, using a candidate-specific feed/cache. Remove all toolkit project references and workspace asset fallbacks. Retain restore records proving the intended package bytes were used, including transitive dependencies.
3. Follow the README and selected guides verbatim from a fresh project. Record prerequisites, elapsed setup time, failures and corrected instructions. Run—not merely compile—the page and validate data, interactivity and static-asset requests.
4. Exercise the approved E04/E05 combinations and representative custom-adaptor/serialization scenarios against the package. A gallery project reference is not a substitute.
5. Repeat essential checks after final packaging/signing (PREPUBLICATION), and after authorized publication using an independently downloaded package (POSTPUBLICATION). Keep download cases pending before publication; they are closeout requirements, not prerequisites for the act of publishing. Distinguish exact package hashes and any repository-added signatures.

**Accept only when:** consumers use the intended package bytes and supported scenarios pass for the case's stage. Prepublication approval covers the frozen candidate; downloaded-package cases close only after publication. The earlier README compile check against 1.0.2 establishes neither future candidate behavior nor browser results.

## 11. E07 — SBOM, checksum and license reconciliation

**Access:** original generation inputs/history, exact candidate package, approved CycloneDX/schema validator and license reviewer. **Output:** provenance decision, schema results, package inventory reconciliation and checksum manifests.

1. Preserve the mismatching `artifacts/sbom/sbom.cdx.json` and its recorded digest as historical evidence. Recompute the digest without editing either file; compare bytes, generation inputs, timestamps and tool history. A mismatch alone does not prove tampering.
2. Decide whether the old snapshot can be reproduced or must be explicitly retired/superseded. Do **not** overwrite just the checksum or search-and-replace product versions to manufacture consistency.
3. Implement and test fresh generation/inclusion under R17. Pin/record the tool and schema. Account for `CI=true` skipping current generation, `--no-build`, multi-target inner/outer builds, and stale mirrors; missing required output must fail the gate.
4. Generate from the same controlled candidate inputs. Validate CycloneDX against its actual declared schema; distinguish schema version, document revision, tool version and product version. Record any SPDX requirement separately—no current generator is presumed.
5. Reconcile product name/version/PURL/root references, all target-framework dependencies, bundled browser/style assets and relevant build tools against package inventory and license notices. Explain exclusions rather than asserting completeness from a package-manager graph.
6. Inspect actual embedded `_sbom/cyclonedx/<tfm>/` files or the explicitly approved external-distribution contract. Hash exact SBOM bytes after final formatting; bind external SBOMs to final package hashes in a separate manifest to avoid circular self-hashes.
7. Have the supply-chain/legal reviewer sign off redistribution notices and unresolved origins. A successful JSON parse or checksum match is not license approval.

**Accept only when:** provenance is explained, schema/inventory/identity checks pass, distribution matches the approved contract, and final evidence is retained.

## 12. E08 — Manual signing, provenance and repository identity

**Access:** authorized release owner and approved signing environment; repository administrator for protected settings. **Output:** redacted verification logs and approver records, not credentials.

1. Request the authorized procedure and scope: assembly strong-name, optional/required DLL Authenticode, NuGet signature, timestamp policy, and repository countersignature. These are separate controls; N/A needs a documented scope decision.
2. Bind approved SHA, clean build inputs, tool/dependency identities and each package stage to a retained manifest. Verify repository ancestry and tag policy with the owner; local metadata alone does not prove trusted provenance.
3. Have only the authorized owner perform required signing in the approved environment. Assembly signing precedes final packaging; record changed payloads and repeat affected consumer checks. Never request or expose credentials in this tracker.
4. Run the approved signature-verification procedure and retain tool/version, exit code, signer identity, chain/timestamp status and artifact hash. Check the exact final package, not a prior unsigned smoke artifact.
5. Inspect final contents and revalidate affected consumers. Freeze the approved bytes. Record a detached manifest or approved provenance statement with verifiable issuer/source/build linkage; do not claim a SLSA level from metadata alone.
6. After authorized publication, verify the downloaded package and tag mapping independently as POSTPUBLICATION cases. Repository-added signatures can change package bytes; reconcile this through signature/content verification and retain both hashes, not by requiring unexplained byte equality.

**Accept only when:** PREPUBLICATION identity/signature evidence passes for the frozen final artifact, followed by the separate owner AUTHORIZATION decision. Downloaded-package verification remains pending until publication and is required for closeout. Missing signing access is BLOCKED, not permission to skip signing.

## 13. E09 — Manual accessibility and external assessment

**Access:** accessibility evaluator, supported browsers/devices, Windows AT where in scope, macOS VoiceOver, and approved synthetic scenarios. An independent assessor can be internal or external; procurement certification is not presumed.

Use the [draft standards register](VPAT-2.5-INT.md), [screen-reader checklist](../.github/accessibility/screen-reader-smoke.md) and [Insights register](../.github/accessibility/insights-summary.md) as scenario inventories. Choose actual supported browser/AT pairings and record versions; access to one platform does not validate another.

### Execution procedure

1. Define package hash, page/process scope, components, themes, cultures, viewports and applicable WCAG A/AA criteria. Include composed application tasks, not isolated markup alone.
2. Run an approved automated accessibility tool in meaningful states: initial, focused, open popup, validation error, loading, selected and disabled. Export raw reports including incomplete/manual checks; zero automated findings is not full conformance.
3. **Keyboard:** use Tab/Shift+Tab, arrows, Enter/Space and documented shortcuts to complete each task; verify visible focus, logical order, no inescapable trap, modal exit/return, and chart point/legend/zoom behavior. Record expected and actual focus targets.
4. **Single pointer:** complete drag-related tasks using equivalent non-dragging single-pointer operations, or document a criterion exception. Keyboard support alone does not satisfy WCAG 2.5.7. Test actual dialog movement/resizing and chart selection/pan/zoom scenarios.
5. **Screen reader:** record a reproducible action sequence and expected/observed speech for names, roles, states, values, errors and status changes. Include chart summaries/equivalent data, dynamic updates, dialog focus and tooltip relationships. Redact recordings before sharing.
6. **Visual measures:** measure text/non-text contrast in actual states, focus visibility/obscuration, target hit bounds/spacing, 200% text resize, 400% zoom/reflow where applicable, text-spacing overrides, forced colors, reduced motion, RTL and responsive layout. Record values and criterion exceptions, not only screenshots.
7. Map each criterion to evidence and a justified Supports/Partially Supports/Does Not Support/Not Applicable decision only after assessment. Leave insufficient evidence Not Evaluated. WCAG 2.4.11 is AA; 2.4.13 is AAA. Assess EN 301 549/Section 508 scope separately.
8. Resolve findings and retest; obtain named evaluator/reviewer approval before producing an ACR in the appropriate template. A completed template is not certification by itself.

### Required session and measurement formats

```markdown
| Step | Input/action | Expected focus/speech/result | Observed focus/speech/result | Criterion | Status | Screenshot/recording/timecode |
|---|---|---|---|---|---|---|
| 1 | TBD | TBD | NOT RUN | TBD | NOT STARTED | — |

| Element/state | Theme/viewport/zoom | Foreground/background or target bounds | Measured ratio/size/spacing | Criterion/exception | Status | Tool/evidence |
|---|---|---|---|---|---|---|
| TBD | TBD | TBD | NOT MEASURED | TBD | NOT STARTED | — |
```

### What to request from an external assessor

Supply the candidate identity, scope matrix, synthetic fixtures, instructions and known limitations under the approved disclosure arrangement. Request: evaluator identity/qualifications, methodology, standards/version and scope, test environment, manual/automated coverage, raw evidence references, criterion-level reasoning, reproducible findings, exclusions, and a remediation/retest section. Require an assessment date and reviewer signoff; do not request a predetermined pass or invent an accreditation.

**Accept only when:** the agreed scope has real evidence and remaining limitations are explicit. Publication of an ACR requires owner approval; no manual session is pre-filled as PASS.

## 14. E10 — Performance, reliability and correctness

**Access:** representative hardware/browser/server, performance owner and approved budgets. **Output:** reproducible harness, raw samples, calculations and approval.

1. Agree workloads and budgets before measuring: initial usable render, incremental update, tooltip/selection/pan/zoom, resize, concurrent charts and repeated mount/unmount. Use 1k/10k/100k points only where supported; disclose aggregation, sampling and disabled features.
2. Record hardware, runtime/mode, browser, network, package/hash, dataset seed, theme/culture, warmup and run count. Measure from before the operation to a defined usable-result endpoint; timing after navigation/network idle is not initial-render latency.
3. Collect raw timings, allocations/browser heap/DOM counts, server-per-circuit growth, interop counts and payload delta over an equivalent empty host. Record instrumentation and garbage-collection conditions.
4. Exercise null/NaN/infinite values, duplicate categories, date boundaries, hidden containers, rapid updates and disposal correctness alongside speed.
5. Calculate median/p95 from retained samples; report failed runs/outliers separately without silently dropping them. Compare under the same conditions and explain variance. Owner sets limits from evidence—this file supplies no invented universal target.

```markdown
| Case | Data/features/mode | Hardware + package | Warmup/measured N | Start/end definition | Median | p95 | Memory/DOM/interop delta | Approved limit | Result | Raw samples |
|---|---|---|---|---|---|---|---|---|---|---|
| E10-001 | TBD | TBD | TBD | TBD | NOT RUN | NOT RUN | NOT RUN | UNAPPROVED | NOT STARTED | — |
```

**Accept only when:** correctness and agreed performance/resource budgets pass with reproducible raw evidence. Source optimizations alone are not benchmarks.

## 15. E11 — Coexistence and commercial access

1. Build a version-pinned package consumer using Toolkit Chart inside the chosen other-library application shell. Record themes/CSS order, service registration, script requests and namespace aliases.
2. Test navigation, dialogs/tooltips, keyboard focus, localization, theming and multiple charts; look for CSS/global JS collisions. Retain actual package/host/browser evidence.
3. For optional commercial testing, request authorized dependencies/licenses through the appropriate owner. Keep credentials/proprietary artifacts private. Lack of access blocks R24b only; it need not block independent R24a work.
4. Document coexistence and migration separately: exact code/configuration changes, licensing boundaries and unsupported scenarios. Do not promise a package-only swap without proof.

**Accept only when:** tested versions and actual differences are recorded. Removing commercial claims requires an approved scope decision, not a fabricated pass.

## 16. E12 — Repository controls, support and recovery

1. Assign release/security/component owners, backups and escalation routes. Approve support windows, framework EOL/backport policy and attainable response targets; existing best-effort wording is not an SLA.
2. Have a repository administrator export/redact rulesets, required checks, review requirements, tag permissions, environment protections and bypass roles. Verify the checks correspond to real job names and intended branches; YAML alone cannot prove enforcement.
3. Review permissions and credential custody without exposing secret values. Keep manual publishing outside public CI unless policy is separately changed and approved.
4. Perform a release-recovery tabletop: simulate a defective package, identify the authorized promotion hold/unlisting/advisory decision makers, and demonstrate consumer rollback instructions. Never overwrite an existing version or move a published tag; retain the scenario, decisions, gaps and follow-up owners without changing a live release.
5. Confirm evidence retention, backup and reviewer access beyond the current 14-day CI expiry. Test retrieval of sanitized reports; record the storage owner and retention policy without exposing restricted records.
6. After required PREPUBLICATION records have been reviewed, record the separate AUTHORIZATION decision: candidate identity, required E-records, unresolved risks, approvers/date, hold criteria and post-release monitoring/triage owner. This decision is not a prerequisite for itself. Track downloaded-package and monitoring cases as POSTPUBLICATION closeout work. A signature on a summary cannot replace missing mandatory evidence.

**Accept only when:** controls and responsibilities are owner-confirmed, recovery steps are executable, and the appropriately staged release approval/closeout references the final artifact and evidence index.

## 17. Open requests and review cadence

Copy a row for every dependency that needs human/external action. Do not leave a bare unchecked box without an owner and unblock condition.

| Request ID | E-ID / R-task | Requested deliverable/access | Owner/contact role | Requested / due | Status | Blocker / next follow-up | Response/evidence | Reviewer |
|---|---|---|---|---|---|---|---|---|
| REQ-01 | E02 / R03 | Approve previous release baseline and next version | Release/API owner — UNASSIGNED | TBD | NOT STARTED | API comparison required | — | UNASSIGNED |
| REQ-02 | E07 / R17 | Reconcile SBOM inputs/checksum and distribution policy | Supply-chain owner — UNASSIGNED | TBD | NOT STARTED | Original provenance required | — | UNASSIGNED |
| REQ-03 | E08 / R18 | Provide authorized signing/verification procedure | Release owner — UNASSIGNED | TBD | NOT STARTED | Private access required | — | UNASSIGNED |
| REQ-04 | E09 / R21–R22 | Agree assessment scope and AT evaluator access | Accessibility owner — UNASSIGNED | TBD | NOT STARTED | Platform/evaluator availability | — | UNASSIGNED |
| REQ-05 | E12 / R25 | Confirm owners, retention and release recovery | Maintainer/admin — UNASSIGNED | TBD | NOT STARTED | Governance decisions required | — | UNASSIGNED |

Review this ledger at each readiness checkpoint and candidate change. Record actual dates and outcomes; no fixed service commitment or external-report deadline is established by this template.
