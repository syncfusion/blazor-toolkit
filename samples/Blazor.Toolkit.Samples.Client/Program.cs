using Blazor.Toolkit.Samples.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit;
using Syncfusion.Blazor.Toolkit.Popups;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<RenderModeService>();

// Add HttpClient for Release Notes and other HTTP requests
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add Syncfusion Blazor services
builder.Services.AddSyncfusionBlazorToolkit();

// Add localization support in the WASM client
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddSingleton<IStringLocalizer>(sp =>
{
    var factory = sp.GetRequiredService<IStringLocalizerFactory>();
    var assemblyName = typeof(Blazor.Toolkit.Samples.Client.Services.RenderModeService).Assembly.GetName().Name;
    return factory.Create("SfToolkitResources", assemblyName ?? "Blazor.Toolkit.Samples");
});

builder.Services.AddScoped<SfDialogService>();
builder.Services.AddScoped<SampleService>();

var host = builder.Build();

// Read persisted culture from browser storage and set .NET culture before app runs
try
{
    var js = host.Services.GetRequiredService<IJSRuntime>();
    const string defaultCulture = "en";
    string result = string.Empty;
    try
    {
        result = await js.InvokeAsync<string>("blazorCulture.get");
    }
    catch
    {
        
    }

    if (!string.IsNullOrEmpty(result))
    {
        var culture = CultureInfo.GetCultureInfo(result ?? defaultCulture);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}
catch
{
    // ignore if JS interop not available or key missing
}

await host.RunAsync();

