using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.Calendar
{
    public class CalendarDefault: BunitTestContext
    {
        private int GetDecade( int year)
        {
            return year < 2000 ? (year / 10 * 10 - 1900) : (year / 10 * 10);
        }

        [Fact(Timeout = 10000, DisplayName = "Initialization: default calendar structure")]
        public void DefaultInitialize()
        {
            var calendar = RenderComponent<SfCalendar<DateTime>>();
            var tableElement = calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Contains("e-calendar", parentContainer?.ClassName);
            Assert.Contains("e-header", parentContainer?.Children[0].ClassName);
            Assert.Contains("e-month", parentContainer?.Children[0].ClassName);
            Assert.Contains("e-content", parentContainer?.Children[1].ClassName);
            Assert.Contains("e-month", parentContainer?.Children[1].ClassName);
            Assert.Contains("e-footer-container", parentContainer?.Children[2].ClassName);
            var buttonList = calendar.FindAll("button");
            var previousButton = buttonList[0];
            var nextButton = buttonList[1];
            var todayButton = buttonList[2];
            Assert.Contains("e-prev", previousButton.ClassName);
            Assert.Contains("e-next", nextButton.ClassName);
            Assert.Contains("e-today", todayButton.ClassName);
            Assert.Equal("Today", todayButton.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Initialization: default structure with nullable value type")]
        public void DefaultInitializeWithNullable()
        {
            var calendar = RenderComponent<SfCalendar<DateTime?>>();
            var tableElement = calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Contains("e-calendar", parentContainer?.ClassName);
            Assert.Contains("e-header", parentContainer?.Children[0].ClassName);
            Assert.Contains("e-month", parentContainer?.Children[0].ClassName);
            Assert.Contains("e-content", parentContainer?.Children[1].ClassName);
            Assert.Contains("e-month", parentContainer?.Children[1].ClassName);
            Assert.Contains("e-footer-container", parentContainer?.Children[2].ClassName);
            var buttonList = calendar.FindAll("button");
            var previousButton = buttonList[0];
            var nextButton = buttonList[1];
            var todayButton = buttonList[2];
            Assert.Contains("e-prev", previousButton.ClassName);
            Assert.Contains("e-next", nextButton.ClassName);
            Assert.Contains("e-today", todayButton.ClassName);
            Assert.Equal("Today", todayButton.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Properties: default values for calendar")]
        public void DefaultValue()
        {
            var calendar = RenderComponent<SfCalendar<DateTime>>();
            Assert.Equal(default(DateTime), calendar.Instance.Value);
            Assert.Equal(default(DateTime[]), calendar.Instance.Values);
            Assert.Null(calendar.Instance.CssClass);
            Assert.False(calendar.Instance.EnablePersistence);
            Assert.False(calendar.Instance.Disabled);
            Assert.False(calendar.Instance.IsMultiSelection);
            Assert.True(calendar.Instance.ShowTodayButton);
            Assert.False(calendar.Instance.WeekNumber);
        }

        [Fact(Timeout = 10000, DisplayName = "Properties: default values with nullable type")]
        public void DefaultValueWithNullable()
        {
            var calendar = RenderComponent<SfCalendar<DateTime?>>();
            Assert.Null(calendar.Instance.Value);
            Assert.Null(calendar.Instance.Values);
            Assert.Null(calendar.Instance.CssClass);
            Assert.False(calendar.Instance.EnablePersistence);
            Assert.False(calendar.Instance.Disabled);
            Assert.False(calendar.Instance.IsMultiSelection);
            Assert.True(calendar.Instance.ShowTodayButton);
            Assert.False(calendar.Instance.WeekNumber);
        }

        [Fact(Timeout = 10000, DisplayName = "Value: predefined single date selection")]
        public void PredefinedValue()
        {
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(val => val.Value, new DateTime(2020,1,1))
            );
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Value);
            var tableElement = calendar.Find("table");
            var selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("1", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(2020, 1, 1).ToString("dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            calendar.SetParametersAndRender(("Value", new DateTime(2020, 1, 2)));
            tableElement = calendar.Find("table");
            selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("2", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(2020, 1, 2).ToString("dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
        }

        [Fact(Timeout = 10000, DisplayName = "Value: predefined single date with nullable type")]
        public void PredefinedValueWithNullable()
        {
            var calendar = RenderComponent<SfCalendar<DateTime?>>(parameters =>
                parameters.Add(val => val.Value, new DateTime(2020, 1, 1))
            );
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Value);
            var tableElement = calendar.Find("table");
            var selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("1", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(2020, 1, 1).ToString("dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            calendar.SetParametersAndRender(("Value", new DateTime(2020, 1, 2)));
            tableElement = calendar.Find("table");
            selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("2", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(2020, 1, 2).ToString("dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
        }

        [Fact(Timeout = 10000, DisplayName = "Values: predefined multiple selections")]
        public void PredefinedValues()
        {
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(val => val.Values, new DateTime[] { new DateTime(2020,1,1), new DateTime(2020,1,2) })
                .Add(multi => multi.IsMultiSelection, true)
            );
            Assert.Equal(new DateTime[] { new DateTime(2020, 1, 1), new DateTime(2020, 1, 2) }, calendar.Instance.Values);
            var tableElement = calendar.Find("table");
            var selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(2, selectedCell.Length);
            Assert.Equal("1", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(2020, 1, 1).ToString("dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            Assert.Equal("2", selectedCell?[1].FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(2020, 1, 2).ToString("dddd, MMMM d, yyyy"), selectedCell?[1].FirstElementChild?.GetAttribute("title"));
            calendar.SetParametersAndRender(("Values", new DateTime[] { new DateTime(2020, 1, 5), new DateTime(2020, 1, 6) }));
            tableElement = calendar.Find("table");
            selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(2, selectedCell.Length);
            Assert.Equal("5", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(2020, 1, 5).ToString("dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            Assert.Equal("6", selectedCell?[1].FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(2020, 1, 6).ToString("dddd, MMMM d, yyyy"), selectedCell?[1].FirstElementChild?.GetAttribute("title"));
        }

        [Fact(Timeout = 10000, DisplayName = "Values: predefined multiple selections with nullable type")]
        public void PredefinedValuesWithNullable()
        {
            var calendar = RenderComponent<SfCalendar<DateTime?>>(parameters =>
                parameters.Add(val => val.Values, new DateTime[] { new DateTime(2020, 1, 1), new DateTime(2020, 1, 2) })
                .Add(multi => multi.IsMultiSelection, true)
            );
            Assert.Equal(new DateTime[] { new DateTime(2020, 1, 1), new DateTime(2020, 1, 2) }, calendar.Instance.Values);
            var tableElement = calendar.Find("table");
            var selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(2, selectedCell.Length);
            Assert.Equal("1", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(2020, 1, 1).ToString("dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            Assert.Equal("2", selectedCell?[1].FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(2020, 1, 2).ToString("dddd, MMMM d, yyyy"), selectedCell?[1].FirstElementChild?.GetAttribute("title"));
            calendar.SetParametersAndRender(("Values", new DateTime[] { new DateTime(2020, 1, 5), new DateTime(2020, 1, 6) }));
            tableElement = calendar.Find("table");
            selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(2, selectedCell.Length);
            Assert.Equal("5", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(2020, 1, 5).ToString("dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            Assert.Equal("6", selectedCell?[1].FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(2020, 1, 6).ToString("dddd, MMMM d, yyyy"), selectedCell?[1].FirstElementChild?.GetAttribute("title"));
        }

        [Fact(Timeout = 10000, DisplayName = "CssClass: applies and updates CSS class")]
        public void CssClass()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.CssClass, "class1"));
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Contains("class1", parentContainer?.ClassName);
            Calendar.SetParametersAndRender(("CssClass", "class2"));
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.DoesNotContain("class1", parentContainer?.ClassName);
            Assert.Contains("class2", parentContainer?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "CssClass: supports hyphenated class names")]
        public void CssClassWithHyphen()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.CssClass, "custom-class"));
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Contains("custom-class", parentContainer?.ClassName);
            Calendar.SetParametersAndRender(("CssClass", "custom-testing-class-1"));
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.DoesNotContain("custom-class", parentContainer?.ClassName);
            Assert.Contains("custom-testing-class-1", parentContainer?.ClassName);
        }
        [Fact(Timeout = 10000, DisplayName = "CssClass: nullable type CSS class behavior")]
        public void CssClassWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>(parameters =>
            parameters.Add(calendar => calendar.CssClass, "class1"));
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Contains("class1", parentContainer?.ClassName);
            Calendar.SetParametersAndRender(("CssClass", "class2"));
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.DoesNotContain("class1", parentContainer?.ClassName);
            Assert.Contains("class2", parentContainer?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "TabIndex: applies and updates tab index")]
        public void TabIndex()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.TabIndex, 1));
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal("1", parentContainer?.GetAttribute("tabindex"));
            Calendar.SetParametersAndRender(("TabIndex", 2));
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.NotEqual("1", parentContainer?.GetAttribute("tabindex"));
            Assert.Equal("2", parentContainer?.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000, DisplayName = "TabIndex: nullable type behavior")]
        public void TabIndexWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>(parameters =>
            parameters.Add(calendar => calendar.TabIndex, 1));
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal("1", parentContainer?.GetAttribute("tabindex"));
            Calendar.SetParametersAndRender(("TabIndex", 2));
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.NotEqual("1", parentContainer?.GetAttribute("tabindex"));
            Assert.Equal("2", parentContainer?.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000, DisplayName = "DayHeaderFormat: supports different formats")]
        public void DayHeaderFormat()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.DayHeaderFormat, DayHeaderFormats.Abbreviated));
            var tableElement = Calendar.Find("table");
            var headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("Sun", headerElement?.QuerySelector("th")?.TextContent);
            Calendar.SetParametersAndRender(("DayHeaderFormat", DayHeaderFormats.Narrow));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement?.QuerySelector("th")?.TextContent);
            Calendar.SetParametersAndRender(("DayHeaderFormat", DayHeaderFormats.Short));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement?.QuerySelector("th")?.TextContent);
            Calendar.SetParametersAndRender(("DayHeaderFormat", DayHeaderFormats.Wide));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("Sunday", headerElement?.QuerySelector("th")?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "DayHeaderFormat: nullable type behavior")]
        public void DayHeaderFormatWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>(parameters =>
            parameters.Add(calendar => calendar.DayHeaderFormat, DayHeaderFormats.Abbreviated));
            var tableElement = Calendar.Find("table");
            var headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("Sun", headerElement?.QuerySelector("th")?.TextContent);
            Calendar.SetParametersAndRender(("DayHeaderFormat", DayHeaderFormats.Narrow));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement?.QuerySelector("th")?.TextContent);
            Calendar.SetParametersAndRender(("DayHeaderFormat", DayHeaderFormats.Short));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement?.QuerySelector("th")?.TextContent);
            Calendar.SetParametersAndRender(("DayHeaderFormat", DayHeaderFormats.Wide));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("Sunday", headerElement?.QuerySelector("th")?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Enabled flag toggles disabled styling")]
        public void Enabled()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>();
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.DoesNotContain("e-disabled", parentContainer?.ClassName);
            Calendar.SetParametersAndRender(("Disabled", true));
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Contains("e-disabled", parentContainer?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Enabled: nullable type behavior")]
        public void EnabledWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>();
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.DoesNotContain("e-disabled", parentContainer?.ClassName);
            Calendar.SetParametersAndRender(("Disabled", true));
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Contains("e-disabled", parentContainer?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "FirstDayOfWeek: updates header order")]
        public void FirstDayOfAWeek()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>();
            var tableElement = Calendar.Find("table");
            var headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement?.QuerySelector("th")?.TextContent);
            Calendar.SetParametersAndRender(("FirstDayOfWeek", 2));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("T", headerElement?.QuerySelector("th")?.TextContent);
            Calendar.SetParametersAndRender(("FirstDayOfWeek", 0));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement?.QuerySelector("th")?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "FirstDayOfWeek: nullable type behavior")]
        public void FirstDayOfAWeekWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>();
            var tableElement = Calendar.Find("table");
            var headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement?.QuerySelector("th")?.TextContent);
            Calendar.SetParametersAndRender(("FirstDayOfWeek", 2));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("T", headerElement?.QuerySelector("th")?.TextContent);
            Calendar.SetParametersAndRender(("FirstDayOfWeek", 0));
            tableElement = Calendar.Find("table");
            headerElement = tableElement.QuerySelector(".e-week-header");
            Assert.Equal("S", headerElement?.QuerySelector("th")?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Min: disables dates before Min")]
        public void MinValue()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(1900,1,1), Calendar.Instance.Min);
            Calendar.SetParametersAndRender(("Min", new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), Calendar.Instance.Min);
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            var buttons = parentContainer?.QuerySelectorAll("button");
            var totalCells = tableElement?.QuerySelectorAll("td");
            var disabledCells = tableElement?.QuerySelectorAll("td.e-disabled");
            var enabledCells = tableElement?.QuerySelectorAll("td:not(.e-disabled)");
            Assert.Equal(3, disabledCells?.Length);
            Assert.Equal(totalCells?.Length, (disabledCells?.Length + enabledCells?.Length));
            Assert.Contains("e-prev", buttons?[0].ClassName);
            Assert.Contains("e-disabled", buttons?[0].ClassName);
            Assert.Contains("e-next", buttons?[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttons?[1].ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Min: nullable type navigation disables previous")]
        public async Task MinValueWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020,1,1)));
            Assert.Equal(new DateTime(1900, 1, 1), Calendar.Instance.Min);
            Calendar.SetParametersAndRender(("Min", new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), Calendar.Instance.Min);
            await Calendar.Instance.NavigateAsync(CalendarView.Month, Calendar.Instance.Min);
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            var buttons = parentContainer?.QuerySelectorAll("button");
            var totalCells = tableElement?.QuerySelectorAll("td");
            var disabledCells = tableElement?.QuerySelectorAll("td.e-disabled");
            var enabledCells = tableElement?.QuerySelectorAll("td:not(.e-disabled)");
            Assert.Equal(3, disabledCells?.Length);
            Assert.Equal(totalCells?.Length, (disabledCells?.Length + enabledCells?.Length));
            Assert.Contains("e-prev", buttons?[0].ClassName);
            Assert.Contains("e-disabled", buttons?[0].ClassName);
            Assert.Contains("e-next", buttons?[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttons?[1].ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Navigation: navigate to month view with Min value")]
        public async Task NavigateCalendarToMonthviewWithMinValue()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
            parameters.Add(Cal => Cal.Min, new DateTime(2020, 3, 1)));
            var tableElement = Calendar.Find("table");
            Assert.Equal(new DateTime(2020, 3, 1), Calendar.Instance.Min);
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            await Calendar.Instance.NavigateAsync(CalendarView.Year, Calendar.Instance.Min);
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            tableElement = Calendar.Find("table");
            Assert.Equal(3, tableElement.QuerySelectorAll("tr").Length);
            Assert.Equal(12, tableElement.QuerySelectorAll("td").Length);
            Assert.Equal(Calendar.Instance.Min.ToString("MMM"), tableElement?.QuerySelector("td.e-focused-date")?.FirstElementChild?.TextContent);
            Assert.Equal(2, tableElement?.QuerySelectorAll("td.e-disabled").Length);
            Assert.Equal("Jan", tableElement?.QuerySelectorAll("td.e-disabled")?[0].FirstElementChild?.TextContent);
            Assert.Equal("Feb", tableElement?.QuerySelectorAll("td.e-disabled")?[1].FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Max: disables dates after Max")]
        public void MaxValue()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)));
            Calendar.SetParametersAndRender(("Max", new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), Calendar.Instance.Max);
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            var buttons = parentContainer?.QuerySelectorAll("button");
            var totalCells = tableElement?.QuerySelectorAll("td");
            var disabledCells = tableElement?.QuerySelectorAll("td.e-disabled");
            var enabledCells = tableElement?.QuerySelectorAll("td:not(.e-disabled)");
            Assert.Equal(38, disabledCells?.Length);
            Assert.Equal(totalCells?.Length, (disabledCells?.Length + enabledCells?.Length));
            Assert.Contains("e-prev", buttons?[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttons?[0].ClassName);
            Assert.Contains("e-next", buttons?[1].ClassName);
            Assert.Contains("e-disabled", buttons?[1].ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Max: nullable type behavior")]
        public void MaxValueWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2099, 12, 31), Calendar.Instance.Max);
            Calendar.SetParametersAndRender(("Max", new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), Calendar.Instance.Max);
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            var buttons = parentContainer?.QuerySelectorAll("button");
            var totalCells = tableElement?.QuerySelectorAll("td");
            var disabledCells = tableElement?.QuerySelectorAll("td.e-disabled");
            var enabledCells = tableElement?.QuerySelectorAll("td:not(.e-disabled)");
            Assert.Equal(38, disabledCells?.Length);
            Assert.Equal(totalCells?.Length, (disabledCells?.Length + enabledCells?.Length));
            Assert.Contains("e-prev", buttons?[0].ClassName);
            Assert.DoesNotContain("e-disabled", buttons?[0].ClassName);
            Assert.Contains("e-next", buttons?[1].ClassName);
            Assert.Contains("e-disabled", buttons?[1].ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "WeekNumber: toggles week number column")]
        public void WeekNumber()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>();
            Assert.False(Calendar.Instance.WeekNumber);
            Calendar.SetParametersAndRender(("WeekNumber", true));
            Assert.True(Calendar.Instance.WeekNumber);
            var tableElement = Calendar.Find("table");
            var weekCells = tableElement.QuerySelectorAll("td.e-week-number");
            Assert.InRange(weekCells.Length, 5, 6);
        }

        [Fact(Timeout = 10000, DisplayName = "WeekNumber: nullable type behavior")]
        public void WeekNumberWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>();
            Assert.False(Calendar.Instance.WeekNumber);
            Calendar.SetParametersAndRender(("WeekNumber", true));
            Assert.True(Calendar.Instance.WeekNumber);
            var tableElement = Calendar.Find("table");
            var weekCells = tableElement.QuerySelectorAll("td.e-week-number");
            Assert.InRange(weekCells.Length, 5, 6);
        }

        [Fact(Timeout = 10000, DisplayName = "TodayButton: shows and hides today button")]
        public void TodayButton()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>();
            Assert.True(Calendar.Instance.ShowTodayButton);
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            var buttons = parentContainer?.QuerySelectorAll("button");
            Assert.Equal(3, buttons?.Length);
            Assert.Contains("e-today", buttons?[2].ClassName);
            Assert.Contains("e-btn", buttons?[2].ClassName);
            Assert.Equal("Today", buttons?[2].TextContent);
            Calendar.SetParametersAndRender(("ShowTodayButton", false));
            Assert.False(Calendar.Instance.ShowTodayButton);
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttons = parentContainer?.QuerySelectorAll("button");
            Assert.Equal(2, buttons?.Length);
        }
        
        [Fact(Timeout = 10000, DisplayName = "TodayButton: nullable type behavior")]
        public void TodayButtonWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>();
            Assert.True(Calendar.Instance.ShowTodayButton);
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            var buttons = parentContainer?.QuerySelectorAll("button");
            Assert.Equal(3, buttons?.Length);
            Assert.Contains("e-today", buttons?[2].ClassName);
            Assert.Contains("e-btn", buttons?[2].ClassName);
            Assert.Equal("Today", buttons?[2].TextContent);
            Calendar.SetParametersAndRender(("ShowTodayButton", false));
            Assert.False(Calendar.Instance.ShowTodayButton);
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttons = parentContainer?.QuerySelectorAll("button");
            Assert.Equal(2, buttons?.Length);
        }

        [Fact(Timeout = 10000, DisplayName = "WeekRules: configures week numbering")]
        public void WeekRules()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>(parameters=>
            parameters.Add(Cal => Cal.WeekNumber, true)
            .Add(Cal => Cal.WeekRule, System.Globalization.CalendarWeekRule.FirstDay)
            .Add(Cal => Cal.Value, new DateTime(2020,1,1)));
        }

        [Fact(Timeout = 10000, DisplayName = "View: start and depth set to Month")]
        public void StartAndDepthAsMonth()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
            parameters.Add(Cal => Cal.Start, CalendarView.Month)
            .Add(Cal => Cal.Depth, CalendarView.Month)
            .Add(Cal => Cal.Value, new DateTime(2020, 1, 1)));
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(CalendarView.Month, Calendar.Instance.Start);
            Assert.Equal(CalendarView.Month, Calendar.Instance.Depth);
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            Assert.Contains("January", parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(42, tableElement?.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.Equal("1", tableElement?.QuerySelectorAll("td.e-focused-date")?[0].FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "View: start and depth Month with nullable type")]
        public void StartAndDepthAsMonthWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>(parameters =>
            parameters.Add(Cal => Cal.Start, CalendarView.Month)
            .Add(Cal => Cal.Depth, CalendarView.Month));
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(CalendarView.Month, Calendar.Instance.Start);
            Assert.Equal(CalendarView.Month, Calendar.Instance.Depth);
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            Assert.Contains(DateTime.Now.ToString("MMMM"), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(42, tableElement?.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.Equal(DateTime.Now.Day.ToString(), tableElement?.QuerySelectorAll("td.e-focused-date")?[0].FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "View: start and depth set to Year")]
        public void StartAndDepthAsYear()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
            parameters.Add(Cal => Cal.Start, CalendarView.Year)
            .Add(Cal => Cal.Depth, CalendarView.Year)
            .Add(Cal => Cal.Value, new DateTime(2021, 1, 1)));
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(CalendarView.Year, Calendar.Instance.Start);
            Assert.Equal(CalendarView.Year, Calendar.Instance.Depth);
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            Assert.Contains("2021", parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(12, tableElement?.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal("Jan", tableElement?.QuerySelectorAll("td.e-selected")?[0].FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "View: start and depth Year with nullable type")]
        public void StartAndDepthAsYearWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>(parameters =>
            parameters.Add(Cal => Cal.Start, CalendarView.Year)
            .Add(Cal => Cal.Depth, CalendarView.Year));
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(CalendarView.Year, Calendar.Instance.Start);
            Assert.Equal(CalendarView.Year, Calendar.Instance.Depth);
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            Assert.Contains(DateTime.Now.ToString("yyyy"), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(12, tableElement?.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.Equal(DateTime.Now.ToString("MMM"), tableElement?.QuerySelectorAll("td.e-focused-date")?[0].FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "View: start and depth set to Decade")]
        public void StartAndDepthAsDecade()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
            parameters.Add(Cal => Cal.Start, CalendarView.Decade)
            .Add(Cal => Cal.Depth, CalendarView.Decade)
            .Add(Cal => Cal.Value, new DateTime(1900, 1, 1)));
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(CalendarView.Decade, Calendar.Instance.Start);
            Assert.Equal(CalendarView.Decade, Calendar.Instance.Depth);
            Assert.Equal("Decade", Calendar.Instance.CurrentView());
            Assert.Contains("1900 - 1909", parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(12, tableElement?.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-selected").Length);
            Assert.Equal("1900", tableElement?.QuerySelectorAll("td.e-selected")?[0].FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "View: start and depth Decade with nullable type")]
        public void StartAndDepthAsDecadeWithNullable()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime?>>(parameters =>
            parameters.Add(Cal => Cal.Start, CalendarView.Decade)
            .Add(Cal => Cal.Depth, CalendarView.Decade));
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(CalendarView.Decade, Calendar.Instance.Start);
            Assert.Equal(CalendarView.Decade, Calendar.Instance.Depth);
            Assert.Equal("Decade", Calendar.Instance.CurrentView());
            Assert.Contains(this.GetDecade(DateTime.Now.Year)+" - "+ (this.GetDecade(DateTime.Now.Year)+9), parentContainer?.QuerySelector(".e-title")?.TextContent);
            Assert.Equal(12, tableElement?.QuerySelectorAll("td").Length);
            Assert.Equal(1, tableElement?.QuerySelectorAll("td.e-focused-date").Length);
            Assert.Equal(DateTime.Now.Year.ToString() , tableElement?.QuerySelectorAll("td.e-focused-date")?[0].FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Persistence: get and remove persisted dates")]
        public async Task GetAndRemoveData()
        {
            Expression<Func<DateTime>> dummyExpression = () => TestData.GetDummyDates();
            var dummyValueChanged = new EventCallback<DateTime[]>();
            var dummyHTMLAttr = new Dictionary<string, object>();
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters
                .Add(val => val.Value, new DateTime(2020, 1, 1))
                .Add(val => val.ValueExpression , dummyExpression)
                .Add(val => val.ValuesChanged, dummyValueChanged)
                .Add(val => val.HtmlAttributes, dummyHTMLAttr)
            );
            DateTime[] newValues = new DateTime[]
            {
                new DateTime(2020, 1, 1),
            };
            Assert.Equal(new DateTime(2020, 1, 1), calendar.Instance.Value);
            var tableElement = calendar.Find("table");
            var selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("1", selectedCell?[0].FirstElementChild?.TextContent);
            await calendar.Instance.GetPersistDataAsync();
            await calendar.Instance.RemoveDatesAsync(newValues);
        }

        // Further customization: when persistence is enabled, calendar should call setItem to persist value
        [Fact(Timeout = 10000, DisplayName = "EnablePersistence writes to localStorage when value changes")]
        public void EnablePersistence_WritesPersistedValue_OnValueChange()
        {
            JSInterop.SetupVoid("window.localStorage.setItem").SetVoidResult();
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.Add(p => p.EnablePersistence, true));
            var buttons = calendar.FindAll("button");
            Assert.True(buttons.Count >= 3, "expected navigation + today buttons to be present");
            buttons[2].Click();
            Assert.True(JSInterop.Invocations.Any(i => i.Identifier == "setLocalStorageItem"), "Expected localStorage.setItem to be invoked");
        }

        public class TestData
        {
            public static DateTime GetDummyDates()
            {
                return new DateTime(2024, 8, 7);
            }
        }
    }
}
