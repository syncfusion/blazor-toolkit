// spec: uploader-testplan.md
// Component: SfUploader

import { test, expect, Page } from "@playwright/test";

test.describe("File Selection and Browse Functionality", () => {
  let page: Page;

  test.beforeEach(async ({ page: testPage }) => {
    page = testPage;
    await page.goto('http://localhost:5000/inputs/uploader');
    await page.waitForLoadState("networkidle");
  });

  test("Single File Selection via Browse Button", async () => {
    // Locate the uploader container (the div wrapper)
    const basicUploader = page.locator('div:has(#basicUploader)').first();
    await expect(basicUploader).toBeVisible();

    // Get the input file element (file inputs are always hidden, so we don't check visibility)
    const fileInput = page.locator('#basicUploader');
    
    // Create a test file
    const testFilePath = "test-file.txt";
    await fileInput.setInputFiles({
      name: "test-file.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("This is a test file content"),
    });

    // Wait for file to appear in the list
    const fileListItem = page.locator('[data-file-name="test-file.txt"]').first();
    await expect(fileListItem).toBeVisible();

    // Verify file name is displayed
    const fileName = page.locator(".e-file-name").first();
    await expect(fileName).toContainText("test-file");

    // Verify file size is displayed
    const fileSize = page.locator(".e-file-size").first();
    await expect(fileSize).toBeVisible();
  });

  test("Multiple Files Selection via Browse Button", async () => {
    // Navigate to multiple files uploader
    const multipleUploader = page.locator('div:has(#multipleFilesUploader)').first();
    await expect(multipleUploader).toBeVisible();

    const fileInput = page.locator('#multipleFilesUploader');

    // Select multiple files
    await fileInput.setInputFiles([
      {
        name: "file1.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("File 1 content"),
      },
      {
        name: "file2.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("File 2 content"),
      },
      {
        name: "file3.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("File 3 content"),
      },
    ]);

    // Wait for files to be added
    await page.waitForTimeout(500);

    // Verify all files appear in the list
    const fileListItems = page.locator('[data-file-name]');
    const count = await fileListItems.count();
    expect(count).toBeGreaterThanOrEqual(3);

    // Verify specific files are present
    await expect(page.locator('[data-file-name="file1.txt"]')).toBeVisible();
    await expect(page.locator('[data-file-name="file2.txt"]')).toBeVisible();
    await expect(page.locator('[data-file-name="file3.txt"]')).toBeVisible();
  });

  test("Single File Selection When AllowMultiple=False", async () => {
    // Navigate to single file uploader
    const singleUploader = page.locator('div:has(#singleFileUploader)').first();
    await expect(singleUploader).toBeVisible();

    const fileInput = page.locator('#singleFileUploader');

    // Select first file
    await fileInput.setInputFiles({
      name: "single-file.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Single file content"),
    });

    // Wait for file to appear
    await page.waitForTimeout(300);

    // Verify file is added to list
    const fileListItem = page.locator('[data-file-name="single-file.txt"]');
    await expect(fileListItem).toBeVisible();

    // Verify file count is 1
    const fileListItems = page.locator('[data-file-name]');
    expect(await fileListItems.count()).toBe(1);
  });

  test("Verify Browse Button Accessibility", async () => {
    // Find browse button
    const browseButton = page.locator(".e-upload-browse-btn").first();
    await expect(browseButton).toBeVisible();

    // Verify button is keyboard accessible (has role and attributes)
    const buttonType = await browseButton.getAttribute("type");
    expect(buttonType).toBe("button");

    // Test keyboard focus
    await browseButton.focus();
    const isFocused = await browseButton.evaluate((el: HTMLElement) => {
      return document.activeElement === el;
    });
    expect(isFocused).toBe(true);

    // Verify button can be clicked
    await expect(browseButton).toBeEnabled();
  });

  test("File List Shows Correct File Information", async () => {
    // Select a file with known size
    const basicUploader = page.locator("#basicUploader");
    const fileInput = page.locator('input[type="file"]').first();

    const testContent = "This is test content with specific size";
    await fileInput.setInputFiles({
      name: "info-test.txt",
      mimeType: "text/plain",
      buffer: Buffer.from(testContent),
    });

    // Wait for file to appear
    await page.waitForTimeout(300);

    // Verify file item container exists
    const fileContainer = page.locator(".e-file-container").first();
    await expect(fileContainer).toBeVisible();

    // Verify file name is displayed
    const fileName = page.locator(".e-file-name").first();
    await expect(fileName).toContainText("info-test");

    // Verify file extension is shown
    const fileType = page.locator(".e-file-type").first();
    await expect(fileType).toContainText(".txt");

    // Verify file size is displayed
    const fileSize = page.locator(".e-file-size").first();
    await expect(fileSize).toBeVisible();
  });
});
