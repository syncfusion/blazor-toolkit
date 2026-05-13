import { test, expect } from '@playwright/test';

test.describe('SfSpinner – Events', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/spinner/events');
    await page.waitForSelector('#events-ready');
  });

  test('Created event fires on first render', async ({ page }) => {
    await expect(page.locator('#log')).toContainText('created|');
  });

  test('OnBeforeClose cancellation prevents spinner hide', async ({ page }) => {
    await page.locator('#btn-show').click();

    await expect(page.locator('.e-spinner-pane')).toHaveCount(1);

    // ✅ Spinner overlay blocks UI → force is required
    await page.locator('#btn-cancel-close').click({ force: true });
    await page.locator('#btn-hide').click({ force: true });

    // ✅ Spinner DOM must remain (cancel worked)
    await expect(page.locator('.e-spinner-pane')).toHaveCount(1);
  });

});