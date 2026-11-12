using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// Runtime overflow of the Popup mode: a measurement goes in, the overflow button and popup come out.
    /// The browser measurement is replaced by a scripted module so every result is deterministic.
    /// </summary>
    public class ToolbarOverflowTests : ToolbarTestBase
    {
        private const string OverflowButton = "[data-testid='toolbar-overflow-button']";
        private const string OverflowPopup = "[data-testid='toolbar-overflow-popup']";
        private const string OverflowItem = "[data-testid='toolbar-overflow-item']";
        private const string ModulePath = "./_content/Syncfusion.Blazor.Toolkit/scripts/toolbar.js";

        private BunitJSModuleInterop? _module;

        /// <summary>Scripts what the browser would report: every item is 50 wide, the button 32, the gap 4.</summary>
        private void ScriptMeasurement(double available, params string[] ids)
        {
            _module ??= JSInterop.SetupModule(ModulePath);
            _module.Mode = JSRuntimeMode.Loose;
            ToolbarMeasurement measurement = new()
            {
                Available = available,
                Gap = 4,
                ButtonSize = 32,
                Items = ids.Select(id => new ToolbarMeasuredItem { Id = id, Size = 50 }).ToList()
            };
            _module.Setup<ToolbarMeasurement>("measure", _ => true).SetResult(measurement);
        }

        private static RenderFragment Five()
        {
            return Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b")), Item("C", ("Id", "c")), Item("D", ("Id", "d")), Item("E", ("Id", "e")));
        }

        private IRenderedComponent<SfToolbar> RenderMeasured(double available, RenderFragment? content = null, Action<ComponentParameterCollectionBuilder<SfToolbar>>? configure = null)
        {
            ScriptMeasurement(available, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(content ?? Five(), configure);
            WaitUntilMeasured();
            _ = SettledMeasureCalls();
            return cut;
        }

        private int MeasureCalls() => JSInterop.Invocations.Count(i => i.Identifier == "measure");

        /// <summary>Waits until no measurement has started for a quiet period, so a count read afterwards is a stable baseline.</summary>
        private int SettledMeasureCalls()
        {
            int last = -1;
            int stable = 0;
            for (int i = 0; i < 200 && stable < 4; i++)
            {
                int now = MeasureCalls();
                stable = now == last ? stable + 1 : 0;
                last = now;
                Thread.Sleep(15);
            }
            return last;
        }

        /// <summary>
        /// Polls until the measurement count equals the expected value. A JavaScript call count never triggers a render, so
        /// bUnit's render-driven waiters have nothing to wake them; polling is the reliable way to wait for it.
        /// </summary>
        private void WaitForMeasureCalls(int expected)
        {
            for (int i = 0; i < 200 && MeasureCalls() < expected; i++)
            {
                Thread.Sleep(10);
            }
            // Give a spurious extra measurement time to show up, then assert the exact count.
            Thread.Sleep(60);
            Assert.Equal(expected, MeasureCalls());
        }

        /// <summary>Polls until the first measurement has happened (a JavaScript call never triggers a render to wake a waiter).</summary>
        private void WaitUntilMeasured()
        {
            for (int i = 0; i < 500 && MeasureCalls() < 1; i++)
            {
                Thread.Sleep(10);
            }
            Assert.True(MeasureCalls() >= 1, "The toolbar never measured.");
        }

        private int ObserveCalls() => JSInterop.Invocations.Count(i => i.Identifier == "observe");

        private static string[] BarTexts(IRenderedComponent<SfToolbar> cut)
        {
            return cut.FindAll("[data-testid='toolbar-item']:not(.e-toolbar-item-overflowed) .e-btn")
                .Select(e => e.TextContent.Trim()).ToArray();
        }

        private static string[] PopupTexts(IRenderedComponent<SfToolbar> cut)
        {
            return cut.FindAll(OverflowItem).Select(e => e.TextContent.Trim()).ToArray();
        }

        // ------------------------------------------------------------------ button visibility

        [Fact(DisplayName = "U-TB-024 When every item fits there is no overflow button")]
        public void AllFit_NoButton()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(1000);

            Assert.Empty(cut.FindAll(OverflowButton));
            Assert.Equal(new[] { "A", "B", "C", "D", "E" }, BarTexts(cut));
            Assert.Empty(cut.Instance.GetOverflowItems());
        }

        [Fact(DisplayName = "U-TB-025 When some items do not fit they move to the popup list and the button appears")]
        public void PartialOverflow_MovesTheTail()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(200);

            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            Assert.Equal(new[] { "A", "B", "C" }, BarTexts(cut));
            Assert.Equal(3, cut.Instance.GetVisibleItems().Count);
            Assert.Equal(2, cut.Instance.GetOverflowItems().Count);
        }

        [Fact(DisplayName = "U-TB-026 When no item fits the button is present and the popup lists every item")]
        public void AllOverflow_ButtonAndFullPopup()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(10);

            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            Assert.Empty(BarTexts(cut));
            cut.Find(OverflowButton).Click();

            Assert.Equal(new[] { "A", "B", "C", "D", "E" }, PopupTexts(cut));
            Assert.Equal(5, cut.Instance.GetOverflowItems().Count);
        }

        [Fact(DisplayName = "U-TB-025 An overflowed item leaves the keyboard flow")]
        public void OverflowedItem_NotFocusable()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(200);
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));

            string[] zero = cut.FindAll("[data-testid='toolbar-button'], " + OverflowButton)
                .Where(e => e.GetAttribute("tabindex") == "0").Select(e => e.TextContent.Trim()).ToArray();

            Assert.Single(zero);
        }

        [Fact(DisplayName = "U-TB-023 Priority decides which item leaves first: the highest Priority value overflows first")]
        public void Priority_DecidesWhoLeaves()
        {
            ScriptMeasurement(200, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("A", ("Id", "a"), ("Priority", 9)), Item("B", ("Id", "b")), Item("C", ("Id", "c")), Item("D", ("Id", "d")), Item("E", ("Id", "e"))));

            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            Assert.Equal(new[] { "B", "C", "D" }, BarTexts(cut));
            cut.Find(OverflowButton).Click();
            Assert.Equal(new[] { "A", "E" }, PopupTexts(cut).Order().ToArray());
        }

        [Fact(DisplayName = "U-TB-023 Overflow=Show items stay when others leave, Overflow=Hide items leave first")]
        public void OverflowOption_ShowAndHide()
        {
            ScriptMeasurement(200, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("A", ("Id", "a"), ("Overflow", ToolbarOverflowOption.Hide)), Item("B", ("Id", "b")), Item("C", ("Id", "c")),
                Item("D", ("Id", "d")), Item("E", ("Id", "e"), ("Overflow", ToolbarOverflowOption.Show))));

            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            Assert.Contains("E", BarTexts(cut));
            Assert.DoesNotContain("A", BarTexts(cut));
        }

        // ------------------------------------------------------------------ popup composition and ARIA

        [Fact(DisplayName = "U-TB-057 The popup is a fixed role=menu whose entries are SfButton menuitems")]
        public void Popup_IsFixedMenuOfButtons()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(200);
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            cut.Find(OverflowButton).Click();

            IElement popup = cut.Find(OverflowPopup);
            Assert.Equal("menu", popup.GetAttribute("role"));
            Assert.Equal(cut.Find(Root), popup.Closest("[data-testid='toolbar-overflow-layer']")!.ParentElement);
            IReadOnlyList<IElement> entries = cut.FindAll(OverflowItem);
            Assert.Equal(2, entries.Count);
            Assert.Equal(2, cut.FindComponents<SfButton>().Count(b => b.Find("button").GetAttribute("data-testid") == "toolbar-overflow-item"));
            foreach (IElement entry in entries)
            {
                Assert.Equal("menuitem", entry.GetAttribute("role"));
                Assert.Equal("button", entry.TagName.ToLowerInvariant());
                Assert.Contains(entry.GetAttribute("tabindex"), new[] { "0", "-1" });
            }
            // Roving inside the popup: exactly one entry is the tab stop.
            Assert.Single(entries, e => e.GetAttribute("tabindex") == "0");
        }

        [Fact(DisplayName = "U-TB-059 The overflow button exposes haspopup, expanded and controls that match the popup id")]
        public void OverflowButton_Aria()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(200);
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));

            IElement button = cut.Find(OverflowButton);
            Assert.Equal("menu", button.GetAttribute("aria-haspopup"));
            Assert.Equal("false", button.GetAttribute("aria-expanded"));

            button.Click();
            button = cut.Find(OverflowButton);
            Assert.Equal("true", button.GetAttribute("aria-expanded"));
            Assert.Equal(cut.Find(OverflowPopup).Id, button.GetAttribute("aria-controls"));
        }

        [Fact(DisplayName = "U-TB-059 The popup is not rendered until the button opens it")]
        public void Popup_ClosedByDefault()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(200);
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));

            Assert.Empty(cut.FindAll(OverflowPopup));
        }

        // ------------------------------------------------------------------ open, close, select

        [Fact(DisplayName = "U-TB-025 Clicking the button twice opens then closes the popup")]
        public async Task Button_Toggles()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(200);
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));

            await cut.Find(OverflowButton).ClickAsync(new MouseEventArgs());
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowPopup)));

            await cut.Find(OverflowButton).ClickAsync(new MouseEventArgs());
            cut.WaitForAssertion(() => Assert.Empty(cut.FindAll(OverflowPopup)));
        }

        [Fact(DisplayName = "U-TB-025 Escape pressed on the overflow button closes the popup before focus has moved into it")]
        public async Task Escape_OnOverflowButton_ClosesPopup()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(200);
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            cut.Find(OverflowButton).Click();
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowPopup)));

            // The server moves focus into the popup a moment after it renders. Escape in that gap is dispatched from the button.
            await cut.Find(OverflowButton).KeyDownAsync(new KeyboardEventArgs { Key = "Escape" });

            cut.WaitForAssertion(() =>
            {
                Assert.Empty(cut.FindAll(OverflowPopup));
                Assert.Equal("false", cut.Find(OverflowButton).GetAttribute("aria-expanded"));
            });
        }

        [Fact(DisplayName = "U-TB-025 Escape closes the popup and returns focus to the overflow button")]
        public void Escape_ClosesAndRefocuses()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(200);
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            cut.Find(OverflowButton).Click();
            cut.WaitForAssertion(() => Assert.Equal(1, FocusCalls()));
            int beforeClose = FocusCalls();

            cut.Find(OverflowPopup).KeyDown(new KeyboardEventArgs { Key = "Escape" });

            cut.WaitForAssertion(() =>
            {
                Assert.Empty(cut.FindAll(OverflowPopup));
                Assert.Equal(beforeClose + 1, FocusCalls());
            });
        }

        [Fact(DisplayName = "U-TB-025 Opening the popup moves focus into its first entry")]
        public void Open_FocusesFirstEntry()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(200);
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            int before = FocusCalls();

            cut.Find(OverflowButton).Click();

            cut.WaitForAssertion(() => Assert.Equal(before + 1, FocusCalls()));
        }

        [Fact(DisplayName = "U-TB-025 Arrow keys move through the popup entries and wrap")]
        public async Task PopupArrows_MoveAndWrap()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(200);
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));

            await cut.Find(OverflowButton).ClickAsync(new MouseEventArgs());
            cut.WaitForAssertion(() =>
            {
                Assert.Single(cut.FindAll(OverflowPopup));
                Assert.Equal("0", cut.FindAll(OverflowItem)[0].GetAttribute("tabindex"));
            });

            IElement layer = cut.Find("[data-testid='toolbar-overflow-layer']");

            await layer.KeyDownAsync(new KeyboardEventArgs { Key = "ArrowDown" });
            cut.WaitForAssertion(() => Assert.Equal("0", cut.FindAll(OverflowItem)[1].GetAttribute("tabindex")));

            await cut.Find("[data-testid='toolbar-overflow-layer']")
                .KeyDownAsync(new KeyboardEventArgs { Key = "ArrowDown" });
            cut.WaitForAssertion(() => Assert.Equal("0", cut.FindAll(OverflowItem)[0].GetAttribute("tabindex")));

            await cut.Find("[data-testid='toolbar-overflow-layer']")
                .KeyDownAsync(new KeyboardEventArgs { Key = "ArrowUp" });
            cut.WaitForAssertion(() => Assert.Equal("0", cut.FindAll(OverflowItem)[1].GetAttribute("tabindex")));

            await cut.Find("[data-testid='toolbar-overflow-layer']")
                .KeyDownAsync(new KeyboardEventArgs { Key = "Home" });
            cut.WaitForAssertion(() => Assert.Equal("0", cut.FindAll(OverflowItem)[0].GetAttribute("tabindex")));

            await cut.Find("[data-testid='toolbar-overflow-layer']")
                .KeyDownAsync(new KeyboardEventArgs { Key = "End" });
            cut.WaitForAssertion(() => Assert.Equal("0", cut.FindAll(OverflowItem)[1].GetAttribute("tabindex")));
        }

        [Fact(DisplayName = "U-TB-025 Selecting an entry runs the item click flow and closes the popup")]
        public async Task SelectEntry_ActivatesAndCloses()
        {
            List<string> log = [];
            ScriptMeasurement(200, "a", "b", "c", "d", "e");

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("A", ("Id", "a")),
                Item("B", ("Id", "b")),
                Item("C", ("Id", "c")),
                Item("D", ("Id", "d"), ("OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, _ => log.Add("item:D")))),
                Item("E", ("Id", "e"))),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, a => log.Add("toolbar:" + a.ItemId))));

            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));

            await cut.Find(OverflowButton).ClickAsync(new MouseEventArgs());
            cut.WaitForAssertion(() =>
            {
                Assert.Single(cut.FindAll(OverflowPopup));
                Assert.NotEmpty(cut.FindAll(OverflowItem));
            });

            await cut.FindAll(OverflowItem)[0].ClickAsync(new MouseEventArgs());

            cut.WaitForAssertion(() =>
            {
                Assert.Equal(new[] { "item:D", "toolbar:d" }, log);
                Assert.Empty(cut.FindAll(OverflowPopup));
            });
        }

        [Fact(DisplayName = "U-TB-025 With CloseOnSelect false the popup stays open after a selection")]
        public void CloseOnSelect_False_KeepsOpen()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(200, configure: p => p.Add(t => t.OverflowSettings, new ToolbarOverflowSettings { CloseOnSelect = false }));
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            cut.Find(OverflowButton).Click();

            cut.FindAll(OverflowItem)[0].Click();

            Assert.Single(cut.FindAll(OverflowPopup));
        }

        [Fact(DisplayName = "U-TB-025 Clicking outside the popup closes it")]
        public async Task OutsideClick_Closes()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(200);
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));

            await cut.Find(OverflowButton).ClickAsync(new MouseEventArgs());
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowPopup)));

            await cut.Find("[data-testid='toolbar-overflow-backdrop']")
                .ClickAsync(new MouseEventArgs());

            cut.WaitForAssertion(() =>
            {
                Assert.Empty(cut.FindAll(OverflowPopup));
                Assert.Empty(cut.FindAll("[data-testid='toolbar-overflow-layer']"));
            });
        }

        [Fact(DisplayName = "U-TB-025 A disabled item shows in the popup as a disabled entry that does nothing")]
        public void DisabledEntry_Inert()
        {
            int clicks = 0;
            ScriptMeasurement(200, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("A", ("Id", "a")), Item("B", ("Id", "b")), Item("C", ("Id", "c")),
                Item("D", ("Id", "d"), ("Enabled", false), ("OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, _ => clicks++))),
                Item("E", ("Id", "e"))));
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            cut.Find(OverflowButton).Click();

            IElement entry = cut.FindAll(OverflowItem)[0];
            Assert.True(entry.HasAttribute("disabled"));
            entry.Click();

            Assert.Equal(0, clicks);
        }

        // ------------------------------------------------------------------ measurement lifecycle and triggers

        [Fact(DisplayName = "U-TB-062 The interactive first render starts observation exactly once")]
        public void FirstRender_ObservesOnce()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(1000);
            Assert.Equal(1, ObserveCalls());

            cut.SetParametersAndRender(p => p.Add(t => t.CssClass, "extra"));
            cut.SetParametersAndRender(p => p.Add(t => t.AriaLabel, "Again"));

            Assert.Equal(1, ObserveCalls());
        }

        [Fact(DisplayName = "U-TB-067 A resize notification recalculates once")]
        public void Trigger_Resize()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(1000);
            int before = SettledMeasureCalls();

            cut.InvokeAsync(() => cut.Instance.OnContainerResizedAsync());

            WaitForMeasureCalls(before + 1);
        }

        [Fact(DisplayName = "U-TB-067 A burst of triggers inside the debounce window recalculates once")]
        public void Trigger_BurstDebounced()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(1000);
            int before = SettledMeasureCalls();

            cut.InvokeAsync(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    _ = cut.Instance.OnContainerResizedAsync();
                }
            });

            WaitForMeasureCalls(before + 1);
            Thread.Sleep(80);
            Assert.Equal(before + 1, MeasureCalls());
        }

        [Fact(DisplayName = "U-TB-067 RefreshOverflowAsync recalculates once")]
        public async Task Trigger_Refresh()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(1000);
            int before = SettledMeasureCalls();

            await cut.InvokeAsync(() => cut.Instance.RefreshOverflowAsync().AsTask());

            WaitForMeasureCalls(before + 1);
        }

        [Fact(DisplayName = "U-TB-067 Changing Orientation recalculates once")]
        public void Trigger_Orientation()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(1000);
            int before = SettledMeasureCalls();

            cut.SetParametersAndRender(p => p.Add(t => t.Orientation, ToolbarOrientation.Vertical));

            WaitForMeasureCalls(before + 1);
        }

        [Fact(DisplayName = "U-TB-067 Changing OverflowMode away from and back to Popup recalculates")]
        public void Trigger_OverflowMode()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(1000);
            cut.SetParametersAndRender(p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Scrollable));
            int before = SettledMeasureCalls();

            cut.SetParametersAndRender(p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Popup));

            WaitForMeasureCalls(before + 1);
        }

        [Fact(DisplayName = "U-TB-067 Changing an item Priority recalculates once")]
        public void Trigger_ItemPriority()
        {
            ScriptMeasurement(1000, "a");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("A", ("Id", "a")));
            WaitUntilMeasured();
            int before = SettledMeasureCalls();

            cut.FindComponent<ToolbarItem>().SetParametersAndRender(p => p.Add(i => i.Priority, 5));

            WaitForMeasureCalls(before + 1);
        }

        [Fact(DisplayName = "U-TB-067 Changing an item Visible recalculates once")]
        public void Trigger_ItemVisible()
        {
            ScriptMeasurement(1000, "a");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("A", ("Id", "a")));
            WaitUntilMeasured();
            int before = SettledMeasureCalls();

            cut.FindComponent<ToolbarItem>().SetParametersAndRender(p => p.Add(i => i.Visible, false));

            WaitForMeasureCalls(before + 1);
        }

        [Fact(DisplayName = "U-TB-067 Adding an item at runtime recalculates")]
        public void Trigger_CollectionAdd()
        {
            ScriptMeasurement(1000, "a", "b");
            bool second = false;
            RenderFragment Content() => builder =>
            {
                builder.AddContent(0, Item("A", ("Id", "a")));
                if (second)
                {
                    builder.AddContent(1, Item("B", ("Id", "b")));
                }
            };
            IRenderedComponent<SfToolbar> cut = RenderComponent<SfToolbar>(p => p.AddChildContent(Content()));
            WaitUntilMeasured();
            int before = SettledMeasureCalls();

            second = true;
            cut.SetParametersAndRender(p => p.AddChildContent(Content()));

            WaitForMeasureCalls(before + 1);
        }

        // ------------------------------------------------------------------ scope of the algorithm

        [Fact(DisplayName = "U-TB-020 Scrollable mode never measures and never shows the overflow button")]
        public void Scrollable_NoMeasurement()
        {
            ScriptMeasurement(10, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Five(), p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Scrollable));
            Thread.Sleep(80);

            Assert.Equal(0, MeasureCalls());
            Assert.Empty(cut.FindAll(OverflowButton));
            Assert.Equal(new[] { "A", "B", "C", "D", "E" }, BarTexts(cut));
        }

        [Fact(DisplayName = "U-TB-024 With ShowOverflowButton false the button is never rendered")]
        public void ShowOverflowButton_False()
        {
            IRenderedComponent<SfToolbar> cut = RenderMeasured(200, configure: p => p.Add(t => t.OverflowSettings, new ToolbarOverflowSettings { ShowOverflowButton = false }));
            Thread.Sleep(60);

            Assert.Empty(cut.FindAll(OverflowButton));
        }

        private int FocusCalls()
        {
            return JSInterop.Invocations.Count(i => i.Identifier == "Blazor._internal.domWrapper.focus");
        }
    }
}
