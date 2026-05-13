import { test, expect } from '@playwright/test';

test.describe('DatePicker - States & Interactions', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/datepicker-test');
    await page.waitForLoadState('networkidle');
  });

  test('focus and blur behavior', async ({ page }) => {
    const input = page.locator('#wrapper-dp-basic input');
    await input.focus();
    await expect(input).toBeFocused();
    await input.press('Tab');
    await expect(input).not.toBeFocused();
  });

  test('disabled state does not open popup', async ({ page }) => {
    const icon = page.locator('#wrapper-dp-disabled .e-timeline-today');
    await icon.click();
    const popup = page.locator('.e-popup');
    await expect(popup).not.toBeVisible();
  });
});
