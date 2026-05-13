import { test, expect } from '@playwright/test';

test.describe('Chart – Logarithmic Trendline › Marker', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/logarithmic-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Trendline markers render when enabled', async ({ page }) => {
    await expect(
      page.locator('#logWithMarker svg')
    ).toBeVisible();
  });

});