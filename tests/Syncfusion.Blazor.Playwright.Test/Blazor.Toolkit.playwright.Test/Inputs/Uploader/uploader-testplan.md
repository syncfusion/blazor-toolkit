# Syncfusion Blazor SfUploader Component - Comprehensive Test Plan

## Document Information
- **Component**: SfUploader (File Upload Component)
- **Version**: Current Release
- **Test Scope**: Complete feature coverage including UI, events, validation, and real-world scenarios
- **Test Framework**: Playwright
- **Created**: April 2, 2026

---

## Executive Summary

This test plan provides comprehensive coverage for the Syncfusion Blazor Uploader component. It includes tests for all major features, edge cases, error conditions, and real-world usage scenarios. The test plan is designed to validate functionality across different configurations and user interactions.

---

## 1. File Selection and Browse Functionality

### 1.1 Single File Selection via Browse Button
**Scenario**: User selects a single file through the browse button
- **Steps**:
  1. Load the uploader component with default settings (AllowMultiple=true)
  2. Click the "Browse" button
  3. Select a single file (e.g., "document.pdf" - 2 MB)
  4. Verify the file appears in the file list
- **Expected Outcome**: 
  - File name is displayed in the list
  - File size is displayed correctly (e.g., "2.0 MB")
  - File status shows "Ready to upload"
  - File is displayed in the correct format with name, size, and extension

### 1.2 Multiple File Selection via Browse Button
**Scenario**: User selects multiple files at once
- **Steps**:
  1. Load uploader with AllowMultiple=true
  2. Click "Browse" button
  3. Select 3-5 different files (various formats: .jpg, .pdf, .txt, .docx)
  4. Verify all files appear in the list
  5. Verify files are listed in the order selected
- **Expected Outcome**: 
  - All selected files are displayed in the file list
  - Each file shows correct name, size, and type
  - Files maintain selection order
  - File count is accurate

### 1.3 Single File Selection When AllowMultiple=False
**Scenario**: Only single file selection allowed
- **Steps**:
  1. Load uploader with AllowMultiple=false
  2. Click "Browse" button
  3. Attempt to select multiple files
  4. Verify only one file is accepted
- **Expected Outcome**: 
  - File picker allows multiple file selection
  - Only the last selected file is added to the uploader
  - Previous files in the list remain unchanged

### 1.4 Verify Browse Button Accessibility
**Scenario**: Browse button is accessible via keyboard
- **Steps**:
  1. Load uploader component
  2. Tab to the "Browse" button
  3. Press Enter key
  4. Verify file picker opens
  5. Check ARIA attributes on browse button
- **Expected Outcome**: 
  - Browse button is keyboard accessible
  - File picker opens when Enter is pressed
  - Button has proper aria-label attribute
  - Visual focus indicator is visible

---

## 2. Drag and Drop Functionality

### 2.1 Single File Drag and Drop
**Scenario**: User drags a single file to the drop area
- **Steps**:
  1. Load uploader with drag-drop enabled
  2. Prepare a file on the system (e.g., "image.jpg")
  3. Drag the file to the uploader's drop area (default container)
  4. Release the file
  5. Verify file appears in the list
- **Expected Outcome**: 
  - Drop area shows visual feedback during drag (cursor changes)
  - File is added to the upload list
  - File metadata is displayed correctly

### 2.2 Multiple Files Drag and Drop
**Scenario**: User drags multiple files simultaneously
- **Steps**:
  1. Load uploader with AllowMultiple=true
  2. Select 3-5 files in file explorer
  3. Drag all files to drop area
  4. Release files
  5. Verify all files are added
- **Expected Outcome**: 
  - All dragged files are added to the list
  - Files are listed correctly with names and sizes
  - Multiple file drag-drop works smoothly

