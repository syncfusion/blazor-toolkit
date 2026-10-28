## SfCard — Sample preparer

| Field | Value |
|-------|--------|
| **Component** | `SfCard` family |
| **Host** | `samples/Blazor.Toolkit.Samples.Client` |
| **Catalog route** | `/cards/card` |
| **Demo namespace** | `Blazor.Toolkit.Samples.Client.Pages.Demos.Cards.Card` |

This file is the sample contract for Card. It lists the pages that exist, what each page demonstrates, and where it is registered. It does not change component source.

---

## 1. Catalog page

| Item | Path |
|------|------|
| Page | `samples/Blazor.Toolkit.Samples.Client/Pages/Components/Cards/Card.razor` |
| Demos | `samples/Blazor.Toolkit.Samples.Client/Pages/Demos/Cards/Card/` |
| Route | `@page "/cards/card"` |
| Base | `TocPublishingComponentBase` |
| Preview | `CodePreview` with `PreviewComponent` and `SourceFile` |

Each section is a `Description` heading plus one `CodePreview`. Heading ids match the right-hand TOC.

| TOC id | Heading | Demo | SourceFile |
|--------|---------|------|------------|
| — | Blazor Toolkit Card Component | `DefaultDemo` | `/Pages/Demos/Cards/Card/DefaultDemo.razor` |
| `variants` | Variants | `VariantsDemo` | `/Pages/Demos/Cards/Card/VariantsDemo.razor` |
| `media` | Media | `MediaDemo` | `/Pages/Demos/Cards/Card/MediaDemo.razor` |
| `actions` | Actions | `ActionsDemo` | `/Pages/Demos/Cards/Card/ActionsDemo.razor` |
| `clickable` | Clickable card | `ClickableDemo` | `/Pages/Demos/Cards/Card/ClickableDemo.razor` |
| `loading` | Loading | `LoadingDemo` | `/Pages/Demos/Cards/Card/LoadingDemo.razor` |
| `orientation` | Orientation | `HorizontalDemo` | `/Pages/Demos/Cards/Card/HorizontalDemo.razor` |

---

## 2. What each demo shows

| Demo | Parameters and blocks | Expected |
|------|------------------------|----------|
| `DefaultDemo` | Header, title, subtitle, content. Width `280px`. | Static card. No role, no tabindex. |
| `VariantsDemo` | `Filled`, `Outlined`, `Elevated`, `Flat`. | One card per variant. |
| `MediaDemo` | `SfCardMedia` with `ImageUrl` and `Alt`. | Informative image keeps its alt text. |
| `ActionsDemo` | `SfCardActions` with Buy and Details buttons. | Nested buttons keep their own tab stop and do not activate the card. |
| `ClickableDemo` | `IsClickable`, `AriaLabel`, `Hoverable`, `OnClick`. | Root is one action and counts activations. |
| `LoadingDemo` | `IsLoading` bound to a toggle. | Busy card blocks activation until loading ends. |
| `HorizontalDemo` | `Orientation="CardOrientation.Horizontal"`. | Media sits beside the content. |

---

## 3. Registration

| Surface | Change |
|---------|--------|
| `Layout/NavMenu.razor` | Cards group, link `cards/card`. `IsComponentsRoute` includes `cards` and `cards/`. |
| `Layout/SearchIndex.cs` | Card, Card Variants, Card Media, Card Actions, Clickable Card, Card Loading, Card Orientation. |
| `Pages/Overview/Explore.razor` | Cards category with one Card item. Icon class `ci-card` maps to an existing catalog SVG. |

---

## 4. Accessibility routes

These routes are the L4 axe contract in `TEST-MATRIX.md` and `accessibility/config.json`. They are separate from the catalog page.

| ID | Route | State |
|----|-------|-------|
| P-AXE-CARD-001 | `/card/static` | Static |
| P-AXE-CARD-002 | `/card/clickable` | Clickable, named |
| P-AXE-CARD-003 | `/card/loading` | Loading |
| P-AXE-CARD-004 | `/card/media` | Media with alt |
| P-AXE-CARD-005 | `/card/horizontal` | Horizontal |

---

## 5. Rules

- Use only public `SfCard` parameters from `API.md`.
- A clickable card always has `AriaLabel`.
- Do not put interactive controls in the card body. Put them in `SfCardActions`.
- Decorative media uses an empty `Alt`. Informative media uses a real `Alt`.
- Do not add a data-bound card list. Card is a composition surface.
