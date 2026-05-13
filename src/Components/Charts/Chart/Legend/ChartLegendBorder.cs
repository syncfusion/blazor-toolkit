using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the border of a chart legend.
    /// </summary>
    public class ChartLegendBorder : ChartDefaultBorder
    {
        #region Internal Properties

        /// <summary>
        /// Gets or sets the associated chart legend settings.
        /// </summary>
        internal ChartLegendSettings? ChartLegend { get; set; }
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Executes during component initialization.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Tracker is ChartLegendSettings legendSettings)
            {
                ChartLegend = legendSettings;
            }
            ChartLegend?.UpdateLegendProperties("Border", this);
        }

        /// <summary>
        /// Executes when parameters are set or updated.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Owner?._legendRenderer?.ProcessRenderQueue();
        }
        #endregion
    }
}
