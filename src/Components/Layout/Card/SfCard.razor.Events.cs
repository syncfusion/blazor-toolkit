using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfCard
    {
        #region Events

        /// <summary>
        /// Gets or sets the callback invoked when a clickable card is activated.
        /// </summary>
        /// <value>An <see cref="EventCallback{TValue}"/> of <see cref="MouseEventArgs"/>.</value>
        /// <remarks>
        /// Fires for a pointer click, for Enter, and for Space, only while <see cref="IsClickable"/> is <see langword="true"/> and <see cref="IsLoading"/> is <see langword="false"/>.
        /// Clicks that originate inside <see cref="SfCardActions"/> do not reach this callback. Keyboard activation passes a <see cref="MouseEventArgs"/> with <c>Detail</c> 0.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCard IsClickable="true" OnClick="@Open" />
        /// @code {
        ///     private void Open(MouseEventArgs args)
        ///     {
        ///         // Navigate to the details page
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        #endregion
    }
}
