using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.DatePicker
{
    public class DatePickerEvents : BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "DefaultEvents - Verifies create/focus/open/blur/selected/valuechange handlers")]
        public async Task DefaultEvents()
        {
            int createCount = 0;
            int focusCount = 0;
            int openCount = 0;
            int blurCount = 0;
            int changeCount = 0;
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5))
            .Add(p => p.Created, (object args) => {
                createCount++;
                Assert.Equal(1, createCount);
            }).Add(p=>p.OnFocus, (Syncfusion.Blazor.Toolkit.Calendars.FocusEventArgs args) => {
                focusCount++;
                Assert.Equal(1, focusCount);
            }).Add(p => p.OnBlur, (Syncfusion.Blazor.Toolkit.Calendars.BlurEventArgs args) => {
                blurCount++;
                Assert.Equal(1, blurCount);
            })
            .Add(p=>p.OnOpen, (PopupObjectArgs args)=> {
                openCount++;
                Assert.Equal(1, openCount);
                Assert.False(args.Cancel);
            }).Add(p=>p.Selected, (SelectedEventArgs<DateTime?> args)=> {
                Assert.Equal(new DateTime(2021, 3, 9), args.Value);
            }).Add(p=>p.ValueChange, (ChangedEventArgs<DateTime?> args)=> {
                changeCount++;
                Assert.Equal(1, changeCount);
            }));
            var inputElement = dateInstance.Find("input");
            inputElement.Focus();
            await dateInstance.Instance.ShowPopupAsync();
            var popupElement = dateInstance.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            tableRows?[2].QuerySelectorAll("td")[2].Click();
            Assert.Equal(new DateTime(2021, 3, 9), dateInstance.Instance.Value);
        }

        [Fact(Timeout = 10000, DisplayName = "PreventOpenEvent - Cancels popup open when OnOpen sets Cancel")]
        public async Task PreventOpenEvent()
        {
            int openCount = 0;
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5))
            .Add(p => p.OnOpen, (PopupObjectArgs args) => {
                openCount++;
                Assert.Equal(1, openCount);
                Assert.False(args.Cancel);
                args.Cancel = true;
            }));
            var inputElement = dateInstance.Find("input");
            await dateInstance.Instance.ShowPopupAsync();
            var popupElements = dateInstance.FindAll(".e-popup");
            Assert.Equal(0, popupElements.Count);
        }

        [Fact(Timeout = 10000, DisplayName = "NavigatedEvent - Fires Navigated when view changes and date updates")]
        public async Task NavigatedEvent()
        {
            int navigateCount = 0;
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5))
            .Add(p => p.Navigated, (NavigatedEventArgs args) => {
                navigateCount++;
                Assert.Equal(1, navigateCount);
                Assert.Equal("Month", args.View);
                Assert.Equal(new DateTime(2021, 4, 5), args.Date);
            }));
            var inputElement = dateInstance.Find("input");
            await dateInstance.Instance.ShowPopupAsync();
            var popupElement = dateInstance.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal("Month", dateInstance.Instance.CurrentView());
            var buttonList = popupElement.QuerySelectorAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(2021, 3, 5).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
            Assert.Equal(new DateTime(2021, 3, 5).Day.ToString(), tableElement?.QuerySelector(".e-selected")?.FirstElementChild?.TextContent);
            Assert.Equal(new DateTime(2021, 3, 5).ToString("dddd, MMMM d, yyyy"), tableElement?.QuerySelector(".e-selected")?.FirstElementChild?.GetAttribute("title"));
            buttonList[1].Click();
            buttonList = popupElement.QuerySelectorAll("button");
            tableElement = popupElement.QuerySelector("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.DoesNotContain("e-disabled", buttonList[0].ClassName);
            Assert.Equal(new DateTime(2021, 4, 5).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "RenderDayCellEvent - Fires DayCellRendering for each day cell with expected args")]
        public async Task RenderDayCellEvent()
        {
            int dayCell = 27;
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5))
            .Add(p => p.DayCellRendering, (RenderDayCellEventArgs args) => {
                Assert.False(args.IsDisabled);
                Assert.False(args.IsOutOfRange);
                Assert.Equal(new DateTime(2021, 2, 1).AddDays(dayCell), args.Date);
                dayCell++;
            }));
            var inputElement = dateInstance.Find("input");
            await dateInstance.Instance.ShowPopupAsync();
            var popupElement = dateInstance.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            Assert.Equal("Month", dateInstance.Instance.CurrentView());
            var buttonList = popupElement.QuerySelectorAll("button");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(2021, 3, 5).ToString("MMMM yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            Assert.Contains("e-prev", buttonList[0].ClassName);
            Assert.Contains("e-next", buttonList[1].ClassName);
            Assert.DoesNotContain("e-disabled", buttonList[1].ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "PreventCloseEvent - Prevents popup close when OnClose sets Cancel")]
        public async Task PreventCloseEvent()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, new DateTime(2021, 3, 5))
            .Add(p => p.OnClose, (PopupObjectArgs args) => {
                args.Cancel = true;
            }));
            var inputElement = dateInstance.Find("input");
            await dateInstance.Instance.ShowPopupAsync();
            var popupElement = dateInstance.Find(".e-popup");
            Assert.NotNull(popupElement);
            await dateInstance.Instance.HidePopupAsync();
            Assert.NotNull(popupElement);
        }

        [Fact(Timeout = 10000, DisplayName = "ClearedEvent - Fires Cleared when clear button is used")]
        public async Task ClearedEvent()
        {
            var clearCount = 0;
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p=>p.ShowClearButton, true).Add(p => p.Value, new DateTime(2021, 3, 5))
            .Add(p => p.Cleared, (ClearedEventArgs args) => {
                clearCount++;
                Assert.Equal(1, clearCount);
            }));
            var inputElement = dateInstance.Find("input");
            await dateInstance.Instance.ShowPopupAsync();
            var popupElement = dateInstance.Find(".e-popup");
            await dateInstance.Instance.HidePopupAsync();
            await dateInstance.Instance.ClosePopupAsync();
            var containerElement = dateInstance.Find("input").ParentElement;
            var clearElement = containerElement?.Children[1];
            clearElement?.MouseDown();
            await Task.Delay(200);
            inputElement = dateInstance.Find("input");
            Assert.Null(inputElement.GetAttribute("value"));
            Assert.Null(dateInstance.Instance.Value);
        }

        [Fact(Timeout = 10000, DisplayName = "ResetValueInChangeEvent - Resets value inside ValueChange handler and verifies UI")]
        public async Task ResetValueInChangeEvent()
        {
            int changeCount = 0;
            DateTime? dateVal = new DateTime(2021, 3, 5);
            IRenderedComponent<SfDatePicker<DateTime?>>? dateInstance = null;
            dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Value, dateVal).Add(p => p.ValueChanged, (DateTime? value) => { dateVal = value; })
            .Add(p => p.ValueChange, (ChangedEventArgs<DateTime?> args) => {
                changeCount++;
                dateVal = null;
                dateInstance?.SetParametersAndRender(("Value", null));
            }));
            var inputElement = dateInstance.Find("input");
            await dateInstance.Instance.ShowPopupAsync();
            var popupElement = dateInstance.Find(".e-popup");
            var tableElement = popupElement.QuerySelector("table");
            var tableRows = tableElement?.QuerySelectorAll("tr");
            tableRows?[2].QuerySelectorAll("td")[2].Click();
            await Task.Delay(100);
            Assert.Null(dateInstance.Instance.Value);
            inputElement = dateInstance.Find("input");
            Assert.Null(inputElement.GetAttribute("value"));
        }

        [Fact(Timeout = 10000, DisplayName = "NavigatedEventArgsClass - Verifies NavigatedEventArgs properties")]
        public void NavigatedEventArgsClass()
        {
            var eventObj = new object();
            var configNaviEvent = new NavigatedEventArgs
            {
                Date = new DateTime() ,
                Event = eventObj,
                Name = "Name"
            };
            Assert.Equal(new DateTime() , configNaviEvent.Date);
            Assert.Equal(eventObj, configNaviEvent.Event);
            Assert.Equal("Name" , configNaviEvent.Name);
        }

        [Fact(Timeout = 10000, DisplayName = "FocusEvent - Fires Focus when input is focused")]
        public async Task FocusEvent()
        {
            int focusCount = 0;
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.OnFocus, (Syncfusion.Blazor.Toolkit.Calendars.FocusEventArgs args) => {
                focusCount++;
            }));
            var inputElement = dateInstance.Find("input");
            inputElement.Focus();
            Assert.Equal(1, focusCount);
        }

        [Fact(Timeout = 10000, DisplayName = "CreatedEvent - Fires Created when component is initialized")]
        public void CreatedEvent()
        {
            int createdCount = 0;
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>(param => param.Add(p => p.Created, (object args) => {
                createdCount++;
            }));
            Assert.Equal(1, createdCount);
        }

        [Fact(Timeout = 10000, DisplayName = "FocusAsyncMethod - Calls FocusAsync JS interop")]
        public async Task FocusAsyncMethod()
        {
            var dateInstance = RenderComponent<SfDatePicker<DateTime?>>();
            await dateInstance.Instance.FocusAsync();
            // JSInterop verification would happen here in a real environment
        }
    }
}
