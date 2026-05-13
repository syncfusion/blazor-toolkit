import { test, expect } from '@playwright/test';

test.describe('Chart Marker API – Layout', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/marker/api');
    await page.waitForLoadState('networkidle');
  });

  test('Chart elements stay within container bounds', async ({ page }) => {
    const hostBox = await page.locator('#chart-host').boundingBox();
    const svgBox = await page.locator('svg').first().boundingBox();

    expect(svgBox!.width).toBeLessThanOrEqual(hostBox!.width);
    expect(svgBox!.height).toBeLessThanOrEqual(hostBox!.height);
  });

});