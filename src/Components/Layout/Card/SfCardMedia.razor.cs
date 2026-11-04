using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Renders media or cover content for an <see cref="SfCard"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When <see cref="ImageUrl"/> is set an <c>img</c> element is rendered; otherwise <see cref="ChildContent"/> is rendered as custom cover content.
    /// </para>
    /// <para>
    /// <strong>Accessibility:</strong> set <see cref="Alt"/> for informative images. When it is not set the image is rendered with an empty
    /// <c>alt</c> and is treated as decorative by assistive technology.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// <SfCardMedia ImageUrl="images/product.jpg" Alt="Trail Runner shoe" />
    /// ]]></code>
    /// </example>
    public partial class SfCardMedia : SfBaseComponent
    {
        private const string RegionClass = "e-card-media";

        /// <exclude />
        private string _class = RegionClass;

        /// <exclude />
        private string? _style;

        /// <exclude />
        private string? _imageStyle;

        /// <exclude />
        private Dictionary<string, object> _attributes = [];

        /// <summary>
        /// Gets or sets the image URL for the media region.
        /// </summary>
        /// <value>An absolute or relative URL. The default value is <see langword="null"/>.</value>
        [Parameter]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the alternate text for the image.
        /// </summary>
        /// <value>A text alternative. The default value is <see langword="null"/>, which renders an empty <c>alt</c> (decorative image).</value>
        [Parameter]
        public string? Alt { get; set; }

        /// <summary>
        /// Gets or sets the CSS height of the media region.
        /// </summary>
        /// <value>A CSS length. A bare number is treated as pixels. The default value is <c>180px</c>.</value>
        [Parameter]
        public string? Height { get; set; } = "180px";

        /// <summary>
        /// Gets or sets how the image is fitted within the media box.
        /// </summary>
        /// <value>One of the <see cref="CardMediaFit"/> values. The default value is <see cref="CardMediaFit.Cover"/>.</value>
        [Parameter]
        public CardMediaFit ObjectFit { get; set; } = CardMediaFit.Cover;

        /// <summary>
        /// Gets or sets custom cover content rendered instead of <see cref="ImageUrl"/>.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/>. The default value is <see langword="null"/>.</value>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets additional CSS class names for the media element.
        /// </summary>
        /// <value>One or more space-separated class names. The default value is <see langword="null"/>.</value>
        [Parameter]
        public string? CssClass { get; set; }

        /// <summary>
        /// Gets or sets additional HTML attributes for the media element.
        /// </summary>
        /// <value>A dictionary of attribute names and values. User attributes win over computed ones, except <c>class</c> and <c>style</c>, which are merged.</value>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? HtmlAttributes { get; set; }

        /// <exclude />
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            _class = CardAttributes.MergeClass(RegionClass, CssClass, HtmlAttributes);
            string? height = string.IsNullOrWhiteSpace(Height) ? null : "height:" + SfBaseUtils.FormatUnit(Height.Trim()) + ";";
            _style = CardAttributes.MergeStyle(height, HtmlAttributes);
            _attributes = CardAttributes.Filter(HtmlAttributes);
            _imageStyle = ObjectFit switch
            {
                CardMediaFit.Contain => "object-fit:contain;",
                CardMediaFit.Fill => "object-fit:fill;",
                CardMediaFit.None => null,
                CardMediaFit.Cover => "object-fit:cover;",
                _ => "object-fit:cover;"
            };
        }
    }
}
