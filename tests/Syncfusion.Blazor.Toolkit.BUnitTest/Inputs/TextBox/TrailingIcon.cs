using Xunit;
using Bunit;
using static Bunit.ComponentParameterFactory;
using Syncfusion.Blazor.Toolkit.Inputs;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Buttons;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.TextBox
{
    public class TrailingIcon : BunitTestContext
    {
        #region Basic Icon Tests

        [Fact(Timeout = 10000, DisplayName = "Adding single trail icon")]
        public async Task TrailIcon()
        {
            //Appending the trail icon
            var textBox = RenderComponent<SfTextBox>();
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            var inputElement = textBox.Find("input");
            inputElement.Focus();
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            var iconElement = containerElement.Children[1];
            Assert.Contains("e-input-group-icon", iconElement.ClassName);
            Assert.Contains("e-date-icon", iconElement.ClassName);
            Assert.Equal(2, containerElement.ChildElementCount);
            //Prepending the trail icon
            textBox = RenderComponent<SfTextBox>();
            await textBox.Instance.AddIconAsync("prepend", "e-date-icon");
            inputElement = textBox.Find("input");
            inputElement.Focus();
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            iconElement = containerElement.Children[0];
            Assert.Contains("e-input-group-icon", iconElement.ClassName);
            Assert.Contains("e-date-icon", iconElement.ClassName);
            Assert.Equal(2, containerElement.ChildElementCount);
        }

        [Fact(Timeout = 10000, DisplayName = "Multiple trail icons")]
        public async Task MultipleTrailIcon()
        {
            //Appending the trail icon
            var textBox = RenderComponent<SfTextBox>();
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            var inputElement = textBox.Find("input");
            inputElement.Focus();
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            var iconElement1 = containerElement.Children[1];
            var iconElement2 = containerElement.Children[2];
            var iconElement3 = containerElement.Children[3];
            var iconElement4 = containerElement.Children[4];
            var iconElement5 = containerElement.Children[5];
            Assert.Contains("e-input-group-icon", iconElement1.ClassName);
            Assert.Contains("e-date-icon", iconElement1.ClassName);
            Assert.Contains("e-date-icon", iconElement2.ClassName);
            Assert.Contains("e-input-group-icon", iconElement2.ClassName);
            Assert.Contains("e-date-icon", iconElement3.ClassName);
            Assert.Contains("e-input-group-icon", iconElement3.ClassName);
            Assert.Contains("e-date-icon", iconElement4.ClassName);
            Assert.Contains("e-input-group-icon", iconElement4.ClassName);
            Assert.Contains("e-date-icon", iconElement5.ClassName);
            Assert.Contains("e-input-group-icon", iconElement5.ClassName);
            Assert.Equal(6, containerElement.ChildElementCount);
            //Prepending the trail icon
            textBox = RenderComponent<SfTextBox>();
            await textBox.Instance.AddIconAsync("prepend", "e-date-icon");
            await textBox.Instance.AddIconAsync("prepend", "e-date-icon");
            await textBox.Instance.AddIconAsync("prepend", "e-date-icon");
            await textBox.Instance.AddIconAsync("prepend", "e-date-icon");
            await textBox.Instance.AddIconAsync("prepend", "e-date-icon");
            inputElement = textBox.Find("input");
            inputElement.Focus();
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            iconElement1 = containerElement.Children[0];
            iconElement2 = containerElement.Children[1];
            iconElement3 = containerElement.Children[2];
            iconElement4 = containerElement.Children[3];
            iconElement5 = containerElement.Children[4];
            Assert.Contains("e-input-group-icon", iconElement1.ClassName);
            Assert.Contains("e-date-icon", iconElement1.ClassName);
            Assert.Contains("e-date-icon", iconElement2.ClassName);
            Assert.Contains("e-input-group-icon", iconElement2.ClassName);
            Assert.Contains("e-date-icon", iconElement3.ClassName);
            Assert.Contains("e-input-group-icon", iconElement3.ClassName);
            Assert.Contains("e-date-icon", iconElement4.ClassName);
            Assert.Contains("e-input-group-icon", iconElement4.ClassName);
            Assert.Contains("e-date-icon", iconElement5.ClassName);
            Assert.Contains("e-input-group-icon", iconElement5.ClassName);
            Assert.Equal(6, containerElement.ChildElementCount);
        }

        [Fact(Timeout = 10000, DisplayName = "Prepending and appending icons on same component")]
        public async Task IconsOnBothSides()
        {
            var textBox = RenderComponent<SfTextBox>();
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            await textBox.Instance.AddIconAsync("prepend", "e-date-icon");
            var inputElement = textBox.Find("input");
            inputElement.Focus();
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            var iconElement1 = containerElement.Children[0];
            var iconElement2 = containerElement.Children[2];
            Assert.Contains("e-input-group-icon", iconElement1.ClassName);
            Assert.Contains("e-date-icon", iconElement1.ClassName);
            Assert.Contains("e-input-group-icon", iconElement2.ClassName);
            Assert.Contains("e-date-icon", iconElement2.ClassName);
            Assert.Equal(3, containerElement.ChildElementCount);
        }

        #endregion

        #region Icon with Clear Button Tests

        [Fact(Timeout = 10000, DisplayName = "Trail icon with clear button")]
        public async Task ClearButton()
        {
            var textBox = RenderComponent<SfTextBox>(("ShowClearButton", true));
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            var inputElement = textBox.Find("input");
            inputElement.Focus();
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            var clearIconElement = containerElement.Children[1];
            var iconElement = containerElement.Children[2];
            Assert.Contains("e-close", clearIconElement.ClassName);
            Assert.Contains("e-clear-icon-hide", clearIconElement.ClassName);
            Assert.Contains("e-input-group-icon", iconElement.ClassName);
            Assert.Contains("e-date-icon", iconElement.ClassName);
            Assert.Equal(3, containerElement.ChildElementCount);
            textBox.SetParametersAndRender(("Value", "Test"));
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            clearIconElement = containerElement.Children[1];
            iconElement = containerElement.Children[2];
            Assert.Equal("Test", textBox.Instance.Value);
            Assert.Equal("Test", inputElement.GetAttribute("value"));
            Assert.Contains("e-input-group-icon", iconElement.ClassName);
            Assert.Contains("e-date-icon", iconElement.ClassName);
            clearIconElement.MouseDown();
            Assert.Null(textBox.Instance.Value);
            Assert.Null(inputElement.GetAttribute("value"));
            Assert.Contains("e-input-group-icon", iconElement.ClassName);
            Assert.Contains("e-date-icon", iconElement.ClassName);
            Assert.Equal(3, containerElement.ChildElementCount);
        }

        [Fact(Timeout = 10000, DisplayName = "Multiple trail icon with clear button")]
        public async Task MultipleTrailIconWithClearButton()
        {
            //Appending the trail icon
            var textBox = RenderComponent<SfTextBox>(("ShowClearButton", true));
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            var inputElement = textBox.Find("input");
            inputElement.Focus();
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            var clearIconElement = containerElement.Children[1];
            var iconElement1 = containerElement.Children[2];
            var iconElement2 = containerElement.Children[3];
            var iconElement3 = containerElement.Children[4];
            var iconElement4 = containerElement.Children[5];
            var iconElement5 = containerElement.Children[6];
            Assert.Contains("e-close", clearIconElement.ClassName);
            Assert.Contains("e-clear-icon-hide", clearIconElement.ClassName);
            Assert.Contains("e-input-group-icon", iconElement1.ClassName);
            Assert.Contains("e-date-icon", iconElement1.ClassName);
            Assert.Contains("e-date-icon", iconElement2.ClassName);
            Assert.Contains("e-input-group-icon", iconElement2.ClassName);
            Assert.Contains("e-date-icon", iconElement3.ClassName);
            Assert.Contains("e-input-group-icon", iconElement3.ClassName);
            Assert.Contains("e-date-icon", iconElement4.ClassName);
            Assert.Contains("e-input-group-icon", iconElement4.ClassName);
            Assert.Contains("e-date-icon", iconElement5.ClassName);
            Assert.Contains("e-input-group-icon", iconElement5.ClassName);
            Assert.Equal(7, containerElement.ChildElementCount);
            textBox.SetParametersAndRender(("Value", "Test"));
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            clearIconElement = containerElement.Children[1];
            Assert.Contains("e-close", clearIconElement.ClassName);
            Assert.Equal("Test", textBox.Instance.Value);
            Assert.Equal("Test", inputElement.GetAttribute("value"));
            clearIconElement.MouseDown();
            await Task.Delay(200);
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            clearIconElement = containerElement.Children[1];
            Assert.Contains("e-close", clearIconElement.ClassName);
            Assert.Contains("e-clear-icon-hide", clearIconElement.ClassName);
            iconElement1 = containerElement.Children[2];
            iconElement2 = containerElement.Children[3];
            iconElement3 = containerElement.Children[4];
            iconElement4 = containerElement.Children[5];
            iconElement5 = containerElement.Children[6];
            Assert.Contains("e-input-group-icon", iconElement1.ClassName);
            Assert.Contains("e-date-icon", iconElement1.ClassName);
            Assert.Contains("e-date-icon", iconElement2.ClassName);
            Assert.Contains("e-input-group-icon", iconElement2.ClassName);
            Assert.Contains("e-date-icon", iconElement3.ClassName);
            Assert.Contains("e-input-group-icon", iconElement3.ClassName);
            Assert.Contains("e-date-icon", iconElement4.ClassName);
            Assert.Contains("e-input-group-icon", iconElement4.ClassName);
            Assert.Contains("e-date-icon", iconElement5.ClassName);
            Assert.Contains("e-input-group-icon", iconElement5.ClassName);
        }

        #endregion

        #region Dynamic Icon Tests

        [Fact(Timeout = 10000, DisplayName = "Dynamically enabling clear button with trail icon")]
        public async Task DynamicClearButton()
        {
            var textBox = RenderComponent<SfTextBox>();
            await textBox.Instance.AddIconAsync("append", "e-date-icon");
            var inputElement = textBox.Find("input");
            inputElement.Focus();
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            var iconElement = containerElement.Children[1];
            Assert.Contains("e-input-group-icon", iconElement.ClassName);
            Assert.Contains("e-date-icon", iconElement.ClassName);
            Assert.Equal(2, containerElement.ChildElementCount);
            textBox.SetParametersAndRender(parameter => parameter.Add(p => p.Value, "Test").Add(p => p.ShowClearButton, true));
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            var clearIconElement = containerElement.Children[1];
            iconElement = containerElement.Children[2];
            Assert.Equal("Test", textBox.Instance.Value);
            Assert.Equal("Test", inputElement.GetAttribute("value"));
            Assert.Contains("e-close", clearIconElement.ClassName);
            Assert.Contains("e-input-group-icon", iconElement.ClassName);
            Assert.Contains("e-date-icon", iconElement.ClassName);
            clearIconElement.MouseDown();
            await Task.Delay(200);
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            clearIconElement = containerElement.Children[1];
            Assert.Null(textBox.Instance.Value);
            Assert.Null(inputElement.GetAttribute("value"));
            Assert.Contains("e-input-group-icon", iconElement.ClassName);
            Assert.Contains("e-date-icon", iconElement.ClassName);
            Assert.Contains("e-close", clearIconElement.ClassName);
            Assert.Contains("e-clear-icon-hide", clearIconElement.ClassName);
            Assert.Equal(3, containerElement.ChildElementCount);
        }

        #endregion

        #region Icon with Floating Label Tests

        [Fact(Timeout = 10000, DisplayName = "Trail icon with floatlabel and clear button ")]
        public async Task IconWithFloatingLabelAndClearButton()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Always).Add(p => p.ShowClearButton, true));
            await Task.Delay(300);
            var button = RenderComponent<SfButton>((EventCallback(nameof(SfButton.OnClick), async (MouseEventArgs args) => {
                await textBox.Instance.AddIconAsync("append", "e-date-icon");
                await textBox.Instance.AddIconAsync("append", "e-date-icon");
                await textBox.Instance.AddIconAsync("prepend", "e-date-icon");
                await textBox.Instance.AddIconAsync("prepend", "e-date-icon");
            }))
            );
            var buttonElement = button.Find("button");
            buttonElement.Click();
            await Task.Delay(500);
            var inputElement = textBox.Find("input");
            inputElement.Focus();
            await Task.Delay(500);
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.NotNull(containerElement.ParentElement);
            var iconElement1 = containerElement.ParentElement.ChildNodes[0];
            var iconElement2 = containerElement.ParentElement.ChildNodes[1];
            var floatLineElement = containerElement.ChildNodes[1];
            var floatLabelElement = containerElement.ChildNodes[3];
            var clearIconElement = containerElement.ChildNodes[4];
            var iconElement3 = containerElement.ChildNodes[5];
            var iconElement4 = containerElement.ChildNodes[6];
            Assert.Contains("e-input-group-icon", iconElement1.ToMarkup());
            Assert.Contains("e-date-icon", iconElement1.ToMarkup());
            Assert.Contains("e-input-group-icon", iconElement2.ToMarkup());
            Assert.Contains("e-date-icon", iconElement2.ToMarkup());
            Assert.Contains("e-input-group-icon", iconElement3.ToMarkup());
            Assert.Contains("e-date-icon", iconElement3.ToMarkup());
            Assert.Contains("e-input-group-icon", iconElement4.ToMarkup());
            Assert.Contains("e-date-icon", iconElement4.ToMarkup());
            Assert.Contains("e-close", clearIconElement.ToMarkup());
            Assert.Contains("e-clear-icon-hide", clearIconElement.ToMarkup());
            Assert.Contains("e-label-top", floatLabelElement.ToMarkup());
            Assert.Equal(7, containerElement.ChildNodes.Count());
            await Task.CompletedTask;
        }

        [Fact(Timeout = 10000, DisplayName = "Multiple trail icons with floatlabel and clear button ")]
        public async Task MultipleIconWithFloatingLabelAndClearButton()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.Placeholder, "Enter the value").Add(p => p.FloatLabelType, FloatLabelType.Always).Add(p => p.ShowClearButton, true));
            var button = RenderComponent<SfButton>((EventCallback(nameof(SfButton.OnClick), async (MouseEventArgs args) => {
                await textBox.Instance.AddIconAsync("append", "e-date-icon");
                await textBox.Instance.AddIconAsync("append", "e-date-icon");
                await textBox.Instance.AddIconAsync("prepend", "e-date-icon");
                await textBox.Instance.AddIconAsync("prepend", "e-date-icon");
            }))
            );
            var buttonElement = button.Find("button");
            buttonElement.Click();
            await Task.Delay(200);
            var inputElement = textBox.Find("input");
            inputElement.Focus();
            var containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.NotNull(containerElement.ParentElement);
            var iconElement1 = containerElement.ParentElement.ChildNodes[0];
            var iconElement2 = containerElement.ParentElement.ChildNodes[1];
            var floatLineElement = containerElement.ChildNodes[1];
            var floatLabelElement = containerElement.ChildNodes[3];
            var clearIconElement = containerElement.ChildNodes[4];
            var iconElement3 = containerElement.ChildNodes[5];
            var iconElement4 = containerElement.ChildNodes[6];
            Assert.Contains("e-input-group-icon", iconElement1.ToMarkup());
            Assert.Contains("e-date-icon", iconElement1.ToMarkup());
            Assert.Contains("e-input-group-icon", iconElement2.ToMarkup());
            Assert.Contains("e-date-icon", iconElement2.ToMarkup());
            Assert.Contains("e-input-group-icon", iconElement3.ToMarkup());
            Assert.Contains("e-date-icon", iconElement3.ToMarkup());
            Assert.Contains("e-input-group-icon", iconElement4.ToMarkup());
            Assert.Contains("e-date-icon", iconElement4.ToMarkup());
            Assert.Contains("e-close", clearIconElement.ToMarkup());
            Assert.Contains("e-clear-icon-hide", clearIconElement.ToMarkup());
            Assert.Contains("e-label-top", floatLabelElement.ToMarkup());
            Assert.Equal(7, containerElement.ChildNodes.Count());
            await Task.CompletedTask;
        }

        #endregion
    }
}
