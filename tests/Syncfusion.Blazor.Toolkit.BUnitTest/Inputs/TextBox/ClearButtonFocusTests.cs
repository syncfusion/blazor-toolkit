using Bunit;
using Syncfusion.Blazor.Toolkit;
using Xunit;

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
    }
}