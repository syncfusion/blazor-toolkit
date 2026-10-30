## SfCard — Sample preparer

| Field              | Value                                                                 |
|--------------------|-----------------------------------------------------------------------|
| **Component**      | `SfCard` family                                                       |
| **Host**           | `samples/Blazor.Toolkit.Samples.Client`                               |
| **Catalog route**  | `/cards/card`                                                         |
| **Demo namespace** | `Blazor.Toolkit.Samples.Client.Pages.Demos.Cards.Card`                |

This document is the official implementation specification for a complete set of `SfCard` demo samples.

It defines the new demo structure, required files, real-world scenarios, visual design, interactions, public APIs to demonstrate, and implementation constraints. Each demo must be implemented as a production-quality sample that clearly teaches one focused Card capability.

**Scope Rule (Strict)**

Only the following files may be created, renamed, or modified:

- `samples/Blazor.Toolkit.Samples.Client/Pages/Components/Cards/Card.razor` — catalog page and demo registration
- `samples/Blazor.Toolkit.Samples.Client/Pages/Demos/Cards/Card/*.razor` — the demo files listed in section 1

Do **not** modify:

- Card component source code
- Chart component source code
- Any bUnit, Playwright, or accessibility test files
- Any other catalog, shared style, configuration, or sample file

**Goal**

Implement a polished, modern, and professional Card sample experience that is competitive with leading UI toolkits. All samples must use public `SfCard` APIs, follow the project’s existing Razor conventions, remain responsive and accessible, and present realistic product, workspace, media, action, loading, and analytics scenarios.

The implementation must deliver these eight demo areas:

1. Default Card composition
2. Region positions and media ordering
3. Media-based contributor cards
4. Card actions with loading feedback
5. Expandable and controlled content
6. Card loading state
7. Horizontal orientation with dynamic regions
8. Analytics Card hosting a Chart

If a required file does not exist, create it. If an existing legacy demo uses a different name but still represents the same sample, replace its catalog registration and final file name as specified in section 4 rather than leaving duplicate or obsolete demo entries.

---

## 1. Catalog Page Structure

| Item          | Path / Value                                                                 |
|---------------|------------------------------------------------------------------------------|
| Page          | `samples/Blazor.Toolkit.Samples.Client/Pages/Components/Cards/Card.razor`    |
| Demos folder  | `samples/Blazor.Toolkit.Samples.Client/Pages/Demos/Cards/Card/`              |
| Route         | `@page "/cards/card"`                                                        |
| Base class    | `TocPublishingComponentBase`                                                 |

### TOC and CodePreview mapping (must be used exactly)

| TOC id        | Heading                        | Demo Component       | SourceFile                                                |
|---------------|--------------------------------|----------------------|-----------------------------------------------------------|
| —             | Blazor Toolkit Card Component  | `DefaultDemo`        | `/Pages/Demos/Cards/Card/DefaultDemo.razor`               |
| `positions`   | Positions                      | `PositionsDemo`      | `/Pages/Demos/Cards/Card/PositionsDemo.razor`             |
| `media`       | Media                          | `MediaDemo`          | `/Pages/Demos/Cards/Card/MediaDemo.razor`                 |
| `actions`     | Actions                        | `ActionsDemo`        | `/Pages/Demos/Cards/Card/ActionsDemo.razor`               |
| `expandable`  | Expandable                     | `ExpandableDemo`     | `/Pages/Demos/Cards/Card/ExpandableDemo.razor`            |
| `loading`     | Loading                        | `LoadingDemo`        | `/Pages/Demos/Cards/Card/LoadingDemo.razor`               |
| `orientation` | Orientation                    | `HorizontalDemo`     | `/Pages/Demos/Cards/Card/HorizontalDemo.razor`            |
| `analytics`   | Analytics Card                 | `AnalyticsCardDemo`  | `/Pages/Demos/Cards/Card/AnalyticsCardDemo.razor`         |

The catalog implementation must provide all of the following:

- Register each demo exactly once through a matching `Description` and `CodePreview` pair.
- Use the exact TOC heading and TOC id shown above.
- Keep the default Card component introduction as the first catalog section, without a TOC id.
- Use the exact `Demo Component` and `SourceFile` values shown above.
- Update the `TocItems` collection inside `Card.razor` so it contains exactly the TOC entries for this sample set, in catalog order.
- Remove obsolete catalog registrations and preview references that correspond to replaced or renamed demos.

---

## 2. Detailed Demo Specifications

### 2.1 DefaultDemo.razor

**Purpose**  
Show the most common and polished usage of `SfCard` as a composition surface.

**Real-world scenario**  
Premium product or destination spotlight card.

