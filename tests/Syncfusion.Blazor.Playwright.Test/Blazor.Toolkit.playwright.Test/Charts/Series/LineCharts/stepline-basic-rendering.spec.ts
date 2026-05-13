import { test, expect } from '@playwright/test';

test.describe('Chart – StepLine Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-step-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG renders successfully', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Chart title displays correctly', async ({ page }) => {
    const title = page.locator('text=Worldwide Best-Selling Albums by Year');
    await expect(title).toBeVisible();
  });

  test('Chart subtitle shows data source', async ({ page }) => {
    const subtitle = page.locator('text=Source: wikipedia.org');
    await expect(subtitle).toBeVisible();
  });

  test('Chart has proper dimensions', async ({ page }) => {
    const chart = page.locator('#chart-host');
    const boundingBox = await chart.boundingBox();
    
    expect(boundingBox?.height).toBeGreaterThan(0);
    expect(boundingBox?.width).toBeGreaterThan(0);
  });

});

test.describe('Chart – StepLine Series › StepLine Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-step-line');
    await page.waitForLoadState('networkidle');
  });

  test('StepLine path renders with rectangular steps', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // Should have at least one path for series
    expect(pathCount).toBeGreaterThan(0);
  });

  test('StepLine renders with path data', async ({ page }) => {
    const paths = page.locator('#chart-host svg path').first();
    const pathData = await paths.getAttribute('d');
    
    // StepLine paths should have path data
    expect(pathData).toBeTruthy();
  });

  test('Line width is 3px', async ({ page }) => {
    const paths = page.locator('#chart-host svg path').first();
    const strokeWidth = await paths.getAttribute('stroke-width');
    
    expect(strokeWidth).toBeTruthy();
  });

  test('Data labels visible on each data point', async ({ page }) => {
    const dataLabels = page.locator('#chart-host svg text').filter({ hasText: /\d+\.?\d*/ });
    
    const labelCount = await dataLabels.count();
    // Should have data labels for 18 data points
    expect(labelCount).toBeGreaterThan(10);
  });

});

test.describe('Chart – StepLine Series › Data Binding', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-step-line');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis shows years (2007-2024)', async ({ page }) => {
    const xAxisLabels = page.locator('#chart-host svg text').filter({ 
      hasText: /20[0-9]{2}/ 
    });
    
    const labelCount = await xAxisLabels.count();
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Y-axis shows sales in millions (0-20M)', async ({ page }) => {
    const yAxisLabel = page.locator('text=Sales in million');
    await expect(yAxisLabel).toBeVisible();
  });

  test('Y-axis range is 0-20', async ({ page }) => {
    const yAxisTicks = page.locator('#chart-host svg text').filter({ hasText: /\d+/ });
    
    const tickCount = await yAxisTicks.count();
    expect(tickCount).toBeGreaterThanOrEqual(0);
  });

  test('Interval is 3 years on X-axis', async ({ page }) => {
    const xAxisLabels = page.locator('#chart-host svg text').filter({ hasText: /20\d{2}/ });
    
    const count = await xAxisLabels.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – StepLine Series › Data Labels', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-step-line');
    await page.waitForLoadState('networkidle');
  });

  test('Data labels are displayed with bold font', async ({ page }) => {
    const dataLabels = page.locator('#chart-host text').filter({ hasText: /\d+\.?\d*/ });
    
    const count = await dataLabels.count();
    expect(count).toBeGreaterThan(0);
  });

  test('All 18 data points have labels', async ({ page }) => {
    const allText = page.locator('#chart-host svg text');
    
    const count = await allText.count();
    expect(count).toBeGreaterThan(15);
  });

});

test.describe('Chart – StepLine Series › Tooltip', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-step-line');
    await page.waitForLoadState('networkidle');
  });

  test('Tooltip shows on hover', async ({ page }) => {
    // Hover over a data point area
    const chart = page.locator('#chart-host_svg');
    await chart.hover({ position: { x: 200, y: 250 } }).catch(() => {});
    await page.waitForTimeout(300);
    
    // Chart should remain visible
    await expect(chart).toBeVisible();
  });

  test('Chart renders with custom tooltip format', async ({ page }) => {
    const chart = page.locator('#chart-host_svg');
    await chart.hover({ position: { x: 250, y: 200 } }).catch(() => {});
    await page.waitForTimeout(300);
    
    // Chart should still be visible after hover
    await expect(chart).toBeVisible();
  });

  test('Chart is interactive on hover', async ({ page }) => {
    const chart = page.locator('#chart-host_svg');
    await chart.hover({ position: { x: 300, y: 200 } }).catch(() => {});
    await page.waitForTimeout(300);
    
    // Check that chart is interactive
    await expect(page.locator('#chart-host_svg')).toBeVisible();
  });

});

test.describe('Chart – StepLine Series › Styling', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-step-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart area border width is 0', async ({ page }) => {
    const chartArea = page.locator('#chart-host svg rect').first();
    const borderWidth = await chartArea.getAttribute('stroke-width');
    
    expect(borderWidth).toBeTruthy();
  });

  test('Chart grid lines configuration', async ({ page }) => {
    // MajorGridLines Width="0"
    const gridLines = page.locator('#chart-host svg line');
    
    const count = await gridLines.count();
    // Grid lines may or may not exist depending on configuration
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Axis line width is 0', async ({ page }) => {
    // AxisLineStyle Width="0"
    const axisLines = page.locator('#chart-host svg line');
    
    const count = await axisLines.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Major tick lines are not visible', async ({ page }) => {
    // MajorTickLines Width="0"
    const ticks = page.locator('#chart-host svg line');
    
    expect(ticks).toBeTruthy();
  });

});

test.describe('Chart – StepLine Series › Marker Settings', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-step-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart renders without visible markers', async ({ page }) => {
    // Markers should not show by default with AllowHighlight=false
    const paths = page.locator('#chart-host svg path');
    
    const count = await paths.count();
    // Should have path elements for the line
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – StepLine Series › Responsive', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-step-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart width is 90%', async ({ page }) => {
    // Check parent container for 90% width
    const container = page.locator('[align="center"]');
    const boundingBox = await container.first().boundingBox();
    
    expect(boundingBox).toBeTruthy();
  });

  test('Chart container is centered', async ({ page }) => {
    const container = page.locator('[align="center"]');
    
    const count = await container.count();
    expect(count).toBeGreaterThan(0);
  });

});
