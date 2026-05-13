import { test, expect } from '@playwright/test';

test.describe('SfCalendar - Hijri Mode', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/calendar-test');
    await page.waitForLoadState('networkidle');
    await page.locator('#calendar-test').waitFor({ state: 'visible', timeout: 5000 });
  });

  test('hijri calendar instance is present', async ({ page }) => {
    const hijri = page.locator('#calendar-hijri');
    await expect(hijri).toBeVisible();
  });
});