**Visual design**
- Single vertical card, width ≈ 340 px, centered.
- Top: Full-bleed professional image (high-resolution, perfect `ObjectFit="Cover"`, height 200–220 px).
- `SfCardHeader` → `SfCardTitle` + `SfCardSubTitle`.
- `SfCardContent` → 2–3 lines of concise, high-quality text.
- `SfCardActions` → Primary button + Secondary button.

**Interaction**  
Static. Buttons are interactive but do not change the card state.

**Main APIs highlighted**  
Basic composition (`SfCardMedia`, `SfCardHeader`, `SfCardTitle`, `SfCardSubTitle`, `SfCardContent`, `SfCardActions`).

**Catalog Description (use exactly)**

```razor
<Description>
    <Header>
        <h1 id="blazor-toolkit-card-component">Blazor Toolkit Card Component</h1>
    </Header>
    <Content>
        The Card is a composition surface. Place header, title, subtitle, media, content, actions, footer, and separator blocks inside <code>SfCard</code>. It is not a data-bound list.
    </Content>
</Description>
```

### 2.2 PositionsDemo.razor

**Purpose**  
Teach that the order of regions inside `SfCard` controls the position of media.

**Real-world scenario**  
Modern meeting-room / workspace booking cards.

**Visual design**
- Two cards side-by-side (equal width ≈ 300–320 px).
- Both cards have identical content.
- Left card order: `SfCardMedia` → Header → Content → Actions.
- Right card order: Header → `SfCardMedia` → Content → Actions.
- Professional interior photograph that fills the media area perfectly.

**Interaction**  
Static comparison.

**Main APIs highlighted**  
Region ordering + `SfCardMedia`.

**Catalog Description (use exactly)**

```razor
<Description>
    <Header>
        <h2 id="positions">Positions</h2>
    </Header>
    <Content>
        The card renders its regions in the order you add them. Place <code>SfCardMedia</code> before or after the header to control media position.
    </Content>
</Description>
```

### 2.3 MediaDemo.razor

**Purpose**  
Showcase `SfCardMedia` with professional images and clean content.

**Real-world scenario**  
Professional team / contributor cards (enterprise style, not social-media clone).

**Visual design**
- Exactly two cards side-by-side.
- Each card:
  - High-quality professional photograph in `SfCardMedia` (perfect Cover fit).
  - `SfCardTitle` (name) + `SfCardSubTitle` (role).
  - Short professional bio in `SfCardContent`.
  - Clean stats row (simple flex layout).
  - Single primary button (“Connect” or “View profile”) inside `SfCardActions`.

**Interaction**  
Static (button may be live).

**Main APIs highlighted**  
`SfCardMedia` with meaningful Alt text.

**Catalog Description (use exactly)**

```razor
<Description>
    <Header>
        <h2 id="media">Media</h2>
    </Header>
    <Content>
        <code>SfCardMedia</code> renders an image or custom content. Set <code>Alt</code> when the image conveys information. Leave it empty when the image is decorative.
    </Content>
</Description>
```

---

### 2.4 ActionsDemo.razor

**Purpose**  
Demonstrate `SfCardActions` and realistic use of `IsLoading` during an action.

**Important:** This demo must NOT expand/collapse content (that belongs to `ExpandableDemo`).

**Real-world scenario**  
Product or service card with a primary call-to-action.

**Visual design**
- Single vertical card (≈ 340 px).
- Professional product/service image.
- Title + Subtitle + short description.
- Two buttons inside `SfCardActions` (Primary + Secondary).

**Interaction flow**
1. User clicks the primary button (e.g. “Add to cart” / “Subscribe”).
2. Card sets `IsLoading="true"` for 1.5 seconds.
3. Surface dims, buttons become disabled.
4. Loading ends → a small success message or badge appears inside the existing `SfCardContent` (card height does not change).

**Main APIs highlighted**  
`SfCardActions` + `IsLoading`.

**Catalog Description (use exactly)**

```razor
<Description>
    <Header>
        <h2 id="actions">Actions</h2>
    </Header>
    <Content>
        Put buttons in <code>SfCardActions</code>. Nested controls keep their own tab stop and do not activate a clickable card. Use <code>IsLoading</code> to show a busy state during the action.
    </Content>
</Description>
```

---

### 2.5 ExpandableDemo.razor

**Purpose**  
Demonstrate progressive disclosure (expand / collapse) — the pattern requested by the lead.

**Real-world scenario**  
City / destination guide card.

**Visual design**
- Collapsed state: Title + Subtitle + chevron button.
- Expanded state: Professional image + richer description + secondary action buttons.

