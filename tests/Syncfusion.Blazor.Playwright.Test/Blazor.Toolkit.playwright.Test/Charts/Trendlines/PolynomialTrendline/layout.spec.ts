import { test, expect } from '@playwright/test';

test.describe('Chart – Polynomial Trendline › Layout', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/polynomial-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('SVG stays within chart container bounds', async ({ page }) => {
    const chart = page.locator('#polyDataWithTrend');
    const svg = chart.locator('svg');

    const chartBox = await chart.boundingBox();
    const svgBox = await svg.boundingBox();

    expect(svgBox!.width).toBeLessThanOrEqual(chartBox!.width);
    expect(svgBox!.height).toBeLessThanOrEqual(chartBox!.height);
  });

});
