# BL0007 Analysis Report — Component parameter should be auto property

**Original investigation date:** 2026-10-01
**Rule:** `BL0007` (Microsoft.AspNetCore.Components.Analyzers) — *Component parameter should be auto property*
**Historical decision:** **Document only — no code conversion applied by that investigation.** Retain justified reactive-setter suppressions rather than blindly moving side effects into lifecycle methods. Release policy requires re-attestation; this report is not evidence that a later release was re-attested.

**Documentation synchronization:** 2026-10-03. Sections 1–6 preserve the original investigation, measurements, examples, and decision; they are not a live inventory. No analyzer, build, or test was rerun for this synchronization. Current source observations are recorded separately below.

### Current source inventory (2026-10-03, not analyzer diagnostics)

Six types currently carry the class-level BL0007 suppression, not three. Counts below inspect declared `[Parameter]` properties, including intervening attributes, and distinguish auto-properties from explicit accessor bodies; inherited parameters are excluded. These counts do **not** establish how many diagnostics the analyzer would emit without suppressions.

| Type | Source file under `src/Components/Charts/Chart/` | Declared parameters | Auto-properties | Explicit-accessor properties |
|---|---|---:|---:|---:|
| `ChartAxis` | `ChartAxis/Models/ChartAxis.cs` | 60 | 28 | 32 |
| `ChartLegendSettings` | `Legend/LegendSettings.cs` | 27 | 5 | 22 |
| `ChartSeries` | `Series/ChartSeries.cs` | 55 | 32 | 23 |
| `ChartCommonMarker` | `Series/Marker/ChartCommonMarker.cs` | 12 | 2 | 10 |
| `ChartLastDataLabel` | `Series/LastLabelSettings/ChartLastDataLabel.cs` | 7 | 0 | 7 |
| `ChartStackLabelSettings` | `StackLabel/ChartStackLabelSettings.cs` | 6 | 1 | 5 |
| **Total** | **6 types** | **167** | **68** | **99** |

