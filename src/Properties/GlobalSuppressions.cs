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
// Organization
// ------------
// Suppressions are grouped by rule ID and, within each rule, ordered
// by namespace / member target. Every justification uses the format:
//     "YYYY-MM-DD: <reason>. <optional context>."
// where the date is the last audit date for that entry.
//
// Re-evaluation rule: at every major release, all entries in this
// file MUST be re-attested by the maintainers. Use a Tracking issue
// labelled `suppressions/<checkid>` to capture each audit decision.
//
// 2026-10-01 audit: removed CA1017 (Mark assemblies with ComVisible)
// and CA1014 (Mark assemblies with CLSCompliantAttribute) — neither
// rule produces a diagnostic against the current assembly, so the
// suppressions were obsolete and have been deleted.

using System.Diagnostics.CodeAnalysis;


// -------------------------------------------------------------------------
// Assembly-wide globalization / naming suppressions.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Globalization",
    "CA1305:Specify IFormatProvider",
    Justification = "2026-10-01: QueryConverter.Write emits a culture-invariant JavaScript sf.data.Query() expression; locale-specific formatting would corrupt the generated query string.")]

[assembly: SuppressMessage(
    "Naming",
    "CA1716:Identifiers should not conflict with keywords",
    Justification = "2026-10-01: The Data namespace mirrors .NET design-time naming conventions (e.g. Dynamic, Value) as required by the public API style guide; restricted to that namespace.")]


// -------------------------------------------------------------------------
// CA1002 — Do not expose generic lists.
// 
// These [JSInvokable] parameters are deserialized from JavaScript by
// System.Text.Json, which materializes concrete List<T> instances.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Design",
    "CA1002:Do not expose generic lists",
    Justification = "2026-10-01: Parameter is deserialized from JavaScript via [JSInvokable]; System.Text.Json requires a concrete List<T>.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.ChartPanAsync(System.Collections.Generic.List{System.String},System.Collections.Generic.List{System.Double},System.Collections.Generic.List{System.Double})")]

[assembly: SuppressMessage(
    "Design",
    "CA1002:Do not expose generic lists",
    Justification = "2026-10-01: Parameter is deserialized from JavaScript via [JSInvokable]; System.Text.Json requires a concrete List<T>.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.OnSelectionChange(System.Collections.Generic.List{Syncfusion.Blazor.Toolkit.Charts.PointXY})")]

[assembly: SuppressMessage(
    "Design",
    "CA1002:Do not expose generic lists",
    Justification = "2026-10-01: Parameter is deserialized from JavaScript via [JSInvokable]; System.Text.Json requires a concrete List<T>.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.SetTooltipTemplateElementSizeAsync(System.Double,System.Double,System.Collections.Generic.List{Syncfusion.Blazor.Toolkit.Charts.Internal.IChartTooltipInfo})")]

[assembly: SuppressMessage(
    "Design",
    "CA1002:Do not expose generic lists",
    Justification = "2026-10-01: Parameter is deserialized from JavaScript via [JSInvokable]; System.Text.Json requires a concrete List<T>.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Inputs.SfUploader.CreateFileListAsync(System.Collections.Generic.List{Syncfusion.Blazor.Toolkit.Inputs.Internal.UploadFileDetails},System.Boolean)")]

[assembly: SuppressMessage(
    "Design",
    "CA1002:Do not expose generic lists",
    Justification = "2026-10-01: Parameter is deserialized from JavaScript via [JSInvokable]; System.Text.Json requires a concrete List<T>.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Inputs.SfUploader.GetFileDetailsAsync(System.Collections.Generic.List{Syncfusion.Blazor.Toolkit.Inputs.FileInfo})")]

[assembly: SuppressMessage(
    "Design",
    "CA1002:Do not expose generic lists",
    Justification = "2026-10-01: Parameter is deserialized from JavaScript via [JSInvokable]; System.Text.Json requires a concrete List<T>.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Inputs.SfUploader.UpdateServerFileDataAsync(System.Collections.Generic.List{Syncfusion.Blazor.Toolkit.Inputs.Internal.UploadFileDetails},System.Boolean)")]


