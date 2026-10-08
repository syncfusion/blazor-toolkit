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


// -------------------------------------------------------------------------
// CA1720: Identifier contains type name.
//
// Each target is a PUBLIC enum member whose name is a domain term that
// happens to collide with a BCL type name. Renaming would be a binary-
// and source-breaking change to the public API (and, for DayHeaderFormats,
// would also break the fixed [EnumMember(Value = "Short")] JSON contract).
// The names are the clearest domain vocabulary for consumers, so the
// collision is accepted.
// -------------------------------------------------------------------------

[assembly: SuppressMessage(
    "Naming",
    "CA1720:Identifier contains type name",
    Justification = "2026-10-01: Public API. 'Single' is the clearest name for single-item selection in SelectionMode; renaming is a binary/source breaking change for consumers. The collision with System.Single is cosmetic.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.SelectionMode.Single")]

[assembly: SuppressMessage(
    "Naming",
    "CA1720:Identifier contains type name",
    Justification = "2026-10-01: Public API. 'Short' names the short day-name display format in DayHeaderFormats and is pinned to the serialized contract [EnumMember(Value = \"Short\")]; renaming would break both the public API and the JSON wire format. The collision with System.Int16's alias is cosmetic.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.DayHeaderFormats.Short")]

[assembly: SuppressMessage(
    "Naming",
    "CA1720:Identifier contains type name",
    Justification = "2026-10-01: Public API. 'Double' names the numeric axis kind in the chart ValueType enum and matches the long-standing Syncfusion chart vocabulary; renaming is a binary/source breaking change for consumers. The collision with System.Double is cosmetic.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.ValueType.Double")]


// -------------------------------------------------------------------------
// CA1024: Use properties where appropriate.
//
// SfDialog.GetButtonItems() is a long-standing PUBLIC API method with
// documented usage examples. Converting it to a property would be a
// binary- and source-breaking change for consumers, and the Get* method
// shape is the established convention for the Syncfusion component API
// (mirrors GetButtonItems across the suite). The method is retained.
// -------------------------------------------------------------------------

[assembly: SuppressMessage(
    "Design",
    "CA1024:Use properties where appropriate",
    Justification = "2026-10-01: Public API. GetButtonItems() is a documented method on SfDialog; converting it to a property is a binary/source breaking change for consumers and departs from the established Get* method convention across the component suite.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Popups.SfDialog.GetButtonItems~System.Collections.Generic.IList{Syncfusion.Blazor.Toolkit.Popups.DialogButton}")]


// -------------------------------------------------------------------------
// CA1003: Use generic event handler instances.
//
// SfDialogService.OnOpen is an internal-wiring event between the injected
// service and SfDialogProvider. It is technically public but hidden from
// the public surface ([EditorBrowsable(Never)] + <exclude/>). Its multi-
// parameter Action<...> signature is the established contract that the
// provider subscribes to; replacing it with EventHandler<T> would require
// a bespoke EventArgs type and is a binary/source-breaking change to the
// (hidden) public API for no consumer-visible benefit.
// -------------------------------------------------------------------------

[assembly: SuppressMessage(
    "Design",
    "CA1003:Use generic event handler instances",
    Justification = "2026-10-01: Internal-wiring event hidden from the public surface ([EditorBrowsable(Never)] + <exclude/>). The multi-parameter Action<...> signature is the established contract between SfDialogService and SfDialogProvider; converting to EventHandler<T> would need a bespoke EventArgs and is a breaking change for no consumer-visible benefit.",
    Scope = "member",
    Target = "~E:Syncfusion.Blazor.Toolkit.Popups.SfDialogService.OnOpen")]


// -------------------------------------------------------------------------
// CA1721: Property names should not match get methods.
//
// Both targets are PUBLIC API where the property and the Get* method are
// intentionally distinct, long-standing members:
//   * ChartAxis.Name is a [Parameter] (the axis identifier bound in markup)
//     while GetName() is a public virtual method overridden by the
//     primary axes to return their reserved names. Renaming either breaks
//     the component markup contract or the inheritance override surface.
//   * BaseComponent.DataManager is the public data-manager property while
//     GetDataManager(...) is a protected helper that resolves a data
//     manager from a data source. Both are part of the public/protected
//     API of an abstract base class consumed by derived components.
// Renaming any of these is a binary/source breaking change.
// -------------------------------------------------------------------------

