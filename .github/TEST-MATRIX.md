# Test matrix

Quick coverage summary for the Syncfusion Blazor Toolkit CI matrix.

## Runner matrix

| Renderer | .NET 8 | .NET 9 | .NET 10 |
|---|:---:|:---:|:---:|
| bUnit (component tests) | ✅ | ✅ | ✅ |
| Playwright (visual regression, accessibility smoke) | ✅ | ✅ | ✅ |
| NuGet vulnerability scan | ✅ | ✅ | ✅ |
| ESLint security | n/a | n/a | ✅ (Node 22, repo-wide) |
| XSS / unsafe markup scan | n/a | n/a | ✅ (Node 22, gulp) |
| `dotnet pack` (smoke, unsigned `.nupkg` for human review) | ✅ | ✅ | ✅ |

## Sanitised coverage from the bUnit report

- 132 `Fact`/`Theory` test cases as of 2026-09-06; the upload
  `bunit-results-<tfm>` is auto-published as a repository artifact
  by `.github/workflows/ci.yml`.
- All 17 `Sf*` components have at least one happy-path render test.

## Gaps

- The unpacked WebAssembly sample smoke runs on .NET 10 only; .NET 8
  and .NET 9 WASM are out of scope for that one job.
- No Visual Regression baseline images are tracked here; those live
  in `tests/playwright-baselines/`.
- Accessibility Insights runs nightly, not on PR — see
  [`.github/accessibility/insights-summary.md`](accessibility/insights-summary.md).

## Test-evidence list (where to find the latest numbers)

- `.github/workflows/ci.yml` summary comment on each PR.
- bUnit `.trx` and `.html` artifacts, gated to a 14-day GitHub
  Actions retention.
- Playwright HTML report, gated to a 14-day retention.
- NuGet vulnerability scan (exit-code-driven; on push, gates the
  pack job).
- `actions/attest-build-provenance` produced on every successful
  push to `main`.