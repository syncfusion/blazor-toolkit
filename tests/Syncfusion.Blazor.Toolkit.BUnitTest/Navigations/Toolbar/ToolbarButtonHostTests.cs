using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// Proves that SfButton can carry the attributes the toolbar needs (ADR-006, ADR-019).
    /// Covers the precondition of U-TB-057.
    /// </summary>
    public class ToolbarButtonHostTests : ToolbarTestBase
    {
        [Fact(DisplayName = "U-TB-057 SfButton passes role, tabindex and data-testid through HtmlAttributes")]
        public void SfButton_CarriesRoleTabIndexAndTestId()
        {
            IRenderedComponent<SfButton> cut = RenderComponent<SfButton>(p => p
                .Add(b => b.Content, "Cut")
                .AddUnmatched("role", "menuitem")
                .AddUnmatched("tabindex", "-1")
                .AddUnmatched("data-testid", "toolbar-overflow-item"));

            AngleSharp.Dom.IElement button = cut.Find("button");
            Assert.Equal("menuitem", button.GetAttribute("role"));
            Assert.Equal("-1", button.GetAttribute("tabindex"));
            Assert.Equal("toolbar-overflow-item", button.GetAttribute("data-testid"));
        }

        [Theory(DisplayName = "U-TB-057 SfButton renders an aria-pressed value passed in HtmlAttributes when it is not a toggle")]
        [InlineData("true")]
        [InlineData("false")]
        public void SfButton_RendersSuppliedAriaPressedWhenNotToggle(string value)
        {
            IRenderedComponent<SfButton> cut = RenderComponent<SfButton>(p => p
                .Add(b => b.Content, "Bold")
                .AddUnmatched("aria-pressed", value));

            Assert.Equal(value, cut.Find("button").GetAttribute("aria-pressed"));
        }

        [Fact(DisplayName = "U-TB-057 SfButton without aria-pressed renders none")]
        public void SfButton_NoAriaPressedByDefault()
        {
            IRenderedComponent<SfButton> cut = RenderComponent<SfButton>(p => p.Add(b => b.Content, "Plain"));
            Assert.False(cut.Find("button").HasAttribute("aria-pressed"));
        }

        [Fact(DisplayName = "U-TB-057 SfButton toggle keeps its own aria-pressed state")]
        public void SfButton_ToggleKeepsOwnState()
        {
            IRenderedComponent<SfButton> cut = RenderComponent<SfButton>(p => p
                .Add(b => b.Content, "Bold")
                .Add(b => b.IsToggle, true)
                .AddUnmatched("aria-pressed", "true"));

            Assert.Equal("false", cut.Find("button").GetAttribute("aria-pressed"));
            cut.Find("button").Click();
            Assert.Equal("true", cut.Find("button").GetAttribute("aria-pressed"));
        }

        [Fact(DisplayName = "U-TB-061 SfButton moves focus to its button element")]
        public async Task SfButton_FocusElementAsync_FocusesButton()
        {
            IRenderedComponent<SfButton> cut = RenderComponent<SfButton>(p => p.Add(b => b.Content, "Focus me"));

            await cut.InvokeAsync(() => cut.Instance.FocusElementAsync().AsTask());

            Assert.Single(JSInterop.Invocations, i => i.Identifier == "Blazor._internal.domWrapper.focus");
        }
    }
}
