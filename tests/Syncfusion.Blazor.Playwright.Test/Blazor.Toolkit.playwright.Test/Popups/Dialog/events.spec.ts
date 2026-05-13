import { test, expect } from '@playwright/test';

const DEFAULT_TIMEOUT = 5000;

test.describe('Dialog - Events', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/dialog/basic-rendering');
    await page.waitForLoadState('networkidle');
  });

  test('Opened and Closed events fire for modal dialog', async ({ page }) => {
    await page.click('#open-modal');
    await expect(page.locator('#dlg-modal')).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    await expect(page.locator('#dlg-modal-opened')).toHaveText('opened', { timeout: DEFAULT_TIMEOUT });

    // Click OK to close
    const ok = page.locator('button:has-text("OK")');
    await ok.click();
    await expect(page.locator('#dlg-modal-closed')).toHaveText('closed', { timeout: DEFAULT_TIMEOUT });
  });

  test('BeforeOpen cancel prevents showing dialog', async ({ page }) => {
    // Attempt to open and have the sample cancel the open
    await page.click('#open-cancel');
    // Since the page sets beforeOpenCancelled flag, expect cancelled marker
    await expect(page.locator('#dlg-beforeopen-cancelled')).toHaveText('cancelled', { timeout: DEFAULT_TIMEOUT });
    // Dialog should not be visible. Check the overlay (user-observable) instead of internal dialog element.
    const overlay = page.locator('#dlg-modal_overlay');
    await expect(overlay).not.toBeVisible({ timeout: DEFAULT_TIMEOUT });
  });

  test('Overlay click triggers overlay handler', async ({ page }) => {
    await page.click('#open-modal');
    await expect(page.locator('#dlg-modal')).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    const overlay = page.locator('#dlg-modal_overlay');
    await expect(overlay).toBeVisible({ timeout: DEFAULT_TIMEOUT });
    // The dialog content may overlap the overlay's center and block pointer events.
    // Use a programmatic click to trigger the overlay handler without actionability checks.
    await page.evaluate(() => document.getElementById('dlg-modal_overlay')?.click());
    await expect(page.locator('#dlg-overlay-clicked')).toHaveText('overlay', { timeout: DEFAULT_TIMEOUT });
  });
});
