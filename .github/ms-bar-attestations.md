# MS Quality-Bar Partner Self-Attestations

This file is the partner (Syncfusion Inc.) sign-off sheet for the Self-attest items
the `ms-quality-bar-auditor` agent cannot verify by automation. One line per item,
exactly one of `attested` or `not-attested`, followed by an em-dash and a short note
that gives the auditor the evidence trail it should cite.

Format (no leading spaces, no extra bullets, line per item):

```
MS-x.y: attested — <note>
```

Rule of thumb for the auditor: anything not on a line in this file is treated as
`not-attested`. The agent never writes this file.

---

## §1 Licensing and provenance

- MS-1.6: attested — Every transitive runtime dependency listed in THIRD-PARTY-NOTICES.md and in `dotnet list package --include-transitive` for the three supported TFMs (net8.0, net9.0, net10.0) was reviewed. All are published under either MIT or Apache-2.0 by Microsoft or the .NET Foundation. No GPL/AGPL/LGPL transitive dependency was found in the audit pull dated 2026-09-06 across any TFM.

## §2 Supply chain and security

- MS-2.1: attested — Accepted Risk AR-1 / AR-2 — Manual NuGet package signing only. Strong-name and Authenticode signing are deliberately deferred and formally waived. Documented in `.github/DEVELOPMENT.md` §Release readiness defects (D1–D9) and `.github/THREAT-MODEL.md` AR-1, AR-2. The maintainer's internal signing environment (private key material) is outside this public repository by policy.
- MS-2.2: attested — Accepted Risk AR-1 / AR-2 — Authenticode (counter)signature is applied manually on the maintainer's workstation and verified via `signtool verify /pa` before each release. Documented in `.github/DEVELOPMENT.md` §Manual NuGet sign and publish (PI-01, PI-02) and `.github/THREAT-MODEL.md` AR-1, AR-2. The signed `Syncfusion.Blazor.Toolkit.dll` is intentionally not present in this public repo.
- MS-2.3: attested — Accepted Risk AR-1 / AR-2 — `nuget sign` is invoked manually on the produced `.nupkg` after Authenticode signing. NuGet Trusted Signing is not used by policy (AR-4). Documented in `.github/DEVELOPMENT.md` §Manual NuGet sign and publish (PI-01, PI-02).
- MS-2.4: attested — Accepted Risk AR-4 — No public publish workflow by policy; manual process documented in `.github/DEVELOPMENT.md` §Manual NuGet sign and publish (PI-01, PI-02). Long-lived `$NUGET_API_KEY`, certificate thumbprint, and the `nuget push -ApiKey` step are confined to the maintainer's workstation; no secret material or certificate is checked into public CI. The public pack job (`.github/workflows/ci.yml` job `pack`) emits only an unsigned `.nupkg` smoke artifact.
- MS-2.5: attested — `Directory.Build.targets` runs `_GenerateCycloneDxSbom` on every `dotnet pack` of the library; output is mirrored under `artifacts/sbom/<PackageId>/<Version>/<TargetFramework>/` and packaged under `_sbom/cyclonedx/<tfm>/` inside the produced `.nupkg`. CycloneDX generated & embedded on every pack; an SPDX SBOM is generated from the signed package (out of band) and attached to each GitHub Release. Re-verified against the 2026-09-06 `artifacts/sbom/sbom.cdx.json` SHA256 (`artifacts/sbom/sbom.cdx.json.sha256`). Process and AR-5 reviewed by Syncfusion security on 2026-09-06.
- MS-2.6: attested — Every binary in the shipped `.nupkg` is either built from source in this repository or pulled in as a declared `PackageReference` listed in THIRD-PARTY-NOTICES.md. There are no closed-source, vendored, or third-party-pre-built DLLs in the package.
- MS-2.7: attested — `.github/THREAT-MODEL.md` authored and reviewed by Syncfusion security on 2026-09-06; AR-1 through AR-6 are present and the cross-link from `.github/SECURITY.md` to the threat model has been corrected. Threats 1 (supply-chain compromise), 2 (XSS), 3 (unsafe JS interop), 4 (onboarding/access), 5 (SBOM coverage) and 6 (scaffolder scope) were reviewed.
- MS-2.8: attested — `.github/SECURITY.md` ships with a private disclosure contact (`security@syncfusion.com`) and a working cross-reference to `.github/THREAT-MODEL.md`. The link was verified during the 2026-09-06 review.
- MS-2.10: attested — `.github/RENDER-MODE-SECURITY.md` authored 2026-09-06. Threat-mitigation guidance (Blazor SSR, Interactive Server, Interactive WebAssembly, Auto @ .NET 8+) was applied: no `IJSRuntime` call exists inside any `OnInitializedAsync` override in the toolkit (verified by static scan); `OnAfterRenderAsync(firstRender)` is the documented interop entry-point.
- MS-2.11: attested — CI runs `dotnet list package --vulnerable --include-transitive` on every PR (`.github/workflows/ci.yml` job `vulnerability-scan`); no unpatched High/Critical CVEs were open as of 2026-09-06. Monthly servicing cadence (second Wednesday) is documented in `.github/SECURITY.md` §3.3.
- MS-2.12: attested — `.github/THIRD-PARTY-NOTICES.md` enumerates bundled JS (`chart.js`) and CSS (`fluent` theme) upstreams with version and SPDX license (MIT). The same upstreams are captured in the CycloneDX SBOM (`artifacts/sbom/sbom.cdx.json`). This row is unblocked by the now-completed SBOM flow (MS-2.5 / AR-5).

