# SfCard — Test Matrix
## Syncfusion Toolkit for Blazor

| Field | Value |
|-------|--------|
| **Component** | `SfCard` family |
| **Matrix version** | 1.0 |
| **Goal** | Full automated coverage; CI is release gate |

---

## 1. Layer summary

| Layer | Tooling | Planned |
|-------|---------|---------|
| L1 Unit | xUnit + bUnit | ~40 |
| L2 Integration | bUnit | ~4 |
| L3 UI | Playwright | ~20 |
| L4 A11y | Playwright + axe | ~5 |
| L5 Visual | Playwright screenshots | ~8 |
| L6 Build | dotnet build | 3 |

---

## 2. data-testid hooks

| testid | Element |
|--------|---------|
| `card-root` | SfCard root |
| `card-header` | Header |
| `card-title` | Title |
| `card-subtitle` | SubTitle |
| `card-media` | Media |
| `card-content` | Content |
| `card-actions` | Actions |
| `card-footer` | Footer |
| `card-separator` | Separator |
| `card-action-btn` | Nested action button |

---

## 3. L1 Unit tests

| ID | Scenario | Expected | Priority |
|----|----------|----------|----------|
| U-CARD-001 | Default render | `e-card` present | P0 |
| U-CARD-002 | All regions | header/media/content/actions/footer | P0 |
| U-CARD-003 | Variant Outlined | `e-card-outlined` | P0 |
| U-CARD-004 | Variant Flat | `e-card-flat` | P0 |
| U-CARD-005 | Variant Elevated | `e-card-elevated` | P0 |
| U-CARD-006 | Orientation Horizontal | `e-card-horizontal` | P0 |
| U-CARD-007 | Square | `e-card-square` | P1 |
| U-CARD-008 | Hoverable class | `e-card-hoverable` | P0 |
| U-CARD-009 | IsLoading | `e-card-loading`, aria-busy | P0 |
| U-CARD-010 | Width/Height | styles applied | P1 |
| U-CARD-011 | Elevation 0 | minimal shadow | P1 |
| U-CARD-012 | ContentPadding false | padding off | P1 |
| U-CARD-013 | Media Alt | img alt set | P0 |
| U-CARD-014 | OnClick when clickable | callback fires | P0 |
| U-CARD-015 | Nested button isolation | root not required | P0 |
| U-CARD-016 | CssClass / HtmlAttributes | applied | P1 |
| U-CARD-017 | Title/SubTitle text | rendered | P0 |
| U-CARD-018 | Separator | rendered | P1 |
| U-CARD-019 | Filled default | default classes | P0 |
| U-CARD-020 | Custom media ChildContent | renders | P1 |

---

## 4. L2 Integration

| ID | Scenario | Priority |
|----|----------|----------|
| I-CARD-001 | Multiple cards in list | P1 |
| I-CARD-002 | Null templates safe | P0 |
| I-CARD-003 | Parameter churn re-render | P1 |
| I-CARD-004 | Loading toggle | P0 |

---

## 5. L3 Playwright UI

| ID | Scenario | Priority |
|----|----------|----------|
| P-CARD-001 | Product card structure | P0 |
| P-CARD-002 | Hoverable focus-visible | P0 |
| P-CARD-003 | Clickable Enter/Space | P0 |
| P-CARD-004 | Action button single fire | P0 |
| P-CARD-005 | Loading state | P0 |
| P-CARD-006 | Horizontal geometry | P1 |
| P-CARD-007 | RTL horizontal | P0 |
| P-CARD-008 | Mobile viewport | P1 |

Browsers: Chromium, Firefox, WebKit for P0.

---

## 6. L4 axe

| ID | State | Priority |
|----|-------|----------|
| P-AXE-CARD-001 | Static sample at `/card/static` | P0 |
| P-AXE-CARD-002 | Clickable named sample at `/card/clickable` | P0 |
| P-AXE-CARD-003 | Loading sample at `/card/loading` | P0 |
| P-AXE-CARD-004 | Media with alt sample at `/card/media` | P0 |
| P-AXE-CARD-005 | Horizontal sample at `/card/horizontal` | P1 |

L4 is not done until the files in this table exist. The axe sample host is `accessibility/Blazor.Toolkit.Accessibility.Samples`. `CardTests.cs` is `accessibility/Blazor.Toolkit.AxeCore.Tests/Tests/CardTests.cs`. The Card entry is the `control: "Card"` object in `accessibility/config.json`.

| ID | Page | Route | CardTests method | config.json |
|----|------|-------|------------------|-------------|
| P-AXE-CARD-001 | Static | `/card/static` | `Card_static` | Card `/card/static` |
| P-AXE-CARD-002 | Clickable | `/card/clickable` | `Card_clickable` | Card `/card/clickable` |
| P-AXE-CARD-003 | Loading | `/card/loading` | `Card_loading` | Card `/card/loading` |
| P-AXE-CARD-004 | Media | `/card/media` | `Card_media` | Card `/card/media` |
| P-AXE-CARD-005 | Horizontal | `/card/horizontal` | `Card_horizontal` | Card `/card/horizontal` |

---

## 7. L5 Visual baselines

`card-basic`, `card-media-actions`, `card-outlined`, `card-horizontal`, `card-loading`, `card-rtl`, `card-flat`, `card-mobile`

---

## 8. L6 Build

| ID | Expected |
|----|----------|
| B-CARD-001 | 0 warnings |
| B-CARD-002 | 0 CS1591 |
| B-CARD-003 | Nullable clean |

---

## 9. Definition of done

All P0/P1 automated tests green in CI; no mandatory manual test gate; XML docs complete per API-xml.md.
