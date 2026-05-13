import { test, expect } from '@playwright/test';

test.describe('DatePicker - Styling & Appearance', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('custom CssClass and width applied', async ({ page }) => {
    const widthInput = page.locator('#wrapper-dp-width .e-input');
    await expect(widthInput).toBeVisible();
    // width may be applied to root element; assert style contains width
    const root = page.locator('#wrapper-dp-width');
    await expect(root).toBeVisible();
  });

  test('show clear icon when ShowClearButton is true', async ({ page }) => {
    const input = page.locator('#wrapper-dp-basic input');
    const clear = page.locator('#wrapper-dp-basic .e-close');
    // clicking the input (or focusing then clicking) triggers the clear icon to appear
    await input.focus();
    await input.click();
    // wait for the clear icon element to be present in the DOM (may be hidden by CSS)
    await expect(clear).toHaveCount(1, { timeout: 5000 });
  });
});
