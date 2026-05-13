namespace Syncfusion.Blazor.Toolkit.Calendars.Interfaces
{
    /// <summary>
    /// Defines the contract for components that handle mask placeholder operations in calendar components.
    /// This interface provides methods to update child component properties related to mask placeholders.
    /// </summary>
    /// <remarks>
    /// This interface is used internally by calendar components to manage mask placeholder behavior
    /// and ensure consistent property updates across child components.
    /// </remarks>
    internal interface IMaskPlaceholder
    {
        /// <summary>
        /// Updates the properties of child components based on the provided mask placeholder value.
        /// </summary>
        /// <param name="maskPlaceholderValue">
        /// The mask placeholder value used to update child component properties.
        /// This object contains the configuration or data needed to properly set up the mask placeholder behavior.
        /// </param>
        /// <remarks>
        /// This method is called internally when the mask placeholder configuration changes
        /// and needs to be propagated to child components for consistent behavior.
        /// </remarks>
        /// <exclude/>
        void UpdateChildProperties(object maskPlaceholderValue);
    }
}
