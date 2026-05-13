import { test, expect } from '@playwright/test';

test.describe('Chart – Polynomial Trendline › Trendline Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/polynomial-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Polynomial trendline renders with data', async ({ page }) => {
    await expect(
      page.locator('#polyDataWithTrend svg')
    ).toBeVisible();
  });

  test('Default appearance trendline renders', async ({ page }) => {
    await expect(
      page.locator('#polyDefaultAppearance svg')
    ).toBeVisible();
  });

});