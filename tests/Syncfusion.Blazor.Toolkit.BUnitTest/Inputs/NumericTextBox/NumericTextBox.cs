using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using System.ComponentModel.DataAnnotations;
//using Syncfusion.Blazor.DataForm;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.NumericTextBox
{
    public class NumericTextBox : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        public void Initialize()
        {
            var numeric = RenderComponent<SfNumericTextBox<int?>>();
            var inputEle = numeric.Find("input");
            Assert.Contains("e-numerictextbox", inputEle.ClassName);
            Assert.Contains("e-input", inputEle.ClassName);
            Assert.Contains("e-numeric", inputEle.ParentElement.ClassName);
            Assert.Contains("e-input-group", inputEle.ParentElement.ClassName);
        }
        [Fact(Timeout = 10000, DisplayName = "Render textbox with value as null")]
        public void BasicValue()
        {
            var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.Value, null));
            var inputEle = numeric.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            Assert.Equal("n0", numeric.Instance.Format); // the default value of the format n0 is set for int and byte data type.
            Assert.True(numeric.Instance.ShowSpinButton);

        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with min value as null")]
        public void MinMaxValue()
        {
            var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.Min, null).Add(p => p.Max, null));
            var inputEle = numeric.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            Assert.Equal(int.MinValue, numeric.Instance.Min);
            Assert.Equal(int.MaxValue, numeric.Instance.Max);
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with int nullable value as 5")]
        public void WithIntNullableValue()
        {
            var numeric = RenderComponent<SfNumericTextBox<int?>>();
            var inputEle = numeric.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            Assert.Null(numeric.Instance.Value);
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with int value as 5")]
        public void WithIntValue()
        {
            var numeric = RenderComponent<SfNumericTextBox<int>>(param => param.Add(p => p.Value, 5));
            var inputEle = numeric.Find("input");
            Assert.Equal("5", inputEle.GetAttribute("value"));
            Assert.Equal("5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with int value as 5")]
        public void WithIntDefaultValue()
        {
            var numeric = RenderComponent<SfNumericTextBox<int>>();
            var inputEle = numeric.Find("input");
            Assert.Equal("0", inputEle.GetAttribute("value"));
            Assert.Equal("0", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
            Assert.Equal(0, numeric.Instance.Value);
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with double value as 5")]
        public void WithDoubleValue()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 5));
            var inputEle = numeric.Find("input");
            Assert.Equal("5.00", inputEle.GetAttribute("value"));
            Assert.NotEqual("5", inputEle.GetAttribute("value"));
            Assert.Equal("5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with byte value as 5")]
        public void WithByteValue()
        {
            byte byteVal = 5;
            var numeric = RenderComponent<SfNumericTextBox<byte>>(param => param.Add(p => p.Value, byteVal));
            var inputEle = numeric.Find("input");
            Assert.Equal("5", inputEle.GetAttribute("value"));
            Assert.NotEqual("5.00", inputEle.GetAttribute("value"));
            Assert.Equal("5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with long value as 5")]
        public void WithLongValue()
        {
            var numeric = RenderComponent<SfNumericTextBox<long>>(param => param.Add(p => p.Value, 5));
            var inputEle = numeric.Find("input");
            Assert.Equal("5", inputEle.GetAttribute("value"));
            Assert.NotEqual("5.00", inputEle.GetAttribute("value"));
            Assert.Equal("5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with nullable long value")]
        public async Task NullableLongValue()
        {
            var numeric = RenderComponent<SfNumericTextBox<long?>>();
            var inputEle = numeric.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            Assert.Null(numeric.Instance.Value);
            long val = 5;
            numeric.SetParametersAndRender(("Value", val));
            await Task.Delay(200);
            inputEle = numeric.Find("input");
            Assert.Equal("5", inputEle.GetAttribute("value"));
            Assert.NotEqual("5.00", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with nullable short value")]
        public async Task NullableShortValueBind()
        {
            var numeric = RenderComponent<SfNumericTextBox<short?>>();
            var inputEle = numeric.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            Assert.Null(numeric.Instance.Value);
            short val = 5;
            numeric.SetParametersAndRender(("Value", val));
            await Task.Delay(100);
            inputEle = numeric.Find("input");
            Assert.Equal("5", inputEle.GetAttribute("value"));
            Assert.NotEqual("5.00", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with short value as 5")]
        public void ShortValueBind()
        {
            short shortVal = 5;
            var numeric = RenderComponent<SfNumericTextBox<short>>(param => param.Add(p => p.Value, shortVal));
            var inputEle = numeric.Find("input");
            Assert.Equal("5", inputEle.GetAttribute("value"));
            Assert.NotEqual("5.00", inputEle.GetAttribute("value"));
            Assert.Equal("5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with short value as null")]
        public void ShortNullableValueBind()
        {
            var numeric = RenderComponent<SfNumericTextBox<short?>>();
            var inputEle = numeric.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with float value as 5.5")]
        public void FloatValueBind()
        {
            var numeric = RenderComponent<SfNumericTextBox<float>>(param => param.Add(p => p.Value, 5.5f));
            var inputEle = numeric.Find("input");
            Assert.Equal("5.50", inputEle.GetAttribute("value"));
            Assert.Equal("5.5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with decimal float value as 5.5")]
        public void FloatDecimalValueBind()
        {
            var numeric = RenderComponent<SfNumericTextBox<float>>(param => param.Add(p => p.Value, 5.5674f).Add(p => p.Decimals, 1));
            var inputEle = numeric.Find("input");
            Assert.Equal("5.60", inputEle.GetAttribute("value"));
            Assert.Equal("5.6", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with decimal short value as 5")]
        public void ShortDecimalValueBind()
        {
            short val = 5;
            var numeric = RenderComponent<SfNumericTextBox<short>>(param => param.Add(p => p.Value, val).Add(p => p.Decimals, 3));
            var inputEle = numeric.Find("input");
            Assert.Equal("5", inputEle.GetAttribute("value"));
            Assert.Equal("5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with decimal long value as 5")]
        public void LongDecimalValueBind()
        {
            var numeric = RenderComponent<SfNumericTextBox<long>>(param => param.Add(p => p.Value, 5).Add(p => p.Decimals, 3));
            var inputEle = numeric.Find("input");
            Assert.Equal("5", inputEle.GetAttribute("value"));
            Assert.Equal("5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with decimal value as 5")]
        public void WithDecimalValue()
        {
            var numeric = RenderComponent<SfNumericTextBox<decimal>>(param => param.Add(p => p.Value, 5));
            var inputEle = numeric.Find("input");
            Assert.Equal("5.00", inputEle.GetAttribute("value"));
            Assert.NotEqual("5", inputEle.GetAttribute("value"));
            Assert.Equal("5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render currency textbox with value as 5")]
        public void CurrencyTextbox()
        {
            var numeric = RenderComponent<SfNumericTextBox<decimal>>(param => param.Add(p => p.Value, 5).Add(p => p.Format, "c2"));
            var inputEle = numeric.Find("input");
            Assert.Equal("$5.00", inputEle.GetAttribute("value"));
            Assert.Equal(5, numeric.Instance.Value);
            Assert.Equal("5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render currency int textbox with value as 5")]
        public void CurrencyIntTextbox()
        {
            var numeric = RenderComponent<SfNumericTextBox<int>>(param => param.Add(p => p.Value, 5).Add(p => p.Format, "c2"));
            var inputEle = numeric.Find("input");
            Assert.Equal("$5.00", inputEle.GetAttribute("value"));
            Assert.Equal(5, numeric.Instance.Value);
            Assert.Equal("5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render percentage textbox with value as 0.5")]
        public void PercentageTextbox()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 0.5).Add(p => p.Format, "p2"));
            var inputEle = numeric.Find("input");
            Assert.Equal("50.00%", inputEle.GetAttribute("value"));
            Assert.Equal(0.5, numeric.Instance.Value);
            Assert.Equal("0.5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render percentage int textbox with value as 0.5")]
        public void PercentageIntTextbox()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 5).Add(p => p.Format, "p2"));
            var inputEle = numeric.Find("input");
            Assert.Equal("500.00%", inputEle.GetAttribute("value"));
            Assert.Equal(5, numeric.Instance.Value);
            Assert.Equal("5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with negative numeric value")]
        public void NegativeValue()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, -5));
            var inputEle = numeric.Find("input");
            Assert.Equal("-5.00", inputEle.GetAttribute("value"));
            Assert.Equal(-5, numeric.Instance.Value);
            Assert.Equal("-5", inputEle.GetAttribute("aria-valuenow"));
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
            numeric.SetParametersAndRender(("Format", "c2"));
            inputEle = numeric.Find("input");
            //Assert.Equal("($5.00)", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(("Value", -0.5), ("Format", "p2"));
            inputEle = numeric.Find("input");
            //Assert.Equal("-50.00%", inputEle.GetAttribute("value"));
            Assert.Equal(-0.5, numeric.Instance.Value);
        }
        [Fact(Timeout = 10000, DisplayName = "Disable the numeric textbox control")]
        public void DisableComponent()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Disabled, true));
            var inputEle = numeric.Find("input");
            Assert.Contains("e-disabled", inputEle.ParentElement.ClassList);
            Assert.Contains("e-disabled", inputEle.ClassList);
            Assert.Contains("true", inputEle.GetAttribute("aria-disabled"));
            numeric.SetParametersAndRender(("Disabled", false));
            Assert.DoesNotContain("e-disabled", inputEle.ParentElement.ClassList);
            Assert.DoesNotContain("e-disabled", inputEle.ClassList);
        }
        //[Fact(Timeout = 10000, DisplayName = "Enable/Disable the spin button option in numerictextbox")]
        //public void SpinButtonRendering()
        //{
        //    var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.ShowSpinButton, false));
        //    var inputEle = numeric.Find("input");
        //    var spinEle = inputEle.ParentElement.QuerySelector(".e-spin-up");
        //    Assert.Null(spinEle);
        //    numeric.SetParametersAndRender(param => param.Add(p => p.ShowSpinButton, true));
        //    inputEle = numeric.Find("input");
        //    var spinUpEle = inputEle.ParentElement.QuerySelector(".e-spin-up");
        //    var spinDownEle = inputEle.ParentElement.QuerySelector(".e-spin-down");
        //    Assert.NotNull(spinUpEle);
        //    Assert.Equal("Increment value", spinUpEle.GetAttribute("title"));
        //    Assert.Equal("Decrement value", spinDownEle.GetAttribute("title"));
        //    Assert.Equal("Increment value", spinUpEle.GetAttribute("aria-label"));
        //    Assert.Equal("Decrement value", spinDownEle.GetAttribute("aria-label"));
        //}
        [Fact(Timeout = 10000, DisplayName = "watermarkText property change testing")]
        public void PlaceholderChange()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Placeholder, "Enter the numeric value"));
            var inputEle = numeric.Find("input");
            Assert.Equal("Enter the numeric value", inputEle.GetAttribute("placeholder"));
            numeric.SetParametersAndRender(("Placeholder", "Enter the number"));
            inputEle = numeric.Find("input");
            Assert.Equal("Enter the number", inputEle.GetAttribute("placeholder"));
        }
        [Fact(Timeout = 10000, DisplayName = "Add custom css class to numerictextbox")]
        public void AddCustomClass()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.CssClass, "custom-css"));
            var inputEle = numeric.Find("input");
            Assert.Contains("custom-css", inputEle.ParentElement.ClassList);
            numeric.SetParametersAndRender(param => param.Add(p => p.CssClass, "dynamic-custom-css"));
            inputEle = numeric.Find("input");
            Assert.DoesNotContain("custom-css", inputEle.ParentElement.ClassList);
            Assert.Contains("dynamic-custom-css", inputEle.ParentElement.ClassList);
        }
        [Fact(Timeout = 10000, DisplayName = "Set and remove the readonly to numerictextbox")]
        public void Readonly()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Readonly, true));
            var inputEle = numeric.Find("input");
            Assert.Empty(inputEle.GetAttribute("readOnly"));
            numeric.SetParametersAndRender(("Readonly", false));
            inputEle = numeric.Find("input");
            Assert.Null(inputEle.GetAttribute("readOnly"));
        }
        [Fact(Timeout = 10000, DisplayName = "Set tabindex attribute to numerictextbox")]
        public void TabIndex()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.TabIndex, 2));
            var inputEle = numeric.Find("input");
            Assert.Equal("2", inputEle.GetAttribute("tabindex"));
            numeric.SetParametersAndRender(("TabIndex", 3));
            inputEle = numeric.Find("input");
            Assert.Equal("3", inputEle.GetAttribute("tabindex"));
        }
        [Fact(Timeout = 10000, DisplayName = "Widht property updating to component")]
        public void WidthChanges()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Width, "500px"));
            var inputEle = numeric.Find("input");
            Assert.Contains("500px", inputEle.ParentElement.GetAttribute("style"));
            numeric.SetParametersAndRender(("Width", "300px"));
            Assert.Contains("300px", inputEle.ParentElement.GetAttribute("style"));
        }
        [Fact(Timeout = 10000, DisplayName = "Set the format as numeric")]
        public async Task NumericFormat()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 10).Add(p => p.Format, "n2"));
            var inputEle = numeric.Find("input");
            Assert.Equal("10.00", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(("Format", "n3"));
            inputEle = numeric.Find("input");
            Assert.Equal("10.000", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, 0.5).Add(p => p.Format, "p2"));
            await Task.Delay(100);
            inputEle = numeric.Find("input");
            Assert.Equal("50.00%", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, 0.5).Add(p => p.Format, "c2"));
            await Task.Delay(100);
            inputEle = numeric.Find("input");
            Assert.Equal("$0.50", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "Set the format as numeric")]
        public async Task FormatWithIntType()
        {
            var numeric = RenderComponent<SfNumericTextBox<int>>(param => param.Add(p => p.Value, 10).Add(p => p.Format, "n2"));
            var inputEle = numeric.Find("input");
            Assert.Equal("10", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(("Format", "n5"));
            inputEle = numeric.Find("input");
            Assert.Equal("10.00000", inputEle.GetAttribute("value"));
            Assert.Equal("n5", numeric.Instance.Format);
        }
        [Fact(Timeout = 10000, DisplayName = "Set the custom currency symbol")]
        public void CustomCurrency()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 10).Add(p => p.Format, "c2").Add(p => p.Currency, "EUR"));
            var inputEle = numeric.Find("input");
            Assert.Equal("€10.00", inputEle.GetAttribute("value"));
            Assert.Equal(10, numeric.Instance.Value);
        }
        [Fact(Timeout = 10000, DisplayName = "Set the custom currency symbol")]
        public void CustomIntCurrency()
        {
            var numeric = RenderComponent<SfNumericTextBox<int>>(param => param.Add(p => p.Value, 10).Add(p => p.Format, "c2").Add(p => p.Currency, "EUR"));
            var inputEle = numeric.Find("input");
            Assert.Equal("€10.00", inputEle.GetAttribute("value"));
            Assert.Equal(10, numeric.Instance.Value);
        }
        [Fact(Timeout = 10000, DisplayName = "Set the custom format as ###.###")]
        public async Task CustomFormat()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 10).Add(p => p.Format, "###.###"));
            var inputEle = numeric.Find("input");
            Assert.Equal("10", inputEle.GetAttribute("value"));
            Assert.Equal(10, numeric.Instance.Value);
            numeric.SetParametersAndRender(("Value", 10.66666));
            await Task.Delay(300);
            inputEle = numeric.Find("input");
            Assert.Equal("10.667", inputEle.GetAttribute("value"));
            Assert.Equal(10.66666, numeric.Instance.Value);
            numeric.SetParametersAndRender(("Value", 100.5), ("Format", "00##.0000"));
            Assert.Equal("0100.5000", inputEle.GetAttribute("value"));
            Assert.Equal(100.5, numeric.Instance.Value);
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, 10).Add(p => p.Format, "##.### KG"));
            await Task.Delay(200);
            inputEle = numeric.Find("input");
            Assert.Equal("10 KG", inputEle.GetAttribute("value"));
            Assert.Equal(10, numeric.Instance.Value);
            numeric.SetParametersAndRender(("Value", 10.66666), ("Format", "##.### KG"));
            Assert.Equal("10.667 KG", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, 10).Add(p => p.Format, "##.### $"));
            await Task.Delay(200);
            inputEle = numeric.Find("input");
            Assert.Equal("10 $", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "Set the decimals value as 3 with min/ max and step in numeric")]
        public void DecimalValueBinding()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 10.88888).Add(p => p.Format, "n4").Add(p => p.Decimals, 3).Add(p => p.Min, 2.877888).Add(p => p.Max, 25.888888).Add(p => p.Step, 1.346666));
            var inputEle = numeric.Find("input");
            Assert.Equal("10.8890", inputEle.GetAttribute("value"));
            Assert.Equal(10.889, numeric.Instance.Value);
            Assert.Equal(2.878, numeric.Instance.Min);
            Assert.Equal(25.889, numeric.Instance.Max);
            Assert.Equal(1.347, numeric.Instance.Step);
            inputEle.Focus();
            //Assert.Equal("10.889", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(("Decimals", null));
            inputEle = numeric.Find("input");

            //Assert.Equal("10.889", inputEle.GetAttribute("value"));
            Assert.Equal(10.889, numeric.Instance.Value);
        }
        [Fact(Timeout = 10000, DisplayName = "Set the decimals value as 7 in numeric")]
        public void DecimalValue()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 0.0000008).Add(p => p.Format, "n7").Add(p => p.Decimals, 7));
            var inputEle = numeric.Find("input");
            Assert.Equal("8E-07", inputEle.GetAttribute("value"));
            Assert.Equal(0.0000008, numeric.Instance.Value);
            inputEle.Focus();
            //Assert.Equal("0.0000008", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "Set the decimals value as 3 in currency")]
        public void DecimalWithCurrency()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 15.88888).Add(p => p.Format, "c4").Add(p => p.Decimals, 3));
            var inputEle = numeric.Find("input");
            Assert.Equal("$15.8890", inputEle.GetAttribute("value"));
            Assert.Equal(15.889, numeric.Instance.Value);
            inputEle.Focus();
            //Assert.Equal("15.889", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "Set the validateonDecimal in numeric")]
        public void ValidateonDecimal()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 10.88888).Add(p => p.Decimals, 2));
            var inputEle = numeric.Find("input");
            Assert.Equal("10.89", inputEle.GetAttribute("value"));
            numeric.Find("input").Focus();
            Assert.Equal("10.89", inputEle.GetAttribute("value"));
            Assert.Equal(10.89, numeric.Instance.Value);
            numeric.SetParametersAndRender(param => param.Add(p => p.ValidateDecimalOnType, true).Add(p => p.Value, 5.56));
            numeric.Find("input").Focus();
            Assert.Equal("5.56", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "Change Event testing")]
        public async Task ChangeEventBinding()
        {
            var count = 0;
            var isInteracted = false;
            double prevValue = 0;
            double value = 0;
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 123)
            .Add(p => p.ValueChange, (ChangeEventArgs<double> args) =>
            {
                count++;
                isInteracted = args.IsInteracted;
                value = args.Value;
                prevValue = args.PreviousValue;
            }).Add(p => p.OnFocus, (NumericFocusEventArgs<double> args) =>
            {
                Assert.NotNull("Focus event is fired");
            }));
            Assert.Equal(0, count);
            var inputEle = numeric.Find("input");
            var spinUp = inputEle.ParentElement.QuerySelector(".e-chevron-up");
            spinUp.MouseDown();
            spinUp.MouseUp();
            await numeric.Instance.IncrementAsync(1);
            await Task.Delay(200);
            inputEle = numeric.Find("input");
            Assert.Equal(1, count);
            Assert.Equal(124, value);
            Assert.Equal(123, prevValue);
            Assert.False(isInteracted);
            Assert.Equal("124.00", inputEle.GetAttribute("value"));
            var spinDown = inputEle.ParentElement.QuerySelector(".e-chevron-down");
            spinDown.MouseDown();
            spinDown.MouseUp();
            await numeric.Instance.DecrementAsync(1);
            await Task.Delay(200);
            inputEle = numeric.Find("input");
            Assert.Equal(2, count);
            Assert.Equal(123, value);
            Assert.Equal(123, numeric.Instance.Value);
            Assert.Equal(124, prevValue);
            Assert.False(isInteracted);
            Assert.Equal("123.00", inputEle.GetAttribute("value"));
            inputEle.Focus();
        }
        [Fact(Timeout = 10000, DisplayName = "Numeric textbox with element attribute")]
        public void HtmlAttributes()
        {
            var attr = new Dictionary<string, object>() { { "disabled", "true" } };
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.HtmlAttributes, attr));
            var inputEle = numeric.Find("input");
            Assert.Equal("true", inputEle.GetAttribute("disabled"));
            var htmlAttr = numeric.Instance.HtmlAttributes;
            Assert.Contains("true", htmlAttr["disabled"].ToString());
            attr = new Dictionary<string, object>() { { "readonly", "true" } };
            numeric.SetParametersAndRender(("HtmlAttributes", attr));
            inputEle = numeric.Find("input");
            Assert.Equal("true", inputEle.GetAttribute("readonly"));
            Assert.Contains("true", numeric.Instance.HtmlAttributes["readonly"].ToString());
        }
        [Fact(Timeout = 10000, DisplayName = "Numerictextbox public methods testing")]
        public async Task IncrementPublicMethods()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 10));
            var inputEle = numeric.Find("input");
            Assert.Equal("10.00", inputEle.GetAttribute("value"));
            await numeric.Instance.IncrementAsync(1);
            inputEle = numeric.Find("input");
            Assert.Equal("11.00", inputEle.GetAttribute("value"));
            await numeric.Instance.DecrementAsync(2);
            inputEle = numeric.Find("input");
            Assert.Equal("9.00", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "Call the increment operation in percentage textbox")]
        public async Task PercentageIncrementAsync()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 0.5).Add(p => p.Format, "p2"));
            var inputEle = numeric.Find("input");
            Assert.Equal("50.00%", inputEle.GetAttribute("value"));
            await numeric.Instance.IncrementAsync(1);
            inputEle = numeric.Find("input");
            Assert.Equal("150.00%", inputEle.GetAttribute("value"));
            Assert.Equal(1.5, numeric.Instance.Value);
            await numeric.Instance.DecrementAsync(1);
            inputEle = numeric.Find("input");
            Assert.Equal("50.00%", inputEle.GetAttribute("value"));
            Assert.Equal(0.5, numeric.Instance.Value);
        }
        [Fact(Timeout = 10000, DisplayName = "Call the increment operation in currency textbox")]
        public async Task CurrencyIncrementAsync()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Value, 5).Add(p => p.Format, "c2"));
            var inputEle = numeric.Find("input");
            Assert.Equal("$5.00", inputEle.GetAttribute("value"));
            await numeric.Instance.IncrementAsync(1);
            inputEle = numeric.Find("input");
            Assert.Equal("$6.00", inputEle.GetAttribute("value"));
            Assert.Equal(6, numeric.Instance.Value);
            await numeric.Instance.DecrementAsync(1);
            inputEle = numeric.Find("input");
            Assert.Equal("$5.00", inputEle.GetAttribute("value"));
            Assert.Equal(5, numeric.Instance.Value);
        }
        [Fact(Timeout = 10000, DisplayName = "Call the increment operation with custom model step value")]
        public async Task StepValueUpdate()
        {
            var numeric = RenderComponent<SfNumericTextBox<double?>>(param => param.Add(p => p.Value, 10).Add(p => p.Step, 5));
            var inputEle = numeric.Find("input");
            Assert.Equal("10.00", inputEle.GetAttribute("value"));
            await numeric.Instance.IncrementAsync(5);
            inputEle = numeric.Find("input");
            Assert.Equal("15.00", inputEle.GetAttribute("value"));
            Assert.Equal(15, numeric.Instance.Value);
            await numeric.Instance.DecrementAsync(5);
            inputEle = numeric.Find("input");
            Assert.Equal("10.00", inputEle.GetAttribute("value"));
            Assert.Equal(10, numeric.Instance.Value);
        }
        [Fact(Timeout = 10000, DisplayName = "Call the GetText method in numeric textbox")]
        public void GetInputTextValue()
        {
            var numeric = RenderComponent<SfNumericTextBox<double?>>(param => param.Add(p => p.Value, 10));
            var inputEle = numeric.Find("input");
            Assert.Equal("10.00", inputEle.GetAttribute("value"));
            var textVal = numeric.Instance.GetFormattedText();
            Assert.Equal(textVal, inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(param => param.Add(p => p.Format, "c2"));
            Assert.Equal("$10.00", inputEle.GetAttribute("value"));
            textVal = numeric.Instance.GetFormattedText();
            Assert.Equal(textVal, inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "Call the decrement operation with custom model in percentage textbox")]
        public async Task DecimalSteps()
        {
            var numeric = RenderComponent<SfNumericTextBox<double?>>(param => param.Add(p => p.Value, 0.5).Add(p => p.Step, 0.1).Add(p => p.Format, "p2"));
            var inputEle = numeric.Find("input");
            Assert.Equal("50.00%", inputEle.GetAttribute("value"));
            await numeric.Instance.DecrementAsync(0.2);
            inputEle = numeric.Find("input");
            Assert.Equal("30.00%", inputEle.GetAttribute("value"));
            Assert.Equal(0.3, numeric.Instance.Value);
        }
        [Fact(Timeout = 10000, DisplayName = "NumericTextBox control with value min,max and strictMode combination")]
        public void MinMaxValueUpdate()
        {
            var numeric = RenderComponent<SfNumericTextBox<double?>>(param => param.Add(p => p.Value, 6).Add(p => p.Min, 5).Add(p => p.Max, 20));
            var inputEle = numeric.Find("input");
            Assert.Equal("6.00", inputEle.GetAttribute("value"));
            Assert.Equal(5, numeric.Instance.Min);
            Assert.Equal(20, numeric.Instance.Max);
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, -6));
            inputEle = numeric.Find("input");
            Assert.Equal("5.00", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, 50));
            inputEle = numeric.Find("input");
            Assert.Equal("20.00", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, -10).Add(p => p.Min, -20).Add(p => p.Max, -5));
            inputEle = numeric.Find("input");
            Assert.Equal("-10.00", inputEle.GetAttribute("value"));
            Assert.Equal(-20, numeric.Instance.Min);
            Assert.Equal(-5, numeric.Instance.Max);
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, -30).Add(p => p.Min, -20).Add(p => p.Max, -10));
            inputEle = numeric.Find("input");
            Assert.Equal("-20.00", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "NumericTextBox control with value min,max and strictMode combination")]
        public void MinMaxIntValueUpdate()
        {
            var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.Value, 6).Add(p => p.Min, 5).Add(p => p.Max, 20));
            var inputEle = numeric.Find("input");
            Assert.Equal("6", inputEle.GetAttribute("value"));
            Assert.Equal(5, numeric.Instance.Min);
            Assert.Equal(20, numeric.Instance.Max);
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, -6));
            inputEle = numeric.Find("input");
            Assert.Equal("5", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, 50));
            inputEle = numeric.Find("input");
            Assert.Equal("20", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, -10).Add(p => p.Min, -20).Add(p => p.Max, -5));
            inputEle = numeric.Find("input");
            Assert.Equal("-10", inputEle.GetAttribute("value"));
            Assert.Equal(-20, numeric.Instance.Min);
            Assert.Equal(-5, numeric.Instance.Max);
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, -30).Add(p => p.Min, -20).Add(p => p.Max, -10));
            inputEle = numeric.Find("input");
            Assert.Equal("-20", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with value as null and strict mode combination")]
        public void StrictModeUpdate()
        {
            var numeric = RenderComponent<SfNumericTextBox<double?>>(param => param.Add(p => p.Value, null).Add(p => p.StrictMode, false));
            var inputEle = numeric.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, -6).Add(p => p.Min, 5).Add(p => p.Max, 20));
            inputEle = numeric.Find("input");
            Assert.Equal("-6.00", inputEle.GetAttribute("value"));
            Assert.Equal(-6, numeric.Instance.Value);
            Assert.Contains("e-error", inputEle.ParentElement.ClassList);
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, 50).Add(p => p.Min, 5).Add(p => p.Max, 20));
            inputEle = numeric.Find("input");
            Assert.Equal("50.00", inputEle.GetAttribute("value"));
            Assert.Equal(50, numeric.Instance.Value);
            Assert.Contains("e-error", inputEle.ParentElement.ClassList);
        }
        [Fact(Timeout = 10000, DisplayName = "Render numeric textbox with value as null and strict mode combination")]
        public void StrictModeIntTypeUpdate()
        {
            var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.Value, null).Add(p => p.StrictMode, false));
            var inputEle = numeric.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, -6).Add(p => p.Min, 5).Add(p => p.Max, 20));
            inputEle = numeric.Find("input");
            Assert.Equal("-6", inputEle.GetAttribute("value"));
            Assert.Equal(-6, numeric.Instance.Value);
            Assert.Contains("e-error", inputEle.ParentElement.ClassList);
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, 50).Add(p => p.Min, 5).Add(p => p.Max, 20));
            inputEle = numeric.Find("input");
            Assert.Equal("50", inputEle.GetAttribute("value"));
            Assert.Equal(50, numeric.Instance.Value);
            Assert.Contains("e-error", inputEle.ParentElement.ClassList);
        }
        [Fact(Timeout = 10000, DisplayName = "Render percentage textbox with value less than min value")]
        public void PercentageWithMinMax()
        {
            var numeric = RenderComponent<SfNumericTextBox<double?>>(param => param.Add(p => p.Value, -0.6).Add(p => p.Min, 0.5).Add(p => p.Max, 1).Add(p => p.Format, "p2"));
            var inputEle = numeric.Find("input");
            Assert.Equal("50.00%", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, 50).Add(p => p.Min, 0.5).Add(p => p.Max, 1));
            inputEle = numeric.Find("input");
            Assert.Equal("100.00%", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(param => param.Add(p => p.Value, 50).Add(p => p.Min, 5).Add(p => p.Max, 20).Add(p => p.StrictMode, false));
            inputEle = numeric.Find("input");
            Assert.Equal("5,000.00%", inputEle.GetAttribute("value"));
            Assert.Equal(50, numeric.Instance.Value);
            Assert.Contains("e-error", inputEle.ParentElement.ClassList);
        }
        [Fact(Timeout = 10000, DisplayName = "float label auto element rendering")]
        public void FloatLabelRendering()
        {
            var numeric = RenderComponent<SfNumericTextBox<double?>>(param => param.Add(p => p.Placeholder, "Enter the numeric value").Add(p => p.FloatLabelType, FloatLabelType.Auto));
            var inputEle = numeric.Find("input");
            var parentEle = inputEle.ParentElement;
            var label = parentEle.QuerySelector(".e-float-text");
            Assert.Equal("Enter the numeric value", label.InnerHtml.Replace("\n", "").Trim());
            Assert.Contains("e-label-bottom", label.ClassList);
            Assert.Contains("e-float-input", parentEle.ClassList);
            inputEle.Focus();
            inputEle = numeric.Find("input");
            parentEle = inputEle.ParentElement;
            label = parentEle.QuerySelector(".e-float-text");
            Assert.Contains("e-label-top", label.ClassList);
        }
        [Fact(Timeout = 10000, DisplayName = "float label always element rendering")]
        public void FloatLabelAlwaysRendering()
        {
            var numeric = RenderComponent<SfNumericTextBox<double?>>(param => param.Add(p => p.Placeholder, "Enter the numeric value").Add(p => p.FloatLabelType, FloatLabelType.Always));
            var inputEle = numeric.Find("input");
            var parentEle = inputEle.ParentElement;
            var label = parentEle.QuerySelector(".e-float-text");
            Assert.Equal("Enter the numeric value", label.InnerHtml.Replace("\n", "").Trim());
            Assert.Contains("e-label-top", label.ClassList);
            Assert.Contains("e-float-input", parentEle.ClassList);
            inputEle.Focus();
            inputEle = numeric.Find("input");
            parentEle = inputEle.ParentElement;
            label = parentEle.QuerySelector(".e-float-text");
            Assert.Contains("e-label-top", label.ClassList);
        }
        [Fact(Timeout = 10000, DisplayName = "float label Never element rendering")]
        public void FloatLabelNeverRendering()
        {
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.Placeholder, "Enter the numeric value").Add(p => p.FloatLabelType, FloatLabelType.Never));
            var inputEle = numeric.Find("input");
            Assert.Equal("Enter the numeric value", inputEle.GetAttribute("placeholder"));
            numeric.SetParametersAndRender(("Placeholder", "Enter the number"));
            inputEle = numeric.Find("input");
            Assert.Equal("Enter the number", inputEle.GetAttribute("placeholder"));
        }
        [Fact(Timeout = 10000, DisplayName = "Enable Clear Icon")]
        public async Task ClearButtonRendering()
        {
            var numeric = RenderComponent<SfNumericTextBox<double?>>(param => param.Add(p => p.Value, 10));
            var inputEle = numeric.Find("input");
            var parentEle = inputEle.ParentElement;
            var clearEle = parentEle.QuerySelector(".e-toolkit-icons.e-close");
            Assert.Null(clearEle);
            numeric.SetParametersAndRender(param => param.Add(p => p.ShowClearButton, true));
            inputEle = numeric.Find("input");
            parentEle = inputEle.ParentElement;
            clearEle = parentEle.QuerySelector(".e-toolkit-icons.e-close");
            Assert.NotNull(clearEle);
            clearEle.MouseDown();
            await Task.Delay(100);
            inputEle = numeric.Find("input");
            Assert.Null(inputEle.GetAttribute("value"));
            Assert.Null(numeric.Instance.Value);
        }
        [Fact(Timeout = 10000, DisplayName = "Focus In and Focus Out handler")]
        public void FocusInAndFocusOutHandler()
        {
            var numeric = RenderComponent<SfNumericTextBox<double?>>(param => param.Add(p => p.Value, 10).Add(p => p.Disabled, true));
            var inputEle = numeric.Find("input");
            Assert.Contains("e-disabled", inputEle.ParentElement.ClassList);
            inputEle.Focus();
            inputEle = numeric.Find("input");
            Assert.Contains("e-disabled", inputEle.ParentElement.ClassList);
            Assert.DoesNotContain("e-input-focus", inputEle.ParentElement.ClassList);
            Assert.Equal("10.00", inputEle.GetAttribute("value"));
        }
        [Fact(Timeout = 10000, DisplayName = "Focus In and Focus Out handler with enabled readonly")]
        public void ReadonlyFocus()
        {
            var numeric = RenderComponent<SfNumericTextBox<double?>>(param => param.Add(p => p.Value, 10).Add(p => p.Readonly, true));
            var inputEle = numeric.Find("input");
            Assert.NotNull(inputEle.GetAttribute("readonly"));
            // Assert.Equal("true", inputEle.GetAttribute("aira-readonly"));
            inputEle.Focus();
            inputEle = numeric.Find("input");
            Assert.NotNull(inputEle.GetAttribute("readonly"));
            // Assert.Equal("true", inputEle.GetAttribute("aira-readonly"));
            Assert.Contains("e-input-focus", inputEle.ParentElement.ClassList);
            Assert.Equal("10.00", inputEle.GetAttribute("value"));
            numeric.SetParametersAndRender(("Readonly", false));
            inputEle = numeric.Find("input");
            Assert.Null(inputEle.GetAttribute("readonly"));
        }

        [Fact(Timeout = 10000)]
        public void AutoLabelwithClearIcon()
        {
            var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.Value, 10).Add(p => p.FloatLabelType, FloatLabelType.Auto).Add(p => p.ShowClearButton, true));
            var inputEle = numeric.Find("input");
            var parentEle = inputEle.ParentElement;
            var clearEle = parentEle.QuerySelector(".e-toolkit-icons.e-close");
            Assert.NotNull(clearEle);
        }

        [Fact(Timeout = 10000)]
        public async Task StepPropertyChanges()
        {
            var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.Value, 10).Add(p => p.Step, 5).Add(p => p.Placeholder, "Enter Value").Add(p => p.FloatLabelType, FloatLabelType.Never));
            numeric.SetParametersAndRender(param => param.Add(p => p.Step, 1));
            await Task.Delay(10);
            Assert.Equal(1, numeric.Instance.Step);
        }

        [Fact(Timeout = 10000)]
        public async Task FormatValueAsStringMethod()
        {
            var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.Value, 10).Add(p => p.Format, "n3"));
            var inputEle = numeric.Find("input");
            Assert.Equal("10.000", inputEle.GetAttribute("value"));
            inputEle.Change("22");
            await Task.Delay(1);
            Assert.Equal("22.000", inputEle.GetAttribute("value"));//The component has no focus so the value is formatted
            inputEle.Focus();
            await Task.Delay(1);
            //Assert.Equal("22", inputEle.GetAttribute("value"));//The component has focus so the value is not formatted
        }

        //[Fact(Timeout = 10000)]
        //public void ErrorClassValidation()
        //{
        //    var dataForm = RenderComponent<SfDataForm>(parameters => parameters
        //     .Add(p => p.Model, new TestModel())
        //     .AddChildContent<FormValidator>(p => p.AddChildContent<DataAnnotationsValidator>())
        //     .AddChildContent<FormItems>(p => p.AddChildContent<FormAutoGenerateItems>())
        //        );
        //    var formElement = dataForm.Find("form");
        //    var buttonElement = dataForm.Find("button");
        //    buttonElement.Click();
        //    var inputElement = dataForm.Find("input");
        //    Assert.Contains("e-error", inputElement.ParentElement.ClassName);
        //}

        //[Fact(Timeout = 10000)]
        //public async void SuccessClassValidation()
        //{
        //    var modelObj = new TestModel();
        //    var dataForm = RenderComponent<SfDataForm>(parameters => parameters
        //     .Add(p => p.Model, new TestModel())
        //     .AddChildContent<FormValidator>(p => p.AddChildContent<DataAnnotationsValidator>())
        //     .AddChildContent<FormItems>(p => p.AddChildContent<FormAutoGenerateItems>())
        //        );
        //    var inputElement = dataForm.FindAll("input");
        //    inputElement[0].Focus();
        //    inputElement[0].Change(122);
        //    await Task.Delay(100);
        //    Assert.Contains("e-success", dataForm.FindAll("input")[0].ParentElement.ClassName);
        //}

        [Fact(Timeout = 10000)]
        public async Task EnablePersistencePropertyCheck()
        {
            var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.ID, "Presist-ID").Add(p => p.Value, 10).Add(p => p.EnablePersistence, true));
            Assert.True(numeric.Instance.EnablePersistence);
            Assert.Null(await numeric.Instance.GetPersistDataAsync());
        }

        [Fact(Timeout = 10000)]
        public async Task PerformActionMethod()
        {
            var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.Value, null)); //PerformAction method Call: Initial Value null to 10 with IncrementAsync
            await numeric.Instance.IncrementAsync(1);
            Assert.Equal(1, numeric.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public void InputAttributesCheck()
        {
            Dictionary<string, object> inputAttributes = new Dictionary<string, object>() { { "Value", "300" } };
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.InputAttributes, inputAttributes).Add(p => p.Width, "300px"));
            var inputAttr = numeric.Instance.InputAttributes;
            Assert.Contains("300", inputAttr["Value"].ToString());
            Assert.Equal(0, numeric.Instance.TabIndex);
            Assert.Equal("300px", numeric.Instance.Width);
            var inputEle = numeric.Find("input");
        }

        [Fact(Timeout = 10000, DisplayName = "InputAttributes type=number sets Format f")]
        public void InputAttributesTypeNumberSetsFormatF()
        {
            var inputAttributes = new Dictionary<string, object>() { { "type", "number" } };
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.InputAttributes, inputAttributes));
            Assert.Equal("f", numeric.Instance.Format);
        }

        [Fact(Timeout = 10000, DisplayName = "InputAttributes type=text sets role textbox")]
        public void InputAttributesTypeTextSetsRoleTextbox()
        {
            var inputAttributes = new Dictionary<string, object>() { { "type", "text" } };
            var numeric = RenderComponent<SfNumericTextBox<double>>(param => param.Add(p => p.InputAttributes, inputAttributes));
            var inputEle = numeric.Find("input");
            Assert.Equal("spinbutton", inputEle.GetAttribute("role"));
        }

        [Fact(Timeout = 10000)]
        public async void TvaluebyteCheck()
        {
            var numeric = RenderComponent<SfNumericTextBox<byte?>>(param => param.Add(p => p.Value, null));
            var inputEle = numeric.Find("input");
            await numeric.Instance.IncrementAsync(5);
            await Task.Delay(200);
            Assert.Equal("5", numeric.Instance.Value.ToString());
        }

        [Fact(Timeout = 10000)]
        public void NumericTextBoxModel()
        {
            var numeric = new NumericTextBoxModel<int?>()
            {
                Value = 10,
                Decimals = 2,
                Format = "n2",
                Min = 5,
                Max = 20,
                Step = 1,
                Placeholder = "Enter the value",
                Enabled = true,
                Readonly = false,
                EnablePersistence = true,
                FloatLabelType = FloatLabelType.Auto,
                ShowClearButton = true,
                CssClass = "custom-css",
                Currency = "EUR",
                ValidateDecimalOnType = true,
                StrictMode = true,
                ShowSpinButton = true,
                Width = "300px",
                TabIndex = 2,
                HtmlAttributes = new Dictionary<string, object>() { { "disabled", "true" } },
                InputAttributes = new Dictionary<string, object>() { { "Value", "300" } }
            };
            Assert.Equal(10, numeric.Value);
            Assert.Equal(2, numeric.Decimals);
            Assert.Equal("n2", numeric.Format);
            Assert.Equal(5, numeric.Min);
            Assert.Equal(20, numeric.Max);
            Assert.Equal(1, numeric.Step);
            Assert.Equal("Enter the value", numeric.Placeholder);
            Assert.True(numeric.Enabled);
            Assert.False(numeric.Readonly);
            Assert.True(numeric.EnablePersistence);
            Assert.Equal(FloatLabelType.Auto, numeric.FloatLabelType);
            Assert.True(numeric.ShowClearButton);
            Assert.Equal("custom-css", numeric.CssClass);
            Assert.Equal("EUR", numeric.Currency);
            Assert.True(numeric.ValidateDecimalOnType);
            Assert.True(numeric.StrictMode);
            Assert.True(numeric.ShowSpinButton);
            Assert.Equal("300px", numeric.Width);
            Assert.Equal(2, numeric.TabIndex);
            Assert.Equal("true", numeric.HtmlAttributes["disabled"].ToString());
            Assert.Contains("300", numeric.InputAttributes["Value"].ToString());
        }

        //[Fact(Timeout = 10000)]
        //public async Task ScriptSideMethod()
        //{
        //    var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.ID, "Script-ID").Add(p => p.Value, 10).Add(p => p.Step, 5));
        //    var inputEle = numeric.Find("input");
        //    await numeric.Instance.InvokePasteHandler("1", "paste");
        //    await numeric.Instance.ServerupdateValue(numeric.Instance.Value, new EventArgs() { });
        //    numeric.Instance.JSRuntime = null;
        //    await numeric.Instance.InvokePasteHandler("1", "paste");
        //}

        //[Fact(Timeout = 10000)]
        //public async Task ServerActionMethodWithDecimalValue()
        //{
        //    var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.ID, "Script-ID").Add(p => p.Value, 10).Add(p => p.Step, 5));
        //    var inputEle = numeric.Find("input");
        //    await numeric.InvokeAsync(() => numeric.Instance.ServerAction("increment", new EventArgs(), "10"));
        //    numeric.Instance.IsFocus = true;
        //    await numeric.InvokeAsync(() => numeric.Instance.ServerAction("decrement", new EventArgs(), "10"));
        //    numeric.Instance.IsFocus = true;
        //    await numeric.InvokeAsync(() => numeric.Instance.ServerAction("decrement", new EventArgs(), "1.2.3"));
        //}

        //[Fact(Timeout = 10000)]
        //public async Task ServerActionMethodWithDoubleValue()
        //{
        //    var numeric = RenderComponent<SfNumericTextBox<double?>>(param => param.Add(p => p.ID, "Script-ID").Add(p => p.Value, 10.5).Add(p => p.Step, 5));
        //    var inputEle = numeric.Find("input");
        //    await numeric.InvokeAsync(() => numeric.Instance.ServerAction("increment", new EventArgs(), "10.5"));
        //    numeric.Instance.IsFocus = true;
        //    await numeric.InvokeAsync(() => numeric.Instance.ServerAction("decrement", new EventArgs(), "10.5"));
        //    numeric.Instance.IsFocus = true;
        //    await numeric.InvokeAsync(() => numeric.Instance.ServerAction("decrement", new EventArgs(), "1.2.3"));
        //}

        //[Fact(Timeout = 10000)]
        //public async Task ServerActionMethodWithNullValue()
        //{
        //    var numeric = RenderComponent<SfNumericTextBox<int?>>(param => param.Add(p => p.ID, "Script-ID").Add(p => p.Value, null).Add(p => p.Step, 5));
        //    var inputEle = numeric.Find("input");
        //    numeric.Instance.IsFocus = true;
        //    await numeric.InvokeAsync(() => numeric.Instance.ServerAction("decrement", new EventArgs(), null));
        //}

        [Fact(Timeout = 10000)]
        public void InputsBlurEventArgs()
        {
            var blurArgs = new NumericBlurEventArgs<int?>
            {
                Value = 10
            };
            Assert.NotNull(blurArgs);
            Assert.Equal(10, blurArgs.Value);
        }

        [Fact(Timeout = 10000)]
        public void InputsFocusEventArgs()
        {
            var InputsFocusEventArgs = new NumericFocusEventArgs<int?>
            {
                Value = 10
            };
            Assert.NotNull(InputsFocusEventArgs);
            Assert.Equal(10, InputsFocusEventArgs.Value);
        }

        //[Fact(Timeout = 10000)]
        //public void InputsFormEventArgs()
        //{
        //    var InputsFormEventArgs = new FormEventArgs()
        //    {
        //        Element = new DOM(/* initialize your DOM object */),
        //        ErrorElement = new DOM(/* initialize your DOM object for error element */),
        //        InputName = "exampleInput",
        //        Message = "Example error message",
        //        Status = "Example status"
        //    };
        //    Assert.NotNull(InputsFormEventArgs.Element);
        //    Assert.NotNull(InputsFormEventArgs.ErrorElement);
        //    Assert.Equal("exampleInput", InputsFormEventArgs.InputName);
        //    Assert.Equal("Example error message", InputsFormEventArgs.Message);
        //    Assert.Equal("Example status", InputsFormEventArgs.Status);
        //}

        //[Fact(Timeout = 10000)]
        //public void IInputClass()
        //{
        //    object dummyChangeEvent = new object();
        //    object dummyFloatLabelType = "Auto";
        //    var inputConfig = new IInput
        //    {
        //        Change = dummyChangeEvent,
        //        CssClass = "inputClass",
        //        EnableRtl = true,
        //        Enabled = true,
        //        FloatLabelType = dummyFloatLabelType,
        //        Placeholder = "Enter text",
        //        Readonly = false,
        //        ShowClearButton = true
        //    };
        //    Assert.Equal(dummyChangeEvent, inputConfig.Change);
        //    Assert.Equal("inputClass", inputConfig.CssClass);
        //    Assert.True(inputConfig.EnableRtl);
        //    Assert.True(inputConfig.Enabled);
        //    Assert.Equal(dummyFloatLabelType, inputConfig.FloatLabelType);
        //    Assert.Equal("Enter text", inputConfig.Placeholder);
        //    Assert.False(inputConfig.Readonly);
        //    Assert.True(inputConfig.ShowClearButton);
        //}

        //[Fact(Timeout = 10000)]
        //public void NumericClientProps()
        //{
        //    var configNumericClientProps = new NumericClientProps
        //    {
        //        Readonly = true,
        //        Enabled = true,
        //        Locale = "en-US",
        //        ValidateDecimalOnType = true,
        //        Decimals = int.MaxValue,
        //        DecimalSeparator = ","
        //    };
        //    Assert.True(configNumericClientProps.Readonly);
        //    Assert.True(configNumericClientProps.Enabled);
        //    Assert.Equal("en-US", configNumericClientProps.Locale);
        //    Assert.True(configNumericClientProps.ValidateDecimalOnType);
        //    Assert.Equal(int.MaxValue, configNumericClientProps.Decimals);
        //    Assert.Equal(",", configNumericClientProps.DecimalSeparator);

        //}
    }
    public class TestModel
    {
        [Required]
        [Range(100, int.MaxValue)]
        public int NumberField { get; set; }
    }
}
