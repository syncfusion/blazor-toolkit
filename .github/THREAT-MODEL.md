# Threat Model

## Purpose

This document describes the current security posture of the Syncfusion Blazor Toolkit project, the trust boundaries for the package and sample applications, and the primary threats most relevant to a UI component library distributed as a NuGet package.

## Scope

This threat model covers:

- the Blazor component library in the `src/` directory
- generated static assets under `src/wwwroot/`
- the sample applications in `samples/`
- the build and packaging flow used to produce NuGet assets
- the GitHub repository and CI/CD workflow used for source control and releases

It does not cover downstream application code built on top of the toolkit, which is the responsibility of the consuming application owner.

## Assets and value

The primary assets are:

- source code for reusable Blazor components
- package identity and versioning
- static CSS/JavaScript assets bundled with the toolkit
- trusted developer workflows used by contributors and maintainers
- repository integrity and release provenance

## Trust boundaries

1. Source repository boundary
   - GitHub is the authoritative source for pull requests, review, and release artifacts.
   - Untrusted contributors are allowed to submit patches, but changes are reviewed before merge.

2. Build and packaging boundary
   - NuGet packages and static assets are generated locally and in CI from source.
   - Dependencies are expected to be reviewed and pinned to explicit versions.

3. Consumer application boundary
   - The toolkit is loaded into a consuming Blazor app and rendered in a browser context.
   - The application is expected to enforce its own content security, authentication, and authorization policies.

4. Browser execution boundary
   - Components execute in the browser and therefore must avoid unsafe HTML rendering, untrusted data execution, or insecure script injection.

## Threats and mitigations

### 1. Supply-chain compromise

Risk:
- a dependency, build script, or package registry entry is compromised or updated unexpectedly.

Mitigations:
- use explicit package versions and review dependency updates before release
- keep the repository and CI workflow under maintainer control
- validate generated assets before publishing packages
- avoid executing untrusted scripts during the build pipeline
- shipped assemblies are **not** strong-name signed (`PublicKeyToken=null`; AR-1) and not Authenticode signed (AR-2). Package integrity rests on the NuGet package signature applied by the `release.yml` workflow (`dotnet nuget sign` + `dotnet nuget verify`), the SLSA build-provenance attestation, and SBOMs generated from the final signed package. The signing certificate and registry credentials are available only to `release.yml` (triggered by a published GitHub release) through repository secrets and the job `GITHUB_TOKEN`; pull-request CI never has them. See Accepted Risks AR-1, AR-2, AR-4 and AR-5.
- the release workflow fails closed: the NuGet vulnerability scan must complete without command, restore or advisory-source errors before its structured result is evaluated, and the AI skill documentation is checked against the packed public API.

### 2. Cross-site scripting (XSS) through user content

Risk:
- a component could render untrusted markup or script content in a way that executes in the browser.

Mitigations:
- prefer strongly typed component APIs over raw HTML injection
- avoid direct `HtmlString` or unsafe markup rendering unless required and explicitly handled
- ensure any user-driven content is sanitized or encoded before display
- validate interactions that produce dynamic markup within the component library

### 3. Unsafe JavaScript interop and browser APIs

Risk:
- excessive or careless interop could expose the app to script injection or unsafe browser actions.

Mitigations:
- keep interop usage minimal and scoped to required functionality
- validate arguments passed across boundaries
- prefer safe patterns and avoid remote script execution from untrusted sources

### 4. Malicious content in static assets or generated styles

Risk:
- bundled CSS or third-party assets may contain unsafe patterns or unexpected logic.

Mitigations:
- treat generated styles as part of the deliverable and review changes in PRs
- keep the asset pipeline deterministic and version-controlled
- review third-party packages before inclusion in the package output

### 5. Repository and release integrity issues

Risk:
- branch protection, tagging, or release metadata is bypassed or a release artifact is published from an untrusted source.

Mitigations:
- use protected branches where possible
- require review before merge to the main branch
- validate release artifacts before publishing to NuGet
- maintain a clear audit trail in the repository history

## Accepted risks

The following risks have been reviewed and accepted by the maintainers. Each entry includes the rationale and the date of acceptance. Accepted risks are re-evaluated at every major release.

