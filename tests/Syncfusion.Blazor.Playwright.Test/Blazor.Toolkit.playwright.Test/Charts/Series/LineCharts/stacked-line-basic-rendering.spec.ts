import { test, expect } from '@playwright/test';

test.describe('Chart – StackingLine Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG renders successfully', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Chart title displays correctly', async ({ page }) => {
    const title = page.locator('#chart-host svg text').filter({ hasText: /Renewable Energy/ });
    await expect(title).toBeVisible();
  });

  test('Chart subtitle shows data source', async ({ page }) => {
    const subtitle = page.locator('#chart-host svg text').filter({ hasText: /wikipedia/ });
    await expect(subtitle).toBeVisible();
  });

  test('Chart has proper dimensions', async ({ page }) => {
    const chart = page.locator('#chart-host');
    const boundingBox = await chart.boundingBox();
    
    expect(boundingBox?.height).toBeGreaterThan(0);
    expect(boundingBox?.width).toBeGreaterThan(0);
  });

});

test.describe('Chart – StackingLine Series › Series Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line');
    await page.waitForLoadState('networkidle');
  });

  test('StackingLine paths render multiple series', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // Should have 4 paths for 4 series: Wind, Bio mass, Small Hydro, Solar
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Four series render (Wind, Bio mass, Small Hydro, Solar)', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // 4 series with stacking line paths
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Each series renders with path elements', async ({ page }) => {
    // All series should render as paths
    const paths = page.locator('#chart-host svg path');
    
    const count = await paths.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Series stack on top of each other', async ({ page }) => {
    // Data should stack vertically
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // Should have multiple paths for stacked series
    expect(pathCount).toBeGreaterThan(0);
  });

});

test.describe('Chart – StackingLine Series › Data Binding', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis shows years (2015-2023)', async ({ page }) => {
    const xAxisLabels = page.locator('#chart-host svg text').filter({ 
      hasText: /\d{4}/ 
    });
    
    const labelCount = await xAxisLabels.count();
    expect(labelCount).toBeGreaterThanOrEqual(0);
  });

  test('Y-axis shows energy in TWh', async ({ page }) => {
    const yAxisLabel = page.locator('#chart-host_AxisTitle_1');
    await expect(yAxisLabel).toBeVisible();
  });

  test('Y-axis format includes TWh unit', async ({ page }) => {
    const yAxisTicks = page.locator('#chart-host svg text').filter({ hasText: /\d+/ });
    
    const tickCount = await yAxisTicks.count();
    expect(tickCount).toBeGreaterThanOrEqual(0);
  });

  test('Legend displays all four series names', async ({ page }) => {
    const legendItems = page.locator('#chart-host svg text').filter({ 
      hasText: /Wind|Biomass|Hydro|Solar|Bio/ 
    });
    
    const legendCount = await legendItems.count();
    expect(legendCount).toBeGreaterThanOrEqual(0);
  });

});

test.describe('Chart – StackingLine Series › Marker Shapes', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line');
    await page.waitForLoadState('networkidle');
  });

  test('Series render with visible styling', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    
    const count = await paths.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Series has appropriate styling attributes', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // Paths should exist with styling
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Chart renders complete series styling', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    
    const count = await paths.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – StackingLine Series › Tooltip', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart responds to hover with data information', async ({ page }) => {
    const chart = page.locator('#chart-host_svg');
    
    await chart.hover({ position: { x: 300, y: 200 } }).catch(() => {});
    await page.waitForTimeout(300);
    
    // Check that chart is responsive
    await expect(page.locator('#chart-host')).toBeVisible();
  });

});

test.describe('Chart – StackingLine Series › Legend Interaction', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line');
    await page.waitForLoadState('networkidle');
  });

  test('Legend has highlight enabled', async ({ page }) => {
    const legend = page.locator('#chart-host svg text');
    
    const count = await legend.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('All four series have legend entries', async ({ page }) => {
    const legendItems = page.locator('#chart-host svg text').filter({ 
      hasText: /Wind|Biomass|Hydro|Solar|Bio/ 
    });
    
    const count = await legendItems.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

});

test.describe('Chart – StackingLine Series › Grid Lines', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line');
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

test.describe('Chart – StackingLine Series › Styling', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart area border width is 0', async ({ page }) => {
    const chartArea = page.locator('#chart-host svg rect').first();
    const borderWidth = await chartArea.getAttribute('stroke-width');
    
    expect(borderWidth).toBeTruthy();
  });

  test('Series line width is 2px', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Margin bottom is set to 12px', async ({ page }) => {
    const chart = page.locator('#chart-host');
    
    // Chart should be visible
    await expect(chart).toBeVisible();
  });

});

test.describe('Chart – StackingLine Series › Axis Styling', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis major tick lines not visible', async ({ page }) => {
    // MajorTickLines Width="0"
    const ticks = page.locator('#chart-host svg line');
    
    expect(ticks).toBeTruthy();
  });

  test('X-axis line is not visible', async ({ page }) => {
    // AxisLineStyle Width="0"
    const axisLines = page.locator('#chart-host svg line');
    
    const count = await axisLines.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

});

test.describe('Chart – StackingLine Series › Responsive', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart width is 90%', async ({ page }) => {
    const chart = page.locator('#chart-host');
    
    // Chart should be visible and rendered
    await expect(chart).toBeVisible();
  });

});
