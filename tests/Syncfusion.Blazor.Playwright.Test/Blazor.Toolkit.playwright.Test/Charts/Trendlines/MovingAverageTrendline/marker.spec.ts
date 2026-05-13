import { test, expect } from '@playwright/test';

test.describe('Chart – Moving Average Trendline › Marker', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/moving-average-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Trendline markers render when enabled', async ({ page }) => {
    await expect(
      page.locator('#maWithMarker svg')
    ).toBeVisible();
  });

});