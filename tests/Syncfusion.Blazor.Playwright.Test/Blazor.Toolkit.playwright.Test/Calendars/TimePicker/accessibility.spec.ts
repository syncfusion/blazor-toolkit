import { test, expect } from '@playwright/test';

test.describe('TimePicker - Accessibility & RTL', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/timepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('ARIA attributes present on input and popup', async ({ page }) => {
    const input = page.locator('#wrapper-tp-basic input');
    await expect(input).toHaveAttribute('role');
    const icon = page.locator('#wrapper-tp-basic .e-clock');
    await icon.click();
    const popup = page.locator('.e-popup');
    await expect(popup).toHaveAttribute('role');
  });
});
