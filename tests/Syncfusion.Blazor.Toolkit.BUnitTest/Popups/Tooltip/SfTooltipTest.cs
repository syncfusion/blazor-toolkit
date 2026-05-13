using Bunit;
using Xunit;
using Syncfusion.Blazor.Toolkit.Popups;
using Syncfusion.Blazor.Toolkit.Buttons;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Tests.Tooltip
{
    public partial class SfTooltipTest : TooltipJsMock
    {
        [Fact(Timeout = 10000, DisplayName = "Tooltip - with dynamic Target property - Task ID: BLAZ-17016")]
        public void DynamicTarget()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                   parameters.Add(p => p.Target, "#btn").Add(p => p.Content, "Check"));
            Assert.Contains("#btn", tooltip.Instance.Target);
            Assert.Contains("Check", tooltip.Instance.Content);
            tooltip.SetParametersAndRender(("Target", "#btn1"), ("Content", "Hello"));
            Assert.Contains("#btn1", tooltip.Instance.Target);
            Assert.Contains("Hello", tooltip.Instance.Content);
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Initial DOM Rendering")]
        public void ComponentRendering()
        {
            var tooltip = RenderComponent<SfTooltip>();
            var rootEle = tooltip.Find(".e-tooltip");
            Assert.Contains("e-control", rootEle.ClassName);
            Assert.Contains("e-lib", rootEle.ClassName);
        }
        
        [Fact(Timeout = 10000, DisplayName = "Tooltip - Properties - CssClass value testing")]
        public async Task CssClass()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                    parameters.Add(p => p.Target, ".TooltipTarget").Add(p => p.CssClass, "Custom").Add(p => p.Content, "Check").AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip").Add(p => p.CssClass, "TooltipTarget")));
            var tooltipEle = tooltip.Find(".e-tooltip");
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.Contains("Custom", tooltip.Find(".e-tooltip-wrap").ClassList);
                });
            tooltip.SetParametersAndRender(("CssClass", "dynamicClass"));
            await tooltip.InvokeAsync(async () =>
            {
                var buttonElem = tooltip.Find("button");
                buttonElem.Click();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.Contains("dynamicClass", tooltip.Find(".e-tooltip-wrap").ClassList);
                });
        }
		
        [Fact(Timeout = 10000, DisplayName = "Tooltip - Properties - ShowTipPointer testing")]
        public async Task ShowTipPointeProperty()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                    parameters.Add(p => p.Target, ".TooltipTarget").Add(p => p.ShowTipPointer, false).Add(p => p.Content, "Check").AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip").Add(p => p.CssClass, "TooltipTarget")));
            var rootEle = tooltip.Find(".e-tooltip");
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
              async (t) =>
              {
                  Assert.True(tooltip.Find(".e-tooltip-wrap").QuerySelector(".e-arrow-tip") == null);
                  Assert.True(!tooltip.Instance.ShowTipPointer);
              });
            tooltip.SetParametersAndRender(("ShowTipPointer", true));
            await tooltip.InvokeAsync(async () =>
            {
                var buttonElem = tooltip.Find("button");
                buttonElem.Click();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.True(tooltip.Find(".e-tooltip-wrap").QuerySelector(".e-arrow-tip") != null);
                    Assert.True(tooltip.Instance.ShowTipPointer);
                });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Properties - Height value testing")]
        public async Task TooltipHeight()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                   parameters.Add(p => p.Height, "40").Add(p => p.Target, ".TooltipTarget").Add(p => p.Content, "Check").AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip").Add(p => p.CssClass, "TooltipTarget")));
            var rootEle = tooltip.Find(".e-tooltip");
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
              async (t) =>
              {
                  Assert.Contains("40", tooltip.Instance.Height);
                  Assert.Contains("height: 40px", tooltip.Find(".e-tooltip-wrap").GetAttribute("style"));
              });
            tooltip.SetParametersAndRender(("Height", "1000px"));
            await tooltip.InvokeAsync(async () =>
            {
                var buttonElem = tooltip.Find("button");
                buttonElem.Click();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.Contains("1000", tooltip.Instance.Height);
                    Assert.Contains("height: 1000px", tooltip.Find(".e-tooltip-wrap").GetAttribute("style"));
                });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Properties - Width value testing")]
        public async Task TooltipWidth()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                   parameters.Add(p => p.Width, "200px").Add(p => p.Target, ".TooltipTarget").Add(p => p.Content, "Check").AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip").Add(p => p.CssClass, "TooltipTarget")));
            var rootEle = tooltip.Find(".e-tooltip");
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
              async (t) =>
              {
                  Assert.Contains("200px", tooltip.Instance.Width);
                  Assert.Contains("width: 200px", tooltip.Find(".e-tooltip-wrap").GetAttribute("style"));
              });
            string styleValue = rootEle.GetAttribute("style");
            tooltip.SetParametersAndRender(("Width", "1000px"));
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
              async (t) =>
              {
                  styleValue = rootEle.GetAttribute("style");
                  Assert.True(styleValue.Contains("width:1000px"), "Width is working fine in dynamic case");
              });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Properties - OpenDelay testing")]
        public async Task OpenDelay()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                   parameters.Add(p => p.OpenDelay, 3000).Add(p => p.OpensOn, "Click").Add(p => p.Target, ".TooltipTarget").Add(p => p.Content, "Check").AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip").Add(p => p.CssClass, "TooltipTarget")));
            var rootEle = tooltip.Find(".e-tooltip");
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
                await Task.Delay(3000);
            }).ContinueWith(
              async (t) =>
              {
                  Assert.Contains("e-popup-open", tooltip.Find(".e-tooltip-wrap").ClassList);
              });
            tooltip.SetParametersAndRender(("OpenDelay", 0.0));
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
              async (t) =>
              {
                  Assert.True(tooltip.Instance.OpenDelay == 0);
                  Assert.Contains("e-popup-open", tooltip.Find(".e-tooltip-wrap").ClassList);
              });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Properties - CloseDelay testing")]
        public async Task CloseDelay()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                   parameters.Add(p => p.OpenDelay, 3000).Add(p => p.CloseDelay, 3000).Add(p => p.Target, ".TooltipTarget").Add(p => p.Content, "Check").AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip").Add(p => p.CssClass, "TooltipTarget")));
            var rootEle = tooltip.Find(".e-tooltip");
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
                await Task.Delay(3000);
            }).ContinueWith(
              async (t) =>
              {
                  Assert.Contains("e-popup-open", tooltip.Find(".e-tooltip-wrap").ClassList);
              });
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.CloseAsync();
                await Task.Delay(3000);
            }).ContinueWith(
             async (t) =>
             {
                 Assert.True(tooltip.Find(".e-tooltip-wrap") == null);
             });
            tooltip.SetParametersAndRender(("CloseDelay", 0.0));
            await tooltip.Instance.OpenAsync();
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.CloseAsync();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.True(tooltip.Instance.CloseDelay == 0);
                });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Properties - IsSticky testing")]
        public async Task TooltipIsSticky()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                   parameters.Add(p => p.IsSticky, true).Add(p => p.Target, ".TooltipTarget").Add(p => p.Content, "Check").Add(p => p.Animation, new AnimationModel() { Open = new TooltipAnimationSettings { Effect = Effect.ZoomIn }, Close = new TooltipAnimationSettings { Effect = Effect.ZoomOut } }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip").Add(p => p.CssClass, "TooltipTarget")));
            var rootEle = tooltip.Find(".e-tooltip");
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
              async (t) =>
              {
                  Assert.True(tooltip.Find(".e-tooltip-wrap").QuerySelector(".e-tooltip-close") != null);
              });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - sticky close testing")]
        public async Task TooltipIsStickyClose()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                   parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(p => p.IsSticky, true).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            var rootEle = tooltip.Find(".e-tooltip");
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
              async (t) =>
              {
                  Assert.True(tooltip.Find(".e-tooltip-wrap").QuerySelector(".e-tooltip-close") != null);
                  tooltip.Find(".e-tooltip-wrap").QuerySelector(".e-tooltip-close").Click();
                  Assert.True(tooltip.Find(".e-tooltip-wrap") == null);
              });
            tooltip.SetParametersAndRender(("IsSticky", false));
            await tooltip.InvokeAsync(async () =>
            {
                var buttonElem = tooltip.Find("button");
                buttonElem.Click();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.True(tooltip.Find(".e-tooltip-wrap").QuerySelector(".e-tooltip-close") == null);
                });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Properties testing - WindowCollision, TipPointerPosition")]
        public void WindowCollision()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                   parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(p => p.WindowCollision, true).Add(p => p.TipPointerPosition, TipPointerPosition.Start).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            Assert.Contains(".e-btn", tooltip.Instance.Target);
            Assert.Contains("Check", tooltip.Instance.Content);
            Assert.True(tooltip.Instance.WindowCollision == true);
            Assert.True(tooltip.Instance.ShowTipPointer == true);
            Assert.True(tooltip.Instance.TipPointerPosition == TipPointerPosition.Start);
            tooltip.SetParametersAndRender(("WindowCollision", false), ("TipPointerPosition", TipPointerPosition.End));
            Assert.True(!tooltip.Instance.WindowCollision);
            Assert.True(tooltip.Instance.TipPointerPosition == TipPointerPosition.End);
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Properties - offset value testing")]
        public async Task Offset()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                   parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(p => p.OffsetX, 30).Add(p => p.OffsetY, 30).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            Assert.Contains(".e-btn", tooltip.Instance.Target);
            Assert.Contains("Check", tooltip.Instance.Content);
            Assert.True(tooltip.Instance.OffsetX == 30);
            Assert.True(tooltip.Instance.OffsetY == 30);
            tooltip.SetParametersAndRender(("OffsetX", 3000.0), ("OffsetY", 3000.0));
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.True(tooltip.Instance.OffsetX == 300);
                    Assert.True(tooltip.Instance.OffsetY == 300);
                });
        }


        [Fact(Timeout = 10000, DisplayName = "Tooltip - Property testing - OpensOn")]
        public async Task OpensOn()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.OpensOn, "Hover").Add(p => p.Target, ".TooltipTarget").Add(p => p.Content, "Check").AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip").Add(p => p.CssClass, "TooltipTarget")));
            var rootEle = tooltip.Find(".e-tooltip");
            Assert.Contains("Hover", tooltip.Instance.OpensOn);
            tooltip.SetParametersAndRender(("OpensOn", "Click"));
            Assert.Contains("Click", tooltip.Instance.OpensOn);
            await tooltip.InvokeAsync(async () =>
            {
                var buttonElem = tooltip.Find("button");
                buttonElem.Click();
            }).ContinueWith(
               async (t) =>
               {
                   Assert.Contains("e-popup-open", tooltip.Find(".e-tooltip-wrap").ClassList);
               });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Property testing - Position")]
        public async Task TooltipPosition()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                   parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(p => p.Position, Position.BottomLeft).Add(s => s.Colliding, (e) =>
                   {
                       Assert.Equal("TopLeft", e.CollidedPosition);
                   }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            tooltip.SetParametersAndRender(("Position", Position.TopLeft));
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
                await tooltip.Instance.RefreshAsync();
            });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - with dynamic animation")]
        public void DynamicAnimation()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                   parameters.Add(p => p.Animation, new AnimationModel() { Open = new TooltipAnimationSettings { Effect = Effect.ZoomIn }, Close = new TooltipAnimationSettings { Effect = Effect.ZoomOut } }));
            tooltip.SetParametersAndRender(("Animation", new AnimationModel() { Open = new TooltipAnimationSettings { Effect = Effect.FadeIn }, Close = new TooltipAnimationSettings { Effect = Effect.FadeOut } }));
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Events - OnRender Event testing")]
        public async Task OnRenderEvent()
        {
            var onbeforerendercount = 0;
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.OnRender, () =>
                 {
                     onbeforerendercount++;
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            Assert.Equal(0, onbeforerendercount);
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.Equal(1, onbeforerendercount);
                });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Events - Created Event testing")]
        public async Task Created()
        {
            var createdeventcount = 0;
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.Created, () =>
                 {
                     createdeventcount++;
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            Assert.Equal(1, createdeventcount);
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Events - Collision Event testing")]
        public async Task Collision()
        {
            var collisioneventcount = 0;
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(s => s.Position, Position.RightBottom).Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.Colliding, (e) =>
                 {
                     Assert.Equal("RightBottom", e.CollidedPosition);
                     collisioneventcount++;
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            Assert.Equal(0, collisioneventcount);
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.Equal(1, collisioneventcount);
                });
        }
        [Fact(Timeout = 10000, DisplayName = "Tooltip - Events - BeforeOpen Event testing")]
        public async Task BeforeOpen()
        {
            var onbeforeopencount = 0;
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.OnOpen, () =>
                 {
                     onbeforeopencount++;
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            Assert.Equal(0, onbeforeopencount);
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.Equal(1, onbeforeopencount);
                });
        }
        [Fact(Timeout = 10000, DisplayName = "Tooltip - Events - AfterOpen Event testing")]
        public async Task Open()
        {
            var onafteropencount = 0;
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.Opened, () =>
                 {
                     onafteropencount++;
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            Assert.Equal(0, onafteropencount);
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.Equal(1, onafteropencount);
                });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Events - BeforeClose Event testing")]
        public async Task Close()
        {
            var onbeforeclose = 0;
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.OnClose, () =>
                 {
                     onbeforeclose++;
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip"))); ;
            Assert.Equal(0, onbeforeclose);
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.Equal(0, onbeforeclose);
                });
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.CloseAsync();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.Equal(1, onbeforeclose);
                });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Events - Closed Event testing")]
        public async Task Closed()
        {
            var onafterclose = 0;
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.Closed, () =>
                 {
                     onafterclose++;
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            Assert.Equal(0, onafterclose);
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.OpenAsync();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.Equal(0, onafterclose);
                });
            await tooltip.InvokeAsync(async () =>
            {
                await tooltip.Instance.CloseAsync();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.Equal(1, onafterclose);
                });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Events - Destroy Event testing")]
        public async Task DestroyedEvent()
        {
            var destroyedcount = 0;
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                            parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.Destroyed, () =>
                            {
                                destroyedcount++;
                            }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            Assert.Equal(0, destroyedcount);
            await tooltip.InvokeAsync(async () =>
            {
                tooltip.Dispose();
            }).ContinueWith(
                async (t) =>
                {
                    Assert.Equal(1, destroyedcount);
                });
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - TooltipEventArgs testing")]
        public async Task TooltipEventArgs()
        {
            TooltipEventArgs eventArgs = new TooltipEventArgs()
            {
                Cancel = false,
                CollidedPosition = "TopLeft",
                Event = new EventArgs() { },
                HasText = true,
                Top = 10,
                Left = 10,
                IsInteracted = true,
                Type = null,
            };
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                     parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.OnOpen, (TooltipEventArgs args) =>
                     {
                         Assert.False(args.Cancel);
                         Assert.True(args.HasText);
                         Assert.Equal(10, args.Top);
                         Assert.Equal(10, args.Left);
                         Assert.True(args.IsInteracted);
                         Assert.Equal("TopLeft", args.CollidedPosition);
                         Assert.Null(args.Type);
                         Assert.NotNull(args.Event);
                         args.Equals(eventArgs);
                         var hsdhCode = args.GetHashCode();
                     }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            await tooltip.Instance.TriggerBeforeOpenEventAsync(eventArgs);
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - TooltipTemplates testing")]
        public void TooltipTemplates()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters => parameters
                .Add(p => p.Target, ".e-btn")
                .Add(p => p.ContentTemplate, (RenderFragment)(b => b.AddContent(0, "tooltip shows")))
                .AddChildContent<SfButton>(b => b.Add(p => p.Content, "ShowTooltip"))
            );

            Assert.NotNull(tooltip.Instance.ContentTemplate);
            Assert.NotNull(tooltip.Instance.ChildContent);
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - OnClose Event testing")]
        public async Task OnClose()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.OnClose, (TooltipEventArgs args) =>
                 {
                     Assert.NotNull(args);
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            await tooltip.Instance.TriggerBeforeCloseEventAsync(new TooltipEventArgs());
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - TriggerClosed Event testing")]
        public async Task TriggerClosed()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.Closed, (TooltipEventArgs args) =>
                 {
                     Assert.NotNull(args);
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            await tooltip.Instance.TriggerClosedEventAsync(new TooltipEventArgs());
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - OnRender Event testing")]
        public async Task OnRender()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.OnRender, (TooltipEventArgs args) =>
                 {
                     Assert.NotNull(args);
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            await tooltip.Instance.TriggerBeforeRenderEventAsync(new TooltipEventArgs());
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Colliding Event testing")]
        public async Task Colliding()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.Colliding, (TooltipEventArgs args) =>
                 {
                     Assert.NotNull(args);
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            await tooltip.Instance.TriggerBeforeCollisionEventAsync(new TooltipEventArgs());
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - OnOpen Event testing")]
        public async Task OnOpen()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.OnOpen, (TooltipEventArgs args) =>
                 {
                     Assert.NotNull(args);
                    // Assert.NotNull(args.JsRuntime);
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            await tooltip.Instance.TriggerBeforeOpenEventAsync(new TooltipEventArgs());
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Opened Event testing")]
        public async Task Opened()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.Opened, (TooltipEventArgs args) =>
                 {
                     Assert.NotNull(args);
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            await tooltip.Instance.TriggerOpenedEventAsync(new TooltipEventArgs());
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - Methods testing")]
        public async Task Methods()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            await tooltip.Instance.CreateTooltipAsync(true);
            await tooltip.Instance.OpenAsync();
            await tooltip.Instance.CloseAsync();
            await tooltip.Instance.RefreshAsync();
            await tooltip.Instance.RefreshPositionAsync();

        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - PreventRender testing")]
        public async Task PreventRender()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                 parameters.Add(p => p.Target, ".e-btn").Add(p => p.Content, "Check").Add(s => s.OnRender, (TooltipEventArgs args) =>
                 {
                     Assert.NotNull(args);
                 }).AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip")));
            tooltip.Instance.PreventRender();
            await tooltip.Instance.TriggerBeforeRenderEventAsync(new TooltipEventArgs());
        }

        [Fact(Timeout = 10000, DisplayName = "Tooltip - DynamicProperty testing")]
        public async Task DynamicProperty()
        {
            var tooltip = RenderComponent<SfTooltip>(parameters =>
                    parameters.Add(p => p.Target, ".TooltipTarget").Add(p => p.Container, ".Custom").Add(p => p.MouseTrail, true).Add(p => p.Content, "Check").AddChildContent<SfButton>(field => field.Add(p => p.Content, "ShowTooltip").Add(p => p.CssClass, "Custom")));
            var tooltipEle = tooltip.Find(".e-tooltip");
            Assert.True(tooltip.Instance.Container == ".Custom");
            tooltip.SetParametersAndRender(("Container", ".dynamicClass"), ("MouseTrail", false), ("Animation", null));
            Assert.True(tooltip.Instance.Container == ".dynamicClass");
            Assert.False(tooltip.Instance.MouseTrail);
        }
    }
}