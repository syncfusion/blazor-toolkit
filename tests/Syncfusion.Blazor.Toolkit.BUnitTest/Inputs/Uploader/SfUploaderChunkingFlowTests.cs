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
    /// Chunk upload lifecycle tests for <see cref="SfUploader"/>.
    /// Covers chunk start/success/failure sequencing, retry, and pause/resume.
    /// </summary>
    /// <remarks>
    /// Feature Group: Chunking flow
    /// </remarks>
    public class SfUploaderChunkingFlowTests : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        [Trait("Category", "Chunking Flow")]
        public async Task ChunkSequence_StartsAndCompletes_InOrder()
        {
            // Arrange
            var order = new List<string>();
            var file = new FileInfo { Name = "chunked.iso", Size = 10_000_000 };

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .AddChildContent<UploaderAsyncSettings>(async => async
                    .Add(a => a.SaveUrl, "/api/upload")
                    .Add(a => a.ChunkSize, 1024 * 1024))
                .Add(p => p.OnChunkUploadStart, (UploadingEventArgs e) => order.Add($"Start:{e.CurrentChunkIndex}"))
                .Add(p => p.OnChunkSuccess, (SuccessEventArgs e) => order.Add($"Success:{e.ChunkIndex}")));

            // Act - simulate three chunks
            await uploader.Instance.ChunkUploadingEventAsync(new UploadingEventArgs { FileData = file, CurrentChunkIndex = 0, ChunkSize = 1024 * 1024 });
            await uploader.Instance.ChunkSuccessEventAsync(new SuccessEventArgs { File = file, ChunkIndex = 0, ChunkSize = 1024 * 1024 });

            await uploader.Instance.ChunkUploadingEventAsync(new UploadingEventArgs { FileData = file, CurrentChunkIndex = 1, ChunkSize = 1024 * 1024 });
            await uploader.Instance.ChunkSuccessEventAsync(new SuccessEventArgs { File = file, ChunkIndex = 1, ChunkSize = 1024 * 1024 });

            await uploader.Instance.ChunkUploadingEventAsync(new UploadingEventArgs { FileData = file, CurrentChunkIndex = 2, ChunkSize = 1024 * 1024 });
            await uploader.Instance.ChunkSuccessEventAsync(new SuccessEventArgs { File = file, ChunkIndex = 2, ChunkSize = 1024 * 1024 });

            // Assert
            var expected = new[] { "Start:0", "Success:0", "Start:1", "Success:1", "Start:2", "Success:2" };
            Assert.True(expected.SequenceEqual(order));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Chunking Flow")]
        public async Task ChunkFailure_TriggersRetry_Path()
        {
            // Arrange
            var failed = false;
            var file = new FileInfo { Name = "retry.bin", Size = 2000000 };

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnChunkFailure, (FailureEventArgs _) => failed = true));

            // Act - simulate failure then retry from canceled stage
            await uploader.Instance.ChunkFailureEventAsync(new FailureEventArgs { File = file, ChunkIndex = 3, ChunkSize = 512 * 1024, StatusText = "Timeout" });
            await uploader.Instance.RetryAsync(new[] { file }, fromcanceledStage: true);

            // Assert - failure callback observed and retry did not throw
            Assert.True(failed);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Chunking Flow")]
        public async Task PauseResume_MidChunk_Sequencing()
        {
            // Arrange
            var timeline = new List<string>();
            var file = new FileInfo { Name = "video.mp4", Size = 50_000_000 };

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Paused, (PauseResumeEventArgs e) => timeline.Add($"Paused@{e.ChunkIndex}"))
                .Add(p => p.OnResume, (PauseResumeEventArgs e) => timeline.Add($"Resumed@{e.ChunkIndex}")));

            // Act
            await uploader.Instance.PausingEventAsync(new PauseResumeEventArgs { File = file, ChunkIndex = 5 });
            await uploader.Instance.ResumingEventAsync(new PauseResumeEventArgs { File = file, ChunkIndex = 6 });

            // Assert
            Assert.Equal(new[] { "Paused@5", "Resumed@6" }, timeline);
        }
    }
}
