import { test, expect } from '@playwright/test';

test.describe('Chart – Trendline LineStyle & Legend › Layout Validation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/trendline-line-style-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Chart container is visible', async ({ page }) => {
    const chartHost = page.locator('#trendLineStyleChart');
    await expect(chartHost).toBeVisible();
  });

  test('SVG is rendered inside chart container', async ({ page }) => {
    const svg = page.locator('#trendLineStyleChart svg');
    await expect(svg).toBeVisible();
  });

  test('SVG stays within chart container bounds', async ({ page }) => {
    const chartHost = page.locator('#trendLineStyleChart');
    const svg = chartHost.locator('svg');

    const hostBox = await chartHost.boundingBox();
    const svgBox = await svg.boundingBox();

    expect(hostBox).not.toBeNull();
    expect(svgBox).not.toBeNull();

    expect(svgBox!.width).toBeLessThanOrEqual(hostBox!.width);
    expect(svgBox!.height).toBeLessThanOrEqual(hostBox!.height);
  });

});