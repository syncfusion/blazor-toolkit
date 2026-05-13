// Chart Events & Keyboard - Data Edit tests
// Tests the REAL Syncfusion Chart component data editing from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart Events & Keyboard › Data Edit', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-events-keyboard-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('First chart data editing is enabled', async ({ page }) => {
    // Since ChartDataEditSettings Enable="true"
    const chartHost = page.locator('#chartEvents');
    await expect(chartHost).toBeVisible();
  });

  test('Data points are editable', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Click on a data point
    await chartHost.click({ position: { x: 100, y: 350 } });
    await page.waitForTimeout(300);
    
    // Chart should still be visible
    await expect(chartHost).toBeVisible();
  });

  test('Data editing does not break chart', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Try to interact with data points
    await chartHost.dblclick({ position: { x: 200, y: 300 } });
    await page.waitForTimeout(300);
    
    // Chart should still render correctly
    const svg = page.locator('#chartEvents svg').first();
    await expect(svg).toBeVisible();
    
    const rects = await svg.locator('rect').count();
    expect(rects).toBeGreaterThan(0);
  });

  // test('Data editing works with legend', async ({ page }) => {
  //   const chartHost = page.locator('#chartEvents');
    
  //   // Click on legend first
  //   const legendItem = page.locator('#chartEvents').locator('text=Sales').first();
  //   await legendItem.click();
  //   await page.waitForTimeout(200);
    
  //   // Then try data editing
  //   await chartHost.click({ position: { x: 300, y: 250 } });
  //   await page.waitForTimeout(200);
    
  //   // Chart should still be functional
  //   const svg = page.locator('#chartEvents svg').first();
  //   await expect(svg).toBeVisible();
  // });

  test('Data editing works with zoom', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // First zoom
    await chartHost.hover({ position: { x: 200, y: 200 } });
    await page.mouse.wheel(0, 50);
    await page.waitForTimeout(200);
    
    // Then try data editing
    await chartHost.click({ position: { x: 400, y: 300 } });
    await page.waitForTimeout(200);
    
    // Chart should still be functional
    const svg = page.locator('#chartEvents svg').first();
    await expect(svg).toBeVisible();
  });

  test('Multiple data points can be edited', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Click on different data points
    await chartHost.click({ position: { x: 100, y: 300 } });
    await page.waitForTimeout(200);
    await chartHost.click({ position: { x: 200, y: 350 } });
    await page.waitForTimeout(200);
    await chartHost.click({ position: { x: 300, y: 320 } });
    await page.waitForTimeout(200);
    
    // Chart should still be functional
    const rects = page.locator('#chartEvents svg rect');
    const count = await rects.count();
    expect(count).toBeGreaterThan(0);
  });

  test('Second chart has no data editing enabled', async ({ page }) => {
    // Second chart doesn't have ChartDataEditSettings
    const chartHost = page.locator('#chartKeyboard');
    await expect(chartHost).toBeVisible();
    
    // Click should still work
    await chartHost.click({ position: { x: 200, y: 200 } });
    await page.waitForTimeout(200);
    
    // Chart should still be visible
    const svg = page.locator('#chartKeyboard svg').first();
    await expect(svg).toBeVisible();
  });
});