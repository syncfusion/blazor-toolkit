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
    /// Drag & Drop related tests for <see cref="SfUploader"/>.
    /// </summary>
    /// <remarks>
    /// Feature Group: Drag & Drop
    /// </remarks>
    public class SfUploaderDragDropTests : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        [Trait("Category", "Drag & Drop")]
        public void DropArea_ExternalSelector_IsRespected()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropArea, "#externalZone"));

            // Assert
            Assert.Equal("#externalZone", uploader.Instance.DropArea);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Drag & Drop")]
        public async Task SelectedEvent_Preserves_FileSource_Drop()
        {
            // Arrange
            var args = new SelectedEventArgs
            {
                FilesData = new List<FileInfo>
                {
                    new FileInfo { Name = "dragged.png", Size = 2048, FileSource = "drop" }
                }
            };

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.DropEffect, DropEffect.Copy));

            // Act
            var result = await uploader.Instance.SelectedEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.FilesData);
            Assert.Equal("drop", result.FilesData[0].FileSource);
            Assert.Equal(DropEffect.Copy, uploader.Instance.DropEffect);
        }
    }
}
