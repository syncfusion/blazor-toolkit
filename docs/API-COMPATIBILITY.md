# API compatibility and 2.0 migration contract

**Reviewed 2026-10-06 — source change, not a released 2.0 package.** The requesting user approved the exact existing major-version exceptions, both getter-only collection ownership changes and the uncommitted source version edit to `2.0.0`. Commit/signing/publication remain owner-controlled. R03's version-commit prerequisite was explicitly replaced for this work by approval/review of the uncommitted version diff; a clean candidate commit is still required for release gates.

## Fixed baseline and enforcement

- Released baseline: **NuGet Syncfusion.Blazor.Toolkit 1.0.2**, SHA-256 `06934c512f9c42497fb37464f36d1b0cd7b987624bf2dab01c77b8a80865705d`.
- Embedded commit differs from the local release tag; source/provenance reconciliation remains R18 work. The installed package, not an assumed equivalent tag, is the API baseline.
- Exact diagnostic inventory: **194 named diagnostic/target pairs** approved for 2.0, not wildcard exclusions. The original 195th diagnostic, assembly version regression CP0003, is **not suppressed**; the authorized version change resolves it.
- `tools/api-compat/gate.py` verifies baseline hash/signature/identity, pins APICompat **10.0.401**, requires all three TFMs, runs the released comparison with the exact exceptions, then compares the current public/protected surface and representative contracts with independently frozen reviewed manifests.
- The second layer prevents an existing exception from hiding another change to that same member. It records implemented interfaces, generic type/method constraints, signatures, defaults, public/protected fields, nullable/contract attributes, Razor parameters, callbacks, assembly identity and representative JSON fixtures. It excludes only named compiler/debug implementation attributes; it does not disable nullable or contract analysis.
- All manifest differences, including additions, require review. There is deliberately **no update/rebaseline option** in the gate. Review exact diffs and migrations before manually replacing snapshots; never regenerate exceptions to make CI green.
- Remote execution and required-check repository settings are not verified by local success. CI logs retain for 14 days; release owners must retain release evidence separately.

## Approved major changes and migration

| Family | Approved scope | Consumer guidance |
|---|---|---|
| Type visibility | 84 exact formerly public Chart types now internal: renderer foundation 4, axis/layout 16, series 11, visual/template 11, stripline/trendline 7, legend 6, interaction controllers 2, SVG machinery 7, geometry/margin 3, animation payloads 10, compact point/settings payloads 7 | No drop-in renderer inheritance/registration or payload-construction replacement. Use public components/settings/events/templates for feature-level customization; application-owned DTOs/SVG may require redesign. `Internal` in a namespace was not an exemption from compatibility accounting. |
| Collection signatures | 45 getters and 54 methods: List to IList/interface contracts; exact symbols in inventory | Rebuild dependants. Existing List arguments often remain assignable, but binary signatures differ. Read results as IList/appropriate interface. Do not cast to List or promise `ToList()` is equivalent—copying changes identity and mutation behavior. |
| Getter-only ownership | Public `UploadedFiles.Files` and protected `ChartData.CurrentPoints` | Replacement assignment no longer supported. Clear/Add retains existing identity; this differs from replacing or aliasing a List. This specific ownership break was separately approved. |
| Parent metadata | `DatePickerParent`, `DateTimePickerParent`, `TimePickerParent`, `TextBoxParent`: protected dynamic to object | Named getters/setters still exist; dynamic and object share CLR representation but derived C# source may need a known-type adapter/cast or explicit dynamic local. No trimming or arbitrary InPlaceEditor integration guarantee. |
| Static helpers | `ChartHelper`, `DataVizCommonHelper` construction/subclassing removed | ChartHelper has no public method replacement for instance use. DataVizCommonHelper's public static `StringToNumber` remains; instance-typed/generic/subclass uses do not. |
| Generic constraint | `DataAdaptor<T> where T : notnull` | Propagate `where T : notnull` on generic subclasses. Nullable/unconstrained consumers can produce CS8714, an error under warnings-as-errors. This is a compiler constraint, not a new CLR runtime constraint. |

All exact target rows link to the frozen inventory, not an invented historical “39 removals” list. The user cited the source-warning cleanup as motivation; **zero warnings does not prove compatibility** and did not remove the need for these explicit major-version decisions.

The fresh build also exposed an **unapproved** namespace difference for `SfDataManager`. Explicit `@namespace Syncfusion.Blazor.Toolkit.Data` now preserves its released identity; no new exception was added. The remaining production method edit corrects the `GetButtonItems` XML example to IList without altering its approved signature.

## Razor and wire contract scope

