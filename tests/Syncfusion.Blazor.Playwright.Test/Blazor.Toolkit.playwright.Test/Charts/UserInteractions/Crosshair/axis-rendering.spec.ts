import { test, expect } from '@playwright/test';

test.describe('Chart – Crosshair › Axis Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/crosshair');
    await page.waitForLoadState('networkidle');
  });

  test('Primary X-axis renders DateTime labels', async ({ page }) => {
    // DateTime axis labels are rendered as SVG text elements
    const axisLabels = page.locator('#chart-host svg text');

    await expect(axisLabels).toContainText([
      '2000',
      '2001',
      '2002',
      '2003'
    ]);
  });

  test('Primary Y-axis labels are rendered', async ({ page }) => {
    const axisLabels = page.locator('#chart-host svg text');

    // Numeric Y-axis values should be present
    await expect(axisLabels).toContainText([
      '2',
      '3',
      '4'
    ]);
  });

});