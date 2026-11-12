using System.Globalization;
using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// Overflow events, nested menus, groups, data binding, wrap modes and localization.
    /// Test names carry the TEST-MATRIX id.
    /// </summary>
    public class ToolbarBehaviorExtendedTests : ToolbarTestBase
    {
        private const string OverflowButton = "[data-testid='toolbar-overflow-button']";
        private const string OverflowPopup = "[data-testid='toolbar-overflow-popup']";
        private const string ModulePath = "./_content/Syncfusion.Blazor.Toolkit/scripts/toolbar.js";

        private BunitJSModuleInterop? _module;

        private void ScriptMeasurement(double available, params string[] ids)
        {
            _module ??= JSInterop.SetupModule(ModulePath);
            BunitJSModuleInterop module = _module;
            module.Mode = JSRuntimeMode.Loose;
            ToolbarMeasurement measurement = new()
            {
                Available = available,
                Gap = 4,
                ButtonSize = 32,
                Items = ids.Select(id => new ToolbarMeasuredItem { Id = id, Size = 50 }).ToList()
            };
            module.Setup<ToolbarMeasurement>("measure", _ => true).SetResult(measurement);
        }

        private void WaitUntilMeasured()
        {
            for (int i = 0; i < 500 && JSInterop.Invocations.Count(i => i.Identifier == "measure") < 1; i++)
            {
                Thread.Sleep(10);
            }
            Assert.True(JSInterop.Invocations.Any(i => i.Identifier == "measure"), "The toolbar never measured.");
        }

        // ------------------------------------------------------------------ overflow events

        [Fact(DisplayName = "U-TB-031 Opening the overflow popup fires OnOverflowOpen once")]
        public void OverflowOpen_FiresOnce()
        {
            int opens = 0;
            ScriptMeasurement(200, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b")), Item("C", ("Id", "c")), Item("D", ("Id", "d")), Item("E", ("Id", "e"))),
                p => p.Add(t => t.OnOverflowOpen, EventCallback.Factory.Create(this, () => opens++)));
            WaitUntilMeasured();
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));

            cut.Find(OverflowButton).Click();
            cut.Find(OverflowButton).Click();

            Assert.Equal(1, opens);
        }

        [Fact(DisplayName = "U-TB-031 Closing the overflow popup fires OnOverflowClose once per close")]
        public async Task OverflowClose_FiresOncePerClose()
        {
            int closes = 0;
            ScriptMeasurement(200, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b")), Item("C", ("Id", "c")), Item("D", ("Id", "d")), Item("E", ("Id", "e"))),
                p => p.Add(t => t.OnOverflowClose, EventCallback.Factory.Create(this, () => closes++)));

            WaitUntilMeasured();
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));

            await cut.Find(OverflowButton).ClickAsync(new MouseEventArgs());
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowPopup)));

            await cut.Find(OverflowButton).ClickAsync(new MouseEventArgs());
            cut.WaitForAssertion(() =>
            {
                Assert.Empty(cut.FindAll(OverflowPopup));
                Assert.Equal(1, closes);
            });
        }

        [Fact(DisplayName = "U-TB-031 Escape, outside click and selection each close the popup once")]
        public void OverflowClose_EveryPathFiresOnce()
        {
            int closes = 0;
            EventCallback close = EventCallback.Factory.Create(this, () => closes++);

            _module = null;
            ScriptMeasurement(200, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> escape = RenderToolbar(Five(), p => p.Add(t => t.OnOverflowClose, close));
            WaitUntilMeasured();
            escape.WaitForAssertion(() => Assert.Single(escape.FindAll(OverflowButton)));
            escape.Find(OverflowButton).Click();
            escape.Find("[data-testid='toolbar-overflow-layer']").KeyDown(new KeyboardEventArgs { Key = "Escape" });
            escape.WaitForAssertion(() =>
            {
                Assert.Empty(escape.FindAll("[data-testid='toolbar-overflow-layer']"));
                Assert.Equal(1, closes);
            });
            escape.Find(OverflowButton).KeyDown(new KeyboardEventArgs { Key = "Escape" });
            Assert.Equal(1, closes);
            escape.Dispose();

            _module = null;
            ScriptMeasurement(200, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> outside = RenderToolbar(Five(), p => p.Add(t => t.OnOverflowClose, close));
            WaitUntilMeasured();
            outside.WaitForAssertion(() => Assert.Single(outside.FindAll(OverflowButton)));
            outside.Find(OverflowButton).Click();
            outside.Find("[data-testid='toolbar-overflow-backdrop']").Click();
            outside.WaitForAssertion(() =>
            {
                Assert.Empty(outside.FindAll("[data-testid='toolbar-overflow-layer']"));
                Assert.Equal(2, closes);
            });
            outside.Dispose();

            _module = null;
            ScriptMeasurement(200, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> select = RenderToolbar(Five(), p => p.Add(t => t.OnOverflowClose, close));
            WaitUntilMeasured();
            select.WaitForAssertion(() => Assert.Single(select.FindAll(OverflowButton)));
            select.Find(OverflowButton).Click();
            select.WaitForAssertion(() => Assert.NotEmpty(select.FindAll("[data-testid='toolbar-overflow-item']")));
            select.Find("[data-testid='toolbar-overflow-item']").Click();
            select.WaitForAssertion(() =>
            {
                Assert.Empty(select.FindAll("[data-testid='toolbar-overflow-layer']"));
                Assert.Equal(3, closes);
            });
        }

        [Fact(DisplayName = "U-TB-042 The overflow button is hidden when ShowOverflowButton is false")]
        public void ShowOverflowButton_False_HidesButton()
        {
            ScriptMeasurement(10, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Five(), p => p.Add(t => t.OverflowSettings, new ToolbarOverflowSettings { ShowOverflowButton = false }));
            WaitUntilMeasured();
            Thread.Sleep(40);

            Assert.Empty(cut.FindAll(OverflowButton));
        }

        // ------------------------------------------------------------------ nested menus

        [Fact(DisplayName = "U-TB-012 Split, DropDown and Menu expose haspopup and the documented test ids")]
        public void PopupItems_ExposeAriaAndTestIds()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Comp<ToolbarItem>(Item("Save As"), ("Text", "Save"), ("Type", ToolbarItemType.Split)),
                Comp<ToolbarItem>(Item("Paste special"), ("Text", "Paste"), ("Type", ToolbarItemType.DropDown)),
                Comp<ToolbarItem>(Item("Zoom"), ("Text", "View"), ("Type", ToolbarItemType.Menu))));

            IElement split = cut.Find("[data-testid='toolbar-split']");
            IElement dropdown = cut.Find("[data-testid='toolbar-dropdown']");
            IElement menu = cut.Find("[data-testid='toolbar-menubutton']");
            // The split wrapper is a plain span and carries no ARIA. The chevron button opens the
            // menu, so it holds aria-haspopup and aria-expanded. A generic element rejects both.
            IElement chevron = cut.Find("[data-testid='toolbar-split-toggle']");
            Assert.Equal("menu", chevron.GetAttribute("aria-haspopup"));
            Assert.Equal("false", chevron.GetAttribute("aria-expanded"));
            Assert.False(split.HasAttribute("aria-haspopup"));
            Assert.False(split.HasAttribute("aria-expanded"));
            Assert.Equal("menu", dropdown.GetAttribute("aria-haspopup"));
            Assert.Equal("menu", menu.GetAttribute("aria-haspopup"));
            Assert.Empty(cut.FindAll("[data-testid='toolbar-nested-popup']"));
        }

        [Fact(DisplayName = "U-TB-012 A DropDown opens nested items and sets aria-expanded")]
        public void DropDown_OpensNestedItems()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Comp<ToolbarItem>(Compose(Item("Option A"), Item("Option B")), ("Text", "More"), ("Type", ToolbarItemType.DropDown)));

            cut.Find("[data-testid='toolbar-dropdown']").Click();

            IElement opener = cut.Find("[data-testid='toolbar-dropdown']");
            Assert.Equal("true", opener.GetAttribute("aria-expanded"));
            Assert.False(string.IsNullOrEmpty(cut.Find("[data-testid='toolbar-nested-popup']").Id));
            Assert.Equal(2, cut.FindAll("[data-testid='toolbar-nested-item']").Count);
        }

        [Fact(DisplayName = "U-TB-012 A Split primary click does not open; the chevron does")]
        public void Split_PrimaryDoesNotOpen_ChevronDoes()
        {
            int clicks = 0;
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Comp<ToolbarItem>(
                Item("Save As"),
                ("Text", "Save"),
                ("Type", ToolbarItemType.Split),
                ("OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, _ => clicks++))));

            cut.Find("[data-testid='toolbar-split-primary']").Click();
            Assert.Equal(1, clicks);
            Assert.Empty(cut.FindAll("[data-testid='toolbar-nested-popup']"));

            cut.Find("[data-testid='toolbar-split-toggle']").Click();
            Assert.Single(cut.FindAll("[data-testid='toolbar-nested-popup']"));
            Assert.Equal("true", cut.Find("[data-testid='toolbar-split-toggle']").GetAttribute("aria-expanded"));
        }

        [Fact(DisplayName = "U-TB-055 Cancel keeps a DropDown nested popup closed and OnItemClick still fires")]
        public void Cancel_KeepsNestedPopupClosed()
        {
            int toolbarClicks = 0;
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Comp<ToolbarItem>(Item("Option A"), ("Text", "More"), ("Id", "more"), ("Type", ToolbarItemType.DropDown)),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, args =>
                {
                    toolbarClicks++;
                    args.Cancel = true;
                })));

            cut.Find("[data-testid='toolbar-dropdown']").Click();

            Assert.Equal(1, toolbarClicks);
            Assert.Empty(cut.FindAll("[data-testid='toolbar-nested-popup']"));
            Assert.Equal("false", cut.Find("[data-testid='toolbar-dropdown']").GetAttribute("aria-expanded"));
        }

        [Fact(DisplayName = "U-TB-012 Arrow keys move nested entries and Escape returns focus to the owner")]
        public async Task NestedMenu_ArrowsAndEscape()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Comp<ToolbarItem>(Compose(Item("A"), Item("B")), ("Text", "More"), ("Type", ToolbarItemType.Menu)));

            await cut.Find("[data-testid='toolbar-menubutton']")
                .ClickAsync(new MouseEventArgs());
            cut.WaitForAssertion(() =>
                Assert.Single(cut.FindAll("[data-testid='toolbar-nested-popup']")));

            await cut.Find("[data-testid='toolbar-nested-popup']")
                .KeyDownAsync(new KeyboardEventArgs { Key = "ArrowDown" });
            cut.WaitForAssertion(() =>
                Assert.Equal("0", cut.FindAll("[data-testid='toolbar-nested-item']")[1].GetAttribute("tabindex")));

            await cut.Find("[data-testid='toolbar-nested-popup']")
                .KeyDownAsync(new KeyboardEventArgs { Key = "Escape" });
            cut.WaitForAssertion(() =>
                Assert.Empty(cut.FindAll("[data-testid='toolbar-nested-popup']")));
        }

        // ------------------------------------------------------------------ groups

        [Fact(DisplayName = "U-TB-013 U-TB-033 GroupName makes Toggle items mutually exclusive across groups")]
        public void GroupName_RadioExclusion()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Group(
                    Item("Bold", ("Type", ToolbarItemType.Toggle), ("GroupName", "style"), ("Selected", true)),
                    Item("Italic", ("Type", ToolbarItemType.Toggle), ("GroupName", "style"))),
                Item("Underline", ("Type", ToolbarItemType.Toggle), ("GroupName", "style"))));

            Assert.NotNull(cut.Find("[data-testid='toolbar-radio-group']"));
            cut.FindAll("[data-testid='toolbar-toggle']")[1].Click();

            IReadOnlyList<IRenderedComponent<ToolbarItem>> items = cut.FindComponents<ToolbarItem>();
            Assert.False(items[0].Instance.Selected);
            Assert.True(items[1].Instance.Selected);
            Assert.False(items[2].Instance.Selected);
        }

        [Fact(DisplayName = "U-TB-013 An empty GroupName does not join a radio group")]
        public void EmptyGroupName_DoesNotExclude()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("One", ("Type", ToolbarItemType.Toggle), ("GroupName", ""), ("Selected", true)),
                Item("Two", ("Type", ToolbarItemType.Toggle), ("GroupName", ""), ("Selected", true))));

            IElement second = cut.FindAll("[data-testid='toolbar-toggle']")[1];
            second.Click();

            Assert.Equal("true", second.GetAttribute("aria-pressed"));
            Assert.Equal("true", cut.FindAll("[data-testid='toolbar-toggle']")[0].GetAttribute("aria-pressed"));
            Assert.Empty(cut.FindAll("[data-testid='toolbar-radio-group']"));
            Assert.Empty(cut.FindAll("[data-testid='toolbar-radio-group']"));
        }

        [Fact(DisplayName = "U-TB-013 Check items with the same GroupName multi-select")]
        public void CheckGroup_MultiSelects()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("Bold", ("Type", ToolbarItemType.Check), ("GroupName", "style")),
                Item("Italic", ("Type", ToolbarItemType.Check), ("GroupName", "style"))));

            Assert.NotNull(cut.Find("[data-testid='toolbar-check-group']"));
            cut.FindAll("[data-testid='toolbar-check']")[0].Click();
            cut.FindAll("[data-testid='toolbar-check']")[1].Click();

            Assert.All(cut.FindComponents<ToolbarItem>(), item => Assert.True(item.Instance.Selected));
        }

        // ------------------------------------------------------------------ data binding

        [Fact(DisplayName = "U-TB-044 Items renders one item per model when no template is set")]
        public void Items_RendersDefaultItems()
        {
            List<ToolbarItemModel> models =
            [
                new() { Id = "cut", Text = "Cut" },
                new() { Id = "copy", Text = "Copy" }
            ];
            IRenderedComponent<SfToolbar> cut = RenderToolbar(configure: p => p.Add(t => t.Items, models));

            Assert.Equal(new[] { "Cut", "Copy" }, cut.FindAll("[data-testid='toolbar-button']").Select(e => e.TextContent.Trim()));
        }

        [Fact(DisplayName = "U-TB-028 ItemTemplate renders once per model with the model on the context")]
        public void ItemTemplate_ReceivesModel()
        {
            List<string> seen = [];
            List<ToolbarItemModel> models = [new() { Id = "cut", Text = "Cut" }, new() { Id = "copy", Text = "Copy" }];
            RenderFragment<ToolbarTemplateContext> template = context => builder =>
            {
                seen.Add(context.Item.Id + ":" + context.IsOverflow);
                builder.AddContent(0, context.Item.Text);
            };
            IRenderedComponent<SfToolbar> cut = RenderToolbar(configure: p => p
                .Add(t => t.Items, models)
                .Add(t => t.ItemTemplate, template));

            Assert.Equal(new[] { "cut:False", "copy:False" }, seen);
            Assert.Contains("Cut", cut.Markup);
            Assert.Contains("Copy", cut.Markup);
        }

        [Fact(DisplayName = "U-TB-045 Declarative children render before data-bound items")]
        public void MixedContent_DeclarativeThenBound()
        {
            List<ToolbarItemModel> models = [new() { Text = "Bound" }];
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Declared"), p => p.Add(t => t.Items, models));

            Assert.Equal(new[] { "Declared", "Bound" }, cut.FindAll("[data-testid='toolbar-button']").Select(e => e.TextContent.Trim()));
        }

        [Fact(DisplayName = "U-TB-044 A null Items collection renders declarative content only")]
        public void NullItems_RendersDeclarativeOnly()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Declared"), p => p.Add(t => t.Items, (IEnumerable<ToolbarItemModel>?)null));

            Assert.Equal(new[] { "Declared" }, cut.FindAll("[data-testid='toolbar-button']").Select(e => e.TextContent.Trim()));
        }

        [Fact(DisplayName = "U-TB-028 An overflowed templated item receives IsOverflow true")]
        public void ItemTemplate_IsOverflowInPopup()
        {
            List<bool> flags = [];
            List<ToolbarItemModel> models =
            [
                new() { Id = "a", Text = "A" },
                new() { Id = "b", Text = "B" },
                new() { Id = "c", Text = "C" },
                new() { Id = "d", Text = "D" },
                new() { Id = "e", Text = "E" }
            ];
            RenderFragment<ToolbarTemplateContext> template = context => builder =>
            {
                flags.Add(context.IsOverflow);
                builder.OpenElement(0, "span");
                builder.AddContent(1, context.Item.Text);
                builder.CloseElement();
            };
            ScriptMeasurement(200, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(configure: p => p
                .Add(t => t.Items, models)
                .Add(t => t.ItemTemplate, template));
            WaitUntilMeasured();
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));
            flags.Clear();

            cut.Find(OverflowButton).Click();

            Assert.Contains(true, flags);
        }

        // ------------------------------------------------------------------ wrap modes

        [Fact(DisplayName = "U-TB-021 MultiRow wraps and does not render the overflow button")]
        public void MultiRow_WrapsWithoutPopup()
        {
            ScriptMeasurement(10, "a", "b");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b"))),
                p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.MultiRow));
            Thread.Sleep(40);

            IElement root = cut.Find("[data-testid='toolbar-root']");
            Assert.Contains("e-toolbar-multirow", root.ClassName);
            Assert.Empty(cut.FindAll(OverflowButton));
            Assert.Empty(cut.Instance.GetOverflowItems());
            Assert.False(JSInterop.Invocations.Any(i => i.Identifier == "observe"));
        }

        [Fact(DisplayName = "U-TB-022 Extended starts collapsed and the expand button toggles the expanded class")]
        public void Extended_TogglesExpanded()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Compose(Item("A"), Item("B")),
                p => p.Add(t => t.OverflowMode, ToolbarOverflowMode.Extended));

            IElement root = cut.Find("[data-testid='toolbar-root']");
            Assert.Contains("e-toolbar-extended", root.ClassName);
            Assert.DoesNotContain("e-toolbar-expanded", root.ClassName);
            Assert.Empty(cut.FindAll(OverflowButton));

            IElement expand = cut.Find("[data-testid='toolbar-expand-button']");
            Assert.Equal("false", expand.GetAttribute("aria-expanded"));
            expand.Click();

            root = cut.Find("[data-testid='toolbar-root']");
            Assert.Contains("e-toolbar-expanded", root.ClassName);
            Assert.Equal("true", cut.Find("[data-testid='toolbar-expand-button']").GetAttribute("aria-expanded"));
            Assert.Empty(cut.FindAll(OverflowPopup));
        }

        // ------------------------------------------------------------------ localization and churn

        [Fact(DisplayName = "U-TB-038 The overflow tooltip and aria-label come from the localizer")]
        public void OverflowLabel_UsesLocalizer()
        {
            Services.AddSingleton<IStringLocalizerFactory>(new StubLocalizerFactory("More commands"));
            ScriptMeasurement(10, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Five());
            WaitUntilMeasured();
            cut.WaitForAssertion(() => Assert.Single(cut.FindAll(OverflowButton)));

            IElement button = cut.Find(OverflowButton);
            Assert.Equal("More commands", button.GetAttribute("aria-label"));
            Assert.Equal("More commands", button.GetAttribute("title"));
        }

        [Fact(DisplayName = "I-TB-012 U-TB-038 A culture change updates the overflow label without a new instance")]
        public void CultureChange_UpdatesLabel()
        {
            SwitchingLocalizer localizer = new("More", "Plus");
            Services.AddSingleton<IStringLocalizerFactory>(new SwitchingLocalizerFactory(localizer));
            ScriptMeasurement(10, "a", "b", "c", "d", "e");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Five());
            WaitUntilMeasured();
            cut.WaitForAssertion(() => Assert.Equal("More", cut.Find(OverflowButton).GetAttribute("aria-label")));

            localizer.Value = "Plus";
            CultureInfo changed = new("fr-FR");
            cut.SetParametersAndRender(p => p.Add(t => t.OverflowSettings, new ToolbarOverflowSettings()));

            Assert.Equal("Plus", cut.Find(OverflowButton).GetAttribute("aria-label"));
            Assert.Same(cut.Instance, cut.Instance);
            _ = changed;
        }

        [Fact(DisplayName = "I-TB-012 EnableRtl updates direction on the same instance")]
        public void RtlChange_UpdatesDirection()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("A"));
            Assert.Null(cut.Find("[data-testid='toolbar-root']").GetAttribute("dir"));

            cut.SetParametersAndRender(p => p.Add(t => t.EnableRtl, true));

            Assert.Equal("rtl", cut.Find("[data-testid='toolbar-root']").GetAttribute("dir"));
        }

        [Fact(DisplayName = "U-TB-040 I-TB-003 Rapid parameter churn ends in the last state")]
        public void ParameterChurn_EndsInLastState()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("A"));
            for (int i = 0; i < 20; i++)
            {
                cut.SetParametersAndRender(p => p
                    .Add(t => t.OverflowMode, i % 2 == 0 ? ToolbarOverflowMode.MultiRow : ToolbarOverflowMode.Extended)
                    .Add(t => t.CssClass, "pass-" + i));
            }

            Assert.Contains("e-toolbar-extended", cut.Find("[data-testid='toolbar-root']").ClassName);
            Assert.Contains("pass-19", cut.Find("[data-testid='toolbar-root']").ClassName);
        }

        [Fact(DisplayName = "U-TB-029 Nested menu content renders without error")]
        public void NestedMenu_RendersWithoutError()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Comp<ToolbarItem>(Item("Nested"), ("Text", "Owner"), ("Type", ToolbarItemType.Menu)));

            Assert.Contains("Owner", cut.Markup);
            cut.Find("[data-testid='toolbar-menubutton']").Click();
            Assert.Contains("Nested", cut.Markup);
        }

        private static RenderFragment Group(params RenderFragment[] items)
        {
            return builder =>
            {
                builder.OpenComponent<ToolbarGroup>(0);
                builder.AddAttribute(1, "Label", "Style");
                builder.AddAttribute(2, "ChildContent", Compose(items));
                builder.CloseComponent();
            };
        }

        private static RenderFragment Five()
        {
            return Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b")), Item("C", ("Id", "c")), Item("D", ("Id", "d")), Item("E", ("Id", "e")));
        }

        private sealed class StubLocalizerFactory(string value) : IStringLocalizerFactory
        {
            public IStringLocalizer Create(Type resourceSource) => new StubLocalizer(value);
            public IStringLocalizer Create(string baseName, string location) => new StubLocalizer(value);
        }

        private sealed class SwitchingLocalizerFactory(SwitchingLocalizer localizer) : IStringLocalizerFactory
        {
            public IStringLocalizer Create(Type resourceSource) => localizer;
            public IStringLocalizer Create(string baseName, string location) => localizer;
        }

        private sealed class StubLocalizer(string value) : IStringLocalizer
        {
            public LocalizedString this[string name] => new(name, value, resourceNotFound: false);

            public LocalizedString this[string name, params object[] arguments] => new(name, value, resourceNotFound: false);

            public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => [];
        }

        private sealed class SwitchingLocalizer(string first, string second) : IStringLocalizer
        {
            public string Value { get; set; } = first;

            public string Second { get; } = second;

            public LocalizedString this[string name] => new(name, Value, resourceNotFound: false);

            public LocalizedString this[string name, params object[] arguments] => this[name];

            public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => [];
        }
    }
}
