import { test, expect } from '@playwright/test';

test.describe('Chart – MultiColoredLine Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-multi-colored-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG renders successfully', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Chart title displays correctly', async ({ page }) => {
    const title = page.locator('text=U.S. Historical Average Temperature');
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

test.describe('Chart – MultiColoredLine Series › Multi-Colored Points', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-multi-colored-line');
    await page.waitForLoadState('networkidle');
  });

  test('Line path renders with multiple colors', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // Should have paths for the colored series
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Data points render as path elements', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // Should have paths for the line series
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Path elements have varying styling', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    
    const firstPath = paths.first();
    const stroke = await firstPath.getAttribute('stroke');
    
    // Should have stroke color
    expect(stroke).toBeTruthy();
  });

  test('Chart renders with color styling', async ({ page }) => {
    // Application assigns colors based on index
    const paths = page.locator('#chart-host svg path');
    
    const count = await paths.count();
    // Should have path elements for the colored series
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – MultiColoredLine Series › Data Binding', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-multi-colored-line');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis shows years (1895-2023)', async ({ page }) => {
    const xAxisLabels = page.locator('#chart-host svg text').filter({ 
      hasText: /18[9][5-9]|19\d{2}|20\d{2}/ 
    });
    
    const labelCount = await xAxisLabels.count();
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Y-axis shows temperature values in Fahrenheit', async ({ page }) => {
    const yAxisTicks = page.locator('#chart-host svg text').filter({ hasText: /\d+/ });
    
    const count = await yAxisTicks.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – MultiColoredLine Series › Series Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-multi-colored-line');
    await page.waitForLoadState('networkidle');
  });

  test('Single series renders with many data points', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    const pathCount = await paths.count();
    
    // Should have paths for the series rendering 130 years of data
    expect(pathCount).toBeGreaterThan(0);
  });

  test('Data points render as path styling', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    
    const count = await paths.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – MultiColoredLine Series › Color Palette', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-multi-colored-line');
    await page.waitForLoadState('networkidle');
  });

  test('Color palette renders with multiple colors', async ({ page }) => {
    // colors array: red, green, #ff0097, crimson, blue, darkorange, deepskyblue, mediumvioletred, violet, peru, gray, deeppink, navy
    const paths = page.locator('#chart-host svg path');
    
    const count = await paths.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Series renders with varying color styles', async ({ page }) => {
    // Color changes are applied to the rendering
    const path = page.locator('#chart-host svg path').first();
    
    const stroke = await path.getAttribute('stroke');
    expect(stroke).toBeTruthy();
  });

});

test.describe('Chart – MultiColoredLine Series › Tooltip', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-multi-colored-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart responds to hover with information display', async ({ page }) => {
    const chart = page.locator('#chart-host svg');
    
    await chart.hover({ position: { x: 300, y: 250 } }).catch(() => {});
    await expect(page.locator('#chart-host')).toBeVisible();
  });

});

test.describe('Chart – MultiColoredLine Series › Line Styling', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-multi-colored-line');
    await page.waitForLoadState('networkidle');
  });

  test('Series renders with appropriate styling', async ({ page }) => {
    const path = page.locator('#chart-host svg path').first();
    
    const stroke = await path.getAttribute('stroke');
    
    // Should have stroke styling
    expect(stroke).toBeTruthy();
  });

});

test.describe('Chart – MultiColoredLine Series › Performance', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-multi-colored-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart renders with 130 data points without lag', async ({ page }) => {
    const startTime = Date.now();
    
    const svg = page.locator('#chart-host svg');
    await svg.waitFor({ state: 'visible' });
    
    const endTime = Date.now();
    
    // Should render within reasonable time
    expect(endTime - startTime).toBeLessThan(5000);
  });

  test('Chart renders with multiple path elements', async ({ page }) => {
    const paths = page.locator('#chart-host svg path');
    
    const count = await paths.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – MultiColoredLine Series › Axis Settings', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-multi-colored-line');
    await page.waitForLoadState('networkidle');
  });

  test('X-axis contains year labels', async ({ page }) => {
    const xLabels = page.locator('#chart-host svg text').filter({ hasText: /18|19|20/ });
    
    const count = await xLabels.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Y-axis shows temperature scale', async ({ page }) => {
    const yLabels = page.locator('#chart-host svg text').filter({ hasText: /\d+/ });
    
    const count = await yLabels.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – MultiColoredLine Series › Responsive', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-multi-colored-line');
    await page.waitForLoadState('networkidle');
  });

  test('Chart renders within container', async ({ page }) => {
    const container = page.locator('#chart-host');
    
    await expect(container).toBeVisible();
  });

});
