# Threat Model — repository evidence and open risks

## Status and scope

This is a source-based security reference for the component library, browser assets, samples, build configuration, and manual release boundary. It is **not an approved security assessment or a clean-scan attestation**. Source observations concern the current checkout, not verified contents of a published NuGet package. Downstream application security must be assessed by the host owner.

## Assets and trust boundaries

| Boundary | Assets and threat | Observed mechanism / remaining responsibility |
|---|---|---|
| Contributions, dependencies, and build tools → build output | Source integrity, package identity, executable JavaScript, generated styles; malicious changes or compromised dependencies | Package references, analyzer settings, and workflows are inspectable. Branch protection, actual review records, reproducible builds, and successful runs need separate evidence. Build scripts execute tools; source visibility is not provenance. |
| Build output → manually published release | Package bytes, assembly identity, repository metadata, SBOM; substitution or incorrect release attribution | CI configures unsigned review packages. Signing/publishing remain manual and outside public CI. Verify the exact published bytes rather than treating configuration or comments as completed signing steps. |
| Application data/templates/attributes → rendered DOM and interop | Browser execution and user data; injection through reachable markup sinks | Razor string rendering provides encoding in ordinary text positions; templates, attributes, and JavaScript paths require context-specific review. A typed API alone is not sanitization. |
| Browser → server circuit, API, data or upload endpoint | Authorization, confidential data, server resources; forged inputs, unsafe destinations, excessive requests/files | Host authentication, authorization, validation, rate/resource limits, and upload handling are required. Client checks and interop callbacks are not trust boundaries. |
| Server/prerender → WebAssembly/Auto client | Secrets, service lifetimes, serialized state; disclosure or inappropriate server dependencies | Scoped toolkit services and JS lifecycle selection exist, not a service-graph security enforcer. See [render-mode security](RENDER-MODE-SECURITY.md). |

## Threat review notes

- **XSS and dynamic markup:** `src/wwwroot/scripts/chart.js`, `svgbase.js`, and `uploader.js` contain `innerHTML` assignments; `sanitize-html-helper.js` also uses an HTML parsing sink. These are review locations, **not automatically proven exploits**. Trace attacker control, transformations/sanitization, sink context, reachability, and browser behavior before classifying a vulnerability. Conversely, absence of heuristic findings would not prove safety.
- **HTTP and error disclosure:** `src/Data/HttpHandler.cs` builds requests from supplied URLs and catches failures into a response containing exception and response text. Host code must constrain destinations where untrusted input can influence them, enforce endpoint authorization, and avoid exposing diagnostic content. No SSRF or disclosure exploit is asserted without a reachable application scenario.
- **Browser interop:** `src/Base/SfBaseComponent.cs` imports `base.js` at first interactive after-render and uses async/in-process references according to runtime. Disposal and disconnection handling are lifecycle controls, not a sandbox or content validation.
- **Static assets and dependencies:** review generated CSS, product JS, dependencies, and the actual package layout as deliverables. No deterministic-output or comprehensive dependency-inventory result is established here.

## Configured checks, not results

| Evidence | Configuration | Limit of the evidence |
|---|---|---|
| [`src/Syncfusion.Blazor.Toolkit.csproj`](../src/Syncfusion.Blazor.Toolkit.csproj) | NuGet audit enabled with moderate threshold and all dependency mode; selected security analyzer diagnostics treated as errors | Does not establish a vulnerability-free dependency set or release. |
| [`workflows/ci.yml`](workflows/ci.yml) | bUnit, Playwright, NuGet vulnerability listing with a text-based finding check, ESLint security, heuristic XSS scan, then unsigned pack | Configuration only; retained successful reports tied to a commit/package are needed. The vulnerability job scans the library project, not every project. |
| [`eslint.config.mjs`](../eslint.config.mjs) | Security rules for remaining JS/TS files | Explicitly excludes `src/wwwroot/scripts/**`, styles, minified JS, samples, tests, and gulp tooling. It is not product-browser-JS scan coverage. |
| [`gulpfile.js`](../gulpfile.js), `security-xss-scan` | Pattern scan of `src/Components`, `src/Base`, and `src/Data` | Excludes `wwwroot`; no product `src/wwwroot/scripts` coverage. Heuristic matches need triage and cannot establish exploitability. |
| [`workflows/codeql.yml`](workflows/codeql.yml) | C#, JavaScript/TypeScript, and Actions analysis on push/PR, weekly schedule, and manual dispatch | JavaScript analysis is configured despite ESLint/XSS exclusions. Actual analyzed-file coverage, findings, and successful runs are not verified here. |

