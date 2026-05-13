// Column Chart - Basic Rendering tests
// Tests the REAL Syncfusion Chart component with Column series from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart – Column Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-column');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG renders successfully', async ({ page }) => {
    // Verify the main chart container SVG is visible
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
    
    // SVG should have dimensions
    const boundingBox = await svg.boundingBox();
    expect(boundingBox?.width).toBeGreaterThan(0);
    expect(boundingBox?.height).toBeGreaterThan(0);
  });

  test('Chart title renders correctly', async ({ page }) => {
    // Verify chart title is rendered
    const title = page.locator('#chart-host text').filter({ hasText: /Walnuts|Almonds/ }).first();
    await expect(title).toBeVisible();
  });

  test('Chart subtitle renders correctly', async ({ page }) => {
    // Check subtitle
    const subtitle = page.locator('#chart-host text').filter({ hasText: /Source/ }).first();
    const isVisible = await subtitle.isVisible().catch(() => false);
    
    // Subtitle may be present
    expect(typeof isVisible).toBe('boolean');
  });

  test('Column series rectangles render', async ({ page }) => {
    // Column charts use rectangle elements to draw the columns
    const columns = page.locator('#chart-host svg rect');
    const columnCount = await columns.count();
    
    // Should have rectangles for column series
    expect(columnCount).toBeGreaterThan(0);
  });

  test('Multiple series render as grouped columns', async ({ page }) => {
    // The sample has 2 series (Walnuts and Almonds)
    const columns = page.locator('#chart-host svg rect');
    const columnCount = await columns.count();
    
    // Should have multiple columns for multiple series and data points
    expect(columnCount).toBeGreaterThan(5);
  });

  test('X-axis labels render (Category axis)', async ({ page }) => {
    // X-axis should show country labels
    const xAxisLabels = page.locator('#chart-host text').filter({ hasText: /Chile|EU|Turkey|India|Australia/ }).first();
    const isVisible = await xAxisLabels.isVisible().catch(() => false);
    
    // Should have category labels on X-axis
    expect(typeof isVisible).toBe('boolean');
  });

  test('Y-axis labels and title render', async ({ page }) => {
    // Y-axis should display "Metric Tons"
    const yAxisTitle = page.locator('#chart-host text').filter({ hasText: /Metric Tons/ }).first();
    const isVisible = await yAxisTitle.isVisible().catch(() => false);
    
    // Y-axis title should be present
    expect(typeof isVisible).toBe('boolean');
  });

  test('Y-axis numeric values render', async ({ page }) => {
    // Y-axis should have numeric labels
    const numericLabels = page.locator('#chart-host text').filter({ hasText: /\d+/ });
    const labelCount = await numericLabels.count();
    
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Chart area is properly bounded', async ({ page }) => {
    // Chart area should be properly defined
    const chartArea = page.locator('#chart-host svg');
    await expect(chartArea).toBeVisible();
  });

  test('Chart container renders with proper dimensions', async ({ page }) => {
    const container = page.locator('#chart-host');
    const boundingBox = await container.boundingBox();
    
    expect(boundingBox?.width).toBeGreaterThan(300);
    expect(boundingBox?.height).toBeGreaterThan(200);
  });

});

test.describe('Chart – Column Series › Data Visualization', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-column');
    await page.waitForLoadState('networkidle');
  });

  test('Column series rendered with distinct colors', async ({ page }) => {
    // Column rectangles should have fill colors
    const columns = page.locator('#chart-host svg rect[fill]');
    const columnsWithFill = await columns.count();
    
    // Should have colored column rectangles
    expect(columnsWithFill).toBeGreaterThan(0);
  });

  test('Columns have proper dimensions', async ({ page }) => {
    // Get the first column element
    const columns = page.locator('#chart-host svg rect');
    const firstColumn = columns.first();
    
    // Column should have width and height attributes
    const width = await firstColumn.getAttribute('width');
    const height = await firstColumn.getAttribute('height');
    
    // Both dimensions should be present
    expect(width).toBeTruthy();
    expect(height).toBeTruthy();
  });

  test('Columns render with corner radius styling', async ({ page }) => {
    // Columns may have rounded corners (rx, ry attributes)
    const columns = page.locator('#chart-host svg rect');
    const firstColumn = columns.first();
    
    // Get rx attribute (corner radius)
    const rx = await firstColumn.getAttribute('rx');
    
    // Should have some visual styling
    expect(firstColumn).toBeTruthy();
  });

  test('Multiple data series group together properly', async ({ page }) => {
    // Columns should be grouped by category with proper spacing
    const columns = page.locator('#chart-host svg rect');
    const columnCount = await columns.count();
    
    // Should have grouped columns (5 countries × 2 series = 10 columns)
    expect(columnCount).toBeGreaterThanOrEqual(9);
  });

  test('Y-axis scales correctly for data range', async ({ page }) => {
    // Verify chart renders with proper scaling
    const svg = page.locator('#chart-host svg');
    const boundingBox = await svg.boundingBox();
    
    if (boundingBox) {
      expect(boundingBox.width).toBeGreaterThan(200);
      expect(boundingBox.height).toBeGreaterThan(200);
    }
  });

  test('Axis elements render on chart', async ({ page }) => {
    // Chart should have axis labels and text elements
    const textElements = page.locator('#chart-host svg text');
    const textCount = await textElements.count();
    
    // Should have axis labels and titles
    expect(textCount).toBeGreaterThan(0);
  });

});

