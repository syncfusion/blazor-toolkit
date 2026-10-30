# Card Component Test Matrix

## Purpose

This document defines the mandatory automated verification matrix for the Card component. The goal is to ensure complete functional, accessibility, visual, responsive, and interaction coverage through automated testing so that no manual validation is required during release readiness assessment.

## Test Strategy

| Layer | Purpose |
|---------|---------|
| bUnit | Component rendering, parameter validation, state transitions, event contracts, accessibility semantics |
| Playwright | End-to-end interaction, keyboard navigation, responsive behavior, browser validation |
| axe | Automated accessibility validation |
| Visual Regression | Visual consistency across variants, themes, layouts, breakpoints, and states |

---

# 1. bUnit Coverage

## Rendering Scenarios

| ID | Scenario |
|----|----------|
B-CARD-001 Empty card renders without exceptions |
B-CARD-002 Header-only card renders correctly |
B-CARD-003 Body-only card renders correctly |
B-CARD-004 Footer-only card renders correctly |
B-CARD-005 Media-only card renders correctly |
B-CARD-006 Actions-only card renders correctly |
B-CARD-007 Header + Body composition renders correctly |
B-CARD-008 Header + Footer composition renders correctly |
B-CARD-009 Body + Footer composition renders correctly |
B-CARD-010 Media + Header composition renders correctly |
B-CARD-011 Media + Footer composition renders correctly |
B-CARD-012 Media + Body composition renders correctly |
B-CARD-013 Full card composition renders correctly |
B-CARD-014 Multiple cards render independently |
B-CARD-015 Nested cards render correctly |
B-CARD-016 SfCardTitle and SfCardSubTitle render with correct CSS classes |
B-CARD-017 SfCardSeparator renders with role="separator" |
B-CARD-018 SfCardSeparator aria-orientation is vertical inside Horizontal card |

---

## Content Rendering

| ID | Scenario |
|----|----------|
B-CARD-019 Plain text content |
B-CARD-020 Rich HTML content |
B-CARD-021 RenderFragment content |
B-CARD-022 Large content block |
B-CARD-023 Long text wrapping |
B-CARD-024 Overflow content handling |
B-CARD-025 Nested components |
B-CARD-026 Nested buttons |
B-CARD-027 Nested inputs |
B-CARD-028 Nested forms |
B-CARD-029 Nested charts |
B-CARD-030 Dynamic content updates |

---

## Variant Validation

| ID | Scenario |
|----|----------|
B-CARD-031 Default variant |
B-CARD-032 Elevated variant |
B-CARD-033 Outlined variant |
B-CARD-034 Filled variant |
B-CARD-035 Flat variant |
B-CARD-036 Runtime variant update |
B-CARD-037 Variant CSS classes applied correctly |

---

## Layout Validation

| ID | Scenario |
|----|----------|
B-CARD-038 Vertical orientation |
B-CARD-039 Horizontal orientation |
B-CARD-040 Runtime orientation update |
B-CARD-041 Media alignment |
B-CARD-042 Header alignment |
B-CARD-043 Footer alignment |
B-CARD-044 Actions alignment |
B-CARD-045 Square property applies e-card-square class |
B-CARD-046 Hoverable property applies e-card-hoverable class |
B-CARD-047 ContentPadding true (default) |
B-CARD-048 ContentPadding false removes region padding |

---

## Parameter Validation

| ID | Scenario |
|----|----------|
B-CARD-049 CssClass applied correctly |
B-CARD-050 Multiple CssClass values |
B-CARD-051 Null CssClass |
B-CARD-052 Empty CssClass |
B-CARD-053 Style attribute applied |
B-CARD-054 Null style |
B-CARD-055 Empty style |
B-CARD-056 Width customization |
B-CARD-057 Height customization |
B-CARD-058 Custom attributes rendered |
B-CARD-059 Arbitrary HTML attributes rendered |
B-CARD-060 Elevation value is clamped between 0 and 5 |
B-CARD-061 Elevated variant boosts elevation by +2 (capped at 5) |
B-CARD-062 Outlined and Flat variants force elevation to 0 |
B-CARD-063 data-elevation attribute reflects resolved elevation |
B-CARD-064 Supplied Id is rendered on root |
B-CARD-065 Generated Id remains stable across re-renders |
B-CARD-066 HtmlAttributes class and style are merged with computed values |
B-CARD-067 Other HtmlAttributes override computed attributes |
B-CARD-068 Global EnableRtl adds e-rtl class |

