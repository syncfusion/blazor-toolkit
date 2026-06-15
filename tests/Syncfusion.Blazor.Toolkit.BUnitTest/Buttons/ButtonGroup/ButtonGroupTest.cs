using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Bunit;
using Syncfusion.Blazor.Toolkit.Tests;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Syncfusion.Blazor.Toolkit.Buttons;

namespace Syncfusion.Blazor.Toolkit.Tests.Buttons
{
    public partial class ButtonGroup : BunitTestContext
    {
        private const int ExpectedButtonCount = 3;

        // ─────────────────────────────────────────────────────────────────────────
        // Basic
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Basic")]
        [Fact(Timeout = 10000, DisplayName = "Single Selection")]
        public void Single_Selection()
        {
            var defaultBGComponent = RenderComponent<DefaultBG>();
            var buttonGroupComponents = defaultBGComponent.FindComponents<SfButtonGroup>();
            // Index 1 = second SfButtonGroup in DefaultBG (Mode=Single)
            var singleGroupButtons = buttonGroupComponents[1].FindComponents<Button>();
            var labels = defaultBGComponent.FindAll("div.e-btn-group")[1].QuerySelectorAll("label");
            Assert.Equal(ExpectedButtonCount, singleGroupButtons.Count);
            for (var index = 0; index < ExpectedButtonCount; index++)
            {
                Assert.NotEqual("", labels[index].TextContent.Trim());
                Assert.False(singleGroupButtons[index].Instance.Selected);
            }
        }

        [Trait("ButtonGroup", "Basic")]
        [Fact(Timeout = 10000, DisplayName = "Multiple Selection")]
        public void Multiple_Selection()
        {
            var defaultBGComponent = RenderComponent<DefaultBG>();
            var buttonGroupComponents = defaultBGComponent.FindComponents<SfButtonGroup>();
            // Index 2 = third SfButtonGroup in DefaultBG (Mode=Multiple)
            var multipleGroupButtons = buttonGroupComponents[2].FindComponents<Button>();
            var group = defaultBGComponent.FindAll("div.e-btn-group")[2];
            var labels = group.QuerySelectorAll("label");
            var inputs = group.QuerySelectorAll("input");
            Assert.Equal(ExpectedButtonCount, multipleGroupButtons.Count);
            // Verify checkbox input type for Multiple mode
            Assert.Equal("checkbox", ((IHtmlInputElement)inputs[0]).Type);
            for (var index = 0; index < ExpectedButtonCount; index++)
            {
                Assert.NotEqual("", labels[index].TextContent.Trim());
                Assert.False(multipleGroupButtons[index].Instance.Selected);
            }
        }