[assembly: SuppressMessage(
    "Naming",
    "CA1721:Property names should not match get methods",
    Justification = "2026-10-01: Public API. ChartAxis.Name is a [Parameter] bound in chart markup; GetName() is a public virtual method overridden by PrimaryXAxis/PrimaryYAxis to supply their reserved names. The two members are intentionally distinct and renaming either is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.ChartAxis.Name")]

[assembly: SuppressMessage(
    "Naming",
    "CA1721:Property names should not match get methods",
    Justification = "2026-10-01: Public API. BaseComponent.DataManager is the public data-manager property; GetDataManager(object, string) is a protected helper that resolves a data manager from a data source. Both belong to the public/protected surface of an abstract base class and renaming either is a binary/source breaking change for derived components.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.BaseComponent.DataManager")]


// -------------------------------------------------------------------------
// CA1707: Identifiers should not contain underscores.
//
// All targets are protected members of PUBLIC types (an externally-
// derivable surface), so renaming is a binary/source breaking change for
// subclasses:
//   * SfDatePicker<TValue>.ARIA_LABELLEDBY / ARIA_DESCRIBEDBY are protected
//     const ARIA attribute-name strings. The SCREAMING_SNAKE_CASE spelling
//     is the long-standing convention for these constants across the suite.
//   * BaseComponent._uniqueId is a protected auto-property on an abstract
//     base class consumed by derived data components.
// -------------------------------------------------------------------------

[assembly: SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "2026-10-01: Protected const on the public type SfDatePicker<TValue> (externally-derivable surface). ARIA_LABELLEDBY is a long-standing ARIA attribute-name constant; renaming is a binary/source breaking change for subclasses.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.ARIA_LABELLEDBY")]

[assembly: SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "2026-10-01: Protected const on the public type SfDatePicker<TValue> (externally-derivable surface). ARIA_DESCRIBEDBY is a long-standing ARIA attribute-name constant; renaming is a binary/source breaking change for subclasses.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.ARIA_DESCRIBEDBY")]

[assembly: SuppressMessage(
    "Naming",
    "CA1707:Identifiers should not contain underscores",
    Justification = "2026-10-01: Protected auto-property on the public abstract base class BaseComponent (externally-derivable surface). _uniqueId is consumed by derived data components; renaming is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.BaseComponent._uniqueId")]


// -------------------------------------------------------------------------
// CA1055: URI return values should not be strings.
//
// DataUtil.GetUrl is a public static helper that composes a base URL, a
// (possibly relative) path and an optional query string into a single
// string. The result is intentionally a string: it may be a relative URL
// and is assigned directly to the string DataManagerRequest/HttpHandler
// Url property. Returning System.Uri would be both a binary/source
// breaking change and semantically wrong (System.Uri cannot represent the
// relative/partial results this helper produces).
// -------------------------------------------------------------------------

[assembly: SuppressMessage(
    "Design",
    "CA1055:URI return values should not be strings",
    Justification = "2026-10-01: Public API. DataUtil.GetUrl composes a base URL, a possibly-relative path and an optional query string into a string that is assigned to the string HttpHandler/DataManager Url. The result may be relative, so System.Uri is semantically unsuitable and changing the return type is a binary/source breaking change.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.DataUtil.GetUrl(System.String,System.String,System.String)~System.String")]


// -------------------------------------------------------------------------
// CA1054: URI parameters should not be strings.
//
// The baseUrl and relativeUrl parameters of the public helper
// DataUtil.GetUrl are intentionally strings. The method accepts
// (possibly relative) URL fragments, concatenates them with slash
// normalization and appends a query string. System.Uri cannot represent
// the relative/partial inputs this helper is designed to combine, and the
// values flow from the string DataManagerRequest/HttpHandler Url property.
// Changing the parameter types (or adding Uri overloads) would be a
// binary/source breaking change for a public API with no behavioral gain.
// -------------------------------------------------------------------------

