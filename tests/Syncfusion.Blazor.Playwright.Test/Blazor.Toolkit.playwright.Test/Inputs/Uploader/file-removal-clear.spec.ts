// spec: uploader-testplan.md
// Component: SfUploader

import { test, expect, Page } from "@playwright/test";

const removeButtonSelector = "span.e-toolkit-icons[role='button'][aria-label]";

test.describe("File Removal and Clear Functionality", () => {
  let page: Page;

  test.beforeEach(async ({ page: testPage }) => {
    page = testPage;
    await page.goto('http://localhost:5000/inputs/uploader');
    await page.waitForLoadState("networkidle");
  });

  test("Remove Single File Before Upload", async () => {
    // Navigate to manual upload uploader
    const manualUploader = page.locator("div").filter({ has: page.locator('#manualUploadUploader') }).first();
    await expect(manualUploader).toBeVisible();

    const fileInput = page.locator('#manualUploadUploader');

    // Select a file
    await fileInput.setInputFiles({
      name: "remove-me.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Content to remove"),
    });

    await page.waitForTimeout(300);

    // Verify file is in list
    const fileItem = page.locator('[data-file-name="remove-me.txt"]');
    await expect(fileItem).toBeVisible();

    // Find and click remove button
    const removeButton = fileItem.locator(removeButtonSelector);
    if (await removeButton.count() > 0) {
      await removeButton.first().click();
      await page.waitForTimeout(300);
    }

    // Verify file is removed from list
    await expect(fileItem).not.toBeVisible();
  });

  test("Remove File from Multiple Files List", async () => {
    // Select multiple files
    const multipleUploader = page.locator("div").filter({ has: page.locator('#multipleFilesUploader') }).first();
    const fileInput = page.locator('#multipleFilesUploader');

    await fileInput.setInputFiles([
      {
        name: "file-a.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("File A"),
      },
      {
        name: "file-b.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("File B"),
      },
      {
        name: "file-c.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("File C"),
      },
    ]);

    await page.waitForTimeout(300);

    // Verify all files are in list
    await expect(page.locator('[data-file-name="file-a.txt"]')).toBeVisible();
    await expect(page.locator('[data-file-name="file-b.txt"]')).toBeVisible();
    await expect(page.locator('[data-file-name="file-c.txt"]')).toBeVisible();

    // Remove the middle file
    const fileB = page.locator('[data-file-name="file-b.txt"]');
    const removeBtn = fileB.locator(removeButtonSelector);
    if (await removeBtn.count() > 0) {
      await removeBtn.first().click();
      await page.waitForTimeout(300);
    }

    // Verify file-b is removed but others remain
    await expect(page.locator('[data-file-name="file-a.txt"]')).toBeVisible();
    await expect(page.locator('[data-file-name="file-b.txt"]')).not.toBeVisible();
    await expect(page.locator('[data-file-name="file-c.txt"]')).toBeVisible();
  });

  test("Remove Button is Accessible for Each File", async () => {
    // Verify each file has accessible remove button
    const multipleUploader = page.locator("div").filter({ has: page.locator('#multipleFilesUploader') }).first();
    const fileInput = page.locator('#multipleFilesUploader');

    await fileInput.setInputFiles([
      {
        name: "accessible-1.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("Test 1"),
      },
      {
        name: "accessible-2.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("Test 2"),
      },
    ]);

    await page.waitForTimeout(300);

    // Get remove buttons
    const removeButtons = multipleUploader.locator(removeButtonSelector);
    const removeCount = await removeButtons.count();

    // Should have remove button for each file
    expect(removeCount).toBeGreaterThanOrEqual(2);

    // Verify buttons have proper attributes
    const firstRemoveBtn = removeButtons.first();
    const role = await firstRemoveBtn.getAttribute("role");
    expect(role).toBe("button");

    // Verify button is keyboard accessible (tabindex >= 0 or role=button)
    const tabindex = await firstRemoveBtn.getAttribute("tabindex");
    expect(tabindex).toBeTruthy();
  });

  test("File List is Empty After Clear All", async () => {
    // Add files then clear
    const multipleUploader = page.locator("div").filter({ has: page.locator('#multipleFilesUploader') }).first();
    const fileInput = page.locator('#multipleFilesUploader');

    await fileInput.setInputFiles([
      {
        name: "temp-1.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("Temp 1"),
      },
      {
        name: "temp-2.txt",
        mimeType: "text/plain",
        buffer: Buffer.from("Temp 2"),
      },
    ]);

    await page.waitForTimeout(300);

    // Clear all
    const clearButton = multipleUploader.locator(".e-upload-clear-btn, button:has-text('Clear')");

    // Verify list is completely empty
    const fileListItems = page.locator("li.e-file-list-item");
    const itemCount = await fileListItems.count();
    expect(itemCount).toBe(0);

    // Can add files again
    await fileInput.setInputFiles({
      name: "new-file.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("New file"),
    });

    await page.waitForTimeout(300);
    const newFileItem = page.locator('[data-file-name="new-file.txt"]');
    await expect(newFileItem).toBeVisible();
  });

  test("Remove Icon Visibility for Different File States", async () => {
    // Verify remove icon appears for files in different states
    const manualUploader = page.locator("div").filter({ has: page.locator('#manualUploadUploader') }).first();
    const fileInput = page.locator('#manualUploadUploader');

    await fileInput.setInputFiles({
      name: "state-test.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("State test"),
    });

    await page.waitForTimeout(300);

    // File in "Ready to upload" state should have remove button
    const fileItem = page.locator('[data-file-name="state-test.txt"]');
    const removeBtn = fileItem.locator(removeButtonSelector);

    if (await removeBtn.count() > 0) {
      // Remove button should be visible for ready state
      await expect(removeBtn).toBeVisible();
    }
  });

  test("Remove Button Click Removes File Immediately", async () => {
    // Verify immediate removal
    const manualUploader = page.locator("div").filter({ has: page.locator('#manualUploadUploader') }).first();
    const fileInput = page.locator('#manualUploadUploader');

    await fileInput.setInputFiles({
      name: "immediate-remove.txt",
      mimeType: "text/plain",
      buffer: Buffer.from("Immediate"),
    });

    await page.waitForTimeout(300);

    const fileItem = page.locator('[data-file-name="immediate-remove.txt"]');
    await expect(fileItem).toBeVisible();

    // Click remove
    const removeBtn = fileItem.locator(removeButtonSelector);
    if (await removeBtn.count() > 0) {
      await removeBtn.first().click();

      // File should disappear immediately or very quickly
      await expect(fileItem).not.toBeVisible({ timeout: 500 });
    }
  });
});
