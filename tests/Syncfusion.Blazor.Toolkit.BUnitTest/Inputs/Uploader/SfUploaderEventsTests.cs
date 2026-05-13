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
    /// Test class for SfUploader events.
    /// Tests all event callbacks and their invocation behavior.
    /// </summary>
    /// <remarks>
    /// Feature Group: Events
    /// </remarks>
    public class SfUploaderEventsTests : BunitTestContext
    {
        #region Created Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_Created_FiresOnInitialization()
        {
            // Arrange - Feature Group: Events
            var createdCalled = false;

            // Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Created, (object args) =>
                {
                    createdCalled = true;
                }));

            // Assert
            Assert.True(createdCalled);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_Created_FiresOnlyOnce()
        {
            // Arrange - Feature Group: Events
            var createdCount = 0;

            // Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Created, (object args) =>
                {
                    createdCount++;
                }));

            // Multiple renders should not trigger Created again
            uploader.SetParametersAndRender(parameters => parameters
                .Add(p => p.CssClass, "new-class"));

            // Assert
            Assert.Equal(1, createdCount);
        }

        #endregion

        #region FileSelected Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_FileSelected_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var fileSelectedArgs = default(SelectedEventArgs);

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.FileSelected, (SelectedEventArgs args) =>
                {
                    fileSelectedArgs = args;
                }));

            // Assert
            Assert.True(uploader.Instance.FileSelected.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_FileSelected_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new SelectedEventArgs
            {
                FilesData = new List<FileInfo>
                {
                    new FileInfo { Name = "test.pdf", Size = 1024 }
                }
            };

            // Act
            var result = await uploader.Instance.SelectedEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(args.FilesData, result.FilesData);
        }

        #endregion

        #region OnValueChange Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_OnValueChange_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var valueChangeArgs = default(UploadChangeEventArgs);

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnValueChange, (UploadChangeEventArgs args) =>
                {
                    valueChangeArgs = args;
                }));

            // Assert
            Assert.True(uploader.Instance.OnValueChange.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_OnValueChange_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new UploadChangeEventArgs
            {
                Files = new List<UploadFiles>()
            };

            // Act
            await uploader.Instance.ChangeEventAsync(args);

            // Assert - Should complete without exception
            Assert.NotNull(args);
        }

        #endregion

        #region BeforeUpload Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_BeforeUpload_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var beforeUploadArgs = default(BeforeUploadEventArgs);

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.BeforeUpload, (BeforeUploadEventArgs args) =>
                {
                    beforeUploadArgs = args;
                }));

            // Assert
            Assert.True(uploader.Instance.BeforeUpload.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_BeforeUpload_CanCancelUpload()
        {
            // Arrange - Feature Group: Events
            var args = new BeforeUploadEventArgs
            {
                Cancel = false,
                FilesData = new List<FileInfo>()
            };

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.BeforeUpload, (BeforeUploadEventArgs e) =>
                {
                    e.Cancel = true;
                }));

            // Act
            var result = await uploader.Instance.BeforeUploadEventAsync(args);

            // Assert
            Assert.True(result.Cancel);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_BeforeUpload_CanAddCustomFormData()
        {
            // Arrange - Feature Group: Events
            var customData = new { Token = "abc123" };
            var args = new BeforeUploadEventArgs
            {
                FilesData = new List<FileInfo>()
            };

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.BeforeUpload, (BeforeUploadEventArgs e) =>
                {
                    e.CustomFormData = customData;
                }));

            // Act
            var result = await uploader.Instance.BeforeUploadEventAsync(args);

            // Assert
            Assert.NotNull(result.CustomFormData);
            Assert.Equal(customData, result.CustomFormData);
        }

        #endregion

        #region OnUploadStart Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_OnUploadStart_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnUploadStart, (UploadingEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.OnUploadStart.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_OnUploadStart_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new UploadingEventArgs
            {
                FileData = new FileInfo { Name = "test.pdf" }
            };

            // Act
            var result = await uploader.Instance.UploadingEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(args.FileData, result.FileData);
        }

        #endregion

        #region Progressing Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_Progressing_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Progressing, (ProgressEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.Progressing.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_Progressing_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new ProgressEventArgs
            {
                Loaded = 512,
                Total = 1024,
                LengthComputable = true
            };

            // Act
            var result = await uploader.Instance.ProgressEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(args.Loaded, result.Loaded);
            Assert.Equal(args.Total, result.Total);
        }

        #endregion

        #region Success Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_Success_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Success, (SuccessEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.Success.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_Success_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new SuccessEventArgs
            {
                File = new FileInfo { Name = "test.pdf" },
                Operation = "upload"
            };

            // Act
            var result = await uploader.Instance.SuccessEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(args.File, result.File);
            Assert.Equal(args.Operation, result.Operation);
        }

        #endregion

        #region OnFailure Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_OnFailure_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnFailure, (FailureEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.OnFailure.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_OnFailure_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new FailureEventArgs
            {
                File = new FileInfo { Name = "test.pdf" }
            };

            // Act
            var result = await uploader.Instance.FailureEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(args.File, result.File);
        }

        #endregion

        #region OnActionComplete Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_OnActionComplete_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnActionComplete, (ActionCompleteEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.OnActionComplete.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_OnActionComplete_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new ActionCompleteEventArgs
            {
                FileData = new List<FileInfo>()
            };

            // Act
            var result = await uploader.Instance.ActionCompleteEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.FileData);
        }

        #endregion

        #region BeforeRemove Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_BeforeRemove_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.BeforeRemove, (BeforeRemoveEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.BeforeRemove.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_BeforeRemove_CanCancelRemoval()
        {
            // Arrange - Feature Group: Events
            var args = new BeforeRemoveEventArgs
            {
                Cancel = false,
                FilesData = new List<FileInfo>()
            };

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.BeforeRemove, (BeforeRemoveEventArgs e) =>
                {
                    e.Cancel = true;
                }));

            // Act
            var result = await uploader.Instance.BeforeRemoveEventAsync(args);

            // Assert
            Assert.True(result.Cancel);
        }

        #endregion

        #region OnRemove Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_OnRemove_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnRemove, (RemovingEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.OnRemove.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_OnRemove_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new RemovingEventArgs
            {
                FilesData = new List<FileInfo>()
            };

            // Act
            var result = await uploader.Instance.RemovingEventAsync(args);

            // Assert
            Assert.NotNull(result);
        }

        #endregion

        #region OnClear Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_OnClear_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnClear, (ClearingEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.OnClear.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_OnClear_CanCancelOperation()
        {
            // Arrange - Feature Group: Events
            var args = new ClearingEventArgs
            {
                Cancel = false
            };

            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnClear, (ClearingEventArgs e) =>
                {
                    e.Cancel = true;
                }));

            // Act
            var result = await uploader.Instance.ClearingEventAsync(args);

            // Assert
            Assert.True(result.Cancel);
        }

        #endregion

        #region OnCancel Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_OnCancel_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnCancel, (CancelEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.OnCancel.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_OnCancel_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new CancelEventArgs
            {
                FileData = new FileInfo { Name = "test.pdf" }
            };

            // Act
            var result = await uploader.Instance.CancelingEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(args.FileData, result.FileData);
        }

        #endregion

        #region Paused Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_Paused_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Paused, (PauseResumeEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.Paused.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_Paused_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new PauseResumeEventArgs
            {
                File = new FileInfo { Name = "test.pdf" },
                ChunkIndex = 5
            };

            // Act
            var result = await uploader.Instance.PausingEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(args.File, result.File);
            Assert.Equal(args.ChunkIndex, result.ChunkIndex);
        }

        #endregion

        #region OnResume Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_OnResume_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnResume, (PauseResumeEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.OnResume.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_OnResume_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new PauseResumeEventArgs
            {
                File = new FileInfo { Name = "test.pdf" },
                ChunkIndex = 5
            };

            // Act
            var result = await uploader.Instance.ResumingEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(args.File, result.File);
        }

        #endregion

        #region Chunk Events

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_OnChunkUploadStart_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnChunkUploadStart, (UploadingEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.OnChunkUploadStart.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_OnChunkUploadStart_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new UploadingEventArgs
            {
                FileData = new FileInfo { Name = "test.pdf" },
                ChunkSize = 1024,
                CurrentChunkIndex = 1
            };

            // Act
            var result = await uploader.Instance.ChunkUploadingEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(args.FileData, result.FileData);
            Assert.Equal(args.ChunkSize, result.ChunkSize);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_OnChunkSuccess_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnChunkSuccess, (SuccessEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.OnChunkSuccess.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_OnChunkSuccess_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new SuccessEventArgs
            {
                File = new FileInfo { Name = "test.pdf" },
                ChunkIndex = 1
            };

            // Act
            var result = await uploader.Instance.ChunkSuccessEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(args.File, result.File);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_OnChunkFailure_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnChunkFailure, (FailureEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.OnChunkFailure.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_OnChunkFailure_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new FailureEventArgs
            {
                File = new FileInfo { Name = "test.pdf" },
                ChunkIndex = 1
            };

            // Act
            var result = await uploader.Instance.ChunkFailureEventAsync(args);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(args.File, result.File);
        }

        #endregion

        #region OnFileListRender Event

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public void Test_OnFileListRender_EventCallback_IsConfigured()
        {
            // Arrange & Act - Feature Group: Events
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.OnFileListRender, (FileListRenderingEventArgs args) => { }));

            // Assert
            Assert.True(uploader.Instance.OnFileListRender.HasDelegate);
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "Events")]
        public async Task Test_OnFileListRender_JSInvokable_Method()
        {
            // Arrange - Feature Group: Events
            var uploader = RenderComponent<SfUploader>();
            var args = new FileListRenderingEventArgs
            {
                FileInfo = new FileInfo { Name = "test.pdf" },
                Index = 0
            };

            // Act
            await uploader.Instance.FileListRenderingEventAsync(args);

            // Assert - Should complete without exception
            Assert.NotNull(args);
        }

        #endregion

        #region Event Argument Validation

        [Fact(Timeout = 10000)]
        public void ClearingEventArgs_Properties()
        {
            // Arrange & Act
            var dummyFilesData = new List<FileInfo>
            {
                new FileInfo { Name = "File1.pdf", Size = 1024 },
                new FileInfo { Name = "File2.jpg", Size = 2048 }
            };

            var clearingEventArgs = new ClearingEventArgs
            {
                Cancel = true,
                FilesData = dummyFilesData
            };

            // Assert
            Assert.True(clearingEventArgs.Cancel);
            Assert.NotNull(clearingEventArgs.FilesData);
            Assert.Equal(2, clearingEventArgs.FilesData.Count);
            Assert.Equal("File1.pdf", clearingEventArgs.FilesData[0].Name);
            Assert.Equal("File2.jpg", clearingEventArgs.FilesData[1].Name);
        }

        [Fact(Timeout = 10000)]
        public void ClearingEventArgs_Cancel_False()
        {
            // Arrange & Act
            var clearingEventArgs = new ClearingEventArgs
            {
                Cancel = false,
                FilesData = new List<FileInfo>()
            };

            // Assert
            Assert.False(clearingEventArgs.Cancel);
            Assert.Empty(clearingEventArgs.FilesData);
        }

        [Fact(Timeout = 10000)]
        public void FileListRenderingEventArgs_Properties()
        {
            // Arrange & Act
            var fileInfo = new FileInfo
            {
                Name = "Document.pdf",
                Size = 2048576,
                Type = "pdf"
            };

            var fileListRenderingEventArgs = new FileListRenderingEventArgs
            {
                FileInfo = fileInfo,
                Index = 5,
                IsPreload = true
            };

            // Assert
            Assert.NotNull(fileListRenderingEventArgs.FileInfo);
            Assert.Equal("Document.pdf", fileListRenderingEventArgs.FileInfo.Name);
            Assert.Equal(5, fileListRenderingEventArgs.Index);
            Assert.True(fileListRenderingEventArgs.IsPreload);
        }

        [Fact(Timeout = 10000)]
        public void FileListRenderingEventArgs_IsPreload_False()
        {
            // Arrange & Act
            var fileInfo = new FileInfo
            {
                Name = "NewFile.jpg",
                Size = 512000
            };

            var fileListRenderingEventArgs = new FileListRenderingEventArgs
            {
                FileInfo = fileInfo,
                Index = 0,
                IsPreload = false
            };

            // Assert
            Assert.False(fileListRenderingEventArgs.IsPreload);
            Assert.Equal(0, fileListRenderingEventArgs.Index);
        }

        [Fact(Timeout = 10000)]
        public void UploadingEventArgs_WithCustomFormData()
        {
            // Arrange & Act
            var customFormData = new { Key = "Value", Token = "ABC123" };
            var fileData = new FileInfo { Name = "Test.pdf", Size = 1024 };

            var uploadingEventArgs = new UploadingEventArgs
            {
                Cancel = false,
                ChunkSize = 1048576,
                CurrentChunkIndex = 0,
                FileData = fileData,
                CustomFormData = customFormData
            };

            // Assert
            Assert.False(uploadingEventArgs.Cancel);
            Assert.Equal(1048576, uploadingEventArgs.ChunkSize);
            Assert.Equal(0, uploadingEventArgs.CurrentChunkIndex);
            Assert.NotNull(uploadingEventArgs.FileData);
            Assert.NotNull(uploadingEventArgs.CustomFormData);
        }

        [Fact(Timeout = 10000)]
        public void UploadingEventArgs_LargeChunkIndex()
        {
            // Arrange & Act
            var uploadingEventArgs = new UploadingEventArgs
            {
                Cancel = false,
                ChunkSize = 2097152, // 2 MB
                CurrentChunkIndex = 100,
                FileData = new FileInfo { Name = "LargeFile.zip" }
            };

            // Assert
            Assert.Equal(100, uploadingEventArgs.CurrentChunkIndex);
            Assert.Equal(2097152, uploadingEventArgs.ChunkSize);
        }

        [Fact(Timeout = 10000)]
        public void UploadingEventArgs_Cancel_True()
        {
            // Arrange & Act
            var uploadingEventArgs = new UploadingEventArgs
            {
                Cancel = true,
                ChunkSize = 524288,
                CurrentChunkIndex = 5,
                FileData = new FileInfo { Name = "Cancelled.doc" }
            };

            // Assert
            Assert.True(uploadingEventArgs.Cancel);
        }

        [Fact(Timeout = 10000)]
        public void ValidationMessages_AllProperties()
        {
            // Arrange & Act
            var validationMessages = new ValidationMessages
            {
                MinSize = "File size is too small",
                MaxSize = "File size is too large"
            };

            // Assert
            Assert.Equal("File size is too small", validationMessages.MinSize);
            Assert.Equal("File size is too large", validationMessages.MaxSize);
        }

        [Fact(Timeout = 10000)]
        public void ValidationMessages_MinSizeOnly()
        {
            // Arrange & Act
            var validationMessages = new ValidationMessages
            {
                MinSize = "Minimum size not met"
            };

            // Assert
            Assert.Equal("Minimum size not met", validationMessages.MinSize);
            Assert.Empty(validationMessages.MaxSize);
        }

        [Fact(Timeout = 10000)]
        public void ValidationMessages_MaxSizeOnly()
        {
            // Arrange & Act
            var validationMessages = new ValidationMessages
            {
                MaxSize = "Maximum size exceeded"
            };

            // Assert
            Assert.Equal("Maximum size exceeded", validationMessages.MaxSize);
            Assert.Empty(validationMessages.MinSize);
        }

        [Fact(Timeout = 10000)]
        public void FileInfo_WithValidationMessages()
        {
            // Arrange & Act
            var validationMessages = new ValidationMessages
            {
                MinSize = "Too small",
                MaxSize = "Too large"
            };

            var fileInfo = new FileInfo
            {
                Name = "Test.pdf",
                Size = 1024,
                ValidationMessages = validationMessages
            };

            // Assert
            Assert.NotNull(fileInfo.ValidationMessages);
            Assert.Equal("Too small", fileInfo.ValidationMessages.MinSize);
            Assert.Equal("Too large", fileInfo.ValidationMessages.MaxSize);
        }

        [Fact(Timeout = 10000)]
        public void FileInfo_StatusCodes()
        {
            // Arrange & Act
            var fileInfo1 = new FileInfo { Name = "File1.txt", StatusCode = "0" }; // Ready
            var fileInfo2 = new FileInfo { Name = "File2.txt", StatusCode = "1" }; // Uploading
            var fileInfo3 = new FileInfo { Name = "File3.txt", StatusCode = "2" }; // Uploaded
            var fileInfo4 = new FileInfo { Name = "File4.txt", StatusCode = "3" }; // Failed

            // Assert
            Assert.Equal("0", fileInfo1.StatusCode);
            Assert.Equal("1", fileInfo2.StatusCode);
            Assert.Equal("2", fileInfo3.StatusCode);
            Assert.Equal("3", fileInfo4.StatusCode);
        }

        [Fact(Timeout = 10000)]
        public void FileInfo_FileSource_Types()
        {
            // Arrange & Act
            var fileFromBrowse = new FileInfo { Name = "Browse.txt", FileSource = "browse" };
            var fileFromDrop = new FileInfo { Name = "Drop.txt", FileSource = "drop" };
            var fileFromPaste = new FileInfo { Name = "Paste.txt", FileSource = "paste" };

            // Assert
            Assert.Equal("browse", fileFromBrowse.FileSource);
            Assert.Equal("drop", fileFromDrop.FileSource);
            Assert.Equal("paste", fileFromPaste.FileSource);
        }

        [Fact(Timeout = 10000)]
        public void FileInfo_MimeContentType()
        {
            // Arrange & Act
            var pdfFile = new FileInfo { Name = "Document.pdf", MimeContentType = "application/pdf" };
            var jpgFile = new FileInfo { Name = "Image.jpg", MimeContentType = "image/jpeg" };
            var txtFile = new FileInfo { Name = "Text.txt", MimeContentType = "text/plain" };
            var zipFile = new FileInfo { Name = "Archive.zip", MimeContentType = "application/zip" };

            // Assert
            Assert.Equal("application/pdf", pdfFile.MimeContentType);
            Assert.Equal("image/jpeg", jpgFile.MimeContentType);
            Assert.Equal("text/plain", txtFile.MimeContentType);
            Assert.Equal("application/zip", zipFile.MimeContentType);
        }

        [Fact(Timeout = 10000)]
        public void FileInfo_LastModifiedDate()
        {
            // Arrange
            var testDate = new DateTime(2024, 12, 15, 10, 30, 0);

            // Act
            var fileInfo = new FileInfo
            {
                Name = "Test.doc",
                LastModifiedDate = testDate
            };

            // Assert
            Assert.Equal(testDate, fileInfo.LastModifiedDate);
        }

        [Fact(Timeout = 10000)]
        public void ResponseEventArgs_Properties()
        {
            // Arrange & Act
            var responseEventArgs = new ResponseEventArgs
            {
                Headers = "Content-Type: application/json",
                ReadyState = 4,
                StatusCode = 200,
                StatusText = "OK"
            };

            // Assert
            Assert.Equal("Content-Type: application/json", responseEventArgs.Headers);
            Assert.Equal(4, responseEventArgs.ReadyState);
            Assert.Equal(200, responseEventArgs.StatusCode);
            Assert.Equal("OK", responseEventArgs.StatusText);
        }

        [Fact(Timeout = 10000)]
        public void ResponseEventArgs_ErrorResponse()
        {
            // Arrange & Act
            var responseEventArgs = new ResponseEventArgs
            {
                Headers = "Content-Type: text/plain",
                ReadyState = 4,
                StatusCode = 500,
                StatusText = "Internal Server Error"
            };

            // Assert
            Assert.Equal(500, responseEventArgs.StatusCode);
            Assert.Equal("Internal Server Error", responseEventArgs.StatusText);
        }

        [Fact(Timeout = 10000)]
        public void SuccessEventArgs_WithResponse()
        {
            // Arrange & Act
            var response = new ResponseEventArgs
            {
                StatusCode = 200,
                StatusText = "Success"
            };

            var fileInfo = new FileInfo { Name = "Success.pdf" };

            var successEventArgs = new SuccessEventArgs
            {
                File = fileInfo,
                Operation = "upload",
                Response = response,
                StatusText = "Upload successful",
                ChunkIndex = 10,
                ChunkSize = 1048576,
                TotalChunk = 10
            };

            // Assert
            Assert.NotNull(successEventArgs.Response);
            Assert.Equal(200, successEventArgs.Response.StatusCode);
            Assert.Equal("Success", successEventArgs.Response.StatusText);
            Assert.Equal(10, successEventArgs.ChunkIndex);
            Assert.Equal(1048576, successEventArgs.ChunkSize);
            Assert.Equal(10, successEventArgs.TotalChunk);
        }

        [Fact(Timeout = 10000)]
        public void FailureEventArgs_WithChunkInformation()
        {
            // Arrange & Act
            var failedFile = new FileInfo { Name = "Failed.zip" };

            var failureEventArgs = new FailureEventArgs
            {
                ChunkIndex = 5,
                ChunkSize = 524288,
                File = failedFile,
                Operation = "upload",
                StatusText = "Network error",
                TotalChunk = 20
            };

            // Assert
            Assert.Equal(5, failureEventArgs.ChunkIndex);
            Assert.Equal(524288, failureEventArgs.ChunkSize);
            Assert.Equal(20, failureEventArgs.TotalChunk);
            Assert.Equal("Network error", failureEventArgs.StatusText);
        }

        [Fact(Timeout = 10000)]
        public void FailureEventArgs_RetryFiles()
        {
            // Arrange & Act
            var file1 = new FileInfo { Name = "Retry1.pdf" };
            var file2 = new FileInfo { Name = "Retry2.doc" };
            var retryFiles = new FileInfo[] { file1, file2 };

            var failureEventArgs = new FailureEventArgs
            {
                RetryFiles = retryFiles
            };

            // Assert
            Assert.NotNull(failureEventArgs.RetryFiles);
            Assert.Equal(2, failureEventArgs.RetryFiles.Length);
            Assert.Equal("Retry1.pdf", failureEventArgs.RetryFiles[0].Name);
            Assert.Equal("Retry2.doc", failureEventArgs.RetryFiles[1].Name);
        }

        [Fact(Timeout = 10000)]
        public void UploadChangeEventArgs_EmptyFiles()
        {
            // Arrange & Act
            var uploadChangeEventArgs = new UploadChangeEventArgs
            {
                Files = new List<UploadFiles>()
            };

            // Assert
            Assert.NotNull(uploadChangeEventArgs.Files);
            Assert.Empty(uploadChangeEventArgs.Files);
        }

        [Fact(Timeout = 10000)]
        public void ActionCompleteEventArgs_MultipleFiles()
        {
            // Arrange & Act
            var fileData = new List<FileInfo>
            {
                new FileInfo { Name = "Complete1.pdf", StatusCode = "2" },
                new FileInfo { Name = "Complete2.doc", StatusCode = "2" },
                new FileInfo { Name = "Complete3.jpg", StatusCode = "2" }
            };

            var actionCompleteEventArgs = new ActionCompleteEventArgs
            {
                FileData = fileData
            };

            // Assert
            Assert.NotNull(actionCompleteEventArgs.FileData);
            Assert.Equal(3, actionCompleteEventArgs.FileData.Count);
            Assert.All(actionCompleteEventArgs.FileData, file => Assert.Equal("2", file.StatusCode));
        }

        [Fact(Timeout = 10000)]
        public void ProgressEventArgs_StreamProperty()
        {
            // Arrange & Act
            var fileInfo = new FileInfo { Name = "Progress.mp4", Size = 10485760 };
            var progressEventArgs = new ProgressEventArgs
            {
                File = fileInfo,
                Total = 10485760,
                Loaded = 5242880,
                LengthComputable = true,
                Operation = "Uploading"
            };

            // Assert
            Assert.Equal(10485760, progressEventArgs.Total);
            Assert.Equal(5242880, progressEventArgs.Loaded);
            Assert.True(progressEventArgs.LengthComputable);
            Assert.Equal("Uploading", progressEventArgs.Operation);
        }

        [Fact(Timeout = 10000)]
        public void ProgressEventArgs_PercentageCalculation()
        {
            // Arrange & Act
            var progressEventArgs = new ProgressEventArgs
            {
                Total = 1000,
                Loaded = 500,
                LengthComputable = true
            };

            // Calculate percentage
            var percentage = (progressEventArgs.Loaded / progressEventArgs.Total) * 100;

            // Assert
            Assert.Equal(50, percentage);
        }

        #endregion

    }
}