[assembly: SuppressMessage(
    "Design",
    "CA1054:URI parameters should not be strings",
    Justification = "2026-10-01: Public API. DataUtil.GetUrl accepts possibly-relative URL fragments (baseUrl, relativeUrl) that originate from the string HttpHandler/DataManager Url and are concatenated with slash normalization. System.Uri cannot represent these relative/partial inputs, so changing the parameter types is both semantically wrong and a binary/source breaking change.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.DataUtil.GetUrl(System.String,System.String,System.String)~System.String")]


// -------------------------------------------------------------------------
// CA1056: URI properties should not be strings.
//
// All flagged members are public string URL properties on public types.
// They are intentionally strings for three reasons:
//   1. Blazor data binding: [Parameter] URL properties (DataManager.Url,
//      UploaderAsyncSettings.SaveUrl/RemoveUrl) are bound from Razor markup
//      as HTML attribute strings and commonly carry RELATIVE URLs
//      ("/api/upload"), which System.Uri cannot represent as an absolute.
//   2. JSON serialization contracts: the adaptor/model DTOs
//      (AsyncSettingsModel, DefaultAdaptor, RequestOptions, Utils) are
//      serialized to/from JavaScript with [JsonPropertyName] string
//      contracts; switching to System.Uri would change the wire format.
//   3. Relative-URL composition: DataManager.BaseUri and the adaptor Url
//      values are concatenated by DataUtil.GetUrl (see CA1054/CA1055) to
//      produce possibly-relative absolute URLs.
// Changing any of these to System.Uri is a binary/source breaking change
// across the public data and uploader APIs with no behavioral benefit.
// -------------------------------------------------------------------------

[assembly: SuppressMessage(
    "Design",
    "CA1056:URI properties should not be strings",
    Justification = "2026-10-01: Public API. [Parameter] URL property bound from Razor markup as an HTML attribute string; commonly carries a relative URL that System.Uri cannot represent. Changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.UploaderAsyncSettings.RemoveUrl")]

[assembly: SuppressMessage(
    "Design",
    "CA1056:URI properties should not be strings",
    Justification = "2026-10-01: Public API. [Parameter] URL property bound from Razor markup as an HTML attribute string; commonly carries a relative URL that System.Uri cannot represent. Changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.UploaderAsyncSettings.SaveUrl")]

[assembly: SuppressMessage(
    "Design",
    "CA1056:URI properties should not be strings",
    Justification = "2026-10-01: Public API. JSON-serialized uploader DTO property with a [JsonPropertyName] string contract; carries a possibly-relative URL. Changing the type to System.Uri would alter the wire format and is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.AsyncSettingsModel.RemoveUrl")]

[assembly: SuppressMessage(
    "Design",
    "CA1056:URI properties should not be strings",
    Justification = "2026-10-01: Public API. JSON-serialized uploader DTO property with a [JsonPropertyName] string contract; carries a possibly-relative URL. Changing the type to System.Uri would alter the wire format and is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.AsyncSettingsModel.SaveUrl")]

[assembly: SuppressMessage(
    "Design",
    "CA1056:URI properties should not be strings",
    Justification = "2026-10-01: Public API. Request DTO (Utils) property holding a possibly-relative batch URL that is composed via DataUtil.GetUrl. Changing the type to System.Uri is semantically unsuitable for relative URLs and is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.Utils.Url")]

[assembly: SuppressMessage(
    "Design",
    "CA1056:URI properties should not be strings",
    Justification = "2026-10-01: Public API. Request DTO (RequestOptions) property holding a possibly-relative service URL. Changing the type to System.Uri is semantically unsuitable for relative URLs and is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.RequestOptions.Url")]

[assembly: SuppressMessage(
    "Design",
    "CA1056:URI properties should not be strings",
    Justification = "2026-10-01: Public API. Request DTO (RequestOptions) property holding a possibly-relative application base URL that is composed with service URLs via DataUtil.GetUrl. Changing the type to System.Uri is semantically unsuitable for relative URLs and is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.RequestOptions.BaseUrl")]

