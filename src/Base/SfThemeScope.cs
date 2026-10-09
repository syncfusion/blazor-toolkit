using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Toolkit
{

    /// <summary>
    /// Renderer-scoped bookkeeping for <see cref="SfThemeRoot"/>: tracks every live
    /// <see cref="SfThemeRoot"/> that belongs to one Blazor renderer and elects exactly one of
    /// them as the <em>owner</em> that emits the shared <c>&lt;style id="sf-theme-root"&gt;</c>.
    /// </summary>
    /// <remarks>
    /// <para><b>Why not a static flag?</b> A process-wide flag (<c>static bool emitted</c>,
    /// <c>Interlocked.Exchange</c>, an AppDomain-wide singleton, ...) is wrong because every static
    /// SSR response is a different HTML document, every Blazor Server circuit and every browser tab
    /// has its own DOM, and Interactive Auto swaps renderers at runtime. Ownership therefore has to
    /// follow the lifetime of the <em>renderer</em>, not the process.</para>
    /// <para><b>Renderer identity.</b> Blazor does not expose the <c>Renderer</c> instance to
    /// components. The two renderer-lifetime objects that <em>are</em> reachable are the renderer's
    /// <see cref="IServiceProvider"/> (the DI scope that activated the component: one per static SSR
    /// request, one per Blazor Server circuit, the root provider in WebAssembly) and its
    /// <see cref="Dispatcher"/> (one per standalone <c>HtmlRenderer</c>, one per circuit, one for
    /// WebAssembly). The pair <c>(IServiceProvider, Dispatcher)</c> is unique per renderer in every
    /// hosting model, including a standalone <c>HtmlRenderer</c> that is created repeatedly on top of
    /// one shared root provider.</para>
    /// <para><b>GC / lifetime safety.</b> Scopes are stored in <see cref="ConditionalWeakTable{TKey, TValue}"/>
    /// instances keyed by those renderer-lifetime objects. Entries never extend the lifetime of a
    /// request, circuit or runtime, and are reclaimed together with the key. Members are removed
    /// deterministically from <see cref="SfThemeRoot.Dispose"/>, so a scope never pins a disposed
    /// component.</para>
    /// <para><b>Recovery.</b> When the owner is disposed (navigation, conditional rendering, the
    /// owning island being removed) ownership is <em>transferred</em> to the oldest surviving member
    /// and that member is re-rendered. Blazor disposes components inside the same render batch that
    /// removed them, so the successor's re-render joins that batch and the style element is never
    /// absent from the DOM in between.</para>
    /// </remarks>
    internal sealed class SfThemeScope
    {
        private static readonly ConditionalWeakTable<IServiceProvider, ProviderScopes> _byProvider = [];

        // Secondary index used only by the JavaScript fallback so that SfBaseComponent (which can reach
        // IJSRuntime but not the renderer Dispatcher) can ask "does this renderer already have an owner?".
        private static readonly ConditionalWeakTable<IJSRuntime, SfThemeScope> _byRuntime = [];

        private readonly object _gate = new();
        private readonly List<SfThemeRoot> _members = [];
        private volatile SfThemeRoot? _owner;

        /// <summary>Gets the member that currently emits the style element, if any.</summary>
        internal SfThemeRoot? Owner => _owner;

        /// <summary>Gets the number of live members (owner + waiting candidates).</summary>
        internal int MemberCount
        {
            get
            {
                lock (_gate)
                {
                    return _members.Count;
                }
            }
        }

        /// <summary>
        /// Returns the scope that belongs to the renderer identified by
        /// <paramref name="services"/> + <paramref name="dispatcher"/>, creating it on first use.
        /// </summary>
        internal static SfThemeScope For(IServiceProvider services, Dispatcher dispatcher)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(dispatcher);
            ProviderScopes scopes = _byProvider.GetValue(services, static _ => new ProviderScopes());
            return scopes._byDispatcher.GetValue(dispatcher, static _ => new SfThemeScope());
        }

        /// <summary>
        /// Returns <see langword="true"/> when the renderer that owns <paramref name="js"/> already has
        /// a live <see cref="SfThemeRoot"/> owner (the theme is part of the rendered output).
        /// </summary>
        internal static bool HasOwner(IJSRuntime js)
        {
            return js is not null && _byRuntime.TryGetValue(js, out SfThemeScope? scope) && scope.Owner is not null;
        }

        /// <summary>Adds <paramref name="root"/> as a member; it becomes the owner if there is none.</summary>
        internal void Register(SfThemeRoot root, IJSRuntime? js)
        {
            lock (_gate)
            {
                _members.Add(root);
                _owner ??= root;
            }

            if (js is not null)
            {
                _byRuntime.AddOrUpdate(js, this);
            }
        }

        /// <summary>
        /// Removes <paramref name="root"/>. If it was the owner, ownership is transferred to the oldest
        /// remaining member (which is then asked to render the style element).
        /// </summary>
        internal void Unregister(SfThemeRoot root)
        {
            SfThemeRoot? successor;
            lock (_gate)
            {
                if (!_members.Remove(root))
                {
                    return;
                }

                if (!ReferenceEquals(_owner, root))
                {
                    return;
                }

                successor = _members.Count > 0 ? _members[0] : null;
                _owner = successor;
            }

            successor?.Promote();
        }

        /// <summary>Returns <see langword="true"/> when <paramref name="root"/> is the current owner.</summary>
        internal bool IsOwner(SfThemeRoot root)
        {
            return ReferenceEquals(_owner, root);
        }

        private sealed class ProviderScopes
        {
            internal readonly ConditionalWeakTable<Dispatcher, SfThemeScope> _byDispatcher = [];
        }
    }
}
