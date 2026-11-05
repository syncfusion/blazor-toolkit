using Bunit;
using Syncfusion.Blazor.Toolkit;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.NumericTextBox
{
    public class ClearButtonFocusTests : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public void ClearButton_IsFocusable_WhenValueExists()
        {
            var cut = RenderComponent<SfNumericTextBox<int?>>(p => p
                .Add(x => x.ShowClearButton, true)
                .Add(x => x.Value, 10));

            var clear = cut.Find("button.e-close");
            Assert.NotNull(clear);
            Assert.Equal("0", clear.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000)]
        public void ClearButton_IsFocusable_AfterClearAndReentry()
        {
            var cut = RenderComponent<SfNumericTextBox<int?>>(p => p
                .Add(x => x.ShowClearButton, true)
                .Add(x => x.Value, 10));

            cut.Find("button.e-close").MouseDown();
            cut.Find("input").Input("5");

            var clear = cut.Find("button.e-close");
            Assert.NotNull(clear);
            Assert.Equal("0", clear.GetAttribute("tabindex"));
        }
    }
}