No current workflow reviewed here signs or publishes packages or emits the previously claimed SLSA release attestation. Workflow comments about manual signing are not evidence that signing happened.

### Retained local observations — not security clearance

[R00 on 2026-10-06, commit `1922761`](../docs/evidence/2026-10-06-r00-1922761/index.md) retained **11 high-severity npm affected-package entries**, a top-level SBOM checksum mismatch, and source-level dispositions for F01–F17. Existing ESLint/XSS checks passed with the exclusions above; the library-only NuGet query reported no advisories from its current feed. These results do not prove safe shipped scripts, establish exploitability of every advisory, or close the independent R19 review. Dependency triage belongs to R12 and artifact reconciliation to R17; no exception is accepted here.

## Accepted risks

**Legacy IDs retained; approvals unverified.** The heading and IDs preserve cross-references only. **Every entry requires disposition; no approval, waiver, owner acceptance, or acceptance date is verified.** Prior maintainer names and dates are not retained as sign-offs.

| ID | Risk / evidence status | Disposition evidence needed |
|---|---|---|
| AR-1 | Assembly strong-name status of published releases is unknown. No strong-name signing is configured in the reviewed public build/pack flow. Strong names are assembly identity, not publisher trust certification. | Inspect exact released DLLs and record identity/signature results and any authorized policy decision. Do not infer signed or unsigned release DLLs from CI's unsigned policy. |
| AR-2 | Authenticode and outer NuGet signature status are unknown. The former text simultaneously claimed manual inner-DLL signing and a waiver declining it; neither is substantiated. | Verify the actual DLL/package signatures, certificate chain and timestamps where applicable. A package signature does not prove per-DLL Authenticode signing; no formal waiver is recorded here. |
| AR-3 | Repository metadata defaults exist in `Directory.Build.props` and SourceLink is referenced by the project. The CI pack command explicitly supplies `RepositoryCommit`. These settings do not prove that a package matches public source, especially with a dirty checkout or overrides. | Compare the emitted `.nuspec` and SourceLink metadata with the public tag/commit and actual build inputs. Record the release package digest. |
| AR-4 | Signing and publishing remain **manual**, outside public CI. This preserves the release policy, not evidence of completion or an exemption from provenance needs. | Obtain a release-specific build/sign/publish record and package verification evidence. Request available evidence through [security@syncfusion.com](mailto:security@syncfusion.com); no response SLA is implied. |
| AR-5 | `Directory.Build.targets` configures project-derived CycloneDX generation under conditions, including `CI != true`, with a hardcoded global-tool executable path. Pack item inclusion is configured but actual `.nupkg` contents are unverified. No shipped SPDX SBOM or per-release SBOM attachment is established. | Inspect generated dependency coverage and exact release package contents; reconcile snapshot checksums and tie any SBOM to package bytes. See [SBOM evidence notes](../artifacts/sbom/README.md). |
| AR-6 | The former claim that `templates/` ships `dotnet new` scaffolders is unsupported: that directory is absent from the reviewed checkout. Referencing a library package alone does not install templates. | Verify an actual template package and installation instructions before claiming availability, or retire the claim through an explicit disposition. |

## Release evidence boundary

The supplied GitHub release metadata lists [v1.0.2 (2026-09-01)](https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.2) and [v1.0.1 (2026-06-26)](https://github.com/syncfusion/blazor-toolkit/releases/tag/v1.0.1), each with an empty uploaded `assets` array. This does not inspect NuGet, signatures, or embedded package contents. GitHub-generated source archives are not evidence of attached release SBOMs. The local project version `1.0.0` and the committed SBOM snapshot must not be represented as a verified assessment of those releases.

`SbomContractTests.ThreatModel_DescribesSbomPolicy` checks only that this document contains **SBOM** and **SPDX**. Passing that text contract would not establish generation, attachment, package inclusion, signature validity, or inventory completeness.

## Maintenance and superseded claims

Revisit this reference when interop, content handling, dependencies, packaging, hosting, or a disclosure changes the trust boundaries. This is guidance, not evidence of a fixed review cadence.

Earlier self-attestations, dated risk acceptances, the Authenticode waiver, the historical SLSA/publish-pipeline narrative, and claims of SBOM generation on every pack or SPDX attachment to every release are **superseded and unverified**. This update records no historical review session, approval, certification, or completed release validation. Private vulnerability reporting remains [SECURITY.md](SECURITY.md).
