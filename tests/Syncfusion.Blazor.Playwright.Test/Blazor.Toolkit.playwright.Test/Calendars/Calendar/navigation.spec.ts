import { test, expect } from '@playwright/test';
import { navigateYearView, eventLog } from './calendar-utils';

// Tests for calendar navigation behaviors (month/year views)
test.describe('Calendar - Navigation', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForSelector('#calendar-test');
  });

  test('navigate to Year view via sample button triggers Navigated event', async ({ page }) => {
    await navigateYearView(page);
    // Wait for the navigated log entry
    await page.waitForFunction(() => document.querySelector('#event-log')?.innerHTML.includes('Navigated'));
    await expect(eventLog(page)).toContainText('Navigated');
  });
});
