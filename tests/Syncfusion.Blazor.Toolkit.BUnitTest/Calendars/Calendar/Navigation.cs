using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.Calendar
{
    public class Navigation: BunitTestContext
    {
        private int GetDecade(int year)
        {
            return (year / 10 * 10);
        }

        [Fact(Timeout = 10000)]
        public void CurrentMonthToNextMonthNavigation()
        {
            var component = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.
            Add(Calendar => Calendar.Value, new DateTime(1900, 1, 1)));
            var tableElement = component.Find("table");
            Assert.Equal("Month", component.Instance.CurrentView());
            var buttonList = component.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900,1,1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            buttonList[1].Click();
            tableElement = component.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = component.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(1900, 2, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 2, 1).ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(new DateTime(1900, 2, 1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: current month to previous month")]
        public void CurrentMonthToPreviousMonthNavigation()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.
            Add(Calendar => Calendar.Value, new DateTime(1900, 1, 1)));
            var tableElement = Calendar.Find("table");
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            buttonList[1].Click();
            buttonList = Calendar.FindAll("button");
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(1900, 2, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 2, 1).ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(new DateTime(1900, 2, 1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            buttonList = Calendar.FindAll("button");
            buttonList[0].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(new DateTime(1900, 1, 1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: nullable current month to next month")]
        public void CurrentMonthToNextMonthNavigationWithNulable()
        {
            var component = RenderComponent<SfCalendar<DateTime?>>();
            var tableElement = component.Find("table");
            Assert.Equal("Month", component.Instance.CurrentView());
            var buttonList = component.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(DateTime.Now.ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            buttonList[1].Click();
            tableElement = component.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = component.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(DateTime.Now.AddMonths(1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.AddMonths(1).ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(DateTime.Now.AddMonths(1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: nullable current month to previous month")]
        public void CurrentMonthToPreviousMonthNavigationWithNullable()
        {
            var component = RenderComponent<SfCalendar<DateTime?>>();
            var tableElement = component.Find("table");
            Assert.Equal("Month", component.Instance.CurrentView());
            var buttonList = component.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(DateTime.Now.ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            buttonList[1].Click();
            buttonList = component.FindAll("button");
            tableElement = component.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(DateTime.Now.AddMonths(1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.AddMonths(1).ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(DateTime.Now.AddMonths(1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            var dayvalue = DateTime.Now.AddMonths(1).Day.ToString();
            buttonList = component.FindAll("button");
            buttonList[0].Click();
            tableElement = component.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(DateTime.Now.ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(dayvalue, tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: current year to next year")]
        public void CurrentYearToNextYearNavigation()
        {
            var component = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Year).
            Add(Cal => Cal.Depth, CalendarView.Year).
            Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            var tableElement = component.Find("table");
            Assert.Equal("Year", component.Instance.CurrentView());
            var buttonList = component.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMM"), tableElement?.QuerySelector(".e-selected")?.FirstElementChild?.TextContent);
            buttonList[1].Click();
            tableElement = component.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = component.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(1).ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: nullable current year to next year")]
        public void CurrentYearToNextYearNavigationWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Year).
            Add(Cal => Cal.Depth, CalendarView.Year));
            var tableElement = Calendar.Find("table");
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(DateTime.Now.ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            buttonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(DateTime.Now.AddYears(1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.AddYears(1).ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: current year to previous year")]
        public void CurrentYearToPreviousYear()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Year).
            Add(Cal => Cal.Depth, CalendarView.Year).
            Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            var tableElement = Calendar.Find("table");
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMM"), tableElement?.QuerySelector(".e-selected")?.FirstElementChild?.TextContent);
            buttonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(1).ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            buttonList[0].Click();
            buttonList = Calendar.FindAll("button");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMM"), tableElement?.QuerySelector(".e-selected")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: nullable current year to previous year")]
        public void CurrentYearToPreviousYearWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Year).
            Add(Cal => Cal.Depth, CalendarView.Year));
            var tableElement = Calendar.Find("table");
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(DateTime.Now.ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            buttonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(DateTime.Now.AddYears(1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.AddYears(1).ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            buttonList[0].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.Equal(DateTime.Now.ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: current decade to next decade")]
        public void CurrentDecadeToNextDecade()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Decade).
            Add(Cal => Cal.Depth, CalendarView.Decade)
            .Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            var tableElement = Calendar.Find("table");
            Assert.Equal("Decade", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(this.GetDecade(new DateTime(1900, 1, 1).Year)+" - "+((this.GetDecade(new DateTime(1900, 1, 1).Year)+9)), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), tableElement?.QuerySelector(".e-selected")?.FirstElementChild?.TextContent);
            buttonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(this.GetDecade(new DateTime(1900, 1, 1).AddYears(10).Year) + " - " + ((this.GetDecade(new DateTime(1900, 1, 1).AddYears(10).Year) + 9)), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(10).ToString("yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: nullable current decade to next decade")]
        public void CurrentDecadeToNextDecadeWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Decade).
            Add(Cal => Cal.Depth, CalendarView.Decade));
            var tableElement = Calendar.Find("table");
            Assert.Equal("Decade", Calendar.Instance.CurrentView());
            var buttonList = Calendar.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(this.GetDecade(DateTime.Now.Year) + " - " + (this.GetDecade(DateTime.Now.Year) + 9), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(DateTime.Now.ToString("yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            buttonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = Calendar.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(this.GetDecade(DateTime.Now.AddYears(10).Year) + " - " + (this.GetDecade(DateTime.Now.AddYears(10).Year)+9), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(DateTime.Now.AddYears(10).ToString("yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: month to year view via title header")]
        public void MonthToYearNavigationTitleHeader()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Value, new DateTime(1900,1,1)));
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            Assert.Equal(new DateTime(1900, 1, 1).Day.ToString(), tableElement?.QuerySelector(".e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(42, parentContainer?.QuerySelectorAll("td").Length);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            parentContainer?.QuerySelector(".e-title")?.Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(12, parentContainer?.QuerySelectorAll("td").Length);
            Assert.Equal(new DateTime(1900, 1, 1).ToString("MMM"), parentContainer?.QuerySelector(".e-selected")?.FirstElementChild?.TextContent);
        }
        
        [Fact(Timeout = 10000, DisplayName = "Navigation: month to year view via title header (nullable)")]
        public void MonthToYearNavigationTitleHeaderWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>();
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            Assert.Equal(DateTime.Now.ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(DateTime.Now.ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(42, parentContainer?.QuerySelectorAll("td").Length);
            Assert.Equal(DateTime.Now.ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            parentContainer?.QuerySelector(".e-title")?.Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            Assert.Equal(DateTime.Now.ToString("yyyy"), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(12, parentContainer?.QuerySelectorAll("td").Length);
            Assert.Equal(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MMM"), parentContainer?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }
    }
}
