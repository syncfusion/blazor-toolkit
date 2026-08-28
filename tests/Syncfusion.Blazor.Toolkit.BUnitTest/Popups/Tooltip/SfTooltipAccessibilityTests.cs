using Bunit;
using Xunit;
using Syncfusion.Blazor.Toolkit.Popups;
using Syncfusion.Blazor.Toolkit.Buttons;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    public class SfTooltipAccessibilityTests : TooltipJsMock
    {
        [Fact(DisplayName = "Tooltip - dynamic wrapper has role attribute (accessibility)")]
        public async Task DynamicWrapper_Has_Role()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "acc1")
                .Add(p => p.Content, "A11y")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            // Simulate JS creating the dynamic wrapper
            await tooltip.Instance.CreateTooltipAsync(true);

            var wrapper = tooltip.Find(".e-tooltip-wrap");
            // The wrapper carries role=tooltip. The previous aria-hidden="false" rendered as a
            // no-op (the tooltip wrapper is always in the DOM, and JS toggles the e-hidden
            // class on show/hide) and was removed so screen readers don't see an incorrect state.
            Assert.Equal("tooltip", wrapper.GetAttribute("role"));
            Assert.Null(wrapper.GetAttribute("aria-hidden"));
        }
    }
}