// -------------------------------------------------------------------------
// CA2227 — Collection properties should be read only.
// 
// These collection properties MUST keep their public setters: they are
// attribute-splat dictionaries, Blazor [Parameter]/event-args DTOs,
// System.Text.Json serialization DTOs, or internal renderer models that
// are reassigned at runtime. Removing the setter breaks binding,
// serialization round-tripping, or the rendering pipeline.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Buttons.Button.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Buttons.SfButton.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Buttons.SfButtonGroup.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.CalendarBase`1.KeyConfigs")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.DatePickerModel.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.DatePickerModel.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfCalendar`1.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.BaseHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.BaseInputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.MaskPlaceholderDictionary")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.BaseHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.BaseInputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.KeyConfigs")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.MaskPlaceholderDictionary")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ChartAxis.MultiLevelLabels")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ChartAxis.StripLines")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ChartMultiLevelLabel.Categories")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ChartSeries.Segments")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ChartSeries.Trendlines")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ChartZoomSettings.ToolbarItems")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.CrosshairMoveEventArgs.AxisInfo")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.ChartInternalMouseEventArgs.Touches")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.DataLabelAnimatioInfo.TemplateId")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.IAxis.Labels")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.IChartPoint.Regions")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.IChartPoint.SymbolLocations")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.ISharedTooltipRenderEventArgs.Data")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.ISharedTooltipRenderEventArgs.Text")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.InitialAnimationInfo.PointHeight")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.InitialAnimationInfo.PointIndex")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.InitialAnimationInfo.PointWidth")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.InitialAnimationInfo.PointX")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.InitialAnimationInfo.PointY")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.MarkerAnimationInfo.LowPointIndex")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.MarkerAnimationInfo.LowPointX")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.MarkerAnimationInfo.LowPointY")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.MarkerAnimationInfo.PointIndex")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.MarkerAnimationInfo.PointX")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.MarkerAnimationInfo.PointY")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.PatternOptions.ShapeOptions")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.SvgPattern.ShapeOptions")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Internal rendering-pipeline model; the chart layout/render stage reassigns this collection, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.SvgSelectionRectCollection.SelectedRectangles")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.SvgText.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.TooltipData.Attributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Point.Regions")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Point.SymbolLocations")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.SelectionCompleteEventArgs.SelectedDataValues")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.SelectionStyleComponent.GivenPattern")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.SharedTooltipRenderEventArgs.Text")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ZoomingEventArgs.AxisCollection")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.BaseComponent.DataContainer")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.BaseComponent.DataHashTable")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.CRUDModel`1.Added")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.CRUDModel`1.Changed")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.CRUDModel`1.Deleted")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Blazor [Parameter]/event-args DTO; the value is assigned by the parent component or the runtime, so the public setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManager.Headers")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Aggregates")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Distinct")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Expand")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Group")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.GroupByFormatter")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Params")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Search")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Select")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Sorted")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManagerRequest.Where")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Result DTO populated by the adaptor pipeline after aggregation; the public setter is required to assign the collection.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataResult`1.Aggregates")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.Query.Distincts")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.Query.Expands")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.Query.Params")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.SearchFilter.Fields")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.WhereFilter.Predicates")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.ActionCompleteEventArgs.FileData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.BeforeRemoveEventArgs.FilesData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.BeforeUploadEventArgs.FilesData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.ClearingEventArgs.FilesData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.NumericTextBoxModel`1.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.NumericTextBoxModel`1.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.RemovingEventArgs.FilesData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SelectedEventArgs.FilesData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SelectedEventArgs.ModifiedFilesData")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfInputBase`1.BaseHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfInputBase`1.BaseInputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfInputBase`1.ContainerAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfInputBase`1.ContainerHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfInputBase`1.InputHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfInputBase`1.ListOfButtons")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfNumericTextBox`1.BaseHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfNumericTextBox`1.BaseInputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfNumericTextBox`1.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfNumericTextBox`1.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfSelectionBase`1.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextArea.BaseHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextArea.BaseInputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextArea.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextArea.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextBox.BaseHtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextBox.BaseInputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextBox.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfTextBox.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfUploader.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.SfUploader.InputAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.UploadChangeEventArgs.Files")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Serialization DTO round-tripped through System.Text.Json; deserialization requires a public setter on the collection property.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.UploaderModel.Files")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Popups.DialogButton.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Popups.SfDialog.HtmlAttributes")]

