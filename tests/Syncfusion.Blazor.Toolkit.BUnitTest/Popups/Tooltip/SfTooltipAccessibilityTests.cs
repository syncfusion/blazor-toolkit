using Bunit;
using Xunit;
using Syncfusion.Blazor.Toolkit.Popups;
using Syncfusion.Blazor.Toolkit.Buttons;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    public class SfTooltipAccessibilityTests : TooltipJsMock
    {
        [Fact(DisplayName = "Tooltip - dynamic wrapper has role and aria-hidden (accessibility)")]
        public async Task DynamicWrapper_Has_Role_And_AriaHidden()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "acc1")
                .Add(p => p.Content, "A11y")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            // Simulate JS creating the dynamic wrapper
            await tooltip.Instance.CreateTooltipAsync(true);

            var wrapper = tooltip.Find(".e-tooltip-wrap");
            Assert.Equal("tooltip", wrapper.GetAttribute("role"));
            Assert.Equal("false", wrapper.GetAttribute("aria-hidden"));
        }
    }
}
