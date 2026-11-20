using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfSidebar
    {
        /// <summary>
        /// Gets or sets the callback invoked when <see cref="IsOpen"/> changes because of a user interaction or a public method.
        /// </summary>
        /// <value>An <see cref="EventCallback{TValue}"/> that receives the new open state.</value>
        /// <remarks>
        /// Enables <c>@bind-IsOpen</c>. It is invoked only when the value changes, and it is invoked before <see cref="OnOpened"/> or <see cref="OnClosed"/>.
        /// A parent-driven change of <see cref="IsOpen"/> does not invoke this callback.
        /// </remarks>
        [Parameter]
        public EventCallback<bool> IsOpenChanged { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked after the sidebar opens.
        /// </summary>
        /// <value>An <see cref="EventCallback{TValue}"/> that receives <see cref="SidebarEventArgs"/>.</value>
        /// <remarks>
        /// Raised after the open state is applied and after <see cref="IsOpenChanged"/>.
        /// </remarks>
        [Parameter]
        public EventCallback<SidebarEventArgs> OnOpened { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked after the sidebar closes.
        /// </summary>
        /// <value>An <see cref="EventCallback{TValue}"/> that receives <see cref="SidebarEventArgs"/>.</value>
        /// <remarks>
        /// Raised after the closed state is applied and after <see cref="IsOpenChanged"/>.
        /// </remarks>
        [Parameter]
        public EventCallback<SidebarEventArgs> OnClosed { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when the viewport crosses <see cref="Breakpoint"/> while <see cref="Responsive"/> is enabled.
        /// </summary>
        /// <value>An <see cref="EventCallback{TValue}"/> that receives <see langword="true"/> when the viewport is below the breakpoint and <see langword="false"/> at or above it.</value>
        /// <remarks>
        /// Independent of the open-state events. It is raised only when the viewport crosses the threshold after the first interactive render; the viewport found on load is not a crossing.
        /// It is raised before the <see cref="IsOpenChanged"/> and <see cref="OnClosed"/> pair of a close that the crossing causes.
        /// </remarks>
        [Parameter]
        public EventCallback<bool> OnBreakpointChanged { get; set; }
    }
}
