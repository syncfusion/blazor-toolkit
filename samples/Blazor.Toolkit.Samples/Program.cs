using Blazor.Toolkit.Samples.Client.Services;
using Blazor.Toolkit.Samples.Components;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Toolkit;
using Syncfusion.Blazor.Toolkit.Popups;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add Syncfusion Blazor services
builder.Services.AddSyncfusionBlazorToolkit();

// Add localization and resource path
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddSingleton<IStringLocalizer>(sp =>
{
    var factory = sp.GetRequiredService<IStringLocalizerFactory>();
    var assemblyName = "Blazor.Toolkit.Samples.Client";
    return factory.Create("SfToolkitResources", assemblyName);
});

builder.Services.AddScoped<SfDialogService>();
builder.Services.AddScoped<SampleService>();
// Register RenderModeService for components that need to switch render mode
builder.Services.AddScoped<RenderModeService>();
// Make HttpContext available to components during server prerender
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

// Localization: set supported cultures and enable RequestLocalization
var supportedCultures = new[] { "en", "ar-AE", "de-DE", "fr-CH", "zh-CN" }
    .Select(c => new CultureInfo(c)).ToList();

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
// Prefer cookie provider so user's preference persists
localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());

app.UseRequestLocalization(localizationOptions);

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Blazor.Toolkit.Samples.Client._Imports).Assembly);

app.Run();
