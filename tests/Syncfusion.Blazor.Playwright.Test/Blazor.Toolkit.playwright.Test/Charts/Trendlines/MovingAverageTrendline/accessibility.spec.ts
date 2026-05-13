import { test, expect } from '@playwright/test';

test.describe('Chart – Moving Average Trendline › Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/moving-average-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading is accessible', async ({ page }) => {
    await expect(
      page.locator('h2')
    ).toHaveText('Moving Average Trendline – Playwright Sample');
  });

});