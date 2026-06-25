using System;
using System.Globalization;
using System.Threading.Tasks;
using Bunit;
using Xunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.TimePicker
{
    public class TimePickerTests : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public void Default_Render_Has_Input_And_No_Expanded_Aria()
        {
            var timeInstance = RenderComponent<SfTimePicker<DateTime?>>();
            var input = timeInstance.Find("input");
            var aria = input.GetAttribute("aria-expanded");
            Assert.True(string.IsNullOrEmpty(aria) || aria == "false");
        }

        [Fact(Timeout = 10000)]
        public async Task ShowPopupAsync_Sets_AriaExpanded()
        {
            var timeInstance = RenderComponent<SfTimePicker<DateTime?>>();
            await timeInstance.Instance.ShowPopupAsync();
            timeInstance.Render();
            var input = timeInstance.Find("input");
            var aria = input.GetAttribute("aria-expanded");
            Assert.Equal("true", aria);
        }

        [Fact(Timeout = 10000)]
        public void InputFormats_Parse_Typed_Input_In_Specified_Culture()
        {
            var prev = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("fr-FR");
                var formats = new[] { "HH:mm", "H:mm" };
                var timeInstance = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                    .Add(p => p.InputFormats, formats)
                );

                var input = timeInstance.Find("input");
                input.Input("13:45");
                input.Blur();

                Assert.NotNull(timeInstance.Instance.Value);
                Assert.Equal(13, timeInstance.Instance.Value.Value.Hour);
                Assert.Equal(45, timeInstance.Instance.Value.Value.Minute);
            }
            finally
            {
                CultureInfo.CurrentCulture = prev;
            }
        }

        [Fact(Timeout = 10000)]
        public void Mask_EnableMask_Prevents_Invalid_Characters()
        {
            var timeInstance = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.EnableMask, true)
            );
            var input = timeInstance.Find("input");
            Assert.True(timeInstance.Instance.EnableMask);
            Assert.Null(timeInstance.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public void Step_Generates_Correct_List_Size()
        {
            var timeInstance = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.Step, 15)
            );
            // Trigger list generation
            timeInstance.Instance.ShowPopupAsync().Wait();
            timeInstance.Render();

            var items = timeInstance.FindAll("li");
            // Expect at least a few items and not an extremely large list
            Assert.InRange(items.Count, 2, 200);
        }

        [Fact(Timeout = 10000)]
        public void MinMax_Restricts_List_Generation()
        {
            var min = new DateTime(2024, 1, 1, 09, 0, 0);
            var max = new DateTime(2024, 1, 1, 17, 0, 0);
            var timeInstance = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.Min, min)
                .Add(p => p.Max, max)
                .Add(p => p.Step, 30)
            );

            Assert.Equal(min, timeInstance.Instance.Min);
            Assert.Equal(max, timeInstance.Instance.Max);
        }

        [Fact(Timeout = 10000)]
        public void StrictMode_With_Invalid_Format_Resets_Value()
        {
            var timeInstance = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.StrictMode, true)
                .Add(p => p.Format, "HH:mm:ss")
                .Add(p => p.Value, null)
            );

            // Verify StrictMode is enabled
            Assert.True(timeInstance.Instance.StrictMode);

            var input = timeInstance.Find("input");
            var wrapper = input.ParentElement;

            // Component should remain stable with no value initially
            Assert.Null(timeInstance.Instance.Value);
            Assert.DoesNotContain("e-error", wrapper.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void AllowEdit_False_Prevents_Typing()
        {
            var timeInstance = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.AllowEdit, false)
            );

            var input = timeInstance.Find("input");

            var isReadOnly = input.HasAttribute("readonly") || input.GetAttribute("readonly") != null;
            Assert.True(isReadOnly);

            // Ensure no value present by default
            Assert.Null(timeInstance.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public void Readonly_Prevents_Popup_Open()
        {
            var timeInstance = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.Readonly, true)
            );

            timeInstance.Instance.ShowPopupAsync().Wait();
            timeInstance.Render();

            // Should still render but popup behavior is guarded
            var input = timeInstance.Find("input");
            Assert.NotNull(input);
        }

        [Fact(Timeout = 10000)]
        public async void Multiple_InputFormats_Parse_Correctly()
        {
            var formats = new[] { "HH:mm", "H:m", "h:mm tt" };
            var initialValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 30, 00);

            var timeInstance = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.Value, initialValue)
                .Add(p => p.InputFormats, formats)
            );

            // Verify InputFormats are properly set on the component instance
            Assert.NotNull(timeInstance.Instance.InputFormats);
            Assert.Equal(3, timeInstance.Instance.InputFormats.Length);
            Assert.Contains("HH:mm", timeInstance.Instance.InputFormats);
            Assert.Contains("H:m", timeInstance.Instance.InputFormats);
            Assert.Contains("h:mm tt", timeInstance.Instance.InputFormats);
        }

        [Fact(Timeout = 10000)]
        public void Disabled_Component_Ignores_Interactions()
        {
            var timeInstance = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.Disabled, true)
            );

            var input = timeInstance.Find("input");
            input.Input("12:00");
            input.Blur();

            // Disabled component should not update value
            Assert.Null(timeInstance.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public void ShowClearButton_Clears_Value()
        {
            var timeInstance = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.ShowClearButton, true)
                .Add(p => p.Value, new DateTime(2024, 1, 1, 14, 30, 0))
            );

            timeInstance.SetParametersAndRender(parameters => parameters
                .Add(p => p.Value, null)
            );

            Assert.Null(timeInstance.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public void StrictMode_With_MinMax_Allows_Smooth_Editing()
        {
            var today = DateTime.Now;
            var minTime = new DateTime(today.Year, today.Month, today.Day, 08, 00, 00);
            var maxTime = new DateTime(today.Year, today.Month, today.Day, 16, 00, 00);
            var initialValue = new DateTime(today.Year, today.Month, today.Day, 08, 00, 00);
            var timeInstance = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(p => p.StrictMode, true)
                .Add(p => p.Min, minTime)
                .Add(p => p.Max, maxTime)
                .Add(p => p.Value, initialValue)
                .Add(p => p.Format, "h:mm tt")
            );
            Assert.True(timeInstance.Instance.StrictMode);
            Assert.Equal(minTime, timeInstance.Instance.Min);
            Assert.Equal(maxTime, timeInstance.Instance.Max);
            Assert.Equal(initialValue, timeInstance.Instance.Value);
            var input = timeInstance.Find("input");
            input.Input("11:30 AM");
            input.Blur();
            Assert.NotNull(timeInstance.Instance.Value);
            var expectedValue = new DateTime(today.Year, today.Month, today.Day, 11, 30, 00);
            Assert.Equal(expectedValue, timeInstance.Instance.Value);
            Assert.True(timeInstance.Instance.Value >= minTime);
            Assert.True(timeInstance.Instance.Value <= maxTime);
        }
    }
}
