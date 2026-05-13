import { test, expect } from '@playwright/test';

test.describe('Chart – Moving Average Trendline › Forecast & Intercept', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/moving-average-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Trendline renders with forecast and intercept', async ({ page }) => {
    await expect(
      page.locator('#maForecastIntercept svg')
    ).toBeVisible();
  });

});