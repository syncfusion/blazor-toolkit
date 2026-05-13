import { test, expect } from '@playwright/test';

test.describe('SfSpinner – Basic Rendering', () => {

  test('spinner is hidden by default', async ({ page }) => {
    await page.goto('http://localhost:5000/spinner/basic');
    await page.waitForSelector('#basic-ready');

    const spinner = page.locator('.e-spinner-pane');
    await expect(spinner).toHaveCount(1);
    await expect(spinner).toHaveClass(/e-spin-hide/);
  });

  test('ShowAsync creates spinner DOM', async ({ page }) => {
    await page.goto('http://localhost:5000/spinner/basic');
    await page.waitForSelector('#basic-ready');

    await page.locator('#btn-show').click();
    await expect(page.locator('.e-spinner-pane')).toHaveCount(1);
  });

  test('spinner DOM persists after HideAsync', async ({ page }) => {
    await page.goto('http://localhost:5000/spinner/basic');
    await page.waitForSelector('#basic-ready');

    await page.locator('#btn-show').click();
    await page.locator('#btn-hide').click({ force: true });

    await expect(page.locator('.e-spinner-pane')).toHaveCount(1);
  });

});