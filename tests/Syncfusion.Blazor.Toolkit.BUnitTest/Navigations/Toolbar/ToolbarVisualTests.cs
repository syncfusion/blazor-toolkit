using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// The visual and layout fixes of the Toolbar: the icon font is not overridden, the overflow popup
    /// and the nested menus are placed so they are not clipped, the Extended mode shows its overflow
    /// control, and the Scrollable mode navigates with chevrons instead of a native scrollbar.
    /// </summary>
    public class ToolbarVisualTests : ToolbarTestBase
    {
        private const string ModulePath = "./_content/Syncfusion.Blazor.Toolkit/scripts/toolbar.js";
        private const string OverflowButton = "[data-testid='toolbar-overflow-button']";
        private const string ExpandButton = "[data-testid='toolbar-expand-button']";
        private const string OverflowPopup = "[data-testid='toolbar-overflow-popup']";
        private const string ScrollStart = "[data-testid='toolbar-scroll-start']";
        private const string ScrollEnd = "[data-testid='toolbar-scroll-end']";

        private BunitJSModuleInterop? _module;

        private BunitJSModuleInterop ScriptModule()
        {
            _module ??= JSInterop.SetupModule(ModulePath);
            _module.Mode = JSRuntimeMode.Loose;
            return _module;
        }

        private void ScriptPlacement(bool below)
        {
            ToolbarPopupPlacement placement = new()
            {
                Placement = below ? "bottom" : "top",
                Below = below,
                Positioned = true,
                Top = below ? 48 : 0,
                Bottom = below ? 0 : 120,
                Right = 16,
                Available = 200
            };
            ScriptModule().Setup<ToolbarPopupPlacement>("placeOverflowPopup", _ => true).SetResult(placement);
            ScriptModule().Setup<ToolbarPopupPlacement>("placePopup", _ => true).SetResult(placement);
        }

        private void ScriptScroll(bool canScroll, bool atStart, bool atEnd)
        {
            ScriptModule().Setup<ToolbarScrollState>("scrollState", _ => true)
                .SetResult(new ToolbarScrollState { CanScroll = canScroll, AtStart = atStart, AtEnd = atEnd });
        }

        /// <summary>Scripts a measurement in which only the first of the two items fits, so one item overflows.</summary>
        private void ScriptOverflowingMeasurement()
        {
            ScriptModule().Setup<ToolbarMeasurement>("measure", _ => true).SetResult(new ToolbarMeasurement
            {
                Available = 100,
                Gap = 4,
                ButtonSize = 32,
                Items = [new ToolbarMeasuredItem { Id = "a", Size = 50 }, new ToolbarMeasuredItem { Id = "b", Size = 50 }]
            });
        }

        private static RenderFragment Five()
        {
            return Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b")), Item("C", ("Id", "c")), Item("D", ("Id", "d")), Item("E", ("Id", "e")));
        }

        private void WaitForScrollState()
        {
            for (int i = 0; i < 200 && !JSInterop.Invocations.Any(x => x.Identifier == "scrollState"); i++)
            {
                Thread.Sleep(10);
            }
        }

        // ------------------------------------------------------------------ icons

        [Fact(DisplayName = "U-TB-100 The toolbar keeps the icon class of the item, so the icon font is not overridden")]
        public void ItemIcon_KeepsConsumerIconClass()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Cut", ("IconCss", "sb-icons e-cut")));

            IElement icon = cut.Find("[data-testid='toolbar-button'] .e-btn-icon");
            Assert.Contains("sb-icons", icon.ClassList);
            Assert.Contains("e-cut", icon.ClassList);
        }

        [Fact(DisplayName = "U-TB-100 An icon-only item keeps its icon class as well")]
        public void IconOnlyItem_KeepsConsumerIconClass()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Item("Bold", ("IconCss", "sb-icons e-bold"), ("DisplayMode", ToolbarItemDisplayMode.IconOnly)));

            IElement icon = cut.Find("[data-testid='toolbar-button'] .e-btn-icon");
            Assert.Contains("sb-icons", icon.ClassList);
            Assert.Contains("e-bold", icon.ClassList);
        }

        [Fact(DisplayName = "U-TB-100 The overflow button uses the toolkit icon class")]
        public void OverflowButton_UsesToolkitIcon()
        {
            ScriptModule();
            ScriptPlacement(true);
            ScriptOverflowingMeasurement();

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b"))));
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));

            IElement icon = cut.Find(OverflowButton + " .e-btn-icon");
            Assert.Contains("e-icons", icon.ClassList);
            Assert.Contains("e-more-horizontal-1", icon.ClassList);
        }

        // ------------------------------------------------------------------ popup placement

        [Fact(DisplayName = "U-TB-021 The overflow popup is a fixed layer outside the toolbar root")]
        public void Popup_EscapesClippingAncestors()
        {
            ScriptModule();
            ScriptPlacement(true);
            ScriptOverflowingMeasurement();

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b"))));
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            cut.Find(OverflowButton).Click();

            cut.WaitForAssertion(() => Assert.Contains("e-toolbar-overflow-layer-positioned", cut.Find("[data-testid='toolbar-overflow-layer']").ClassList));
            IElement popup = cut.Find(OverflowPopup);
            IElement layer = cut.Find("[data-testid='toolbar-overflow-layer']");
            Assert.Equal(cut.Find(Root), layer.ParentElement);
            Assert.Equal("menu", popup.GetAttribute("role"));
            Assert.Null(popup.GetAttribute("style"));
        }

        [Fact(DisplayName = "U-TB-021 Closing the popup removes the fixed layer")]
        public async Task Popup_LayerRemovedOnClose()
        {
            ScriptModule();
            ScriptPlacement(true);
            ScriptOverflowingMeasurement();

            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b"))));

            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));

            await cut.Find(OverflowButton).ClickAsync(new MouseEventArgs());
            cut.WaitForAssertion(() =>
                Assert.Single(cut.FindAll("[data-testid='toolbar-overflow-layer']")));

            await cut.Find("[data-testid='toolbar-overflow-backdrop']")
                .ClickAsync(new MouseEventArgs());

            cut.WaitForAssertion(() =>
            {
                Assert.Empty(cut.FindAll("[data-testid='toolbar-overflow-layer']"));
                Assert.Empty(cut.FindAll(OverflowPopup));
            });
        }

        [Fact(DisplayName = "U-TB-021 Disposing the toolbar removes the fixed layer")]
        public void Popup_LayerRemovedOnDispose()
        {
            ScriptModule();
            ScriptPlacement(true);
            ScriptOverflowingMeasurement();

            RenderFragment toolbar = builder =>
            {
                builder.OpenComponent<SfToolbar>(0);
                builder.AddAttribute(1, "ChildContent", Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b"))));
                builder.CloseComponent();
            };
            IRenderedComponent<ConditionalToolbarHost> host = RenderComponent<ConditionalToolbarHost>(p => p
                .Add(h => h.Visible, true)
                .AddChildContent(toolbar));
            host.WaitForAssertion(() => Assert.Single(host.FindAll(OverflowButton)));
            host.Find(OverflowButton).Click();
            host.WaitForAssertion(() => Assert.Single(host.FindAll("[data-testid='toolbar-overflow-layer']")));

            host.SetParametersAndRender(p => p
                .Add(h => h.Visible, false)
                .AddChildContent(toolbar));

            Assert.Empty(host.FindAll("[data-testid='toolbar-overflow-layer']"));
            Assert.Empty(host.FindComponents<SfToolbar>());
        }

        [Fact(DisplayName = "U-TB-101 The popup opens above the trigger when there is no room below it")]
        public void Popup_FlipsAboveWhenNeeded()
        {
            ScriptModule();
            ScriptPlacement(false);
            ScriptOverflowingMeasurement();

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b"))));
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            cut.Find(OverflowButton).Click();

            cut.WaitForAssertion(() => Assert.Contains("e-toolbar-overflow-popup-top", cut.Find(OverflowPopup).ClassList));
        }

        [Fact(DisplayName = "U-TB-101 The popup opens below the trigger when there is room")]
        public void Popup_StaysBelowWhenThereIsRoom()
        {
            ScriptModule();
            ScriptPlacement(true);
            ScriptOverflowingMeasurement();

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b"))));
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            cut.Find(OverflowButton).Click();

            cut.WaitForAssertion(() => Assert.DoesNotContain("e-toolbar-overflow-popup-top", cut.Find(OverflowPopup).ClassList));
        }

        [Fact(DisplayName = "U-TB-102 The nested menu opens above the trigger when there is no room below it")]
        public void NestedMenu_FlipsAboveWhenNeeded()
        {
            ScriptModule();
            ScriptPlacement(false);

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Comp<ToolbarItem>(Compose(Item("Option A"), Item("Option B")), ("Text", "More"), ("Type", ToolbarItemType.DropDown))));

            cut.Find("[data-testid='toolbar-dropdown']").Click();

            cut.WaitForAssertion(() => Assert.Contains("e-toolbar-nested-menu-top", cut.Find("[data-testid='toolbar-nested-popup']").ClassList));
        }

        [Fact(DisplayName = "U-TB-102 The nested menu opens below the trigger when there is room")]
        public void NestedMenu_StaysBelowWhenThereIsRoom()
        {
            ScriptModule();
            ScriptPlacement(true);

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Comp<ToolbarItem>(Compose(Item("Option A"), Item("Option B")), ("Text", "More"), ("Type", ToolbarItemType.DropDown))));

            cut.Find("[data-testid='toolbar-dropdown']").Click();

            cut.WaitForAssertion(() => Assert.DoesNotContain("e-toolbar-nested-menu-top", cut.Find("[data-testid='toolbar-nested-popup']").ClassList));
        }

        // ------------------------------------------------------------------ Extended mode

        [Fact(DisplayName = "U-TB-103 Extended mode renders the overflow control as the expand toggle")]
        public void Extended_RendersExpandControl()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Compose(Item("A"), Item("B")),
                p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Extended));

            IElement expand = cut.Find(ExpandButton);
            Assert.Equal("false", expand.GetAttribute("aria-expanded"));
            Assert.NotEmpty(expand.QuerySelectorAll(".e-btn-icon"));
            Assert.False(string.IsNullOrWhiteSpace(expand.GetAttribute("aria-label")));
        }

        [Fact(DisplayName = "U-TB-103 The Extended overflow control expands the toolbar and shows the extra rows")]
        public void Extended_ControlExpandsTheToolbar()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Compose(Item("A"), Item("B")),
                p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Extended));

            Assert.DoesNotContain("e-toolbar-expanded", cut.Find("[data-testid='toolbar-root']").ClassName);

            cut.Find(ExpandButton).Click();

            IElement root = cut.Find("[data-testid='toolbar-root']");
            Assert.Contains("e-toolbar-expanded", root.ClassName);
            Assert.Equal("true", cut.Find(ExpandButton).GetAttribute("aria-expanded"));
        }

        [Fact(DisplayName = "U-TB-103 The Extended overflow control carries no menu semantics")]
        public void Extended_ControlHasNoMenuSemantics()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Compose(Item("A"), Item("B")),
                p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Extended));

            IElement expand = cut.Find(ExpandButton);
            Assert.False(expand.HasAttribute("aria-haspopup"));
            Assert.False(expand.HasAttribute("aria-controls"));
        }

        [Fact(DisplayName = "U-TB-103 The Extended overflow control opens no popup")]
        public void Extended_ControlOpensNoPopup()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Compose(Item("A"), Item("B")),
                p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Extended));

            cut.Find(ExpandButton).Click();

            Assert.Empty(cut.FindAll(OverflowPopup));
        }

        [Fact(DisplayName = "U-TB-103 The Extended overflow control collapses the toolbar again")]
        public void Extended_ControlCollapsesTheToolbar()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Compose(Item("A"), Item("B")),
                p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Extended));

            cut.Find(ExpandButton).Click();
            cut.Find(ExpandButton).Click();

            Assert.DoesNotContain("e-toolbar-expanded", cut.Find("[data-testid='toolbar-root']").ClassName);
            Assert.Equal("false", cut.Find(ExpandButton).GetAttribute("aria-expanded"));
        }

        [Fact(DisplayName = "U-TB-103 Leaving the Extended mode collapses the toolbar again")]
        public void Extended_LeavingTheModeCollapses()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Compose(Item("A"), Item("B")),
                p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Extended));

            cut.Find(ExpandButton).Click();
            Assert.Contains("e-toolbar-expanded", cut.Find("[data-testid='toolbar-root']").ClassName);

            cut.SetParametersAndRender(p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.MultiRow));

            Assert.DoesNotContain("e-toolbar-expanded", cut.Find("[data-testid='toolbar-root']").ClassName);
        }

        // ------------------------------------------------------------------ Scrollable navigation

        [Fact(DisplayName = "U-TB-104 The Scrollable mode shows no chevron when everything fits")]
        public void Scrollable_NoChevronWhenEverythingFits()
        {
            ScriptModule();
            ScriptScroll(false, true, true);

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Five(), p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Scrollable));
            WaitForScrollState();

            Assert.Empty(cut.FindAll(ScrollStart));
            Assert.Empty(cut.FindAll(ScrollEnd));
        }

        [Fact(DisplayName = "U-TB-104 The Scrollable mode shows both chevrons when the items overflow")]
        public void Scrollable_ShowsBothChevronsWhenOverflowing()
        {
            ScriptModule();
            ScriptScroll(true, true, false);

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Five(), p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Scrollable));

            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(ScrollStart)));
            Assert.Single(cut.FindAll(ScrollEnd));
        }

        [Fact(DisplayName = "U-TB-104 The chevron that scrolls towards the start is disabled at the start")]
        public void Scrollable_StartChevronDisabledAtStart()
        {
            ScriptModule();
            ScriptScroll(true, true, false);

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Five(), p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Scrollable));

            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(ScrollStart)));
            Assert.Equal("true", cut.Find(ScrollStart).GetAttribute("aria-disabled"));
            Assert.Equal("false", cut.Find(ScrollEnd).GetAttribute("aria-disabled"));
        }

        [Fact(DisplayName = "U-TB-104 The chevron that scrolls towards the end is disabled at the end")]
        public void Scrollable_EndChevronDisabledAtEnd()
        {
            ScriptModule();
            ScriptScroll(true, false, true);

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Five(), p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Scrollable));

            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(ScrollEnd)));
            Assert.Equal("false", cut.Find(ScrollStart).GetAttribute("aria-disabled"));
            Assert.Equal("true", cut.Find(ScrollEnd).GetAttribute("aria-disabled"));
        }

        [Fact(DisplayName = "U-TB-104 Clicking a chevron scrolls the toolbar one page")]
        public void Scrollable_ClickScrollsOnePage()
        {
            ScriptModule();
            ScriptScroll(true, true, false);

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Five(), p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Scrollable));
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(ScrollEnd)));

            cut.Find(ScrollEnd).Click();

            Assert.Contains(JSInterop.Invocations, i => i.Identifier == "scrollByPage");
        }

        [Fact(DisplayName = "U-TB-104 The chevrons carry an accessible name")]
        public void Scrollable_ChevronsHaveAccessibleNames()
        {
            ScriptModule();
            ScriptScroll(true, true, false);

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Five(), p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Scrollable));

            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(ScrollStart)));
            Assert.False(string.IsNullOrWhiteSpace(cut.Find(ScrollStart).GetAttribute("aria-label")));
            Assert.False(string.IsNullOrWhiteSpace(cut.Find(ScrollEnd).GetAttribute("aria-label")));
            Assert.NotEmpty(cut.Find(ScrollStart).QuerySelectorAll(".e-btn-icon"));
            Assert.NotEmpty(cut.Find(ScrollEnd).QuerySelectorAll(".e-btn-icon"));
        }

        [Fact(DisplayName = "U-TB-104 The Scrollable mode shows no overflow button")]
        public void Scrollable_NoOverflowButton()
        {
            ScriptModule();
            ScriptScroll(true, true, false);

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Five(), p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Scrollable));

            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(ScrollStart)));
            Assert.Empty(cut.FindAll(OverflowButton));
            Assert.Empty(cut.FindAll(OverflowPopup));
        }

        [Fact(DisplayName = "U-TB-020 The Scrollable mode never measures the items for overflow")]
        public void Scrollable_NeverMeasures()
        {
            ScriptModule();
            ScriptScroll(true, true, false);
            ScriptModule().Setup<ToolbarMeasurement>("measure", _ => true).SetResult(new ToolbarMeasurement { Available = 10, Gap = 4 });

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Five(), p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Scrollable));
            Thread.Sleep(80);

            Assert.DoesNotContain(JSInterop.Invocations, i => i.Identifier == "measure");
        }

        // ------------------------------------------------------------------ structure

        [Fact(DisplayName = "U-TB-010 The items render inside the content element of the toolbar")]
        public void Items_RenderInsideTheContentElement()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Five());

            IElement content = cut.Find("[data-testid='toolbar-content']");
            Assert.Equal(5, content.QuerySelectorAll("[data-testid='toolbar-item']").Length);
        }

        private sealed class ConditionalToolbarHost : ComponentBase
        {
            [Parameter]
            public bool Visible { get; set; }

            [Parameter]
            public RenderFragment? ChildContent { get; set; }

            protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
            {
                if (Visible)
                {
                    builder.AddContent(0, ChildContent);
                }
            }
        }
    }
}
