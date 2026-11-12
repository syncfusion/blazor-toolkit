using Syncfusion.Blazor.Toolkit.Internal;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Base
{
    /// <summary>
    /// <see cref="SfBaseUtils.FormatUnit"/> feeds Width, Height and MinHeight of Card, Dialog, DatePicker and TimePicker
    /// into inline styles and into scripts that set element styles, so it must keep every legitimate CSS size and drop
    /// anything that could add declarations.
    /// </summary>
    public class SfBaseUtilsFormatUnitTests
    {
        [Theory(DisplayName = "A bare number becomes pixels")]
        [InlineData("300", "300px")]
        [InlineData("0", "0px")]
        [InlineData("1.5", "1.5px")]
        [InlineData(".5", ".5px")]
        [InlineData("-4", "-4px")]
        [InlineData(" 280 ", "280px")]
        public void BareNumber_GetsPx(string input, string expected)
        {
            Assert.Equal(expected, SfBaseUtils.FormatUnit(input));
        }

        [Theory(DisplayName = "Lengths, keywords and CSS math functions pass through unchanged")]
        [InlineData("280px")]
        [InlineData("50%")]
        [InlineData("2.5rem")]
        [InlineData("10em")]
        [InlineData("100vh")]
        [InlineData("50vw")]
        [InlineData("20vmin")]
        [InlineData("1fr")]
        [InlineData("12pt")]
        [InlineData("3cm")]
        [InlineData("1in")]
        [InlineData("200PX")]
        [InlineData("auto")]
        [InlineData("fit-content")]
        [InlineData("max-content")]
        [InlineData("inherit")]
        [InlineData("calc(100% - 2rem)")]
        [InlineData("min(100%, 480px)")]
        [InlineData("clamp(200px, 50%, 600px)")]
        [InlineData("var(--card-width)")]
        [InlineData("calc(var(--gap) * 2)")]
        public void SafeCssValue_IsKept(string input)
        {
            Assert.Equal(input, SfBaseUtils.FormatUnit(input));
        }

        [Fact(DisplayName = "A safe value is trimmed")]
        public void SafeCssValue_IsTrimmed()
        {
            Assert.Equal("50%", SfBaseUtils.FormatUnit("  50%  "));
        }

        [Theory(DisplayName = "Values that could inject CSS are dropped")]
        [InlineData("10px;background:red")]
        [InlineData("100px; position:fixed; inset:0")]
        [InlineData("10px}body{display:none")]
        [InlineData("10px\" onmouseover=\"alert(1)")]
        [InlineData("10px' x='1")]
        [InlineData("10px<script>")]
        [InlineData("url(https://example.com/x.png)")]
        [InlineData("URL (https://example.com/x.png)")]
        [InlineData("calc(1px + url(x))")]
        [InlineData("expression(alert(1))")]
        [InlineData("\\75 rl(x)")]
        [InlineData("10px\nbackground:red")]
        public void UnsafeValue_IsDropped(string input)
        {
            Assert.Equal(string.Empty, SfBaseUtils.FormatUnit(input));
        }

        [Theory(DisplayName = "Null, empty and whitespace are returned as given")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Blank_IsReturnedAsIs(string? input)
        {
            Assert.Equal(input, SfBaseUtils.FormatUnit(input!));
        }
    }
}
