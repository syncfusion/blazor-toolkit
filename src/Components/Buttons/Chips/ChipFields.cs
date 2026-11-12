namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>Maps property names on a data item to chip fields when selector delegates are not provided.</summary>
    public sealed class ChipFields
    {
        /// <summary>Property name for display text.</summary>
        public string? Text { get; set; }

        /// <summary>Property name for selection value.</summary>
        public string? Value { get; set; }

        /// <summary>Property name for disabled state.</summary>
        public string? Disabled { get; set; }

        /// <summary>Property name for removable state.</summary>
        public string? Removable { get; set; }

        /// <summary>Property name for leading icon CSS class.</summary>
        public string? IconCss { get; set; }

        /// <summary>Property name for leading image URL.</summary>
        public string? ImageUrl { get; set; }

        /// <summary>Property name for image alternate text.</summary>
        public string? ImageAlt { get; set; }

        /// <summary>Property name for additional CSS classes.</summary>
        public string? CssClass { get; set; }

        /// <summary>Property name for navigation URL.</summary>
        public string? Href { get; set; }
    }
}
