namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Provides options for customizing the text style of the chart component.
    /// </summary>
    public interface IChartDefaultFont
    {
        /// <summary> 
        /// Gets or sets the color for the text. 
        /// </summary> 
        /// <value> 
        /// A string representing the color for the text. The default value is <b>null</b>. 
        /// </value> 
        /// <remarks> 
        /// Accepts values in hex and rgba as a valid CSS color string. 
        /// </remarks>
        string Color { get; set; }

        /// <summary> 
        /// Gets or sets the font family for the text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font family for the text. The default value is <b>"Segoe UI"</b>. 
        /// </value> 
        /// <remarks>
        /// This property specifies the font face used in rendering the text, offering a wide variety of standard and custom fonts.
        /// </remarks>
        string FontFamily { get; set; }

        /// <summary> 
        /// Gets or sets the font style for the text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font style for the text. The default value is <b>"Normal"</b>. 
        /// </value> 
        /// <remarks>
        /// The font style can be used to apply settings like Italic or Oblique to the text, affecting its emphasis and legibility.
        /// </remarks>
        string FontStyle { get; set; }

        /// <summary> 
        /// Gets or sets the font weight for the text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font weight for the text. The default value is <b>"Normal"</b>. 
        /// </value> 
        /// <remarks>
        /// Use this property to define the thickness of the text characters, providing levels of boldness.
        /// </remarks>
        string FontWeight { get; set; }

        /// <summary> 
        /// Gets or sets the opacity for the text. 
        /// </summary> 
        /// <value> 
        /// A double value representing the opacity for the text. The default value is <b>1</b>. 
        /// </value> 
        /// <remarks>
        /// The opacity determines the transparency level of the text, where 1 is completely opaque, and 0 is fully transparent.
        /// </remarks>
        double Opacity { get; set; }

        /// <summary> 
        /// Gets or sets the font size for the text. 
        /// </summary> 
        /// <value> 
        /// A string representing the font size for the text. The default value is <b>"16px"</b>. 
        /// </value> 
        /// <remarks>
        /// This property determines the size of the text, impacting its readability within the chart component context.
        /// </remarks>
        string Size { get; set; }

        /// <summary> 
        /// Gets or sets the text alignment. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="Alignment"/> enumerations that specifies the alignment of the text. 
        /// The options include: 
        /// - <c>Near</c> 
        /// - <c>Center</c> 
        /// - <c>Far</c> 
        /// <br/>
        /// The default value is <b>Alignment.Center</b>. 
        /// </value> 
        /// <remarks>
        /// Text alignment adjusts the horizontal placement of text within its container, affecting its position relative to other content.
        /// </remarks>
        Alignment TextAlignment { get; set; }

        /// <summary> 
        /// Gets or sets the text overflow behavior to employ when the text exceeds the defined margins. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="TextOverflow"/> enumeration that specifies the text overflow options. 
        /// The options include: 
        /// - <c>TextOverflow.Trim</c>: Trims the text if it exceeds the defined margins. 
        /// - <c>TextOverflow.Wrap</c>: Wraps the text if it exceeds the defined margins. 
        /// - <c>TextOverflow.None</c>: Shows the text as it is. 
        /// <br/>
        /// The default value is <b>TextOverflow.Trim</b>. 
        /// </value> 
        /// <remarks>
        /// This property manages how text should behave when it's too long for its allocated space, either by trimming, wrapping, or displaying fully.
        /// </remarks>
        TextOverflow TextOverflow { get; set; }
    }
}