[assembly: SuppressMessage(
    "Design",
    "CA1056:URI properties should not be strings",
    Justification = "2026-10-01: Public API. JSON-serialized adaptor DTO (DefaultAdaptor) property with a [JsonPropertyName] string contract; carries a possibly-relative service URL. Changing the type to System.Uri would alter the wire format and is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DefaultAdaptor.Url")]

[assembly: SuppressMessage(
    "Design",
    "CA1056:URI properties should not be strings",
    Justification = "2026-10-01: Public API. [Parameter] endpoint URL property bound from Razor markup as an HTML attribute string with a [JsonPropertyName] contract; commonly carries a relative URL that System.Uri cannot represent. Changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManager.Url")]

[assembly: SuppressMessage(
    "Design",
    "CA1056:URI properties should not be strings",
    Justification = "2026-10-01: Public API. DataManager.BaseUri holds the application base URL used to resolve the relative Url/InsertUrl/UpdateUrl/RemoveUrl values via DataUtil.GetUrl. It is derived from NavigationManager and may itself be relative, so System.Uri is semantically unsuitable and changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.DataManager.BaseUri")]


// -------------------------------------------------------------------------
// CA1819: Properties should not return arrays.
//
// All flagged members are public array properties on public types. They
// are intentionally arrays for three reasons:
//   1. Blazor two-way binding: [Parameter] array properties (SfCalendar
//      Values, SfDatePicker/SfTimePicker InputFormats, SfChart.Palettes,
//      SfDialog.ResizeHandles, and the internal calendar renderer
//      MultiValues/MultiselectValues) are bound with @bind-* against
//      user-supplied T[] fields; arrays are the idiomatic, documented
//      binding type and the examples in the public XML docs assign arrays.
//   2. JSON / JS-interop contracts: the serialization models
//      (ChangedEventArgs<T>.Values, SVGTooltip Content/Palette/Shapes,
//      BoxPoint Outliers/YValueCollection, FailureEventArgs.RetryFiles)
//      are serialized to/from JavaScript; arrays map directly to JS arrays
//      and the [JsonPropertyName] wire format depends on the array shape.
//   3. Public fluent query API: Query.Lookups/SortedColumns/GroupedColumns
//      are populated through object initializers and the chainable query
//      builder and are part of the documented remote-adaptor contract.
// Returning a read-only collection instead would be a binary/source
// breaking change across the public component, data and charting APIs with
// no behavioral benefit, so the array shape is retained by design.
// -------------------------------------------------------------------------

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. JSON/JS-interop serialization model; Values maps to a JavaScript array and is part of the event-args wire contract. Changing to a read-only collection is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.ChangedEventArgs`1.Values")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. [Parameter] DateTime[] bound via @bind against user-supplied arrays in multi-selection mode (see documented examples). Changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.Internal.CalendarBaseRender`1.MultiValues")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. [Parameter] DateTime[] on the public calendar cell renderer, bound against user-supplied arrays in multi-selection mode. Changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.Internal.CalendarDayCell`1.MultiselectValues")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. [Parameter] DateTime[] bound via @bind-Values against user-supplied arrays in multi-selection mode (see documented examples). Changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfCalendar`1.Values")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. [Parameter] string[] of input format patterns assigned from user-supplied arrays (see documented examples). Changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.InputFormats")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. [Parameter] string[] of input format patterns assigned from user-supplied arrays (see documented examples). Changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.InputFormats")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. JSON/JS-interop box-plot model; Outliers maps to a JavaScript array in the chart tooltip/series wire contract. Changing to a read-only collection is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.BoxPoint.Outliers")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. JSON/JS-interop box-plot model; YValueCollection maps to a JavaScript array in the chart tooltip/series wire contract. Changing to a read-only collection is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.BoxPoint.YValueCollection")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. [Parameter] string[] color palette assigned from user-supplied arrays (see documented examples). Changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.SfChart.Palettes")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. JSON/JS-interop tooltip model; Content maps to a JavaScript array of content strings in the wire contract. Changing to a read-only collection is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.SVGTooltip.Content")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. JSON/JS-interop tooltip model; Palette maps to a JavaScript array of color strings in the wire contract. Changing to a read-only collection is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.SVGTooltip.Palette")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. JSON/JS-interop tooltip model; Shapes maps to a JavaScript array of legend-marker shapes in the wire contract. Changing to a read-only collection is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.SVGTooltip.Shapes")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. JSON-serialized uploader event-args; RetryFiles maps to a JavaScript array in the [JsonPropertyName] wire contract. Changing to a read-only collection is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Inputs.FailureEventArgs.RetryFiles")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. [Parameter] ResizeDirection[] assigned from user-supplied arrays (see documented examples). Changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Popups.SfDialog.ResizeHandles")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. Query.Lookups is part of the documented fluent query / remote-adaptor contract, populated through object initializers and the chainable query builder. Changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.Query.Lookups")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. Query.SortedColumns is part of the documented fluent query / remote-adaptor contract, populated through object initializers and the chainable query builder. Changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.Query.SortedColumns")]

[assembly: SuppressMessage(
    "Performance",
    "CA1819:Properties should not return arrays",
    Justification = "2026-10-01: Public API. Query.GroupedColumns is part of the documented fluent query / remote-adaptor contract, populated through object initializers and the chainable query builder. Changing the type is a binary/source breaking change.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.Query.GroupedColumns")]


// -------------------------------------------------------------------------
// IDE0031 — Use null propagation.
//
// 2026-10-02 audit: 27 null-conditional ASSIGNMENT suggestions in 22 methods,
// including event unsubscription. The C# 14 assignment syntax is unavailable
// to shared net8.0/C# 12 and net9.0/C# 13 source; retain the explicit guards.
// Each entry covers only the containing method, never the containing type.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Calendars.Internal.CalendarBaseRender`1.CellClickAsync(Syncfusion.Blazor.Toolkit.Calendars.CellDetails)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Calendars.Internal.CalendarBaseRender`1.ClickHandlerAsync(Syncfusion.Blazor.Toolkit.Calendars.CellDetails)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Calendars.Internal.CalendarBaseRender`1.HandleSelectKeyAsync(Syncfusion.Blazor.Toolkit.Calendars.Internal.KeyActions,Syncfusion.Blazor.Toolkit.Calendars.CellDetails,System.Boolean,System.Int32)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Calendars.Internal.CalendarBaseRender`1.HandleYearViewClickAsync(Syncfusion.Blazor.Toolkit.Calendars.CellDetails,System.DateTime,System.Boolean)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Calendars.Internal.CalendarBaseRender`1.PrepareKeyboardNavigation")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Calendars.Internal.CalendarBaseRender`1.SelectKeyActionAsync(Syncfusion.Blazor.Toolkit.Calendars.Internal.KeyActions,Syncfusion.Blazor.Toolkit.Calendars.CellDetails,System.Boolean,System.Int32)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Calendars.SfDateTimePicker`1.IsValidTimeAsync")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.ChartMargin.OnInitialized")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.ChartSeries.DataCollectionChanged(System.Object,System.Collections.Specialized.NotifyCollectionChangedEventArgs)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.ChartSeries.OnAfterRender(System.Boolean)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.ChartSeries.PrepareForLegendToggle")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.ChartSeries.RefreshSeriesAsync")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.ChartSeries.UpdateSeriesDataAsync")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.ChartStackLabelSettings.OnInitialized")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.Internal.ChartSeriesRendererContainer.CalculateStackingValues(System.Collections.Generic.List{Syncfusion.Blazor.Toolkit.Charts.ChartSeries},System.Boolean)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.Internal.ChartSeriesRendererContainer.FindFrequencies(System.Collections.Generic.List{Syncfusion.Blazor.Toolkit.Charts.ChartSeries})")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain all three guarded assignments; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.Internal.LegendBase.HandlePaging(System.String)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.AddSeriesAsync(System.Collections.Generic.IList{Syncfusion.Blazor.Toolkit.Charts.ChartSeries})")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain both guarded assignments; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.OnChartKeyboardNavigationsAsync(System.String,System.String)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded assignment; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.ZoomToolkitSetDeferredZoom(Syncfusion.Blazor.Toolkit.Charts.ZoomingEventArgs)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain all three guarded assignments; null-conditional assignment requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.ZoomingComplete(Syncfusion.Blazor.Toolkit.Charts.ZoomingEventArgs,Syncfusion.Blazor.Toolkit.Charts.Internal.IZoomingStates)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0031:Use null propagation",
    Justification = "2026-10-02: Retain the guarded Service.OnOpen -= OnOpenAsync event unsubscription; null-conditional assignment requires C# 14, unavailable in shared net8.0/C# 12 and net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Popups.SfDialogProvider.Dispose(System.Boolean)")]


