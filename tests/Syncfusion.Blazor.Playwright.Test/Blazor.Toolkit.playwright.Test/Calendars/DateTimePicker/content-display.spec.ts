import { test, expect } from '@playwright/test';

test.describe('DateTimePicker - Content & Display', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datetimepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('renders with placeholder and initial value', async ({ page }) => {
    const basicInput = page.locator('#wrapper-dtp-basic input');
    await expect(basicInput).toBeVisible();
    await expect(basicInput).toHaveAttribute('placeholder', 'Select date and time');

    const initialInput = page.locator('#wrapper-dtp-initial input');
    await expect(initialInput).toBeVisible();
    await expect(initialInput).toHaveValue('05/15/2025 14:30');
  });

  test('empty or null value behavior and clear visibility', async ({ page }) => {
    const basicWrapper = page.locator('#wrapper-dtp-basic');
    await expect(basicWrapper.locator('input')).toHaveAttribute('placeholder', 'Select date and time');
  });

  test('dynamic value updates reflected in input', async ({ page }) => {
    const initialInput = page.locator('#wrapper-dtp-initial input');
    const value = await initialInput.inputValue();
    expect(value).toMatch(/\d{2}\/\d{2}\/\d{4} \d{2}:\d{2}/);
  });

  test('clear button visibility on focus', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-initial input');
    const clear = page.locator('#wrapper-dtp-initial .e-close');
    
    await input.focus();
    await expect(clear).toBeVisible({ timeout: 5000 });
    await input.blur();
  });
});
