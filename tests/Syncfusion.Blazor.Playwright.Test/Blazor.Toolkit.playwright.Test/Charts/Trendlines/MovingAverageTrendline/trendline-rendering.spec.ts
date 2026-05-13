import { test, expect } from '@playwright/test';

test.describe('Chart – Moving Average Trendline › Trendline Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/moving-average-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Moving average trendline renders with data', async ({ page }) => {
    const chart = page.locator('#maDataWithTrend svg');
    await expect(chart).toBeVisible();
  });

  test('Default appearance trendline renders', async ({ page }) => {
    await expect(
      page.locator('#maDefaultAppearance svg')
    ).toBeVisible();
  });

});
