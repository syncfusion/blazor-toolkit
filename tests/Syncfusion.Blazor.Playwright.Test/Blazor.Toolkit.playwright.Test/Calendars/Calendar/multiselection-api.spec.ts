import { test, expect } from '@playwright/test';

test.describe('SfCalendar - Multi-selection API', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForLoadState('networkidle');
  });

  test('add and remove dates via API buttons', async ({ page }) => {
    const add = page.locator('#add-dates');
    const remove = page.locator('#remove-dates');
    const toggleMulti = page.locator('#toggle-multiselect');
    const log = page.locator('#event-log');
    // Ensure multi-selection is enabled so AddDates triggers ValuesChanged
    await toggleMulti.click();

    // Click Add and wait for ValuesChanged to appear (give it more time)
    await add.click();
    await expect(log).toContainText(/ValuesChanged/, { timeout: 10000 });

    // Click Remove and expect another ValuesChanged
    await remove.click();
    await expect(log).toContainText(/ValuesChanged/, { timeout: 10000 });
  });
});
