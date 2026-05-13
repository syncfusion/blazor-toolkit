import { test, expect } from '@playwright/test';

test.describe('Chart – Linear Trendline › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/linear-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders', async ({ page }) => {
    await expect(
      page.locator('h2:has-text("Linear Trendline – Playwright Converted Sample")')
    ).toBeVisible();
  });

  test('All charts render SVG output', async ({ page }) => {
    const svgs = page.locator('svg');
    expect(await svgs.count()).toBeGreaterThanOrEqual(10);
  });

});