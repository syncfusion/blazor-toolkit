import { test, expect } from '@playwright/test';

test.describe('Chart – Logarithmic Trendline › Category Axis', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/logarithmic-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Logarithmic trendline renders on category axis', async ({ page }) => {
    const chart = page.locator('#logCategoryAxis svg');
    await expect(chart).toBeVisible();
  });

});