import { test, expect } from '@playwright/test';

test.describe('Chart – Polynomial Trendline › Style', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/polynomial-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Custom colored polynomial trendline renders', async ({ page }) => {
    const trendline = page
      .locator('#polyCustomColor svg path')
      .first();

    const stroke = await trendline.getAttribute('stroke');
    expect(stroke).toBeTruthy();
  });

});