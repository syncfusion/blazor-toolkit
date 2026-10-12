# Render-mode security

This describes current source mechanisms and host responsibilities, not a security boundary enforced by the toolkit or a tested compatibility matrix.

## Hosting boundaries

| Mode | Execution and security responsibility |
|---|---|
| Static server-side rendering (SSR) | Component code executes on the server for the request and produces HTML. Blazor does not call `OnAfterRenderAsync` during static rendering or prerendering; DOM-dependent interop is unavailable at that stage. Server access still needs authorization and output must not disclose secrets. Static output is not automatically safe, and it does not provide interactive toolkit behavior. |
| Interactive Server | Component code executes on the server; browser events, render updates, and interop cross the circuit connection. Treat browser inputs as untrusted. The host controls authentication, authorization, circuit resource limits, and endpoint security. An HTTP context is not a reliable representation of the entire interactive circuit lifetime. |
| Interactive WebAssembly | Client assemblies and configuration are accessible to the user. Keep secrets and privileged operations on authenticated, authorized server endpoints. Client validation is not access control; network calls may use `HttpClient` or JavaScript depending on the feature. Server-only dependencies must be separated by the application. |
| Interactive Auto | Interactive Server is used initially while the client bundle is obtained; subsequent visits can use WebAssembly. This is not an in-place migration of an existing server component instance. Configure and secure both execution hosts and any prerender path; do not assume one DI scope or state instance spans them. |

## What the toolkit actually does

Source: [`SfBaseComponent.cs`](../src/Base/SfBaseComponent.cs) and [`SyncfusionService.cs`](../src/Base/SyncfusionService.cs).

- `AddSyncfusionBlazorToolkit` registers `SyncfusionBlazorToolkitService` as **scoped**, localization services as singletons, and a scoped `HttpClient` if one is not already registered. It does not configure authentication, authorization, safe endpoint allowlists, or credentials.
- Scoped lifetime depends on the host: request scope for static SSR, circuit scope for Interactive Server, and client application lifetime for typical WebAssembly DI. These are not a cross-host security or synchronization mechanism.
- On the first interactive `OnAfterRenderAsync`, the base implementation creates a `DotNetObjectReference`, sets `IsRendered`, imports `./_content/Syncfusion.Blazor.Toolkit/scripts/base.js`, then calls `OnAfterScriptRenderedAsync`. Derived components can add imports. Even components with native HTML markup can inherit this base-module lifecycle.
- `IsJsInProcess` tests for `IJSInProcessRuntime` (with a bUnit exclusion) to select the interop reference type. `ImportModuleAsync` reuses existing references; base disposal contains reference-release and disconnection-handling paths. This is not proof that every derived component releases its listeners, tasks or registrations: [R00](../docs/evidence/2026-10-06-r00-1922761/index.md) leaves lifecycle/isolation findings F05–F07 open. These are mechanisms, not input validation or sandboxing.
- `IsStaticServerRendering` checks renderer information under `NET9_0_OR_GREATER`; on .NET 8 it returns false. It does not reject a component graph or authorize calls.

**Legacy enforcement claims superseded:** no implementation of `HttpHandlerRequirements` or the alleged `IJSInteropConnection` opt-in was found in source. The actual [`Data/HttpHandler.cs`](../src/Data/HttpHandler.cs) prepares and sends HTTP requests; it is not a compile-time service-graph checker. There is no toolkit-wide guarantee of a diagnostic naming every server-only API reached from WebAssembly. Framework/runtime errors are not a substitute for host separation and tests.

## Sample configuration and integration checks

The sample [server Program.cs](../samples/Blazor.Toolkit.Samples/Program.cs) enables both Server and WebAssembly interactive components/endpoints, registers toolkit services, and adds `IHttpContextAccessor`. The [client Program.cs](../samples/Blazor.Toolkit.Samples.Client/Program.cs) separately registers toolkit services and a base-address `HttpClient`. The samples do not establish a single-mode-only contract or production security assurance.

For the actual deployment, validate:

- service availability in each host and prerender path, without putting server secrets in the client;
- authorization for data and upload endpoints, antiforgery where applicable, upload size/content limits, and safe HTTP destinations;
- content encoding/sanitization at actual markup sinks and a CSP compatible with the deployed framework and scripts;
- initialization, navigation, disposal, disconnection, and Auto revisits in each supported configuration.

These are host verification tasks, not checks asserted to have passed by this document. See [SECURITY.md](SECURITY.md) for private disclosure and [THREAT-MODEL.md](THREAT-MODEL.md) for scan scope and unresolved risks.

## Public reference

[Microsoft: ASP.NET Core Blazor render modes](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes).