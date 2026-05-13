using Bunit;
using Xunit;
using System.Reflection;
using System.Text.Json;
using Syncfusion.Blazor.Toolkit.Popups;
using Syncfusion.Blazor.Toolkit.Buttons;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    public class SfTooltipPositioningIntegrationTests : TooltipJsMock
    {
        [Fact(DisplayName = "Tooltip - WireEvents sends position, container and windowCollision properties")]
        public async Task WireEvents_Includes_Position_Container_WindowCollision()
        {
            const string WIREEVENTS = "wireEvents";

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Target, ".e-btn")
                .Add(p => p.Content, "PlacementTest")
                .Add(p => p.Position, Position.RightBottom)
                .Add(p => p.Container, "#my-container")
                .Add(p => p.WindowCollision, true)
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            // Invoke the internal OnAfterScriptRenderedAsync to simulate script initialization
            var method = tooltip.Instance.GetType().GetMethod("OnAfterScriptRenderedAsync", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Assert.NotNull(method);
            var task = (Task)method!.Invoke(tooltip.Instance, new object[] { });
            await task.ConfigureAwait(false);

            Assert.Contains(WIREEVENTS, JSInterop.Invocations.Select(i => i.Identifier));
            var inv = JSInterop.Invocations.First(i => i.Identifier == WIREEVENTS);

            // properties argument is at index 3: dataId, _tooltipElement, dotnetRef, properties, events
            Assert.True(inv.Arguments.Count > 3);
            var props = inv.Arguments[3];

            string positionVal = props switch
            {
                JsonElement je when je.ValueKind == JsonValueKind.Object && je.TryGetProperty("position", out var p) => p.GetString() ?? string.Empty,
                System.Collections.IDictionary dict when dict.Contains("position") => dict["position"]?.ToString() ?? string.Empty,
                _ => props?.ToString() ?? string.Empty
            };

            string containerVal = props switch
            {
                JsonElement je when je.ValueKind == JsonValueKind.Object && je.TryGetProperty("container", out var p) => p.GetString() ?? string.Empty,
                System.Collections.IDictionary dict when dict.Contains("container") => dict["container"]?.ToString() ?? string.Empty,
                _ => string.Empty
            };

            string windowCollisionVal = props switch
            {
                JsonElement je when je.ValueKind == JsonValueKind.Object && je.TryGetProperty("windowCollision", out var p) => p.GetRawText(),
                System.Collections.IDictionary dict when dict.Contains("windowCollision") => dict["windowCollision"]?.ToString() ?? string.Empty,
                _ => string.Empty
            };

            Assert.Contains("RightBottom", positionVal);
            Assert.Contains("#my-container", containerVal);
            Assert.True(windowCollisionVal.Contains("true") || windowCollisionVal.Contains("True"));
        }

        [Fact(DisplayName = "Tooltip - BeforeCollision event is only triggered once per display cycle")]
        public async Task TriggerBeforeCollisionEvent_OnlyOnce()
        {
            var callCount = 0;
            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Target, ".e-btn")
                .Add(p => p.Content, "CollisionTest")
                .Add(s => s.Colliding, (TooltipEventArgs e) =>
                {
                    callCount++;
                    Assert.Equal("TopLeft", e.CollidedPosition);
                })
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show")));

            // First invocation should trigger the handler
            await tooltip.Instance.TriggerBeforeCollisionEventAsync(new TooltipEventArgs { CollidedPosition = "TopLeft" });

            // Second invocation should be ignored due to internal flag
            await tooltip.Instance.TriggerBeforeCollisionEventAsync(new TooltipEventArgs { CollidedPosition = "TopLeft" });

            Assert.Equal(1, callCount);
        }
    }
}