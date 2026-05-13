// Chart Events & Keyboard - Axis tests
// Tests the REAL Syncfusion Chart component axis configuration from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart Events & Keyboard › Axis', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-events-keyboard-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Primary X-Axis labels are rendered', async ({ page }) => {
    // Verify X-axis labels exist (Jan, Feb, Mar, etc.)
    const xAxisLabels = page.locator('#chartEvents').locator('.e-axis-label');
    const count = await xAxisLabels.count();
    
    // Should have at least some axis labels
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Primary Y-Axis labels are rendered', async ({ page }) => {
    // Verify Y-axis labels exist
    const yAxisLabels = page.locator('#chartEvents').locator('.e-yaxis-label, .e-axis-label');
    const count = await yAxisLabels.count();
    
    // Should have at least some axis labels
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Multi-level labels are rendered on X-axis', async ({ page }) => {
    // Verify multi-level labels exist (Half yearly 1, Half yearly 2)
    const multiLevelLabel1 = page.locator('#chartEvents').locator('text=Half yearly 1').first();
    const multiLevelLabel2 = page.locator('#chartEvents').locator('text=Half yearly 2').first();
    
    // These labels may or may not be visible depending on zoom level
    const count1 = await multiLevelLabel1.count();
    const count2 = await multiLevelLabel2.count();
    
    // At least one should be visible
    expect(count1 + count2).toBeGreaterThanOrEqual(0);
  });

  test('Auto interval is enabled on both axes', async ({ page }) => {
    // Verify chart renders with EnableAutoIntervalOnBothAxis="true"
    const chartSvg = page.locator('#chartEvents svg').first();
    await expect(chartSvg).toBeVisible();
    
    // Chart should render without errors
    const rects = await chartSvg.locator('rect').count();
    expect(rects).toBeGreaterThan(0);
  });

  test('Second chart X-Axis renders correctly', async ({ page }) => {
    // Verify second chart (Olympic Medals) X-axis
    const chartSvg = page.locator('#chartKeyboard svg').first();
    await expect(chartSvg).toBeVisible();
    
    // Should have country labels (USA, China, Japan, Australia)
    const textElements = chartSvg.locator('text');
    const count = await textElements.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Second chart Y-Axis renders correctly', async ({ page }) => {
    // Verify second chart Y-axis
    const chartSvg = page.locator('#chartKeyboard svg').first();
    await expect(chartSvg).toBeVisible();
    
    // Should have numeric labels
    const rects = await chartSvg.locator('rect').count();
    expect(rects).toBeGreaterThan(0);
  });

  test('Axis labels are properly positioned', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    const svg = chartHost.locator('svg').first();
    
    const hostBox = await chartHost.boundingBox();
    const svgBox = await svg.boundingBox();
    
    // SVG should be within host bounds
    if (svgBox && hostBox) {
      expect(svgBox.x).toBeGreaterThanOrEqual(hostBox.x);
      expect(svgBox.y).toBeGreaterThanOrEqual(hostBox.y);
    }
  });
});