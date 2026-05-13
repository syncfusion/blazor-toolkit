import { test, expect } from '@playwright/test';

test.describe('TimePicker - Integration & Combinations', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/timepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('form submission includes time value', async ({ page }) => {
    const input = page.locator('#wrapper-tp-form .e-input');
    await input.fill('14:30');
    await input.press('Tab');
    const submit = page.locator('#wrapper-tp-form button[type="submit"]');
    await submit.click();
    const result = page.locator('#form-result');
    await expect(result).toContainText('14:30');
  });
});
