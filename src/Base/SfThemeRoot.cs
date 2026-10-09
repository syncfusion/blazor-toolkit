using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Toolkit
{

    /// <summary>
    /// Render-time emitter of the shared theme layer (Tier 1): the <c>:root</c> design tokens, base
    /// and utility styles, icon font, keyframes and the dark / high-contrast / forced-colors rules,
    /// written into the component output as a single
    /// <c>&lt;style id="sf-theme-root" data-sf-theme-root="true"&gt;</c> element.
    /// </summary>
    /// <remarks>
    /// <para>Every consumer-facing root toolkit component renders <c>&lt;SfThemeRoot /&gt;</c> as the
    /// <em>previous sibling</em> of its own root element. Only one instance per Blazor renderer (the
    /// <em>owner</em>) emits the element; the others render nothing. Because the style is part of the
    /// render output it is present in the static SSR / prerendered HTML, in the first interactive
    /// render, and in every render-mode transition, with no JavaScript and no consumer configuration.</para>
    /// <para><b>Ownership</b> is renderer-scoped (see <see cref="SfThemeScope"/>) and recovers
    /// automatically: when the owner is disposed the oldest surviving instance becomes the owner and
    /// re-renders in the same render batch, so the theme never disappears while a toolkit component is
    /// still alive.</para>
    /// <para><b>Placement rules</b> (enforced by <c>SfThemeRootEnforcementTests</c>): the emitter must be
    /// a sibling placed immediately before the component's root element, never inside the root element
    /// (this would disturb <c>:first-child</c>, sibling and <c>:empty</c> selectors, flex and grid
    /// layouts), never inside <c>&lt;svg&gt;</c>, and never inside popup / portal content that is moved
    /// in the DOM by script.</para>
    /// <para>CSS isolation (<c>Component.razor.css</c> -> <c>bundle.scp.css</c> -> <c>App.styles.css</c>)
    /// is a separate tier and is intentionally untouched by this component.</para>
    /// </remarks>
    public sealed partial class SfThemeRoot : IComponent, IDisposable
    {
        /// <summary>The DOM id of the emitted style element.</summary>
        internal const string StyleElementId = "sf-theme-root";

        private RenderHandle _renderHandle;
        private SfThemeScope? _scope;
        private bool _hasRendered;
        private bool _disposed;

        /// <summary>The DI scope that activated this component (identifies the renderer together with its dispatcher).</summary>
        /// <exclude />
        [Inject]
        internal IServiceProvider? Services { get; set; }

        /// <summary>Gets a value indicating whether this instance currently emits the style element.</summary>
        internal bool IsOwner => _scope?.IsOwner(this) ?? false;

        /// <inheritdoc />
        void IComponent.Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
        }

        /// <inheritdoc />
        Task IComponent.SetParametersAsync(ParameterView parameters)
        {
            // The emitter has no parameters. Registration happens exactly once, synchronously, while
            // the parent's diff instantiates this component. That makes the owner election
            // deterministic (tree order) and independent of OnAfterRender, JavaScript or hydration.
            if (_scope is null)
            {
                IServiceProvider services = Services ?? throw new InvalidOperationException(
                    $"{nameof(SfThemeRoot)} requires an {nameof(IServiceProvider)} to identify its renderer.");
                _scope = SfThemeScope.For(services, _renderHandle.Dispatcher);
                _scope.Register(this, services.GetService(typeof(IJSRuntime)) as IJSRuntime);
                _renderHandle.Render(BuildRenderTree);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Releases ownership. If this instance owns the style element, ownership is transferred to the
        /// oldest surviving instance of the same renderer so the theme is never lost.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _scope?.Unregister(this);
        }

        /// <summary>Called by <see cref="SfThemeScope"/> when this instance has just become the owner.</summary>
        internal void Promote()
        {
            if (!_hasRendered || _disposed)
            {
                // Not rendered yet: the first render will observe IsOwner == true by itself.
                return;
            }

            // Normally already on the renderer's dispatcher (disposal runs inside the render batch), in
            // which case this runs inline and the re-render joins the batch that is being built.
            _ = _renderHandle.Dispatcher.InvokeAsync(() =>
            {
                if (_disposed)
                {
                    return;
                }

                try
                {
                    _renderHandle.Render(BuildRenderTree);
                }
                catch (ObjectDisposedException)
                {
                    // The renderer is already gone; nothing left to keep styled.
                }
                catch (InvalidOperationException)
                {
                    // Renderer torn down concurrently (e.g. circuit disconnect); nothing to recover.
                }
            });
        }

        private void BuildRenderTree(RenderTreeBuilder builder)
        {
            _hasRendered = true;

            if (_disposed || !IsOwner)
            {
                return;
            }

            // IMPORTANT - DO NOT "FIX" THIS INTO builder.AddContent(...) / @Payload / an HTML-encoded
            // string. CSS contains characters that must reach the browser verbatim ('>' child
            // combinators, quotes inside url()/content, '&' ...). Encoding them silently breaks rules.
            // The payload is a trusted compile-time constant (never user input), so raw markup is safe.
            builder.AddMarkupContent(0, Markup._value);
        }

        private static class Markup
        {
            // Built once per process (read-only, immutable, shared by every renderer - this is NOT an
            // ownership flag). A nested class guarantees SfThemeRoot's static initializers (which live
            // in the generated SfThemeRoot.Payload.cs) have completed before this runs.
            internal static readonly string _value = string.Concat(
                "<style id=\"", StyleElementId, "\" data-sf-theme-root=\"true\">",
                Payload,
                "</style>");
        }

        #region JavaScript fallback

        // Fallback ONLY. The render-time emitter above is the delivery mechanism. This path exists for
        // a derived SfBaseComponent that (incorrectly) omits <SfThemeRoot />: the first time such a
        // component renders in a renderer that has no owner, the payload is injected through
        // document.head, guarded by an id check in JS so it can never duplicate an existing element.
        private static readonly ConditionalWeakTable<IJSRuntime, Task> _fallbackTasks = [];
        private static readonly object _fallbackGate = new();

        /// <summary>
        /// Injects the theme through JavaScript when - and only when - the renderer of
        /// <paramref name="js"/> has no <see cref="SfThemeRoot"/> owner. Concurrent callers share a
        /// single in-flight interop call per runtime.
        /// </summary>
        internal static ValueTask EnsureFallbackAsync(IJSRuntime js)
        {
            if (js is null || SfThemeScope.HasOwner(js))
            {
                return ValueTask.CompletedTask;
            }

            TaskCompletionSource completion;
            lock (_fallbackGate)
            {
                if (_fallbackTasks.TryGetValue(js, out Task? inFlight))
                {
                    return ObserveAsync(inFlight);
                }

                completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
                _fallbackTasks.Add(js, completion.Task);
            }

            return RunFallbackAsync(js, completion);
        }

        private static async ValueTask RunFallbackAsync(IJSRuntime js, TaskCompletionSource completion)
        {
            try
            {
                await js.InvokeVoidAsync("sfBlazorToolkit.themeRoot.ensure", StyleElementId, Payload).ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is JSDisconnectedException or InvalidOperationException or JSException or OperationCanceledException)
            {
                // Prerender / disconnected circuit / script not loaded: allow a later component to retry
                // on the live runtime instead of caching the failure.
                lock (_fallbackGate)
                {
                    _ = _fallbackTasks.Remove(js);
                }
            }
            finally
            {
                _ = completion.TrySetResult();
            }
        }

        private static async ValueTask ObserveAsync(Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is JSDisconnectedException or InvalidOperationException or JSException or OperationCanceledException)
            {
                // Errors are handled by the caller that started the interop call.
            }
        }

        #endregion
    }
}