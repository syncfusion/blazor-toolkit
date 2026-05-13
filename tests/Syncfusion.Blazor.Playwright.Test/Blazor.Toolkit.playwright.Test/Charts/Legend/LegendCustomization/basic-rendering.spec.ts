import { test, expect } from '@playwright/test';

test.describe('Chart – Legend Customization › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/legend/customization');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders correctly', async ({ page }) => {
    await expect(
      page.locator('h3:has-text("Chart – Legend Customization")')
    ).toBeVisible();
  });

  test('Chart container renders with expected dimensions', async ({ page }) => {
    const chartHost = page.locator('#chart-host');
    await expect(chartHost).toBeVisible();

    const box = await chartHost.boundingBox();
    expect(box).not.toBeNull();
    expect(box!.width).toBe(900);
    expect(box!.height).toBe(500);
  });

  test('Chart SVG is rendered inside container', async ({ page }) => {
    const svg = page.locator('#chart-host svg');
    await expect(svg).toBeVisible();
  });

  test('Chart title renders correctly', async ({ page }) => {
    await expect(
      page.locator('text=Olympic Medals')
    ).toBeVisible();
  });

});