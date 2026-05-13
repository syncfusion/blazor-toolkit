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

        private string _propertyName = null!;
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
        /// A <see cref="string"/> specifying the property name for sorting, such as the <c>X</c>, <c>Y</c>, <c>High</c>, <c>Low</c>, <c>Open</c>, <c>Close</c>, or <c>Size</c> fields in the chart's data source. The default value is <c>null</c>.
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
        public string PropertyName { get; set; } = null!;

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
        /// </summary>
        /// <param name="sortKey">The data member name used for sorting.</param>
        /// <param name="sortDirection">The desired <see cref="ListSortDirection"/>.</param>
        internal void SetSortKeyAndDirection(string sortKey, ListSortDirection sortDirection)
        {
            _propertyName = sortKey;
            PropertyName = sortKey;
            _direction = sortDirection;
            Direction = sortDirection;
        }

        /// <summary>
        /// Clears the configured sort key.
        /// </summary>
        internal void ClearSortKey()
        {
            _propertyName = null!;
            PropertyName = null!;
        }

        #endregion
    }
}
