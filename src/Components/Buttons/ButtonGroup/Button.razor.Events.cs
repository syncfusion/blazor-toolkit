using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Buttons
{
    public partial class Button
    {
        #region Events

        /// <exclude />
        /// <summary>
        /// Gets or sets a callback that fires when the <see cref="Selected"/> property changes.
        /// </summary>
        /// <remarks>
        /// Enables two-way binding via <c>@bind-Selected</c>.
        /// </remarks>
        [Parameter]
        public EventCallback<bool> SelectedChanged { get; set; }

        #endregion
    }
}