// -------------------------------------------------------------------------
// IDE0032 — Use auto property.
//
// 2026-10-02 audit: 96 diagnostics on backing FIELD declarations, so every
// target below is ~F, not the associated ~P. Six trivial properties have
// already been converted and are not suppressed. Remaining custom accessors
// preserve reactive renderer updates, lazy initialization or guarded reads.
// Ordinary auto-properties lose those semantics; C# 14 field-backed property
// syntax preserves them but cannot compile on net8.0/C# 12 or net9.0/C# 13.
// Existing BL0007 decisions and the BL0007 report are unchanged.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._crossesAt")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._desiredIntervals")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._edgeLabelPlacement")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._enableTrim")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._format")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._interval")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._intervalType")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._isIndexed")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._labelFormat")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._labelIntersectAction")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._labelPlacement")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._labelPosition")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._labelRotation")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._maximum")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._maximumLabelWidth")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._minimum")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._name")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._opposedPosition")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._placeNextToAxisLine")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._rangePadding")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._startAngle")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._tickPosition")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._title")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive axis accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartAxis._valueType")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom marker accessors and their guard/update semantics; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartCommonMarker._border")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom marker accessors and their guard/update semantics; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartCommonMarker._dataLabel")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom marker accessors and their guard/update semantics; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartCommonMarker._fill")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom marker accessors and their guard/update semantics; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartCommonMarker._height")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom marker accessors and their guard/update semantics; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartCommonMarker._imageUrl")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom marker accessors and their guard/update semantics; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartCommonMarker._offset")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom marker accessors and their guard/update semantics; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartCommonMarker._opacity")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom marker accessors and their guard/update semantics; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartCommonMarker._renderer")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom marker accessors and their guard/update semantics; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartCommonMarker._shape")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom marker accessors and their guard/update semantics; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartCommonMarker._visible")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom marker accessors and their guard/update semantics; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartCommonMarker._widthProperty")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive last-label accessors and their renderer side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLastDataLabel._background")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive last-label accessors and their renderer side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLastDataLabel._cornerRadiusX")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive last-label accessors and their renderer side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLastDataLabel._cornerRadiusY")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive last-label accessors and their renderer side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLastDataLabel._dashArray")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive last-label accessors and their renderer side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLastDataLabel._lineColor")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive last-label accessors and their renderer side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLastDataLabel._lineWidth")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive last-label accessors and their renderer side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLastDataLabel._renderer")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive last-label accessors and their renderer side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLastDataLabel._showLabel")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._accessibilityRole")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._alignment")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._background")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._height")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._isInversed")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._itemPadding")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._maximumLabelWidth")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._opacity")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._padding")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._position")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._reverse")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._shapeHeight")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._shapePadding")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._shapeWidth")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._tabIndex")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._textWrap")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._visible")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive legend accessors and their renderer/layout side effects; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartLegendSettings._width")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._accessibilityRole")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._cardinalSplineTension")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._columnSpacing")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._columnWidth")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._columnWidthInPixel")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._dashArrayProperty")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._dataSource")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._enableComplexProperty")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._enableTooltip")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._fill")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._legendShape")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._name")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._opacityProperty")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._pointColorMapping")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._query")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._rendererProperty")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._showNearestTooltip")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._splineType")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._stepPosition")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._tooltipFormat")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._type")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._visible")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain custom series accessors and their guarded renderer/data updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartSeries._widthProperty")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive stack-label accessors and their guarded renderer updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartStackLabelSettings._angle")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive stack-label accessors and their guarded renderer updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartStackLabelSettings._border")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive stack-label accessors and their guarded renderer updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartStackLabelSettings._fill")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive stack-label accessors and their guarded renderer updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartStackLabelSettings._format")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive stack-label accessors and their guarded renderer updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartStackLabelSettings._stackLabelCornerRadiusX")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Retain reactive stack-label accessors and their guarded renderer updates; ordinary auto-properties lose them, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.ChartStackLabelSettings._stackLabelCornerRadiusY")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: AxisLayout changes clear/re-register axes and invalidate layout; an ordinary auto-property loses these side effects, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Charts.Internal.ChartAxisRendererContainer._axisLayout")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Preserve lazy reflection-method caching; an ordinary auto-property loses lazy initialization, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Data.QueryableExtensions._enumerableaverageMethods")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Preserve lazy reflection-method caching; an ordinary auto-property loses lazy initialization, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Data.QueryableExtensions._enumerablesummethods")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Preserve lazy reflection-method caching; an ordinary auto-property loses lazy initialization, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Data.QueryableExtensions._queryableSumMethod")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Preserve lazy reflection-method caching; an ordinary auto-property loses lazy initialization, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Data.QueryableExtensions._queryableaverageMethod")]

