using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the category for the multilevel labels.
    /// </summary>
    /// <remarks>
    /// This component allows you to adjust properties related to individual categories, enhancing the organizational clarity of chart labels.
    /// </remarks>
    public class ChartCategory : ChartSubComponent
    {
        #region Fields

        private bool _isPropertyChanged;
        private object? _start;
        private object? _end;
        private string _text = string.Empty;

        [CascadingParameter]
        private ChartCategories? Parent { get; set; }

        #endregion

        #region Properties

        /// <summary> 
        /// Gets or sets the custom attributes for multi-level labels. 
        /// </summary> 
        /// <value> 
        /// An object that represents custom attributes for multi-labels. 
        /// </value> 
        /// <remarks> 
        /// The provided custom attribute can be accessed by the user from the OnAxisMultiLevelLabelRender event arguments for further actions or modifications. 
        /// </remarks> 
        /// <example> 
        /// <code> 
        /// <![CDATA[ 
        /// // This example demonstrates how to modify the text style of a multi-level label using custom attributes from the 'OnAxisMultiLevelLabelRender' event.
        /// <SfChart OnAxisMultiLevelLabelRender="OnMultiLevelLabelRender">
        ///     <ChartMultiLevelLabels> 
        ///         <ChartMultiLevelLabel> 
        ///             <ChartCategories> 
        ///                 <ChartCategory Start="-0.5" End="3.5" Text="Half yearly 1" MaximumTextWidth=50 CustomAttributes="@customAtt"></ChartCategory> 
        ///                 <ChartCategory Start="3.5" End="7.5" Text="Half yearly 2" MaximumTextWidth=50></ChartCategory> 
        ///             </ChartCategories> 
        ///         </ChartMultiLevelLabel> 
        ///     </ChartMultiLevelLabels> 
        ///     ... 
        /// </SfChart> 
        /// @code { 
        ///     object customAtt = new ChartAxisMultiLevelLabelTextStyle 
        ///     { 
        ///         FontFamily = "Roboto", 
        ///         FontWeight = "600", 
        ///         Color = "Red", 
        ///         Size = "14px" 
        ///     }; 
        ///     public void OnMultiLevelLabelRender(AxisMultiLabelRenderEventArgs args) 
        ///     { 
        ///         if (args.CustomAttributes is not null) 
        ///         { 
        ///             args.TextStyle = (ChartAxisMultiLevelLabelTextStyle)args.CustomAttributes; 
        ///         } 
        ///     } 
        /// } 
        /// ]]> 
        /// </code> 
        /// </example>
        [Parameter]
        public object CustomAttributes { get; set; } = null!;

        /// <summary> 
        /// Gets or sets the end value of the multi-level label. 
        /// </summary> 
        /// <value> 
        /// The end value of the multi-level label, specifying where the label range terminates. 
        /// </value> 
        /// <remarks>
        /// This property sets the end of a single multi-level label. It accepts the value based on the provided <see cref="ChartAxis.ValueType"/> and axis labels.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to define multi-level labels on the primary X-axis of a Chart. The ChartCategory defines a label spanning from value 10 to 40 with the text "Half yearly 1".
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
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
        public object? End { get; set; }

        /// <summary> 
        /// Gets or sets the maximum width of the text for multi-level labels. 
        /// </summary> 
        /// <value> 
        /// The maximum width of the text for multi-level labels. The default value is <b>0</b>.
        /// </value> 
        /// <remarks> 
        /// If the text exceeds this value, the overflow is handled based on the multi-level label's "TextOverflow" setting. 
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
        public double MaximumTextWidth { get; set; }

        /// <summary> 
        /// Gets or sets the start value of the multi-level labels. 
        /// </summary> 
        /// <value> 
        /// The start value of the multi-level label, determining where the label range begins. 
        /// </value> 
        /// <remarks>
        /// This property sets the start of a single multi-level label. It accepts the value based on the provided <see cref="ChartAxis.ValueType"/> and axis labels.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to define multi-level labels on the primary X-axis of a Chart. The ChartCategory defines a label spanning from value 10 to 40 with the text "Half yearly 1".
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
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
        public object? Start { get; set; }

        /// <summary> 
        /// Gets or sets the text for the multi-level labels. 
        /// </summary> 
        /// <value> 
        /// A string representing the text for the multi-level labels. 
        /// The default value is an empty string. 
        /// </value> 
        /// <remarks>
        /// This property specifies the label text that will be displayed for a multi-level category in the chart.
        /// You can set this to any descriptive text necessary to identify the category.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to define multi-level label text on the primary X-axis of a chart
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
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
        public string Text { get; set; } = string.Empty;

        /// <summary> 
        /// Gets or sets the border type for the individual multi-level label. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="BorderType"/> enumerations that specifies the border type for the individual multi-level label. 
        /// The options include: 
        /// - <c>Rectangle</c> - Renders the border in a rectangular shape around the label.
        /// - <c>WithoutTopBorder</c> - Renders the border without the top edge around the label.
        /// - <c>WithoutTopandBottomBorder</c> - Renders the border without both top and bottom edges around the label.
        /// - <c>WithoutBorder</c> - Renders the label without any border.
        /// - <c>Brace</c> - Renders the border with a brace-like shape around the label.
        /// - <c>CurlyBrace</c> - Renders the border with a curly brace-like shape around the label.
        /// <br/>
        /// The default value is <b>BorderType.Auto</b>. 
        /// </value> 
        /// <remarks> 
        /// The multi-level label for this category will be rendered with the specified border type.
        /// </remarks> 
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // This example demonstrates how to apply a 'CurlyBrace' border to a multi-level category label on the primary X-axis, grouping values between specified positions.
        /// <SfChart>
        ///     <ChartPrimaryXAxis>
        ///         <ChartMultiLevelLabels>
        ///             <ChartMultiLevelLabel>
        ///                 <ChartCategories>
        ///                     <ChartCategory Start="10" End="40" Text="Half yearly 1" Type="BorderType.CurlyBrace" />
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
        public BorderType Type { get; set; } = BorderType.Auto;

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
            if (Tracker is ChartCategories categories)
            {
                Parent = categories;
            }

            Parent?.Categories.Add(this);

            if (Parent?.Axis?.Renderer is not null)
            {
                _ = Parent.Chart?.ProcessOnLayoutChangeAsync();
            }
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
            if (!Equals(_start, Start) ||
                    !Equals(_end, End) ||
                    _text != Text)
            {
                _start = Start;
                _end = End;
                _text = Text;

                _isPropertyChanged = Parent is not null;
            }

            if (_isPropertyChanged)
            {
                _isPropertyChanged = false;
                Parent?.Chart?._axisContainer?.UpdateAxisRendering();
            }
        }

        /// <summary>
        /// Disposes resources used by this component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            _ = (Parent?.Categories?.Remove(this));
            if (Parent?.Axis?.Renderer is not null)
            {
                _ = (Parent?.Chart?.ProcessOnLayoutChangeAsync());
            }

            Parent = null;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }
        #endregion
    }
}