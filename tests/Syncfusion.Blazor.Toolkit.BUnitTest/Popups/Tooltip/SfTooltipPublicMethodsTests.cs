using Bunit;
using Xunit;
using Syncfusion.Blazor.Toolkit.Popups;
using Syncfusion.Blazor.Toolkit.Buttons;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    public class SfTooltipPublicMethodsTests : TooltipJsMock
    {
        [Fact(DisplayName = "Tooltip - CloseAsync invokes JS hideTooltip")]
        public async Task CloseAsync_Invokes_HideTooltip()
        {
            const string HIDETOOLTIP = "hideTooltip";
            JSInterop.Setup<string>(HIDETOOLTIP, _ => true).SetResult(string.Empty);

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Target, ".e-btn")
                .Add(p => p.Content, "CloseTest")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            await tooltip.Instance.CloseAsync();

            Assert.Contains(HIDETOOLTIP, JSInterop.Invocations.Select(i => i.Identifier));
        }

        [Fact(DisplayName = "Tooltip - Refresh calls JS refresh")]
        public async Task Refresh_Invokes_Refresh()
        {
            const string REFRESH = "refresh";
            JSInterop.Setup<string>(REFRESH, _ => true).SetResult(string.Empty);

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "ref1")
                .Add(p => p.Content, "RefTest")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            await tooltip.Instance.RefreshAsync();

            Assert.Contains(REFRESH, JSInterop.Invocations.Select(i => i.Identifier));
        }

        [Fact(DisplayName = "Tooltip - RefreshPositionAsync with no argument passes string Target to JS")]
        public async Task RefreshPositionAsync_WithNoArgument_PassesStringTargetToJs()
        {
            const string REFRESHPOSITION = "refreshPosition";
            JSInterop.Setup<string>(REFRESHPOSITION, _ => true).SetResult(string.Empty);

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Target, ".my-target")
                .Add(p => p.Content, "PosTest")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            await tooltip.Instance.RefreshPositionAsync();

            var invocation = JSInterop.Invocations.LastOrDefault(i => i.Identifier == REFRESHPOSITION);
            Assert.NotNull(invocation);
            // Second argument (index 1) is the targetArg — should be the string Target selector.
            Assert.Equal(".my-target", invocation.Arguments[1]);
        }

        [Fact(DisplayName = "Tooltip - RefreshPositionAsync with ElementReference passes ElementReference to JS instead of string Target")]
        public async Task RefreshPositionAsync_WithElementReference_PassesElementReferenceToJs()
        {
            const string REFRESHPOSITION = "refreshPosition";
            JSInterop.Setup<string>(REFRESHPOSITION, _ => true).SetResult(string.Empty);

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Target, ".my-target")
                .Add(p => p.Content, "PosTest")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            var elementRef = new ElementReference("test-element-id");
            await tooltip.Instance.RefreshPositionAsync(elementRef);

            var invocation = JSInterop.Invocations.LastOrDefault(i => i.Identifier == REFRESHPOSITION);
            Assert.NotNull(invocation);
            // Second argument (index 1) must be the ElementReference, not the string Target.
            Assert.IsType<ElementReference>(invocation.Arguments[1]);
            Assert.NotEqual(".my-target", invocation.Arguments[1]);
        }
    }
}
