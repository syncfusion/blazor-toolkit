## SfCard — Sample preparer

| Field | Value |
|-------|--------|
| **Component** | `SfCard` family |
| **Host** | `samples/Blazor.Toolkit.Samples.Client` |
| **Catalog route** | `/cards/card` |
| **Demo namespace** | `Blazor.Toolkit.Samples.Client.Pages.Demos.Cards.Card` |

This file is the sample contract for Card.

**Scope Rule (Strict)**  
Only the following files may be modified:
- `samples/Blazor.Toolkit.Samples.Client/Pages/Components/Cards/Card.razor` (catalog page)
- `samples/Blazor.Toolkit.Samples.Client/Pages/Demos/Cards/Card/*.razor` (the 7 existing demos + new MetricCardDemo.razor)

Do **not** modify:
- Card component source
- Chart component source
- Any bUnit, Playwright, or accessibility test files
- Any other non-demo files

**Goal**  
Update the existing 7 demos with professional real-world use cases and add one new demo (`MetricCardDemo`) that shows a Chart inside a Card. All demos must have correct alignment, good visual appearance, and proper interaction behavior.

---

## 1. Catalog page

| Item | Path |
|------|------|
| Page | `samples/Blazor.Toolkit.Samples.Client/Pages/Components/Cards/Card.razor` |
| Demos folder | `samples/Blazor.Toolkit.Samples.Client/Pages/Demos/Cards/Card/` |
| Route | `@page "/cards/card"` |

| TOC id        | Heading                  | Demo                  | SourceFile                                            |
|---------------|--------------------------|-----------------------|-------------------------------------------------------|
| —             | Blazor Toolkit Card Component | `DefaultDemo`     | `/Pages/Demos/Cards/Card/DefaultDemo.razor`           |
| `variants`    | Variants                 | `VariantsDemo`        | `/Pages/Demos/Cards/Card/VariantsDemo.razor`          |
| `media`       | Media                    | `MediaDemo`           | `/Pages/Demos/Cards/Card/MediaDemo.razor`             |
| `actions`     | Actions                  | `ActionsDemo`         | `/Pages/Demos/Cards/Card/ActionsDemo.razor`           |
| `clickable`   | Clickable card           | `ClickableDemo`       | `/Pages/Demos/Cards/Card/ClickableDemo.razor`         |
| `loading`     | Loading                  | `LoadingDemo`         | `/Pages/Demos/Cards/Card/LoadingDemo.razor`           |
| `orientation` | Orientation              | `HorizontalDemo`      | `/Pages/Demos/Cards/Card/HorizontalDemo.razor`        |
| `metric`      | Metric Card              | `MetricCardDemo`      | `/Pages/Demos/Cards/Card/MetricCardDemo.razor`        |

---

## 2. Demo Specifications (Final)

| Demo File             | Real-world Use Case                          | What must be shown                                                                 | Main API(s)                          | Interaction |
|-----------------------|----------------------------------------------|------------------------------------------------------------------------------------|--------------------------------------|-----------|
| `DefaultDemo.razor`   | Project / Task Cards                         | Responsive grid of 3–4 vertical cards (title, subtitle, description, button)      | Basic composition                    | Static |
| `VariantsDemo.razor`  | Same Project Card in 4 styles                | Four identical cards using Filled, Outlined, Elevated, Flat                        | `Variant`                            | Static |
| `MediaDemo.razor`     | Team Member Profile Cards                    | Profile cards with photo, name, role, bio and button (use `https://i.pravatar.cc`) | `SfCardMedia`                        | Static |
| `ActionsDemo.razor`   | Product Card with Reveal                     | Product card. One button switches between Summary view and Detailed view (Reveal) | `SfCardActions` + Reveal logic       | Reveal |
| `ClickableDemo.razor` | Selectable Plan / Category Cards             | Cards that can be selected by clicking anywhere on the card                        | `IsClickable`, `OnClick`, `AriaLabel`, `Hoverable` | Whole card clickable |
| `LoadingDemo.razor`   | Loading State Card                           | Card + “Simulate Loading” button. Shows real dimming behavior of `IsLoading`      | `IsLoading`                          | Loading toggle |
| `HorizontalDemo.razor`| Horizontal Card + Dynamic Content            | Horizontal card + checkboxes to show/hide Media, Content and Footer                | `Orientation` + Dynamic composition  | Dynamic show/hide |
| `MetricCardDemo.razor`| KPI / Metric Card                            | Metric card with title, large number, percentage change and a simple Chart         | Composition + Chart                  | Chart interaction |

---

## 3. Rules

- Use only Toolkit components (`SfCard*` family, `SfButton`, `SfChart`, `SfCheckBox`).
- Clickable cards must have a proper `AriaLabel`.
- Buttons must be placed inside `SfCardActions` or `SfCardFooter`.
- All demos must have correct alignment, spacing, and responsive behavior.
- All interactions must work smoothly without visual glitches.
- You are allowed to add additional CSS styles when required.
  - Shared styles → inside the <style> block of Card.razor (catalog page)
  - Demo-specific styles → inside the individual demo .razor file
- Images:  
  - Profile photos → `https://i.pravatar.cc/300`  
  - Product images → `https://picsum.photos/`

---

## 4. New Demo – MetricCardDemo

- **File**: `MetricCardDemo.razor`
- **TOC id**: `metric`
- **Heading**: Metric Card
- **Purpose**: Show a professional KPI/metric card that contains a simple Chart.

**Description text to use:**
```razor
<Description>
    <Header>
        <h2 id="metric">Metric Card</h2>
    </Header>
    <Content>
        This sample demonstrates a professional metric card that hosts a Chart component.
        Ideal for KPI widgets, sales overviews, and analytics summaries.
    </Content>
</Description>