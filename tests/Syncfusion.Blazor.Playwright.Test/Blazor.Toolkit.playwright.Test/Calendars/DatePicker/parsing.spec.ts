import { test, expect } from '@playwright/test';

test.describe('DatePicker - Input Parsing & Formats', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('accept typed input matching Format', async ({ page }) => {
    const input = page.locator('#wrapper-dp-basic input');
    await input.fill('12/31/2024');
    await input.press('Tab');
    await expect(input).toHaveValue('12/31/2024');
  });

  test('strict mode rejects invalid input', async ({ page }) => {
    const input = page.locator('#wrapper-dp-strict input');
    await input.fill('31-12-2024');
    await input.press('Tab');
    // Strict mode should not accept wrong format; expect value not equal to typed invalid
    await expect(input).not.toHaveValue('31-12-2024');
  });

  test('clear button resets value', async ({ page }) => {
    const input = page.locator('#wrapper-dp-initial input');
    const clear = page.locator('#wrapper-dp-initial .e-close');
    await expect(input).toHaveValue('05/15/2025');
    // Focus the input to reveal the clear icon (it may be hidden until focus/hover)
    await input.focus();
    await expect(clear).toHaveCount(1);
    await clear.click();
    await expect(input).toHaveValue('', { timeout: 5000 });
  });
});
