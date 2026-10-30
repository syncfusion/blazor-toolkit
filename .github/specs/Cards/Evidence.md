# SfCard — Spec evidence

| Field | Value |
|-------|--------|
| Component | `SfCard` family |
| Specs compared | `Spec.md` 1.0, `API.md` 1.0.0-proposed, `PRD.md` 1.0, `TEST-MATRIX.md` (expanded B/P/A/V matrix), `API-xml.md` |
| Implementation branch | `bc/379d77ed-develop-card` |
| Source | `src/Components/Cards/Card/` |
| Method | Source comparison plus runtime runs in this environment: bUnit on net8.0, Playwright Chromium, axe, visual baselines, and a Release library build with SBOM generation disabled. |
| Date | 2026-10-28 |

## Verdict

The implementation matches the Card spec for public API, regions, variants, orientation, elevation, loading, media, click and keyboard behavior, RTL, CSS isolation, and the required accessibility sample routes.

Runtime results are in section 4.

## 1. Spec behavior

| ID | Requirement | Evidence | Result |
|----|-------------|----------|--------|
| S-01 | Optional regions; markup order is visual order | `SfCard.razor` renders `ChildContent` inside the root. No region is required. `U-CARD-002` asserts header, media, content, separator, actions, footer order. | Passed (source) |
| S-02 | Variants Filled, Outlined, Elevated, Flat | `CardVariant` values 0–3. Classes: none extra for Filled, `e-card-outlined`, `e-card-elevated`, `e-card-flat`. | Passed (source) |
| S-03 | Square removes border radius | `e-card-square` plus `border-radius: 0` in `SfCard.razor.css`. | Passed (source) |
| S-04 | Elevation 0–5 maps to shadow tokens; 0 is none | `ResolveElevation` clamps to 0–5. CSS maps `data-elevation` to `--e-card-shadow-0` through `--e-card-shadow-5`. Outlined and Flat force 0. Elevated adds 2, capped at 5. | Passed (source) |
| S-05 | Width and Height are CSS lengths on the root | Bare numbers become pixels. `U-CARD-010` expects `width:320px;height:200px;`. | Passed (source) |
| S-06 | ContentPadding defaults true and regions honor it | Default `data-content-padding="true"`. CSS pads header, content, and actions only when that attribute is true. | Passed (source) |
| S-07 | Vertical stack; horizontal flex row; RTL reverses inline direction | Default column flex. `.e-card-horizontal` uses `flex-direction: row`. `.e-rtl.e-card-horizontal` uses `row-reverse`. | Passed (source) |
| S-08 | Hoverable lifts on hover and focus-within; pointer when clickable | `.e-card-hoverable:hover` and `:focus-within` change border, shadow, and transform. Clickable also sets `cursor: pointer`. | Passed (source) |
| S-09 | Loading sets `aria-busy`, shows a loading presentation, and makes actions inert | `e-card-loading`, overlay `::after`, `aria-busy="true"`. `SfCardActions` sets `inert` while `IsBusy`. | Passed (source) |
| S-10 | Clickable root activates on click, Enter, and Space; nested actions do not | `role="button"`, `tabindex="0"`, click and key handlers. Enter fires on key down. Space arms on key down and fires on key up. `SfCardActions` uses `stopPropagation`. | Passed (source) |
| S-11 | Static card has no fake button role | Role and tabindex are set only when `IsClickable` is true. | Passed (source) |
| S-12 | Clickable card is keyboard operable and named | `tabindex="0"`, Enter/Space handlers, `AriaLabel` renders `aria-label`. | Passed (source) |
| S-13 | Media uses customer alt; empty alt only when decorative | `alt="@(Alt ?? string.Empty)"`. Missing alt renders an empty `alt`. | Passed (source) |
| S-14 | CSS isolation, forced-colors, reduced motion | Region rules use `::deep`. `@media (forced-colors: active)` sets borders and focus. `@media (prefers-reduced-motion: reduce)` removes motion. | Passed (source) |
| S-15 | Static SSR renders visually; OnClick needs interactivity | Markup is declarative. Event handlers are attached only when clickable. XML remarks state that `OnClick` needs an interactive render mode. | Passed (source) |
| S-16 | Theme tokens via `SfThemeRoot` | `SfCard.razor` emits `SfThemeRoot` before the root. Colors use `--color-sf-*` tokens. | Passed (source) |
| S-17 | Title uses a real heading when conLevel` is 1–6. The default is a non-heading `div`, so a heading is available but not forced. | Passed with note |

## 2. Public API

| Item | Spec | Implementation | Result |
|------|------|----------------|--------|
| Components | `SfCard`, Header, Title, SubTitle, Media, Content, Actions, Footer, Separator | All present under `src/Components/Cards/Card/`, namespace `Syncfusion.Blazor.Toolkit` | Passed (source) |
| Enums | `CardVariant`, `CardOrientation`, `CardMediaFit` with the specified numeric values | `CardEnumerations.cs` matches | Passed (source) |
| `SfCard` parameters | ChildContent, Variant, Orientation, Elevation, Square, Hoverable, IsLoading, ContentPadding, Width, Height, CssClass, Id, HtmlAttributes, IsClickable, OnClick, AriaLabel | `SfCard.razor.Members.cs` and `SfCard.razor.Events.cs`. Defaults match Spec section 10. | Passed (source) |
| `SfCardMedia` | ImageUrl, Alt, Height `"180px"`, ObjectFit `Cover`, ChildContent, CssClass, HtmlAttributes | `SfCardMedia.razor.cs` | Passed (source) |
| Region parameters | ChildContent, CssClass, HtmlAttributes | Present on each region component | Passed (source) |
| Public classes | `e-card`, `e-card-horizontal`, `e-card-outlined`, `e-card-elevated`, `e-card-flat`, `e-card-square`, `e-card-hoverable`, `e-card-loading`, and the region classes | Constants and region markup match the spec list | Passed (source) |

`HtmlAttributes` is also declared as `[Parameter(CaptureUnmatchedValues = true)]`, so unmatched attributes bind without a dictionary.

## 3. TEST-MATRIX cases

Matrix source: `.github/specs/Cards/TEST-MATRIX.md` (bUnit `B-CARD-*`, Playwright `P-CARD-*`, axe `A-CARD-*`, visual `V-CARD-*`). `SfCardTest.cs` keeps its earlier `U-CARD` and `I-CARD` tests for the v1 matrix.

### 3.1 bUnit (`B-CARD-*`)

`tests/Syncfusion.Blazor.Toolkit.BUnitTest/Cards/SfCardMatrixTest.cs` carries the matrix id in each test name.

### 3.2 Playwright (`P-CARD-*`)

`scenarios.spec.ts` (page `/card/scenarios`) covers the core, keyboard, responsive, theme, RTL, high-contrast, and end-to-end ids. `interactions.spec.ts` keeps the original interaction checks. `matrix-visual.spec.ts` covers the visual ids.

### 3.3 axe (`A-CARD-*`)

Routes are registered under the `control: "Card"` entry in `accessibility/config.json`. `CardTests.cs` has one `[Test]` per route plus the colour-contrast and media-decorative extras.

### 3.4 Visual (`V-CARD-*`)

Baselines live in `visual.spec.ts-snapshots/` (original eight) and `matrix-visual.spec.ts-snapshots/` (24 added in this branch).

## 4. Runtime record

Run on 2026-10-28 in this environment. The sample hosts ran in Development and the SBOM target was off (`-p:CycloneDxEnabled=false`).

| Suite | What was checked | Result |
|-------|------------------|--------|
| bUnit | `FullyQualifiedName~Syncfusion.Blazor.Toolkit.Tests.Cards`, net8.0 (run with `DOTNET_ROLL_FORWARD=Major` because only the .NET 10 runtime is installed). | Passed. 174 passed, 0 failed, 0 skipped. |
| Playwright (Chromium) | All specs under `Cards/Card`: `interactions`, `visual`, `scenarios`, `matrix-visual`. `scenarios.spec.ts` was also run three times in a row to check for flakiness. | Passed. 113 passed, 0 skipped, 0 failed. |
| axe | `FullyQualifiedName~CardTests`, 17 tests. | Passed. 17 passed, 0 failed. |
| Visual regression | 34 baselines compared on Chromium, repeated three times with no diff. | Passed |
| Build | `dotnet build src/Syncfusion.Blazor.Toolkit.csproj -c Release -p:CycloneDxEnabled=false`. | Build succeeded. 0 warnings and 0 CS1591 under `Components/Cards/`. |

`color-contrast` and `color-contrast-enhanced` are disabled in `CardAxeOptions()` for the page-level runs, so a green run does not by itself prove WCAG 1.4.3. `Card_color_contrast` runs those rules explicitly.

## 5. Notes that do not fail the spec

- A clickable card prevents the browser default for Enter and Space only. Tab is not included in that predicate.
- Loading a clickable card also sets `aria-disabled="true"` and `e-disabled`.
- `CardTests` disables the `color-contrast` and `color-contrast-enhanced` axe rules, so a green axe run does not by itself prove WCAG 1.4.3.
- Spec section 9 lists `docs/card/`. The signed specs used for this comparison live in `.github/specs/Cards/`.

- Spec section 9 lists `docs/card/`. The signed specs used for this comparison live in `.github/specs/Cards/`.
