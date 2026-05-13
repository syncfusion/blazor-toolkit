import { test, expect } from '@playwright/test';

test.describe('SfCalendar - Min/Max Overlay', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForLoadState('networkidle');
    await page.locator('#calendar-test').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('set Min > Max shows e-overlay class', async ({ page }) => {
    const set = page.locator('#set-minmax');
    await set.click();
    const root = page.locator('#calendar-test, .e-calendar').first();
    await expect(root).toHaveClass(/e-overlay/);
  });
});
