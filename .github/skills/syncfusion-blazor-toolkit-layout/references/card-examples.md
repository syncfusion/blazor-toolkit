# Card Examples and Anti-Patterns

## Static Card

```razor
<SfCard Variant="CardVariant.Outlined" Width="280px">
    <SfCardHeader>
        <SfCardTitle><h3>Account</h3></SfCardTitle>
        <SfCardSubTitle>Personal profile</SfCardSubTitle>
    </SfCardHeader>
    <SfCardContent>Profile details and preferences.</SfCardContent>
</SfCard>
```

Static Cards have no button role and should not add activation handlers.

## Loading Card

```razor
<SfCard IsLoading="@isLoading" IsClickable="true" OnClick="OpenAsync">
    <SfCardHeader>
        <SfCardTitle><h3>Order summary</h3></SfCardTitle>
    </SfCardHeader>
    <SfCardActions>
        <SfButton Content="Refresh" />
    </SfCardActions>
</SfCard>
```

While `isLoading` is true, activation is blocked and actions are inert.

## Horizontal Card

```razor
<SfCard Orientation="CardOrientation.Horizontal" Width="520px">
    <SfCardMedia ImageUrl="images/preview.jpg"
                 Alt="Preview"
                 Height="160px" />
    <SfCardContent>Horizontal media and content layout.</SfCardContent>
</SfCard>
```

## Media and Custom Body

```razor
<SfCard Variant="CardVariant.Elevated" Elevation="2">
    <SfCardMedia ImageUrl="/images/card-cover.jpg"
                 Alt="Mountain trail"
                 Height="220px"
                 ObjectFit="CardMediaFit.Cover" />
    <SfCardHeader>
        <SfCardTitle><h3>Alpine Trail</h3></SfCardTitle>
        <SfCardSubTitle>Weekend itinerary</SfCardSubTitle>
    </SfCardHeader>
    <SfCardContent>
        <p>See elevations, weather, and route details before departure.</p>
    </SfCardContent>
</SfCard>
```

## Custom Media Content

```razor
<SfCard Width="360px">
    <SfCardMedia Height="160px">
        <div class="custom-card-media">
            Custom media markup
        </div>
    </SfCardMedia>
</SfCard>
```

When `ImageUrl` is null or whitespace, `SfCardMedia` renders `ChildContent`.

## Clickable Card with Isolated Actions

```razor
<SfCard IsClickable="true"
        AriaLabel="Open Project Atlas"
        OnClick="OpenProjectAsync">
    <SfCardHeader>
        <SfCardTitle><h3>Project Atlas</h3></SfCardTitle>
    </SfCardHeader>
    <SfCardContent>Infrastructure modernization.</SfCardContent>
    <SfCardActions>
        <SfButton Content="Share"
                  OnClick="ShareAsync" />
    </SfCardActions>
</SfCard>
```

Clicks and keyboard events from `SfCardActions` do not activate the parent Card
under a clickable Card.

## Horizontal Card with Separator

```razor
<SfCard Orientation="CardOrientation.Horizontal" Width="640px">
    <SfCardContent>Main summary</SfCardContent>
    <SfCardSeparator />
    <SfCardFooter>Secondary metadata</SfCardFooter>
</SfCard>
```

The separator automatically uses vertical `aria-orientation` for horizontal
Cards.

## Anti-Patterns

| Anti-pattern | Problem | Correct approach |
|---|---|---|
| Adding `OnClick` without `IsClickable="true"` | Activation is not enabled | Set both only when the whole Card should activate |
| Manually adding `role="button"` to a static Card | Creates a fake interactive control | Use `IsClickable="true"` |
| Putting an `SfButton` directly beside `SfCard` body without `SfCardActions` | It may be visually inconsistent and activation isolation is less explicit | Place controls in `SfCardActions` |
| Using a clickable Card with no label and no readable text | Screen readers may not identify its purpose | Set `AriaLabel` or provide meaningful readable content |
| Setting `Alt=""` for an informative image | Image information is lost | Provide an accurate image description in `Alt` |
| Removing `<SfCardSeparator>` orientation behavior | Accessibility validation may reject the separator | Keep the separator as a child of the Card and let it derive orientation |
| Overriding class and style attributes instead of merging | Computed Card classes/styles can be lost | Use `CssClass` and `HtmlAttributes` merge support |
