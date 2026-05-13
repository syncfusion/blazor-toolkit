import { test, expect } from '@playwright/test';

test.describe('Chart – Crosshair › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/crosshair');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders correctly', async ({ page }) => {
    await expect(
      page.locator('h3')
    ).toHaveText('Chart – Crosshair');
  });

  test('Chart container renders with expected size', async ({ page }) => {
    const host = page.locator('#chart-host');
    await expect(host).toBeVisible();

    const box = await host.boundingBox();
    expect(box?.width).toBe(800);
    expect(box?.height).toBe(450);
  });

  test('Chart SVG renders successfully', async ({ page }) => {
    await expect(page.locator('#chart-host svg')).toBeVisible();
  });

});
