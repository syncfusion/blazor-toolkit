using System.IO;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations
{
    public class SfSidebarSampleMarkupTest
    {
        private static readonly string DemoDirectory = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..", "..",
            "samples", "Blazor.Toolkit.Samples.Client", "Pages", "Demos", "Navigations", "Sidebar"));

        private static readonly string[] Demos =
        {
            "DefaultDemo.razor",
            "ModesDemo.razor",
            "MiniRailDemo.razor",
            "PositionsDemo.razor",
            "DualSidebarsDemo.razor",
            "ResponsiveDemo.razor",
            "InteractionControlsDemo.razor",
            "ApiDemo.razor",
        };

        [Fact(Timeout = 10000, DisplayName = "U-SMP-001 Navigate away stays on the page and does not leak a query string")]
        public void NavigateAway_ShouldNotLeakQueryString()
        {
            string source = File.ReadAllText(Path.Combine(DemoDirectory, "ResponsiveDemo.razor"));

            Assert.DoesNotContain("sidebar-nav", source, StringComparison.Ordinal);
            Assert.Contains("Navigation.NavigateTo(\"navigations/sidebar\")", source, StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-SMP-002 The API sample keeps open state, last event and trigger")]
        public void ApiDemo_ShouldLogTrigger()
        {
            string source = File.ReadAllText(Path.Combine(DemoDirectory, "ApiDemo.razor"));

            Assert.Contains("<dt>Open state:</dt>", source, StringComparison.Ordinal);
            Assert.Contains("<dt>Last event:</dt>", source, StringComparison.Ordinal);
            Assert.Contains("<dt>Trigger:</dt>", source, StringComparison.Ordinal);
            Assert.Contains("args.Trigger", source, StringComparison.Ordinal);
            Assert.Contains("Api method caused the Sidebar to change state", source, StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-SMP-003 Close controls follow the sample placement")]
        public void Samples_ShouldPlaceCloseControls()
        {
            Assert.DoesNotContain("sbd-corner-close", File.ReadAllText(Path.Combine(DemoDirectory, "MiniRailDemo.razor")), StringComparison.Ordinal);
            Assert.Contains("sbd-corner-close", File.ReadAllText(Path.Combine(DemoDirectory, "DualSidebarsDemo.razor")), StringComparison.Ordinal);
            Assert.Contains("sbd-corner-close", File.ReadAllText(Path.Combine(DemoDirectory, "InteractionControlsDemo.razor")), StringComparison.Ordinal);
            Assert.Contains("aria-label=\"Close navigation\"", File.ReadAllText(Path.Combine(DemoDirectory, "ModesDemo.razor")), StringComparison.Ordinal);
            Assert.Contains("aria-label=\"Close filters\"", File.ReadAllText(Path.Combine(DemoDirectory, "PositionsDemo.razor")), StringComparison.Ordinal);

            string shared = File.ReadAllText(Path.GetFullPath(Path.Combine(
                DemoDirectory, "..", "..", "..", "Components", "Navigations", "Sidebar.razor")));
            Assert.DoesNotContain("sbd-mobile-toggle", shared, StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-SMP-004 Only the Navigations group icon uses e-toolkit-icons")]
        public void NavMenu_ShouldChangeOnlyNavigationsIcon()
        {
            string source = File.ReadAllText(Path.GetFullPath(Path.Combine(
                DemoDirectory, "..", "..", "..", "..", "Layout", "NavMenu.razor")));

            Assert.Contains("e-toolkit-icons e-navigation sf-menu-icon", source, StringComparison.Ordinal);
            Assert.Contains("sb-icons e-home sf-menu-icon", source, StringComparison.Ordinal);
        }
    }
}
