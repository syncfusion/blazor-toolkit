using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using Syncfusion.Blazor.Toolkit.Inputs;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.TimePicker
{
    public class TimeOnlyTimePicker : BunitTestContext
    {
#if NET7_0_OR_GREATER

        [Fact(Timeout = 10000)]
        public void FloatlabelType()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters => parameters
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
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters => parameters
            .Add(calendar => calendar.Format, "HH:mm").Add(p => p.EnableMask, true));
            // Checking formatted value added to input
            Assert.Equal("HH:mm", Timepicker.Instance.Format);
            var inputElement = Timepicker.Find("input");
            var inputValue = inputElement.GetAttribute("value");
            Assert.Equal("00:00", inputValue);
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender(("Format", "hh:mm"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00", inputValue.Replace('\u202F', ' ').Trim());
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender(("Format", "hh:mm:ss"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00:00", inputValue);
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender(("Format", "hh:mm tt"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00 AM", inputValue.Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public void DateFormatWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(parameters => parameters
            .Add(calendar => calendar.Format, "HH:mm")
            .Add(calendar => calendar.Value, new TimeOnly(00,00)).Add(p => p.EnableMask, true));
            // Checking formatted value added to input
            Assert.Equal("HH:mm", Timepicker.Instance.Format);
            var inputElement = Timepicker.Find("input");
            var inputValue = inputElement.GetAttribute("value");
            Assert.Equal("00:00", inputValue);
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly?>>(("Format", "hh:mm"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00", inputValue.Replace('\u202F', ' ').Trim());
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly?>>(("Format", "hh:mm:ss"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00:00", inputValue);
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly?>>(("Format", "hh:mm tt"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00 AM", inputValue.Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public void MaxValue()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters =>
            parameters.Add(calendar => calendar.Value, new TimeOnly(09, 30, 00))
            .Add(p => p.EnableMask, true)
            .Add(p => p.Format, "h:mm tt"));
            // Checking input element value less than Max value
            Timepicker.SetParametersAndRender(("Max", new DateTime(1900, 1, 1, 10, 30, 00)));
            Assert.Equal(new DateTime(1900, 1, 1, 10, 30, 00), Timepicker.Instance.Max);
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Equal("9:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            // Checking e-error class added to wrapper when value greater than Max value
            Timepicker.SetParametersAndRender(("Value", new TimeOnly(11, 30, 00)));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("11:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", parentContainer.ClassName);
            // Dynamically changing Max property
            Timepicker.SetParametersAndRender(("Max", new DateTime(1900, 1, 1, 15, 30, 00)));
            Assert.Equal(new DateTime(1900, 1, 1, 15, 30, 00), Timepicker.Instance.Max);
            // Checking e-error class not added when extending max value greater than value
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("11:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", parentContainer.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void MinValue()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters =>
            parameters.Add(calendar => calendar.Value, new TimeOnly(09, 30, 00))
            .Add(p => p.EnableMask, true)
            .Add(p => p.Format, "h:mm tt"));
            // Checking input element value lesser than Min value
            Timepicker.SetParametersAndRender(("Min", new DateTime(2020, 1, 1, 08, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 08, 30, 00), Timepicker.Instance.Min);
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Equal("9:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            // Checking e-error class added to wrapper when value less than Min value
            Timepicker.SetParametersAndRender(("Value", new TimeOnly(07, 30, 00)));
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
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters =>
            parameters.Add(calendar => calendar.Placeholder, "Enter Time value").Add(p => p.EnableMask, true));
            // Checking Placeholder value
            Assert.Equal("Enter Time value", Timepicker.Instance.Placeholder);
            var inputElement = Timepicker.Find("input");
            Assert.Equal("Enter Time value", inputElement.GetAttribute("placeholder"));
            // Dynamically changing placeholder value
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly>>(("Placeholder", "Choose Time"));
            Assert.Equal("Choose Time", Timepicker.Instance.Placeholder);
            inputElement = Timepicker.Find("input");
            Assert.Equal("Choose Time", inputElement.GetAttribute("placeholder"));
        }

        //[Fact(Timeout = 10000)]
        //public async Task Readonly()
        //{
        //    var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters => parameters
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
        //    Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly>>(("ReadOnly", false));
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
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters => parameters
            .Add(calendar => calendar.ShowClearButton, true).Add(p => p.EnableMask, true));
            Assert.True(Timepicker.Instance.ShowClearButton);
            // Checking clear icon present inside input container
            var inputElement = Timepicker.Find("input");
            inputElement.Focus();
            var parentContainer = inputElement.ParentElement;
            var ClearIcon = parentContainer.QuerySelectorAll(".e-close");
            Assert.True(ClearIcon.Length > 0);
            // Dynamically disabling clear button
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly>>(("ShowClearButton", false));
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
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly>>(("ShowClearButton", true));
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
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters => parameters
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
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly>>(("Step", 90));
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
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters => parameters
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
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(parameters => parameters
            .Add(calendar => calendar.StrictMode, true).Add(p => p.EnableMask, true));
            Assert.True(Timepicker.Instance.StrictMode);
            var containEle = Timepicker.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containEle.ClassName);
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly?>>(("StrictMode", false));
            Assert.False(Timepicker.Instance.StrictMode);
            containEle = Timepicker.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containEle.ClassName);

        }

        [Fact(Timeout = 10000)]
        public void StrictModeMinMax()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(param => param
                .Add(p => p.Value, new TimeOnly(00, 30, 00))
                .Add(p => p.EnableMask, true)
                .Add(p => p.StrictMode, true)
                .Add(p => p.Min, new DateTime(1900, 1, 1, 08, 30, 00))
                .Add(p => p.Max, new DateTime(1900, 1, 1, 10, 30, 00))
                .Add(p => p.Format, "h:mm tt") // ensure deterministic 12-hour output
            );
            // StrictMode should push out-of-range Value (00:30) up to Min (08:30)
            var expected = TimeOnly.FromDateTime(Timepicker.Instance.Min);
            Assert.Equal(expected, Timepicker.Instance.Value);
            // UI should display the corrected time in the forced format
            var inputEle = Timepicker.Find("input");
            Assert.Equal("8:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public void StrictModeMinMaxWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(param => param
                .Add(p => p.Value, new TimeOnly(00, 30, 00))
                .Add(p => p.EnableMask, true)
                .Add(p => p.StrictMode, true)
                .Add(p => p.Min, new DateTime(1900, 1, 1, 08, 30, 00))
                .Add(p => p.Max, new DateTime(1900, 1, 1, 10, 30, 00))
                .Add(p => p.Format, "h:mm tt"));

            // Value less than Min should be set to Min (compare as TimeOnly to avoid locale/format variance)
            var expected = TimeOnly.FromDateTime(Timepicker.Instance.Min);
            Assert.Equal(expected, Timepicker.Instance.Value!.Value);

            // UI should display the corrected time in the forced format
            var inputEle = Timepicker.Find("input");
            Assert.Equal("8:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public void StrictModeMinGreaterthanMax()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(param => param
            .Add(p => p.Value, new TimeOnly(07, 30, 00)).Add(p => p.EnableMask, true)
            .Add(p => p.StrictMode, true)
            .Add(p => p.Min, new DateTime(1900, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(1900, 1, 1, 07, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new TimeOnly(07, 30, 00), Timepicker.Instance.Value);
            Assert.Equal(new DateTime(1900, 1, 1, 08, 30, 00), Timepicker.Instance.Min);
            Assert.Equal(new DateTime(1900, 1, 1, 07, 30, 00), Timepicker.Instance.Max);
            var inputEle = Timepicker.Find("input");
            Assert.Equal("7:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", inputEle.ParentElement.ClassName);
            Timepicker.SetParametersAndRender(("Min", new DateTime(1900, 1, 1, 07, 30, 00)));
            Assert.Equal(new DateTime(1900, 1, 1, 07, 30, 00), Timepicker.Instance.Min);
            inputEle = Timepicker.Find("input");
            Assert.Equal("7:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", inputEle.ParentElement.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void StrictModeMinGreaterthanMaxWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(param => param
            .Add(p => p.Value, new TimeOnly(07, 30, 00))
            .Add(p => p.EnableMask, true)
            .Add(p => p.StrictMode, true)
            .Add(p => p.Min, new DateTime(1900, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(1900, 1, 1, 07, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new TimeOnly(07, 30, 00), Timepicker.Instance.Value);
            Assert.Equal(new DateTime(1900, 1, 1, 08, 30, 00), Timepicker.Instance.Min);
            Assert.Equal(new DateTime(1900, 1, 1, 07, 30, 00), Timepicker.Instance.Max);
            var inputEle = Timepicker.Find("input");
            Assert.Equal("7:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", inputEle.ParentElement.ClassName);
            Timepicker.SetParametersAndRender(("Min", new DateTime(1900, 1, 1, 07, 30, 00)));
            Assert.Equal(new DateTime(1900, 1, 1, 07, 30, 00), Timepicker.Instance.Min);
            inputEle = Timepicker.Find("input");
            Assert.Equal("7:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", inputEle.ParentElement.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void ValueLessThanMinMaxRange()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(param => param
            .Add(p => p.Value, new TimeOnly(00, 30, 00))
            .Add(p => p.EnableMask, true)
            .Add(p => p.Min, new DateTime(1900, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(1900, 1, 1, 10, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new TimeOnly(00, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("12:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", ParentContainer.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void ValueLessThanMinMaxRangeWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(param => param
            .Add(p => p.Value, new TimeOnly(00, 30, 00))
            .Add(p => p.EnableMask, true)
            .Add(p => p.Min, new DateTime(1900, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(1900, 1, 1, 10, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new TimeOnly(00, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("12:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", ParentContainer.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void ValueGreaterThanMinMaxRange()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(param => param
            .Add(p => p.Value, new TimeOnly(11, 30, 00))
            .Add(p => p.EnableMask, true)
            .Add(p => p.Min, new DateTime(1900, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(1900, 1, 1, 10, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new TimeOnly(11, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("11:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", ParentContainer.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void ValueGreaterThanMinMaxRangeWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(param => param
            .Add(p => p.Value, new TimeOnly(11, 30, 00))
            .Add(p => p.EnableMask, true)
            .Add(p => p.Min, new DateTime(1900, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(1900, 1, 1, 10, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new TimeOnly(11, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("11:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", ParentContainer.ClassName);
        }     

        [Fact(Timeout = 10000)]
        public void DefaultInitialize()
        {
            var TimePicker = RenderComponent<SfTimePicker<TimeOnly>>();
            var inputElement = TimePicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Contains("e-time-wrapper", parentContainer.ClassName);
            Assert.Contains("e-timepicker", inputElement.ClassName);
            Assert.Equal(2, parentContainer.ChildElementCount);
            Assert.Equal("INPUT", parentContainer.Children[0].TagName);
            Assert.Equal("SPAN", parentContainer.Children[1].TagName);
            var timeIcon = parentContainer.Children[1];
            Assert.Contains("e-clock", timeIcon.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void DefaultInitializeWithNullable()
        {
            var TimePicker = RenderComponent<SfTimePicker<TimeOnly?>>();
            var inputElement = TimePicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Contains("e-time-wrapper", parentContainer.ClassName);
            Assert.Contains("e-timepicker", inputElement.ClassName);
            Assert.Equal(2, parentContainer.ChildElementCount);
            Assert.Equal("INPUT", parentContainer.Children[0].TagName);
            Assert.Equal("SPAN", parentContainer.Children[1].TagName);
            var timeIcon = parentContainer.Children[1];
            Assert.Contains("e-clock", timeIcon.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void DefaultValue()
        {
            var TimePicker = RenderComponent<SfTimePicker<TimeOnly>>();
            Assert.Equal(default(TimeOnly), TimePicker.Instance.Value);
            Assert.Null(TimePicker.Instance.CssClass);
            Assert.False(TimePicker.Instance.EnablePersistence);
            Assert.False(TimePicker.Instance.Disabled);
            Assert.True(TimePicker.Instance.AllowEdit);
            Assert.Equal(FloatLabelType.Never, TimePicker.Instance.FloatLabelType);
            Assert.Null(TimePicker.Instance.Format);
            Assert.Null(TimePicker.Instance.HtmlAttributes);
            Assert.Null(TimePicker.Instance.KeyConfigs);
            Assert.Equal(new DateTime(2099, 12, 31, 23, 59, 59), TimePicker.Instance.Max);
            Assert.Equal(new DateTime(1900, 01, 01, 00, 00, 00), TimePicker.Instance.Min);
            Assert.Null(TimePicker.Instance.Placeholder);
            Assert.False(TimePicker.Instance.Readonly);
            Assert.Null(TimePicker.Instance.ScrollTo);
            Assert.False(TimePicker.Instance.ShowClearButton);
            Assert.Equal(30, TimePicker.Instance.Step);
            Assert.False(TimePicker.Instance.StrictMode);
            Assert.Equal(default(int), TimePicker.Instance.TabIndex);
            Assert.Equal(default(TimeOnly), TimePicker.Instance.Value);
            Assert.Null(TimePicker.Instance.Width);
            Assert.Equal(1000, TimePicker.Instance.ZIndex);
        }

        [Fact(Timeout = 10000)]
        public void DefaultValueWithNullable()
        {
            var TimePicker = RenderComponent<SfTimePicker<TimeOnly?>>();
            Assert.Null(TimePicker.Instance.Value);
            Assert.Null(TimePicker.Instance.CssClass);
            Assert.False(TimePicker.Instance.EnablePersistence);
            Assert.False(TimePicker.Instance.Disabled);
            Assert.True(TimePicker.Instance.AllowEdit);
            Assert.Equal(FloatLabelType.Never, TimePicker.Instance.FloatLabelType);
            Assert.Null(TimePicker.Instance.Format);
            Assert.Null(TimePicker.Instance.HtmlAttributes);
            Assert.Null(TimePicker.Instance.KeyConfigs);
            Assert.Equal(new DateTime(2099, 12, 31, 23, 59, 59), TimePicker.Instance.Max);
            Assert.Equal(new DateTime(1900, 01, 01, 00, 00, 00), TimePicker.Instance.Min);
            Assert.Null(TimePicker.Instance.Placeholder);
            Assert.False(TimePicker.Instance.Readonly);
            Assert.Null(TimePicker.Instance.ScrollTo);
            Assert.False(TimePicker.Instance.ShowClearButton);
            Assert.Equal(30, TimePicker.Instance.Step);
            Assert.False(TimePicker.Instance.StrictMode);
            Assert.Equal(default(int), TimePicker.Instance.TabIndex);
            Assert.Null(TimePicker.Instance.Value);
            Assert.Null(TimePicker.Instance.Width);
            Assert.Equal(1000, TimePicker.Instance.ZIndex);
        }

        [Fact(Timeout = 10000)]
        public async Task PredefinedValue()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters => parameters
                .Add(val => val.Value, new TimeOnly(00,00))
                .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new TimeOnly(00,00), Timepicker.Instance.Value);
            var inputElement = Timepicker.Find("input");
            // Checking value selected in the popup
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            var ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            var liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(48, liCollections.Length);
            var selectedLi = ulEle.QuerySelectorAll(".e-list-item.e-active");
            Assert.Equal(1, selectedLi.Length);
            Assert.Equal("12:00 AM", selectedLi[0].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());
            Assert.Equal("12:00 AM", selectedLi[0].GetAttribute("data-value").Replace('\u202F', ' ').Trim());
            // Select the value using popup
            liCollections[1].Click();
            Assert.Equal(new TimeOnly(00, 30, 00), Timepicker.Instance.Value);
            await Timepicker.Instance.ShowPopupAsync();
            popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(48, liCollections.Length);
            selectedLi = ulEle.QuerySelectorAll(".e-list-item.e-active");
            Assert.Equal(1, selectedLi.Length);
            Assert.Equal("12:30 AM", selectedLi[0].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());
            Assert.Equal("12:30 AM", selectedLi[0].GetAttribute("data-value").Replace('\u202F', ' ').Trim());
            // Dynamically changing value
            Timepicker.SetParametersAndRender(("Value", new TimeOnly( 00, 00, 00)));
            Assert.Equal(new TimeOnly(00, 00, 00), Timepicker.Instance.Value);
            // Checking value selected in the popup
            await Timepicker.Instance.ShowPopupAsync();
            popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(48, liCollections.Length);
            selectedLi = ulEle.QuerySelectorAll(".e-list-item.e-active");
            Assert.Equal(1, selectedLi.Length);
            Assert.Equal("12:00 AM", selectedLi[0].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());
            Assert.Equal("12:00 AM", selectedLi[0].GetAttribute("data-value").Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public async Task PredefinedValueWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(parameters => parameters
                .Add(val => val.Value, new TimeOnly(00,00))
                .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new TimeOnly(00,00), Timepicker.Instance.Value);
            var inputElement = Timepicker.Find("input");
            // Checking value selected in the popup
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            var ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            var liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(48, liCollections.Length);
            var selectedLi = ulEle.QuerySelectorAll(".e-list-item.e-active");
            Assert.Equal(1, selectedLi.Length);
            Assert.Equal("12:00 AM", selectedLi[0].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());
            Assert.Equal("12:00 AM", selectedLi[0].GetAttribute("data-value").Replace('\u202F', ' ').Trim());
            // Select the value using popup
            liCollections[1].Click();
            Assert.Equal(new TimeOnly(00, 30, 00), Timepicker.Instance.Value);
            await Timepicker.Instance.ShowPopupAsync();
            popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(48, liCollections.Length);
            selectedLi = ulEle.QuerySelectorAll(".e-list-item.e-active");
            Assert.Equal(1, selectedLi.Length);
            Assert.Equal("12:30 AM", selectedLi[0].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());
            Assert.Equal("12:30 AM", selectedLi[0].GetAttribute("data-value").Replace('\u202F', ' ').Trim());
            // Dynamically changing value
            Timepicker.SetParametersAndRender(("Value", new TimeOnly(00, 00, 00)));
            Assert.Equal(new TimeOnly(00, 00, 00), Timepicker.Instance.Value);
            // Checking value selected in the popup
            await Timepicker.Instance.ShowPopupAsync();
            popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(48, liCollections.Length);
            selectedLi = ulEle.QuerySelectorAll(".e-list-item.e-active");
            Assert.Equal(1, selectedLi.Length);
            Assert.Equal("12:00 AM", selectedLi[0].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());
            Assert.Equal("12:00 AM", selectedLi[0].GetAttribute("data-value").Replace('\u202F', ' ').Trim());
        }      

        [Fact(Timeout = 10000)]
        public async Task FloatlabelTypeWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(parameters => parameters
            .Add(calendar => calendar.FloatLabelType, FloatLabelType.Auto)
            .Add(calendar => calendar.Placeholder, "Enter Time"));
            // Checking floatinput class added to input
            Assert.Equal(FloatLabelType.Auto, Timepicker.Instance.FloatLabelType);
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Contains("e-float-input", parentContainer.ClassName);
            var floatingElement = parentContainer.QuerySelector(".e-float-text");
            Assert.NotNull(floatingElement);
            // Checking label bottom and label top class when focusing
            Assert.Contains("e-label-bottom", floatingElement.ClassName);
            inputElement.Focus();
            Timepicker.SetParametersAndRender(("Value", new TimeOnly(11, 15, 0)));
            await Task.Delay(20);
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
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
        public void TimeFormat()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters => parameters
            .Add(calendar => calendar.Format, "HH:mm"));
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
            Assert.Equal("12:00:00", inputValue);
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender(("Format", "hh:mm tt"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00 AM", inputValue.Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public void TimeFormatWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(parameters => parameters
            .Add(calendar => calendar.Format, "HH:mm")
            .Add(calendar => calendar.Value, new TimeOnly(00,00)));
            // Checking formatted value added to input
            Assert.Equal("HH:mm", Timepicker.Instance.Format);
            var inputElement = Timepicker.Find("input");
            var inputValue = inputElement.GetAttribute("value");
            Assert.Equal("00:00", inputValue);
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly?>>(("Format", "hh:mm"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00", inputValue.Replace('\u202F', ' ').Trim());
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly?>>(("Format", "hh:mm:ss"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00:00", inputValue);
            // Dynamically changing format and check formatted value added to input
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly?>>(("Format", "hh:mm tt"));
            inputElement = Timepicker.Find("input");
            inputValue = inputElement.GetAttribute("value");
            Assert.Equal("12:00 AM", inputValue.Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public void MaxValue1()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters => parameters
                .Add(calendar => calendar.Value, new TimeOnly(09, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            // Checking input element value less than Max value
            Timepicker.SetParametersAndRender(("Max", new DateTime(1900, 1, 1, 10, 30, 00)));
            Assert.Equal(new DateTime(1900, 1, 1, 10, 30, 00), Timepicker.Instance.Max);
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Equal("9:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            // Checking e-error class added to wrapper when value greater than Max value
            Timepicker.SetParametersAndRender(("Value", new TimeOnly(11, 30, 00)));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("11:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", parentContainer.ClassName);
            // Dynamically changing Max property
            Timepicker.SetParametersAndRender(("Max", new DateTime(1900, 1, 1, 15, 30, 00)));
            Assert.Equal(new DateTime(1900, 1, 1, 15, 30, 00), Timepicker.Instance.Max);
            // Checking e-error class not added when extending max value greater than value
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("11:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", parentContainer.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void MaxValueWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(parameters => parameters
                .Add(calendar => calendar.Value, new TimeOnly(09, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            // Checking input element value less than Max value
            Timepicker.SetParametersAndRender(("Max", new DateTime(1900, 1, 1, 10, 30, 00)));
            Assert.Equal(new DateTime(1900, 1, 1, 10, 30, 00), Timepicker.Instance.Max);
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Equal("9:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            // Checking e-error class added to wrapper when value greater than Max value
            Timepicker.SetParametersAndRender(("Value", new TimeOnly(11, 30, 00)));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("11:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", parentContainer.ClassName);
            // Dynamically changing Max property
            Timepicker.SetParametersAndRender(("Max", new DateTime(1900, 1, 1, 15, 30, 00)));
            Assert.Equal(new DateTime(1900, 1, 1, 15, 30, 00), Timepicker.Instance.Max);
            // Checking e-error class not added when extending max value greater than value
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("11:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", parentContainer.ClassName);
        }
        [Fact(Timeout = 10000)]
        public void MinValue1()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters => parameters
                .Add(calendar => calendar.Value, new TimeOnly(09, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            // Checking input element value lesser than Min value
            Timepicker.SetParametersAndRender(("Min", new DateTime(1900, 1, 1, 08, 30, 00)));
            Assert.Equal(new DateTime(1900, 1, 1, 08, 30, 00), Timepicker.Instance.Min);
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Equal("9:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            // Checking e-error class added to wrapper when value less than Min value
            Timepicker.SetParametersAndRender(("Value", new TimeOnly( 07, 30, 00)));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("7:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", parentContainer.ClassName);
            // Dynamically changing Max property
            Timepicker.SetParametersAndRender(("Min", new DateTime(1900, 1, 1, 06, 30, 00)));
            Assert.Equal(new DateTime(1900, 1, 1, 06, 30, 00), Timepicker.Instance.Min);
            // Checking e-error class not added when lowering min value lesser than value
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("7:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", parentContainer.ClassName);
        }
        [Fact(Timeout = 10000)]
        public void MinValueWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(parameters => parameters
                .Add(calendar => calendar.Value, new TimeOnly(09, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            // Checking input element value greater than Max value
            Timepicker.SetParametersAndRender(("Min", new DateTime(1900, 1, 1, 08, 30, 00)));
            Assert.Equal(new DateTime(1900, 1, 1, 08, 30, 00), Timepicker.Instance.Min);
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Equal("9:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            // Checking e-error class added to wrapper when value less than Min value
            Timepicker.SetParametersAndRender(("Value", new TimeOnly(07, 30, 00)));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("7:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", parentContainer.ClassName);
            // Dynamically changing Max property
            Timepicker.SetParametersAndRender(("Min", new DateTime(1900, 1, 1, 06, 30, 00)));
            Assert.Equal(new DateTime(1900, 1, 1, 06, 30, 00), Timepicker.Instance.Min);
            // Checking e-error class not added when lowering min value lesser than value
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("7:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", parentContainer.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void PlaceHolderWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(parameters =>
            parameters.Add(calendar => calendar.Placeholder, "Enter Time value"));
            // Checking Placeholder value
            Assert.Equal("Enter Time value", Timepicker.Instance.Placeholder);
            var inputElement = Timepicker.Find("input");
            Assert.Equal("Enter Time value", inputElement.GetAttribute("placeholder"));
            // Dynamically changing placeholder value
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly?>>(("Placeholder", "Choose Time"));
            Assert.Equal("Choose Time", Timepicker.Instance.Placeholder);
            inputElement = Timepicker.Find("input");
            Assert.Equal("Choose Time", inputElement.GetAttribute("placeholder"));
        }
       
        [Fact(Timeout = 10000)]
        public async Task ReadonlyWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(parameters => parameters
            .Add(calendar => calendar.Readonly, true));
            // Checking readonly attribute added to input element
            Assert.True(Timepicker.Instance.Readonly);
            var inputElement = Timepicker.Find("input");
            Assert.True(inputElement.HasAttribute("readonly"));
            // Checking popup not open when readonly is true
            await Timepicker.Instance.ShowPopupAsync();
            var isPopupOpen = false;
            try
            {
                Timepicker.Find(".e-popup-wrapper");
                isPopupOpen = true;
            }
            catch
            {
                isPopupOpen = false;
            }
            Assert.False(isPopupOpen);
            // Dynamically changing readonly to false
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly?>>(("ReadOnly", false));
            Assert.False(Timepicker.Instance.Readonly);
            inputElement = Timepicker.Find("input");
            Assert.False(inputElement.HasAttribute("readonly"));
            // Checking popup should open when readonly is false
            await Timepicker.Instance.ShowPopupAsync();
            try
            {
                Timepicker.Find(".e-popup-wrapper");
                isPopupOpen = true;
            }
            catch
            {
                isPopupOpen = false;
            }
            Assert.True(isPopupOpen);
        }

        [Fact(Timeout = 10000)]
        public void ShowClearbutton1()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters => parameters
            .Add(calendar => calendar.ShowClearButton, true));
            Assert.True(Timepicker.Instance.ShowClearButton);
            // Checking clear icon present inside input container
            var inputElement = Timepicker.Find("input");
            inputElement.Focus();
            var parentContainer = inputElement.ParentElement;
            var ClearIcon = parentContainer.QuerySelectorAll(".e-close");
            Assert.True(ClearIcon.Length > 0);
            // Dynamically disabling clear button
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly>>(("ShowClearButton", false));
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
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly>>(("ShowClearButton", true));
            Assert.True(Timepicker.Instance.ShowClearButton);
            // Checking clear icon present inside input container
            inputElement = Timepicker.Find("input");
            inputElement.Focus();
            parentContainer = inputElement.ParentElement;
            ClearIcon = parentContainer.QuerySelectorAll(".e-close");
            Assert.True(ClearIcon.Length > 0);
        }

        [Fact(Timeout = 10000)]
        public async void ShowClearbuttonWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(parameters => parameters
            .Add(calendar => calendar.ShowClearButton, true)
            .Add(calendar => calendar.Value, new TimeOnly(12, 30, 00)));
            Assert.True(Timepicker.Instance.ShowClearButton);
            // Checking clear icon present inside input container
            var inputElement = Timepicker.Find("input");
            inputElement.Focus();
            var parentContainer = inputElement.ParentElement;
            var ClearIcon = parentContainer.QuerySelectorAll(".e-close");
            Assert.True(ClearIcon.Length > 0);
            // Checking value clears on clicking clear icon
            ClearIcon[0].MouseDown();
            await Task.Delay(200);
            inputElement = Timepicker.Find("input");
            Assert.Null(inputElement.GetAttribute("value"));
            // Dynamically disabling clear button
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly?>>(("ShowClearButton", false));
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
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly?>>(("ShowClearButton", true));
            Assert.True(Timepicker.Instance.ShowClearButton);
            // Checking clear icon present inside input container
            inputElement = Timepicker.Find("input");
            inputElement.Focus();
            parentContainer = inputElement.ParentElement;
            ClearIcon = parentContainer.QuerySelectorAll(".e-close");
            Assert.True(ClearIcon.Length > 0);
        }

        [Fact(Timeout = 10000)]
        public async Task Step1()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters => parameters
                .Add(calendar => calendar.Step, 60)
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
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly>>(("Step", 90));
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
        public async Task StepWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(parameters => parameters
                .Add(calendar => calendar.Step, 60)
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
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly?>>(("Step", 90));
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
        public async Task StrictMode1()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(parameters => parameters
                .Add(calendar => calendar.StrictMode, true)
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
        public void StrictModeWithNullable1()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(parameters => parameters
            .Add(calendar => calendar.StrictMode, true));
            Assert.True(Timepicker.Instance.StrictMode);
            var containEle = Timepicker.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containEle.ClassName);
            Timepicker.SetParametersAndRender<SfTimePicker<TimeOnly?>>(("StrictMode", false));
            Assert.False(Timepicker.Instance.StrictMode);
            containEle = Timepicker.Find("input").ParentElement;
            Assert.DoesNotContain("e-error", containEle.ClassName);

        }

        [Fact(Timeout = 10000)]
        public void StrictModeMinMax1()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(param => param
                .Add(p => p.Value, new TimeOnly(00, 30, 00))
                .Add(p => p.StrictMode, true)
                .Add(p => p.Min, new DateTime(2020, 1, 3, 08, 30, 00))
                .Add(p => p.Max, new DateTime(2020, 1, 4, 10, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            // Value less than Min should be set to Min (compare as TimeOnly to avoid format variance)
            var expected = TimeOnly.FromDateTime(Timepicker.Instance.Min);
            Assert.Equal(expected, Timepicker.Instance.Value);
            // UI should display the corrected time in the forced format
            var inputEle = Timepicker.Find("input");
            Assert.Equal("8:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public void StrictModeMinMaxWithNullable1()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly?>>(param => param
                .Add(p => p.Value, new TimeOnly(00, 30, 00))
                .Add(p => p.StrictMode, true)
                .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
                .Add(p => p.Max, new DateTime(2020, 1, 1, 10, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            // Value less than Min should be set to Min (compare as TimeOnly)
            var expected = TimeOnly.FromDateTime(Timepicker.Instance.Min);
            Assert.Equal(expected, Timepicker.Instance.Value!.Value);
            // UI should display the corrected time in the forced format
            var inputEle = Timepicker.Find("input");
            Assert.Equal("8:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public void StrictModeMinGreaterthanMax1()
        {
            var Timepicker = RenderComponent<SfTimePicker<TimeOnly>>(param => param
            .Add(p => p.Value, new TimeOnly(07, 30, 00))
            .Add(p => p.StrictMode, true)
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 1, 07, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new TimeOnly(07, 30, 00), Timepicker.Instance.Value);
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
        public void Value_SetParametersAndRender_UpdatesInputDisplay_TimeOnly()
        {
            var first = new TimeOnly(9, 0, 0);
            var second = new TimeOnly(14, 45, 0);
            var cut = RenderComponent<SfTimePicker<TimeOnly>>(p => p
                .Add(x => x.Value, first)
                .Add(x => x.Format, "h:mm tt")
            );
            var input = cut.Find("input");
            Assert.Equal("9:00 AM", input.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
            cut.SetParametersAndRender(("Value", second));
            input = cut.Find("input");
            Assert.Equal("2:45 PM", input.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public async Task ShowPopupAsync_InvokesJs_TimeOnly()
        {
            JSInterop.SetupVoid("renderPopup", _ => true);
            var cut = RenderComponent<SfTimePicker<TimeOnly>>(p => p
                .Add(x => x.Value, new TimeOnly(10, 0, 0))
                .Add(x => x.Format, "h:mm tt")
            );
            await cut.Instance.ShowPopupAsync();
            var popup = cut.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popup.ClassName);
            Assert.Contains("e-timepicker", popup.ClassName);
            await Task.Delay(100);
            JSInterop.VerifyInvoke("renderPopup");
        }

        [Fact(Timeout = 10000)]
        public void TypingValidTime_UpdatesValue_And_NoError_TimeOnly()
        {
            var min = new DateTime(2026, 1, 1, 08, 00, 00);
            var max = new DateTime(2026, 1, 1, 18, 00, 00);
            var cut = RenderComponent<SfTimePicker<TimeOnly>>(p => p
                .Add(x => x.AllowEdit, true)
                .Add(p => p.Value, new TimeOnly(10, 30, 00))
                .Add(x => x.Min, min)
                .Add(x => x.Max, max)
                .Add(x => x.Format, "h:mm tt")
            );
            var input = cut.Find("input");
            input.Change("10:30 AM");
            var wrapper = input.ParentElement;
            Assert.DoesNotContain("e-error", wrapper.ClassName);
            var expected = new TimeOnly(10, 30, 0);
            Assert.Equal(expected, cut.Instance.Value);
            Assert.Equal("10:30 AM", input.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
        }
#endif
    }
}
