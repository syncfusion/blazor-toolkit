import { test, expect } from '@playwright/test';

test.describe('Chart – Legend Template › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/legend/template');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders correctly', async ({ page }) => {
    await expect(
      page.locator('h3:has-text("Chart – Legend Template")')
    ).toBeVisible();
  });

  test('Chart container renders with expected size', async ({ page }) => {
    const host = page.locator('#chart-host');
    await expect(host).toBeVisible();

    const box = await host.boundingBox();
    expect(box?.width).toBe(900);
    expect(box?.height).toBe(500);
  });

  test('Chart SVG renders', async ({ page }) => {
    await expect(page.locator('#chart-host svg')).toBeVisible();
  });

  test('Chart title renders', async ({ page }) => {
    await expect(
      page.locator('text=Olympic Medals')
    ).toBeVisible();
  });

});