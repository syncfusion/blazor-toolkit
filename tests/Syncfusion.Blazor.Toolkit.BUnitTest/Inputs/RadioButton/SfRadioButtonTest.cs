using AngleSharp.Dom;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Syncfusion.Blazor.Toolkit.Buttons;
using Syncfusion.Blazor.Toolkit.Inputs;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs
{
    public partial class SfRadioButtonTest : BunitTestContext
    {
        #region Default rendering
        // Basic rendering of wrapper, input, label
        [Trait("SfRadioButton", "Basic")]
        [Fact(Timeout = 10000, DisplayName="UI Rendering")]
        public void Basic_UI_Rendering()
        {
            var cut = RenderComponent<DefaultRadio>();
            var inputElem = cut.FindAll("input",true);
             var labelElem = cut.FindAll("e-label",true);
            Assert.True(inputElem[0].ParentElement.ClassList.Contains("e-radio-wrapper"));
            Assert.True(inputElem[0].ClassList.Contains("e-radio"));
            //Assert.Equal("",labelElem[0].TextContent.Trim());
            //click radiobutton
            //inputElem[0].Click();
            //var radio_value_checked=inputElem[0].IsChecked();
            //Assert.True(radio_value_checked);
            
        }
        #endregion

        #region Binding (Direct / Property / Two-way)
        [Trait("SfRadioButton", "Binding")]
        [Fact(Timeout = 10000, DisplayName="Direct Value Binding")]
        public void Direct_Value_Binding()
        {
            var cut = RenderComponent<DefaultRadio>();
            var inputElem = cut.FindAll("input",true);
            var labelElem = cut.FindAll("e-label",true);
            Assert.Equal("Country",inputElem[1].GetAttribute("name"));
            Assert.Equal("US",inputElem[1].GetAttribute("value"));
            Assert.Equal("country",inputElem[2].GetAttribute("name"));
            Assert.Equal("India",inputElem[2].GetAttribute("value"));
            Assert.False(inputElem[1].IsChecked());
            Assert.True(inputElem[2].IsChecked());  
            //inputElem[1].Click();
            //Assert.True(inputElem[1].IsChecked());
            //Assert.False(inputElem[2].IsChecked());  

        }

        [Trait("SfRadioButton", "Binding")]
        [Fact(Timeout = 10000, DisplayName="Property Binding")]
        public void Property_Binding()
        {
            var cut = RenderComponent<DefaultRadio>();
            var inputElem = cut.FindAll("input",true);
            var propElem = cut.FindAll("div.e-prop-value",true);
            var buttonElem =cut.FindAll("button",true);
            Assert.False(inputElem[3].IsChecked());
            Assert.True(inputElem[4].IsChecked());
            Assert.Equal("female",propElem[0].TextContent.Trim());
            buttonElem[0].Click();
            Assert.True(inputElem[3].IsChecked());
            Assert.False(inputElem[4].IsChecked());
            Assert.Equal("male",propElem[0].TextContent.Trim());
            buttonElem[0].Click();
            Assert.False(inputElem[3].IsChecked());
            Assert.True(inputElem[4].IsChecked());
            Assert.Equal("female",propElem[0].TextContent.Trim());

        }

        [Trait("SfRadioButton", "Binding")]
        [Fact(Timeout = 10000, DisplayName="Two Way Binding - Bool")]
        public void Two_Way_Binding_Bool()
        {
            var cut = RenderComponent<DefaultRadio>();
            var inputElem = cut.FindAll("input",true);
            var propElem = cut.FindAll("div.e-prop-value",true);
            var buttonElem =cut.FindAll("button",true);
            Assert.False(inputElem[5].IsChecked());
            Assert.True(inputElem[6].IsChecked());
            Assert.Equal("False",propElem[1].TextContent.Trim());
            Assert.False(inputElem[7].IsChecked());
            Assert.True(inputElem[8].IsChecked());
            inputElem[5].Click();
            Assert.True(inputElem[5].IsChecked());
            Assert.False(inputElem[6].IsChecked());
            Assert.Equal("True",propElem[1].TextContent.Trim());
            Assert.True(inputElem[7].IsChecked());
            Assert.False(inputElem[8].IsChecked());
            inputElem[6].Click();
            Assert.False(inputElem[5].IsChecked());
            Assert.True(inputElem[6].IsChecked());
            Assert.Equal("False",propElem[1].TextContent.Trim());
            Assert.False(inputElem[7].IsChecked());
            Assert.True(inputElem[8].IsChecked());
        }

        [Trait("SfRadioButton", "Binding")]
        [Fact(Timeout = 10000, DisplayName="Two Way Binding - Nullable Bool")]
        public void Two_Way_Binding_Nullable_Bool()
        {
            var cut = RenderComponent<DefaultRadio>();
            var inputElem = cut.FindAll("input",true);
            var propElem = cut.FindAll("div.e-prop-value",true);
            var buttonElem =cut.FindAll("button",true);
            Assert.False(inputElem[9].IsChecked());
            Assert.False(inputElem[10].IsChecked());
            Assert.False(inputElem[11].IsChecked());
            Assert.Equal("",propElem[2].TextContent.Trim());

            //inputElem[9].Click();
            //Assert.True(inputElem[9].IsChecked());
            //Assert.False(inputElem[10].IsChecked());
            //Assert.False(inputElem[11].IsChecked());
            //Assert.Equal("",propElem[2].TextContent.Trim());

            //inputElem[10].Click();
            //Assert.False(inputElem[9].IsChecked());
            //Assert.True(inputElem[10].IsChecked());
            //Assert.False(inputElem[11].IsChecked());
            //Assert.Equal("True",propElem[2].TextContent.Trim());

            //inputElem[11].Click();
            //Assert.False(inputElem[9].IsChecked());
            //Assert.False(inputElem[10].IsChecked());
            //Assert.True(inputElem[11].IsChecked());
            //Assert.Equal("False",propElem[2].TextContent.Trim());

            //inputElem[9].Click();
            //Assert.True(inputElem[9].IsChecked());
            //Assert.False(inputElem[10].IsChecked());
            //Assert.False(inputElem[11].IsChecked());
            //Assert.Equal("",propElem[2].TextContent.Trim());

        }

        [Trait("SfRadioButton", "Binding")]
        [Fact(Timeout = 10000, DisplayName="Two Way Binding - String")]
        public void Two_Way_Binding_String()
        {
            var cut = RenderComponent<DefaultRadio>();
            var inputElem = cut.FindAll("input",true);
            var propElem = cut.FindAll("div.e-prop-value",true);
            var buttonElem =cut.FindAll("button",true);
            Assert.True(inputElem[12].IsChecked());
            Assert.False(inputElem[13].IsChecked());
            Assert.Equal("cash",propElem[3].TextContent.Trim());
            inputElem[13].Click();
            Assert.False(inputElem[12].IsChecked());
            Assert.True(inputElem[13].IsChecked());
            Assert.Equal("card",propElem[3].TextContent.Trim());

        }

        [Trait("SfRadioButton", "Binding")]
        [Fact(Timeout = 10000, DisplayName="Two Way Binding - Int")]
        public void Two_Way_Binding_Int()
        {
            var cut = RenderComponent<DefaultRadio>();
            var inputElem = cut.FindAll("input",true);
            var propElem = cut.FindAll("div.e-prop-value",true);
            var buttonElem =cut.FindAll("button",true);
            Assert.False(inputElem[14].IsChecked());
            Assert.True(inputElem[15].IsChecked());
            Assert.False(inputElem[16].IsChecked());
            Assert.Equal("1",propElem[4].TextContent.Trim());

            inputElem[14].Click();
            Assert.True(inputElem[14].IsChecked());
            Assert.False(inputElem[15].IsChecked());
            Assert.False(inputElem[16].IsChecked());
            Assert.Equal("0",propElem[4].TextContent.Trim());

            inputElem[16].Click();
            Assert.False(inputElem[14].IsChecked());
            Assert.False(inputElem[15].IsChecked());
            Assert.True(inputElem[16].IsChecked());
            Assert.Equal("2",propElem[4].TextContent.Trim());

        }

        [Trait("SfRadioButton", "Binding")]
        [Fact(Timeout = 10000, DisplayName="Two Way Binding - Double")]
        public void Two_Way_Binding_Double()
        {
            var cut = RenderComponent<DefaultRadio>();
            var inputElem = cut.FindAll("input",true);
            var propElem = cut.FindAll("div.e-prop-value",true);
            var buttonElem =cut.FindAll("button",true);
            Assert.False(inputElem[17].IsChecked());
            Assert.False(inputElem[18].IsChecked());
            Assert.True(inputElem[19].IsChecked());
            Assert.Equal("1.3",propElem[5].TextContent.Trim());

            inputElem[17].Click();
            Assert.True(inputElem[17].IsChecked());
            Assert.False(inputElem[18].IsChecked());
            Assert.False(inputElem[19].IsChecked());
            Assert.Equal("1.1",propElem[5].TextContent.Trim());

            inputElem[18].Click();
            Assert.False(inputElem[17].IsChecked());
            Assert.True(inputElem[18].IsChecked());
            Assert.False(inputElem[19].IsChecked());
            Assert.Equal("1.2",propElem[5].TextContent.Trim());

        }
        #endregion

        #region Label & Size
        [Trait("SfRadioButton", "Label")]
        [Fact(Timeout = 10000, DisplayName="Custom Label")]
        public void Custom_Label()
        {
            var cut = RenderComponent<LabelRadio>();
            var inputElem = cut.FindAll("input",true);
            var labelElem=cut.FindAll("label",true);
            Assert.Equal("custom",labelElem[2].GetAttribute("for"));
            Assert.Equal("custom",labelElem[3].GetAttribute("for"));
            Assert.Equal("custom",inputElem[2].Id);
            //labelElem[3].Click();
            //Assert.True(inputElem[2].IsChecked());
        }


        [Trait("SfRadioButton", "Label")]
        [Fact(Timeout = 10000, DisplayName="Label Position")]
        public void Default_LabelPosition()
        {
            var cut = RenderComponent<LabelRadio>();
            var inputElem = cut.FindAll("input",true);
            Assert.Null(inputElem[0].NextElementSibling.ClassName);
            Assert.Equal("e-right",inputElem[1].NextElementSibling.ClassName);
        }

        // Sizes
        [Trait("SfRadioButton", "Size")]
        [Fact(Timeout = 10000, DisplayName="Size small")]
        public void Size_Small()
        {
            var cut = RenderComponent<SizeRadio>();
            var inputElem = cut.FindAll("input",true);
            Assert.True(inputElem[0].ParentElement.ClassList.Contains("e-small")); 
        }

        [Trait("SfRadioButton", "Size")]
        [Fact(Timeout = 10000, DisplayName="Size bigger")]
        public void Size_Bigger()
        {
            var cut = RenderComponent<SizeRadio>();
            var inputElem = cut.FindAll("input",true);
            Assert.True(inputElem[1].ParentElement.ClassList.Contains("e-bigger")); 
        }

        [Trait("SfRadioButton", "Size")]
        [Fact(Timeout = 10000, DisplayName="Size bigger Small")]
        public void Size_BiggerSmall()
        {
            var cut = RenderComponent<SizeRadio>();
            var inputElem = cut.FindAll("input",true);
            Assert.Equal("e-radio-wrapper e-wrapper e-bigger e-small",inputElem[2].ParentElement.ClassName); 
        }
        #endregion

        #region Events
        // All event tests: ValueChange, onclick/lambda, Created, etc.
        [Trait("SfRadioButton", "Events")]
        [Fact(Timeout = 10000, DisplayName = "Lambda Expression")]
        public void Event_Lambda()
        {
            var cut = RenderComponent<EventRadio>();
            var inputElem = cut.FindAll("input", true);
            var tdElems = cut.FindAll("td", true);
            //inputElem[7].Click();
            //Assert.Contains("netbanking",tdElems[3].TextContent.Trim());
        }

        [Trait("SfRadioButton", "Events")]
        [Fact(DisplayName = "ValueChange returns args.Value and args.Event")]
        public void ValueChange_Event_Payload()
        {
            string receivedValue = null;
            object receivedEvent = null;

            var cut = RenderComponent<SfRadioButton<string>>(p => p
                .Add(x => x.Name, "pay")
                .Add(x => x.Value, "cash")
                .Add(x => x.Checked, (string)null)
                .Add(x => x.ValueChange, (ChangeArgs<string> args) =>
                {
                    receivedValue = args.Value;
                    receivedEvent = args.Event;
                })
            );
            cut.Find("input").Click();
            Assert.Equal("cash", receivedValue);
            Assert.NotNull(receivedEvent);
        }
        #endregion

        #region Others (Iteration / RTL / Disabled)
        [Trait("SfRadioButton", "Others")]
        [Fact(Timeout = 10000, DisplayName = "Iteration")]
        public void Iteration()
        {
            var cut = RenderComponent<OtherRadio>();
            var inputElem = cut.FindAll("input", true);
            var tdElems = cut.FindAll("div.e-c-list", true);
            Assert.Equal(3, tdElems[0].QuerySelectorAll("div.e-radio-wrapper").Count());
        }

        [Trait("SfRadioButton", "Others")]
        [Fact(Timeout = 10000, DisplayName="RTL")]
        public void RTL()
        {
            Services.AddScoped(_ => new SyncfusionBlazorToolkitService(Options.Create(new GlobalOptions { EnableRtl = true })));
            var cut = RenderComponent<OtherRadio>();
            var inputElem = cut.FindAll("input",true);
             Assert.True(inputElem[4].NextElementSibling.ClassList.Contains("e-rtl")); 
        }

        [Trait("SfRadioButton", "Others")]
        [Fact(Timeout = 10000, DisplayName="Disabled")]
        public void Disabled()
        {
            var cut = RenderComponent<OtherRadio>();
            var inputElem = cut.FindAll("input",true);
            Assert.True(inputElem[3].IsDisabled());
        }
        #endregion

        #region EnablePersistence (basic sample)
        [Trait("SfRadioButton", "EnablePersistence")]
        [Fact(Timeout = 10000, DisplayName = "EnablePersistence")]
        public async Task EnablePersistence()
        {
            var cut = RenderComponent<EnablePersistenceRadio>();
            var inputElem = cut.FindAll("input", true);
            Assert.True(inputElem[2].IsChecked());
            inputElem[3].Click();
            RenderComponent<EnablePersistenceRadio>();
            await Task.Delay(500);
            //Assert.True(inputElem[3].IsChecked());
        }
        #endregion

        #region Property Changes (SetParametersAndRender)
        // SetParametersAndRender tests: Disabled, RTL, Label, Value, Name, Checked, LabelPosition

        [Trait("SfRadioButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName="PropertyChanges Disabled")]
        public void PropertyChanges_Disabled()
        {
            var cut = RenderComponent<OtherRadio>();
            var inputElem = cut.FindAll("input",true);
            Assert.True(inputElem[3].IsDisabled()); 
            cut.SetParametersAndRender((nameof(OtherRadio.IsDisabled),false));
            Assert.False(inputElem[3].IsDisabled()); 
        }

        [Trait("SfRadioButton", "Property Changes")]
        [Fact(DisplayName="PropertyChanges RTL Enabled adds e-rtl")]
        public void RTL_Enabled_AddsClass()
        {
            Services.AddScoped(_ => new SyncfusionBlazorToolkitService(Options.Create(new GlobalOptions { EnableRtl = true })));
            var cut = RenderComponent<OtherRadio>();
            var inputElem = cut.FindAll("input",true);
            Assert.True(inputElem[4].NextElementSibling.ClassList.Contains("e-rtl"));
        }
        [Trait("SfRadioButton", "Property Changes")]
        [Fact(DisplayName="PropertyChanges RTL Disabled removes e-rtl")]
        public void RTL_Disabled_RemovesClass()
        {
            Services.AddScoped(_ => new SyncfusionBlazorToolkitService(Options.Create(new GlobalOptions { EnableRtl = false })));
            var cut = RenderComponent<OtherRadio>();
            var inputElem = cut.FindAll("input", true);

            Assert.False(inputElem[4].NextElementSibling.ClassList.Contains("e-rtl"));
        }    
        [Trait("SfRadioButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName="PropertyChanges Label")]
        public void PropertyChanges_Label()
        {
            var cut = RenderComponent<LabelRadio>();
            var inputElem = cut.FindAll("input",true);
            var labelElem=cut.FindAll("span.e-label",true);
            Assert.Equal("Credit Card",labelElem[0].TextContent.Trim());
            cut.SetParametersAndRender((nameof(LabelRadio.label),"Syncfusion"));
            labelElem=cut.FindAll("span.e-label",true);
            Assert.Equal("Syncfusion",labelElem[0].TextContent.Trim());
        }
        [Trait("SfRadioButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName="PropertyChanges Label Position")]
        public void PropertyChanges_LabelPosition()
        {
            var cut = RenderComponent<LabelRadio>();
            var inputElem = cut.FindAll("input",true);
            Assert.Null(inputElem[0].NextElementSibling.ClassName);
            Assert.Equal("e-right",inputElem[1].NextElementSibling.ClassName);
           cut.SetParametersAndRender((nameof(LabelRadio.labelposition), LabelPosition.After));
            Assert.Null(inputElem[1].QuerySelector(".e-right"));
        }
        [Trait("SfRadioButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName="Property Changes - Value")]
        public void PropertyChanges_Value()
        {
            var cut = RenderComponent<DefaultRadio>();
            var inputElem = cut.FindAll("input",true);
            var labelElem = cut.FindAll("e-label",true);
            Assert.Equal("Country",inputElem[1].GetAttribute("name"));
            Assert.Equal("US",inputElem[1].GetAttribute("value"));
            Assert.Equal("country",inputElem[2].GetAttribute("name"));
            Assert.Equal("India",inputElem[2].GetAttribute("value"));  
            cut.SetParametersAndRender((nameof(DefaultRadio.value1),"India"),(nameof(DefaultRadio.value2),"US"));
            Assert.Equal("Country",inputElem[1].GetAttribute("name"));
            Assert.Equal("India",inputElem[1].GetAttribute("value"));
            Assert.Equal("country",inputElem[2].GetAttribute("name"));
            Assert.Equal("US",inputElem[2].GetAttribute("value"));
        }
        [Trait("SfRadioButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName="Property Changes - Name")]
        public void PropertyChanges_Name()
        {
            var cut = RenderComponent<DefaultRadio>();
            var inputElem = cut.FindAll("input",true);
            var labelElem = cut.FindAll("e-label",true);
            Assert.Equal("Country",inputElem[1].GetAttribute("name"));
            cut.SetParametersAndRender((nameof(DefaultRadio.name),"Country1"));
            Assert.Equal("Country1",inputElem[1].GetAttribute("name"));
        }
         [Trait("SfRadioButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName="Property Property Changes - Checked")]
        public void PropertyChanges_Checked()
        {
            var cut = RenderComponent<DefaultRadio>();
            var inputElem = cut.FindAll("input",true);
            var propElem = cut.FindAll("div.e-prop-value",true);
            var buttonElem =cut.FindAll("button",true);
            Assert.False(inputElem[3].IsChecked());
            Assert.True(inputElem[4].IsChecked());
            Assert.Equal("female",propElem[0].TextContent.Trim());
            cut.SetParametersAndRender((nameof(DefaultRadio.gender),"male"));
            Assert.True(inputElem[3].IsChecked());
            Assert.False(inputElem[4].IsChecked());
            Assert.Equal("male",propElem[0].TextContent.Trim());
        }
        #endregion

        #region CR Issues / Regression
        [Trait("SfRadioButton", "CR_Issues")]
        [Fact(DisplayName = "BLAZ-9780: Screenshot attached in Radiobutton Documentation link  is mismatch to the code")]
        public void BLAZ_9780()
        {
            var cut = RenderComponent<DisabledRadio>();
            var inputElem = cut.FindAll("input",true);
            Assert.True(inputElem[1].IsDisabled());
        }
        #endregion

        #region ID handling
        [Trait("SfRadioButton", "ID")]
        [Fact(DisplayName = "ID passed via @attributes is preserved and label linked")]
        public void Id_Preserved_Via_Attributes()
        {
            var cut = RenderComponent<IdViaAttributesRadio>();
            var input = cut.Find("input");
            var label = cut.Find("label");

            Assert.Equal("custom-id-radio", input.Id);
            Assert.Equal("custom-id-radio", label.GetAttribute("for"));
        }


        [Trait("SfRadioButton", "ID")]
        [Fact(DisplayName = "Auto ID uses 'radiobutton-' prefix and links label")]
        public void Auto_Id_Prefix_And_Label()
        {
            var cut = RenderComponent<AutoIdRadio>();
            var input = cut.Find("input");
            var label = cut.Find("label");

            Assert.StartsWith("radiobutton-", input.Id);
            Assert.Equal(input.Id, label.GetAttribute("for"));
        }
        #endregion

        #region #region Label + RTL Composition
        [Trait("SfRadioButton", "Label/RTL")]
        [Fact(DisplayName = "Before + RTL yields 'e-right e-rtl' on label")]
        public void Before_RTL_Label_Class()
        {
            Services.AddScoped(_ => new SyncfusionBlazorToolkitService(Options.Create(new GlobalOptions { EnableRtl = true })));
            var cut = RenderComponent<RtlRadio>();
            var label = cut.Find("label");

            Assert.Contains("e-right", label.ClassList);
            Assert.Contains("e-rtl", label.ClassList);
        }
        #endregion

        #region Appearance / CssClass
        [Trait("SfRadioButton", "Appearance")]
        [Fact(DisplayName = "CssClass merges into root wrapper classes")]
        public void CssClass_Merges_Into_Wrapper()
        {
            var cut = RenderComponent<CssClassRadio>();
            var wrapper = cut.Find(".e-radio-wrapper.e-wrapper");

            Assert.Contains("e-custom", wrapper.ClassList);
            Assert.Contains("e-shadow", wrapper.ClassList);
        }
        #endregion

        #region Label Semantics
        [Trait("SfRadioButton", "Label")]
        [Fact(DisplayName = "Empty label renders an empty e-label span")]
        public void Empty_Label_Span_Renders()
        {
            var cut = RenderComponent<EmptyLabelRadio>();
            var span = cut.Find("span.e-label");
            Assert.Equal(string.Empty, span.TextContent);
        }
        #endregion

    }
}
