using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Specifies how the <see cref="SfSidebar"/> is displayed relative to the main content.
    /// </summary>
    /// <remarks>
    /// <see cref="Overlay"/> is the zero value and therefore the runtime default of <see cref="SfSidebar.Mode"/>.
    /// </remarks>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SidebarMode
    {
        /// <summary>
        /// Displays the sidebar over the main content. A backdrop is rendered behind the open sidebar when <see cref="SfSidebar.ShowBackdrop"/> is <see langword="true"/>.
        /// </summary>
        [EnumMember(Value = "Overlay")]
        Overlay = 0,

        /// <summary>
        /// Displays the sidebar beside the main content and slides the content aside when the sidebar opens.
        /// </summary>
        [EnumMember(Value = "Push")]
        Push,

        /// <summary>
        /// Displays the sidebar beside the main content and resizes the content when the sidebar opens.
        /// </summary>
        [EnumMember(Value = "Shrink")]
        Shrink
    }

    /// <summary>
    /// Specifies the edge of the container at which the <see cref="SfSidebar"/> is docked.
    /// </summary>
    /// <remarks>
    /// <see cref="Start"/> is the zero value and therefore the runtime default of <see cref="SfSidebar.Position"/>.
    /// <see cref="Start"/> and <see cref="End"/> are logical edges that follow the global right-to-left setting.
    /// </remarks>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SidebarPosition
    {
        /// <summary>
        /// Docks the sidebar at the logical start edge: left in left-to-right layouts, right in right-to-left layouts.
        /// </summary>
        [EnumMember(Value = "Start")]
        Start = 0,

        /// <summary>
        /// Docks the sidebar at the logical end edge: right in left-to-right layouts, left in right-to-left layouts.
        /// </summary>
        [EnumMember(Value = "End")]
        End,

        /// <summary>
        /// Docks the sidebar at the left edge regardless of the layout direction.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,

        /// <summary>
        /// Docks the sidebar at the right edge regardless of the layout direction.
        /// </summary>
        [EnumMember(Value = "Right")]
        Right
    }

    /// <summary>
    /// Specifies the viewport width threshold below which a responsive <see cref="SfSidebar"/> switches to mobile behavior.
    /// </summary>
    /// <remarks>
    /// Thresholds are exact widths, not ranges. A viewport narrower than the threshold is mobile: the effective mode is <see cref="SidebarMode.Overlay"/>
    /// and the sidebar starts closed. <see cref="None"/>, <see cref="Xs"/> and <see cref="Always"/> never select mobile behavior.
    /// </remarks>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SidebarBreakpoint
    {
        /// <summary>
        /// Disables breakpoint-based adaptation. The configured <see cref="SfSidebar.Mode"/> is kept at every width.
        /// </summary>
        [EnumMember(Value = "None")]
        None = 0,

        /// <summary>
        /// Threshold <c>0px</c>. No viewport is narrower, so desktop behavior applies at every width.
        /// </summary>
        [EnumMember(Value = "Xs")]
        Xs,

        /// <summary>
        /// Threshold <c>576px</c>.
        /// </summary>
        [EnumMember(Value = "Sm")]
        Sm,

        /// <summary>
        /// Threshold <c>768px</c>. This is the default of <see cref="SfSidebar.Breakpoint"/>.
        /// </summary>
        [EnumMember(Value = "Md")]
        Md,

        /// <summary>
        /// Threshold <c>992px</c>.
        /// </summary>
        [EnumMember(Value = "Lg")]
        Lg,

        /// <summary>
        /// Threshold <c>1200px</c>.
        /// </summary>
        [EnumMember(Value = "Xl")]
        Xl,

        /// <summary>
        /// Threshold <c>1400px</c>.
        /// </summary>
        [EnumMember(Value = "Xxl")]
        Xxl,

        /// <summary>
        /// Applies desktop behavior at every width. The sidebar never falls back to <see cref="SidebarMode.Overlay"/>.
        /// </summary>
        [EnumMember(Value = "Always")]
        Always
    }

    /// <summary>
    /// Identifies the interaction that requested a <see cref="SfSidebar"/> open-state change.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SidebarTrigger
    {
        /// <summary>
        /// A public method such as <see cref="SfSidebar.OpenAsync"/>, <see cref="SfSidebar.CloseAsync"/> or <see cref="SfSidebar.ToggleAsync"/> was called,
        /// or the component changed its own state, for example on a responsive breakpoint crossing.
        /// </summary>
        [EnumMember(Value = "Api")]
        Api = 0,

        /// <summary>
        /// The user clicked the backdrop.
        /// </summary>
        [EnumMember(Value = "Backdrop")]
        Backdrop,

        /// <summary>
        /// The user pressed a keyboard key, such as Escape.
        /// </summary>
        [EnumMember(Value = "Keyboard")]
        Keyboard,

        /// <summary>
        /// The user performed a horizontal swipe gesture.
        /// </summary>
        [EnumMember(Value = "Gesture")]
        Gesture,

        /// <summary>
        /// The sidebar closed because the user navigated to another location.
        /// </summary>
        [EnumMember(Value = "Navigation")]
        Navigation
    }
}
