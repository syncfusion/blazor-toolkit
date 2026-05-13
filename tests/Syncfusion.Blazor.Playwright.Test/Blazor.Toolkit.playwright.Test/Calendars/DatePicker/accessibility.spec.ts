import { test, expect } from '@playwright/test';

test.describe('DatePicker - Accessibility', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('ARIA attributes present on input and popup', async ({ page }) => {
    const input = page.locator('#wrapper-dp-basic input');
    await expect(input).toHaveAttribute('role');
    const icon = page.locator('#wrapper-dp-basic .e-timeline-today');
    await icon.click();
    const popup = page.locator('.e-popup');
    await expect(popup).toHaveAttribute('role');
  });
});
