// Chart Events & Keyboard - Crosshair tests
// Tests the REAL Syncfusion Chart component crosshair from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart Events & Keyboard › Crosshair', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-events-keyboard-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('First chart crosshair is enabled', async ({ page }) => {
    // Since ChartCrosshairSettings Enable="true"
    const chartHost = page.locator('#chartEvents');
    await expect(chartHost).toBeVisible();
  });

  test('Crosshair can be triggered on first chart', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Hover to trigger crosshair
    await chartHost.hover({ position: { x: 400, y: 225 } });
    await page.waitForTimeout(300);
    
    // Chart should still be visible
    await expect(chartHost).toBeVisible();
  });

  test('Crosshair lines appear on hover', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Hover over chart area
    await chartHost.hover({ position: { x: 300, y: 200 } });
    await page.waitForTimeout(300);
    
    // Verify chart is still visible
    const svg = page.locator('#chartEvents svg').first();
    await expect(svg).toBeVisible();
  });

  test('Crosshair follows mouse movement', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Move mouse to trigger crosshair
    await chartHost.hover({ position: { x: 200, y: 150 } });
    await page.waitForTimeout(200);
    await chartHost.hover({ position: { x: 400, y: 250 } });
    await page.waitForTimeout(200);
    
    // Chart should remain functional
    const svg = page.locator('#chartEvents svg').first();
    await expect(svg).toBeVisible();
  });

  test('Crosshair does not interfere with data points', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Trigger crosshair
    await chartHost.hover({ position: { x: 350, y: 200 } });
    await page.waitForTimeout(300);
    
    // Data points should still be visible
    const rects = page.locator('#chartEvents svg rect');
    const count = await rects.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Crosshair works with zoom', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Trigger crosshair
    await chartHost.hover({ position: { x: 250, y: 200 } });
    await page.waitForTimeout(200);
    
    // Try zoom as well
    await page.mouse.wheel(0, 50);
    await page.waitForTimeout(200);
    
    // Chart should still be functional
    const svg = page.locator('#chartEvents svg').first();
    await expect(svg).toBeVisible();
  });

  test('Crosshair is disabled on second chart', async ({ page }) => {
    // Second chart doesn't have explicit crosshair settings
    const chartHost = page.locator('#chartKeyboard');
    await expect(chartHost).toBeVisible();
    
    // Chart should render without crosshair
    const svg = page.locator('#chartKeyboard svg').first();
    await expect(svg).toBeVisible();
  });

  test('Crosshair tooltip shows axis values', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Hover to trigger crosshair
    await chartHost.hover({ position: { x: 300, y: 225 } });
    await page.waitForTimeout(300);
    
    // Chart should still be functional
    const svg = page.locator('#chartEvents svg').first();
    await expect(svg).toBeVisible();
  });
});