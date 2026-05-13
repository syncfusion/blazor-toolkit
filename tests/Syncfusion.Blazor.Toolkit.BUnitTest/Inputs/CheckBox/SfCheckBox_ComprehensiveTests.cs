using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Xunit;
using Syncfusion.Blazor.Toolkit.Buttons;
using Syncfusion.Blazor.Toolkit.Inputs;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Tests.Buttons
{
    public class SfCheckBox_ComprehensiveTests : BunitTestContext
    {
        #region Default Rendering

        // Verifies wrapper label gets default aria-label when no Label or ChildContent provided.
        [Trait("SfCheckBox", "Rendering")]
        [Fact(DisplayName = "No Label/Content => aria-label defaults to 'checkbox'")]
        public void Default_AriaLabel_When_NoLabel_And_NoContent()
        {
            var renderedComponent = RenderComponent<SfCheckBox<bool>>();
            var wrapperElement = renderedComponent.Find(".e-checkbox-wrapper");
            var labelElement = wrapperElement.QuerySelector("label");
            Assert.NotNull(labelElement);
            Assert.Equal("checkbox", labelElement.GetAttribute("aria-label"));
        }

        #endregion

        #region API names (public properties)

        // Verifies title moves to wrapper, aria-label moves to label, and removed from input (HtmlAttributes mapping).
        [Trait("SfCheckBox", "HtmlAttributes")]
        [Fact(DisplayName = "HtmlAttributes: title -> wrapper, aria-label -> label, removed from input")]
        public void HtmlAttributes_Applied_Correctly()
        {
            var attribute = new Dictionary<string, object>
            {
                { "title", "Checkbox tooltip" },
                { "aria-label", "CustomLabel" }
            };
            var renderedComponent = RenderComponent<SfCheckBox<bool>>(componentParameters => componentParameters
                .Add(component => component.HtmlAttributes, attribute)
            );

            var wrapperElement = renderedComponent.Find(".e-checkbox-wrapper");
            Assert.Equal("Checkbox tooltip", wrapperElement.GetAttribute("title"));

            var labelElement = wrapperElement.QuerySelector("label");
            Assert.Equal("CustomLabel", labelElement.GetAttribute("aria-label"));

            var inputElement = wrapperElement.QuerySelector("input");
            Assert.Null(inputElement.GetAttribute("aria-label"));
        }

        // Verifies readonly HtmlAttribute is applied to wrapper element (AdditionalAttributes behavior).
        [Trait("SfCheckBox", "HtmlAttributes")]
        [Fact(DisplayName = "HtmlAttributes: readonly -> wrapper attribute")]
        public void HtmlAttributes_Readonly_Added_To_Wrapper()
        {
            var attributes = new Dictionary<string, object>
            {
                { "readonly", "readonly" }
            };
            var renderedComponent = RenderComponent<SfCheckBox<bool>>(componentParameters => componentParameters
                .Add(component => component.HtmlAttributes, attributes)
            );

            var wrapperElement = renderedComponent.Find(".e-checkbox-wrapper");
            Assert.Equal("readonly", wrapperElement.GetAttribute("readonly"));
        }

        // Verifies Disabled property reflects on wrapper aria-disabled attribute.
        [Trait("SfCheckBox", "Accessibility")]
        [Fact(DisplayName = "aria-disabled reflects Disabled state")]
        public void AriaDisabled_Reflects_Disabled()
        {
            var renderedComponent = RenderComponent<SfCheckBox<bool>>(componentParameters => componentParameters.Add(component => component.Disabled, true));
            var wrapperElement = renderedComponent.Find(".e-checkbox-wrapper");
            Assert.Equal("true", wrapperElement.GetAttribute("aria-disabled"));

            renderedComponent.SetParametersAndRender(componentParameters => componentParameters.Add(component => component.Disabled, false));
            Assert.Equal("false", wrapperElement.GetAttribute("aria-disabled"));
        }

        // Verifies click does not toggle when Disabled = true (disabled state behavior).
        [Trait("SfCheckBox", "Disabled")]
        [Fact(DisplayName = "Click ignored when Disabled = true")]
        public void Disabled_Click_Does_Not_Toggle()
        {
            var renderedComponent = RenderComponent<SfCheckBox<bool>>(componentParameters => componentParameters
                .Add(component => component.Checked, true)
                .Add(component => component.Disabled, true)
            );

            var inputElement = renderedComponent.Find("input");
            var frameElement = inputElement.NextElementSibling; // .e-frame
            Assert.Contains("e-check", frameElement.ClassList);

            inputElement.Click();
            // Still checked because disabled blocks toggle
            Assert.Contains("e-check", frameElement.ClassList);
        }

        // Verifies tri-state click cycle (checked -> indeterminate -> unchecked -> checked) when EnableTriState=true.
        [Trait("SfCheckBox", "TriState")]
        [Fact(DisplayName = "TriState click cycle: checked -> indeterminate -> unchecked")]
        public void TriState_Click_Cycle()
        {
            // Start as checked
            var renderedComponent = RenderComponent<SfCheckBox<bool?>>(componentParameters => componentParameters
                .Add(component => component.EnableTriState, true)
                .Add(component => component.Checked, true)
            );

            var inputElement = renderedComponent.Find("input");
            var frameElement = inputElement.NextElementSibling;
            Assert.Contains("e-check", frameElement.ClassList);

            // checked -> indeterminate
            inputElement.Click();
            frameElement = inputElement.NextElementSibling;
            Assert.DoesNotContain("e-check", frameElement.ClassList);
            Assert.Contains("e-stop", frameElement.ClassList);

            // indeterminate -> unchecked
            inputElement.Click();
            frameElement = inputElement.NextElementSibling;
            Assert.DoesNotContain("e-check", frameElement.ClassList);
            Assert.DoesNotContain("e-stop", frameElement.ClassList);

            // unchecked -> checked
            inputElement.Click();
            frameElement = inputElement.NextElementSibling;
            Assert.Contains("e-check", frameElement.ClassList);
        }

        // Verifies non-tristate Indeterminate=true clears on click and toggles to checked (two-state indeterminate behavior).
        [Trait("SfCheckBox", "Indeterminate")]
        [Fact(DisplayName = "Non-tristate: Indeterminate true -> click clears and toggles to current Checked")]
        public void Indeterminate_TwoState_Behavior()
        {
            var renderedComponent = RenderComponent<SfCheckBox<bool>>(componentParameters => componentParameters
                .Add(component => component.Checked, true)
                .Add(component => component.Indeterminate, true)
            );

            var inputElement = renderedComponent.Find("input");
            var frameElement = inputElement.NextElementSibling;
            Assert.Contains("e-stop", frameElement.ClassList);

            inputElement.Click();
            frameElement = inputElement.NextElementSibling;
            // Should become checked and clear indeterminate
            Assert.Contains("e-check", frameElement.ClassList);
            Assert.DoesNotContain("e-stop", frameElement.ClassList);
        }

        // Verifies TChecked=byte produces 0/1 values via ValueChange on user clicks.
        [Trait("SfCheckBox", "TypeConversion")]
        [Fact(DisplayName = "TChecked=byte: ValueChange provides 0/1 values")]
        public void Byte_TChecked_ValueChange()
        {
            byte? last = null;
            var renderedComponent = RenderComponent<SfCheckBox<byte>>(componentParameters => componentParameters
                .Add(component => component.ValueChange, EventCallback.Factory.Create<CheckedChangeEventArgs<byte>>(this, (CheckedChangeEventArgs<byte> e) => last = e.Checked))
            );

            var inputElement = renderedComponent.Find("input");
            inputElement.Click();
            Assert.Equal((byte)1, last);
            inputElement.Click();
            Assert.Equal((byte)0, last);
        }

        // Verifies TChecked=byte? with tri-state cycles 1 -> null -> 0 and raises ValueChange accordingly.
        [Trait("SfCheckBox", "TypeConversion")]
        [Fact(DisplayName = "TChecked=byte? + TriState: ValueChange provides 1 -> null -> 0 cycle")]
        public void NullableByte_TriState_ValueChange()
        {
            byte? last = null;
            var renderedComponent = RenderComponent<SfCheckBox<byte?>>(componentParameters => componentParameters
                .Add(component => component.EnableTriState, true)
                .Add(component => component.Checked, (byte?)1)
                .Add(component => component.ValueChange, EventCallback.Factory.Create<CheckedChangeEventArgs<byte?>>(this, (CheckedChangeEventArgs<byte?> e) => last = e.Checked))
            );

            var inputElement = renderedComponent.Find("input");

            // 1 -> null
            inputElement.Click();
            Assert.Null(last);

            // null -> 0
            inputElement.Click();
            Assert.Equal((byte)0, last);

            // 0 -> 1
            inputElement.Click();
            Assert.Equal((byte)1, last);
        }

        // Verifies Indeterminate property results in an 'indeterminate' input attribute on initial render.
        [Trait("SfCheckBox", "Attributes")]
        [Fact(DisplayName = "input has 'indeterminate' attribute when Indeterminate true")]
        public void Input_Indeterminate_Attribute_Rendered()
        {
            var renderedComponent = RenderComponent<SfCheckBox<bool>>(componentParameters => componentParameters.Add(component => component.Indeterminate, true));
            var inputElement = renderedComponent.Find("input");
            Assert.NotNull(inputElement.GetAttribute("indeterminate"));
        }

        // Verifies Name and Value public properties render as input attributes.
        [Trait("SfCheckBox", "Attributes")]
        [Fact(DisplayName = "Name and Value attributes rendered on input")]
        public void Name_Value_Attributes()
        {
            var renderedComponent = RenderComponent<SfCheckBox<bool>>(componentParameters => componentParameters
                .Add(component => component.Name, "status")
                .Add(component => component.Value, "on")
            );
            var inputElement = renderedComponent.Find("input");
            Assert.Equal("status", inputElement.GetAttribute("name"));
            Assert.Equal("on", inputElement.GetAttribute("value"));
        }

        #endregion

        #region Events

        // Verifies ValueChange EventCallback fires only on user interaction (not on programmatic parameter set).
        [Trait("SfCheckBox", "Events")]
        [Fact(DisplayName = "ValueChange fires only on UI interaction, not programmatic changes")]
        public void ValueChange_UIOnly()
        {
            int callCount = 0;
            bool? lastValueFromEvent = null;

            var renderedComponent = RenderComponent<SfCheckBox<bool?>>(componentParameters => componentParameters
                .Add(component => component.EnableTriState, true)
                .Add(component => component.ValueChange, (CheckedChangeEventArgs<bool?> e) => { callCount++; lastValueFromEvent = e.Checked; })
            );

            // Programmatic change
            renderedComponent.SetParametersAndRender(componentParameters => componentParameters.Add(component => component.Checked, true));
            Assert.Equal(0, callCount);

            // UI click -> should fire
            var inputElement = renderedComponent.Find("input");
            inputElement.Click();
            Assert.Equal(1, callCount);
            Assert.Null(lastValueFromEvent);
        }

      

        #endregion
    }
}