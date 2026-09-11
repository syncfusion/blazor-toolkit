using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Defines the types of target elements for positioning popup components.
    /// </summary>
    /// <remarks>
    /// The <see cref="TargetType"/> enumeration specifies how the popup element should be positioned relative to its target element.
    /// This affects the positioning behavior and reference point used by the popup component.
    /// </remarks>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TargetType
    {
        /// <summary>
        /// Specifies that the popup should be positioned relative to the target element.
        /// </summary>
        /// <remarks>
        /// When set to Relative, the popup position is calculated based on the target element's position and dimensions.
        /// This is useful when you want the popup to appear in relation to a specific UI element.
        /// </remarks>
        Relative,

        /// <summary>
        /// Specifies that the popup should be positioned relative to the container element.
        /// </summary>
        /// <remarks>
        /// When set to Container, the popup position is calculated based on the container's boundaries.
        /// This is useful for creating popups that stay within specific container boundaries.
        /// </remarks>
        Container
    }

    /// <summary>
    /// Defines the types of collision handling behavior for popup elements when they exceed viewport boundaries.
    /// </summary>
    /// <remarks>
    /// The <see cref="CollisionType"/> enumeration specifies how the popup should behave when it would be positioned outside the visible area.
    /// This helps ensure popups remain visible and accessible to users regardless of the target element's position.
    /// </remarks>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CollisionType
    {
        /// <summary>
        /// Specifies that no collision handling should be applied to the popup.
        /// </summary>
        /// <remarks>
        /// When set to None, the popup will be positioned at its calculated position regardless of viewport boundaries.
        /// This may result in the popup being partially or completely hidden outside the visible area.
        /// </remarks>
        [EnumMember(Value = "none")]
        None,

        /// <summary>
        /// Specifies that the popup should flip to the opposite side when collision is detected.
        /// </summary>
        /// <remarks>
        /// When set to Flip, the popup will automatically reposition to the opposite side of the target element
        /// if the original position would cause the popup to exceed viewport boundaries.
        /// </remarks>
        [EnumMember(Value = "flip")]
        Flip,

        /// <summary>
        /// Specifies that the popup should be adjusted to fit within the viewport boundaries.
        /// </summary>
        /// <remarks>
        /// When set to Fit, the popup position will be adjusted to ensure it remains completely visible within the viewport.
        /// The popup may be moved or resized to accommodate the available space.
        /// </remarks>
        [EnumMember(Value = "fit")]
        Fit
    }

    /// <summary>
    /// Specifies the animation effects that can be applied to the Tooltip component during show and hide transitions.
    /// </summary>
    /// <remarks>
    /// The <see cref="Effect"/> enumeration provides various animation options to enhance the visual experience when displaying or hiding tooltips.
    /// Different effects can be configured for open and close actions to create custom transition behaviors.
    /// Animation effects improve user experience by providing smooth visual feedback during tooltip interactions.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to set animation effects for a tooltip:
    /// <code><![CDATA[
    /// <SfTooltip Content="Sample Tooltip" OpenEffect="Effect.FadeIn" CloseEffect="Effect.FadeOut">
    ///     <div>Hover over me</div>
    /// </SfTooltip>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Effect
    {
        /// <summary>
        /// Applies a fade-in animation effect where the tooltip gradually appears with increasing opacity.
        /// </summary>
        /// <remarks>
        /// The FadeIn effect provides a smooth transition by gradually increasing the tooltip's opacity from 0% to 100%.
        /// This is commonly used for tooltip opening animations to create a gentle appearance effect.
        /// </remarks>
        [EnumMember(Value = "FadeIn")]
        FadeIn,

        /// <summary>
        /// Applies a fade-out animation effect where the tooltip gradually disappears with decreasing opacity.
        /// </summary>
        /// <remarks>
        /// The FadeOut effect provides a smooth transition by gradually decreasing the tooltip's opacity from 100% to 0%.
        /// This is commonly used for tooltip closing animations to create a gentle disappearance effect.
        /// </remarks>
        [EnumMember(Value = "FadeOut")]
        FadeOut,

        /// <summary>
        /// Applies a combined fade and zoom-in animation effect where the tooltip appears with both opacity and scale transitions.
        /// </summary>
        /// <remarks>
        /// The FadeZoomIn effect combines fading and scaling animations, making the tooltip appear to grow from a smaller size while fading in.
        /// This creates a more dynamic and attention-grabbing entrance animation compared to simple fade effects.
        /// </remarks>
        [EnumMember(Value = "FadeZoomIn")]
        FadeZoomIn,

        /// <summary>
        /// Applies a combined fade and zoom-out animation effect where the tooltip disappears with both opacity and scale transitions.
        /// </summary>
        /// <remarks>
        /// The FadeZoomOut effect combines fading and scaling animations, making the tooltip appear to shrink to a smaller size while fading out.
        /// This creates a more dynamic and visually appealing exit animation compared to simple fade effects.
        /// </remarks>
        [EnumMember(Value = "FadeZoomOut")]
        FadeZoomOut,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the X-axis from the bottom upward during appearance.
        /// </summary>
        /// <remarks>
        /// The FlipXDownIn effect creates a 3D rotation animation where the tooltip flips from the bottom edge upward along the X-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate into view from below.
        /// </remarks>
        [EnumMember(Value = "FlipXDownIn")]
        FlipXDownIn,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the X-axis from the top downward during disappearance.
        /// </summary>
        /// <remarks>
        /// The FlipXDownOut effect creates a 3D rotation animation where the tooltip flips from the top edge downward along the X-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate out of view downward.
        /// </remarks>
        [EnumMember(Value = "FlipXDownOut")]
        FlipXDownOut,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the X-axis from the top upward during appearance.
        /// </summary>
        /// <remarks>
        /// The FlipXUpIn effect creates a 3D rotation animation where the tooltip flips from the top edge upward along the X-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate into view from above.
        /// </remarks>
        [EnumMember(Value = "FlipXUpIn")]
        FlipXUpIn,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the X-axis from the bottom upward during disappearance.
        /// </summary>
        /// <remarks>
        /// The FlipXUpOut effect creates a 3D rotation animation where the tooltip flips from the bottom edge upward along the X-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate out of view upward.
        /// </remarks>
        [EnumMember(Value = "FlipXUpOut")]
        FlipXUpOut,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the Y-axis from the right toward the left during appearance.
        /// </summary>
        /// <remarks>
        /// The FlipYLeftIn effect creates a 3D rotation animation where the tooltip flips from the right edge toward the left along the Y-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate into view from the right side.
        /// </remarks>
        [EnumMember(Value = "FlipYLeftIn")]
        FlipYLeftIn,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the Y-axis from the left toward the right during disappearance.
        /// </summary>
        /// <remarks>
        /// The FlipYLeftOut effect creates a 3D rotation animation where the tooltip flips from the left edge toward the right along the Y-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate out of view toward the right side.
        /// </remarks>
        [EnumMember(Value = "FlipYLeftOut")]
        FlipYLeftOut,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the Y-axis from the left toward the right during appearance.
        /// </summary>
        /// <remarks>
        /// The FlipYRightIn effect creates a 3D rotation animation where the tooltip flips from the left edge toward the right along the Y-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate into view from the left side.
        /// </remarks>
        [EnumMember(Value = "FlipYRightIn")]
        FlipYRightIn,

        /// <summary>
        /// Applies a 3D flip animation effect where the tooltip rotates along the Y-axis from the right toward the left during disappearance.
        /// </summary>
        /// <remarks>
        /// The FlipYRightOut effect creates a 3D rotation animation where the tooltip flips from the right edge toward the left along the Y-axis.
        /// This provides a sophisticated 3D visual effect that makes the tooltip appear to rotate out of view toward the left side.
        /// </remarks>
        [EnumMember(Value = "FlipYRightOut")]
        FlipYRightOut,

        /// <summary>
        /// Applies a zoom-in animation effect where the tooltip appears by scaling from a smaller size to its full size.
        /// </summary>
        /// <remarks>
        /// The ZoomIn effect creates a scaling animation where the tooltip grows from a smaller scale to its normal size during appearance.
        /// This provides a dynamic visual effect that draws attention to the tooltip as it expands into view.
        /// </remarks>
        [EnumMember(Value = "ZoomIn")]
        ZoomIn,

        /// <summary>
        /// Applies a zoom-out animation effect where the tooltip disappears by scaling from its full size to a smaller size.
        /// </summary>
        /// <remarks>
        /// The ZoomOut effect creates a scaling animation where the tooltip shrinks from its normal size to a smaller scale during disappearance.
        /// This provides a dynamic visual effect that makes the tooltip appear to compress as it exits view.
        /// </remarks>
        [EnumMember(Value = "ZoomOut")]
        ZoomOut,

        /// <summary>
        /// Specifies that no animation effect should be applied to the tooltip during show or hide transitions.
        /// </summary>
        /// <remarks>
        /// The None option disables all animation effects, causing the tooltip to appear or disappear instantly without any transition.
        /// This is useful for performance-critical scenarios or when immediate tooltip display is preferred over visual effects.
        /// </remarks>
        [EnumMember(Value = "None")]
        None
    }

    /// <summary>
    /// Specifies the different trigger modes that determine how the Tooltip component is opened and displayed to users.
    /// </summary>
    /// <remarks>
    /// The <see cref="OpenMode"/> enumeration defines various interaction patterns for triggering tooltip display.
    /// Different modes provide flexibility to accommodate different user interface patterns and device types.
    /// The behavior may vary between desktop and mobile devices to ensure optimal user experience across platforms.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to set different open modes for a tooltip:
    /// <code><![CDATA[
    /// <SfTooltip Content="Click to see tooltip" OpenMode="OpenMode.Click">
    ///     <button>Click me</button>
    /// </SfTooltip>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OpenMode
    {
        /// <summary>
        /// Automatically determines the best trigger method based on the target element type and device capabilities.
        /// </summary>
        /// <remarks>
        /// In Auto mode, the tooltip intelligently selects the appropriate trigger mechanism:
        /// - On desktop: appears on hover for most elements, or focus for interactive elements like buttons and inputs
        /// - On touch devices: opens on tap and hold gesture to accommodate touch interaction patterns
        /// This mode provides the most intuitive user experience by adapting to the context and platform.
        /// </remarks>
        [EnumMember(Value = "Auto")]
        Auto,

        /// <summary>
        /// Triggers the tooltip when the user hovers the mouse pointer over the target element.
        /// </summary>
        /// <remarks>
        /// Hover mode is optimized for mouse-based interactions on desktop devices:
        /// - On desktop: tooltip appears immediately when the mouse enters the target element area
        /// - On touch devices: opens on tap and hold gesture since hover is not directly supported
        /// This mode is ideal for providing contextual information without requiring explicit user actions.
        /// </remarks>
        [EnumMember(Value = "Hover")]
        Hover,

        /// <summary>
        /// Triggers the tooltip when the user clicks or taps on the target element.
        /// </summary>
        /// <remarks>
        /// Click mode requires explicit user action to display the tooltip:
        /// - On desktop: tooltip appears when the target element is clicked with the mouse
        /// - On touch devices: tooltip appears with a single tap on the target element
        /// This mode is useful when you want to show tooltips only on demand or for important information that requires user acknowledgment.
        /// </remarks>
        [EnumMember(Value = "Click")]
        Click,

        /// <summary>
        /// Triggers the tooltip when the target element receives keyboard focus or is programmatically focused.
        /// </summary>
        /// <remarks>
        /// Focus mode is particularly useful for accessibility and keyboard navigation:
        /// - On desktop: tooltip appears when the element receives focus via keyboard navigation or programmatic focus
        /// - On touch devices: tooltip appears with a single tap since touch typically triggers focus
        /// This mode ensures tooltips are accessible to users relying on keyboard navigation and assistive technologies.
        /// </remarks>
        [EnumMember(Value = "Focus")]
        Focus,

        /// <summary>
        /// Disables all default trigger behaviors, requiring manual control through programmatic methods.
        /// </summary>
        /// <remarks>
        /// Custom mode provides complete control over tooltip display timing:
        /// - No automatic triggers are active, preventing default show/hide behavior
        /// - Tooltips must be controlled using the Open() and Close() public methods
        /// - Both desktop and mobile devices require explicit method calls for tooltip management
        /// This mode is ideal for complex scenarios where tooltip display depends on custom business logic or specific application states.
        /// </remarks>
        [EnumMember(Value = "Custom")]
        Custom
    }

    /// <summary>
    /// Specifies the positioning options that determine where the Tooltip should be displayed relative to its target element.
    /// </summary>
    /// <remarks>
    /// The <see cref="Position"/> enumeration provides precise control over tooltip placement around target elements.
    /// Each position combines a primary direction (top, bottom, left, right) with an alignment (center, left, right, top, bottom).
    /// The tooltip positioning system automatically handles collision detection and may adjust the position if the preferred location would place the tooltip outside the viewport.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to set tooltip position:
    /// <code><![CDATA[
    /// <SfTooltip Content="Positioned at top-center" Position="Position.TopCenter">
    ///     <div>Target element</div>
    /// </SfTooltip>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Position
    {
        /// <summary>
        /// Positions the tooltip above the target element, horizontally centered with respect to the target's width.
        /// </summary>
        /// <remarks>
        /// TopCenter positioning places the tooltip directly above the target element with the tooltip's horizontal center aligned with the target's horizontal center.
        /// This is one of the most commonly used positions as it provides clear visibility without obscuring the target element.
        /// The tooltip's bottom edge will be positioned near the target's top edge.
        /// </remarks>
        [EnumMember(Value = "TopCenter")]
        TopCenter,

        /// <summary>
        /// Positions the tooltip above the target element, aligned with the target's left edge.
        /// </summary>
        /// <remarks>
        /// TopLeft positioning places the tooltip above the target element with the tooltip's left edge aligned with the target's left edge.
        /// This position is useful when you want to maintain left alignment between the tooltip and target.
        /// The tooltip appears above the target without extending beyond the target's left boundary.
        /// </remarks>
        [EnumMember(Value = "TopLeft")]
        TopLeft,

        /// <summary>
        /// Positions the tooltip above the target element, aligned with the target's right edge.
        /// </summary>
        /// <remarks>
        /// TopRight positioning places the tooltip above the target element with the tooltip's right edge aligned with the target's right edge.
        /// This position is useful when you want to maintain right alignment between the tooltip and target.
        /// The tooltip appears above the target without extending beyond the target's right boundary.
        /// </remarks>
        [EnumMember(Value = "TopRight")]
        TopRight,

        /// <summary>
        /// Positions the tooltip below the target element, aligned with the target's left edge.
        /// </summary>
        /// <remarks>
        /// BottomLeft positioning places the tooltip below the target element with the tooltip's left edge aligned with the target's left edge.
        /// This position maintains consistent left alignment and is useful when screen space above the target is limited.
        /// The tooltip's top edge will be positioned near the target's bottom edge.
        /// </remarks>
        [EnumMember(Value = "BottomLeft")]
        BottomLeft,

        /// <summary>
        /// Positions the tooltip below the target element, horizontally centered with respect to the target's width.
        /// </summary>
        /// <remarks>
        /// BottomCenter positioning places the tooltip directly below the target element with the tooltip's horizontal center aligned with the target's horizontal center.
        /// This is another commonly used position that provides excellent visibility when there's insufficient space above the target.
        /// The tooltip's top edge will be positioned near the target's bottom edge.
        /// </remarks>
        [EnumMember(Value = "BottomCenter")]
        BottomCenter,

        /// <summary>
        /// Positions the tooltip below the target element, aligned with the target's right edge.
        /// </summary>
        /// <remarks>
        /// BottomRight positioning places the tooltip below the target element with the tooltip's right edge aligned with the target's right edge.
        /// This position maintains consistent right alignment and is useful for right-aligned UI layouts.
        /// The tooltip appears below the target without extending beyond the target's right boundary.
        /// </remarks>
        [EnumMember(Value = "BottomRight")]
        BottomRight,

        /// <summary>
        /// Positions the tooltip to the left of the target element, aligned with the target's top edge.
        /// </summary>
        /// <remarks>
        /// LeftTop positioning places the tooltip to the left side of the target element with the tooltip's top edge aligned with the target's top edge.
        /// This position is useful for wide layouts where horizontal space is available on the left side.
        /// The tooltip's right edge will be positioned near the target's left edge.
        /// </remarks>
        [EnumMember(Value = "LeftTop")]
        LeftTop,

        /// <summary>
        /// Positions the tooltip to the left of the target element, vertically centered with respect to the target's height.
        /// </summary>
        /// <remarks>
        /// LeftCenter positioning places the tooltip to the left side of the target element with the tooltip's vertical center aligned with the target's vertical center.
        /// This position provides optimal balance and is commonly used when there's adequate space to the left of the target.
        /// The tooltip's right edge will be positioned near the target's left edge.
        /// </remarks>
        [EnumMember(Value = "LeftCenter")]
        LeftCenter,

        /// <summary>
        /// Positions the tooltip to the left of the target element, aligned with the target's bottom edge.
        /// </summary>
        /// <remarks>
        /// LeftBottom positioning places the tooltip to the left side of the target element with the tooltip's bottom edge aligned with the target's bottom edge.
        /// This position maintains bottom alignment and is useful when you want the tooltip to align with the target's lower portion.
        /// The tooltip's right edge will be positioned near the target's left edge.
        /// </remarks>
        [EnumMember(Value = "LeftBottom")]
        LeftBottom,

        /// <summary>
        /// Positions the tooltip to the right of the target element, aligned with the target's top edge.
        /// </summary>
        /// <remarks>
        /// RightTop positioning places the tooltip to the right side of the target element with the tooltip's top edge aligned with the target's top edge.
        /// This position is useful for layouts where horizontal space is available on the right side.
        /// The tooltip's left edge will be positioned near the target's right edge.
        /// </remarks>
        [EnumMember(Value = "RightTop")]
        RightTop,

        /// <summary>
        /// Positions the tooltip to the right of the target element, vertically centered with respect to the target's height.
        /// </summary>
        /// <remarks>
        /// RightCenter positioning places the tooltip to the right side of the target element with the tooltip's vertical center aligned with the target's vertical center.
        /// This position provides optimal balance and is commonly used when there's adequate space to the right of the target.
        /// The tooltip's left edge will be positioned near the target's right edge.
        /// </remarks>
        [EnumMember(Value = "RightCenter")]
        RightCenter,

        /// <summary>
        /// Positions the tooltip to the right of the target element, aligned with the target's bottom edge.
        /// </summary>
        /// <remarks>
        /// RightBottom positioning places the tooltip to the right side of the target element with the tooltip's bottom edge aligned with the target's bottom edge.
        /// This position maintains bottom alignment and is useful when you want the tooltip to align with the target's lower portion.
        /// The tooltip's left edge will be positioned near the target's right edge.
        /// </remarks>
        [EnumMember(Value = "RightBottom")]
        RightBottom
    }

    /// <summary>
    /// Specifies the positioning options for the tip pointer (arrow) that connects the Tooltip to its target element.
    /// </summary>
    /// <remarks>
    /// The <see cref="TipPointerPosition"/> enumeration controls the placement of the visual pointer that indicates the relationship between the tooltip and its target.
    /// The tip pointer provides a clear visual connection, making it obvious which element the tooltip refers to.
    /// The actual position depends on the tooltip's overall position relative to the target element (top, bottom, left, or right).
    /// Different pointer positions can improve visual balance and alignment in various UI layouts.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to set tip pointer position:
    /// <code><![CDATA[
    /// <SfTooltip Content="Tooltip with custom pointer" TipPointerPosition="TipPointerPosition.Start">
    ///     <div>Target element</div>
    /// </SfTooltip>
    /// ]]></code>
    /// </example>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipPointerPosition
    {
        /// <summary>
        /// Automatically determines the optimal tip pointer position based on the tooltip's position and available space.
        /// </summary>
        /// <remarks>
        /// Auto positioning allows the tooltip component to intelligently select the best pointer placement:
        /// - Considers the tooltip's position relative to the target element
        /// - Takes into account available viewport space and potential collisions
        /// - Ensures optimal visual connection between tooltip and target
        /// This mode provides the most user-friendly experience by adapting to different scenarios automatically.
        /// </remarks>
        [EnumMember(Value = "Auto")]
        Auto,

        /// <summary>
        /// Positions the tip pointer at the beginning edge of the tooltip element.
        /// </summary>
        /// <remarks>
        /// Start positioning places the pointer at the beginning of the tooltip's edge that faces the target:
        /// - For top/bottom positioned tooltips: pointer appears at the left edge of the tooltip
        /// - For left/right positioned tooltips: pointer appears at the top edge of the tooltip
        /// This position is useful for creating left-aligned or top-aligned pointer placement that matches UI design requirements.
        /// </remarks>
        [EnumMember(Value = "Start")]
        Start,

        /// <summary>
        /// Positions the tip pointer at the center of the tooltip element's edge facing the target.
        /// </summary>
        /// <remarks>
        /// Middle positioning places the pointer at the center point of the tooltip's edge that faces the target:
        /// - For top/bottom positioned tooltips: pointer appears at the horizontal center of the tooltip
        /// - For left/right positioned tooltips: pointer appears at the vertical center of the tooltip
        /// This position provides balanced visual alignment and is commonly used for centered tooltip designs.
        /// </remarks>
        [EnumMember(Value = "Middle")]
        Middle,

        /// <summary>
        /// Positions the tip pointer at the ending edge of the tooltip element.
        /// </summary>
        /// <remarks>
        /// End positioning places the pointer at the end of the tooltip's edge that faces the target:
        /// - For top/bottom positioned tooltips: pointer appears at the right edge of the tooltip
        /// - For left/right positioned tooltips: pointer appears at the bottom edge of the tooltip
        /// This position is useful for creating right-aligned or bottom-aligned pointer placement that matches specific UI design requirements.
        /// </remarks>
        [EnumMember(Value = "End")]
        End
    }

    /// <summary>
    /// Specifies the direction from which the <see cref="SfDialog"/> can be resized.
    /// </summary>
    /// <remarks>
    /// This enumeration controls which edges and corners of the dialog are active for resizing. To enable resizing, set <see cref="SfDialog.EnableResize"/> to <c>true</c>.
    /// </remarks>
    public enum ResizeDirection
    {
        /// <summary>
        /// Specifies that the dialog can be resized by dragging the bottom-right corner.
        /// </summary>
        [EnumMember(Value = "SouthEast")]
        SouthEast,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the bottom edge.
        /// </summary>
        [EnumMember(Value = "South")]
        South,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the top edge.
        /// </summary>
        [EnumMember(Value = "North")]
        North,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the right edge.
        /// </summary>
        [EnumMember(Value = "East")]
        East,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the left edge.
        /// </summary>
        [EnumMember(Value = "West")]
        West,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the top-right corner.
        /// </summary>
        [EnumMember(Value = "NorthEast")]
        NorthEast,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the top-left corner.
        /// </summary>
        [EnumMember(Value = "NorthWest")]
        NorthWest,

        /// <summary>
        /// Specifies that the dialog can be resized by dragging the bottom-left corner.
        /// </summary>
        [EnumMember(Value = "SouthWest")]
        SouthWest,

        /// <summary>
        /// Specifies that the dialog can be resized from all edges and corners.
        /// </summary>
        [EnumMember(Value = "All")]
        All
    }

    /// <summary>
    /// Specifies the built-in animation effect to apply when the <see cref="SfDialog"/> is shown or hidden.
    /// </summary>
    /// <remarks>
    /// These effects provide visual transitions to enhance user experience. The animation is determined by the <see cref="DialogAnimationSettings.Effect"/> property.
    /// </remarks>
    public enum DialogEffect
    {
        /// <summary>
        /// The dialog fades in when opening and fades out when closing.
        /// </summary>
        [EnumMember(Value = "Fade")]
        Fade,

        /// <summary>
        /// The dialog fades and zooms in when opening, and fades and zooms out when closing.
        /// </summary>
        [EnumMember(Value = "FadeZoom")]
        FadeZoom,

        /// <summary>
        /// The dialog flips in from the left and downwards when opening.
        /// </summary>
        [EnumMember(Value = "FlipLeftDown")]
        FlipLeftDown,

        /// <summary>
        /// The dialog flips in from the left and upwards when opening.
        /// </summary>
        [EnumMember(Value = "FlipLeftUp")]
        FlipLeftUp,

        /// <summary>
        /// The dialog flips in from the right and downwards when opening.
        /// </summary>
        [EnumMember(Value = "FlipRightDown")]
        FlipRightDown,

        /// <summary>
        /// The dialog flips in from the right and upwards when opening.
        /// </summary>
        [EnumMember(Value = "FlipRightUp")]
        FlipRightUp,

        /// <summary>
        /// The dialog flips downwards on its X-axis when opening.
        /// </summary>
        [EnumMember(Value = "FlipXDown")]
        FlipXDown,

        /// <summary>
        /// The dialog flips upwards on its X-axis when opening.
        /// </summary>
        [EnumMember(Value = "FlipXUp")]
        FlipXUp,

        /// <summary>
        /// The dialog flips to the left on its Y-axis when opening.
        /// </summary>
        [EnumMember(Value = "FlipYLeft")]
        FlipYLeft,

        /// <summary>
        /// The dialog flips to the right on its Y-axis when opening.
        /// </summary>
        [EnumMember(Value = "FlipYRight")]
        FlipYRight,

        /// <summary>
        /// The dialog slides in from the bottom when opening.
        /// </summary>
        [EnumMember(Value = "SlideBottom")]
        SlideBottom,

        /// <summary>
        /// The dialog slides in from the left when opening.
        /// </summary>
        [EnumMember(Value = "SlideLeft")]
        SlideLeft,

        /// <summary>
        /// The dialog slides in from the right when opening.
        /// </summary>
        [EnumMember(Value = "SlideRight")]
        SlideRight,

        /// <summary>
        /// The dialog slides in from the top when opening.
        /// </summary>
        [EnumMember(Value = "SlideTop")]
        SlideTop,

        /// <summary>
        /// The dialog zooms in when opening and zooms out when closing.
        /// </summary>
        [EnumMember(Value = "Zoom")]
        Zoom,

        /// <summary>
        /// No animation is applied. The dialog appears and disappears instantly.
        /// </summary>
        [EnumMember(Value = "None")]
        None
    }
}