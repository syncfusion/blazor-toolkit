namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Internal helper for crosshair-related orchestration across chart primitives.
    /// </summary>
    /// <remarks>
    /// Retained as a minimal placeholder to preserve API shape; no internal state is stored.
    /// </remarks>
    internal sealed class Crosshair
    {
        #region Lifecycle Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="Crosshair"/> class.
        /// </summary>
        /// <param name="sfChart">The owning <see cref="SfChart"/> instance.</param>
        internal Crosshair(SfChart sfChart)
        {
            // Intentionally no-op to avoid retaining references and to remove unused field allocations.
        }

        #endregion
    }
}
