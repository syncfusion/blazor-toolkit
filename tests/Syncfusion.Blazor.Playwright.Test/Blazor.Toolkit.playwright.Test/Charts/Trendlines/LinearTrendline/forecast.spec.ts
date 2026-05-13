import { test, expect } from '@playwright/test';

test.describe('Chart – Linear Trendline › Forecast & Intercept', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/linear-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Forward forecast trendline renders', async ({ page }) => {
    await expect(
      page.locator('#trendForwardForecast svg')
    ).toBeVisible();
  });

  test('Backward forecast trendline renders', async ({ page }) => {
    await expect(
      page.locator('#trendBackwardForecast svg')
    ).toBeVisible();
  });

  test('Intercept trendline renders', async ({ page }) => {
    await expect(
      page.locator('#trendIntercept svg')
    ).toBeVisible();
  });

});