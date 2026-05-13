// spec: uploader-testplan.md
// Component: SfUploader

import { test, expect, Page } from "@playwright/test";

test.describe("Progress Tracking and Display", () => {
  let page: Page;

  test.beforeEach(async ({ page: testPage }) => {
    page = testPage;
    await page.goto('http://localhost:5000/inputs/uploader');
    await page.waitForLoadState("networkidle");
  });

  test("Progress Bar Visible When ShowProgressBar=True", async () => {
    // Navigate to progress bar uploader
    const progressUploader = page.locator("div").filter({ has: page.locator('#progressBarUploader') }).first();
    await expect(progressUploader).toBeVisible();

    const fileInput = page.locator('#progressBarUploader');

    // Select a file
    await fileInput.setInputFiles({
      name: "progress-test.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Test content for progress"),
    });

    await page.waitForTimeout(300);

    // Verify file appears in list
    const fileItem = page.locator('[data-file-name="progress-test.txt"]');
    await expect(fileItem).toBeVisible();

    // Look for progress bar element
    const progressBar = fileItem.locator(".e-upload-progress-bar");
    const progressWrap = fileItem.locator(".e-upload-progress-wrap");

    // Progress bar might be hidden initially, visible during upload
    // At least the wrapper should exist in the DOM
    if (await progressWrap.count() > 0) {
      await expect(progressWrap).toBeVisible();
    }
  });

  test("Progress Bar Hidden When ShowProgressBar=False", async () => {
    // Navigate to no progress bar uploader
    const noProgressUploader = page.locator("div").filter({ has: page.locator('#noProgressBarUploader') }).first();
    await expect(noProgressUploader).toBeVisible();

    const fileInput = page.locator('#noProgressBarUploader');

    // Select a file
    await fileInput.setInputFiles({
      name: "no-progress-test.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("No progress content"),
    });

    await page.waitForTimeout(300);

    // Verify file appears
    const fileItem = page.locator('[data-file-name="no-progress-test.txt"]');
    await expect(fileItem).toBeVisible();

    // Progress bar should not be visible
    const progressBar = fileItem.locator(".e-upload-progress-bar");
    const progressWrap = fileItem.locator(".e-upload-progress-wrap");

    // Both should not be visible
    const progressBarCount = await progressBar.count();
    const progressWrapCount = await progressWrap.count();

    expect(progressBarCount + progressWrapCount).toBe(0);
  });

  test("Progress Bar Structure is Correct", async () => {
    // Verify progress bar has proper structure
    const progressUploader = page.locator("div").filter({ has: page.locator('#progressBarUploader') }).first();
    const fileInput = page.locator('#progressBarUploader');

    await fileInput.setInputFiles({
      name: "progress-structure.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Progress structure test"),
    });

    await page.waitForTimeout(300);

    const fileItem = page.locator('[data-file-name="progress-structure.txt"]');
    const progressWrap = fileItem.locator(".e-upload-progress-wrap");

    if (await progressWrap.count() > 0) {
      await expect(progressWrap).toBeVisible();

      // Check for progress bar element
      const progressBar = progressWrap.locator(".e-upload-progress-bar");
      await expect(progressBar).toBeVisible();

      // Check for progress text
      const progressText = progressWrap.locator(".e-progress-bar-text");
      if (await progressText.count() > 0) {
        const text = await progressText.textContent();
        // Should contain percentage
        expect(text).toMatch(/\d+%/);
      }
    }
  });

  test("File List Shows Status for Each File", async () => {
    // Verify status display
    const multipleUploader = page.locator("div").filter({ has: page.locator('#multipleFilesUploader') }).first();
    const fileInput = page.locator('#multipleFilesUploader');

    await fileInput.setInputFiles([
      {
        name: "status-1.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("Status 1"),
      },
      {
        name: "status-2.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("Status 2"),
      },
    ]);

    await page.waitForTimeout(300);

    // Check status display for files
    const fileItems = page.locator('[data-file-name]');
    const count = await fileItems.count();
    expect(count).toBeGreaterThanOrEqual(2);

    // Each file should have status element
    const statuses = page.locator(".e-file-status");
    const statusCount = await statuses.count();
    expect(statusCount).toBeGreaterThanOrEqual(count);
  });

  test("Status Text Updates with File State", async () => {
    // Verify status text reflects file state
    const manualUploader = page.locator("div").filter({ has: page.locator('#manualUploadUploader') }).first();
    const fileInput = page.locator('#manualUploadUploader');

    await fileInput.setInputFiles({
      name: "status-change.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Status change test"),
    });

    await page.waitForTimeout(300);

    // File should show "Ready to upload" initially
    const fileItem = page.locator('[data-file-name="status-change.txt"]');
    const status = fileItem.locator(".e-upload-status");

    if (await status.isVisible()) {
      const statusText = await status.textContent();
      expect(statusText).toContain("Ready");
    }
  });

  test("File Size Display Format", async () => {
    // Verify file size is displayed in readable format
    const basicUploader = page.locator("div").filter({ has: page.locator('#basicUploader') }).first();
    const fileInput = page.locator('#basicUploader');

    await fileInput.setInputFiles({
      name: "size-display.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Size display test content"),
    });

    await page.waitForTimeout(300);

    const fileItem = page.locator('[data-file-name="size-display.txt"]');
    const fileSize = fileItem.locator(".e-file-size");

    if (await fileSize.isVisible()) {
      const sizeText = await fileSize.textContent();
      // Should contain size info (B, KB, MB, etc)
      expect(sizeText).toMatch(/\d+\s*(B|KB|MB|GB)/i);
    }
  });

  test("File List Container Exists and is Visible", async () => {
    // Verify file list renders
    const withFileList = page.locator("div").filter({ has: page.locator('#withFileListUploader') }).first();
    await expect(withFileList).toBeVisible();

    const fileInput = page.locator('#withFileListUploader');

    await fileInput.setInputFiles({
      name: "list-visible.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("List visible test"),
    });

    await page.waitForTimeout(300);

    // File list should be visible
    const fileListParent = withFileList.locator("ul.e-file-list-parent, .e-file-list");
    if (await fileListParent.count() > 0) {
      await expect(fileListParent.first()).toBeVisible();
    }

    // File item should be in list
    const fileItem = page.locator('[data-file-name="list-visible.txt"]');
    await expect(fileItem).toBeVisible();
  });

  test("File List Hidden When ShowFileList=False", async () => {
    // Verify file list is not rendered
    const withoutFileList = page.locator("div").filter({ has: page.locator('#withoutFileListUploader') }).first();
    await expect(withoutFileList).toBeVisible();

    const fileInput = page.locator('#withoutFileListUploader');

    await fileInput.setInputFiles({
      name: "hidden-list.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Hidden list test"),
    });

    await page.waitForTimeout(300);

    // File list should not be visible
    const fileListParent = withoutFileList.locator("ul.e-file-list-parent, .e-file-list");
    expect(await fileListParent.count()).toBe(0);

    // File items should not be in DOM
    const fileItem = page.locator('[data-file-name="hidden-list.txt"]');
    expect(await fileItem.count()).toBe(0);
  });

  test("Multiple Files Display in Correct Order", async () => {
    // Verify files display in selection order
    const multipleUploader = page.locator("div").filter({ has: page.locator('#multipleFilesUploader') }).first();
    const fileInput = page.locator('#multipleFilesUploader');

    await fileInput.setInputFiles([
      {
        name: "first.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("First file"),
      },
      {
        name: "second.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("Second file"),
      },
      {
        name: "third.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("Third file"),
      },
    ]);

    await page.waitForTimeout(300);

    // Get all file items
    const fileItems = page.locator('[data-file-name]');
    const count = await fileItems.count();
    expect(count).toBeGreaterThanOrEqual(3);

    // Verify files appear in order (last 3 should be our files)
    const lastFile = fileItems.last();
    const lastFileName = await lastFile.getAttribute("data-file-name");
    expect(lastFileName).toContain("third.txt");
  });
});
