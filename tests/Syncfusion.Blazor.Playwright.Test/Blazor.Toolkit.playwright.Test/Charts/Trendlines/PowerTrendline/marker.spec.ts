import { test, expect } from '@playwright/test';

test.describe('Chart – Power Trendline › Marker', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/power-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Trendline markers render when enabled', async ({ page }) => {
    await expect(
      page.locator('#powerWithMarker svg')
    ).toBeVisible();
  });

});