namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Defines the axis requirements for Cartesian series renderers.
    /// </summary>
    /// <remarks>
    /// Implementers provide axis renderers, axis names and ranges required
    /// for mapping series data to chart coordinates. Implementations should
    /// update consumers by invoking <see cref="OnAxisChanged"/> when any axis
    /// related property changes.
    /// </remarks>
    public interface IRequireAxis
    {
        #region Properties

        /// <summary>
        /// Gets or sets the renderer for the X axis.
        /// </summary>
        /// <value>The <see cref="ChartAxisRenderer"/> responsible for rendering the X axis.</value>
        ChartAxisRenderer XAxisRenderer { get; set; }

        /// <summary>
        /// Gets or sets the renderer for the Y axis.
        /// </summary>
        /// <value>The <see cref="ChartAxisRenderer"/> responsible for rendering the Y axis.</value>
        ChartAxisRenderer YAxisRenderer { get; set; }

        /// <summary>
        /// Gets or sets the name of the X axis this series is bound to.
        /// </summary>
        /// <value>The axis name used to resolve the X axis renderer. Default: <c>null</c>.</value>
        string XAxisName { get; set; }

        /// <summary>
        /// Gets or sets the name of the Y axis this series is bound to.
        /// </summary>
        /// <value>The axis name used to resolve the Y axis renderer. Default: <c>null</c>.</value>
        string YAxisName { get; set; }

        /// <summary>
        /// Gets or sets the computed numeric range for the X axis.
        /// </summary>
        /// <value>The <see cref="DoubleRange"/> representing the X axis visible range.</value>
        DoubleRange XRange { get; set; }

        /// <summary>
        /// Gets or sets the computed numeric range for the Y axis.
        /// </summary>
        /// <value>The <see cref="DoubleRange"/> representing the Y axis visible range.</value>
        DoubleRange YRange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the series is visible.
        /// </summary>
        /// <value><see langword="true"/> if the series should be rendered; otherwise <see langword="false"/>.</value>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the minimum X value for the series (computed or forced).
        /// </summary>
        /// <value>The minimum X value.</value>
        double XMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum X value for the series (computed or forced).
        /// </summary>
        /// <value>The maximum X value.</value>
        double XMax { get; set; }

        /// <summary>
        /// Gets or sets the minimum Y value for the series (computed or forced).
        /// </summary>
        /// <value>The minimum Y value.</value>
        double YMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum Y value for the series (computed or forced).
        /// </summary>
        /// <value>The maximum Y value.</value>
        double YMax { get; set; }

        /// <summary>
        /// Gets or sets the X data points for the series.
        /// </summary>
        /// <value>A mutable list of X coordinate values. Implementers should avoid exposing internal lists directly when possible.</value>
        List<double> XData { get; set; }

        /// <summary>
        /// Gets or sets the Y data points for the series.
        /// </summary>
        /// <value>A mutable list of Y coordinate values. Implementers should avoid exposing internal lists directly when possible.</value>
        List<double> YData { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Notifies the implementer that axis-related properties have changed.
        /// </summary>
        void OnAxisChanged();

        #endregion
    }
}