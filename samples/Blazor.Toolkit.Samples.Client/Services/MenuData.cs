using System.Collections.Generic;

namespace Blazor.Toolkit.Samples.Client.Services
{
    public class MenuItemModel
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public string? IconCss { get; set; }
        public string? Url { get; set; }
        public bool Separator { get; set; }
        public Dictionary<string, object>? HtmlAttributes { get; set; }
    }
    public static class MenuData
    {
        // Shared lists used across multiple samples
        public static readonly List<MenuItemModel> DownloadMenu = new()
        {
            new () { Text = "CSV",   IconCss = "sb-icons e-export-csv" },
            new () { Text = "Excel", IconCss = "sb-icons e-export-excel" },
            new () { Text = "PDF",   IconCss = "sb-icons e-export-pdf" }
        };

        public static readonly List<MenuItemModel> UploadMenu = new()
        {
            new () { Text = "From file", IconCss = "sb-icons e-upload-1" },
            new () { Text = "From URL",  IconCss = "sb-icons e-link"   }
        };

        public static readonly List<MenuItemModel> MoreItems = new()
        {
            new () { Text = "Settings", IconCss = "sb-icons e-settings" },
            new () { Text = "Help", IconCss = "sb-icons e-comment-show" },
            new () { Text = "About" }
        };

        public static readonly List<MenuItemModel> SettingsMenu = new()
        {
            new () { Text = "Preferences", IconCss = "sb-icons e-settings" },
            new () { Text = "Account",     IconCss = "sb-icons e-user"     }
        };

        public static readonly List<MenuItemModel> BasicMenu = new()
        {
            new () { Text = "Edit",      IconCss = "sb-icons e-edit"   },
            new () { Text = "Duplicate", IconCss = "sb-icons e-copy"   },
            new () { Text = "Archive",   IconCss = "sb-icons e-trash" }
        };

        public static readonly List<MenuItemModel> ShareItems = new()
        {
            new () { Text = "Copy link", IconCss = "sb-icons e-link" },
            new () { Text = "Search",    IconCss = "sb-icons e-search" }
        };

        public static readonly List<MenuItemModel> ClipboardItems = new()
        {
            new () { Text = "Cut",   IconCss = "sb-icons e-cut"   },
            new () { Text = "Copy",  IconCss = "sb-icons e-copy"  },
            new () { Text = "Paste", IconCss = "sb-icons e-paste" }
        };

        public static readonly List<MenuItemModel> DisabledItems = new()
        {
            new () { Text = "N/A" }
        };

        public static readonly List<MenuItemModel> NavigationItems = new()
        {
            new () { Text = "Home", IconCss = "sb-icons e-home",  Url = "/"    },
            new () { Text = "Docs", IconCss = "sb-icons e-search", Url = "/docs"}
        };

        public static readonly List<MenuItemModel> InsertItems = new()
        {
            new () { Id = "A1", Text = "Snippet",   IconCss = "sb-icons e-edit"  },
            new () { Id = "B1", Text = "Reference", IconCss = "sb-icons e-link"  },
            new () { Id = "C1", Text = "Account",   IconCss = "sb-icons e-user"  }
        };

        public static readonly List<MenuItemModel> TemplateItems = new()
        {
            new () { Id = "Edit (E)",   Text = "Edit",   IconCss = "sb-icons e-edit"   },
            new () { Id = "Delete (D)", Text = "Delete", IconCss = "sb-icons e-trash" },
            new () { Id = "Reload (R)", Text = "Reload", IconCss = "sb-icons e-refresh"}
        };

        // Separator examples
        public static readonly List<MenuItemModel> ClipboardWithSeparators = new()
        {
            new () { Text = "Cut",   IconCss = "sb-icons e-cut"   },
            new () { Text = "Copy",  IconCss = "sb-icons e-copy"  },
            new () { Text = "Paste", IconCss = "sb-icons e-paste" },
            new () { Separator = true },
            new () { Text = "Bold",   IconCss = "sb-icons e-edit"  },
            new () { Text = "Refresh",IconCss = "sb-icons e-refresh"}
        };

        public static readonly List<MenuItemModel> FileMenuGrouped = new()
        {
            new () { Text = "Open",            IconCss = "sb-icons e-play" },
            new () { Text = "Open recent",     IconCss = "sb-icons e-search" },
            new () { Separator = true },
            new () { Text = "Export (group)",  HtmlAttributes = new Dictionary<string, object>() { ["aria-hidden"] = "true" } },
            new () { Text = "Export CSV",      IconCss = "sb-icons e-export-csv" },
            new () { Text = "Export PDF",      IconCss = "sb-icons e-export-pdf" },
            new () { Separator = true },
            new () { Text = "Settings",        IconCss = "sb-icons e-settings" }
        };

        public static readonly List<MenuItemModel> StyledSeparatorItems = new()
        {
            new () { Text = "First",  IconCss = "sb-icons e-check"  },
            new () { Separator = true },
            new () { Text = "Second", IconCss = "sb-icons e-refresh"},
            new () { Separator = true },
            new () { Text = "Third",  IconCss = "sb-icons e-user"   }
        };

        // SplitButton specific sets
        public static readonly List<MenuItemModel> RunMenuItems = new()
        {
            new () { Text = "Run with logging", IconCss = "sb-icons e-search" },
            new () { Text = "Run in preview",   IconCss = "sb-icons e-search" },
            new () { Text = "Schedule" , IconCss = "sb-icons e-date-time"}
        };

        public static readonly List<MenuItemModel> ExportItems = new()
        {
            new () { Text = "CSV",   IconCss = "sb-icons e-export-csv" },
            new () { Text = "Excel", IconCss = "sb-icons e-export-excel" },
            new () { Text = "PDF",   IconCss = "sb-icons e-export-pdf" }
        };

        public static readonly List<MenuItemModel> BuildItems = new()
        {
            new () { Text = "Build Debug",   IconCss = "sb-icons e-refresh" },
            new () { Text = "Build Release", IconCss = "sb-icons e-refresh" }
        };

        public static readonly List<MenuItemModel> MediaItems = new()
        {
            new () { Text = "Open player", IconCss = "sb-icons e-play"  },
            new () { Text = "Pause",       IconCss = "sb-icons e-pause" },
            new () { Text = "Stop",        IconCss = "sb-icons e-stop-rectangle"  }
        };

        public static readonly List<MenuItemModel> SettingsItems = new()
        {
            new () { Text = "Preferences", IconCss = "sb-icons e-settings" },
            new () { Text = "Account",     IconCss = "sb-icons e-user" }
        };

        public static readonly List<MenuItemModel> ColorItems = new()
        {
            new () { Text = "Pick…" },
            new () { Text = "Recent" }
        };

        public static readonly List<MenuItemModel> A11yItems = new()
        {
            new () { Text = "Quick Action 1", IconCss = "sb-icons e-check" },
            new () { Text = "Quick Action 2", IconCss = "sb-icons e-check" }
        };

        public static readonly List<MenuItemModel> PlayItems = new()
        {
            new () { Text = "Open playlist", IconCss = "sb-icons e-play" },
            new () { Text = "Refresh", IconCss = "sb-icons e-refresh" }
        };

        public static readonly List<MenuItemModel> PipelineBuildItems = new()
        {
            new () { Text = "Build Debug",   IconCss = "sb-icons e-refresh" },
            new () { Text = "Build Release", IconCss = "sb-icons e-refresh" }
        };

        public static readonly List<MenuItemModel> PipelineTestItems = new()
        {
            new () { Text = "Run Unit Tests",        IconCss = "sb-icons e-refresh" },
            new () { Text = "Run Integration Tests", IconCss = "sb-icons e-refresh" },
            new () { Text = "Coverage Report",       IconCss = "sb-icons e-search"  }
        };

        // ButtonGroup / nested examples
        public static readonly List<MenuItemModel> SplitItems = new()
        {
            new () { Text = "Paste", IconCss = "sb-icons e-paste" },
            new () { Text = "Paste Special", IconCss = "sb-icons e-paste-match-destination" },
            new () { Separator = true },
            new () { Text = "Cut", IconCss = "sb-icons e-cut" },
            new () { Text = "Copy", IconCss = "sb-icons e-copy" }
        };

        public static readonly List<MenuItemModel> ActionItems = new()
        {
            new () { Text = "Rename", IconCss = "sb-icons e-rename" },
            new () { Text = "Move",   IconCss = "sb-icons e-drag-and-drop" },
            new () { Text = "Archive",IconCss = "sb-icons e-box" },
            new () { Separator = true },
            new () { Text = "Delete", IconCss = "sb-icons e-trash" }
        };

        public static readonly List<MenuItemModel> DownloadItems = new()
        {
            new () { Text = "PDF",  IconCss = "sb-icons e-export-pdf" },
            new () { Text = "CSV",  IconCss = "sb-icons e-export-csv" },
            new () { Text = "XLSX", IconCss = "sb-icons e-export-excel" }
        };

        public static readonly List<MenuItemModel> ProfileItems = new()
        {
            new () { Text = "View Profile", IconCss = "sb-icons e-user" },
            new () { Text = "Edit Profile", IconCss = "sb-icons e-edit" }
        };

        public static readonly List<MenuItemModel> LogoutItems = new()
        {
            new () { Text = "Logout", IconCss = "sb-icons e-exit-full-screen" }
        };
    }
}
