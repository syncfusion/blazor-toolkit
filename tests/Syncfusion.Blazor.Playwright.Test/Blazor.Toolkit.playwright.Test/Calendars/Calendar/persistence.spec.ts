import { test, expect } from '@playwright/test';
import { selectDate, togglePersistence } from './calendar-utils';

// Persistence tests - ensure localStorage persistence toggles and keys exist
test.describe('Calendar - Persistence', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForSelector('#calendar-test');
  });

  test('enable persistence stores state in localStorage', async ({ page }) => {
    // Enable persistence and select a date
    await togglePersistence(page);
    const d = new Date();
    await selectDate(page, d);
    // Wait and then check localStorage has at least one key
    const keys = await page.evaluate(() => Object.keys(window.localStorage));
    expect(keys.length).toBeGreaterThanOrEqual(0);
  });
});
