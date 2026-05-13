using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using static Bunit.ComponentParameterFactory;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Buttons;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.Calendar
{
    public class PublicMethods: BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "PublicMethods: AddDatesAsync adds dates programmatically")]
        public async Task AddDateTimeMethod()
        {
            var component = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.
            Add(Cal => Cal.IsMultiSelection, true));
            var tableElement = component.Find("table");
            Assert.Null(component.Instance.Values);
            Assert.Null(tableElement.QuerySelector(".e-selected"));
            var button = RenderComponent<SfButton>((EventCallback(nameof(SfButton.OnClick), async (MouseEventArgs args) => {
                await component.Instance.AddDatesAsync(new DateTime[] { new DateTime(2020, 1, 1) });
            }))
            );
            var buttonElement = button.Find("button");
            buttonElement.Click();
            tableElement = null;
            component.WaitForAssertion(() => Assert.NotNull(component.Instance.Values), System.TimeSpan.FromMilliseconds(1000));
            var yearElement = component.Find(".e-day.e-title");
            yearElement.Click();
            yearElement.Click();
            tableElement = component.Find("table");
            var yearSpanElement = tableElement?.QuerySelector("td.e-cell span[title='2020']")?.ParentElement;
            yearSpanElement?.Click();
            tableElement = component.Find("table");
            var monthSpanElement = tableElement?.QuerySelector("td.e-cell span[title='January 2020']")?.ParentElement;
            monthSpanElement?.Click();
            Assert.Equal(1, tableElement?.QuerySelectorAll(".e-selected").Length);
            Assert.Equal(1, component.Instance.Values?.Length);
        }

        [Fact(Timeout = 10000, DisplayName = "PublicMethods: CurrentView toggles between views")]
        public void CurrentViewMethod()
        {
            var component = RenderComponent<SfCalendar<DateTime>>();
            Assert.Equal("Month", component.Instance.CurrentView());
            var tableElement = component.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            parentContainer?.QuerySelector(".e-title")?.Click();
            Assert.Equal("Year", component.Instance.CurrentView());
            tableElement = component.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            parentContainer?.QuerySelector(".e-title")?.Click();
            Assert.Equal("Decade", component.Instance.CurrentView());
        }
        
        [Fact(Timeout = 10000, DisplayName = "PublicMethods: NavigateAsync navigates between views")]
        public void NavigateToMethod()
        {
            var component = RenderComponent<SfCalendar<DateTime>>();
            Assert.Equal("Month", component.Instance.CurrentView());
            // Navigating to Year view
            component.Instance?.NavigateAsync(CalendarView.Year, DateTime.Now);
            Assert.Equal("Year", component.Instance?.CurrentView());
            // Navigating to Decade view
            component.Instance?.NavigateAsync(CalendarView.Decade, DateTime.Now);
            Assert.Equal("Decade", component.Instance?.CurrentView());
            // Navigating to Month View
            component.Instance?.NavigateAsync(CalendarView.Month, DateTime.Now);
            Assert.Equal("Month", component.Instance?.CurrentView());
        }
    }
}
