using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides options to customize the crosshair line.
    /// </summary>
    public class ChartCrosshairLine : ChartDefaultBorder
    {
        #region Fields

        private string? _color;
        private double _width;

        #endregion

        #region Properties

        [CascadingParameter]
        private ChartCrosshairSettings? Parent { get; set; }

        /// <summary>
        /// Gets or sets the color of the crosshair line. Accepts values in hex or rgba as valid CSS color strings.
        /// </summary>
        /// <value>
        /// A string specifying the color of the crosshair line. The default value is inherited from <see cref="ChartDefaultBorder.Color"/>.
        /// </value>
        /// <remarks>
        /// Use this to match the crosshair line to your app's color scheme.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartCrosshairSettings Enable="true">
        ///     <ChartCrosshairLine Width="2" Color="blue"></ChartCrosshairLine>
        /// </ChartCrosshairSettings>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override string Color
        {
            get => _color ?? base.Color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    _isPropertyChanged = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the crosshair line in pixels.
        /// </summary>
        /// <value>
        /// A double value representing the width of the crosshair line. If not set, the default is inherited from <see cref="ChartDefaultBorder.Width"/>.
        /// </value>
        /// <remarks>
        /// Adjust the width to control the line thickness.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartCrosshairSettings Enable="true">
        ///     <ChartCrosshairLine Width="2" Color="blue"></ChartCrosshairLine>
        /// </ChartCrosshairSettings>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public override double Width
        {
            get => _width > 0 ? _width : base.Width;
            set
            {
                if (_width != value)
                {
                    _width = value;
                    _isPropertyChanged = true;
                }
            }
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the component and notifies the parent crosshair settings of the line configuration.
        /// </summary>
        /// <remarks>
        /// Uses a safe fire-and-forget pattern to avoid blocking the synchronous lifecycle method.
        /// </remarks>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Tracker is ChartCrosshairSettings crosshairSettings)
            {
                Parent = crosshairSettings;
            }

            // Safe fire-and-forget to avoid sync-over-async in a sync lifecycle method.
            _ = NotifyParentLineChangedAsync();
        }

        /// <summary>
        /// Applies parameter updates and ensures the parent chart is updated when ready.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);

            if (Parent?.Chart is not null && Parent.Chart._isChartFirstRender)
            {
                await Parent.UpdateCrosshairPropertiesAsync("Line", this).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Disposes the component and releases references.
        /// </summary>
        /// <remarks>Clears references to avoid memory retention after component disposal.</remarks>
        protected override ValueTask DisposeAsyncCore()
        {
            Parent = null;
            ChildContent = null!;
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Notifies the parent crosshair settings that the line configuration has changed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task NotifyParentLineChangedAsync()
        {
            if (Parent is not null)
            {
                await Parent.UpdateCrosshairPropertiesAsync("Line", this).ConfigureAwait(false);
            }
        }

        #endregion
    }
}