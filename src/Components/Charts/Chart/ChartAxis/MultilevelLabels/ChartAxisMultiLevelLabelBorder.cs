using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the multilevel label border for a chart axis, including color, border type, and width.
    /// </summary>
    /// <remarks>
    /// This component allows fine-grained control over the visual styling of borders around multi-level labels, supporting multiple border types and customizable appearance.
    /// </remarks>
    public class ChartAxisMultiLevelLabelBorder : ChartSubComponent
    {
        #region Fields

        private string _color = string.Empty;
        private double _width;
        private BorderType _type;

        [CascadingParameter]
        private ChartMultiLevelLabel? Parent { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the owner chart component.
        /// </summary>
        [CascadingParameter]
        internal SfChart? Owner { get; set; }

        /// <summary> 
        /// Gets or sets the color of the multi-level labels border.
        /// </summary> 
        /// <value> 
        /// A string representing the color of the multi-level labels border. The default multi-level label border color is determined by the chart's theme. By default, the theme is set to Fluent with a font color of <b>#b5b5b5</b>. 
        /// The default value is an empty string. 
        /// </value> 
        /// <remarks> 
        /// Use valid hex or rgba CSS color strings for the color value. 
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a colored border to a multi-level label on the X-axis.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
        ///                 <ChartAxisMultiLevelLabelBorder Color="blue" />
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
        public string Color { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the border type for multi-level labels. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="BorderType"/> enumerations that specifies the border type for multi-level labels. 
        /// The options include: 
        ///   - <c>Rectangle</c> - Renders the border in a rectangular shape around the label.
        ///   - <c>WithoutTopBorder</c> - Renders the border without the top edge around the label.
        ///   - <c>WithoutTopandBottomBorder</c> - Renders the border without both top and bottom edges around the label.
        ///   - <c>WithoutBorder</c> - Renders the label without any border.
        ///   - <c>Brace</c> - Renders the border with a brace-like shape around the label.
        ///   - <c>CurlyBrace</c> - Renders the border with a curly brace-like shape around the label.
        /// <br/>
        /// The default value is <b>BorderType.Rectangle</b>.
        /// </value> 
        /// <remarks>  
        /// This property determines the style of the border around multi-level labels.  
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a curly brace border to a multi-level label on the X-axis
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
        ///                 <ChartAxisMultiLevelLabelBorder Type="BorderType.CurlyBrace" />
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
        public BorderType Type { get; set; }

        /// <summary> 
        /// Gets or sets the width of the multi-level labels border in pixels. 
        /// </summary> 
        /// <value> 
        /// The width of the border in pixels. The default value is <b>1</b>. 
        /// </value>
        /// <remarks>
        /// This property specifies the thickness of the border.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to set the border width for a multi-level label on the X-axis.
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
        public double Width { get; set; } = 1;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs initialization when the component is first rendered.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartMultiLevelLabel multiLavelLabel)
            {
                Parent = multiLavelLabel;
            }
            Parent?.UpdateMultiLevelLabelProperties("Border", this);
        }

        /// <summary>
        /// Handles parameter changes and triggers a chart refresh if necessary.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Color != _color || Width != _width || Type != _type)
            {
                _color = Color;
                _width = Width;
                _type = Type;
                if (Owner != null && Owner._isChartFirstRender)
                {
                    _ = Owner.RefreshChartAsync();
                }
            }
        }

        /// <summary>
        /// Releases resources associated with this component.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            Parent = null;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }
        #endregion
    }
}