[assembly: SuppressMessage(
    "Style",
    "IDE0032:Use auto property",
    Justification = "2026-10-02: Preserve guarded InputFile event reads and fallback to the latest valid event; an ordinary auto-property loses those semantics, and C# 14 field syntax is unavailable on net8.0/C# 12 and net9.0/C# 13.",
    Scope = "member",
    Target = "~F:Syncfusion.Blazor.Toolkit.Inputs.SfUploader._inputFileChangeEvent")]


// -------------------------------------------------------------------------
// IDE0060 — Remove unused parameter.
//
// Preserve these public/protected signatures, including parameter names used
// by named-argument callers and the positional JS-invokable zoom contract.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Style",
    "IDE0060:Remove unused parameter",
    Justification = "2026-10-02: Preserve the protected SelectCalendarAsync(bool isSelection = false) signature for derived components and named-argument callers; removing or renaming the parameter breaks source/binary compatibility.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Calendars.SfDatePicker`1.SelectCalendarAsync(System.Boolean)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0060:Remove unused parameter",
    Justification = "2026-10-02: Preserve the public JSInvokable TriggerZoomingEvents signature and isZoomStart parameter name for JavaScript dispatch, binary compatibility and named-argument callers.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.TriggerZoomingEvents(System.String,System.Boolean)")]

