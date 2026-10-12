# Accessibility Insights — DRAFT evidence register

**Status: Not Evaluated.** No retained FastPass or Assessment artifact establishing the previous results was found in the reviewed evidence set. The previously cited `tests/accessibility/insights/` directory is absent. This file is a validation checklist, not a test report.

## Evidence status

| Item | Status / evidence required |
|---|---|
| Product/package and source commit evaluated | Not established; record exact source and package digest, including any local changes. |
| Run date, evaluator, tool/ruleset version | Not established; record actual values when an evaluation occurs. No evaluator or approval is assigned here. |
| Browser/OS, theme, viewport, culture, render mode | Not established; retain the actual configuration, including prerender and interactive phases. |
| FastPass findings and Assessment results | Not Evaluated; no counts, severity totals, or zero-finding claims can be supported. |
| Component coverage | Use the 17-component register in [the DRAFT standards checklist](../../docs/VPAT-2.5-INT.md); record pages, parameters, and states actually exercised. |
| Finding-to-issue links | Not verified. Link reproducible findings only after confirming the issue describes the same behavior. |

## What is configured

[`../workflows/ci.yml`](../workflows/ci.yml) configures Playwright tests and report uploads. The root [`playwright.config.ts`](../../playwright.config.ts) selects Chromium and starts the Playwright sample host. Those facts do not establish an Insights Assessment or a successful accessibility run. The reviewed workflows contain no nightly Insights job or `accessibility-bot` publication process.

## Evaluation checklist

- [ ] Pin the evaluated version/commit, package bytes, URLs/sample fixtures, browser/OS, mode, generated theme, locale, and tool/ruleset versions.
- [ ] Run and retain the automated report, including excluded rules, incomplete checks, and pages/states not reached.
- [ ] Complete guided manual checks; automated results alone cannot establish WCAG conformance.
- [ ] Cover keyboard traversal, modal exit/restoration, hover/focus content, validation, dynamic updates, zoom/reflow, text spacing, forced colors, and reduced motion.
- [ ] Measure text/non-text contrast against actual backgrounds, with component states and calculation inputs recorded.
- [ ] Evaluate WCAG 2.4.11 focus obscuration (AA), 2.5.7 non-drag single-pointer alternatives, and 2.5.8 target geometry/spacing and justified exceptions. Do not substitute keyboard alternatives for 2.5.7 or assume native targets are large enough.
- [ ] Link AT observations from the [screen-reader checklist](screen-reader-smoke.md); retain raw observations separately from expected outcomes.
- [ ] Record failures, unresolved checks, scope exclusions, and any actual reviewer decision without inventing approvals.

## Superseded legacy claims

The claimed `2026-09-06` session, Insights v3.0.0 / Edge 130 sweep, numerical finding totals, issue associations #271–#273, nightly/PR bot runs, quarterly assessments, and parallel VoiceOver sweeps are **unverified and superseded**, not established historical runs. They must not be cited as evidence of passing results or absence of outstanding issues.

Report findings through [ACCESSIBILITY.md](../ACCESSIBILITY.md). Private disclosures remain **security@syncfusion.com** under [SECURITY.md](../SECURITY.md).