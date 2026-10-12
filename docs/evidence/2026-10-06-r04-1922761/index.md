# R04 — Chart text measurement: verified source fix, private review pending

**Date:** 2026-10-06. **Base:** `address-warnings` / `19227619082433fa3b86e15d4e7aee625850aed9`, plus preserved R02/R03 and R04 uncommitted changes. [Controlling plan](../../ENTERPRISE-READINESS-PLAN.md#9-execution-log-and-restart-record) · [Security policy](../../../.github/SECURITY.md).

**R04 remains OPEN.** The source implementation and bounded regressions are verified. The user selected the recommended approach: retain the open gate pending a named security owner's private review/disposition. No owner, severity, affected-release range, disclosure decision or release remediation is invented. No report has been sent externally; no commit, publication or risk waiver was authorized.

## Source implementation and compatibility

- Both Chart measurement functions use the shared `src/wwwroot/scripts/measurement-text.js` boundary. Input is constructed as text, never passed to an HTML parser. Only exact lowercase, attribute-free `<br>`, `<br/>`, `<b>`, `</b>`, `<strong>` and `</strong>` tokens affect formatting.
- All other markup and entity-looking strings remain literal. Null/undefined become empty; other values are stringified. Closing bold tokens close only a matching current parent; unmatched/mismatched closing tokens remain text. Unclosed permitted opening tokens retain their constructed children. This is deterministic formatting, not browser HTML-parser recovery or a general sanitizer.
- DOM measurement, font resolution, normal-to-bold maximum sizing, return casing and existing exports remain. Chart's lone-space special case uses a real nonbreaking space. No canvas/font-metric substitution, cache change, key-delimiter repair, managed API change or CSS migration was made.
- Literal entities/unsupported tags can change dimensions versus the old HTML interpretation. The separate tooltip renderer may strip unsupported tags that measurement now treats literally; unsupported-markup geometry equality is not promised. Supported break/bold geometry is tested against safely constructed DOM.
- The helper is a sibling ES-module static web asset. The project-reference browser host loads it successfully. Actual candidate-package inclusion and package-reference consumers remain **R16**, not established by source tests.

## Reachability and regression scope

Source tracing covers normal series/point tooltip text, axis crosshair text, and managed category/series collection into `getCharSizeByFontKeys`. The whole-string managed path is triggered by characters in **U+0600–U+06FF**, not by every non-Latin or RTL string. The fixture covers Arabic categories both with and without a break, initial layout and explicit `RefreshAsync(false)`, series header, mapped tooltip detail and actual axis crosshair tooltip. Transport/JSON encoding does not replace safety at the browser measurement boundary.

Nine new Playwright cases cover:

1. Three direct entry paths: SVG measurement, Chart measurement and keyed character measurement; unexpected descendants, local intercepted requests and a benign execution sentinel.
2. Two geometry cases: 14 strings × normal/bold × LTR/RTL per path (**112 comparisons**), including Unicode/combining/emoji, spaces, entities, angle brackets, breaks and allowed formatting.
3. Character JSON keys/casing/numeric dimensions and 558 width entries.
4. Eleven explicit formatting/reuse cases, including malformed nesting, unsupported tags, null/undefined and numeric values.
5. Reused SVG node, normal→bold→normal sizing, header/theme fallbacks and stale-child clearing.
6. Integrated layout/explicit refresh/tooltip/crosshair, transient measurement-node observation, current text, positive finite crosshair geometry and no browser exceptions.

The max-bold case verifies current output but does not establish that a normal dimension exceeds bold for the chosen font. The observer covers child additions, not arbitrary attribute-only mutations or detached-only parsing. These are scoped regressions, not universal XSS or geometry certification.

## Executed verification

Environment: macOS 27.0.1 arm64; SDK 10.0.401; .NET 8.0.31 bUnit and .NET 10.0.12 Interactive Server browser host; Node 26.10.0; npm 12.1.0; Playwright 1.63.0 / Chromium 153.0.8010.12. Browser runs use one worker, retries disabled, Release source/project references.

| Check | Result |
|---|---|
| Initial six direct cases, original source sinks | **5 failed, 1 passed**; expected boundary/geometry failures |
| Final nine cases with original sink logic temporarily restored in disposable worktree | **6 failed, 3 passed, 0 skipped/flaky**; integration reached the final unexpected-descendant assertion |
| Restored safe implementation, final browser run | **22 passed, 0 failed/skipped/flaky**: nine new cases plus existing Chart title/series/point/legend and NumericTextBox keyboard-step smoke, each repeated twice |
| Main bUnit Release/net8 | **2,738 passed, 0 failed, 0 skipped**; existing test compilation warnings retained |
| Fresh nonincremental Release library, net8/net9/net10 | **0 warnings, 0 errors** |
| Pinned R03 API/current-contract gate | **Pass all three TFMs**; no new exceptions or snapshot changes |
| Focused strict TypeScript check | **Pass**, ES2022/node16 module settings; original checkout has no installed Playwright declarations, so editor diagnostics there are environment-dependent |
| Source review | Two read-only agent reviews; bounded crosshair/reuse/malformed/no-break coverage gaps addressed; no final implementation blocker identified. Not independent security-owner approval |

The final red control retained the unused new helper/import but restored both original parsing sinks; it tests the old sink behavior, not a pristine released package. Both temporary mutations were reverted. The final five R04 source/test files match the user checkout byte-for-byte. Probe inventories execute on net10, not three runtime hosts. No complete browser suite, WebAssembly, Firefox/WebKit, package, AOT/trim, accessibility, performance or fresh security/dependency scan was performed.

### Failed investigation attempts retained

- Early integration attempts failed before the security assertion and are **not** fail-before security proof. An attributed-tag fixture hit the existing tooltip `isValidHTMLElement` validator exception; the bounded fixture uses inert unsupported tags while direct tests retain handler/request sentinels.
- Unbounded host CSS stretched the SVG viewport while hit-testing used internal fixed dimensions. The fixture now explicitly matches its 800×400 container and asserts the actual SVG bounds; production CSS/hit-testing was not altered.
- Replacing data alone follows incremental update and does not guarantee a new whole-string measurement. The fixture waits for child parameters, explicitly refreshes, and publishes a completed-generation marker. Mutation records capture added text instead of only the final reused node value.
- The first crosshair assertion assumed axis index zero. It now asserts exactly one enabled-axis tooltip without assuming internal collection order.
- Initial TypeScript invocation used deprecated node10 resolution; current node16 configuration passed. The editor test runner could not discover the isolated project, so the documented CLI fallback ran the full suite.

## Reproduction commands (source only)

- Browser: `npx --no-install playwright test Charts/Security/text-measurement.spec.ts --workers=1 --retries=0 --reporter=line,json` with explicit output/report paths. The final repeat adds `Charts/Others/chart-basics.spec.ts` and `Inputs/NumericTextBox/min-max-step.spec.ts`, exact smoke-name filtering and `--repeat-each=2`.
- Managed: `dotnet test tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj -c Release -p:CycloneDxEnabled=false --logger "trx;LogFileName=r04-bunit.trx" --results-directory <evidence>/bunit`.
- Build: `dotnet build src/Syncfusion.Blazor.Toolkit.csproj -c Release --no-incremental -p:CycloneDxEnabled=false -v:minimal`. SBOM opt-out is explicit for this source check, not SBOM verification.
- R03 gate: existing `tools/api-compat/gate.py` with fresh three-TFM assemblies, existing reviewed probe, exact hash-checked downloaded 1.0.2 package and isolated output directory.
- Types: `npx --no-install tsc --noEmit --strict --target ES2022 --moduleResolution node16 --module node16 --skipLibCheck <text-measurement.spec.ts>` in the dependency-installed worktree.

## Evidence and private disposition

Raw reproduction logs/traces/source snapshots are **not bundled in this repository**. Local retention reference: `R04-LOCAL-20261006`, archive `local-measurement-evidence.tar.gz`, SHA-256:

`539c97adb37225f38e49f2974247c3c3d7518046bf42425641d5bad03d80ace7`

A local copy is retained under the current user's Code Studio application-support `readiness-evidence/2026-10-06-r04-1922761/` directory (directory mode 0700, archive 0600). Raw working evidence also remains at `/private/tmp/blazor-toolkit-r04-8r09s3/evidence`; the isolated source is its sibling `source`. Local permissions and checksums do **not** make this an approved organizational evidence store. Named owner, private ticket/reference, transfer/access policy, retention period, released-version assessment and disclosure decision remain unassigned. Source tests also contain a local benign regression sentinel; publication review must cover them before sharing the diff.

Tested SHA-256 identities:

| File | SHA-256 |
|---|---|
| `src/wwwroot/scripts/measurement-text.js` | `cbbe44af35246243a0a448538819396408043d056b7db1c74ea7bbd1a92525fc` |
| `src/wwwroot/scripts/chart.js` | `4b3af364a9e61250b9a893f28cfae851be949901189cfa9ab1ca1aed3b94738b` |
| `src/wwwroot/scripts/svgbase.js` | `d40d74ec5b70f3070420b851d4724ef39a1d4d7372ca1b3ce562e5d0e1333eab` |
| `text-measurement.spec.ts` | `19acb5984a42fa37f119345a86eb8b145d162172c36397b65994ed5c5755e421` |
| `TextMeasurement.razor` | `85c5df6a1edc217a69f2cdafe9ed40840e2f115985fd6f131a0121e4fe83248a` |

Prior R00–R03/R03a archive hashes and original top-level SBOM bytes/checksum were rechecked unchanged. Incoming user files remain preserved. Generated SBOM output was confined to isolated builds. No commit, push, tag, signing, publication, remote-setting change or external vulnerability submission occurred.

## Handoff

- **R04:** source fixed/verified; **blocked on named private security review/disposition**, not marked complete.
- **R10/R11 follow-up:** investigate separately the existing attributed-tag tooltip validator exception, unsupported-markup tooltip/measurement differences, and CSS viewport/hit-testing mismatch. No universal tooltip-safety or layout fix claimed.
- **R16:** verify helper inclusion and matching-version module loading in actual package-reference consumers.
- **Next independently eligible implementation:** R05 uploader metadata text rendering, only on the next user continuation. R04's pending review must remain visible. All nine final release-readiness gates remain open.
