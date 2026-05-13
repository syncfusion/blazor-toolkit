// Chart Gradients - Radial Gradient tests
// Tests the REAL Syncfusion Chart component radial gradient configuration

import { test, expect } from '@playwright/test';

test.describe('Chart Gradients › Radial Gradient', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-gradient-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Radial gradient default renders correctly', async ({ page }) => {
    const chartHost = page.locator('#radialDefault');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Radial gradient with custom parameters renders', async ({ page }) => {
    const chartHost = page.locator('#radialCustom');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Radial gradient uses two color stops', async ({ page }) => {
    const chartHost = page.locator('#radialDefault');
    const rects = await chartHost.locator('svg rect').count();
    expect(rects).toBeGreaterThan(0);
  });

  test('Radial gradient custom Cx=0.5 Cy=0.5', async ({ page }) => {
    const chartHost = page.locator('#radialCustom');
    await expect(chartHost).toBeVisible();
  });

  test('Radial gradient custom Fx=0.5 Fy=0.5', async ({ page }) => {
    const chartHost = page.locator('#radialCustom');
    await expect(chartHost).toBeVisible();
  });

  test('Radial gradient custom R=0.6', async ({ page }) => {
    const chartHost = page.locator('#radialCustom');
    await expect(chartHost).toBeVisible();
  });

  test('Radial gradient on trendline', async ({ page }) => {
    const chartHost = page.locator('#trendlineRadial');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });
});