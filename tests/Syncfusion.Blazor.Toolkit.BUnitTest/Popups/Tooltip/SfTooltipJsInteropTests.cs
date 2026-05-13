using Bunit;
using Xunit;
using Syncfusion.Blazor.Toolkit.Popups;
using Syncfusion.Blazor.Toolkit.Buttons;
using System.Text.Json;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    public class SfTooltipJsInteropTests : TooltipJsMock
    {
        [Fact(DisplayName = "Tooltip - OpenAsync with handleId passes selector and animation to JS")]
        public async System.Threading.Tasks.Task OpenAsync_WithHandleAndAnimation_Invokes_ShowTooltip_With_Target()
        {
            const string SHOWTOOLTIP = "showTooltip";
            JSInterop.Setup<string>(SHOWTOOLTIP, _ => true).SetResult(string.Empty);

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Target, ".e-btn")
                .Add(p => p.Content, "OpenArgTest")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            var animation = new TooltipAnimationSettings { Effect = Effect.ZoomIn, Duration = 250 };
            await tooltip.Instance.OpenAsync("my-handle-id", animation);

            Assert.Contains(SHOWTOOLTIP, JSInterop.Invocations.Select(i => i.Identifier));
            var inv = JSInterop.Invocations.First(i => i.Identifier == SHOWTOOLTIP);
            // args: dataId, animation, targetProp
            Assert.NotNull(inv.Arguments[1]);
            var arg1 = inv.Arguments[1]!;
            string effectStr = arg1 switch
            {
                JsonElement je when je.ValueKind == JsonValueKind.Object && je.TryGetProperty("effect", out var p) => p.GetString() ?? string.Empty,
                TooltipAnimationSettings tas => tas.Effect.ToString(),
                _ => arg1.ToString() ?? string.Empty
            };
            int durationVal = arg1 switch
            {
                JsonElement je when je.ValueKind == JsonValueKind.Object && je.TryGetProperty("duration", out var d) && d.ValueKind == JsonValueKind.Number => d.GetInt32(),
                TooltipAnimationSettings tas => (int)(tas.Duration ?? 0.0),
                _ => int.TryParse(arg1.ToString(), out var v) ? v : 0
            };

            Assert.Contains("ZoomIn", effectStr);
            Assert.Equal(250, durationVal);
            Assert.Equal("#my-handle-id", inv.Arguments[2]!.ToString());
        }

        [Fact(DisplayName = "Tooltip - CloseAsync with animation invokes hideTooltip with animation")]
        public async System.Threading.Tasks.Task CloseAsync_WithAnimation_Invokes_HideTooltip_With_Animation()
        {
            const string HIDETOOLTIP = "hideTooltip";
            JSInterop.Setup<string>(HIDETOOLTIP, _ => true).SetResult(string.Empty);

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Target, ".e-btn")
                .Add(p => p.Content, "CloseArgTest")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            var animation = new TooltipAnimationSettings { Effect = Effect.FadeOut, Duration = 100 };
            await tooltip.Instance.CloseAsync(animation);

            Assert.Contains(HIDETOOLTIP, JSInterop.Invocations.Select(i => i.Identifier));
            var inv = JSInterop.Invocations.First(i => i.Identifier == HIDETOOLTIP);
            // args: dataId, animation
            Assert.NotNull(inv.Arguments[1]);
            var argClose = inv.Arguments[1]!;
            string closeEffect = argClose switch
            {
                JsonElement je when je.ValueKind == JsonValueKind.Object && je.TryGetProperty("effect", out var p) => p.GetString() ?? string.Empty,
                TooltipAnimationSettings tas => tas.Effect.ToString(),
                _ => argClose.ToString() ?? string.Empty
            };
            int closeDuration = argClose switch
            {
                JsonElement je when je.ValueKind == JsonValueKind.Object && je.TryGetProperty("duration", out var d) && d.ValueKind == JsonValueKind.Number => d.GetInt32(),
                TooltipAnimationSettings tas => (int)(tas.Duration ?? 0),
                _ => int.TryParse(argClose.ToString(), out var v) ? v : 0
            };

            Assert.Contains("FadeOut", closeEffect);
            Assert.Equal(100, closeDuration);
        }

        [Fact(DisplayName = "Tooltip - TriggerBeforeRenderEvent causes CONTENTUPDATED JS interop when wrapper rendered")]
        public async System.Threading.Tasks.Task TriggerBeforeRenderEvent_Invokes_ContentUpdated_When_WrapperRendered()
        {
            const string CONTENTUPDATED = "contentUpdated";
            const string BEFORERENDERCALLBACK = "beforeRenderCallBack";

            JSInterop.Setup<string>(CONTENTUPDATED, _ => true).SetResult(string.Empty);
            JSInterop.Setup<string>(BEFORERENDERCALLBACK, _ => true).SetResult(string.Empty);

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "content-updated")
                .Add(p => p.Content, "CUpdated")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            // ensure wrapper is rendered
            await tooltip.Instance.CreateTooltipAsync(true);

            // call the before-render invokable which should trigger the callback(s)
            await tooltip.Instance.TriggerBeforeRenderEventAsync(new TooltipEventArgs());

            Assert.Contains(BEFORERENDERCALLBACK, JSInterop.Invocations.Select(i => i.Identifier));
            Assert.Contains(CONTENTUPDATED, JSInterop.Invocations.Select(i => i.Identifier));
        }
    }
}
