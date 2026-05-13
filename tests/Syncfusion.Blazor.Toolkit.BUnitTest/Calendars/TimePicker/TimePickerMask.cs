using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using Syncfusion.Blazor.Toolkit.Inputs;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.TimePicker
{
    public class TimePickerMask : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public void FloatlabelType()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
            .Add(calendar => calendar.FloatLabelType, FloatLabelType.Auto)
            .Add(calendar => calendar.Placeholder, "Enter Time").Add(p => p.EnableMask, true));
            // Checking floatinput class added to input
            Assert.Equal(FloatLabelType.Auto, Timepicker.Instance.FloatLabelType);
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Contains("e-float-input", parentContainer.ClassName);
            var floatingElement = parentContainer.QuerySelector(".e-float-text");
            Assert.NotNull(floatingElement);
            // Checking label bottom and label top class when focusing
            Assert.DoesNotContain("e-label-bottom", floatingElement.ClassName);
            inputElement.Focus();
            floatingElement = parentContainer.QuerySelector(".e-float-text");
            Assert.Contains("e-label-top", floatingElement.ClassName);
            Timepicker.SetParametersAndRender(("FloatLabelType", FloatLabelType.Always));
            // Checking floatinput class added to input
            Assert.Equal(FloatLabelType.Always, Timepicker.Instance.FloatLabelType);
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Contains("e-float-input", parentContainer.ClassName);
            // Checking label top class added to the label element.
            floatingElement = parentContainer.QuerySelector(".e-float-text");
            Assert.NotNull(floatingElement);
            floatingElement = parentContainer.QuerySelector(".e-float-text");
            Assert.Contains("e-label-top", floatingElement.ClassName);
            Timepicker.SetParametersAndRender(("FloatLabelType", FloatLabelType.Never));
            // Checking floatinput class not added to input
            Assert.Equal(FloatLabelType.Never, Timepicker.Instance.FloatLabelType);
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.DoesNotContain("e-float-input", parentContainer.ClassName);
        }
        
        [Fact(Timeout = 10000)]
        public void DateFormat()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
            .Add(calendar => calendar.Format, "HH:mm").Add(p => p.EnableMask, true));
            // Checking formatted value added to input
            Assert.Equal("HH:mm", Timepicker.Instance.Format);
            var inputElement = Timepicker.Find("input");
            var inputValue = inputElement.GetAttribute("value");
            Assert.Equal("00:00", inputValue.Replace('\u202F', ' ').Trim());
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender(("Format", "hh:mm"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00", inputValue.Replace('\u202F', ' ').Trim());
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender(("Format", "hh:mm:ss"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00:00", inputValue.Replace('\u202F', ' ').Trim());
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender(("Format", "hh:mm tt"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00 AM", inputValue.Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public void DateFormatWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
            .Add(calendar => calendar.Format, "HH:mm")
            .Add(calendar => calendar.Value, new DateTime(2020, 1, 1)).Add(p => p.EnableMask, true));
            // Checking formatted value added to input
            Assert.Equal("HH:mm", Timepicker.Instance.Format.Replace('\u202F', ' ').Trim());
            var inputElement = Timepicker.Find("input");
            var inputValue = inputElement.GetAttribute("value");
            Assert.Equal("00:00", inputValue.Replace('\u202F', ' ').Trim());
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(("Format", "hh:mm"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00", inputValue.Replace('\u202F', ' ').Trim());
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(("Format", "hh:mm:ss"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00:00", inputValue.Replace('\u202F', ' ').Trim());
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(("Format", "hh:mm tt"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00 AM", inputValue.Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public async Task MaxValue()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters =>
                parameters
                    .Add(c => c.Value, new DateTime(2020, 1, 1, 9, 30, 0))
                    .Add(p => p.EnableMask, true)
            );

            // Set Max and verify
            Timepicker.SetParametersAndRender(("Max", new DateTime(2020, 1, 1, 10, 30, 0)));
            Assert.Equal(new DateTime(2020, 1, 1, 10, 30, 0), Timepicker.Instance.Max);

            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            var raw = inputElement.GetAttribute("value")?.Replace('\u202F', ' ').Trim() ?? string.Empty;

            // Assert displayed time is 09:30
            Assert.True(DateTime.TryParse(raw, out var parsed));
            Assert.Equal(9, parsed.Hour);
            Assert.Equal(30, parsed.Minute);

            // Set Value > Max and assert error state
            Timepicker.SetParametersAndRender(("Value", new DateTime(2020, 1, 1, 11, 30, 0)));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            raw = inputElement.GetAttribute("value")?.Replace('\u202F', ' ').Trim() ?? string.Empty;

            Assert.True(DateTime.TryParse(raw, out parsed));
            Assert.Equal(11, parsed.Hour);
            Assert.Equal(30, parsed.Minute);
            Assert.Contains("e-error", parentContainer.ClassName);

            // Extend Max above current value and ensure error cleared
            Timepicker.SetParametersAndRender(("Max", new DateTime(2020, 1, 1, 15, 30, 0)));
            Assert.Equal(new DateTime(2020, 1, 1, 15, 30, 0), Timepicker.Instance.Max);

            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            raw = inputElement.GetAttribute("value")?.Replace('\u202F', ' ').Trim() ?? string.Empty;

            Assert.True(DateTime.TryParse(raw, out parsed));
            Assert.Equal(11, parsed.Hour);
            Assert.Equal(30, parsed.Minute);
            Assert.DoesNotContain("e-error", parentContainer.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void MinValue()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters =>
                parameters
                    .Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 09, 30, 00))
                    .Add(p => p.EnableMask, true)
                    .Add(p => p.Format, "h:mm tt"));
            // Checking input element value lesser than Min value
            Timepicker.SetParametersAndRender(("Min", new DateTime(2020, 1, 1, 08, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 08, 30, 00), Timepicker.Instance.Min);
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Equal("9:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            // Checking e-error class added to wrapper when value less than Min value
            Timepicker.SetParametersAndRender(("Value", new DateTime(2020, 1, 1, 07, 30, 00)));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("7:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", parentContainer.ClassName);
            // Dynamically changing Max property
            Timepicker.SetParametersAndRender(("Min", new DateTime(2020, 1, 1, 06, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 06, 30, 00), Timepicker.Instance.Min);
            // Checking e-error class not added when lowering min value lesser than value
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("7:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", parentContainer.ClassName);
        }
       
        [Fact(Timeout = 10000)]
        public void PlaceHolder()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.Placeholder, "Enter Time value").Add(p => p.EnableMask, true));
            // Checking Placeholder value
            Assert.Equal("Enter Time value", Timepicker.Instance.Placeholder);
            var inputElement = Timepicker.Find("input");
            Assert.Equal("Enter Time value", inputElement.GetAttribute("placeholder"));
            // Dynamically changing placeholder value
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime>>(("Placeholder", "Choose Time"));
            Assert.Equal("Choose Time", Timepicker.Instance.Placeholder);
            inputElement = Timepicker.Find("input");
            Assert.Equal("Choose Time", inputElement.GetAttribute("placeholder"));
        }
        
        //[Fact(Timeout = 10000)]
        //public async Task Readonly()
        //{
        //    var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
        //    .Add(calendar => calendar.Readonly, true).Add(p => p.EnableMask, true));
        //    // Checking readonly attribute added to input element
        //    Assert.True(Timepicker.Instance.Readonly);
        //    var inputElement = Timepicker.Find("input");
        //    Assert.True(inputElement.HasAttribute("readonly"));
        //    // Checking popup not open when readonly is true
        //    await Timepicker.Instance.ShowPopupAsync();
        //    var isPopupOpen = false;
        //    try
        //    {
        //        Timepicker.Find(".e-popup-wrapper");
        //        isPopupOpen = true;
        //    }
        //    catch
        //    {
        //        isPopupOpen = false;
        //    }
        //    Assert.False(isPopupOpen);
        //    // Dynamically changing readonly to false
        //    Timepicker.SetParametersAndRender<SfTimePicker<DateTime>>(("ReadOnly", false));
        //    Assert.False(Timepicker.Instance.Readonly);
        //    inputElement = Timepicker.Find("input");
        //    Assert.False(inputElement.HasAttribute("readonly"));
        //    // Checking popup should open when readonly is false
        //    await Timepicker.Instance.ShowPopupAsync();
        //    try
        //    {
        //        Timepicker.Find(".e-popup-wrapper");
        //        isPopupOpen = true;
        //    }
        //    catch
        //    {
        //        isPopupOpen = false;
        //    }
        //    Assert.True(isPopupOpen);
        //}
       
        [Fact(Timeout = 10000)]
        public void ShowClearbutton()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
            .Add(calendar => calendar.ShowClearButton, true).Add(p => p.EnableMask, true));
            Assert.True(Timepicker.Instance.ShowClearButton);
            // Checking clear icon present inside input container
            var inputElement = Timepicker.Find("input");
            inputElement.Focus();
            var parentContainer = inputElement.ParentElement;
            var ClearIcon = parentContainer.QuerySelectorAll(".e-close");
            Assert.True(ClearIcon.Length > 0);
            // Dynamically disabling clear button
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime>>(("ShowClearButton", false));
            Assert.False(Timepicker.Instance.ShowClearButton);
            inputElement = Timepicker.Find("input");
            inputElement.Focus();
            parentContainer = inputElement.ParentElement;
            var isClearIconRendered = false;
            try
            {
                ClearIcon = parentContainer.QuerySelectorAll(".e-close");
                isClearIconRendered = true;
            }
            catch
            {
                isClearIconRendered = false;
            }
            Assert.True(isClearIconRendered);
            // Dynamically enabling clear button
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime>>(("ShowClearButton", true));
            Assert.True(Timepicker.Instance.ShowClearButton);
            // Checking clear icon present inside input container
            inputElement = Timepicker.Find("input");
            inputElement.Focus();
            parentContainer = inputElement.ParentElement;
            ClearIcon = parentContainer.QuerySelectorAll(".e-close");
            Assert.True(ClearIcon.Length > 0);
        }
        
        [Fact(Timeout = 10000)]
        public async Task Step()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters =>
                parameters
                    .Add(calendar => calendar.Step, 60)
                    .Add(p => p.EnableMask, true)
                    .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(60, Timepicker.Instance.Step);
            // Opening popup to check li created based on step.
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            var ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            var liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal("12:00 AM", liCollections[0].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());
            Assert.Equal("1:00 AM", liCollections[1].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());
            // Closing the popup
            await Timepicker.Instance.HidePopupAsync();
            // Dynamically changing step value
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime>>(("Step", 90));
            // Opening popup to check li created based on step.
            await Timepicker.Instance.ShowPopupAsync();
            popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal("12:00 AM", liCollections[0].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());
            Assert.Equal("1:30 AM", liCollections[1].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());

        }
       
        [Fact(Timeout = 10000)]
        public async Task StrictMode()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters =>
                parameters
                    .Add(calendar => calendar.StrictMode, true)
                    .Add(p => p.EnableMask, true)
                    .Add(p => p.Format, "h:mm tt"));
            Assert.True(Timepicker.Instance.StrictMode);
            var containEle = Timepicker.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containEle.ClassName);
            Assert.Contains("12:00 AM", Timepicker.Find("input").GetAttribute("value").Replace('\u202F', ' ').Trim());
            Timepicker.SetParametersAndRender(("StrictMode", false));
            await Timepicker.Instance.ShowPopupAsync();
            Assert.False(Timepicker.Instance.StrictMode);
            containEle = Timepicker.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containEle.ClassName);

        }

        [Fact(Timeout = 10000)]
        public void StrictModeWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
            .Add(calendar => calendar.StrictMode, true).Add(p => p.EnableMask, true));
            Assert.True(Timepicker.Instance.StrictMode);
            var containEle = Timepicker.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containEle.ClassName);
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(("StrictMode", false));
            Assert.False(Timepicker.Instance.StrictMode);
            containEle = Timepicker.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containEle.ClassName);

        }

        [Fact(Timeout = 10000)]
        public void StrictModeMinMax()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 00, 30, 00))
            .Add(p => p.EnableMask, true)
            .Add(p => p.StrictMode, true)
            .Add(p => p.Min, new DateTime(2020, 1, 3, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 4, 10, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            // Checking Value less than min set to min value
            Assert.Equal(Timepicker.Instance.Min, Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            Assert.Equal("8:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public void StrictModeMinMaxWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(param => param
                .Add(p => p.Value, new DateTime(2020, 1, 1, 00, 30, 00))
                .Add(p => p.EnableMask, true)
                .Add(p => p.StrictMode, true)
                .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
                .Add(p => p.Max, new DateTime(2020, 1, 1, 10, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            // Checking Value less than min set to min value
            Assert.Equal(Timepicker.Instance.Min, Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            Assert.Equal("8:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public void StrictModeMinGreaterthanMax()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
                .Add(p => p.Value, new DateTime(2020, 1, 1, 07, 30, 00)).Add(p => p.EnableMask, true)
                .Add(p => p.StrictMode, true)
                .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
                .Add(p => p.Max, new DateTime(2020, 1, 1, 07, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1, 07, 30, 00), Timepicker.Instance.Value);
            Assert.Equal(new DateTime(2020, 1, 1, 08, 30, 00), Timepicker.Instance.Min);
            Assert.Equal(new DateTime(2020, 1, 1, 07, 30, 00), Timepicker.Instance.Max);
            var inputEle = Timepicker.Find("input");
            Assert.Equal("7:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", inputEle.ParentElement.ClassName);
            Timepicker.SetParametersAndRender(("Min", new DateTime(2020, 1, 1, 07, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 07, 30, 00), Timepicker.Instance.Min);
            inputEle = Timepicker.Find("input");
            Assert.Equal("7:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", inputEle.ParentElement.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void StrictModeMinGreaterthanMaxWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(param => param
                .Add(p => p.Value, new DateTime(2020, 1, 1, 07, 30, 00)).Add(p => p.EnableMask, true)
                .Add(p => p.StrictMode, true)
                .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
                .Add(p => p.Max, new DateTime(2020, 1, 1, 07, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1, 07, 30, 00), Timepicker.Instance.Value);
            Assert.Equal(new DateTime(2020, 1, 1, 08, 30, 00), Timepicker.Instance.Min);
            Assert.Equal(new DateTime(2020, 1, 1, 07, 30, 00), Timepicker.Instance.Max);
            var inputEle = Timepicker.Find("input");
            Assert.Equal("7:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", inputEle.ParentElement.ClassName);
            Timepicker.SetParametersAndRender(("Min", new DateTime(2020, 1, 1, 07, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 07, 30, 00), Timepicker.Instance.Min);
            inputEle = Timepicker.Find("input");
            Assert.Equal("7:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", inputEle.ParentElement.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void ValueLessThanMinMaxRange()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
                .Add(p => p.Value, new DateTime(2020, 1, 1, 00, 30, 00))
                .Add(p => p.EnableMask, true)
                .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
                .Add(p => p.Max, new DateTime(2020, 1, 1, 10, 30, 00))
                .Add(p => p.Format, "h:mm tt")); // added
            Assert.Equal(new DateTime(2020, 1, 1, 00, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("12:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", ParentContainer.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void ValueLessThanMinMaxRangeWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 00, 30, 00)).Add(p => p.EnableMask, true)
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 1, 10, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1, 00, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("12:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", ParentContainer.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void ValueGreaterThanMinMaxRange()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 11, 30, 00)).Add(p => p.EnableMask, true)
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 1, 10, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1, 11, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("11:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", ParentContainer.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void ValueGreaterThanMinMaxRangeWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 11, 30, 00)).Add(p => p.EnableMask, true)
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 1, 10, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1, 11, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("11:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", ParentContainer.ClassName);
        }
    }
}
