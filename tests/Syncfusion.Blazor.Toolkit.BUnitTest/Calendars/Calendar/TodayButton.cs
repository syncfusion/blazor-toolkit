using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.Calendar
{
    public class TodayButton: BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "TodayButton: click selects today's date")]
        public void TodayButtonClick()
        {
            var component = RenderComponent<SfCalendar<DateTime>>();
            Assert.Equal(default(DateTime), component.Instance.Value);
            var tableElement = component.Find("table");
            Assert.Equal(0, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            var selectedDate = component.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            var buttonList = component.FindAll("button");
            buttonList[2].Click();
            tableElement = component.Find("table");
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector("td.e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.True(tableElement?.QuerySelector("td.e-selected")?.ClassList.Contains("e-today"));
        }

        [Fact(Timeout = 10000, DisplayName = "TodayButton: click selects today's date (nullable)")]
        public void TodayButtonClickWithNullable()
        {
            var component = RenderComponent<SfCalendar<DateTime?>>();
            Assert.Null(component.Instance.Value);
            var tableElement = component.Find("table");
            Assert.Equal(0, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-focused-date").Length);
            var selectedDate = component.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            var buttonList = component.FindAll("button");
            buttonList[2].Click();
            tableElement = component.Find("table");
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector("td.e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.True(tableElement?.QuerySelector("td.e-selected")?.ClassList.Contains("e-today"));
        }

        [Fact(Timeout = 10000, DisplayName = "TodayButton: click in other views navigates and selects today")]
        public void TodayButtonClickInOtherViews()
        {
            var component = RenderComponent<SfCalendar<DateTime>>();
            Assert.Equal(default(DateTime), component.Instance.Value);
            var tableElement = component.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(0, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            var selectedDate = component.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            Assert.Equal("Month", component.Instance.CurrentView());
            // Today button click in Year view
            parentContainer?.QuerySelector(".e-title")?.Click();
            Assert.Equal("Year", component.Instance.CurrentView());
            var buttonList = component.FindAll("button");
            buttonList[2].Click();
            tableElement = component.Find("table");
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector("td.e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.True(tableElement?.QuerySelector("td.e-selected")?.ClassList.Contains("e-today"));
            Assert.Equal("Month", component.Instance.CurrentView());
            // Today button click in Decade View
            parentContainer = tableElement?.ParentElement?.ParentElement;
            parentContainer?.QuerySelector(".e-title")?.Click();
            Assert.Equal("Year", component.Instance.CurrentView());
            parentContainer = tableElement?.ParentElement?.ParentElement;
            parentContainer?.QuerySelector(".e-title")?.Click();
            Assert.Equal("Decade", component.Instance.CurrentView());
            buttonList[2].Click();
            tableElement = component.Find("table");
            Assert.Equal("Month", component.Instance.CurrentView());
            Assert.Equal(1, tableElement.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelector("td.e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.True(tableElement?.QuerySelector("td.e-selected")?.ClassList.Contains("e-today"));
        }

        [Fact(Timeout = 10000, DisplayName = "TodayButton: behavior when Start and Depth are Year")]
        public void TodayButtonClickWithStartAndDepthAsYear()
        {
            var component = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Year).
            Add(Cal => Cal.Depth, CalendarView.Year));
            Assert.Equal(default(DateTime), component.Instance.Value);
            var tableElement = component.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(0, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            var selectedDate = component.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            Assert.Equal("Year", component.Instance.CurrentView());
            var buttonList = component.FindAll("button");
            buttonList[2].Click();
            Assert.Equal("Year", component.Instance.CurrentView());
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement?.QuerySelector("td.e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(0, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
        }

        [Fact(Timeout = 10000, DisplayName = "TodayButton: behavior when Start and Depth are Decade")]
        public void TodayButtonClickWithStartAndDepthAsDecade()
        {
            var component = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.
            Add(Cal => Cal.Start, CalendarView.Decade).
            Add(Cal => Cal.Depth, CalendarView.Decade));
            Assert.Equal(default(DateTime), component.Instance.Value);
            var tableElement = component.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(0, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            var selectedDate = component.Find("table").QuerySelector("td.e-selected");
            Assert.Null(selectedDate);
            Assert.Equal("Decade", component.Instance.CurrentView());
            var buttonList = component.FindAll("button");
            buttonList[2].Click();
            Assert.Equal("Decade", component.Instance.CurrentView());
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal(DateTime.Now.ToString("yyyy"), tableElement?.QuerySelector("td.e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(0, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
        }
    }
}
