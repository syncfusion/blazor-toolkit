# API-xml.md
## XML documentation templates for SfCard family
## Goal: 0 compiler warnings (CS1591 and related)

### Project rules

1. Every public type and member has XML documentation.
2. `RenderFragment?` and reference types use correct nullable annotations.
3. Enums: document the type and each value.
4. `EventCallback` parameters: document when they fire.
5. Prefer fixing CS1591 over suppressing it.
6. Enable `GenerateDocumentationFile=true` on the component project.

---

## CardVariant.cs

```csharp
namespace Syncfusion.Blazor.Toolkit;

/// <summary>
/// Specifies the visual style of an <see cref="SfCard"/>.
/// </summary>
public enum CardVariant
{
    /// <summary>
    /// Solid surface using the theme card background and default elevation.
    /// </summary>
    Filled = 0,

    /// <summary>
    /// Emphasizes the border with a lighter or neutral fill.
    /// </summary>
    Outlined = 1,

    /// <summary>
    /// Stronger drop shadow for emphasis; respects <see cref="SfCard.Elevation"/>.
    /// </summary>
    Elevated = 2,

    /// <summary>
    /// Minimal chrome with little or no shadow.
    /// </summary>
    Flat = 3
}
```

---

## CardOrientation.cs

```csharp
namespace Syncfusion.Blazor.Toolkit;

/// <summary>
/// Specifies how regions inside an <see cref="SfCard"/> are laid out.
/// </summary>
public enum CardOrientation
{
    /// <summary>
    /// Stacks header, media, content, actions, and footer from top to bottom.
    /// </summary>
    Vertical = 0,

    /// <summary>
    /// Arranges primary regions in a horizontal flex row. Inline direction respects RTL.
    /// </summary>
    Horizontal = 1
}
```

---

## CardMediaFit.cs

```csharp
namespace Syncfusion.Blazor.Toolkit;

/// <summary>
/// Specifies how media content is fitted inside <see cref="SfCardMedia"/>.
/// </summary>
public enum CardMediaFit
{
    /// <summary>Fills the media box; may crop overflow (CSS object-fit: cover).</summary>
    Cover = 0,

    /// <summary>Letterboxes media within the box (CSS object-fit: contain).</summary>
    Contain = 1,

    /// <summary>Stretches media to the box (CSS object-fit: fill).</summary>
    Fill = 2,

    /// <summary>No object-fit override; intrinsic sizing applies.</summary>
    None = 3
}
```

---

## SfCard parameters (XML)

```csharp
/// <summary>
/// A composable content surface for presenting a single subject with optional
/// header, media, body, actions, and footer regions.
/// </summary>
/// <remarks>
/// <para>
/// Place building blocks such as <see cref="SfCardHeader"/>, <see cref="SfCardMedia"/>,
/// <see cref="SfCardContent"/>, <see cref="SfCardActions"/>, and <see cref="SfCardFooter"/>
/// as child content, or supply arbitrary markup via <see cref="ChildContent"/>.
/// </para>
/// <para>
/// Works in Static SSR for presentation; <see cref="OnClick"/> requires an interactive render mode.
/// </para>
/// </remarks>
public partial class SfCard
{
    /// <summary>
    /// Gets or sets the content of the card, typically region components or free markup.
    /// </summary>
    /// <value>A <see cref="RenderFragment"/>; the default is <see langword="null"/>.</value>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the visual variant of the card.
    /// </summary>
    /// <value>One of <see cref="CardVariant"/> values. The default is <see cref="CardVariant.Filled"/>.</value>
    [Parameter]
    public CardVariant Variant { get; set; } = CardVariant.Filled;

    /// <summary>
    /// Gets or sets whether regions are stacked vertically or arranged horizontally.
    /// </summary>
    /// <value>The default is <see cref="CardOrientation.Vertical"/>.</value>
    [Parameter]
    public CardOrientation Orientation { get; set; } = CardOrientation.Vertical;

    /// <summary>
    /// Gets or sets the relative elevation (drop shadow depth) of the card.
    /// </summary>
    /// <value>An integer where 0 means no shadow. The default is 1.</value>
    [Parameter]
    public int Elevation { get; set; } = 1;

    /// <summary>
    /// Gets or sets whether the card uses square corners (no border radius).
    /// </summary>
    /// <value><see langword="true"/> to disable rounded corners; otherwise <see langword="false"/>.</value>
    [Parameter]
    public bool Square { get; set; }

    /// <summary>
    /// Gets or sets whether the card shows a hover affordance.
    /// </summary>
    /// <value>The default is <see langword="false"/>.</value>
    [Parameter]
    public bool Hoverable { get; set; }

    /// <summary>
    /// Gets or sets whether the card is in a loading state.
    /// </summary>
    /// <remarks>When true, the root sets aria-busy="true" and applies loading presentation.</remarks>
    /// <value>The default is <see langword="false"/>.</value>
    [Parameter]
    public bool IsLoading { get; set; }

    /// <summary>
    /// Gets or sets whether content regions use the theme content padding.
    /// </summary>
    /// <value>The default is <see langword="true"/>.</value>
    [Parameter]
    public bool ContentPadding { get; set; } = true;

    /// <summary>
    /// Gets or sets the CSS width of the card (for example, 280px or 100%).
    /// </summary>
    /// <value>A CSS length string, or <see langword="null"/>.</value>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the CSS height of the card.
    /// </summary>
    /// <value>A CSS length string, or <see langword="null"/>.</value>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets additional CSS class names applied to the root element.
    /// </summary>
    /// <value>Space-separated class names, or <see langword="null"/>.</value>
    [Parameter]
    public string? CssClass { get; set; }

    /// <summary>
    /// Gets or sets the id attribute of the root element.
    /// </summary>
    /// <value>A unique element id, or <see langword="null"/>.</value>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets additional HTML attributes to apply to the root element.
    /// </summary>
    /// <value>A dictionary of attribute names and values, or <see langword="null"/>.</value>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? HtmlAttributes { get; set; }

    /// <summary>
    /// Gets or sets whether the entire card is an interactive control.
    /// </summary>
    /// <remarks>
    /// When true, ensure nested interactive elements stop propagation as needed.
    /// Provide an accessible name via visible title text or <see cref="AriaLabel"/>.
    /// </remarks>
    /// <value>The default is <see langword="false"/>.</value>
    [Parameter]
    public bool IsClickable { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when a clickable card is activated.
    /// </summary>
    /// <value>An <see cref="EventCallback{TValue}"/> of <see cref="MouseEventArgs"/>.</value>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Gets or sets an accessible name for the card when it is clickable or when additional naming is required.
    /// </summary>
    /// <value>A string used for aria-label, or <see langword="null"/>.</value>
    [Parameter]
    public string? AriaLabel { get; set; }
}
```

