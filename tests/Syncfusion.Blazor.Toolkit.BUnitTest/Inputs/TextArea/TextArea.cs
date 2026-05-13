using AngleSharp.Css.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Toolkit.Inputs;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.TextArea
{
    public class TextArea : BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "Default render")]
        public void DefaultRender()
        {
            var textArea = RenderComponent<SfTextArea>();
            var textAreaEle = textArea.Find("textarea");
            Assert.NotNull(textAreaEle);
            Assert.Contains("e-textarea", textAreaEle.ClassName);
            Assert.Contains("e-resize-xy", textAreaEle.ClassName);
            Assert.NotNull(textAreaEle.ParentElement);
            Assert.Contains("e-multi-line-input", textAreaEle.ParentElement.ClassName);
            Assert.Contains("e-auto-width", textAreaEle.ParentElement.ClassName);
            Assert.Contains("e-control-container", textAreaEle.ParentElement.ClassName);
            Assert.Contains("e-input-group", textAreaEle.ParentElement.ClassName);
            Assert.True(textAreaEle.ParentElement.NodeName == "DIV");
            Assert.Equal("0", textAreaEle.GetAttribute("tabindex"));
        }
        [Fact(Timeout = 10000, DisplayName = "Default params")]
        public void DefaultParams()
        {
            var textArea = RenderComponent<SfTextArea>();
            var textAreaEle = textArea.Find("textarea");
            Assert.Null(textArea.Instance.Value);
            Assert.Null(textArea.Instance.Placeholder);
            Assert.Null(textArea.Instance.CssClass);
            Assert.Null(textArea.Instance.Width);
            Assert.Equal(FloatLabelType.Never, textArea.Instance.FloatLabelType);
            Assert.False(textArea.Instance.ReadOnly);
            Assert.False(textArea.Instance.ShowClearButton);
            Assert.False(textArea.Instance.EnablePersistence);
            Assert.False(textArea.Instance.Disabled);

            Assert.Equal("2", textAreaEle.GetAttribute("rows"));
            Assert.Equal("20", textAreaEle.GetAttribute("cols"));
            Assert.Equal("true", textAreaEle.GetAttribute("aria-multiline"));
            Assert.Equal("false", textAreaEle.GetAttribute("aria-disabled"));
        }

        [Fact(Timeout = 10000, DisplayName = "CssClass updates")]
        public void CssClassUpdates()
        {
            var textArea = RenderComponent<SfTextArea>(parameters => parameters.Add(p => p.CssClass, "sample-css"));
            var containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            Assert.Contains("sample-css", containerEle.ClassName);
            textArea.SetParametersAndRender(("CssClass", "test test1"));
            containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            Assert.Contains("test test1", containerEle.ClassName);
            Assert.DoesNotContain("sample-css", containerEle.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "TabIndex updates")]
        public void TabIndexUpdates()
        {
            var textArea = RenderComponent<SfTextArea>(parameters => parameters.Add(p => p.TabIndex, 1));
            var textAreaEle = textArea.Find("textarea");
            Assert.Equal("1", textAreaEle.GetAttribute("tabindex"));
            textArea.SetParametersAndRender(("TabIndex", 3));
            textAreaEle = textArea.Find("textarea");
            Assert.Equal("3", textAreaEle.GetAttribute("tabindex"));
        }

        [Fact(DisplayName = "ReadOnly toggles", Timeout = 10000)]
        public void ReadOnlyToggles()
        {
            var textArea = RenderComponent<SfTextArea>(("ReadOnly", true));
            var textAreaEle = textArea.Find("textarea");
            Assert.True(textAreaEle.HasAttribute("readonly"));
            textArea.SetParametersAndRender(("ReadOnly", false));
            Assert.False(textAreaEle.HasAttribute("readonly"));
        }

        [Fact(Timeout = 10000, DisplayName = "ResizeMode toggles")]
        public void ResizeModeToggles()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.ResizeMode, Resize.Vertical));
            var textAreaEle = textArea.Find("textarea");
            Assert.Contains("e-resize-y", textAreaEle.ClassName);
            textArea.SetParametersAndRender(("ResizeMode", Resize.Horizontal));
            Assert.Contains("e-resize-x", textAreaEle.ClassName);
            textArea.SetParametersAndRender(("ResizeMode", Resize.None));
            Assert.Contains("e-resize-none", textAreaEle.ClassName);
            textArea.SetParametersAndRender(("ResizeMode", Resize.Both));
            Assert.Contains("e-resize-xy", textAreaEle.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "MaxLength updates")]
        public void MaxLengthUpdates()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.MaxLength, 10));
            var textAreaEle = textArea.Find("textarea");
            Assert.True(textAreaEle.HasAttribute("maxlength"));
            Assert.Equal("10", textAreaEle.GetAttribute("maxlength"));
            textArea.SetParametersAndRender(("MaxLength", 20));
            Assert.Equal("20", textAreaEle.GetAttribute("maxlength"));
        }

        [Fact(Timeout = 10000, DisplayName = "Rows and Columns update")]
        public void RowsColsUpdate()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.RowCount, 3).Add(p => p.ColumnCount, 25));
            var textAreaEle = textArea.Find("textarea");
            Assert.True(textAreaEle.HasAttribute("rows"));
            Assert.True(textAreaEle.HasAttribute("cols"));
            Assert.Equal("3", textAreaEle.GetAttribute("rows"));
            Assert.Equal("25", textAreaEle.GetAttribute("cols"));
            textArea.SetParametersAndRender(("RowCount", 5));
            textArea.SetParametersAndRender(("ColumnCount", 30));
            Assert.Equal("5", textAreaEle.GetAttribute("rows"));
            Assert.Equal("30", textAreaEle.GetAttribute("cols"));
        }

        [Fact(Timeout = 10000, DisplayName = "Width and resize")]
        public void WidthAndResize()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.Width, "300px"));
            var textAreaEle = textArea.Find("textarea");
            var containerEle = textAreaEle.ParentElement;
            Assert.NotNull(containerEle);
            Assert.DoesNotContain("e-auto-width", containerEle.ClassName);
            Assert.Contains("width: 300px", containerEle.GetStyle().CssText.Trim());
            Assert.Contains("e-resize-y", textAreaEle.ClassName);
            textArea.SetParametersAndRender(("Width", "600px"));
            textArea.SetParametersAndRender(("ResizeMode", Resize.Horizontal));
            textAreaEle = textArea.Find("textarea");
            containerEle = textAreaEle.ParentElement;
            Assert.Contains("width: 600px", containerEle.GetStyle().CssText.Trim());
            Assert.Contains("e-resize-none", textAreaEle.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Placeholder updates")]
        public void PlaceholderUpdates()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.Placeholder, "Enter the value"));
            var textAreaEle = textArea.Find("textarea");
            Assert.Contains("Enter the value", textAreaEle.GetAttribute("placeholder"));
            textArea.SetParametersAndRender(("Placeholder", "Enter text"));
            Assert.Contains("Enter text", textAreaEle.GetAttribute("placeholder"));
        }

        [Fact(Timeout = 10000, DisplayName = "Enabled toggles")]
        public void EnabledToggles()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.Disabled, false));
            var textAreaEle = textArea.Find("textarea");
            var containerEle = textAreaEle.ParentElement;
            Assert.NotNull(containerEle);
            Assert.DoesNotContain("e-disabled", containerEle.ClassName);
            Assert.DoesNotContain("e-disabled", textAreaEle.ClassName);
            Assert.Equal("false", textAreaEle.GetAttribute("aria-disabled"));
            textArea.SetParametersAndRender(("Disabled", true));
            textAreaEle = textArea.Find("textarea");
            containerEle = textAreaEle.ParentElement;
            Assert.NotNull(containerEle);
            Assert.Contains("e-disabled", containerEle.ClassName);
            Assert.Contains("e-disabled", textAreaEle.ClassName);
            Assert.Contains("e-resize-none", textAreaEle.ClassName);
            Assert.Equal("true", textAreaEle.GetAttribute("aria-disabled"));
            textArea.SetParametersAndRender(("Disabled", false));
            textAreaEle = textArea.Find("textarea");
            containerEle = textAreaEle.ParentElement;
            Assert.NotNull(containerEle);
            Assert.DoesNotContain("e-disabled", containerEle.ClassName);
            Assert.DoesNotContain("e-disabled", textAreaEle.ClassName);
            Assert.Contains("e-resize-xy", textAreaEle.ClassName);
            Assert.Equal("false", textAreaEle.GetAttribute("aria-disabled"));
        }

        [Fact(Timeout = 10000, DisplayName = "Disabled render")]
        public void DisabledRender()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.Disabled, true).Add(p => p.ResizeMode, Resize.None));
            var textAreaEle = textArea.Find("textarea");
            var containerEle = textAreaEle.ParentElement;
            Assert.NotNull(containerEle);
            Assert.Contains("e-disabled", containerEle.ClassName);
            Assert.Contains("e-disabled", textAreaEle.ClassName);
            Assert.Contains("e-resize-none", textAreaEle.ClassName);
            Assert.Equal("true", textAreaEle.GetAttribute("aria-disabled"));
        }

        [Fact(Timeout = 10000, DisplayName = "HtmlAttributes update")]
        public void HtmlAttributesUpdate()
        {
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>() { { "name", "textarea" }, { "required", "true" }, { "class", "e-text" }, { "autofocus", "" } };
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.HtmlAttributes, htmlAttributes));
            var textAreaEle = textArea.Find("textarea");
            var containerEle = textAreaEle.ParentElement;
            Assert.NotNull(containerEle);
            Assert.Contains("textarea", textAreaEle.GetAttribute("name"));
            Assert.Contains("true", textAreaEle.GetAttribute("required"));
            Assert.Contains("e-text", containerEle.ClassName);
            Assert.True(textAreaEle.HasAttribute("autofocus"));
            textArea.SetParametersAndRender(("HtmlAttributes", new Dictionary<string, object>() { { "name", "textarea1" }, { "required", "false" }, { "class", "e-text1" } }));
            textAreaEle = textArea.Find("textarea");
            containerEle = textAreaEle.ParentElement;
            Assert.NotNull(containerEle);
            Assert.Contains("textarea1", textAreaEle.GetAttribute("name"));
            Assert.Contains("false", textAreaEle.GetAttribute("required"));
            Assert.Contains("e-text1", containerEle.ClassName);
            textArea.SetParametersAndRender(("HtmlAttributes", new Dictionary<string, object>() { { "maxlength", 5 } }));
            textAreaEle = textArea.Find("textarea");
            containerEle = textAreaEle.ParentElement;
            Assert.Equal("5", textAreaEle.GetAttribute("maxlength"));
        }

        [Fact(Timeout = 10000, DisplayName = "Clears value using clear button")]
        public void ClearButton()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.ShowClearButton, true));
            var containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            Assert.True(containerEle.Children.Length > 1);
            var clearEle = containerEle.Children[1];
            Assert.Contains("e-close", clearEle.ClassName);
            Assert.Contains("e-clear-icon-hide", clearEle.ClassName);
            textArea.SetParametersAndRender(("ShowClearButton", false));
            containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            clearEle = containerEle.QuerySelector(".e-close");
            Assert.Null(clearEle);
            textArea.SetParametersAndRender(parameter => parameter.Add(p => p.ShowClearButton, true).Add(p => p.Value, "Test"));
            containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            var textAreaEle = textArea.Find("textarea");
            clearEle = containerEle.Children[1];
            Assert.Contains("e-close", clearEle.ClassName);
            Assert.Contains("e-clear-icon-hide", clearEle.ClassName);
            Assert.Contains("Test", textAreaEle.GetAttribute("value"));
            Assert.Contains("Test", textArea.Instance.Value);
            clearEle.MouseDown();
            Assert.Null(textAreaEle.GetAttribute("value"));
            Assert.Null(textArea.Instance.Value);
        }

        [Fact(Timeout = 10000, DisplayName = "Float label Updates")]
        public void FloatingLabel()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.Placeholder, "Enter the value"));
            var containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            Assert.DoesNotContain("e-float-text", containerEle.ClassName);
            textArea.SetParametersAndRender(("FloatLabelType", FloatLabelType.Auto));
            containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            Assert.Contains("e-float-input", containerEle.ClassName);
            var floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-bottom", floatEle.ClassName);
            textArea.SetParametersAndRender(("Value", "Test"));
            var textAreaEle = textArea.Find("textarea");
            containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-top", floatEle.ClassName);
            textArea.SetParametersAndRender(("FloatLabelType", FloatLabelType.Always));
            containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            Assert.Contains("e-float-input", containerEle.ClassName);
            floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-top", floatEle.ClassName);
            textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Always));
            textAreaEle = textArea.Find("textarea");
            textAreaEle.Focus();
            containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-top", floatEle.ClassName);
            textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Never));
            containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            Assert.DoesNotContain("e-float-input", containerEle.ClassName);
            floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.Null(floatEle);
            textArea.SetParametersAndRender(("FloatLabelType", FloatLabelType.Auto));
            containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            Assert.Contains("e-float-input", containerEle.ClassName);
            floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-bottom", floatEle.ClassName);
            textAreaEle = textArea.Find("textarea");
            textAreaEle.Focus();
            containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            floatEle = containerEle.QuerySelector(".e-float-text");
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-top", floatEle.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Sets focus state on input")]
        public void FocusComponent()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.Placeholder, "Enter the value"));
            var textAreaEle = textArea.Find("textarea");
            textAreaEle.Focus();
            var containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            Assert.Contains("e-input-focus", containerEle.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Value Binding")]
        public void ValueBinding()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.Value, "Test"));
            Assert.Equal("Test", textArea.Find("textarea").GetAttribute("value"));
            Assert.Equal("Test", textArea.Instance.Value);
            textArea.SetParametersAndRender(("Value", "Test1"));
            Assert.Equal("Test1", textArea.Find("textarea").GetAttribute("value"));
            Assert.Equal("Test1", textArea.Instance.Value);
            textArea.Find("textarea").HasAttribute("role");
        }

        [Fact(Timeout = 10000, DisplayName = "Clears value using static clear icon")]
        public void StaticClearIcon()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.CssClass, "e-static-clear").Add(p => p.ShowClearButton, true));
            var containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            Assert.True(containerEle.Children.Length > 1);
            var clearEle = containerEle.Children[1];
            var textAreaEle = textArea.Find("textarea");
            Assert.Contains("e-static-clear", containerEle.ClassName);
            Assert.Contains("e-close", clearEle.ClassName);
            Assert.Contains("e-clear-icon-hide", clearEle.ClassName);
            textArea.SetParametersAndRender(("Value", "Test"));
            textAreaEle = textArea.Find("textarea");
            Assert.Equal("Test", textArea.Instance.Value);
            Assert.Equal("Test", textAreaEle.GetAttribute("value"));
            clearEle.MouseDown();
            containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            clearEle = containerEle.Children[1];
            textAreaEle = textArea.Find("textarea");
            Assert.Null(textArea.Instance.Value);
            Assert.Null(textAreaEle.GetAttribute("value"));
            Assert.Contains("e-static-clear", containerEle.ClassName);
            Assert.Contains("e-close", clearEle.ClassName);
            Assert.Contains("e-clear-icon-hide", clearEle.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Renders all the Properties")]
        public void AllPropertiesRender()
        {
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>() {  { "class", "e-text" } };
            var textArea = RenderComponent<SfTextArea>(parameter => parameter
                .Add(p => p.ID, "Text-1")
                .Add(p => p.Disabled, false)
                .Add(p => p.ReadOnly, false)
                .Add(p => p.ResizeMode, Resize.Horizontal)
                .Add(p => p.CssClass, "e-test")
                .Add(p => p.EnablePersistence, true)
                .Add(p => p.Placeholder, "Enter the value")
                .Add(p => p.FloatLabelType, FloatLabelType.Always)
                .Add(p => p.HtmlAttributes, htmlAttributes)
                .Add(p => p.ShowClearButton, true)
                .Add(p => p.Value, "Test")
                .Add(p => p.Width, "600px")
                .Add(p => p.TabIndex, 1)
                .Add(p => p.MaxLength, 20)
                .Add(p => p.RowCount, 4)
                .Add(p => p.ColumnCount, 28)
            );
            var textAreaEle = textArea.Find("textarea");
            var containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            var clearEle = containerEle.Children[3];
            var floatEle = containerEle.QuerySelector(".e-float-text");
            var style = containerEle.GetStyle();
            Assert.NotNull(style);
            Assert.Equal("Text-1", textAreaEle.GetAttribute("id"));
            Assert.Contains("e-test", containerEle.ClassName);
            Assert.DoesNotContain("e-disabled", containerEle.ClassName);
            Assert.DoesNotContain("e-disabled", textAreaEle.ClassName);
            Assert.Contains("e-float-input", containerEle.ClassName);
            Assert.NotNull(floatEle);
            Assert.Contains("e-label-top", floatEle.ClassName);
            Assert.Contains("e-text", containerEle.ClassName);
            Assert.False(textAreaEle.HasAttribute("placeholder"));
            Assert.False(textAreaEle.HasAttribute("readonly"));
            Assert.Contains("e-close", clearEle.ClassName);
            Assert.Equal("Test", textAreaEle.GetAttribute("value"));
            Assert.Contains("width: 600px", style.CssText.Trim());
            Assert.Equal("1", textAreaEle.GetAttribute("tabindex"));
            Assert.DoesNotContain("e-auto-width", containerEle.ClassName);
            Assert.Contains("e-resize-none", textAreaEle.ClassName);
            Assert.Equal("20", textAreaEle.GetAttribute("maxlength"));
            Assert.Equal("4", textAreaEle.GetAttribute("rows"));
            Assert.Equal("28", textAreaEle.GetAttribute("cols"));
        }

        [Fact(Timeout = 10000, DisplayName = "Applies error class on validation failure")]
        public async Task FormInvalid_ErrorClass()
        {
            var model = new StringEditorTypes();

            var textArea = RenderComponent<EditForm>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.ChildContent, context => builder =>
                {
                    // 1. Add DataAnnotationsValidator
                    builder.OpenComponent<DataAnnotationsValidator>(0);
                    builder.CloseComponent();

                    // 2. Add SfTextArea
                    builder.OpenComponent<SfTextArea>(1);
                    builder.AddAttribute(2, "Value", model.TextAreaField);
                    builder.AddAttribute(3, "ValueChanged", EventCallback.Factory.Create<string>(this, val => model.TextAreaField = val));
                    builder.AddAttribute(4, "ValueExpression", (System.Linq.Expressions.Expression<Func<string>>)(() => model.TextAreaField));
                    builder.CloseComponent();

                    // 3. Add Submit Button
                    builder.OpenElement(5, "button");
                    builder.AddAttribute(6, "type", "submit");
                    builder.AddContent(7, "Submit");
                    builder.CloseElement();
                })
            );
            var buttonElement = textArea.Find("button");
            buttonElement.Click();
            var textAreaEle = textArea.Find("textarea");
            Assert.NotNull(textAreaEle.ParentElement);
            Assert.Contains("e-error", textAreaEle.ParentElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Applies success class on valid input")]
        public async Task FormModifiedValid_SuccessClass()
        {
            var model = new StringEditorTypes();
            var textArea = RenderComponent<EditForm>(parameters => parameters
                .Add(p => p.Model, model)
                .Add(p => p.ChildContent, context => builder =>
                {
                    // 1. Add DataAnnotationsValidator
                    builder.OpenComponent<DataAnnotationsValidator>(0);
                    builder.CloseComponent();

                    // 2. Add First SfTextArea
                    builder.OpenComponent<SfTextArea>(1);
                    builder.AddAttribute(2, "Value", model.TextAreaField);
                    builder.AddAttribute(3, "ValueChanged", EventCallback.Factory.Create<string>(this, val => model.TextAreaField = val));
                    builder.AddAttribute(4, "ValueExpression", (System.Linq.Expressions.Expression<Func<string>>)(() => model.TextAreaField));
                    builder.CloseComponent();

                    // 3. Add Second SfTextArea
                    builder.OpenComponent<SfTextArea>(5);
                    builder.AddAttribute(6, "Value", model.TextAreaField);
                    builder.AddAttribute(7, "ValueChanged", EventCallback.Factory.Create<string>(this, val => model.TextAreaField = val));
                    builder.AddAttribute(8, "ValueExpression", (System.Linq.Expressions.Expression<Func<string>>)(() => model.TextAreaField));
                    builder.CloseComponent();
                })
            );
            var textAreas = textArea.FindAll("textarea");
            textAreas[0].Change("Test");
            textAreas[1].Change("Test");
            await Task.Delay(100);
            var textAreaElement = textArea.FindAll("textarea");
            Assert.Contains("e-success", textAreaElement[0].ParentElement?.ClassName);
            Assert.Contains("e-success", textAreaElement[1].ParentElement?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Renders default aria-label")]
        public void Render_DefaultAriaLabel()
        {
            var textArea = RenderComponent<SfTextArea>();
            var ta = textArea.Find("textarea");
            Assert.Equal("textarea", ta.GetAttribute("aria-label"));
        }

        [Fact(Timeout = 10000, DisplayName = "Persistence read normalizes and preserves value")]
        public void Persistence_Read_NormalizesAndPreserves()
        {
            string id = "TA-1";

            // 1) getItem returns "" -> normalize to null -> component Value stays null
            JSInterop.Setup<string>("window.localStorage.getItem", args => args.Arguments.Count == 1 && args.Arguments[0]?.ToString() == id)
                     .SetResult(string.Empty);

            var textArea = RenderComponent<SfTextArea>(ps =>
                ps.Add(p => p.ID, id)
                  .Add(p => p.EnablePersistence, true)
            );
            Assert.Null(textArea.Instance.Value);

            // 2) getItem returns "null" -> normalize to null; do not overwrite non-null Value
            textArea = RenderComponent<SfTextArea>(ps =>
                ps.Add(p => p.ID, id)
                  .Add(p => p.EnablePersistence, true)
                  .Add(p => p.Value, "Syncfusion")
            );
            JSInterop.Setup<string>("window.localStorage.getItem", args => args.Arguments.Count == 1 && args.Arguments[0]?.ToString() == id)
                     .SetResult("null");
            // Re-render to trigger OnAfterRenderAsync again in test context
            textArea.Render();
            Assert.Equal("Syncfusion", textArea.Instance.Value);
        }
    }
    public class StringEditorTypes
    {
        [Required(ErrorMessage = "Please enter a value for TextAreaField")]
        public string TextAreaField { get; set; } = string.Empty;
    }
}
