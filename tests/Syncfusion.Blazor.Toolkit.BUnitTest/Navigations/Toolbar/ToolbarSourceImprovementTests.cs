using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Text.RegularExpressions;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// Source-side Toolbar improvements: RTL icon order stays with the toolbar, the bar has no
    /// outer border, scroll controls use existing glyphs, and the icon catalog matches the payload.
    /// </summary>
    public class ToolbarSourceImprovementTests : ToolbarTestBase
    {
        [Fact(DisplayName = "TB-RTL-CLICK Icon stays after text after click when the toolbar is RTL and the page is not")]
        public void RtlToolbar_KeepsIconAfterTextAfterClick()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Item("Cut", ("IconCss", "e-icons e-cut"), ("Id", "cut")),
                p => p.Add(t => t.EnableRtl, true));

            AssertIconOrder(cut, before: false);
            cut.Find("[data-testid='toolbar-button']").Click();
            AssertIconOrder(cut, before: false);
        }

        [Fact(DisplayName = "TB-RTL-LTR Icon stays before text after click when the toolbar is LTR")]
        public void LtrToolbar_KeepsIconBeforeTextAfterClick()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Item("Cut", ("IconCss", "e-icons e-cut"), ("Id", "cut")),
                p => p.Add(t => t.EnableRtl, false));

            AssertIconOrder(cut, before: true);
            cut.Find("[data-testid='toolbar-button']").Click();
            AssertIconOrder(cut, before: true);
        }

        [Fact(DisplayName = "TB-RTL-TOGGLE A selected toggle does not swap icon and text in RTL")]
        public void RtlToggle_KeepsIconBeforeTextWhenSelected()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Item("Bold", ("IconCss", "e-icons e-bold"), ("Type", ToolbarItemType.Toggle), ("Id", "bold")),
                p => p.Add(t => t.EnableRtl, true));

            cut.Find("[data-testid='toolbar-toggle']").Click();
            Assert.Equal("true", cut.Find("[data-testid='toolbar-toggle']").GetAttribute("aria-pressed"));
            AssertIconOrder(cut, before: false);
        }

        [Fact(DisplayName = "TB-RTL-KEY Keyboard activation does not swap icon and text in RTL")]
        public async Task RtlToolbar_KeepsIconBeforeTextAfterKey()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(
                Item("Cut", ("IconCss", "e-icons e-cut"), ("Id", "cut")),
                p => p.Add(t => t.EnableRtl, true));

            await cut.Find("[data-testid='toolbar-button']").KeyDownAsync(new KeyboardEventArgs { Key = "Enter" });
            AssertIconOrder(cut, before: false);
        }

        [Fact(DisplayName = "TB-VIS-BORDER The toolbar stylesheet has no outer border and names the scroll glyphs")]
        public void Styles_RemoveOuterBorderAndNameScrollGlyphs()
        {
            string css = File.ReadAllText(ToolbarStylesheet());
            string script = File.ReadAllText(ToolbarScript());

            Assert.Contains("border: 0;", css, StringComparison.Ordinal);
            Assert.DoesNotContain("border: var(--e-toolbar-border-width)", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar-ripple", css, StringComparison.Ordinal);
            Assert.Contains("opacity: 0.32;", css, StringComparison.Ordinal);
            Assert.Contains("animation: e-toolbar-ripple 1000ms ease-out forwards;", css, StringComparison.Ordinal);
            Assert.DoesNotContain(".e-tbar-btn::after", css, StringComparison.Ordinal);
            Assert.Contains("prefers-reduced-motion", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar.e-toolbar-scrollable:not(.e-toolbar-vertical) .e-toolbar-scroll-controls-start", css, StringComparison.Ordinal);
            Assert.Contains("border-inline-end: 1px solid var(--color-sf-border);", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar.e-toolbar-scrollable:not(.e-toolbar-vertical) .e-toolbar-scroll-controls-end", css, StringComparison.Ordinal);
            Assert.Contains("border-inline-start: 1px solid var(--color-sf-border);", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar.e-toolbar-scrollable.e-toolbar-vertical .e-toolbar-scroll-controls-start", css, StringComparison.Ordinal);
            Assert.Contains("border-block-end: 1px solid var(--color-sf-border);", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar.e-toolbar-scrollable.e-toolbar-vertical .e-toolbar-scroll-controls-end", css, StringComparison.Ordinal);
            Assert.Contains("border-block-start: 1px solid var(--color-sf-border);", css, StringComparison.Ordinal);
            Assert.DoesNotContain("border-inline: 1px solid var(--color-sf-border);", css, StringComparison.Ordinal);
            Assert.Contains("const bounds = button.getBoundingClientRect();", script, StringComparison.Ordinal);
            Assert.Contains("clientX - bounds.left", script, StringComparison.Ordinal);
            Assert.Contains("clientY - bounds.top", script, StringComparison.Ordinal);
            Assert.Contains("}, 1000);", script, StringComparison.Ordinal);
            Assert.Contains("const ScrollAnimationMs = 1000;", script, StringComparison.Ordinal);
            Assert.Contains("prefers-reduced-motion: reduce", script, StringComparison.Ordinal);
            Assert.Contains("Popup, MultiRow and Extended do not call this", script, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "TB-OVF Popup content stays on one row while width changes")]
        public void PopupContent_DoesNotWrap()
        {
            string css = File.ReadAllText(ToolbarStylesheet());
            Match rule = Regex.Match(
                css,
                @"(?s)\.e-toolbar\.e-toolbar-popup \.e-toolbar-content\s*\{(?<declarations>[^}]*)\}");

            Assert.True(rule.Success, "Popup content must have an explicit single-row layout rule.");
            Assert.Contains("flex-wrap: nowrap;", rule.Groups["declarations"].Value, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "TB-THM Toolbar uses the reviewed background, hover, and selected colors")]
        public void Styles_UseReviewedToolbarPalette()
        {
            string css = File.ReadAllText(ToolbarStylesheet());

            Assert.Contains("--e-toolbar-background: #F1F5F9;", css, StringComparison.Ordinal);
            Assert.Contains("--e-toolbar-hover-background: #E2E8F0;", css, StringComparison.Ordinal);
            Assert.Contains("--e-toolbar-selected-background: #DBEAFE;", css, StringComparison.Ordinal);
            Assert.DoesNotContain("--e-toolbar-selected-border:", css, StringComparison.Ordinal);
            Assert.Contains("background: var(--e-toolbar-background);", css, StringComparison.Ordinal);
            Assert.Contains("background: var(--e-toolbar-hover-background);", css, StringComparison.Ordinal);
            Assert.Contains("background: var(--e-toolbar-selected-background);", css, StringComparison.Ordinal);
            Assert.DoesNotContain("border: 1px solid var(--e-toolbar-selected-border);", css, StringComparison.Ordinal);
            Assert.Contains(".e-tbar-btn:hover:where(:not(:disabled))", css, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "TB-THM Popup and nested menus use the toolbar surface palette")]
        public void PopupSurfaces_UseToolbarBackground()
        {
            string css = File.ReadAllText(ToolbarStylesheet());

            Assert.Matches(@"(?s)\.e-toolbar-overflow-popup\s*\{\s*background:\s*var\(--e-toolbar-background\);", css);
            Assert.Matches(@"(?s)\.e-toolbar ::deep \.e-toolbar-nested-menu\s*\{\s*background:\s*var\(--e-toolbar-background\);", css);
            Assert.DoesNotContain("background: var(--color-sf-content-bg-color);", css, StringComparison.Ordinal);
            Assert.DoesNotContain("background: var(--color-sf-flyout-bg-color, var(--color-sf-content-bg-color));", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar-overflow-popup ::deep .e-toolbar-overflow-entry", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar ::deep .e-toolbar-nested-menu .e-tbar-btn", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar ::deep .e-tbar-btn:hover:where(:not(:disabled))", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar ::deep .e-tbar-btn.e-active", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar ::deep .e-toolbar-item.e-selected .e-tbar-btn", css, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "TB-OVF Nested menu ripples stay inside buttons without horizontal scrolling")]
        public void NestedMenu_ContainsRipplesAndOnlyScrollsVertically()
        {
            string css = File.ReadAllText(ToolbarStylesheet());
            Match menuRule = Regex.Match(
                css,
                @"(?s)\.e-toolbar ::deep \.e-toolbar-nested-menu\s*\{(?<declarations>[^}]*)\}");
            Match buttonRule = Regex.Match(
                css,
                @"(?s)\.e-toolbar ::deep \.e-toolbar-nested-menu \.e-toolbar-item,\s*\.e-toolbar ::deep \.e-toolbar-nested-menu \.e-tbar-btn\s*\{(?<declarations>[^}]*)\}");

            Assert.True(menuRule.Success, "Nested-menu styles were not found.");
            Assert.Contains("overflow-x: hidden;", menuRule.Groups["declarations"].Value, StringComparison.Ordinal);
            Assert.Contains("overflow-y: auto;", menuRule.Groups["declarations"].Value, StringComparison.Ordinal);
            Assert.True(buttonRule.Success, "Nested-menu button styles were not found.");
            Assert.DoesNotContain("position: static;", buttonRule.Groups["declarations"].Value, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "TB-RTL-SCROLL Horizontal scrolling reads and writes logical RTL positions")]
        public void Scrollable_NormalizesHorizontalScrollPositions()
        {
            string script = File.ReadAllText(ToolbarScript());
            string css = File.ReadAllText(ToolbarStylesheet());

            Assert.Contains("function logicalScrollPosition(", script, StringComparison.Ordinal);
            Assert.Contains("function setLogicalScrollPosition(", script, StringComparison.Ordinal);
            Assert.Contains("function isFullyVisible(", script, StringComparison.Ordinal);
            Assert.Contains("function clampToVisibleBoundary(", script, StringComparison.Ordinal);
            Assert.Contains("function measuredToolbarItems(", script, StringComparison.Ordinal);
            Assert.Contains("item.closest('.e-toolbar-content') === scroller && !item.closest('.e-toolbar-nested-menu')", script, StringComparison.Ordinal);
            Assert.Contains("atStart: firstItem ? isFullyVisible(firstItem, scroller, vertical) : position <= tolerance", script, StringComparison.Ordinal);
            Assert.Contains("atEnd: lastItem ? isFullyVisible(lastItem, scroller, vertical) : position >= max - tolerance", script, StringComparison.Ordinal);
            Assert.Contains("const instance = instances.get(scroller) || {", script, StringComparison.Ordinal);
            Assert.Contains("instances.set(scroller, instance);", script, StringComparison.Ordinal);
            Assert.Contains("export function unobserveScroll(scroller)", script, StringComparison.Ordinal);
            Assert.Contains("notifyScrollState(scroller);", script, StringComparison.Ordinal);
            Assert.Contains(
                "getComputedStyle(scroller).direction === 'rtl' ? -scroller.scrollLeft : scroller.scrollLeft",
                script,
                StringComparison.Ordinal);
            Assert.Contains(
                "scroller.scrollLeft = getComputedStyle(scroller).direction === 'rtl' ? -position : position;",
                script,
                StringComparison.Ordinal);
            Assert.Contains("logicalScrollPosition(scroller, vertical)", script, StringComparison.Ordinal);
            Assert.Contains("setLogicalScrollPosition(scroller, clamped, vertical)", script, StringComparison.Ordinal);
            Assert.Contains("instance.dotNetRef.invokeMethodAsync('OnScrolledAsync')", script, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar:dir(rtl)", css, StringComparison.Ordinal);
            Assert.Contains("transform: scaleX(-1);", css, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "TB-SCR-ICON Scroll controls use the existing small chevron classes")]
        public void Scrollable_UsesExistingSmallChevrons()
        {
            string source = File.ReadAllText(Path.Combine(
                Path.GetDirectoryName(ToolbarStylesheet())!,
                "SfToolbar.razor.Scrollable.cs"));

            Assert.Contains("\"e-icons e-chevron-left\"", source, StringComparison.Ordinal);
            Assert.Contains("\"e-icons e-chevron-right\"", source, StringComparison.Ordinal);
            Assert.DoesNotContain("e-chevron-left-small", source, StringComparison.Ordinal);
            Assert.DoesNotContain("e-chevron-right-small", source, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "TB-OVF Vertical layout wraps columns and keeps overflowing alignment reachable")]
        public void VerticalOverflowModes_UseAxisAwareWrappingAndSafeAlignment()
        {
            string css = File.ReadAllText(ToolbarStylesheet());

            Assert.Contains(".e-toolbar.e-toolbar-align-center .e-toolbar-content", css, StringComparison.Ordinal);
            Assert.Contains("justify-content: safe center;", css, StringComparison.Ordinal);
            Assert.Contains("justify-content: safe flex-end;", css, StringComparison.Ordinal);
            Assert.Contains("justify-content: space-between;", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar.e-toolbar-multirow.e-toolbar-vertical", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar.e-toolbar-extended.e-toolbar-vertical", css, StringComparison.Ordinal);
            Match verticalContentRule = Regex.Match(
                css,
                @"(?s)\.e-toolbar\.e-toolbar-vertical \.e-toolbar-content\s*\{(?<declarations>[^}]*)\}");
            Assert.True(verticalContentRule.Success, "Vertical toolbar content must have an explicit flex layout rule.");
            Assert.Contains("flex: 1 1 auto;", verticalContentRule.Groups["declarations"].Value, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar.e-toolbar-multirow.e-toolbar-vertical .e-toolbar-content", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar.e-toolbar-extended.e-toolbar-vertical .e-toolbar-content", css, StringComparison.Ordinal);
            Assert.Contains("--e-toolbar-wrapped-root-inline-size", css, StringComparison.Ordinal);
            Assert.Contains("--e-toolbar-wrapped-content-inline-size", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar.e-toolbar-extended.e-toolbar-vertical:not(.e-toolbar-expanded)", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar-item-extended-collapsed", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar-vertical.e-toolbar-extended:not(.e-toolbar-expanded) ::deep .e-toolbar-separator", css, StringComparison.Ordinal);
            Assert.Contains(".e-toolbar-vertical.e-toolbar-extended:not(.e-toolbar-expanded) ::deep .e-toolbar-spacer", css, StringComparison.Ordinal);
            Assert.Matches(
                @"(?s)\.e-toolbar\.e-toolbar-extended\.e-toolbar-vertical:not\(\.e-toolbar-expanded\)\s*\{\s*max-block-size:\s*none;\s*overflow:\s*visible;",
                css);
            Assert.Matches(
                @"(?s)\.e-toolbar\.e-toolbar-extended\.e-toolbar-vertical \.e-toolbar-overflow\s*\{[^}]*position:\s*static;",
                css);
        }

        [Fact(DisplayName = "TB-OVF Vertical Split item gives its label space without stretching its chevron")]
        public void VerticalSplitItem_UsesAvailableWidthForItsLabel()
        {
            string css = File.ReadAllText(ToolbarStylesheet());
            Match splitRule = Regex.Match(
                css,
                @"(?s)\.e-toolbar\.e-toolbar-vertical ::deep \.e-toolbar-split\s*\{(?<declarations>[^}]*)\}");
            Match primaryButtonRule = Regex.Match(
                css,
                @"(?s)\.e-toolbar\.e-toolbar-vertical ::deep \.e-toolbar-split \.e-tbar-btn:not\(\.e-toolbar-chevron\)\s*\{(?<declarations>[^}]*)\}");
            Match chevronRule = Regex.Match(
                css,
                @"(?s)\.e-toolbar\.e-toolbar-vertical ::deep \.e-toolbar-split \.e-toolbar-chevron\s*\{(?<declarations>[^}]*)\}");

            Assert.True(splitRule.Success, "Vertical Split items must fill their toolbar item.");
            Assert.Contains("display: flex;", splitRule.Groups["declarations"].Value, StringComparison.Ordinal);
            Assert.Contains("inline-size: 100%;", splitRule.Groups["declarations"].Value, StringComparison.Ordinal);
            Assert.True(primaryButtonRule.Success, "The Split primary button must use the remaining inline space.");
            Assert.Contains("flex: 1 1 auto;", primaryButtonRule.Groups["declarations"].Value, StringComparison.Ordinal);
            Assert.Contains("inline-size: auto;", primaryButtonRule.Groups["declarations"].Value, StringComparison.Ordinal);
            Assert.Contains("min-inline-size: 0;", primaryButtonRule.Groups["declarations"].Value, StringComparison.Ordinal);
            Assert.True(chevronRule.Success, "The Split chevron must retain its compact target size.");
            Assert.Contains("flex: 0 0 var(--e-toolbar-target);", chevronRule.Groups["declarations"].Value, StringComparison.Ordinal);
            Assert.Contains("inline-size: var(--e-toolbar-target);", chevronRule.Groups["declarations"].Value, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "TB-OVF Vertical Popup control stays at inline-start")]
        public void VerticalPopupControl_AlignsWithItemsAtBlockEnd()
        {
            string css = File.ReadAllText(ToolbarStylesheet());
            Match rule = Regex.Match(
                css,
                @"(?s)\.e-toolbar\.e-toolbar-vertical\.e-toolbar-popup \.e-toolbar-overflow\s*\{(?<declarations>[^}]*)\}");
            Match contentRule = Regex.Match(
                css,
                @"(?s)\.e-toolbar\.e-toolbar-vertical\.e-toolbar-popup \.e-toolbar-content\s*\{(?<declarations>[^}]*)\}");

            Assert.True(rule.Success, "Vertical Popup mode must override the horizontal overflow-control placement.");
            Assert.Contains("position: absolute;", rule.Groups["declarations"].Value, StringComparison.Ordinal);
            Assert.Contains("inset-block-end: var(--e-toolbar-padding-block);", rule.Groups["declarations"].Value, StringComparison.Ordinal);
            Assert.Contains("inset-inline-start: var(--e-toolbar-padding-inline);", rule.Groups["declarations"].Value, StringComparison.Ordinal);
            Assert.True(contentRule.Success, "Vertical Popup content must reserve a dedicated area for its control.");
            Assert.Contains(
                "padding-block-end: calc(var(--e-toolbar-target) + var(--e-toolbar-gap));",
                contentRule.Groups["declarations"].Value,
                StringComparison.Ordinal);
        }

        [Fact(DisplayName = "TB-ICON-CATALOG Every catalog code point is a mapping already in the theme payload")]
        public void IconCatalog_MatchesPayloadMappings()
        {
            string payload = File.ReadAllText(Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory,
                "..", "..", "..", "..", "..",
                "src", "Base", "SfThemeRoot.Payload.cs")));

            Assert.NotEmpty(ToolkitIconCatalog._entries);

            foreach (ToolkitIconCatalog.Entry entry in ToolkitIconCatalog._entries)
            {
                string point = "\\" + ((int)entry.CodePoint[0]).ToString("x4", System.Globalization.CultureInfo.InvariantCulture);
                string mapping = ".e-icons." + entry.ClassName + "::before";
                Assert.Contains(mapping, payload, StringComparison.Ordinal);
                Assert.Contains(point, payload, StringComparison.Ordinal);
            }

            string catalog = File.ReadAllText(Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory,
                "..", "..", "..", "..", "..",
                "src", "Base", "ToolkitIconCatalog.cs")));
            Assert.DoesNotContain("SfThemeRoot", catalog, StringComparison.Ordinal);
            Assert.Contains("internal static class ToolkitIconCatalog", catalog, StringComparison.Ordinal);
        }

        private static void AssertIconOrder(IRenderedComponent<SfToolbar> cut, bool before)
        {
            AngleSharp.Dom.IElement button = cut.Find("[data-testid='toolbar-button'], [data-testid='toolbar-toggle']");
            string markup = button.InnerHtml;
            int icon = markup.IndexOf("toolbar-item-icon", StringComparison.Ordinal);
            int text = markup.IndexOf("toolbar-item-text", StringComparison.Ordinal);
            bool iconLeads = icon >= 0 && text > icon;
            Assert.True(icon >= 0 && text >= 0, "Icon or text was missing: " + markup);
            Assert.Equal(before, iconLeads);
        }

        private static string ToolbarStylesheet()
        {
            return Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory,
                "..", "..", "..", "..", "..",
                "src", "Components", "Navigations", "Toolbar", "SfToolbar.razor.css"));
        }

        private static string ToolbarScript()
        {
            return Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory,
                "..", "..", "..", "..", "..",
                "src", "wwwroot", "scripts", "toolbar.js"));
        }
    }
}
