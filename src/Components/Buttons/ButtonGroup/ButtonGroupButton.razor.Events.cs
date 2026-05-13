using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Buttons
{
    public partial class ButtonGroupButton
    {
        #region Events

        /// <exclude/>
        /// <summary>
        /// Gets or sets a callback that fires when the <see cref="Selected"/> property changes.
        /// </summary>
        [Parameter]
        public EventCallback<bool> SelectedChanged { get; set; }

        #endregion
    }
}