[assembly: SuppressMessage(
    "Usage",
    "CA2227:Collection properties should be read only",
    Justification = "2026-10-01: Attribute-splat dictionary rendered onto the element; Blazor assigns it through the public setter, so the setter must remain.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Popups.SfTooltip.HtmlAttributes")]


// -------------------------------------------------------------------------
// CA1711 — Identifiers should not have incorrect suffix.
// 
// The 'EventArgs' suffix is the idiomatic Blazor convention for event
// payload types; one internal collection type carries the 'Collection'
// suffix which accurately describes it.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.BlurEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.ChangeEventArgs`1")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.ChangedEventArgs`1")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.ClearedEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.DeSelectedEventArgs`1")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.FocusEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.ItemEventArgs`1")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.NavigatedEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.PopupEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.RenderDayCellEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Calendars.SelectedEventArgs`1")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.AxisLabelClickEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.AxisLabelRenderEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.AxisMultiLabelRenderEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.AxisRangeCalculatedEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.BaseEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.ChartMouseEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.CrosshairMoveEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.DataEditingEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.Internal.ChartInternalMouseEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.Internal.ISharedTooltipRenderEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.Internal.ITooltipRenderEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: Internal rendering-pipeline collection type; the 'Collection' suffix accurately describes the member and is not consumer-facing.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.Internal.SvgSelectionRectCollection")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.LegendClickEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.LegendRenderEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.LoadedEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.MultiLevelLabelClickEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.PointEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.PointRenderEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.ResizeEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.ScrollEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.SelectionCompleteEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.SeriesRenderEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.SharedTooltipRenderEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.TextRenderEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.TooltipRenderEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Charts.ZoomingEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.ActionCompleteEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.BeforeRemoveEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.BeforeUploadEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.CancelEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.ChangeEventArgs`1")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.ClearingEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.FailureEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.FileListRenderingEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.NumericBlurEventArgs`1")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.NumericFocusEventArgs`1")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.PauseResumeEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.ProgressEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.RemovingEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.ResponseEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.SelectedEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.SuccessEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.UploadChangeEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Inputs.UploadingEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Popups.TooltipEventArgs")]

[assembly: SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "2026-10-01: The 'EventArgs' suffix is the idiomatic Blazor convention for component event payload types.",
    Scope = "type",
    Target = "~T:Syncfusion.Blazor.Toolkit.Spinner.SpinnerEventArgs")]


// -------------------------------------------------------------------------
// CA1822 — Mark members as static.
//
// This [JSInvokable] member is invoked from JavaScript against the
// component's DotNetObjectReference instance (chart.js calls
// dotnetref.invokeMethodAsync('OnChartLongPress')). JS interop can only
// dispatch to instance methods on an object reference, so the member
// MUST remain an instance method even though it accesses no instance
// state.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Performance",
    "CA1822:Mark members as static",
    Justification = "2026-10-01: [JSInvokable] member dispatched from JavaScript via DotNetObjectReference.invokeMethodAsync; JS interop requires an instance method.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.OnChartLongPress")]


// -------------------------------------------------------------------------
// CA2213 — Disposable fields should be disposed.
//
// These fields hold Blazor child components (types deriving from
// SfBaseComponent / ChartSubComponent, which implement IAsyncDisposable).
// They are declarative [Parameter] / nested-component references whose
// lifecycle is owned by the Blazor renderer: the child registers itself
// on its parent during OnInitialized (e.g. ChartMargin sets
// Owner._margin = this) and the renderer disposes it when the component
// tree is torn down. Disposing them again from the parent would be a
// double-dispose. Where additional eager cleanup is required the owning
// type already nulls / resets these fields via its own ComponentDispose
// helper (not recognized by the analyzer). The setter / field therefore
// MUST NOT call Dispose on these members.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; disposed via the component tree, not the parent. Field is reset in ComponentDispose.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartDataLabel._border")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; disposed via the component tree, not the parent. Field is reset in ComponentDispose.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartDataLabel._margin")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; disposed via the component tree, not the parent. Field is reset in ComponentDispose.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartDataLabel._font")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; disposed via the component tree, not the parent.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartEmptyPointSettings._border")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; disposed via the component tree, not the parent. Field is reset in ComponentDispose.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartStackLabelSettings._border")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; disposed via the component tree, not the parent. Field is reset in ComponentDispose.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartStackLabelSettings._font")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; disposed via the component tree, not the parent. Field is reset in ComponentDispose.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartStackLabelSettings._margin")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; registered via Owner._margin = this and disposed via the component tree.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.SfChart._margin")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; registered via Chart._zoomSettings = this and disposed via the component tree.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.SfChart._zoomSettings")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; registered via Chart._annotations = this and disposed via the component tree.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.SfChart._annotations")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; registered via Parent._tooltip = this and disposed via the component tree.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.SfChart._tooltip")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; registered via Parent._sorting = this and disposed via the component tree.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.SfChart._sorting")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; registered via Chart._crosshair = this and disposed via the component tree.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.SfChart._crosshair")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; disposed via the component tree, not the parent.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.SfChart._annotationsContainer")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; assigned via UpdateChildProperties and disposed via the component tree.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Inputs.SfUploader._asyncSettings")]

