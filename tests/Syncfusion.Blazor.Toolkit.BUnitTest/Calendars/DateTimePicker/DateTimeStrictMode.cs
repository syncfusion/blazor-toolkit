using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.DateTimePicker
{
    public class DateTimeStrictMode : BunitTestContext
    {
        const string FORMATFULLDATE = "dddd, MMMM dd, yyyy";
        const string FORMATDATE = " d ";
        const string TITLE_SEPARATOR = " - ";
        const string FORMAT_YEAR = "yyyy";
        private string shortPattern { get; set; } = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        private string fullPattern { get; set; } = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
        private string GetNativeDigits(string formatValue, string[] nativeDigits)
        {
            return formatValue.Replace("0", nativeDigits[0])
                .Replace("1", nativeDigits[1])
                .Replace("2", nativeDigits[2])
                .Replace("3", nativeDigits[3])
                .Replace("4", nativeDigits[4])
                .Replace("5", nativeDigits[5])
                .Replace("6", nativeDigits[6])
                .Replace("7", nativeDigits[7])
                .Replace("8", nativeDigits[8])
                .Replace("9", nativeDigits[9]);
        }
        private string GetDateFormat<T>(T date, string format = null, string culture = null)
        {
            try
            {
                var currentCulture = CultureInfo.CurrentCulture;
                IFormattable dateValue = date as IFormattable;
                var dateCulture = dateValue.ToString(format, currentCulture);
                dateCulture = GetNativeDigits(dateCulture, currentCulture.NumberFormat.NativeDigits);
                return dateCulture;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [Fact(Timeout = 10000)]
        public void DefaultStrictMode()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>();
            Assert.False(dateInstance.Instance.StrictMode);
            var containEle = dateInstance.Find("input").ParentElement;
            Assert.Contains("e-error", containEle.ClassName);
            dateInstance.SetParametersAndRender(("StrictMode", true));
            Assert.True(dateInstance.Instance.StrictMode);
            containEle = dateInstance.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containEle.ClassName);
        }
        [Fact(Timeout = 10000, DisplayName = "strictMode true with min and max as same value")]
        public void StrictModeMinMax()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>(param => param.Add(p => p.Value, DateTime.Now).Add(p => p.StrictMode, true).Add(p => p.Min, new DateTime(2017, 3, 3)).Add(p => p.Max, new DateTime(2017, 3, 3)));
            Assert.Equal(new DateTime(2017, 3, 3), dateInstance.Instance.Value);
            var inputEle = dateInstance.Find("input");
            Assert.Equal("3/3/2017 12:00 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
        }
        [Fact(Timeout = 10000, DisplayName = "strictMode true with min greater than max")]
        public void StrictModeMinGreaterthanMax()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(2017, 3, 3)).Add(p => p.StrictMode, true).Add(p => p.Min, new DateTime(2017, 3, 4)).Add(p => p.Max, new DateTime(2017, 3, 3)));
            Assert.Equal(new DateTime(2017, 3, 3), dateInstance.Instance.Value);
            Assert.Equal(new DateTime(2017, 3, 4), dateInstance.Instance.Min);
            Assert.Equal(new DateTime(2017, 3, 3), dateInstance.Instance.Max);
            var inputEle = dateInstance.Find("input");
            Assert.Equal("3/3/2017 12:00 AM", inputEle.GetAttribute("value"));
            dateInstance.SetParametersAndRender(("Min", new DateTime(2017, 3, 3)));
            Assert.Equal(new DateTime(2017, 3, 3), dateInstance.Instance.Min);

        }
        [Fact(Timeout = 10000, DisplayName = "strictMode with value higher than the max value test case")]
        public void ValueHigherThanMax()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(9999, 4, 4)).Add(p => p.StrictMode, true));
            Assert.Equal(new DateTime(2099, 12, 31, 23, 59, 59), dateInstance.Instance.Value);
            var inputEle = dateInstance.Find("input");
            Assert.Equal("12/31/2099 11:59 PM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
        }
        [Fact(Timeout = 10000, DisplayName = "strictMode test case with value lower than the min value test case")]
        public void ValueLowerThanMax()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(1111, 1, 1)).Add(p => p.StrictMode, true).Add(p => p.Format, "M/d/yyyy"));
            Assert.Equal(new DateTime(1900, 01, 01), dateInstance.Instance.Value);
            var inputEle = dateInstance.Find("input");
            Assert.Equal("1/1/1900", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "strictMode with value out of min and max  test case")]
        public void MinMaxValueBind()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(2017, 3, 3)).Add(p => p.StrictMode, true).Add(p => p.Format, "M/d/yyyy").
            Add(p => p.Min, new DateTime(2017, 4, 4)).Add(p => p.Max, new DateTime(2017, 6, 6)));
            Assert.Equal(new DateTime(2017, 4, 4), dateInstance.Instance.Value);
            var inputEle = dateInstance.Find("input");
            Assert.Equal("4/4/2017", inputEle.GetAttribute("value"));
            Assert.Equal(new DateTime(2017, 4, 4), dateInstance.Instance.Min);
            Assert.Equal(new DateTime(2017, 6, 6), dateInstance.Instance.Max);
        }
        [Fact(Timeout = 10000, DisplayName = "strictMode  false with value higher than the max value test case")]
        public void HigherThanMax()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(9999, 4, 4)).Add(p => p.StrictMode, false));
            Assert.Equal(new DateTime(9999, 4, 4), dateInstance.Instance.Value);
            var inputEle = dateInstance.Find("input");
            var containEle = inputEle.ParentElement;
            Assert.Equal("4/4/9999 12:00 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", containEle.ClassName);
        }
        [Fact(Timeout = 10000, DisplayName = "strictMode false test case with value lower than the min value test case")]
        public void LowerThanMin()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(1111, 1, 1)).Add(p => p.StrictMode, false));
            Assert.Equal(new DateTime(1111, 1, 1), dateInstance.Instance.Value);
            var inputEle = dateInstance.Find("input");
            var containEle = inputEle.ParentElement;
            Assert.Equal("1/1/1111 12:00 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", containEle.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "strictMode false test case with value lower than the min value test case")]
        public void MaxMinStrictMode()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime>>(param => param.Add(p => p.Value, new DateTime(2017, 3, 3)).Add(p => p.Min, new DateTime(2017, 4, 4)).Add(p => p.Max, new DateTime(2017, 6, 6)).Add(p => p.StrictMode, false));
            Assert.Equal(new DateTime(2017, 3, 3), dateInstance.Instance.Value);
            var inputEle = dateInstance.Find("input");
            var containEle = inputEle.ParentElement;
            Assert.Equal("3/3/2017 12:00 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", containEle.ClassName);
            Assert.Equal(new DateTime(2017, 4, 4), dateInstance.Instance.Min);
            Assert.Equal(new DateTime(2017, 6, 6), dateInstance.Instance.Max);
        }
    }
}
