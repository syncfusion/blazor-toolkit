import { test, expect } from '@playwright/test';

test.describe('Chart – Linear Trendline › Marker', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/linear-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Trendline marker renders when enabled', async ({ page }) => {
    const chart = page.locator('#trendWithMarker svg');
    await expect(chart).toBeVisible();
  });

});