[assembly: SuppressMessage(
    "Style",
    "IDE0060:Remove unused parameter",
    Justification = "2026-10-02: Preserve the public ParseValueWithTypeInformation(string, object, bool) overload and retVal parameter name for binary compatibility and named-argument callers.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.ValueConvert.ParseValueWithTypeInformation(System.String,System.Object,System.Boolean)")]


// -------------------------------------------------------------------------
// IDE0340 — Use unbound generic type.
//
// Four nameof expressions in this precise Predicate overload require bound
// Nullable<DateTime>/Nullable<DateTimeOffset> types on C# 12/13.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Style",
    "IDE0340:Use unbound generic type",
    Justification = "2026-10-02: Retain the four bound Nullable<T> nameof expressions; unbound generic nameof requires C# 14 and cannot compile in shared net8.0/C# 12 or net9.0/C# 13 source.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.QueryableExtensions.Predicate(System.Linq.IQueryable,System.Object,Syncfusion.Blazor.Toolkit.Data.FilterType,Syncfusion.Blazor.Toolkit.Data.FilterBehavior,System.Boolean,System.Type,System.Type,System.Linq.Expressions.Expression,System.Linq.Expressions.ParameterExpression,System.String,System.Boolean,System.Boolean)")]


// -------------------------------------------------------------------------
// IDE0390 — Make method synchronous.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Style",
    "IDE0390:Make method synchronous",
    Justification = "2026-10-02: Keep this awaited Task-returning renderer helper async even without await; renderer failures are delivered through a faulted Task rather than thrown synchronously, preserving caller error delivery.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.SfChart.UpdateNeededRenderersAsync")]


