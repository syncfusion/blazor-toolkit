import { test, expect } from '@playwright/test';

test.describe('Chart – Power Trendline › Trendline Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/power-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Power trendline renders with data', async ({ page }) => {
    await expect(
      page.locator('#powerDataWithTrend svg')
    ).toBeVisible();
  });

  test('Default appearance power trendline renders', async ({ page }) => {
    await expect(
      page.locator('#powerDefaultAppearance svg')
    ).toBeVisible();
  });

});