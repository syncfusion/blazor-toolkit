using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Calendars;

namespace Syncfusion.Blazor.Toolkit.Tests.Calendars.TimePicker
{
    public class TimePickerEvents : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public void CreatedEvent()
        {
            bool isCreated = false;
            var CreatedCount = 0;
            var TimePicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
                .Add(p => p.Created, () =>
                {
                    isCreated = true;
                    CreatedCount++;
                }));
            TimePicker.WaitForAssertion(() => Assert.True(isCreated), System.TimeSpan.FromMilliseconds(100));
            Assert.Equal(1, CreatedCount);
        }

        [Fact(Timeout = 10000)]
        public async Task SelectedEvent()
        {
            bool IsSelectedTriggered = false;
            var SelectedCount = 0;
            var SelectedEventDate = default(DateTime);

            var TimePicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
                .Add(p => p.Selected, (SelectedEventArgs<DateTime> args) =>
                {
                    SelectedEventDate = args.Value;
                    SelectedCount++;
                    IsSelectedTriggered = true;
                }));

            await TimePicker.Instance.ShowPopupAsync();
            TimePicker.WaitForElement(".e-popup-wrapper"); // wait for popup

            var popupEle = TimePicker.Find(".e-popup-wrapper");
            var ulEle = popupEle.QuerySelector("ul");
            var liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(48, liCollections.Length);

            liCollections[1].Click();
            TimePicker.WaitForAssertion(() => Assert.True(IsSelectedTriggered));
            TimePicker.WaitForAssertion(() =>  Assert.Equal(1, SelectedCount));
        }

        [Fact(Timeout = 10000)]
        public async Task SelectedEventinDynamic()
        {
            bool IsSelectedTriggered = false;
            var SelectedCount = 0;
            var SelectedEventDate = default(DateTime);

            var Timepicker = RenderComponent<SfTimePicker<DateTime>>();
            Assert.False(IsSelectedTriggered);
            Assert.Equal(0, SelectedCount);

            Timepicker.SetParametersAndRender<SfTimePicker<DateTime>>(parameters => parameters
                .Add(p => p.Selected, (SelectedEventArgs<DateTime> args) =>
                {
                    SelectedEventDate = args.Value;
                    SelectedCount++;
                    IsSelectedTriggered = true;
                }));

            await Timepicker.Instance.ShowPopupAsync();
            Timepicker.WaitForElement(".e-popup-wrapper"); // wait for popup

            var popupEle = Timepicker.Find(".e-popup-wrapper");
            var ulEle = popupEle.QuerySelector("ul");
            var liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(48, liCollections.Length);

            liCollections[1].Click();
            await Task.Delay(200);
            Assert.True(IsSelectedTriggered);
            Assert.Equal(1, SelectedCount);
        }

        [Fact(Timeout = 10000)]
        public async Task ChangeEvent()
        {
            bool IsChangeTriggered = false;
            int ChangeCount = 0;

            var Timepicker = RenderComponent<SfTimePicker<DateTime>>(parameters => parameters
                .Add(e => e.ValueChange, (ChangeEventArgs<DateTime> args) => // use component's ChangeEventArgs
                {
                    IsChangeTriggered = true;
                    ChangeCount++;
                })
                .Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 10, 30, 00)));

            Assert.Equal(new DateTime(2020, 1, 1, 10, 30, 00), Timepicker.Instance.Value);

            await Timepicker.Instance.ShowPopupAsync();
            Timepicker.WaitForElement(".e-popup-wrapper"); // wait for popup

            var popupEle = Timepicker.Find(".e-popup-wrapper");
            var ulEle = popupEle.QuerySelector("ul");
            var liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(48, liCollections.Length);

            liCollections[1].Click();
            await Task.Delay(500);
            Assert.True(IsChangeTriggered);
            Assert.Equal(1, ChangeCount);

            await Timepicker.Instance.HidePopupAsync();

            // Dynamically change value property should not trigger ValueChange
            IsChangeTriggered = false;
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime>>(("Value", new DateTime(2020, 1, 1, 11, 30, 00)));
            Assert.False(IsChangeTriggered);
            Assert.Equal(1, ChangeCount);
        }

        [Fact(Timeout = 10000)]
        public async Task ChangeEventInDynamic()
        {
            bool IsChangeTriggered = false;
            int ChangeCount = 0;

            var Timepicker = RenderComponent<SfTimePicker<DateTime>>();
            Assert.False(IsChangeTriggered);
            Assert.Equal(0, ChangeCount);

            Timepicker.SetParametersAndRender<SfTimePicker<DateTime>>(parameters => parameters
                .Add(e => e.ValueChange, (ChangeEventArgs<DateTime> args) => // use component's ChangeEventArgs
                {
                    IsChangeTriggered = true;
                    ChangeCount++;
                })
                .Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 10, 30, 00)));

            Assert.Equal(new DateTime(2020, 1, 1, 10, 30, 00), Timepicker.Instance.Value);

            await Timepicker.Instance.ShowPopupAsync();
            Timepicker.WaitForElement(".e-popup-wrapper"); // wait for popup

            var popupEle = Timepicker.Find(".e-popup-wrapper");
            var ulEle = popupEle.QuerySelector("ul");
            var liCollections = ulEle.QuerySelectorAll(".e-list-item");
            Assert.True(liCollections.Length > 0);
            Assert.Equal(48, liCollections.Length);

            liCollections[1].Click();
            Assert.True(IsChangeTriggered);
            Assert.Equal(1, ChangeCount);
            await Timepicker.Instance.HidePopupAsync();

            // Dynamically change value property should not trigger ValueChange
            IsChangeTriggered = false;
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime>>(("Value", new DateTime(2020, 1, 1, 11, 30, 00)));
            await Task.Delay(300);
            Assert.False(IsChangeTriggered);
            Assert.Equal(1, ChangeCount);
        }

        [Fact(Timeout = 10000)]
        public async Task ClearedEvent()
        {
            bool IsClearedTriggered = false;
            int ClearCount = 0;

            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(e => e.Cleared, (ClearedEventArgs args) => // use component's ClearedEventArgs
                {
                    IsClearedTriggered = true;
                    ClearCount++;
                })
                .Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 10, 30, 00))
                .Add(calendar => calendar.ShowClearButton, true));

            Assert.Equal(new DateTime(2020, 1, 1, 10, 30, 00), Timepicker.Instance.Value);

            var inputElement = Timepicker.Find("input");
            inputElement.Focus();

            var parentContainer = inputElement.ParentElement;
            var ClearIcon = parentContainer.QuerySelectorAll(".e-close");
            Assert.True(ClearIcon.Length > 0);

            ClearIcon[0].MouseDown();
            await Task.Delay(100);

            inputElement = Timepicker.Find("input");
            Assert.Null(inputElement.GetAttribute("value"));
            Assert.True(IsClearedTriggered);
            Assert.Equal(1, ClearCount);
        }

        [Fact(Timeout = 10000)]
        public async Task ClearedEventInDynamic()
        {
            bool IsClearedTriggered = false;
            int ClearCount = 0;

            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>();
            Assert.False(IsClearedTriggered);
            Assert.Equal(0, ClearCount);

            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(e => e.Cleared, (ClearedEventArgs args) => // use component's ClearedEventArgs
                {
                    IsClearedTriggered = true;
                    ClearCount++;
                })
                .Add(calendar => calendar.Value, new DateTime(2020, 1, 1, 10, 30, 00))
                .Add(calendar => calendar.ShowClearButton, true));

            Assert.Equal(new DateTime(2020, 1, 1, 10, 30, 00), Timepicker.Instance.Value);

            var inputElement = Timepicker.Find("input");
            inputElement.Focus();

            var parentContainer = inputElement.ParentElement;
            var ClearIcon = parentContainer.QuerySelectorAll(".e-close");
            Assert.True(ClearIcon.Length > 0);

            ClearIcon[0].MouseDown();
            await Task.Delay(100);

            inputElement = Timepicker.Find("input");
            Assert.Null(inputElement.GetAttribute("value"));
            Assert.True(IsClearedTriggered);
            Assert.Equal(1, ClearCount);
        }

        [Fact(Timeout = 10000)]
        public void FocusEvent()
        {
            bool IsFocusTriggered = false;
            int FocusCount = 0;

            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(e => e.OnFocus, (FocusEventArgs args) => // use component's FocusEventArgs
                {
                    IsFocusTriggered = true;
                    FocusCount++;
                }));

            var inputElement = Timepicker.Find("input");
            inputElement.Focus();

            Assert.Equal(1, FocusCount);
            Assert.True(IsFocusTriggered);
        }

        [Fact(Timeout = 10000)]
        public void FocusEventInDynamic()
        {
            bool IsFocusTriggered = false;
            int FocusCount = 0;

            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>();
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(e => e.OnFocus, (FocusEventArgs args) => // use component's FocusEventArgs
                {
                    IsFocusTriggered = true;
                    FocusCount++;
                }));

            var inputElement = Timepicker.Find("input");
            inputElement.Focus();

            Assert.Equal(1, FocusCount);
            Assert.True(IsFocusTriggered);
        }

        [Fact(Timeout = 10000)]
        public async Task CloseEvent()
        {
            bool IsCloseTriggered = false;
            int CloseCount = 0;

            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(e => e.OnClose, (PopupEventArgs args) => // use component's PopupEventArgs
                {
                    IsCloseTriggered = true;
                    CloseCount++;
                }));

            await Timepicker.Instance.ShowPopupAsync();
            await Timepicker.Instance.HidePopupAsync();

            Assert.True(IsCloseTriggered);
            Assert.Equal(1, CloseCount);
        }

        [Fact(Timeout = 10000)]
        public async Task CloseEventinDyanmic()
        {
            bool IsCloseTriggered = false;
            int CloseCount = 0;

            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>();
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(e => e.OnClose, (PopupEventArgs args) => // use component's PopupEventArgs
                {
                    IsCloseTriggered = true;
                    CloseCount++;
                }));

            await Timepicker.Instance.ShowPopupAsync();
            await Timepicker.Instance.HidePopupAsync();

            Assert.True(IsCloseTriggered);
            Assert.Equal(1, CloseCount);
        }

        [Fact(Timeout = 10000)]
        public async Task OpenEvent()
        {
            bool IsOpenTriggered = false;
            int OpenCount = 0;

            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(e => e.OnOpen, (PopupEventArgs args) => // use component's PopupEventArgs
                {
                    IsOpenTriggered = true;
                    OpenCount++;
                }));

            await Timepicker.Instance.ShowPopupAsync();

            Assert.True(IsOpenTriggered);
            Assert.Equal(1, OpenCount);
        }

        [Fact(Timeout = 10000)]
        public async Task OpenEventInDyanmic()
        {
            bool IsOpenTriggered = false;
            int OpenCount = 0;

            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>();
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(e => e.OnOpen, (PopupEventArgs args) => // use component's PopupEventArgs
                {
                    IsOpenTriggered = true;
                    OpenCount++;
                }));

            await Timepicker.Instance.ShowPopupAsync();

            Assert.True(IsOpenTriggered);
            Assert.Equal(1, OpenCount);
        }

        [Fact(Timeout = 10000)]
        public async Task ItemRenderEvent()
        {
            bool IsItemRendered = false;
            int ItemCount = 0;

            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(e => e.OnItemRender, (ItemEventArgs<DateTime?> args) =>
                {
                    IsItemRendered = true;
                    ItemCount++;
                }));

            await Timepicker.Instance.ShowPopupAsync();
            Assert.True(IsItemRendered);
            Assert.Equal(48, ItemCount);
        }

        [Fact(Timeout = 10000)]
        public async Task ItemRenderEventInDynamic()
        {
            bool IsItemRendered = false;
            int ItemCount = 0;

            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>();
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(e => e.OnItemRender, (ItemEventArgs<DateTime?> args) =>
                {
                    IsItemRendered = true;
                    ItemCount++;
                }));

            Assert.False(IsItemRendered);
            Assert.Equal(0, ItemCount);

            await Timepicker.Instance.ShowPopupAsync();
            Assert.True(IsItemRendered);
            Assert.Equal(48, ItemCount);
        }

        [Fact(Timeout = 10000)]
        public async Task DestroyedEvent()
        {
            bool IsItemRendered = true;
            int ItemCount = 0;

            var Timepicker = RenderComponent<SfTimePicker<DateTime?>>();
            Timepicker.SetParametersAndRender<SfTimePicker<DateTime?>>(parameters => parameters
                .Add(e => e.Destroyed, (object args) =>
                {
                    IsItemRendered = false;
                    ItemCount++;
                }));

            await Timepicker.Instance.ShowPopupAsync();
            await Timepicker.Instance.HidePopupAsync();

            Assert.True(IsItemRendered);
            Assert.Equal(0, ItemCount);
        }

        [Fact(Timeout = 10000)]
        public void ItemEventArgsClass()
        {
            var itemEventArgs = new ItemEventArgs<DateTime>
            {
                IsDisabled = true,
                Name = "TestEvent",
                Text = "TestText",
                Value = DateTime.Now
            };
            Assert.True(itemEventArgs.IsDisabled);
            Assert.Equal("TestEvent", itemEventArgs.Name);
            Assert.Equal("TestText", itemEventArgs.Text);
            Assert.NotNull(itemEventArgs.Value);
        }

        [Fact(Timeout = 10000)]
        public void ChangeEventArgsClass()
        {
            var changeEventArgs = new ChangeEventArgs<DateTime> // use component's ChangeEventArgs
            {
                Event = new { Type = "click", Details = "Some details" },
                IsInteracted = true,
                Text = "12:30",
                Value = new DateTime(2024, 8, 11, 12, 30, 0)
            };

            Assert.Equal(new { Type = "click", Details = "Some details" }, changeEventArgs.Event);
            Assert.True(changeEventArgs.IsInteracted);
            Assert.Equal("12:30", changeEventArgs.Text);
            Assert.Equal(new DateTime(2024, 8, 11, 12, 30, 0), changeEventArgs.Value);
        }
    }
}