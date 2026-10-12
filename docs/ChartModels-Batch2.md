# ChartModels Internal API-Leak Remediation — Batch 2 (2A–2D)

**Historical status:** Batch 2 was recorded complete (2A, 2B-1, 2B-2, 2C, 2D). Sections 1–9 retain the batch results, validation at that time, and public-API approval fence. The original report did not record a batch date; none is inferred here.

**Scope of this document:** Documentation only. No source changes. The fence still requires explicit API redesign/product approval before changing the protected public contracts; historical scope restrictions are not instructions to undo later approved work or reintroduce resolved test failures.

## Documentation-sync status — 2026-10-03

- The five historical bUnit `CS0266` assignments and nested generated-source compilation blocker are **resolved in the current local source**. The test project excludes nested `obj`/`bin` C# inputs; `SfDialogTest.cs`, `SharedTooltip.razor`, `Crosshair.razor`, and `Events.razor` consume the existing `IList<T>` contracts. This does not change the Batch 2 decision to leave those fixes out of that batch.
- The [dated development record](../.github/DEVELOPMENT.md#known-analyzer--trim--aot-findings) reports a non-incremental library build on .NET 8/9/10 with **0 warnings, 0 errors**, and bUnit on **.NET 8: 2,735 passed, 0 failed, 0 skipped**, after the approved adaptor constraint change. These are existing local results, not checks rerun by this docs-only synchronization, remote CI results, or released-package certification.
- Preserve the 39-type internalization record and the approval fence in §6. Current source still declares the listed fenced types public. Source-path clarification: `Size` is in `Chart/ChartEvent/ChartEventInterface.cs`, not `ChartModels.cs`; `Rect` remains in `Common/Renderer/SvgRendererOptions.cs`. Paths are relative to `src/Components/Charts/`.
- The historical JS/source-generation pre-flight rules were conservative batch constraints, not universal serializer requirements. Future boundary changes still need actual API/interop/source-generation review and approval; this synchronization neither changes accessibility nor authorizes a redesign.
- Outstanding evidence: released API/package compatibility, clean package-consumer interop/serialization checks, supported browser/render-mode execution, and published trim/WASM-AOT validation. The historical zero-error builds and current local tests do not establish these gates.

---

## 1. Problem

The namespace `Syncfusion.Blazor.Toolkit.Charts.Internal` contained many `public` types. Despite the `Internal` segment in the namespace name, `public` accessibility means these types are part of the shipped **public API surface** of the assembly. This is an accidental API leak:

- Consumers can take binary/source dependencies on types that were never intended to be supported API.
- Any future change to those "internal-by-name" types becomes a potential breaking change.
- The surface is larger than the documented, supported component API.

The remediation goal is to make these types `internal` (their true intent) wherever that can be done **without** breaking the genuinely public API or JS interop.

Primary file: `src/Components/Charts/Chart/Models/ChartModels.cs`
Related fenced file: `src/Components/Charts/Common/Models/EventModels.cs` (`BorderModel`, `VisibleRangeModel` remain public).

---

## 2. Goals / Non-goals

### Goals
- Flip `public class` → `internal class` for Charts DTOs that are only consumed inside the assembly.
- Preserve all attributes, XML docs, and members exactly (visibility keyword change only).
- Keep the main library (`src/Syncfusion.Blazor.Toolkit.slnx`) building with **0 errors** after every batch.
- Record a permanent public fence and the rationale behind it.

### Non-goals
- No redesign of genuinely public API (event args, `[JSInvokable]` signatures, `ChartData`).
- No namespace moves, renames, or drive-by refactors.
- No `[EditorBrowsable]`, no new suppressions.
- No fixing of pre-existing BUnit `CS0266` (`IList`→`List`) errors — unrelated to this program.
- No reversion of any type internalized in Batches 1, 2A, 2B, 2C, or 2D.

---

## 3. Method

### Constraint model
- **CS0050–CS0060 accessibility rules:** a `public`/`protected` member of a `public` type cannot expose a less-accessible (`internal`) type. This is the primary gate.
- **`[JSInvokable]`:** parameters and return types of JSInvokable methods are reachable from JS and must remain public.
- **System.Text.Json source-gen:** types registered on `ChartInteropJsonContext` (or any `JsonSerializerContext`) form a public serialization contract and must stay public.
- **`InternalsVisibleTo("Syncfusion.Blazor.Toolkit.BUnitTest")`:** BUnit can see internal types, so internalizing does not break those tests.
- **Internal derived from public base is OK.** A public base type may have internal derived types.
- **Public API must not expose internal types** (the inverse of the above — the whole point of the exercise).

### Per-type pre-flight checklist (run before every flip)
1. **Public surface** — Does any `public` or `protected` member on a **public** type use the candidate as a property/field type, method parameter, return type, or interface member? If yes → BLOCK (or move the container too).
2. **`[JSInvokable]`** — Is the candidate a parameter or return type of any `[JSInvokable]` method? If yes → BLOCK.
3. **STJ source-gen** — Is the candidate registered on `ChartInteropJsonContext` or another `JsonSerializerContext`? If yes → BLOCK.
4. **Inheritance / containment** — Base/derived types and container/contained types must move **together** when a public exposure would otherwise result. Treat such clusters as all-or-nothing.
5. **Verdict** — PASS (internalize) or BLOCK (leave public, record the exact blocker: file + member).

### Build gate
- After each batch: `dotnet build src/Syncfusion.Blazor.Toolkit.slnx` must report **0 errors**.
- BUnit may be built opportunistically; the only acceptable remaining errors are the **5 pre-existing `CS0266`** cases, which are out of scope and must not be "fixed" here.

### Incremental batching
- Small, independently buildable batches. Batch 1 (prior work) internalized the Chart **renderers**. Batch 2 (this program) targets the **DTOs** in `ChartModels.cs`.

### Key lesson (established in Batch 2D)
Source comments such as *"Need to pass all these members to JS so they are declared as public"* are **member-level**, not type-level. An `internal class` with `public` members serializes correctly through:
- reflection-based `InvokeVoidAsync` / `InvokeAsync<T>` interop, and
- runtime `JsonSerializer.Serialize/Deserialize`.

Such comments must **not** be treated as a type-level fence unless there is a real public API, `[JSInvokable]`, or source-gen contract. Verify the actual boundary before blocking.

---

## 4. Results by batch

**Totals: 39 types internalized; 1 blocked (`IRect`, Batch 2C); 0 blocked in 2D.**
Breakdown: 13 + 7 + 2 + 7 + 10 = 39.

### Batch 2A — math / point helpers (13 → `internal`)

| Type | Verdict |
|---|---|
| `ZoomAxisRange` | PASS |
| `ConnectPoints` | PASS |
| `FromTo` | PASS |
| `RectPosition` | PASS |
| `AtrPoints` | PASS |
| `StackValues` | PASS |
| `ControlPoints` | PASS |
| `SlopeIntercept` | PASS |
| `Mean` | PASS |
| `BollingerPoints` | PASS |
| `FinancialPoint` | PASS |
| `BoxPoint` | PASS |
| `BubblePoint` | PASS |

Build: main slnx 0 errors.

### Batch 2B-1 — theme / misc (7 → `internal`)

| Type | Verdict | Note |
|---|---|---|
| `Browser` | PASS | |
| `VisibleLabels` | PASS | |
| `LabelTemplateOptions` | PASS | |
| `LegendOption` | PASS | |
| `ChartFontOptions` | PASS | |
| `ScrollbarThemeStyle` | PASS | |
| `ChartThemeStyle` | PASS | High fan-out, but all consumers internal → still safe |

Build: main slnx 0 errors.

### Batch 2B-2 — geometry pair (2 → `internal`)

| Type | Verdict | Note |
|---|---|---|
| `DomRect` | PASS | Internalized as a pair |
| `Thickness` | PASS | Internalized as a pair |

Build: main slnx 0 errors.

### Batch 2C — `I*` geometry / settings (7 → `internal`; 1 BLOCK)

| Type | Verdict | Note |
|---|---|---|
| `IChartZoomSettings` | PASS | Lowest risk; only used in a private method of internal `JsInteropState` |
| `IChartEventBorder` | PASS | |
| `IMarkerSettingModel` | PASS | |
| `IChartInternalLocation` | PASS | |
| `IChartPoint` | PASS | Tier-B family, all-or-nothing within the safe subset |
| `IFinancialPoint` | PASS | `: IChartPoint` |
| `IBubblePoint` | PASS | `: IChartPoint` |
| **`IRect`** | **BLOCK** | Public `IAxis` exposes `IRect` via `public IRect Rect` and `public IRect UpdatedRect`. Leave public. |

Notes:
- The `IChartPoint` family was treated all-or-nothing among the safe subset (base + derived move together).
- `IChartPoint` carries `public IList<IRect> Regions`. With `IChartPoint` internal and `IRect` public, that is a **public member on an internal type** — which is legal — so `IChartPoint` could still be internalized while `IRect` stays public.

Build: main slnx 0 errors.

### Batch 2D — Animation / Dynamic* (10 → `internal`; 0 BLOCK)

| Type | Verdict | Group |
|---|---|---|
| `AnimationOptions` | PASS | — |
| `DynamicPathAnimationOptions` | PASS | — |
| `DynamicRectAnimationOptions` | PASS | References public `Rect` outbound only (internal→public legal); `Rect` untouched |
| `DynamicLastLabelOptions` | PASS | — |
| `DynamicTextAnimationOptions` | PASS | Inheritance pair (base) |
| `DynamicAccTextAnimationOptions` | PASS | Inheritance pair (`: DynamicTextAnimationOptions`) |
| `MarkerAnimationInfo` | PASS | Containment group (member of `InitialAnimationInfo`) |
| `ErrorBarAnimationInfo` | PASS | Containment group |
| `DataLabelAnimatioInfo` | PASS | Containment group — spelling kept as in source (`Animatio`, not `Animation`) |
| `InitialAnimationInfo` | PASS | Containment group (container of the three `*Info` types) |

Notes:
- Interop path is generic `InvokeVoidAsync` / reflection — **not** an STJ source-gen registration. No `ChartInteropJsonContext` entry for any of these.
- The *"public for JS"* comments on several of these types are false type-level blockers — members stay `public`, the class is `internal`.
- `DynamicRectAnimationOptions.PreviousRect` / `CurrentRect` are of public type `Rect`; this outbound reference is legal and `Rect` was not touched.

Build: main slnx 0 errors; BUnit still only 5× pre-existing `CS0266`.

---

## 5. Key lessons

1. **JS comment = member-level, not type-level.** *"Need to pass all these members to JS so they are declared as public"* means the **members** must be public for the serializer. An `internal` class with `public` members works for reflection-based `InvokeVoidAsync` and runtime `JsonSerializer`. Do not treat these comments as a type-level fence.
2. **Inheritance pairs move together.** `DynamicTextAnimationOptions` + `DynamicAccTextAnimationOptions` were internalized as a unit so no public base/internal-derived or exposure mismatch occurs.
3. **Containment groups move together.** `InitialAnimationInfo` holds public properties of `MarkerAnimationInfo`, `ErrorBarAnimationInfo`, and `DataLabelAnimatioInfo`; all four were internalized together to avoid a public-member-exposes-internal-type violation.
4. **`internal` class + `public` members is fine** for both `InvokeVoidAsync` reflection serialization and runtime STJ. Source-gen (`JsonSerializerContext`) is the only STJ path that would force a type to stay public — none of the internalized types were registered there.
5. **Outbound references are not blockers.** An `internal` type may freely reference a `public` type (e.g. `DynamicRectAnimationOptions` → `Rect`). Only the reverse (public member exposing an internal type) is a CS005x violation.
6. **A public container blocks its referenced type.** `IRect` is blocked solely because the public `IAxis` exposes it via public properties. The blocker is the container's public API, not `IRect` itself.
7. **High fan-out does not imply public.** `ChartThemeStyle` has many consumers but all are internal, so it was safely internalized.

---

## 6. Permanent public fence

These types **must stay public** and must **not** be internalized without an explicit API redesign. Each has a real public-API, event, interop, or geometry-contract reason.

| Type | File / area | Why it must stay public |
|---|---|---|
| `IRect` | `ChartModels.cs` | Exposed by public `IAxis` via `public IRect Rect` and `public IRect UpdatedRect`. Internalizing would be a CS0053 public-member-exposes-internal violation unless `IAxis` is redesigned. |
| `IAxis` | `ChartModels.cs` | Public axis contract reachable from the public surface; exposes `IRect`. |
| `Size` | `ChartModels.cs` | Public geometry contract. |
| `Rect` | `SvgRendererOptions.cs` (Charts.Internal) | Public geometry type referenced outbound by internal animation DTOs; part of the public surface. |
| `ClientXY` | Charts.Internal | Pointer-event payload reachable from public event flow. |
| `Touches` | Charts.Internal | Pointer/touch-event payload reachable from public event flow. |
| `ChartInternalMouseEventArgs` | Charts.Internal | Mouse event args on the public event path. |
| `ChartMouseWheelArgs` | Charts.Internal | Mouse-wheel event args on the public event path. |
| `IChartTooltipInfo` | Charts.Internal | Tooltip info exposed through public tooltip/event API. |
| `IPointInfo` | Charts.Internal | Point info exposed through public event/tooltip API. |
| `ITooltipRenderEventArgs` | Charts.Internal | Public tooltip render event args. |
| `ISharedTooltipRenderEventArgs` | Charts.Internal | Public shared-tooltip render event args. |
| `IZoomingStates` | Charts.Internal | Zoom state exposed through public zoom event API. |
| `IScrollEventsArgs` | Charts.Internal | Scrollbar event args on the public event path. |
| `SymbolLocation` | Charts.Internal | Public location/marker contract. |
| `BorderModel` | `EventModels.cs` | Public event model. |
| `VisibleRangeModel` | `EventModels.cs` | Public event model. |

**Grouping rationale summary:**
- **Geometry contracts** (`IRect`, `Rect`, `Size`): referenced across the public surface and/or by public `IAxis`.
- **Mouse / pointer / scroll / zoom / tooltip event DTOs** (`ClientXY`, `Touches`, `ChartInternalMouseEventArgs`, `ChartMouseWheelArgs`, `IChartTooltipInfo`, `IPointInfo`, `ITooltipRenderEventArgs`, `ISharedTooltipRenderEventArgs`, `IZoomingStates`, `IScrollEventsArgs`): reachable from the public event/callback API.
- **EventModels** (`BorderModel`, `VisibleRangeModel`): public event models in `EventModels.cs`.

**Do not revert** any type internalized in Batch 1, 2A, 2B-1, 2B-2, 2C, or 2D.

---

## 7. Residual surface

Types that may still be public and are either fenced or deliberately deferred:

- **Fenced (keep public):** all types in Section 6.
- **Deferred / unaudited:** `PointData` / `ChartData` and any other residual public `ChartModels.cs` / `Charts.Internal` types outside the clusters audited in Batches 2A–2D. These were not flipped and must not be flipped without their own pre-flight and a product decision (see Section 8).

---

## 8. Historical follow-ups (optional, not scheduled in Batch 2)

The following were unimplemented and unscheduled **at the batch handoff**, not necessarily in the current tree. In particular, item 6's compiler errors were fixed by later work as recorded in the 2026-10-03 status above; that fix is not prohibited or reversed by this historical scope record.

1. **`PointData` / `ChartData` strategy.** `ChartData` has `protected` coupling on the public surface. Internalizing `PointData`/`ChartData` requires an explicit API strategy and product sign-off. Blocked pending decision.
2. **`[EditorBrowsable(EditorBrowsableState.Never)]` hygiene** on the fenced public `Charts.Internal` types (Section 6) to hide them from IntelliSense while keeping them technically public. IntelliSense-only cleanup, no behavioral change. Not applied in Batch 2.
3. **Redundant `GlobalSuppressions.cs` CA entries** (around the L298–380 area) that target now-internal animation types. These are dead but harmless; cleanup deferred.
4. **`[JSInvokable]` method accessibility redesign** — out of scope; would require interop contract review.
5. **BL0007** — out of scope.
6. **BUnit `CS0266` (`IList`→`List`) fixes** — pre-existing, unrelated; must not be fixed as part of this program.
7. **Samples redesign, namespace moves, drive-by refactors** — explicitly out of scope.

---

## 9. Historical Batch 2 scope rules

These rules record the batch's boundaries. The public-API approval requirement remains; the visibility-only and deferred-test restrictions apply to that batch, not all later approved maintenance. In particular, rule 6 does not require reverting the now-resolved bUnit compiler fixes.

1. **Do not revert** any 2A/2B/2C/2D internalized type back to `public`.
2. **Do not internalize** any type in the Section 6 fence without a full redesign and product sign-off.
3. **Do not treat "public for JS" comments as type-level fences.** Verify for a real public API, `[JSInvokable]`, or source-gen (`JsonSerializerContext`) contract first. Member-level public is sufficient for reflection/STJ runtime serialization on an internal type.
4. **Move inheritance pairs and containment groups together.** Never leave a public type exposing an internal base/member.
5. **Visibility keyword only.** No attribute additions, renames, namespace moves, or member changes when internalizing.
6. **Build gate.** `dotnet build src/Syncfusion.Blazor.Toolkit.slnx` must stay at 0 errors. Do not fix pre-existing BUnit `CS0266`.
7. **`PointData`/`ChartData` are off-limits** until a documented strategy exists.
