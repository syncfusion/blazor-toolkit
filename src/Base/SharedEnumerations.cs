namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Specifies the global animation behavior applied to Blazor Toolkit components.
    /// </summary>
    public enum GlobalAnimationMode
    {
        /// <summary>
        /// Use the default animation behavior (no override). The toolkit or application-level setting will determine
        /// whether animations are enabled.
        /// </summary>
        Default,

        /// <summary>
        /// Force animations to be enabled for components that support animation effects.
        /// </summary>
        Enable,

        /// <summary>
        /// Force animations to be disabled for components that support animation effects.
        /// </summary>
        Disable
    }


    /// <summary> 
    /// Specifies text overflow options when text overflows its container. 
    /// </summary> 
    public enum LabelOverflow
    {
        /// <summary> 
        /// Appends an ellipsis (...) to clipped text.
        /// </summary>     
        Ellipse,

        /// <summary> 
        /// Clips the text without appending any indicator.
        /// </summary> 
        Clip
    }

    /// <summary> 
    /// Specifies text wrap options when the text overflowing the container. 
    /// </summary> 
    public enum TextWrap
    {
        /// <summary> 
        /// Specifies to break words only at allowed break points. 
        /// </summary> 
        Normal,

        /// <summary> 
        /// Specifies to break a word once it is too long to fit on a line by itself. 
        /// </summary>     
        Wrap,

        /// <summary> 
        /// Specifies to break a word at any point if there are no otherwise-acceptable break points in the line. 
        /// </summary> 
        AnyWhere
    }

    /// <summary>
    /// Specifies the orientation of chart axis.
    /// </summary>
    public enum Orientation
    {
        /// <summary>
        /// Defines the null orientation.
        /// </summary>
        Null,

        /// <summary>
        /// Defines the horizontal orientation.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Defines the vertical orientation.
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Specifies the horizontal alignment of chart elements.
    /// </summary>
    /// <remarks>
    /// Used to position titles, legends, labels, and other UI elements within their containers.
    /// </remarks>
    public enum Alignment
    {
        /// <summary>
        /// Elements are aligned toward the start (left for LTR, right for RTL).
        /// </summary>
        Near,

        /// <summary>
        /// Elements are centered within their container.
        /// </summary>
        Center,

        /// <summary>
        /// Elements are aligned toward the end (right for LTR, left for RTL).
        /// </summary>
        Far
    }

    /// <summary>
    /// Specifies the unit of strip line size.
    /// </summary>
    public enum SizeType
    {
        /// <summary>
        /// Defines auto type.
        /// </summary>
        Auto,

        /// <summary>
        /// Defines pixel type.
        /// </summary>
        Pixel,

        /// <summary>
        /// Defines years type.
        /// </summary>
        Years,

        /// <summary>
        /// Defines months type.
        /// </summary>
        Months,

        /// <summary>
        /// Defines days type.
        /// </summary>
        Days,

        /// <summary>
        /// Defines hours type.
        /// </summary>
        Hours,

        /// <summary>
        /// Defines minutes type.
        /// </summary>
        Minutes,

        /// <summary>
        /// Defines seconds type.
        /// </summary>
        Seconds
    }

    /// <summary>
    /// Specifies the type of animation.
    /// </summary>
    public enum AnimationType
    {
        /// <summary>
        /// Defines the progressive animation type.
        /// </summary>
        Progressive,

        /// <summary>
        /// Defines the linear animation type.
        /// </summary>
        Linear,

        /// <summary>
        /// Defines the rect animation type.
        /// </summary>
        Rect,

        /// <summary>
        /// Defines the marker animation type.
        /// </summary>
        Marker
    }

    /// <summary>
    /// Specifies the marker shape options for SVG rendering.
    /// </summary>
    public enum ShapeName
    {
        /// <summary>
        /// Defines a path shape element.
        /// </summary>
        Path,

        /// <summary>
        /// Defines an ellipse shape element.
        /// </summary>
        Ellipse,

        /// <summary>
        /// Defines an image shape element.
        /// </summary>
        Image
    }

    /// <summary>
    /// Defines the visual style and color scheme of the chart.
    /// </summary>
    /// <remarks>
    /// The theme affects the chart's background color, text color, grid lines, axis labels, series colors, and legend appearance.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to apply a theme to the chart.
    /// <code>
    /// <![CDATA[
    /// <SfChart Title="Sales Data" Theme="Theme.Fluent">
    ///     <ChartSeries DataSource="@SalesData" XName="Month" YName="Revenue" Type="ChartSeriesType.Column" />
    /// </SfChart>
    /// ]]>
    /// </code>
    /// </example>
    public enum Theme
    {
        /// <summary>
        /// Applies the Fluent light theme to the chart, rendering with a light background, dark text, and neutral accent colors.
        /// </summary>
        /// <remarks>
        /// The Fluent light theme is ideal for applications that follow a light mode interface.
        /// </remarks>
        Fluent,
        /// <summary>
        /// Applies the Fluent dark theme to the chart, rendering with a dark background, light text, and adjusted accent colors for visibility.
        /// </summary>
        /// <remarks>
        /// The Fluent dark theme is designed for dark mode interfaces.
        /// </remarks>
        FluentDark,
        /// <summary>
        /// Applies the High Contrast theme to the chart, rendering with maximum-contrast colors for users who require stronger visual differentiation.
        /// </summary>
        /// <remarks>
        /// The High Contrast theme uses a black background (<b>#000000</b>), white text (<b>#FFFFFF</b>),
        /// a soft accessibility-yellow focus / selection color (<b>#FFD939</b>), and a high-contrast accent palette
        /// (axis labels and grid lines in neutral grays <b>#969696</b> / <b>#BFBFBF</b>).
        /// It is designed for accessibility scenarios and environments where Windows High Contrast Mode (or an equivalent OS-level accessibility setting) is enabled.
        /// </remarks>
        HighContrast,
        /// <summary>
        /// Applies the High Contrast Light theme to the chart, rendering with maximum-contrast colors for users who require stronger visual differentiation.
        /// </summary>
        /// <remarks>
        /// The High Contrast theme uses a black background (<b>#000000</b>), white text (<b>#FFFFFF</b>),
        /// a soft accessibility-yellow focus / selection color (<b>#FFD939</b>), and a high-contrast accent palette
        /// (axis labels and grid lines in neutral grays <b>#969696</b> / <b>#BFBFBF</b>).
        /// It is designed for accessibility scenarios and environments where Windows High Contrast Mode (or an equivalent OS-level accessibility setting) is enabled.
        /// </remarks>
        HighContrastLight
    }
}
