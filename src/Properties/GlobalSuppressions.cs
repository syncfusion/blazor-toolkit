// Copyright © 2001-2026 Syncfusion Inc. All rights reserved.
// Licensed under the MIT license. See LICENSE in the repository root for full license information.
//
// GlobalSuppressions.cs
//
// Centrally-documented analyzer suppressions. Every suppression in this
// file is a known, accepted risk. The maintainer team RE-evaluates the
// file at every major release. See DEVELOPMENT.md §Known analyzer /
// trim / AOT findings for the per-suppression rationale and audit
// table.
//
// Effect model
// ------------
// The csproj's <WarningsAsErrors> configuration only escalates the
// security-relevant subset of CA rules (CA21xx / CA23xx / CA53xx /
// CA54xx). Non-security CA rules are reported at Warning level and
// are either suppressed here (with justification) or accepted as
// open issues tracked in the GitHub issues queue.
//
// Re-evaluation rule: at every major release, all entries in this
// file MUST be re-attested by the maintainers. Use a Tracking issue
// labelled `suppressions/<checkid>` to capture each audit decision.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Design",
    "CA1017:Mark assemblies with ComVisible",
    Justification = "COM exposure is not a consumer surface for NuGet distribution. Audited 2026-09-06. See DEVELOPMENT.md §Known analyzer / trim / AOT findings.")]

[assembly: SuppressMessage(
    "Design",
    "CA1014:Mark assemblies with CLSCompliantAttribute",
    Justification = "Public surface is consumed from non-CLS-compliant languages downstream. Audited 2026-09-06. See DEVELOPMENT.md §Known analyzer / trim / AOT findings.")]

[assembly: SuppressMessage(
    "Globalization",
    "CA1305:Specify IFormatProvider",
    Justification = "Error/log message formatting does not interpolate user-controlled values. Audited 2026-09-06. See DEVELOPMENT.md §Known analyzer / trim / AOT findings.")]

[assembly: SuppressMessage(
    "Naming",
    "CA1716:Identifiers should not conflict with keywords",
    Justification = "The Data namespace mirrors .NET design-time naming conventions (e.g. Dynamic, Value) as required by the public API style guide. Restricted to that namespace. Audited 2026-09-06.")]

