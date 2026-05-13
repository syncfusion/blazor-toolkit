using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents the Syncfusion Switch component's members including constants, fields, and parameters.
    /// </summary>
    public partial class SfSwitch<TChecked>
    {
        #region Members

        /// <summary>
        /// Gets or sets the label text displayed when the Switch is in the ON state.
        /// </summary>
        /// <value>The label shown for the ON state. Default: <c>null</c>.</value>
        [Parameter]
        public string? OnLabel { get; set; }

        /// <summary>
        /// Gets or sets the label text displayed when the Switch is in the OFF state.
        /// </summary>
        /// <value>The label shown for the OFF state. Default: <c>null</c>.</value>
        [Parameter]
        public string? OffLabel { get; set; }

        #endregion
    }
}