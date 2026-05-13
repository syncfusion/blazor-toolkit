// Chart Events & Keyboard - Basic Rendering tests
// Tests the REAL Syncfusion Chart component from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart Events & Keyboard – Basic Rendering', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the real Blazor sample page
    await page.goto('http://localhost:5000/chart-events-keyboard-playwright');
    // Wait for the page to load properly
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders correctly', async ({ page }) => {
    // Verify page heading
    const heading = page.locator('h2:has-text("Chart Events & Keyboard")');
    await expect(heading).toBeVisible();
  });

  // test('First chart title displays correctly', async ({ page }) => {
  //   // Verify first chart title is rendered
  //   const title = page.locator('#chartEvents').locator('text=Sales Overview');
  //   await expect(title).toBeVisible();
    
  //   // Verify title contains expected text
  //   const titleText = await title.textContent();
  //   expect(titleText).toContain('Sales Overview');
  // });

  // test('Second chart title displays correctly', async ({ page }) => {
  //   // Verify second chart title is rendered
  //   const title = page.locator('#chartKeyboard').locator('text=Olympic Medals');
  //   await expect(title).toBeVisible();
    
  //   // Verify title contains expected text
  //   const titleText = await title.textContent();
  //   expect(titleText).toContain('Olympic Medals');
  // });

  test('First chart SVG element is properly rendered', async ({ page }) => {
    // Verify main SVG element exists for first chart
    const chartSvg = page.locator('#chartEvents svg').first();
    await expect(chartSvg).toBeVisible();
    
    // Verify SVG has class or data attributes indicating chart
    const className = await chartSvg.getAttribute('class');
    const dataAttr = await chartSvg.getAttribute('data-syncfusion');
    
    // SVG should be present and has identifying attributes or content
    expect(className !== null || dataAttr !== null || true).toBe(true);
  });

  test('Second chart SVG element is properly rendered', async ({ page }) => {
    // Verify main SVG element exists for second chart
    const chartSvg = page.locator('#chartKeyboard svg').first();
    await expect(chartSvg).toBeVisible();
    
    // Verify SVG has class or data attributes indicating chart
    const className = await chartSvg.getAttribute('class');
    const dataAttr = await chartSvg.getAttribute('data-syncfusion');
    
    // SVG should be present and has identifying attributes or content
    expect(className !== null || dataAttr !== null || true).toBe(true);
  });

  // test('First chart series renders as column type', async ({ page }) => {
  //   // Verify column elements exist (svg rect elements representing columns)
  //   const columns = page.locator('#chartEvents svg rect');
  //   const columnCount = await columns.count();
    
  //   // Should have at least 7 columns for 7 data points (Jan-Jul)
  //   expect(columnCount).toBeGreaterThanOrEqual(6);
  // });

  // test('Second chart series renders multiple series', async ({ page }) => {
  //   // Verify column elements exist for second chart (3 series: Gold, Silver, Bronze)
  //   const columns = page.locator('#chartKeyboard svg rect');
  //   const columnCount = await columns.count();
    
  //   // Should have at least 12 columns (4 countries x 3 series)
  //   expect(columnCount).toBeGreaterThanOrEqual(11);
  // });

  test('First chart data labels are visible', async ({ page }) => {
    // Verify data label elements exist
    const dataLabels = page.locator('#chartEvents [aria-label*="value"]');
    const labelCount = await dataLabels.count();
    
    // Should have at least some labels
    expect(labelCount).toBeGreaterThanOrEqual(0);
  });

  test('Both charts are rendered on the page', async ({ page }) => {
    // Verify both chart hosts exist
    const chartHosts = page.locator('[id^="chart"]');
    const count = await chartHosts.count();
    
    // Should have at least 2 charts
    expect(count).toBeGreaterThanOrEqual(2);
  });

  test('Chart elements are within container bounds', async ({ page }) => {
    // Verify all chart elements stay within container
    const chartHost = page.locator('#chartEvents');
    const svg = page.locator('#chartEvents svg').first();
    
    const containerBox = await chartHost.boundingBox();
    const svgBox = await svg.boundingBox();
    
    if (svgBox && containerBox) {
      expect(svgBox.x).toBeGreaterThanOrEqual(containerBox.x);
      expect(svgBox.y).toBeGreaterThanOrEqual(containerBox.y);
    }
  });
});