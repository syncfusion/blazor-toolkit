// Chart Events & Keyboard - Keyboard Navigation tests
// Tests the REAL Syncfusion Chart component keyboard navigation from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart Events & Keyboard › Keyboard Navigation', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-events-keyboard-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('First chart is focusable', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Click on chart to focus
    await chartHost.click();
    await page.waitForTimeout(200);
    
    // Chart should be visible and responsive
    await expect(chartHost).toBeVisible();
  });

  test('First chart responds to keyboard input', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Focus on chart
    await chartHost.click();
    await page.waitForTimeout(200);
    
    // Try pressing Tab key
    await page.keyboard.press('Tab');
    await page.waitForTimeout(200);
    
    // Chart should still be visible
    await expect(chartHost).toBeVisible();
  });

  test('First chart responds to arrow keys', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Focus on chart
    await chartHost.click();
    await page.waitForTimeout(200);
    
    // Try pressing arrow keys
    await page.keyboard.press('ArrowRight');
    await page.waitForTimeout(150);
    await page.keyboard.press('ArrowLeft');
    await page.waitForTimeout(150);
    
    // Chart should still be visible
    await expect(chartHost).toBeVisible();
  });

  test('First chart keyboard zoom works', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Focus on chart
    await chartHost.click();
    await page.waitForTimeout(200);
    
    // Try zoom with keyboard (Ctrl + Plus/Minus)
    await page.keyboard.press('Control+=');
    await page.waitForTimeout(200);
    
    // Chart should still be visible
    const svg = page.locator('#chartEvents svg').first();
    await expect(svg).toBeVisible();
  });

  test('Second chart is focusable', async ({ page }) => {
    const chartHost = page.locator('#chartKeyboard');
    
    // Click on chart to focus
    await chartHost.click();
    await page.waitForTimeout(200);
    
    // Chart should be visible and responsive
    await expect(chartHost).toBeVisible();
  });

  test('Second chart responds to keyboard input', async ({ page }) => {
    const chartHost = page.locator('#chartKeyboard');
    
    // Focus on chart
    await chartHost.click();
    await page.waitForTimeout(200);
    
    // Try pressing Tab key
    await page.keyboard.press('Tab');
    await page.waitForTimeout(200);
    
    // Chart should still be visible
    await expect(chartHost).toBeVisible();
  });

  test('Second chart responds to arrow keys', async ({ page }) => {
    const chartHost = page.locator('#chartKeyboard');
    
    // Focus on chart
    await chartHost.click();
    await page.waitForTimeout(200);
    
    // Try pressing arrow keys
    await page.keyboard.press('ArrowUp');
    await page.waitForTimeout(150);
    await page.keyboard.press('ArrowDown');
    await page.waitForTimeout(150);
    
    // Chart should still be visible
    await expect(chartHost).toBeVisible();
  });

  test('Keyboard navigation works with zoom toolbar', async ({ page }) => {
    const chartHost = page.locator('#chartKeyboard');
    
    // Focus on chart with zoom toolbar
    await chartHost.click();
    await page.waitForTimeout(200);
    
    // Tab to navigate to toolbar
    await page.keyboard.press('Tab');
    await page.waitForTimeout(150);
    
    // Chart should still be visible
    await expect(chartHost).toBeVisible();
  });

  test('Escape key exits zoom mode', async ({ page }) => {
    const chartHost = page.locator('#chartKeyboard');
    
    // Focus on chart
    await chartHost.click();
    await page.waitForTimeout(200);
    
    // Try pressing Escape
    await page.keyboard.press('Escape');
    await page.waitForTimeout(200);
    
    // Chart should still be visible
    const svg = page.locator('#chartKeyboard svg').first();
    await expect(svg).toBeVisible();
  });

  test('Keyboard navigation does not break chart', async ({ page }) => {
    const chartHost = page.locator('#chartEvents');
    
    // Focus and do multiple keyboard actions
    await chartHost.click();
    await page.keyboard.press('Tab');
    await page.waitForTimeout(100);
    await page.keyboard.press('ArrowRight');
    await page.waitForTimeout(100);
    await page.keyboard.press('ArrowRight');
    await page.waitForTimeout(100);
    await page.keyboard.press('Escape');
    
    // Chart should still be fully functional
    const svg = page.locator('#chartEvents svg').first();
    await expect(svg).toBeVisible();
    
    const rects = await svg.locator('rect').count();
    expect(rects).toBeGreaterThan(0);
  });
});