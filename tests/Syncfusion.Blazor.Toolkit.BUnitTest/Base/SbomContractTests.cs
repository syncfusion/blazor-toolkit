// Copyright © 2001-2026 Syncfusion Inc. All rights reserved.
// Licensed under the MIT license. See LICENSE in the repository root for full license information.

using System.IO;
using System.Reflection;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.BUnitTest.Base;

/// <summary>
/// D6 / PI-06 coarse regression test. The artifact-level SBOM presence
/// is verified in CI by reading the resulting `sbom` artifact; this test
/// simply asserts that the public repository still describes the SBOM
/// hand-off in <c>.github/THREAT-MODEL.md</c>.
/// </summary>
public sealed class SbomContractTests
{
    [Fact]
    public void ThreatModel_DescribesSbomPolicy()
    {
        var path = Path.Combine(
            RepositoryRootFind(Assembly.GetExecutingAssembly().Location)!,
            ".github",
            "THREAT-MODEL.md");

        Assert.True(File.Exists(path), $"Phreat model file not found at: {path}");

        var text = File.ReadAllText(path);
        Assert.Contains("SBOM", text, System.StringComparison.OrdinalIgnoreCase);
        Assert.Contains("SPDX", text, System.StringComparison.OrdinalIgnoreCase);
    }

    private static string? RepositoryRootFind(string startPath)
    {
        var dir = new DirectoryInfo(Path.GetDirectoryName(startPath)!);
        while (dir is not null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, "src"))
                && Directory.Exists(Path.Combine(dir.FullName, ".github")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        return null;
    }
}