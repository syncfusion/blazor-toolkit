using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents SVG helper IDs for axis-related interactive elements used by chart components.
    /// </summary>
    public partial class SvgAxisGroup
    {
        #region Fields
        private string? _horizontalId;
        private string? _verticalId;
        private string? _axisTooltipId;
        private string? _groupId;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the base chart identifier used to build element IDs.
        /// </summary>
        /// <value>
        /// The chart identifier. If not provided or empty, a stable GUID will be generated during initialization.
        /// </value>
        [Parameter]
        public string ChartId { get; set; } = null!;
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes component state and computes stable SVG element IDs.
        /// </summary>
        protected override void OnInitialized()
        {
            _horizontalId = ChartId + "_HorizontalLine";
            _verticalId = ChartId + "_VerticalLine";
            _axisTooltipId = ChartId + "_crosshair_axis";
            _groupId = ChartId + "_UserInteraction";
        }
        #endregion
    }
}