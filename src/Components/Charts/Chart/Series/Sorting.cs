using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Data;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Configures the sorting option for the chart.
    /// </summary>
    /// <remarks>
    /// Use this component to sort the chart's bound data source by a specific property and direction
    /// before rendering the series. Runtime changes call <see cref="SfChart.RefreshChartAsync"/> when the chart
    /// is already rendered.
    /// </remarks>
    /// <example>
    /// This example shows how to sort a chart by the "Y" property in ascending order.
    /// <code>
    /// <![CDATA[
    /// <SfChart @ref="Chart" DataSource="Data">
    ///     <ChartSorting PropertyName="Y" Direction="ListSortDirection.Ascending" />
    ///     ...
    /// </SfChart>
    /// ]]>
    /// </code>
    /// </example>
    public class ChartSorting : ChartSubComponent
    {
        #region Fields

        private string _propertyName = string.Empty;
        private ListSortDirection _direction;

        #endregion

        #region Properties

        /// <summary>
        /// Specifies the direction in which to sort the chart data.
        /// </summary>
        [CascadingParameter]
        private SfChart? Parent { get; set; }


        /// <summary>
        /// Gets or sets the sorting direction for the chart data.
        /// </summary>
        /// <value>
        /// A <see cref="ListSortDirection"/> that specifies the sort order for the chart data.
        /// The possible values are:
        /// <list type="bullet">
        /// <item><description><c>Ascending</c>: Sorts the chart data from smallest value to largest (e.g., A to Z).</description></item>
        /// <item><description><c>Descending</c>: Sorts the chart data from largest value to smallest (e.g., Z to A).</description></item>
        /// </list>
        /// The default value is <see cref="ListSortDirection.Ascending"/>.
        /// </value>
        /// <remarks>
        /// This property defines the sorting behavior applied to the chart's data source.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartSorting PropertyName="X" Direction="ListSortDirection.Descending" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public ListSortDirection Direction { get; set; }

        /// <summary>
        /// Gets or sets the property name used as the sorting criterion.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> specifying the property name for sorting, such as the <c>X</c>, <c>Y</c>, <c>High</c>, <c>Low</c>, <c>Open</c>, <c>Close</c>, or <c>Size</c> fields in the chart's data source. The default value is <see cref="string.Empty"/>.
        /// </value>
        /// <remarks>
        /// This determines the field by which the chart data is sorted.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartSorting PropertyName="X" Direction="ListSortDirection.Ascending" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        [EditorRequired]
        public string PropertyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets the effective sort key (property name) currently in use by the chart.
        /// Reads from the backing field so imperative updates via <see cref="SetSortKeyAndDirection"/>
        /// or <see cref="ClearSortKey"/> are visible to renderers without mutating the
        /// <see cref="PropertyName"/> parameter.
        /// </summary>
        internal string SortKey => _propertyName;

        /// <summary>
        /// Gets the effective sort direction currently in use by the chart.
        /// Reads from the backing field so imperative updates via <see cref="SetSortKeyAndDirection"/>
        /// are visible to renderers without mutating the <see cref="Direction"/> parameter.
        /// </summary>
        internal ListSortDirection SortDirection => _direction;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the sorting component and registers it with the parent <see cref="SfChart"/>.
        /// </summary>       
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.ComponentModel.Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Parent is null)
            {
                return;
            }
            Parent._sorting = this;
            _propertyName = PropertyName;
            _direction = Direction;
        }

        /// <exclude />
        /// <summary>
        /// Handles parameter changes and refreshes the chart if necessary.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.ComponentModel.Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_propertyName != PropertyName || _direction != Direction)
            {
                _propertyName = PropertyName;
                _direction = Direction;
                if (Parent is not null && Parent.IsRendered)
                {
                    _ = Parent.RefreshChartAsync();
                }
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Sets the sort key and direction programmatically without triggering multiple refresh cycles.
        /// Only updates the backing fields — the <see cref="PropertyName"/> and <see cref="Direction"/>
        /// parameters are owned by the parent and are not mutated here.
        /// </summary>
        /// <param name="sortKey">The data member name used for sorting.</param>
        /// <param name="sortDirection">The desired <see cref="ListSortDirection"/>.</param>
        internal void SetSortKeyAndDirection(string sortKey, ListSortDirection sortDirection)
        {
            _propertyName = sortKey;
            _direction = sortDirection;
        }

        /// <summary>
        /// Clears the configured sort key.
        /// Only updates the backing field — the <see cref="PropertyName"/> parameter is owned by the
        /// parent and is not mutated here.
        /// </summary>
        internal void ClearSortKey()
        {
            _propertyName = string.Empty;
        }

        #endregion
    }
}
