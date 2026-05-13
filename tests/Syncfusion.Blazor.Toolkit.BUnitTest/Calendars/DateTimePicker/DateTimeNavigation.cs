using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.DateTimePicker
{
    public class DateTimeNavigation : BunitTestContext
    {
        const string FORMATFULLDATE = "dddd, MMMM d, yyyy";
        const string FORMATDATE = " d ";
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
        private string getDecadeTitle(DateTime localDate)
        {
            int localYr = localDate.Year;
            localYr = localYr < 10 ? 10 : localYr;
            int startYr = localYr - localYr % 10;
            int endYr = startYr + (10 - 1);
            string startHdrYr = GetDateFormat(new DateTime(startYr, 1, 1), FORMAT_YEAR);
            string endHdrYr = GetDateFormat(new DateTime(endYr, 1, 1), FORMAT_YEAR);
            return startHdrYr + TITLE_SEPARATOR + endHdrYr;
        }
        [Fact(Timeout = 10000)]
        public async Task CurrentMonthToNextMonthNavigation()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).Day.ToString(), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.GetAttribute("title"));
            buttonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(1900, 2, 1).ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Equal(new DateTime(1900, 2, 1).ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.GetAttribute("title"));
            Assert.Equal(new DateTime(1900, 2, 1).Day.ToString(), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task CurrentMonthToPreviousMonthNavigation()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).Day.ToString(), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.GetAttribute("title"));
            buttonList[1].Click();
            buttonList = Calendar.FindAll("button");
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(1900, 2, 1).ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Equal(new DateTime(1900, 2, 1).ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.GetAttribute("title"));
            Assert.Equal(new DateTime(1900, 2, 1).Day.ToString(), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
            buttonList = Calendar.FindAll("button");
            buttonList[0].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.GetAttribute("title"));
            Assert.Equal(new DateTime(1900, 1, 1).Day.ToString(), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task CurrentMonthToNextMonthNavigationWithNulable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>();
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(DateTime.Now.ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
            Assert.Equal(DateTime.Now.ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.GetAttribute("title"));
            buttonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(DateTime.Now.AddMonths(1).ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Equal(DateTime.Now.AddMonths(1).ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.GetAttribute("title"));
            Assert.Equal(DateTime.Now.AddMonths(1).Day.ToString(), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task CurrentMonthToPreviousMonthNavigationWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>();
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(DateTime.Now.ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
            Assert.Equal(DateTime.Now.ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.GetAttribute("title"));
            buttonList[1].Click();
            buttonList = Calendar.FindAll("button");
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(DateTime.Now.AddMonths(1).ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Equal(DateTime.Now.AddMonths(1).ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.GetAttribute("title"));
            Assert.Equal(DateTime.Now.AddMonths(1).Day.ToString(), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
            buttonList = Calendar.FindAll("button");
            buttonList[0].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(DateTime.Now.ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task CurrentYearToNextYearNavigation()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Year).
            Add(Cal => Cal.Depth, CalendarView.Year).
            Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMM"), tableElement.QuerySelector(".e-selected").FirstElementChild.TextContent);
            buttonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(1).ToString("yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(1).ToString("MMM"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task CurrentYearToNextYearNavigationWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Year).
            Add(Cal => Cal.Depth, CalendarView.Year));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(DateTime.Now.ToString("yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
            buttonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(DateTime.Now.AddYears(1).ToString("yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Equal(DateTime.Now.AddYears(1).ToString("MMM"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task CurrentYearToPreviousYear()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Year).
            Add(Cal => Cal.Depth, CalendarView.Year).
            Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMM"), tableElement.QuerySelector(".e-selected").FirstElementChild.TextContent);
            buttonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(1).ToString("yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(1).ToString("MMM"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            buttonList[0].Click();
            buttonList = Calendar.FindAll("button");
            parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMM"), tableElement.QuerySelector(".e-selected").FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task CurrentYearToPreviousYearWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Year).
            Add(Cal => Cal.Depth, CalendarView.Year));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(DateTime.Now.ToString("yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
            buttonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(DateTime.Now.AddYears(1).ToString("yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            //Assert.Null(tableElement.QuerySelector(".e-focused-date").FirstElementChild.GetAttribute("title"));
            Assert.Equal(DateTime.Now.AddYears(1).ToString("MMM"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
            buttonList[0].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.Equal(DateTime.Now.ToString("yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task CurrentDecadeToNextDecade()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Decade).
            Add(Cal => Cal.Depth, CalendarView.Decade).
            Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Decade", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(this.getDecadeTitle(new DateTime(1900, 1, 1)), parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), tableElement.QuerySelector(".e-selected").FirstElementChild.TextContent);
            buttonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(this.getDecadeTitle(new DateTime(1910, 1, 1)), parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(10).ToString("yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task CurrentDecadeToNextDecadeWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Decade).
            Add(Cal => Cal.Depth, CalendarView.Decade));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Decade", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(this.getDecadeTitle(DateTime.Now), parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.ToString("yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
            buttonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(this.getDecadeTitle(DateTime.Now.AddYears(10)), parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Equal(DateTime.Now.AddYears(10).ToString("yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
        }

        [Fact(Timeout = 10000)]
        public async Task MonthToYearNavigationTitleHeader()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            Assert.Equal(new DateTime(1900, 1, 1).Day.ToString(), tableElement.QuerySelector(".e-selected").FirstElementChild.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-selected").FirstElementChild.GetAttribute("title"));
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Equal(42, parentContainer.QuerySelectorAll("td").Length);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-selected").FirstElementChild.GetAttribute("title"));
            parentContainer.QuerySelector(".e-title").Click();
            await Task.Delay(200);
            popupEle = Calendar.Find(".e-popup");
            tableElement = popupEle.QuerySelector("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            var tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-year", tContent.ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Equal(12, parentContainer.QuerySelectorAll("td").Length);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMM"), parentContainer.QuerySelector(".e-selected").FirstElementChild.TextContent);
        }
        [Fact(Timeout = 10000)]
        public async Task MonthToYearNavigationTitleHeaderWithNullable()
        {
            var Calendar = RenderComponent<SfDateTimePicker<DateTime?>>();
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
            Assert.Equal(DateTime.Now.ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.GetAttribute("title"));
            Assert.Equal(DateTime.Now.ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Equal(42, parentContainer.QuerySelectorAll("td").Length);
            Assert.Equal(DateTime.Now.ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-focused-date").FirstElementChild.GetAttribute("title"));
            parentContainer.QuerySelector(".e-title").Click();
            tableElement = popupEle.QuerySelector("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            var tContent = popupEle.QuerySelector(".e-content");
            Assert.Contains("e-year", tContent.ClassName);
            Assert.Equal(DateTime.Now.ToString("yyyy"), parentContainer.QuerySelector(".e-title").TextContent);
            Assert.Equal(12, parentContainer.QuerySelectorAll("td").Length);
            Assert.Equal(DateTime.Now.ToString("MMM"), parentContainer.QuerySelector(".e-focused-date").FirstElementChild.TextContent);
        }

    }
}
