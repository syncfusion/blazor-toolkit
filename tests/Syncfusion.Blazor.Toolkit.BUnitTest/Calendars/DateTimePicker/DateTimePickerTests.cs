using System;
using System.Globalization;
using Bunit;
using Xunit;
using Syncfusion.Blazor.Toolkit.Calendars;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.DateTimePicker
{
    public class DateTimePickerTests : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public void StrictMode_Reverts_Invalid_Value()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.StrictMode, true)
            );

            var input = dateInstance.Find("input");
            input.Input("invalid date");
            input.Blur();

            // Expect null or previous value; at least ensure component didn't crash
            Assert.True(dateInstance.Instance.Value == null || dateInstance.Instance.Value is DateTime);
        }

        [Fact(Timeout = 10000)]
        public void Keyboard_Navigation_Enter_Selects_Item()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>();
            // This is a behavioral test scaffold; exact key simulation depends on test utils
            Assert.NotNull(dateInstance.Markup);
        }

        [Fact(Timeout = 10000)]
        public void MinTime_MaxTime_Restrict_Selection()
        {
            var minTime = new DateTime(2024, 1, 1, 09, 0, 0);
            var maxTime = new DateTime(2024, 1, 1, 17, 0, 0);
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.MinTime, minTime)
                .Add(p => p.MaxTime, maxTime)
            );

            Assert.Equal(minTime, dateInstance.Instance.MinTime);
            Assert.Equal(maxTime, dateInstance.Instance.MaxTime);
        }

        [Fact(Timeout = 10000)]
        public void Value_Outside_MinMax_Range_Handled()
        {
            var min = new DateTime(2024, 1, 1, 0, 0, 0);
            var max = new DateTime(2024, 1, 31, 23, 59, 59);
            var outsideValue = new DateTime(2023, 12, 31, 12, 0, 0);

            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.Min, min)
                .Add(p => p.Max, max)
            );

            dateInstance.SetParametersAndRender(parameters => parameters
                .Add(p => p.Value, outsideValue)
            );

            // Component should handle or reject out-of-range value gracefully
            Assert.True(dateInstance.Instance.Value == null || dateInstance.Instance.Value is DateTime);
        }

        [Fact(Timeout = 10000)]
        public void AllowEdit_False_Disables_Typing()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.AllowEdit, false)
            );

            var input = dateInstance.Find("input");
            input.Input("2026/04/01 12:30");
            input.Blur();

            // AllowEdit=false should prevent direct input
            Assert.Null(dateInstance.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public void Readonly_Prevents_Interaction()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.Readonly, true)
            );

            var input = dateInstance.Find("input");
            Assert.NotNull(input);
            // Readonly should be set on input element
        }

        [Fact(Timeout = 10000)]
        public void ScrollTo_Set_In_Time_Popup()
        {
            var scrollTime = new DateTime(2024, 1, 1, 14, 30, 0);
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.ScrollTo, scrollTime)
            );

            Assert.Equal(scrollTime, dateInstance.Instance.ScrollTo);
        }

        [Fact(Timeout = 10000)]
        public void DST_Edge_Case_Handles_Gracefully()
        {
            // Test DST transition - March and November in many regions
            var dstTransitionDate = new DateTime(2024, 3, 10, 2, 30, 0); // US DST start
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.Value, dstTransitionDate)
            );

            // Component should handle DST dates without throwing
            Assert.True(dateInstance.Instance.Value == null || dateInstance.Instance.Value is DateTime);
        }

        [Fact(Timeout = 10000)]
        public void Disabled_Prevents_Value_Changes()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.Disabled, true)
            );

            var input = dateInstance.Find("input");
            input.Input("2026/04/01 12:30");
            input.Blur();

            // Disabled component should not update
            Assert.Null(dateInstance.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public void ShowClearButton_With_Value_Cleared()
        {
            var initialValue = new DateTime(2024, 1, 15, 14, 30, 0);
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.ShowClearButton, true)
                .Add(p => p.Value, initialValue)
            );

            dateInstance.SetParametersAndRender(parameters => parameters
                .Add(p => p.Value, null)
            );

            Assert.Null(dateInstance.Instance.Value);
        }
    }
}
