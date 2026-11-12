using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.JSInterop;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// Render-mode behavior that bUnit can check without a browser: the static server renderer, state across a
    /// parent re-render, and which components re-render when one item changes.
    /// A real prerender-to-interactive handover needs a browser and is covered by the Playwright render-mode tests.
    /// </summary>
    public class ToolbarRenderModeTests : ToolbarTestBase
    {
        private sealed class NoJsRuntime : IJSRuntime
        {
            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
                throw new InvalidOperationException("Static server rendering has no JavaScript runtime: " + identifier);

            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) =>
                throw new InvalidOperationException("Static server rendering has no JavaScript runtime: " + identifier);
        }

        [Fact(DisplayName = "I-TB-013 Static server rendering emits item markup without touching JavaScript or showing overflow")]
        public async Task StaticSsr_RendersMarkupOnly()
        {
            ServiceCollection services = new();
            _ = services.AddLogging();
            _ = services.AddSingleton<IJSRuntime>(new NoJsRuntime());
            _ = services.AddSyncfusionBlazorToolkit();
            await using ServiceProvider provider = services.BuildServiceProvider();
            await using HtmlRenderer renderer = new(provider, NullLoggerFactory.Instance);

            string html = await renderer.Dispatcher.InvokeAsync(async () =>
            {
                Microsoft.AspNetCore.Components.Web.HtmlRendering.HtmlRootComponent root = await renderer.RenderComponentAsync<SfToolbar>(ParameterView.FromDictionary(new Dictionary<string, object?>
                {
                    ["AriaLabel"] = "Static",
                    ["ChildContent"] = Compose(Item("Cut", ("Id", "cut")), Item("Copy", ("Id", "copy")))
                }));
                return root.ToHtmlString();
            });

            Assert.Contains("role=\"toolbar\"", html, StringComparison.Ordinal);
            Assert.Contains("Cut", html, StringComparison.Ordinal);
            Assert.Contains("Copy", html, StringComparison.Ordinal);
            Assert.DoesNotContain("toolbar-overflow-button", html, StringComparison.Ordinal);
            Assert.DoesNotContain("toolbar-overflow-popup", html, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "I-TB-014 A parent re-render keeps item state such as the selected toggle")]
        public void ParentRerender_KeepsItemState()
        {
            IRenderedComponent<ToggleHost> cut = RenderComponent<ToggleHost>();
            cut.Find(".e-btn").Click();
            Assert.Equal("true", cut.Find(".e-btn").GetAttribute("aria-pressed"));

            cut.SetParametersAndRender(p => p.Add(h => h.Label, "changed"));

            Assert.Equal("true", cut.Find(".e-btn").GetAttribute("aria-pressed"));
            Assert.Equal("changed", cut.Find("#label").TextContent);
        }

        [Fact(DisplayName = "I-TB-015 A bound Selected value survives re-rendering and flows both ways")]
        public void BoundSelected_SurvivesRerender()
        {
            IRenderedComponent<ToggleHost> cut = RenderComponent<ToggleHost>(p => p.Add(h => h.Initial, true));
            Assert.Equal("true", cut.Find(".e-btn").GetAttribute("aria-pressed"));

            cut.Find(".e-btn").Click();
            Assert.Equal("false", cut.Find(".e-btn").GetAttribute("aria-pressed"));
            Assert.Equal("False", cut.Find("#state").TextContent);

            cut.Find("#set").Click();
            Assert.Equal("true", cut.Find(".e-btn").GetAttribute("aria-pressed"));
        }

        [Fact(DisplayName = "U-TB-069 Toggling one item does not re-render its sibling items")]
        public void ItemToggle_RendersOnlyThatItem()
        {
            IRenderedComponent<ToggleHost> cut = RenderComponent<ToggleHost>();
            IReadOnlyList<IRenderedComponent<ToolbarItem>> items = cut.FindComponents<ToolbarItem>();
            int siblingBefore = items[1].RenderCount;
            int ownBefore = items[0].RenderCount;

            cut.Find(".e-btn").Click();

            Assert.True(items[0].RenderCount > ownBefore, "The toggled item should re-render.");
            Assert.Equal(siblingBefore, items[1].RenderCount);
        }

        [Fact(DisplayName = "U-TB-069 Reporting an item change does not re-render the toolbar or a sibling item")]
        public void ItemChangeNotification_DoesNotRerenderToolbar()
        {
            IRenderedComponent<ToggleHost> cut = RenderComponent<ToggleHost>();
            IRenderedComponent<SfToolbar> toolbar = cut.FindComponent<SfToolbar>();
            IReadOnlyList<IRenderedComponent<ToolbarItem>> items = cut.FindComponents<ToolbarItem>();
            int toolbarBefore = toolbar.RenderCount;
            int siblingBefore = items[1].RenderCount;

            toolbar.InvokeAsync(() => ((IToolbarHost)toolbar.Instance).NotifyItemChanged((IToolbarItem)items[0].Instance)).GetAwaiter().GetResult();
            Thread.Sleep(80);

            Assert.Equal(siblingBefore, items[1].RenderCount);
            Assert.Equal(toolbarBefore, toolbar.RenderCount);
        }

        private sealed class ToggleHost : ComponentBase
        {
            [Parameter]
            public string Label { get; set; } = "start";

            [Parameter]
            public bool Initial { get; set; }

            private bool _selected;
            private bool _seeded;

            protected override void OnParametersSet()
            {
                if (!_seeded)
                {
                    _selected = Initial;
                    _seeded = true;
                }
            }

            protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
            {
                int seq = 0;
                builder.OpenComponent<SfToolbar>(seq++);
                builder.AddAttribute(seq++, "AriaLabel", "Host");
                builder.AddAttribute(seq++, "ChildContent", (RenderFragment)(b =>
                {
                    int s = 0;
                    b.OpenComponent<ToolbarItem>(s++);
                    b.AddAttribute(s++, "Id", "bold");
                    b.AddAttribute(s++, "Text", "Bold");
                    b.AddAttribute(s++, "Type", ToolbarItemType.Toggle);
                    b.AddAttribute(s++, "Selected", _selected);
                    b.AddAttribute(s++, "SelectedChanged", EventCallback.Factory.Create<bool>(this, v => _selected = v));
                    b.CloseComponent();
                    b.OpenComponent<ToolbarItem>(s++);
                    b.AddAttribute(s++, "Id", "plain");
                    b.AddAttribute(s++, "Text", "Plain");
                    b.CloseComponent();
                }));
                builder.CloseComponent();
                builder.OpenElement(seq++, "p");
                builder.AddAttribute(seq++, "id", "label");
                builder.AddContent(seq++, Label);
                builder.CloseElement();
                builder.OpenElement(seq++, "p");
                builder.AddAttribute(seq++, "id", "state");
                builder.AddContent(seq++, _selected.ToString());
                builder.CloseElement();
                builder.OpenElement(seq++, "button");
                builder.AddAttribute(seq++, "id", "set");
                builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, () => _selected = true));
                builder.AddContent(seq++, "Set");
                builder.CloseElement();
            }
        }
    }
}
