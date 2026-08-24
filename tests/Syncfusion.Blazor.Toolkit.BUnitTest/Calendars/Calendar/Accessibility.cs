using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.Calendar
{
    public class Accessibility: BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "Accessibility: Default ARIA attributes")]
        public void DefaultAccessibility()
        {
            var component = RenderComponent<SfCalendar<DateTime>>();
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("MMMM yyyy");
            var tableElement = component.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            var buttonList = component.FindAll("button");
            // Calendar Title Accessibility
            var headerElement = parentContainer?.QuerySelector(".e-title");
            Assert.Equal("true", headerElement?.GetAttribute("aria-atomic"));
            Assert.Equal(("Calendar view title "+ formattedDate), headerElement?.GetAttribute("aria-label"));
            // Previous button next button Accessibility
            Assert.Equal("false", buttonList[1].GetAttribute("aria-disabled"));
            Assert.Equal("Previous month", buttonList[1].GetAttribute("aria-label"));
            Assert.Equal("false", buttonList[2].GetAttribute("aria-disabled"));
            Assert.Equal("Next month", buttonList[2].GetAttribute("aria-label"));
            // Today button Accessibility
            Assert.Equal("Today", buttonList[3].GetAttribute("aria-label"));
            // Calendar Table Accessibility
            Assert.Equal("grid", tableElement?.GetAttribute("role"));
        }

        [Fact(Timeout = 10000, DisplayName = "Accessibility: navigation icons ARIA when navigating")]
        public void PreviousIconNextIconWhenNavigate()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>();
            var tableElement = Calendar.Find("table");
            var ParentContainer = tableElement?.ParentElement?.ParentElement;
            var ButtonList = ParentContainer?.QuerySelectorAll("button");
            // Default values when initial loading (buttonList[0] = title, buttonList[1] = prev, buttonList[2] = next)
            Assert.Equal("false", ButtonList?[1].GetAttribute("aria-disabled"));
            Assert.Equal("Previous month", ButtonList?[1].GetAttribute("aria-label"));
            Assert.Equal("false", ButtonList?[2].GetAttribute("aria-disabled"));
            Assert.Equal("Next month", ButtonList?[2].GetAttribute("aria-label"));
            // Navigating to next month
            ButtonList?[2].Click();
            tableElement = Calendar.Find("table");
            ParentContainer = tableElement?.ParentElement?.ParentElement;
            ButtonList = ParentContainer?.QuerySelectorAll("button");
            Assert.DoesNotContain("e-disabled", ButtonList?[1].ClassName);
            Assert.Equal("false", ButtonList?[1].GetAttribute("aria-disabled"));
            Assert.Equal("false", ButtonList?[2].GetAttribute("aria-disabled"));
            // Navigating to previous month
            ButtonList?[1].Click();
            tableElement = Calendar.Find("table");
            ParentContainer = tableElement?.ParentElement?.ParentElement;
            ButtonList = ParentContainer?.QuerySelectorAll("button");
            Assert.Equal("false", ButtonList?[1].GetAttribute("aria-disabled"));
            Assert.Equal("false", ButtonList?[2].GetAttribute("aria-disabled"));
        }
        
        [Fact(Timeout = 10000, DisplayName = "Accessibility: table attributes when a date is selected")]
        public void TableElementAttributesWhenSelected()
        {
            var Calendar = RenderComponent<SfCalendar<DateTime>>();
            var tableElement = Calendar.Find("table");
            var ParentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal("grid", tableElement?.GetAttribute("role"));
            Assert.Equal(42, tableElement?.QuerySelectorAll("td").Length);
            tableElement?.QuerySelectorAll("td")[10].Click();
        }
    }
}
