# R05 — Uploader metadata text rendering: source verified, private review pending

**Date:** 2026-10-06. **Base:** `address-warnings` / `19227619082433fa3b86e15d4e7aee625850aed9` plus preserved uncommitted work. [Plan](../../ENTERPRISE-READINESS-PLAN.md#9-execution-log-and-restart-record) · [Behavior migration](../../API-COMPATIBILITY.md#r05-selected-filename-behavior-correction--2026-10-06) · [Security policy](../../../.github/SECURITY.md).

**R05 remains OPEN pending named private security-owner disposition.** Source changes and scoped automated verification are complete. The user selected the recommended original-filename correction and private-review hold, as with R04. No release, disclosure, risk waiver or independent security approval follows.

## Implementation and boundary

- `uploader.js`: eight metadata-related HTML assignments now use `textContent`: filename/extension in AJAX and legacy form-list creation, size, initial status, auto-upload status clearing and cancellation status. Existing status updates, progress text and truncation text getters remain intact.
- Default Razor metadata was already rendered through encoded expressions and is unchanged. Regression tests now execute the actual JS-to-Razor paths rather than relying on absent HTML in a mocked list.
- Optional selected-filename validation no longer rewrites names to numeric entities. Original names remain in callback metadata and templates. Invalid comparison results still receive status code `0`; this behavior correction was explicitly presented to the user and the recommended approach selected. No callback/signature/JSON-property-shape change or new API exception.
- Sanitizer validation now creates its `div` in an inert template-content owner document and never adopts parsed nodes. This preserves the div parsing context while preventing the demonstrated registered custom-element constructor from running during validation. Banned-tag removal uses `element.remove()` instead of the undefined global `detach`.
- `EnableHtmlSanitizer` XML documentation now distinguishes optional validation from unconditional default text rendering, application-owned templates and server path validation. The helper is used only by uploader selection in the inspected production source; it is not certified as a general HTML sanitizer.
- Five remaining uploader HTML assignments belong to internal spinner label/template handling and fixed empty button clearing, not these ordinary metadata paths. Current uploader spinner call sites do not supply metadata labels/templates. No shared DOM helper, template flattening, upload-header policy or resource-budget change was made.

## Reproduction and meaningful coverage

The fixture uses a valid root-relative AJAX URL and asserts the initialized route, sanitizer and template settings. Existing uploader samples use relative URLs rejected by current URL validation and therefore cannot be assumed to test AJAX rendering. Test selectors do not depend on metadata attributes that the existing shared `createElement` helper omits.

All upload/probe and external-origin requests are intercepted and aborted; tests expect **zero attempts**. No storage/upload endpoint was added and no external upload was performed. Real tiny file selections still cross the local Interactive Server circuit; `AutoUpload=false` is not a guarantee that no bytes enter .NET.

**40 new browser cases:**

- AJAX/Razor × sanitizer on/off: preloads, real selection with exact filename/callback identity, benign/no-extension/Unicode/ampersand names and existing size validation, custom-element/nested-resource selections, public `CreateFileListAsync` metadata and `FileSelected` modified metadata/status.
- Template AJAX/Razor × sanitizer on/off × preload/selection/public API: expected rows, literal data and preserved trusted `strong` structure. Application-authored raw markup remains a separate trust boundary.
- AJAX cancellation/status/progress/truncation with both sanitizer settings; localized cancellation text injected through the real instance method, not an HTTP lifecycle test.
- Direct legacy form-list helper coverage and direct optional-validator serialization/banned-tag coverage. The legacy form route is not normally reached through mutually exclusive AJAX/no-URL predicates; direct coverage is not presented as a normal form-upload integration test.

Assertions include nonzero exact row counts, exact displayed name/extension/status, no metadata descendants, transient added-node observation, callback status codes, a benign constructor/execution counter, no probe requests and no browser exceptions. Truncation is forced with a narrow test-only style and retains the raw ten-code-unit tail; progress remains `37%` without losing sibling text.

## Executed results

Environment: macOS 27.0.1 arm64; SDK 10.0.401; .NET 8.0.31 bUnit; .NET 10.0.12 Interactive Server Release browser host; Node 26.10.0, npm 12.1.0, Playwright 1.63.0 / Chromium 153.0.8010.12. Browser runs use one worker, retries disabled.

| Check | Recorded result |
|---|---|
| Initial 20 browser cases against unchanged uploader | **9 failed, 11 passed**; AJAX parsing and sanitizer-on filename rewriting reproduced; Razor/template controls passed |
| Four supplemental cases before production change | **4 failed**: two cancellation cases, legacy name/type parsing, undefined sanitizer `detach` |
| Initial fixed implementation | **24 passed** |
| Review-driven control before inert-document fix | **4 passed, 2 failed**: custom-element counter correctly exposed one construction; the other failure was an incorrect expected localization string, not a security reproduction |
| Expanded final matrix | **40 passed, 0 failed/skipped/flaky** |
| Final repeated combined browser run | **102 passed, 0 failed/skipped/flaky**: 40 R05 + 9 R04 + existing Chart render and NumericTextBox keyboard-step smoke, each twice |
| Full bUnit Release/net8 | **2,738 passed, 0 failed, 0 skipped**; existing test compilation warnings retained |
| Fresh nonincremental library Release/net8/net9/net10 | **0 warnings/errors** |
| Existing pinned released API/current-contract gate | **Pass all three TFMs**, unchanged exception/snapshot files |
| Focused TypeScript | **Pass with explicit temporary pinned `@types/node@22.10.2`**, TypeScript 6.0.3; missing repository Node declarations caused earlier failed checks and remain a tooling limitation |
| JS syntax / scoped whitespace | Both changed scripts pass `node --check`; production diff whitespace check passes |
| Review | Two read-only agent reviews; inert parsing, validation-flag and template-lifecycle gaps addressed. No final source blocker identified; not security-owner approval |

The final 40-case suite was not wholesale rerun against original source: retained fail-before runs cover the initial sink cases, supplemental paths and the later custom-element defect independently. No temporary vulnerable production mutations remain. Source/test bytes match the user checkout. The API probe inventories all TFM assemblies on net10, not three runtime hosts.

## Commands and setup limits

- Isolated detached worktree at the base SHA, overlaid with preserved current source changes for **dirty-tree regression verification**, not a new clean baseline. Existing R04 locked Node installation was reused through a local symlink; themes were regenerated only in the isolated tree. No repository dependency or lockfile changed.
- Browser: `npx --no-install playwright test Inputs/Uploader/metadata-rendering.spec.ts --workers=1 --retries=0 --reporter=line,json`, explicit report/output paths. Final combined run adds the R04 spec and two exact named smoke tests, `--repeat-each=2`.
- Managed: `dotnet test tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj -c Release -p:CycloneDxEnabled=false --logger 'trx;LogFileName=r05-bunit.trx' --results-directory <evidence>/bunit`. Editor runner did not discover the isolated test target; documented CLI fallback used.
- Build: `dotnet build src/Syncfusion.Blazor.Toolkit.csproj -c Release --no-incremental -p:CycloneDxEnabled=false -v:minimal`. This SBOM opt-out is only for source verification, not SBOM evidence. An earlier editor build also succeeded.
- API: existing `tools/api-compat/gate.py`, fresh Release assemblies, reviewed existing probe, exact hash-checked 1.0.2 nupkg, isolated output. No rebaseline.
- Types: `tsc --noEmit --strict --types node --typeRoots <temporary-install>/node_modules/@types --target ES2022 --moduleResolution node16 --module node16 --skipLibCheck <metadata-rendering.spec.ts>`. Earlier checks with missing declarations retained, not suppressed or represented as repository-native passes.

## Known limitations / follow-ups

- Existing missing `InvalidFileName` localization returns that key. Razor's within-size non-ready branch remaps the status to its invalid-type message. Tests preserve/document these behaviors; message repair is R11 work, not silently included here.
- AJAX hides the extension span for extensionless names; Razor retains its existing period. Callback identity and exact DOM text are tested, not identical presentation between those paths.
- Editor diagnostics still suggest converting the unchanged block namespace in `SfUploader.razor.Members.cs` to file-scoped form. This unrelated style suggestion was not fixed or suppressed; the recorded fresh build has zero warnings/errors.
- Existing helper omission of `attrs`, duplicated child-content structure, relative sample URL validation and upload-cancellation transport ordering remain outside this rendering fix. No claim that cancellation prevents a request, or that uploads are bounded/authorized. R06/R07 remain required.
- Observer coverage is child additions, not every attribute-only mutation or detached tree. Inert parsing and direct counters provide separate evidence; no universal sanitizer/XSS proof.
- Only Chromium/net10 Interactive Server source/project references were exercised. No full browser suite, Firefox/WebKit, WebAssembly, actual package consumer, AOT/trim, accessibility, load benchmark, fresh dependency audit or released-version remediation assessment. Actual shipped-script/package verification remains R12/R16/R19.

## Evidence retention and disposition

Detailed reproduction logs and source snapshots are **outside the repository**. Local reference **`R05-LOCAL-20261006`**, archive `local-uploader-evidence.tar.gz`, SHA-256:

`c89fcfbbb78006c23fb8b887ccb58649816fcac2f0502636c2c8a786ffd780d6`

Retained under the current user's Code Studio application-support `readiness-evidence/2026-10-06-r05-1922761/` directory (0700; archive 0600). Working evidence remains `/private/tmp/blazor-toolkit-r05-pmDAG7/evidence`, with source in sibling `source`. These local permissions/hash do not constitute an approved organizational store. Owner/private ticket, secure transfer, retention policy, impact/affected-release assessment and disclosure decision are **unassigned**. Review regression fixture contents before externally sharing the diff. No external report has been sent.

| Tested file | SHA-256 |
|---|---|
| `src/wwwroot/scripts/uploader.js` | `87d2a159dcb414ff33c527318fb400d16d24e7b4785d24e0b0f775af02eff713` |
| `src/wwwroot/scripts/sanitize-html-helper.js` | `b2aabacbea2e9bb82a56e0610175964cb633bd47ba03062998eb7ea6c14183a7` |
| `SfUploader.razor.Members.cs` | `cd09e22bcd4fa133645a8b4bfa5949cafdb06a507a5d8e494166afa9674cae3d` |
| `MetadataRendering.razor` | `5ac4a05d3f84d2b1b37ef2397a3aa404d751527afc80983de0d2a9a4474177ad` |
| `metadata-rendering.spec.ts` | `b09d6aef41f905b0f72442c2faa27d690647ccadedcf891a8d38b4e9e552c9f9` |

Prior R00–R03/R03a archive hashes and original top-level SBOM bytes remain unchanged; incoming R04 and user changes preserved. No commit, push, tag, signing, publication, remote-settings change or vulnerability submission occurred.

**Handoff:** R05 source verified; checklist remains unchecked pending named private review, as explicitly recommended and selected. R04's existing hold remains. Next eligible implementation is **R06 — clarify and constrain uploader credential forwarding**, with its required policy decision before changing credential behavior. Stop at the R05 task boundary. All nine final release gates remain open.
