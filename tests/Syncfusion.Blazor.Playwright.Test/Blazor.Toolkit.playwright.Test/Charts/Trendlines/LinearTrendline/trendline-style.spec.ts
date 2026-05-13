import { test, expect } from '@playwright/test';

test.describe('Chart – Linear Trendline › Style', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/linear-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Custom colored trendline renders', async ({ page }) => {
    const trendline = page
      .locator('#trendCustomColor svg path')
      .filter({ hasText: '' })
      .first();

    const stroke = await trendline.getAttribute('stroke');
    expect(stroke).toBeTruthy();
  });

});
