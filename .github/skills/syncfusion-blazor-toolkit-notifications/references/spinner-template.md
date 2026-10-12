---
license: MIT
name: spinner-template
description: >
  Customize Syncfusion Blazor Toolkit SfSpinner visuals with nested
  SpinnerTemplates.Template, CssClass, Size, Thickness, scoped SVG stroke
  styling, and renderer CSS variables. USE FOR: branded or non-default
  spinners, optional Label text alongside decorative custom visuals,
  accessible naming with AriaLabel, and positioned spinner compositions.
---

# Spinner Template & Visual Customization

The current Toolkit `SfSpinner` renders a Fluent-style SVG arc, 30×30 pixels
by default. Customize it with `CssClass`, `Size`, `Thickness`, or a nested
`SpinnerTemplates.Template` fragment. `SfSpinner` has no `Type`, `Color`,
`Theme`, or direct `Template` parameter; do not use `SpinType` values here.

Source authority: `src/Components/Spinner/SfSpinner.razor.Members.cs`,
`SfSpinner.razor`, `ChildComponents/SpinnerTemplates.cs`, `Renderer/Border.*`,
and `src/wwwroot/styles/spinner.scss`.

Examples are fragments unless all state and dependencies are declared. Import
`Syncfusion.Blazor.Toolkit.Spinner` and complete host/service setup as described
in [spinner implementation](spinner-implementation.md). Initial visible markup
can render in static SSR; client-driven visibility, browser event handlers, and
Blazor JS interop require an interactive host. These snippets have not been
compile-tested.

## Anatomy

```
<spinner-root>            (CSS class e-spinner-pane; role=status)
  <spinner-inner>           (e-spinner-inner)
    <spinner-visual/>       (Fluent SVG or custom template)
    <label/>                (e-spin-label; when Label is set)
  </spinner-inner>
</spinner-root>
```

`SpinnerTemplates.Template` replaces the visual, not the root, inner wrapper,
or `Label`. Optional label text and a decorative custom visual can coexist.
While visible, the renderer uses the template only when global animation is
enabled; otherwise it uses the default `Border` renderer.

The default root is `position: absolute`, top/left zero, and 100% width/height.
Its `display: inline-flex` does not put it in normal inline flow. Give region
spinners a positioned, sized container; use explicit root positioning and
sizing for inline compositions. The inner wrapper is also absolutely positioned.

---

## `SpinnerTemplates.Template` (RenderFragment)

Nest `<Template>` inside `<SpinnerTemplates>`, which must be a direct child of
`SfSpinner`. **You are responsible for the custom animation and its sizing.**

```razor
@using Syncfusion.Blazor.Toolkit.Spinner

<div style="position: relative; min-height: 80px;">
    <SfSpinner @bind-Visible="isLoading" Label="Loading…" AriaLabel="Loading…">
        <SpinnerTemplates>
            <Template>
                <div class="brand-spinner" aria-hidden="true">
                    <div class="brand-spinner__dot"></div>
                    <div class="brand-spinner__dot"></div>
                    <div class="brand-spinner__dot"></div>
                </div>
            </Template>
        </SpinnerTemplates>
    </SfSpinner>
</div>

<style>
  .brand-spinner { display: inline-flex; gap: 4px; }
  .brand-spinner__dot {
    width: 8px; height: 8px; border-radius: 50%;
    background: var(--brand-color, #5b6dcd);
    animation: brand-pulse 1.2s ease-in-out infinite;
  }
  .brand-spinner__dot:nth-child(2) { animation-delay: 0.15s; }
  .brand-spinner__dot:nth-child(3) { animation-delay: 0.3s; }
  @@keyframes brand-pulse {
    0%, 80%, 100% { transform: scale(0.6); opacity: 0.5; }
    40% { transform: scale(1); opacity: 1; }
  }
  @@media (prefers-reduced-motion: reduce) {
    .brand-spinner__dot { animation: none; }
  }
</style>

@code {
    private bool isLoading = true;
}
```

**Rules:**
- `Label` still renders below the template. Keep decorative template content
  `aria-hidden="true"` and avoid repeating the label inside it.
- `AriaLabel` names the root status region independently of the visible `Label`.
- `CssClass` is applied to the *outer* root and survives template replacement.
- In a `.razor` `<style>` block, escape CSS at-rules as `@@keyframes`/`@@media`;
  use a single `@` in a standalone CSS file.

---

## Built-in visual style

The built-in renderer uses the `e-spin-fluent` SVG class. There is no per-spinner
style enum or `Theme` selector. Keep the default visual or supply a custom
`SpinnerTemplates.Template`; use `CssClass` for scoped application styling.

```razor
<SfSpinner Visible="true" CssClass="brand-loading" Label="Loading…" />
```

---

## `Size` and `Thickness` parameters

| Parameter | Type | Use |
|---|---|---|
| `Size` | `string` (e.g. `"24"`, `"36px"`, `"48"`) | Width AND height of the spinner (square) |
| `Thickness` | `string` (e.g. `"2"`, `"4"`, `"6px"`) | Width of the stroke arc |

```razor
<SfSpinner Size="48" Thickness="6" />
```

The current renderer parses the leading numeric portion of `Size`, rounds half
of it to an integer radius, and emits a pixel diameter. Use even pixel values
such as `"48"` or `"48px"`; `%`/`em` suffixes do not produce relative sizing.
An omitted or unparsable size falls back to 30px. A unitless `Thickness` gets
`px` appended; its default is `4px`. These parameters size the built-in SVG,
not arbitrary custom template content.

