using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.TextBox
{
    public class NegativeInputs : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public void HtmlAttributes_NonStringValues_DoNotThrow()
        {
            var htmlAttributes = new Dictionary<string, object>()
            {
                { "data-num", 123 },
                { "data-string", "string" },
                { "data-object", new { X = 1 } }
            };

            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.HtmlAttributes, htmlAttributes));
            var input = textBox.Find("input");
            Assert.NotNull(input);
            Assert.Equal("123", input.GetAttribute("data-num"));
            Assert.Equal("string", input.GetAttribute("data-string"));
            Assert.NotNull(input.GetAttribute("data-object"));
        }

        [Fact(Timeout = 10000)]
        public void LongValue_IsHandled()
        {
            var longVal = new string('a', 10000);
            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.Value, longVal));
            var input = textBox.Find("input");
            Assert.Equal(longVal, input.GetAttribute("value"));
            Assert.Equal(longVal, textBox.Instance.Value);
        }
    }
}
