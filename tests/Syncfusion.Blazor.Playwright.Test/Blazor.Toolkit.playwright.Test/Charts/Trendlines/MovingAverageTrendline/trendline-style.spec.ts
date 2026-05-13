import { test, expect } from '@playwright/test';

test.describe('Chart – Moving Average Trendline › Style', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/moving-average-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Custom colored moving average trendline renders', async ({ page }) => {
    const trendline = page
      .locator('#maCustomColor svg path')
      .first();

    const stroke = await trendline.getAttribute('stroke');
    expect(stroke).toBeTruthy();
  });

});