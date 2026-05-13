using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// The SfTooltip component displays a tooltip that appears as a pop-up containing information or a message when you hover, click, focus, or touch an element.
    /// </summary>
    /// <remarks>
    /// The SfTooltip component can be customized with various properties such as content, position, and animation. 
    /// The Tooltip can be triggered by various events including hover, click and focus. 
    /// The component can be used to provide additional context or information about an element on a web page.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfTooltip Content="Let's go green to save the planet!!">
    ///  <SfButton Content="Show Tooltip"></SfButton>
    /// </SfTooltip>
    /// ]]></code>
    ///</example>
    public partial class SfTooltip
    {
        #region Public Parameters properties

        /// <summary>
        /// Gets or sets the child content to be rendered inside the tooltip's trigger element.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> representing the component's child content. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// When no <see cref="Target"/> selector is provided, the element(s) rendered by <see cref="ChildContent"/> act as the tooltip trigger.
        /// Use the <see cref="Content"/> or <see cref="ContentTemplate"/> properties to define the tooltip's displayed content.
        /// </remarks>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <exclude/>
        /// <summary>
        /// Specifies the Id of the Tooltip component.
        /// </summary>
        [Parameter]
        public string ID { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the template that defines the content of the component.
        /// </summary>
        /// <value>
        /// The default value is `null`.
        /// </value>
        /// <remarks>
        /// The ContentTemplate is a RenderFragment that allows users to customize the appearance and content of the component. 
        /// This property can be used to define custom HTML or Razor markup to be rendered as the content of the component.
        /// </remarks>
        [Parameter]
        public RenderFragment ContentTemplate { get; set; } = null!;

        /// <summary>
        /// Gets or sets the animation settings for the opening and closing of the Tooltip.
        /// </summary>
        /// <remarks>
        /// The animation property allows you to customize the animation of the Tooltip component, including the delay, duration, and various other effects of your choice. 
        /// You can set the same or different animation options to the Tooltip when it is in the open or close state.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!" Animation="@Animation">
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        ///@code {
                ///  public AnimationModel Animation { get; set; } = new AnimationModel
                /// {
                ///    Open = new TooltipAnimationSettings {Delay = 0, Duration = 500, Effect = Effect.ZoomIn },
                ///    Close = new TooltipAnimationSettings{Delay=0,Duration=500,Effect=Effect.ZoomOut}
                /// };
        /// }
        /// ]]></code>
        ///</example>
        [Parameter]
        public AnimationModel Animation { get; set; } = new AnimationModel
        {
            Open = new TooltipAnimationSettings { Delay = 0, Duration = 150, Effect = Effect.FadeIn },
            Close = new TooltipAnimationSettings { Delay = 0, Duration = 150, Effect = Effect.FadeOut },
        };

        /// <summary>
        /// Gets or sets the delay in milliseconds before the Tooltip closes.
        /// </summary>
        /// <remarks>
        /// The CloseDelay property is used to specify the delay in milliseconds before the Tooltip closes. If no delay is needed, the default value of 0 can be used.
        /// </remarks>
        /// <value>
        /// Accepts a double value representing the delay in milliseconds. The default value is 0.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!" CloseDelay="2000">
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// ]]></code>
        ///</example>
        [Parameter]
        [JsonPropertyName("closeDelay")]
        public double CloseDelay { get; set; } = 0;

        /// <summary>
        /// Gets or sets the content of the Tooltip component.
        /// </summary>
        /// <remarks>
        /// The Content property is used to specify the content of the Tooltip component as a string element.
        /// <para>
        /// <strong>Security Note:</strong> The content is rendered as plain text by default. If you need to render HTML content,
        /// use the <see cref="ContentTemplate"/> property instead. When using ContentTemplate with user-provided content,
        /// ensure that the HTML is properly sanitized to prevent Cross-Site Scripting (XSS) attacks.
        /// Do not directly render untrusted user input as HTML.
        /// </para>
        /// <para>
        /// For dynamic HTML content, consider using Razor markup within the ContentTemplate to benefit from
        /// automatic HTML encoding provided by Blazor.
        /// </para>
        /// </remarks>
        /// <value>
        /// Accepts a string value representing the content of the Tooltip component. The default value is <see cref="string.Empty"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!">
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// ]]></code>
        ///</example>
        [Parameter]
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CSS classes to be applied to the Tooltip component.
        /// </summary>
        /// <remarks>
        /// The CssClass property is used to apply custom CSS class names that define specific user-defined styles and themes to be applied to the Tooltip element. Multiple class names can be specified by separating them with a space.
        /// </remarks>
        /// <value>
        /// Accepts a CSS class string separated by space to customize the appearance of the component.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!" CssClass="customtip">
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// ]]></code>
        ///</example>
        [Parameter]
        [JsonPropertyName("cssClass")]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether to set the collision target element as the page viewport (window) or the Tooltip element, when using the target.
        /// </summary>
        /// <remarks>
        /// The WindowCollision property is used to enable or disable the collision calculation between the target elements and viewport (window) instead of the Tooltip element. 
        /// If the value is set to <c>true</c>, the collision will be calculated between the target and the viewport (window). Otherwise, the collision will be calculated between the target and the Tooltip element.
        /// </remarks>
        /// <value>
        /// <c>true</c>, if the window collision can be enabled; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        [Parameter]
        [JsonPropertyName("WindowCollision")]
        public bool WindowCollision { get; set; } = false;

        /// <summary>
        /// Gets or sets the height of the Tooltip component.
        /// </summary>
        /// <remarks>
        /// The Height property is used to specify the height of the Tooltip component. 
        /// If no height is specified, the Tooltip height will be set based on its content.
        /// When the content of the Tooltip exceeds the height value, the scroll mode will be enabled. 
        /// The value can be any valid CSS height value, such as "100px" or "50%".
        /// </remarks>
        /// <value>
        /// Accepts the string value. The default value is <c>auto</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!" Height="40px">
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// ]]></code>
        ///</example>
        [Parameter]
        [JsonPropertyName("height")]
        public string Height { get; set; } = "auto";

        /// <summary>
        /// Gets or sets a value indicating whether the Tooltip should be displayed in an open state until it is closed manually.
        /// </summary>
        /// <remarks>
        /// The IsSticky property is used to set the behavior of the Tooltip when it is displayed.
        /// If the value is set to true, the Tooltip will be displayed in an open state until it is closed manually, regardless of the user interaction that triggered the Tooltip. 
        /// If the value is set to false, the Tooltip will be displayed for a specified duration based on the Animation property and then automatically closed.
        ///</remarks>
        /// <value>
        /// <c>true</c>, if the Tooltip should be displayed in an open state until it is closed manually. Otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        [Parameter]
        [JsonPropertyName("isSticky")]
        public bool IsSticky { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Tooltip should follow the mouse pointer as it moves over the specified target element.
        /// </summary>
        /// <value>
        /// <c>true</c>, if the Tooltip should follow the mouse pointer when it moves over the specific target. Otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// The MouseTrail property enables the Tooltip to dynamically follow the mouse cursor position when the user moves the mouse over the target element.
        /// When enabled, the Tooltip position will continuously update based on the mouse coordinates, creating a trailing effect.
        /// </remarks>
        [Parameter]
        [JsonPropertyName("mouseTrail")]
        public bool MouseTrail { get; set; }

        /// <summary>
        /// Gets or sets the X-axis offset between the target and Tooltip element.
        /// </summary>
        /// <value>
        /// A double value representing the space between the target and Tooltip element in X-axis. The default value is 0.
        /// </value>
        /// <remarks>
        /// The OffsetX property allows you to adjust the horizontal position of the Tooltip relative to its target element.
        /// Positive values move the Tooltip to the right, while negative values move it to the left.
        /// This offset is applied in addition to the base position determined by the Position property.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!" OffsetX="20">
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// ]]></code>
        ///</example>
        [Parameter]
        [JsonPropertyName("offsetX")]
        public double OffsetX { get; set; }

        /// <summary>
        /// Gets or sets the Y-axis offset between the target and Tooltip element.
        /// </summary>
        /// <value>
        /// Accepts a double value representing the space between the target and Tooltip element in the Y-axis. The default value is 0.
        /// </value>
        /// <remarks>
        /// The OffsetY property allows you to adjust the vertical position of the Tooltip relative to its target element.
        /// Positive values move the Tooltip downward, while negative values move it upward.
        /// This offset is applied in addition to the base position determined by the Position property.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!" OffsetY="70">
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// ]]></code>
        ///</example>
        [Parameter]
        [JsonPropertyName("offsetY")]
        public double OffsetY { get; set; }

        /// <summary>
        /// Gets or sets the delay time in milliseconds for opening the Tooltip after the user hovers over the target element.
        /// </summary>
        /// <value>
        /// A double value representing the delay time in milliseconds. The default value is 0, which means that the Tooltip opens immediately after the user hovers over the target element.
        /// </value>
        /// <remarks>
        /// The OpenDelay property is useful for preventing the Tooltip from appearing too quickly when users move their mouse over elements.
        /// By setting a delay, you can improve the user experience by avoiding tooltips that flash or appear unintentionally during quick mouse movements.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!" OpenDelay="2000">
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// ]]></code>
        ///</example>
        [Parameter]
        [JsonPropertyName("openDelay")]
        public double OpenDelay { get; set; }

        /// <summary>
        /// Gets or sets the type of open mode to display the Tooltip content.
        /// The available open modes are Auto, Hover, Click, Focus, and Custom.
        /// </summary>
        /// <value>
        /// Accepts a string value that specifies when the Tooltip should be displayed. The default value is <c>Auto</c>.
        /// </value>
        /// <remarks>
        /// The OpensOn property determines the event that triggers the Tooltip to appear. The supported values are:
        /// <list type="bullet">
        /// <item><description><c>Auto</c>: Automatically detects the appropriate trigger event based on the target element</description></item>
        /// <item><description><c>Hover</c>: Tooltip appears when the mouse hovers over the target element</description></item>
        /// <item><description><c>Click</c>: Tooltip appears when the target element is clicked</description></item>
        /// <item><description><c>Focus</c>: Tooltip appears when the target element receives focus</description></item>
        /// <item><description><c>Custom</c>: Tooltip is triggered programmatically using custom events</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!" OpensOn="Click">
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// ]]></code>
        ///</example>
        [Parameter]
        [JsonPropertyName("opensOn")]
        public string OpensOn { get; set; } = "Auto";

        /// <summary>
        /// Gets or sets the position of the Tooltip element with respect to the Target element.
        /// </summary>
        /// <value>
        /// A value of the <see cref="Position" /> enumeration that specifies the position of Tooltip element. The default value is <see cref="Position.TopCenter" />.
        /// </value>
        /// <remarks>
        /// The Position property determines where the Tooltip will appear relative to its target element.
        /// The component automatically adjusts the position if there isn't enough space in the specified direction,
        /// ensuring the Tooltip remains visible within the viewport boundaries.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!" Position=Position.LeftBottom>
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// ]]></code>
        ///</example>
        [Parameter]
        [JsonPropertyName("position")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Position Position { get; set; } = Position.TopCenter;

        /// <summary>
        /// Gets or sets a value indicating whether to show or hide the tip pointer of the Tooltip.
        /// </summary>
        /// <value>
        /// <c>true</c>, if the tip pointer should be shown. Otherwise, <c>false</c>. The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// The ShowTipPointer property controls the visibility of the small arrow or pointer that connects the Tooltip to its target element.
        /// The tip pointer provides a visual connection between the Tooltip and the element that triggered it, improving the user experience.
        /// When set to false, the Tooltip will appear without the pointing arrow.
        /// </remarks>
        [Parameter]
        [JsonPropertyName("showTipPointer")]
        public bool ShowTipPointer { get; set; } = true;

        /// <summary>
        /// Gets or sets the target selector where the Tooltip needs to be displayed.
        /// </summary>
        /// <value>
        /// A string value representing the target selector. The target element is considered as the parent container. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// The Target property specifies the CSS selector for the element(s) that should trigger the Tooltip.
        /// If not specified, the Tooltip will be applied to its immediate child content.
        /// You can use any valid CSS selector such as element names, class names, IDs, or attribute selectors.
        /// <para>
        /// <strong>Security Note:</strong> Ensure that the CSS selector string provided is from a trusted source.
        /// Invalid or malformed selectors may cause JavaScript errors. When accepting user input for selectors,
        /// validate and sanitize the input to prevent potential security issues.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!" Target="#btn" >
        ///  <SfButton ID="btn" Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// ]]></code>
        ///</example>
        [Parameter]
        [JsonPropertyName("target")]
        public string Target { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CSS selector for a container where target elements will automatically have tooltips applied.
        /// </summary>
        /// <value>
        /// A string representing a CSS selector for the container.
        /// The default value is <c>null</c>. If the target elements reside within a specific container, provide its selector as the value; otherwise, tooltip element will be considered as target container.
        /// </value>
        /// <remarks>
        /// Use this property to attach tooltips to multiple target elements within a specific container or across the entire body element.
        /// It facilitates the automatic binding of tooltips to newly added elements matching the specified target selector within the defined container.
        /// <para>
        /// <strong>Security Note:</strong> Ensure that the CSS selector is from a trusted source. Validate selector strings
        /// when accepting user input to prevent potential DOM manipulation issues.
        /// </para>
        /// </remarks>
        /// <value>
        /// A string value representing a valid CSS selector. The default value is <see cref="string.Empty"/>.
        /// </value>
        [Parameter]
        [JsonPropertyName("targetContainer")]
        public string TargetContainer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the container element in which the Tooltip's pop-up will be appended.
        /// </summary>
        /// <value>
        /// A string value that represents the container element selector. The default value is <c>body</c>.
        /// </value>
        /// <remarks>
        /// The Container property specifies where the Tooltip element should be rendered in the DOM.
        /// By default, tooltips are appended to the document body, but you can specify a different container
        /// using a CSS selector. This is useful for controlling the stacking context and ensuring proper positioning
        /// within specific layout containers.
        /// <para>
        /// <strong>Security Note:</strong> The container selector should be from a trusted source.
        /// When using user-provided selectors, validate that they conform to expected patterns (e.g., class names, IDs)
        /// to prevent potential security issues.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <div class="parent">
        /// <SfTooltip Content="Let's go green to save the planet!!" Container=".parent">
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// </div>
        /// ]]></code>
        ///</example>
        [Parameter]
        [JsonPropertyName("container")]
        public string Container { get; set; } = "body";

        /// <summary>
        /// Gets or sets the position of the tip pointer on the tooltip. The available options are Auto, Start, Middle, and End.
        /// When set to auto, the tip pointer gets auto adjusted within the space of the target's length and does not point outside.
        /// </summary>
        /// <value>
        /// A value of the <see cref="TipPointerPosition" /> enumeration that specifies the position of the tip pointer. The default value is <see cref="TipPointerPosition.Auto" />.
        /// </value>
        /// <remarks>
        /// The TipPointerPosition property controls where the tip pointer is positioned along the edge of the Tooltip.
        /// When set to Auto, the component automatically adjusts the pointer position to ensure it points toward the target element
        /// and remains within the bounds of the Tooltip. Manual positioning options (Start, Middle, End) allow for precise control
        /// over the pointer placement regardless of the target element's position.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!" TipPointerPosition=TipPointerPosition.End>
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// ]]></code>
        ///</example>
        [Parameter]
        [JsonPropertyName("tipPointerPosition")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipPointerPosition TipPointerPosition { get; set; } = TipPointerPosition.Auto;

        /// <summary>
        /// Gets or sets the width of the Tooltip component which accepts a string value.
        /// </summary>
        /// <remarks>
        /// When set to auto, the Tooltip width gets auto adjusted to display its content within the viewable screen.
        ///</remarks>
        /// <value>
        /// Accepts the string value.The default value is <c>auto</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!" Width="100px">
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// ]]></code>
        ///</example>
        [Parameter]
        [JsonPropertyName("width")]
        public string Width { get; set; } = "auto";

        /// <summary> 
        /// Gets or sets a collection of additional attributes that will applied to the tooltip element. 
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary{TKey, TValue}"/> of string keys and object values representing HTML attributes. The default value is <c>null</c>.
        /// When <c>null</c>, no additional attributes are applied to the tooltip element.
        /// </value>
        /// <remarks> 
        /// Additional attributes can be added by specifying as inline attributes or by specifying <c>@attributes</c> directive. 
        /// </remarks> 
        /// <example> 
        /// In the below code example, tooltip width has been specified as style attribute in <see cref="SfTooltip"/> tag directive. 
        /// <code><![CDATA[
        /// <SfTooltip Content="Let's go green to save the planet!!" style="width:100px">
        ///  <SfButton Content="Show Tooltip"></SfButton>
        /// </SfTooltip>
        /// ]]></code>
        /// </example>
        [Parameter(CaptureUnmatchedValues = true)]
        [JsonPropertyName("htmlAttributes")]
        public Dictionary<string, object> HtmlAttributes { get; set; }
        #endregion
    }
}
