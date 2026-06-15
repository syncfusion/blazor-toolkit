using System;
using System.IO;
using AngleSharp.Dom;
using Bunit;
using Xunit;
using Syncfusion.Blazor.Toolkit.Buttons;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Syncfusion.Blazor.Toolkit.Tests.Buttons
{
    public partial class SfButtonTest : BunitTestContext
    {
        #region Default rendering

        // Verifies a button rendered with no text has empty content.
        [Trait("SfButton", "Content")]
        [Fact(Timeout = 10000, DisplayName = "No Text")]
        public void ButtonWithNoText()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.Find("button");
            var buttonText = buttonElement.TextContent.Trim();
            Assert.Equal("", buttonText);
        }

        // Verifies child content rendered through component instances (initial structure).
        [Trait("SfButton", "Content")]
        [Fact(Timeout = 10000, DisplayName = "Text")]
        public void ButtonTextThroughApi()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.FindAll("button")[1];
            var buttonText = buttonElement.TextContent.Trim();
            Assert.Equal("Button", buttonText);
        }

        // Verifies child content rendered as plain child content.
        [Trait("SfButton", "Content")]
        [Fact(Timeout = 10000, DisplayName = "Text as child content")]
        public void ButtonTextThroughChildContent()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.FindAll("button")[2];
            var buttonText = buttonElement.TextContent.Trim();
            Assert.Equal("Child Content", buttonText);
        }

        // Verifies HTML element inside child content renders as expected.
        [Trait("SfButton", "Content")]
        [Fact(Timeout = 10000, DisplayName = "Text as html element")]
        public void ButtonTextThroughHTMLChildContent()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.FindAll("button")[3];
            var buttonText = buttonElement.Children[0].OuterHtml;
            Assert.Equal("<div> ChildContent Button</div>", buttonText);
        }

        // Verifies default style for button.
        [Trait("SfButton", "Style")]
        [Fact(Timeout = 10000, DisplayName = "Default")]
        public void DefaultButton()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.Find("button");            
            Assert.Contains("e-btn", buttonElement.ClassList);
            Assert.Contains("e-control", buttonElement.ClassList);
            Assert.Contains("e-lib", buttonElement.ClassList);
        }

        // Verifies default classes and aria-disabled attribute on basic render.
        [Fact(DisplayName = "Default button renders base classes and aria-disabled=false")]
        public void DefaultButton_RendersRootCssAndAriaDisabled()
        {
            // Test Group: Initial Rendering
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Content, "Default"));

            var buttonElement = renderedComponent.Find("button");

            Assert.Contains("e-control", buttonElement.ClassList);
            Assert.Contains("e-btn", buttonElement.ClassList);
            Assert.Contains("e-lib", buttonElement.ClassList);
            Assert.Equal("false", buttonElement.GetAttribute("aria-disabled"));
        }
        #endregion

        #region API names (public properties)

        // Verifies title attribute and type are applied on rendered button.
        [Trait("SfButton", "ButtonType")]
        [Fact(Timeout = 10000, DisplayName = "Submit")]
        public void SubmitButton()
        {
            var renderedComponent = RenderComponent<AttributesAndTypes>();
            var buttonElement = renderedComponent.FindAll("button")[0];            
            Assert.Equal("Button tooltip", buttonElement.GetAttribute("title"));
        }

        // Verifies the default button type is "button" when Type property is not set.
        [Trait("SfButton", "ButtonType")]
        [Fact(Timeout = 10000, DisplayName = "Default button type is button")]
        public void DefaultButtonType_IsButton()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Content, "Default Type"));

            var buttonElement = renderedComponent.Find("button");
            Assert.Equal("button", buttonElement.GetAttribute("type"));
        }

        // Verifies button type is "submit" when Type is set to ButtonType.Submit.
        [Trait("SfButton", "ButtonType")]
        [Fact(Timeout = 10000, DisplayName = "Submit button type")]
        public void SubmitButtonType_IsSubmit()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Type, ButtonType.Submit)
                .Add(component => component.Content, "Submit"));

            var buttonElement = renderedComponent.Find("button");
            Assert.Equal("submit", buttonElement.GetAttribute("type"));
        }

        // Verifies button type is "reset" when Type is set to ButtonType.Reset.
        [Trait("SfButton", "ButtonType")]
        [Fact(Timeout = 10000, DisplayName = "Reset button type")]
        public void ResetButtonType_IsReset()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Type, ButtonType.Reset)
                .Add(component => component.Content, "Reset"));

            var buttonElement = renderedComponent.Find("button");
            Assert.Equal("reset", buttonElement.GetAttribute("type"));
        }

        // Verifies aria-label attribute is rendered as expected.
        [Trait("SfButton", "Other")]
        [Fact(Timeout = 10000, DisplayName = "aria-label")]
        public void aria_label()
        {
            var renderedComponent = RenderComponent<AttributesAndTypes>();
            var buttonElement = renderedComponent.FindAll("button")[0];
            Assert.Equal("Button", buttonElement.GetAttribute("aria-label"));
        }

        // Verifies icon CSS from component parameter appears inside icon element.
        [Trait("SfButton", "Icons")]
        [Fact(Timeout = 10000, DisplayName = "Icon CSS")]
        public void IconCSS()
        {
            string IconCss = "e-icons e-play";
            var renderedComponent = RenderComponent<IconBtn>((nameof(IconBtn.IconCssClass), IconCss));
            var buttonElement = renderedComponent.FindAll("button")[0];
            Assert.Contains(IconCss, buttonElement.Children[0].ClassName) ;
        }

        // Verifies icon position top CSS applied.
        [Trait("SfButton", "Icons")]
        [Fact(Timeout = 10000, DisplayName = "Icon Position Top")]
        public void IconTop()
        {
            var renderedComponent = RenderComponent<ButtonIconPosition>();
            var buttonElement = renderedComponent.FindAll("button")[2];
            Assert.Contains("e-icon-top", buttonElement.Children[0].ClassList);
        }

        // Verifies icon position bottom CSS applied.
        [Trait("SfButton", "Icons")]
        [Fact(Timeout = 10000, DisplayName = "Icon Position Bottom")]
        public void IconBottom()
        {
            var renderedComponent = RenderComponent<ButtonIconPosition>();
            var buttonElement = renderedComponent.FindAll("button")[3];
            Assert.Contains("e-icon-bottom", buttonElement.Children[0].ClassList);
        }

        // Verifies IsToggle toggles aria-pressed and active class when clicked.
        [Fact(DisplayName = "Toggle button updates aria-pressed and e-active class")]
        public void ToggleButton_Click_TogglesAriaPressed()
        {
            // Test Group: API Names
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.IsToggle, true)
                .Add(component => component.Content, "Toggle Me"));

            var buttonElement = renderedComponent.Find("button");
            Assert.Equal("false", buttonElement.GetAttribute("aria-pressed"));

            buttonElement.Click();
            buttonElement = renderedComponent.Find("button");

            Assert.Equal("true", buttonElement.GetAttribute("aria-pressed"));
            Assert.Contains("e-active", buttonElement.ClassList);
        }

        // Verifies IconCss property renders an icon-only button with expected classes.
        [Fact(DisplayName = "Icon-only button adds e-btn-icon modifier and renders span")]
        public void IconOnlyButton_AddsIconBtnClass()
        {
            // Test Group: API Names
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.IconCss, "e-icons e-play"));

            var buttonElement = renderedComponent.Find("button");
            var iconSpan = renderedComponent.Find("span");

            Assert.Contains("e-btn-icon", buttonElement.ClassList);
            Assert.Contains("e-icons", iconSpan.ClassList);
            Assert.Contains("e-btn-icon", iconSpan.ClassList);
        }

        // Verifies default toggle initial active state and toggling behavior.
        [Trait("SfButton", "Others")]
        [Fact(Timeout = 10000, DisplayName = "Default Toggle")]
        public void DefaultToggle()
        {
            var renderedComponent = RenderComponent<DefaultToggleBtn>();
            var buttonElements = renderedComponent.FindAll("button", true);
            buttonElements[0].Click();
            Assert.Equal(true, buttonElements[0].ClassList.Contains("e-active"));
            buttonElements[0].Click();
            Assert.Equal(false, buttonElements[0].ClassList.Contains("e-active"));
        }

        // Verifies rendering of disabled state via DOM disabled attribute.
        [Trait("SfButton", "Others")]
        [Fact(Timeout = 10000, DisplayName = "Default Disable")]
        public void DefaultDisabled()
        {
            var renderedComponent = RenderComponent<DefaultDisabledBtn>();
            var buttonElement = renderedComponent.FindAll("button")[1];
            bool disabled = buttonElement.IsDisabled();
            Assert.True(disabled, "Button element is not disabled");
        }

        // Verifies enabled state of a button element.
        [Trait("SfButton", "Others")]
        [Fact(Timeout = 10000, DisplayName = "Default Enable")]
        public void DefaulEnable()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.FindAll("button")[1];
            bool enabled = buttonElement.IsDisabled();
            Assert.False(enabled, "Button element is not enabled");
        }

        // Verifies RTL CSS class is applied when enableRTL parameter is true.
        [Trait("SfButton", "Others")]
        [Fact(Timeout = 10000, DisplayName = "Enable RTL")]
        public void EnableRTL()
        {
            bool rtl_set = true;
            Services.AddScoped(_ => new SyncfusionBlazorToolkitService(Options.Create(new GlobalOptions { EnableRtl = true })));
            var renderedComponent = RenderComponent<AttributesAndTypes>((nameof(AttributesAndTypes.enableRTL), rtl_set));
            var buttonElement = renderedComponent.FindAll("button")[1];
            Assert.Contains("e-rtl", buttonElement.ClassList);

        }

        [Trait("SfButton", "Type")]
        [Fact(Timeout = 10000, DisplayName = "Is Primary")]
        public void IsPrimary()
        {
            bool primary_set = true;
            var renderedComponent = RenderComponent<DefaultBtn>((nameof(DefaultBtn.Primary),primary_set));
            var buttonElement = renderedComponent.FindAll("button")[1];
            Assert.Contains("e-primary", buttonElement.ClassList);
        }

        // Verifies disabled attribute logic across elements (regression).
        [Trait("SfButton", "Regression")]
        [Fact(DisplayName = "Disabled attribute updated correctly in OnAfterRenderAsync")]
        public void DisabledAttribute()
        {
            var renderedComponent = RenderComponent<IconBtn>();
            var buttonElements = renderedComponent.FindAll("button.e-btn");
            Assert.True(buttonElements[1].IsDisabled());
            Assert.False(buttonElements[2].IsDisabled());
        }

        // Placeholder test for component initialization binding scenario.
        [Trait("SfCheckBox", "InitializationBinding")]
        [Fact(DisplayName = "Disabled property binding works when set in OnInitializedAsync")]
        public void InitializationBinding()
        {
            var renderedComponent = RenderComponent<Regression>();
            var buttonElements = renderedComponent.FindAll(".d-inline");
            Assert.True(buttonElements[0].IsDisabled());
        }
        #endregion

        #region Events

        // Verifies the OnClick EventCallback is triggered when the user clicks the button.
        [Fact(DisplayName = "OnClick EventCallback is raised on user click")]
        public void OnClickCallback_RaisesEvent()
        {
            // Test Group: Events
            var factory = new EventCallbackFactory();
            var clicked = false;

            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Content, "Click")
                .Add(component => component.OnClick, factory.Create<MouseEventArgs>(this, _ => clicked = true)));

            renderedComponent.Find("button").Click();

            Assert.True(clicked);
        }

        // Verifies Created event handling updates UI on initialization.
        [Trait("SfButton", "Events")]
        [Fact(Timeout = 10000, DisplayName = "On Created")]
        public void OnCreated()
        {
           var renderedComponent = RenderComponent<EventButton>();
           var buttonElement = renderedComponent.Find("button");
           var buttonText = buttonElement.TextContent.Trim();
           Assert.Equal("Created", buttonText);
        }

        // Verifies click handler increments and updates displayed click count.
        [Trait("SfButton", "Events")]
        [Fact(Timeout = 10000, DisplayName = "On Clicked")]
        public void OnClick()
        {            
            var renderedComponent = RenderComponent<EventButton>();
            var buttonElements = renderedComponent.FindAll("button", true);
            var buttonText = buttonElements[1].TextContent.Trim();
            Assert.Equal("Click", buttonText);

            // first click
            buttonElements[1].Click();
            buttonText = buttonElements[1].TextContent.Trim();
            Assert.Equal("Clicked 1", buttonText);

            // second click
            buttonElements[1].Click();
            buttonText = buttonElements[1].TextContent.Trim();
            Assert.Equal("Clicked 2", buttonText);
        }

        // Verifies KeyDown event handling reflects key input in UI text.
        [Trait("SfButton", "Events")]
        [Fact(Timeout = 10000, DisplayName = "On KeyDown")]
        public void OnKeyDown()
        {
            var renderedComponent = RenderComponent<EventButton>();
            var buttonElements = renderedComponent.FindAll("button", true);
            var buttonText = buttonElements[2].TextContent.Trim();
            Assert.Equal("KeyDown", buttonText);

            // first click
            buttonElements[2].KeyDown("a");
            buttonText = buttonElements[2].TextContent.Trim();
            Assert.Equal("KeyDown: a", buttonText);
        }

        #endregion

        #region Customization

        // Verifies AdditionalAttributes merge custom class and data attribute into the rendered element.
        [Fact(DisplayName = "AdditionalAttributes merge custom class and data attribute")]
        public void AdditionalAttributes_MergeWithGeneratedClasses()
        {
            // Test Group: Further Customization
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Content, "Custom")
                .AddUnmatched("class", "user-class")
                .AddUnmatched("data-testid", "custom-button"));

            var buttonElement = renderedComponent.Find("[data-testid='custom-button']");

            Assert.Contains("user-class", buttonElement.ClassList);
            Assert.Equal("custom-button", buttonElement.GetAttribute("data-testid"));
        }

        // Verifies style variants map to appropriate CSS classes (Primary).
        [Trait("SfButton", "Style")]
        [Fact(Timeout = 10000, DisplayName = "Primary")]
        public void PrimaryButton()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.FindAll("button")[7];            
            Assert.Contains("e-primary", buttonElement.ClassList);
        }

        // Verifies style variants map to appropriate CSS classes (Success).
        [Trait("SfButton", "Style")]
        [Fact(Timeout = 10000, DisplayName = "Success")]
        public void SuccessButton()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.FindAll("button")[2];            
            Assert.Contains("e-success", buttonElement.ClassList);
        }

        // Verifies style variants map to appropriate CSS classes (Info).
        [Trait("SfButton", "Style")]
        [Fact(Timeout = 10000, DisplayName = "Info")]
        public void InfoButton()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.FindAll("button")[3];            
            Assert.Contains("e-info", buttonElement.ClassList);
        }

        // Verifies style variants map to appropriate CSS classes (Warning).
        [Trait("SfButton", "Style")]
        [Fact(Timeout = 10000, DisplayName = "Warning")]
        public void WarningButton()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.FindAll("button")[9];            
            Assert.Contains("e-warning", buttonElement.ClassList);
        }

        // Verifies style variants map to appropriate CSS classes (Danger).
        [Trait("SfButton", "Style")]
        [Fact(Timeout = 10000, DisplayName = "Danger")]
        public void DangerButton()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.FindAll("button")[5];            
            Assert.Contains("e-danger", buttonElement.ClassList);
        }

        // Verifies style variants map to appropriate CSS classes (Warning Outline).
        [Trait("SfButton", "Type")]
        [Fact(Timeout = 10000, DisplayName = "Warning Outline")]
        public void WarningOutlineButton()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.FindAll("button")[9];            
            Assert.Contains("e-outline", buttonElement.ClassList);
            Assert.Contains("e-warning", buttonElement.ClassList);
        }

        // Verifies round type button has round modifier class.
        [Trait("SfButton", "Type")]
        [Fact(Timeout = 10000, DisplayName = "Round")]
        public void RoundButton()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.FindAll("button")[10];            
            Assert.Contains("e-round", buttonElement.ClassList);
        }

        // Verifies combined primary & round styles.
        [Trait("SfButton", "Type")]
        [Fact(Timeout = 10000, DisplayName = "Primary Round button")]
        public void PrimaryRoundButton()
        {
            var renderedComponent = RenderComponent<DefaultBtn>();
            var buttonElement = renderedComponent.FindAll("button")[11];            
            Assert.Contains("e-round", buttonElement.ClassList);
            Assert.Contains("e-primary", buttonElement.ClassList);
        }

        #endregion

        #region Accessibility Tests

        // Verifies aria-disabled attribute is set correctly when button is disabled.
        [Trait("SfButton", "Accessibility")]
        [Fact(Timeout = 10000, DisplayName = "Aria-disabled attribute for disabled button")]
        public void Accessibility_AriaDisabledAttribute_WhenDisabled()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Disabled, true)
                .Add(component => component.Content, "Disabled"));

            var buttonElement = renderedComponent.Find("button");
            Assert.Equal("true", buttonElement.GetAttribute("aria-disabled"));
        }

        // Verifies aria-disabled attribute is set to false when button is enabled.
        [Trait("SfButton", "Accessibility")]
        [Fact(Timeout = 10000, DisplayName = "Aria-disabled attribute for enabled button")]
        public void Accessibility_AriaDisabledAttribute_WhenEnabled()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Disabled, false)
                .Add(component => component.Content, "Enabled"));

            var buttonElement = renderedComponent.Find("button");
            Assert.Equal("false", buttonElement.GetAttribute("aria-disabled"));
        }

        // Verifies icons have aria-hidden="true" for screen reader accessibility.
        [Trait("SfButton", "Accessibility")]
        [Fact(Timeout = 10000, DisplayName = "Icon has aria-hidden attribute")]
        public void Accessibility_IconHasAriaHidden()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.IconCss, "e-icons e-settings")
                .Add(component => component.Content, "Settings"));

            var iconSpan = renderedComponent.Find("span.e-icons");
            Assert.Equal("true", iconSpan.GetAttribute("aria-hidden"));
        }

        // Verifies DEBUG time warning is emitted for icon-only button without accessible name.
        [Trait("SfButton", "Accessibility")]
        [Fact(Timeout = 10000, DisplayName = "Icon-only without accessible name emits debug warning")]
        public void Accessibility_IconOnly_NoAccessibleName_DebugWarning()
        {
#if DEBUG
            var sw = new StringWriter();
            var originalListeners = Trace.Listeners;
            var textWriterListener = new TextWriterTraceListener(sw);
            try
            {
                Trace.Listeners.Add(textWriterListener);
                var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                    .Add(component => component.IconCss, "e-icons e-warning")
                );
                // OnParametersSet runs during render; capture Debug output
                Trace.Flush();
            }
            finally
            {
                Trace.Listeners.Remove(textWriterListener);
                Trace.Listeners.AddRange(originalListeners);
            }

            var output = sw.ToString();
            Assert.Contains("Warning: Icon-only button", output);
