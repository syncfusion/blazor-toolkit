using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize a single multi-level label for a chart axis.
    /// </summary>
    /// <remarks>
    /// This component represents an individual multi-level label that can contain multiple <see cref="ChartCategory"/> items.
    /// </remarks>
    public class ChartMultiLevelLabel : ChartSubComponent
    {
        #region Fields

        private Alignment _alignment = Alignment.Center;
        private TextOverflow _overflow = TextOverflow.Wrap;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cascading parent multi-level label component.
        /// </summary>
        [CascadingParameter]
        internal ChartMultiLevelLabels? MultilevelLabelCollection { get; set; }

        /// <summary>
        /// Gets or sets the parent chart instance.
        /// </summary>
        [CascadingParameter]
        internal SfChart? Owner { get; set; }

        /// <summary>  
        /// Gets or sets the alignment of the multi-level labels in the chart.  
        /// </summary>  
        /// <value>  
        /// One of the <see cref="Alignment"/> enumeration values that specifies the positioning of the multi-level labels.
        /// The options include:  
        /// - <c>Near</c>: Aligns labels at the start of the multi-level label rectangle (left for horizontal orientation, top for vertical orientation).  
        /// - <c>Center</c>: Centers the multi-level labels within the rectangle.  
        /// - <c>Far</c>: Aligns labels at the end of the multi-level label rectangle (right for horizontal orientation, bottom for vertical orientation).
        /// The default value is <b>Alignment.Center</b>.  
        /// </value>
        /// <remarks>
        /// This property determines how the multi-level labels are positioned relative to their placeholder rectangle, providing flexibility in label presentation.
        /// Proper alignment can enhance the readability and visual appeal of chart data.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to alignment the multi-level axis label text in chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel Alignment="Alignment.Far">
        ///                 <ChartCategories>
        ///                     <ChartCategory Start="10" End="40" Text="Half yearly 1" />
        ///                 </ChartCategories>
        ///             </ChartMultiLevelLabel>
        ///         </ChartMultiLevelLabels>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Alignment Alignment { get; set; } = Alignment.Center;

        /// <summary>  
        /// Gets or sets an instance of <see cref="ChartAxisMultiLevelLabelBorder"/> that specifies the border settings for multi-level labels.  
        /// </summary>  
        /// <value>  
        /// An instance of <see cref="ChartAxisMultiLevelLabelBorder"/>.  
        /// </value>  
        /// <remarks>  
        /// The Width, Color, and Type of the border for multi-level labels can be customized using this property.  
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the border for a multi-level label on the X-axis.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
        ///                 <ChartAxisMultiLevelLabelBorder Width="3" />
        ///                 <ChartCategories>
        ///                     <ChartCategory Start="10" End="40" Text="Half yearly 1" />
        ///                 </ChartCategories>
        ///             </ChartMultiLevelLabel>
        ///         </ChartMultiLevelLabels>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartAxisMultiLevelLabelBorder Border { get; set; } = new ChartAxisMultiLevelLabelBorder();

        /// <summary> 
        /// Gets or sets a list of <see cref="ChartCategory"/> representing the categories for multi-level labels. 
        /// </summary> 
        /// <value> 
        /// A list of <see cref="ChartCategory"/>. 
        /// </value> 
        /// <remarks> 
        /// The Start, End, Text, and MaximumTextWidth of multilevel labels can be customized using the <see cref="Categories"/> which accepts the collections of ChartCategory. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to configure a multi-level label on the primary X-axis with a maximum text width. The 'MaximumTextWidth' property ensures that the label text
        /// // wraps or truncates to fit within the specified width.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
        ///                 <ChartCategories>
        ///                     <ChartCategory Start="10" End="40" Text="Half yearly 1" MaximumTextWidth="50" />
        ///                 </ChartCategories>
        ///             </ChartMultiLevelLabel>
        ///         </ChartMultiLevelLabels>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public List<ChartCategory> Categories { get; set; } = [];

        /// <summary> 
        /// Gets or sets the text overflow behavior for multi-level labels. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="TextOverflow"/> enumeration values specifying text overflow options. 
        /// Options include: 
        /// - <c>TextOverflow.Trim</c>: Trims text exceeding defined margins. 
        /// - <c>TextOverflow.Wrap</c>: Wraps text exceeding defined margins. 
        /// - <c>TextOverflow.None</c>: Displays text without modification.
        /// <br/>
        /// The default value is <b>TextOverflow.Wrap</b>. 
        /// </value>
        /// <remarks>
        /// This property controls how multilevel label text is displayed when space is constrained, allowing for trimming, wrapping, or full display.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to trim the multi-level axis label text using the Overflow property.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel Overflow="TextOverflow.Trim">
        ///                 <ChartCategories>
        ///                     <ChartCategory Start="10" End="13" Text="Half yearly 1" />
        ///                 </ChartCategories>
        ///             </ChartMultiLevelLabel>
        ///         </ChartMultiLevelLabels>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public TextOverflow Overflow { get; set; } = TextOverflow.Wrap;

        /// <summary> 
        /// Gets or sets an instance of <see cref="ChartAxisMultiLevelLabelTextStyle"/> that controls the customization of the multilevel label text. 
        /// </summary> 
        /// <value> 
        /// An instance of <see cref="ChartAxisMultiLevelLabelTextStyle"/>. 
        /// </value> 
        /// <remarks> 
        /// This property can be used to customize the color, and font-properties of the multilevel label text. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to customize the text style of a multi-level label in the primary X-axis of a Chart.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
        ///                 <ChartAxisMultiLevelLabelTextStyle Size="15px" />
        ///                 <ChartCategories>
        ///                     <ChartCategory Start="10" End="40" Text="Half yearly 1" />
        ///                 </ChartCategories>
        ///             </ChartMultiLevelLabel>
        ///         </ChartMultiLevelLabels>
        ///     </ChartPrimaryXAxis>
        ///     <ChartSeriesCollection>
        ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" />
        ///     </ChartSeriesCollection>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ChartAxisMultiLevelLabelTextStyle TextStyle { get; set; } = new ChartAxisMultiLevelLabelTextStyle();

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Invoked when the component initializes.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartMultiLevelLabels multiLevelLabels)
            {
                MultilevelLabelCollection = multiLevelLabels;
            }
            MultilevelLabelCollection?.MultiLevelLabels.Add(this);
        }

        /// <summary>
        /// Invoked when component parameters are set.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Overflow != _overflow || Alignment != _alignment)
            {
                _overflow = Overflow;
                _alignment = Alignment;
                if (Owner != null && Owner._isChartFirstRender)
                {
                    _ = Owner.RefreshChartAsync();
                }
            }
        }

        /// <summary>
        /// Disposes resources used by this component.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            MultilevelLabelCollection = null;
            ChildContent = null!;
            Categories = null!;
            Border = null!;
            TextStyle = null!;
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates multi-level label properties when child components change.
        /// </summary>
        /// <param name="key">The property name to update.</param>
        /// <param name="keyValue">The new property value.</param>
        internal void UpdateMultiLevelLabelProperties(string key, object keyValue)
        {
            if (key == nameof(Border))
            {
                Border = (ChartAxisMultiLevelLabelBorder)keyValue;
            }
            else if (key == nameof(TextStyle))
            {
                TextStyle = (ChartAxisMultiLevelLabelTextStyle)keyValue;
            }
            else if (key == nameof(Categories))
            {
                Categories = (List<ChartCategory>)keyValue;
            }
        }
        #endregion
    }
}