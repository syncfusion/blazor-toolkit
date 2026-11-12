using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// Lifecycle safety of the toolbar: disposal after the circuit is gone, callbacks that arrive after disposal,
    /// duplicate registration, a container with no room, rapid parameter changes and several toolbars on one page.
    /// </summary>
    public class ToolbarLifecycleTests : ToolbarTestBase
    {
        private const string OverflowButton = "[data-testid='toolbar-overflow-button']";
        private const string ModulePath = "./_content/Syncfusion.Blazor.Toolkit/scripts/toolbar.js";

        private BunitJSModuleInterop? _module;

        private void ScriptMeasurement(double available, params string[] ids)
        {
            _module ??= JSInterop.SetupModule(ModulePath);
            _module.Mode = JSRuntimeMode.Loose;
            ToolbarMeasurement measurement = new()
            {
                Available = available,
                Gap = 4,
                ButtonSize = 32,
                Items = ids.Select(id => new ToolbarMeasuredItem { Id = id, Size = 50 }).ToList()
            };
            _module.Setup<ToolbarMeasurement>("measure", _ => true).SetResult(measurement);
        }

        private static RenderFragment Three()
        {
            return Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b")), Item("C", ("Id", "c")));
        }

        private int MeasureCalls() => JSInterop.Invocations.Count(i => i.Identifier == "measure");

        private void WaitUntilMeasured()
        {
            for (int i = 0; i < 500 && MeasureCalls() < 1; i++)
            {
                Thread.Sleep(10);
            }
            Assert.True(MeasureCalls() >= 1, "The toolbar never measured.");
        }

        private int Settled()
        {
            int last = -1;
            int stable = 0;
            for (int i = 0; i < 200 && stable < 4; i++)
            {
                int now = MeasureCalls();
                stable = now == last ? stable + 1 : 0;
                last = now;
                Thread.Sleep(15);
            }
            return last;
        }

        [Fact(DisplayName = "U-TB-063 Disposing after the circuit disconnected swallows JSDisconnectedException")]
        public void Dispose_WhenCircuitDisconnected_DoesNotThrow()
        {
            ScriptMeasurement(1000, "a", "b", "c");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Three());
            WaitUntilMeasured();
            SfToolbar toolbar = cut.Instance;
            _module!.SetupVoid("unobserve", _ => true).SetException(new JSDisconnectedException("The circuit is gone."));

            Exception? thrown = Record.Exception(DisposeComponents);

            Assert.Null(thrown);
            Assert.Empty(toolbar.GetVisibleItems());
        }

        [Fact(DisplayName = "U-TB-064 A resize, a refresh and a click arriving after disposal are no-ops")]
        public async Task LateCallbacks_AfterDispose_AreNoOps()
        {
            int clicks = 0;
            ScriptMeasurement(1000, "a", "b", "c");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Three(),
                p => p.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, _ => clicks++)));
            WaitUntilMeasured();
            SfToolbar toolbar = cut.Instance;
            int before = Settled();

            DisposeComponents();
            await toolbar.OnContainerResizedAsync();
            await toolbar.RefreshOverflowAsync();
            await toolbar.FocusAsync();
            Thread.Sleep(80);

            Assert.Equal(before, MeasureCalls());
            Assert.Equal(0, clicks);
        }

        [Fact(DisplayName = "U-TB-064 A recalculation that is still pending when the toolbar is disposed never measures")]
        public async Task PendingRecalculation_DisposedBeforeItFires_NeverMeasures()
        {
            ScriptMeasurement(1000, "a", "b", "c");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Three());
            WaitUntilMeasured();
            SfToolbar toolbar = cut.Instance;
            int before = Settled();

            await cut.InvokeAsync(() => toolbar.OnContainerResizedAsync());
            DisposeComponents();
            Thread.Sleep(120);

            Assert.Equal(before, MeasureCalls());
        }

        [Fact(DisplayName = "U-TB-036 Registering the same item twice keeps one entry and does not throw")]
        public void DuplicateRegistration_IsIdempotent()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b"))));
            IToolbarHost host = cut.Instance;
            IToolbarItem first = (IToolbarItem)cut.FindComponents<ToolbarItem>()[0].Instance;

            Exception? thrown = Record.Exception(() =>
            {
                host.Register(first);
                host.Register(first);
            });

            Assert.Null(thrown);
            Assert.Equal(2, cut.Instance.GetVisibleItems().Count);
            Assert.Equal(2, cut.FindAll("[data-testid='toolbar-item']").Count);
        }

        [Fact(DisplayName = "U-TB-039 A container with no room overflows everything and keeps the button")]
        public void ZeroWidthContainer_OverflowsEverything()
        {
            ScriptMeasurement(0, "a", "b", "c");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Three());
            WaitUntilMeasured();

            cut.WaitForAssertion(() =>
            {
                Assert.Single(cut.FindAll(OverflowButton));
                Assert.Empty(cut.FindAll("[data-testid='toolbar-item']:not(.e-toolbar-item-overflowed)"));
                Assert.Equal(3, cut.Instance.GetOverflowItems().Count);
            });
        }

        [Fact(DisplayName = "U-TB-042 With nothing to overflow the overflow button is not rendered")]
        public void NothingOverflowing_ButtonNotRendered()
        {
            ScriptMeasurement(1000, "a", "b", "c");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Three());
            WaitUntilMeasured();
            _ = Settled();

            Assert.Empty(cut.FindAll(OverflowButton));
            Assert.Empty(cut.Instance.GetOverflowItems());
        }

        [Fact(DisplayName = "U-TB-040 Rapid parameter changes end in the final state without throwing")]
        public void RapidParameterChurn_EndsInFinalState()
        {
            ScriptMeasurement(1000, "a", "b", "c");
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Three());
            WaitUntilMeasured();

            Exception? thrown = Record.Exception(() =>
            {
                for (int i = 0; i < 25; i++)
                {
                    int n = i;
                    cut.SetParametersAndRender(p =>
                    {
                        p.Add(t => t.Orientation, n % 2 == 0 ? ToolbarOrientation.Vertical : ToolbarOrientation.Horizontal);
                        p.Add(t => t.OverflowMode, n % 3 == 0 ? ToolbarOverflowMode.Scrollable : ToolbarOverflowMode.Popup);
                        p.Add(t => t.CssClass, "c" + n);
                    });
                }
                cut.SetParametersAndRender(p =>
                {
                    p.Add(t => t.Orientation, ToolbarOrientation.Horizontal);
                    p.Add(t => t.OverflowMode, ToolbarOverflowMode.Popup);
                    p.Add(t => t.CssClass, "final");
                });
            });

            Assert.Null(thrown);
            IReadOnlyList<string> classes = cut.Find(Root).ClassList.ToList();
            Assert.Contains("final", classes);
            Assert.Contains("e-toolbar-popup", classes);
            Assert.DoesNotContain("e-toolbar-vertical", classes);
            Assert.Equal(3, cut.FindAll("[data-testid='toolbar-item']").Count);
        }

        [Fact(DisplayName = "I-TB-001 Two toolbars on one page keep separate items, ids and popup state")]
        public void TwoToolbars_AreIndependent()
        {
            ScriptMeasurement(80, "a", "b", "c");
            IRenderedComponent<SfToolbar> first = RenderToolbar(Three(), p => { p.Add(t => t.AriaLabel, "First"); p.Add(t => t.HtmlAttributes, new Dictionary<string, object> { ["id"] = "first-bar" }); });
            IRenderedComponent<SfToolbar> second = RenderToolbar(Compose(Item("X", ("Id", "x"))), p => { p.Add(t => t.AriaLabel, "Second"); p.Add(t => t.HtmlAttributes, new Dictionary<string, object> { ["id"] = "second-bar" }); });
            WaitUntilMeasured();

            first.WaitForAssertion(() => Assert.Single(first.FindAll(OverflowButton)));
            first.Find(OverflowButton).Click();

            Assert.Single(first.FindAll("[data-testid='toolbar-overflow-popup']"));
            Assert.Empty(second.FindAll("[data-testid='toolbar-overflow-popup']"));
            Assert.Equal("First", first.Find(Root).GetAttribute("aria-label"));
            Assert.Equal("Second", second.Find(Root).GetAttribute("aria-label"));
            Assert.Equal("first-bar", first.Find(Root).Id);
            Assert.Equal("second-bar", second.Find(Root).Id);
        }

        [Fact(DisplayName = "I-TB-003 Re-rendering with changed parameters keeps the items and their state")]
        public void ParameterChurn_KeepsItems()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("A", ("Id", "a")),
                Comp<ToolbarItem>(("Text", "Bold"), ("Id", "bold"), ("Type", ToolbarItemType.Toggle), ("Selected", true))));

            for (int i = 0; i < 5; i++)
            {
                int n = i;
                cut.SetParametersAndRender(p => p.Add(t => t.AriaLabel, "Label " + n));
            }

            Assert.Equal("Label 4", cut.Find(Root).GetAttribute("aria-label"));
            Assert.Equal(2, cut.FindAll("[data-testid='toolbar-item']").Count);
            Assert.Equal("true", cut.FindAll(".e-btn").First(e => e.TextContent.Trim() == "Bold").GetAttribute("aria-pressed"));
        }
        [Fact(DisplayName = "I-TB-012 Switching EnableRtl at runtime flips direction and the arrow key mapping")]
        public void RuntimeRtlChange_FlipsDirectionAndKeys()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(Item("A", ("Id", "a")), Item("B", ("Id", "b")), Item("C", ("Id", "c"))),
                p => p.Add(t => t.EnableRtl, false));
            Assert.DoesNotContain("e-rtl", cut.Find(Root).ClassList);

            cut.SetParametersAndRender(p => p.Add(t => t.EnableRtl, true));

            Assert.Contains("e-rtl", cut.Find(Root).ClassList);
            Assert.Equal("rtl", cut.Find(Root).GetAttribute("dir"));
            AngleSharp.Dom.IElement first = cut.FindAll("[data-testid='toolbar-item']")[0];
            first.KeyDown(new KeyboardEventArgs { Key = "ArrowLeft" });
            cut.WaitForAssertion(() => Assert.Equal("0", cut.FindAll("[data-testid='toolbar-item'] .e-btn")[1].GetAttribute("tabindex")));
        }

        [Fact(DisplayName = "I-TB-010 A toolbar nested in other layout components renders and works")]
        public void NestedInLayout_RendersAndClicks()
        {
            string? clicked = null;
            IRenderedComponent<CascadingValue<string>> cut = RenderComponent<CascadingValue<string>>(p =>
            {
                p.Add(c => c.Value, "theme");
                p.AddChildContent<CascadingValue<int>>(inner =>
                {
                    inner.Add(c => c.Value, 7);
                    inner.AddChildContent<SfToolbar>(tb =>
                    {
                        tb.Add(t => t.OnItemClick, EventCallback.Factory.Create<ToolbarItemClickEventArgs>(this, a => clicked = a.ItemId));
                        tb.AddChildContent(Item("Run", ("Id", "run")));
                    });
                });
            });

            cut.Find("[data-testid='toolbar-item'] .e-btn").Click();

            Assert.Equal("run", clicked);
        }
    }
}
