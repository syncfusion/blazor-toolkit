import { test, expect } from '@playwright/test';

test.describe('Chart – Power Trendline › Style', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/power-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Custom colored power trendline renders', async ({ page }) => {
    const trendline = page
      .locator('#powerCustomColor svg path')
      .first();

    const stroke = await trendline.getAttribute('stroke');
    expect(stroke).toBeTruthy();
  });

});