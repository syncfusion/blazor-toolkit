using Bunit;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Syncfusion.Blazor.Toolkit.Tests
{
    public class SfFixture : Fixture, IDisposable
    {
        protected bool DisableScriptManager = false;
       
        public SfFixture()
        {
            this.BeforeEachRun();
        }

        public virtual void BeforeEachRun()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            Services.AddSyncfusionBlazorToolkit(); 
            var options = Options.Create<GlobalOptions>(new GlobalOptions() {  });
            SyncfusionBlazorService serv = new SyncfusionBlazorService(options);
            serv.GetType().GetProperty("IsScriptRendered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(serv, true);
            Services.AddScoped((IServiceProvider provider) => serv);
        }
    }
}
