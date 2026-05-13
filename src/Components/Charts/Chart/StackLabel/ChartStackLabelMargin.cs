using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the margin around the stack label.
    /// </summary>
    /// <remarks>
    /// Use this component to fine-tune the space between the label text and its bounding box.
    /// </remarks>
    public class ChartStackLabelMargin : ChartDefaultMargin
    {
        #region Fields

        private ChartStackLabelSettings? _stackLabel;
        private double _previousBottom;
        private double _previousTop;
        private double _previousRight;
        private double _previousLeft;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the bottom margin of the stack label.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value that specifies the bottom margin in pixels. The default value is <b>5</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName="x" YName="y" Type="ChartSeriesType.StackingColumn">
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true">
        ///         <ChartStackLabelMargin Bottom="5">
        ///         </ChartStackLabelMargin>
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This property customizes the space between the stack label and the bottom edge of its border.
        /// </remarks>
        [Parameter]
        public override double Bottom { get; set; } = Constants.DefaultMargin;

        /// <summary>
        /// Gets or sets the left margin of the stack label.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value that specifies the left margin in pixels. The default value is <b>5</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName="x" YName="y" Type="ChartSeriesType.StackingColumn">
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true">
        ///         <ChartStackLabelMargin Left="5">
        ///         </ChartStackLabelMargin>
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This property customizes the space between the stack label and the left edge of its border.
        /// </remarks>
        [Parameter]
        public override double Left { get; set; } = Constants.DefaultMargin;

        /// <summary>
        /// Gets or sets the right margin of the stack label.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value that specifies the right margin in pixels. The default value is <b>5</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName="x" YName="y" Type="ChartSeriesType.StackingColumn">
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true">
        ///         <ChartStackLabelMargin Right="5">
        ///         </ChartStackLabelMargin>
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This property customizes the space between the stack label and the right edge of its border.
        /// </remarks>
        [Parameter]
        public override double Right { get; set; } = Constants.DefaultMargin;

        /// <summary>
        /// Gets or sets the top margin of the stack label.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> value that specifies the top margin in pixels. The default value is <b>5</b>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName="x" YName="y" Type="ChartSeriesType.StackingColumn">
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true">
        ///         <ChartStackLabelMargin Top="5">
        ///         </ChartStackLabelMargin>
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This property customizes the space between the stack label and the top edge of its border.
        /// </remarks>
        [Parameter]
        public override double Top { get; set; } = Constants.DefaultMargin;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component and registers this margin instance with the parent <see cref="ChartStackLabelSettings"/>.
        /// </summary>
        /// <remarks>
        /// This ensures the parent stack-label settings can track and consume this margin configuration when rendering.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartStackLabelSettings chartStackLabelSettings)
            {
                _stackLabel = chartStackLabelSettings;
            }

            _stackLabel?.UpdateStackLabelProperties(nameof(ChartStackLabelSettings.Margin), this);
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter updates and notifies the renderer if any margin values changed since the previous render.
        /// </summary>
        /// <remarks>
        /// Re-renders are minimized by diffing <see cref="Left"/>, <see cref="Right"/>, <see cref="Top"/>, and <see cref="Bottom"/>.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            _stackLabel?.UpdateStackLabelProperties(nameof(ChartStackLabelSettings.Margin), this);

            if (_previousBottom != Bottom || _previousTop != Top || _previousRight != Right || _previousLeft != Left)
            {
                _previousBottom = Bottom;
                _previousTop = Top;
                _previousRight = Right;
                _previousLeft = Left;

                _stackLabel?.Renderer?.StackLabelValueChanged();
            }
        }

        /// <summary>
        /// Releases references to allow the component to be collected and avoid memory leaks.
        /// </summary>
        /// <remarks>
        /// Clears references to the parent and child content; no unmanaged resources are held.
        /// </remarks>
        protected override ValueTask DisposeAsyncCore()
        {
            ComponentDispose();
            return base.DisposeAsyncCore();
        }
        internal void ComponentDispose()
        {
            _stackLabel = null;
            ChildContent = null!;
        }
        #endregion
    }
}
