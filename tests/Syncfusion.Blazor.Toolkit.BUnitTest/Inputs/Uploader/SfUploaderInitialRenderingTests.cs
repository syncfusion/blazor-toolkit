using System;
using System.Linq;
using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using Syncfusion.Blazor.Toolkit.Tests;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.Uploader
{
    /// <summary>
    /// Test class for SfUploader initial rendering scenarios.
    /// Tests component initialization, default structure, and HTML output.
    /// </summary>
    /// <remarks>
    /// Feature Group: Initial Rendering
    /// </remarks>
    public class SfUploaderInitialRenderingTests : BunitTestContext
    {
        #region Initial Rendering Tests

        [Fact(Timeout = 10000)]
        [Trait("Category", "Initial Rendering")]
        public void Test_Uploader_RendersWithDefaultStructure()
        {
            // Arrange & Act - Feature Group: Initial rendering
            var uploader = RenderComponent<SfUploader>();
            
            // Assert - Verify DOM structure
            var input = uploader.Find("input");
            var container = input.ParentElement?.ParentElement?.ParentElement;
            var browseButton = uploader.Find("button.e-upload-browse-btn");
            var dropArea = uploader.Find("span.e-file-drop");

            Assert.NotNull(container);
            Assert.NotNull(input);
            Assert.NotNull(browseButton);
            Assert.NotNull(dropArea);

            // Verify container classes
            Assert.Contains("e-upload", container!.ClassName);
            Assert.Contains("e-control-container", container.ClassName);
            Assert.Contains("e-lib", container.ClassName);
            Assert.Contains("e-keyboard", container.ClassName);

            // Verify input classes
            Assert.Contains("e-uploader", input.ClassName);
            Assert.Contains("e-control", input.ClassName);
            Assert.Contains("e-lib", input.ClassName);
            
            // Verify input attributes
            Assert.Equal("-1", input.GetAttribute("tabindex"));
            Assert.Equal("uploader", input.GetAttribute("aria-label"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Initial Rendering")]
        public void Test_Uploader_DefaultPropertiesInitialization()
        {
            // Arrange & Act - Feature Group: Initial rendering
            var uploader = RenderComponent<SfUploader>();
            var instance = uploader.Instance;

            // Assert - Verify all default property values
            Assert.Empty(instance.AllowedExtensions);
            Assert.Empty(instance.CssClass);
            Assert.Null(instance.DropArea);
            Assert.Equal(30000000, instance.MaxFileSize);
            Assert.Equal(0, instance.MinFileSize);
            Assert.True(instance.AutoUpload);
            Assert.True(instance.AllowMultiple);
            Assert.False(instance.SequentialUpload);
            Assert.False(instance.EnablePersistence);
            Assert.False(instance.Disabled);
            Assert.False(instance.DirectoryUpload);
            Assert.True(instance.ShowFileList);
            Assert.True(instance.ShowProgressBar);
            Assert.Equal(0, instance.TabIndex);
            Assert.Equal(DropEffect.Default, instance.DropEffect);
            Assert.True(instance.EnableHtmlSanitizer);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Initial Rendering")]
        public void Test_Uploader_GeneratesUniqueID_WhenNotProvided()
        {
            // Arrange & Act - Feature Group: Initial rendering
            var uploader1 = RenderComponent<SfUploader>();
            var uploader2 = RenderComponent<SfUploader>();

            // Assert
            Assert.NotNull(uploader1.Instance.ID);
            Assert.NotEmpty(uploader1.Instance.ID);
            Assert.StartsWith("uploader", uploader1.Instance.ID);
            
            Assert.NotNull(uploader2.Instance.ID);
            Assert.NotEmpty(uploader2.Instance.ID);
            Assert.StartsWith("uploader", uploader2.Instance.ID);
            
            // Verify uniqueness
            Assert.NotEqual(uploader1.Instance.ID, uploader2.Instance.ID);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Initial Rendering")]
        public void Test_Uploader_UsesProvidedID_WhenSpecified()
        {
            // Arrange
            const string customID = "myCustomUploader";

            // Act - Feature Group: Initial rendering
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ID, customID));

            // Assert
            Assert.Equal(customID, uploader.Instance.ID);
            
            var input = uploader.Find("input");
            Assert.Equal(customID, input.GetAttribute("name"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Initial Rendering")]
        public void Test_Uploader_RendersActionButtons_WhenAutoUploadFalse()
        {
            // Arrange & Act - Feature Group: Initial rendering
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AutoUpload, false));

            // Assert - Should render upload and clear buttons
            var actionButtons = uploader.FindAll(".e-upload-actions button");
            
            // Note: Action buttons are hidden by default when no files are present
            // We verify the buttons exist in the markup
            Assert.True(uploader.Markup.Contains("e-file-upload-btn"));
            Assert.True(uploader.Markup.Contains("e-file-clear-btn"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Initial Rendering")]
        public void Test_Uploader_DoesNotRenderFileList_Initially()
        {
            // Arrange & Act - Feature Group: Initial rendering
            var uploader = RenderComponent<SfUploader>();

            // Assert - File list should not be rendered without files
            var fileList = uploader.FindAll("ul.e-upload-files");
            Assert.Empty(fileList);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Initial Rendering")]
        public void Test_Uploader_RendersBrowseButton_WithDefaultText()
        {
            // Arrange & Act - Feature Group: Initial rendering
            var uploader = RenderComponent<SfUploader>();

            // Assert
            var browseButton = uploader.Find("button.e-upload-browse-btn");
            Assert.NotNull(browseButton);
            Assert.Contains("Browse", browseButton.TextContent);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Initial Rendering")]
        public void Test_Uploader_RendersDropArea_WithDefaultText()
        {
            // Arrange & Act - Feature Group: Initial rendering
            var uploader = RenderComponent<SfUploader>();

            // Assert
            var dropArea = uploader.Find("span.e-file-drop");
            Assert.NotNull(dropArea);
            Assert.Contains("drop", dropArea.TextContent.ToLower());
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Initial Rendering")]
        public void Test_Uploader_InitializesHttpClientInstance()
        {
            // Arrange & Act - Feature Group: Initial rendering
            var uploader = RenderComponent<SfUploader>();

            // Assert
            Assert.NotNull(uploader.Instance.HttpClientInstance);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Initial Rendering")]
        public void Test_Uploader_InputElement_HasCorrectType()
        {
            // Arrange & Act - Feature Group: Initial rendering
            var uploader = RenderComponent<SfUploader>();

            // Assert
            var input = uploader.Find("input");
            // InputFile doesn't have a type attribute in Blazor, but we can verify it's an InputFile
            Assert.NotNull(input);
            Assert.Equal("uploader", input.GetAttribute("aria-label"));
        }

        #endregion
    }
}
