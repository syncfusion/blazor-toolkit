using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// Remaining spec rows that source can prove without the Sample Browser: invalid input, script audit,
    /// scoped CSS, isolation and per-item refresh.
    /// </summary>
    public class ToolbarCompletionTests : ToolbarTestBase
    {
        [Fact(DisplayName = "U-TB-040 An invalid enum, a negative priority and a zero budget do not throw")]
        public void InvalidInput_DoesNotThrow()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("A", ("Priority", -5), ("Type", (ToolbarItemType)99)));

            Assert.Contains("A", cut.Markup, StringComparison.Ordinal);
            OverflowResult result = OverflowCalculator.Calculate(
                [new OverflowCandidate("a", 20, ToolbarOverflowOption.None, -3)],
                double.NaN,
                -1,
                0);
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "U-TB-036 Duplicate item ids render both items")]
        public void DuplicateIds_RenderBoth()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("One", ("Id", "same")),
                Item("Two", ("Id", "same"))));

            Assert.Equal(2, cut.FindAll("[data-testid='toolbar-button']").Count);
        }

        [Fact(DisplayName = "U-TB-016 The measurement script uses only ResizeObserver and getBoundingClientRect")]
        public void Script_UsesOnlyAllowedApis()
        {
            string path = Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory,
                "..", "..", "..", "..", "..",
                "src", "wwwroot", "scripts", "toolbar.js"));
            string script = File.ReadAllText(path);

            Assert.Contains("ResizeObserver", script, StringComparison.Ordinal);
            Assert.Contains("getBoundingClientRect", script, StringComparison.Ordinal);
            Assert.DoesNotContain("eval(", script, StringComparison.Ordinal);
            Assert.DoesNotContain("new Function", script, StringComparison.Ordinal);
            Assert.DoesNotContain("paddingLeft", script, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "U-TB-018 Toolbar CSS is scoped, token-based and free of color literals")]
        public void Styles_AreScopedAndTokenBased()
        {
            // The audit reads the component stylesheet from the repository, the same way the script
            // audit above does. A copy target used to stage the file next to the test assembly, but
            // it was removed with the rest of that staging step and the test kept running.
            string path = Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory,
                "..", "..", "..", "..", "..",
                "src", "Components", "Navigations", "Toolbar", "SfToolbar.razor.css"));
            Assert.True(File.Exists(path), "Toolbar stylesheet not found: " + path);
            string css = File.ReadAllText(path);

            Assert.Contains("var(--color-sf-content-bg-color)", css, StringComparison.Ordinal);
            Assert.Contains("margin-inline-start: auto", css, StringComparison.Ordinal);
            Assert.Contains("flex-wrap: wrap", css, StringComparison.Ordinal);
            Assert.DoesNotContain("#", css, StringComparison.Ordinal);
            Assert.DoesNotContain("rgb(", css, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "U-TB-019 Two toolbars on one page do not share item state")]
        public void TwoToolbars_AreIsolated()
        {
            RenderFragment page = builder =>
            {
                builder.OpenElement(0, "div");
                builder.OpenComponent<SfToolbar>(1);
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b =>
                {
                    b.OpenComponent<ToolbarItem>(0);
                    b.AddAttribute(1, "Text", "Left");
                    b.CloseComponent();
                }));
                builder.CloseComponent();
                builder.OpenComponent<SfToolbar>(3);
                builder.AddAttribute(4, "ChildContent", (RenderFragment)(b =>
                {
                    b.OpenComponent<ToolbarItem>(0);
                    b.AddAttribute(1, "Text", "Right");
                    b.AddAttribute(2, "Type", ToolbarItemType.Toggle);
                    b.AddAttribute(3, "Selected", true);
                    b.CloseComponent();
                }));
                builder.CloseComponent();
                builder.CloseElement();
            };

            var cut = Render(page);
            Assert.Equal(2, cut.FindAll("[data-testid='toolbar-root']").Count);
            Assert.Single(cut.FindAll("[aria-pressed='true']"));
            Assert.Contains("Left", cut.Markup, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "U-TB-061 An item refresh does not require a new toolbar instance")]
        public void ItemRefresh_KeepsTheSameInstance()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Item("A", ("Type", ToolbarItemType.Toggle)));
            SfToolbar before = cut.Instance;

            cut.Find("[data-testid='toolbar-toggle']").Click();

            Assert.Same(before, cut.Instance);
            Assert.Equal("true", cut.Find("[data-testid='toolbar-toggle']").GetAttribute("aria-pressed"));
        }

        private IRenderedFragment Render(RenderFragment fragment)
        {
            return RenderComponent<PassThrough>(p => p.Add(c => c.ChildContent, fragment));
        }

        private sealed class PassThrough : ComponentBase
        {
            [Parameter]
            public RenderFragment? ChildContent { get; set; }

            protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
            {
                builder.AddContent(0, ChildContent);
            }
        }
    }
}
