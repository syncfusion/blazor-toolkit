import { test, expect } from '@playwright/test';

test.describe('Chart – Logarithmic Trendline › Basic Rendering', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5000/logarithmic-trendline-playwright');
    await page.waitForLoadState('networkidle');
  });

  test('Page heading renders', async ({ page }) => {
    await expect(
      page.locator('h2')
    ).toHaveText('Logarithmic Trendline – Playwright Sample');
  });

  test('All charts render SVG output', async ({ page }) => {
    const svgs = page.locator('svg');
    expect(await svgs.count()).toBeGreaterThanOrEqual(9);
  });

});