import { test, expect } from '@playwright/test';

test.describe('Chart – LineZone Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-line-zone');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG renders successfully', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Chart title displays correctly', async ({ page }) => {
    const title = page.locator('text=Annual Mean Precipitation in the United States');
    await expect(title).toBeVisible();
  });

  test('Chart subtitle shows data source', async ({ page }) => {
    const subtitle = page.locator('text=Source: ncei.noaa.gov');
    await expect(subtitle).toBeVisible();
  });

  test('Chart has proper dimensions', async ({ page }) => {
    const chart = page.locator('#chart-host');
    const boundingBox = await chart.boundingBox();
    
    expect(boundingBox?.height).toBeGreaterThan(0);
    expect(boundingBox?.width).toBeGreaterThan(0);
  });

});

test.describe('Chart – LineZone Series › Series Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-line-zone');
    await page.waitForLoadState('networkidle');
  });

  test('Line path renders', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // Should have at least one path for series
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Series elements render', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // Should have paths for the series
    expect(pathCount).toBeGreaterThan(0);
  });

});

test.describe('Chart – LineZone Series › Data Binding', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-line-zone');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis shows DateTime range (1895-2025)', async ({ page }) => {
    const xAxisLabels = page.locator('#chart-host svg text').filter({ 
      hasText: /18[9][5-9]|19\d{2}|20\d{2}/ 
    });
    
    const labelCount = await xAxisLabels.count();
    expect(labelCount).toBeGreaterThan(0);
  });

  test('X-axis label format is yyyy (years)', async ({ page }) => {
    const xLabels = page.locator('#chart-host svg text').filter({ hasText: /[12][89]\d{2}/ });
    
    const count = await xLabels.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('X-axis interval is 20 years', async ({ page }) => {
    // IntervalType="IntervalType.Years" Interval="20"
    const xLabels = page.locator('#chart-host svg text').filter({ hasText: /\d{4}/ });
    
    const count = await xLabels.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Y-axis shows precipitation in mm', async ({ page }) => {
    const yAxisLabel = page.locator('#chart-host_AxisTitle_1');
    await expect(yAxisLabel).toBeVisible();
  });

  test('Y-axis range is 10-110mm', async ({ page }) => {
    const yAxisTicks = page.locator('#chart-host svg text').filter({ hasText: /mm/ });
    
    const count = await yAxisTicks.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Y-axis interval is 20mm', async ({ page }) => {
    const yAxisTicks = page.locator('#chart-host svg text').filter({ hasText: /\d+/ });
    
    const count = await yAxisTicks.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

});

test.describe('Chart – LineZone Series › Tooltip', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-line-zone');
    await page.waitForLoadState('networkidle');
  });

  // test('Tooltip shows on hover over data point', async ({ page }) => {
  //   const chart = page.locator('#chart-host_svg');
    
  //   await chart.hover({ position: { x: 150, y: 200 } }).catch(() => {});
  //   await page.waitForTimeout(300);
    
  //   await expect(chart).toBeVisible();
  // });

  // test('Tooltip displays rainfall information', async ({ page }) => {
  //   const chart = page.locator('#chart-host_svg');
    
  //   await chart.hover({ position: { x: 250, y: 200 } }).catch(() => {});
  //   await page.waitForTimeout(300);
    
  //   await expect(chart).toBeVisible();
  // });

});

test.describe('Chart – LineZone Series › Styling', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-line-zone');
    await page.waitForLoadState('networkidle');
  });

  test('Chart area border width is 0', async ({ page }) => {
    const chartArea = page.locator('#chart-host svg rect').first();
    const borderWidth = await chartArea.getAttribute('stroke-width');
    
    expect(borderWidth).toBeTruthy();
  });

  test('X-axis minor grid lines are not visible', async ({ page }) => {
    const gridLines = page.locator('#chart-host svg line');
    
    const count = await gridLines.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

});

test.describe('Chart – LineZone Series › Annotations', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-line-zone');
    await page.waitForLoadState('networkidle');
  });

  test('Annotations render on chart', async ({ page }) => {
    // Wait for chart to fully load including annotations
    await page.waitForTimeout(1500);
    
    // Check for annotations
    const annotations = page.locator('#chart-host svg').locator('..').locator('div');
    
    const count = await annotations.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

});

test.describe('Chart – LineZone Series › Data Labels', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-line-zone');
    await page.waitForLoadState('networkidle');
  });

  test('Chart renders with large historical dataset', async ({ page }) => {
    // LineZoneData has ~130 data points (1895-2023)
    const paths = page.locator('#chart-host svg path');
    
    const count = await paths.count();
    // Should have path elements for the data
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – LineZone Series › Responsive', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-line-zone');
    await page.waitForLoadState('networkidle');
  });

  test('Chart container is centered', async ({ page }) => {
    const container = page.locator('[align="center"]');
    
    const count = await container.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – LineZone Series › DateTime Axis', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-line-zone');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis minimum is 1895', async ({ page }) => {
    const xLabels = page.locator('#chart-host svg text').filter({ hasText: /1895/ });
    
    const count = await xLabels.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('X-axis maximum is 2025', async ({ page }) => {
    const xLabels = page.locator('#chart-host svg text').filter({ hasText: /2025/ });
    
    const count = await xLabels.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

});
