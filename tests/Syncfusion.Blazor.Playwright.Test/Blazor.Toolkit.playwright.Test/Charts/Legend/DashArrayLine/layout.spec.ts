import { test, expect } from '@playwright/test';

test.describe('Chart – DashArray (Line Variants) › Layout Validation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/dasharray/line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart container is visible', async ({ page }) => {
    const host = page.locator('#chart-host');
    await expect(host).toBeVisible();
  });

  test('SVG is rendered inside the chart container', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('SVG stays within container bounds', async ({ page }) => {
    const containerBox = await page.locator('#chart-host').boundingBox();
    const svgBox = await page.locator('#chart-host svg').boundingBox();

    expect(containerBox).not.toBeNull();
    expect(svgBox).not.toBeNull();

    expect(svgBox!.width).toBeLessThanOrEqual(containerBox!.width);
    expect(svgBox!.height).toBeLessThanOrEqual(containerBox!.height);
  });

});