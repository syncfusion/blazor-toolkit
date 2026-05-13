using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renderer responsible for displaying the "no data" template inside the chart.
    /// </summary>
    public class NoDataTemplateContainer : ChartRenderer
    {
        #region Fields
        private bool _hasValidData;
        private string _noDataStyle = string.Empty;
        private CultureInfo _culture = CultureInfo.InvariantCulture;
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Registers renderer with the chart render queue and assigns reference on the owner.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            if (Owner is { })
            {
                Owner._noDataTemplateContainer = this;
            }
        }

        /// <summary>
        /// Performs cleanup when component is disposed.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Recomputes whether the "no data" template should be shown and calculates its inline style.
        /// </summary>
        /// <summary>
        /// Recomputes whether the "no data" template should be shown and calculates its inline style.
        /// </summary>
        private void CreateNoDataTemplate()
        {
            _hasValidData = Owner is not null && Owner._visibleSeriesRenderers.All(x => x.Series?.DataSource?.Any() != true);

            if (!_hasValidData || Owner?.NoDataTemplate is null)
            {
                return;
            }

            Rect overlayRect = ComputeOverlayRect();
            _noDataStyle = BuildStyleFromRect(overlayRect);
        }

        /// <summary>
        /// Computes the overlay rectangle for the no-data template by accounting for borders and title/subtitle layout.
        /// </summary>
        private Rect ComputeOverlayRect()
        {
            double borderWidth = Owner?._chartBorderRenderer?.ChartBorder?.Width ?? 0;
            double width = Owner!.AvailableSize.Width - (2 * borderWidth);
            double height = Owner.AvailableSize.Height - (2 * borderWidth);
            Rect overlayRect = new(borderWidth, borderWidth, width, height);

            if (Owner?.Title is null || Owner._chartTitleRenderer is null)
            {
                return overlayRect;
            }

            double titleHeight = Owner._chartTitleRenderer.TitleSize.Height;
            double subTitleHeight = Owner._chartTitleRenderer.SubTitleSize.Height;
            double totalTitleHeight = titleHeight + subTitleHeight;
            double titleGap = ComputeTitleGap(titleHeight, subTitleHeight, Owner._chartTitleRenderer.Padding);

            ChartTitlePosition titlePosition = Owner._chartTitleRenderer.TitleStyle?.Position ?? ChartTitlePosition.Custom;

            double titleBlock = totalTitleHeight + titleGap;

            switch (titlePosition)
            {
                case ChartTitlePosition.Top:
                    overlayRect.Y += titleBlock;
                    overlayRect.Height -= titleBlock;
                    break;
                case ChartTitlePosition.Bottom:
                    overlayRect.Height -= titleBlock;
                    break;
                case ChartTitlePosition.Left:
                    overlayRect.X += titleBlock;
                    overlayRect.Width -= titleBlock;
                    break;
                case ChartTitlePosition.Right:
                    overlayRect.Width -= titleBlock;
                    break;
                case ChartTitlePosition.Custom:
                    break;
                default:
                    break;
            }

            return overlayRect;
        }

        /// <summary>
        /// Builds a style string from a rectangle using the existing culture for numeric formatting.
        /// </summary>
        private string BuildStyleFromRect(Rect rect)
        {
            return $"position: absolute; " +
                   $"top: {Convert.ToString(rect.Y, _culture)}px; " +
                   $"left: {Convert.ToString(rect.X, _culture)}px; " +
                   $"width: {Convert.ToString(rect.Width, _culture)}px; " +
                   $"height: {Convert.ToString(rect.Height, _culture)}px;";
        }

        /// <summary>
        /// Computes gap between title and subtitle/padding.
        /// </summary>
        private static double ComputeTitleGap(double titleHeight, double subTitleHeight, double padding)
        {
            if (titleHeight > 0 && subTitleHeight > 0)
            {
                return padding + 10;
            }
            else if (titleHeight > 0)
            {
                return padding;
            }

            return 0;
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Renders the "no data" template when applicable.
        /// </summary>
        /// <param name="builder">Render tree builder instance.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            if (_hasValidData && Owner?.NoDataTemplate is not null)
            {
                int seq = 0;
                if (builder is not null)
                {
                    builder.OpenElement(seq++, "div");
                    builder.AddAttribute(seq++, "id", "noDataTemplateContainer");
                    builder.AddAttribute(seq++, "style", _noDataStyle);
                    builder.AddContent(seq++, Owner.NoDataTemplate);
                    builder.CloseElement();
                }
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Handles size changes from the parent chart and marks renderer to re-render.
        /// </summary>
        /// <param name="rect">Updated chart bounds.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = true;
            CreateNoDataTemplate();
        }
        #endregion
    }
}