using Bunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    public class TooltipJsMock : BunitTestContext
    {
        protected bool DisableScriptManager = true;

        public TooltipJsMock()
        {
            this.BeforeEachRun();
            this.UpdateRequiredMockJSRuntime();
        }

        public virtual void UpdateRequiredMockJSRuntime()
        {
            JSInterop.Setup<bool>("sfBlazor.isRendered", _ = true).SetResult(true);
            var createMeasureElements = JSInterop.Setup<object>("createMeasureElements");
            createMeasureElements.SetResult("");
            var isDevice = JSInterop.Setup<bool>("sfBlazor.isDevice", false);
            isDevice.SetResult(false);
            var import = JSInterop.Setup<string>("sfBlazor.import", _ => true);
            import.SetResult("");
        }
    }
}
