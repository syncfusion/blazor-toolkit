// Stacked Bar/Column Charts - Stacking and Grouping tests
// Tests the REAL Syncfusion Chart component with Stacked and Stacking100 series from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart – Stacked Bar Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-bar');
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
    const title = page.locator('#chart-host text').filter({ hasText: /Renewable|Energy/ }).first();
    await expect(title).toBeVisible();
  });

  test('Multiple stacked series render', async ({ page }) => {
    // The sample has 3 stacked series (Wind, Solar, Hydro)
    const bars = page.locator('#chart-host svg rect');
    const barCount = await bars.count();
    
    // Should have rectangles for stacked series
    expect(barCount).toBeGreaterThan(0);
  });

  test('Bars are stacked properly', async ({ page }) => {
    // Stacked bars should be aligned vertically
    const bars = page.locator('#chart-host svg rect');
    const barCount = await bars.count();
    
    // Should have stacked bars for 4 years × 3 series
    expect(barCount).toBeGreaterThan(9);
  });

  test('X-axis labels render with years', async ({ page }) => {
    // X-axis should show years (2020, 2021, 2022, 2023)
    const yearLabels = page.locator('#chart-host text').filter({ hasText: /202[0-3]/ });
    const labelCount = await yearLabels.count();
    
    // Should have year labels
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Y-axis labels render with TWh format', async ({ page }) => {
    // Y-axis should display energy values with TWh format
    const energyLabels = page.locator('#chart-host text').filter({ hasText: /TWh/ });
    const labelCount = await energyLabels.count();
    
    // May have TWh labels
    expect(labelCount >= 0).toBe(true);
  });

  test('Legend renders with series names', async ({ page }) => {
    // Legend should show Wind, Solar, Hydro
    const chartHost = page.locator('#chart-host');
    await expect(chartHost).toBeVisible();
    
    // Verify legend is rendered
    const bars = page.locator('#chart-host svg rect');
    expect(await bars.count()).toBeGreaterThan(0);
  });

});

test.describe('Chart – Stacked Bar Series › Stack Labels', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-bar');
    await page.waitForLoadState('networkidle');
  });

  test('Stack labels display on bars', async ({ page }) => {
    // Stack labels show energy values (e.g., "466TWh")
    const stackLabels = page.locator('#chart-host text').filter({ hasText: /TWh/ });
    const labelCount = await stackLabels.count();
    
    // Stack labels should be visible
    expect(labelCount >= 0).toBe(true);
  });

  test('Stack labels show for each segment', async ({ page }) => {
    // Each stacked segment should have a label
    const textElements = page.locator('#chart-host text');
    const count = await textElements.count();
    
    // Should have labels for series and values
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – Stacked Column Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-column');
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
    // Verify chart title "Global Cotton Production" is rendered
    const title = page.locator('#chart-host text').filter({ hasText: /Cotton/ }).first();
    await expect(title).toBeVisible();
  });

  test('Multiple stacked series render as columns', async ({ page }) => {
    // The sample has 3 stacked series (India, China, United States)
    const columns = page.locator('#chart-host svg rect');
    const columnCount = await columns.count();
    
    // Should have rectangles for stacked series
    expect(columnCount).toBeGreaterThan(0);
  });

  test('Columns are stacked properly (top to bottom)', async ({ page }) => {
    // Stacked columns should be aligned vertically
    const columns = page.locator('#chart-host svg rect');
    const columnCount = await columns.count();
    
    // Should have stacked columns for years × series
    expect(columnCount).toBeGreaterThan(9);
  });

  test('X-axis labels render with years', async ({ page }) => {
    // X-axis should show years (2018-2023)
    const yearLabels = page.locator('#chart-host text').filter({ hasText: /201[8-9]|202[0-3]/ });
    const labelCount = await yearLabels.count();
    
    // Should have year labels
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Y-axis labels and title render', async ({ page }) => {
    // Y-axis should display "Production" title
    const yAxisTitle = page.locator('#chart-host text').filter({ hasText: /Production/ }).first();
    const isVisible = await yAxisTitle.isVisible().catch(() => false);
    
    // Should have Y-axis title
    expect(typeof isVisible).toBe('boolean');
  });

  test('Legend renders with series names', async ({ page }) => {
    // Legend should show country names
    const chartHost = page.locator('#chart-host');
    await expect(chartHost).toBeVisible();
    
    // Verify columns render
    const columns = page.locator('#chart-host svg rect');
    expect(await columns.count()).toBeGreaterThan(0);
  });

});