// -------------------------------------------------------------------------
// IDE0391 — Make method synchronous.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Style",
    "IDE0391:Make method synchronous",
    Justification = "2026-10-02: Keep initialization in OnInitializedAsync rather than moving it to OnInitialized; preserve Blazor lifecycle ordering, the async override/base-call contract for subclasses and faulted-Task error delivery.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Charts.Internal.TrimTooltipBase.OnInitializedAsync")]


// -------------------------------------------------------------------------
// IDE1006 — Naming Styles.
//
// These exact names are ABI/source, JS dispatch or JSON wire contracts.
// Internal IChartInternalLocation.x/y are wire names, not a public API claim.
// -------------------------------------------------------------------------
[assembly: SuppressMessage(
    "Naming",
    "IDE1006:Naming Styles",
    Justification = "2026-10-02: Preserve the public JSInvokable HidePopup name used by JavaScript and existing consumers; adding an Async suffix would break dispatch and source/binary compatibility.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.HidePopup(System.EventArgs)")]

[assembly: SuppressMessage(
    "Naming",
    "IDE1006:Naming Styles",
    Justification = "2026-10-02: Preserve the public JSInvokable ShowPopup name used by JavaScript and existing consumers; adding an Async suffix would break dispatch and source/binary compatibility.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Calendars.SfTimePicker`1.ShowPopup(System.EventArgs)")]

[assembly: SuppressMessage(
    "Naming",
    "IDE1006:Naming Styles",
    Justification = "2026-10-02: Internal location model uses lowercase x as a JS/JSON wire name; renaming to satisfy CLR naming style would change serialization compatibility.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.IChartInternalLocation.x")]

[assembly: SuppressMessage(
    "Naming",
    "IDE1006:Naming Styles",
    Justification = "2026-10-02: Internal location model uses lowercase y as a JS/JSON wire name; renaming to satisfy CLR naming style would change serialization compatibility.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Charts.Internal.IChartInternalLocation.y")]

[assembly: SuppressMessage(
    "Naming",
    "IDE1006:Naming Styles",
    Justification = "2026-10-02: Preserve the public Refresh method name for existing consumers; adding an Async suffix would break source/binary compatibility.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.BaseComponent.Refresh")]

[assembly: SuppressMessage(
    "Naming",
    "IDE1006:Naming Styles",
    Justification = "2026-10-02: Preserve the public virtual JSInvokable Trigger name for JavaScript dispatch, overrides and existing consumers; an Async suffix would break those contracts.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.BaseComponent.Trigger(System.String,System.String)")]

[assembly: SuppressMessage(
    "Naming",
    "IDE1006:Naming Styles",
    Justification = "2026-10-02: Preserve the public JSInvokable UpdateModel name for JavaScript dispatch and source/binary compatibility; an Async suffix would break those contracts.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.BaseComponent.UpdateModel(System.Collections.Generic.Dictionary{System.String,System.Object})")]

[assembly: SuppressMessage(
    "Naming",
    "IDE1006:Naming Styles",
    Justification = "2026-10-02: _uniqueId is a protected property on the public abstract BaseComponent; renaming it would break source/binary compatibility for derived components.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.BaseComponent._uniqueId")]

[assembly: SuppressMessage(
    "Naming",
    "IDE1006:Naming Styles",
    Justification = "2026-10-02: Preserve the public generic ExecuteQuery<T>(DataManagerRequest) overload name; adding an Async suffix would break source/binary compatibility for consumers.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.DataManager.ExecuteQuery``1(Syncfusion.Blazor.Toolkit.Data.DataManagerRequest)")]

[assembly: SuppressMessage(
    "Naming",
    "IDE1006:Naming Styles",
    Justification = "2026-10-02: Preserve the public generic ExecuteQuery<T>(Query) overload name; adding an Async suffix would break source/binary compatibility for consumers.",
    Scope = "member",
    Target = "~M:Syncfusion.Blazor.Toolkit.Data.DataManager.ExecuteQuery``1(Syncfusion.Blazor.Toolkit.Data.Query)")]

[assembly: SuppressMessage(
    "Naming",
    "IDE1006:Naming Styles",
    Justification = "2026-10-02: Preserve the public lowercase value property and its value JSON contract; a CLR rename would break source/binary compatibility even if JsonPropertyName kept the wire spelling.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.Data.WhereFilter.value")]
