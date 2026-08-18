# MS Quality Bar Attestations

This file records per-catalog attestations for the Syncfusion Blazor Toolkit.
Format per line:

```
<catalog-id>: <status> — <one-line evidence>, <date>, <reviewer>.
```

## Active attestations

MS-2.10: attested — Toolkit avoids JSRuntime calls in OnInitializedAsync (verified by manual scan of `OnInitializedAsync` overrides across `src/Components/**` and bUnit regression suite `tests/Syncfusion.Blazor.Toolkit.BUnitTest/Base/RenderModeSecurityTests.cs`); render-mode guidance reviewed in .github/blazor-render-mode-guidance.md, 2026-08-17, Toolkit Security WG.
