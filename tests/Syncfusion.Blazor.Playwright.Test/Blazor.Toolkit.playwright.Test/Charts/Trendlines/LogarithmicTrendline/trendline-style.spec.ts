import { test, expect } from '@playwright/test';

test.describe('Chart – Logarithmic Trendline › Style', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/logarithmic-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Custom colored trendline renders', async ({ page }) => {
    const trendline = page
      .locator('#logCustomColor svg path')
      .first();

    const stroke = await trendline.getAttribute('stroke');
    expect(stroke).toBeTruthy();
  });

});