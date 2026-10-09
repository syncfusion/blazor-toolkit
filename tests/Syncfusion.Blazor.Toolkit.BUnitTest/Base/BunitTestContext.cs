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

        /// <summary>
        /// When <see langword="true"/> (default) the render-time theme emitter (<see cref="SfThemeRoot"/>) is replaced by a
        /// bUnit stub. The emitter writes a ~90 KB &lt;style&gt; element into the first render of every component; bUnit
        /// re-serialises and re-parses the whole markup on every query, which makes the ~2,600 component tests ~2x slower and
        /// pushes the time-boxed ones over their timeout. The emitter itself is covered by <c>SfThemeRootTests</c> and
        /// <c>SfThemeRootEnforcementTests</c>, which opt out by overriding this property.
        /// </summary>
        protected virtual bool StubThemeRoot => true;

        public virtual void BeforeEachRun()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            Services.AddSyncfusionBlazorToolkit();
            Services.AddOptions();
            if (StubThemeRoot)
            {
                ComponentFactories.AddStub<SfThemeRoot>();
            }
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
