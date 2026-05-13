import { test, expect } from '@playwright/test';

test.describe('Chart Marker API – Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/marker/api');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders', async ({ page }) => {
    await expect(
      page.locator('h3:has-text("Chart – Marker API")')
    ).toBeVisible();
  });

  test('Chart container renders with correct size', async ({ page }) => {
    const host = page.locator('#chart-host');
    await expect(host).toBeVisible();

    const box = await host.boundingBox();
    expect(box?.width).toBe(800);
    expect(box?.height).toBe(450);
  });

  test('Chart SVG is rendered', async ({ page }) => {
    const svg = page.locator('#chart-host >> svg').first();
    await expect(svg).toBeVisible();
  });

  test('Update Marker button exists', async ({ page }) => {
    await expect(page.locator('#update-marker')).toBeVisible();
  });

});