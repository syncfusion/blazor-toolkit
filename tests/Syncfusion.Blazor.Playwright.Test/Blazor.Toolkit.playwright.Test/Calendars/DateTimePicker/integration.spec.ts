import { test, expect } from '@playwright/test';

test.describe('DateTimePicker - Integration & Combinations', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datetimepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('form submission includes date and time value', async ({ page }) => {
    const input = page.locator('#wrapper-dtp-form .e-datetimepicker.e-input');
    await input.fill('05/20/2025 14:30');
    const submit = page.locator('#wrapper-dtp-form button[type="submit"]');
    await submit.click();
    const result = page.locator('#form-result');
    await expect(result).toContainText('05/20/2025 14:30');
  });
});
