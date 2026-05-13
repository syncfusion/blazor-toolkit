using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using System.Globalization; 

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.DatePicker
{
    public class DateNavigation : BunitTestContext
    {
        const string FORMATFULLDATE = "dddd, MMMM d, yyyy";
        const string TITLE_SEPARATOR = " - ";
        const string FORMAT_YEAR = "yyyy";

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
        private string GetDecadeTitle(DateTime localDate)
        {
            int localYr = localDate.Year;
            localYr = localYr < 10 ? 10 : localYr;
            int startYr = localYr - localYr % 10;
            int endYr = startYr + (10 - 1);
            string startHdrYr = GetDateFormat(new DateTime(startYr, 1, 1), FORMAT_YEAR);
            string endHdrYr = GetDateFormat(new DateTime(endYr, 1, 1), FORMAT_YEAR);
            return startHdrYr + TITLE_SEPARATOR + endHdrYr;
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: Current Month To Next Month")]
        public async Task CurrentMonthToNextMonthNavigation()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
                Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal("Month", calendar.Instance.CurrentView());
            var buttonList = calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).ToString(FORMATFULLDATE), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            buttonList[1].Click();
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(1900, 2, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 2, 1).ToString(FORMATFULLDATE), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(new DateTime(1900, 2, 1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: Current Month To Previous Month")]
        public async Task CurrentMonthToPreviousMonthNavigation()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
                Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal("Month", calendar.Instance.CurrentView());
            var buttonList = calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).ToString(FORMATFULLDATE), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            buttonList[1].Click();
            buttonList = calendar.FindAll("button");
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(1900, 2, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 2, 1).ToString(FORMATFULLDATE), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(new DateTime(1900, 2, 1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            buttonList = calendar.FindAll("button");
            buttonList[0].Click();
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).ToString(FORMATFULLDATE), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(new DateTime(1900, 1, 1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: Current Month To Next Month (Nullable)")]
        public async Task CurrentMonthToNextMonthNavigationWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>();
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal("Month", calendar.Instance.CurrentView());
            var buttonList = calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(DateTime.Now.ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.ToString(FORMATFULLDATE), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            buttonList[1].Click();
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(DateTime.Now.AddMonths(1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.AddMonths(1).ToString(FORMATFULLDATE), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(DateTime.Now.AddMonths(1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: Current Month To Previous Month (Nullable)")]
        public async Task CurrentMonthToPreviousMonthNavigationWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>();
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal("Month", calendar.Instance.CurrentView());
            var buttonList = calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(DateTime.Now.ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.ToString(FORMATFULLDATE), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            buttonList[1].Click();
            buttonList = calendar.FindAll("button");
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(DateTime.Now.AddMonths(1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.AddMonths(1).ToString(FORMATFULLDATE), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(DateTime.Now.AddMonths(1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            var dayvalue = DateTime.Now.AddMonths(1).Day.ToString();
            buttonList = calendar.FindAll("button");
            buttonList[0].Click();
            await Task.Delay(100);
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(DateTime.Now.ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(dayvalue, tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: Current Year To Next Year")]
        public async Task CurrentYearToNextYearNavigation()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
                Add(Cal => Cal.Start, CalendarView.Year).
                Add(Cal => Cal.Depth, CalendarView.Year).
                Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal("Year", calendar.Instance.CurrentView());
            var buttonList = calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMM"), tableElement?.QuerySelector(".e-selected")?.FirstElementChild?.TextContent);
            buttonList[1].Click();
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(1).ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: Current Year To Next Year (Nullable)")]
        public async Task CurrentYearToNextYearNavigationWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(parameters => parameters.
                Add(Cal => Cal.Start, CalendarView.Year).
                Add(Cal => Cal.Depth, CalendarView.Year));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal("Year", calendar.Instance.CurrentView());
            var buttonList = calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(DateTime.Now.ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            buttonList[1].Click();
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(DateTime.Now.AddYears(1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.AddYears(1).ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: Current Year To Previous Year")]
        public async Task CurrentYearToPreviousYear()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
                Add(Cal => Cal.Start, CalendarView.Year).
                Add(Cal => Cal.Depth, CalendarView.Year).
                Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal("Year", calendar.Instance.CurrentView());
            var buttonList = calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMM"), tableElement?.QuerySelector(".e-selected")?.FirstElementChild?.TextContent);
            buttonList[1].Click();
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(1).ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = calendar.FindAll("button");
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            buttonList[0].Click();
            buttonList = calendar.FindAll("button");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMM"), tableElement?.QuerySelector(".e-selected")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: Current Year To Previous Year (Nullable)")]
        public async Task CurrentYearToPreviousYearWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(parameters => parameters.
                Add(Cal => Cal.Start, CalendarView.Year).
                Add(Cal => Cal.Depth, CalendarView.Year));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal("Year", calendar.Instance.CurrentView());
            var buttonList = calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(DateTime.Now.ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            buttonList[1].Click();
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(DateTime.Now.AddYears(1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.AddYears(1).ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            buttonList[0].Click();
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = calendar.FindAll("button");
            Assert.Equal(DateTime.Now.ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: Current Decade To Next Decade")]
        public async Task CurrentDecadeToNextDecade()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
                Add(Cal => Cal.Start, CalendarView.Decade).
                Add(Cal => Cal.Depth, CalendarView.Decade)
                .Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal("Decade", calendar.Instance.CurrentView());
            var buttonList = calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(this.GetDecadeTitle(new DateTime(1900, 1, 1)), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), tableElement?.QuerySelector(".e-selected")?.FirstElementChild?.TextContent);
            buttonList[1].Click();
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(this.GetDecadeTitle(new DateTime(1910, 1, 1)), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(10).ToString("yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: Current Decade To Next Decade (Nullable)")]
        public async Task CurrentDecadeToNextDecadeWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>(parameters => parameters.
                Add(Cal => Cal.Start, CalendarView.Decade).
                Add(Cal => Cal.Depth, CalendarView.Decade));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal("Decade", calendar.Instance.CurrentView());
            var buttonList = calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(this.GetDecadeTitle(DateTime.Now), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.ToString("yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            buttonList[1].Click();
            tableElement = calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(this.GetDecadeTitle(DateTime.Now.AddYears(10)), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(DateTime.Now.AddYears(10).ToString("yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: Month To Year Navigation Title Header")]
        public async Task MonthToYearNavigationTitleHeader()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime>>(parameters => parameters.
                Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal("Month", calendar.Instance.CurrentView());
            Assert.Equal(new DateTime(1900, 1, 1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).ToString(FORMATFULLDATE), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(42, parentContainer?.QuerySelectorAll("td").Length);
            Assert.Equal(new DateTime(1900, 1, 1).ToString(FORMATFULLDATE), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            parentContainer?.QuerySelector(".e-title")?.Click();
            await Task.Delay(200);
            popupElement = calendar.Find(".e-popup");
            tableElement = popupElement.QuerySelector("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            var tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-year", tableContent?.ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(12, parentContainer?.QuerySelectorAll("td").Length);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMM"), parentContainer?.QuerySelector(".e-selected")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: Month To Year Navigation Title Header (Nullable)")]
        public async Task MonthToYearNavigationTitleHeaderWithNullable()
        {
            var calendar = RenderComponent<SfDatePicker<DateTime?>>();
            await calendar.Instance.ShowPopupAsync();
            var popupElement = calendar.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal("Month", calendar.Instance.CurrentView());
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.ToString(FORMATFULLDATE), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(DateTime.Now.ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(42, parentContainer?.QuerySelectorAll("td").Length);
            Assert.Equal(DateTime.Now.ToString(FORMATFULLDATE), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            parentContainer?.QuerySelector(".e-title")?.Click();
            tableElement = popupElement.QuerySelector("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            var tableContent = popupElement.QuerySelector(".e-content");
            Assert.Contains("e-year", tableContent?.ClassName);
            Assert.Equal(DateTime.Now.ToString("yyyy"), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(12, parentContainer?.QuerySelectorAll("td").Length);
            Assert.Equal(DateTime.Now.ToString("MMM"), parentContainer?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }
    }
}
