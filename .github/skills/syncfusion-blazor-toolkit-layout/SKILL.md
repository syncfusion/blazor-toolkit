---
license: MIT
name: syncfusion-blazor-toolkit-layout
description: >
  Implement Syncfusion Blazor Toolkit layout components, starting with SfCard
  and supporting the Layout category as additional components are added.
  USE FOR: Card variants and orientations, header, title, subtitle, media, body,
  actions, separator, footer, loading, clickable activation, custom styling,
  HTML attributes, keyboard behavior, and accessibility.
  REQUIRES interactive render mode only for Card.OnClick; static presentation
  works in Static SSR, Server, WebAssembly, and Auto.
  DO NOT USE FOR: page navigation components, dialogs, tooltips, data grids,
  or form containers (use the corresponding Toolkit skill).
compatibility: ".NET 8+, render-modes: Static SSR for presentation; interactive modes for OnClick"
metadata:
  author: "Syncfusion Inc"
  version: "1.0.0"
---

# Syncfusion Blazor Toolkit Layout Components

## When to Use Card

Use `SfCard` when the interface needs a composable, themed content surface for
one subject: a profile summary, product detail, dashboard item, media preview,
notification summary, or selectable content region.

Use `SfCard` when the layout can be composed from optional header, media, body,
action, separator, and footer regions. Use ordinary semantic markup instead
when a Card would add no grouping, visual hierarchy, or interaction.

## Setup

The Layout components use the main Syncfusion Blazor Toolkit package and the
same public namespace as the other Toolkit components.

```razor
@using Syncfusion.Blazor.Toolkit
```

Register the Toolkit service in the application host before using components:

```csharp
builder.Services.AddSyncfusionBlazorToolkit();
```

Presentation-only Cards can render in Static SSR. Interactive callback
behavior requires a configured interactive render mode.

## Core Rules

1. Keep the Card implementation under `src/Components/Layout/Card/` and use
   the public namespace `Syncfusion.Blazor.Toolkit`.
2. Use `<SfCard>` as the root and compose regions in markup order:
   `<SfCardHeader>`, `<SfCardMedia>`, `<SfCardContent>`,
   `<SfCardActions>`, `<SfCardSeparator>`, and `<SfCardFooter>`.
3. `IsClickable="true"` exposes the Card root as a keyboard-focusable button.
   `OnClick` works only in interactive render modes and only when
   `IsLoading` is false.
4. Enter activates on key down and Space activates on key up, matching native
   button behavior. Preserve the existing event and prevent-default handling.
5. `<SfCardActions>` must isolate nested controls from Card activation. Do not
   remove its isolation handlers or make `<SfCardActions>` recursively clickable.
6. While `IsLoading="true"`, Card exposes `aria-busy="true"`,
   `aria-disabled="true"` for clickable Cards, prevents activation, and makes
   actions inert.
7. Static Cards do not expose `role="button"` and do not need interactivity.
8. Use `ContentPadding="false"` only when every region should deliberately use
   its own padding. Separator and footer behavior depends on the parent Card
   orientation and padding settings.
9. `HtmlAttributes` captures unmatched HTML attributes. Merged `class` and
   `style` values must preserve computed Card and region values.

## Supported Card API

See [Card API reference](references/card-api.md) for parameters, child
components, events, enums, and default values.

## Implementation Guidance

- `Variant` controls fill, outline, elevation, and shadow behavior.
- `Orientation="CardOrientation.Horizontal"` lays primary regions out in a
  horizontal flex row; separators use the vertical ARIA orientation.
- `Elevation` is clamped to 0-5 and only affects filled/elevated presentation.
  `CardVariant.Outlined` and `CardVariant.Flat` never render a shadow.
- `Width` and `Height` accept CSS lengths; bare numeric strings are converted to
  pixels by the existing utility.
- `Square`, `Hoverable`, `ContentPadding`, `CssClass`, `Id`, and
  `HtmlAttributes` customize presentation without changing the public behavior
  of the component.
- `ChildContent` can contain region components or arbitrary markup.

## Parameter Selection

| Requirement | Recommended Card configuration |
|---|---|
| Default themed surface | `Variant="CardVariant.Filled"` |
| Border emphasis without shadow | `Variant="CardVariant.Outlined"` |
| Strong visual emphasis | `Variant="CardVariant.Elevated"` with a supported elevation |
| Minimal surface without border or shadow | `Variant="CardVariant.Flat"` |
| Side-by-side regions | `Orientation="CardOrientation.Horizontal"` |
| Media-image Card | `<SfCardMedia ImageUrl="..." Alt="..." />` |
| Custom media or overlays | Use `<SfCardMedia>` with `ChildContent` |
| Whole Card activation | Set `IsClickable="true"` and provide `OnClick` |
| Nested buttons | Place them in `<SfCardActions>` |
| Temporary unavailable content | Set `IsLoading="true"` |
| Square presentation | Set `Square="true"` |
| Hover affordance | Set `Hoverable="true"` |

## Accessibility and Interaction

- Give clickable Cards a visible name or meaningful `AriaLabel`.
- Static Card roots expose no role; do not add `role="button"` unless the root
  intentionally activates.
- Keep nested interactive controls in `<SfCardActions>`.
- `SfCardTitle` is neutral by default. Add a real heading such as `<h3>` inside
  it when the Card title belongs in the page heading outline.
- For `<SfCardMedia>`, provide `Alt` for informative images. Leave `Alt` unset
  only for decorative images, which render an empty `alt`.
- `<SfCardSeparator>` derives `aria-orientation` from the parent Card
  orientation and must remain a valid `role="separator"`.

## Example

```razor
@using Syncfusion.Blazor.Toolkit

<SfCard Variant="CardVariant.Outlined"
        Width="320px"
        IsClickable="true"
        AriaLabel="Open the Trail Runner details"
        OnClick="OpenAsync">
    <SfCardHeader>
        <SfCardTitle><h3>Trail Runner</h3></SfCardTitle>
        <SfCardSubTitle>Outdoor collection</SfCardSubTitle>
    </SfCardHeader>
    <SfCardMedia ImageUrl="images/trail-runner.jpg"
                 Alt="Trail Runner shoe"
                 Height="180px"
                 ObjectFit="CardMediaFit.Cover" />
    <SfCardContent>Lightweight cushioning for daily outdoor runs.</SfCardContent>
    <SfCardActions>
        <SfButton Content="View details" />
    </SfCardActions>
    <SfCardFooter>Updated today</SfCardFooter>
</SfCard>

@code {
    private Task OpenAsync(MouseEventArgs args)
    {
        return Task.CompletedTask;
    }
}
```

## References

- [Card API reference](references/card-api.md)
- [Card implementation and accessibility](references/card-implementation.md)
- [Card examples and anti-patterns](references/card-examples.md)
