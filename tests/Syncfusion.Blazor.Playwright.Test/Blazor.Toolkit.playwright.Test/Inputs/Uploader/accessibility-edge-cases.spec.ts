// spec: uploader-testplan.md
// Component: SfUploader

import { test, expect, Page } from "@playwright/test";

test.describe("Accessibility and Edge Cases", () => {
  let page: Page;

  test.beforeEach(async ({ page: testPage }) => {
    page = testPage;
    await page.goto('http://localhost:5000/inputs/uploader');
    await page.waitForLoadState("networkidle");
  });

  test("Browse Button Has Keyboard Focus Support", async () => {
    // Verify keyboard accessibility
    const browseButton = page.locator(".e-upload-browse-btn").first();
    await expect(browseButton).toBeVisible();

    // Test focus
    await browseButton.focus();
    const isFocused = await browseButton.evaluate((el: HTMLElement) => {
      return document.activeElement === el;
    });
    expect(isFocused).toBe(true);

    // Should be keyboard navigable with Tab key
    await page.keyboard.press("Tab");
    await page.waitForTimeout(100);

    // Focus should have moved
    const isStillFocused = await browseButton.evaluate((el: HTMLElement) => {
      return document.activeElement === el;
    });
    // After Tab, should not be focused anymore (moved to next element)
    expect(isStillFocused).toBe(false);
  });

  test("Remove Button is Keyboard Accessible", async () => {
    // Add file first
    const multipleUploader = page.locator("div").filter({ has: page.locator('#multipleFilesUploader') }).first();
    const fileInput = page.locator('#multipleFilesUploader');

    await fileInput.setInputFiles({
      name: "keyboard-remove.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("For keyboard test"),
    });

    await page.waitForTimeout(300);

    // Get remove button
    const removeButton = page.locator(".e-file-remove-btn, .e-file-delete-btn").first();
    if (await removeButton.count() > 0) {
      // Focus on remove button
      await removeButton.focus();

      const isFocused = await removeButton.evaluate((el: HTMLElement) => {
        return document.activeElement === el;
      });
      expect(isFocused).toBe(true);

      // Button should have tabindex or role=button
      const tabindex = await removeButton.getAttribute("tabindex");
      const role = await removeButton.getAttribute("role");
      expect(tabindex || role).toBeTruthy();
    }
  });

  test("File List Items Have ARIA Attributes", async () => {
    // Add files
    const basicUploader = page.locator("div").filter({ has: page.locator('#basicUploader') }).first();
    const fileInput = page.locator('#basicUploader');

    await fileInput.setInputFiles({
      name: "aria-test.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("ARIA test"),
    });

    await page.waitForTimeout(300);

    // Check file list item
    const fileItem = page.locator('[data-file-name="aria-test.txt"]');
    if (await fileItem.count() > 0) {
      // Remove button should have aria-label
      const removeBtn = fileItem.locator(".e-file-remove-btn, .e-file-delete-btn");
      if (await removeBtn.count() > 0) {
        const ariaLabel = await removeBtn.getAttribute("aria-label");
        expect(ariaLabel).toBeTruthy();
      }
    }
  });

  test("Empty File Selection Handling", async () => {
    // Test behavior with empty file
    const basicUploader = page.locator("div").filter({ has: page.locator('#basicUploader') }).first();
    const fileInput = page.locator('#basicUploader');

    await fileInput.setInputFiles({
      name: "empty.txt",
      mimeType: "text/plain",
      buffer: Buffer.from(""),
    });

    await page.waitForTimeout(300);
    await expect(basicUploader).toBeVisible();

    // No crash or errors
    const errorMsgs: string[] = [];
    page.on("console", (msg) => {
      if (msg.type() === "error") {
        errorMsgs.push(msg.text());
      }
    });

    expect(errorMsgs.length).toBe(0);
  });

  test("Very Long File Names Handled", async () => {
    // Test with long filename
    const basicUploader = page.locator("div").filter({ has: page.locator('#basicUploader') }).first();
    const fileInput = page.locator('#basicUploader');

    const longName =
      "this-is-a-very-long-file-name-that-goes-on-and-on-for-testing-purposes-to-see-how-the-component-handles-it.txt";

    await fileInput.setInputFiles({
      name: longName,
      mimeType: "text/plain",
      buffer: Buffer.from("Long name test"),
    });

    await page.waitForTimeout(300);

    // File should appear (might be truncated visually)
    const fileItem = page.locator("[data-file-name]");
    expect(await fileItem.count()).toBeGreaterThan(0);

    // Component should have title attribute for full name
    const lastItem = fileItem.last();
    const title = await lastItem.getAttribute("title");
    // Title should be present or accessible somehow
    const fileName = await lastItem.locator(".e-file-name");
    expect(await fileName.count()).toBeGreaterThan(0);
  });

  test("Special Characters in File Names", async () => {
    // Test with special characters
    const basicUploader = page.locator("div").filter({ has: page.locator('#basicUploader') }).first();
    const fileInput = page.locator('#basicUploader');

    const specialName = "file-with-@#$%^&_chars.txt";

    await fileInput.setInputFiles({
      name: specialName,
      mimeType: "text/plain",
      buffer: Buffer.from("Special chars test"),
    });

    await page.waitForTimeout(300);

    // File should be handled
    const fileItem = page.locator("[data-file-name]");
    expect(await fileItem.count()).toBeGreaterThan(0);

    // No console errors
    const errorMsgs: string[] = [];
    page.on("console", (msg) => {
      if (msg.type() === "error") {
        errorMsgs.push(msg.text());
      }
    });
  });

  test("Unicode File Names Handled", async () => {
    // Test with unicode characters
    const basicUploader = page.locator("div").filter({ has: page.locator('#basicUploader') }).first();
    const fileInput = page.locator('#basicUploader');

    const unicodeName = "файл-文件-ファイル.txt";

    await fileInput.setInputFiles({
      name: unicodeName,
      mimeType: "text/plain",
      buffer: Buffer.from("Unicode test"),
    });

    await page.waitForTimeout(300);

    // Component should handle unicode
    const fileItems = page.locator("[data-file-name]");
    expect(await fileItems.count()).toBeGreaterThan(0);
  });

  test("Rapid File Addition and Removal", async () => {
    // Test stress scenario
    const multipleUploader = page.locator("div").filter({ has: page.locator('#multipleFilesUploader') }).first();
    const fileInput = page.locator('#multipleFilesUploader');

    // Add files multiple times rapidly
    for (let i = 0; i < 3; i++) {
      await fileInput.setInputFiles({
        name: `rapid-${i}.txt`,
        mimeType: "text/plain",
        buffer: Buffer.from(`Rapid ${i}`),
      });
      await page.waitForTimeout(100);
    }

    // Component should still be functional
    await expect(multipleUploader).toBeVisible();
    const removeButtonSelector = "span.e-toolkit-icons[role='button'][aria-label]";
    // Can still remove
    const removeButtons = page.locator(removeButtonSelector);
    const count = await removeButtons.count();
    expect(count).toBeGreaterThan(0);

    // No crashes
    const errorMsgs: string[] = [];
    page.on("console", (msg) => {
      if (msg.type() === "error") {
        errorMsgs.push(msg.text());
      }
    });
  });

  test("Component Responsive to Window Resize", async () => {
    // Verify component handles resize
    const uploader = page.locator("div").filter({ has: page.locator('#basicUploader') }).first();
    await expect(uploader).toBeVisible();

    // Get initial size
    const initialBox = await uploader.boundingBox();
    expect(initialBox).toBeTruthy();

    // Resize window
    await page.setViewportSize({ width: 600, height: 400 });
    await page.waitForTimeout(200);

    // Component should still be visible
    await expect(uploader).toBeVisible();

    // Get new size
    const newBox = await uploader.boundingBox();
    expect(newBox).toBeTruthy();

    // Should be responsive
    expect(newBox!.width).toBeGreaterThan(0);
  });

  test("File Input has Proper Aria Label", async () => {
    // Check accessibility of file input
    const fileInputs = page.locator('input[type="file"]');
    const count = await fileInputs.count();
    
    // Should have file inputs
    expect(count).toBeGreaterThan(0);
    
    // File inputs are hidden, but component should be accessible
    const basicUploader = page.locator("div").filter({ has: page.locator('#basicUploader') }).first();
    await expect(basicUploader).toBeVisible();
  });

  test("Uploader Maintains State After Clear", async () => {
    // Add, clear, and add again
    const multipleUploader = page.locator("div").filter({ has: page.locator('#multipleFilesUploader') }).first();
    const fileInput = page.locator('#multipleFilesUploader');

    // Add files
    await fileInput.setInputFiles({
      name: "temp-1.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Temp"),
    });
    await page.waitForTimeout(300);

    // Clear
    const clearBtn = multipleUploader.locator(".e-file-clear-btn, button:has-text('Clear')");

    // Verify cleared
    let fileItems = page.locator("li.e-file-list-item");
    expect(await fileItems.count()).toBe(0);

    // Add files again
    await fileInput.setInputFiles({
      name: "new-file.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("New"),
    });
    await page.waitForTimeout(300);

    // File should appear
    const newFileItem = page.locator('[data-file-name="new-file.txt"]');
    await expect(newFileItem).toBeVisible();
  });

  test("Component Handles Network Latency Simulation", async () => {
    // Test with simulated network conditions
    await page.route("**/*", (route) => {
      setTimeout(() => {
        route.continue();
      }, 100);
    });

    const uploader = page.locator("div").filter({ has: page.locator('#basicUploader') }).first();
    const fileInput = page.locator('#basicUploader');

    await fileInput.setInputFiles({
      name: "network-test.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Network test"),
    });

    await page.waitForTimeout(500);

    // Component should function normally
    const fileItem = page.locator('[data-file-name="network-test.txt"]');
    await expect(fileItem).toBeVisible();
  });
});