[assembly: SuppressMessage(
    "Usage",
    "CA2213:Disposable fields should be disposed",
    Justification = "2026-10-01: Blazor child component owned by the renderer; assigned via UpdateChildProperties and disposed via the component tree.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Inputs.SfUploader._buttons")]


// -------------------------------------------------------------------------
// CA2000 — Dispose objects before losing scope.
//
// Each flagged `new()` here constructs a chart *model* object whose type
// derives from ComponentBase (SfBaseComponent / ChartSubComponent, which
// implement IAsyncDisposable). The analyzer therefore treats every such
// instance as an owned IDisposable that must be disposed before the local
// goes out of scope. That is a false positive in this code base:
//
//   * These model objects are created programmatically and are NEVER
//     rendered by the Blazor renderer. The only disposable resources
//     SfBaseComponent holds (imported JS modules and the
//     DotNetObjectReference bridge) are acquired exclusively during
//     rendering (OnAfterRenderAsync). A non-rendered instance owns no
//     unmanaged / JS resources, so DisposeAsync would be a no-op.
//   * In every case ownership of the object is transferred out of the
//     method: it is added to a container collection (Elements / Axes),
//     wrapped into an event-args object handed to a user callback, or
//     serialized across JS interop. Disposing it inside the creating
//     method would be incorrect.
//
// The ChartStriplineTooltipSettings entry additionally covers `out`
// locals that merely alias already-rendered, renderer-owned child
// components (resolved from existing collections) — disposing them would
// be a double-dispose. Suppressing at method scope is the correct action.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Reliability",
    "CA2000:Dispose objects before losing scope",
    Justification = "2026-10-01: Default axis models (ChartPrimaryXAxis / ChartPrimaryYAxis / InitAxis) are never rendered and ownership is transferred to the Axes/Elements collections. DisposeAsync would be a no-op and disposing here would be incorrect.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.Internal.ChartAxisRendererContainer.AddRenderer(Syncfusion.Blazor.Toolkit.Charts.Internal.IChartElementRenderer)")]

[assembly: SuppressMessage(
    "Reliability",
    "CA2000:Dispose objects before losing scope",
    Justification = "2026-10-01: ChartSeriesBorder is a non-rendered model object passed by value into SetTrendlineValues; it owns no JS/unmanaged resources so DisposeAsync is a no-op.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.Internal.TrendlineBase.SetSeriesProperties")]

[assembly: SuppressMessage(
    "Reliability",
    "CA2000:Dispose objects before losing scope",
    Justification = "2026-10-01: ChartTrendline is a non-rendered model object used only for a local polynomial-order calculation; it owns no JS/unmanaged resources so DisposeAsync is a no-op.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.Internal.TrendlineBase.GetPolynomialPoints(System.Collections.Generic.List{System.Double},System.Collections.Generic.List{System.Double})")]

[assembly: SuppressMessage(
    "Reliability",
    "CA2000:Dispose objects before losing scope",
    Justification = "2026-10-01: ChartMarker is a non-rendered model object handed to UpdateSeriesProperties; it owns no JS/unmanaged resources so DisposeAsync is a no-op.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.Internal.TrendlineBase.UpdateTrendlineMarker")]

[assembly: SuppressMessage(
    "Reliability",
    "CA2000:Dispose objects before losing scope",
    Justification = "2026-10-01: ChartDefaultAnimation is a non-rendered model object used only to compute animation settings; it owns no JS/unmanaged resources so DisposeAsync is a no-op.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.Internal.TrendlineBase.UpdateTrendlineAnimation")]

