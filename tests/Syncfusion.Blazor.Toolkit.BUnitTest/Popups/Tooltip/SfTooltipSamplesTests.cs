using Bunit;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Buttons;
using Syncfusion.Blazor.Toolkit.Popups;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    /// <summary>
    /// BUnit tests covering each Tooltip sample component:
    ///   - OutsideTargetButton
    ///   - TemplateTooltip
    ///   - TooltipDelay
    ///   - TooltipEvent
    ///   - TooltipMethod
    /// </summary>
    public class SfTooltipSamplesTests : TooltipJsMock
    {
        // -------------------------------------------------------
        // OutsideTargetButton sample
        // -------------------------------------------------------

        [Fact(DisplayName = "OutsideTargetButton - Button renders without errors")]
        public void OutsideTargetButton_Renders_WithCorrectId()
        {
            // Arrange & Act
            var cut = RenderComponent<SfButton>(parameters => parameters
                .Add(p => p.Content, "Outside Target"));

            // Assert
            var button = cut.Find("button");
            Assert.NotNull(button);
        }

        [Fact(DisplayName = "OutsideTargetButton - Button renders with correct content text")]
        public void OutsideTargetButton_Renders_WithCorrectContent()
        {
            // Arrange & Act
            var cut = RenderComponent<SfButton>(parameters => parameters
                .Add(p => p.Content, "Outside Target"));

            // Assert
            Assert.Contains("Outside Target", cut.Markup);
        }

        // -------------------------------------------------------
        // TemplateTooltip sample
        // -------------------------------------------------------

        [Fact(DisplayName = "TemplateTooltip - Tooltip renders with CssClass e-tooltip-css")]
        public void TemplateTooltip_Renders_WithCssClass()
        {
            // Arrange & Act
            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.CssClass, "e-tooltip-css")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show")));

            // Assert
            Assert.Equal("e-tooltip-css", cut.Instance.CssClass);
        }

        [Fact(DisplayName = "TemplateTooltip - OpensOn parameter is set to Click")]
        public void TemplateTooltip_OpensOn_IsClick()
        {
            // Arrange & Act
            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.CssClass, "e-tooltip-css")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show")));

            // Assert
            Assert.Equal("Click", cut.Instance.OpensOn);
        }

        [Fact(DisplayName = "TemplateTooltip - Target is set to .TooltipTarget")]
        public void TemplateTooltip_Target_IsBtn()
        {
            // Arrange & Act
            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.CssClass, "e-tooltip-css")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show")));

            // Assert
            Assert.Equal(".TooltipTarget", cut.Instance.Target);
        }

        [Fact(DisplayName = "TemplateTooltip - ContentTemplate renders custom HTML content")]
        public async Task TemplateTooltip_ContentTemplate_RendersCustomHtml()
        {
            // Arrange & Act
            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.CssClass, "e-tooltip-css")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .Add<RenderFragment>(p => p.ContentTemplate, builder =>
                {
                    builder.OpenElement(0, "div");
                    builder.AddAttribute(1, "id", "democontent");
                    builder.AddAttribute(2, "class", "democontent");
                    builder.OpenElement(3, "h3");
                    builder.AddContent(4, "Eastern Bluebird");
                    builder.CloseElement();
                    builder.CloseElement();
                })
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show")));

            await cut.Instance.CreateTooltipAsync(true);

            // Assert — custom template content appears in markup
            Assert.Contains("Eastern Bluebird", cut.Markup);
            Assert.Contains("democontent", cut.Markup);
        }

        // -------------------------------------------------------
        // TooltipDelay sample
        // -------------------------------------------------------

        [Fact(DisplayName = "TooltipDelay - Initial Width is 200px before OnInitializedAsync")]
        public void TooltipDelay_InitialWidth_Is200px()
        {
            // Arrange & Act — render with initial parameter values (before async init completes)
            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Width, "200px")
                .Add(p => p.Content, "hi")
                .Add(p => p.ShowTipPointer, true)
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show Tooltip")));

            // Assert
            Assert.Equal("200px", cut.Instance.Width);
        }

        [Fact(DisplayName = "TooltipDelay - Initial Content is 'hi' before OnInitializedAsync")]
        public void TooltipDelay_InitialContent_IsHi()
        {
            // Arrange & Act
            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Width, "200px")
                .Add(p => p.Content, "hi")
                .Add(p => p.ShowTipPointer, true)
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show Tooltip")));

            // Assert
            Assert.Equal("hi", cut.Instance.Content);
        }

        [Fact(DisplayName = "TooltipDelay - Width updates to 300px after parameter change")]
        public void TooltipDelay_Width_UpdatesTo300px()
        {
            // Arrange
            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Width, "200px")
                .Add(p => p.Content, "hi")
                .Add(p => p.ShowTipPointer, true)
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show Tooltip")));

            // Act — simulate OnInitializedAsync result by updating parameters
            cut.SetParametersAndRender(parameters => parameters
                .Add(p => p.Width, "300px")
                .Add(p => p.Content, "hello"));

            // Assert
            Assert.Equal("300px", cut.Instance.Width);
        }

        [Fact(DisplayName = "TooltipDelay - Content updates to 'hello' after parameter change")]
        public void TooltipDelay_Content_UpdatesToHello()
        {
            // Arrange
            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Width, "200px")
                .Add(p => p.Content, "hi")
                .Add(p => p.ShowTipPointer, true)
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "Show Tooltip")));

            // Act — simulate OnInitializedAsync result by updating parameters
            cut.SetParametersAndRender(parameters => parameters
                .Add(p => p.Width, "300px")
                .Add(p => p.Content, "hello"));

            // Assert
            Assert.Equal("hello", cut.Instance.Content);
        }

        // -------------------------------------------------------
        // TooltipEvent sample
        // -------------------------------------------------------

        [Fact(DisplayName = "TooltipEvent - OnRender (BeforeRender) event fires with correct args")]
        public async Task TooltipEvent_OnRender_EventFires()
        {
            // Arrange
            TooltipEventArgs? capturedArgs = null;

            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Sample ToolTip")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .Add(p => p.OnRender, EventCallback.Factory.Create<TooltipEventArgs>(
                    this, args => capturedArgs = args))
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show Tooltip")));

            var eventArgs = new TooltipEventArgs
            {
                Cancel = false,
                IsInteracted = true,
                HasText = false,
                Left = 100,
                Top = 200
            };

            // Act
            await cut.Instance.TriggerBeforeRenderEventAsync(eventArgs);

            // Assert
            Assert.NotNull(capturedArgs);
            Assert.False(capturedArgs!.Cancel);
            Assert.True(capturedArgs.IsInteracted);
        }

        [Fact(DisplayName = "TooltipEvent - Colliding (BeforeCollision) event fires with correct args")]
        public async Task TooltipEvent_Colliding_EventFires()
        {
            // Arrange
            TooltipEventArgs? capturedArgs = null;

            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Sample ToolTip")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .Add(p => p.Colliding, EventCallback.Factory.Create<TooltipEventArgs>(
                    this, args => capturedArgs = args))
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show Tooltip")));

            var eventArgs = new TooltipEventArgs
            {
                Cancel = false,
                IsInteracted = true,
                CollidedPosition = "TopCenter",
                HasText = true
            };

            // Act
            await cut.Instance.TriggerBeforeCollisionEventAsync(eventArgs);

            // Assert
            Assert.NotNull(capturedArgs);
            Assert.Equal("TopCenter", capturedArgs!.CollidedPosition);
            Assert.True(capturedArgs.HasText);
        }

        [Fact(DisplayName = "TooltipEvent - OnOpen (BeforeOpen) event fires with correct args")]
        public async Task TooltipEvent_OnOpen_EventFires()
        {
            // Arrange
            TooltipEventArgs? capturedArgs = null;

            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Sample ToolTip")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .Add(p => p.OnOpen, EventCallback.Factory.Create<TooltipEventArgs>(
                    this, args => capturedArgs = args))
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show Tooltip")));

            var eventArgs = new TooltipEventArgs
            {
                Cancel = false,
                IsInteracted = true,
                HasText = true
            };

            // Act
            await cut.Instance.TriggerBeforeOpenEventAsync(eventArgs);

            // Assert
            Assert.NotNull(capturedArgs);
            Assert.False(capturedArgs!.Cancel);
            Assert.True(capturedArgs.IsInteracted);
        }

        [Fact(DisplayName = "TooltipEvent - OnClose (BeforeClose) event fires with correct args")]
        public async Task TooltipEvent_OnClose_EventFires()
        {
            // Arrange
            TooltipEventArgs? capturedArgs = null;

            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Sample ToolTip")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .Add(p => p.OnClose, EventCallback.Factory.Create<TooltipEventArgs>(
                    this, args => capturedArgs = args))
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show Tooltip")));

            var eventArgs = new TooltipEventArgs
            {
                Cancel = false,
                IsInteracted = false,
                HasText = false
            };

            // Act
            await cut.Instance.TriggerBeforeCloseEventAsync(eventArgs);

            // Assert
            Assert.NotNull(capturedArgs);
            Assert.False(capturedArgs!.Cancel);
            Assert.False(capturedArgs.IsInteracted);
        }

        [Fact(DisplayName = "TooltipEvent - Opened (AfterOpen) event fires")]
        public async Task TooltipEvent_Opened_EventFires()
        {
            // Arrange
            TooltipEventArgs? capturedArgs = null;

            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Sample ToolTip")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .Add(p => p.Opened, EventCallback.Factory.Create<TooltipEventArgs>(
                    this, args => capturedArgs = args))
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show Tooltip")));

            var eventArgs = new TooltipEventArgs
            {
                Cancel = false,
                IsInteracted = true,
                HasText = true
            };

            // Act — simulate JS invoking the after-open callback
            await cut.Instance.TriggerOpenedEventAsync(eventArgs);

            // Assert
            Assert.NotNull(capturedArgs);
            Assert.False(capturedArgs!.Cancel);
            Assert.True(capturedArgs.IsInteracted);
        }

        [Fact(DisplayName = "TooltipEvent - Closed (AfterClose) event fires")]
        public async Task TooltipEvent_Closed_EventFires()
        {
            // Arrange
            TooltipEventArgs? capturedArgs = null;

            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Sample ToolTip")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .Add(p => p.Closed, EventCallback.Factory.Create<TooltipEventArgs>(
                    this, args => capturedArgs = args))
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show Tooltip")));

            var eventArgs = new TooltipEventArgs
            {
                Cancel = false,
                IsInteracted = false,
                HasText = false
            };

            // Act — simulate JS invoking the after-close callback
            await cut.Instance.TriggerClosedEventAsync(eventArgs);

            // Assert
            Assert.NotNull(capturedArgs);
            Assert.False(capturedArgs!.Cancel);
            Assert.False(capturedArgs.IsInteracted);
        }

        // -------------------------------------------------------
        // TooltipMethod sample
        // -------------------------------------------------------

        [Fact(DisplayName = "TooltipMethod - Tooltip renders with Content parameter")]
        public void TooltipMethod_Renders_WithContent()
        {
            // Arrange & Act
            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Sample ToolTip")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show Tooltip")));

            // Assert
            Assert.Equal("Sample ToolTip", cut.Instance.Content);
        }

        [Fact(DisplayName = "TooltipMethod - IsSticky tooltip renders")]
        public void TooltipMethod_IsSticky_Renders()
        {
            // Arrange & Act
            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.ID, "tooltip1")
                .Add(p => p.IsSticky, true)
                .Add(p => p.Target, "#target")
                .Add(p => p.Content, "Sticky content")
                .AddChildContent(builder =>
                {
                    builder.OpenElement(0, "div");
                    builder.AddAttribute(1, "id", "container");
                    builder.OpenElement(2, "a");
                    builder.AddAttribute(3, "id", "target");
                    builder.AddContent(4, "environmentally friendly");
                    builder.CloseElement();
                    builder.CloseElement();
                }));

            // Assert
            Assert.True(cut.Instance.IsSticky);
            Assert.Equal("tooltip1", cut.Instance.ID);
        }

        [Fact(DisplayName = "TooltipMethod - Container parameter is set correctly")]
        public void TooltipMethod_Container_IsSetCorrectly()
        {
            // Arrange & Act
            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Sample ToolTip")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .Add(p => p.Container, ".container1")
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show Tooltip")));

            // Assert
            Assert.Equal(".container1", cut.Instance.Container);
        }

        [Fact(DisplayName = "TooltipMethod - Container parameter updates on re-render")]
        public void TooltipMethod_Container_UpdatesOnRerender()
        {
            // Arrange
            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Sample ToolTip")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .Add(p => p.Container, ".container1")
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show Tooltip")));

            // Act — simulate ChangeContainer()
            cut.SetParametersAndRender(parameters => parameters
                .Add(p => p.Container, ".container2"));

            // Assert
            Assert.Equal(".container2", cut.Instance.Container);
        }

        [Fact(DisplayName = "TooltipMethod - RefreshPositionAsync can be invoked without error")]
        public async Task TooltipMethod_RefreshPositionAsync_InvokesWithoutError()
        {
            // Arrange
            JSInterop.Setup<string>("refreshPosition", _ => true).SetResult(string.Empty);

            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Sample ToolTip")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show Tooltip")));

            // Act & Assert — should not throw
            var exception = await Record.ExceptionAsync(() => cut.Instance.RefreshPositionAsync());
            Assert.Null(exception);
        }

        [Fact(DisplayName = "TooltipMethod - RefreshAsync can be invoked without error")]
        public async Task TooltipMethod_RefreshAsync_InvokesWithoutError()
        {
            // Arrange
            JSInterop.Setup<string>("refresh", _ => true).SetResult(string.Empty);

            var cut = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Content, "Sample ToolTip")
                .Add(p => p.Target, ".TooltipTarget")
                .Add(p => p.OpensOn, "Click")
                .AddChildContent<SfButton>(b => b.Add(p => p.CssClass, "TooltipTarget").Add(p => p.Content, "Show Tooltip")));

            // Act & Assert — should not throw
            var exception = await Record.ExceptionAsync(() => cut.Instance.RefreshAsync());
            Assert.Null(exception);
        }
    }
}
