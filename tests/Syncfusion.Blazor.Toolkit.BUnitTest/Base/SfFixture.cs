using Bunit;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Tests
{
    public class SfFixture : Fixture, IDisposable
    {
        protected bool DisableScriptManager = false;
        private readonly DomRect chartContainerBounds = new() { Width = 600, Height = 450 };

        public BunitJSModuleInterop ChartModule { get; private set; } = null!;
       
        public SfFixture()
        {
            this.BeforeEachRun();
        }

        // Call before GetComponentUnderTest so percentage sizing starts with the intended container.
        internal void SetChartContainerSize(double width, double height)
        {
            chartContainerBounds.Width = width;
            chartContainerBounds.Height = height;
        }

        public virtual void BeforeEachRun()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            ChartModule = ChartBrowserInterop.Setup(JSInterop, chartContainerBounds);
            Services.AddSyncfusionBlazorToolkit(); 
            var options = Options.Create<GlobalOptions>(new GlobalOptions() {  });
            SyncfusionBlazorToolkitService serv = new SyncfusionBlazorToolkitService(options);
            var isScriptRendered = serv.GetType().GetProperty("IsScriptRendered", BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new MissingMemberException($"SfFixture requires the non-public instance property '{serv.GetType().FullName}.IsScriptRendered' to initialize script state.");
            isScriptRendered.SetValue(serv, true);
            Services.AddScoped((IServiceProvider provider) => serv);
        }
    }
}
