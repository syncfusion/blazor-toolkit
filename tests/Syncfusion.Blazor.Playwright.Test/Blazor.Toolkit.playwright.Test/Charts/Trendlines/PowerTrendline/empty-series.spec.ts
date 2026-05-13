import { test, expect } from '@playwright/test';

test.describe('Chart – Power Trendline › Empty Series', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/power-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Empty series without trendline renders safely', async ({ page }) => {
    await expect(
      page.locator('#powerEmptyNoTrend svg')
    ).toBeVisible();
  });

  test('Empty series with power trendline does not break rendering', async ({ page }) => {
    await expect(
      page.locator('#powerEmptyWithTrend svg')
    ).toBeVisible();
  });

});
