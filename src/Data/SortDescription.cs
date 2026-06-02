namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// Defines the direction and the property name to be used as the criteria for sorting a collection.
    /// </summary>
    /// <param name="propertyName">The name of the property to sort the list by.</param>
    /// <param name="direction">The sort order.</param>
    public struct SortDescription(string propertyName, ListSortDirection direction) : IEquatable<SortDescription>
    {

        /// <summary>
        /// Compares two System.ComponentModel.SortDescription objects for value inequality.
        /// </summary>
        /// <param name="sd1">The first instance to compare.</param>
        /// <param name="sd2">The second instance to compare.</param>
        /// <returns>bool.</returns>
        public static bool operator !=(SortDescription sd1, SortDescription sd2)
        {
            return !sd1.Equals(sd2);
        }

        /// <summary>
        /// Compares two System.ComponentModel.SortDescription objects for value equality.
        /// </summary>
        /// <param name="sd1">The first instance to compare.</param>
        /// <param name="sd2">The second instance to compare.</param>
        /// <returns>true.</returns>
        public static bool operator ==(SortDescription sd1, SortDescription sd2)
        {
            return sd1.Equals(sd2);
        }

        /// <summary>
        /// Gets or sets a value that indicates whether to sort in ascending or descending
        ///     order.
        /// </summary>
        public ListSortDirection Direction { readonly get; set; } = direction;

        /// <summary>
        /// Gets or sets the property name being used as the sorting criteria.
        /// </summary>
        public string PropertyName { get; set; } = propertyName;

        /// <summary>
        /// Compares the specified instance and the current instance of System.ComponentModel.SortDescription
        ///     for value equality.
        /// </summary>
        /// <param name="obj">The System.ComponentModel.SortDescription instance to compare.</param>
        /// <returns>true.</returns>
        public override readonly bool Equals(object obj)
        {
            return obj is SortDescription other && Equals(other);
        }
        /// <summary>
        /// Compares the specified instance and the current instance of System.ComponentModel.SortDescription
        ///     for value equality.
        /// </summary>
        /// <param name="other">The System.ComponentModel.SortDescription instance to compare.</param>
        /// <returns>true.</returns>
        public readonly bool Equals(SortDescription other)
        {
            return string.Equals(PropertyName, other.PropertyName, StringComparison.Ordinal) && Direction == other.Direction;
        }

        /// <summary>
        /// Returns the hash code.
        /// </summary>
        /// <returns>int.</returns>
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(PropertyName, Direction);
        }
    }
}
