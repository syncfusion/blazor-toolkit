using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.Calendar
{
    public class SfCalendarExtraTests : BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "NavigateAsync: null date throws ArgumentNullException")]
        public async Task NavigateAsync_NullDate_Throws()
        {
            var calendar = RenderComponent<SfCalendar<DateTime?>>();
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await calendar.Instance.NavigateAsync(CalendarView.Month, null);
            });
        }

        [Fact(Timeout = 10000, DisplayName = "NavigateAsync: invalid view throws ArgumentOutOfRangeException")]
        public async Task NavigateAsync_InvalidView_Throws()
        {
            var calendar = RenderComponent<SfCalendar<DateTime>>();
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await calendar.Instance.NavigateAsync((CalendarView)999, DateTime.Now);
            });
        }

        [Fact(Timeout = 10000, DisplayName = "AddDatesAsync: null input is no-op")]
        public async Task AddDatesAsync_Null_NoOp()
        {
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.IsMultiSelection, true)
                          .Add(p => p.Values, new DateTime[] { new DateTime(2020,1,1) })
            );
            var before = calendar.Instance.Values?.Length ?? 0;
            await calendar.Instance.AddDatesAsync(null);
            var after = calendar.Instance.Values?.Length ?? 0;
            Assert.Equal(before, after);
        }

        [Fact(Timeout = 10000, DisplayName = "AddDatesAsync: duplicates are not added")]
        public async Task AddDatesAsync_Duplicates_Prevented()
        {
            var initial = new DateTime[] { new DateTime(2020,1,1) };
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.IsMultiSelection, true)
                          .Add(p => p.Values, initial)
            );
            await calendar.Instance.AddDatesAsync(new DateTime[] { new DateTime(2020,1,1) });
            Assert.Single(calendar.Instance.Values);
            Assert.Equal(initial[0], calendar.Instance.Values[0]);
        }

        [Fact(Timeout = 10000, DisplayName = "RemoveDatesAsync: empty array is no-op")]
        public async Task RemoveDatesAsync_Empty_NoOp()
        {
            var initial = new DateTime[] { new DateTime(2020,1,1), new DateTime(2020,1,2) };
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.IsMultiSelection, true)
                          .Add(p => p.Values, initial)
            );
            await calendar.Instance.RemoveDatesAsync(Array.Empty<DateTime>());
            Assert.Equal(2, calendar.Instance.Values.Length);
        }

        [Fact(Timeout = 10000, DisplayName = "IsMultiSelection=false causes Add/Remove to no-op")]
        public async Task MultiSelectionFalse_NoOp()
        {
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.IsMultiSelection, false)
                          .Add(p => p.Values, new DateTime[] { new DateTime(2020,1,1) })
            );
            var before = calendar.Instance.Values?.Length ?? 0;
            await calendar.Instance.AddDatesAsync(new DateTime[] { new DateTime(2020,1,2) });
            await calendar.Instance.RemoveDatesAsync(new DateTime[] { new DateTime(2020,1,1) });
            var after = calendar.Instance.Values?.Length ?? 0;
            Assert.Equal(before, after);
        }

        [Fact(Timeout = 10000, DisplayName = "Min>Max sets e-overlay class on root")]
        public void MinGreaterThanMax_SetsOverlay()
        {
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.Min, new DateTime(2021,1,1))
                          .Add(p => p.Max, new DateTime(2020,1,1))
            );
            var tableElement = calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Contains("e-overlay", parentContainer?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Day cells include ARIA attributes")]
        public void DayCells_IncludeAriaAttributes()
        {
            var calendar = RenderComponent<SfCalendar<DateTime>>();
            var tableElement = calendar.Find("table");
            var firstCell = tableElement.QuerySelector("td");
            Assert.NotNull(firstCell);
            var button = firstCell.QuerySelector("button") ?? firstCell.FirstElementChild;
            Assert.NotNull(button);
            var ariaLabel = button.GetAttribute("aria-label") ?? firstCell.GetAttribute("aria-label");
            Assert.False(string.IsNullOrEmpty(ariaLabel));
        }

        [Fact(Timeout = 10000, DisplayName = "HtmlAttributes: merges class into root and applies attributes")]
        public void HtmlAttributes_ClassMerged()
        {
            var htmlAttr = new System.Collections.Generic.Dictionary<string, object>() { { "data-test", "1" } };
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.CssClass, "base-class")
                          .Add(p => p.HtmlAttributes, htmlAttr)
            );
            var tableElement = calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Contains("base-class", parentContainer?.ClassName);
            Assert.Equal("1", parentContainer?.GetAttribute("data-test"));
        }
    }
}
