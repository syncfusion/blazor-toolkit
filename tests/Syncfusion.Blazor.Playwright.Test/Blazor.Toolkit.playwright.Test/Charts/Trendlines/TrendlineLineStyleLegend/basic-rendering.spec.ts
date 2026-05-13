import { test, expect } from '@playwright/test';

test.describe('Chart – Trendline LineStyle & Legend › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/trendline-line-style-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders correctly', async ({ page }) => {
    await expect(
      page.locator('h2')
    ).toHaveText('Trendline – LineStyle & Legend (Playwright Sample)');
  });

  test('All charts render SVG output', async ({ page }) => {
    const svgs = page.locator('svg');
    expect(await svgs.count()).toBeGreaterThanOrEqual(3);
  });

  test('First chart SVG is visible', async ({ page }) => {
    const svg = page.locator('#trendLineStyleChart svg');
    await expect(svg).toBeVisible();
  });

});