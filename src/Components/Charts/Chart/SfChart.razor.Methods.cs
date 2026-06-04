using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    public partial class SfChart
    {
        #region Public Methods

        /// <summary>
        /// The method is used to re-render the chart.
        /// </summary>
        /// <param name="shouldAnimate">Specifies whether the chart should animate during refresh.</param>
        /// <returns>An asynchronous task.</returns>
        /// <remarks>
        /// This method is used to update the chart with new data and settings.
        /// </remarks>
        public async Task RefreshAsync(bool shouldAnimate = true)
        {
            try
            {
                _shouldAnimateSeries = shouldAnimate;
                _isRefreshing = true;
                await GetElementOffsetAsync("getRefreshElementBoundsById").ConfigureAwait(false);
                CalculateAvailableSize();
                await SetSvgDimensionAsync(Constants.SetSvgDimensions).ConfigureAwait(true);
                await UpdateDataAsync().ConfigureAwait(true);
                await GetRemoteDataAsync().ConfigureAwait(true);
                InitModules();
                InitPrivateModules();
                Prerender();
                await RefreshChartAsync().ConfigureAwait(true);
                ApplyZoomkit();
                await UpdateDatalabelTemplateAsync().ConfigureAwait(true);
                UpdateClientSideScrollbar();
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, Constants.SetHighlightSelectionOptions, new object[] { _dataId, GetSelectionHighlightOptions() }).ConfigureAwait(true);
                await PerformDelayAnimationAsync().ConfigureAwait(false);
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// The method is used to refresh the chart data live updates.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void RefreshLiveData()
        {
            _ = RefreshChartAsync();
        }

        /// <summary>
        /// The method is used to add the series collection in chart.
        /// </summary>
        /// <param name="seriesCollection">Specifies the chart series collection.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task AddSeriesAsync(List<ChartSeries> seriesCollection)
        {
            if (seriesCollection is null)
            {
                return;
            }

            if (seriesCollection.Count > 0)
            {
                foreach (ChartSeries series in seriesCollection)
                {
                    series.Container = this;
                    series.RendererType = ChartSeriesRenderer.GetRendererType(series.Type);
                    await series.UpdateSeriesDataAsync().ConfigureAwait(false);
                    if (series.Marker is not null && series.Marker.Visible && _shouldRenderMarker)
                    {
                        series.Marker.RendererType = typeof(ChartMarkerRenderer);
                    }

                    if (series.Marker?.DataLabel is not null && series.Marker.DataLabel.Visible && _shouldRenderDataLabel)
                    {
                        series.Marker.DataLabel.RendererType = typeof(ChartDataLabelRenderer);
                    }

                    if (series.LastDataLabel is not null && series.LastDataLabel.ShowLabel && _shouldRenderDataLabel)
                    {
                        series.LastDataLabel.RendererType = typeof(LastDataLabelRenderer);
                    }

                    AddSeries(series);
                    foreach (ChartTrendline trendline in series.Trendlines)
                    {
                        trendline.Parent = new ChartTrendlines();
                        trendline.Parent.Series = series;
                        trendline.InitTrendline();
                    }
                }
                if (_seriesContainer is not null)
                {
                    _seriesContainer.RendererShouldRender = true;
                }
                _seriesContainer?.Prerender();
                await ProcessOnLayoutChangeAsync().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// The method is used to remove the specific series in chart.
        /// </summary>
        /// <param name="index">Specifies the index of the series collection.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void RemoveSeries(int index)
        {
            if (_seriesContainer?.Elements.Count > 0)
            {
                ChartSeries series = _seriesContainer.Elements[index] as ChartSeries ?? null!;
                _seriesContainer.RemoveElement(series ?? null!);
            }
        }

        /// <summary>
        ///  The method is used to clear the series in chart.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ClearSeries()
        {
            if (_seriesContainer is not null)
            {
                foreach (ChartSeries series in _seriesContainer.Elements.ToList())
                {
                    _seriesContainer.RemoveElement(series);
                }
            }

            _ = ProcessOnLayoutChangeAsync();
        }

        /// <summary>
        /// Displays a tooltip based on the data points or coordinates of the <see cref="SfChart">Chart</see>.
        /// </summary>
        /// <param name="x">Specifies the x-value of the point or x-coordinate on the chart. </param>
        /// <param name="y">Specifies the y-value of the point or y-coordinate on the chart. </param>
        /// <param name="isPoint">Specifies whether x and y are data point or chart coordinates. (Optional, default value is true.)</param>
        /// <remarks>
        /// If the parameter values for 'x' and 'y' are not provided, this method will be unable to determine the position of the tooltip and will not display any tooltip. It is essential to provide valid 'x' and 'y' values for this method to work as expected.
        /// </remarks>
        /// <example>
        /// This example demonstrates how to display a tooltip.
        /// <code>
        /// <![CDATA[
        /// <SfButton OnClick="ShowTooltip">ShowTooltip</SfButton>
        /// <SfChart @ref="Chart">
        /// ...
        /// </SfChart>
        /// @code {
        ///     public SfChart Chart;
        ///     void ShowTooltip()
        ///     {
        ///         Chart.ShowTooltipAsync("Gold", 40);
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        public async Task ShowTooltipAsync(object x, double y, bool isPoint = true)
        {
            if (_isChartFirstRender && _isScriptLoaded)
            {
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "showTooltip", new object[] { x, y, isPoint, this._dataId }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Hides the tooltip for data points.
        /// </summary>
        /// <example>
        /// This example demonstrates how to hide the tooltip.
        /// <code>
        /// <![CDATA[
        /// <SfButton OnClick="HideTooltip">Hide Tooltip</SfButton>
        /// <SfChart @ref="Chart">
        ///     <!-- Chart configuration -->
        /// </SfChart>
        /// @code {
        ///     public SfChart Chart;
        ///     void HideTooltip()
        ///     {
        ///         Chart.HideTooltipAsync();
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        public async Task HideTooltipAsync()
        {
            if (_isChartFirstRender && _isScriptLoaded)
            {
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "hideTooltip", new object[] { this._dataId }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Displays a crosshair based on the coordinates of the <see cref="SfChart" >Chart</see>.
        /// </summary>
        /// <param name="x">Specifies the x-coordinate on the chart. </param>
        /// <param name="y">Specifies the y-coordinate on the chart. </param>
        /// <remarks>
        /// If the parameter values for 'x' and 'y' are not provided, this method will be unable to determine the position of the crosshair, and the crosshair will not be displayed. It is essential to provide valid 'x' and 'y' coordinates for this method to work as expected and show the crosshair at the specified location.
        /// </remarks>
        /// <example>
        /// This example demonstrates how to display a crosshair.
        /// <code>
        /// <![CDATA[
        /// <SfButton OnClick="ShowCrosshair">Show Crosshair</SfButton>
        /// <SfChart @ref="Chart">
        /// ...
        /// </SfChart>  
        /// @code {
        ///     public SfChart Chart;
        ///     void ShowCrosshair()
        ///     {
        ///         Chart.ShowCrosshairAsync(100, 40);
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        public async Task ShowCrosshairAsync(double x, double y)
        {
            if (_isChartFirstRender && _isScriptLoaded)
            {
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "showCrosshair", new object[] { x, y, this._dataId }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Hides the crosshair for the chart.
        /// </summary>
        /// <example>
        /// This example demonstrates how to hide the crosshair.
        /// <code>
        /// <![CDATA[
        /// <SfButton OnClick="HideCrosshair">Hide Crosshair</SfButton>
        /// <SfChart @ref="Chart">
        ///     <!-- Chart configuration -->
        /// </SfChart>
        /// @code {
        ///     public SfChart Chart;
        ///
        ///     void HideCrosshair()
        ///     {
        ///         Chart.HideCrosshairAsync();
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        public async Task HideCrosshairAsync()
        {
            if (_isChartFirstRender && _isScriptLoaded)
            {
                await InvokeVoidAsync(_chartJsModule!, _chartJsInProcessModule!, "hideCrosshair", new object[] { this._dataId }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Sorts the chart data based on the property name and direction specified.  
        /// </summary>  
        /// <param name="propertyName"> Specifies property name for the sorting. </param> 
        /// <param name="direction"> Specifies sorting direction for the chart data .</param> 
        /// <example>  
        /// This example shows how to sort a chart by the "Y" property in descending order. 
        /// <code>
        /// <![CDATA[  
        /// <SfButton OnClick="SortChart" IsToggle="true" IsPrimary="true">SortChart</SfButton>  
        /// <SfChart @ref=”Chart” DataSource=”Data”>  
        /// <ChartSorting PropertyName ="X" Direction ="ListSortDirection.Ascending "/> 
        /// …  
        /// </SfChart>
        /// ]]>
        /// @code{  
        /// public SfChart Chart; 
        /// public void SortChart() 
        /// { 
        ///     Chart.Sort(‘Y’, ListSortDirection.Descending);  
        /// }
        /// }  
        /// </code>  
        /// </example> 
        public void Sort(string propertyName, Data.ListSortDirection direction)
        {
            _sorting.SetSortKeyAndDirection(propertyName, direction);
            _ = RefreshChartAsync();
        }

        /// <summary>
        /// Clears the sorting applied to the chart.  
        /// </summary>  
        /// <example>
        /// <code>
        /// <![CDATA[  
        /// <SfButton OnClick="ClearSort" IsToggle="true" IsPrimary="true">ClearSort</SfButton>  
        /// <SfChart @ref=”Chart” DataSource=”Data”>  
        /// <ChartSorting PropertyName ="X" Direction ="ListSortDirection.Ascending "/> 
        /// …  
        /// </SfChart>
        /// ]]>
        /// @code {  
        /// public SfChart Chart; 
        /// public void ClearSort() 
        /// { 
        ///     Chart.ClearSort();
        /// }
        /// }  
        /// </code>  
        /// </example> 
        public void ClearSort()
        {
            _sorting.ClearSortKey();
            _ = RefreshChartAsync();
        }

        /// <summary>
        /// Clears the selection applied to the chart.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfButton OnClick="ClearSelection" IsToggle="true" IsPrimary="true">ClearSelection</SfButton>
        /// <SfChart @ref ="Chart">
        ///    <ChartSelectedDataIndexes>
        ///        @foreach(SelectedDataPoint data in SelectionData)
        ///        {
        ///            <ChartSelectedDataIndex Series="@data.seriesIndex" Point="@data.pointIndex">
        ///            </ChartSelectedDataIndex>
        ///        }
        ///    </ChartSelectedDataIndexes>
        ///    ...
        /// </SfChart>
        /// @code
        /// {
        ///    public SfChart Chart;
        ///    public List<SelectedDataPoint> SelectionData = new List<SelectedDataPoint>
        ///    {
        ///        new SelectedDataPoint { seriesIndex = 0, pointIndex = 1 },
        ///        new SelectedDataPoint { seriesIndex = 1, pointIndex = 3 }
        ///    };
        ///    public class SelectedDataPoint
        ///    {
        ///        public int seriesIndex { get; set; }
        ///        public int pointIndex { get; set; }
        ///    }
        ///    public void ClearSelection()
        ///    {
        ///        Chart.ClearSelection();
        ///    }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This method clears all selections applied to the chart for all types of <see cref="ChartSelectionMode"/>.
        /// </remarks>
        public void ClearSelection()
        {
            if (_selectionModule is not null && _selection.SelectionMode != ChartSelectionMode.None)
            {
                _selectionModule.PerformClearSelection();
            }
        }

        /// <summary>
        /// Prevents the Chart render. This method will internally sets value to be returned from ShouldRender method.
        /// </summary>
        /// <param name="preventRender">Default value is true. Once PreventRender(true) called, component won't re-render until PreventRender(false) called.</param>
        public void PreventRender(bool preventRender = true) => _render.ShouldChartRender = !preventRender;

        #endregion
    }
}
