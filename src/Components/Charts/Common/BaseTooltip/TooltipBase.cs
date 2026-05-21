using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Provides shared tooltip data and lifecycle management for chart components.
    /// </summary>
    /// <remarks>
    /// This internal helper centralizes tooltip text formatting data and JS runtime reference.
    /// Designed for low-allocation patterns by reusing lists and clearing them on dispose.
    /// </remarks>
    public class TooltipBase : ChartData
    {
        #region Properties

        /// <summary>
        /// Gets the list of formatted tooltip text lines. Never null for the lifetime of the instance.
        /// </summary>
        /// <value>A list of formatted strings.</value>
        internal List<string> FormattedText { get; set; } = [];

        /// <summary>
        /// Gets or sets the raw tooltip text values prior to formatting.
        /// </summary>
        /// <value>A list of raw text strings; may be <c>null</c> when unavailable.</value>
        internal List<string>? Text { get; set; }

        /// <summary>
        /// Gets or sets the header text for the tooltip.
        /// </summary>
        /// <value>The header string; may be <c>null</c>.</value>
        internal string? HeaderText { get; set; }

        /// <summary>
        /// Gets the JS runtime used for potential JS interop operations.
        /// </summary>
        /// <value>An <see cref="IJSRuntime"/> instance provided by the owning chart.</value>
        internal IJSRuntime JSRuntime { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of <see cref="TooltipBase"/>.
        /// </summary>
        /// <param name="sfchart">The owning chart instance used to derive services such as JS runtime.</param>
        internal TooltipBase(SfChart sfchart) : base(sfchart)
        {
            if (sfchart.JSRuntime != null)
            {
                JSRuntime = sfchart.JSRuntime;
            }
        }
        #endregion

        /// <summary>
        /// Releases managed resources used by the tooltip helper.
        /// </summary>
        internal override void Dispose()
        {
            base.Dispose();
            FormattedText = null!;
            Text = null;
        }
    }
}