**Interaction**
- Clicking the chevron (or the card when using `IsClickable`) toggles the expanded state.
- Smooth height transition.
- Proper `AriaLabel` and accessibility state.

**Main APIs highlighted**  
Controlled content + optional `IsClickable` / `OnClick` / `AriaLabel`.

**Catalog Description (use exactly)**

```razor
<Description>
    <Header>
        <h2 id="expandable">Expandable</h2>
    </Header>
    <Content>
        Use controlled content (or <code>IsClickable</code> with an <code>AriaLabel</code>) to create an expandable card that reveals additional media and details on demand.
    </Content>
</Description>
```

### 2.6 LoadingDemo.razor

**Purpose**  
Show a clean, modern loading state.

**Real-world scenario**  
Content card that supports explicit refresh / loading.

**Visual design**
- Professional vertical card that includes a media section.
- Same high-quality visual language as Default and Actions demos.

**Interaction**
- A “Refresh” or “Simulate loading” button sets `IsLoading="true"` for 1.5–2 seconds.
- While loading: surface is dimmed, content is faded, actions are unreachable.
- After the delay the card returns to the normal state.

**Main APIs highlighted**  
`IsLoading`.

**Catalog Description (use exactly)**

```razor
<Description>
    <Header>
        <h2 id="loading">Loading</h2>
    </Header>
    <Content>
        <code>IsLoading</code> marks the card busy, dims the surface, blocks activation, and makes actions unreachable until loading ends.
    </Content>
</Description>
```

---

### 2.7 HorizontalDemo.razor (Orientation)

**Purpose**  
Demonstrate horizontal layout and dynamic region visibility.

**Real-world scenario**  
Workspace or product horizontal card.

**Visual design**
- Wide horizontal card (≈ 520–560 px).
- Professional image on one side.
- Checkboxes above the card: Header, Media, Content, Footer.

**Interaction**
- Toggling any checkbox shows or hides the corresponding region.
- Layout remains perfectly aligned at all times.

**Main APIs highlighted**  
`Orientation="CardOrientation.Horizontal"` + dynamic composition.

**Catalog Description (use exactly)**

```razor
<Description>
    <Header>
        <h2 id="orientation">Orientation</h2>
    </Header>
    <Content>
        <code>Orientation</code> is Vertical by default. Horizontal places the media beside the content and flips in right-to-left layouts. Use the checkboxes to show or hide regions without changing orientation.
    </Content>
</Description>
```

---

### 2.8 AnalyticsCardDemo.razor

**Purpose**  
Show a professional analytics / KPI card that hosts a simple Chart.

**Real-world scenario**  
Executive dashboard widget.

**Visual design**
- Horizontal card (≈ 580 px wide).
- Left side (≈ 200 px): Title, subtitle, large KPI value, trend percentage, 1–2 secondary metrics.
- Right side: Simple, clean Area or Line chart (lightweight data, soft gradient fill, minimal axes, perfect alignment, no overflow).

**Interaction**  
Mostly static (light tooltips on the chart are allowed).

**Main APIs highlighted**  
Card as a composition surface that hosts another Toolkit component (Chart).

**Catalog Description (use exactly)**

```razor
<Description>
    <Header>
        <h2 id="analytics">Analytics Card</h2>
    </Header>
    <Content>
        A professional analytics card that hosts a simple Chart. Ideal for KPI widgets, sales overviews, and dashboard summaries.
    </Content>
</Description>
```

---

## 3. Global Implementation Rules

### 3.1 Suggested Image References

The following external image references may be used while preparing the demos:

| Demo | Recommended source | Example URL / usage | Purpose |
|------|--------------------|---------------------|---------|
| Default | Picsum photos or Unsplash | `https://picsum.photos/id/1015/600/400` | High-quality landscape/product visual |
| Positions | Picsum photos (interior/workspace) | `https://picsum.photos/id/117/600/400` | Clean meeting-room or workspace image |
| Media | Pravatar or Picsum photos (portraits) | `https://i.pravatar.cc/400?img=11` | Professional headshot |
| Actions | Picsum photos (product) | `https://picsum.photos/id/21/600/400` | Product-focused image |
| Expandable | Picsum photos (city/travel) | `https://picsum.photos/id/1016/600/400` | Destination image |
| Loading | Same image as the Default or Actions demo | Use the selected demo reference above | Consistent loading-state visual |
| Orientation | Picsum photos (workspace/lifestyle) | `https://picsum.photos/id/42/600/400` | Horizontal lifestyle or workspace image |
| Analytics Card | No image required | None, or a small decorative visual if needed | Keep focus on the KPI and Chart |

