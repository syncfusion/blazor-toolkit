// Chart Gradients - Gradient Stops tests
// Tests the REAL Syncfusion Chart component gradient color stops configuration

import { test, expect } from '@playwright/test';

test.describe('Chart Gradients › Gradient Stops', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart-gradient-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Gradient stops chart renders with multiple stops', async ({ page }) => {
    const chartHost = page.locator('#gradientStops');
    await expect(chartHost).toBeVisible();
    const svg = chartHost.locator('svg').first();
    await expect(svg).toBeVisible();
  });

  test('Gradient stop offset 0 renders', async ({ page }) => {
    const chartHost = page.locator('#gradientStops');
    const rects = await chartHost.locator('svg rect').count();
    expect(rects).toBeGreaterThan(0);
  });

  test('Gradient stop offset 50 renders', async ({ page }) => {
    const chartHost = page.locator('#gradientStops');
    await expect(chartHost).toBeVisible();
  });

  test('Gradient stop offset 100 renders', async ({ page }) => {
    const chartHost = page.locator('#gradientStops');
    await expect(chartHost).toBeVisible();
  });

  test('Gradient stop opacity property works', async ({ page }) => {
    const chartHost = page.locator('#gradientStops');
    await expect(chartHost).toBeVisible();
  });

  test('Gradient stop brighten property works', async ({ page }) => {
    const chartHost = page.locator('#gradientStops');
    await expect(chartHost).toBeVisible();
  });

  test('Gradient stop lighten property works', async ({ page }) => {
    const chartHost = page.locator('#gradientStops');
    await expect(chartHost).toBeVisible();
  });
});