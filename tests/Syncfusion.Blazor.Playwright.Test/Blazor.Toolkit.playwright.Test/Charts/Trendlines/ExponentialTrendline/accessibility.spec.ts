import { test, expect } from '@playwright/test';

test.describe('Chart – Exponential Trendline › Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/exponential');
    await page.waitForLoadState('networkidle');
  });

  test('Chart heading is readable', async ({ page }) => {
    await expect(
      page.locator('h3')
    ).toHaveText('Chart – Exponential Trendline');
  });

});