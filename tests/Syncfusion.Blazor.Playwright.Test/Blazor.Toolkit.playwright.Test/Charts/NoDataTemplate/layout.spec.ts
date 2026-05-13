import { test, expect } from '@playwright/test';

test.describe('Chart – No Data Template › Layout Validation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/no-data-template');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG stays within container bounds', async ({ page }) => {
    const hostBox = await page.locator('#chart-host').boundingBox();
    const svgBox = await page.locator('#chart-host svg').boundingBox();

    expect(svgBox!.width).toBeLessThanOrEqual(hostBox!.width);
    expect(svgBox!.height).toBeLessThanOrEqual(hostBox!.height);
  });

});
