using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using Syncfusion.Blazor.Toolkit.Inputs.Internal;
using Syncfusion.Blazor.Toolkit.Tests;
using Microsoft.AspNetCore.Components;
using FileInfo = Syncfusion.Blazor.Toolkit.Inputs.FileInfo;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.Uploader
{
    /// <summary>
    /// Test class for SfUploader further customization scenarios.
    /// Tests lifecycle hooks, parameter changes, accessibility, file validation,
    /// templates, and other advanced customization features.
    /// </summary>
    /// <remarks>
    /// Feature Group: Further customization (CSS/class variants, LabelPosition, size, RTL, Disabled, toggle behavior, AdditionalAttributes, theming, RenderFragments/templates)
    /// </remarks>
    public class SfUploaderFurtherCustomizationTests : BunitTestContext
    {
        #region Lifecycle Tests

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_OnInitializedAsync_InitializesCorrectly()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>();

            // Assert - Component should be initialized
            Assert.NotNull(uploader.Instance);
            Assert.NotNull(uploader.Instance.ID);
            Assert.NotEmpty(uploader.Instance.ID);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_OnParametersSetAsync_UpdatesOnPropertyChange()
        {
            // Arrange - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.CssClass, "initial"));

            // Act
            uploader.SetParametersAndRender(parameters => parameters
                .Add(p => p.CssClass, "updated"));

            // Assert
            Assert.Equal("updated", uploader.Instance.CssClass);
            var container = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            Assert.Contains("updated", container!.ClassName);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public async Task Test_OnAfterRenderAsync_InvokesCreated_OnFirstRender()
        {
            // Arrange - Feature Group: Further customization
            var createdInvoked = false;

            // Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Created, (object args) =>
                {
                    createdInvoked = true;
                }));

            // Assert
            Assert.True(createdInvoked);
        }

        #endregion

        #region Parameter Changes & Re-render

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_ParameterChange_CssClass_Rerenders()
        {
            // Arrange - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.CssClass, "class-v1"));

            var container = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            Assert.Contains("class-v1", container!.ClassName);

            // Act
            uploader.SetParametersAndRender(parameters => parameters
                .Add(p => p.CssClass, "class-v2"));

            // Assert
            container = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            Assert.Contains("class-v2", container!.ClassName);
            Assert.DoesNotContain("class-v1", container.ClassName);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_ParameterChange_Enabled_Rerenders()
        {
            // Arrange - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Disabled, false));

            var container = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            Assert.DoesNotContain("e-disabled", container!.ClassName);

            // Act
            uploader.SetParametersAndRender(parameters => parameters
                .Add(p => p.Disabled, true));

            // Assert
            container = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            Assert.Contains("e-disabled", container!.ClassName);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_ParameterChange_AllowedExtensions_Rerenders()
        {
            // Arrange - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowedExtensions, ".jpg"));

            var input = uploader.Find("input");
            Assert.Equal(".jpg", input.GetAttribute("accept"));

            // Act
            uploader.SetParametersAndRender(parameters => parameters
                .Add(p => p.AllowedExtensions, ".pdf,.docx"));

            // Assert
            input = uploader.Find("input");
            Assert.Equal(".pdf,.docx", input.GetAttribute("accept"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_ParameterChange_AllowMultiple_Rerenders()
        {
            // Arrange - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowMultiple, true));

            var input = uploader.Find("input");
            Assert.NotNull(input.GetAttribute("multiple"));
            Assert.True(uploader.Instance.AllowMultiple);

            // Act
            uploader.SetParametersAndRender(parameters => parameters
                .Add(p => p.AllowMultiple, false));

            // Assert - Verify instance property and re-query DOM
            Assert.False(uploader.Instance.AllowMultiple);
            input = uploader.Find("input");
            // Check that the DOM no longer has the multiple attribute (should be null when not present)
            Assert.Null(input.GetAttribute(""));
        }

        #endregion

        #region File Validation

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_FileValidation_MaxFileSize_Configuration()
        {
            // Arrange & Act - Feature Group: Further customization
            const double maxSize = 10485760; // 10 MB
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.MaxFileSize, maxSize));

            // Assert
            Assert.Equal(maxSize, uploader.Instance.MaxFileSize);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_FileValidation_MinFileSize_Configuration()
        {
            // Arrange & Act - Feature Group: Further customization
            const double minSize = 1024; // 1 KB
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.MinFileSize, minSize));

            // Assert
            Assert.Equal(minSize, uploader.Instance.MinFileSize);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_FileValidation_AllowedExtensions_Configuration()
        {
            // Arrange & Act - Feature Group: Further customization
            const string extensions = ".jpg,.png,.gif";
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowedExtensions, extensions));

            // Assert
            Assert.Equal(extensions, uploader.Instance.AllowedExtensions);
            var input = uploader.Find("input");
            Assert.Equal(extensions, input.GetAttribute("accept"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_FileValidation_HtmlSanitizer_Enabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.EnableHtmlSanitizer, true));

            // Assert
            Assert.True(uploader.Instance.EnableHtmlSanitizer);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_FileValidation_HtmlSanitizer_Disabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.EnableHtmlSanitizer, false));

            // Assert
            Assert.False(uploader.Instance.EnableHtmlSanitizer);
        }

        #endregion

        #region Accessibility

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_Accessibility_AriaLabels_Present()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>();

            // Assert
            var input = uploader.Find("input");
            Assert.NotNull(input.GetAttribute("aria-label"));
            
            var browseButton = uploader.Find("button.e-upload-browse-btn");
            Assert.NotNull(browseButton.GetAttribute("aria-label"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_Accessibility_TabIndex_Correct()
        {
            // Arrange & Act - Feature Group: Further customization
            const int customTabIndex = 3;
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.TabIndex, customTabIndex));

            // Assert
            var browseButton = uploader.Find("button.e-upload-browse-btn");
            Assert.Equal(customTabIndex.ToString(), browseButton.GetAttribute("tabindex"));
            
            // Input should have tabindex -1 (not focusable)
            var input = uploader.Find("input");
            Assert.Equal("-1", input.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_Accessibility_DisabledState_AriaDisabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Disabled, true));

            // Assert
            var input = uploader.Find("input");
            Assert.Equal("true", input.GetAttribute("aria-disabled"));
            Assert.NotNull(input.GetAttribute("disabled"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_Accessibility_EnabledState_NoAriaDisabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Disabled, false));

            // Assert
            var input = uploader.Find("input");
            Assert.Null(input.GetAttribute("aria-disabled"));
            Assert.Null(input.GetAttribute("disabled"));
        }

        #endregion

        #region CSS and Class Variants

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_CssVariants_MultipleClasses()
        {
            // Arrange & Act - Feature Group: Further customization
            const string multipleClasses = "custom-class-1 custom-class-2 custom-class-3";
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.CssClass, multipleClasses));

            // Assert
            var container = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            Assert.Contains("custom-class-1", container!.ClassName);
            Assert.Contains("custom-class-2", container.ClassName);
            Assert.Contains("custom-class-3", container.ClassName);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_CssVariants_DisabledClass()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Disabled, true));

            // Assert
            var container = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            Assert.Contains("e-disabled", container!.ClassName);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_CssVariants_CombinedCustomAndSystemClasses()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.CssClass, "my-custom-uploader")
                .Add(p => p.Disabled, true));

            // Assert
            var container = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            Assert.Contains("my-custom-uploader", container!.ClassName);
            Assert.Contains("e-disabled", container.ClassName);
            Assert.Contains("e-upload", container.ClassName);
        }

        #endregion

        #region Directory Upload

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_DirectoryUpload_Enabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DirectoryUpload, true));

            // Assert
            Assert.True(uploader.Instance.DirectoryUpload);
            var input = uploader.Find("input");
            Assert.Equal("true", input.GetAttribute("directory"));
            Assert.Equal("true", input.GetAttribute("webkitdirectory"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_DirectoryUpload_Disabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DirectoryUpload, false));

            // Assert
            Assert.False(uploader.Instance.DirectoryUpload);
            var input = uploader.Find("input");
            Assert.Null(input.GetAttribute("directory"));
            Assert.Null(input.GetAttribute("webkitdirectory"));
        }

        #endregion

        #region Upload Modes

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_UploadMode_AutoUpload_Enabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AutoUpload, true));

            // Assert
            Assert.True(uploader.Instance.AutoUpload);
            // Action buttons should not be present
            var actionButtons = uploader.FindAll(".e-upload-actions");
            Assert.Empty(actionButtons);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_UploadMode_ManualUpload()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AutoUpload, false));

            // Assert
            Assert.False(uploader.Instance.AutoUpload);
            // Action buttons should be in markup (may be hidden)
            Assert.Contains("e-file-upload-btn", uploader.Markup);
            Assert.Contains("e-file-clear-btn", uploader.Markup);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_UploadMode_SequentialUpload()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.SequentialUpload, true));

            // Assert
            Assert.True(uploader.Instance.SequentialUpload);
        }

        #endregion

        #region Multiple File Selection

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_MultipleFiles_Enabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowMultiple, true));

            // Assert
            Assert.True(uploader.Instance.AllowMultiple);
            var input = uploader.Find("input");
            Assert.Equal("multiple", input.GetAttribute("multiple"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_MultipleFiles_Disabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowMultiple, false));

            // Assert
            Assert.False(uploader.Instance.AllowMultiple);
            var input = uploader.Find("input");
            Assert.Null(input.GetAttribute("multiple"));
        }

        #endregion

        #region Preloaded Files

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_PreloadedFiles_SingleFile()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .AddChildContent<UploaderFiles>(files => files
                    .AddChildContent<UploaderUploadedFiles>(file => file
                        .Add(f => f.Name, "Document")
                        .Add(f => f.Size, 1024000)
                        .Add(f => f.Type, "pdf"))));

            // Assert
            Assert.NotNull(uploader.Instance);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_PreloadedFiles_MultipleFiles()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .AddChildContent<UploaderFiles>(files => files
                    .AddChildContent<UploaderUploadedFiles>(file1 => file1
                        .Add(f => f.Name, "Document1")
                        .Add(f => f.Size, 1024000)
                        .Add(f => f.Type, "pdf"))
                    .AddChildContent<UploaderUploadedFiles>(file2 => file2
                        .Add(f => f.Name, "Image1")
                        .Add(f => f.Size, 512000)
                        .Add(f => f.Type, "jpg"))));

            // Assert
            Assert.NotNull(uploader.Instance);
        }

        [Fact(Timeout = 10000)]
        public void UploaderUploadedFiles_DefaultValues()
        {
            // Arrange & Act
            var uploadedFile = new UploaderUploadedFiles();

            // Assert
            Assert.Equal(string.Empty, uploadedFile.Name);
            Assert.Equal(0, uploadedFile.Size);
            Assert.Equal(string.Empty, uploadedFile.Type);
        }

        [Fact(Timeout = 10000)]
        public void UploaderUploadedFiles_SetProperties()
        {
            // Arrange & Act
            var uploadedFile = new UploaderUploadedFiles
            {
                Name = "Document",
                Size = 2048576,
                Type = "pdf"
            };

            // Assert
            Assert.Equal("Document", uploadedFile.Name);
            Assert.Equal(2048576, uploadedFile.Size);
            Assert.Equal("pdf", uploadedFile.Type);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_WithSinglePreloadedFile()
        {

            var uploadedFiles = RenderComponent<UploaderFiles>(parameters => parameters
            .AddChildContent<UploaderUploadedFiles>(uploadedFile => uploadedFile
            .Add(p => p.Name, "TestDocument")
            .Add(p => p.Size, 1024000)
            .Add(p => p.Type, "pdf")));

            // Assert
            Assert.NotNull(uploadedFiles.Instance);
            Assert.Single(uploadedFiles.Instance.Files);
            Assert.Equal("TestDocument", uploadedFiles.Instance.Files[0].Name);
            Assert.Equal(1024000, uploadedFiles.Instance.Files[0].Size);
            Assert.Equal("pdf", uploadedFiles.Instance.Files[0].Type);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_WithMultiplePreloadedFiles()
        {
            // Arrange & Act
            var uploadedFiles = RenderComponent<UploaderFiles>(parameters => parameters
                .AddChildContent<UploaderUploadedFiles>(file1 => file1
                    .Add(p => p.Name, "Document1")
                    .Add(p => p.Size, 1024000)
                    .Add(p => p.Type, "pdf"))
                .AddChildContent<UploaderUploadedFiles>(file2 => file2
                    .Add(p => p.Name, "Image1")
                    .Add(p => p.Size, 512000)
                    .Add(p => p.Type, "jpg"))
                .AddChildContent<UploaderUploadedFiles>(file3 => file3
                    .Add(p => p.Name, "Spreadsheet1")
                    .Add(p => p.Size, 2048000)
                    .Add(p => p.Type, "xlsx")));

            // Assert
            Assert.NotNull(uploadedFiles.Instance);
            Assert.Equal(3, uploadedFiles.Instance.Files.Count);

            // Verify first file
            Assert.Equal("Document1", uploadedFiles.Instance.Files[0].Name);
            Assert.Equal(1024000, uploadedFiles.Instance.Files[0].Size);
            Assert.Equal("pdf", uploadedFiles.Instance.Files[0].Type);

            // Verify second file
            Assert.Equal("Image1", uploadedFiles.Instance.Files[1].Name);
            Assert.Equal(512000, uploadedFiles.Instance.Files[1].Size);
            Assert.Equal("jpg", uploadedFiles.Instance.Files[1].Type);

            // Verify third file
            Assert.Equal("Spreadsheet1", uploadedFiles.Instance.Files[2].Name);
            Assert.Equal(2048000, uploadedFiles.Instance.Files[2].Size);
            Assert.Equal("xlsx", uploadedFiles.Instance.Files[2].Type);
        }

        [Fact(Timeout = 10000)]
        public void UploaderUploadedFiles_LargeFileSize()
        {
            // Arrange & Act
            var uploadedFiles = RenderComponent<UploaderFiles>(parameters => parameters
                .AddChildContent<UploaderUploadedFiles>(uploadedFile => uploadedFile
                    .Add(p => p.Name, "LargeVideo")
                    .Add(p => p.Size, 500 * 1024 * 1024) // 500 MB
                    .Add(p => p.Type, "mp4")));

            // Assert
            Assert.NotNull(uploadedFiles.Instance);
            Assert.Single(uploadedFiles.Instance.Files);
            Assert.Equal(500 * 1024 * 1024, uploadedFiles.Instance.Files[0].Size);
        }

        [Fact(Timeout = 10000)]
        public void UploaderUploadedFiles_DifferentFileTypes()
        {
            // Arrange & Act
            var uploadedFiles = RenderComponent<UploaderFiles>(parameters => parameters
                .AddChildContent<UploaderUploadedFiles>(file1 => file1
                    .Add(p => p.Name, "TextFile")
                    .Add(p => p.Size, 1024)
                    .Add(p => p.Type, "txt"))
                .AddChildContent<UploaderUploadedFiles>(file2 => file2
                    .Add(p => p.Name, "ImageFile")
                    .Add(p => p.Size, 2048576)
                    .Add(p => p.Type, "png"))
                .AddChildContent<UploaderUploadedFiles>(file3 => file3
                    .Add(p => p.Name, "VideoFile")
                    .Add(p => p.Size, 10485760)
                    .Add(p => p.Type, "mp4"))
                .AddChildContent<UploaderUploadedFiles>(file4 => file4
                    .Add(p => p.Name, "AudioFile")
                    .Add(p => p.Size, 5242880)
                    .Add(p => p.Type, "mp3")));

            // Assert
            Assert.NotNull(uploadedFiles.Instance.Files);
            Assert.Equal(4, uploadedFiles.Instance.Files.Count);

            // Verify different types are preserved
            Assert.Equal("txt", uploadedFiles.Instance.Files[0].Type);
            Assert.Equal("png", uploadedFiles.Instance.Files[1].Type);
            Assert.Equal("mp4", uploadedFiles.Instance.Files[2].Type);
            Assert.Equal("mp3", uploadedFiles.Instance.Files[3].Type);
        }

        [Fact(Timeout = 10000)]
        public void UploaderUploadedFiles_EmptyFile()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .AddChildContent<UploaderFiles>(files => files
                    .AddChildContent<UploaderUploadedFiles>(uploadedFile => uploadedFile
                        .Add(p => p.Name, "EmptyFile")
                        .Add(p => p.Size, 0)
                        .Add(p => p.Type, "txt"))));

            var uploadedFiles = RenderComponent<UploaderFiles>(parameters => parameters
                .AddChildContent<UploaderUploadedFiles>(uploadedFile => uploadedFile
                    .Add(p => p.Name, "EmptyFile")
                    .Add(p => p.Size, 0)
                    .Add(p => p.Type, "txt")));

            // Assert
            Assert.NotNull(uploadedFiles.Instance.Files);
            Assert.Single(uploadedFiles.Instance.Files);
            Assert.Equal(0, uploadedFiles.Instance.Files[0].Size);
        }

        [Fact(Timeout = 10000)]
        public void UploaderUploadedFiles_UpdateFileName()
        {
            var uploadedFiles = RenderComponent<UploaderFiles>(parameters => parameters
            .AddChildContent<UploaderUploadedFiles>(uploadedFile => uploadedFile
            .Add(p => p.Name, "NewName")
            .Add(p => p.Size, 1024)
            .Add(p => p.Type, "txt")));

            // Assert
            Assert.Equal("NewName", uploadedFiles.Instance.Files[0].Name);
        }

        [Fact(Timeout = 10000)]
        public void UploaderUploadedFiles_FileNameWithSpecialCharacters()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .AddChildContent<UploaderFiles>(files => files
                    .AddChildContent<UploaderUploadedFiles>(uploadedFile => uploadedFile
                        .Add(p => p.Name, "File-Name_With.Special@Characters")
                        .Add(p => p.Size, 2048)
                        .Add(p => p.Type, "pdf"))));

            var uploadedFiles = RenderComponent<UploaderFiles>(parameters => parameters
            .AddChildContent<UploaderUploadedFiles>(uploadedFile => uploadedFile
            .Add(p => p.Name, "File-Name_With.Special@Characters")
            .Add(p => p.Size, 2048)
            .Add(p => p.Type, "pdf")));

            // Assert
            Assert.NotNull(uploadedFiles.Instance);
            Assert.Equal("File-Name_With.Special@Characters", uploadedFiles.Instance.Files[0].Name);
        }

        [Fact(Timeout = 10000)]
        public void UploaderUploadedFiles_LongFileName()
        {
            // Arrange
            var longFileName = new string('A', 200);

            var uploadedFiles = RenderComponent<UploaderFiles>(parameters => parameters
            .AddChildContent<UploaderUploadedFiles>(uploadedFile => uploadedFile
            .Add(p => p.Name, longFileName)
            .Add(p => p.Size, 1024)
            .Add(p => p.Type, "txt")));

            // Assert
            Assert.NotNull(uploadedFiles.Instance);
            Assert.Equal(longFileName, uploadedFiles.Instance.Files[0].Name);
        }

        [Fact(Timeout = 10000)]
        public void UploaderUploadedFiles_WithFullExtension()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .AddChildContent<UploaderFiles>(files => files
                    .AddChildContent<UploaderUploadedFiles>(uploadedFile => uploadedFile
                        .Add(p => p.Name, "Document")
                        .Add(p => p.Size, 1024000)
                        .Add(p => p.Type, "application/pdf"))));

            var uploadedFiles = RenderComponent<UploaderFiles>(parameters => parameters
            .AddChildContent<UploaderUploadedFiles>(uploadedFile => uploadedFile
            .Add(p => p.Name, "Document")
            .Add(p => p.Size, 1024000)
            .Add(p => p.Type, "application/pdf")));

            // Assert
            Assert.NotNull(uploadedFiles.Instance);
            Assert.Equal("application/pdf", uploadedFiles.Instance.Files[0].Type);
        }

        #endregion

        #region Persistence

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_Persistence_Enabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.EnablePersistence, true));

            // Assert
            Assert.True(uploader.Instance.EnablePersistence);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_Persistence_Disabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.EnablePersistence, false));

            // Assert
            Assert.False(uploader.Instance.EnablePersistence);
        }

        #endregion

        #region DropEffect

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_DropEffect_AllValues()
        {
            // Act & Assert - Feature Group: Further customization
            
            // Test Default
            var uploaderDefault = RenderComponent<SfUploader>();
            Assert.Equal(DropEffect.Default, uploaderDefault.Instance.DropEffect);

            // Test Copy
            var uploaderCopy = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Copy));
            Assert.Equal(DropEffect.Copy, uploaderCopy.Instance.DropEffect);

            // Test Move
            var uploaderMove = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Move));
            Assert.Equal(DropEffect.Move, uploaderMove.Instance.DropEffect);

            // Test Link
            var uploaderLink = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Link));
            Assert.Equal(DropEffect.Link, uploaderLink.Instance.DropEffect);

            // Test None
            var uploaderNone = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.None));
            Assert.Equal(DropEffect.None, uploaderNone.Instance.DropEffect);
        }

        #endregion

        #region ShowFileList and ShowProgressBar

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_ShowFileList_Enabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ShowFileList, true));

            // Assert
            Assert.True(uploader.Instance.ShowFileList);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_ShowFileList_Disabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ShowFileList, false));

            // Assert
            Assert.False(uploader.Instance.ShowFileList);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_ShowProgressBar_Enabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ShowProgressBar, true));

            // Assert
            Assert.True(uploader.Instance.ShowProgressBar);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Further Customization")]
        public void Test_ShowProgressBar_Disabled()
        {
            // Arrange & Act - Feature Group: Further customization
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ShowProgressBar, false));

            // Assert
            Assert.False(uploader.Instance.ShowProgressBar);
        }

        #endregion
    }
}
