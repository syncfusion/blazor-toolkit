using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using Syncfusion.Blazor.Toolkit.Tests;
using FileInfo = Syncfusion.Blazor.Toolkit.Inputs.FileInfo;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.Uploader
{
    /// <summary>
    /// Test class for SfUploader public methods.
    /// Tests all public async methods with various parameter combinations.
    /// </summary>
    /// <remarks>
    /// Feature Group: Public methods
    /// </remarks>
    public class SfUploaderPublicMethodsTests : BunitTestContext
    {
        #region CancelAsync

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_CancelAsync_WithoutParameters()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();

            // Act & Assert - Should not throw exception
            await uploader.Instance.CancelAsync();
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_CancelAsync_WithFileData()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.CancelAsync(fileData);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_CancelAsync_WithMultipleFiles()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 },
                new FileInfo { Name = "file2.jpg", Size = 2048 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.CancelAsync(fileData);
        }

        #endregion

        #region ClearAllAsync

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_ClearAllAsync_ClientSide()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();

            // Act & Assert - Should not throw exception
            await uploader.Instance.ClearAllAsync();
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_ClearAllAsync_WithAsyncSettings()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .AddChildContent<UploaderAsyncSettings>(settings => settings
                    .Add(s => s.SaveUrl, "/api/upload")
                    .Add(s => s.RemoveUrl, "/api/remove")));

            // Act & Assert - Should not throw exception
            await uploader.Instance.ClearAllAsync();
        }

        #endregion

        #region GetFilesDataAsync

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_GetFilesDataAsync_ReturnsAllFiles()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();

            // Act
            var result = await uploader.Instance.GetFilesDataAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FileInfo>>(result);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_GetFilesDataAsync_ReturnsEmptyList_WhenNoFiles()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();

            // Act
            var result = await uploader.Instance.GetFilesDataAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_GetFilesDataAsync_WithIndex()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();

            // Act
            var result = await uploader.Instance.GetFilesDataAsync(0);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FileInfo>>(result);
        }

        #endregion

        #region PauseAsync

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_PauseAsync_WithoutParameters()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();

            // Act & Assert - Should not throw exception
            await uploader.Instance.PauseAsync();
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_PauseAsync_WithFileData()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new List<FileInfo>
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.PauseAsync(fileData);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_PauseAsync_WithCustomTemplate()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new List<FileInfo>
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.PauseAsync(fileData, custom: true);
        }

        #endregion

        #region RemoveAsync

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_RemoveAsync_WithoutParameters()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();

            // Act & Assert - Should not throw exception
            await uploader.Instance.RemoveAsync();
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_RemoveAsync_WithFileData()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.RemoveAsync(fileData);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_RemoveAsync_WithCustomTemplate()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.RemoveAsync(fileData, customTemplate: true);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_RemoveAsync_WithRemoveDirectly()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.RemoveAsync(fileData, removeDirectly: true);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_RemoveAsync_WithPostRawFile()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.RemoveAsync(fileData, postRawFile: false);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_RemoveAsync_WithAllParameters()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };
            var args = new { CustomKey = "CustomValue" };

            // Act & Assert - Should not throw exception
            await uploader.Instance.RemoveAsync(
                fileData,
                customTemplate: true,
                removeDirectly: false,
                postRawFile: true,
                args: args);
        }

        #endregion

        #region ResumeAsync

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_ResumeAsync_WithoutParameters()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();

            // Act & Assert - Should not throw exception
            await uploader.Instance.ResumeAsync();
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_ResumeAsync_WithFileData()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.ResumeAsync(fileData);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_ResumeAsync_WithCustomTemplate()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.ResumeAsync(fileData, custom: true);
        }

        #endregion

        #region RetryAsync

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_RetryAsync_WithoutParameters()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();

            // Act & Assert - Should not throw exception
            await uploader.Instance.RetryAsync();
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_RetryAsync_WithFileData()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.RetryAsync(fileData);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_RetryAsync_FromCanceledStage()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.RetryAsync(fileData, fromcanceledStage: true);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_RetryAsync_FromInitialStage()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.RetryAsync(fileData, fromcanceledStage: false);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_RetryAsync_WithCustomTemplate()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var fileData = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.RetryAsync(fileData, custom: true);
        }

        #endregion

        #region UploadAsync

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_UploadAsync_WithoutParameters()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();

            // Act & Assert - Should not throw exception
            await uploader.Instance.UploadAsync();
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_UploadAsync_WithSpecificFiles()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var files = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.UploadAsync(files);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_UploadAsync_WithCustomTemplate()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var files = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.UploadAsync(files, custom: true);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Public Methods")]
        public async Task Test_UploadAsync_WithMultipleFiles()
        {
            // Arrange - Feature Group: Public methods
            var uploader = RenderComponent<SfUploader>();
            var files = new FileInfo[]
            {
                new FileInfo { Name = "file1.pdf", Size = 1024 },
                new FileInfo { Name = "file2.jpg", Size = 2048 },
                new FileInfo { Name = "file3.png", Size = 3072 }
            };

            // Act & Assert - Should not throw exception
            await uploader.Instance.UploadAsync(files);
        }

        #endregion
    }
}
