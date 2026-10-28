# SfCard — Public API Reference
## Syncfusion.Blazor.Toolkit

| Field | Value |
|-------|--------|
| **API version** | 1.0.0-proposed |
| **Namespace** | `Syncfusion.Blazor.Toolkit` |

---

## 1. Components

| Type | Role |
|------|------|
| `SfCard` | Root surface |
| `SfCardHeader` | Header region |
| `SfCardTitle` | Title |
| `SfCardSubTitle` | Subtitle |
| `SfCardMedia` | Image/cover |
| `SfCardContent` | Body |
| `SfCardActions` | Actions row |
| `SfCardFooter` | Footer |
| `SfCardSeparator` | Divider |

---

## 2. Enums

```csharp
public enum CardVariant { Filled = 0, Outlined = 1, Elevated = 2, Flat = 3 }
public enum CardOrientation { Vertical = 0, Horizontal = 1 }
public enum CardMediaFit { Cover = 0, Contain = 1, Fill = 2, None = 3 }
```

---

## 3. SfCard parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ChildContent` | `RenderFragment?` | null | Building blocks / free content |
| `Variant` | `CardVariant` | `Filled` | Visual variant |
| `Orientation` | `CardOrientation` | `Vertical` | Layout direction |
| `Elevation` | `int` | `1` | Shadow depth; 0 = none |
| `Square` | `bool` | `false` | No border-radius |
| `Hoverable` | `bool` | `false` | Hover affordance |
| `IsLoading` | `bool` | `false` | Loading state |
| `ContentPadding` | `bool` | `true` | Region padding |
| `Width` | `string?` | null | CSS width |
| `Height` | `string?` | null | CSS height |
| `CssClass` | `string?` | null | Extra classes |
| `Id` | `string?` | auto | Element id |
| `HtmlAttributes` | `Dictionary<string, object>?` | null | Attribute splat |
| `IsClickable` | `bool` | `false` | Root interactive |
| `OnClick` | `EventCallback<MouseEventArgs>` | — | Root activation |
| `AriaLabel` | `string?` | null | Accessible name |

---

## 4. SfCardMedia parameters

| Parameter | Type | Default |
|-----------|------|---------|
| `ImageUrl` | `string?` | null |
| `Alt` | `string?` | null |
| `Height` | `string?` | `"180px"` |
| `ObjectFit` | `CardMediaFit` | `Cover` |
| `ChildContent` | `RenderFragment?` | null |
| `CssClass` | `string?` | null |
| `HtmlAttributes` | `Dictionary<string, object>?` | null |

---

## 5. Region components

Shared parameters: `ChildContent`, `CssClass`, `HtmlAttributes`.

---

## 6. CSS classes

`e-card`, `e-card-horizontal`, `e-card-outlined`, `e-card-elevated`, `e-card-flat`, `e-card-square`, `e-card-hoverable`, `e-card-loading`, `e-card-header`, `e-card-title`, `e-card-subtitle`, `e-card-media`, `e-card-content`, `e-card-actions`, `e-card-footer`, `e-card-separator`

---

## 7. Example

```razor
@using Syncfusion.Blazor.Toolkit

<SfCard Variant="CardVariant.Outlined" Width="280px">
    <SfCardMedia ImageUrl="images/product.jpg" Alt="Product" />
    <SfCardHeader>
        <SfCardTitle>Trail Runner</SfCardTitle>
        <SfCardSubTitle>Outdoor</SfCardSubTitle>
    </SfCardHeader>
    <SfCardContent>Lightweight daily trainer.</SfCardContent>
    <SfCardActions>
        <SfButton Content="Buy" IsPrimary="true" />
        <SfButton Content="Details" />
    </SfCardActions>
</SfCard>
```
