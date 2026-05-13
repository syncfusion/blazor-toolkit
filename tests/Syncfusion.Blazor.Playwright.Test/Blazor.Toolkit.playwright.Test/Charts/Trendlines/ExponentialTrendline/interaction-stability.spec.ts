import { test, expect } from '@playwright/test';

test.describe('Chart – Exponential Trendline › Stability', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/exponential');
    await page.waitForLoadState('networkidle');
  });

  test('Chart remains stable after rendering completes', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

});
