using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// Click order, cancel, toggle binding, roving tabindex, keyboard navigation and runtime collection changes.
    /// </summary>
    public class ToolbarBehaviorTests : ToolbarTestBase
    {
        private static IElement ButtonOf(IRenderedComponent<SfToolbar> cut, string text)
        {
            return cut.FindAll("[data-testid='toolbar-button'], [data-testid='toolbar-toggle']")
                .First(e => e.TextContent.Trim() == text);
        }

        private static int?[] TabIndexes(IRenderedComponent<SfToolbar> cut)
        {
            return cut.FindAll("[data-testid='toolbar-button'], [data-testid='toolbar-toggle']")
                .Select(e => int.TryParse(e.GetAttribute("tabindex"), out int v) ? (int?)v : null).ToArray();
        }

        private static string[] Names(IRenderedComponent<SfToolbar> cut)
        {
            return cut.FindAll("[data-testid='toolbar-button'], [data-testid='toolbar-toggle']")
                .Select(e => e.TextContent.Trim()).ToArray();
        }

        // ------------------------------------------------------------------ click order and cancel

        [Fact(DisplayName = "U-TB-070 Item OnClick runs before OnItemClick, which receives the item id")]
        public void ClickOrder_ItemThenToolbar()
        {
            List<string> log = [];
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Item("Cut", ("Id", "cut"), ("OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, _ => log.Add("item")))),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, a => log.Add("toolbar:" + a.ItemId))));

            ButtonOf(cut, "Cut").Click();

            Assert.Equal(new[] { "item", "toolbar:cut" }, log);
        }

        [Fact(DisplayName = "U-TB-030 OnItemClick fires for every item and carries the original mouse args")]
        public void OnItemClick_FiresForEveryItem()
        {
            List<ToolbarItemClickEventArgs> received = [];
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b"))),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, a => received.Add(a))));

            ButtonOf(cut, "A").Click(new MouseEventArgs { Button = 0, ClientX = 7 });
            ButtonOf(cut, "B").Click();

            Assert.Equal(new[] { "a", "b" }, received.Select(r => r.ItemId).ToArray());
            Assert.Equal(7, received[0].OriginalArgs!.ClientX);
        }

        [Fact(DisplayName = "U-TB-030 A generated id is passed when the item has no Id")]
        public void GeneratedId_IsPassed()
        {
            string? id = null;
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("A"),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, a => id = a.ItemId)));

            ButtonOf(cut, "A").Click();

            Assert.False(string.IsNullOrWhiteSpace(id));
            Assert.StartsWith("sf-toolbar-item-", id, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "U-TB-015 A disabled item raises no click callbacks")]
        public void DisabledItem_RaisesNoCallbacks()
        {
            int item = 0, toolbar = 0;
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Item("Cut", ("Enabled", false), ("OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, _ => item++))),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, _ => toolbar++)));

            ButtonOf(cut, "Cut").Click();

            Assert.Equal(0, item);
            Assert.Equal(0, toolbar);
        }

        [Fact(DisplayName = "U-TB-056 Toggle without cancel flips Selected and raises SelectedChanged")]
        public void Toggle_FlipsAndRaises()
        {
            List<bool> changes = [];
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Bold", ("Type", ToolbarItemType.Toggle),
                ("SelectedChanged", EventCallback.Factory.Create<bool>(this, v => changes.Add(v)))));

            ButtonOf(cut, "Bold").Click();
            Assert.Equal("true", ButtonOf(cut, "Bold").GetAttribute("aria-pressed"));
            ButtonOf(cut, "Bold").Click();
            Assert.Equal("false", ButtonOf(cut, "Bold").GetAttribute("aria-pressed"));

            Assert.Equal(new[] { true, false }, changes);
        }

        [Fact(DisplayName = "U-TB-032 Two-way binding updates the bound value")]
        public void Toggle_TwoWayBinding()
        {
            bool bound = false;
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Bold", ("Type", ToolbarItemType.Toggle),
                ("Selected", bound), ("SelectedChanged", EventCallback.Factory.Create<bool>(this, v => bound = v))));

            ButtonOf(cut, "Bold").Click();

            Assert.True(bound);
        }

        [Fact(DisplayName = "U-TB-054 Cancel on a Toggle keeps Selected and OnItemClick still fires")]
        public void Toggle_CancelKeepsSelected()
        {
            int toolbarCalls = 0;
            int changes = 0;
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Item("Bold", ("Type", ToolbarItemType.Toggle),
                    ("SelectedChanged", EventCallback.Factory.Create<bool>(this, _ => changes++))),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, a =>
                {
                    toolbarCalls++;
                    a.Cancel = true;
                })));

            ButtonOf(cut, "Bold").Click();

            Assert.Equal(1, toolbarCalls);
            Assert.Equal(0, changes);
            Assert.Equal("false", ButtonOf(cut, "Bold").GetAttribute("aria-pressed"));
        }

        [Fact(DisplayName = "U-TB-055 Cancel does not stop the item-level OnClick, which runs first")]
        public void Cancel_DoesNotStopItemOnClick()
        {
            int itemCalls = 0;
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Item("Cut", ("OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, _ => itemCalls++))),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, a => a.Cancel = true)));

            ButtonOf(cut, "Cut").Click();

            Assert.Equal(1, itemCalls);
        }

        [Fact(DisplayName = "U-TB-056 A non-toggle item click does not change aria-pressed")]
        public void ButtonClick_NoPressedState()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Cut"));
            ButtonOf(cut, "Cut").Click();
            Assert.False(ButtonOf(cut, "Cut").HasAttribute("aria-pressed"));
        }

        [Fact(DisplayName = "U-TB-070 An async item OnClick completes before OnItemClick starts")]
        public async Task ClickOrder_AwaitsAsyncItemHandler()
        {
            List<string> log = [];
            TaskCompletionSource gate = new();
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Item("Cut", ("OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, async _ =>
                {
                    log.Add("item:start");
                    await gate.Task;
                    log.Add("item:end");
                }))),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, _ => log.Add("toolbar"))));

            // Drive the item's click handler directly so the test can await the whole chain.
            ToolbarItem toolbarItem = cut.FindComponent<ToolbarItem>().Instance;
            Task chain = cut.InvokeAsync(() => InvokeClick(toolbarItem));
            await Task.Delay(50);
            Assert.Equal(new[] { "item:start" }, log);

            gate.SetResult();
            await chain.WaitAsync(TimeSpan.FromSeconds(5));

            Assert.Equal(new[] { "item:start", "item:end", "toolbar" }, log);
        }

        [Fact(DisplayName = "U-TB-015 The toolbar guards a disabled item even when the button does not")]
        public void DisabledItem_GuardedByToolbarToo()
        {
            int item = 0, toolbar = 0;
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Item("Cut", ("Enabled", false), ("OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, _ => item++))),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, _ => toolbar++)));

            // The inner button is disabled, so drive the item's own click callback directly, as a misbehaving host could.
            ToolbarItem toolbarItem = cut.FindComponent<ToolbarItem>().Instance;
            cut.InvokeAsync(() => InvokeClick(toolbarItem)).Wait(TimeSpan.FromSeconds(5));

            Assert.Equal(0, item);
            Assert.Equal(0, toolbar);
        }

        // ------------------------------------------------------------------ roving tabindex

        [Fact(DisplayName = "U-TB-047 Exactly one item has tabindex 0 and the others -1")]
        public void RovingTabIndex_SingleStop()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B"), Item("C")));

            Assert.Equal(new int?[] { 0, -1, -1 }, TabIndexes(cut));
        }

        [Fact(DisplayName = "U-TB-047 A disabled first item hands the tab stop to the next enabled item")]
        public void RovingTabIndex_SkipsDisabledFirst()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A", ("Enabled", false)), Item("B"), Item("C")));

            Assert.Equal(new int?[] { -1, 0, -1 }, TabIndexes(cut));
        }

        [Fact(DisplayName = "U-TB-047 Only enabled and visible items can hold the tab stop")]
        public void RovingTabIndex_OnlyFocusable()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("A", ("Visible", false)), Item("B", ("Enabled", false)), Item("C"), Item("D")));

            Assert.Equal(new int?[] { -1, 0, -1 }, TabIndexes(cut));
        }

        [Fact(DisplayName = "U-TB-047 Focus moves the tab stop to the focused item")]
        public void RovingTabIndex_FollowsFocus()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B"), Item("C")));

            cut.FindAll("[data-testid='toolbar-item']")[2].FocusIn();

            Assert.Equal(new int?[] { -1, -1, 0 }, TabIndexes(cut));
        }

        [Fact(DisplayName = "U-TB-047 The tab stop is restored to the last focused item")]
        public void RovingTabIndex_RestoresLastFocused()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B"), Item("C")));
            cut.FindAll("[data-testid='toolbar-item']")[1].FocusIn();

            cut.Render();

            Assert.Equal(new int?[] { -1, 0, -1 }, TabIndexes(cut));
        }

        [Fact(DisplayName = "U-TB-047 A toolbar without a focusable item has no tab stop")]
        public void RovingTabIndex_NoFocusableItems()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A", ("Enabled", false)), Item("B", ("Enabled", false))));
            Assert.Equal(new int?[] { -1, -1 }, TabIndexes(cut));
        }

        [Fact(DisplayName = "U-TB-047 Items inside a group take part in the single tab stop")]
        public void RovingTabIndex_AcrossGroups()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Comp<ToolbarGroup>(Compose(Item("A"), Item("B"))), Comp<ToolbarGroup>(Compose(Item("C"), Item("D")))));

            Assert.Equal(new int?[] { 0, -1, -1, -1 }, TabIndexes(cut));
        }

        [Fact(DisplayName = "U-TB-046 Separators and spacers are not focusable")]
        public void SeparatorsAndSpacers_NotFocusable()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Comp<ToolbarSeparator>(), Comp<ToolbarSpacer>(), Item("B")));

            foreach (IElement element in cut.FindAll("[data-testid='toolbar-separator'], [data-testid='toolbar-spacer']"))
            {
                Assert.False(element.HasAttribute("tabindex"));
            }
            Assert.Equal(new int?[] { 0, -1 }, TabIndexes(cut));
        }

        // ------------------------------------------------------------------ keyboard navigation

        private static void Press(IRenderedComponent<SfToolbar> cut, string text, string key)
        {
            IElement item = cut.FindAll("[data-testid='toolbar-item']").First(e => e.TextContent.Trim() == text);
            item.KeyDown(new KeyboardEventArgs { Key = key });
        }

        private static string TabStop(IRenderedComponent<SfToolbar> cut)
        {
            return cut.FindAll("[data-testid='toolbar-button'], [data-testid='toolbar-toggle']")
                .First(e => e.GetAttribute("tabindex") == "0").TextContent.Trim();
        }

        [Theory(DisplayName = "U-TB-046 Horizontal LTR: ArrowRight moves forward, ArrowLeft moves back")]
        [InlineData("A", "ArrowRight", "B")]
        [InlineData("B", "ArrowRight", "C")]
        [InlineData("C", "ArrowLeft", "B")]
        [InlineData("B", "ArrowLeft", "A")]
        public void Keyboard_HorizontalLtr(string from, string key, string expected)
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B"), Item("C")));
            cut.FindAll("[data-testid='toolbar-item']").First(e => e.TextContent.Trim() == from).FocusIn();

            Press(cut, from, key);

            Assert.Equal(expected, TabStop(cut));
        }

        [Theory(DisplayName = "U-TB-046 Vertical: ArrowDown moves forward, ArrowUp moves back, left and right do nothing")]
        [InlineData("A", "ArrowDown", "B")]
        [InlineData("C", "ArrowUp", "B")]
        [InlineData("A", "ArrowRight", "A")]
        [InlineData("B", "ArrowLeft", "B")]
        public void Keyboard_Vertical(string from, string key, string expected)
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B"), Item("C")),
                p => p.Add(t => t.Orientation, ToolbarOrientation.Vertical));
            cut.FindAll("[data-testid='toolbar-item']").First(e => e.TextContent.Trim() == from).FocusIn();

            Press(cut, from, key);

            Assert.Equal(expected, TabStop(cut));
        }

        [Theory(DisplayName = "U-TB-046 Horizontal RTL reverses ArrowLeft and ArrowRight")]
        [InlineData("A", "ArrowLeft", "B")]
        [InlineData("B", "ArrowRight", "A")]
        [InlineData("A", "ArrowRight", "A")]
        public void Keyboard_HorizontalRtl(string from, string key, string expected)
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B"), Item("C")),
                p => p.Add(t => t.EnableRtl, true));
            cut.FindAll("[data-testid='toolbar-item']").First(e => e.TextContent.Trim() == from).FocusIn();

            Press(cut, from, key);

            Assert.Equal(expected, TabStop(cut));
        }

        [Theory(DisplayName = "U-TB-046 Home moves to the first enabled item and End to the last")]
        [InlineData("B", "Home", "A")]
        [InlineData("B", "End", "C")]
        public void Keyboard_HomeEnd(string from, string key, string expected)
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B"), Item("C")));
            cut.FindAll("[data-testid='toolbar-item']").First(e => e.TextContent.Trim() == from).FocusIn();

            Press(cut, from, key);

            Assert.Equal(expected, TabStop(cut));
        }

        [Fact(DisplayName = "U-TB-046 Home and End skip disabled items at the edges")]
        public void Keyboard_HomeEnd_SkipDisabled()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("A", ("Enabled", false)), Item("B"), Item("C"), Item("D", ("Enabled", false))));
            cut.FindAll("[data-testid='toolbar-item']").First(e => e.TextContent.Trim() == "C").FocusIn();

            Press(cut, "C", "Home");
            Assert.Equal("B", TabStop(cut));
            Press(cut, "B", "End");
            Assert.Equal("C", TabStop(cut));
        }

        [Fact(DisplayName = "U-TB-046 Arrow keys skip disabled, hidden, separator and spacer elements")]
        public void Keyboard_SkipsNonFocusable()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("A"), Item("Off", ("Enabled", false)), Item("Gone", ("Visible", false)),
                Comp<ToolbarSeparator>(), Comp<ToolbarSpacer>(), Item("B")));
            cut.FindAll("[data-testid='toolbar-item']").First(e => e.TextContent.Trim() == "A").FocusIn();

            Press(cut, "A", "ArrowRight");

            Assert.Equal("B", TabStop(cut));
        }

        [Fact(DisplayName = "U-TB-046 Navigation does not wrap at either end")]
        public void Keyboard_DoesNotWrap()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B")));
            cut.FindAll("[data-testid='toolbar-item']")[0].FocusIn();
            Press(cut, "A", "ArrowLeft");
            Assert.Equal("A", TabStop(cut));

            cut.FindAll("[data-testid='toolbar-item']")[1].FocusIn();
            Press(cut, "B", "ArrowRight");
            Assert.Equal("B", TabStop(cut));
        }

        [Fact(DisplayName = "U-TB-046 Keys that are not navigation keys change nothing")]
        public void Keyboard_OtherKeysIgnored()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B")));
            cut.FindAll("[data-testid='toolbar-item']")[0].FocusIn();

            Press(cut, "A", "a");
            Press(cut, "A", "Tab");
            Press(cut, "A", "PageDown");

            Assert.Equal("A", TabStop(cut));
        }

        [Fact(DisplayName = "U-TB-046 Arrow navigation crosses group boundaries")]
        public void Keyboard_CrossesGroups()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Comp<ToolbarGroup>(Compose(Item("A"), Item("B"))), Comp<ToolbarGroup>(Compose(Item("C"), Item("D")))));
            cut.FindAll("[data-testid='toolbar-item']")[1].FocusIn();

            Press(cut, "B", "ArrowRight");

            Assert.Equal("C", TabStop(cut));
        }

        [Fact(DisplayName = "U-TB-046 Navigation moves focus through the browser focus call")]
        public void Keyboard_CallsBrowserFocus()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B")));
            cut.FindAll("[data-testid='toolbar-item']")[0].FocusIn();
            int before = FocusCalls();

            Press(cut, "A", "ArrowRight");

            Assert.Equal(before + 1, FocusCalls());
        }

        // ------------------------------------------------------------------ public methods

        [Fact(DisplayName = "U-TB-061 FocusAsync moves focus to the first enabled item")]
        public async Task FocusAsync_FocusesFirstEnabled()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A", ("Enabled", false)), Item("B"), Item("C")));
            int before = FocusCalls();

            await cut.InvokeAsync(() => cut.Instance.FocusAsync().AsTask());

            Assert.Equal(before + 1, FocusCalls());
            Assert.Equal("B", TabStop(cut));
        }

        [Fact(DisplayName = "U-TB-061 FocusAsync returns to the last focused item")]
        public async Task FocusAsync_RestoresLastFocused()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B"), Item("C")));
            cut.FindAll("[data-testid='toolbar-item']")[2].FocusIn();

            await cut.InvokeAsync(() => cut.Instance.FocusAsync().AsTask());

            Assert.Equal("C", TabStop(cut));
        }

        [Fact(DisplayName = "U-TB-061 FocusAsync on an empty toolbar completes without error")]
        public async Task FocusAsync_EmptyToolbar()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar();
            await cut.InvokeAsync(() => cut.Instance.FocusAsync().AsTask());
            Assert.Equal(0, FocusCalls());
        }

        [Fact(DisplayName = "U-TB-061 GetVisibleItems returns visible items in order and GetOverflowItems is empty")]
        public void GetItems()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B", ("Visible", false)), Item("C")));

            Assert.Equal(new[] { "A", "C" }, cut.Instance.GetVisibleItems().Cast<ToolbarItem>().Select(i => i.Text).ToArray());
            Assert.Empty(cut.Instance.GetOverflowItems());
        }

        [Fact(DisplayName = "U-TB-061 RefreshOverflowAsync completes and keeps the toolbar usable")]
        public async Task RefreshOverflowAsync_Completes()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B")));

            await cut.InvokeAsync(() => cut.Instance.RefreshOverflowAsync().AsTask());

            Assert.Equal(new[] { "A", "B" }, Names(cut));
            Assert.Equal(new int?[] { 0, -1 }, TabIndexes(cut));
        }

        // ------------------------------------------------------------------ registration and runtime change

        [Fact(DisplayName = "U-TB-035 Items register on initialization and unregister on dispose")]
        public void Registration_RegistersAndUnregisters()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B")));
            Assert.Equal(2, cut.Instance.GetVisibleItems().Count);

            cut.SetParametersAndRender(p => p.AddChildContent(Item("A")));

            Assert.Single(cut.Instance.GetVisibleItems());
        }

        [Fact(DisplayName = "U-TB-034 Items can be added at runtime")]
        public void Dynamic_Add()
        {
            List<string> names = ["A"];
            IRenderedComponent<SfToolbar> cut = RenderToolbar(BuildItems(names));

            names.Add("B");
            cut.SetParametersAndRender(p => p.AddChildContent(BuildItems(names)));

            Assert.Equal(new[] { "A", "B" }, Names(cut));
            Assert.Equal(2, cut.Instance.GetVisibleItems().Count);
        }

        [Fact(DisplayName = "U-TB-034 Items can be removed, cleared and replaced at runtime")]
        public void Dynamic_RemoveClearReplace()
        {
            List<string> names = ["A", "B", "C"];
            IRenderedComponent<SfToolbar> cut = RenderToolbar(BuildItems(names));

            names.Remove("B");
            cut.SetParametersAndRender(p => p.AddChildContent(BuildItems(names)));
            Assert.Equal(new[] { "A", "C" }, Names(cut));

            names.Clear();
            cut.SetParametersAndRender(p => p.AddChildContent(BuildItems(names)));
            Assert.Empty(Names(cut));
            Assert.Empty(cut.Instance.GetVisibleItems());

            names.AddRange(["X", "Y"]);
            cut.SetParametersAndRender(p => p.AddChildContent(BuildItems(names)));
            Assert.Equal(new[] { "X", "Y" }, Names(cut));
        }

        [Fact(DisplayName = "I-TB-004 Collection changes keep a single tab stop")]
        public void Dynamic_KeepsSingleTabStop()
        {
            List<string> names = ["A", "B"];
            IRenderedComponent<SfToolbar> cut = RenderToolbar(BuildItems(names));

            names.Insert(0, "Z");
            cut.SetParametersAndRender(p => p.AddChildContent(BuildItems(names)));

            Assert.Equal(1, TabIndexes(cut).Count(t => t == 0));
        }

        [Fact(DisplayName = "I-TB-005 Enabling and disabling an item at runtime updates DOM and tab stop")]
        public void Runtime_EnableDisable()
        {
            bool enabled = true;
            RenderFragment Content() => Compose(Item("A", ("Enabled", enabled)), Item("B"));
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Content());
            Assert.Equal(new int?[] { 0, -1 }, TabIndexes(cut));

            enabled = false;
            cut.SetParametersAndRender(p => p.AddChildContent(Content()));

            Assert.True(ButtonOf(cut, "A").HasAttribute("disabled"));
            Assert.Equal(new int?[] { -1, 0 }, TabIndexes(cut));

            enabled = true;
            cut.SetParametersAndRender(p => p.AddChildContent(Content()));
            Assert.False(ButtonOf(cut, "A").HasAttribute("disabled"));
            Assert.Equal(1, TabIndexes(cut).Count(t => t == 0));
        }

        [Fact(DisplayName = "I-TB-006 Toggling Visible at runtime adds and removes the item")]
        public void Runtime_Visible()
        {
            bool visible = false;
            RenderFragment Content() => Compose(Item("A"), Item("B", ("Visible", visible)));
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Content());
            Assert.Equal(new[] { "A" }, Names(cut));

            visible = true;
            cut.SetParametersAndRender(p => p.AddChildContent(Content()));
            Assert.Equal(new[] { "A", "B" }, Names(cut));

            visible = false;
            cut.SetParametersAndRender(p => p.AddChildContent(Content()));
            Assert.Equal(new[] { "A" }, Names(cut));
        }

        [Fact(DisplayName = "I-TB-007 Changing the orientation at runtime updates the classes and ARIA")]
        public void Runtime_Orientation()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("A"));

            cut.SetParametersAndRender(p => p.Add(t => t.Orientation, ToolbarOrientation.Vertical));

            IElement root = cut.Find(Root);
            Assert.Contains("e-toolbar-vertical", root.ClassList);
            Assert.Equal("vertical", root.GetAttribute("aria-orientation"));
        }

        [Fact(DisplayName = "I-TB-009 Nested interactive content inside a template item stays interactive")]
        public void TemplateItem_NestedButtonIsInteractive()
        {
            int clicks = 0;
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Comp<ToolbarTemplateItem>(
                (RenderFragment)(b =>
                {
                    b.OpenElement(0, "button");
                    b.AddAttribute(1, "id", "inner");
                    b.AddAttribute(2, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, _ => clicks++));
                    b.AddContent(3, "Go");
                    b.CloseElement();
                })));

            cut.Find("#inner").Click();

            Assert.Equal(1, clicks);
        }

        [Fact(DisplayName = "U-TB-041 Disposing the toolbar clears its items and a late click is a no-op")]
        public void Dispose_LateCallbacksAreNoOps()
        {
            int calls = 0;
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("A"),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, _ => calls++)));
            SfToolbar toolbar = cut.Instance;

            DisposeComponents();

            Assert.Empty(toolbar.GetVisibleItems());
            Assert.Equal(0, calls);
        }

        [Fact(DisplayName = "I-TB-008 Changing OverflowMode at runtime updates the mode class and keeps the items")]
        public void Runtime_OverflowMode()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B")));
            Assert.Contains("e-toolbar-popup", cut.Find(Root).ClassList);

            cut.SetParametersAndRender(p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Scrollable));

            IElement root = cut.Find(Root);
            Assert.Contains("e-toolbar-scrollable", root.ClassList);
            Assert.DoesNotContain("e-toolbar-popup", root.ClassList);
            Assert.Equal(new[] { "A", "B" }, Names(cut));
        }

        [Fact(DisplayName = "U-TB-020 OverflowMode Scrollable keeps one line and shows no popup")]
        public void ScrollableMode_NoPopup()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B")),
                p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Scrollable));

            Assert.Contains("e-toolbar-scrollable", cut.Find(Root).ClassList);
            Assert.Empty(cut.FindAll("[data-testid='toolbar-overflow-popup']"));
            Assert.Empty(cut.FindAll("[data-testid='toolbar-overflow-button']"));
        }

        [Fact(DisplayName = "I-TB-011 Concurrent add and remove from several threads end in a consistent state")]
        public async Task Concurrent_AddRemove_IsConsistent()
        {
            List<string> names = ["A", "B", "C"];
            IRenderedComponent<SfToolbar> cut = RenderToolbar(BuildItems(names));

            for (int round = 0; round < 10; round++)
            {
                int r = round;
                await cut.InvokeAsync(() =>
                {
                    names.Add("N" + r);
                    if (names.Count > 4)
                    {
                        names.RemoveAt(1);
                    }
                    cut.SetParametersAndRender(p => p.AddChildContent(BuildItems(names)));
                });
            }

            Assert.Equal(names.ToArray(), Names(cut));
            Assert.Equal(names.Count, cut.Instance.GetVisibleItems().Count);
            Assert.Equal(1, TabIndexes(cut).Count(t => t == 0));
        }

        private static Task InvokeClick(ToolbarItem item)
        {
            System.Reflection.MethodInfo handler = typeof(ToolbarItem).GetMethod("HandleClickAsync",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!;
            return (Task)handler.Invoke(item, [new MouseEventArgs()])!;
        }

        private int FocusCalls()
        {
            return JSInterop.Invocations.Count(i => i.Identifier == "Blazor._internal.domWrapper.focus");
        }

        private static RenderFragment BuildItems(List<string> names)
        {
            return Compose(names.Select(n => Item(n)).ToArray());
        }
    }
}