[assembly: SuppressMessage(
    "Reliability",
    "CA2000:Dispose objects before losing scope",
    Justification = "2026-10-01: ChartSelectedDataIndex is a non-rendered model object; CreateSelectedData hands it to SelectionChartAsync which serializes it across JS interop (ownership transferred). It owns no JS/unmanaged resources.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.Internal.Selection.SelectPointAndAddToValues(Syncfusion.Blazor.Toolkit.Charts.Point,Syncfusion.Blazor.Toolkit.Charts.Internal.ChartSeriesRenderer,System.Collections.Generic.List{Syncfusion.Blazor.Toolkit.Charts.PointXY})")]

[assembly: SuppressMessage(
    "Reliability",
    "CA2000:Dispose objects before losing scope",
    Justification = "2026-10-01: ChartAxis / ChartAxisScrollbarSettingsRange models built here are non-rendered and are handed into the user's OnScrollChanged event args (ownership transferred). They own no JS/unmanaged resources.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.TriggerScrollEvents(Syncfusion.Blazor.Toolkit.Charts.Internal.IScrollEventsArgs)")]

[assembly: SuppressMessage(
    "Reliability",
    "CA2000:Dispose objects before losing scope",
    Justification = "2026-10-01: ChartAxisScrollbarSettingsRange is a non-rendered model object returned to the caller (ownership transferred); it owns no JS/unmanaged resources so DisposeAsync is a no-op.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.GetStartEnd(System.Object,System.Object,Syncfusion.Blazor.Toolkit.ValueType)")]

[assembly: SuppressMessage(
    "Reliability",
    "CA2000:Dispose objects before losing scope",
    Justification = "2026-10-01: ChartAxisScrollbarSettingsRange / ChartAxis models built here are non-rendered and are wrapped into the returned ScrollEventArgs (ownership transferred). They own no JS/unmanaged resources.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.GetScrollArguments(System.String,Syncfusion.Blazor.Toolkit.Charts.ChartAxis,Syncfusion.Blazor.Toolkit.Charts.Internal.DoubleRange,System.Double,System.Double,Syncfusion.Blazor.Toolkit.Charts.ChartAxisScrollbarSettingsRange,System.Double)")]

[assembly: SuppressMessage(
    "Reliability",
    "CA2000:Dispose objects before losing scope",
    Justification = "2026-10-01: The flagged out-locals alias already-rendered, renderer-owned ChartAxis / ChartStripline components resolved from existing collections; disposing them here would be a double-dispose.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.Internal.ChartStriplineTooltipSettings.MouseMoveHandlerAsync(System.String)")]


// -------------------------------------------------------------------------
// CA1308 — Normalize strings to uppercase.
//
// CA1308 recommends ToUpperInvariant over ToLowerInvariant because
// uppercasing is round-trip safe for a small set of locale edge cases.
// Every flagged call here, however, requires lowercase output as a
// semantic contract and is already culture-invariant, so the rule's
// suggested fix would break behaviour rather than improve correctness.
// CA1308 is a Globalization rule, not a security rule.
//
//   * SfButton.ApplyIconClasses — the lowercased value becomes a CSS
//     class name (e-icon-left, e-top-icon-btn, ...). CSS class names are
//     lowercase by convention and are emitted into the element's `class`
//     attribute; uppercasing would produce invalid, non-matching classes.
//   * DataUtil.PerformAggregation — the lowercased value forms the
//     user-facing aggregate dictionary key ("Field - sum"). The lowercase
//     key is part of the published result contract; uppercasing changes it.
//   * The DynamicQueryableExtensions / QueryableExtensions filter methods
//     build case-insensitive filter expression trees. The constant side
//     is lowered with ToLowerInvariant to match the column side, which is
//     lowered by the shared ToLowerMethodCallExpression helper (string
//     .ToLower, provider-translatable). Both sides MUST use the same
//     lowercase casing for the comparison to work; switching the constant
//     to uppercase without the matching column-side change would silently
//     break every case-insensitive filter. All calls are invariant-culture
//     (no Turkish-I / locale defect), so the correctness concern CA1308
//     guards against does not apply.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Globalization",
    "CA1308:Normalize strings to uppercase",
    Justification = "2026-10-01: Produces a lowercase CSS class name (e-icon-left / e-top-icon-btn) emitted into the element class attribute; lowercase is required by CSS convention and the call is already culture-invariant.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Buttons.SfButton.ApplyIconClasses(System.Text.StringBuilder)")]