#else
            // No-op in Release builds - accessibility check is conditional on DEBUG
            Assert.True(true);
#endif
        }

        // Verifies non-toggle button does not have aria-pressed attribute.
        [Trait("SfButton", "Accessibility")]
        [Fact(Timeout = 10000, DisplayName = "Non-toggle button without aria-pressed")]
        public void Accessibility_NonToggleButton_NoAriaPressed()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.IsToggle, false)
                .Add(component => component.Content, "Normal Button"));

            var buttonElement = renderedComponent.Find("button");
            Assert.Null(buttonElement.GetAttribute("aria-pressed"));
        }

        // Verifies toggle button cycles through multiple states correctly.
        [Trait("SfButton", "Accessibility")]
        [Fact(Timeout = 10000, DisplayName = "Toggle button multiple state changes")]
        public void Accessibility_ToggleButton_MultipleStateChanges()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.IsToggle, true)
                .Add(component => component.Content, "Multi-Toggle"));

            var buttonElement = renderedComponent.Find("button");

            // Initial state - not active
            Assert.Equal("false", buttonElement.GetAttribute("aria-pressed"));
            Assert.DoesNotContain("e-active", buttonElement.ClassList);

            // First click - activate
            buttonElement.Click();
            buttonElement = renderedComponent.Find("button");
            Assert.Equal("true", buttonElement.GetAttribute("aria-pressed"));
            Assert.Contains("e-active", buttonElement.ClassList);

            // Second click - deactivate
            buttonElement.Click();
            buttonElement = renderedComponent.Find("button");
            Assert.Equal("false", buttonElement.GetAttribute("aria-pressed"));
            Assert.DoesNotContain("e-active", buttonElement.ClassList);

            // Third click - activate again
            buttonElement.Click();
            buttonElement = renderedComponent.Find("button");
            Assert.Equal("true", buttonElement.GetAttribute("aria-pressed"));
            Assert.Contains("e-active", buttonElement.ClassList);
        }

        #endregion

        #region Content Rendering Tests

        // ChildContent should render when both ChildContent and Content are provided
        [Trait("SfButton", "Content")]
        [Fact(Timeout = 10000,  DisplayName = "Content parameter renders when ChildContent is null")]
        public void ContentRendering_ContentRendersWhenChildContentIsNull()
        {
            // Arrange
            var renderedComponent = RenderComponent<SfButton>(parameters => parameters
                .Add(p => p.Content, "Content Parameter")
            // No ChildContent added
            );

            // Act
            var buttonElement = renderedComponent.Find("button");

            // Assert
            Assert.Equal("Content Parameter", buttonElement.TextContent);
        }
        [Fact(Timeout = 10000, DisplayName = "ChildContent overrides Content parameter when both provided")]
        public void ContentRendering_ChildContentOverridesContent()
        {
            // Arrange
            var renderedComponent = RenderComponent<SfButton>(parameters => parameters
                .Add(p => p.Content, "Content Parameter")
                .AddChildContent("<span>Child Content</span>")
            );

            // Act
            var buttonElement = renderedComponent.Find("button");

            // Assert
            // ChildContent is rendered
            Assert.Contains("Child Content", buttonElement.InnerHtml);
            // Content is not rendered
            Assert.DoesNotContain("Content Parameter", buttonElement.TextContent);
        }

        // Verifies button renders with empty content when both Content and ChildContent are null.
        [Trait("SfButton", "Content")]
        [Fact(Timeout = 10000, DisplayName = "Empty content when no Content or ChildContent")]
        public void ContentRendering_EmptyContentWhenBothNull()
        {
            var renderedComponent = RenderComponent<SfButton>();
            
            var buttonElement = renderedComponent.Find("button");
            var textContent = buttonElement.TextContent.Trim();
            Assert.Equal("", textContent);
        }
        #endregion

        #region Icon Position Tests

        // Verifies icon appears before content when IconPosition is Left.
        [Trait("SfButton", "Icons")]
        [Fact(Timeout = 10000, DisplayName = "Icon position left - icon before content")]
        public void IconPosition_Left_IconBeforeContent()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Content, "Save")
                .Add(component => component.IconCss, "e-icons e-save")
                .Add(component => component.IconPosition, IconPosition.Left));

            var buttonElement = renderedComponent.Find("button");
            var iconIndex = buttonElement.InnerHtml.IndexOf("<span");
            var textIndex = buttonElement.InnerHtml.IndexOf("Save");
            Assert.True(iconIndex < textIndex, "Icon should appear before content");
        }

        // Verifies icon appears after content when IconPosition is Right.
        [Trait("SfButton", "Icons")]
        [Fact(Timeout = 10000, DisplayName = "Icon position right - icon after content")]
        public void IconPosition_Right_IconAfterContent()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Content, "Next")
                .Add(component => component.IconCss, "e-icons e-arrow-right")
                .Add(component => component.IconPosition, IconPosition.Right));

            var buttonElement = renderedComponent.Find("button");
            var iconIndex = buttonElement.InnerHtml.IndexOf("<span");
            var textIndex = buttonElement.InnerHtml.IndexOf("Next");
            Assert.True(iconIndex > textIndex, "Icon should appear after content");
        }

        // Verifies icon has correct CSS class for Top position.
        [Trait("SfButton", "Icons")]
        [Fact(Timeout = 10000, DisplayName = "Icon position top - has correct CSS class")]
        public void IconPosition_Top_HasCorrectCssClass()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Content, "Upload")
                .Add(component => component.IconCss, "e-icons e-upload")
                .Add(component => component.IconPosition, IconPosition.Top));

            var iconSpan = renderedComponent.Find("span.e-icons");
            Assert.Contains("e-icon-top", iconSpan.ClassList);
        }

        // Verifies icon has correct CSS class for Bottom position.
        [Trait("SfButton", "Icons")]
        [Fact(Timeout = 10000, DisplayName = "Icon position bottom - has correct CSS class")]
        public void IconPosition_Bottom_HasCorrectCssClass()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Content, "Download")
                .Add(component => component.IconCss, "e-icons e-download")
                .Add(component => component.IconPosition, IconPosition.Bottom));

            var iconSpan = renderedComponent.Find("span.e-icons");
            Assert.Contains("e-icon-bottom", iconSpan.ClassList);
        }

        #endregion

        #region Disabled State Tests

        // Verifies click event does not fire when button is disabled.
        [Trait("SfButton", "Others")]
        [Fact(Timeout = 10000, DisplayName = "Disabled button does not invoke OnClick")]
        public void DisabledState_DoesNotInvokeOnClick()
        {
            var factory = new EventCallbackFactory();
            var clickCount = 0;

            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Disabled, true)
                .Add(component => component.Content, "Disabled")
                .Add(component => component.OnClick, factory.Create<MouseEventArgs>(this, _ => clickCount++)));

            var buttonElement = renderedComponent.Find("button");
            buttonElement.Click();

            Assert.Equal(0, clickCount);
        }

        // Verifies disabled attribute is present on button element when Disabled is true.
        [Trait("SfButton", "Others")]
        [Fact(Timeout = 10000, DisplayName = "Disabled attribute present when Disabled=true")]
        public void DisabledState_HasDisabledAttribute()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Disabled, true));

            var buttonElement = renderedComponent.Find("button");
            Assert.True(buttonElement.HasAttribute("disabled"));
        }

        // Verifies disabled attribute is not present when Disabled is false.
        [Trait("SfButton", "Others")]
        [Fact(Timeout = 10000, DisplayName = "Disabled attribute absent when Disabled=false")]
        public void DisabledState_NoDisabledAttribute_WhenEnabled()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Disabled, false));

            var buttonElement = renderedComponent.Find("button");
            Assert.False(buttonElement.HasAttribute("disabled"));
        }

        #endregion

        #region Parameter Update Tests

        // Verifies Content parameter update re-renders button text.
        [Trait("SfButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName = "Content parameter update")]
        public void ParameterUpdate_Content_UpdatesButtonText()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.Content, "Initial"));

            var buttonElement = renderedComponent.Find("button");
            Assert.Equal("Initial", buttonElement.TextContent);

            renderedComponent.SetParametersAndRender(componentParameters => componentParameters
                .Add(component => component.Content, "Updated"));

            buttonElement = renderedComponent.Find("button");
            Assert.Equal("Updated", buttonElement.TextContent);
        }

        // Verifies icon visibility changes when IconCss parameter changes from value to empty.
        [Trait("SfButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName = "IconCss clear removes icon")]
        public void ParameterUpdate_IconCss_RemovesIconWhenCleared()
        {
            var renderedComponent = RenderComponent<SfButton>(componentParameters => componentParameters
                .Add(component => component.IconCss, "e-icons e-save")
                .Add(component => component.Content, "Save"));

            // Verify icon exists initially
            var iconSpans = renderedComponent.FindAll("span.e-icons");
            Assert.NotEmpty(iconSpans);

            // Clear IconCss
            renderedComponent.SetParametersAndRender(componentParameters => componentParameters
                .Add(component => component.IconCss, string.Empty)
                .Add(component => component.Content, "Save"));

            // Verify icon is removed
            iconSpans = renderedComponent.FindAll("span.e-icons");
            Assert.Empty(iconSpans);
        }

        #endregion

        #region Property Changes

        // Verifies runtime property change to Primary updates class list on re-render.
        [Trait("SfButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName = "IsPrimary")]
        public void PropertyChange_IsPrimary()
        {
            var renderedComponent = RenderComponent<DefaultBtn>((nameof(DefaultBtn.Primary),true));
            renderedComponent.SetParametersAndRender((nameof(DefaultBtn.Primary), false));
            var buttonElement = renderedComponent.FindAll("button")[1];
            Assert.False(buttonElement.ClassList.Contains("e-primary"));
        }

        // Verifies changing CssClass parameter updates DOM classes on re-render (single class case).
        [Trait("SfButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName = "CssClass for Single class")]
        public void PropertyChange_CssClass_Single()
        {
            var renderedComponent = RenderComponent<DefaultBtn>((nameof(DefaultBtn.ClassName), "e-warning"));
            renderedComponent.SetParametersAndRender((nameof(DefaultBtn.ClassName), "e-danger"));
            var buttonElement = renderedComponent.FindAll("button")[4];
            Assert.Contains("e-danger", buttonElement.ClassList);
        }

        // Verifies changing CssClass parameter updates DOM classes on re-render (multiple classes).
        [Trait("SfButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName = "CssClass for multiple class")]
        public void PropertyChange_CssClass_multiple()
        {
            var renderedComponent = RenderComponent<DefaultBtn>((nameof(DefaultBtn.ClassName), "e-outline e-info"));
            renderedComponent.SetParametersAndRender((nameof(DefaultBtn.ClassName), "e-flat e-warning"));
            var buttonElement = renderedComponent.FindAll("button")[8];
            Assert.Contains("e-flat", buttonElement.ClassList);
            Assert.Contains("e-warning", buttonElement.ClassList);
        }

        // Verifies updating IconCssClass parameter reflects in DOM on re-render.
        [Trait("SfButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName = "Icon CSS")]
        public void PropertyChange_IconCSS()
        {
            var renderedComponent = RenderComponent<IconBtn>((nameof(IconBtn.IconCssClass), "e-icons e-play"));
            renderedComponent.SetParametersAndRender((nameof(IconBtn.IconCssClass), "e-icons e-pause"));
            var buttonElement = renderedComponent.FindAll("button")[0];
            Assert.Contains("e-pause", buttonElement.Children[0].ClassName);
        }

        // Verifies updating icon position parameter updates CSS class on re-render.
        [Trait("SfButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName = "ButtonIconPosition - Right")]
        public void PropertyChange_IconRight()
        {
            var renderedComponent = RenderComponent<ButtonIconPosition>((nameof(ButtonIconPosition.IconcssPosition),IconPosition.Left));
            renderedComponent.SetParametersAndRender((nameof(ButtonIconPosition.IconcssPosition),IconPosition.Right));
            var buttonElement = renderedComponent.FindAll("button")[0];

            Assert.Contains("e-icon-right", buttonElement.Children[0].ClassName);
        }

        // Verifies disabling via parameter change updates disabled attribute on element.
        [Trait("SfButton", "Property Changes")]
        [Fact(Timeout = 10000, DisplayName = "Default Disable")]
        public void PropertyChange_DefaultDisabled()
        {
            var renderedComponent = RenderComponent<DefaultDisabledBtn>((nameof(DefaultDisabledBtn.Disabled),true));
            renderedComponent.SetParametersAndRender((nameof(DefaultDisabledBtn.Disabled), false));
            var buttonElement = renderedComponent.FindAll("button")[0];
            bool disabled = buttonElement.IsDisabled();
            Assert.False(disabled, "Button element is in disabled state");
        }

        // Verifies setting aria-label attribute on DOM element at runtime.
        [Trait("SfButton", "PropertyChanges")]
        [Fact(Timeout = 10000, DisplayName = "aria-label for single value")]
        public void PropertyChange_aria_label_SingleValue()
        {
            var renderedComponent = RenderComponent<AttributesAndTypes>();
            var buttonElement = renderedComponent.FindAll("button")[0];
            buttonElement.SetAttribute("aria-label", "Syncfusion");
            Assert.Equal("Syncfusion", buttonElement.GetAttribute("aria-label"));
        }

        // Verifies setting title and type attributes on DOM element at runtime.
        [Trait("SfButton", "PropertyChanges")]
        [Fact(Timeout = 10000, DisplayName = "title for multiple value")]
        public void PropertyChange_title_MultipleValue()
        {
            var renderedComponent = RenderComponent<AttributesAndTypes>();
            var buttonElement = renderedComponent.FindAll("button")[0];
            buttonElement.SetAttribute("type", "button");
            buttonElement.SetAttribute("title", "Blazor Button");
            Assert.Equal("button", buttonElement.GetAttribute("type"));
            Assert.Equal("Blazor Button", buttonElement.GetAttribute("title"));
        }

        // Verifies toggling RTL parameter updates CSS on re-render.
        [Trait("SfButton", "PropertyChanges")]
        [Fact(Timeout = 10000, DisplayName = "Enable RTL")]
        public void PropertyChanges_EnableRTL()
        {
            var renderedComponent = RenderComponent<AttributesAndTypes>((nameof(AttributesAndTypes.enableRTL), true));
            renderedComponent.SetParametersAndRender((nameof(AttributesAndTypes.enableRTL), false));
            var buttonElement = renderedComponent.FindAll("button")[1];
            Assert.False(buttonElement.ClassList.Contains("e-rtl"));
        }

        #endregion
    }
}
