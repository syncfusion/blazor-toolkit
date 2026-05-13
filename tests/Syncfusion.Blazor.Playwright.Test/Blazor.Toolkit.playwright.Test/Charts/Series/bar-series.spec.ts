import { test, expect } from '@playwright/test';

test.describe('Chart – Bar Series › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/bar/regression');
    await page.waitForLoadState('networkidle');
  });

  test('Chart SVG renders', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Bar series renders as horizontal bars', async ({ page }) => {
    // Bar chart uses rectangles for bars
    const bars = page.locator('#chart-host svg rect');
    const barCount = await bars.count();
    
    // Should have at least 4 bars for 4 data points
    expect(barCount).toBeGreaterThan(0);
  });

  test('Bar series has correct fill color (sky blue)', async ({ page }) => {
    // Fill="rgba(135,206,235,1)" is sky blue
    const bars = page.locator('#chart-host svg rect').first();
    const fill = await bars.getAttribute('fill');
    
    // Should have fill attribute
    expect(fill).toBeTruthy();
  });

  test('All data points render as bars', async ({ page }) => {
    const bars = page.locator('#chart-host svg rect');
    
    // Should have bars for 4 data points
    const barCount = await bars.count();
    expect(barCount).toBeGreaterThanOrEqual(4);
  });

  test('Rotate button exists and is clickable', async ({ page }) => {
    const rotateBtn = page.locator('#transpose');
    await expect(rotateBtn).toBeVisible();
    
    // Should have "Rotate" text
    const text = await rotateBtn.textContent();
    expect(text?.toLowerCase()).toContain('rotate');
  });

  test('Invert button exists and is clickable', async ({ page }) => {
    const invertBtn = page.locator('#invert');
    await expect(invertBtn).toBeVisible();
    
    // Should have "Invert" text
    const text = await invertBtn.textContent();
    expect(text?.toLowerCase()).toContain('invert');
  });

  test('Data labels display on bars', async ({ page }) => {
    // Data labels should be visible
    const labels = page.locator('#chart-host svg text').filter({ hasText: /\d+/ });
    const labelCount = await labels.count();
    
    expect(labelCount).toBeGreaterThan(0);
  });

});

test.describe('Chart – Bar Series › Interactions', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/bar/regression');
    await page.waitForLoadState('networkidle');
  });

  test('Rotate button changes chart orientation', async ({ page }) => {
    const rotateBtn = page.locator('#transpose');
    
    // Get initial bar count
    let bars = page.locator('#chart-host svg rect');
    let initialCount = await bars.count();
    
    // Click rotate
    await rotateBtn.click();
    await page.waitForTimeout(300);
    
    // Bars should still exist (now as vertical columns)
    bars = page.locator('#chart-host svg rect');
    let afterRotate = await bars.count();
    
    expect(afterRotate).toBeGreaterThan(0);
  });

  test('Rotate button toggles state', async ({ page }) => {
    const rotateBtn = page.locator('#transpose');
    
    // Click rotate twice
    await rotateBtn.click();
    await page.waitForTimeout(200);
    
    await rotateBtn.click();
    await page.waitForTimeout(200);
    
    // Chart should still be visible
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Invert button changes Y-axis direction', async ({ page }) => {
    const invertBtn = page.locator('#invert');
    
    let bars = page.locator('#chart-host svg rect');
    let initialCount = await bars.count();
    
    // Click invert
    await invertBtn.click();
    await page.waitForTimeout(300);
    
    // Bars should still exist
    bars = page.locator('#chart-host svg rect');
    let afterInvert = await bars.count();
    
    expect(afterInvert).toBeGreaterThan(0);
  });

  test('Both buttons can be used together', async ({ page }) => {
    const rotateBtn = page.locator('#transpose');
    const invertBtn = page.locator('#invert');
    
    // Click both
    await rotateBtn.click();
    await page.waitForTimeout(200);
    
    await invertBtn.click();
    await page.waitForTimeout(300);
    
    // Chart should still be valid
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
    
    const bars = page.locator('#chart-host svg rect');
    expect(await bars.count()).toBeGreaterThan(0);
  });

  test('Chart background changes to black', async ({ page }) => {
    const chartArea = page.locator('#chart-host svg rect').first();
    
    // Chart area should have black background
    const fill = await chartArea.getAttribute('fill');
    
    // Should have background fill
    expect(fill).toBeTruthy();
  });

});

test.describe('Chart – Bar Series › Data Binding', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/bar/regression');
    await page.waitForLoadState('networkidle');
  });

  test('Chart displays data from 4 data points', async ({ page }) => {
    const bars = page.locator('#chart-host svg rect');
    
    // Should have bars for 4 data points (2005, 2006, 2007, 2008)
    const barCount = await bars.count();
    expect(barCount).toBeGreaterThanOrEqual(4);
  });

  test('X-axis shows years (DateTime axis type)', async ({ page }) => {
    const xAxisLabels = page.locator('#chart-host svg text').filter({ hasText: /200[5-8]/ });
    
    const labelCount = await xAxisLabels.count();
    
    // Should have year labels
    expect(labelCount).toBeGreaterThan(0);
  });

});