[assembly: SuppressMessage(
    "Globalization",
    "CA1308:Normalize strings to uppercase",
    Justification = "2026-10-01: Builds the user-facing aggregate dictionary key ('Field - sum'); the lowercase key is part of the published result contract and the call is already culture-invariant.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.DataUtil.PerformAggregation(System.Collections.IEnumerable,System.Collections.Generic.IList{Syncfusion.Blazor.Toolkit.Data.Aggregate})")]

[assembly: SuppressMessage(
    "Globalization",
    "CA1308:Normalize strings to uppercase",
    Justification = "2026-10-01: Case-insensitive filter: the constant side is lowered to match the column side lowered by ToLowerMethodCallExpression (string.ToLower). Both sides must share lowercase casing; uppercasing would break the filter. Call is invariant-culture.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.DynamicQueryableExtensions.GetExpression(Syncfusion.Blazor.Toolkit.Data.FilterType,System.Type,System.Object,System.Linq.Expressions.Expression,System.Boolean)")]

[assembly: SuppressMessage(
    "Globalization",
    "CA1308:Normalize strings to uppercase",
    Justification = "2026-10-01: Case-insensitive filter: the constant side is lowered to match the column side lowered by ToLowerMethodCallExpression (string.ToLower). Both sides must share lowercase casing; uppercasing would break the filter. Call is invariant-culture.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.DynamicQueryableExtensions.GetPExpression(Syncfusion.Blazor.Toolkit.Data.FilterType,System.Boolean,System.Object,System.Linq.Expressions.Expression,System.Type)")]

[assembly: SuppressMessage(
    "Globalization",
    "CA1308:Normalize strings to uppercase",
    Justification = "2026-10-01: Case-insensitive filter: the constant side is lowered to match the column side lowered by ToLowerMethodCallExpression (string.ToLower). Both sides must share lowercase casing; uppercasing would break the filter. Call is invariant-culture.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.QueryableExtensions.GetPxExpression(Syncfusion.Blazor.Toolkit.Data.FilterType,System.Type,System.Object,System.Boolean,System.Linq.Expressions.Expression,System.Linq.Expressions.Expression,System.Boolean)")]

[assembly: SuppressMessage(
    "Globalization",
    "CA1308:Normalize strings to uppercase",
    Justification = "2026-10-01: Case-insensitive filter: the constant side is lowered to match the column side lowered by ToLowerMethodCallExpression (string.ToLower). Both sides must share lowercase casing; uppercasing would break the filter. Call is invariant-culture.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.QueryableExtensions.Predicate(System.Linq.IQueryable,System.Linq.Expressions.ParameterExpression,System.String,System.Object,Syncfusion.Blazor.Toolkit.Data.FilterType,Syncfusion.Blazor.Toolkit.Data.FilterBehavior,System.Boolean,System.Type,System.String)")]

[assembly: SuppressMessage(
    "Globalization",
    "CA1308:Normalize strings to uppercase",
    Justification = "2026-10-01: Case-insensitive filter: the constant side is lowered to match the column side lowered by ToLowerMethodCallExpression (string.ToLower). Both sides must share lowercase casing; uppercasing would break the filter. Call is invariant-culture.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.QueryableExtensions.GetPExpression(Syncfusion.Blazor.Toolkit.Data.FilterType,System.Type,System.String,System.Linq.Expressions.Expression,System.Object,System.Type,System.Boolean)")]

[assembly: SuppressMessage(
    "Globalization",
    "CA1308:Normalize strings to uppercase",
    Justification = "2026-10-01: Case-insensitive filter: the constant side is lowered to match the column side lowered by ToLowerMethodCallExpression (string.ToLower). Both sides must share lowercase casing; uppercasing would break the filter. Call is invariant-culture.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.QueryableExtensions.Where(System.Linq.IQueryable,System.String,System.Object,Syncfusion.Blazor.Toolkit.Data.FilterType,System.Boolean,System.Type)")]