| # | Risk | Rationale | Owner | Accepted |
|---|---|---|---|---|
| AR-1 | Shipped assemblies are not strong-name signed (`PublicKeyToken=null`) | **Policy A — accepted.** No strong-name key exists or is used, in CI or elsewhere; the repository contains no `SignAssembly=true` directive and no `AssemblyOriginatorKeyFile` value. Assembly identity is therefore not a trust anchor: it cannot be used to detect a substituted DLL, and consumers who require strong-named assemblies (for example legacy .NET Framework scenarios that demand them) are not supported. Authentication of a release relies on the NuGet package signature (AR-2), the SLSA provenance attestation and the published SHA-256 digests. Moving to strong-name signing (Policy B) would require a key-management decision and changes to the build, `release.yml`, [DEVELOPMENT.md §D2](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/DEVELOPMENT.md#d2--pi-01--strong-name-signing-not-applied-accepted-risk) and this entry. | Syncfusion Maintainers | 2026-10-30 |
| AR-2 | Shipped assemblies are not Authenticode signed (`NotSigned`); only the outer `.nupkg` carries a signature, applied by `release.yml` | **Formal waiver for per-DLL Authenticode signing.** The gating item is satisfied by (a) the outer `.nupkg` signature made with `dotnet nuget sign` (single primary signature, RFC 3161 timestamp) and verified with `dotnet nuget verify --all` before anything is attested, pushed or uploaded, (b) `RepositoryCommit` pointing at a public commit, (c) the SLSA provenance attestation over the signed package, and (d) SBOMs (AR-5). The package signature authenticates every byte inside the package. The signing certificate is held as the `NUGET_SIGNING_CERT_BASE64` / `NUGET_SIGNING_CERT_PASSWORD` repository secrets, written to a temporary file in the release job and destroyed immediately after signing; compromise of those secrets or of the release workflow would allow a validly signed malicious package, which is why changes to `release.yml` require review and who can publish a release is restricted. No counter-signature is applied. | Syncfusion Maintainers | 2026-10-30 |
| AR-3 | `RepositoryCommit` in the `.nuspec` is automatically derived from the public commit being packed (the tagged release commit in `release.yml`), instead of being maintained manually (D1 / LP-10) | `Directory.Build.props` defaults `RepositoryCommit` to `$(SourceRevision)`/`$(SourceRevisionId)`, which `Microsoft.SourceLink.GitHub` populates from the local `.git/HEAD`. `RepositoryBranch` is resolved from `.git/HEAD` at build time. `release.yml` checks out and packs the tagged release commit, which must be on `main` (verified by `git cat-file -e <SHA>^{commit}` per the DEVELOPMENT.md checklist). Manual override is possible with `-p:SourceRevision=<SHA>`. | Syncfusion Maintainers | 2026-09-06 |
| AR-4 | Signing and publication run in public CI (`.github/workflows/release.yml`) with repository secrets, and the authoritative registry is GitHub Packages (D8 / CI-07, D9 / CI-08) | Replaces the earlier position that signing and publishing were manual and outside the repository. `release.yml` is triggered only by a **published GitHub release** and is the only workflow with access to the signing secrets and to `packages: write`; `ci.yml` (pull requests) never signs or publishes. Packages are pushed to GitHub Packages (`https://nuget.pkg.github.com/syncfusion/index.json`) and the identical signed `.nupkg` is attached to the GitHub release. The workflow does not publish to nuget.org and there is no `NUGET_API_KEY`; any other channel must redistribute the signed `.nupkg` from the release unchanged. Residual risk: anyone able to publish a release or alter `release.yml` can cause a signed package to be produced, so those permissions are restricted and workflow changes are reviewed. The audit trail is the workflow run, the SLSA attestation and `SHA256SUMS` on the release. Consumers can request more detail through the security contact in [SECURITY.md](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/SECURITY.md). | Syncfusion Maintainers | 2026-10-30 |
| AR-5 | SBOM subject: CycloneDX describes the dependency graph; SPDX describes the final signed `.nupkg` files (D6 / PI-06, PI-07) | `release.yml` generates the SPDX document with `sbom-tool` **after** `dotnet nuget sign` and `dotnet nuget verify`, then re-reads it and fails the release if any recorded SHA-256 differs from the digest of the package to be published; `SHA256SUMS` lists every package and SBOM. The SPDX digest therefore identifies the exact `.nupkg` pushed to GitHub Packages and attached to the GitHub release (they are the same file). The CycloneDX SBOM is generated from the project (`dotnet-CycloneDX`) and is unaffected by signing. Earlier releases generated SPDX before signing, so its digest matched neither distributed package; those documents describe the unsigned payload only. A package that is re-packed or re-signed after release is not covered by the published SBOM. | Syncfusion Maintainers | 2026-10-30 |
| AR-6 | The project intentionally ships `dotnet new` scaffolders (templates) for its toolkit instead of contributing to the upstream `dotnet/scaffolding` repo | First-party scaffolders are the supported consumer side-channel for tooling, locale samples, and component recipes. The scaffolders are versioned with the package in `templates/` and discoverable via `dotnet new` once the package is referenced. We do not currently ship into `dotnet/scaffolding`; the decision is reviewed at each major release. | Syncfusion Maintainers | 2026-09-06 |

## Current security posture

The project currently emphasizes:

- open-source review and contributor oversight
- package metadata hygiene and repository linkage
- restricted use of browser interop and static assets
- clear reporting paths through the security policy

The toolkit is not designed to replace app-level security controls for authentication, authorization, CSP, or runtime isolation. Consumers remain responsible for securing the application hosting the components.

## Review and update cadence

This threat model should be reviewed when:

- a new component introduces browser interop or dynamic markup
- a dependency is added or significantly upgraded
- package packaging or release flow changes
- a security incident or vulnerability disclosure occurs

## Self-attestation

This threat model was prepared as a current security reference for the Syncfusion Blazor Toolkit project and reflects the maintainers’ understanding of the project as of 2026-10-30. The project team intends to review and update this document as changes to the component library, assets, or build pipeline occur.

The maintainers attest that the information provided here is a good-faith assessment of the project’s current security risks and mitigations based on the repository structure and package design at the time of publication.

### Change since last review

- **2026-10-30 — Signing / publication reconciliation.** Documentation now matches the shipped 2.0.1 behaviour and `release.yml`. Policy A adopted explicitly: assemblies keep `PublicKeyToken=null` and the docs no longer claim manual strong-name signing (AR-1). AR-2, AR-4 and AR-5 were rewritten because `release.yml` signs the `.nupkg` and publishes to GitHub Packages from public CI; GitHub Packages is the single authoritative registry. SPDX is now generated from the signed packages and cross-checked against their SHA-256 (PI-06 / PI-07). The release and CI vulnerability scans now fail on command, restore or advisory errors instead of masking them (SEC-06). The popups and calendars AI skill docs are checked against the packed public API (AI-03).
- **2026-10-19 — Post–2.0.0 / 2.0.1 review.** Major 2.0 shipped (namespace consolidation, CSS isolation, `SfThemeRoot` theme delivery). Accepted risks AR-1 through AR-6 were carried forward; the manual sign/publish wording in AR-1, AR-2, AR-4 and AR-5 was superseded by the 2026-10-30 entry above. Style delivery is no longer documented as a permanent `src/wwwroot/styles/STYLE-CONTRACT.md` sheet; see DEVELOPMENT.md D5 / CHANGELOG `[2.0.0]`.
- **2026-09-06 — Readiness defect pass (D1–D9), documentation-only.** Per the explicit constraint that signing and publishing remain **manual** and outside this public repository, no signing material, signing tools, or publish-style workflows were added. `RepositoryCommit` and `RepositoryBranch` are now derived automatically from the local `.git/HEAD` via `Directory.Build.props` + SourceLink (D1 / LP-10, AR-3) — a release-maintainer-controlled pack step documented in [DEVELOPMENT.md §Repository metadata + SBOM during `dotnet pack`](https://github.com/syncfusion/blazor-toolkit/blob/main/.github/DEVELOPMENT.md#repository-metadata--sbom-during-dotnet-pack). A new `pack` job was added to `.github/workflows/ci.yml` producing an unsigned `.nupkg` artifact for human review only (D7 / CI-01). Repository-root `Directory.Build.targets` runs `dotnet CycloneDX` on every `dotnet pack` and ships `*.cdx.json` inside the `.nupkg` (D6 / PI-06, AR-5). Style contract published at `src/wwwroot/styles/STYLE-CONTRACT.md` (D5 / BEQ-20). Reflection-based behaviour tests added under `tests/Syncfusion.Blazor.Toolkit.BUnitTest/Base/` for D4 / BEQ-10, D5 / BEQ-20 and D6 / PI-06 narrative cross-checks. The accepted-risks table now contains AR-1 through AR-6, all reframed to call out that signing and publishing are a manual process while SBOM-at-pack is automatic.
- **2026-08-21 — Hardened CD pipeline for nuget-publish.** Added SLSA build provenance attestation (`actions/attest-build-provenance`), deterministic builds via `ContinuousIntegrationBuild=true`, exit-code-driven vulnerability scan with downloadable `vuln-report` artifact, and concurrency guard for re-tagged same-version pushes. Accepted-risks entries AR-1 and AR-2 were reviewed and remain applicable; no new accepted risk was introduced.
