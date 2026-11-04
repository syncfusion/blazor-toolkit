using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Renders the actions row of an <see cref="SfCard"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Controls placed here are isolated from a clickable parent card: clicks and key presses inside the row never reach
    /// <see cref="SfCard.OnClick"/>, so a nested button fires exactly once.
    /// </para>
    /// <para>
    /// While the parent card has <see cref="SfCard.IsLoading"/> set, the row is rendered <c>inert</c>, which removes the
    /// controls from the tab order and from assistive technology interaction.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfCardActions>
    ///     <SfButton Content="Buy" IsPrimary="true" />
    /// </SfCardActions>
    /// ]]></code>
    /// </example>
    public partial class SfCardActions : SfBaseComponent
    {
        private const string RegionClass = "e-card-actions";

        // A plain object as the callback receiver means the no-op handlers never trigger a component re-render.
        private static readonly object _noRenderReceiver = new();

        /// <exclude />
        private string _class = RegionClass;

        /// <exclude />
        private string? _style;

        /// <exclude />
        private bool _inert;

        /// <exclude />
        private bool _isolate;

        /// <exclude />
        private Dictionary<string, object> _attributes = [];

        /// <exclude />
        private EventCallback<MouseEventArgs> _isolateClick;

        /// <exclude />
        private EventCallback<KeyboardEventArgs> _isolateKey;

        /// <exclude />
        [CascadingParameter]
        internal SfCard? Card { get; set; }

        /// <summary>
        /// Gets or sets the content of the actions row.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/>. The default value is <see langword="null"/>.</value>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets additional CSS class names for the actions element.
        /// </summary>
        /// <value>One or more space-separated class names. The default value is <see langword="null"/>.</value>
        [Parameter]
        public string? CssClass { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes for the actions element.
        /// </summary>
        /// <value>A dictionary of attribute names and values. User attributes win over computed ones, except <c>class</c> and <c>style</c>, which are merged.</value>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? HtmlAttributes { get; set; }

        /// <exclude />
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            _class = CardAttributes.MergeClass(RegionClass, CssClass, HtmlAttributes);
            _style = CardAttributes.MergeStyle(null, HtmlAttributes);
            _attributes = CardAttributes.Filter(HtmlAttributes);
            _inert = Card?.IsBusy ?? false;

            // Isolation is needed only under a clickable card. Elsewhere no handler is registered,
            // so a static card costs no SignalR round trip per event.
            _isolate = Card?.IsClickable ?? false;
            _isolateClick = EventCallback.Factory.Create<MouseEventArgs>(_noRenderReceiver, static () => { });
            _isolateKey = EventCallback.Factory.Create<KeyboardEventArgs>(_noRenderReceiver, static () => { });
        }
    }
}
