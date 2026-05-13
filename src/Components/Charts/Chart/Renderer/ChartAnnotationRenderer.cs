using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders and manages chart annotations, including positioning and visibility.
    /// </summary>
    public class ChartAnnotationRenderer : ChartRenderer, IChartElementRenderer
    {
        #region Constants
        private const string ADAPTIVE_SMALL_ANNOTATION = "e-chart-small-annotation";
        private const string ADAPTIVE_MEDIUM_ANNOTATION = "e-chart-medium-annotation";
        private const string ADAPTIVE_LARGE_ANNOTATION = "e-chart-large-annotation";
        #endregion

        #region Fields
        private ChartEventLocation? _location;
        private object? _xCoordinate;
        private string? _yCoordinate;
        private CultureInfo _culture = CultureInfo.InvariantCulture;
        private bool _annotationVisibility = true;
        private string? _annotationId;
        private bool _shouldUpdateAnnotationStyle;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the annotation being rendered.
        /// </summary>
        /// <value>A <see cref="ChartAnnotation"/> instance, or <c>null</c> if not set.</value>
        internal ChartAnnotation? Annotation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the annotation has been rendered at least once.
        /// </summary>
        /// <value><see langword="true"/> if the annotation is rendered; otherwise <see langword="false"/>.</value>
        internal bool IsAnnotationRendered { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs component initialization and registers renderer with the owner.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Owner?._annotationContainer?.AddRenderer(this);
            if (Annotation is { })
            {
                Annotation.Renderer = this;
            }
        }

        /// <summary>
        /// Executes post-render JS interop for annotation positioning/visibility.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
            if (_shouldUpdateAnnotationStyle && Owner is not null && _location is not null && !string.IsNullOrEmpty(_annotationId))
            {
                _shouldUpdateAnnotationStyle = false;
                await InvokeVoidAsync(
                    Owner._chartJsModule,
                    Owner._chartJsInProcessModule,
                    "updateAnnotationStyle",
                    [.. new object[] { _annotationId, _location.Y.ToString(_culture), _location.X.ToString(_culture), _annotationVisibility }]).ConfigureAwait(true);
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Converts annotation position to pixel values relative to the chart area.
        /// </summary>
        /// <returns>A <see cref="ChartEventLocation"/> with pixel-based X and Y coordinates.</returns>
        /// <remarks>
        /// Uses the chart's initial rectangle and axis layout to calculate absolute pixel positions.
        /// Returns <c>NaN</c> coordinates if rendering context is not available.
        /// </remarks>
        private ChartEventLocation SetAnnotationPixelValue()
        {
            ChartEventLocation finalLocation = new(double.NaN, double.NaN);
            if (Owner?.InitialRect is not null && Owner._axisContainer?.AxisLayout is not null)
            {
                Rect? result = Annotation?.Region == Regions.Chart ? new Rect(0, 0, Owner.InitialRect.Width, Owner.InitialRect.Height) : Owner._axisContainer.AxisLayout.SeriesClipRect;
                if (result is not null)
                {
                    finalLocation.X = ChartHelper.StringToNumber(_xCoordinate?.ToString() ?? null!, result.Width) + result.X;
                    finalLocation.Y = ChartHelper.StringToNumber(_yCoordinate ?? null!, result.Height) + result.Y;
                }
            }
            return finalLocation;
        }

        /// <summary>
        /// Converts annotation position to point values based on axis coordinate systems.
        /// </summary>
        /// <returns>A <see cref="ChartEventLocation"/> with point-based X and Y coordinates.</returns>
        /// <remarks>
        /// Handles multiple axis types (Category, DateTime, Logarithmic, Numeric).
        /// Returns <c>NaN</c> coordinates if the point is outside the chart's visible range.
        /// </remarks>
        private ChartEventLocation SetAnnotationPointValue()
        {
            ChartEventLocation pointLocation = new(double.NaN, double.NaN);
            ChartAxisRenderer chartXAxisRenderer = null!, chartYAxisRenderer = null!;
            double pointXValue = double.NaN;

            if (Owner?._axisContainer is not null)
            {
                FindAxisRenderers(ref chartXAxisRenderer, ref chartYAxisRenderer, out pointXValue);
            }

            if (WithInChartArea(chartXAxisRenderer, chartYAxisRenderer, pointXValue))
            {
                pointLocation = ChartHelper.GetPoint(
                    chartXAxisRenderer.GetPointValue(pointXValue),
                    chartYAxisRenderer.GetPointValue(Convert.ToDouble(_yCoordinate, _culture)),
                    chartXAxisRenderer,
                    chartYAxisRenderer,
                    Owner?._requireInvertedAxis ?? false);

                pointLocation.X += Owner is not null && Owner._requireInvertedAxis ? chartYAxisRenderer.Rect.X : chartXAxisRenderer.Rect.X;
                pointLocation.Y += Owner is not null && Owner._requireInvertedAxis ? chartXAxisRenderer.Rect.Y : chartYAxisRenderer.Rect.Y;
            }
            return pointLocation;
        }

        /// <summary>
        /// Locates and assigns the appropriate X and Y axis renderers based on annotation axis names.
        /// </summary>
        private void FindAxisRenderers(ref ChartAxisRenderer chartXAxisRenderer, ref ChartAxisRenderer chartYAxisRenderer, out double pointXValue)
        {
            ValueType axisType;
            pointXValue = double.NaN;

            foreach (ChartAxisRenderer axis in Owner!._axisContainer!.Renderers.Cast<ChartAxisRenderer>())
            {
                if (Annotation?.XAxisName == axis.Axis?.Name || (Annotation?.XAxisName is null && axis.Axis?.GetName() == "PrimaryXAxis"))
                {
                    chartXAxisRenderer = axis;
                    axisType = chartXAxisRenderer.Axis?.ValueType ?? ValueType.Double;

                    if (axisType is ValueType.Category or ValueType.DateTimeCategory)
                    {
                        string label = axisType == ValueType.Category ? Convert.ToString(_xCoordinate, _culture) ?? string.Empty : Convert.ToString(ChartHelper.GetTime(Convert.ToDateTime(_xCoordinate, _culture)), _culture);
                        pointXValue = Array.IndexOf([.. chartXAxisRenderer.Labels], label);
                    }
                    else
                    {
                        pointXValue = axisType == ValueType.DateTime
                            ? ChartHelper.GetTime(Convert.ToDateTime(_xCoordinate, _culture))
                            : Convert.ToDouble(Convert.ToString(_xCoordinate, _culture), _culture);
                    }
                }
                else if (Annotation?.YAxisName == axis.Axis?.Name || (Annotation?.YAxisName is null && axis.Axis?.GetName() == "PrimaryYAxis"))
                {
                    chartYAxisRenderer = axis;
                }
            }
        }

        /// <summary>
        /// Determines whether the annotation point is within the visible chart area.
        /// </summary>
        private bool WithInChartArea(ChartAxisRenderer chartXAxisRenderer, ChartAxisRenderer chartYAxisRenderer, double pointXValue)
        {
            return chartXAxisRenderer is not null && chartYAxisRenderer is not null && ChartHelper.WithIn(chartXAxisRenderer.Axis?.ValueType == ValueType.Logarithmic
                ? ChartHelper.LogBase(pointXValue, chartXAxisRenderer.Axis.LogBase) : pointXValue, chartXAxisRenderer.VisibleRange)
                && ChartHelper.WithIn(chartYAxisRenderer.Axis?.ValueType == ValueType.Logarithmic
                ? ChartHelper.LogBase(Convert.ToDouble(_yCoordinate, _culture), chartYAxisRenderer.Axis.LogBase) : Convert.ToDouble(_yCoordinate, _culture), chartYAxisRenderer.VisibleRange);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds the annotation element into the render tree with appropriate styling, accessibility attributes, and content template.
        /// </summary>
        /// <param name="builder">The render tree builder instance.</param>
        /// <remarks>
        /// This method generates a <c>div</c> element positioned absolutely with CSS transforms.
        /// It includes ARIA labels for accessibility and visibility control based on animation state.
        /// </remarks>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (_location is not null && builder is not null)
            {
                int seq = 0;
                if (Owner is not null && !double.IsNaN(_location.X) && !double.IsNaN(_location.Y) && !(Owner._widthCategory == ChartWidthCategory.Small || Owner._heightCategory == ChartHeightCategory.Small))
                {
                    _annotationId = Owner.ID + "_Annotation_" + Owner._annotationContainer?.Renderers.IndexOf(this);
                    builder.OpenElement(seq++, "div");
                    builder.AddAttribute(seq++, "id", _annotationId);
                    builder.AddAttribute(seq++, "tabindex", Owner.Focusable && Owner._annotations.Focusable && Owner._annotationContainer?.Renderers.IndexOf(this) == 0 ? "0" : "");
                    builder.AddAttribute(seq++, "aria-label", !string.IsNullOrEmpty(Owner._annotations.AccessibilityDescription) ? Owner._annotations.AccessibilityDescription : ("The annotation index is " + Owner._annotationContainer?.Renderers.IndexOf(this) + ", positioned at x-coordinate " + _location.X.ToString(_culture) + " and y-coordinate " + _location.Y.ToString(_culture)));
                    builder.AddAttribute(seq++, "role", !string.IsNullOrEmpty(Owner._annotations.AccessibilityRole) ? Owner._annotations.AccessibilityRole : "region");

                    if (Owner._isAdaptiveRendering)
                    {
                        builder.AddAttribute(seq++, "class", Owner._widthCategory == ChartWidthCategory.Small || Owner._heightCategory == ChartHeightCategory.Small ? ADAPTIVE_SMALL_ANNOTATION : Owner._widthCategory == ChartWidthCategory.Medium || Owner._heightCategory == ChartHeightCategory.Medium ? ADAPTIVE_MEDIUM_ANNOTATION : ADAPTIVE_LARGE_ANNOTATION);
                    }

                    builder.AddContent(seq++, Annotation?.ContentTemplate);
                    builder.CloseElement();

                    _shouldUpdateAnnotationStyle = true;
                    RendererShouldRender = false;
                }
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Calculates rendering options for the annotation prior to render, including position and visibility state.
        /// </summary>
        /// <remarks>
        /// This method updates the annotation's location based on coordinate units (pixel or point),
        /// determines visibility based on active series animations, and marks the renderer as needing re-render.
        /// </remarks>
        internal void CalculateRenderingOption()
        {
            RendererShouldRender = true;
            if (!(Owner?._widthCategory == ChartWidthCategory.Small || Owner?._heightCategory == ChartHeightCategory.Small))
            {
                _xCoordinate = Annotation?.X;
                _yCoordinate = Annotation?.Y;
                _location = Annotation?.CoordinateUnits == Units.Pixel ? SetAnnotationPixelValue() : SetAnnotationPointValue();
                if (Owner?._visibleSeriesRenderers.Count > 0)
                {
                    _annotationVisibility = !Owner._visibleSeriesRenderers.Any(series => series?.Series is not null && series.Series.Animation.Enable) || IsAnnotationRendered;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Signal to re-render annotation. Kept for interface compatibility.
        /// </summary>
        public void InvalidateRender()
        {
        }

        /// <summary>
        /// Handle layout changes. Kept for interface compatibility.
        /// </summary>
        public void HandleLayoutChange()
        {
        }
        #endregion
    }
}
