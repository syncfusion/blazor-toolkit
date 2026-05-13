import { test, expect } from '@playwright/test';

test.describe('Chart – Logarithmic Trendline › Empty Series', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/logarithmic-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Empty series without trendline renders safely', async ({ page }) => {
    await expect(
      page.locator('#logEmptyNoTrend svg')
    ).toBeVisible();
  });

  test('Empty series with logarithmic trendline does not break rendering', async ({ page }) => {
    await expect(
      page.locator('#logEmptyWithTrend svg')
    ).toBeVisible();
  });

});