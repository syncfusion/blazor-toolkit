import { test, expect } from '@playwright/test';

test.describe('Chart – Exponential Trendline › Layout', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/exponential');
    await page.waitForLoadState('networkidle');
  });

  test('SVG stays within container bounds', async ({ page }) => {
    const hostBox = await page.locator('#chart-host').boundingBox();
    const svgBox = await page.locator('#chart-host svg').boundingBox();

    expect(svgBox!.width).toBeLessThanOrEqual(hostBox!.width);
    expect(svgBox!.height).toBeLessThanOrEqual(hostBox!.height);
  });

});