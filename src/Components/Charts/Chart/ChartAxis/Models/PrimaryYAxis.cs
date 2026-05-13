using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents the primary Y-axis configuration in a <see cref="SfChart"/> component.
    /// </summary>
    public class ChartPrimaryYAxis : ChartAxis
    {
        /// <summary>
        /// Gets the name of the primary Y-axis, initializing it to the default constant if not already set.
        /// </summary>
        /// <returns>
        /// The name of the axis. Returns <see cref="Constants.PrimaryYAxis"/> if <see cref="ChartAxis.Name"/> 
        /// is <see langword="null"/> or empty; otherwise returns the configured name.
        /// </returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public override string GetName()
        {
            Name = string.IsNullOrEmpty(Name) ? Constants.PrimaryYAxis : Name;
            return Name;
        }
    }
}
