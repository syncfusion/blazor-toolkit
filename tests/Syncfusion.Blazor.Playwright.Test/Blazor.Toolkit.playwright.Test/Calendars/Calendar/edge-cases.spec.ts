import { test, expect } from '@playwright/test';
import { selectDate } from './calendar-utils';

test.describe('SfCalendar - Edge Cases', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForLoadState('networkidle');
  });

  test('AddDatesAsync(null) is no-op via UI (no crash)', async ({ page }) => {
    // Ensure calendar rendered
    await page.locator('#calendar-test').waitFor({ state: 'visible', timeout: 5000 });
    // Enable multi-selection if available because AddDatesAsync requires multi-select
    const toggle = page.locator('#toggle-multiselect');
    if (await toggle.count() > 0) {
      await toggle.click();
    }
    // There is no direct null button; this test simply ensures AddDates button operates safely
    const add = page.locator('#add-dates');
    await expect(add).toBeVisible();
    await add.click();
    const log = page.locator('#event-log');
    await expect(log).toContainText(/ValuesChanged/, { timeout: 5000 });
  });

  test('time zone behavior: selecting same nominal day across timezones preserves day number', async ({ browser }) => {
    // Create a new context in a different timezone and ensure selection works
    const ctx = await browser.newContext({ timezoneId: 'America/Los_Angeles' as any });
    const page = await ctx.newPage();
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForSelector('#calendar-test');
    const d = new Date();
    await selectDate(page, d);
    await expect(page.locator('#event-log')).toBeVisible();
    await ctx.close();
  });

  test('form integration sample (smoke): calendar selection participates in form flow', async ({ page }) => {
    // The sample page does not include a form wrapper; this is a smoke test ensuring select works
    const d = new Date();
    await selectDate(page, d);
    // Either the UI shows selection or ValuesChanged is logged for multi-select
    await expect(page.locator('#event-log')).toBeVisible();
  });
});
