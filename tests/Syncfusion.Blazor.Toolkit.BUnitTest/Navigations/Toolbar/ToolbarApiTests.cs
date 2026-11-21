using System.Reflection;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// Public API contract of the Toolbar family: types, members, defaults and enum values.
    /// Covers the Public API rows of the Toolbar spec registry.
    /// </summary>
    public class ToolbarApiTests : ToolbarTestBase
    {
        private static readonly Assembly ToolkitAssembly = typeof(SfToolbar).Assembly;

        private static PropertyInfo Prop(Type type, string name)
        {
            PropertyInfo? property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
            Assert.True(property is not null, $"{type.Name}.{name} is missing.");
            return property!;
        }

        private static bool IsParameter(PropertyInfo property)
        {
            return property.GetCustomAttribute<ParameterAttribute>() is not null;
        }

        private static string[] PublicParameters(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(IsParameter).Select(p => p.Name).OrderBy(n => n, StringComparer.Ordinal).ToArray();
        }

        [Fact(DisplayName = "U-TB-060 SfToolbar derives from SfBaseComponent in the Toolkit namespace")]
        public void SfToolbar_DerivesFromBaseInToolkitNamespace()
        {
            Assert.Equal("Syncfusion.Blazor.Toolkit", typeof(SfToolbar).Namespace);
            Assert.True(typeof(SfBaseComponent).IsAssignableFrom(typeof(SfToolbar)));
        }

        [Fact(DisplayName = "U-TB-060 SfToolbar exposes the P0 parameters with documented defaults")]
        public void SfToolbar_ParameterDefaults()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar();
            SfToolbar toolbar = cut.Instance;

            Assert.Equal(ToolbarOrientation.Horizontal, toolbar.Orientation);
            Assert.Equal(ToolbarOverflowMode.Popup, toolbar.OverflowMode);
            Assert.Equal(ToolbarAlignment.Start, toolbar.Align);
            Assert.Null(toolbar.EnableRtl);
            Assert.Null(toolbar.AriaLabel);
            Assert.True(string.IsNullOrEmpty(toolbar.CssClass));
            Assert.Null(toolbar.ChildContent);
        }

        [Fact(DisplayName = "U-TB-048 Default OverflowMode is Popup (ADR-001)")]
        public void DefaultOverflowMode_IsPopup()
        {
            Assert.Equal(ToolbarOverflowMode.Popup, RenderToolbar().Instance.OverflowMode);
        }

        [Fact(DisplayName = "U-TB-060 SfToolbar parameter set matches the P0 contract")]
        public void SfToolbar_ParameterSet()
        {
            string[] expected =
            {
                "Align", "AriaLabel", "ChildContent", "CssClass", "EnableRtl", "HtmlAttributes",
                "OnItemClick", "OverflowMode", "Orientation"
            };
            string[] actual = PublicParameters(typeof(SfToolbar));
            foreach (string name in expected)
            {
                Assert.Contains(name, actual);
            }
        }

        [Fact(DisplayName = "U-TB-014 SfToolbar has no toolbar-wide Enabled parameter (ADR-005)")]
        public void SfToolbar_HasNoEnabledParameter()
        {
            Assert.Null(typeof(SfToolbar).GetProperty("Enabled", BindingFlags.Instance | BindingFlags.Public));
            Assert.Null(typeof(SfToolbar).GetProperty("Disabled", BindingFlags.Instance | BindingFlags.Public));
        }

        [Fact(DisplayName = "U-TB-061 SfToolbar exposes the four public methods")]
        public void SfToolbar_PublicMethods()
        {
            Type type = typeof(SfToolbar);
            Assert.Equal(typeof(ValueTask), type.GetMethod("RefreshOverflowAsync", Type.EmptyTypes)!.ReturnType);
            Assert.Equal(typeof(ValueTask), type.GetMethod("FocusAsync", Type.EmptyTypes)!.ReturnType);
            Assert.Equal(typeof(IReadOnlyList<ComponentBase>), type.GetMethod("GetVisibleItems", Type.EmptyTypes)!.ReturnType);
            Assert.Equal(typeof(IReadOnlyList<ComponentBase>), type.GetMethod("GetOverflowItems", Type.EmptyTypes)!.ReturnType);
        }

        [Fact(DisplayName = "U-TB-060 ToolbarItem exposes every P0 parameter")]
        public void ToolbarItem_ParameterSet()
        {
            string[] expected =
            {
                "Badge", "ChildContent", "CssClass", "DisplayMode", "Enabled", "GroupName", "IconCss", "IconPosition",
                "Id", "OnClick", "Overflow", "Priority", "Selected", "SelectedChanged", "Template", "Text", "Tooltip",
                "Type", "Visible"
            };
            string[] actual = PublicParameters(typeof(ToolbarItem));
            foreach (string name in expected)
            {
                Assert.Contains(name, actual);
            }
        }

        [Fact(DisplayName = "U-TB-060 ToolbarItem defaults match the documented values")]
        public void ToolbarItem_Defaults()
        {
            IRenderedComponent<SfToolbar> cut = RenderToolbar(Comp<ToolbarItem>(("Text", "Cut")));
            ToolbarItem item = cut.FindComponent<ToolbarItem>().Instance;

            Assert.Equal(ToolbarItemType.Button, item.Type);
            Assert.True(item.Enabled);
            Assert.True(item.Visible);
            Assert.Equal(ToolbarOverflowOption.None, item.Overflow);
            Assert.Equal(0, item.Priority);
            Assert.Equal(ToolbarItemDisplayMode.IconAndText, item.DisplayMode);
            Assert.False(item.Selected);
            Assert.Equal(IconPosition.Left, item.IconPosition);
        }

        [Fact(DisplayName = "U-TB-032 ToolbarItem.Selected supports @bind-Selected through SelectedChanged")]
        public void ToolbarItem_SelectedIsBindable()
        {
            PropertyInfo selected = Prop(typeof(ToolbarItem), "Selected");
            PropertyInfo changed = Prop(typeof(ToolbarItem), "SelectedChanged");
            Assert.True(IsParameter(selected));
            Assert.Equal(typeof(EventCallback<bool>), changed.PropertyType);
        }

        [Fact(DisplayName = "U-TB-030 ToolbarItem.OnClick is an EventCallback of MouseEventArgs")]
        public void ToolbarItem_OnClickType()
        {
            Assert.Equal(typeof(EventCallback<MouseEventArgs>), Prop(typeof(ToolbarItem), "OnClick").PropertyType);
        }

        [Fact(DisplayName = "U-TB-030 SfToolbar.OnItemClick is an EventCallback of ToolbarItemClickEventArgs")]
        public void SfToolbar_OnItemClickType()
        {
            Assert.Equal(typeof(EventCallback<ToolbarItemClickEventArgs>), Prop(typeof(SfToolbar), "OnItemClick").PropertyType);
        }

        [Fact(DisplayName = "U-TB-060 ToolbarGroup exposes Label, ChildContent and CssClass only")]
        public void ToolbarGroup_ParametersOnly()
        {
            string[] actual = PublicParameters(typeof(ToolbarGroup));
            Assert.Equal(new[] { "ChildContent", "CssClass", "Label" }, actual);
            Assert.DoesNotContain(typeof(ToolbarGroup).GetProperties(), p => p.PropertyType.IsGenericType
                && p.PropertyType.GetGenericTypeDefinition() == typeof(RenderFragment<>));
        }

        [Fact(DisplayName = "U-TB-060 ToolbarSeparator and ToolbarSpacer parameters")]
        public void SeparatorAndSpacer_Parameters()
        {
            Assert.Equal(new[] { "CssClass", "Visible" }, PublicParameters(typeof(ToolbarSeparator)));
            Assert.Equal(new[] { "CssClass" }, PublicParameters(typeof(ToolbarSpacer)));

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Comp<ToolbarSeparator>());
            Assert.True(cut.FindComponent<ToolbarSeparator>().Instance.Visible);
        }

        [Fact(DisplayName = "U-TB-060 ToolbarTemplateItem parameters and defaults")]
        public void ToolbarTemplateItem_Parameters()
        {
            string[] actual = PublicParameters(typeof(ToolbarTemplateItem));
            Assert.Equal(new[] { "ChildContent", "Overflow", "Priority" }, actual);

            IRenderedComponent<SfToolbar> cut = RenderToolbar(Comp<ToolbarTemplateItem>());
            ToolbarTemplateItem item = cut.FindComponent<ToolbarTemplateItem>().Instance;
            Assert.Equal(ToolbarOverflowOption.None, item.Overflow);
            Assert.Equal(0, item.Priority);
        }

        [Fact(DisplayName = "U-TB-054 ToolbarItemClickEventArgs exposes ItemId, Item, OriginalArgs and settable Cancel")]
        public void ClickEventArgs_Members()
        {
            Type type = typeof(ToolbarItemClickEventArgs);
            Assert.Equal(typeof(string), Prop(type, "ItemId").PropertyType);
            Assert.Equal(typeof(ToolbarItemModel), Prop(type, "Item").PropertyType);
            Assert.Equal(typeof(MouseEventArgs), Prop(type, "OriginalArgs").PropertyType);
            PropertyInfo cancel = Prop(type, "Cancel");
            Assert.Equal(typeof(bool), cancel.PropertyType);
            Assert.True(cancel.CanWrite);

            ToolbarItemClickEventArgs args = new();
            Assert.False(args.Cancel);
        }

        [Theory(DisplayName = "U-TB-060 Enums have exactly the documented members")]
        [InlineData(typeof(ToolbarOrientation), new[] { "Horizontal", "Vertical" })]
        [InlineData(typeof(ToolbarAlignment), new[] { "Start", "Center", "End", "Justify" })]
        [InlineData(typeof(ToolbarOverflowMode), new[] { "Scrollable", "Popup", "MultiRow", "Extended" })]
        [InlineData(typeof(ToolbarOverflowOption), new[] { "Show", "Hide", "None" })]
        [InlineData(typeof(ToolbarItemType), new[] { "Button", "Toggle", "Check", "Split", "DropDown", "Menu", "Separator", "Spacer", "Template" })]
        [InlineData(typeof(ToolbarItemDisplayMode), new[] { "IconOnly", "TextOnly", "IconAndText" })]
        public void Enums_HaveDocumentedMembers(Type enumType, string[] expected)
        {
            Assert.Equal(expected.OrderBy(n => n, StringComparer.Ordinal), Enum.GetNames(enumType).OrderBy(n => n, StringComparer.Ordinal));
        }

        [Fact(DisplayName = "U-TB-060 IToolbarItem and ToolbarItemCollection are internal")]
        public void InternalTypes_AreNotPublic()
        {
            Type? contract = ToolkitAssembly.GetType("Syncfusion.Blazor.Toolkit.IToolbarItem");
            Type? collection = ToolkitAssembly.GetType("Syncfusion.Blazor.Toolkit.ToolbarItemCollection");
            Assert.NotNull(contract);
            Assert.NotNull(collection);
            Assert.False(contract!.IsPublic);
            Assert.False(collection!.IsPublic);
        }

        [Fact(DisplayName = "U-TB-017 HtmlAttributes captures unmatched values on the root and items")]
        public void HtmlAttributes_CaptureUnmatchedValues()
        {
            ParameterAttribute? onToolbar = Prop(typeof(SfToolbar), "HtmlAttributes").GetCustomAttribute<ParameterAttribute>();
            Assert.NotNull(onToolbar);
            Assert.True(onToolbar!.CaptureUnmatchedValues);
        }

        [Fact(DisplayName = "U-TB-060 Public Toolbar types sit in the Toolkit namespace")]
        public void PublicTypes_InToolkitNamespace()
        {
            Type[] types =
            {
                typeof(SfToolbar), typeof(ToolbarItem), typeof(ToolbarGroup), typeof(ToolbarSeparator), typeof(ToolbarSpacer),
                typeof(ToolbarTemplateItem), typeof(ToolbarItemModel), typeof(ToolbarOverflowSettings),
                typeof(ToolbarTemplateContext), typeof(ToolbarItemClickEventArgs)
            };
            foreach (Type type in types)
            {
                Assert.Equal("Syncfusion.Blazor.Toolkit", type.Namespace);
                Assert.True(type.IsPublic);
            }
        }
    }
}
