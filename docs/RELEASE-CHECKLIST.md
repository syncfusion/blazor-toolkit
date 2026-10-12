# Release checklist — Syncfusion Toolkit for Blazor

**Created:** 2026-10-03. **Reusable template, not a completed release record.** Copy it into the candidate's evidence index or an approved release ticket for every release. All boxes below are deliberately unchecked.

[Readiness plan](ENTERPRISE-READINESS-PLAN.md#12-remaining-issue-closure-plan) · [Evidence procedures and formats](MANUAL-EXTERNAL-EVIDENCE-TRACKER.md) · [Manual publishing policy](../.github/DEVELOPMENT.md#manual-release-preparation) · [Changelog](../CHANGELOG.md)

**First priority: update the approved version in source before creating the release candidate.** At this checklist's creation, source declares `1.0.0`, while the reviewed GitHub release history ends at `v1.0.2`. This is a discrepancy to resolve, not permission to republish either version or automatically choose `1.0.3`.

Signing, tagging and publication remain authorized **manual** operations. This document supplies preparation/verification steps, not permission to execute them. No private keys, publishing credentials or secret values belong in the repository or evidence bundle.

## 1. Release record — fill before starting

```markdown
- Proposed version (without v): UNAPPROVED
- Proposed tag: v<approved-version>
- Previous verified release/tag/package: TBD
- Version rationale and API review (E02): NOT APPROVED
- Candidate SHA (after version/docs/assets changes are committed): TBD
- Upstream repository/branch and verified ancestry: TBD
- Release owner / backup / independent reviewer: UNASSIGNED
- Planned publication date / actual UTC publication date: TBD / NOT PUBLISHED
- Candidate record and evidence-store location: TBD
- Required E01–E12 cases, supported scope and approved exceptions: TBD
- Unsigned package path + SHA-256: NOT BUILT
- Final signed package path + SHA-256: NOT PRODUCED
- Downloaded published package path + SHA-256: NOT PUBLISHED
- State: PREPARATION
- Go/no-go approval: NOT REQUESTED
- Recovery/monitoring owner and contact: UNASSIGNED
```

A local date, tag name, build number or prior chat approval is not a release approval. Name the exact candidate SHA and artifact hash in the go/no-go record.

## 2. Approve scope, version and compatibility

- [ ] Recheck [GitHub releases](https://github.com/syncfusion/blazor-toolkit/releases), tags and the [NuGet package](https://www.nuget.org/packages/Syncfusion.Blazor.Toolkit); record versions, URLs, retrieval date and candidate availability. A GitHub release record and a NuGet publication are distinct.
- [ ] Complete E02's EXPLORATORY API comparison and migration review against the verified released baseline using explicitly exploratory outputs from the selected source. Include `DataAdaptor<T> : notnull`, nullable metadata, exception changes, Razor parameters, serialization/JS contracts and behavior fixes. This informs version choice; repeat against the approved-version package in §8 before release approval.
- [ ] Release owner approves the next SemVer. Do not infer patch/minor/major from previous numbering; diagnostics promoted to errors in consumer builds can matter for source compatibility.
- [ ] Confirm the new version/tag is unused. If already published, stop and select a new approved version; never overwrite package bytes or move an old release tag.
- [ ] Agree supported targets/modes/browsers and which E-records are required. New or changed behavior needs corresponding evidence, not an unexplained N/A entry.
- [ ] Resolve release blockers or obtain explicit authorized dispositions under the evidence policy. Unknown source/artifact identity, missing mandatory evidence and unapproved versions hold release.

## 3. Update register — prevent missed version and document edits

This table is the required pre-candidate review. Mark each row changed or reviewed-not-applicable in the release record; keep historical records accurate.

| File / artifact | What to update or verify | Do not do |
|---|---|---|
| [`src/Syncfusion.Blazor.Toolkit.csproj`](../src/Syncfusion.Blazor.Toolkit.csproj) | Set `<Version>` to the approved product version. Confirm evaluated `PackageVersion`, package ID, TFMs and metadata. | Do not rely solely on `-p:Version`/`PackageVersion` or a workflow override while source remains stale. |
| [`Directory.Build.props`](../Directory.Build.props) | Verify repository URL/commit/branch derivation against the chosen checkout. Record SDK-inferred assembly/file/informational versions. | Do not pin a guessed SHA or treat `ContinuousIntegrationBuild=true` as proof of reproducibility. |
| Generated `src/obj/<configuration>/<tfm>/*AssemblyInfo.cs` and output DLL/PDB/XML | Regenerate from the candidate; inspect actual assembly/package metadata. | Do not hand-edit generated assembly versions to mask a stale project version. Assembly version need not equal full SemVer prerelease text. |
| [`CHANGELOG.md`](../CHANGELOG.md) | Move only included Unreleased entries into the approved version section; review breaking changes/migration notes; keep a fresh Unreleased section and correct compare links. | Do not backdate local fixes into v1.0.1/v1.0.2 or invent a publication date. Until publication, identify planned dates as planned. |
| [`README.md`](../README.md), [roadmap](../ROADMAP.md), [documentation index](../.github/Index.md) | Update current-release statements, setup/package examples, tested scope and resolved discrepancy text where evidence supports it. Dynamic badges usually need no version edit. | Do not replace source-based descriptions with unverified release guarantees or promise Grid/new components. |
| [GettingStarted sample](../samples/Blazor.Toolkit.Samples.Client/Pages/Getting%20Started/GettingStarted.razor) and `.github/skills/**` | Search for outdated version displays (including the existing `v1.0.1` success text), packages, methods, render-mode assumptions and removed APIs. Update current instructions; retain clearly historical examples. | Do not globally replace every `1.0.0` or alter code examples without checking the target package API. |
| [Development guide](../.github/DEVELOPMENT.md), [test matrix](../.github/TEST-MATRIX.md) | Add exact candidate checks, runtime/browser identity and limitations. Preserve earlier dated results as history. | Do not carry forward 2,735 tests/zero warnings as a newly executed candidate result. |
| [Security policy](../.github/SECURITY.md), [threat model](../.github/THREAT-MODEL.md), [render-mode guidance](../.github/RENDER-MODE-SECURITY.md) | Reconcile fixed/open findings, scan scope, release-specific verification and approved disclosures. | Do not publish private reproductions or advance an attestation/approval date without a real review. |
| [Accessibility register](VPAT-2.5-INT.md), [accessibility guidance](../.github/ACCESSIBILITY.md) and AT records | Update evaluated version/scenarios only with E09 reports and actual evaluator/reviewer signoff. | Do not convert Not Evaluated to Supports based on a build or automated scan alone. |
| [Third-party notices](../.github/THIRD-PARTY-NOTICES.md), [license](../LICENSE.txt), package notices | Review exact shipped dependencies/assets and required notice inclusion; obtain owner/legal review where needed. | Do not treat devDependency or PrivateAssets labels as proof of no redistribution obligations. |
| `artifacts/sbom/<PackageId>/<Version>/<tfm>/` and final embedded/external SBOMs | Regenerate with approved tool and inputs, validate schema/inventory/identity, compute fresh hashes of exact bytes. | Do not edit snapshot versions or only replace the mismatching digest. Preserve/supersede historical snapshots explicitly. |
| [`package.json`](../package.json), lockfile, SDK/workload/tool pins | Review dependency changes independently and record resolutions. Current package.json has no Toolkit product-version authority. | Do not bump dependency, schema, skill, action, SDK or license versions just because the Toolkit version changed. |
| Sample/test `.csproj` references | Current main hosts/tests use Toolkit `ProjectReference`; inspect any new package consumers separately and pin their candidate version. | Do not confuse the nested ButtonGroup commercial `Syncfusion.Blazor Version="*"` project with the Toolkit regression entry point. |
| GitHub release draft/tag, NuGet listing and evidence index | Agree version, date, SHA, migration notes, artifact identity, attachments and public/private boundaries. | Do not publish before final review or expose secrets/private security reports. |

- [ ] Every row above has an owner and recorded disposition.
- [ ] Search current docs/samples for the previous version and the stale source version; classify each match rather than applying a global replacement.
- [ ] Regenerate generated themes/assets in the authorized development branch. Review any tracked differences and complete the version/docs/assets commit before selecting the candidate SHA.
- [ ] Obtain code/API/release review; record the approved candidate SHA **after** the version update. Changes after approval require a new candidate and affected revalidation.

## 4. Clean candidate and hard-stop version check

Use a fresh checkout of the approved commit rather than deleting files from a developer's dirty tree. Commands below are for a later authorized operator, run from that checkout's repository root. They were **not executed when this checklist was created**.

Set `APPROVED_VERSION`, `APPROVED_SHA`, and `APPROVED_UPSTREAM_MAIN` from the release record. The upstream reference must already be freshly fetched and verified; the remote need not be named `origin`. These variables are identifiers, not secrets.

```sh
set -eu
set -o pipefail
: "${APPROVED_VERSION:?Set the owner-approved product version without v}"
: "${APPROVED_SHA:?Set the approved candidate SHA after the version commit}"
: "${APPROVED_UPSTREAM_MAIN:?Set the verified remote/main reference}"
LIB=src/Syncfusion.Blazor.Toolkit.csproj
BUNIT=tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj

test -z "$(git status --porcelain --untracked-files=normal)"
test "$(git rev-parse HEAD)" = "$APPROVED_SHA"
git merge-base --is-ancestor "$APPROVED_SHA" "$APPROVED_UPSTREAM_MAIN"
test "$(dotnet msbuild "$LIB" -nologo -getProperty:Version)" = "$APPROVED_VERSION"
test "$(dotnet msbuild "$LIB" -nologo -getProperty:PackageVersion)" = "$APPROVED_VERSION"
```

Also inspect the literal source `<Version>` and property imports for environment overrides. The evaluated checks do not excuse a stale source value. They are **operator checks, not installed CI gates**; R03/R16 should automate the approved validation later without adding public signing/publishing.

- [ ] Literal source and evaluated version match the approved value; no version override hides a mismatch.
- [ ] Candidate SHA is clean and belongs to the approved upstream history. A locally existing object alone does not prove ancestry.
- [ ] No local generated/untracked file masks a missing release input. Regenerated tracked assets either reproduce or create a new reviewed candidate.
- [ ] Record `dotnet --info`, SDK/runtime/workload lists, Node/npm versions and global tools in E01. Use SDK 10, .NET 8 runtime for current bUnit, Node 22.13+ within 22.x, and required gallery/WASM workloads.
- [ ] Pin/approve CycloneDX and other evidence tools for this candidate; do not update tools mid-run without recording/revalidating.
- [ ] Allocate evidence output **outside the checkout**, retain logs and establish durable storage. Current CI retention is only 14 days.

## 5. Generate, build, discover and test

After prerequisites, commands for the current source layout are:

```sh
EVIDENCE_DIR="$(mktemp -d "${TMPDIR:-/tmp}/toolkit-release.XXXXXX")"
printf 'Evidence directory: %s\n' "$EVIDENCE_DIR"
npm ci
npx gulp blazor-toolkit-themes
dotnet restore "$LIB"
dotnet build "$LIB" -c Release --no-restore --no-incremental \
  2>&1 | tee "$EVIDENCE_DIR/library-build.log"
dotnet test "$BUNIT" -c Release -f net8.0 --list-tests \
  2>&1 | tee "$EVIDENCE_DIR/bunit-discovery.log"
TZ=UTC LANG=en_US.UTF-8 dotnet test "$BUNIT" -c Release -f net8.0 \
  --logger "trx;LogFileName=bunit-net8.trx" \
  --results-directory "$EVIDENCE_DIR/TestResults" --collect:"XPlat Code Coverage"
dotnet build samples/Blazor.Toolkit.Samples.slnx -c Release
dotnet format style "$LIB" --verify-no-changes --no-restore --severity warn
```

`npm ci` needs the matching tracked lockfile; if it fails, fix dependency reproducibility in the reviewed branch instead of silently using an unrecorded install. Preserve nonzero command exit codes; do not ignore them to keep collecting a release “pass.”

- [ ] Build each promised TFM, inspect full diagnostics, and retain actual results. Never suppress new warnings merely to meet a count.
- [ ] Discover nonzero intended tests, then record actual pass/fail/skip/retry counts and coverage. No `--no-build` against stale test binaries.
- [ ] Review failures and flaky tests; fix root causes or hold with an authorized disposition. Do not weaken assertions or disable parallelism to hide defects.
- [ ] Run actual approved browser/render-mode combinations under E04, not only project-reference unit tests.
- [ ] Run supported published trimming/WASM-AOT cases under E05; no flag or declaration substitutes for published-app execution.
- [ ] Capture source status after generation/build; unexpected tracked changes invalidate the frozen candidate.

### Existing Chromium check — limited scope

```sh
npx playwright install chromium
# The current config reuses an existing server on port 5000. Inspect it first.
command -v lsof >/dev/null 2>&1 || { printf '%s\n' 'Stop: port inspection unavailable.'; exit 1; }
if lsof -nP -iTCP:5000 -sTCP:LISTEN > "$EVIDENCE_DIR/port5000.txt" 2> "$EVIDENCE_DIR/port5000-errors.txt"; then
  cat "$EVIDENCE_DIR/port5000.txt"
  printf '%s\n' 'Stop: verify/free port 5000 before testing this candidate.'
  exit 1
else
  probe_exit=$?
  # lsof uses 1 for no matching entries; errors/output must still be reviewed.
  if test "$probe_exit" -ne 1 || test -s "$EVIDENCE_DIR/port5000-errors.txt" || test -s "$EVIDENCE_DIR/port5000.txt"; then
    printf '%s\n' 'Stop: port inspection was inconclusive; inspect the saved output.'
    exit 1
  fi
fi
npx playwright test --project=chromium --forbid-only --trace retain-on-failure
```

If `lsof` or required browser dependencies are unavailable, stop and use an approved platform equivalent; do not treat that as a free port. Do not automatically kill an unrelated service. Archive `playwright-report/` and `test-results/` with the tested SHA. This command covers the configured .NET 10 Server host only, not the whole E04 matrix.

## 6. Security and policy checks

```sh
npm run lint:security
npx gulp security-xss-scan
dotnet list "$LIB" package --vulnerable --include-transitive \
  2>&1 | tee "$EVIDENCE_DIR/nuget-vulnerabilities.txt"
npm audit --json > "$EVIDENCE_DIR/npm-audit.json"
```

- [ ] Review the NuGet report contents; command success alone does not mean no advisories. Include the other in-scope projects/dependency graphs and bundled assets in E03/E07.
- [ ] Review npm audit failures/findings; retain results and disposition rather than swallowing the exit code. Tool/database errors are not clean findings.
- [ ] Obtain matching CodeQL run evidence and actual analyzed-file coverage. CodeQL is separate from the CI pack dependency chain.
- [ ] Resolve the documented product-JS coverage gap in ESLint/XSS checks or perform the approved additional review; current exclusions must stay visible in the evidence.
- [ ] Complete required negative tests, security review, confidentiality checks and approved disclosures. No secrets in package, source, logs or public attachments.
- [ ] Collect required accessibility/performance/manual results and explicit scope exclusions using E09/E10. Do not claim unmeasured certification or budgets.
- [ ] Validate claimed library coexistence/migration scenarios under E11. If commercial access is unavailable, keep that case blocked or obtain an explicit scope decision removing the unverified claim; do not fabricate compatibility.
- [ ] Re-attest relevant suppressions and API decisions; retain security diagnostics as errors.

## 7. Reconcile SBOMs before packaging

See [E07](MANUAL-EXTERNAL-EVIDENCE-TRACKER.md#11-e07--sbom-checksum-and-license-reconciliation) for the detailed procedure.

- [ ] Preserve and resolve/retire the legacy checksum mismatch with provenance. Do not merely edit `sbom.cdx.json.sha256` to agree with unexplained bytes.
- [ ] Confirm generation actually ran with the approved tool. Current targets skip generation when `CI=true`, when `CycloneDxEnabled` is false, or when other guards are unmet. A successful CI pack is not proof of a fresh SBOM.
- [ ] Validate declared schema, tool identity and product/dependency inventory for every TFM; `.toolversion.json` currently contains labels, not proof of actual executable/schema versions.
- [ ] Test multi-target inclusion/freshness and stale-file rejection. Do not approve the current target wiring solely from inspection of a configured `None` item.
- [ ] Decide and approve the distribution contract (embedded and/or external artifacts, formats, hashes and retention) before treating it as mandatory. If the required contract cannot be met, fix R17 and stop the release.
- [ ] Review licenses/notices for shipped JS/styles/fonts and dependencies, not only NuGet references.

## 8. Build and inspect the unsigned candidate package

The preceding Release build must match the intended configuration/version. Package outside the repository; do not reuse an unexplained stale output directory.

```sh
dotnet pack "$LIB" -c Release --no-build -o "$EVIDENCE_DIR/unsigned"
PKG="$EVIDENCE_DIR/unsigned/Syncfusion.Blazor.Toolkit.$APPROVED_VERSION.nupkg"
test -s "$PKG"
unzip -l "$PKG" > "$EVIDENCE_DIR/package-files.txt"
unzip -p "$PKG" Syncfusion.Blazor.Toolkit.nuspec > "$EVIDENCE_DIR/package.nuspec"
unzip -q "$PKG" -d "$EVIDENCE_DIR/unpacked"
shasum -a 256 "$PKG" > "$EVIDENCE_DIR/unsigned-package.sha256"
for tfm in net8.0 net9.0 net10.0; do
  test -s "$EVIDENCE_DIR/unpacked/lib/$tfm/Syncfusion.Blazor.Toolkit.dll"
  test -s "$EVIDENCE_DIR/unpacked/lib/$tfm/Syncfusion.Blazor.Toolkit.xml"
done
```

- [ ] Package ID/version and `.nuspec` repository commit match the approved source. Verify SourceLink/PDBs independently; property strings alone are not build provenance.
- [ ] Inspect assembly/file/informational versions for all TFMs and any approved versioning policy; do not equate prerelease package version with a CLR assembly version string.
- [ ] Check current static assets, required CSS/scripts, XML docs, license/readme/notices and actual dependency groups.
- [ ] Check the exact required SBOM files/attachments and their identity/freshness. If embedding is approved, verify each expected `_sbom/cyclonedx/<tfm>/Syncfusion.Blazor.Toolkit.cdx.json` exists in this package and validates.
- [ ] Install this exact package into isolated consumers using a clean candidate-specific NuGet cache/feed and `PackageReference` only. Record restore evidence proving the intended bytes were selected.
- [ ] Repeat E02 compatibility checks against the approved-version package; exploratory results alone do not satisfy this gate. Execute E04/E05/E06 package scenarios without toolkit project references or unpublished workspace assets masking missing contents.
- [ ] Have an independent reviewer reconcile package contents, compatibility, consumer results and unresolved findings before handing the unsigned candidate to the authorized signing owner.

## 9. Authorized signing and go/no-go

- [ ] The owner-approved private procedure specifies which assembly/package signatures are required and why. Keep credentials outside the repository/public CI; no secret is entered into this checklist.
- [ ] Required assembly signing occurs before final packaging. Inspect resulting identity/dependency consequences; do not assume `sn -R` converts arbitrary unsigned assemblies safely.
- [ ] Verify final NuGet signatures, chain/timestamp policy and any applicable assembly signatures. Ordinary repeated signing calls are not a generic repository-countersigning procedure.
- [ ] Record exact **final signed** package and embedded/external evidence hashes. Update the detached evidence manifest after all signing/packaging changes; avoid circular self-hashes.
- [ ] Reinspect final contents and rerun affected package-consumer checks. Unsigned smoke checks do not automatically transfer to changed payloads.
- [ ] Required PREPUBLICATION cases within E01–E12 are reviewed; failures/blockers and exceptions are explicit. EXPLORATORY records are not candidate passes. Downloaded-package and post-release cases remain openly pending as POSTPUBLICATION requirements, not impossible prepublication prerequisites.
- [ ] Record the separate AUTHORIZATION decision after those prerequisite cases: final go/no-go approval names the exact source SHA, package version/hash, support scope, approvers, date and recovery owner. E12's approval record is not its own prerequisite; it summarizes the reviewed prepublication evidence.

Verification-only examples, once the authorized owner has produced the final package:

```sh
: "${FINAL_PACKAGE:?Set the owner-produced final package path}"
dotnet nuget verify "$FINAL_PACKAGE" --all
shasum -a 256 "$FINAL_PACKAGE"
```

A valid NuGet signature is not proof of per-DLL Authenticode, strong-name policy, source provenance, license completeness, or conformance. Record those separately under E08.

**Stop:** missing/mismatched identity, failed required tests/signatures, stale/inconsistent SBOM, undisclosed API breaks, unapproved evidence exceptions or absent release authorization. Do not bypass these by changing version overrides, deleting failures, or broad suppressions.

## 10. Manual publication and immediate post-release checks

Enter this section only after the separate AUTHORIZATION decision. Downloaded-package and public-feed cases in E06/E08, and applicable E12 monitoring cases, are POSTPUBLICATION work. Keep them pending before publication and require them for closeout; they are not prerequisites for publication authorization.

- [ ] Only the authorized owner performs tagging/publication using the approved process. Confirm the new tag points to the approved commit; never repoint historical tags.
- [ ] Publish the exact approved final package. Do not rebuild it during upload or substitute a similarly named file.
- [ ] Record the actual UTC publication date and release URLs; finalize changelog links/date without misrepresenting a planned date as already published. If documentation-only follow-up commits are needed, record their relationship to the package SHA/tag rather than moving the tag.
- [ ] GitHub release notes describe included changes and migrations; distinguish historical evidence, known limitations and actual release verification.
- [ ] Attach only the approved public SBOM/checksum/provenance/assessment artifacts. Private vulnerabilities, signer credentials and confidential reports remain private.
- [ ] Download the package independently, record its hash, inspect version/dependency/asset identity and verify signatures. Repository-added signatures may change package bytes; reconcile signer/content differences and retain both pre-upload and downloaded hashes.
- [ ] Run a clean consumer smoke using the public feed and exact new version, not a cached local build. Test basic chart rendering/interactivity and relevant changed features.
- [ ] Confirm README/current-version examples, release comparison links, sample onboarding displays and the evidence index reflect the release accurately. Do not rewrite historical record versions.
- [ ] Archive required CI evidence before expiration and confirm reviewer access, retention owner and support/security contacts.
- [ ] Record the next development version policy/Unreleased section through a separate reviewed change if required; do not accidentally rebuild the released package under that version.

## 11. Recovery and release closeout

- [ ] Monitoring/triage owner checks installation failures, regressions, security reports and documentation corrections during the agreed post-release window. Record the window and owner; this template establishes no new SLA or telemetry collection.
- [ ] For a bad release, hold promotion, coordinate deprecation/unlisting/advisory actions through authorized owners, and provide supported consumer rollback guidance. Never overwrite an existing package version or move its tag to conceal the defect.
- [ ] A corrected package gets a new approved version and repeats affected gates; keep the incident and prior evidence linked.
- [ ] Close the release record only after downloaded-package verification and evidence retention are complete. Record unresolved follow-ups with owner/date, not a blanket “all ready.”

### Copyable release signoff

Fill the AUTHORIZATION decision after reviewing required PREPUBLICATION evidence. Keep the POSTPUBLICATION fields pending until execution; complete them before closeout, not before go/no-go.

```markdown
## Release decision and closeout
- Version / tag / approved SHA: TBD
- Final signed package SHA-256: TBD
- Evidence index and required PREPUBLICATION case results: TBD
- Known limitations and approved exceptions (expiry): TBD
- Security/API/accessibility/package reviewers: UNASSIGNED
- AUTHORIZATION — release owner go/no-go + UTC date: NOT APPROVED
- POSTPUBLICATION — publication URLs + UTC date: NOT PUBLISHED
- POSTPUBLICATION — downloaded package SHA-256 and reconciliation: PENDING PUBLICATION
- POSTPUBLICATION — required case results and reviewer: NOT RUN
- Recovery/monitoring owner and window: UNASSIGNED
- Follow-up tasks / next review: TBD
- Closeout status / reviewer / date: OPEN / UNASSIGNED / TBD
```
