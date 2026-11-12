using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>Public parameters for <see cref="SfChip"/>.</summary>
    public partial class SfChip
    {
        /// <summary>Display text. Encoded by the renderer.</summary>
        [Parameter]
        public string? Text { get; set; }

        /// <summary>Chip value used when the chip is projected into a set.</summary>
        [Parameter]
        public object? Value { get; set; }

        /// <summary>Functional mode. Default is <see cref="ChipType.Informational"/>.</summary>
        [Parameter]
        public ChipType ChipType { get; set; } = ChipType.Informational;

        /// <summary>Visual variant. Default is <see cref="ChipVariant.Filled"/>.</summary>
        [Parameter]
        public ChipVariant Variant { get; set; } = ChipVariant.Filled;

        /// <summary>Density. Default is <see cref="ChipSize.Medium"/>.</summary>
        [Parameter]
        public ChipSize Size { get; set; } = ChipSize.Medium;

        /// <summary>Corner shape. Default is <see cref="ChipShape.Rounded"/>.</summary>
        [Parameter]
        public ChipShape Shape { get; set; } = ChipShape.Rounded;

        /// <summary>Leading icon CSS class.</summary>
        [Parameter]
        public string? IconCss { get; set; }

        /// <summary>Leading image URL.</summary>
        [Parameter]
        public string? ImageUrl { get; set; }

        /// <summary>Alternate text for <see cref="ImageUrl"/>.</summary>
        [Parameter]
        public string? ImageAlt { get; set; }

        /// <summary>Navigation URL. Required for a navigation chip to render as a link.</summary>
        [Parameter]
        public string? Href { get; set; }

        /// <summary>Link target. Applied only when the chip renders as an anchor.</summary>
        [Parameter]
        public string? Target { get; set; }

        /// <summary>Link relationship. Applied only when the chip renders as an anchor.</summary>
        [Parameter]
        public string? Rel { get; set; }

        /// <summary>Selected state for filter and choice chips.</summary>
        [Parameter]
        public bool Selected { get; set; }

        /// <summary>Shows a remove control.</summary>
        [Parameter]
        public bool Removable { get; set; }

        /// <summary>Disables interaction and sets native disabled or aria-disabled.</summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>Blocks selection and removal without hiding the value.</summary>
        [Parameter]
        public bool ReadOnly { get; set; }

        /// <summary>Loading state. Suppresses activation.</summary>
        [Parameter]
        public bool IsLoading { get; set; }

        /// <summary>Error state.</summary>
        [Parameter]
        public bool HasError { get; set; }

        /// <summary>Additional CSS classes.</summary>
        [Parameter]
        public string? CssClass { get; set; }

        /// <summary>Inline style.</summary>
        [Parameter]
        public string? Style { get; set; }

        /// <summary>Element id. A stable id is generated when omitted.</summary>
        [Parameter]
        public string? Id { get; set; }

        /// <summary>Accessible name.</summary>
        [Parameter]
        public string? AriaLabel { get; set; }

        /// <summary>Id of the element that names this chip.</summary>
        [Parameter]
        public string? AriaLabelledBy { get; set; }

        /// <summary>Ids of elements that describe this chip.</summary>
        [Parameter]
        public string? AriaDescribedBy { get; set; }

        /// <summary>Focus override. When null, the chip uses the default tabindex.</summary>
        [Parameter]
        public int? TabIndex { get; set; }

        /// <summary>HTML attributes applied to the root. Framework attributes win on conflict.</summary>
        [Parameter]
        public Dictionary<string, object>? HtmlAttributes { get; set; }

        /// <summary>Custom body content. Replaces <see cref="Text"/> when set.</summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>Leading slot.</summary>
        [Parameter]
        public RenderFragment? LeadingTemplate { get; set; }

        /// <summary>Trailing slot.</summary>
        [Parameter]
        public RenderFragment? TrailingTemplate { get; set; }

        /// <summary>Content inside the remove button. The button element remains.</summary>
        [Parameter]
        public RenderFragment? RemoveTemplate { get; set; }

        /// <summary>Right-to-left layout.</summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        [CascadingParameter]
        private IChipSetRegistration? Owner { get; set; }

        [Inject]
        private IStringLocalizer? Localizer { get; set; }
    }
}