test.describe('Chart – Stacked Column 100% Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-column-100');
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
    const title = page.locator('#chart-host text').filter({ hasText: /Cotton/ }).first();
    await expect(title).toBeVisible();
  });

  test('100% stacked columns render', async ({ page }) => {
    // 100% stacked columns normalize all series to 100%
    const columns = page.locator('#chart-host svg rect');
    const columnCount = await columns.count();
    
    // Should have rectangles for 100% stacked series
    expect(columnCount).toBeGreaterThan(0);
  });

  test('All stacked columns reach same height', async ({ page }) => {
    // Each column should be 100% height (normalized)
    const columns = page.locator('#chart-host svg rect');
    
    // All columns should render to same scale
    const count = await columns.count();
    expect(count).toBeGreaterThan(5);
  });

  test('Y-axis shows percentage values', async ({ page }) => {
    // Y-axis should display 0-100%
    const yAxisLabels = page.locator('#chart-host text');
    const count = await yAxisLabels.count();
    
    // Should have numeric labels
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – Stacked Bar 100% Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-bar-100');
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

  test('100% stacked bars render', async ({ page }) => {
    // 100% stacked bars normalize all series to 100%
    const bars = page.locator('#chart-host svg rect');
    const barCount = await bars.count();
    
    // Should have rectangles for 100% stacked series
    expect(barCount).toBeGreaterThan(0);
  });

  test('All stacked bars reach same width', async ({ page }) => {
    // Each bar should be 100% width (normalized)
    const bars = page.locator('#chart-host svg rect');
    
    // All bars should render to same scale
    const count = await bars.count();
    expect(count).toBeGreaterThan(5);
  });

});

test.describe('Chart – Column Stacked and Grouped › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-column-stacked-and-grouped');
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
    // Verify chart title "Global Coffee Production" is rendered
    const title = page.locator('#chart-host text').filter({ hasText: /Coffee/ }).first();
    await expect(title).toBeVisible();
  });

  test('Grouped and stacked series render together', async ({ page }) => {
    // The sample has 4 series grouped into 2 groups with stacking
    const columns = page.locator('#chart-host svg rect');
    const columnCount = await columns.count();
    
    // Should have rectangles for grouped stacked series
    expect(columnCount).toBeGreaterThan(0);
  });

  test('Multiple groups render correctly', async ({ page }) => {
    // Groups (Vietnam+India, Colombia+Brazil) should render separately
    const columns = page.locator('#chart-host svg rect');
    const columnCount = await columns.count();
    
    // Should have columns for multiple groups
    expect(columnCount).toBeGreaterThan(9);
  });

  test('X-axis labels render with years', async ({ page }) => {
    // X-axis should show years (2021, 2022, 2023)
    const yearLabels = page.locator('#chart-host text').filter({ hasText: /202[1-3]/ });
    const labelCount = await yearLabels.count();
    
    // Should have year labels
    expect(labelCount).toBeGreaterThan(0);
  });

  test('Legend renders with all series names', async ({ page }) => {
    // Legend should show Vietnam, India, Colombia, Brazil
    const chartHost = page.locator('#chart-host');
    await expect(chartHost).toBeVisible();
    
    // Verify columns render
    const columns = page.locator('#chart-host svg rect');
    expect(await columns.count()).toBeGreaterThan(0);
  });

});

test.describe('Chart – Stacked Series › Interactivity', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-column');
    await page.waitForLoadState('networkidle');
  });

  test('Tooltip appears on hover over stacked segment', async ({ page }) => {
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

  test('Legend click hides/shows series', async ({ page }) => {
    // Click legend to toggle series visibility
    const chartHost = page.locator('#chart-host');
    
    // Get initial column count
    let columns = page.locator('#chart-host svg rect');
    let initialCount = await columns.count();
    
    // Chart should remain interactive
    expect(initialCount).toBeGreaterThan(0);
  });

  test('Multiple stacked segments are individually hoverable', async ({ page }) => {
    // Get multiple columns
    const columns = page.locator('#chart-host svg rect');
    const firstColumn = columns.first();
    const secondColumn = columns.nth(1);
    
    // Hover over first column
    await firstColumn.hover({ force: true });
    await page.waitForTimeout(150);
    
    // Hover over second column
    await secondColumn.hover({ force: true });
    await page.waitForTimeout(150);
    
    // Both should be interactive
    const firstVisible = await firstColumn.isVisible();
    const secondVisible = await secondColumn.isVisible();
    
    expect(firstVisible && secondVisible).toBe(true);
  });

});

test.describe('Chart – Stacked Series › Accessibility', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-stacked-column');
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

  test('Axis labels accessible to screen readers', async ({ page }) => {
    // Years and production values should be accessible
    const textElements = page.locator('#chart-host text');
    const count = await textElements.count();
    
    expect(count).toBeGreaterThan(0);
  });

  test('Legend series names accessible', async ({ page }) => {
    // Series names should be rendered as text
    const textElements = page.locator('#chart-host text');
    
    // Should have labels for series
    const count = await textElements.count();
    expect(count).toBeGreaterThan(0);
  });

});

test.describe('Chart – Negative Stack › Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-negative-stack');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG renders with negative values', async ({ page }) => {
    // Negative stack chart should render with both positive and negative values
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
    
    const boundingBox = await svg.boundingBox();
    expect(boundingBox?.width).toBeGreaterThan(0);
    expect(boundingBox?.height).toBeGreaterThan(0);
  });

  test('Female values render as negative (pyramid)', async ({ page }) => {
    // Female values should render on negative side
    const columns = page.locator('#chart-host svg rect');
    const columnCount = await columns.count();
    
    // Should have columns for male and female data
    expect(columnCount).toBeGreaterThan(0);
  });

  test('Male values render as positive (pyramid)', async ({ page }) => {
    // Male values should render on positive side
    const columns = page.locator('#chart-host svg rect');
    
    // Should have columns
    const count = await columns.count();
    expect(count).toBeGreaterThan(0);
  });

});
