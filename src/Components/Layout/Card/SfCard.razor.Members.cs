using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit
{
    public partial class SfCard
    {
        #region Members

        /// <summary>
        /// Gets or sets the content of the card, typically region components or free markup.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/> that holds the card regions. The default value is <see langword="null"/>.</value>
        /// <remarks>
        /// Regions render in markup order; none of them is required.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCard>
        ///     <SfCardContent>Body text</SfCardContent>
        /// </SfCard>
        /// ]]></code>
        /// </example>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the visual variant of the card.
        /// </summary>
        /// <value>One of the <see cref="CardVariant"/> values. The default value is <see cref="CardVariant.Filled"/>.</value>
        /// <remarks>
        /// <see cref="CardVariant.Outlined"/> and <see cref="CardVariant.Flat"/> never draw a shadow, regardless of <see cref="Elevation"/>.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCard Variant="CardVariant.Outlined" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public CardVariant Variant { get; set; } = CardVariant.Filled;

        /// <summary>
        /// Gets or sets whether regions are stacked vertically or arranged horizontally.
        /// </summary>
        /// <value>One of the <see cref="CardOrientation"/> values. The default value is <see cref="CardOrientation.Vertical"/>.</value>
        /// <example>
        /// <code><![CDATA[
        /// <SfCard Orientation="CardOrientation.Horizontal" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public CardOrientation Orientation { get; set; } = CardOrientation.Vertical;

        /// <summary>
        /// Gets or sets the relative elevation (drop shadow depth) of the card.
        /// </summary>
        /// <value>An integer from 0 to 5, where 0 means no shadow. The default value is 1.</value>
        /// <remarks>
        /// Values outside the range are clamped. With <see cref="CardVariant.Elevated"/> the card is drawn two levels deeper than the configured value, up to the maximum.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCard Variant="CardVariant.Elevated" Elevation="3" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public int Elevation { get; set; } = 1;

        /// <summary>
        /// Gets or sets whether the card uses square corners (no border radius).
        /// </summary>
        /// <value><see langword="true"/> to disable rounded corners; otherwise, <see langword="false"/>. The default value is <see langword="false"/>.</value>
        [Parameter]
        public bool Square { get; set; }

        /// <summary>
        /// Gets or sets whether the card shows a hover and focus-within affordance.
        /// </summary>
        /// <value><see langword="true"/> to lift the card on hover; otherwise, <see langword="false"/>. The default value is <see langword="false"/>.</value>
        [Parameter]
        public bool Hoverable { get; set; }

        /// <summary>
        /// Gets or sets whether the card is in a loading state.
        /// </summary>
        /// <value><see langword="true"/> to show the loading presentation; otherwise, <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// A loading card exposes <c>aria-busy="true"</c>, ignores activation, and makes the <see cref="SfCardActions"/> region inert.
        /// </remarks>
        [Parameter]
        public bool IsLoading { get; set; }

        /// <summary>
        /// Gets or sets whether content regions use the theme content padding.
        /// </summary>
        /// <value><see langword="true"/> to pad the regions; otherwise, <see langword="false"/>. The default value is <see langword="true"/>.</value>
        [Parameter]
        public bool ContentPadding { get; set; } = true;

        /// <summary>
        /// Gets or sets the CSS width of the card.
        /// </summary>
        /// <value>A CSS length such as <c>280px</c> or <c>50%</c>. A bare number is treated as pixels. The default value is <see langword="null"/>.</value>
        [Parameter]
        public string? Width { get; set; }

        /// <summary>
        /// Gets or sets the CSS height of the card.
        /// </summary>
        /// <value>A CSS length such as <c>320px</c>. A bare number is treated as pixels. The default value is <see langword="null"/>.</value>
        [Parameter]
        public string? Height { get; set; }

        /// <summary>
        /// Gets or sets additional CSS class names applied to the root element.
        /// </summary>
        /// <value>One or more space-separated class names. The default value is <see langword="null"/>.</value>
        [Parameter]
        public string? CssClass { get; set; }

        /// <summary>
        /// Gets or sets the id attribute of the root element.
        /// </summary>
        /// <value>A unique element id. When not set, an id is generated.</value>
        [Parameter]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes to apply to the root element.
        /// </summary>
        /// <value>A dictionary of attribute names and values. The default value is <see langword="null"/>.</value>
        /// <remarks>
        /// User attributes win over the attributes the card computes, except <c>class</c> and <c>style</c>, which are merged with the computed values.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfCard title="Profile" data-owner="qa" />
        /// ]]></code>
        /// </example>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? HtmlAttributes { get; set; }

        /// <summary>
        /// Gets or sets whether the entire card is an interactive control.
        /// </summary>
        /// <value><see langword="true"/> to expose the root as a keyboard-focusable button; otherwise, <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>
        /// A clickable card requires an interactive render mode to invoke <see cref="OnClick"/>. Enter activates on key down and Space on key up, as a native button does.
        /// </remarks>
        [Parameter]
        public bool IsClickable { get; set; }

        /// <summary>
        /// Gets or sets an accessible name for the card.
        /// </summary>
        /// <value>The text exposed as <c>aria-label</c>. The default value is <see langword="null"/>.</value>
        /// <remarks>
        /// Provide a value when a clickable card has no readable text content.
        /// </remarks>
        [Parameter]
        public string? AriaLabel { get; set; }

        #endregion
    }
}
