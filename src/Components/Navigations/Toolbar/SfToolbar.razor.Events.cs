using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfToolbar
    {
        #region Events

        /// <summary>
        /// Gets or sets the callback raised when any item is clicked.
        /// </summary>
        /// <value>An <see cref="EventCallback{TValue}"/> of <see cref="ToolbarItemClickEventArgs"/>.</value>
        /// <remarks>
        /// It fires after the item-level <c>OnClick</c> and still fires when <see cref="ToolbarItemClickEventArgs.Cancel"/> is set.
        /// Setting <see cref="ToolbarItemClickEventArgs.Cancel"/> stops the default action of the item.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfToolbar AriaLabel="Main" OnItemClick="@OnClick">
        ///     <ToolbarItem Id="save" Text="Save" />
        /// </SfToolbar>
        /// @code {
        ///     private void OnClick(ToolbarItemClickEventArgs args) => Console.WriteLine(args.ItemId);
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<ToolbarItemClickEventArgs> OnItemClick { get; set; }

        /// <summary>
        /// Gets or sets the callback raised when the overflow menu opens.
        /// </summary>
        /// <value>An <see cref="EventCallback"/> with no arguments (ADR-009).</value>
        /// <remarks>It fires once for each closed-to-open transition, after the popup state has changed.</remarks>
        [Parameter]
        public EventCallback OnOverflowOpen { get; set; }

        /// <summary>
        /// Gets or sets the callback raised when the overflow menu closes.
        /// </summary>
        /// <value>An <see cref="EventCallback"/> with no arguments (ADR-009).</value>
        /// <remarks>It fires once for each open-to-closed transition. A second close does not fire it again.</remarks>
        [Parameter]
        public EventCallback OnOverflowClose { get; set; }

        #endregion
    }
}
