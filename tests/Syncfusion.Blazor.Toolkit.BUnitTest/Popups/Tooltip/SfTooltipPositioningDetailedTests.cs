using Bunit;
using Xunit;
using Syncfusion.Blazor.Toolkit.Popups;
using Syncfusion.Blazor.Toolkit.Buttons;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    public class SfTooltipPositioningDetailedTests : TooltipJsMock
    {
        [Fact(DisplayName = "Tooltip - RefreshPosition passes Target string to JS")]
        public async System.Threading.Tasks.Task RefreshPosition_Passes_Target_String()
        {
            const string REFRESHPOSITION = "refreshPosition";
            JSInterop.Setup<string>(REFRESHPOSITION, _ => true).SetResult(string.Empty);

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "dpos1")
                .Add(p => p.Target, ".my-target")
                .Add(p => p.Content, "PosArgTest")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            await tooltip.Instance.RefreshPositionAsync();

            var inv = JSInterop.Invocations.First(i => i.Identifier == REFRESHPOSITION);
            // Arguments: dataId, target(ElementReference or null), Target (string)
            Assert.Equal(".my-target", inv.Arguments[1]?.ToString());
        }

        [Fact(DisplayName = "Tooltip - CreateTooltip(false) removes wrapper from markup")]
        public async System.Threading.Tasks.Task CreateTooltip_Removes_Wrapper()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "dpos2")
                .Add(p => p.Content, "Removable")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            // create wrapper and verify
            await tooltip.Instance.CreateTooltipAsync(true);
            Assert.Contains("Removable", tooltip.Markup);

            // remove wrapper and verify content no longer present
            await tooltip.Instance.CreateTooltipAsync(false);
            Assert.DoesNotContain("Removable", tooltip.Markup);
        }
    }
}
