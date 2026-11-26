using Bunit;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.JSInterop;
using System.Text.Json;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    public class ToolbarInitialMeasurementTests : ToolbarTestBase
    {
        private const string ModulePath = "./_content/Syncfusion.Blazor.Toolkit/scripts/toolbar.js";
        private const string OverflowButton = "[data-testid='toolbar-overflow-button']";

        [Fact(DisplayName = "U-TB-110 Browser measurement deserializes measured items")]
        public void BrowserMeasurement_DeserializesMeasuredItems()
        {
            ToolbarMeasurement? measurement = JsonSerializer.Deserialize<ToolbarMeasurement>(
                "{\"available\":100,\"gap\":4,\"buttonSize\":32,\"items\":[{\"id\":\"a\",\"size\":50,\"row\":0}] }");

            Assert.NotNull(measurement);
            Assert.Equal(100, measurement.Available);
            Assert.Equal("a", Assert.Single(measurement.Items).Id);
        }

        [Fact(DisplayName = "U-TB-111 Trimmed browser measurements preserve nested DTO members")]
        public void BrowserMeasurement_PreservesNestedDtoMembersWhenTrimming()
        {
            MethodInfo recalculate = typeof(SfToolbar).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Single(method => method.Name == "RecalculateAsync" && method.GetParameters().Length == 0);
            DynamicallyAccessedMemberTypes requiredMembers =
                DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties;

            Assert.Contains(
                recalculate.GetCustomAttributes<DynamicDependencyAttribute>(),
                dependency => dependency.Type == typeof(ToolbarMeasuredItem) &&
                    (dependency.MemberTypes & requiredMembers) == requiredMembers);
        }

        [Fact(DisplayName = "U-TB-108 Popup toolbar retries an incomplete initial measurement")]
        public void PopupToolbar_IncompleteInitialMeasurement_RetriesBeforeShowingItems()
        {
            BunitJSModuleInterop module = JSInterop.SetupModule(ModulePath);
            module.Mode = JSRuntimeMode.Loose;
            module.Setup<ToolbarInitialMeasurement>("initialize", _ => true).SetResult(new ToolbarInitialMeasurement
            {
                Observing = true,
                Measurement = new ToolbarMeasurement
                {
                    Available = 100,
                    Gap = 4,
                    ButtonSize = 32,
                    Items = [new ToolbarMeasuredItem { Id = "a", Size = 50 }]
                }
            });
            module.Setup<ToolbarMeasurement>("measure", _ => true).SetResult(new ToolbarMeasurement
            {
                Available = 100,
                Gap = 4,
                ButtonSize = 32,
                Items =
                [
                    new ToolbarMeasuredItem { Id = "a", Size = 50 },
                    new ToolbarMeasuredItem { Id = "b", Size = 50 },
                    new ToolbarMeasuredItem { Id = "c", Size = 50 }
                ]
            });

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("A", ("Id", "a")),
                Item("B", ("Id", "b")),
                Item("C", ("Id", "c"))));

            cut.WaitForAssertion(() =>
            {
                Assert.DoesNotContain("e-toolbar-measuring", cut.Find(Root).ClassList);
                Assert.Single(cut.FindAll(OverflowButton));
                Assert.Single(cut.Instance.GetVisibleItems());
                Assert.Equal(2, cut.Instance.GetOverflowItems().Count);
            });
        }

        [Fact(DisplayName = "U-TB-109 Popup toolbar retries a null initial measurement")]
        public void PopupToolbar_NullInitialMeasurement_RetriesBeforeShowingItems()
        {
            BunitJSModuleInterop module = JSInterop.SetupModule(ModulePath);
            module.Mode = JSRuntimeMode.Loose;
            module.Setup<ToolbarInitialMeasurement>("initialize", _ => true).SetResult(new ToolbarInitialMeasurement
            {
                Observing = true,
                Measurement = null
            });
            module.Setup<ToolbarMeasurement>("measure", _ => true).SetResult(new ToolbarMeasurement
            {
                Available = 100,
                Gap = 4,
                ButtonSize = 32,
                Items =
                [
                    new ToolbarMeasuredItem { Id = "a", Size = 50 },
                    new ToolbarMeasuredItem { Id = "b", Size = 50 },
                    new ToolbarMeasuredItem { Id = "c", Size = 50 }
                ]
            });

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("A", ("Id", "a")),
                Item("B", ("Id", "b")),
                Item("C", ("Id", "c"))));

            cut.WaitForAssertion(() =>
            {
                Assert.DoesNotContain("e-toolbar-measuring", cut.Find(Root).ClassList);
                Assert.Single(cut.FindAll(OverflowButton));
                Assert.Single(cut.Instance.GetVisibleItems());
                Assert.Equal(2, cut.Instance.GetOverflowItems().Count);
            });
        }

        [Fact(DisplayName = "U-TB-112 Popup toolbar applies its final initial layout through one interop")]
        public void PopupToolbar_AppliesFinalInitialLayoutThroughOneInterop()
        {
            BunitJSModuleInterop module = JSInterop.SetupModule(ModulePath);
            module.Mode = JSRuntimeMode.Loose;
            module.Setup<ToolbarInitialMeasurement>("initialize", _ => true).SetResult(new ToolbarInitialMeasurement
            {
                Observing = true,
                Measurement = new ToolbarMeasurement
                {
                    Available = 100,
                    Gap = 4,
                    ButtonSize = 32,
                    Items =
                    [
                        new ToolbarMeasuredItem { Id = "a", Size = 50 },
                        new ToolbarMeasuredItem { Id = "b", Size = 50 },
                        new ToolbarMeasuredItem { Id = "c", Size = 50 }
                    ]
                }
            });

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Compose(
                Item("A", ("Id", "a")),
                Item("B", ("Id", "b")),
                Item("C", ("Id", "c"))));

            cut.WaitForAssertion(() =>
            {
                Assert.DoesNotContain("e-toolbar-measuring", cut.Find(Root).ClassList);
                Assert.Single(cut.Instance.GetVisibleItems());
                Assert.Equal(2, cut.Instance.GetOverflowItems().Count);
            });

            Assert.Single(module.Invocations, invocation => invocation.Identifier == "initialize");
            Assert.DoesNotContain(module.Invocations, invocation =>
                invocation.Identifier is "observe" or "trackOverflowKeys" or "measure");
            FieldInfo retryScheduled = typeof(SfToolbar).GetField("_initialMeasurementRetryScheduled", BindingFlags.Instance | BindingFlags.NonPublic)!;
            Assert.False((bool)retryScheduled.GetValue(cut.Instance)!);
            Assert.Equal(1, (int)typeof(SfToolbar)
                .GetField("_initialMeasurementAttempts", BindingFlags.Instance | BindingFlags.NonPublic)!
                .GetValue(cut.Instance)!);
        }
    }
}
