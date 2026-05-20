using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using Syncfusion.Blazor.Toolkit.Tests;
using FileInfo = Syncfusion.Blazor.Toolkit.Inputs.FileInfo;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.Uploader
{
    /// <summary>
    /// Simulated end-to-end flow tests for <see cref="SfUploader"/>.
    /// Verifies ordering across core events in auto and manual modes.
    /// </summary>
    /// <remarks>
    /// Feature Group: Upload flow (simulated E2E)
    /// </remarks>
    public class SfUploaderUploadFlowTests : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        [Trait("Category", "Upload Flow")]
        public async Task AutoUpload_True_Events_Order()
        {
            // Arrange
            var calls = new List<string>();
            var file = new FileInfo { Name = "auto.bin", Size = 2048 };

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AutoUpload, true)
                .Add(p => p.Created, (object _) => calls.Add("Created"))
                .Add(p => p.OnUploadStart, (UploadingEventArgs _) => calls.Add("Uploading"))
                .Add(p => p.Progressing, (ProgressEventArgs _) => calls.Add("Progress"))
                .Add(p => p.Success, (SuccessEventArgs _) => calls.Add("Success"))
                .Add(p => p.OnActionComplete, (ActionCompleteEventArgs _) => calls.Add("ActionComplete")));

            // Act - simulate lifecycle by invoking JSInvokable bridges directly
            await uploader.Instance.UploadingEventAsync(new UploadingEventArgs { FileData = file });
            await uploader.Instance.ProgressEventAsync(new ProgressEventArgs { File = file, Total = 100, Loaded = 50, LengthComputable = true });
            await uploader.Instance.SuccessEventAsync(new SuccessEventArgs { File = file, Operation = "upload" });
            await uploader.Instance.ActionCompleteEventAsync(new ActionCompleteEventArgs { FileData = new List<FileInfo> { file } });

            // Assert - created first, then Uploading -> Progress -> Success -> ActionComplete
            var expected = new[] { "Created", "Uploading", "Progress", "Success", "ActionComplete" };
            Assert.True(expected.SequenceEqual(calls));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Upload Flow")]
        public async Task ManualUpload_False_Events_Order()
        {
            // Arrange
            var calls = new List<string>();
            var file = new FileInfo { Name = "manual.bin", Size = 1024 };

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.AutoUpload, false)
                .Add(p => p.Created, (object _) => calls.Add("Created"))
                .Add(p => p.OnUploadStart, (UploadingEventArgs _) => calls.Add("Uploading"))
                .Add(p => p.Progressing, (ProgressEventArgs _) => calls.Add("Progress"))
                .Add(p => p.Success, (SuccessEventArgs _) => calls.Add("Success"))
                .Add(p => p.OnActionComplete, (ActionCompleteEventArgs _) => calls.Add("ActionComplete")));

            // Action buttons markup should exist in manual mode
            Assert.Contains("e-file-upload-btn", uploader.Markup);
            Assert.Contains("e-file-clear-btn", uploader.Markup);

            // Act - emulate the flow post user-click on Upload
            await uploader.Instance.UploadingEventAsync(new UploadingEventArgs { FileData = file });
            await uploader.Instance.ProgressEventAsync(new ProgressEventArgs { File = file, Total = 100, Loaded = 100, LengthComputable = true });
            await uploader.Instance.SuccessEventAsync(new SuccessEventArgs { File = file, Operation = "upload" });
            await uploader.Instance.ActionCompleteEventAsync(new ActionCompleteEventArgs { FileData = new List<FileInfo> { file } });

            // Assert
            var expected = new[] { "Created", "Uploading", "Progress", "Success", "ActionComplete" };
            Assert.True(expected.SequenceEqual(calls));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Upload Flow")]
        public async Task ProgressEvent_Allowed_When_ShowProgressBar_False()
        {
            // Arrange
            var file = new FileInfo { Name = "nobar.png", Size = 5000 };
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ShowProgressBar, false));

            // Act & Assert - Should not throw when reporting progress
            await uploader.Instance.ProgressEventAsync(new ProgressEventArgs
            {
                File = file,
                Total = 100,
                Loaded = 10,
                LengthComputable = true
            });

            Assert.False(uploader.Instance.ShowProgressBar);
        }

        [Fact(Timeout = 10000)]
        public void Dispose_DoesNotThrow()
        {
            // Arrange
            var uploader = RenderComponent<SfUploader>();

            // Act / Assert - disposing the rendered component should not throw
            uploader.Dispose();
        }

        [Fact(Timeout = 10000)]
        public void HtmlSanitizer_EscapesOrRemoves_RawHtmlInPreloadedNames()
        {
            // Arrange - provide a preloaded file whose name contains HTML/script
            var maliciousName = "<script>alert('x')</script>evil.jpg";

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.EnableHtmlSanitizer, true)
                .Add(p => p.ShowFileList, true)
                .AddChildContent<UploaderFiles>(files => files
                    .AddChildContent<UploaderUploadedFile>(file => file
                        .Add(f => f.Name, maliciousName)
                        .Add(f => f.Size, 1234)
                        .Add(f => f.Type, "jpg")))) ;

            // Act - read rendered markup
            var markup = uploader.Markup;

            // Assert - raw script tag should not appear in markup
            Assert.DoesNotContain("<script", markup);
            Assert.DoesNotContain(maliciousName, markup);
        }

        [Fact(Timeout = 10000)]
        public async Task ProgressEvent_LargeValues_HandledWithoutException()
        {
            // Arrange
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.ShowProgressBar, true));

            // Act & Assert - send a very large progress event and ensure no exception
            var args = new ProgressEventArgs
            {
                Total = int.MaxValue,
                Loaded = int.MaxValue - 1,
                LengthComputable = true
            };

            await uploader.Instance.ProgressEventAsync(args);
        }
    }
}
