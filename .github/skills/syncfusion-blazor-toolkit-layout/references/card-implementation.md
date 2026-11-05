# Card Implementation and Accessibility

## Render modes

`SfCard` renders Static SSR presentation markup without JavaScript interop.
Interactive render mode is required for clickable activation callbacks:
`OnClick` cannot fire unless the host application is Server, WebAssembly, or
Auto interactive mode.

The root markup includes:

- a stable generated or caller-supplied id;
- merged root classes and styles;
- `role="button"` and `tabindex="0"` only for clickable Cards;
- `aria-busy="true"` while loading;
- `aria-disabled="true"` for clickable Cards while loading; and
- `data-elevation`, `data-content-padding`, and theme-related Card classes.

## Keyboard and pointer activation

When `IsClickable` is true and `IsLoading` is false, the root handles:

- pointer clicks through the generated mouse click handler;
- Enter activation on key down; and
- Space activation on key up.

Keys are handled by the `scripts/card.js` module, which the Card loads only
after it first renders as clickable. The module reacts only to keys pressed
while the Card element itself has focus, cancels the default action of Enter
and Space (so Space does not scroll the page), and activates the Card with a
native `click()`. Keys pressed in a nested input, link, or button are left
alone. Keyboard activation therefore arrives as `MouseEventArgs` with
`Detail == 0`. Repeated keys and loading states do not raise `OnClick`.

## Region isolation

`SfCardActions` is a cascading child of `SfCard`. It adds a no-op mouse
isolation handler only when the parent is clickable, preventing
activation of the parent Card while a nested action is used. When the parent is
loading, it also renders `inert`, removing controls from interaction and
assistive-technology traversal.

Other regions are rendered without event isolation.

## HTML attribute and style merging

Root and region components merge computed class/style values with caller
supplied `CssClass`, `HtmlAttributes["class"]`, and
`HtmlAttributes["style"]`. Unmatched attributes are filtered into the rendered
element. Height and width are formatted through `SfBaseUtils.FormatUnit`, which
converts bare numeric strings to pixel units.

## Elevation behavior

Elevation is resolved on each parameter update:

- values below zero become zero and values above five become five;
- `Outlined` and `Flat` variants always resolve to elevation zero; and
- `Elevated` adds two levels to a positive configured elevation, capped at five.

The rendered `data-elevation` value reflects the resolved level.

## Loading behavior

Loading changes both presentation state and interaction state:

- the root receives loading and disabled classes;
- `aria-busy="true"` is emitted;
- clickable Cards emit `aria-disabled="true"`; and
- pointer and keyboard activation handlers return without invoking `OnClick`.

The actions region is rendered with the HTML `inert` attribute while loading.
Use `IsLoading` to represent an unavailable or pending Card rather than adding
custom click guards around nested controls.

## Stable markup identity

The Card generates an `sf-card-` id from the component context when `Id` is
not supplied. The generated id is initialized once and remains stable across
parameter updates unless a caller supplies a different `Id`.