### 2.3 Drag and Drop on Custom Drop Area
**Scenario**: Drop area is customized to a specific element
- **Steps**:
  1. Load uploader with custom DropArea selector (e.g., #customDropZone)
  2. Verify drop area is applied to the custom element
  3. Drag file to default drop area - should not accept
  4. Drag file to custom drop area - should accept
  5. Verify file is added to uploader
- **Expected Outcome**: 
  - Custom drop area is the only drop zone
  - Files can only be dropped on custom area
  - Default drop area does not accept drops

### 2.4 Drop Effect Visual Feedback
**Scenario**: Verify drop effect cursor changes
- **Steps**:
  1. Load uploader with DropEffect=Copy
  2. Drag file over drop area
  3. Verify cursor shows copy icon/indicator
  4. Check DropEffect=Move scenario
  5. Check DropEffect=Link scenario
  6. Check DropEffect=None scenario
- **Expected Outcome**: 
  - Cursor changes according to DropEffect setting
  - "Copy" shows copy cursor
  - "Move" shows move cursor
  - "Link" shows link cursor
  - "None" shows not-allowed cursor

---

## 3. File Validation

### 3.1 File Extension Validation - Allowed Extensions
**Scenario**: User tries to upload files with specific allowed extensions
- **Steps**:
  1. Load uploader with AllowedExtensions=".jpg, .png, .pdf"
  2. Select allowed files (.jpg, .png, .pdf)
  3. Verify files are accepted
  4. Select disallowed files (.exe, .docx, .txt)
  5. Verify files show validation error
- **Expected Outcome**: 
  - Allowed files show "Ready to upload" status
  - Disallowed files show error message: "Invalid file type"
  - Error message persists until file is removed
  - File count only includes valid files for upload

### 3.2 File Size Validation - Maximum Size
**Scenario**: Files exceeding MaxFileSize are rejected
- **Steps**:
  1. Load uploader with MaxFileSize=5242880 (5 MB)
  2. Select file below limit (3 MB) - should be accepted
  3. Select file above limit (10 MB) - should be rejected
  4. Verify error message is displayed
  5. Check upload is prevented for oversized files
- **Expected Outcome**: 
  - Files under limit show "Ready to upload"
  - Files over limit show error: "File size exceeds maximum limit"
  - Oversized files cannot be uploaded even if Upload button is clicked
  - File size validation happens immediately after selection

### 3.3 File Size Validation - Minimum Size
**Scenario**: Files below MinFileSize are rejected
- **Steps**:
  1. Load uploader with MinFileSize=1024 (1 KB)
  2. Create empty file (0 bytes) - should be rejected
  3. Create small file (512 bytes) - should be rejected
  4. Create valid file (5 KB) - should be accepted
  5. Verify error messages
- **Expected Outcome**: 
  - Empty files show error: "File size is less than minimum size"
  - Small files show the same error
  - Valid files show "Ready to upload"
  - All validation errors are displayed immediately

### 3.4 File Name Sanitization
**Scenario**: HTML/XSS code in file names is sanitized
- **Precondition**: EnableHtmlSanitizer=true (default)
- **Steps**:
  1. Prepare file with name: "<script>alert('xss')</script>.pdf"
  2. Upload the file (if possible) or add to list
  3. Verify script tags are removed from display
  4. Check file name is sanitized but file is accepted if extension valid
  5. Repeat with EnableHtmlSanitizer=false
- **Expected Outcome**: 
  - With sanitizer enabled: dangerous content removed, extension checked
  - With sanitizer disabled: raw file name shown (control/testing only)
  - File validation still works based on extension

### 3.5 Multiple Validation Errors
**Scenario**: File fails multiple validation criteria
- **Steps**:
  1. Load uploader with MaxFileSize=1024, AllowedExtensions=".jpg, .pdf"
  2. Select 10 MB Excel file (.xlsx)
  3. Verify both size and type validation errors
  4. Select 0 byte file (.txt)
  5. Verify size and extension validation errors
- **Expected Outcome**: 
  - File shows all applicable validation errors
  - User understands why file was rejected
  - File is not added to uploadable files list

---

## 4. File List Display

### 4.1 Default File List Rendering
**Scenario**: Files are displayed in default format
- **Steps**:
  1. Load uploader with ShowFileList=true
  2. Select 3 files with different sizes
  3. Verify file list displays all files
  4. Check each item shows: name, extension, size, status
  5. Verify remove icons are present for each file
- **Expected Outcome**: 
  - File list is visible below upload area
  - Each file item contains: name, extension, size
  - Remove icon (trash bin) is clickable
  - Status indicator shows current state
  - Files are displayed in correct order

### 4.2 File List Hidden
**Scenario**: File list is hidden when ShowFileList=false
- **Steps**:
  1. Load uploader with ShowFileList=false
  2. Select multiple files
  3. Verify file list is not displayed
  4. Check files are still in component state (accessible via API)
  5. Verify custom template still works if provided
- **Expected Outcome**: 
  - File list is not rendered
  - No file items are visible
  - Custom template displays if provided
  - Files can still be uploaded/managed via API

### 4.3 File List with Custom Template
**Scenario**: Custom template is used for file items
- **Steps**:
  1. Load uploader with custom template showing: [File Name] - [Size] - [Status]
  2. Select files
  3. Verify template is applied to each file item
  4. Check custom HTML structure is rendered
  5. Verify remove button still works with template
- **Expected Outcome**: 
  - Custom template is applied to all file items
  - Template displays all provided variables correctly
  - Remove functionality still works with template
  - File operations respect template structure

### 4.4 File List with RTL (Right-to-Left)
**Scenario**: File list displays correctly in RTL mode
- **Steps**:
  1. Load uploader with EnableRtl=true
  2. Select files
  3. Verify file list is right-aligned
  4. Check file name and extension positions are reversed
  5. Verify remove button is on the left side
- **Expected Outcome**: 
  - File list layout is mirrored for RTL
  - Text direction is right-to-left
  - Remove button is positioned on left
  - All elements are properly positioned for RTL

### 4.5 File Item Status Display
**Scenario**: File status updates correctly based on state
- **Steps**:
  1. Select a file - verify status "Ready to upload"
  2. Start upload - verify status "Uploading"
  3. Complete upload - verify status "File uploaded successfully"
  4. Try to upload invalid file - verify status shows error
  5. Click remove during upload - verify "File removed successfully"
- **Expected Outcome**: 
  - Status updates appropriately for each state
  - Status text matches component state
  - Status is visible and clear to user
  - Status colors/indicators help identify state

---

## 5. Auto Upload Feature

### 5.1 Auto Upload Enabled (Default)
**Scenario**: Files are automatically uploaded on selection
- **Precondition**: AutoUpload=true, SaveUrl configured
- **Steps**:
  1. Load uploader with auto-upload enabled
  2. Select a file
  3. Verify upload starts immediately without clicking Upload button
  4. Monitor upload progress
  5. Verify file completes upload
  6. Check file status changes to "File uploaded successfully"
- **Expected Outcome**: 
  - Upload starts immediately after file selection
  - No manual upload action needed
  - File progresses from "Uploading" to "File uploaded successfully"
  - Upload button is not visible or disabled

### 5.2 Auto Upload Disabled - Manual Upload
**Scenario**: Files must be manually uploaded
- **Precondition**: AutoUpload=false
- **Steps**:
  1. Load uploader with auto-upload disabled
  2. Select file
  3. Verify file shows in list but does not upload
  4. Verify "Upload" button is visible and enabled
  5. Verify "Clear" button is visible
  6. Click "Upload" button
  7. Verify upload starts and completes
- **Expected Outcome**: 
  - File is not uploaded automatically
  - Upload and Clear buttons are visible
  - Clicking Upload button starts the upload
  - File uploads after button click
  - Buttons remain visible until all files are uploaded

### 5.3 Sequential Upload
**Scenario**: Multiple files are uploaded one after another
- **Precondition**: AutoUpload=false, SequentialUpload=true, SaveUrl configured
- **Steps**:
  1. Load uploader with sequential upload enabled
  2. Select 3 files
  3. Click Upload button
  4. Verify first file uploads completely before second starts
  5. Verify each file shows upload progress individually
  6. Monitor all files complete sequentially
- **Expected Outcome**: 
  - Files upload in sequence, not in parallel
  - Only one file's progress bar shows at a time
  - Each file completes before next starts
  - All files complete upload in order

### 5.4 Parallel Upload (Sequential = False)
**Scenario**: Multiple files are uploaded simultaneously
- **Precondition**: AutoUpload=false, SequentialUpload=false, SaveUrl configured
- **Steps**:
  1. Load uploader with parallel upload
  2. Select 3-5 files
  3. Click Upload button
  4. Verify multiple files show progress bars
  5. Monitor all files progress simultaneously
  6. Verify all files complete
- **Expected Outcome**: 
  - Multiple files upload concurrently
  - Multiple progress bars visible
  - Files may complete in different order
  - Upload time is faster than sequential

---

## 6. Progress Tracking

### 6.1 Progress Bar Display
**Scenario**: Progress bar shows upload progress
- **Precondition**: ShowProgressBar=true, file size > 1 MB recommended
- **Steps**:
  1. Load uploader with progress bar enabled
  2. Select and upload a file (or start auto-upload)
  3. Verify progress bar appears during upload
  4. Observe progress bar fills from 0-100%
  5. Verify percentage text updates (0%, 25%, 50%, 75%, 100%)
  6. Verify progress bar disappears when upload completes
- **Expected Outcome**: 
  - Progress bar is visible during upload
  - Bar width increases smoothly
  - Percentage is displayed and updates correctly
  - Bar disappears on completion

### 6.2 Progress Bar Hidden
**Scenario**: Progress bar is hidden
- **Precondition**: ShowProgressBar=false
- **Steps**:
  1. Load uploader with progress bar disabled
  2. Select and upload file
  3. Verify no progress bar is displayed
  4. Verify file still uploads normally
  5. Verify status text still updates
- **Expected Outcome**: 
  - No progress bar is shown
  - File uploads normally without visual progress indicator
  - File status updates appropriately
  - Upload completes successfully

### 6.3 Progressing Event Fired
**Scenario**: Progressing event is triggered during upload
- **Steps**:
  1. Load uploader with Progressing event handler
  2. Select and upload file
  3. Monitor Progressing event fires multiple times
  4. Verify event provides: loaded, total, file info
  5. Verify percentage can be calculated: (loaded/total)*100
- **Expected Outcome**: 
  - Progressing event fires multiple times during upload
  - Event arguments contain correct loaded/total values
  - Percentage calculations are accurate
  - Event fires with appropriate frequency

---

## 7. Upload Operations

### 7.1 Upload Method - Upload All Files
**Scenario**: Upload all pending files via API call
- **Steps**:
  1. Load uploader with AutoUpload=false
  2. Select 3 files
  3. Call UploadAsync() method without parameters
  4. Verify all 3 files start uploading
  5. Monitor all files upload to completion
- **Expected Outcome**: 
  - All pending files are uploaded
  - Each file shows progress and completes
  - All files show success status
  - Method works as expected

### 7.2 Upload Method - Upload Specific Files
**Scenario**: Upload only selected files via API
- **Steps**:
  1. Load uploader with AutoUpload=false
  2. Select 5 files
  3. Call UploadAsync(fileArray) with 2 specific files
  4. Verify only 2 specified files upload
  5. Verify other 3 files remain in "Ready to upload" state
- **Expected Outcome**: 
  - Only specified files are uploaded
  - Other files are not affected
  - Specified files complete upload
  - Method allows selective upload

### 7.3 Upload With Custom Parameters (BeforeUpload)
**Scenario**: Additional parameters added to upload request
- **Steps**:
  1. Load uploader with BeforeUpload event handler
  2. In handler, add custom parameters (userId, category, etc.)
  3. Select and upload file
  4. Monitor network request (using browser dev tools)
  5. Verify custom parameters are in the request
- **Expected Outcome**: 
  - Custom parameters are added to upload request
  - Parameters are sent to server
  - Server receives custom data successfully
  - File uploads with custom parameters

### 7.4 BeforeUpload Event Cancellation
**Scenario**: Upload can be canceled in BeforeUpload event
- **Steps**:
  1. Load uploader with BeforeUpload event handler
  2. In handler, set Cancel=true
  3. Select and attempt to upload file
  4. Verify upload does not start
  5. Verify file remains in "Ready to upload" state
- **Expected Outcome**: 
  - Upload is canceled when Cancel flag is set
  - File does not upload
  - File remains available for later upload
  - Status is not changed

---

## 8. File Removal

### 8.1 Remove Single File Before Upload
**Scenario**: User removes a file before upload
- **Steps**:
  1. Select file(s) - file is in "Ready to upload" state
  2. Click remove icon (trash icon) for one file
  3. Verify file is removed from list
  4. Verify file count decreases
  5. Verify remaining files are still in list
- **Expected Outcome**: 
  - File is immediately removed from list
  - File count is updated
  - Remaining files are unaffected
  - Remove action is immediate and visual

### 8.2 Remove File During Upload
**Scenario**: User removes file while it's uploading
- **Steps**:
  1. Start uploading a large file
  2. While file is "Uploading", click remove icon
  3. Verify file is removed from list
  4. Verify upload is stopped
  5. Verify file is not completed on server
- **Expected Outcome**: 
  - File is removed from list
  - Upload is canceled/stopped
  - Partial file data should be cleaned on server
  - Other uploads continue normally

### 8.3 Remove Uploaded File
**Scenario**: Remove a successfully uploaded file
- **Precondition**: RemoveUrl is configured on server
- **Steps**:
  1. Upload file successfully
  2. File shows "File uploaded successfully" status
  3. Click remove icon for uploaded file
  4. Verify file is removed from list
  5. Verify server-side file is deleted (if applicable)
  6. Verify event is triggered (OnRemove or Success with operation="upload")
- **Expected Outcome**: 
  - File is removed from client list
  - Server receives removal request
  - File is deleted from server storage
  - Removal is confirmed to user

### 8.4 Remove Via API (RemoveAsync)
**Scenario**: Remove files via RemoveAsync method
- **Steps**:
  1. Upload some files
  2. Call RemoveAsync(fileArray) with specific files
  3. Verify specified files are removed
  4. Verify other files remain in list
  5. Verify server-side removal is called
- **Expected Outcome**: 
  - Specified files are removed
  - Server removal is processed
  - Other files are unaffected
  - Method works as expected

### 8.5 BeforeRemove Event Cancellation
**Scenario**: Removal can be canceled in BeforeRemove event
- **Steps**:
  1. Load uploader with BeforeRemove event handler
  2. In handler, set Cancel=true
  3. Click remove icon on a file
  4. Verify file is NOT removed
  5. Verify file remains in list with same status
- **Expected Outcome**: 
  - Remove operation is canceled
  - File remains in list
  - File status is unchanged
  - User can retry removal

---

## 9. Clear All Functionality

### 9.1 Clear All Files
**Scenario**: User clears all files at once
- **Precondition**: AutoUpload=false (so Clear button is visible)
- **Steps**:
  1. Select multiple files
  2. Verify "Clear" button is visible
  3. Click "Clear" button
  4. Verify all files are removed from list
  5. Verify file list becomes empty
- **Expected Outcome**: 
  - All files are cleared from list
  - File list is now empty
  - Clear button remains visible for future selections
  - No files remain in upload queue

### 9.2 Clear Files Via API (ClearAllAsync)
**Scenario**: Clear files using ClearAllAsync method
- **Steps**:
  1. Select files
  2. Call ClearAllAsync() method
  3. Verify all files are cleared
  4. Verify file list is empty
  5. Verify no files are uploaded
- **Expected Outcome**: 
  - All files are cleared immediately
  - File list is empty
  - No pending uploads
  - Method works as expected

### 9.3 OnClear Event With Cancellation
**Scenario**: Clear operation can be canceled in event
- **Steps**:
  1. Load uploader with OnClear event handler
  2. In handler, set Cancel=true
  3. Click Clear button
  4. Verify files are NOT cleared
  5. Verify all files remain in list
- **Expected Outcome**: 
  - Clear operation is canceled
  - Files remain in list
  - User can retry clear or cancel

---

## 10. Success and Failure Scenarios

### 10.1 File Upload Success
**Scenario**: File uploads successfully to server
- **Precondition**: Valid SaveUrl endpoint, file passes validation
- **Steps**:
  1. Select valid file
  2. Upload file
  3. Monitor upload completion
  4. Verify file status changes to "File uploaded successfully"
  5. Verify Success event is triggered
  6. Check file appears in uploaded files list
- **Expected Outcome**: 
  - File uploads completely
  - Status updates to success message
  - Success event fires with file info
  - File is marked as uploaded

### 10.2 File Upload Failure - Server Error
**Scenario**: Server returns error (500, 404, etc.)
- **Steps**:
  1. Configure SaveUrl to invalid endpoint
  2. Select valid file
  3. Attempt upload
  4. Verify upload fails
  5. Verify error status is displayed
  6. Verify OnFailure event fires
  7. Verify error message is shown
- **Expected Outcome**: 
  - Upload fails with appropriate error
  - Status shows error message
  - OnFailure event is triggered
  - Error details are provided
  - File can be retried

### 10.3 File Upload Failure - Network Error
**Scenario**: Network connection fails during upload
- **Steps**:
  1. Select file for upload
  2. Simulate network failure (offline mode)
  3. Verify upload fails
  4. Verify error is displayed
  5. Verify file can be retried
  6. Restore network and retry
- **Expected Outcome**: 
  - Upload fails on network error
  - Error is clearly communicated
  - File status shows failure
  - Retry option is available

### 10.4 Action Complete Event
**Scenario**: OnActionComplete fires after all files processed
- **Steps**:
  1. Upload multiple files (mix of success/failure)
  2. Monitor all files upload
  3. Verify OnActionComplete event fires
  4. Verify event includes all file data
  5. Check FileData array contains all uploaded files
- **Expected Outcome**: 
  - OnActionComplete event fires after all uploads
  - Event contains all file information
  - Event provides summary of all operations
  - Event allows post-upload processing

---

## 11. Chunk Upload Feature

### 11.1 Chunk Upload Configuration
**Scenario**: Files are uploaded in chunks
- **Precondition**: AsyncSettings.ChunkSize set (e.g., 1048576 bytes = 1MB)
- **Steps**:
  1. Load uploader with chunk size = 1MB
  2. Select file larger than chunk size (e.g., 5MB)
  3. Start upload
  4. Monitor multiple OnChunkUploadStart events
  5. Verify OnChunkSuccess fires for each chunk
  6. Verify final chunk completes upload
- **Expected Outcome**: 
  - File is split into multiple chunks
  - Each chunk is uploaded separately
  - OnChunkUploadStart fires for each chunk
  - OnChunkSuccess fires for successful chunks
  - File completes when all chunks uploaded

### 11.2 Chunk Upload Progress
**Scenario**: Progress tracking works with chunks
- **Steps**:
  1. Configure chunk upload
  2. Upload large file in chunks
  3. Monitor Progressing event fires frequently
  4. Verify loaded/total values increase correctly
  5. Verify progress percentage is accurate across chunks
- **Expected Outcome**: 
  - Progress events fire for chunk progress
  - Progress values are accurate
  - Overall progress reflects all chunks
  - Progress bar updates smoothly

### 11.3 Chunk Upload Pause
**Scenario**: User pauses chunk upload
- **Steps**:
  1. Start uploading file in chunks
  2. During upload, call PauseAsync()
  3. Verify upload stops
  4. Verify file state shows "paused"
  5. Verify already-uploaded chunks are preserved
  6. Call ResumeAsync() to continue
  7. Verify upload resumes from pause point
- **Expected Outcome**: 
  - Upload pauses immediately
  - State is preserved
  - Already-uploaded chunks are not re-sent
  - Resume continues upload from pause point

### 11.4 Chunk Upload Cancel
**Scenario**: User cancels chunk upload
- **Steps**:
  1. Start uploading large file
  2. During upload, call CancelAsync()
  3. Verify upload stops
  4. Verify file is removed from list
  5. Verify server cleans up partial upload
  6. Verify file cannot be resumed
- **Expected Outcome**: 
  - Upload is canceled
  - File is removed
  - Partial chunks are cleaned on server
  - Cancel is permanent (no resume option)

### 11.5 Chunk Upload Failure and Retry
**Scenario**: Chunk upload fails and is retried
- **Precondition**: RetryCount and RetryAfterDelay configured
- **Steps**:
  1. Configure chunk upload with retry settings
  2. Simulate failure on a chunk (network/server issue)
  3. Verify chunk is retried after delay
  4. Verify retry count is respected
  5. Verify max retries exceeded shows error
  6. Verify user can manually retry
- **Expected Outcome**: 
  - Failed chunk is automatically retried
  - Retry delay is honored
  - Max retries are respected
  - Manual retry option available
  - Error shown if all retries fail

---

## 12. Disabled State

### 12.1 Disabled Uploader
**Scenario**: Uploader component is disabled
- **Steps**:
  1. Load uploader with Enabled=false
  2. Verify Browse button is disabled
  3. Verify drag-drop does not work
  4. Verify "disabled" class is applied
  5. Verify disabled CSS styling is applied
  6. Check aria-disabled="true" attribute
- **Expected Outcome**: 
  - Browse button is visually disabled
  - Drag-drop is not functional
  - Disabled styling is applied
  - Accessibility attributes are set
  - User cannot interact with uploader

### 12.2 Enable/Disable Toggle
**Scenario**: Uploader is toggled between enabled/disabled
- **Steps**:
  1. Load enabled uploader
  2. Toggle Enabled property to false
  3. Verify uploader is disabled
  4. Toggle Enabled property to true
  5. Verify uploader is enabled again
  6. Verify functionality is restored
- **Expected Outcome**: 
  - Uploader state changes immediately
  - Disabled state is properly applied/removed
  - Functionality is restored when re-enabled
  - Property changes are respected

---

## 13. Directory Upload Feature

### 13.1 Directory Upload Enabled
**Scenario**: User can upload entire directories
- **Precondition**: DirectoryUpload=true
- **Steps**:
  1. Load uploader with directory upload enabled
  2. Click Browse button
  3. Verify browse dialog allows selecting folders
  4. Select a folder with multiple files
  5. Verify all files in folder are added to list
  6. Verify folder structure is reflected if applicable
- **Expected Outcome**: 
  - Browse dialog allows folder selection
  - All files in folder are added
  - File list shows all uploaded files
  - Folder structure is preserved if applicable

### 13.2 Directory Upload Disabled
**Scenario**: User can only select files, not directories
- **Precondition**: DirectoryUpload=false (default)
- **Steps**:
  1. Load uploader with directory upload disabled
  2. Click Browse button
  3. Verify browse dialog shows "Files only" mode
  4. Attempt to select folder
  5. Verify folder cannot be selected
  6. Select individual files instead
- **Expected Outcome**: 
  - Folder selection is not possible
  - Only individual files can be selected
  - Browse dialog is in file-only mode

---

## 14. File Selection Event

### 14.1 FileSelected Event Handler
**Scenario**: FileSelected event is triggered on file selection
- **Steps**:
  1. Load uploader with FileSelected event handler
  2. Select files
  3. Verify FileSelected event fires
  4. Verify event provides file information
  5. Verify SelectedEventArgs includes FilesData array
  6. Verify Cancel property can be used to reject files
- **Expected Outcome**: 
  - FileSelected event fires on file selection
  - Event includes all selected file info
  - Files can be programmatically rejected via Cancel
  - Event fires before files are added to list

### 14.2 Cancel Files in FileSelected Event
**Scenario**: Files are rejected by canceling in event
- **Steps**:
  1. Load uploader with FileSelected handler that cancels
  2. Select files
  3. In handler, set Cancel=true
  4. Verify files are NOT added to list
  5. Verify file list remains empty
  6. Verify no upload occurs
- **Expected Outcome**: 
  - Files are rejected on Cancel
  - Files do not appear in list
  - No files are queued for upload
  - User can select different files

---

## 15. Keyboard Navigation

### 15.1 Tab Navigation to Buttons
**Scenario**: Buttons are accessible via Tab key
- **Steps**:
  1. Load uploader component
  2. Press Tab key to navigate to Browse button
  3. Verify Browse button receives focus
  4. Verify visual focus indicator is shown
  5. If AutoUpload=false, press Tab again
  6. Verify Upload button receives focus
  7. Press Tab again to verify Clear button
- **Expected Outcome**: 
  - All buttons are reachable via Tab
  - Focus order is logical
  - Focus indicator is visible
  - Buttons are properly sequenced

### 15.2 Enter Key on Browse Button
**Scenario**: Pressing Enter on Browse button opens file picker
- **Steps**:
  1. Focus Browse button
  2. Press Enter key
  3. Verify file picker dialog opens
  4. Select file and close picker
  5. Verify file is added to uploader
- **Expected Outcome**: 
  - Enter key activates Browse button
  - File picker opens
  - Selected files are added
  - Keyboard accessibility works

### 15.3 Enter Key on Upload Button
**Scenario**: Pressing Enter on Upload button starts upload
- **Precondition**: AutoUpload=false
- **Steps**:
  1. Select files
  2. Focus Upload button
  3. Press Enter key
  4. Verify upload starts
  5. Monitor upload completion
- **Expected Outcome**: 
  - Enter key activates Upload button
  - Upload process starts
  - Upload completes normally
  - Keyboard control works

### 15.4 Enter Key on Remove Icon
**Scenario**: Pressing Enter on remove icon removes file
- **Steps**:
  1. Select file
  2. Focus remove icon (using Tab if needed)
  3. Press Enter key
  4. Verify file is removed
  5. Verify file list is updated
- **Expected Outcome**: 
  - Enter key activates remove icon
  - File is removed immediately
  - File list updates
  - Keyboard navigation works

---

## 16. CSS Class and Styling

### 16.1 Custom CSS Class Application
**Scenario**: Custom CSS class is applied to uploader
- **Steps**:
  1. Load uploader with CssClass="custom-uploader large"
  2. Inspect component's root element
  3. Verify "custom-uploader" class is present
  4. Verify "large" class is present
  5. Verify custom CSS is applied
- **Expected Outcome**: 
  - Custom CSS classes are applied to root element
  - Multiple classes can be applied
  - Custom styling is visible
  - Classes are correctly assigned

### 16.2 HTML Attributes
**Scenario**: Custom HTML attributes are applied
- **Steps**:
  1. Load uploader with HtmlAttributes="title='My Uploader', data-testid='file-upload'"
  2. Inspect component element
  3. Verify title attribute is present
  4. Verify data-testid attribute is present
  5. Verify values are correct
- **Expected Outcome**: 
  - Custom HTML attributes are applied
  - Multiple attributes can be set
  - Attributes are correctly assigned
  - Values are preserved

---

## 17. RTL (Right-to-Left) Support

### 17.1 RTL Layout
**Scenario**: Component layout is mirrored for RTL languages
- **Steps**:
  1. Load uploader with EnableRtl=true
  2. Inspect component for RTL class
  3. Verify Browse button is positioned appropriately
  4. Verify file list items are right-aligned
  5. Verify remove icons are on the left
  6. Verify all text flows right-to-left
- **Expected Outcome**: 
  - RTL class is applied
  - Layout is mirrored
  - Text flows right-to-left
  - All elements are repositioned

### 17.2 RTL Functionality
**Scenario**: All functionality works in RTL mode
- **Steps**:
  1. Load RTL uploader
  2. Browse and select files - should work
  3. Remove files - should work
  4. Upload files - should work
  5. Verify all events fire normally
- **Expected Outcome**: 
  - All functionality works in RTL
  - Events fire correctly
  - File operations work normally
  - No functional differences

---

## 18. Preloaded Files (UploadedFiles Property)

### 18.1 Display Preloaded Files
**Scenario**: Previously uploaded files are displayed
- **Steps**:
  1. Load uploader with UploadedFiles configured:
     - File1.pdf (2 MB)
     - Document.docx (1.5 MB)
  2. Verify preloaded files appear in list
  3. Verify files show status "File uploaded successfully"
  4. Verify preloaded files cannot be re-uploaded
- **Expected Outcome**: 
  - Preloaded files are displayed in list
  - Status shows success
  - Files are marked as already uploaded
  - No duplicate uploads possible

### 18.2 Remove Preloaded Files
**Scenario**: Users can remove preloaded files
- **Steps**:
  1. Load with preloaded files
  2. Click remove icon on preloaded file
  3. Verify file is removed from list
  4. Verify server is notified of removal
  5. Verify removed file does not reappear
- **Expected Outcome**: 
  - Preloaded files can be removed
  - Remove request sent to server
  - File is deleted from server
  - File is removed from display

### 18.3 Mix Preloaded and New Files
**Scenario**: Display mix of preloaded and newly selected files
- **Steps**:
  1. Load with 2 preloaded files
  2. Select 3 new files
  3. Verify all 5 files are displayed
  4. Verify preloaded files show success status
  5. Verify new files show "Ready to upload" status
  6. Upload new files
  7. Verify all uploads complete
- **Expected Outcome**: 
  - Preloaded and new files display together
  - Each file shows appropriate status
  - New files upload while preloaded remain
  - All operations work correctly

---

## 19. Form Integration

### 19.1 Uploader in Form Submission
**Scenario**: Uploader works correctly inside HTML form
- **Steps**:
  1. Place uploader inside HTML form
  2. Set AutoUpload=false
  3. Select and upload files
  4. Submit form containing uploader
  5. Verify form submission works
  6. Verify uploaded files are not lost
- **Expected Outcome**: 
  - Uploader works inside form
  - Form can be submitted
  - File data is preserved
  - No conflicts with form submission

### 19.2 Form Disabled State
**Scenario**: Uploader inherits disabled state from form
- **Steps**:
  1. Place uploader in form
  2. Disable form
  3. Verify uploader is also disabled
  4. Verify Browse and Upload buttons are disabled
  5. Enable form
  6. Verify uploader is enabled
- **Expected Outcome**: 
  - Uploader respects form disabled state
  - Uploader is disabled when form is disabled
  - Uploader is enabled when form is enabled

---

## 20. State Persistence

### 20.1 Persistence Enabled
**Scenario**: Uploader state persists across page reloads
- **Precondition**: EnablePersistence=true
- **Steps**:
  1. Load uploader with persistence enabled
  2. Select files
  3. Refresh page (F5 or hard refresh)
  4. Verify selected files are still present
  5. Verify file list is restored
  6. Verify upload can continue
- **Expected Outcome**: 
  - File list persists across reloads
  - File data is restored
  - Upload can continue after reload
  - State is maintained

### 20.2 Persistence Disabled
**Scenario**: Uploader state is not persisted
- **Precondition**: EnablePersistence=false (default)
- **Steps**:
  1. Load uploader with persistence disabled
  2. Select files
  3. Refresh page
  4. Verify file list is cleared
  5. Verify no files are restored
- **Expected Outcome**: 
  - File list is cleared on refresh
  - No state is persisted
  - Fresh start after reload
  - Default behavior

---

## 21. Multiple Uploader Instances

### 21.1 Multiple Uploaders on Same Page
**Scenario**: Multiple uploader components work independently
- **Steps**:
  1. Add 2-3 uploader components to page
  2. Select files in first uploader
  3. Verify files appear only in first uploader
  4. Select files in second uploader
  5. Verify files appear only in second uploader
  6. Upload from both uploaders simultaneously
  7. Verify uploads happen independently
- **Expected Outcome**: 
  - Each uploader maintains separate file list
  - Files do not mix between uploaders
  - Uploads are independent
  - Multiple operations can occur simultaneously

### 21.2 Multiple Uploaders Different Configurations
**Scenario**: Multiple uploaders with different settings
- **Steps**:
  1. Create Uploader A: AutoUpload=true, AllowedExtensions=".jpg, .png"
  2. Create Uploader B: AutoUpload=false, AllowedExtensions=".pdf, .docx"
  3. Select appropriate files for each
  4. Verify A auto-uploads images
  5. Verify B requires manual upload of documents
  6. Verify each respects its own settings
- **Expected Outcome**: 
  - Each uploader has independent configuration
  - Each uploader operates according to its settings
  - No interference between uploaders
  - Multiple configurations work simultaneously

---

## 22. API Methods - GetFilesData

### 22.1 GetFilesData - All Files
**Scenario**: Retrieve all files data via API
- **Steps**:
  1. Select multiple files
  2. Call GetFilesDataAsync() without parameters
  3. Verify returns all files
  4. Verify returned data includes name, size, status
  5. Verify data structure matches FileInfo
- **Expected Outcome**: 
  - Method returns all files
  - Data includes complete file information
  - Data structure is correct
  - Can be used for validation/processing

### 22.2 GetFilesData - Specific File by Index
**Scenario**: Retrieve specific file by index
- **Steps**:
  1. Select 5 files
  2. Call GetFilesDataAsync(2) to get file at index 2
  3. Verify only file at index 2 is returned
  4. Verify returned data is correct for that file
  5. Try invalid index - verify behavior
- **Expected Outcome**: 
  - Returns file at specified index
  - Only one file returned
  - Data is accurate
  - Invalid index handled gracefully

---

## 23. API Methods - Sorting

### 23.1 Sort File List
**Scenario**: Sort files alphabetically by name
- **Steps**:
  1. Select files: "Zebra.pdf", "Apple.pdf", "Monkey.pdf"
  2. Call SortFileListAsync()
  3. Verify files are sorted alphabetically
  4. Verify order is: "Apple.pdf", "Monkey.pdf", "Zebra.pdf"
  5. Verify sort is case-insensitive
- **Expected Outcome**: 
  - Files are sorted alphabetically
  - Sort order is correct
  - Case-insensitive sort
  - File list updates with sorted order

---

## 24. Localization

### 24.1 Default English Localization
**Scenario**: Component displays in English by default
- **Steps**:
  1. Load uploader with default locale
  2. Verify button text: "Browse", "Upload", "Clear"
  3. Verify status messages in English
  4. Verify error messages in English
  5. Verify drop area text in English
- **Expected Outcome**: 
  - All text is in English
  - Messages are localized correctly
  - Button labels are standard
  - Error messages are clear

### 24.2 Alternative Language Localization
**Scenario**: Component displays in different language
- **Steps**:
  1. Configure component for Spanish locale
  2. Verify button text is in Spanish
  3. Verify status messages are in Spanish
  4. Verify error messages are in Spanish
  5. Verify localization strings are correct
- **Expected Outcome**: 
  - All text is in target language
  - Localization is correct
  - Messages make sense in target language
  - All components are localized

---

## 25. Error Scenarios

### 25.1 No SaveUrl Configured
**Scenario**: Upload attempted without SaveUrl
- **Steps**:
  1. Load uploader without SaveUrl in AsyncSettings
  2. Select and attempt to upload file
  3. Verify client-side upload handler is used
  4. Verify upload processes locally
  5. Verify OnValueChange event fires
- **Expected Outcome**: 
  - Client-side upload handler is triggered
  - No server request is made
  - OnValueChange event fires with file data
  - Local upload processing works

### 25.2 Invalid SaveUrl
**Scenario**: SaveUrl points to non-existent endpoint
- **Steps**:
  1. Configure SaveUrl to invalid URL
  2. Select file
  3. Attempt upload
  4. Verify upload fails with appropriate error
  5. Verify OnFailure event fires
  6. Verify error message is displayed
- **Expected Outcome**: 
  - Upload fails
  - Error event is triggered
  - Error message is shown to user
  - User can attempt again or remove file

### 25.3 Insufficient Permissions
**Scenario**: Server returns 403 Forbidden
- **Steps**:
  1. Configure upload to endpoint that requires auth
  2. Do not provide authentication
  3. Select and upload file
  4. Verify upload fails with 403 error
  5. Verify appropriate error message shown
- **Expected Outcome**: 
  - Upload fails with permission error
  - Error is clearly communicated
  - User understands they lack permission
  - File can be retried after providing auth

---

## 26. Accessibility (WCAG 2.1 AA)

### 26.1 Screen Reader Announcements
**Scenario**: Component announces state changes to screen readers
- **Steps**:
  1. Use screen reader to interact with uploader
  2. Navigate to Browse button
  3. Verify button purpose is announced
  4. Select file
  5. Verify file list is announced
  6. Verify file names are announced
  7. Check status changes are announced
- **Expected Outcome**: 
  - Browse button purpose is clear
  - File list is announced
  - File details are announced
  - Status changes are announced
  - Component is screen reader accessible

### 26.2 Focus Management
**Scenario**: Focus is managed correctly
- **Steps**:
  1. Tab through uploader
  2. Verify logical tab order
  3. Verify focus is always visible
  4. Check no focus traps
  5. Verify focus returns correctly after operations
- **Expected Outcome**: 
  - Tab order is logical
  - Focus is always visible
  - No focus traps exist
  - Focus management is correct

### 26.3 Color Contrast
**Scenario**: Text and elements have sufficient contrast
- **Steps**:
  1. Use contrast checker tool
  2. Check button text vs background
  3. Check status messages color
  4. Check error messages color
  5. Verify all contrast ratios >= 4.5:1 for text
- **Expected Outcome**: 
  - All text meets WCAG AA contrast ratio
  - Error messages are distinguishable
  - Buttons are clearly visible
  - Component is readable for all users

### 26.4 ARIA Attributes
**Scenario**: Proper ARIA attributes are used
- **Steps**:
  1. Inspect component for ARIA attributes
  2. Verify aria-label on buttons
  3. Verify aria-disabled="true" when disabled
  4. Verify aria-live regions for status updates
  5. Verify role attributes where appropriate
- **Expected Outcome**: 
  - ARIA labels are present and correct
  - Disabled state is announced
  - Status updates use live regions
  - Semantic roles are used correctly

---

## 27. Browser Compatibility

### 27.1 Chrome/Chromium
**Scenario**: Component works in Chrome browser
- **Steps**:
  1. Open component in latest Chrome
  2. Test file selection
  3. Test drag-drop
  4. Test upload
  5. Test all features
- **Expected Outcome**: 
  - All features work in Chrome
  - No JavaScript errors
  - UI renders correctly
  - All interactions work smoothly

### 27.2 Firefox
**Scenario**: Component works in Firefox browser
- **Steps**:
  1. Open component in latest Firefox
  2. Test file selection
  3. Test drag-drop
  4. Test upload
  5. Test all features
- **Expected Outcome**: 
  - All features work in Firefox
  - No JavaScript errors
  - UI renders correctly
  - All interactions work smoothly

### 27.3 Safari
**Scenario**: Component works in Safari browser
- **Steps**:
  1. Open component in latest Safari
  2. Test file selection
  3. Test drag-drop
  4. Test upload
  5. Test all features
- **Expected Outcome**: 
  - All features work in Safari
  - No JavaScript errors
  - UI renders correctly
  - All interactions work smoothly

### 27.4 Edge
**Scenario**: Component works in Edge browser
- **Steps**:
  1. Open component in latest Edge
  2. Test file selection
  3. Test drag-drop
  4. Test upload
  5. Test all features
- **Expected Outcome**: 
  - All features work in Edge
  - No JavaScript errors
  - UI renders correctly
  - All interactions work smoothly

---

## 28. Performance Testing

### 28.1 Large File Upload
**Scenario**: Component handles large file upload (100+ MB)
- **Steps**:
  1. Create large file (100 MB)
  2. Upload via component
  3. Monitor memory usage
  4. Verify upload completes
  5. Check for memory leaks
  6. Verify browser remains responsive
- **Expected Outcome**: 
  - Large files upload successfully
  - Memory usage is reasonable
  - Browser remains responsive
  - No memory leaks detected
  - Upload completes successfully

### 28.2 Multiple Large Files
**Scenario**: Component handles multiple large files simultaneously
- **Steps**:
  1. Select 10 large files (50 MB each)
  2. Upload all files
  3. Monitor system resources
  4. Verify all upload
  5. Check completion
  6. Verify system stability
- **Expected Outcome**: 
  - Multiple large files upload successfully
  - System resources are used efficiently
  - Uploads complete
  - System remains stable

### 28.3 Large File List
**Scenario**: Component displays many files (1000+) efficiently
- **Steps**:
  1. Programmatically add 1000+ files to list
  2. Verify list renders
  3. Check UI responsiveness
  4. Scroll file list
  5. Verify performance is acceptable
- **Expected Outcome**: 
  - Large file lists render
  - UI remains responsive
  - Scrolling is smooth
  - Performance is acceptable

---

## 29. Custom HttpClient

### 29.1 Custom HttpClient Instance
**Scenario**: Use custom HttpClient with specific configuration
- **Steps**:
  1. Create custom HttpClient with custom headers
  2. Inject custom HttpClient into uploader
  3. Upload file
  4. Verify custom headers are sent
  5. Verify custom client is used
- **Expected Outcome**: 
  - Custom HttpClient is used
  - Custom headers are sent
  - Upload works with custom client
  - Configuration is respected

---

## 30. Edge Cases and Boundary Conditions

### 30.1 Empty File Upload
**Scenario**: 0-byte file is handled
- **Precondition**: MinFileSize=0 to allow empty files
- **Steps**:
  1. Create 0-byte file
  2. Upload empty file
  3. Verify file is accepted
  4. Verify OnValueChange fires with 0-byte file
  5. Verify server receives 0-byte file
- **Expected Outcome**: 
  - 0-byte file is accepted if MinFileSize allows
  - File shows in list
  - Upload completes
  - Server receives empty file

### 30.2 Special Characters in File Names
**Scenario**: Files with special characters are handled
- **Steps**:
  1. Create files with special names:
     - "file@#$%.txt"
     - "file[brackets].pdf"
     - "file(parentheses).doc"
  2. Upload files
  3. Verify file names are handled correctly
  4. Verify no errors occur
  5. Verify special chars are preserved/escaped
- **Expected Outcome**: 
  - Special characters are handled
  - File names are preserved correctly
  - No errors occur
  - Files upload successfully

### 30.3 Very Long File Names
**Scenario**: Files with very long names (255+ characters) are handled
- **Steps**:
  1. Create file with 300-character name
  2. Upload file
  3. Verify file is accepted or error is clear
  4. Check display truncation if needed
  5. Verify server handles long name
- **Expected Outcome**: 
  - Very long names are handled
  - Error message is clear if too long
  - Display is appropriate (truncation if needed)
  - File operations work correctly

### 30.4 Rapid File Selection
**Scenario**: User rapidly selects and removes files
- **Steps**:
  1. Rapidly select files (multiple quick selections)
  2. Rapidly remove files
  3. Verify component remains stable
  4. Verify file list is accurate
  5. Verify no errors or crashes
- **Expected Outcome**: 
  - Rapid operations are handled
  - File list remains accurate
  - No race conditions occur
  - Component is stable

### 30.5 Duplicate File Selection
**Scenario**: User selects same file multiple times
- **Steps**:
  1. Select "document.pdf"
  2. Upload document
  3. Select same "document.pdf" again
  4. Verify both entries appear in list (or handled appropriately)
  5. Verify both can be uploaded
- **Expected Outcome**: 
  - Duplicate files are allowed
  - Each instance is treated separately
  - Both can be uploaded
  - System doesn't deduplicate automatically

---

## Test Execution Guidelines

### Prerequisites
- Blazor application running with uploader components
- Test data files prepared (various sizes, types, formats)
- Backend endpoints configured (SaveUrl, RemoveUrl)
- Browser developer tools available
- Accessibility testing tools ready (WAVE, Axe, etc.)

### Test Data Requirements
- Sample image files: .jpg, .png, .gif (various sizes)
- Document files: .pdf, .docx, .xlsx, .pptx
- Text files: .txt, .csv, .xml
- Executable files: .exe, .dll (for extension validation)
- Large files: 100+ MB (for performance testing)
- Empty files (0 bytes)
- Files with special characters

### Test Environment
- Multiple browsers: Chrome, Firefox, Safari, Edge
- Multiple devices: Desktop, Tablet, Mobile
- Network conditions: Normal, Slow 3G, Offline simulation
- Accessibility tools: Screen reader, keyboard only

### Reporting
- Document pass/fail status for each scenario
- Include screenshots/videos for failures
- Report browser and OS information
- Include console errors and warnings
- Record performance metrics
- Note any unexpected behaviors
- Provide recommendations for fixes

---

## End of Test Plan
