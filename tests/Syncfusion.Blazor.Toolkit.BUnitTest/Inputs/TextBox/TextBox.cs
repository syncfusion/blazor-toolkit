using AngleSharp.Css.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Toolkit.Inputs;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.TextBox
{
    public class TextBox : BunitTestContext
    {
        #region Initialization Tests

        [Fact(Timeout = 10000)]
        public void DefaultInitialize()
        {
            var textBox = RenderComponent<SfTextBox>();
            var inputElement = textBox.Find("input");
            Assert.NotNull(inputElement.ParentElement);
            Assert.Contains("e-textbox", inputElement.ClassName);
            Assert.Contains("e-control-container", inputElement.ParentElement.ClassName);
            Assert.Contains("e-input-group", inputElement.ParentElement.ClassName);
            Assert.True(inputElement.ParentElement.NodeName == "SPAN");
            Assert.Equal("0", inputElement.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000)]
        public void DefaultValue()
        {
            var textBox = RenderComponent<SfTextBox>();
            Assert.Null(textBox.Instance.Value);
            Assert.Null(textBox.Instance.Placeholder);
            Assert.Null(textBox.Instance.CssClass);
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
            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.CssClass, "sample-css"));
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("sample-css", containerElement.ClassName);
            textBox.SetParametersAndRender(("CssClass", "test test1"));
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("test test1", containerElement.ClassName);
            Assert.DoesNotContain("sample-css", containerElement.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void TabIndex()
        {
            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.TabIndex, 1));
            var inputElement = textBox.Find("input");
            Assert.Equal("1", inputElement.GetAttribute("tabindex"));
            textBox.SetParametersAndRender(("TabIndex", 3));
            inputElement = textBox.Find("input");
            Assert.Equal("3", inputElement.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000)]
        public void ReadOnly()
        {
            var textBox = RenderComponent<SfTextBox>(("ReadOnly", true));
            var inputElement = textBox.Find("input");
            Assert.True(inputElement.HasAttribute("readonly"));
            textBox.SetParametersAndRender(("ReadOnly", false));
            Assert.False(inputElement.HasAttribute("readonly"));
        }

        [Fact(Timeout = 10000)]
        public void WidthChange()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Width, "300px"));
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
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Placeholder, "Enter the value"));
            var inputElement = textBox.Find("input");
            Assert.Contains("Enter the value", inputElement.GetAttribute("placeholder"));
            textBox.SetParametersAndRender(("Placeholder", "Enter text"));
            Assert.Contains("Enter text", inputElement.GetAttribute("placeholder"));
        }

        [Fact(Timeout = 10000)]
        public void Enabled()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Disabled, false));
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
            var textBox = RenderComponent<SfTextBox>();
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
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.HtmlAttributes, htmlAttributes));
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
            Assert.Equal("text", inputElement.GetAttribute("type"));
            Assert.Contains("e-text1", containerElement.ClassName);
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
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.InputAttributes, inputAttributes));
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.Contains("textbox", inputElement.GetAttribute("name"));
            Assert.Contains("true", inputElement.GetAttribute("required"));
            textBox.SetParametersAndRender(("InputAttributes", new Dictionary<string, object>() { { "name", "textbox1" }, { "required", "false" }, { "value", "Test1" } }));
            inputElement = textBox.Find("input");
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
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.ShowClearButton, true));
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
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.ShowClearButton, true));
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
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "e-static-clear").Add(p => p.ShowClearButton, true));
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
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Placeholder, "Enter the value"));
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
            textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Always));
            inputElement = textBox.Find("input");
            inputElement.Focus();
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.NotNull(floatLabelElement);
            Assert.Contains("e-label-top", floatLabelElement.ClassName);
            textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Never));
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
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Placeholder, "Enter the value"));
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
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Value, "Test"));
            Assert.Equal("Test", textBox.Find("input").GetAttribute("value"));
            Assert.Equal("Test", textBox.Instance.Value);
            textBox.SetParametersAndRender(("Value", "Test1"));
            Assert.Equal("Test1", textBox.Find("input").GetAttribute("value"));
            Assert.Equal("Test1", textBox.Instance.Value);
            textBox.Find("input").HasAttribute("role");
        }

        [Fact(Timeout = 10000)]
        public async Task ErrorClassValidation()
        {
            var model = new TestModel();
            RenderFragment<EditContext> child = editContext => builder =>
            {
                builder.OpenComponent<DataAnnotationsValidator>(0);
                builder.CloseComponent();

                builder.OpenComponent<SfTextBox>(1);
                builder.AddAttribute(2, "id", "input1");
                builder.AddAttribute(3, "Value", model.StringField);
                builder.AddAttribute(4, "ValueChanged", EventCallback.Factory.Create<string>(model, __value => model.StringField = __value));
                builder.AddAttribute(5, "ValueExpression", (Expression<Func<string>>)(() => model.StringField));
                builder.CloseComponent();

                builder.OpenComponent<SfTextBox>(6);
                builder.AddAttribute(7, "id", "input2");
                builder.AddAttribute(8, "Value", model.StringField_1);
                builder.AddAttribute(9, "ValueChanged", EventCallback.Factory.Create<string>(model, __value => model.StringField_1 = __value));
                builder.AddAttribute(10, "ValueExpression", (Expression<Func<string>>)(() => model.StringField_1));
                builder.CloseComponent();

                builder.OpenElement(11, "button");
                builder.AddAttribute(12, "type", "submit");
                builder.AddContent(13, "Submit");
                builder.CloseElement();
            };

            var editForm = RenderComponent<EditForm>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.ChildContent, child));

            await editForm.InvokeAsync(() => editForm.Find("button").Click());
            var inputElement = editForm.Find("input");
            Assert.Contains("e-error", inputElement?.ParentElement?.ClassName);
        }

        [Fact(Timeout = 10000)]
        public async Task SuccessClassValidation()
        {
            var model = new TestModel();
            RenderFragment<EditContext> child = editContext => builder =>
            {
                builder.OpenComponent<DataAnnotationsValidator>(0);
                builder.CloseComponent();

                builder.OpenComponent<SfTextBox>(1);
                builder.AddAttribute(2, "id", "input1");
                builder.AddAttribute(3, "Value", model.StringField);
                builder.AddAttribute(4, "ValueChanged", EventCallback.Factory.Create<string>(model, __value => model.StringField = __value));
                builder.AddAttribute(5, "ValueExpression", (Expression<Func<string>>)(() => model.StringField));
                builder.CloseComponent();

                builder.OpenComponent<SfTextBox>(6);
                builder.AddAttribute(7, "id", "input2");
                builder.AddAttribute(8, "Value", model.StringField_1);
                builder.AddAttribute(9, "ValueChanged", EventCallback.Factory.Create<string>(model, __value => model.StringField_1 = __value));
                builder.AddAttribute(10, "ValueExpression", (Expression<Func<string>>)(() => model.StringField_1));
                builder.CloseComponent();
            };

            var editForm = RenderComponent<EditForm>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.ChildContent, child));

            await editForm.InvokeAsync(() =>
            {
                var inputs = editForm.FindAll("input");
                inputs[0].Change("Test");
                inputs[1].Change("Test");
            });

            await Task.Delay(100);
            Assert.Contains("e-success", editForm?.FindAll("input")[0]?.ParentElement?.ClassName);
            Assert.Contains("e-success", editForm?.FindAll("input")[1]?.ParentElement?.ClassName);
        }

        #endregion

        #region Input Type Tests

        [Fact(Timeout = 10000)]
        public void InputTypes()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Type, InputType.Email));
            var inputElement = textBox.Find("input");
            Assert.Equal("email", inputElement.GetAttribute("type"));
            textBox.SetParametersAndRender(("Type", InputType.Number));
            inputElement = textBox.Find("input");
            Assert.Equal("number", inputElement.GetAttribute("type"));
        }

        [Fact(Timeout = 10000)]
        public void MultiLineTextBox()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.ID, "Multiline-ID").Add(p => p.Multiline, true));
            var textAreaElement = textBox.Find("textarea");
            Assert.Contains("Multiline-ID", textAreaElement.GetAttribute("id"));
            Assert.NotNull(textAreaElement.ParentElement);
            Assert.Contains("e-multi-line-input", textAreaElement.ParentElement.ClassName);
        }

        #endregion

        #region Comprehensive Tests

        [Fact(Timeout = 10000)]
        public void AllAPICombination()
        {
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>() { { "class", "e-text" } };
            Dictionary<string, object> inputAttributes = new Dictionary<string, object>() { { "name", "textbox" } };
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.ID, "Text-1").Add(p => p.Autocomplete, AutoComplete.Off).Add(p => p.CssClass, "e-test").Add(p => p.EnablePersistence, true).Add(p => p.Placeholder, "Enter the value").Add(p => p.Disabled, false).Add(p => p.FloatLabelType, FloatLabelType.Always).Add(p => p.HtmlAttributes, htmlAttributes).Add(p => p.InputAttributes, inputAttributes).Add(p => p.Multiline, false).Add(p => p.ReadOnly, false).Add(p => p.ShowClearButton, true).Add(p => p.Value, "Test").Add(p => p.Width, "600px").Add(p => p.TabIndex, 1).Add(p => p.Type, InputType.Email));
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            var clearIconElement = containerElement.Children[3];
            var floatLabelElement = containerElement.QuerySelector(".e-float-text");
            Assert.Equal("Text-1", inputElement.GetAttribute("id"));
            Assert.Equal("off", inputElement.GetAttribute("autocomplete"));
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

        #region Method Coverage Tests

        [Fact(Timeout = 10000)]
        public async Task NonCoveredMethodsAndProperty()
        {
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>() { { "name", "textbox" } };
            Dictionary<string, object> inputAttributes = new Dictionary<string, object>() { { "Value", "Forms Component" } };
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.ID, "Multiline-ID").Add(p => p.HtmlAttributes, htmlAttributes).Add(p => p.InputAttributes, inputAttributes).Add(p => p.TabIndex, 0));
            var inputElement = textBox.Find("input");
            var PersistData = await textBox.Instance.GetPersistDataAsync();
            Assert.Null(PersistData);
            var htmlAttr = textBox.Instance.HtmlAttributes;
            Assert.NotNull(htmlAttr);
            Assert.Contains("textbox", htmlAttr["name"].ToString());
            var inputAttr = textBox.Instance.InputAttributes;
            Assert.NotNull(inputAttr);
            Assert.Contains("Forms Component", inputAttr["Value"].ToString());
            Assert.Equal(0, textBox.Instance.TabIndex);

            Assert.NotNull(inputElement.ParentElement);
            Assert.NotNull(inputElement.ParentElement.Parent);

            var containerClass = inputElement.ParentElement.ToString();
            var rootClass = inputElement.ParentElement.Parent.ToString();

            Assert.NotNull(rootClass);
            Assert.NotNull(containerClass);

            textBox.Instance.UpdateParentClass(rootClass, containerClass);
        }

        [Fact(Timeout = 10000)]
        public async Task UpdateFieldSetStatusMethod()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Disabled, false));
            var inputElement = textBox.Find("input");
            Assert.Equal("false", inputElement.GetAttribute("aria-disabled"));

            await textBox.InvokeAsync(() => textBox.Instance.UpdateFieldSetStatus(true));
            inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("e-disabled", containerElement.ClassName);
            Assert.Equal("true", inputElement.GetAttribute("aria-disabled"));

            await textBox.InvokeAsync(() => textBox.Instance.UpdateFieldSetStatus(false));
            inputElement = textBox.Find("input");
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.DoesNotContain("e-disabled", containerElement.ClassName);
            Assert.Equal("false", inputElement.GetAttribute("aria-disabled"));
        }

        #endregion

        #region Event Tests

        [Fact(Timeout = 10000)]
        public async Task CreatedEventIsInvokedOnFirstRender()
        {
            bool createdCalled = false;
            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.Created, EventCallback.Factory.Create<object>(this, (object args) => { createdCalled = true; })));
            await Task.Delay(50);
            Assert.True(createdCalled);
        }

        [Fact(Timeout = 10000)]
        public async Task ValueChangeEventReceivesChangedEventArgs()
        {
            ChangedEventArgs? received = null;
            var textBox = RenderComponent<SfTextBox>(parameters => parameters.Add(p => p.ValueChange, EventCallback.Factory.Create<ChangedEventArgs>(this, (ChangedEventArgs args) => { received = args; })));
            var inputElement = textBox.Find("input");
            inputElement.Change("New Value");

            await Task.Delay(50);
            Assert.NotNull(received);
            Assert.Equal("New Value", received.Value);
        }

        #endregion

        #region Edge Case Tests

        [Fact(Timeout = 10000, DisplayName = "The dynamically added CSS class is not removed from the wrapper when rendering the TextBox as a custom component.")]
        public void DynamicCssClassWithSpace()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.CssClass, "custom "));
            var inputElement = textBox.Find("input");
            var parentElement = inputElement.ParentElement;
            Assert.NotNull(parentElement);
            Assert.Equal("custom ", textBox.Instance.CssClass);
            Assert.True(parentElement.ClassList.Contains("custom"));
            textBox.SetParametersAndRender(("CssClass", "custom1"));
            parentElement = inputElement.ParentElement;
            Assert.NotNull(parentElement);
            Assert.False(parentElement.ClassList.Contains("custom"));
            Assert.True(parentElement.ClassList.Contains("custom1"));
            textBox.SetParametersAndRender(("CssClass", null));
            parentElement = inputElement.ParentElement;
            Assert.NotNull(parentElement);
            Assert.False(parentElement.ClassList.Contains("custom"));
            Assert.False(parentElement.ClassList.Contains("custom1"));
            textBox.SetParametersAndRender(("CssClass", "custom1   custom  "));
            parentElement = inputElement.ParentElement;
            Assert.NotNull(parentElement);
            Assert.True(parentElement.ClassList.Contains("custom"));
            Assert.True(parentElement.ClassList.Contains("custom1"));
            textBox.SetParametersAndRender(("CssClass", "custom2"));
            parentElement = inputElement.ParentElement;
            Assert.NotNull(parentElement);
            Assert.True(parentElement.ClassList.Contains("custom2"));
        }

        #endregion
    }

    public class TestModel
    {
        [Required]
        public string StringField { get; set; } = default!;

        [Required]
        public string StringField_1 { get; set; } = default!;
    }
}
