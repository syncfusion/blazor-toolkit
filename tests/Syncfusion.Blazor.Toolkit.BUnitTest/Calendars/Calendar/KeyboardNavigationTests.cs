using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.Calendar
{
    public class KeyboardNavigationTests : BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "Keyboard: Arrow keys update focused cell")]
        public async Task Keyboard_ArrowKeys_UpdateFocus()
        {
            // Set a fixed date for initial render to ensure predictable navigation
            var initialDate = new DateTime(2026, 3, 10); // A Tuesday
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.Value, initialDate)
            );

            var table = calendar.Find("table");
            
            // Simulate ArrowRight (should move focus to March 11)
            await table.KeyDownAsync(new KeyboardEventArgs { Key = "ArrowRight", Code = "ArrowRight" });
            calendar.Render();

            // Verify a cell has the 'e-focused-date' class or equivalent focus indicator
            var focusedCell = calendar.FindAll("td").FirstOrDefault(c => c.ClassName.Contains("e-focused-date"));
            Assert.NotNull(focusedCell);
            
            // March 11 ticks check (rough check)
            var expectedDate = new DateTime(2026, 3, 11);
            Assert.Contains(expectedDate.Ticks.ToString(), focusedCell.Id);
        }

        [Fact(Timeout = 10000, DisplayName = "Keyboard: Home/End keys navigate to start/end of month")]
        public async Task Keyboard_HomeEnd_Navigate()
        {
            var initialDate = new DateTime(2026, 3, 15);
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.Value, initialDate)
            );

            var table = calendar.Find("table");
            
            // Home key
            await table.KeyDownAsync(new KeyboardEventArgs { Key = "Home", Code = "Home" });
            calendar.Render();
            
            var focusedCell = calendar.FindAll("td").FirstOrDefault(c => c.ClassName.Contains("e-focused-date"));
            Assert.Contains(new DateTime(2026, 3, 1).Ticks.ToString(), focusedCell?.Id ?? "");

            // End key
            await table.KeyDownAsync(new KeyboardEventArgs { Key = "End", Code = "End" });
            calendar.Render();
            
            focusedCell = calendar.FindAll("td").FirstOrDefault(c => c.ClassName.Contains("e-focused-date"));
            Assert.Contains(new DateTime(2026, 3, 31).Ticks.ToString(), focusedCell?.Id ?? "");
        }

        [Fact(Timeout = 10000, DisplayName = "Keyboard: Enter selects focused cell")]
        public async Task Keyboard_Enter_Selects()
        {
            var initialDate = new DateTime(2026, 3, 10);
            DateTime? selectedValue = null;
            var calendar = RenderComponent<SfCalendar<DateTime>>(parameters =>
                parameters.Add(p => p.Value, initialDate)
                .Add(p => p.ValueChange, (ChangedEventArgs<DateTime> args) =>
                {
                    selectedValue = args.Value;
                })
            );

            var table = calendar.Find("table");
            await table.KeyDownAsync(new KeyboardEventArgs { Key = "ArrowRight", Code = "ArrowRight" });
            await table.KeyDownAsync(new KeyboardEventArgs { Key = "Enter", Code = "Enter" });
            Assert.Equal(new DateTime(2026, 3, 11), selectedValue);
        }
    }
}
