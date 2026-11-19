using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfSidebar
    {
        /// <summary>
        /// Gets or sets a value indicating whether the sidebar is open.
        /// </summary>
        /// <value><see langword="true"/> if the sidebar is open; otherwise, <see langword="false"/>. The default is <see langword="false"/>.</value>
        /// <remarks>
        /// Use <c>@bind-IsOpen</c> for two-way binding. A change made by the user, by a public method or by a gesture is reported through
        /// <see cref="IsOpenChanged"/> only when the value actually changes. A change supplied by the parent through this parameter does not raise
        /// <see cref="OnOpened"/> or <see cref="OnClosed"/>.
        /// </remarks>
        [Parameter]
        public bool IsOpen { get; set; }

        /// <summary>
        /// Gets or sets how the sidebar is displayed relative to the target content.
        /// </summary>
        /// <value>A <see cref="SidebarMode"/> value. The default is <see cref="SidebarMode.Overlay"/>.</value>
        [Parameter]
        public SidebarMode Mode { get; set; } = SidebarMode.Overlay;

        /// <summary>
        /// Gets or sets the edge at which the sidebar is docked.
        /// </summary>
        /// <value>A <see cref="SidebarPosition"/> value. The default is <see cref="SidebarPosition.Start"/>.</value>
        /// <remarks>
        /// <see cref="SidebarPosition.Start"/> and <see cref="SidebarPosition.End"/> follow the global right-to-left option of
        /// <see cref="GlobalOptions"/>; <see cref="SidebarPosition.Left"/> and <see cref="SidebarPosition.Right"/> do not.
        /// </remarks>
        [Parameter]
        public SidebarPosition Position { get; set; } = SidebarPosition.Start;

        /// <summary>
        /// Gets or sets the width of the open sidebar.
        /// </summary>
        /// <value>A CSS length such as <c>280px</c>. The default is <c>280px</c>.</value>
        /// <remarks>
        /// <see langword="null"/>, empty or an unsafe value (one containing declaration separators, quotes, comments or <c>url(</c>) falls back to the default.
        /// </remarks>
        [Parameter]
        public string? Width { get; set; } = DEFAULT_WIDTH;

        /// <summary>
        /// Gets or sets a value indicating whether a narrow rail stays visible while the sidebar is closed.
        /// </summary>
        /// <value><see langword="true"/> to keep the mini rail visible; otherwise, <see langword="false"/>. The default is <see langword="false"/>.</value>
        [Parameter]
        public bool EnableMini { get; set; }

        /// <summary>
        /// Gets or sets the width of the mini rail.
        /// </summary>
        /// <value>A CSS length such as <c>64px</c>. The default is <c>64px</c>.</value>
        /// <remarks>
        /// <see langword="null"/>, empty or an unsafe value falls back to the default. The value is used only when <see cref="EnableMini"/> is <see langword="true"/>.
        /// </remarks>
        [Parameter]
        public string? MiniWidth { get; set; } = DEFAULT_MINI_WIDTH;

        /// <summary>
        /// Gets or sets a value indicating whether a backdrop is rendered behind an open overlay sidebar.
        /// </summary>
        /// <value><see langword="true"/> to render the backdrop; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        /// <remarks>
        /// The backdrop is only rendered in <see cref="SidebarMode.Overlay"/>. When it is rendered the sidebar is exposed as a modal dialog.
        /// </remarks>
        [Parameter]
        public bool ShowBackdrop { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a pointer activation of the backdrop closes an open overlay sidebar.
        /// </summary>
        /// <value><see langword="true"/> to close when the backdrop is activated; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        /// <remarks>Applies only to a backdrop click. Escape, gestures and the public methods are not affected.</remarks>
        [Parameter]
        public bool CloseOnOverlayClick { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether pressing Escape closes an open sidebar.
        /// </summary>
        /// <value><see langword="true"/> to close on Escape; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        [Parameter]
        public bool CloseOnEscape { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a horizontal touch or pen swipe opens or closes the sidebar.
        /// </summary>
        /// <value><see langword="true"/> to enable swipe gestures; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        /// <remarks>Mouse drags are ignored so that selecting text never changes the sidebar state.</remarks>
        [Parameter]
        public bool EnableGestures { get; set; } = true;

        /// <summary>
        /// Gets or sets additional CSS classes applied to the sidebar element.
        /// </summary>
        /// <value>A space-separated list of class names. <see langword="null"/> or empty adds no classes. The default is an empty string.</value>
        [Parameter]
        public string? CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the HTML identifier of the sidebar panel.
        /// </summary>
        /// <value>
        /// The identifier rendered on the panel. <see langword="null"/> or empty generates a stable identifier for this instance.
        /// </value>
        /// <remarks>
        /// The identifier is rendered on <c>div.e-sidebar</c> so a consumer toggle can point <c>aria-controls</c> at it. It is never rendered on the layout host.
        /// A value supplied after the first render replaces the generated identifier.
        /// </remarks>
        [Parameter]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the ARIA role of the sidebar element.
        /// </summary>
        /// <value>
        /// An ARIA role. <see langword="null"/> or empty selects the role automatically: <c>dialog</c> while a modal backdrop is displayed, otherwise <c>navigation</c>.
        /// </value>
        [Parameter]
        public string? Role { get; set; }

        /// <summary>
        /// Gets or sets the accessible name of the sidebar element.
        /// </summary>
        /// <value>The text exposed through <c>aria-label</c>. <see langword="null"/> or empty uses the localized default "Sidebar".</value>
        [Parameter]
        public string? AriaLabel { get; set; }

        /// <summary>
        /// Gets or sets a CSS media query that opens the sidebar while the query matches and closes it when the query stops matching.
        /// </summary>
        /// <value>
        /// A media query such as <c>(max-width: 768px)</c>, or <see langword="null"/> when media-query behaviour is disabled.
        /// The default is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// <para>
        /// The query is evaluated in the browser after the first render and has no effect during static server rendering or in a test host
        /// that does not implement <c>matchMedia</c>. A match opens the sidebar; a non-match closes it. Both changes raise
        /// <see cref="OnOpened"/> or <see cref="OnClosed"/> with <see cref="SidebarTrigger.Api"/> after the state is applied.
        /// </para>
        /// <para>
        /// A parent that sets <see cref="IsOpen"/> still wins: the next media-query notification is applied only when it differs from the
        /// current state. Clearing this parameter stops watching the previous query.
        /// </para>
        /// </remarks>
        [Parameter]
        public string? MediaQuery { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sidebar closes when <see cref="Microsoft.AspNetCore.Components.NavigationManager.LocationChanged"/> is raised while it is open.
        /// </summary>
        /// <value><see langword="true"/> to close on navigation; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        /// <remarks>
        /// The component subscribes only when <see cref="IsOpenChanged"/> has a delegate (two-way binding).
        /// <see langword="false"/> never subscribes. The subscription is removed on disposal.
        /// </remarks>
        [Parameter]
        public bool CloseOnNavigate { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the sidebar adapts to the viewport width.
        /// </summary>
        /// <value><see langword="true"/> to enable breakpoint behavior; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        /// <remarks>
        /// Below <see cref="Breakpoint"/> the effective mode is <see cref="SidebarMode.Overlay"/> and the sidebar starts closed.
        /// <see langword="false"/> keeps <see cref="Mode"/> at every width. The viewport is read after the first interactive render; static rendering paints from the parameters only.
        /// </remarks>
        [Parameter]
        public bool Responsive { get; set; } = true;

        /// <summary>
        /// Gets or sets the viewport width below which a responsive sidebar uses mobile behavior.
        /// </summary>
        /// <value>A <see cref="SidebarBreakpoint"/> value. The default is <see cref="SidebarBreakpoint.Md"/> (<c>768px</c>).</value>
        [Parameter]
        public SidebarBreakpoint Breakpoint { get; set; } = SidebarBreakpoint.Md;

        /// <summary>
        /// Gets or sets a value indicating whether pointer hover expands a collapsed mini rail on a desktop viewport.
        /// </summary>
        /// <value><see langword="true"/> to expand the mini rail while the pointer is over it; otherwise, <see langword="false"/>. The default is <see langword="false"/>.</value>
        /// <remarks>
        /// Applies only when <see cref="EnableMini"/> is <see langword="true"/>, the sidebar is closed, <see cref="IsEnabled"/> is <see langword="true"/> and the pointer is a mouse or pen.
        /// The expansion does not change <see cref="IsOpen"/> and raises no events. In Push and Shrink it still reserves the expanded width, matching an explicit open.
        /// Keyboard users expand the rail through their toggle or the public methods.
        /// </remarks>
        [Parameter]
        public bool OpenMiniOnHover { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user interactions can open or close the sidebar.
        /// </summary>
        /// <value><see langword="true"/> to allow user interactions; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        /// <remarks>
        /// <see langword="false"/> ignores backdrop activation, Escape, hover expansion and swipe gestures. <see cref="OpenAsync"/>, <see cref="CloseAsync"/>,
        /// <see cref="ToggleAsync"/> and a new <see cref="IsOpen"/> value still apply, and so do navigation and breakpoint closing.
        /// </remarks>
        [Parameter]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the identifier of the element that labels the sidebar.
        /// </summary>
        /// <value>One or more space-separated element identifiers rendered as <c>aria-labelledby</c>, or <see langword="null"/>. The default is <see langword="null"/>.</value>
        /// <remarks>It takes precedence over <see cref="AriaLabel"/> for the accessible name. When it is set and <see cref="AriaLabel"/> is not, no default <c>aria-label</c> is rendered.</remarks>
        [Parameter]
        public string? AriaLabelledBy { get; set; }

        /// <summary>
        /// Gets or sets the duration of the open and close transition in milliseconds.
        /// </summary>
        /// <value>A non-negative number of milliseconds. The default is <c>300</c>. A negative value is treated as <c>0</c>.</value>
        /// <remarks>
        /// Exposed as <c>--e-sidebar-duration</c>. When animation is active, <see cref="OnOpened"/> and <see cref="OnClosed"/> are raised after this delay;
        /// a newer state change cancels the pending callback.
        /// </remarks>
        [Parameter]
        public int Duration { get; set; } = DEFAULT_DURATION;

        /// <summary>
        /// Gets or sets a value indicating whether open and close transitions are animated.
        /// </summary>
        /// <value><see langword="true"/> to animate; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        /// <remarks>
        /// <see langword="false"/>, the global animation option, and a <c>prefers-reduced-motion: reduce</c> setting all complete the transition immediately,
        /// and <see cref="OnOpened"/> and <see cref="OnClosed"/> are raised without waiting for <see cref="Duration"/>.
        /// </remarks>
        [Parameter]
        public bool EnableAnimation { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the panel is positioned against the viewport or against its layout host.
        /// </summary>
        /// <value>
        /// <see langword="true"/> for <c>position: fixed</c> against the viewport; <see langword="false"/> for <c>position: absolute</c> inside the layout host. The default is <see langword="true"/>.
        /// </value>
        /// <remarks>The backdrop follows the same reference: it covers the viewport when fixed and the layout host otherwise.</remarks>
        [Parameter]
        public bool Fixed { get; set; } = true;

        /// <summary>
        /// Gets or sets additional CSS classes applied to the backdrop element.
        /// </summary>
        /// <value>A space-separated list of class names, or <see langword="null"/>. The default is <see langword="null"/>.</value>
        /// <remarks>
        /// Applied to <c>div.e-sidebar-backdrop</c> only while the backdrop is rendered. It does not decide whether the backdrop renders and does not change its <c>aria-hidden</c> state.
        /// </remarks>
        [Parameter]
        public string? BackdropClass { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes applied to the sidebar element.
        /// </summary>
        /// <value>
        /// A dictionary of attribute names and values, or <see langword="null"/> when no additional attributes are set.
        /// The default is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// Attributes that the component owns are not overwritten: <c>id</c>, <c>class</c>, <c>role</c>, <c>tabindex</c>, <c>aria-modal</c>,
        /// <c>aria-hidden</c>, <c>aria-label</c>, <c>aria-labelledby</c> and <c>aria-describedby</c>. Event-handler attributes are ignored. Other names, including <c>data-*</c>, <c>title</c>
        /// and <c>style</c>, are rendered on <c>div.e-sidebar</c>.
        /// </remarks>
        [Parameter]
        public Dictionary<string, object>? HtmlAttributes { get; set; }

        /// <summary>
        /// Gets or sets the content rendered in the sidebar header.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/>. <see langword="null"/> renders no header region.</value>
        [Parameter]
        public RenderFragment? HeaderTemplate { get; set; }

        /// <summary>
        /// Gets or sets the main content of the sidebar.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/>. <see langword="null"/> renders an empty body region.</value>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the content rendered in the sidebar footer.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/>. <see langword="null"/> renders no footer region.</value>
        [Parameter]
        public RenderFragment? FooterTemplate { get; set; }

        /// <summary>
        /// Gets or sets the content that sits beside the sidebar and reacts to its state in <see cref="SidebarMode.Push"/> and <see cref="SidebarMode.Shrink"/>.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/>. <see langword="null"/> renders no target region.</value>
        [Parameter]
        public RenderFragment? TargetContent { get; set; }
    }
}
