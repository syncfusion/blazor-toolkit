// Chart Gradients - Series Types tests
// Tests the REAL Syncfusion Chart component different series types with gradients

import { test, expect } from '@playwright/test';

test.describe('Chart Gradients › Series Types', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-gradient-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Column series with gradient renders', async ({ page }) => {
    const chartHost = page.locator('#linearDefault');
    const rects = await chartHost.locator('svg rect').count();
    expect(rects).toBeGreaterThan(0);
  });

  test('Area series with gradient renders', async ({ page }) => {
    const chartHost = page.locator('#multipleSeries');
    const paths = await chartHost.locator('svg path').count();
    expect(paths).toBeGreaterThan(0);
  });

  test('Line series with gradient renders', async ({ page }) => {
    const chartHost = page.locator('#lineGradient');
    const paths = await chartHost.locator('svg path').count();
    expect(paths).toBeGreaterThan(0);
  });

  test('Multiple series with different gradients', async ({ page }) => {
    const chartHost = page.locator('#multipleSeries');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Trendline with linear gradient', async ({ page }) => {
    const chartHost = page.locator('#trendlineLinear');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Trendline with radial gradient', async ({ page }) => {
    const chartHost = page.locator('#trendlineRadial');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });
});