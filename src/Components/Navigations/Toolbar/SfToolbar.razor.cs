namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Represents a responsive, accessible command bar that arranges items, groups, separators and spacers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The toolbar follows the WAI-ARIA toolbar pattern. It is a single tab stop with a roving <c>tabindex</c>. Arrow keys,
    /// <c>Home</c> and <c>End</c> move focus between enabled items, and the keys that move focus follow the orientation and
    /// the text direction.
    /// </para>
    /// <para>
    /// <strong>Security:</strong> every consumer string is encoded by Blazor and none reaches a raw HTML sink.
    /// <see cref="CssClass"/> and <see cref="HtmlAttributes"/> are trust-boundary inputs. Do not fill them from untrusted data.
    /// </para>
    /// <para>
    /// <strong>Render modes:</strong> Static SSR renders the item markup only, with no events. The interactive modes add event handling.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfToolbar AriaLabel="Main commands">
    ///     <ToolbarItem Text="Cut" IconCss="e-icons e-cut" OnClick="@OnCut" />
    ///     <ToolbarItem Text="Copy" IconCss="e-icons e-copy" />
    ///     <ToolbarSeparator />
    ///     <ToolbarItem Text="Paste" IconCss="e-icons e-paste" />
    /// </SfToolbar>
    /// ]]></code>
    /// </example>
    public partial class SfToolbar : SfBaseComponent, IToolbarHost
    {
        #region Constants

        private const string RootClass = "e-control e-toolbar e-lib";
        private const string HorizontalClass = "e-toolbar-horizontal";
        private const string VerticalClass = "e-toolbar-vertical";
        private const string RtlClass = "e-rtl";
        private const char Space = ' ';

        private bool HasRadioGroup => _collection.Flatten().Any(item => item.IsRadio);

        private bool HasCheckGroup => _collection.Flatten().Any(item => item.IsCheck);

        #endregion

        #region Fields

        private readonly ToolbarItemCollection _collection = new();
        private Dictionary<string, object> _attributes = [];
        private string _rootClass = RootClass;
        private string _ariaOrientation = "horizontal";
        private string? _direction;
        private IToolbarItem? _tabStop;
        private IToolbarItem? _lastFocused;
        private bool _rtl;

        #endregion

        #region Internal members

        private IToolbarHost Self => this;

        ToolbarItemCollection IToolbarHost.Collection => _collection;

        ToolbarOrientation IToolbarHost.Orientation => Orientation;

        bool IToolbarHost.IsRtl => _rtl;

        #endregion

        #region Rendering state

        private void ComputeRenderState()
        {
            bool globalRtl = SyncfusionService?._options?.EnableRtl ?? false;
            _rtl = EnableRtl ?? globalRtl;
            // null inherits: render no dir attribute so the page direction applies. An explicit value is written out.
            _direction = EnableRtl is null ? (globalRtl ? "rtl" : null) : (EnableRtl.Value ? "rtl" : "ltr");

            string orientationClass = Orientation == ToolbarOrientation.Vertical ? VerticalClass : HorizontalClass;
            string userClass = HtmlAttributes is not null && HtmlAttributes.TryGetValue("class", out object? raw) ? raw?.ToString() ?? string.Empty : string.Empty;

            System.Text.StringBuilder css = new(RootClass);
            _ = css.Append(Space).Append(orientationClass);
            _ = css.Append(Space).Append(AlignClass(Align));
            _ = css.Append(Space).Append(ModeClass(OverflowMode));
            if (OverflowMode == ToolbarOverflowMode.Extended && _extendedExpanded)
            {
                _ = css.Append(Space).Append("e-toolbar-expanded");
            }
            if (_rtl)
            {
                _ = css.Append(Space).Append(RtlClass);
            }
            if (!string.IsNullOrWhiteSpace(CssClass))
            {
                _ = css.Append(Space).Append(CssClass);
            }
            if (!string.IsNullOrWhiteSpace(userClass))
            {
                _ = css.Append(Space).Append(userClass);
            }
            _rootClass = css.ToString();
            _ariaOrientation = Orientation == ToolbarOrientation.Vertical ? "vertical" : "horizontal";

            _attributes = [];
            if (HtmlAttributes is not null)
            {
                foreach (KeyValuePair<string, object> pair in HtmlAttributes)
                {
                    if (!string.Equals(pair.Key, "class", StringComparison.OrdinalIgnoreCase))
                    {
                        _attributes[pair.Key] = pair.Value;
                    }
                }
            }
        }

        private static string AlignClass(ToolbarAlignment align)
        {
            return align switch
            {
                ToolbarAlignment.Start => "e-toolbar-align-start",
                ToolbarAlignment.Center => "e-toolbar-align-center",
                ToolbarAlignment.End => "e-toolbar-align-end",
                ToolbarAlignment.Justify => "e-toolbar-align-justify",
                _ => "e-toolbar-align-start"
            };
        }

        private static string ModeClass(ToolbarOverflowMode mode)
        {
            return mode switch
            {
                ToolbarOverflowMode.Scrollable => "e-toolbar-scrollable",
                ToolbarOverflowMode.Popup => "e-toolbar-popup",
                ToolbarOverflowMode.MultiRow => "e-toolbar-multirow",
                ToolbarOverflowMode.Extended => "e-toolbar-extended",
                _ => "e-toolbar-popup"
            };
        }

        #endregion
    }
}
