using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfSidebar
    {
        private const string CONTAINER = "e-sidebar-container e-control e-lib";
        private const string CONTAINER_OPEN = "e-sidebar-container-open";
        private const string CONTAINER_LEFT = "e-sidebar-container-left";
        private const string CONTAINER_RIGHT = "e-sidebar-container-right";
        private const string CONTAINER_OVERLAY = "e-sidebar-container-overlay";
        private const string CONTAINER_PUSH = "e-sidebar-container-push";
        private const string CONTAINER_SHRINK = "e-sidebar-container-shrink";
        private const string CONTAINER_MINI = "e-sidebar-container-mini";
        private const string CONTAINER_FIXED = "e-sidebar-container-fixed";
        private const string BACKDROP = "e-sidebar-backdrop";
        private const string SIDEBAR = "e-sidebar";
        private const string OPEN = "e-sidebar-open";
        private const string LEFT = "e-sidebar-left";
        private const string RIGHT = "e-sidebar-right";
        private const string OVERLAY = "e-sidebar-overlay";
        private const string PUSH = "e-sidebar-push";
        private const string SHRINK = "e-sidebar-shrink";
        private const string MINI = "e-sidebar-mini";
        private const string OCCUPIED_NONE = "0px";
        private const string OCCUPIED_MINI = "var(--e-sidebar-mini-width)";
        private const string OCCUPIED_OPEN = "var(--e-sidebar-width)";
        private const string HOVER = "e-sidebar-hover";
        private const string RTL = "e-rtl";
        private const string NO_ANIMATION = "e-sidebar-no-animation";

        private const string DEFAULT_WIDTH = "280px";
        private const string DEFAULT_MINI_WIDTH = "64px";
        private const int DEFAULT_DURATION = 300;
        private readonly string _generatedId = SfBaseUtils.GenerateID("e-sidebar");
        private const double SWIPE_THRESHOLD = 50;

        private const string ROLE_DIALOG = "dialog";
        private const string ROLE_NAVIGATION = "navigation";
        private const string POINTER_MOUSE = "mouse";
        private const string ESCAPE_KEY = "Escape";
        private const string POINTER_TOUCH = "touch";
        private const string ARIA_LABELLEDBY = "aria-labelledby";

        private const string LABEL_KEY = "Sidebar_Label";
        private const string OPENED_KEY = "Sidebar_Opened";
        private const string CLOSED_KEY = "Sidebar_Closed";
        private const string LABEL_FALLBACK = "Sidebar";
        private const string OPENED_FALLBACK = "Sidebar opened";
        private const string CLOSED_FALLBACK = "Sidebar closed";
        private const string NOTES_KEY = "Sidebar_Notes";
        private const string NOTES_FALLBACK = "Collapsible side panel. Use the menu control to open or close it.";

        private const string MEDIA_QUERY_SCRIPT = "./_content/Syncfusion.Blazor.Toolkit/scripts/sidebar.js";
        private const string MEDIA_QUERY_CHANGED = "OnMediaQueryChanged";

        private static readonly HashSet<string> _reservedAttributes = new(StringComparer.OrdinalIgnoreCase)
        {
            "id",
            "class",
            "role",
            "tabindex",
            "aria-modal",
            "aria-hidden",
            "aria-label",
            "aria-labelledby",
            "aria-describedby",
        };
    }
}