---

## Arc color through `CssClass`

`SfSpinner` has no `Color` parameter. For the current Fluent SVG, apply scoped
stroke rules in a global stylesheet loaded after Toolkit styles (or a `.razor`
`<style>` block). A parent's isolated stylesheet needs a wrapper and `::deep`
to target child-component elements.

```razor
<SfSpinner Visible="true" CssClass="brand-loading" Label="Loading…" />

<style>
  .e-spinner-pane.brand-loading .e-spinner-inner .e-spin-fluent .e-path-arc {
    stroke: var(--brand-primary, #ff5722);
  }
  .e-spinner-pane.brand-loading .e-spinner-inner .e-spin-fluent .e-path-circle {
    stroke: rgba(0, 0, 0, 0.08);
  }
</style>
```

`--brand-primary` is an application-owned variable, not a Toolkit parameter.
The built-in stylesheet sets stroke colors explicitly, so do not assume the
arc inherits `currentColor`.

---

## CSS variables used by the current SVG renderer

| Variable | Source |
|---|---|
| `--spinner-size` | Inline SVG value computed from `Size`; consumed by `Border.razor.css` |
| `--spinner-thickness` | Inline SVG value computed from `Thickness`; consumed by the spinner styles |

Prefer the public `Size` and `Thickness` parameters. The previously listed
`--e-spinner-arc-*` and `--e-spinner-zindex` variables are not implemented by
this renderer; use scoped stroke rules and the `ZIndex` parameter instead.

---

## Accessibility with custom templates

The root emits `role="status"`, `aria-busy`, and `aria-live` (default `polite`).
`AriaLabel` controls its accessible name (default `Loading`); `Label` is separate
visible text and still renders with a template. Fragment using `isLoading` and
the `.brand-spinner*` CSS from the template example above:

```razor
<SfSpinner @bind-Visible="isLoading"
           Label="Submitting your order"
           AriaLabel="Submitting your order"
           AriaLive="polite">
    <SpinnerTemplates>
        <Template>
            <div class="brand-spinner" aria-hidden="true">
                <div class="brand-spinner__dot"></div>
                <div class="brand-spinner__dot"></div>
                <div class="brand-spinner__dot"></div>
            </div>
        </Template>
    </SpinnerTemplates>
</SfSpinner>
```

`aria-hidden="true"` keeps decorative template nodes out of the accessibility
tree. A visible `Label` is optional and has no minimum character count; provide
a meaningful `AriaLabel` and test announcements in the application's supported
assistive technologies. A wrapper does not always need `role="alert"`; the
spinner already supplies a status region. These attributes do not by themselves
guarantee WCAG compliance.

---

## Composition patterns

### Inline (button-adjacent) spinner

Fragment: the application's submit handler owns `isSubmitting`. Place the
status indicator beside the disabled button, with explicit inline positioning.

```razor
<button disabled="@isSubmitting">Submit</button>
@if (isSubmitting)
{
    <SfSpinner Visible="true"
               Size="14"
               Thickness="2"
               AriaLabel="Submitting"
               CssClass="btn-spinner"/>
}

<style>
  .e-spinner-pane.btn-spinner {
    position: relative; width: 18px; height: 18px; margin-left: 6px;
  }
</style>
```

`CssClass` is appended to the spinner root, not the button. The dimensions above
constrain the default absolutely positioned inner element.

### Content-loading placeholder

Fragment: `data`, `showSpinner`, and `DataTable` are application-provided;
`DataTable` is not a Toolkit component. The loading region's minimum height
is illustrative; choose it for the expected content.

```razor
@if (data is null)
{
    <div style="position: relative; min-height: 160px;">
        <SfSpinner Visible="@showSpinner"
                   Label="Loading data"
                   AriaLabel="Loading data" />
    </div>
}
else
{
    <DataTable Data="@data" />
}
```

Reserve space in the wrapper to reduce layout shift; a `CssClass` name alone
does not size the region or guarantee matching dimensions. This is a spinner
placeholder, not a skeleton component.

### Page-level overlay

Fragment: the application owns `showOverlay` and any body-scroll locking.

```razor
@if (showOverlay)
{
    <div class="page-overlay">
        <SfSpinner @bind-Visible="@showOverlay"
                   ZIndex="9000"
                   Size="56"
                   Thickness="6"
                   Label="Loading the next page…"
                   AriaLabel="Loading the next page…" />
    </div>
}

<style>
  .page-overlay {
    position: fixed; inset: 0; z-index: 9000;
    background: rgba(255,255,255,0.6);
  }
</style>
```

`ZIndex` applies to the spinner root, not its wrapper. The wrapper also needs
positioning and a suitable z-index; ancestor stacking contexts still apply.

---

## Don'ts

- Don't use `Type`, `Color`, `Theme`, or a direct `<Template>` on `SfSpinner`.
- Don't duplicate visible `Label` text in the decorative template.
- Don't set `Size="auto"` — it falls back to the default 30×30.
- Don't override `.e-spin-hide`/`.e-spin-show` display rules with unconditional
  root display styles; they control visibility.
- Don't rely on a spinner to disable input; disable the action separately and
  keep its label accessible.