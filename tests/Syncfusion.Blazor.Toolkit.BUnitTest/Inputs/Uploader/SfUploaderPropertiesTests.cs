using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using Syncfusion.Blazor.Toolkit.Tests;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.Uploader
{
    /// <summary>
    /// Test class for SfUploader API properties and parameters.
    /// Tests all public properties, nested components, and parameter binding.
    /// </summary>
    /// <remarks>
    /// Feature Group: API names (public properties/parameters & cascading)
    /// </remarks>
    public class SfUploaderPropertiesTests : BunitTestContext
    {
        #region Basic Properties

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_AllowedExtensions_RendersInInputAcceptAttribute()
        {
            // Arrange
            const string extensions = ".jpg,.png,.pdf";

            // Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowedExtensions, extensions));

            // Assert
            Assert.Equal(extensions, uploader.Instance.AllowedExtensions);
            
            var input = uploader.Find("input");
            Assert.Equal(extensions, input.GetAttribute("accept"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_AllowedExtensions_UpdatesDynamically()
        {
            // Arrange - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowedExtensions, ".jpg"));

            // Act
            uploader.SetParametersAndRender(parameters => parameters
                .Add(p => p.AllowedExtensions, ".pdf,.docx"));

            // Assert
            Assert.Equal(".pdf,.docx", uploader.Instance.AllowedExtensions);
            
            var input = uploader.Find("input");
            Assert.Equal(".pdf,.docx", input.GetAttribute("accept"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_AutoUpload_HidesActionButtons_WhenTrue()
        {
            // Arrange & Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AutoUpload, true));

            // Assert
            Assert.True(uploader.Instance.AutoUpload);
            
            // Action buttons should not be visible in AutoUpload mode
            var actionButtons = uploader.FindAll(".e-upload-actions");
            Assert.Empty(actionButtons);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_AutoUpload_ShowsActionButtons_WhenFalse()
        {
            // Arrange & Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AutoUpload, false));

            // Assert
            Assert.False(uploader.Instance.AutoUpload);
            
            // Verify markup contains action buttons (they may be hidden without files)
            Assert.Contains("e-file-upload-btn", uploader.Markup);
            Assert.Contains("e-file-clear-btn", uploader.Markup);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_AllowMultiple_AddsMultipleAttributeToInput()
        {
            // Arrange & Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowMultiple, true));

            // Assert
            Assert.True(uploader.Instance.AllowMultiple);
            
            var input = uploader.Find("input");
            Assert.Equal("multiple", input.GetAttribute("multiple"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_AllowMultiple_RemovesMultipleAttribute_WhenFalse()
        {
            // Arrange & Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowMultiple, false));

            // Assert
            Assert.False(uploader.Instance.AllowMultiple);
            
            var input = uploader.Find("input");
            Assert.Null(input.GetAttribute("multiple"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_DirectoryUpload_AddsDirectoryAttributes()
        {
            // Arrange & Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DirectoryUpload, true));

            // Assert
            Assert.True(uploader.Instance.DirectoryUpload);
            
            var input = uploader.Find("input");
            Assert.Equal("true", input.GetAttribute("directory"));
            Assert.Equal("true", input.GetAttribute("webkitdirectory"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_DirectoryUpload_RemovesDirectoryAttributes_WhenDisabled()
        {
            // Arrange - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DirectoryUpload, true));

            // Act
            uploader.SetParametersAndRender(parameters => parameters
                .Add(p => p.DirectoryUpload, false));

            // Assert
            Assert.False(uploader.Instance.DirectoryUpload);
            
            var input = uploader.Find("input");
            Assert.Null(input.GetAttribute("directory"));
            Assert.Null(input.GetAttribute("webkitdirectory"));
        }

        #endregion

        #region Visual/Style Properties

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_CssClass_AppliesCustomClasses()
        {
            // Arrange
            const string customClass = "my-custom-uploader custom-style";

            // Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.CssClass, customClass));

            // Assert
            Assert.Equal(customClass, uploader.Instance.CssClass);
            
            var container = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            Assert.NotNull(container);
            Assert.Contains("my-custom-uploader", container!.ClassName);
            Assert.Contains("custom-style", container.ClassName);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_CssClass_UpdatesClassesDynamically()
        {
            // Arrange - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.CssClass, "initial-class"));

            // Act
            uploader.SetParametersAndRender(parameters => parameters
                .Add(p => p.CssClass, "updated-class"));

            // Assert
            Assert.Equal("updated-class", uploader.Instance.CssClass);
            
            var container = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            Assert.NotNull(container);
            Assert.Contains("updated-class", container!.ClassName);
            Assert.DoesNotContain("initial-class", container.ClassName);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_Enabled_DisablesComponent_AddsDisabledClass()
        {
            // Arrange & Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Disabled, true));

            // Assert
            Assert.True(uploader.Instance.Disabled);
            
            var container = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            var input = uploader.Find("input");
            
            Assert.NotNull(container);
            Assert.Contains("e-disabled", container!.ClassName);
            Assert.Equal("disabled", input.GetAttribute("disabled"));
            Assert.Equal("true", input.GetAttribute("aria-disabled"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_Enabled_EnablesComponent_RemovesDisabledClass()
        {
            // Arrange - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Disabled, true));

            // Act
            uploader.SetParametersAndRender(parameters => parameters
                .Add(p => p.Disabled, false));

            // Assert
            Assert.False(uploader.Instance.Disabled);
            
            var container = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            var input = uploader.Find("input");
            
            Assert.NotNull(container);
            Assert.DoesNotContain("e-disabled", container!.ClassName);
            Assert.Null(input.GetAttribute("disabled"));
            Assert.Null(input.GetAttribute("aria-disabled"));
        }

        #endregion

        #region Validation Properties

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_MaxFileSize_PropertyValue()
        {
            // Arrange
            const double maxSize = 5242880; // 5 MB

            // Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.MaxFileSize, maxSize));

            // Assert
            Assert.Equal(maxSize, uploader.Instance.MaxFileSize);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_MinFileSize_PropertyValue()
        {
            // Arrange
            const double minSize = 1024; // 1 KB

            // Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.MinFileSize, minSize));

            // Assert
            Assert.Equal(minSize, uploader.Instance.MinFileSize);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_EnableHtmlSanitizer_DefaultValue()
        {
            // Arrange & Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>();

            // Assert
            Assert.True(uploader.Instance.EnableHtmlSanitizer);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_EnableHtmlSanitizer_CanBeDisabled()
        {
            // Arrange & Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.EnableHtmlSanitizer, false));

            // Assert
            Assert.False(uploader.Instance.EnableHtmlSanitizer);
        }

        #endregion

        #region Advanced Properties

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_DropArea_CustomSelector()
        {
            // Arrange
            const string dropAreaSelector = "#myDropZone";

            // Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropArea, dropAreaSelector));

            // Assert
            Assert.Equal(dropAreaSelector, uploader.Instance.DropArea);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_DropEffect_AllEnumValues()
        {
            // Act & Assert - Feature Group: API names
            
            // Default
            var uploaderDefault = RenderComponent<SfUploader>();
            Assert.Equal(DropEffect.Default, uploaderDefault.Instance.DropEffect);

            // Copy
            var uploaderCopy = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Copy));
            Assert.Equal(DropEffect.Copy, uploaderCopy.Instance.DropEffect);

            // Move
            var uploaderMove = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Move));
            Assert.Equal(DropEffect.Move, uploaderMove.Instance.DropEffect);

            // Link
            var uploaderLink = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Link));
            Assert.Equal(DropEffect.Link, uploaderLink.Instance.DropEffect);

            // None
            var uploaderNone = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.None));
            Assert.Equal(DropEffect.None, uploaderNone.Instance.DropEffect);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_ShowFileList_HidesFileList_WhenFalse()
        {
            // Arrange & Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ShowFileList, false));

            // Assert
            Assert.False(uploader.Instance.ShowFileList);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_ShowProgressBar_DefaultValue()
        {
            // Arrange & Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>();

            // Assert
            Assert.True(uploader.Instance.ShowProgressBar);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_ShowProgressBar_CanBeDisabled()
        {
            // Arrange & Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ShowProgressBar, false));

            // Assert
            Assert.False(uploader.Instance.ShowProgressBar);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_SequentialUpload_PropertyValue()
        {
            // Arrange & Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.SequentialUpload, true));

            // Assert
            Assert.True(uploader.Instance.SequentialUpload);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_EnablePersistence_PropertyValue()
        {
            // Arrange & Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.EnablePersistence, true));

            // Assert
            Assert.True(uploader.Instance.EnablePersistence);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_TabIndex_SetsCorrectValue()
        {
            // Arrange
            const int tabIndex = 5;

            // Act - Feature Group: API names
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.TabIndex, tabIndex));

            // Assert
            Assert.Equal(tabIndex, uploader.Instance.TabIndex);
            
            var browseButton = uploader.Find("button.e-upload-browse-btn");
            Assert.Equal(tabIndex.ToString(), browseButton.GetAttribute("tabindex"));
        }

        #endregion

        #region Complex Properties

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_HtmlAttributes_AppliesAllAttributes()
        {
            // Arrange - Feature Group: API names
            var htmlAttributes = new Dictionary<string, object>
            {
                { "name", "fileUploader" },
                { "required", "true" },
                { "data-test", "uploader-test" }
            };

            // Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.HtmlAttributes, htmlAttributes));

            // Assert
            var input = uploader.Find("input");
            Assert.Equal("fileUploader", input.GetAttribute("name"));
            Assert.Equal("true", input.GetAttribute("required"));
            Assert.Equal("uploader-test", input.GetAttribute("data-test"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_InputAttributes_AppliesAllAttributes()
        {
            // Arrange - Feature Group: API names
            var inputAttributes = new Dictionary<string, object>
            {
                { "placeholder", "Select files" },
                { "data-custom", "custom-value" }
            };

            // Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.InputAttributes, inputAttributes));

            // Assert
            var input = uploader.Find("input");
            Assert.Equal("Select files", input.GetAttribute("placeholder"));
            Assert.Equal("custom-value", input.GetAttribute("data-custom"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_HttpClientInstance_UsesCustomInstance()
        {
            // Arrange - Feature Group: API names
            var customHttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.test.com/")
            };

            // Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.HttpClientInstance, customHttpClient));

            // Assert
            Assert.NotNull(uploader.Instance.HttpClientInstance);
            Assert.Equal(customHttpClient.BaseAddress, uploader.Instance.HttpClientInstance.BaseAddress);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_IDProperty()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ID, "customUploaderID"));

            // Assert
            Assert.Equal("customUploaderID", uploader.Instance.ID);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_IDProperty_DefaultGenerated()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>();

            // Assert
            Assert.NotNull(uploader.Instance.ID);
            Assert.NotEmpty(uploader.Instance.ID);
            Assert.StartsWith("uploader", uploader.Instance.ID);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_HttpClientInstance_Default()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>();

            // Assert
            Assert.NotNull(uploader.Instance.HttpClientInstance);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_HttpClientInstance_CustomInstance()
        {
            // Arrange
            var customHttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.example.com/")
            };

            // Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.HttpClientInstance, customHttpClient));

            // Assert
            Assert.NotNull(uploader.Instance.HttpClientInstance);
            Assert.Equal(customHttpClient.BaseAddress, uploader.Instance.HttpClientInstance.BaseAddress);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_AllDropEffectValues()
        {
            // Test DropEffect.Copy
            var uploaderCopy = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Copy));
            Assert.Equal(DropEffect.Copy, uploaderCopy.Instance.DropEffect);

            // Test DropEffect.Move
            var uploaderMove = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Move));
            Assert.Equal(DropEffect.Move, uploaderMove.Instance.DropEffect);

            // Test DropEffect.Link
            var uploaderLink = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Link));
            Assert.Equal(DropEffect.Link, uploaderLink.Instance.DropEffect);

            // Test DropEffect.None
            var uploaderNone = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.None));
            Assert.Equal(DropEffect.None, uploaderNone.Instance.DropEffect);

            // Test DropEffect.Default
            var uploaderDefault = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Default));
            Assert.Equal(DropEffect.Default, uploaderDefault.Instance.DropEffect);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_ChangeDropEffect()
        {
            // Arrange
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Default));

            // Act
            uploader.SetParametersAndRender(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Copy));

            // Assert
            Assert.Equal(DropEffect.Copy, uploader.Instance.DropEffect);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_AllowMultiple_True()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowMultiple, true));

            var inputElement = uploader.Find("input");

            // Assert
            Assert.True(uploader.Instance.AllowMultiple);
            Assert.True(inputElement.HasAttribute("multiple"));
        }

        [Fact(Timeout = 10000)]
        public void Uploader_AllowMultiple_False()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowMultiple, false));

            var inputElement = uploader.Find("input");

            // Assert
            Assert.False(uploader.Instance.AllowMultiple);
            Assert.False(inputElement.HasAttribute("multiple"));
        }

        [Fact(Timeout = 10000)]
        public void Uploader_DirectoryUpload_True()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DirectoryUpload, true));

            var inputElement = uploader.Find("input");

            // Assert
            Assert.True(uploader.Instance.DirectoryUpload);
            Assert.True(inputElement.HasAttribute("directory") || inputElement.HasAttribute("webkitdirectory"));
        }

        [Fact(Timeout = 10000)]
        public void Uploader_DirectoryUpload_False()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DirectoryUpload, false));

            var inputElement = uploader.Find("input");

            // Assert
            Assert.False(uploader.Instance.DirectoryUpload);
            Assert.False(inputElement.HasAttribute("directory"));
            Assert.False(inputElement.HasAttribute("webkitdirectory"));
        }

        [Fact(Timeout = 10000)]
        public void Uploader_AllowedExtensions_SingleExtension()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowedExtensions, ".pdf"));

            var inputElement = uploader.Find("input");

            // Assert
            Assert.Equal(".pdf", uploader.Instance.AllowedExtensions);
            Assert.Equal(".pdf", inputElement.GetAttribute("accept"));
        }

        [Fact(Timeout = 10000)]
        public void Uploader_AllowedExtensions_MultipleExtensions()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowedExtensions, ".jpg,.png,.gif,.bmp"));

            var inputElement = uploader.Find("input");

            // Assert
            Assert.Equal(".jpg,.png,.gif,.bmp", uploader.Instance.AllowedExtensions);
            Assert.Equal(".jpg,.png,.gif,.bmp", inputElement.GetAttribute("accept"));
        }

        [Fact(Timeout = 10000)]
        public void Uploader_AllowedExtensions_MimeTypes()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowedExtensions, "image/*,application/pdf"));

            var inputElement = uploader.Find("input");

            // Assert
            Assert.Equal("image/*,application/pdf", uploader.Instance.AllowedExtensions);
            Assert.Equal("image/*,application/pdf", inputElement.GetAttribute("accept"));
        }

        [Fact(Timeout = 10000)]
        public void Uploader_MinFileSize_Validation()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.MinFileSize, 1024)); // 1 KB

            // Assert
            Assert.Equal(1024, uploader.Instance.MinFileSize);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_MaxFileSize_Validation()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.MaxFileSize, 10 * 1024 * 1024)); // 10 MB

            // Assert
            Assert.Equal(10 * 1024 * 1024, uploader.Instance.MaxFileSize);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_FileSizeRange()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.MinFileSize, 1024) // 1 KB
                .Add(p => p.MaxFileSize, 50 * 1024 * 1024)); // 50 MB

            // Assert
            Assert.Equal(1024, uploader.Instance.MinFileSize);
            Assert.Equal(50 * 1024 * 1024, uploader.Instance.MaxFileSize);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_AutoUpload_True()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AutoUpload, true));

            // Assert
            Assert.True(uploader.Instance.AutoUpload);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_AutoUpload_False()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AutoUpload, false));

            // Assert
            Assert.False(uploader.Instance.AutoUpload);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_SequentialUpload_True()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.SequentialUpload, true));

            // Assert
            Assert.True(uploader.Instance.SequentialUpload);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_SequentialUpload_False()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.SequentialUpload, false));

            // Assert
            Assert.False(uploader.Instance.SequentialUpload);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_ShowFileList_True()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ShowFileList, true));

            // Assert
            Assert.True(uploader.Instance.ShowFileList);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_ShowFileList_False()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ShowFileList, false));

            // Assert
            Assert.False(uploader.Instance.ShowFileList);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_ShowProgressBar_Default()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>();

            // Assert
            Assert.True(uploader.Instance.ShowProgressBar);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_ShowProgressBar_False()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ShowProgressBar, false));

            // Assert
            Assert.False(uploader.Instance.ShowProgressBar);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_EnableHtmlSanitizer_Default()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>();

            // Assert
            Assert.True(uploader.Instance.EnableHtmlSanitizer);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_EnableHtmlSanitizer_False()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.EnableHtmlSanitizer, false));

            // Assert
            Assert.False(uploader.Instance.EnableHtmlSanitizer);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_EnablePersistence_True()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.EnablePersistence, true));

            // Assert
            Assert.True(uploader.Instance.EnablePersistence);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_EnablePersistence_False()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.EnablePersistence, false));

            // Assert
            Assert.False(uploader.Instance.EnablePersistence);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_MultiplePropertiesCombination()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ID, "testUploader")
                .Add(p => p.AutoUpload, false)
                .Add(p => p.AllowMultiple, true)
                .Add(p => p.SequentialUpload, true)
                .Add(p => p.ShowProgressBar, true)
                .Add(p => p.EnableHtmlSanitizer, false)
                .Add(p => p.DirectoryUpload, false)
                .Add(p => p.AllowedExtensions, ".pdf,.doc,.docx")
                .Add(p => p.MinFileSize, 1024)
                .Add(p => p.MaxFileSize, 10485760) // 10 MB
                .Add(p => p.DropEffect, DropEffect.Copy));

            // Assert
            Assert.Equal("testUploader", uploader.Instance.ID);
            Assert.False(uploader.Instance.AutoUpload);
            Assert.True(uploader.Instance.AllowMultiple);
            Assert.True(uploader.Instance.SequentialUpload);
            Assert.True(uploader.Instance.ShowProgressBar);
            Assert.False(uploader.Instance.EnableHtmlSanitizer);
            Assert.False(uploader.Instance.DirectoryUpload);
            Assert.Equal(".pdf,.doc,.docx", uploader.Instance.AllowedExtensions);
            Assert.Equal(1024, uploader.Instance.MinFileSize);
            Assert.Equal(10485760, uploader.Instance.MaxFileSize);
            Assert.Equal(DropEffect.Copy, uploader.Instance.DropEffect);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_UpdateMultipleProperties()
        {
            // Arrange
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AutoUpload, true)
                .Add(p => p.AllowMultiple, false));

            // Act
            uploader.SetParametersAndRender(parameters => parameters
                .Add(p => p.AutoUpload, false)
                .Add(p => p.AllowMultiple, true));

            // Assert
            Assert.False(uploader.Instance.AutoUpload);
            Assert.True(uploader.Instance.AllowMultiple);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_DropArea_CustomSelector()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropArea, "#customDropZone"));

            // Assert
            Assert.Equal("#customDropZone", uploader.Instance.DropArea);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_DropArea_MultipleSelectors()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropArea, "#zone1, #zone2, .drop-area"));

            // Assert
            Assert.Equal("#zone1, #zone2, .drop-area", uploader.Instance.DropArea);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_TabIndex_CustomValue()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.TabIndex, 5));

            var buttonElement = uploader.Find("button");

            // Assert
            Assert.Equal(5, uploader.Instance.TabIndex);
            Assert.Equal("5", buttonElement.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000)]
        public void Uploader_TabIndex_NegativeValue()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.TabIndex, -1));

            var inputElement = uploader.Find("input");

            // Assert
            Assert.Equal(-1, uploader.Instance.TabIndex);
            Assert.Equal("-1", inputElement.GetAttribute("tabindex"));
        }

        #endregion

        #region Nested Components - UploaderButtons

        [Fact(Timeout = 10000)]
        [Trait("Category", "API Names")]
        public void Test_UploaderButtons_CustomTexts()
        {
            var uploaderButtons = RenderComponent<UploaderButtons>(parameters => parameters
             .Add(b => b.Browse, "Choose")
             .Add(b => b.Clear, "Remove")
             .Add(b => b.Upload, "Send"));

            // Assert
            Assert.NotNull(uploaderButtons.Instance);
            Assert.Equal("Choose", uploaderButtons.Instance.Browse);
            Assert.Equal("Remove", uploaderButtons.Instance.Clear);
            Assert.Equal("Send", uploaderButtons.Instance.Upload);
        }

        #endregion

        #region Uploader Integration Tests

        [Fact(Timeout = 10000)]
        public void Uploader_CompleteConfiguration()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ID, "fullConfigUploader")
                .Add(p => p.AutoUpload, false)
                .Add(p => p.AllowMultiple, true)
                .Add(p => p.SequentialUpload, false)
                .Add(p => p.ShowFileList, true)
                .Add(p => p.ShowProgressBar, true)
                .Add(p => p.EnableHtmlSanitizer, true)
                .Add(p => p.DirectoryUpload, false)
                .Add(p => p.Disabled, false)
                .Add(p => p.EnablePersistence, false)
                .Add(p => p.AllowedExtensions, ".pdf,.doc,.docx,.jpg,.png")
                .Add(p => p.MinFileSize, 1024) // 1 KB
                .Add(p => p.MaxFileSize, 10 * 1024 * 1024) // 10 MB
                .Add(p => p.DropEffect, DropEffect.Copy)
                .Add(p => p.DropArea, "#dropZone")
                .Add(p => p.CssClass, "custom-uploader")
                .Add(p => p.TabIndex, 1));

            // Assert component properties
            Assert.Equal("fullConfigUploader", uploader.Instance.ID);
            Assert.False(uploader.Instance.AutoUpload);
            Assert.True(uploader.Instance.AllowMultiple);
            Assert.False(uploader.Instance.SequentialUpload);
            Assert.True(uploader.Instance.ShowFileList);
            Assert.True(uploader.Instance.ShowProgressBar);
            Assert.True(uploader.Instance.EnableHtmlSanitizer);
            Assert.False(uploader.Instance.DirectoryUpload);
            Assert.False(uploader.Instance.Disabled);
            Assert.False(uploader.Instance.EnablePersistence);
            Assert.Equal(".pdf,.doc,.docx,.jpg,.png", uploader.Instance.AllowedExtensions);
            Assert.Equal(1024, uploader.Instance.MinFileSize);
            Assert.Equal(10 * 1024 * 1024, uploader.Instance.MaxFileSize);
            Assert.Equal(DropEffect.Copy, uploader.Instance.DropEffect);
            Assert.Equal("#dropZone", uploader.Instance.DropArea);
            Assert.Equal("custom-uploader", uploader.Instance.CssClass);
            Assert.Equal(1, uploader.Instance.TabIndex);

            var uploaderButtons = RenderComponent<UploaderButtons>(parameters => parameters
                .Add(b => b.Browse, "Select Files")
                .Add(b => b.Clear, "Clear All")
                .Add(b => b.Upload, "Upload Files"));

            // Assert buttons configuration
            Assert.NotNull(uploaderButtons.Instance);
            Assert.Equal("Select Files", uploaderButtons.Instance.Browse);
            Assert.Equal("Clear All", uploaderButtons.Instance.Clear);
            Assert.Equal("Upload Files", uploaderButtons.Instance.Upload);


            var uploaderAsyncSettings = RenderComponent<UploaderAsyncSettings>(parameters => parameters
                .Add(a => a.SaveUrl, "/api/upload/save")
                .Add(a => a.RemoveUrl, "/api/upload/remove")
                .Add(a => a.ChunkSize, 1024 * 1024) // 1 MB chunks
                .Add(a => a.RetryCount, 3)
                .Add(a => a.RetryAfterDelay, 1000));

            // Assert async settings
            Assert.NotNull(uploaderAsyncSettings.Instance);
            Assert.Equal("/api/upload/save", uploaderAsyncSettings.Instance.SaveUrl);
            Assert.Equal("/api/upload/remove", uploaderAsyncSettings.Instance.RemoveUrl);
            Assert.Equal(1024 * 1024, uploaderAsyncSettings.Instance.ChunkSize);
            Assert.Equal(3, uploaderAsyncSettings.Instance.RetryCount);
            Assert.Equal(1000, uploaderAsyncSettings.Instance.RetryAfterDelay);

            // Assert HTML rendering
            var containerEle = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            Assert.NotNull(containerEle);
            Assert.Contains("custom-uploader", containerEle!.ClassName);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_WithPreloadedFilesAndConfiguration()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AutoUpload, true)
                .Add(p => p.AllowMultiple, true));

            var uploaderAsyncSettings = RenderComponent<UploaderAsyncSettings>(parameters => parameters
                .Add(a => a.SaveUrl, "/api/upload")
                .Add(a => a.RemoveUrl, "/api/remove"));

            var uploadedFiles = RenderComponent<UploadedFiles>(parameters => parameters
                .AddChildContent<UploadedFile>(file1 => file1
                    .Add(f => f.Name, "Document1")
                    .Add(f => f.Size, 1024000)
                    .Add(f => f.Type, "pdf"))
                .AddChildContent<UploadedFile>(file2 => file2
                    .Add(f => f.Name, "Image1")
                    .Add(f => f.Size, 512000)
                    .Add(f => f.Type, "jpg")));

            // Assert
            Assert.True(uploader.Instance.AutoUpload);
            Assert.True(uploader.Instance.AllowMultiple);
            Assert.NotNull(uploaderAsyncSettings.Instance.SaveUrl);
            Assert.NotNull(uploaderAsyncSettings.Instance.RemoveUrl);
            Assert.NotNull(uploadedFiles.Instance);
            Assert.Equal(2, uploadedFiles.Instance.Files.Count);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_DisabledState_WithConfiguration()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Disabled, true)
                .Add(p => p.AllowMultiple, true)
                .Add(p => p.AutoUpload, false)
                .Add(p => p.CssClass, "disabled-uploader"));

            var containerEle = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            var inputEle = uploader.Find("input");

            // Assert
            Assert.True(uploader.Instance.Disabled);
            Assert.NotNull(containerEle);
            Assert.Contains("e-disabled", containerEle!.ClassName);
            Assert.Contains("disabled-uploader", containerEle.ClassName);
            Assert.True(inputEle.HasAttribute("disabled"));
        }

        [Fact(Timeout = 10000)]
        public void Uploader_WithConfiguration()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowMultiple, true)
                .Add(p => p.AllowedExtensions, ".pdf,.doc")
                .Add(p => p.CssClass, "rtl-uploader"));

            var containerEle = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;

            // Assert
            Assert.NotNull(containerEle);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_ChunkUpload_Configuration()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AutoUpload, true)
                .AddChildContent<UploaderAsyncSettings>(async => async
                    .Add(a => a.SaveUrl, "/api/chunk/upload")
                    .Add(a => a.ChunkSize, 2 * 1024 * 1024)
                    .Add(a => a.RetryCount, 5)
                    .Add(a => a.RetryAfterDelay, 2000)));

            var uploaderAsyncSettings = RenderComponent<UploaderAsyncSettings>(parameters => parameters
            .Add(a => a.SaveUrl, "/api/chunk/upload")
            .Add(a => a.ChunkSize, 2 * 1024 * 1024)
            .Add(a => a.RetryCount, 5)
            .Add(a => a.RetryAfterDelay, 2000));

            // Assert
            Assert.NotNull(uploaderAsyncSettings.Instance);
            Assert.Equal(2 * 1024 * 1024, uploaderAsyncSettings.Instance.ChunkSize);
            Assert.Equal(5, uploaderAsyncSettings.Instance.RetryCount);
            Assert.Equal(2000, uploaderAsyncSettings.Instance.RetryAfterDelay);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_FileValidation_Configuration()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AllowedExtensions, ".jpg,.jpeg,.png,.gif")
                .Add(p => p.MinFileSize, 10 * 1024) // 10 KB
                .Add(p => p.MaxFileSize, 5 * 1024 * 1024) // 5 MB
                .Add(p => p.EnableHtmlSanitizer, true));

            // Assert
            Assert.Equal(".jpg,.jpeg,.png,.gif", uploader.Instance.AllowedExtensions);
            Assert.Equal(10 * 1024, uploader.Instance.MinFileSize);
            Assert.Equal(5 * 1024 * 1024, uploader.Instance.MaxFileSize);
            Assert.True(uploader.Instance.EnableHtmlSanitizer);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_DirectoryUpload_Configuration()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DirectoryUpload, true)
                .Add(p => p.AllowMultiple, true)
                .Add(p => p.AutoUpload, false));

            var inputEle = uploader.Find("input");

            // Assert
            Assert.True(uploader.Instance.DirectoryUpload);
            Assert.True(uploader.Instance.AllowMultiple);
            Assert.False(uploader.Instance.AutoUpload);
            Assert.True(inputEle.HasAttribute("directory") || inputEle.HasAttribute("webkitdirectory"));
        }

        [Fact(Timeout = 10000)]
        public void Uploader_SequentialUpload_Configuration()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.SequentialUpload, true)
                .Add(p => p.AllowMultiple, true)
                .Add(p => p.AutoUpload, true)
                .AddChildContent<UploaderAsyncSettings>(async => async
                    .Add(a => a.SaveUrl, "/api/sequential/upload")));

            // Assert
            Assert.True(uploader.Instance.SequentialUpload);
            Assert.True(uploader.Instance.AllowMultiple);
            Assert.True(uploader.Instance.AutoUpload);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_HtmlAttributes_Integration()
        {
            // Arrange
            var htmlAttributes = new Dictionary<string, object>
            {
                { "name", "fileUpload" },
                { "required", "true" },
                { "class", "custom-class" },
                { "data-testid", "uploader-test" }
            };

            // Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.HtmlAttributes, htmlAttributes)
                .Add(p => p.CssClass, "additional-class"));

            var containerEle = uploader.Find("input").ParentElement?.ParentElement?.ParentElement;
            var inputEle = uploader.Find("input");

            // Assert
            Assert.NotNull(containerEle);
            Assert.Contains("custom-class", containerEle!.ClassName);
            Assert.Contains("additional-class", containerEle.ClassName);
            Assert.Equal("fileUpload", inputEle.GetAttribute("name"));
            Assert.Equal("true", inputEle.GetAttribute("required"));
        }

        [Fact(Timeout = 10000)]
        public void Uploader_InputAttributes_Integration()
        {
            // Arrange
            var inputAttributes = new Dictionary<string, object>
            {
                { "name", "customInput" },
                { "data-upload", "true" },
                { "aria-label", "File upload input" }
            };

            // Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.InputAttributes, inputAttributes));

            var inputEle = uploader.Find("input");

            // Assert
            Assert.Equal("customInput", inputEle.GetAttribute("name"));
            Assert.Equal("true", inputEle.GetAttribute("data-upload"));
            Assert.Equal("File upload input", inputEle.GetAttribute("aria-label"));
        }

        [Fact(Timeout = 10000)]
        public void Uploader_AllEvents_Configuration()
        {
            // Arrange
            var createdCalled = false;

            // Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Created, (object args) => { createdCalled = true; }));

            // Assert - Created should be called immediately
            Assert.True(createdCalled);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_CustomButtons_WithEvents()
        {
            // Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AutoUpload, false));

            var uploaderButtons = RenderComponent<UploaderButtons>(parameters => parameters
            .Add(b => b.Browse, "Choose")
            .Add(b => b.Clear, "Remove")
            .Add(b => b.Upload, "Send"));

            // Assert
            Assert.NotNull(uploaderButtons.Instance);
            Assert.Equal("Choose", uploaderButtons.Instance.Browse);
            Assert.Equal("Remove", uploaderButtons.Instance.Clear);
            Assert.Equal("Send", uploaderButtons.Instance.Upload);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_DynamicPropertyUpdates()
        {
            // Arrange
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Disabled, false)
                .Add(p => p.AutoUpload, true)
                .Add(p => p.AllowMultiple, false));

            // Act - Update multiple properties
            uploader.SetParametersAndRender(parameters => parameters
                .Add(p => p.Disabled, true)
                .Add(p => p.AutoUpload, false)
                .Add(p => p.AllowMultiple, true));

            // Assert
            Assert.True(uploader.Instance.Disabled);
            Assert.False(uploader.Instance.AutoUpload);
            Assert.True(uploader.Instance.AllowMultiple);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_AllDropEffects_Scenarios()
        {
            // Test Copy
            var uploaderCopy = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Copy)
                .Add(p => p.DropArea, "#dropZone"));
            Assert.Equal(DropEffect.Copy, uploaderCopy.Instance.DropEffect);

            // Test Move
            var uploaderMove = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Move)
                .Add(p => p.DropArea, "#moveZone"));
            Assert.Equal(DropEffect.Move, uploaderMove.Instance.DropEffect);

            // Test Link
            var uploaderLink = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Link)
                .Add(p => p.DropArea, "#linkZone"));
            Assert.Equal(DropEffect.Link, uploaderLink.Instance.DropEffect);

            // Test None
            var uploaderNone = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.None));
            Assert.Equal(DropEffect.None, uploaderNone.Instance.DropEffect);
        }

        [Fact(Timeout = 10000)]
        public void Uploader_MaximumConfiguration_StressTest()
        {
            // Arrange & Act - Test with maximum number of configurations
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ID, "stressTestUploader")
                .Add(p => p.AutoUpload, false)
                .Add(p => p.AllowMultiple, true)
                .Add(p => p.SequentialUpload, true)
                .Add(p => p.ShowFileList, true)
                .Add(p => p.ShowProgressBar, true)
                .Add(p => p.EnableHtmlSanitizer, true)
                .Add(p => p.DirectoryUpload, true)
                .Add(p => p.Disabled, false)
                .Add(p => p.EnablePersistence, true)
                .Add(p => p.AllowedExtensions, ".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.txt,.jpg,.jpeg,.png,.gif,.bmp,.zip,.rar")
                .Add(p => p.MinFileSize, 100)
                .Add(p => p.MaxFileSize, 100 * 1024 * 1024) // 100 MB
                .Add(p => p.DropEffect, DropEffect.Copy)
                .Add(p => p.DropArea, "#largeDropZone, #alternateZone")
                .Add(p => p.CssClass, "stress-test-class another-class")
                .Add(p => p.TabIndex, 10)
                .AddChildContent<UploaderButtons>(buttons => buttons
                    .Add(b => b.Browse, "Select Multiple Files")
                    .Add(b => b.Clear, "Clear All Files")
                    .Add(b => b.Upload, "Upload All Files"))
                .AddChildContent<UploaderAsyncSettings>(async => async
                    .Add(a => a.SaveUrl, "/api/v2/upload/save")
                    .Add(a => a.RemoveUrl, "/api/v2/upload/remove")
                    .Add(a => a.ChunkSize, 5 * 1024 * 1024) // 5 MB
                    .Add(a => a.RetryCount, 10)
                    .Add(a => a.RetryAfterDelay, 5000)));

            var uploadedFiles = RenderComponent<UploadedFiles>(parameters => parameters
            .AddChildContent<UploadedFile>(f1 => f1
            .Add(f => f.Name, "PreloadedDoc1")
            .Add(f => f.Size, 2048000)
            .Add(f => f.Type, "pdf"))
            .AddChildContent<UploadedFile>(f2 => f2
            .Add(f => f.Name, "PreloadedImage1")
            .Add(f => f.Size, 1024000)
            .Add(f => f.Type, "jpg")));

            // Assert - Component should handle all configurations without errors
            Assert.NotNull(uploader.Instance);
            Assert.Equal("stressTestUploader", uploader.Instance.ID);
            Assert.True(uploader.Instance.DirectoryUpload);
            Assert.True(uploader.Instance.DirectoryUpload);
            Assert.Equal(2, uploadedFiles.Instance.Files.Count);
        }

        #endregion
    }
}
