using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.Calendar
{
    public class Events: BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "Event: Created event is invoked")]
        public void CreatedEvent()
        {
            bool isCreated = false;
            var CreatedCount = 0;
            var component = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.Add(p => p.Created, () =>
            {
                isCreated = true;
                CreatedCount++;
            }));
            component.WaitForAssertion(()=>Assert.True(isCreated),System.TimeSpan.FromMilliseconds(100));
            Assert.Equal(1, CreatedCount);
        }

        [Fact(Timeout = 10000, DisplayName = "Event: RenderDayCell is triggered for each cell")]
        public void RenderDayCellEvent()
        {
            bool isRenderDayCellTriggered = false;
            var RenderCount = 0;
            var component = RenderComponent<SfCalendar<DateTime?>>(parameters => parameters.Add(p => p.DayCellRendering, (RenderDayCellEventArgs args) =>
            {
                isRenderDayCellTriggered = true;
                RenderCount++;
            }).
            Add(e=> e.Created, () => 
            {
                Assert.True(isRenderDayCellTriggered);
                Assert.Equal(42, RenderCount);
            }));
        }

        [Fact(Timeout = 10000, DisplayName = "Event: Selected event on cell click")]
        public void SelectedEvent()
        {
            bool IsSelectedTriggered = false;
            var SelectedCount = 0;
            var SelectedDate = default(DateTime);
            var SelectedEventDate = default(DateTime);
            var component = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.Add(p => p.Selected, (SelectedEventArgs<DateTime> args) =>
            {
                SelectedEventDate = args.Value;
                SelectedCount++;
                IsSelectedTriggered = true;
            }).Add(Calendar => Calendar.Value, new DateTime(2021, 1, 1)));
            var tableElement = component.Find("table");
            Assert.True(tableElement.QuerySelectorAll("td").Length >= 42);
            SelectedDate = Convert.ToDateTime(tableElement?.QuerySelectorAll("td")?[30].FirstElementChild?.GetAttribute("title"));
            // Clicking td element
            tableElement?.QuerySelectorAll("td")[30].Click();
            component.WaitForAssertion(() =>  Assert.True(IsSelectedTriggered));
            Assert.Equal(SelectedDate, SelectedEventDate);
        }

        [Fact(Timeout = 10000, DisplayName = "Event: DeSelected event on multi-selection toggles")]
        public void DeSelectedEvent()
        {
            bool IsSelectedTriggered = false;
            bool IsDeselectedTriggered = false;
            var SelectedCount = 0;
            var SelectedDate = default(DateTime);
            var SelectedEventDate = default(DateTime);
            var DeselectedDate = default(DateTime);
            var DeselectedEventDate = default(DateTime);
            var component = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.
            Add(Cal => Cal.IsMultiSelection, true).Add(p => p.Selected, (SelectedEventArgs<DateTime> args) =>
            {
                SelectedEventDate = args.Value;
                SelectedCount++;
                IsSelectedTriggered = true;
            }).Add(e => e.DeSelected, (DeSelectedEventArgs<DateTime> args) => 
            {
                IsDeselectedTriggered = true;
                DeselectedEventDate = args.Value;
            }).Add(Calendar => Calendar.Value, new DateTime(2021, 1, 1)));
            var tableElement = component.Find("table");
            Assert.True(tableElement.QuerySelectorAll("td").Length >= 42);
            SelectedDate = Convert.ToDateTime(tableElement?.QuerySelectorAll("td")?[31].FirstElementChild?.GetAttribute("title"));
            // Selecting Date 1 and check selected event triggering
            tableElement?.QuerySelectorAll("td")[31].Click();
            component.WaitForAssertion(() => Assert.True(IsSelectedTriggered));
            Assert.Equal(SelectedDate, SelectedEventDate);
            IsSelectedTriggered = false;
            SelectedDate = Convert.ToDateTime(tableElement?.QuerySelectorAll("td")?[30].FirstElementChild?.GetAttribute("title"));
            // Selecting Date 2 and check selected event triggering
            tableElement?.QuerySelectorAll("td")[30].Click();
            component.WaitForAssertion(() => Assert.True(IsSelectedTriggered));
            Assert.Equal(SelectedDate, SelectedEventDate);
            DeselectedDate = Convert.ToDateTime(tableElement?.QuerySelectorAll("td")?[31].FirstElementChild?.GetAttribute("title"));
            // Deselecting Date 2 and check deselected event triggering
            tableElement?.QuerySelectorAll("td")[31].Click();
            component.WaitForAssertion(() => Assert.True(IsDeselectedTriggered));
            Assert.Equal(DeselectedDate, DeselectedEventDate);
            IsDeselectedTriggered = false;
            DeselectedDate = Convert.ToDateTime(tableElement?.QuerySelectorAll("td")?[30].FirstElementChild?.GetAttribute("title"));
            // Deselecting Date 1 and check deselected event triggering
            tableElement?.QuerySelectorAll("td")[30].Click();
            component.WaitForAssertion(() => Assert.True(IsDeselectedTriggered));
            Assert.Equal(DeselectedDate, DeselectedEventDate);
        }

        [Fact(Timeout = 10000, DisplayName = "Event: ValueChange event fired on selection")]
        public void ChangeEvent()
        {
            bool IsChangeTriggered = false;
            int ChangeCount = 0;
            var SelectedDate = default(DateTime);
            var ChangeEventDate = default(DateTime);
            var component = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.Add(p => p.ValueChange, (ChangedEventArgs<DateTime> args) =>
            {
                IsChangeTriggered = true;
                ChangeEventDate = args.Value;
                Assert.Equal("ValueChange", args.Name);
                ChangeCount++;
            }));
            var tableElement = component.Find("table");
            Assert.True(tableElement.QuerySelectorAll("td").Length >= 42);
            SelectedDate = Convert.ToDateTime(tableElement?.QuerySelectorAll("td")?[31].FirstElementChild?.GetAttribute("title"));
            // Clicking a cell and check change event triggering
            tableElement?.QuerySelectorAll("td")[31].Click();
            component.WaitForAssertion(() => Assert.True(IsChangeTriggered));
            Assert.Equal(SelectedDate, ChangeEventDate);
            Assert.Equal(1, ChangeCount);
        }

        [Fact(Timeout = 10000, DisplayName = "Event: ValueChange with multi-selection updates values")]
        public void ChangeEventWithMultiSelection()
        {
            bool IsChangeTriggered = false;
            int ChangeCount = 0;
            DateTime[] ChangeEventDates= new DateTime[] { };
            var SelectedDate = default(DateTime);
            var ChangeEventDate = default(DateTime);
            var component = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.
            Add(Cal => Cal.IsMultiSelection, true).
            Add(Cal => Cal.Value, new DateTime(2021, 3, 1)).
            Add(e => e.ValueChange, (ChangedEventArgs<DateTime> args) =>
            {
                IsChangeTriggered = true;
                ChangeEventDate = args.Value;
                Assert.Equal("ValueChange", args.Name);
                ChangeCount++;
                ChangeEventDates = args.Values;

            }));
            Assert.Equal(0, ChangeEventDates?.Length);
            var tableElement = component.Find("table");
            Assert.True(tableElement.QuerySelectorAll("td").Length >= 42);
            SelectedDate = Convert.ToDateTime(tableElement?.QuerySelectorAll("td")?[31].FirstElementChild?.GetAttribute("title"));
            // Selecting Date 1 and check change event triggering
            tableElement?.QuerySelectorAll("td")[31].Click();
            component.WaitForAssertion(() => Assert.True(IsChangeTriggered));
            Assert.Equal(SelectedDate, ChangeEventDate);
            Assert.Equal(1, ChangeCount);
            Assert.Equal(2, ChangeEventDates?.Length);
            IsChangeTriggered = false;
            SelectedDate = Convert.ToDateTime(tableElement?.QuerySelectorAll("td")?[21].FirstElementChild?.GetAttribute("title"));
            // Selecting Date 2 and check change event triggering
            tableElement?.QuerySelectorAll("td")[21].Click();
            component.WaitForAssertion(() => Assert.True(IsChangeTriggered));
            Assert.Equal(SelectedDate, ChangeEventDate);
            Assert.Equal(2, ChangeCount);
            Assert.Equal(3, ChangeEventDates?.Length);
            IsChangeTriggered = false;
            SelectedDate = Convert.ToDateTime(tableElement?.QuerySelectorAll("td")?[31].FirstElementChild?.GetAttribute("title"));
            // Deselecting Date 1 and check change event triggering
            tableElement?.QuerySelectorAll("td")[31].Click();
            component.WaitForAssertion(() => Assert.True(IsChangeTriggered));
            Assert.Equal(3, ChangeCount);
            Assert.Equal(2, ChangeEventDates?.Length);
            IsChangeTriggered = false;
            SelectedDate = Convert.ToDateTime(tableElement?.QuerySelectorAll("td")?[21].FirstElementChild?.GetAttribute("title"));
            // Deselecting Date 1 and check change event triggering
            tableElement?.QuerySelectorAll("td")[21].Click();
            component.WaitForAssertion(() => Assert.True(IsChangeTriggered));
            Assert.Equal(4, ChangeCount);
            Assert.Equal(1, ChangeEventDates?.Length);
        }

        [Fact(Timeout = 10000, DisplayName = "Event: Navigated event fires on navigation")]
        public void NavigatedEvent()
        {
            bool IsNavigatedTriggered = false;
            string NavigatedView = "";
            var Calendar = RenderComponent<SfCalendar<DateTime>>(parameters => parameters.
            Add(e => e.Navigated, (NavigatedEventArgs args) =>
            {
                IsNavigatedTriggered = true;
                NavigatedView = args.View;
            })
            .Add(Calendar => Calendar.Value, new DateTime(1900, 1, 1)));
            var tableElement = Calendar.Find("table");
            var parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal("Month", Calendar.Instance.CurrentView());
            // Navigated from Month to Year by clicking title header
            parentContainer?.QuerySelector(".e-title")?.Click();
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            Assert.True(IsNavigatedTriggered);
            Assert.Equal("Year", Calendar.Instance.CurrentView());
            Assert.Equal("Year", NavigatedView);
            Assert.Equal(NavigatedView, Calendar.Instance.CurrentView());
            Calendar.WaitForAssertion(() => Assert.True(IsNavigatedTriggered));
            IsNavigatedTriggered = false;
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            // Navigating current view using icons
            var ButtonList = Calendar.FindAll("button");
            Assert.Equal(new DateTime(1900, 1, 1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            ButtonList[1].Click();
            tableElement = Calendar.Find("table");
            parentContainer = tableElement?.ParentElement?.ParentElement;
            Assert.Equal(new DateTime(1900, 1, 1).AddYears(1).ToString("yyyy"), parentContainer?.QuerySelector(".e-header")?.FirstElementChild?.TextContent);
            // Navigated from Year to Decade by clicking title header
            parentContainer?.QuerySelector(".e-title")?.Click();
            Assert.Equal("Decade", Calendar.Instance.CurrentView());
            Assert.True(IsNavigatedTriggered);
            Assert.Equal("Decade", Calendar.Instance.CurrentView());
            Assert.Equal("Decade", NavigatedView);
            Assert.Equal(NavigatedView, Calendar.Instance.CurrentView());
        }

        [Fact(Timeout = 10000, DisplayName = "Model: PopupEventArgs properties")]
        public void PopupEventArgsClass()
        {
            var eventObject = new object();
            var config = new PopupEventArgs
            {
                Cancel = true ,
                Event = eventObject,
                Name = "Name"

            };
            Assert.Equal(eventObject, config.Event);
            Assert.True(config.Cancel);
            Assert.Equal("Name" , config.Name);
        }

        [Fact(Timeout = 10000, DisplayName = "Model: PopupObjectArgs properties")]
        public void PopupObjectArgsClass()
        { 
            var eventObject = new object();
            var model = new PopupObjectArgs 
            { 
                Event = eventObject ,
                PreventDefault = eventObject
            };
            Assert.Equal(eventObject , model.Event);
            Assert.Equal(eventObject, model.PreventDefault);
        }

        [Fact(Timeout = 10000, DisplayName = "Model: ClearedEventArgs properties")]
        public void ClearedEventArgs()
        {
            var eventObject = new object();
            var model = new ClearedEventArgs 
            {
                Event = eventObject
            };
            Assert.Equal(eventObject , model.Event);
        }

        [Fact(Timeout = 10000, DisplayName = "Model: FocusEventArgs properties")]
        public void FocusEventArgs()
        {
            var eventObject = new object();
            var model = new Blazor.Toolkit.Calendars.FocusEventArgs
            {
                Model = eventObject
            };
            Assert.Equal(eventObject , model.Model);
        }

        [Fact(Timeout = 10000, DisplayName = "Model: BlurEventArgs properties")]
        public void BlurEventArgs()
        {
            var eventObject = new object();
            var model = new Blazor.Toolkit.Calendars.BlurEventArgs
            {
                Model = eventObject
            };
            Assert.Equal(eventObject, model.Model);
        }
    }
}
