using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Manages tooltip data state and template rendering for chart components.
    /// </summary>
    /// <remarks>
    /// This internal component is responsible for collecting tooltip data from visible chart series,
    /// rendering tooltip templates, and communicating with the JavaScript interop layer via the owning chart instance.
    /// </remarks>
    public partial class TooltipData
    {
        #region Fields
        private bool _sharedTemplate;
        private ChartTooltipInfo _chartTooltipInfo = new();
        private readonly List<ChartTooltipInfo> _chartTooltipInfos = [];
        private SfChart? _owner;
        private bool _isNeedRender;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the component element id.
        /// </summary>
        [Parameter]
        public string ID { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional attributes applied to the root element.
        /// </summary>
        [Parameter]
        public Dictionary<string, object> Attributes { get; set; } =  new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the template RenderFragment provided by the owner chart.
        /// </summary>
        [Parameter]
        public RenderFragment<object> GivenContent { get; set; } = null!;
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Invoked after component render. Triggers chart interop to set tooltip data if required.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            if (_owner is not null && _owner._isScriptLoaded && _owner._isScriptCalled && _isNeedRender)
            {
                await _owner.SetTooltipDataAsync().ConfigureAwait(true);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Builds the RenderFragment that wraps the provided template content in a hidden container.
        /// </summary>
        /// <returns>A <see cref="RenderFragment"/> that renders the tooltip template.</returns>
        private RenderFragment TemplateElements()
        {
            return builder =>
            {
                int seq = 0;
                if (GivenContent is not null)
                {
                    builder.OpenElement(seq++, "div");
                    builder.AddAttribute(seq++, "id", "tooltip_template");

                    if (_sharedTemplate)
                    {
                        builder.AddContent(3, GivenContent(_chartTooltipInfos));
                    }
                    else
                    {
                        builder.AddContent(3, GivenContent(_chartTooltipInfo));
                    }
                    builder.CloseElement();
                }
                _ = (_owner?.SetTooltipStyleAsync(ID).ConfigureAwait(true));
            };
        }

        /// <summary>
        /// Creates a tooltip information object from the given point data.
        /// </summary>
        /// <param name="point">The data point from which to extract tooltip information.</param>
        /// <param name="financialPoint">The financial point data, if applicable; otherwise <see langword="null"/>.</param>
        /// <returns>A <see cref="ChartTooltipInfo"/> object populated with point and financial data.</returns>
        private static ChartTooltipInfo CreateTooltipInfo(Point point, FinancialPoint? financialPoint)
        {
            return new ChartTooltipInfo
            {
                X = point.X,
                Y = point.Y,
                Text = point.Text,
                PointX = point.X,
                PointY = point.Y,
                PointIndex = point.Index,
                High = financialPoint?.High ?? null!,
                Low = financialPoint?.Low ?? null!,
                Open = financialPoint?.Open ?? null!,
                Close = financialPoint?.Close ?? null!,
                Volume = financialPoint?.Volume ?? null!,
            };
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates attributes required for the tooltip and schedules a render.
        /// </summary>
        /// <param name="chart">The owning chart instance.</param>
        /// <param name="points">List of serialized point data used as attributes.</param>
        internal void UpdateAttribute(SfChart chart, List<string> points)
        {
            _owner = chart;
            ID = chart.ID + "_tooltip_data";
            _isNeedRender = true;
            _sharedTemplate = false;
            GetTemplateOptions(chart);

            if (points.Count > 0)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    Attributes["tooltip-data" + i] = points[i];
                }
            }
            _ = InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Collects a representative tooltip data sample from each visible series.
        /// </summary>
        /// <remarks>
        /// Samples the first point from each visible series to provide template preview data.
        /// Handles both standard and financial chart types, extracting their specific properties.
        /// </remarks>
        /// <param name="chart">The chart instance from which to collect tooltip data.</param>
        internal void GetTemplateOptions(SfChart chart)
        {
            _chartTooltipInfos.Clear();

            if (chart._tooltip.Template is null)
            {
                GivenContent = _ => builder => { };
                return;
            }

            _sharedTemplate = chart._tooltip.Shared;

            for (int i = 0; i < chart._visibleSeriesRenderers.Count; i++)
            {
                ChartSeries series = chart._visibleSeriesRenderers[i].Series ?? null!;

                if (!series.Visible || !(series.Renderer.Points?.Count > 0))
                {
                    continue;
                }

                Point point = series.Renderer.Points[0];
                FinancialPoint? financialPoint = point as FinancialPoint;
                ChartTooltipInfo tooltipInfo = CreateTooltipInfo(point, financialPoint);

                if (!_sharedTemplate)
                {
                    _chartTooltipInfo = tooltipInfo;
                }
                else
                {
                    _chartTooltipInfos.Add(tooltipInfo);
                }
            }

            GivenContent = chart._tooltip.Template ?? (_ => builder => { });
        }

        /// <summary>
        /// Removes attributes and prevents further JS notifications until needed.
        /// </summary>
        internal void RemoveAttribute()
        {
            if (Attributes.Count > 0)
            {
                Attributes.Clear();
            }
            _isNeedRender = false;
            _ = InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
