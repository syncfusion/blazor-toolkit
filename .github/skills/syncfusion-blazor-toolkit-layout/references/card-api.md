# Card API Reference

`SfCard` and its child components are in the `Syncfusion.Blazor.Toolkit`
namespace.

## SfCard parameters

| Parameter | Type | Default | Purpose |
|---|---|---:|---|
| `ChildContent` | `RenderFragment?` | `null` | Card regions or arbitrary markup |
| `Variant` | `CardVariant` | `Filled` | Filled, outlined, elevated, or flat surface |
| `Orientation` | `CardOrientation` | `Vertical` | Vertical or horizontal region layout |
| `Elevation` | `int` | `1` | Shadow depth, clamped to 0-5 |
| `Square` | `bool` | `false` | Disables rounded corners |
| `Hoverable` | `bool` | `false` | Adds hover and focus-within affordance |
| `IsLoading` | `bool` | `false` | Sets busy presentation and disables activation |
| `ContentPadding` | `bool` | `true` | Applies theme content padding |
| `Width` | `string?` | `null` | CSS width |
| `Height` | `string?` | `null` | CSS height |
| `CssClass` | `string?` | `null` | Additional root classes |
| `Id` | `string?` | `null` | Root element id; generated when omitted |
| `HtmlAttributes` | `Dictionary<string, object>?` | `null` | Unmatched HTML attributes |
| `IsClickable` | `bool` | `false` | Exposes a keyboard-focusable button root |
| `AriaLabel` | `string?` | `null` | Accessible name for the Card |
| `OnClick` | `EventCallback<MouseEventArgs>` | empty | Pointer, Enter, and Space activation |

## Enums

- `CardVariant.Filled`: solid themed surface.
- `CardVariant.Outlined`: border emphasis, no shadow.
- `CardVariant.Elevated`: stronger shadow using elevation plus the elevated variant adjustment.
- `CardVariant.Flat`: minimal surface, no border or shadow.
- `CardOrientation.Vertical`: header-to-footer vertical flow.
- `CardOrientation.Horizontal`: primary regions in an inline row.
- `CardMediaFit.Cover`: fill and crop.
- `CardMediaFit.Contain`: contain within the media box.
- `CardMediaFit.Fill`: stretch.
- `CardMediaFit.None`: no object-fit override.

## Child components

| Component | Purpose | Parameters |
|---|---|---|
| `SfCardHeader` | Header region containing titles or summary content | `ChildContent`, `CssClass`, `HtmlAttributes` |
| `SfCardTitle` | Neutral title region without an implicit heading element | `ChildContent`, `CssClass`, `HtmlAttributes` |
| `SfCardSubTitle` | Supporting subtitle or metadata text | `ChildContent`, `CssClass`, `HtmlAttributes` |
| `SfCardMedia` | Image cover or custom media region | `ImageUrl`, `Alt`, `Height = "180px"`, `ObjectFit = CardMediaFit.Cover`, `ChildContent`, `CssClass`, `HtmlAttributes` |
| `SfCardContent` | Primary body region | `ChildContent`, `CssClass`, `HtmlAttributes` |
| `SfCardActions` | Action row isolated from clickable parent activation | `ChildContent`, `CssClass`, `HtmlAttributes` |
| `SfCardSeparator` | Accessible divider oriented from the parent Card | `ChildContent`, `CssClass`, `HtmlAttributes` |
| `SfCardFooter` | Footer region; padding follows the parent Card setting | `ChildContent`, `CssClass`, `HtmlAttributes` |

Every region supports `ChildContent`, `CssClass`, and unmatched HTML attributes.

## Region behavior

- `SfCardTitle` does not create a heading. Add an explicit semantic heading
  element when the Card participates in the document outline.
- `SfCardMedia` renders an `<img>` when `ImageUrl` is supplied; otherwise it
  renders `ChildContent`.
- `Alt` maps directly to the image's `alt` attribute. A null or omitted value
  renders an empty alternate text.
- `SfCardActions` stops mouse and keyboard bubbling under a clickable Card.
- `SfCardActions` renders `inert` while its cascading parent has
  `IsLoading="true"`.
- `SfCardSeparator` renders `role="separator"` with horizontal orientation for
  vertical Cards and vertical orientation for horizontal Cards.
- `SfCardFooter` applies Card padding through the theme variable when padding is
  enabled.

## HTML attribute behavior

`HtmlAttributes` is a capture-unmatched-values parameter. Computed `class` and
`style` values are merged with caller values. Other unmatched attributes are
applied to the element; caller attributes override matching computed values
where the implementation does not merge them.
