using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Text.Json;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Base class for all Syncfusion Blazor Toolkit components, centralizing lifecycle logic,
    /// JavaScript interop abstraction, property-change tracking, and shared module imports.
    /// </summary>
    /// <remarks>
    /// Derived components must inherit from <see cref="SfBaseComponent"/> to receive:
    /// <list type="bullet">
    /// <item><description>Automatic property-change tracking via <see cref="NotifyPropertyChanges{T}(string, T, T)"/>.</description></item>
    /// <item><description>Runtime-agnostic JavaScript interop through <see cref="InvokeVoidAsync(IJSObjectReference, IJSInProcessObjectReference, string, object[])"/> and <see cref="InvokeAsync{T}(IJSObjectReference, IJSInProcessObjectReference, string, object[])"/>.</description></item>
    /// <item><description>Lazy loading of shared JS modules (base, animation, draggable, popup, touch).</description></item>
    /// <item><description>Touch-device detection and static-server rendering awareness.</description></item>
    /// </list>
    /// </remarks>
    public abstract class SfBaseComponent : ComponentBase, IAsyncDisposable
    {
        #region internal properties
        /// <summary>
        /// Gets or sets the JavaScript runtime instance injected by the Blazor framework, used for all JS interop calls.
        /// </summary>
        /// <remarks>
        /// <see cref="IsJsInProcess()"/> checks whether this runtime supports synchronous in-process calls.
        /// </remarks>
        /// <exclude />
        [Inject]
        internal IJSRuntime? JSRuntime { get; set; }

        /// <summary>
        /// The scoped Syncfusion service instance that caches global options, device mode, and runtime flags.
        /// </summary>
        /// <exclude />
        [Inject]
        internal SyncfusionBlazorToolkitService? SyncfusionService { get; set; }

        /// <summary>
        /// Asynchronous JavaScript module reference for the shared base script (<c>base.js</c>).
        /// </summary>
        /// <exclude />
        internal IJSObjectReference? _baseJsModule;

        /// <summary>
        /// In-process JavaScript module reference for the shared base script (<c>base.js</c>).
        /// </summary>
        /// <exclude />
        internal IJSInProcessObjectReference? _baseJsInProcessModule;

        /// <summary>
        /// Asynchronous JavaScript module reference for the animation script (<c>animation.js</c>).
        /// </summary>
        /// <exclude />
        internal IJSObjectReference? _animationJsModule;

        /// <summary>
        /// In-process JavaScript module reference for the animation script (<c>animation.js</c>).
        /// </summary>
        /// <exclude />
        internal IJSInProcessObjectReference? _animationJsInProcessModule;

        /// <summary>
        /// Asynchronous JavaScript module reference for the popup script (<c>popup.js</c>).
        /// </summary>
        /// <exclude />
        internal IJSObjectReference? _popupJsModule;

        /// <summary>
        /// In-process JavaScript module reference for the popup script (<c>popup.js</c>).
        /// </summary>
        /// <exclude />
        internal IJSInProcessObjectReference? _popupInProcessModule;

        /// <summary>
        /// Asynchronous JavaScript module reference for the touch script (<c>touch.js</c>).
        /// </summary>
        /// <exclude />
        internal IJSObjectReference? _touchJsModule;

        /// <summary>
        /// In-process JavaScript module reference for the touch script (<c>touch.js</c>).
        /// </summary>
        /// <exclude />
        internal IJSInProcessObjectReference? _touchInProcessModule;

        /// <summary>
        /// Gets or sets a value indicating whether the component has been rendered at least once.
        /// </summary>
        /// <value>
        /// <see langword="true"/> after <see cref="OnAfterRenderAsync(bool)"/> is called with <paramref name="firstRender"/> as <see langword="true"/>;
        /// otherwise <see langword="false"/>.
        /// </value>
        /// <exclude />
        internal bool IsRendered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the component is currently being rendered in static
        /// server (prerender) mode.
        /// </summary>
        /// <value>
        /// <see langword="true"/> when the renderer reports a non-interactive static context;
        /// otherwise <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// Set by <see cref="IsStaticServerRendering()"/> during component initialization.
        /// </remarks>
        /// <exclude />
        internal bool StaticServerRendering { get; set; }

        /// <summary>
        /// A dictionary that tracks changed property names and their new values across parameter sets.
        /// </summary>
        /// <value>
        /// A dictionary mapping property names to their latest values. Cleared at the end of every render cycle in <see cref="OnAfterRenderAsync(bool)"/>.
        /// </value>
        /// <remarks>
        /// <para>Derived components record property changes during <see cref="OnParametersSetAsync"/> by calling <see cref="NotifyPropertyChanges{T}(string, T, T)"/>. The count of entries can be used to determine whether a UI refresh is required.</para>
        /// <para>This dictionary is instantiated in <see cref="OnInitializedAsync"/> and cleared after every render so that each parameter cycle starts fresh.</para>
        /// </remarks>
        /// <exclude />
        internal Dictionary<string, object>? PropertyChanges { get; set; }

        /// <summary>
        /// A bridge reference passed to JavaScript so the client can invoke .NET instance methods on this component.
        /// </summary>
        /// <remarks>
        /// Created during <see cref="OnAfterRenderAsync(bool)"/> on the first render and disposed in <see cref="DisposeAsync()"/>.
        /// </remarks>
        /// <exclude />

        internal DotNetObjectReference<object>? DotnetObjectReference { get; set; }

        /// <summary>
        /// Gets a value indicating whether the component and its associated JavaScript resources have been disposed.
        /// </summary>
        /// <value>
        /// <see langword="true"/> after <see cref="DisposeAsync"/> has started; otherwise <see langword="false"/>.
        /// </value>
        /// <exclude />
        protected bool IsDisposed { get; set; }

        #endregion

        #region life cycle methods

        /// <exclude />
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            PropertyChanges = [];
        }

        /// <exclude />
        /// <summary>
        /// Called after the component has finished rendering. When <paramref name="firstRender"/>
        /// is true, <see cref="OnAfterScriptRenderedAsync"/> is invoked for first-time initialization.
        /// </summary>
        /// <param name="firstRender">True on the initial render; otherwise false.</param>
        /// <remarks>
        /// <para>The base implementation creates a <see cref="DotNetObjectReference"/> bridging this component to JavaScript callbacks, sets <see cref="IsRendered"/>, imports the shared base script module via <see cref="ImportComponentModuleAsync"/>, and then raises <see cref="OnAfterScriptRenderedAsync"/>.</para>
        /// <para>On every call (not just first render), the <see cref="PropertyChanges"/> dictionary is cleared so that parameter-tracking starts fresh for the next cycle.</para>
        /// </remarks>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            if (firstRender)
            {
                DotnetObjectReference = DotNetObjectReference.Create<object>(this);
                IsRendered = firstRender;

                await ImportComponentModuleAsync().ConfigureAwait(true);

                // Notify the component that the required scripts have been loaded.
                await OnAfterScriptRenderedAsync().ConfigureAwait(true);
            }
            PropertyChanges?.Clear();
        }

        /// <summary>
        /// Releases unmanaged resources, JavaScript module references, and DotNet object references for this component.
        /// </summary>
        /// <remarks>
        /// <para>The base implementation clears <see cref="PropertyChanges"/>, disposes each imported JS module (<see cref="_baseJsModule"/>, <see cref="_animationJsModule"/>, <see cref="_draggableJsModule"/>, <see cref="_popupJsModule"/>, <see cref="_touchJsModule"/>), and disposes the <see cref="DotnetObjectReference"/> bridge.</para>
        /// <para>A <see cref="JSDisconnectedException"/> is caught and ignored because the circuit may disconnect (page reload) before JS disposal completes.</para>
        /// <para>Derived components that hold additional disposable resources should override <see cref="DisposeAsyncCore"/> rather than replacing this method.</para>
        /// </remarks>
        public async ValueTask DisposeAsync()
        {
            PropertyChanges?.Clear();
            IsDisposed = true;
            await DisposeAsyncCore().ConfigureAwait(true);
            try
            {
                if (_baseJsModule is not null)
                {
                    await _baseJsModule.DisposeAsync().ConfigureAwait(true);
                }
                if (_animationJsModule is not null)
                {
                    await _animationJsModule.DisposeAsync().ConfigureAwait(true);
                }
                if (_popupJsModule is not null)
                {
                    await _popupJsModule.DisposeAsync().ConfigureAwait(true);
                }
                if (_touchJsModule is not null)
                {
                    await _touchJsModule.DisposeAsync().ConfigureAwait(true);
                }
                _baseJsInProcessModule?.Dispose();
                _animationJsInProcessModule?.Dispose();
                _popupInProcessModule?.Dispose();
                _touchInProcessModule?.Dispose();
            }
            catch (JSDisconnectedException)
            {
                // Ignore: The circuit disconnected (e.g., page reload) before JS disposal could complete.
            }
            DotnetObjectReference?.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Determines whether the component is being rendered in static server (pre-render) mode.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> when the current renderer reports the name <c>"Static"</c>, the renderer
        /// is not interactive, and no explicit render mode was assigned; otherwise <see langword="false"/>.
        /// </returns>
        /// <value>
        /// The cached result is also stored in <see cref="StaticServerRendering"/>.
        /// </value>
        /// <remarks>
        /// The runtime check used here is only compiled for .NET 9 and later (<c>NET9_0_OR_GREATER</c>).
        /// On earlier target frameworks this method always returns <see langword="false"/> because the
        /// platform-specific renderer information is not available.
        /// </remarks>
        internal bool IsStaticServerRendering()
        {
            StaticServerRendering = false;
#if NET9_0_OR_GREATER
            StaticServerRendering = RendererInfo.Name.Equals("Static", StringComparison.Ordinal) &&
                !RendererInfo.IsInteractive && AssignedRenderMode is null;
#endif
            return StaticServerRendering;
        }

        /// <summary>
        /// Determines whether the JavaScript runtime supports synchronous in-process calls.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if <see cref="JSRuntime"/> is an instance of <see cref="IJSInProcessRuntime"/>
        /// (Blazor WebAssembly) and the current context is not a bUnit test; otherwise <see langword="false"/>
        /// for async-only interop (Blazor Server).
        /// </returns>
        /// <remarks>
        /// In Blazor WebAssembly, <see cref="IJSInProcessRuntime"/> allows synchronous calls, which avoids
        /// serialization overhead and is more efficient. In Blazor Server, only asynchronous interop
        /// (<see cref="IJSObjectReference"/>) is available. The result is cached in
        /// <see cref="SyncfusionService.IsJsInProcess"/> during component initialization for performance.
        /// </remarks>
        internal bool IsJsInProcess()
        {
            // Returns true only for a real in-process JS runtime; returns false under bUnit tests since IJSInProcessObjectReference js import is not supported in bunit
            return !JSRuntime!.GetType().FullName!.Contains("Bunit", StringComparison.Ordinal) && JSRuntime is IJSInProcessRuntime;
        }

        /// <summary>
        /// Invokes a JavaScript interop method that does not return a value, choosing between
        /// synchronous (in-process) and asynchronous (async) invocation based on the runtime type.
        /// </summary>
        /// <param name="jsObjectReference">
        /// The async JavaScript module reference (<see cref="IJSObjectReference"/>) used for
        /// asynchronous interop calls in Blazor Server mode. Can be <c>null</c> if in-process
        /// interop is available.
        /// </param>
        /// <param name="jsInProcessObjectReference">
        /// The in-process JavaScript module reference (<see cref="IJSInProcessObjectReference"/>) used for
        /// synchronous interop calls in Blazor WebAssembly mode. Can be <c>null</c> if only
        /// async interop is available.
        /// </param>
        /// <param name="identifier">The identifier of the JavaScript method to invoke (e.g., "methodName").</param>
        /// <param name="args">Parameters to pass to the JavaScript method.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// This method provides a unified interface for invoking JavaScript methods regardless of the hosting mode.
        /// It automatically selects the appropriate module reference based on availability:
        /// - If <paramref name="jsObjectReference"/> is <c>null</c>, the synchronous in-process module is used.
        /// - Otherwise, the asynchronous module is used for interop.
        ///
        /// <see cref="JSDisconnectedException"/> is caught and safely ignored, as it is expected during
        /// prerendering or page refresh scenarios.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a <see cref="JSException"/> occurs during the interop call, wrapping the original exception
        /// with contextual information.
        /// </exception>
        /// <example>
        /// <code><![CDATA[
        /// // Invoke a void JavaScript method using the appropriate module
        /// await InvokeVoidAsync(_componentModule, _componentInProcessModule,
        ///     "sfBlazorToolkit.component.initialize", componentElement, options);
        /// ]]></code>
        /// </example>
        /// <exclude />
        internal static async Task InvokeVoidAsync(IJSObjectReference? jsObjectReference, IJSInProcessObjectReference? jsInProcessObjectReference, string identifier, params object[] args)
        {
            try
            {
                if (jsInProcessObjectReference is not null)
                {
                    jsInProcessObjectReference.InvokeVoid(identifier, args);
                }
                else if (jsObjectReference is not null)
                {
                    await jsObjectReference.InvokeVoidAsync(identifier, args).ConfigureAwait(true);
                }
                else
                {
                    return;
                }
            }
            catch (JSException e)
            {
                throw new InvalidOperationException("Unhandled exception occurred during interop call. ", e);
            }
            catch (JSDisconnectedException)
            {
                // Expected during prerendering, or page refresh; safe to ignore
            }
            catch (ObjectDisposedException)
            {
                // Add this to safely ignore calls on dead JS references
            }
        }


        /// <summary>
        /// Invokes a JavaScript interop method and returns a result of type <typeparamref name="T"/>.
        /// This overload is async-safe by default and is suitable for invoking JavaScript methods that
        /// are asynchronous or return Promises.
        /// </summary>
        /// <typeparam name="T">
        /// The expected CLR type to which the JavaScript return value will be deserialized.
        /// </typeparam>
        /// <param name="jsObjectReference">
        /// The asynchronous JavaScript module reference (<see cref="IJSObjectReference"/>) used for
        /// interop calls in Blazor Server or when in-process invocation is not available.
        /// </param>
        /// <param name="jsInProcessObjectReference">
        /// The in-process JavaScript module reference (<see cref="IJSInProcessObjectReference"/>) used in
        /// Blazor WebAssembly when synchronous or in-process asynchronous interop is supported.
        /// </param>
        /// <param name="identifier">
        /// The identifier of the JavaScript function to invoke (for example, <c>"someObject.someMethod"</c>).
        /// </param>
        /// <param name="args">
        /// Arguments to pass to the JavaScript function. These must be JSON-serializable.
        /// </param>
        /// <returns>
        /// A task that resolves to the result of the JavaScript function, deserialized into
        /// <typeparamref name="T"/>.  
        /// If a <see cref="JSDisconnectedException"/> occurs (for example, during prerendering,
        /// page navigation, or application shutdown), the default value of <typeparamref name="T"/> is returned.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This overload defaults to asynchronous JavaScript invocation and is safe for:
        /// </para>
        /// <list type="bullet">
        /// <item><description>JavaScript functions declared with <c>async</c></description></item>
        /// <item><description>Functions that return Promises</description></item>
        /// <item><description>Browser APIs that perform asynchronous work (e.g., <c>fetch</c>, <c>document.fonts.load</c>)</description></item>
        /// </list>
        /// <para>
        /// Internally, this method delegates to the overload that accepts an <c>isSynchronous</c> flag
        /// and explicitly disables synchronous invocation to prevent Promise deserialization errors
        /// in Blazor WebAssembly.
        /// </para>
        /// <para>
        /// Any <see cref="JSException"/> thrown during invocation is wrapped in an
        /// <see cref="InvalidOperationException"/> to provide additional diagnostic context.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a <see cref="JSException"/> occurs during the JavaScript interop call.
        /// </exception>
        /// <example>
        /// <code><![CDATA[
        /// // Invoke an async JavaScript method safely
        /// DomRect bounds = await InvokeAsync<DomRect>(
        ///     _componentModule,
        ///     _componentInProcessModule,
        ///     "getElementBounds",
        ///     elementId);
        /// ]]></code>
        /// </example>
        /// <exclude />
        internal static async Task<T> InvokeAsync<T>(IJSObjectReference jsObjectReference, IJSInProcessObjectReference jsInProcessObjectReference, string identifier, params object[] args)
        {
            return await InvokeAsync<T>(jsObjectReference, jsInProcessObjectReference, identifier, isSynchronous: false, args).ConfigureAwait(true);
        }

        /// <summary>
        /// Invokes a JavaScript interop method and returns a result of type <typeparamref name="T"/>,
        /// using either synchronous (in-process) or asynchronous invocation based on the
        /// <paramref name="isSynchronous"/> flag.
        /// </summary>
        /// <typeparam name="T">
        /// The expected CLR type to which the JavaScript return value will be deserialized.
        /// </typeparam>
        /// <param name="jsObjectReference">
        /// The asynchronous JavaScript module reference (<see cref="IJSObjectReference"/>) used for
        /// interop calls in Blazor Server or when synchronous in-process invocation is not available.
        /// </param>
        /// <param name="jsInProcessObjectReference">
        /// The in-process JavaScript module reference (<see cref="IJSInProcessObjectReference"/>) used
        /// in Blazor WebAssembly for synchronous or in-process asynchronous interop.
        /// </param>
        /// <param name="identifier">
        /// The identifier of the JavaScript function to invoke (for example, <c>"someObject.someMethod"</c>).
        /// </param>
        /// <param name="isSynchronous">
        /// A value indicating whether to invoke the JavaScript function synchronously using
        /// <see cref="IJSInProcessObjectReference.Invoke{TValue}(string, object?[])"/>.
        /// <para>
        /// This parameter must be set to <c>true</c> <strong>only</strong> for JavaScript functions
        /// that are fully synchronous and do not return a Promise.
        /// </para>
        /// <para>
        /// When <c>false</c>, the invocation is performed asynchronously using
        /// <see cref="IJSObjectReference.InvokeAsync{TValue}(string, object?[])"/> or its in-process
        /// equivalent, ensuring compatibility with async JavaScript and Promise-based APIs.
        /// </para>
        /// </param>
        /// <param name="args">
        /// Arguments to pass to the JavaScript function. These must be JSON-serializable.
        /// </param>
        /// <returns>
        /// A task that resolves to the result of the JavaScript function, deserialized into
        /// <typeparamref name="T"/>.
        /// <para>
        /// If a <see cref="JSDisconnectedException"/> occurs (for example, during prerendering,
        /// navigation, or application shutdown), the default value of <typeparamref name="T"/> is returned.
        /// </para>
        /// </returns>
        /// <remarks>
        /// <para>
        /// This overload provides explicit control over the JavaScript invocation mode.
        /// Incorrectly specifying <paramref name="isSynchronous"/> as <c>true</c> for a JavaScript
        /// function that is asynchronous or returns a Promise can result in runtime errors,
        /// particularly in Blazor WebAssembly.
        /// </para>
        /// <para>
        /// For general use and maximum safety, prefer calling the overload that does not accept
        /// the <paramref name="isSynchronous"/> parameter, which defaults to asynchronous invocation.
        /// </para>
        /// <para>
        /// Any <see cref="JSException"/> thrown during invocation is wrapped in an
        /// <see cref="InvalidOperationException"/> to provide additional diagnostic context.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a <see cref="JSException"/> occurs during the JavaScript interop call.
        /// </exception>
        /// <example>
        /// <code><![CDATA[
        /// // Explicitly invoke synchronous JavaScript (no Promise, no async/await)
        /// int sum = await InvokeAsync<int>(
        ///     _componentModule,
        ///     _componentInProcessModule,
        ///     "add",
        ///     isSynchronous: true,
        ///     5, 6);
        ///
        /// // Explicitly invoke async JavaScript
        /// string result = await InvokeAsync<string>(
        ///     _componentModule,
        ///     _componentInProcessModule,
        ///     "getCharCollectionSize",
        ///     isSynchronous: false,
        ///     fontKeys);
        /// ]]></code>
        /// </example>
        internal static async Task<T> InvokeAsync<T>(IJSObjectReference jsObjectReference, IJSInProcessObjectReference jsInProcessObjectReference, string identifier, bool isSynchronous, params object[] args)
        {
            try
            {
                return jsObjectReference is null
                    ? !isSynchronous ? await jsInProcessObjectReference.InvokeAsync<T>(identifier, args).ConfigureAwait(true)
                        : jsInProcessObjectReference.Invoke<T>(identifier, args)
                    : await jsObjectReference.InvokeAsync<T>(identifier, args).ConfigureAwait(true);
            }
            catch (JSException e)
            {
                throw new InvalidOperationException("Unhandled exception occurred during interop call. ", e);
            }
            catch (JSDisconnectedException)
            {
                return default!;
            }
        }

        /// <summary>
        /// Records a property change when the public value differs from the private value.
        /// </summary>
        /// <typeparam name="T">The type of the property value being compared and recorded.</typeparam>
        /// <param name="propertyName">The name of the property to track.</param>
        /// <param name="publicValue">The new (public) value to compare and record.</param>
        /// <param name="privateValue">The previous (private) value to compare against.</param>
        /// <returns>The <paramref name="publicValue"/> (for convenience in setters).</returns>
        /// <exclude />
        internal T NotifyPropertyChanges<T>(string propertyName, T publicValue, T privateValue)
        {
            if (!SfBaseUtils.Equals(publicValue, privateValue))
            {
                _ = SfBaseUtils.UpdateDictionary(propertyName, publicValue!, PropertyChanges);
            }
            return publicValue;
        }

        /// <summary>
        /// Disposes a window.sfBlazor instance on the client by invoking the corresponding JavaScript interop method.
        /// </summary>
        /// <param name="id">The identifier of the window.sfBlazor instance to dispose.</param>
        /// <exclude />
        internal async Task WindowInstanceDisposeAsync(string id)
        {
            await InvokeVoidAsync(_baseJsModule, _baseJsInProcessModule, "disposeWindowsInstance", id).ConfigureAwait(true);
        }

        /// <exclude />
        protected virtual ValueTask DisposeAsyncCore()
        {
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// Ensures the toolkit's base JavaScript module is available for the component and
        /// updates the cached runtime mode on <see cref="SyncfusionService"/>.
        /// </summary>
        /// <remarks>
        /// This virtual method is invoked during the component's first render. The base
        /// implementation sets <see cref="SyncfusionService.IsJsInProcess"/> by calling
        /// <see cref="IsJsInProcess"/>, then imports the shared base script via
        /// <see cref="ImportModuleAsync(string, IJSObjectReference?, IJSInProcessObjectReference?)"/>.
        /// The imported references are assigned to the internal fields `_baseJsModule` and
        /// `_baseJsInProcessModule` so derived components can use them or import additional
        /// component-specific modules. Derived overrides should call the base implementation
        /// before importing their own modules.
        /// </remarks>
        /// <returns>A task representing the completion of the import and assignment work.</returns>
        /// <exclude />
        internal virtual async Task ImportComponentModuleAsync()
        {
            SyncfusionService!.IsJsInProcess = IsJsInProcess();
            JsModuleReference baseJSModuleReference = await ImportModuleAsync("./_content/Syncfusion.Blazor.Toolkit/scripts/base.js", _baseJsModule, _baseJsInProcessModule).ConfigureAwait(true);
            _baseJsModule = baseJSModuleReference.AsyncRef;
            _baseJsInProcessModule = baseJSModuleReference.InProcessRef;
        }

        /// <summary>
        /// Called after required client scripts have been rendered; override to perform
        /// initialization that depends on those scripts.
        /// </summary>
        /// <exclude />
        internal virtual Task OnAfterScriptRenderedAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Detects whether the current client is a touch-capable device and caches the result on <see cref="SyncfusionService"/>.
        /// </summary>
        /// <remarks>
        /// <para>The check runs only once per application lifetime by checking <see cref="SyncfusionBlazorService.IsFirstResource"/>.</para>
        /// <para>It invokes the <c>isDevice</c> JavaScript interop method through <see cref="InvokeAsync{T}(IJSObjectReference, IJSInProcessObjectReference, string, object[])"/> and stores the result in <see cref="SyncfusionBlazorService.IsDeviceMode"/>.</para>
        /// </remarks>
        /// <returns>A task representing the asynchronous device-detection operation.</returns>
        /// <exclude />
        internal async Task UpdateIsDeviceModeAsync()
        {
            if (SyncfusionService is not null && SyncfusionService.IsFirstResource)
            {
                SyncfusionService.IsFirstResource = false;
                DeviceMode deviceMode = await InvokeAsync<DeviceMode>(_baseJsModule!, _baseJsInProcessModule!, "isDevice").ConfigureAwait(false);
                if (deviceMode is not null)
                {
                    SyncfusionService.IsDeviceMode = deviceMode.IsDevice;
                }
            }
        }

        /// <exclude />
        internal class JsModuleReference
        {
            /// <summary>
            /// Gets or sets the async JavaScript module reference.
            /// </summary>
            /// <exclude />
            public IJSObjectReference? AsyncRef { get; set; }

            /// <summary>
            /// Gets or sets the in-process JavaScript module reference.
            /// </summary>
            /// <exclude />
            public IJSInProcessObjectReference? InProcessRef { get; set; }
        }

        /// <summary>
        /// Imports a JavaScript module and returns a <see cref="JsModuleReference"/>
        /// optimized for the current Blazor runtime (Server or WebAssembly).
        /// </summary>
        /// <param name="scriptPath">The path to the JavaScript module to import.</param>
        /// <param name="asyncRef">An existing <see cref="IJSObjectReference"/> module reference to return if already initialized.</param>
        /// <param name="inProcessRef">An existing <see cref="IJSInProcessObjectReference"/> module reference to return if already initialized.</param>
        /// <returns>
        /// A task representing the import operation, containing the initialized
        /// <see cref="JsModuleReference"/>.
        /// </returns>
        /// <exclude />
        internal async Task<JsModuleReference> ImportModuleAsync(string scriptPath, IJSObjectReference? asyncRef, IJSInProcessObjectReference? inProcessRef)
        {
            JsModuleReference result = new();
            if (SyncfusionService is null || JSRuntime is null || !(asyncRef is null && inProcessRef is null))
            {
                return new JsModuleReference { AsyncRef = asyncRef, InProcessRef = inProcessRef };
            }
            if (SyncfusionService.IsJsInProcess)
            {
                result.InProcessRef = await JSRuntime.InvokeAsync<IJSInProcessObjectReference>("import", scriptPath).ConfigureAwait(true);
            }
            else
            {
                result.AsyncRef = await JSRuntime.InvokeAsync<IJSObjectReference>("import", scriptPath).ConfigureAwait(true);
            }
            return result;
        }

        /// <summary>
        /// Lazily loads the animation JavaScript module and assigns the returned
        /// async and in-process references to `_animationJsModule` and
        /// `_animationJsInProcessModule` respectively.
        /// </summary>
        /// <remarks>
        /// The implementation delegates the import to <see cref="ImportModuleAsync(string, IJSObjectReference?, IJSInProcessObjectReference?)"/>
        /// which handles early-return conditions (missing service/runtime or already-initialized refs)
        /// and selects the proper import mechanism based on the runtime mode.
        /// </remarks>
        /// <returns>A task representing the completion of the import and assignment.</returns>
        /// <exclude />
        internal async Task LoadAnimationScriptAsync()
        {
            JsModuleReference animationJsModuleReference = await ImportModuleAsync("./_content/Syncfusion.Blazor.Toolkit/scripts/animation.js", _animationJsModule, _animationJsInProcessModule).ConfigureAwait(true);
            _animationJsModule = animationJsModuleReference.AsyncRef;
            _animationJsInProcessModule = animationJsModuleReference.InProcessRef;
        }

        /// <summary>
        /// Lazily loads the popup JavaScript module and assigns the returned
        /// async and in-process references to `_popupJsModule` and `_popupInProcessModule` respectively.
        /// </summary>
        /// <remarks>
        /// The method calls <see cref="ImportModuleAsync(string, IJSObjectReference?, IJSInProcessObjectReference?)"/>
        /// to perform the import; that helper manages the runtime-specific import logic
        /// and returns existing references when available.
        /// </remarks>
        /// <returns>A task representing the completion of the import and assignment.</returns>
        /// <exclude />
        internal async Task LoadPopupScriptAsync()
        {
            JsModuleReference popupJsModuleReference = await ImportModuleAsync("./_content/Syncfusion.Blazor.Toolkit/scripts/popup.js", _popupJsModule, _popupInProcessModule).ConfigureAwait(true);
            _popupJsModule = popupJsModuleReference.AsyncRef;
            _popupInProcessModule = popupJsModuleReference.InProcessRef;
        }

        /// <summary>
        /// Lazily loads the touch JavaScript module and assigns the returned
        /// async and in-process references to `_touchJsModule` and `_touchInProcessModule` respectively.
        /// </summary>
        /// <remarks>
        /// The method delegates to <see cref="ImportModuleAsync(string, IJSObjectReference?, IJSInProcessObjectReference?)"/>
        /// which takes care of choosing the correct import mechanism and returning
        /// existing references when the module has already been initialized or when
        /// the runtime/service are not available.
        /// </remarks>
        /// <returns>A task representing the completion of the import and assignment.</returns>
        /// <exclude />
        internal async Task LoadTouchScriptAsync()
        {
            JsModuleReference touchJsModuleReference = await ImportModuleAsync("./_content/Syncfusion.Blazor.Toolkit/scripts/touch.js", _touchJsModule, _touchInProcessModule).ConfigureAwait(true);
            _touchJsModule = touchJsModuleReference.AsyncRef;
            _touchInProcessModule = touchJsModuleReference.InProcessRef;
        }
    }

    /// <summary>
    /// Represents the device-type information returned by the JavaScript <c>isDevice</c> interop call.
    /// </summary>
    /// <exclude />
    internal class DeviceMode
    {
        /// <summary>
        /// Gets or sets a value indicating whether the client reports itself as a touch-capable (mobile or tablet) device.
        /// </summary>
        /// <exclude />
        public bool IsDevice { get; set; }
    }
}
