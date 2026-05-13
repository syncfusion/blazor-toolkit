import { test, expect } from '@playwright/test';

const DEFAULT_TIMEOUT = 5000;

test.describe('Dialog - Content & Templates', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/dialog/basic-rendering');
    await page.waitForLoadState('networkidle');
  });

  test('Header and content render for non-modal dialog', async ({ page }) => {
    await page.click('#open-nonmodal');
    const title = page.locator('#dlg-non-modal_title');
    await expect(title).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    await expect(title).toContainText('Non-Modal Dialog');
    // Select the visible content node (there may be duplicated IDs in the DOM). Filter by visible text.
    const content = page.locator('#dlg-non-modal_dialog-div-content');
    await expect(content).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    await expect(content).toContainText('This is non-modal dialog content.');
  });

  test('Footer buttons exist for modal dialog', async ({ page }) => {
    await page.click('#open-modal');
    await expect(page.locator('#dlg-modal')).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    const okBtn = page.locator('.e-footer-content button').first();
    const cancelBtn = page.locator('.e-footer-content button').last();
    await expect(okBtn).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    await expect(cancelBtn).toBeVisible({ timeout: DEFAULT_TIMEOUT });
  });
});
