using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.TimePicker
{
    public class TimePickerAccessibility: BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public async Task DefaultAccessibility()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>();
            var inputElement = Timepicker.Find("input");
            // Input element Accessibility
            Assert.Equal("combobox", inputElement.GetAttribute("role"));
            Assert.Equal("off", inputElement.GetAttribute("autocomplete"));
            Assert.Equal("false", inputElement.GetAttribute("aria-disabled"));
            Assert.Equal("true", inputElement.GetAttribute("aria-haspopup"));
            Assert.Equal("list", inputElement.GetAttribute("aria-autocomplete"));
            Assert.Equal(null, inputElement.GetAttribute("aria-activedescendant"));
            //Assert.Equal(Timepicker.Instance.ID+"_options", inputElement.GetAttribute("aria-owns"));// Previous button next button Accessibility
            Assert.Equal("false", inputElement.GetAttribute("aria-disabled"));
            Assert.Equal("false", inputElement.GetAttribute("aria-invalid"));
            Assert.Equal("false", inputElement.GetAttribute("aria-expanded"));
            // Popup element's Accessibility
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            var ulEle = popupEle.QuerySelector("ul");
            Assert.Contains("e-ul", ulEle.ClassName);
            var liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(48, liCollections.Length);
            Assert.Null(liCollections[0].GetAttribute("aria-selected"));
        }

        [Fact(Timeout = 10000)]
        public async Task InputElementWhenPopupOpen()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>();
            var inputElement = Timepicker.Find("input");
            // Input element Accessibility when opening the popup
            await Timepicker.Instance.ShowPopupAsync();
            var popupEle = Timepicker.Find(".e-popup-wrapper");
            Assert.Contains("e-popup", popupEle.ClassName);
            Assert.Contains("e-timepicker", popupEle.ClassName);
            Assert.Equal("true", inputElement.GetAttribute("aria-haspopup"));
            Assert.Equal("true", inputElement.GetAttribute("aria-expanded"));
        }
        [Fact(Timeout = 10000)]
        public async Task InputAndPopupWhenValueSelected()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters.
            Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 00, 30, 00)));
            var inputElement = Timepicker.Find("input");
            // Input element Accessibility when predefined value
            Assert.Equal("false", inputElement.GetAttribute("aria-invalid"));
            // li element Accesibility when predefined value
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
        }
        [Fact(Timeout = 10000)]
        public async Task InValidValue()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(param => param
                .Add(p => p.Value, new DateTime(2020, 1, 1, 00, 30, 00))
                .Add(p => p.Min, new DateTime(2020, 1, 1, 08, 30, 00))
                .Add(p => p.Max, new DateTime(2020, 1, 1, 10, 30, 00))
                .Add(p => p.Format, "hh:mm tt")          // Force 12-hour format
            );
            Assert.Equal(new DateTime(2020, 1, 1, 00, 30, 00), Timepicker.Instance.Value);
            var inputElement = Timepicker.Find("input");
            var ParentContainer = inputElement.ParentElement;
            Assert.Equal("12:30 AM", inputElement.GetAttribute("value").Replace('\u202F', ' ').Trim());
            Assert.Contains("e-error", ParentContainer.ClassName);
            Assert.Equal("true", inputElement.GetAttribute("aria-invalid"));
        }
    }
}
