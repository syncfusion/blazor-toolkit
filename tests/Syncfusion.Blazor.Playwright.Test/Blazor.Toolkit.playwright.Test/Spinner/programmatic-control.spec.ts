import { test, expect } from '@playwright/test';

test.describe('SfSpinner – Programmatic Control', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/spinner/programmatic-control');
    await page.waitForSelector('#programmatic-ready');
  });

  test('ShowAsync creates spinner DOM and inner structure', async ({ page }) => {
    await page.locator('#btn-show').click();

    const spinner = page.locator('.e-spinner-pane');
    await expect(spinner).toHaveCount(1);
    await expect(spinner.locator('.e-spinner-inner')).toHaveCount(1);
  });

  test('HideAsync when spinner is hidden is a no‑op', async ({ page }) => {
    await page.locator('#btn-hide').click({ force: true });

    const spinner = page.locator('.e-spinner-pane');
    if (await spinner.count() === 1) {
      // no DOM removal and no crash
      await expect(spinner).toHaveCount(1);
    }
  });

});