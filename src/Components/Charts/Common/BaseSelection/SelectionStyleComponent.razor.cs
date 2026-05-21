using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Component that exposes selection style pattern data for charts.
    /// </summary>
    public partial class SelectionStyleComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        /// <value>Unique identifier for this component instance. Default: <c>string.Empty</c>.</value>
        [Parameter]
        public string ComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of pattern options supplied to this component.
        /// </summary>
        /// <value>A list of <see cref="PatternOptions"/> representing patterns. Default: an empty list.</value>
        [Parameter]
        public List<PatternOptions> GivenPattern { get; set; } = new List<PatternOptions>();
        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the internal pattern list and triggers a UI refresh.
        /// </summary>
        /// <param name="data">The pattern options to apply. If <c>null</c> or empty, no update occurs.</param>
        internal void DrawPattern(List<PatternOptions> data)
        {
            if (data is not null && data.Count > 0)
            {
                GivenPattern = data;
                _ = InvokeAsync(StateHasChanged);
            }
        }
        #endregion
    }
}