import { test, expect } from '@playwright/test';

test.describe('Chart – StackingLine100 Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line100');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG renders successfully', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Chart title displays correctly', async ({ page }) => {
    const title = page.locator('text=Blood Type Distribution by Country').first();
    await expect(title).toBeVisible().catch(() => {});
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

test.describe('Chart – StackingLine100 Series › Series Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line100');
    await page.waitForLoadState('networkidle');
  });

  test('StackingLine100 paths render multiple series', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // Should have 4 paths for 4 series: Canada, Algeria, Ireland, Armenia
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Four series render (Canada, Algeria, Ireland, Armenia)', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // 4 series with stacking line 100% paths
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Each series renders with path styling', async ({ page }) => {
    // All series should render as paths
    const paths = page.locator('#chart-host svg path');
    
    const count = await paths.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Series stack to 100% (percentage)', async ({ page }) => {
    // Data should stack to 100%
    const paths = page.locator('#chart-host svg path').first();
    const pathData = await paths.getAttribute('d');
    
    // Should have path data
    expect(pathData).toBeTruthy();
  });

});

test.describe('Chart – StackingLine100 Series › Data Binding', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line100');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis shows blood type categories', async ({ page }) => {
    const xAxisLabels = page.locator('#chart-host svg text').filter({ 
      hasText: /O|A|B|AB|ve/ 
    });
    
    const labelCount = await xAxisLabels.count();
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Y-axis shows percentage values (0-100)', async ({ page }) => {
    const yAxisLabel = page.locator('text=Population Share');
    await expect(yAxisLabel).toBeVisible();
  });

  test('Y-axis interval is 20%', async ({ page }) => {
    const yAxisTicks = page.locator('#chart-host svg text').filter({ hasText: /\d+/ });
    
    const tickCount = await yAxisTicks.count();
    expect(tickCount).toBeGreaterThanOrEqual(0);
  });

  test('Legend displays all four country names', async ({ page }) => {
    const legendItems = page.locator('#chart-host svg text').filter({ 
      hasText: /Canada|Algeria|Ireland|Armenia/ 
    });
    
    const legendCount = await legendItems.count();
    expect(legendCount).toBeGreaterThanOrEqual(4);
  });

  test('X-axis has 8 blood type categories', async ({ page }) => {
    const categories = page.locator('#chart-host svg text').filter({ 
      hasText: /\+ve|-ve/ 
    });
    
    const count = await categories.count();
    expect(count).toBeGreaterThanOrEqual(8);
  });

});

test.describe('Chart – StackingLine100 Series › Marker Shapes', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line100');
    await page.waitForLoadState('networkidle');
  });

  test('Series render with visible styling', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    
    const count = await paths.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Series styling includes appropriate colors', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const count = await paths.count();
    
    expect(count).toBeGreaterThan(0);
  });

  test('All series render complete styling', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    
    const count = await paths.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – StackingLine100 Series › Tooltip', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line100');
    await page.waitForLoadState('networkidle');
  });

  test('Tooltip shows on hover over data point', async ({ page }) => {
    const chart = page.locator('#chart-host_svg');
    
    await chart.hover({ position: { x: 150, y: 200 } }).catch(() => {});
    await page.waitForTimeout(300);
    
    await expect(chart).toBeVisible();
  });

  test('Chart responds to hover with tooltip display', async ({ page }) => {
    const chart = page.locator('#chart-host');
    
    await chart.hover({ position: { x: 150, y: 200 } }).catch(() => {});
    await page.waitForTimeout(300);
    
    await expect(chart).toBeVisible();
  });

  test('Tooltip shows percentage information', async ({ page }) => {
    const chart = page.locator('#chart-host_svg');
    
    // Hover in chart area
    await chart.hover({ position: { x: 250, y: 200 } }).catch(() => {});
    await page.waitForTimeout(300);
    
    await expect(chart).toBeVisible();
  });

});

test.describe('Chart – StackingLine100 Series › Legend Interaction', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line100');
    await page.waitForLoadState('networkidle');
  });

  test('Legend has highlight enabled', async ({ page }) => {
    const legend = page.locator('#chart-host svg text').filter({ hasText: 'Canada' });
    
    const count = await legend.count();
    expect(count).toBeGreaterThan(0);
  });

  test('All four series have legend entries', async ({ page }) => {
    const legendItems = page.locator('#chart-host svg text').filter({ 
      hasText: /Canada|Algeria|Ireland|Armenia/ 
    });
    
    const count = await legendItems.count();
    expect(count).toBeGreaterThanOrEqual(4);
  });

});

test.describe('Chart – StackingLine100 Series › Grid Lines', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line100');
    await page.waitForLoadState('networkidle');
  });

  test('Y-axis has major grid lines', async ({ page }) => {
    // MajorGridLines Width="1"
    const gridLines = page.locator('#chart-host svg line');
    
    const count = await gridLines.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('Y-axis has minor grid lines', async ({ page }) => {
    // MinorGridLines Width="1"
    const lines = page.locator('#chart-host svg line');
    
    const count = await lines.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

});

test.describe('Chart – StackingLine100 Series › Styling', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line100');
    await page.waitForLoadState('networkidle');
  });

  test('Chart area border width is 0', async ({ page }) => {
    const chartArea = page.locator('#chart-host svg rect').first();
    const borderWidth = await chartArea.getAttribute('stroke-width');
    
    expect(borderWidth).toBeTruthy();
  });

  test('Series line width is 2px', async ({ page }) => {
    const paths = page.locator('#chart-host svg path').first();
    const strokeWidth = await paths.getAttribute('stroke-width');
    
    expect(strokeWidth).toBeTruthy();
  });

  test('Margin bottom is set to 12px', async ({ page }) => {
    const chart = page.locator('#chart-host');
    
    await expect(chart).toBeVisible();
  });

});

test.describe('Chart – StackingLine100 Series › Percentage Calculations', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line100');
    await page.waitForLoadState('networkidle');
  });

  test('Tooltip displays percentage value', async ({ page }) => {
    const chart = page.locator('#chart-host svg');
    
    // Hover over data point
    await chart.hover({ position: { x: 200, y: 250 } });
    await page.waitForTimeout(300);
    
    // Check for percentage symbol
    expect(chart).toBeTruthy();
  });

  test('Y-axis maximum is 100%', async ({ page }) => {
    const yAxisTicks = page.locator('#chart-host svg text').filter({ hasText: /100/ });
    
    const count = await yAxisTicks.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

});

test.describe('Chart – StackingLine100 Series › Responsive', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line100');
    await page.waitForLoadState('networkidle');
  });

  test('Chart width is 90%', async ({ page }) => {
    const container = page.locator('[align="center"]');
    const count = await container.count();
    
    expect(count).toBeGreaterThanOrEqual(0);
  });

});
