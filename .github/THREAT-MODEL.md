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
- assembly strong-name signing (PI-01), Authenticode signing of inner DLLs (PI-02), and NuGet package signing + publishing are performed **manually** by the release maintainer. No signing key material, code-signing certificate, signing tooling, or publishing credential is ever present in this public repository. CI never imports, references, or attempts to use any of these. See Accepted Risks AR-1, AR-2, AR-4.

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
| AR-1 | Shipped assemblies are not strong-name signed (`PublicKeyToken=null`); automated strong-name signing is intentionally not implemented in the public repository | **Sole signing control.** The signed `.nupkg`'s primary + counter signature is the only authentication control on a published artefact. Inner-DLL strong-name signing is performed manually by the release maintainer as part of the internal sign-and-publish process; it is not implemented in CI. The public repository contains no strong-name key material, no `SignAssembly=true` directive, and no `AssemblyOriginatorKeyFile` value — by policy. The manual strong-name step is documented in [DEVELOPMENT.md §Manual NuGet sign and publish (PI-01, PI-02)](DEVELOPMENT.md#manual-nuget-sign-and-publish-pi-01-pi-02). | Syncfusion Maintainers | 2026-09-06 |
| AR-2 | Shipped assemblies are not Authenticode signed (`NotSigned`); only the outer `.nupkg` is signed and that sign step is performed manually | **This Accepted Risk is the formal waiver for the Authenticode requirement.** Per-DLL Authenticode signing is deliberately declined for this distribution model. The gating item is satisfied by the combination of (a) outer `.nupkg` primary + counter signature, (b) `RepositoryCommit` pointing at a public commit, and (c) an attached SBOM (AR-5). The outer `.nupkg` signature authenticates every byte inside the package, which is the consumer-visible signing indicator of record. CI never attempts to sign a `.nupkg`. Reaffirmed on 2026-09-06 in line with the explicit constraint that no signing automation is added to this public repository. | Syncfusion Maintainers | 2026-09-06 |
| AR-3 | `RepositoryCommit` in the `.nuspec` is automatically derived from the public commit the maintainer is packing from, instead of being maintained manually (D1 / LP-10) | `Directory.Build.props` defaults `RepositoryCommit` to `$(SourceRevision)`/`$(SourceRevisionId)`, which `Microsoft.SourceLink.GitHub` populates from the local `.git/HEAD`. `RepositoryBranch` is resolved from `.git/HEAD` at build time. The maintainer must run `dotnet pack` from a clone whose `HEAD` matches the on-`main` tag candidate (verified by `git cat-file -e <SHA>^{commit}` immediately before pack). Manual override is possible with `-p:SourceRevision=<SHA>`. | Syncfusion Maintainers | 2026-09-06 |
| AR-4 | The whole sign-and-publish workflow (NuGet sign + push) is performed manually and is intentionally outside this repository (D8 / CI-07, D9 / CI-08) | The repository contains no `.github/workflows/nuget-publish.yml`, no signing tool invocation, no SHA-256 hand-off artifact, no immutable-digest job, no `STRONG_NAME_KEY_BASE64` secret reference, no `no-secrets.yml` sentinel, and no `immutable-artifact.yml`. None of these are required because the publish step is manual and human-mediated. Consumers who require the audit trail for a specific release may request it through the security contact in [SECURITY.md](SECURITY.md). | Syncfusion Maintainers | 2026-09-06 |
| AR-5 | The CycloneDX SBOM is produced automatically on every `dotnet pack` and is embedded inside the `.nupkg`; the SPDX SBOM is generated from the **signed** `.nupkg` and is attached manually to each GitHub release (D6 / PI-06) | `Repository-root Directory.Build.targets` runs `dotnet CycloneDX` after `Build` and packages the resulting `.cdx.json` at `_sbom/cyclonedx/<tfm>/` inside every `.nupkg`. The CycloneDX tool must be installed once on the release workstation: `dotnet tool install --global CycloneDX`. The SPDX file is generated manually by the maintainer **from the signed `.nupkg`** (qualifying the bytes), uploaded as a release asset alongside the `.cdx.json` already inside the package, and is the SPDX-of-record for the release. | Syncfusion Maintainers | 2026-09-06 |
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

This threat model was prepared as a current security reference for the Syncfusion Blazor Toolkit project and reflects the maintainers’ understanding of the project as of 2026-09-06. The project team intends to review and update this document as changes to the component library, assets, or build pipeline occur.

The maintainers attest that the information provided here is a good-faith assessment of the project’s current security risks and mitigations based on the repository structure and package design at the time of publication.

### Change since last review

- **2026-09-06 — Readiness defect pass (D1–D9), documentation-only.** Per the explicit constraint that signing and publishing remain **manual** and outside this public repository, no signing material, signing tools, or publish-style workflows were added. `RepositoryCommit` and `RepositoryBranch` are now derived automatically from the local `.git/HEAD` via `Directory.Build.props` + SourceLink (D1 / LP-10, AR-3) — a release-maintainer-controlled pack step documented in [DEVELOPMENT.md §Repository metadata + SBOM during `dotnet pack`](DEVELOPMENT.md#repository-metadata--sbom-during-dotnet-pack). A new `pack` job was added to `.github/workflows/ci.yml` producing an unsigned `.nupkg` artifact for human review only (D7 / CI-01). Repository-root `Directory.Build.targets` runs `dotnet CycloneDX` on every `dotnet pack` and ships `*.cdx.json` inside the `.nupkg` (D6 / PI-06, AR-5). Style contract published at `src/wwwroot/styles/STYLE-CONTRACT.md` (D5 / BEQ-20). Reflection-based behaviour tests added under `tests/Syncfusion.Blazor.Toolkit.BUnitTest/Base/` for D4 / BEQ-10, D5 / BEQ-20 and D6 / PI-06 narrative cross-checks. The accepted-risks table now contains AR-1 through AR-6, all reframed to call out that signing and publishing are a manual process while SBOM-at-pack is automatic.
- **2026-08-21 — Hardened CD pipeline for nuget-publish.** Added SLSA build provenance attestation (`actions/attest-build-provenance`), deterministic builds via `ContinuousIntegrationBuild=true`, exit-code-driven vulnerability scan with downloadable `vuln-report` artifact, and concurrency guard for re-tagged same-version pushes. Accepted-risks entries AR-1 and AR-2 were reviewed and remain applicable; no new accepted risk was introduced.
