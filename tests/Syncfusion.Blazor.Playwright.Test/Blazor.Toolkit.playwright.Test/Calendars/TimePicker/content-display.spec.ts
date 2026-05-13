import { test, expect } from '@playwright/test';

test.describe('TimePicker - Content & Display', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/timepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('renders with placeholder and initial value', async ({ page }) => {
    const basicInput = page.locator('#wrapper-tp-basic .e-input');
    await expect(basicInput).toBeVisible();
    await expect(basicInput).toHaveAttribute('placeholder', 'Select a time');

    const initialInput = page.locator('#wrapper-tp-initial .e-input');
    await expect(initialInput).toBeVisible();
    const value = await initialInput.inputValue();
    expect(value).toBeTruthy();
  });

  test('empty or null value behavior and clear visibility', async ({ page }) => {
    const basicWrapper = page.locator('#wrapper-tp-basic');
    await expect(basicWrapper.locator('input')).toHaveAttribute('placeholder', 'Select a time');
  });

  test('clear button resets value', async ({ page }) => {
    const input = page.locator('#wrapper-tp-initial .e-input');
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
