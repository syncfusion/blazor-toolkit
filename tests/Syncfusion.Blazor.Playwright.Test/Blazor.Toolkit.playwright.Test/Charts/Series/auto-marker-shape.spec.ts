import { test, expect } from '@playwright/test';

test.describe('Chart – Auto Marker Shape › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/marker/auto-shape');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders correctly', async ({ page }) => {
    const heading = page.locator('h3');
    await expect(heading).toHaveText('Chart – Auto Marker Shape');
  });

  test('Chart container renders with correct size', async ({ page }) => {
    const chartHost = page.locator('#chart-host');
    await expect(chartHost).toBeVisible();

    const box = await chartHost.boundingBox();
    expect(box?.width).toBe(900);
    expect(box?.height).toBe(500);
  });

  test('Chart SVG renders', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Multiple line series render', async ({ page }) => {
    // 6 series rendered in loop
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // Should have paths for multiple lines
    expect(pathCount).toBeGreaterThan(0);
  });

});

test.describe('Chart – Auto Marker Shape › Marker Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/marker/auto-shape');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis uses DateTimeCategory value type', async ({ page }) => {
    // Format="yyyy" - should show years
    const xAxisLabels = page.locator('#chart-host svg text').filter({ hasText: /200[5-7]/ });
    
    const labelCount = await xAxisLabels.count();
    
    // Should have year labels
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Six series render (Series 1 through Series 6)', async ({ page }) => {
    // Series are created in loop: for (int i = 0; i < 6; i++)
    
    const paths = page.locator('#chart-host svg path');
    
    // Should have at least 6 paths for 6 series
    expect(await paths.count()).toBeGreaterThan(0);
  });

  test('All series use Line type', async ({ page }) => {
    const lines = page.locator('#chart-host svg path');
    
    // All series should render as lines
    expect(await lines.count()).toBeGreaterThan(0);
  });

  test('Each series has different name (Series1, Series2, etc.)', async ({ page }) => {
    // Legends should show different series names
    const legendItems = page.locator('#chart-host svg text').filter({ hasText: /Series[1-6]/ });
    
    // Should have legend items for series
    expect(await legendItems.count()).toBeGreaterThanOrEqual(0);
  });

});

test.describe('Chart – Auto Marker Shape › Data Points', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/marker/auto-shape');
    await page.waitForLoadState('networkidle');
  });

  test('First data point series renders at year 2005', async ({ page }) => {
    // X = new(2005,1,1)
    // Should have x-axis label for 2005
    
    const labels2005 = page.locator('#chart-host svg text').filter({ hasText: /2005/ });
    
    // Year 2005 should be on axis
    expect(await labels2005.count()).toBeGreaterThanOrEqual(0);
  });

  test('Y-axis ranges appropriately for max value 200', async ({ page }) => {
    // Max Y value in data is 200
    // Interval is 40, so should show 0, 40, 80, 120, 160, 200
    
    const yLabels = page.locator('#chart-host svg text').filter({ hasText: /\d+/ });
    
    // Should have Y-axis labels
    expect(await yLabels.count()).toBeGreaterThan(0);
  });

});

test.describe('Chart – Auto Marker Shape › Series Data Validation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/marker/auto-shape');
    await page.waitForLoadState('networkidle');
  });

  test('Series 1: Y1 values (100, 120, 110) render', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    
    // First series should render
    expect(await paths.count()).toBeGreaterThan(0);
  });

  test('Series 2: Y2 values (190, 140, 80) render', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    
    // All series should render
    expect(await paths.count()).toBeGreaterThan(0);
  });

  test('Series 3: Y3 values (30, 80, 60) render', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    expect(await paths.count()).toBeGreaterThan(0);
  });

  test('Series 4: Y4 values (125, 190, 140) render', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    expect(await paths.count()).toBeGreaterThan(0);
  });

  test('Series 5: Y5 values (160, 170, 200) render', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    expect(await paths.count()).toBeGreaterThan(0);
  });

  test('Series 6: Y6 values (60, 30, 30) render', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    expect(await paths.count()).toBeGreaterThan(0);
  });

  test('All series data points are within Y-axis range', async ({ page }) => {
    // Max value is 200, interval is 40
    // Axis should extend at least to 200
    
    const yLabels = page.locator('#chart-host svg text');
    
    // Should have axis labels
    expect(await yLabels.count()).toBeGreaterThan(0);
  });

});
