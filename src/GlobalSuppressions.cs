// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

// Suppress warnings for reflection-heavy code that cannot be easily refactored
[assembly: SuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "Used in reflection-heavy scenarios where alternatives are not feasible.")]
[assembly: SuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access to referenced types or members.", Justification = "Used in reflection-heavy scenarios where alternatives are not feasible.")]