# SBOM artifacts — unverified snapshots

This directory contains source-project dependency snapshots, **not verified release SBOMs or a partner attestation**. Presence in the repository does not prove generation at tag time, inclusion in a NuGet package, inventory completeness, or attachment to every release.

## Files and scope

| File / directory | Observed role | Evidence limitation |
|---|---|---|
| `sbom.cdx.json` | Top-level CycloneDX-formatted snapshot | Generation inputs, tool history, schema validity and release binding remain unverified. |
| `sbom.cdx.json.sha256` | Claimed checksum for that snapshot | **Does not match the file**, as reproduced by R00. |
| `Syncfusion.Blazor.Toolkit/1.0.0/<tfm>/` | Checked-in per-target mirrors and format/version sidecars | Existing mirrors may be stale; sidecars are not proof of the executable version or schema validation. |
| `README.md` | Evidence guidance | No package or compliance gate is satisfied by this text. |

## Current generation configuration

Sources: [Directory.Build.props](../../Directory.Build.props), [Directory.Build.targets](../../Directory.Build.targets), and the [library project](../../src/Syncfusion.Blazor.Toolkit.csproj).

- Generation runs after `Build` / before `GenerateNuspec` only when enabled, `CI != true`, the project is packable and not a test project, and `TargetFramework` is nonempty. It is not an unconditional every-pack or every-commit operation.
- The command invokes the global `dotnet-CycloneDX` executable using a source `.csproj`, `--framework` and `-dpr`. The repository does not pin that global executable. The earlier `Microsoft.Sbom.Targets` provenance assertion is not established by this configuration.
- Pack integration attempts to include an existing mirror under `_sbom/cyclonedx/<tfm>/`. Correct multi-target timing, freshness and actual package inclusion require artifact tests. A stale file can exist when generation is skipped.
- This is not an inventory extracted from final signed package bytes. It does not establish SPDX generation, bundled-script/license completeness, signing or provenance. See [development guidance](../../.github/DEVELOPMENT.md#repository-metadata-and-sbom).

## Reproduced checksum mismatch

[R00, 2026-10-06, commit `1922761`](../../docs/evidence/2026-10-06-r00-1922761/index.md) compared `shasum -a 256 artifacts/sbom/sbom.cdx.json` with its committed checksum:

- Actual: `c5062333134c5ac3836f3ab3abbc18fe9244e17960b6a2f08a6dbf6b8f7b2579`.
- Expected (case-normalized): `ce5ac7c9194195560ea5e88e60ef17b342bb221fb40f7dba11a8cbd9afaf880f`.

**Do not use this snapshot as release evidence.** R01 changes this documentation only; the JSON and checksum are preserved. R17 must establish authoritative inputs, regenerate and validate schema/inventory/freshness before producing a new checksum. Replacing the hash alone would not establish authenticity or close R17.

`dotnet list package`, npm audit and CodeQL do not become consumers of this snapshot merely because it exists here. NuGet signature verification does not automatically validate this separate SBOM checksum.

## Release and license evidence

The public GitHub response retained by R00 lists v1.0.1 and v1.0.2 with empty uploaded asset lists. That does not determine embedded NuGet contents, signatures or private evidence. Source `Version=1.0.0` is unresolved against release history; no release identity is selected by this document.

Use [.github/THIRD-PARTY-NOTICES.md](../../.github/THIRD-PARTY-NOTICES.md) for dependency/license scope and [R17 in the readiness plan](../../docs/ENTERPRISE-READINESS-PLAN.md) for closure requirements. Prior claims of verified checksum equality, a v1.0.0 release-preview attestation and SBOM shipping with every release are withdrawn as unsupported. No signing, publication or approved risk acceptance occurred in this documentation correction.

_Last reviewed: 2026-10-06, R01 documentation-only reconciliation._