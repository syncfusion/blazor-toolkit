using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.DatePicker
{
    public class DateFormat : BunitTestContext
    {
        private string shortPattern { get; set; } = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
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

        private string GetDateFormat<T>(T date, string? format = null, string? culture = null)
        {
            try
            {
                var currentCulture = CultureInfo.CurrentCulture;
                IFormattable? dateValue = date as IFormattable;
                var dateCulture = dateValue?.ToString(format, currentCulture);
                dateCulture = GetNativeDigits(dateCulture!, currentCulture.NumberFormat.NativeDigits);
                return dateCulture;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Fact(Timeout = 10000, DisplayName = "Format: default short date format")]
        public async Task DefaultFormat()
        {
            var component = RenderComponent<SfDatePicker<DateTime?>>(param=>param.Add(p=>p.Value, new DateTime(2021, 3, 5)));
            await component.Instance.ShowPopupAsync();
            var popupElement = component.Find(".e-popup");
            var tableElement = popupElement?.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            tableRows?[2].QuerySelectorAll("td")[2].Click();
            Assert.Equal(new DateTime(2021, 3, 9), component.Instance.Value);
        }

        [Fact(Timeout = 10000, DisplayName = "Format: custom date/month format (d/M)")]
        public async Task DateMonthFormat()
        {
            var component = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p=>p.Format, "d/M"));
            var inputElement = component.Find("input");
            var inputValue = GetDateFormat(component.Instance.Value, component.Instance.Format);
            Assert.Equal(inputValue, inputElement.GetAttribute("value"));
            await component.Instance.ShowPopupAsync();
            var popupElement = component.Find(".e-popup");
            var tableElement = popupElement?.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            var selectedCell = tableElement?.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("5", selectedCell?[0].FirstElementChild?.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(component.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
        }

        [Fact(Timeout = 10000, DisplayName = "Format: year format (y)")]
        public async Task YearFormat()
        {
            var component = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.Format, "y"));
            var inputElement = component.Find("input");
            var inputValue = GetDateFormat(component.Instance.Value, "y");
            Assert.Equal(inputValue, inputElement.GetAttribute("value"));
            await component.Instance.ShowPopupAsync();
            var popupElement = component.Find(".e-popup");
            var tableElement = popupElement?.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            var selectedCell = tableElement?.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("5", selectedCell?[0].FirstElementChild?.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(component.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            await component.Instance.HidePopupAsync();
            await component.Instance.ClosePopupAsync();
            component.SetParametersAndRender(("Format", shortPattern));
            inputElement = component.Find("input");
            inputValue = GetDateFormat(component.Instance.Value, shortPattern);
            Assert.Equal(inputValue, inputElement.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "Format: full date format")]
        public async Task FullDateFormat()
        {
            var component = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.Format, fullPattern));
            var inputElement = component.Find("input");
            var inputValue = GetDateFormat(component.Instance.Value, fullPattern);
            Assert.Equal(inputValue, inputElement.GetAttribute("value"));
            await component.Instance.ShowPopupAsync();
            var popupElement = component.Find(".e-popup");
            var tableElement = popupElement?.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            var selectedCell = tableElement?.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("5", selectedCell?[0].FirstElementChild?.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(component.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            await component.Instance.HidePopupAsync();
            await component.Instance.ClosePopupAsync();            
        }

        [Fact(Timeout = 10000, DisplayName = "Format: short format (d/M/yy)")]
        public async Task shortFormat()
        {
            var component = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.Format, "d/M/yy"));
            await component.Instance.ShowPopupAsync();
            var popupElement = component.Find(".e-popup");
            var tableElement = popupElement?.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            var selectedCell = tableElement?.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("5", selectedCell?[0].FirstElementChild?.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(component.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            var inputElement = component.Find("input");
            var inputValue = GetDateFormat(component.Instance.Value, "d/M/yy");
            Assert.Equal(inputValue, inputElement.GetAttribute("value"));
            await component.Instance.HidePopupAsync();
            await component.Instance.ClosePopupAsync();
        }

        [Fact(Timeout = 10000, DisplayName = "Format: full day/month/year (dddd/MMMM/yyyy)")]
        public async Task MonthFormat()
        {
            var component = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.Format, "dddd/MMMM/yyyy"));
            await component.Instance.ShowPopupAsync();
            var popupElement = component.Find(".e-popup");
            var tableElement = popupElement?.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            var selectedCell = tableElement?.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("5", selectedCell?[0].FirstElementChild?.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(component.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            var inputElement = component.Find("input");
            var inputValue = GetDateFormat(component.Instance.Value, "dddd/MMMM/yyyy");
            Assert.Equal(inputValue, inputElement.GetAttribute("value"));
            Assert.Equal("Friday/March/2021", inputElement.GetAttribute("value"));
            await component.Instance.HidePopupAsync();
            await component.Instance.ClosePopupAsync();
            component.SetParametersAndRender(("Format", "dd/MM/y"));
            inputElement = component.Find("input");
            Assert.Equal("05/03/21", inputElement.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "Format: custom format with text (ddd/MMMMTet/y)")]
        public async Task CustomFormat()
        {
            var component = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.Format, "ddd/MMMMTet/y"));
            await component.Instance.ShowPopupAsync();
            var popupElement = component.Find(".e-popup");
            var tableElement = popupElement?.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            var selectedCell = tableElement?.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("5", selectedCell?[0].FirstElementChild?.TextContent.Replace("\n", "").Trim());
            var inputElement = component.Find("input");
            Assert.Equal("Fri/MarchTeA/21", inputElement.GetAttribute("value"));
            await component.Instance.HidePopupAsync();
            await component.Instance.ClosePopupAsync();
            component.SetParametersAndRender(("Format", "ddd/MMMMMONTH/y"));
            inputElement = component.Find("input");
            Assert.Equal("Fri/MarchONT0/21", inputElement.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "Format: month only (MMMM)")]
        public async Task MonthOnlyFormat()
        {
            var component = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p => p.Format, "MMMM"));
            await component.Instance.ShowPopupAsync();
            var popupElement = component.Find(".e-popup");
            var tableElement = popupElement?.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            var selectedCell = tableElement?.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("5", selectedCell?[0].FirstElementChild?.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(component.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            var inputElement = component.Find("input");
            Assert.Equal("March", inputElement.GetAttribute("value"));
            await component.Instance.HidePopupAsync();
            await component.Instance.ClosePopupAsync();
            component.SetParametersAndRender(("Format", "dddd/MMM"));
            inputElement = component.Find("input");
            Assert.Equal("Friday/Mar", inputElement.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "Format: dynamic format change")]
        public async Task DynamicFormat()
        {
            var component = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)));
            await component.Instance.ShowPopupAsync();
            var popupElement = component.Find(".e-popup");
            var tableElement = popupElement?.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            var selectedCell = tableElement?.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("5", selectedCell?[0].FirstElementChild?.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(component.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            var inputElement = component.Find("input");
            var inputValue = GetDateFormat(component.Instance.Value, shortPattern);
            Assert.Equal(inputValue, inputElement.GetAttribute("value"));
            await component.Instance.HidePopupAsync();
            await component.Instance.ClosePopupAsync();
            component.SetParametersAndRender(("Format", fullPattern));
            inputElement = component.Find("input");
            inputValue = GetDateFormat(component.Instance.Value, fullPattern);
            Assert.Equal(inputValue, inputElement.GetAttribute("value"));
            Assert.Equal("Friday, March 5, 2021 12:00:00 AM", inputElement?.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000, DisplayName = "Format: without year (dd.MM)")]
        public async Task WithOutYearFormat()
        {
            var component = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5)).Add(p=>p.Format, "dd.MM"));
            await component.Instance.ShowPopupAsync();
            var popupElement = component.Find(".e-popup");
            var tableElement = popupElement?.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            var selectedCell = tableElement?.QuerySelectorAll("td.e-selected");
            Assert.Equal(1, selectedCell?.Length);
            Assert.Equal("5", selectedCell?[0].FirstElementChild?.TextContent.Replace("\n", "").Trim());
            Assert.Equal(GetDateFormat(component.Instance.Value, "dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            var inputElement = component.Find("input");
            var inputValue = GetDateFormat(component.Instance.Value, "dd.MM");
            Assert.Equal(inputValue, inputElement.GetAttribute("value"));
            Assert.Equal("05.03", inputElement.GetAttribute("value"));
            await component.Instance.HidePopupAsync();
            await component.Instance.ClosePopupAsync();
        }
    }
}
