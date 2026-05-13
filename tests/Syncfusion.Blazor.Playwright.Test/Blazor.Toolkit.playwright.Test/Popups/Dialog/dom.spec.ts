import { test, expect } from '@playwright/test';

const DEFAULT_TIMEOUT = 5000;

test.describe('Dialog - DOM & Rendering', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/dialog/basic-rendering');
    await page.waitForLoadState('networkidle');
  });

  test('Render non-modal dialog (root and attributes)', async ({ page }) => {
    await page.click('#open-nonmodal');
    const dlg = page.locator('#dlg-non-modal');
    await expect(dlg).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    await expect(dlg).toHaveAttribute('role', 'dialog');

    // Content element exists
    const content = page.locator('#dlg-non-modal_dialog-content').first();
    await expect(content).toBeVisible({ timeout: DEFAULT_TIMEOUT });
  });

  test('Open modal dialog and overlay appears', async ({ page }) => {
    await page.click('#open-modal');
    const dlg = page.locator('#dlg-modal');
    await expect(dlg).toBeVisible({ timeout: DEFAULT_TIMEOUT });

    const overlay = page.locator('#dlg-modal_overlay');
    await expect(overlay).toBeVisible({ timeout: DEFAULT_TIMEOUT });

    await expect(dlg).toHaveAttribute('aria-modal', 'true');
  });
});
