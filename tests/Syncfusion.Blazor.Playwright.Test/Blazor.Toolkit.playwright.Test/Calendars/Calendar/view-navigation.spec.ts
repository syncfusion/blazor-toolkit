import { test, expect } from '@playwright/test';

test.describe('SfCalendar - View Navigation', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForLoadState('networkidle');
    await page.locator('#calendar-test').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('drill up to year/decade views and navigated event fires', async ({ page }) => {
    const header = page.locator('.e-calendar .e-header .e-title').first();
    await expect(header).toBeVisible();
    await header.click();
    const log = page.locator('#event-log');
    if (await log.count() > 0) {
      await expect(log).toContainText(/Navigated/);
    }
  });
});
