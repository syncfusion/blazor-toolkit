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

- bUnit: **2,735** tests passed (0 failed, 0 skipped) on .NET 8 as of
  the DEVELOPMENT.md verification note (2026-10-03). That figure is
  **executed test cases** from `dotnet test` on
  `tests/Syncfusion.Blazor.Toolkit.BUnitTest`, not a count of
  `[Fact]`/`[Theory]` method declarations alone.
- Re-count after meaningful test changes:
  `dotnet test tests/Syncfusion.Blazor.Toolkit.BUnitTest --logger "console;verbosity=minimal"`.
- The upload `bunit-results-<tfm>` is published as a repository artifact
  by `.github/workflows/ci.yml`.
- All public `Sf*` components should keep at least one happy-path render test.

## Gaps

- The unpacked WebAssembly sample smoke runs on .NET 10 only; .NET 8
  and .NET 9 WASM are out of scope for that one job.
- Visual regression / browser tests live under
  `tests/Syncfusion.Blazor.Playwright.Test/` (see also repo-root
  `playwright.config.ts`). Baseline or snapshot assets, if used, are
  stored with that project or produced by the Playwright run—not under
  a separate `tests/playwright-baselines/` folder.
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