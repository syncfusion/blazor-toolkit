using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.TimePicker
{
    
    public class PublicMethods: BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public async Task ShowMethod()
        {
            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
               .Add(val => val.Value, new DateTime(2020, 1, 1))
               .Add(p => p.Format, "hh:mm tt") // Force 12-hour format with AM/PM
            );
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
        }
    }
}
