using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders and manages a single row of the chart, including its associated axes and computed dimensions.
    /// </summary>
    public class ChartRowRenderer : ChartRenderer, IChartElementRenderer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the underlying chart row element represented by this renderer.
        /// </summary>
        public ChartRow? ChartRow { get; set; }

        /// <summary>
        /// Gets or sets the cascading container responsible for managing row renderers.
        /// </summary>
        [CascadingParameter]
        internal ChartRendererContainer? Container { get; set; }

        /// <summary>
        /// Gets the collection of axes assigned to this row.
        /// </summary>
        internal ObservableCollection<ChartAxis> Axes { get; set; } = [];

        /// <summary>
        /// Gets or sets the computed height of this row after layout calculations.
        /// </summary>
        internal double ComputedHeight { get; set; }

        /// <summary>
        /// Gets or sets the computed top offset of this row after layout calculations.
        /// </summary>
        internal double ComputedTop { get; set; }

        /// <summary>
        /// Gets or sets the list of calculated sizes for near-side (left/bottom) axes.
        /// </summary>
        internal List<double>? NearSizes { get; set; }

        /// <summary>
        /// Gets or sets the list of calculated sizes for far-side (right/top) axes.
        /// </summary>
        internal List<double>? FarSizes { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes renderer and registers it with the owner's row container.
        /// </summary>
        protected override void OnInitialized()
        {
            Owner?._rowContainer?.AddRenderer(this);
            SvgRenderer = Owner?._svgRenderer ?? null;
            if (ChartRow is { })
            {
                ChartRow.Renderer = this;
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Computes row height and top offset based on chart rectangle.
        /// </summary>
        /// <param name="rect">Total chart rectangle.</param>
        /// <param name="remainingHeight">Remaining height available to distribute.</param>
        /// <param name="rowTop">Current top coordinate used for stacking.</param>
        /// <param name="isLast">Indicates whether this is not the final row (affects distribution).</param>
        public void HandleChartSizeChange(Rect rect, double remainingHeight, double rowTop, bool isLast)
        {
            if (rect is null)
            {
                return;
            }

            double height = ChartRow is not null && ChartRow.Height.Contains('%', StringComparison.InvariantCulture)
                ? Math.Min(remainingHeight, rect.Height * double.Parse(ChartRow.Height.Replace("%", string.Empty, StringComparison.InvariantCulture), null) / 100)
                : Math.Min(remainingHeight, double.Parse(ChartRow?.Height.Replace("px", string.Empty, StringComparison.InvariantCulture) ?? null!, null));
            height = isLast ? height : remainingHeight;
            ComputedHeight = height;
            ComputedTop = rowTop - height;
        }

        /// <summary>
        /// Computes size contributions of an axis for this row.
        /// </summary>
        /// <param name="axis">Axis to compute.</param>
        /// <param name="scrollBarHeight">Scrollbar height to include when appropriate.</param>
        public void ComputeSize(ChartAxis axis, double scrollBarHeight = 0)
        {
            double width = 0;
            double innerPadding = 5;
            if (axis is not null && axis.Visible)
            {
                width += (axis.Renderer?.FindTickSize() ?? 0)
                    + ((axis.ScrollbarSettings.Position is ScrollbarPosition.Right or ScrollbarPosition.Left) ? 0 : scrollBarHeight)
                    + (axis.Renderer?.FindLabelSize(innerPadding, axis.Renderer.Rect.Height) ?? 0)
                    + (axis.LineStyle.Width * 0.5);
            }

            if (axis is not null && axis.IsAxisOpposedPosition)
            {
                FarSizes?.Add(width);
            }
            else
            {
                NearSizes?.Add(width);
            }
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

        #region Internal Methods

        /// <summary>
        /// Applies default renderer values by delegating to the cascading container.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {
            Container?.SetDefaultRendererContainerValues();
        }
        #endregion
    }
}
