using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using AngleSharp.Css.Dom;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.TextBox
{
    public class MultilineTextBox : BunitTestContext
    {
        #region Initialization Tests

        [Fact(Timeout = 10000)]
        public void DefaultInitialize()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true));
            var textAreaElement = textBox.Find("textarea");
            Assert.NotNull(textAreaElement.ParentElement);
            Assert.Contains("e-textbox", textAreaElement.ClassName);
            Assert.Contains("e-control-container", textAreaElement.ParentElement.ClassName);
            Assert.Contains("e-input-group", textAreaElement.ParentElement.ClassName);
            Assert.True(textAreaElement.ParentElement.NodeName == "SPAN");
            Assert.Equal("0", textAreaElement.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000)]
        public void DefaultValue()
        {
            var textBox = RenderComponent<SfTextBox>(("Multiline", true));
            Assert.Null(textBox.Instance.Value);
            Assert.Null(textBox.Instance.Placeholder);
            Assert.Null(textBox.Instance.CssClass);
            Assert.Null(textBox.Instance.Width);
            Assert.Equal(FloatLabelType.Never, textBox.Instance.FloatLabelType);
            Assert.False(textBox.Instance.ReadOnly);
            Assert.True(textBox.Instance.Multiline);
            Assert.False(textBox.Instance.ShowClearButton);
            Assert.False(textBox.Instance.EnablePersistence);
            Assert.False(textBox.Instance.Disabled);
        }

        #endregion

        #region Style and Appearance Tests

        [Fact(Timeout = 10000)]
        public void CssClass()
        {
            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.Multiline, true).Add(p => p.CssClass, "sample-css"));
            var textAreaElement = textBox.Find("textarea");
            var containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("sample-css", containerElement.ClassName);
            textBox.SetParametersAndRender(("CssClass", "test test1"));
            containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("test test1", containerElement.ClassName);
            Assert.DoesNotContain("sample-css", containerElement.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void TabIndex()
        {
            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.Multiline, true).Add(p => p.TabIndex, 1));
            var textAreaElement = textBox.Find("textarea");
            Assert.Equal("1", textAreaElement.GetAttribute("tabindex"));
            textBox.SetParametersAndRender(("TabIndex", 3));
            textAreaElement = textBox.Find("textarea");
            Assert.Equal("3", textAreaElement.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000)]
        public void ReadOnly()
        {
            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.Multiline, true).Add(p => p.ReadOnly, true));
            var textAreaElement = textBox.Find("textarea");
            Assert.True(textAreaElement.HasAttribute("readonly"));
            textBox.SetParametersAndRender(("ReadOnly", false));
            Assert.False(textAreaElement.HasAttribute("readonly"));
        }

        [Fact(Timeout = 10000)]
        public void WidthChange()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.Width, "300px"));
            var textAreaElement = textBox.Find("textarea");
            var containerElement = textAreaElement.ParentElement;
            Assert.Contains("width: 300px", containerElement.GetStyle().CssText.Trim());
            textBox.SetParametersAndRender(("Width", "600px"));
            containerElement = textAreaElement.ParentElement;
            Assert.Contains("width: 600px", containerElement.GetStyle().CssText.Trim());
        }

        #endregion

        #region Basic Properties Tests

        [Fact(Timeout = 10000)]
        public void Placeholder()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.Placeholder, "Enter the value"));
            var textAreaElement = textBox.Find("textarea");
            Assert.Contains("Enter the value", textAreaElement.GetAttribute("placeholder"));
            textBox.SetParametersAndRender(("Placeholder", "Enter text"));
            Assert.Contains("Enter text", textAreaElement.GetAttribute("placeholder"));
        }

        [Fact(Timeout = 10000)]
        public void Enabled()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.Disabled, false));
            var textAreaElement = textBox.Find("textarea");
            var containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.DoesNotContain("e-disabled", containerElement.ClassName);
            Assert.DoesNotContain("e-disabled", textAreaElement.ClassName);
            Assert.Equal("false", textAreaElement.GetAttribute("aria-disabled"));
            textBox.SetParametersAndRender(("Disabled", true));
            textAreaElement = textBox.Find("textarea");
            containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("e-disabled", containerElement.ClassName);
            Assert.Contains("e-disabled", textAreaElement.ClassName);
            Assert.Equal("true", textAreaElement.GetAttribute("aria-disabled"));
            textBox.SetParametersAndRender(("Disabled", false));
            textAreaElement = textBox.Find("textarea");
            containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.DoesNotContain("e-disabled", containerElement.ClassName);
            Assert.DoesNotContain("e-disabled", textAreaElement.ClassName);
            Assert.Equal("false", textAreaElement.GetAttribute("aria-disabled"));
        }

        [Fact(Timeout = 10000)]
        public void Autocomplete()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true));
            var textAreaElement = textBox.Find("textarea");
            Assert.Equal("on", textAreaElement.GetAttribute("autocomplete"));
            textBox.SetParametersAndRender(("Autocomplete", AutoComplete.Off));
            Assert.Equal("off", textAreaElement.GetAttribute("autocomplete"));
            textBox.SetParametersAndRender(("Autocomplete", AutoComplete.On));
            Assert.Equal("on", textAreaElement.GetAttribute("autocomplete"));
        }

        #endregion

        #region Attributes Tests

        [Fact(Timeout = 10000)]
        public void HtmlAttributes()
        {
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>() { { "name", "textbox" }, { "required", "true" }, { "class", "e-text" }, { "autocomplete", "off" }, { "autofocus", "" }, { "rows", "5" }, { "cols", "10" } };
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.HtmlAttributes, htmlAttributes));
            var textAreaElement = textBox.Find("textarea");
            var containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("textbox", textAreaElement.GetAttribute("name"));
            Assert.Contains("true", textAreaElement.GetAttribute("required"));
            Assert.Equal("off", textAreaElement.GetAttribute("autocomplete"));
            Assert.Contains("e-text", containerElement.ClassName);
            Assert.True(textAreaElement.HasAttribute("autofocus"));
            Assert.Equal("5", textAreaElement.GetAttribute("rows"));
            Assert.Equal("10", textAreaElement.GetAttribute("cols"));
            textBox.SetParametersAndRender(("HtmlAttributes", new Dictionary<string, object>() { { "name", "textbox1" }, { "required", "false" }, { "class", "e-text1" }, { "autocomplete", "on" }, { "rows", "6" }, { "cols", "11" } }));
            textAreaElement = textBox.Find("textarea");
            containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("textbox1", textAreaElement.GetAttribute("name"));
            Assert.Contains("false", textAreaElement.GetAttribute("required"));
            Assert.Equal("on", textAreaElement.GetAttribute("autocomplete"));
            Assert.Contains("e-text1", containerElement.ClassName);
            Assert.Equal("6", textAreaElement.GetAttribute("rows"));
            Assert.Equal("11", textAreaElement.GetAttribute("cols"));
            textBox.SetParametersAndRender(("HtmlAttributes", new Dictionary<string, object>() { { "type", "number" }, { "autocomplete", "off" }, { "max", 10 }, { "min", 1 } }));
            textAreaElement = textBox.Find("textarea");
            Assert.Equal("number", textAreaElement.GetAttribute("type"));
            Assert.Equal("off", textAreaElement.GetAttribute("autocomplete"));
            Assert.Equal("10", textAreaElement.GetAttribute("max"));
            Assert.Equal("1", textAreaElement.GetAttribute("min"));
            textBox.SetParametersAndRender(("HtmlAttributes", new Dictionary<string, object>() { { "maxlength", 5 } }));
            textAreaElement = textBox.Find("textarea");
            Assert.Equal("5", textAreaElement.GetAttribute("maxlength"));
        }

        [Fact(Timeout = 10000)]
        public void InputAttributes()
        {
            Dictionary<string, object> inputAttributes = new Dictionary<string, object>() { { "name", "textbox" }, { "required", "true" }, { "value", "Test" } };
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.InputAttributes, inputAttributes));
            var textAreaElement = textBox.Find("textarea");
            var containerElement = textAreaElement.ParentElement;
            Assert.Contains("textbox", textAreaElement.GetAttribute("name"));
            Assert.Contains("true", textAreaElement.GetAttribute("required"));
            textBox.SetParametersAndRender(("InputAttributes", new Dictionary<string, object>() { { "name", "textbox1" }, { "required", "false" }, { "value", "Test1" } }));
            textAreaElement = textBox.Find("textarea");
            containerElement = textAreaElement.ParentElement;
            Assert.Contains("textbox1", textAreaElement.GetAttribute("name"));
            Assert.Contains("false", textAreaElement.GetAttribute("required"));
            textBox.SetParametersAndRender(("InputAttributes", new Dictionary<string, object>() { { "type", "number" }, { "autocomplete", "off" }, { "max", 10 }, { "min", 1 } }));
            textAreaElement = textBox.Find("textarea");
            Assert.Equal("number", textAreaElement.GetAttribute("type"));
            Assert.Equal("off", textAreaElement.GetAttribute("autocomplete"));
            Assert.Equal("10", textAreaElement.GetAttribute("max"));
            Assert.Equal("1", textAreaElement.GetAttribute("min"));
            textBox.SetParametersAndRender(("InputAttributes", new Dictionary<string, object>() { { "maxlength", 5 } }));
            textAreaElement = textBox.Find("textarea");
            Assert.Equal("5", textAreaElement.GetAttribute("maxlength"));
        }

        [Fact(Timeout = 10000)]
        public void RoleAttribute()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.ShowClearButton, true));
            var textAreaElement = textBox.Find("textarea");
            var containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.True(textAreaElement.HasAttribute("role"));
            Assert.Contains("textbox", textAreaElement.GetAttribute("role"));
            var clearIconElement = containerElement.QuerySelector(".e-close");
            Assert.NotNull(clearIconElement);
            textBox.SetParametersAndRender(("Multiline", false));
            containerElement = textBox.Find("input").ParentElement;
            Assert.NotNull(containerElement);
            var inputElement = textBox.Find("input");
            clearIconElement = containerElement.Children[1];
            Assert.Contains("textbox", inputElement.GetAttribute("role"));
            Assert.Contains("button", clearIconElement.GetAttribute("role"));
        }

        #endregion

        #region Clear Button Tests

        [Fact(Timeout = 10000)]
        public void ClearButton()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.ShowClearButton, true));
            var textAreaElement = textBox.Find("textarea");
            var containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            var clearIconElement = containerElement.QuerySelector(".e-close");
            Assert.NotNull(clearIconElement);
            textBox.SetParametersAndRender(parameter => parameter.Add(p => p.Multiline, false).Add(p => p.Value, "Test"));
            containerElement = textBox.Find("input").ParentElement;
            Assert.NotNull(containerElement);
            var inputElement = textBox.Find("input");
            clearIconElement = containerElement.Children[1];
            Assert.Contains("Test", inputElement.GetAttribute("value"));
            Assert.Contains("Test", textBox.Instance.Value);
            clearIconElement.MouseDown();
            Assert.Null(inputElement.GetAttribute("value"));
            Assert.Null(textBox.Instance.Value);
        }

        [Fact(Timeout = 10000)]
        public void StaticClearIcon()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.CssClass, "e-static-clear").Add(p => p.ShowClearButton, true));
            var textAreaElement = textBox.Find("textarea");
            var containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            var clearIconElement = containerElement.QuerySelector(".e-close");
            Assert.NotNull(clearIconElement);

        }

        #endregion

        #region Floating Label Tests

        [Fact(Timeout = 10000)]
        public void FloatingLabel()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.Placeholder, "Enter the value"));
            var textAreaElement = textBox.Find("textarea");
            var containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.DoesNotContain("e-float-text", containerElement.ClassName);
            textBox.SetParametersAndRender(("FloatLabelType", FloatLabelType.Auto));
            containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("e-float-input", containerElement.ClassName);
            var floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-bottom", floatLabelElement.ClassName);
            textBox.SetParametersAndRender(("Value", "Test"));
            textAreaElement = textBox.Find("textarea");
            containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-top", floatLabelElement.ClassName);
            textBox.SetParametersAndRender(("FloatLabelType", FloatLabelType.Always));
            containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("e-float-input", containerElement.ClassName);
            floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-top", floatLabelElement.ClassName);
            textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Always));
            textAreaElement = textBox.Find("textarea");
            textAreaElement.Focus();
            containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-top", floatLabelElement.ClassName);
            textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Never));
            textAreaElement = textBox.Find("textarea");
            containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.DoesNotContain("e-float-input", containerElement.ClassName);
            floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.Null(floatLabelElement);
            textBox.SetParametersAndRender(("FloatLabelType", FloatLabelType.Auto));
            containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("e-float-input", containerElement.ClassName);
            floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-bottom", floatLabelElement.ClassName);
            textAreaElement = textBox.Find("textarea");
            textAreaElement.Focus();
            containerElement = textAreaElement.ParentElement;
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
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.Placeholder, "Enter the value"));
            var textAreaElement = textBox.Find("textarea");
            textAreaElement.Focus();
            var containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("e-input-focus", containerElement.ClassName);
        }

        #endregion

        #region Value and Data Binding Tests

        [Fact(Timeout = 10000)]
        public void ValueBinding()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.Value, "Test"));
            Assert.Equal("Test", textBox.Find("textarea").GetAttribute("value"));
            Assert.Equal("Test", textBox.Instance.Value);
            textBox.SetParametersAndRender(("Value", "Test1"));
            Assert.Equal("Test1", textBox.Find("textarea").GetAttribute("value"));
            Assert.Equal("Test1", textBox.Instance.Value);
            textBox.Find("textarea").HasAttribute("role");
        }

        #endregion

        #region Input Type Tests

        [Fact(Timeout = 10000)]
        public void InputTypes()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Multiline, true).Add(p => p.Type, InputType.Email));
            var textAreaElement = textBox.Find("textarea");
            //Type attribute cannot be applied for multiline textbox.
            Assert.Null(textAreaElement.GetAttribute("type"));
        }

        #endregion

        #region Comprehensive Tests

        [Fact(Timeout = 10000)]
        public void AllAPICombination()
        {
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>() { { "class", "e-text" } };
            Dictionary<string, object> inputAttributes = new Dictionary<string, object>() { { "name", "textbox" } };
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.ID, "Text-1").Add(p => p.Autocomplete, AutoComplete.Off)
            .Add(p => p.CssClass, "e-test").Add(p => p.EnablePersistence, true)
            .Add(p => p.Placeholder, "Enter the value").Add(p => p.Disabled, false).Add(p => p.FloatLabelType, FloatLabelType.Always)
            .Add(p => p.HtmlAttributes, htmlAttributes).Add(p => p.InputAttributes, inputAttributes).Add(p => p.Multiline, true).Add(p => p.ReadOnly, false)
            .Add(p => p.ShowClearButton, true).Add(p => p.Value, "Test").Add(p => p.Width, "600px").Add(p => p.TabIndex, 1).Add(p => p.Type, InputType.Email));
            var textAreaElement = textBox.Find("textarea");
            var containerElement = textAreaElement.ParentElement;
            Assert.NotNull(containerElement);
            var clearIconElement = containerElement.QuerySelector(".e-close");
            var floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.Equal("Text-1", textAreaElement.GetAttribute("id"));
            Assert.Equal("off", textAreaElement.GetAttribute("autocomplete"));
            Assert.Contains("e-test", containerElement.ClassName);
            Assert.DoesNotContain("e-disabled", containerElement.ClassName);
            Assert.DoesNotContain("e-disabled", textAreaElement.ClassName);
            Assert.Contains("e-float-input", containerElement.ClassName);
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-top", floatLabelElement.ClassName);
            Assert.Contains("e-text", containerElement.ClassName);
            Assert.Contains("textbox", textAreaElement.GetAttribute("name"));
            Assert.False(textAreaElement.HasAttribute("placeholder"));
            Assert.False(textAreaElement.HasAttribute("readonly"));
            Assert.NotNull(clearIconElement);
            Assert.Equal("Test", textAreaElement.GetAttribute("value"));
            Assert.Contains("width: 600px", containerElement.GetStyle().CssText.Trim());
            Assert.Equal("1", textAreaElement.GetAttribute("tabindex"));
            Assert.False(textAreaElement.HasAttribute("type"));
        }

        #endregion
    }
}
