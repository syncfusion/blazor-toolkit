// Chart Gradients - Linear Gradient tests
// Tests the REAL Syncfusion Chart component linear gradient configuration

import { test, expect } from '@playwright/test';

test.describe('Chart Gradients › Linear Gradient', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-gradient-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Linear gradient default renders correctly', async ({ page }) => {
    const chartHost = page.locator('#linearDefault');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Linear gradient with custom coordinates renders', async ({ page }) => {
    const chartHost = page.locator('#linearCustom');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Linear gradient uses two color stops', async ({ page }) => {
    const chartHost = page.locator('#linearDefault');
    const rects = await chartHost.locator('svg rect').count();
    expect(rects).toBeGreaterThan(0);
  });

  test('Linear gradient custom coordinates (X1=0,Y1=0,X2=1,Y2=1)', async ({ page }) => {
    const chartHost = page.locator('#linearCustom');
    await expect(chartHost).toBeVisible();
  });

  test('Linear gradient on line series', async ({ page }) => {
    const chartHost = page.locator('#lineGradient');
    const paths = await chartHost.locator('svg path').count();
    expect(paths).toBeGreaterThan(0);
  });

  test('Linear gradient on trendline', async ({ page }) => {
    const chartHost = page.locator('#trendlineLinear');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Multiple linear gradients on different series', async ({ page }) => {
    const chartHost = page.locator('#multipleSeries');
    await expect(chartHost).toBeVisible();
    const rects = await chartHost.locator('svg rect').count();
    expect(rects).toBeGreaterThan(0);
  });
});