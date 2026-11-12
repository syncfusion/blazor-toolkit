using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class ToolbarItem
    {
        #region Parameters

        /// <summary>
        /// Gets or sets the unique identifier of the item.
        /// </summary>
        /// <value>An identifier string. When it is not set, a unique identifier is generated. The default is <see langword="null"/>.</value>
        /// <remarks>
        /// The value is HTML-encoded when rendered. Duplicate identifiers do not throw; the first registered item wins lookups.
        /// </remarks>
        [Parameter]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the display text and the accessible name of the item.
        /// </summary>
        /// <value>A text string. The default is <see langword="null"/>.</value>
        /// <remarks>
        /// The text stays the accessible name when <see cref="DisplayMode"/> is <see cref="ToolbarItemDisplayMode.IconOnly"/>.
        /// The value is HTML-encoded when rendered.
        /// </remarks>
        [Parameter]
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets the CSS class of the icon.
        /// </summary>
        /// <value>A space-separated list of CSS class names. The default is <see langword="null"/>.</value>
        [Parameter]
        public string? IconCss { get; set; }

        /// <summary>
        /// Gets or sets the data model when this item was created from <c>SfToolbar.Items</c>.
        /// </summary>
        /// <value>The source model, or <see langword="null"/> for a declarative item.</value>
        [Parameter]
        public ToolbarItemModel? BoundModel { get; set; }

        /// <summary>
        /// Gets or sets the position of the icon relative to the text.
        /// </summary>
        /// <value>An <see cref="Syncfusion.Blazor.Toolkit.IconPosition"/> value. The default is <see cref="Syncfusion.Blazor.Toolkit.IconPosition.Left"/>.</value>
        [Parameter]
        public IconPosition IconPosition { get; set; } = IconPosition.Left;

        /// <summary>
        /// Gets or sets the behavior and appearance of the item.
        /// </summary>
        /// <value>A <see cref="ToolbarItemType"/> value. The default is <see cref="ToolbarItemType.Button"/>.</value>
        /// <remarks>
        /// <see cref="ToolbarItemType.Split"/> keeps a primary action and a chevron that opens <see cref="Items"/>.
        /// <see cref="ToolbarItemType.DropDown"/> and <see cref="ToolbarItemType.Menu"/> open <see cref="Items"/> from the main button.
        /// <see cref="ToolbarItemType.Toggle"/> with a group name is a radio item. <see cref="ToolbarItemType.Check"/> multi-selects.
        /// </remarks>
        [Parameter]
        public ToolbarItemType Type { get; set; } = ToolbarItemType.Button;

        /// <summary>
        /// Gets or sets whether the item is interactive.
        /// </summary>
        /// <value><see langword="true"/> to make the item interactive; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        /// <remarks>
        /// A disabled item stays in the DOM with <c>aria-disabled="true"</c>, raises no click callbacks and is skipped by keyboard navigation.
        /// <see cref="SfToolbar"/> has no toolbar-wide enabled switch.
        /// </remarks>
        [Parameter]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the item is rendered.
        /// </summary>
        /// <value><see langword="true"/> to render the item; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets the selected state of a <see cref="ToolbarItemType.Toggle"/> item.
        /// </summary>
        /// <value><see langword="true"/> when selected. The default is <see langword="false"/>.</value>
        /// <example>
        /// <code><![CDATA[
        /// <ToolbarItem Text="Bold" Type="ToolbarItemType.Toggle" @bind-Selected="isBold" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool Selected { get; set; }

        /// <summary>
        /// Gets or sets the callback raised when <see cref="Selected"/> changes. It enables <c>@bind-Selected</c>.
        /// </summary>
        /// <value>An <see cref="EventCallback{TValue}"/> of <see cref="bool"/>.</value>
        [Parameter]
        public EventCallback<bool> SelectedChanged { get; set; }

        /// <summary>
        /// Gets or sets how strongly the item prefers to stay in the main bar when space is short.
        /// </summary>
        /// <value>A <see cref="ToolbarOverflowOption"/> value. The default is <see cref="ToolbarOverflowOption.None"/>.</value>
        [Parameter]
        public ToolbarOverflowOption Overflow { get; set; } = ToolbarOverflowOption.None;

        /// <summary>
        /// Gets or sets the secondary overflow sort key. Lower values, including negative values, stay visible longer.
        /// </summary>
        /// <value>An integer. The default is 0.</value>
        [Parameter]
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the supplemental tooltip text.
        /// </summary>
        /// <value>A text string. The default is <see langword="null"/>.</value>
        /// <remarks>
        /// The tooltip never replaces <see cref="Text"/> as the accessible name. The value is HTML-encoded when rendered.
        /// </remarks>
        [Parameter]
        public string? Tooltip { get; set; }

        /// <summary>
        /// Gets or sets the badge content.
        /// </summary>
        /// <value>A text string. The default is <see langword="null"/>.</value>
        /// <remarks>The value is HTML-encoded when rendered.</remarks>
        [Parameter]
        public string? Badge { get; set; }

        /// <summary>
        /// Gets or sets additional CSS classes applied to the item.
        /// </summary>
        /// <value>A space-separated list of CSS class names. The default is <see langword="null"/>.</value>
        /// <remarks>
        /// <strong>Security:</strong> this value is a trust-boundary input. Do not fill it from untrusted data.
        /// </remarks>
        [Parameter]
        public string? CssClass { get; set; }

        /// <summary>
        /// Gets or sets whether the icon, the text, or both are shown.
        /// </summary>
        /// <value>A <see cref="ToolbarItemDisplayMode"/> value. The default is <see cref="ToolbarItemDisplayMode.IconAndText"/>.</value>
        [Parameter]
        public ToolbarItemDisplayMode DisplayMode { get; set; } = ToolbarItemDisplayMode.IconAndText;

        /// <summary>
        /// Gets or sets the name of the mutually exclusive group the item belongs to.
        /// </summary>
        /// <value>A group name. The default is <see langword="null"/>.</value>
        [Parameter]
        public string? GroupName { get; set; }

        /// <summary>
        /// Gets or sets nested commands for a Split, DropDown or Menu item. This is not the same member as <see cref="SfToolbar.Items"/> (ADR-011).
        /// </summary>
        /// <value>A <see cref="RenderFragment"/> of nested <see cref="ToolbarItem"/> children. The default is <see langword="null"/>.</value>
        /// <remarks>Only one level of nested commands is rendered. Setting cancel on the click stops the menu from opening.</remarks>
        [Parameter]
        public RenderFragment? Items { get; set; }

        /// <summary>
        /// Gets or sets custom content for the item.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/> rendered inside the item button. The default is <see langword="null"/>.</value>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets an advanced template for the item.
        /// </summary>
        /// <value>A <see cref="RenderFragment{TValue}"/> of <see cref="ToolbarTemplateContext"/>. The default is <see langword="null"/>.</value>
        /// <remarks>An item with a template renders the template instead of a button. The template manages its own focus.</remarks>
        [Parameter]
        public RenderFragment<ToolbarTemplateContext>? Template { get; set; }

        /// <summary>
        /// Gets or sets the callback raised when the item is clicked.
        /// </summary>
        /// <value>An <see cref="EventCallback{TValue}"/> of <see cref="MouseEventArgs"/>.</value>
        /// <remarks>It runs before <see cref="SfToolbar.OnItemClick"/>.</remarks>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        #endregion
    }
}
