namespace Syncfusion.Blazor.Toolkit.Spinner
{
    /// <summary>
    /// Contains the public method implementations for the <see cref="SfSpinner"/> component.
    /// </summary>
    /// <remarks>
    /// This partial class exposes public methods for programmatic control of the spinner's visibility and behavior.
    /// </remarks>
    public partial class SfSpinner : SfBaseComponent
    {
        #region Public Methods

        /// <summary>
        /// Displays the spinner visually on the screen.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method displays the spinner, making it visible to the user. It is useful for indicating a loading or processing state in the application.
        /// Calling this method triggers the <see cref="OnOpen"/> event, which can be used to cancel the operation.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the <see cref="ShowAsync"/> and <see cref="HideAsync"/> methods.
        /// <code><![CDATA[
        /// <SfSpinner @ref="Spinner" />
        /// <button @onclick="ProcessData">Process Data</button>
        /// @code {
        ///     private SfSpinner? Spinner;
        ///     private async Task ProcessData()
        ///     {
        ///         await Spinner.ShowAsync();
        ///         // Simulate long-running operation
        ///         await Task.Delay(2000);
        ///         await Spinner.HideAsync();
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task ShowAsync()
        {
            await ShowInternalAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Hides the spinner, removing it from the visible display.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method hides the spinner, removing it from display. It should be called after the associated loading or processing task is complete.
        /// Calling this method triggers the <see cref="OnClose"/> event, which can be used to cancel the operation.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the <see cref="ShowAsync"/> and <see cref="HideAsync"/> methods.
        /// <code><![CDATA[
        /// <SfSpinner @ref="Spinner" />
        /// <button @onclick="ProcessData">Process Data</button>
        /// @code {
        ///     private SfSpinner? Spinner;
        ///     private async Task ProcessData()
        ///     {
        ///         await Spinner.ShowAsync();
        ///         // Simulate long-running operation
        ///         await Task.Delay(2000);
        ///         await Spinner.HideAsync();
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        public async Task HideAsync()
        {
            await HideInternalAsync().ConfigureAwait(false);
        }

        #endregion
    }
}
