using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfToolbar
    {
        #region Parameters

        /// <summary>
        /// Gets or sets the layout direction of the toolbar.
        /// </summary>
        /// <value>A <see cref="ToolbarOrientation"/> value. The default is <see cref="ToolbarOrientation.Horizontal"/>.</value>
        /// <remarks>
        /// A vertical toolbar moves focus with <c>ArrowUp</c> and <c>ArrowDown</c>. A horizontal toolbar uses <c>ArrowLeft</c> and
        /// <c>ArrowRight</c>, reversed when the toolbar renders right to left.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfToolbar Orientation="ToolbarOrientation.Vertical" AriaLabel="Tools">
        ///     <ToolbarItem Text="Select" />
        ///     <ToolbarItem Text="Draw" />
        /// </SfToolbar>
        /// ]]></code>
        /// </example>
        [Parameter]
        public ToolbarOrientation Orientation { get; set; } = ToolbarOrientation.Horizontal;

        /// <summary>
        /// Gets or sets the strategy used when the items do not fit in the available space.
        /// </summary>
        /// <value>A <see cref="ToolbarOverflowMode"/> value. The default is <see cref="ToolbarOverflowMode.Popup"/>.</value>
        [Parameter]
        public ToolbarOverflowMode OverflowMode { get; set; } = ToolbarOverflowMode.Popup;

        /// <summary>
        /// Gets or sets how the items are aligned inside the toolbar.
        /// </summary>
        /// <value>A <see cref="ToolbarAlignment"/> value. The default is <see cref="ToolbarAlignment.Start"/>.</value>
        [Parameter]
        public ToolbarAlignment Align { get; set; } = ToolbarAlignment.Start;

        /// <summary>
        /// Gets or sets additional CSS classes applied to the root element.
        /// </summary>
        /// <value>A space-separated list of CSS class names. The default is <see langword="null"/>.</value>
        /// <remarks>
        /// <strong>Security:</strong> this value is a trust-boundary input. Do not fill it from untrusted data.
        /// </remarks>
        [Parameter]
        public string? CssClass { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes applied to the root element.
        /// </summary>
        /// <value>A dictionary of attribute names and values. Unmatched attributes on the tag are captured here.</value>
        /// <remarks>
        /// Use it for native attributes such as <c>id</c>, <c>style</c> or <c>data-*</c>. A <c>class</c> value is merged with the
        /// component classes. A <c>style</c> value such as a width or height reaches the root element.
        /// <strong>Security:</strong> this value is a trust-boundary input. Do not fill it from untrusted data.
        /// </remarks>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? HtmlAttributes { get; set; }

        /// <summary>
        /// Gets or sets the accessible name of the toolbar, rendered as <c>aria-label</c>.
        /// </summary>
        /// <value>A text string. The default is <see langword="null"/>.</value>
        /// <remarks>
        /// A toolbar needs an accessible name. When this value is not set, give the toolbar one with
        /// <c>aria-labelledby</c> through <see cref="HtmlAttributes"/>.
        /// </remarks>
        [Parameter]
        public string? AriaLabel { get; set; }

        /// <summary>
        /// Gets or sets whether the toolbar renders right to left.
        /// </summary>
        /// <value>
        /// <see langword="true"/> to render right to left, <see langword="false"/> to render left to right, or
        /// <see langword="null"/> to inherit the direction from the page and the global Toolkit option. The default is <see langword="null"/>.
        /// </value>
        [Parameter]
        public bool? EnableRtl { get; set; }

        /// <summary>
        /// Gets or sets the declarative content of the toolbar.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/> holding <see cref="ToolbarItem"/>, <see cref="ToolbarGroup"/>,
        /// <see cref="ToolbarSeparator"/>, <see cref="ToolbarSpacer"/> and <see cref="ToolbarTemplateItem"/> children.</value>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the advanced overflow configuration.
        /// </summary>
        /// <value>A <see cref="ToolbarOverflowSettings"/> instance. The default is a new instance with the documented defaults.</value>
        /// <remarks>The settings are read when the toolbar renders. Assign a new instance to change them at runtime.</remarks>
        [Parameter]
        public ToolbarOverflowSettings OverflowSettings { get; set; } = new();

        /// <summary>
        /// Gets or sets the data-bound commands. This is not the same member as <see cref="ToolbarItem.Items"/> (ADR-011).
        /// </summary>
        /// <value>A sequence of <see cref="ToolbarItemModel"/>. The default is <see langword="null"/>.</value>
        /// <remarks>Declarative children render first, then these items, in collection order.</remarks>
        [Parameter]
        public IEnumerable<ToolbarItemModel>? Items { get; set; }

        /// <summary>
        /// Gets or sets the template used for each item from <see cref="Items"/>.
        /// </summary>
        /// <value>A <see cref="RenderFragment{TValue}"/> of <see cref="ToolbarTemplateContext"/>. The default is <see langword="null"/>.</value>
        /// <remarks>
        /// When it is not set, each model renders as a <see cref="ToolbarItem"/>. <see cref="ToolbarTemplateContext.IsOverflow"/> is
        /// <see langword="true"/> only when the same template renders inside the overflow menu.
        /// </remarks>
        [Parameter]
        public RenderFragment<ToolbarTemplateContext>? ItemTemplate { get; set; }

        /// <exclude />
        [Inject]
        private IStringLocalizerFactory? LocalizerFactory { get; set; }

        private IStringLocalizer? _localizer;

        #endregion
    }
}
