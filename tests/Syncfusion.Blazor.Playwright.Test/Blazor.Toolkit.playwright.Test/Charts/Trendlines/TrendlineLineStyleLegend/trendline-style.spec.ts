import { test, expect } from '@playwright/test';

test.describe('Chart – Trendline LineStyle & Legend › Trendline Style', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/trendline-line-style-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Trendline stroke-width is applied', async ({ page }) => {
    const trendline = page
      .locator('#trendLineStyleChart svg path')
      .first();

    const strokeWidth = await trendline.getAttribute('stroke-width');
    expect(strokeWidth).toBeTruthy();
  });

  test('Trendline stroke color is applied', async ({ page }) => {
    const trendline = page
      .locator('#trendLineStyleChart svg path')
      .first();

    const stroke = await trendline.getAttribute('stroke');
    expect(stroke).toBeTruthy();
  });

});