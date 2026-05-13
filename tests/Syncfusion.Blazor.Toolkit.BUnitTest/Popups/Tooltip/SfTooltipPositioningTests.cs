using Bunit;
using Xunit;
using Syncfusion.Blazor.Toolkit.Popups;
using Syncfusion.Blazor.Toolkit.Buttons;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    public class SfTooltipPositioningTests : TooltipJsMock
    {
        [Fact(DisplayName = "Tooltip - RefreshPosition invokes JS refreshPosition")]
        public async System.Threading.Tasks.Task RefreshPosition_Invokes_RefreshPosition()
        {
            const string REFRESHPOSITION = "refreshPosition";
            JSInterop.Setup<string>(REFRESHPOSITION, _ => true).SetResult(string.Empty);

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "pos1")
                .Add(p => p.Content, "PosTest")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            await tooltip.Instance.RefreshPositionAsync();

            Assert.Contains(REFRESHPOSITION, JSInterop.Invocations.Select(i => i.Identifier));
        }

        [Fact(DisplayName = "Tooltip - with Container still allows RefreshPosition")]
        public async System.Threading.Tasks.Task Container_Allows_RefreshPosition()
        {
            const string REFRESHPOSITION = "refreshPosition";
            JSInterop.Setup<string>(REFRESHPOSITION, _ => true).SetResult(string.Empty);

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "pos2")
                .Add(p => p.Container, "#app")
                .Add(p => p.Content, "ContainerTest")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            await tooltip.Instance.RefreshPositionAsync();

            Assert.Contains(REFRESHPOSITION, JSInterop.Invocations.Select(i => i.Identifier));
        }
    }
}
