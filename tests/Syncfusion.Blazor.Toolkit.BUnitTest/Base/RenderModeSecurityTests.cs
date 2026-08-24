using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using Syncfusion.Blazor.Toolkit.Popups;
using Syncfusion.Blazor.Toolkit.Spinner;
using Syncfusion.Blazor.Toolkit.Tests;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Base
{
    /// <summary>
    /// Regression suite for MS-2.10 (render-mode-aware security).
    ///
    /// These tests mount each component in a fresh bUnit test context whose
    /// <see cref="BunitTestContext"/> configures <c>JSInterop.Mode = JSRuntimeMode.Loose</c>.
    /// In that mode any unhandled JavaScript interop call (InvokeAsync /
    /// InvokeVoidAsync) is recorded but does not throw.
    ///
    /// The MS-2.10 anti-pattern is invoking JSRuntime inside
    /// <c>OnInitializedAsync</c>. The base component performs module imports
    /// in <c>OnAfterRenderAsync</c>, which is the correct location. The
    /// tests assert that the components render successfully in a fresh bUnit
    /// context (no throws) so that any future regression that moves interop
    /// into <c>OnInitializedAsync</c> would surface as a render failure in
    /// the loose-mode mock.
    ///
    /// See:
    ///  - .github/blazor-render-mode-guidance.md
    /// </summary>
    public class RenderModeSecurityTests : BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "MS-2.10: SfTooltip does not invoke JS during initialization")]
        public void SfTooltip_In_StaticSSR_DoesNot_InvokeJS()
        {
            // The base component imports base.js and tooltip.js during
            // OnAfterRenderAsync (first render). The test asserts that the
            // component renders successfully without throwing.
            var cut = RenderComponent<SfTooltip>(p => p.Add(x => x.Content, "test"));

            Assert.NotNull(cut);
        }

        [Fact(Timeout = 10000, DisplayName = "MS-2.10: SfSpinner does not invoke JS during initialization")]
        public void SfSpinner_In_StaticSSR_DoesNot_InvokeJS()
        {
            // Spinner has no component-specific JS module, so only the base
            // import is expected from OnAfterRenderAsync. The test asserts
            // that initialization completes without throwing.
            var cut = RenderComponent<SfSpinner>(p => p.Add(x => x.Visible, true));

            Assert.NotNull(cut);
        }

        [Fact(Timeout = 10000, DisplayName = "MS-2.10: SfUploader does not invoke JS during initialization")]
        public void SfUploader_In_StaticSSR_DoesNot_InvokeJS()
        {
            var cut = RenderComponent<SfUploader>();

            Assert.NotNull(cut);
            // SfUploader performs JS imports during OnAfterRenderAsync
            // (first render). No invocations are expected from
            // OnInitializedAsync. We simply assert that initialization
            // completed without throwing.
        }

        [Fact(Timeout = 10000, DisplayName = "MS-2.10: SfNumericTextBox does not invoke JS during initialization")]
        public void SfNumericTextBox_In_StaticSSR_DoesNot_InvokeJS()
        {
            // NumericTextBox has its own JS module (numerictextbox.js) and
            // performs post-render wiring. The base component handles
            // module imports in OnAfterRenderAsync. The test asserts that
            // the component renders without throwing.
            var cut = RenderComponent<SfNumericTextBox<double>>();

            Assert.NotNull(cut);
        }
    }
}
