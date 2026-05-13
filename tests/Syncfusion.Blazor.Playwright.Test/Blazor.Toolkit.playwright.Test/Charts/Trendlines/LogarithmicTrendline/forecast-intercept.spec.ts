import { test, expect } from '@playwright/test';

test.describe('Chart – Logarithmic Trendline › Forecast & Intercept', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/logarithmic-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Trendline renders with forecast and intercept', async ({ page }) => {
    await expect(
      page.locator('#logForecastIntercept svg')
    ).toBeVisible();
  });

});