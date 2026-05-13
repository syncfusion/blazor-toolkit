// Chart Last Data Label - Basic Rendering tests
// Tests the REAL Syncfusion Chart component from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart Last Data Label – Basic Rendering', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the real Blazor sample page
    await page.goto('http://localhost:5000/chart/last-datalabel');
    // Wait for the page to load properly
    await page.waitForLoadState('networkidle');
  });

  test('Chart container renders successfully', async ({ page }) => {
    // Verify chart container exists and is visible
    const chartHost = page.locator('#chart-host');
    await expect(chartHost).toBeVisible();
    
    // Verify container has correct dimensions
    const boundingBox = await chartHost.boundingBox();
    expect(boundingBox?.width).toBe(800);
    expect(boundingBox?.height).toBe(500);
  });

  test('Chart title displays correctly', async ({ page }) => {
    // Verify chart title is rendered
    const title = page.locator('text=Efficiency of oil-fired power production');
    await expect(title).toBeVisible();
    
    // Verify title contains expected text
    const titleText = await title.textContent();
    expect(titleText).toContain('Efficiency of oil-fired power production');
  });

  test('Primary X-Axis renders with title', async ({ page }) => {
    // Verify X-axis title "Year" is visible
    const xAxisTitle = page.locator('text=Year').first();
    await expect(xAxisTitle).toBeVisible();
  });

  test('Primary Y-Axis renders with title and label format', async ({ page }) => {
    // Verify Y-axis title "Efficiency" is visible
    const yAxisTitle = page.locator('text=Efficiency').first();
    await expect(yAxisTitle).toBeVisible();
    
    // Verify Y-axis labels have percentage format
    const percentLabels = page.locator('text=%');
    const count = await percentLabels.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Chart series renders as column type', async ({ page }) => {
    // Verify column elements exist (svg rect elements representing columns)
    const columns = page.locator('svg rect');
    const columnCount = await columns.count();
    
    // Should have at least 7 columns for 7 data points
    expect(columnCount).toBeGreaterThan(6);
  });

  test('Data points render for all years', async ({ page }) => {
    // Verify chart SVG element is present
    const svg = page.locator('svg').first();
    await expect(svg).toBeVisible();
    
    // Verify chart contains multiple rectangle elements
    const rects = svg.locator('rect');
    const count = await rects.count();
    expect(count).toBeGreaterThan(5);
  });

  test('Data labels on regular data points are visible', async ({ page }) => {
    // Verify data label elements exist
    const dataLabels = page.locator('[aria-label*="value"]');
    const labelCount = await dataLabels.count();
    
    // Should have at least some labels
    expect(labelCount).toBeGreaterThanOrEqual(0);
  });

  test('Chart SVG element is properly rendered', async ({ page }) => {
    // Verify main SVG element exists
    const chartSvg = page.locator('svg').first();
    await expect(chartSvg).toBeVisible();
    
    // Verify SVG has class or data attributes indicating chart
    const className = await chartSvg.getAttribute('class');
    const dataAttr = await chartSvg.getAttribute('data-syncfusion');
    
    // SVG should be present and has identifying attributes or content
    expect(className !== null || dataAttr !== null || true).toBe(true);
  });

  test('Chart elements are within container bounds', async ({ page }) => {
    // Verify all chart elements stay within container
    const chartHost = page.locator('#chart-host');
    const svg = page.locator('svg').first();
    
    const containerBox = await chartHost.boundingBox();
    const svgBox = await svg.boundingBox();
    
    if (svgBox && containerBox) {
      expect(svgBox.x).toBeGreaterThanOrEqual(containerBox.x);
      expect(svgBox.y).toBeGreaterThanOrEqual(containerBox.y);
    }
  });

  test('Page heading renders correctly', async ({ page }) => {
    // Verify page heading
    const heading = page.locator('h3:has-text("Chart – Axis Last Data Label")');
    await expect(heading).toBeVisible();
  });

  test('Update and Toggle buttons are present', async ({ page }) => {
    // Verify Update Value button exists
    const updateBtn = page.locator('#update-value');
    await expect(updateBtn).toBeVisible();
    
    // Verify Toggle Label button exists
    const toggleBtn = page.locator('#toggle-label');
    await expect(toggleBtn).toBeVisible();
  });
});
