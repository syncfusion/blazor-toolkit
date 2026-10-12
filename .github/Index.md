# Repository Documentation Index

**Evidence reconciliation:** 2026-10-06, R01, building on pre-existing documentation edits. This is navigation, not an audit, certification, approved waiver, or release-readiness declaration. Read each document's scope: source observations, configured checks, measured local results, plans, historical decisions, and drafts are different kinds of evidence.

## Start here — current guidance and status

| Document | Scope / status |
|----------|----------------|
| [README](../README.md) | Chart-first MIT toolkit, onboarding and current limitations; no Grid commitment |
| [Roadmap](../ROADMAP.md) | Approved product direction and future work, not shipped features or delivery promises |
| [Changelog](../CHANGELOG.md) | Released history separated from unreleased local work |
| [Contributing](./CONTRIBUTING.md) / [Support](./SUPPORT.md) / [Code of conduct](./CODE_OF_CONDUCT.md) | Contribution, issue/support and community guidance; not an SLA |
| [Development](./DEVELOPMENT.md) | Setup, dated local verification records, compatibility decisions and **manual** release preparation |
| [Test matrix](./TEST-MATRIX.md) | Configured coverage versus recorded execution and outstanding platform evidence |
| [Enterprise readiness plan](../docs/ENTERPRISE-READINESS-PLAN.md) | Authoritative task state and ordered closure plan; completing baseline/documentation tasks does not establish readiness |
| [R00 retained baseline](../docs/evidence/2026-10-06-r00-1922761/index.md) | Commit `1922761`: classified build failures, diagnostic successes, 2,735 bUnit passes twice, four browser smoke passes, warnings and scan findings; not package evidence |
| [Manual/external evidence tracker](../docs/MANUAL-EXTERNAL-EVIDENCE-TRACKER.md) | E01–E12 procedures, owner/status ledger, manual testing and report formats; blank records, not approvals |
| [Release checklist](../docs/RELEASE-CHECKLIST.md) | Repeatable source-version, build/package, evidence, manual-signing and post-publication checks; copy for each release |

The GitHub release response retained by R00 records [v1.0.2 — September 1, 2026](https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.2) and [v1.0.1 — June 26, 2026](https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.1). These dates do not verify NuGet contents/signatures or promote local changes to released status. Source package version is not changed by this synchronization. Signing/publishing remain with authorized manual owners, outside repository CI.

## Licensing and dependencies

- [LICENSE.txt](../LICENSE.txt) — toolkit license; commercial products have separate licensing.
- [THIRD-PARTY-NOTICES.md](./THIRD-PARTY-NOTICES.md) — dependency/license inventory with its stated scope; not proof of package inventory completeness.
- [SBOM evidence notes](../artifacts/sbom/README.md) — repository snapshots, generation/packaging conditions and unresolved release verification; not a shipped-artifact attestation.

## Security references — guidance and open evidence

- [SECURITY.md](./SECURITY.md) — private disclosure process.
- [THREAT-MODEL.md](./THREAT-MODEL.md) — source-based trust boundaries, risks and missing disposition evidence; not an approved security assessment.
- [RENDER-MODE-SECURITY.md](./RENDER-MODE-SECURITY.md) — render-mode/host responsibilities; render mode is not an authorization boundary.

## Accessibility — drafts, not conformance results

- [ACCESSIBILITY.md](./ACCESSIBILITY.md) — goals, source mechanisms and evidence limitations.
- [DRAFT standards/component register](../docs/VPAT-2.5-INT.md) — historical filename retained; not an approved VPAT/ACR or certification.
- [Insights evidence register](./accessibility/insights-summary.md) — Not Evaluated; future assessment checklist, not retained passing results.
- [Screen-reader validation checklist](./accessibility/screen-reader-smoke.md) — Not Evaluated; requires actual assistive-technology session records.

## Historical engineering records

The previously linked `docs/BL0007-analysis.md` and `docs/ChartModels-Batch2.md` are **absent from this checkout**. Their claimed experiment/approval records are not available through this index. Recover original evidence from the responsible owner/history before relying on it; do not reconstruct approvals or undo source changes from an unavailable narrative. [Development notes](./DEVELOPMENT.md#known-analyzer--trim--aot-findings) retain their own dated claims and limitations.

## Performance — source observations and unexecuted manual plans

- [Virtualization strategy](./evidences/performance/virtualization-startegy.md) — fixed/eager/configuration-sized collections; no demonstrated `Virtualize`/`PageSize` windowing. The existing `virtualization-startegy.md` path is intentionally unchanged.
- [Render-tree efficiency](./evidences/performance/render-tree-efficiency.md) — actual SVG component/builder pipeline, conditional subtrees and unmeasured costs.
- [ShouldRender behavior](./evidences/performance/shouldRender-optimization.md) — chart and Tooltip overrides versus subtree conditions; correctness/performance checks remain planned.
- [Key usage](./evidences/performance/key-usage.md) — calendar/time/chart/TextBox/dialog/uploader keys, unkeyed paths and identity caveats.

None of these documents supplies a measured performance baseline or a conformance pass. See the enterprise plan's R23 gate for required reproducible results.

## Samples and component guidance

- [samples/](../samples/) — checked-in examples; source existence does not establish a clean released-package consumer test.
- [Component skills/reference directory](./skills/) — implementation guidance; verify API examples against the chosen source/package version.
- [Toolkit demos](https://blazor.syncfusion.com/demos/toolkit/) — hosted examples; not evidence that the local tree or every supported configuration passed.

## Evidence still needed

Use the linked plans/status records to locate missing clean-checkout/runtime/browser results, package/API/trim/WASM-AOT validation, manual-owner release signatures/provenance/SBOM verification, measured Chart workloads and actual accessibility/AT reports. A navigation entry, local warning/test count, snapshot, or draft title does not close those gates.