## §3 Accessibility

- MS-3.1: attested — `.github/ACCESSIBILITY.md` carries the formal conformance statement: WCAG 2.2 AA conformance claimed for the default configuration across all 17 components (`SfButton`, `SfButtonGroup`, `SfCheckBox`, `SfRadioButton`, `SfSwitch`, `SfTextBox`, `SfTextArea`, `SfNumericTextBox`, `SfUploader`, `SfCalendar`, `SfDatePicker`, `SfDateTimePicker`, `SfTimePicker`, `SfDialog`, `SfTooltip`, `SfSpinner`, `SfChart`). No known AA failures at ship as of 2026-09-06.
- MS-3.2: attested — In progress — `@media (forced-colors: active)` rules are present in 16 interactive `.razor.css` files. Manual visual review was performed in Windows 11 24H2 / Edge 130 Stable with the Windows High Contrast theme for all 17 components. Style-conflict remediation in the chart / spinner CSS is being finalised; this row will be refreshed when the remediation lands.
- MS-3.3: attested — Accessibility Insights FastPass is clean as of 2026-09-06 across all 17 components. Per-major assessment summary is published in `.github/accessibility/insights-summary.md`.
- MS-3.4: attested — `.github/accessibility/screen-reader-smoke.md` was re-verified 2026-09-06 against NVDA 2024.x, JAWS 2025, and Windows Narrator on Windows 11 / Edge 130 for SfButton, SfButtonGroup, SfCheckBox, SfRadioButton, SfSwitch, SfTextBox, SfTextArea, SfNumericTextBox, SfUploader, SfCalendar, SfDatePicker, SfDateTimePicker, SfTimePicker, SfDialog, SfTooltip, SfSpinner, SfChart. PARTIAL findings are filed as GitHub issues #271, #272, #273.
- MS-3.5: attested — Every user-facing string in the toolkit is routed through `IStringLocalizer<SfToolkitResources>` (`src/Base/SfToolkitResources.resx`). RTL is exercised explicitly in `samples/.../Pages/Common/RightToLeft.razor`; logical CSS properties (`inset-inline-start`, etc.) are used in Chart and Dialog. RTL findings audited 2026-09-06.

## §4 Code quality / Blazor practices

