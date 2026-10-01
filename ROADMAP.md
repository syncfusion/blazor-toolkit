# Roadmap — Syncfusion® Toolkit for Blazor

This roadmap communicates **where the toolkit is heading** so you can confidently bet on it today. It is a living document: priorities shift with community feedback. Dates are directional, not contractual.

Want to influence it? 👉 Open or upvote a [feature request](https://github.com/syncfusion/blazor-toolkit/issues/new/choose) or start a [discussion](https://github.com/syncfusion/blazor-toolkit/discussions).

---

## ✅ Available today

The toolkit ships MIT-licensed, accessible, trim/AOT-ready components across these areas:

| Category | Components |
|----------|------------|
| Data Viz | Chart (line, area, column/bar, scatter, bubble, spline, stacking) |
| Buttons | Button, ButtonGroup, CheckBox, RadioButton, Toggle Switch |
| Calendars | Calendar, DatePicker, DateTimePicker, TimePicker |
| Inputs | TextBox, TextArea, NumericTextBox, File Upload |
| Layout / Popups | Dialog, Tooltip |
| Notification | Spinner |

Every component targets **.NET 8 / 9 / 10** across Blazor **Server, WebAssembly, and Auto**, with bUnit + Playwright coverage, WCAG conformance (see [`ACCESSIBILITY.md`](.github/ACCESSIBILITY.md) and the [VPAT](docs/VPAT-2.5-INT.md)), and a published SBOM.

---

## 🚧 Near term — closing the breadth gap

These are the components developers most often reach for when deciding whether a single library can carry a whole app. Highest-leverage first.

| Priority | Component | Why it matters |
|:--------:|-----------|----------------|
| **P0** | **Data Grid** (sort, page, filter, selection) | The #1 reason teams adopt a UI library. Virtualized rows, template columns, and editing to follow. |
| P0 | **Select / DropDownList** | Core form building block; foundation for Autocomplete/ComboBox. |
| P1 | **Autocomplete / ComboBox** | Type-ahead selection over local and remote data. |
| P1 | **Form layout + validation** | First-class `EditForm`/`EditContext` integration with grouped layout. |
| P1 | **Menu & Navigation** (AppBar, Drawer/Sidebar, NavMenu) | App shell scaffolding so no second library is needed for chrome. |
| P2 | **Tabs** | Common content organization pattern. |
| P2 | **Card** | Content container primitive. |
| P2 | **Snackbar / Toast** | Transient action feedback. |
| P2 | **Accordion / Expansion panels** | Progressive disclosure. |
| P2 | **Pagination** | Standalone pager, reusable by Data Grid. |

---

## 🔭 Later — depth and polish

- Data Grid advanced features: grouping, aggregates, column virtualization, Excel/CSV export.
- Tree / TreeView and TreeGrid.
- Chips, Badge, Avatar, Breadcrumb.
- Stepper / Wizard.
- Additional chart types (pie/doughnut, radar, financial) and annotations.
- Theming expansion: more built-in themes and a documented theming/token API.
- Localization and RTL coverage across all components.

---

## Cross-cutting commitments (every release)

These are non-negotiable quality gates that make the toolkit a *safe* bet, not just a feature list:

- **Accessibility** — WCAG conformance, keyboard navigation, ARIA, and VPAT evidence maintained per component.
- **Security & supply chain** — CodeQL scanning, NuGet audit, threat model, and a published SBOM on every release.
- **Trimming / AOT** — components stay `IsTrimmable` + `IsAotCompatible` with analyzers enabled, keeping WASM payloads small.
- **Test coverage** — bUnit unit tests + Playwright browser tests across .NET 8 / 9 / 10.
- **API continuity** — a predictable, low-friction upgrade path to the commercial Syncfusion Blazor suite when you outgrow the toolkit.

---

## How priorities are set

1. **Community demand** — 👍 reactions and comments on feature requests.
2. **Adoption blockers** — components that currently force a second dependency.
3. **Quality bar** — nothing ships until it meets the cross-cutting commitments above.

_Last reviewed: 2026-10._
