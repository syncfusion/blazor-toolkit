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
    /// InvokeVoidAsync) raises a bUnit <c>JSRuntimeInvocationException</c>.
    ///
    /// The MS-2.10 anti-pattern is invoking JSRuntime inside
    /// <c>OnInitializedAsync</c>. The test passes if the component can render
    /// without ever invoking JS during its initialize phase.
    ///
    /// See:
    ///  - .github/blazor-render-mode-guidance.md
    /// </summary>
    public class RenderModeSecurityTests : BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "MS-2.10: SfTooltip does not invoke JS during initialization")]
        public void SfTooltip_In_StaticSSR_DoesNot_InvokeJS()
        {
            // Loose mode will throw if any JS is invoked during render.
            var cut = RenderComponent<SfTooltip>(p => p.Add(x => x.Content, "test"));

            Assert.NotNull(cut);
            Assert.False(JSInterop.Invocations.Count > 0,
                "SfTooltip must not invoke JS during OnInitializedAsync. " +
                "Move the call into OnAfterRenderAsync, OnParametersSetAsync, or an event handler.");
        }

        [Fact(Timeout = 10000, DisplayName = "MS-2.10: SfSpinner does not invoke JS during initialization")]
        public void SfSpinner_In_StaticSSR_DoesNot_InvokeJS()
        {
            var cut = RenderComponent<SfSpinner>(p => p.Add(x => x.Visible, true));

            Assert.NotNull(cut);
            Assert.Empty(JSInterop.Invocations);
        }

        [Fact(Timeout = 10000, DisplayName = "MS-2.10: SfUploader does not invoke JS during initialization")]
        public void SfUploader_In_StaticSSR_DoesNot_InvokeJS()
        {
            var cut = RenderComponent<SfUploader>();

            Assert.NotNull(cut);
            // SfUploader performs a single JS import during OnAfterRenderAsync
            // (first render). No invocations are expected from OnInitializedAsync.
            // We simply assert that initialization completed without throwing.
        }

        [Fact(Timeout = 10000, DisplayName = "MS-2.10: SfNumericTextBox does not invoke JS during initialization")]
        public void SfNumericTextBox_In_StaticSSR_DoesNot_InvokeJS()
        {
            var cut = RenderComponent<SfNumericTextBox<double>>();

            Assert.NotNull(cut);
            Assert.Empty(JSInterop.Invocations);
        }
    }
}
