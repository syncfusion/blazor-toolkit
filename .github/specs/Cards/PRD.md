# Product Requirements Document (PRD)
## SfCard — Syncfusion Toolkit for Blazor

| Field | Value |
|-------|--------|
| **Component** | `SfCard` (+ region building blocks) |
| **Namespace** | `Syncfusion.Blazor.Toolkit` |
| **Repo** | https://github.com/syncfusion/blazor-toolkit |
| **License** | MIT |
| **TFMs** | net8.0 / net9.0 / net10.0 |
| **Render modes** | Static SSR, Server, WASM, Auto |
| **Status** | Proposed |
| **Doc version** | 1.0 |

---

## 1. Problem statement

Apps need a flexible **content surface** for product tiles, dashboards, profiles, and media summaries. Competitors (MudBlazor, Telerik, Ant Design Blazor, Radzen, Blazorise, HAVIT) already ship mature Card patterns. The Toolkit has Dialog/Tooltip/layout primitives but **no Card**. Gap: industrial OSS Card with composition blocks, variants, a11y, contrast, CSS isolation, and zero-warning XML docs.

## 2. Goals

| ID | Goal |
|----|------|
| G1 | Ship `SfCard` with Header / Media / Content / Actions / Footer / Title / SubTitle / Separator blocks |
| G2 | Variants: Filled, Outlined, Elevated (shadow), Flat; optional Square corners |
| G3 | Vertical / Horizontal orientation |
| G4 | Hoverable, Loading, optional whole-card click |
| G5 | Width / Height, Elevation, ContentPadding |
| G6 | WCAG 2.2 AA (AAA targets where feasible) |
| G7 | Windows forced-colors + High Contrast themes |
| G8 | CSS isolation + `SfThemeRoot` tokens |
| G9 | RTL-safe layout |
| G10 | **0 build warnings** (including CS1591 via complete XML docs) |

## 3. Non-goals (v1)

- CardView data-grid (virtualized data cards as a separate product)
- Built-in tabbed card (compose with future Tabs)
- Mandatory image CDN / media pipeline

## 4. Users and scenarios

- Dashboard KPI / summary tiles
- E-commerce product cards
- Media + title + actions
- Horizontal profile / list row cards
- Loading skeleton while data fetches

## 5. Quality bar (mandatory)

- Windows contrast / forced-colors
- CSS isolation with scoped styles
- Accessibility WCAG 2.2 AA minimum; AAA checklist
- Responsive samples (mobile/desktop)
- RTL + localization for any built-in strings
- No build warnings

## 6. Success metrics

| Metric | Target |
|--------|--------|
| Feature parity vs Mud + Telerik + Ant core | >= 95% checklist |
| axe on samples | 0 serious/critical. P3 deliverable: `/card/static`, `/card/clickable`, `/card/loading`, `/card/media`, `/card/horizontal` |
| Build | 0 warnings |
| Coverage | >= 85% statements on Card types |

## 7. Risks

| Risk | Mitigation |
|------|------------|
| Over-composition API sprawl | Fixed small set of blocks; free ChildContent |
| Clickable card vs nested buttons | Document event stopPropagation; role strategy |
| Loading a11y | `aria-busy`, polite status |

## 8. Rollout

| Phase | Scope |
|-------|--------|
| P0 | PRD, Spec, API, matrix, API-xml sign-off |
| P1 | Root + regions + variants + orientation |
| P2 | Hoverable, Loading, clickable, media |
| P3 | A11y, contrast, RTL, samples, tests. Accessibility samples cover a static card, a clickable card, a loading card, a card with media, and a horizontal card. |
| P4 | CHANGELOG, PR merge |
