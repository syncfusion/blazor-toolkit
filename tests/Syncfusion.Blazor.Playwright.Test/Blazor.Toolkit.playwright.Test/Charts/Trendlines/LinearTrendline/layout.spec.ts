import { test, expect } from '@playwright/test';

test.describe('Chart – Linear Trendline › Layout', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/linear-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('SVG stays within configured size', async ({ page }) => {
    const chart = page.locator('#trendWithDataAndTrendline');
    const svg = chart.locator('svg');

    const chartBox = await chart.boundingBox();
    const svgBox = await svg.boundingBox();

    expect(svgBox!.width).toBeLessThanOrEqual(chartBox!.width);
    expect(svgBox!.height).toBeLessThanOrEqual(chartBox!.height);
  });

});