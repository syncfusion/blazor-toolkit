using Bunit;
using Xunit;
using Syncfusion.Blazor.Toolkit.Popups;
using Syncfusion.Blazor.Toolkit.Buttons;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    public class SfTooltipTemplatesTests : TooltipJsMock
    {
        [Fact(DisplayName = "Tooltip - ContentTemplate renders in markup")]
        public async System.Threading.Tasks.Task ContentTemplate_Renders_InMarkup()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "tpl1")
                .Add<RenderFragment>(p => p.ContentTemplate, builder =>
                {
                    builder.OpenElement(0, "span");
                    builder.AddAttribute(1, "class", "tpl-content");
                    builder.AddContent(2, "TemplateContent");
                    builder.CloseElement();
                })
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            // simulate JS creating the wrapper so template is rendered
            await tooltip.Instance.CreateTooltipAsync(true);

            Assert.Contains("TemplateContent", tooltip.Markup);
            Assert.Contains("tpl-content", tooltip.Markup);
        }

        [Fact(DisplayName = "Tooltip - Updating Content updates markup")]
        public async System.Threading.Tasks.Task Updating_Content_Updates_Markup()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "tpl2")
                .Add(p => p.Content, "Initial")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            await tooltip.Instance.CreateTooltipAsync(true);
            Assert.Contains("Initial", tooltip.Markup);

            // update content via parameters and re-render
            tooltip.SetParametersAndRender(parameters => parameters.Add(p => p.Content, "Updated"));

            Assert.Contains("Updated", tooltip.Markup);
        }
    }
}