> These URLs are reference sources for development and visual preparation only. Before final release, confirm image rights and prefer licensed or repository-hosted images under the application assets folder so demo rendering does not depend on third-party availability.

1. **Images**
   - Must be professional, high-resolution, and correctly cropped.
   - Always use `ObjectFit="CardMediaFit.Cover"` (or `Contain` only when absolutely necessary).
   - Zero stretching, letterboxing, or alignment issues.

2. **Components**
   - Use only public Toolkit components (`SfCard*` family, `SfButton`, `SfChart`, `SfCheckBox`, etc.).
   - For avatar-like UI, use a simple styled `<img>` or `<div>` (no external avatar component required).

3. **Accessibility**
   - Every clickable / expandable card must have a meaningful `AriaLabel`.
   - Informative images must have proper `Alt` text.
   - Loading states must be clear and keyboard-friendly.

4. **Actions & Loading**
   - Interactive buttons belong inside `SfCardActions`.
   - All loading simulations must use a realistic delay of 1.5–2 seconds.

5. **Styling**
   - Shared styles → `<style>` block in `Card.razor`.
   - Demo-specific styles → inside the individual demo `.razor` file.
   - Maintain consistent spacing, typography, and visual hierarchy.

6. **Quality bar**
   - Demos must look more polished, more useful, and more professional than competitor Card demos.
   - No visual noise, no uneven gaps, no competing elements.

---

## 4. Demo File Implementation Plan

Create the final demo file structure below under the Card demo namespace. The names listed here are the implementation targets; legacy names must not remain as catalog entries after the implementation is complete.

| Final demo file | Implementation responsibility | Existing name, if any |
|-----------------|--------------------------------|------------------------|
| `DefaultDemo.razor` | Implement the complete composition example with professional media, header, content, and actions. | `DefaultDemo.razor` |
| `PositionsDemo.razor` | Implement the two-card media-position comparison from scratch. | `VariantsDemo.razor` |
| `MediaDemo.razor` | Implement exactly two professional contributor cards with media, biography, stats, and action regions. | `MediaDemo.razor` |
| `ActionsDemo.razor` | Implement the action/loading workflow and keep it separate from expandable behavior. | `ActionsDemo.razor` |
| `ExpandableDemo.razor` | Implement controlled or clickable progressive disclosure with accessible toggle behavior. | `ClickableDemo.razor` |
| `LoadingDemo.razor` | Implement a visible refresh/loading interaction using a 1.5–2 second delay. | `LoadingDemo.razor` |
| `HorizontalDemo.razor` | Implement the horizontal card and dynamic Header, Media, Content, and Footer visibility controls. | `HorizontalDemo.razor` |
| `AnalyticsCardDemo.razor` | Implement the KPI composition and aligned Chart without introducing images unless decorative. | `MetricCardDemo.razor`, if present |

### 4.1 File and component conventions

- Place the catalog page only in `Card.razor` and individual implementations only in the demo folder.
- Use public component APIs and follow the existing Razor and namespace conventions used by neighboring demo samples.
- Keep private state and event handlers inside the individual demo that owns the behavior.
- Use demo-local CSS for layout specific to one sample.
- Use the catalog page style block only for shared Card demo layout rules.
- Do not add tests, component source edits, package changes, or unrelated configuration changes.
- Remove obsolete preview registrations and references when an existing demo name is replaced.
- Preserve the existing host project and render mode; do not introduce a separate demo application.

### 4.2 New implementation completion checklist

Before implementation is considered complete:

1. Confirm all eight final demo files exist at the expected paths.
2. Confirm `Card.razor` registers one preview for each demo in the required order.
3. Confirm the TOC collection and every `SourceFile` exactly match section 1.
4. Confirm each description text matches section 2 exactly.
5. Confirm no source-code, test, shared stylesheet, or configuration files outside the scope were changed.
6. Confirm each demo works in the existing sample host at the `/cards/card` route.

---

## 5. Final Acceptance Criteria

- All eight new demo implementations render correctly in the existing sample host and look professional.
- The final demo set uses the exact names, TOC ids, headings, preview mappings, and descriptions specified in this document.
- `PositionsDemo`, `MediaDemo`, and `ExpandableDemo` have no image, subtitle, or chevron alignment defects.
- Images are properly cropped with `CardMediaFit.Cover`, have meaningful alternative text where required, and do not stretch or overflow.
- Actions and Expandable samples provide visibly different interactions.
- Loading states are blocked and visibly busy for 1.5–2 seconds before returning to normal.
- Analytics Card combines a clear KPI summary with a correctly aligned, responsive Chart.
- The complete Card demo page presents a consistent hierarchy with no visual noise, uneven gaps, overlap, or competing elements.