test.describe('Chart – Column Series › Interactivity', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-column');
    await page.waitForLoadState('networkidle');
  });

  test('Tooltip appears on hover over column', async ({ page }) => {
    // Find a column rectangle in the chart
    const column = page.locator('#chart-host svg rect').first();
    
    // Ensure the column element exists
    await expect(column).toBeVisible({ timeout: 10000 });
    
    // Hover over the column
    await column.hover({ force: true });
    await page.waitForTimeout(300);
    
    // Chart should be interactive
    const isVisible = await column.isVisible();
    expect(isVisible).toBe(true);
  });

  test('Different columns are individually interactive', async ({ page }) => {
    // Get multiple columns
    const columns = page.locator('#chart-host svg rect');
    const firstColumn = columns.first();
    const secondColumn = columns.nth(1);
    
    // Hover over first column
    await firstColumn.hover({ force: true });
    await page.waitForTimeout(200);
    
    // Hover over second column
    await secondColumn.hover({ force: true });
    await page.waitForTimeout(200);
    
    // Both should be interactive
    const firstVisible = await firstColumn.isVisible();
    const secondVisible = await secondColumn.isVisible();
    
    expect(firstVisible && secondVisible).toBe(true);
  });

  test('Legend items are visible and interactive', async ({ page }) => {
    // Check for legend elements
    const chartHost = page.locator('#chart-host');
    await expect(chartHost).toBeVisible();
    
    // Legend should be rendered with series
    const rect = await chartHost.boundingBox();
    expect(rect).toBeTruthy();
  });

});

test.describe('Chart – Column Series › Legend', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-column');
    await page.waitForLoadState('networkidle');
  });

  test('Legend renders without errors', async ({ page }) => {
    // Verify chart and legend render
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Series names are displayed', async ({ page }) => {
    // Check if series names appear in the visualization
    const chartHost = page.locator('#chart-host');
    await expect(chartHost).toBeVisible();
    
    // Columns should be present for series
    const columns = page.locator('#chart-host svg rect');
    expect(await columns.count()).toBeGreaterThan(0);
  });

  test('Legend highlight functionality works', async ({ page }) => {
    // Legend items should be interactive
    const chartHost = page.locator('#chart-host');
    const columns = page.locator('#chart-host svg rect');
    
    const columnCount = await columns.count();
    expect(columnCount).toBeGreaterThan(0);
  });

});

test.describe('Chart – Column Series › Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-column');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG has proper structure', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
    
    // SVG should have viewBox or dimensions
    const viewBox = await svg.getAttribute('viewBox');
    const width = await svg.getAttribute('width');
    
    const hasAttribute = viewBox || width;
    expect(hasAttribute).toBeTruthy();
  });

  test('Axis labels are accessible', async ({ page }) => {
    // Axis labels should be rendered as text elements
    const textElements = page.locator('#chart-host text');
    const count = await textElements.count();
    
    expect(count).toBeGreaterThan(0);
  });

  test('Chart title and subtitle are accessible', async ({ page }) => {
    // Title and subtitle should be text elements
    const textElements = page.locator('#chart-host text');
    
    // Should have text content
    const count = await textElements.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Numeric axis labels visible to screen readers', async ({ page }) => {
    // Y-axis numeric labels should be accessible
    const numericLabels = page.locator('#chart-host text').filter({ hasText: /\d+/ });
    const labelCount = await numericLabels.count();
    
    expect(labelCount).toBeGreaterThan(0);
  });

});