- Metadata comparison: 1.0.2 had 1,119 declared public component parameters; current has 1,113. Six removed declarations belong to internalized renderers. Among retained parameters, seven collection types and ten nullable annotations differ. The same three `EditorRequired` parameters (`ChartSeries.XName`, `YName`, `ChartSorting.PropertyName`) exist in both; no new required-parameter promise is inferred.
- All **86 declared JS-invokable signatures** matched, including explicit identifiers, staticness and ordered argument types. This is metadata verification, not every real browser dispatch path.
- `tools/ApiContractProbe/Fixtures/RazorConsumer.razor` compiles the seven one-way collection settings plus NumericTextBox value and Dialog visibility bindings with warnings-as-errors. It does not invent nonexistent `*Changed` events for those collection properties, nor prove runtime behavior of every configuration.
- Generated Chart geometry and Dialog dictionary metadata is mandatory in candidate mode. Reflection fallback is allowed only with explicit `--historical` for the released baseline. Removing candidate context/type metadata fails the probe.
- Representative Chart compact-point serialization, uploader selection/null/empty arrays, zoom payloads, data requests, actual HTTP POST/PATCH null/default/string-enum handling and generated geometry/Dialog state fixtures match baseline. Outgoing-only DTOs that reject deserialization are recorded as **unsupported**, never counted as successful round trips.
- The probe runs all assembly TFMs on the pinned **net10 runtime**. That is not execution on three runtimes; R14–R16 cover actual host/runtime/publish/package behavior.

Use matching-version assemblies/scripts/styles. **Arbitrary 1.x persisted application state, serialized internal payloads, and internal renderer extensions are not promised upgrade compatibility.** Applications must migrate or clear their own versioned state after reviewing user impact; the toolkit does not silently clear customer data. Dialog's representative dictionary comparison is not a blanket persistence migration. Full callback dispatch, invalid-client input, trim/AOT and host behavior remain separate gates.

At the R03 comparison, twenty of 21 released script files matched source after line-ending normalization. That is historical evidence, not a claim about later R04/R05 edits. NumericTextBox's spinner-button focus/prevent-default behavior differed; the keyboard smoke is not proof of every mouse/touch/hold ordering. No mixed-version asset promise or broad behavioral equivalence follows from byte matching.

### R05 selected-filename behavior correction — 2026-10-06

The user selected the recommended correction: preserve original selected filenames rather than rewriting them to numeric HTML entities when `EnableHtmlSanitizer` validation flags a difference. Callback/file metadata and ordinary Razor templates now receive the original name. Default name/extension/status displays use text independently of that option. The optional validation comparison and invalid status code remain; it is not a guarantee that every markup-looking name is rejected. Validation parsing uses an inert document and does not attach parsed nodes.

Applications must not rely on the old entity-encoded identity or insert callback names into HTML or storage paths. Use ordinary Razor interpolation/text APIs; application templates remain a separate trust boundary. No parameter, callback signature or JSON property shape changed, and no new API exception was added. R05 source evidence is not released-package remediation or private security-owner approval. Existing missing invalid-name localization and Razor invalid-status remapping remain separate quality findings.

## CSS-isolation direction

The user identified a **planned migration to CSS isolation instead of static wwwroot CSS**. R03 does not implement or pre-approve that asset change. Before replacing documented CSS paths, a separate change must validate generated scoped assets, selector/override behavior, packaging, first-chart and coexistence consumers (R16/R20/R24), and migration documentation. Keep current CSS instructions until tested replacement assets exist; neither the API exceptions nor source version edit certifies CSS readiness.

## Running and maintaining the gate

Build the library and `tools/ApiContractProbe/ApiContractProbe.csproj` in one configuration, then run `python3 tools/api-compat/gate.py --assemblies src/bin/Release --probe tools/ApiContractProbe/bin/Release/net10.0/ApiContractProbe.dll --output artifacts/api-compat`. Use `--package <exact-1.0.2.nupkg>` for an offline baseline input; hash/signature validation is still mandatory. The gate does not build or trust stale outputs on your behalf. CI builds inputs nonincrementally before checking them.

Run verifier tests with `python3 -m unittest discover -s tools/api-compat -p test_gate.py`. Frozen manifests are gzip JSON for compact storage; inspect with any gzip/JSON reader. Source paths, timestamps and MVIDs are not part of the contract snapshots. Diagnostic logs and actual inventories are retained on failure.

Evidence and boundaries: A passing source gate establishes this reviewed contract only; package validation, signatures/provenance of the final candidate, CSS changes, security, accessibility and performance remain independent release requirements.
