import { test, expect } from '@playwright/test';

test.describe('Chart – Polynomial Trendline › Empty Series', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/polynomial-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Empty series without trendline renders safely', async ({ page }) => {
    await expect(
      page.locator('#polyEmptyNoTrend svg')
    ).toBeVisible();
  });

  test('Empty series with polynomial trendline does not break rendering', async ({ page }) => {
    await expect(
      page.locator('#polyEmptyWithTrend svg')
    ).toBeVisible();
  });

});