# Chips

Use chips to present compact labels, statuses, filters, choices, suggestions,
or navigation actions. Choose the chip type for its behavior, not only its
appearance.

## Choose a component

- Use **`SfChip`** for one standalone chip. Set `Text` or provide
  `ChildContent`; when supplied, `ChildContent` replaces `Text`.
- Use **`SfChipSet<TItem>`** for a collection. Bind `DataSource` to project
  chips from items, or provide declarative `SfChip` children. When both
  `DataSource` and `ChildContent` are set, `DataSource` takes precedence.
  Removing an item hides it from the rendered set; the set does not modify the
  caller's data collection.

## Chip types

`ChipType` selects the chip's functional mode:

| Type | Behavior |
|---|---|
| `Informational`, `Status` | Passive content; does not activate. |
| `Action`, `Assist`, `Suggestion` | Button activation. |
| `Filter`, `Choice` | Button selection with `aria-pressed`. |
| `Input` | Represents a token; it is not an activation mode. Enable `Removable` when users need to remove it. |
| `Navigation` | Renders as a link when `Href` is set. Without `Href`, it falls back to a button and writes a debug diagnostic. |
| `Custom` | Passive custom content; use templates and attributes to shape its presentation. |

`Filter` and `Choice` chips can be selected. A standalone chip toggles its
`Selected` value and raises `SelectedChanged`. In a set, use `SelectionMode`
to manage selection across chips.

Use `Variant`, `Size`, and `Shape` for visual presentation. `IconCss`,
`ImageUrl`, and `ImageAlt` configure leading imagery; `Href`, `Target`, and
`Rel` configure navigation links. `Disabled`, `ReadOnly`, `IsLoading`, and
`HasError` control state. `OnClick` handles standalone activation.

## Selection and data

`SfChipSet<TItem>.SelectionMode` accepts `ChipSelectionMode.None` (the
default), `Single`, or `Multiple`. Bind `SelectedValue` for single selection
or `SelectedValues` for multiple selection. `AllowEmpty` defaults to `true`;
set it to `false` when a selected value must remain. `SelectionChanging` can
cancel a proposed change, and `SelectionChanged` reports a committed change.
`OnChipClick` reports activation.

When a set uses `ChipType.Choice`, `ChipSelectionMode.Multiple` is coerced to
single selection and writes a debug warning.

For data-bound sets, use selector parameters such as `TextSelector`,
`ValueSelector`, `DisabledSelector`, and `RemovableSelector` to map item
properties. Alternatively, set `Fields` to a `ChipFields` property-name map.
Selectors take precedence over the corresponding mapped fields. Provide a
stable `ItemKey` for object items so selection and rendering remain stable
when the collection changes.

When using declarative child chips, set `ChipType` on each child that should
activate or participate in selection. A set's `ChipType` default applies to
data-projected chips.

### Standalone chip

```razor
@using Syncfusion.Blazor.Toolkit

<SfChip ChipType="@ChipType.Status" Text="Approved" />
<SfChip ChipType="@ChipType.Filter"
        Text="In stock"
        @bind-Selected="_inStock" />

@code {
    private bool _inStock;
}
```

### Declarative set

```razor
@using Syncfusion.Blazor.Toolkit

<SfChipSet TItem="string"
           SelectionMode="@ChipSelectionMode.Multiple"
           @bind-SelectedValues="_selectedValues">
    <SfChip ChipType="@ChipType.Filter" Text="Open" Value="open" />
    <SfChip ChipType="@ChipType.Filter" Text="Assigned" Value="assigned" />
</SfChipSet>

@code {
    private IEnumerable<object?> _selectedValues = Array.Empty<object?>();
}
```

### Data-bound set

```razor
@using Syncfusion.Blazor.Toolkit

<SfChipSet TItem="FilterOption"
           DataSource="@_filters"
           ItemKey="@(item => item.Id)"
           TextSelector="@(item => item.Label)"
           ValueSelector="@(item => item.Id)"
           ChipType="@ChipType.Filter"
           SelectionMode="@ChipSelectionMode.Multiple"
           @bind-SelectedValues="_selectedValues" />

@code {
    private readonly List<FilterOption> _filters =
    [
        new("open", "Open"),
        new("assigned", "Assigned")
    ];

    private IEnumerable<object?> _selectedValues = Array.Empty<object?>();

    private sealed record FilterOption(string Id, string Label);
}
```

## Removal and layout

Set `Removable` to show the remove control. `Removing` runs before removal and
can cancel it; `Removed` runs after a successful removal. In a chip set,
removal hides the item from the rendered set but does not mutate `DataSource`.
`Delete` and `Backspace` request removal when removal is enabled.

`OverflowMode` controls the set layout:

- `Wrap` (default) places chips on additional lines.
- `Scroll` keeps chips on one horizontally scrollable line.
- `Collapse` folds overflowing chips behind an expand control. Collapse
  measurement uses JavaScript through `chip.js` and requires interactive
  rendering for measurement.

Use `ChipTemplate`, `LeadingTemplate`, `TrailingTemplate`, and `RemoveTemplate`
to customize projected chip content. Sets also provide `LoadingTemplate`,
`ErrorTemplate`, and `EmptyTemplate`; `OverflowExpandTemplate` customizes the
collapse expand control.

## Rendering and accessibility

Passive chip markup can render during static server-side rendering. Browser
event handling—including activation, selection, and removal—requires an
interactive render mode. Collapse overflow also requires interactivity for
its JavaScript layout measurement.

Chip roots use native button, link, or span elements according to type.
Interactive chips support `Enter` and `Space`; a set supports arrow-key
navigation and `Home`/`End`, with horizontal movement aware of right-to-left
layout. Set `EnableRtl` for right-to-left chip layout. Filter and choice chips
expose `aria-pressed`; disabled links expose `aria-disabled`.

Use `AriaLabel`, `AriaLabelledBy`, or `AriaDescribedBy` when visible text does
not provide a sufficient accessible name or description. `HtmlAttributes`
adds attributes to the chip root. For removable chips, provide meaningful
`Text` or `AriaLabel` because the localized remove control name includes the
chip name (`Remove {name}`). Supply useful `ImageAlt` text for informative
images.
