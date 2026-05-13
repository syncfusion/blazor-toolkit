namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// Provides common utility methods for the Blazor toolkit Calendars components.
    /// </summary>
    internal static class SfTimePickerUtils
    {
        /// <summary>
        /// Compare the two values and returns a value indicating whether one value is less than, equal to, or greater than the second value.
        /// </summary>
        /// <returns>Less than Zero - value1 is less than value2.</returns>
        /// <returns>Zero - Both values are equals.</returns>
        /// <returns>Greater than Zero - value1 is greater than value2.</returns>
        internal static int CompareValues<T>(T value1, T value2)
        {
            return Comparer<T>.Default.Compare(value1, value2);
        }
    }
}
