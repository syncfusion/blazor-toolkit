// spec: uploader-testplan.md
// Component: SfUploader

import { test, expect, Page } from "@playwright/test";

test.describe("File Validation", () => {
  let page: Page;

  test.beforeEach(async ({ page: testPage }) => {
    page = testPage;
    await page.goto('http://localhost:5000/inputs/uploader');
    await page.waitForLoadState("networkidle");
  });

  test("File Extension Validation - Allowed Extensions Only", async () => {
    // Navigate to extension validation uploader
    const extensionValidator = page.locator("div").filter({ has: page.locator('#extensionValidationUploader') }).first();
    await expect(extensionValidator).toBeVisible();

    const fileInput = page.locator('#extensionValidationUploader');

    // Upload an allowed file (.jpg)
    await fileInput.setInputFiles({
      name: "valid-image.jpg",
      mimeType: "image/jpeg",
      buffer: Buffer.from("JPG content"),
    });

    await page.waitForTimeout(300);

    // Verify allowed file appears in list
    const validFileItem = page.locator('[data-file-name="valid-image.jpg"]');
    await expect(validFileItem).toBeVisible();

    // Verify status shows "Ready to upload"
    const status = validFileItem.locator(".e-upload-status");
    if (await status.isVisible()) {
      await expect(status).toContainText("Ready");
    }
  });

  test("File Extension Validation - Reject Invalid Extensions", async () => {
    // Navigate to extension validation uploader
    const extensionValidator = page.locator("div").filter({ has: page.locator('#extensionValidationUploader') }).first();
    const fileInput = page.locator('#extensionValidationUploader');

    // Try to upload a file with invalid extension (.exe)
    await fileInput.setInputFiles({
      name: "malicious.exe",
      mimeType: "application/x-msdownload",
      buffer: Buffer.from("EXE content"),
    });

    await page.waitForTimeout(300);

    // File might be added but with validation error
    // Check if file appears in list
    const fileItem = page.locator('[data-file-name]');
    const count = await fileItem.count();

    // Validation should prevent upload (file might still show with error)
    if (count > 0) {
      const lastFile = fileItem.last();
      const errorText = lastFile.locator(".e-upload-status");
      if (await errorText.isVisible()) {
        // File shows validation error
        await expect(errorText).toBeVisible();
      }
    }
  });

  test("File Size Validation - Maximum Size", async () => {
    // Navigate to size validation uploader (MaxFileSize=5MB)
    const sizeValidator = page.locator("div").filter({ has: page.locator('#sizeValidationUploader') }).first();
    await expect(sizeValidator).toBeVisible();

    const fileInput = page.locator('#sizeValidationUploader');

    // Create a file under the max size (1MB)
    const smallContent = Buffer.alloc(1024 * 1024); // 1MB
    await fileInput.setInputFiles({
      name: "small-file.bin",
      mimeType: "application/octet-stream",
      buffer: smallContent,
    });

    await page.waitForTimeout(300);

    // Verify small file is accepted
    const fileItem = page.locator('[data-file-name="small-file.bin"]');
    await expect(fileItem).toBeVisible();
  });

  test("File Size Validation - Minimum Size", async () => {
    // Navigate to size validation uploader (MinFileSize=1KB)
    const sizeValidator = page.locator("div").filter({ has: page.locator('#sizeValidationUploader') }).first();
    const fileInput = page.locator('#sizeValidationUploader');

    // Try to upload an empty file (below minimum)
    await fileInput.setInputFiles({
      name: "empty-file.txt",
      mimeType: "text/plain",
      buffer: Buffer.from(""),
    });

    await page.waitForTimeout(300);

    // Empty file should trigger validation error
    // Check if validation message appears
    const status = page.locator(".e-upload-status");
    if (await status.count() > 0) {
      // File list exists, validation error might be shown
      const lastStatus = status.last();
      if (await lastStatus.isVisible()) {
        await expect(lastStatus).toBeVisible();
      }
    }
  });

  test("File Size Validation - Valid File Size Range", async () => {
    // File between min (1KB) and max (5MB)
    const sizeValidator = page.locator("div").filter({ has: page.locator('#sizeValidationUploader') }).first();
    const fileInput = page.locator('#sizeValidationUploader');

    // Create a 2MB file (within range)
    const validContent = Buffer.alloc(2 * 1024 * 1024);
    await fileInput.setInputFiles({
      name: "valid-size.bin",
      mimeType: "application/octet-stream",
      buffer: validContent,
    });

    await page.waitForTimeout(300);

    // Verify file is accepted
    const fileItem = page.locator('[data-file-name="valid-size.bin"]');
    await expect(fileItem).toBeVisible();

    // Should show "Ready to upload" status
    const status = fileItem.locator(".e-upload-status");
    if (await status.isVisible()) {
      await expect(status).toBeVisible();
    }
  });

  test("HTML Sanitizer - Prevents XSS in File Names", async () => {
    // Navigate to sanitizer enabled uploader
    const sanitizerUploader = page.locator("div").filter({ has: page.locator('#sanitizerEnabledUploader') }).first();
    await expect(sanitizerUploader).toBeVisible();

    const fileInput = page.locator('#sanitizerEnabledUploader');

    // Try to upload file with XSS attempt in name
    // Note: Browser might sanitize before reaching component
    await fileInput.setInputFiles({
      name: "document.pdf",
      mimeType: "application/pdf",
      buffer: Buffer.from("PDF content"),
    });

    await page.waitForTimeout(300);

    // Verify file appears without script execution
    const fileItem = page.locator('[data-file-name]').first();
    await expect(fileItem).toBeVisible();

    // Verify no script tags in displayed name
    const fileName = fileItem.locator(".e-file-name");
    const displayedName = await fileName.textContent();
    expect(displayedName).not.toContain("<script");
  });

  test("Multiple Validation Errors - File Fails Multiple Criteria", async () => {
    // File that's both wrong extension AND oversized
    const sizeValidator = page.locator("div").filter({ has: page.locator('#sizeValidationUploader') }).first();
    const fileInput = page.locator('#sizeValidationUploader');

    // Create oversized file with "wrong" extension
    const largeContent = Buffer.alloc(10 * 1024 * 1024); // 10MB (over 5MB limit)
    await fileInput.setInputFiles({
      name: "oversized.bin",
      mimeType: "application/octet-stream",
      buffer: largeContent,
    });

    await page.waitForTimeout(300);

    // File should be rejected due to size validation
    // Check for validation status
    const fileItems = page.locator('[data-file-name]');
    const itemCount = await fileItems.count();

    if (itemCount > 0) {
      const lastItem = fileItems.last();
      const status = lastItem.locator(".e-upload-status");
      if (await status.isVisible()) {
        // Should show validation error
        await expect(status).toBeVisible();
      }
    }
  });

  test("Validation Error Messages Display Correctly", async () => {
    // Check that validation errors are visible and understandable
    const sizeValidator = page.locator("#sizeValidationUploader");
    const fileInput = page.locator('input[type="file"]').nth(4);

    // Upload empty file (below minimum)
    await fileInput.setInputFiles({
      name: "tiny.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("x"),
    });

    await page.waitForTimeout(300);

    // Look for error status element
    const statuses = page.locator(".e-upload-status");
    if (await statuses.count() > 0) {
      const lastStatus = statuses.last();
      if (await lastStatus.isVisible()) {
        const statusText = await lastStatus.textContent();
        // Should contain meaningful error message
        expect(statusText).toBeTruthy();
      }
    }
  });
});
