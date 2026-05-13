import { test, expect } from '@playwright/test';

test.describe('Chart – DashArray (Line Variants) › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/dasharray/line');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders', async ({ page }) => {
    await expect(
      page.locator('h3:has-text("Chart – DashArray (Line Variants)")')
    ).toBeVisible();
  });

  test('Chart container renders with expected size', async ({ page }) => {
    const host = page.locator('#chart-host');
    await expect(host).toBeVisible();

    const box = await host.boundingBox();
    expect(box?.width).toBe(900);
    expect(box?.height).toBe(500);
  });

  test('Chart SVG is rendered', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Chart title renders correctly', async ({ page }) => {
    await expect(
      page.locator('text=Fruits Production Statistics')
    ).toBeVisible();
  });

});