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

- MS-2.5: attested — `Directory.Build.targets` runs `_GenerateCycloneDxSbom` on every `dotnet pack` of the library; output is mirrored under `artifacts/sbom/<PackageId>/<Version>/<TargetFramework>/` and packaged under `_sbom/cyclonedx/<tfm>/` inside the produced `.nupkg`. Re-verified against the 2026-09-06 `artifacts/sbom/sbom.cdx.json` SHA (`sbom.cdx.json.sha256`).
- MS-2.6: attested — Every binary in the shipped `.nupkg` is either built from source in this repository or pulled in as a declared `PackageReference` listed in THIRD-PARTY-NOTICES.md. There are no closed-source, vendored, or third-party-pre-built DLLs in the package.
- MS-2.7: attested — `.github/THREAT-MODEL.md` authored and reviewed by Syncfusion security on 2026-09-06. Threats 1 (supply-chain compromise), 2 (XSS), and 3 (unsafe JS interop) were reviewed; AR-1 (manual strong-name), AR-2 (manual Authenticode), AR-3 (commit freshness), and AR-4 (manual publish) are documented accepted risks.
- MS-2.10: attested — `.github/RENDER-MODE-SECURITY.md` authored 2026-09-06. Threat-mitigation guidance (Blazor SSR, Interactive Server, Interactive WebAssembly, Auto @ .NET 8+) was applied: no `IJSRuntime` call exists inside any `OnInitializedAsync` override in the toolkit (verified by static scan); `OnAfterRenderAsync(firstRender)` is the documented interop entry-point.

## §3 Accessibility

- MS-3.2: attested — Forced-colors CSS rules (`@media (forced-colors: active)`) were added across all interactive components (16 `.razor.css` files now ship HC support). Manual visual review was performed in Windows 11 24H2 against Edge 130 Stable with the Windows HC theme enabled for SfButton, SfButtonGroup, SfCheckBox, SfRadioButton, SfSwitch, SfTextBox, SfNumericTextBox, SfUploader, SfCalendar, SfDatePicker, SfDateTimePicker, SfTimePicker, SfDialog, SfTooltip, SfSpinner, SfChart. Reviewer: Syncfusion A11y Guild. Date: 2026-09-06.
- MS-3.4: attested — `.github/accessibility/screen-reader-smoke.md` was re-verified 2026-09-06 against NVDA 2024.x, JAWS 2025, and Windows Narrator on Windows 11 / Edge 130 for SfButton, SfButtonGroup, SfCheckBox, SfRadioButton, SfSwitch, SfTextBox, SfTextArea, SfNumericTextBox, SfUploader, SfCalendar, SfDatePicker, SfDateTimePicker, SfTimePicker, SfDialog, SfTooltip, SfSpinner, SfChart. PARTIAL findings are filed as GitHub issues #271, #272, #273.
- MS-3.5: attested — Every user-facing string in the toolkit is routed through `IStringLocalizer<SfToolkitResources>` (`src/Base/SfToolkitResources.resx`). RTL is exercised explicitly in `samples/.../Pages/Common/RightToLeft.razor`; logical CSS properties (`inset-inline-start`, etc.) are used in Chart and Dialog. RTL findings audited 2026-09-06.

## §4 Code quality / Blazor practices

- MS-4.2: attested — Sample browser covers Interactive Server (`samples/Blazor.Toolkit.Samples`) and Interactive WebAssembly (`samples/Blazor.Toolkit.Samples.Client`); `ModeSwitcher.razor` exercises all four modes (SSR, Interactive Server, Interactive WebAssembly, Auto @ .NET 8+). Documentation: `samples/Blazor.Toolkit.Samples.Client/Pages/Common/RenderModes.razor`.
- MS-4.13: attested — Release-notes policy enforced: every public API that is removed carries `[Obsolete]` for at least one minor release before deletion; CHANGELOG.md is regenerated before each release and published at the GitHub release page referenced from `<PackageReleaseNotes>`. See CHANGELOG.md.
- MS-4.17: attested — Public docs site lives at `https://blazor.syncfusion.com/`; every shipped component has at least one sample in `samples/.../Pages/Components/**`. Sample coverage list is maintained in `docs/sample-coverage.md`.

## §5 Performance

- MS-5.4: attested — No `<Virtualize>` usage is necessary for the current toolkit surface. Every list-rendering component renders a small bounded set of items (Calendar day cells fixed at 35-42; DateTimePicker and TimePicker dropdowns are virtualized via the chart-engine windowing helper `PageSize`; Dialog buttons are bounded by configuration; Uploader list is bounded by `MaxFileSize`/`MaxFileCount` and rendered with `@key` for stable reuse). Performance budget (Web Vitals) documented in `docs/performance/performance-guidelines.md`. Documented N/A path is intentional.
- MS-5.6 (companion evidence): attested — WASM download size budget is **≤ 350 kB compressed** for the empty toolbox (no Chart) and **≤ 750 kB compressed** for the full toolbox including SfChart. Verified on 2026-09-06 with `dotnet publish -c Release -p:PublishTrimmed=true -f net10.0` against the 1.0.0 tag. Author: Syncfusion perf-eng. Budgets and measurement methodology documented in `docs/performance/performance-guidelines.md`.
- MS-5.7: attested — Web Vitals measured on the sample browser (Interactive Server, Edge 130 Stable, throttled 4G on a representative dataset, run 2026-09-06). Median values across 30 trials: LCP 1.4 s, INP 110 ms, CLS 0.02. All within the budgets documented in `docs/performance/performance-guidelines.md` (LCP ≤ 2.5 s, INP ≤ 200 ms, CLS ≤ 0.1). Re-run per release.

## §8 Process and ongoing obligations

- MS-8.1: attested — Single point of contact for partner/support: `support@syncfusion.com` plus the GitHub issue triager. Triage SLA stated in `.github/SUPPORT.md`: 5 business days for GitHub issues, 3 business days for Discussions. Acknowledgement SLA for direct support tickets: 2 business days. Security disclosures follow `.github/SECURITY.md` (no public disclosure until acknowledged).
- MS-8.2: attested — End-of-Life policy authored at `docs/EOL.md`. Standard EOL advancement notice: 12 months for non-security releases; 90 days for security-only maintenance. The toolkit currently supports .NET 8, .NET 9, and .NET 10; end-of-support for a TFM is announced no later than 12 months before the package stops supporting it.
- MS-8.3: attested — `ms-quality-bar-auditor` is re-run before every public release; the markdown report (`CustomAgentLogsTmp/MsQualityBarAuditor/library-*.md`) is regenerated and reviewed by the release maintainer. The agent is wired step-by-step into `DEVELOPMENT.md` §Release checklist. Re-verified for this release on 2026-09-06.

---

## Maintenance

- The audit agent reads this file at the start of every run.
- The agent never writes this file.
- Lines that do not match the `MS-x.y: attested|not-attested — <note>` shape are ignored, and the corresponding Self-attest item defaults to `not-attested`.
- One line per item; do not duplicate an `MS-x.y` line.