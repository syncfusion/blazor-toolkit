import { test, expect } from '@playwright/test';

test.describe('Chart – Exponential Trendline › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/chart/exponential');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders', async ({ page }) => {
    await expect(
      page.locator('h3:has-text("Chart – Exponential Trendline")')
    ).toBeVisible();
  });

  test('Chart container renders with expected dimensions', async ({ page }) => {
    const host = page.locator('#chart-host');
    await expect(host).toBeVisible();

    const box = await host.boundingBox();
    expect(box?.width).toBe(850);
    expect(box?.height).toBe(600);
  });

  test('Chart SVG renders', async ({ page }) => {
    await expect(page.locator('#chart-host svg')).toBeVisible();
  });

  test('Chart title renders', async ({ page }) => {
    await expect(
      page.locator('text=Online trading')
    ).toBeVisible();
  });

});