        [Trait("ButtonGroup", "Basic")]
        [Fact(Timeout = 10000, DisplayName = "Multiple with Default Selection")]
        public void Multiple_Default_Selection()
        {
            var defaultBGComponent = RenderComponent<DefaultBG>();
            var buttonGroups = defaultBGComponent.FindAll("div.e-btn-group");
            // Index 3 = fourth group (Multiple, first button pre-selected)
            Assert.Contains("e-btn-group", buttonGroups[3].ClassName);

            var inputsInGroup = buttonGroups[3].QuerySelectorAll("input");
            Assert.Equal(ExpectedButtonCount, inputsInGroup.Length);
            Assert.Equal("checkbox", ((IHtmlInputElement)inputsInGroup[0]).Type);

            var labelsInGroup = buttonGroups[3].QuerySelectorAll("label");
            Assert.Equal(ExpectedButtonCount, labelsInGroup.Length);
            for (var index = 0; index < ExpectedButtonCount; index++)
            {
                Assert.NotEqual("", labelsInGroup[index].TextContent.Trim());
            }

            // Component-level default selection verification (single render, no duplicate)
            var buttonGroupComponents = defaultBGComponent.FindComponents<SfButtonGroup>();
            var defaultChildren = buttonGroupComponents[3].FindComponents<Button>();
            Assert.True(defaultChildren[0].Instance.Selected);
            Assert.False(defaultChildren[1].Instance.Selected);
            Assert.False(defaultChildren[2].Instance.Selected);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Icons
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Icons")]
        [Fact(Timeout = 10000, DisplayName = "Icon positions - Top/Right/Bottom/Left applied correctly")]
        public void IconPosition_AllPositions()
        {
            var iconBGComponent = RenderComponent<IconBG>();
            // Index 2 = third group in IconBG (four buttons with explicit icon positions)
            var buttons = iconBGComponent.FindAll("div.e-btn-group")[2].QuerySelectorAll(".e-btn");
            Assert.Equal(4, buttons.Length);

            // Search by class anywhere inside the button element (not Children[0])
            // because Right/Bottom icons are rendered AFTER the content node
            Assert.NotNull(buttons[0].QuerySelector(".e-icon-top"));
            Assert.NotNull(buttons[1].QuerySelector(".e-icon-right"));
            Assert.NotNull(buttons[2].QuerySelector(".e-icon-bottom"));
            Assert.NotNull(buttons[3].QuerySelector(".e-icon-left"));
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Others (Disabled, RTL, Orientation, Nesting)
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Others")]
        [Fact(Timeout = 10000, DisplayName = "Disabled")]
        public void DisabledGroup_Others()
        {
            var othersBGComponent = RenderComponent<OthersBG>();
            var groupButtons = othersBGComponent.FindAll("div.e-btn-group")[0].QuerySelectorAll(".e-btn");

            Assert.NotNull(groupButtons[0]);
            Assert.NotNull(groupButtons[1]);
            Assert.NotNull(groupButtons[2]);
            Assert.False(groupButtons[0].IsDisabled());
            Assert.True(groupButtons[1].IsDisabled());
            Assert.True(groupButtons[2].IsDisabled());
        }

        [Trait("ButtonGroup", "Disabled")]
        [Fact(Timeout = 10000, DisplayName = "Single button disabled")]
        public void SingleButton_Disabled()
        {
            var othersBGComponent = RenderComponent<OthersBG>();
            othersBGComponent.WaitForAssertion(() => Assert.True(othersBGComponent.FindAll("div.e-btn-group").Count > 1));

            var buttonGroupComponents = othersBGComponent.FindComponents<SfButtonGroup>();
            // Index 1 = second group in OthersBG (Single mode; middle button disabled)
            var targetGroup  = buttonGroupComponents[1];
            var childButtons = targetGroup.FindComponents<Button>();

            Assert.True(childButtons[1].Instance.Disabled);

            // Click disabled label — Selected must remain false (fix: was incorrectly Assert.True)
            childButtons[1].Find("label").Click();
            Assert.False(targetGroup.FindComponents<Button>()[1].Instance.Selected);

            // Click enabled label — it should become selected
            childButtons[2].Find("label").Click();
            Assert.True(targetGroup.FindComponents<Button>()[2].Instance.Selected);
        }

        [Trait("ButtonGroup", "Others")]
        [Fact(Timeout = 10000, DisplayName = "RTL")]
        public void RTL()
        {
            var othersBGComponent = RenderComponent<OthersBG>();
            var buttonGroups = othersBGComponent.FindAll("div.e-btn-group");
            Assert.True(buttonGroups[2].ClassList.Contains("e-rtl"));
        }
        [Trait("ButtonGroup", "Orientation")]
        [Fact(Timeout = 10000, DisplayName = "Orientation - Vertical")]
        public void Orientation_Vertical()
        {
            var othersBGComponent = RenderComponent<OthersBG>();
            var buttonGroups = othersBGComponent.FindAll("div.e-btn-group");
            Assert.True(buttonGroups[3].ClassList.Contains("e-vertical"));
        }
        // ─────────────────────────────────────────────────────────────────────────
        // Style
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Style")]
        [Fact(Timeout = 10000, DisplayName = "PrimaryButtonStyle")]
        public void Style_Primary()
        {
            var styleComponent = RenderComponent<Style>();
            var buttons = styleComponent.FindAll("div.e-btn-group")[0].QuerySelectorAll(".e-btn");
            Assert.Equal(ExpectedButtonCount, buttons.Length);
            foreach (var btn in buttons)
                Assert.True(btn.ClassList.Contains("e-primary"), $"Expected e-primary on '{btn.ClassName}'");
        }

        [Trait("ButtonGroup", "Style")]
        [Fact(Timeout = 10000, DisplayName = "SuccessButtonStyle")]
        public void Style_Success()
        {
            var styleComponent = RenderComponent<Style>();
            var buttons = styleComponent.FindAll("div.e-btn-group")[1].QuerySelectorAll(".e-btn");
            Assert.Equal(ExpectedButtonCount, buttons.Length);
            foreach (var btn in buttons)
                Assert.True(btn.ClassList.Contains("e-success"), $"Expected e-success on '{btn.ClassName}'");
        }

        [Trait("ButtonGroup", "Style")]
        [Fact(Timeout = 10000, DisplayName = "MixedButtonStyle")]
        public void Style_Mixed()
        {
            var styleComponent = RenderComponent<Style>();
            var buttons = styleComponent.FindAll("div.e-btn-group")[2].QuerySelectorAll(".e-btn");
            Assert.Equal(ExpectedButtonCount, buttons.Length);
            Assert.True(buttons[0].ClassList.Contains("e-info"),    $"Expected e-info on '{buttons[0].ClassName}'");
            Assert.True(buttons[1].ClassList.Contains("e-warning"), $"Expected e-warning on '{buttons[1].ClassName}'");
            Assert.True(buttons[2].ClassList.Contains("e-danger"),  $"Expected e-danger on '{buttons[2].ClassName}'");
        }
        [Trait("ButtonGroup", "Type")]
        [Fact(Timeout = 10000, DisplayName = "Default Type")]
        public void Type_Default()
        {
            var typeComponent = RenderComponent<Type>();
            var buttonGroups = typeComponent.FindAll("div.e-btn-group");
            Assert.Equal(ExpectedButtonCount, buttonGroups[0].QuerySelectorAll(".e-btn").Length);
            Assert.False(buttonGroups[0].ClassList.Contains("e-outline"));
        }

        [Trait("ButtonGroup", "Type")]
        [Fact(Timeout = 10000, DisplayName = "Outline Type")]
        public void Type_Outline()
        {
            var typeComponent = RenderComponent<Type>();
            var buttonGroups = typeComponent.FindAll("div.e-btn-group");
            Assert.Equal(ExpectedButtonCount, buttonGroups[1].QuerySelectorAll(".e-btn").Length);
            Assert.True(buttonGroups[1].ClassList.Contains("e-outline"));
        }

        [Trait("ButtonGroup", "Others")]
        [Fact(Timeout = 10000, DisplayName = "Nesting-DropdownButton")]
        public void Nesting_DropdownButton()
        {
            var dropdownComponent = RenderComponent<Dropdown>();
            var buttonGroups = dropdownComponent.FindAll("div.e-btn-group");
            Assert.Equal(2, buttonGroups[0].QuerySelectorAll(".e-btn").Length);
        }

        [Trait("ButtonGroup", "Others")]
        [Fact(Timeout = 10000, DisplayName = "Nesting-SplitButton")]
        public void Nesting_SplitButton()
        {
            var dropdownComponent = RenderComponent<Dropdown>();
            var buttonGroups = dropdownComponent.FindAll("div.e-btn-group");
            Assert.Equal(2, buttonGroups[1].QuerySelectorAll(".e-btn").Length);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Clicking
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Clicking")]
        [Fact(Timeout = 10000, DisplayName = "Clicking - Component level")]
        public void Clicking_ComponentLevel()
        {
            var defaultBGComponent    = RenderComponent<DefaultBG>();
            var buttonGroupComponents = defaultBGComponent.FindComponents<SfButtonGroup>();

            // Single mode group (index 1)
            var singleGroupComponent = buttonGroupComponents[1];
            var singleGroupButtons   = singleGroupComponent.FindComponents<Button>();
            Assert.True(singleGroupButtons.Count > 2, "Expected at least 3 child buttons in single group.");
            singleGroupButtons[2].Find("label").Click();
            Assert.True(singleGroupComponent.FindComponents<Button>()[2].Instance.Selected);

            // Multiple mode group (index 2)
            var multiGroupComponent = buttonGroupComponents[2];
            var multiGroupButtons   = multiGroupComponent.FindComponents<Button>();
            Assert.True(multiGroupButtons.Count > 2, "Expected at least 3 child buttons in multiple group.");
            multiGroupButtons[2].Find("label").Click();
            Assert.True(multiGroupComponent.FindComponents<Button>()[2].Instance.Selected);
        }

        // Note: Multiple_LabelClick_TogglesSelected removed — fully subsumed by Multiple_Toggle_OnOff.

        [Trait("ButtonGroup", "Clicking")]
        [Fact(Timeout = 10000, DisplayName = "Multiple - Toggle on/off")]
        public void Multiple_Toggle_OnOff()
        {
            var defaultBGComponent    = RenderComponent<DefaultBG>();
            var buttonGroupComponents = defaultBGComponent.FindComponents<SfButtonGroup>();
            var multiGroup   = buttonGroupComponents[2];
            var childButtons = multiGroup.FindComponents<Button>();
            Assert.True(childButtons.Count > 0);

            // Toggle on
            childButtons[0].Find("label").Click();
            Assert.True(multiGroup.FindComponents<Button>()[0].Instance.Selected);

            // Toggle off
            multiGroup.FindComponents<Button>()[0].Find("label").Click();
            Assert.False(multiGroup.FindComponents<Button>()[0].Instance.Selected);
        }

        [Trait("ButtonGroup", "Clicking")]
        [Fact(Timeout = 10000, DisplayName = "Single - Exclusivity across")]
        public void Single_Exclusivity_ComponentBased()
        {
            var defaultBGComponent    = RenderComponent<DefaultBG>();
            var buttonGroupComponents = defaultBGComponent.FindComponents<SfButtonGroup>();
            var singleGroup  = buttonGroupComponents[1];
            var childButtons = singleGroup.FindComponents<Button>();
            Assert.True(childButtons.Count >= 3, "Expected at least three child buttons in single group.");

            // Initial: all unselected
            foreach (var btn in childButtons)
                Assert.False(btn.Instance.Selected);

            // Select first
            childButtons[0].Find("label").Click();
            var r1 = singleGroup.FindComponents<Button>();
            Assert.True(r1[0].Instance.Selected);
            Assert.False(r1[1].Instance.Selected);
            Assert.False(r1[2].Instance.Selected);

            // Select second — first must uncheck
            r1[1].Find("label").Click();
            var r2 = singleGroup.FindComponents<Button>();
            Assert.False(r2[0].Instance.Selected);
            Assert.True(r2[1].Instance.Selected);
            Assert.False(r2[2].Instance.Selected);

            // All radios in this group must share one name
            var radios = singleGroup.FindAll("input[type=radio]");
            var names  = radios.Select(r => ((IHtmlInputElement)r).Name).Distinct().ToList();
            Assert.Single(names);
        }

        // Note: Single_Exclusivity_ComponentBased_Async removed.
        // It was functionally identical to the sync variant above and used
        // 'async void' which silently swallows exceptions in xUnit.
        // The async scenario is now covered by Single_Exclusivity_Async below,
        // which uses the correct 'async Task' signature.

        [Trait("ButtonGroup", "Clicking")]
        [Fact(Timeout = 10000, DisplayName = "Single - Exclusivity across labels (async Task)")]
        public async Task Single_Exclusivity_Async()
        {
            var defaultBGComponent    = RenderComponent<DefaultBG>();
            var buttonGroupComponents = defaultBGComponent.FindComponents<SfButtonGroup>();
            var singleGroup  = buttonGroupComponents[1];
            var childButtons = singleGroup.FindComponents<Button>();
            Assert.True(childButtons.Count >= 3);

            // Select first
            childButtons[0].Find("label").Click();
            var r1 = singleGroup.FindComponents<Button>();
            Assert.True(r1[0].Instance.Selected);
            Assert.False(r1[1].Instance.Selected);

            // Select second — first must uncheck
            r1[1].Find("label").Click();
            var r2 = singleGroup.FindComponents<Button>();
            Assert.False(r2[0].Instance.Selected);
            Assert.True(r2[1].Instance.Selected);

            await Task.CompletedTask;
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Basic (continued)
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Basic")]
        [Fact(Timeout = 10000, DisplayName = "Single - Radio inputs share same name")]
        public void Single_RadioInputs_ShareName()
        {
            var defaultBGComponent = RenderComponent<DefaultBG>();
            var singleSelectionGroup = defaultBGComponent.FindAll("div.e-btn-group")[1];
            var radioInputs = singleSelectionGroup.QuerySelectorAll("input");
            Assert.Equal(ExpectedButtonCount, radioInputs.Length);
            var firstName  = ((IHtmlInputElement)radioInputs[0]).Name;
            var secondName = ((IHtmlInputElement)radioInputs[1]).Name;
            var thirdName  = ((IHtmlInputElement)radioInputs[2]).Name;
            Assert.Equal(firstName, secondName);
            Assert.Equal(secondName, thirdName);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Binding
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Binding")]
        [Fact(Timeout = 10000, DisplayName = "Binding - Single mode updates backing fields")]
        public void Binding_SingleMode_Backings()
        {
            var bindingComponent = RenderComponent<Binding>();
            // Index 1 = second group in Binding.razor (Single mode)
            var labels   = bindingComponent.FindAll("div.e-btn-group")[1].QuerySelectorAll(".e-btn");
            var instance = bindingComponent.Instance;

            // Reflection is justified here: private backing fields on the fixture
            // are the only way to verify @bind-Selected propagation from the child component.
            var trueField  = instance.GetType().GetField("_isTrueChecked",  System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var falseField = instance.GetType().GetField("_isFalseChecked", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.NotNull(trueField);
            Assert.NotNull(falseField);

            Assert.False(((bool?)trueField!.GetValue(instance)).GetValueOrDefault());
            Assert.False(((bool?)falseField!.GetValue(instance)).GetValueOrDefault());

            // Click "True"
            labels[0].Click();
            bindingComponent.WaitForAssertion(() => Assert.True(((bool?)trueField.GetValue(instance)).GetValueOrDefault()));
            Assert.False(((bool?)falseField.GetValue(instance)).GetValueOrDefault());

            // Click "False"
            labels[1].Click();
            bindingComponent.WaitForAssertion(() => Assert.True(((bool?)falseField.GetValue(instance)).GetValueOrDefault()));
            Assert.False(((bool?)trueField.GetValue(instance)).GetValueOrDefault());
        }

        [Trait("ButtonGroup", "Binding")]
        [Fact(Timeout = 10000, DisplayName = "Binding - Multiple mode updates backing fields")]
        public void Binding_MultipleMode_Backings()
        {
            var bindingComponent = RenderComponent<Binding>();
            // Index 0 = first group in Binding.razor (Multiple mode)
            var labels   = bindingComponent.FindAll("div.e-btn-group")[0].QuerySelectorAll(".e-btn");
            var instance = bindingComponent.Instance;

            var boldField   = instance.GetType().GetField("_boldChecked",   System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var italicField = instance.GetType().GetField("_italicChecked", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.NotNull(boldField);
            Assert.NotNull(italicField);

            Assert.False(((bool?)boldField!.GetValue(instance)).GetValueOrDefault());
            Assert.False(((bool?)italicField!.GetValue(instance)).GetValueOrDefault());

            // Toggle Bold on
            labels[0].Click();
            bindingComponent.WaitForAssertion(() => Assert.True(((bool?)boldField.GetValue(instance)).GetValueOrDefault()));
            Assert.False(((bool?)italicField.GetValue(instance)).GetValueOrDefault());

            // Toggle Italic on — Bold stays on
            labels[1].Click();
            bindingComponent.WaitForAssertion(() => Assert.True(((bool?)italicField.GetValue(instance)).GetValueOrDefault()));
            Assert.True(((bool?)boldField.GetValue(instance)).GetValueOrDefault());

            // Toggle Bold off — Italic stays on
            labels[0].Click();
            bindingComponent.WaitForAssertion(() => Assert.False(((bool?)boldField.GetValue(instance)).GetValueOrDefault()));
        }

        // ─────────────────────────────────────────────────────────────────────────
        // NEW — Mode=Default rendering (previously missing)
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Basic")]
        [Fact(Timeout = 10000, DisplayName = "Mode=Default renders SfButton elements")]
        public void ModeDefault_RendersSfButton()
        {
            var extraBGComponent = RenderComponent<ExtraBG>();
            // Group 0: Mode=Default — Button emits SfButton (button.e-btn), no input/label pairs
            var defaultGroup = extraBGComponent.FindAll("div.e-btn-group")[0];
            var sfButtons    = defaultGroup.QuerySelectorAll(".e-btn");
            Assert.Equal(ExpectedButtonCount, sfButtons.Length);
            Assert.Empty(defaultGroup.QuerySelectorAll("input"));
            Assert.Empty(defaultGroup.QuerySelectorAll("label"));
        }

        // ─────────────────────────────────────────────────────────────────────────
        // NEW — Content property (previously missing)
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Basic")]
        [Fact(Timeout = 10000, DisplayName = "Content property renders button text")]
        public void Content_RendersButtonText()
        {
            var extraBGComponent = RenderComponent<ExtraBG>();
           
            // Group 1: Mode=Default, buttons use Content= property
            var buttons = extraBGComponent.FindAll("div.e-btn-group")[1].QuerySelectorAll(".e-btn");
            Assert.Equal(ExpectedButtonCount, buttons.Length);
            Assert.Contains("Left",   buttons[0].TextContent);
            Assert.Contains("Center", buttons[1].TextContent);
            Assert.Contains("Right",  buttons[2].TextContent);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // NEW — ChildContent overrides Content (previously missing)
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Basic")]
        [Fact(Timeout = 10000, DisplayName = "ChildContent overrides Content property")]
        public void ChildContent_OverridesContent()
        {
            var extraBGComponent = RenderComponent<ExtraBG>();
            // Group 2: both ChildContent (<span>Child</span>) and Content="Fallback" provided
            var buttons = extraBGComponent.FindAll("div.e-btn-group")[2].QuerySelectorAll(".e-btn");
            Assert.Equal(ExpectedButtonCount, buttons.Length);
            foreach (var btn in buttons)
            {
                Assert.Contains("Child",    btn.InnerHtml);
                Assert.DoesNotContain("Fallback", btn.InnerHtml);
            }
        }

        // ─────────────────────────────────────────────────────────────────────────
        // NEW — Empty ChildContent + Content renders empty button (previously missing)
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Basic")]
        [Fact(Timeout = 10000, DisplayName = "Empty ChildContent and Content renders empty button")]
        public void EmptyContent_RendersEmptyButton()
        {
            var extraBGComponent = RenderComponent<ExtraBG>();
            // Group 3: no Content or ChildContent on any button
            var buttons = extraBGComponent.FindAll("div.e-btn-group")[3].QuerySelectorAll(".e-btn");
            Assert.Equal(ExpectedButtonCount, buttons.Length);
            foreach (var btn in buttons)
                Assert.Equal("", btn.TextContent.Trim());
        }

        // ─────────────────────────────────────────────────────────────────────────
        // NEW — IsToggle passed to SfButton in Default mode (previously missing)
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Basic")]
        [Fact(Timeout = 10000, DisplayName = "IsToggle property is passed to SfButton")]
        public void IsToggle_PassedToSfButton()
        {
            var extraBGComponent      = RenderComponent<ExtraBG>();
            var buttonGroupComponents = extraBGComponent.FindComponents<SfButtonGroup>();
            // Group 4: first child IsToggle=true, second IsToggle=false
            var group4Buttons = buttonGroupComponents[4].FindComponents<Button>();
            Assert.Equal(2, group4Buttons.Count);
            Assert.True(group4Buttons[0].Instance.IsToggle);
            Assert.False(group4Buttons[1].Instance.IsToggle);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // NEW — Value property overrides input value attribute (previously missing)
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Basic")]
        [Fact(Timeout = 10000, DisplayName = "Value property sets input value attribute")]
        public void Value_SetsInputValueAttribute()
        {
            var extraBGComponent = RenderComponent<ExtraBG>();
            // Group 5: Mode=Single, explicit Value= per button
            var inputs = extraBGComponent.FindAll("div.e-btn-group")[5].QuerySelectorAll("input");
            Assert.Equal(ExpectedButtonCount, inputs.Length);
            Assert.Equal("val-a", ((IHtmlInputElement)inputs[0]).Value);
            Assert.Equal("val-b", ((IHtmlInputElement)inputs[1]).Value);
            Assert.Equal("val-c", ((IHtmlInputElement)inputs[2]).Value);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // NEW — HtmlAttributes applied in selection mode (previously missing)
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Basic")]
        [Fact(Timeout = 10000, DisplayName = "HtmlAttributes (title) applied in selection mode")]
        public void HtmlAttributes_AppliedInSelectionMode()
        {
            var extraBGComponent = RenderComponent<ExtraBG>();
            // Group 6: Mode=Multiple, each button has title= via HtmlAttributes
            var labels = extraBGComponent.FindAll("div.e-btn-group")[6].QuerySelectorAll("label");
            Assert.Equal(2, labels.Length);
            Assert.Equal("Button One", labels[0].GetAttribute("title"));
            Assert.Equal("Button Two", labels[1].GetAttribute("title"));
        }

        // ─────────────────────────────────────────────────────────────────────────
        // NEW — HtmlAttributes applied in Default mode (previously missing)
        // ─────────────────────────────────────────────────────────────────────────

        [Trait("ButtonGroup", "Basic")]
        [Fact(Timeout = 10000, DisplayName = "HtmlAttributes (data-testid) applied in Default mode")]
        public void HtmlAttributes_AppliedInDefaultMode()
        {
            var extraBGComponent = RenderComponent<ExtraBG>();
            // Group 7: Mode=Default, button has data-testid= via HtmlAttributes
            var btn = extraBGComponent.FindAll("div.e-btn-group")[7].QuerySelector(".e-btn");
            Assert.NotNull(btn);
            Assert.Equal("btn-attr-test", btn!.GetAttribute("data-testid"));
        }
    }
}