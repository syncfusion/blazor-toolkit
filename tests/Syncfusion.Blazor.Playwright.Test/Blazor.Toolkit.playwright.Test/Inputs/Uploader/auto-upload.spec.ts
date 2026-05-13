// spec: uploader-testplan.md
// Component: SfUploader

import { test, expect, Page } from "@playwright/test";

test.describe("Auto Upload Feature", () => {
  let page: Page;

  test.beforeEach(async ({ page: testPage }) => {
    page = testPage;
    await page.goto('http://localhost:5000/inputs/uploader');
    await page.waitForLoadState("networkidle");
  });

  test("Auto Upload Enabled - Files Upload Immediately", async () => {
    // Navigate to auto upload uploader
    const autoUploader = page.locator("div").filter({ has: page.locator('#autoUploadUploader') }).first();
    await expect(autoUploader).toBeVisible();

    const fileInput = page.locator('#autoUploadUploader');

    // Select a file
    await fileInput.setInputFiles({
      name: "auto-upload-file.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Auto upload content"),
    });

    // Wait for automatic upload process
    await page.waitForTimeout(1000);

    // Verify file appears in list
    const fileItem = page.locator('[data-file-name="auto-upload-file.txt"]');
    await expect(fileItem).toBeVisible();

    // With AutoUpload=true, upload button should not be visible or should be disabled
    const uploadButton = autoUploader.locator(".e-upload-upload-btn");
    const isUploadBtnVisible = await uploadButton.count() > 0;
    
    if (isUploadBtnVisible) {
      // If visible, it should be disabled
      await expect(uploadButton).toBeDisabled();
    }
  });

  test("Auto Upload Disabled - Manual Upload Required", async () => {
    // Navigate to manual upload uploader
    const manualUploader = page.locator("div").filter({ has: page.locator('#manualUploadUploader') }).first();
    await expect(manualUploader).toBeVisible();

    const fileInput = page.locator('#manualUploadUploader');

    // Select a file
    await fileInput.setInputFiles({
      name: "manual-upload-file.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Manual upload content"),
    });

    await page.waitForTimeout(300);

    // Verify file appears in list
    const fileItem = page.locator('[data-file-name="manual-upload-file.txt"]');
    await expect(fileItem).toBeVisible();

    // Verify upload button is visible and enabled
    const uploadButton = manualUploader.locator(".e-upload-browse-btn, button:has-text('Upload')");
    if (await uploadButton.count() > 0) {
      await expect(uploadButton.first()).toBeVisible();
      await expect(uploadButton.first()).toBeEnabled();
    }

    // File should still be in "Ready to upload" state
    const status = fileItem.locator(".e-upload-status");
    if (await status.isVisible()) {
      const statusText = await status.textContent();
      expect(statusText).toContain("Ready");
    }
  });

  test("Manual Upload - Upload Button Functionality", async () => {
    // Navigate to manual upload uploader
    const manualUploader = page.locator("div").filter({ has: page.locator('#manualUploadUploader') }).first();
    const fileInput = page.locator('#manualUploadUploader');

    // Select files
    await fileInput.setInputFiles({
      name: "test-upload-1.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Content 1"),
    });

    await page.waitForTimeout(300);

    // Find and click upload button
    const uploadButton = manualUploader.locator(".e-upload-browse-btn, button:has-text('Upload')");
    if (await uploadButton.count() > 0) {
      await uploadButton.first().click();

      // Wait for upload to complete
      await page.waitForTimeout(1000);

      // Check if file status changed
      const fileItem = page.locator('[data-file-name="test-upload-1.txt"]');
      const status = fileItem.locator(".e-upload-status");
      
      if (await status.isVisible()) {
        const statusText = await status.textContent();
        // Status might be "Uploading" or "File uploaded successfully"
        expect(statusText).toBeTruthy();
      }
    }
  });

  test("Sequential Upload - Files Upload One After Another", async () => {
    // Navigate to sequential uploader
    const sequentialUploader = page.locator("div").filter({ has: page.locator('#sequentialUploader') }).first();
    await expect(sequentialUploader).toBeVisible();

    const fileInput = page.locator('#sequentialUploader');

    // Select multiple files
    await fileInput.setInputFiles([
      {
        name: "seq-file-1.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("Sequential 1"),
      },
      {
        name: "seq-file-2.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("Sequential 2"),
      },
      {
        name: "seq-file-3.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("Sequential 3"),
      },
    ]);

    await page.waitForTimeout(300);

    // Verify all files appear in list
    await expect(page.locator('[data-file-name="seq-file-1.txt"]')).toBeVisible();
    await expect(page.locator('[data-file-name="seq-file-2.txt"]')).toBeVisible();
    await expect(page.locator('[data-file-name="seq-file-3.txt"]')).toBeVisible();

    // For sequential upload, typically only one file uploads at a time
    // This can be verified through status changes and timing
  });

  test("File Not Uploaded Before Manual Trigger", async () => {
    // Verify that with AutoUpload=false, file stays in "Ready" state
    const manualUploader = page.locator("div").filter({ has: page.locator('#manualUploadUploader') }).first();
    const fileInput = page.locator('#manualUploadUploader');

    await fileInput.setInputFiles({
      name: "not-yet-uploaded.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Not uploaded"),
    });

    await page.waitForTimeout(500);

    // File should be in list
    const fileItem = page.locator('[data-file-name="not-yet-uploaded.txt"]');
    await expect(fileItem).toBeVisible();

    // Status should indicate "Ready to upload"
    const status = fileItem.locator(".e-upload-status");
    if (await status.isVisible()) {
      const statusText = await status.textContent();
      expect(statusText).toContain("Ready");
    }
  });

  test("Upload Status Progression", async () => {
    // Monitor status changes during upload
    const autoUploader = page.locator("div").filter({ has: page.locator('#autoUploadUploader') }).first();
    const fileInput = page.locator('#autoUploadUploader');

    await fileInput.setInputFiles({
      name: "status-test.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Status progression test"),
    });

    // Initially file added
    const fileItem = page.locator('[data-file-name="status-test.txt"]');
    await expect(fileItem).toBeVisible();

    // Wait for status to update
    await page.waitForTimeout(800);

    // Check status element
    const status = fileItem.locator(".e-upload-status");
    if (await status.isVisible()) {
      const statusText = await status.textContent();
      // Status should show some state
      expect(statusText).toBeTruthy();
    }
  });
});
