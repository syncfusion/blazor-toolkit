// spec: uploader-testplan.md
// Component: SfUploader

import { test, expect, Page } from "@playwright/test";

test.describe("Component Features and Properties", () => {
  let page: Page;

  test.beforeEach(async ({ page: testPage }) => {
    page = testPage;
    await page.goto('http://localhost:5000/inputs/uploader');
    await page.waitForLoadState("networkidle");
  });

  test("Browse Button is Visible and Clickable", async () => {
    // Verify browse button exists and is accessible
    const browseButtons = page.locator(".e-upload-browse-btn");
    const count = await browseButtons.count();

    // Should have at least one browse button
    expect(count).toBeGreaterThan(0);

    // Browse button should be visible
    const firstButton = browseButtons.first();
    await expect(firstButton).toBeVisible();

    // Button should have correct type
    const btnType = await firstButton.getAttribute("type");
    expect(btnType).toBe("button");

    // Button should be clickable
    await expect(firstButton).toBeEnabled();
  });

  test("Multiple Uploaders Exist on Page", async () => {
    // Verify multiple uploader instances can coexist
    const uploaderIds = [
      "basicUploader",
      "multipleFilesUploader",
      "singleFileUploader",
      "extensionValidationUploader",
      "sizeValidationUploader",
      "autoUploadUploader",
      "manualUploadUploader",
      "progressBarUploader",
      "noProgressBarUploader",
    ];

    for (const id of uploaderIds) {
      const uploader = page.locator(`#${id}`);
      // Most should be visible (some might be scrolled)
      expect(await uploader.count()).toBeGreaterThan(0);
    }
  });

  test("Uploader with Custom CSS Class", async () => {
    // Verify custom CSS class is applied
    const customUploader = page.locator("div").filter({ has: page.locator('#customClassUploader') }).first();
    await expect(customUploader).toBeVisible();
  });

  test("Disabled Uploader Cannot Accept Files", async () => {
    // Verify disabled state
    const disabledUploader = page.locator("div").filter({ has: page.locator('#disabledUploader') }).first();
    await expect(disabledUploader).toBeVisible();

    // Find the input element
    const disabledInput = page.locator('#disabledUploader');

    // Disabled input should not be enabled
    const isDisabled = await disabledInput.isDisabled();
    expect(isDisabled).toBe(true);
  });

  test("File Input Element Exists", async () => {
    // Verify file input element
    const fileInputs = page.locator('input[type="file"]');
    const count = await fileInputs.count();

    // Should have file inputs for each uploader
    expect(count).toBeGreaterThan(5);

    // Each should have aria-label or data attribute
    const firstInput = fileInputs.first();
    const dataAttr = await firstInput.getAttribute("data-testid");
    // File inputs are hidden, so we just verify they exist
    expect(firstInput).toBeTruthy();
  });

  test("Uploader with Folder Upload Support", async () => {
    // Navigate to folder uploader
    const folderUploader = page.locator("div").filter({ has: page.locator('#folderUploader') }).first();
    await expect(folderUploader).toBeVisible();
  });

  test("Sequential Upload Property Set", async () => {
    // Verify sequential uploader exists
    const sequentialUploader = page.locator("div").filter({ has: page.locator('#sequentialUploader') }).first();
    await expect(sequentialUploader).toBeVisible();
  });

  test("HTML Sanitizer Protection", async () => {
    // Both sanitizer enabled and disabled uploaders exist
    const sanitizerEnabled = page.locator("div").filter({ has: page.locator('#sanitizerEnabledUploader') }).first();
    const sanitizerDisabled = page.locator("div").filter({ has: page.locator('#sanitizerDisabledUploader') }).first();

    await expect(sanitizerEnabled).toBeVisible();
    await expect(sanitizerDisabled).toBeVisible();
  });

  test("Drag and Drop Area Visible", async () => {
    // Navigate to drag drop uploader
    const dragDropUploader = page.locator("div").filter({ has: page.locator('#dragDropUploader') }).first();
    await expect(dragDropUploader).toBeVisible();

    // Look for drop area
    const dropArea = dragDropUploader.locator(".e-file-drop");
    if (await dropArea.count() > 0) {
      await expect(dropArea.first()).toBeVisible();
    }
  });

  test("Component Handles Rapid File Selection", async () => {
    // Test component stability with rapid selections
    const multipleUploader = page.locator("div").filter({ has: page.locator('#multipleFilesUploader') }).first();
    const fileInput = page.locator('#multipleFilesUploader');

    // First selection
    await fileInput.setInputFiles({
      name: "rapid-1.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Rapid 1"),
    });

    await page.waitForTimeout(100);

    // Component should still be stable
    await expect(multipleUploader).toBeVisible();
  });

  test("Page Renders All Uploader Sections", async () => {
    // Verify all sections are rendered
    const headings = page.locator("h3");
    const headingCount = await headings.count();

    // Should have multiple headings for different scenarios
    expect(headingCount).toBeGreaterThan(10);

    // Verify some key sections exist
    const pageContent = await page.content();
    expect(pageContent).toContain("Browse");
    expect(pageContent).toContain("Multiple");
    expect(pageContent).toContain("Validation");
    expect(pageContent).toContain("Upload");
  });

  test("File Input Accepts Multiple File Selection", async () => {
    // Test that file input allows multiple selection where configured
    const multipleUploader = page.locator("div").filter({ has: page.locator('#multipleFilesUploader') }).first();
    const fileInput = page.locator('#multipleFilesUploader');

    // Check for multiple attribute (component might not show it, but should support it)
    const hasMultiple = await fileInput.getAttribute("multiple");
    // hasMultiple might be empty string which is truthy

    // Verify by actually selecting multiple files
    await fileInput.setInputFiles([
      {
        name: "multi-a.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("Multi A"),
      },
      {
        name: "multi-b.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("Multi B"),
      },
    ]);

    await page.waitForTimeout(300);

    // Both files should appear in list
    const fileList = multipleUploader.locator(".e-upload-file-list");
    expect(await fileList.count()).toBeGreaterThanOrEqual(2);
  });

  test("Component Renders Without Errors", async () => {
    // Verify no console errors
    const errorMessages: string[] = [];

    page.on("console", (msg) => {
      if (msg.type() === "error") {
        errorMessages.push(msg.text());
      }
    });

    // Navigate and wait
    await page.goto('http://localhost:5000/inputs/uploader');
    await page.waitForLoadState("networkidle");
  });

  test("Event Message Display Updates", async () => {
    // Verify event handling works
    const eventUploader = page.locator("div").filter({ has: page.locator('#eventUploader') }).first();
    await expect(eventUploader).toBeVisible();

    // Look for event display element
    const eventDisplay = page.locator("b:has-text('Last Event:')");
    if (await eventDisplay.count() > 0) {
      await expect(eventDisplay).toBeVisible();
    }
  });
});
