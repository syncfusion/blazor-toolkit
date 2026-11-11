namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Provides data for the <see cref="SfSidebar.OnOpened"/> and <see cref="SfSidebar.OnClosed"/> events.
    /// </summary>
    /// <remarks>
    /// The event is raised after the open state has been applied and after <see cref="SfSidebar.IsOpenChanged"/>.
    /// <see cref="IsOpen"/> and <see cref="Mode"/> are the contract members. <see cref="Trigger"/> is retained for compatibility.
    /// </remarks>
    public sealed class SidebarEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the open state that was applied.
        /// </summary>
        /// <value><see langword="true"/> when the sidebar opened; otherwise, <see langword="false"/>.</value>
        public required bool IsOpen { get; init; }

        /// <summary>
        /// Gets the effective mode that was active when the change was applied.
        /// </summary>
        /// <value>
        /// <see cref="SidebarMode.Overlay"/> while a responsive sidebar is below its breakpoint; otherwise the configured <see cref="SfSidebar.Mode"/>.
        /// The default is <see cref="SidebarMode.Overlay"/>.
        /// </value>
        public SidebarMode Mode { get; init; }

        /// <summary>
        /// Gets the interaction that requested the change.
        /// </summary>
        /// <value>A compatibility member. The contract does not require it.</value>
        public required SidebarTrigger Trigger { get; init; }
    }
}