// ---------------------------------------------------------------------------
// CA2227 — Collection properties on serialization DTOs / Blazor parameters.
//
// The following collection properties MUST keep their public setters. They are
// data-transfer objects that System.Text.Json deserializes into (Query.Clone()
// round-trips DataManagerRequest through JsonSerializer.Deserialize) and that
// the fluent Query builder reassigns (e.g. `Queries.Select = Queries.Select ?? []`).
// Removing the setter would break serialization round-tripping and the builder.
// The declared type has already been narrowed from List<T> to IList<T> to satisfy
// CA1002. Audited 2026-10-01. See DEVELOPMENT.md §Known analyzer / trim / AOT findings.
// ---------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor sets [Parameter] values through the public setter at runtime; removing it breaks parameter binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManager.Headers")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Serialization DTO: deserialized by System.Text.Json and reassigned by the fluent Query builder. Setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Group")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Serialization DTO: deserialized by System.Text.Json and reassigned by the fluent Query builder. Setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Select")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Serialization DTO: deserialized by System.Text.Json and reassigned by the fluent Query builder. Setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Expand")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Serialization DTO: deserialized by System.Text.Json and reassigned by the fluent Query builder. Setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Sorted")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Serialization DTO: deserialized by System.Text.Json and reassigned by the fluent Query builder. Setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Search")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Serialization DTO: deserialized by System.Text.Json and reassigned by the fluent Query builder. Setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Where")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Serialization DTO: deserialized by System.Text.Json and reassigned by the fluent Query builder. Setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Aggregates")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Serialization DTO: deserialized by System.Text.Json and reassigned by the fluent Query builder. Setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Params")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Serialization DTO: deserialized by System.Text.Json and reassigned by the fluent Query builder. Setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Distinct")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Serialization DTO: deserialized by System.Text.Json and reassigned by the fluent Query builder. Setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.GroupByFormatter")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Serialization DTO: deserialized by System.Text.Json. Setter is required for round-tripping. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.SearchFilter.Fields")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Serialization DTO: deserialized by System.Text.Json and populated via object initializers. Setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.WhereFilter.Predicates")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Result DTO: assigned by the adaptor pipeline after aggregation. Setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataResult`1.Aggregates")]
// ---------------------------------------------------------------------------
// CA2227 — Collection properties on Blazor [Parameter] inputs, attribute-splat
// dictionaries, serialization/event-args DTOs, and internal renderer models.
//
// Each property below MUST keep its public setter: Blazor assigns [Parameter]
// and attribute dictionaries through the setter during parameter binding, the
// event-args DTOs are round-tripped by System.Text.Json, and the internal
// renderer models are reassigned by the chart layout pipeline. The declared
// collection type has already been narrowed from List<T> to IList<T> to satisfy
// CA1002. Audited 2026-10-01. See DEVELOPMENT.md §Known analyzer / trim / AOT findings.
// ---------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Buttons.Button.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Buttons.SfButton.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Buttons.SfButtonGroup.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.CalendarBase`1.KeyConfigs")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.DatePickerModel.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.DatePickerModel.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfCalendar`1.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.BaseHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.BaseInputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.MaskPlaceholderDictionary")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.BaseHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.BaseInputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.KeyConfigs")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.MaskPlaceholderDictionary")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ChartAxis.MultiLevelLabels")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ChartAxis.StripLines")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ChartMultiLevelLabel.Categories")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ChartSeries.Segments")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ChartSeries.Trendlines")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ChartZoomSettings.ToolbarItems")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.CrosshairMoveEventArgs.AxisInfo")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.ChartInternalMouseEventArgs.Touches")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.ColumnBaseRenderer.ColumnPathOptions")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.DataLabelAnimatioInfo.TemplateId")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.IAxis.Labels")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.IChartPoint.Regions")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.IChartPoint.SymbolLocations")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.IRequireAxis.XData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.IRequireAxis.YData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.ISharedTooltipRenderEventArgs.Data")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.ISharedTooltipRenderEventArgs.Text")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.InitialAnimationInfo.PointHeight")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.InitialAnimationInfo.PointIndex")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.InitialAnimationInfo.PointWidth")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.InitialAnimationInfo.PointX")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.InitialAnimationInfo.PointY")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.LegendBase.ColumnHeights")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.LegendBase.PageHeights")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.LegendBase.PageXCollections")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.LegendBase.PagingOptions")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.LegendBase.PagingRegions")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.LegendBase.RowHeights")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.MarkerAnimationInfo.LowPointIndex")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.MarkerAnimationInfo.LowPointX")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.MarkerAnimationInfo.LowPointY")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.MarkerAnimationInfo.PointIndex")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.MarkerAnimationInfo.PointX")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.MarkerAnimationInfo.PointY")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.PatternOptions.ShapeOptions")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.SvgPattern.ShapeOptions")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.SvgSelectionRectCollection.SelectedRectangles")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.SvgText.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.TextOptions.TextCollection")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Internal rendering-pipeline model: the collection is reassigned by the chart layout/renderer pipeline. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.TextOptions.TextLocationCollection")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.TooltipData.Attributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Point.Regions")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Point.SymbolLocations")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.SelectionCompleteEventArgs.SelectedDataValues")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.SelectionStyleComponent.GivenPattern")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.SharedTooltipRenderEventArgs.Text")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ZoomingEventArgs.AxisCollection")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.BaseComponent.DataContainer")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.BaseComponent.DataHashTable")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.CRUDModel`1.Added")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.CRUDModel`1.Changed")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.CRUDModel`1.Deleted")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.Query.Distincts")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.Query.Expands")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.Query.Params")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.ActionCompleteEventArgs.FileData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.BeforeRemoveEventArgs.FilesData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.BeforeUploadEventArgs.FilesData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.ClearingEventArgs.FilesData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.NumericTextBoxModel`1.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.NumericTextBoxModel`1.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.RemovingEventArgs.FilesData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SelectedEventArgs.FilesData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SelectedEventArgs.ModifiedFilesData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfInputBase`1.BaseHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfInputBase`1.BaseInputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfInputBase`1.ContainerAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfInputBase`1.ContainerHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfInputBase`1.InputHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfInputBase`1.ListOfButtons")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfNumericTextBox`1.BaseHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfNumericTextBox`1.BaseInputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfNumericTextBox`1.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfNumericTextBox`1.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfSelectionBase`1.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextArea.BaseHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextArea.BaseInputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextArea.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextArea.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextBox.BaseHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextBox.BaseInputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextBox.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextBox.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfUploader.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfUploader.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.UploadChangeEventArgs.Files")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor [Parameter] / event-args DTO: the value is assigned by the parent component or by System.Text.Json during event round-tripping. Type narrowed from List<T> to IList<T> for CA1002; setter is required. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.UploaderModel.Files")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Popups.DialogButton.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Popups.SfDialog.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "Blazor splats this attribute dictionary onto the rendered element and sets it through the public setter during parameter binding; removing the setter breaks attribute binding. Audited 2026-10-01.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Popups.SfTooltip.HtmlAttributes")]

// ---------------------------------------------------------------------------
// CA1002 — Concrete List<T> parameters on [JSInvokable] interop methods.
//
// These methods are invoked from JavaScript and their arguments are
// deserialized by System.Text.Json, which materializes a concrete List<T>.
// The parameter type cannot be narrowed to IList<T> without breaking JS interop
// deserialization. Audited 2026-10-01. See DEVELOPMENT.md §Known analyzer / trim / AOT findings.
// ---------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Design",
    "CA1002:Do not expose generic lists",
    Justification = "Parameter is deserialized from JavaScript via [JSInvokable]; System.Text.Json requires a concrete List<T>. Audited 2026-10-01.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.ChartPanAsync(System.Collections.Generic.List{System.String},System.Collections.Generic.List{System.Double},System.Collections.Generic.List{System.Double})")]

