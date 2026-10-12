using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Syncfusion.Blazor.Toolkit.Buttons;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.BUnitTest.Base;

public sealed class TestProjectContractTests
{
    [Theory]
    [InlineData("tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj")]
    [InlineData("tests/Syncfusion.Blazor.Toolkit.BUnitTest/Buttons/ButtonGroup/Syncfusion.Blazor.Tests.ButtonGroup.csproj")]
    public void ToolkitTestProject_ReferencesSourceWithoutCommercialPackagesOrSigning(string relativePath)
    {
        var root = FindRepositoryRoot();
        var projectPath = Path.Combine(root, relativePath);
        var project = XDocument.Load(projectPath);
        var sourceProject = Path.GetFullPath(Path.Combine(root, "src/Syncfusion.Blazor.Toolkit.csproj"));
        var references = project.Descendants("ProjectReference")
            .Select(reference => Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(projectPath)!,
                reference.Attribute("Include")!.Value.Replace('\\', Path.DirectorySeparatorChar))));

        Assert.Contains(sourceProject, references);
        Assert.DoesNotContain(project.Descendants("PackageReference"), reference =>
            reference.Attribute("Include")?.Value.StartsWith("Syncfusion.Blazor", StringComparison.OrdinalIgnoreCase) == true);
        Assert.DoesNotContain(project.Descendants("SignAssembly"), setting =>
            string.Equals(setting.Value.Trim(), "true", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(project.Descendants("IsPackable"), setting =>
            string.Equals(setting.Value.Trim(), "false", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ButtonGroupTests_LoadToolkitAssembly()
    {
        Assert.Equal("Syncfusion.Blazor.Toolkit", typeof(SfButtonGroup).Assembly.GetName().Name);
        Assert.DoesNotContain(typeof(TestProjectContractTests).Assembly.GetReferencedAssemblies(), reference =>
            reference.Name is not null &&
            reference.Name.StartsWith("Syncfusion.Blazor", StringComparison.Ordinal) &&
            reference.Name != "Syncfusion.Blazor.Toolkit");
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "src/Syncfusion.Blazor.Toolkit.csproj")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Cannot locate the Toolkit source project from the test output.");
    }
}