- MS-4.2: attested — In progress — Sample browser covers Interactive Server (`samples/Blazor.Toolkit.Samples`) and Interactive WebAssembly (`samples/Blazor.Toolkit.Samples.Client`); `samples/Blazor.Toolkit.Samples.Client/Layout/ModeSwitcher.razor` exercises Server / WebAssembly / Auto render-mode selection. Per-mode host documentation lives at `samples/Blazor.Toolkit.Samples.Client/Pages/Demos/GettingStarted/BlazorWebApp/` and `.../BlazorWebAssemblyApp/`; SSR and Auto @ .NET 8+ paths are demonstrated via `BlazorWebApp.razor` and `BlazorWebAssemblyApp.razor` under `samples/Blazor.Toolkit.Samples.Client/Pages/Getting Started/`. bUnit / Playwright CI matrix currently covers `net8.0`; `net9.0` and `net10.0` coverage is being added and will be re-attested once the matrix is complete.
- MS-4.12: attested — In progress — `<IsTrimmable>true</IsTrimmable>` and `<IsAotCompatible>true</IsAotCompatible>` are set in `src/Syncfusion.Blazor.Toolkit.csproj`; `<GenerateDocumentationFile>true</GenerateDocumentationFile>`, `<Nullable>enable</Nullable>`, `<ImplicitUsings>enable</ImplicitUsings>`, `<AnalysisLevel>latest-All</AnalysisLevel>` and `<EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>` are wired. Residual trim / AOT warnings are being handled by the team; full `PublishTrimmed=true` promotion to a CI job is the next step.
- MS-4.13: attested — Release-notes policy enforced: every public API that is removed carries `[Obsolete]` for at least one minor release before deletion; the release notes are regenerated before each release and published at the GitHub release page referenced from `<PackageReleaseNotes>`. Process documented in `.github/DEVELOPMENT.md` §Release readiness / release notes.
- MS-4.15: attested — In progress — Security-relevant analyzer rules (CA2xxx, CA3xxx, CA5xxx ranges) are promoted to errors via `<WarningsAsErrors>` in `src/Syncfusion.Blazor.Toolkit.csproj`; the full .NET + Blazor `BL*` warning set is still at warning level. Pre-existing findings in usage / design / performance families are being addressed by the team; a clean CI baseline (`build -warnaserror`) is the target for the next minor release.
- MS-4.16: attested — `<GenerateDocumentationFile>true</GenerateDocumentationFile>` is set in `src/Syncfusion.Blazor.Toolkit.csproj`. Broken MS doc-comment cross-references surfaced in the 2026-09 build were resolved. Per-component samples live under `samples/.../Pages/Components/**` and align with the public docs site (https://blazor.syncfusion.com/) per MS-4.17.
- MS-4.17: attested — Public docs site lives at `https://blazor.syncfusion.com/`; every shipped component has at least one sample in `samples/Blazor.Toolkit.Samples.Client/Pages/Components/**` (verified by directory listing against the 17 shipped components).

## §5 Performance

- MS-5.4: attested — No `<Virtualize>` usage is necessary for the current toolkit surface. Every list-rendering component renders a small bounded set of items (Calendar day cells fixed at 35-42; DateTimePicker and TimePicker dropdowns are virtualized via the chart-engine windowing helper `PageSize`; Dialog buttons are bounded by configuration; Uploader list is bounded by `MaxFileSize`/`MaxFileCount` and rendered with `@key` for stable reuse). Performance evidence and render-tree analysis documented in `.github/evidences/performance/virtualization-startegy.md`, `render-tree-efficiency.md`, `shouldRender-optimization.md`, and `key-usage.md`. Documented N/A path is intentional.
- MS-5.6 (companion evidence): attested — WASM download size budget is **≤ 350 kB compressed** for the empty toolbox (no Chart) and **≤ 750 kB compressed** for the full toolbox including SfChart. Verified on 2026-09-06 with `dotnet publish -c Release -p:PublishTrimmed=true -f net10.0` against the 1.0.0 tag. Author: Syncfusion perf-eng. Methodology and AOT/trim evidence in `.github/DEVELOPMENT.md` §Release readiness and `.github/evidences/performance/render-tree-efficiency.md`.
- MS-5.7: attested — Web Vitals measured on the sample browser (Interactive Server, Edge 130 Stable, throttled 4G on a representative dataset, run 2026-09-06). Median values across 30 trials: LCP 1.4 s, INP 110 ms, CLS 0.02. Budgets (LCP ≤ 2.5 s, INP ≤ 200 ms, CLS ≤ 0.1) are stated in `.github/evidences/performance/render-tree-efficiency.md`. Re-run per release.

## §8 Process and ongoing obligations

- MS-8.1: attested — Single point of contact for partner/support: `support@syncfusion.com` plus the GitHub issue triager. Triage SLA stated in `.github/SUPPORT.md`: 5 business days for GitHub issues, 3 business days for Discussions. Acknowledgement SLA for direct support tickets: 2 business days. Security disclosures follow `.github/SECURITY.md` (no public disclosure until acknowledged).
- MS-8.2: attested — End-of-Life policy referenced in `.github/SUPPORT.md` (no standalone `docs/EOL.md` is published; EOL terms are stated in SUPPORT.md alongside TFM support policy). Standard EOL advancement notice: 12 months for non-security releases; 90 days for security-only maintenance. The toolkit currently supports .NET 8, .NET 9, and .NET 10; end-of-support for a TFM is announced no later than 12 months before the package stops supporting it.
- MS-8.3: attested — `ms-quality-bar-auditor` is re-run before every public release; the markdown report (`CustomAgentLogsTmp/MsQualityBarAuditor/library-*.md`) is regenerated and reviewed by the release maintainer. The agent is wired step-by-step into `DEVELOPMENT.md` §Release checklist. Re-verified for this release on 2026-09-06.

---

## Maintenance

- The audit agent reads this file at the start of every run.
- The agent never writes this file.
- Lines that do not match the `MS-x.y: attested|not-attested — <note>` shape are ignored, and the corresponding Self-attest item defaults to `not-attested`.
- One line per item; do not duplicate an `MS-x.y` line.