import { test, expect } from '@playwright/test';

test.describe('Chart – No Data Template › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/no-data-template');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders correctly', async ({ page }) => {
    await expect(
      page.locator('h3:has-text("Chart – No Data Template")')
    ).toBeVisible();
  });

  test('Chart container renders with correct size', async ({ page }) => {
    const host = page.locator('#chart-host');
    await expect(host).toBeVisible();

    const box = await host.boundingBox();
    expect(box?.width).toBe(800);
    expect(box?.height).toBe(450);
  });

  test('Chart SVG renders even with no data', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Load Data button is present', async ({ page }) => {
    await expect(page.locator('#load-data')).toBeVisible();
  });

});
