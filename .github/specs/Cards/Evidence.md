# SfCard — Spec evidence

| Field | Value |
|-------|--------|
| Component | `SfCard` family |
| Specs compared | `Spec.md` 1.0, `API.md` 1.0.0-proposed, `PRD.md` 1.0, `TEST-MATRIX.md` 1.0, `API-xml.md` |
| Implementation branch | `bc/379d77ed-develop-card` |
| Source | `src/Components/Cards/Card/` |
| Method | Source comparison plus runtime runs in this environment: bUnit on net8.0, Playwright Chromium against the sample host, and a Release library build with SBOM generation disabled. |
| Date | 2026-09-12 |

## Verdict

The implementation matches the Card spec for public API, regions, variants, orientation, elevation, loading, media, click and keyboard behavior, RTL, CSS isolation, and the required accessibility sample routes.

Runtime results are recorded separately. They are not marked passed unless a run in this environment produced that result.

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

| ID | Expected | Implementation evidence | Result |
|----|----------|-------------------------|--------|
| U-CARD-001 | `e-card` present | `DefaultCard_RendersPlainRoot` | Implemented |
| U-CARD-002 | All regions | `AllRegions_RenderInOrder` | Implemented |
| U-CARD-003 | `e-card-outlined` | `Variant_AddsClass` | Implemented |
| U-CARD-004 | `e-card-flat` | `Variant_AddsClass` | Implemented |
| U-CARD-005 | `e-card-elevated` | `Variant_AddsClass` | Implemented |
| U-CARD-006 | `e-card-horizontal` | `Horizontal_AddsClass` | Implemented |
| U-CARD-007 | `e-card-square` | `Square_AddsClass` | Implemented |
| U-CARD-008 | `e-card-hoverable` | `Hoverable_AddsClass` | Implemented |
| U-CARD-009 | loading class and `aria-busy` | `Loading_SetsBusyState` | Implemented |
| U-CARD-010 | width and height styles | `WidthAndHeight_AreApplied` | Implemented |
| U-CARD-011 | elevation 0 has no shadow level | `Elevation_IsClamped` plus CSS level 0 | Implemented |
| U-CARD-012 | padding off | `ContentPaddingFalse_IsReflected` | Implemented |
| U-CARD-013 | image alt | `Media_RendersImageWithAlt` | Implemented |
| U-CARD-014 | click callback | `Clickable_ExposesSemanticsAndRaisesOnClick` | Implemented |
| U-CARD-015 | nested action does not activate the card | `ClickInsideActions_DoesNotActivateCard` | Implemented |
| U-CARD-016 | CssClass and HtmlAttributes | `CssClassAndHtmlAttributes_AreApplied` | Implemented |
| U-CARD-017 | title and subtitle text | covered by `AllRegions_RenderInOrder` | Implemented |
| U-CARD-018 | separator | covered by `AllRegions_RenderInOrder` | Implemented |
| U-CARD-019 | filled default classes | `FilledVariant_AddsNoVariantClass` | Implemented |
| U-CARD-020 | custom media content | `Media_CustomChildContent_Renders` | Implemented |
| I-CARD-001 | multiple cards | `MultipleCards_HaveUniqueIds` | Implemented |
| I-CARD-002 | null content is safe | `NullContent_IsSafe` | Implemented |
| I-CARD-003 | parameter churn | `ParameterChurn_LeavesNoStaleClasses` | Implemented |
| I-CARD-004 | loading toggle | `LoadingToggle_UpdatesBusyState` | Implemented |
| P-AXE-CARD-001 | `/card/static` | `Static.razor`, `Card_static` | Deliverable present |
| P-AXE-CARD-002 | `/card/clickable` | `Clickable.razor`, `Card_clickable` | Deliverable present |
| P-AXE-CARD-003 | `/card/loading` | `Loading.razor`, `Card_loading` | Deliverable present |
| P-AXE-CARD-004 | `/card/media` | `Media.razor`, `Card_media` | Deliverable present |
| P-AXE-CARD-005 | `/card/horizontal` | `Horizontal.razor`, `Card_horizontal` | Deliverable present |

`accessibility/config.json` has a `control: "Card"` entry for all five routes.

L3 scenarios P-CARD-001 through P-CARD-008 are covered by `interactions.spec.ts` and `visual.spec.ts`. L5 baselines (`card-basic`, `card-media-actions`, `card-outlined`, `card-horizontal`, `card-loading`, `card-rtl`, `card-flat`, `card-mobile`) are stored under `Cards/Card/visual.spec.ts-snapshots/` and matched in the Chromium run recorded below.

## 4. Runtime record

| Suite | What was checked | Result |
|-------|------------------|--------|
| bUnit `Cards.SfCardTest` | Re-run on net8.0. The old Enter/Tab assertion was replaced by `Enter activates once and Tab is not prevented`, which allows key-specific `preventDefault` for Enter and Space only. | Passed. 47 passed, 0 failed, 47 total. |
| Playwright L3 | Chromium against `http://localhost:5000` in Development. `interactions.spec.ts` and `visual.spec.ts` cover P-CARD-001 through P-CARD-008. | Passed. 34 passed (5.2s). |
| axe L4 | `CardTests.cs` and the `control: "Card"` entry cover `/card/static`, `/card/clickable`, `/card/loading`, `/card/media`, and `/card/horizontal`. | Not run in this session. Deliverables are present. |
| L5 visual | Eight baselines in `visual.spec.ts-snapshots/`: `card-basic`, `card-media-actions`, `card-outlined`, `card-horizontal`, `card-loading`, `card-rtl`, `card-flat`, `card-mobile`. | Passed on Chromium. Included in the 34 Playwright results above. |
| L6 warnings | `dotnet build src/Syncfusion.Blazor.Toolkit.csproj -c Release -p:CycloneDxEnabled=false`. SBOM generation is opt-out only; `Directory.Build.props` still defaults `CycloneDxEnabled` to true. | Build succeeded. 0 warnings and 0 CS1591 under `Components/Cards/`. Remaining warnings are outside Card. |

The earlier bUnit failure is closed. `onkeydown:preventDefault` is still emitted, but only for Enter and Space. Tab is not in that predicate, and the current test asserts that.

## 5. Notes that do not fail the spec

- A clickable card prevents the browser default for Enter and Space only. Tab is not included in that predicate.
- Loading a clickable card also sets `aria-disabled="true"` and `e-disabled`.
- `CardTests` disables the `color-contrast` and `color-contrast-enhanced` axe rules, so a green axe run does not by itself prove WCAG 1.4.3.
- Spec section 9 lists `docs/card/`. The signed specs used for this comparison live in `.github/specs/Cards/`.
