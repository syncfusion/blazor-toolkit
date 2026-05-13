using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.DateTimePicker
{
    public class DateTimeFormat : BunitTestContext
    {
        const string FORMATFULLDATE = "dddd, MMMM d, yyyy";
        const string FORMATDATE = " d ";
        const string TITLE_SEPARATOR = " - ";
        const string FORMAT_YEAR = "yyyy";
        private CultureInfo currentCulture = CultureInfo.CurrentCulture;
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
        public async Task DefaultFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5, 11, 30, 30)));
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            tRows[2].QuerySelectorAll("td")[2].Click();
            Assert.Equal(new DateTime(2021, 3, 9, 11, 30, 0), dateInstance.Instance.Value);
            var inputEle = dateInstance.Find("input");
            var inputValue = GetDateFormat(dateInstance.Instance.Value, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + this.currentCulture.DateTimeFormat.ShortTimePattern);
            Assert.Equal(inputValue, inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000)]
        public async Task DateMonthFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5, 11, 30, 30)).Add(p => p.Format, "d/M"));
            var inputEle = dateInstance.Find("input");
            var inputValue = GetDateFormat(dateInstance.Instance.Value, dateInstance.Instance.Format);
            Assert.Equal(inputValue, inputEle.GetAttribute("value"));
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            var selectedCell = tableElement.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("5", selectedCell[0].FirstElementChild.TextContent.Replace("\n", "").Replace('\u202F', ' ').Trim());
            Assert.Equal(GetDateFormat(dateInstance.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
        }
        [Fact(Timeout = 10000, DisplayName = "value with format(y) test case")]
        public async Task YearFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.Format, "y"));
            var inputEle = dateInstance.Find("input");
            var inputValue = GetDateFormat(dateInstance.Instance.Value, "y");
            Assert.Equal(inputValue, inputEle.GetAttribute("value"));
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            var selectedCell = tableElement.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("5", selectedCell[0].FirstElementChild.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(dateInstance.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            dateInstance.SetParametersAndRender(("Format", shortPattern));
            inputEle = dateInstance.Find("input");
            inputValue = GetDateFormat(dateInstance.Instance.Value, shortPattern);
            Assert.Equal(inputValue, inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "value with datetimeformat(time) test case")]
        public async Task TimeFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5, 11, 30, 30)).Add(p => p.Format, "h:mm tt"));
            var inputEle = dateInstance.Find("input");
            var inputValue = GetDateFormat(dateInstance.Instance.Value, "h:mm tt");
            Assert.Equal(inputValue, inputEle.GetAttribute("value"));
            Assert.Contains("11:30 AM", inputEle.GetAttribute("value"));
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            var selectedCell = tableElement.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("5", selectedCell[0].FirstElementChild.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(dateInstance.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            dateInstance.SetParametersAndRender(("Format", shortPattern));
            inputEle = dateInstance.Find("input");
            inputValue = GetDateFormat(dateInstance.Instance.Value, shortPattern);
            Assert.Equal(inputValue, inputEle.GetAttribute("value"));
            await dateInstance.Instance.ShowTimePopupAsync();
            popupEle = dateInstance.Find(".e-popup");
            var liCollec = popupEle.QuerySelectorAll("li");
            liCollec[1].Click();
            inputEle = dateInstance.Find("input");
            inputValue = GetDateFormat(dateInstance.Instance.Value, shortPattern);
            Assert.Equal(inputValue, inputEle.GetAttribute("value"));
            Assert.Contains("12:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
        }
        [Fact(Timeout = 10000)]
        public async Task FullDateTimeFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.Format, fullPattern));
            var inputEle = dateInstance.Find("input");
            var inputValue = GetDateFormat(dateInstance.Instance.Value, fullPattern);
            Assert.Equal(inputValue, inputEle.GetAttribute("value"));
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            var selectedCell = tableElement.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("5", selectedCell[0].FirstElementChild.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(dateInstance.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            await dateInstance.Instance.ShowTimePopupAsync();
            popupEle = dateInstance.Find(".e-popup");
            var liCollec = popupEle.QuerySelectorAll("li");
            liCollec[1].Click();
            inputEle = dateInstance.Find("input");
            inputValue = GetDateFormat(dateInstance.Instance.Value, fullPattern);
            Assert.Equal(inputValue, inputEle.GetAttribute("value"));
            Assert.Contains("12:30:00 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000, DisplayName = "value with format(short) test case ")]
        public async Task shortFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.Format, "d/M/yy"));
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            var selectedCell = tableElement.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("5", selectedCell[0].FirstElementChild.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(dateInstance.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
            var inputEle = dateInstance.Find("input");
            var inputValue = GetDateFormat(dateInstance.Instance.Value, "d/M/yy");
            Assert.Equal(inputValue, inputEle.GetAttribute("value"));
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
        }
        [Fact(Timeout = 10000, DisplayName = "format(dddd/MMMM/yyyy) test case")]
        public async Task MonthFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.Format, "dddd/MMMM/yyyy"));
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            var selectedCell = tableElement.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("5", selectedCell[0].FirstElementChild.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(dateInstance.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
            var inputEle = dateInstance.Find("input");
            var inputValue = GetDateFormat(dateInstance.Instance.Value, "dddd/MMMM/yyyy");
            Assert.Equal(inputValue, inputEle.GetAttribute("value"));
            Assert.Equal("Friday/March/2021", inputEle.GetAttribute("value"));
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            dateInstance.SetParametersAndRender(("Format", "dd/MM/y"));
            inputEle = dateInstance.Find("input");
            Assert.Equal("05/03/21", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "custom format(ddd/MMMMTet/y) test case")]
        public async Task CustomFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.Format, "ddd/MMMMTet/y"));
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            var selectedCell = tableElement.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("5", selectedCell[0].FirstElementChild.TextContent.Replace("\n", "").Trim());
            var inputEle = dateInstance.Find("input");
            Assert.Equal("Fri/MarchTeA/21", inputEle.GetAttribute("value"));
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            dateInstance.SetParametersAndRender(("Format", "ddd/MMMMMONTH/y"));
            inputEle = dateInstance.Find("input");
            Assert.Equal("Fri/MarchONT0/21", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "format(MMMM) test case")]
        public async Task MonthOnlyFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.Format, "MMMM"));
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            var selectedCell = tableElement.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("5", selectedCell[0].FirstElementChild.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(dateInstance.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
            var inputEle = dateInstance.Find("input");
            Assert.Equal("March", inputEle.GetAttribute("value"));
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            dateInstance.SetParametersAndRender(("Format", "dddd/MMM"));
            inputEle = dateInstance.Find("input");
            Assert.Equal("Friday/Mar", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000)]
        public async Task DynamicFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5, 10, 30, 0)));
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            var selectedCell = tableElement.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("5", selectedCell[0].FirstElementChild.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(dateInstance.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
            var inputEle = dateInstance.Find("input");
            var inputVal = GetDateFormat(dateInstance.Instance.Value, shortPattern);
            Assert.Equal(inputVal, inputEle.GetAttribute("value"));
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            dateInstance.SetParametersAndRender(("Format", fullPattern));
            inputEle = dateInstance.Find("input");
            inputVal = GetDateFormat(dateInstance.Instance.Value, fullPattern);
            Assert.Equal(inputVal, inputEle.GetAttribute("value"));
            Assert.Equal("Friday, March 5, 2021 10:30:00 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
        }
        [Fact(Timeout = 10000)]
        public async Task WithOutYearFormat()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.Format, "dd.MM"));
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            var selectedCell = tableElement.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("5", selectedCell[0].FirstElementChild.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(dateInstance.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell[0].FirstElementChild.GetAttribute("title"));
            var inputEle = dateInstance.Find("input");
            var inputVal = GetDateFormat(dateInstance.Instance.Value, "dd.MM");
            Assert.Equal(inputVal, inputEle.GetAttribute("value"));
            Assert.Equal("05.03", inputEle.GetAttribute("value"));
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            await dateInstance.Instance.ShowTimePopupAsync();
            popupEle = dateInstance.Find(".e-popup");
            var liCollec = popupEle.QuerySelectorAll("li");
            liCollec[1].Click();
            inputEle = dateInstance.Find("input");
            Assert.Contains("05.03", inputEle.GetAttribute("value"));
            Assert.Equal(new DateTime(DateTime.Today.Year, 3, 5, 0, 0, 0).ToString("M/d/yyyy hh:mm:ss tt"),
            dateInstance.Instance.Value.ToString().Replace('\u202F', ' ').Trim());

        }
    }
}
