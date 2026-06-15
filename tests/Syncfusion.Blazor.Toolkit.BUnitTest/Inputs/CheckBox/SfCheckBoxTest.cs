using Bunit;
using Xunit;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Buttons;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Inputs;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs
{
    public partial class SfCheckBoxTest : BunitTestContext
    {
        #region Default rendering

        // Verifies base checkbox DOM structure, initial state and click toggles.
        [Trait("SfCheckBox", "Basic")]
        [Fact(Timeout = 10000, DisplayName="UI Rendering")]
        public void Basic_UI_Rendering()
        {
            var renderedComponent = RenderComponent<Default>();
            var inputElement = renderedComponent.FindAll("input",true);
            Assert.True(inputElement[0].ParentElement.ParentElement.ClassList.Contains("e-checkbox-wrapper"));
            Assert.True(inputElement[0].ClassList.Contains("e-checkbox"));
            Assert.True(inputElement[1].NextElementSibling.ClassList.Contains("e-check"));
            Assert.Equal("",inputElement[0].ParentElement.TextContent.Trim());
            inputElement[1].Click();
            Assert.False(inputElement[1].NextElementSibling.ClassList.Contains("e-check"));
            inputElement[1].Click();
            Assert.True(inputElement[1].NextElementSibling.ClassList.Contains("e-check"));
        }

        // Verifies clicking individual checkboxes toggles their checked UI state.
        [Trait("SfCheckBox", "Basic")]
        [Fact(Timeout = 10000, DisplayName="Checked")]
        public void Basic_Checked()
        {
            var renderedComponent = RenderComponent<Default>();
            var inputElement = renderedComponent.FindAll("input",true);
            Assert.False(inputElement[0].NextElementSibling.ClassList.Contains("e-check"));
            inputElement[0].Click();
            Assert.True(inputElement[0].NextElementSibling.ClassList.Contains("e-check"));
            inputElement[0].Click();
            Assert.False(inputElement[0].NextElementSibling.ClassList.Contains("e-check"));
            Assert.True(inputElement[1].NextElementSibling.ClassList.Contains("e-check"));
            inputElement[1].Click();
            Assert.False(inputElement[1].NextElementSibling.ClassList.Contains("e-check"));
            inputElement[1].Click();
            Assert.True(inputElement[1].NextElementSibling.ClassList.Contains("e-check"));
        }

        // Verifies html id attribute is applied to both the input and its label for accessibility.
        [Fact(DisplayName = "HtmlAttributes id applied to input and label")]
        public void HtmlAttributes_Id_Applied_To_Input_And_Label()
        {
            var attributes = new Dictionary<string, object> { { "id", "custom-box" } };

            var renderedComponent = RenderComponent<SfCheckBox<bool>>(componentParameters => componentParameters
                .Add(component => component.Label, "Consent")
                .Add(component => component.HtmlAttributes, attributes));

            var inputElement = renderedComponent.Find("input");
            var labelElement = renderedComponent.Find("label");

            Assert.Equal("custom-box", inputElement.Id);
            Assert.Equal("custom-box", labelElement.GetAttribute("for"));
        }

        #endregion

        #region API names (public properties)

        // Verifies tri-state (EnableTriState) behavior sequence on user interaction.
        [Trait("SfCheckBox", "EnableTriState")]
        [Fact(Timeout = 10000, DisplayName = "EnableTriState")]
        public void EnableTriState()
        {
            var renderedComponent = RenderComponent<Inderminate>();
            var inputElement = renderedComponent.FindAll("input", true);
            inputElement[3].Click();
            Assert.False(inputElement[3].NextElementSibling.ClassList.Contains("e-check"));
            inputElement[3].Click();
            Assert.True(inputElement[3].NextElementSibling.ClassList.Contains("e-check"));
            inputElement[4].Click();
        }

        // Verifies two-way binding scenarios for nullable checked values.
        [Trait("SfCheckBox", "Two Way Binding")]
        [Fact(Timeout = 10000, DisplayName="Two way binding - Null Binding")]
        public void Two_Way_Binding_Null_Binding()
        {
            var renderedComponent = RenderComponent<Default>();
            var inputElement = renderedComponent.FindAll("input.e-checkbox", true);
            Assert.True(inputElement[4].NextElementSibling.ClassList.Contains("e-check"));
            inputElement[4].Click();
            Assert.False(inputElement[4].NextElementSibling.ClassList.Contains("e-check"));
            inputElement[4].Click();
            Assert.True(inputElement[4].NextElementSibling.ClassList.Contains("e-check"));
        }

        // Verifies normal two-way binding (bool) toggles update the UI.
        [Trait("SfCheckBox", "Binding")]
        [Fact(Timeout = 10000, DisplayName="Two way Binding")]
        public void Two_Way_Binding()
        {
            var renderedComponent = RenderComponent<Default>();
            var inputElement = renderedComponent.FindAll("input",true);
            Assert.True(inputElement[3].NextElementSibling.ClassList.Contains("e-check"));
            inputElement[3].Click();
            Assert.False(inputElement[3].NextElementSibling.ClassList.Contains("e-check"));
        }

        // Verifies nullable two-way binding toggling UI state.
        [Trait("SfCheckBox", "Binding")]
        [Fact(Timeout = 10000, DisplayName="Two way Null Binding")]
        public void Two_Way_Null_Binding()
        {
            var renderedComponent = RenderComponent<Default>();
            var inputElement = renderedComponent.FindAll("input",true);
            Assert.True(inputElement[5].NextElementSibling.ClassList.Contains("e-check"));
            inputElement[5].Click();
            Assert.False(inputElement[5].NextElementSibling.ClassList.Contains("e-check"));
        }

        // Verifies label text and label click mapping for default and custom labels.
        [Trait("SfCheckBox", "Label")]
        [Fact(Timeout = 10000, DisplayName="Default Label")]
        public void Default_Label()
        {
            var renderedComponent = RenderComponent<Label>();
            var inputElement = renderedComponent.FindAll("input", true);
            Assert.Equal("Checked as True", inputElement[0].NextElementSibling.NextElementSibling.TextContent.Trim());
        }

        // Verifies custom label 'for' attribute links to provided input id.
        [Trait("SfCheckBox", "Label")]
        [Fact(Timeout = 10000, DisplayName="Custom Label")]
        public void Custom_Label()
        {
            var renderedComponent = RenderComponent<Label>();
            var inputElement = renderedComponent.FindAll("input", true);
            Assert.Equal("custom", inputElement[2].ParentElement.GetAttribute("for"));
            Assert.Equal("custom", inputElement[2].Id);
        }

        // Verifies label position before/after is applied in markup.
        [Trait("SfCheckBox", "Label")]
        [Fact(Timeout = 10000, DisplayName="Label Position")]
        public void Label_Position()
        {
            var renderedComponent = RenderComponent<Label>();
            var inputElement = renderedComponent.FindAll("input",true);
            Assert.True(inputElement[0].ParentElement.Children[2].ClassList.Contains("e-label"));
            Assert.True(inputElement[1].ParentElement.Children[1].ClassList.Contains("e-label"));   
        }

        // label formatting regression
        [Trait("SfCheckBox", "Label")]
        [Fact(DisplayName = "Label formatting preserves inline markup")]
        public void SfCheckBox_LabelFormatting()
        {
            var renderedComponent = RenderComponent<Default>();
            var inputElement = renderedComponent.FindAll(".e-checkbox-wrapper",true);
            Assert.Equal("B",inputElement[5].NextElementSibling.Children[0].TagName);
        }

        // child-content anchor rendering
        [Trait("SfCheckBox", "ChildContent")]
        [Fact(DisplayName= "ChildContent renders anchor elements inside SfCheckBox")]
        public void SfCheckBox_ChildContent_RendersAnchor()
        {
            var renderedComponent = RenderComponent<ContentRendering>();
            var anchorElem = renderedComponent.FindAll("a.agreement-link",true);
            Assert.Equal("terms and conditions",anchorElem[0].TextContent.Trim());
        }

        #endregion

        #region Events

        // Verifies ValueChange event updates bound label text when user toggles the checkbox.
        [Trait("SfCheckBox", "Events")]
        [Fact(Timeout = 10000, DisplayName="Events - ValueChange")]
        public void Events_ValueChange()
        {
            var renderedComponent = RenderComponent<Event>();
            var checkboxElement = renderedComponent.FindAll("input",true);
            Assert.Equal("Checkbox state is: false", checkboxElement[1].NextElementSibling.NextElementSibling.TextContent.Trim());
            checkboxElement[1].Click();
            Assert.Equal("Checkbox state is: True",checkboxElement[1].NextElementSibling.NextElementSibling.TextContent.Trim());
        }

        // Verifies the Created EventCallback fires once on initial render and not again on parameter update.
        [Fact(DisplayName = "Created fires once on first render")]
        public void Created_Event_Fires_Only_Once()
        {
            var callCount = 0;

            var renderedComponent = RenderComponent<SfCheckBox<bool>>(componentParameters => componentParameters
                .Add(component => component.Created, EventCallback.Factory.Create<object>(this, _ => callCount++)));

            renderedComponent.WaitForAssertion(() => Assert.Equal(1, callCount));

            renderedComponent.SetParametersAndRender(componentParameters => componentParameters.Add(component => component.Label, "Updated"));
            Assert.Equal(1, callCount);
        }

        // Verifies IndeterminateChanged raises when entering indeterminate (tri-state).
        [Fact(DisplayName = "IndeterminateChanged emits true when entering tri-state state")]
        public void IndeterminateChanged_Fires_When_Entering_TriState()
        {
            var changes = new List<bool>();

            var renderedComponent = RenderComponent<SfCheckBox<bool?>>(componentParameters => componentParameters
                .Add(component => component.EnableTriState, true)
                .Add(component => component.Checked, true)
                .Add(component => component.IndeterminateChanged, EventCallback.Factory.Create<bool>(this, (bool value) => changes.Add(value))));

            renderedComponent.Find("input").Click();

            Assert.Equal(new[] { true }, changes);
        }

        // Verifies IndeterminateChanged raises false when clearing an indeterminate state.
        [Fact(DisplayName = "IndeterminateChanged emits false when clearing indeterminate")]
        public void IndeterminateChanged_Fires_When_Leaving_Indeterminate()
        {
            var changes = new List<bool>();

            var renderedComponent = RenderComponent<SfCheckBox<bool>>(componentParameters => componentParameters
                .Add(component => component.Indeterminate, true)
                .Add(component => component.IndeterminateChanged, EventCallback.Factory.Create<bool>(this, (bool value) => changes.Add(value))));

            renderedComponent.Find("input").Click();

            Assert.Equal(new[] { false }, changes);
        }

        #endregion

        #region PropertyChanges

        // Verifies property change of Disabled updates disabled CSS class on wrapper.
        [Trait("SfCheckBox", "PropertyChanges")]
        [Fact(Timeout = 10000, DisplayName="PropertyChanges_Disabled")]
        public void PropertyChanges_Disabled()
        {
            var renderedComponent = RenderComponent<Variants>();
            var inputElement = renderedComponent.FindAll("input",true);
            Assert.True(inputElement[2].ParentElement.ParentElement.ClassList.Contains("e-checkbox-disabled"));
            renderedComponent.SetParametersAndRender((nameof(Variants.disabled),false));
            Assert.False(inputElement[2].ParentElement.ParentElement.ClassList.Contains("e-checkbox-disabled"));
        }

        // Verifies programmatic Checked changes update the UI on re-render.
        [Trait("SfCheckBox", "PropertyChanges")]
        [Fact(Timeout = 10000, DisplayName="PropertyChanges_Checked")]
        public void PropertyChanges_Checked()
        {
            var renderedComponent = RenderComponent<Default>();
            var inputElement = renderedComponent.FindAll("input",true);
            renderedComponent.SetParametersAndRender((nameof(Default.IsChecked),true));
            Assert.True(inputElement[0].NextElementSibling.ClassList.Contains("e-check"));  
        }

        // Verifies programmatic Indeterminate toggles and interaction update UI and aria state.
        [Trait("SfCheckBox", "PropertyChanges")]
        [Fact(Timeout = 10000, DisplayName="PropertyChanges_Indeterminate")]
        public void PropertyChanges_Indeterminate()
        {
            var renderedComponent = RenderComponent<Inderminate>();
            var inputElement = renderedComponent.FindAll("input",true);            
            renderedComponent.SetParametersAndRender((nameof(Inderminate.IsChcked),true));
            inputElement[1].Click();
            Assert.True(inputElement[1].NextElementSibling.ClassList.Contains("e-check"));
            Assert.NotEqual("mixed",inputElement[1].ParentElement.ParentElement.GetAttribute("aria-checked"));
            renderedComponent.SetParametersAndRender((nameof(Inderminate.IsChcked),false));
            inputElement[1].Click();
            Assert.False(inputElement[1].NextElementSibling.ClassList.Contains("e-check"));
        }

        // Verifies toggling RTL parameter updates wrapper CSS.
        [Trait("SfCheckBox", "PropertyChanges")]
        [Fact(DisplayName = "RTL Enabled adds e-rtl")]
        public void RTL_Enabled_AddsClass()
        {
            Services.AddScoped(_ => new SyncfusionBlazorToolkitService(Options.Create(new GlobalOptions { EnableRtl = true })));
            var renderedComponent = RenderComponent<Variants>();
            var inputElement = renderedComponent.FindAll("input", true);
            Assert.True(inputElement[1].ParentElement.ParentElement.ClassList.Contains("e-rtl"));
        }

        [Fact(DisplayName = "RTL Disabled removes e-rtl")]
        public void RTL_Disabled_RemovesClass()
        {
            Services.AddScoped(_ => new SyncfusionBlazorToolkitService(Options.Create(new GlobalOptions { EnableRtl = false })));
            var renderedComponent = RenderComponent<Variants>();
            var inputElement = renderedComponent.FindAll("input", true);
            Assert.False(inputElement[1].ParentElement.ParentElement.ClassList.Contains("e-rtl"));
        }

        // Verifies CssClass parameter merges and updates class names on re-render.
        [Trait("SfCheckBox", "PropertyChanges")]
        [Fact(Timeout = 10000, DisplayName="PropertyChanges_CSSClass")]
        public void PropertyChanges_CSSClass()
        {
            var renderedComponent = RenderComponent<Variants>();
            var inputElement = renderedComponent.FindAll("input",true);
            Assert.Equal("e-checkbox-wrapper e-wrapper e-custom1 e-custom2",inputElement[0].ParentElement.ParentElement.ClassName);
            renderedComponent.SetParametersAndRender((nameof(Variants.cssClass),"e-custom3 e-custom4"));
            Assert.Equal("e-checkbox-wrapper e-wrapper e-custom3 e-custom4",inputElement[0].ParentElement.ParentElement.ClassName);
        }

        #endregion

        #region Customization

        // Size-related checks: small, bigger, and combined classes.
        [Trait("SfCheckBox", "Size")]
        [Fact(Timeout = 10000, DisplayName="Size Small")]
        public void Size_Small()
        {
            var renderedComponent = RenderComponent<Variants>();
            var inputElement = renderedComponent.FindAll("input",true);
            Assert.True(inputElement[3].ParentElement.ParentElement.ClassList.Contains("e-small"));
        }

        [Trait("SfCheckBox", "Size")]
        [Fact(Timeout = 10000, DisplayName="Size Bigger")]
        public void Size_Bigger()
        {
            var renderedComponent = RenderComponent<Variants>();
            var inputElement = renderedComponent.FindAll("input",true);
            Assert.True(inputElement[4].ParentElement.ParentElement.ClassList.Contains("e-bigger")); 
        }

        [Trait("SfCheckBox", "Size")]
        [Fact(Timeout = 10000, DisplayName="Size Bigger Small")]
        public void Size_BiggerSmall()
        {
            var renderedComponent = RenderComponent<Variants>();
            var inputElement = renderedComponent.FindAll("input",true);
            Assert.Equal("e-checkbox-wrapper e-wrapper e-bigger e-small",inputElement[5].ParentElement.ParentElement.ClassName);
            inputElement[7].Click();
        }

        // Form integration tests: model validation message and summary behavior.
        [Trait("SfCheckBox", "Form")]
        [Fact(Timeout = 10000, DisplayName="Form - Model and Validation Message")]
        public void Model_Validation_Message()
        {
            var renderedComponent = RenderComponent<FormCheck>();
            var inputElement = renderedComponent.FindAll("input",true);
            var formElements =renderedComponent.FindAll("form",true);
            Assert.Null(formElements[1].QuerySelector("div.validation-message"));
            inputElement[1].Click();
            inputElement[1].Click();
            var validationElements = renderedComponent.FindAll("div.validation-message",true);
            Assert.Equal("This form disallows unapproved ships.",validationElements[0].TextContent.Trim());     
            inputElement[1].Click();
            Assert.Null(formElements[1].QuerySelector("div.validation-message"));
        }

        [Trait("SfCheckBox", "Form")]
        [Fact(Timeout = 10000, DisplayName="Form - Model and Validation Summary")]
        public void Model_Validation_Summary()
        {
            var renderedComponent = RenderComponent<FormCheck>();
            var inputElement = renderedComponent.FindAll("input",true);
            var formElements =renderedComponent.FindAll("form",true);
            Assert.Null(formElements[2].QuerySelector("div.validation-message"));
            inputElement[2].Click();
            inputElement[2].Click();
            var validationElements = renderedComponent.FindAll("li.validation-message",true);
            Assert.Equal("This form disallows unapproved ships.",validationElements[0].TextContent.Trim());     
            inputElement[2].Click();
            Assert.Null(formElements[2].QuerySelector("div.validation-message"));
        }

        [Trait("SfCheckBox", "Form")]
        [Fact(Timeout = 10000, DisplayName="Form - Edit Context and Validation Message")]
        public void EditContext_Validation_Message()
        {
            var renderedComponent = RenderComponent<FormCheck>();
            var inputElement = renderedComponent.FindAll("input",true);
            var formElements =renderedComponent.FindAll("form",true);
            Assert.Null(formElements[3].QuerySelector("div.validation-message"));
            inputElement[3].Click();
            inputElement[3].Click();
            var validationElements = renderedComponent.FindAll("div.validation-message",true);
            Assert.Equal("This form disallows unapproved ships.",validationElements[0].TextContent.Trim());     
            inputElement[3].Click();
            Assert.Null(formElements[3].QuerySelector("div.validation-message"));
        }

        [Trait("SfCheckBox", "Form")]
        [Fact(Timeout = 10000, DisplayName="Form - Edit Context and Validation Summary")]
        public void EditContext_Validation_Summary()
        {
            var renderedComponent = RenderComponent<FormCheck>();
            var inputElement = renderedComponent.FindAll("input",true);
            var formElements =renderedComponent.FindAll("form",true);
            Assert.Null(formElements[4].QuerySelector("div.validation-message"));
            inputElement[4].Click();
            inputElement[4].Click();
            var validationElements = renderedComponent.FindAll("li.validation-message",true);
            Assert.Equal("This form disallows unapproved ships.",validationElements[0].TextContent.Trim());     
            inputElement[4].Click();
            Assert.Null(formElements[4].QuerySelector("div.validation-message"));
        }

        #endregion
    }
}
