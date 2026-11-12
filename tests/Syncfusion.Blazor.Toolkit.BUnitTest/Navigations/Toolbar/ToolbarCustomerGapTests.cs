using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// Customer-facing gaps: icons, event feedback, toggle binding, alignment, RTL and data-bound items.
    /// </summary>
    public class ToolbarCustomerGapTests : ToolbarTestBase
    {
        [Fact(DisplayName = "U-TB-050 IconCss renders an e-icons span the shared font can paint")]
        public void IconCss_RendersIconSpan()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Cut", ("IconCss", "e-icons e-cut")));
            IElement icon = cut.Find("span.e-icons.e-cut");

            Assert.Contains("e-btn-icon", icon.ClassName, StringComparison.Ordinal);
            Assert.Equal("true", icon.GetAttribute("aria-hidden"));
        }

        [Fact(DisplayName = "U-TB-050 Badge items still render IconCss inside the button")]
        public void IconCss_RendersWhenBadgeForcesChildContent()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Cut", ("IconCss", "e-icons e-cut"), ("Badge", "2")));

            Assert.NotNull(cut.Find("span.e-icons.e-cut"));
            Assert.Contains("2", cut.Find(".e-toolbar-badge").TextContent, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "U-TB-070 A consumer can display the clicked item id")]
        public void Click_ConsumerCanDisplayResult()
        {
            string shown = "";
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Save", ("Id", "save")),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, a => shown = a.ItemId)));

            cut.Find("[data-testid='toolbar-button']").Click();

            Assert.Equal("save", shown);
        }

        [Fact(DisplayName = "U-TB-030 Toggle click flips Selected, aria-pressed and SelectedChanged")]
        public void Toggle_FlipsAndBinds()
        {
            bool bound = false;
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Item("Bold", ("Type", ToolbarItemType.Toggle), ("Selected", false),
                    ("SelectedChanged", EventCallback.Factory.Create<bool>(this, v => bound = v))));

            IElement button = cut.Find("[data-testid='toolbar-toggle']");
            Assert.Equal("false", button.GetAttribute("aria-pressed"));
            button.Click();

            Assert.True(bound);
            Assert.Equal("true", cut.Find("[data-testid='toolbar-toggle']").GetAttribute("aria-pressed"));
        }

        [Fact(DisplayName = "U-TB-030 Cancel keeps Selected and still raises OnItemClick")]
        public void Toggle_CancelKeepsSelected()
        {
            int toolbar = 0;
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Item("Bold", ("Id", "bold"), ("Type", ToolbarItemType.Toggle), ("Selected", true)),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, a =>
                {
                    toolbar++;
                    a.Cancel = true;
                })));

            cut.Find("[data-testid='toolbar-toggle']").Click();

            Assert.Equal(1, toolbar);
            Assert.Equal("true", cut.Find("[data-testid='toolbar-toggle']").GetAttribute("aria-pressed"));
        }

        [Theory(DisplayName = "U-TB-040 Align maps to a layout class the stylesheet can use")]
        [InlineData(ToolbarAlignment.Start, "e-toolbar-align-start")]
        [InlineData(ToolbarAlignment.Center, "e-toolbar-align-center")]
        [InlineData(ToolbarAlignment.End, "e-toolbar-align-end")]
        [InlineData(ToolbarAlignment.Justify, "e-toolbar-align-justify")]
        public void Align_RendersClass(ToolbarAlignment align, string expected)
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("A"), p => p.Add(t => t.Align, align));

            Assert.Contains(expected, cut.Find(Root).ClassName, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "U-TB-041 EnableRtl true, false and null set or omit dir")]
        public void Rtl_DirectionContract()
        {
            IRenderedComponent<SfToolbar> rtl = RenderToolbar(Item("A"), p => p.Add(t => t.EnableRtl, true));
            IRenderedComponent<SfToolbar> ltr = RenderToolbar(Item("A"), p => p.Add(t => t.EnableRtl, false));
            IRenderedComponent<SfToolbar> inherit = RenderToolbar(Item("A"));

            Assert.Equal("rtl", rtl.Find(Root).GetAttribute("dir"));
            Assert.Contains("e-rtl", rtl.Find(Root).ClassName, StringComparison.Ordinal);
            Assert.Equal("ltr", ltr.Find(Root).GetAttribute("dir"));
            Assert.False(inherit.Find(Root).HasAttribute("dir"));
        }

        [Fact(DisplayName = "U-TB-028 Items without a template render icons, text and clicks")]
        public void Items_DefaultRendering()
        {
            string? clicked = null;
            List<ToolbarItemModel> models =
            [
                new() { Id = "cut", Text = "Cut", IconCss = "e-icons e-cut" },
                new() { Id = "copy", Text = "Copy", IconCss = "e-icons e-copy" },
                new() { Id = "paste", Text = "Paste", IconCss = "e-icons e-paste", Enabled = false }
            ];
            IRenderedComponent<SfToolbar> cut = RenderComponent<SfToolbar>(p => p
                .Add(t => t.Items, models)
                .Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, a => clicked = a.ItemId)));

            Assert.Equal(3, cut.FindAll("[data-testid='toolbar-button']").Count);
            Assert.NotNull(cut.Find("span.e-icons.e-cut"));
            cut.FindAll("[data-testid='toolbar-button']")[0].Click();
            Assert.Equal("cut", clicked);
        }

        [Fact(DisplayName = "U-TB-028 ItemTemplate receives the model and still raises the item click")]
        public void Items_TemplatePath()
        {
            string? seen = null;
            string? clicked = null;
            List<ToolbarItemModel> models = [new() { Id = "one", Text = "One", IconCss = "e-icons e-cut" }];
            RenderFragment<ToolbarTemplateContext> template = ctx => builder =>
            {
                seen = ctx.Item.Text;
                builder.AddContent(0, ctx.Item.IconCss);
            };
            IRenderedComponent<SfToolbar> cut = RenderComponent<SfToolbar>(p => p
                .Add(t => t.Items, models)
                .Add(t => t.ItemTemplate, template)
                .Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, a =>
                {
                    clicked = a.ItemId;
                    Assert.Same(models[0], a.Item);
                })));

            Assert.Equal("One", seen);
            Assert.Contains("e-icons e-cut", cut.Markup, StringComparison.Ordinal);
            cut.Find("[data-testid='toolbar-template-item']").Click();
            Assert.Equal("one", clicked);
        }

        [Fact(DisplayName = "U-TB-028 Declarative children render before data-bound items")]
        public void Items_MixedOrder()
        {
            List<ToolbarItemModel> models = [new() { Id = "bound", Text = "Bound" }];
            IRenderedComponent<SfToolbar> cut = RenderComponent<SfToolbar>(p => p
                .Add(t => t.Items, models)
                .AddChildContent(Item("Declared")));

            string[] texts = cut.FindAll("[data-testid='toolbar-button']").Select(e => e.TextContent.Trim()).ToArray();
            Assert.Equal(new[] { "Declared", "Bound" }, texts);
        }
    }
}
