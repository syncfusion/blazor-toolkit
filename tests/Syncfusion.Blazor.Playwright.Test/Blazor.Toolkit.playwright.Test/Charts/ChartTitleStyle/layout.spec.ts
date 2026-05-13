import { test, expect } from '@playwright/test';

test.describe('Chart – ChartTitleStyle › Layout Validation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-title-style-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG stays within specified bounds', async ({ page }) => {
    const chart = page.locator('#chartTitleStyle1');
    const svg = chart.locator('svg');

    const chartBox = await chart.boundingBox();
    const svgBox = await svg.boundingBox();

    expect(svgBox!.width).toBeLessThanOrEqual(chartBox!.width);
    expect(svgBox!.height).toBeLessThanOrEqual(chartBox!.height);
  });

});