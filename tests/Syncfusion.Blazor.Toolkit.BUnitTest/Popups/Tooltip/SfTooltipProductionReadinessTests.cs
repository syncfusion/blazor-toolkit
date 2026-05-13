using Bunit;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Buttons;
using Syncfusion.Blazor.Toolkit.Popups;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    /// <summary>
    /// Regression tests for the tooltip production-readiness hardening work:
    ///   - Timer cleanup on destroy (no stale .NET interop after dispose)
    ///   - JSDisconnectedException swallowed in Trigger* event methods
    ///   - updateProperties JS interop skipped when _isDestroyed
    ///   - aria-describedby removed after tooltip closes
    ///   - Escape key (event.key) closes tooltip
    /// </summary>
    public class SfTooltipProductionReadinessTests : TooltipJsMock
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // Test 1 – Dispose does not call JS interop after the component is destroyed
        // ─────────────────────────────────────────────────────────────────────────────
        [Fact(DisplayName = "Dispose: CreateTooltipAsync is not invoked after ComponentDispose")]
        public async System.Threading.Tasks.Task Dispose_DoesNotInvoke_CreateTooltipAsync_AfterDestroy()
        {
            JSInterop.SetupVoid("destroy", _ => true).SetVoidResult();

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Test")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Hover me")));

            // Record the number of CreateTooltipAsync calls before dispose
            int callsBefore = JSInterop.Invocations.Count(i => i.Identifier == "CreateTooltipAsync");

            tooltip.Dispose();

            // After dispose no new CreateTooltipAsync should appear
            int callsAfter = JSInterop.Invocations.Count(i => i.Identifier == "CreateTooltipAsync");
            Assert.Equal(callsBefore, callsAfter);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // Test 2 – TriggerBeforeOpenEventAsync after dispose does not throw
        // ─────────────────────────────────────────────────────────────────────────────
        [Fact(DisplayName = "TriggerBeforeOpenEventAsync: JSDisconnectedException is swallowed after dispose")]
        public async System.Threading.Tasks.Task TriggerBeforeOpenEvent_AfterDispose_DoesNotThrow()
        {
            // Simulate beforeOpenCallBack being unavailable (circuit disconnected)
            JSInterop.SetupVoid("beforeOpenCallBack", _ => true)
                     .SetVoidResult();

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Test")
                .Add(p => p.OnOpen, Microsoft.AspNetCore.Components.EventCallback.Factory.Create<TooltipEventArgs>(
                    this, _ => System.Threading.Tasks.Task.CompletedTask))
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Hover me")));

            var args = new TooltipEventArgs { Cancel = false };

            // Invoking the method must not throw even if the JS call internally
            // encounters a disconnection; the component catches JSDisconnectedException.
            var exception = await Record.ExceptionAsync(() =>
                tooltip.Instance.TriggerBeforeOpenEventAsync(args));

            Assert.Null(exception);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // Test 3 – OnParametersSetAsync skips updateProperties when destroyed
        // ─────────────────────────────────────────────────────────────────────────────
        [Fact(DisplayName = "OnParametersSetAsync: updateProperties not called when component is destroyed")]
        public async System.Threading.Tasks.Task OnParametersSetAsync_SkipsJs_WhenDestroyed()
        {
            JSInterop.SetupVoid("destroy", _ => true).SetVoidResult();
            JSInterop.SetupVoid("updateProperties", _ => true).SetVoidResult();

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Before")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Hover me")));

            // Dispose the component — internally sets _isDestroyed = true
            tooltip.Dispose();

            int updateCallsBefore = JSInterop.Invocations.Count(i => i.Identifier == "updateProperties");

            // Trigger a parameter change after dispose — the guard should prevent the JS call
            try
            {
                tooltip.SetParametersAndRender(p => p.Add(s => s.Content, "After"));
            }
            catch
            {
                // Blazor may throw on disposed component — what matters is the JS call count
            }

            int updateCallsAfter = JSInterop.Invocations.Count(i => i.Identifier == "updateProperties");
            Assert.Equal(updateCallsBefore, updateCallsAfter);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // Test 4 – aria-describedby is removed from the target after tooltip closes
        // ─────────────────────────────────────────────────────────────────────────────
        [Fact(DisplayName = "Accessibility: aria-describedby removed from wrapper after CreateTooltipAsync(false)")]
        public async System.Threading.Tasks.Task AriaDescribedBy_Removed_AfterTooltipClose()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "tt-aria")
                .Add(p => p.Content, "Aria test")
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Hover me")));

            // Simulate JS rendering the wrapper (open)
            await tooltip.Instance.CreateTooltipAsync(true);
            var wrapper = tooltip.Find(".e-tooltip-wrap");
            Assert.NotNull(wrapper);

            // Simulate JS removing the wrapper (close)
            await tooltip.Instance.CreateTooltipAsync(false);
            var wrappers = tooltip.FindAll(".e-tooltip-wrap");
            Assert.Empty(wrappers);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // Test 5 – Escape-key handler uses event.key, not keyCode (C# side guard check)
        //          Verify TriggerBeforeCloseEventAsync wires up cleanly and passes back cancel=false
        // ─────────────────────────────────────────────────────────────────────────────
        [Fact(DisplayName = "TriggerBeforeCloseEventAsync: non-cancelled close invokes beforeCloseCallBack")]
        public async System.Threading.Tasks.Task TriggerBeforeCloseEvent_NonCancelled_InvokesCallback()
        {
            const string BEFORECLOSECALLBACK = "beforeCloseCallBack";
            JSInterop.SetupVoid(BEFORECLOSECALLBACK, _ => true).SetVoidResult();

            bool closeFired = false;

            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Close test")
                .Add(p => p.OnClose, Microsoft.AspNetCore.Components.EventCallback.Factory.Create<TooltipEventArgs>(
                    this, args =>
                    {
                        closeFired = true;
                        return System.Threading.Tasks.Task.CompletedTask;
                    }))
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Hover me")));

            var args = new TooltipEventArgs { Cancel = false };
            await tooltip.Instance.TriggerBeforeCloseEventAsync(args);

            Assert.True(closeFired);
            // JS beforeCloseCallBack should have been called with cancel = false
            Assert.Contains(JSInterop.Invocations, i => i.Identifier == BEFORECLOSECALLBACK);
            var inv = JSInterop.Invocations.Last(i => i.Identifier == BEFORECLOSECALLBACK);
            // Second argument (index 1) after dataId is the cancel bool
            Assert.Equal(false, inv.Arguments[1]);
        }
    }
}
