import { test, expect } from '@playwright/test';

test.describe('Chart – Power Trendline › Forecast & Intercept', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/power-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Power trendline renders with forecast and intercept', async ({ page }) => {
    await expect(
      page.locator('#powerForecastIntercept svg')
    ).toBeVisible();
  });

});