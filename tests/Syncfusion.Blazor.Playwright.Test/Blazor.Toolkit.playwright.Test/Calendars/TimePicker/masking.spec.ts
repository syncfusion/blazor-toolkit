import { test, expect } from '@playwright/test';

test.describe('TimePicker - Input Masking', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/timepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('EnableMask=true shows format placeholder', async ({ page }) => {
    const input = page.locator('#wrapper-tp-mask input');
    await expect(input).toBeVisible();
    const placeholder = await input.getAttribute('placeholder');
    expect(placeholder).toBeTruthy();
  });

  test('clear button on masked input works', async ({ page }) => {
    const input = page.locator('#wrapper-tp-initial input');
    const clear = page.locator('#wrapper-tp-initial .e-close');
    const initialValue = await input.inputValue();
    expect(initialValue).toBeTruthy();
    await input.focus();
    await expect(clear).toBeVisible({ timeout: 5000 });
    await clear.click();
    await expect(input).toHaveValue('', { timeout: 5000 });
  });
});
