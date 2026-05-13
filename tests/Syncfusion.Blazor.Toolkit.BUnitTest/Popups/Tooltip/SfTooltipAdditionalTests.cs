using Bunit;
using Xunit;
using Syncfusion.Blazor.Toolkit.Popups;
using Syncfusion.Blazor.Toolkit.Buttons;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    public class SfTooltipAdditionalTests : TooltipJsMock
    {
        // Initial rendering / Further customization
        [Fact(Timeout = 10000, DisplayName = "Tooltip - AdditionalAttributes are applied")]
        public void AdditionalAttributes_AreRenderedOnWrapper()
        {
            var attrs = new Dictionary<string, object>() { { "data-test", "tooltip-1" }, { "aria-label", "help" } };
            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "tt1")
                .Add(p => p.HtmlAttributes, attrs)
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "ShowTooltip")));

            var outer = tooltip.Find("div#tt1");
            Assert.Equal("tooltip-1", outer.GetAttribute("data-test"));
            Assert.Equal("help", outer.GetAttribute("aria-label"));
        }

        // Public methods / JS interop verification
       [Fact(Timeout = 10000, DisplayName = "Tooltip - OpenAsync invokes JS interop showTooltip")]
        public async Task OpenAsync_Invoke_JS_ShowTooltip()
        {
            bool invoked = false;
            const string SHOWTOOLTIP = "showTooltip";

            // Arrange JSInterop to capture the invocation
            JSInterop.Setup<string>(SHOWTOOLTIP, _ => true).SetResult(string.Empty);

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Target, ".e-btn")
                .Add(p => p.Content, "Check")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "ShowTooltip")));

            // Act
            await tooltip.Instance.OpenAsync();

            // Assert
            Assert.True(JSInterop.Invocations.Any(i => i.Identifier == SHOWTOOLTIP), "Expected JS interop 'showTooltip' to be invoked by OpenAsync");
        }

        // Events / Accessibility
        [Fact(Timeout = 10000, DisplayName = "Tooltip - dynamic wrapper has role and aria attributes")]
        public async Task DynamicWrapper_HasRoleAndAriaHidden()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "tt2")
                .Add(p => p.Content, "Hello")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "ShowTooltip")));

            // Simulate JS callback that creates the wrapper
            await tooltip.Instance.CreateTooltipAsync(true);

            // The dynamic wrapper carries the role attribute
            var wrapper = tooltip.Find(".e-tooltip-wrap");
            Assert.Equal("tooltip", wrapper.GetAttribute("role"));
            Assert.Equal("false", wrapper.GetAttribute("aria-hidden"));
        }
    }
}