[assembly: SuppressMessage(
    "Design",
    "CA1002:Do not expose generic lists",
    Justification = "Parameter is deserialized from JavaScript via [JSInvokable]; System.Text.Json requires a concrete List<T>. Audited 2026-10-01.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.OnSelectionChange(System.Collections.Generic.List{Syncfusion.Blazor.Toolkit.Charts.PointXY})")]

[assembly: SuppressMessage(
    "Design",
    "CA1002:Do not expose generic lists",
    Justification = "Parameter is deserialized from JavaScript via [JSInvokable]; System.Text.Json requires a concrete List<T>. Audited 2026-10-01.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.SetTooltipTemplateElementSizeAsync(System.Double,System.Double,System.Collections.Generic.List{Syncfusion.Blazor.Toolkit.Charts.Internal.IChartTooltipInfo})")]

[assembly: SuppressMessage(
    "Design",
    "CA1002:Do not expose generic lists",
    Justification = "Parameter is deserialized from JavaScript via [JSInvokable]; System.Text.Json requires a concrete List<T>. Audited 2026-10-01.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Inputs.SfUploader.GetFileDetailsAsync(System.Collections.Generic.List{Syncfusion.Blazor.Toolkit.Inputs.FileInfo})")]

[assembly: SuppressMessage(
    "Design",
    "CA1002:Do not expose generic lists",
    Justification = "Parameter is deserialized from JavaScript via [JSInvokable]; System.Text.Json requires a concrete List<T>. Audited 2026-10-01.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Inputs.SfUploader.CreateFileListAsync(System.Collections.Generic.List{Syncfusion.Blazor.Toolkit.Inputs.Internal.UploadFileDetails},System.Boolean)")]

[assembly: SuppressMessage(
    "Design",
    "CA1002:Do not expose generic lists",
    Justification = "Parameter is deserialized from JavaScript via [JSInvokable]; System.Text.Json requires a concrete List<T>. Audited 2026-10-01.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Inputs.SfUploader.UpdateServerFileDataAsync(System.Collections.Generic.List{Syncfusion.Blazor.Toolkit.Inputs.Internal.UploadFileDetails},System.Boolean)")]

// ---------------------------------------------------------------------------
// CA1711: Identifiers should not have incorrect suffix
// ---------------------------------------------------------------------------
// The 'EventArgs' suffix is the deliberate, idiomatic naming convention for
// Blazor component event-argument DTOs (consistent with the broader Syncfusion
// Blazor product line and System.*EventArgs). Renaming these public types would
// break every consuming application's event handlers for no functional benefit.
// The single 'Collection' type is an internal rendering-pipeline collection whose
// name accurately describes its role. Suppressed centrally. Audited 2026-10-01.
// ---------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.BlurEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.ChangeEventArgs`1")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.ChangedEventArgs`1")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.ClearedEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.DeSelectedEventArgs`1")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.FocusEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.ItemEventArgs`1")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.NavigatedEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.PopupEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.RenderDayCellEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.SelectedEventArgs`1")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.AxisLabelClickEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.AxisLabelRenderEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.AxisMultiLabelRenderEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.AxisRangeCalculatedEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.BaseEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.ChartMouseEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.CrosshairMoveEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.DataEditingEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.Internal.ChartInternalMouseEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.Internal.ISharedTooltipRenderEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.Internal.ITooltipRenderEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "Internal rendering-pipeline collection type; the 'Collection' suffix accurately describes the type and it is not part of the supported public surface (EditorBrowsable(Never)). Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.Internal.SvgSelectionRectCollection")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.LegendClickEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.LegendRenderEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.LoadedEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.MultiLevelLabelClickEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.PointEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.PointRenderEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.ResizeEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.ScrollEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.SelectionCompleteEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.SeriesRenderEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.SharedTooltipRenderEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.TextRenderEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.TooltipRenderEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.ZoomingEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.ActionCompleteEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.BeforeRemoveEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.BeforeUploadEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.CancelEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.ChangeEventArgs`1")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.ClearingEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.FailureEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.FileListRenderingEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.NumericBlurEventArgs`1")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.NumericFocusEventArgs`1")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.PauseResumeEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.ProgressEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.RemovingEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.ResponseEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.SelectedEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.SuccessEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.UploadChangeEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.UploadingEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Popups.TooltipEventArgs")]
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The 'EventArgs' suffix is the idiomatic, well-established naming convention for Blazor component event-argument types; renaming would break consumer event handlers with no functional benefit. Audited 2026-10-01.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Spinner.SpinnerEventArgs")]
