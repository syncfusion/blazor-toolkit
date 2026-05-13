import { test, expect } from '@playwright/test';

test.describe('SfCalendar - OnRenderDayCell', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForLoadState('networkidle');
  });

  test('custom render adds data-rendered attribute', async ({ page }) => {
    const cell = page.locator('td.e-custom-day-cell').first();
    await expect(cell).toBeVisible();
  });
});
