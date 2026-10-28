# Competitive Matrix — Blazor Card

**Product:** Syncfusion Blazor Toolkit `SfCard`  
**Doc version:** 1.0  
**Goal:** Industrial best-in-class OSS Card; match strongest patterns; lead on a11y, contrast, CSS isolation, XML completeness.

---

## 1. Competitor set

### Tier A — Primary

| Library | Component | Notes |
|---------|-----------|--------|
| MudBlazor | MudCard + Header/Content/Actions/Media | Elevation, Outlined, Square, ContentPadding |
| Telerik UI for Blazor | TelerikCard + building blocks | Orientation, Image, Separator, Title/SubTitle, deck/list/group CSS |
| Ant Design Blazor | Card | Bordered, Hoverable, Loading, Cover, Title, Actions, Size |
| Radzen | RadzenCard | Variant Filled/Flat/Outlined/Text |
| Blazorise | Card + regions | Bootstrap-oriented composition |

### Tier B — Secondary

| Library | Component |
|---------|-----------|
| HAVIT Blazor | HxCard |
| Material.Blazor / others | Material-style surfaces |

---

## 2. Feature matrix

Legend: Full / Partial / Missing / SfCard target = star

| Feature | Mud | Telerik | Ant | Radzen | Blazorise | HAVIT | SfCard |
|---------|-----|---------|-----|--------|-----------|-------|--------|
| Header / Body / Actions | Y | Y | Y | P | Y | Y | Y |
| Footer | P | Y | P | P | Y | Y | Y |
| Media / Image / Cover | Y | Y | Y | P | Y | Y | Y |
| Title / SubTitle | P | Y | Y | P | Y | Y | Y |
| Separator | P | Y | P | N | P | P | Y |
| Elevation | Y | P | P | Y | P | P | Y |
| Outlined / bordered | Y | P | Y | Y | P | P | Y |
| Flat / filled variants | P | P | P | Y | P | P | Y |
| Horizontal orientation | P | Y | P | P | P | P | Y |
| Hoverable | P | P | Y | P | P | P | Y |
| Loading | P | P | Y | P | P | P | Y |
| Width / Height | Style | Y | Style | Style | Style | Style | Y |
| Content padding control | Y | P | P | P | P | P | Y |
| Square corners | Y | P | P | P | P | P | Y |
| Deck/list helpers | P | Y | Grid | Row | P | P | samples |
| RTL | Y | Y | P | Y | P | P | Y |
| Clickable card | P | P | P | P | P | P | Y |
| CSS isolation | P | P | P | P | P | P | Y |
| Forced-colors | P | P | P | P | P | P | Y |
| MIT OSS | Y | N | Y | Y | Y | Y | Y |

---

## 3. Acceptance checklist

- [ ] Header, Content, Actions, Footer, Media, Title, SubTitle, Separator
- [ ] Filled / Outlined / Elevated / Flat
- [ ] Vertical / Horizontal
- [ ] Hoverable, Loading, optional IsClickable
- [ ] Elevation, Square, Width, Height, ContentPadding
- [ ] axe clean samples
- [ ] forced-colors + High Contrast
- [ ] CSS isolation
- [ ] RTL sample
- [ ] 0 build warnings (XML complete)

---

## 4. Differentiation

1. Full block set in MIT Toolkit
2. Variant + Orientation + Hoverable + Loading together
3. A11y + forced-colors + isolation + XML docs for zero warnings
4. Deck/list via documented samples without proprietary layout lock-in