---

## Media Specific

| ID | Scenario |
|----|----------|
B-CARD-069 SfCardMedia renders ImageUrl with Alt text |
B-CARD-070 SfCardMedia without Alt renders empty alt (decorative) |
B-CARD-071 CardMediaFit.Cover (default) |
B-CARD-072 CardMediaFit.Contain |
B-CARD-073 CardMediaFit.Fill |
B-CARD-074 CardMediaFit.None emits no object-fit |
B-CARD-075 Custom media Height |
B-CARD-076 Custom ChildContent when ImageUrl is not set |

---

## State Scenarios

| ID | Scenario |
|----|----------|
B-CARD-077 Default state |
B-CARD-078 Clickable state |
B-CARD-079 Loading state |
B-CARD-080 Hover state class rendering |
B-CARD-081 Focus-visible state rendering |
B-CARD-082 Clickable + Loading combination sets aria-disabled |

---

## State Transitions

| ID | Scenario |
|----|----------|
B-CARD-083 Loading → Loaded |
B-CARD-084 Loaded → Loading |
B-CARD-085 Clickable → Non-clickable |
B-CARD-086 Non-clickable → Clickable |
B-CARD-087 Vertical → Horizontal |
B-CARD-088 Horizontal → Vertical |
B-CARD-089 Runtime content updates |
B-CARD-090 Runtime action updates |
B-CARD-091 Runtime Elevation / Square / Hoverable / ContentPadding updates |

---

## Events

| ID | Scenario |
|----|----------|
B-CARD-092 Mouse click invokes callback |
B-CARD-093 Callback invoked once |
B-CARD-094 Multiple clicks produce expected count |
B-CARD-095 Loading state suppresses callback |
B-CARD-096 Enter activates clickable card |
B-CARD-097 Space activates clickable card |
B-CARD-098 Tab navigation preserved |
B-CARD-099 Event arguments propagated correctly |
B-CARD-100 Enter activates on keydown |
B-CARD-101 Space activates on keyup only after matching keydown |
B-CARD-102 Repeat keys are ignored |
B-CARD-103 Click or keyboard activation inside SfCardActions does not raise card OnClick |
B-CARD-104 SfCardActions receives inert attribute while card is loading |

---

## Accessibility Semantics

| ID | Scenario |
|----|----------|
B-CARD-105 Default role assignment |
B-CARD-106 Custom role assignment |
B-CARD-107 aria-label support |
B-CARD-108 aria-labelledby support |
B-CARD-109 aria-describedby support |
B-CARD-110 Focusable interactive card |
B-CARD-111 Non-interactive card not focusable |
B-CARD-112 Focus-visible semantics |
B-CARD-113 Keyboard activation semantics |
B-CARD-114 Screen-reader attributes preserved |
B-CARD-115 Loading sets aria-busy="true" |
B-CARD-116 Clickable + Loading sets aria-disabled="true" |

---

# 2. Playwright Coverage

## Core Interactions

| ID | Scenario |
|----|----------|
P-CARD-001 Clickable card activation |
P-CARD-002 Loading card interaction suppression |
P-CARD-003 Multiple card interaction |
P-CARD-004 Nested interactive content |
P-CARD-005 Touch interaction |
P-CARD-006 Mouse interaction |
P-CARD-007 Pointer interaction |
P-CARD-008 Actions isolation – click on action button does not activate card |

---

## Keyboard Navigation

| ID | Scenario |
|----|----------|
P-CARD-009 Tab enters card |
P-CARD-010 Shift+Tab exits card |
P-CARD-011 Enter activation |
P-CARD-012 Space activation |
P-CARD-013 Focus-visible indicator |
P-CARD-014 Keyboard navigation order |
P-CARD-015 Multiple cards keyboard navigation |
P-CARD-016 Enter activates on keydown |
P-CARD-017 Space activates on keyup |
P-CARD-018 Actions buttons remain independently focusable |

---

## Responsive Validation

| ID | Scenario |
|----|----------|
P-CARD-019 Mobile portrait layout |
P-CARD-020 Mobile landscape layout |
P-CARD-021 Tablet layout |
P-CARD-022 Desktop layout |
P-CARD-023 Large desktop layout |
P-CARD-024 Horizontal card responsiveness |
P-CARD-025 Media responsiveness |
P-CARD-026 Overflow handling |

