import { test, expect } from '@playwright/test';

test.describe('Chart – Exponential Trendline › Trendline Style', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/exponential');
    await page.waitForLoadState('networkidle');
  });

  test('Trendline stroke color is applied', async ({ page }) => {
    const trendline = page.locator('#chart-host svg path')
      .filter({ hasText: '' }).first();

    const stroke = await trendline.getAttribute('stroke');
    expect(stroke).toBeTruthy();
  });

});
