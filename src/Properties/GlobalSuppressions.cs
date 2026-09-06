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

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Globalization",
    "CA1305:Specify IFormatProvider",
    Justification = "Error/log message formatting in this codebase never substitutes user-controlled values. The localized format is the framework default. Re-evaluated 2026-09-06.",
    Scope = "member",
    Target = "~P:Syncfusion.Blazor.Toolkit.LoggingErrorMessageArgs.Culture")]
[assembly: SuppressMessage(
    "Naming",
    "CA1716:Identifiers should not conflict with reserved keywords",
    Justification = "Reserved-keyword identifiers appear in public API surface mirrors of .NET design-time integrations (e.g. `Dynamic`, `Value`). The PDF style guide requires this naming convention.",
    Scope = "namespaceanddescendants",
    Target = "~N:Syncfusion.Blazor.Toolkit.Data")]
[assembly: SuppressMessage(
    "Design",
    "CA1017:Mark assemblies with ComVisible",
    Justification = "The assembly is a client-only Razor class library. COM exposure is not applicable and the attribute would have no effect. Marked `false` by policy.",
    Scope = "assembly",
    Target = "~M:Syncfusion.Blazor.Toolkit.AssemblyRef")]
[assembly: SuppressMessage(
    "Design",
    "CA1014:Mark assemblies with CLSCompliantAttribute",
    Justification = "The assembly is a Blazor component library that exposes public surface consumed from non-CLS-compliant languages downstream. We have opted out of CLS-compliance checks at the assembly level. Re-evaluated 2026-09-06.",
    Scope = "assembly",
    Target = "~M:Syncfusion.Blazor.Toolkit.AssemblyRef")]