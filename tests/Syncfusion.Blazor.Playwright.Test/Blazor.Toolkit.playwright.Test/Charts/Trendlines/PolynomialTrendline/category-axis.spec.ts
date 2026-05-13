import { test, expect } from '@playwright/test';

test.describe('Chart – Polynomial Trendline › Category Axis', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/polynomial-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Polynomial trendline renders on category axis', async ({ page }) => {
    await expect(
      page.locator('#polyCategoryAxis svg')
    ).toBeVisible();
  });

});