---

## Region component pattern

```csharp
/// <summary>
/// Renders the header region of an <see cref="SfCard"/>.
/// </summary>
/// <remarks>
/// Typically hosts <see cref="SfCardTitle"/>, <see cref="SfCardSubTitle"/>, or custom header content.
/// </remarks>
public partial class SfCardHeader
{
    /// <summary>
    /// Gets or sets the header content.
    /// </summary>
    /// <value>A <see cref="RenderFragment"/>; the default is <see langword="null"/>.</value>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets additional CSS class names for the header element.
    /// </summary>
    /// <value>Space-separated class names, or <see langword="null"/>.</value>
    [Parameter]
    public string? CssClass { get; set; }

    /// <summary>
    /// Gets or sets additional HTML attributes for the header element.
    /// </summary>
    /// <value>A dictionary of attributes, or <see langword="null"/>.</value>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? HtmlAttributes { get; set; }
}
```

Apply the same pattern to: SfCardTitle, SfCardSubTitle, SfCardContent, SfCardActions, SfCardFooter, SfCardSeparator.

---

## SfCardMedia

```csharp
/// <summary>
/// Renders media or cover content for an <see cref="SfCard"/>.
/// </summary>
public partial class SfCardMedia
{
    /// <summary>
    /// Gets or sets the image URL for the media region.
    /// </summary>
    /// <value>An absolute or relative image URL, or <see langword="null"/> when using ChildContent.</value>
    [Parameter]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the alternate text for the image.
    /// </summary>
    /// <remarks>
    /// Required for informative images. Use an empty string only for decorative images per WCAG.
    /// </remarks>
    /// <value>Alternate text, or <see langword="null"/>.</value>
    [Parameter]
    public string? Alt { get; set; }

    /// <summary>
    /// Gets or sets the CSS height of the media region.
    /// </summary>
    /// <value>A CSS length; the default is 180px.</value>
    [Parameter]
    public string? Height { get; set; } = "180px";

    /// <summary>
    /// Gets or sets how the image is fitted within the media box.
    /// </summary>
    /// <value>The default is <see cref="CardMediaFit.Cover"/>.</value>
    [Parameter]
    public CardMediaFit ObjectFit { get; set; } = CardMediaFit.Cover;

    /// <summary>
    /// Gets or sets custom cover content instead of ImageUrl.
    /// </summary>
    /// <value>A <see cref="RenderFragment"/>; the default is <see langword="null"/>.</value>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets additional CSS class names for the media element.
    /// </summary>
    [Parameter]
    public string? CssClass { get; set; }

    /// <summary>
    /// Gets or sets additional HTML attributes for the media element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? HtmlAttributes { get; set; }
}
```

---

## Warning-prevention checklist

| Warning | Prevention |
|---------|------------|
| CS1591 | XML on all public types/members |
| CS8618 | Defaults or nullable `?` |
| CS8767 | Match EventCallback nullability |
| Malformed XML | Well-formed tags; escape less-than in text |

## csproj snippet

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```