// ---------------------------------------------------------------------------
// CA1031: Do not catch general exception types
// ---------------------------------------------------------------------------
// The suppressions below cover intentional broad catches on Blazor component
// lifecycle (OnParametersSetAsync / OnAfterRenderAsync / OnAfterScriptRenderedAsync)
// and asynchronous disposal (DisposeAsyncCore) / best-effort I/O paths. In these
// paths a thrown exception propagates to the renderer SynchronizationContext and
// can terminate the Blazor Server circuit (or crash a WASM app). The code
// deliberately catches all exceptions, logs them, and keeps the renderer alive so
// subsequent updates still work. These catches cannot be safely narrowed because
// user-supplied EventCallbacks, templates and JS interop may throw arbitrary
// exception types. The two computational catches that COULD be narrowed
// (SfTimePicker list generation and SfSwitch accessible-name) were fixed in code
// with exception filters instead of being suppressed here.

[assembly: SuppressMessage(
    "Design",
    "CA1031:Do not catch general exception types",
    Justification = "2026-10-01: Component lifecycle must not re-throw. An exception escaping OnParametersSetAsync propagates to the renderer SynchronizationContext and can terminate the Blazor Server circuit. User parameters/templates may throw arbitrary types, so the catch is intentionally broad; it is logged and the renderer kept alive.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Inputs.SfNumericTextBox`1.OnParametersSetAsync~System.Threading.Tasks.Task")]

[assembly: SuppressMessage(
    "Design",
    "CA1031:Do not catch general exception types",
    Justification = "2026-10-01: Component lifecycle must not re-throw. An exception escaping OnAfterRenderAsync propagates to the renderer SynchronizationContext and can terminate the Blazor Server circuit. JS interop and user callbacks may throw arbitrary types, so the catch is intentionally broad; it is logged and the renderer kept alive.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Inputs.SfNumericTextBox`1.OnAfterRenderAsync(System.Boolean)~System.Threading.Tasks.Task")]

[assembly: SuppressMessage(
    "Design",
    "CA1031:Do not catch general exception types",
    Justification = "2026-10-01: Component lifecycle must not re-throw. An exception escaping OnAfterRenderAsync propagates to the renderer SynchronizationContext and can terminate the Blazor Server circuit. JS interop and user callbacks may throw arbitrary types, so the catch is intentionally broad; it is logged and the renderer kept alive.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Popups.SfDialog.OnAfterRenderAsync(System.Boolean)~System.Threading.Tasks.Task")]

[assembly: SuppressMessage(
    "Design",
    "CA1031:Do not catch general exception types",
    Justification = "2026-10-01: First-render JS bootstrap must not crash the Blazor Server circuit. An exception escaping OnAfterScriptRenderedAsync propagates to the renderer SynchronizationContext. JS interop and the Created callback may throw arbitrary types, so the catch is intentionally broad; it is logged and the component kept usable.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Popups.SfDialog.OnAfterScriptRenderedAsync~System.Threading.Tasks.Task")]

[assembly: SuppressMessage(
    "Design",
    "CA1031:Do not catch general exception types",
    Justification = "2026-10-01: Async disposal must not re-throw. An unhandled exception out of DisposeAsyncCore propagates to the SynchronizationContext and terminates the Blazor Server circuit (or crashes WASM). More specific types are caught first (InvalidOperationException); the general catch is a best-effort fallback that logs and reports via the Destroyed callback.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Popups.SfTooltip.DisposeAsyncCore~System.Threading.Tasks.ValueTask")]

[assembly: SuppressMessage(
    "Design",
    "CA1031:Do not catch general exception types",
    Justification = "2026-10-01: Disposal must not re-throw. The specific types (ObjectDisposedException, InvalidOperationException) are handled first; this general catch is a logged best-effort fallback so a stray exception cannot terminate the Blazor Server circuit during teardown.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Spinner.SfSpinner.DisposeAsyncCore~System.Threading.Tasks.ValueTask")]

[assembly: SuppressMessage(
    "Design",
    "CA1031:Do not catch general exception types",
    Justification = "2026-10-01: Best-effort per-file caching. If reading a single selected file's stream fails (I/O, browser quirk, size mismatch), the broad catch lets the remaining files continue caching rather than aborting the whole upload. Nothing is re-thrown; the file is simply skipped.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Inputs.SfUploader.CacheFilesAsync(Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs)~System.Threading.Tasks.Task")]
