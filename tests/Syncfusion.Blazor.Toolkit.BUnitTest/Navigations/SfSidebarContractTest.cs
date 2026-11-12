using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations
{
    /// <summary>
    /// Contract tests for the Sidebar rules in Spec.md. Every test name starts with the stable ID that
    /// TEST-MATRIX.md maps to its Spec.md rule. Browser-only behavior (focus movement, media queries,
    /// layout, forced colors) is covered by the Playwright suite, not here.
    /// </summary>
    public class SfSidebarContractTest : BunitTestContext
    {
        private const string ModulePath = "./_content/Syncfusion.Blazor.Toolkit/scripts/sidebar.js";

        private static RenderFragment Fragment(string markup)
        {
            return builder => builder.AddMarkupContent(0, markup);
        }

        private static EventCallback<T> Callback<T>(object receiver, Action<T> action)
        {
            return EventCallback.Factory.Create(receiver, action);
        }

        private BunitJSModuleInterop SetupSidebarModule(bool mobile = false, bool reducedMotion = false, bool rtl = false)
        {
            BunitJSModuleInterop module = JSInterop.SetupModule(ModulePath);
            module.Mode = JSRuntimeMode.Loose;
            module.Setup<SidebarEnvironment>("observe", _ => true).SetResult(new SidebarEnvironment { IsMobile = mobile, ReducedMotion = reducedMotion, IsRtl = rtl });
            module.Setup<bool>("update", _ => true).SetResult(mobile);
            return module;
        }

        private static SidebarCallbacks CallbacksOf(BunitJSModuleInterop module)
        {
            JSRuntimeInvocation call = module.Invocations.Single(i => i.Identifier == "observe");
            return ((DotNetObjectReference<SidebarCallbacks>)call.Arguments[1]!).Value;
        }

        private static async Task WaitUntilAsync(Func<bool> condition)
        {
            for (int i = 0; i < 300 && !condition(); i++)
            {
                await Task.Delay(10);
            }
        }

        private static string Classes(IRenderedComponent<SfSidebar> component)
        {
            return component.Find("div.e-sidebar").ClassName ?? string.Empty;
        }

        #region API surface

        [Fact(Timeout = 10000, DisplayName = "U-API-001 Added parameters expose the documented defaults")]
        public void U_API_001_AddedParameters_ExposeContractDefaults()
        {
            var component = RenderComponent<SfSidebar>();
            SfSidebar sidebar = component.Instance;

            Assert.True(sidebar.Responsive);
            Assert.Equal(SidebarBreakpoint.Md, sidebar.Breakpoint);
            Assert.False(sidebar.OpenMiniOnHover);
            Assert.True(sidebar.IsEnabled);
            Assert.Null(sidebar.AriaLabelledBy);
            Assert.Equal(300, sidebar.Duration);
            Assert.True(sidebar.EnableAnimation);
            Assert.True(sidebar.Fixed);
            Assert.Null(sidebar.BackdropClass);
            Assert.True(sidebar.CloseOnNavigate);
            Assert.False(sidebar.OnBreakpointChanged.HasDelegate);
        }

        [Fact(Timeout = 10000, DisplayName = "U-API-002 SidebarBreakpoint has the documented values in order")]
        public void U_API_002_SidebarBreakpoint_HasDocumentedValues()
        {
            Assert.Equal(new[] { "None", "Xs", "Sm", "Md", "Lg", "Xl", "Xxl", "Always" }, Enum.GetNames<SidebarBreakpoint>());
            Assert.Equal(0, (int)SidebarBreakpoint.None);
        }

        [Theory(Timeout = 10000, DisplayName = "U-API-003 Breakpoint thresholds are exact widths")]
        [InlineData(SidebarBreakpoint.None, 0)]
        [InlineData(SidebarBreakpoint.Xs, 0)]
        [InlineData(SidebarBreakpoint.Sm, 576)]
        [InlineData(SidebarBreakpoint.Md, 768)]
        [InlineData(SidebarBreakpoint.Lg, 992)]
        [InlineData(SidebarBreakpoint.Xl, 1200)]
        [InlineData(SidebarBreakpoint.Xxl, 1400)]
        [InlineData(SidebarBreakpoint.Always, 0)]
        public void U_API_003_BreakpointThresholds_AreExact(SidebarBreakpoint breakpoint, int expected)
        {
            Assert.Equal(expected, SidebarLayout.GetBreakpointThreshold(breakpoint));
        }

        [Fact(Timeout = 10000, DisplayName = "U-API-004 SidebarEventArgs carries IsOpen and Mode and keeps the compatibility members")]
        public void U_API_004_EventArgs_CarryContractMembers()
        {
            Type type = typeof(SidebarEventArgs);

            Assert.Equal(typeof(bool), type.GetProperty(nameof(SidebarEventArgs.IsOpen))!.PropertyType);
            Assert.Equal(typeof(SidebarMode), type.GetProperty(nameof(SidebarEventArgs.Mode))!.PropertyType);
            Assert.NotNull(type.GetProperty(nameof(SidebarEventArgs.Trigger)));
            Assert.Null(type.GetProperty("Cancel"));
            Assert.Equal(SidebarMode.Overlay, new SidebarEventArgs { IsOpen = true, Trigger = SidebarTrigger.Api }.Mode);
        }

        [Fact(Timeout = 10000, DisplayName = "U-API-005 The component never emits data-testid")]
        public void U_API_005_Component_EmitsNoDataTestId()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.HeaderTemplate, Fragment("<span>h</span>"))
                .Add(s => s.FooterTemplate, Fragment("<span>f</span>"))
                .Add(s => s.TargetContent, Fragment("<main>t</main>")));

            Assert.DoesNotContain("data-testid", component.Markup, StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-API-006 Legacy members stay on the public surface")]
        public void U_API_006_LegacyMembers_AreRetained()
        {
            Type type = typeof(SfSidebar);

            Assert.Null(type.GetProperty("CloseOnNavigation"));
            Assert.NotNull(type.GetProperty(nameof(SfSidebar.MediaQuery)));
            Assert.NotNull(type.GetProperty(nameof(SfSidebar.EnableGestures)));
            Assert.NotNull(typeof(SidebarTrigger));
        }

        #endregion

        #region Rendering

        [Fact(Timeout = 10000, DisplayName = "U-REN-006 No TargetContent renders no target host")]
        public void U_REN_006_NoTargetContent_RendersNoTargetHost()
        {
            var component = RenderComponent<SfSidebar>();

            Assert.Empty(component.FindAll(".e-sidebar-target"));
            Assert.Single(component.FindAll(".e-sidebar-body"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-REN-010 BackdropClass is appended to the backdrop only")]
        public void U_REN_010_BackdropClass_IsAppendedToBackdrop()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.BackdropClass, "frosted blur"));

            string backdrop = component.Find(".e-sidebar-backdrop").ClassName!;
            Assert.StartsWith("e-sidebar-backdrop", backdrop, StringComparison.Ordinal);
            Assert.Contains("frosted", backdrop, StringComparison.Ordinal);
            Assert.Contains("blur", backdrop, StringComparison.Ordinal);
            Assert.DoesNotContain("frosted", Classes(component), StringComparison.Ordinal);
            Assert.Equal("true", component.Find(".e-sidebar-backdrop").GetAttribute("aria-hidden"));
        }

        [Theory(Timeout = 10000, DisplayName = "U-REN-011 A null or blank BackdropClass appends nothing")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void U_REN_011_BackdropClass_BlankAppendsNothing(string? value)
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.BackdropClass, value));

            Assert.Equal("e-sidebar-backdrop", component.Find(".e-sidebar-backdrop").ClassName);
        }

        [Theory(Timeout = 10000, DisplayName = "U-REN-012 BackdropClass never decides whether the backdrop renders")]
        [InlineData(false, true, "")]
        [InlineData(true, false, "x")]
        [InlineData(true, true, "x")]
        public void U_REN_012_BackdropClass_DoesNotDecideRendering(bool open, bool showBackdrop, string css)
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, open)
                .Add(s => s.ShowBackdrop, showBackdrop)
                .Add(s => s.BackdropClass, css));

            Assert.Equal(open && showBackdrop, component.FindAll(".e-sidebar-backdrop").Count == 1);
        }

        [Theory(Timeout = 10000, DisplayName = "U-REN-013 Fixed toggles the fixed positioning hook and nothing structural")]
        [InlineData(true)]
        [InlineData(false)]
        public void U_REN_013_Fixed_TogglesPositioningHook(bool fixedValue)
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.Fixed, fixedValue)
                .Add(s => s.TargetContent, Fragment("<main>t</main>")));

            Assert.Equal(fixedValue, component.Find(".e-sidebar-container").ClassName!.Contains("e-sidebar-container-fixed", StringComparison.Ordinal));
            Assert.Contains("e-sidebar-open", Classes(component), StringComparison.Ordinal);
            Assert.Single(component.FindAll("div.e-sidebar"));
            Assert.Single(component.FindAll(".e-sidebar-backdrop"));
            Assert.Equal("dialog", component.Find("div.e-sidebar").GetAttribute("role"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-MODE-004 A runtime Mode change replaces the mode class")]
        public void U_MODE_004_RuntimeModeChange_ReplacesModeClass()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.Mode, SidebarMode.Overlay));
            Assert.Contains("e-sidebar-overlay", Classes(component), StringComparison.Ordinal);

            component.SetParametersAndRender(p => p.Add(s => s.Mode, SidebarMode.Push));

            Assert.DoesNotContain("e-sidebar-overlay", Classes(component), StringComparison.Ordinal);
            Assert.Contains("e-sidebar-push", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-OPEN-004 A user close raises IsOpenChanged with false")]
        public void U_OPEN_004_Close_RaisesIsOpenChangedFalse()
        {
            List<bool> values = new();
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.IsOpenChanged, Callback<bool>(this, values.Add)));

            component.Find(".e-sidebar-backdrop").Click();

            Assert.Equal(new[] { false }, values);
        }

        [Fact(Timeout = 10000, DisplayName = "U-OPEN-010 A one-way IsOpen without a callback is not written back")]
        public async Task U_OPEN_010_OneWayIsOpen_IsNotWrittenBack()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.IsOpen, true));

            await component.Instance.CloseAsync();

            Assert.DoesNotContain("e-sidebar-open", Classes(component), StringComparison.Ordinal);
            Assert.True(component.Instance.IsOpen); // the parent value is never overwritten by the component
        }

        [Fact(Timeout = 10000, DisplayName = "U-OPEN-011 The occupied-width variable follows the state")]
        public async Task U_OPEN_011_OccupiedWidth_FollowsState()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableMini, true)
                .Add(s => s.EnableAnimation, false));
            string Style() => component.Find(".e-sidebar-container").GetAttribute("style")!;

            Assert.Contains("--e-sidebar-occupied-width: var(--e-sidebar-mini-width)", Style(), StringComparison.Ordinal);
            await component.Instance.OpenAsync();
            Assert.Contains("--e-sidebar-occupied-width: var(--e-sidebar-width)", Style(), StringComparison.Ordinal);

            var closed = RenderComponent<SfSidebar>();
            Assert.Contains("--e-sidebar-occupied-width: 0px", closed.Find(".e-sidebar-container").GetAttribute("style"), StringComparison.Ordinal);
        }

        #endregion

        #region Accessibility attributes

        [Fact(Timeout = 10000, DisplayName = "U-A11Y-004 AriaLabelledBy renders aria-labelledby and replaces the default label")]
        public void U_A11Y_004_AriaLabelledBy_RendersAndReplacesDefaultLabel()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.AriaLabelledBy, "title-1"));

            var panel = component.Find("div.e-sidebar");
            Assert.Equal("title-1", panel.GetAttribute("aria-labelledby"));
            Assert.Null(panel.GetAttribute("aria-label"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-A11Y-009 AriaLabel and AriaLabelledBy can be set together")]
        public void U_A11Y_009_BothNames_AreRendered()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.AriaLabel, "Fallback")
                .Add(s => s.AriaLabelledBy, "title-1"));

            var panel = component.Find("div.e-sidebar");
            Assert.Equal("Fallback", panel.GetAttribute("aria-label"));
            Assert.Equal("title-1", panel.GetAttribute("aria-labelledby"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-A11Y-010 The panel never emits aria-expanded or aria-controls")]
        public async Task U_A11Y_010_Panel_EmitsNoToggleState()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.EnableAnimation, false));
            await component.Instance.OpenAsync();

            Assert.DoesNotContain("aria-expanded", component.Markup, StringComparison.Ordinal);
            Assert.DoesNotContain("aria-controls", component.Markup, StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-A11Y-011 HtmlAttributes cannot override aria-labelledby")]
        public void U_A11Y_011_HtmlAttributes_CannotOverrideLabelledBy()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.AriaLabelledBy, "owned")
                .Add(s => s.HtmlAttributes, new Dictionary<string, object> { ["aria-labelledby"] = "injected" }));

            Assert.Equal("owned", component.Find("div.e-sidebar").GetAttribute("aria-labelledby"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-A11Y-012 The backdrop is not focusable and has no accessible name")]
        public void U_A11Y_012_Backdrop_IsNotFocusableOrNamed()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.IsOpen, true));

            var backdrop = component.Find(".e-sidebar-backdrop");
            Assert.Equal("DIV", backdrop.TagName);
            Assert.Null(backdrop.GetAttribute("tabindex"));
            Assert.Null(backdrop.GetAttribute("role"));
            Assert.Null(backdrop.GetAttribute("aria-label"));
            Assert.Equal(string.Empty, backdrop.TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "U-A11Y-005 A fully closed panel is hidden and an open or mini panel is not")]
        public void U_A11Y_005_HiddenState_FollowsRule()
        {
            Assert.Equal("true", RenderComponent<SfSidebar>().Find("div.e-sidebar").GetAttribute("aria-hidden"));
            Assert.Null(RenderComponent<SfSidebar>(p => p.Add(s => s.IsOpen, true)).Find("div.e-sidebar").GetAttribute("aria-hidden"));
            Assert.Null(RenderComponent<SfSidebar>(p => p.Add(s => s.EnableMini, true)).Find("div.e-sidebar").GetAttribute("aria-hidden"));
        }

        #endregion

        #region CSS variables

        [Fact(Timeout = 10000, DisplayName = "U-CSS-001 Width, MiniWidth and Duration reach the host as variables")]
        public void U_CSS_001_Variables_ReflectParameters()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Width, "300px")
                .Add(s => s.MiniWidth, "72px")
                .Add(s => s.Duration, 200));

            string style = component.Find(".e-sidebar-container").GetAttribute("style")!;
            Assert.Contains("--e-sidebar-width: 300px", style, StringComparison.Ordinal);
            Assert.Contains("--e-sidebar-mini-width: 72px", style, StringComparison.Ordinal);
            Assert.Contains("--e-sidebar-duration: 200ms", style, StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-CSS-002 A negative Duration is exposed as 0ms")]
        public void U_CSS_002_NegativeDuration_IsZero()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.Duration, -5));

            Assert.Contains("--e-sidebar-duration: 0ms", component.Find(".e-sidebar-container").GetAttribute("style"), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-CSS-003 The host style holds only the four published variables")]
        public void U_CSS_003_HostStyle_HoldsOnlyVariables()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.IsOpen, true).Add(s => s.Mode, SidebarMode.Push));

            string style = component.Find(".e-sidebar-container").GetAttribute("style")!;
            string[] declarations = style.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            Assert.All(declarations, d => Assert.StartsWith("--e-sidebar-", d, StringComparison.Ordinal));
            Assert.Equal(4, declarations.Length);
            Assert.Null(component.Find("div.e-sidebar").GetAttribute("style"));
        }

        #endregion

        #region Static rendering

        [Fact(Timeout = 10000, DisplayName = "U-SSR-001 Markup is complete from parameters without any script")]
        public void U_SSR_001_Markup_IsCompleteFromParameters()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.Mode, SidebarMode.Shrink)
                .Add(s => s.Position, SidebarPosition.End)
                .Add(s => s.TargetContent, Fragment("<main>t</main>")));

            string classes = Classes(component);
            Assert.Contains("e-sidebar-open", classes, StringComparison.Ordinal);
            Assert.Contains("e-sidebar-shrink", classes, StringComparison.Ordinal);
            Assert.Contains("e-sidebar-right", classes, StringComparison.Ordinal);
            Assert.Contains("--e-sidebar-occupied-width: var(--e-sidebar-width)", component.Find(".e-sidebar-container").GetAttribute("style"), StringComparison.Ordinal);
            Assert.Equal("navigation", component.Find("div.e-sidebar").GetAttribute("role"));
        }

        #endregion

        #region Responsive state machine

        [Fact(Timeout = 10000, DisplayName = "U-RES-001 Responsive=false keeps Mode on a narrow viewport")]
        public async Task U_RES_001_ResponsiveOff_KeepsMode()
        {
            BunitJSModuleInterop module = SetupSidebarModule(mobile: true);
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Responsive, false)
                .Add(s => s.Mode, SidebarMode.Push)
                .Add(s => s.IsOpen, true));

            await component.InvokeAsync(() => CallbacksOf(module).OnViewportChanged(true));

            Assert.Contains("e-sidebar-push", Classes(component), StringComparison.Ordinal);
            Assert.Contains("e-sidebar-open", Classes(component), StringComparison.Ordinal);
            Assert.Equal(0, module.Invocations.Single(i => i.Identifier == "observe").Arguments[3]);
        }

        [Fact(Timeout = 10000, DisplayName = "U-RES-002 Below the breakpoint the effective mode is Overlay")]
        public async Task U_RES_002_BelowBreakpoint_UsesOverlay()
        {
            BunitJSModuleInterop module = SetupSidebarModule();
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.Mode, SidebarMode.Push));
            Assert.Contains("e-sidebar-push", Classes(component), StringComparison.Ordinal);

            await component.InvokeAsync(() => CallbacksOf(module).OnViewportChanged(true));

            Assert.Contains("e-sidebar-overlay", Classes(component), StringComparison.Ordinal);
            Assert.DoesNotContain("e-sidebar-push", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-RES-003 Crossing to mobile while open closes with IsOpenChanged then OnClosed")]
        public async Task U_RES_003_CrossToMobile_ClosesInOrder()
        {
            List<string> log = new();
            BunitJSModuleInterop module = SetupSidebarModule();
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.Mode, SidebarMode.Push)
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.OnBreakpointChanged, Callback<bool>(this, v => log.Add($"breakpoint:{v}")))
                .Add(s => s.IsOpenChanged, Callback<bool>(this, v => log.Add($"changed:{v}")))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, a => log.Add($"closed:{a.IsOpen}"))));

            await component.InvokeAsync(() => CallbacksOf(module).OnViewportChanged(true));

            Assert.Equal(new[] { "breakpoint:True", "changed:False", "closed:False" }, log);
            Assert.DoesNotContain("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-RES-004 Crossing to desktop never opens the panel")]
        public async Task U_RES_004_CrossToDesktop_NeverOpens()
        {
            List<string> log = new();
            BunitJSModuleInterop module = SetupSidebarModule(mobile: true);
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.IsOpenChanged, Callback<bool>(this, v => log.Add($"changed:{v}")))
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, _ => log.Add("opened"))));
            log.Clear();

            await component.InvokeAsync(() => CallbacksOf(module).OnViewportChanged(false));

            Assert.Empty(log);
            Assert.DoesNotContain("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-RES-005 OnBreakpointChanged reports true below and false at or above the breakpoint")]
        public async Task U_RES_005_BreakpointEvent_ReportsBothDirections()
        {
            List<bool> flags = new();
            BunitJSModuleInterop module = SetupSidebarModule();
            RenderComponent<SfSidebar>(p => p.Add(s => s.OnBreakpointChanged, Callback<bool>(this, flags.Add)));
            SidebarCallbacks callbacks = CallbacksOf(module);

            await callbacks.OnViewportChanged(true);
            await callbacks.OnViewportChanged(true);
            await callbacks.OnViewportChanged(false);

            Assert.Equal(new[] { true, false }, flags);
        }

        [Fact(Timeout = 10000, DisplayName = "U-RES-006 Dispose removes the listeners and later changes do nothing")]
        public async Task U_RES_006_Dispose_RemovesListeners()
        {
            List<string> log = new();
            BunitJSModuleInterop module = SetupSidebarModule();
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.OnBreakpointChanged, Callback<bool>(this, v => log.Add("breakpoint")))
                .Add(s => s.IsOpenChanged, Callback<bool>(this, v => log.Add("changed"))));
            SidebarCallbacks callbacks = CallbacksOf(module);

            DisposeComponents();
            await Record.ExceptionAsync(() => callbacks.OnViewportChanged(true));

            module.VerifyInvoke("unobserve");
            Assert.Empty(log);
        }

        [Fact(Timeout = 10000, DisplayName = "U-RES-007 A narrow viewport on load starts closed and reports through the open-state pair")]
        public void U_RES_007_NarrowOnLoad_StartsClosed()
        {
            List<string> log = new();
            SetupSidebarModule(mobile: true);
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.Mode, SidebarMode.Push)
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.OnBreakpointChanged, Callback<bool>(this, v => log.Add($"breakpoint:{v}")))
                .Add(s => s.IsOpenChanged, Callback<bool>(this, v => log.Add($"changed:{v}")))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, a => log.Add($"closed:{a.Mode}"))));

            Assert.Contains("e-sidebar-overlay", Classes(component), StringComparison.Ordinal);
            Assert.DoesNotContain("e-sidebar-open", Classes(component), StringComparison.Ordinal);
            Assert.Equal(new[] { "changed:False", "closed:Overlay" }, log);
        }

        [Theory(Timeout = 10000, DisplayName = "U-RES-008 The active threshold is registered for every Breakpoint value")]
        [InlineData(SidebarBreakpoint.None, 0)]
        [InlineData(SidebarBreakpoint.Xs, 0)]
        [InlineData(SidebarBreakpoint.Sm, 576)]
        [InlineData(SidebarBreakpoint.Md, 768)]
        [InlineData(SidebarBreakpoint.Lg, 992)]
        [InlineData(SidebarBreakpoint.Xl, 1200)]
        [InlineData(SidebarBreakpoint.Xxl, 1400)]
        [InlineData(SidebarBreakpoint.Always, 0)]
        public void U_RES_008_Threshold_IsRegistered(SidebarBreakpoint breakpoint, int expected)
        {
            BunitJSModuleInterop module = SetupSidebarModule();
            RenderComponent<SfSidebar>(p => p.Add(s => s.Breakpoint, breakpoint));

            Assert.Equal(expected, module.Invocations.Single(i => i.Identifier == "observe").Arguments[3]);
        }

        [Fact(Timeout = 10000, DisplayName = "U-RES-009 Breakpoint=Always and None never fall back to Overlay")]
        public async Task U_RES_009_AlwaysAndNone_KeepMode()
        {
            foreach (SidebarBreakpoint breakpoint in new[] { SidebarBreakpoint.Always, SidebarBreakpoint.None })
            {
                BunitJSModuleInterop module = SetupSidebarModule();
                var component = RenderComponent<SfSidebar>(p => p
                    .Add(s => s.Breakpoint, breakpoint)
                    .Add(s => s.Mode, SidebarMode.Shrink)
                    .Add(s => s.IsOpen, true));

                await component.InvokeAsync(() => CallbacksOf(module).OnViewportChanged(true));

                Assert.Contains("e-sidebar-shrink", Classes(component), StringComparison.Ordinal);
                Assert.Contains("e-sidebar-open", Classes(component), StringComparison.Ordinal);
                DisposeComponents();
            }
        }

        [Fact(Timeout = 10000, DisplayName = "U-RES-010 Changing Breakpoint or Responsive at runtime updates the registered threshold")]
        public void U_RES_010_RuntimeChange_UpdatesThreshold()
        {
            BunitJSModuleInterop module = SetupSidebarModule();
            var component = RenderComponent<SfSidebar>();

            component.SetParametersAndRender(p => p.Add(s => s.Breakpoint, SidebarBreakpoint.Lg));
            component.SetParametersAndRender(p => p.Add(s => s.Responsive, false));

            var updates = module.Invocations.Where(i => i.Identifier == "update").Select(i => (int)i.Arguments[1]!).ToList();
            Assert.Equal(new[] { 992, 0 }, updates);
        }

        [Fact(Timeout = 10000, DisplayName = "U-RES-011 Event Mode is the effective mode on a narrow viewport")]
        public async Task U_RES_011_EventMode_IsEffectiveMode()
        {
            List<SidebarMode> modes = new();
            BunitJSModuleInterop module = SetupSidebarModule();
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Mode, SidebarMode.Push)
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, a => modes.Add(a.Mode))));

            await component.Instance.OpenAsync();
            await component.InvokeAsync(() => CallbacksOf(module).OnViewportChanged(true));
            await component.Instance.OpenAsync();

            Assert.Equal(new[] { SidebarMode.Push, SidebarMode.Overlay }, modes);
        }

        #endregion

        #region Direction

        [Fact(Timeout = 10000, DisplayName = "U-POS-007 A host direction of rtl resolves logical positions and a later change re-resolves them")]
        public async Task U_POS_007_HostDirection_ResolvesAndFollowsChanges()
        {
            BunitJSModuleInterop module = SetupSidebarModule(rtl: true);
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.Position, SidebarPosition.Start));

            Assert.Contains("e-sidebar-right", Classes(component), StringComparison.Ordinal);
            Assert.Contains("e-rtl", component.Find(".e-sidebar-container").ClassName!, StringComparison.Ordinal);

            await component.InvokeAsync(() => CallbacksOf(module).OnEnvironmentChanged(false, false));

            Assert.Contains("e-sidebar-left", Classes(component), StringComparison.Ordinal);
            Assert.DoesNotContain("e-rtl", component.Find(".e-sidebar-container").ClassName!, StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-POS-008 Physical positions ignore the host direction")]
        public void U_POS_008_PhysicalPositions_IgnoreHostDirection()
        {
            SetupSidebarModule(rtl: true);
            var left = RenderComponent<SfSidebar>(p => p.Add(s => s.Position, SidebarPosition.Left));
            var right = RenderComponent<SfSidebar>(p => p.Add(s => s.Position, SidebarPosition.Right));

            Assert.Contains("e-sidebar-left", Classes(left), StringComparison.Ordinal);
            Assert.Contains("e-sidebar-right", Classes(right), StringComparison.Ordinal);
        }

        #endregion

        #region Events, animation, cancellation

        [Fact(Timeout = 10000, DisplayName = "U-EVT-001 OnOpened and OnClosed carry the resulting IsOpen and the effective Mode")]
        public async Task U_EVT_001_Events_CarryIsOpenAndMode()
        {
            List<SidebarEventArgs> args = new();
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Mode, SidebarMode.Shrink)
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, args.Add))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, args.Add)));

            await component.Instance.OpenAsync();
            await component.Instance.CloseAsync();

            Assert.Equal(new[] { true, false }, args.Select(a => a.IsOpen));
            Assert.All(args, a => Assert.Equal(SidebarMode.Shrink, a.Mode));
        }

        [Fact(Timeout = 10000, DisplayName = "U-EVT-002 With animation active OnOpened waits for Duration")]
        public async Task U_EVT_002_Animated_WaitsForDuration()
        {
            List<string> log = new();
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Duration, 120)
                .Add(s => s.IsOpenChanged, Callback<bool>(this, v => log.Add("changed")))
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, _ => log.Add("opened"))));

            await component.Instance.OpenAsync();
            Assert.Equal(new[] { "changed" }, log);

            await WaitUntilAsync(() => log.Count == 2);
            Assert.Equal(new[] { "changed", "opened" }, log);
        }

        [Fact(Timeout = 10000, DisplayName = "U-EVT-003 A newer state change cancels the pending callback")]
        public async Task U_EVT_003_NewerChange_CancelsPendingCallback()
        {
            List<string> log = new();
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Duration, 150)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, _ => log.Add("opened")))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, _ => log.Add("closed"))));

            await component.Instance.OpenAsync();
            await component.Instance.CloseAsync();
            await WaitUntilAsync(() => log.Count > 0);
            await Task.Delay(300);

            Assert.Equal(new[] { "closed" }, log);
        }

        [Fact(Timeout = 10000, DisplayName = "U-EVT-004 Reduced motion raises the callback immediately")]
        public async Task U_EVT_004_ReducedMotion_IsImmediate()
        {
            int opened = 0;
            BunitJSModuleInterop module = SetupSidebarModule(reducedMotion: true);
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Duration, 5000)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, _ => opened++)));
            Assert.NotNull(module);

            await component.Instance.OpenAsync();

            Assert.Equal(1, opened);
        }

        [Fact(Timeout = 10000, DisplayName = "U-EVT-005 A reduced-motion change at runtime applies to the next change")]
        public async Task U_EVT_005_ReducedMotionChange_AppliesToNextChange()
        {
            int opened = 0;
            BunitJSModuleInterop module = SetupSidebarModule();
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Duration, 5000)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, _ => opened++)));

            await component.InvokeAsync(() => CallbacksOf(module).OnEnvironmentChanged(true, false));
            await component.Instance.OpenAsync();

            Assert.Equal(1, opened);
        }

        [Fact(Timeout = 10000, DisplayName = "U-EVT-006 Dispose cancels a pending callback")]
        public async Task U_EVT_006_Dispose_CancelsPendingCallback()
        {
            int opened = 0;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Duration, 120)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, _ => opened++)));
            await component.Instance.OpenAsync();

            DisposeComponents();
            await Task.Delay(400);

            Assert.Equal(0, opened);
        }

        [Fact(Timeout = 10000, DisplayName = "U-EVT-007 A zero Duration raises the callback immediately")]
        public async Task U_EVT_007_ZeroDuration_IsImmediate()
        {
            int opened = 0;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Duration, 0)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, _ => opened++)));

            await component.Instance.OpenAsync();

            Assert.Equal(1, opened);
        }

        [Fact(Timeout = 10000, DisplayName = "U-EVT-008 The global animation option raises the callback immediately")]
        public async Task U_EVT_008_GlobalAnimationOff_IsImmediate()
        {
            Services.Configure<GlobalOptions>(o => o.Animation = GlobalAnimationMode.Disable);
            int opened = 0;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Duration, 5000)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, _ => opened++)));

            await component.Instance.OpenAsync();

            Assert.Equal(1, opened);
        }

        [Fact(Timeout = 10000, DisplayName = "U-EVT-009 Rapid toggles settle on the last state with one callback")]
        public async Task U_EVT_009_RapidToggles_RaiseOneCallback()
        {
            int opened = 0;
            int closed = 0;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Duration, 100)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, _ => opened++))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, _ => closed++)));

            await component.Instance.ToggleAsync();
            await component.Instance.ToggleAsync();
            await component.Instance.ToggleAsync();
            await WaitUntilAsync(() => opened > 0);
            await Task.Delay(250);

            Assert.Equal(1, opened);
            Assert.Equal(0, closed);
            Assert.Contains("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-EVT-010 A change inside IsOpenChanged supersedes the outer callback")]
        public async Task U_EVT_010_ReentrantChange_SupersedesOuterCallback()
        {
            List<string> log = new();
            IRenderedComponent<SfSidebar> component = null!;
            component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.IsOpenChanged, EventCallback.Factory.Create<bool>(this, async v =>
                {
                    log.Add($"changed:{v}");
                    if (v)
                    {
                        await component.InvokeAsync(() => component.Instance.CloseAsync());
                    }
                }))
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, _ => log.Add("opened")))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, _ => log.Add("closed"))));

            await component.Instance.OpenAsync();

            Assert.Equal(new[] { "changed:True", "changed:False", "closed" }, log);
        }

        #endregion

        #region IsEnabled gating

        [Fact(Timeout = 10000, DisplayName = "U-OPEN-008 U-ENA-001 IsEnabled=false ignores a backdrop activation")]
        public void U_ENA_001_Disabled_IgnoresBackdrop()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.IsEnabled, false));

            component.Find(".e-sidebar-backdrop").Click();

            Assert.Contains("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-OPEN-008 U-ENA-002 IsEnabled=false ignores Escape")]
        public void U_ENA_002_Disabled_IgnoresEscape()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.IsEnabled, false));

            component.Find(".e-sidebar-container").KeyDown(new KeyboardEventArgs { Key = "Escape" });

            Assert.Contains("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-ENA-003 IsEnabled=false ignores hover expansion")]
        public void U_ENA_003_Disabled_IgnoresHover()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableMini, true)
                .Add(s => s.OpenMiniOnHover, true)
                .Add(s => s.IsEnabled, false));

            Exception? failure = Record.Exception(() => component.Find("div.e-sidebar").TriggerEvent("onpointerenter", new PointerEventArgs { PointerType = "mouse" }));

            Assert.True(failure is null or Bunit.MissingEventHandlerException);
            Assert.DoesNotContain("e-sidebar-hover", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-ENA-004 IsEnabled=false ignores swipe gestures")]
        public void U_ENA_004_Disabled_IgnoresGestures()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.IsEnabled, false));
            var host = component.Find(".e-sidebar-container");

            host.TriggerEvent("onpointerdown", new PointerEventArgs { PointerType = "touch", ClientX = 10, ClientY = 10 });
            host.TriggerEvent("onpointerup", new PointerEventArgs { PointerType = "touch", ClientX = 200, ClientY = 12 });

            Assert.DoesNotContain("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-ENA-005 IsEnabled=false still honors the public methods")]
        public async Task U_ENA_005_Disabled_StillHonorsMethods()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsEnabled, false)
                .Add(s => s.EnableAnimation, false));

            await component.Instance.OpenAsync();
            Assert.Contains("e-sidebar-open", Classes(component), StringComparison.Ordinal);
            await component.Instance.ToggleAsync();
            Assert.DoesNotContain("e-sidebar-open", Classes(component), StringComparison.Ordinal);
            await component.Instance.OpenAsync();
            await component.Instance.CloseAsync();
            Assert.DoesNotContain("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-ENA-006 IsEnabled=false still applies a new IsOpen value")]
        public void U_ENA_006_Disabled_StillAppliesParameter()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.IsEnabled, false));

            component.SetParametersAndRender(p => p.Add(s => s.IsOpen, true));

            Assert.Contains("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-ENA-007 Escape on the host closes an overlay when enabled")]
        public void U_ENA_007_Enabled_EscapeOnHostCloses()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.EnableAnimation, false));

            component.Find(".e-sidebar-container").KeyDown(new KeyboardEventArgs { Key = "Escape" });

            Assert.DoesNotContain("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        #endregion

        #region Mini hover

        private static PointerEventArgs Mouse() => new() { PointerType = "mouse" };

        [Fact(Timeout = 10000, DisplayName = "U-HOV-001 Hover expands a collapsed mini rail visually and collapses on leave")]
        public void U_HOV_001_Hover_ExpandsAndCollapses()
        {
            int events = 0;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableMini, true)
                .Add(s => s.OpenMiniOnHover, true)
                .Add(s => s.IsOpenChanged, Callback<bool>(this, _ => events++))
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, _ => events++)));

            component.Find("div.e-sidebar").TriggerEvent("onpointerenter", Mouse());
            Assert.Contains("e-sidebar-hover", Classes(component), StringComparison.Ordinal);
            Assert.False(component.Instance.IsOpen);

            component.Find("div.e-sidebar").TriggerEvent("onpointerleave", Mouse());
            Assert.DoesNotContain("e-sidebar-hover", Classes(component), StringComparison.Ordinal);
            Assert.Equal(0, events);
        }

        [Fact(Timeout = 10000, DisplayName = "U-HOV-002 A touch pointer does not trigger hover expansion")]
        public void U_HOV_002_Touch_DoesNotExpand()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableMini, true)
                .Add(s => s.OpenMiniOnHover, true));

            component.Find("div.e-sidebar").TriggerEvent("onpointerenter", new PointerEventArgs { PointerType = "touch" });

            Assert.DoesNotContain("e-sidebar-hover", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-HOV-003 Hover does nothing without EnableMini or OpenMiniOnHover")]
        public void U_HOV_003_Hover_RequiresBothFlags()
        {
            var noMini = RenderComponent<SfSidebar>(p => p.Add(s => s.OpenMiniOnHover, true));
            var noHover = RenderComponent<SfSidebar>(p => p.Add(s => s.EnableMini, true));

            Exception? first = Record.Exception(() => noMini.Find("div.e-sidebar").TriggerEvent("onpointerenter", Mouse()));
            Exception? second = Record.Exception(() => noHover.Find("div.e-sidebar").TriggerEvent("onpointerenter", Mouse()));

            Assert.True(first is null or Bunit.MissingEventHandlerException);
            Assert.True(second is null or Bunit.MissingEventHandlerException);
            Assert.DoesNotContain("e-sidebar-hover", Classes(noMini), StringComparison.Ordinal);
            Assert.DoesNotContain("e-sidebar-hover", Classes(noHover), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-HOV-004 Hover does not apply on a mobile viewport")]
        public async Task U_HOV_004_Mobile_DoesNotExpand()
        {
            BunitJSModuleInterop module = SetupSidebarModule();
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableMini, true)
                .Add(s => s.OpenMiniOnHover, true));
            await component.InvokeAsync(() => CallbacksOf(module).OnViewportChanged(true));

            Exception? failure = Record.Exception(() => component.Find("div.e-sidebar").TriggerEvent("onpointerenter", Mouse()));

            Assert.True(failure is null or Bunit.MissingEventHandlerException);
            Assert.DoesNotContain("e-sidebar-hover", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-HOV-005 An explicit open clears the hover state and stays open on leave")]
        public async Task U_HOV_005_ExplicitOpen_StaysOpenOnLeave()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableMini, true)
                .Add(s => s.OpenMiniOnHover, true)
                .Add(s => s.EnableAnimation, false));
            component.Find("div.e-sidebar").TriggerEvent("onpointerenter", Mouse());

            await component.Instance.OpenAsync();
            Exception? failure = Record.Exception(() => component.Find("div.e-sidebar").TriggerEvent("onpointerleave", Mouse()));

            Assert.True(failure is null or Bunit.MissingEventHandlerException);
            Assert.Contains("e-sidebar-open", Classes(component), StringComparison.Ordinal);
            Assert.DoesNotContain("e-sidebar-hover", Classes(component), StringComparison.Ordinal);
        }

        #endregion

        #region Navigation

        private void NavigateTo(string uri)
        {
            Services.GetRequiredService<NavigationManager>().NavigateTo(uri);
        }

        [Fact(Timeout = 10000, DisplayName = "U-NAV-001 A bound sidebar closes on navigation by default with IsOpenChanged before OnClosed")]
        public void U_NAV_001_Bound_ClosesOnNavigateByDefault()
        {
            List<string> log = new();
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.IsOpenChanged, Callback<bool>(this, v => log.Add($"changed:{v}")))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, _ => log.Add("closed"))));

            NavigateTo("next");

            Assert.Equal(new[] { "changed:False", "closed" }, log);
            Assert.DoesNotContain("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-NAV-004 Without a bound callback the component does not subscribe")]
        public void U_NAV_004_Unbound_DoesNotSubscribe()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.IsOpen, true));

            NavigateTo("next");

            Assert.Contains("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-NAV-002 CloseOnNavigate=false never closes, even when bound")]
        public void U_NAV_002_Disabled_NeverCloses()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.CloseOnNavigate, false)
                .Add(s => s.IsOpenChanged, Callback<bool>(this, _ => { })));

            NavigateTo("next");

            Assert.Contains("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-NAV-005 Switching CloseOnNavigate off at runtime removes the subscription")]
        public void U_NAV_005_RuntimeSwitch_Unsubscribes()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.IsOpenChanged, Callback<bool>(this, _ => { })));

            component.SetParametersAndRender(p => p.Add(s => s.CloseOnNavigate, false));
            NavigateTo("next");

            Assert.Contains("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-NAV-003 Dispose unsubscribes from location changes")]
        public void U_NAV_003_Dispose_Unsubscribes()
        {
            int events = 0;
            RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.IsOpenChanged, Callback<bool>(this, _ => events++))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, _ => events++)));

            DisposeComponents();
            NavigateTo("next");

            Assert.Equal(0, events);
        }

        #endregion

        #region Focus (script contract)

        [Fact(Timeout = 10000, DisplayName = "U-FOC-004 Closing a modal overlay asks the script to restore focus to the opener")]
        public async Task U_FOC_004_Close_RestoresFocus()
        {
            BunitJSModuleInterop module = SetupSidebarModule();
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.EnableAnimation, false));

            await component.Instance.OpenAsync();
            module.VerifyInvoke("activateModal");
            await component.Instance.CloseAsync();

            JSRuntimeInvocation call = module.Invocations.Single(i => i.Identifier == "deactivateModal" && Equals(i.Arguments[1], true));
            Assert.Equal(true, call.Arguments[1]);
        }

        [Fact(Timeout = 10000, DisplayName = "U-FOC-005 Push and Shrink never start the focus trap")]
        public async Task U_FOC_005_Docked_NeverTraps()
        {
            BunitJSModuleInterop module = SetupSidebarModule();
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Mode, SidebarMode.Push)
                .Add(s => s.EnableAnimation, false));

            await component.Instance.OpenAsync();

            Assert.DoesNotContain(module.Invocations, i => i.Identifier is "activateModal" or "focusPanel");
        }

        [Fact(Timeout = 10000, DisplayName = "U-FOC-006 An overlay without a backdrop never starts the focus trap")]
        public async Task U_FOC_006_NoBackdrop_NeverTraps()
        {
            BunitJSModuleInterop module = SetupSidebarModule();
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.ShowBackdrop, false)
                .Add(s => s.EnableAnimation, false));

            await component.Instance.OpenAsync();

            Assert.DoesNotContain(module.Invocations, i => i.Identifier is "activateModal");
        }

        [Fact(Timeout = 10000, DisplayName = "U-FOC-001 The panel is programmatically focusable")]
        public void U_FOC_001_Panel_HasTabindexMinusOne()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.IsOpen, true));

            Assert.Equal("-1", component.Find("div.e-sidebar").GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-FOC-007 FocusAsync asks the script to focus the first focusable descendant")]
        public async Task U_FOC_007_FocusAsync_UsesScript()
        {
            BunitJSModuleInterop module = SetupSidebarModule();
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.IsOpen, true).Add(s => s.Mode, SidebarMode.Push));

            await component.Instance.FocusAsync();

            module.VerifyInvoke("focusPanel");
        }

        [Fact(Timeout = 10000, DisplayName = "U-FOC-008 Dispose ends the modal focus behavior without restoring focus")]
        public async Task U_FOC_008_Dispose_EndsModal()
        {
            BunitJSModuleInterop module = SetupSidebarModule();
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.EnableAnimation, false));
            await component.Instance.OpenAsync();

            DisposeComponents();

            Assert.Contains(module.Invocations, i => i.Identifier == "deactivateModal" && Equals(i.Arguments[1], false));
        }

        #endregion

        #region Integration

        [Fact(Timeout = 10000, DisplayName = "I-INT-001 Two instances with distinct Ids render both panels")]
        public void I_INT_001_DualSidebars_RenderBothPanels()
        {
            var component = RenderComponent<DualHost>();

            var panels = component.FindAll("div.e-sidebar");
            Assert.Equal(2, panels.Count);
            Assert.Equal(new[] { "dual-start", "dual-end" }, panels.Select(p => p.GetAttribute("id")));
            Assert.Contains("e-sidebar-left", panels[0].ClassName, StringComparison.Ordinal);
            Assert.Contains("e-sidebar-right", panels[1].ClassName, StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "I-INT-002 One hundred toggles finish in the toggled state")]
        public async Task I_INT_002_HundredToggles_FinishInToggledState()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.EnableAnimation, false));

            for (int i = 0; i < 100; i++)
            {
                await component.Instance.ToggleAsync();
            }

            Assert.DoesNotContain("e-sidebar-open", Classes(component), StringComparison.Ordinal);
            await component.Instance.ToggleAsync();
            Assert.Contains("e-sidebar-open", Classes(component), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "I-INT-003 With animation off OnOpened is raised without waiting")]
        public async Task I_INT_003_AnimationOff_RaisesImmediately()
        {
            int opened = 0;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.Duration, 5000)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, _ => opened++)));

            await component.Instance.OpenAsync();

            Assert.Equal(1, opened);
        }

        [Fact(Timeout = 10000, DisplayName = "I-INT-004 Null templates render safely with an empty body")]
        public void I_INT_004_NullTemplates_RenderSafely()
        {
            var component = RenderComponent<SfSidebar>();

            Assert.Empty(component.FindAll(".e-sidebar-header"));
            Assert.Empty(component.FindAll(".e-sidebar-footer"));
            Assert.Empty(component.FindAll(".e-sidebar-target"));
            Assert.Equal(string.Empty, component.Find(".e-sidebar-body").TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "I-INT-005 Missing Id generates one stable identifier per instance")]
        public void I_INT_005_MissingId_IsStablePerInstance()
        {
            var first = RenderComponent<SfSidebar>();
            var second = RenderComponent<SfSidebar>();

            string firstId = first.Find("div.e-sidebar").GetAttribute("id")!;
            first.SetParametersAndRender(p => p.Add(s => s.CssClass, "again"));

            Assert.Equal(firstId, first.Find("div.e-sidebar").GetAttribute("id"));
            Assert.NotEqual(firstId, second.Find("div.e-sidebar").GetAttribute("id"));
        }

        #endregion

        private sealed class DualHost : ComponentBase
        {
            protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
            {
                builder.OpenComponent<SfSidebar>(0);
                builder.AddAttribute(1, nameof(SfSidebar.Id), "dual-start");
                builder.AddAttribute(2, nameof(SfSidebar.Mode), SidebarMode.Push);
                builder.AddAttribute(3, nameof(SfSidebar.Position), SidebarPosition.Start);
                builder.AddAttribute(4, nameof(SfSidebar.IsOpen), true);
                builder.CloseComponent();
                builder.OpenComponent<SfSidebar>(5);
                builder.AddAttribute(6, nameof(SfSidebar.Id), "dual-end");
                builder.AddAttribute(7, nameof(SfSidebar.Mode), SidebarMode.Push);
                builder.AddAttribute(8, nameof(SfSidebar.Position), SidebarPosition.End);
                builder.AddAttribute(9, nameof(SfSidebar.IsOpen), true);
                builder.CloseComponent();
            }
        }
    }
}