---

## Theme Validation

| ID | Scenario |
|----|----------|
P-CARD-027 Default theme |
P-CARD-028 Dark theme |
P-CARD-029 Theme switch at runtime |
P-CARD-030 Consistent spacing across themes |

---

## RTL Validation

| ID | Scenario |
|----|----------|
P-CARD-031 RTL rendering |
P-CARD-032 RTL horizontal layout |
P-CARD-033 RTL actions layout |
P-CARD-034 RTL keyboard navigation |
P-CARD-035 RTL focus handling |

---

## High Contrast Validation

| ID | Scenario |
|----|----------|
P-CARD-036 Forced colors mode |
P-CARD-037 High contrast light |
P-CARD-038 High contrast dark |
P-CARD-039 Focus visibility in high contrast |

---

## Browser Validation

| ID | Scenario |
|----|----------|
P-CARD-040 Chromium |

---

## End-to-End Scenarios

| ID | Scenario |
|----|----------|
P-CARD-041 Header only page |
P-CARD-042 Media card page |
P-CARD-043 Actions page |
P-CARD-044 Clickable card page |
P-CARD-045 Loading card page |
P-CARD-046 Horizontal card page |
P-CARD-047 Dynamic update scenario |
P-CARD-048 Multiple card collection |
P-CARD-049 Nested interaction scenario |

---

# 3. Accessibility Coverage (axe)

| ID | Scenario |
|----|----------|
A-CARD-001 Basic card |
A-CARD-002 Clickable card |
A-CARD-003 Loading card |
A-CARD-004 Header-only card |
A-CARD-005 Media card |
A-CARD-006 Action card |
A-CARD-007 Horizontal card |
A-CARD-008 RTL card |
A-CARD-009 Dark theme card |
A-CARD-010 High contrast card |
A-CARD-011 Nested interactive content |
A-CARD-012 Keyboard-only usage |
A-CARD-013 Focus visibility validation |
A-CARD-014 Color contrast validation |
A-CARD-015 Separator semantics |
A-CARD-016 Media with empty Alt (decorative) |

---

# 4. Visual Regression Coverage

| ID | Baseline |
|----|----------|
V-CARD-001 Basic card |
V-CARD-002 Elevated |
V-CARD-003 Outlined |
V-CARD-004 Filled |
V-CARD-005 Flat |
V-CARD-006 Header only |
V-CARD-007 Body only |
V-CARD-008 Footer only |
V-CARD-009 Media only |
V-CARD-010 Actions only |
V-CARD-011 Clickable |
V-CARD-012 Loading |
V-CARD-013 Horizontal |
V-CARD-014 Vertical |
V-CARD-015 Long content |
V-CARD-016 Overflow content |
V-CARD-017 Mobile |
V-CARD-018 Tablet |
V-CARD-019 Desktop |
V-CARD-020 Dark theme |
V-CARD-021 High contrast |
V-CARD-022 RTL |
V-CARD-023 Hover |
V-CARD-024 Focus-visible |
V-CARD-025 Pressed |
V-CARD-026 Nested content |
V-CARD-027 Multiple cards layout |
V-CARD-028 Square |
V-CARD-029 Hoverable |
V-CARD-030 ContentPadding off |
V-CARD-031 Media ObjectFit variants |
V-CARD-032 Separator (vertical & horizontal) |

---

# 5. Runtime Record

| Suite | Validation Scope |
|---------|---------|
| bUnit | All B-CARD test cases |
| Playwright | All P-CARD test cases across Chromium, Firefox, and WebKit |
| axe | All A-CARD accessibility validations |
| Visual Regression | All V-CARD visual baselines |
| Build Validation | Release build, analyzers, accessibility checks, trimming validation |

---

# 6. Exit Criteria

The Card component is considered release-ready when:

- All B-CARD tests pass.
- All P-CARD tests pass in Chromium, Firefox, and WebKit.
- All A-CARD accessibility validations pass.
- All V-CARD visual baselines pass.
- No accessibility violations remain.
- No component-specific warnings remain.
- All public APIs have automated validation coverage.
- All documented behaviors have automated validation coverage.
- No scenario listed in the Card specification requires manual verification.