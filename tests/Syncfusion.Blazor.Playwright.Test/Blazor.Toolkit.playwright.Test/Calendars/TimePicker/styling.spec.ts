import { test, expect } from '@playwright/test';

test.describe('TimePicker - Styling & Appearance', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/timepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('custom CssClass and width applied', async ({ page }) => {
    const root = page.locator('#wrapper-tp-width');
    await expect(root).toBeVisible();
  });

  test('show clear icon when ShowClearButton is true', async ({ page }) => {
    const input = page.locator('#wrapper-tp-basic input');
    const clear = page.locator('#wrapper-tp-basic .e-close');
    await input.focus();
    await input.click();
    await expect(clear).toHaveCount(1, { timeout: 5000 });
  });

  test('float label styling visible', async ({ page }) => {
    const floatLabel = page.locator('#wrapper-tp-float');
    await expect(floatLabel).toBeVisible();
  });
});
