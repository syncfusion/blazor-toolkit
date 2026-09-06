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