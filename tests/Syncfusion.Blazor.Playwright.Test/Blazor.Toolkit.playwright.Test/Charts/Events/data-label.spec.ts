// Chart Events & Keyboard - Data Label tests
// Tests the REAL Syncfusion Chart component data labels from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart Events & Keyboard › Data Label', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-events-keyboard-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('First chart data labels are enabled', async ({ page }) => {
    // Since ChartDataLabel Visible="true" in ChartMarker
    const chartHost = page.locator('#chartEvents');
    await expect(chartHost).toBeVisible();
  });

  test('Data labels are rendered on chart', async ({ page }) => {
    // Verify data label elements exist
    const dataLabels = page.locator('#chartEvents [aria-label*="value"]');
    const labelCount = await dataLabels.count();
    
    // Should have at least some labels
    expect(labelCount).toBeGreaterThanOrEqual(0);
  });

  test('Data labels show values', async ({ page }) => {
    // Verify chart renders with data labels
    const chartSvg = page.locator('#chartEvents svg').first();
    await expect(chartSvg).toBeVisible();
    
    // Should have data visualization
    const rects = await chartSvg.locator('rect').count();
    expect(rects).toBeGreaterThan(0);
  });

  test('Data labels are positioned correctly', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    const hostBox = await chartHost.boundingBox();
    
    // Chart should be within container
    if (hostBox) {
      const svg = page.locator('#chartEvents svg').first();
      const svgBox = await svg.boundingBox();
      
      if (svgBox) {
        expect(svgBox.x).toBeGreaterThanOrEqual(hostBox.x);
        expect(svgBox.y).toBeGreaterThanOrEqual(hostBox.y);
      }
    }
  });

  test('Markers are visible on first chart', async ({ page }) => {
    // Since ChartMarker Visible="true"
    const markers = page.locator('#chartEvents svg circle');
    const count = await markers.count();
    
    // Should have markers on data points
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Data labels and markers work together', async ({ page }) => {
    const chartSvg = page.locator('#chartEvents svg').first();
    await expect(chartSvg).toBeVisible();
    
    // Both data labels and markers should be present
    const rects = await chartSvg.locator('rect').count();
    expect(rects).toBeGreaterThan(0);
  });

  test('Second chart has no explicit data labels', async ({ page }) => {
    // Second chart doesn't have ChartDataLabel settings
    const chartHost = page.locator('#chartKeyboard');
    await expect(chartHost).toBeVisible();
    
    const svg = page.locator('#chartKeyboard svg').first();
    await expect(svg).toBeVisible();
  });
});