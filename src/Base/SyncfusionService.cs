using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Extension method to register Syncfusion Blazor services into the <see href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection">service collection</see>.
    /// </summary>
    public static class SyncfusionBlazor
    {
        /// <summary>
        /// Registers Syncfusion Blazor Toolkit services and optional global configuration.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <param name="configure">Optional action to configure <see cref="GlobalOptions"/>.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSyncfusionBlazorToolkit(this IServiceCollection services, Action<GlobalOptions>? configure = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            services.TryAddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
            _ = services.AddSingleton(sp =>
            {
                IStringLocalizerFactory factory = sp.GetRequiredService<IStringLocalizerFactory>();
                return factory.Create("Base.SfToolkitResources", typeof(SyncfusionBlazor).Assembly.GetName().Name!);
            });
            _ = services.AddScoped<SyncfusionBlazorService>();
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
    /// Represents an instance of Syncfusion Blazor service.
    /// </summary>
    public class SyncfusionBlazorService(IOptions<GlobalOptions>? configure)
    {
        internal readonly GlobalOptions _options = configure?.Value ?? new GlobalOptions();

        /// <summary>
        /// Specifies the init JSInterop script is loaded, when DisableScriptManager is false.
        /// Specifies the IsDevice JSInterop call invoked, when DisableScriptManager is true.
        /// </summary>
        internal bool IsScriptRendered { get; set; }

        /// <summary>
        /// Specifies the application is rendering in device mode.
        /// </summary>
        internal bool IsDeviceMode { get; set; }

        /// <summary>
        /// Specifies the first component rendering in the application.
        /// </summary>
        internal bool IsFirstResource { get; set; } = true;

        /// <summary>
        /// Specifies the first BaseComponent inherited rendering in the application.
        /// </summary>
        internal bool IsFirstBaseResource { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the JavaScript runtime is in-process.
        /// </summary>
        internal bool IsJsInProcess { get; set; }

        /// <summary>
        /// Enable right-to-left text direction to the Syncfusion Blazor Toolkit components.
        /// </summary>
        /// <param name="enable">Set false to disable right-to-left text direction.</param>
        public void EnableRtl(bool enable = true)
        {
            _options.EnableRtl = enable;
        }

        /// <summary>
        /// Determines whether animation is enabled based on global settings.
        /// </summary>
        /// <returns><see langword="true"/> if animation is enabled; otherwise, <see langword="false"/>.</returns>
        public bool IsAnimationEnabled()
        {
            return _options.Animation is GlobalAnimationMode.Default or GlobalAnimationMode.Enable;
        }
    }

    /// <summary>
    /// A class that provides options to configure global settings for Syncfusion Blazor ToolKit components.
    /// </summary>
    public class GlobalOptions
    {
        /// <summary>
        /// Specifies whether the right-to-left (RTL) mode enabled for supported components.
        /// </summary>
        /// <value>
        /// true, if the RTL mode enabled for supported components. The default value is false.
        /// </value>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Gets or sets global options to control animation behaviors for animation supported Blazor Toolkit components.
        /// </summary>
        /// <value>
        /// Default, if the animation works in all the Blazor Toolkit components based on its animation property value. Enable/Disable, to control the animation in all the Blazor Toolkit components. The default value is 'Default'.
        /// </value>
        public GlobalAnimationMode Animation { get; set; } = GlobalAnimationMode.Default;
    }
}
