// Chart Gradients - Basic Rendering tests
// Tests the REAL Syncfusion Chart component with gradients from the Blazor sample app

import { test, expect } from '@playwright/test';

test.describe('Chart Gradients – Basic Rendering', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-gradient-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders correctly', async ({ page }) => {
    const heading = page.locator('h2:has-text("Chart Gradients")');
    await expect(heading).toBeVisible();
  });

  test('Linear Gradient default chart renders', async ({ page }) => {
    const chartHost = page.locator('#linearDefault');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Linear Gradient custom coordinates chart renders', async ({ page }) => {
    const chartHost = page.locator('#linearCustom');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Radial Gradient default chart renders', async ({ page }) => {
    const chartHost = page.locator('#radialDefault');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Radial Gradient custom chart renders', async ({ page }) => {
    const chartHost = page.locator('#radialCustom');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Gradient Stops chart renders', async ({ page }) => {
    const chartHost = page.locator('#gradientStops');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Multiple Series chart renders', async ({ page }) => {
    const chartHost = page.locator('#multipleSeries');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Line Series chart renders', async ({ page }) => {
    const chartHost = page.locator('#lineGradient');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Trendline Linear chart renders', async ({ page }) => {
    const chartHost = page.locator('#trendlineLinear');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Trendline Radial chart renders', async ({ page }) => {
    const chartHost = page.locator('#trendlineRadial');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Chart titles display correctly', async ({ page }) => {
    const title1 = page.locator('#linearDefault').locator('text=Linear Gradient').first();
    await expect(title1).toBeVisible();
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
});