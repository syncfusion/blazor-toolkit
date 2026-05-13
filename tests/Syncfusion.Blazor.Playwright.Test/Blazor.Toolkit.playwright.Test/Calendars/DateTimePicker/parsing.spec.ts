import { test, expect } from '@playwright/test';

test.describe('DateTimePicker - Input Parsing & Formats', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datetimepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('accept typed input matching Format', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    await input.fill('12/31/2024 14:30');
    await input.press('Tab');
    await expect(input).toHaveValue('12/31/2024 14:30');
  });

  test('strict mode rejects invalid input', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-strict input');
    await input.fill('31-12-2024 14:30');
    await input.press('Tab');
    // Strict mode should not accept wrong format; expect value not equal to typed invalid
    await expect(input).not.toHaveValue('31-12-2024 14:30');
  });

  test('clear button resets value', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-initial input');
    const clear = page.locator('#wrapper-dtp-initial .e-close');
    
    await expect(input).toHaveValue('05/15/2025 14:30');
    // Focus the input to reveal the clear icon
    await input.focus();
    await expect(clear).toBeVisible({ timeout: 5000 });
    await clear.click();
    await expect(input).toHaveValue('', { timeout: 5000 });
  });

  test('accept valid date and time format with different times', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-basic input');
    
    const testCases = [
      '01/01/2025 08:00',
      '06/15/2025 12:30',
      '12/25/2025 18:45'
    ];
    
    for (const testCase of testCases) {
      await input.fill(testCase);
      await input.press('Tab');
      await expect(input).toHaveValue(testCase);
    }
  });

  test('12-hour time format with AM/PM', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-timeformat input');
    await input.fill('01/15/2025 02:30 PM');
    await input.press('Tab');
    const value = await input.inputValue();
    expect(value).toMatch(/\d{2}\/\d{2}\/\d{4} \d{2}:\d{2}/);
  });
});
