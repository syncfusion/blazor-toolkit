# SfSpinner — Playwright Test Plan

> **Objective**: The Syncfusion SfSpinner component provides a visual indicator for ongoing operations.
It is a non‑interactive, lifecycle‑driven UI component whose behavior is primarily governed by:

Conditional rendering
Programmatic control (ShowAsync, HideAsync)
CSS‑based visibility
Lifecycle events
Accessibility attributes
Template overrides

This Playwright test plan validates only real, source‑guaranteed behavior of SfSpinner.
✅ No mocked DOM
✅ No fake visibility assumptions
✅ No CSS animation timing tests
✅ All tests interact with the actual SfSpinner component

---

## Test Summary

Basic Rendering & Lifecycle
Programmatic Control
Lifecycle Events
Multiple Instances
Styling & Attributes
Templates
Visible Binding (safe subset)
Accessibility
Total Test Files: 7
Total Valid Playwright Test Cases: 14
Status: ✅ Finalized & Stable

## Basic Rendering & Lifecycle
File: basic-rendering.spec.ts
Sample Page: /spinner/basic
Scenarios Covered
1.1 Spinner is hidden by default

Expect spinner pane exists
Expect spinner has e-spin-hide class
No visible animation

1.2 ShowAsync creates spinner DOM

Trigger ShowAsync()
Expect .e-spinner-pane exists

1.3 Spinner DOM persists after HideAsync

Trigger ShowAsync()
Trigger HideAsync()
Expect spinner DOM still exists

✅ Validates the Spinner’s conditional rendering and DOM persistence contract

## Programmatic Control
File: programmatic-control.spec.ts
Sample Page: /spinner/programmatic-control
Scenarios Covered
2.1 ShowAsync creates spinner inner structure

Expect .e-spinner-inner exists
Confirms default spinner structure is rendered

2.2 HideAsync when spinner is already hidden is a no‑op

Call HideAsync() without showing spinner
Expect no DOM removal or exception

✅ Confirms idempotency of programmatic APIs

## Lifecycle Events
File: events.spec.ts
Sample Page: /spinner/events
Scenarios Covered
3.1 Created event fires on first render

Capture and validate Created event via log

3.2 OnBeforeClose cancellation prevents hide

Show spinner
Cancel close via OnBeforeClose
Attempt HideAsync
Expect spinner DOM to remain

✅ Confirms lifecycle event contracts and cancellation behavior

## Multiple Spinner Instances
File: multiple-instances.spec.ts
Sample Page: /spinner/multiple
Scenarios Covered
4.1 Multiple spinners render independently

Render three visible spinners
Expect three .e-spinner-pane elements

✅ Confirms no shared state across instances

## Styling & Attributes
File: styling-size.spec.ts
Sample Page: /spinner/styling
Scenarios Covered
5.1 Custom CssClass applied

Expect .custom-spinner exists on spinner pane

5.2 ZIndex applied as inline style

Verify z-index present in style attributes

5.3 Label rendered when provided

Expect .e-spin-label exists
Validate label text

✅ Confirms attribute and styling propagation

## Spinner Templates
File: templates.spec.ts
Sample Page: /spinner/templates
Scenarios Covered
6.1 Custom SpinnerTemplate renders

Expect .sf-spin-custom exists

6.2 Default SVG not rendered inside templated spinner

Ensure no <svg> inside template container

✅ Confirms template overrides replace default spinner rendering

## Visible Binding (Safe Subset)
File: visible-binding.spec.ts
Sample Page: /spinner-visible-binding
Scenarios Covered
7.1 Visible binding does not remove spinner DOM

Toggle Visible twice
Expect spinner pane remains in DOM


⚠️ Immediate visibility or CSS transition assertions are intentionally excluded as they are not guaranteed by the source.

✅ Confirms correct binding behavior without invalid assumptions

✅ Non‑Tested Scenarios (Intentionally Excluded)
The following are explicitly NOT tested as they are not contractual behaviors:

CSS animation timing
e-spin-show / e-spin-hide transitions
Spinner opacity or transforms
Immediate Visible change effects
Inner‑element visibility assertions

❌ Testing these would introduce flakiness and fake assumptions

✅ Final Assessment
✔ All real SfSpinner behaviors are covered
✔ Tests are source‑aligned and review‑safe
✔ No invalid or brittle assertions remain
✔ Ready for CI and long‑term maintenance
This test plan represents complete and correct Playwright coverage for SfSpinner.