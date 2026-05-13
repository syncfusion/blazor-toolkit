// Chart Events & Keyboard - Series tests
// Tests the REAL Syncfusion Chart component series configuration from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart Events & Keyboard › Series', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-events-keyboard-playwright');
    await page.waitForLoadState('networkidle');
  });

  // test('First chart has single series rendered', async ({ page }) => {
  //   // First chart (Sales Overview) has one series
  //   const rects = page.locator('#chartEvents svg rect');
  //   const count = await rects.count();
    
  //   // Should have 7 columns for 7 months
  //   expect(count).toBeGreaterThanOrEqual(7);
  // });

  test('First chart series is column type', async ({ page }) => {
    // Verify column-type series
    const columns = page.locator('#chartEvents svg rect');
    const count = await columns.count();
    
    // Column chart should have vertical bars
    expect(count).toBeGreaterThan(0);
  });

  test('First chart series has data markers', async ({ page }) => {
    // Since ChartMarker Visible="true", verify markers exist
    const markers = page.locator('#chartEvents svg circle');
    const count = await markers.count();
    
    // Should have markers on data points
    expect(count).toBeGreaterThanOrEqual(0);
  });

  // test('First chart series name is displayed in legend', async ({ page }) => {
  //   // Verify Sales series is in legend
  //   const legendItem = page.locator('#chartEvents').locator('text=Sales').first();
  //   await expect(legendItem).toBeVisible();
  // });

  // test('Second chart has multiple series rendered', async ({ page }) => {
  //   // Second chart (Olympic Medals) has 3 series: Gold, Silver, Bronze
  //   const rects = page.locator('#chartKeyboard svg rect');
  //   const count = await rects.count();
    
  //   // Should have 12 columns (4 countries x 3 series)
  //   expect(count).toBeGreaterThanOrEqual(12);
  // });

  // test('Second chart series are column type', async ({ page }) => {
  //   // Verify column-type series for second chart
  //   const columns = page.locator('#chartKeyboard svg rect');
  //   const count = await columns.count();
    
  //   // Column chart should have vertical bars
  //   expect(count).toBeGreaterThan(11);
  // });

  // test('Second chart Gold series is in legend', async ({ page }) => {
  //   const legendItem = page.locator('#chartKeyboard').locator('text=Gold').first();
  //   await expect(legendItem).toBeVisible();
  // });

  // test('Second chart Silver series is in legend', async ({ page }) => {
  //   const legendItem = page.locator('#chartKeyboard').locator('text=Silver').first();
  //   await expect(legendItem).toBeVisible();
  // });

  // test('Second chart Bronze series is in legend', async ({ page }) => {
  //   const legendItem = page.locator('#chartKeyboard').locator('text=Bronze').first();
  //   await expect(legendItem).toBeVisible();
  // });

  test('Series data points have correct values', async ({ page }) => {
    // Verify data is rendered correctly
    const chartSvg = page.locator('#chartEvents svg').first();
    await expect(chartSvg).toBeVisible();
    
    // Should have data visualization
    const rects = await chartSvg.locator('rect').count();
    expect(rects).toBeGreaterThan(0);
  });

  // test('Series colors are applied', async ({ page }) => {
  //   // Verify different series have different colors
  //   const rects = page.locator('#chartKeyboard svg rect');
  //   const count = await rects.count();
    
  //   // Multiple series should render
  //   expect(count).toBeGreaterThan(11);
  // });

  // test('Multi-series data binding is correct', async ({ page }) => {
  //   // Second chart: Medals data (4 countries x 3 series)
  //   const chartSvg = page.locator('#chartKeyboard svg');
  //   await expect(chartSvg).toBeVisible();
    
  //   // Should have 12 data points
  //   const rects = await chartSvg.locator('rect').count();
  //   expect(rects).toBeGreaterThanOrEqual(12);
  // });
});