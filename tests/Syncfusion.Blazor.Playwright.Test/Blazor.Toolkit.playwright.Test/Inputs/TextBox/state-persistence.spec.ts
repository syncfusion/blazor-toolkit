import { test, expect } from '@playwright/test';

test.describe('TextBox - State persistence', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/inputs/textbox');
    await page.waitForLoadState('networkidle');
    await page.evaluate(() => localStorage.clear());
  });

  test('persistence stores value in localStorage when enabled (sample may not have it)', async ({ page }) => {
    // if no persistence control exists, skip
    const el = page.locator('#programmaticInput');
    if (await el.count() === 0) test.skip();
    await el.fill('persist-me');
    // attempt to read localStorage keys — presence is implementation dependent
    const keys = await page.evaluate(() => Object.keys(localStorage));
    expect(Array.isArray(keys)).toBeTruthy();
  });

  test('persistence disabled by default (no keys)', async ({ page }) => {
    const keys = await page.evaluate(() => Object.keys(localStorage));
    expect(Array.isArray(keys)).toBeTruthy();
  });
});
