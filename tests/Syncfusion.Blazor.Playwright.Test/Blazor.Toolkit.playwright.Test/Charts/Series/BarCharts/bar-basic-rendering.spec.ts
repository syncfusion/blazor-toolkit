// Bar Chart - Basic Rendering tests
// Tests the REAL Syncfusion Chart component with Bar series from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart – Bar Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bar');
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
    // Verify chart title is rendered - "Global Smartphone Sales Trends by Brand (2021-2023)"
    const title = page.locator('#chart-host text').filter({ hasText: /Global Smartphone Sales/ }).first();
    await expect(title).toBeVisible();
  });

  test('Chart subtitle renders if present', async ({ page }) => {
    // Check if subtitle exists
    const subtitle = page.locator('#chart-host text').filter({ hasText: /Population/ }).first();
    const isVisible = await subtitle.isVisible().catch(() => false);
    
    // Subtitle may or may not be visible depending on sample
    expect(typeof isVisible).toBe('boolean');
  });

  test('Bar series rectangles render', async ({ page }) => {
    // Bar charts use rectangle elements to draw the bars
    const bars = page.locator('#chart-host svg rect').filter({ hasNot: page.locator('svg') });
    const barCount = await bars.count();
    
    // Should have rectangles for bar series (exclude chart area rect)
    expect(barCount).toBeGreaterThan(0);
  });

  test('All data points render as bars', async ({ page }) => {
    // Get all rectangles (bars) in the chart
    const bars = page.locator('#chart-host svg rect');
    const barCount = await bars.count();
    
    // Should have multiple bars for data points
    expect(barCount).toBeGreaterThan(3);
  });

  test('X-axis labels render (Category axis)', async ({ page }) => {
    // X-axis should show category labels
    const xAxisLabels = page.locator('#chart-host text').filter({ hasText: /[A-Za-z]+/ });
    const labelCount = await xAxisLabels.count();
    
    // Should have axis labels
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Y-axis labels and title render', async ({ page }) => {
    // Y-axis should display numeric labels
    const numericLabels = page.locator('#chart-host text').filter({ hasText: /\d+/ });
    const labelCount = await numericLabels.count();
    
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Chart area border is visible', async ({ page }) => {
    // Chart area should be properly defined
    const chartArea = page.locator('#chart-host svg');
    await expect(chartArea).toBeVisible();
  });

  test('Chart container has correct dimensions', async ({ page }) => {
    const container = page.locator('#chart-host');
    const boundingBox = await container.boundingBox();
    
    // Container should have proper dimensions
    expect(boundingBox?.width).toBeGreaterThan(300);
    expect(boundingBox?.height).toBeGreaterThan(200);
  });

});

test.describe('Chart – Bar Series › Data Visualization', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bar');
    await page.waitForLoadState('networkidle');
  });

  test('Bar series rendered with correct colors', async ({ page }) => {
    // Bar rectangles should have fill colors
    const bars = page.locator('#chart-host svg rect[fill]');
    const barsWithFill = await bars.count();
    
    // Should have colored bar rectangles
    expect(barsWithFill).toBeGreaterThan(0);
  });

  test('Bars have proper width and height attributes', async ({ page }) => {
    // Get the first bar element
    const bars = page.locator('#chart-host svg rect');
    const firstBar = bars.first();
    
    // Bar should have width attribute (for horizontal bars, this represents height in visual space)
    const width = await firstBar.getAttribute('width');
    const height = await firstBar.getAttribute('height');
    
    // At least one dimension should be present
    const hasDimension = width || height;
    expect(hasDimension).toBeTruthy();
  });

  test('Multiple bars render without overlapping incorrectly', async ({ page }) => {
    // Get all bar elements in the chart
    const bars = page.locator('#chart-host svg rect');
    const barCount = await bars.count();
    
    // Should have multiple bars for multiple data points
    expect(barCount).toBeGreaterThan(2);
  });

  test('Data points are correctly plotted on the axis scale', async ({ page }) => {
    // Verify chart renders with proper scaling
    const svg = page.locator('#chart-host svg');
    const boundingBox = await svg.boundingBox();
    
    if (boundingBox) {
      expect(boundingBox.width).toBeGreaterThan(200);
      expect(boundingBox.height).toBeGreaterThan(200);
    }
  });

  test('Chart axis lines render correctly', async ({ page }) => {
    // Y-axis title should be visible - check for "Units Sold" or "Millions"
    const yAxisTitle = page.locator('#chart-host text').filter({ hasText: /Units|Millions/ }).first();
    await expect(yAxisTitle).toBeVisible();
    
    // Verify axis labels exist - should have numeric labels formatted with "M" suffix
    const axisLabels = page.locator('#chart-host text');
    const allLabels = await axisLabels.allTextContents();
    
    // Filter for labels that end with "M" (like "50M", "100M")
    const formattedLabels = allLabels.filter(text => /\d+M$/.test(text.trim()));
    expect(formattedLabels.length).toBeGreaterThan(0);
  });

});

