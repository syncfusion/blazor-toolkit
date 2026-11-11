using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit;
using Xunit;
using WebFocusEventArgs = Microsoft.AspNetCore.Components.Web.FocusEventArgs;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.TextArea
{
    public class ClearButtonFocusTests : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public async Task ClearButton_MouseDown_ClearsValueAndRestoresInputFocus()
        {
            var cut = RenderComponent<SfTextArea>(p => p
                .Add(x => x.ShowClearButton, true)
                .Add(x => x.Value, "hello"));

            var input = cut.Find("textarea");
            await input.TriggerEventAsync("onfocus", new WebFocusEventArgs());
            var clearButton = cut.Find("button.e-close");
            var focusCallsBeforeClear = JSInterop.Invocations.Count(invocation =>
                invocation.Identifier.Contains("focus", StringComparison.OrdinalIgnoreCase));

            await clearButton.TriggerEventAsync("onmousedown", new MouseEventArgs());

            Assert.Null(cut.Instance.Value);
            Assert.True(string.IsNullOrEmpty(cut.Find("textarea").GetAttribute("value")));
            Assert.True(JSInterop.Invocations.Count(invocation =>
                invocation.Identifier.Contains("focus", StringComparison.OrdinalIgnoreCase)) > focusCallsBeforeClear);
        }

        [Fact(Timeout = 10000)]
        public async Task FocusHandler_ConcurrentReentry_InvokesOnFocusOnce()
        {
            var callbackEntered = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var callbackCanFinish = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var focusCount = 0;
            var cut = RenderComponent<SfTextArea>(p => p.Add(x => x.OnFocus,
                EventCallback.Factory.Create<TextAreaFocusInEventArgs>(this, async _ =>
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