The original three-type table recorded 79 violations (32/24/23); those three source classes now contain 77 explicit-accessor parameters (32/22/23). The historical 48-diagnostic Legend experiment below is retained as reported, not equated to a property count or extrapolated to six types. Reconciling analyzer diagnostic multiplicity requires a separately authorized investigation. The [development record](../.github/DEVELOPMENT.md#known-analyzer--trim--aot-findings) reports a later local zero-warning library build; it is not a new unsuppressed BL0007 measurement or release result.

---

## 1. Historical summary — 2026-10-01

| Metric | Originally recorded result |
|---|---|
| BL0007 warnings escaping the build in that investigation | **0** |
| Reported BL0007 violations in the investigated source | **79** |
| Files identified in that report | **3** (all chart sub-components; not the current suppression inventory) |
| Suppression mechanism | Class-level `[SuppressMessage("Usage", "BL0007:...")]` with written justification |
| Input components (`SfTextBox`, `SfNumericTextBox`, `SfDatePicker`, …) | **Not reported by that experiment** — see §4 |

The original report attributed its clean build to deliberate suppressions while the analyzer remained active. That conclusion is historical, not a fresh verification of all current source.

### Historical verification method

The original investigation recorded the following analyzer checks (not rerun here):

1. Temporarily removing the suppression from `LegendSettings.cs` produced **48** `BL0007` diagnostics.
2. Removing all three suppressions produced the full violation set below.
3. Restoring the suppressions returned the build to **0** escaping warnings. The three files were confirmed byte-identical to their pre-test state.

```
dotnet build src/Syncfusion.Blazor.Toolkit.csproj -f net8.0 --no-incremental
→ Build succeeded.  BL0007 count: 0
```

---

## 2. Historical violation locations — 2026-10-01

| File | Originally reported violations | Class | Suppression justification (verbatim, condensed) |
|---|---:|---|---|
| `src/Components/Charts/Chart/ChartAxis/Models/ChartAxis.cs` | 32 | `ChartAxis` | "explicit setters compare against a backing field and dispatch incremental renderer updates (layout/axis/series refresh) the moment a value changes… deliberate part of the chart update pipeline." |
| `src/Components/Charts/Chart/Legend/LegendSettings.cs` | 24 | `ChartLegendSettings` | "dispatch incremental renderer updates (legend re-render / layout change) the moment a value changes." |
| `src/Components/Charts/Chart/Series/ChartSeries.cs` | 23 | `ChartSeries` | "series re-render / renderer swap / legend refresh the moment a value changes." |

---

## 3. Before / After examples

These show the *correct* BL0007 transformation for the three categories of setter found. They are illustrative — **not applied** — because of the risks in §5.

### Example A — Trivial guarded setter (LOW risk, mechanically convertible)

Some properties only guard-and-assign with **no** reactive side effect. Example: `ChartLegendSettings.Opacity`.

**Before**
```csharp
private double _opacity = 1;

[Parameter]
public double Opacity
{
    get => _opacity;
    set
    {
        if (_opacity != value)
        {
            _opacity = value;   // no side effect
        }
    }
}
```

**After (BL0007-compliant)**
```csharp
[Parameter]
public double Opacity { get; set; } = 1;
```

> The `if (_opacity != value)` guard is dead weight here because the body does nothing but assign. This is safe to convert to a plain auto-property.

---

### Example B — Reactive setter with renderer dispatch (MEDIUM risk)

Example: `ChartLegendSettings.Visible`.

**Before**
```csharp
private bool _visible = true;

[Parameter]
public bool Visible
{
    get => _visible;
    set
    {
        if (_visible != value)
        {
            _visible = value;
            if (_renderer is not null)
            {
                _renderer.RendererShouldRender = true;
                Owner?.OnLayoutChange();
            }
        }
    }
}
```

**After (BL0007-compliant pattern)**
```csharp
[Parameter]
public bool Visible { get; set; } = true;

private bool _previousVisible = true;

protected override void OnParametersSet()
{
    base.OnParametersSet();

    if (Visible != _previousVisible)
    {
        _previousVisible = Visible;
        if (_renderer is not null)
        {
            _renderer.RendererShouldRender = true;
            Owner?.OnLayoutChange();
        }
    }
}
```

> ⚠️ **Behavior change:** the dispatch now fires once per parameter batch in `OnParametersSet`, *after* all parameters are set — not the instant `Visible` is assigned. For a single reactive property this is usually equivalent, but when several reactive properties change together the *number and ordering* of `OnLayoutChange()` calls changes.

---

### Example C — Reactive setter also invoked internally (HIGH risk — see §5)

Example: `ChartLegendSettings.Location` / `Border` / `TextStyle` / `Margin`. These setters are called **programmatically** from `UpdateLegendProperties(...)` and from child components (`ChartLegendTextStyle`), not only by the Blazor parameter-setting machinery.

**Before**
```csharp
[Parameter]
public ChartLocation Location
{
    get => PrevLocation;
    set
    {
        if (PrevLocation != value)
        {
            PrevLocation = value;
        }
    }
}

// Called internally — NOT via Blazor parameter binding:
internal void UpdateLegendProperties(string key, object keyValue)
{
    switch (key)
    {
        case nameof(Location):
            Location = (ChartLocation)keyValue;   // relies on setter running now
            LegendPropertyChanged();
            break;
        // ...
    }
}
```

**Why the naive conversion breaks:** if the reactive logic moves to `OnParametersSet`, the internal `Location = ...` assignment no longer triggers that logic, because `OnParametersSet` only runs during the Blazor render lifecycle — not on arbitrary property assignment. The programmatic update path would silently lose its side effect.

A correct conversion would require **rerouting every internal caller** to invoke the side-effect method directly (e.g. call `LegendPropertyChanged()` explicitly at each internal assignment) and adding per-property change tracking — a non-trivial refactor of the render pipeline.

---

## 4. Why the input components are NOT violations

`SfTextBox`, `SfTextArea`, `SfNumericTextBox`, `SfDatePicker`, `SfTimePicker`, etc. use **expression-bodied delegation** to a base class:

```csharp
[Parameter]
public string Placeholder
{
    get => BasePlaceholder; set => BasePlaceholder = value;
}
```

`BL0007` only flags setters that assign to a **private backing field of the same component**. A getter/setter that delegates to another property (`BasePlaceholder`) is **not** reported by the analyzer. This was confirmed empirically: with all three chart suppressions removed, **none** of the input components produced a `BL0007` diagnostic.

No action is required for these components.

---

## 5. Complex cases flagged for manual review

If a future decision is made to actually convert these (rather than keep the documented suppression), the following must be handled carefully. **These are the reasons a blind auto-fix was not applied.**

1. **Immediate vs. batched dispatch (all 3 files).** Setters currently dispatch renderer/layout updates synchronously on assignment. `OnParametersSet` batches them to once-per-render. When multiple reactive parameters change in the same render, the count/ordering of `OnLayoutChange()` / `LegendPropertyChanged()` / renderer refreshes will differ. Requires visual + interaction regression testing of charts.

2. **Setters invoked internally (`LegendSettings`).** `UpdateLegendProperties(...)` and `ChartLegendTextStyle` assign these properties directly and rely on the setter's side effect running *then*. `OnParametersSet` will not run for those assignments. Every internal caller must be rerouted to call the side-effect method explicitly.

3. **Trim/AOT suppressions bound to the setter body (`ChartSeries.Visible` and peers).** The setter carries `[UnconditionalSuppressMessage("Trimming","IL2026")]` and `[UnconditionalSuppressMessage("AOT","IL3050")]` because it reflects over the user-supplied `DataSource` element type via `Renderer.UpdateCategoryData()`. Moving that logic into `OnParametersSet` moves the trim/AOT surface too, and the suppressions must be re-placed and re-audited on the new method.

4. **Validation that throws (`LegendSettings.AccessibilityRole`).** The setter calls `DataVizCommonHelper.AriaRoleValidator.EnsureValidRole(value, ...)`, which throws on invalid input. In the investigated implementation this throws *at assignment*. In `OnParametersSet` it would throw during the render lifecycle instead — a different, harder-to-attribute failure mode for consumers. Needs a deliberate call on whether to keep set-time validation.

5. **Bounds recalculation with cross-property dependencies (`LegendSettings.Alignment`).** The setter recomputes `LegendBounds.X/Y` using the *current* `_position` and owner rects, then calls `CalculateRenderTreeBuilderOptions()`. Correctness depends on `Position` already being set. In `OnParametersSet` the relative ordering of `Position` vs `Alignment` no longer matters (both are set before it runs), which is arguably *more* correct — but the behavior around partial updates changes and must be validated.

6. **Complex conditional pipelines (`ChartAxis`, `ChartSeries`).** Several setters branch into layout-vs-range-refresh decisions (`FindLayoutChange()`, `_needLayoutUpdate`, `_refreshRange`, category-axis relabeling, stacking recalculation). These are the highest-value reactive paths and the riskiest to move. Each needs individual review and a targeted chart test.

---

## 6. Recommendation

Keep the current class-level suppressions. They are:

- **Accurate** — the setters genuinely implement a reactive render pipeline.
- **Documented** — each carries a specific justification and points to `DEVELOPMENT.md`.
- **Governed** — `GlobalSuppressions.cs` states every suppression is re-attested at each major release.

If conversion is later desired, do it **per-property**, starting with the trivial no-side-effect setters (Example A category), and gate the reactive ones (Examples B/C) behind chart rendering regression tests. Do not batch-convert.
