using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// Rendering, ARIA and conditional-UI behavior of the Toolbar family.
    /// </summary>
    public class ToolbarRenderTests : ToolbarTestBase
    {
        private static string[] Texts(IRenderedComponent<SfToolbar> cut)
        {
            return cut.FindAll("[data-testid='toolbar-button'], [data-testid='toolbar-toggle']")
                .Select(e => e.TextContent.Trim()).ToArray();
        }

        // ------------------------------------------------------------------ default render and structure

        [Fact(DisplayName = "U-TB-001 Default render: root present, horizontal, Toolkit classes")]
        public void DefaultRender()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar();
            IElement root = cut.Find(Root);

            Assert.Contains("e-toolbar", root.ClassList);
            Assert.Contains("e-toolbar-horizontal", root.ClassList);
            Assert.Equal("toolbar", root.GetAttribute("role"));
            Assert.Equal("horizontal", root.GetAttribute("aria-orientation"));
        }

        [Fact(DisplayName = "U-TB-002 Empty toolbar renders without error and without items")]
        public void EmptyToolbar()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar();
            Assert.Empty(cut.FindAll("[data-testid='toolbar-item']"));
            Assert.Empty(cut.Instance.GetVisibleItems());
        }

        [Fact(DisplayName = "U-TB-003 A single item renders")]
        public void SingleItem()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Cut"));
            Assert.Single(cut.FindAll("[data-testid='toolbar-item']"));
            Assert.Equal(new[] { "Cut" }, Texts(cut));
        }

        [Fact(DisplayName = "U-TB-004 Multiple items render in declaration order")]
        public void MultipleItemsInOrder()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B"), Item("C"), Item("D")));
            Assert.Equal(new[] { "A", "B", "C", "D" }, Texts(cut));
        }

        [Fact(DisplayName = "U-TB-005 Orientation Horizontal applies the horizontal class")]
        public void OrientationHorizontal()
        {
            IElement root = RenderToolbar(null, p => p.Add(t => t.Orientation, ToolbarOrientation.Horizontal)).Find(Root);
            Assert.Contains("e-toolbar-horizontal", root.ClassList);
            Assert.DoesNotContain("e-toolbar-vertical", root.ClassList);
        }

        [Fact(DisplayName = "U-TB-006 Orientation Vertical applies the vertical class and aria-orientation")]
        public void OrientationVertical()
        {
            IElement root = RenderToolbar(null, p => p.Add(t => t.Orientation, ToolbarOrientation.Vertical)).Find(Root);
            Assert.Contains("e-toolbar-vertical", root.ClassList);
            Assert.DoesNotContain("e-toolbar-horizontal", root.ClassList);
            Assert.Equal("vertical", root.GetAttribute("aria-orientation"));
        }

        [Theory(DisplayName = "U-TB-058 Root role and orientation for both orientations")]
        [InlineData(ToolbarOrientation.Horizontal, "horizontal")]
        [InlineData(ToolbarOrientation.Vertical, "vertical")]
        public void RootRoleAndOrientation(ToolbarOrientation orientation, string expected)
        {
            IElement root = RenderToolbar(null, p => p.Add(t => t.Orientation, orientation)).Find(Root);
            Assert.Equal("toolbar", root.GetAttribute("role"));
            Assert.Equal(expected, root.GetAttribute("aria-orientation"));
        }

        // ------------------------------------------------------------------ item types

        [Fact(DisplayName = "U-TB-007 Button item renders a Toolkit SfButton with the toolbar classes")]
        public void ButtonItem()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Cut"));
            IElement button = cut.Find("[data-testid='toolbar-button']");

            Assert.Equal("BUTTON", button.TagName);
            Assert.Contains("e-btn", button.ClassList);
            Assert.Single(cut.FindComponents<SfButton>());
            Assert.False(button.HasAttribute("aria-pressed"));
        }

        [Fact(DisplayName = "U-TB-008 Toggle item renders aria-pressed equal to Selected")]
        public void ToggleItem()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("Off", ("Type", ToolbarItemType.Toggle)),
                Item("On", ("Type", ToolbarItemType.Toggle), ("Selected", true))));

            IReadOnlyList<IElement> toggles = cut.FindAll("[data-testid='toolbar-toggle']");
            Assert.Equal("false", toggles[0].GetAttribute("aria-pressed"));
            Assert.Equal("true", toggles[1].GetAttribute("aria-pressed"));
        }

        [Fact(DisplayName = "U-TB-009 Separator renders with role separator")]
        public void Separator()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Comp<ToolbarSeparator>(), Item("B")));
            IElement separator = cut.Find("[data-testid='toolbar-separator']");
            Assert.Equal("separator", separator.GetAttribute("role"));
        }

        [Fact(DisplayName = "U-TB-009 Separator with Visible=false is not rendered")]
        public void Separator_Hidden()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Comp<ToolbarSeparator>(("Visible", false)));
            Assert.Empty(cut.FindAll("[data-testid='toolbar-separator']"));
        }

        [Fact(DisplayName = "U-TB-009 A separator item type renders a separator")]
        public void SeparatorItemType()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("", ("Type", ToolbarItemType.Separator)));
            Assert.Equal("separator", cut.Find("[data-testid='toolbar-separator']").GetAttribute("role"));
        }

        [Fact(DisplayName = "U-TB-010 Spacer renders a non-interactive flexible element")]
        public void Spacer()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Comp<ToolbarSpacer>(), Item("B")));
            IElement spacer = cut.Find("[data-testid='toolbar-spacer']");
            Assert.Contains("e-toolbar-spacer", spacer.ClassList);
            Assert.Equal("true", spacer.GetAttribute("aria-hidden"));
            Assert.False(spacer.HasAttribute("tabindex"));
        }

        [Fact(DisplayName = "U-TB-010 A spacer item type renders a spacer")]
        public void SpacerItemType()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("", ("Type", ToolbarItemType.Spacer)));
            Assert.Single(cut.FindAll("[data-testid='toolbar-spacer']"));
        }

        [Fact(DisplayName = "U-TB-011 Group renders role group with its label and children")]
        public void Group()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Comp<ToolbarGroup>(
                Compose(Item("Cut"), Item("Copy")), ("Label", "Clipboard")));

            IElement group = cut.Find("[data-testid='toolbar-group']");
            Assert.Equal("group", group.GetAttribute("role"));
            Assert.Equal("Clipboard", group.GetAttribute("aria-label"));
            Assert.Equal(2, group.QuerySelectorAll("[data-testid='toolbar-button']").Length);
        }

        [Fact(DisplayName = "U-TB-011 Group without a label renders no aria-label")]
        public void Group_NoLabel()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Comp<ToolbarGroup>(Item("Cut")));
            Assert.False(cut.Find("[data-testid='toolbar-group']").HasAttribute("aria-label"));
        }

        [Fact(DisplayName = "U-TB-004 Items in a group keep declaration order with items around them")]
        public void GroupKeepsDeclarationOrder()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("A"), Comp<ToolbarGroup>(Compose(Item("B"), Item("C"))), Item("D")));

            Assert.Equal(new[] { "A", "B", "C", "D" }, Texts(cut));
            IReadOnlyList<ComponentBase> visible = cut.Instance.GetVisibleItems();
            Assert.Equal(new[] { "A", "B", "C", "D" }, visible.Cast<ToolbarItem>().Select(i => i.Text).ToArray());
        }

        // ------------------------------------------------------------------ conditional UI

        [Fact(DisplayName = "U-TB-016 Visible=false removes the item from the DOM and the visible list")]
        public void ItemNotVisible()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A"), Item("B", ("Visible", false)), Item("C")));

            Assert.Equal(new[] { "A", "C" }, Texts(cut));
            Assert.Equal(2, cut.Instance.GetVisibleItems().Count);
        }

        [Fact(DisplayName = "U-TB-015 Enabled=false keeps the item in the DOM, disabled and aria-disabled")]
        public void ItemDisabled()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Cut", ("Enabled", false)));
            IElement button = cut.Find("[data-testid='toolbar-button']");

            Assert.True(button.HasAttribute("disabled"));
            Assert.Equal("true", button.GetAttribute("aria-disabled"));
        }

        [Theory(DisplayName = "U-TB-050 DisplayMode controls the icon and the text")]
        [InlineData(ToolbarItemDisplayMode.IconAndText, true, true)]
        [InlineData(ToolbarItemDisplayMode.TextOnly, false, true)]
        [InlineData(ToolbarItemDisplayMode.IconOnly, true, false)]
        public void DisplayModes(ToolbarItemDisplayMode mode, bool hasIcon, bool hasText)
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Save", ("IconCss", "e-icons e-save"), ("DisplayMode", mode)));
            IElement button = cut.Find("[data-testid='toolbar-button']");

            Assert.Equal(hasIcon, button.QuerySelector("span.e-save") is not null);
            Assert.Equal(hasText, button.TextContent.Contains("Save", StringComparison.Ordinal));
        }

        [Fact(DisplayName = "U-TB-050 IconOnly keeps Text as the accessible name")]
        public void IconOnly_KeepsAccessibleName()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Save",
                ("IconCss", "e-icons e-save"), ("DisplayMode", ToolbarItemDisplayMode.IconOnly)));

            Assert.Equal("Save", cut.Find("[data-testid='toolbar-button']").GetAttribute("aria-label"));
        }

        [Fact(DisplayName = "U-TB-050 Tooltip is supplemental and does not replace the text")]
        public void Tooltip_IsSupplemental()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Save", ("Tooltip", "Save the file")));
            IElement button = cut.Find("[data-testid='toolbar-button']");

            Assert.Equal("Save the file", button.GetAttribute("title"));
            Assert.Contains("Save", button.TextContent, StringComparison.Ordinal);
            Assert.False(button.HasAttribute("aria-label"));
        }

        [Fact(DisplayName = "U-TB-050 Badge renders next to the text")]
        public void Badge_Renders()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Inbox", ("Badge", "3")));
            Assert.Equal("3", cut.Find(".e-toolbar-badge").TextContent);
            Assert.Contains("Inbox", cut.Find("[data-testid='toolbar-button']").TextContent, StringComparison.Ordinal);
        }

        // ------------------------------------------------------------------ alignment

        [Theory(DisplayName = "U-TB-049 Align values map to a documented layout class")]
        [InlineData(ToolbarAlignment.Start, "e-toolbar-align-start")]
        [InlineData(ToolbarAlignment.Center, "e-toolbar-align-center")]
        [InlineData(ToolbarAlignment.End, "e-toolbar-align-end")]
        [InlineData(ToolbarAlignment.Justify, "e-toolbar-align-justify")]
        public void Alignment(ToolbarAlignment align, string expectedClass)
        {
            IElement root = RenderToolbar(null, p => p.Add(t => t.Align, align)).Find(Root);
            Assert.Contains(expectedClass, root.ClassList);
        }

        [Theory(DisplayName = "U-TB-019 OverflowMode is reflected in a mode class")]
        [InlineData(ToolbarOverflowMode.Popup, "e-toolbar-popup")]
        [InlineData(ToolbarOverflowMode.Scrollable, "e-toolbar-scrollable")]
        [InlineData(ToolbarOverflowMode.MultiRow, "e-toolbar-multirow")]
        [InlineData(ToolbarOverflowMode.Extended, "e-toolbar-extended")]
        public void OverflowModeClass(ToolbarOverflowMode mode, string expectedClass)
        {
            IElement root = RenderToolbar(null, p => p.Add(t => t.OverflowMode, mode)).Find(Root);
            Assert.Contains(expectedClass, root.ClassList);
        }

        // ------------------------------------------------------------------ root attributes

        [Fact(DisplayName = "U-TB-043 AriaLabel is applied to the root")]
        public void AriaLabel()
        {
            IElement root = RenderToolbar(null, p => p.Add(t => t.AriaLabel, "Main commands")).Find(Root);
            Assert.Equal("Main commands", root.GetAttribute("aria-label"));
        }

        [Fact(DisplayName = "U-TB-043 Without AriaLabel the root renders no aria-label")]
        public void AriaLabel_Absent()
        {
            Assert.False(RenderToolbar().Find(Root).HasAttribute("aria-label"));
        }

        [Fact(DisplayName = "U-TB-043 aria-labelledby passed through HtmlAttributes names the toolbar")]
        public void AriaLabelledBy()
        {
            IElement root = RenderToolbar(null, p => p.AddUnmatched("aria-labelledby", "heading-1")).Find(Root);
            Assert.Equal("heading-1", root.GetAttribute("aria-labelledby"));
        }

        [Fact(DisplayName = "U-TB-017 CssClass and a class attribute merge with the component classes")]
        public void CssClassAndHtmlAttributes()
        {
            IElement root = RenderToolbar(null, p => p
                .Add(t => t.CssClass, "custom-a")
                .AddUnmatched("class", "custom-b")
                .AddUnmatched("data-extra", "x")).Find(Root);

            Assert.Contains("e-toolbar", root.ClassList);
            Assert.Contains("custom-a", root.ClassList);
            Assert.Contains("custom-b", root.ClassList);
            Assert.Equal("x", root.GetAttribute("data-extra"));
        }

        [Fact(DisplayName = "U-TB-018 Width and height in a style attribute reach the root")]
        public void WidthAndHeightThroughStyle()
        {
            IElement root = RenderToolbar(null, p => p.AddUnmatched("style", "width: 320px; height: 48px")).Find(Root);
            Assert.Contains("width: 320px", root.GetAttribute("style"), StringComparison.Ordinal);
            Assert.Contains("height: 48px", root.GetAttribute("style"), StringComparison.Ordinal);
        }

        [Fact(DisplayName = "U-TB-017 Item CssClass is applied to the item")]
        public void ItemCssClass()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Cut", ("CssClass", "my-item")));
            Assert.Contains("my-item", cut.Find("[data-testid='toolbar-item']").ClassList);
        }

        [Fact(DisplayName = "U-TB-052 An explicit item Id is rendered on the item")]
        public void ItemId_IsRendered()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Cut", ("Id", "cut-cmd")));
            Assert.Equal("cut-cmd", cut.Find("[data-testid='toolbar-item']").Id);
        }

        // ------------------------------------------------------------------ RTL

        [Fact(DisplayName = "U-TB-037 EnableRtl=true sets direction rtl and the rtl class")]
        public void Rtl_True()
        {
            IElement root = RenderToolbar(null, p => p.Add(t => t.EnableRtl, true)).Find(Root);
            Assert.Equal("rtl", root.GetAttribute("dir"));
            Assert.Contains("e-rtl", root.ClassList);
        }

        [Fact(DisplayName = "U-TB-037 EnableRtl=false forces left to right")]
        public void Rtl_False()
        {
            IElement root = RenderToolbar(null, p => p.Add(t => t.EnableRtl, false)).Find(Root);
            Assert.Equal("ltr", root.GetAttribute("dir"));
            Assert.DoesNotContain("e-rtl", root.ClassList);
        }

        [Fact(DisplayName = "U-TB-068 EnableRtl=null inherits: no dir attribute and no rtl class")]
        public void Rtl_Null_Inherits()
        {
            IElement root = RenderToolbar().Find(Root);
            Assert.False(root.HasAttribute("dir"));
            Assert.DoesNotContain("e-rtl", root.ClassList);
        }

        [Fact(DisplayName = "U-TB-068 EnableRtl=null follows the global Toolkit RTL option")]
        public void Rtl_Null_FollowsGlobalOption()
        {
            Services.GetRequiredService<SyncfusionBlazorToolkitService>().EnableRtl(true);
            IElement root = RenderToolbar().Find(Root);
            Assert.Equal("rtl", root.GetAttribute("dir"));
            Assert.Contains("e-rtl", root.ClassList);
        }

        // ------------------------------------------------------------------ templates

        [Fact(DisplayName = "U-TB-027 ToolbarTemplateItem renders its content")]
        public void TemplateItem_RendersContent()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Comp<ToolbarTemplateItem>(
                (RenderFragment)(b => b.AddMarkupContent(0, "<input type='search' data-x='y' />"))));

            IElement host = cut.Find("[data-testid='toolbar-template-item']");
            Assert.NotNull(host.QuerySelector("input[data-x='y']"));
        }

        [Fact(DisplayName = "U-TB-027 ToolbarItem Template renders instead of a button and receives the item model")]
        public void ItemTemplate_Renders()
        {
            RenderFragment<ToolbarTemplateContext> template = ctx => b =>
            {
                b.OpenElement(0, "em");
                b.AddAttribute(1, "class", "tpl");
                b.AddContent(2, ctx.Item.Text + ":" + ctx.IsOverflow);
                b.CloseElement();
            };

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Custom", ("Template", template)));

            Assert.Equal("Custom:False", cut.Find("em.tpl").TextContent);
            Assert.Empty(cut.FindAll("[data-testid='toolbar-button']"));
        }

        [Fact(DisplayName = "U-TB-027 ToolbarItem ChildContent renders inside the button")]
        public void ItemChildContent_RendersInsideButton()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Comp<ToolbarItem>(
                (RenderFragment)(b => b.AddMarkupContent(0, "<span class='inner'>X</span>")), ("Text", "Label")));

            IElement button = cut.Find("[data-testid='toolbar-button']");
            Assert.NotNull(button.QuerySelector("span.inner"));
            Assert.Contains("Label", button.TextContent, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "U-TB-029 Nested custom content with interactive children renders without error")]
        public void TemplateItem_NestedInteractive()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Comp<ToolbarTemplateItem>(
                (RenderFragment)(b => b.AddMarkupContent(0, "<button id='inner'>Go</button><a href='#x'>link</a>"))));

            Assert.NotNull(cut.Find("#inner"));
            Assert.NotNull(cut.Find("a[href='#x']"));
        }

        [Fact(DisplayName = "I-TB-002 A null ChildContent and an empty template item are safe")]
        public void NullAndEmptyTemplates_AreSafe()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Comp<ToolbarTemplateItem>(), Item("A", ("Template", null)), Comp<ToolbarGroup>()));

            Assert.NotNull(cut.Find(Root));
            Assert.NotNull(cut.Find("[data-testid='toolbar-template-item']"));
        }

        // ------------------------------------------------------------------ security

        [Fact(DisplayName = "U-TB-051 Text, IconCss, Tooltip and Badge render escaped and create no element")]
        public void Security_EscapesConsumerStrings()
        {
            const string payload = "<img src=x onerror=alert(1)>";
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item(payload,
                ("IconCss", payload), ("Tooltip", payload), ("Badge", payload), ("Id", payload)));

            Assert.Empty(cut.FindAll("img"));
            Assert.Contains("&lt;img", cut.Markup, StringComparison.Ordinal);
            Assert.DoesNotContain("<img", cut.Markup, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "U-TB-052 Quotes and angle brackets in Id and Tooltip do not break attributes")]
        public void Security_AttributeEscaping()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("Cut",
                ("Id", "a\"><script>x</script>"), ("Tooltip", "q\" onmouseover=\"x")));

            Assert.Empty(cut.FindAll("script"));
            IElement button = cut.Find("[data-testid='toolbar-button']");
            Assert.False(button.HasAttribute("onmouseover"));
            Assert.Equal("q\" onmouseover=\"x", button.GetAttribute("title"));
        }

        [Fact(DisplayName = "U-TB-053 Source scan finds MarkupString only in popup, separator and spacer markup")]
        public void Security_NoMarkupStringInSources()
        {
            string dir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "src", "Components", "Navigations", "Toolbar"));
            Assert.True(Directory.Exists(dir), "Toolbar source folder not found: " + dir);

            string[] offenders = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
                .Where(f => f.EndsWith(".cs", StringComparison.Ordinal) || f.EndsWith(".razor", StringComparison.Ordinal))
                .Where(f => File.ReadAllText(f).Contains("MarkupString", StringComparison.Ordinal))
                .Select(Path.GetFileName).OfType<string>().ToArray();

            Assert.Empty(offenders);
        }

        // ------------------------------------------------------------------ misc

        [Fact(DisplayName = "U-TB-066 Invalid enum values, a negative priority and a duplicate Id do not throw")]
        public void InvalidInput_DoesNotThrow()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("A", ("Priority", -5), ("Id", "dup")),
                Item("B", ("Id", "dup")),
                Item("C", ("DisplayMode", (ToolbarItemDisplayMode)99), ("Type", (ToolbarItemType)99))),
                p => p.Add(t => t.Orientation, (ToolbarOrientation)99)
                      .Add(t => t.Align, (ToolbarAlignment)99)
                      .Add(t => t.OverflowMode, (ToolbarOverflowMode)99));

            Assert.NotNull(cut.Find(Root));
            Assert.Equal(3, cut.Instance.GetVisibleItems().Count);
        }

        [Fact(DisplayName = "U-TB-065 Duplicate item ids render in declaration order")]
        public void DuplicateIds_AreDeterministic()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("First", ("Id", "same")), Item("Second", ("Id", "same")), Item("Third", ("Id", "same"))));

            Assert.Equal(new[] { "First", "Second", "Third" }, Texts(cut));
        }
    }
}
