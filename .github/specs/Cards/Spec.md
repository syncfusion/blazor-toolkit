# Technical Specification
## SfCard — Syncfusion Toolkit for Blazor

| Field | Value |
|-------|--------|
| **Document** | Technical Specification |
| **Version** | 1.0 |
| **Component family** | `SfCard`, `SfCardHeader`, `SfCardTitle`, `SfCardSubTitle`, `SfCardMedia`, `SfCardContent`, `SfCardActions`, `SfCardFooter`, `SfCardSeparator` |
| **Namespace** | `Syncfusion.Blazor.Toolkit` |
| **Companion docs** | PRD.md, API.md, competitive-matrix.md, TEST-MATRIX.md, API-xml.md |

---

## 1. Purpose and scope

### 1.1 Purpose

Composable rectangular surface for a single subject: text, media, and actions. Benchmark against MudCard, TelerikCard, Ant Card, RadzenCard, Blazorise Card.

### 1.2 In scope

- Root card + region building blocks
- Variants, orientation, elevation, hoverable, loading, optional click
- CSS isolation, forced-colors, RTL, WCAG-oriented a11y
- Fully automated tests (see TEST-MATRIX.md)

### 1.3 Out of scope

- Virtualized CardView data product
- Built-in tabs inside card
- Proprietary layout runtime for decks (samples/CSS only)

### 1.4 Design principles

1. Composition-first (optional regions; free ChildContent)
2. Blazor-first (C# + CSS; minimal JS)
3. Accessible by default
4. Theme-native (`SfThemeRoot` tokens)
5. Test-or-it-does-not-exist

---

## 2. Definitions

| Term | Definition |
|------|------------|
| **Root** | `SfCard` host element |
| **Region** | Header, Media, Content, Actions, Footer, Separator, Title, SubTitle |
| **Variant** | Visual chrome (Filled, Outlined, Elevated, Flat) |
| **Clickable card** | Root activates via pointer/keyboard when `IsClickable` |

---

## 3. Architecture

```
SfCard (root)
├── SfCardHeader        // optional
│   ├── SfCardTitle / SfCardSubTitle / Extra
├── SfCardMedia         // optional image/cover
├── SfCardContent       // body
├── SfCardSeparator     // optional
├── SfCardActions       // action row
└── SfCardFooter        // optional
```

No child is required. Order of regions in markup controls visual order.

---

## 4. Appearance

### 4.1 Variants

| Variant | Behavior |
|---------|----------|
| Filled | Solid surface + default elevation |
| Outlined | Border emphasis, lighter fill |
| Elevated | Stronger shadow; uses `Elevation` |
| Flat | Minimal chrome, little/no shadow |

### 4.2 Other

| Rule | Spec |
|------|------|
| Square | No border-radius when true |
| Elevation | 0 = none; map to theme shadow tokens |
| Width/Height | CSS length strings on root |
| ContentPadding | Default true; regions honor padding tokens |

### 4.3 Orientation

| Value | Layout |
|-------|--------|
| Vertical (default) | Stack top to bottom |
| Horizontal | Flex row; RTL reverses inline direction |

---

## 5. Behavior

| Feature | Spec |
|---------|------|
| Hoverable | Lift/border on :hover / :focus-within; pointer if clickable |
| IsLoading | Loading presentation; aria-busy=true; disable or inert actions |
| IsClickable + OnClick | Root click/Enter/Space; nested buttons stopPropagation |
| Static SSR | Full visual render; OnClick needs interactivity |

---

## 6. Accessibility

| State | Requirements |
|-------|----------------|
| Static | No fake button role; use real headings in title |
| Clickable | tabindex=0, keyboard Enter/Space, accessible name |
| Loading | aria-busy=true |
| Media | Alt for informative images; empty alt only if decorative |

WCAG focus: 1.4.3, 2.1.1 (if clickable), 2.4.7, 4.1.2.

---

## 7. Styling and CSS isolation

- Files: `SfCard.razor.css` and region CSS as needed
- Public classes: `e-card`, `e-card-horizontal`, `e-card-outlined`, `e-card-elevated`, `e-card-flat`, `e-card-square`, `e-card-hoverable`, `e-card-loading`, `e-card-header`, `e-card-title`, `e-card-subtitle`, `e-card-media`, `e-card-content`, `e-card-actions`, `e-card-footer`, `e-card-separator`
- `@media (forced-colors: active)` borders/focus
- `prefers-reduced-motion` for hover motion

---

## 8. Automated testing

See **TEST-MATRIX.md** for full IDs.

Layers: L1 bUnit, L3 Playwright, L4 axe, L5 visual, L6 zero warnings (XML via API-xml.md).

L4 is the axe deliverable table in TEST-MATRIX.md section 6. That table is required. It names the axe sample host (`accessibility/Blazor.Toolkit.Accessibility.Samples`), the five routes, `accessibility/Blazor.Toolkit.AxeCore.Tests/Tests/CardTests.cs`, and the Card entry in `accessibility/config.json`. L4 is not done until those files exist.

---

## 9. File layout

```
src/.../Card/
  SfCard.razor(.cs/.css)
  SfCardHeader.razor(.cs)
  SfCardTitle.razor(.cs)
  SfCardSubTitle.razor(.cs)
  SfCardMedia.razor(.cs)
  SfCardContent.razor(.cs)
  SfCardActions.razor(.cs)
  SfCardFooter.razor(.cs)
  SfCardSeparator.razor(.cs)
  CardVariant.cs
  CardOrientation.cs
  CardMediaFit.cs
docs/card/
  PRD.md Spec.md API.md competitive-matrix.md TEST-MATRIX.md API-xml.md
```

---

## 10. Defaults

| Parameter | Default |
|-----------|---------|
| Variant | Filled |
| Orientation | Vertical |
| Elevation | 1 |
| Square | false |
| Hoverable | false |
| IsLoading | false |
| ContentPadding | true |
| IsClickable | false |
