# Render-mode security

Blazor offers three render modes that map to very different security
postures. The components in this toolkit are designed to work in one or
more of them. Below is the contract.

## Static Server-Side Rendering (SSR)

- Executes on the server during the request.
- Has access to `IHttpContextAccessor`, `NavigationManager`, scoped DI
  services, and configuration.
- No SignalR circuit. No JS interop unless `IJSInteropConnection`
  is explicitly requested.
- The browser receives only the rendered HTML payload.

Any component in this mode can call server-only APIs; it is the
tightest threat surface but also the least interactive.

## Interactive Server

- A SignalR circuit carries component state between server and
  browser.
- Component code still runs on the server; only the DOM diff is
  shipped to the browser.
- Server-only APIs remain accessible; the component must not assume
  a browser-only environment.

## Interactive WebAssembly

- Component code is compiled into .NET assemblies loaded by the
  browser runtime. Anything that hits the server must go through
  `HttpClient` (`HttpClientInstance` on `SfUploader`).
- Components in this mode **MUST NOT** directly call
  `IHttpContextAccessor`, `IDbContextFactory<T>` for shared
  contexts, `SignInManager<T>`, or any other server-only API.
- The runtime throws a clear exception if such an API is reached
  from a WebAssembly component.

## How we detect and refuse

The pattern catalogue is:

- `Syncfusion.Blazor.Toolkit.Http.HttpHandlerRequirements` is a
  compile-time enforcer: it inspects the component graph and refuses
  to render a WebAssembly-interactive page that references
  server-only service markers.
- Each sample (`samples/Blazor.Toolkit.Samples/`) declares a single
  render mode in `Program.cs`; consumers porting the toolkit to a
  different render mode are expected to configure their
  `ComponentsWebAssemblyPreserveAssemblyAttributes` and prerender
  policy accordingly.
- The runtime fallback when a WebAssembly component tries to use an
  server-only API is to surface a clear, actionable exception that
  names the API and explains how to move the call server-side.

## Public references

- Microsoft Blazor render modes:
  <https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/rendering>
- WAI-ARIA Authoring Practices (referenced by all components with
  public APIs):
  <https://www.w3.org/WAI/ARIA/apg/>