using Microsoft.JSInterop;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// Animation options that are common for both open and close actions of the Tooltip.
    /// </summary>
    /// <remarks>
    /// This class provides configuration options for controlling the timing and visual effects of Tooltip animations during both opening and closing transitions.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// var animationSettings = new TooltipAnimationSettings
    /// {
    ///     Delay = 100,
    ///     Duration = 300,
    ///     Effect = Effect.FadeIn
    /// };
    /// ]]></code>
    /// </example>
    public class TooltipAnimationSettings
    {
        /// <summary>
        /// Gets or sets the delay value in milliseconds indicating the waiting time before the animation begins.
        /// </summary>
        /// <value>
        /// A <c>double</c> value representing the delay in milliseconds. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property specifies how long to wait before starting the Tooltip animation. A higher value will create a longer pause before the animation effect begins.
        /// </remarks>
        [JsonPropertyName("delay")]
        public double? Delay { get; set; }

        /// <summary>
        /// Gets or sets the duration of the animation that is completed per animation cycle.
        /// </summary>
        /// <value>
        /// A <c>double</c> value representing the animation duration in milliseconds. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property controls how long the Tooltip animation takes to complete one full cycle. Shorter durations create faster animations, while longer durations create slower, more gradual animations.
        /// </remarks>
        [JsonPropertyName("duration")]
        public double? Duration { get; set; }

        /// <summary>
        /// Gets or sets the animation effect applied to the Tooltip during open and close actions.
        /// </summary>
        /// <value>
        /// An <see cref="Effect"/> enumeration value that specifies the type of animation effect to apply.
        /// </value>
        /// <remarks>
        /// This property determines the visual transition effect used when showing or hiding the Tooltip. The available effects include fade, slide, zoom, and other predefined animation types.
        /// </remarks>
        [JsonPropertyName("effect")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Effect Effect { get; set; }
    }

    /// <summary>
    /// Provides data for Tooltip-related events and contains properties that describe the event context and state.
    /// </summary>
    /// <remarks>
    /// This class serves as the base event argument class for all Tooltip events, providing common properties such as cancellation support, position information, and target element details.
    /// Event handlers can use these properties to access information about the Tooltip state and modify the behavior as needed.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// private void OnTooltipBeforeOpen(TooltipEventArgs args)
    /// {
    ///     if (args.Target != null && args.HasText)
    ///     {
    ///         // Custom logic for tooltip positioning
    ///         args.Left = 100;
    ///         args.Top = 50;
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public class TooltipEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the current action should be cancelled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current action needs to be cancelled; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Setting this property to <c>true</c> in event handlers will prevent the default Tooltip behavior from executing, allowing for custom handling of the event.
        /// </remarks>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets the adjusted position of the Tooltip after collision detection has been applied.
        /// </summary>
        /// <value>
        /// A <c>string</c> value that specifies the final position of the Tooltip element after collision handling. The default value corresponds to <see cref="Position.TopCenter"/>.
        /// </value>
        /// <remarks>
        /// This property indicates the position where the Tooltip will be displayed after the collision detection algorithm has determined the best placement to keep the Tooltip within the viewport boundaries.
        /// </remarks>
        [JsonPropertyName("collidedPosition")]
        public string CollidedPosition { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current event object that triggered the Tooltip event.
        /// </summary>
        /// <value>
        /// An <see cref="EventArgs"/> object representing the original event that caused the Tooltip event to be raised. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property provides access to the underlying DOM event information, such as mouse events or keyboard events, that initiated the Tooltip action.
        /// </remarks>
        [JsonPropertyName("event")]
        public EventArgs? Event { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Tooltip content contains text characters.
        /// </summary>
        /// <value>
        /// <c>true</c> if the Tooltip content contains text characters; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property is used to determine if the Tooltip has meaningful text content to display. It helps in deciding whether to show the Tooltip or skip it when the content is empty.
        /// </remarks>
        [JsonPropertyName("hasText")]
        public bool HasText { get; set; }

        /// <summary>
        /// Gets or sets the vertical position (Y-coordinate) where the Tooltip should be displayed.
        /// </summary>
        /// <value>
        /// A <c>double</c> value representing the top position in pixels relative to the viewport. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property specifies the clientY position for positioning the Tooltip. The value represents the distance from the top edge of the viewport to the desired Tooltip position.
        /// </remarks>
        [JsonPropertyName("top")]
        public double? Top { get; set; }

        /// <summary>
        /// Gets or sets the horizontal position (X-coordinate) where the Tooltip should be displayed.
        /// </summary>
        /// <value>
        /// A <c>double</c> value representing the left position in pixels relative to the viewport. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property specifies the clientX position for positioning the Tooltip. The value represents the distance from the left edge of the viewport to the desired Tooltip position.
        /// </remarks>
        [JsonPropertyName("left")]
        public double? Left { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event was triggered by user interaction.
        /// </summary>
        /// <value>
        /// <c>true</c> if the event was triggered by user interaction; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property helps distinguish between events triggered by direct user actions (such as mouse hover or click) and events triggered programmatically through code.
        /// </remarks>
        [JsonPropertyName("isInteracted")]
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Gets or sets the type of event that triggered the Tooltip action.
        /// </summary>
        /// <value>
        /// A <c>string</c> value representing the event type. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property indicates the specific type of user interaction or event that caused the Tooltip to be triggered.
        /// The common event types that will be received include:
        /// <list type="bullet">
        /// <item><description>mouseover - When the mouse pointer enters the target element</description></item>
        /// <item><description>mouseleave - When the mouse pointer leaves the target element</description></item>
        /// <item><description>mousedown - When a mouse button is pressed on the target element</description></item>
        /// </list>
        /// </remarks>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonIgnore]
        internal IJSRuntime JsRuntime
        {
            get => TooltipJsRuntime!;
            set => TooltipJsRuntime = value;
        }

        internal IJSRuntime? TooltipJsRuntime { get; set; }

        /// <exclude/>
        /// <summary>
        /// Compares the obj.
        /// </summary>
        /// <param name="obj">obj.</param>
        /// <returns>="obj".</returns>
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Gets the HashCode.
        /// </summary>
        /// <exclude/>
        /// <returns>int.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Represents the animation configuration model for Tooltip open and close operations.
    /// </summary>
    /// <remarks>
    /// This class provides separate animation settings for the opening and closing transitions of the Tooltip, allowing for different visual effects during each phase of the Tooltip lifecycle.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// var animationModel = new AnimationModel
    /// {
    ///     Open = new TooltipAnimationSettings
    ///     {
    ///         Effect = Effect.FadeIn,
    ///         Duration = 300,
    ///         Delay = 0
    ///     },
    ///     Close = new TooltipAnimationSettings
    ///     {
    ///         Effect = Effect.FadeOut,
    ///         Duration = 200,
    ///         Delay = 0
    ///     }
    /// };
    /// ]]></code>
    /// </example>
    public class AnimationModel
    {
        /// <summary>
        /// Gets or sets the animation settings applied to the Tooltip when it is being closed or hidden.
        /// </summary>
        /// <value>
        /// A <see cref="TooltipAnimationSettings"/> object that defines the closing animation properties. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property controls the visual transition effects when the Tooltip is being dismissed or hidden from view. Setting this property allows customization of the closing animation duration, delay, and effect type.
        /// </remarks>
        [JsonPropertyName("close")]
        public TooltipAnimationSettings? Close { get; set; }

        /// <summary>
        /// Gets or sets the animation settings applied to the Tooltip when it is being opened or shown.
        /// </summary>
        /// <value>
        /// A <see cref="TooltipAnimationSettings"/> object that defines the opening animation properties. The default value is <c>null</c>.
        /// </value>
        /// <remarks>
        /// This property controls the visual transition effects when the Tooltip is being displayed over the target element. Setting this property allows customization of the opening animation duration, delay, and effect type.
        /// </remarks>
        [JsonPropertyName("open")]
        public TooltipAnimationSettings? Open { get; set; }
    }
}