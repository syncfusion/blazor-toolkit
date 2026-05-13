import { test, expect } from '@playwright/test';

test.describe('Chart – Moving Average Trendline › Empty Series', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/moving-average-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Empty series without trendline renders safely', async ({ page }) => {
    await expect(
      page.locator('#maEmptyNoTrend svg')
    ).toBeVisible();
  });

  test('Empty series with moving average trendline does not break rendering', async ({ page }) => {
    await expect(
      page.locator('#maEmptyWithTrend svg')
    ).toBeVisible();
  });

});
