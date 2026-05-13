using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.Calendar
{
    public class DateOnlyCalendar : BunitTestContext
    {
#if NET7_0_OR_GREATER
        [Fact(Timeout = 10000, DisplayName = "Initialization: DateOnly default structure")]
        public void DefaultInitialize()
        {
            var calendar = RenderComponent<SfCalendar<DateOnly>>();
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

        [Fact(Timeout = 10000, DisplayName = "Min: DateOnly - disables dates before Min")]
        public void MinValue()
        {
            var component = RenderComponent<SfCalendar<DateOnly>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateOnly(2020, 1, 1)));
            Assert.Equal(new DateTime(1900, 1, 1), component.Instance.Min);
            component.SetParametersAndRender(("Min", new DateTime(2020, 1, 1)));
            Assert.Equal(new DateTime(2020, 1, 1), component.Instance.Min);
            var tableElement = component.Find("table");
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

        [Fact(Timeout = 10000, DisplayName = "Max: DateOnly - disables dates after Max")]
        public void MaxValue()
        {
            var Calendar = RenderComponent<SfCalendar<DateOnly>>(parameters =>
            parameters.Add(calendar => calendar.Value, new DateOnly(2020, 1, 1)));
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

        [Fact(Timeout = 10000, DisplayName = "Navigation: DateOnly current month to next month")]
        public void CurrentMonthToNextMonthNavigation()
        {
            var component = RenderComponent<SfCalendar<DateOnly>>(parameters => parameters.
            Add(Calendar => Calendar.Value, new DateOnly(1900, 1, 1)));
            var tableElement = component.Find("table");
            Assert.Equal("Month", component.Instance.CurrentView());
            var buttonList = component.FindAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateOnly(1900, 1, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-disabled", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateOnly(1900, 1, 1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateOnly(1900, 1, 1).ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            buttonList[1].Click();
            tableElement = component.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            buttonList = component.FindAll("button");
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateOnly(1900, 2, 1).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateOnly(1900, 2, 1).ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.GetAttribute("title"));
            Assert.Equal(new DateOnly(1900, 2, 1).Day.ToString(), tableElement?.QuerySelector(".e-focused-date")?.FirstElementChild?.TextContent);
        }
        
        [Fact(Timeout = 10000, DisplayName = "CurrentView: DateOnly toggles views")]
        public void CurrentViewMethod()
        {
            var Calendar = RenderComponent<SfCalendar<DateOnly>>();
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            parentContainer?.QuerySelector(".e-title")?.Click();
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            parentContainer?.QuerySelector(".e-title")?.Click();
            Assert.Equal("Decade", Calendar.Instance.CurrentView());
        }

        [Fact(Timeout = 10000, DisplayName = "Initialization: DateOnly nullable default structure")]
        public void DefaultInitializeWithNullable()
        {
            var calendar = RenderComponent<SfCalendar<DateOnly?>>();
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

        [Fact(Timeout = 10000, DisplayName = "Properties: DateOnly default values")]
        public void DefaultValue()
        {
            var calendar = RenderComponent<SfCalendar<DateOnly>>();
            Assert.Equal(default(DateOnly), calendar.Instance.Value);
            Assert.Null(calendar.Instance.CssClass);
            Assert.False(calendar.Instance.EnablePersistence);
            Assert.False(calendar.Instance.Disabled);
            Assert.False(calendar.Instance.IsMultiSelection);
            Assert.True(calendar.Instance.ShowTodayButton);
            Assert.False(calendar.Instance.WeekNumber);
        }

        [Fact(Timeout = 10000, DisplayName = "Properties: DateOnly nullable default values")]
        public void DefaultValueWithNullable()
        {
            var calendar = RenderComponent<SfCalendar<DateOnly?>>();
            Assert.Null(calendar.Instance.Value);
            Assert.Null(calendar.Instance.Values);
            Assert.Null(calendar.Instance.CssClass);
            Assert.False(calendar.Instance.EnablePersistence);
            Assert.False(calendar.Instance.Disabled);
            Assert.False(calendar.Instance.IsMultiSelection);
            Assert.True(calendar.Instance.ShowTodayButton);
            Assert.False(calendar.Instance.WeekNumber);
        }

        [Fact(Timeout = 10000, DisplayName = "Value: DateOnly predefined selection")]
        public void PredefinedValue()
        {
            var calendar = RenderComponent<SfCalendar<DateOnly>>(parameters =>
                parameters.Add(val => val.Value, new DateOnly(2020, 1, 1))
            );
            Assert.Equal(new DateOnly(2020, 1, 1), calendar.Instance.Value);
            var tableElement = calendar.Find("table");
            var selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("1", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(new DateOnly(2020, 1, 1).ToString("dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            calendar.SetParametersAndRender(("Value", new DateOnly(2020, 1, 2)));
            tableElement = calendar.Find("table");
            selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("2", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(new DateOnly(2020, 1, 2).ToString("dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
        }

        [Fact(Timeout = 10000, DisplayName = "Value: DateOnly predefined selection with nullable")]
        public void PredefinedValueWithNullable()
        {
            var calendar = RenderComponent<SfCalendar<DateOnly?>>(parameters =>
                parameters.Add(val => val.Value, new DateOnly(2020, 1, 1))
            );
            Assert.Equal(new DateOnly(2020, 1, 1), calendar.Instance.Value);
            var tableElement = calendar.Find("table");
            var selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("1", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(new DateOnly(2020, 1, 1).ToString("dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
            calendar.SetParametersAndRender(("Value", new DateOnly(2020, 1, 2)));
            tableElement = calendar.Find("table");
            selectedCell = tableElement.QuerySelectorAll(".e-selected");
            Assert.Equal(1, selectedCell.Length);
            Assert.Equal("2", selectedCell?[0].FirstElementChild?.TextContent);
            Assert.Equal(new DateOnly(2020, 1, 2).ToString("dddd, MMMM d, yyyy"), selectedCell?[0].FirstElementChild?.GetAttribute("title"));
        }
#endif
    }
}
