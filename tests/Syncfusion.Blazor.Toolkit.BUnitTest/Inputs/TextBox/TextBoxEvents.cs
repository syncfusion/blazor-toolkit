using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.TextBox
{
    public class TextBoxEvents : BunitTestContext
    {
        #region Lifecycle Events

        [Fact(Timeout = 10000)]
        public void CreatedEvent()
        {
            var count = 0;
            var textBox = RenderComponent<SfTextBox>();
            textBox.SetParametersAndRender((events => events.Add(e => e.Created, (object args) =>
            {
                count++;
                Assert.NotNull("Create event is triggered, when render the component");
                Assert.Equal(1, count);
            }))
            );
        }

        [Fact(Timeout = 10000)]
        public async Task DestroyedHandler()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.ID, "Sample-TextID").Add(p => p.FloatLabelType, FloatLabelType.Always));
            textBox.SetParametersAndRender((events => events.Add(e => e.Destroyed, async (Object args) =>
            {
                Assert.Null(args);
            })));
            textBox = null;
        }

        #endregion

        #region Focus and Blur Events

        [Fact(Timeout = 10000)]
        public async Task FocusAndBlurMethod()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.ID, "Sample-TextID").Add(p => p.Value, "Syncfusion").Add(p => p.FloatLabelType, FloatLabelType.Always));
            textBox.SetParametersAndRender((events => events.Add(e => e.OnFocus, async (FocusInEventArgs args) =>
            {
                Assert.Equal("Microsoft.AspNetCore.Components.Web.FocusEventArgs", args.Event?.ToString());
                Assert.Equal("Syncfusion", args.Value);
            })));
            await textBox.Instance.AddIconAsync("prepend", "e-input-reload");
            await textBox.Instance.FocusAsync();
            var inputElement = textBox.Find("input");
            inputElement.Focus();
            Assert.NotNull(inputElement.ParentElement);
            var containerElement = inputElement.ParentElement.ParentElement;
            Assert.NotNull(containerElement);
            Assert.Contains("e-input-focus", containerElement.ClassName);
            await textBox.Instance.FocusOutAsync();
            inputElement.Blur();
        }

        [Fact(Timeout = 10000)]
        public async Task FocusEventsAreInvoked()
        {
            int focusCalled = 0;
            int blurCalled = 0;
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.OnFocus, EventCallback.Factory.Create<FocusInEventArgs>(this, (e) => focusCalled++)).Add(p => p.OnBlur, EventCallback.Factory.Create<FocusOutEventArgs>(this, (e) => blurCalled++)));
            var inputElement = textBox.Find("input");
            await inputElement.TriggerEventAsync("onfocus", new Microsoft.AspNetCore.Components.Web.FocusEventArgs());
            Assert.Equal(1, focusCalled);
        }

        #endregion

        #region Value Change Events

        [Fact(Timeout = 10000)]
        public async Task ChangeEventArgs()
        {
            bool isDynamicChange = false;
            bool isClearChange = false;
            var count = 0;
            var textBox = RenderComponent<SfTextBox>();
            textBox.SetParametersAndRender((events => events.Add(e => e.ValueChange, (ChangedEventArgs args) =>
            {
                if (isDynamicChange)
                {
                    count++;
                    Assert.Equal("Test", args.Value);
                    Assert.False(args.IsInteracted);
                    Assert.Null(args.PreviousValue);
                    isDynamicChange = false;
                }
                else if (isClearChange)
                {
                    count++;
                    Assert.Null(args.Value);
                    Assert.True(args.IsInteracted);
                    Assert.NotNull(args.PreviousValue);
                    Assert.Null(args.Value);
                    isClearChange = false;
                }
            })
            ));
            var inputElement = textBox.Find("input");
            var containerElement = inputElement.ParentElement;
            isDynamicChange = true;
            textBox.SetParametersAndRender(("Value", "Test"));
            Assert.Equal(1, count);
            Assert.Equal("Test", textBox.Instance.Value);
            Assert.Equal("Test", inputElement.GetAttribute("value"));
            textBox.SetParametersAndRender(("ShowClearButton", true));
            containerElement = inputElement.ParentElement;
            Assert.NotNull(containerElement);
            var clearIconElement = containerElement.Children[1];
            Assert.Contains("e-clear-icon", clearIconElement.ClassName);
            isClearChange = true;
            clearIconElement.MouseDown();
            await Task.Delay(300);
            Assert.Equal(2, count);
        }

        #endregion

        #region Input Events

        [Fact(Timeout = 10000)]
        public async Task InputChangeHandler()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.ID, "Sample-TextID").Add(p => p.FloatLabelType, FloatLabelType.Always).Add(p => p.EnablePersistence, true));
            var inputElement = textBox.Find("input");
            textBox.SetParametersAndRender((events => events.Add(e => e.OnChange, async (ChangeEventArgs args) =>
            {
                Assert.Equal("Syncfusion TextBox", args.Value);
            })));
            inputElement.Change("Syncfusion TextBox");
        }

        [Fact(Timeout = 10000)]
        public async Task InputHandler()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.ID, "Sample-TextID").Add(p => p.FloatLabelType, FloatLabelType.Always));
            textBox.SetParametersAndRender((events => events.Add(e => e.OnInput, async (InputEventArgs args) =>
            {
                Assert.NotNull(args.Event);
                Assert.Null(args.PreviousValue);
                Assert.Equal("Syncfusion", args.Value);
            })));
            var inputElement = textBox.Find("input");
            inputElement.Change("Syncfusion");
        }

        [Fact(Timeout = 10000)]
        public async Task InputEventReceivesArgsOnOnInput()
        {
            int called = 0;
            string? value = null;
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.OnInput, EventCallback.Factory.Create<InputEventArgs>(this, (InputEventArgs args) => { called++; value = args?.Value; })));
            var inputElement = textBox.Find("input");
            await inputElement.TriggerEventAsync("oninput", new ChangeEventArgs { Value = "typed" });
            Assert.Equal(1, called);
            Assert.Equal("typed", value);
        }

        #endregion

        #region Clear Button Events

        [Fact(Timeout = 10000)]
        public async Task InvokeClearButtonEventHandler()
        {
            var textBox = RenderComponent<SfTextBox>(parameter => parameter.Add(p => p.ID, "Sample-TextID").Add(p => p.Value, "InvokeClearBtnEvent").Add(p => p.ShowClearButton, true));
            textBox.SetParametersAndRender((events => events.Add(e => e.OnInput, async (InputEventArgs args) =>
            {
                Assert.Null(args.Value);
            })));
            Assert.Equal("InvokeClearBtnEvent", textBox.Instance.Value);
            var clearIconElement = textBox.Find(".e-close");
            clearIconElement.TouchStart();
        }

        #endregion
    }
}
