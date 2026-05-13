using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.DateTimePicker
{
    public class DateTimePickerEvents : BunitTestContext
    {
        const string FORMATFULLDATE = "dddd, MMMM d, yyyy";
        const string FORMATDATE = " d ";
        const string TITLE_SEPARATOR = " - ";
        const string FORMAT_YEAR = "yyyy";
        private string shortPattern { get; set; } = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        private string fullPattern { get; set; } = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
        private string GetNativeDigits(string formatValue, string[] nativeDigits)
        {
            return formatValue.Replace("0", nativeDigits[0])
                .Replace("1", nativeDigits[1])
                .Replace("2", nativeDigits[2])
                .Replace("3", nativeDigits[3])
                .Replace("4", nativeDigits[4])
                .Replace("5", nativeDigits[5])
                .Replace("6", nativeDigits[6])
                .Replace("7", nativeDigits[7])
                .Replace("8", nativeDigits[8])
                .Replace("9", nativeDigits[9]);
        }
        private string GetDateFormat<T>(T date, string format = null, string culture = null)
        {
            try
            {
                var currentCulture = CultureInfo.CurrentCulture;
                IFormattable dateValue = date as IFormattable;
                var dateCulture = dateValue.ToString(format, currentCulture);
                dateCulture = GetNativeDigits(dateCulture, currentCulture.NumberFormat.NativeDigits);
                return dateCulture;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [Fact(Timeout = 10000)]
        public async Task DefaultEvents()
        {
            int createCount = 0;
            int focusCount = 0;
            int openCount = 0;
            int changeCount = 0;
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param
                .Add(p => p.Value, new DateTime(2021, 3, 5))
                .Add(p => p.Created, (object args) =>
                {
                    createCount++;
                    Assert.Equal(1, createCount);
                })
                .Add(p => p.OnFocus, (FocusEventArgs args) =>
                {
                    focusCount++;
                    Assert.Equal(1, focusCount);
                })
                .Add(p => p.OnOpen, (PopupObjectArgs args) =>
                {
                    openCount++;
                    Assert.Equal(1, openCount);
                    Assert.False(args.Cancel);
                })
                .Add(p => p.Selected, (SelectedEventArgs<DateTime?> args) =>
                {
                    Assert.Equal(new DateTime(2021, 3, 9), args.Value);
                })
                .Add(p => p.ValueChange, (ChangedEventArgs<DateTime?> args) =>
                {
                    changeCount++;
                    Assert.Equal(1, changeCount);
                })
            );

            var inputEle = dateInstance.Find("input");
            inputEle.Focus();
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            tRows[2].QuerySelectorAll("td")[2].Click();
            Assert.Equal(new DateTime(2021, 3, 9), dateInstance.Instance.Value);
            inputEle = dateInstance.Find("input");
            var inputValue = GetDateFormat(dateInstance.Instance.Value, shortPattern);
            Assert.Equal(inputValue, inputEle.GetAttribute("value"));
        }

        [Fact(Timeout = 10000)]
        public async Task PreventOpenEvent()
        {
            int openCount = 0;
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param
                .Add(p => p.Value, new DateTime(2021, 3, 5))
                .Add(p => p.OnOpen, (PopupObjectArgs args) =>
                {
                    openCount++;
                    Assert.Equal(1, openCount);
                    Assert.False(args.Cancel);
                    args.Cancel = true;
                })
            );
            var inputEle = dateInstance.Find("input");
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.FindAll(".e-popup");
            Assert.Equal(0, popupEle.Count);
        }

        [Fact(Timeout = 10000)]
        public async Task NavigatedEvent()
        {
            int navigateCount = 0;
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param
                .Add(p => p.Value, new DateTime(2021, 3, 5))
                .Add(p => p.Navigated, (NavigatedEventArgs args) =>
                {
                    navigateCount++;
                    Assert.Equal(1, navigateCount);
                    Assert.Equal("Month", args.View);
                    Assert.Equal(new DateTime(2021, 4, 5), args.Date);
                })
            );

            var inputEle = dateInstance.Find("input");
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Month", dateInstance.Instance.CurrentView());
            var buttonList = popupEle.QuerySelectorAll("button");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(new DateTime(2021, 3, 5).ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(2021, 3, 5).Day.ToString(), tableElement.QuerySelector(".e-selected").FirstElementChild.TextContent);
            Assert.Equal(new DateTime(2021, 3, 5).ToString("dddd, MMMM d, yyyy"), tableElement.QuerySelector(".e-selected").FirstElementChild.GetAttribute("title"));
            buttonList[1].Click();
            buttonList = popupEle.QuerySelectorAll("button");
            tableElement = popupEle.QuerySelector("table");
            parentContainer = tableElement.ParentElement.ParentElement;
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(2021, 4, 5).ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
        }

        [Fact(Timeout = 10000)]
        public async Task RenderDayCellEvent()
        {
            int dayCell = 27;
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param
                .Add(p => p.Value, new DateTime(2021, 3, 5))
                .Add(p => p.DayCellRendering, (RenderDayCellEventArgs args) =>
                {
                    Assert.False(args.IsDisabled);
                    Assert.False(args.IsOutOfRange);
                    Assert.Equal(new DateTime(2021, 2, 1).AddDays(dayCell), args.Date);
                    dayCell++;
                })
            );
            var inputEle = dateInstance.Find("input");
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            Assert.Equal("Month", dateInstance.Instance.CurrentView());
            var buttonList = popupEle.QuerySelectorAll("button");
            var parentContainer = tableElement.ParentElement.ParentElement;
            Assert.Equal(new DateTime(2021, 3, 5).ToString("MMMM yyyy"), parentContainer.QuerySelector(".e-header").FirstElementChild.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
        }

        [Fact(Timeout = 10000)]
        public async Task PreventCloseEvent()
        {
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param
                .Add(p => p.Value, new DateTime(2021, 3, 5))
                .Add(p => p.OnClose, (PopupObjectArgs args) =>
                {
                    args.Cancel = true;
                })
            );
            var inputEle = dateInstance.Find("input");
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            Assert.NotNull(popupEle);
            await dateInstance.Instance.HidePopupAsync();
            Assert.NotNull(popupEle);
        }

        [Fact(Timeout = 10000)]
        public async Task ClearedEvent()
        {
            var clearCount = 0;
            var dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param
                .Add(p => p.ShowClearButton, true)
                .Add(p => p.Value, new DateTime(2021, 3, 5))
                .Add(p => p.Cleared, (ClearedEventArgs args) =>
                {
                    clearCount++;
                    Assert.Equal(1, clearCount);
                })
            );
            var inputEle = dateInstance.Find("input");
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            var containerEle = dateInstance.Find("input").ParentElement;
            var clearEle = containerEle.Children[1];
            clearEle.MouseDown();
            await Task.Delay(200);
            inputEle = dateInstance.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            Assert.Null(dateInstance.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public async Task ResetValueInChangeEvent()
        {
            int changeCount = 0;
            DateTime? dateVal = new DateTime(2021, 3, 5);
            IRenderedComponent<SfDateTimePicker<DateTime?>> dateInstance = null;
            dateInstance = RenderComponent<SfDateTimePicker<DateTime?>>(param => param
                .Add(p => p.Value, dateVal)
                .Add(p => p.ValueChanged, (DateTime? value) => { dateVal = value; })
                .Add(p => p.ValueChange, (ChangedEventArgs<DateTime?> args) =>
                {
                    changeCount++;
                    dateVal = null;
                    dateInstance.SetParametersAndRender(("Value", (DateTime?)null));
                })
            );
            var inputEle = dateInstance.Find("input");
            await dateInstance.Instance.ShowPopupAsync();
            var popupEle = dateInstance.Find(".e-popup");
            var tableElement = popupEle.QuerySelector("table");
            var tRows = tableElement.QuerySelectorAll("tr");
            tRows[2].QuerySelectorAll("td")[2].Click();
            await Task.Delay(100);
            Assert.Null(dateInstance.Instance.Value);
            inputEle = dateInstance.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
        }
    }
}
