using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Blazor.Toolkit;
using Syncfusion.Blazor.Toolkit.Internal;
using Syncfusion.Blazor.Toolkit.Tests;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations
{
    public class SfSidebarTest : BunitTestContext
    {
        private static RenderFragment Fragment(string markup)
        {
            return builder => builder.AddMarkupContent(0, markup);
        }

        private static EventCallback<T> Callback<T>(object receiver, Action<T> action)
        {
            return EventCallback.Factory.Create(receiver, action);
        }

        #region Defaults and structure

        [Fact(Timeout = 10000, DisplayName = "U-REN-001 U-OPEN-002 Sidebar renders closed with documented defaults")]
        public void Defaults_ShouldMatchDocumentedValues()
        {
            var component = RenderComponent<SfSidebar>();

            Assert.False(component.Instance.IsOpen);
            Assert.Equal(SidebarMode.Overlay, component.Instance.Mode);
            Assert.Equal(SidebarPosition.Start, component.Instance.Position);
            Assert.Equal("280px", component.Instance.Width);
            Assert.Equal("64px", component.Instance.MiniWidth);
            Assert.True(component.Instance.ShowBackdrop);
            Assert.True(component.Instance.CloseOnOverlayClick);
            Assert.True(component.Instance.CloseOnEscape);
            Assert.True(component.Instance.EnableGestures);
            Assert.False(component.Instance.EnableMini);

            var sidebar = component.Find("div.e-sidebar");
            Assert.Contains("e-sidebar-overlay", sidebar.ClassName);
            Assert.Contains("e-sidebar-left", sidebar.ClassName);
            Assert.DoesNotContain("e-sidebar-open", sidebar.ClassName);
            Assert.DoesNotContain("e-sidebar-mini", sidebar.ClassName);
            Assert.Equal("navigation", sidebar.GetAttribute("role"));
            Assert.Equal("true", sidebar.GetAttribute("aria-hidden"));
            Assert.Equal("Sidebar", sidebar.GetAttribute("aria-label"));
            Assert.Null(sidebar.GetAttribute("aria-modal"));
            var style = component.Find(".e-sidebar-container").GetAttribute("style");
            Assert.Contains("--e-sidebar-width: 280px", style);
            Assert.Contains("--e-sidebar-duration: 300ms", style);
            Assert.StartsWith("e-sidebar-", sidebar.GetAttribute("id"), StringComparison.Ordinal);
            Assert.Null(component.Find(".e-sidebar-container").GetAttribute("id"));
            Assert.Empty(component.FindAll(".e-sidebar-header"));
            Assert.Empty(component.FindAll(".e-sidebar-footer"));
            Assert.Empty(component.FindAll(".e-sidebar-target"));
            Assert.Empty(component.FindAll(".e-sidebar-backdrop"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-API-007 Enum zero values are the parameter defaults")]
        public void Enums_ZeroValueShouldBeParameterDefault()
        {
            Assert.Equal(0, (int)SidebarMode.Overlay);
            Assert.Equal(0, (int)SidebarPosition.Start);
            Assert.Equal(0, (int)SidebarTrigger.Api);
        }

        [Fact(Timeout = 10000, DisplayName = "U-REN-002 U-REN-003 U-REN-004 U-REN-005 Templates and target render only when supplied")]
        public void Templates_ShouldRenderRegionsOnlyWhenSupplied()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.HeaderTemplate, Fragment("<span>Header</span>"))
                .Add(s => s.ChildContent, Fragment("<nav>Menu</nav>"))
                .Add(s => s.FooterTemplate, Fragment("<span>Footer</span>"))
                .Add(s => s.TargetContent, Fragment("<main>Body</main>")));

            Assert.Equal("Header", component.Find(".e-sidebar-header").TextContent.Trim());
            Assert.Equal("Menu", component.Find(".e-sidebar-body").TextContent.Trim());
            Assert.Equal("Footer", component.Find(".e-sidebar-footer").TextContent.Trim());
            Assert.Equal("Body", component.Find(".e-sidebar-target").TextContent.Trim());
        }

        [Fact(Timeout = 10000, DisplayName = "U-REN-007 An explicit Id lands on the panel and stays there across re-renders")]
        public void Id_Explicit_ShouldLandOnThePanel()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.Id, "main-nav-sidebar"));

            Assert.Equal("main-nav-sidebar", component.Find("div.e-sidebar").GetAttribute("id"));
            Assert.Null(component.Find(".e-sidebar-container").GetAttribute("id"));

            component.SetParametersAndRender(p => p.Add(s => s.CssClass, "changed"));

            Assert.Equal("main-nav-sidebar", component.Find("div.e-sidebar").GetAttribute("id"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-REN-014 An omitted Id is generated once and a later value replaces it")]
        public void Id_Generated_ShouldBeStableUntilSet()
        {
            var component = RenderComponent<SfSidebar>();
            var generated = component.Find("div.e-sidebar").GetAttribute("id");

            Assert.StartsWith("e-sidebar-", generated, StringComparison.Ordinal);

            component.SetParametersAndRender(p => p.Add(s => s.CssClass, "changed"));
            Assert.Equal(generated, component.Find("div.e-sidebar").GetAttribute("id"));

            component.SetParametersAndRender(p => p.Add(s => s.Id, "assigned-later"));
            Assert.Equal("assigned-later", component.Find("div.e-sidebar").GetAttribute("id"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-REN-008 CssClass is appended to the sidebar element")]
        public void CssClass_ShouldBeAppended()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.CssClass, "custom-one custom-two"));

            var sidebar = component.Find("div.e-sidebar");
            Assert.Contains("custom-one", sidebar.ClassName);
            Assert.Contains("custom-two", sidebar.ClassName);
        }

        #endregion

        #region Modes, position, mini, RTL

        [Fact(Timeout = 10000, DisplayName = "U-OPEN-001 Open sidebar carries the open hooks")]
        public void Open_ShouldRenderOpenClasses()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.IsOpen, true));

            var sidebar = component.Find("div.e-sidebar");
            Assert.Contains("e-sidebar-open", sidebar.ClassName);
            Assert.Null(sidebar.GetAttribute("aria-hidden"));
            Assert.Contains("e-sidebar-container-open", component.Find(".e-sidebar-container").ClassName);
        }

        [Theory(Timeout = 10000, DisplayName = "U-MODE-001 U-MODE-002 U-MODE-003 Mode drives sidebar and container classes")]
        [InlineData(SidebarMode.Overlay, "e-sidebar-overlay", "e-sidebar-container-overlay")]
        [InlineData(SidebarMode.Push, "e-sidebar-push", "e-sidebar-container-push")]
        [InlineData(SidebarMode.Shrink, "e-sidebar-shrink", "e-sidebar-container-shrink")]
        public void Mode_ShouldRenderModeClasses(SidebarMode mode, string sidebarClass, string containerClass)
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Mode, mode)
                .Add(s => s.TargetContent, Fragment("<main>Body</main>")));

            Assert.Contains(sidebarClass, component.Find("div.e-sidebar").ClassName);
            Assert.Contains(containerClass, component.Find(".e-sidebar-container").ClassName);
        }

        [Theory(Timeout = 10000, DisplayName = "U-POS-001 U-POS-002 U-POS-003 U-POS-004 U-POS-005 U-POS-006 Physical and logical positions resolve to an edge")]
        [InlineData(SidebarPosition.Left, false, "e-sidebar-left")]
        [InlineData(SidebarPosition.Right, false, "e-sidebar-right")]
        [InlineData(SidebarPosition.Start, false, "e-sidebar-left")]
        [InlineData(SidebarPosition.End, false, "e-sidebar-right")]
        [InlineData(SidebarPosition.Left, true, "e-sidebar-left")]
        [InlineData(SidebarPosition.Right, true, "e-sidebar-right")]
        [InlineData(SidebarPosition.Start, true, "e-sidebar-right")]
        [InlineData(SidebarPosition.End, true, "e-sidebar-left")]
        public void Position_ShouldResolveAgainstRtl(SidebarPosition position, bool rtl, string expected)
        {
            if (rtl)
            {
                Services.Configure<GlobalOptions>(o => o.EnableRtl = true);
            }

            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.Position, position));

            Assert.Contains(expected, component.Find("div.e-sidebar").ClassName);
            Assert.Equal(rtl, component.Find(".e-sidebar-container").ClassName.Contains("e-rtl", StringComparison.Ordinal));
        }

        [Fact(Timeout = 10000, DisplayName = "U-OPEN-003 Mini rail keeps a closed sidebar visible to assistive technology")]
        public void Mini_WhenClosed_ShouldStayExposed()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Mode, SidebarMode.Shrink)
                .Add(s => s.EnableMini, true)
                .Add(s => s.Width, "300px")
                .Add(s => s.MiniWidth, "72px"));

            var sidebar = component.Find("div.e-sidebar");
            Assert.Contains("e-sidebar-mini", sidebar.ClassName);
            Assert.Null(sidebar.GetAttribute("aria-hidden"));
            var style = component.Find(".e-sidebar-container").GetAttribute("style");
            Assert.Contains("--e-sidebar-width: 300px", style);
            Assert.Contains("--e-sidebar-mini-width: 72px", style);
        }

        [Fact(Timeout = 10000, DisplayName = "U-CSS-006 Disabling global animation adds the no-animation hook")]
        public void GlobalAnimationDisabled_ShouldAddNoAnimationClass()
        {
            Services.Configure<GlobalOptions>(o => o.Animation = GlobalAnimationMode.Disable);

            var component = RenderComponent<SfSidebar>();

            Assert.Contains("e-sidebar-no-animation", component.Find(".e-sidebar-container").ClassName);
        }

        [Theory(Timeout = 10000, DisplayName = "U-CSS-004 Unsafe CSS lengths fall back to the defaults")]
        [InlineData("10px; background:url(x)")]
        [InlineData("url(javascript:alert(1))")]
        [InlineData("10px } body { color: red")]
        [InlineData("1px /* c */")]
        [InlineData("\"280px\"")]
        [InlineData("")]
        [InlineData(null)]
        public void Width_Unsafe_ShouldFallBackToDefault(string? width)
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.Width, width));

            var style = component.Find(".e-sidebar-container").GetAttribute("style");
            Assert.Contains("--e-sidebar-width: 280px;", style);
            Assert.DoesNotContain("url", style, StringComparison.OrdinalIgnoreCase);
        }

        [Fact(Timeout = 10000, DisplayName = "U-CSS-005 calc() lengths are accepted")]
        public void Width_Calc_ShouldBeAccepted()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.Width, "calc(100% - 2rem)"));

            Assert.Contains("--e-sidebar-width: calc(100% - 2rem);", component.Find(".e-sidebar-container").GetAttribute("style"));
        }

        #endregion

        #region Accessibility

        [Fact(Timeout = 10000, DisplayName = "U-MODE-005 U-A11Y-001 U-A11Y-002 U-A11Y-008 Open modal overlay is a modal dialog and the target is inert")]
        public void ModalOverlay_ShouldExposeDialogSemantics()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.TargetContent, Fragment("<main>Body</main>")));

            var sidebar = component.Find("div.e-sidebar");
            Assert.Equal("dialog", sidebar.GetAttribute("role"));
            Assert.Equal("true", sidebar.GetAttribute("aria-modal"));
            Assert.NotNull(component.Find(".e-sidebar-target").GetAttribute("inert"));
            Assert.Single(component.FindAll(".e-sidebar-backdrop"));
            Assert.Equal("true", component.Find(".e-sidebar-backdrop").GetAttribute("aria-hidden"));
        }

        [Theory(Timeout = 10000, DisplayName = "U-MODE-007 U-A11Y-006 Docked rendering uses navigation semantics without a backdrop")]
        [InlineData(SidebarMode.Push)]
        [InlineData(SidebarMode.Shrink)]
        public void Docked_ShouldExposeNavigationSemantics(SidebarMode mode)
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Mode, mode)
                .Add(s => s.IsOpen, true)
                .Add(s => s.TargetContent, Fragment("<main>Body</main>")));

            var sidebar = component.Find("div.e-sidebar");
            Assert.Equal("navigation", sidebar.GetAttribute("role"));
            Assert.Null(sidebar.GetAttribute("aria-modal"));
            Assert.Null(component.Find(".e-sidebar-target").GetAttribute("inert"));
            Assert.Empty(component.FindAll(".e-sidebar-backdrop"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-MODE-006 Overlay without backdrop is not modal")]
        public void OverlayWithoutBackdrop_ShouldNotBeModal()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.ShowBackdrop, false));

            Assert.Equal("navigation", component.Find("div.e-sidebar").GetAttribute("role"));
            Assert.Null(component.Find("div.e-sidebar").GetAttribute("aria-modal"));
            Assert.Empty(component.FindAll(".e-sidebar-backdrop"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-A11Y-003 U-A11Y-007 Explicit Role and AriaLabel win over the defaults")]
        public void RoleAndLabel_Explicit_ShouldWin()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.Role, "complementary")
                .Add(s => s.AriaLabel, "Main menu"));

            var sidebar = component.Find("div.e-sidebar");
            Assert.Equal("complementary", sidebar.GetAttribute("role"));
            Assert.Equal("Main menu", sidebar.GetAttribute("aria-label"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-A11Y-013 State changes are announced through a polite live region")]
        public async Task StateChange_ShouldBeAnnounced()
        {
            var component = RenderComponent<SfSidebar>();
            var live = component.Find(".e-sidebar-live");
            Assert.Equal("polite", live.GetAttribute("aria-live"));
            Assert.Equal("status", live.GetAttribute("role"));
            Assert.Equal(string.Empty, live.TextContent);

            await component.Instance.OpenAsync();
            Assert.Equal("Sidebar opened", component.Find(".e-sidebar-live").TextContent);

            await component.Instance.CloseAsync();
            Assert.Equal("Sidebar closed", component.Find(".e-sidebar-live").TextContent);
        }

        [Fact(Timeout = 10000, DisplayName = "U-FOC-003 Opening a modal overlay starts the focus-entry script")]
        public async Task ModalOpen_ShouldFocusSidebar()
        {
            var component = RenderComponent<SfSidebar>();

            await component.Instance.OpenAsync();

            Assert.Single(JSInterop.Invocations.Where(i => i.Identifier == "activateModal"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-FOC-010 Opening a docked sidebar does not steal focus")]
        public async Task DockedOpen_ShouldNotFocus()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.Mode, SidebarMode.Push));

            await component.Instance.OpenAsync();

            Assert.DoesNotContain(JSInterop.Invocations, i => i.Identifier is "activateModal" or "focusPanel");
        }

        #endregion

        #region Methods and events

        [Fact(Timeout = 10000, DisplayName = "U-OPEN-005 U-OPEN-006 U-OPEN-007 OpenAsync, CloseAsync and ToggleAsync change the state")]
        public async Task Methods_ShouldChangeState()
        {
            var component = RenderComponent<SfSidebar>();

            await component.Instance.OpenAsync();
            Assert.Contains("e-sidebar-open", component.Find("div.e-sidebar").ClassName);

            await component.Instance.CloseAsync();
            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);

            await component.Instance.ToggleAsync();
            Assert.Contains("e-sidebar-open", component.Find("div.e-sidebar").ClassName);

            await component.Instance.ToggleAsync();
            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "U-OPEN-009 OnOpened fires after the state change and carries the applied state and trigger")]
        public async Task Open_ShouldRaiseEventsInOrder()
        {
            List<string> log = new();
            IRenderedComponent<SfSidebar> component = null!;
            component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, a => log.Add($"opened:{a.IsOpen}:{a.Trigger}:{component.Find("div.e-sidebar").ClassList.Contains("e-sidebar-open")}")))
                .Add(s => s.IsOpenChanged, Callback<bool>(this, v => log.Add($"changed:{v}"))));

            await component.Instance.OpenAsync();

            Assert.Equal(new[] { "changed:True", "opened:True:Api:True" }, log);
            Assert.Contains("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "U-OPEN-016 OnClosed fires after the state change and carries the applied state and trigger")]
        public async Task Close_ShouldRaiseEventsInOrder()
        {
            List<string> log = new();
            IRenderedComponent<SfSidebar> component = null!;
            component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.IsOpen, true)
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, a => log.Add($"closed:{a.IsOpen}:{a.Trigger}:{component.Find("div.e-sidebar").ClassList.Contains("e-sidebar-open")}")))
                .Add(s => s.IsOpenChanged, Callback<bool>(this, v => log.Add($"changed:{v}"))));

            await component.Instance.CloseAsync();

            Assert.Equal(new[] { "changed:False", "closed:False:Api:False" }, log);
            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "U-OPEN-013 No-op changes raise no events and IsOpenChanged only fires on a real change")]
        public async Task NoOp_ShouldRaiseNothing()
        {
            var count = 0;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, a => count++))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, a => count++))
                .Add(s => s.IsOpenChanged, Callback<bool>(this, v => count++)));

            await component.Instance.CloseAsync();

            Assert.Equal(0, count);
        }

        [Fact(Timeout = 10000, DisplayName = "U-OPEN-012 Two-way binding follows parent changes without raising events")]
        public void ParentChange_ShouldUpdateWithoutEvents()
        {
            var count = 0;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, a => count++))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, a => count++))
                .Add(s => s.IsOpenChanged, Callback<bool>(this, v => count++)));

            component.SetParametersAndRender(p => p.Add(s => s.IsOpen, true));
            Assert.Contains("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
            Assert.Equal("Sidebar opened", component.Find(".e-sidebar-live").TextContent);

            component.SetParametersAndRender(p => p.Add(s => s.IsOpen, false));
            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);

            Assert.Equal(0, count);
        }

        [Fact(Timeout = 10000, DisplayName = "U-OPEN-014 An unrelated parent re-render keeps an internally changed state")]
        public async Task UnrelatedRerender_ShouldKeepInternalState()
        {
            var component = RenderComponent<SfSidebar>();
            await component.Instance.OpenAsync();

            component.SetParametersAndRender(p => p.Add(s => s.CssClass, "changed"));

            Assert.Contains("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "U-FOC-002 FocusAsync is safe before and after the first render and never changes state")]
        public async Task FocusAsync_ShouldBeSafe()
        {
            var component = RenderComponent<SfSidebar>();

            await component.Instance.FocusAsync();
            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);

            await component.Instance.OpenAsync();
            await component.Instance.FocusAsync();
            Assert.Contains("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
        }

        #endregion

        #region User interaction

        [Fact(Timeout = 10000, DisplayName = "U-UIX-002 U-UIX-004 CloseOnOverlayClick=false ignores a backdrop click and leaves Escape working")]
        public void BackdropClick_Disabled_ShouldIgnore()
        {
            var count = 0;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.IsOpen, true)
                .Add(s => s.CloseOnOverlayClick, false)
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, a => count++)));

            component.Find(".e-sidebar-backdrop").Click();

            Assert.Contains("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
            Assert.Equal(0, count);

            component.Find("div.e-sidebar").KeyDown(new KeyboardEventArgs { Key = "Escape" });

            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
            Assert.Equal(1, count);
        }

        [Fact(Timeout = 10000, DisplayName = "U-UIX-001 Backdrop click closes the sidebar with the Backdrop trigger")]
        public void BackdropClick_ShouldClose()
        {
            SidebarEventArgs? args = null;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.IsOpen, true)
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, a => args = a)));

            component.Find(".e-sidebar-backdrop").Click();

            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
            Assert.NotNull(args);
            Assert.Equal(SidebarTrigger.Backdrop, args!.Trigger);
            Assert.Empty(component.FindAll(".e-sidebar-backdrop"));
        }

        [Fact(Timeout = 10000, DisplayName = "U-UIX-003 Escape closes an open sidebar and other keys do not")]
        public void Escape_ShouldClose()
        {
            SidebarEventArgs? args = null;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.IsOpen, true)
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, a => args = a)));

            component.Find("div.e-sidebar").KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });
            Assert.Contains("e-sidebar-open", component.Find("div.e-sidebar").ClassName);

            component.Find("div.e-sidebar").KeyDown(new KeyboardEventArgs { Key = "Escape" });

            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
            Assert.Equal(SidebarTrigger.Keyboard, args!.Trigger);
        }

        [Fact(Timeout = 10000, DisplayName = "U-UIX-004 CloseOnEscape=false ignores Escape")]
        public void Escape_Disabled_ShouldIgnore()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.IsOpen, true)
                .Add(s => s.CloseOnEscape, false));

            component.Find("div.e-sidebar").KeyDown(new KeyboardEventArgs { Key = "Escape" });

            Assert.Contains("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "U-LEG-001 A touch swipe toward the content opens a left sidebar and back closes it")]
        public void Swipe_ShouldToggle()
        {
            SidebarEventArgs? last = null;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, a => last = a))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, a => last = a)));
            var container = component.Find(".e-sidebar-container");

            container.PointerDown(new PointerEventArgs { ClientX = 0, ClientY = 0, PointerType = "touch" });
            container.PointerUp(new PointerEventArgs { ClientX = 120, ClientY = 5, PointerType = "touch" });
            Assert.Contains("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
            Assert.Equal(SidebarTrigger.Gesture, last!.Trigger);

            container.PointerDown(new PointerEventArgs { ClientX = 120, ClientY = 0, PointerType = "touch" });
            container.PointerUp(new PointerEventArgs { ClientX = 0, ClientY = 5, PointerType = "touch" });
            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "U-LEG-002 Mouse drags and EnableGestures=false never change the state")]
        public void Swipe_MouseOrDisabled_ShouldBeIgnored()
        {
            var component = RenderComponent<SfSidebar>();
            var container = component.Find(".e-sidebar-container");

            container.PointerDown(new PointerEventArgs { ClientX = 0, ClientY = 0, PointerType = "mouse" });
            container.PointerUp(new PointerEventArgs { ClientX = 200, ClientY = 0, PointerType = "mouse" });
            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);

            component.SetParametersAndRender(p => p.Add(s => s.EnableGestures, false));
            container.PointerDown(new PointerEventArgs { ClientX = 0, ClientY = 0, PointerType = "touch" });
            container.PointerUp(new PointerEventArgs { ClientX = 200, ClientY = 0, PointerType = "touch" });
            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "U-LEG-003 A cancelled pointer discards the swipe start")]
        public void PointerCancel_ShouldDiscardSwipe()
        {
            var component = RenderComponent<SfSidebar>();
            var container = component.Find(".e-sidebar-container");

            container.PointerDown(new PointerEventArgs { ClientX = 0, ClientY = 0, PointerType = "touch" });
            container.TriggerEvent("onpointercancel", new PointerEventArgs());
            container.PointerUp(new PointerEventArgs { ClientX = 200, ClientY = 0, PointerType = "touch" });

            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
        }

        #endregion

        #region Media query, navigation, attributes

        [Fact(Timeout = 10000, DisplayName = "U-LEG-004 A matching media query opens the sidebar and a non-match closes it")]
        public async Task MediaQuery_ShouldFollowMatchChanges()
        {
            List<string> log = new();
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.MediaQuery, "(max-width: 768px)")
                .Add(s => s.OnOpened, Callback<SidebarEventArgs>(this, a => log.Add($"opened:{a.Trigger}")))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, a => log.Add($"closed:{a.Trigger}"))));

            await component.InvokeAsync(() => component.Instance.OnMediaQueryChanged(true));
            Assert.Contains("e-sidebar-open", component.Find("div.e-sidebar").ClassName);

            await component.InvokeAsync(() => component.Instance.OnMediaQueryChanged(false));
            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
            Assert.Equal(new[] { "opened:Api", "closed:Api" }, log);
        }

        [Fact(Timeout = 10000, DisplayName = "U-LEG-006 An unchanged media-query match raises no events")]
        public async Task MediaQuery_NoOp_ShouldRaiseNothing()
        {
            var count = 0;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.MediaQuery, "(max-width: 768px)")
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, a => count++)));

            await component.InvokeAsync(() => component.Instance.OnMediaQueryChanged(false));

            Assert.Equal(0, count);
        }

        [Fact(Timeout = 10000, DisplayName = "U-LEG-007 CloseOnNavigate closes an open sidebar with the Navigation trigger")]
        public void Navigation_ShouldCloseOpenSidebar()
        {
            SidebarEventArgs? args = null;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.IsOpen, true)
                .Add(s => s.IsOpenChanged, Callback<bool>(this, _ => { }))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, a => args = a)));

            NavigateTo("next");

            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
            Assert.Equal(SidebarTrigger.Navigation, args!.Trigger);
        }

        [Fact(Timeout = 10000, DisplayName = "U-LEG-008 CloseOnNavigate leaves a closed sidebar unchanged")]
        public void Navigation_WhenClosed_ShouldDoNothing()
        {
            var count = 0;
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.IsOpenChanged, Callback<bool>(this, _ => { }))
                .Add(s => s.OnClosed, Callback<SidebarEventArgs>(this, a => count++)));

            NavigateTo("next");

            Assert.Equal(0, count);
            Assert.DoesNotContain("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "U-LEG-010 Without two-way binding location changes are ignored")]
        public void Navigation_Disabled_ShouldIgnore()
        {
            var component = RenderComponent<SfSidebar>(p => p.Add(s => s.IsOpen, true));

            NavigateTo("next");

            Assert.Contains("e-sidebar-open", component.Find("div.e-sidebar").ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "U-REN-009 HtmlAttributes render safe attributes and never overwrite owned ones")]
        public void HtmlAttributes_ShouldRenderSafeAttributesOnly()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.Id, "owned-id")
                .Add(s => s.AriaLabel, "Owned label")
                .Add(s => s.HtmlAttributes, new Dictionary<string, object>
                {
                    ["data-region"] = "primary",
                    ["title"] = "Application menu",
                    ["id"] = "injected",
                    ["class"] = "injected",
                    ["role"] = "injected",
                    ["aria-label"] = "injected",
                    ["aria-hidden"] = "false",
                    ["tabindex"] = "0",
                    ["onclick"] = "alert(1)",
                }));

            var sidebar = component.Find("div.e-sidebar");
            Assert.Equal("primary", sidebar.GetAttribute("data-region"));
            Assert.Equal("Application menu", sidebar.GetAttribute("title"));
            Assert.Equal("owned-id", sidebar.GetAttribute("id"));
            Assert.Equal("navigation", sidebar.GetAttribute("role"));
            Assert.Equal("Owned label", sidebar.GetAttribute("aria-label"));
            Assert.Equal("true", sidebar.GetAttribute("aria-hidden"));
            Assert.Equal("-1", sidebar.GetAttribute("tabindex"));
            Assert.Null(sidebar.GetAttribute("onclick"));
            Assert.DoesNotContain("injected", sidebar.ClassName);
        }

        private void NavigateTo(string uri)
        {
            Services.GetRequiredService<NavigationManager>().NavigateTo(uri);
        }

        [Theory(Timeout = 10000, DisplayName = "U-HOV-006 Hover expansion occupies Push and Shrink layout space")]
        [InlineData(SidebarMode.Push, "e-sidebar-push")]
        [InlineData(SidebarMode.Shrink, "e-sidebar-shrink")]
        public void HoverExpansion_ShouldOccupyPushAndShrinkLayout(SidebarMode mode, string modeClass)
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableMini, true)
                .Add(s => s.OpenMiniOnHover, true)
                .Add(s => s.Mode, mode)
                .Add(s => s.TargetContent, Fragment("<main>Body</main>")));

            component.Find("div.e-sidebar").TriggerEvent("onpointerenter", new PointerEventArgs { PointerType = "mouse" });

            Assert.Contains("e-sidebar-hover", component.Find("div.e-sidebar").ClassName, StringComparison.Ordinal);
            Assert.Contains(modeClass, component.Find("div.e-sidebar").ClassName, StringComparison.Ordinal);
            Assert.False(component.Instance.IsOpen);
            Assert.Contains("--e-sidebar-occupied-width: var(--e-sidebar-width)", component.Find(".e-sidebar-container").GetAttribute("style"), StringComparison.Ordinal);
        }

        [Fact(Timeout = 10000, DisplayName = "U-HOV-007 Enabling hover while open takes effect after close")]
        public void EnablingHoverWhileOpen_ShouldExpandAfterClose()
        {
            var component = RenderComponent<SfSidebar>(p => p
                .Add(s => s.EnableMini, true)
                .Add(s => s.IsOpen, true)
                .Add(s => s.EnableAnimation, false)
                .Add(s => s.TargetContent, Fragment("<main>Body</main>")));

            component.SetParametersAndRender(p => p.Add(s => s.OpenMiniOnHover, true));
            Assert.DoesNotContain("onpointerenter", component.Markup, StringComparison.OrdinalIgnoreCase);

            component.SetParametersAndRender(p => p.Add(s => s.IsOpen, false));
            component.Find("div.e-sidebar").TriggerEvent("onpointerenter", new PointerEventArgs { PointerType = "mouse" });

            Assert.Contains("e-sidebar-hover", component.Find("div.e-sidebar").ClassName, StringComparison.Ordinal);
            Assert.False(component.Instance.IsOpen);
        }

        #endregion
        #region Internal layout helpers
        [Theory(Timeout = 10000, DisplayName = "U-LAY-001 SidebarLayout.ResolveToRight handles every position")]
        [InlineData(SidebarPosition.Start, false, false)]
        [InlineData(SidebarPosition.Start, true, true)]
        [InlineData(SidebarPosition.End, false, true)]
        [InlineData(SidebarPosition.End, true, false)]
        [InlineData(SidebarPosition.Left, true, false)]
        [InlineData(SidebarPosition.Right, false, true)]
        public void ResolveToRight_ShouldMatchTable(SidebarPosition position, bool rtl, bool expected)
        {
            Assert.Equal(expected, SidebarLayout.ResolveToRight(position, rtl));
        }

        [Theory(Timeout = 10000, DisplayName = "U-LAY-002 SidebarLayout.GetSwipeResult interprets gestures")]
        [InlineData(false, false, 80, 0, true)]
        [InlineData(false, true, -80, 0, false)]
        [InlineData(false, false, -80, 0, null)]
        [InlineData(false, true, 80, 0, null)]
        [InlineData(true, false, -80, 0, true)]
        [InlineData(true, true, 80, 0, false)]
        [InlineData(false, false, 30, 0, null)]
        [InlineData(false, false, 80, 90, null)]
        public void GetSwipeResult_ShouldMatchTable(bool toRight, bool isOpen, double dx, double dy, bool? expected)
        {
            Assert.Equal(expected, SidebarLayout.GetSwipeResult(toRight, isOpen, dx, dy, 50));
        }

        #endregion
    }
}
