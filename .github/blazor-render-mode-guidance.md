# Blazor Render-Mode Security Guidance

**Status:** Reviewed 2026-08-17
**Reviewers:** Toolkit Security WG
**Applies to:** Syncfusion.Blazor.Toolkit v1.x and later
**Related catalog entry:** MS-2.10 (render-mode-aware security, Should-fix, Auto + Self-attest)

## 1. Render modes the toolkit supports

| Render mode | Supported | Notes |
|---|---|---|
| Static SSR (prerender / no interactivity) | ✅ | Visual output only; no event handling. JS interop is a no-op. |
| Interactive Server (SignalR) | ✅ | Default for samples. All interop uses async `IJSRuntime`. |
| Interactive WebAssembly | ✅ | Interop uses in-process `IJSInProcessRuntime` when available. |
| Interactive Auto | ✅ | First render on server, then switches to WASM. |

## 2. Static SSR threat model

- The toolkit MUST NOT assume `IJSRuntime` is available in Static SSR.
- All interop calls are guarded by `IsStaticServerRendering()` (see
  `SfBaseComponent.IsStaticServerRendering()`) or by checking
  `RendererInfo.IsInteractive` before invoking JS.
- Components render markup-only in Static SSR; event handlers and progressive
  enhancement are deferred to the interactive re-render.
- The `#if NET9_0_OR_GREATER` guard around the renderer-name check is
  intentional: the runtime API is only reliable on .NET 9+; on .NET 8 the
  method conservatively returns `false`.

## 3. Interactive Server threat model

- SignalR circuit scope: all component state is server-side. Inputs are
  user-controlled; components MUST treat parameter values as untrusted.
- XSS surface: all rendered text uses Razor's default HTML encoding; no
  `MarkupString` is used for user-supplied content (verified by grep of
  `src/Components/**/*.razor` for `MarkupString` → 0 matches).
- Input sanitization: the Uploader includes an `EnableHtmlSanitizer` switch
  (`SfUploader.EnableHtmlSanitizer`) that loads `sanitize-html-helper.js` from
  the package, not from a CDN.
- Anti-forgery: toolkit does not manage user auth, sessions, or antiforgery
  tokens. The host application is responsible for `[ValidateAntiForgeryToken]`
  on form posts and SignalR circuit authorization.

## 4. IJSRuntime anti-pattern (MS-2.10)

The MS Quality Bar forbids `IJSRuntime.InvokeAsync` inside `OnInitializedAsync`.
The toolkit is in compliance — all interop is in `OnAfterRenderAsync`,
`OnParametersSetAsync`, or event handlers. The bUnit regression suite
(`tests/Syncfusion.Blazor.Toolkit.BUnitTest/Base/RenderModeSecurityTests.cs`)
asserts that no interop is attempted during component initialization.

## 5. Review cadence

- Triggered by: any new component, any new JS interop surface, any change
  to a lifecycle method.
- Reviewer: Toolkit Security WG.
- Cadence: once per minor release, plus on every addition listed above.

## 6. Related controls

- `Base/SfBaseComponent.cs` — `IsStaticServerRendering()`, `IsJsInProcess()`,
  `RendererInfo` / `AssignedRenderMode` guarded checks.
- `wwwroot/scripts/sanitize-html-helper.js` — bundled with the package, no
  external CDN dependency.
- `tests/Syncfusion.Blazor.Toolkit.BUnitTest/Base/RenderModeSecurityTests.cs`
  — bUnit regression coverage.
- `.github/ms-bar-attestations.md` — MS-2.10 attestation line.
