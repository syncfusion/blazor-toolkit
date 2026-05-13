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
    /// Validation behavior tests for <see cref="SfUploader"/>.
    /// Focuses on min/max size handling via cancelable events and message propagation.
    /// </summary>
    /// <remarks>
    /// Feature Group: Validation behavior
    /// </remarks>
    public class SfUploaderValidationBehaviorTests : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        [Trait("Category", "Validation Behavior")]
        public async Task BeforeUpload_Cancels_WhenFileSize_TooLarge()
        {
            // Arrange
            var maxSize = 1024d; // 1 KB
            var canceled = false;

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.MaxFileSize, maxSize)
                .Add(p => p.BeforeUpload, (BeforeUploadEventArgs e) =>
                {
                    // Emulate validation based on size
                    if (e.FilesData != null && e.FilesData.Count > 0 && e.FilesData[0].Size > maxSize)
                    {
                        e.Cancel = true;
                    }
                }));

            var args = new BeforeUploadEventArgs
            {
                Cancel = false,
                FilesData = new List<FileInfo>
                {
                    new FileInfo { Name = "Big.bin", Size = 4096 }
                }
            };

            // Act
            var result = await uploader.Instance.BeforeUploadEventAsync(args);
            canceled = result.Cancel;

            // Assert
            Assert.True(canceled);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Validation Behavior")]
        public async Task BeforeUpload_Cancels_WhenFileSize_TooSmall()
        {
            // Arrange
            var minSize = 2048d; // 2 KB

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.MinFileSize, minSize)
                .Add(p => p.BeforeUpload, (BeforeUploadEventArgs e) =>
                {
                    if (e.FilesData != null && e.FilesData.Count > 0 && e.FilesData[0].Size < minSize)
                    {
                        e.Cancel = true;
                    }
                }));

            var args = new BeforeUploadEventArgs
            {
                Cancel = false,
                FilesData = new List<FileInfo>
                {
                    new FileInfo { Name = "Small.txt", Size = 512 }
                }
            };

            // Act
            var result = await uploader.Instance.BeforeUploadEventAsync(args);

            // Assert
            Assert.True(result.Cancel);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Validation Behavior")]
        public async Task BeforeUpload_MixedFiles_CancelsInvalid_AllowsValid()
        {
            // Arrange
            var minSize = 1000d;
            var maxSize = 3000d;

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.MinFileSize, minSize)
                .Add(p => p.MaxFileSize, maxSize)
                .Add(p => p.BeforeUpload, (BeforeUploadEventArgs e) =>
                {
                    if (e.FilesData != null && e.FilesData.Count > 0)
                    {
                        var s = e.FilesData[0].Size;
                        e.Cancel = s < minSize || s > maxSize;
                    }
                }));

            var tooSmall = new BeforeUploadEventArgs
            {
                FilesData = new List<FileInfo> { new FileInfo { Name = "a.bin", Size = 500 } }
            };
            var valid = new BeforeUploadEventArgs
            {
                FilesData = new List<FileInfo> { new FileInfo { Name = "b.bin", Size = 1500 } }
            };
            var tooBig = new BeforeUploadEventArgs
            {
                FilesData = new List<FileInfo> { new FileInfo { Name = "c.bin", Size = 5000 } }
            };

            // Act
            var r1 = await uploader.Instance.BeforeUploadEventAsync(tooSmall);
            var r2 = await uploader.Instance.BeforeUploadEventAsync(valid);
            var r3 = await uploader.Instance.BeforeUploadEventAsync(tooBig);

            // Assert
            Assert.True(r1.Cancel);
            Assert.False(r2.Cancel);
            Assert.True(r3.Cancel);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Validation Behavior")]
        public async Task ValidationMessages_PassThrough_InSelectedEvent()
        {
            // Arrange: provide files with preset validation messages and ensure pass-through via JSInvokable
            var fileWithMessages = new FileInfo
            {
                Name = "invalid.pdf",
                Size = 10,
                ValidationMessages = new ValidationMessages
                {
                    MinSize = "File is too small.",
                    MaxSize = "File is too large."
                }
            };

            var args = new SelectedEventArgs
            {
                FilesData = new List<FileInfo> { fileWithMessages }
            };

            var uploader = RenderComponent<SfUploader>();

            // Act
            var result = await uploader.Instance.SelectedEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.FilesData);
            Assert.Single(result.FilesData);
            Assert.Equal("File is too small.", result.FilesData[0].ValidationMessages.MinSize);
            Assert.Equal("File is too large.", result.FilesData[0].ValidationMessages.MaxSize);
        }
    }
}
