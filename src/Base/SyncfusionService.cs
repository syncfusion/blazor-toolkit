using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Extension methods for registering Syncfusion Blazor Toolkit services into the
    /// dependency injection container.
    /// </summary>
    public static class SyncfusionBlazorToolkit
    {
        /// <summary>
        /// Registers Syncfusion Blazor Toolkit services into the dependency injection container,
        /// including localization, the <see cref="SyncfusionBlazorToolkitService"/>, and optional
        /// global configuration.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configure">Optional action to configure <see cref="GlobalOptions"/>.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        /// <remarks>
        /// This method is intended to be called in <c>Program.cs</c> or <c>Startup.cs</c> during
        /// application initialization. It registers <see cref="SyncfusionBlazorToolkitService"/> as
        /// a scoped service and sets up the <see cref="IStringLocalizerFactory"/> for component
        /// localization.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var builder = WebApplication.CreateBuilder(args);
        /// builder.Services.AddSyncfusionBlazorToolkit(options =>
        /// {
        ///     options.EnableRtl = true;
        ///     options.Animation = GlobalAnimationMode.Enable;
        /// });
        /// ]]></code>
        /// </example>
        public static IServiceCollection AddSyncfusionBlazorToolkit(this IServiceCollection services, Action<GlobalOptions>? configure = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            services.TryAddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
            _ = services.AddSingleton(sp =>
            {
                IStringLocalizerFactory factory = sp.GetRequiredService<IStringLocalizerFactory>();
                return factory.Create("Base.SfToolkitResources", typeof(SyncfusionBlazorToolkit).Assembly.GetName().Name!);
            });
            _ = services.AddScoped<SyncfusionBlazorToolkitService>();
            if (configure is not null)
            {
                _ = services.Configure(configure);
            }
            if (!services.Any(s => s.ServiceType == typeof(HttpClient)))
            {
                services.AddScoped<HttpClient>();
            }
            return services;
        }
    }

    /// <summary>
    /// Provides global runtime state and configuration for Syncfusion Blazor Toolkit components,
    /// including RTL settings, animation preferences, and device-mode detection caching.
    /// </summary>
    /// <remarks>
    /// Registered as a scoped service during application startup by
    /// <see cref="SyncfusionBlazorToolkit.AddSyncfusionBlazorToolkit(IServiceCollection, Action{GlobalOptions}?)"/>.
    /// Derived components access this service to read global options and share one-time
    /// initialization state (for example, the in-process JavaScript runtime flag).
    /// </remarks>
    public class SyncfusionBlazorToolkitService(IOptions<GlobalOptions>? configure)
    {
        /// <exclude />
        internal readonly GlobalOptions _options = configure?.Value ?? new GlobalOptions();

        /// <summary>
        /// Gets or sets a value indicating whether the initialization JavaScript interop script
        /// has been rendered or the device-detection call has been invoked.
        /// </summary>
        /// <value>
        /// <see langword="true"/> when the initial script or device interop call has completed;
        /// otherwise <see langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// When the script manager is disabled, this tracks whether the <c>IsDevice</c> interop call
        /// has been made instead of the normal script-rendering path.
        /// </remarks>
        /// <exclude />
        internal bool IsScriptRendered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current client is detected as a touch-capable
        /// (mobile or tablet) device.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the client reports itself as a touch-capable device;
        /// otherwise <see langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// This value is set by calling the JavaScript <c>isDevice</c> interop method during the
        /// first component render and is cached so that subsequent components reuse the result.
        /// </remarks>
        /// <exclude />
        internal bool IsDeviceMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the first component to render in the
        /// current application lifecycle.
        /// </summary>
        /// <value>
        /// <see langword="true"/> until the first component finishes rendering; afterwards
        /// <see langword="false"/>. The default is <see langword="true"/>.
        /// </value>
        /// <remarks>
        /// Used to coordinate one-time initialization logic (for example, device-mode detection)
        /// that should execute only once per application.
        /// </remarks>
        /// <exclude />
        internal bool IsFirstResource { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether this is the first component inheriting
        /// from <see cref="SfBaseComponent"/> to render in the current application lifecycle.
        /// </summary>
        /// <value>
        /// <see langword="true"/> until the first <see cref="SfBaseComponent"/> finishes rendering;
        /// afterwards <see langword="false"/>. The default is <see langword="true"/>.
        /// </value>
        /// <remarks>
        /// Used to coordinate one-time initialization logic specific to the base component class
        /// (for example, importing the shared base JavaScript module).
        /// </remarks>
        /// <exclude />
        internal bool IsFirstBaseResource { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the JavaScript runtime is in-process.
        /// (Blazor WebAssembly) rather than out-of-process (Blazor Server).
        /// </summary>
        /// <value>
        /// <see langword="true"/> when the runtime supports synchronous JavaScript interop;
        /// otherwise <see langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// This value is cached by <see cref="SfBaseComponent.ImportComponentModuleAsync"/>
        /// during the first component render to avoid repeated type checks.
        /// In-process interop is available only in Blazor WebAssembly and is more efficient
        /// because it avoids serialization overhead.
        /// </remarks>
        /// <exclude />
        internal bool IsJsInProcess { get; set; }

        /// <summary>
        /// Enables or disables right-to-left (RTL) text direction for all supported Syncfusion
        /// Blazor Toolkit components in this application scope.
        /// </summary>
        /// <param name="enable">
        /// <see langword="true"/> to enable RTL layout; <see langword="false"/> to disable it.
        /// The default is <see langword="true"/>.
        /// </param>
        /// <remarks>
        /// This method modifies the global options instance. It is equivalent to setting
        /// <see cref="GlobalOptions.EnableRtl"/> directly. The change takes effect for components
        /// rendered after the call.
        /// </remarks>
        /// <returns>None. This method modifies global state directly without returning a value.</returns>
        public void EnableRtl(bool enable = true)
        {
            _options.EnableRtl = enable;
        }

        /// <summary>
        /// Determines whether component animations are enabled globally based on the current
        /// <see cref="GlobalOptions.Animation"/> setting.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if animation is enabled (either <see cref="GlobalAnimationMode.Default"/> or
        /// <see cref="GlobalAnimationMode.Enable"/>); <see langword="false"/> if disabled.
        /// </returns>
        /// <remarks>
        /// Individual components reference this method before running animation transitions so that
        /// a single global switch can turn animations on or off everywhere.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// if (Service.IsAnimationEnabled())
        /// {
        ///     await AnimateAsync();
        /// }
        /// ]]></code>
        /// </example>
        public bool IsAnimationEnabled()
        {
            return _options.Animation is GlobalAnimationMode.Default or GlobalAnimationMode.Enable;
        }
    }

    /// <summary>
    /// Provides global configuration options for Syncfusion Blazor Toolkit components,
    /// including RTL support and animation behavior.
    /// </summary>
    /// <remarks>
    /// These options are configured once at application startup via the
    /// <see cref="SyncfusionBlazorToolkit.AddSyncfusionBlazorToolkit(IServiceCollection, Action{GlobalOptions}?)"/>
    /// extension method and then surfaced read-only to components through
    /// <see cref="SyncfusionBlazorToolkitService"/>.
    /// </remarks>
    /// <exclude />
    public class GlobalOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether right-to-left (RTL) text direction is enabled
        /// for supported components.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if RTL layout is active; otherwise <see langword="false"/>.
        /// The default is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// When enabled, components render their content in right-to-left order.
        /// This setting is typically configured once at application startup.
        /// </remarks>
        /// <exclude />
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Gets or sets global options to control animation behaviors for animation supported Blazor Toolkit components.
        /// </summary>
        /// <value>
        /// Default, if the animation works in all the Blazor Toolkit components based on its animation property value. Enable/Disable, to control the animation in all the Blazor Toolkit components. The default value is 'Default'.
        /// </value>
        /// <exclude />
        public GlobalAnimationMode Animation { get; set; } = GlobalAnimationMode.Default;
    }
}
