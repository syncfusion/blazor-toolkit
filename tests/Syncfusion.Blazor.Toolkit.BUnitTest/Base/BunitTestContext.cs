using Bunit;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Tests
{
    public class BunitTestContext : TestContext
    {
        public BunitTestContext()
        {
            // Create a new culture based on en-US and set the ShortDatePattern
            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.DateTimeFormat.ShortDatePattern = "M/d/yyyy";
            cultureInfo.DateTimeFormat.ShortestDayNames = new[] { "S", "M", "T", "W", "T", "F", "S" };

            // Apply this culture globally
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            this.BeforeEachRun();
        }

        public virtual void BeforeEachRun()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            Services.AddSyncfusionBlazorToolkit();
            Services.AddOptions();
        }

        public void Dispose()
        {
            base.Dispose();
            this.AfterEachRun();
        }

        public virtual void AfterEachRun() { }
    }

    public abstract class BaseTestContext : TestContext
    //IBeforeTestStarting, IBeforeTestFinished, IAfterTestStarting, IAfterTestFinished
    {

    }
}
