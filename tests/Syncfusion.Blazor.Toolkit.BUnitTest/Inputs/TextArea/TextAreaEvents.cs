using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.TextArea
{
    public class TextAreaEvents : BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "Created Event Trigger")]
        public void CreatedEvent()
        {
            var count = 0;
            var textArea = RenderComponent<SfTextArea>();
            textArea.SetParametersAndRender((events => events.Add(e => e.Created, (object args) =>
            {
                count++;
                Assert.NotNull("Create event is triggered, when render the component");
                Assert.Equal(1, count);
            }))
            );
        }

        [Fact(Timeout = 10000, DisplayName = "ValueChange Event Trigger")]
        public async Task ChangeEventArgs()
        {
            bool isDynamicChange = false;
            bool isClearChange = false;
            var count = 0;
            var textArea = RenderComponent<SfTextArea>();
            textArea.SetParametersAndRender((events => events.Add(e=>e.ValueChange, (TextAreaValueChangeEventArgs args) =>
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
            var containerEle = textArea.Find("textarea").ParentElement;
            isDynamicChange = true;
            textArea.SetParametersAndRender(("Value", "Test"));

            Assert.Equal(1, count);
            Assert.Equal("Test", textArea.Instance.Value);
            var textAreaElement = textArea.Find("textarea");
            Assert.Equal("Test", textAreaElement.GetAttribute("value"));

            textArea.SetParametersAndRender(("ShowClearButton", true));
            containerEle = textArea.Find("textarea").ParentElement;
            Assert.NotNull(containerEle);
            Assert.True(containerEle.Children.Length > 1);
            var clearEle = containerEle.Children[1];
            textAreaElement = textArea.Find("textarea");
            Assert.Contains("e-close", clearEle.ClassName);
            isClearChange = true;

            clearEle.MouseDown();
            await Task.Delay(300);
            Assert.Equal(2, count);
        }

        [Fact(Timeout = 10000, DisplayName = "OnChange Event Trigger")]
        public async Task InputChangeHandler()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.ID, "Sample-TextID").Add(p => p.FloatLabelType, FloatLabelType.Always).Add(p => p.EnablePersistence, true));
            var textAreaElement = textArea.Find("textarea");
            textArea.SetParametersAndRender((events => events.Add(e => e.OnChange, async (ChangeEventArgs args) =>
            {
                Assert.Equal("Syncfusion TextArea", args.Value);
            })));
            textAreaElement.Change("Syncfusion TextArea");
        }

        [Fact(Timeout = 10000, DisplayName = "OnInput Event Trigger")]
        public async Task InputHandler()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.ID, "Sample-TextID").Add(p => p.FloatLabelType, FloatLabelType.Always));
            textArea.SetParametersAndRender((events => events.Add(e => e.OnInput, async (TextAreaInputEventArgs args) =>
            {
                Assert.Null(args.PreviousValue);
                Assert.Equal("Syncfusion", args.Value);
            })));
            var textAreaElement = textArea.Find("textarea");
            textAreaElement.Change("Syncfusion");
        }

        [Fact(Timeout = 10000, DisplayName = "Clear button clears value")]
        public async Task InvokeClearBtnEventHandler()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.ID, "Sample-TextID").Add(p => p.Value, "InvokeClearBtnEvent").Add(p => p.ShowClearButton, true));
            textArea.SetParametersAndRender((events => events.Add(e => e.OnInput, async (TextAreaInputEventArgs args) =>
            {
                Assert.Null(args.Value);
            })));
            Assert.Equal("InvokeClearBtnEvent", textArea.Instance.Value);
            var ClearBtn = textArea.Find(".e-close");
            Assert.NotNull(ClearBtn);
            ClearBtn.TouchStart();
        }   

        [Fact(Timeout = 10000, DisplayName = "Destroyed fires on dispose")]
        public async Task DestroyedHandler()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.ID, "Sample-TextID").Add(p => p.FloatLabelType, FloatLabelType.Always));
            textArea.SetParametersAndRender((events => events.Add(e => e.Destroyed, async (Object args) =>
            {
                Assert.Null(args);
            })));
            textArea.Dispose();
        }

        [Fact(Timeout = 10000, DisplayName = "Focus and blur toggle focus class")]
        public async Task FocusAndBlur()
        {
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.ID, "Sample-TextID").Add(p => p.Value, "Syncfusion").Add(p => p.FloatLabelType, FloatLabelType.Always));
            textArea.SetParametersAndRender((events => events.Add(e => e.OnFocus, async (TextAreaFocusInEventArgs args) =>
            {
                Assert.Equal("Syncfusion", args.Value);
            })));
            await textArea.Instance.FocusAsync();
            var textAreaElement = textArea.Find("textarea");
            textAreaElement.Focus();

            var containerEle = textAreaElement.ParentElement;
            Assert.NotNull(containerEle);
            Assert.Contains("e-input-focus", containerEle.ClassName);

            textArea.SetParametersAndRender((events => events.Add(e => e.OnBlur, async (TextAreaFocusOutEventArgs args) =>
            {
                Assert.Equal("Syncfusion", args.Value);
            })));
            await textArea.Instance.FocusOutAsync();
            textAreaElement = textArea.Find("textarea");

            textAreaElement.Blur();
            containerEle = textAreaElement.ParentElement;
            Assert.NotNull(containerEle);
            Assert.DoesNotContain("e-input-focus", containerEle.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Get persist data returns null")]
        public async Task GetPersistData()
        {
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>() { { "name", "textarea" } };
            Dictionary<string, object> inputAttributes = new Dictionary<string, object>() { { "Value", "Forms Component" } };
            var textArea = RenderComponent<SfTextArea>(parameter => parameter.Add(p => p.ID, "Multiline-ID").Add(p => p.HtmlAttributes, htmlAttributes).Add(p => p.TabIndex, 0));
            var textAreaEle = textArea.Find("textarea");
            var PersistData = await textArea.Instance.GetPersistDataAsync();
            Assert.Null(PersistData);
            Dictionary<string, object>? htmlAttr = textArea.Instance.HtmlAttributes;
            Assert.NotNull(htmlAttr);
            Assert.Contains("textarea", htmlAttr["name"].ToString());
            Assert.Equal(0, textArea.Instance.TabIndex);
        }
    }
}
