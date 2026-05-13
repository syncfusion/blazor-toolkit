import { test, expect } from '@playwright/test';

test.describe('DatePicker - Integration & Combinations', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('form submission includes date value', async ({ page }) => {
    const input = page.locator('#wrapper-dp-form .e-datepicker.e-input');
    await input.fill('01/02/2024');
    const submit = page.locator('#wrapper-dp-form button[type="submit"]');
    await submit.click();
    const result = page.locator('#form-result');
    await expect(result).toContainText('01/02/2024');
  });
});
