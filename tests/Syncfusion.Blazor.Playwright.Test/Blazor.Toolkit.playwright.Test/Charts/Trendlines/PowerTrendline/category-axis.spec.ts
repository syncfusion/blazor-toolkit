import { test, expect } from '@playwright/test';

test.describe('Chart – Power Trendline › Category Axis', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/power-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Power trendline renders on category axis', async ({ page }) => {
    await expect(
      page.locator('#powerCategoryAxis svg')
    ).toBeVisible();
  });

});