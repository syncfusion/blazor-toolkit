using Xunit;
using Syncfusion.Blazor.Toolkit.Popups;
using Syncfusion.Blazor.Toolkit.Buttons;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    public class SfTooltipRenderingTests : TooltipJsMock
    {
        [Fact(DisplayName = "Tooltip - Content renders in markup")]
        public async Task Content_Renders_InMarkup()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "r1")
                .Add(p => p.Content, "Hello Tooltip")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            // Create the tooltip wrapper (simulates JS rendering) so content is included in markup
            await tooltip.Instance.CreateTooltipAsync(true);

            Assert.Contains("Hello Tooltip", tooltip.Markup);
        }

        [Fact(DisplayName = "Tooltip - HtmlAttributes applied to wrapper")]
        public void HtmlAttributes_Applied()
        {
            var attrs = new System.Collections.Generic.Dictionary<string, object>() { { "data-test", "r2" } };
            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "r2")
                .Add(p => p.HtmlAttributes, attrs)
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            Assert.Contains("data-test=\"r2\"", tooltip.Markup);
        }
    }
}
