using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using AngleSharp.Css.Dom;
using Syncfusion.Blazor.Toolkit.Tests;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.TextBox
{
    public class FilledTextBox : BunitTestContext
    {
        #region Initialization Tests

        [Fact(Timeout = 10000)]
        public void DefaultInitialize()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled"));
            var inputElement = textBox.Find("input");
            Assert.Contains("e-textbox", inputElement.ClassName);
            Assert.NotNull(inputElement.ParentElement);
            Assert.Contains("e-control-container", inputElement.ParentElement.ClassName);
            Assert.Contains("e-input-group", inputElement.ParentElement.ClassName);
            Assert.True(inputElement.ParentElement.NodeName == "SPAN");
            Assert.Equal("0", inputElement.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000)]
        public void DefaultValue()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled"));
            Assert.Null(textBox.Instance.Value);
            Assert.Null(textBox.Instance.Placeholder);
            Assert.Contains("e-filled", textBox.Instance.CssClass);
            Assert.Null(textBox.Instance.Width);
            Assert.Equal(FloatLabelType.Never, textBox.Instance.FloatLabelType);
            Assert.False(textBox.Instance.ReadOnly);
            Assert.False(textBox.Instance.Multiline);
            Assert.False(textBox.Instance.ShowClearButton);
            Assert.False(textBox.Instance.EnablePersistence);
            Assert.False(textBox.Instance.Disabled);
        }

        #endregion

        #region Style and Appearance Tests

        [Fact(Timeout = 10000)]
        public void CssClass()
        {
            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.CssClass, "e-filled"));
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("e-filled", containerElement.ClassName);
            textBox.SetParametersAndRender(("CssClass", "test test1"));
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("test test1", containerElement.ClassName);
            Assert.DoesNotContain("e-filled", containerElement.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void TabIndex()
        {
            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.CssClass, "e-filled").Add(p => p.TabIndex, 1));
            var inputElement = textBox.Find("input");
            Assert.Equal("1", inputElement.GetAttribute("tabindex"));
            textBox.SetParametersAndRender(("TabIndex", 3));
            inputElement = textBox.Find("input");
            Assert.Equal("3", inputElement.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000)]
        public void ReadOnly()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled").Add(p => p.ReadOnly, true));
            var inputElement = textBox.Find("input");
            Assert.True(inputElement.HasAttribute("readonly"));
            textBox.SetParametersAndRender(("ReadOnly", false));
            Assert.False(inputElement.HasAttribute("readonly"));
        }

        [Fact(Timeout = 10000)]
        public void WidthChange()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled").Add(p => p.Width, "300px"));
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.Contains("width: 300px", containerElement.GetStyle().CssText.Trim());
            textBox.SetParametersAndRender(("Width", "600px"));
            containerElement = inputElement.ParentElement;
            Assert.Contains("width: 600px", containerElement.GetStyle().CssText.Trim());
        }

        #endregion

        #region Basic Properties Tests

        [Fact(Timeout = 10000)]
        public void Placeholder()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled").Add(p => p.Placeholder, "Enter the value"));
            var inputElement = textBox.Find("input");
            Assert.Contains("Enter the value", inputElement.GetAttribute("placeholder"));
            textBox.SetParametersAndRender(("Placeholder", "Enter text"));
            Assert.Contains("Enter text", inputElement.GetAttribute("placeholder"));
        }

        [Fact(Timeout = 10000)]
        public void Enabled()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled").Add(p => p.Disabled, false));
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.DoesNotContain("e-disabled", containerElement.ClassName);
            Assert.DoesNotContain("e-disabled", inputElement.ClassName);
            Assert.Equal("false", inputElement.GetAttribute("aria-disabled"));
            textBox.SetParametersAndRender(("Disabled", true));
            inputElement = textBox.Find("input");
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("e-disabled", containerElement.ClassName);
            Assert.Contains("e-disabled", inputElement.ClassName);
            Assert.Equal("true", inputElement.GetAttribute("aria-disabled"));
            textBox.SetParametersAndRender(("Disabled", false));
            inputElement = textBox.Find("input");
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.DoesNotContain("e-disabled", containerElement.ClassName);
            Assert.DoesNotContain("e-disabled", inputElement.ClassName);
            Assert.Equal("false", inputElement.GetAttribute("aria-disabled"));
        }

        [Fact(Timeout = 10000)]
        public void Autocomplete()
        {
            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.CssClass, "e-filled"));
            var inputElement = textBox.Find("input");
            Assert.Equal("on", inputElement.GetAttribute("autocomplete"));
            textBox.SetParametersAndRender(("Autocomplete", AutoComplete.Off));
            Assert.Equal("off", inputElement.GetAttribute("autocomplete"));
            textBox.SetParametersAndRender(("Autocomplete", AutoComplete.On));
            Assert.Equal("on", inputElement.GetAttribute("autocomplete"));
        }

        #endregion

        #region Attributes Tests

        [Fact(Timeout = 10000)]
        public void HtmlAttributes()
        {
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>() { { "name", "textbox" }, { "required", "true" }, { "class", "e-text" }, { "autocomplete", "off" }, { "type", "email" }, { "autofocus", "" } };
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled").Add(p => p.HtmlAttributes, htmlAttributes));
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("textbox", inputElement.GetAttribute("name"));
            Assert.Contains("true", inputElement.GetAttribute("required"));
            Assert.Equal("off", inputElement.GetAttribute("autocomplete"));
            Assert.Contains("e-text", containerElement.ClassName);
            Assert.Equal("email", inputElement.GetAttribute("type"));
            Assert.True(inputElement.HasAttribute("autofocus"));
            textBox.SetParametersAndRender(("HtmlAttributes", new Dictionary<string, object>() { { "name", "textbox1" }, { "required", "false" }, { "class", "e-text1" }, { "autocomplete", "on" }, { "type", "text" } }));
            inputElement = textBox.Find("input");
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("textbox1", inputElement.GetAttribute("name"));
            Assert.Contains("false", inputElement.GetAttribute("required"));
            Assert.Equal("on", inputElement.GetAttribute("autocomplete"));
            Assert.Contains("e-text1", containerElement.ClassName);
            Assert.Equal("text", inputElement.GetAttribute("type"));
            textBox.SetParametersAndRender(("HtmlAttributes", new Dictionary<string, object>() { { "type", "number" }, { "autocomplete", "off" }, { "max", 10 }, { "min", 1 } }));
            inputElement = textBox.Find("input");
            Assert.Equal("number", inputElement.GetAttribute("type"));
            Assert.Equal("off", inputElement.GetAttribute("autocomplete"));
            Assert.Equal("10", inputElement.GetAttribute("max"));
            Assert.Equal("1", inputElement.GetAttribute("min"));
            textBox.SetParametersAndRender(("HtmlAttributes", new Dictionary<string, object>() { { "maxlength", 5 } }));
            inputElement = textBox.Find("input");
            Assert.Equal("5", inputElement.GetAttribute("maxlength"));
        }

        [Fact(Timeout = 10000)]
        public void InputAttributes()
        {
            Dictionary<string, object> inputAttributes = new Dictionary<string, object>() { { "name", "textbox" }, { "required", "true" }, { "value", "Test" } };
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.InputAttributes, inputAttributes).Add(p => p.CssClass, "e-filled"));
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.Contains("textbox", inputElement.GetAttribute("name"));
            Assert.Contains("true", inputElement.GetAttribute("required"));
            textBox.SetParametersAndRender(("InputAttributes", new Dictionary<string, object>() { { "name", "textbox1" }, { "required", "false" }, { "value", "Test1" } }));
            inputElement = textBox.Find("input");
            containerElement = inputElement.ParentElement;
            Assert.Contains("textbox1", inputElement.GetAttribute("name"));
            Assert.Contains("false", inputElement.GetAttribute("required"));
            textBox.SetParametersAndRender(("InputAttributes", new Dictionary<string, object>() { { "type", "number" }, { "autocomplete", "off" }, { "max", 10 }, { "min", 1 } }));
            inputElement = textBox.Find("input");
            Assert.Equal("number", inputElement.GetAttribute("type"));
            Assert.Equal("off", inputElement.GetAttribute("autocomplete"));
            Assert.Equal("10", inputElement.GetAttribute("max"));
            Assert.Equal("1", inputElement.GetAttribute("min"));
            textBox.SetParametersAndRender(("InputAttributes", new Dictionary<string, object>() { { "maxlength", 5 } }));
            inputElement = textBox.Find("input");
            Assert.Equal("5", inputElement.GetAttribute("maxlength"));
        }

        [Fact(Timeout = 10000)]
        public void RoleAttribute()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled").Add(p => p.ShowClearButton, true));
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            var clearIconElement = containerElement.Children[1];
            Assert.Contains("textbox", inputElement.GetAttribute("role"));
            Assert.Contains("button", clearIconElement.GetAttribute("role"));
            textBox.SetParametersAndRender(("Multiline", true));
            var textAreaElement = textBox.Find("textarea");
            Assert.True(textAreaElement.HasAttribute("role"));
            Assert.Contains("textbox", textAreaElement.GetAttribute("role"));
        }

        #endregion

        #region Clear Button Tests

        [Fact(Timeout = 10000)]
        public void ClearButton()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled").Add(p => p.ShowClearButton, true));
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            var clearIconElement = containerElement.Children[1];
            Assert.Contains("e-clear-icon", clearIconElement.ClassName);
            Assert.Contains("e-clear-icon-hide", clearIconElement.ClassName);
            textBox.SetParametersAndRender(("ShowClearButton", false));
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            clearIconElement = containerElement.QuerySelector(".e-clear-icon");
            Assert.Null(clearIconElement);
            textBox.SetParametersAndRender(parameter => parameter.Add(p => p.ShowClearButton, true).Add(p => p.Value, "Test"));
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            clearIconElement = containerElement.Children[1];
            Assert.Contains("e-clear-icon", clearIconElement.ClassName);
            Assert.Contains("e-clear-icon-hide", clearIconElement.ClassName);
            Assert.Contains("Test", inputElement.GetAttribute("value"));
            Assert.Contains("Test", textBox.Instance.Value);
            clearIconElement.MouseDown();
            Assert.Null(inputElement.GetAttribute("value"));
            Assert.Null(textBox.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public void StaticClearIcon()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled e-static-clear").Add(p => p.ShowClearButton, true));
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            var clearIconElement = containerElement.Children[1];
            Assert.Contains("e-static-clear", containerElement.ClassName);
            Assert.Contains("e-clear-icon", clearIconElement.ClassName);
            Assert.Contains("e-clear-icon-hide", clearIconElement.ClassName);
            textBox.SetParametersAndRender(("Value", "Test"));
            inputElement = textBox.Find("input");
            Assert.Equal("Test", textBox.Instance.Value);
            Assert.Equal("Test", inputElement.GetAttribute("value"));
            clearIconElement.MouseDown();
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            clearIconElement = containerElement.Children[1];
            Assert.Null(textBox.Instance.Value);
            Assert.Null(inputElement.GetAttribute("value"));
            Assert.Contains("e-static-clear", containerElement.ClassName);
            Assert.Contains("e-clear-icon", clearIconElement.ClassName);
            Assert.Contains("e-clear-icon-hide", clearIconElement.ClassName);
        }

        #endregion

        #region Floating Label Tests

        [Fact(Timeout = 10000)]
        public void FloatingLabel()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled").Add(p => p.Placeholder, "Enter the value"));
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.DoesNotContain("e-float-text", containerElement.ClassName);
            textBox.SetParametersAndRender(("FloatLabelType", FloatLabelType.Auto));
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("e-float-input", containerElement.ClassName);
            var floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-bottom", floatLabelElement.ClassName);
            textBox.SetParametersAndRender(("Value", "Test"));
            inputElement = textBox.Find("input");
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-top", floatLabelElement.ClassName);
            textBox.SetParametersAndRender(("FloatLabelType", FloatLabelType.Always));
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("e-float-input", containerElement.ClassName);
            floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-top", floatLabelElement.ClassName);
            textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled").Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Always));
            inputElement = textBox.Find("input");
            inputElement.Focus();
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-top", floatLabelElement.ClassName);
            textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled").Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Never));
            inputElement = textBox.Find("input");
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.DoesNotContain("e-float-input", containerElement.ClassName);
            floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.Null(floatLabelElement);
            textBox.SetParametersAndRender(("FloatLabelType", FloatLabelType.Auto));
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("e-float-input", containerElement.ClassName);
            floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-bottom", floatLabelElement.ClassName);
            inputElement = textBox.Find("input");
            inputElement.Focus();
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-top", floatLabelElement.ClassName);
        }

        #endregion

        #region Focus and Interaction Tests

        [Fact(Timeout = 10000)]
        public void FocusComponent()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled").Add(p => p.Placeholder, "Enter the value"));
            var inputElement = textBox.Find("input");
            inputElement.Focus();
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("e-input-focus", containerElement.ClassName);
        }

        #endregion

        #region Value and Data Binding Tests

        [Fact(Timeout = 10000)]
        public void ValueBinding()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled").Add(p => p.Value, "Test"));
            Assert.Equal("Test", textBox.Find("input").GetAttribute("value"));
            Assert.Equal("Test", textBox.Instance.Value);
            textBox.SetParametersAndRender(("Value", "Test1"));
            Assert.Equal("Test1", textBox.Find("input").GetAttribute("value"));
            Assert.Equal("Test1", textBox.Instance.Value);
            textBox.Find("input").HasAttribute("role");
        }

        #endregion

        #region Input Type Tests

        [Fact(Timeout = 10000)]
        public void InputTypes()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-filled").Add(p => p.Type, InputType.Email));
            var inputElement = textBox.Find("input");
            Assert.Equal("email", inputElement.GetAttribute("type"));
            textBox.SetParametersAndRender(("Type", InputType.Number));
            inputElement = textBox.Find("input");
            Assert.Equal("number", inputElement.GetAttribute("type"));
        }

        #endregion

        #region Comprehensive Tests

        [Fact(Timeout = 10000)]
        public async Task AllAPICombination()
        {
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>() { { "class", "e-text" } };
            Dictionary<string, object> inputAttributes = new Dictionary<string, object>() { { "name", "textbox" } };
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.ID, "Text-1").Add(p => p.Autocomplete, AutoComplete.Off).Add(p => p.CssClass, "e-filled e-test").Add(p => p.EnablePersistence, true).Add(p => p.Placeholder, "Enter the value").Add(p => p.Disabled, false).Add(p => p.FloatLabelType, FloatLabelType.Always).Add(p => p.HtmlAttributes, htmlAttributes).Add(p => p.InputAttributes, inputAttributes).Add(p => p.Multiline, false).Add(p => p.ReadOnly, false).Add(p => p.ShowClearButton, true).Add(p => p.Value, "Test").Add(p => p.Width, "600px").Add(p => p.TabIndex, 1).Add(p => p.Type, InputType.Email));
            await Task.Delay(200);
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            var clearIconElement = containerElement.Children[3];
            var floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.Equal("Text-1", inputElement.GetAttribute("id"));
            Assert.Equal("off", inputElement.GetAttribute("autocomplete"));
            Assert.Contains("e-filled", containerElement.ClassName);
            Assert.Contains("e-test", containerElement.ClassName);
            Assert.DoesNotContain("e-disabled", containerElement.ClassName);
            Assert.DoesNotContain("e-disabled", inputElement.ClassName);
            Assert.Contains("e-float-input", containerElement.ClassName);
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-top", floatLabelElement.ClassName);
            Assert.Contains("e-text", containerElement.ClassName);
            Assert.Contains("textbox", inputElement.GetAttribute("name"));
            Assert.False(inputElement.HasAttribute("placeholder"));
            Assert.False(inputElement.HasAttribute("readonly"));
            Assert.Contains("e-clear-icon", clearIconElement.ClassName);
            Assert.Equal("Test", inputElement.GetAttribute("value"));
            Assert.Contains("width: 600px", containerElement.GetStyle().CssText.Trim());
            Assert.Equal("1", inputElement.GetAttribute("tabindex"));
            Assert.Equal("email", inputElement.GetAttribute("type"));
        }

        #endregion
    }
}
