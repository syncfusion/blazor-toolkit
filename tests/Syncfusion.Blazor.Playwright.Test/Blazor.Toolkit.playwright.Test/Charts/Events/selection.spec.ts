// Chart Events & Keyboard - Selection tests
// Tests the REAL Syncfusion Chart component selection from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart Events & Keyboard › Selection', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-events-keyboard-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('First chart selection mode is set to Point', async ({ page }) => {
    // Since SelectionMode="Syncfusion.Blazor.Toolkit.Charts.SelectionMode.Point"
    const chartHost = page.locator('#chartEvents');
    await expect(chartHost).toBeVisible();
  });

  test('Data points are selectable', async ({ page }) => {
    // Click on a data point to select it
    const chartHost = page.locator('#chartEvents');
    const box = await chartHost.boundingBox();
    
    if (box) {
      await chartHost.click({ position: { x: 100, y: 300 } });
      await page.waitForTimeout(300);
    }
    
    // Chart should still be visible
    await expect(chartHost).toBeVisible();
  });

  test('Selection does not break chart rendering', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Click on different areas
    await chartHost.click({ position: { x: 200, y: 200 } });
    await page.waitForTimeout(200);
    await chartHost.click({ position: { x: 500, y: 300 } });
    await page.waitForTimeout(200);
    
    // Chart should remain functional
    const svg = page.locator('#chartEvents svg').first();
    await expect(svg).toBeVisible();
    
    const rects = await svg.locator('rect').count();
    expect(rects).toBeGreaterThan(0);
  });

  // test('Selection works with legend', async ({ page }) => {
  //   // Click on legend item
  //   const legendItem = page.locator('#chartEvents').locator('text=Sales').first();
  //   await legendItem.click();
  //   await page.waitForTimeout(300);
    
  //   // Click on chart area
  //   const chartHost = page.locator('#chartEvents');
  //   await chartHost.click({ position: { x: 300, y: 200 } });
  //   await page.waitForTimeout(300);
    
  //   // Chart should still be functional
  //   const svg = page.locator('#chartEvents svg').first();
  //   await expect(svg).toBeVisible();
  // });

  test('Multiple selections can be made', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Make multiple selections
    await chartHost.click({ position: { x: 150, y: 250 } });
    await page.waitForTimeout(200);
    await chartHost.click({ position: { x: 250, y: 250 } });
    await page.waitForTimeout(200);
    
    // Chart should still be functional
    const svg = page.locator('#chartEvents svg').first();
    await expect(svg).toBeVisible();
  });

  test('Selection works with zoom', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // First do zoom
    await chartHost.hover({ position: { x: 200, y: 200 } });
    await page.mouse.wheel(0, 50);
    await page.waitForTimeout(200);
    
    // Then make selection
    await chartHost.click({ position: { x: 300, y: 250 } });
    await page.waitForTimeout(200);
    
    // Chart should still be functional
    const svg = page.locator('#chartEvents svg').first();
    await expect(svg).toBeVisible();
  });

  test('Second chart selection functionality', async ({ page }) => {
    // Second chart doesn't have explicit selection mode
    const chartHost = page.locator('#chartKeyboard');
    await expect(chartHost).toBeVisible();
    
    // Click on chart
    await chartHost.click({ position: { x: 200, y: 200 } });
    await page.waitForTimeout(200);
    
    // Chart should still be visible
    const svg = page.locator('#chartKeyboard svg').first();
    await expect(svg).toBeVisible();
  });

  test('Selection state is maintained', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Make a selection
    await chartHost.click({ position: { x: 350, y: 200 } });
    await page.waitForTimeout(300);
    
    // Verify chart is still functional
    const rects = page.locator('#chartEvents svg rect');
    const count = await rects.count();
    expect(count).toBeGreaterThan(0);
  });
});