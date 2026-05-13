import { test, expect } from '@playwright/test';

test.describe('TimePicker - Input Parsing & Formats', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/timepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('accept typed input matching Format', async ({ page }) => {
    const input = page.locator('#wrapper-tp-basic input');
    await input.fill('14:30');
    await input.press('Tab');
    await expect(input).toHaveValue('14:30');
  });

  test('strict mode rejects invalid input', async ({ page }) => {
    const input = page.locator('#wrapper-tp-strict input');
    await input.fill('25:70');
    await input.press('Tab');
    // Strict mode should not accept wrong format
    await expect(input).toHaveValue('14:30');
  });

  test('clear button resets value', async ({ page }) => {
    const input = page.locator('#wrapper-tp-initial input');
    const clear = page.locator('#wrapper-tp-initial .e-close');
    const initialValue = await input.inputValue();
    expect(initialValue).toBeTruthy();
    // Focus the input to reveal the clear icon
    await input.focus();
    await expect(clear).toBeVisible({ timeout: 5000 });
    await clear.click();
    await expect(input).toHaveValue('', { timeout: 5000 });
  });
});
