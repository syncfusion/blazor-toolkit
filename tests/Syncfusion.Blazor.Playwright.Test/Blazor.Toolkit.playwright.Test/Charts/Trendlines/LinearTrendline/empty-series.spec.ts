import { test, expect } from '@playwright/test';

test.describe('Chart – Linear Trendline › Empty Series', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/linear-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Empty series without trendline renders safely', async ({ page }) => {
    const chart = page.locator('#trendEmptyNoTrendline svg');
    await expect(chart).toBeVisible();
  });

  test('Empty series with trendline does not crash rendering', async ({ page }) => {
    const chart = page.locator('#trendEmptyWithTrendline svg');
    await expect(chart).toBeVisible();
  });

});