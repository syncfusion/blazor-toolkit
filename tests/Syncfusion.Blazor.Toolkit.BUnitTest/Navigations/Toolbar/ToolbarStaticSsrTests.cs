#if NET9_0_OR_GREATER
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.HtmlRendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.JSInterop;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    public class ToolbarStaticSsrTests
    {
        private sealed class NoJsRuntime : IJSRuntime
        {
            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
                throw new InvalidOperationException("Static server rendering has no JavaScript runtime: " + identifier);

            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) =>
                throw new InvalidOperationException("Static server rendering has no JavaScript runtime: " + identifier);
        }

        [Theory(DisplayName = "I-TB-013 Static SSR Popup renders the visual overflow affordance at the logical end")]
        [InlineData(false, "ltr")]
        [InlineData(true, "rtl")]
        public async Task Popup_RendersVisualOverflowAffordance(bool enableRtl, string direction)
        {
            string html = await RenderToolbarAsync(enableRtl, showOverflowButton: true);

            Assert.Contains("e-toolbar-ssr-popup-overflow", html, StringComparison.Ordinal);
            Assert.Contains("toolbar-overflow-button", html, StringComparison.Ordinal);
            Assert.Contains("e-toolbar-overflow-btn", html, StringComparison.Ordinal);
            Assert.Contains("dir=\"" + direction + "\"", html, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "I-TB-013 Static SSR Popup honors ShowOverflowButton false")]
        public async Task Popup_HidesDisabledOverflowAffordance()
        {
            string html = await RenderToolbarAsync(enableRtl: false, showOverflowButton: false);

            Assert.DoesNotContain("e-toolbar-ssr-popup-overflow", html, StringComparison.Ordinal);
            Assert.DoesNotContain("toolbar-overflow-button", html, StringComparison.Ordinal);
        }

        private static async Task<string> RenderToolbarAsync(bool enableRtl, bool showOverflowButton)
        {
            ServiceCollection services = new();
            _ = services.AddLogging();
            _ = services.AddSingleton<IJSRuntime>(new NoJsRuntime());
            _ = services.AddSyncfusionBlazorToolkit();
            await using ServiceProvider provider = services.BuildServiceProvider();
            await using HtmlRenderer renderer = new(provider, NullLoggerFactory.Instance);

            return await renderer.Dispatcher.InvokeAsync(async () =>
            {
                HtmlRootComponent root = await renderer.RenderComponentAsync<SfToolbar>(ParameterView.FromDictionary(new Dictionary<string, object?>
                {
                    ["AriaLabel"] = "Static",
                    ["EnableRtl"] = enableRtl,
                    ["OverflowMode"] = ToolbarOverflowMode.Popup,
                    ["OverflowSettings"] = new ToolbarOverflowSettings { ShowOverflowButton = showOverflowButton },
                    ["ChildContent"] = (RenderFragment)(builder => builder.AddContent(0, "Cut"))
                }));
                return root.ToHtmlString();
            });
        }
    }
}
#endif
