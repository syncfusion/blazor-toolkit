using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit;
using Xunit;
using WebFocusEventArgs = Microsoft.AspNetCore.Components.Web.FocusEventArgs;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.TextBox
{
    public class ClearButtonFocusTests : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public void ClearButton_IsFocusable_WhenValueExists ()
        {
            var cut = RenderComponent<SfTextBox>(p => p
                .Add(x => x.ShowClearButton, true)
                .Add(x => x.Value, "hello"));

            // Value alone does not run UpdateIconState; focus does (real user path).
            cut.Find("input").Focus();

            var clear = cut.Find("button.e-close");
            Assert.NotNull(clear);
            Assert.DoesNotContain("e-clear-icon-hide", clear.ClassName);
            Assert.Equal("0", clear.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000)]
        public void ClearButton_IsFocusable_AfterClearAndReentry()
        {
            var cut = RenderComponent<SfTextBox>(p => p
                .Add(x => x.ShowClearButton, true)
                .Add(x => x.Value, "first"));

            var clear = cut.Find("button.e-close");
            clear.MouseDown(); // clear

            var input = cut.Find("input");
            input.Input("second"); // re-entry → UpdateIconState / HasClearableContent

            clear = cut.Find("button.e-close");
            Assert.NotNull(clear);
            Assert.Equal("0", clear.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000)]
        public async Task ClearButton_MouseDown_ClearsValueAndRestoresInputFocus()
        {
            var cut = RenderComponent<SfTextBox>(p => p
                .Add(x => x.ShowClearButton, true)
                .Add(x => x.Value, "hello"));

            var input = cut.Find("input");
            await input.TriggerEventAsync("onfocus", new WebFocusEventArgs());
            var clearButton = cut.Find("button.e-close");
            var focusCallsBeforeClear = JSInterop.Invocations.Count(invocation =>
                invocation.Identifier.Contains("focus", StringComparison.OrdinalIgnoreCase));

            await clearButton.TriggerEventAsync("onmousedown", new MouseEventArgs());

            Assert.Null(cut.Instance.Value);
            Assert.True(string.IsNullOrEmpty(cut.Find("input").GetAttribute("value")));
            Assert.True(JSInterop.Invocations.Count(invocation =>
                invocation.Identifier.Contains("focus", StringComparison.OrdinalIgnoreCase)) > focusCallsBeforeClear);
        }

        [Fact(Timeout = 10000)]
        public async Task FocusHandler_ConcurrentReentry_InvokesOnFocusOnce()
        {
            var callbackEntered = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var callbackCanFinish = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var focusCount = 0;
            var cut = RenderComponent<SfTextBox>(p => p.Add(x => x.OnFocus,
                EventCallback.Factory.Create<FocusInEventArgs>(this, async _ =>
                {
                    if (Interlocked.Increment(ref focusCount) == 1)
                    {
                        callbackEntered.SetResult(true);
                        await callbackCanFinish.Task;
                    }
                })));
            var focusArgs = new WebFocusEventArgs();

            var firstFocus = cut.InvokeAsync(() => cut.Instance.OnFocusHandlerAsync(focusArgs));
            await callbackEntered.Task.WaitAsync(TimeSpan.FromSeconds(5));
            var secondFocus = cut.InvokeAsync(() => cut.Instance.OnFocusHandlerAsync(focusArgs));
            await secondFocus;
            callbackCanFinish.SetResult(true);
            await firstFocus;
            Assert.Equal(1, focusCount);
        }
    }
}