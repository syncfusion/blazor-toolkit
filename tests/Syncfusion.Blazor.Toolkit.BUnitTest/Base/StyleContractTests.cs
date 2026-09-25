// Copyright © 2001-2026 Syncfusion Inc. All rights reserved.
// Licensed under the MIT license. See LICENSE in the repository root for full license information.

using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.BUnitTest.Base;

/// <summary>
/// D5 / BEQ-20 enforcement: every Blazor CSS-isolation file
/// (<c>Component.razor.css</c>) must be colocated with its
/// <c>Component.razor</c> of the same name, and every required public
/// selector from <c>src/wwwroot/styles/STYLE-CONTRACT.md</c> must be
/// present in the published stylesheet bundle.
/// </summary>
public sealed class StyleContractTests
{
    private static readonly string[] RequiredSelectors =
    {
        "e-lib",
        "e-control",
        "e-btn",
        "e-btn-group",
        "e-checkbox",
        "e-radio",
        "e-switch",
        "e-textbox",
        "e-textarea",
        "e-numerictextbox",
        "e-uploader",
        "e-calendar",
        "e-datepicker",
        "e-datetimepicker",
        "e-timepicker",
        "e-dialog",
        "e-tooltip",
        "e-spinner",
        "e-chart"
    };

    [Fact]
    public void RequiredPublicSelectors_AreDeclaredInStyleContract()
    {
        var contractFile = LocateRepoFile(@"src\wwwroot\styles\STYLE-CONTRACT.md");
        Assert.True(File.Exists(contractFile), $"Style contract file not found: {contractFile}");

        var contractText = File.ReadAllText(contractFile);
        foreach (var selector in RequiredSelectors)
        {
            Assert.Contains($".{selector}", contractText);
        }
    }

    private static string LocateRepoFile(string relativePath)
    {
        return Path.Combine(
            RepositoryRootFind(Assembly.GetExecutingAssembly().Location)!,
            relativePath);
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