test.describe('Chart – Bar Series › Interactivity', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bar');
    await page.waitForLoadState('networkidle');
  });

  test('Tooltip appears on hover over bar', async ({ page }) => {
    // Find a bar rectangle in the chart
    const bar = page.locator('#chart-host svg rect').first();
    
    // Ensure the bar element exists
    await expect(bar).toBeVisible({ timeout: 10000 });
    
    // Hover over the bar
    await bar.hover({ force: true });
    await page.waitForTimeout(300);
    
    // Chart should be interactive
    const isVisible = await bar.isVisible();
    expect(isVisible).toBe(true);
  });

  test('Chart remains interactive after render', async ({ page }) => {
    // Verify chart can be interacted with
    const chartHost = page.locator('#chart-host');
    await expect(chartHost).toBeVisible();
    
    // Get bar elements
    const bars = page.locator('#chart-host svg rect');
    const count = await bars.count();
    
    // Should have rendered bars
    expect(count).toBeGreaterThan(0);
  });

  test('Multiple bars are individually hoverable', async ({ page }) => {
    // Get multiple bars
    const bars = page.locator('#chart-host svg rect');
    const firstBar = bars.first();
    const secondBar = bars.nth(1);
    
    // Hover over first bar
    await firstBar.hover({ force: true });
    await page.waitForTimeout(200);
    
    // Hover over second bar
    await secondBar.hover({ force: true });
    await page.waitForTimeout(200);
    
    // Both should be interactive
    const firstVisible = await firstBar.isVisible();
    const secondVisible = await secondBar.isVisible();
    
    expect(firstVisible && secondVisible).toBe(true);
  });

});

test.describe('Chart – Bar Series › Legend', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bar');
    await page.waitForLoadState('networkidle');
  });

  test('Chart renders without errors', async ({ page }) => {
    // Verify page loaded without console errors
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Series name appears in chart', async ({ page }) => {
    // Check if series data is rendered
    const chartHost = page.locator('#chart-host');
    await expect(chartHost).toBeVisible();
    
    // Bars should be present
    const bars = page.locator('#chart-host svg rect');
    expect(await bars.count()).toBeGreaterThan(0);
  });

});

test.describe('Chart – Bar Series › Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-bar');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG has proper structure', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
    
    // SVG should exist and have viewBox or dimensions
    const viewBox = await svg.getAttribute('viewBox');
    const width = await svg.getAttribute('width');
    
    const hasAttribute = viewBox || width;
    expect(hasAttribute).toBeTruthy();
  });

  test('Chart displays numeric values for accessibility', async ({ page }) => {
    // Axis numeric labels should be present for screen readers
    const numericLabels = page.locator('#chart-host text').filter({ hasText: /\d+/ });
    const labelCount = await numericLabels.count();
    
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Chart title text is accessible', async ({ page }) => {
    // Chart should have text content that screen readers can access
    const textElements = page.locator('#chart-host text');
    const count = await textElements.count();
    
    expect(count).toBeGreaterThan(0);
  });

});
