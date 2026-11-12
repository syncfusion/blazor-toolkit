# Navigation Accessibility

## Toolbar

- The root has `role="toolbar"` and `aria-orientation`.
- Supply `AriaLabel` or `aria-labelledby`; do not leave the toolbar landmark
  unnamed. Name groups with `ToolbarGroup.Label`.
- The toolbar uses one roving tab stop. Horizontal arrow keys follow direction;
  vertical toolbars use Up/Down. Home and End move to the first/last item.
  Disabled, invisible, separators, and spacers are skipped.
- Enabled selectable items expose `aria-pressed`. Nested commands use menu
  semantics, and the overflow popup uses `role="menu"`.
- Set `ToolbarItem.Text` for every icon-only item. It is retained as the
  accessible name even when `DisplayMode` hides the visible text.
- A custom `Template` is responsible for making its own interactive content
  keyboard-accessible and for sensible focus handling. Call `FocusAsync()` to
  enter the toolbar from outside.
- In Popup mode, the overflow button uses menu semantics; in Extended mode it
  is an expand/collapse control instead. Do not describe the latter as a menu.

## Sidebar

- Supply `AriaLabel` or `AriaLabelledBy`. `AriaLabelledBy` takes precedence;
  when absent, the component falls back to a localized Sidebar label.
- The default role is `navigation`, except a modal overlay with a backdrop,
  which uses `dialog` and `aria-modal="true"`. Set an explicit `Role` only
  when the content semantics justify it.
- The modal overlay moves focus into the panel, traps Tab/Shift+Tab, makes
  target content inert, and returns focus to the opener when the overlay
  closes. Use `FocusAsync()` when deliberately moving focus into a visible
  panel; it focuses the first focusable descendant or the panel itself.
- Ensure the control that opens the sidebar exposes `aria-controls` pointing
  at the panel `Id`, and an accurate `aria-expanded` value. The component does
  not render this external trigger for you.
- `OpenMiniOnHover` is pointer-only; provide an accessible keyboard-operable
  toggle or public API path to open the rail.

## Styling and Contrast

Toolbar styles use its isolated stylesheet and `::deep` for its separately
rendered item components; theme values and logical properties support RTL and
forced colors. Sidebar supplies component styling and reduced-motion behavior.
Avoid overriding focus indicators or relying on color alone to convey
selection/state. Keep consumer CSS scoped to component classes.

## Validation Checklist

- Name each toolbar using `AriaLabel` or `aria-labelledby`; label every group
  with `ToolbarGroup.Label`.
- Supply descriptive `Text` for every toolbar item, particularly
  `IconOnly` items. Tooltip text is supplemental.
- Preserve the toolbar's single roving tab stop. Horizontal arrows follow
  LTR/RTL, vertical navigation uses Up/Down, and Home/End move to the
  boundaries. Disabled, invisible, separator, and spacer items are skipped.
- Verify menu keyboard behavior: Escape closes an open nested/overflow menu;
  Popup overflow restores focus when dismissed. Extended mode's expand control
  is not a menu.
- Ensure custom Toolbar templates provide their own accessible interactive
  content, focus handling, and state announcements.
- Name Sidebar with `AriaLabel` or `AriaLabelledBy`; the latter takes
  precedence. The default role is navigation, while a modal Overlay with an
  active backdrop uses dialog and `aria-modal="true"`.
- Keep the external Sidebar trigger's `aria-controls` pointed at the panel
  `Id` and its `aria-expanded` synchronized with `IsOpen`.
- For modal Overlay, verify focus entry, Tab/Shift+Tab containment, inert
  target content, and focus restoration. Push/Shrink without a backdrop are
  non-modal and should not be treated as a focus trap.
- Provide a keyboard-operated Sidebar trigger; hover expansion alone is not
  accessible. Respect reduced motion and preserve visible focus indicators.
