using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders and manages column layout, sizing, and axis positioning for chart components.
    /// </summary>
    public class ChartColumnRenderer : ChartRenderer, IChartElementRenderer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the chart column element managed by this renderer.
        /// </summary>
        /// <value>The <see cref="ChartColumn"/> instance; <c>null</c> if not assigned.</value>
        public ChartColumn? ChartColumn { get; set; }

        /// <summary>
        /// Gets the cascading chart renderer container that owns this column renderer.
        /// </summary>
        [CascadingParameter]
        internal ChartRendererContainer? Container { get; set; }

        /// <summary>
        /// Gets the collection of axes assigned to this column.
        /// </summary>
        internal ObservableCollection<ChartAxis> Axes { get; set; } = [];

        /// <summary>
        /// Gets or sets the computed width of this column after layout calculations.
        /// </summary>
        internal double ComputedWidth { get; set; }

        /// <summary>
        /// Gets or sets the computed left offset of this column after layout calculations.
        /// </summary>
        internal double ComputedLeft { get; set; }

        /// <summary>
        /// Gets the near-side axis sizes (inner margins).
        /// </summary>
        internal List<double>? NearSizes { get; set; }

        /// <summary>
        /// Gets the far-side axis sizes (outer/opposed margins).
        /// </summary>
        internal List<double>? FarSizes { get; set; }

        /// <summary>
        /// Gets the column-left sizing adjustments.
        /// </summary>
        internal List<double>? ColumnLeftSizes { get; set; }

        /// <summary>
        /// Gets the column-right sizing adjustments.
        /// </summary>
        internal List<double>? ColumnRightSizes { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes renderer and registers it with the owner's column container.
        /// </summary>
        protected override void OnInitialized()
        {
            Owner?._columnContainer?.AddRenderer(this);
            SvgRenderer = Owner?._svgRenderer ?? null;
            if (ChartColumn is { })
            {
                ChartColumn.Renderer = this;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Applies default renderer values by delegating to the container.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {
            Container?.SetDefaultRendererContainerValues();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Computes axis related size for this column.
        /// </summary>
        /// <param name="axis">Axis to measure.</param>
        /// <param name="scrollBarHeight">Scrollbar height to include when appropriate.</param>
        public void ComputeSize(ChartAxis axis, double scrollBarHeight = 0)
        {
            double height = 0;
            if (axis?.Renderer is not null && axis.Visible)
            {
                height += axis.Renderer.FindTickSize()
                    + ((axis.ScrollbarSettings.Position is ScrollbarPosition.Top or ScrollbarPosition.Bottom) ? 0 : scrollBarHeight)
                    + axis.Renderer.FindLabelSize(5, axis.Renderer.Rect.Width)
                    + (axis.LineStyle.Width * 0.5);
            }

            if (axis is not null && axis.IsAxisOpposedPosition)
            {
                FarSizes?.Add(height);
            }
            else
            {
                NearSizes?.Add(height);
            }
        }

        /// <summary>
        /// Handles chart size changes and computes column dimensions accordingly.
        /// </summary>
        /// <param name="rect">The new chart rectangle bounds.</param>
        /// <param name="remainingWidth">The available width remaining for column layout.</param>
        /// <param name="columnLeft">The left offset position for this column.</param>
        /// <param name="isLast">Indicates whether this is the last column in the layout.</param>
        public void HandleChartSizeChange(Rect rect, double remainingWidth, double columnLeft, bool isLast)
        {
            if (rect is null)
            {
                return;
            }
            double width = (ChartColumn is not null && ChartColumn.Width.Contains('%', StringComparison.InvariantCulture)) ? Math.Min(remainingWidth, rect.Width * double.Parse(ChartColumn.Width.Replace("%", string.Empty, StringComparison.InvariantCulture), null) / 100) : Math.Min(remainingWidth, Convert.ToInt32(ChartColumn?.Width, null));
            width = isLast ? width : remainingWidth;
            ComputedWidth = width;
            ComputedLeft = columnLeft;
        }

        /// <summary>
        /// Handles generic chart size changes (base interface requirement).
        /// </summary>
        /// <param name="rect">The new chart rectangle bounds.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
        }

        /// <summary>
        /// Handle layout changes. Kept for interface compatibility.
        /// </summary>
        public void HandleLayoutChange()
        {
        }

        /// <summary>
        /// Marks the renderer for re-rendering.
        /// </summary>
        public void InvalidateRender()
        {
            StateHasChanged();
        }
        #endregion
    }
}
