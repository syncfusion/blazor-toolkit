using Bunit;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Tests
{
    internal static class ChartBrowserInterop
    {
        public static BunitJSModuleInterop Setup(BunitJSInterop jsInterop, DomRect? containerBounds = null)
        {
            containerBounds ??= new DomRect { Width = 600, Height = 450 };
            // Match SfChart's lifecycle imports without accepting arbitrary modules.
            jsInterop.SetupModule("./_content/Syncfusion.Blazor.Toolkit/scripts/base.js").Mode = JSRuntimeMode.Loose;
            jsInterop.SetupModule("./_content/Syncfusion.Blazor.Toolkit/scripts/touch.js").Mode = JSRuntimeMode.Loose;
            jsInterop.SetupModule("./_content/Syncfusion.Blazor.Toolkit/scripts/svgbase.js").Mode = JSRuntimeMode.Loose;
            jsInterop.SetupModule("./_content/Syncfusion.Blazor.Toolkit/scripts/animation.js").Mode = JSRuntimeMode.Loose;

            var chartModule = jsInterop.SetupModule("./_content/Syncfusion.Blazor.Toolkit/scripts/chart.js");
            chartModule.Mode = JSRuntimeMode.Loose;
            chartModule.Setup<string>("getCharCollectionSize", _ => true).SetResult("[]");
            chartModule.Setup<string>("getCharSizeByFontKeys", _ => true).SetResult("{}");
            chartModule.Setup<DomRect>("getParentElementBoundsById", _ => true)
                .SetResult(containerBounds);
            chartModule.Setup<DomRect>("getRefreshElementBoundsById", _ => true)
                .SetResult(containerBounds);
            chartModule.Setup<DomRect>("getElementBoundsById",
                invocation => invocation.Arguments[0] is string id && id.EndsWith("_svg", StringComparison.Ordinal))
                .SetResult(new DomRect { Width = 600, Height = 450 });
            chartModule.Setup<DomRect>("getElementBoundsById",
                invocation => invocation.Arguments[0] is not string id || !id.EndsWith("_svg", StringComparison.Ordinal))
                .SetResult(containerBounds);

            // Template geometry belongs to individual tests, not shared array defaults.
            return chartModule;
        }
    }
}
