using Bunit;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Tests
{
    public class BunitTestContext : TestContext
    {
        private static readonly CultureInfo TestCulture = BuildTestCulture();
        private static CultureInfo BuildTestCulture()
        {
            var cultureInfo = (CultureInfo)CultureInfo.GetCultureInfo("en-US").Clone();
            cultureInfo.DateTimeFormat.ShortDatePattern = "M/d/yyyy";
            cultureInfo.DateTimeFormat.LongDatePattern = "dddd, MMMM d, yyyy";
            cultureInfo.DateTimeFormat.ShortTimePattern = "h:mm tt";
            cultureInfo.DateTimeFormat.LongTimePattern = "h:mm:ss tt";
            cultureInfo.DateTimeFormat.FullDateTimePattern = "dddd, MMMM d, yyyy h:mm:ss tt";
            cultureInfo.DateTimeFormat.ShortestDayNames = new[] { "S", "M", "T", "W", "T", "F", "S" };
            return CultureInfo.ReadOnly(cultureInfo);
        }

        static BunitTestContext()
        {
            CultureInfo.DefaultThreadCurrentCulture = TestCulture;
            CultureInfo.DefaultThreadCurrentUICulture = TestCulture;
            CultureInfo.CurrentCulture = TestCulture;
            CultureInfo.CurrentUICulture = TestCulture;
        }

        public BunitTestContext()
        {
            Thread.CurrentThread.CurrentCulture = TestCulture;
            Thread.CurrentThread.CurrentUICulture = TestCulture;
            CultureInfo.CurrentCulture = TestCulture;
            CultureInfo.CurrentUICulture = TestCulture;
            BeforeEachRun();
        }

        public virtual void BeforeEachRun()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            Services.AddSyncfusionBlazorToolkit();
            Services.AddOptions();
        }

        public new void Dispose()
        {
            base.Dispose();
            AfterEachRun();
        }

        public virtual void AfterEachRun() { }
    }

    public abstract class BaseTestContext : TestContext
    //IBeforeTestStarting, IBeforeTestFinished, IAfterTestStarting, IAfterTestFinished
    {

    }
}
