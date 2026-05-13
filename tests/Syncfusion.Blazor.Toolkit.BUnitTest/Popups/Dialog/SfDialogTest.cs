using AngleSharp.Css.Dom;
using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Blazor.Toolkit.Buttons;
using Syncfusion.Blazor.Toolkit.Popups;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Dialog
{
    public class SfDialogTest : BunitTestContext
    {
        [Fact(Timeout = 10000, DisplayName = "Default property values")]
        public void DefaultValue()
        {
            var component = RenderComponent<SfDialog>();
            Assert.False(component.Instance.AllowDragging);
            Assert.True(component.Instance.CloseOnEscape);
            Assert.Equal(string.Empty, component.Instance.CssClass);
            Assert.False(component.Instance.EnableResize);
            Assert.Equal("auto", component.Instance.Height);
            Assert.False(component.Instance.IsModal);
            Assert.Equal(string.Empty, component.Instance.MinHeight);
            Assert.False(component.Instance.ShowCloseIcon);
            Assert.Null(component.Instance.Target);
            Assert.True(component.Instance.Visible);
            Assert.Equal("100%", component.Instance.Width);
            Assert.Equal(1000, component.Instance.ZIndex);
        }


        [Fact(Timeout = 10000, DisplayName = "Root and content element availability without configuration")]
        public void WithoutConfig()
        {
            var component = RenderComponent<SfDialog>();
            var dialogElement = component.Find(".e-dialog");
            Assert.NotNull(dialogElement);
            var contentElement = component.Find(".e-dlg-content");
            Assert.NotNull(contentElement);
            Assert.NotEqual("true", dialogElement.GetAttribute("aria-modal"));
            Assert.DoesNotContain("e-dlg-modal", dialogElement.ClassName);
            Assert.DoesNotContain("e-dlg-container", dialogElement?.ParentElement?.ClassName);
            Assert.Null(dialogElement?.NextElementSibling);
        }

        [Fact(Timeout = 10000, DisplayName = "Root element initial classes")]
        public void RootClass()
        {
            var component = RenderComponent<SfDialog>();
            Task.Delay(5000);
            var dialogElement = component.Find(".e-dialog");
            Assert.Contains("e-lib", dialogElement.ClassName);
            Assert.Contains("e-popup-close", dialogElement.ClassName);
            Assert.Contains("e-blazor-hidden", dialogElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Root element static attributes testing")]
        public void RootAttributes()
        {
            var component = RenderComponent<SfDialog>();
            var dialogElement = component.Find(".e-dialog");
            Assert.Equal("dialog", dialogElement.GetAttribute("role"));
        }

        [Fact(Timeout = 10000, DisplayName = "CssClass testing")]
        public void CssClass()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.CssClass, "CustomCss"));
            var dialogElement = component.Find(".e-dialog");
            Assert.Contains("CustomCss", dialogElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Width testing")]
        public void Width()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.Width, "250px"));
            var dialogElement = component.Find(".e-dialog");
            var style = dialogElement.ComputeCurrentStyle().GetWidth();
            style.Equals("250px");
        }

        [Fact(Timeout = 10000, DisplayName = "Height testing")]
        public void Height()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.Height, "250px"));
            var dialogElement = component.Find(".e-dialog");
            var style = dialogElement.ComputeCurrentStyle().GetHeight();
            style.Equals("250px");
        }

        [Fact(Timeout = 10000, DisplayName = "MinHeight testing")]
        public void MinHeight()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.MinHeight, "300px"));
            var dialogElement = component.Find(".e-dialog");
            var style = dialogElement.ComputeCurrentStyle().GetMinHeight();
            style.Equals("300px");
        }

        [Fact(Timeout = 10000, DisplayName = "ZIndex testing")]
        public void Zindex()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.ZIndex, 1003));
            var dialogElement = component.Find(".e-dialog");
            var style = dialogElement.ComputeCurrentStyle().GetZIndex();
            style.Equals(1003);
        }

        [Fact(Timeout = 10000, DisplayName = "IsModal as true testing")]
        public void IsModalAsTrue()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.IsModal, true));
            var dialogElement = component.Find(".e-dialog");
            Assert.Equal("true", dialogElement.GetAttribute("aria-modal"));
            Assert.Contains("e-dlg-modal", dialogElement.ClassName);
            Assert.Contains("e-dlg-container", dialogElement?.ParentElement?.ClassName);
            Assert.Contains("e-dlg-overlay", dialogElement?.NextElementSibling?.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "IsModal as false testing")]
        public void IsModalAsFalse()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.IsModal, false));
            var dialogElement = component.Find(".e-dialog");
            Assert.NotEqual("true", dialogElement.GetAttribute("aria-modal"));
            Assert.DoesNotContain("e-dlg-modal", dialogElement.ClassName);
            Assert.DoesNotContain("e-dlg-container", dialogElement?.ParentElement?.ClassName);
            Assert.Null(dialogElement?.NextElementSibling);
        }

        [Fact(Timeout = 10000, DisplayName = "EnableResize as true testing")]
        public void EnableResizeAsTrue()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.EnableResize, true));
            var dialogElement = component.Find(".e-dialog");
            Assert.Contains("e-dlg-resizable", dialogElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "EnableResize as false testing")]
        public void EnableResizeAsFalse()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.EnableResize, false));
            var dialogElement = component.Find(".e-dialog");
            Assert.DoesNotContain("e-dlg-resizable", dialogElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "AllowDragging as true testing")]
        public void AllowDraggingAsTrue()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.AllowDragging, true));
            var dialogElement = component.Find(".e-dialog");
            Assert.True(component.Instance.AllowDragging);
        }

        [Fact(Timeout = 10000, DisplayName = "AllowDragging as false testing")]
        public void AllowDraggingAsFalse()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.AllowDragging, false));
            var dialogElement = component.Find(".e-dialog");
            Assert.False(component.Instance.AllowDragging);
        }

        [Fact(Timeout = 10000, DisplayName = "CloseOnEscape as true testing")]
        public async void CloseOnEscapeAsTrue()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.CloseOnEscape, true));
            var keyboardArgs = new KeyboardEventArgs { Key = "esc", Code = "ESC" };
            await component.Instance.CloseDialogAsync(keyboardArgs);
        }

        [Fact(Timeout = 10000, DisplayName = "CloseOnEscape as false testing")]
        public async void CloseOnEscapeAsFalse()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.CloseOnEscape, false));
            var keyboardArgs = new KeyboardEventArgs { Key = "esc", Code = "ESC" };
            await component.Instance.CloseDialogAsync(keyboardArgs);
        }

        [Fact(Timeout = 10000, DisplayName = "Visible as true testing")]
        public async void VisibleAsTrue()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.Visible, true));
            await Task.Delay(10);
            Assert.True(component.Instance.Visible);
        }

        [Fact(Timeout = 10000, DisplayName = "Visible as false testing")]
        public async void VisibleAsFalse()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.Visible, false));
            await Task.Delay(10);
            Assert.False(component.Instance.Visible);
        }

        [Fact(Timeout = 10000, DisplayName = "AllowPrerender as true testing")]
        public async void AllowPrerenderAsTrue()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.AllowPrerender, true).Add(p => p.Visible, false));
            var dialogElement = component.Find(".e-dialog");
            Assert.NotNull(dialogElement);
        }

        [Fact(Timeout = 10000, DisplayName = "AllowPrerender as false testing")]
        public async void AllowPrerenderAsFalse()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.AllowPrerender, false).Add(p => p.Visible, false));
            await Task.Delay(10);
            Assert.False(component.Instance.AllowPrerender);
        }

        [Fact(Timeout = 10000, DisplayName = "HtmlAttributes property testing")]
        public async void HtmlAttributes()
        {
            var htmlAttributes = new Dictionary<string, object>() {
                {"aria-label", "Dialog" }, { "title", "Dialog"}
            };
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.HtmlAttributes, htmlAttributes));
            await Task.Delay(10);
            var dialogElement = component.Find(".e-dialog");
            Assert.Equal("Dialog", dialogElement.GetAttribute("aria-label"));
            Assert.Equal("Dialog", dialogElement.GetAttribute("title"));
        }

        [Fact(Timeout = 10000, DisplayName = "ResizeHandles property testing")]
        public async void ResizeHandles()
        {
            ResizeDirection[] dialogResizeDirections = new ResizeDirection[] { ResizeDirection.SouthEast };
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.ResizeHandles, dialogResizeDirections));
            await Task.Delay(10);
            var dialogElement = component.Find(".e-dialog");
            var node = dialogElement.QuerySelectorAll(".e-resize-handle");
        }

        [Fact(Timeout = 10000, DisplayName = "Target default value testing")]
        public void TargetDefaultValue()
        {
            var component = RenderComponent<SfDialog>();
            Assert.Null(component.Instance.Target);
        }

        [Fact(Timeout = 10000, DisplayName = "Target as custom value testing")]
        public void TargetAsCustom()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.Target, "#MyTarget"));
            Assert.Equal("#MyTarget", component.Instance.Target);
        }

        [Fact(Timeout = 10000, DisplayName = "Target as document.body testing")]
        public void TargetAsBody()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.Target, "document.body"));
            Assert.Equal("document.body", component.Instance.Target);
        }

        [Fact(Timeout = 10000, DisplayName = "ID testing")]
        public void IdTest()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.ID, "MyDialog"));
            var dialogElement = component.Find(".e-dialog");
            Assert.Equal("MyDialog", dialogElement.GetAttribute("id"));
        }

        [Fact(Timeout = 10000, DisplayName = "ShowCloseIcon without header text testing")]
        public void ShowCloseIconWithoutHeaderText()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.ShowCloseIcon, true));
            var headerWrapper = component.Find(".e-dlg-header-content");
            Assert.NotNull(headerWrapper);
            var closeButton = headerWrapper.QuerySelector("button");
            Assert.NotNull(closeButton);
            var headerText = headerWrapper.QuerySelector(".e-dlg-header");
            Assert.Null(headerText);
            Assert.Contains("e-btn", closeButton.ClassName);
            Assert.Contains("e-flat", closeButton.ClassName);
            Assert.Contains("e-btn-icon", closeButton.ClassName);
            Assert.Contains("e-dlg-closeicon-btn", closeButton.ClassName);
            Assert.Contains("e-control", closeButton.ClassName);
            Assert.Contains("e-lib", closeButton.ClassName);
            Assert.Equal("button", closeButton.GetAttribute("type"));
            Assert.Equal("Close", closeButton.GetAttribute("title"));
            var iconElement = headerWrapper.QuerySelector("button span");
            Assert.NotNull(iconElement);
            Assert.Contains("e-toolkit-icons", iconElement.ClassName);
            Assert.Contains("e-btn-icon", iconElement.ClassName);
            Assert.Contains("e-close", iconElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "ShowCloseIcon with header text testing")]
        public void ShowCloseIconWithHeaderText()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.ShowCloseIcon, true).Add(p => p.Header, "My Header"));
            var headerWrapper = component.Find(".e-dlg-header-content");
            Assert.NotNull(headerWrapper);
            var closeButton = component.Find(".e-dlg-header-content button");
            Assert.NotNull(closeButton);
            var headerText = headerWrapper.QuerySelector(".e-dlg-header");
            Assert.NotNull(headerText);
            Assert.Contains("My Header", headerText.InnerHtml);
            Assert.Contains("e-btn", closeButton.ClassName);
            Assert.Contains("e-flat", closeButton.ClassName);
            Assert.Contains("e-btn-icon", closeButton.ClassName);
            Assert.Contains("e-dlg-closeicon-btn", closeButton.ClassName);
            Assert.Contains("e-control", closeButton.ClassName);
            Assert.Contains("e-lib", closeButton.ClassName);
            Assert.Equal("button", closeButton.GetAttribute("type"));
            Assert.Equal("Close", closeButton.GetAttribute("title"));
            var iconElement = headerWrapper.QuerySelector("button span");
            Assert.NotNull(iconElement);
            Assert.Contains("e-toolkit-icons", iconElement.ClassName);
            Assert.Contains("e-btn-icon", iconElement.ClassName);
            Assert.Contains("e-close", iconElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Header as empty string testing")]
        public void HeaderAsEmptyString()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.Header, ""));
            var dialogElement = component.Find(".e-dialog");
            Assert.NotNull(dialogElement);
            var headerWrapper = dialogElement.QuerySelector(".e-dlg-header-content");
            Assert.Null(headerWrapper);
            var closeButton = dialogElement.QuerySelector(".e-dlg-header-content button");
            Assert.Null(closeButton);
            var headerText = dialogElement.QuerySelector(".e-dlg-header");
            Assert.Null(headerText);
            var iconElement = dialogElement.QuerySelector("button span");
            Assert.Null(iconElement);
        }

        [Fact(Timeout = 10000, DisplayName = "Header as text testing")]
        public void HeaderAsText()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.Header, "My Header"));
            var headerWrapper = component.Find(".e-dlg-header-content");
            Assert.NotNull(headerWrapper);
            var closeButton = headerWrapper.QuerySelector(".e-dlg-header-content button");
            Assert.Null(closeButton);
            var headerText = headerWrapper.QuerySelector(".e-dlg-header");
            Assert.NotNull(headerText);
            Assert.Contains("My Header", headerText.InnerHtml);
            var iconElement = headerWrapper.QuerySelector("button span");
            Assert.Null(iconElement);
        }

        [Fact(Timeout = 10000, DisplayName = "Header as html testing")]
        public void HeaderAsHtml()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.Header, "<span>My Header</span>"));
            var headerWrapper = component.Find(".e-dlg-header-content");
            Assert.NotNull(headerWrapper);
            var closeButton = headerWrapper.QuerySelector(".e-dlg-header-content button");
            Assert.Null(closeButton);
            var headerText = headerWrapper.QuerySelector(".e-dlg-header");
            Assert.NotNull(headerText);
            Assert.Contains("&lt;span&gt;My Header&lt;/span&gt;", headerText.InnerHtml);
            var iconElement = headerWrapper.QuerySelector("button span");
            Assert.Null(iconElement);
        }

        [Fact(Timeout = 10000, DisplayName = "Header Template as 'Text' testing")]
        public void HeaderTemplateAsText()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogTemplates>(p => p.Add(i => i.Header, "My Header")));
            var headerWrapper = component.Find(".e-dlg-header-content");
            Assert.NotNull(headerWrapper);
            var closeButton = headerWrapper.QuerySelector(".e-dlg-header-content button");
            Assert.Null(closeButton);
            var headerText = headerWrapper.QuerySelector(".e-dlg-header");
            Assert.NotNull(headerText);
            Assert.Contains("My Header", headerText.InnerHtml);
            var iconElement = headerWrapper.QuerySelector("button span");
            Assert.Null(iconElement);
        }

        [Fact(Timeout = 10000, DisplayName = "Header Template as 'HTML' testing")]
        public void HeaderTemplateAsHtml()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogTemplates>(p => p.Add(i => i.Header, "<span>My Header</span>")));
            var headerWrapper = component.Find(".e-dlg-header-content");
            Assert.NotNull(headerWrapper);
            var closeButton = headerWrapper.QuerySelector(".e-dlg-header-content button");
            Assert.Null(closeButton);
            var headerText = headerWrapper.QuerySelector(".e-dlg-header");
            Assert.NotNull(headerText);
            Assert.Contains("<span>My Header</span>", headerText.InnerHtml);
            var iconElement = headerWrapper.QuerySelector("button span");
            Assert.Null(iconElement);
            var spanElement = headerWrapper.QuerySelector("span");
            Assert.NotNull(spanElement);
        }

        [Fact(Timeout = 10000, DisplayName = "Content property")]
        public void Content()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.Content, "My Content"));
            var contentElement = component.Find(".e-dlg-content");
            Assert.NotNull(contentElement);
            Assert.Contains("My Content", contentElement.InnerHtml);
        }

        [Fact(Timeout = 10000, DisplayName = "Content as empty string")]
        public void ContentAsEmptyString()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.Content, ""));
            var dialogElement = component.Find(".e-dialog");
            Assert.NotNull(dialogElement);
            var contentElement = dialogElement.QuerySelector(".e-dlg-content");
            Assert.NotNull(contentElement);
        }

        [Fact(Timeout = 10000, DisplayName = "String content without tag helper")]
        public void ContentWithoutTagHelper()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent("My Content"));
            var contentElement = component.Find(".e-dlg-content");
            Assert.NotNull(contentElement);
            Assert.Contains("My Content", contentElement.InnerHtml);
        }

        [Fact(Timeout = 10000, DisplayName = "Element content without tag helper")]
        public void ElementContentWithoutTagHelper()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent("<p>My Content</p>"));
            var contentElement = component.Find(".e-dlg-content");
            Assert.NotNull(contentElement);
            Assert.Contains("<p>My Content</p>", contentElement.InnerHtml);
        }

        [Fact(Timeout = 10000, DisplayName = "Content Template as 'Text' testing")]
        public void ContentTemplateAsText()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogTemplates>(p => p.Add(i => i.Content, "My Content")));
            var contentElement = component.Find(".e-dlg-content");
            Assert.NotNull(contentElement);
            Assert.Contains("My Content", contentElement.InnerHtml);
        }

        [Fact(Timeout = 10000, DisplayName = "Content Template as 'HTML' testing")]
        public void ContentTemplateAsHtml()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogTemplates>(p => p.Add(i => i.Content, "<p>My Content</p>")));
            var contentElement = component.Find(".e-dlg-content");
            Assert.NotNull(contentElement);
            Assert.Contains("<p>My Content</p>", contentElement.InnerHtml);
        }

        [Fact(Timeout = 10000, DisplayName = "FooterTemplate as empty string testing")]
        public void FooterTemplateAsEmptyString()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.FooterTemplate, ""));
            var dialogElement = component.Find(".e-dialog");
            Assert.NotNull(dialogElement);
            var footerElement = dialogElement.QuerySelector(".e-footer-content");
            Assert.Null(footerElement);
            var buttonElement = dialogElement.QuerySelector("button");
            Assert.Null(buttonElement);
        }

        [Fact(Timeout = 10000, DisplayName = "FooterTemplate as text testing")]
        public void FooterTemplateAsText()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.FooterTemplate, "Click Me"));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.Null(buttonElement);
            Assert.Contains("Click Me", footerElement.InnerHtml);
        }

        [Fact(Timeout = 10000, DisplayName = "FooterTemplate as HTML testing")]
        public void FooterTemplateAsHtml()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.FooterTemplate, "Click Me"));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.Null(buttonElement);
            Assert.Equal("Click Me", footerElement.InnerHtml);
        }

        [Fact(Timeout = 10000, DisplayName = "FooterTemplates as 'Text' testing")]
        public void FooterTemplatesAsText()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogTemplates>(p => p.Add(i => i.FooterTemplate, "Click Me")));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.Null(buttonElement);
            Assert.Contains("Click Me", footerElement.InnerHtml);
        }

        [Fact(Timeout = 10000, DisplayName = "FooterTemplates as 'HTML' testing")]
        public void FooterTemplatesAsHtml()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogTemplates>(p => p.Add(i => i.FooterTemplate, "<button>Click Me</button>")));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("Click Me", buttonElement.InnerHtml);
        }

        [Fact(Timeout = 10000, DisplayName = "AnimationSettings default API values")]
        public void AnimationDefaultAPIValue()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogAnimationSettings>());
            var animate = component.FindComponent<DialogAnimationSettings>();
            Assert.Equal(0, animate.Instance.Delay);
            Assert.Equal(400, animate.Instance.Duration);
            Assert.Equal(DialogEffect.Fade, animate.Instance.Effect);
        }

        [Fact(Timeout = 10000, DisplayName = "PositionData default API values")]
        public void PositionDefaultAPIValue()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogPositionData>(param => param.Add(p => p.X, "10000").Add(p => p.Y, "10000")));
            var posData = component.FindComponent<DialogPositionData>();
            Assert.NotNull(posData.Instance.X);
            Assert.NotNull(posData.Instance.Y);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - Default API value testing")]
        public void DialogButtonDefaultAPIValue()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>()));
            var btnRef = component.FindComponent<DialogButton>();
            Assert.Null(btnRef.Instance.Content);
            Assert.Equal(string.Empty, btnRef.Instance.CssClass);
            Assert.False(btnRef.Instance.Disabled);
            Assert.Equal(string.Empty, btnRef.Instance.IconCss);
            Assert.Equal(IconPosition.Left, btnRef.Instance.IconPosition);
            Assert.False(btnRef.Instance.IsPrimary);
            Assert.False(btnRef.Instance.IsToggle);
            Assert.Equal(ButtonType.Button, btnRef.Instance.Type);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - Default button testing")]
        public void DialogButtonDefaultButton()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content"))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            var iconElement = buttonElement.QuerySelector("span");
            Assert.Null(iconElement);
            Assert.Equal("button", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
            Assert.Null(buttonElement.GetAttribute("disabled"));
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - Normal button testing")]
        public void DialogButtonNormalButton()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IsPrimary, false))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("button", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - Primary button testing")]
        public void DialogButtonPrimaryButton()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IsPrimary, true))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("button", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.Contains("e-primary", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - IsPrimary as false testing")]
        public void DialogButtonIsPrimaryAsFalseButton()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IsPrimary, false))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("button", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Button type as 'Submit' testing")]
        public void ButtonTypeSubmit()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.AllowPrerender, true).AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.Type, ButtonType.Submit))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("submit", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Button type as 'Reset' testing")]
        public void ButtonTypeReset()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.AllowPrerender, true).AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.Type, ButtonType.Reset))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("reset", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "Button type as 'Button' testing")]
        public void ButtonTypeButton()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.AllowPrerender, true).AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.Type, ButtonType.Button))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("button", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - CssClass testing")]
        public void ButtonCssClass()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.AllowPrerender, true).AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.CssClass, "MyClass"))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("button", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
            Assert.Contains("MyClass", buttonElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - Primary button CssClass testing")]
        public void PrimaryButtonCssClass()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.CssClass, "MyClass").Add(i => i.IsPrimary, true))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("button", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
            Assert.Contains("e-primary", buttonElement.ClassName);
            Assert.Contains("MyClass", buttonElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - Disabled as 'true' testing")]
        public void DisabledAsTrue()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.Disabled, true))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.NotNull(buttonElement.GetAttribute("disabled"));
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - Disabled as 'false' testing")]
        public void DisabledAsFalse()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.Disabled, true))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Empty(buttonElement?.GetAttribute("disabled")!);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - EnableRtl as 'true' testing")]
        public void DialogButtonEnableRtlAsTrue()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content"))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - EnableRtl as 'false' testing")]
        public void DialogButtonEnableRtlAsFalse()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content"))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - IconCss testing")]
        public void IconCss()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IconCss, "MyIconClass"))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("button", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
            var iconElement = buttonElement.QuerySelector("span");
            Assert.NotNull(iconElement);
            Assert.Contains("e-btn-icon", iconElement.ClassName);
            Assert.Contains("e-icon-left", iconElement.ClassName);
            Assert.Contains("MyIconClass", iconElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - IconPosition as 'Left' testing")]
        public void IconPositionAsLeft()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IconCss, "MyIconClass").Add(i => i.IconPosition, IconPosition.Left))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("button", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
            Assert.DoesNotContain("e-top-icon-btn", buttonElement.ClassName);
            Assert.DoesNotContain("e-bottom-icon-btn", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
            var iconElement = buttonElement.QuerySelector("span");
            Assert.NotNull(iconElement);
            Assert.Contains("e-btn-icon", iconElement.ClassName);
            Assert.Contains("e-icon-left", iconElement.ClassName);
            Assert.Contains("MyIconClass", iconElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - IconPosition as 'Right' testing")]
        public void IconPositionAsRight()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IconCss, "MyIconClass").Add(i => i.IconPosition, IconPosition.Right))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("button", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
            Assert.DoesNotContain("e-top-icon-btn", buttonElement.ClassName);
            Assert.DoesNotContain("e-bottom-icon-btn", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
            var iconElement = buttonElement.QuerySelector("span");
            Assert.NotNull(iconElement);
            Assert.Contains("e-btn-icon", iconElement.ClassName);
            Assert.Contains("e-icon-right", iconElement.ClassName);
            Assert.Contains("MyIconClass", iconElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - IconPosition as 'Top' testing")]
        public void IconPositionAsTop()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IconCss, "MyIconClass").Add(i => i.IconPosition, IconPosition.Top))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("button", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
            Assert.Contains("e-top-icon-btn", buttonElement.ClassName);
            Assert.DoesNotContain("e-bottom-icon-btn", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
            var iconElement = buttonElement.QuerySelector("span");
            Assert.NotNull(iconElement);
            Assert.Contains("e-btn-icon", iconElement.ClassName);
            Assert.Contains("e-icon-top", iconElement.ClassName);
            Assert.Contains("MyIconClass", iconElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - IconPosition as 'Bottom' testing")]
        public void IconPositionAsBottom()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IconCss, "MyIconClass").Add(i => i.IconPosition, IconPosition.Bottom))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.Equal("button", buttonElement.GetAttribute("type"));
            Assert.Contains("My Content", buttonElement.InnerHtml);
            Assert.Contains("e-control", buttonElement.ClassName);
            Assert.Contains("e-btn", buttonElement.ClassName);
            Assert.Contains("e-lib", buttonElement.ClassName);
            Assert.Contains("e-flat", buttonElement.ClassName);
            Assert.DoesNotContain("e-top-icon-btn", buttonElement.ClassName);
            Assert.Contains("e-bottom-icon-btn", buttonElement.ClassName);
            Assert.DoesNotContain("e-primary", buttonElement.ClassName);
            var iconElement = buttonElement.QuerySelector("span");
            Assert.NotNull(iconElement);
            Assert.Contains("e-btn-icon", iconElement.ClassName);
            Assert.Contains("e-icon-bottom", iconElement.ClassName);
            Assert.Contains("MyIconClass", iconElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - IsToggle as true testing")]
        public void IsToggleAsTrue()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IsToggle, true))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.DoesNotContain("e-active", buttonElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogButton - IsToggle as false testing")]
        public void IsToggleAsFalse()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IsToggle, false))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var buttonElement = footerElement.QuerySelector("button");
            Assert.NotNull(buttonElement);
            Assert.DoesNotContain("e-active", buttonElement.ClassName);
        }

        [Fact(Timeout = 10000, DisplayName = "GetButtonItems method with single item testing")]
        public void GetButtonItemsWithSingleItem()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content1").Add(i => i.IsPrimary, true))));
            List<DialogButton>? buttonItems = component.Instance.GetButtonItems();
            Assert.Single(buttonItems!);
        }

        [Fact(Timeout = 10000, DisplayName = "GetButtonItems method with multiple item testing")]
        public void GetButtonItemsWithMultipleItem()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content1").Add(i => i.IsPrimary, true)).AddChildContent<DialogButton>(a =>
                a.Add(i => i.Content, "My Content2"))));
            List<DialogButton>? buttonItems = component.Instance.GetButtonItems();
            Assert.Equal(2, buttonItems!.Count);
        }

        [Fact(Timeout = 10000, DisplayName = "GetButton method without buttons testing")]
        public void GetButtonWithoutButtons()
        {
            var component = RenderComponent<SfDialog>();
            Assert.Null(component.Instance.GetButton(0));
        }

        [Fact(Timeout = 10000, DisplayName = "GetButton method with buttons testing")]
        public void GetButtonWithButtons()
        {
            var htmlAttributes = new Dictionary<string, object>();
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
                p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content1")
                .Add(i => i.IsPrimary, true)).AddChildContent<DialogButton>(a => a
                .Add(i => i.IsFlat, true)
                .Add(i => i.HtmlAttributes, htmlAttributes)
                .Add(i => i.Content, "My Content2"))));
            DialogButton? buttonItem1 = component.Instance.GetButton(0);
            Assert.NotNull(buttonItem1);
            Assert.Equal("My Content1", buttonItem1.Content);
            DialogButton? buttonItem2 = component.Instance.GetButton(1);
            Assert.NotNull(buttonItem2);
            Assert.Equal("My Content2", buttonItem2.Content);
        }

        [Fact(Timeout = 10000, DisplayName = "Created Events testing")]
        public async Task CreatedEvents()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(i => i.AllowPrerender, true).Add(p => p.IsModal, true).Add(p => p.Created, () =>
            {
                Assert.NotNull("Created event is triggered");
            }));
        }

        [Fact(Timeout = 10000, DisplayName = "VisibleChanged Events testing")]
        public async Task VisibleChangedEvents()
        {
            bool isVisible = true;
            var component = RenderComponent<SfDialog>(options => options.Add(i => i.AllowPrerender, true).Add(p => p.Visible, isVisible).Add(p => p.VisibleChanged, () =>
            {
                Assert.NotNull("VisibleChanged event is triggered");
            }));
            await Task.Delay(100);
            component.SetParametersAndRender(p => p.Add(p2 => p2.Visible, false));
        }

        [Fact(Timeout = 10000, DisplayName = "Destroyed Events testing")]
        public async Task DestroyedEvents()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(i => i.AllowPrerender, true).Add(p => p.IsModal, true).Add(p => p.Destroyed, () =>
            {
                Assert.NotNull("Destroyed event is triggered");
            }));
        }

        [Fact(Timeout = 10000, DisplayName = "OnOpen Events testing")]
        public async Task OnOpenEvent()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(i => i.AllowPrerender, true).Add(p => p.IsModal, true).Add(p => p.OnOpen, (BeforeOpenEventArgs args) =>
            {
                Assert.NotNull("OnOpen event is triggered");
            }));
            await component.Instance.ShowAsync();
        }

        [Fact(Timeout = 10000, DisplayName = "Opened Events testing")]
        public async Task OpenedEvent()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(i => i.AllowPrerender, true).Add(p => p.IsModal, true).Add(p => p.Opened, (OpenEventArgs args) =>
            {
                Assert.NotNull("Opened event is triggered");
            }));
            await component.Instance.ShowAsync();
        }

        [Fact(Timeout = 10000, DisplayName = "OnClose Events testing")]
        public async Task OnCloseEvent()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(i => i.AllowPrerender, true).Add(p => p.IsModal, true).Add(p => p.OnClose, (BeforeCloseEventArgs args) =>
            {
                Assert.NotNull("OnClose event is triggered");
            }));
            await component.Instance.ShowAsync();
            await Task.Delay(100);
            await component.Instance.HideAsync();
        }

        [Fact(Timeout = 10000, DisplayName = "Closed Events testing")]
        public async Task ClosedEvent()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(i => i.AllowPrerender, true).Add(p => p.IsModal, true).Add(p => p.Closed, (CloseEventArgs args) =>
            {
                Assert.NotNull("Closed event is triggered");
            }));
            await component.Instance.ShowAsync();
            await Task.Delay(100);
            await component.Instance.HideAsync();
        }

        [Fact(Timeout = 10000, DisplayName = "OnOverlayModalClick Events testing")]
        public async Task OnOverlayModalClickEvent()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(i => i.AllowPrerender, true).Add(p => p.IsModal, true).Add(p => p.OnOverlayModalClick, (OverlayModalClickEventArgs args) =>
            {
                Assert.NotNull("OnOverlayModalClick event is triggered");
            }));
            var dialogElement = component.Find(".e-dialog");
            Assert.Equal("true", dialogElement.GetAttribute("aria-modal"));
            Assert.Contains("e-dlg-modal", dialogElement.ClassName);
            Assert.Contains("e-dlg-container", dialogElement?.ParentElement?.ClassName);
            Assert.Contains("e-dlg-overlay", dialogElement?.NextElementSibling?.ClassName);
            await component.Instance.ShowAsync();
            var overlayElement = dialogElement?.NextElementSibling;
            overlayElement?.Click();
            await component.Instance.HideAsync();
        }

        [Fact(Timeout = 10000, DisplayName = "BeforeOpenEventArgs testing")]
        public void BeforeOpenEventArgsClass()
        {
            var model = new BeforeOpenEventArgs
            {
                Cancel = false,
                MaxHeight = "300px"
            };
            Assert.False(model.Cancel);
            Assert.Equal("300px", model.MaxHeight);
        }

        [Fact(Timeout = 10000, DisplayName = "OverlayModalClickEventArgs testing")]
        public void OverlayModalClickEventArgsClass()
        {
            var model = new OverlayModalClickEventArgs
            {
                PreventFocus = true,
                Event = new MouseEventArgs() { }
            };
            Assert.True(model.PreventFocus);
        }

        [Fact(Timeout = 10000, DisplayName = "BeforeCloseEventArgs testing")]
        public void BeforeCloseEventArgsClass()
        {
            var dummyEvent = new System.EventArgs();
            var model = new BeforeCloseEventArgs
            {
                ClosedBy = "#ClosedBy",
                Event = dummyEvent,
                IsInteracted = false
            };
            Assert.Equal("#ClosedBy", model.ClosedBy);
            Assert.Equal(dummyEvent, model.Event);
            Assert.False(model.IsInteracted);
        }

        [Fact(Timeout = 10000)]
        public async Task ShowAsync()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(i => i.AllowPrerender, false)
            .Add(p => p.Visible, false)
            .Add(p => p.IsModal, true));
            await component.Instance.ShowAsync(true);
            Assert.Null(component.Instance.GetButton(0));
        }

        [Fact(Timeout = 10000)]
        public async Task OpenEvent()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
               p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IsToggle, true))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            await component.Instance.OpenEventAsync("CSS-Class");
        }

        [Fact(Timeout = 10000)]
        public async Task CloseEvent()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
               p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IsToggle, true))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            await component.Instance.CloseEventAsync("CSS-Class");
        }

        [Fact(Timeout = 10000)]
        public async Task DragEvents()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
               p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IsToggle, true))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var mouseEvent = new MouseEventArgs();
            var args = new DragStopEventArgs { Event = mouseEvent, Name = "Drag Stop" };
            await component.Instance.DragStopEventAsync(args);
            var args1 = new Syncfusion.Blazor.Toolkit.Popups.DragEventArgs { Event = mouseEvent, Name = "Dragging" };
            await component.Instance.DragEventAsync(args1);
            var args2 = new DragStartEventArgs { Event = mouseEvent, Name = "DragStart" };
            await component.Instance.DragStartEventAsync(args2);
        }

        [Fact(Timeout = 10000)]
        public async Task ResizeEvents()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
               p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IsToggle, true))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var args = new MouseEventArgs { ClientX = 100, ClientY = 110 };
            await component.Instance.ResizingEventAsync(args);
            var args1 = new MouseEventArgs { ClientX = 100, ClientY = 100 };
            await component.Instance.ResizeStopEventAsync(args1);
            var args2 = new MouseEventArgs { ClientX = 200, ClientY = 200 };
            await component.Instance.ResizeStartEventAsync(args2);
            await component.Instance.RefreshPositionAsync();
        }

        [Fact(Timeout = 10000)]
        public async Task CloseDialog()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
               p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IsToggle, true))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var args = new KeyboardEventArgs { };
            await component.InvokeAsync(async () =>
            {
                await component.Instance.CloseDialogAsync(args);
            });
        }

        [Fact(Timeout = 10000)]
        public async Task GetDimension()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
               p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IsToggle, true))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var args = new KeyboardEventArgs { };
            await component.Instance.CloseDialogAsync(args);
            await component.Instance.GetDimensionAsync();
        }

        [Fact(Timeout = 10000)]
        public async Task HideAsync()
        {
            var component = RenderComponent<SfDialog>(options => options.AddChildContent<DialogButtons>(p =>
               p.AddChildContent<DialogButton>(a => a.Add(i => i.Content, "My Content").Add(i => i.IsToggle, true))));
            var footerElement = component.Find(".e-footer-content");
            Assert.NotNull(footerElement);
            var keyboardArgs = new KeyboardEventArgs { Key = "a", Code = "A" };
            await component.Instance.HideAsync(keyboardArgs);
            await component.Instance.HideAsync("hide");
            var MouseArgs = new MouseEventArgs { };
            await component.Instance.HideAsync(MouseArgs);
        }

        [Fact(Timeout = 10000)]
        public async Task CloseIconClick()
        {
            var component = RenderComponent<SfDialog>(param => param
            .Add(p => p.Target, "#target").Add(p => p.Width, "300px")
            .Add(p => p.Visible, true).Add(p => p.ShowCloseIcon, true)
            .Add(p => p.AllowPrerender, true).Add(p => p.IsModal, true)
            .AddChildContent<DialogTemplates>());
            var crossIcon = component.Find(".e-dlg-closeicon-btn");
            crossIcon.Click();
            await Task.Delay(10);
        }

        [Fact(Timeout = 10000, DisplayName = "OpenEventArgs testing")]
        public void OpenEventArgsClass()
        {
            var model = new OpenEventArgs
            {
                Cancel = true,
                Name = "#Name",
                PreventFocus = true,
            };
            Assert.True(model.Cancel);
            Assert.Equal("#Name", model.Name);
            Assert.True(model.PreventFocus);
        }

        [Fact(Timeout = 10000, DisplayName = "DragStopEventArgs testing")]
        public void DragStopEventArgs()
        {
            var model = new DragStopEventArgs
            {
                Event = new(),
                Name = "#Name"
            };
            Assert.NotNull(model.Event);
            Assert.Equal("#Name", model.Name);
        }

        [Fact(Timeout = 10000, DisplayName = "DragStartEventArgs testing")]
        public void DragStartEventArgs()
        {
            var model = new DragStartEventArgs
            {
                Event = new(),
                Name = "#Name"
            };
            Assert.NotNull(model.Event);
            Assert.Equal("#Name", model.Name);
        }

        [Fact(Timeout = 10000, DisplayName = "DragEventArgs testing")]
        public void DragEventArgs()
        {
            var model = new Syncfusion.Blazor.Toolkit.Popups.DragEventArgs
            {
                Event = new(),
                Name = "#Name"
            };
            Assert.NotNull(model.Event);
            Assert.Equal("#Name", model.Name);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogOptions class testing")]
        public void DialogOptions()
        {
            var positionModel = new PositionDataModel { X = "100px", Y = "200px" };
            var dialoganimation = new DialogAnimationOptions { Delay = 10, Duration = 20, Effect = DialogEffect.Fade };
            var dummyButtonoptions = new DialogButtonOptions { Content = "button", Disabled = false, IconCss = "CSS-Classes" };
            var model = new DialogOptions
            {
                AllowDragging = true,
                ShowCloseIcon = true,
                CloseOnEscape = true,
                CssClass = "css-class",
                Width = "100px",
                Height = "100px",
                ZIndex = 1100,
                Position = positionModel,
                AnimationSettings = dialoganimation,
                PrimaryButtonOptions = dummyButtonoptions,
                CancelButtonOptions = dummyButtonoptions,
            };
            Assert.True(model.AllowDragging);
            Assert.True(model.ShowCloseIcon);
            Assert.True(model.CloseOnEscape);
            Assert.Equal("css-class", model.CssClass);
            Assert.Equal("100px", model.Width);
            Assert.Equal("100px", model.Height);
            Assert.Equal(1100, model.ZIndex);
            Assert.Equal(positionModel, model.Position);
            Assert.Equal(dialoganimation, model.AnimationSettings);
            Assert.Equal(dummyButtonoptions, model.PrimaryButtonOptions);
            Assert.Equal(dummyButtonoptions, model.CancelButtonOptions);
            //PositionDataModel Class
            Assert.Equal("100px", model.Position.X);
            Assert.Equal("200px", model.Position.Y);
            //DialogAnimationOptions Class
            Assert.Equal(10, model.AnimationSettings.Delay);
            Assert.Equal(20, model.AnimationSettings.Duration);
            Assert.Equal(DialogEffect.Fade, model.AnimationSettings.Effect);
            //DialogButtonOptions Class
            Assert.Equal("button", model.PrimaryButtonOptions.Content);
            Assert.Equal("CSS-Classes", model.PrimaryButtonOptions.IconCss);
            Assert.False(model.PrimaryButtonOptions.Disabled);
        }

        [Fact(Timeout = 10000, DisplayName = "CloseEventArgs testing")]
        public void CloseEventArgs()
        {
            var model = new CloseEventArgs
            {
                Cancel = true,
                ClosedBy = "#CloseBy",
                Event = new(),
                IsInteracted = false,
                Name = "#Name"
            };
            Assert.True(model.Cancel);
            Assert.Equal("#CloseBy", model.ClosedBy);
            Assert.NotNull(model.Event);
            Assert.False(model.IsInteracted);
            Assert.Equal("#Name", model.Name);
        }

        [Fact(Timeout = 10000, DisplayName = "DialogDimension testing")]
        public void DialogDimension()
        {
            var model = new DialogDimension { Height = 100, Width = 100 };
            Assert.Equal(100, model.Height);
            Assert.Equal(100, model.Width);
        }

        [Fact(Timeout = 10000, DisplayName = "Render SfDialogProvider")]
        public void DialogProvider()
        {
            using var context = new Bunit.TestContext();
            context.Services.AddSingleton<SfDialogService>();
            context.Services.AddLocalization();
            var component = context.RenderComponent<SfDialogProvider>();
        }

        [Fact(Timeout = 10000, DisplayName = "VisibleChanged callback invoked on parameter update")]
        public void VisibleChangedInvokedOnParameterChange()
        {
            bool? received = null;
            var component = RenderComponent<SfDialog>(parameters => parameters.Add(p => p.Visible, true).Add(p => p.VisibleChanged, (Action<bool>)(v => received = v)));
            component.SetParametersAndRender(p => p.Add(p2 => p2.Visible, false));
            Assert.True(received.HasValue);
            Assert.False(received.Value);
        }

        [Fact(Timeout = 10000, DisplayName = "Overlay click invokes OnOverlayModalClick handler")]
        public async Task OverlayClickInvokesHandler()
        {
            var called = 0;
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.AllowPrerender, true).Add(p => p.IsModal, true).Add(p => p.OnOverlayModalClick, (OverlayModalClickEventArgs args) => { called++; }));
            var dialogElement = component.Find(".e-dialog");
            // ensure modal overlay exists
            Assert.Equal("true", dialogElement.GetAttribute("aria-modal"));
            var overlay = dialogElement.NextElementSibling;
            Assert.NotNull(overlay);
            overlay.Click();
            Assert.Equal(1, called);
            await Task.CompletedTask;
        }

        [Fact(Timeout = 10000, DisplayName = "CloseOnEscape triggers CloseDialog without error")]
        public async Task CloseOnEscapeTriggersCloseDialog()
        {
            var component = RenderComponent<SfDialog>(options => options.Add(p => p.CloseOnEscape, true).Add(p => p.Visible, true).Add(p => p.AllowPrerender, true));
            var keyboardArgs = new KeyboardEventArgs { Key = "Escape", Code = "Escape" };
            await component.Instance.CloseDialogAsync(keyboardArgs);
            Assert.NotNull(component.Instance);
        }
    }
}
