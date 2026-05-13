using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Calendars;
using Syncfusion.Blazor.Toolkit.Calendars.Internal;
using Syncfusion.Blazor.Toolkit.Inputs;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.TimePicker
{
    public class TimePicker : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public void DefaultInitialize()
        {
            var TimePicker = RenderComponent<SfTimePicker<DateTime>>();
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
            var TimePicker = RenderComponent<SfTimePicker<DateTime?>>();
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
            var TimePicker = RenderComponent<SfTimePicker<DateTime>>();
            Assert.Equal(default(DateTime), TimePicker.Instance.Value);
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
            Assert.Equal(default(DateTime), TimePicker.Instance.Value);
            Assert.Null(TimePicker.Instance.Width);
            Assert.Equal(1000, TimePicker.Instance.ZIndex);
        }

        [Fact(Timeout = 10000)]
        public void DefaultValueWithNullable()
        {
            var TimePicker = RenderComponent<SfTimePicker<DateTime?>>();
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
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
                .Add(val => val.Value, new DateTime(2020, 1, 1))
                .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1), Timepicker.Instance.Value);
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
            Assert.Equal(new DateTime(2020, 1, 1, 00, 30, 00), Timepicker.Instance.Value);
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
            Timepicker.SetParametersAndRender(("Value", new DateTime(2020, 1, 1, 00, 00, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 00, 00, 00), Timepicker.Instance.Value);
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
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(val => val.Value, new DateTime(2020, 1, 1))
                .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1), Timepicker.Instance.Value);
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
            Assert.Equal(new DateTime(2020, 1, 1, 00, 30, 00), Timepicker.Instance.Value);
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
            Timepicker.SetParametersAndRender(("Value", new DateTime(2020, 1, 1, 00, 00, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 00, 00, 00), Timepicker.Instance.Value);
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
        public async Task CssClass()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters =>
            parameters.Add(timepicker => timepicker.CssClass, "class1"));
            // Checking cssclass added in wrapper
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Contains("class1", parentContainer.ClassName);
            // Checking cssclass added in popup wrapper
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            Assert.Contains("class1", popupEle.ClassName);
            // Dynamically changing cssclass and checking class added in wrapper
            Timepicker.SetParametersAndRender(("CssClass", "class2"));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.DoesNotContain("class1", parentContainer.ClassName);
            Assert.Contains("class2", parentContainer.ClassName);
            // Checking cssclass added in popup wrapper
            await Timepicker.Instance.ShowPopupAsync();
            popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            Assert.Contains("class2", popupEle.ClassName);
            Assert.DoesNotContain("class1", popupEle.ClassName);

        }

        [Fact(Timeout = 10000)]
        public async Task CssClassWithHyphen()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.CssClass, "custom-class"));
            // Checking cssclass added in wrapper
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Contains("custom-class", parentContainer.ClassName);
            // Checking cssclass added in popup wrapper
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            Assert.Contains("custom-class", popupEle.ClassName);
            // Dynamically changing cssclass and checking class added in wrapper
            Timepicker.SetParametersAndRender(("CssClass", "custom-test-class"));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.DoesNotContain("custom-class", parentContainer.ClassName);
            Assert.Contains("custom-test-class", parentContainer.ClassName);
            // Checking cssclass added in popup wrapper
            await Timepicker.Instance.ShowPopupAsync();
            popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            Assert.Contains("custom-test-class", popupEle.ClassName);
            Assert.DoesNotContain("custom-class", popupEle.ClassName);
        }
        [Fact(Timeout = 10000)]
        public async Task CssClassWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters =>
            parameters.Add(timepicker => timepicker.CssClass, "class1"));
            // Checking cssclass added in wrapper
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Contains("class1", parentContainer.ClassName);
            // Checking cssclass added in popup wrapper
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            Assert.Contains("class1", popupEle.ClassName);
            // Dynamically changing cssclass and checking class added in wrapper
            Timepicker.SetParametersAndRender(("CssClass", "class2"));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.DoesNotContain("class1", parentContainer.ClassName);
            Assert.Contains("class2", parentContainer.ClassName);
            // Checking cssclass added in popup wrapper
            await Timepicker.Instance.ShowPopupAsync();
            popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            Assert.Contains("class2", popupEle.ClassName);
            Assert.DoesNotContain("class1", popupEle.ClassName);
        }

        [Fact(Timeout = 10000)]
        public async Task Enabled()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters =>
            parameters.Add(timepicker => timepicker.Disabled, true));
            // Checking e-disabled class added to the wrapper
            Assert.True(Timepicker.Instance.Disabled);
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            var isPopupOpen = false;
            Assert.Contains("e-disabled", parentContainer.ClassName);
            // Checking popup should not open when disabled
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
            Assert.False(isPopupOpen);
            Timepicker.SetParametersAndRender(("Disabled", false));
            // Checking e-disabled class not added to the wrapper
            Assert.False(Timepicker.Instance.Disabled);
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.DoesNotContain("e-disabled", parentContainer.ClassName);
            // Checking popup should open when enabled
            await Timepicker.Instance.ShowPopupAsync();
            Assert.NotNull(Timepicker.Find(".e-popup-wrapper"));
        }
        [Fact(Timeout = 10000)]
        public async Task EnabledWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters =>
            parameters.Add(timepicker => timepicker.Disabled, true));
            // Checking e-disabled class added to the wrapper
            Assert.True(Timepicker.Instance.Disabled);
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            var isPopupOpen = false;
            Assert.Contains("e-disabled", parentContainer.ClassName);
            // Checking popup should not open when disabled
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
            Assert.False(isPopupOpen);
            Timepicker.SetParametersAndRender(("Disabled", false));
            // Checking e-disabled class not added to the wrapper
            Assert.False(Timepicker.Instance.Disabled);
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.DoesNotContain("e-disabled", parentContainer.ClassName);
            // Checking popup should open when enabled
            await Timepicker.Instance.ShowPopupAsync();
            Assert.NotNull(Timepicker.Find(".e-popup-wrapper"));
        }

        [Fact(Timeout = 10000)]
        public void FloatlabelType()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
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
        public async Task FloatlabelTypeWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
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
            Timepicker.SetParametersAndRender(("Value", new DateTime(2021, 3, 3, 11, 15, 0)));
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
        public void DateFormat()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
            .Add(calendar => calendar.Format, "HH:mm"));
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
            .Add(calendar => calendar.Value, new DateTime(2020, 1, 1)));
            // Checking formatted value added to input
            Assert.Equal("HH:mm", Timepicker.Instance.Format);
            var inputElement = Timepicker.Find("input");
            var inputValue = inputElement.GetAttribute("value");
            Assert.Equal("00:00", inputValue);
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
        public void HtmlAttributes()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
            .Add(calendar => calendar.HtmlAttributes, new Dictionary<string, object>() {
                {"name", "timevalue" },
                {"class", "custom-timepicker" },
                {"required", "true" }
            }));
            // Checking attributes added to the input element
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Contains("custom-timepicker", parentContainer.ClassName);
            Assert.Equal("true", inputElement.GetAttribute("required"));
            Assert.Equal("timevalue", inputElement.GetAttribute("name"));
            // Dynamically changing HTML attributes and checking attributes added to the input element
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime>>(("HtmlAttributes", new Dictionary<string, object>() {
                {"name", "timevalue_1" },
                {"class", "custom-timepicker_1" },
                {"required", "false" }
            }));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Contains("custom-timepicker_1", parentContainer.ClassName);
            Assert.Equal("false", inputElement.GetAttribute("required"));
            Assert.Equal("timevalue_1", inputElement.GetAttribute("name"));
        }
        [Fact(Timeout = 10000)]
        public void HtmlAttributesWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
            .Add(calendar => calendar.HtmlAttributes, new Dictionary<string, object>() {
                {"name", "timevalue" },
                {"class", "custom-timepicker" },
                {"required", "true" }
            }));
            // Checking attributes added to the input element
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Contains("custom-timepicker", parentContainer.ClassName);
            Assert.Equal("true", inputElement.GetAttribute("required"));
            Assert.Equal("timevalue", inputElement.GetAttribute("name"));
            // Dynamically changing HTML attributes and checking attributes added to the input element
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(("HtmlAttributes", new Dictionary<string, object>() {
                {"name", "timevalue_1" },
                {"class", "custom-timepicker_1" },
                {"required", "false" }
            }));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Contains("custom-timepicker_1", parentContainer.ClassName);
            Assert.Equal("false", inputElement.GetAttribute("required"));
            Assert.Equal("timevalue_1", inputElement.GetAttribute("name"));
        }
        [Fact(Timeout = 10000)]
        public void ID()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
            .Add(calendar => calendar.ID, "timeid"));
            // Checking ID updated to the input element
            var inputElement = Timepicker.Find("input");
            Assert.Equal("timeid", Timepicker.Instance.ID);
            Assert.Equal("timeid", inputElement.GetAttribute("id"));
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime>>(("ID", "timeid1"));
            // Dynamically changing ID and checking changed ID updated to the input element
            inputElement = Timepicker.Find("input");
            Assert.Equal("timeid1", Timepicker.Instance.ID);
            Assert.Equal("timeid1", inputElement.GetAttribute("id"));
        }
        [Fact(Timeout = 10000)]
        public void IDwithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
            .Add(calendar => calendar.ID, "timeid"));
            // Checking ID updated to the input element
            var inputElement = Timepicker.Find("input");
            Assert.Equal("timeid", Timepicker.Instance.ID);
            Assert.Equal("timeid", inputElement.GetAttribute("id"));
            // Dynamically changing ID and checking changed ID updated to the input element
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(("ID", "timeid1"));
            inputElement = Timepicker.Find("input");
            Assert.Equal("timeid1", Timepicker.Instance.ID);
            Assert.Equal("timeid1", inputElement.GetAttribute("id"));
        }
        [Fact(Timeout = 10000)]
        public void MaxValue()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
                .Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 09, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            // Checking input element value less than Max value
            Timepicker.SetParametersAndRender(("Max", new DateTime(2020, 1, 1, 10, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 10, 30, 00), Timepicker.Instance.Max);
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Equal("9:30 AM", inputElement.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
            // Checking e-error class added to wrapper when value greater than Max value
            Timepicker.SetParametersAndRender(("Value", new DateTime(2020, 1, 1, 11, 30, 00)));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("11:30 AM", inputElement.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", parentContainer.ClassName);
            // Dynamically changing Max property
            Timepicker.SetParametersAndRender(("Max", new DateTime(2020, 1, 1, 15, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 15, 30, 00), Timepicker.Instance.Max);
            // Checking e-error class not added when extending max value greater than value
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("11:30 AM", inputElement.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", parentContainer.ClassName);
        }
        [Fact(Timeout = 10000)]
        public void MaxValueWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 09, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            // Checking input element value less than Max value
            Timepicker.SetParametersAndRender(("Max", new DateTime(2020, 1, 1, 10, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 10, 30, 00), Timepicker.Instance.Max);
            var inputElement = Timepicker.Find("input");
            var parentContainer = inputElement.ParentElement;
            Assert.Equal("9:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            // Checking e-error class added to wrapper when value greater than Max value
            Timepicker.SetParametersAndRender(("Value", new DateTime(2020, 1, 1, 11, 30, 00)));
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("11:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", parentContainer.ClassName);
            // Dynamically changing Max property
            Timepicker.SetParametersAndRender(("Max", new DateTime(2020, 1, 1, 15, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 15, 30, 00), Timepicker.Instance.Max);
            // Checking e-error class not added when extending max value greater than value
            inputElement = Timepicker.Find("input");
            parentContainer = inputElement.ParentElement;
            Assert.Equal("11:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", parentContainer.ClassName);
        }
        [Fact(Timeout = 10000)]
        public void MinValue()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
                .Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 09, 30, 00))
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
        public void MinValueWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 09, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            // Checking input element value greater than Max value
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
            parameters.Add(calendar => calendar.Placeholder, "Enter Time value"));
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
        [Fact(Timeout = 10000)]
        public void PlaceHolderWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters =>
            parameters.Add(calendar => calendar.Placeholder, "Enter Time value"));
            // Checking Placeholder value
            Assert.Equal("Enter Time value", Timepicker.Instance.Placeholder);
            var inputElement = Timepicker.Find("input");
            Assert.Equal("Enter Time value", inputElement.GetAttribute("placeholder"));
            // Dynamically changing placeholder value
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(("Placeholder", "Choose Time"));
            Assert.Equal("Choose Time", Timepicker.Instance.Placeholder);
            inputElement = Timepicker.Find("input");
            Assert.Equal("Choose Time", inputElement.GetAttribute("placeholder"));
        }
        [Fact(Timeout = 10000)]
        public async Task Readonly()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
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
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime>>(("ReadOnly", false));
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
        public async Task ReadonlyWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
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
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(("ReadOnly", false));
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
        public void ShowClearbutton()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
            .Add(calendar => calendar.ShowClearButton, true));
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
        public async void ShowClearbuttonWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
            .Add(calendar => calendar.ShowClearButton, true)
            .Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 12, 30, 00)));
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
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(("ShowClearButton", false));
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
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(("ShowClearButton", true));
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
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
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
        public async Task StepWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
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
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(("Step", 90));
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
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
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
        public void StrictModeWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
            .Add(calendar => calendar.StrictMode, true));
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
            .Add(p => p.StrictMode, true)
            .Add(p => p.Min, new DateTime(2020, 1, 3, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 4, 10, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            // Checking Value less than min set to min value
            Assert.Equal(Timepicker.Instance.Min, Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            Assert.Equal("8:30 AM", inputEle.GetAttribute("value")?.Replace('\u202F', ' ')?.Trim());
        }
        [Fact(Timeout = 10000)]
        public void StrictModeMinMaxWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 00, 30, 00))
            .Add(p => p.StrictMode, true)
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 1, 10, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            // Checking Value less than min set to min value
            Assert.Equal(Timepicker.Instance.Min, Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            Assert.Equal("8:30 AM", inputEle.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
        }
        [Fact(Timeout = 10000)]
        public void StrictModeMinGreaterthanMax()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 07, 30, 00))
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
            Assert.Equal("7:30 AM", inputEle.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", inputEle.ParentElement.ClassName);
        }
        [Fact(Timeout = 10000)]
        public void StrictModeMinGreaterthanMaxWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 07, 30, 00))
            .Add(p => p.StrictMode, true)
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 1, 07, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1, 07, 30, 00), Timepicker.Instance.Value);
            Assert.Equal(new DateTime(2020, 1, 1, 08, 30, 00), Timepicker.Instance.Min);
            Assert.Equal(new DateTime(2020, 1, 1, 07, 30, 00), Timepicker.Instance.Max);
            var inputEle = Timepicker.Find("input");
            Assert.Equal("7:30 AM", inputEle.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", inputEle.ParentElement.ClassName);
            Timepicker.SetParametersAndRender(("Min", new DateTime(2020, 1, 1, 07, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 07, 30, 00), Timepicker.Instance.Min);
            inputEle = Timepicker.Find("input");
            Assert.Equal("7:30 AM", inputEle.GetAttribute("value")?.Replace('\u202F', ' ')?.Trim());
            Assert.DoesNotContain("e-error", inputEle.ParentElement.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void TabIndex()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters =>
            parameters.Add(calendar => calendar.TabIndex, 1));
            var inputElement = Timepicker.Find("input");
            Assert.Equal("1", inputElement.GetAttribute("tabindex"));
            Timepicker.SetParametersAndRender(("TabIndex", 2));
            inputElement = Timepicker.Find("input");
            Assert.NotEqual("1", inputElement.GetAttribute("tabindex"));
            Assert.Equal("2", inputElement.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000)]
        public void TabIndexWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters =>
            parameters.Add(calendar => calendar.TabIndex, 1));
            var inputElement = Timepicker.Find("input");
            Assert.Equal("1", inputElement.GetAttribute("tabindex"));
            Timepicker.SetParametersAndRender(("TabIndex", 2));
            inputElement = Timepicker.Find("input");
            Assert.NotEqual("1", inputElement.GetAttribute("tabindex"));
            Assert.Equal("2", inputElement.GetAttribute("tabindex"));
        }
        [Fact(Timeout = 10000)]
        public async Task Value()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
                .Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 01, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            // Checking value added to input element
            Assert.Equal(new DateTime(2020, 1, 1, 01, 30, 00), Timepicker.Instance.Value);
            var inputElement = Timepicker.Find("input");
            Assert.Equal("1:30 AM", inputElement.GetAttribute("value")?.Replace('\u202F', ' ')?.Trim());
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
            Assert.Equal("1:30 AM", selectedLi[0].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());
            Assert.Equal("1:30 AM", selectedLi[0].GetAttribute("data-value")?.Replace('\u202F', ' ')?.Trim());
            await Timepicker.Instance.HidePopupAsync();
            // Dynamically changing value and checking value added to the input element
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime>>(("Value", new DateTime(2020, 1, 1, 02, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 02, 30, 00), Timepicker.Instance.Value);
            inputElement = Timepicker.Find("input");
            Assert.Equal("2:30 AM", inputElement.GetAttribute("value")?.Replace('\u202F', ' ')?.Trim());
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
            Assert.Equal("2:30 AM", selectedLi[0].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());
            Assert.Equal("2:30 AM", selectedLi[0].GetAttribute("data-value")?.Replace('\u202F', ' ')?.Trim());
        }
        [Fact(Timeout = 10000)]
        public async Task ValueWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 01, 30, 00))
                .Add(p => p.Format, "h:mm tt"));
            // Checking value added to input element
            Assert.Equal(new DateTime(2020, 1, 1, 01, 30, 00), Timepicker.Instance.Value);
            var inputElement = Timepicker.Find("input");
            Assert.Equal("1:30 AM", inputElement.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
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
            Assert.Equal("1:30 AM", selectedLi[0].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());
            Assert.Equal("1:30 AM", selectedLi[0].GetAttribute("data-value")?.Replace('\u202F', ' ').Trim());
            await Timepicker.Instance.HidePopupAsync();
            // Dynamically changing value and checking value added to the input element
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(("Value", new DateTime(2020, 1, 1, 02, 30, 00)));
            Assert.Equal(new DateTime(2020, 1, 1, 02, 30, 00), Timepicker.Instance.Value);
            inputElement = Timepicker.Find("input");
            Assert.Equal("2:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
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
            Assert.Equal("2:30 AM", selectedLi[0].TextContent.Replace("\n", String.Empty).Replace('\u202F', ' ').Trim());
            Assert.Equal("2:30 AM", selectedLi[0].GetAttribute("data-value").Replace('\u202F', ' ').Trim());
        }
        [Fact(Timeout = 10000)]
        public void ValueLessThanMinMaxRange()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 00, 30, 00))
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
        public void ValueLessThanMinMaxRangeWithNullable()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 00, 30, 00))
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 1, 10, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1, 00, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("12:30 AM", inputEle.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", ParentContainer.ClassName);
        }
        [Fact(Timeout = 10000)]
        public void ValueGreaterThanMinMaxRange()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 11, 30, 00))
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
            .Add(p => p.Value, new DateTime(2020, 1, 1, 11, 30, 00))
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
        public async Task OpenPopupWithRangeExceeded()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 07, 30, 00))
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 1, 10, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1, 07, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("7:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", ParentContainer.ClassName);
            // Open popup to check li count
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            var ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            var liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(5, liCollections.Length);
            var selectedLi = ulEle.QuerySelectorAll(".e-list-item.e-active");
            Assert.Equal(0, selectedLi.Length);
        }
        [Fact(Timeout = 10000)]
        public async Task OpenPopupWithRangeLesser()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 11, 30, 00))
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 1, 10, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1, 11, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("11:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", ParentContainer.ClassName);
            // Open popup to check li count
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            var ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            var liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(5, liCollections.Length);
            var selectedLi = ulEle.QuerySelectorAll(".e-list-item.e-active");
            Assert.Equal(0, selectedLi.Length);
        }
        [Fact(Timeout = 10000)]
        public async Task OpenPopupWithValueEqualToMax()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 01, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1, 08, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("8:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", ParentContainer.ClassName);
            // Open popup to check li count
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            var ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            var liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(18, liCollections.Length);
            var selectedLi = ulEle.QuerySelectorAll(".e-list-item.e-active");
            Assert.Equal(1, selectedLi.Length);
        }
        [Fact(Timeout = 10000)]
        public async Task OpenPopupWithValueEqualToMin()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1, 08, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("8:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", ParentContainer.ClassName);
            // Open popup to check li count
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            var ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            var liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(31, liCollections.Length);
            var selectedLi = ulEle.QuerySelectorAll(".e-list-item.e-active");
            Assert.Equal(1, selectedLi.Length);
        }
        [Fact(Timeout = 10000)]
        public async Task OpenPopupWithValueEqualToMinAndMax()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 01, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Format, "h:mm tt"));
            Assert.Equal(new DateTime(2020, 1, 1, 08, 30, 00), Timepicker.Instance.Value);
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Equal("8:30 AM", inputEle.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.DoesNotContain("e-error", ParentContainer.ClassName);
            // Open popup to check li count
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            var ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            var liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(1, liCollections.Length);
            var selectedLi = ulEle.QuerySelectorAll(".e-list-item.e-active");
            Assert.Equal(1, selectedLi.Length);
        }
        [Fact(Timeout = 10000)]
        public async Task OpenPopupWithMaxLessThanMin()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
            .Add(p => p.Max, new DateTime(2020, 1, 1, 07, 30, 00))
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00)));
            var inputEle = Timepicker.Find("input");
            var ParentContainer = inputEle.ParentElement;
            Assert.Contains("e-error", ParentContainer.ClassName);
            // Open popup to check li count
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            var ulEle = popupEle.QuerySelector("ul");
            Assert.Null(ulEle);
        }

        [Fact(Timeout = 1000000)]
        public async Task KeyboardActions()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
           .Add(p => p.Format, "HH:mm")
           .Add(p => p.Step, 60)
           .Add(p => p.Value, TimeValue)
           );
            await Timepicker.Instance.ShowPopupAsync();
            await Task.Delay(100);
            var args = new KeyActions
            {
                Action = "down",
                Key = "ArrowDown"
            };

            await Timepicker.Instance.KeyboardHandlerAsync(args);
            await Task.Delay(100);
            args = new KeyActions
            {
                Action = "enter",
                Key = "Enter"
            };
            Timepicker.Instance.EnableMask = true;
            await Timepicker.Instance.KeyboardHandlerAsync(args);
            await Task.Delay(100);
            args = new KeyActions
            {
                Action = "open",
                Key = "Open"
            };
            await Timepicker.Instance.KeyboardHandlerAsync(args);
            await Task.Delay(100);
            args = new KeyActions
            {
                Action = "close",
                Key = "Close"
            };
            await Timepicker.Instance.KeyboardHandlerAsync(args);
            await Task.Delay(100); args = new KeyActions
            {
                Action = "escape",
                Key = "Escape"
            };
            await Timepicker.Instance.KeyboardHandlerAsync(args);
            await Task.Delay(100);


        }

        [Fact(Timeout = 10000)]
        public async Task ClickingTimeIcon()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
           .Add(p => p.Format, "HH:mm")
           .Add(p => p.Step, 60)
           .Add(p => p.Value, TimeValue)
            );
            var containerEle = Timepicker.Find("input").ParentElement;
            var dateIcon = containerEle.QuerySelector(".e-clock");
            dateIcon.MouseDown();
            await Task.Delay(200);
            var popupEle = Timepicker.Find(".e-popup");
            Assert.Contains("e-popup", popupEle.ClassName);
        }

        [Fact(Timeout = 10000)]
        public async Task TimeIconHandler()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
           .Add(p => p.Format, "HH:mm")
           .Add(p => p.Step, 60)
           .Add(p => p.Value, TimeValue)
           .Add(p => p.FullScreen, false)
            );
            var containerEle = Timepicker.Find("input").ParentElement;
            var dateIcon = containerEle.QuerySelector(".e-clock");
            dateIcon.MouseDown();
            await Task.Delay(100);
            await Timepicker.Instance.ClosePopupAsync();
            await Task.Delay(100);
            Assert.False(Timepicker.Instance.FullScreen);

        }

        [Fact(Timeout = 10000)]
        public async Task HidePopup()
        {
            var dummy = new Dictionary<string, object>();
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
           .Add(p => p.Format, "HH:mm")
           .Add(p => p.Step, 60)
           .Add(p => p.Value, TimeValue)
           .Add(p => p.InputAttributes , dummy)
            );
            var containerEle = Timepicker.Find("input");
            var args = new KeyboardEventArgs { Code = "0", Key = "0" };
            containerEle.Input(args);
            await Task.Delay(100);
            await Timepicker.Instance.HidePopup();
            Assert.Equal(TimeValue , Timepicker.Instance.Value);
            Assert.Equal(dummy , Timepicker.Instance.InputAttributes);
            await Timepicker.Instance.FocusOutAsync();
        }

        [Fact(Timeout = 10000)]
        public void UpdateChildProperties()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
           .Add(p => p.Format, "HH//mm//")
           .Add(p => p.Step, 60)
           .Add(p => p.Value, TimeValue)
            );
            var containerEle = Timepicker.Find("input");
            Timepicker.Instance.UpdateChildProperties(null);
            
            Assert.Equal(TimeValue, Timepicker.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public async Task UpdateFieldSetStatus()
        {

            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
           .Add(p => p.Format, "HH:mm")
           .Add(p => p.Step, 60)
           .Add(p => p.Value, TimeValue)
            );
            var containerEle = Timepicker.Find("input");
            var args = new KeyboardEventArgs { Code = "0", Key = "0" };
            containerEle.Input(args);
            await Task.Delay(100);
            await Timepicker.InvokeAsync(() => Timepicker.Instance.UpdateFieldSetStatus(false));
            Assert.Equal(TimeValue, Timepicker.Instance.Value);
            

        }
        [Fact(Timeout = 10000)]
        public void SetHtmlAttributes()
        {
            var attributes = new Dictionary<string, object> { {  "value",  "TestValue" } , { "tabindex", "5" } , { "readonly", "true" } , { "disabled", "true" } , { "max", "2024-08-09" } , { "min", "2023-08-09" } , { "step", "10" } };
            var dummy = new Dictionary<string, object>();
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
           .Add(p => p.Format, "HH:mm")
           .Add(p => p.HtmlAttributes, attributes)
            );
            var TimePInstance = Timepicker.Instance;

            Assert.Equal(10, TimePInstance.Step);
            Assert.True(TimePInstance.Disabled);
            Assert.True(TimePInstance.Readonly);
            Assert.False(TimePInstance.AllowEdit);
            Assert.Equal(5, TimePInstance.TabIndex);
            Assert.Equal(5, TimePInstance.TabIndex);
        }

        /// <summary>
        /// Commented out the test cases as certain properties and classes are inaccessible since they are defined as internal
        /// </summary>
        //[Fact(Timeout = 10000)]
        //public void TimePickerClientClass()
        //{
        //    var configTimePickerClient = new TimePickerClientProps<DateTime>
        //    {
        //        EnableRtl = true,
        //        Enabled = true,
        //        ZIndex = 2000,
        //        KeyConfigs = new Dictionary<string, object>
        //        {
        //            { "Enter", "Submit" },
        //            { "Escape", "Cancel" }
        //        },
        //        Value = new DateTime(2024, 8, 9, 14, 30, 0),
        //        Width = "400px",
        //        ScrollTo = new DateTime(2024, 8, 9, 14, 30, 0),
        //        Step = 15,
        //        EnableMask = true,
        //        Format = "HH:mm",
        //        IsFocused = true,
        //        IsRendered = true,
        //        DayName = new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" },
        //        MonthName = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" },
        //        DayAbbreviatedName = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" },
        //        MonthAbbreviatedName = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },
        //        DayPeriod = new[] { "AM", "PM" },
        //        MaskPlaceholderDictionary = new Dictionary<string, string>
        //        {
        //            { "hour", "hh" },
        //            { "minute", "mm" },
        //            { "second", "ss" }
        //        },
        //        FloatLabelType = "Auto",
        //        Placeholder = "Select a time",
        //        ValueString = "14:30",
        //        Offset = "+00:00",
        //        Navigated = true,
        //        IsBlurred = false,
        //        Readonly = false
        //    };

        //    Assert.True(configTimePickerClient.EnableRtl);
        //    Assert.True(configTimePickerClient.Enabled);
        //    Assert.Equal(2000, configTimePickerClient.ZIndex);

        //    Assert.Equal(2, configTimePickerClient.KeyConfigs.Count);
        //    Assert.Equal("Submit", configTimePickerClient.KeyConfigs["Enter"]);
        //    Assert.Equal("Cancel", configTimePickerClient.KeyConfigs["Escape"]);

        //    Assert.Equal(new DateTime(2024, 8, 9, 14, 30, 0), configTimePickerClient.Value);
        //    Assert.Equal("400px", configTimePickerClient.Width);
        //    Assert.Equal(new DateTime(2024, 8, 9, 14, 30, 0), configTimePickerClient.ScrollTo);
        //    Assert.Equal(15, configTimePickerClient.Step);
        //    Assert.True(configTimePickerClient.EnableMask);
        //    Assert.Equal("HH:mm", configTimePickerClient.Format);
        //    Assert.True(configTimePickerClient.IsFocused);
        //    Assert.True(configTimePickerClient.IsRendered);

        //    Assert.Equal(new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" }, configTimePickerClient.DayName);
        //    Assert.Equal(new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" }, configTimePickerClient.MonthName);
        //    Assert.Equal(new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" }, configTimePickerClient.DayAbbreviatedName);
        //    Assert.Equal(new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }, configTimePickerClient.MonthAbbreviatedName);
        //    Assert.Equal(new[] { "AM", "PM" }, configTimePickerClient.DayPeriod);

        //    Assert.Equal(new Dictionary<string, string>
        //    {
        //        { "hour", "hh" },
        //        { "minute", "mm" },
        //        { "second", "ss" }
        //    }, configTimePickerClient.MaskPlaceholderDictionary);

        //    Assert.Equal("Auto", configTimePickerClient.FloatLabelType);
        //    Assert.Equal("Select a time", configTimePickerClient.Placeholder);
        //    Assert.Equal("14:30", configTimePickerClient.ValueString);
        //    Assert.Equal("+00:00", configTimePickerClient.Offset);
        //    Assert.True(configTimePickerClient.Navigated);
        //    Assert.False(configTimePickerClient.IsBlurred);
        //    Assert.False(configTimePickerClient.Readonly);
        //}

        //[Fact(Timeout = 10000)]
        //public void TimePickerMaskPlaceholder()
        //{
        //    var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
        //        .Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 01, 30, 00))
        //        .Add(p => p.EnableMask, true)
        //        .Add(p => p.Width, "100px")
        //        .Add(p => p.ScrollTo, DateTime.Now)
        //        .Add(p => p.ZIndex, 10000)
        //        .Add(p => p.KeyConfigs, new Dictionary<string, object>())
        //        .AddChildContent<TimePickerMaskPlaceholder>(param => param
        //            .Add(p => p.Hour, "hour")
        //            .Add(p => p.Minute, "minute")
        //            .Add(p => p.Second, "second")));
        //    Timepicker.Instance.TimePickerLocale = "#TimePickerLocale";
        //    Timepicker.Instance.CurrentMaskFormat = "#CurrentMaskFormat";
        //    Assert.True(Timepicker.Instance.IsChangeValue);
        //}

        [Fact(Timeout = 10000)]
        public void TimePickerModelClass()
        {
            var dummyEvent = new EventCallback<object>();
            var model = new TimePickerModel<DateTime>
            {
                Blur = dummyEvent,
                Change = dummyEvent,
                Cleared = dummyEvent,
                Close = dummyEvent,
                Created = dummyEvent,
                Destroyed = dummyEvent,
                Focus = dummyEvent,
                ItemRender = dummyEvent,
                Open = dummyEvent,
                AllowEdit = false,
                CssClass = "custom-class",
                EnablePersistence = true,
                Disabled = true,
                FloatLabelType = FloatLabelType.Always,
                Format = "HH:mm",
                HtmlAttributes = new { disabled = "true" },
                KeyConfigs = new { Enter = "submit" },
                Locale = "en-US",
                Max = new DateTime(2100, 01, 01, 23, 59, 59),
                Min = new DateTime(2000, 01, 01, 00, 00, 00),
                Placeholder = "Select time",
                Readonly = true,
                ScrollTo = new DateTime(2000, 01, 01, 12, 00, 00),
                ShowClearButton = false,
                Step = 15,
                StrictMode = true,
                Width = "200px",
                ZIndex = 2000
            };

            Assert.Equal(dummyEvent, model.Blur);
            Assert.Equal(dummyEvent, model.Change);
            Assert.Equal(dummyEvent, model.Cleared);
            Assert.Equal(dummyEvent, model.Close);
            Assert.Equal(dummyEvent, model.Created);
            Assert.Equal(dummyEvent, model.Destroyed);
            Assert.Equal(dummyEvent, model.Focus);
            Assert.Equal(dummyEvent, model.Open);
            Assert.Equal(dummyEvent, model.ItemRender);
            Assert.False(model.AllowEdit);
            Assert.Equal("custom-class", model.CssClass);
            Assert.True(model.EnablePersistence);
            Assert.True(model.Disabled);
            Assert.Equal(FloatLabelType.Always, model.FloatLabelType);
            Assert.Equal("HH:mm", model.Format);
            Assert.Equal(new { disabled = "true" }, model.HtmlAttributes);
            Assert.Equal(new { Enter = "submit" }, model.KeyConfigs);
            Assert.Equal("en-US", model.Locale);
            Assert.Equal(new DateTime(2100, 01, 01, 23, 59, 59), model.Max);
            Assert.Equal(new DateTime(2000, 01, 01, 00, 00, 00), model.Min);
            Assert.Equal("Select time", model.Placeholder);
            Assert.True(model.Readonly);
            Assert.Equal(new DateTime(2000, 01, 01, 12, 00, 00), model.ScrollTo);
            Assert.False(model.ShowClearButton);
            Assert.Equal(15, model.Step);
            Assert.True(model.StrictMode);
            Assert.Equal("200px", model.Width);
            Assert.Equal(2000, model.ZIndex);

        }

        [Fact(Timeout = 10000)]
        public async Task FocusOutHandler()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 00, 30, 00))
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 1, 10, 30, 00)));
            var inputEle = Timepicker.Find("input");
            inputEle.Focus();
            var args = new ChangeEventArgs { Value = "08:17" };
            inputEle.Change(args);
            await Task.Delay(10);
            var jsInterop = Services.GetRequiredService<IJSRuntime>();
            jsInterop.InvokeVoidAsync("eval", "document.activeElement.blur();").GetAwaiter().GetResult();
            Assert.NotNull(Timepicker.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public async Task ChangeHandler()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(param => param
            .Add(p => p.Value, new DateTime(2020, 1, 1, 00, 30, 00))
            .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
            .Add(p => p.Max, new DateTime(2020, 1, 1, 10, 30, 00)));
            var inputEle = Timepicker.Find("input");
            var args = new ChangeEventArgs { Value ="08:17"};
            inputEle.Change(args);
            await Task.Delay(50);
            Assert.NotNull(Timepicker.Instance.Value);

        }

        [Fact(Timeout = 10000)]
        public async Task AriaControlsMatchesPopupId()
        {
            var Calendar = RenderComponent<SfTimePicker<DateTime?>>(parameters =>
            parameters.Add(Cal => Cal.ID, "TimePicker"));
            await Calendar.Instance.ShowPopupAsync();
            var popupEle = Calendar.Find(".e-popup");
            var inputEle = Calendar.Find("input");
            var ariaControls = inputEle.GetAttribute("aria-controls");
            var popupId = popupEle.GetAttribute("id");
            Assert.False(string.IsNullOrWhiteSpace(ariaControls));
            Assert.False(string.IsNullOrWhiteSpace(popupId));
            Assert.Equal(popupId, ariaControls);
        }

        [Fact(Timeout = 10000, DisplayName = "DateTimeKind is set to Unspecified in DateTimePicker's value")]
        public async Task TimePicker_KindStaysLocal_OnSelection()
        {
            var Val = new DateTime(2022, 3, 1, 10, 0, 0, DateTimeKind.Local);
            var dateInstance = RenderComponent<SfTimePicker<DateTime?>>(param => param
                .Add(p => p.Value, Val)
                .Add(p => p.Format, "hh:mm tt") // Force 12-hour format with AM/PM
            );
            // Value preserved and Kind is Local
            Assert.Equal(Val, dateInstance.Instance.Value);
            Assert.Equal(DateTimeKind.Local, dateInstance.Instance.Value!.Value.Kind);
            // Open popup and wait for it to render
            await dateInstance.Instance.ShowPopupAsync();
            dateInstance.WaitForElement(".e-popup");
            // Select the third list item
            var popupEle = dateInstance.Find(".e-popup");
            var liCollec = popupEle.QuerySelectorAll("li");
            liCollec[2].Click();
            // With "hh:MM tt", hour is two-digit; expect leading zero
            var inputEle1 = dateInstance.Find("input");
            var actual = inputEle1.GetAttribute("value").Replace('\u202F', ' ').Trim();
            await Task.Delay(1000);
            Assert.Contains("01:00 AM", actual);
            // Kind remains Local after selection
            Assert.Equal(DateTimeKind.Local, dateInstance.Instance.Value!.Value.Kind);
        }

        [Fact(Timeout = 10000)]
        public void Value_SetParametersAndRender_UpdatesInputDisplay()
        {
            var first = new DateTime(2020, 1, 1, 9, 0, 0);
            var second = new DateTime(2020, 1, 1, 14, 30, 0);
            var cut = RenderComponent<SfTimePicker<DateTime>>(p => p
                .Add(x => x.Value, first)
                .Add(x => x.Format, "h:mm tt")
            );
            var input = cut.Find("input");
            Assert.Equal("9:00 AM", input.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
            cut.SetParametersAndRender(("Value", second));
            input = cut.Find("input");
            Assert.Equal("2:30 PM", input.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
        }

        [Fact(Timeout = 10000)]
        public async System.Threading.Tasks.Task ShowPopupAsync_InvokesJs_And_RendersPopup()
        {
            JSInterop.SetupVoid("renderPopup", _ => true);
            var cut = RenderComponent<SfTimePicker<DateTime>>(p => p
                .Add(x => x.Value, new DateTime(2020, 1, 1, 9, 0, 0))
                .Add(x => x.Format, "h:mm tt")
            );
            await cut.Instance.ShowPopupAsync();
            var popup = cut.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popup.ClassName);
            Assert.Contains("e-timepicker", popup.ClassName);
            await Task.Delay(1000);
            JSInterop.VerifyInvoke("renderPopup");
        }

        [Fact(Timeout = 10000)]
        public void TypingValidTime_UpdatesValue_And_NoError()
        {
            var min = new DateTime(2020, 1, 1, 08, 00, 00);
            var max = new DateTime(2020, 1, 1, 18, 00, 00);
            var cut = RenderComponent<SfTimePicker<DateTime>>(p => p
                .Add(x => x.AllowEdit, true)
                .Add(x => x.Value, new DateTime(2020, 1, 1, 10, 30, 00))
                .Add(x => x.Min, min)
                .Add(x => x.Max, max)
                .Add(x => x.Format, "h:mm tt")
            );
            var input = cut.Find("input");
            input.Change("10:30 AM");
            var wrapper = input.ParentElement;
            Assert.DoesNotContain("e-error", wrapper.ClassName);
            var expected = new DateTime(2020, 1, 1, 10, 30, 0);
            Assert.Equal(expected, cut.Instance.Value);
            Assert.Equal("10:30 AM", input.GetAttribute("value")?.Replace('\u202F', ' ').Trim());
        }

        public DateTime TimeValue { get; set; } = DateTime.Now;
    }
}
