using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// A class used in the <see cref="SfDialog"/> to configure the custom position within the document or target.
    /// </summary>
    /// <remarks>
    /// The <see cref="DialogPositionData"/> class allows you to specify custom positioning for the <see cref="SfDialog"/> component.
    /// You can set both X and Y coordinates to position the dialog at a specific location within the viewport or relative to a target element.
    /// The position values can be specified as pixels, percentages, or predefined keywords like "center", "top", "bottom", "left", and "right".
    /// </remarks>
    /// <example>
    /// In the following example, position the dialog at the center horizontally and top vertically.
    /// <code><![CDATA[
    /// @using Syncfusion.Blazor.Toolkit.Popups
    /// <SfDialog Width="500px" @bind-Visible="Visibility">
    ///   <DialogTemplates>
    ///     <Content>
    ///         <p>
    ///            Dialog content
    ///           </p>
    ///       </Content>
    ///   </DialogTemplates>
    ///   <DialogPositionData X="center" Y="top">
    ///   </DialogPositionData>
    ///   </SfDialog>
    ///  @code {
    ///   private bool Visibility { get; set; } = true;
    ///  }
    /// ]]></code>
    /// </example>
    public class DialogPositionData : SfBaseComponent
    {
        [CascadingParameter]
        internal SfDialog? Parent { get; set; }

        private const string DATA_ID = "dataId";
        private const string POSITION = "position";
        private const string CHANGE_POSITION = "changePosition";

        private string? _x;
        private string? _y;

        /// <summary>
        /// Gets or sets the horizontal position (X-coordinate) for positioning the <see cref="SfDialog"/>.
        /// </summary>
        /// <value>
        /// A string that specifies the horizontal position of the dialog. This can be:
        /// <list type="bullet">
        /// <item><description>A numeric value in pixels (e.g., "100", "200px")</description></item>
        /// <item><description>A percentage value (e.g., "50%")</description></item>
        /// <item><description>Predefined keywords: "left", "center", "right"</description></item>
        /// </list>
        /// The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// The X property controls the horizontal positioning of the dialog within its container or viewport.
        /// When using keywords, "left" positions the dialog at the left edge, "center" centers it horizontally,
        /// and "right" positions it at the right edge. Numeric values position the dialog at the specified distance
        /// from the left edge of the container.
        /// </remarks>
        [Parameter]
        public string X { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the vertical position (Y-coordinate) for positioning the <see cref="SfDialog"/>.
        /// </summary>
        /// <value>
        /// A string that specifies the vertical position of the dialog. This can be:
        /// <list type="bullet">
        /// <item><description>A numeric value in pixels (e.g., "100", "200px")</description></item>
        /// <item><description>A percentage value (e.g., "50%")</description></item>
        /// <item><description>Predefined keywords: "top", "center", "bottom"</description></item>
        /// </list>
        /// The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// The Y property controls the vertical positioning of the dialog within its container or viewport.
        /// When using keywords, "top" positions the dialog at the top edge, "center" centers it vertically,
        /// and "bottom" positions it at the bottom edge. Numeric values position the dialog at the specified distance
        /// from the top edge of the container.
        /// </remarks>
        [Parameter]
        public string Y { get; set; } = string.Empty;

        /// <summary>
        /// Method invoked when the component is ready to start, typically after the component has been rendered to the page.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called once when the component is initialized. It registers the position data with the parent dialog
        /// and stores the initial X and Y values for change detection in subsequent parameter updates.
        /// </remarks>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
            Parent?.UpdateChildProperties(POSITION, this);
            _x = X;
            _y = Y;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent and any time afterwards when the parent renders.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called whenever the component's parameters are updated. It compares the new X and Y values with the previous ones
        /// and triggers a position change in the dialog if any differences are detected. This ensures the dialog's position is updated
        /// dynamically when the position parameters change.
        /// </remarks>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);
            if (!string.Equals(X, _x, StringComparison.Ordinal) || !string.Equals(Y, _y, StringComparison.Ordinal))
            {
                _x = X;
                _y = Y;
                if (Parent is not null)
                {
                    await InvokeVoidAsync(Parent._dialogJsModule, Parent._dialogJsInProcessModule, CHANGE_POSITION, new Dictionary<string, object>
                    {
                        { DATA_ID, Parent._dataId },
                        { POSITION, Parent.GetPosition() }
                    }).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DialogPositionData"/> and optionally releases the managed resources.
        /// </summary>
        /// <remarks>
        /// This method is called by the public <see cref="SfBaseComponent.DisposeAsync()"/> method and the finalizer.
        /// The method also clears the reference to the parent dialog to prevent memory leaks.
        /// </remarks>
        protected override ValueTask DisposeAsyncCore()
        {
            Parent = null;
            return base.DisposeAsyncCore();
        }
    }
}